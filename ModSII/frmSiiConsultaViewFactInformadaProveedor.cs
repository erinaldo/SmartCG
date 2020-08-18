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
    public partial class frmSiiConsultaViewFactInformadaProveedor : frmPlantilla, IReLocalizable
    {
        private string ejercicio;
        private string periodo;
        private DataRow rowDatosGrles;
        private DataRow rowMasInfo;
        private DataTable dtDetallesIVA;
        private DataTable dtDetallesExenta;
        private FacturaIdentificador facturaID;

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

        public DataTable DtDetallesExenta
        {
            get
            {
                return (this.dtDetallesExenta);
            }
            set
            {
                this.dtDetallesExenta = value;
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
        #endregion

        public frmSiiConsultaViewFactInformadaProveedor()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiConsultaViewFactInformadaProveedor_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta Ver Factura Informada por Proveedor");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Escribe la información de la cabecera de la factura
            this.EscribirDatosCabecera();

            //Escribe la información del apartado de Datos Generales de la factura
            this.EscribirDatosGenerales();

            this.ActiveControl = this.lblEjercicio;
        }

        private void dgDF_SNE_DetalleIVA_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDF_SNE_DetalleIVA.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesIVA(ref this.dgDF_SNE_DetalleIVA, false);

                //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                this.CambiarColumnasEncabezadosDetallesIVA(ref this.dgDF_SNE_DetalleIVA);

                this.dgDF_SNE_DetalleIVA.Rows[0].Selected = false;
            }
        }

        private void dgDPS_SNE_DetalleIVA_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDPS_SNE_DetalleIVA.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesIVA(ref this.dgDPS_SNE_DetalleIVA, true);

                //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                this.CambiarColumnasEncabezadosDetallesIVA(ref this.dgDPS_SNE_DetalleIVA);

                this.dgDPS_SNE_DetalleIVA.Rows[0].Selected = false;
            }
        }

        private void dgDE_SNE_DetalleIVA_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDE_SNE_DetalleIVA.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesIVA(ref this.dgDE_SNE_DetalleIVA, false);

                //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                this.CambiarColumnasEncabezadosDetallesIVA(ref this.dgDE_SNE_DetalleIVA);

                this.dgDE_SNE_DetalleIVA.Rows[0].Selected = false;
            }
        }

        private void dgDF_SE_DetalleExenta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDF_SE_DetalleExenta.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de Exenta que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesExenta(ref this.dgDF_SE_DetalleExenta);

                //Cambiar los headers de las columnas del DataGrid de Detalles de Exenta
                this.CambiarColumnasEncabezadosDetallesExenta(ref this.dgDF_SE_DetalleExenta);

                this.dgDF_SE_DetalleExenta.Rows[0].Selected = false;
            }
        }

        private void dgDPS_SE_DetalleExenta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDPS_SE_DetalleExenta.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de Exenta que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesExenta(ref this.dgDPS_SE_DetalleExenta);

                //Cambiar los headers de las columnas del DataGrid de Detalles de Exenta
                this.CambiarColumnasEncabezadosDetallesExenta(ref this.dgDPS_SE_DetalleExenta);

                this.dgDPS_SE_DetalleExenta.Rows[0].Selected = false;
            }
        }

        private void dgDE_SE_DetalleExenta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgDE_SE_DetalleExenta.Rows.Count > 0)
            {
                //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de Exenta que ya se visualizan en la cabecera de la factura
                this.EliminarColumnasDetallesExenta(ref this.dgDE_SE_DetalleExenta);

                //Cambiar los headers de las columnas del DataGrid de Detalles de Exenta
                this.CambiarColumnasEncabezadosDetallesExenta(ref this.dgDE_SE_DetalleExenta);

                this.dgDE_SE_DetalleExenta.Rows[0].Selected = false;
            }
        }

        private void frmSiiConsultaViewFactInformadaProveedor_Shown(object sender, EventArgs e)
        {
            //if (true)
            if (this.agencia == "C")
            {
                this.lblNumRegistroAcuerdoFacturacion.Text = "Número Registro Autorización Facturación";
                this.lblImporteTransmisionSujetoAIVA.Text = "Importe Transmisión Inmuebles Sujeto a IGIC";

                this.gbDF_SNE_DetalleIVA.Text = " Detalle IGIC ";
                this.gbDPS_SNE_DetalleIVA.Text = " Detalle IGIC ";
                this.gbDE_SNE_DetalleIVA.Text = " Detalle IGIC ";

                this.lblDF_NS_ImporteArticulos7_14_Otros.Text = "Importe Por Artículos 9_Otros";
                this.lblDPS_NS_ImporteArticulos7_14_Otros.Text = "Importe Por Artículos 9_Otros";
                this.lblDE_NS_ImporteArticulos7_14_Otros.Text = "Importe Por Artículos 9_Otros";

                this.lblRegistroPrevioGGEEoREDEMEoCompetencia.Text = "Registro Previo GGEE o REDEME";

                this.lblImpRecCuotaRecargoRectificado.Visible = false;
                this.txtImpRecCuotaRecargoRectificado.Visible = false;

                this.lblDF_SNE_NoExisteDetalleIVA.Text = "No hay informado detalle de IGIC";
                this.lblDPS_SNE_NoExisteDetalleIVA.Text = "No hay informado detalle de IGIC";
                this.lblDE_SNE_NoExisteDetalleIVA.Text = "No hay informado detalle de IGIC";
            }
        }

        private void frmSiiConsultaViewFactInformadaProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaViewFactInformadaProveedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta Ver Factura Informada por Proveedor");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiConsultaViewFactInformadaProveedorTitulo", "Consulta de Factura Informada por Proveedor (datos en Hacienda)");     //Falta traducir
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
                    this.txtProveedorNIF.Text = this.rowDatosGrles["ProveedorNIF"].ToString();
                    this.txtProveedorNombreRazonSocial.Text = this.rowDatosGrles["ProveedorNombreRazonSocial"].ToString();
                    this.txtEmisorFacturaNIF.Text = this.rowDatosGrles["IDEmisorFacturaNIF"].ToString();
                    this.txtNombreRazonSocialEmisorFact.Text = this.rowDatosGrles["IDNombreRazonSocial"].ToString();
                    this.txtNoFact.Text = this.rowDatosGrles["NumSerieFacturaEmisor"].ToString();
                    this.txtFechaDoc.Text = this.rowDatosGrles["FechaExpedicionFacturaEmisor"].ToString();

                    this.txtEstadoCuadre.Text = this.rowDatosGrles["EstadoCuadreDesc"].ToString();
                    this.txtFechaHoraEstadoCuadre.Text = this.rowDatosGrles["TimestampEstadoCuadre"].ToString();
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
                    if (this.rowDatosGrles["ImporteTotalCalculado"].ToString() == "1") this.lblImporteTotal.ForeColor = Color.Blue;
                    this.txtDescripcionOperacion.Text = this.rowDatosGrles["DescripcionOperacion"].ToString();
                    this.txtClaveRegimenEspecialOTrascendencia.Text = this.rowDatosGrles["ClaveRegimenEspecialOTrascendenciaDesc"].ToString();
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
                    this.txtImporteTransmisionSujetoAIVA.Text = this.rowMasInfo["ImporteTransmisionSujetoIva"].ToString();

                    string emitidaPorTerceros = this.rowMasInfo["EmitidaPorTerceros"].ToString().Trim();
                    this.txtEmitidaPorTerceros.Text = LibroUtiles.ListaSII_SiNo(emitidaPorTerceros);

                    string variosDestinatarios = this.rowMasInfo["VariosDestinatarios"].ToString().Trim();
                    this.txtVariosDestinatarios.Text = LibroUtiles.ListaSII_SiNo(variosDestinatarios);

                    string cupon = this.rowMasInfo["Cupon"].ToString().Trim();
                    this.txtCupon.Text = LibroUtiles.ListaSII_SiNo(cupon);

                    this.txtNumSerieFacturaEmisorResumenFin.Text = this.rowMasInfo["NumSerieFacturaEmisorResumenFin"].ToString();
                    
                    string situacionInmueble = this.rowMasInfo["DatosInmuebleSituacionInmueble"].ToString().Trim();
                    if (situacionInmueble != "0") this.txtSituacionInmueble.Text = LibroUtiles.ListaSII_Descripcion("W", situacionInmueble, null);
                    else this.txtSituacionInmueble.Text = "";

                    this.txtReferenciaCatastral.Text = this.rowMasInfo["DatosInmuebleReferenciaCatastral"].ToString();
                   
                    string cobros = this.rowMasInfo["Cobros"].ToString().Trim();
                    this.txtCobros.Text = LibroUtiles.ListaSII_SiNo(cobros);

                    string factSimplificadaArticulos72_73 = this.rowMasInfo["FacturaSimplificadaArticulos7.2_7.3"].ToString().Trim();
                    this.txtFactSimplificadaArticulos72_73.Text = LibroUtiles.ListaSII_SiNo(factSimplificadaArticulos72_73);
                    
                    this.txtEntidadSucedidaNIF.Text = this.rowMasInfo["EntidadSucedidaNIF"].ToString();
                    this.txtEntidadSucedidaNombreRazonSocial.Text = this.rowMasInfo["EntidadSucedidaNombreRazonSocial"].ToString();

                    string registroPrevioGGEEoREDEMEoCompetencia = this.rowMasInfo["RegPrevioGGEEoREDEMEoCompetencia"].ToString().Trim();
                    this.txtRegistroPrevioGGEEoREDEMEoCompetencia.Text = LibroUtiles.ListaSII_SiNo(registroPrevioGGEEoREDEMEoCompetencia);

                    string macrodato = this.rowMasInfo["MacroDato"].ToString().Trim();
                    this.txtMacrodato.Text = LibroUtiles.ListaSII_SiNo(macrodato);

                    string facturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas = this.rowMasInfo["FacturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas"].ToString().Trim();
                    this.txtFacturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas.Text = LibroUtiles.ListaSII_SiNo(facturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas);

                    string facturaSinIdentifDestinatarioArticulo61d = this.rowMasInfo["FacturaSinIdentifDestinatarioArticulo6.1.d"].ToString().Trim();
                    this.txtFacturaSinIdentifDestinatarioArticulo61d.Text = LibroUtiles.ListaSII_SiNo(facturaSinIdentifDestinatarioArticulo61d);

                    string tipoDesgloseFactura = this.rowMasInfo["TipoDesgloseDesgloseFactura"].ToString();
                    if (tipoDesgloseFactura == "S")
                    {
                        //Escribe la información del apartado de Datos de Desglose Factura de la factura
                        this.EscribirDatosDesgloseFactura();

                        this.tabControlFactura.TabPages.Remove(this.tabPagePrestacionServicios);
                        this.tabControlFactura.TabPages.Remove(this.tabPageEntrega);
                    }
                    else
                    {
                        this.tabControlFactura.TabPages.Remove(this.tabPageDesgloseFactura);
                        this.tabControlFactura.TabPages.Remove(this.tabPagePrestacionServicios);
                        this.tabControlFactura.TabPages.Remove(this.tabPageEntrega);

                        int posicionTab = 1;
                        string tipoDesgloseDesgloseTipoOperacionPS = this.rowMasInfo["TipoDesgloseDesgloseTipoOperacionPS"].ToString();
                        if (tipoDesgloseDesgloseTipoOperacionPS == "S")
                        {
                            //Escribe la información del apartado de Datos de Desglose por Tipo de Prestaciones de Servicios de la factura
                            this.EscribirDatosDesgloseTipoOperacionPrestacionServicio();
                            this.tabControlFactura.TabPages.Insert(posicionTab, this.tabPagePrestacionServicios);
                            posicionTab++;
                        }

                        string tipoDesgloseDesgloseTipoOperacionEN = this.rowMasInfo["TipoDesgloseDesgloseTipoOperacionEN"].ToString();
                        if (tipoDesgloseDesgloseTipoOperacionEN == "S")
                        {
                            //Escribe la información del apartado de Datos de Desglose por Tipo de Entrega de la factura
                            this.EscribirDatosDesgloseTipoOperacionEntrega();
                            this.tabControlFactura.TabPages.Insert(posicionTab, this.tabPageEntrega);
                        }
                    }
                }
                else
                {
                    this.tabControlFactura.TabPages.Remove(this.tabPageDesgloseFactura);
                    this.tabControlFactura.TabPages.Remove(this.tabPagePrestacionServicios);
                    this.tabControlFactura.TabPages.Remove(this.tabPageEntrega);
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
                if (this.dtDetallesExenta != null && this.dtDetallesExenta.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesExenta.Select("TipoDesglose='F'");
                    if (rows.Length != 0)
                    {
                        DataTable dtExentaDesgloseFactura = rows.CopyToDataTable();

                        dtDetallesExenta.TableName = "DetalleExenta";

                        if (dtExentaDesgloseFactura.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleExenta(ref this.dgDF_SE_DetalleExenta);

                            this.dgDF_SE_DetalleExenta.dsDatos.Tables.Add(dtExentaDesgloseFactura.Copy());

                            this.dgDF_SE_DetalleExenta.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDF_SE_DetalleExenta.DataSource = this.dgDF_SE_DetalleExenta.dsDatos.Tables[0];

                            this.dgDF_SE_DetalleExenta.ClearSelection();
                            this.dgDF_SE_DetalleExenta.Refresh();

                            this.dgDF_SE_DetalleExenta.Visible = true;
                            this.lblDF_SE_NoExisteDetalleExenta.Visible = false;
                        }
                        else
                        {
                            this.dgDF_SE_DetalleExenta.Visible = false;
                            this.lblDF_SE_NoExisteDetalleExenta.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDF_SE_DetalleExenta.Visible = false;
                        this.lblDF_SE_NoExisteDetalleExenta.Visible = true;
                    }
                }
                else
                {
                    this.dgDF_SE_DetalleExenta.Visible = false;
                    this.lblDF_SE_NoExisteDetalleExenta.Visible = true;
                }

                if (this.rowMasInfo != null)
                {
                    string tipoNoExenta = this.rowMasInfo["TipoDesgloseDFSujetaNoExentaTipoNoExenta"].ToString();
                    txtDF_SNE_TipoNoExenta.Text = LibroUtiles.ListaSII_Descripcion("H", tipoNoExenta, null);
                    txtDF_NS_ImporteArticulos7_14_Otros.Text = this.rowMasInfo["TipoDesgloseDFNOSujetaImportePorArticulos714Otros"].ToString();
                    txtDF_NS_ImporteTAIReglasLocalizacion.Text = this.rowMasInfo["TipoDesgloseDFNOSujetaImporteTAIReglasLocalizacion"].ToString();
                }

                if (this.dtDetallesIVA != null && this.dtDetallesIVA.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesIVA.Select("TipoDesglose='F'");
                    if (rows.Length != 0)
                    {
                        DataTable dtIVADesgloseFactura = rows.CopyToDataTable();

                        dtIVADesgloseFactura.TableName = "DetalleIVA";

                        if (dtIVADesgloseFactura.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleIVA(ref this.dgDF_SNE_DetalleIVA);

                            this.dgDF_SNE_DetalleIVA.dsDatos.Tables.Add(dtIVADesgloseFactura.Copy());

                            this.dgDF_SNE_DetalleIVA.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDF_SNE_DetalleIVA.DataSource = this.dgDF_SNE_DetalleIVA.dsDatos.Tables[0];

                            this.dgDF_SNE_DetalleIVA.ClearSelection();
                            this.dgDF_SNE_DetalleIVA.Refresh();

                            this.dgDF_SNE_DetalleIVA.Visible = true;
                            this.lblDF_SNE_NoExisteDetalleIVA.Visible = false;
                        }
                        else
                        {
                            this.dgDF_SNE_DetalleIVA.Visible = false;
                            this.lblDF_SNE_NoExisteDetalleIVA.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDF_SNE_DetalleIVA.Visible = false;
                        this.lblDF_SNE_NoExisteDetalleIVA.Visible = true;
                    }
                }
                else
                {
                    this.dgDF_SNE_DetalleIVA.Visible = false;
                    this.lblDF_SNE_NoExisteDetalleIVA.Visible = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe los datos del apartado de Desglose Prestaciones Servicio de la factura
        /// </summary>
        private void EscribirDatosDesgloseTipoOperacionPrestacionServicio()
        {
            try
            {
                if (this.dtDetallesExenta != null && this.dtDetallesExenta.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesExenta.Select("TipoDesglose='P'");
                    if (rows.Length != 0)
                    {
                        DataTable dtExentaDesglosePrestacionServicio = rows.CopyToDataTable();

                        dtExentaDesglosePrestacionServicio.TableName = "DetalleExenta";

                        if (dtExentaDesglosePrestacionServicio.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleExenta(ref this.dgDPS_SE_DetalleExenta);

                            this.dgDPS_SE_DetalleExenta.dsDatos.Tables.Add(dtExentaDesglosePrestacionServicio.Copy());

                            this.dgDPS_SE_DetalleExenta.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDPS_SE_DetalleExenta.DataSource = this.dgDPS_SE_DetalleExenta.dsDatos.Tables[0];

                            this.dgDPS_SE_DetalleExenta.ClearSelection();
                            this.dgDPS_SE_DetalleExenta.Refresh();

                            this.dgDPS_SE_DetalleExenta.Visible = true;
                            this.lblDPS_SE_NoExisteDetalleExenta.Visible = false;
                        }
                        else
                        {
                            this.dgDPS_SE_DetalleExenta.Visible = false;
                            this.lblDPS_SE_NoExisteDetalleExenta.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDPS_SE_DetalleExenta.Visible = false;
                        this.lblDPS_SE_NoExisteDetalleExenta.Visible = true;
                    }
                }
                else
                {
                    this.dgDPS_SE_DetalleExenta.Visible = false;
                    this.lblDPS_SE_NoExisteDetalleExenta.Visible = true;
                }

                if (this.rowMasInfo != null)
                {
                    string tipoNoExenta = this.rowMasInfo["TipoDesglosePSSujetaNoExentaTipoNoExenta"].ToString();
                    txtDPS_SNE_TipoNoExenta.Text = LibroUtiles.ListaSII_Descripcion("H", tipoNoExenta, null);
                    txtDPS_NS_ImporteArticulos7_14_Otros.Text = this.rowMasInfo["TipoDesglosePSNOSujetaImportePorArticulos714Otros"].ToString();
                    txtDPS_NS_ImporteTAIReglasLocalizacion.Text = this.rowMasInfo["TipoDesglosePSNOSujetaImporteTAIReglasLocalizacion"].ToString();
                }

                if (this.dtDetallesIVA != null && this.dtDetallesIVA.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesIVA.Select("TipoDesglose='P'");
                    if (rows.Length != 0)
                    {
                        DataTable dtIVADesgloseFactura = rows.CopyToDataTable();

                        dtIVADesgloseFactura.TableName = "DetalleIVA";

                        if (dtIVADesgloseFactura.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleIVA(ref this.dgDPS_SNE_DetalleIVA);

                            this.dgDPS_SNE_DetalleIVA.dsDatos.Tables.Add(dtIVADesgloseFactura.Copy());

                            this.dgDPS_SNE_DetalleIVA.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado

                            this.dgDPS_SNE_DetalleIVA.DataSource = this.dgDPS_SNE_DetalleIVA.dsDatos.Tables[0];

                            //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                            this.EliminarColumnasDetallesIVA(ref this.dgDPS_SNE_DetalleIVA, true);

                            //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                            this.CambiarColumnasEncabezadosDetallesIVA(ref this.dgDPS_SNE_DetalleIVA);

                            this.dgDPS_SNE_DetalleIVA.ClearSelection();
                            this.dgDPS_SNE_DetalleIVA.Refresh();

                            this.dgDPS_SNE_DetalleIVA.Visible = true;
                            this.lblDPS_SNE_NoExisteDetalleIVA.Visible = false;
                        }
                        else
                        {
                            this.dgDPS_SNE_DetalleIVA.Visible = false;
                            this.lblDPS_SNE_NoExisteDetalleIVA.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDPS_SNE_DetalleIVA.Visible = false;
                        this.lblDPS_SNE_NoExisteDetalleIVA.Visible = true;
                    }
                }
                else
                {
                    this.dgDPS_SNE_DetalleIVA.Visible = false;
                    this.lblDPS_SNE_NoExisteDetalleIVA.Visible = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe los datos del apartado de Desglose Entrega de la factura
        /// </summary>
        private void EscribirDatosDesgloseTipoOperacionEntrega()
        {
            try
            {
                if (this.dtDetallesExenta != null && this.dtDetallesExenta.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesExenta.Select("TipoDesglose='E'");
                    if (rows.Length != 0)
                    {
                        DataTable dtExentaDesgloseEntrega = rows.CopyToDataTable();

                        dtDetallesExenta.TableName = "DetalleExenta";

                        if (dtExentaDesgloseEntrega.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleExenta(ref this.dgDE_SE_DetalleExenta);

                            this.dgDE_SE_DetalleExenta.dsDatos.Tables.Add(dtExentaDesgloseEntrega.Copy());

                            this.dgDE_SE_DetalleExenta.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDE_SE_DetalleExenta.DataSource = this.dgDE_SE_DetalleExenta.dsDatos.Tables[0];

                            this.dgDE_SE_DetalleExenta.ClearSelection();
                            this.dgDE_SE_DetalleExenta.Refresh();

                            this.dgDE_SE_DetalleExenta.Visible = true;
                            this.lblDE_SE_NoExisteDetalleExenta.Visible = false;
                        }
                        else
                        {
                            this.dgDE_SE_DetalleExenta.Visible = false;
                            this.lblDE_SE_NoExisteDetalleExenta.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDE_SE_DetalleExenta.Visible = false;
                        this.lblDE_SE_NoExisteDetalleExenta.Visible = true;
                    }
                }
                else
                {
                    this.dgDE_SE_DetalleExenta.Visible = false;
                    this.lblDE_SE_NoExisteDetalleExenta.Visible = true;
                }

                if (this.rowMasInfo != null)
                {
                    string tipoNoExenta = this.rowMasInfo["TipoDesgloseENSujetaNoExentaTipoNoExenta"].ToString();
                    txtDE_SNE_TipoNoExenta.Text = LibroUtiles.ListaSII_Descripcion("H", tipoNoExenta, null);
                    txtDE_NS_ImporteArticulos7_14_Otros.Text = this.rowMasInfo["TipoDesgloseENNOSujetaImportePorArticulos714Otros"].ToString();
                    txtDE_NS_ImporteTAIReglasLocalizacion.Text = this.rowMasInfo["TipoDesgloseENNOSujetaImporteTAIReglasLocalizacion"].ToString();
                }

                if (this.dtDetallesIVA != null && this.dtDetallesIVA.Rows.Count > 0)
                {
                    DataRow[] rows = this.dtDetallesIVA.Select("TipoDesglose='E'");

                    if (rows.Length != 0)
                    {
                        DataTable dtIVADesgloseFactura = rows.CopyToDataTable();

                        dtIVADesgloseFactura.TableName = "DetalleIVA";
                        if (dtIVADesgloseFactura.Rows.Count > 0)
                        {
                            this.BuildDataGridDetalleIVA(ref this.dgDE_SNE_DetalleIVA);

                            this.dgDE_SNE_DetalleIVA.dsDatos.Tables.Add(dtIVADesgloseFactura.Copy());

                            this.dgDE_SNE_DetalleIVA.AutoGenerateColumns = true;

                            //Poner como DataSource del DataGrid el DataTable creado
                            this.dgDE_SNE_DetalleIVA.DataSource = this.dgDE_SNE_DetalleIVA.dsDatos.Tables[0];

                            //Elimina las columnas que contiene los campos claves de la Tabla de Detalles de IVA que ya se visualizan en la cabecera de la factura
                            this.EliminarColumnasDetallesIVA(ref this.dgDE_SNE_DetalleIVA, false);

                            //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                            this.CambiarColumnasEncabezadosDetallesIVA(ref this.dgDE_SNE_DetalleIVA);

                            this.dgDE_SNE_DetalleIVA.ClearSelection();
                            this.dgDE_SNE_DetalleIVA.Refresh();

                            this.dgDE_SNE_DetalleIVA.Visible = true;
                            this.lblDE_SNE_NoExisteDetalleIVA.Visible = false;
                        }
                        else
                        {
                            this.dgDE_SNE_DetalleIVA.Visible = false;
                            this.lblDE_SNE_NoExisteDetalleIVA.Visible = true;
                        }
                    }
                    else
                    {
                        this.dgDE_SNE_DetalleIVA.Visible = false;
                        this.lblDE_SNE_NoExisteDetalleIVA.Visible = true;
                    }
                }
                else
                {
                    this.dgDE_SNE_DetalleIVA.Visible = false;
                    this.lblDE_SNE_NoExisteDetalleIVA.Visible = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataGrid que almacenará los detalles de IVAS
        /// </summary>
        /// <param name="grid"></param>
        private void BuildDataGridDetalleIVA(ref TGGrid grid)
        {
            try
            {
                //Crear el DataGrid
                grid.dsDatos = new DataSet();
                grid.dsDatos.DataSetName = "DesgloseIVA";
                grid.AddUltimaFilaSiNoHayDisponile = false;
                grid.ReadOnly = true;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                grid.RowHeadersVisible = false;

                grid.AllowUserToAddRows = false;
                grid.AllowUserToOrderColumns = false;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataGrid que almacenará los detalles de Exenta
        /// </summary>
        /// <param name="grid"></param>
        private void BuildDataGridDetalleExenta(ref TGGrid grid)
        {
            try
            {
                //Crear el DataGrid
                grid.dsDatos = new DataSet();
                grid.dsDatos.DataSetName = "DesgloseExenta";
                grid.AddUltimaFilaSiNoHayDisponile = false;
                grid.ReadOnly = true;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                grid.RowHeadersVisible = false;

                grid.AllowUserToAddRows = false;
                grid.AllowUserToOrderColumns = false;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina las columnas que son campos claves de la tabla de Detalles de IVA
        /// </summary>
        /// <param name="grid"></param>
        private void EliminarColumnasDetallesIVA(ref TGGrid grid, bool prestacionServicio)
        {
            try
            {
                if (grid.dsDatos.Tables[0].Columns.Contains("ProveedorNIF")) grid.dsDatos.Tables[0].Columns.Remove("ProveedorNIF");
                if (grid.dsDatos.Tables[0].Columns.Contains("ProveedorNombreRazonSocial")) grid.dsDatos.Tables[0].Columns.Remove("ProveedorNombreRazonSocial");
                if (grid.dsDatos.Tables[0].Columns.Contains("IDEmisorFacturaNIF")) grid.dsDatos.Tables[0].Columns.Remove("IDEmisorFacturaNIF");
                if (grid.dsDatos.Tables[0].Columns.Contains("IDNombreRazonSocial")) grid.dsDatos.Tables[0].Columns.Remove("IDNombreRazonSocial");
                if (grid.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) grid.dsDatos.Tables[0].Columns.Remove("NumSerieFacturaEmisor");
                if (grid.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) grid.dsDatos.Tables[0].Columns.Remove("FechaExpedicionFacturaEmisor");
                if (grid.dsDatos.Tables[0].Columns.Contains("TipoDesglose")) grid.dsDatos.Tables[0].Columns.Remove("TipoDesglose");

                if (prestacionServicio || this.agencia == "C")
                {
                    if (grid.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) grid.dsDatos.Tables[0].Columns.Remove("TipoRecargoEquivalencia");
                    if (grid.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) grid.dsDatos.Tables[0].Columns.Remove("CuotaRecargoEquivalencia");
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detalles de IVA
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetallesIVA(ref TGGrid grid)
        {
            try
            {
                if (grid.dsDatos.Tables[0].Columns.Contains("TipoImpositivo")) grid.CambiarColumnHeader("TipoImpositivo", "Tipo Impositivo %");  //Falta traducir
                if (grid.dsDatos.Tables[0].Columns.Contains("BaseImponible")) grid.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (grid.dsDatos.Tables[0].Columns.Contains("CuotaRepercutida")) grid.CambiarColumnHeader("CuotaRepercutida", "Cuota Repercutida");  //Falta traducir

                if (this.agencia != "C")
                {
                    if (grid.dsDatos.Tables[0].Columns.Contains("TipoRecargoEquivalencia")) grid.CambiarColumnHeader("TipoRecargoEquivalencia", "Tipo Recargo Equivalencia %");  //Falta traducir
                    if (grid.dsDatos.Tables[0].Columns.Contains("CuotaRecargoEquivalencia")) grid.CambiarColumnHeader("CuotaRecargoEquivalencia", "Cuota Recargo Equivalencia");  //Falta traducir
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina las columnas que son campos claves de la tabla de Detalles de Exenta
        /// </summary>
        /// <param name="grid"></param>
        private void EliminarColumnasDetallesExenta(ref TGGrid grid)
        {
            try
            {
                if (grid.dsDatos.Tables[0].Columns.Contains("ProveedorNIF")) grid.dsDatos.Tables[0].Columns.Remove("ProveedorNIF");
                if (grid.dsDatos.Tables[0].Columns.Contains("ProveedorNombreRazonSocial")) grid.dsDatos.Tables[0].Columns.Remove("ProveedorNombreRazonSocial");
                if (grid.dsDatos.Tables[0].Columns.Contains("IDEmisorFacturaNIF")) grid.dsDatos.Tables[0].Columns.Remove("IDEmisorFacturaNIF");
                if (grid.dsDatos.Tables[0].Columns.Contains("IDNombreRazonSocial")) grid.dsDatos.Tables[0].Columns.Remove("IDNombreRazonSocial");
                if (grid.dsDatos.Tables[0].Columns.Contains("TipoDesglose")) grid.dsDatos.Tables[0].Columns.Remove("TipoDesglose");
                if (grid.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) grid.dsDatos.Tables[0].Columns.Remove("NumSerieFacturaEmisor");
                if (grid.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) grid.dsDatos.Tables[0].Columns.Remove("FechaExpedicionFacturaEmisor");
                if (grid.dsDatos.Tables[0].Columns.Contains("Compania")) grid.dsDatos.Tables[0].Columns.Remove("Compania");
                if (grid.dsDatos.Tables[0].Columns.Contains("Ejercicio")) grid.dsDatos.Tables[0].Columns.Remove("Ejercicio");
                if (grid.dsDatos.Tables[0].Columns.Contains("Periodo")) grid.dsDatos.Tables[0].Columns.Remove("Periodo");
                if (grid.dsDatos.Tables[0].Columns.Contains("CausaExencion")) grid.dsDatos.Tables[0].Columns.Remove("CausaExencion");
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detalles de Exenta
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetallesExenta(ref TGGrid grid)
        {
            try
            {
                //if (grid.dsDatos.Tables[0].Columns.Contains("CausaExencion")) grid.CambiarColumnHeader("CausaExencion", "Causa Exención");  //Falta traducir
                if (grid.dsDatos.Tables[0].Columns.Contains("CausaExencionDesc")) grid.CambiarColumnHeader("CausaExencionDesc", "Causa Exención");  //Falta traducir
                if (grid.dsDatos.Tables[0].Columns.Contains("BaseImponible")) grid.CambiarColumnHeader("BaseImponible", "Importe");  //Falta traducir

                //Eliminar columna CargoAbono
                if (grid.dsDatos.Tables[0].Columns.Contains("CargoAbono")) grid.dsDatos.Tables[0].Columns.Remove("CargoAbono");
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}
