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
    public partial class frmMtoGLM11ZonaJerarq : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOZONJER";

        private bool nuevo;
        private string codigo;

        private string nombreZona;
        private string codigoClaseZona;
        private string nombreClaseZona;

        private string etiquetaZonaNivel1;
        private string etiquetaZonaNivel2;
        private string etiquetaZonaNivel3;
        private string etiquetaZonaNivel4;
        private int longZonaNivel1;
        private int longZonaNivel2;
        private int longZonaNivel3;
        private int longZonaNivel4;
        private int cantNiveles;
        private int nivelActual;

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

        public frmMtoGLM11ZonaJerarq()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM11ZonaJerarq_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Zonas Jerárquicas Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Ajustar el control de selección del tipo de auxiliar
            this.radTextBoxControlClaseZona.Text = this.nombreClaseZona;

            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);

                //Mostrar la información de la zona en los controles
                this.CargarInfoZona();

                this.ActiveControl = this.txtNivel1;
                this.txtNivel1.Select(0, 0);
                this.txtNivel1.Focus();
            }
            else
            {
                //Mostrar la información de la zona en los controles
                this.CargarInfoZona();

                //Actualizar el estado de la zona
                string estado = this.BuscarEstadoZona(this.codigo);
                if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                else this.radToggleSwitchEstadoActiva.Value = false;

                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigoClaseZona, autOperModifica);
                this.autEditar = operarModificar;
                if (!operarModificar) this.NoEditarCampos();
            }

            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
        }

        private void TxtNivel1_Leave(object sender, EventArgs e)
        {
            if (this.nuevo) this.NivelLeave(ref this.txtNivel1);
        }

        private void TxtNivel2_Leave(object sender, EventArgs e)
        {
            if (this.nuevo) this.NivelLeave(ref this.txtNivel2);
        }

        private void TxtNivel3_Leave(object sender, EventArgs e)
        {
            if (this.nuevo) this.NivelLeave(ref this.txtNivel3);
        }

        private void TxtNivel4_Leave(object sender, EventArgs e)
        {
            if (this.nuevo) this.NivelLeave(ref this.txtNivel4);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonNivelAnterior_Click(object sender, EventArgs e)
        {
            this.NivelAnterior();
        }

        private void RadButtonNivelSiguiente_Click(object sender, EventArgs e)
        {
            this.NivelSiguiente();
        }

        private void RadButtonNuevoNivel_Click(object sender, EventArgs e)
        {
            this.NuevoNivel();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadButtonNivelAnterior_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNivelAnterior);
        }

        private void RadButtonNivelAnterior_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNivelAnterior);
        }

        private void RadButtonNivelSiguiente_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNivelSiguiente);
        }

        private void RadButtonNivelSiguiente_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNivelSiguiente);
        }

        private void RadButtonNuevoNivel_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNuevoNivel);
        }

        private void RadButtonNuevoNivel_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNuevoNivel);
        }

        private void FrmMtoGLM11ZonaJerarq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLM11ZonaJerarq_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            //Chequear si existen modificaciones para grabar
            if (this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                this.ExistenCambiosGrabar())
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Zonas Jerárquicas Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM11TituloALta", "Mantenimiento de Zona - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM11TituloEdit", "Mantenimiento de Cuentas de Zona - Edición");           //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonNivelAnterior.Text = this.LP.GetText("toolStripHabNivelAnterior", "Nivel Anterior");
            this.radButtonNivelSiguiente.Text = this.LP.GetText("toolStripHabNivelSgte", "Nivel Siguiente");
            this.radButtonNuevoNivel.Text = this.LP.GetText("toolStripNuevoNivel", "Nuevo Nivel");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar"); 

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
            this.txtNivel2.Enabled = valor;
            this.txtNivel3.Enabled = valor;
            this.txtNivel4.Enabled = valor;
            this.txtNombre1.Enabled = valor;
            this.txtNombre2.Enabled = valor;
            this.txtNombre3.Enabled = valor;
            this.txtNombre4.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La zona está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);
            utiles.ButtonEnabled(ref this.radButtonNivelAnterior, false);
            utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
            utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
            
            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNivel1.Enabled = false;
            this.txtNivel2.Enabled = false;
            this.txtNivel3.Enabled = false;
            this.txtNivel4.Enabled = false;
            this.txtNombre1.Enabled = false;
            this.txtNombre2.Enabled = false;
            this.txtNombre3.Enabled = false;
            this.txtNombre4.Enabled = false;
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
                    this.etiquetaZonaNivel2 = dr.GetValue(dr.GetOrdinal("NNV2Z0")).ToString().TrimEnd();
                    this.etiquetaZonaNivel3 = dr.GetValue(dr.GetOrdinal("NNV3Z0")).ToString().TrimEnd();
                    this.etiquetaZonaNivel4 = dr.GetValue(dr.GetOrdinal("NNV4Z0")).ToString().TrimEnd();

                    string estructura = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();
                    if (estructura.Length < 4) estructura.PadRight(4, '0');
                    
                    this.longZonaNivel1 = Convert.ToInt16(estructura.Substring(0, 1));
                    this.longZonaNivel2 = Convert.ToInt16(estructura.Substring(1, 1));
                    this.longZonaNivel3 = Convert.ToInt16(estructura.Substring(2, 1));
                    this.longZonaNivel4 = Convert.ToInt16(estructura.Substring(3, 1));
                 }
                 
                 dr.Close();

                 this.cantNiveles = 0;

                 this.ActualizarInfoZona(1, this.longZonaNivel1, this.etiquetaZonaNivel1, ref this.lblNivel1, ref this.txtNivel1, ref this.txtNombre1);
                 this.ActualizarInfoZona(2, this.longZonaNivel2, this.etiquetaZonaNivel2, ref this.lblNivel2, ref this.txtNivel2, ref this.txtNombre2);
                 this.ActualizarInfoZona(3, this.longZonaNivel3, this.etiquetaZonaNivel3, ref this.lblNivel3, ref this.txtNivel3, ref this.txtNombre3);
                 this.ActualizarInfoZona(4, this.longZonaNivel4, this.etiquetaZonaNivel4, ref this.lblNivel4, ref this.txtNivel4, ref this.txtNombre4);

                 this.nivelActual = 1;
                 int longitudNivelActual = 0;

                 if (!this.nuevo)
                 {
                     this.txtNivel1.Text = this.codigo.Substring(0, this.longZonaNivel1).Trim();
                     this.txtNivel1.Tag = this.txtNivel1.Text;
                     if (this.txtNivel1.Text.Trim() != "")
                     {
                         this.txtNombre1.Text = this.BuscarNombreZona(this.txtNivel1.Text);
                         longitudNivelActual += this.longZonaNivel1;
                     }
                     this.txtNombre1.Tag = this.txtNombre1.Text;
                     
                     if (this.longZonaNivel1 != 0)
                     {
                         this.txtNivel2.Text = this.codigo.Substring(this.longZonaNivel1, this.longZonaNivel2).Trim();
                         this.txtNivel2.Tag = this.txtNivel2.Text;
                         if (this.txtNivel2.Text.Trim() != "")
                         {
                             this.txtNombre2.Text = this.BuscarNombreZona(this.txtNivel1.Text + this.txtNivel2.Text);
                             this.nivelActual++;
                             longitudNivelActual += this.longZonaNivel2;
                         }
                         this.txtNombre2.Tag = this.txtNombre2.Text;
                     }

                     if (this.longZonaNivel2 != 0)
                     {
                         this.txtNivel3.Text = this.codigo.Substring(this.longZonaNivel1 + this.longZonaNivel2, this.longZonaNivel3).Trim();
                         this.txtNivel3.Tag = this.txtNivel3.Text;
                         if (this.txtNivel3.Text.Trim() != "")
                         {
                             this.txtNombre3.Text = this.BuscarNombreZona(this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text);
                             this.nivelActual++;
                             longitudNivelActual += this.longZonaNivel3;
                         }
                         this.txtNombre3.Tag = this.txtNombre3.Text;
                     }

                     if (this.longZonaNivel3 != 0)
                     {
                         this.txtNivel4.Text = this.codigo.Substring(this.longZonaNivel1 + this.longZonaNivel2 + this.longZonaNivel3, this.longZonaNivel4).Trim();
                         this.txtNivel4.Tag = this.txtNivel4.Text;
                         if (this.txtNivel4.Text.Trim() != "")
                         {
                             this.txtNombre4.Text = this.BuscarNombreZona(this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text + this.txtNivel4.Text);
                             this.nivelActual++;
                             longitudNivelActual += this.longZonaNivel4;
                         }
                         this.txtNombre4.Tag = this.txtNombre4.Text;
                     }

                     string codigoZonaAux = "";
                     if (this.codigo != null) codigoZonaAux = this.codigo.TrimEnd();                     

                     if (longitudNivelActual > codigoZonaAux.Length)
                     {
                        utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, true);
                         //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgte;
                     }
                     else
                     {
                        utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                         //this.radButtonNivelSiguienteImage = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
                     }

                     if (this.longZonaNivel1 != codigoZonaAux.Length)
                     {
                        utiles.ButtonEnabled(ref this.radButtonNivelAnterior, true);
                         //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAnt;
                     }
                     else
                     {
                        utiles.ButtonEnabled(ref this.radButtonNivelAnterior, false);
                         //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAntNoAct;
                     }

                     int longTotal = this.longZonaNivel1 + this.longZonaNivel2 + this.longZonaNivel3 + this.longZonaNivel4;
                     if (longTotal > codigoZonaAux.Length)
                     {
                        utiles.ButtonEnabled(ref this.radButtonNuevoNivel, true);
                         //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionar;
                     }
                     else
                     {
                        utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
                         //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionarNoAct;
                     }

                     //Campos Zonas de solo lectura
                     this.txtNivel1.IsReadOnly = true;
                     this.txtNivel2.IsReadOnly = true;
                     this.txtNivel3.IsReadOnly = true;
                     this.txtNivel4.IsReadOnly = true;

                     //Activa para edición el nivel actual
                     this.ActivarEdicionNivelActual();
                 }
                 else
                 {
                     utiles.ButtonEnabled(ref this.radButtonNivelAnterior, false);
                    //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAntNoAct;

                     utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                    //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;

                     utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
                     //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionarNoAct;
                     
                     this.txtNivel1.IsReadOnly = false;
                     this.txtNivel1.Enabled = true;
                     this.txtNivel2.IsReadOnly = true;
                     this.txtNivel3.IsReadOnly = true;
                     this.txtNivel4.IsReadOnly = true;
                     this.txtNombre1.IsReadOnly = false;
                     this.txtNombre1.Enabled = true;
                     this.txtNombre2.IsReadOnly = true;
                     this.txtNombre3.IsReadOnly = true;
                     this.txtNombre4.IsReadOnly = true;
                 }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Activa para edición el nivel actual
        /// </summary>
        private void ActivarEdicionNivelActual()
        {
            switch (this.nivelActual)
            {
                case 1:
                    this.txtNivel1.IsReadOnly = true;
                    this.txtNombre1.IsReadOnly = false;
                    this.txtNivel2.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = true;
                    this.txtNivel3.IsReadOnly = true;
                    this.txtNombre3.IsReadOnly = true;
                    this.txtNivel4.IsReadOnly = true;
                    this.txtNombre4.IsReadOnly = true;
                    this.ActiveControl = this.txtNombre1;
                    this.txtNombre1.Select(0, 0);
                    this.txtNombre1.Focus();
                    break;
                case 2:
                    this.txtNivel1.IsReadOnly = true;
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNivel2.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = false;
                    this.txtNivel3.IsReadOnly = true;
                    this.txtNombre3.IsReadOnly = true;
                    this.txtNivel4.IsReadOnly = true;
                    this.txtNombre4.IsReadOnly = true;
                    this.ActiveControl = this.txtNombre2;
                    this.txtNombre2.Select(0, 0);
                    this.txtNombre2.Focus();
                    break;
                case 3:
                    this.txtNivel1.IsReadOnly = true;
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNivel2.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = true;
                    this.txtNivel3.IsReadOnly = true;
                    this.txtNombre3.IsReadOnly = false;
                    this.txtNivel4.IsReadOnly = true;
                    this.txtNombre4.IsReadOnly = true;
                    this.ActiveControl = this.txtNombre3;
                    this.txtNombre3.Select(0, 0);
                    this.txtNombre3.Focus();
                    break;
                case 4:
                    this.txtNivel1.IsReadOnly = true;
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNivel2.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = true;
                    this.txtNivel3.IsReadOnly = true;
                    this.txtNombre3.IsReadOnly = true;
                    this.txtNivel4.IsReadOnly = true;
                    this.txtNombre4.IsReadOnly = false;
                    this.ActiveControl = this.txtNombre4;
                    this.txtNombre4.Select(0, 0);
                    this.txtNombre4.Focus();
                    break;
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
        private void ActualizarInfoZona(int nivel, int longitudCampo, string valorEtiqueta, ref Telerik.WinControls.UI.RadLabel ctrllblEtiqueta, ref Telerik.WinControls.UI.RadTextBoxControl ctrltxtZona, ref Telerik.WinControls.UI.RadTextBoxControl ctrltxtNombre)
        {
            int ancho = 0;

            if (longitudCampo > 0)
            {
                if (valorEtiqueta != "") ctrllblEtiqueta.Text = valorEtiqueta;

                ctrltxtZona.MaxLength = longitudCampo;
                ancho = 40 + ((longitudCampo - 1) * 4);
                ctrltxtZona.Size = new Size(ancho, 30);

                this.cantNiveles++;
            }
            else
            {
                ctrllblEtiqueta.Visible = false;
                ctrltxtZona.Visible = false;
                ctrltxtNombre.Visible = false;
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
        /// 
        /// </summary>
        /// <param name="codigoZonaActual"></param>
        /// <returns></returns>
        private string BuscarNombreZona(string codigoZonaActual)
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
                    result = dr.GetValue(dr.GetOrdinal("NOMBZ1")).ToString().TrimEnd();
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
        private string AltaInfo(string nombreNivelActual)
        {
            string result = "";
            try
            {
                string codigoZonaActual = this.CodigoNivelActual();
                string tipo = (this.cantNiveles == this.nivelActual) ? "D" : "T";

                string estado = this.BuscarEstadoZona(this.codigo);
                if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                else this.radToggleSwitchEstadoActiva.Value = false;

                //Dar de alta a la zona la tabla del maestro de zonas (GLM11)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM11";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATZ1, CLASZ1, ZONAZ1, TIPOZ1, NOMBZ1) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigoClaseZona + "', '" + codigoZonaActual + "', ";
                query += "'" + tipo + "', '" + nombreNivelActual + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM11", this.codigoClaseZona, codigoZonaActual);

                //this.nuevo = false;

                int longitudNivelActual = 0;
                if (this.txtNivel1.Text.Trim() != "") longitudNivelActual = this.longZonaNivel1;
                if (this.txtNivel2.Text.Trim() != "") longitudNivelActual += this.longZonaNivel2;
                if (this.txtNivel3.Text.Trim() != "") longitudNivelActual += this.longZonaNivel3;
                if (this.txtNivel4.Text.Trim() != "") longitudNivelActual += this.longZonaNivel4;

                if (longitudNivelActual > codigoZonaActual.Length)
                {
                    utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, true);
                    //this.toolStripButtonNivelSgte.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgte;
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                    //this.toolStripButtonNivelSgte.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
                }

                int longTotal = this.longZonaNivel1 + this.longZonaNivel2 + this.longZonaNivel3 + this.longZonaNivel4;
                if (longTotal > codigoZonaActual.Length)
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevoNivel, true);
                    //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionar;
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
                    //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionarNoAct;
                }

                //Actualiza el tag del control correspondiente
                this.ActualizaTagNivelActual(nombreNivelActual);
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
        private string ActualizarInfo(string nombreNivelActual)
        {
            string result = "";
            try
            {
                string codigoZonaActual = this.CodigoNivelActual();

                string estado = this.BuscarEstadoZona(this.codigo);
                if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                else this.radToggleSwitchEstadoActiva.Value = false;

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM11 set ";
                query += "STATZ1 = '" + estado + "', NOMBZ1 = '" + nombreNivelActual + "' ";
                query += "where CLASZ1 = '" + this.codigoClaseZona + "' and ZONAZ1 = '" + codigoZonaActual + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM11", this.codigoClaseZona, codigoZonaActual);

                //Actualiza el tag del control correspondiente
                this.ActualizaTagNivelActual(nombreNivelActual);
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
        private bool FormValid(string nombreZonaActual)
        {
            bool result = false;
            string errores = "";

            try
            {
                //Validar las longitudes de la zona
                switch (this.nivelActual)
                {
                    case 1:
                        errores = this.ValidarLongitudNivelZona(1, this.txtNivel1.Text, this.longZonaNivel1, ref this.txtNivel1);
                        break;
                    case 2:
                        errores = this.ValidarLongitudNivelZona(2, this.txtNivel2.Text, this.longZonaNivel2, ref this.txtNivel2);
                        break;
                    case 3:
                        errores = this.ValidarLongitudNivelZona(3, this.txtNivel3.Text, this.longZonaNivel3, ref this.txtNivel3);
                        break;
                    case 4:
                        errores = this.ValidarLongitudNivelZona(4, this.txtNivel4.Text, this.longZonaNivel4, ref this.txtNivel4);
                        break;
                }

                if (errores != "")
                {
                    this.SetFocusCodigoNivelActual();
                }
                else
                {
                    bool codZonaOk = true;
                    if (this.nuevo) codZonaOk = this.CodigoZonaValido();    //Verificar que el codigo de zona no exista

                    if (!codZonaOk)
                    {
                        errores += "- El código de zona existe \n\r";      //Falta traducir
                        this.SetFocusCodigoNivelActual();
                    }
                }

                if (nombreZonaActual == "")
                {
                    errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.SetFocusNombreNivelActual();
                }

                string tipo = (this.cantNiveles == this.nivelActual) ? "D" : "T";
                if (tipo == "D")
                {
                    bool nombreZonaOk = true;
                    nombreZonaOk = this.NombreZonaValido(nombreZonaActual);    //Verificar que el nombre de zona no exista

                    if (!nombreZonaOk)
                    {
                        errores += "- Ya existe una zona con este nombre \n\r";      //Falta traducir
                        this.SetFocusNombreNivelActual();
                    }
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
        /// Le da el focus al nivel de zona actual
        /// </summary>
        private void SetFocusCodigoNivelActual()
        {
            switch (this.nivelActual)
            {
                case 1:
                    this.txtNivel1.Focus();
                    break;
                case 2:
                    this.txtNivel2.Focus();
                    break;
                case 3:
                    this.txtNivel3.Focus();
                    break;
                case 4:
                    this.txtNivel4.Focus();
                    break;
            }
        }

        /// <summary>
        /// Le da el focus al nombre de zona actual
        /// </summary>
        private void SetFocusNombreNivelActual()
        {
            switch (this.nivelActual)
            {
                case 1:
                    this.txtNombre1.Focus();
                    break;
                case 2:
                    this.txtNombre2.Focus();
                    break;
                case 3:
                    this.txtNombre3.Focus();
                    break;
                case 4:
                    this.txtNombre4.Focus();
                    break;
            }
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
                string codigoZonaActual = this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text + this.txtNivel4.Text;

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
        /// Valida que no exista el nombre de la zona
        /// </summary>
        /// <returns></returns>
        private bool NombreZonaValido(string nombreZonaActual)
        {
            bool result = false;

            try
            {
                string codigoZonaActual = this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text + this.txtNivel4.Text;

                if (codigoZonaActual != "")
                {
                    string query = "select count(NOMBZ1) from " + GlobalVar.PrefijoTablaCG + "GLM11 ";
                    query += "where CLASZ1 = '" + this.codigoClaseZona + "' and ZONAZ1 = '" + codigoZonaActual + "' and ";
                    query += "TIPOZ1 = 'D' and NOMBZ1 = '" + nombreZonaActual + "'";

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
                    utiles.ButtonEnabled(ref this.radButtonSave, false);

                    ctrltxtNivel.Focus();

                    RadMessageBox.Show("Código de zona obligatorio", this.LP.GetText("errValCodZona", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                string error = "";
                bool codZonaOk = true;

                if (this.nuevo)
                {
                    string longZonaOK = "";
                    //Validar las longitudes de la zona
                    switch (this.nivelActual)
                    {
                        case 1:
                            longZonaOK = this.ValidarLongitudNivelZona(1, this.txtNivel1.Text, this.longZonaNivel1, ref this.txtNivel1);
                            break;
                        case 2:
                            longZonaOK = this.ValidarLongitudNivelZona(2, this.txtNivel2.Text, this.longZonaNivel2, ref this.txtNivel2);
                            break;
                        case 3:
                            longZonaOK = this.ValidarLongitudNivelZona(3, this.txtNivel3.Text, this.longZonaNivel3, ref this.txtNivel3);
                            break;
                        case 4:
                            longZonaOK = this.ValidarLongitudNivelZona(4, this.txtNivel4.Text, this.longZonaNivel4, ref this.txtNivel4);
                            break;
                    }


                    if (longZonaOK != "")
                    {
                        codZonaOk = false;
                        error = longZonaOK;
                    }
                    else
                    {
                        codZonaOk = this.CodigoZonaValido();    //Verificar que el codigo de zona no exista
                        if (!codZonaOk) error = "Código de zona ya existe";
                    }

                }

                if (codZonaOk)
                {
                    //this.txtNombre.Enabled = true;
                    ctrltxtNivel.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    //this.txtNombre.Enabled = false;
                    utiles.ButtonEnabled(ref this.radButtonSave, false);
                    ctrltxtNivel.Focus();

                    RadMessageBox.Show(error, this.LP.GetText("errValCodZonaExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        /// <summary>
        /// Verifica si existen cambios que no han sido grabados
        /// </summary>
        /// <returns></returns>
        private bool ExistenCambiosGrabar()
        {
            //Chequear si existen modificaciones para grabar
            bool hayCambios = false;
            try
            {
                switch (this.nivelActual)
                {
                    case 1:
                        if ((this.txtNombre1.Tag != null && this.txtNombre1.Text != this.txtNombre1.Tag.ToString()) ||
                            (this.txtNivel1.Tag != null && this.txtNivel1.Text != this.txtNivel1.Tag.ToString())) hayCambios = true;
                        break;
                    case 2:
                        if ((this.txtNombre2.Tag != null && this.txtNombre2.Text != this.txtNombre2.Tag.ToString()) ||
                            (this.txtNivel2.Tag != null && this.txtNivel2.Text != this.txtNivel2.Tag.ToString())) hayCambios = true;
                        break;
                    case 3:
                        if (this.txtNombre3.Tag != null && this.txtNombre3.Text != this.txtNombre3.Tag.ToString() ||
                           (this.txtNivel3.Tag != null && this.txtNivel3.Text != this.txtNivel3.Tag.ToString())) hayCambios = true;
                        break;
                    case 4:
                        if (this.txtNombre4.Tag != null && this.txtNombre4.Text != this.txtNombre4.Tag.ToString() ||
                           (this.txtNivel4.Tag != null && this.txtNivel4.Text != this.txtNivel4.Tag.ToString())) hayCambios = true;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (hayCambios);
        }

        /// <summary>
        /// Devuelve el nombre de la zona del nivel actual
        /// </summary>
        /// <returns></returns>
        private string NombreNivelActual()
        {
            string result = "";
            try
            {
                switch (this.nivelActual)
                {
                    case 1:
                        result = this.txtNombre1.Text;
                        break;
                    case 2:
                        result = this.txtNombre2.Text;
                        break;
                    case 3:
                        result = this.txtNombre3.Text;
                        break;
                    case 4:
                        result = this.txtNombre4.Text;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el codigo de la zona del nivel actual
        /// </summary>
        /// <returns></returns>
        private string CodigoNivelActual()
        {
            string result = "";
            try
            {
                switch (this.nivelActual)
                {
                    case 1:
                        result = this.txtNivel1.Text;
                        break;
                    case 2:
                        result = this.txtNivel1.Text + this.txtNivel2.Text;
                        break;
                    case 3:
                        result = this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text;
                        break;
                    case 4:
                        result = this.txtNivel1.Text + this.txtNivel2.Text + this.txtNivel3.Text + this.txtNivel4.Text;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Actualiza el Tag del nivel actual con el valor actual del nombre que le corresponde 
        /// </summary>
        /// <param name="nombreNivelActual"></param>
        /// <returns></returns>
        private void ActualizaTagNivelActual(string nombreNivelActual)
        {
            try
            {
                switch (this.nivelActual)
                {
                    case 1:
                        this.txtNombre1.Tag = nombreNivelActual;
                        this.txtNivel1.Tag = this.txtNivel1.Text;
                        break;
                    case 2:
                        this.txtNombre2.Tag = nombreNivelActual;
                        this.txtNivel2.Tag = this.txtNivel2.Text;
                        break;
                    case 3:
                        this.txtNombre3.Tag = nombreNivelActual;
                        this.txtNivel3.Tag = this.txtNivel3.Text;
                        break;
                    case 4:
                        this.txtNombre4.Tag = nombreNivelActual;
                        this.txtNivel4.Tag = this.txtNivel4.Text;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el Texto del nivel actual con el valor actual del tag que le corresponde 
        /// </summary>
        /// <returns></returns>
        private void ActualizaTextoNivelActualConTag()
        {
            try
            {
                switch (this.nivelActual)
                {
                    case 1:
                        this.txtNombre1.Text = this.txtNombre1.Tag.ToString();
                        this.txtNivel1.Text = this.txtNivel1.Tag.ToString();
                        break;
                    case 2:
                        this.txtNombre2.Text = this.txtNombre2.Tag.ToString();
                        this.txtNivel2.Text = this.txtNivel2.Tag.ToString();
                        break;
                    case 3:
                        this.txtNombre3.Text = this.txtNombre3.Tag.ToString();
                        this.txtNivel3.Text = this.txtNivel3.Tag.ToString();
                        break;
                    case 4:
                        this.txtNombre4.Text = this.txtNombre4.Tag.ToString();
                        this.txtNivel4.Text = this.txtNivel4.Tag.ToString();
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Retrocede un nivel
        /// </summary>
        private void NivelAnterior()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Chequear si existen modificaciones para grabar
            if (this.ExistenCambiosGrabar())
            {
                //Pedir confirmación de pérdida de los campos
                string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    this.radButtonSave.PerformClick();
                }
                else if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.No)
                {
                    this.ActualizaTextoNivelActualConTag();
                }
            }

            //this.nuevo = false;

            string codigoNivelSgte = "";
            string nombreNivelSgte = "";

            switch (this.nivelActual)
            {
                case 2:
                    codigoNivelSgte = this.txtNivel2.Text.Trim();
                    nombreNivelSgte = this.txtNombre2.Text.Trim();
                    break;
                case 3:
                    codigoNivelSgte = this.txtNivel3.Text.Trim();
                    nombreNivelSgte = this.txtNombre3.Text.Trim();
                    break;
                case 4:
                    codigoNivelSgte = this.txtNivel4.Text.Trim();
                    nombreNivelSgte = this.txtNombre4.Text.Trim();
                    break;
            }

            this.nivelActual--;
            this.ActivarEdicionNivelActual();

            if (codigoNivelSgte == "" && nombreNivelSgte == "")
            {
                utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
            }
            else
            {
                utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, true);
                //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgte;
            }

            if (this.nivelActual == 1)
            {
                utiles.ButtonEnabled(ref this.radButtonNivelAnterior, false);
                //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAntNoAct;
            }
            else
            {
                utiles.ButtonEnabled(ref this.radButtonNivelAnterior, true);
                //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAnt;
            }

            utiles.ButtonEnabled(ref this.radButtonNuevoNivel, true);
            //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionar;
            
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Avanza un nivel
        /// </summary>
        private void NivelSiguiente()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Chequear si existen modificaciones para grabar
            if (this.ExistenCambiosGrabar())
            {
                //Pedir confirmación de pérdida de los campos
                string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    //if (this.nuevo) this.AltaInfo();
                    //else this.ActualizarInfo();
                    this.radButtonSave.PerformClick();
                }
                else if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.No)
                {
                    this.ActualizaTextoNivelActualConTag();
                }
            }

            //this.nuevo = false;
            this.nivelActual++;
            this.ActivarEdicionNivelActual();
            if (this.nivelActual < this.cantNiveles)
            {
                string codigoNivelSgte = "";
                string nombreNivelSgte = "";

                switch (this.nivelActual + 1)
                {
                    case 2:
                        codigoNivelSgte = this.txtNivel2.Text.Trim();
                        nombreNivelSgte = this.txtNombre2.Text.Trim();
                        break;
                    case 3:
                        codigoNivelSgte = this.txtNivel3.Text.Trim();
                        nombreNivelSgte = this.txtNombre3.Text.Trim();
                        break;
                    case 4:
                        codigoNivelSgte = this.txtNivel4.Text.Trim();
                        nombreNivelSgte = this.txtNombre4.Text.Trim();
                        break;
                }


                if (codigoNivelSgte == "" && nombreNivelSgte == "")
                {
                    utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                    //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, true);
                    //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgte;
                }

                utiles.ButtonEnabled(ref this.radButtonNuevoNivel, true);
                //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionar;
            }
            else
            {
                utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
                //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
                utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
                //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionarNoAct;
            }

            utiles.ButtonEnabled(ref this.radButtonNivelAnterior, true);
            //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAnt;
            
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Graba la información
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            string nombreNivelActual = this.NombreNivelActual();

            if (this.FormValid(nombreNivelActual))
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo(nombreNivelActual);
                    if (result == "")
                    {
                        //this.nuevo = false;
                        //this.codigo = this.txtCodigo.Text.Trim();
                    }
                }
                else result = this.ActualizarInfo(nombreNivelActual);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Crea un nuveo nivel
        /// </summary>
        private void NuevoNivel()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Chequear si existen modificaciones para grabar
            if (this.ExistenCambiosGrabar())
            {
                //Pedir confirmación de pérdida de los campos
                string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    this.radButtonSave.PerformClick();
                }
                else if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.No)
                {
                    this.ActualizaTextoNivelActualConTag(); 
                }
            }

            this.nuevo = true;
            this.nivelActual++;
            utiles.ButtonEnabled(ref this.radButtonNivelSiguiente, false);
            //this.radButtonNivelSiguiente.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelSgteNoAct;
            utiles.ButtonEnabled(ref this.radButtonNivelAnterior, true);
            //this.radButtonNivelAnterior.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAnt;
            utiles.ButtonEnabled(ref this.radButtonNuevoNivel, false);
            //this.radButtonNuevoNivel.Image = global::ModMantenimientos.Properties.Resources.ZonaNivelAdicionarNoAct;

            switch (this.nivelActual)
            {
                case 2:
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNivel2.Text = "";
                    this.txtNivel2.Tag = "";
                    this.txtNombre2.Text = "";
                    this.txtNombre2.Tag = "";
                    this.txtNivel2.IsReadOnly = false;
                    this.txtNombre2.IsReadOnly = false;
                    this.txtNivel3.Text = "";
                    this.txtNivel3.Tag = "";
                    this.txtNombre3.Text = "";
                    this.txtNombre3.Tag = "";
                    this.txtNombre3.IsReadOnly = true;
                    this.txtNivel4.Text = "";
                    this.txtNivel4.Tag = "";
                    this.txtNombre4.Text = "";
                    this.txtNombre4.Tag = "";
                    this.txtNombre4.IsReadOnly = true;
                    this.ActiveControl = this.txtNivel2;
                    this.txtNivel2.Select(0, 0);
                    this.txtNivel2.Focus();
                    break;
                case 3:
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = true;
                    this.txtNivel3.Text = "";
                    this.txtNivel3.Tag = "";
                    this.txtNombre3.Text = "";
                    this.txtNombre3.Tag = "";
                    this.txtNivel3.IsReadOnly = false;
                    this.txtNombre3.IsReadOnly = false;
                    this.txtNivel4.Text = "";
                    this.txtNivel4.Tag = "";
                    this.txtNombre4.Text = "";
                    this.txtNombre4.Tag = "";
                    this.txtNombre4.IsReadOnly = true;
                    this.ActiveControl = this.txtNivel3;
                    this.txtNivel3.Select(0, 0);
                    this.txtNivel3.Focus();
                    break;
                case 4:
                    this.txtNombre1.IsReadOnly = true;
                    this.txtNombre2.IsReadOnly = true;
                    this.txtNombre3.IsReadOnly = true;
                    this.txtNivel4.Text = "";
                    this.txtNivel4.Tag = "";
                    this.txtNombre4.Text = "";
                    this.txtNombre4.Tag = "";
                    this.txtNivel4.IsReadOnly = false;
                    this.txtNombre4.IsReadOnly = false;
                    this.ActiveControl = this.txtNivel4;
                    this.txtNivel4.Select(0, 0);
                    this.txtNivel4.Focus();
                    break;
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Valida la longitud del código de zona
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="valorZonaNivel"></param>
        /// <param name="longZonaNivel"></param>
        /// <param name="etiquetaZonaNivel"></param>
        /// <param name="controlZonaNivel"></param>
        /// <returns></returns>
        private string ValidarLongitudNivelZona(int nivel, string valorZonaNivel, int longZonaNivel, ref Telerik.WinControls.UI.RadTextBoxControl controlZonaNivel)
        {
            string result = "";
            try
            {
                if (valorZonaNivel.Length != longZonaNivel)
                {
                    result = "- Longitud del código de zona " + nivel + " no válido";   //Falta traducir
                    this.ActiveControl = (Control)controlZonaNivel;
                    ((Control)controlZonaNivel).Select();
                    ((Control)controlZonaNivel).Focus();
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando la longitud del código de zona (" + ex.Message + ")";
            }

            return (result);
        }
        #endregion

        private void frmMtoGLM11ZonaJerarq_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void txtNivel1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }

        private void txtNivel2_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }

        private void txtNivel3_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }

        private void txtNivel4_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }
    }
}
