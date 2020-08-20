using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using log4net;
using ObjectModel;

namespace ModSII
{
    class LibroBienesInversion
    {
        private DataSet dsConsultaRespuesta;
        private ILog Log;
        protected Utiles utiles;
        protected LanguageProvider LP;

        private string cargoAbono = "";

        private bool facturaBaja = false;

        public LibroBienesInversion(ILog log, Utiles utiles, LanguageProvider lp)
        {
            this.Log = log;
            this.utiles = utiles;
            this.LP = lp;
        }

        public DataSet ObtenerDatosBienesInversion(string codigoCompania, string ejercicioCG, string periodo,
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
                this.dsConsultaRespuesta = this.CrearDataSetResultadoConsultaBienesInversion();

                string companiaActual = "";
                string ejercicioActual = "";
                string periodoActual = "";

                string iDEmisorFacturaNIF = "";
                string nifActual = "";
                string numSerieFacturaEmisor = "";
                string fechaExpedicionFacturaEmisor = "";
                string estadoActual = "";

                string fechaCGStr = "";
                int fechaCG = -1;

                DataRow row;

                int totalReg = 0;

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (codigoCompania != "") filtro += "and CIAFS4 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS4 ='" + ejercicioCG + "' ";
                
                if (nif_id != "")
                {
                    if (codPais != "" || tipoIdentif != "")
                    {
                        filtro += "and IDOES4 = '" + nif_id + "' ";
                        filtroLOG += "and IDOEL1 = '" + nif_id + "' ";
                    }
                    else
                    {
                        filtro += "and NIFES4 = '" + nif_id + "' ";
                        filtroLOG += "and NIFEL1 = '" + nif_id + "' ";
                    }
                }

                if (codPais != "")
                {
                    filtro += "and PAISS4 = '" + codPais + "' ";
                    filtroLOG += "and PAISL1 = '" + codPais + "' ";
                }

                if (tipoIdentif != "")
                {
                    filtro += "and TIDES4 = '" + tipoIdentif + "' ";
                    filtroLOG += "and TIDEL1 = '" + tipoIdentif + "' ";
                }

                if (nombreRazon != "") filtro += "and NSFRS4 ='" + nombreRazon + "' ";
                if (numSerieFactura != "")
                {
                    filtro += "and NSFES4 LIKE '%" + numSerieFactura + "%' ";
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
                        filtro += "and FDOCS4 >= " + fechaCG + " ";
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
                        filtro += "and FDOCS4 <= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 <= " + fechaCG + " ";
                    }
                }
                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    if (consultaFiltroEstado == "B") filtro += "and BAJAS4='B' and STATS4 = 'V' ";  //Anuladas
                    else if (consultaFiltroEstado == "V") filtro += "and STATS4 = 'V' and BAJAS4 = ' ' ";    //Correctas
                    else filtro += "and STATS4 = '" + consultaFiltroEstado + "' ";  //Pendientes de envio, Aceptadas con errores e Incorrectas
                }

                string tablaIVSII4J = GlobalVar.PrefijoTablaCG + "IVSII4J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII4J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII4J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_BienesInversion + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS4 = T1.TDOCL1 and NIFES4 = T1.NIFEL1 and PAISS4 = T1.PAISL1 and TIDES4 = T1.TIDEL1 and IDOES4 = T1.IDOEL1 and ";
                query += "NSFES4 = T1.NSFEL1 and FDOCS4 = T1.FDOCL1 and TPCGS4 = T1.TPCGL1 ";
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

                query += "order by CIAFS4, EJERS4, PERIS4, FDOCS4, NSFES4, TPCGS4 DESC";
                
                string timestamp = "";
                string fecha = "";
                /*string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";*/
                fechaCGStr = "";

                string fechaExpedicionFacturaEmisorSII = "";

                string facturaBajaValor = "";
                string ciafBaja = "";
                string ejerBaja = "";
                string periBaja = "";
                string tdocBaja = "";
                string nifeBaja = "";
                string paisBaja = "";
                string tideBaja = "";
                string idoeBaja = "";
                string nsfeBaja = "";
                string fdocBaja = "";

                bool totalFacturaIncrementar = true;

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    //Preguntar si es una anulación o no (si es una anulación no se tratan ni los importes, ni las bases imponibles, ni las cuotas, ni se incrementa el contador de facturas) 
                    facturaBajaValor = dr.GetValue(dr.GetOrdinal("BAJAS4")).ToString().Trim();
                    if (facturaBajaValor == "B")
                    {
                        this.facturaBaja = true;
                        totalFacturaIncrementar = false;
                    }
                    else this.facturaBaja = false;

                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    row = this.dsConsultaRespuesta.Tables["DatosGenerales"].NewRow();

                    companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS4")).ToString().Trim();
                    ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS4")).ToString().Trim();
                    periodoActual = dr.GetValue(dr.GetOrdinal("PERIS4")).ToString().Trim();

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFES4")).ToString().Trim();
                    nifActual = iDEmisorFacturaNIF;
                    numSerieFacturaEmisor = dr.GetValue(dr.GetOrdinal("NSFES4")).ToString().Trim();
                    fechaExpedicionFacturaEmisor = dr.GetValue(dr.GetOrdinal("FDOCS4")).ToString().Trim();

                    row["compania"] = companiaActual;
                    row["ejercicio"] = ejercicioActual;
                    row["periodo"] = periodoActual;

                    row["IDNIF"] = nifActual;
                    row["IDOTROCodigoPais"] = dr.GetValue(dr.GetOrdinal("PAISS4")).ToString().Trim();
                    row["IDOTROIdType"] = dr.GetValue(dr.GetOrdinal("TIDES4")).ToString().Trim();
                    row["IDOTROId"] = dr.GetValue(dr.GetOrdinal("IDOES4")).ToString().Trim();

                    if (iDEmisorFacturaNIF == "")
                    {
                        if (row["IDOTROCodigoPais"].ToString() != "") iDEmisorFacturaNIF += row["IDOTROCodigoPais"] + "-";
                        iDEmisorFacturaNIF += row["IDOTROIdType"] + "-" + row["IDOTROId"];
                    }

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    fechaExpedicionFacturaEmisorSII = utiles.FormatoCGToFecha(fechaExpedicionFacturaEmisor).ToShortDateString();
                    row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisorSII;
                    row["IdentificacionBien"] = dr.GetValue(dr.GetOrdinal("IDEBS4")).ToString().Trim();

                    row["RefExterna"] = dr.GetValue(dr.GetOrdinal("REXTS4")).ToString().Trim();

                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS4")).ToString().Trim();
                    row["CargoAbono"] = this.cargoAbono;

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FINUS4")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["FechaInicioUtilizacion"] = utiles.FormatoCGToFecha(fechaCGStr).ToShortDateString();
                            else row["FechaInicioUtilizacion"] = "";
                        }
                        catch { row["FechaInicioUtilizacion"] = ""; };
                    }
                    else row["FechaInicioUtilizacion"] = "";

                    if (this.facturaBaja) row["EstadoFactura"] = "Anulada";
                    else
                    {
                        estadoActual = dr.GetValue(dr.GetOrdinal("STATS4")).ToString().Trim();
                        row["EstadoFactura"] = LibroUtiles.EstadoDescripcion(estadoActual);
                    }

                    //------------ Coger Datos LOG -------------------
                    timestamp = "";
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString().Trim();
                    if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                    timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString());
                    row["TimestampUltimaModificacion"] = timestamp;
                    row["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    row["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();

                    row["ProrrataAnualDefinitiva"] = dr.GetValue(dr.GetOrdinal("PRADS4")).ToString().Trim();
                    row["RegulacionAnualDeduccion"] = dr.GetValue(dr.GetOrdinal("REGAS4")).ToString().Trim();
                    row["RegularizacionDeduccionEfectuada"] = dr.GetValue(dr.GetOrdinal("RGDES4")).ToString().Trim();

                    this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Add(row);

                    if (this.facturaBaja)
                    {
                        //Almacenar los campos claves de la factura
                        ciafBaja = companiaActual;
                        ejerBaja = ejercicioActual;
                        periBaja = periodoActual;
                        tdocBaja = dr.GetValue(dr.GetOrdinal("TDOCS4")).ToString().Trim();
                        //tcomBaja = dr.GetValue(dr.GetOrdinal("TCOMS4")).ToString().Trim();
                        paisBaja = row["IDOTROCodigoPais"].ToString();
                        tideBaja = row["IDOTROIdType"].ToString();
                        idoeBaja = row["IDOTROId"].ToString();
                        nifeBaja = iDEmisorFacturaNIF;
                        nsfeBaja = numSerieFacturaEmisor;
                        fdocBaja = fechaExpedicionFacturaEmisor;
                    }
                    else
                    {
                        //Si la clave de la factura actual es igual a la clave de la factura anterior (en caso de haber sido una baja), 
                        //no incrementar el contador de facturas porque se trata del cargo de una factura anulada
                        if (ciafBaja == companiaActual && ejerBaja == ejercicioActual && periBaja == periodoActual && tdocBaja == dr.GetValue(dr.GetOrdinal("TDOCS3")).ToString().Trim() &&
                            nifeBaja == iDEmisorFacturaNIF &&
                            paisBaja == row["IDOTROCodigoPais"].ToString() && tideBaja == row["IDOTROIdType"].ToString() &&
                            idoeBaja == row["IDOTROId"].ToString() &&
                            nsfeBaja == numSerieFacturaEmisor && fdocBaja == fechaExpedicionFacturaEmisor)
                            totalFacturaIncrementar = false;
                        else totalFacturaIncrementar = true;

                        //Blanquear los campos claves que se almacenan en caso de ser una baja
                        ciafBaja = "";
                        ejerBaja = "";
                        periBaja = "";
                        tdocBaja = "";
                        nifeBaja = "";
                        paisBaja = "";
                        tideBaja = "";
                        idoeBaja = "";
                        nsfeBaja = "";
                        fdocBaja = "";
                    }

                    if (totalFacturaIncrementar) totalReg++;

                    //----- Insertar la factura en la tabla de MasInfo ------
                    row = this.dsConsultaRespuesta.Tables["MasInfo"].NewRow();

                    row["compania"] = companiaActual;
                    row["ejercicio"] = ejercicioActual;
                    row["periodo"] = periodoActual;

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["NumSerieFacturaEmisor"] = numSerieFacturaEmisor;
                    row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisorSII;
                    row["IdentificacionEntrega"] = dr.GetValue(dr.GetOrdinal("IDEES4")).ToString().Trim();
                    row["NIFPresentador"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString().Trim();
                    row["TimestampPresentacion"] = timestamp;
                    
                    row["CargoAbono"] = this.cargoAbono;

                    row["NumRegistroAcuerdoFacturacion"] = dr.GetValue(dr.GetOrdinal("NRAFS4")).ToString().Trim();

                    string entidadSucedidaNIF = dr.GetValue(dr.GetOrdinal("NIFSS4")).ToString().Trim();
                    row["EntidadSucedidaNIF"] = entidadSucedidaNIF;
                    if (entidadSucedidaNIF != "") row["EntidadSucedidaNombreRazonSocial"] = LibroUtiles.ObtenerNombreRazonSocialCiaFiscalDadoNIF(entidadSucedidaNIF);
                    else row["EntidadSucedidaNombreRazonSocial"] = "";

                    this.dsConsultaRespuesta.Tables["MasInfo"].Rows.Add(row);
                }

                dr.Close();

                if (totalReg > 0)
                {
                    //Insertar la Tabla Resumen
                    row = this.dsConsultaRespuesta.Tables["Resumen"].NewRow();
                    row["NoReg"] = totalReg;
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

        #region DataSet Respuesta Consultas de Bienes de Inversion
        /// <summary>
        /// Crea el DataSet para el resulta de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaBienesInversion()
        {
            this.dsConsultaRespuesta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaMasInfo();
                this.DataSetCrearTablaResumen();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Crea la tabla de DatosGenerales para la respuesta de la consulta de Bienes de Inversion
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
            dtDatosGrles.Columns.Add("IdentificacionBien", typeof(string));
            dtDatosGrles.Columns.Add("RefExterna", typeof(string));     //NUEVO
            dtDatosGrles.Columns.Add("FechaInicioUtilizacion", typeof(string));
            dtDatosGrles.Columns.Add("ProrrataAnualDefinitiva", typeof(string));
            dtDatosGrles.Columns.Add("RegulacionAnualDeduccion", typeof(string));
            dtDatosGrles.Columns.Add("RegularizacionDeduccionEfectuada", typeof(string));
            dtDatosGrles.Columns.Add("EstadoFactura", typeof(string));
            dtDatosGrles.Columns.Add("TimestampUltimaModificacion", typeof(string));
            dtDatosGrles.Columns.Add("CodigoErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("DescripcionErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("IDNIF", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROCodigoPais", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROIdType", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROId", typeof(string));
            dtDatosGrles.Columns.Add("CargoAbono", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtDatosGrles);
        }

        /// <summary>
        /// Crea la tabla de MasInfo para la respuesta de la consulta de bienes de inversión
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
            dtMasInfo.Columns.Add("IdentificacionEntrega", typeof(string));
            dtMasInfo.Columns.Add("NumRegistroAcuerdoFacturacion", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNIF", typeof(string));
            dtMasInfo.Columns.Add("EntidadSucedidaNombreRazonSocial", typeof(string));
            dtMasInfo.Columns.Add("NIFPresentador", typeof(string));
            dtMasInfo.Columns.Add("TimestampPresentacion", typeof(string));
            dtMasInfo.Columns.Add("CargoAbono", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtMasInfo);
        }

        /// <summary>
        /// Crea la tabla de Resumen para la respuesta de la consulta local de bienes de inversión
        /// </summary>
        private void DataSetCrearTablaResumen()
        {
            DataTable dtResumen = new DataTable();
            dtResumen.TableName = "Resumen";

            dtResumen.Columns.Add("NoReg", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtResumen);
        }
        #endregion
    }
}
