using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoPRT03 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOTIPEXT";

        private bool nuevo;
        private string codigo;
        private string nombre;
        private string estado;
        private string posTipoAux;

        string[] codigosReservados = { "A", "C", "D", "E", "I", "M", "R", "U", "Z", "E1", "R1", "U1", "E2", "R2", "U2" };

        private const string autClaseElemento = "005";
        private const string autGrupo = "";
        private const string autOperConsulta = "";
        private const string autOperModifica = "";
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

        public string Nombre
        {
            get
            {
                return (this.nombre);
            }
            set
            {
                this.nombre = value;
            }
        }

        public string Estado
        {
            get
            {
                return (this.estado);
            }
            set
            {
                this.estado = value;
            }
        }

        public string PosTipoAux
        {
            get
            {
                return (this.posTipoAux);
            }
            set
            {
                this.posTipoAux = value;
            }
        }

        public frmMtoPRT03()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbPosTipoAux.ElementTree.EnableApplicationThemeName = false;
            this.gbPosTipoAux.ThemeName = "ControlDefault";

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoPRT03_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Tipos de Extracontable Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();
            
            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonEliminar, false);
               
                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información del tipo de extracontable y la muestra en los controles
                this.CargarInfoTiposExt();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }

                bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                else utiles.ButtonEnabled(ref this.radButtonEliminar, false);
            }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codTipoExt = this.txtCodigo.Text.Trim();

                if (codTipoExt == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Tipo de extracontable obligatorio", this.LP.GetText("errValTipoExt", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                int pos = Array.IndexOf(this.codigosReservados, codTipoExt);
                if (pos > -1)
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Tipo de dato reservado", "Error");  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codTipoExtOk = true;
                if (this.nuevo) codTipoExtOk = this.ValidarTipoExtracontable(this.txtCodigo.Text);    //Verificar que el codigo no exista

                if (codTipoExtOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActivo.Value = true;
                        this.radToggleSwitchEstadoActivo.Enabled = false;
                    }

                    this.txtCodigo.IsReadOnly = true;

                    this.codigo = codTipoExt;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de tipo de extracontable ya existe", this.LP.GetText("errValTipoExtExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
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

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
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

        private void RadButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void RadButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
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

        private void FrmMtoPRT03_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoPRT03_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    (this.rbPosTipoAuxTodos.IsChecked && !(Convert.ToBoolean(this.rbPosTipoAuxTodos.Tag))) ||
                    (!this.rbPosTipoAuxTodos.IsChecked && (Convert.ToBoolean(this.rbPosTipoAuxTodos.Tag))) ||
                    (this.rbPosTipoAux1.IsChecked && !(Convert.ToBoolean(this.rbPosTipoAux1.Tag))) ||
                    (!this.rbPosTipoAux1.IsChecked && (Convert.ToBoolean(this.rbPosTipoAux1.Tag))) ||
                    (this.rbPosTipoAux2.IsChecked && !(Convert.ToBoolean(this.rbPosTipoAux2.Tag))) ||
                    (!this.rbPosTipoAux2.IsChecked && (Convert.ToBoolean(this.rbPosTipoAux2.Tag))) ||
                    (this.rbPosTipoAux3.IsChecked && !(Convert.ToBoolean(this.rbPosTipoAux3.Tag))) ||
                    (!this.rbPosTipoAux3.IsChecked && (Convert.ToBoolean(this.rbPosTipoAux3.Tag)))
                )
                {
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
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Tipos de Extracontable Alta/Edita");
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoPRT03TituloALta", "Mantenimiento de Tipos de Extracontables - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoPRT03TituloEdit", "Mantenimiento de Tipos de Extracontables - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo del grupo de cuentas de auxiliar (al dar de alta a un grupo)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActivo.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.gbPosTipoAux.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El grupo de cuentas de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);
            
            this.radToggleSwitchEstadoActivo.Enabled = false;
            this.txtNombre.IsReadOnly = true;
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
                //-------------- Validar el nombre de la cuenta de auxiliar ------------------
                if (this.txtNombre.Text.Trim() == "") errores += "El nombre no puede estar en blanco \n\r";      //Falta traducir
                else if (this.txtNombre.Text.Substring(0, 1) == " ") errores += "El nombre no puede comenzar por blanco \n\r";      //Falta traducir
                else 
                    {
                        int pos = Array.IndexOf(this.codigosReservados, this.txtNombre.Text.Trim());
                        if (pos > -1)
                        {
                            errores += "Tipo de dato reservado \n\r";      //Falta traducir
                        }
                    }

                if (errores == "") result = true;
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select();
                    this.txtNombre.Focus();
                }
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
        /// Valida el código del tipo de extracontable
        /// </summary>
        /// <returns></returns>
        private bool ValidarTipoExtracontable(string codigoTipoExt)
        {
            bool result = false;

            try
            {
                string codTipoExt = this.txtCodigo.Text.Trim();

                if (codTipoExt != "")
                {
                    string query = "select count(TDATAH) from " + GlobalVar.PrefijoTablaCG + "PRT03 ";
                    query += "where TDATAH = '" + codigoTipoExt + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Dar de alta a un in tipo de extracontable
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string APNAAH = this.ObtenerValorPosTipoAux();

                string estadoActual = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                //Dar de alta al tipo de comprobante extracontable (PRT03)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "PRT03";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATAH, TDATAH, NOMBAH, APNAAH) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estadoActual + "', '" + this.codigo + "', '" + this.txtNombre.Text + "', ";
                query += APNAAH + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "PRT03", this.codigo, null);

                //Insertar al usuario como propietario del elemento
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";

                query += "'" + autClaseElemento + "', '" + this.codigo + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar un tipo de extracontable
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string APNAAH = this.ObtenerValorPosTipoAux();

                string estadoActual = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "PRT03 set ";
                query += "STATAH = '" + estadoActual + "', NOMBAH = '" + this.txtNombre.Text + "', ";
                query += "APNAAH = " + APNAAH;
                query += " where TDATAH = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "PRT03", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve el valor que le corresponde a informar la posición del valod de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ObtenerValorPosTipoAux()
        {
            string posTipoAux = "";
            if (this.rbPosTipoAuxTodos.IsChecked) posTipoAux = "0";
            else if (this.rbPosTipoAux1.IsChecked) posTipoAux = "1";
            else if (this.rbPosTipoAux2.IsChecked) posTipoAux = "2";
            else if (this.rbPosTipoAux3.IsChecked) posTipoAux = "3";
            return (posTipoAux);
        }

        /// <summary>
        /// Grabar un Tipo de Extracontable
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
                        this.codigo = this.txtCodigo.Text.Trim();

                        bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                        else utiles.ButtonEnabled(ref this.radButtonEliminar, true);
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
                    
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar un Tipo de Extracontable
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar el Tipo de Extracontable " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en los asientos de extracontables
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "PRB01 ";
                    query += "where TIDAP3 = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso el asientos de extracontables
                        mensaje = "No es posible eliminar el Tipo de extracontable porque está en uso en asientos de extracontables.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el tipo de comprobante
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "PRT03 ";
                        query += "where TDATAH = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "PRT03", this.codigo, null);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar el Tipo de extracontable.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Eliminarlo de las tablas de autorizaciones
                            try
                            {
                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                                query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + this.codigo + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM07", autClaseElemento, this.codigo);

                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                                query += "where CLELAG = '" + autClaseElemento + "' and ELEMAG = '" + this.codigo + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));

                                //Cerrar el formulario
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtNombre.Tag = this.txtNombre.Text;

            if (this.rbPosTipoAuxTodos.IsChecked) this.rbPosTipoAuxTodos.Tag = true;
            else this.rbPosTipoAuxTodos.Tag = false;
            if (this.rbPosTipoAux1.IsChecked) this.rbPosTipoAux1.Tag = true;
            else this.rbPosTipoAux1.Tag = false;
            if (this.rbPosTipoAux2.IsChecked) this.rbPosTipoAux2.Tag = true;
            else this.rbPosTipoAux2.Tag = false;
            if (this.rbPosTipoAux3.IsChecked) this.rbPosTipoAux3.Tag = true;
            else this.rbPosTipoAux3.Tag = false;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActivo.Tag = true;
            this.txtNombre.Tag = "";
            this.rbPosTipoAuxTodos.Tag = true;
            this.rbPosTipoAux1.Tag = false;
            this.rbPosTipoAux2.Tag = false;
            this.rbPosTipoAux3.Tag = false;
        }

        /// <summary>
        /// Rellena los controles con los datos del grupo de cuentas auxiliares (modo edición)
        /// </summary>
        private void CargarInfoTiposExt()
        {
            try
            {
                this.txtCodigo.Text = this.codigo;
                this.txtNombre.Text = this.nombre.Trim();
                if (this.estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                else this.radToggleSwitchEstadoActivo.Value = false;

                switch (this.posTipoAux)
                {
                    case "1":
                        this.rbPosTipoAux1.IsChecked = true;
                        break;
                    case "2":
                        this.rbPosTipoAux2.IsChecked = true;
                        break;
                    case "3":
                        this.rbPosTipoAux3.IsChecked = true;
                        break;
                    case "0":
                    default:
                        this.rbPosTipoAuxTodos.IsChecked = true;
                        break;
                }

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        private void frmMtoPRT03_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}