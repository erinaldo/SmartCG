using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using log4net;
using System.Configuration;

namespace ModImpuestos
{
    public partial class frmPlantilla : Form
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        protected UtilesCGConsultas utilesCG;

        protected Autorizaciones aut;

        
        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
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

        private void frmPlantilla_Load(object sender, EventArgs e)
        {
            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();

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
    }
}
