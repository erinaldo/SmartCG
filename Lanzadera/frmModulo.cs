using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace FinanzasNet
{
    public partial class frmModulo : frmPlantilla, IReLocalizable
    {
        public frmModulo()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            this.Text = this.LP.GetText("lblfrmModuloTitulo", "Gestionar Módulos");
        }
        #endregion

        private void frmModulo_Load(object sender, EventArgs e)
        {

        }

       
    }
}
