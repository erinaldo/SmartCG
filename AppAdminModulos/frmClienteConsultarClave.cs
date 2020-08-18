using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppAdminModulos
{
    public partial class frmClienteConsultarClave : frmPlantilla
    {
        public frmClienteConsultarClave()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmClienteConsultarClave_Load(object sender, EventArgs e)
        {
            this.gbInfo.Visible = false;
            this.Height = 200;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.gbInfo.Visible = true;
            this.Height = 654;
        }
        #endregion

        #region Métodos Privados
        #endregion
    }
}
