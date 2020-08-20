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
    class LibroFacturasRecibidas
    {
        private DataSet dsConsultaRespuesta;
        private ILog Log;
        protected Utiles utiles;
        protected LanguageProvider LP;
        protected string agencia;

        private string cargoAbono = "";

        private bool facturaBaja = false;

        public LibroFacturasRecibidas(ILog log, Utiles utiles, LanguageProvider lp, string agencia)
        {
            this.Log = log;
            this.utiles = utiles;
            this.LP = lp;
            this.agencia = agencia;
        }

        public DataSet ObtenerDatosFacturasRecibidas(string codigoCompania, string ejercicioCG, string periodo,
                                                    string nif_id, string codPais, string tipoIdentif,
                                                    string nombreRazon, string numSerieFactura,
                                                    string consultaFiltroFechaDesde, string consultaFiltroFechaHasta,
                                                    string consultaFiltroEstado,
                                                    string agenciaActual)
        {
            IDataReader dr = null;

            try
            {
                //Inicializar el DataTable de Resultado
                this.dsConsultaRespuesta = this.CrearDataSetResultadoConsultaFacturasRecibidas();

                string companiaActual = "";
                string ejercicioActual = "";
                string periodoActual = "";

                string iDEmisorFacturaNIF = "";
                string numSerieFacturaEmisor = "";
                string fechaExpedicionFacturaEmisor = "";

                string fechaCGStr = "";
                int fechaCG = -1;

                string tipoDesglose = "";
                string estadoActual = "";

                DataRow rowDatosGrles;
                DataRow row;

                int totalReg = 0;
                decimal importeTotal = 0;
                string  importeActual = "";
                decimal totalBaseImponible = 0;
                decimal totalBaseImponibleActual = 0;
                decimal totalCuotaDeducible = 0;
                string cuotaDeducibleActual = "";

                decimal totalBaseImponibleSujetoPasivoActual = 0;
                decimal totalBaseImponibleDesgloseIVAActual = 0;

                decimal importeTotalBaja = 0;
                decimal totalBaseImponibleBaja = 0;
                decimal totalCuotaDeducibleBaja = 0;
                bool facturaBajaPendiente = false;
                bool primeraFactura = true;
                string nifActual = "";

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (codigoCompania != "") filtro += "and CIAFS3 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS3 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS3 ='" + periodo + "' ";

                if (nif_id != "")
                {
                    if (codPais != "" || tipoIdentif != "")
                    {
                        filtro += "and IDOES3 = '" + nif_id + "' ";
                        filtroLOG += "and IDOEL1 = '" + nif_id + "' ";
                    }
                    else
                    {
                        filtro += "and NIFES3 = '" + nif_id + "' ";
                        filtroLOG += "and NIFEL1 = '" + nif_id + "' ";
                    }
                }

                if (codPais != "")
                {
                    filtro += "and PAISS3 = '" + codPais + "' ";
                    filtroLOG += "and PAISL1 = '" + codPais + "' ";
                }

                if (tipoIdentif != "")
                {
                    filtro += "and TIDES3 = '" + tipoIdentif + "' ";
                    filtroLOG += "and TIDEL1 = '" + tipoIdentif + "' ";
                }

                if (nombreRazon != "") filtro += "and NSFRS3 ='" + nombreRazon + "' ";

                if (numSerieFactura != "")
                {
                    filtro += "and NSFES3 LIKE '%" + numSerieFactura + "%' ";
                    filtroLOG += "and NSFEL1 LIKE '%" + numSerieFactura + "%' ";
                }

                if (consultaFiltroFechaDesde != "")
                {
                    //fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde), true);
                    fechaCGStr = utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde));
                    try
                    {
                        fechaCG = -1;
                        if (fechaCGStr != "") fechaCG = Convert.ToInt32(fechaCGStr);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    if (fechaCG != -1)
                    {
                        filtro += "and FDOCS3 >= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 >= " + fechaCG + " ";
                    }
                }

                if (consultaFiltroFechaHasta != "")
                {
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
                        filtro += "and FDOCS3 <= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 <= " + fechaCG + " ";
                    }
                }

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    if (consultaFiltroEstado == "B") filtro += "and BAJAS3='B' and STATS3 = 'V' ";  //Anuladas
                    else if (consultaFiltroEstado == "V") filtro += "and STATS3 = 'V' and BAJAS3 = ' ' ";    //Correctas
                    else filtro += "and STATS3 = '" + consultaFiltroEstado + "' ";  //Pendientes de envio, Aceptadas con errores e Incorrectas
                }
                
                string tablaIVSII3J = GlobalVar.PrefijoTablaCG + "IVSII3J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII3J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII3J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_FacturasRecibidas + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";
                
                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS3 = T1.TDOCL1 and NIFES3 = T1.NIFEL1 and PAISS3 = T1.PAISL1 and TIDES3 = T1.TIDEL1 and IDOES3 = T1.IDOEL1 and ";
                query += "NSFES3 = T1.NSFEL1 and FDOCS3 = T1.FDOCL1 and TPCGS3 = T1.TPCGL1 ";
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

                query += "order by CIAFS3, EJERS3, PERIS3, FDOCS3, NSFES3, TPCGS3, BAJAS3 DESC";
                
                /*
                //Obtener Consulta
                string filtro = "";
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII3 ";

                if (codigoCompania != "") filtro += "and CIAFS3 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS3 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS3 ='" + periodo + "' ";

                if (nif_id != "")
                {
                    if (codPais != "" || tipoIdentif != "")
                    {
                        filtro += "and IDOES3 = '" + nif_id + "' ";
                        if (codPais != "") filtro += "and PAISS3 = '" + codPais + "' ";
                        if (tipoIdentif != "") filtro += "and TIDES3 = '" + tipoIdentif + "' ";
                    }
                    else filtro += "and NIFES3 = '" + nif_id + "' ";
                }

                if (nombreRazon != "") filtro += "and NSFRS3 ='" + nombreRazon + "' ";
                if (numSerieFactura != "") filtro += "and NSFES3 LIKE '%" + numSerieFactura + "%' ";
                if (consultaFiltroFechaDesde != "")
                {
                    fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde), true);
                    if (fechaCG != -1) filtro += "and FDOCS3 = " + fechaCG + " ";
                }
                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T") filtro += "and STATS3 = '" + consultaFiltroEstado + "' ";

                if (filtro != "")
                {
                    if (filtro.Length > 3) filtro = filtro.Substring(3, filtro.Length - 3);
                    query += "where " + filtro;
                }

                query += "order by CIAFS3, EJERS3, PERIS3, FDOCS3, NSFES3";

                DataTable dtLogLastMov = null;
                */
                string tipoFactura = "";
                string claveRegimenEspecialOTrascendencia = "";
                string claveRegimenEspecialOTrascendenciaAdicional1 = "";
                string claveRegimenEspecialOTrascendenciaAdicional2 = "";
                string numRegistroAcuerdoFacturacion = "";
                string timestamp = "";
                string fecha = "";
                string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";

                fechaCGStr = "";
                
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

                string fechaExpedicionFacturaEmisorSII = "";

                this.facturaBaja = false;

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
   
                while (dr.Read())
                {
                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    rowDatosGrles = this.dsConsultaRespuesta.Tables["DatosGenerales"].NewRow();

                    companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS3")).ToString().Trim();
                    ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS3")).ToString().Trim();
                    periodoActual = dr.GetValue(dr.GetOrdinal("PERIS3")).ToString().Trim();

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFES3")).ToString().Trim();
                    nifActual = iDEmisorFacturaNIF;

                    if (iDEmisorFacturaNIF != "")
                    {
                        rowDatosGrles["IDNIF"] = iDEmisorFacturaNIF;
                        rowDatosGrles["IDOTROCodigoPais"] = "";
                        rowDatosGrles["IDOTROIdType"] = "";
                        rowDatosGrles["IDOTROId"] = "";

                        idOtroCodPais = " ";
                        idOtroIdType = " ";
                        idOtroId = " ";
                    }
                    else
                    {
                        rowDatosGrles["IDNIF"] = "";
                        
                        idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAISS3")).ToString().Trim();
                        if (idOtroCodPais != "")
                        {
                            rowDatosGrles["IDOTROCodigoPais"] = idOtroCodPais;
                            iDEmisorFacturaNIF += idOtroCodPais + "-";
                        }
                        rowDatosGrles["IDOTROCodigoPais"] = idOtroCodPais;

                        idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDES3")).ToString().Trim();
                        rowDatosGrles["IDOTROIdType"] = idOtroIdType;
                        idOtroId = dr.GetValue(dr.GetOrdinal("IDOES3")).ToString().Trim();
                        rowDatosGrles["IDOTROId"] = idOtroId;
                        iDEmisorFacturaNIF += idOtroIdType + "-" + idOtroId;
                    }

                    numSerieFacturaEmisor = dr.GetValue(dr.GetOrdinal("NSFES3")).ToString().Trim();
                    fechaExpedicionFacturaEmisor = dr.GetValue(dr.GetOrdinal("FDOCS3")).ToString().Trim();
                    fechaExpedicionFacturaEmisorSII = utiles.FormatoCGToFecha(fechaExpedicionFacturaEmisor).ToShortDateString();

                    importeActual = dr.GetValue(dr.GetOrdinal("IMPTS3")).ToString().Trim();
                    
                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS3")).ToString().Trim();

                    if (!primeraFactura & facturaBajaPendiente)
                    {
                        //Procesar la factura de baja anterior
                        //Si la clave de la factura actual es igual a la clave de la factura anterior (en caso de haber sido una baja), 
                        //no incrementar el contador de facturas porque se trata del cargo de una factura anulada
                        if (ciafBaja == companiaActual && ejerBaja == ejercicioActual && periBaja == periodoActual &&
                            nifeBaja == nifActual &&
                            paisBaja == idOtroCodPais && tideBaja == idOtroIdType && idoeBaja == idOtroId &&
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
                    facturaBajaValor = dr.GetValue(dr.GetOrdinal("BAJAS3")).ToString().Trim();
                    if (facturaBajaValor == "B")
                    {
                        this.facturaBaja = true;
                        totalFacturaIncrementar = false;
                        facturaBajaPendiente = true;

                        //Almacenar los campos claves de la factura
                        ciafBaja = companiaActual;
                        ejerBaja = ejercicioActual;
                        periBaja = periodoActual;
                        nifeBaja = dr.GetValue(dr.GetOrdinal("NIFES3")).ToString().Trim();
                        nsfeBaja = numSerieFacturaEmisor;
                        fdocBaja = fechaExpedicionFacturaEmisor;
                        cargoAbonoBaja = this.cargoAbono;

                        paisBaja = idOtroCodPais;
                        tideBaja = idOtroIdType;
                        idoeBaja = idOtroId;

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
                    rowDatosGrles["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    rowDatosGrles["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisorSII;

                    rowDatosGrles["RefExterna"] = dr.GetValue(dr.GetOrdinal("REXTS3")).ToString().Trim();

                    tipoFactura = dr.GetValue(dr.GetOrdinal("TFACS3")).ToString().Trim();
                    rowDatosGrles["TipoFactura"] = tipoFactura;
                    rowDatosGrles["TipoFacturaDesc"] = LibroUtiles.ListaSII_Descripcion("U", tipoFactura, null);

                    claveRegimenEspecialOTrascendencia = dr.GetValue(dr.GetOrdinal("COPSS3")).ToString().Trim();
                    rowDatosGrles["ClaveRegimenEspecialOTrascendencia"] = claveRegimenEspecialOTrascendencia;
                    rowDatosGrles["ClaveRegimenEspecialOTrascendenciaDesc"] = LibroUtiles.ListaSII_Descripcion("F", claveRegimenEspecialOTrascendencia, null);

                    rowDatosGrles["DescripcionOperacion"] = dr.GetValue(dr.GetOrdinal("DESCS3")).ToString().Trim();

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
                        if (totalFacturaIncrementar && importeActual != "") importeTotal += Convert.ToDecimal(importeActual);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }

                    estadoActual = dr.GetValue(dr.GetOrdinal("STATS3")).ToString().Trim();
                    if (this.facturaBaja && estadoActual == "V") rowDatosGrles["EstadoFactura"] = "Anulada";
                    else rowDatosGrles["EstadoFactura"] = LibroUtiles.EstadoDescripcion(estadoActual);

                    idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAISS3")).ToString().Trim();
                    idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDES3")).ToString().Trim();
                    idOtroId = dr.GetValue(dr.GetOrdinal("IDOES3")).ToString().Trim();

                    rowDatosGrles["CargoAbono"] = this.cargoAbono;
                    rowDatosGrles["SumaImporte"] = "Si";
                    
                    //------------ Coger Datos LOG -------------------
                    timestamp = "";
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString().Trim();
                    if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                    timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString());
                    rowDatosGrles["TimestampUltimaModificacion"] = timestamp;
                    rowDatosGrles["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    rowDatosGrles["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();
                    
                    /*  BUG CORREGIDO, el nif no era correcto y ya estaba instanciado anteriormente, no hacen falta las lineas sgtes  !!!!
                    rowDatosGrles["IDNIF"] = iDEmisorFacturaNIF;
                    rowDatosGrles["IDOTROCodigoPais"] = dr.GetValue(dr.GetOrdinal("PAISS3")).ToString().Trim();
                    rowDatosGrles["IDOTROIdType"] = dr.GetValue(dr.GetOrdinal("TIDES3")).ToString().Trim();
                    rowDatosGrles["IDOTROId"] = dr.GetValue(dr.GetOrdinal("IDOES3")).ToString().Trim();
                    */

                    cuotaDeducibleActual = dr.GetValue(dr.GetOrdinal("CDEDS3")).ToString().Trim();
                    rowDatosGrles["CuotaDeducible"] = cuotaDeducibleActual;

                    try
                    {
                        if (totalFacturaIncrementar && cuotaDeducibleActual != "") totalCuotaDeducible += Convert.ToDecimal(cuotaDeducibleActual);
                        else if (this.facturaBaja) totalCuotaDeducibleBaja = Convert.ToDecimal(cuotaDeducibleActual);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }

                    //----- Insertar la factura en la tabla de MasInfo ------
                    row = this.dsConsultaRespuesta.Tables["MasInfo"].NewRow();

                    row["compania"] = companiaActual;
                    row["ejercicio"] = ejercicioActual;
                    row["periodo"] = periodoActual;

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisorSII;

                    row["ContraparteNombreRazonSocial"] = dr.GetValue(dr.GetOrdinal("NRAZS3")).ToString().Trim();
                    row["ContraparteNifRepresentante"] = dr.GetValue(dr.GetOrdinal("NICRS3")).ToString().Trim();
                    row["ContraparteNIF"] = dr.GetValue(dr.GetOrdinal("NIFCS3")).ToString().Trim();
                    row["ContraparteIDOTROCodigoPais"] = dr.GetValue(dr.GetOrdinal("PAICS3")).ToString().Trim();
                    row["ContraparteIDOTROIdType"] = dr.GetValue(dr.GetOrdinal("TIDCS3")).ToString().Trim();
                    row["ContraparteIDOTROId"] = dr.GetValue(dr.GetOrdinal("IDOCS3")).ToString().Trim();

                    row["TipoNoExenta"] = dr.GetValue(dr.GetOrdinal("PAISS3")).ToString().Trim();   //FALTA modificarlo, ya no es asi !!!

                    row["NIFPresentador"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString().Trim();
                    row["TimestampPresentacion"] = timestamp;
                    
                    row["NumSerieFacturaEmisorResumenFin"] = dr.GetValue(dr.GetOrdinal("NSFRS3")).ToString().Trim();
                    row["TipoRectificativa"] = dr.GetValue(dr.GetOrdinal("TRTFS3")).ToString().Trim();
                    row["FacturasRectificadasNumSerieFacturaEmisor"] = dr.GetValue(dr.GetOrdinal("NRECS3")).ToString().Trim();

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FRECS3")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = utiles.FormatoCGToFecha(fechaCGStr).ToShortDateString();
                            else row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = "";
                        }
                        catch { row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = ""; };
                    }
                    else row["FacturasRectificadasFechaExpedicionFacturaEmisor"] = "";

                    row["ImporteRectificacionBaseRectificada"] = dr.GetValue(dr.GetOrdinal("BRECS3")).ToString().Trim();
                    row["ImporteRectificacionCuotaRectificada"] = dr.GetValue(dr.GetOrdinal("CRECS3")).ToString().Trim();
                    row["ImporteRectificacionCuotaRecargoRectificado"] = dr.GetValue(dr.GetOrdinal("CRRCS3")).ToString().Trim();

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FOPES3")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FechaOperacion"] = utiles.FormatoCGToFecha(fechaCGStr).ToShortDateString();
                            else row["FechaOperacion"] = "";
                        }
                        catch { row["FechaOperacion"] = ""; };
                    }
                    else row["FechaOperacion"] = "";

                    claveRegimenEspecialOTrascendenciaAdicional1 = dr.GetValue(dr.GetOrdinal("COP1S3")).ToString().Trim();
                    if (claveRegimenEspecialOTrascendenciaAdicional1 != "")
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1"] = claveRegimenEspecialOTrascendenciaAdicional1;
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1Desc"] = LibroUtiles.ListaSII_Descripcion("F", claveRegimenEspecialOTrascendenciaAdicional1, null);
                    }
                    else
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1"] = "";
                        row["ClaveRegimenEspecialOTrascendenciaAdicional1Desc"] = "";
                    }

                    claveRegimenEspecialOTrascendenciaAdicional2 = dr.GetValue(dr.GetOrdinal("COP2S3")).ToString().Trim();
                    if (claveRegimenEspecialOTrascendenciaAdicional2 != "")
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2"] = claveRegimenEspecialOTrascendenciaAdicional2;
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2Desc"] = LibroUtiles.ListaSII_Descripcion("F", claveRegimenEspecialOTrascendenciaAdicional2, null);
                    }
                    else
                    {
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2"] = "";
                        row["ClaveRegimenEspecialOTrascendenciaAdicional2Desc"] = "";
                    }

                    numRegistroAcuerdoFacturacion = dr.GetValue(dr.GetOrdinal("NRGAS3")).ToString().Trim();
                    if (numRegistroAcuerdoFacturacion != "") row["NumRegistroAcuerdoFacturacion"] = numRegistroAcuerdoFacturacion;
                    else row["NumRegistroAcuerdoFacturacion"] = "";

                    row["ImporteTotal"] = importeActual;

                    row["BaseImponibleACoste"] = dr.GetValue(dr.GetOrdinal("BIMCS3")).ToString().Trim();

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FRCOS3")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FechaRegistroContable"] = utiles.FormatoCGToFecha(fechaCGStr).ToShortDateString();
                            else row["FechaRegistroContable"] = "";
                        }
                        catch { row["FechaRegistroContable"] = ""; };
                    }
                    else row["FechaRegistroContable"] = "";

                    row["CargoAbono"] = this.cargoAbono;

                    row["FacturaSimplificadaArticulos7.2_7.3"] = dr.GetValue(dr.GetOrdinal("FSIMS3")).ToString().Trim();

                    string entidadSucedidaNIF = dr.GetValue(dr.GetOrdinal("NIFSS3")).ToString().Trim();
                    row["EntidadSucedidaNIF"] = entidadSucedidaNIF;
                    if (entidadSucedidaNIF != "") row["EntidadSucedidaNombreRazonSocial"] = LibroUtiles.ObtenerNombreRazonSocialCiaFiscalDadoNIF(entidadSucedidaNIF);
                    else row["EntidadSucedidaNombreRazonSocial"] = "";

                    row["RegPrevioGGEEoREDEMEoCompetencia"] = dr.GetValue(dr.GetOrdinal("RPRES3")).ToString().Trim();

                    row["Macrodato"] = dr.GetValue(dr.GetOrdinal("MACDS3")).ToString().Trim();

                    if (this.agencia == "C")
                    {
                        //Datos Articulo 25
                        string pagoAnticipadoArt25 = dr.GetValue(dr.GetOrdinal("PANTSC")).ToString().Trim();
                        row["DatosArt25PagoAnticipadoArt25"] = pagoAnticipadoArt25;
                        string tipoBienArt25 = dr.GetValue(dr.GetOrdinal("TBIESC")).ToString().Trim();
                        row["DatosArt25TipoBienArt25"] = tipoBienArt25;
                        string tipoDocumArt25 = dr.GetValue(dr.GetOrdinal("TPDCSC")).ToString().Trim();
                        row["DatosArt25TipoDocumArt25"] = tipoDocumArt25;
                        string numeroProtocolo = dr.GetValue(dr.GetOrdinal("NPROSC")).ToString().Trim();
                        row["DatosArt25NumeroProtocolo"] = numeroProtocolo;
                        string apellidosNombreNotario = dr.GetValue(dr.GetOrdinal("NOTASC")).ToString().Trim();
                        row["DatosArt25ApellidosNombreNotario"] = apellidosNombreNotario;
                    }

                    row["Pagos"] = ""; 

                    this.dsConsultaRespuesta.Tables["MasInfo"].Rows.Add(row);

                    //----- Insertar la factura en la tabla de DetallesIVA ------
                    //Tipo de desglose de la Factura
                    tipoDesglose = dr.GetValue(dr.GetOrdinal("TDSGS3")).ToString().Trim();
                    
                    totalBaseImponibleSujetoPasivoActual = 0;
                    //----- InversionSujetoPasivo : DetalleIVA  ------
                    if (tipoDesglose == "A" || tipoDesglose == "P")
                    {
                        //----- Insertar la factura en la tabla de DetallesIVA ------
                        this.InsertarDetalleIVADesgloseInversionSujetoPasivo(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, "P", numSerieFacturaEmisor, fechaExpedicionFacturaEmisorSII, ref totalBaseImponibleSujetoPasivoActual);
                    }
                    
                    totalBaseImponibleDesgloseIVAActual = 0;
                    //----- DesgloseIVA : DetalleIVA  ------
                    if (tipoDesglose == "A" || tipoDesglose == "I")
                    {

                        bool tipoImpositivoExiste = true;
                        bool cuotaSoportadaExiste = true;
                        bool cargaImpositivaImplicitaExiste = false;
                        bool cuotaRecargoMinoristaExiste = false;
                        bool reagypExiste = false;

                        if (claveRegimenEspecialOTrascendencia == "02" || claveRegimenEspecialOTrascendenciaAdicional1 == "02" || claveRegimenEspecialOTrascendenciaAdicional2 == "02")
                        {
                            tipoImpositivoExiste = false;
                            cuotaSoportadaExiste = false;
                            reagypExiste = true;
                        }
                        else
                        {
                            if (this.agencia == "C" && (claveRegimenEspecialOTrascendencia == "15" || 
                                claveRegimenEspecialOTrascendenciaAdicional1 == "15" || claveRegimenEspecialOTrascendenciaAdicional2 == "15"))
                            {
                                if (tipoFactura != "F5" && tipoFactura != "LC")
                                {
                                    cargaImpositivaImplicitaExiste = true;
                                    cuotaSoportadaExiste = false;
                                }
                                else if (tipoFactura == "F5" || tipoFactura != "LC") cuotaRecargoMinoristaExiste = true;
                            }
                        }
 
                        this.InsertarDetalleIVADesgloseIVA(ref dr, companiaActual, ejercicioActual, periodoActual, iDEmisorFacturaNIF, "I", numSerieFacturaEmisor, fechaExpedicionFacturaEmisorSII, ref totalBaseImponibleDesgloseIVAActual,
                                                           tipoImpositivoExiste, cuotaSoportadaExiste, 
                                                           cargaImpositivaImplicitaExiste, cuotaRecargoMinoristaExiste, 
                                                           reagypExiste);
                    }

                    totalBaseImponibleActual = totalBaseImponibleSujetoPasivoActual + totalBaseImponibleDesgloseIVAActual;
                    if (totalFacturaIncrementar) totalBaseImponible += totalBaseImponibleActual;
                    else if (this.facturaBaja) totalBaseImponibleBaja = totalBaseImponibleActual;

                    rowDatosGrles["BaseImponible"] = totalBaseImponibleActual.ToString("N2", this.LP.MyCultureInfo);

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
        /// Verfica si existen líneas de detalle de iva (desglose inversión sujeto pasivo) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleIVADesgloseInversionSujetoPasivo(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                                                     string iDEmisorFactura, string tipoDesglose,
                                                                     string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                                     ref decimal totalBaseImponible)
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
                decimal cuotaSoportadaDec = 0;
                totalBaseImponible = 0;

                string porcentCompensacionREAGYP = "";
                string importeCompensacionREAGYP = "";

                string tipoImpositivo1 = dr.GetValue(dr.GetOrdinal("TIM1P3")).ToString().Trim();
                if (tipoImpositivo1 == "") tipoImpositivo1 = "0";
                try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo1); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible1 = dr.GetValue(dr.GetOrdinal("BIM1P3")).ToString().Trim();
                if (baseImponible1 == "") baseImponible1 = "0";
                try { 
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada1 = dr.GetValue(dr.GetOrdinal("CUO1P3")).ToString().Trim();
                if (cuotaSoportada1 == "") cuotaSoportada1 = "0";
                try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada1); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia1 = "";
                string cuotaRecargoEquivalencia1 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("TRE1P3")).ToString().Trim();
                    cuotaRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("CRE1P3")).ToString().Trim();
                }
                /*if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe1 = true;
                }*/

                string tipoImpositivo2 = dr.GetValue(dr.GetOrdinal("TIM2P3")).ToString().Trim();
                if (tipoImpositivo2 == "") tipoImpositivo2 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo2); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible2 = dr.GetValue(dr.GetOrdinal("BIM2P3")).ToString().Trim();
                if (baseImponible2 == "") baseImponible2 = "0";
                try { 
                    baseImponibleDec = 0; 
                    baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada2 = dr.GetValue(dr.GetOrdinal("CUO2P3")).ToString().Trim();
                if (cuotaSoportada2 == "") cuotaSoportada2 = "0";
                try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada2); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia2 = "";
                string cuotaRecargoEquivalencia2 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("TRE2P3")).ToString().Trim();
                    cuotaRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("CRE2P3")).ToString().Trim();
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe2 = true;
                }

                string tipoImpositivo3 = dr.GetValue(dr.GetOrdinal("TIM3P3")).ToString().Trim();
                if (tipoImpositivo3 == "") tipoImpositivo3 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo3); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible3 = dr.GetValue(dr.GetOrdinal("BIM3P3")).ToString().Trim();
                if (baseImponible3 == "") baseImponible3 = "0";
                try { 
                    baseImponibleDec = 0; 
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada3 = dr.GetValue(dr.GetOrdinal("CUO3P3")).ToString().Trim();
                if (cuotaSoportada3 == "") cuotaSoportada1 = "0";
                try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada3); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia3 = "";
                string cuotaRecargoEquivalencia3 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("TRE3P3")).ToString().Trim();
                    cuotaRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("CRE3P3")).ToString().Trim();
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe3 = true;
                }

                string tipoImpositivo4 = dr.GetValue(dr.GetOrdinal("TIM4P3")).ToString().Trim();
                if (tipoImpositivo4 == "") tipoImpositivo4 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo4); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible4 = dr.GetValue(dr.GetOrdinal("BIM4P3")).ToString().Trim();
                if (baseImponible4 == "") baseImponible4 = "0";
                try { 
                    baseImponibleDec = 0; 
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada4 = dr.GetValue(dr.GetOrdinal("CUO4P3")).ToString().Trim();
                if (cuotaSoportada4 == "") cuotaSoportada4 = "0";
                try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada4); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia4 = "";
                string cuotaRecargoEquivalencia4 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("TRE4P3")).ToString().Trim();
                    cuotaRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("CRE4P3")).ToString().Trim();
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe4 = true;
                }

                string tipoImpositivo5 = dr.GetValue(dr.GetOrdinal("TIM5P3")).ToString().Trim();
                if (tipoImpositivo5 == "") tipoImpositivo5 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo5); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible5 = dr.GetValue(dr.GetOrdinal("BIM5P3")).ToString().Trim();
                if (baseImponible5 == "") baseImponible5 = "0";
                try { 
                    baseImponibleDec = 0; 
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada5 = dr.GetValue(dr.GetOrdinal("CUO5P3")).ToString().Trim();
                if (cuotaSoportada5 == "") cuotaSoportada5 = "0";
                try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada5); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia5 = "";
                string cuotaRecargoEquivalencia5 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("TRE5P3")).ToString().Trim();
                    cuotaRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("CRE5P3")).ToString().Trim();
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe5 = true;
                }

                string tipoImpositivo6 = dr.GetValue(dr.GetOrdinal("TIM6P3")).ToString().Trim();
                if (tipoImpositivo6 == "") tipoImpositivo6 = "0";
                try { tipoImpositivoDec = 0; tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo6); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string baseImponible6 = dr.GetValue(dr.GetOrdinal("BIM6P3")).ToString().Trim();
                if (baseImponible6 == "") baseImponible6 = "0";
                try { 
                    baseImponibleDec = 0; 
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada6 = dr.GetValue(dr.GetOrdinal("CUO6P3")).ToString().Trim();
                if (cuotaSoportada6 == "") cuotaSoportada6 = "0";
                try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada6); }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string tipoRecargoEquivalencia6 = "";
                string cuotaRecargoEquivalencia6 = "";
                if (this.agencia != "C")
                {
                    tipoRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("TRE6P3")).ToString().Trim();
                    cuotaRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("CRE6P3")).ToString().Trim();
                }
                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe6 = true;
                }

                //--- Adicionar las lineas de IVA al DesgloseIVA
                if (existe1) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo1, baseImponible1, cuotaSoportada1,
                                                        tipoRecargoEquivalencia1, cuotaRecargoEquivalencia1);

                if (existe2) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo2, baseImponible2, cuotaSoportada2,
                                                        tipoRecargoEquivalencia2, cuotaRecargoEquivalencia2);

                if (existe3) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo3, baseImponible3, cuotaSoportada3,
                                                        tipoRecargoEquivalencia3, cuotaRecargoEquivalencia3);

                if (existe4) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo4, baseImponible4, cuotaSoportada4,
                                                        tipoRecargoEquivalencia4, cuotaRecargoEquivalencia4);

                if (existe5) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo5, baseImponible5, cuotaSoportada5,
                                                        tipoRecargoEquivalencia5, cuotaRecargoEquivalencia5);

                if (existe6) this.LineaDetalleIVAAddDesgloseInversionSujetoPasivo(compania, ejercicio, periodo, iDEmisorFactura, 
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo6, baseImponible6, cuotaSoportada6,
                                                        tipoRecargoEquivalencia6, cuotaRecargoEquivalencia6);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Verfica si existen líneas de detalle de iva (desglose IVA) para la factura
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="iDDestinatario"></param>
        /// <param name="numSerieFacturaEmisor"></param>
        /// <param name="fechaExpedicionFacturaEmisor"></param>
        private void InsertarDetalleIVADesgloseIVA(ref IDataReader dr, string compania, string ejercicio, string periodo,
                                                   string iDEmisorFactura, string tipoDesglose,
                                                   string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                                   ref decimal totalBaseImponible, bool tipoImpositivoExiste, bool cuotaSoportadaExiste, bool cargaImpositivaImplicitaExiste, 
                                                   bool cuotaRecargoMinoristaExiste, bool reagypExiste)
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
                decimal cuotaSoportadaDec = 0;
                decimal cuotaRecargoMinoristaDec = 0;

                totalBaseImponible = 0;

                string tipoImpositivo1 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo1 = dr.GetValue(dr.GetOrdinal("TIM1I3")).ToString().Trim();
                    if (tipoImpositivo1 == "") tipoImpositivo1 = "0";
                        try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo1); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                string baseImponible1 = dr.GetValue(dr.GetOrdinal("BIM1I3")).ToString().Trim();
                if (baseImponible1 == "") baseImponible1 = "0";
                try
                {
                    baseImponibleDec = Convert.ToDecimal(baseImponible1);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada1 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada1 = dr.GetValue(dr.GetOrdinal("CUO1I3")).ToString().Trim();
                    if (cuotaSoportada1 == "") cuotaSoportada1 = "0";
                    try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada1); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                string cuotaRecargoMinorista1 = "";
                string tipoRecargoEquivalencia1 = "";
                string cuotaRecargoEquivalencia1 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada1 = dr.GetValue(dr.GetOrdinal("CUO1I3")).ToString().Trim();
                        if (cuotaSoportada1 == "") cuotaSoportada1 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada1); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista1 = dr.GetValue(dr.GetOrdinal("CRE1I3")).ToString().Trim();
                            if (cuotaRecargoMinorista1 == "") cuotaRecargoMinorista1 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista1); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }
                }
                else
                {
                    tipoRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("TRE1I3")).ToString().Trim();
                    cuotaRecargoEquivalencia1 = dr.GetValue(dr.GetOrdinal("CRE1I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP1 = "";
                string importeCompensacionREAGYP1 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP1 = dr.GetValue(dr.GetOrdinal("TAG1I3")).ToString().Trim();
                    importeCompensacionREAGYP1 = dr.GetValue(dr.GetOrdinal("CAG1I3")).ToString().Trim();
                }
                
                /*if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0)
                {
                    contador++;
                    existe1 = true;
                }*/

                string tipoImpositivo2 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo2 = dr.GetValue(dr.GetOrdinal("TIM2I3")).ToString().Trim();
                    if (tipoImpositivo2 == "") tipoImpositivo2 = "0";
                    try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo2); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else tipoImpositivoDec = 0;
                string baseImponible2 = dr.GetValue(dr.GetOrdinal("BIM2I3")).ToString().Trim();
                if (baseImponible2 == "") baseImponible2 = "0";
                try
                {
                    baseImponibleDec = 0; baseImponibleDec = Convert.ToDecimal(baseImponible2);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada2 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada2 = dr.GetValue(dr.GetOrdinal("CUO2I3")).ToString().Trim();
                    if (cuotaSoportada2 == "") cuotaSoportada2 = "0";
                    try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada2); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else cuotaSoportadaDec = 0;
                string cuotaRecargoMinorista2 = "";
                string tipoRecargoEquivalencia2 = "";
                string cuotaRecargoEquivalencia2 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada2 = dr.GetValue(dr.GetOrdinal("CUO2I3")).ToString().Trim();
                        if (cuotaSoportada2 == "") cuotaSoportada2 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada2); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        cuotaRecargoMinoristaDec = 0;
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista2 = dr.GetValue(dr.GetOrdinal("CRE2I3")).ToString().Trim();
                            if (cuotaRecargoMinorista2 == "") cuotaRecargoMinorista2 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista2); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else cuotaRecargoMinoristaDec = 0;
                    }
                }
                else
                {
                    cuotaRecargoMinoristaDec = 0;
                    tipoRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("TRE2I3")).ToString().Trim();
                    cuotaRecargoEquivalencia2 = dr.GetValue(dr.GetOrdinal("CRE2I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP2 = "";
                string importeCompensacionREAGYP2 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP2 = dr.GetValue(dr.GetOrdinal("TAG2I3")).ToString().Trim();
                    importeCompensacionREAGYP2 = dr.GetValue(dr.GetOrdinal("CAG2I3")).ToString().Trim();
                }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0 || cuotaRecargoMinoristaDec != 0 || reagypExiste)
                {
                    contador++;
                    existe2 = true;
                }

                string tipoImpositivo3 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo3 = dr.GetValue(dr.GetOrdinal("TIM3I3")).ToString().Trim();
                    if (tipoImpositivo3 == "") tipoImpositivo3 = "0";
                    try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo3); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else tipoImpositivoDec = 0;
                string baseImponible3 = dr.GetValue(dr.GetOrdinal("BIM3I3")).ToString().Trim();
                if (baseImponible3 == "") baseImponible3 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible3);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada3 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada3 = dr.GetValue(dr.GetOrdinal("CUO3I3")).ToString().Trim();
                    if (cuotaSoportada3 == "") cuotaSoportada3 = "0";
                    try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada3); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else cuotaSoportadaDec = 0;
                string cuotaRecargoMinorista3 = "";
                string tipoRecargoEquivalencia3 = "";
                string cuotaRecargoEquivalencia3 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada3 = dr.GetValue(dr.GetOrdinal("CUO3I3")).ToString().Trim();
                        if (cuotaSoportada3 == "") cuotaSoportada3 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada3); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        cuotaRecargoMinoristaDec = 0;
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista3 = dr.GetValue(dr.GetOrdinal("CRE3I3")).ToString().Trim();
                            if (cuotaRecargoMinorista3 == "") cuotaRecargoMinorista3 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista3); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else cuotaRecargoMinoristaDec = 0;
                    }
                }
                else
                {
                    cuotaRecargoMinoristaDec = 0;
                    tipoRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("TRE3I3")).ToString().Trim();
                    cuotaRecargoEquivalencia3 = dr.GetValue(dr.GetOrdinal("CRE3I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP3 = "";
                string importeCompensacionREAGYP3 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP3 = dr.GetValue(dr.GetOrdinal("TAG3I3")).ToString().Trim();
                    importeCompensacionREAGYP3 = dr.GetValue(dr.GetOrdinal("CAG3I3")).ToString().Trim();
                }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0 || cuotaRecargoMinoristaDec != 0 || reagypExiste)
                {
                    contador++;
                    existe3 = true;
                }

                string tipoImpositivo4 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo4 = dr.GetValue(dr.GetOrdinal("TIM4I3")).ToString().Trim();
                    if (tipoImpositivo4 == "") tipoImpositivo4 = "0";
                    try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo4); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else tipoImpositivoDec = 0;
                string baseImponible4 = dr.GetValue(dr.GetOrdinal("BIM4I3")).ToString().Trim();
                if (baseImponible4 == "") baseImponible4 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible4);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada4 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada4 = dr.GetValue(dr.GetOrdinal("CUO4I3")).ToString().Trim();
                    if (cuotaSoportada4 == "") cuotaSoportada4 = "0";
                    try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada4); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else cuotaSoportadaDec = 0;
                string cuotaRecargoMinorista4 = "";
                string tipoRecargoEquivalencia4 = "";
                string cuotaRecargoEquivalencia4 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada4 = dr.GetValue(dr.GetOrdinal("CUO4I3")).ToString().Trim();
                        if (cuotaSoportada4 == "") cuotaSoportada4 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada4); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        cuotaRecargoMinoristaDec = 0;
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista4 = dr.GetValue(dr.GetOrdinal("CRE4I3")).ToString().Trim();
                            if (cuotaRecargoMinorista4 == "") cuotaRecargoMinorista4 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista4); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else cuotaRecargoMinoristaDec = 0;
                    }
                }
                else
                {
                    cuotaRecargoMinoristaDec = 0;
                    tipoRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("TRE4I3")).ToString().Trim();
                    cuotaRecargoEquivalencia4 = dr.GetValue(dr.GetOrdinal("CRE4I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP4 = "";
                string importeCompensacionREAGYP4 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP4 = dr.GetValue(dr.GetOrdinal("TAG4I3")).ToString().Trim();
                    importeCompensacionREAGYP4 = dr.GetValue(dr.GetOrdinal("CAG4I3")).ToString().Trim();
                }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0 || cuotaRecargoMinoristaDec != 0 || reagypExiste)
                {
                    contador++;
                    existe4 = true;
                }

                string tipoImpositivo5 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo5 = dr.GetValue(dr.GetOrdinal("TIM5I3")).ToString().Trim();
                    if (tipoImpositivo5 == "") tipoImpositivo5 = "0";
                    try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo5); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else tipoImpositivoDec = 0;
                string baseImponible5 = dr.GetValue(dr.GetOrdinal("BIM5I3")).ToString().Trim();
                if (baseImponible5 == "") baseImponible5 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible5);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada5 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada5 = dr.GetValue(dr.GetOrdinal("CUO5I3")).ToString().Trim();
                    if (cuotaSoportada5 == "") cuotaSoportada5 = "0";
                    try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada5); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else cuotaSoportadaDec = 0;
                string cuotaRecargoMinorista5 = "";
                string tipoRecargoEquivalencia5 = "";
                string cuotaRecargoEquivalencia5 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada5 = dr.GetValue(dr.GetOrdinal("CUO5I3")).ToString().Trim();
                        if (cuotaSoportada5 == "") cuotaSoportada5 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada5); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        cuotaRecargoMinoristaDec = 0;
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista5 = dr.GetValue(dr.GetOrdinal("CRE5I3")).ToString().Trim();
                            if (cuotaRecargoMinorista5 == "") cuotaRecargoMinorista5 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista5); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else cuotaRecargoMinoristaDec = 0;
                    }
                }
                else
                {
                    cuotaRecargoMinoristaDec = 0;
                    tipoRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("TRE5I3")).ToString().Trim();
                    cuotaRecargoEquivalencia5 = dr.GetValue(dr.GetOrdinal("CRE5I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP5 = "";
                string importeCompensacionREAGYP5 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP5 = dr.GetValue(dr.GetOrdinal("TAG5I3")).ToString().Trim();
                    importeCompensacionREAGYP5 = dr.GetValue(dr.GetOrdinal("CAG5I3")).ToString().Trim();
                }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0 || cuotaRecargoMinoristaDec != 0 || reagypExiste)
                {
                    contador++;
                    existe5 = true;
                }

                string tipoImpositivo6 = "";
                if (tipoImpositivoExiste)
                {
                    tipoImpositivo6 = dr.GetValue(dr.GetOrdinal("TIM6I3")).ToString().Trim();
                    if (tipoImpositivo6 == "") tipoImpositivo6 = "0";
                    try { tipoImpositivoDec = Convert.ToDecimal(tipoImpositivo6); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else tipoImpositivoDec = 0;
                string baseImponible6 = dr.GetValue(dr.GetOrdinal("BIM6I3")).ToString().Trim();
                if (baseImponible6 == "") baseImponible6 = "0";
                try
                {
                    baseImponibleDec = 0;
                    baseImponibleDec = Convert.ToDecimal(baseImponible6);
                    totalBaseImponible += baseImponibleDec;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                string cuotaSoportada6 = "";
                if (cuotaSoportadaExiste)
                {
                    cuotaSoportada6 = dr.GetValue(dr.GetOrdinal("CUO6I3")).ToString().Trim();
                    if (cuotaSoportada6 == "") cuotaSoportada6 = "0";
                    try { cuotaSoportadaDec = 0; cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada6); }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else cuotaSoportadaDec = 0;
                string cuotaRecargoMinorista6 = "";
                string tipoRecargoEquivalencia6 = "";
                string cuotaRecargoEquivalencia6 = "";
                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        cuotaSoportada6 = dr.GetValue(dr.GetOrdinal("CUO6I3")).ToString().Trim();
                        if (cuotaSoportada6 == "") cuotaSoportada6 = "0";
                        try { cuotaSoportadaDec = Convert.ToDecimal(cuotaSoportada6); }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        cuotaRecargoMinoristaDec = 0;
                    }
                    else
                    {
                        if (cuotaRecargoMinoristaExiste)
                        {
                            cuotaRecargoMinorista6 = dr.GetValue(dr.GetOrdinal("CRE6I3")).ToString().Trim();
                            if (cuotaRecargoMinorista6 == "") cuotaRecargoMinorista6 = "0";
                            try { cuotaRecargoMinoristaDec = Convert.ToDecimal(cuotaRecargoMinorista6); }
                            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else cuotaRecargoMinoristaDec = 0;
                    }
                }
                else
                {
                    cuotaRecargoMinoristaDec = 0;
                    tipoRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("TRE6I3")).ToString().Trim();
                    cuotaRecargoEquivalencia6 = dr.GetValue(dr.GetOrdinal("CRE6I3")).ToString().Trim();
                }
                string porcentCompensacionREAGYP6 = "";
                string importeCompensacionREAGYP6 = "";
                if (reagypExiste)
                {
                    porcentCompensacionREAGYP6 = dr.GetValue(dr.GetOrdinal("TAG6I3")).ToString().Trim();
                    importeCompensacionREAGYP6 = dr.GetValue(dr.GetOrdinal("CAG6I3")).ToString().Trim();
                }

                if (tipoImpositivoDec != 0 || baseImponibleDec != 0 || cuotaSoportadaDec != 0 || cuotaRecargoMinoristaDec != 0 || reagypExiste)
                {
                    contador++;
                    existe6 = true;
                }

                //--- Adicionar las lineas de IVA al DesgloseIVA
                if (existe1) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo1, baseImponible1, cuotaSoportada1,
                                                        tipoRecargoEquivalencia1, cuotaRecargoEquivalencia1,
                                                        porcentCompensacionREAGYP1, importeCompensacionREAGYP1,
                                                        cuotaRecargoMinorista1,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste, 
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

                if (existe2) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo2, baseImponible2, cuotaSoportada2,
                                                        tipoRecargoEquivalencia2, cuotaRecargoEquivalencia2,
                                                        porcentCompensacionREAGYP2, importeCompensacionREAGYP2,
                                                        cuotaRecargoMinorista2,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste,
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

                if (existe3) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo3, baseImponible3, cuotaSoportada3,
                                                        tipoRecargoEquivalencia3, cuotaRecargoEquivalencia3,
                                                        porcentCompensacionREAGYP3, importeCompensacionREAGYP3,
                                                        cuotaRecargoMinorista3,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste,
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

                if (existe4) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo4, baseImponible4, cuotaSoportada4,
                                                        tipoRecargoEquivalencia4, cuotaRecargoEquivalencia4,
                                                        porcentCompensacionREAGYP4, importeCompensacionREAGYP4,
                                                        cuotaRecargoMinorista4,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste,
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

                if (existe5) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo5, baseImponible5, cuotaSoportada5,
                                                        tipoRecargoEquivalencia5, cuotaRecargoEquivalencia5,
                                                        porcentCompensacionREAGYP5, importeCompensacionREAGYP5,
                                                        cuotaRecargoMinorista5,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste,
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

                if (existe6) this.LineaDetalleIVAAdd(compania, ejercicio, periodo, iDEmisorFactura,
                                                        numSerieFacturaEmisor, fechaExpedicionFacturaEmisor,
                                                        tipoDesglose,
                                                        tipoImpositivo6, baseImponible6, cuotaSoportada6,
                                                        tipoRecargoEquivalencia6, cuotaRecargoEquivalencia6,
                                                        porcentCompensacionREAGYP6, importeCompensacionREAGYP6,
                                                        cuotaRecargoMinorista6,
                                                        tipoImpositivoExiste, cuotaSoportadaExiste, cargaImpositivaImplicitaExiste,
                                                        cuotaRecargoMinoristaExiste, reagypExiste);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona una línea de Detalle de IVA de Desglose de IVA
        /// </summary>
        /// <param name="detIVA"></param>
        /// <param name="indice"></param>
        /// <param name="tipoImpositivo"></param>
        /// <param name="baseImponible"></param>
        /// <param name="cuotaSoportada"></param>
        /// <param name="tipoRecargoEquivalencia"></param>
        /// <param name="cuotaRecargoEquivalencia"></param>
        /// <param name="porcentCompensacionREAGYP"></param>
        /// <param name="importeCompensacionREAGYP"></param>
        private void LineaDetalleIVAAdd(string compania, string ejercicio, string periodo,
                                        string iDEmisorFactura, 
                                        string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                        string tipoDesglose,
                                        string tipoImpositivo, string baseImponible, string cuotaSoportada,
                                        string tipoRecargoEquivalencia, string cuotaRecargoEquivalencia,
                                        string porcentCompensacionREAGYP, string importeCompensacionREAGYP,
                                        string cuotaRecargoMinorista,
                                        bool tipoImpositivoExiste, bool cuotaSoportadaExiste, bool cargaImpositivaImplicitaExiste,
                                        bool cuotaRecargoMinoristaExiste, bool reagypExiste)
        {
            try
            {
               DataRow row = this.dsConsultaRespuesta.Tables["DetalleIVA"].NewRow();

                row["Compania"] = compania;
                row["Ejercicio"] = ejercicio;
                row["Periodo"] = periodo;
                row["IDEmisorFactura"] = iDEmisorFactura;
                row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;
                row["TipoDesglose"] = tipoDesglose;
                if (tipoImpositivoExiste) row["TipoImpositivo"] = tipoImpositivo;
                else row["TipoImpositivo"] = "";
                row["BaseImponible"] = baseImponible;
                if (cuotaSoportadaExiste) row["CuotaSoportada"] = cuotaSoportada;
                else row["CuotaSoportada"] = "";
                row["TipoRecargoEquivalencia"] = tipoRecargoEquivalencia;
                row["CuotaRecargoEquivalencia"] = cuotaRecargoEquivalencia;
                if (reagypExiste)
                {
                    row["PorcentCompensacionREAGYP"] = porcentCompensacionREAGYP;
                    row["ImporteCompensacionREAGYP"] = importeCompensacionREAGYP;
                }
                else
                {
                    row["PorcentCompensacionREAGYP"] = "";
                    row["ImporteCompensacionREAGYP"] = "";
                }
                row["CargoAbono"] = this.cargoAbono;

                if (this.agencia == "C")
                {
                    if (cargaImpositivaImplicitaExiste)
                    {
                        row["CargaImpositivaImplicita"] = cuotaSoportada;
                        row["CuotaRecargoMinorista"] = "";
                    }
                    else if (cuotaRecargoMinoristaExiste)
                    {
                        row["CargaImpositivaImplicita"] = "";
                        row["CuotaRecargoMinorista"] = cuotaRecargoMinorista;
                    }
                    else
                    {
                        row["CargaImpositivaImplicita"] = "";
                        row["CuotaRecargoMinorista"] = "";
                    }
                }

                this.dsConsultaRespuesta.Tables["DetalleIVA"].Rows.Add(row);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona una línea de Detalle de IVA de Desglose de IVA de Inversion Sujeto Pasivo
        /// </summary>
        /// <param name="detIVA"></param>
        /// <param name="indice"></param>
        /// <param name="tipoImpositivo"></param>
        /// <param name="baseImponible"></param>
        /// <param name="cuotaSoportada"></param>
        /// <param name="tipoRecargoEquivalencia"></param>
        /// <param name="cuotaRecargoEquivalencia"></param>
        /// <param name="porcentCompensacionREAGYP"></param>
        /// <param name="importeCompensacionREAGYP"></param>
        private void LineaDetalleIVAAddDesgloseInversionSujetoPasivo(string compania, string ejercicio, string periodo,
                                        string iDEmisorFactura, 
                                        string numSerieFacturaEmisor, string fechaExpedicionFacturaEmisor,
                                        string tipoDesglose,
                                        string tipoImpositivo, string baseImponible, string cuotaSoportada,
                                        string tipoRecargoEquivalencia, string cuotaRecargoEquivalencia)
        {
            try
            {
               DataRow row = this.dsConsultaRespuesta.Tables["DetalleIVA"].NewRow();

                row["Compania"] = compania;
                row["Ejercicio"] = ejercicio;
                row["Periodo"] = periodo;
                row["IDEmisorFactura"] = iDEmisorFactura;
                row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisor;
                row["TipoDesglose"] = tipoDesglose;
                row["TipoImpositivo"] = tipoImpositivo;
                row["BaseImponible"] = baseImponible;
                row["CuotaSoportada"] = cuotaSoportada;
                row["TipoRecargoEquivalencia"] = tipoRecargoEquivalencia;
                row["CuotaRecargoEquivalencia"] = cuotaRecargoEquivalencia;
                row["PorcentCompensacionREAGYP"] = "";
                row["ImporteCompensacionREAGYP"] = "";
                row["CargoAbono"] = this.cargoAbono;

                if (this.agencia == "C")
                {
                    row["CargaImpositivaImplicita"] = "";
                    row["CuotaRecargoMinorista"] = "";
                }

                this.dsConsultaRespuesta.Tables["DetalleIVA"].Rows.Add(row);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
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

                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVSII3 ";
                query += "where CIAFS3='" + compania + "' and NIFES3='" + nif + "' and ";
                query += "PAISS3='" + idOtroCodPais + "' and TIDES3='" + idOtroIdType + "' and IDOES3='" + idOtroId + "' and ";
                query += "NSFES3='" + numeroSerie + "' and FDOCS3=" + fechaDoc + " and TPCGS3='" + cargoAbonoCambiado + "' and ";

                switch (GlobalVar.ConexionCG.TipoBaseDatos)
                {
                    case ProveedorDatos.DBTipos.SQLServer:
                        //query += "EJERS3<='" + ejercicio + "' and PERIS3<'" + periodo + "'";
                        query += "EJERS3+PERIS3<='" + ejercicio + periodo + "'";
                        break;
                    default:
                        query += "EJERS3||PERIS3<='" + ejercicio + periodo + "'";
                        break;
                }
                    
                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (registros > 0) result = true;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            return (result);
        }

        #region DataSet Respuesta Consultas de facturas recibidas
        /// <summary>
        /// Crea el DataSet para el resulta de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaFacturasRecibidas()
        {
            this.dsConsultaRespuesta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaMasInfo();
                this.DataSetCrearTablaDetalleIVA();
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
            dtDatosGrles.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("RefExterna", typeof(string));
            dtDatosGrles.Columns.Add("TipoFactura", typeof(string));
            dtDatosGrles.Columns.Add("TipoFacturaDesc", typeof(string));
            dtDatosGrles.Columns.Add("ClaveRegimenEspecialOTrascendencia", typeof(string));
            dtDatosGrles.Columns.Add("ClaveRegimenEspecialOTrascendenciaDesc", typeof(string));
            dtDatosGrles.Columns.Add("DescripcionOperacion", typeof(string));
            dtDatosGrles.Columns.Add("ImporteTotal", typeof(string));
            dtDatosGrles.Columns.Add("BaseImponible", typeof(string));
            dtDatosGrles.Columns.Add("CuotaDeducible", typeof(string));
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
            dtMasInfo.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNombreRazonSocial", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNifRepresentante", typeof(string));
            dtMasInfo.Columns.Add("ContraparteNIF", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROCodigoPais", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROIdType", typeof(string));
            dtMasInfo.Columns.Add("ContraparteIDOTROId", typeof(string));
            dtMasInfo.Columns.Add("TipoNoExenta", typeof(string));
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
            dtMasInfo.Columns.Add("FechaRegistroContable", typeof(string));
            dtMasInfo.Columns.Add("FacturaSimplificadaArticulos7.2_7.3", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNIF", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNombreRazonSocial", typeof(string));
            dtMasInfo.Columns.Add("RegPrevioGGEEoREDEMEoCompetencia", typeof(string));
            dtMasInfo.Columns.Add("Macrodato", typeof(string));
            dtMasInfo.Columns.Add("Pagos", typeof(string));
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
            dtDetalleIVA.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDetalleIVA.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDetalleIVA.Columns.Add("TipoDesglose", typeof(string));
            dtDetalleIVA.Columns.Add("TipoImpositivo", typeof(string));
            dtDetalleIVA.Columns.Add("BaseImponible", typeof(string));
            dtDetalleIVA.Columns.Add("CuotaSoportada", typeof(string));
            dtDetalleIVA.Columns.Add("TipoRecargoEquivalencia", typeof(string));
            dtDetalleIVA.Columns.Add("CuotaRecargoEquivalencia", typeof(string));
            dtDetalleIVA.Columns.Add("PorcentCompensacionREAGYP", typeof(string));
            dtDetalleIVA.Columns.Add("ImporteCompensacionREAGYP", typeof(string));
            dtDetalleIVA.Columns.Add("CargoAbono", typeof(string));

            //---NUEVOS
            if (this.agencia == "C")
            {
                dtDetalleIVA.Columns.Add("CargaImpositivaImplicita", typeof(string));
                dtDetalleIVA.Columns.Add("CuotaRecargoMinorista", typeof(string));
            }

            this.dsConsultaRespuesta.Tables.Add(dtDetalleIVA);
        }

        /// <summary>
        /// Crea la tabla de Resumen para la respuesta de la consulta local de facturas recibidas
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
