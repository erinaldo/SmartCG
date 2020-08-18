using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ObjectModel;
using System.IO;

namespace SmartCG
{
    public partial class RadFrmAbout : frmPlantilla, IReLocalizable
    {
        public RadFrmAbout()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Acerca de Smart CG");
            try
            {
                var versionSmartCG = Assembly.GetExecutingAssembly().GetName().Version;
                string versionSmartCGValor = versionSmartCG.Major.ToString() + "." + versionSmartCG.Minor.ToString() + "." + versionSmartCG.Build.ToString();

                this.radLabelSmartCGVersionValor.Text = versionSmartCGValor;

                string copyrightSmartCGValor = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
                this.radLabelSmartCGFechaAggity.Text = copyrightSmartCGValor;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            
            //ModuloSII
            bool existeModSII = false;
            try
            {
                string modSIIdll = Application.StartupPath + @"\ModSII.dll";
                if (File.Exists(modSIIdll))
                {
                    //Módulo SII Versión y Copyright
                    Assembly extAssembly = Assembly.LoadFrom(modSIIdll);
                    var versionModSII = extAssembly.GetName().Version;
                    string versionModSIIValor = versionModSII.Major.ToString() + "." + versionModSII.Minor.ToString() + "." + versionModSII.Build.ToString();

                    this.radLabelModSIIVersionValor.Text = versionModSIIValor;
                    string copyrightModSIIValor = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(extAssembly, typeof(AssemblyCopyrightAttribute), false)).Copyright;
                    this.radLabelModSIIFechaAggity.Text = copyrightModSIIValor;
                    existeModSII = true;
                    
                    //Servicio Web Versión
                    string versionWebService = "";
                    try
                    {
                        //Servicio Web Version
                        Type t = extAssembly.GetType("ModSII.frmAbout");
                        dynamic instance = Activator.CreateInstance(t);
                        versionWebService = ((ModSII.frmAbout)instance).WebServiceVersion;
                        this.radLabelModSIIWebServiceVersionValor.Text = versionWebService;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    if (versionWebService == "")
                    {
                        this.radLabelModSIIWebServiceVersion.Visible = false;
                        this.radLabelModSIIWebServiceVersionValor.Visible = false;
                        this.radLabelModSIIFechaAggity.Location = new Point(433, 254);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            
            if (!existeModSII)
            {
                this.radLabelModSII.Visible = false;
                this.radLabelModSIIVersion.Visible = false;
                this.radLabelModSIIVersionValor.Visible = false;
                this.radLabelModSIIWebServiceVersion.Visible = false;
                this.radLabelModSIIWebServiceVersionValor.Visible = false;
                this.radLabelModSIIFechaAggity.Visible = false;
            }
        }

        private void RadButtonAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonAceptar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAceptar);
        }

        private void RadButtonAceptar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAceptar);
        }

        private void FrmAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Acerca de Smart CG");
        }
        #endregion
    }
}