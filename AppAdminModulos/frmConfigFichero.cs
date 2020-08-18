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
    public partial class frmConfigFichero : frmPlantilla
    {
        public frmConfigFichero()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmConfigFichero_Load(object sender, EventArgs e)
        {
            string pathFile = ConfigurationManager.AppSettings["PathFicheros"];
            if (pathFile == null || pathFile == "") pathFile = Application.StartupPath + "\\";
            this.txtPath.Text = pathFile;

            string nombreFichero = ConfigurationManager.AppSettings["FicheroModulosGenerado"];
            if (nombreFichero == null || nombreFichero == "") nombreFichero = "ModulosCliente.xml";
            this.txtFicheroGenerado.Text = nombreFichero;

            string claveEncriptar = ConfigurationManager.AppSettings["ClaveEncriptar"];
            if (claveEncriptar == null || claveEncriptar == "") claveEncriptar = "btgsa!admin";
            this.txtClaveEncriptar.Text = claveEncriptar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.isFormValid())
            {
                Utiles utiles = new Utiles();
                //Grabar los valores en las variables del fichero app.config
                utiles.ModificarappSettings("PathFicheros", this.txtPath.Text);
                utiles.ModificarappSettings("FicheroModulosGenerado", this.txtFicheroGenerado.Text);
                utiles.ModificarappSettings("ClaveEncriptar", this.txtClaveEncriptar.Text);
                this.Close();
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Valida el formulario
        /// </summary>
        private bool isFormValid()
        {
            if (this.txtPath.Text == "")
            {
                MessageBox.Show("Debe indicar el path de los ficheros", "Error");
                this.txtPath.Focus();
                return (false);

            }

            if (this.txtFicheroGenerado.Text == "")
            {
                MessageBox.Show("Debe indicar el nombre del fichero que será generado (nombre.extensión)", "Error");
                this.txtFicheroGenerado.Focus();
                return (false);

            }

            if (this.txtClaveEncriptar.Text == "")
            {
                MessageBox.Show("Debe indicar la clave con la que se encriptará el fichero de módulos generados para los clientes", "Error");
                this.txtClaveEncriptar.Focus();
                return (false);
            }

            //PDTE verificar que el path escrito sea correcto
            //Verificar el formato para el fichero generado y para el de log (nombre.extension)

            return (true);
        }
        #endregion
    }
}
