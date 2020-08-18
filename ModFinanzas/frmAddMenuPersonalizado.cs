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
    public partial class frmAddMenuPersonalizado : frmPlantilla, IReLocalizable
    {
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

        public frmAddMenuPersonalizado()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmAddMenuPersonalizado_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Añadir Menú Personalizado");

            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.txtDesc.Text.Trim() == "")
            {
                string msgError = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errValDescMenu", "Debe introducir una descripción para el menú"), msgError);
                this.txtDesc.Focus();

                this._aceptar = false;
            }
            else
            {
                this._descripcion = this.txtDesc.Text;
                this._aceptar = true;
                this.Close();
            }
        }

        private void frmAddMenuPersonalizado_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Añadir Menú Personalizado");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmAddMenuPersonalizado", "Añadir Menú Personalizado");
            this.lblDesc.Text = this.LP.GetText("lblDescMenu", "Descripción");
            this.grpBoxGrupo.Text = this.LP.GetText("grpBoxMenu", "Menú"); 
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }
        #endregion        
    }
}