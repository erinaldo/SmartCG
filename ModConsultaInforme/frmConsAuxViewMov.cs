using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ModConsultaInforme
{
    public partial class frmConsAuxViewMov :  frmPlantilla, IReLocalizable
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
        public string MostrarCuentas { get; set; }
        public string Documentos { get; set; }
        public string DatosMonedaExt { get; set; }
        
        public string Clase { get; set; }
        public string NoDocumento { get; set; }
        public string SaldoInicialDocumento { get; set; }

        ArrayList aEmpresas = null;
        //ArrayList cuentasAProcesar = null;
        
        string calendario = "";
        //int nivelPlanCuentas;

        bool planCamposExtendidos = false;
        DataTable dtGLMX2;
        //DataTable dtGLMX3;
        string tipoBaseDatosCG;

        Dictionary<string, string> displayNamesMov;
        DataTable dtMov;

        public frmConsAuxViewMov()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radGridViewMov.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxViewMov_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar Movimientos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Cargar Menu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            //Muestra la cabecera de la consulta (criterio de selección / saldos / totales)
            this.CargarValoresCabecera();

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Verificar si el plan tiene campos extendidos
            this.planCamposExtendidos = utilesCG.PlanCamposExtendidos(this.PlanCodigo, tipoBaseDatosCG, ref dtGLMX2);

            //Crear el DataTable para la Grid
            this.BuildDataTableMovimientos();

            //Cargar los movimientos
            this.CargarInfoMovimientos();
        }

        private void TgGridMov_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.VerComprobante();
        }

        private void RadButtonComprobante_Click(object sender, EventArgs e)
        {
            this.VerComprobante();
        }

        private void RadButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarTelerik();
            //this.GridExportar();
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void RadGridViewMov_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewMov.Columns.Contains("CAUXDT"))
                if (this.CtaAuxCodigo != "") this.radGridViewMov.Columns["CAUXDT"].IsVisible = false;
                else this.radGridViewMov.Columns["CAUXDT"].Width = 45;
            if (this.radGridViewMov.Columns.Contains("DOCUMENTO")) this.radGridViewMov.Columns["DOCUMENTO"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("FDOCDT")) this.radGridViewMov.Columns["FDOCDT"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("FEVEDT")) this.radGridViewMov.Columns["FEVEDT"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("TMOVDT")) this.radGridViewMov.Columns["TMOVDT"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("MONTDT"))
            {
                this.radGridViewMov.Columns["MONTDT"].Width = 100;         //Falta traducir
                this.radGridViewMov.Columns["MONTDT"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("MOSMAD"))
            {
                if (this.DatosMonedaExt != "1") this.radGridViewMov.Columns["MOSMAD"].IsVisible = false;
                else
                {
                    this.radGridViewMov.Columns["MOSMAD"].Width = 120;
                    this.radGridViewMov.Columns["MOSMAD"].TextAlignment = ContentAlignment.MiddleRight;
                }
            }
            if (this.radGridViewMov.Columns.Contains("DESCAD")) this.radGridViewMov.Columns["DESCAD"].Width = 150;
            if (this.radGridViewMov.Columns.Contains("CDDOAD"))
            {
                this.radGridViewMov.Columns["CDDOAD"].Width = 40;       //Falta traducir
                this.radGridViewMov.Columns["CDDOAD"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("NDDOAD"))
            {
                this.radGridViewMov.Columns["NDDOAD"].Width = 85;
                this.radGridViewMov.Columns["MONTDT"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("TAAD01"))
            {
                this.radGridViewMov.Columns["TAAD01"].Width = 85;
                this.radGridViewMov.Columns["TAAD01"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("AUAD01")) this.radGridViewMov.Columns["AUAD01"].Width = 85;
            if (this.radGridViewMov.Columns.Contains("TAAD02"))
            {
                this.radGridViewMov.Columns["TAAD02"].Width = 85;
                this.radGridViewMov.Columns["TAAD02"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("AUAD02")) this.radGridViewMov.Columns["AUAD02"].Width = 85;       //Falta traducir
            if (this.radGridViewMov.Columns.Contains("TERCAD"))
            {
                this.radGridViewMov.Columns["TERCAD"].Width = 85;       //Falta traducir
                this.radGridViewMov.Columns["TERCAD"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewMov.Columns.Contains("CDIVDT")) this.radGridViewMov.Columns["CDIVDT"].Width = 40;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("TEINDT")) this.radGridViewMov.Columns["TEINDT"].Width = 40;      //Falta traducir
            if (this.radGridViewMov.Columns.Contains("PCIFAD")) this.radGridViewMov.Columns["PCIFAD"].Width = 100;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("NNITAD")) this.radGridViewMov.Columns["NNITAD"].Width = 100;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("SAPRDTFormat")) this.radGridViewMov.Columns["SAPRDTFormat"].Width = 100;        //Falta traducir0
            if (this.radGridViewMov.Columns.Contains("TICODTFormat")) this.radGridViewMov.Columns["TICODTFormat"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("NUCODTFormat")) this.radGridViewMov.Columns["NUCODTFormat"].Width = 85;        //Falta traducir
            if (this.radGridViewMov.Columns.Contains("FECODT")) this.radGridViewMov.Columns["FECODT"].Width = 140;        //Falta traducir

            if (this.radGridViewMov.Columns.Contains("SIMIDT")) this.radGridViewMov.Columns["SIMIDT"].IsVisible = false;
            if (this.radGridViewMov.Columns.Contains("SAPRDT")) this.radGridViewMov.Columns["SAPRDT"].IsVisible = false;
            if (this.radGridViewMov.Columns.Contains("CLDODT")) this.radGridViewMov.Columns["CLDODT"].IsVisible = false;
            if (this.radGridViewMov.Columns.Contains("NDOCDT")) this.radGridViewMov.Columns["NDOCDT"].IsVisible = false;
            if (this.radGridViewMov.Columns.Contains("TICODT")) this.radGridViewMov.Columns["TICODT"].IsVisible = false;
            if (this.radGridViewMov.Columns.Contains("NUCODT")) this.radGridViewMov.Columns["NUCODT"].IsVisible = false;

            if (this.planCamposExtendidos)
            {
                if (this.radGridViewMov.Columns.Contains("PRFDDX")) this.radGridViewMov.Columns["PRFDDX"].Width = 140;       //Falta traducir
                if (this.radGridViewMov.Columns.Contains("NFAADX")) this.radGridViewMov.Columns["NFAADX"].Width = 160;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("NFARDX")) this.radGridViewMov.Columns["NFARDX"].Width = 160;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("FIVADX")) this.radGridViewMov.Columns["FIVADX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA1DX")) this.radGridViewMov.Columns["USA1DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA2DX")) this.radGridViewMov.Columns["USA2DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA3DX")) this.radGridViewMov.Columns["USA3DX"].Width = 140;       //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA4DX")) this.radGridViewMov.Columns["USA4DX"].Width = 140;       //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA5DX")) this.radGridViewMov.Columns["USA5DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA6DX")) this.radGridViewMov.Columns["USA6DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA7DX")) this.radGridViewMov.Columns["USA7DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USA8DX")) this.radGridViewMov.Columns["USA8DX"].Width = 140;        //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USN1DX"))
                {
                    this.radGridViewMov.Columns["USN1DX"].Width = 140;        //Falta traducir
                    this.radGridViewMov.Columns["USN1DX"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewMov.Columns.Contains("USN2DX"))
                {
                    this.radGridViewMov.Columns["USN2DX"].Width = 140;        //Falta traducir
                    this.radGridViewMov.Columns["USN2DX"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewMov.Columns.Contains("USF1DX")) this.radGridViewMov.Columns["USF1DX"].Width = 140;       //Falta traducir
                if (this.radGridViewMov.Columns.Contains("USF2DX")) this.radGridViewMov.Columns["USF2DX"].Width = 140;       //Falta traducir
            }
        }

        private void RadGridViewMov_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadGridViewMov_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewMov, ref this.selectAll);
        }

        private void RadButtonComprobante_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonComprobante);
        }

        private void RadButtonComprobante_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonComprobante);
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

        private void FrmConsAuxViewMov_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void FrmConsAuxViewMov_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Auxiliar Movimientos");
        }

        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemConsultaAuxMov", "Consulta de Auxiliar - Movimientos");

            this.radButtonComprobante.Text = this.LP.GetText("lblVerComp", "Ver el Comprobante");     //Falta traducir
            this.radButtonSalir.Text = this.LP.GetText("lblSalir", "Salir");     //Falta traducir

            this.lblResult.Text = this.LP.GetText("lblConsultaAuxMovResult", "No existen movimientos para el criterio de selección indicado");     //Falta traducir
        }

        /// <summary>
        /// Muestra la cabecera (criterio de selección indicado por el usuario y saldos/totales)
        /// </summary>
        private void CargarValoresCabecera()
        {
            try
            {
                this.ucConsAuxCab.Lp = this.LP;

                //Tipos de Auxiliar
                this.ucConsAuxCab.TipoAuxCodigo = this.TipoAuxCodigo;
                this.ucConsAuxCab.TipoAuxDesc = this.TipoAuxDesc;

                //Cuenta de Auxiliar
                this.ucConsAuxCab.CtaAuxCodigo = this.CtaAuxCodigo;
                this.ucConsAuxCab.CtaAuxDesc = this.CtaAuxDesc;

                //Posición del Auxiliar
                this.ucConsAuxCab.PosAux = this.PosAux;

                //Compañía/Grupo
                if (this.CompaniaCodigo != "")
                {
                    //Compañía
                    this.ucConsAuxCab.Compania_GrupoLabel.Text = "Compañía";        //Falta traducir
                    this.ucConsAuxCab.CompaniaDesc = this.CompaniaDesc;

                    this.ucConsAuxCab.PlanLabel.Visible = false;
                    this.ucConsAuxCab.PlanDescLabel.Visible = false;
                }
                else
                {
                    //Grupo
                    this.ucConsAuxCab.Compania_GrupoLabel.Text = "Grupo de compañías";        //Falta traducir
                    this.ucConsAuxCab.GrupoDesc = this.GrupoDesc;

                    //Plan
                    this.ucConsAuxCab.PlanDesc = this.PlanDesc;
                    this.ucConsAuxCab.PlanLabel.Visible = false;
                    this.ucConsAuxCab.PlanDescLabel.Visible = false;
                }

                //Año-periodo desde
                this.ucConsAuxCab.AAPPDesde = this.AAPPDesdeFormat;             //Falta

                //Año-periodo hasta
                this.ucConsAuxCab.AAPPHasta = this.AAPPHastaFormat;             //Falta

                //Cuenta de Mayor
                this.ucConsAuxCab.CtaMayorCodigo = this.CtaMayorCodigo;
                this.ucConsAuxCab.CtaMayorDesc = this.CtaMayorDesc;

                //Mostrar datos de moneda extranjera
                if (this.DatosMonedaExt == "1") this.ucConsAuxCab.DatosMonedaExt.Visible = true;
                else this.ucConsAuxCab.DatosMonedaExt.Visible = false;
                
                //Documentos
                //OJO modificar la etiqueta para que sea documento y pasarle clase-doc
                if ((this.Clase != null && this.Clase.Trim() != "") || (this.NoDocumento != null && this.NoDocumento.Trim() != "")) this.ucConsAuxCab.DocumentosDesc = this.Clase.PadLeft(2, '0') + "-" + this.NoDocumento.PadLeft(7, '0');
                else this.ucConsAuxCab.DocumentosDesc = "";

                this.ucConsAuxCab.ActualizarValores();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuildDataTableMovimientos()
        {
            try
            {
                this.dtMov = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                this.dtMov.Columns.Add("CAUXDT", typeof(string));
                this.dtMov.Columns.Add("DOCUMENTO", typeof(string));
                this.dtMov.Columns.Add("FDOCDT", typeof(string));
                this.dtMov.Columns.Add("FEVEDT", typeof(string));
                this.dtMov.Columns.Add("TMOVDT", typeof(string));
                this.dtMov.Columns.Add("MONTDT", typeof(string));
                this.dtMov.Columns.Add("MOSMAD", typeof(string));
                this.dtMov.Columns.Add("DESCAD", typeof(string));
                this.dtMov.Columns.Add("CDDOAD", typeof(string));
                this.dtMov.Columns.Add("NDDOAD", typeof(string));
                this.dtMov.Columns.Add("TAAD01", typeof(string));
                this.dtMov.Columns.Add("AUAD01", typeof(string));
                this.dtMov.Columns.Add("TAAD02", typeof(string));
                this.dtMov.Columns.Add("AUAD02", typeof(string));
                this.dtMov.Columns.Add("TERCAD", typeof(string));
                this.dtMov.Columns.Add("CDIVDT", typeof(string));
                this.dtMov.Columns.Add("TEINDT", typeof(string));
                this.dtMov.Columns.Add("PCIFAD", typeof(string));
                this.dtMov.Columns.Add("NNITAD", typeof(string));
                this.dtMov.Columns.Add("SAPRDTFormat", typeof(string));
                this.dtMov.Columns.Add("TICODTFormat", typeof(string));
                this.dtMov.Columns.Add("NUCODTFormat", typeof(string));
                this.dtMov.Columns.Add("FECODT", typeof(string));
                this.dtMov.Columns.Add("SIMIDT", typeof(string));
                this.dtMov.Columns.Add("SAPRDT", typeof(string));
                this.dtMov.Columns.Add("CLDODT", typeof(string));
                this.dtMov.Columns.Add("NDOCDT", typeof(string));
                this.dtMov.Columns.Add("TICODT", typeof(string));
                this.dtMov.Columns.Add("NUCODT", typeof(string));

                if (this.planCamposExtendidos)
                {
                    if (this.dtGLMX2 != null && this.dtGLMX2.Rows.Count == 1)
                    {
                        try
                        {
                            this.dtMov.Columns.Add("PRFDDX", typeof(string));
                            this.dtMov.Columns.Add("NFAADX", typeof(string));
                            this.dtMov.Columns.Add("NFARDX", typeof(string));
                            this.dtMov.Columns.Add("FIVADX", typeof(string));
                            this.dtMov.Columns.Add("USA1DX", typeof(string));
                            this.dtMov.Columns.Add("USA2DX", typeof(string));
                            this.dtMov.Columns.Add("USA3DX", typeof(string));
                            this.dtMov.Columns.Add("USA4DX", typeof(string));
                            this.dtMov.Columns.Add("USA5DX", typeof(string));
                            this.dtMov.Columns.Add("USA6DX", typeof(string));
                            this.dtMov.Columns.Add("USA7DX", typeof(string));
                            this.dtMov.Columns.Add("USA8DX", typeof(string));
                            this.dtMov.Columns.Add("USN1DX", typeof(string));
                            this.dtMov.Columns.Add("USN2DX", typeof(string));
                            this.dtMov.Columns.Add("USF1DX", typeof(string));
                            this.dtMov.Columns.Add("USF2DX", typeof(string));
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    else this.planCamposExtendidos = false;
                }

                this.radGridViewMov.DataSource = this.dtMov;
                //Escribe el encabezado de la Grid de Movimientos
                this.BuildDisplayNamesMov();
                this.RadGridViewMovHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Encabezados para la Grid de Movimientos
        /// </summary>
        private void BuildDisplayNamesMov()
        {
            try
            {
                this.displayNamesMov = new Dictionary<string, string>
                {
                    { "CAUXDT", this.LP.GetText("HeaderAuxiliar", "Auxiliar") },
                    { "DOCUMENTO", this.LP.GetText("HeaderDocumento", "Documento") },        //Falta traducir
                    { "FDOCDT", this.LP.GetText("HeaderFecha", "Fecha Doc.") },        //Falta traducir
                    { "FEVEDT", this.LP.GetText("HeaderVencimiento", "Vencimiento") },        //Falta traducir
                    { "TMOVDT", this.LP.GetText("HeaderDebeHaber", "D/H") },        //Falta traducir
                    { "MONTDT", this.LP.GetText("HeaderMonedaLocal", "Moneda Local") },        //Falta traducir
                    { "MOSMAD", this.LP.GetText("HeaderMonedaExt", "Moneda Extranjera") },        //Falta traducir
                    { "DESCAD", this.LP.GetText("HeaderDescripcion", "Descripción") },        //Falta traducir
                    { "CDDOAD", this.LP.GetText("HeaderClase2", "Clase-2") },        //Falta traducir
                    { "NDDOAD", this.LP.GetText("HeaderDocumento2", "Documento-2") },        //Falta traducir
                    { "TAAD01", this.LP.GetText("HeaderTipo2", "Tipo-2") },        //Falta traducir
                    { "AUAD01", this.LP.GetText("HeaderAux2", "Auxiliar-2") },        //Falta traducir
                    { "TAAD02", this.LP.GetText("HeaderTipo3", "Tipo-3") },        //Falta traducir
                    { "AUAD02", this.LP.GetText("HeaderAux3", "Auxiliar-3") },        //Falta traducir
                    { "TERCAD", this.LP.GetText("HeaderMoneda3", "Moneda-3") },        //Falta traducir
                    { "CDIVDT", this.LP.GetText("HeaderCodIva", "IVA") },        //Falta traducir
                    { "TEINDT", this.LP.GetText("HeaderRU", "RU") },        //Falta traducir
                    { "PCIFAD", this.LP.GetText("HeaderPais", "Pais") },        //Falta traducir
                    { "NNITAD", this.LP.GetText("HeaderNit", "NIT") },        //Falta traducir
                    { "SAPRDTFormat", this.LP.GetText("HeaderAAPP", "Año-Periodo") },        //Falta traducir0
                    { "TICODTFormat", this.LP.GetText("HeaderComp", "Comprobante") },        //Falta traducir
                    { "NUCODTFormat", this.LP.GetText("HeaderNoComp", "No Comprobante") },        //Falta traducir
                    { "FECODT", this.LP.GetText("HeaderFechaComp", "Fecha Comprobante") },        //Falta traducir
                    { "SIMIDT", "SIMIDT" },
                    { "SAPRDT", this.LP.GetText("HeaderAAPP", "Año-Periodo") },
                    { "CLDODT", this.LP.GetText("HeaderClase", "Clase") },
                    { "NDOCDT", this.LP.GetText("HeaderDocumento", "Documento") },
                    { "TICODT", this.LP.GetText("HeaderComp", "Comprobante") },
                    { "NUCODT", this.LP.GetText("HeaderNoComp", "No Comprobante") }
                };

                if (this.planCamposExtendidos)
                {
                    this.displayNamesMov.Add("PRFDDX", this.LP.GetText("HeaderPrefijoDoc", "Prefijo de Documento"));        //Falta traducir
                    this.displayNamesMov.Add("NFAADX", this.LP.GetText("HeaderNumFactAmp", "Número Fact. Ampliado"));        //Falta traducir
                    this.displayNamesMov.Add("NFARDX", this.LP.GetText("HeaderNumFactRect", "Número Fact. Rectificativa"));        //Falta traducir
                    this.displayNamesMov.Add("FIVADX", this.LP.GetText("HeaderFechaServicio", "Fecha de Servicio"));        //Falta traducir
                    this.displayNamesMov.Add("USA1DX", this.dtGLMX2.Rows[0]["NM01PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA2DX", this.dtGLMX2.Rows[0]["NM02PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA3DX", this.dtGLMX2.Rows[0]["NM03PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA4DX", this.dtGLMX2.Rows[0]["NM04PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA5DX", this.dtGLMX2.Rows[0]["NM05PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA6DX", this.dtGLMX2.Rows[0]["NM06PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA7DX", this.dtGLMX2.Rows[0]["NM07PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USA8DX", this.dtGLMX2.Rows[0]["NM08PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USN1DX", this.dtGLMX2.Rows[0]["NM09PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USN2DX", this.dtGLMX2.Rows[0]["NM10PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USF1DX", this.dtGLMX2.Rows[0]["NM11PX"].ToString());        //Falta traducir
                    this.displayNamesMov.Add("USF2DX", this.dtGLMX2.Rows[0]["NM12PX"].ToString());        //Falta traducir
                }
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Movimientos
        /// </summary>
        private void RadGridViewMovHeader()
        {
            try
            {
                if (this.radGridViewMov.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesMov)
                    {
                        if (this.radGridViewMov.Columns.Contains(item.Key)) this.radGridViewMov.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CargarInfoMovimientos()
        {
            IDataReader dr = null;
            try
            {
                string query = this.ObtenerConsulta();

                int contReg = 0;
                string montdtStr = "";
                decimal montdt = 0;
                decimal sumMontdt = 0;
                string mosmadStr = "";
                decimal sumMosmaddt = 0;
                string tipo = "";
                string nuco = "";
                string fdocdt = "";
                string fevedt = "";
                string fecodt = "";
                string fivadx = "";

                decimal totalDebeML = 0;
                decimal totalHaberML = 0;
                decimal totalDebeME = 0;
                decimal totalHaberME = 0;

                decimal[] saldoPeriodo = { 0, 0, 0 };

                string saapp = "";

                decimal importe = 0;

                string clase = "";
                string doc = "";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                DataRow rowMov;

                while (dr.Read())
                {
                    rowMov = this.dtMov.NewRow();

                    rowMov["CAUXDT"] = dr.GetValue(dr.GetOrdinal("CAUXDT")).ToString();

                    clase = dr.GetValue(dr.GetOrdinal("CLDODT")).ToString();
                    rowMov["CLDODT"] = clase;
                    doc = dr.GetValue(dr.GetOrdinal("NDOCDT")).ToString();
                    rowMov["NDOCDT"] = doc;
                    rowMov["DOCUMENTO"] = clase.PadLeft(2, '0') + "-" + doc.PadLeft(7, '0');

                    fdocdt = dr.GetValue(dr.GetOrdinal("FDOCDT")).ToString().Trim();
                    if (fdocdt != "" && fdocdt != "0") rowMov["FDOCDT"] = utiles.FechaToFormatoCG(fdocdt).ToShortDateString();
                    else rowMov["FDOCDT"] = "";

                    fevedt = dr.GetValue(dr.GetOrdinal("FEVEDT")).ToString().Trim();
                    if (fevedt != "" && fevedt != "0") rowMov["FEVEDT"] = utiles.FechaToFormatoCG(fevedt).ToShortDateString();
                    else rowMov["FEVEDT"] = "";

                    rowMov["TMOVDT"] = dr.GetValue(dr.GetOrdinal("TMOVDT")).ToString();

                    montdtStr = dr.GetValue(dr.GetOrdinal("MONTDT")).ToString().Trim();
                    if (montdtStr != "")
                        try
                        { 
                            montdt = Convert.ToDecimal(montdtStr);

                            switch (rowMov["TMOVDT"].ToString().Trim())
                            {
                                case "D":
                                    totalDebeML += montdt;
                                    break;
                                case "H":
                                    totalHaberML += montdt;
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
                    rowMov["MONTDT"] = montdt.ToString("N2", this.LP.MyCultureInfo);

                    mosmadStr = dr.GetValue(dr.GetOrdinal("MOSMAD")).ToString().Trim();
                    if (mosmadStr != "")
                        try { 

                            importe = Convert.ToDecimal(mosmadStr);

                            switch (rowMov["TMOVDT"].ToString().Trim())
                            {
                                case "D":
                                    totalDebeME += importe;
                                    break;
                                case "H":
                                    totalHaberME += importe;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            importe = 0;
                        }
                    else importe = 0;

                    sumMosmaddt += importe;
                    rowMov["MOSMAD"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    rowMov["DESCAD"] = dr.GetValue(dr.GetOrdinal("DESCAD")).ToString();
                    rowMov["CDDOAD"] = dr.GetValue(dr.GetOrdinal("CDDOAD")).ToString();
                    rowMov["NDDOAD"] = dr.GetValue(dr.GetOrdinal("NDDOAD")).ToString();
                    rowMov["TAAD01"] = dr.GetValue(dr.GetOrdinal("TAAD01")).ToString();
                    rowMov["AUAD01"] = dr.GetValue(dr.GetOrdinal("AUAD01")).ToString();
                    rowMov["TAAD02"] = dr.GetValue(dr.GetOrdinal("TAAD02")).ToString();
                    rowMov["AUAD02"] = dr.GetValue(dr.GetOrdinal("AUAD02")).ToString();
                    rowMov["TERCAD"] = dr.GetValue(dr.GetOrdinal("TERCAD")).ToString();
                    rowMov["CDIVDT"] = dr.GetValue(dr.GetOrdinal("CDIVDT")).ToString();
                    rowMov["TEINDT"] = dr.GetValue(dr.GetOrdinal("TEINDT")).ToString();
                    rowMov["PCIFAD"] = dr.GetValue(dr.GetOrdinal("PCIFAD")).ToString();
                    rowMov["NNITAD"] = dr.GetValue(dr.GetOrdinal("NNITAD")).ToString();

                    saapp = dr.GetValue(dr.GetOrdinal("SAPRDT")).ToString().Trim();
                    rowMov["SAPRDT"] = saapp;
                    if (saapp != null) saapp = utiles.SAAPPFormat(saapp, "-");
                    rowMov["SAPRDTFormat"] = saapp;

                    tipo = dr.GetValue(dr.GetOrdinal("TICODT")).ToString();
                    rowMov["TICODT"] = tipo;
                    rowMov["TICODTFormat"] = tipo.PadLeft(2, '0');

                    nuco = dr.GetValue(dr.GetOrdinal("NUCODT")).ToString();
                    rowMov["NUCODT"] = nuco;
                    rowMov["NUCODTFormat"] = nuco.PadLeft(5, '0');

                    fecodt = dr.GetValue(dr.GetOrdinal("FECODT")).ToString().Trim();
                    if (fecodt != "" && fecodt != "0") rowMov["FECODT"] = utiles.FechaToFormatoCG(fecodt).ToShortDateString();
                    else rowMov["FECODT"] = "";

                    rowMov["SIMIDT"] = dr.GetValue(dr.GetOrdinal("SIMIDT")).ToString();

                    if (this.planCamposExtendidos)
                    {
                        rowMov["PRFDDX"] = dr.GetValue(dr.GetOrdinal("PRFDDX")).ToString();
                        rowMov["NFAADX"] = dr.GetValue(dr.GetOrdinal("NFAADX")).ToString();
                        rowMov["NFARDX"] = dr.GetValue(dr.GetOrdinal("NFARDX")).ToString();

                        fivadx = dr.GetValue(dr.GetOrdinal("FIVADX")).ToString().Trim();
                        if (fivadx != "" && fivadx != "0") rowMov["FIVADX"] = utiles.FechaToFormatoCG(fivadx).ToShortDateString();
                        else rowMov["FIVADX"] = "";

                        rowMov["USA1DX"] = dr.GetValue(dr.GetOrdinal("USA1DX")).ToString();
                        rowMov["USA2DX"] = dr.GetValue(dr.GetOrdinal("USA2DX")).ToString();
                        rowMov["USA3DX"] = dr.GetValue(dr.GetOrdinal("USA3DX")).ToString();
                        rowMov["USA4DX"] = dr.GetValue(dr.GetOrdinal("USA4DX")).ToString();
                        rowMov["USA5DX"] = dr.GetValue(dr.GetOrdinal("USA5DX")).ToString();
                        rowMov["USA6DX"] = dr.GetValue(dr.GetOrdinal("USA6DX")).ToString();
                        rowMov["USA7DX"] = dr.GetValue(dr.GetOrdinal("USA7DX")).ToString();
                        rowMov["USA8DX"] = dr.GetValue(dr.GetOrdinal("USA8DX")).ToString();
                        rowMov["USN1DX"] = dr.GetValue(dr.GetOrdinal("USN1DX")).ToString();
                        rowMov["USN2DX"] = dr.GetValue(dr.GetOrdinal("USN2DX")).ToString();
                        rowMov["USF1DX"] = dr.GetValue(dr.GetOrdinal("USF1DX")).ToString();
                        rowMov["USF2DX"] = dr.GetValue(dr.GetOrdinal("USF2DX")).ToString();
                    }

                    this.dtMov.Rows.Add(rowMov);

                    contReg++;
                }

                if (contReg > 0)
                {
                    if (contReg > 1)
                    {
                        rowMov = this.dtMov.NewRow();
                        rowMov["CAUXDT"] = "";
                        rowMov["FDOCDT"] = "";
                        rowMov["FDOCDT"] = "";
                        rowMov["FEVEDT"] = "TOTALES";
                        rowMov["TMOVDT"] = "";
                        rowMov["MONTDT"] = sumMontdt.ToString("N2", this.LP.MyCultureInfo);
                        rowMov["MOSMAD"] = sumMosmaddt.ToString("N2", this.LP.MyCultureInfo);
                        this.dtMov.Rows.Add(rowMov);

                        this.radGridViewMov.Rows[this.radGridViewMov.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                    }

                    importe = Convert.ToDecimal(this.SaldoInicialDocumento);
                    this.ucConsAuxCab.SaldoInicialDesc = importe.ToString("N2", this.LP.MyCultureInfo);

                    //Calcular saldo final
                    try
                    {
                        //Saldo final es la suma de los importes + el saldo inicial
                        decimal saldoIniDoc = Convert.ToDecimal(this.SaldoInicialDocumento);
                        sumMontdt += saldoIniDoc;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.ucConsAuxCab.SaldoFinalDesc = sumMontdt.ToString("N2", this.LP.MyCultureInfo);

                    this.ucConsAuxCab.TotalDebeDesc = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.TotalHaberDesc = totalHaberML.ToString("N2", this.LP.MyCultureInfo);


                    //Ocultar columna moneda extranjera
                    if (this.DatosMonedaExt != "1")
                    {
                        this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;
                    }
                    else
                    {
                        importe = 0;
                        this.ucConsAuxCab.SaldoInicialMEDesc = importe.ToString("N2", this.LP.MyCultureInfo);   //Duda ... como se calcula
                        this.ucConsAuxCab.SaldoFinalMEDesc = importe.ToString("N2", this.LP.MyCultureInfo);   //Duda ... como se calcula
                        this.ucConsAuxCab.TotalDebeMEDesc = totalDebeME.ToString("N2", this.LP.MyCultureInfo); 
                        this.ucConsAuxCab.TotalHaberMEDesc = totalHaberME.ToString("N2", this.LP.MyCultureInfo);
                    }

                    //Ocultar columnas dependiendo de la cuenta
                    this.OcultarColumnasDadoCuenta();

                    this.radGridViewMov.Visible = true;
                }
                else
                {
                    //No hay movimientos
                    utiles.ButtonEnabled(ref this.radButtonComprobante, false);
                    utiles.ButtonEnabled(ref this.radButtonExportar, false);

                    this.radContextMenuClickDerecho.Items["menuItemVerComprobante"].Enabled = false;
                    this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                    this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                    this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                    this.lblResult.Text = "No existe o no se han podido recuperar los movimientos para el criterio de selección indicado";    //Falta traducir
                    this.lblResult.Visible = true;
                    this.radGridViewMov.Visible = false;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                utiles.ButtonEnabled(ref this.radButtonComprobante, false);
                utiles.ButtonEnabled(ref this.radButtonExportar, false);

                this.radContextMenuClickDerecho.Items["menuItemVerComprobante"].Enabled = false;
                this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                this.lblResult.Text = "No se han podido recuperar los movimientos";    //Falta traducir
                this.lblResult.Visible = true;
                this.radGridViewMov.Visible = false;
            }

            if (this.radGridViewMov.Visible)
            {
                for (int i = 0; i < this.radGridViewMov.Columns.Count; i++)
                    this.radGridViewMov.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                //this.radGridViewComp.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewMov.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.None;

                //this.radGridViewComp.MasterTemplate.BestFitColumns();
                this.radGridViewMov.Rows[0].IsCurrent = true;
            }
        }

        /// <summary>
        /// Devuelve la consulta donde se recuperan los movimientos
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsulta()
        {
            string query = "";
            try
            {
                if (this.planCamposExtendidos)
                {
                    query = "select CAUXDT, CLDODT, NDOCDT, FDOCDT, FEVEDT, TMOVDT, MONTDT, MOSMAD, DESCAD, CDDOAD, NDDOAD, TAAD01, AUAD01, TAAD02, AUAD02, ";
                    query += "TERCAD, CDIVDT, TEINDT, PCIFAD, NNITAD, SAPRDT, TICODT, NUCODT, FECODT, SIMIDT, PRFDDX, NFAADX, NFARDX, FIVADX, ";
                    query += "USA1DX, USA2DX, USA3DX, USA4DX, USA5DX, USA6DX, USA7DX, USA8DX, USN1DX, USN2DX, USF1DX, USF2DX ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLB01 left join " + GlobalVar.PrefijoTablaCG + "GLBX1 on  CCIADT = CCIADX AND SAPRDT = SAPRDX AND ";
                    query += "TICODT = TICODX AND NUCODT = NUCODX AND SIMIDT = SIMIDX ";
                    
                    //if (this.PosAux != "-1") query += ", " + GlobalVar.PrefijoTablaCG + "GLM03 ";

                    query += "where ";
                    //query += "STATDT='E' and TAUXDT ='" + this.TipoAuxCodigo + "' and ";
                    query += "STATDT='E' and ";
                    query += "CCIADT='" + this.CompaniaCodigo + "' ";
                }
                else
                {
                    query = "select CAUXDT, CLDODT, NDOCDT, FDOCDT, FEVEDT, TMOVDT, MONTDT, MOSMAD, DESCAD, CDDOAD, NDDOAD, TAAD01, AUAD01, TAAD02, AUAD02, ";
                    query += "TERCAD, CDIVDT, TEINDT, PCIFAD, NNITAD, SAPRDT, TICODT, NUCODT, FECODT, SIMIDT ";

                    //if (this.PosAux != "-1") query += "from " + GlobalVar.PrefijoTablaCG + "GLB01, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLB01 ";

                    query += "where ";
                    //query += "STATDT='E' and TAUXDT ='" + this.TipoAuxCodigo + "' and ";
                    query += "STATDT='E' and ";
                    query += "CCIADT='" + this.CompaniaCodigo + "' ";
                }

                //Posición del auxiliar
                /*if (this.PosAux != "-1")
                {
                    query += "TIPLMC = TIPLDT and CUENMC = CUENDT and ";
                    switch (this.PosAux)
                    {
                        case "1":
                            query += "TAU1MC ='" + this.TipoAuxCodigo + "' and ";
                            break;
                        case "2":
                            query += "TAU2MC ='" + this.TipoAuxCodigo + "' and ";
                            break;
                        case "3":
                            query += "TAU3MC ='" + this.TipoAuxCodigo + "' and ";
                            break;
                    }
                }*/

                switch (this.PosAux)
                {
                    case "1":
                        query += "and TAUXDT ='" + this.TipoAuxCodigo + "' ";
                        break;
                    case "2":
                        query += "and TAAD01 ='" + this.TipoAuxCodigo + "' ";
                        break;
                    case "3":
                        query += "and TAAD02 ='" + this.TipoAuxCodigo + "' ";
                        break;
                    case "-1":
                        query += "and (TAUXDT ='" + this.TipoAuxCodigo + "' or  TAAD01 ='" + this.TipoAuxCodigo + "' or TAAD02 ='" + this.TipoAuxCodigo + "') ";
                        break;
                    default:
                        query += "and TAUXDT ='" + this.TipoAuxCodigo + "' ";
                        break;
                }

                if (this.CtaAuxCodigo != "")
                {
                    switch (this.PosAux)
                    {
                        case "1":
                            query += " and CAUXDT = '" + this.CtaAuxCodigo + "' ";
                            break;
                        case "2":
                            query += " and AUAD01 ='" + this.CtaAuxCodigo + "' ";
                            break;
                        case "3":
                            query += " and AUAD02 ='" + this.CtaAuxCodigo + "' ";
                            break;
                        case "-1":
                            query += " and (CAUXDT ='" + this.CtaAuxCodigo + "' or  AUAD01 ='" + this.CtaAuxCodigo + "' or AUAD02 ='" + this.CtaAuxCodigo + "') ";
                            break;
                        default:
                            query += " and CAUXDT ='" + this.CtaAuxCodigo + "' ";
                            break;
                    }
                }                    

                if ((this.Clase != null && this.Clase.Trim() != "") || (this.NoDocumento != null && this.NoDocumento.Trim() != ""))
                    query += " and TIPLDT='" + this.PlanCodigo + "' and CLDODT ='" + this.Clase + "' and NDOCDT = " + this.NoDocumento;
                else query += " and TIPLDT='" + this.PlanCodigo + "' ";

                query += " and SAPRDT >= " + this.AAPPDesde + " and SAPRDT <= " + this.AAPPHasta + " and CUENDT='" + this.CtaMayorCodigo + "' ";
                query += "Order by CAUXDT";
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
        /// Oculta columnas dependiendo de los datos de la cuenta
        /// </summary>
        private void OcultarColumnasDadoCuenta()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";

                if (this.planCamposExtendidos)
                {
                    query += "left join " + GlobalVar.PrefijoTablaCG + "GLMX3 on ";
                    query += "TIPLMC = TIPLMX and CUENMC = CUENMX ";
                }

                query += " where TIPLMC = '" + this.PlanCodigo + "' and CUENMC = '" + this.CtaMayorCodigo + "'";

                string TDOCMC = "";
                string FEVEMC = "";
                string NDDOMC = "";
                string TERMMC = "";
                string TIMOMC = "";
                string TAU1MC = "";
                string TAU2MC = "";
                string TAU3MC = "";
                string RNITMC = "";

                string FGPRMX = "";
                string FGFAMX = "";
                string FGFRMX = "";
                string FGDVMX = "";
                string FG01MX = "";
                string FG02MX = "";
                string FG03MX = "";
                string FG04MX = "";
                string FG05MX = "";
                string FG06MX = "";
                string FG07MX = "";
                string FG08MX = "";
                string FG09MX = "";
                string FG10MX = "";
                string FG11MX = "";
                string FG12MX = "";
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    TDOCMC = dr.GetValue(dr.GetOrdinal("TDOCMC")).ToString().Trim();
                    FEVEMC = dr.GetValue(dr.GetOrdinal("FEVEMC")).ToString().Trim();
                    NDDOMC = dr.GetValue(dr.GetOrdinal("NDDOMC")).ToString().Trim();
                    TERMMC = dr.GetValue(dr.GetOrdinal("TERMMC")).ToString().Trim();
                    TIMOMC = dr.GetValue(dr.GetOrdinal("TIMOMC")).ToString().Trim();
                    TAU1MC = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString().Trim();
                    TAU2MC = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString().Trim();
                    TAU3MC = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString().Trim();
                    RNITMC = dr.GetValue(dr.GetOrdinal("RNITMC")).ToString().Trim();

                    if (this.planCamposExtendidos)
                    {
                        //Ocultar las columnas que no se utilizan de los campos extendidos
                        FGPRMX = dr.GetValue(dr.GetOrdinal("FGPRMX")).ToString().Trim();
                        FGFAMX = dr.GetValue(dr.GetOrdinal("FGFAMX")).ToString().Trim();
                        FGFRMX = dr.GetValue(dr.GetOrdinal("FGFRMX")).ToString().Trim();
                        FGDVMX = dr.GetValue(dr.GetOrdinal("FGDVMX")).ToString().Trim();
                        FG01MX = dr.GetValue(dr.GetOrdinal("FG01MX")).ToString().Trim();
                        FG02MX = dr.GetValue(dr.GetOrdinal("FG02MX")).ToString().Trim();
                        FG03MX = dr.GetValue(dr.GetOrdinal("FG03MX")).ToString().Trim();
                        FG04MX = dr.GetValue(dr.GetOrdinal("FG04MX")).ToString().Trim();
                        FG05MX = dr.GetValue(dr.GetOrdinal("FG05MX")).ToString().Trim();
                        FG06MX = dr.GetValue(dr.GetOrdinal("FG06MX")).ToString().Trim();
                        FG07MX = dr.GetValue(dr.GetOrdinal("FG07MX")).ToString().Trim();
                        FG08MX = dr.GetValue(dr.GetOrdinal("FG08MX")).ToString().Trim();
                        FG09MX = dr.GetValue(dr.GetOrdinal("FG09MX")).ToString().Trim();
                        FG10MX = dr.GetValue(dr.GetOrdinal("FG10MX")).ToString().Trim();
                        FG11MX = dr.GetValue(dr.GetOrdinal("FG11MX")).ToString().Trim();
                        FG12MX = dr.GetValue(dr.GetOrdinal("FG12MX")).ToString().Trim();
                        
                        if (FGPRMX == "0" || FGPRMX == "") this.radGridViewMov.Columns["PRFDDX"].IsVisible = false;
                        if (FGFAMX == "0" || FGFAMX == "") this.radGridViewMov.Columns["NFAADX"].IsVisible = false;
                        if (FGFRMX == "0" || FGFRMX == "") this.radGridViewMov.Columns["NFARDX"].IsVisible = false;
                        if (FGDVMX == "0" || FGDVMX == "") this.radGridViewMov.Columns["FIVADX"].IsVisible = false;
                        if (FG01MX == "0" || FG01MX == "") this.radGridViewMov.Columns["USA1DX"].IsVisible = false;
                        if (FG02MX == "0" || FG02MX == "") this.radGridViewMov.Columns["USA2DX"].IsVisible = false;
                        if (FG03MX == "0" || FG03MX == "") this.radGridViewMov.Columns["USA3DX"].IsVisible = false;
                        if (FG04MX == "0" || FG04MX == "") this.radGridViewMov.Columns["USA4DX"].IsVisible = false;
                        if (FG05MX == "0" || FG05MX == "") this.radGridViewMov.Columns["USA5DX"].IsVisible = false;
                        if (FG06MX == "0" || FG06MX == "") this.radGridViewMov.Columns["USA6DX"].IsVisible = false;
                        if (FG07MX == "0" || FG07MX == "") this.radGridViewMov.Columns["USA7DX"].IsVisible = false;
                        if (FG08MX == "0" || FG08MX == "") this.radGridViewMov.Columns["USA8DX"].IsVisible = false;
                        if (FG09MX == "0" || FG09MX == "") this.radGridViewMov.Columns["USN1DX"].IsVisible = false;
                        if (FG10MX == "0" || FG10MX == "") this.radGridViewMov.Columns["USN2DX"].IsVisible = false;
                        if (FG11MX == "0" || FG11MX == "") this.radGridViewMov.Columns["USF1DX"].IsVisible = false;
                        if (FG12MX == "0" || FG12MX == "") this.radGridViewMov.Columns["USF2DX"].IsVisible = false;
                    }
                }

                dr.Close();

                if (TDOCMC == "N")
                {
                    this.radGridViewMov.Columns["CLDODT"].IsVisible = false;   //Clase
                    this.radGridViewMov.Columns["NDOCDT"].IsVisible = false;   //Numero
                    this.radGridViewMov.Columns["FDOCDT"].IsVisible = false;   //Fecha documento
                }

                if (FEVEMC == "N") this.radGridViewMov.Columns["FEVEDT"].IsVisible = false;    //Fecha vencimiento

                if (NDDOMC == "N")
                {
                    this.radGridViewMov.Columns["CDDOAD"].IsVisible = false;   //Clase 2do documento
                    this.radGridViewMov.Columns["NDDOAD"].IsVisible = false;   //Numero 2do documento
                }

                if (TERMMC == "N") this.radGridViewMov.Columns["TERCAD"].IsVisible = false;    //Tercer importe

                if (TIMOMC == "N") this.radGridViewMov.Columns["MOSMAD"].IsVisible = false;    //Moneda Extranjera

                if (TAU1MC == "")
                {
                    this.radGridViewMov.Columns["TAUXDT"].IsVisible = false;   //Tipo 1er auxiliar
                    this.radGridViewMov.Columns["CAUXDT"].IsVisible = false;   //Cuenta 1er auxiliar
                }

                if (TAU2MC == "")
                {
                    this.radGridViewMov.Columns["TAAD01"].IsVisible = false;   //Tipo 2do auxiliar
                    this.radGridViewMov.Columns["AUAD01"].IsVisible = false;   //Cuenta 2do auxiliar
                }

                if (TAU3MC == "")
                {
                    this.radGridViewMov.Columns["TAAD02"].IsVisible = false;   //Tipo 3er auxiliar
                    this.radGridViewMov.Columns["AUAD02"].IsVisible = false;   //Cuenta 3er auxiliar
                }

                if (RNITMC == "N")
                {
                    this.radGridViewMov.Columns["PCIFAD"].IsVisible = false;   //País
                    this.radGridViewMov.Columns["NNITAD"].IsVisible = false;   //NIF
                    this.radGridViewMov.Columns["CDIVDT"].IsVisible = false;   //Código IVA
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

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
                    DateTableDatos = this.dtMov,
                    Cabecera = true,

                    //Titulo
                    Titulo = this.Text + " -  Tipo de auxiliar: " + this.TipoAuxDesc.Replace("  -  ", "-")       //Falta traducir
                };
                if (this.CtaAuxCodigo != "") excelImport.Titulo += "  Cuenta de Auxiliar: " + this.CtaAuxDesc.Replace("  -  ", "-");
                if (this.GrupoCodigo != "")
                {
                    excelImport.Titulo += "  Grupo de compañías: " + this.GrupoDesc.Replace("  -  ", "-");                //Falta traducir
                    excelImport.Titulo += "  Plan: " + this.PlanDesc.Replace("  -  ", "-");                               //Falta traducir
                }
                else excelImport.Titulo += "  Compañía: " + this.CompaniaDesc.Replace("  -  ", "-");                      //Falta traducir
                excelImport.Titulo += "  Perido: " + ucConsAuxCab.AAPPDesde + " Hasta " + ucConsAuxCab.AAPPHasta;       //Falta traducir
                if (this.CtaMayorCodigo != "") excelImport.Titulo += "  Cuenta de Mayor: " + this.CtaMayorDesc.Replace("  -  ", "-");

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.radGridViewMov.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.radGridViewMov.Columns[i].HeaderText;                   //Nombre de la columna

                    switch (this.radGridViewMov.Columns[i].Name)
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

                    nombreTipoVisible[2] = this.radGridViewMov.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.radGridViewMov.SelectedRows.Count > 0 && this.radGridViewMov.SelectedRows.Count < this.radGridViewMov.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.radGridViewMov.SelectedRows.Count; i++)
                    {
                        indice = this.radGridViewMov.Rows.IndexOf(this.radGridViewMov.SelectedRows[i]);

                        if (this.radGridViewMov.Rows.Count - 1 == indice)
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
            string titulo = this.Text + " -  Tipo de auxiliar: " + this.TipoAuxDesc.Replace("  -  ", "-");

            if (this.CtaAuxCodigo != "") titulo += "  Cuenta de Auxiliar: " + this.CtaAuxDesc.Replace("  -  ", "-");
            if (this.GrupoCodigo != "")
            {
                titulo += "  Grupo de compañías: " + this.GrupoDesc.Replace("  -  ", "-");
                titulo += "  Plan: " + this.PlanDesc.Replace("  -  ", "-");
            }
            else titulo += "  Compañía: " + this.CompaniaDesc.Replace("  -  ", "-");

            titulo += "  Perido: " + ucConsAuxCab.AAPPDesde + " Hasta " + ucConsAuxCab.AAPPHasta;
            if (this.CtaMayorCodigo != "") titulo += "  Cuenta de Mayor: " + this.CtaMayorDesc.Replace("  -  ", "-");

            //Columnas
            ArrayList descColumnas = new ArrayList();
            string[] nombreTipoVisible;
            for (int i = 0; i < this.radGridViewMov.ColumnCount; i++)
            {
                nombreTipoVisible = new string[3];
                nombreTipoVisible[0] = this.radGridViewMov.Columns[i].HeaderText;                   //Nombre de la columna

                switch (this.radGridViewMov.Columns[i].Name)
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

                nombreTipoVisible[2] = this.radGridViewMov.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                descColumnas.Add(nombreTipoVisible);
            }

            this.ExportarGrid(ref this.radGridViewMov, titulo, false, null, "Movimientos", ref descColumnas, null);
        }

        /// <summary>
        /// Ver el comprobante del movimiento
        /// </summary>
        private void VerComprobante()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewMov.SelectedRows.Count == 0)
                {
                    if (this.radGridViewMov.Rows.Count > 1)
                    {
                        RadMessageBox.Show("Debe seleccionar un movimiento", "Error");  //Falta traducir
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else this.radGridViewMov.Rows[0].IsSelected = true;
                }

                if (this.radGridViewMov.SelectedRows.Count > 1)
                {
                    RadMessageBox.Show("Debe seleccionar solo un movimiento", "Error");  //Falta traducir
                    Cursor.Current = Cursors.Default;
                    return;
                }

                int indice = this.radGridViewMov.Rows.IndexOf(this.radGridViewMov.CurrentRow);

                bool lineaTotales = false;
                if (this.radGridViewMov.Rows.Count > 1 && this.radGridViewMov.Rows.Count - 1 == indice) lineaTotales = true;

                if (!lineaTotales)
                {
                    string tico = this.radGridViewMov.Rows[indice].Cells["TICODT"].Value.ToString();
                    string nuco = this.radGridViewMov.Rows[indice].Cells["NUCODT"].Value.ToString();
                    string sapr = this.radGridViewMov.Rows[indice].Cells["SAPRDT"].Value.ToString();

                    frmConsAuxViewComp frmViewConsComp = new frmConsAuxViewComp
                    {
                        TipoAuxCodigo = this.TipoAuxCodigo,
                        TipoAuxDesc = this.TipoAuxDesc,
                        CompaniaCodigo = this.CompaniaCodigo,
                        CompaniaDesc = this.CompaniaDesc,
                        GrupoCodigo = this.GrupoCodigo,
                        GrupoDesc = this.GrupoDesc,
                        PlanCodigo = this.PlanCodigo,
                        PlanDesc = this.PlanDesc,
                        //frmViewConsComp.AAPPDesde = this.AAPPDesde;
                        AAPPDesde = sapr,
                        AAPPDesdeFormat = this.AAPPDesdeFormat,
                        AAPPHasta = this.AAPPHasta,
                        AAPPHastaFormat = this.AAPPHastaFormat,
                        CtaAuxCodigo = this.CtaAuxCodigo,
                        CtaAuxDesc = this.CtaAuxDesc,
                        CtaMayorCodigo = this.CtaMayorCodigo,
                        CtaMayorDesc = this.CtaMayorDesc,
                        PosAux = this.PosAux,
                        Documentos = this.Documentos,
                        DatosMonedaExt = this.DatosMonedaExt,
                        TipoComp = tico,
                        NoComp = nuco,
                        PlanCamposExtendidos = this.planCamposExtendidos,
                        DtGLMX2 = this.dtGLMX2
                    };
                    frmViewConsComp.Show(this);
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("errSelCtaMov", "Debe seleccionar un movimiento"), this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemVerComprobante = new RadMenuItem(this.radButtonComprobante.Text)
            {
                Name = "menuItemVerComprobante"
            };
            menuItemVerComprobante.Click += new EventHandler(RadButtonComprobante_Click);
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

            this.radContextMenuClickDerecho.Items.Add(menuItemVerComprobante);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportar);
            this.radContextMenuClickDerecho.Items.Add(menuItemSalir);
        }
        #endregion              
    }
}