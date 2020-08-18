using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ObjectModel;
using log4net;

namespace ModComprobantes
{
    public class ComprobanteContable
    {
        private string _cab_compania;
        private string _cab_anoperiodo;
        private string _cab_fecha;
        private string _cab_tipo;
        private string _cab_noComprobante;
        private string _cab_clase;
        private string _cab_tasa;
        private string _cab_descripcion;
        private string _cab_extendido;

        private DataTable _det_detalles;

        private DataSet dsErrores;
        
        //---- Se utiliza desde la Gestión de Lotes para cargar el comprobante
        private string biblioteca;
        private string prefijo;

        //-- Se utiliza para validar el tipo de comprobante batch por defecto, si compGLB01 es true se validan tipos interactivos
        private bool compGLB01 = false;

        private readonly Autorizaciones aut;
        private Utiles utiles;
        private readonly UtilesCGConsultas utilesCG;
        private LanguageProvider LP;
        private ILog Log;

        private string tPlan = "";
        private string nombtvGLT06 = "";
        private string defdtvGLT06 = "";

        //Campos GLMX2
        private string NM01PX = "";
        private string MX01PX = "";
        private string TA01PX = "";
        private string NM02PX = "";
        private string MX02PX = "";
        private string TA02PX = "";
        private string NM03PX = "";
        private string MX03PX = "";
        private string TA03PX = "";
        private string NM04PX = "";
        private string MX04PX = "";
        private string TA04PX = "";
        private string NM05PX = "";
        private string MX05PX = "";
        private string TA05PX = "";
        private string NM06PX = "";
        private string MX06PX = "";
        private string TA06PX = "";
        private string NM07PX = "";
        private string MX07PX = "";
        private string TA07PX = "";
        private string NM08PX = "";
        private string MX08PX = "";
        private string TA08PX = "";
        private string NM09PX = "";
        private string NM10PX = "";
        private string NM11PX = "";
        private string NM12PX = "";

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

        public string Cab_clase
        {
            get
            {
                return (this._cab_clase);
            }
            set
            {
                this._cab_clase = value;
            }
        }

        public string Cab_tasa
        {
            get
            {
                return (this._cab_tasa);
            }
            set
            {
                this._cab_tasa = value;
            }
        }

        public string Cab_descripcion
        {
            get
            {
                return (this._cab_descripcion);
            }
            set
            {
                this._cab_descripcion = value;
            }
        }

        public string Cab_extendido
        {
            get
            {
                return (this._cab_extendido);
            }
            set
            {
                this._cab_extendido = value;
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

        public string Biblioteca
        {
            get
            {
                return (this.biblioteca);
            }
            set
            {
                this.biblioteca = value;
            }
        }

        public string Prefijo
        {
            get
            {
                return (this.prefijo);
            }
            set
            {
                this.prefijo = value;
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

        public bool CompGLB01
        {
            get
            {
                return (this.compGLB01);
            }
            set
            {
                this.compGLB01 = value;
            }
        }
        #endregion

        public ComprobanteContable()
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

            string timompGLM02 = "";

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
                    this.DSErroresAdd(-1, this.LP.GetText("error_148", "Compañía no existe"), "C", "cmbCompania");
                    result = false;
                    dr.Close();
                    return (result);
                }

                //------------- Compañía Inactiva  ----------------
                if (statmgGLM01 == "*")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_149", "Compañía inactiva"), "C", "cmbCompania");
                    result = false;
                }
                
                //------------- Autorización Compañía  ----------------
                if (this._cab_compania.Trim() != "")
                {
                    if (!this.aut.Validar("002", "02", this._cab_compania.Trim(), "20"))
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + this._cab_compania, "C", "cmbCompania");
                        result = false;
                    }
                }
                
                //------------- Periodo Incorrecto  ----------------   
                int registros;
                //Buscar el siglo dado el año
                if (this._cab_anoperiodo != "")
                {
                    string aa = this._cab_anoperiodo.Substring(0, 2);
                    saprflGLT04 = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this._cab_anoperiodo;

                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + titamgGLM01 + "' and SAPRFL = " + saprflGLT04;

                    registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                    if (registros == 0)
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_150", "Periodo incorrecto"), "C", "txtMaskAAPP");
                        result = false;
                    }
                }
                else
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_PeriodoNoInformado", "Periodo no está informado"), "C", "txtMaskAAPP");   //Falta traducir
                    result = false;
                }

                //------------- Tipo de Comprobante Exista  ---------------- 
                query = "select STATTV, CODITV, NOMBTV, DEFDTV from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where TIVOTV = '" + this._cab_tipo + "'";
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    stattvGLT06 = dr.GetValue(dr.GetOrdinal("STATTV")).ToString();
                    coditvGLT06 = dr.GetValue(dr.GetOrdinal("CODITV")).ToString();
                    nombtvGLT06 = dr.GetValue(dr.GetOrdinal("NOMBTV")).ToString();
                    defdtvGLT06 = dr.GetValue(dr.GetOrdinal("DEFDTV")).ToString();
                }
                else
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_151", "Tipo de comprobante no existe"), "C", "cmbTipo");
                    result = false;
                }
                dr.Close();

                //------------- Tipo de Comprobante Invalido para Batch  ---------------- 
                if (coditvGLT06 != "1" && !this.compGLB01)
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_152", "Tipo de comprobante inválido para entrada batch"), "C", "cmbTipo");
                    result = false;
                }

                //------------- Tipo de Comprobante Invalido para Entrada Interactiva ---------------- 
                if (this.compGLB01 && coditvGLT06 != "0")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_152_1", "Tipo de comprobante inválido para entrada interactiva"), "C", "cmbTipo"); //Falta traducir
                    result = false;
                }
                
                //------------- Tipo de Comprobante Inactivo  ---------------- 
                if (stattvGLT06 == "*")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_153", "Tipo de comprobante inactivo"), "C", "cmbTipo");
                    result = false;
                }

                //------------- Autorización Tipo de Comprobante  ----------------
                if (this._cab_tipo.Trim() != "")
                {
                    if (!this.aut.Validar("004", "03", this._cab_tipo.Trim(), "20"))
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + this._cab_tipo, "C", "cmbTipo");
                        result = false;
                    }
                }

                //------------- Fecha de Comprobante  ----------------
                if (saprflGLT04 != "")
                {
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

                //------------- Clase de comprobante 0,1,2,3  ----------------
                bool errorClase = false;
                if (this._cab_clase.Trim() != "")
                {
                    try
                    {
                        int clase = Convert.ToInt32(this._cab_clase);
                        if (!(clase >= 0 && clase <= 3)) errorClase = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        errorClase = true;
                    } 
                }
                else errorClase = true;

                if (errorClase)
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_155", "Clase de comprobante debe ser 0,1,2 o 3"), "C", "cmbClase");
                    result = false;
                }

                //------------- Clase de comprobante inválida para la moneda  ----------------
                query = "select TIMOMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP='" + tPlan + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    timompGLM02 = dr.GetValue(dr.GetOrdinal("TIMOMP")).ToString();
                }
                else
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_156", "Clase de comprobante inválida para la moneda"), "C", "cmbClase");
                    result = false;
                }
                dr.Close();

                if (timompGLM02.Trim() == "" && (this._cab_clase == "1" || this._cab_clase == "2"))
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_156", "Clase de comprobante inválida para la moneda"), "C", "cmbClase");
                    result = false;
                }

                if (timompGLM02.Trim() != "" && this._cab_clase == "0")
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_156", "Clase de comprobante inválida para la moneda"), "C", "cmbClase");
                    result = false;
                }

                //------------- Tasa de cambio  ----------------
                string auxTasa = "";
                if (this._cab_tasa != null && this._cab_tasa != "") auxTasa = this._cab_tasa.Trim();

                if ((this._cab_clase.Trim() == "0" || this._cab_clase.Trim() == "3") && (auxTasa != ""))
                {
                    this.DSErroresAdd(-1, this.LP.GetText("error_157", "Tasa de cambio inválida"), "C", "txtTasa");
                    result = false;
                }

                //Validaciones de campos extendidos
                if (this._cab_extendido == "1")
                {
                    //------------- La compañía no tiene campos ampliados (por seguridad, no debería darse el caso)  ----------------
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                    query += "where TIPLPX = '" + tPlan + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        NM01PX = dr.GetValue(dr.GetOrdinal("NM01PX")).ToString().Trim();
                        MX01PX = dr.GetValue(dr.GetOrdinal("MX01PX")).ToString().Trim();
                        TA01PX = dr.GetValue(dr.GetOrdinal("TA01PX")).ToString().Trim();
                        NM02PX = dr.GetValue(dr.GetOrdinal("NM02PX")).ToString().Trim();
                        MX02PX = dr.GetValue(dr.GetOrdinal("MX02PX")).ToString().Trim();
                        TA02PX = dr.GetValue(dr.GetOrdinal("TA02PX")).ToString().Trim();
                        NM03PX = dr.GetValue(dr.GetOrdinal("NM03PX")).ToString().Trim();
                        MX03PX = dr.GetValue(dr.GetOrdinal("MX03PX")).ToString().Trim();
                        TA03PX = dr.GetValue(dr.GetOrdinal("TA03PX")).ToString().Trim();
                        NM04PX = dr.GetValue(dr.GetOrdinal("NM04PX")).ToString().Trim();
                        MX04PX = dr.GetValue(dr.GetOrdinal("MX04PX")).ToString().Trim();
                        TA04PX = dr.GetValue(dr.GetOrdinal("TA04PX")).ToString().Trim();
                        NM05PX = dr.GetValue(dr.GetOrdinal("NM05PX")).ToString().Trim();
                        MX05PX = dr.GetValue(dr.GetOrdinal("MX05PX")).ToString().Trim();
                        TA05PX = dr.GetValue(dr.GetOrdinal("TA05PX")).ToString().Trim();
                        NM06PX = dr.GetValue(dr.GetOrdinal("NM06PX")).ToString().Trim();
                        MX06PX = dr.GetValue(dr.GetOrdinal("MX06PX")).ToString().Trim();
                        TA06PX = dr.GetValue(dr.GetOrdinal("TA06PX")).ToString().Trim();
                        NM07PX = dr.GetValue(dr.GetOrdinal("NM07PX")).ToString().Trim();
                        MX07PX = dr.GetValue(dr.GetOrdinal("MX07PX")).ToString().Trim();
                        TA07PX = dr.GetValue(dr.GetOrdinal("TA07PX")).ToString().Trim();
                        NM08PX = dr.GetValue(dr.GetOrdinal("NM08PX")).ToString().Trim();
                        MX08PX = dr.GetValue(dr.GetOrdinal("MX08PX")).ToString().Trim();
                        TA08PX = dr.GetValue(dr.GetOrdinal("TA08PX")).ToString().Trim();
                        NM09PX = dr.GetValue(dr.GetOrdinal("NM09PX")).ToString().Trim();
                        NM10PX = dr.GetValue(dr.GetOrdinal("NM10PX")).ToString().Trim();
                        NM11PX = dr.GetValue(dr.GetOrdinal("NM11PX")).ToString().Trim();
                        NM12PX = dr.GetValue(dr.GetOrdinal("NM12PX")).ToString().Trim();
                    }
                    else
                    {
                        this.DSErroresAdd(-1, this.LP.GetText("error_209", "La compañía no tiene campos ampliados"), "C", "cmbCompania");  //Falta traducir
                        result = false;
                    }
                    dr.Close();
                }
            }
            catch(Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

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
            string adicmcGLM03 = "";
            string sasimcGLM03 = "";
            string fevemcGLM03 = "";
            string nddomcGLM03 = "";
            string tercmcGLM03 = "";
            string timomcGLM03 = "";
            string tau1mcGLM03 = "";
            string tau2mcGLM03 = "";
            string tau3mcGLM03 = "";
            string tdocmcGLM03 = "";
            string grupmcGLM03 = "";
            string rnitmcGLM03 = "";
            string termmcGLM03 = "";
            string timompGLM02 = "";
            string auxTipoDH = "";

            //IVA
            string ciaft2IVT02 = "";
            string statciIVT01 = "";
            string cuenciIVT01 = "";
            string tauxciIVT01 = "";
            string tpivciIVT01 = "";
            string recgciIVT01 = "";
            string cuadciIVT01 = "";
            string mxajciIVT01 = "";
            string ulmct3IVT03 = "";

            int chkcrcGLC01 = 0;
            int registros = 0;

            //Valores Línea de Detalles del Comprobante
            string cuenta = "";
            string auxiliar1 = "";
            string auxiliar2 = "";
            string auxiliar3 = "";
            string dH = "";
            string monedaLocal = "";
            decimal monedaLocalDec = 0;
            string monedaExt = "";
            decimal monedaExtDec = 0;
            string rU = "";
            string descripcion = "";
            string documento = "";
            string claseDoc = "";
            string numeroDoc = "";
            string fecha = "";
            string vencimiento = "";
            string documento2 = "";
            string claseDoc2 = "";
            string numeroDoc2 = "";
            int numeroDoc2Num = 0;
            string importe3 = "";
            decimal importe3Dec = 0;
            string iva = "";
            string cifDni = "";

            string prefijoDoc = "";
            string numFactAmp = "";
            string numFactRectif = "";
            string fechaServIVA = "";
            string campoUserAlfa1 = "";
            string campoUserAlfa2 = "";
            string campoUserAlfa3 = "";
            string campoUserAlfa4 = "";
            string campoUserAlfa5 = "";
            string campoUserAlfa6 = "";
            string campoUserAlfa7 = "";
            string campoUserAlfa8 = "";
            string campoUserNum1 = "";
            string campoUserNum2 = "";
            string campoUserFecha1 = "";
            string campoUserFecha2 = "";

            try
            {
                DataRow row = this._det_detalles.Rows[indice];

                cuenta = row["Cuenta"].ToString();
                auxiliar1 = row["Auxiliar1"].ToString().ToUpper();
                auxiliar2 = row["Auxiliar2"].ToString().ToUpper();
                auxiliar3 = row["Auxiliar3"].ToString().ToUpper();
                dH = row["DH"].ToString().ToUpper();
                monedaLocal = row["MonedaLocal"].ToString();
                try { if (monedaLocal != "") monedaLocalDec = Convert.ToDecimal(monedaLocal); }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                monedaExt = row["MonedaExt"].ToString();
                try { if (monedaExt != "") monedaExtDec = Convert.ToDecimal(monedaExt); }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                rU = row["RU"].ToString().ToUpper();
                descripcion = row["Descripcion"].ToString();
                documento = row["Documento"].ToString();
                int pos = documento.IndexOf('-');
                if (pos != -1)
                {
                    claseDoc = documento.Substring(0, pos).Trim().ToUpper();
                    numeroDoc = documento.Substring(pos+1, documento.Length - pos - 1).Trim();
                }

                fecha = row["Fecha"].ToString();
                vencimiento = row["Vencimiento"].ToString();
                documento2 = row["Documento2"].ToString();
                pos = documento2.IndexOf('-');
                if (pos != -1)
                {
                    claseDoc2 = documento2.Substring(0, pos).Trim().ToUpper();
                    numeroDoc2 = documento2.Substring(pos + 1, documento2.Length - pos - 1).Trim();
                    try { if (numeroDoc2 != "") numeroDoc2Num = Convert.ToInt32(numeroDoc2); }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else
                {
                    if (documento2 != "")
                    {
                        if (documento2.Length < 2) claseDoc2 = documento2;
                        else claseDoc2 = documento2.Substring(0, 2);
                    }
                }
                
                importe3 = row["Importe3"].ToString();
                try { if (importe3 != "") importe3Dec = Convert.ToDecimal(importe3); }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                iva = row["Iva"].ToString().ToUpper();
                cifDni = row["CifDni"].ToString().ToUpper();

                if (cuenta.Trim() == "" && auxiliar1.Trim() == "" && auxiliar2.Trim() == "" && auxiliar3.Trim() == "" &&
                    dH.Trim() == "" && monedaLocal.Trim() == "" && monedaExt.Trim() == "" && rU.Trim() == "" &&
                    descripcion.Trim() == "" && documento.Trim() == "" && fecha.Trim() == "" && vencimiento.Trim() == "" &&
                    documento2.Trim() == "" && importe3.Trim() == "" && iva.Trim() == "" && cifDni.Trim() == "")
                {
                    //Línea en blanco
                    return (result);
                }

                //------------- Cuenta de Mayor no informada  ----------------
                if (cuenta.Trim() == "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_158_1", "La cuenta de mayor no está informada"), "D", "Cuenta");
                    result = false;
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
                    adicmcGLM03 = dr.GetValue(dr.GetOrdinal("ADICMC")).ToString();
                    sasimcGLM03 = dr.GetValue(dr.GetOrdinal("SASIMC")).ToString();
                    fevemcGLM03 = dr.GetValue(dr.GetOrdinal("FEVEMC")).ToString();
                    nddomcGLM03 = dr.GetValue(dr.GetOrdinal("NDDOMC")).ToString();
                    tercmcGLM03 = dr.GetValue(dr.GetOrdinal("TERMMC")).ToString();
                    timomcGLM03 = dr.GetValue(dr.GetOrdinal("TIMOMC")).ToString();
                    tau1mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString();
                    tau2mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString();
                    tau3mcGLM03 = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString();
                    tdocmcGLM03 = dr.GetValue(dr.GetOrdinal("TDOCMC")).ToString();
                    grupmcGLM03 = dr.GetValue(dr.GetOrdinal("GRUPMC")).ToString();
                    rnitmcGLM03 = dr.GetValue(dr.GetOrdinal("RNITMC")).ToString();
                    termmcGLM03 = dr.GetValue(dr.GetOrdinal("TERMMC")).ToString();
                    dr.Close();
                }
                else
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_158", "La cuenta de mayor no existe"), "D", "Cuenta");
                    result = false;
                    dr.Close();
                    return(result);
                }

                //------------- Cuenta de Mayor de Detalle  ----------------
                if (tcuemcGLM03 != "D")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_159", "La cuenta de mayor no es de detalle"), "D", "Cuenta");
                    result = false;
                    return (result);
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


                    //----- Autorización Grupo Cuenta de Mayor ------
                    //Verificar que exista la tabla GLMX3
                    //bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX3");

                    //if (!existeTabla) return;

                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX = '" + tPlan + "' and ";
                    query += "CUENMX like '" + cuenta + "%' ";

                    string grctmxGLMX3 = "";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        grctmxGLMX3 = dr.GetValue(dr.GetOrdinal("GRCTMX")).ToString();
                    }

                    dr.Close();

                    
                    if (grctmxGLMX3.Trim() != "")
                    {
                        if (!this.aut.Validar("008", "02", tPlan + grctmxGLMX3, "20"))
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_79", "Usuario no autorizado a este Elemento") + " " + cuenta, "D", cuenta);
                            result = false;
                        }
                    }
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

                bool resultAux = true;
                //------------- Primer Auxiliar  ----------------
                string errorAuxReq = this.LP.GetText("error_162", "Cuenta de mayor requiere primer auxiliar");
                string errorAuxNoLleva = this.LP.GetText("error_163", "Cuenta de mayor no lleva primer auxiliar");
                string errorAuxNoExiste = this.LP.GetText("error_164", "Primer auxiliar: Cuenta de auxiliar no existe");
                string errorAuxInactivo = this.LP.GetText("error_165", "Primer auxiliar: Cuenta de auxiliar inactiva");
                string errorAuxResOtrasComp = this.LP.GetText("error_166", "Primer auxiliar: Cuenta de auxiliar reservada para otras compañías");
                resultAux = this.ValidarAuxiliar(linea, "Auxiliar1", tau1mcGLM03, auxiliar1, errorAuxReq, errorAuxNoLleva,
                                              errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                if (!resultAux) result = false;

                //------------- Segundo Auxiliar  ----------------
                errorAuxReq = this.LP.GetText("error_167", "Cuenta de mayor requiere segundo auxiliar");
                errorAuxNoLleva = this.LP.GetText("error_168", "Cuenta de mayor no lleva segundo auxiliar");
                errorAuxNoExiste = this.LP.GetText("error_169", "Segundo auxiliar: Cuenta de auxiliar no existe");
                errorAuxInactivo = this.LP.GetText("error_170", "Segundo auxiliar: Cuenta de auxiliar inactiva");
                errorAuxResOtrasComp = this.LP.GetText("error_171", "Segundo auxiliar: Cuenta de auxiliar reservada para otras compañías");
                resultAux = this.ValidarAuxiliar(linea, "Auxiliar2", tau2mcGLM03, auxiliar2, errorAuxReq, errorAuxNoLleva,
                                              errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                if (!resultAux) result = false;

                //------------- Tercer Auxiliar  ----------------
                errorAuxReq = this.LP.GetText("error_172", "Cuenta de mayor requiere tercer auxiliar");
                errorAuxNoLleva = this.LP.GetText("error_173", "Cuenta de mayor no lleva tercer auxiliar");
                errorAuxNoExiste = this.LP.GetText("error_174", "Tercer auxiliar: Cuenta de auxiliar no existe");
                errorAuxInactivo = this.LP.GetText("error_175", "Tercer auxiliar: Cuenta de auxiliar inactiva");
                errorAuxResOtrasComp = this.LP.GetText("error_176", "Tercer auxiliar: Cuenta de auxiliar reservada para otras compañías");
                resultAux = this.ValidarAuxiliar(linea, "Auxiliar3", tau3mcGLM03, auxiliar3, errorAuxReq, errorAuxNoLleva,
                                              errorAuxNoExiste, errorAuxInactivo, errorAuxResOtrasComp);
                if (!resultAux) result = false;

                //Este caso no debe suceder porque el desplegable devuelve siempre D o H
                //'Tipo de movimiento D/H/C/' '
                //auxTipoDH = Trim(UCase(det.sTipDH))
                //If auxTipoDH = "C" Then auxTipoDH = "H"


                //--------------Combinacion de Auxiliares ---------------
                Boolean hayCombinacion = false;
                //si no existe entrada en glt21 para ccia21/*,tipl21,cuen21 -> validacion OK
                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT21 ";
                query += "where (CCIA21 = '" + this._cab_compania + "' OR CCIA21 = '*') AND ";
                query += "TIPL21 = '" + tPlan + "' AND CUEN21 = '" + CtaMayorUltimoNivel(tPlan, cuenta) + "'";

                registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (registros == 0)
                {
                    hayCombinacion = true;
                }

                //buscar en GLT21 combinaciones correctas
                if (hayCombinacion == false) {
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT21 ";
                    query += "where (CCIA21 = '" + this._cab_compania + "' OR CCIA21 = '*') AND TIPL21 = '" + tPlan + "' AND ";
                    query += "CUEN21 = '" + CtaMayorUltimoNivel(tPlan, cuenta) + "' AND ";
                    query += "(AUX121 = '" + auxiliar1 + "' OR AUX121 = '') AND ";
                    query += "(AUX221 = '" + auxiliar2 + "' OR AUX221 = '') AND ";
                    query += "(AUX321 = '" + auxiliar3 + "' OR AUX321 = '')";

                    registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                    if (registros > 0)
                    {
                        hayCombinacion = true;
                    }
                }

                if (hayCombinacion == false)
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_217", "Combinación de Cuenta de Mayor/Auxiliares inválida"), "D", "Cuenta");
                    result = false;
                }



                //------------- Tipo de movimiento D/H/  ----------------
                auxTipoDH = dH.Trim();
                if (auxTipoDH != "D" && auxTipoDH != "H" && auxTipoDH != "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_177", "Tipo de movimiento erróneo"), "D", "DH");
                    result = false;
                }

                //------------- Moneda local en blanco para clase 2  ----------------
                if (this._cab_clase == "2" && (monedaLocal != "" && monedaLocalDec != 0 ))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_178", "Moneda local debe estar en blanco para clase de comprobante 2"), "D", "MonedaLocal");
                    result = false;
                }

                //------------- Moneda extranjera en blanco para clases 0 y 1  ----------------
                // if ((this._cab_clase == "0" || this._cab_clase == "1") && (monedaExt != "" && monedaExtDec != 0)) cambiado por jl el 13/7/20 para igualarlo con el AS
                if ((this._cab_clase == "0") && (monedaExt != "" && monedaExtDec != 0))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_179", "Moneda extranjera debe estar en blanco para clases de comprobante 0 y 1"), "D", "MonedaExt");
                    result = false;
                }

                //------------- Moneda extranjera, validar solo si diferente de blanco  ----------------
                if (monedaExt != "" && monedaExtDec != 0)
                {
                    //Moneda extranjera, no lleva ni en GLM02 ni en GLM03
                    query = "select TIMOMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                    query += "where TIPLMP = '" + tPlan + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        timompGLM02 = dr.GetValue(dr.GetOrdinal("TIMOMP")).ToString();
                    }
                    else
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_180", "Este movimiento no lleva moneda extranjera"), "D", "MonedaExt");
                        result = false;
                    }
                    dr.Close();

                    if (timompGLM02.Trim() == "" && timomcGLM03.Trim() == "")
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_180", "Este movimiento no lleva moneda extranjera"), "D", "MonedaExt");
                        result = false;
                    }
                }

                //------------- RU  ----------------
                if (adicmcGLM03 != "N")
                {
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "G3T03 ";
                    query += "where CODI3F = '" + rU + "'";

                    registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                    if (registros == 0)
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_189", "Reservado usuario no definido"), "D", "RU");
                        result = false;
                    }
                }

                //------------- Clase y numero de documento, carteras  ----------------
                if (nombtvGLT06.Length < 40)
                {
                    if (adicmcGLM03.Trim() != "N" && fevemcGLM03.Trim() != dH.Trim())
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_190", "Este documento debe cancelarse desde carteras"), "D", "Documento");
                        result = false;
                    }
                }
                else
                {
                    if (nombtvGLT06.Substring(39, 1) != "*")
                    {
                        if (adicmcGLM03.Trim() != "N" && fevemcGLM03.Trim() != dH.Trim())
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_190", "Este documento debe cancelarse desde carteras"), "D", "Documento");
                            result = false;
                        }
                    }
                }
                

                documento = documento.Trim();
                //------------- Clase y numero de documento, no lleva  ----------------
                if (tdocmcGLM03 == "N" && (claseDoc != "" || numeroDoc != ""))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_181", "Este asiento no lleva número de documento"), "D", "Documento");
                    result = false;
                }

                //------------- Clase y numero de documento, falta  ----------------
                if (tdocmcGLM03 == "S" && (claseDoc == "" && numeroDoc == ""))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_182", "Falta el número de documento"), "D", "Documento");
                    result = false;
                }

                //-------------- Numero de documento - ya existe/no existe/ debe ser carteras -----------
                if ((numeroDoc != "") && (auxiliar1 != ""))
                {
                    //---Buscar si ya existe
                    if ((defdtvGLT06 == "1") && (auxTipoDH == fevemcGLM03))
                    {
                        query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                        query += "where TAUXDT = '" + tau1mcGLM03 + "' AND ";
                        query += "CAUXDT ='" + auxiliar1 + "' AND ";
                        query += "CCIADT ='" + this._cab_compania + "' AND ";
                        query += "TIPLDT ='" + tPlan + "' AND ";
                        query += "CUENDT = '" + CtaMayorUltimoNivel(tPlan, cuenta) + "' AND ";
                        query += "CLDODT = '" + claseDoc + "' AND ";
                        query += "NDOCDT = " + numeroDoc + " AND ";
                        query += "TMOVDT = '" +  auxTipoDH + "'"; 

                        registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                        if (registros > 0)
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_80", "Documento ya exite"), "D", "Documento");
                            result = false;
                        }
                    }

                    //---Buscar si no existe
                    if ((defdtvGLT06 == "1" || defdtvGLT06 == "2") && (auxTipoDH != fevemcGLM03) && (fevemcGLM03 != "N"))
                    {
                        query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                        query += "where TAUXDT = '" + tau1mcGLM03 + "' AND ";
                        query += "CAUXDT ='" + auxiliar1 + "' AND ";
                        query += "CCIADT ='" + this._cab_compania + "' AND ";
                        query += "TIPLDT ='" + tPlan + "' AND ";
                        query += "CUENDT = '" + CtaMayorUltimoNivel(tPlan, cuenta) + "' AND ";
                        query += "CLDODT = '" + claseDoc + "' AND ";
                        query += "NDOCDT = " + numeroDoc;

                        registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                        if (registros == 0)
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_81", "Documento no exite"), "D", "Documento");
                            result = false;
                        }
                    }

                    //---Buscar si debe ser de carteras
                    if (nombtvGLT06.Length < 40)
                    {
                        if (adicmcGLM03.Trim() != "N" && fevemcGLM03.Trim() != dH.Trim())
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_190", "Este documento debe cancelarse desde carteras"), "D", "Documento");
                            result = false;
                        }
                    }
                    else
                    {
                        if (nombtvGLT06.Substring(39, 1) != "*")
                        {
                            if ((adicmcGLM03 != "N") && (fevemcGLM03 != "N") && (auxTipoDH != fevemcGLM03))
                            {
                                this.DSErroresAdd(linea, this.LP.GetText("error_190", "Este documento debe cancelarse desde carteras"), "D", "Documento");
                                result = false;
                            }
                        }
                    }
                }
                //---------------------------------------------------------------------------------------
                
                iva = iva.Trim();
                fecha = fecha.Trim();
                //------------- Fecha documento, no lleva  ----------------
                if (tdocmcGLM03 == "N" && iva == "" && fecha != "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_183", "Este asiento no lleva fecha de documento"), "D", "Fecha");
                    result = false;
                }

                //------------- Fecha documento, falta  ----------------
                //If tdocmcGLM03 = "S" And Trim(det.sFechaDocumento) = 0 Then
                //    ValidarDetalle = Not RC_OK
                //    oMultiIdioma.ShowMsg 184, vbCritical, Me.Caption
                //    Exit Function
                //End If

                if (tdocmcGLM03 == "S" && fecha.Trim() == "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_184", "Falta la fecha de documento"), "D", "Fecha");
                    result = false;
                }


                vencimiento = vencimiento.Trim();
                //------------- Fecha vencimiento, no lleva  ----------------
                if ((fevemcGLM03 == "N" || fevemcGLM03 != auxTipoDH) && vencimiento != "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_185", "Este asiento no lleva fecha de vencimiento"), "D", "Vencimiento");
                    result = false;
                }
    
                //------------- Fecha vencimiento, falta  ----------------
                if (fevemcGLM03 != "N" && vencimiento == "" && fevemcGLM03 == auxTipoDH)
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_186", "Falta la fecha de vencimiento"), "D", "Vencimiento");
                    result = false;
                }

                claseDoc2 = claseDoc2.Trim();
                documento2 = documento2.Trim();
                //------------- Segundo documento, no lleva  ----------------
                if (nddomcGLM03 == "N" && (claseDoc2 != "" || documento2 != ""))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_187", "Este asiento no lleva segundo documento"), "D", "Documento2");
                    result = false;
                }

                //------------- Segundo documento, falta  ----------------
                if (nddomcGLM03 == "S" && (claseDoc2 == "" && numeroDoc2 == ""))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_188", "Falta el segundo documento"), "D", "Documento2");
                    result = false;
                }
                //else
                //{
                //    if (adicmcGLM03 != "N" && nddomcGLM03 == "S" && auxTipoDH == fevemcGLM03 && (claseDoc2 != "*" || numeroDoc2Num != 0))
                //    {
                //        this.DSErroresAdd(linea, this.LP.GetText("error_208", "El segundo documento debe ser *-000000000"), "D", "Documento2");
                //        result = false;
                //    }
                //}

                //------------- Segundo documento, no lleva si cuenta es iva e iva es blanco  ----------------
                if (nddomcGLM03 == "R" && iva == "" && (claseDoc2 != "" || numeroDoc2Num != 0))
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_193", "Cuando la cuenta es de IVA y el código de IVA está en blanco, no lleva segundo documento"), "D", "Documento2");
                    result = false;
                }

                //------------- Tercer importe, no debe llevar  ----------------
                if (termmcGLM03 == "R" && iva == "" && importe3Dec != 0)
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_191", "Este asiento no debe llevar tercer importe"), "D", "Importe3");
                    result = false;
                }
                if (termmcGLM03 == "N" && importe3Dec != 0)
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_191", "Este asiento no debe llevar tercer importe"), "D", "Importe3");
                    result = false;
                }
                    
                //------------- Tercer importe, debe llevar  ----------------
                if (termmcGLM03 == "S" && importe3Dec == 0)
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_204", "Este asiento debe llevar tercer importe"), "D", "Importe3");
                    result = false;
                }

                //------------- IVA, validar solo si diferente de blanco  ----------------
                if (iva != "")
                {
                    bool validarIVA = true;
                    //------------- Cia no definida en cias fiscales + recuperar datos  ----------------
                    query = "select CIAFT2 from " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "where CCIAT2 = '" + this._cab_compania +  "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        ciaft2IVT02 = dr.GetValue(dr.GetOrdinal("CIAFT2")).ToString();
                    }
                    else
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_192", "Compañía no definida en la tabla de compañías fiscales"), "D", "");
                        result = false;
                        validarIVA = false;
                    }
                    dr.Close();

                    //------------- Cuenta de mayor invalida para iva  ----------------
                    if (validarIVA && rnitmcGLM03 != "R")
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_194", "Cuenta de mayor inválida para IVA"), "D", "Cuenta");
                        result = false;
                    }

                    //------------- Falta documento para iva  ----------------
                    if (validarIVA && nddomcGLM03 == "R" && (claseDoc2 == "" && numeroDoc2 == ""))
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_195", "Falta el número de documento de IVA"), "D", "Documento2");
                        result = false;
                    }

                    if (validarIVA)
                    {
                        //------------- Código iva no existe + recuperar datos  ----------------
                        query = "select STATCI, CUENCI, TAUXCI, TPIVCI, RECGCI, CUADCI, MXAJCI from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                        query += "where COIVCI = '" + iva + "' and TIPLCI = '" + tPlan + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            statciIVT01 = dr.GetValue(dr.GetOrdinal("STATCI")).ToString();
                            cuenciIVT01 = dr.GetValue(dr.GetOrdinal("CUENCI")).ToString();
                            tauxciIVT01 = dr.GetValue(dr.GetOrdinal("TAUXCI")).ToString();
                            tpivciIVT01 = dr.GetValue(dr.GetOrdinal("TPIVCI")).ToString();
                            recgciIVT01 = dr.GetValue(dr.GetOrdinal("RECGCI")).ToString();
                            cuadciIVT01 = dr.GetValue(dr.GetOrdinal("CUADCI")).ToString();
                            mxajciIVT01 = dr.GetValue(dr.GetOrdinal("MXAJCI")).ToString();
                        }
                        else
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_199", "El código de IVA no existe"), "D", "Iva");
                            result = false;
                            validarIVA = false;
                        }
                        dr.Close();
                    }

                    //------------- Código iva vigente  ----------------
                    if (validarIVA && statciIVT01 == "*")
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_196", "Código de IVA inactivo"), "D", "Iva");
                        result = false;
                    }

                    //------------- Número de documento de iva no puede ser mayor que 9999999  ----------------
                    if (validarIVA && numeroDoc2Num > 9999999 && nddomcGLM03.Trim() == "R")
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_197", "El número de documento de IVA no puede ser mayor que 9999999"), "D", "Iva");
                        result = false;
                    }

                    //------------- El código de iva tiene asignada otra cuenta de mayor  ----------------
                    if (validarIVA && cuenciIVT01.Trim() != "")
                    {
                        string ctaMayorUltimoNivel = CtaMayorUltimoNivel(tPlan, cuenta);
                        if (ctaMayorUltimoNivel.ToString() != cuenciIVT01.Trim())
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_198", "El código de IVA tiene asignada otra cuenta de mayor"), "D", "Iva");
                            result = false;
                        }
                    }    

                    //------------- No puede ser clase de comprobante 2  ----------------
                    if (validarIVA && this._cab_clase == "2")
                    {
                        this.DSErroresAdd(linea, this.LP.GetText("error_200", "El comprobante de clase 2 no admite IVA"), "D", "Iva");
                        result = false;
                    }

                    if (validarIVA)
                    {
                        //------------- Año/mes cerrado  ----------------
                        query = "select ULMCT3 from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                        query += "where CIAFT3 = '" + ciaft2IVT02 + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            ulmct3IVT03 = dr.GetValue(dr.GetOrdinal("ULMCT3")).ToString();
                        }
                        else
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_192", "Compañía no definida en la tabla de compañías fiscales"), "D", "");
                            result = false;
                            validarIVA = false;
                        }
                        dr.Close();

                        if (validarIVA)
                        {
                            string fecCon = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this._cab_fecha), true).ToString();
                            string saammCon = fecCon.Substring(0, 5);
                            if (Convert.ToInt32(saammCon) <= Convert.ToInt32(ulmct3IVT03))
                            {
                                this.DSErroresAdd(linea, this.LP.GetText("error_201", "Año/mes cerrado para movimientos de IVA"), "D", "Iva");
                                result = false;
                            }
                        }

                        //------------- Cuadre de IVA, IVT01-IVTX1  ----------------
                        //buscar en ivtx1 los datos de la fecha anterior mas proxima a la fecha de documento (si fecha documento = 0 tomar fecha comprobante)
                        //si no existe el regitro o la tabla se toman los valores de ivt01
                        //solo validar cuadre si cuadciIVT01 = S
                        if (validarIVA && cuadciIVT01 == "S")
                        {
                            string fechaDocIVTX1 = fecha;
                            if (fechaDocIVTX1 == "") fechaDocIVTX1 = this._cab_fecha;
                            fechaDocIVTX1 = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(fechaDocIVTX1), true).ToString();

                            query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTX1 ";
                            query += "where TIPLCX = '" + tPlan + "' and COIVCX = '" + iva + "' and ";
                            query += "FEIVCX <= " + fechaDocIVTX1 + " ORDER BY FEIVCX DESC";

                            dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                            if (dr.Read())
                            {
                                //datos IVTX1
                                tpivciIVT01 = dr.GetValue(dr.GetOrdinal("TPIVCX")).ToString();
                                recgciIVT01 = dr.GetValue(dr.GetOrdinal("RECGCX")).ToString();
                            }
                            else
                            {
                                //No se encuentra tabla o registro, dejamos los datos del IVT01
                            }
                            dr.Close();

                            decimal tpivciIVT01Dec = 0;
                            if (tpivciIVT01 != "") tpivciIVT01Dec = Convert.ToDecimal(tpivciIVT01);

                            decimal recgciIVT01Dec = 0;
                            if (recgciIVT01 != "") recgciIVT01Dec = Convert.ToDecimal(recgciIVT01);

                            decimal mxajciIVT01Dec = 0;
                            if (mxajciIVT01 != "") mxajciIVT01Dec = Convert.ToDecimal(mxajciIVT01);
                            
                            if ((((importe3Dec * tpivciIVT01Dec) / 100) + ((importe3Dec * recgciIVT01Dec) / 100) < monedaLocalDec - mxajciIVT01Dec) ||
                                (((importe3Dec * tpivciIVT01Dec) / 100) + ((importe3Dec * recgciIVT01Dec) / 100) > monedaLocalDec + mxajciIVT01Dec))
                            {
                                this.DSErroresAdd(linea, this.LP.GetText("error_206", "Superado máximo admitido para ajuste redondeo"), "D", "Importe3");
                                result = false;
                            }
                        }

                        cifDni = cifDni.Trim();
                        //------------- CIF/DNI, falta  ----------------
                        if (rnitmcGLM03 == "R" && cifDni == "")
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_202", "Falta el número de identificación tributaria"), "D", "CifDni");
                            result = false;
                        }
                    }
                }

                //------------- CIF/DNI, no lleva  ----------------
                if (rnitmcGLM03 == "N" && cifDni != "")
                {
                    this.DSErroresAdd(linea, this.LP.GetText("error_203", "Este asiento no lleva número de identificación tributaria"), "D", "CifDni");
                    result = false;
                }

                //If rnitmcGLM03 = "R" And Trim(det.sCif) <> "" And Trim(det.sIva) = "" Then
                //    ValidarDetalle = Not RC_OK
                //    oMultiIdioma.ShowMsg 203, vbCritical, Me.Caption
                //    Exit Function
                //End If

                //------------- DIF/DNI incorrecto, solo verificar si CHKCRC de GLC01 es igual a 1 y IVA esta informado  ----------------
                //------------- Obtener Valores de glc01  ----------------
                if (CGParametrosGrles.GLC01_CHKCRC == "" || CGParametrosGrles.GLC01_CHKCRC == null) chkcrcGLC01 = 0;
                else chkcrcGLC01 = Convert.ToInt32(CGParametrosGrles.GLC01_CHKCRC);

                if (chkcrcGLC01 == 1 && iva != "")
                {
                    //------------- validar si es un NIF español correcto  ----------------
                    bool nifCorrecto = false;
                    string resultValidarNIF = "";
                    if (CheckNif.Check(cifDni, ref resultValidarNIF)) nifCorrecto = true;

                    if (!nifCorrecto)
                    {
                        //------------- buscar en IVM05  ----------------
                        query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                        query += "where NITRCF = '" + cifDni + "'";

                        registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                        if (registros == 0)
                        {
                            //------------- si no se encuentra en IVM05  ----------------
                            if (tauxciIVT01 != "")
                            {
                                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                                query += "where TAUXMA = '" + tauxciIVT01 + "' and NNITMA = '" + cifDni + "'";

                                registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                                if (registros == 0)
                                {
                                    this.DSErroresAdd(linea, this.LP.GetText("error_205", "CIF no encontrado en maestro de auxiliares ni en maestro de CIF/DNI"), "D", "CifDni");
                                    result = false;
                                }
                            }

                            //------------- si no se encuentra en IVM05  ----------------
                            if (tauxciIVT01 == "")
                            {
                                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                                query += "where NNITMA = '" + cifDni + "'";

                                registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                                if (registros == 0)
                                {
                                    this.DSErroresAdd(linea, this.LP.GetText("error_205", "CIF no encontrado en maestro de auxiliares ni en maestro de CIF/DNI"), "D", "CifDni");
                                    result = false;
                                }
                            }
                        }
                    }
                }

                //Validaciones de campos extendidos
                if (this._cab_extendido == "1")
                {
                    //Leer valores de los campos extendidos
                    prefijoDoc = row["PrefijoDoc"].ToString().ToUpper();
                    numFactAmp = row["NumFactAmp"].ToString();
                    numFactRectif = row["NumFactRectif"].ToString();
                    fechaServIVA = row["FechaServIVA"].ToString();
                    campoUserAlfa1 = row["CampoUserAlfa1"].ToString();
                    campoUserAlfa2 = row["CampoUserAlfa2"].ToString();
                    campoUserAlfa3 = row["CampoUserAlfa3"].ToString();
                    campoUserAlfa4 = row["CampoUserAlfa4"].ToString();
                    campoUserAlfa5 = row["CampoUserAlfa5"].ToString();
                    campoUserAlfa6 = row["CampoUserAlfa6"].ToString();
                    campoUserAlfa7 = row["CampoUserAlfa7"].ToString();
                    campoUserAlfa8 = row["CampoUserAlfa8"].ToString();
                    campoUserNum1 = row["CampoUserNum1"].ToString();
                    campoUserNum2 = row["CampoUserNum2"].ToString();
                    campoUserFecha1 = row["CampoUserFecha1"].ToString();
                    campoUserFecha2 = row["CampoUserFecha2"].ToString();

                    //------------- datos de GLMX3  ----------------
                    bool hayAmpliadosCuenta = true;

                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX = '" + tPlan + "' and CUENMX = '" + CtaMayorUltimoNivel(tPlan, cuenta) + "'";

                    string FGPRMX = "";
                    string FGFAMX = "";
                    string FGFRMX = "";
                    string FGDVMX = "";
                    string FG01MX = "";
                    string FG02MX = "";
                    string FG03MX = "";
                    string FG04MX = "";
                    string FG05MX = "";
                    string FG06MX = "";
                    string FG07MX = "";
                    string FG08MX = "";
                    string FG09MX = "";
                    string FG10MX = "";
                    string FG11MX = "";
                    string FG12MX = "";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        FGPRMX = dr.GetValue(dr.GetOrdinal("FGPRMX")).ToString().Trim();
                        FGFAMX = dr.GetValue(dr.GetOrdinal("FGFAMX")).ToString().Trim();
                        FGFRMX = dr.GetValue(dr.GetOrdinal("FGFRMX")).ToString().Trim();
                        FGDVMX = dr.GetValue(dr.GetOrdinal("FGDVMX")).ToString().Trim();
                        FG01MX = dr.GetValue(dr.GetOrdinal("FG01MX")).ToString().Trim();
                        FG02MX = dr.GetValue(dr.GetOrdinal("FG02MX")).ToString().Trim();
                        FG03MX = dr.GetValue(dr.GetOrdinal("FG03MX")).ToString().Trim();
                        FG04MX = dr.GetValue(dr.GetOrdinal("FG04MX")).ToString().Trim();
                        FG05MX = dr.GetValue(dr.GetOrdinal("FG05MX")).ToString().Trim();
                        FG06MX = dr.GetValue(dr.GetOrdinal("FG06MX")).ToString().Trim();
                        FG07MX = dr.GetValue(dr.GetOrdinal("FG07MX")).ToString().Trim();
                        FG08MX = dr.GetValue(dr.GetOrdinal("FG08MX")).ToString().Trim();
                        FG09MX = dr.GetValue(dr.GetOrdinal("FG09MX")).ToString().Trim();
                        FG10MX = dr.GetValue(dr.GetOrdinal("FG10MX")).ToString().Trim();
                        FG11MX = dr.GetValue(dr.GetOrdinal("FG11MX")).ToString().Trim();
                        FG12MX = dr.GetValue(dr.GetOrdinal("FG12MX")).ToString().Trim();

                        //en el caso de que la cuenta sea de IVA y no se informe el codigo de IVA
                        //no validar la obligatoriedad de los  campos ampliados.
                        //If(Trim(det.sIva) = "") And(rnitmcGLM03 = "R") Then
                        if ((iva.Trim() == "") && (rnitmcGLM03 == "R"))
                        {
                            if (FGPRMX == "1") FGPRMX = "2";
                            if (FGFAMX == "1") FGFAMX = "2";
                            if (FGFRMX == "1") FGFRMX = "2";
                            if (FGDVMX == "1") FGDVMX = "2";
                            if (FG01MX == "1") FG01MX = "2";
                            if (FG02MX == "1") FG02MX = "2";
                            if (FG03MX == "1") FG03MX = "2";
                            if (FG04MX == "1") FG04MX = "2";
                            if (FG05MX == "1") FG05MX = "2";
                            if (FG06MX == "1") FG06MX = "2";
                            if (FG07MX == "1") FG07MX = "2";
                            if (FG08MX == "1") FG08MX = "2";
                            if (FG09MX == "1") FG09MX = "2";
                            if (FG10MX == "1") FG10MX = "2";
                            if (FG11MX == "1") FG11MX = "2";
                            if (FG12MX == "1") FG12MX = "2";
                        }
                        
                    }
                    else hayAmpliadosCuenta = false;
                    dr.Close();
                
                    //verificar que la cuenta no tiene campos ampliados
                    if (prefijoDoc.Trim() != "" || numFactAmp.Trim() != "" || numFactRectif.Trim() != "" || 
                        (fechaServIVA.Trim() != "" && fechaServIVA != "0") ||
                        campoUserAlfa1.Trim() != "" || campoUserAlfa2.Trim() != "" || campoUserAlfa3.Trim() != "" || 
                        campoUserAlfa4.Trim() != "" || campoUserAlfa5.Trim() != "" || campoUserAlfa6.Trim() != "" || 
                        campoUserAlfa7.Trim() != "" || campoUserAlfa8.Trim() != "" || 
                        (campoUserNum1.Trim() != "" && campoUserNum1 != "0") ||
                        (campoUserNum2.Trim() != "" && campoUserNum2 != "0") ||
                        (campoUserFecha1.Trim() != "" && campoUserFecha1 != "0") ||
                        (campoUserFecha1.Trim() != "" && campoUserFecha1 != "0"))
                    {
                        if (!hayAmpliadosCuenta)
                        {
                            this.DSErroresAdd(linea, this.LP.GetText("error_210", "La cuenta no tiene informados campos ampliados"), "D", "Cuenta");
                            result = false;
                        }
                    }

                    //salir si no hay definidos ampliados para la cuenta
                    if (!hayAmpliadosCuenta) return (result);

                    bool resultVal = true;
                    string literalCampo = ""; 

                    //1-prefijo documento
                    literalCampo = this.LP.GetText("lblfrmCompContdgPrefijoDoc", "Prefijo documento");
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FGPRMX, prefijoDoc, literalCampo, "PrefijoDoc", false);
                    if (!resultVal) result = resultVal;

                    //2-Numero de factura ampliada
                    literalCampo = this.LP.GetText("lblfrmCompContdgNumFactAmp", "Número Factura Ampliado");
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FGFAMX, numFactAmp, literalCampo, "NumFactAmp", false);
                    if (!resultVal) result = resultVal;

                    //3-Numero de factura rectificada
                    literalCampo = this.LP.GetText("lblfrmCompContdgNumFactRectif", "Número Factura Rectificada");
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FGFRMX, numFactRectif, literalCampo, "NumFactRectif", false);
                    if (!resultVal) result = resultVal;
                    
                    //4-Fecha servicio
                    literalCampo = this.LP.GetText("lblfrmCompContdgFechaServIVA", "Fecha de Servicio");
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FGDVMX, fechaServIVA, literalCampo, "FechaServIVA", true);
                    if (!resultVal) result = resultVal;

                    //5-Alfa 1 (15)
                    resultVal = this.VerificarCampoAlfa(linea, FG01MX, campoUserAlfa1, NM01PX, "CampoUserAlfa1", MX01PX, TA01PX, false);
                    if (!resultVal) result = resultVal;
   
                    //6-Alfa 2 (15)
                    resultVal = this.VerificarCampoAlfa(linea, FG02MX, campoUserAlfa2, NM02PX, "CampoUserAlfa2", MX02PX, TA02PX, false);
                    if (!resultVal) result = resultVal;

                    //7-Alfa 3 (15)
                    resultVal = this.VerificarCampoAlfa(linea, FG03MX, campoUserAlfa3, NM03PX, "CampoUserAlfa3", MX03PX, TA03PX, false);
                    if (!resultVal) result = resultVal;

                    //8-Alfa 4 (15)
                    resultVal = this.VerificarCampoAlfa(linea, FG04MX, campoUserAlfa4, NM04PX, "CampoUserAlfa4", MX04PX, TA04PX, false);
                    if (!resultVal) result = resultVal;

                    //9-Alfa 5 (25)
                    resultVal = this.VerificarCampoAlfa(linea, FG05MX, campoUserAlfa5, NM05PX, "CampoUserAlfa5", MX05PX, TA05PX, false);
                    if (!resultVal) result = resultVal;

                    //10-Alfa 6 (25)
                    resultVal = this.VerificarCampoAlfa(linea, FG06MX, campoUserAlfa6, NM06PX, "CampoUserAlfa6", MX06PX, TA06PX, false);
                    if (!resultVal) result = resultVal;

                    //11-Alfa 7 (25)
                    resultVal = this.VerificarCampoAlfa(linea, FG07MX, campoUserAlfa7, NM07PX, "CampoUserAlfa7", MX07PX, TA07PX, false);
                    if (!resultVal) result = resultVal;

                    //12-Alfa 8 (25)
                    resultVal = this.VerificarCampoAlfa(linea, FG08MX, campoUserAlfa8, NM08PX, "CampoUserAlfa8", MX08PX, TA08PX, false);
                    if (!resultVal) result = resultVal;

                    //13-Numerico 1
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FG09MX, campoUserNum1, NM09PX, "CampoUserNum1", true);
                    if (!resultVal) result = resultVal;

                    //14-Numerico 2
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FG10MX, campoUserNum2, NM10PX, "CampoUserNum2", true);
                    if (!resultVal) result = resultVal;

                    //15-Fecha 1
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FG11MX, campoUserFecha1, NM11PX, "CampoUserFecha1", true);
                    if (!resultVal) result = resultVal;

                    //16-Fecha 2
                    resultVal = this.VerificarCampoConValorNoBlanco(linea, FG12MX, campoUserFecha2, NM12PX, "CampoUserFecha2", true);
                    if (!resultVal) result = resultVal;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

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
                        for (int i = 0; i <= grupmaGLM05.Length-1; i++)
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
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                this.DSErroresAdd(linea, ex.Message, "D", campoAux);
                result = false;
            }

            return (result);
        }

        /// <summary>
        /// Verifica los campos extendidos (los 4 primeros fijos y los de tipo numérico y fecha)
        /// </summary>
        /// <param name="linea">línea de detalle</param>
        /// <param name="valorCampoTabla">valor campo en la bbdd</param>
        /// <param name="valorCampoGrid">valor en la línea del detalle de la columna que se está validando</param>
        /// <param name="literalCampo">nombre del campo</param>
        /// <param name="nombreColumnaGrid">nombre de la columna de la grid</param>
        /// <param name="checkNumerico">si es un valor numérico o no</param>
        /// <returns></returns>
        private bool VerificarCampoConValorNoBlanco(int linea, string valorCampoTabla, string valorCampoGrid, string literalCampo, string nombreColumnaGrid, bool checkNumerico)
        {
            bool result = true;

            bool error = false;
            //No debe tener valor
            if (valorCampoTabla == "0" && valorCampoGrid != "")
            {
                if (checkNumerico)
                {
                    try
                    {
                        decimal valorCampoGridNum = Convert.ToDecimal(valorCampoGrid);

                        if (valorCampoGridNum != 0) error = true;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else error = true;

                if (error)
                {
                    this.DSErroresAdd(linea, literalCampo + " " + this.LP.GetText("error_211", "El valor debe estar en blanco"), "D", nombreColumnaGrid);
                    result = false;
                }
            }

            error = false;

            //Debe tener valor
            if (valorCampoTabla == "1")
            {
                if (checkNumerico)
                {
                    if (valorCampoGrid == "") error = true;
                    else 
                    {
                        try
                        {
                            decimal valorCampoGridNum = Convert.ToDecimal(valorCampoGrid);

                            if (valorCampoGridNum == 0) error = true;
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                }
                else
                    if (valorCampoGrid == "") error = true;

                if (error)
                {
                    this.DSErroresAdd(linea, literalCampo + " " + this.LP.GetText("error_212", "El valor no puede estar en blanco"), "D", nombreColumnaGrid);
                    result = false;
                }
            }

            return (result);
        }

        /// <summary>
        /// Verifica los campos extendidos alfanuméricos
        /// </summary>
        /// <param name="linea">línea de detalle</param>
        /// <param name="valorCampoTabla">valor campo en la bbdd</param>
        /// <param name="valorCampoGrid">valor en la línea del detalle de la columna que se está validando</param>
        /// <param name="literalCampo">nombre del campo</param>
        /// <param name="nombreColumnaGrid">nombre de la columna de la grid</param>
        /// <param name="longitud">longitud del campo</param>
        /// <param name="TA0PX">valor en la bbdd del campo TA0?PX</param>
        /// <param name="checkNumerico">si es un valor numérico o no</param>
        /// <returns></returns>
        private bool VerificarCampoAlfa(int linea, string valorCampoTabla, string valorCampoGrid, string literalCampo, string nombreColumnaGrid, string longitud, string TA0PX, bool checkNumerico)
        {
            bool result = true;
            int longitudCampo;
            IDataReader dr = null;
            try
            {
                result = this.VerificarCampoConValorNoBlanco(linea, valorCampoTabla, valorCampoGrid, literalCampo, nombreColumnaGrid, checkNumerico);
                
                //Excede longitud maxima
                longitudCampo = Convert.ToInt16(longitud);
                if (valorCampoGrid.TrimEnd().Length > longitudCampo)
                {
                    this.DSErroresAdd(linea, literalCampo + " " + this.LP.GetText("error_213", "La longitud excede a la del campo de entrada"), "D", nombreColumnaGrid);    //oMultiIdioma.sGetText(623)
                    result = false;
                }

                //cuenta de auxiliar incorrecta
                if (TA0PX.Trim() != "" && valorCampoGrid.Trim() != "")
                {
                    string query = "select STATMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + TA0PX + "' and CAUXMA = '" + valorCampoGrid + "'";

                    string statmaGLM05 = "";
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        statmaGLM05 = dr.GetValue(dr.GetOrdinal("STATMA")).ToString().Trim();
                    }
                    else
                    {
                        dr.Close();
                        this.DSErroresAdd(linea, literalCampo + " " + this.LP.GetText("error_214", "Cuenta de auxiliar no existe"), "D", nombreColumnaGrid);
                        result = false; 
                    }
                    dr.Close();

                    //auxiliar no vigente
                    if (statmaGLM05 == "*")
                    {
                        this.DSErroresAdd(linea, literalCampo + " " + this.LP.GetText("error_215", "Cuenta de auxiliar inactiva"), "D", nombreColumnaGrid);
                        result = false; 
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }
        /// <summary>
        /// FALTA !!
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        private string CtaMayorUltimoNivel(string plan, string cuenta)
        {
            string result = cuenta;

            int[] nivelLongitud = this.utilesCG.ObtenerNivelLongitudDadoPlan(plan);

            int longitudPlanCuentas = nivelLongitud[1];

            if (longitudPlanCuentas > cuenta.Length) result = cuenta.PadRight(longitudPlanCuentas, '0');

            return (result);
        }

        /// <summary>
        /// A la tabla de errores del DataSet, le añade una línea
        /// </summary>
        /// <param name="linea">No. Fila donde hay error</param>
        /// <param name="error">Mensaje de error</param>
        /// <param name="codiTipo">C - cabecera    D - detalle</param>
        /// <param name="control_celda">Nombre del control donde se produce el error o columna del detalle</param>
        public void DSErroresAdd(int linea, string error, string codiTipo, string control_celda)
        {
            DataRow row = this.dsErrores.Tables["Errores"].NewRow();

            switch (codiTipo)
            {
                case "C":
                    row[0] = this.LP.GetText("lblCabecera", "Cabecera");   //FALTA traducir
                    row[1] = "";
                    break;
                case "T":
                    row[0] = this.LP.GetText("lblTotales", "Totales");   //FALTA traducir
                    row[1] = "";
                    break;
                case "D":
                default:
                    row[0] = this.LP.GetText("lblDetalle", "Detalle");                     //FALTA traducir
                    row[1] = linea.ToString();
                    break;
            }

            row[2] = error;
            row[3] = codiTipo;
            row[4] = control_celda;
            
            this.dsErrores.Tables["Errores"].Rows.Add(row);
        }
    }
}
