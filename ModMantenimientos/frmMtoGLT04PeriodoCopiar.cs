using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;

namespace ModMantenimientos
{
    public partial class frmMtoGLT04PeriodoCopiar : frmPlantilla, IReLocalizable
    {
        private bool nuevo;
        private string codigo;
        private int situarseAno;

        private DataTable dtCalendario;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public int SituarseAno
        {
            get
            {
                return (this.situarseAno);
            }
            set
            {
                this.situarseAno= value;
            }
        }

        public frmMtoGLT04PeriodoCopiar()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmMtoGLT04PeriodoCopiar_Load(object sender, EventArgs e)
        {
            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales
            this.TraducirLiterales();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMtoGLT04PeriodoCopiar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                toolStripButtonSalir_Click(sender, null);
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoGLT04PeriodoCopiarTitulo", "Mantenimiento de Calendarios Contables - Periodo Copiar");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.toolStripButtonCopiar.Text = this.LP.GetText("toolStripCopiar", "Copiar");
            this.toolStripButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");

        }

        private void toolStripButtonCopiar_Click(object sender, EventArgs e)
        {

        }
        #endregion

    }
}
