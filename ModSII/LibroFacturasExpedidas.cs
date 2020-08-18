using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using log4net;
using ObjectModel;

namespace ModSII
{
    class LibroFacturasExpedidas
    {
        private DataSet dsConsultaRespuesta;
        private ILog Log;   
        protected Utiles utiles;
        protected LanguageProvider LP;
        protected string agencia;

        private string cargoAbono = "";

        private bool facturaBaja = false;

        public LibroFacturasExpedidas(ILog log, Utiles utiles, LanguageProvider lp, string agencia)
        {
            this.Log = log;
            this.utiles = utiles;
            this.LP = lp;
            this.agencia = agencia;
        }

        public DataSet ObtenerDatosFacturasEmitidas(string codigoCompania, string ejercicioCG, string periodo,
                                                    string nif, string codPais, string tipoIdentif,
                                                    string nombreRazon, string numSerieFactura,
                                                    string consultaFiltroFechaDesde, string consultaFiltroFechaHasta, 
                                                    string consultaFiltroEstado,
                                                    string agenciaActual)
        {
            IDataReader dr = null;

            try
            {
                //Inicializar el DataTable de Resultado
                this.dsConsultaRespuesta = this.CrearDataSetResultadoConsultaFacturasEnviadas();

                string companiaActual = "";
                string ejercicioActual = "";
                string periodoActual = "";

                string iDEmisorFacturaNIF = "";
                string numSerieFacturaEmisor = "";
                string fechaExpedicionFacturaEmisorCG = "";
                string fechaExpedicionFacturaEmisor = "";

                string iDDestinatarioFacturaNIF = "";
                string destinatarioNIF = "";

                string fechaCGStr = "";
                int fechaCG = -1;

                string tipoDesglose = "";
                bool insertarDetalleIVADesgloseFactura = false;
                bool insertarDetalleIVADesglosePrestacionServicio = false;
                bool insertarDetalleIVADesgloseEntrega = false;

                bool insertarDetalleExentaDesgloseFactura = false;
                bool insertarDetalleExentaDesglosePrestacionServicio = false;
                bool insertarDetalleExentaDesgloseEntrega = false;

                DataRow rowDatosGrles;
                DataRow row;

                int totalReg = 0;
                decimal importeTotal = 0;
                string importeActual = "";
                decimal totalBaseImponible = 0;
                decimal totalBaseImponibleExentaActual = 0;
                decimal totalBaseImponibleNoExentaActual = 0;
                decimal totalBaseImponibleActual = 0;
                decimal totalCuotaDeducible = 0;
                decimal totalCuotaDeducibleActual = 0;

                decimal importeTotalBaja = 0;
                decimal totalBaseImponibleBaja = 0;
                decimal totalCuotaDeducibleBaja = 0;
                bool facturaBajaPendiente = false;
                bool primeraFactura = true;
                string nifActual = "";

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (codigoCompania != "") filtro += "and CIAFS2 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS2 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS2 ='" + periodo + "' ";

                //if (nif != "") filtro += "and NIFCS2 = '" + nif + "' ";

                if (nif != "")
                {
                    if (codPais != "" || tipoIdentif != "")
                    {
                        filtro += "and IDOCS2 = '" + nif + "' ";
                        filtroLOG += "and IDOEL1 = '" + nif + "' ";
                    }
                    else
                    {
                        filtro += "and NIFCS2 = '" + nif + "' ";
                        filtroLOG += "and NIFEL1 = '" + nif + "' ";
                    }
                }

                if (codPais != "")
                {
                    filtro += "and PAICS2 = '" + codPais + "' ";
                    filtroLOG += "and PAISL1 = '" + codPais + "' ";
                }

                if (tipoIdentif != "")
                {
                    filtro += "and TIDCS2 = '" + tipoIdentif + "' ";
                    filtroLOG += "and TIDEL1 = '" + tipoIdentif + "' ";
                }

                if (nombreRazon != "") filtro += "and NSFRS2 ='" + nombreRazon + "' ";
                if (numSerieFactura != "")
                {
                    filtro += "and NSFES2 LIKE '%" + numSerieFactura + "%' ";
                    filtroLOG += "and NSFEL1 LIKE '%" + numSerieFactura + "%' ";
                }
                if (consultaFiltroFechaDesde != "")
                {
                    //fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde), true);
                    fechaCGStr = utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde));
                    try{
                        fechaCG = -1;
                        if (fechaCGStr != "") fechaCG = Convert.ToInt32(fechaCGStr);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    if (fechaCG != -1)
                    {
                        filtro += "and FDOCS2 >= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 >= " + fechaCG + " ";
                    }
                }

                if (consultaFiltroFechaHasta != "")
                {
                    //filtro fecha !!!
                    //fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaHasta), true);
                    fechaCGStr = utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaHasta));
                    try
                    {
                        fechaCG = -1;
                        if (fechaCGStr != "") fechaCG = Convert.ToInt32(fechaCGStr);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    if (fechaCG != -1)
                    {
                        filtro += "and FDOCS2 <= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 <= " + fechaCG + " ";
                    }
                }

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    if (consultaFiltroEstado == "B") filtro += "and BAJAS2='B' and STATS2 = 'V' ";  //Anuladas
                    else if (consultaFiltroEstado == "V") filtro += "and STATS2 = 'V' and BAJAS2 = ' ' ";    //Correctas
                    else filtro += "and STATS2 = '" + consultaFiltroEstado + "' ";  //Pendientes de envio, Aceptadas con errores e Incorrectas
                }

                string tablaIVSII2J = GlobalVar.PrefijoTablaCG + "IVSII2J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII2J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII2J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_FacturasEmitidas + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS2 = T1.TDOCL1 and NIFES2 = T1.NIFEL1 and ";
                query += "NSFES2 = T1.NSFEL1 and FDOCS2 = T1.FDOCL1 and TPCGS2 = T1.TPCGL1 ";
                query += "left join " + tablaIVLSII + " ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "LOG on ";

                query += "LOG.TDOCL1 = T1.TDOCL1 and LOG.NIFEL1 = T1.NIFEL1 and LOG.PAISL1 = T1.PAISL1 and LOG.TIDEL1 = T1.TIDEL1 and LOG.IDOEL1 = T1.IDOEL1 and LOG.NSFEL1 = T1.NSFEL1 and ";
                query += "LOG.FDOCL1 = T1.FDOCL1 and LOG.TPCGL1 = T1.TPCGL1 and (LOG.DATEL1 * 1000000 + LOG.TIMEL1) = T1.FECHAHORA ";

                query += " where DEDUS1 = '" + agenciaActual + "' ";

                if (filtro != "")
                {
                    if (filtro.Length > 3) filtro = filtro.Substring(3, filtro.Length - 3);
                    query += "and " + filtro;
                }

                query += "order by CIAFS2, EJERS2, PERIS2, FDOCS2, NSFES2, TPCGS2 DESC";

                /*
                //Obtener Consulta
                string filtro = "";
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII2 ";

                if (codigoCompania != "") filtro += "and CIAFS2 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS2 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS2 ='" + periodo + "' ";
                if (nombreRazon != "") filtro += "and NSFRS2 ='" + nombreRazon + "' ";
                if (numSerieFactura != "") filtro += "and NSFES2 LIKE '%" + numSerieFactura + "%' ";
                if (consultaFiltroFechaDesde != "")
                {
                    fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde), true);
                    if (fechaCG != -1) filtro += "and FDOCS2 = " + fechaCG + " ";
                }

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T") filtro += "and STATS2 = '" + consultaFiltroEstado + "' ";

                if (filtro != "")
                {
                    if (filtro.Length > 3) filtro = filtro.Substring(3, filtro.Length - 3);
                    query += "where " + filtro;
                }

                query += "order by CIAFS2, EJERS2, PERIS2, FDOCS2, NSFES2";

                DataTable dtLogLastMov = null;
                */
                string tipoFactura = "";
                string claveRegimenEspecialOTrascendencia = "";
                string claveRegimenEspecialOTrascendenciaAdicional1 = "";
                string claveRegimenEspecialOTrascendenciaAdicional2 = "";
                string numRegistroAcuerdoFacturacion = "";
                string timestamp = "";
                string fecha = "";

                string destinatarioCodPais = "";
                string destinatarioIdType = "";
                string destinatarioId = "";
                string estadoActual = "";
                this.cargoAbono = "";

                string baseImponibleExenta = "";

                string facturaBajaValor = "";
                string ciafBaja = "";
                string ejerBaja = "";
                string periBaja = "";
                string nifeBaja = "";
                string paisBaja = "";
                string tideBaja = "";
                string idoeBaja = "";
                string nsfeBaja = "";
                string fdocBaja = "";
                string cargoAbonoBaja = "";

                bool totalFacturaIncrementar = true;
                decimal importeTotalActualDec = 0;

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    iDDestinatarioFacturaNIF = "";

                    insertarDetalleIVADesgloseFactura = false;
                    insertarDetalleIVADesglosePrestacionServicio = false;
                    insertarDetalleIVADesgloseEntrega = false;

                    insertarDetalleExentaDesgloseFactura = false;
                    insertarDetalleExentaDesglosePrestacionServicio = false;
                    insertarDetalleExentaDesgloseEntrega = false;

                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    rowDatosGrles = this.dsConsultaRespuesta.Tables["DatosGenerales"].NewRow();

                    companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS2")).ToString().Trim();
                    ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS2")).ToString().Trim();
                    periodoActual = dr.GetValue(dr.GetOrdinal("PERIS2")).ToString().Trim();

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFES2")).ToString().Trim();
                    numSerieFacturaEmisor = dr.GetValue(dr.GetOrdinal("NSFES2")).ToString().Trim();
                    fechaExpedicionFacturaEmisorCG = dr.GetValue(dr.GetOrdinal("FDOCS2")).ToString().Trim();
                    fechaExpedicionFacturaEmisor = utiles.FechaToFormatoCG(fechaExpedicionFacturaEmisorCG).ToShortDateString();

                    destinatarioNIF = dr.GetValue(dr.GetOrdinal("NIFCS2")).ToString().Trim();
                    if (destinatarioNIF != "")
                    {
                        iDDestinatarioFacturaNIF = destinatarioNIF;
                        destinatarioCodPais = " ";
                        destinatarioIdType = " ";
                        destinatarioId = " ";
                    }
                    else
                    {
                        destinatarioCodPais = dr.GetValue(dr.GetOrdinal("PAICS2")).ToString().Trim();

                        if (destinatarioCodPais != "") iDDestinatarioFacturaNIF = destinatarioCodPais + "-";

                        destinatarioIdType = dr.GetValue(dr.GetOrdinal("TIDCS2")).ToString().Trim();
                        destinatarioId = dr.GetValue(dr.GetOrdinal("IDOCS2")).ToString().Trim();

                        iDDestinatarioFacturaNIF += destinatarioIdType;

                        if (destinatarioId != "") iDDestinatarioFacturaNIF += "-" + destinatarioId;
                        
                        paisBaja = destinatarioCodPais;
                        tideBaja = destinatarioIdType;
                        idoeBaja = destinatarioId;
                    }

                    importeActual = dr.GetValue(dr.GetOrdinal("IMPTS2")).ToString().Trim();

                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS2")).ToString().Trim();

                    if (!primeraFactura & facturaBajaPendiente)
                    {
                        //Procesar la factura de baja anterior
                        //Si la clave de la factura actual es igual a la clave de la factura anterior (en caso de haber sido una baja), 
                        //no incrementar el contador de facturas porque se trata del cargo de una factura anulada
                        if (ciafBaja == companiaActual && ejerBaja == ejercicioActual && periBaja == periodoActual &&
                            nifeBaja == nifActual &&
                            paisBaja == destinatarioCodPais && tideBaja == destinatarioIdType && idoeBaja == destinatarioId &&
                            nsfeBaja == numSerieFacturaEmisor && fdocBaja == fechaExpedicionFacturaEmisor)
                        {
                            totalFacturaIncrementar = false;
                            try
                            {
                                this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count - 1]["SumaImporte"] = "No";
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else
                        {
                            //Chequear si existe la factura en un periodo anterior
                            bool existeFacturaPeriodoAnterior = this.ExisteFacturaPeriodoAnterior(ciafBaja, ejerBaja, periBaja, nifeBaja,
                                                                                                  paisBaja, tideBaja, idoeBaja,
                                                                                                  nsfeBaja, fdocBaja, cargoAbonoBaja);

                            if (existeFacturaPeriodoAnterior)
                            {
                                //Los importes de la factura de baja acumulan a los actuales
                                importeTotal += importeTotalBaja;
                                totalCuotaDeducible += totalCuotaDeducibleBaja;
                                totalBaseImponible += totalBaseImponibleBaja;
                                totalReg++;
                            }
                            else
                                try
                                {
                                    this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count - 1]["SumaImporte"] = "No";
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }

                        //Blanquear los campos claves que se almacenan en caso de ser una baja
                        ciafBaja = "";
                        ejerBaja = "";
                        periBaja = "";
                        //tcomBaja = "";
                        nifeBaja = "";
                        paisBaja = "";
                        tideBaja = "";
                        idoeBaja = "";
                        nsfeBaja = "";
                        fdocBaja = "";
                        cargoAbonoBaja = "";

                        importeTotalBaja = 0;
                        totalBaseImponibleBaja = 0;
                        totalCuotaDeducibleBaja = 0;

                        facturaBajaPendiente = false;
                    }

                    //Preguntar si es una anulación o no (si es una anulación no se tratan ni los importes, ni las bases imponibles, ni las cuotas, ni se incrementa el contador de facturas) 
                    facturaBajaValor = dr.GetValue(dr.GetOrdinal("BAJAS2")).ToString().Trim();
                    if (facturaBajaValor == "B")
                    {
                        this.facturaBaja = true;
                        totalFacturaIncrementar = false;
                        facturaBajaPendiente = true;

                        //Almacenar los campos claves de la factura
                        ciafBaja = companiaActual;
                        ejerBaja = ejercicioActual;
                        periBaja = periodoActual;
                        nifeBaja = dr.GetValue(dr.GetOrdinal("NIFES2")).ToString().Trim();
                        nsfeBaja = numSerieFacturaEmisor;
                        fdocBaja = fechaExpedicionFacturaEmisor;
                        cargoAbonoBaja = this.cargoAbono;

                        paisBaja = destinatarioCodPais;
                        tideBaja = destinatarioIdType;
                        idoeBaja = destinatarioId;

                        try
                        {
                            importeTotalBaja = Convert.ToDecimal(importeActual);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    else
                    {
                        this.facturaBaja = false;
                        totalFacturaIncrementar = true;
                        importeTotalBaja = 0;
                    }

                    rowDatosGrles["compania"] = companiaActual;
                    rowDatosGrles["ejercicio"] = ejercicioActual;
                    rowDatosGrles["periodo"] = periodoActual;

                    rowDatosGrles["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    rowDatosGrles["IDDestinatario"] = iDDestinatarioFacturaNIF;

                    rowDatosGrles["IDNIF"] = iDEmisorFacturaNIF;
                    rowDatosGrles["IDOTROCodigoPais"] = "";
                    rowDatosGrles["IDOTROIdType"] = "";
                    rowDatosGrles["IDOTROId"] = "";

                    rowDatosGrles["CargoAbono"] = this.cargoAbono;
                    rowDatosGrles["SumaImporte"] = "Si";

                    rowDatosGrles["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    rowDatosGrles["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;

                    rowDatosGrles["RefExterna"] = dr.GetValue(dr.GetOrdinal("REXTS2")).ToString().Trim();

                    tipoFactura = dr.GetValue(dr.GetOrdinal("TFACS2")).ToString().Trim();
                    rowDatosGrles["TipoFactura"] = tipoFactura;
                    rowDatosGrles["TipoFacturaDesc"] = LibroUtiles.ListaSII_Descripcion("U", tipoFactura, null);

                    claveRegimenEspecialOTrascendencia = dr.GetValue(dr.GetOrdinal("COPSS2")).ToString().Trim();
                    rowDatosGrles["ClaveRegimenEspecialOTrascendencia"] = claveRegimenEspecialOTrascendencia;
                    rowDatosGrles["ClaveRegimenEspecialOTrascendenciaDesc"] = LibroUtiles.ListaSII_Descripcion("E", claveRegimenEspecialOTrascendencia, null);

                    rowDatosGrles["DescripcionOperacion"] = dr.GetValue(dr.GetOrdinal("DESCS2")).ToString().Trim();

                    try
                    {
                        importeTotalActualDec = Convert.ToDecimal(importeActual);
                        rowDatosGrles["ImporteTotal"] = importeTotalActualDec.ToString("N2", this.LP.MyCultureInfo);
                    }
                    catch (Exception ex)
                    {
                        rowDatosGrles["ImporteTotal"] = importeActual;
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }

                    try
                    {
                        if (importeActual != "" && totalFacturaIncrementar) importeTotal += Convert.ToDecimal(importeActual);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }

                    estadoActual = dr.GetValue(dr.GetOrdinal("STATS2")).ToString().Trim();
                    if (this.facturaBaja && estadoActual == "V") rowDatosGrles["EstadoFactura"] = "Anulada";
                    else rowDatosGrles["EstadoFactura"] = LibroUtiles.EstadoDescripcion(estadoActual);

                    //------------ Coger Datos del LOG -------------------
                    timestamp = "";
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString().Trim();
                    if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                    timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString());
                    rowDatosGrles["TimestampUltimaModificacion"] = timestamp;
                    rowDatosGrles["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    rowDatosGrles["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();

                    //----- Insertar la factura en la tabla de MasInfo ------
                    row = this.dsConsultaRespuesta.Tables["MasInfo"].NewRow();

                    row["compania"] = companiaActual;
                    row["ejercicio"] = ejercicioActual;
                    row["periodo"] = periodoActual;

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["IDDestinatario"] = iDDestinatarioFacturaNIF;
                    row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;

                    row["Cobros"] = "";  

                    row["NIFPresentador"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString().Trim();
                    row["TimestampPresentacion"] = timestamp;

                    row["NumSerieFacturaEmisorResumenFin"] = dr.GetValue(dr.GetOrdinal("NSFRS2")).ToString().Trim();
                    row["TipoRectificativa"] = dr.GetValue(dr.GetOrdinal("TRTFS2")).ToString().Trim();

                    row["FacturasRectificadasNumSerieFacturaEmisor"] = dr.GetValue(dr.GetOrdinal("NRECS2")).ToString().Trim();
                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FRECS2")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = utiles.FechaToFormatoCG(fechaCGStr).ToShortDateString();
                            else row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = "";
                        }
                        catch { row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = ""; };
                    }
                    else row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = "";

                    row["ImporteRectificacionBaseRectificada"] = dr.GetValue(dr.GetOrdinal("BRECS2")).ToString().Trim();
                    row["ImporteRectificacionCuotaRectificada"] = dr.GetValue(dr.GetOrdinal("CRECS2")).ToString().Trim();
                    
                    if (this.agencia == "C") row["ImporteRectificacionCuotaRecargoRectificado"] = "";
                    else row["ImporteRectificacionCuotaRecargoRectificado"] = dr.GetValue(dr.GetOrdinal("CRRCS2")).ToString().Trim();

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FOPES2")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FechaOperacion"] = utiles.FechaToFormatoCG(fechaCGStr).ToShortDateString();
                            else row["FechaOperacion"] = "";
                        }
                        catch { row["FechaOperacion"] = ""; };
                    }
                    else row["FechaOperacion"] = "";

                    claveRegimenEspecialOTrascendenciaAdicional1 = dr.GetValue(dr.GetOrdinal("COP1S2")).ToString().Trim();
                    if (claveRegimenEspecialOTrascendenciaAdicional1 != "")
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1"] = claveRegimenEspecialOTrascendenciaAdicional1;
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1Desc"] = LibroUtiles.ListaSII_Descripcion("E", claveRegimenEspecialOTrascendenciaAdicional1, null);
                    }
                    else
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1"] = "";
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1Desc"] = "";
                    }

                    claveRegimenEspecialOTrascendenciaAdicional2 = dr.GetValue(dr.GetOrdinal("COP2S2")).ToString().Trim();
                    if (claveRegimenEspecialOTrascendenciaAdicional2 != "")
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2"] = claveRegimenEspecialOTrascendenciaAdicional2;
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2Desc"] = LibroUtiles.ListaSII_Descripcion("E", claveRegimenEspecialOTrascendenciaAdicional2, null);
                    }
                    else
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2"] = "";
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2Desc"] = "";
                    }

                    numRegistroAcuerdoFacturacion = dr.GetValue(dr.GetOrdinal("NRGAS2")).ToString().Trim();
                    if (numRegistroAcuerdoFacturacion != "") row["NumRegistroAcuerdoFacturacion"] = numRegistroAcuerdoFacturacion;
                    else row["NumRegistroAcuerdoFacturacion"] = "";

                    row["BaseImponibleACoste"] = dr.GetValue(dr.GetOrdinal("BIMCS2")).ToString().Trim();

                    row["DatosInmuebleSituacionInmueble"] = dr.GetValue(dr.GetOrdinal("SITIS2")).ToString().Trim();
                    row["DatosInmuebleReferenciaCatastral"] = dr.GetValue(dr.GetOrdinal("RCASS2")).ToString().Trim();

                    if (this.agencia == "C")
                    {
                        //Datos Articulo 25
                        string pagoAnticipadoArt25 = dr.GetValue(dr.GetOrdinal("PANTSB")).ToString().Trim(); 
                        row["DatosArt25PagoAnticipadoArt25"] = pagoAnticipadoArt25;
                        string tipoBienArt25 = dr.GetValue(dr.GetOrdinal("TBIESB")).ToString().Trim(); 
                        row["DatosArt25TipoBienArt25"] = tipoBienArt25;
                        string tipoDocumArt25 = dr.GetValue(dr.GetOrdinal("TPDCSB")).ToString().Trim(); 
                        row["DatosArt25TipoDocumArt25"] = tipoDocumArt25;
                        string numeroProtocolo = dr.GetValue(dr.GetOrdinal("NPROSB")).ToString().Trim(); 
                        row["DatosArt25NumeroProtocolo"] = numeroProtocolo;
                        string apellidosNombreNotario = dr.GetValue(dr.GetOrdinal("NOTASB")).ToString().Trim(); 
                        row["DatosArt25ApellidosNombreNotario"] = apellidosNombreNotario;
                    }

                    row["ImporteTransmisionSujetoIva"] = dr.GetValue(dr.GetOrdinal("ITSIS2")).ToString().Trim();

                    /*row["EmitidaPorTerceros"] = LibroUtiles.ListaSII_SiNo(dr.GetValue(dr.GetOrdinal("EMT3S2")).ToString().Trim());
                    row["VariosDestinatarios"] = LibroUtiles.ListaSII_SiNo(dr.GetValue(dr.GetOrdinal("VARDS2")).ToString().Trim());
                    row["Cupon"] = LibroUtiles.ListaSII_SiNo(dr.GetValue(dr.GetOrdinal("CUPOS2")).ToString().Trim());*/

                    row["EmitidaPorTerceros"] = dr.GetValue(dr.GetOrdinal("EMT3S2")).ToString().Trim();
                    row["VariosDestinatarios"] = dr.GetValue(dr.GetOrdinal("VARDS2")).ToString().Trim();
                    row["Cupon"] = dr.GetValue(dr.GetOrdinal("CUPOS2")).ToString().Trim();

                    row["ContraparteNombreRazonSocial"] = dr.GetValue(dr.GetOrdinal("NRAZS2")).ToString().Trim();
                    row["ContraparteNifRepresentante"] = dr.GetValue(dr.GetOrdinal("NICRS2")).ToString().Trim();

                    row["ContraparteNIF"] = destinatarioNIF;
                    row["ContraparteIDOTROCodigoPais"] = destinatarioCodPais;
                    row["ContraparteIDOTROIdType"] = destinatarioIdType;
                    row["ContraparteIDOTROId"] = destinatarioId;

                    totalBaseImponibleExentaActual = 0;
                    totalBaseImponibleNoExentaActual = 0;
                    tipoDesglose = dr.GetValue(dr.GetOrdinal("TDSGS2")).ToString().Trim();
                    if (tipoDesglose == "F")
                    {
                        row["TipoDesgloseDesgloseFactura"] = "S";

                        //Sujeta
                        if (dr.GetValue(dr.GetOrdinal("HYSJF2")).ToString().Trim() == "S")
                        {
                            row["TipoDesgloseDFSujeta"] = "S";

                            //Exenta
                            if (dr.GetValue(dr.GetOrdinal("HYEXF2")).ToString().Trim() == "S")
                            {
                                row["TipoDesgloseDFSujetaExenta"] = "S";

                                insertarDetalleExentaDesgloseFactura = true;
                            }
                            else row["TipoDesgloseDFSujetaExenta"] = "N";

                            //No Exenta
                            if (dr.GetValue(dr.GetOrdinal("HYNEF2")).ToString().Trim() == "S")
                            {
                                row["TipoDesgloseDFSujetaNoExenta"] = "S";
                                row["TipoDesgloseDFSujetaNoExentaTipoNoExenta"] = dr.GetValue(dr.GetOrdinal("CTOSF2")).ToString().Trim();

                                insertarDetalleIVADesgloseFactura = true;
                            }
                            else
                            {
                                row["TipoDesgloseDFSujetaNoExenta"] = "N";
                                row["TipoDesgloseDFSujetaNoExentaTipoNoExenta"] = "";
                            }
                        }
                        else
                        {
                            row["TipoDesgloseDFSujeta"] = "N";
                        }

                        //No Sujeta
                        if (dr.GetValue(dr.GetOrdinal("HYNSF2")).ToString().Trim() == "S")
                        {
                            row["TipoDesgloseDFNOSujeta"] = "S";

                            row["TipoDesgloseDFNOSujetaImportePorArticulos714Otros"] = dr.GetValue(dr.GetOrdinal("INS1F2")).ToString().Trim();
                            row["TipoDesgloseDFNOSujetaImporteTAIReglasLocalizacion"] = dr.GetValue(dr.GetOrdinal("INS2F2")).ToString().Trim();
                        }
                        else
                        {
                            row["TipoDesgloseDFNOSujeta"] = "N";
                        }
                    }
                    else
                    {
                        row["TipoDesgloseDesgloseFactura"] = "N";

                        if (tipoDesglose == "O" || tipoDesglose == "S")
                        {
                            //Tipo desglose Pestación de Servicios
                            row["TipoDesgloseDesgloseTipoOperacionPS"] = "S";

                            //Sujeta
                            if (dr.GetValue(dr.GetOrdinal("HYSJS2")).ToString().Trim() == "S")
                            { 
                                row["TipoDesglosePSSujeta"] = "S";
                                 
                                //Exenta
                                if (dr.GetValue(dr.GetOrdinal("HYEXS2")).ToString().Trim() == "S")
                                {
                                    row["TipoDesglosePSSujetaExenta"] = "S";

                                    insertarDetalleExentaDesglosePrestacionServicio = true;
                                }
                                else row["TipoDesglosePSSujetaExenta"] = "N";

                                //No Exenta
                                if (dr.GetValue(dr.GetOrdinal("HYNES2")).ToString().Trim() == "S")
                                {
                                    row["TipoDesglosePSSujetaNoExenta"] = "S";
                                    row["TipoDesglosePSSujetaNoExentaTipoNoExenta"] = dr.GetValue(dr.GetOrdinal("CTOSS2")).ToString().Trim();

                                    insertarDetalleIVADesglosePrestacionServicio = true;
                                }
                                else
                                {
                                    row["TipoDesglosePSSujetaNoExenta"] = "N";
                                    row["TipoDesglosePSSujetaNoExentaTipoNoExenta"] = "";
                                }
                            }
                            else
                            {
                                row["TipoDesglosePSSujeta"] = "N";
                            }

                            //No Sujeta
                            if (dr.GetValue(dr.GetOrdinal("HYNSS2")).ToString().Trim() == "S")
                            {
                                row["TipoDesglosePSNOSujeta"] = "S";

                                row["TipoDesglosePSNOSujetaImportePorArticulos714Otros"] = dr.GetValue(dr.GetOrdinal("INS1S2")).ToString().Trim();
                                row["TipoDesglosePSNOSujetaImporteTAIReglasLocalizacion"] = dr.GetValue(dr.GetOrdinal("INS2S2")).ToString().Trim();
                            }
                            else
                            {
                                row["TipoDesglosePSNOSujeta"] = "N";
                            }
                        }

                        if (tipoDesglose == "O" || tipoDesglose == "E")
                        {
                            //Tipo desglose Entrega
                            row["TipoDesgloseDesgloseTipoOperacionEN"] = "S";

                            //Sujeta
                            if (dr.GetValue(dr.GetOrdinal("HYSJE2")).ToString().Trim() == "S")
                            {
                                row["TipoDesgloseENSujeta"] = "S";

                                //Exenta
                                if (dr.GetValue(dr.GetOrdinal("HYEXE2")).ToString().Trim() == "S")
                                {
                                    row["TipoDesgloseENSujetaExenta"] = "S";

                                    insertarDetalleExentaDesgloseEntrega = true;
                                }
                                else row["TipoDesglosePSSujetaExenta"] = "N";

                                //No Exenta
                                if (dr.GetValue(dr.GetOrdinal("HYNEE2")).ToString().Trim() == "S")
                                {
                                    row["TipoDesgloseENSujetaNoExenta"] = "S";
                                    row["TipoDesgloseENSujetaNoExentaTipoNoExenta"] = dr.GetValue(dr.GetOrdinal("CTOSE2")).ToString().Trim();

                                    insertarDetalleIVADesgloseEntrega = true;
                                }
                                else
                                {
                                    row["TipoDesgloseENSujetaNoExenta"] = "N";
                                    row["TipoDesgloseENSujetaNoExentaTipoNoExenta"] = "";
                                }
                            }
                            else
                            {
                                row["TipoDesglosePSSujeta"] = "N";
                            }

                            //No Sujeta
                            if (dr.GetValue(dr.GetOrdinal("HYNSE2")).ToString().Trim() == "S")
                            {
                                row["TipoDesgloseENNOSujeta"] = "S";

                                row["TipoDesgloseENNOSujetaImportePorArticulos714Otros"] = dr.GetValue(dr.GetOrdinal("INS1E2")).ToString().Trim();
                                row["TipoDesgloseENNOSujetaImporteTAIReglasLocalizacion"] = dr.GetValue(dr.GetOrdinal("INS2E2")).ToString().Trim();
                            }
                            else
                            {
                                row["TipoDesgloseENNOSujeta"] = "N";
                            }
                        }
                    }

                    row["CargoAbono"] = this.cargoAbono;

                    row["FacturaSimplificadaArticulos7.2_7.3"] = dr.GetValue(dr.GetOrdinal("FSIMS2")).ToString().Trim();

                    string entidadSucedidaNIF = dr.GetValue(dr.GetOrdinal("NIFSS2")).ToString().Trim();
                    row["EntidadSucedidaNIF"] = entidadSucedidaNIF;
                    if (entidadSucedidaNIF != "") row["EntidadSucedidaNombreRazonSocial"] = LibroUtiles.ObtenerNombreRazonSocialCiaFiscalDadoNIF(entidadSucedidaNIF);
                    else row["EntidadSucedidaNombreRazonSocial"] = "";

                    row["RegPrevioGGEEoREDEMEoCompetencia"] = dr.GetValue(dr.GetOrdinal("RPRES2")).ToString().Trim();
                    row["Macrodato"] = dr.GetValue(dr.GetOrdinal("MACDS2")).ToString().Trim();
                    row["FacturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas"] = dr.GetValue(dr.GetOrdinal("FGASS2")).ToString().Trim();
                    row["FacturaSinIdentifDestinatarioArticulo6.1.d"] = dr.GetValue(dr.GetOrdinal("FSIDS2")).ToString().Trim();

                    this.dsConsultaRespuesta.Tables["MasInfo"].Rows.Add(row);

                    totalCuotaDeducibleActual = 0;
                    if (insertarDetalleIVADesgloseFactura)
                    {
                        //----- Insertar la factura en la tabla de DetallesIVA ------
                        this.InsertarDetalleIVADesgloseFactura(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, "F", numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, ref totalCuotaDeducibleActual);
                    }
                    else
                    {
                        if (insertarDetalleIVADesglosePrestacionServicio)
                        {
                            //----- Insertar la factura en la tabla de DetallesIVA ------
                            this.InsertarDetalleIVADesglosePrestacionServicio(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, ref totalCuotaDeducibleActual);
                        }

                        if (insertarDetalleIVADesgloseEntrega)
                        {
                            //----- Insertar la factura en la tabla de DetallesIVA ------
                            this.InsertarDetalleIVADesgloseEntrega(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, "E", numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, ref totalCuotaDeducibleActual);
                        }
                    }

                    if (insertarDetalleExentaDesgloseFactura)
                    {
                        //----- Insertar la factura en la tabla de DetallesExenta ------
                        this.InsertarDetalleExenta(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, "F", numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, "BINE", "F2");
                    }
                    else
                    {
                        if (insertarDetalleExentaDesglosePrestacionServicio)
                        {
                            //----- Insertar la factura en la tabla de DetallesIVA ------
                            this.InsertarDetalleExenta(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, "P", numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, "BINS", "S2");
                        }

                        if (insertarDetalleExentaDesgloseEntrega)
                        {
                            //----- Insertar la factura en la tabla de DetallesIVA ------
                            this.InsertarDetalleExenta(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, iDDestinatarioFacturaNIF, "E", numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, ref totalBaseImponibleNoExentaActual, "BINS", "E2");
                        }
                    }

                    totalBaseImponibleActual = totalBaseImponibleExentaActual + totalBaseImponibleNoExentaActual;
                    if (totalFacturaIncrementar)
                    {
                        totalBaseImponible += totalBaseImponibleActual;
                        totalCuotaDeducible += totalCuotaDeducibleActual;
                    }
                    else 
                        if (this.facturaBaja)
                        {
                            totalBaseImponibleBaja = totalBaseImponibleActual;
                            totalCuotaDeducibleBaja = totalCuotaDeducibleActual;
                        }

                    rowDatosGrles["BaseImponible"] = totalBaseImponibleActual.ToString("N2", this.LP.MyCultureInfo);
                    rowDatosGrles["Cuota"] = totalCuotaDeducibleActual.ToString("N2", this.LP.MyCultureInfo);

                    this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Add(rowDatosGrles);

                    if (totalFacturaIncrementar) totalReg++;

                    primeraFactura = false;
                }

                dr.Close();

                //Pendiente de analizar la última factura de baja
                if (facturaBajaPendiente)
                {
                    //Chequear si existe la factura en un periodo anterior
                    bool existeFacturaPeriodoAnterior = this.ExisteFacturaPeriodoAnterior(ciafBaja, ejerBaja, periBaja, nifeBaja,
                                                                                            paisBaja, tideBaja, idoeBaja,
                                                                                            nsfeBaja, fdocBaja, cargoAbonoBaja);

                    if (existeFacturaPeriodoAnterior)
                    {
                        //Los importes de la factura de baja acumulan a los actuales
                        importeTotal += importeTotalBaja;
                        totalCuotaDeducible += totalCuotaDeducibleBaja;
                        totalBaseImponible += totalBaseImponibleBaja;
                        totalReg++;
                    }
                    else
                        try
                        {
                            this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count - 1]["SumaImporte"] = "No";
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                if (totalReg > 0)
                {
                    //Insertar la Tabla Resumen
                    row = this.dsConsultaRespuesta.Tables["Resumen"].NewRow();
                    row["NoReg"] = totalReg;
                    row["TotalImp"] = importeTotal.ToString("N2", this.LP.MyCultureInfo);
                    row["TotalBaseImponible"] = totalBaseImponible.ToString("N2", this.LP.MyCultureInfo);
                    row["TotalCuotaDeducible"] = totalCuotaDeducible.ToString("N2", this.LP.MyCultureInfo);
                    this.dsConsultaRespuesta.Tables["Resumen"].Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Verfica si existen líneas de detalle de iva (desglose factura) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleIVADesgloseFactura(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                                       string iDEmisorFactura, string iDDestinatario,
                                                       string tipoDesglose,
                                                       string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                       ref decimal totalBaseImponible,
                                                       ref decimal totalCuotaDeducible)
        {
            try
            {
                int contador = 1;
                bool existe1 = true;
                bool existe2 = false;
                bool existe3 = false;
                bool existe4 = false;
                bool existe5 = false;
                bool existe6 = false;

                decimal tipoImpositivoDec = 0;
                decimal baseImponibleDec = 0;
                decimal cuotaRepercutidaDec = 0;
                decimal cuotaRecargoEquivDec = 0;
                totalBaseImponible = 0;
                totalCuotaDeducible = 0;

                string tipoImpositivo1 = dr.GetValue(dr.GetOrdinal("TIM1F2")).ToString().Trim();
                if (tipoImpositivo1 == "") tipoImpositivo1 = "0";
                try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo1); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible1 = dr.GetValue(dr.GetOrdinal("BIM1F2")).ToString().Trim();
                if (baseImponible1 == "") baseImponible1 = "0";
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida1 = dr.GetValue(dr.GetOrdinal("CUO1F2")).ToString().Trim();
                if (cuotaRepercutida1 == "") cuotaRepercutida1 = "0";
                try
                {
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida1);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia1 = "";
                string cuotaRecargoEquivalencia1 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("TRE1F2")).ToString().Trim();
                    cuotaRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("CRE1F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia1 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia1);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                /*if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe1 = true;
                }*/

                string tipoImpositivo2 = dr.GetValue(dr.GetOrdinal("TIM2F2")).ToString().Trim();
                if (tipoImpositivo2 == "") tipoImpositivo2 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo2); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible2 = dr.GetValue(dr.GetOrdinal("BIM2F2")).ToString().Trim();
                if (baseImponible2 == "") baseImponible2 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida2 = dr.GetValue(dr.GetOrdinal("CUO2F2")).ToString().Trim();
                if (cuotaRepercutida2 == "") cuotaRepercutida2 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida2);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia2 = "";
                string cuotaRecargoEquivalencia2 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("TRE2F2")).ToString().Trim();
                    cuotaRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("CRE2F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia2 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia2);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe2 = true;
                }

                string tipoImpositivo3 = dr.GetValue(dr.GetOrdinal("TIM3F2")).ToString().Trim();
                if (tipoImpositivo3 == "") tipoImpositivo3 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo3); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible3 = dr.GetValue(dr.GetOrdinal("BIM3F2")).ToString().Trim();
                if (baseImponible3 == "") baseImponible3 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida3 = dr.GetValue(dr.GetOrdinal("CUO3F2")).ToString().Trim();
                if (cuotaRepercutida3 == "") cuotaRepercutida3 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida3);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia3 = "";
                string cuotaRecargoEquivalencia3 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("TRE3F2")).ToString().Trim();
                    cuotaRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("CRE3F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia3 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia3);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe3 = true;
                }
                string tipoImpositivo4 = dr.GetValue(dr.GetOrdinal("TIM4F2")).ToString().Trim();
                if (tipoImpositivo4 == "") tipoImpositivo4 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo4); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible4 = dr.GetValue(dr.GetOrdinal("BIM4F2")).ToString().Trim();
                if (baseImponible4 == "") baseImponible4 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida4 = dr.GetValue(dr.GetOrdinal("CUO4F2")).ToString().Trim();
                if (cuotaRepercutida4 == "") cuotaRepercutida4 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida4);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia4 = "";
                string cuotaRecargoEquivalencia4 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("TRE4F2")).ToString().Trim();
                    cuotaRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("CRE4F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia4 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia4);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe4 = true;
                }

                string tipoImpositivo5 = dr.GetValue(dr.GetOrdinal("TIM5F2")).ToString().Trim();
                if (tipoImpositivo5 == "") tipoImpositivo5 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo5); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible5 = dr.GetValue(dr.GetOrdinal("BIM5F2")).ToString().Trim();
                if (baseImponible5 == "") baseImponible5 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida5 = dr.GetValue(dr.GetOrdinal("CUO5F2")).ToString().Trim();
                if (cuotaRepercutida5 == "") cuotaRepercutida5 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida5);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia5 = "";
                string cuotaRecargoEquivalencia5 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("TRE5F2")).ToString().Trim();
                    cuotaRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("CRE5F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia5 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia5);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe5 = true;
                }

                string tipoImpositivo6 = dr.GetValue(dr.GetOrdinal("TIM6F2")).ToString().Trim();
                if (tipoImpositivo6 == "") tipoImpositivo6 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo6); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible6 = dr.GetValue(dr.GetOrdinal("BIM6F2")).ToString().Trim();
                if (baseImponible6 == "") baseImponible6 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida6 = dr.GetValue(dr.GetOrdinal("CUO6F2")).ToString().Trim();
                if (cuotaRepercutida6 == "") cuotaRepercutida6 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida6);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia6 = "";
                string cuotaRecargoEquivalencia6 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("TRE6F2")).ToString().Trim();
                    cuotaRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("CRE6F2")).ToString().Trim();
                }
                if (cuotaRecargoEquivalencia6 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia6);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe6 = true;
                }

                //if (contador > 0)
                //{
                    //--- Adicionar las lineas de IVA al DesgloseIVA
                    if (existe1) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo1, baseImponible1, cuotaRepercutida1,
                                                         tipoRecargoEquivalencia1, cuotaRecargoEquivalencia1);

                    if (existe2) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo2, baseImponible2, cuotaRepercutida2,
                                                         tipoRecargoEquivalencia2, cuotaRecargoEquivalencia2);

                    if (existe3) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo3, baseImponible3, cuotaRepercutida3,
                                                         tipoRecargoEquivalencia3, cuotaRecargoEquivalencia3);

                    if (existe4) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo4, baseImponible4, cuotaRepercutida4,
                                                         tipoRecargoEquivalencia4, cuotaRecargoEquivalencia4);

                    if (existe5) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo5, baseImponible5, cuotaRepercutida5,
                                                         tipoRecargoEquivalencia5, cuotaRecargoEquivalencia5);

                    if (existe6) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo6, baseImponible6, cuotaRepercutida6,
                                                         tipoRecargoEquivalencia6, cuotaRecargoEquivalencia6);
                //}
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona una línea de Detalle de IVA de Desglose de Factura o del desglose de tipo de operación Entrega
        /// </summary>
        /// <param name="detIVA"></param>
        /// <param name="indice"></param>
        /// <param name="tipoImpositivo"></param>
        /// <param name="baseImponible"></param>
        /// <param name="cuotaRepercutida"></param>
        /// <param name="tipoRecargoEquivalencia"></param>
        /// <param name="cuotaRecargoEquivalencia"></param>
        private void LineaDetalleIVAAdd(string compania, string ejercicio, string periodo,
                                        string iDEmisorFactura, string iDDestinatario,
                                        string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                        string tipoDesglose,
                                        string tipoImpositivo, string baseImponible, string cuotaRepercutida,
                                        string tipoRecargoEquivalencia, string cuotaRecargoEquivalencia)
        {
            try
            {
                DataRow row = this.dsConsultaRespuesta.Tables["DetalleIVA"].NewRow();

                row["Compania"] = compania;
                row["Ejercicio"] = ejercicio;
                row["Periodo"] = periodo;
                row["IDEmisorFactura"] = iDEmisorFactura;
                row["IDDestinatario"] = iDDestinatario;
                row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;
                row["TipoDesglose"] = tipoDesglose;
                row["TipoImpositivo"] = tipoImpositivo;
                row["BaseImponible"] = baseImponible;
                row["CuotaRepercutida"] = cuotaRepercutida;
                row["TipoRecargoEquivalencia"] = tipoRecargoEquivalencia;
                row["CuotaRecargoEquivalencia"] = cuotaRecargoEquivalencia;
                row["CargoAbono"] = this.cargoAbono;
                this.dsConsultaRespuesta.Tables["DetalleIVA"].Rows.Add(row);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }
        }


        /// <summary>
        /// Verfica si existen líneas de detalle de iva (desglose factura) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleIVADesglosePrestacionServicio(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                                                  string iDEmisorFactura, string iDDestinatario,
                                                                  string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                                  ref decimal totalBaseImponible,
                                                                  ref decimal totalCuotaDeducible)
        {
            try
            {
                int contador = 1;
                bool existe1 = true;
                bool existe2 = false;
                bool existe3 = false;
                bool existe4 = false;
                bool existe5 = false;
                bool existe6 = false;

                decimal tipoImpositivoDec = 0;
                decimal baseImponibleDec = 0;
                decimal cuotaRepercutidaDec = 0;
                totalBaseImponible = 0;
                totalCuotaDeducible = 0;

                string tipoImpositivo1 = dr.GetValue(dr.GetOrdinal("TIM1S2")).ToString().Trim();
                if (tipoImpositivo1 == "") tipoImpositivo1 = "0";
                try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo1); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible1 = dr.GetValue(dr.GetOrdinal("BIM1S2")).ToString().Trim();
                if (baseImponible1 == "") baseImponible1 = "0";
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida1 = dr.GetValue(dr.GetOrdinal("CUO1S2")).ToString().Trim();
                if (cuotaRepercutida1 == "") cuotaRepercutida1 = "0";
                try
                {
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida1);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

                /*if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe1 = true;
                }*/

                string tipoImpositivo2 = dr.GetValue(dr.GetOrdinal("TIM2S2")).ToString().Trim();
                if (tipoImpositivo2 == "") tipoImpositivo2 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo2); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible2 = dr.GetValue(dr.GetOrdinal("BIM2S2")).ToString().Trim();
                if (baseImponible2 == "") baseImponible2 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida2 = dr.GetValue(dr.GetOrdinal("CUO2S2")).ToString().Trim();
                if (cuotaRepercutida2 == "") cuotaRepercutida2 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida2);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe2 = true;
                }

                string tipoImpositivo3 = dr.GetValue(dr.GetOrdinal("TIM3S2")).ToString().Trim();
                if (tipoImpositivo3 == "") tipoImpositivo3 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo3); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible3 = dr.GetValue(dr.GetOrdinal("BIM3S2")).ToString().Trim();
                if (baseImponible3 == "") baseImponible3 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida3 = dr.GetValue(dr.GetOrdinal("CUO3S2")).ToString().Trim();
                if (cuotaRepercutida3 == "") cuotaRepercutida3 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida3);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe3 = true;
                }

                string tipoImpositivo4 = dr.GetValue(dr.GetOrdinal("TIM4S2")).ToString().Trim();
                if (tipoImpositivo4 == "") tipoImpositivo4 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo4); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible4 = dr.GetValue(dr.GetOrdinal("BIM4S2")).ToString().Trim();
                if (baseImponible4 == "") baseImponible4 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida4 = dr.GetValue(dr.GetOrdinal("CUO4S2")).ToString().Trim();
                if (cuotaRepercutida4 == "") cuotaRepercutida4 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida4);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe4 = true;
                }

                string tipoImpositivo5 = dr.GetValue(dr.GetOrdinal("TIM5S2")).ToString().Trim();
                if (tipoImpositivo5 == "") tipoImpositivo5 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo5); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible5 = dr.GetValue(dr.GetOrdinal("BIM5S2")).ToString().Trim();
                if (baseImponible5 == "") baseImponible5 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida5 = dr.GetValue(dr.GetOrdinal("CUO5S2")).ToString().Trim();
                if (cuotaRepercutida5 == "") cuotaRepercutida5 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida5);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe5 = true;
                }

                string tipoImpositivo6 = dr.GetValue(dr.GetOrdinal("TIM6S2")).ToString().Trim();
                if (tipoImpositivo6 == "") tipoImpositivo6 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo6); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible6 = dr.GetValue(dr.GetOrdinal("BIM6S2")).ToString().Trim();
                if (baseImponible6 == "") baseImponible6 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida6 = dr.GetValue(dr.GetOrdinal("CUO6S2")).ToString().Trim();
                if (cuotaRepercutida6 == "") cuotaRepercutida6 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida6);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe6 = true;
                }

                //if (contador > 0)
                //{
                    //--- Adicionar las lineas de IVA al DesgloseIVA
                    if (existe1) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo1, baseImponible1, cuotaRepercutida1);

                    if (existe2) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo2, baseImponible2, cuotaRepercutida2);

                    if (existe3) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo3, baseImponible3, cuotaRepercutida3);

                    if (existe4) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo4, baseImponible4, cuotaRepercutida4);

                    if (existe5) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo5, baseImponible5, cuotaRepercutida5);

                    if (existe6) this.LineaDetalleIVAAddPrestacionServicio(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                                           numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                                           tipoImpositivo6, baseImponible6, cuotaRepercutida6);
                //}
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona una línea de Detalle de IVA de Desglose de Factura de tipo de operación Prestacion de Servicio
        /// </summary>
        /// <param name="detIVA"></param>
        /// <param name="indice"></param>
        /// <param name="tipoImpositivo"></param>
        /// <param name="baseImponible"></param>
        /// <param name="cuotaRepercutida"></param>
        private void LineaDetalleIVAAddPrestacionServicio(string compania, string ejercicio, string periodo,
                                                          string iDEmisorFactura, string iDDestinatario,
                                                          string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                          string tipoImpositivo, string baseImponible, string cuotaRepercutida)
        {
            try
            {
                DataRow row = this.dsConsultaRespuesta.Tables["DetalleIVA"].NewRow();

                row["Compania"] = compania;
                row["Ejercicio"] = ejercicio;
                row["Periodo"] = periodo;
                row["IDEmisorFactura"] = iDEmisorFactura;
                row["IDDestinatario"] = iDDestinatario;
                row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;
                row["TipoDesglose"] = "P";
                row["TipoImpositivo"] = tipoImpositivo;
                row["BaseImponible"] = baseImponible;
                row["CuotaRepercutida"] = cuotaRepercutida;
                row["TipoRecargoEquivalencia"] = "";
                row["CuotaRecargoEquivalencia"] = "";
                row["CargoAbono"] = this.cargoAbono;
                this.dsConsultaRespuesta.Tables["DetalleIVA"].Rows.Add(row);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Verfica si existen líneas de detalle de iva (desglose entrega) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleIVADesgloseEntrega(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                                       string iDEmisorFactura, string iDDestinatario,
                                                       string tipoDesglose,
                                                       string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                       ref decimal totalBaseImponible,
                                                       ref decimal totalCuotaDeducible)
        {
            try
            {
                int contador = 1;
                bool existe1 = true;
                bool existe2 = false;
                bool existe3 = false;
                bool existe4 = false;
                bool existe5 = false;
                bool existe6 = false;

                decimal tipoImpositivoDec = 0;
                decimal baseImponibleDec = 0;
                decimal cuotaRepercutidaDec = 0;
                decimal cuotaRecargoEquivDec = 0;
                totalBaseImponible = 0;
                totalCuotaDeducible = 0;

                string tipoImpositivo1 = dr.GetValue(dr.GetOrdinal("TIM1E2")).ToString().Trim();
                if (tipoImpositivo1 == "") tipoImpositivo1 = "0";
                try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo1); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible1 = dr.GetValue(dr.GetOrdinal("BIM1E2")).ToString().Trim();
                if (baseImponible1 == "") baseImponible1 = "0";
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida1 = dr.GetValue(dr.GetOrdinal("CUO1E2")).ToString().Trim();
                if (cuotaRepercutida1 == "") cuotaRepercutida1 = "0";
                try
                {
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida1);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("TRE1E2")).ToString().Trim();
                string cuotaRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("CRE1E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia1 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia1);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                /*if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe1 = true;
                }*/

                string tipoImpositivo2 = dr.GetValue(dr.GetOrdinal("TIM2E2")).ToString().Trim();
                if (tipoImpositivo2 == "") tipoImpositivo2 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo2); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible2 = dr.GetValue(dr.GetOrdinal("BIM2E2")).ToString().Trim();
                if (baseImponible2 == "") baseImponible2 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida2 = dr.GetValue(dr.GetOrdinal("CUO2E2")).ToString().Trim();
                if (cuotaRepercutida2 == "") cuotaRepercutida2 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida2);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("TRE2E2")).ToString().Trim();
                string cuotaRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("CRE2E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia2 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia2);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe2 = true;
                }

                string tipoImpositivo3 = dr.GetValue(dr.GetOrdinal("TIM3E2")).ToString().Trim();
                if (tipoImpositivo3 == "") tipoImpositivo3 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo3); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible3 = dr.GetValue(dr.GetOrdinal("BIM3E2")).ToString().Trim();
                if (baseImponible3 == "") baseImponible3 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida3 = dr.GetValue(dr.GetOrdinal("CUO3E2")).ToString().Trim();
                if (cuotaRepercutida3 == "") cuotaRepercutida3 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida3);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("TRE3E2")).ToString().Trim();
                string cuotaRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("CRE3E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia3 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia3);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe3 = true;
                }

                string tipoImpositivo4 = dr.GetValue(dr.GetOrdinal("TIM4E2")).ToString().Trim();
                if (tipoImpositivo4 == "") tipoImpositivo4 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo4); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible4 = dr.GetValue(dr.GetOrdinal("BIM4E2")).ToString().Trim();
                if (baseImponible4 == "") baseImponible4 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida4 = dr.GetValue(dr.GetOrdinal("CUO4E2")).ToString().Trim();
                if (cuotaRepercutida4 == "") cuotaRepercutida4 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida4);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("TRE4E2")).ToString().Trim();
                string cuotaRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("CRE4E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia4 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia4);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe4 = true;
                }

                string tipoImpositivo5 = dr.GetValue(dr.GetOrdinal("TIM5E2")).ToString().Trim();
                if (tipoImpositivo5 == "") tipoImpositivo5 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo5); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible5 = dr.GetValue(dr.GetOrdinal("BIM5E2")).ToString().Trim();
                if (baseImponible5 == "") baseImponible5 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida5 = dr.GetValue(dr.GetOrdinal("CUO5E2")).ToString().Trim();
                if (cuotaRepercutida5 == "") cuotaRepercutida5 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida5);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("TRE5E2")).ToString().Trim();
                string cuotaRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("CRE5E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia5 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia5);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe5 = true;
                }

                string tipoImpositivo6 = dr.GetValue(dr.GetOrdinal("TIM6E2")).ToString().Trim();
                if (tipoImpositivo6 == "") tipoImpositivo6 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo6); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible6 = dr.GetValue(dr.GetOrdinal("BIM6E2")).ToString().Trim();
                if (baseImponible6 == "") baseImponible6 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaRepercutida6 = dr.GetValue(dr.GetOrdinal("CUO6E2")).ToString().Trim();
                if (cuotaRepercutida6 == "") cuotaRepercutida6 = "0";
                try
                {
                    cuotaRepercutidaDec = 0;
                    cuotaRepercutidaDec = Convert.ToDecimal(cuotaRepercutida6);
                    totalCuotaDeducible += cuotaRepercutidaDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("TRE6E2")).ToString().Trim();
                string cuotaRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("CRE6E2")).ToString().Trim();
                if (cuotaRecargoEquivalencia6 != "")
                {
                    try
                    {
                        cuotaRecargoEquivDec = Convert.ToDecimal(cuotaRecargoEquivalencia6);
                        totalCuotaDeducible += cuotaRecargoEquivDec;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaRepercutidaDec != 0)
                {
                    contador++;
                    existe6 = true;
                }

                //if (contador > 0)
                //{
                    //--- Adicionar las lineas de IVA al DesgloseIVA
                    if (existe1) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo1, baseImponible1, cuotaRepercutida1,
                                                         tipoRecargoEquivalencia1, cuotaRecargoEquivalencia1);

                    if (existe2) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo2, baseImponible2, cuotaRepercutida2,
                                                         tipoRecargoEquivalencia2, cuotaRecargoEquivalencia2);

                    if (existe3) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo3, baseImponible3, cuotaRepercutida3,
                                                         tipoRecargoEquivalencia3, cuotaRecargoEquivalencia3);

                    if (existe4) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo4, baseImponible4, cuotaRepercutida4,
                                                         tipoRecargoEquivalencia4, cuotaRecargoEquivalencia4);

                    if (existe5) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo5, baseImponible5, cuotaRepercutida5,
                                                         tipoRecargoEquivalencia5, cuotaRecargoEquivalencia5);

                    if (existe6) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                         numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                         tipoDesglose,
                                                         tipoImpositivo6, baseImponible6, cuotaRepercutida6,
                                                         tipoRecargoEquivalencia6, cuotaRecargoEquivalencia6);
                //}
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Verfica si existen líneas de detalle de iva (desglose factura) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleExenta(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                           string iDEmisorFactura, string iDDestinatario,
                                           string tipoDesglose,
                                           string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                           ref decimal totalBaseImponible, string campoBaseImponiblePrimero,
                                           string campoTermina)
        {
            try
            {
                int contador = 1;
                bool existe1 = true;
                bool existe2 = false;
                bool existe3 = false;
                bool existe4 = false;
                bool existe5 = false;
                bool existe6 = false;
                bool existe7 = false;

                string campoCausaExencionPrimero = "CEOS";
                string campoCausaExencion = "CEO";
                string campoBaseImponible = "BIN";

                decimal baseImponibleDec = 0;
                
                totalBaseImponible = 0;

                string campoCausaExencionActual = campoCausaExencionPrimero + campoTermina;      //CEOSF2
                string campoBaseImponibleActual = campoBaseImponiblePrimero + campoTermina;      //BINSF2
                string baseImponible1 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try {
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion1 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion1 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe1 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "2" + campoTermina;      //CEO2F2
                campoBaseImponibleActual = campoBaseImponible + "2" + campoTermina;      //BIN2F2
                string baseImponible2 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion2 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion2 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe2 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "3" + campoTermina;      //CEO3F2
                campoBaseImponibleActual = campoBaseImponible + "3" + campoTermina;      //BIN3F2
                string baseImponible3 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion3 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion3 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe3 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "4" + campoTermina;      //CEO4F2
                campoBaseImponibleActual = campoBaseImponible + "4" + campoTermina;      //BIN4F2
                string baseImponible4 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion4 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion4 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe4 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "5" + campoTermina;      //CEO5F2
                campoBaseImponibleActual = campoBaseImponible + "5" + campoTermina;      //BIN5F2
                string baseImponible5 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion5 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion5 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe5 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "6" + campoTermina;      //CEO6F2
                campoBaseImponibleActual = campoBaseImponible + "6" + campoTermina;      //BIN6F2
                string baseImponible6 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion6 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion6 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe6 = true;
                }

                campoCausaExencionActual = campoCausaExencion + "7" + campoTermina;      //CEO7F2
                campoBaseImponibleActual = campoBaseImponible + "7" + campoTermina;      //BIN7F2
                string baseImponible7 = dr.GetValue(dr.GetOrdinal(campoBaseImponibleActual)).ToString().Trim();
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible7);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string causaExencion7 = dr.GetValue(dr.GetOrdinal(campoCausaExencionActual)).ToString().Trim();
                if (causaExencion7 != "" || baseImponibleDec != 0)
                {
                    contador++;
                    existe7 = true;
                }

                //if (contador > 0)
                //{
                //--- Adicionar las lineas de Exenta al DetalleExenta
                if (existe1) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible1, causaExencion1);

                if (existe2) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible2, causaExencion2);

                if (existe3) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible3, causaExencion3);

                if (existe4) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible4, causaExencion4);

                if (existe5) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible5, causaExencion5);

                if (existe6) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible6, causaExencion6);

                if (existe7) this.LineaDetalleExentaAdd(compania, ejercicio, periodo, iDEmisorFactura, iDDestinatario,
                                                     numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                     tipoDesglose,
                                                     baseImponible7, causaExencion7);
                //}
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona una línea de Detalle de IVA de Desglose de Factura o del desglose de tipo de operación Entrega
        /// </summary>
        /// <param name="detIVA"></param>
        /// <param name="indice"></param>
        /// <param name="tipoImpositivo"></param>
        /// <param name="baseImponible"></param>
        /// <param name="cuotaRepercutida"></param>
        /// <param name="tipoRecargoEquivalencia"></param>
        /// <param name="cuotaRecargoEquivalencia"></param>
        private void LineaDetalleExentaAdd(string compania, string ejercicio, string periodo,
                                        string iDEmisorFactura, string iDDestinatario,
                                        string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                        string tipoDesglose,
                                        string baseImponible, string causaExencion)
        {
            try
            {
                DataRow row = this.dsConsultaRespuesta.Tables["DetalleExenta"].NewRow();

                row["Compania"] = compania;
                row["Ejercicio"] = ejercicio;
                row["Periodo"] = periodo;
                row["IDEmisorFactura"] = iDEmisorFactura;
                row["IDDestinatario"] = iDDestinatario;
                row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;
                row["TipoDesglose"] = tipoDesglose;
                row["BaseImponible"] = baseImponible;
                row["CausaExencion"] = causaExencion;
                if (causaExencion != "") row["CausaExencionDesc"] = LibroUtiles.ListaSII_Descripcion("B", causaExencion, null);
                else row["CausaExencionDesc"] = "";
                row["CargoAbono"] = this.cargoAbono;
                this.dsConsultaRespuesta.Tables["DetalleExenta"].Rows.Add(row);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Verifica si existe la factura en un periodo anterior (con el cargo/abono cambiado)
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="nif"></param>
        /// <param name="idOtroCodPais"></param>
        /// <param name="idOtroIdType"></param>
        /// <param name="idOtroId"></param>
        /// <param name="numeroSerie"></param>
        /// <param name="fechaDoc"></param>
        /// <param name="cargoAbono"></param>
        /// <returns></returns>
        private bool ExisteFacturaPeriodoAnterior(string compania, string ejercicio, string periodo, string nif,
                                                  string idOtroCodPais, string idOtroIdType, string idOtroId,
                                                  string numeroSerie, string fechaDoc, string cargoAbono)
        {
            bool result = false;
            try
            {
                string cargoAbonoCambiado = "";

                switch (cargoAbono)
                {
                    case "A":
                        cargoAbonoCambiado = "C";
                        break;
                    case "C":
                        cargoAbonoCambiado = "A";
                        break;
                }

                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVSII2 ";
                query += "where CIAFS2='" + compania + "' and NIFCS2='" + nif + "' and ";
                query += "PAICS2='" + idOtroCodPais + "' and TIDCS2='" + idOtroIdType + "' and IDOCS2='" + idOtroId + "' and ";
                query += "NSFES2='" + numeroSerie + "' and FDOCS2=" + fechaDoc + " and TPCGS2='" + cargoAbonoCambiado + "' and ";

                switch (GlobalVar.ConexionCG.TipoBaseDatos)
                {
                    case ProveedorDatos.DBTipos.SQLServer:
                        query += "EJERS2+PERIS2<='" + ejercicio + periodo + "'";
                        break;
                    default:
                        query += "EJERS2||PERIS2<='" + ejercicio + periodo + "'";
                        break;
                }

                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (registros > 0) result = true;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            return (result);
        }

        #region DataSet Respuesta Consultas de facturas emitidas
        /// <summary>
        /// Crea el DataSet para el resulta de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaFacturasEnviadas()
        {
            this.dsConsultaRespuesta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaMasInfo();
                this.DataSetCrearTablaDetalleIVA();
                this.DataSetCrearTablaDetalleExenta();
                this.DataSetCrearTablaResumen();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Crea la tabla de DatosGenerales para la respuesta de la consulta de facturas emitidas
        /// </summary>
        private void DataSetCrearTablaDatosGenerales()
        {
            DataTable dtDatosGrles = new DataTable();
            dtDatosGrles.TableName = "DatosGenerales";

            dtDatosGrles.Columns.Add("Compania", typeof(string));
            dtDatosGrles.Columns.Add("Ejercicio", typeof(string));
            dtDatosGrles.Columns.Add("Periodo", typeof(string));
            dtDatosGrles.Columns.Add("IDEmisorFactura", typeof(string));
            dtDatosGrles.Columns.Add("IDDestinatario", typeof(string));
            dtDatosGrles.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("RefExterna", typeof(string));     //NUEVO
            dtDatosGrles.Columns.Add("TipoFactura", typeof(string));
            dtDatosGrles.Columns.Add("TipoFacturaDesc", typeof(string));
            dtDatosGrles.Columns.Add("ClaveRegimenEspecialOTrascendencia", typeof(string));
            dtDatosGrles.Columns.Add("ClaveRegimenEspecialOTrascendenciaDesc", typeof(string));
            dtDatosGrles.Columns.Add("DescripcionOperacion", typeof(string));
            dtDatosGrles.Columns.Add("ImporteTotal", typeof(string));
            dtDatosGrles.Columns.Add("BaseImponible", typeof(string));
            dtDatosGrles.Columns.Add("Cuota", typeof(string));
            dtDatosGrles.Columns.Add("EstadoFactura", typeof(string));
            dtDatosGrles.Columns.Add("TimestampUltimaModificacion", typeof(string));
            dtDatosGrles.Columns.Add("CodigoErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("DescripcionErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("IDNIF", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROCodigoPais", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROIdType", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROId", typeof(string));
            dtDatosGrles.Columns.Add("CargoAbono", typeof(string));
            dtDatosGrles.Columns.Add("SumaImporte", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtDatosGrles);
        }

        /// <summary>
        /// Crea la tabla de MasInfo para la respuesta de la consulta de facturas emitidas
        /// </summary>
        private void DataSetCrearTablaMasInfo()
        {
            DataTable dtMasInfo = new DataTable();
            dtMasInfo.TableName = "MasInfo";

            dtMasInfo.Columns.Add("Compania", typeof(string));
            dtMasInfo.Columns.Add("Ejercicio", typeof(string));
            dtMasInfo.Columns.Add("Periodo", typeof(string));
            dtMasInfo.Columns.Add("IDEmisorFactura", typeof(string));
            dtMasInfo.Columns.Add("IDDestinatario", typeof(string));
            dtMasInfo.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("Cobros", typeof(string));
            dtMasInfo.Columns.Add("NIFPresentador", typeof(string));
            dtMasInfo.Columns.Add("TimestampPresentacion", typeof(string));
            dtMasInfo.Columns.Add("NumSerieFacturaEmisorResumenFin", typeof(string));
            dtMasInfo.Columns.Add("TipoRectificativa", typeof(string));
            dtMasInfo.Columns.Add("FacturasRectificadasNumSerieFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("FacturasRectificadasFechaExpedicionFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("ImporteRectificacionBaseRectificada", typeof(string));
            dtMasInfo.Columns.Add("ImporteRectificacionCuotaRectificada", typeof(string));
            dtMasInfo.Columns.Add("ImporteRectificacionCuotaRecargoRectificado", typeof(string));
            dtMasInfo.Columns.Add("FechaOperacion", typeof(string));
            dtMasInfo.Columns.Add("ClaveRegimenEspecialOTrascendenciaAdicional1", typeof(string));
            dtMasInfo.Columns.Add("ClaveRegimenEspecialOTrascendenciaAdicional1Desc", typeof(string));
            dtMasInfo.Columns.Add("ClaveRegimenEspecialOTrascendenciaAdicional2", typeof(string));
            dtMasInfo.Columns.Add("ClaveRegimenEspecialOTrascendenciaAdicional2Desc", typeof(string));
            dtMasInfo.Columns.Add("NumRegistroAcuerdoFacturacion", typeof(string));
            dtMasInfo.Columns.Add("ImporteTotal", typeof(string));
            dtMasInfo.Columns.Add("BaseImponibleACoste", typeof(string));
            dtMasInfo.Columns.Add("FacturaSimplificadaArticulos7.2_7.3", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNIF", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNombreRazonSocial", typeof(string));
            dtMasInfo.Columns.Add("RegPrevioGGEEoREDEMEoCompetencia", typeof(string));
            dtMasInfo.Columns.Add("Macrodato", typeof(string));
            dtMasInfo.Columns.Add("DatosInmuebleSituacionInmueble", typeof(string));
            dtMasInfo.Columns.Add("DatosInmuebleReferenciaCatastral", typeof(string));
            dtMasInfo.Columns.Add("ImporteTransmisionSujetoIva", typeof(string));
            dtMasInfo.Columns.Add("EmitidaPorTerceros", typeof(string));
            dtMasInfo.Columns.Add("FacturacionDispAdicionalTerceraYsextayDelMercadoOrganizadoDelGas", typeof(string));
            dtMasInfo.Columns.Add("VariosDestinatarios", typeof(string));
            dtMasInfo.Columns.Add("Cupon", typeof(string));
            dtMasInfo.Columns.Add("FacturaSinIdentifDestinatarioArticulo6.1.d", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNombreRazonSocial", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNifRepresentante", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNIF", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROCodigoPais", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROIdType", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROId", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDesgloseFactura", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFSujetaExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFSujetaNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFSujetaNoExentaTipoNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFNOSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFNOSujetaImportePorArticulos714Otros", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDFNOSujetaImporteTAIReglasLocalizacion", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDesgloseTipoOperacionPS", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSSujetaExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSSujetaNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSSujetaNoExentaTipoNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSNOSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSNOSujetaImportePorArticulos714Otros", typeof(string));
            dtMasInfo.Columns.Add("TipoDesglosePSNOSujetaImporteTAIReglasLocalizacion", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseDesgloseTipoOperacionEN", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENSujetaExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENSujetaNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENSujetaNoExentaTipoNoExenta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENNOSujeta", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENNOSujetaImportePorArticulos714Otros", typeof(string));
            dtMasInfo.Columns.Add("TipoDesgloseENNOSujetaImporteTAIReglasLocalizacion", typeof(string));
            dtMasInfo.Columns.Add("CargoAbono", typeof(string));

            if (this.agencia == "C")
            {
                //----NUEVOS
                dtMasInfo.Columns.Add("DatosArt25PagoAnticipadoArt25", typeof(string));
                dtMasInfo.Columns.Add("DatosArt25TipoBienArt25", typeof(string));
                dtMasInfo.Columns.Add("DatosArt25TipoDocumArt25", typeof(string));
                dtMasInfo.Columns.Add("DatosArt25NumeroProtocolo", typeof(string));
                dtMasInfo.Columns.Add("DatosArt25ApellidosNombreNotario", typeof(string));
            }

            this.dsConsultaRespuesta.Tables.Add(dtMasInfo);
        }

        /// <summary>
        /// Crea la tabla de DetalleIVA para la respuesta de la consulta de facturas emitidas
        /// </summary>
        private void DataSetCrearTablaDetalleIVA()
        {
            DataTable dtDetalleIVA = new DataTable();
            dtDetalleIVA.TableName = "DetalleIVA";

            dtDetalleIVA.Columns.Add("Compania", typeof(string));
            dtDetalleIVA.Columns.Add("Ejercicio", typeof(string));
            dtDetalleIVA.Columns.Add("Periodo", typeof(string));
            dtDetalleIVA.Columns.Add("IDEmisorFactura", typeof(string));
            dtDetalleIVA.Columns.Add("IDDestinatario", typeof(string));
            dtDetalleIVA.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDetalleIVA.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDetalleIVA.Columns.Add("TipoDesglose", typeof(string));
            dtDetalleIVA.Columns.Add("TipoImpositivo", typeof(string));
            dtDetalleIVA.Columns.Add("BaseImponible", typeof(string));
            dtDetalleIVA.Columns.Add("CuotaRepercutida", typeof(string));
            dtDetalleIVA.Columns.Add("TipoRecargoEquivalencia", typeof(string));
            dtDetalleIVA.Columns.Add("CuotaRecargoEquivalencia", typeof(string));
            dtDetalleIVA.Columns.Add("CargoAbono", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtDetalleIVA);
        }

        /// <summary>
        /// Crea la tabla de DetalleExenta para la respuesta de la consulta de facturas emitidas exentas
        /// </summary>
        private void DataSetCrearTablaDetalleExenta()
        {
            DataTable dtDetalleExenta = new DataTable();
            dtDetalleExenta.TableName = "DetalleExenta";

            dtDetalleExenta.Columns.Add("Compania", typeof(string));
            dtDetalleExenta.Columns.Add("Ejercicio", typeof(string));
            dtDetalleExenta.Columns.Add("Periodo", typeof(string));
            dtDetalleExenta.Columns.Add("IDEmisorFactura", typeof(string));
            dtDetalleExenta.Columns.Add("IDDestinatario", typeof(string));
            dtDetalleExenta.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDetalleExenta.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDetalleExenta.Columns.Add("TipoDesglose", typeof(string));
            dtDetalleExenta.Columns.Add("CausaExencion", typeof(string));
            dtDetalleExenta.Columns.Add("CausaExencionDesc", typeof(string));
            dtDetalleExenta.Columns.Add("BaseImponible", typeof(string));
            dtDetalleExenta.Columns.Add("CargoAbono", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtDetalleExenta);
        }

        /// <summary>
        /// Crea la tabla de Resumen para la respuesta de la consulta local de facturas emitidas
        /// </summary>
        private void DataSetCrearTablaResumen()
        {
            DataTable dtResumen = new DataTable();
            dtResumen.TableName = "Resumen";

            dtResumen.Columns.Add("NoReg", typeof(string));
            dtResumen.Columns.Add("TotalImp", typeof(string));
            dtResumen.Columns.Add("TotalBaseImponible", typeof(string));
            dtResumen.Columns.Add("TotalCuotaDeducible", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtResumen);
        }
        #endregion
    }
}