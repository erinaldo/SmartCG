using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModConsultaInforme
{
    public partial class frmConsAuxViewDocInfPrdoPago :  frmPlantilla, IReLocalizable
    {
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
        public string FEVEMC { get; set; }
        public DataTable Datos { get; set; }

        public frmConsAuxViewDocInfPrdoPago()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxViewDocInfPrdoPago_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar Documentos Información Periodo de Pagos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Muestra la cabecera de la consulta (criterio de selección / saldos / totales)
            this.CargarValoresCabecera();

            //Realiza los calculos
            this.CalcularInfoPeriodoMedioPago();

            //this.tablaInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSalir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSalir);
        }

        private void RadButtonSalir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSalir);
        }

        private void FrmConsAuxViewDocInfPrdoPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void FrmConsAuxViewDocInfPrdoPago_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("Consultas de Auxiliar Documentos Información Periodo de Pagos");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            switch (this.FEVEMC)
            {
                case "D":
                    this.Text = this.LP.GetText("lblInfoPrdoCobro", "Información sobre el periodo de cobro de clientes");   //Falta traducir
                    this.lblPrdoMedioPago.Text = this.LP.GetText("lblInfoPrdoMedioCobro", "Periodo medio de cobro de clientes");   //Falta traducir
                    this.lblRatioOpPagadas.Text = this.LP.GetText("lblInfoRatioOperCobradas", "Ratio de operaciones cobradas");   //Falta traducir
                    this.lblRatioOpPdtePago.Text = this.LP.GetText("lblInfoRatioOperPdteCobro", "Ratio de operaciones pendientes de cobro");   //Falta traducir
                    this.lblTotalPagos.Text = this.LP.GetText("lblInfoTotalCobros", "Total cobros realizados");   //Falta traducir
                    this.lblTotalPagosPdtes.Text = this.LP.GetText("lblInfoTotalPdtesCobro", "Total cobros pendientes");   //Falta traducir
                    break;
                case "H": 
                default:
                    this.Text = this.LP.GetText("lblInfoPrdoPago", "Información sobre el periodo de pago a proveedores");   //Falta traducir
                    this.lblPrdoMedioPago.Text = this.LP.GetText("lblInfoPrdoMedioPago", "Periodo medio de pago a proveedores");   //Falta traducir
                    this.lblRatioOpPagadas.Text = this.LP.GetText("lblInfoRatioOperPagadas", "Ratio de operaciones pagadas");   //Falta traducir
                    this.lblRatioOpPdtePago.Text = this.LP.GetText("lblInfoRatioOperPdtePago", "Ratio de operaciones pendientes de pago");   //Falta traducir
                    this.lblTotalPagos.Text = this.LP.GetText("lblInfoTotalPagos", "Total pagos realizados");   //Falta traducir
                    this.lblTotalPagosPdtes.Text = this.LP.GetText("lblInfoTotalPdtesPago", "Total pagos pendientes");   //Falta traducir
                    break;

            }

            this.radButtonSalir.Text = this.LP.GetText("lblSalir", "Salir");   //Falta traducir
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
                this.ucConsAuxCab.DatosMonedaExt.Visible = false;

                this.ucConsAuxCab.TotalDebeVisible = false;
                this.ucConsAuxCab.TotalHaberVisible = false;

                this.ucConsAuxCab.MostrarDocumentos = false;

                this.ucConsAuxCab.MostrarGrupoSaldosTotales = false;

                //Documentos
                this.ucConsAuxCab.Documentos = this.Documentos;

                this.ucConsAuxCab.ActualizarValores();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Realiza los cálculos para rellenar la tabla
        /// </summary>
        private void CalcularInfoPeriodoMedioPago()
        {
            try
            {
                string debeMLstr = "";
                decimal debeML = 0;
                string haberMLstr = "";
                decimal haberML = 0;

                string periodoPagosstr = "";
                decimal periodoPagos = 0;
                
                decimal ratioOperPagadas = 0;
                decimal sumPerPagoxDebeOHaber = 0;

                decimal sumDebeML = 0;
                decimal sumHaberML = 0;

                string periodoDiasPdtePagostr = "";
                decimal periodoDiasPdtePagos = 0;

                string saldoMLstr = "";
                decimal saldoML = 0;
                decimal sumSaldoML = 0;

                decimal ratioOperPdtesPago = 0;
                decimal sumPerPdtesPagoxSaldoML = 0;

                decimal periodoMedioPago = 0;


                this.lblValorDiasPromPago.Text = "0";
                this.lblValorRatioOperPagadas.Text = "0";
                this.lblValorRatioOperPdtesPago.Text = "0";
                this.lblValorTotalPagos.Text = "0";
                this.lblValorTotalPagosPdtes.Text = "0";

                //Cálculos
                //(No se tiene en cuenta la última línea porque es la de Totales)
                for (int i = 0; i < this.Datos.Rows.Count-1; i++)
                {
                    debeMLstr = this.Datos.Rows[i]["DEBEML"].ToString().Trim();
                    haberMLstr = this.Datos.Rows[i]["HABERML"].ToString().Trim();

                    periodoPagosstr = this.Datos.Rows[i]["DIASPAGO"].ToString().Trim();
                    periodoDiasPdtePagostr = this.Datos.Rows[i]["DIASPDTEPAGO"].ToString().Trim();

                    saldoMLstr = this.Datos.Rows[i]["SALDOML"].ToString().Trim();

                    if (debeMLstr == "") debeML = 0;
                    else
                        try { debeML = Convert.ToDecimal(debeMLstr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            debeML = 0;
                        }

                    if (haberMLstr == "") haberML = 0;
                    else
                        try { haberML = Convert.ToDecimal(haberMLstr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            haberML = 0;
                        }

                    if (saldoMLstr == "") saldoML = 0;
                    else
                        try { saldoML = Convert.ToDecimal(saldoMLstr); }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            saldoML = 0;
                        }

                    if (periodoPagosstr == "") periodoPagos = 0;
                    else
                        try 
                        { 
                            periodoPagos = Convert.ToDecimal(periodoPagosstr);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            periodoPagos = 0;
                        }
                    
                    if (periodoDiasPdtePagostr == "") periodoDiasPdtePagos = 0;
                    else
                        try 
                        { 
                            periodoDiasPdtePagos = Convert.ToDecimal(periodoDiasPdtePagostr);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            periodoDiasPdtePagos = 0;
                        }

                    switch (this.FEVEMC)
                    {
                        case "H":
                            saldoML = saldoML * -1;
                            sumPerPagoxDebeOHaber += (periodoPagos * debeML);
                            break;
                        case "D":
                            haberML = haberML * -1;
                            sumPerPagoxDebeOHaber += (periodoPagos * haberML);
                            break;
                    }

                    sumDebeML += debeML;
                    sumHaberML += haberML;
                    sumSaldoML += saldoML;

                    sumPerPdtesPagoxSaldoML += (periodoDiasPdtePagos * saldoML);
                }

                try
                {
                    switch (this.FEVEMC)
                    {
                        case "H":
                            if (sumDebeML != 0) ratioOperPagadas = sumPerPagoxDebeOHaber / sumDebeML;
                            break;
                        case "D":
                            if (haberML != 0) ratioOperPagadas = sumPerPagoxDebeOHaber / sumHaberML;
                            break;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (sumSaldoML != 0) ratioOperPdtesPago = sumPerPdtesPagoxSaldoML / sumSaldoML;
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    switch (this.FEVEMC)
                    {
                        case "H":
                            if ((sumDebeML + sumSaldoML) != 0)
                            {
                                periodoMedioPago = ((ratioOperPagadas * sumDebeML) + (ratioOperPdtesPago * sumSaldoML)) / (sumDebeML + sumSaldoML);
                            }
                            break;
                        case "D":
                            if ((sumHaberML + sumSaldoML) != 0)
                            {
                                periodoMedioPago = ((ratioOperPagadas * sumHaberML) + (ratioOperPdtesPago * sumSaldoML)) / (sumHaberML + sumSaldoML);
                            }
                            break;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }


                //Rellenar los valores en las etiquetas
                this.lblValorDiasPromPago.Text = Math.Round(periodoMedioPago).ToString();
                this.lblValorRatioOperPagadas.Text = Math.Round(ratioOperPagadas).ToString();
                this.lblValorRatioOperPdtesPago.Text = Math.Round(ratioOperPdtesPago).ToString();

                switch (this.FEVEMC)
                {
                    case "H":
                        this.lblValorTotalPagos.Text = sumDebeML.ToString("N2", this.LP.MyCultureInfo);
                        break;
                    case "D":
                        this.lblValorTotalPagos.Text = sumHaberML.ToString("N2", this.LP.MyCultureInfo);
                        break;
                }
                
                this.lblValorTotalPagosPdtes.Text = sumSaldoML.ToString("N2", this.LP.MyCultureInfo);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}