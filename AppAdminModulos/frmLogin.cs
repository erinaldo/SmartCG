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

namespace AppAdminModulos
{
    public partial class frmLogin : frmPlantilla
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Comprobar si la conexión es vía ODBC para activar los controles relacionados con la DSN
            string proveedorDatosCG = ConfigurationManager.AppSettings["proveedorDatosCG"];
            if (proveedorDatosCG == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.lblDSN.Visible = true;
                this.cmbDSN.Visible = true;

                //Cargar el combo con los nombres de las DSNs
                this.ListODBCsources();
                this.cmbDSN.Focus();
            }
            else
            {
                this.lblDSN.Visible = false;
                this.cmbDSN.Visible = false;
                this.txtUsuario.Focus();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validar usuario y contraseña
            if (this.txtUsuario.Text == "" && this.txtPwd.Text == "")
            {
                MessageBox.Show("Debe introducir un usuario y una contraseña", "Error");
                this.txtUsuario.Focus();
                return;
            }

            //Validar usuario
            if (this.txtUsuario.Text == "")
            {
                MessageBox.Show("Debe introducir un usuario", "Error");
                this.txtUsuario.Focus();
                return;
            }

            //Validar contraseña
            if (this.txtPwd.Text == "")
            {
                MessageBox.Show("Debe introducir una contraseña", "Error");
                this.txtPwd.Focus();
                return;
            }

            //Validar que el usuario y la contraseña sean correctos
            //Hacer una conexión de pruebas a la bbdd
            proveedorDatos = null;
            try
            {
                //Leer configuración del Proveedor de Datos
                string proveedorConfig = ConfigurationManager.AppSettings["proveedorDatosCG"];
                ProveedorDatos.DBProveedores tipoProveedorDatos = (ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), proveedorConfig);

                //Leer configuración del Tipo de Base de Datos
                string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                ProveedorDatos.DBTipos tipoBaseDatos = (ProveedorDatos.DBTipos)Enum.Parse(typeof(ProveedorDatos.DBTipos), proveedorTipo);

                //Crear un objeto Proveedor de Datos
                proveedorDatos = new ProveedorDatos(tipoProveedorDatos);

                //Coger la cadena de conexión
                string cadConexion = ConfigurationManager.AppSettings["cadenaConexionCG"];

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
                            cadenaNueva += "DSN=" + this.cmbDSN.Text + ";";
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

                cadConexion = cadConexion.Replace("@USER", this.txtUsuario.Text);
                cadConexion = cadConexion.Replace("@PWD", this.txtPwd.Text);

                proveedorDatos.ConnectionString = cadConexion;
                proveedorDatos.TipoBaseDatos = tipoBaseDatos;

                proveedorDatos.OpenConnection();

                if (proveedorDatos.GetConnectionValue.State == ConnectionState.Open)
                {
                    //proveedorDatos.CloseConnection();

                    this.Close();
                    this.Hide();
                    
                    frmPrincipal frmPcpal = new frmPrincipal();
                    frmPcpal.ShowDialog();
                }
                else
                {
                    if (proveedorDatos != null) proveedorDatos.CloseConnection();
                    MessageBox.Show("Las credenciales no son válidas", "Error");
                    this.txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error validando las credenciales (" + ex.Message + ")", "Error");
                this.txtUsuario.Focus();
                if (proveedorDatos != null) proveedorDatos.CloseConnection();
            }
        }
        #endregion

        #region Métodos Privados
        private void ListODBCsources()
        {
            Utiles utiles = new Utiles();

            ArrayList odbcArray = utiles.ListODBCsources();

            this.cmbDSN.DataSource = odbcArray;

            //Buscar si la DSN está inicializada
            //Coger la cadena de conexión
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

            if (this.cmbDSN.Items.Count > 0 && valorDSN != "")
            {
                this.cmbDSN.Text = valorDSN.ToUpper();
            }
        }
        #endregion
    }
}
