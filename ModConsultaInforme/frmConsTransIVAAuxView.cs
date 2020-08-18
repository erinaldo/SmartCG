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
    public partial class frmConsTransIVAAuxView :  frmPlantilla, IReLocalizable
    {
        protected bool menuLateralExpanded = true;
        protected static int collapseWidth = 0;

        public string NumeroIdTributaria { get; set; }
        public string CiaFiscalCodigo { get; set; }
        public string CiaFiscalDesc { get; set; }
        public string Libro { get; set; }
        public string Serie { get; set; }
        public string CodigoIVA { get; set; }
        public string FechaContableDesde { get; set; }
        public string FechaContableHasta { get; set; }
        public string FechaDocumentoDesde { get; set; }
        public string FechaDocumentoHasta { get; set; }
        public string NumDocumento { get; set; }
        public string NumFactAmpliada { get; set; }
        public string TipoTransaccion { get; set; }
        public string TipoTransaccionDesc { get; set; }

        string tipoBaseDatosCG;

        private Dictionary<string, string> displayNamesTransacciones;
        private DataTable dtTransacciones;

        public frmConsTransIVAAuxView()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radGridViewTrans.MasterView.TableSearchRow.IsVisible = false;

            this.gbCabeceraSel.ElementTree.EnableApplicationThemeName = false;
            this.gbCabeceraSel.ThemeName = "ControlDefault";

            this.gbTotales.ElementTree.EnableApplicationThemeName = false;
            this.gbTotales.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsTransIVAAuxView_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Transacciones de IVA View");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Cargar Menu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            //Muestra la cabecera de la consulta 
            this.CargarValoresCabecera();

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            
            //Crear el DataTable para la Grid
            this.BuildDataTableTransaccionesIVA();

            //Cargar las transacciones de IVA
            this.CargarInfoTransaccionesIVA();
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

        private void RadGridViewTrans_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            //if (this.radGridViewTrans.Columns.Contains("CIAFB3")) this.radGridViewTrans.Columns["CIAFB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("CLDOB3")) this.radGridViewTrans.Columns["CLDOB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("NDOCB3")) this.radGridViewTrans.Columns["NDOCB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("PCIFB3")) this.radGridViewTrans.Columns["PCIFB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("NITRB3")) this.radGridViewTrans.Columns["NITRB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("NITRB3")) this.radGridViewTrans.Columns["NUCOB3"].IsVisible = false;

            if (this.radGridViewTrans.Columns.Contains("TAUXB3")) this.radGridViewTrans.Columns["TAUXB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("CAUXB3")) this.radGridViewTrans.Columns["CAUXB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("NURCB3")) this.radGridViewTrans.Columns["NURCB3"].IsVisible = false;
            if (this.radGridViewTrans.Columns.Contains("PRFDB3")) this.radGridViewTrans.Columns["PRFDB3"].IsVisible = false;

            if (this.radGridViewTrans.Columns.Contains("BAIMB3")) this.radGridViewTrans.Columns["BAIMB3"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewTrans.Columns.Contains("CUOTB3")) this.radGridViewTrans.Columns["CUOTB3"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewTrans.Columns.Contains("TOTAL")) this.radGridViewTrans.Columns["TOTAL"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewTrans.Columns.Contains("RECGB3")) this.radGridViewTrans.Columns["RECGB3"].TextAlignment = ContentAlignment.MiddleRight;

            if (this.radGridViewTrans.Columns.Contains("TPIVB3")) this.radGridViewTrans.Columns["TPIVB3"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewTrans.Columns.Contains("REQUB3")) this.radGridViewTrans.Columns["REQUB3"].TextAlignment = ContentAlignment.MiddleRight;
        }

        private void RadGridViewTrans_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadGridViewTrans_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewTrans, ref this.selectAll);
        }

        private void RadGridViewTrans_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            this.VerComprobante();
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

        private void FrmConsTransIVAAuxView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void FrmConsTransIVAAuxView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Transacciones de IVA View");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            //this.Text = this.LP.GetText("subMenuItemConsultaAuxMov", "Consulta de Auxiliar - Movimientos");

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
                //Número de Identificación Tributaria
                this.lblNITValor.Text = this.NumeroIdTributaria;

                //Compañía Fiscal
                string ciaFiscal = this.CiaFiscalCodigo;
                if (this.CiaFiscalDesc.Trim() != "") ciaFiscal += " - " + this.CiaFiscalDesc;
                this.lblCiaFiscalValor.Text = ciaFiscal;

                //Libro
                this.lblLibroValor.Text = this.Libro;
                
                //Serie
                this.lblSerieValor.Text = this.Serie;

                //Código de IVA
                this.lblCodIVAValor.Text = this.CodigoIVA; 

                //Fecha Contable Desde
                this.lblFechaContDesdeValor.Text = this.FechaContableDesde;

                //Fecha Contable Hasta
                this.lblFechaContHastaValor.Text = this.FechaContableHasta;

                //Fecha Documento Desde
                this.lblFechaDocHastaValor.Text = this.FechaDocumentoDesde;

                //Fecha Documento Hasta
                this.lblFechaDocDesdeValor.Text = this.FechaDocumentoHasta;

                //Número de Documento
                string numDocFormat = this.NumDocumento;
                if (this.NumDocumento.Length >= 2)
                {
                    if (this.NumDocumento.Length != 2 && this.NumDocumento.Length < 9)
                    {
                        string clase = this.NumDocumento.Substring(0, 2);
                        string nDoc = this.NumDocumento.Substring(2).PadLeft(7, '0');

                        if (this.NumDocumento.Length < 9) numDocFormat = clase + nDoc;
                    }
                    numDocFormat = this.NumDocumento.Substring(0, 2) + "-" + this.NumDocumento.Substring(2);
                }
                else numDocFormat = "";
                
                this.lblNumDocValor.Text = numDocFormat;

                //Número de Factura Ampliada
                this.lblNumFactAmpliadaValor.Text = this.NumFactAmpliada;

                //Tipo de Transacción
                this.lblTipoTransacValor.Text = this.TipoTransaccionDesc;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuildDataTableTransaccionesIVA()
        {
            try
            {
                this.dtTransacciones = new DataTable
                {
                    TableName = "Tabla"
                };

                this.dtTransacciones.Columns.Add("CIAFB3", typeof(string));
                this.dtTransacciones.Columns.Add("CCIAB3", typeof(string));
                this.dtTransacciones.Columns.Add("LIBRB3", typeof(string));
                this.dtTransacciones.Columns.Add("SERIB3", typeof(string));
                this.dtTransacciones.Columns.Add("RECNB3", typeof(string));
                this.dtTransacciones.Columns.Add("FECOB3", typeof(string));
                this.dtTransacciones.Columns.Add("CLDOB3_NDOCB3", typeof(string));
                this.dtTransacciones.Columns.Add("FDOCB3", typeof(string));
                this.dtTransacciones.Columns.Add("FIVAB3", typeof(string));
                this.dtTransacciones.Columns.Add("PCIFB3_NITRB3", typeof(string));
                this.dtTransacciones.Columns.Add("NOMBMA", typeof(string));  //Se busca en otra tabla
                this.dtTransacciones.Columns.Add("BAIMB3", typeof(string));
                this.dtTransacciones.Columns.Add("CUOTB3", typeof(string));
                this.dtTransacciones.Columns.Add("TOTAL", typeof(string));  //Se calcula
                this.dtTransacciones.Columns.Add("TPIVB3", typeof(string));
                this.dtTransacciones.Columns.Add("RECGB3", typeof(string));
                this.dtTransacciones.Columns.Add("REQUB3", typeof(string));
                this.dtTransacciones.Columns.Add("COIVB3", typeof(string));
                this.dtTransacciones.Columns.Add("RESOB3", typeof(string));
                this.dtTransacciones.Columns.Add("DEDUB3", typeof(string));
                this.dtTransacciones.Columns.Add("NFAAB3", typeof(string));
                this.dtTransacciones.Columns.Add("NFARB3", typeof(string));
                this.dtTransacciones.Columns.Add("DESCB3", typeof(string));
                this.dtTransacciones.Columns.Add("NUCOB3", typeof(string));
                this.dtTransacciones.Columns.Add("NUCOB3_FORMAT", typeof(string));

                this.dtTransacciones.Columns.Add("TAUXB3", typeof(string));
                this.dtTransacciones.Columns.Add("CAUXB3", typeof(string));
                this.dtTransacciones.Columns.Add("NURCB3", typeof(string));
                this.dtTransacciones.Columns.Add("PRFDB3", typeof(string));

                this.dtTransacciones.Columns.Add("CLDOB3", typeof(string));
                this.dtTransacciones.Columns.Add("NDOCB3", typeof(string));
                this.dtTransacciones.Columns.Add("PCIFB3", typeof(string));
                this.dtTransacciones.Columns.Add("NITRB3", typeof(string));
                
                this.radGridViewTrans.DataSource = this.dtTransacciones;
                //Escribe el encabezado de la Grid de Movimientos
                this.BuildDisplayNamesTransacciones();
                this.RadGridViewTransaccionesHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Encabezados para la Grid de Transacciones
        /// </summary>
        private void BuildDisplayNamesTransacciones()
        {
            try
            {
                this.displayNamesTransacciones = new Dictionary<string, string>
                {
                    { "CIAFB3", "Compañia Fiscal" },
                    { "CCIAB3", "Compañía contable" },
                    { "LIBRB3", "Libro" },
                    { "SERIB3", "Serie" },
                    { "RECNB3", "No. Movimiento" },
                    { "FECOB3", "Fecha Comprobante" },
                    { "CLDOB3_NDOCB3", "Clase-No. Documento" },
                    { "FDOCB3", "Fecha Documento" },
                    { "FIVAB3", "Fecha Servicio" },
                    { "PCIFB3_NITRB3", "País NIF" },
                    { "NOMBMA", "Nombre o Razón Social" },  //Se busca en otra tabla
                    { "BAIMB3", "Base Imponible" },
                    { "CUOTB3", "Cuota IVA" },
                    { "TOTAL", "TOTAL" },
                    { "TPIVB3", "%IVA" },
                    { "RECGB3", "Recargo Equivalencia" },
                    { "REQUB3", "%Recargo" },
                    { "COIVB3", "Código IVA" },
                    { "RESOB3", "Tipo Transacción" },
                    { "DEDUB3", "Deducible" },
                    { "NFAAB3", "Número Factura Ampliado" },
                    { "NFARB3", "Número Factura Rectificada" },
                    { "DESCB3", "Descrripción Movimiento" },
                    { "NUCOB3", "NUCOB3" },
                    { "NUCOB3_FORMAT", "No. Comprobante" },

                    //----No se utilizan
                    { "TAUXB3", "TAUXB3" },
                    { "CAUXB3", "CAUXB3" },
                    { "NURCB3", "NURCB3" },
                    { "PRFDB3", "PRFDB3" },

                    //----Serán ocultos, se utilizan los editados
                    { "CLDOB3", "Clase" },
                    { "NDOCB3", "No Documento" },
                    { "PCIFB3", "País" },
                    { "NITRB3", "NITRB3" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Movimientos
        /// </summary>
        private void RadGridViewTransaccionesHeader()
        {
            try
            {
                if (this.radGridViewTrans.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesTransacciones)
                    {
                        if (this.radGridViewTrans.Columns.Contains(item.Key)) this.radGridViewTrans.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CargarInfoTransaccionesIVA()
        {
            IDataReader dr = null;
            try
            {
                string query = this.ObtenerConsulta();

                int contReg = 0;

                string fecha = "";
                string clase = "";
                string noDoc = "";
                string pais = "";
                string nif = "";
                string nuco = "";
                string nucoAux = "";
                string nucoFormat = "";
                string baseImpStr = "";
                decimal baseImp = 0;
                string cuotaStr = "";
                decimal cuota = 0;
                string recargoStr = "";
                decimal recargo = 0;

                decimal total = 0;
                decimal sumBaseImp = 0;
                decimal sumCuota = 0;
                decimal sumRecargo = 0;
                decimal sumTotal = 0;
                
                string nombreRazonSocial = this.ObtenerNombreRazonSocial(this.NumeroIdTributaria);

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                DataRow rowTrans;

                while (dr.Read())
                {
                    rowTrans = this.dtTransacciones.NewRow();

                    rowTrans["CIAFB3"] = dr.GetValue(dr.GetOrdinal("CIAFB3")).ToString();
                    rowTrans["CCIAB3"] = dr.GetValue(dr.GetOrdinal("CCIAB3")).ToString();
                    rowTrans["LIBRB3"] = dr.GetValue(dr.GetOrdinal("LIBRB3")).ToString();
                    rowTrans["SERIB3"] = dr.GetValue(dr.GetOrdinal("SERIB3")).ToString();
                    rowTrans["RECNB3"] = dr.GetValue(dr.GetOrdinal("RECNB3")).ToString();
                   
                    fecha = dr.GetValue(dr.GetOrdinal("FECOB3")).ToString().Trim();
                    if (fecha != "" && fecha != "0") rowTrans["FECOB3"] = utiles.FechaToFormatoCG(fecha).ToShortDateString();
                    else rowTrans["FECOB3"] = "";

                    clase = dr.GetValue(dr.GetOrdinal("CLDOB3")).ToString().Trim();
                    noDoc = dr.GetValue(dr.GetOrdinal("NDOCB3")).ToString();
                    rowTrans["CLDOB3"] = clase;
                    rowTrans["NDOCB3"] = noDoc;
                    rowTrans["CLDOB3_NDOCB3"] = clase.PadLeft(2, '0') + "-" + noDoc.PadLeft(7, '0');

                    fecha = dr.GetValue(dr.GetOrdinal("FDOCB3")).ToString().Trim();
                    if (fecha != "" && fecha != "0") rowTrans["FDOCB3"] = utiles.FechaToFormatoCG(fecha).ToShortDateString();
                    else rowTrans["FDOCB3"] = "";

                    fecha = dr.GetValue(dr.GetOrdinal("FIVAB3")).ToString().Trim();
                    if (fecha != "" && fecha != "0") rowTrans["FIVAB3"] = utiles.FechaToFormatoCG(fecha).ToShortDateString();
                    else rowTrans["FIVAB3"] = "";

                    pais = dr.GetValue(dr.GetOrdinal("PCIFB3")).ToString().Trim();
                    nif = dr.GetValue(dr.GetOrdinal("NITRB3")).ToString().Trim();
                    rowTrans["PCIFB3"] = pais;
                    rowTrans["NITRB3"] = nif;
                    rowTrans["PCIFB3_NITRB3"] = pais + nif;

                    rowTrans["NOMBMA"] = nombreRazonSocial;

                    baseImpStr = dr.GetValue(dr.GetOrdinal("BAIMB3")).ToString().Trim();
                    if (baseImpStr != "")
                        try
                        {
                            baseImp = Convert.ToDecimal(baseImpStr);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            baseImp = 0;
                        }
                    else baseImp = 0;
                    sumBaseImp += baseImp;
                    rowTrans["BAIMB3"] = baseImpStr;

                    cuotaStr = dr.GetValue(dr.GetOrdinal("CUOTB3")).ToString().Trim();
                    if (cuotaStr != "")
                        try
                        {
                            cuota = Convert.ToDecimal(cuotaStr);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            cuota = 0;
                        }
                    else cuota = 0;
                    sumCuota += cuota;
                    rowTrans["CUOTB3"] = cuotaStr;

                    recargoStr = dr.GetValue(dr.GetOrdinal("RECGB3")).ToString().Trim();
                    if (recargoStr != "")
                        try
                        {
                            recargo = Convert.ToDecimal(recargoStr);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            recargo = 0;
                        }
                    else recargo = 0;
                    sumRecargo += recargo;
                    rowTrans["RECGB3"] = recargoStr;

                    total = baseImp + cuota + recargo;
                    sumTotal += total;
                    rowTrans["TOTAL"] = dr.GetValue(dr.GetOrdinal("TOTAL")).ToString().Trim();

                    rowTrans["TPIVB3"] = dr.GetValue(dr.GetOrdinal("TPIVB3")).ToString();                    
                    rowTrans["REQUB3"] = dr.GetValue(dr.GetOrdinal("REQUB3")).ToString();
                    rowTrans["COIVB3"] = dr.GetValue(dr.GetOrdinal("COIVB3")).ToString();

                    string tipoTrans = dr.GetValue(dr.GetOrdinal("RESOB3")).ToString();
                    switch (tipoTrans)
                    {
                        case "S":
                            rowTrans["RESOB3"] = "Soportado";
                            break;
                        case "R":
                            rowTrans["RESOB3"] = "Repercutido";
                            break;
                        default:
                            rowTrans["RESOB3"] = "";
                            break;
                    }

                    rowTrans["DEDUB3"] = dr.GetValue(dr.GetOrdinal("DEDUB3")).ToString();
                    rowTrans["NFAAB3"] = dr.GetValue(dr.GetOrdinal("NFAAB3")).ToString();
                    rowTrans["NFARB3"] = dr.GetValue(dr.GetOrdinal("NFARB3")).ToString();
                    rowTrans["DESCB3"] = dr.GetValue(dr.GetOrdinal("DESCB3")).ToString();

                    nuco = dr.GetValue(dr.GetOrdinal("NUCOB3")).ToString();
                    nucoAux = nuco.Trim().PadRight(12, '0');
                    nucoFormat = nucoAux.Substring(1, 2) + "-" +
                                     nucoAux.Substring(3, 2) + "-" +
                                     nucoAux.Substring(5, 2) + "-" +
                                     nucoAux.Substring(7);
                    rowTrans["NUCOB3_FORMAT"] = nucoFormat;
                    rowTrans["NUCOB3"] = nuco;

                    rowTrans["TAUXB3"] = dr.GetValue(dr.GetOrdinal("TAUXB3")).ToString();
                    rowTrans["CAUXB3"] = dr.GetValue(dr.GetOrdinal("CAUXB3")).ToString();
                    rowTrans["NURCB3"] = dr.GetValue(dr.GetOrdinal("NURCB3")).ToString();
                    rowTrans["PRFDB3"] = dr.GetValue(dr.GetOrdinal("PRFDB3")).ToString();
                    
                    this.dtTransacciones.Rows.Add(rowTrans);

                    contReg++;
                }

                if (contReg > 0)
                {
                    this.lblTotalBaseImpValor.Text = sumBaseImp.ToString("N2", this.LP.MyCultureInfo);
                    this.lblTotalCuotaValor.Text = sumCuota.ToString("N2", this.LP.MyCultureInfo);
                    this.lblTotalRecargoValor.Text = sumRecargo.ToString("N2", this.LP.MyCultureInfo);
                    this.lblTotalValor.Text = sumTotal.ToString("N2", this.LP.MyCultureInfo);

                    if (contReg > 1)
                    {
                        rowTrans = this.dtTransacciones.NewRow();
                        rowTrans["CCIAB3"] = "TOTALES";
                        rowTrans["BAIMB3"] = sumBaseImp.ToString("N2", this.LP.MyCultureInfo);
                        rowTrans["CUOTB3"] = sumCuota.ToString("N2", this.LP.MyCultureInfo);
                        rowTrans["TOTAL"] = sumTotal.ToString("N2", this.LP.MyCultureInfo);
                        rowTrans["RECGB3"] = sumRecargo.ToString("N2", this.LP.MyCultureInfo);
                        this.dtTransacciones.Rows.Add(rowTrans);

                        this.radGridViewTrans.Rows[this.radGridViewTrans.Rows.Count - 1].PinPosition = PinnedRowPosition.Bottom;
                    }
                    
                    //importe = Convert.ToDecimal(this.SaldoInicialDocumento);
                    //this.ucConsAuxCab.SaldoInicialDesc = importe.ToString("N2", this.LP.MyCultureInfo);

                    //Calcular saldo final
                    try
                    {
                        //Saldo final es la suma de los importes + el saldo inicial
                        //decimal saldoIniDoc = Convert.ToDecimal(this.SaldoInicialDocumento);
                        //sumMontdt += saldoIniDoc;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    //this.ucConsAuxCab.SaldoFinalDesc = sumMontdt.ToString("N2", this.LP.MyCultureInfo);

                    //this.ucConsAuxCab.TotalDebeDesc = totalDebeML.ToString("N2", this.LP.MyCultureInfo);
                    //this.ucConsAuxCab.TotalHaberDesc = totalHaberML.ToString("N2", this.LP.MyCultureInfo);

                    this.radGridViewTrans.Visible = true;
                }
                else
                {
                    //No hay movimientos
                    utiles.ButtonEnabled(ref this.radButtonComprobante, false);
                    utiles.ButtonEnabled(ref this.radButtonExportar, false);

                    this.radContextMenuClickDerecho.Items["menuItemVerComprobante"].Enabled = false;
                    this.radContextMenuClickDerecho.Items["menuItemExportar"].Enabled = false;

                    //this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                    //this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                    this.lblResult.Text = "No existe o no se han podido recuperar las transacciones de IVA para el criterio de selección indicado";    //Falta traducir
                    this.lblResult.Visible = true;
                    this.radGridViewTrans.Visible = false;
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

                //this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;
                //this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;

                this.lblResult.Text = "No se han podido recuperar las transacciones de IVA";    //Falta traducir
                this.lblResult.Visible = true;
                this.radGridViewTrans.Visible = false;
            }

            if (this.radGridViewTrans.Visible)
            {
                for (int i = 0; i < this.radGridViewTrans.Columns.Count; i++)
                    this.radGridViewTrans.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                //this.radGridViewTrans.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewTrans.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.None;

                this.radGridViewTrans.MasterTemplate.BestFitColumns();
                this.radGridViewTrans.Rows[0].IsCurrent = true;
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
                query = "select CIAFB3, CCIAB3, LIBRB3, SERIB3, RECNB3, FECOB3, FDOCB3, FIVAB3, ";
                query += "CASE WHEN RESOB3 = 'S' THEN BAIMB3 ELSE(BAIMB3 * -1) END BAIMB3, ";
                query += "CASE WHEN RESOB3 = 'S' THEN CUOTB3 ELSE(CUOTB3 * -1) END CUOTB3, ";
                query += "TPIVB3, ";
                query += "CASE WHEN RESOB3 = 'S' THEN RECGB3 ELSE(RECGB3 * -1) END RECGB3, ";
                query += "REQUB3, COIVB3, RESOB3, DEDUB3, NFAAB3, NFARB3, DESCB3, NUCOB3, ";
                query += "TAUXB3, CAUXB3, NURCB3, PRFDB3, CLDOB3, NDOCB3, PCIFB3, NITRB3, ";
                query += "CASE WHEN RESOB3 = 'S' THEN BAIMB3+CUOTB3 + RECGB3 ELSE((BAIMB3 + CUOTB3 + RECGB3) * -1) END TOTAL ";
                query += "from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                query += " where ";
                query += "NITRB3 = '" + this.NumeroIdTributaria + "' ";

                if (this.CiaFiscalCodigo.Trim() != "") query += "and CIAFB3 = '" + this.CiaFiscalCodigo + "' "; 
                if (this.Libro.Trim() != "") query += "and LIBRB3 = '" + this.Libro + "' ";
                if (this.Serie.Trim() != "") query += "and SERIB3 = '" + this.Serie + "' ";
                if (this.CodigoIVA.Trim() != "") query += "and COIVB3 = '" + this.CodigoIVA + "' ";
                if (this.FechaContableDesde.Trim() != "") query += "and FECOB3 >= " + utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this.FechaContableDesde), true).ToString();
                if (this.FechaContableHasta.Trim() != "") query += " and FECOB3 <= " + utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this.FechaContableHasta), true).ToString();
                if (this.FechaDocumentoDesde.Trim() != "") query += " and FDOCB3 >= " + utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this.FechaDocumentoDesde), true).ToString();
                if (this.FechaDocumentoHasta.Trim() != "") query += " and FDOCB3 <= " + utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this.FechaDocumentoHasta), true).ToString();
                if (this.NumFactAmpliada.Trim() != "") query += " and  NFAAB3 like '%" + this.NumFactAmpliada + "%' ";
                if (this.NumDocumento.Trim() != "")
                {
                    if (this.NumDocumento.Length >= 2)
                    {
                        string clase = this.NumDocumento.Substring(0, 2);

                        if (this.NumDocumento.Length > 2)
                        {
                            string nDoc = this.NumDocumento.Substring(2);
                            query += " and  CLDOB3 = '" + clase + "' and NDOCB3 = " + nDoc;
                        }
                        else query += " and  CLDOB3 = '" + clase + "' ";
                    }
                }

                if (this.TipoTransaccion.Trim() != "") query += " and RESOB3 = '" + this.TipoTransaccion + "' ";

                query += " Order by FECOB3";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }

        /// <summary>
        /// Devuelve el nombre o razoón social para el número de identificación tributaria solicitado
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        private string ObtenerNombreRazonSocial(string nit)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                bool nombreFound = false;

                string query = "select NOMBCF from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                query += " where ";
                query += "NITRCF = '" + nit + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("NOMBCF")).ToString().Trim();
                    nombreFound = true;
                }
                dr.Close();

                if (!nombreFound)
                {
                    query = "select NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += " where ";
                    query += "NNITMA = '" + nit + "' ";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        result = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString().Trim();
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                Log.Error(Utiles.CreateExceptionString(ex));
            }
            return (result);
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
                    DateTableDatos = this.dtTransacciones,
                    Cabecera = true,
                    //Titulo
                    Titulo = this.Text + " -  Num. Id. Tributaria: " + this.NumeroIdTributaria       //Falta traducir
                };
                if (this.CiaFiscalCodigo != "")
                {
                    excelImport.Titulo += "  Compañía Fiscal: " + this.CiaFiscalCodigo;
                    if (this.CiaFiscalDesc != "") excelImport.Titulo += "  " + this.CiaFiscalDesc.Replace("  -  ", "-");     
                }
                if (this.Libro != "") excelImport.Titulo += "  Libro: " + this.Libro;
                if (this.Serie != "") excelImport.Titulo += "  Serie: " + this.Serie;
                if (this.FechaContableDesde != "") excelImport.Titulo += "  Fecha Contable Desde: " + this.FechaContableDesde;
                if (this.FechaContableHasta != "") excelImport.Titulo += "  Fecha Contable Hasta: " + this.FechaContableHasta;
                if (this.FechaDocumentoDesde != "") excelImport.Titulo += "  Fecha Documento Desde: " + this.FechaDocumentoDesde;
                if (this.FechaDocumentoHasta != "") excelImport.Titulo += "  Fecha Documento Hasta: " + this.FechaDocumentoHasta;
                if (this.NumDocumento != "") excelImport.Titulo += "  Número Documento: " + this.NumDocumento;
                if (this.NumFactAmpliada != "") excelImport.Titulo += "  Número Fact. Ampliado contiene: " + this.NumDocumento;
                if (this.TipoTransaccion != "") excelImport.Titulo += "  Tipo Transacción: " + this.TipoTransaccionDesc;

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.radGridViewTrans.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.radGridViewTrans.Columns[i].HeaderText;                   //Nombre de la columna
                    
                    switch (this.radGridViewTrans.Columns[i].Name)
                    {
                        case "BAIMB3":
                        case "CUOTB3":
                        case "TOTAL":
                        case "RECGB3":
                        case "TPIVB3":
                        case "REQUB3":
                        case "RECNB3":
                            nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            break;
                    }
                    //dt.Columns.Add("DIASPAGO", typeof(string));
                    //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                    nombreTipoVisible[2] = this.radGridViewTrans.Columns[i].IsVisible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.radGridViewTrans.SelectedRows.Count > 0 && this.radGridViewTrans.SelectedRows.Count < this.radGridViewTrans.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.radGridViewTrans.SelectedRows.Count; i++)
                    {
                        indice = this.radGridViewTrans.Rows.IndexOf(this.radGridViewTrans.SelectedRows[i]);

                        if (this.radGridViewTrans.Rows.Count - 1 == indice)
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
            string titulo = "Consulta Transacciones de IVA";

            if (this.NumeroIdTributaria.Trim() != "") titulo += " Número Id. Tributaria: " + this.NumeroIdTributaria;
            if (this.CiaFiscalCodigo.Trim() != "") titulo += " Cia. Fiscal: " + this.CiaFiscalCodigo;
            if (this.Libro.Trim() != "") titulo += " Libro: " + this.Libro;
            if (this.Serie.Trim() != "") titulo += " Serie: '" + this.Serie;
            if (this.CodigoIVA.Trim() != "") titulo += " Código IVA: " + this.CodigoIVA;
            if (this.FechaContableDesde.Trim() != "") titulo += " Fecha Contable Desde: " + this.FechaContableDesde;
            if (this.FechaContableHasta.Trim() != "") titulo += " Fecha Contable Hasta: " + this.FechaContableHasta;
            if (this.FechaDocumentoDesde.Trim() != "") titulo += " Fecha Documento Desde: " + this.FechaDocumentoDesde;
            if (this.FechaDocumentoHasta.Trim() != "") titulo += " Fecha Documento Hasta: " + this.FechaDocumentoHasta;
            if (this.NumFactAmpliada.Trim() != "") titulo += " Número Factura ampliada: " + this.NumFactAmpliada;
            if (this.NumDocumento.Trim() != "") titulo += " Número Documento: " + this.NumDocumento;
            if (this.TipoTransaccion.Trim() != "") titulo += " Tipo Transacción: " + this.TipoTransaccionDesc;

            //Columnas
            ArrayList descColumnas = new ArrayList();
            string[] nombreTipoVisible;
            for (int i = 0; i < this.radGridViewTrans.ColumnCount; i++)
            {
                nombreTipoVisible = new string[2];
                nombreTipoVisible[0] = this.radGridViewTrans.Columns[i].HeaderText;                   //Nombre de la columna

                switch (this.radGridViewTrans.Columns[i].Name)
                {
                    case "BAIMB3":
                    case "CUOTB3":
                    case "TOTAL":
                    case "RECGB3":
                    case "TPIVB3":
                    case "REQUB3":
                        nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                        break;
                    default:
                        nombreTipoVisible[1] = "string";
                        break;
                }
                //dt.Columns.Add("DIASPAGO", typeof(string));
                //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                descColumnas.Add(nombreTipoVisible);
            }

            this.ExportarGrid(ref this.radGridViewTrans, titulo, false, null, "TransaccionesIVA", ref descColumnas, null);
            /*
            string result = "";

            try
            {
                string pathFicheros = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"];

                string titulo = "Consulta Transacciones de IVA";

                if (this.NumeroIdTributaria.Trim() != "") titulo += " Número Id. Tributaria: " + this.NumeroIdTributaria;
                if (this.CiaFiscalCodigo.Trim() != "") titulo += " Cia. Fiscal: " + this.CiaFiscalCodigo;
                if (this.Libro.Trim() != "") titulo += " Libro: " + this.Libro;
                if (this.Serie.Trim() != "") titulo += " Serie: '" + this.Serie;
                if (this.CodigoIVA.Trim() != "") titulo += " Código IVA: " + this.CodigoIVA;
                if (this.FechaContableDesde.Trim() != "") titulo += " Fecha Contable Desde: " + this.FechaContableDesde;
                if (this.FechaContableHasta.Trim() != "") titulo += " Fecha Contable Hasta: " + this.FechaContableHasta;
                if (this.FechaDocumentoDesde.Trim() != "") titulo += " Fecha Documento Desde: " + this.FechaDocumentoDesde;
                if (this.FechaDocumentoHasta.Trim() != "") titulo += " Fecha Documento Hasta: " + this.FechaDocumentoHasta;
                if (this.NumFactAmpliada.Trim() != "") titulo += " Número Factura ampliada: " + this.NumFactAmpliada;
                if (this.NumDocumento.Trim() != "") titulo += " Número Documento: " + this.NumDocumento;
                if (this.TipoTransaccion.Trim() != "") titulo += " Tipo Transacción: " + this.TipoTransaccionDesc;

                ExportTelerik exportarConTelerik = new ExportTelerik(ref this.radGridViewTrans)
                {
                    Titulo = titulo,
                    PathFichero = pathFicheros,
                    ExportToMemory = false,
                    ExportType = ExportFileType.EXCEL
                };

                if (exportarConTelerik.ExportType == ExportFileType.EXCEL)
                {
                    exportarConTelerik.NombreHojaExcel_CaptionText = "TransaccionesIVA";

                    //Columnas
                    ArrayList descColumnas = new ArrayList();
                    string[] nombreTipoVisible;
                    for (int i = 0; i < this.radGridViewTrans.ColumnCount; i++)
                    {
                        nombreTipoVisible = new string[2];
                        nombreTipoVisible[0] = this.radGridViewTrans.Columns[i].HeaderText;                   //Nombre de la columna

                        switch (this.radGridViewTrans.Columns[i].Name)
                        {
                            case "BAIMB3":
                            case "CUOTB3":
                            case "TOTAL":
                            case "RECGB3":
                            case "TPIVB3":
                            case "REQUB3":
                                nombreTipoVisible[1] = "decimal";                                   //Tipo de la columna
                                break;
                            default:
                                nombreTipoVisible[1] = "string";
                                break;
                        }
                        //dt.Columns.Add("DIASPAGO", typeof(string));
                        //dt.Columns.Add("DIASPDTEPAGO", typeof(string));

                        descColumnas.Add(nombreTipoVisible);
                        exportarConTelerik.GridColumnas = descColumnas;
                    }
                }

                result = exportarConTelerik.Export();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Se ha producido un error exportando el fichero. Consulte el fichero de log para mayor información.";

                RadMessageBox.Show("Error exportando fichero (" + ex.Message + ")");
            }

            if (result != "") RadMessageBox.Show(result);

            Cursor.Current = Cursors.Default;
            */
        }

        /// <summary>
        /// Ver el comprobante del movimiento
        /// </summary>
        private void VerComprobante()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewTrans.CurrentRow is GridViewGroupRowInfo)
                {
                    if (this.radGridViewTrans.CurrentRow.IsExpanded) this.radGridViewTrans.CurrentRow.IsExpanded = false;
                    else this.radGridViewTrans.CurrentRow.IsExpanded = true;
                }
                else if (this.radGridViewTrans.CurrentRow is GridViewDataRowInfo)
                {
                    if (this.radGridViewTrans.SelectedRows.Count == 0)
                    {
                        if (this.radGridViewTrans.Rows.Count > 1)
                        {
                            RadMessageBox.Show("Debe seleccionar una transacción de IVA", "Error");  //Falta traducir
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        else this.radGridViewTrans.Rows[0].IsSelected = true;
                    }

                    if (this.radGridViewTrans.SelectedRows.Count > 1)
                    {
                        RadMessageBox.Show("Debe seleccionar solo una transacción de IVA", "Error");  //Falta traducir
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    int indice = this.radGridViewTrans.Rows.IndexOf(this.radGridViewTrans.CurrentRow);

                    bool lineaTotales = false;
                    if (this.radGridViewTrans.Rows.Count > 1 && this.radGridViewTrans.Rows.Count - 1 == indice) lineaTotales = true;

                    if (!lineaTotales)
                    {
                        string cia = this.radGridViewTrans.Rows[indice].Cells["CCIAB3"].Value.ToString();
                        string numComp = this.radGridViewTrans.Rows[indice].Cells["NUCOB3"].Value.ToString();
                        string sapr = numComp.Substring(0, 5);
                        string tico = numComp.Substring(5, 2);
                        string nuco = numComp.Substring(7);

                        //Obtener codigo plan, descripción de la compañía y si la compañia tiene campos extendidos los mismos
                        bool planCamposExtendidos = false;
                        string descCia = "";
                        string plan = "";
                        DataTable dtGLMX2 = null;
                        string result = this.ObtenerDatosCompaniaContable(cia, ref descCia, ref plan, ref planCamposExtendidos, ref dtGLMX2);

                        frmConsAuxViewComp frmViewConsComp = new frmConsAuxViewComp
                        {
                            CompaniaCodigo = cia,
                            CompaniaDesc = cia + " - " + descCia,
                            PlanCodigo = plan,
                            PlanDesc = "",
                            AAPPDesde = sapr,
                            AAPPDesdeFormat = sapr.Substring(1, 2) + "-" + sapr.Substring(3),
                            TipoComp = tico,
                            NoComp = nuco,
                            PlanCamposExtendidos = planCamposExtendidos,
                            DtGLMX2 = dtGLMX2
                        };
                        frmViewConsComp.Show(this);
                    }
                    else
                    {
                        RadMessageBox.Show(this.LP.GetText("errSelCtaMov", "Debe seleccionar un movimiento"), this.LP.GetText("errValTitulo", "Error"));
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        private string ObtenerDatosCompaniaContable(string cia, ref string descCia, ref string plan, ref bool planCamposExtendidos, ref DataTable dtGLMX2)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select TIPLMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG = '" + cia + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    descCia = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString();
                    plan = dr.GetValue(dr.GetOrdinal("TIPLMG")).ToString();
                }
                dr.Close();

                //Tipo de Base de Datos 
                tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

                //Verificar si el plan tiene campos extendidos
                planCamposExtendidos = utilesCG.PlanCamposExtendidos(plan, tipoBaseDatosCG, ref dtGLMX2);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
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


            /*
            RadMenuItem menuItemExportTelerikExcel = new RadMenuItem("Export Telerik");
            menuItemExportTelerikExcel.Name = "menuItemTelerikExcel";
            menuItemExportTelerikExcel.Click += new EventHandler(MenuItemExportTelerikExcel_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportTelerikExcel);

            RadMenuItem menuItemExportTelerikExcelMemoria = new RadMenuItem("Export Telerik Memoria");
            menuItemExportTelerikExcelMemoria.Name = "menuItemTelerikExcelMemoria";
            menuItemExportTelerikExcelMemoria.Click += new EventHandler(MenuItemExportTelerikExcelMemoria_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportTelerikExcelMemoria);

            RadMenuItem menuItemExportTelerikHTML = new RadMenuItem("Export Telerik HTML");
            menuItemExportTelerikHTML.Name = "menuItemTelerikExcelHTML";
            menuItemExportTelerikHTML.Click += new EventHandler(MenuItemExportTelerikHTML_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportTelerikHTML);

            RadMenuItem menuItemExportTelerikPDF = new RadMenuItem("Export Telerik PDF");
            menuItemExportTelerikPDF.Name = "menuItemTelerikExcelPDF";
            menuItemExportTelerikPDF.Click += new EventHandler(MenuItemExportTelerikPDF_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemExportTelerikPDF);



            RadMenuItem menuItemExportTelerikMemoria = new RadMenuItem("Export Telerik Memoria");
            menuItemExportTelerikExcel.Name = "menuItemTelerikMemoria";
            //menuItemExportTelerikExcel.Click += new EventHandler(MenuItemExportTelerikExcel_Click);

            RadMenuItem menuItemExportTelerikMemoriaExcel = new RadMenuItem("Excel");
            menuItemExportTelerikMemoriaExcel.Name = "menuItemTelerikMemoriaExcel";
            menuItemExportTelerikMemoriaExcel.Click += new EventHandler(MenuItemExportTelerikExcelMemoria_Click);

            RadMenuItem menuItemExportTelerikMemoriaHTML = new RadMenuItem("HTML");
            menuItemExportTelerikMemoriaHTML.Name = "menuItemTelerikMemoriaHTML";
            menuItemExportTelerikMemoriaHTML.Click += new EventHandler(MenuItemExportTelerikHTMLMemoria_Click);

            RadMenuItem menuItemExportTelerikMemoriaPDF = new RadMenuItem("PDF");
            menuItemExportTelerikMemoriaPDF.Name = "menuItemTelerikMemoriaPDF";
            menuItemExportTelerikMemoriaPDF.Click += new EventHandler(MenuItemExportTelerikPDFMemoria_Click);

            menuItemExportTelerikMemoria.Items.Add(menuItemExportTelerikMemoriaExcel);
            menuItemExportTelerikMemoria.Items.Add(menuItemExportTelerikMemoriaHTML);
            menuItemExportTelerikMemoria.Items.Add(menuItemExportTelerikMemoriaPDF);

            this.radContextMenuClickDerecho.Items.Add(menuItemExportTelerikMemoria);
            */
        }
        #endregion
    }
}