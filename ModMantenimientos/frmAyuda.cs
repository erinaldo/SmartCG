using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModMantenimientos
{
    public partial class frmAyuda : frmPlantilla
    {
        private string tituloForm;
        private string titulo;
        private string descripcion;

        #region Properties
        public string TituloForm
        {
            get
            {
                return (this.tituloForm);
            }
            set
            {
                this.tituloForm = value;
            }
        }

        public string Titulo
        {
            get
            {
                return (this.titulo);
            }
            set
            {
                this.titulo = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return (this.descripcion);
            }
            set
            {
                this.descripcion = value;
            }
        }
        #endregion


        public frmAyuda()
        {
            InitializeComponent();
            this.lblTitulo.Text = "";
            this.txtDescripcion.Text = "";

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        private void frmAyuda_Load(object sender, EventArgs e)
        {
            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.Text = "   " + this.tituloForm;

            this.lblTitulo.Text = this.titulo;

            this.txtDescripcion.Text = this.descripcion;

            this.ActiveControl = this.lblTitulo;    
        }

        private void frmAyuda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                radButtonExit_Click(sender, null);
            }
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Eventos

        #endregion
    }
}
