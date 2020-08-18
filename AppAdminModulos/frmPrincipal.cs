using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace AppAdminModulos
{
    public partial class frmPrincipal : frmPlantilla
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        #region Eventos

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Utiles utiles = new Utiles();

            utiles.DisableCloseButton(this.Handle.ToInt32());
        }

        private void altaSubMenuModulos_Click(object sender, EventArgs e)
        {
            frmModuloAltaEdita frmModuloAlta = new frmModuloAltaEdita();
            frmModuloAlta.NuevoModulo = true;
            frmModuloAlta.ShowDialog();
        }

        private void editarSubMenuModulos_Click(object sender, EventArgs e)
        {
            frmModuloAltaEdita frmModuloEdita = new frmModuloAltaEdita();
            frmModuloEdita.NuevoModulo = false;
            frmModuloEdita.ShowDialog();
        }

        private void activarDesactivarSubMenuModulos_Click(object sender, EventArgs e)
        {
            frmModuloActivarDesactivar frmModuloActivar = new frmModuloActivarDesactivar();
            frmModuloActivar.ShowDialog();
        }

        private void generarFicheroSubMenuModulos_Click(object sender, EventArgs e)
        {
            frmModuloGenerarFichero frmGenerar = new frmModuloGenerarFichero();
            frmGenerar.ShowDialog();
        }

        private void altaSubMenuClientes_Click(object sender, EventArgs e)
        {
            frmClienteAltaEdita frmClienteAlta = new frmClienteAltaEdita();
            frmClienteAlta.NuevoCliente = true;
            frmClienteAlta.ShowDialog();
        }

        private void editarSubMenuClientes_Click(object sender, EventArgs e)
        {
            frmClienteAltaEdita frmClienteAlta = new frmClienteAltaEdita();
            frmClienteAlta.NuevoCliente = false;
            frmClienteAlta.ShowDialog();
        }

        private void generarClavesSubMenuClientes_Click(object sender, EventArgs e)
        {
            frmClienteGenerarClave frmGenerarClave = new frmClienteGenerarClave();
            frmGenerarClave.ShowDialog();
        }

        private void consultarClaveSubMenuClientes_Click(object sender, EventArgs e)
        {
            frmClienteConsultarClave frmConsultarClave = new frmClienteConsultarClave();
            frmConsultarClave.ShowDialog();
        }

        private void listarClavesSubMenuClientes_Click(object sender, EventArgs e)
        {
            frmClienteListarClaves frmListarClave = new frmClienteListarClaves();
            frmListarClave.ShowDialog();
        }

        private void bbbddSubMenuConfig_Click(object sender, EventArgs e)
        {
            frmConfigBBDD frmConfigBbDd = new frmConfigBBDD();
            frmConfigBbDd.ShowDialog();
        }

        private void ficherosSubMenuConfig_Click(object sender, EventArgs e)
        {
            frmConfigFichero frmConfigfichero = new frmConfigFichero();
            frmConfigfichero.ShowDialog();
        }

        private void salirMenuItem_Click(object sender, EventArgs e)
        {
            if (proveedorDatos != null)
            {
                if (proveedorDatos.GetConnectionValue.State == ConnectionState.Open) proveedorDatos.CloseConnection();
            }
            Program.Close();
            //this.Close();
        }
        #endregion


        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnModuloAlta_Click(object sender, EventArgs e)
        {
            frmModuloAltaEdita frmModuloAlta = new frmModuloAltaEdita();
            frmModuloAlta.NuevoModulo = true;
            frmModuloAlta.ShowDialog();
        }

        private void btnModuloEditar_Click(object sender, EventArgs e)
        {
            frmModuloAltaEdita frmModuloEdita = new frmModuloAltaEdita();
            frmModuloEdita.NuevoModulo = false;
            frmModuloEdita.ShowDialog();
        }

        private void btnModuloActivarDesactivar_Click(object sender, EventArgs e)
        {
            frmModuloActivarDesactivar frmModuloActivar = new frmModuloActivarDesactivar();
            frmModuloActivar.ShowDialog();
        }

        private void btnModuloGenerarFicheroModulo_Click(object sender, EventArgs e)
        {
            frmModuloGenerarFichero frmGenerar = new frmModuloGenerarFichero();
            frmGenerar.ShowDialog();
        }
    }
}
