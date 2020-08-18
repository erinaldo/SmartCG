using System;
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
    public partial class frmLogin : frmPlantilla, IReLocalizable
    {

        //Leer configuración del Proveedor de Datos
        string proveedorConfig = ConfigurationManager.AppSettings["proveedorDatosCG"];

        //Leer configuración del Tipo de Base de Datos
        string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

        bool sqlServerWindowsAut = false;

        public frmLogin()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string msgError = this.LP.GetText("errValTitulo", "Error");

            //Leer configuración de la cadena de conexión
            string cadConexion = ConfigurationManager.AppSettings["cadenaConexionCG"];

            //Validar usuario y contraseña
            /*if ((proveedorTipo == "DB2" && proveedorConfig == "Odbc") ||
                (proveedorTipo == "DB2" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1) ||
                (proveedorTipo == "SQLServer" && proveedorConfig == "OleDb") ||
                (proveedorTipo == "Oracle" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1)
               )*/
            if ((proveedorTipo == "DB2" && proveedorConfig == "Odbc") ||
                (proveedorTipo == "DB2" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1) ||
                (proveedorTipo == "SQLServer" && proveedorConfig == "OleDb" && !this.sqlServerWindowsAut) ||
                (proveedorTipo == "Oracle" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1)
                )
            {
                if (this.radTextBoxControlUser.Text == "" && this.radTextBoxControlPwd.Text == "")
                {
                    MessageBox.Show(this.LP.GetText("errValUsuarioPwd", "Debe introducir un usuario y una contraseña"), msgError);
                    this.radTextBoxControlUser.Focus();
                    return;
                }

                //Validar usuario
                if (this.radTextBoxControlUser.Text == "")
                {
                    MessageBox.Show(this.LP.GetText("errValUsuario", "Debe introducir un usuario"), msgError);
                    this.radTextBoxControlUser.Focus();
                    return;
                }

                //Validar contraseña
                if (this.radTextBoxControlPwd.Text == "")
                {
                    MessageBox.Show(this.LP.GetText("errValPwd", "Debe introducir una contraseña"), msgError);
                    this.radTextBoxControlPwd.Focus();
                    return;
                } 
            }

            //Validar que el usuario y la contraseña sean correctos
            //Hacer una conexión de pruebas a la bbdd
            ProveedorDatos proveedorDatos = null;
            try
            {
                //Leer configuración del Proveedor de Datos
                ///string proveedorConfig = ConfigurationManager.AppSettings["proveedorDatosCG"];
                ProveedorDatos.DBProveedores tipoProveedorDatos = (ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), proveedorConfig);

                //Leer configuración del Tipo de Base de Datos
                ///string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                ProveedorDatos.DBTipos tipoBaseDatos = (ProveedorDatos.DBTipos)Enum.Parse(typeof(ProveedorDatos.DBTipos), proveedorTipo);

                //Crear un objeto Proveedor de Datos
                proveedorDatos = new ProveedorDatos(tipoProveedorDatos);

                if (proveedorConfig == ProveedorDatos.DBProveedores.Odbc.ToString())
                {
                    //En la cadena de conexión, copiar el valor de la DSN al parámetro DSN
                    string[] cadConexionAux = cadConexion.Split(';');
                    string cadenaNueva = "";
                    string[] cadenaValor;
                    for (int i = 0; i < cadConexionAux.Length; i++)
                    {
                        cadenaValor = cadConexionAux[i].ToString().Split('=');

                        if (cadenaValor[0].ToUpper() == "DSN")
                        {
                            cadenaNueva += "DSN=" + this.radDropDownListDSN.Text + ";";
                        }
                        else cadenaNueva += cadConexionAux[i] + ";";
                    }

                    if (cadenaNueva != "")
                    {
                        if (cadenaNueva.Substring(cadenaNueva.Length - 1, 1) == ";")
                        {
                            cadenaNueva = cadenaNueva.Substring(0, cadenaNueva.Length - 1);
                        }
                        //Escribir la variable con esta nueva DSN en el fichero de configuración
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                        config.AppSettings.Settings["cadenaConexionCG"].Value = cadenaNueva;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                        cadConexion = cadenaNueva;
                    }
                }

                if (proveedorTipo == "DB2") this.radTextBoxControlPwd.Text = this.radTextBoxControlPwd.Text.ToUpper();

                //if (proveedorTipo == "DB2" || proveedorConfig != "Odbc")
                if ((proveedorTipo == "DB2" && proveedorConfig == "Odbc") ||
                    (proveedorTipo == "DB2" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1) ||
                    (proveedorTipo == "SQLServer" && proveedorConfig == "OleDb") ||
                    (proveedorTipo == "Oracle" && proveedorConfig == "OleDb" && cadConexion.IndexOf("@PWD") != -1)
                   )
                {
                
                    cadConexion = cadConexion.Replace("@USER", this.radTextBoxControlUser.Text);
                    cadConexion = cadConexion.Replace("@PWD", this.radTextBoxControlPwd.Text);
                }
                else
                {
                    cadConexion = cadConexion.Replace("@USER", "");
                    cadConexion = cadConexion.Replace("@PWD", "");
                }

                proveedorDatos.ConnectionString = cadConexion;
                proveedorDatos.TipoBaseDatos = tipoBaseDatos;

                proveedorDatos.OpenConnection();

                if (proveedorDatos.GetConnectionValue.State == ConnectionState.Open)
                {
                    //No cerrar la conexión para poder ser utilizada por los demás módulos
                    //proveedorDatos.CloseConnection();
                    GlobalVar.CadenaConexionCG = cadConexion;
                    Log.Info("Usuario conectado al servidor: " + this.radTextBoxControlUser.Text);
                    GlobalVar.UsuarioLogadoCG_BBDD = this.radTextBoxControlUser.Text;

                    GlobalVar.UsuarioLogadoCG = "";

                    GlobalVar.ConexionCG = proveedorDatos;

                    //Coger Prefijo de Tablas para CG
                    string prefijoTablaCG = ConfigurationManager.AppSettings["prefijoTablaCG"];
                    if (prefijoTablaCG != null && prefijoTablaCG != "")
                    {
                        if (proveedorTipo == "DB2") GlobalVar.PrefijoTablaCG = prefijoTablaCG + ".";
                        else GlobalVar.PrefijoTablaCG = prefijoTablaCG;
                    }
                    else GlobalVar.PrefijoTablaCG = "";

                    //Cargar los valores de los parámetros generales de CG
                    CGParametrosGrles.CargarParametrosGenerales(GlobalVar.ConexionCG);

                    //Actualizar los datos del usuario
                    GlobalVar.UsuarioEnv.UserNameServidor = this.radTextBoxControlUser.Text;

                    //montar entorno para DB2
                    if (proveedorTipo == "DB2")
                    {
                        /* crear entorno en AS400*/
                        string bbddCGAPP = ConfigurationManager.AppSettings["bbddCGAPP"];
                        //string comando = "CALL PGM(" + bbddCGAPP + "/CG024) PARM(CGDATASII X)";
                        string bibliotecaEntorno = GlobalVar.EntornoActivo.NombreBBDD;
                        string comando = "CALL PGM(" + bbddCGAPP + "/CG024) PARM(" + bibliotecaEntorno + " X)";
                        string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";
                        string longitudComando = comando.Length.ToString();

                        sentencia = sentencia + longitudComando.PadLeft(10, '0');
                        sentencia = sentencia + ".00000)";

                        GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);
                    }

                    //Cerrar formulario de login
                    this.Close();
                }
                else
                {
                    if (proveedorDatos != null) proveedorDatos.CloseConnection();
                    MessageBox.Show(this.LP.GetText("errValCredenciales", "Las credenciales no son válidas"), msgError);
                    this.radTextBoxControlUser.Focus();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                //MessageBox.Show(this.LP.GetText("errValCredExcepcion", "Se ha producido un error validando las credenciales") + " (" + ex.Message + ")", msgError);
                MessageBox.Show(this.LP.GetText("errValCredenciales", "Las credenciales no son válidas"), msgError);
                this.radTextBoxControlUser.Focus();
                if (proveedorDatos != null) proveedorDatos.CloseConnection();
            }

            //Grabar los valores de la última petición
            //if (this.cmbDSN.Visible) utiles.ModificarappSettings("lastDSNContab", this.cmbDSN.Text);
            //utiles.ModificarappSettings("lastUserContab", this.txtUsuario.Text);

            Cursor.Current = Cursors.Default;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Conectar al servidor");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();

            //Comprobar si la conexión es vía ODBC para activar los controles relacionados con la DSN
            string proveedorDatosCG = ConfigurationManager.AppSettings["proveedorDatosCG"];
            if (proveedorDatosCG == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.radLabelDSN.Visible = true;
                this.radDropDownListDSN.Visible = true;

                //Cargar el combo con los nombres de las DSNs
                this.ListODBCsources();
                //this.cmbDSN.Focus();
            }
            else
            {
                this.radLabelDSN.Visible = false;
                this.radDropDownListDSN.Visible = false;
                //this.txtUsuario.Focus();
            }

            //Recuperar los valores de la última petición
            if (this.radDropDownListDSN.Visible)
            {
                //string lastDSNContab = ConfigurationManager.AppSettings["lastDSNContab"];
                string lastDSNContab = GlobalVar.EntornoActivo.UserDSN;
                if (lastDSNContab != null && lastDSNContab != "")
                {
                    try { this.radDropDownListDSN.Text = lastDSNContab; }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }
                }
            }

            //string lastUserContab = ConfigurationManager.AppSettings["lastUserContab"];
            string lastUserContab = GlobalVar.UsuarioEnv.UserNameServidor;
            if (lastUserContab != null && lastUserContab != "")
            {
                this.radTextBoxControlUser.Text = lastUserContab;
                this.radTextBoxControlPwd.Select();
            }
            else this.radTextBoxControlUser.Select();

            /// SMR - No mostrar pantalla de peticion de SQLServer u Oracle en caso de Odbc
            //if (((proveedorTipo == "SQLServer" || proveedorTipo == "Oracle") && proveedorConfig == "Odbc"))
            if (((proveedorTipo == "SQLServer" || proveedorTipo == "Oracle") && proveedorConfig == "Odbc") ||
                (proveedorTipo == "SQLServer" && proveedorConfig == "OleDb"))
            {
                if (proveedorTipo == "SQLServer" && proveedorConfig == "OleDb")
                {
                    //Verificar que la conexión sea con Seguridad de Windows Integrada
                    this.sqlServerWindowsAut = this.SQLServerWindowsAuthentication();

                    if (this.sqlServerWindowsAut) this.AcceptButton.PerformClick();
                }
                else this.AcceptButton.PerformClick();
            }
            else
                if (proveedorConfig == "OleDb" && (proveedorTipo == "DB2" || proveedorTipo == "Oracle"))
                {
                    //Leer configuración de la cadena de conexión
                    string cadConexion = ConfigurationManager.AppSettings["cadenaConexionCG"];

                    if (cadConexion != null && cadConexion != "" && cadConexion.IndexOf("@PWD") == -1)
                    {
                        this.AcceptButton.PerformClick();
                    }
                }
        }

        private bool SQLServerWindowsAuthentication()
        {
            bool result = false;

            //Leer configuración de la cadena de conexión
            string cadConexion = ConfigurationManager.AppSettings["cadenaConexionCG"];

            if (cadConexion != null && cadConexion != "")
            {
                cadConexion = cadConexion.ToUpper();
                string[] cadConexionParametros = cadConexion.Split(';');
                string[] parametro;
                for (int i = 0; i < cadConexionParametros.Length; i++)
                {
                    parametro = cadConexionParametros[i].Split('=');
                    if ((parametro[0].Trim() == "INTEGRATED SECURITY"
                        && (parametro[1].Trim() == "SSPI" || parametro[1].Trim() == "TRUE")) ||
                        (parametro[0].Trim() == "TRUSTED_CONNECTION" && parametro[1].Trim() == "TRUE"))
                        return (true);
                }
            }

            /*
            If you specify either Trusted_Connection=True; or Integrated Security=SSPI; or Integrated Security=true; in your connection string

==> THEN (and only then) you have Windows Authentication happening. Any user id= setting in the connection string will be ignored.

If you DO NOT specify either of those settings,

==> then you DO NOT have Windows Authentication happening (SQL Authentication mode will be used)
             */
            return (result);
        }

        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = "   " + this.LP.GetText("lblfrmLoginTitulo", "Conectar al servidor");
            this.radLabelDSN.Text = this.LP.GetText("lblDSN", "DSN") + ":";
            this.radLabelUser.Text = this.LP.GetText("lblUsuario", "Usuario") + ":";
            this.radLabelPwd.Text = this.LP.GetText("lblPwd", "Contraseña") + ":";
        }

        private void ListODBCsources()
        {
            ArrayList odbcArray = utiles.ListODBCsources();

            this.radDropDownListDSN.DataSource = odbcArray;

            //Buscar si la DSN está inicializada

            //Leer configuración de la cadena de conexión
            string cadConexion = ConfigurationManager.AppSettings["cadenaConexionCG"];
            
            //Buscar si en la cadena de conexión existe una DSN y mostrarla
            string[] cadConexionAux = cadConexion.Split(';');
            string[] cadenaValor;
            string valorDSN = "";
            for (int i = 0; i < cadConexionAux.Length; i++)
            {
                cadenaValor = cadConexionAux[i].ToString().Split('=');

                if (cadenaValor[0].ToUpper() == "DSN")
                {
                    if (cadenaValor.Length == 2) valorDSN = cadenaValor[1];
                    break;
                }
            }

            if (this.radDropDownListDSN.Items.Count > 0 && valorDSN != "")
            {
                this.radDropDownListDSN.Text = valorDSN.ToUpper();
            }

        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Conectar al servidor");
        }
        

        private void RadTextBoxControlUser_Enter(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void RadTextBoxControlUser_Click(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void RadTextBoxControlUser_DoubleClick(object sender, EventArgs e)
        {
            this.radTextBoxControlUser.SelectAll();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.Close();
        }

        private void BtnAceptar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAceptar);
        }

        private void BtnAceptar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAceptar);
        }
    }
}