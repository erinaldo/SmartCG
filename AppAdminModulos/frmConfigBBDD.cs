using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;

namespace AppAdminModulos
{
    public partial class frmConfigBBDD : frmPlantilla
    {
        public frmConfigBBDD()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmConfigBBDD_Load(object sender, EventArgs e)
        {
            //Llena el combo de tipos
            this.FillTipo();

            //Llena el combo de las vías de acceso posibles
            this.FillTipoAcceso();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTipoAcceso.SelectedIndex != -1)
            {
                if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.SqlClient.ToString())
                {
                    //Si la via de acceso a los datos es SqlClient, el tipo de bbdd tiene que ser SQLServer
                    int actual = 0;
                    bool existe = false;
                    foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
                    {
                        if (tipo == ProveedorDatos.DBTipos.SQLServer)
                        {
                            existe = true;
                            break;
                        }
                        else actual++;
                    }

                    if (existe) this.cmbTipo.SelectedIndex = actual;
                }
            }
        }

        private void cmbTipoAcceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.lblIpNombreServidor.Enabled = true;
                this.txtIpNombreServidor.Enabled = true;
                this.lblNombreBbdd.Enabled = true;
                this.txtNombreBbdd.Enabled = true;

                if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.SqlClient.ToString())
                {
                    //Si la via de acceso a los datos es SqlClient, el tipo de bbdd tiene que ser SQLServer
                    int actual = 0;
                    bool existe = false;
                    foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
                    {
                        if (tipo == ProveedorDatos.DBTipos.SQLServer)
                        {
                            existe = true;
                            break;
                        }
                        else actual++;
                    }

                    if (existe) this.cmbTipo.SelectedIndex = actual;
                }
            }
            else
            {
                this.lblIpNombreServidor.Enabled = false;
                this.txtIpNombreServidor.Enabled = false;
                this.lblNombreBbdd.Enabled = false;
                this.txtNombreBbdd.Enabled = false;
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Llena el combo con los Tipos de bbdd posibles
        /// </summary>
        private void FillTipo()
        {
            int actual = 0;
            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            bool existe = false;

            this.cmbTipo.Items.Clear();

            foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
            {
                if (!existe)
                {
                    if (tipo.ToString() != tipoBaseDatosCG) actual++;
                    else existe = true;
                }
                this.cmbTipo.Items.Add(tipo);
            }

            this.cmbTipo.SelectedIndex = actual;
        }

        /// <summary>
        /// Llena el combo con los Tipos de accesos a las bbdd posibles
        /// </summary>
        private void FillTipoAcceso()
        {
            int actual = 0;
            string proveedorDatosCG = ConfigurationManager.AppSettings["proveedorDatosCG"];
            bool existe = false;

            this.cmbTipoAcceso.Items.Clear();

            foreach (ProveedorDatos.DBProveedores tipoAcceso in Enum.GetValues(typeof(ProveedorDatos.DBProveedores)))
            {
                if (!existe)
                {
                    if (tipoAcceso.ToString() != proveedorDatosCG) actual++;
                    else existe = true;
                }
                this.cmbTipoAcceso.Items.Add(tipoAcceso);
            }

            this.cmbTipoAcceso.SelectedIndex = actual;

            if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.lblIpNombreServidor.Enabled = true;
                this.txtIpNombreServidor.Enabled = true;
                this.lblNombreBbdd.Enabled = true;
                this.txtNombreBbdd.Enabled = true;
            }
            else
            {
                this.lblIpNombreServidor.Enabled = false;
                this.txtIpNombreServidor.Enabled = false;
                this.lblNombreBbdd.Enabled = false;
                this.txtNombreBbdd.Enabled = false;

                //Recuperar los valores de la cadena de conexión
            }
        }
        #endregion
    }
}
