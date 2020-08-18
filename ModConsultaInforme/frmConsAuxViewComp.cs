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
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ModConsultaInforme
{
    public partial class frmConsAuxViewComp :  frmPlantilla, IReLocalizable
    {
        protected bool menuLateralExpanded = true;
        protected static int collapseWidth = 0;

        public string TipoAuxCodigo { get; set; }
        public string TipoAuxDesc { get; set; }
        public string CtaAuxCodigo { get; set; }
        public string CtaAuxDesc { get; set; }
        public string PosAux { get; set; }
        public string CompaniaCodigo { get; set; }
        public string CompaniaDesc { get; set; }
        public string GrupoCodigo { get; set; }
        public string GrupoDesc { get; set; }
        public string PlanCodigo { get; set; }
        public string PlanDesc { get; set; }
        public string AAPPDesde { get; set; }
        public string AAPPDesdeFormat { get; set; }
        public string AAPPHasta { get; set; }
        public string AAPPHastaFormat { get; set; }
        public string CtaMayorCodigo { get; set; }
        public string CtaMayorDesc { get; set; }
        public string Documentos { get; set; }
        public string DatosMonedaExt { get; set; }
        
        public string Clase { get; set; }
        public string NoDocumento { get; set; }

        public string TipoComp { get; set; }
        public string NoComp{ get; set; }

        public bool PlanCamposExtendidos { get; set; }
        public DataTable DtGLMX2 { get; set; }
        
        ArrayList aEmpresas = null;
        
        string calendario = "";

        Dictionary<string, string> dictAuxZonaNombre = new Dictionary<string, string>();

        Dictionary<string, string> displayNamesComp;
        DataTable dtComp;

        public frmConsAuxViewComp()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbCabeceraSel.ElementTree.EnableApplicationThemeName = false;
            this.gbCabeceraSel.ThemeName = "ControlDefault";

            this.gbTotales.ElementTree.EnableApplicationThemeName = false;
            this.gbTotales.ThemeName = "ControlDefault";

            this.gbTotalesME.ElementTree.EnableApplicationThemeName = false;
            this.gbTotalesME.ThemeName = "ControlDefault";

            this.radGridViewComp.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxViewComp_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar Comprobante");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Cargar Menu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            //Muestra la cabecera de la consulta (criterio de selección / saldos / totales)
            this.CargarValoresCabecera();

            //Crear el DataTable para la Grid
            this.BuildDataTableComprobante();

            //Cargar el comprobante
            this.CargarInfoComprobante();
        }

        private void FrmConsAuxViewComp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }
        
        private void RadButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarTelerik();
            //this.GridExportar();
        }

        private void RadBtnMenuMostrarOcultar_Click(object sender, EventArgs e)
        {
            if (this.menuLateralExpanded)
            {
                int sizePanel = this.radPanelAcciones.Size.Width;
                int sizeButton = this.radBtnMenuMostrarOcultar.Width;
                collapseWidth = sizePanel - sizeButton - 4;

                this.menuLateralExpanded = false;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width - collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = true;

                //this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width + collapseWidth, this.radPanelMenuPath.Height);
                //this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X - collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width + collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X - collapseWidth, this.radPanelApp.Location.Y);
            }
            else
            {
                this.menuLateralExpanded = true;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width + collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = false;

                //this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width - collapseWidth, this.radPanelMenuPath.Height);
                //this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X + collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width - collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X + collapseWidth, this.radPanelApp.Location.Y);
            }
        }

        private void RadGridViewComp_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewComp.Columns.Contains("TAUXDT")) this.radGridViewComp.Columns["TAUXDT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("CUENDT")) this.radGridViewComp.Columns["CUENDT"].Width = 80;
            if (this.radGridViewComp.Columns.Contains("NOABMC")) this.radGridViewComp.Columns["NOABMC"].Width = 154;
            if (this.radGridViewComp.Columns.Contains("TAUXDT"))
            {
                this.radGridViewComp.Columns["TAUXDT"].Width = 40;
                this.radGridViewComp.Columns["TAUXDT"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewComp.Columns.Contains("CAUXDT")) this.radGridViewComp.Columns["CAUXDT"].Width = 45;
            if (this.radGridViewComp.Columns.Contains("NOMBMT")) this.radGridViewComp.Columns["NOMBMT"].Width = 120;
            if (this.radGridViewComp.Columns.Contains("ZONAMA")) this.radGridViewComp.Columns["ZONAMA"].Width = 90;
            if (this.radGridViewComp.Columns.Contains("TAAD01"))
            {
                this.radGridViewComp.Columns["TAAD01"].Width = 40;
                this.radGridViewComp.Columns["TAAD01"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewComp.Columns.Contains("AUAD01")) this.radGridViewComp.Columns["AUAD01"].Width = 45;

            if (this.radGridViewComp.Columns.Contains("NOMBMT1")) this.radGridViewComp.Columns["NOMBMT1"].Width = 120;
            if (this.radGridViewComp.Columns.Contains("ZONAMA1")) this.radGridViewComp.Columns["ZONAMA1"].Width = 90;
            if (this.radGridViewComp.Columns.Contains("TAAD02"))
            {
                this.radGridViewComp.Columns["TAAD02"].Width = 40;
                this.radGridViewComp.Columns["TAAD02"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewComp.Columns.Contains("AUAD02")) this.radGridViewComp.Columns["AUAD02"].Width = 45;
            if (this.radGridViewComp.Columns.Contains("NOMBMT2")) this.radGridViewComp.Columns["NOMBMT2"].Width = 120;
            if (this.radGridViewComp.Columns.Contains("ZONAMA2")) this.radGridViewComp.Columns["ZONAMA2"].Width = 90;
            if (this.radGridViewComp.Columns.Contains("TMOVDT")) this.radGridViewComp.Columns["TMOVDT"].Width = 30;
            if (this.radGridViewComp.Columns.Contains("MONTDT")) this.radGridViewComp.Columns["MONTDT"].Width = 100;
            if (this.radGridViewComp.Columns.Contains("DESCAD")) this.radGridViewComp.Columns["DESCAD"].Width = 150;
            if (this.radGridViewComp.Columns.Contains("TPIVCX")) this.radGridViewComp.Columns["TPIVCX"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("TICODT")) this.radGridViewComp.Columns["TICODT"].Width = 45;
            if (this.radGridViewComp.Columns.Contains("NUCODT")) this.radGridViewComp.Columns["NUCODT"].Width = 85;
            if (this.radGridViewComp.Columns.Contains("FECODT")) this.radGridViewComp.Columns["FECODT"].Width = 100;
            if (this.radGridViewComp.Columns.Contains("CCIADT")) this.radGridViewComp.Columns["CCIADT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("TIPLDT")) this.radGridViewComp.Columns["TIPLDT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("CLDODT")) this.radGridViewComp.Columns["CLDODT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("NDOCDT")) this.radGridViewComp.Columns["NDOCDT"].Width = 80;
            if (this.radGridViewComp.Columns.Contains("FDOCDT")) this.radGridViewComp.Columns["FDOCDT"].Width = 90;
            if (this.radGridViewComp.Columns.Contains("FEVEDT")) this.radGridViewComp.Columns["NDOCDT"].Width = 90;
            if (this.radGridViewComp.Columns.Contains("TEINDT")) this.radGridViewComp.Columns["NDOCDT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("PCIFAD")) this.radGridViewComp.Columns["NDOCDT"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("NNITAD")) this.radGridViewComp.Columns["NDOCDT"].Width = 80;
            if (this.radGridViewComp.Columns.Contains("CDDOAD")) this.radGridViewComp.Columns["CDDOAD"].Width = 40;
            if (this.radGridViewComp.Columns.Contains("NDDOAD")) this.radGridViewComp.Columns["NDDOAD"].Width = 80;
            if (this.radGridViewComp.Columns.Contains("MOSMAD"))
            {
                if (this.DatosMonedaExt != "1") this.radGridViewComp.Columns["MOSMAD"].IsVisible = false;
                else
                {
                    this.radGridViewComp.Columns["MOSMAD"].Width = 85;
                    this.radGridViewComp.Columns["MOSMAD"].TextAlignment = ContentAlignment.MiddleRight;
                }
            }
            if (this.radGridViewComp.Columns.Contains("TERCAD"))
            {
                this.radGridViewComp.Columns["TERCAD"].Width = 85;
                this.radGridViewComp.Columns["TERCAD"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewComp.Columns.Contains("CDIVDT")) this.radGridViewComp.Columns["CDIVDT"].Width = 40;

            if (this.PlanCamposExtendidos)
            {
                if (this.radGridViewComp.Columns.Contains("PRFDDX")) this.radGridViewComp.Columns["PRFDDX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("NFAADX")) this.radGridViewComp.Columns["NFAADX"].Width = 160;
                if (this.radGridViewComp.Columns.Contains("NFARDX")) this.radGridViewComp.Columns["NFARDX"].Width = 160;
                if (this.radGridViewComp.Columns.Contains("FIVADX")) this.radGridViewComp.Columns["FIVADX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA1DX")) this.radGridViewComp.Columns["USA1DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA2DX")) this.radGridViewComp.Columns["USA2DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA3DX")) this.radGridViewComp.Columns["USA3DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA4DX")) this.radGridViewComp.Columns["USA4DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA5DX")) this.radGridViewComp.Columns["USA5DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA6DX")) this.radGridViewComp.Columns["USA6DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA7DX")) this.radGridViewComp.Columns["USA7DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USA8DX")) this.radGridViewComp.Columns["USA8DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USN1DX"))
                {
                    this.radGridViewComp.Columns["USN1DX"].Width = 140;
                    this.radGridViewComp.Columns["USN1DX"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewComp.Columns.Contains("USN2DX"))
                {
                    this.radGridViewComp.Columns["USN2DX"].Width = 140;
                    this.radGridViewComp.Columns["USN2DX"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewComp.Columns.Contains("USF1DX")) this.radGridViewComp.Columns["USF1DX"].Width = 140;
                if (this.radGridViewComp.Columns.Contains("USF2DX")) this.radGridViewComp.Columns["USF2DX"].Width = 140;
            }
        }

        private void RadGridViewComp_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement is Telerik.WinControls.UI.GridHeaderCellElement)
            {
                e.CellElement.TextWrap = true;
            }
        }

        private void RadGridViewComp_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadGridViewComp_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewComp, ref this.selectAll);
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonExportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExportar);
        }

        private void RadButtonExportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExportar);
        }

        private void RadButtonSalir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSalir);
        }

        private void RadButtonSalir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSalir);
        }

        private void FrmConsAuxViewComp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Auxiliar Comprobante");
        }

        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemConsultaAuxComp", "Consulta de Auxiliar - Comprobante");   //Falta traducir

            this.gbCabeceraSel.Text = "Criterio de Selección";   //Falta traducir
            this.lblCia.Text = this.LP.GetText("lblCompania", "Compañía");   //Falta traducir
            this.lblAnoPer.Text = this.LP.GetText("lblAAPeriodo", "Año-periodo");   //Falta traducir
            this.lblTipoComp.Text = this.LP.GetText("lblTipoComp", "Tipo de comprobante");   //Falta traducir
            this.lblNoComp.Text = this.LP.GetText("lblNumComp", "Número de comprobante");   //Falta traducir
            this.lblTotalDebe.Text = this.LP.GetText("lblTotalDebe", "Total Debe");   //Falta traducir
            this.lblTotalHaber.Text = this.LP.GetText("lblTotalHaber", "Total Haber");   //Falta traducir

            this.radButtonExportar.Text = this.LP.GetText("lblExportar", "Exportar");   //Falta traducir
            this.radButtonSalir.Text = this.LP.GetText("lblSalir", "Salir");   //Falta traducir

            this.lblResult.Text = this.LP.GetText("lblConsultaAuxCompResult", "No existe o no se ha podido recuperar el comprobante para el criterio de selección indicado");     //Falta traducir
        }

        /// <summary>
        /// Muestra la cabecera dado el movimiento seleccionado
        /// </summary>
        private void CargarValoresCabecera()
        {
            try
            {
                this.lblCiaValor.Text = this.CompaniaDesc;
                this.lblAnoPerValor.Text = this.AAPPDesdeFormat;

                string tico = this.TipoComp;
                if (tico.Trim() != "") tico = tico.PadLeft(2, '0');
                this.lblTipoCompValor.Text = tico;

                string tipoCompDesc = utilesCG.ObtenerDescDadoCodigo("GLT06", "TIVOTV", "NOMBTV", this.TipoComp, false, "").Trim();
                if (tipoCompDesc != "") this.lblTipoCompValor.Text += " " + this.separadorDesc + " " + tipoCompDesc;

                string nuco = this.NoComp;
                if (nuco.Trim() != "") nuco = nuco.PadLeft(5, '0');
                this.lblNoCompValor.Text = nuco;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuildDataTableComprobante()
        {
            try
            {
                this.dtComp = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                this.dtComp.Columns.Add("SAPRDT", typeof(string));
                this.dtComp.Columns.Add("CUENDT", typeof(string));
                this.dtComp.Columns.Add("NOABMC", typeof(string));
                this.dtComp.Columns.Add("TAUXDT", typeof(string));
                this.dtComp.Columns.Add("CAUXDT", typeof(string));
                this.dtComp.Columns.Add("NOMBMT", typeof(string));
                this.dtComp.Columns.Add("ZONAMA", typeof(string));
                this.dtComp.Columns.Add("TAAD01", typeof(string));
                this.dtComp.Columns.Add("AUAD01", typeof(string));
                this.dtComp.Columns.Add("NOMBMT1", typeof(string));
                this.dtComp.Columns.Add("ZONAMA1", typeof(string));
                this.dtComp.Columns.Add("TAAD02", typeof(string));
                this.dtComp.Columns.Add("AUAD02", typeof(string));
                this.dtComp.Columns.Add("NOMBMT2", typeof(string));
                this.dtComp.Columns.Add("ZONAMA2", typeof(string));
                this.dtComp.Columns.Add("TMOVDT", typeof(string));
                this.dtComp.Columns.Add("MONTDT", typeof(string));
                this.dtComp.Columns.Add("DESCAD", typeof(string));
                this.dtComp.Columns.Add("TPIVCX", typeof(string));
                this.dtComp.Columns.Add("TICODT", typeof(string));
                this.dtComp.Columns.Add("NUCODT", typeof(string));
                this.dtComp.Columns.Add("FECODT", typeof(string));
                this.dtComp.Columns.Add("CCIADT", typeof(string));
                this.dtComp.Columns.Add("TIPLDT", typeof(string));
                this.dtComp.Columns.Add("CLDODT", typeof(string));
                this.dtComp.Columns.Add("NDOCDT", typeof(string));
                this.dtComp.Columns.Add("FDOCDT", typeof(string));
                this.dtComp.Columns.Add("FEVEDT", typeof(string));
                this.dtComp.Columns.Add("TEINDT", typeof(string));
                this.dtComp.Columns.Add("PCIFAD", typeof(string));
                this.dtComp.Columns.Add("NNITAD", typeof(string));
                this.dtComp.Columns.Add("CDDOAD", typeof(string));
                this.dtComp.Columns.Add("NDDOAD", typeof(string));
                this.dtComp.Columns.Add("MOSMAD", typeof(string));
                this.dtComp.Columns.Add("TERCAD", typeof(string));
                this.dtComp.Columns.Add("CDIVDT", typeof(string));

                if (this.PlanCamposExtendidos)
                {
                    this.dtComp.Columns.Add("PRFDDX", typeof(string));
                    this.dtComp.Columns.Add("NFAADX", typeof(string));
                    this.dtComp.Columns.Add("NFARDX", typeof(string));
                    this.dtComp.Columns.Add("FIVADX", typeof(string));
                    this.dtComp.Columns.Add("USA1DX", typeof(string));
                    this.dtComp.Columns.Add("USA2DX", typeof(string));
                    this.dtComp.Columns.Add("USA3DX", typeof(string));
                    this.dtComp.Columns.Add("USA4DX", typeof(string));
                    this.dtComp.Columns.Add("USA5DX", typeof(string));
                    this.dtComp.Columns.Add("USA6DX", typeof(string));
                    this.dtComp.Columns.Add("USA7DX", typeof(string));
                    this.dtComp.Columns.Add("USA8DX", typeof(string));
                    this.dtComp.Columns.Add("USN1DX", typeof(string));
                    this.dtComp.Columns.Add("USN2DX", typeof(string));
                    this.dtComp.Columns.Add("USF1DX", typeof(string));
                    this.dtComp.Columns.Add("USF2DX", typeof(string));
                }

                this.radGridViewComp.DataSource = this.dtComp;
                //Escribe el encabezado de la Grid de Comprobantes
                this.BuildDisplayNamesComp();
                this.RadGridViewCompHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Encabezados para la Grid de Comprobantes
        /// </summary>
        private void BuildDisplayNamesComp()
        {
            try
            {
                this.displayNamesComp = new Dictionary<string, string>
                {
                    { "SAPRDT", this.LP.GetText("HeaderAAPP", "AA-PP") },       //Falta traducir
                    { "CUENDT", this.LP.GetText("HeaderCtaMayor", "Cta Mayor") },       //Falta traducir
                    { "NOABMC", this.LP.GetText("HeaderNombreCuenta", "Nombre de la Cuenta") },       //Falta traducir
                    { "TAUXDT", this.LP.GetText("HeaderTA1", "TA-1") },        //Falta traducir
                    { "CAUXDT", this.LP.GetText("HeaderAx1", "Aux-1") },        //Falta traducir
                    { "NOMBMT", this.LP.GetText("HeaderNomAux1", "Nombre Auxiliar-1") },        //Falta traducir
                    { "ZONAMA", this.LP.GetText("HeaderZonaAux1", "Zona Aux-1") },        //Falta traducir
                    { "TAAD01", this.LP.GetText("HeaderTA2", "TA-2") },        //Falta traducir
                    { "AUAD01", this.LP.GetText("HeaderAx2", "Aux-2") },        //Falta traducir
                    { "NOMBMT1", this.LP.GetText("HeaderNomAux2", "Nombre Auxiliar-2") },        //Falta traducir
                    { "ZONAMA1", this.LP.GetText("HeaderZonaAux2", "Zona Aux-2") },        //Falta traducir
                    { "TAAD02", this.LP.GetText("HeaderTA3", "TA-3") },        //Falta traducir
                    { "AUAD02", this.LP.GetText("HeaderAx3", "Aux-3") },        //Falta traducir
                    { "NOMBMT2", this.LP.GetText("HeaderNomAux3", "Nombre Auxiliar-3") },        //Falta traducir
                    { "ZONAMA2", this.LP.GetText("HeaderZonaAux3", "Zona Aux-3") },        //Falta traducir
                    { "TMOVDT", this.LP.GetText("HeaderDebeHaber", "D/H") },        //Falta traducir
                    { "MONTDT", this.LP.GetText("HeaderMonedaLocal", "Moneda Local") },        //Falta traducir
                    { "DESCAD", this.LP.GetText("HeaderDescripcion", "Descripción") },        //Falta traducir
                    { "TPIVCX", this.LP.GetText("HeaderPorc", "%") },       //Falta traducir
                    { "TICODT", this.LP.GetText("HeaderTipoComp", "Tipo") },        //Falta traducir
                    { "NUCODT", this.LP.GetText("HeaderNoComp", "NoComp.") },        //Falta traducir
                    { "FECODT", this.LP.GetText("HeaderFechaComp", "Fecha Comp.") },        //Falta traducir
                    { "CCIADT", this.LP.GetText("HeaderCiaAbrev", "Cía") },        //Falta traducir
                    { "TIPLDT", this.LP.GetText("HeaderPlan", "Plan") },        //Falta traducir
                    { "CLDODT", this.LP.GetText("HeaderClase1", "Cls-1") },     //Falta traducir
                    { "NDOCDT", this.LP.GetText("HeaderDoc1", "Doc-1") },      //Falta traducir
                    { "FDOCDT", this.LP.GetText("HeaderFechaDoc", "Fecha Doc.") },        //Falta traducir
                    { "FEVEDT", this.LP.GetText("HeaderVencimiento", "Vencimiento") },        //Falta traducir
                    { "TEINDT", this.LP.GetText("HeaderRU", "RU") },        //Falta traducir
                    { "PCIFAD", this.LP.GetText("HeaderPrefijo", "Prefijo") },        //Falta traducir
                    { "NNITAD", this.LP.GetText("HeaderPrefijo", "CIF/DNI") },        //Falta traducir
                    { "CDDOAD", this.LP.GetText("HeaderClase2", "Cls-2") },     //Falta traducir
                    { "NDDOAD", this.LP.GetText("HeaderDoc2", "Doc-2") },      //Falta traducir
                    { "MOSMAD", this.LP.GetText("HeaderImporte2", "Importe-2") },        //Falta traducir
                    { "TERCAD", this.LP.GetText("HeaderImporte3", "Importe-3") },        //Falta traducir
                    { "CDIVDT", this.LP.GetText("HeaderIva", "IVA") }        //Falta traducir
                };

                if (this.PlanCamposExtendidos)
                {
                    this.displayNamesComp.Add("PRFDDX", this.LP.GetText("HeaderPrefijoDoc", "Prefijo de Documento"));        //Falta traducir
                    this.displayNamesComp.Add("NFAADX", this.LP.GetText("HeaderNumFactAmp", "Número Fact. Ampliado"));        //Falta traducir
                    this.displayNamesComp.Add("NFARDX", this.LP.GetText("HeaderNumFactRect", "Número Fact. Rectificativa"));        //Falta traducir
                    this.displayNamesComp.Add("FIVADX", this.LP.GetText("HeaderFechaServicio", "Fecha de Servicio"));        //Falta traducir
                    this.displayNamesComp.Add("USA1DX", this.DtGLMX2.Rows[0]["NM01PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA2DX", this.DtGLMX2.Rows[0]["NM02PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA3DX", this.DtGLMX2.Rows[0]["NM03PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA4DX", this.DtGLMX2.Rows[0]["NM04PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA5DX", this.DtGLMX2.Rows[0]["NM05PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA6DX", this.DtGLMX2.Rows[0]["NM06PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA7DX", this.DtGLMX2.Rows[0]["NM07PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USA8DX", this.DtGLMX2.Rows[0]["NM08PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USN1DX", this.DtGLMX2.Rows[0]["NM09PX"].ToString());        //RIGHT Falta traducir
                    this.displayNamesComp.Add("USN2DX", this.DtGLMX2.Rows[0]["NM10PX"].ToString());        //RIGHT Falta traducir
                    this.displayNamesComp.Add("USF1DX", this.DtGLMX2.Rows[0]["NM11PX"].ToString());        //Falta traducir
                    this.displayNamesComp.Add("USF2DX", this.DtGLMX2.Rows[0]["NM12PX"].ToString());        //Falta traducir
                }
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Comprobantes
        /// </summary>
        private void RadGridViewCompHeader()
        {
            try
            {
                if (this.radGridViewComp.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesComp)
                    {
                        if (this.radGridViewComp.Columns.Contains(item.Key)) this.radGridViewComp.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CargarInfoComprobante()
        {
            IDataReader dr = null;
            try
            {
                string query = this.ObtenerConsulta();

                int contReg = 0;

                decimal[] saldoPeriodo = { 0, 0, 0 };

                DataRow rowComp;

                string cTipoAux = "";
                string cCtaAux = "";
                string descAux = "";
                string zonaAux = "";
                string montdtStr = "";
                decimal montdt = 0;
                string fdocdt = "";
                string cIVA = "";
                string porcIVA = "";
                string fecodt = "";
                string fevedt = "";
                string fivadx = "";
                string tico = "";
                string nuco = "";
                string doc = "";
                string saapp = "";
                string mosmadStr = "";
                string tercadStr = "";

                decimal totalDebe = 0;
                decimal totalHaber = 0;
                decimal sumMontdt = 0;

                decimal totalDebeME = 0;
                decimal totalHaberME = 0;
                
                decimal sumMosmad = 0;
                decimal sumTercad = 0;

                decimal importe = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    rowComp = this.dtComp.NewRow();

                    saapp = dr.GetValue(dr.GetOrdinal("SAPRDT")).ToString().Trim();
                    if (saapp != null) saapp = utiles.SAAPPFormat(saapp, "-");
                    rowComp["SAPRDT"] = saapp;

                    rowComp["CUENDT"] = dr.GetValue(dr.GetOrdinal("CUENDT")).ToString();
                    rowComp["NOABMC"] = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();

                    cTipoAux = dr.GetValue(dr.GetOrdinal("TAUXDT")).ToString().Trim();
                    rowComp["TAUXDT"] = cTipoAux;

                    cCtaAux = dr.GetValue(dr.GetOrdinal("CAUXDT")).ToString().Trim();
                    rowComp["CAUXDT"] = cCtaAux;

                    if (cTipoAux != "" && cCtaAux != "")
                    {
                        this.ObtenerDescripcionZonaAuxiliar(cTipoAux, cCtaAux, ref descAux, ref zonaAux);
                        rowComp["NOMBMT"] = descAux;
                        rowComp["ZONAMA"] = zonaAux;
                    }
                    else
                    {
                        rowComp["NOMBMT"] = "";
                        rowComp["ZONAMA"] = "";
                    }

                    cTipoAux = dr.GetValue(dr.GetOrdinal("TAAD01")).ToString();
                    rowComp["TAAD01"] = cTipoAux;

                    cCtaAux = dr.GetValue(dr.GetOrdinal("AUAD01")).ToString().Trim();
                    rowComp["AUAD01"] = cCtaAux;

                    if (cTipoAux != "" && cCtaAux != "")
                    {
                        this.ObtenerDescripcionZonaAuxiliar(cTipoAux, cCtaAux, ref descAux, ref zonaAux);
                        rowComp["NOMBMT1"] = descAux;
                        rowComp["ZONAMA1"] = zonaAux;
                    }
                    else
                    {
                        rowComp["NOMBMT1"] = "";
                        rowComp["ZONAMA1"] = "";
                    }

                    cTipoAux = dr.GetValue(dr.GetOrdinal("TAAD02")).ToString();
                    rowComp["TAAD02"] = cTipoAux;

                    cCtaAux = dr.GetValue(dr.GetOrdinal("AUAD02")).ToString().Trim();
                    rowComp["AUAD02"] = cCtaAux;

                    if (cTipoAux != "" && cCtaAux != "")
                    {
                        this.ObtenerDescripcionZonaAuxiliar(cTipoAux, cCtaAux, ref descAux, ref zonaAux);
                        rowComp["NOMBMT2"] = descAux;
                        rowComp["ZONAMA2"] = zonaAux;
                    }
                    else
                    {
                        rowComp["NOMBMT2"] = "";
                        rowComp["ZONAMA2"] = "";
                    }

                    rowComp["TMOVDT"] = dr.GetValue(dr.GetOrdinal("TMOVDT")).ToString();

                    montdtStr = dr.GetValue(dr.GetOrdinal("MONTDT")).ToString();
                    if (montdtStr != "")
                        try
                        {
                            montdt = Convert.ToDecimal(montdtStr);

                            switch (rowComp["TMOVDT"].ToString().Trim())
                            {
                                case "D":
                                    totalDebe += montdt;
                                    break;
                                case "H":
                                    totalHaber += montdt;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            montdt = 0;
                        }
                    else montdt = 0;
                    sumMontdt += montdt;
                    rowComp["MONTDT"] = montdt.ToString("N2", this.LP.MyCultureInfo);

                    rowComp["DESCAD"] = dr.GetValue(dr.GetOrdinal("DESCAD")).ToString().Trim();

                    cIVA = dr.GetValue(dr.GetOrdinal("CDIVDT")).ToString().Trim();
                    rowComp["CDIVDT"] = cIVA;

                    fdocdt = dr.GetValue(dr.GetOrdinal("FDOCDT")).ToString().Trim();
                    if (fdocdt != "" && fdocdt != "0") rowComp["FDOCDT"] = utiles.FechaToFormatoCG(fdocdt).ToShortDateString();
                    else rowComp["FDOCDT"] = "";

                    if (cIVA != "" && fdocdt != "") porcIVA = this.ObtenerPorcentajeIVA(cIVA, fdocdt);
                    else porcIVA = "";
                    rowComp["TPIVCX"] = porcIVA;

                    tico = dr.GetValue(dr.GetOrdinal("TICODT")).ToString().Trim();
                    if (tico != "") tico = tico.PadLeft(2, '0');
                    rowComp["TICODT"] = tico;

                    nuco = dr.GetValue(dr.GetOrdinal("NUCODT")).ToString().Trim();
                    if (nuco != "") nuco = nuco.PadLeft(5, '0'); 
                    rowComp["NUCODT"] = nuco;

                    fecodt = dr.GetValue(dr.GetOrdinal("FECODT")).ToString().Trim();
                    if (fecodt != "" && fecodt != "0") rowComp["FECODT"] = utiles.FechaToFormatoCG(fecodt).ToShortDateString();
                    else rowComp["FECODT"] = "";

                    rowComp["CCIADT"] = dr.GetValue(dr.GetOrdinal("CCIADT")).ToString();
                    rowComp["TIPLDT"] = dr.GetValue(dr.GetOrdinal("TIPLDT")).ToString();
                    rowComp["CLDODT"] = dr.GetValue(dr.GetOrdinal("CLDODT")).ToString();

                    doc = dr.GetValue(dr.GetOrdinal("NDOCDT")).ToString().Trim();
                    if (doc != "") doc = doc.PadLeft(9, '0');
                    rowComp["NDOCDT"] = doc;

                    fevedt = dr.GetValue(dr.GetOrdinal("FEVEDT")).ToString().Trim();
                    if (fevedt != "" && fevedt != "0") rowComp["FEVEDT"] = utiles.FechaToFormatoCG(fevedt).ToShortDateString();
                    else rowComp["FEVEDT"] = "";

                    rowComp["TEINDT"] = dr.GetValue(dr.GetOrdinal("TEINDT")).ToString();
                    rowComp["PCIFAD"] = dr.GetValue(dr.GetOrdinal("PCIFAD")).ToString();
                    rowComp["NNITAD"] = dr.GetValue(dr.GetOrdinal("NNITAD")).ToString();
                    rowComp["CDDOAD"] = dr.GetValue(dr.GetOrdinal("CDDOAD")).ToString();

                    doc = dr.GetValue(dr.GetOrdinal("NDDOAD")).ToString().Trim();
                    if (doc != "") doc = doc.PadLeft(9, '0');
                    rowComp["NDDOAD"] = doc;

                    importe = 0;
                    mosmadStr = dr.GetValue(dr.GetOrdinal("MOSMAD")).ToString();
                    if (mosmadStr != "")
                        try
                        {
                            importe = Convert.ToDecimal(mosmadStr);
                            switch (rowComp["TMOVDT"].ToString().Trim())
                            {
                                case "D":
                                    totalDebeME += importe;
                                    break;
                                case "H":
                                    totalHaberME += importe;
                                    break;
                            }
                        }
                        catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    sumMosmad += importe;
                    rowComp["MOSMAD"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    tercadStr = dr.GetValue(dr.GetOrdinal("TERCAD")).ToString();
                    if (tercadStr != "")
                        try
                        {
                            importe = Convert.ToDecimal(tercadStr);
                        }
                        catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    sumTercad += importe;
                    rowComp["TERCAD"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    if (this.PlanCamposExtendidos)
                    {
                        rowComp["PRFDDX"] = dr.GetValue(dr.GetOrdinal("PRFDDX")).ToString();
                        rowComp["NFAADX"] = dr.GetValue(dr.GetOrdinal("NFAADX")).ToString();
                        rowComp["NFARDX"] = dr.GetValue(dr.GetOrdinal("NFARDX")).ToString();

                        fivadx = dr.GetValue(dr.GetOrdinal("FIVADX")).ToString().Trim();
                        if (fivadx != "" && fivadx != "0") rowComp["FIVADX"] = utiles.FechaToFormatoCG(fivadx).ToShortDateString();
                        else rowComp["FIVADX"] = "";

                        rowComp["USA1DX"] = dr.GetValue(dr.GetOrdinal("USA1DX")).ToString();
                        rowComp["USA2DX"] = dr.GetValue(dr.GetOrdinal("USA2DX")).ToString();
                        rowComp["USA3DX"] = dr.GetValue(dr.GetOrdinal("USA3DX")).ToString();
                        rowComp["USA4DX"] = dr.GetValue(dr.GetOrdinal("USA4DX")).ToString();
                        rowComp["USA5DX"] = dr.GetValue(dr.GetOrdinal("USA5DX")).ToString();
                        rowComp["USA6DX"] = dr.GetValue(dr.GetOrdinal("USA6DX")).ToString();
                        rowComp["USA7DX"] = dr.GetValue(dr.GetOrdinal("USA7DX")).ToString();
                        rowComp["USA8DX"] = dr.GetValue(dr.GetOrdinal("USA8DX")).ToString();
                        rowComp["USN1DX"] = dr.GetValue(dr.GetOrdinal("USN1DX")).ToString();
                        rowComp["USN2DX"] = dr.GetValue(dr.GetOrdinal("USN2DX")).ToString();
                        rowComp["USF1DX"] = dr.GetValue(dr.GetOrdinal("USF1DX")).ToString();
                        rowComp["USF2DX"] = dr.GetValue(dr.GetOrdinal("USF2DX")).ToString();
                    }

                    this.dtComp.Rows.Add(rowComp);

                    contReg++;
                }
                    
                dr.Close();

                if (contReg > 0)
                {
                    if (contReg > 1)
                    {
                        //Insertar línea de totales
                        rowComp = this.dtComp.NewRow();
                        rowComp["NOABMC"] = "TOTALES";  //Falta traducir
                        rowComp["MONTDT"] = sumMontdt.ToString("N2", this.LP.MyCultureInfo);
                        rowComp["MOSMAD"] = sumMosmad.ToString("N2", this.LP.MyCultureInfo);
                        rowComp["TERCAD"] = sumTercad.ToString("N2", this.LP.MyCultureInfo);
                        this.dtComp.Rows.Add(rowComp);

                        this.radGridViewComp.Rows[this.radGridViewComp.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                    }

                    this.lblTotalDebeValor.Text = totalDebe.ToString("N2", this.LP.MyCultureInfo);
                    this.lblTotalHaberValor.Text = totalHaber.ToString("N2", this.LP.MyCultureInfo);

                    //Ocultar columna moneda extranjera
                    if (this.DatosMonedaExt != "1")
                    {
                        this.gbTotalesME.Visible = false;
                    }
                    else
                    {
                        this.lblTotalDebeMEValor.Text = totalDebeME.ToString("N2", this.LP.MyCultureInfo);
                        this.lblTotalHaberMEValor.Text = totalHaberME.ToString("N2", this.LP.MyCultureInfo);
                    }

                    this.radGridViewComp.Visible = true;
                }
                else
                {
                    //No hay comprobante
                    utiles.ButtonEnabled(ref this.radButtonExportar, false);
                    this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                    this.gbTotales.Visible = false;
                    this.gbTotalesME.Visible = false;

                    this.lblResult.Text = "No existe o no se ha podido recuperar el comprobante para el criterio de selección indicado";    //Falta traducir
                    this.lblResult.Visible = true;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                utiles.ButtonEnabled(ref this.radButtonExportar, false);
                this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                this.gbTotales.Visible = false;
                this.gbTotalesME.Visible = false;

                this.lblResult.Text = "No se ha podido recuperar el comprobante";   //Falta traducir
                this.lblResult.Visible = true;
                this.radGridViewComp.Visible = false;
            }

            if (this.radGridViewComp.Visible)
            {
                for (int i = 0; i < this.radGridViewComp.Columns.Count; i++)
                    this.radGridViewComp.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                //this.radGridViewComp.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewComp.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.None;

                //this.radGridViewComp.MasterTemplate.BestFitColumns();
                this.radGridViewComp.Rows[0].IsCurrent = true;
            }
        }

        /// <summary>
        /// Devuelve la consulta donde se recupera el comprobante
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsulta()
        {
            string query = "";
            try
            {             
                if (this.PlanCamposExtendidos)
                {
                    query += "select SAPRDT, CUENDT, NOABMC, TAUXDT, CAUXDT, TAAD01, AUAD01, TAAD02, AUAD02, TMOVDT, MONTDT, DESCAD, CDIVDT, ";
                    query += "TICODT, NUCODT, FECODT, CCIADT, TIPLDT, CLDODT, NDOCDT, FDOCDT, FEVEDT, TEINDT, PCIFAD, NNITAD, CDDOAD, NDDOAD, ";
                    query += "MOSMAD, TERCAD, CDIVDT, PRFDDX, NFAADX, NFARDX, FIVADX, USA1DX, USA2DX, USA3DX, USA4DX, USA5DX, USA6DX, USA7DX, ";
                    query += "USA8DX, USN1DX, USN2DX, USF1DX, USF2DX ";
                    query += "from " + GlobalVar.PrefijoTablaCG  + "GLB01 left join " + GlobalVar.PrefijoTablaCG + "GLBX1 on CCIADT = CCIADX and ";
                    query += "SAPRDT = SAPRDX and TICODT = TICODX and NUCODT = NUCODX and SIMIDT = SIMIDX, ";
                }
                else
                {
                    query += "select SAPRDT, CUENDT, NOABMC, TAUXDT, CAUXDT, TAAD01, AUAD01, TAAD02, AUAD02, TMOVDT, MONTDT, DESCAD, CDIVDT, ";
                    query += "TICODT, NUCODT, FECODT, CCIADT, TIPLDT, CLDODT, NDOCDT, FDOCDT, FEVEDT, TEINDT, PCIFAD, NNITAD, CDDOAD, NDDOAD, ";
                    query += "MOSMAD, TERCAD, CDIVDT ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLB01, ";
                }

                query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where STATDT='E' and CCIADT='" + this.CompaniaCodigo + "' and SAPRDT = " + this.AAPPDesde + " and ";
                query += "TICODT = '" + this.TipoComp + "' and NUCODT = " + this.NoComp + " and CUENDT= CUENMC and TIPLDT = TIPLMC";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }

        /// <summary>
        /// Obtener el listado de compañías que forman el grupo (si la solicitud es por grupo) o el plan si la solicitud es por compañía
        /// </summary>
        private void ObtenerDatosCompania_Grupo()
        {
            string[] codDes = new string[2];

            if (this.GrupoCodigo != "")
            {
                //Buscar las empresas del grupo
                aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(this.GrupoCodigo, this.PlanCodigo);
                calendario = "";
            }
            else
            {
                codDes[0] = this.CompaniaCodigo;
                codDes[1] = this.CompaniaDesc;
                aEmpresas = new ArrayList
                {
                    codDes
                };

                //Buscar el plan de la compañía (el método devuelve además el calendario)
                string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(this.CompaniaCodigo);
                this.PlanCodigo = datosCompaniaAct[1];
                calendario = datosCompaniaAct[0];
            }
        }
         
        /// <summary>
        /// Devuelve el nombre y la zona del auxiliar
        /// </summary>
        /// <param name="cTipoAux">código Tipo de Auxiliar</param>
        /// <param name="cAux">código de Auxiliar</param>
        /// <param name="descripcion">nombre del auxiliar</param>
        /// <param name="zona">zona del auxiliar</param>
        private void ObtenerDescripcionZonaAuxiliar(string cTipoAux, string cAux, ref string descripcion, ref string zona)
        {
            IDataReader dr = null;
            descripcion = "";
            zona = "";

            try
            {
                //Verificar si ya existe su información
                string auxTipoCuenta = cTipoAux.PadRight(2, ' ') + cAux.PadRight(8, ' ');
                string auxZonaNombre = utiles.FindFirstKeyByValue(ref this.dictAuxZonaNombre, auxTipoCuenta);

                if (auxZonaNombre != auxTipoCuenta)
                {
                    if (auxZonaNombre.Length != 48) auxZonaNombre = auxZonaNombre.PadRight(48, ' ');
                    zona = auxZonaNombre.Substring(0, 8).Trim();
                    descripcion = auxZonaNombre.Substring(7, 40).Trim();
                }
                else
                {
                    string query = "select NOMBMA, ZONAMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + cTipoAux + "' and CAUXMA = '" + cAux + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        descripcion = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString().Trim();
                        zona = dr.GetValue(dr.GetOrdinal("ZONAMA")).ToString().Trim();
                    }

                    dr.Close();

                    if (descripcion != "" || zona != "")
                    {
                        auxZonaNombre = zona.PadRight(8, ' ') + descripcion.PadRight(40, ' ');

                        this.dictAuxZonaNombre.Add(auxZonaNombre, auxTipoCuenta);
                    }
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex)); 

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Devuelve el porcentaje de IVA que se aplica
        /// </summary>
        /// <param name="cIVA">código de IVA</param>
        /// <param name="fechaDoc">fecha documento</param>
        /// <returns></returns>
        private string ObtenerPorcentajeIVA(string cIVA, string fechaDoc)
        {
            string result = "";

            IDataReader dr = null;
            decimal porc = 0;

            try
            {
                //Obtener % de IVA
                //select TPIVCX, RECGCX from TIGSA_IVTX1
                //where TIPLCX = 'Z' and COIVCX = 'V1' and FEIVCX <= fdocdt
                //order by FEIVCX desc
                //% = TPIVCX + RECGCX

                string query = "select TPIVCX, RECGCX from " + GlobalVar.PrefijoTablaCG + "IVTX1 ";
                query += "where TIPLCX = '" + this.PlanCodigo + "' and COIVCX = '" + cIVA + "' and ";
                query += "FEIVCX <= " + fechaDoc;
                query += " order by FEIVCX desc";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                decimal recargoequiv;

                if (dr.Read())
                {
                    try
                    {
                        porc = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("TPIVCX")).ToString().Trim());
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); porc = 0; }

                    try
                    {
                        recargoequiv = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("RECGCX")).ToString().Trim());
                    }
                    catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex));  recargoequiv = 0; }

                    porc = porc + recargoequiv;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            if (porc == 0) result = "";
            else result = porc.ToString();

            return (result);
        }

        /// <summary>
        /// 
        /// </summary>
        private void GridExportar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                this.grBoxProgressBar.Text = this.LP.GetText("lblCompContExportando", "Exportando");
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                ExcelExportImport excelImport = new ExcelExportImport
                {
                    DateTableDatos = this.dtComp,
                    Cabecera = true,

                    //Titulo
                    Titulo = this.Text + " -  Compañía: " + this.CompaniaDesc.Replace("  -  ", "-")       //Falta traducir
                };
                excelImport.Titulo += "  Año-periodo: " + lblAnoPerValor.Text;       //Falta traducir
                excelImport.Titulo += "  Tipo de comprobante : " + this.lblTipoCompValor.Text.Replace("  -  ", "-");       //Falta traducir
                excelImport.Titulo += "  Número de comprobante : " + this.lblNoCompValor.Text;      //Falta traducir
                
                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.radGridViewComp.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.radGridViewComp.Columns[i].HeaderText;                   //Nombre de la columna

                    switch (this.radGridViewComp.Columns[i].Name)
                    {
                        case "MONTDT":
                        case "MOSMAD":
                        case "TERCAD":
                            nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            break;
                    }
                    //dt.Columns.Add("DIASPAGO", typeof(string));
                    //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                    nombreTipoVisible[2] = this.radGridViewComp.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.radGridViewComp.SelectedRows.Count > 0 && this.radGridViewComp.SelectedRows.Count < this.radGridViewComp.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.radGridViewComp.SelectedRows.Count; i++)
                    {
                        indice = this.radGridViewComp.Rows.IndexOf(this.radGridViewComp.SelectedRows[i]);

                        if (this.radGridViewComp.Rows.Count - 1 == indice)
                        {
                            //Linea Totales
                            if (aIndice.Count > 0)
                            {
                                aIndice.Add(indice);
                            }
                            else
                            {
                                //Solo linea de Totales, no se adiciona y se exportan todas las filas
                            }
                        }
                        else aIndice.Add(indice);
                    }

                    excelImport.IndiceFilasSeleccionadas = aIndice;
                }
                
                string cabecera = System.Configuration.ConfigurationManager.AppSettings["ModComp_Excel_Cabecera"];
                if (cabecera != null)
                {
                    excelImport.Cabecera = (cabecera == "Yes" ? true : false);
                    //if (excelImport.Cabecera) excelImport.GridColumnas = this.radGridViewComp.Columns;
                }

                string result = excelImport.ExportarMemoria();

                this.progressBarEspera.Visible = false;
                this.grBoxProgressBar.Visible = false;

                if (result != "" && result != "CANCELAR")
                {
                    RadMessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + result + ")", error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                RadMessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void GridExportarTelerik()
        {
            string titulo = this.Text + " -  Compañía: " + this.CompaniaDesc.Replace("  -  ", "-");
            titulo += "  Año-periodo: " + lblAnoPerValor.Text;
            titulo += "  Tipo de comprobante : " + this.lblTipoCompValor.Text.Replace("  -  ", "-");
            titulo += "  Número de comprobante : " + this.lblNoCompValor.Text;
                
            //Columnas
            ArrayList descColumnas = new ArrayList();
            string[] nombreTipoVisible;
            for (int i = 0; i < this.radGridViewComp.ColumnCount; i++)
            {
                nombreTipoVisible = new string[3];
                nombreTipoVisible[0] = this.radGridViewComp.Columns[i].HeaderText;                   //Nombre de la columna

                switch (this.radGridViewComp.Columns[i].Name)
                {
                    case "MONTDT":
                    case "MOSMAD":
                    case "TERCAD":
                        nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                        break;
                    default:
                        nombreTipoVisible[1] = "string";
                        break;
                }
                //dt.Columns.Add("DIASPAGO", typeof(string));
                //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                nombreTipoVisible[2] = this.radGridViewComp.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                descColumnas.Add(nombreTipoVisible);
            }

            this.ExportarGrid(ref this.radGridViewComp, titulo, false, null, "Comprobante", ref descColumnas, null);
        }


        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemExportar = new RadMenuItem(this.radButtonExportar.Text)
            {
                Name = "menuItemExportar"
            };
            menuItemExportar.Click += new EventHandler(RadButtonExportar_Click);
            RadMenuItem menuItemSalir = new RadMenuItem(this.radButtonSalir.Text)
            {
                Name = "menuItemSalir"
            };
            menuItemSalir.Click += new EventHandler(RadButtonSalir_Click);

            this.radContextMenuClickDerecho.Items.Add(menuItemExportar);
            this.radContextMenuClickDerecho.Items.Add(menuItemSalir);
        }
        #endregion
    }
}