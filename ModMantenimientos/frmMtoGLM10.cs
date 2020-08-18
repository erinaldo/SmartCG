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
    public partial class frmMtoGLM10 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCLSZON";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "024";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "30";

        private DataTable dtTiposAux;
        private const string separadorEstructura = "-";

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        Dictionary<string, string> displayNames;

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

        public frmMtoGLM10()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchValidarZonas.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchValidarZonas.ThemeName = "MaterialBlueGrey";

            this.radCollapsiblePanelTipoAux.IsExpanded = false;
            this.radCollapsiblePanelTipoAux.EnableAnimation = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM10_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Clase de Zona Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            BuildDisplayNames();

            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();

                //DataGrid Tipos de Auxiliar
                this.lblNoExistenTiposAux.Visible = false;
                this.radCollapsiblePanelTipoAux.Visible = false;
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información de los tipos de auxiliar y mostrarla en los controles
                this.CargarInfoClaseZona();

                /*bool operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta, this.codigo);
                bool operarModificar = aut.Operar(autClaseElemento, autGrupo, autOperModifica, this.codigo);
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else this.txtNombre.Focus();
                */

                this.ActiveControl = this.txtNombre;
                this.txtNombre.Select(0, 0);
                this.txtNombre.Focus();

                //Tipos de auxiliar
                this.radCollapsiblePanelTipoAux.Visible = true;

                this.dtTiposAux = new DataTable();

                bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                else utiles.ButtonEnabled(ref this.radButtonDelete, false);
            }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);

            //SMR this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.nuevo)
            {
                string codClaseZona = this.txtCodigo.Text;

                if (codClaseZona == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Clase de zona obligatorio", this.LP.GetText("errValCodClaseZona", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                codClaseZona = codClaseZona.Replace(" ", "");
                if (codClaseZona.Length < 3)
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Clase de zona no puede tener carácteres blancos", this.LP.GetText("errValCodClaseZona", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codClaseZonaOk = true;
                if (this.nuevo) codClaseZonaOk = this.CodigoClaseZonaValido();    //Verificar que el codigo no exista

                if (codClaseZonaOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo) 
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                    }

                    this.txtCodigo.IsReadOnly = true;
                    this.codigo = codClaseZona;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Clase de zona ya existe", this.LP.GetText("errValCodClaseZonaExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }
        
        private void TxtEstNivel1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CaracterNumericoEstructura(e);
        }

        private void TxtEstNivel2_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CaracterNumericoEstructura(e);
        }

        private void TxtEstNivel3_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CaracterNumericoEstructura(e);
        }

        private void TxtEstNivel4_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CaracterNumericoEstructura(e);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
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

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadCollapsiblePanelTipoAux_Expanded(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(888, 721);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 100);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, this.Size.Height - 100);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 100);

            this.CentrarForm();
        }

        private void RadCollapsiblePanelTipoAux_Collapsed(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(888, 511);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 100);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, this.Size.Height - 100);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 100);

            this.CentrarForm();
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

        private void FrmMtoGLM10_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtEstNivel1.Text != this.txtEstNivel1.Tag.ToString() ||
                    this.txtEstNivel2.Text != this.txtEstNivel2.Tag.ToString() ||
                    this.txtEstNivel3.Text != this.txtEstNivel3.Tag.ToString() ||
                    this.txtEstNivel4.Text != this.txtEstNivel4.Tag.ToString() ||
                    this.txtNombreNiv1.Text != this.txtNombreNiv1.Tag.ToString() ||
                    this.txtNombreNiv2.Text != this.txtNombreNiv2.Tag.ToString() ||
                    this.txtNombreNiv3.Text != this.txtNombreNiv3.Tag.ToString() ||
                    this.txtNombreNiv4.Text != this.txtNombreNiv4.Tag.ToString() ||
                    this.radToggleSwitchValidarZonas.Value != (bool)(this.radToggleSwitchValidarZonas.Tag)
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Clase de Zona Alta/Edita");
        }

        private void FrmMtoGLM10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM10TituloALta", "Mantenimiento de Clase de Zona - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT03TituloEdit", "Mantenimiento de Clase de Zona - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblClaseZona.Text = this.LP.GetText("lblGLM10CodClaseZona", "Clase de zona");
            this.lblNombre.Text = this.LP.GetText("lblGLM10Nombre", "Nombre");
            this.lblEstructura.Text = this.LP.GetText("lblGLM10EstZona", "Estructura de zona");
            this.lblNombreNiv1.Text = this.LP.GetText("lblGLM10NombreNiv1", "Nombre de nivel 1");
            this.lblNombreNiv2.Text = this.LP.GetText("lblGLM10NombreNiv2", "Nombre de nivel 2");
            this.lblNombreNiv3.Text = this.LP.GetText("lblGLM10NombreNiv3", "Nombre de nivel 3");
            this.lblNombreNiv4.Text = this.LP.GetText("lblGLM10NombreNiv4", "Nombre de nivel 4");
            this.lblValidarZonas.Text = this.LP.GetText("lblGLM10ValidarZona", "Validar zonas");
            this.lblNoExistenTiposAux.Text = this.LP.GetText("lblGLM10NoExistenTiposAux", "Clase no asociada a ningún tipo de auxiliar");
        }
        
        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.txtEstNivel1.Enabled = valor;
            this.txtEstNivel2.Enabled = valor;
            this.txtEstNivel3.Enabled = valor;
            this.txtEstNivel4.Enabled = valor;
            //this.txtNombreNiv1.Enabled = valor;
            //this.txtNombreNiv2.Enabled = valor;
            //this.txtNombreNiv3.Enabled = valor;
            //this.txtNombreNiv4.Enabled = valor;
            this.radToggleSwitchValidarZonas.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El tipo de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNombre.Enabled = false;
            this.txtEstNivel1.Enabled = false;
            this.txtEstNivel2.Enabled = false;
            this.txtEstNivel3.Enabled = false;
            this.txtEstNivel4.Enabled = false;
            this.txtNombreNiv1.Enabled = false;
            this.txtNombreNiv2.Enabled = false;
            this.txtNombreNiv3.Enabled = false;
            this.txtNombreNiv4.Enabled = false;
            this.radToggleSwitchValidarZonas.Enabled = false;
        }

        /// <summary>
        /// Rellena los controles con los datos de la clase de zona (modo edición)
        /// </summary>
        private void CargarInfoClaseZona()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    string estado = dr.GetValue(dr.GetOrdinal("STATZ0")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;

                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBZ0")).ToString().Trim();

                    string estructura = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();
                    if (estructura.Length < 4) estructura.PadRight(4, '0');
                    this.txtEstNivel1.Text = estructura.Substring(0, 1);
                    this.txtEstNivel2.Text = estructura.Substring(1, 1);
                    this.txtEstNivel3.Text = estructura.Substring(2, 1);
                    this.txtEstNivel4.Text = estructura.Substring(3, 1);

                    this.txtNombreNiv1.Text = dr.GetValue(dr.GetOrdinal("NNV1Z0")).ToString().Trim();
                    this.txtNombreNiv2.Text = dr.GetValue(dr.GetOrdinal("NNV2Z0")).ToString().Trim();
                    this.txtNombreNiv3.Text = dr.GetValue(dr.GetOrdinal("NNV3Z0")).ToString().Trim();
                    this.txtNombreNiv4.Text = dr.GetValue(dr.GetOrdinal("NNV4Z0")).ToString().Trim();

                    string validarZonas = dr.GetValue(dr.GetOrdinal("UTILZ0")).ToString().Trim();
                    if (validarZonas == "S") this.radToggleSwitchValidarZonas.Value = true;
                    else this.radToggleSwitchValidarZonas.Value = false;
                }

                dr.Close();

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();

                this.TiposDeAuxiliar();

                //Verifica si la clase de zona se utiliza en alguna zona
                if (this.ClaseZonaExisteZona())
                {
                    this.txtEstNivel1.Enabled = false;
                    this.txtEstNivel2.Enabled = false;
                    this.txtEstNivel3.Enabled = false;
                    this.txtEstNivel4.Enabled = false;

                    if (this.txtEstNivel1.Text == "0") this.txtNombreNiv1.Enabled = false;
                    else this.txtNombreNiv1.Enabled = true;
                    if (this.txtEstNivel2.Text == "0") this.txtNombreNiv2.Enabled = false;
                    else this.txtNombreNiv2.Enabled = true;
                    if (this.txtEstNivel3.Text == "0") this.txtNombreNiv3.Enabled = false;
                    else this.txtNombreNiv3.Enabled = true;
                    if (this.txtEstNivel4.Text == "0") this.txtNombreNiv4.Enabled = false;
                    else this.txtNombreNiv4.Enabled = true;
                }
                else
                {
                    this.txtEstNivel1.Enabled = true;
                    this.txtEstNivel2.Enabled = true;
                    this.txtEstNivel3.Enabled = true;
                    this.txtEstNivel4.Enabled = true;

                    this.txtNombreNiv1.Enabled = true;
                    this.txtNombreNiv2.Enabled = true;
                    this.txtNombreNiv3.Enabled = true;
                    this.txtNombreNiv4.Enabled = true;
                }
            }
            catch (Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Valida que no exista el código de la Clase de Zona
        /// </summary>
        /// <returns></returns>
        private bool CodigoClaseZonaValido()
        {
            bool result = false;

            try
            {
                string codClaseZona = this.txtCodigo.Text.Trim();

                if (codClaseZona != "")
                {
                    string query = "select count(CLASZ0) from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                    query += "where CLASZ0 = '" + codClaseZona + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica si la clase de zona se utiliza en alguna zona
        /// </summary>
        /// <returns></returns>
        private bool ClaseZonaExisteZona()
        {
            bool result = false;
            try
            {
                string query = "select count(CLASZ1) from " + GlobalVar.PrefijoTablaCG + "GLM11 ";
                query += "where CLASZ1 = '" + this.codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

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
                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "- El nombre de la clase de zona no puede estar en blanco \n\r";      //Falta traducir
                    this.txtNombre.Focus();
                }

                string resultValidarEstructura = this.ValidarEstructura();
                if (resultValidarEstructura != "")
                {
                    errores += "- " + resultValidarEstructura + "\n\r";
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
        /// Dar de alta a una clase de zona
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string nombre = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;

                string estNivel1 = this.txtEstNivel1.Text.Trim() == "" ? "0" : this.txtEstNivel1.Text;
                string estNivel2 = this.txtEstNivel2.Text.Trim() == "" ? "0" : this.txtEstNivel2.Text;
                string estNivel3 = this.txtEstNivel3.Text.Trim() == "" ? "0" : this.txtEstNivel3.Text;
                string estNivel4 = this.txtEstNivel4.Text.Trim() == "" ? "0" : this.txtEstNivel4.Text;
                string estructura = estNivel1 + estNivel2 + estNivel3 + estNivel4;

                string nombreEstNivel1 = this.txtNombreNiv1.Text.Trim() == "" ? " " : this.txtNombreNiv1.Text;
                string nombreEstNivel2 = this.txtNombreNiv2.Text.Trim() == "" ? " " : this.txtNombreNiv2.Text;
                string nombreEstNivel3 = this.txtNombreNiv3.Text.Trim() == "" ? " " : this.txtNombreNiv3.Text;
                string nombreEstNivel4 = this.txtNombreNiv4.Text.Trim() == "" ? " " : this.txtNombreNiv4.Text;

                string validarZona = "N";
                if (this.radToggleSwitchValidarZonas.Value) validarZona = "S";

                string estado = "";
                if (this.radToggleSwitchEstadoActiva.Value) estado = "V";
                else estado = "*";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM10";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATZ0, CLASZ0, NOMBZ0, ESTRZ0, NNV1Z0, NNV2Z0, NNV3Z0, NNV4Z0, UTILZ0) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtNombre.Text + "', " + estructura;
                query += ", '" + nombreEstNivel1 + "', '" + nombreEstNivel2 + "', '" + nombreEstNivel3 + "', '";
                query += nombreEstNivel4 + "', '" + validarZona + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM10", this.codigo, null);

                //Insertarlo como propietario del elemento
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
        /// Actualizar una clase de zona
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string nombre = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;

                string estNivel1 = this.txtEstNivel1.Text.Trim() == "" ? "0" : this.txtEstNivel1.Text;
                string estNivel2 = this.txtEstNivel2.Text.Trim() == "" ? "0" : this.txtEstNivel2.Text;
                string estNivel3 = this.txtEstNivel3.Text.Trim() == "" ? "0" : this.txtEstNivel3.Text;
                string estNivel4 = this.txtEstNivel4.Text.Trim() == "" ? "0" : this.txtEstNivel4.Text;
                string estructura = estNivel1 + estNivel2 + estNivel3 + estNivel4;

                string nombreEstNivel1 = this.txtNombreNiv1.Text.Trim() == "" ? " " : this.txtNombreNiv1.Text;
                string nombreEstNivel2 = this.txtNombreNiv2.Text.Trim() == "" ? " " : this.txtNombreNiv2.Text;
                string nombreEstNivel3 = this.txtNombreNiv3.Text.Trim() == "" ? " " : this.txtNombreNiv3.Text;
                string nombreEstNivel4 = this.txtNombreNiv4.Text.Trim() == "" ? " " : this.txtNombreNiv4.Text;

                string validarZona = "N";
                if (this.radToggleSwitchValidarZonas.Value) validarZona = "S";

                string estado = "";
                if (this.radToggleSwitchEstadoActiva.Value) estado = "V";
                else estado = "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM10 set ";
                query += "STATZ0 = '" + estado + "', NOMBZ0 = '" + nombre + "', ";
                query += "ESTRZ0 = " + estructura + ", NNV1Z0 = '" + nombreEstNivel1 + "', NNV2Z0 = '" + nombreEstNivel2 + "', ";
                query += "NNV3Z0 = '" + nombreEstNivel3 + "', NNV4Z0 = '" + nombreEstNivel4 + "', UTILZ0 = '" + validarZona + "' ";
                query += "where CLASZ0 = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM10", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string ValidarEstructura()
        {
            string result = "";

            string estNivel1 = this.txtEstNivel1.Text.Trim() == "" ? "0" : this.txtEstNivel1.Text;
            string estNivel2 = this.txtEstNivel2.Text.Trim() == "" ? "0" : this.txtEstNivel2.Text;
            string estNivel3 = this.txtEstNivel3.Text.Trim() == "" ? "0" : this.txtEstNivel3.Text;
            string estNivel4 = this.txtEstNivel4.Text.Trim() == "" ? "0" : this.txtEstNivel4.Text;

            string nombreEstNivel1 = this.txtNombreNiv1.Text.Trim();
            string nombreEstNivel2 = this.txtNombreNiv2.Text.Trim();
            string nombreEstNivel3 = this.txtNombreNiv3.Text.Trim();
            string nombreEstNivel4 = this.txtNombreNiv4.Text.Trim();

            //------------------- Valida que no sea 0 la estructura -------------------
            if (estNivel1 == "0" && estNivel2 == "0" && estNivel3 == "0" && estNivel4 == "0")
            {
                result = "La estructura no puede ser cero";     //Falta traducir
                this.txtEstNivel1.Focus();
                return (result);
            }

            //------------------- Valida que no existan ceros intercalados -------------------
            if (estNivel2 != "0" && estNivel1 == "0")
            {
                result = "No se admiten ceros intercalados";     //Falta traducir
                this.txtEstNivel1.Focus();
                return (result);
            }
            if (estNivel3 != "0" && estNivel2 == "0")
            {
                result = "No se admiten ceros intercalados";     //Falta traducir
                this.txtEstNivel2.Focus();
                return (result);
            }
            if (estNivel4 != "0" && estNivel3 == "0")
            {
                result = "No se admiten ceros intercalados";     //Falta traducir
                this.txtEstNivel2.Focus();
                return (result);
            }

            //------------------- Valida que la estructura no sume más de 8 ------------------- 
            int estNiv1 = Convert.ToInt16(this.txtEstNivel1.Text);
            int estNiv2 = Convert.ToInt16(this.txtEstNivel2.Text);
            int estNiv3 = Convert.ToInt16(this.txtEstNivel3.Text);
            int estNiv4 = Convert.ToInt16(this.txtEstNivel4.Text);
            if ((estNiv1 + estNiv2 + estNiv3 + estNiv4) > 8)
            {
                result = "La estructura no puede sumar más de 8";   //Falta traducir
                this.txtEstNivel1.Focus();
                return (result);
            }

            //------------------- Valida que si está indicado un nivel de estructura, exista su nombre -------------------
            if (estNivel1 != "0" && nombreEstNivel1 == "")
            {
                result = "El nombre no puede estar en blanco";     //Falta traducir
                this.txtNombreNiv1.Focus();
                return (result);
            }

            if (estNivel2 != "0" && nombreEstNivel2 == "")
            {
                result = "El nombre no puede estar en blanco";     //Falta traducir
                this.txtNombreNiv2.Focus();
                return (result);
            }

            if (estNivel3 != "0" && nombreEstNivel3 == "")
            {
                result = "El nombre no puede estar en blanco";     //Falta traducir
                this.txtNombreNiv3.Focus();
                return (result);
            }

            if (estNivel4 != "0" && nombreEstNivel4 == "")
            {
                result = "El nombre no puede estar en blanco";     //Falta traducir
                this.txtNombreNiv4.Focus();
                return (result);
            }

            //------------------- Valida que si existe el nombre del nivel de estructura, exista su nivel -------------------
            if (nombreEstNivel1 != "" && estNivel1 == "0")
            {
                result = "El nombre tiene que estar en blanco. No existe nivel de estructura definido";     //Falta traducir
                this.txtNombreNiv1.Focus();
                return (result);
            }

            if (nombreEstNivel2 != "" && estNivel1 == "0")
            {
                result = "El nombre tiene que estar en blanco. No existe nivel de estructura definido";     //Falta traducir
                this.txtNombreNiv2.Focus();
                return (result);
            }

            if (nombreEstNivel3 != "" && estNivel3 == "0")
            {
                result = "El nombre tiene que estar en blanco. No existe nivel de estructura definido";     //Falta traducir
                this.txtNombreNiv3.Focus();
                return (result);
            }

            if (nombreEstNivel4 != "" && estNivel4 == "0")
            {
                result = "El nombre tiene que estar en blanco. No existe nivel de estructura definido";     //Falta traducir
                this.txtNombreNiv4.Focus();
                return (result);
            }

            return (result);
        }

        /// <summary>
        /// Carga la información de los tipos de auxiliar que utilizan la clase de zona
        /// </summary>
        private void CargarInfoTiposAux()
        {
            try
            {
                string query = "select TAUXMT, NOMBMT, AZMT, CLS1MT, CLS2MT, CLS3MT, CLS4MT, STATMT from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where CLS1MT = '" + this.codigo + "' or CLS2MT = '" + this.codigo + "' or CLS3MT = '" + this.codigo;
                query += "' or CLS4MT = '" + this.codigo + "' ";
                query += "order by TAUXMT";

                this.dtTiposAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Cambiar la columna AZMT de tipo
                DataTable dtCloned = this.dtTiposAux.Clone();
                dtCloned.Columns["AZMT"].DataType = typeof(String);

                if (this.dtTiposAux != null && this.dtTiposAux.Rows.Count > 0)
                {
                    DataRow row;
                    string estructura = "";
                    string valorCampoEstado = "";
                    for (int i = 0; i < this.dtTiposAux.Rows.Count; i++)
                    {
                        row = dtCloned.NewRow();

                        valorCampoEstado = this.dtTiposAux.Rows[i]["STATMT"].ToString();
                        if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                        else valorCampoEstado = this.estadoActivo;

                        row["STATMT"] = valorCampoEstado;
                        row["TAUXMT"] = this.dtTiposAux.Rows[i]["TAUXMT"].ToString();
                        row["NOMBMT"] = this.dtTiposAux.Rows[i]["NOMBMT"].ToString();
                        estructura = this.dtTiposAux.Rows[i]["AZMT"].ToString().PadRight(4, '0');
                        estructura = estructura.Substring(0, 1) + separadorEstructura + estructura.Substring(1, 1) + separadorEstructura + estructura.Substring(2, 1) + separadorEstructura + estructura.Substring(3, 1); ;
                        row["AZMT"] = estructura;

                        row["CLS1MT"] = this.dtTiposAux.Rows[i]["CLS1MT"].ToString();
                        row["CLS2MT"] = this.dtTiposAux.Rows[i]["CLS2MT"].ToString();
                        row["CLS3MT"] = this.dtTiposAux.Rows[i]["CLS3MT"].ToString();
                        row["CLS4MT"] = this.dtTiposAux.Rows[i]["CLS4MT"].ToString();

                        dtCloned.Rows.Add(row);
                    }

                    this.radGridViewTipoAux.DataSource = dtCloned;
                    this.RadGridViewHeader();

                    this.lblNoExistenTiposAux.Visible = false;
                    this.radGridViewTipoAux.Visible = true;

                    if (this.radGridViewTipoAux.Rows != null && this.radGridViewTipoAux.Rows.Count > 0)
                    {
                        //this.tgBuscadorTiposComp.Enabled = true;
                        //this.tgTiposComp.Visible = true;

                        for (int p = 0; p < this.radGridViewTipoAux.Columns.Count; p++)
                        {
                            this.radGridViewTipoAux.Columns[p].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewTipoAux.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewTipoAux.MasterTemplate.BestFitColumns();
                    }
                    else
                    {
                    }
                }
                else
                {
                    this.lblNoExistenTiposAux.Visible = true;
                    this.radGridViewTipoAux.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida que en la estructura se entren los números permitidos (dígito del 0 al 8)
        /// </summary>
        /// <param name="e"></param>
        private void CaracterNumericoEstructura(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar >= 48 && e.KeyChar <= 56)                 //Número entre 0 y 8
                    e.Handled = false;
                else e.Handled = true;
            }
            else
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else e.Handled = true;
        }

        /// <summary>
        /// Grabar una clase de zona
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

                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar una clase de zona
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la clase de zona " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en la tabla de zonas
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                    query += "where CLS1MT = '" + this.codigo + "' or CLS2MT = '" + this.codigo + "' ";
                    query += "or CLS3MT = '" + this.codigo + "' or CLS4MT = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en zonas
                        mensaje = "No es posible eliminar la Clase de zona porque está en uso en algunas de las Zonas.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el grupo de cuenta de auxiliar
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                        query += "where CLASZ0 = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLM10", this.codigo, null);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar la Clase de zona.";
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
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            //Cerrar el formulario
                            this.Close();
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

        private void TiposDeAuxiliar()
        {
            //if (this.gbTipoAux.Visible == false)
            //{
                Cursor.Current = Cursors.WaitCursor;
                //Tipos de auxiliar
                //this.dtTiposAux.Rows.Clear();

                //Llenar el DataGrid
                this.CargarInfoTiposAux();

                this.radGridViewTipoAux.ClearSelection();

                if (this.dtTiposAux.Rows.Count == 0)
                {
                    this.lblNoExistenTiposAux.Visible = true; 
                    this.radGridViewTipoAux.Visible = false;
                }
                else
                {
                    this.lblNoExistenTiposAux.Visible = false;
                    this.radGridViewTipoAux.Visible = true;
                }

                Cursor.Current = Cursors.Default;
            /*}
            else
            {
                this.gbTipoAux.Visible = false;
            }*/
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtEstNivel1.Tag = this.txtEstNivel1.Text; 
            this.txtEstNivel2.Tag = this.txtEstNivel2.Text;
            this.txtEstNivel3.Tag = this.txtEstNivel3.Text;
            this.txtEstNivel4.Tag = this.txtEstNivel4.Text;
            this.txtNombreNiv1.Tag = this.txtNombreNiv1.Text;
            this.txtNombreNiv2.Tag = this.txtNombreNiv2.Text;
            this.txtNombreNiv3.Tag = this.txtNombreNiv3.Text;
            this.txtNombreNiv4.Tag = this.txtNombreNiv4.Text;

            if (this.radToggleSwitchValidarZonas.Value) this.radToggleSwitchValidarZonas.Tag = true;
            else this.radToggleSwitchValidarZonas.Tag = false;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;
            this.txtNombre.Tag = "";
            this.txtEstNivel1.Tag = "0";
            this.txtEstNivel2.Tag = "0";
            this.txtEstNivel3.Tag = "0";
            this.txtEstNivel4.Tag = "0";
            this.txtNombreNiv1.Tag = "";
            this.txtNombreNiv2.Tag = "";
            this.txtNombreNiv3.Tag = "";
            this.txtNombreNiv4.Tag = "";

            this.radToggleSwitchValidarZonas.Tag = true;
        }

        private void CentrarForm()
        {
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundHeight = Screen.PrimaryScreen.Bounds.Height;
            int x = boundWidth - this.Width;
            int y = boundHeight - this.Height;
            this.Location = new Point(x / 2, y / 2);
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                campo = "STATMT";
                header = "Estado";
                this.displayNames.Add(campo, header);

                campo = "TAUXMT";
                header = "Tipo Auxiliar";
                this.displayNames.Add(campo, header);

                campo = "NOMBMT";
                header = "Nombre";
                this.displayNames.Add(campo, header);

                campo = "AZMT";
                header = "Estructura";
                this.displayNames.Add(campo, header);

                campo = "CLS1MT";
                header = "Clase Zona 1";
                this.displayNames.Add(campo, header);

                campo = "CLS2MT";
                header = "Clase Zona 2";
                this.displayNames.Add(campo, header);

                campo = "CLS3MT";
                header = "Clase Zona 3";
                this.displayNames.Add(campo, header);

                campo = "CLS4MT";
                header = "Clase Zona 4";
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewTipoAux.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewTipoAux.Columns.Contains(item.Key)) this.radGridViewTipoAux.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        private void frmMtoGLM10_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void txtEstNivel1_Leave(object sender, EventArgs e)
        {
            if (txtEstNivel1.Text.Trim() != "" && txtEstNivel1.Text.Trim() != "0")
            {
                txtNombreNiv1.Enabled = true;
            }
            else
            {
                txtNombreNiv1.Enabled = false;
                txtNombreNiv1.Text = "";
            }
        }

        private void txtEstNivel2_Leave(object sender, EventArgs e)
        {
            if (txtEstNivel2.Text.Trim() != "" && txtEstNivel2.Text.Trim() != "0")
            {
                txtNombreNiv2.Enabled = true;
            }
            else
            {
                txtNombreNiv2.Enabled = false;
                txtNombreNiv2.Text = "";
            }   
        }

        private void txtEstNivel3_Leave(object sender, EventArgs e)
        {
            if (txtEstNivel3.Text.Trim() != "" && txtEstNivel3.Text.Trim() != "0")
            {
                txtNombreNiv3.Enabled = true;
            }
            else
            {
                txtNombreNiv3.Enabled = false;
                txtNombreNiv3.Text = "";
            }
        }

        private void txtEstNivel4_Leave(object sender, EventArgs e)
        {
            if (txtEstNivel4.Text.Trim() != "" && txtEstNivel4.Text.Trim() != "0")
            {
                txtNombreNiv4.Enabled = true;
            }
            else
            {
                txtNombreNiv4.Enabled = false;
                txtNombreNiv4.Text = "";
            }
        }
    }
}
