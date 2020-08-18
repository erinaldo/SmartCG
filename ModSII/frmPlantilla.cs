using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using log4net;
using System.Configuration;

namespace ModSII
{
    public partial class frmPlantilla : Form
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        protected UtilesCGConsultas utilesCG;

        protected Autorizaciones aut;
       
        //protected static ProveedorDatos SII_ProveedorDatos = null;

        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
        /// </summary>
        public Form FrmPadre
        {
            get
            {
                return (this._frmPadre);
            }
            set
            {
                this._frmPadre = value;
            }
        }

        protected string agencia = null;
        protected const string agenciaDefault = "A";

        protected string entorno = null;
        protected const string entornoDefault = "T";

        protected ServicioWebSII serviceSII;

        public frmPlantilla()
        {
            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();

            //Inicializar agencia y entorno
            string agenciaValue = "";
            try
            {
                agenciaValue = GlobalVar.EntornoActivo.SiiAgencia;
                if (agenciaValue != null && agenciaValue != "") this.agencia = agenciaValue;
                else this.agencia = agenciaDefault;
            }
            catch (Exception ex)
            {
                this.agencia = agenciaDefault;
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            string entornoValue = "";
            try
            {
                entornoValue = GlobalVar.EntornoActivo.SiiEntorno;
                if (entornoValue != null && entornoValue != "") this.entorno = entornoValue;
                else this.entorno = entornoDefault;
            }
            catch (Exception ex)
            {
                this.entorno = entornoDefault;
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            this.serviceSII = new ServicioWebSII(Log, this.agencia, this.entorno);
        }

        private void frmPlantilla_Load(object sender, EventArgs e)
        {
            /*//Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();
            */

            //Centrar formulario
            if (this._frmPadre == null)
            {
                //Centrar formulario respecto a la pantalla completa
                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Top = (rect.Height / 2) - (this.Height / 2);
                this.Left = (rect.Width / 2) - (this.Width / 2);
            }
            else
            {
                //Centrar el formulario respecto al formulario padre
                utiles.CentrarFormHijo(this._frmPadre, this);
            }

            /*
            //Inicializar agencia y entorno
            var agenciaValue = ConfigurationManager.AppSettings["tipoAgencia"];
            if (!string.IsNullOrEmpty(agenciaValue))
            {
                try
                {
                    this.agencia = agenciaValue.ToString();
                }
                catch (Exception ex)
                {
                    this.agencia = agenciaDefault;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }
            }
            else this.agencia = agenciaDefault;

            var entornoValue = ConfigurationManager.AppSettings["entorno"];
            if (!string.IsNullOrEmpty(entornoValue))
            {
                try
                {
                    this.entorno = entornoValue.ToString();
                }
                catch (Exception ex)
                {
                    this.entorno = entornoDefault;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }
            }
            else this.entorno = entornoDefault;

            this.serviceSII = new ServicioWebSII(Log, this.agencia, this.entorno);*/
        }

        /// <summary>
        /// Devuelve la descripción de la operación
        /// </summary>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public string ObtenerDescripcionOperacion(string operacion)
        {
            string desc = "";
            
            switch (operacion.Trim())
            {
                case "A0":
                    desc = this.LP.GetText("lblOperacionAlta", "Alta");
                    break;
                case "A1":
                    desc = this.LP.GetText("lblOperacionModificar", "Modificar");
                    break;
                case "A4":
                    desc = this.LP.GetText("lblOperacionModificar", "Modificar Reg. Viajeros");
                    break;
                case "A5":
                    desc = this.LP.GetText("lblOperacionAltaDevIVAViajeros", "Alta devolución IVA Viajeros");
                    break;
                case "A6":
                    desc = this.LP.GetText("lblOperacionModificarDevIVAViajeros", "Modificar devolución IVA Viajeros");
                    break;
                case "B":
                    desc = this.LP.GetText("lblOperacionAnular", "Anular");
                    break;
            }

            return (desc);
        }

        /// <summary>
        /// Devuelve la descripción del Libro (según código del libro de la tabla IVSII1 campo TDOCS1)
        /// </summary>
        /// <param name="libroCod"></param>
        /// <returns></returns>
        public string ObtenerDescripcionLibro(string libroCod)
        {
            string desc = "";
            string cod = "";

            if (libroCod != null) cod = libroCod.Trim();

            switch (cod)
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                    desc = this.LP.GetText("lblLibroFactEmitidas", "Facturas Emitidas");
                    break;
                case LibroUtiles.LibroID_FacturasRecibidas:
                    desc = this.LP.GetText("lblLibroFacturasRecibidas", "Facturas Recibidas");
                    break;
                case LibroUtiles.LibroID_BienesInversion:
                    desc = this.LP.GetText("lblLibroBienesInversion", "Bienes de Inversión");
                    break;
                case LibroUtiles.LibroID_CobrosMetalico:
                    desc = this.LP.GetText("lblLibroCobrosMetalico", "Cobros en Metálico");
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    desc = this.LP.GetText("lblLibroOperaIntra", "Operaciones Intracomunitarias");
                    break;
                case LibroUtiles.LibroID_CobrosEmitidas:
                    desc = this.LP.GetText("lblLibroCobros", "Cobros Emitidas");
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    desc = this.LP.GetText("lblLibroPagos", "Pagos Recibidas RECC");
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    desc = this.LP.GetText("lblLibroOperacionesSeguros", "Operaciones de Seguros");
                    break;
                case LibroUtiles.LibroID_AgenciasViajes:
                    desc = this.LP.GetText("lblLibroAgenciasViajes", "Agencias de Viajes");
                    break;
            }

            return (desc);
        }

        /// <summary>
        /// Valida la existencia o no de la compañía fiscal
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        /// <param name="descripcion">nombre razon social</param>
        /// <param name="nif">nif compañía</param>
        /// <param name="periodoImpositivo">periodo impositivo M - mensual / T - Trimestral</param>
        /// <returns></returns>
        public string ValidarCompaniaFiscal(string codigo, ref string descripcion, ref string nif, ref string periodoImpositivo)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                //Comprobar que la compañía fiscal es válido
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                query += "where CIAFT3 = '" + codigo + "'";

                string agenciaEntornoActual = "";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NOMBT3")).ToString().Trim();
                    //descripcion = dr.GetValue(dr.GetOrdinal("NRAZS0")).ToString().Trim();
                    //nif = dr.GetValue(dr.GetOrdinal("NIFDS0")).ToString().Trim();
                    nif = dr.GetValue(dr.GetOrdinal("NIFDT3")).ToString().Trim();
                    periodoImpositivo = dr.GetValue(dr.GetOrdinal("EPG1T3")).ToString().Trim();

                    if (periodoImpositivo.Length < 1) periodoImpositivo = "M";
                    else
                    {
                        string primerCaracter = periodoImpositivo.Substring(0,1);
                        switch (primerCaracter)
                        {
                            case "T":
                                primerCaracter = "T";
                                break;
                            case "M":
                            default:
                                primerCaracter = "M";
                                break;
                        }
                    }

                    //Validar agencia-entorno activo
                    agenciaEntornoActual = dr.GetValue(dr.GetOrdinal("EPG2T3")).ToString();
                    if (agenciaEntornoActual.Trim() != "")
                    {
                        if (agenciaEntornoActual.Length < 2) agenciaEntornoActual = agenciaEntornoActual.PadRight(2, ' ');

                        if (!((agenciaEntornoActual.Substring(0, 1) == " " || agenciaEntornoActual.Substring(0, 1) == this.agencia)
                            &&
                            (agenciaEntornoActual.Substring(1, 1) == " " || agenciaEntornoActual.Substring(1, 1) == this.entorno)
                            ))
                        {
                            result = this.LP.GetText("lblfrmSiiSumInfoErrCompFiscalAgenciaEntorno", "La compañía fiscal no es válida para la agencia y entorno que está activo");   //Falta traducir
                        }                      
                    }
                }
                else
                {
                    //Error el tipo de auxiliar no es válido
                    result = this.LP.GetText("lblfrmSiiSumInfoErrCompFiscal", "La compañía fiscal no es válida");   //Falta traducir
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmSiiSumInfoErrCompFiscalExcep", "Error al validar la compañía fiscal") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Devuelve la lista de países para el desplegable de países
        /// </summary>
        /// <param name="filtro">true -> filtrar por el criterio indicado / false -> devuelve todos los países</param>
        /// <param name="valorFiltro">S -> países europeos /  N -> países no europeos</param>
        /// <returns></returns>
        public string ObtenerPaises(bool filtro, string valorFiltro, ref System.Collections.ArrayList arrayPaises )
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                //Prefijo para la tabla de Países (GLT30)
                string prefijoTablaGLT30 = "";
                string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                if (proveedorTipo == "DB2")
                {
                    prefijoTablaGLT30 = ConfigurationManager.AppSettings["bbddCGAPP"];
                    if (prefijoTablaGLT30 != null && prefijoTablaGLT30 != "") prefijoTablaGLT30 += ".";
                }
                else prefijoTablaGLT30 = GlobalVar.PrefijoTablaCG;

                string query = "select * from " + prefijoTablaGLT30 + "GLT30 ";

                if (filtro) query += "where UNEU30 = '" + valorFiltro + "' ";
                query += "order by NOMB30";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string codigo = "";
                string desc = "";
                while (dr.Read())
                {
                    codigo = dr.GetValue(dr.GetOrdinal("PCIF30")).ToString().Trim();
                    desc = dr.GetValue(dr.GetOrdinal("NOMB30")).ToString().Trim();
                    if (desc.Length > 32) desc = desc.Substring(0, 31);

                    arrayPaises.Add(new AddValue(desc + " (" + codigo + ")", codigo));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmSiiSumInfoErrCompFiscalExcep", "Error al validar la compañía fiscal") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Devuelve la sentencia SQL para la lista de compañías fiscales posibles a seleccionar desde el buscador
        /// </summary>
        /// <returns></returns>
        public string ObtenerQueryListaCompaniasFiscales()
        {
            string query = "";

            try
            {
                string agenciaEntorno = this.agencia + this.entorno;
                string textoVacio = "Múltiple";
                string textoAP = "AEAT - Producción";
                string textoAT = "AEAT - Test";
                string textoVP = "Foral Vizcaya - Producción";
                string textoCP = "Agencia Tributaria Canaria - Producción";
                string textoCT = "Agencia Tributaria Canaria - Test";
                string textoVacioP = "Múltiple - Producción";
                string textoVacioT = "Múltiple - Test";

                string textoAVacio = "AEAT - Múltiple";
                string textoVVacio = "Foral Vizcaya - Producción";
                string textoCVacio = "Agencia Tributaria Canaria - Múltiple";

                query = "select CIAFT3, NOMBT3, NIFDT3, ";
                string queryfiltroAgenciaEntorno = "";
                switch (GlobalVar.ConexionCG.TipoBaseDatos)
                {
                    case ProveedorDatos.DBTipos.DB2:
                        query += "SUBSTR((DIGITS(ULMCT3 + 190000)), 2, 4)|| '-' || SUBSTR((DIGITS(ULMCT3 + 190000)), 6, 2) AS CIERRE, ";
                        query += "CASE SUBSTR(EPG2T3, 1, 2) ";
                        query += "WHEN '  ' THEN '" + textoVacio + "' ";
                        query += "WHEN 'AP' THEN '" + textoAP + "' ";
                        query += "WHEN 'AT' THEN '" + textoAT + "' ";
                        query += "WHEN 'VP' THEN '" + textoVP + "' ";
                        query += "WHEN 'CP' THEN '" + textoCP + "' ";
                        query += "WHEN 'CT' THEN '" + textoCT + "' ";
                        query += "WHEN ' T' THEN '" + textoVacioT + "' ";
                        query += "WHEN ' P' THEN '" + textoVacioP + "' ";
                        query += "WHEN 'A ' THEN '" + textoAVacio + "' ";
                        query += "WHEN 'V ' THEN '" + textoVVacio + "' ";
                        query += "WHEN 'C ' THEN '" + textoCVacio + "' ";
                        query += "END EPG2T3 ";

                        //queryfiltroAgenciaEntorno = "SUBSTR(EPG2T3, 1, 2) = '" + agenciaEntorno + "'";
                        //queryfiltroAgenciaEntorno += "OR (SUBSTR(EPG2T3, 1, 1) = ' '   and SUBSTR(EPG2T3, 2, 1) = '" + this.entorno + "') " ;

                        queryfiltroAgenciaEntorno =  "(SUBSTR(EPG2T3, 1, 1) = ' ' or SUBSTR(EPG2T3, 1, 1) = '" + this.agencia + "') AND ";
                        queryfiltroAgenciaEntorno += "(SUBSTR(EPG2T3, 2, 1) = ' ' or SUBSTR(EPG2T3, 2, 1) = '" + this.entorno + "') ";
                        break;
                    case ProveedorDatos.DBTipos.SQLServer:
                        query += "SUBSTRING((CAST((ULMCT3 + 190000) AS CHAR(6))), 1, 4) + '-' +  SUBSTRING((CAST((ULMCT3 + 190000) AS CHAR(6))), 5, 2) AS CIERRE, ";
                        query += "EPG2T3 = (CASE SUBSTRING(EPG2T3, 1 , 2) ";
                        query += "WHEN '  ' THEN '" + textoVacio + "' ";
                        query += "WHEN 'AP' THEN '" + textoAP + "' ";
                        query += "WHEN 'AT' THEN '" + textoAT + "' ";
                        query += "WHEN 'VP' THEN '" + textoVP + "' ";
                        query += "WHEN 'CP' THEN '" + textoCP + "' ";
                        query += "WHEN 'CT' THEN '" + textoCT + "' ";
                        query += "WHEN ' T' THEN '" + textoVacioT + "' ";
                        query += "WHEN ' P' THEN '" + textoVacioP + "' ";
                        query += "WHEN 'A ' THEN '" + textoAVacio + "' ";
                        query += "WHEN 'V ' THEN '" + textoVVacio + "' ";
                        query += "WHEN 'C ' THEN '" + textoCVacio + "' ";
                        query += "END) ";

                        //queryfiltroAgenciaEntorno = "SUBSTRING(EPG2T3, 0, 2) = '" + agenciaEntorno + "'";
                        //queryfiltroAgenciaEntorno += "OR (SUBSTR(EPG2T3, 1, 1) = ' '   and SUBSTR(EPG2T3, 2, 1) = '" + this.entorno + "') " ;

                        queryfiltroAgenciaEntorno = "(SUBSTRING(EPG2T3, 1, 1) = ' ' or SUBSTRING(EPG2T3, 1, 1) = '" + this.agencia + "') AND ";
                        queryfiltroAgenciaEntorno += "(SUBSTRING(EPG2T3, 2, 1) = ' ' or SUBSTRING(EPG2T3, 2, 1) = '" + this.entorno + "') ";
                        break;
                    case ProveedorDatos.DBTipos.Oracle:
                        query += "SUBSTR((CAST((ULMCT3 + 190000) AS CHAR(6))), 1, 4)|| '-' || SUBSTR((CAST((ULMCT3 + 190000) AS CHAR(6))), 5, 2) AS CIERRE, ";
                        query += "CASE SUBSTR(EPG2T3, 1 , 2) ";
                        query += "WHEN '  ' THEN '" + textoVacio + "' ";
                        query += "WHEN 'AP' THEN '" + textoAP + "' ";
                        query += "WHEN 'AT' THEN '" + textoAT + "' ";
                        query += "WHEN 'VP' THEN '" + textoVP + "' ";
                        query += "WHEN 'CP' THEN '" + textoCP + "' ";
                        query += "WHEN 'CT' THEN '" + textoCT + "' ";
                        query += "WHEN ' T' THEN '" + textoVacioT + "' ";
                        query += "WHEN ' P' THEN '" + textoVacioP + "' ";
                        query += "WHEN 'A ' THEN '" + textoAVacio + "' ";
                        query += "WHEN 'V ' THEN '" + textoVVacio + "' ";
                        query += "WHEN 'C ' THEN '" + textoCVacio + "' ";
                        query += "END EPG2T3 ";

                        //queryfiltroAgenciaEntorno = "SUBSTR(EPG2T3, 0, 2) = '" + agenciaEntorno + "'";
                        //queryfiltroAgenciaEntorno += "OR (SUBSTR(EPG2T3, 1, 1) = ' '   and SUBSTR(EPG2T3, 2, 1) = '" + this.entorno + "') ";

                        queryfiltroAgenciaEntorno =  "(SUBSTR(EPG2T3, 1, 1) = ' ' or SUBSTR(EPG2T3, 1, 1) = '" + this.agencia + "') AND ";
                        queryfiltroAgenciaEntorno += "(SUBSTR(EPG2T3, 2, 1) = ' ' or SUBSTR(EPG2T3, 2, 1) = '" + this.entorno + "') ";
                        break;
                }

                query += "from ";
                query += GlobalVar.PrefijoTablaCG + "IVT03 ";
                //query += "where (EPG2T3 = '' or " + queryfiltroAgenciaEntorno + ") and ";
                query += "where (" + queryfiltroAgenciaEntorno + ") and ";
                query += "exists (select CIAFS0 from ";
                query += GlobalVar.PrefijoTablaCG + "IVSII0 ";
                query += "where CIAFS0 = CIAFT3) ";
                query += "order by NOMBT3";
            }
            catch (Exception ex) {Log.Error(Utiles.CreateExceptionString(ex));}

            return (query);
        }

        /// <summary>
        /// Devuelve la descripción del tipo de agencia
        /// </summary>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public string ObtenerDescripcionAgencia(string tipoAgenciaValor)
        {
            string desc = "";

            switch (tipoAgenciaValor)
            {
                case "A":
                    desc = this.LP.GetText("lblAEAT", "AEAT");
                    break;
                case "V":
                    desc = this.LP.GetText("lblForalVizcaya", "Foral Viscaya");
                    break;
                case "C":
                    desc = this.LP.GetText("lblAgenciaTribCanaria", "Agencia Tributaria Canaria");
                    break;
            }

            return (desc);
        }

        /// <summary>
        /// Devuelve la descripción del entorno
        /// </summary>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public string ObtenerDescripcionEntorno(string entornoValor)
        {
            string desc = "";

            switch (entornoValor)
            {
                case "P":
                    desc = this.LP.GetText("lblProduccion", "Producción");
                    break;
                case "T":
                    desc = this.LP.GetText("lblTest", "Test");
                    break;
            }

            return (desc);
        }

        /// <summary>
        /// Devuelve una cadena con la agencia - entorno activo para ser mostrada en el título de los form
        /// </summary>
        /// <returns></returns>
        public string FormTituloAgenciaEntorno()
        {
            string titulo = " - " + ObtenerDescripcionAgencia(this.agencia) + " - " + ObtenerDescripcionEntorno(this.entorno);
            return titulo;
        }
    }

    #region FacturaIdentificador -- Almacena el identificador de una factura (nacional o extranjera)
    public class FacturaIdentificador
    {
        private string numeroSerie;
        private string fechaDocumento;
        private string emisorFacturaNIF;
        private string emisorFacturaIdOtroCodPais;
        private string emisorFacturaIdOtroIdType;
        private string emisorFacturaIdOtroId;
        private string cargoAbono;

        #region Propiedades
        public string NumeroSerie
        {
            get
            {
                return (this.numeroSerie);
            }
            set
            {
                this.numeroSerie = value;
            }
        }

        public string FechaDocumento
        {
            get
            {
                return (this.fechaDocumento);
            }
            set
            {
                this.fechaDocumento = value;
            }
        }

        public string EmisorFacturaNIF
        {
            get
            {
                return (this.emisorFacturaNIF);
            }
            set
            {
                this.emisorFacturaNIF = value;
            }
        }

        public string EmisorFacturaIdOtroCodPais
        {
            get
            {
                return (this.emisorFacturaIdOtroCodPais);
            }
            set
            {
                this.emisorFacturaIdOtroCodPais = value;
            }
        }

        public string EmisorFacturaIdOtroIdType
        {
            get
            {
                return (this.emisorFacturaIdOtroIdType);
            }
            set
            {
                this.emisorFacturaIdOtroIdType = value;
            }
        }

        public string EmisorFacturaIdOtroId
        {
            get
            {
                return (this.emisorFacturaIdOtroId);
            }
            set
            {
                this.emisorFacturaIdOtroId = value;
            }
        }

        public string CargoAbono
        {
            get
            {
                return (this.cargoAbono);
            }
            set
            {
                this.cargoAbono = value;
            }
        }
        #endregion

        public FacturaIdentificador()
        {
            this.numeroSerie = "";
            this.fechaDocumento = "0";
            this.emisorFacturaNIF = "";
            this.emisorFacturaIdOtroCodPais = "";
            this.emisorFacturaIdOtroIdType = "";
            this.emisorFacturaIdOtroId = "";
            this.cargoAbono = "";
        }

        public FacturaIdentificador(string numeroSerie, string fechaDocumento, string emisorFacturaNIF,
                                    string emisorFacturaIdOtroCodPais, string emisorFacturaIdOtroIdType, string emisorFacturaIdOtroId)
        {
            this.numeroSerie = numeroSerie;
            this.fechaDocumento = fechaDocumento;
            this.emisorFacturaNIF = emisorFacturaNIF;
            this.emisorFacturaIdOtroCodPais = emisorFacturaIdOtroCodPais;
            this.emisorFacturaIdOtroIdType = emisorFacturaIdOtroIdType;
            this.emisorFacturaIdOtroId = emisorFacturaIdOtroId;
        }

        public FacturaIdentificador(string numeroSerie, string fechaDocumento, string emisorFacturaNIF,
                                    string emisorFacturaIdOtroCodPais, string emisorFacturaIdOtroIdType, string emisorFacturaIdOtroId,
                                    string cargoAbono)
        {
            this.numeroSerie = numeroSerie;
            this.fechaDocumento = fechaDocumento;
            this.emisorFacturaNIF = emisorFacturaNIF;
            this.emisorFacturaIdOtroCodPais = emisorFacturaIdOtroCodPais;
            this.emisorFacturaIdOtroIdType = emisorFacturaIdOtroIdType;
            this.emisorFacturaIdOtroId = emisorFacturaIdOtroId;
            this.cargoAbono = cargoAbono;
        }
    }
    #endregion

    #region Servicio Web del SII -- Objeto que conectará con el SII
    public class ServicioWebSII
    {
        private int timeOut;
        private string url;
        private ILog log;
        private tgSIIWebService.TGsiiService wsTGsii;
        private string agencia;
        private string entorno;

        #region Propiedades
        public int TimeOut
        {
            get
            {
                return (this.timeOut);
            }
            set
            {
                this.timeOut = value;
            }
        }

        public string URL
        {
            get
            {
                return (this.url);
            }
            set
            {
                this.url = value;
            }
        }

        public tgSIIWebService.TGsiiService WSTGsii
        {
            get
            {
                return (this.wsTGsii);
            }
            set
            {
                this.wsTGsii = value;
            }
        }

        public string Agencia
        {
            get
            {
                return (this.agencia);
            }
            set
            {
                this.agencia = value;
            }
        }

        public string Entorno
        {
            get
            {
                return (this.entorno);
            }
            set
            {
                this.entorno = value;
            }
        }
        #endregion

        public ServicioWebSII(ILog logValue, string agenciaValor, string entornoValor)
        {
            this.log = logValue;
            this.agencia = agenciaValor;
            this.entorno = entornoValor;

            this.ServicioWebSIIInit();
        }

        /// <summary>
        /// Inicializa el servicio web del SII (timeout y url)
        /// </summary>
        protected void ServicioWebSIIInit()
        {
            try
            {
                //Inicializar el TimeOut del WebService
                var SiiWSTimeOutValue = ConfigurationManager.AppSettings["SIITimeOutWS"];
                if (!string.IsNullOrEmpty(SiiWSTimeOutValue))
                {
                    try
                    {
                        int timeOut = Convert.ToInt32(SiiWSTimeOutValue);
                        this.timeOut = timeOut;
                    }
                    catch (Exception ex)
                    {
                        this.timeOut = 0;
                        this.log.Error(Utiles.CreateExceptionString(ex));
                    }
                }
                else this.timeOut = 0;

                //Inicializar el WebService
                this.url = LibroUtiles.ObtenerURLServicioWebActivo(this.agencia, this.entorno);
                LibroUtiles.GrabarURLServicioWebSIIConfig(this.url);
                this.wsTGsii = new tgSIIWebService.TGsiiService();
                if (this.timeOut == 0) this.wsTGsii.Timeout = System.Threading.Timeout.Infinite;
                else wsTGsii.Timeout = this.timeOut;
                this.wsTGsii.Url = this.url;
            }
            catch (Exception ex)
            {
                this.log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Graba en el fichero de configuración la url del servicio web del SII
        /// </summary>
        /// <param name="agenciaValor"></param>
        /// <param name="entornoValor"></param>
        /// <param name="urlValor"></param>
        public void ServicioWebSIICambiarURL(string agenciaValor, string entornoValor, string urlValor)
        {
            try
            {
                this.agencia = agenciaValor;
                this.entorno = entornoValor;
                LibroUtiles.GrabarURLServicioWebSIIConfig(urlValor);
                this.url = urlValor;
                this.wsTGsii.Url = this.url;
            }
            catch (Exception ex)
            {
                this.log.Error(Utiles.CreateExceptionString(ex));
            }
        }
    }
    #endregion  

}
