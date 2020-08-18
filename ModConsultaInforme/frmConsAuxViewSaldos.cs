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
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.Export;
using System.IO;
using System.Diagnostics;

namespace ModConsultaInforme
{
    public partial class frmConsAuxViewSaldos : frmPlantilla, IReLocalizable
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
        public bool PorCuentasAux { get; set; }

        ArrayList aEmpresas = null;

        Dictionary<string, string> displayNamesSaldos;
        DataTable dtSaldos;

        public frmConsAuxViewSaldos()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radGridViewSaldos.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxViewSaldos_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar Saldos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Muestra la cabecera de la consulta (criterio de selección / saldos / totales)
            this.CargarValoresCabecera();

            //Cargar MEnu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            //Actualizar acciones del formulario en dependencia de si solicitaron documentos o no
            if (this.MostrarCuentas == "1")
            {
                utiles.ButtonEnabled(ref this.radButtonDocCancel, false);
                utiles.ButtonEnabled(ref this.radButtonDocNoCancel, false);
                utiles.ButtonEnabled(ref this.radButtonDocTodos, false);

                //this.radContextMenuClickDerecho.Items["menuItemDocCancelados"].Visibility = ElementVisibility.Hidden;
                this.radContextMenuClickDerecho.Items["menuItemDocCancelados"].Enabled = false;
                this.radContextMenuClickDerecho.Items["menuItemDocNoCancelados"].Enabled = false;
                this.radContextMenuClickDerecho.Items["menuItemDocTodos"].Enabled = false;
            }
            else
            {
                utiles.ButtonEnabled(ref this.radButtonDocCancel, true);
                utiles.ButtonEnabled(ref this.radButtonDocNoCancel, true);
                utiles.ButtonEnabled(ref this.radButtonDocTodos, true);

                this.radContextMenuClickDerecho.Items["menuItemDocCancelados"].Enabled = true;
                this.radContextMenuClickDerecho.Items["menuItemDocCancelados"].Enabled = true;
                this.radContextMenuClickDerecho.Items["menuItemDocTodos"].Enabled = true;
            }

            //Crear el DataTable que se asocia a la Grid
            this.BuildDataTableSaldos();

            //Cargar los documentos
            this.CargarInfoSaldos();
        }
        
        private void RadButtonDocCancel_Click(object sender, EventArgs e)
        {
            //Ver los documentos cancelados
            this.LlamarVerDocumentos(1);
        }

        private void RadButtonDocNoCancel_Click(object sender, EventArgs e)
        {
            //Ver los documentos no cancelados
            this.LlamarVerDocumentos(2);
        }

        private void RadButtonDocTodos_Click(object sender, EventArgs e)
        {
            //Ver todos los documentos
            this.LlamarVerDocumentos(3);
        }

        private void RadButtonMovimientos_Click(object sender, EventArgs e)
        {
            this.LlamarVerMovimientos();
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

        private void RadGridViewSaldos_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewSaldos.Columns.Contains("CUENDT")) this.radGridViewSaldos.Columns["CUENDT"].IsVisible = false;
            if (this.radGridViewSaldos.Columns.Contains("SCONMC")) this.radGridViewSaldos.Columns["SCONMC"].IsVisible = false;
            if (this.radGridViewSaldos.Columns.Contains("FEVEMC")) this.radGridViewSaldos.Columns["FEVEMC"].IsVisible = false;

            if (this.DatosMonedaExt != "1")
            {
                if (this.radGridViewSaldos.Columns.Contains("saldoiniME")) this.radGridViewSaldos.Columns["saldoiniME"].IsVisible = false;
                if (this.radGridViewSaldos.Columns.Contains("MonedaExt")) this.radGridViewSaldos.Columns["MonedaExt"].IsVisible = false;
                if (this.radGridViewSaldos.Columns.Contains("saldofinME")) this.radGridViewSaldos.Columns["saldofinME"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewSaldos.Columns.Contains("saldoiniME")) this.radGridViewSaldos.Columns["saldoiniME"].TextAlignment = ContentAlignment.MiddleRight;
                if (this.radGridViewSaldos.Columns.Contains("MonedaExt")) this.radGridViewSaldos.Columns["MonedaExt"].TextAlignment = ContentAlignment.MiddleRight;
                if (this.radGridViewSaldos.Columns.Contains("saldofinME")) this.radGridViewSaldos.Columns["saldofinME"].TextAlignment = ContentAlignment.MiddleRight;
            }

            if (this.radGridViewSaldos.Columns.Contains("saldoiniML")) this.radGridViewSaldos.Columns["saldoiniML"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewSaldos.Columns.Contains("MonedaLocal")) this.radGridViewSaldos.Columns["MonedaLocal"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewSaldos.Columns.Contains("saldofinML")) this.radGridViewSaldos.Columns["saldofinML"].TextAlignment = ContentAlignment.MiddleRight;

            if (this.CompaniaCodigo != "" && this.radGridViewSaldos.Columns.Contains("CCIADT")) this.radGridViewSaldos.Columns["CCIADT"].IsVisible = false;
        }

        private void RadGridViewSaldos_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadGridViewSaldos_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewSaldos, ref this.selectAll);
        }

        private void RadButtonDocCancel_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDocCancel);
        }

        private void RadButtonDocCancel_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDocCancel);
        }

        private void RadButtonDocNoCancel_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDocNoCancel);
        }

        private void RadButtonDocNoCancel_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDocNoCancel);
        }

        private void RadButtonDocTodos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDocTodos);
        }

        private void RadButtonDocTodos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDocTodos);
        }

        private void RadButtonMovimientos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonMovimientos);
        }

        private void RadButtonMovimientos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonMovimientos);
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

        private void FrmConsAuxViewSaldos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void FrmConsAuxViewSaldos_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Auxiliar Saldos");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemConsultaSaldosCtasMayor", "Saldos de las Cuentas de Mayor");     //Falta traducir

            this.radButtonDocCancel.Text = this.LP.GetText("lblVerDocCanc", "Ver Documentos Cancelados");   //Falta traducir
            this.radButtonDocNoCancel.Text = this.LP.GetText("lblVerDocNoCanc", "Ver Documentos No Cancelados");   //Falta traducir
            this.radButtonDocTodos.Text = this.LP.GetText("lblVerDocTodos", "Ver Todos los Documentos");   //Falta traducir
            this.radButtonMovimientos.Text = this.LP.GetText("lblVerMov", "Ver los Movimientos");   //Falta traducir
            this.radButtonExportar.Text = this.LP.GetText("lblExportar", "Exportar");   //Falta traducir
            this.radButtonSalir.Text = this.LP.GetText("lblSalir", "Salir");   //Falta traducir

            this.lblResult.Text = this.LP.GetText("lblConsultaSaldosCtaMayorResult", "No existen saldos para el criterio de selección indicado");     //Falta traducir
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

                this.ucConsAuxCab.ActualizarValores();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el Grid que almacenará los saldos
        /// </summary>
        private void BuildDataTableSaldos()
        {
            try
            {
                this.dtSaldos = new DataTable
                {
                    TableName = "Tabla"
                };

                this.dtSaldos.Columns.Add("CCIADT", typeof(string));
                if (this.PorCuentasAux) this.dtSaldos.Columns.Add("CAUXDT", typeof(string));
                else this.dtSaldos.Columns.Add("CEDTMC", typeof(string));
                if (this.PorCuentasAux) this.dtSaldos.Columns.Add("NOMBMA", typeof(string));
                else this.dtSaldos.Columns.Add("NOLAAD", typeof(string));
                this.dtSaldos.Columns.Add("saldoiniML", typeof(string));
                this.dtSaldos.Columns.Add("MonedaLocal", typeof(string));
                this.dtSaldos.Columns.Add("saldofinML", typeof(string));
                this.dtSaldos.Columns.Add("saldoiniME", typeof(string));
                this.dtSaldos.Columns.Add("MonedaExt", typeof(string));
                this.dtSaldos.Columns.Add("saldofinME", typeof(string));
                this.dtSaldos.Columns.Add("CUENDT", typeof(string));
                this.dtSaldos.Columns.Add("SCONMC", typeof(string));
                this.dtSaldos.Columns.Add("FEVEMC", typeof(string));

                this.radGridViewSaldos.DataSource = this.dtSaldos;
                //Escribe el encabezado de la Grid de Saldos
                this.BuildDisplayNamesSaldos();
                this.RadGridViewSaldosHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Encabezados para la Grid de Saldos
        /// </summary>
        private void BuildDisplayNamesSaldos()
        {
            try
            {
                this.displayNamesSaldos = new Dictionary<string, string>
                {
                    { "CCIADT", this.LP.GetText("HeaderCompania", "Compañía") }
                };
                if (this.PorCuentasAux) this.displayNamesSaldos.Add("CAUXDT", this.LP.GetText("HeaderCtaAux", "Cuenta Auxiliar"));
                else this.displayNamesSaldos.Add("CEDTMC", this.LP.GetText("HeaderCtaMayor", "Cuenta Mayor"));
                if (this.PorCuentasAux) this.displayNamesSaldos.Add("NOMBMA", this.LP.GetText("HeaderNombre", "Nombre"));
                else this.displayNamesSaldos.Add("NOLAAD", this.LP.GetText("HeaderNombre", "Nombre"));
                this.displayNamesSaldos.Add("saldoiniML", this.LP.GetText("HeaderSaldoIniML", "Saldo inicial ML"));
                this.displayNamesSaldos.Add("MonedaLocal", this.LP.GetText("HeaderMonedaLocal", "Moneda Local"));
                this.displayNamesSaldos.Add("saldofinML", this.LP.GetText("HeaderSaldoFinML", "Saldo final ML"));
                this.displayNamesSaldos.Add("saldoiniME", this.LP.GetText("HeaderSaldoIniME", "Saldo inicial ME"));
                this.displayNamesSaldos.Add("MonedaExt", this.LP.GetText("HeaderMonedaExt", "Moneda Extranjera"));
                this.displayNamesSaldos.Add("saldofinME", this.LP.GetText("HeaderSaldoFinME", "Saldo final ME"));
                this.displayNamesSaldos.Add("CUENDT", "CUENDT");
                this.displayNamesSaldos.Add("SCONMC", "SCONMC");
                this.displayNamesSaldos.Add("FEVEMC", "FEVEMC");
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Saldos
        /// </summary>
        private void RadGridViewSaldosHeader()
        {
            try
            {
                if (this.radGridViewSaldos.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesSaldos)
                    {
                        if (this.radGridViewSaldos.Columns.Contains(item.Key)) this.radGridViewSaldos.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Carga los saldos en la Grid
        /// </summary>
        private void CargarInfoSaldos()
        {
            IDataReader dr = null;
            try
            {
                string query = this.ObtenerConsulta();

                int contReg = 0;
                DataRow rowSaldos;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                decimal importe = 0;

                string saldoIni = "";
                string saldoFin = "";
                string monedaLocal = "";
                string monedaExt = "";

                decimal sumSaldoInicialML = 0;
                decimal sumMonedaLocal = 0;
                decimal sumSaldoFinalML = 0;
                decimal sumSaldoInicialME = 0;
                decimal sumMonedaExt = 0;
                decimal sumSaldoFinalME = 0;

                while (dr.Read())
                {
                    rowSaldos = this.dtSaldos.NewRow();
                    rowSaldos["CCIADT"] = dr.GetValue(dr.GetOrdinal("CCIADT")).ToString();

                    if (PorCuentasAux) rowSaldos["CAUXDT"] = dr.GetValue(dr.GetOrdinal("CAUXDT")).ToString();
                    else rowSaldos["CEDTMC"] = dr.GetValue(dr.GetOrdinal("CEDTMC")).ToString();

                    if (PorCuentasAux) rowSaldos["NOMBMA"] = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString();
                    else rowSaldos["NOLAAD"] = dr.GetValue(dr.GetOrdinal("NOLAAD")).ToString();

                    importe = 0;
                    saldoIni = dr.GetValue(dr.GetOrdinal("saldoiniML")).ToString().Trim();
                    if (saldoIni != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(saldoIni);

                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumSaldoInicialML += importe;
                    rowSaldos["saldoiniML"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    monedaLocal = dr.GetValue(dr.GetOrdinal("MonedaLocal")).ToString().Trim();
                    if (monedaLocal != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(monedaLocal);

                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumMonedaLocal += importe;
                    rowSaldos["MonedaLocal"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    saldoFin = dr.GetValue(dr.GetOrdinal("saldofinML")).ToString().Trim();
                    if (saldoFin != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(saldoFin);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumSaldoFinalML += importe;
                    rowSaldos["saldofinML"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    saldoIni = dr.GetValue(dr.GetOrdinal("saldoiniME")).ToString().Trim();
                    if (saldoIni != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(saldoIni);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumSaldoInicialME += importe;
                    rowSaldos["saldoiniME"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    monedaExt = dr.GetValue(dr.GetOrdinal("MonedaExt")).ToString().Trim();
                    if (monedaExt != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(monedaLocal);

                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumMonedaExt += importe;
                    rowSaldos["MonedaExt"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    importe = 0;
                    saldoFin = dr.GetValue(dr.GetOrdinal("saldofinME")).ToString().Trim();
                    if (saldoFin != "")
                    {
                        try
                        {
                            importe = Convert.ToDecimal(saldoFin);

                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    sumSaldoFinalME += importe;
                    rowSaldos["saldofinME"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    rowSaldos["CUENDT"] = dr.GetValue(dr.GetOrdinal("CUENDT")).ToString();
                    rowSaldos["SCONMC"] = dr.GetValue(dr.GetOrdinal("SCONMC")).ToString();
                    rowSaldos["FEVEMC"] = dr.GetValue(dr.GetOrdinal("FEVEMC")).ToString();

                    this.dtSaldos.Rows.Add(rowSaldos);

                    contReg++;
                }

                if (contReg > 0)
                {
                    if (contReg > 1)
                    {
                        //Insertar la línea de totales
                        rowSaldos = this.dtSaldos.NewRow();
                        if (PorCuentasAux) rowSaldos["NOMBMA"] = "TOTALES";      //Falta traducir
                        else rowSaldos["NOLAAD"] = "TOTALES";      //Falta traducir
                        rowSaldos["saldoiniML"] = sumSaldoInicialML.ToString("N2", this.LP.MyCultureInfo);
                        rowSaldos["MonedaLocal"] = sumMonedaLocal.ToString("N2", this.LP.MyCultureInfo);
                        rowSaldos["saldofinML"] = sumSaldoFinalML.ToString("N2", this.LP.MyCultureInfo);
                        rowSaldos["saldoiniME"] = sumSaldoInicialME.ToString("N2", this.LP.MyCultureInfo);
                        rowSaldos["MonedaExt"] = sumMonedaExt.ToString("N2", this.LP.MyCultureInfo);
                        rowSaldos["saldofinME"] = sumSaldoFinalME.ToString("N2", this.LP.MyCultureInfo);
                        this.dtSaldos.Rows.Add(rowSaldos);

                        this.radGridViewSaldos.Rows[this.radGridViewSaldos.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                        /*
                        this.tgGridSaldos.RowBold(0);
                        this.tgGridSaldos.RowBold(this.tgGridSaldos.Rows.Count - 1);
                        this.tgGridSaldos.Refresh();
                        */
                    }

                    if (this.DatosMonedaExt != "1") this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;
                    else
                    {
                        this.ucConsAuxCab.SaldoInicialMEDesc = sumSaldoInicialME.ToString("N2", this.LP.MyCultureInfo);
                        this.ucConsAuxCab.SaldoFinalMEDesc = sumSaldoFinalME.ToString("N2", this.LP.MyCultureInfo);
                        this.ucConsAuxCab.TotalDebeMEDesc = sumMonedaExt.ToString("N2", this.LP.MyCultureInfo);
                        this.ucConsAuxCab.CambiarLiteralTotalDebeME("Total ME");    //Falta traducir
                    }

                    //Calcular los saldos
                    try
                    {
                        string compania = "";
                        string cuenta = "";

                        decimal saldoInicialActual = 0;
                        decimal saldoFinalActual = 0;
                        decimal saldoInicialTotal = 0;
                        decimal saldoFinalTotal = 0;

                        decimal[] saldoPeriodo = { 0, 0, 0 };

                        int sigloanoPeriodoDesdeAnterior = Convert.ToInt32(this.AAPPDesde) - 1;
                        string sapr = sigloanoPeriodoDesdeAnterior.ToString();
                        if (sapr.Length < 5) sapr = sapr.PadLeft(5, '0');

                        //Para cada una de las cuentas calcularlo
                        for (int i = 0; i < this.radGridViewSaldos.Rows.Count; i++)
                        {
                            cuenta = this.radGridViewSaldos.Rows[i].Cells["CUENDT"].Value.ToString();
                            compania = this.radGridViewSaldos.Rows[i].Cells["CCIADT"].Value.ToString();

                            //Calcular saldo inicial (moneda local)
                            saldoPeriodo = utilesCG.ObtenerSaldo(compania, this.PlanCodigo, "00000", sapr, cuenta, "R", this.TipoAuxCodigo, this.CtaAuxCodigo);

                            saldoInicialActual = Convert.ToDecimal(saldoPeriodo[2].ToString());
                            saldoInicialTotal += saldoInicialActual;

                            //Calcular saldo final (moneda local)
                            saldoPeriodo = utilesCG.ObtenerSaldo(compania, this.PlanCodigo, "00000", this.AAPPHasta, cuenta, "R", this.TipoAuxCodigo, this.CtaAuxCodigo);

                            saldoFinalActual = Convert.ToDecimal(saldoPeriodo[2].ToString());
                            saldoFinalTotal += saldoFinalActual;
                        }

                        this.ucConsAuxCab.SaldoInicialDesc = saldoInicialTotal.ToString("N2", this.LP.MyCultureInfo);
                        this.ucConsAuxCab.SaldoFinalDesc = saldoFinalTotal.ToString("N2", this.LP.MyCultureInfo);
                        this.ucConsAuxCab.TotalDebeDesc = sumMonedaLocal.ToString("N2", this.LP.MyCultureInfo);

                        this.ucConsAuxCab.CambiarLiteralTotalDebe("Total ML");    //Falta traducir
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    for (int i = 0; i < this.radGridViewSaldos.Columns.Count; i++)
                        this.radGridViewSaldos.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    this.radGridViewSaldos.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewSaldos.MasterTemplate.BestFitColumns();
                    this.radGridViewSaldos.Rows[0].IsCurrent = true;

                    this.radGridViewSaldos.Visible = true;
                }
                else
                {
                    //No hay saldos
                    utiles.ButtonEnabled(ref this.radButtonDocCancel, false);
                    utiles.ButtonEnabled(ref this.radButtonDocNoCancel, false);
                    utiles.ButtonEnabled(ref this.radButtonDocTodos, false);
                    utiles.ButtonEnabled(ref this.radButtonMovimientos, false);
                    utiles.ButtonEnabled(ref this.radButtonExportar, false);

                    this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                    this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                    this.lblResult.Text = "No existen saldos para las cuentas de mayor para el criterio de selección indicado";    //Falta traducir
                    this.lblResult.Visible = true;

                    this.radGridViewSaldos.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                utiles.ButtonEnabled(ref this.radButtonDocCancel, false);
                utiles.ButtonEnabled(ref this.radButtonDocNoCancel, false);
                utiles.ButtonEnabled(ref this.radButtonDocTodos, false);
                utiles.ButtonEnabled(ref this.radButtonMovimientos, false);
                utiles.ButtonEnabled(ref this.radButtonExportar, false);

                this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                this.lblResult.Text = "No se han podido recuperar los saldos para las cuentas de mayor para el criterio de selección indicado";    //Falta traducir
                this.lblResult.Visible = true;
                this.radGridViewSaldos.Visible = false;
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

                if (this.PorCuentasAux) query = "select CCIADT, CAUXDT, NOMBMA, ";
                else query = "select CCIADT, CEDTMC, NOLAAD, ";

                query += "sum(CASE WHEN  SAPRDT < " + this.AAPPDesde + " THEN MONTDT END) saldoiniML, ";
                query += "sum(CASE WHEN  SAPRDT >= " + this.AAPPDesde + " and SAPRDT <= " + this.AAPPHasta + " THEN MONTDT END) MonedaLocal, ";
                query += "sum(MONTDT) saldofinML, ";
                query += "sum(CASE WHEN  SAPRDT < " + this.AAPPDesde + " THEN MOSMAD END) saldoiniME, ";
                query += "sum(CASE WHEN  SAPRDT >= " + this.AAPPDesde + " and SAPRDT <= " + this.AAPPHasta + " THEN MOSMAD END) MonedaExt, ";
                query += "sum(MOSMAD) saldofinME, ";
                query += "CUENDT, SCONMC, FEVEMC ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLB01 ";

                if (this.PorCuentasAux) query += "left join " + GlobalVar.PrefijoTablaCG + "GLM05 on TAUXDT = TAUXMA and CAUXDT = CAUXMA ";

                query += ", " + GlobalVar.PrefijoTablaCG + "GLM03 ";

                query += "where ";
                query += "STATDT= 'E' and TIPLDT = TIPLMC and CUENDT = CUENMC ";

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
                    query += ") ";
                }

                //query += "CCIADT ='" + this.CompaniaCodigo + "' and ";
                query += "and TIPLDT ='" + this.PlanCodigo + "' ";
                //query += "SAPRDT >= " + this.AAPPDesde + " and SAPRDT <= " + this.AAPPHasta + " and ";

                query += "and SAPRDT <= " + this.AAPPHasta;

                if (this.CtaMayorCodigo != "") query += " and CUENDT like '" + this.CtaMayorCodigo + "%'";

                //if (this.CtaAuxCodigo != "") query += " and CAUXDT = '" + this.CtaAuxCodigo + "'";

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

                /*if (this.Documentos == "-1") query += "(CLDODT = '  ' and NDOCDT = 0) ";     //Sin documentos
                else query += "(CLDODT <> '  ' or NDOCDT > 0) ";                            //Con documentos*/

                if (this.MostrarCuentas == "1") query += " and (CLDODT = '  ' and NDOCDT = 0)";     //Sin documentos
                else if (this.MostrarCuentas == "2") query += " and (CLDODT <> '  ' or NDOCDT > 0)";     //Con documentos       

                if (PorCuentasAux)
                {
                    query += " Group by CCIADT, CAUXDT, NOMBMA, CUENDT, SCONMC, FEVEMC ";
                    query += " Order by CAUXDT, CCIADT";
                }
                else
                {
                    query += " Group by CCIADT, CEDTMC, NOLAAD, CUENDT, SCONMC, FEVEMC ";
                    query += " Order by CEDTMC, CCIADT";
                }
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

            string calendario = "";

            if (this.GrupoCodigo != "")
            {
                //Buscar las empresas del grupo
                aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(this.GrupoCodigo, this.PlanCodigo);
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
        /// Ver los documentos
        /// </summary>
        /// <param name="documentos">-1->sin documentos / 1->cancelados / 2->no cancelados / 3->todos</param>
        private void LlamarVerDocumentos(int documentos)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (this.radGridViewSaldos.SelectedRows.Count == 0)
                {
                    if (this.radGridViewSaldos.Rows.Count > 1)
                    {
                        RadMessageBox.Show("Debe seleccionar un documento", "Error");  //Falta traducir
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else this.radGridViewSaldos.Rows[0].IsSelected = true;
                }

                if (this.radGridViewSaldos.SelectedRows.Count > 1)
                {
                    RadMessageBox.Show("Debe seleccionar solo un documento", "Error");  //Falta traducir
                    Cursor.Current = Cursors.Default;
                    return;
                }

                int indice = this.radGridViewSaldos.Rows.IndexOf(this.radGridViewSaldos.CurrentRow);
               
                bool lineaTotales = false;
                if (this.radGridViewSaldos.Rows.Count > 1 && this.radGridViewSaldos.Rows.Count - 1 == indice) lineaTotales = true;

                if (!lineaTotales)
                {
                    string codCuenta = this.radGridViewSaldos.Rows[indice].Cells["CUENDT"].Value.ToString();

                    string descCuenta = "";

                    if (this.PorCuentasAux) descCuenta = this.radGridViewSaldos.Rows[indice].Cells["NOMBMA"].Value.ToString();
                    else descCuenta = this.radGridViewSaldos.Rows[indice].Cells["NOLAAD"].Value.ToString();

                    string fevemc = this.radGridViewSaldos.Rows[indice].Cells["FEVEMC"].Value.ToString();

                    if (this.GrupoCodigo != "")
                    {
                        this.CompaniaCodigo = this.radGridViewSaldos.Rows[indice].Cells["CCIADT"].Value.ToString().Trim();

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

                    frmConsAuxViewDoc frmConsDoc = new frmConsAuxViewDoc
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
                        CtaMayorCodigo = codCuenta,
                        CtaMayorDesc = codCuenta.Trim() + " " + this.separadorDesc + " " + descCuenta,
                        PosAux = PosAux,
                        Documentos = documentos.ToString(),
                        DatosMonedaExt = this.DatosMonedaExt,
                        FEVEMC = fevemc
                    };

                    if (documentos == 3) frmConsDoc.CalcularPrdoMedioPago = true;
                    else frmConsDoc.CalcularPrdoMedioPago = false;

                    frmConsDoc.Show();
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("errSelCtaMayor", "Debe seleccionar una cuenta de mayor"), this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamar a Ver los movimientos
        /// </summary>
        private void LlamarVerMovimientos()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewSaldos.SelectedRows.Count == 0)
                {
                    if (this.radGridViewSaldos.Rows.Count > 1)
                    {
                        RadMessageBox.Show("Debe seleccionar un documento", "Error");  //Falta traducir
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else this.radGridViewSaldos.Rows[0].IsSelected = true;
                }

                if (this.radGridViewSaldos.SelectedRows.Count > 1)
                {
                    RadMessageBox.Show("Debe seleccionar solo un documento", "Error");  //Falta traducir
                    Cursor.Current = Cursors.Default;
                    return;
                }

                int indice = this.radGridViewSaldos.Rows.IndexOf(this.radGridViewSaldos.CurrentRow);

                bool lineaTotales = false;
                if (this.radGridViewSaldos.Rows.Count > 1 && this.radGridViewSaldos.Rows.Count - 1 == indice) lineaTotales = true;

                if (!lineaTotales)
                {
                    string codCuenta = this.radGridViewSaldos.Rows[indice].Cells["CUENDT"].Value.ToString();
                    string descCuenta = "";

                    string codCtaAux = "";
                    string descCtaAux = "";

                    if (this.PorCuentasAux)
                    {
                        descCuenta = this.CtaMayorDesc;

                        codCtaAux = this.radGridViewSaldos.Rows[indice].Cells["CAUXDT"].Value.ToString();
                        descCtaAux = codCtaAux.Trim().Trim() + this.separadorDesc + this.radGridViewSaldos.Rows[indice].Cells["NOMBMA"].Value.ToString();
                    }
                    else
                    {
                        descCuenta = this.radGridViewSaldos.Rows[indice].Cells["CUENDT"].Value.ToString().Trim() + this.separadorDesc + this.radGridViewSaldos.Rows[indice].Cells["NOLAAD"].Value.ToString().Trim();

                        codCtaAux = this.CtaAuxCodigo;
                        descCtaAux = this.CtaAuxDesc;
                    }

                    frmConsAuxViewMov frmViewConsMov = new frmConsAuxViewMov
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
                        CtaAuxCodigo = codCtaAux,
                        CtaAuxDesc = descCtaAux,
                        CtaMayorCodigo = codCuenta,
                        CtaMayorDesc = descCuenta,
                        PosAux = this.PosAux,
                        Documentos = this.Documentos,
                        DatosMonedaExt = this.DatosMonedaExt,
                        Clase = "",
                        NoDocumento = ""
                    };
                    frmViewConsMov.Show(this);
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("errSelCtaMayor", "Debe seleccionar una cuenta de mayor"), this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
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
                    DateTableDatos = this.dtSaldos,

                    //Titulo
                    Titulo = this.Text + " -  Tipo de auxiliar: " + this.TipoAuxDesc.Replace("  -  ", "-"),       //Falta traducir
                    Cabecera = true
                };
                if (this.CtaAuxCodigo != "") excelImport.Titulo += "  Cuenta de Auxiliar: " + this.CtaAuxDesc.Replace("  -  ", "-");                //Falta traducir

                if (this.GrupoCodigo != "")
                {
                    excelImport.Titulo += "  Grupo de compañías: " + this.GrupoDesc.Replace("  -  ", "-");                //Falta traducir
                    excelImport.Titulo += "  Plan: " + this.PlanDesc.Replace("  -  ", "-");                               //Falta traducir
                }
                else excelImport.Titulo += "  Compañía: " + this.CompaniaDesc.Replace("  -  ", "-");                      //Falta traducir
                excelImport.Titulo += "  Perido: " + ucConsAuxCab.AAPPDesde + " Hasta " + ucConsAuxCab.AAPPHasta;       //Falta traducir

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.radGridViewSaldos.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.radGridViewSaldos.Columns[i].HeaderText;                   //Nombre de la columna

                    switch (this.radGridViewSaldos.Columns[i].Name)
                    {
                        case "saldoiniML":
                        case "MonedaLocal":
                        case "saldofinML":
                        case "saldoiniME":
                        case "MonedaExt":
                        case "saldofinME":
                            nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            break;
                    }
                    nombreTipoVisible[2] = this.radGridViewSaldos.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.radGridViewSaldos.SelectedRows.Count > 0 && this.radGridViewSaldos.SelectedRows.Count < this.radGridViewSaldos.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.radGridViewSaldos.SelectedRows.Count; i++)
                    {
                        indice = this.radGridViewSaldos.Rows.IndexOf(this.radGridViewSaldos.SelectedRows[i]);

                        if (radGridViewSaldos.Rows.Count - 1 == indice)
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

            //Columnas
            ArrayList descColumnas = new ArrayList();
            string[] nombreTipoVisible;
            for (int i = 0; i < this.radGridViewSaldos.ColumnCount; i++)
            {
                nombreTipoVisible = new string[3];
                nombreTipoVisible[0] = this.radGridViewSaldos.Columns[i].HeaderText;                   //Nombre de la columna

                switch (this.radGridViewSaldos.Columns[i].Name)
                {
                    case "saldoiniML":
                    case "MonedaLocal":
                    case "saldofinML":
                    case "saldoiniME":
                    case "MonedaExt":
                    case "saldofinME":
                        nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                        break;
                    default:
                        nombreTipoVisible[1] = "string";
                        break;
                }
                nombreTipoVisible[2] = this.radGridViewSaldos.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                descColumnas.Add(nombreTipoVisible);
            }

            this.ExportarGrid(ref this.radGridViewSaldos, titulo, false, null, "Saldos", ref descColumnas, null);
        }

        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemDocCancelados = new RadMenuItem(this.radButtonDocCancel.Text)
            {
                Name = "menuItemDocCancelados"
            };
            menuItemDocCancelados.Click += new EventHandler(RadButtonDocCancel_Click);
            RadMenuItem menuItemDocNoCancelados = new RadMenuItem(this.radButtonDocNoCancel.Text)
            {
                Name = "menuItemDocNoCancelados"
            };
            menuItemDocNoCancelados.Click += new EventHandler(RadButtonDocNoCancel_Click);
            RadMenuItem menuItemDocTodos = new RadMenuItem(this.radButtonDocTodos.Text)
            {
                Name = "menuItemDocTodos"
            };
            menuItemDocTodos.Click += new EventHandler(RadButtonDocTodos_Click);
            RadMenuItem menuItemMovimientos = new RadMenuItem(this.radButtonMovimientos.Text)
            {
                Name = "menuItemMovimientos"
            };
            menuItemMovimientos.Click += new EventHandler(RadButtonMovimientos_Click);
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

            this.radContextMenuClickDerecho.Items.Add(menuItemDocCancelados);
            this.radContextMenuClickDerecho.Items.Add(menuItemDocNoCancelados);
            this.radContextMenuClickDerecho.Items.Add(menuItemDocTodos);
            this.radContextMenuClickDerecho.Items.Add(menuItemMovimientos);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportar);
            this.radContextMenuClickDerecho.Items.Add(menuItemSalir);
        }
        #endregion

    }
}