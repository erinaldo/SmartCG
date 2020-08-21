using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using ObjectModel;
using log4net;

namespace ModComprobantes
{
    /// <summary>
    /// Clase que gestiona la transferencia de comprobantes contables 
    /// (se utiliza desde el formulario que contiene la lista de comprobantes contables (frmCompContLista), 
    /// desde el formulario de transferir comprobantes contables a Finanzas (frmCompContTransferirFinanComprobantes) y 
    /// desde el formulario de transferir Archivos de Lote (frmCompContTransferirArchivoLote))
    /// </summary>
    class ComprobanteContableTransferir
    {
        //Campos Cabecera
        public string TTRAWS;
        public string CCIAWS;
        public string ANOCWS;
        public string LAPSWS;
        public string TICOWS;
        public string NUCOWS;
        public string TVOUWS;
        public string DIAEWS;
        public string MESEWS;
        public string ANOEWS;
        public string TASCWS;
        public string TIMOWS;
        public string STATWS;
        public string DOCRWS;
        public string DOCDWS;

        //Campos Detalle
        public string CUENWS;
        public string CAUXWS;
        public string DESCWS;
        public string MONTWS;
        public string TMOVWS;
        public string MOSMWS;
        public string CLDOWS;
        public string NDOCWS;
        public string FDOCWS;
        public string FEVEWS;
        public string TEINWS;
        public string NNITWS;
        public string AUA1WS;
        public string AUA2WS;
        public string CDDOWS;
        public string NDDOWS;
        public string TERCWS;
        public string CDIVWS;

        //Campos Extendidos
        public string PRFDWS;
        public string NFAAWS;
        public string NFARWS;
        public string FIVAWS;
        public string USA1WS;
        public string USA2WS;
        public string USA3WS;
        public string USA4WS;
        public string USA5WS;
        public string USA6WS;
        public string USA7WS;
        public string USA8WS;
        public string USN1WS;
        public string USN2WS;
        public string USF1WS;
        public string USF2WS;

        public bool extendido = false;
        public int tipoContabilizacion = 0;

        public string tipoBaseDatosCG = "";
        public string prefijoLote = "";
        public string descripcion = "";
        public string estado = "";
        public string bibliotecaPrefijo = "";
        public string cola = "";
        public string biliotecaCola = "";

        public string archivo = "";             //Nombre del archivo
        public string descCompArchivo = "";     //Descripción del comprobante dentro del archivo

        public string revertir = "";
        public bool verNoComp = false;

        private ArrayList revertirComp;     //Datos de la reversión de los comprobantes

        public ArrayList tablasLotes;

        public DataTable numCompGenerados;

        public string bibliotecaTablasLoteAS;

        private Utiles utiles;
        private UtilesCGConsultas utilesCG;
        public LanguageProvider LP;
        private ILog Log;

        public ComprobanteContableTransferir()
        {
            this.tablasLotes = new ArrayList();

            this.utiles = new Utiles();
            this.utilesCG = new UtilesCGConsultas();

            this.Log = log4net.LogManager.GetLogger(this.GetType());

            this.bibliotecaTablasLoteAS = "";

            this.revertirComp = new ArrayList();

            //Construir el DataTable que almancena los números de comprobantes generados
            this.BuildDataTableNumCompGenerados();
        }


        #region Métodos Públicos
        /// <summary>
        /// Crea las columnas para el DataTable que contiene los comprobantes
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string CrearColumnasDataTableComprobantes(ref DataTable dataTable)
        {
            string result = "";

            try
            {
                //Adicionar las columnas al DataTable
                DataColumn col = new DataColumn("archivo", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("compania", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("aapp", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("descripcion", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("tipo", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("noComp", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("fecha", typeof(System.DateTime));
                dataTable.Columns.Add(col);
                col = new DataColumn("clase", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("tasa", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("debeML", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("haberML", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("debeME", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("haberME", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("noMovimiento", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("transferido", typeof(System.Boolean));
                dataTable.Columns.Add(col);
                col = new DataColumn("extendido", typeof(System.Boolean));
                dataTable.Columns.Add(col);
                col = new DataColumn("revertir", typeof(System.String));
                dataTable.Columns.Add(col);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = ex.Message;
            }

            return (result);
        }

        /// <summary>
        /// Dado un fichero de comprobante, busca su información y devuelve una fila en el datatable para despues ser adicionado a la Grid de comprobantes
        /// </summary>
        /// <param name="pathFichero">ruta de los ficheros de comprobantes contables</param>
        /// <param name="nombreFichero">nombre del fichero que contiene el comprobante contable</param>
        /// <param name="dataTable">DataTable de comprobantes extracontables</param>
        /// <param name="NOfrmCompExtContLista">true -> llamada desde el formulario frmCompExtContTransferirFinanComprobantes   false -> llamada desde frmCompExtContLista</param>
        /// <param name="todosComp">false -> llamada desde el formulario frmCompExtContLista    llamada desde el formulario frmCompExtContTransferirFinanComprobantes, true si rbEstadoTodos.Checked false en otro caso</param>
        /// <returns></returns>
        public string ProcesarFichero(string pathFichero, string nombreFichero, ref DataTable dataTable, bool NOfrmCompExtContLista, bool todosComp)
        {
            string result = "";
            try
            {
                //Definir esquema del dataset
                DataSet ds = new DataSet();
                DataTable dtCab = ds.Tables.Add("Cabecera");
                DataColumn dcCab = dtCab.Columns.Add("Contable");
                dcCab = dtCab.Columns.Add("Transferido");
                dcCab = dtCab.Columns.Add("Compania");
                dcCab = dtCab.Columns.Add("AnoPeriodo");
                dcCab = dtCab.Columns.Add("Descripcion");
                dcCab = dtCab.Columns.Add("Tipo");
                dcCab = dtCab.Columns.Add("Numero");
                dcCab = dtCab.Columns.Add("Fecha");
                dcCab = dtCab.Columns.Add("Clase");
                dcCab = dtCab.Columns.Add("Tasa");
                dcCab = dtCab.Columns.Add("Extendido");
                dcCab = dtCab.Columns.Add("Revertir");
                DataTable dtTot = ds.Tables.Add("Totales");
                DataColumn dcTot = dtTot.Columns.Add("MonedaLocalDebe");
                dcTot = dtTot.Columns.Add("MonedaLocalHaber");
                dcTot = dtTot.Columns.Add("MonedaExtDebe");
                dcTot = dtTot.Columns.Add("MonedaExtHaber");
                dcTot = dtTot.Columns.Add("NumeroApuntes");

                ds.ReadXml(pathFichero, XmlReadMode.IgnoreSchema); //solo lee del xml los datos del esquema definido

                int posSep = 0;
                //Verificar que exista la tabla de Cabecera
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                {
                    //Verificar la marca contable para asegurar que sea un comprobante contable
                    if (ds.Tables["Cabecera"].Rows[0]["Contable"].ToString() == "1")
                    {

                        string transferido = "0";
                        try { transferido = ds.Tables["Cabecera"].Rows[0]["Transferido"].ToString().Trim(); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        if (transferido == "") transferido = "0";

                        //Chequear si el comprobante está transferido o no (para la llamada desde el formulario comprobante extracontable transferir finanzas comprobantes)
                        bool procesarFichero = true;

                        if (NOfrmCompExtContLista)
                            if (todosComp || (!todosComp && transferido == "0")) procesarFichero = true;
                            else procesarFichero = false;

                        if (procesarFichero)
                        {
                            string compania = "";
                            string aaPP = "";
                            string descripcion = "";
                            string tipo = "";
                            string noComp = "";
                            string fecha = "";
                            string clase = "";
                            string tasa = "";
                            string debeML = "";
                            string haberML = "";
                            string debeME = "";
                            string haberME = "";
                            string noMovimiento = "";
                            string extendido = "0";
                            string revertir = "";

                            try { compania = ds.Tables["Cabecera"].Rows[0]["Compania"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { aaPP = ds.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { descripcion = ds.Tables["Cabecera"].Rows[0]["Descripcion"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { tipo = ds.Tables["Cabecera"].Rows[0]["Tipo"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { noComp = ds.Tables["Cabecera"].Rows[0]["Numero"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { fecha = ds.Tables["Cabecera"].Rows[0]["Fecha"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { clase = ds.Tables["Cabecera"].Rows[0]["Clase"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { tasa = ds.Tables["Cabecera"].Rows[0]["Tasa"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { extendido = ds.Tables["Cabecera"].Rows[0]["Extendido"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { revertir = ds.Tables["Cabecera"].Rows[0]["Revertir"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            //Verificar que exista la tabla de Totales
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Totales"].Rows.Count > 0)
                            {
                                try { debeML = ds.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { haberML = ds.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { debeME = ds.Tables["Totales"].Rows[0]["MonedaExtDebe"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { haberME = ds.Tables["Totales"].Rows[0]["MonedaExtHaber"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { noMovimiento = ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            }

                            //Insertar una fila en el DataTable
                            DataRow dr = dataTable.NewRow();
                            dr["archivo"] = nombreFichero;
                            dr["compania"] = compania;

                            //Formatear el año periodo con el separador (-)
                            if (aaPP != "")
                            {
                                posSep = aaPP.IndexOf('-');
                                if (posSep == -1)
                                {
                                    aaPP = aaPP.PadRight(4, ' ');
                                    aaPP = aaPP.Substring(0, 2) + "-" + aaPP.Substring(2, 2); 
                                }
                            }
                            dr["aapp"] = aaPP;
                            dr["descripcion"] = descripcion;

                            if (tipo.Length == 1) tipo = "0" + tipo;
                            dr["tipo"] = tipo;

                            dr["noComp"] = noComp;
                            dr["fecha"] = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                            dr["clase"] = clase;
                            dr["tasa"] = tasa;
                            if (transferido == "1") dr["transferido"] = true;
                            else dr["transferido"] = false;
                            dr["debeML"] = debeML;
                            dr["haberML"] = haberML;
                            dr["debeME"] = debeME;
                            dr["haberME"] = haberME;
                            dr["noMovimiento"] = noMovimiento;
                            if (extendido == "1") dr["extendido"] = true;
                            else dr["extendido"] = false;
                            dr["revertir"] = revertir;

                            dataTable.Rows.Add(dr);
                        }
                    }
                }
                else result = this.LP.GetText("errFicheroBadCabecera", "Fichero incorrecto (Cabecera)");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errProcesandoFichero", "Error procesando el fichero") + " " + nombreFichero + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Transferir la cabecera del comprobante
        /// </summary>
        /// <returns></returns>
        public string TransferirCabecera(Boolean esReversion = false)
        {
            string result = "";

            try
            {
                string nombreTablaCab = "";
                string camposCab = "";

                string saapp = "";
                string fecComp = "";

                string[] revertirCompDatos = new string[10];

                switch (tipoBaseDatosCG)
                {
                    case "DB2":
                        camposCab = "TTRAWS,CCIAWS,AÑOCWS,LAPSWS,TICOWS,NUCOWS,TVOUWS,DIAEWS,MESEWS,AÑOEWS,TASCWS,TIMOWS,STATWS,DOCRWS,DOCDWS";
                        break;
                    case "SQLServer":
                        camposCab = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,TVOUWS,DIAEWS,MESEWS,AVOEWS,TASCWS,TIMOWS,STATWS,DOCRWS,DOCDWS";
                        break;
                    case "Oracle":
                        camposCab = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,TVOUWS,DIAEWS,MESEWS,AVOEWS,TASCWS,TIMOWS,STATWS,DOCRWS,DOCDWS";
                        break;
                }

                saapp = utiles.SigloDadoAnno(ANOCWS, CGParametrosGrles.GLC01_ALSIRC) + ANOCWS + LAPSWS;

                if (NUCOWS == "")
                {
                    if (esReversion)
                    {
                        string[] valorGLT04 = new string[2];
                        valorGLT04 = this.ObtenerSiguientePeriodo(CCIAWS, saapp);
                        saapp = valorGLT04[0];
                        fecComp = valorGLT04[1];
                        //ANOCWS,LAPSWS,DIAEWS,MESEWS,AÑOEWS - cambiar a siguiente periodo
                        ANOCWS = saapp.Substring(1,2);
                        LAPSWS = saapp.Substring(3,2);
                        DIAEWS = fecComp.Substring(5,2);
                        MESEWS = fecComp.Substring(3,2);
                        ANOEWS = fecComp.Substring(1,2);
                        DOCDWS = DOCDWS + " - Reversión";
                        if(DOCDWS.Length > 36) DOCDWS = DOCDWS.Substring(0, 36);
                    }
                    
                    NUCOWS = this.GenerarNoComprobante(CCIAWS, saapp, TICOWS);
                }

                //Crear la tabla de visualización del comprobante
                DataRow row = this.numCompGenerados.NewRow();
                row["archivo"] = archivo;
                //row["descripcion"] = this.descCompArchivo;
                row["descripcion"] = DOCDWS;
                row["compania"] = CCIAWS;

                string aappFormato = ANOCWS + LAPSWS;
                aappFormato = utiles.AAPPConFormato(aappFormato);

                row["aapp"] = aappFormato;
                row["tipo"] = TICOWS;
                row["noComp"] = NUCOWS;

                this.numCompGenerados.Rows.Add(row);

                if (extendido) nombreTablaCab = this.prefijoLote + "W10";
                else nombreTablaCab = this.prefijoLote + "W00";

                //Insertar la cabecera del comprobante
                result = this.InsertarCabeceraComprobante(nombreTablaCab, camposCab, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                TVOUWS, DIAEWS, MESEWS, ANOEWS, TASCWS, TIMOWS, STATWS, DOCRWS, DOCDWS);

                if (result != "") return (result);

                //if (revertir == "S" || revertir == "T")
                //{
                //    //Chequear que no se haya calculado anteriormente FALTA
                //
                //    revertirCompDatos = this.RevertirDatosComprobante(CCIAWS, saapp, TICOWS, DOCDWS);
                //
                //    if (revertirCompDatos != null)
                //    {
                //        this.revertirComp.Add(revertirCompDatos);
                //
                //        //Insertar la cabecera del comprobante de reversión
                //        this.InsertarCabeceraComprobante(nombreTablaCab, camposCab, TTRAWS, CCIAWS,
                //                                        revertirCompDatos[3], revertirCompDatos[4], TICOWS, revertirCompDatos[0],
                //                                        TVOUWS, revertirCompDatos[5], revertirCompDatos[6], revertirCompDatos[7],
                //                                        TASCWS, TIMOWS, STATWS, DOCRWS, revertirCompDatos[10]);
                //    }
                //}
                //else revertirCompDatos = null;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errTransferCabComp", "Error transfiriendo la cabecera del comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Transferir una línea de detalle del comprobante
        /// </summary>
        /// <returns></returns>
        public string TransferirDetalle(Boolean esReversion = false)
        {
            string result = "";

            decimal montws = 0;
            decimal mosmws = 0;
            decimal tercws = 0;
            decimal usn1ws = 0;
            decimal usn2ws = 0;

            if (MONTWS.ToString() != "") montws = Convert.ToDecimal(MONTWS);
            if (MOSMWS.ToString() != "") mosmws = Convert.ToDecimal(MOSMWS);
            if (TERCWS.ToString() != "") tercws = Convert.ToDecimal(TERCWS);
            if (extendido)
            {
                if (USN1WS.ToString() != "") usn1ws = Convert.ToDecimal(USN1WS);
                if (USN2WS.ToString() != "") usn2ws = Convert.ToDecimal(USN2WS);
            }

            //poner fechas en formato ddmmaa
            //FDOCWS, FEVEWS, FIVAWS, USF1WS, USF2WS
            if ((FDOCWS.ToString() != "0") && (FDOCWS.Length == 8)) FDOCWS = FDOCWS.Substring(6, 2) + FDOCWS.Substring(4, 2) + FDOCWS.Substring(2, 2);//SMR!!!!
            if ((FEVEWS.ToString() != "0") && (FEVEWS.Length == 8)) FEVEWS = FEVEWS.Substring(6, 2) + FEVEWS.Substring(4, 2) + FEVEWS.Substring(2, 2);
            if (extendido)
            {
                if ((FIVAWS.ToString() != "0") && (FIVAWS.Length == 8)) FIVAWS = FIVAWS.Substring(6, 2) + FIVAWS.Substring(4, 2) + FIVAWS.Substring(2, 2);
                if ((USF1WS.ToString() != "0") && (USF1WS.Length == 8)) USF1WS = USF1WS.Substring(6, 2) + USF1WS.Substring(4, 2) + USF1WS.Substring(2, 2);
                if ((USF2WS.ToString() != "0") && (USF2WS.Length == 8)) USF2WS = USF2WS.Substring(6, 2) + USF2WS.Substring(4, 2) + USF2WS.Substring(2, 2);
            }

            try
            {
                string nombreTablaDet = "";
                string camposDet = "";
                string camposDetExt = "";

                string saapp = "";

                string[] revertirCompDatos = new string[10];

                switch (tipoBaseDatosCG)
                {
                    case "DB2":
                        camposDet = "TTRAWS,CCIAWS,AÑOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                        camposDetExt = "TTRAWS,CCIAWS,AÑOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                        break;
                    case "SQLServer":
                        camposDet = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                        camposDetExt = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                        break;
                    case "Oracle":
                        camposDet = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                        camposDetExt = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                        break;
                }

                if (extendido) nombreTablaDet = this.prefijoLote + "W11";
                else nombreTablaDet = this.prefijoLote + "W01";

                if (NUCOWS == "")
                {
                    string compania = "";
                    string tipoComp = "";
                    string noComp = "";
                    //Buscar el numero de comprobante que le corresponde
                    for (int i = 0; i < this.numCompGenerados.Rows.Count; i++)
                    {
                        compania = this.numCompGenerados.Rows[i]["compania"].ToString();
                        saapp = this.numCompGenerados.Rows[i]["aapp"].ToString();
                        tipoComp = this.numCompGenerados.Rows[i]["tipo"].ToString();
                        noComp = this.numCompGenerados.Rows[i]["noComp"].ToString();

                        if (CCIAWS == compania && (ANOCWS + "-" + LAPSWS == saapp) && TICOWS == tipoComp)
                        {
                            NUCOWS = noComp;
                            break;
                        }
                    }

                    if (NUCOWS == "")
                    {
                        NUCOWS = this.GenerarNoComprobante(CCIAWS, saapp, TICOWS);

                        DataRow row = this.numCompGenerados.NewRow();
                        row["archivo"] = archivo;
                        row["descripcion"] = this.descCompArchivo;
                        row["compania"] = CCIAWS;

                        string aappFormato = ANOCWS + LAPSWS;
                        aappFormato = utiles.AAPPConFormato(aappFormato);

                        row["aapp"] = aappFormato;
                        row["tipo"] = TICOWS;
                        row["noComp"] = NUCOWS;

                        this.numCompGenerados.Rows.Add(row);
                    }
                }

                if (esReversion == true)
                {
                    montws = montws * -1;
                    mosmws = mosmws * -1;
                    tercws = tercws * -1;
                    if (revertir == "T")
                    {
                        if (TMOVWS == "D")
                        {
                            TMOVWS = "H";
                        }
                        else
                        {
                            if (TMOVWS == "H") TMOVWS = "D";
                        }
                    }
                }

                //Insertar detalle del comprobante
                if (!extendido)
                {
                    result = this.InsertarDetalleComprobante(nombreTablaDet, camposDet, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                    CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                    TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS);
                }
                else
                {
                    result = this.InsertarDetalleComprobanteExt(nombreTablaDet, camposDetExt, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                        CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                        TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS,
                                                        PRFDWS, NFAAWS, NFARWS, FIVAWS, USA1WS, USA2WS, USA3WS, USA4WS, USA5WS, USA6WS,
                                                        USA7WS, USA8WS, usn1ws, usn2ws, USF1WS, USF2WS);
                }

                if (result != "") return (result);

                //if (revertir == "S" || revertir == "T")
                //{
                //    if (this.revertirComp != null)
                //    {
                //
                //        //Buscar si existen los datos para la reversión del comprobante
                //        bool datosCompReversionNulos = true;
                //        for (int i = 0; i < this.revertirComp.Count; i++)
                //        {
                //            revertirCompDatos = (string[])(this.revertirComp[i]);
                //            if (CCIAWS == revertirCompDatos[8] && saapp == revertirCompDatos[1] && 
                //                TICOWS == revertirCompDatos[9] && DESCWS == revertirCompDatos[10])
                //            {
                //                datosCompReversionNulos = false;
                //                break;
                //            }
                //        }
                //
                //        if (datosCompReversionNulos)
                //        {
                //            //Generar los datos para la reversión del comprobante
                //            revertirCompDatos = this.RevertirDatosComprobante(CCIAWS, saapp, TICOWS, DESCWS);
                //
                //            if (revertirCompDatos != null) this.revertirComp.Add(revertirCompDatos);
                //        }
                //
                //
                //    }
                //    if (revertirCompDatos != null)
                //    {
                //        montws = montws * -1;
                //        tercws = tercws * -1;
                //        mosmws = mosmws * -1;
                //        
                //        string dh = TMOVWS;
                //        if (revertir == "T")
                //        {
                //            if (dh == "D") dh = "H";
                //            else if (dh == "H") dh = "D";
                //        }
                //
                //        //Insertar el detalle del comprobante de reversión
                //        if (!extendido)
                //        {
                //            this.InsertarDetalleComprobante(nombreTablaDet, camposDet, TTRAWS, CCIAWS,
                //                                            revertirCompDatos[4], revertirCompDatos[5], TICOWS, revertirCompDatos[6],
                //                                            CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                //                                            TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS);
                //        }
                //        else
                //        {
                //            this.InsertarDetalleComprobanteExt(nombreTablaDet, camposDetExt, TTRAWS, CCIAWS,
                //                                            revertirCompDatos[4], revertirCompDatos[5], TICOWS, revertirCompDatos[6],
                //                                            CUENWS, CAUXWS, DESCWS, montws, dh, mosmws,
                //                                            CLDOWS, NDOCWS, FDOCWS, FEVEWS, TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS,
                //                                            NDDOWS, tercws, CDIVWS,
                //                                            PRFDWS, NFAAWS, NFARWS, FIVAWS, USA1WS, USA2WS, USA3WS, USA4WS, USA5WS, USA6WS,
                //                                            USA7WS, USA8WS, usn1ws, usn2ws, USF1WS, USF2WS);
                //        }
                //    }
                //}
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errTransferDetComp", "Error transfiriendo el detalle del comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Inserta un registro en la tabla GLC04
        /// </summary>
        public string InsertarRegistroGLC04()
        {
            string result = "";
            try
            {
                string STATC4 = "";
                string PREFC4 = this.prefijoLote;

                string APROC4 = "N";
                if (this.extendido == true) APROC4 = "n";
                string estado = this.estado;
                /*switch (estado)*/
                switch (tipoContabilizacion)
                {
                    case 1:
                        //No aprobado
                        APROC4 = "N";
                        if (this.extendido == true) APROC4 = "n";
                        break;
                    case 2:
                        //Aprobado
                        APROC4 = "S";
                        if (this.extendido == true) APROC4 = "s";
                        break;
                    case 3:
                        //Contabilizado
                        APROC4 = "C";
                        if (this.extendido == true) APROC4 = "c";
                        break;
                }

                string USERC4 = GlobalVar.UsuarioLogadoCG;
                string WSGEC4 = "CGCS";
                string DESCC4 = this.descripcion;

                string LIBLC4 = "";
                string LIBEC4 = "";
                string OUTQC4 = "";
                string LIBQC4 = "";

                if (tipoBaseDatosCG == "DB2")
                {
                    LIBLC4 = this.bibliotecaPrefijo;
                    LIBEC4 = "";
                    OUTQC4 = this.cola;
                    LIBQC4 = this.biliotecaCola;
                }

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLC04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATC4,PREFC4,APROC4,USERC4,WSGEC4,DESCC4,LIBLC4,LIBEC4,OUTQC4,LIBQC4) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + STATC4 + "', '" + PREFC4 + "', '" + APROC4 + "', '" + USERC4 + "', '" + WSGEC4 + "', '" + DESCC4 + "', '" + LIBLC4 + "', '";
                query += LIBEC4 + "', '" + OUTQC4 + "', '" + LIBQC4 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (tipoBaseDatosCG == "DB2")
                {
                    try
                    {
                        string proveedorConfig = ConfigurationManager.AppSettings["proveedorDatosCG"];
                        ProveedorDatos.DBProveedores tipoProveedorDatos = (ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), proveedorConfig);

                        IDbCommand command;

                        switch (tipoProveedorDatos)
                        {
                            case ProveedorDatos.DBProveedores.Odbc:
                                command = new OdbcCommand("", (OdbcConnection)GlobalVar.ConexionCG.GetConnectionValue);
                                break;
                            default:
                                command = new OleDbCommand("", (OleDbConnection)GlobalVar.ConexionCG.GetConnectionValue);
                                break;
                        }

                        /* crear entorno en AS400*
                        string bbddCGAPP = ConfigurationManager.AppSettings["bbddCGAPP"];
                        string comando = "CALL PGM(" + bbddCGAPP + "/CG024) PARM(CGDATASII X)";
                        string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";
                        string longitudComando = comando.Length.ToString();
                        
                        sentencia = sentencia + longitudComando.PadLeft(10, '0');
                        sentencia = sentencia + ".00000)";

                        GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);
                        */

                        /* llamar a GL21G */
                        string comando = "CALL PGM(GL21G)";
                        string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";
                        string longitudComando = comando.Length.ToString();

                        sentencia = sentencia + longitudComando.PadLeft(10, '0');
                        sentencia = sentencia + ".00000)";

                        GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);

                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else
                {
                    query = "insert into XDTAQ (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_XDTAQ, ";
                    query += "NOMBRE, LIBRERIA, LONGITUD, DATOS) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_XDTAQ.nextval, ";
                    query += "'CGADTQ', ' ', 10, 'GO-A')";
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

                /*
                 7.2- Si AS llamar a:  “ appl.REGFRAConn.Execute "CALL " & Biblioteca & "GL21G” ”
	      - Ni no AS, insertar en  XDTAQ
                   .Fields("NOMBRE").Value = "CGADTQ"
                   .Fields("LIBRERIA").Value = " "
                   .Fields("LONGITUD").Value = 10
                   .Fields("DATOS").Value = "GO-A"

                 * */
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertGLC04", "Error insertando en la tabla GLC04") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Verifica si existen datos del lote
        /// </summary>
        /// <returns></returns>
        public string VerificarExistenDatosLote()
        {
            string result = "";

            try
            {
                if (tipoBaseDatosCG != "DB2") bibliotecaTablasLoteAS = "";
                else
                {
                    if (this.bibliotecaPrefijo != "") bibliotecaTablasLoteAS = this.bibliotecaPrefijo + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                if (!this.extendido)
                {
                    result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W00");
                    result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W01");

                    //Si existe la tabla W30
                    if (utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + this.prefijoLote + "W30")) result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W30");

                    //Si existe la tabla W31
                    if (utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + this.prefijoLote + "W31")) result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W31");
                }

                if (this.extendido)
                {
                    result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W10");
                    result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W11");

                    //Si existe la tabla W40
                    if (utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W40")) result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W40");

                    //Si existe la tabla W41
                    if (utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W41")) result += VerificarExistenDatosLoteTabla(this.tipoBaseDatosCG, this.prefijoLote + "W41");
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result += " - " + this.LP.GetText("errVerificarDatosLote", "Error verificando si existen datos del lote") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }

        /// <summary>
        /// Elimina los datos del lote en la tabla especificada
        /// </summary>
        /// <returns></returns>
        public string EliminarDatosLoteTabla(string nombreTabla)
        {
            string result = "";

            try
            {
                string query = "delete from " + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + nombreTabla;

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errDelDatosLote", "Error eliminando el lote de la tabla") + " " + nombreTabla + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Verifica si existen datos del lote en la tabla
        /// </summary>
        /// <returns></returns>
        public string VerificarExistenDatosLoteTabla(string tipoBaseDatosCG, string nombreTabla)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                string query = "select * from " + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + nombreTabla;

                int registro = 0;
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string CCIAWS = "";
                string ANOCWS = "";
                string LAPSWS = "";
                string TICOWS = "";
                string NUCOWS = "";
                string DOCDWS = "";

                while (dr.Read())
                {
                    if (registro == 0)
                    {
                        this.tablasLotes.Add(nombreTabla);
                        result += this.LP.GetText("errExistenDatosLote", "Existen datos del lote en la Tabla") + ": " + nombreTabla + "\n\r";
                    }

                    CCIAWS = dr.GetValue(dr.GetOrdinal("CCIAWS")).ToString();
                    switch (tipoBaseDatosCG)
                    {
                        case "DB2":
                            ANOCWS = dr.GetValue(dr.GetOrdinal("AÑOCWS")).ToString();
                            break;
                        default:
                            ANOCWS = dr.GetValue(dr.GetOrdinal("AVOCWS")).ToString();
                            break;
                    }

                    LAPSWS = dr.GetValue(dr.GetOrdinal("LAPSWS")).ToString();
                    TICOWS = dr.GetValue(dr.GetOrdinal("TICOWS")).ToString();
                    NUCOWS = dr.GetValue(dr.GetOrdinal("NUCOWS")).ToString();
                    DOCDWS = dr.GetValue(dr.GetOrdinal("DOCDWS")).ToString();

                    result += "     Cia: " + CCIAWS + " AAPP: " + LAPSWS + " Tico: " + TICOWS + " No. Comp: " + NUCOWS + " Desc: " + DOCDWS + "\n\r";   //Falta traducir

                    registro++;
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Verifica que el lote no esté en uso
        /// </summary>
        /// <returns></returns>
        public string VerificarLoteNoEstaEnUso(string prefijoLote)
        {
            string result = "";

            string query;

            try
            {
                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLC04 ";
                query += "where PREFC4 = '" + prefijoLote + "'";

                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (registros > 0)
                {
                    result = " - " + this.LP.GetText("errLoteEnUso", "Lote en uso") + " \n\r";
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result += " - " + this.LP.GetText("errVerificarPrefLoteGLC04", "Error verificando el prefijo del lote en la tabla GLC04") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }

        /// <summary>
        /// Construye el DataTable con los números de comprobantes generados
        /// </summary>
        public void BuildDataTableNumCompGenerados()
        {
            this.numCompGenerados = new DataTable();

            try
            {
                //Adicionar las columnas al DataGrid
                this.AddColumn("archivo", typeof(System.String));
                this.AddColumn("descripcion", typeof(System.String));
                this.AddColumn("compania", typeof(System.String));
                this.AddColumn("aapp", typeof(System.String));
                this.AddColumn("tipo", typeof(System.String));
                this.AddColumn("noComp", typeof(System.String));
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
        #endregion

        #region Métodos Privados 
        private void AddColumn(string nombre, Type tipo)
        {
            DataColumn col = new DataColumn(nombre, tipo);
            this.numCompGenerados.Columns.Add(col);
        }

        /// <summary>
        /// Genera un nuevo número de comprobante
        /// </summary>
        /// <param name="compania">código de la compañía</param>
        /// <param name="aapp">año y período</param>
        /// <param name="tipo">tipo</param>
        /// <returns></returns>
        private string GenerarNoComprobante(string compania, string aapp, string tipo)
        {
            string result = "";
            bool actualizar = true;

            IDataReader dr = null;
            try
            {
                compania = compania.ToUpper();

                //Buscar el último número de comprobante en la tabla GLC13
                string query = "select NUCO13 from " + GlobalVar.PrefijoTablaCG + "GLC13 ";
                query += "where CCIA13 = '" + compania + "' and SAPR13 = " + aapp + " and TICO13 = " + tipo;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                int noComp = 0;
                if (dr.Read())
                {
                    noComp = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("NUCO13")).ToString());
                }
                else actualizar = false;
                dr.Close();

                noComp++;

                //Verificar que no exista en la tabla GLI03
                int registros = 0;
                bool numeroCompValido = false;
                while (!numeroCompValido)
                {
                    query = "select count(*) from  " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "where CCIAIC = '" + compania + "' and SAPRIC = " + aapp + " and TICOIC = " + tipo;
                    query += " and NUCOIC = " + noComp;

                    registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                    if (registros != 0) noComp++;
                    else numeroCompValido = true;
                }

                if (actualizar)
                {
                    //Actualizar el número de comprobante en la tabla GLC13
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLC13 set NUCO13 = " + noComp;
                    query += " where CCIA13 = '" + compania + "' and SAPR13 = " + aapp + " and TICO13 = " + tipo;
                }
                else
                {
                    //Insertar el número de comprobante en la tabla GLC13
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLC13";
                    query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "CCIA13, SAPR13, TICO13, NUCO13) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "'" + compania + "', " + aapp + ", " + tipo + ", " + noComp + ")";
                }
                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                result = noComp.ToString();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Inserta la cabecera del comprobante
        /// </summary>
        /// <param name="nombreTablaCab"></param>
        /// <param name="camposCab"></param>
        /// <param name="TTRAWS"></param>
        /// <param name="CCIAWS"></param>
        /// <param name="ANOCWS"></param>
        /// <param name="LAPSWS"></param>
        /// <param name="TICOWS"></param>
        /// <param name="NUCOWS"></param>
        /// <param name="TVOUWS"></param>
        /// <param name="DIAEWS"></param>
        /// <param name="MESEWS"></param>
        /// <param name="ANOEWS"></param>
        /// <param name="TASCWS"></param>
        /// <param name="TIMOWS"></param>
        /// <param name="STATWS"></param>
        /// <param name="DOCRWS"></param>
        /// <param name="DOCDWS"></param>
        private string InsertarCabeceraComprobante(string nombreTablaCab, string camposCab, string TTRAWS, string CCIAWS, string ANOCWS,
                                                 string LAPSWS, string TICOWS, string NUCOWS, string TVOUWS, string DIAEWS, string MESEWS,
                                                 string ANOEWS, string TASCWS, string TIMOWS, string STATWS, string DOCRWS, string DOCDWS)
        {
            string result = "";

            //Insertar la cabecera del comprobante
            if (TASCWS.Trim() == "") TASCWS = "0";
            TASCWS = TASCWS.ToString().Replace(".", "");
            TASCWS = TASCWS.ToString().Replace(",", ".");

            string nombreTabla = GlobalVar.PrefijoTablaCG + nombreTablaCab;
            string query = "insert into " + this.bibliotecaTablasLoteAS + nombreTabla + " (" ;
            if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
            query += camposCab + ") values (";
            if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
            query += "'" + TTRAWS + "', '" + CCIAWS + "', " + ANOCWS + ", " + LAPSWS + ", " + TICOWS + ", " + NUCOWS + ", ";
            query += TVOUWS + ", " + DIAEWS + ", " + MESEWS + ", " + ANOEWS + ", " + TASCWS + ", '" + TIMOWS + "', '";
            query += STATWS + "', '" + DOCRWS + "', '" + DOCDWS + "')";

            try
            {
                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertCabComp", "Error insertando la cabecera del comprobante") + " (CCIA: " + CCIAWS + " AAPP: " + utiles.AAPPConFormato(ANOCWS + LAPSWS) + " Tipo: " + TICOWS;
                result += " NUCO: " + NUCOWS + ") (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Obtiene los datos para los comprobantes revertidos (aapp, fecha, noComprobante)
        /// </summary>
        /// <param name="CCIAWS">compañía</param>
        /// <param name="aapp">añoperiodo</param>
        /// <param name="tipo">tipo</param>
        /// <returns>0 -> número de comprobante
        ///          1 -> aapp
        ///          2 -> fecha
        ///          3 -> aa (del aapp)
        ///          4 -> pp (del aapp)
        ///          5 -> dia (de la fecha)
        ///          6 -> mes (de la fecha)
        ///          7 -> año (de la fecha)
        ///          8 -> compañia
        ///          9 -> tipo de comprobante
        ///         10 -> descripcion
        /// </returns>
        private string[] RevertirDatosComprobante(string CCIAWS, string saapp, string tipo, string desc)
        {
            string[] result = null;
            IDataReader dr = null;
            try
            {
                //Buscar el calendario de la compañía
                string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(CCIAWS);
                string titafl = datosCompaniaAct[0];

                string query = "select SAPRFL, INLAFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titafl + "' and SAPRFL > " + saapp + " ORDER BY SAPRFL";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = new string[11];

                    string saprfl = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString().Trim();
                    if (saprfl != "" && saprfl.Length < 5 && saprfl.Length >= 2)
                    {
                        saprfl = utiles.SigloDadoAnno(saprfl.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + saprfl; 
                    }
                    result[1] = saprfl;
                    result[2] = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    result[0] = this.GenerarNoComprobante(CCIAWS, result[1], tipo);

                    //FALTA revisar los valores de la reversion para el numero de comprobante generado
                    DataRow row = this.numCompGenerados.NewRow();
                    row["archivo"] = archivo;
                    row["descripcion"] = desc + " - " + "Reversión";   //Falta traducir
                    row["compania"] = CCIAWS;

                    string aappFormato = ANOCWS + LAPSWS;
                    aappFormato = utiles.AAPPConFormato(aappFormato);

                    row["aapp"] = aappFormato;
                    row["tipo"] = TICOWS;
                    row["noComp"] = result[0];

                    this.numCompGenerados.Rows.Add(row);

                    if (result[1].Length == 5)
                    {
                        result[3] = result[1].Substring(1, 2);
                        result[4] = result[1].Substring(3, 2);
                    }
                    else
                    {
                        result[3] = result[1].Substring(0, 2);
                        result[4] = result[1].Substring(2, 2);
                    }

                    //DateTime fecha = Convert.ToDateTime(result[2]);
                    //result[5] = fecha.Day.ToString();
                    //result[6] = fecha.Month.ToString();
                    //result[7] = fecha.Year.ToString();
                    result[5] = result[2].Substring(5, 2);
                    result[6] = result[2].Substring(3, 2);
                    result[7] = result[2].Substring(1, 2);

                    if (result[7].Length > 2) result[7] = result[7].Substring(result[7].Length - 2, 2);

                    result[8] = CCIAWS;
                    result[9] = TICOWS;
                    //result[10] = DOCDWS;
                    result[10] = desc + " - " + "Reversión";
                    result[10] = result[10].Substring(0, 36);
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            return (result);
        }

        private string[] ObtenerSiguientePeriodo(string CCIAWS, string saapp)
        {
            string[] result = null;
            IDataReader dr = null;

            try
            {
                //Buscar el calendario de la compañía
                string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(CCIAWS);
                string titafl = datosCompaniaAct[0];

                string query = "select SAPRFL, INLAFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titafl + "' and SAPRFL > " + saapp + " ORDER BY SAPRFL";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = new string[2];

                    string saprfl = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString().Trim();
                    if (saprfl != "" && saprfl.Length < 5 && saprfl.Length >= 2)
                    {
                        saprfl = utiles.SigloDadoAnno(saprfl.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + saprfl; 
                    }
                    result[0] = saprfl;
                    result[1] = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    //result[0] = this.GenerarNoComprobante(CCIAWS, result[1], tipo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            return (result);
        }



        /// <summary>
        /// Insertar detalle del comprobante
        /// </summary>
        /// <param name="nombreTablaDet"></param>
        /// <param name="camposDet"></param>
        /// <param name="TTRAWS"></param>
        /// <param name="CCIAWS"></param>
        /// <param name="ANOCWS"></param>
        /// <param name="LAPSWS"></param>
        /// <param name="TICOWS"></param>
        /// <param name="NUCOWS"></param>
        /// <param name="CUENWS"></param>
        /// <param name="CAUXWS"></param>
        /// <param name="DESCWS"></param>
        /// <param name="MONTWS"></param>
        /// <param name="TMOVWS"></param>
        /// <param name="MOSMWS"></param>
        /// <param name="CLDOWS"></param>
        /// <param name="NDOCWS"></param>
        /// <param name="FDOCWS"></param>
        /// <param name="FEVEWS"></param>
        /// <param name="TEINWS"></param>
        /// <param name="NNITWS"></param>
        /// <param name="AUA1WS"></param>
        /// <param name="AUA2WS"></param>
        /// <param name="CDDOWS"></param>
        /// <param name="NDDOWS"></param>
        /// <param name="TERCWS"></param>
        /// <param name="CDIVWS"></param>
        public string InsertarDetalleComprobante(string nombreTablaDet, string camposDet, string TTRAWS, string CCIAWS, string ANOCWS,
                                                string LAPSWS, string TICOWS, string NUCOWS, string CUENWS, string CAUXWS, string DESCWS,
                                                decimal MONTWS, string TMOVWS, decimal MOSMWS, string CLDOWS, string NDOCWS, string FDOCWS,
                                                string FEVEWS, string TEINWS, string NNITWS, string AUA1WS, string AUA2WS, string CDDOWS,
                                                string NDDOWS, decimal TERCWS, string CDIVWS)
        {
            string result = "";
            try
            {
                //string MONTWS_Cad = MONTWS.ToString().Replace(".", "");
                //string MOSMWS_Cad = MOSMWS.ToString().Replace(".", "");
                //string TERCWS_Cad = TERCWS.ToString().Replace(".", "");
                string MONTWS_Cad = MONTWS.ToString().Replace(",", ".");
                string MOSMWS_Cad = MOSMWS.ToString().Replace(",", ".");
                string TERCWS_Cad = TERCWS.ToString().Replace(",", ".");
                //string MONTWS_Cad = MONTWS.ToString();
                //string MOSMWS_Cad = MOSMWS.ToString();
                //string TERCWS_Cad = TERCWS.ToString();

                if (FDOCWS == "") FDOCWS = "0";
                if (FEVEWS == "") FEVEWS = "0";

                //Insertar el detalle del comprobante
                string nombreTabla = GlobalVar.PrefijoTablaCG + nombreTablaDet;
                string query = "insert into " + this.bibliotecaTablasLoteAS + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += camposDet + ") values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + TTRAWS + "', '" + CCIAWS + "', " + ANOCWS + ", " + LAPSWS + ", " + TICOWS + ", " + NUCOWS + ", '";
                query += CUENWS + "', '" + CAUXWS + "', '" + DESCWS + "', " + MONTWS_Cad + ", '" + TMOVWS + "', " + MOSMWS_Cad + ", '";
                query += CLDOWS + "', " + NDOCWS + ", " + FDOCWS + ", " + FEVEWS + ", '" + TEINWS + "', '" + NNITWS + "', '";
                query += AUA1WS + "', '" + AUA2WS + "', '" + CDDOWS + "', " + NDDOWS + ", " + TERCWS_Cad + ", '" + CDIVWS + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertDetComp", "Error insertando el detalle del comprobante") + " (CCIA: " + CCIAWS + " AAPP: " + utiles.AAPPConFormato(ANOCWS + LAPSWS) + " Tipo: " + TICOWS;
                result += " NUCO: " + NUCOWS + ") (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Inserta el detalle en la tabla de detalles de campos extendidos
        /// </summary>
        /// <param name="nombreTablaDet"></param>
        /// <param name="camposDet"></param>
        /// <param name="TTRAWS"></param>
        /// <param name="CCIAWS"></param>
        /// <param name="ANOCWS"></param>
        /// <param name="LAPSWS"></param>
        /// <param name="TICOWS"></param>
        /// <param name="NUCOWS"></param>
        /// <param name="CUENWS"></param>
        /// <param name="CAUXWS"></param>
        /// <param name="DESCWS"></param>
        /// <param name="MONTWS"></param>
        /// <param name="TMOVWS"></param>
        /// <param name="MOSMWS"></param>
        /// <param name="CLDOWS"></param>
        /// <param name="NDOCWS"></param>
        /// <param name="FDOCWS"></param>
        /// <param name="FEVEWS"></param>
        /// <param name="TEINWS"></param>
        /// <param name="NNITWS"></param>
        /// <param name="AUA1WS"></param>
        /// <param name="AUA2WS"></param>
        /// <param name="CDDOWS"></param>
        /// <param name="NDDOWS"></param>
        /// <param name="TERCWS"></param>
        /// <param name="CDIVWS"></param>
        /// <param name="PRFDWS"></param>
        /// <param name="NFAAWS"></param>
        /// <param name="NFARWS"></param>
        /// <param name="FIVAWS"></param>
        /// <param name="USA1WS"></param>
        /// <param name="USA2WS"></param>
        /// <param name="USA3WS"></param>
        /// <param name="USA4WS"></param>
        /// <param name="USA5WS"></param>
        /// <param name="USA6WS"></param>
        /// <param name="USA7WS"></param>
        /// <param name="USA8WS"></param>
        /// <param name="USN1WS"></param>
        /// <param name="USN2WS"></param>
        /// <param name="USF1WS"></param>
        /// <param name="USF2WS"></param>
        public string InsertarDetalleComprobanteExt(string nombreTablaDet, string camposDet, string TTRAWS, string CCIAWS, string ANOCWS,
                                        string LAPSWS, string TICOWS, string NUCOWS, string CUENWS, string CAUXWS, string DESCWS,
                                        decimal MONTWS, string TMOVWS, decimal MOSMWS, string CLDOWS, string NDOCWS, string FDOCWS,
                                        string FEVEWS, string TEINWS, string NNITWS, string AUA1WS, string AUA2WS, string CDDOWS,
                                        string NDDOWS, decimal TERCWS, string CDIVWS,
                                        string PRFDWS, string NFAAWS, string NFARWS, string FIVAWS, string USA1WS, string USA2WS, string USA3WS,
                                        string USA4WS, string USA5WS, string USA6WS, string USA7WS, string USA8WS, decimal USN1WS, decimal USN2WS,
                                        string USF1WS, string USF2WS)
        {
            string result = "";
            try
            {
                //string MONTWS_Cad = MONTWS.ToString().Replace(".", "");
                //string MOSMWS_Cad = MOSMWS.ToString().Replace(".", "");
                //string TERCWS_Cad = TERCWS.ToString().Replace(".", "");
                //string USN1WS_Cad = USN1WS.ToString().Replace(".", "");
                //string USN2WS_Cad = USN2WS.ToString().Replace(".", "");
                string MONTWS_Cad = MONTWS.ToString().Replace(",", ".");
                string MOSMWS_Cad = MOSMWS.ToString().Replace(",", ".");
                string TERCWS_Cad = TERCWS.ToString().Replace(",", ".");
                string USN1WS_Cad = USN1WS.ToString().Replace(",", ".");
                string USN2WS_Cad = USN2WS.ToString().Replace(",", ".");
                //string MONTWS_Cad = MONTWS.ToString();
                //string MOSMWS_Cad = MOSMWS.ToString();
                //string TERCWS_Cad = TERCWS.ToString();
                //string USN1WS_Cad = USN1WS.ToString();
                //string USN2WS_Cad = USN2WS.ToString();

                if (FDOCWS == "") FDOCWS = "0";
                if (FEVEWS == "") FEVEWS = "0";
                if (FIVAWS == "") FIVAWS = "0";
                ///if (USN1WS == "") USN1WS = "0";
                ///if (USN2WS == "") USN2WS = "0";
                if (USF1WS == "") USF1WS = "0";
                if (USF2WS == "") USF2WS = "0";

                //Insertar el detalle del comprobante
                string nombreTabla = GlobalVar.PrefijoTablaCG + nombreTablaDet;
                string query = "insert into " + this.bibliotecaTablasLoteAS + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += camposDet + ") values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + TTRAWS + "', '" + CCIAWS + "', " + ANOCWS + ", " + LAPSWS + ", " + TICOWS + ", " + NUCOWS + ", '";
                query += CUENWS + "', '" + CAUXWS + "', '" + DESCWS + "', " + MONTWS_Cad + ", '" + TMOVWS + "', " + MOSMWS_Cad + ", '";
                query += CLDOWS + "', " + NDOCWS + ", " + FDOCWS + ", " + FEVEWS + ", '" + TEINWS + "', '" + NNITWS + "', '";
                query += AUA1WS + "', '" + AUA2WS + "', '" + CDDOWS + "', " + NDDOWS + ", " + TERCWS_Cad + ", '" + CDIVWS + "', '";
                query += PRFDWS + "', '" + NFAAWS + "', '" + NFARWS + "', " + FIVAWS + ", '" + USA1WS + "', '" + USA2WS + "', '";
                query += USA3WS + "', '" + USA4WS + "', '" + USA5WS + "', '" + USA6WS + "', '" + USA7WS + "', '" + USA8WS + "', ";
                query += USN1WS + ", " + USN2WS + ", " + USF1WS + ", " + USF2WS + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertDetComp", "Error insertando el detalle del comprobante") + " (CCIA: " + CCIAWS + " AAPP: " + utiles.AAPPConFormato(ANOCWS + LAPSWS) + " Tipo: " + TICOWS;
                result += " NUCO: " + NUCOWS + ") (" + ex.Message + ")";
            }

            return (result);
        }   
        #endregion
    }
}
