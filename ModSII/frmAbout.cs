using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;

namespace ModSII
{
    public partial class frmAbout : frmPlantilla
    {
        public string WebServiceVersion = "";
        public frmAbout()
        {
            InitializeComponent();
            this.Text = String.Format("Acerca de {0}", AssemblyTitle);
            this.lblProducto.Text = AssemblyProduct;

            //string versionModSII = AssemblyVersion;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionModSII = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString();

            this.WebServiceVersion = this.GetWebServiceVersion();
            if (WebServiceVersion == "") this.lblVersion.Text = String.Format("Version {0}", versionModSII);
            else this.lblVersion.Text = String.Format("Versión {0}", versionModSII) + "       " + String.Format("Versión Servicio Web {0}", WebServiceVersion);

            this.lblCompania.Text = AssemblyCopyright;
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string GetWebServiceVersion()
        {
            string result = "";

            try
            {
                result = this.serviceSII.WSTGsii.GetApplicationVersion();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
    }
}