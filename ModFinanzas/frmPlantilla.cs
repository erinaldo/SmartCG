using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ObjectModel;
using log4net;

namespace ModFinanzas
{
    public partial class frmPlantilla : Form, IReLocalizable
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre
        /// </summary>
        public Form FrmPadre
        {
            get
            {
                return (this._frmPadre);
            }
            set
            {
                this._frmPadre = value;
            }
        }

        public frmPlantilla()
        {
            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }

        private void frmPlantilla_Load(object sender, EventArgs e)
        {
            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            //Centrar formulario
            if (this._frmPadre == null)
            {
                //Centrar formulario respecto a la pantalla completa
                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Top = (rect.Height / 2) - (this.Height / 2);
                this.Left = (rect.Width / 2) - (this.Width / 2);
            }
            else
            {
                //Centrar el formulario respecto al formulario padre
                utiles.CentrarFormHijo(this._frmPadre, this);
            }
        }

        /// <summary>
        /// Activa o desactiva un nodo del fichero XML (MenuFinanzas.xml)
        /// </summary>
        /// <param name="activar">activar o desactivar (true/false)</param>
        /// <param name="idNodo">identificador del Nodo Padre</param>
        /// <param name="baseNodeList">Nodo Base</param>
        public void ActivateDesactivateNodeToXML(bool activar, string idNodo, XmlNodeList baseNodeList)
        {
            string idNode;
            foreach (XmlNode xmlnode in baseNodeList)
            {
                idNode = "";
                if (xmlnode.Attributes["id"] != null)
                {
                    idNode = xmlnode.Attributes["id"].Value;
                }
                if (idNode == idNodo)
                {
                    xmlnode.Attributes["activo"].Value = activar == true ? "1" : "0";
                    return;
                }
                else
                {
                    if (xmlnode.HasChildNodes)
                    {
                        this.ActivateDesactivateNodeToXML(activar, idNodo, xmlnode.ChildNodes);
                    }
                }
            }
        }

    }
}
