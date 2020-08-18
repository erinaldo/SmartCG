using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ObjectModel;
using log4net;

namespace ModComprobantes
{
    public class ComprobanteExtContable
    {
        private string _cab_compania;
        private string _cab_anoperiodo;
        private string _cab_fecha;
        private string _cab_tipo;
        private string _cab_noComprobante;
        private string _cab_noMov;
        private string _cab_periodoDesde;
        private string _cab_periodoHasta;
        private string _cab_tipoExtDefecto;

        private DataTable _det_detalles;

        private DataSet dsErrores;

        private readonly Autorizaciones aut;
        private Utiles utiles;
        private readonly UtilesCGConsultas utilesCG;
        private LanguageProvider LP;
        private readonly ILog Log;

        private string tPlan = "";
        private string nombtvGLT06 = "";

        private string cuentaAuxiliarGlobal;

        #region Properties
        public string Cab_compania
        {
            get
            {
                return (this._cab_compania);
            }
            set
            {
                this._cab_compania = value;
            }
        }

        public string Cab_anoperiodo
        {
            get
            {
                return (this._cab_anoperiodo);
            }
            set
            {
                this._cab_anoperiodo = value;
            }
        }

        public string Cab_fecha
        {
            get
            {
                return (this._cab_fecha);
            }
            set
            {
                this._cab_fecha = value;
            }
        }

        public string Cab_tipo
        {
            get
            {
                return (this._cab_tipo);
            }
            set
            {
                this._cab_tipo = value;
            }
        }

        public string Cab_noComprobante
        {
            get
            {
                return (this._cab_noComprobante);
            }
            set
            {
                this._cab_noComprobante = value;
            }
        }

        public string Cab_noMov
        {
            get
            {
                return (this._cab_noMov);
            }
            set
            {
                this._cab_noMov = value;
            }
        }

        public string Cab_periodoDesde
        {
            get
            {
                return (this._cab_periodoDesde);
            }
            set
            {
                this._cab_periodoDesde = value;
            }
        }

        public string Cab_periodoHasta
        {
            get
            {
                return (this._cab_periodoHasta);
            }
            set
            {
                this._cab_periodoHasta = value;
            }
        }

        public string Cab_TipoExtDefecto
        {
            get
            {
                return (this._cab_tipoExtDefecto);
            }
            set
            {
                this._cab_tipoExtDefecto = value;
            }
        }

        public DataTable Det_detalles
        {
            get
            {
                return (this._det_detalles);
            }
            set
            {
                this._det_detalles = value;
            }
        }

        public LanguageProvider LPValor
        {
            get
            {
                return (this.LP);
            }
            set
            {
                this.LP = value;
            }
        }

        public DataSet DSErrores
        {
            get
            {
                return (this.dsErrores);
            }
            set
            {
                this.dsErrores = value;
            }
        }

        public string CuentaAuxiliarGlobal
        {
            get
            {
                return (this.cuentaAuxiliarGlobal);
            }
            set
            {
                this.cuentaAuxiliarGlobal = value;
            }
        }
        #endregion

        public ComprobanteExtContable()
        {
            if (this.dsErrores != null && this.dsErrores.Tables.Count > 0) this.dsErrores.Tables.Clear();
            else this.CreateDataSetErrores();

            this.aut = new Autorizaciones();
            this.utiles = new Utiles();
            this.utilesCG = new UtilesCGConsultas();
            this.Log = log4net.LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Crear el DataSet que almacenará los errores 
        /// </summary>
        private void CreateDataSetErrores()
        {
            this.dsErrores = new DataSet
            {
                DataSetName = "ErroresValComp"
            };

            DataTable dt = new DataTable
            {
                TableName = "Errores"
            };

            dt.Columns.Add("Tipo", typeof(string));
            dt.Columns.Add("Linea", typeof(string));
            dt.Columns.Add("Error", typeof(string));
            dt.Columns.Add("CodiTipo", typeof(string));
            dt.Columns.Add("CtrlCelda", typeof(string));

            this.dsErrores.Tables.Add(dt);
        }

        /// <summary>
        /// Validar la cabecera del comprobante
        /// </summary>
        /// <returns></returns>
        public bool ValidarCabecera()
        {
            bool result = true;
            IDataReader dr = null;

            string statmgGLM01 = "";
            string titamgGLM01 = "";
            string felamgGLM01 = "";
            
            string saprflGLT04 = "";

            string stattvGLT06 = "";
            string coditvGLT06 = "";

            try
            {
                //compañía exista + recuperar datos
                string query = "select TIPLMG, STATMG, TITAMG, FELAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG='" + this._cab_compania + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    statmgGLM01 = dr.GetValue(dr.GetOrdinal("STATMG")).ToString();
                    titamgGLM01 = dr.GetValue(dr.GetOrdinal("TITAMG")).ToString();
                    felamgGLM01 = dr.GetValue(dr.GetOrdinal("FELAMG")).ToString();
                    tPlan = dr.GetValue(dr.GetOrdinal("TIPLMG")).ToString();
                    dr.Close();
                }
                else
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_148", "Compañía no existe"), "C", "tgTexBoxSelCompania");
                    result = false;
                    dr.Close();
                    return (result);
                }

                //------------- Compañía Inactiva  ----------------
                if (statmgGLM01 == "*")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_149", "Compañía inactiva"), "C", "tgTexBoxSelCompania");
                    result = false;
                }
                
                //------------- Autorización Compañía  ----------------
                if (this._cab_compania.Trim() != "")
                {
                    if (!this.aut.Validar("002", "02", this._cab_compania.Trim(), "20"))
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + this._cab_compania, "C", "tgTexBoxSelCompania");
                        result = false;
                    }
                }
                
                //------------- Periodo Incorrecto  ----------------               
                //Buscar el siglo dado el año
                string aa = this._cab_anoperiodo.Substring(0, 2);
                saprflGLT04 = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this._cab_anoperiodo;

                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titamgGLM01 + "' and SAPRFL = " + saprflGLT04;
                
                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (registros == 0)
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_150", "Periodo incorrecto"), "C", "txtMaskAAPP");
                    result = false;
                }

                //------------- Tipo de Comprobante Exista  ---------------- 
                query = "select STATTV, CODITV, NOMBTV from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where TIVOTV = '" + this._cab_tipo + "'";
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    stattvGLT06 = dr.GetValue(dr.GetOrdinal("STATTV")).ToString();
                    coditvGLT06 = dr.GetValue(dr.GetOrdinal("CODITV")).ToString();
                    nombtvGLT06 = dr.GetValue(dr.GetOrdinal("NOMBTV")).ToString();
                }
                else
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_151", "Tipo de comprobante no existe"), "C", "tgTexBoxSelTipo");
                    result = false;
                }
                dr.Close();

                //------------- Tipo de Comprobante Invalido para Batch  ---------------- 
                if (coditvGLT06 != "1")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_152", "Tipo de comprobante inválido para entrada batch"), "C", "tgTexBoxSelTipo");
                    result = false;
                }
                
                //------------- Tipo de Comprobante Inactivo  ---------------- 
                if (stattvGLT06 == "*")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_153", "Tipo de comprobante inactivo"), "C", "tgTexBoxSelTipo");
                    result = false;
                }

                //------------- Autorización Tipo de Comprobante  ----------------
                if (this._cab_tipo.Trim() != "")
                {
                    if (!this.aut.Validar("004", "03", this._cab_tipo.Trim(), "20"))
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + this._cab_tipo, "C", "tgTexBoxSelTipo");
                        result = false;
                    }
                }

                //------------- Fecha de Comprobante  ----------------
                int fechaflGLT04 = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this._cab_fecha), true);    //FALTA repasar funcion que devuelve mal la fecha !!!
                //int fechaflGLT04 = 1201201;
                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titamgGLM01 + "' and ";
                query += "SAPRFL = " + saprflGLT04 + " and INLAFL <= " + fechaflGLT04;
                query += " and FINLFL >= " + fechaflGLT04;

                registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (registros == 0 && felamgGLM01 == "T") 
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_154", "Fecha de comprobante incorrecta para el periodo"), "C", "dateTimePickerFecha");
                    result = false;
                }
            }
            catch(Exception ex) 
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                this.DSErroresAdd(-1, ex.Message, "C", "");
                result = false;
            }

            return (result);
        }

        /// <summary>
        /// Validar una línea del detalle del comprobante
        /// </summary>
        /// <param name="indice">Indice de linea a validar del dataSet de la clase de ComprobanteContable</param>
        /// <param name="linea">Línea del comprobante</param>
        /// <returns></returns>
        public bool ValidarDetalle(int indice, int linea)
        {
            bool result = true;
            IDataReader dr = null;

            string statmcGLM03 = "";
            string tcuemcGLM03 = "";
            string sasimcGLM03 = "";
            string tau1mcGLM03 = "";
            string tau2mcGLM03 = "";
            string tau3mcGLM03 = "";
            string grupmcGLM03 = "";
            string auxTipoDH = "";

            //Valores Línea de Detalles del Comprobante
            string cuenta = "";
            string auxiliar1 = "";
            string auxiliar2 = "";
            string auxiliar3 = "";
            string dH = "";
            string tipoExt = "";
            string descripcion = "";

            try
            {
                DataRow row = this._det_detalles.Rows[indice];

                cuenta = row["Cuenta"].ToString();
                auxiliar1 = row["Auxiliar1"].ToString().ToUpper();
                auxiliar2 = row["Auxiliar2"].ToString().ToUpper();
                auxiliar3 = row["Auxiliar3"].ToString().ToUpper();
                dH = row["DH"].ToString().ToUpper();
                tipoExt = row["TipoExt"].ToString().ToUpper();
                descripcion = row["Descripcion"].ToString();

                //periodos ??
                if (cuenta.Trim() == "" && auxiliar1.Trim() == "" && auxiliar2.Trim() == "" && auxiliar3.Trim() == "" &&
                    dH.Trim() == "" && tipoExt.Trim() == "" && descripcion.Trim() == "")
                {
                    //Línea en blanco
                    return (result);
                }

                //------------- Cuenta de Mayor exista + recuperar datos  ----------------
                string query = "select STATMC, TCUEMC, ADICMC, SASIMC, ";
                query += "FEVEMC, NDDOMC, TERMMC, TIMOMC, TAU1MC, TAU2MC, TAU3MC, ";
                query += "TDOCMC, GRUPMC, RNITMC, TERMMC ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where CUENMC = '" + cuenta + "' and TIPLMC = '" + tPlan + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    statmcGLM03 = dr.GetValue(dr.GetOrdinal("STATMC")).ToString();
                    tcuemcGLM03 = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString();
                    sasimcGLM03 = dr.GetValue(dr.GetOrdinal("SASIMC")).ToString();
                    tau1mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString();
                    tau2mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString();
                    tau3mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString();
                    grupmcGLM03 = dr.GetValue(dr.GetOrdinal("GRUPMC")).ToString();
                    dr.Close();
                }
                else
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_158", "La cuenta de mayor no existe"), "D", "Cuenta");
                    result = false;
                    dr.Close();
                    return(result);
                }
                
                //------------- Cuenta de Mayor Vigente  ----------------
                if (statmcGLM03 == "*")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_160", "Cuenta de mayor inactiva"), "D", "Cuenta");
                    result = false;
                }

                cuenta = cuenta.Trim();
                //------------- Autorización Cuenta de Mayor  ----------------
                if (cuenta != "")
                {
                    if (!this.aut.Validar("003", "03", tPlan, "0" + sasimcGLM03))
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + cuenta, "D", "Cuenta");
                        result = false;
                    }
                }
        
                //------------- Cuenta de Mayor de Detalle  ----------------
                if (tcuemcGLM03 != "D")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_159", "La cuenta de mayor no es de detalle"), "D", "Cuenta");
                    result = false;
                }

                //------------- Cuenta de Mayor reservada a otras compañias  ----------------
                if (grupmcGLM03.Trim() != "")
                {
                    bool encuentra = false;
                    for (int i = 0; i < grupmcGLM03.Length-1; i++)
                    {
                        if (grupmcGLM03.Substring(i, 1) == this._cab_compania.Substring(0, 1))
                        {
                            encuentra = true;
                            break;
                        }
                    }

                    if (!encuentra)
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_161", "Cuenta de mayor reservada para otras compañías"), "D", "Cuenta");
                        result = false;
                    }
                }

                //------------- Tipo de extracontable  ----------------
                string apnaahPRT03 = "";
                string statahPRT03 = "";

                query = "select * ";
                query += "from " + GlobalVar.PrefijoTablaCG + "PRT03 ";
                query += "where TDATAH = '" + tipoExt + "'";
                 
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    apnaahPRT03 = dr.GetValue(dr.GetOrdinal("APNAAH")).ToString();
                    statahPRT03 = dr.GetValue(dr.GetOrdinal("STATAH")).ToString();
                    dr.Close();
                }
                else
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_??", "El tipo de extracontable no existe"), "D", "TipoExt");    //Falta traducir y dar de alta
                    result = false;
                    dr.Close();
                    return (result);
                }

                //------------- Tipo de extracontable Vigente  ----------------
                if (statahPRT03 == "*")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_??", "Tipo de extracontable inactivo"), "D", "TipoExt");    //Falta traducir y dar de alta
                    result = false;
                }

                bool resultAux = true;
                string errorAuxReq = "";
                string errorAuxNoLleva = "";
                string errorAuxNoExiste = "";
                string errorAuxInactivo = "";
                string errorAuxResOtrasComp = "";

                //------------- Primer Auxiliar  ----------------
                if (apnaahPRT03 == "0" || apnaahPRT03 == "1" ||
                    ((apnaahPRT03 == "2" || apnaahPRT03 == "3") &&
                     (auxiliar2 != "" && auxiliar2 != this.cuentaAuxiliarGlobal) ||
                     (auxiliar3 != "" && auxiliar3 != this.cuentaAuxiliarGlobal))
                    )
                {
                    errorAuxReq = this.LP.GetText("error_162", "Cuenta de mayor requiere primer auxiliar");
                    errorAuxNoLleva = this.LP.GetText("error_163", "Cuenta de mayor no lleva primer auxiliar");
                    errorAuxNoExiste = this.LP.GetText("error_164", "Primer auxiliar: Cuenta de auxiliar no existe");
                    errorAuxInactivo = this.LP.GetText("error_165", "Primer auxiliar: Cuenta de auxiliar inactiva");
                    errorAuxResOtrasComp = this.LP.GetText("error_166", "Primer auxiliar: Cuenta de auxiliar reservada para otras compañías");
                    resultAux = this.ValidarAuxiliar(linea, "Auxiliar1", tau1mcGLM03, auxiliar1, errorAuxReq, errorAuxNoLleva,
                                                  errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                    if (!resultAux) result = false;
                }

                //------------- Segundo Auxiliar  ----------------
                if (apnaahPRT03 == "0" || apnaahPRT03 == "2" ||
                    ((apnaahPRT03 == "1" || apnaahPRT03 == "3") &&
                     (auxiliar1 != "" && auxiliar1 != this.cuentaAuxiliarGlobal) ||
                     (auxiliar3 != "" && auxiliar3 != this.cuentaAuxiliarGlobal))
                    )
                {
                    errorAuxReq = this.LP.GetText("error_167", "Cuenta de mayor requiere segundo auxiliar");
                    errorAuxNoLleva = this.LP.GetText("error_168", "Cuenta de mayor no lleva segundo auxiliar");
                    errorAuxNoExiste = this.LP.GetText("error_169", "Segundo auxiliar: Cuenta de auxiliar no existe");
                    errorAuxInactivo = this.LP.GetText("error_170", "Segundo auxiliar: Cuenta de auxiliar inactiva");
                    errorAuxResOtrasComp = this.LP.GetText("error_171", "Segundo auxiliar: Cuenta de auxiliar reservada para otras compañías");
                    resultAux = this.ValidarAuxiliar(linea, "Auxiliar2", tau2mcGLM03, auxiliar2, errorAuxReq, errorAuxNoLleva,
                                                  errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                    if (!resultAux) result = false;
                }

                //------------- Tercer Auxiliar  ----------------
                if (apnaahPRT03 == "0" || apnaahPRT03 == "3" ||
                    ((apnaahPRT03 == "1" || apnaahPRT03 == "2") &&
                     (auxiliar1 != "" && auxiliar1 != this.cuentaAuxiliarGlobal) ||
                     (auxiliar2 != "" && auxiliar2 != this.cuentaAuxiliarGlobal))
                    )
                {
                    errorAuxReq = this.LP.GetText("error_172", "Cuenta de mayor requiere tercer auxiliar");
                    errorAuxNoLleva = this.LP.GetText("error_173", "Cuenta de mayor no lleva tercer auxiliar");
                    errorAuxNoExiste = this.LP.GetText("error_174", "Tercer auxiliar: Cuenta de auxiliar no existe");
                    errorAuxInactivo = this.LP.GetText("error_175", "Tercer auxiliar: Cuenta de auxiliar inactiva");
                    errorAuxResOtrasComp = this.LP.GetText("error_176", "Tercer auxiliar: Cuenta de auxiliar reservada para otras compañías");
                    resultAux = this.ValidarAuxiliar(linea, "Auxiliar3", tau3mcGLM03, auxiliar3, errorAuxReq, errorAuxNoLleva,
                                                  errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                    if (!resultAux) result = false;
                }
                else
                {
                    if (auxiliar1 != "" && auxiliar1 != "99999999")
                    {
                        //Validar 1er auxiliar
                        errorAuxReq = this.LP.GetText("error_162", "Cuenta de mayor requiere primer auxiliar");
                        errorAuxNoLleva = this.LP.GetText("error_163", "Cuenta de mayor no lleva primer auxiliar");
                        errorAuxNoExiste = this.LP.GetText("error_164", "Primer auxiliar: Cuenta de auxiliar no existe");
                        errorAuxInactivo = this.LP.GetText("error_165", "Primer auxiliar: Cuenta de auxiliar inactiva");
                        errorAuxResOtrasComp = this.LP.GetText("error_166", "Primer auxiliar: Cuenta de auxiliar reservada para otras compañías");
                        resultAux = this.ValidarAuxiliar(linea, "Auxiliar1", tau1mcGLM03, auxiliar1, errorAuxReq, errorAuxNoLleva,
                                                      errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                        if (!resultAux) result = false;
                    }

                    if (auxiliar2 != "" && auxiliar2 != "99999999")
                    {
                        //Validar 2do auxiliar
                        errorAuxReq = this.LP.GetText("error_167", "Cuenta de mayor requiere segundo auxiliar");
                        errorAuxNoLleva = this.LP.GetText("error_168", "Cuenta de mayor no lleva segundo auxiliar");
                        errorAuxNoExiste = this.LP.GetText("error_169", "Segundo auxiliar: Cuenta de auxiliar no existe");
                        errorAuxInactivo = this.LP.GetText("error_170", "Segundo auxiliar: Cuenta de auxiliar inactiva");
                        errorAuxResOtrasComp = this.LP.GetText("error_171", "Segundo auxiliar: Cuenta de auxiliar reservada para otras compañías");
                        resultAux = this.ValidarAuxiliar(linea, "Auxiliar2", tau2mcGLM03, auxiliar2, errorAuxReq, errorAuxNoLleva,
                                                      errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                        if (!resultAux) result = false;
                    }
                }

                //Este caso no debe suceder porque el desplegable devuelve siempre D o H
                //'Tipo de movimiento D/H/C/' '
                //auxTipoDH = Trim(UCase(det.sTipDH))
                //If auxTipoDH = "C" Then auxTipoDH = "H"

                //------------- Tipo de movimiento D/H/  ----------------
                auxTipoDH = dH.Trim();
                if (auxTipoDH != "D" && auxTipoDH != "H" && auxTipoDH != "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_177", "Tipo de movimiento erróneo"), "D", "DH");
                    result = false;
                }
            }
            catch(Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = false;
            }

            return (result);
        }

        /// <summary>
        /// Validar la cuenta de auxiliar
        /// </summary>
        /// <param name="linea">línea de detalle</param>
        /// <param name="campoAux">Literal de la Cuenta de auxiliar (Auxiliar1, Auxiliar2, Auxiliar3)</param>
        /// <param name="tauXmcGLM03">Tipo de auxiliar</param>
        /// <param name="detalleAuxiliar">Cuenta de auxiliar</param>
        /// <param name="errorAuxReq">mensaje de error de auxiliar requerido</param>
        /// <param name="errorAuxNoLleva">mensaje de error que no lleva auxiliar</param>
        /// <param name="errorAuxNoExiste">mensaje de error de no existe el auxiliar</param>
        /// <param name="errorAuxInactivo">mensaje de error de auxiliar inactivo</param>
        /// <param name="errorAuxResOtrasComp">mensaje de error cuenta de auxiliar reservado a otras compañías</param>
        /// <returns></returns>
        private bool ValidarAuxiliar(int linea, string campoAux, string tauXmcGLM03, string detalleAuxiliar, string errorAuxReq, string errorAuxNoLleva, 
                                    string errorAuxNoExiste, string errorAuxInactivo, string errorAuxResOtrasComp)
        {
            bool result = true;
            IDataReader dr = null;

            tauXmcGLM03 = tauXmcGLM03.Trim();
            detalleAuxiliar = detalleAuxiliar.Trim();
            try
            {
                //1er (2do o 3er) auxiliar requerido
                if (tauXmcGLM03 != "" && detalleAuxiliar == "")
                {
                    //162 - Cuenta de mayor requiere primer auxiliar
                    //167 - Cuenta de mayor requiere segundo auxiliar
                    //172 - Cuenta de mayor requiere tercer auxiliar
                    this.DSErroresAdd(linea, errorAuxReq, "D", campoAux);
                    result = false;
                }
                    
                //1er (2do o 3er) auxiliar, no lleva
                if (tauXmcGLM03 == "" && detalleAuxiliar != "")
                {
                    //163 - Cuenta de mayor no lleva primer auxiliar
                    //168 - Cuenta de mayor no lleva segundo auxiliar
                    //173 - Cuenta de mayor no lleva tercer auxiliar
                    this.DSErroresAdd(linea, errorAuxNoLleva, "D", campoAux);
                    result = false;
                }

                string query = "";
                string statmaGLM05 = "";
                string grupmaGLM05 = "";
                string grctmaGLM05 = "";
                //1er (2do o 3er) auxiliar, solo validar si no blanco
                if (detalleAuxiliar != "")
                {
                    //1er (2do o 3er) auxiliar exista + recuperar datos
                    query = "select STATMA, GRUPMA, GRCTMA from "  + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + tauXmcGLM03 + "' and CAUXMA = '" + detalleAuxiliar + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        statmaGLM05 = dr.GetValue(dr.GetOrdinal("STATMA")).ToString();
                        grupmaGLM05 = dr.GetValue(dr.GetOrdinal("GRUPMA")).ToString();
                        grctmaGLM05 = dr.GetValue(dr.GetOrdinal("GRCTMA")).ToString();
                    }
                    else 
                    {
                        //164 - Primer auxiliar: Cuenta de auxiliar no existe
                        //169 - Segundo auxiliar: Cuenta de auxiliar no existe
                        //174 - Tercer auxiliar: Cuenta de auxiliar no existe
                        this.DSErroresAdd(linea, errorAuxNoExiste, "D", campoAux);
                        result = false;
                    }
                    dr.Close();

                    //1er (2do o 3er) auxiliar no vigente
                    if (statmaGLM05 == "*")
                    {
                        //165 - Primer auxiliar: Cuenta de auxiliar inactiva
                        //170 - Segundo auxiliar: Cuenta de auxiliar inactiva
                        //175 - Tercer auxiliar: Cuenta de auxiliar inactiva
                        this.DSErroresAdd(linea, errorAuxInactivo, "D", campoAux);
                        result = false;
                    }

                    //Autorización 1er (2do o 3er) auxiliar
                    if (!this.aut.Validar("006", "02", tauXmcGLM03, "20"))
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + tauXmcGLM03, "D", campoAux);
                        result = false;
                    }
                    else
                    {
                        //Autorización grupo 1er (2do o 3er) auxiliar
                        if (grctmaGLM05.Trim() != "")
                        {
                            if (!this.aut.Validar("007", "02", tauXmcGLM03 + grctmaGLM05, "20"))
                            {
                                this.DSErroresAdd(linea, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + detalleAuxiliar, "D", campoAux);
                                result = false;
                            }
                        }
                    }

                    //1er (2do o 3er) auxiliar, cuenta auxiliar reservada para otras compañias
                    if (grupmaGLM05.Trim() != "")
                    {
                        bool encuentra = false;
                        for (int i = 0; i < grupmaGLM05.Length-1; i++)
                        {
                            if (grupmaGLM05.Substring(i, 1) == this._cab_compania.Substring(0, 1))
                            {
                                encuentra = true;
                                break;
                            }
                        }

                        if (!encuentra)
                        {
                            //166 - Primer auxiliar: Cuenta de auxiliar reservada para otras compañías
                            //171 - Segundo auxiliar: Cuenta de auxiliar reservada para otras compañías
                            //176 - Tercer auxiliar: Cuenta de auxiliar reservada para otras compañías
                            this.DSErroresAdd(linea, errorAuxResOtrasComp, "D", campoAux);
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                this.DSErroresAdd(linea, ex.Message, "D", campoAux);
                result = false;
            }

            return (result);
        }

        /// <summary>
        /// A la tabla de errores del DataSet, le añade una línea
        /// </summary>
        /// <param name="linea">No. Fila donde hay error</param>
        /// <param name="error">Mensaje de error</param>
        /// <param name="codiTipo">C - cabecera    D - detalle</param>
        /// <param name="control_celda">Nombre del control donde se produce el error o columna del detalle</param>
        private void DSErroresAdd(int linea, string error, string codiTipo, string control_celda)
        {
            DataRow row = this.dsErrores.Tables["Errores"].NewRow();

            if (codiTipo == "C")
            {
                row[0] = this.LP.GetText("lblCabecera", "Cabecera");   //FALTA traducir
                row[1] = "";
            }
            else
            {
                row[0] = this.LP.GetText("lblDetalle", "Detalle");                     //FALTA traducir
                row[1] = linea.ToString();
            }

            row[2] = error;
            row[3] = codiTipo;
            row[4] = control_celda;
            
            this.dsErrores.Tables["Errores"].Rows.Add(row);
        }
    }
}
