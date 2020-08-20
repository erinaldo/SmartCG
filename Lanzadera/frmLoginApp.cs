using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using ObjectModel;

namespace SmartCG
{
    public partial class frmLoginApp : frmPlantilla, IReLocalizable
    {
        private string _titulo;
        private string tipoBaseDatosCG;

        private bool verificarClave = false;

        public string Titulo
        {
            get
            {
                return (this._titulo);
            }
            set
            {
                this._titulo = value;
            }
        }

        public bool VerificarClave
        {
            get
            {
                return (this.verificarClave);
            }
            set
            {
                this.verificarClave = value;
            }
        }

        public frmLoginApp()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }
        
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Conectar a la aplicación");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.radTextBoxControlUser.TextBoxElement.ViewElement.Margin = new Padding(10);
            this.radTextBoxControlPwd.TextBoxElement.ViewElement.Margin = new Padding(10);

            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();

            bool claveValida = false;
            if (this.verificarClave)
            {
                //La llamada al formulario de login fue desde la opción del menú a la derecha de la lanzadera
                //validar clave
                frmValidaClave frmValidaClave = new frmValidaClave();
                frmValidaClave.ShowDialog();
                if (frmValidaClave.claveOK == true)
                {
                    claveValida = true;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            else claveValida = true;

            if (claveValida)
            {
                //Recuperar los valores de la última petición
                //string lastUserApp = ConfigurationManager.AppSettings["lastUserApp"];
                string lastUserApp = GlobalVar.UsuarioEnv.UserNameApp;
                if (lastUserApp != null && lastUserApp != "")
                {
                    //this.txtUsuario.Text = lastUserApp;
                    this.radTextBoxControlUser.Text = lastUserApp;
                    this.radTextBoxControlPwd.Select();

                    //************* OJO QUITAR estas dos lineas !!!!!!!!!!!! ************************
                    //this.radTextBoxControlPwd.Text = "ifs";
                    //this.radButtonLogin.PerformClick();
                }
                else this.radTextBoxControlUser.Select();
            }
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Conectar a la aplicación");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            //if (this.tipoBaseDatosCG == "DB2") this.Text = this.LP.GetText("lblfrmLoginAppTituloCG", "Login CG");
            //else this.Text = this.LP.GetText("lblfrmLoginAppTituloUniclass", "Login Uniclass");
            this.Text = "  " + this.LP.GetText("lblfrmLoginAppTituloUniclass", "Conectar a la aplicación");

            this.radLlblUser.Text = this.LP.GetText("lblUsuario", "Usuario");
            this.radLblPwd.Text = this.LP.GetText("lblPwd", "Contraseña");
        }

        /// <summary>
        /// Validar que hayan introducido el usuario y la contraseña
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            string msgError = this.LP.GetText("errValTitulo", "Error");
            //Validar usuario y contraseña
            if (this.radTextBoxControlUser.Text == "" && this.radTextBoxControlPwd.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValUsuarioPwd", "Debe introducir un usuario y una contraseña"), msgError);
                this.radTextBoxControlUser.Focus();
                return (false);
            }

            //Validar usuario
            if (this.radTextBoxControlUser.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValUsuario", "Debe introducir un usuario"), msgError);
                this.radTextBoxControlUser.Focus();
                return (false);
            }

            //Validar contraseña
            if (this.radTextBoxControlPwd.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValPwd", "Debe introducir una contraseña"), msgError);
                this.radTextBoxControlPwd.Focus();
                return (false);
            }

            return (true);
        }

        /// <summary>
        /// Verifica si el login es correcto (si existe el usuario y contraseña encriptada) en la tabla de la bbdd
        /// </summary>
        /// <param name="pwd">Contraseña</param>
        /// <returns></returns>
        private bool LoginOK(string pwd)
        {
            bool result = false;

            /***********************
             * Para SQL Server peta la rutina StringToByte
             * Hace falta saber cómo se hace la comprobación del pwd para SQL Server y Oracle
             * Esta comprobación solo funciona para DB2
             ***********************/
            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            //if (tipoBaseDatosCG == "SQLServer")
            //{
            //    GlobalVar.UsuarioReferenciaCG = this.txtUsuario.Text.ToUpper();
            //    return (true);
            //}

            /*string tipoConexion = ConfigurationManager.AppSettings["proveedorDatosCG"];
            if (tipoBaseDatosCG == "DB2" && tipoConexion == "OleDb")
            {
                GlobalVar.UsuarioReferenciaCG = this.txtUsuario.Text.ToUpper();
                return (true);
            }
            */

            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "where STATMO = 'V' and IDUSMO = '" + this.radTextBoxControlUser.Text.ToUpper() + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    //Recuperar el usuario referencia para el usuario logado
                    string USRFMO = dr["USRFMO"].ToString();
                    if (USRFMO.Trim() == "") GlobalVar.UsuarioReferenciaCG = this.radTextBoxControlUser.Text.ToUpper();
                    else GlobalVar.UsuarioReferenciaCG = USRFMO;

                    string contrasena = dr["PASWMO"].ToString();
                    if (tipoBaseDatosCG != "DB2") contrasena = contrasena.TrimEnd();

                    GlobalVar.UsuarioLogadoCG_Nombre = dr["NOMBMO"].ToString().Trim();
                    GlobalVar.UsuarioLogadoCG_TipoSeguridad = dr["UADMMO"].ToString();

                    dr.Close();

                    switch (tipoBaseDatosCG)
                    {
                        case "DB2":
                        case "SQLServer":
                            byte[] pwdEncriptado = new byte[10];
                            pwdEncriptado = utiles.EncriptarPwd(pwd, tipoBaseDatosCG);
                            byte[] pwdBBDD = utiles.StringToByte(contrasena, tipoBaseDatosCG);
                            if (Encoding.Default.GetString(pwdBBDD) == Encoding.Default.GetString(pwdEncriptado)) result = true;
                            break;
                        case "Oracle":
                            string pwdEncrypt = utiles.ObtenerPwdEncriptadoOracle(pwd);
                            if (pwdEncrypt == contrasena) result = true;
                            break;
                    }
                }
                else
                {
                    GlobalVar.UsuarioReferenciaCG = "";
                    GlobalVar.UsuarioLogadoCG_Nombre = "";
                    GlobalVar.UsuarioLogadoCG_TipoSeguridad = "";
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                GlobalVar.UsuarioReferenciaCG = "";
            }

            return (result);
        }
        #endregion

        private void RadBtnAceptar_Click(object sender, EventArgs e)
        {
            if (this.FormValid())
            {
                //Grabar los valores de la última petición
                //utiles.ModificarappSettings("lastUserApp", this.txtUsuario.Text);

                //Comprobar si el usuario y las credenciales son válidas
                if (this.LoginOK(this.radTextBoxControlPwd.Text))
                {
                    //GlobalVar.UsuarioLogadoCG = this.txtUsuario.Text;
                    GlobalVar.UsuarioLogadoCG = this.radTextBoxControlUser.Text;
                    Log.Info("Usuario conectado a la aplicación: " + this.radTextBoxControlUser.Text);

                    //Actualizar los datos del usuario
                    GlobalVar.UsuarioEnv.UserNameApp = this.radTextBoxControlUser.Text;

                    //Verificar si está abierto el formulario principal del módulo de mantenimientos para refrescar las opciones de menú 
                    //de acuerdo con los permisos para el usuario logado
                    /*foreach (Form frm in Application.OpenForms)
                    {
                        System.Reflection.Assembly a = null;

                        try
                        {
                            //Verificar que exista la dll de ModMantenimientos
                            a = System.Reflection.Assembly.Load("ModMantenimientos");

                            if (frm.GetType() == typeof(ModMantenimientos.frmPrincipal))
                            {
                                //Formulario principal del Módulo de mantenimientos
                                ModMantenimientos.IPrincipalOpcionesMenu formInterface = frm as ModMantenimientos.IPrincipalOpcionesMenu;

                                if (formInterface != null)
                                    formInterface.ActualizaOpcionesMenuPermisos();

                                break;
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        try
                        {
                            //Verificar que exista la dll de ModSII
                            a = System.Reflection.Assembly.Load("ModSII");

                            if (frm.GetType() == typeof(ModSII.frmPrincipal))
                            {
                                //Formulario principal del Módulo del SII
                                ObjectModel.IOpcionesMenu formInterfaceSII = frm as ObjectModel.IOpcionesMenu;

                                if (formInterfaceSII != null)
                                    formInterfaceSII.ActualizaOpcionesMenuPermisos();

                                break;
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    */
                    this.Close();
                }
                else
                {
                    GlobalVar.UsuarioLogadoCG = "";
                    string msgError = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(this.LP.GetText("errValCredenciales", "Las credenciales no son válidas"), msgError);
                    //this.txtUsuario.Focus();
                    this.radTextBoxControlUser.Focus();
                    return;
                }
            }
            else return;
        }

        private void RadTextBoxControlUser_Click(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void RadTextBoxControlUser_DoubleClick(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void RadTextBoxControlUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadTextBoxControlUser_Enter(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void RadTextBoxControlPwd_Click(object sender, EventArgs e)
        {
            this.radTextBoxControlPwd.SelectAll();
        }

        private void RadTextBoxControlPwd_DoubleClick(object sender, EventArgs e)
        {
            this.radTextBoxControlPwd.SelectAll();
        }

        private void RadTextBoxControlPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadTextBoxControlPwd_Enter(object sender, EventArgs e)
        {
            this.radTextBoxControlPwd.SelectAll();
        }

        private void RadButtonLogin_Click(object sender, EventArgs e)
        {
            if (this.FormValid())
            {
                //Grabar los valores de la última petición
                //utiles.ModificarappSettings("lastUserApp", this.txtUsuario.Text);

                //Comprobar si el usuario y las credenciales son válidas
                if (this.LoginOK(this.radTextBoxControlPwd.Text))
                {
                    //GlobalVar.UsuarioLogadoCG = this.txtUsuario.Text;
                    GlobalVar.UsuarioLogadoCG = this.radTextBoxControlUser.Text;
                    Log.Info("Usuario conectado a la aplicación: " + this.radTextBoxControlUser.Text);

                    //Actualizar los datos del usuario
                    GlobalVar.UsuarioEnv.UserNameApp = this.radTextBoxControlUser.Text;

                    //Verificar si está abierto el formulario principal del módulo de mantenimientos para refrescar las opciones de menú 
                    //de acuerdo con los permisos para el usuario logado
                    foreach (Form frm in Application.OpenForms)
                    {
                        System.Reflection.Assembly a = null;

                        //Nueva Lanzadera
                        try
                        {
                            //Verificar que exista el ejecutable de la aplicación
                            a = System.Reflection.Assembly.Load("SmartCG");

                            if (frm.GetType() == typeof(RadFrmMain))
                            {
                                //Formulario principal de la Lanzadera

                                if (frm is IOpcionesMenu formInterface)
                                    formInterface.ActualizaOpcionesMenuPermisos();

                                break;
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    //crear carpeta de usuario si no existe (para configuracion de grids)
                    string carpetaUsuario = Application.StartupPath + @"\app\usuarios\" + GlobalVar.UsuarioEnv.UserNameEnv;
                    try
                    {
                        // Si no existe
                        if (!Directory.Exists(carpetaUsuario))
                        {
                            // Crea carpeta
                            DirectoryInfo di = Directory.CreateDirectory(carpetaUsuario);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    //

                    this.Close();
                }
                else
                {
                    GlobalVar.UsuarioLogadoCG = "";
                    string msgError = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(this.LP.GetText("errValCredenciales", "Las credenciales no son válidas"), msgError);
                    //this.txtUsuario.Focus();
                    this.radTextBoxControlUser.Focus();
                    return;
                }
            }
            else return;
        }

        private void FrmLoginApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.Close();
        }

        private void RadButtonLogin_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonLogin);
        }

        private void RadButtonLogin_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonLogin);
        }
    }
}
