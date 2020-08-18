using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiConsultaViewFactRecibida : frmPlantilla, IReLocalizable
    {
        private string ejercicio;
        private string periodo;
        private DataRow rowDatosGrles;
        private DataRow rowMasInfo;
        private DataTable dtDetallesIVA;
        private DataRow rowEstadoCuadre;
        private FacturaIdentificador facturaID;

        private bool datosLocal = false;

        #region Propiedades
        public string Ejercicio
        {
            get
            {
                return (this.ejercicio);
            }
            set
            {
                this.ejercicio = value;
            }
        }

        public string Periodo
        {
            get
            {
                return (this.periodo);
            }
            set
            {
                this.periodo = value;
            }
        }

        public DataRow RowDatosGrles
        {
            get
            {
                return (this.rowDatosGrles);
            }
            set
            {
                this.rowDatosGrles = value;
            }
        }

        public DataRow RowMasInfo
        {
            get
            {
                return (this.rowMasInfo);
            }
            set
            {
                this.rowMasInfo = value;
            }
        }

        public DataTable DtDetallesIVA
        {
            get
            {
                return (this.dtDetallesIVA);
            }
            set
            {
                this.dtDetallesIVA = value;
            }
        }

        public DataRow RowEstadoCuadre
        {
            get
            {
                return (this.rowEstadoCuadre);
            }
            set
            {
                this.rowEstadoCuadre = value;
            }
        }

        public FacturaIdentificador FacturaID
        {
            get
            {
                return (this.facturaID);
            }
            set
            {
                this.facturaID = value;
            }
        }

        public bool DatosLocal
        {
            get
            {
                return (this.datosLocal);
            }
            set
            {
                this.datosLocal = value;
            }
        }
        #endregion

        public frmSiiConsultaViewFactRecibida()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiConsultaViewFactRecibida_Load(object sender, EventArgs e)
        {
            if (this.datosLocal)
            {
                Log.Info("INICIO SII Consulta Ver Factura Recibida (datos en local)");
            }
            else
            {
                Log.Info("INICIO SII Consulta Ver Factura Recibida (datos en Hacienda)");
            }

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Escribe la información de la cabecera de la factura
            this.EscribirDatosCabecera();

            //Escribe la información del apartado de Datos Generales de la factura
            this.EscribirDatosGenerales();

            this.ActiveControl = this.lblEjercicio;
        }

        private void dgDF_NE_DetalleIVA_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDF_DetalleIVA.Rows.Count > 0) this.dgDF_DetalleIVA.Rows[0].Selected = false;
        }

        private void toolStripButtonVerMovimientos_Click(object sender, EventArgs e)
        {
            frmSiiConsultaListaMovimientos frmListaMovs = new frmSiiConsultaListaMovimientos();
            frmListaMovs.LibroID = LibroUtiles.LibroID_FacturasRecibidas;
            frmListaMovs.FacturaID = this.facturaID;
            frmListaMovs.Ejercicio = this.ejercicio;
            frmListaMovs.Periodo = this.periodo;
            frmListaMovs.IDEmisorFactura = this.txtNIFEmisor.Text;
            frmListaMovs.Show(this);
        }

        private void frmSiiConsultaViewFactRecibida_Shown(object sender, EventArgs e)
        {
            //if (true)
            if (this.agencia == "C")
            {
                this.lblNumRegistroAcuerdoFacturacion.Text = "Número Registro Autorización Facturación";
                this.lblImpRecCuotaRecargoRectificado.Text = "Carga Impositiva Implícita Rectificada";
                this.lblRegistroPrevioGGEEoREDEMEoCompetencia.Text = "Registro Previo GGEE o REDEME";

                this.tabPageDesgloseIVA.Text = " Desglose IGIC ";
                this.gbDF_DetalleIVA.Text = " Detalle IGIC ";
                this.gbDF_DetalleSujetoPasivo.Text = " Detalle IGIC ";

                this.lblDescuadreSumCuotaRecargoEquivalenciaSinISP.Visible = false;
                this.txtDescuadreSumCuotaRecargoEquivalenciaSinISP.Visible = false;

                this.lblDF_NoExisteDetalleIVA.Text = "No hay informado detalle de IGIC";
                this.lblDF_NoExisteDetalleIVASujetoPasivo.Text = "No hay informado detalle de IGIC";
            }
        }

        private void frmSiiConsultaViewFactRecibida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaViewFactRecibida_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.datosLocal)
            {
                Log.Info("FIN SII Consulta Ver Factura Recibida (datos en local)");
            }
            else
            {
                Log.Info("FIN SII Consulta Ver Factura Recibida (datos en Hacienda)");
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            if (this.datosLocal)
            {
                this.Text = this.LP.GetText("lblfrmSiiConsultaViewFactRecibidaLocalTitulo", "Consulta de Factura Recibida (datos en local)");     //Falta traducir
            }
            else
            {
                this.Text = this.LP.GetText("lblfrmSiiConsultaViewFactRecibidaTitulo", "Consulta de Factura Recibida (datos en Hacienda)");     //Falta traducir
            }
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Escribe los datos de la cabecera de la factura
        /// </summary>
        private void EscribirDatosCabecera()
        {
            try
            {
                this.txtEjercicio.Text = this.ejercicio;
                this.txtPeriodo.Text = this.periodo;

                if (this.rowDatosGrles != null)
                {
                    this.txtNIFEmisor.Text = this.rowDatosGrles["IDEmisorFactura"].ToString();
                    this.txtNoFact.Text = this.rowDatosGrles["NumSerieFacturaEmisor"].ToString();
                    this.txtFechaDoc.Text = this.rowDatosGrles["FechaExpedicionFacturaEmisor"].ToString();

                    this.txtEstadoFactura.Text = this.rowDatosGrles["EstadoFactura"].ToString();
                    this.txtFechaUltimaModificacion.Text = this.rowDatosGrles["TimestampUltimaModificacion"].ToString();

                    if (this.txtEstadoFactura.Text == "Correcta")
                    {
                        this.txtError.Text = "";
                        this.lblError.Enabled = false;
                    }
                    else
                    {
                        string codigoError = this.rowDatosGrles["CodigoErrorRegistro"].ToString();
                        string error = this.rowDatosGrles["DescripcionErrorRegistro"].ToString();

                        if (codigoError != "" && error != "") this.txtError.Text = codigoError + " - " + error;
                        else
                        {
                            if (codigoError != "" && error == "") this.txtError.Text = codigoError;
                            else if (codigoError == "" && error != "") this.txtError.Text = error;
                        }
                        this.lblError.Enabled = true;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe los datos del apartado de Datos Generales de la factura
        /// </summary>
        private void EscribirDatosGenerales()
        {
            try
            {
                if (this.rowDatosGrles != null)
                {
                    this.txtTipoFactura.Text = this.rowDatosGrles["TipoFacturaDesc"].ToString();
                    this.txtImporteTotal.Text = this.rowDatosGrles["ImporteTotal"].ToString();
                    this.txtDescripcionOperacion.Text = this.rowDatosGrles["DescripcionOperacion"].ToString();
                    this.txtClaveRegimenEspecialOTrascendencia.Text = this.rowDatosGrles["ClaveRegimenEspecialOTrascendenciaDesc"].ToString();
                    this.txtCuotaDeducible.Text = this.rowDatosGrles["CuotaDeducible"].ToString();
                    this.txtRefExterna.Text = this.rowDatosGrles["RefExterna"].ToString();
                }

                if (this.rowMasInfo != null)
                {
                    string tipoRectificativa = this.rowMasInfo["TipoRectificativa"].ToString();
                    this.txtTipoRectificativa.Text = LibroUtiles.ListaSII_Descripcion("G", tipoRectificativa, null);

                    this.txtlRectificadaNumSeriFacturaEmisor.Text = this.rowMasInfo["FacturasRectificadasNumSerieFacturaEmisor"].ToString();
                    this.txtRectificadaFechaExpedicionFacturaEmisor.Text = this.rowMasInfo["FacturasRectificadasFechaExpedicionFacturaEmisor"].ToString();
                    this.txtImpRecBaseRectificada.Text = this.rowMasInfo["ImporteRectificacionBaseRectificada"].ToString();
                    this.txtImpRecCuotaRectificada.Text = this.rowMasInfo["ImporteRectificacionCuotaRectificada"].ToString();
                    this.txtImpRecCuotaRecargoRectificado.Text = this.rowMasInfo["ImporteRectificacionCuotaRecargoRectificado"].ToString();
                    this.txtFechaOperacion.Text = this.rowMasInfo["FechaOperacion"].ToString();
                    this.txtClaveRegimenEspecialOTrasAdicional1.Text = this.rowMasInfo["ClaveRegimenEspecialOTrascendenciaAdicional1Desc"].ToString();
                    this.txtClaveRegimenEspecialOTrasAdicional2.Text = this.rowMasInfo["ClaveRegimenEspecialOTrascendenciaAdicional2Desc"].ToString();
                    this.txtNumRegistroAcuerdoFacturacion.Text = this.rowMasInfo["NumRegistroAcuerdoFacturacion"].ToString();
                    this.txtBaseImponibleACoste.Text = this.rowMasInfo["BaseImponibleACoste"].ToString();
                    this.txtContraparteNombreRazonSocial.Text = this.rowMasInfo["ContraparteNombreRazonSocial"].ToString();
                    this.txtContraparteNIFRepresentante.Text = this.rowMasInfo["ContraparteNifRepresentante"].ToString();
                    this.txtContraparteNIF.Text = this.rowMasInfo["ContraparteNIF"].ToString();
                    this.txtContraparteIDOTROPais.Text = this.rowMasInfo["ContraparteIDOTROCodigoPais"].ToString();
                    string idOtro = this.rowMasInfo["ContraparteIDOTROIdType"].ToString();
                    this.txtContraparteIDOTROTipo.Text = LibroUtiles.ListaSII_Descripcion("I", idOtro, null);
                    this.txtContraparteIDOTROId.Text = this.rowMasInfo["ContraparteIDOTROId"].ToString();
                    this.txtDatosPresentacionNIF.Text = this.rowMasInfo["NIFPresentador"].ToString();
                    this.txtDatosPresentacionFechaHora.Text = this.rowMasInfo["TimestampPresentacion"].ToString();
                    this.txtFechaRegistroContable.Text = this.rowMasInfo["FechaRegistroContable"].ToString();
                    
                    string factSimplificadaArticulos72_73 = this.rowMasInfo["FacturaSimplificadaArticulos7.2_7.3"].ToString().Trim();
                    this.txtFactSimplificadaArticulos72_73.Text = LibroUtiles.ListaSII_SiNo(factSimplificadaArticulos72_73);

                    this.txtEntidadSucedidaNIF.Text = this.rowMasInfo["EntidadSucedidaNIF"].ToString();
                    this.txtEntidadSucedidaNombreRazonSocial.Text = this.rowMasInfo["EntidadSucedidaNombreRazonSocial"].ToString();

                    string registroPrevioGGEEoREDEMEoCompetencia = this.rowMasInfo["RegPrevioGGEEoREDEMEoCompetencia"].ToString().Trim();
                    this.txtRegistroPrevioGGEEoREDEMEoCompetencia.Text = LibroUtiles.ListaSII_SiNo(registroPrevioGGEEoREDEMEoCompetencia);

                    string macrodato = this.rowMasInfo["MacroDato"].ToString().Trim();
                    this.txtMacrodato.Text = LibroUtiles.ListaSII_SiNo(macrodato);

                    string pagos = this.rowMasInfo["Pagos"].ToString().Trim();
                    this.txtPagos.Text = LibroUtiles.ListaSII_SiNo(pagos);

                    if (this.agencia == "C")
                    {
                        if (this.rowMasInfo["DatosArt25PagoAnticipadoArt25"].ToString().Trim() == "")
                        {
                            this.tabControlFactura.TabPages.Remove(this.tabPageArticulo25);
                        }
                        else
                        {
                            this.txtPagoAnticipadoArt25.Text = this.rowMasInfo["DatosArt25PagoAnticipadoArt25"].ToString().Trim();
                            this.txtTipoBienArt25.Text = this.rowMasInfo["DatosArt25TipoBienArt25"].ToString().Trim();
                            this.txtTipoDocumentoArt25.Text = this.rowMasInfo["DatosArt25TipoDocumArt25"].ToString().Trim();
                            this.txtNumeroProtocoloArt25.Text = this.rowMasInfo["DatosArt25NumeroProtocolo"].ToString().Trim();
                            this.txtApellidosNombreArt25.Text = this.rowMasInfo["DatosArt25ApellidosNombreNotario"].ToString().Trim();
                        }
                    }
                    else this.tabControlFactura.TabPages.Remove(this.tabPageArticulo25);

                    //Escribe la información del apartado de Datos de Desglose Factura de la factura
                    this.EscribirDatosDesgloseFactura();
                }

                if (!this.datosLocal && this.rowEstadoCuadre != null)
                {
                    this.tabControlFactura.TabPages.Add(this.tabPageEstadoCuadre);

                    //Escribir estado de cuadre
                    this.txtEstadoCuadre.Text = this.rowEstadoCuadre["EstadoCuadreDesc"].ToString().Trim();
                    this.txtFechaHoraCuadre.Text = this.rowEstadoCuadre["TimestampEstadoCuadre"].ToString().Trim();

                    //Escribir datos descuadre
                    string estadoCuadre = this.rowEstadoCuadre["EstadoCuadre"].ToString().Trim();
                    if (estadoCuadre == "4")
                    {
                        this.gbDiscrepancias.Enabled = true;
                        this.txtDescuadreSumBaseImponibleConISP.Text = this.rowEstadoCuadre["DescuadreSumBaseImponibleISP"].ToString().Trim();
                        this.txtDescuadreSumBaseImponibleSinISP.Text = this.rowEstadoCuadre["DescuadreSumBaseImponible"].ToString().Trim();
                        this.txtDescuadreSumCuotaImpuestoSinISP.Text = this.rowEstadoCuadre["DescuadreSumCuota"].ToString().Trim();
                        this.txtDescuadreSumCuotaRecargoEquivalenciaSinISP.Text = this.rowEstadoCuadre["DescuadreSumCuotaRecargoEquivalencia"].ToString().Trim();
                        this.txtDescuadreImporteTotal.Text = this.rowEstadoCuadre["DescuadreImporteTotal"].ToString().Trim();
                    }
                    else
                    {
                        this.gbDiscrepancias.Enabled = false;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe los datos del apartado de Desglose Factura de la factura
        /// </summary>
        private void EscribirDatosDesgloseFactura()
        {
            try
            {
                this.tabControlFactura.TabPages.Remove(this.tabPageDesgloseIVA);
                this.tabControlFactura.TabPages.Remove(this.tabPageDesgloseSujetoPasivo);
                this.tabControlFactura.TabPages.Remove(this.tabPageEstadoCuadre);

                if (this.dtDetallesIVA != null && this.dtDetallesIVA.Rows.Count > 0)
                {
                    //Desglose Factura : Inversion Sujeto Pasivo
                    DataRow[] rows = this.dtDetallesIVA.Select("TipoDesglose='P'");
                    if (rows.Length != 0)
                    {
                        DataTable dtIVADesgloseSujetoPasivo = rows.CopyToDataTable();

                        dtIVADesgloseSujetoPasivo.TableName = "DetalleIVA";
                        if (dtIVADesgloseSujetoPasivo.Rows.Count > 0)
                        {
                            this.tabControlFactura.TabPages.Add(this.tabPageDesgloseSujetoPasivo);

                            this.BuildDataGridDetalleIVASujetoPasivo();

                            this.dgDF_DetalleSujetoPasivo.dsDatos.Tables.Add(dtIVADesgloseSujetoPasivo.Copy());

                            this.dgDF_DetalleSujetoPasivo.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDF_DetalleSujetoPasivo.DataSource = this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0];

                            //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                            this.EliminarColumnasDetallesIVASujetoPasivo();

                            //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                            this.CambiarColumnasEncabezadosDetallesIVASujetoPasivo();

                            this.dgDF_DetalleSujetoPasivo.ClearSelection();
                            this.dgDF_DetalleSujetoPasivo.Refresh();

                            this.dgDF_DetalleSujetoPasivo.Visible = true;
                            this.lblDF_NoExisteDetalleIVA.Visible = false;
                        }
                        else
                        {
                            this.dgDF_DetalleSujetoPasivo.Visible = false;
                            this.lblDF_NoExisteDetalleIVASujetoPasivo.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDF_DetalleSujetoPasivo.Visible = false;
                        this.lblDF_NoExisteDetalleIVASujetoPasivo.Visible = true;
                    }

                    //Desglose Factura : Detalle IVA
                    rows = this.dtDetallesIVA.Select("TipoDesglose='I'");
                    if (rows.Length != 0)
                    {
                        DataTable dtIVADesgloseIVA = rows.CopyToDataTable();

                        dtIVADesgloseIVA.TableName = "DetalleIVA";
                        if (dtIVADesgloseIVA.Rows.Count > 0)
                        {
                            this.tabControlFactura.TabPages.Add(this.tabPageDesgloseIVA);

                            this.BuildDataGridDetalleIVA();

                            this.dgDF_DetalleIVA.dsDatos.Tables.Add(dtIVADesgloseIVA.Copy());

                            this.dgDF_DetalleIVA.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDF_DetalleIVA.DataSource = this.dgDF_DetalleIVA.dsDatos.Tables[0];

                            //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                            this.EliminarColumnasDetallesIVA();

                            //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                            this.CambiarColumnasEncabezadosDetallesIVA();

                            this.dgDF_DetalleSujetoPasivo.ClearSelection();
                            this.dgDF_DetalleSujetoPasivo.Refresh();

                            this.dgDF_DetalleSujetoPasivo.Visible = true;
                            this.lblDF_NoExisteDetalleIVA.Visible = false;
                        }
                        else
                        {
                            this.dgDF_DetalleIVA.Visible = false;
                            this.lblDF_NoExisteDetalleIVA.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDF_DetalleIVA.Visible = false;
                        this.lblDF_NoExisteDetalleIVA.Visible = true;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataGrid que almacenará los detalles de IVAS del desglose de IVA
        /// </summary>
        /// <param name="grid"></param>
        private void BuildDataGridDetalleIVA()
        {
            try
            {
                //Crear el DataGrid
                this.dgDF_DetalleIVA.dsDatos = new DataSet();
                this.dgDF_DetalleIVA.dsDatos.DataSetName = "DesgloseIVA";
                this.dgDF_DetalleIVA.AddUltimaFilaSiNoHayDisponile = false;
                this.dgDF_DetalleIVA.ReadOnly = true;
                this.dgDF_DetalleIVA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.dgDF_DetalleIVA.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.dgDF_DetalleIVA.RowHeadersVisible = false;

                this.dgDF_DetalleIVA.AllowUserToAddRows = false;
                this.dgDF_DetalleIVA.AllowUserToOrderColumns = false;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina las columnas que son campos claves de la tabla de Detalles de IVA del Desglose de IVA
        /// </summary>
        /// <param name="grid"></param>
        private void EliminarColumnasDetallesIVA()
        {
            try
            {
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("IDEmisorFactura");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("NumSerieFacturaEmisor");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("FechaExpedicionFacturaEmisor");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("TipoDesglose")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("TipoDesglose");
                //if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("PorcentCompensacionREAGYP")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("PorcentCompensacionREAGYP");
                //if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("ImporteCompensacionREAGYP")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("ImporteCompensacionREAGYP");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("Compania")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("Compania");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("Ejercicio")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("Ejercicio");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("Periodo")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("Periodo");
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CargoAbono")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("CargoAbono");

                if (this.agencia == "C")
                {
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("TipoRecargoEquivalencia");
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Remove("CuotaRecargoEquivalencia");
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detalles de IVA del Desglose de IVA
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetallesIVA()
        {
            try
            {
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("TipoImpositivo")) this.dgDF_DetalleIVA.CambiarColumnHeader("TipoImpositivo", "Tipo Impositivo %");  //Falta traducir
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("BaseImponible")) this.dgDF_DetalleIVA.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CuotaSoportada")) this.dgDF_DetalleIVA.CambiarColumnHeader("CuotaSoportada", "Cuota Soportada");  //Falta traducir
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("PorcentCompensacionREAGYP")) this.dgDF_DetalleIVA.CambiarColumnHeader("PorcentCompensacionREAGYP", "% Compensación REAGYP");  //Falta traducir
                if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("ImporteCompensacionREAGYP")) this.dgDF_DetalleIVA.CambiarColumnHeader("ImporteCompensacionREAGYP", "Importe Compensación REAGYP");  //Falta traducir

                if (this.agencia == "C")
                {
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CargaImpositivaImplicita")) this.dgDF_DetalleIVA.CambiarColumnHeader("CargaImpositivaImplicita", "Carga Impositiva Implícita");  //Falta traducir
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CuotaRecargoMinorista")) this.dgDF_DetalleIVA.CambiarColumnHeader("CuotaRecargoMinorista", "Cuota Recargo Minorista");  //Falta traducir
                }
                else
                {
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) this.dgDF_DetalleIVA.CambiarColumnHeader("TipoRecargoEquivalencia", "Tipo Recargo Equivalencia %");  //Falta traducir
                    if (this.dgDF_DetalleIVA.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) this.dgDF_DetalleIVA.CambiarColumnHeader("CuotaRecargoEquivalencia", "Cuota Recargo Equivalencia");  //Falta traducir
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataGrid que almacenará los detalles de IVAS del Sujeto Pasivo
        /// </summary>
        /// <param name="grid"></param>
        private void BuildDataGridDetalleIVASujetoPasivo()
        {
            try
            {
                //Crear el DataGrid
                this.dgDF_DetalleSujetoPasivo.dsDatos = new DataSet();
                this.dgDF_DetalleSujetoPasivo.dsDatos.DataSetName = "DesgloseIVASujetoPasivo";
                this.dgDF_DetalleSujetoPasivo.AddUltimaFilaSiNoHayDisponile = false;
                this.dgDF_DetalleSujetoPasivo.ReadOnly = true;
                this.dgDF_DetalleSujetoPasivo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.dgDF_DetalleSujetoPasivo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.dgDF_DetalleSujetoPasivo.RowHeadersVisible = false;

                this.dgDF_DetalleSujetoPasivo.AllowUserToAddRows = false;
                this.dgDF_DetalleSujetoPasivo.AllowUserToOrderColumns = false;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina las columnas que son campos claves de la tabla de Detalles de IVA del Sujeto Pasivo
        /// </summary>
        /// <param name="grid"></param>
        private void EliminarColumnasDetallesIVASujetoPasivo()
        {
            try
            {
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("IDEmisorFactura");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("NumSerieFacturaEmisor");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("FechaExpedicionFacturaEmisor");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("TipoDesglose")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("TipoDesglose");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("Compania")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("Compania");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("Ejercicio")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("Ejercicio");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("Periodo")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("Periodo");

                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("PorcentCompensacionREAGYP")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("PorcentCompensacionREAGYP");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("ImporteCompensacionREAGYP")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("ImporteCompensacionREAGYP");
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CargoAbono")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("CargoAbono");

                if (this.agencia == "C")
                {
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("TipoRecargoEquivalencia");
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("CuotaRecargoEquivalencia");
                    
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CargaImpositivaImplicita")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("CargaImpositivaImplicita");
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CuotaRecargoMinorista")) this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Remove("CuotaRecargoMinorista");
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detalles de IVA del Sujeto Pasivo
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetallesIVASujetoPasivo()
        {
            try
            {
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("TipoImpositivo")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("TipoImpositivo", "Tipo Impositivo %");  //Falta traducir
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("BaseImponible")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CuotaSoportada")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("CuotaSoportada", "Cuota Soportada");  //Falta traducir

                if (this.agencia == "C")
                {
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CargaImpositivaImplicita")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("CargaImpositivaImplicita", "Carga Impositiva Implícita");  //Falta traducir
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CuotaRecargoMinorista")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("CuotaRecargoMinorista", "Cuota Recargo Minorista");  //Falta traducir
                }
                else
                {
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("TipoRecargoEquivalencia", "Tipo Recargo Equivalencia %");  //Falta traducir
                    if (this.dgDF_DetalleSujetoPasivo.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) this.dgDF_DetalleSujetoPasivo.CambiarColumnHeader("CuotaRecargoEquivalencia", "Cuota Recargo Equivalencia");  //Falta traducir
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}
