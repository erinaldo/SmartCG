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
    public partial class frmAccesoDirectoEliminar : frmPlantilla
    {
        private DataSet _dsAD;

        //handler y evento que se lanzarán cuando se ejecuta la acción Aceptar 
        //viajen desde el user control hacia el formulario
        public delegate void OkFormCommandEventHandler(OkFormCommandEventArgs e);
        public event OkFormCommandEventHandler OkAccesoDirectoEliminar;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class OkFormCommandEventArgs
        {
            public string ADNombre { get; protected set; }
            public int ADIndice { get; protected set; }
            public OkFormCommandEventArgs(string nombre, int indice)
            {
                this.ADNombre = nombre;
                this.ADIndice = indice;
            }
        }

        public DataSet AccesosDirectos
        {
            get
            {
                return (this._dsAD);
            }
            set
            {
                this._dsAD = value;
            }
        }

        public frmAccesoDirectoEliminar()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmAccesoDirectoEliminar_Load(object sender, EventArgs e)
        {
            try
            {
                Log.Info("INICIO Eliminar Acceso Directo");

                //Traducir Literales
                this.TraducirLiterales();

                if (this._dsAD.Tables != null && this._dsAD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < this._dsAD.Tables[0].Rows.Count; i++)
                    {
                        this.cmbAccesosDirectos.Items.Add(this._dsAD.Tables[0].Rows[i]["nombre"]);
                    }

                    this.cmbAccesosDirectos.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string textoIdioma = this.LP.GetText("lblConfirEliminarAD", "¿Desea eliminar el acceso directo");
            string mensaje = textoIdioma + " \"" + this.cmbAccesosDirectos.SelectedItem.ToString() + "\"?";
            string titulo = this.LP.GetText("lblConfirmacion", "Confirmación");
            DialogResult result = MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (OkAccesoDirectoEliminar != null)
                {
                    OkAccesoDirectoEliminar(new OkFormCommandEventArgs(this.cmbAccesosDirectos.SelectedItem.ToString(), this.cmbAccesosDirectos.SelectedIndex));
                }
                this.Close();
            }
        }

        private void frmAccesoDirectoEliminar_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Eliminar Acceso Directo");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("frmAccesoDirectoEliminarTitulo", "Eliminar Acceso Directo");

            this.lblNombreEliminarAD.Text = this.LP.GetText("lblNombreEliminarAD", "Nombre");
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }
        #endregion
    }
}
