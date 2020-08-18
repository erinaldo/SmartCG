using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using ObjectModel;
using log4net;

namespace ModComprobantes
{
    /// <summary>
    /// Clase que contiene los datos de la cabecera del comprobante contable + los detalles de 1 fila para la tabla GLB01
    /// </summary>
    public class ComprobanteContCabeceraDetalle1FilaGLB01
    {
        public string tipoBaseDatosCG;              //Tipo bbdd (DB2, SQLServer, Oracle)
        public string cab_extendido;                //1 -> campos extendidos      0 -> no tiene campos extendidos
        public string cab_plan;                     //Plan de la compañía
        public string rowNumberPrimerComentario;    //Número de Fila del 1er comentario del comprobante
        
        public string cab_compania;         //CCIA
        public string cab_anoperiodo;       //SAPR
        public string cab_fecha;            //FECO
        public string cab_tipo;             //TICO
        public string cab_noComprobante;    //NUCO
        public string cab_descripcion;      //1er comentario

        //Campos GLI03
        public string cab_STAT;             //Estado
        public string cab_WSGE;             //No se utiliza - blanco
        public string cab_IDUS;             //Usuario logado app
        public string cab_TVOU;             //Clase
        public string cab_DEBE;             //Total Debe Moneda Local
        public string cab_HABE;             //Total Habe Moneda Local
        public string cab_DEME;             //Total Debe Moneda Extranjera
        public string cab_HAME;             //Total Haber Moneda Extranjera
        public string cab_TASC;             //Tasa
        public string cab_TIMO;             //No se utiliza - blanco
        public string cab_SIMI;             //Número de Lineas del Detalle del comprobante
        public string cab_DOCR;             //No se utiliza - blanco

        public string cab_DEBE_format;
        public string cab_HABE_format;
        public string cab_DEME_format;
        public string cab_HAME_format;
        public string cab_TASC_format;

        public int ultimoSIMIIC;

        //Campos GLB01
        public string det_STAT;
        public string det_CUEN;
        public string det_TAUX;
        public string det_CAUX;
        public string det_TMOV;
        public string det_NIVE;
        public string det_CLDO;
        public string det_NDOC;
        public string det_FDOC;
        public string det_FEVE;
        public string det_COBA;
        public string det_TEIN;
        public string det_SECH;
        public string det_MONT;
        public string det_SLML;
        public string det_UTIL;
        public string det_DESC;
        public string det_NNIT;
        public string det_TAAD01;
        public string det_AUAD01;
        public string det_TAAD02;
        public string det_AUAD02;
        public string det_CDDO;
        public string det_NDDO;
        public string det_MOSM;
        public string det_TERC;
        public string det_CDIV;
        public string det_SIMI;
        public string det_PCIF;
        public string det_G3NC;

        public string det_RowNumber;
        
        public string det_MONT_format;
        public decimal det_MONT_decimal;
        public string det_MOSM_format;
        public decimal det_MOSM_decimal;
        public string det_TERC_format;
        public decimal det_TERC_decimal;

        public string det_CUEN_UltNivel;

        //Campos GLBX1 (Extendidos)
        public string det_ext_PRFD;
        public string det_ext_NFAA;
        public string det_ext_NFAR;
        public string det_ext_FIVA;
        public string det_ext_USA1;
        public string det_ext_USA2;
        public string det_ext_USA3;
        public string det_ext_USA4;
        public string det_ext_USA5;
        public string det_ext_USA6;
        public string det_ext_USA7;
        public string det_ext_USA8;
        public string det_ext_USN1;
        public string det_ext_USN2;
        public string det_ext_USF1;
        public string det_ext_USF2;

        public string cab_anoperiodoInicial;
        public string cab_tipoInicial;
        public string cab_noComprobanteInicial;
        private frmCompContListaGLB01 compcontlistaGLB01;
        private readonly Autorizaciones aut;
        private Utiles utiles;
        private UtilesCGConsultas utilesCG;
        private LanguageProvider LP;
        private ILog Log;

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

        public ComprobanteContCabeceraDetalle1FilaGLB01()
        {
            this.compcontlistaGLB01 = new frmCompContListaGLB01();
            this.aut = new Autorizaciones();
            this.utiles = new Utiles();
            this.utilesCG = new UtilesCGConsultas();
            this.Log = log4net.LogManager.GetLogger(this.GetType());

            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Campos fijos
            this.cab_WSGE = " ";
            this.cab_IDUS = GlobalVar.UsuarioLogadoCG;
            this.cab_TIMO = " ";
            this.cab_DOCR = " ";
        }

        #region Métodos públicos
        /// <summary>
        /// Inserta el comprobante en la tabla cabecera (GLI03)
        /// </summary>
        /// <returns></returns>
        public string InsertarComprobanteTablaCabeceraGLI03(DataRow rowCabecera)
        {
            string result = "";

            try
            {
                //Insertar cabecera del comprobantes
                string STAT = "V";

                string query = "insert into " + GlobalVar.PrefijoTablaCG + "GLI03 (STATIC, CCIAIC, SAPRIC, TICOIC, NUCOIC, FECOIC, WSGEIC, ";
                query += "IDUSIC, TVOUIC, DEBEIC, HABEIC, DEMEIC, HAMEIC, TASCIC, TIMOIC, SIMIIC, DOCRIC) values ('";
                query += STAT + "', '" + this.cab_compania + "', " + this.cab_anoperiodo + ", ";
                query += this.cab_tipo + ", " + this.cab_noComprobante + ", ";
                query += this.cab_fecha + ", '" + this.cab_WSGE + "', '" + this.cab_IDUS + "', '";
                query += rowCabecera["Clase"].ToString() + "', " + this.cab_DEBE_format + ", ";
                query += this.cab_HABE_format + ", " + this.cab_DEME_format + ", " + this.cab_HAME_format + ", " + this.cab_TASC_format + ", '";
                query += this.cab_TIMO + "', " + this.cab_SIMI + ", '" + this.cab_DOCR + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando la cabecera del comprobante (" + ex.Message + ")";
            }
            
            return (result);
        }

        /// <summary>
        /// Actualiza la tabla cabecera del comprobante (GLI03)
        /// </summary>
        /// <returns></returns>
        public string ActualizarComprobanteTablaCabeceraGLI03(DataRow rowCabecera, DataRow rowTotales)
        {
            string result = "";

            try
            {
                //Campos decimales, reemplazar comas por puntos
                //this.ReemplazarCamposCabFormatoComasPunto();

                //Actualizar cabecera del comprobante
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLI03 set ";
                query += "SAPRIC = " + this.cab_anoperiodo + ", TICOIC = " + rowCabecera["Tipo"].ToString() + ", NUCOIC = " + rowCabecera["Numero"].ToString() + ", ";
                query += "FECOIC = " + this.cab_fecha + ", IDUSIC = '" + this.cab_IDUS + "', TVOUIC = '" + rowCabecera["Clase"].ToString() + "', ";
                query += "DEBEIC = " + this.cab_DEBE_format + ", HABEIC = " + this.cab_HABE_format + ", DEMEIC = " + this.cab_DEME_format + ", ";
                query += "HAMEIC = " + this.cab_HAME_format + ", TASCIC = " + this.cab_TASC_format + ", SIMIIC = " + this.cab_SIMI;
                query += " where ";
                query += "CCIAIC = '" + rowCabecera["Compania"].ToString() + "' and ";
                //query += "SAPRIC = " + this.cab_anoperiodoInicial + " and ";
                query += "SAPRIC = " + this.cab_anoperiodo + " and ";
                query += "TICOIC = " + this.cab_tipoInicial + " and ";
                query += "NUCOIC = " + this.cab_noComprobanteInicial ;

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando la cabecera del comprobante (" + ex.Message + ")";
            }
            
            return (result);
        }

        /// <summary>
        /// Inserta un registro en la tabla detalle del comprobante GLB01
        /// </summary>
        /// <returns></returns>
        public string InsertarDetalleComprobanteGLB01()
        {
            string result = "";
            try
            {
                string STAT = "V";
                string fdoc = "0";
                string feve = "0";


                // pongo 0 a los campos numéricos nulos.
                if (this.cab_anoperiodo == "") this.cab_anoperiodo = "0";
                //if (aa.Length >= 2) aa = aa.Substring(aa.Length - 2, 2);
                //Coger el siglo
                

                if (this.det_FDOC == "") this.det_FDOC = "0";
                fdoc = this.det_FDOC;
                //else
                //{
                //if (this.det_FDOC != "0")
                //{
                //string aa = this.det_FDOC.Substring(2, 2).ToString();
                //aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                //fdoc = aa + this.det_FDOC.Substring(4, 4).ToString();
                //}
                //}
                if (this.det_FEVE == "") this.det_FEVE = "0";
                else
                {
                    if (this.det_FEVE != "0" && this.det_FEVE.Length==8)
                    {
                        string aa = this.det_FEVE.Substring(2, 2).ToString();
                        aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                        feve = aa + this.det_FEVE.Substring(4, 4).ToString();
                    }
                    else
                    {
                        if (this.det_FEVE != "0" && this.det_FEVE.Length == 7)
                        { 
                            feve = this.det_FEVE;
                        }
                    }
                }
                if (this.det_TAUX == "" || this.det_TAUX is null)
                {
                    string[] DatosTipoAux = utilesCG.ObtenerTiposAuxiliarCuentaMayor(cab_plan, this.det_CUEN_UltNivel);
                    this.det_TAUX = DatosTipoAux[0];
                    this.det_TAAD01 = DatosTipoAux[1];
                    this.det_TAAD02 = DatosTipoAux[2];
                }

                //Insertar el detalle del comprobante
                string query = "insert into " + GlobalVar.PrefijoTablaCG + "GLB01 (STATDT,TIPLDT,CCIADT,CUENDT,TAUXDT,CAUXDT,";
                query += "TMOVDT,SAPRDT,TICODT,NUCODT,FECODT,NIVEDT,CLDODT,NDOCDT,FDOCDT,FEVEDT,COBADT,TEINDT,SECHDT,MONTDT,SLMLDT,";
                query += "UTILDT,DESCAD,NNITAD,TAAD01,AUAD01,TAAD02,AUAD02,CDDOAD,NDDOAD,MOSMAD,TERCAD,CDIVDT,SIMIDT,PCIFAD,G3NCDT) values ('";
                query += STAT + "', '" + this.cab_plan + "', '" + this.cab_compania + "', '" + this.det_CUEN_UltNivel + "', '" + this.det_TAUX + "', '" + this.det_CAUX + "', '";
                query += this.det_TMOV + "', " + this.cab_anoperiodo + ", " + this.cab_tipo + ", " + this.cab_noComprobante + ", " + this.cab_fecha + ", ";
                //query += this.det_NIVE + ", '" + this.det_CLDO + "', " + this.det_NDOC + ", " + this.det_FDOC + ", " + this.det_FEVE + ", '" + this.det_COBA + "', '";
                query += this.det_NIVE + ", '" + this.det_CLDO + "', " + this.det_NDOC + ", " + fdoc + ", " + feve + ", '" + this.det_COBA + "', '";
                query += this.det_TEIN + "', '" + this.det_SECH + "', " + this.det_MONT_format + ", ";
                query += this.det_SLML + ", '" + this.det_UTIL + "', '";
                query += this.det_DESC + "', '" + this.det_NNIT + "', '" + this.det_TAAD01 + "', '" + this.det_AUAD01 + "', '" + this.det_TAAD02 + "', '";
                query += this.det_AUAD02 + "', '" + this.det_CDDO + "', " + this.det_NDDO +  ", " + this.det_MOSM_format + ", " + this.det_TERC_format + ", '";
                query += this.det_CDIV + "', " + this.det_SIMI + ", '" + this.det_PCIF + "', '" + this.det_G3NC + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string aappFormat = this.cab_anoperiodo;
                if (aappFormat.Length == 5) aappFormat = aappFormat.Substring(1, 4);
                if (aappFormat.Length == 4) aappFormat = aappFormat.Substring(0, 2) + "-" + aappFormat.Substring(2, 2);

                result = this.LP.GetText("errInsertDetComp", "Error insertando el detalle del comprobante") + " (CCIA: " + this.cab_compania + " AAPP: ";
                result += aappFormat + " Tipo: " + this.cab_tipo + " NUCO: " + this.cab_noComprobante + ") (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Inserta el detalle en la tabla de detalles de campos extendidos GLBX1
        /// </summary>
        /// <returns></returns>
        public string InsertarDetalleComprobanteExtGLBX1()
        {
            string result = "";
            string usf1dx = "0";
            string usf2dx = "0";
            try
            {
                if (this.det_ext_USF1 == "") this.det_ext_USF1 = "0";
                else
                {
                    if (this.det_ext_USF1 != "0")
                    {
                        string aa = this.det_ext_USF1.Substring(2, 2).ToString();
                        aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                        usf1dx = aa + this.det_ext_USF1.Substring(4, 4).ToString();
                    }
                }

                if (this.det_ext_USF2 == "") this.det_ext_USF2 = "0";
                else
                {
                    if (this.det_ext_USF2 != "0")
                    {
                        string aa = this.det_ext_USF2.Substring(2, 2).ToString();
                        aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                        usf2dx = aa + this.det_ext_USF2.Substring(4, 4).ToString();
                    }
                }
                
                //Insertar el detalle del comprobante para los campos extendidos
                string query = "insert into " + GlobalVar.PrefijoTablaCG + "GLBX1 (CCIADX,CUENDX,TAUXDX,CAUXDX,SAPRDX,TICODX,NUCODX,";
                query += "SIMIDX,CLDODX,NDOCDX,NNITDX,PRFDDX,NFAADX,NFARDX,FIVADX,USA1DX,USA2DX,USA3DX,USA4DX,USA5DX,USA6DX,USA7DX,USA8DX,";
                query += "USN1DX,USN2DX,USF1DX,USF2DX) values ('";
                query += this.cab_compania + "', '" + this.det_CUEN_UltNivel + "', '" + this.det_TAUX + "', '" + this.det_CAUX + "', " + this.cab_anoperiodo + ", ";
                query += this.cab_tipo + ", " + this.cab_noComprobante + ", " + this.det_SIMI + ", '" + this.det_CLDO + "', " + this.det_NDOC + ", '";
                query += this.det_NNIT + "', '" + this.det_ext_PRFD + "', '" + this.det_ext_NFAA + "', '" + this.det_ext_NFAR + "', " + this.det_ext_FIVA + ", '";
                query += this.det_ext_USA1 + "', '" + this.det_ext_USA2 + "', '" + this.det_ext_USA3 + "', '" + this.det_ext_USA4 + "', '" + this.det_ext_USA5 + "', '";
                query += this.det_ext_USA6 + "', '" + this.det_ext_USA7 + "', '" + this.det_ext_USA8 + "', " + this.det_ext_USN1 + ", ";
                // query += this.det_ext_USN2 + ", " + this.det_ext_USF1 + ", " + this.det_ext_USF2 + ")"; jl
                query += this.det_ext_USN2 + ", " + usf1dx + ", " + usf2dx + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string aappFormat = this.cab_anoperiodo;
                if (aappFormat.Length == 5) aappFormat = aappFormat.Substring(1, 4);
                if (aappFormat.Length == 4) aappFormat = aappFormat.Substring(0, 2) + "-" + aappFormat.Substring(2, 2);

                result = this.LP.GetText("errInsertDetComp", "Error insertando el detalle del comprobante") + " (CCIA: " + this.cab_compania + " AAPP: ";
                result += aappFormat + " Tipo: " + this.cab_tipo + " NUCO: " + this.cab_noComprobante + ") (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Actualiza un registro en la tabla detalle del comprobante GLB01
        /// </summary>
        /// <returns></returns>
        public string ActualizarDetalleComprobanteTablaGLB01()
        {
            string result = "";
            try
            {
                string fdoc = "0";
                string feve = "0";


                // pongo 0 a los campos numéricos nulos.
                if (this.cab_anoperiodo == "") this.cab_anoperiodo = "0";
                //if (aa.Length >= 2) aa = aa.Substring(aa.Length - 2, 2);
                //Coger el siglo


                if (this.det_FDOC == "" || this.det_FDOC == "0") this.det_FDOC = "0";
                fdoc = this.det_FDOC;
                //else
                //{

                //string aa = this.det_FDOC.Substring(2, 2).ToString();
                //aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                //fdoc = aa + this.det_FDOC.Substring(4, 4).ToString();

            //}
                if (this.det_FEVE == "" || this.det_FEVE=="0") this.det_FEVE = "0";
                feve = this.det_FEVE;
                //else
                //{
                //string aa = this.det_FEVE.Substring(2, 2).ToString();
                //aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                //feve = aa + this.det_FEVE.Substring(4, 4).ToString();
                //}

                if (this.det_TAUX == "" || this.det_TAUX is null)
                {
                    string[] DatosTipoAux = utilesCG.ObtenerTiposAuxiliarCuentaMayor(cab_plan, this.det_CUEN_UltNivel);
                    this.det_TAUX = DatosTipoAux[0];
                    this.det_TAAD01 = DatosTipoAux[1];
                    this.det_TAAD02 = DatosTipoAux[2];
                }

                //Actualizar el detalle del comprobante
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLB01 set CUENDT = '" + this.det_CUEN_UltNivel + "', TAUXDT  = '" + this.det_TAUX;
                query += "', CAUXDT = '" + this.det_CAUX + "', TMOVDT = '" + this.det_TMOV + "', SAPRDT = " + this.cab_anoperiodo + ", TICODT = " + this.cab_tipo;
                //query += ", NUCODT = " + this.cab_noComprobante + ", FDOCDT = " + this.cab_fecha + ", FEVEDT = " + this.det_FEVE;
                query += ", NUCODT = " + this.cab_noComprobante + ", FDOCDT = " + fdoc + ", FEVEDT = " + feve + ", FECODT = " + this.cab_fecha;
                query += ", TEINDT = '" + this.det_TEIN + "', MONTDT = " + this.det_MONT_format + ", DESCAD = '" + this.det_DESC;
                query += "', NNITAD = '" + this.det_NNIT + "', TAAD01 = '" + this.det_TAAD01 + "', AUAD01 = '" + this.det_AUAD01 + "', TAAD02  = '" + this.det_TAAD02;
                query += "', AUAD02 = '" + this.det_AUAD02 + "', CDDOAD  = '" + this.det_CDDO + "', NDDOAD = " + this.det_NDDO + ", MOSMAD = " + this.det_MOSM_format;
                query += ", TERCAD = " + this.det_TERC_format + ", CDIVDT = '" + this.det_CDIV + "', PCIFAD  = '" + this.det_PCIF + "' ";
                query += ", CLDODT = '" + this.det_CLDO + "', NDOCDT = " + this.det_NDOC;


                query += " Where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + GlobalVar.PrefijoTablaCG + "GLB01" + ") = " + this.det_RowNumber;
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = " + this.det_RowNumber;
                        break;
                    case "Oracle":
                        //id_prefijotabla_prefijolote + W01   o W11
                        string campoOracle = "ID_" + GlobalVar.PrefijoTablaCG + "GLB01";
                        query += campoOracle + " = " + this.det_RowNumber;
                        break;
                }

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errUpdateDetComp", "Error actualizando el detalle del comprobante") + " (CCIA: " + this.cab_compania;
                result += " AAPP: " + this.cab_anoperiodo + " Tipo: " + this.cab_tipo;      //Falta traducir
                result += " NUCO: " + this.cab_noComprobante + ") (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Actualiza el detalle en la tabla de detalles de campos extendidos GLBX1
        /// </summary>
        /// <returns></returns>
        public string ActualizarDetalleComprobanteExtTablaGLBX1()
        {
            string result = "";
            Int32 simi = 1;
            string resultInsertarGLBX1 = "";
            if (this.det_CLDO == "") this.det_CLDO = " ";
            try
            {
                simi = CalcularSIMIDetalleComprobanteTablaGLB01(this.det_RowNumber);
                string usf1dx = "0";
                string usf2dx = "0";
                if (this.det_ext_USF1 == "") this.det_ext_USF1 = "0";
                else
                {
                    if (this.det_ext_USF1 != "0")
                    {
                        string aa = this.det_ext_USF1.Substring(2, 2).ToString();
                        aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                        usf1dx = aa + this.det_ext_USF1.Substring(4, 4).ToString();
                    }
                }

                if (this.det_ext_USF2 == "") this.det_ext_USF2 = "0";
                else
                {
                    if (this.det_ext_USF2 != "0")
                    {
                        string aa = this.det_ext_USF2.Substring(2, 2).ToString();
                        aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;
                        usf2dx = aa + this.det_ext_USF2.Substring(4, 4).ToString();
                    }
                }

                //Actualizar el detalle del comprobante
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLBX1 set CUENDX = '" + this.det_CUEN_UltNivel + "', TAUXDX  = '" + this.det_TAUX;
                query += "', CAUXDX = '" + this.det_CAUX + "', SAPRDX = " + this.cab_anoperiodo + ", TICODX  = '" + this.cab_tipo + "', NUCODX = '" + this.cab_noComprobante;
                query += "', SIMIDX = '" + simi + "', CLDODX  = '" + this.det_CLDO + "', NDOCDX= " + this.det_NDOC + ", NNITDX = '" + this.det_NNIT;
                query += "', PRFDDX = '" + this.det_ext_PRFD + "', NFAADX = '" + this.det_ext_NFAA + "', NFARDX  = '" + this.det_ext_NFAR + "', FIVADX  = " + this.det_ext_FIVA;
                query += ", USA1DX = '" + this.det_ext_USA1 + "', USA2DX = '" + this.det_ext_USA2 + "', USA3DX = '" + this.det_ext_USA3 + "', USA4DX = '" + this.det_ext_USA4;
                query += "', USA5DX = '" + this.det_ext_USA5 + "', USA6DX = '" + this.det_ext_USA6 + "', USA7DX = '" + this.det_ext_USA7 + "', USA8DX = '" + this.det_ext_USA8;
                query += "', USN1DX = " + this.det_ext_USN1 + ", USN2DX = " + this.det_ext_USN2 + ", USF1DX = " + usf1dx + ", USF2DX = " + usf2dx;
                //query += " Where ";
                query += " Where CCIADX = '" + this.cab_compania + "' AND SAPRDX = " + this.cab_anoperiodo + " AND  TICODX = " + this.cab_tipo + " AND NUCODX = " + this.cab_noComprobante;
                // query += " AND SIMIDX = '" + this.det_SIMI + "'";
                query += " AND SIMIDX = '" + simi + "'";

                //switch (this.tipoBaseDatosCG)
                //{
                //case "DB2":
                //query += "RRN(" + GlobalVar.PrefijoTablaCG + "GLBX1" + ") = " + this.det_RowNumber;
                // break;
                //case "SQLServer":
                //query += "GERIDENTI = " + this.det_RowNumber;
                //break;
                //case "Oracle":
                //id_prefijotabla_prefijolote + W01   o W11
                //string campoOracle = "ID_" + GlobalVar.PrefijoTablaCG + "GLBX1";
                //query += campoOracle + " = " + this.det_RowNumber;
                //break;
                //}

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (registros == 0) resultInsertarGLBX1 = InsertarDetalleComprobanteExtGLBX1();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertDetComp", "Error actualizando el detalle del comprobante") + " (CCIA: " + this.cab_compania;
                result += " AAPP: " + this.cab_anoperiodo + " Tipo: " + this.cab_tipo + " NUCO: " + this.cab_noComprobante + ") (" + ex.Message + ")";
            }
            this.det_SIMI = "0";
            return (result);
        }
        /// </summary>
        /// <param name="plan">código del plan de cuentas de la compañía</param>
        /// <param name="plan">código de la cuenta de mayor</param>
        /// <returns></returns>
        public string ObtenerTipoAuxiliarCuentaAuxiliar(string CtaAux)
        {

            string result = "";
            IDataReader dr = null;
            try
            {
                string query = " select TAUXMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                query += "where CAUXMA ='" + CtaAux + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("TAUXMA")).ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }
        /// <summary>
        /// Buscar el identificador del registro del 1er comentario del comprobante
        /// </summary>
        /// <returns></returns>
        public string BuscarRowNumberPrimerComentarioCompTablaGLAI3()
        {
            string idPrimerComentario = "";
            IDataReader dr = null;
            try
            {
                //Coger la 1ra línea de comentarios
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLAI3"; 
                string query = "";
                string whereId = "";
                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "select RRN(" + nombreTabla + ") as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = "RRN(" + nombreTabla + ")";
                        break;
                    case "SQLServer":
                        query += "select GERIDENTI as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = "GERIDENTI";
                        break;
                    case "Oracle":
                        //id_prefijotabla + GLAI3
                        string campoOracle = "ID_" + nombreTabla;
                        query += "select " + campoOracle + " as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = campoOracle;
                        break;
                }

                query += " where CCIAAD='" + this.cab_compania + "' and ";
                query += "SAPRAD= " + this.cab_anoperiodo + " and ";
                query += "TICOAD= " + this.cab_tipo + " and ";
                query += "NUCOAD= " + this.cab_noComprobante;
                query += " order by id ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    idPrimerComentario = dr["id"].ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (idPrimerComentario);
        }

        /// <summary>
        /// Inserta un comentario en la tabla de comentarios del comprobante (GLAI3)
        /// </summary>
        /// <returns></returns>
        public string InsertarComentarioCompTablaGLAI3(string comentario)
        {
            string result = "";
            try
            {
                string query = "insert into " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                query += "(CCIAAD, SAPRAD, TICOAD, NUCOAD, COHEAD) values ('";
                query += this.cab_compania + "', " + this.cab_anoperiodo + ", ";
                query += this.cab_tipo + ", " + this.cab_noComprobante + ", '";
                query += comentario.PadRight(36, ' ');
                query += "')";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Actualiza el 1er comentario de la tabla de comentarios del comprobante (GLAI3)
        /// </summary>
        /// <returns></returns>
        public string ActualizaPrimerComentarioCompTablaGLAI3(string comentario)
        {
            string result = "";
            try
            {
                //Coger la 1ra línea de comentarios
                this.rowNumberPrimerComentario = this.BuscarRowNumberPrimerComentarioCompTablaGLAI3();

                if (this.rowNumberPrimerComentario != "")
                {
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLAI3";
                    string whereId = "";
                    switch (this.tipoBaseDatosCG)
                    {
                        case "DB2":
                            whereId = "RRN(" + nombreTabla + ")";
                            break;
                        case "SQLServer":
                            whereId = "GERIDENTI";
                            break;
                        case "Oracle":
                            //id_prefijotabla + GLAI3
                            string campoOracle = "ID_" + nombreTabla;
                            whereId = campoOracle;
                            break;
                    }

                    string query = "update " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                    query += "set COHEAD = '" + comentario.PadRight(36, ' ') + "' ";
                    query += "where CCIAAD='" + this.cab_compania + "' and ";
                    query += "SAPRAD= " + this.cab_anoperiodo + " and ";
                    query += "TICOAD= " + this.cab_tipo + " and ";
                    query += "NUCOAD= " + this.cab_noComprobante + " and ";
                    query += whereId + "= " + this.rowNumberPrimerComentario;

                    int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Actualiza datos de la cabecera del comprobante a partir de los datos del formulario
        /// </summary>
        /// <param name="rowCabecera"></param>
        /// <param name="rowTotales"></param>
        public void ActualizarCompCabeceraDesdeFormAltaEdita(DataRow rowCabecera, DataRow rowTotales)
        {          
            try
            {
                this.cab_extendido = rowCabecera["Extendido"].ToString();
                this.cab_compania = rowCabecera["Compania"].ToString();
                this.cab_tipo = rowCabecera["Tipo"].ToString();
                this.cab_noComprobante = rowCabecera["Numero"].ToString();

                //Buscar el plan de la compañía
                string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(this.cab_compania);
                this.cab_plan = datosCompaniaAct[1];

                this.cab_anoperiodo = rowCabecera["AnoPeriodo"].ToString().Replace("-", "");
                if (this.cab_anoperiodo != "" && this.cab_anoperiodo.Length < 5 && this.cab_anoperiodo.Length >= 2)
                {
                    //Siglo + Año + Periodo
                    this.cab_anoperiodo = utiles.SigloDadoAnno(this.cab_anoperiodo.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + this.cab_anoperiodo;
                }

                this.cab_fecha = rowCabecera["Fecha"].ToString();
                if (this.cab_fecha.Length == 8)
                {
                    this.cab_fecha = this.cab_fecha.Substring(6, 2) + "/" + this.cab_fecha.Substring(4, 2) + "/" + this.cab_fecha.Substring(0, 4);
                    this.cab_fecha = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(this.cab_fecha), true).ToString();
                }

                this.cab_DEBE_format = rowTotales["MonedaLocalDebe"].ToString();
                if (this.cab_DEBE_format == "") this.cab_DEBE_format = "0.00";
                else this.cab_DEBE_format = this.cab_DEBE_format.Replace(',', '.');
                this.cab_HABE_format = rowTotales["MonedaLocalHaber"].ToString();
                if (this.cab_HABE_format == "") this.cab_HABE_format = "0.00";
                else this.cab_HABE_format = this.cab_HABE_format.Replace(',', '.');
                this.cab_DEME_format = rowTotales["MonedaExtDebe"].ToString();
                if (this.cab_DEME_format == "") this.cab_DEME_format = "0.00";
                else this.cab_DEME_format = this.cab_DEME_format.Replace(',', '.');
                this.cab_HAME_format = rowTotales["MonedaExtHaber"].ToString();
                if (this.cab_HAME_format == "") this.cab_HAME_format = "0.00";
                else this.cab_HAME_format = this.cab_HAME_format.Replace(',', '.');

                this.cab_TASC_format = rowCabecera["Tasa"].ToString();
                if (this.cab_TASC_format == "") this.cab_TASC_format = "0.0000000";
                else this.cab_TASC_format = this.cab_TASC_format.Replace(',', '.');

                this.cab_SIMI = rowTotales["NumeroApuntes"].ToString();
                if (this.cab_SIMI == "") this.cab_SIMI = "0";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Dado una fila de la Grid de Detalle, actualiza los valores en los atributos del objeto de comprobante 1 fila
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public string ActualizarCompDetalleDesdeFilaForm(DataRow row)
        {
            string result = "";
            try
            {
                this.det_MOSM = row["MonedaExt"].ToString();
                this.det_MOSM_format = "0";
                this.det_MOSM_decimal = 0;
                if (this.det_MOSM != "")
                {
                    try
                    {
                        this.det_MOSM_decimal = Convert.ToDecimal(row["MonedaExt"]);
                        // this.det_MOSM_format = this.det_MOSM.Replace(",", ".");
                        this.det_MOSM_format = this.det_MOSM.Replace(".", " ").Replace(",", ".").Replace(" ", ""); //jl
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.det_MOSM_decimal = 0;
                    }
                }

                this.det_CUEN = row["Cuenta"].ToString().ToUpper();

                if (det_CUEN != "") this.det_CUEN_UltNivel = this.ObtenerCuentaUltimoNivel();
                else this.det_CUEN_UltNivel = "";

                this.det_CAUX = row["Auxiliar1"].ToString().ToUpper();
                this.det_DESC = row["Descripcion"].ToString();
                this.det_DESC = this.det_DESC.Replace("'", "''");

                this.det_MONT = row["MonedaLocal"].ToString();
                this.det_MONT_format = "0";
                this.det_MONT_decimal = 0;
                if (this.det_MONT != "")
                {
                    try 
                    { 
                        this.det_MONT_decimal = Convert.ToDecimal(row["MonedaLocal"]);
                        this.det_MONT_format = this.det_MONT.Replace(".", " ").Replace(",", ".").Replace(" ",""); //jl

                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.det_MONT_decimal = 0;
                    }
                }

                this.det_TMOV = row["DH"].ToString().ToUpper();

                this.det_CLDO = "";
                this.det_NDOC = "";
                string documento = "";
                try { documento = row["Documento"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (documento.Length > 2)
                {
                    this.det_CLDO = documento.Substring(0, 2).ToUpper();
                    this.det_NDOC = documento.Substring(2, documento.Length - 2);
                    //this.det_NDOC = documento.Substring(2, 7);
                    if (this.det_NDOC == "") this.det_NDOC = "0";
                }
                else
                {
                    this.det_CLDO = documento.ToUpper();
                    this.det_NDOC = "0";
                }

                this.det_FDOC = row["Fecha"].ToString();
                this.det_FEVE = row["Vencimiento"].ToString().Trim();
                
                DateTime fecha;
                try
                {
                    if (this.det_FDOC != "")
                    {
                        string fecha_fmt = this.det_FDOC.Substring(0, 4) + "/" +
                                           this.det_FDOC.Substring(4, 2) + "/" +
                                           this.det_FDOC.Substring(6, 2);
                        fecha = Convert.ToDateTime(fecha_fmt);
                        //fecha = Convert.ToDateTime(this.det_FDOC);
                        //this.det_FDOC = utiles.FechaToFormatoCG(fecha, false).ToString(); jl
                        this.det_FDOC = utiles.FechaToFormatoCG(fecha, true).ToString();
                    }
                    else this.det_FDOC = "0";
                    
                    if (this.det_FEVE != "")
                    {
                        string fecha_fmt = this.det_FEVE.Substring(0, 4) + "/" +
                                           this.det_FEVE.Substring(4, 2) + "/" +
                                           this.det_FEVE.Substring(6, 2);
                        fecha = Convert.ToDateTime(this.det_FEVE);
                        this.det_FEVE = utiles.FechaToFormatoCG(fecha, false).ToString();
                    }
                    else this.det_FEVE = "0";
                }

                 catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                this.det_TEIN = row["RU"].ToString().ToUpper();
                this.det_NNIT = row["CifDni"].ToString().ToUpper();
                this.det_AUAD01 = row["Auxiliar2"].ToString().ToUpper();
                this.det_AUAD02 = row["Auxiliar3"].ToString().ToUpper();

                this.det_CDDO = "";
                this.det_NDDO = "";

                try { documento = row["Documento2"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    documento = "";
                }

                if (documento.Length > 2)
                {
                    this.det_CDDO = documento.Substring(0, 2).ToString();
                    this.det_NDDO = documento.Substring(2, documento.Length - 2);
                    if (this.det_NDDO == "") this.det_NDDO = "0";
                }
                else
                {
                    this.det_CDDO = documento.ToString();
                    this.det_NDDO = "0";
                }

                this.det_TERC = row["Importe3"].ToString();
                this.det_TERC_format = "0";
                this.det_TERC_decimal = 0;
                if (this.det_TERC != "")
                {
                    try { 
                        this.det_TERC_decimal = Convert.ToDecimal(row["Importe3"]);
                        // this.det_TERC_format = this.det_TERC.Replace(",", "."); jl
                        this.det_TERC_format = this.det_TERC.Replace(".", " ").Replace(",", ".").Replace(" ", ""); //jl
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.det_TERC_decimal = 0;
                    }
                }

                this.det_CDIV = row["Iva"].ToString().ToUpper();

                //Campos que no se utilizan
                this.det_COBA = " ";
                this.det_SECH = " ";
                this.det_SLML = "0";
                this.det_UTIL = " ";
                this.det_G3NC = " ";
                this.det_NIVE = "0";

                if (this.cab_extendido == "1")
                {
                    this.det_ext_PRFD = row["PrefijoDoc"].ToString();
                    this.det_ext_NFAA = row["NumFactAmp"].ToString();
                    this.det_ext_NFAR = row["NumFactRectif"].ToString();
                    
                    this.det_ext_FIVA = row["FechaServIVA"].ToString().Trim();
                    if (this.det_ext_FIVA == "") this.det_ext_FIVA = "0";
                    else this.det_ext_FIVA = this.det_ext_FIVA.Replace("'", "''");
                    
                    this.det_ext_USA1 = row["CampoUserAlfa1"].ToString();
                    this.det_ext_USA2 = row["CampoUserAlfa2"].ToString();
                    this.det_ext_USA3 = row["CampoUserAlfa3"].ToString();
                    this.det_ext_USA4 = row["CampoUserAlfa4"].ToString();
                    this.det_ext_USA5 = row["CampoUserAlfa5"].ToString();
                    this.det_ext_USA6 = row["CampoUserAlfa6"].ToString();
                    this.det_ext_USA7 = row["CampoUserAlfa7"].ToString();
                    this.det_ext_USA8 = row["CampoUserAlfa8"].ToString();

                    this.det_ext_USN1 = row["CampoUserNum1"].ToString().Trim();
                    if (this.det_ext_USN1 == "") this.det_ext_USN1 = "0";
                    else this.det_ext_USN1 = this.det_ext_USN1.Replace("'", "''");
                    
                    this.det_ext_USN2 = row["CampoUserNum2"].ToString().Trim();
                    if (this.det_ext_USN2 == "") this.det_ext_USN2 = "0";
                    else this.det_ext_USN2 = this.det_ext_USN2.Replace("'", "''");

                    this.det_ext_USF1 = row["CampoUserFecha1"].ToString().Trim();
                    if (this.det_ext_USF1 == "") this.det_ext_USF1 = "0";
                    else this.det_ext_USF1 = this.det_ext_USF1.Replace("'", "''");

                    this.det_ext_USF2 = row["CampoUserFecha2"].ToString().Trim();
                    if (this.det_ext_USF2 == "") this.det_ext_USF2 = "0";
                    else this.det_ext_USF2 = this.det_ext_USF2.Replace("'", "''");
                }

                //Verificar si existen las columnas
                if (row.Table.Columns.Contains("RowNumber")) this.det_RowNumber = row["RowNumber"].ToString();
                if (row.Table.Columns.Contains("Pais")) this.det_PCIF = row["Pais"].ToString();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Inserta una línea de detalle del comprobante
        /// </summary>
        /// <returns></returns>
        public string InsertarComprobanteTablaDetalleGLB01()
        {
            string result = "";
            string resultInsertarGLB01 = "";
            string resultInsertarGLBX1 = "";

            try
            {
                //Insertar el detalle del comprobante
                resultInsertarGLB01 = InsertarDetalleComprobanteGLB01();

                if (resultInsertarGLB01 == "" && this.cab_extendido == "1")
                {
                    //Insertar detalle del comprobante campos extendidos
                    resultInsertarGLBX1 = InsertarDetalleComprobanteExtGLBX1();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //Dejar incrementado el valor del SIMI para la siguiente línea de detalle
            if (resultInsertarGLB01 == "")
            {
                try
                {
                    Int32 simiActual = Convert.ToInt32(this.det_SIMI);
                    this.det_SIMI = (simiActual++).ToString();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }

            result = resultInsertarGLB01 + resultInsertarGLBX1;

            return (result);
        }

        /// <summary>
        /// Devuelve el simi ultimo utilizado + 1 del detalle del comprobante
        /// </summary>
        /// <returns></returns>
        public Int32 CalcularSIMIDetalleComprobanteTablaGLM01()
        {
            Int32 simi = 1;
            IDataReader dr = null;
            try
            {
                string query = "select SIMIDT from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where ";
                query += "CCIADT = '" + this.cab_compania + "' and ";
                query += "SAPRDT = " + this.cab_anoperiodo + " and ";
                query += "TICODT = " + this.cab_tipo + " and ";
                query += "NUCODT = " + this.cab_noComprobante + " and ";
                query += "FECODT = " + this.cab_fecha;
                query += " order by SIMIDT desc";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string simidt = "";
                if (dr.Read())
                {
                    simidt = dr.GetValue(dr.GetOrdinal("SIMIDT")).ToString().Trim();
                }

                dr.Close();
                if (simidt == "") simidt = "0";
                if (simidt != "")
                {
                    simi = Convert.ToInt32(simidt);
                    simi++;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (simi);
        }

        public Int32 CalcularSIMIDetalleComprobanteTablaGLB01(string idRowNumber)
        {
            Int32 simi = 1;
            IDataReader dr = null;
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLB01";
                
                string query = "select SIMIDT from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "WHERE ";
                
                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + nombreTabla + ") = ";
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = ";
                        break;
                    case "Oracle":
                        //id_prefijotabla + GLAI3
                        string campoOracle = "ID_" + nombreTabla;
                        query += "ID_" + nombreTabla + " = ";
                        break;
                }

                query += idRowNumber;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string simidt = "";
                if (dr.Read())
                {
                    simidt = dr.GetValue(dr.GetOrdinal("SIMIDT")).ToString().Trim();
                }

                dr.Close();

                if (simidt != "")
                {
                    simi = Convert.ToInt32(simidt);                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (simi);
        }
        /// <summary>
        /// Devuelve el número de registro de la ultima fila insertada del detalle del comprobante
        /// </summary>
        /// <returns></returns>
        public string ObtenerIdLastRowNumberDetalleGLB01()
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                //Coger la 1ra línea de comentarios
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLB01";
                string query = "";
                string whereId = "";
                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query = "select RRN(" + nombreTabla + ") as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = "RRN(" + nombreTabla + ")";
                        break;
                    case "SQLServer":
                        query += "select GERIDENTI as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = "GERIDENTI";
                        break;
                    case "Oracle":
                        //id_prefijotabla + GLAI3
                        string campoOracle = "ID_" + nombreTabla;
                        query += "select " + campoOracle + " as id, " + nombreTabla + ".* from " + nombreTabla;
                        whereId = campoOracle;
                        break;
                }

                /*query += " where CCIAAD='" + this.cab_compania + "' and ";   //JL
                query += "SAPRAD= " + this.cab_anoperiodo + " and ";
                query += "TICOAD= " + this.cab_tipo + " and ";
                query += "NUCOAD= " + this.cab_noComprobante + " and ";*/
                query += " where CCIADT='" + this.cab_compania + "' and ";   //jl
                query += "SAPRDT= " + this.cab_anoperiodo + " and ";
                query += "TICODT= " + this.cab_tipo + " and ";
                query += "NUCODT= " + this.cab_noComprobante + " and ";
                query += "FECODT = " + this.cab_fecha;
                query += " order by id desc";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("id")).ToString().Trim();
                }

                dr.Close();

            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Chequea si existe el comprobante (antes de insertar el nuevo comprobante)
        /// </summary>
        /// <returns></returns>
        public bool ExisteComprobanteTablaGLI03()
        {
            bool existeComp = false;
            
            try
            {
                if (this.cab_noComprobante == "") this.cab_noComprobante = CalculaNumComprobanteTablaGLI03();

                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += " where CCIAIC='" + this.cab_compania + "' and ";
                query += "SAPRIC= " + this.cab_anoperiodo + " and ";
                query += "TICOIC= " + this.cab_tipo + " and ";
                query += "NUCOIC= " + this.cab_noComprobante;

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) existeComp = true;

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (existeComp);
        }
        private string CalculaNumComprobanteTablaGLI03()
        {
            IDataReader dr = null;
            try
            {
                string query = "select Max(NUCOIC) as NUMCOM from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += " where CCIAIC='" + this.cab_compania + "' and ";
                query += "SAPRIC= " + this.cab_anoperiodo + " and ";
                query += "TICOIC= " + this.cab_tipo;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.cab_noComprobante = dr.GetValue(dr.GetOrdinal("NUMCOM")).ToString().Trim();
                }
                if (this.cab_noComprobante=="") this.cab_noComprobante = "0";
                decimal Ncomp = Convert.ToDecimal(this.cab_noComprobante);
                this.cab_noComprobante = Convert.ToString(Ncomp=Ncomp +1);
               

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            
            return (this.cab_noComprobante);
        }
        /// <summary>
        /// Suprimme una línea del detalle del comprobante
        /// </summary>
        /// <param name="idRowNumber"></param>
        public void SuprimirDetalleComprobanteTablaGLB01(string idRowNumber)
        {
            try
            {
                //Devuelve los campos claves del detalle del comprobante para también eliminar los campos extendidos y actualizar las líneas de detalle
                string result = this.ObtenerCamposDetalleCompASuprimir(idRowNumber);
                if (result == "")
                {
                    //Eliminar linea del comprobante en la tabla GLB01
                    int reg = this.SuprimirDetalleCompTablaGLB01(idRowNumber);
                    
                    if (reg != 0 && this.cab_extendido == "1") 
                    {
                        //Eliminar linea del comprobante en la tabla GLBX1 (campos extendidos)
                        reg = this.SuprimirDetalleCompTablaGLBX1();

                        //Actualizar líneas de detalles en la cabecera del comprobante (GLI03)
                        this.CabeceraCompSuprimirCantidadLineasDetalle(1);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        #endregion

        #region Métodos Privados
        /// <summary>
        /// Devuelve la cuenta a último nivel
        /// </summary>
        /// <returns></returns>
        private string ObtenerCuentaUltimoNivel()
        {
            string ctaUltNivel = this.det_CUEN;

            try
            {
                int fila = 0;
                
                string errorMsg = "";
                DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivelValoresCampos(this.det_CUEN, this.cab_plan, ref errorMsg);

                if (errorMsg == "" && (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0))
                {
                    fila = dtCtasUltimoNivel.Rows.Count - 1;
                    ctaUltNivel = dtCtasUltimoNivel.Rows[fila]["CUENMC"].ToString();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (ctaUltNivel);
        }

        /// <summary>
        /// Devuelve los campos claves del detalle del comprobante para también eliminar los campos extendidos y actualizar las líneas de detalle
        /// </summary>
        /// <param name="idRowNumber"></param>
        /// <returns></returns>
        private string ObtenerCamposDetalleCompASuprimir(string idRowNumber)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLB01";
                string query = "select CCIADT, CUENDT, TAUXDT, CAUXDT, SAPRDT, TICODT, NUCODT, SIMIDT, FECODT from " + nombreTabla;
                query += " where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + nombreTabla + ") = ";
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = ";
                        break;
                    case "Oracle":
                        //id_prefijotabla + GLAI3
                        string campoOracle = "ID_" + nombreTabla;
                        query += "ID_" + nombreTabla + " = ";
                        break;
                }

                query += idRowNumber;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.cab_compania = dr["CCIADT"].ToString().Trim();
                    this.det_CUEN = dr["CUENDT"].ToString().Trim();
                    this.det_TAUX = dr["TAUXDT"].ToString().Trim();
                    this.det_CAUX = dr["CAUXDT"].ToString().Trim();
                    this.cab_anoperiodo = dr["SAPRDT"].ToString().Trim();
                    this.cab_tipo = dr["TICODT"].ToString().Trim();
                    this.cab_noComprobante = dr["NUCODT"].ToString().Trim();
                    this.det_SIMI = dr["SIMIDT"].ToString().Trim();
                    this.cab_fecha = dr["FECODT"].ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = "Error obteniendo los datos del comprobante para eliminarlo (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Suprime una línea del detalle del comprobante en la tabla GLB01
        /// </summary>
        /// <param name="idRowNumber"></param>
        private int SuprimirDetalleCompTablaGLB01(string idRowNumber)
        {
            int reg = 0;
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLB01";
                string query = "delete from " + nombreTabla;
                query += " where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + nombreTabla + ") = ";
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = ";
                        break;
                    case "Oracle":
                        //id_prefijotabla + GLAI3
                        string campoOracle = "ID_" + nombreTabla;
                        query += "ID_" + nombreTabla + " = ";
                        break;
                }
                query += idRowNumber; // jl

                reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (reg);
        }
        

        /// <summary>
        /// Suprime una línea del detalle del comprobante en la tabla GLBX1 (campos extendidos)
        /// </summary>
        private int SuprimirDetalleCompTablaGLBX1()
        {
            string result = "";
            Int32 simi = 1;
            int reg = 0;
            try
            {
                simi = CalcularSIMIDetalleComprobanteTablaGLB01(this.det_RowNumber);

                string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLBX1 ";
                query += "where ";
                query += "CCIADX = '" + this.cab_compania + "' and CUENDX = '" + this.det_CUEN + "' and TAUXDX = '" + this.det_TAUX + "' and ";
                query += "CAUXDX = '" + this.det_CAUX + "' and SAPRDX = " + this.cab_anoperiodo + " and TICODX = " + this.cab_tipo + " and ";
                query += "NUCODX = " + this.cab_noComprobante + " and SIMIDX = " + this.det_SIMI;

                reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.det_SIMI = "0";
            return (reg);
        }
        /// <summary>
        /// Actualiza la cantidad de líneas de detalle de la tabla de cabecera de los comprobantes
        /// </summary>
        /// <param name="cantidad">Cantidad de líneas a restar de las actuales</param>
        private void CabeceraCompSuprimirCantidadLineasDetalle(int cantidad)
        {
            IDataReader dr = null;
            try
            {
                string query = "select SIMIIC from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "where ";
                query += "CCIAIC = '" + this.cab_compania + "' and SAPRIC = " + this.cab_anoperiodo + " and TICOIC = " + this.cab_tipo + " and ";
                query += "NUCOIC = " + this.cab_noComprobante + " and FECOIC = " + this.cab_fecha;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                Int32 cantLineas = 0;
                if (dr.Read())
                {
                    try
                    {
                        cantLineas = Convert.ToInt32(dr["SIMIIC"].ToString().Trim());
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                dr.Close();

                Int32 lineas = cantLineas - cantidad;
                if (lineas >= 0)
                {
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLI03 set ";
                    query += "SIMIIC = " + lineas;
                    query += " where ";
                    query += "CCIAIC = '" + this.cab_compania + "' and SAPRIC = " + this.cab_anoperiodo + " and TICOIC = " + this.cab_tipo + " and ";
                    query += "NUCOIC = " + this.cab_noComprobante + " and FECOIC = " + this.cab_fecha;

                    int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        /// <summary>
        /// Obtiene por SQL la suma de los debes y haber de la moneda local y extrangera
        /// </summary>
        

        #endregion
    }
}

