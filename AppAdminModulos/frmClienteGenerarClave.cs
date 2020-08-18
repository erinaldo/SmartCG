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
    public partial class frmClienteGenerarClave : frmPlantilla
    {
        public frmClienteGenerarClave()
        {
            InitializeComponent();
        }


        #region Eventos
        private void frmClienteGenerarClave_Load(object sender, EventArgs e)
        {
            //Cargar los módulos activos en la lista Modulos Activos (listboxModulosActivos)
            this.FillModulosActivos();
        }

        private void btnAdicionarAll_Click(object sender, EventArgs e)
        {
            int j = 0;
            for (j = 0; j <= this.listboxModulosActivos.Items.Count - 1; j++)
            {
                this.listboxModulosSel.Items.Add(this.listboxModulosActivos.Items[j]);
            }
            this.listboxModulosActivos.Items.Clear();
        }

        private void btnQuitarAll_Click(object sender, EventArgs e)
        {
            int j = 0;
            for (j = 0; j <= this.listboxModulosSel.Items.Count - 1; j++)
            {
                this.listboxModulosActivos.Items.Add(this.listboxModulosSel.Items[j]);
            }
            this.listboxModulosSel.Items.Clear();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (this.listboxModulosActivos.SelectedItem != null)
            {
                this.listboxModulosSel.Items.Add(this.listboxModulosActivos.SelectedItem);
                int i = 0;
                i = this.listboxModulosActivos.SelectedIndex;
                this.listboxModulosActivos.Items.RemoveAt(i);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (this.listboxModulosSel.SelectedItem != null)
            {
                this.listboxModulosActivos.Items.Add(this.listboxModulosSel.SelectedItem);
                int i = 0;
                i = this.listboxModulosSel.SelectedIndex;
                this.listboxModulosSel.Items.RemoveAt(i);
            }
        }

        private void btnGenerarClave_Click(object sender, EventArgs e)
        {
            //Si el resultado es correcto
            this.gbClaveGenerada.Visible = true;
            this.lblClaveGenerada.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Métodos Privados
        private void FillModulosActivos()
        {
            //Cargar los módulos si aún no se ha hecho
            string result = this.CargarModulos();
            if (result != "")
            {
                MessageBox.Show("Error cargando los módulos activos (" + result + ")", "Error");
                return;
            }

            if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0)
            {
                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];
                string moduloDesc = "";
                for (int i = 0; i < modulosTabla.Rows.Count; i++)
                {
                    moduloDesc = modulosTabla.Rows[i]["id"].ToString() + " - " +
                                 modulosTabla.Rows[i]["nombre"].ToString();

                    //Falta insertarlo como nombre, valor
                    this.listboxModulosActivos.Items.Add(moduloDesc);
                }
            }
        }
        #endregion
    }
}
