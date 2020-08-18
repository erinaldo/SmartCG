using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM11ZonaNOJerarq : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOZONNOJ";
               
        private bool nuevo;
        private string codigo;
        private string codigoZonaActual;

        private string nombreZona;
        private string codigoClaseZona;
        private string nombreClaseZona;

        private string etiquetaZonaNivel1;
        private int longZonaNivel1;

        private string autClaseElemento = "024";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        //private string autOperAlta = "20";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public string CodigoClaseZona
        {
            get
            {
                return (this.codigoClaseZona);
            }
            set
            {
                this.codigoClaseZona = value;
            }
        }

        public string NombreClaseZona
        {
            get
            {
                return (this.nombreClaseZona);
            }
            set
            {
                this.nombreClaseZona = value;
            }
        }

        public string NombreZona
        {
            get
            {
                return (this.nombreZona);
            }
            set
            {
                this.nombreZona = value;
            }
        }

        public frmMtoGLM11ZonaNOJerarq()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";
        }

        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        #region Eventos
        private void FrmMtoGLM11ZonaNOJerarq_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Zonas NO Jerárquicas Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC'
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            this.radTextBoxControlClaseZona.Text = this.nombreClaseZona;

            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                this.radButtonSave.Enabled = false;

                //Mostrar la información de la zona en los controles
                this.CargarInfoZona();

                this.ActiveControl = this.txtNivel1;
                this.txtNivel1.Select(0, 0);
                this.txtNivel1.Focus();
            }
            else
            {
                this.txtNombre.Text = this.nombreZona.Trim();
                this.txtNombre.Tag = this.txtNombre.Text;

                //Mostrar la información de la zona en los controles
                this.CargarInfoZona();

                //Actualizar el estado de la zona
                string estado = this.BuscarEstadoZona(this.codigo);
                if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                else this.radToggleSwitchEstadoActiva.Value = false;

                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigoClaseZona, autOperModifica);
                this.autEditar = true;
                if (!operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();

                    /*if (this.tgTexBoxSelGrupoCtas.Textbox.Text.Trim() != "")
                    {
                        //Verificar si el usuario tiene autorización a modificar el grupo de cuentas de auxiliar
                        string elemento = this.codigoTipoAux + this.codigoGrupoCuentas;

                        bool operarModificarGrupoCtas = aut.Validar(autClaseElementoGLT08, autGrupoGLT08, elemento, autOperModificaGLT08);
                        if (!operarModificarGrupoCtas) this.NoEditarCampos();
                        else
                        {
                            this.ActiveControl = this.txtNombreCuentaAux;
                            this.txtNombreCuentaAux.Select(0, 0);
                            this.txtNombreCuentaAux.Focus();
                        }
                    }
                    else
                    {
                        this.ActiveControl = this.txtNombreCuentaAux;
                        this.txtNombreCuentaAux.Select(0, 0);
                        this.txtNombreCuentaAux.Focus();
                    }*/
                }
            }
        }

        private void TxtNivel1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtNivel1_Leave(object sender, EventArgs e)
        {
            if (this.nuevo) this.NivelLeave(ref this.txtNivel1);
        }

        private void RadButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigoZonaActual
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonCancelar_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonCancelar);
        }

        private void RadButtonCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCancelar);
        }

        private void FrmMtoGLM11ZonaNOJerarq_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            //Chequear si existen modificaciones para grabar
            //if ((this.txtNombre.Tag != null && this.txtNombre.Text != this.txtNombre.Tag.ToString()) || this.txtNivel1.Text != this.txtNivel1.Tag.ToString())
            if (this.txtNivel1.Text.Trim() != this.txtNivel1.Tag.ToString() ||
                this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim())    
            {
                //Pedir confirmación de pérdida de los campos
                string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    this.radButtonSave.PerformClick();
                    e.Cancel = false;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    cerrarForm = false;
                }
                else e.Cancel = false;
            }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Zonas NO Jerárquicas Alta/Edita");
        }

        private void FrmMtoGLM11ZonaNOJerarq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonCancelar_Click(sender, null);
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM11TituloALta", "Mantenimiento de Zona - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM11TituloEdit", "Mantenimiento de Zona - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonExit.Text = this.LP.GetText("toolStripSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblClaseZona.Text = this.LP.GetText("lblGLM05ClaseZona", "Clase de zona");
            //this.lblEstado.Text = this.LP.GetText("lblEstado", "Estado");
        }
        
        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la zona (al dar de alta una zona)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNivel1.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La zona está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            this.radButtonSave.Enabled = false;

            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNivel1.Enabled = false;
            this.txtNombre.Enabled = false;
        }

        /// <summary>
        /// Rellena los controles con los datos de la zona (modo edición)
        /// </summary>
        private void CargarInfoZona()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 where ";
                query += "CLASZ0 = '" + this.codigoClaseZona + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.etiquetaZonaNivel1 = dr.GetValue(dr.GetOrdinal("NNV1Z0")).ToString().TrimEnd();

                    string estructura = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();
                    if (estructura.Length < 4) estructura.PadRight(4, '0');
                    
                    this.longZonaNivel1 = Convert.ToInt16(estructura.Substring(0, 1));
                 }
                 
                 dr.Close();

                 this.ActualizarInfoZona(1, this.longZonaNivel1, this.etiquetaZonaNivel1, ref this.lblNivel1, ref this.txtNivel1);

                 
                 if (!this.nuevo)
                 {
                     this.txtNivel1.Text = this.codigo.Trim();
                     this.txtNivel1.Tag = this.txtNivel1.Text;
                     this.txtNivel1.IsReadOnly = true;
                 }
                 else
                 {
                     this.txtNivel1.IsReadOnly = false;
                     this.txtNivel1.Enabled = true;
                     this.txtNivel1.Tag = "";
                     this.txtNombre.Tag = "";
                 }

                 // Actualiza el atributo TAG de los controles al valor actual de los controles
                 this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="longitudCampo"></param>
        /// <param name="valorEtiqueta"></param>
        /// <param name="ctrllblEtiqueta"></param>
        /// <param name="ctrltxtZona"></param>
        private void ActualizarInfoZona(int nivel, int longitudCampo, string valorEtiqueta, ref Telerik.WinControls.UI.RadLabel ctrllblEtiqueta, ref Telerik.WinControls.UI.RadTextBoxControl ctrltxtZona)
        {
            int ancho = 0;

            if (longitudCampo > 0)
            {
                if (valorEtiqueta != "") ctrllblEtiqueta.Text = valorEtiqueta;

                ctrltxtZona.MaxLength = longitudCampo;
                ancho = 40 + ((longitudCampo - 1) * 4);
                ctrltxtZona.Size = new Size(ancho, 30);
            }
            else
            {
                ctrllblEtiqueta.Visible = false;
                ctrltxtZona.Visible = false;
            }
        }

        /// <summary>
        /// Devuelve el estado de la zona actual
        /// </summary>
        /// <param name="codigoZonaActual"></param>
        /// <returns></returns>
        private string BuscarEstadoZona(string codigoZonaActual)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM11 where ";
                query += "CLASZ1 = '" + this.codigoClaseZona + "' and ZONAZ1 = '" + codigoZonaActual + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("STATZ1")).ToString().TrimEnd();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            return (result);
        }

        /// <summary>
        /// Dar de alta a una zona
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                this.codigoZonaActual = this.txtNivel1.Text;
                string tipo = "D";

                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                //Dar de alta a la zona la tabla del maestro de zonas (GLM11)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM11";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATZ1, CLASZ1, ZONAZ1, TIPOZ1, NOMBZ1) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigoClaseZona + "', '" + codigoZonaActual + "', ";
                query += "'" + tipo + "', '" + this.txtNombre.Text + "')";
                
                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM11", this.codigoClaseZona, codigoZonaActual);

                //this.nuevo = false;
                
                this.txtNombre.Tag = this.txtNombre.Text;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una zona
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                this.codigoZonaActual = this.txtNivel1.Text;

                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM11 set ";
                query += "STATZ1 = '" + estado + "', NOMBZ1 = '" + this.txtNombre.Text + "' ";
                query += "where CLASZ1 = '" + this.codigoClaseZona + "' and ZONAZ1 = '" + codigoZonaActual + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM11", this.codigoClaseZona, codigoZonaActual);

                this.txtNombre.Tag = this.txtNombre.Text;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            try
            {
                bool codZonaOk = true;
                if (this.nuevo) codZonaOk = this.CodigoZonaValido();    //Verificar que el codigo de zona no exista

                if (!codZonaOk)
                {
                    errores += "- El código de zona existe \n\r";      //Falta traducir
                    this.txtNivel1.Focus();
                }

                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.txtNombre.Focus();
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Valida que no exista el código de la zona
        /// </summary>
        /// <returns></returns>
        private bool CodigoZonaValido()
        {
            bool result = false;

            try
            {
                string codigoZonaActual = this.txtNivel1.Text;

                if (codigoZonaActual != "")
                {
                    string query = "select count(ZONAZ1) from " + GlobalVar.PrefijoTablaCG + "GLM11 ";
                    query += "where CLASZ1 = '" + this.codigoClaseZona + "' and ZONAZ1 = '" + codigoZonaActual + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Evento Leave de los controles TextBox de los niveles de zona
        /// Chequea que el código introducido sea válido
        /// </summary>
        /// <param name="ctrltxtNivel"></param>
        private void NivelLeave(ref Telerik.WinControls.UI.RadTextBoxControl ctrltxtNivel)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codZona = ctrltxtNivel.Text.Trim();

                if (codZona == "")
                {
                    //this.txtNombre.Enabled = false;
                    this.radButtonSave.Enabled = false;
                    ctrltxtNivel.Focus();

                    RadMessageBox.Show("Código de zona obligatorio", this.LP.GetText("errValCodZona", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codZonaOk = true;
                if (this.nuevo) codZonaOk = this.CodigoZonaValido();    //Verificar que el codigo de zona no exista

                if (codZonaOk)
                {
                    //this.txtNombre.Enabled = true;
                    ctrltxtNivel.IsReadOnly = true;

                    this.radButtonSave.Enabled = true;
                }
                else
                {
                    //this.txtNombre.Enabled = false;
                    this.radButtonSave.Enabled = false;
                    ctrltxtNivel.Focus();

                    RadMessageBox.Show("Código de zona ya existe", this.LP.GetText("errValCodZonaExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        /// <summary>
        /// Grabar una zona 
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        //this.codigo = this.txtCodigo.Text.Trim();
                    }
                }
                else result = this.ActualizarInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }
        
        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtNivel1.Tag = this.txtNivel1.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
        }
        #endregion

        private void frmMtoGLM11ZonaNOJerarq_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
