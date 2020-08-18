using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModFinanzas
{
    public partial class frmAddOpcionEspecial : frmPlantilla, IReLocalizable
    {
        private LanguageProvider _LPForm;
        public LanguageProvider LPForm
        {
            get
            {
                return (_LPForm);
            }
            set
            {
                this._LPForm = value;
            }
        }

        private string _nombre;
        public string Nombre
        {
            get
            {
                return (_nombre);
            }
            set
            {
                this._nombre = value;
            }
        }

        private string _descripcion;
        public string Descripcion
        {
            get
            {
                return (_descripcion);
            }
            set
            {
                this._descripcion = value;
            }
        }

        private bool _aceptar;
        public bool Aceptar
        {
            get
            {
                return (_aceptar);
            }
            set
            {
                this._aceptar = value;
            }
        }

        public frmAddOpcionEspecial()
        {
            InitializeComponent();

            this._LPForm = new LanguageProvider();

            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmAddOpcionEspecial_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Añadir Opción Especial");

            this.ActiveControl = this.txtNombre;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.FormValid())
            {
                this._nombre = this.txtNombre.Text;
                this._descripcion = this.txtDesc.Text;

                this._aceptar = true;
                this.Close();
            }
            else this._aceptar = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this._aceptar = false;
            this.Close();
        }

        private void frmAddOpcionEspecial_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Añadir Opción Especial");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this._LPForm.GetText("lblfrmAddOpcionEspecial", "Añadir Opción Especial");
            this.grpBoxProceso.Text = " " + this._LPForm.GetText("grpBoxProceso", "Proceso") + " ";
            this.lblNombre.Text = this._LPForm.GetText("lblNombre", "Nombre") + ":";
            this.lblDesc.Text = this._LPForm.GetText("lblDesc", "Descripción") + ":";
            this.btnAceptar.Text = this._LPForm.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this._LPForm.GetText("lblCancelar", "Cancelar");
        }

        private bool FormValid()
        {
            string msgError = this._LPForm.GetText("errValTitulo", "Error");
            if (this.txtNombre.Text.Trim() == "" && this.txtDesc.Text.Trim() == "")
            {
                MessageBox.Show(this._LPForm.GetText("errValNombreDescProceso", "Debe introducir un nombre y una descripción para el proceso"), msgError);
                this.txtNombre.Focus();
                return (false);
            }

            if (this.txtNombre.Text.Trim() == "")
            {
                MessageBox.Show(this._LPForm.GetText("errValNombreProceso", "Debe introducir un nombre para el proceso"), msgError);
                this.txtNombre.Focus();
                return (false);
            }
            if (this.txtDesc.Text.Trim() == "")
            {
                MessageBox.Show(this._LPForm.GetText("errValDescProceso", "Debe introducir una descripción para el proceso"), msgError);
                this.txtDesc.Focus();
                return (false);
            }
            return (true);
        }
        #endregion
    }
}
