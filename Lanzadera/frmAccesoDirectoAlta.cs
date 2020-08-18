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
    public partial class frmAccesoDirectoAlta : frmPlantilla
    {
        //handler y evento que se lanzarán cuando se ejecuta la acción Aceptar 
        //viajen desde el user control hacia el formulario
        public delegate void OkFormCommandEventHandler(OkFormCommandEventArgs e);
        public event OkFormCommandEventHandler OkAccesoDirectoForm;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class OkFormCommandEventArgs
        {
            public string ADNombre { get; protected set; }
            public string ADPrograma { get; protected set; }
            public OkFormCommandEventArgs(string nombre, string programa)
            {
                this.ADNombre = nombre;
                this.ADPrograma = programa;
            }
        }

        public frmAccesoDirectoAlta()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmAccesoDirectoAlta_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Crear Acceso Directo");

            //Traducir Literales
            this.TraducirLiterales();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.ValidarForm())
            {
                if (OkAccesoDirectoForm != null)
                {
                    OkAccesoDirectoForm(new OkFormCommandEventArgs(this.txtADNombre.Text, this.txtADPrograma.Text));
                }

                this.Close();
            }
        }

        private void btnSelPrograma_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = this.LP.GetText("lblSelArchivosAD", "Seleccionar archivos");
            this.openFileDialog1.FileName = "";
            string filtro = this.LP.GetText("lblTipoArchivosExeAD", "Archivos ejecutables") + "|*.exe|" +
                            this.LP.GetText("lblTipoArchivosBatAD", "Archivos de procesamiento por lotes") + "|*.bat";
            //this.openFileDialog1.Filter = "Archivos ejecutables|*.exe|Archivos de procesamiento por lotes|*.bat";
            this.openFileDialog1.Filter = filtro;

            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.txtADPrograma.Text = this.openFileDialog1.FileName;
            }
        }

        private void frmAccesoDirectoAlta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Crear Acceso Directo");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("frmAccesoDirectoAltaTitulo", "Crear Acceso Directo");

            this.lblNombre.Text = this.LP.GetText("lblNombreAD", "Nombre");
            this.lblPrograma.Text = this.LP.GetText("lblProgramaAD", "Programa");
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }

        private bool ValidarForm()
        {
            string msgError = this.LP.GetText("errValTitulo", "Error");
            //Validar nombre y programa
            if (this.txtADNombre.Text == "" && this.txtADPrograma.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValADNomProg", "Debe introducir un nombre y un programa"), msgError);
                this.txtADNombre.Focus();
                return (false);
            }

            //Validar nombre
            if (this.txtADNombre.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValADNombre", "Debe introducir un nombre"), msgError);
                this.txtADNombre.Focus();
                return (false);
            }

            //Validar programa
            if (this.txtADPrograma.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValADPrograma", "Debe introducir un programa"), msgError);
                this.txtADPrograma.Focus();
                return (false);
            }

            return (true);
        }
        #endregion

    }
}
