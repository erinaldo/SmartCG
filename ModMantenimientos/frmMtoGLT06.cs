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
    public partial class frmMtoGLT06 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOTIPCOM";

        private bool nuevo;
        private string codigo;
        private string nombre;
        private string estado;
        private string posModoTrabajo;
        private string posValidarDoc;

        private const string autClaseElemento = "004";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "20";
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

        public string PosModoTrabajo
        {
            get
            {
                return (this.posModoTrabajo);
            }
            set
            {
                this.posModoTrabajo = value;
            }
        }

        public string PosValidarDoc
        {
            get
            {
                return (this.posValidarDoc);
            }
            set
            {
                this.posValidarDoc = value;
            }
        }

        public frmMtoGLT06()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";

            this.gbModoTrabajo.ElementTree.EnableApplicationThemeName = false;
            this.gbModoTrabajo.ThemeName = "ControlDefault";

            this.gbValidarDoc.ElementTree.EnableApplicationThemeName = false;
            this.gbValidarDoc.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLT06_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Tipos de Comprobantes Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información del tipo de comprobante y la muestra en los controles
                this.CargarInfoTiposComp();

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
                if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                else utiles.ButtonEnabled(ref this.radButtonDelete, false);
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
                string codTipoComp = this.txtCodigo.Text.Trim();

                if (codTipoComp == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Tipo de comprobante obligatorio", this.LP.GetText("errValTipoComp", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codTipoCompOk = true;
                if (this.nuevo) codTipoCompOk = this.ValidarTipoComprobante(this.txtCodigo.Text);    //Verificar que el codigo no exista

                if (codTipoCompOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActivo.Value = true;
                        //this.radToggleSwitchEstadoActivo.Enabled = false;
                        this.radToggleSwitchEstadoActivo.Enabled = true;
                    }

                    this.txtCodigo.IsReadOnly = true;

                    this.codigo = codTipoComp;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de tipo de comprobante ya existe", this.LP.GetText("errValTipoCompExiste", "Error"));  //Falta traducir
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

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
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

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
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

        private void FrmMtoGLT06_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLT06_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    (this.rbModoTrabajoBatch.IsChecked && !(Convert.ToBoolean(this.rbModoTrabajoBatch.Tag))) ||
                    (!this.rbModoTrabajoBatch.IsChecked && (Convert.ToBoolean(this.rbModoTrabajoBatch.Tag))) ||
                    (this.rbModoTrabajoInteractivo.IsChecked && !(Convert.ToBoolean(this.rbModoTrabajoInteractivo.Tag))) ||
                    (!this.rbModoTrabajoInteractivo.IsChecked && (Convert.ToBoolean(this.rbModoTrabajoInteractivo.Tag))) ||
                    (this.rbValidarDocNo.IsChecked && !(Convert.ToBoolean(this.rbValidarDocNo.Tag))) ||
                    (!this.rbValidarDocNo.IsChecked && (Convert.ToBoolean(this.rbValidarDocNo.Tag))) ||
                    (this.rbValidarDocSi.IsChecked && !(Convert.ToBoolean(this.rbValidarDocSi.Tag))) ||
                    (!this.rbValidarDocSi.IsChecked && (Convert.ToBoolean(this.rbValidarDocSi.Tag))) ||
                    (this.rbValidarDocSoloSiCancel.IsChecked && !(Convert.ToBoolean(this.rbValidarDocSoloSiCancel.Tag))) ||
                    (!this.rbValidarDocSoloSiCancel.IsChecked && (Convert.ToBoolean(this.rbValidarDocSoloSiCancel.Tag)))
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
            catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(frmMtoGLT06_FormClosing)"); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Tipos de Comprobantes Alta/Edita");
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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLT06TituloALta", "Mantenimiento de Tipos de Comprobantes - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT06TituloEdit", "Mantenimiento de Tipos de Comprobantes - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");;
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
            this.gbModoTrabajo.Enabled = valor;
            this.gbValidarDoc.Enabled = valor;
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
                //-------------- Validar el nombre del tipo de comprobante ------------------
                if (this.txtNombre.Text.Trim() == "") errores += "El nombre no puede estar en blanco \n\r";      //Falta traducir
                //else if (this.txtNombre.Text.Substring(0, 1) == " ") errores += "El nombre no puede comenzar por blanco \n\r";      //Falta traducir

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
                Log.Error(ex.Message + " " + this.GetType() + "(FormValid)");

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Valida el código del tipo de comprobante
        /// </summary>
        /// <returns></returns>
        private bool ValidarTipoComprobante(string codigoTipoExt)
        {
            bool result = false;

            try
            {
                string codTipoComp = this.txtCodigo.Text.Trim();

                if (codTipoComp != "")
                {
                    string query = "select count(TIVOTV) from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                    query += "where TIVOTV = '" + codTipoComp + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(ValidarTipoComprobante)"); }

            return (result);
        }

        /// <summary>
        /// Devuelve el valor que le corresponde a informar la posición del modo de trabajo
        /// </summary>
        /// <returns></returns>
        private string ObtenerValorModoTrabajo()
        {
            string posModoTrabajo = "";
            if (this.rbModoTrabajoBatch.IsChecked) posModoTrabajo = "1";
            else if (this.rbModoTrabajoInteractivo.IsChecked) posModoTrabajo = "0";

            return (posModoTrabajo);
        }


        /// <summary>
        /// Devuelve el valor que le corresponde a informar la posición de la existencia del documento
        /// </summary>
        /// <returns></returns>
        private string ObtenerValorExisteDoc()
        {
            string posExisteDoc = "";
            if (this.rbValidarDocNo.IsChecked) posExisteDoc = "0";
            else if (this.rbValidarDocSi.IsChecked) posExisteDoc = "1";
            else if (this.rbValidarDocSoloSiCancel.IsChecked) posExisteDoc = "2";

            return (posExisteDoc);
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
                string CODITV = this.ObtenerValorModoTrabajo();
                string DEFDTV = this.ObtenerValorExisteDoc();

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                //Dar de alta al tipo de comprobante extracontable (GLT06)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT06";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATTV, TIVOTV, NOMBTV, CODITV, DEFDTV) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtNombre.Text + "', '";
                query += CODITV + "', '" + DEFDTV + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLT06", this.codigo, null);

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
                Log.Error(ex.Message + " " + this.GetType() + "(AltaInfo)");

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
                string CODITV = this.ObtenerValorModoTrabajo();
                string DEFDTV = this.ObtenerValorExisteDoc();

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT06 set ";
                query += "STATTV = '" + estado + "', NOMBTV = '" + this.txtNombre.Text + "', ";
                query += "CODITV = '" + CODITV + "', DEFDTV = '" + DEFDTV + "' ";
                query += " where TIVOTV = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT06", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " " + this.GetType() + "(ActualizarInfo)");

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtNombre.Tag = this.txtNombre.Text;

            if (this.rbModoTrabajoBatch.IsChecked) this.rbModoTrabajoBatch.Tag = true;
            else this.rbModoTrabajoBatch.Tag = false;
            if (this.rbModoTrabajoInteractivo.IsChecked) this.rbModoTrabajoInteractivo.Tag = true;
            else this.rbModoTrabajoInteractivo.Tag = false;
            if (this.rbValidarDocNo.IsChecked) this.rbValidarDocNo.Tag = true;
            else this.rbValidarDocNo.Tag = false;
            if (this.rbValidarDocSi.IsChecked) this.rbValidarDocSi.Tag = true;
            else this.rbValidarDocSi.Tag = false;
            if (this.rbValidarDocSoloSiCancel.IsChecked) this.rbValidarDocSoloSiCancel.Tag = true;
            else this.rbValidarDocSoloSiCancel.Tag = false;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActivo.Tag = true;
            this.txtNombre.Tag = "";
            this.rbModoTrabajoBatch.Tag = true;
            this.rbModoTrabajoInteractivo.Tag = false;
            this.rbValidarDocNo.Tag = true;
            this.rbValidarDocSi.Tag = false;
            this.rbValidarDocSoloSiCancel.Tag = false;
        }

        /// <summary>
        /// Rellena los controles con los datos del grupo de cuentas auxiliares (modo edición)
        /// </summary>
        private void CargarInfoTiposComp()
        {
            try
            {
                this.txtCodigo.Text = this.codigo;
                this.txtNombre.Text = this.nombre.Trim();
                if (this.estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                else this.radToggleSwitchEstadoActivo.Value = false;

                switch (this.posModoTrabajo)
                {
                    case "0":
                        this.rbModoTrabajoInteractivo.IsChecked = true;
                        break;
                    case "1":
                    default:
                        this.rbModoTrabajoBatch.IsChecked = true;
                        break;
                }

                switch (this.posValidarDoc)
                {
                    case "1":
                        this.rbValidarDocSi.IsChecked = true;
                        break;
                    case "2":
                        this.rbValidarDocSoloSiCancel.IsChecked = true;
                        break;
                    case "0":
                    default:
                        this.rbValidarDocNo.IsChecked = true;
                        break;
                }

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(CargarInfoTiposComp)"); }
        }

        /// <summary>
        /// Eliminar un tipo de comprobante
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar el Tipo de comprobante " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en las cabecera de los comprobantes
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "where TICOIC = " + this.codigo;

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en cabeceras de comprobantes
                        mensaje = "No es posible eliminar el Tipo de comprobante porque está en uso en la cabecera de algunos comprobantes.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el tipo de comprobante
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                        query += "where TIVOTV = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT06", this.codigo, null);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar el Tipo de comprobante.";
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

                                if (cantRegistros > 0) utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM07", autClaseElemento, this.codigo);

                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                                query += "where CLELAG = '" + autClaseElemento + "' and ELEMAG = '" + this.codigo + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            }
                            catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(radButtonEliminar_Click)"); }
                            
                            //Cerrar el formulario
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(radButtonButtonEliminar_Click)"); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Grabar un tipo de comprobante
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
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                        else utiles.ButtonEnabled(ref this.radButtonDelete, false);
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
        #endregion

        private void frmMtoGLT06_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
