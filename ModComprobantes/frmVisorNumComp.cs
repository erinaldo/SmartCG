using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModComprobantes
{
    public partial class frmVisorNumComp : frmPlantilla, IReLocalizable
    {
        private int extracontable;

        private DataTable dataTable;

        public int Extracontable
        {
            get
            {
                return (this.extracontable);
            }
            set
            {
                this.extracontable = value;
            }
        }


        public DataTable Datos
        {
            get
            {
                return (this.dataTable);
            }
            set
            {
                this.dataTable = value;
            }
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        public frmVisorNumComp()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radGridViewInfo.MasterView.TableSearchRow.IsVisible = false;

            this.dataTable = new DataTable();
        }

        private void FrmVisorNumComp_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Visor de números de comprobantes generados");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //this.dgInfo.DataSource = this.dataTable;
            this.radGridViewInfo.DataSource = this.dataTable;

            this.TraducirLiterales();

            if (this.radGridViewInfo.Rows.Count > 0)
            {
                for (int i = 0; i < this.radGridViewInfo.Columns.Count; i++)
                    this.radGridViewInfo.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                this.radGridViewInfo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewInfo.MasterTemplate.BestFitColumns();
                this.radGridViewInfo.ClearSelection();
            }
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSalir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonSalir_Click(sender, null);
        }

        private void FrmVisorNumComp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Visor de números de comprobantes generados");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            if (this.extracontable == 1) this.Text = this.LP.GetText("lblfrmVisorCompExtContTitulo", "Números de Comprobantes Extracontables Generados");    //Falta traducir y dar de alta la etiqueta en el fichero de idiomas
            else this.Text = this.LP.GetText("lblfrmVisorCompContTitulo", "Números de Comprobantes Generados");    //Falta traducir y dar de alta la etiqueta en el fichero de idiomas

            //Traducir los Literales de los ToolStrip
            this.radButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");

            //Traducir los literales de los encabezados de las columnas
            this.radGridViewInfo.Columns["archivo"].HeaderText = this.LP.GetText("CompContdgHeaderArchivo", "Archivo");
            this.radGridViewInfo.Columns["descripcion"].HeaderText = this.LP.GetText("CompContdgHeaderDescripcion", "Descripción");
            this.radGridViewInfo.Columns["compania"].HeaderText = this.LP.GetText("CompContdgHeaderCompania", "Compañía");
            this.radGridViewInfo.Columns["aapp"].HeaderText = this.LP.GetText("CompContdgHeaderAAPP", "AA-PP");
            this.radGridViewInfo.Columns["tipo"].HeaderText = this.LP.GetText("CompContdgHeaderTipo", "Tipo");
            this.radGridViewInfo.Columns["noComp"].HeaderText = this.LP.GetText("CompContdgHeaderNoComp", "No Comp.");
        }
        #endregion
    }
}
