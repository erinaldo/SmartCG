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
    public partial class frmConsAuxViewDoc :  frmPlantilla, IReLocalizable
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
        public string FEVEMC { get; set; }
        public bool CalcularPrdoMedioPago { get; set; }

        private string calendario;

        ArrayList aEmpresas = null;

        Dictionary<string, string> displayNamesDoc;
        DataTable dtDoc;

        public frmConsAuxViewDoc()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbFiltro.ElementTree.EnableApplicationThemeName = false;
            this.gbFiltro.ThemeName = "ControlDefault";

            this.radGridViewDoc.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxViewDoc_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar Documentos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Cargar Menu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            //Muestra la cabecera de la consulta (criterio de selección / saldos / totales)
            this.CargarValoresCabecera();

            //Crear el TGGrid
            this.BuildtgGridDocumentos();

            //Cargar los documentos
            this.CargarInfoDocumentos();

            //Calcular Saldos y Totales (al inicio son los saldos y totales dentro del periodo)
            this.RecalcularTotales();
        }

        private void TgGridDoc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.VerMovimientos();
        }
        
        private void BtnFiltroDocPeriodo_Click(object sender, EventArgs e)
        {
            this.lblFiltro.Text = "Documentos del Periodo"; //Falta traducir

            utiles.ButtonEnabled(ref this.btnFiltroDocPeriodo, false);
            utiles.ButtonEnabled(ref this.btnFiltroDocTodos, true);
            this.dtDoc.DefaultView.RowFilter = "num_mov_dentro_periodo > 0";
            this.radGridViewDoc.Refresh();

            this.RecalcularTotales();

            //Ningún documento seleccionado
            //this.radGridViewDoc.ClearSelection();
            //this.radGridViewDoc.Refresh();
        }

        private void BtnFiltroDocTodos_Click(object sender, EventArgs e)
        {
            this.lblFiltro.Text = "Todos los Documentos"; //Falta traducir

            utiles.ButtonEnabled(ref this.btnFiltroDocPeriodo, true);
            utiles.ButtonEnabled(ref this.btnFiltroDocTodos, false);
            this.dtDoc.DefaultView.RowFilter = "";
            this.radGridViewDoc.Refresh();

            this.RecalcularTotales();

            //Ningún documento seleccionado
            //this.tgGridDoc.ClearSelection();
            //this.tgGridDoc.Refresh();
        }

        private void RadButtonMovimientos_Click(object sender, EventArgs e)
        {
            this.VerMovimientos();
        }

        private void RadButtonCalcularPrdoMedioPago_Click(object sender, EventArgs e)
        {
            this.LLamarCalcularPrdMedioPago();
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

        private void RadGridViewDoc_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewDoc.Columns.Contains("CCIADT"))
                if (this.CompaniaCodigo != "") this.radGridViewDoc.Columns["CCIADT"].IsVisible = false;
                else this.radGridViewDoc.Columns["CCIADT"].Width = 80;     //Falta traducir

            if (this.radGridViewDoc.Columns.Contains("DOCUMENTO")) this.radGridViewDoc.Columns["DOCUMENTO"].Width = 80;     //Falta traducir
            if (this.radGridViewDoc.Columns.Contains("CAUXDT")) this.radGridViewDoc.Columns["CAUXDT"].Width = 45;       //Falta traducir
            if (this.radGridViewDoc.Columns.Contains("NOMBMA")) this.radGridViewDoc.Columns["NOMBMA"].Width = 140;       //Falta traducir
            if (this.radGridViewDoc.Columns.Contains("FDOCDT")) this.radGridViewDoc.Columns["FDOCDT"].Width = 90;       //Falta traducir

            if (this.radGridViewDoc.Columns.Contains("SALDOANTML"))
            {
                this.radGridViewDoc.Columns["SALDOANTML"].Width = 120;        //Falta traducir
                this.radGridViewDoc.Columns["SALDOANTML"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewDoc.Columns.Contains("DEBEML"))
            {
                this.radGridViewDoc.Columns["DEBEML"].Width = 120;        //Falta traducir
                this.radGridViewDoc.Columns["DEBEML"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewDoc.Columns.Contains("HABERML"))
            {
                this.radGridViewDoc.Columns["HABERML"].Width = 120;        //Falta traducir
                this.radGridViewDoc.Columns["HABERML"].TextAlignment = ContentAlignment.MiddleRight;
            }
            if (this.radGridViewDoc.Columns.Contains("SALDOML"))
            {
                this.radGridViewDoc.Columns["SALDOML"].Width = 120;        //Falta traducir
                this.radGridViewDoc.Columns["SALDOML"].TextAlignment = ContentAlignment.MiddleRight;
            }

            if (this.DatosMonedaExt != "1")
            {
                if (this.radGridViewDoc.Columns.Contains("SALDOANTME")) this.radGridViewDoc.Columns["SALDOANTME"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("DEBEME")) this.radGridViewDoc.Columns["DEBEME"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("HABERME")) this.radGridViewDoc.Columns["HABERME"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("SALDOME")) this.radGridViewDoc.Columns["SALDOME"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewDoc.Columns.Contains("SALDOANTME"))
                {
                    this.radGridViewDoc.Columns["SALDOANTME"].Width = 120;         //Falta traducir
                    this.radGridViewDoc.Columns["SALDOANTME"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewDoc.Columns.Contains("DEBEME"))
                {
                    this.radGridViewDoc.Columns["DEBEME"].Width = 120;        //Falta traducir
                    this.radGridViewDoc.Columns["DEBEME"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewDoc.Columns.Contains("HABERME"))
                {
                    this.radGridViewDoc.Columns["HABERME"].Width = 120;        //Falta traducir
                    this.radGridViewDoc.Columns["HABERME"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewDoc.Columns.Contains("SALDOME"))
                {
                    this.radGridViewDoc.Columns["SALDOME"].Width = 120;        //Falta traducir
                    this.radGridViewDoc.Columns["SALDOME"].TextAlignment = ContentAlignment.MiddleRight;
                }
            }

            if (this.FEVEMC == "N")
            {
                if (this.radGridViewDoc.Columns.Contains("FEVEDT")) this.radGridViewDoc.Columns["FEVEDT"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("FECODT")) this.radGridViewDoc.Columns["FECODT"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("DIASPAGO")) this.radGridViewDoc.Columns["DIASPAGO"].IsVisible = false;
                if (this.radGridViewDoc.Columns.Contains("DIASPDTEPAGO")) this.radGridViewDoc.Columns["DIASPDTEPAGO"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewDoc.Columns.Contains("FEVEDT")) this.radGridViewDoc.Columns["FEVEDT"].Width = 90;        //Falta traducir
                if (this.radGridViewDoc.Columns.Contains("FECODT")) this.radGridViewDoc.Columns["FECODT"].Width = 90;        //Falta traducir

                if (this.radGridViewDoc.Columns.Contains("DIASPAGO"))
                {
                    this.radGridViewDoc.Columns["DIASPAGO"].Width = 85;        //Falta traducir
                    this.radGridViewDoc.Columns["DIASPAGO"].TextAlignment = ContentAlignment.MiddleRight;
                }
                if (this.radGridViewDoc.Columns.Contains("DIASPDTEPAGO"))
                {
                    this.radGridViewDoc.Columns["DIASPDTEPAGO"].Width = 85;        //Falta traducir
                    this.radGridViewDoc.Columns["DIASPDTEPAGO"].TextAlignment = ContentAlignment.MiddleRight;
                }
            }

            if (this.radGridViewDoc.Columns.Contains("num_mov_dentro_periodo")) this.radGridViewDoc.Columns["num_mov_dentro_periodo"].IsVisible = false;        //Falta traducir
            if (this.radGridViewDoc.Columns.Contains("CLDODT")) this.radGridViewDoc.Columns["CLDODT"].IsVisible = false;        //Falta traducir
            if (this.radGridViewDoc.Columns.Contains("NDOCDT")) this.radGridViewDoc.Columns["NDOCDT"].IsVisible = false;        //Falta traducir
        }

        private void RadGridViewDoc_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement is Telerik.WinControls.UI.GridHeaderCellElement)
            {
                e.CellElement.TextWrap = true;
            }
        }

        private void RadGridViewDoc_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadGridViewDoc_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewDoc, ref this.selectAll);
        }

        private void RadButtonMovimientos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonMovimientos);
        }

        private void RadButtonMovimientos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonMovimientos);
        }

        private void RadButtonCalcularPrdoMedioPago_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCalcularPrdoMedioPago);
        }

        private void RadButtonCalcularPrdoMedioPago_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCalcularPrdoMedioPago);
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

        private void BtnFiltroDocPeriodo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnFiltroDocPeriodo);
        }

        private void BtnFiltroDocPeriodo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnFiltroDocPeriodo);
        }

        private void BtnFiltroDocTodos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnFiltroDocTodos);
        }

        private void BtnFiltroDocTodos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnFiltroDocTodos);
        }

        private void FrmConsAuxViewDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void FrmConsAuxViewDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Auxiliar Documentos");
        }

        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemConsultaAuxDoc", "Consulta de Auxiliar - Documentos");   //Falta traducir

            if (this.FEVEMC == "D") this.radButtonCalcularPrdoMedioPago.Text = "Calcular periodo medio de cobro";     //Falta traducir
            else this.radButtonCalcularPrdoMedioPago.Text = "Calcular periodo medio de pago";

            this.radButtonMovimientos.Text = this.LP.GetText("lblVerMov", "Ver los Movimientos");   //Falta traducir
            this.radButtonExportar.Text = this.LP.GetText("lblExportar", "Exportar");   //Falta traducir
            this.radButtonSalir.Text = this.LP.GetText("lblSalir", "Salir");   //Falta traducir

            this.lblResult.Text = this.LP.GetText("lblConsultaAuxDocResult", "No existen documentos para el criterio de selección indicado");     //Falta traducir
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

                //Documentos
                this.ucConsAuxCab.MostrarCuentas = this.MostrarCuentas;
                this.ucConsAuxCab.Documentos = this.Documentos;

                //Mostrar datos de moneda extranjera
                if (this.DatosMonedaExt == "1") this.ucConsAuxCab.DatosMonedaExt.Visible = true;
                else this.ucConsAuxCab.DatosMonedaExt.Visible = false;
                
                this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = true;

                this.ucConsAuxCab.TotalDebeVisible = true;
                this.ucConsAuxCab.TotalHaberVisible = true;

                this.ucConsAuxCab.ActualizarValores();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que calcula el periodo medio de pago
        /// </summary>
        private void LLamarCalcularPrdMedioPago()
        {
            frmConsAuxViewDocInfPrdoPago frmConsDocInfPagos = new frmConsAuxViewDocInfPrdoPago
            {
                TipoAuxCodigo = this.TipoAuxCodigo,
                TipoAuxDesc = this.TipoAuxDesc,
                CompaniaCodigo = this.CompaniaCodigo,
                CompaniaDesc = this.CompaniaDesc,
                GrupoCodigo = this.GrupoCodigo,
                GrupoDesc = this.GrupoDesc,
                PlanCodigo = this.PlanCodigo,
                PlanDesc = this.PlanDesc,
                AAPPDesde = this.AAPPDesde,
                AAPPDesdeFormat = this.AAPPDesdeFormat,
                AAPPHasta = this.AAPPHasta,
                AAPPHastaFormat = this.AAPPHastaFormat,
                CtaAuxCodigo = this.CtaAuxCodigo,
                CtaAuxDesc = this.CtaAuxDesc,
                CtaMayorCodigo = this.CtaMayorCodigo,
                CtaMayorDesc = this.CtaMayorDesc,
                PosAux = PosAux,
                FEVEMC = this.FEVEMC,
                Datos = this.dtDoc
            };

            frmConsDocInfPagos.Show();
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
                    DateTableDatos = this.dtDoc,

                    //Titulo
                    Titulo = this.Text + " -  Tipo de auxiliar: " + this.TipoAuxDesc.Replace("  -  ", "-"),       //Falta traducir
                    Cabecera = true
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
                for (int i = 0; i < this.radGridViewDoc.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.radGridViewDoc.Columns[i].HeaderText;                   //Nombre de la columna

                    switch (this.radGridViewDoc.Columns[i].Name)
                    {
                        case "SALDOANTML":
                        case "DEBEML":
                        case "HABERML":
                        case "SALDOML":
                        case "SALDOANTME":
                        case "DEBEME":
                        case "HABERME":
                        case "SALDOME":
                            nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                            break;
                        case "DIASPAGO":
                        case "DIASPDTEPAGO":
                            nombreTipoVisible[1] = "numero";                                   //Tipo de la columna
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            break;
                    }
                    //dt.Columns.Add("DIASPAGO", typeof(string));
                    //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                    nombreTipoVisible[2] = this.radGridViewDoc.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.radGridViewDoc.SelectedRows.Count > 0 && this.radGridViewDoc.SelectedRows.Count <= this.radGridViewDoc.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.radGridViewDoc.SelectedRows.Count; i++)
                    {
                        //Coger el índice del DataTable y no de la Grid porque hay un filtro y sino no corresponde
                        indice = ((DataTable)this.radGridViewDoc.DataSource).Rows.IndexOf(((DataRowView)this.radGridViewDoc.Rows[this.radGridViewDoc.SelectedRows[i].Index].DataBoundItem).Row);

                        aIndice.Add(indice);

                        //indice = this.tgGridDoc.SelectedRows[i].Index;    -> Este no es correcto cuando esté filtrado por Documentos del Periodo

                        //if (tgGridDoc.Rows.Count - 1 == indice)
                        //{
                        //Linea Totales
                        /*if (aIndice.Count > 0)
                        {
                            aIndice.Add(indice);
                        }
                        else
                        {
                            //Solo linea de Totales, no se adiciona y se exportan todas las filas
                        }*/
                        //}
                        //else aIndice.Add(indice);
                    }

                    excelImport.IndiceFilasSeleccionadas = aIndice;
                }
                else
                {
                    if (!this.btnFiltroDocPeriodo.Enabled)
                    {
                        int indice = 0;
                        ArrayList aIndice = new ArrayList();
                        for (int i = 0; i < this.radGridViewDoc.Rows.Count; i++)
                        {
                            //Coger el índice del DataTable y no de la Grid porque hay un filtro y sino no corresponde
                            indice = ((DataTable)this.radGridViewDoc.DataSource).Rows.IndexOf(((DataRowView)this.radGridViewDoc.Rows[this.radGridViewDoc.Rows[i].Index].DataBoundItem).Row);

                            aIndice.Add(indice);

                            //indice = this.tgGridDoc.SelectedRows[i].Index;    -> Este no es correcto cuando esté filtrado por Documentos del Periodo

                            /*if (tgGridDoc.Rows.Count - 1 == indice)
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
                            */
                        }

                        excelImport.IndiceFilasSeleccionadas = aIndice;
                    }
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
            for (int i = 0; i < this.radGridViewDoc.ColumnCount; i++)
            {
                nombreTipoVisible = new string[3];
                nombreTipoVisible[0] = this.radGridViewDoc.Columns[i].HeaderText;                   //Nombre de la columna

                switch (this.radGridViewDoc.Columns[i].Name)
                {
                    case "SALDOANTML":
                    case "DEBEML":
                    case "HABERML":
                    case "SALDOML":
                    case "SALDOANTME":
                    case "DEBEME":
                    case "HABERME":
                    case "SALDOME":
                        nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                        break;
                    case "DIASPAGO":
                    case "DIASPDTEPAGO":
                        nombreTipoVisible[1] = "numero";                                   //Tipo de la columna
                        break;
                    default:
                        nombreTipoVisible[1] = "string";
                        break;
                }
                //dt.Columns.Add("DIASPAGO", typeof(string));
                //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                nombreTipoVisible[2] = this.radGridViewDoc.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                descColumnas.Add(nombreTipoVisible);
            }

            this.ExportarGrid(ref this.radGridViewDoc, titulo, false, null, "Documento", ref descColumnas, null);
        }

        /// <summary>
        /// Construye el Grid que almacenará los documentos
        /// </summary>
        private void BuildtgGridDocumentos()
        {
            try
            {
                this.dtDoc = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                this.dtDoc.Columns.Add("CCIADT", typeof(string));
                this.dtDoc.Columns.Add("DOCUMENTO", typeof(string));
                this.dtDoc.Columns.Add("CAUXDT", typeof(string));
                this.dtDoc.Columns.Add("NOMBMA", typeof(string));
                this.dtDoc.Columns.Add("FDOCDT", typeof(string));
                this.dtDoc.Columns.Add("FEVEDT", typeof(string));
                this.dtDoc.Columns.Add("FECODT", typeof(string));
                this.dtDoc.Columns.Add("SALDOANTML", typeof(string));
                this.dtDoc.Columns.Add("DEBEML", typeof(string));
                this.dtDoc.Columns.Add("HABERML", typeof(string));
                this.dtDoc.Columns.Add("SALDOML", typeof(string));
                this.dtDoc.Columns.Add("SALDOANTME", typeof(string));
                this.dtDoc.Columns.Add("DEBEME", typeof(string));
                this.dtDoc.Columns.Add("HABERME", typeof(string));
                this.dtDoc.Columns.Add("SALDOME", typeof(string));
                this.dtDoc.Columns.Add("DIASPAGO", typeof(string));
                this.dtDoc.Columns.Add("DIASPDTEPAGO", typeof(string));
                this.dtDoc.Columns.Add("num_mov_dentro_periodo", typeof(Int32));
                this.dtDoc.Columns.Add("CLDODT", typeof(string));
                this.dtDoc.Columns.Add("NDOCDT", typeof(string));

                this.radGridViewDoc.DataSource = this.dtDoc;
                //Escribe el encabezado de la Grid de Documentos
                this.BuildDisplayNamesDoc();
                this.RadGridViewDocHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Encabezados para la Grid de Documentos
        /// </summary>
        private void BuildDisplayNamesDoc()
        {
            try
            {
                this.displayNamesDoc = new Dictionary<string, string>
                {
                    { "CCIADT", this.LP.GetText("HeaderCompania", "Compañía") },     //Falta traducir
                    { "DOCUMENTO", this.LP.GetText("HeaderDoc", "Documento") },     //Falta traducir
                    { "CAUXDT", this.LP.GetText("HeaderAuxiliar", "Auxiliar") },       //Falta traducir
                    { "NOMBMA", this.LP.GetText("HeaderNombreAux", "Nombre") },       //Falta traducir
                    { "FDOCDT", this.LP.GetText("HeaderFechaDoc", "Fecha Doc.") },        //Falta traducir
                    { "FEVEDT", this.LP.GetText("HeaderVencimiento", "Vencimiento") },        //Falta traducir
                    { "FECODT", this.LP.GetText("HeaderFecha1Canc", "Fecha 1ra Canc.") },        //Falta traducir
                    { "SALDOANTML", this.LP.GetText("HeaderSaldoAntML", "Saldo Anterior ML") },        //Falta traducir
                    { "DEBEML", this.LP.GetText("HeaderDebeML", "Debe ML") },        //Falta traducir
                    { "HABERML", this.LP.GetText("HeaderHaberML", "Haber ML") },        //Falta traducir
                    { "SALDOML", this.LP.GetText("HeaderSaldoML", "Saldo ML") },        //Falta traducir
                    { "SALDOANTME", this.LP.GetText("HeaderSaldoAntME", "Saldo Anterior ME") },        //Falta traducir
                    { "DEBEME", this.LP.GetText("HeaderDebeME", "Debe ME") },        //Falta traducir
                    { "HABERME", this.LP.GetText("HeaderHaberME", "Haber ME") },        //Falta traducir
                    { "SALDOME", this.LP.GetText("HeaderSaldoME", "Saldo ME") }        //Falta traducir
                };

                string cabeceraDiasPagos = "";
                string cabeceraDiasPdtesPago = "";
                switch (this.FEVEMC)
                {
                    case "D":
                        cabeceraDiasPagos = this.LP.GetText("HeaderDiasCobro", "Días de cobro");   //Falta traducir
                        cabeceraDiasPdtesPago = this.LP.GetText("HeaderDiasPdtesCobro", "Días pendientes de cobro");   //Falta traducir
                        break;
                    case "H":
                    default:
                        cabeceraDiasPagos = this.LP.GetText("HeaderDiasPago", "Días de pago");   //Falta traducir
                        cabeceraDiasPdtesPago = this.LP.GetText("HeaderDiasPdtePago", "Días pendientes de pago");   //Falta traducir
                        break;
                }

                this.displayNamesDoc.Add("DIASPAGO", cabeceraDiasPagos);        //Falta traducir
                this.displayNamesDoc.Add("DIASPDTEPAGO", cabeceraDiasPdtesPago);        //Falta traducir
                this.displayNamesDoc.Add("num_mov_dentro_periodo", "num_mov_dentro_periodo");

                this.displayNamesDoc.Add("CLDODT", this.LP.GetText("HeaderClase", "Clase"));
                this.displayNamesDoc.Add("NDOCDT", this.LP.GetText("HeaderDoc", "Documento"));
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Documentos
        /// </summary>
        private void RadGridViewDocHeader()
        {
            try
            {
                if (this.radGridViewDoc.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesDoc)
                    {
                        if (this.radGridViewDoc.Columns.Contains(item.Key)) this.radGridViewDoc.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Carga los documentos en la Grid
        /// </summary>
        private void CargarInfoDocumentos()
        {
            IDataReader dr = null;
            try
            {
                string query = this.ObtenerConsulta();

                string cciadt = "";
                string cldodt = "";
                string ndocdt = "";
                string cauxdt = "";

                /*decimal totalGlobalDebe = 0;
                decimal totalGlobalHaber = 0;

                string debe_haber = "";
                decimal montdt = 0;*/

                int contReg = 0;

                string compania = "";
                string plan = "";
                              
                bool insertarReg = false;

                string fechaDocStr = "";
                int fechaDoc = 0;
                string fechaVencStr = "";
                int fechaVenc = 0;
                string fecha1CancStr = "";
                int fecha1Canc = 0;
                string saldoInicialStr = "";
                decimal saldoInicialML = 0;
                string totalDebeMLStr = "";
                decimal totalDebeML = 0;
                string totalHaberMLStr = "";
                decimal totalHaberML = 0;
                string saldoInicialMEStr = "";
                decimal saldoInicialME = 0;
                string totalDebeMEStr = "";
                decimal totalDebeME = 0;
                string totalHaberMEStr = "";
                decimal totalHaberME = 0;
                string saldoFinalMLStr = "";
                decimal saldoFinalML = 0;
                string saldoFinalMEStr = "";
                decimal saldoFinalME = 0;
                string num_mov_dentroPeriodoStr = "";
                int num_mov_dentroPeriodo = 0;

                decimal sumSaldoInicialML = 0;
                decimal sumTotalDebeML = 0;
                decimal sumTotalHaberML = 0;
                decimal sumSaldoFinalML = 0;
                decimal sumSaldoInicialME = 0;
                decimal sumTotalDebeME = 0;
                decimal sumTotalHaberME = 0;
                decimal sumSaldoFinalME = 0;

                string nombreAux = "";

                //Calcular la fecha del último período
                //buscar última fecha del periodo
                string fechaFinPeriodo = utilesCG.ObtenerFechaFinCalendarioDadoPeriodo(this.calendario, AAPPHasta);

                DateTime fechaUltPeriodo = utiles.FormatoCGToFecha(fechaFinPeriodo);

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    insertarReg = false;

                    cciadt = dr.GetValue(dr.GetOrdinal("CCIADT")).ToString();
                    cldodt = dr.GetValue(dr.GetOrdinal("CLDODT")).ToString();
                    ndocdt = dr.GetValue(dr.GetOrdinal("NDOCDT")).ToString();
                    cauxdt = dr.GetValue(dr.GetOrdinal("CAUXDT")).ToString();

                    compania = dr.GetValue(dr.GetOrdinal("CCIADT")).ToString();
                    plan  = dr.GetValue(dr.GetOrdinal("TIPLDT")).ToString();

                    fechaDocStr = dr.GetValue(dr.GetOrdinal("FDOCDT")).ToString().Trim();
                    fechaVencStr = dr.GetValue(dr.GetOrdinal("FEVEDT")).ToString().Trim();
                    fecha1CancStr = dr.GetValue(dr.GetOrdinal("FECODT")).ToString().Trim();
                    saldoInicialStr = dr.GetValue(dr.GetOrdinal("SALDO_INI_ML")).ToString().Trim();
                    totalDebeMLStr = dr.GetValue(dr.GetOrdinal("TOT_DEBE_ML")).ToString().Trim();
                    totalHaberMLStr = dr.GetValue(dr.GetOrdinal("TOT_HABER_ML")).ToString().Trim();
                    saldoInicialMEStr = dr.GetValue(dr.GetOrdinal("SALDO_INI_ME")).ToString().Trim();
                    totalDebeMEStr = dr.GetValue(dr.GetOrdinal("TOT_DEBE_ME")).ToString().Trim();
                    totalHaberMEStr = dr.GetValue(dr.GetOrdinal("TOT_HABER_ME")).ToString().Trim();
                    saldoFinalMLStr = dr.GetValue(dr.GetOrdinal("SALDO_FINAL_ML")).ToString().Trim();
                    saldoFinalMEStr = dr.GetValue(dr.GetOrdinal("SALDO_FINAL_ME")).ToString().Trim();
                    num_mov_dentroPeriodoStr = dr.GetValue(dr.GetOrdinal("num_mov_dentro_periodo")).ToString().Trim();
                    nombreAux = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString().Trim();

                    if (fechaDocStr != "")
                        try { fechaDoc = Convert.ToInt32(fechaDocStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            fechaDoc = 0;
                        }
                    else fechaDoc = 0;

                    if (fechaVencStr != "")
                        try { fechaVenc = Convert.ToInt32(fechaVencStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            fechaVenc = 0;
                        }
                    else fechaVenc = 0;

                    if (fecha1CancStr != "")
                        try { fecha1Canc = Convert.ToInt32(fecha1CancStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            fecha1Canc = 0;
                        }
                    else fecha1Canc = 0;

                    if (saldoInicialStr != "")
                        try { saldoInicialML = Convert.ToDecimal(saldoInicialStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            saldoInicialML = 0;
                        }
                    else saldoInicialML = 0;

                    if (totalDebeMLStr != "")
                        try { totalDebeML = Convert.ToDecimal(totalDebeMLStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            totalDebeML = 0;
                        }
                    else totalDebeML = 0;

                    if (totalHaberMLStr != "")
                        try { totalHaberML = Convert.ToDecimal(totalHaberMLStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            totalHaberML = 0;
                        }
                    else totalHaberML = 0;

                    if (saldoInicialMEStr != "")
                        try { saldoInicialME = Convert.ToDecimal(saldoInicialMEStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            saldoInicialME = 0;
                        }
                    else saldoInicialME = 0;

                    if (totalDebeMEStr != "")
                        try { totalDebeME = Convert.ToDecimal(totalDebeMEStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            totalDebeME = 0;
                        }
                    else totalDebeME = 0;

                    if (totalHaberMEStr != "")
                        try { totalHaberME = Convert.ToDecimal(totalHaberMEStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            totalHaberME = 0;
                        }
                    else totalHaberME = 0;

                    if (saldoFinalMLStr != "")
                        try { saldoFinalML = Convert.ToDecimal(saldoFinalMLStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            saldoFinalML = 0;
                        }
                    else saldoFinalML = 0;

                    if (saldoFinalMEStr != "")
                        try { saldoFinalME = Convert.ToDecimal(saldoFinalMEStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            saldoFinalME = 0;
                        }
                    else saldoFinalME = 0;

                    if (num_mov_dentroPeriodoStr != "")
                        try { num_mov_dentroPeriodo = Convert.ToInt32(num_mov_dentroPeriodoStr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            num_mov_dentroPeriodo = 0;
                        }
                    else num_mov_dentroPeriodo = 0;

                    //Chequear si todos los importes son 0 y el contador too es 0 -> No insertar el registro
                    if (!(saldoFinalML == 0 && totalDebeML == 0 && totalHaberML == 0 && saldoInicialME == 0 && totalDebeME == 0 && totalHaberME == 0 &&
                        num_mov_dentroPeriodo == 0))
                    {
                        if (this.Documentos == "3") insertarReg = true;
                        else if (this.Documentos == "1" && (saldoInicialML + totalDebeML + totalHaberML) == 0) insertarReg = true;
                        else if (this.Documentos == "2" && (saldoInicialML + totalDebeML + totalHaberML) != 0) insertarReg = true;

                        if (insertarReg)
                        {
                            this.InsertarFilaGrid(cciadt, cldodt, ndocdt, cauxdt, fechaDoc, fechaVenc, fecha1Canc, saldoInicialML, saldoInicialME,
                                                  totalDebeML, totalHaberML, totalDebeME, totalHaberME, saldoFinalML, saldoFinalME,
                                                  fechaUltPeriodo, nombreAux, num_mov_dentroPeriodo);

                            contReg++;

                            sumSaldoInicialML += saldoInicialML;
                            sumTotalDebeML += totalDebeML;
                            sumTotalHaberML += totalHaberML;
                            sumSaldoFinalML += saldoFinalML;
                            sumSaldoInicialME += saldoInicialME;
                            sumTotalDebeME += totalDebeME;
                            sumTotalHaberME += totalHaberME;
                            sumSaldoFinalME += saldoFinalME;
                        }
                    }
                }

                dr.Close();

                if (contReg > 0)
                {
                    //Insertar la línea de totales
                    if (contReg > 1)
                    {
                        string totalesEtiqueta = "TOTALES"; //Falta traducir
                        this.InsertarFilaGrid("", "", totalesEtiqueta, "", 0, 0, 0, sumSaldoInicialML, sumSaldoInicialME,
                                                  sumTotalDebeML, sumTotalHaberML, sumTotalDebeME, sumTotalHaberME, 
                                                  sumSaldoFinalML, sumSaldoFinalME,
                                                  fechaUltPeriodo, "", 1);

                        //this.radGridViewDoc.Rows[this.radGridViewDoc.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;

                        //this.radGridViewDoc.Rows[0].DefaultCellStyle.Font = new Font(this.tgGridDoc.Font, FontStyle.Bold);

                        //this.tgGridDoc.Rows[this.tgGridDoc.Rows.Count - 1].DefaultCellStyle.Font = new Font(this.tgGridDoc.Font, FontStyle.Bold);
                        /*DataGridViewCellStyle style = new DataGridViewCellStyle();
                        style.Font = new Font(this.tgGridDoc.Font, FontStyle.Bold);
                        this.tgGridDoc.Rows[this.tgGridDoc.Rows.Count - 1].DefaultCellStyle = style;
                        
                        //this.tgGridDoc.Rows[this.tgGridDoc.Rows.Count - 1].DefaultCellStyle.Font = new Font(this.tgGridDoc.DefaultCellStyle.Font, FontStyle.Bold);                     
                        */
                    }

                    /*
                    //Calcular saldos 
                    decimal[] saldoPeriodo = { 0, 0, 0 };
                    
                    //Calcular saldo inicial (moneda local)
                    int sigloanoPeriodoDesdeAnterior = Convert.ToInt32(this.AAPPDesde) - 1;
                    string sapr = sigloanoPeriodoDesdeAnterior.ToString();
                    if (sapr.Length < 5) sapr = sapr.PadLeft(5, '0');
                    saldoPeriodo = utilesCG.ObtenerSaldo(compania, plan, "00000", sapr, this.CtaMayorCodigo, "R", this.TipoAuxCodigo, this.CtaAuxCodigo);

                    this.ucConsAuxCab.SaldoInicialDesc = saldoPeriodo[2].ToString("N2", this.LP.MyCultureInfo);

                    //Calcular saldo final (moneda local)
                    saldoPeriodo = utilesCG.ObtenerSaldo(compania, plan, "00000", this.AAPPHasta, this.CtaMayorCodigo, "R", this.TipoAuxCodigo, cauxdt);

                    this.ucConsAuxCab.SaldoFinalDesc = saldoPeriodo[2].ToString("N2", this.LP.MyCultureInfo);
                    
                    if (debe_haber == "D") totalGlobalDebe += montdt;
                    else totalGlobalHaber += montdt;

                    this.ucConsAuxCab.TotalDebeDesc = sumTotalDebeML.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.TotalHaberDesc = sumTotalHaberML.ToString("N2", this.LP.MyCultureInfo);

                    this.ucConsAuxCab.SaldoInicialMEDesc = sumSaldoInicialME.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.SaldoFinalMEDesc = sumSaldoFinalME.ToString("N2", this.LP.MyCultureInfo);

                    this.ucConsAuxCab.TotalDebeMEDesc = sumTotalDebeME.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.TotalHaberMEDesc = sumTotalHaberME.ToString("N2", this.LP.MyCultureInfo);
                    */

                    //Ocultar columna cuenta auxiliar
                    //if (this.CtaAuxCodigo != "") this.tgGridDoc.Columns["CAUXDT"].Visible = false;

                    //Ocultar columnas moneda extranjera
                    if (this.DatosMonedaExt != "1") this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                    this.dtDoc.DefaultView.RowFilter = "num_mov_dentro_periodo > 0";
                    this.radGridViewDoc.Refresh();

                    this.radGridViewDoc.Visible = true;

                    if (!this.CalcularPrdoMedioPago)
                    {
                        utiles.ButtonEnabled(ref this.radButtonCalcularPrdoMedioPago, false);
                        this.radContextMenuClickDerecho.Items["menuItemCalcularPrdoMedioPago"].Enabled = false;
                    }
                }
                else
                {
                    //No hay documentos
                    utiles.ButtonEnabled(ref this.radButtonMovimientos, false);
                    utiles.ButtonEnabled(ref this.radButtonCalcularPrdoMedioPago, false);
                    utiles.ButtonEnabled(ref this.radButtonExportar, false);

                    this.radContextMenuClickDerecho.Items["menuItemVerMovimientos"].Enabled = false;
                    this.radContextMenuClickDerecho.Items["menuItemCalcularPrdoMedioPago"].Enabled = false;
                    this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                    this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                    this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                    this.lblResult.Text = "No existe o no se han podido recuperar los documentos para el criterio de selección indicado";    //Falta traducir
                    this.lblResult.Visible = true;

                    this.gbFiltro.Visible = false;
                    this.radGridViewDoc.Visible = false;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                utiles.ButtonEnabled(ref this.radButtonMovimientos, false);
                utiles.ButtonEnabled(ref this.radButtonCalcularPrdoMedioPago, false);
                utiles.ButtonEnabled(ref this.radButtonExportar, false);

                this.radContextMenuClickDerecho.Items["menuItemVerMovimientos"].Enabled = false;
                this.radContextMenuClickDerecho.Items["menuItemCalcularPrdoMedioPago"].Enabled = false;
                this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                this.lblResult.Text = "No se han podido recuperar los documentos";    //Falta traducir
                this.lblResult.Visible = true;

                this.gbFiltro.Visible = false;
                this.radGridViewDoc.Visible = false;
            }

            if (this.radGridViewDoc.Visible)
            {
                for (int i = 0; i < this.radGridViewDoc.Columns.Count; i++)
                    this.radGridViewDoc.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                //this.radGridViewComp.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewDoc.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.None;

                //this.radGridViewComp.MasterTemplate.BestFitColumns();
                this.radGridViewDoc.Rows[0].IsCurrent = true;

                this.radGridViewDoc.Rows[this.radGridViewDoc.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                //this.radGridViewDoc.Rows[this.radGridViewDoc.Rows.Count - 1].IsPinned = true;
            }
        }

        /// <summary>
        /// Devuelve la consulta donde se recuperan los documentos
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsulta()
        {
            string query = "";
            try
            {
                //Obtener datos compañía o del grupo de compañías
                //(listado de compañías que forman el grupo, si la solicitud es por grupo, o el plan si la solicitud es por compañía
                this.ObtenerDatosCompania_Grupo();

                string tipoCancelacion = "";
                switch (this.FEVEMC)
                {
                    case "D":
                        tipoCancelacion = "H";
                        break;
                    case "H":
                        tipoCancelacion = "D";
                        break;
                    default:
                        tipoCancelacion = "";
                        break;
                }

                query = "select CCIADT, TIPLDT, CUENDT, CLDODT, NDOCDT, TAUXDT, CAUXDT, NOMBMA, ";
                query += "MIN(FDOCDT) FDOCDT, ";
                query += "MAX(CASE WHEN FEVEDT > 0 THEN FEVEDT ELSE 0 END) FEVEDT, ";

                //query += "MIN(CASE WHEN MONTDT <> 0 AND TMOVDT='" + tipoCancelacion + "' THEN FECODT END) FECODT, ";
                query += "CASE WHEN (MIN(CASE WHEN MONTDT <> 0 AND TMOVDT='" + tipoCancelacion + "' THEN FECODT END) IS NULL) THEN 0 ELSE ";
                query += "MIN(CASE WHEN MONTDT <> 0 AND TMOVDT='" + tipoCancelacion + "' THEN FECODT END) END FECODT, ";
                query += "SUM(CASE WHEN SAPRDT < " + this.AAPPDesde + " THEN MONTDT ELSE 0 END) SALDO_INI_ML, ";
                query += "SUM(CASE WHEN SAPRDT >= " + this.AAPPDesde + " AND SAPRDT <= " + this.AAPPHasta + " AND TMOVDT='D' THEN MONTDT ELSE 0 END) TOT_DEBE_ML, ";
                query += "SUM(CASE WHEN SAPRDT >= " + this.AAPPDesde + " AND SAPRDT <= " + this.AAPPHasta + " AND TMOVDT='H' THEN MONTDT ELSE 0 END) TOT_HABER_ML, ";
                query += "SUM(CASE WHEN SAPRDT < " + this.AAPPDesde + " THEN MOSMAD ELSE 0 END) SALDO_INI_ME, ";
                query += "SUM(CASE WHEN SAPRDT >= " + this.AAPPDesde + " AND SAPRDT <= " + this.AAPPHasta + " AND TMOVDT='D' THEN MOSMAD ELSE 0 END) TOT_DEBE_ME, ";
                query += "SUM(CASE WHEN SAPRDT >= " + this.AAPPDesde + " AND SAPRDT <= " + this.AAPPHasta + " AND TMOVDT='H' THEN MOSMAD ELSE 0 END) TOT_HABER_ME, ";
                query += "SUM(MONTDT) SALDO_FINAL_ML, ";
                query += "SUM(MOSMAD) SALDO_FINAL_ME, ";
                query += "COUNT(CASE WHEN SAPRDT >= " + this.AAPPDesde + " AND SAPRDT <= " + this.AAPPHasta + " THEN NDOCDT END) num_mov_dentro_periodo ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "LEFT JOIN " + GlobalVar.PrefijoTablaCG + "GLM05 ON TAUXDT = TAUXMA and CAUXDT = CAUXMA ";

                //if (this.PosAux != "-1") query += ", " + GlobalVar.PrefijoTablaCG + "GLM03 ";

                //query += "where STATDT='E' and TAUXDT ='" + this.TipoAuxCodigo + "'";
                query += "where STATDT='E' ";
                
                //Compañia o grupos de compañias
                string[] datosCompania;
                if (this.CompaniaCodigo != "")
                {
                    //Por compañía
                    datosCompania = (string[])aEmpresas[0];
                    query += " and CCIADT='" + datosCompania[0] + "' ";
                }
                else
                {
                    //Por grupo de compañías
                    query += " and (";
                    for (int i = 0; i < aEmpresas.Count; i++)
                    {
                        datosCompania = (string[])aEmpresas[i];
                        if (i != 0) query += " or ";
                        query += "CCIADT = '" + datosCompania[0] + "'";
                    }
                    query += ")";
                }

                query += " and TIPLDT = '" + this.PlanCodigo + "'";
                query += " and (CLDODT <> '  ' OR NDOCDT > 0)";

                query += " and SAPRDT <= " + this.AAPPHasta;
                query += " and CUENDT = '" + this.CtaMayorCodigo + "' ";

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

                /*

                //Posición del auxiliar
                if (this.PosAux != "-1")
                {
                    query += "and TIPLMC = TIPLDT and CUENMC = CUENDT ";
                    switch (this.PosAux)
                    {
                        case "1":
                            query += "and TAU1MC ='" + this.TipoAuxCodigo + "' ";
                            break;
                        case "2":
                            query += "and TAU2MC ='" + this.TipoAuxCodigo + "' ";
                            break;
                        case "3":
                            query += "and TAU3MC ='" + this.TipoAuxCodigo + "' ";
                            break;
                    }
                }

                if (this.CtaAuxCodigo != "") query += " and CAUXDT = '" + this.CtaAuxCodigo + "'";*/

                query += " GROUP BY TAUXDT, CAUXDT, CCIADT, TIPLDT, CUENDT, CLDODT, NDOCDT, NOMBMA ";
                query += " ORDER BY TAUXDT, CAUXDT, CCIADT, TIPLDT, CUENDT, CLDODT, NDOCDT";
                
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
            
            this.calendario = "";

            if (this.GrupoCodigo != "")
            {
                //Buscar las empresas del grupo
                aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(this.GrupoCodigo, this.PlanCodigo);

                if (aEmpresas.Count > 0)
                {
                    string[] datosCompania = utilesCG.ObtenerTipoCalendarioCompania(((string[])aEmpresas[0])[0]);
                    //this.PlanCodigo = datosCompaniaAct[1];
                    this.calendario = datosCompania[0];
                }
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
                this.calendario = datosCompaniaAct[0];
            }
        }

        /// <summary>
        /// Inserta una fila en la Grid
        /// </summary>
        /// <param name="cldot"></param>
        /// <param name="ndocdt"></param>
        /// <param name="cauxdt"></param>
        /// <param name="fdocdt"></param>
        /// <param name="fevedt"></param>
        /// <param name="fecodt"></param>
        /// <param name="saldoInicial"></param>
        /// <param name="totalDebeME"></param>
        /// <param name="totalHaberML"></param>
        /// <param name="totalDebeME"></param>
        /// <param name="totalHaberME"></param>
        /// <param name="fechaUltPeriodo"></param>
        private void InsertarFilaGrid(string cciadt, string cldot, string ndocdt, string cauxdt, int fdocdt, int fevedt, int fecodt,
                                      decimal saldoInicialML, decimal saldoInicialME,
                                      decimal totalDebeML, decimal totalHaberML, decimal totalDebeME, decimal totalHaberME,
                                      decimal saldoFinalML, decimal saldoFinalME, DateTime fechaUltPeriodo, string nombreAux,
                                      int num_mov_dentro_periodo)
        {

            try
            {
                DataRow rowDoc = this.dtDoc.NewRow();

                rowDoc["CCIADT"] = cciadt;
                rowDoc["CLDODT"] = cldot;
                rowDoc["NDOCDT"] = ndocdt;

                if (cciadt == "" && cldot == "" && ndocdt == "TOTALES")     //Falta traducir
                {
                    rowDoc["DOCUMENTO"] = ndocdt;
                }
                else rowDoc["DOCUMENTO"] = cldot.PadLeft(2, '0') + "-" + ndocdt.PadLeft(7, '0');

                rowDoc["CAUXDT"] = cauxdt;
                rowDoc["NOMBMA"] = nombreAux;

                DateTime fechaDoc = new DateTime();

                if (fdocdt != 0)
                {
                    fechaDoc = utiles.FormatoCGToFecha(fdocdt.ToString());
                    rowDoc["FDOCDT"] = fechaDoc.ToShortDateString();
                }
                else rowDoc["FDOCDT"] = "";

                if (fevedt != 0) rowDoc["FEVEDT"] = utiles.FormatoCGToFecha(fevedt.ToString()).ToShortDateString();
                else rowDoc["FEVEDT"] = "";

                DateTime fecha1raCanc = new DateTime();

                if (fecodt != 0)
                {
                    fecha1raCanc = utiles.FormatoCGToFecha(fecodt.ToString());
                    rowDoc["FECODT"] = fecha1raCanc.ToShortDateString();
                }
                else rowDoc["FECODT"] = "";

                rowDoc["SALDOANTML"] = saldoInicialML.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["DEBEML"] = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["HABERML"] = totalHaberML.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["SALDOML"] = saldoFinalML.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["SALDOANTME"] = saldoInicialME.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["DEBEME"] = totalDebeME.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["HABERME"] = totalHaberME.ToString("N2", this.LP.MyCultureInfo);
                rowDoc["SALDOME"] = saldoFinalME.ToString("N2", this.LP.MyCultureInfo);

                //Calcular días de pago (fecha 1ra cancelacion - fecha doc)
                if (rowDoc["FDOCDT"].ToString() != "" && rowDoc["FECODT"].ToString() != "")
                {
                    TimeSpan ts = fecha1raCanc - fechaDoc;
                    //Diferencia en dias.
                    int differenceInDays = ts.Days;

                    if (differenceInDays == 0) rowDoc["DIASPAGO"] = "";
                    else rowDoc["DIASPAGO"] = differenceInDays.ToString();
                }
                else rowDoc["DIASPAGO"] = "";

                //Calcular días pendientes de pago (fecha último período hasta del formulario - fecha documento)
                if (saldoFinalML != 0 && rowDoc["FDOCDT"].ToString() != "")
                {
                    TimeSpan ts = fechaUltPeriodo - fechaDoc;
                    //Diferencia en dias.
                    int diffInDays = ts.Days;

                    if (diffInDays == 0) rowDoc["DIASPDTEPAGO"] = "";
                    else rowDoc["DIASPDTEPAGO"] = diffInDays.ToString();

                }
                else rowDoc["DIASPDTEPAGO"] = "";

                rowDoc["num_mov_dentro_periodo"] = num_mov_dentro_periodo;

                this.dtDoc.Rows.Add(rowDoc);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Ver los movimientos del documento
        /// </summary>
        private void VerMovimientos()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewDoc.SelectedRows.Count == 0)
                {
                    if (this.radGridViewDoc.Rows.Count > 1)
                    {
                        RadMessageBox.Show("Debe seleccionar un documento", "Error");  //Falta traducir
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else this.radGridViewDoc.Rows[0].IsSelected = true;
                }

                if (this.radGridViewDoc.SelectedRows.Count > 1)
                {
                    RadMessageBox.Show("Debe seleccionar solo un documento", "Error");  //Falta traducir
                    Cursor.Current = Cursors.Default;
                    return;
                }

                int indice = this.radGridViewDoc.CurrentRow.Index;

                bool lineaTotales = false;
                if (this.radGridViewDoc.Rows.Count > 1 && this.radGridViewDoc.Rows.Count - 1 == indice) lineaTotales = true;

                if (!lineaTotales)
                {
                    string clase = this.radGridViewDoc.Rows[indice].Cells["CLDODT"].Value.ToString();
                    string doc = this.radGridViewDoc.Rows[indice].Cells["NDOCDT"].Value.ToString();
                    string saldoInicial = this.radGridViewDoc.Rows[indice].Cells["SALDOANTML"].Value.ToString();

                    if (this.GrupoCodigo != "")
                    {
                        this.CompaniaCodigo = this.radGridViewDoc.Rows[indice].Cells["CCIADT"].Value.ToString().Trim();

                        string[] datosCompania;
                        for (int i = 0; i < aEmpresas.Count; i++)
                        {
                            datosCompania = (string[])aEmpresas[i];
                            if (datosCompania[0] == this.CompaniaCodigo)
                            {
                                this.CompaniaDesc = this.CompaniaCodigo + " " + this.separadorDesc + " " + datosCompania[1];
                                break;
                            }
                        }
                    }

                    frmConsAuxViewMov frmViewConsMov = new frmConsAuxViewMov
                    {
                        TipoAuxCodigo = this.TipoAuxCodigo,
                        TipoAuxDesc = this.TipoAuxDesc,
                        CompaniaCodigo = this.CompaniaCodigo,
                        CompaniaDesc = this.CompaniaDesc,
                        GrupoCodigo = "",
                        GrupoDesc = "",
                        PlanCodigo = this.PlanCodigo,
                        PlanDesc = this.PlanDesc,
                        AAPPDesde = this.AAPPDesde,
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
                        Clase = clase,
                        NoDocumento = doc,
                        SaldoInicialDocumento = saldoInicial
                    };
                    frmViewConsMov.Show(this);
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("errSelCtaDoc", "Debe seleccionar un documento"), this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Recalcula los Totales cuando se cambio el filtro por Periodo o no de la Grid
        /// </summary>
        private void RecalcularTotales()
        {
            try
            {
                string saldoInicialMLStr = "";
                string debeMLStr = "";
                string haberMLStr = "";
                string saldoFinalMLStr = "";

                string saldoInicialMEStr = "";
                string debeMEStr = "";
                string haberMEStr = "";
                string saldoFinalMEStr = "";

                decimal saldoInicialML = 0;
                decimal debeML = 0;
                decimal haberML = 0;
                decimal saldoFinalML = 0;

                decimal saldoInicialME = 0;
                decimal debeME = 0;
                decimal haberME = 0;
                decimal saldoFinalME = 0;

                decimal totalSaldoInicialML = 0;
                decimal totalDebeML = 0;
                decimal totalHaberML = 0;
                decimal totalSaldoFinalML = 0;

                decimal totalSaldoInicialME = 0;
                decimal totalDebeME = 0;
                decimal totalHaberME = 0;
                decimal totalSaldoFinalME = 0;

                for (int i = 0; i < this.radGridViewDoc.Rows.Count; i++)
                {
                    saldoInicialMLStr = this.radGridViewDoc.Rows[i].Cells["SALDOANTML"].Value.ToString().Trim();
                    if (saldoInicialMLStr != "")
                        try
                        {
                            saldoInicialML = Convert.ToDecimal(saldoInicialMLStr);
                        }
                        catch{}
                    else saldoInicialML = 0;

                    debeMLStr = this.radGridViewDoc.Rows[i].Cells["DEBEML"].Value.ToString().Trim();
                    if (debeMLStr != "")
                        try
                        {
                            debeML = Convert.ToDecimal(debeMLStr);
                        }
                        catch{}
                    else debeML = 0;

                    haberMLStr = this.radGridViewDoc.Rows[i].Cells["HABERML"].Value.ToString().Trim();
                    if (haberMLStr != "")
                        try
                        {
                            haberML = Convert.ToDecimal(haberMLStr);
                        }
                        catch{}
                    else haberML = 0;

                    saldoFinalMLStr = this.radGridViewDoc.Rows[i].Cells["SALDOML"].Value.ToString().Trim();
                    if (saldoFinalMLStr != "")
                        try
                        {
                            saldoFinalML = Convert.ToDecimal(saldoFinalMLStr);
                        }
                        catch{}
                    else saldoFinalML = 0;

                    //Moneda Extranjera
                    if (this.DatosMonedaExt == "1")
                    {
                        saldoInicialMEStr = this.radGridViewDoc.Rows[i].Cells["SALDOANTME"].Value.ToString().Trim();
                        if (saldoInicialMEStr != "")
                            try
                            {
                                saldoInicialME = Convert.ToDecimal(saldoInicialMEStr);
                            }
                            catch{}
                        else saldoInicialME = 0;

                        debeMEStr = this.radGridViewDoc.Rows[i].Cells["DEBEME"].Value.ToString().Trim();
                        if (debeMEStr != "")
                            try
                            {
                                debeME = Convert.ToDecimal(debeMEStr);
                            }
                            catch{}
                        else debeME = 0;

                        haberMEStr = this.radGridViewDoc.Rows[i].Cells["HABERME"].Value.ToString().Trim();
                        if (haberMEStr != "")
                            try
                            {
                                haberME = Convert.ToDecimal(haberMEStr);
                            }
                            catch{}
                        else haberME = 0;

                        saldoFinalMEStr = this.radGridViewDoc.Rows[i].Cells["SALDOME"].Value.ToString().Trim();
                        if (saldoFinalMEStr != "")
                            try
                            {
                                saldoFinalME = Convert.ToDecimal(saldoFinalMEStr);
                            }
                            catch{}
                        else saldoFinalME = 0;
                    }

                    //Linea de totales
                    if (i == this.radGridViewDoc.Rows.Count - 1)
                    {
                        if (i == 0)
                        {
                            totalSaldoInicialML = saldoInicialML;
                            totalDebeML = debeML;
                            totalHaberML = haberML;
                            totalSaldoFinalML = saldoFinalML;
                        }

                        //Actualizar Linea Totales
                        this.radGridViewDoc.Rows[i].Cells["SALDOANTML"].Value = totalSaldoInicialML.ToString("N2", this.LP.MyCultureInfo);
                        this.radGridViewDoc.Rows[i].Cells["DEBEML"].Value = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                        this.radGridViewDoc.Rows[i].Cells["HABERML"].Value = totalHaberML.ToString("N2", this.LP.MyCultureInfo);
                        this.radGridViewDoc.Rows[i].Cells["SALDOML"].Value = totalSaldoFinalML.ToString("N2", this.LP.MyCultureInfo);

                        //Moneda Extranjera
                        if (this.DatosMonedaExt == "1")
                        {
                            if (i == 0)
                            {
                                totalSaldoInicialME = saldoInicialME;
                                totalDebeME = debeME;
                                totalHaberME = haberME;
                                totalSaldoFinalME = saldoFinalME;
                            }

                            this.radGridViewDoc.Rows[i].Cells["SALDOANTME"].Value = totalSaldoInicialME.ToString("N2", this.LP.MyCultureInfo);
                            this.radGridViewDoc.Rows[i].Cells["DEBEME"].Value = totalDebeME.ToString("N2", this.LP.MyCultureInfo);
                            this.radGridViewDoc.Rows[i].Cells["HABERME"].Value = totalHaberME.ToString("N2", this.LP.MyCultureInfo);
                            this.radGridViewDoc.Rows[i].Cells["SALDOME"].Value = totalSaldoFinalME.ToString("N2", this.LP.MyCultureInfo);
                        }
                    }
                    else
                    {
                        totalSaldoInicialML += saldoInicialML;
                        totalDebeML += debeML;
                        totalHaberML += haberML;
                        totalSaldoFinalML += saldoFinalML;

                        totalSaldoInicialME += saldoInicialME;
                        totalDebeME += debeME;
                        totalHaberME += haberME;
                        totalSaldoFinalME += saldoFinalME;
                    }
                }

                this.ucConsAuxCab.SaldoInicialDesc = totalSaldoInicialML.ToString("N2", this.LP.MyCultureInfo);
                this.ucConsAuxCab.SaldoFinalDesc = totalSaldoFinalML.ToString("N2", this.LP.MyCultureInfo);
                this.ucConsAuxCab.TotalDebeDesc = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                this.ucConsAuxCab.TotalHaberDesc = totalHaberML.ToString("N2", this.LP.MyCultureInfo);

                //Moneda Extranjera
                if (this.DatosMonedaExt == "1")
                {
                    this.ucConsAuxCab.SaldoInicialMEDesc = totalSaldoInicialML.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.SaldoFinalMEDesc = totalSaldoFinalML.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.TotalDebeMEDesc = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                    this.ucConsAuxCab.TotalHaberMEDesc = totalHaberML.ToString("N2", this.LP.MyCultureInfo);
                }

                if (this.radGridViewDoc.Visible)
                {
                    this.radGridViewDoc.Rows[this.radGridViewDoc.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                    //this.radGridViewDoc.Rows[this.radGridViewDoc.Rows.Count - 1].IsPinned = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemVerMovimientos = new RadMenuItem(this.radButtonMovimientos.Text)
            {
                Name = "menuItemVerMovimientos"
            };
            menuItemVerMovimientos.Click += new EventHandler(RadButtonMovimientos_Click);
            RadMenuItem menuItemCalcularPrdoMedioPago = new RadMenuItem(this.radButtonCalcularPrdoMedioPago.Text)
            {
                Name = "menuItemCalcularPrdoMedioPago"
            };
            menuItemCalcularPrdoMedioPago.Click += new EventHandler(RadButtonCalcularPrdoMedioPago_Click);
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

            this.radContextMenuClickDerecho.Items.Add(menuItemVerMovimientos);
            this.radContextMenuClickDerecho.Items.Add(menuItemCalcularPrdoMedioPago);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportar);
            this.radContextMenuClickDerecho.Items.Add(menuItemSalir);
        }
        #endregion
    }
}