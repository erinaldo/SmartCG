using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModPagoProv
{
    public partial class frmPrincipal : Form, IReLocalizable
    {
        protected LanguageProvider LP;

        public frmPrincipal()
        {
            InitializeComponent();

            GlobalVar.ProveedorDatosCG = ProveedorDatos.DBProveedores.Odbc.ToString();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            LP = new LanguageProvider();

            this.TraducirLiterales();
        }

        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }


        private void TraducirLiterales()
        {
            this.label1.Text = this.LP.GetText("lblTest", "Esto es una prueba");
        }
    }
}
