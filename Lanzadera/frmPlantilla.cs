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
using log4net;
using Telerik.WinControls;

//Para leer procedimientos externos radicados en librerías de windows
using System.Runtime.InteropServices;
using Telerik.WinControls.UI.Localization;

namespace SmartCG
{
    public partial class frmPlantilla : Telerik.WinControls.UI.RadForm, IReLocalizable
    {
        protected LanguageProvider LP;

        protected Utiles utiles;

        protected ILog Log;

        protected DataSet dsModulosApp = null;
        protected DataTable tablaModulos;

        public frmPlantilla()
        {
            ThemeResolutionService.ApplicationThemeName = "Office2013Light";

            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            //Traducir literales propios de la Grid
            RadGridLocalizationProvider.CurrentProvider = new RadGridLocalizationProviderES();
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }

        private void FrmPlantilla_Load(object sender, EventArgs e)
        {
            //Centrar formulario
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Top = (rect.Height / 2) - (this.Height / 2);
            this.Left = (rect.Width / 2) - (this.Width / 2);

            LP = new LanguageProvider();

            utiles = new Utiles();
        }
    }
}
