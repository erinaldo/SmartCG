using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmWebBrowser : frmPlantilla
    {
        private string documento;

        public string Documento
        {
            get
            {
                return (this.documento);
            }
            set
            {
                this.documento = value;
            }
        }

        public frmWebBrowser()
        {
            InitializeComponent();
        }

        #region Eventos
        private void FrmWebBrowser_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Visor de Informes");

            this.webB.AllowWebBrowserDrop = false;
            this.webB.IsWebBrowserContextMenuEnabled = false;
            this.webB.WebBrowserShortcutsEnabled = false;
            //this.webB.ObjectForScripting = this;
            if (this.documento != "") this.webB.DocumentText = this.documento;
            //this.Navigate("www.google.com");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            //Directorio donde se almacenan los comprobantes contables
            string pathFicherosInformes = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"];

            this.radSaveFileDialogGuardar = new Telerik.WinControls.UI.RadSaveFileDialog
            {

                //Recuperar el directorio por defecto que está en la configuarción
                InitialDirectory = pathFicherosInformes,
                DefaultExt = "html",
                Filter = "ficheros html (*.html)|*.html"
            };

            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            if (DialogResult.OK == this.radSaveFileDialogGuardar.ShowDialog())
            {
                //Grabar físicamente el fichero (el contenido del documento en un nuevo fichero html)

                try
                {
                    string fichero = this.radSaveFileDialogGuardar.FileName;

                    System.IO.StreamWriter sw = new System.IO.StreamWriter(fichero);
                    sw.WriteLine(this.documento);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    RadMessageBox.Show("Error grabando el fichero (" + ex.Message + ")");
                }

                //FALTA !!!!
                //Informar que se grabo correctamente y cerrar el informe ?
            }
            else
            {
            }
        }

        private void FrmWebBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Visor de Informes");
        }

        private void BtnGrabar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnGrabar);
        }

        private void BtnGrabar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnGrabar);
        }

        private void BtnCancel_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnCancel);
        }

        private void BtnCancel_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnCancel);
        }
        #endregion

        #region Métodos Privados

        // Navigates to the given URL if it is valid.
        private void Navigate(String address)
        {
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                this.webB.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }
        #endregion

    }
}
