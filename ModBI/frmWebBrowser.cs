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
using Telerik.WinControls;
using System.IO;

namespace ModBI
{
    public partial class frmWebBrowser : frmPlantilla
    {
        private string titulo;
        private string codigo;
        private string url;

        public string Titulo
        {
            get
            {
                return (this.titulo);
            }
            set
            {
                this.titulo = value;
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

        public string URL
        {
            get
            {
                return (this.url);
            }
            set
            {
                this.url = value;
            }
        }

        public frmWebBrowser()
        {
            InitializeComponent();
        }

        #region Eventos

        private void frmWebBrowser_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO WebBrowser " + this.titulo);

            try
            {

                this.radLabelTitulo.Text += titulo;

                this.webB.AllowWebBrowserDrop = false;
                this.webB.IsWebBrowserContextMenuEnabled = false;
                this.webB.WebBrowserShortcutsEnabled = false;
                //this.webB.ObjectForScripting = this;
                //this.url = this.ObtenerURLDadoCodigo(this.codigo);
                this.url = this.ObtenerURLFicheroDadoCodigo(this.codigo);

                if (this.url != "")
                {
                    this.Navigate(this.url);
                }
                else
                {
                    this.webB.Visible = false;
                    this.radLabelError.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void frmWebBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN WebBrowser " + this.titulo);
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

        private string ObtenerURLDadoCodigo(string cod)
        {
            string urlBI = "";
            switch (cod)
            {
                case "Mapa":
                    urlBI = "https://app.powerbi.com/view?r=eyJrIjoiMzE1OTQ5ZWEtMmZlOC00ZGYyLTliMTctOGYzNWM1OTg1NWY5IiwidCI6IjE2NmFkMTFmLTQ2YjQtNDJiOC1hZmJhLTUxOTFiNmUyNzdmMyIsImMiOjl9";
                    break;
                case "DetIngresos":
                    urlBI = "https://app.powerbi.com/view?r=eyJrIjoiMzE1OTQ5ZWEtMmZlOC00ZGYyLTliMTctOGYzNWM1OTg1NWY5IiwidCI6IjE2NmFkMTFmLTQ2YjQtNDJiOC1hZmJhLTUxOTFiNmUyNzdmMyIsImMiOjl9&pageName=ReportSection0b9252b26e22cfa050bf";
                    break;
                case "DetGastos":
                    urlBI = "https://app.powerbi.com/view?r=eyJrIjoiMzE1OTQ5ZWEtMmZlOC00ZGYyLTliMTctOGYzNWM1OTg1NWY5IiwidCI6IjE2NmFkMTFmLTQ2YjQtNDJiOC1hZmJhLTUxOTFiNmUyNzdmMyIsImMiOjl9&pageName=ReportSection2cba2f86cc3e9dba6315";
                    break;
                case "CompGastosIngresos":
                    urlBI = "https://app.powerbi.com/view?r=eyJrIjoiMzE1OTQ5ZWEtMmZlOC00ZGYyLTliMTctOGYzNWM1OTg1NWY5IiwidCI6IjE2NmFkMTFmLTQ2YjQtNDJiOC1hZmJhLTUxOTFiNmUyNzdmMyIsImMiOjl9&pageName=ReportSectionbaccfc65c6dde6deec7b";
                    break;
            }

            return (urlBI);
        }

        private string ObtenerURLFicheroDadoCodigo(string cod)
        {
            string urlBI = "";

            try
            {
                string ficheroBIEnlaces = System.Windows.Forms.Application.StartupPath + "\\tmp\\ModBIEnlaces.xml";

                //Leer el fichero de usuario
                DataSet ds = new DataSet();
                ds.ReadXml(ficheroBIEnlaces);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["BIEnlaces"].Rows.Count > 0)
                {
                    try {
                        urlBI = ds.Tables["BIEnlaces"].Rows[0][cod].ToString().Trim();
                        urlBI = urlBI.Replace("@AMP@", "&");
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (urlBI);
        }
        #endregion
    }
}
