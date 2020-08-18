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
    class LibroPagoRecibidas
    {
        private DataSet dsPagosRecibidasConsulta;
        private ILog Log;
        protected Utiles utiles;
        protected LanguageProvider LP;

        private string cargoAbono = "";

        private bool facturaBaja = false;

        public LibroPagoRecibidas(ILog log, Utiles utiles, LanguageProvider lp)
        {
            this.Log = log;
            this.utiles = utiles;
            this.LP = lp;
        }

        public DataSet ObtenerDatosPagosRecibidas(string codigoCompania, string ejercicioCG, string periodo,
                                                  string nombreRazon, string nif_id, string codPais, string tipoIdentif,
                                                  string numSerieFactura, string consultaFiltroFechaDesde, string consultaFiltroFechaHasta,
                                                  string agenciaActual)
        {
            IDataReader dr = null;
            try
            {
                //Inicializar el DataTable de Resultado
                this.dsPagosRecibidasConsulta = this.CrearDataSetResultadoConsultaPagosRecibidas();

                string companiaActual = "";
                string ejercicioActual = "";
                string periodoActual = "";

                string iDEmisorFacturaNIF = "";
                //string numSerieFacturaEmisor = "";
                //string fechaExpedicionFacturaEmisor = "";

                string fechaCGStr = "";
                int fechaCG = -1;

                DataRow row;

                int totalReg = 0;
                decimal importeTotal = 0;
                string importeActual = "";

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (codigoCompania != "") filtro += "and CIAFS9 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS9 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS9 ='" + periodo + "' ";
                if (nombreRazon != "") filtro += "and NRAZS9 LIKE '%" + nombreRazon.Trim() + "%' ";
                if (numSerieFactura != "")
                {
                    filtro += "and NSFES9 LIKE '%" + numSerieFactura + "%' ";
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
                        filtro += "and FDOCS9 >= " + fechaCG + " ";
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
                        filtro += "and FDOCS9 <= " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 <= " + fechaCG + " ";
                    }
                }
                //if (consultaFiltroEstado != "" && consultaFiltroEstado != "T") filtro += "and STATS9 = '" + consultaFiltroEstado + "' ";

                string tablaIVSII9J = GlobalVar.PrefijoTablaCG + "IVSII9J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII9J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII9J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_PagosRecibidas + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS9 = T1.TDOCL1 and NIFES9 = T1.NIFEL1 and PAISS9 = T1.PAISL1 and TIDES9 = T1.TIDEL1 and IDOES9 = T1.IDOEL1 and ";
                query += "NSFES9 = T1.NSFEL1 and FDOCS9 = T1.FDOCL1 and TPCGS9 = T1.TPCGL1 ";
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

                query += "order by CIAFS9, EJERS9, PERIS9, FDOCS9, NSFES9, TPCGS9 DESC";
                
                string timestamp = "";
                string fecha = "";
                string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";
                fechaCGStr = "";
                string pagoMedio = "";

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
                //decimal importeTotalActualDec = 0;

                string fechaExpedicionFacturaEmisor = "";
                string fechaExpedicionFacturaEmisorSII = "";
                decimal importeTotalActualDec = 0;

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    //Preguntar si es una anulación o no (si es una anulación no se tratan ni los importes, ni las bases imponibles, ni las cuotas, ni se incrementa el contador de facturas) 
                    facturaBajaValor = dr.GetValue(dr.GetOrdinal("BAJAS9")).ToString().Trim();
                    if (facturaBajaValor == "B")
                    {
                        this.facturaBaja = true;
                        totalFacturaIncrementar = false;
                    }
                    else this.facturaBaja = false;

                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    row = this.dsPagosRecibidasConsulta.Tables["DatosGenerales"].NewRow();

                    companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS9")).ToString().Trim();
                    ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS9")).ToString().Trim();
                    periodoActual = dr.GetValue(dr.GetOrdinal("PERIS9")).ToString().Trim();

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFES9")).ToString().Trim();

                    if (iDEmisorFacturaNIF != "")
                    {
                        row["IDNIF"] = iDEmisorFacturaNIF;
                        row["IDOTROCodigoPais"] = "";
                        row["IDOTROIdType"] = "";
                        row["IDOTROId"] = "";

                        idOtroCodPais = "";
                        idOtroIdType = "";
                        idOtroId = "";
                    }
                    else
                    {
                        row["IDNIF"] = "";
                        
                        idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAISS9")).ToString().Trim();
                        if (idOtroCodPais != "")
                        {
                            row["IDOTROCodigoPais"] = idOtroCodPais;
                            iDEmisorFacturaNIF += idOtroCodPais + "-";
                        }                        
                        row["IDOTROCodigoPais"] = idOtroCodPais;

                        idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDES9")).ToString().Trim();
                        row["IDOTROIdType"] = idOtroIdType;
                        idOtroId = dr.GetValue(dr.GetOrdinal("IDOES9")).ToString().Trim();
                        row["IDOTROId"] = idOtroId;
                        iDEmisorFacturaNIF += idOtroIdType + "-" + idOtroId;


                        if (this.facturaBaja)
                        {
                            paisBaja = idOtroCodPais;
                            tideBaja = idOtroIdType;
                            idoeBaja = idOtroId;
                        }
                    }

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["NumSerieFacturaEmisor"] = dr.GetValue(dr.GetOrdinal("NSFES9")).ToString().Trim();

                    fechaExpedicionFacturaEmisor = dr.GetValue(dr.GetOrdinal("FDOCS9")).ToString().Trim();
                    fechaExpedicionFacturaEmisorSII = utiles.FechaToFormatoCG(fechaExpedicionFacturaEmisor).ToShortDateString();
                    row["FechaExpedicionFacturaEmisor"] = fechaExpedicionFacturaEmisorSII;

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FPAGS9")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["PagoFecha"] = utiles.FechaToFormatoCG(fechaCGStr).ToShortDateString();
                            else row["PagoFecha"] = "";
                        }
                        catch { row["PagoFecha"] = ""; };
                    }
                    else row["PagoFecha"] = "";

                    importeActual = dr.GetValue(dr.GetOrdinal("IMPTS9")).ToString().Trim();
                    
                    try
                    {
                        importeTotalActualDec = Convert.ToDecimal(importeActual);
                        row["PagoImporte"] = importeTotalActualDec.ToString("N2", this.LP.MyCultureInfo);
                    }
                    catch (Exception ex)
                    {
                        row["PagoImporte"] = importeActual;
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }

                    if (this.facturaBaja)
                    {
                        //Almacenar los campos claves de la factura
                        ciafBaja = companiaActual;
                        ejerBaja = ejercicioActual;
                        periBaja = periodoActual;
                        tdocBaja = dr.GetValue(dr.GetOrdinal("TDOCS9")).ToString().Trim();
                        //tcomBaja = dr.GetValue(dr.GetOrdinal("TCOMS5")).ToString().Trim();
                        paisBaja = row["IDOTROCodigoPais"].ToString();
                        tideBaja = row["IDOTROIdType"].ToString();
                        idoeBaja = row["IDOTROId"].ToString();
                        nifeBaja = iDEmisorFacturaNIF;
                        nsfeBaja = dr.GetValue(dr.GetOrdinal("NFSES9")).ToString().Trim();
                        fdocBaja = dr.GetValue(dr.GetOrdinal("FDOCS9")).ToString().Trim();
                    }
                    else
                    {
                        //Si la clave de la factura actual es igual a la clave de la factura anterior (en caso de haber sido una baja), 
                        //no incrementar el contador de facturas porque se trata del cargo de una factura anulada
                        if (ciafBaja == companiaActual && ejerBaja == ejercicioActual && periBaja == periodoActual && tdocBaja == dr.GetValue(dr.GetOrdinal("TDOCS3")).ToString().Trim() &&
                            nifeBaja == iDEmisorFacturaNIF &&
                            paisBaja == row["IDOTROCodigoPais"].ToString() && tideBaja == row["IDOTROIdType"].ToString() &&
                            idoeBaja == row["IDOTROId"].ToString() &&
                            nsfeBaja == dr.GetValue(dr.GetOrdinal("NFSES9")).ToString().Trim() && fdocBaja == dr.GetValue(dr.GetOrdinal("FDOCS9")).ToString().Trim())
                            totalFacturaIncrementar = false;
                        else totalFacturaIncrementar = true;

                        //Blanquear los campos claves que se almacenan en caso de ser una baja
                        ciafBaja = "";
                        ejerBaja = "";
                        periBaja = "";
                        tdocBaja = "";
                        //tcomBaja = "";
                        nifeBaja = "";
                        paisBaja = "";
                        tideBaja = "";
                        idoeBaja = "";
                        nsfeBaja = "";
                        fdocBaja = "";
                    }

                    try
                    {
                        if (totalFacturaIncrementar && importeActual != "") importeTotal += importeTotalActualDec;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }
                    
                    pagoMedio = dr.GetValue(dr.GetOrdinal("MPAGS9")).ToString().Trim();
                    row["PagoMedio"] = pagoMedio;
                    row["PagoMedioDesc"] = LibroUtiles.ListaSII_Descripcion("C", pagoMedio, null);
                    row["PagoCuentaOMedio"] = dr.GetValue(dr.GetOrdinal("CTAPS9")).ToString().Trim();

                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS9")).ToString().Trim();
                    row["CargoAbono"] = this.cargoAbono;

                    //------------ Coger Datos LOG -------------------
                    timestamp = "";
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString().Trim();
                    if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                    timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString());
                    row["TimestampPresentacion"] = timestamp;
                    row["NIFPresentador"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString().Trim();
                    
                    this.dsPagosRecibidasConsulta.Tables["DatosGenerales"].Rows.Add(row);

                    if (totalFacturaIncrementar) totalReg++;
                }

                dr.Close();

                if (totalReg > 0)
                {
                    //Insertar la Tabla Resumen
                    row = this.dsPagosRecibidasConsulta.Tables["Resumen"].NewRow();
                    row["NoReg"] = totalReg;
                    row["TotalImp"] = importeTotal.ToString("N2", this.LP.MyCultureInfo);
                    this.dsPagosRecibidasConsulta.Tables["Resumen"].Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (this.dsPagosRecibidasConsulta);
        }

        #region DataSet Respuesta Consultas de Pagos recibidas
        /// <summary>
        /// Crea el DataSet para el resultado de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaPagosRecibidas()
        {
            this.dsPagosRecibidasConsulta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaResumen();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsPagosRecibidasConsulta);
        }

        /// <summary>
        /// Crea la tabla de DatosGenerales para la respuesta de la consulta de pagos recibidas
        /// </summary>
        private void DataSetCrearTablaDatosGenerales()
        {
            DataTable dtDatosGrles = new DataTable();
            dtDatosGrles.TableName = "DatosGenerales";

            dtDatosGrles.Columns.Add("IDEmisorFactura", typeof(string));
            dtDatosGrles.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("PagoFecha", typeof(string));
            dtDatosGrles.Columns.Add("PagoImporte", typeof(string));
            dtDatosGrles.Columns.Add("PagoMedio", typeof(string));
            dtDatosGrles.Columns.Add("PagoMedioDesc", typeof(string));
            dtDatosGrles.Columns.Add("PagoCuentaOMedio", typeof(string));
            dtDatosGrles.Columns.Add("NIFPresentador", typeof(string));
            dtDatosGrles.Columns.Add("TimestampPresentacion", typeof(string));
            dtDatosGrles.Columns.Add("IDNIF", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROCodigoPais", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROIdType", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROId", typeof(string));
            dtDatosGrles.Columns.Add("CargoAbono", typeof(string));
            this.dsPagosRecibidasConsulta.Tables.Add(dtDatosGrles);
        }

        /// <summary>
        /// Crea la tabla de Resumen para la respuesta de la consulta local de pagos recibidas
        /// </summary>
        private void DataSetCrearTablaResumen()
        {
            DataTable dtResumen = new DataTable();
            dtResumen.TableName = "Resumen";

            dtResumen.Columns.Add("NoReg", typeof(string));
            dtResumen.Columns.Add("TotalImp", typeof(string));
            this.dsPagosRecibidasConsulta.Tables.Add(dtResumen);
        }
        #endregion
    }
}
