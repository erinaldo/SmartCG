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
    public partial class frmMtoATM05 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOUSUARI";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "024";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "30";

        private const string separadorEstructura = "-";

        private const string UADMMO_Si = "Si";
        private const string UADMMO_No = "No";
        private const string UADMMO_Seguridad = "Seguridad";
        private const string UADMMO_Sistemas = "Sistemas";

        private static string pwdInicialBBDD = "";

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

        public frmMtoATM05()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmMtoATM05_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Usuario Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Inicializar los desplegables
            string[] valores = new string[] { UADMMO_No, UADMMO_Seguridad, UADMMO_Sistemas };
            utiles.CreateRadDropDownListElement(ref this.cmbUserAdminCG, ref valores);

            valores = new string[] { UADMMO_No, UADMMO_Si };
            utiles.CreateRadDropDownListElement(ref this.cmbPantallaSeleccion, ref valores);

            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonAutorizClaseProceso, false);
                utiles.ButtonEnabled(ref this.radButtonAutorizElemento, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();

                this.cmbUserAdminCG.SelectedIndex = 0;
                this.cmbPantallaSeleccion.SelectedIndex = 0;
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información del usuario
                this.CargarInfoUsuario();

                //utiles.ButtonEnabled(ref this.radButtonSave, true);
                utiles.ButtonEnabled(ref this.radButtonAutorizClaseProceso, true);
                utiles.ButtonEnabled(ref this.radButtonAutorizElemento, true);


                this.ActiveControl = this.txtPwd;
                this.txtPwd.Select(0, 0);
                this.txtPwd.Focus();
            }
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.nuevo)
            {
                string codUsuario = this.txtCodigo.Text;

                if (codUsuario == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    MessageBox.Show("Usuario obligatorio", this.LP.GetText("errValUsuario", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codUsuarioOk = true;
                if (this.nuevo) codUsuarioOk = this.CodigoUsuarioValido();    //Verificar que el codigo no exista

                if (codUsuarioOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActivo.Value = true;
                        //this.radToggleSwitchEstadoActivo.Enabled = false;
                        this.radToggleSwitchEstadoActivo.Enabled = true;
                    }

                    this.txtCodigo.IsReadOnly = true;
                    this.codigo = codUsuario;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    MessageBox.Show("Usuario ya existe", this.LP.GetText("errValCodUsuarioExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        private void frmMtoATM05_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtPwd.Text.Trim() != this.txtPwd.Tag.ToString().Trim() ||
                    this.txtNombre.Text != this.txtNombre.Tag.ToString() ||
                    this.radSpinEditorNumPwdUnicas.Text != this.radSpinEditorNumPwdUnicas.Tag.ToString() ||
                    this.radSpinEditorDiasValidezPwd.Text != this.radSpinEditorDiasValidezPwd.Tag.ToString() ||
                    this.radSpinEditorNumMaxDiasInactividad.Text != this.radSpinEditorNumMaxDiasInactividad.Tag.ToString() ||
                    this.cmbUserAdminCG.Text.ToString().Trim() != this.cmbUserAdminCG.Tag.ToString().Trim() ||
                    this.cmbPantallaSeleccion.Text.ToString().Trim() != this.cmbPantallaSeleccion.Tag.ToString().Trim()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Usuario Alta/Edita");
        }

        private void frmMtoATM05_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.radButtonExit_Click(sender, null);
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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoATM05TituloALta", "Mantenimiento de Usuario - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT03TituloEdit", "Mantenimiento de Usuario - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblUsuario.Text = this.LP.GetText("lblATM05CodUsuario", "Usuario");
            this.lblPwd.Text = this.LP.GetText("lblATM05Pwd", "Contraseña");
            this.lblNumPwdUnicas.Text = this.LP.GetText("lblATM05NumPwdUnicas", "Número de contraseñas únicas");
            this.lblDiasValidezPwd.Text = this.LP.GetText("lblATM05NombreNiv2", "Días de validez de la contraseña");
            this.lblNumMaxDiasInactividad.Text = this.LP.GetText("lblATM05NumMaxDiasInac", "Número máximo de días de inactividad");
            this.lblUserAdminCG.Text = this.LP.GetText("lblATM05UserAdminCG", "Usuario administrador CG");
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActivo.Enabled = valor;
            this.txtPwd.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.radSpinEditorNumPwdUnicas.Enabled = valor;
            this.radSpinEditorDiasValidezPwd.Enabled = valor;
            this.radSpinEditorNumMaxDiasInactividad.Enabled = valor;
            this.cmbUserAdminCG.Enabled = valor;
            this.cmbPantallaSeleccion.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El tipo de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActivo.Enabled = false;
            this.txtPwd.Enabled = false;
            this.txtNombre.Enabled = false;
            this.radSpinEditorNumPwdUnicas.Enabled = false;
            this.radSpinEditorDiasValidezPwd.Enabled = false;
            this.radSpinEditorNumMaxDiasInactividad.Enabled = false;
            this.cmbUserAdminCG.Enabled = false;
            this.cmbPantallaSeleccion.Enabled = false;
        }

        /// <summary>
        /// Rellena los controles con los datos del usuario (modo edición)
        /// </summary>
        private void CargarInfoUsuario()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "where IDUSMO = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string valorAuxStr = "";
                int valorAux = 0;

                if (dr.Read())
                {
                    string estado = dr.GetValue(dr.GetOrdinal("STATMO")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                    else this.radToggleSwitchEstadoActivo.Value = false;

                    pwdInicialBBDD = dr.GetValue(dr.GetOrdinal("PASWMO")).ToString().Trim();
                    this.txtPwd.Text = pwdInicialBBDD;

                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBMO")).ToString().Trim();

                    valorAuxStr = dr.GetValue(dr.GetOrdinal("NCONMO")).ToString().Trim();
                    if (valorAuxStr != "0")
                        try
                        {
                            valorAux = Convert.ToInt16(valorAuxStr);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.radSpinEditorNumPwdUnicas.Value = valorAux;

                    valorAuxStr = dr.GetValue(dr.GetOrdinal("DVALMO")).ToString().Trim();
                    if (valorAuxStr != "0")
                        try
                        {
                            valorAux = Convert.ToInt16(valorAuxStr);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.radSpinEditorDiasValidezPwd.Value = valorAux;

                    valorAuxStr = dr.GetValue(dr.GetOrdinal("MDIAMO")).ToString().Trim();
                    if (valorAuxStr != "0")
                        try
                        {
                            valorAux = Convert.ToInt16(valorAuxStr);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.radSpinEditorNumMaxDiasInactividad.Value = valorAux;

                    string UADMMO = dr.GetValue(dr.GetOrdinal("UADMMO")).ToString().Trim();
                    switch (UADMMO)
                    {
                        case "1":   //Seguridad
                            this.cmbUserAdminCG.SelectedIndex = 1;
                            break;
                        case "2":   //Sistemas
                            this.cmbUserAdminCG.SelectedIndex = 2;
                            break;
                        default:    //No
                            this.cmbUserAdminCG.SelectedIndex = 0;
                            break;
                    }

                    string WSIZMO = dr.GetValue(dr.GetOrdinal("WSIZMO")).ToString().Trim();
                    switch (WSIZMO)
                    {
                        case "1":   //Si
                            this.cmbPantallaSeleccion.SelectedIndex = 1;
                            break;
                        default:    //No
                            this.cmbPantallaSeleccion.SelectedIndex = 0;
                            break;
                    }
                }

                dr.Close();

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
        /// Valida que no exista el código del usuario
        /// </summary>
        /// <returns></returns>
        private bool CodigoUsuarioValido()
        {
            bool result = false;

            try
            {
                string codUsuario = this.txtCodigo.Text.Trim();

                if (codUsuario != "")
                {
                    string query = "select count(IDUSMO) from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                    query += "where IDUSMO = '" + codUsuario + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
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
                if (this.txtPwd.Text.Trim() != "" && this.txtPwd.Text != pwdInicialBBDD)
                {
                    string validarPwd = this.ValidarEdicionPwd(this.txtPwd.Text);
                    if (validarPwd != "")
                    {
                        //RadMessageBox.Show(validarPwd, this.LP.GetText("errValTitulo", "Error"));
                        errores += validarPwd;
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
        /// Rutina que valida que la contraseña sea válida
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string ValidarEdicionPwd(string pwd)
        {
            string result = "";

            try
            {
                pwd = pwd.Trim();           //DUDA
                if (pwd == "")
                {
                    result = "La contraseña no puede estar en blanco";
                    return (result);
                }

                if (pwd.Length < 6)
                {
                    result = "La contraseña tiene que tener al menos 6 caracteres";
                    return (result);
                }

                if (pwd.Length > 10)
                {
                    result = "La contraseña no puede tener más de 10 caracteres";
                    return (result);
                }

                int pos = 0;
                bool digitExist = false;
                foreach (char c in pwd)
                {
                    if (pos == 0)
                    {
                        //Comprobar que el 1er digito sea alfanumerico
                        if (!Char.IsLetter(c))
                        {
                            result = "El primer caracter de la contraseña tiene que ser alfabético";
                            return (result);
                        }
                    }
                    if (Char.IsDigit(c))
                    {
                        digitExist = true;
                        break;
                    }
                    pos++;
                }
                //Comprobar que al menos tenga 1 digito
                if (!digitExist)
                {
                    result = "La contraseña debe contener como mínimo 1 dígito numérico";
                    return (result);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Dar de alta a un usuario
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                /////////////////
                string pwd = this.txtPwd.Text.Trim();
                if (this.txtPwd.Text.Trim() != "")
                {
                    //pwd = this.txtPwd.Text.Trim();
                    switch (tipoBaseDatosCG)
                    {
                        case "DB2":
                        case "SQLServer":
                            byte[] pwdEncriptado = new byte[10];
                            pwdEncriptado = utiles.EncriptarPwd(pwd, tipoBaseDatosCG);
                            //pwd = Encoding.Default.GetString(pwdEncriptado); //esto no funciona bien!!!!!! se cambia a lo siguiente:
                            pwd = "";
                            for (int i = 0; i < 10; i++)
                                pwd = pwd + Convert.ToChar(pwdEncriptado[i]).ToString();

                            //actualizaPwd = true;
                            break;
                        case "Oracle":
                            string pwdEncrypt = utiles.ObtenerPwdEncriptadoOracle(pwd);
                            //if (pwdEncrypt == contrasena) result = true;
                            pwd = pwdEncrypt;
                            //actualizaPwd = true;
                            break;
                    }
                }
                else if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) pwd = pwd.PadRight(10, ' ');

                ///////////////

                //string pwd = "";
                //if (this.txtPwd.Text.Trim() != "")
                //{
                //    byte[] aux = utiles.EncriptarPwd(pwd, GlobalVar.ConexionCG.TipoBaseDatos.ToString());
                //    pwd = Encoding.Default.GetString(aux);
                //}
                //else if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) pwd = pwd.PadRight(10, ' ');

                string nombre = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;

                string estado = "";
                if (this.radToggleSwitchEstadoActivo.Value) estado = "V";
                else estado = "*";

                string USRFMO = " ";
                string INLOMO = " ";
                string JOBNMO = " ";
                string TMSTMO = "0";
                string WSIZMO = this.cmbPantallaSeleccion.SelectedIndex.ToString();
                string TCONMO = "0";
                string UADMMO = this.cmbUserAdminCG.SelectedIndex.ToString();

                string nombreTabla = GlobalVar.PrefijoTablaCG + "ATM05";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATMO, IDUSMO, NOMBMO, PASWMO, USRFMO, INLOMO, JOBNMO, TMSTMO, WSIZMO, NCONMO, ";
                query += "DVALMO, TCONMO, MDIAMO, UADMMO) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtNombre.Text + "', '" + pwd;
                query += "', '" + USRFMO + "', '" + INLOMO + "', '" + JOBNMO + "', ";
                query += TMSTMO + ", '" + WSIZMO + "', " + this.radSpinEditorNumPwdUnicas.Value;
                query += ", " + this.radSpinEditorDiasValidezPwd.Value + ", " + TCONMO + ", ";
                query += this.radSpinEditorNumMaxDiasInactividad.Value + ", '" + UADMMO + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (registros == 1)
                {
                    utiles.ButtonEnabled(ref this.radButtonAutorizClaseProceso, true);
                    utiles.ButtonEnabled(ref this.radButtonAutorizElemento, true);
                }

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM05", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar un usuario
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                bool actualizaPwd = false;

                /////////////////
                string pwd = this.txtPwd.Text.Trim();
                if (this.txtPwd.Text.Trim() != "" && this.txtPwd.Text != pwdInicialBBDD)
                {
                    actualizaPwd = true;
                    //pwd = this.txtPwd.Text.Trim();
                    switch (tipoBaseDatosCG)
                    {
                        case "DB2":
                        case "SQLServer":
                            byte[] pwdEncriptado = new byte[10];
                            pwdEncriptado = utiles.EncriptarPwd(pwd, tipoBaseDatosCG);
                            //pwd = Encoding.Default.GetString(pwdEncriptado); //esto no funciona bien!!!!!! se cambia a lo siguiente:
                            pwd = "";
                            for (int i = 0; i < 10; i++)
                                pwd = pwd + Convert.ToChar(pwdEncriptado[i]).ToString();

                            actualizaPwd = true;
                            break;
                        case "Oracle":
                            string pwdEncrypt = utiles.ObtenerPwdEncriptadoOracle(pwd);
                            //if (pwdEncrypt == contrasena) result = true;
                            pwd = pwdEncrypt;
                            actualizaPwd = true;
                            break;
                    }
                }
                else if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) pwd = pwd.PadRight(10, ' ');
                ///////////////

                //string pwd = "";
                //if (this.txtPwd.Text.Trim() != "" && this.txtPwd.Text != pwdInicialBBDD)
                //{
                //    actualizaPwd = true;
                //    pwd = this.txtPwd.Text.Trim();
                //    byte[] aux = utiles.EncriptarPwd(pwd, GlobalVar.ConexionCG.TipoBaseDatos.ToString());
                //    pwd = Encoding.Default.GetString(aux);
                //}
                //else if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) pwd = pwd.PadRight(10, ' ');

                string nombre = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;

                string estado = "";
                if (this.radToggleSwitchEstadoActivo.Value) estado = "V";
                else estado = "*";

                string UADMMO = this.cmbUserAdminCG.SelectedIndex.ToString();
                string WSIZMO = this.cmbPantallaSeleccion.SelectedIndex.ToString();

                string query = "update " + GlobalVar.PrefijoTablaCG + "ATM05 set ";
                query += "STATMO = '" + estado + "', NOMBMO = '" + nombre + "', ";
                if (actualizaPwd) query += "PASWMO = '" + pwd + "', ";
                query += "NCONMO = " + this.radSpinEditorNumPwdUnicas.Value.ToString();
                query += ", DVALMO = " + this.radSpinEditorDiasValidezPwd.Value.ToString();
                query += ", MDIAMO = " + this.radSpinEditorNumMaxDiasInactividad.Value.ToString();
                query += ", WSIZMO = '" + this.cmbPantallaSeleccion.SelectedIndex.ToString();
                query += "', UADMMO = '" + this.cmbUserAdminCG.SelectedIndex.ToString();
                query += "' where IDUSMO = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "ATM05", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        
        /// <summary>
        /// Grabar un usuario
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //if (this.FormValid())
            //{
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtCodigo.Text.Trim();
                    }
                }
                else result = this.ActualizarInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Cerrar el formulario
                    //this.Close();         //Este formulario no se cerrará para que puedan entrar las autorizaciones si lo desean
                    this.Close();

                    utiles.ButtonEnabled(ref this.radButtonAutorizClaseProceso, true);
                    utiles.ButtonEnabled(ref this.radButtonAutorizElemento, true);
                }
            //}

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtPwd.Tag = this.txtPwd.Text;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.radSpinEditorNumPwdUnicas.Tag = this.radSpinEditorNumPwdUnicas.Value.ToString();
            this.radSpinEditorDiasValidezPwd.Tag = this.radSpinEditorDiasValidezPwd.Value.ToString();
            this.radSpinEditorNumMaxDiasInactividad.Tag = this.radSpinEditorNumMaxDiasInactividad.Value.ToString();
            this.cmbUserAdminCG.Tag = this.cmbUserAdminCG.Text;
            this.cmbPantallaSeleccion.Tag = this.cmbPantallaSeleccion.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActivo.Tag = true;
            this.txtPwd.Tag = "";
            this.txtNombre.Tag = "";
            this.radSpinEditorNumPwdUnicas.Tag = "0";
            this.radSpinEditorDiasValidezPwd.Tag = "0";
            this.radSpinEditorNumMaxDiasInactividad.Tag = "0";
            this.cmbUserAdminCG.Tag = "No";
            this.cmbPantallaSeleccion.Tag = "No";
        }
        #endregion

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            if (this.FormValid())
            {
                this.Grabar();

                ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs();
                args.Codigo = this.codigo;
                if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
                else args.Operacion = OperacionMtoTipo.Modificar;
                DoUpdateDataForm(args);
            }
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CentrarForm()
        {
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundHeight = Screen.PrimaryScreen.Bounds.Height;
            int x = boundWidth - this.Width;
            int y = boundHeight - this.Height;
            this.Location = new Point(x / 2, y / 2);
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void radButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void radButtonAutorizaciones_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAutorizClaseProceso);
        }

        private void radButtonAutorizaciones_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAutorizClaseProceso);
        }

        private void radSpinEditorNumPwdUnicas_TextChanged(object sender, EventArgs e)
        {
            var value = decimal.Parse(this.radSpinEditorNumPwdUnicas.Text);
            if (value > this.radSpinEditorNumPwdUnicas.Maximum)
            {
                Telerik.WinControls.RadMessageBox.Show("El valor es mayor que 99", "Error");
            }
        }

        private void radSpinEditorDiasValidezPwd_TextChanged(object sender, EventArgs e)
        {
            var value = decimal.Parse(this.radSpinEditorDiasValidezPwd.Text);
            if (value > this.radSpinEditorDiasValidezPwd.Maximum)
            {
                Telerik.WinControls.RadMessageBox.Show("El valor es mayor que 999", "Error");
            }
        }

        private void radSpinEditorNumMaxDiasInactividad_TextChanged(object sender, EventArgs e)
        {
            var value = decimal.Parse(this.radSpinEditorNumMaxDiasInactividad.Text);
            if (value > this.radSpinEditorNumMaxDiasInactividad.Maximum)
            {
                Telerik.WinControls.RadMessageBox.Show("El valor es mayor que 999", "Error");
            }
        }

        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void radButtonAutorizClaseProceso_Click(object sender, EventArgs e)
        {
            frmMtoATM05Autorizaciones frmMtoUserAutoriz = new frmMtoATM05Autorizaciones();
            frmMtoUserAutoriz.Nuevo = this.nuevo;
            frmMtoUserAutoriz.Codigo = this.codigo;
            frmMtoUserAutoriz.FrmPadre = this;
            frmMtoUserAutoriz.Show(this);
        }

        private void radButtonAutorizElemento_Click(object sender, EventArgs e)
        {
            frmMtoAutElementos frmMtoAutorizElementos = new frmMtoAutElementos();
            frmMtoAutorizElementos.CodigoUsuario = this.codigo;
            frmMtoAutorizElementos.FrmPadre = this;
            frmMtoAutorizElementos.Show(this);
        }

        private void frmMtoATM05_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}