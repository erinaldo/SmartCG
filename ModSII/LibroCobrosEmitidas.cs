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
    class LibroCobrosEmitidas
    {
        private DataSet dsCobrosEmitidasConsulta;
        private ILog Log;
        protected Utiles utiles;

        private string cargoAbono = "";

        public LibroCobrosEmitidas(ILog log, Utiles utiles)
        {
            this.Log = log;
            this.utiles = utiles;
        }

        public DataSet ObtenerDatosCobrosEmitidas(string nombreRazon, 
                                                  string nif, 
                                                  string numSerieFactura, string consultaFiltroFecha, string agenciaActual)
        {
            IDataReader dr = null;
            try
            {
                //Inicializar el DataTable de Resultado
                this.dsCobrosEmitidasConsulta = this.CrearDataSetResultadoConsultaCobrosEmitidas();

                //string numSerieFacturaEmisor = "";
                //string fechaExpedicionFacturaEmisor = "";

                int fechaCG;

                DataRow row;

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (nif != "") filtro += "and NIFES8 ='" + nif + "' ";
                if (nombreRazon != "") filtro += "and NRAZS8 ='" + nombreRazon + "' ";
                if (numSerieFactura != "")
                {
                    filtro += "and NSFES8 LIKE '%" + numSerieFactura + "%' ";
                    filtroLOG += "and NSFEL1 LIKE '%" + numSerieFactura + "%' ";
                }
                if (consultaFiltroFecha != "")
                {
                    fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFecha), true);
                    if (fechaCG != -1)
                    {
                        filtro += "and FDOCS8 = " + fechaCG + " ";
                        filtroLOG += "and FDOCL1 = " + fechaCG + " ";
                    }
                }
                //if (consultaFiltroEstado != "" && consultaFiltroEstado != "T") filtro += "and STATS8 = '" + consultaFiltroEstado + "' ";

                string tablaIVSII8J = GlobalVar.PrefijoTablaCG + "IVSII8J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII8J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII8J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_CobrosEmitidas + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS8 = T1.TDOCL1 and NIFES8 = T1.NIFEL1 and PAISS8 = T1.PAISL1 and TIDES8 = T1.TIDEL1 and IDOES8 = T1.IDOEL1 and ";
                query += "NSFES8 = T1.NSFEL1 and FDOCS8 = T1.FDOCL1 and TPCGS8 = T1.TPCGL1 ";
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

                query += "order by CIAFS8, EJERS8, PERIS8, FDOCS8, NSFES8";
                
                /*
                //Obtener Consulta
                string filtro = "";
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII8 ";

                if (nif != "") filtro += "and NIFES8 ='" + nif + "' ";
                if (nombreRazon != "") filtro += "and NRAZS8 ='" + nombreRazon + "' ";
                if (numSerieFactura != "") filtro += "and NSFES8 LIKE '%" + numSerieFactura + "%' ";
                if (consultaFiltroFecha != "")
                {
                    fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFecha), true);
                    if (fechaCG != -1) filtro += "and FDOCS8 = " + fechaCG + " ";
                }
                //if (consultaFiltroEstado != "" && consultaFiltroEstado != "T") filtro += "and STATS9 = '" + consultaFiltroEstado + "' ";

                if (filtro != "")
                {
                    if (filtro.Length > 3) filtro = filtro.Substring(3, filtro.Length - 3);
                    query += "where " + filtro;
                }

                query += "order by CIAFS8, EJERS8, PERIS8, FDOCS8, NSFES8";

                DataTable dtLogLastMov = null;
                */
                string timestamp = "";
                string fecha = "";
                /*string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";*/
                string fechaCGStr = "";
                string pagoMedio = "";

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    row = this.dsCobrosEmitidasConsulta.Tables["DatosGenerales"].NewRow();

                    fechaCGStr = dr.GetValue(dr.GetOrdinal("FCOBS8")).ToString().Trim();
                    if (fechaCGStr != "")
                    {
                        try
                        {
                            fechaCG = Convert.ToInt32(fechaCGStr);
                            if (fechaCG != 0) row["CobroFecha"] = utiles.FormatoCGToFecha(fechaCGStr).ToShortDateString();
                            else row["CobroFecha"] = "";
                        }
                        catch { row["CobroFecha"] = ""; };
                    }
                    else row["CobroFecha"] = "";

                    row["CobroImporte"] = dr.GetValue(dr.GetOrdinal("IMPTS8")).ToString().Trim();
                    
                    pagoMedio = dr.GetValue(dr.GetOrdinal("MPAGS8")).ToString().Trim();
                    row["CobroMedio"] = pagoMedio;
                    row["CobroMedioDesc"] = LibroUtiles.ListaSII_Descripcion("C", pagoMedio, null);
                    row["CobroCuentaOMedio"] = dr.GetValue(dr.GetOrdinal("CTAPS8")).ToString().Trim();
                    
                    //dtDatosGrles.Columns.Add("NIFPresentador", typeof(string));
                    //dtDatosGrles.Columns.Add("TimestampPresentacion", typeof(string));

                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS8")).ToString().Trim();
                    row["CargoAbono"] = this.cargoAbono;

                    timestamp = "";
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString().Trim();
                    if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                    timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString());
                    row["TimestampPresentacion"] = timestamp;
                    row["NIFPresentador"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString().Trim();
                    
                    /*
                    //------------ Coger Datos LOG -------------------
                    timestamp = "";
                    dtLogLastMov = LibroUtiles.ObtenerUltimoMovimiento(LibroUtiles.LibroID_PagosRecibidas, nif, idOtroCodPais, idOtroIdType, idOtroId, numSerieFacturaEmisor, fechaExpedicionFacturaEmisor, this.cargoAbono);
                    if (dtLogLastMov != null)
                    {
                        fecha = dtLogLastMov.Rows[0]["DATEL1"].ToString();
                        if (fecha.Trim() != "") fecha = LibroUtiles.FormatoCGToFechaSii(fecha);
                        timestamp = fecha + " " + LibroUtiles.HoraLogFormato(dtLogLastMov.Rows[0]["TIMEL1"].ToString());
                        row["NIFPresentador"] = dtLogLastMov.Rows[0]["NIFDL1"].ToString();
                        row["TimestampPresentacion"] = timestamp;
                    }
                    else
                    {
                        row["NIFPresentador"] = "";
                        row["TimestampPresentacion"] = "";
                    }
                    */
                    this.dsCobrosEmitidasConsulta.Tables["DatosGenerales"].Rows.Add(row);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (this.dsCobrosEmitidasConsulta);
        }

        #region DataSet Respuesta Consultas de Bienes de Inversion
        /// <summary>
        /// Crea el DataSet para el resultado de la consulta con la única tabla de Resultado para cuando existe error
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoError()
        {
            this.dsCobrosEmitidasConsulta = new DataSet();
            try
            {
                this.DataSetCrearTablaErrores();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsCobrosEmitidasConsulta);
        }

        /// <summary>
        /// Crea el DataSet para el resultado de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaCobrosEmitidas()
        {
            this.dsCobrosEmitidasConsulta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaErrores();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsCobrosEmitidasConsulta);
        }

        /// <summary>
        /// Crea la tabla de DatosGenerales para la respuesta de la consulta de Bienes de Inversion
        /// </summary>
        private void DataSetCrearTablaDatosGenerales()
        {
            DataTable dtDatosGrles = new DataTable();
            dtDatosGrles.TableName = "DatosGenerales";

            //dtDatosGrles.Columns.Add("IDEmisorFactura", typeof(string));
            //dtDatosGrles.Columns.Add("NumSerieFacturaEmisor", typeof(string));
            //dtDatosGrles.Columns.Add("FechaExpedicionFacturaEmisor", typeof(string));
            dtDatosGrles.Columns.Add("CobroFecha", typeof(string));
            dtDatosGrles.Columns.Add("CobroImporte", typeof(string));
            dtDatosGrles.Columns.Add("CobroMedio", typeof(string));
            dtDatosGrles.Columns.Add("CobroMedioDesc", typeof(string));
            dtDatosGrles.Columns.Add("CobroCuentaOMedio", typeof(string));
            dtDatosGrles.Columns.Add("NIFPresentador", typeof(string));
            dtDatosGrles.Columns.Add("TimestampPresentacion", typeof(string));
            dtDatosGrles.Columns.Add("CargoAbono", typeof(string));
            this.dsCobrosEmitidasConsulta.Tables.Add(dtDatosGrles);
        }

        /// <summary>
        /// Crea la tabla de Resultado para la respuesta de la consulta de facturas emitidas que almacena los errores
        /// </summary>
        private void DataSetCrearTablaErrores()
        {
            DataTable dtErrores = new DataTable();
            dtErrores = LibroUtiles.CrearDataTableResultado();

            this.dsCobrosEmitidasConsulta.Tables.Add(dtErrores);
        }
        #endregion
    }
}
