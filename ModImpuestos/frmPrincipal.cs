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

namespace ModImpuestos
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo de Impuestos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmAbout frmInfo = new frmAbout();
            frmInfo.FrmPadre = this;
            frmInfo.Show();

            Cursor.Current = Cursors.Default;
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) menuItemSalir_Click(sender, null);
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo de Impuestos");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo de Impuestos");
        }
        #endregion
    }
}
