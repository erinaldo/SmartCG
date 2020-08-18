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
    class LibroCobrosMetalico
    {
        private DataSet dsConsultaRespuesta;
        private ILog Log;
        protected Utiles utiles;
        protected LanguageProvider LP;

        private string cargoAbono = "";

        private bool facturaBaja = false;

        public LibroCobrosMetalico(ILog log, Utiles utiles,LanguageProvider lp)
        {
            this.Log = log;
            this.utiles = utiles;
            this.LP = lp;
        }

        public DataSet ObtenerDatosCobrosMetalico(string codigoCompania, string ejercicioCG, string periodo,
                                                  string nif_id, string codPais, string tipoIdentif,
                                                  string nombreRazon, string consultaFiltroEstado,
                                                  string agenciaActual)
        {
            IDataReader dr = null;
            try
            {
                //Inicializar el DataTable de Resultado
                this.dsConsultaRespuesta = this.CrearDataSetResultadoConsultaCobrosMetalico();

                string companiaActual = "";
                string ejercicioActual = "";
                string periodoActual = "";
                string estadoActual = "";

                string iDEmisorFacturaNIF = "";

                DataRow row;

                int totalReg = 0;
                decimal importeTotal = 0;
                string importeActual = "";

                //Obtener Consulta
                string filtro = "";
                string filtroLOG = "";

                if (codigoCompania != "") filtro += "and CIAFS6 = '" + codigoCompania + "' ";
                if (ejercicioCG != "") filtro += "and EJERS6 ='" + ejercicioCG + "' ";
                if (periodo != "") filtro += "and PERIS6 ='" + periodo + "' ";

                if (nif_id != "")
                {
                    if (codPais != "" || tipoIdentif != "")
                    {
                        filtro += "and IDOCS6 = '" + nif_id + "' ";
                        filtroLOG += "and IDOEL1 = '" + nif_id + "' ";
                    }
                    else
                    {
                        filtro += "and NIFCS6 = '" + nif_id + "' ";
                        filtroLOG += "and NIFEL1 = '" + nif_id + "' ";
                    }
                }

                if (codPais != "")
                {
                    filtro += "and PAICS6 = '" + codPais + "' ";
                    filtroLOG += "and PAISL1 = '" + codPais + "' ";
                }

                if (tipoIdentif != "")
                {
                    filtro += "and TIDCS6 = '" + tipoIdentif + "' ";
                    filtroLOG += "and TIDEL1 = '" + tipoIdentif + "' ";
                }

                if (nombreRazon != "") filtro += "and NSFRS6 LIKE '%" + nombreRazon + "%' ";

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    if (consultaFiltroEstado == "B") filtro += "and BAJAS6='B' and STATS6 = 'V' ";  //Anuladas
                    else if (consultaFiltroEstado == "V") filtro += "and STATS6 = 'V' and BAJAS6 = ' ' ";    //Correctas
                    else filtro += "and STATS6 = '" + consultaFiltroEstado + "' ";  //Pendientes de envio, Aceptadas con errores e Incorrectas
                }

                string tablaIVSII6J = GlobalVar.PrefijoTablaCG + "IVSII6J";
                string tablaIVLSII = GlobalVar.PrefijoTablaCG + "IVLSII";

                string query = "select " + tablaIVSII6J + ".*, LOG.DATEL1, LOG.TIMEL1, LOG.SFACL1, LOG.ERROL1, LOG.DERRL1, LOG.NIFDL1 ";
                query += "from " + tablaIVSII6J;
                query += " left join ( ";
                query += "select TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1, MAX(DATEL1 * 1000000 + TIMEL1) FECHAHORA ";
                query += "from " + tablaIVLSII + " where ";
                query += "TDOCL1='" + LibroUtiles.LibroID_CobrosMetalico + "' ";
                if (filtroLOG != "") query += filtroLOG;
                query += "group by TDOCL1, NIFEL1, PAISL1, TIDEL1, IDOEL1, NSFEL1, FDOCL1, TPCGL1 ) ";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2) query += " AS ";

                query += "T1 on ";

                query += "TDOCS6 = T1.TDOCL1 and NIFCS6 = T1.NIFEL1 and PAICS6 = T1.PAISL1 and TIDCS6 = T1.TIDEL1 and IDOCS6 = T1.IDOEL1 and ";
                query += "TPCGS6 = T1.TPCGL1 ";
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

                query += "order by CIAFS6, EJERS6, PERIS6, NRAZS6, TPCGS6 DESC";
                
                string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";

                string facturaBajaValor = "";
                string ciafBaja = "";
                string ejerBaja = "";
                string periBaja = "";
                string tdocBaja = "";
                //string tcomBaja = "";
                string nifcBaja = "";
                string paicBaja = "";
                string tidcBaja = "";
                string idocBaja = "";
                string nrazBaja = "";
                string nicrBaja = "";

                bool totalFacturaIncrementar = true;
                decimal importeTotalActualDec = 0;

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    //Preguntar si es una anulación o no (si es una anulación no se tratan ni los importes, ni las bases imponibles, ni las cuotas, ni se incrementa el contador de facturas) 
                    facturaBajaValor = dr.GetValue(dr.GetOrdinal("BAJAS6")).ToString().Trim();
                    if (facturaBajaValor == "B")
                    {
                        this.facturaBaja = true;
                        totalFacturaIncrementar = false;
                    }
                    else this.facturaBaja = false;

                    //----- Insertar la factura en la tabla de DatosGenerales ------
                    row = this.dsConsultaRespuesta.Tables["DatosGenerales"].NewRow();

                    companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS6")).ToString().Trim();
                    ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS6")).ToString().Trim();
                    periodoActual = dr.GetValue(dr.GetOrdinal("PERIS6")).ToString().Trim();

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFCS6")).ToString().Trim();

                    row["compania"] = companiaActual;
                    row["ejercicio"] = ejercicioActual;
                    row["periodo"] = periodoActual;

                    row["IDNIF"] = iDEmisorFacturaNIF;

                    //bool idOtro = false;

                    if (iDEmisorFacturaNIF == "")
                    {
                        //idOtro = true;

                        idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAICS6")).ToString().Trim();
                        row["IDOTROCodigoPais"] = idOtroCodPais;

                        if (idOtroCodPais != "") iDEmisorFacturaNIF = idOtroCodPais + "-";

                        idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDCS6")).ToString().Trim();
                        row["IDOTROIdType"] = idOtroIdType;
                        idOtroId = dr.GetValue(dr.GetOrdinal("IDOCS6")).ToString().Trim();
                        row["IDOTROId"] = idOtroId;

                        iDEmisorFacturaNIF += idOtroIdType + "-" + idOtroId;

                        if (this.facturaBaja)
                        {
                            paicBaja = idOtroCodPais;
                            tidcBaja = idOtroIdType;
                            idocBaja = idOtroId;
                        }
                    }
                    else
                    {
                        row["IDOTROCodigoPais"] = "";
                        row["IDOTROIdType"] = "";
                        row["IDOTROId"] = "";

                        idOtroCodPais = "";
                        idOtroIdType = "";
                        idOtroId = "";
                    }

                    this.cargoAbono = dr.GetValue(dr.GetOrdinal("TPCGS6")).ToString().Trim();
                    row["CargoAbono"] = this.cargoAbono;

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["ContraparteNifIdOtro"] = iDEmisorFacturaNIF;

                    row["ContraparteNombreRazon"] = dr.GetValue(dr.GetOrdinal("NRAZS6")).ToString().Trim();
                    row["ContraparteNIFRepresentante"] = dr.GetValue(dr.GetOrdinal("NICRS6")).ToString().Trim();

                    if (this.facturaBaja)
                    {
                        //Almacenar los campos claves de la factura
                        ciafBaja = companiaActual;
                        ejerBaja = ejercicioActual;
                        periBaja = periodoActual;
                        tdocBaja = dr.GetValue(dr.GetOrdinal("TDOCSA")).ToString().Trim();
                        //tcomBaja = dr.GetValue(dr.GetOrdinal("TCOMS3")).ToString().Trim();
                        nifcBaja = row["IDNIF"].ToString();
                        nrazBaja = row["ContraparteNombreRazon"].ToString();
                        nicrBaja = row["ContraparteNIFRepresentante"].ToString();
                        row["SumaImporte"] = "No";
                    }
                    else
                    {
                        //Si la clave de la factura actual es igual a la clave de la factura anterior (en caso de haber sido una baja), 
                        //no incrementar el contador de facturas porque se trata del cargo de una factura anulada
                        if (ciafBaja == companiaActual && ejerBaja == ejercicioActual && periBaja == periodoActual && tdocBaja == dr.GetValue(dr.GetOrdinal("TDOCS3")).ToString().Trim() &&
                            nifcBaja == row["IDNIF"].ToString() &&
                            paicBaja == idOtroCodPais && tidcBaja == idOtroIdType && idocBaja == idOtroId &&
                            nrazBaja == row["ContraparteNombreRazon"].ToString() && nicrBaja == row["ContraparteNIFRepresentante"].ToString())
                        {
                            totalFacturaIncrementar = false;
                            row["SumaImporte"] = "No";
                        }
                        else
                        {
                            totalFacturaIncrementar = true;
                            row["SumaImporte"] = "Si";
                        }

                        //Blanquear los campos claves que se almacenan en caso de ser una baja
                        ciafBaja = "";
                        ejerBaja = "";
                        periBaja = "";
                        tdocBaja = "";
                        //tcomBaja = "";
                        nifcBaja = "";
                        paicBaja = "";
                        tidcBaja = "";
                        idocBaja = "";
                        nrazBaja = "";
                        nicrBaja = "";
                    }

                    importeActual = dr.GetValue(dr.GetOrdinal("IMPTS6")).ToString().Trim();

                    try
                    {
                        importeTotalActualDec = Convert.ToDecimal(importeActual);
                        row["ImporteTotal"] = importeTotalActualDec.ToString("N2", this.LP.MyCultureInfo);
                    }
                    catch (Exception ex)
                    {
                        row["ImporteTotal"] = importeActual;
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

                    string entidadSucedidaNIF = dr.GetValue(dr.GetOrdinal("NIFSS6")).ToString().Trim();
                    row["EntidadSucedidaNIF"] = entidadSucedidaNIF;
                    if (entidadSucedidaNIF != "") row["EntidadSucedidaNombreRazonSocial"] = LibroUtiles.ObtenerNombreRazonSocialCiaFiscalDadoNIF(entidadSucedidaNIF);
                    else row["EntidadSucedidaNombreRazonSocial"] = "";

                    if (this.facturaBaja) row["EstadoRegistro"] = "Anulada";
                    else
                    {
                        estadoActual = dr.GetValue(dr.GetOrdinal("STATS6")).ToString().Trim();
                        row["EstadoRegistro"] = LibroUtiles.EstadoDescripcion(estadoActual);
                    }

                    //------------ Coger Datos LOG -------------------
                    row["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    row["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();
                    
                    this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Add(row);

                    if (totalFacturaIncrementar) totalReg++;
                }

                dr.Close();

                if (totalReg > 0)
                {
                    //Insertar la Tabla Resumen
                    row = this.dsConsultaRespuesta.Tables["Resumen"].NewRow();
                    row["NoReg"] = totalReg;
                    row["TotalImp"] = importeTotal.ToString("N2", this.LP.MyCultureInfo);
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

        #region DataSet Respuesta Consultas de Cobros en Metalico
        /// <summary>
        /// Crea el DataSet para el resulta de la consulta
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaCobrosMetalico()
        {
            this.dsConsultaRespuesta = new DataSet();
            try
            {
                this.DataSetCrearTablaDatosGenerales();
                this.DataSetCrearTablaResumen();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Crea la tabla de DatosGenerales para la respuesta de la consulta de cobros en metalico
        /// </summary>
        private void DataSetCrearTablaDatosGenerales()
        {
            DataTable dtDatosGrles = new DataTable();
            dtDatosGrles.TableName = "DatosGenerales";

            dtDatosGrles.Columns.Add("Compania", typeof(string));
            dtDatosGrles.Columns.Add("Ejercicio", typeof(string));
            dtDatosGrles.Columns.Add("Periodo", typeof(string));
            dtDatosGrles.Columns.Add("ContraparteNifIdOtro", typeof(string));
            dtDatosGrles.Columns.Add("ContraparteNombreRazon", typeof(string));
            dtDatosGrles.Columns.Add("ContraparteNIFRepresentante", typeof(string));
            dtDatosGrles.Columns.Add("ImporteTotal", typeof(string));
            dtDatosGrles.Columns.Add("EntidadSucedidaNIF", typeof(string));
            dtDatosGrles.Columns.Add("EntidadSucedidaNombreRazonSocial", typeof(string));
            dtDatosGrles.Columns.Add("EstadoRegistro", typeof(string));
            dtDatosGrles.Columns.Add("CodigoErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("DescripcionErrorRegistro", typeof(string));
            dtDatosGrles.Columns.Add("IDEmisorFactura", typeof(string));
            dtDatosGrles.Columns.Add("IDNIF", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROCodigoPais", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROIdType", typeof(string));
            dtDatosGrles.Columns.Add("IDOTROId", typeof(string));
            dtDatosGrles.Columns.Add("CargoAbono", typeof(string));
            dtDatosGrles.Columns.Add("SumaImporte", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtDatosGrles);
        }

        /// <summary>
        /// Crea la tabla de Resumen para la respuesta de la consulta local de cobros en metálico
        /// </summary>
        private void DataSetCrearTablaResumen()
        {
            DataTable dtResumen = new DataTable();
            dtResumen.TableName = "Resumen";

            dtResumen.Columns.Add("NoReg", typeof(string));
            dtResumen.Columns.Add("TotalImp", typeof(string));
            this.dsConsultaRespuesta.Tables.Add(dtResumen);
        }
        #endregion
    }
}
