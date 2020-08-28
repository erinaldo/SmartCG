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
    /// Clase que gestiona la transferencia de comprobantes extracontables 
    /// (se utiliza desde el formulario que contiene la lista de comprobantes contables (frmCompExtContLista), 
    /// desde el formulario de transferir comprobantes contables a Finanzas (frmCompExtContTransferirFinanComprobantes) y 
    /// desde el formulario de transferir Archivos de Lote (frmCompExtContTransferirArchivoLote))
    /// </summary>
    class ComprobanteExtContableTransferir
    {
        private const string prefijoColumnaPeriodo = "PRD";

        public DataSet dsComprobante;

        public string STATHD = "";
        public string CCIAHD = "";
        public string SAPCHD = "";
        public string TICOHD = "";
        public string NUCOHD = "";
        public string SIMIHD = "";
        public string FECOHD = "";
        public string TIPLHD = "";
        public string CUENHD = "";
        public string TAX1HD = "";
        public string CAX1HD = "";
        public string TAX2HD = "";
        public string CAX2HD = "";
        public string TAX3HD = "";
        public string CAX3HD = "";
        public string SAPRHD = "";
        public string TIDAHD = "";
        public string MONTHD = "";
        public decimal monthd = 0;
        public string TMOVHD = "";
        public string DESCHD = "";

        public string tipoBaseDatosCG = "";
        public string prefijoLote = "";
        public string descripcion = "";
        public string estado = "";
        public string bibliotecaPrefijo = "";
        public string cola = "";
        public string biliotecaCola = "";

        public string archivo = "";             //Nombre del archivo
        public string descCompArchivo = "";     //Descripción del comprobante dentro del archivo

        public ArrayList noCompGenerados;   //Número de comprobantes generados

        public ArrayList tablasLotes;

        public DataTable numCompGenerados;

        public string bibliotecaTablasLoteAS;

        private Utiles utiles;
        private UtilesCGConsultas utilesCG;
        public LanguageProvider LP;
        private ILog Log;

        public ComprobanteExtContableTransferir()
        {
            this.tablasLotes = new ArrayList();

            this.utiles = new Utiles();
            this.utilesCG = new UtilesCGConsultas();
            this.Log = log4net.LogManager.GetLogger(this.GetType());

            this.dsComprobante = new DataSet();

            this.bibliotecaTablasLoteAS = "";

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
                DataColumn col = new DataColumn("transferido", typeof(System.Boolean));
                dataTable.Columns.Add(col);
                col = new DataColumn("archivo", typeof(System.String));
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
                col = new DataColumn("debeML", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("haberML", typeof(System.String));
                dataTable.Columns.Add(col);
                col = new DataColumn("noMovimiento", typeof(System.String));
                dataTable.Columns.Add(col);
            }
            catch(Exception ex)
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
                DataTable dtTot = ds.Tables.Add("Totales");
                DataColumn dcTot = dtTot.Columns.Add("MonedaLocalDebe");
                dcTot = dtTot.Columns.Add("MonedaLocalHaber");
                dcTot = dtTot.Columns.Add("NumeroApuntes");

                ds.ReadXml(pathFichero, XmlReadMode.IgnoreSchema); //solo lee del xml los datos del esquema definido

                //Verificar que exista la tabla de Cabecera
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                {
                    //Verificar la marca contable para asegurar que sea un comprobante extracontable
                    if (ds.Tables["Cabecera"].Rows[0]["Contable"].ToString() == "0")
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
                            string debeML = "";
                            string haberML = "";
                            string noMovimiento = "";

                            try { compania = ds.Tables["Cabecera"].Rows[0]["Compania"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { aaPP = ds.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString(); if (aaPP.Length >= 2) aaPP = aaPP.Insert(2, "-"); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { descripcion = ds.Tables["Cabecera"].Rows[0]["Descripcion"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { tipo = ds.Tables["Cabecera"].Rows[0]["Tipo"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { noComp = ds.Tables["Cabecera"].Rows[0]["Numero"].ToString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            try { fecha = ds.Tables["Cabecera"].Rows[0]["Fecha"].ToString(); if (fecha != "") fecha = utiles.FormatoCGToFecha(fecha).ToShortDateString(); }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            //Verificar que exista la tabla de Totales
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Totales"].Rows.Count > 0)
                            {
                                try { debeML = ds.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { haberML = ds.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                try { noMovimiento = ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString(); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            }

                            //Insertar una fila en el DataTable
                            DataRow row = dataTable.NewRow();
                            row["archivo"] = nombreFichero;
                            row["compania"] = compania;
                            row["aapp"] = aaPP;
                            row["descripcion"] = descripcion;
                            row["tipo"] = tipo;
                            row["noComp"] = noComp;
                            row["fecha"] = fecha;
                            if (transferido == "1") row["transferido"] = true;
                            else row["transferido"] = false;
                            row["debeML"] = debeML;
                            row["haberML"] = haberML;
                            row["noMovimiento"] = noMovimiento;

                            dataTable.Rows.Add(row);
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
        /// Transferir el comprobante extracontable
        /// </summary>
        /// <returns></returns>
        public string TransferirComprobante()
        {
            string result = "";
            try
            {
                string saapp = "";

                string[] noCompNuevo = new string[5];

                //Crear el ArrayList donde se almacenarán los números de comprobantes generados
                this.noCompGenerados = new ArrayList();

                bool existeCabecera = false;
                bool existeDetalle = false;
                
                //Verificar que exista la tabla de cabecera
                if (this.dsComprobante != null && this.dsComprobante.Tables != null && this.dsComprobante.Tables.Count > 0 && 
                    this.dsComprobante.Tables["Cabecera"].Rows.Count > 0)
                {
                    existeCabecera = true;
                    this.CCIAHD = this.dsComprobante.Tables["Cabecera"].Rows[0]["Compania"].ToString().Trim();

                    saapp = this.dsComprobante.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                    if (saapp.Length == 4)
                    {
                        string aa = saapp.Substring(0, 2);
                        saapp = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + saapp;
                    }
                    this.SAPCHD = saapp;

                    this.TICOHD = this.dsComprobante.Tables["Cabecera"].Rows[0]["Tipo"].ToString().Trim();
                    this.NUCOHD = this.dsComprobante.Tables["Cabecera"].Rows[0]["Numero"].ToString().Trim();

                    string fecha = this.dsComprobante.Tables["Cabecera"].Rows[0]["Fecha"].ToString().Trim();
                    if (fecha != "")
                    {
                        this.FECOHD = utiles.FechaToFormatoCG(utiles.FormatoCGToFecha(fecha), true).ToString();
                    }
                    
                    //Buscar el plan de la compañía (el método devuelve además el calendario)
                    string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(this.CCIAHD);
                    this.TIPLHD = datosCompaniaAct[1];
                }

                //Verificar que exista la tabla de Detalle
                if (this.dsComprobante != null && this.dsComprobante.Tables != null && this.dsComprobante.Tables.Count > 0 && 
                    this.dsComprobante.Tables["Detalle"].Rows.Count > 0)
                {
                    existeDetalle = true;
                }

                if (existeCabecera && existeDetalle)
                {
                    this.STATHD = "";

                    if (this.NUCOHD == "")
                    {
                        this.NUCOHD = this.GenerarNoComprobante(CCIAHD, saapp, TICOHD);
                        noCompNuevo[0] = CCIAHD;
                        noCompNuevo[1] = saapp;
                        noCompNuevo[2] = TICOHD;
                        noCompNuevo[3] = NUCOHD;
                        noCompNuevo[4] = NUCOHD;


                        DataRow row = this.numCompGenerados.NewRow();
                        row["archivo"] = archivo;
                        row["descripcion"] = this.descCompArchivo;
                        row["compania"] = CCIAHD;
                        
                        string aappFormato =  this.dsComprobante.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                        aappFormato = utiles.AAPPConFormato(aappFormato);
                        
                        row["aapp"] = aappFormato;
                        row["tipo"] = TICOHD;
                        row["noComp"] = NUCOHD;

                        this.numCompGenerados.Rows.Add(row);

                        this.noCompGenerados.Add(noCompNuevo);
                    }

                    int contador = 0;
                    string insertarCompResult = "";
                    for (int i = 0; i < this.dsComprobante.Tables["Detalle"].Rows.Count; i++)
                    {
                        if (!TodaFilaEnBlanco(this.dsComprobante.Tables["Detalle"], i))
                        {
                            this.CUENHD = this.dsComprobante.Tables["Detalle"].Rows[i]["Cuenta"].ToString().Trim();

                            this.CAX1HD = this.dsComprobante.Tables["Detalle"].Rows[i]["Auxiliar1"].ToString().Trim();
                            this.CAX2HD = this.dsComprobante.Tables["Detalle"].Rows[i]["Auxiliar2"].ToString().Trim();
                            this.CAX3HD = this.dsComprobante.Tables["Detalle"].Rows[i]["Auxiliar3"].ToString().Trim();

                            if (this.CUENHD != "" && (this.CAX1HD != "" || this.CAX2HD != "" || this.CAX3HD != "")) this.ObtenerTiposCuentasAux();
                            else
                            {
                                this.TAX1HD = "";
                                this.TAX2HD = "";
                                this.TAX3HD = "";
                            }

                            this.TIDAHD = this.dsComprobante.Tables["Detalle"].Rows[i]["TipoExt"].ToString().Trim();
                            this.DESCHD = this.dsComprobante.Tables["Detalle"].Rows[i]["Descripcion"].ToString().Trim();
                            this.TMOVHD = this.dsComprobante.Tables["Detalle"].Rows[i]["DH"].ToString().Trim();

                            string nombreColumnaActual = "";
                            for (int j = 0; j < this.dsComprobante.Tables["Detalle"].Columns.Count; j++)
                            {
                                nombreColumnaActual = this.dsComprobante.Tables["Detalle"].Columns[j].ColumnName;
                                if (nombreColumnaActual.Length > 3)
                                {
                                    if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                    {
                                        
                                        this.MONTHD = this.dsComprobante.Tables["Detalle"].Rows[i][nombreColumnaActual].ToString().Trim();

                                        if(this.MONTHD != "" && decimal.Parse(this.MONTHD) != 0)
                                        //if (this.MONTHD != "" && this.MONTHD != "0")
                                        {
                                            //this.SAPRHD = nombreColumnaActual.Replace(prefijoColumnaPeriodo, "");
                                            saapp = nombreColumnaActual.Replace(prefijoColumnaPeriodo, "");
                                            if (saapp.Length == 4)
                                            {
                                                string aa = saapp.Substring(0, 2);
                                                saapp = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + saapp;
                                            }
                                            this.SAPRHD = saapp;

                                            monthd = decimal.Parse(this.MONTHD);

                                            contador++;
                                            this.SIMIHD = contador.ToString();

                                            insertarCompResult = this.InsertarComprobante();
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errTransferComprob", "Error transfiriendo el comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Obtienes los tipos de auxiliares para la cuenta de mayor
        /// </summary>
        private void ObtenerTiposCuentasAux()
        {
            IDataReader dr = null;
            try
            {
                this.TAX1HD = "";
                this.TAX2HD = "";
                this.TAX3HD = "";

                //Buscar los tipos de las cuentas de auxiliar
                string query = "select STATMC, TCUEMC, ADICMC, SASIMC, ";
                query += "FEVEMC, NDDOMC, TERMMC, TIMOMC, TAU1MC, TAU2MC, TAU3MC, ";
                query += "TDOCMC, GRUPMC, RNITMC, TERMMC ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where CUENMC = '" + CUENHD + "' and TIPLMC = '" + TIPLHD + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    if (this.CAX1HD != "") this.TAX1HD = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString();

                    if (this.CAX2HD != "") this.TAX2HD = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString();

                    if (this.CAX3HD != "") this.TAX3HD = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString();
                }
                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Devuelve si todas las columnas están vacías dado una fila de un DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="row">fila</param>
        /// <returns></returns>
        private bool TodaFilaEnBlanco(DataTable table, int row)
        {
            bool todaFilaBlanco = true;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Rows[row][i].ToString() != "")
                {
                    if (table.Columns[i].ColumnName != "TipoËxt")
                    {
                        todaFilaBlanco = false;
                        break;
                    }
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// Inserta el comprobante en la tabla prefijo del lote + W02
        /// </summary>
        /// <returns></returns>
        private string InsertarComprobante()
        {
            string result = "";
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + this.prefijoLote + "W02";

                //if (MONTHD == "") MONTHD = "0";
                MONTHD = monthd.ToString().Replace(",", ".");

                //Insertar el comprobante en la tabla PrefijoLote + W02
                string query = "insert into " + this.bibliotecaTablasLoteAS + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATHD, CCIAHD, SAPCHD, TICOHD, NUCOHD, SIMIHD, FECOHD, TIPLHD, CUENHD, TAX1HD, CAX1HD, TAX2HD, CAX2HD,";
                query += "TAX3HD, CAX3HD, SAPRHD, TIDAHD, MONTHD, TMOVHD, DESCHD) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'', '" + CCIAHD + "', " + SAPCHD + ", " + TICOHD + ", " + NUCOHD + ", " + SIMIHD + ", " + FECOHD + ", '" + TIPLHD + "', '";
                query += CUENHD + "', '" + TAX1HD + "', '" + CAX1HD + "', '" + TAX2HD + "', '" + CAX2HD + "', '";
                query += TAX3HD + "', '" + CAX3HD + "', " + SAPRHD + ", '" + TIDAHD + "', " + MONTHD + ", '" + TMOVHD + "', '" + DESCHD + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = ex.Message;
            }

            return (result);
        }

        /// <summary>
        /// Inserta un registro en la tabla GLC04
        /// </summary>
        public string InsertarRegistroPRC04()
        {
            string result = "";
            try
            {
                string STATC4 = "";
                string PREFC4 = this.prefijoLote;
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
                string nombreTabla = GlobalVar.PrefijoTablaCG + "PRC04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATC4,PREFC4,USERC4,WSGEC4,DESCC4,LIBLC4,LIBEC4,OUTQC4,LIBQC4) values(";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + STATC4 + "', '" + PREFC4 + "', '" + USERC4 + "', '" + WSGEC4 + "', '" + DESCC4 + "', '" + LIBLC4 + "', '";
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


                        /* llamar a GL21G */
                        string comando = "CALL PGM(GL21G)";
                        string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";
                        string longitudComando = comando.Length.ToString();

                        sentencia = sentencia + longitudComando.PadLeft(10, '0');
                        sentencia = sentencia + ".00000)";

                        GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);
                        //command.CommandText = "CALL " + this.bibliotecaTablasLoteAS + "GL21G', '    ')";
                        //command.ExecuteNonQuery();

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

                result = this.LP.GetText("errInsertPRC04", "Error insertando en la tabla PRC04") + " (" + ex.Message + ")";
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
            IDataReader dr = null;
            try
            {
                if (tipoBaseDatosCG != "DB2") bibliotecaTablasLoteAS = "";
                else
                {
                    if (this.bibliotecaPrefijo != "") bibliotecaTablasLoteAS = this.bibliotecaPrefijo + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                string nombreTabla = this.prefijoLote + "W02";

                string query = "select * from " + this.bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + nombreTabla;

                int registro = 0;
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string CCIAHD = "";
                string SAPCHD = "";
                string TICOHD = "";
                string NUCOHD = "";
                string FECOHD = "";

                string cciaActual = "";
                string aappActual = "";
                string ticoActual = "";
                string numCompActual = "";
                string fechaAct = "";
                string fechaConFormato = "";
                string aappConFormato = "";

                while (dr.Read())
                {
                    if (registro == 0)
                    {
                        this.tablasLotes.Add(nombreTabla);
                        result += "Existen datos del lote en la Tabla: " + nombreTabla + "\n\r";  //Falta traducir
                    }

                    CCIAHD = dr.GetValue(dr.GetOrdinal("CCIAHD")).ToString();
                    SAPCHD = dr.GetValue(dr.GetOrdinal("SAPCHD")).ToString();
                    TICOHD = dr.GetValue(dr.GetOrdinal("TICOHD")).ToString();
                    NUCOHD = dr.GetValue(dr.GetOrdinal("NUCOHD")).ToString();
                    FECOHD = dr.GetValue(dr.GetOrdinal("FECOHD")).ToString();

                    if (CCIAHD != cciaActual || SAPCHD != aappActual || TICOHD != ticoActual || NUCOHD != numCompActual || FECOHD != fechaAct)
                    {
                        fechaConFormato = utiles.FormatoCGToFecha(FECOHD).ToShortDateString();
                        aappConFormato = utiles.AAPPConFormato(SAPCHD);

                        result += "     Cia: " + CCIAHD + " AAPP: " + aappConFormato + " Tico: " + TICOHD + " No. Comp: " + NUCOHD + " Fecha: " + fechaConFormato + "\n\r";   //Falta traducir
                        
                        cciaActual = CCIAHD;
                        aappActual = SAPCHD;
                        ticoActual = TICOHD;
                        numCompActual = NUCOHD;
                        fechaAct = FECOHD;
                    }
                    registro++;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

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
                string query = "delete from " + this.bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + nombreTabla;

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

                //Verificar que no exista en la tabla PRB01
                int registros = 0;
                bool numeroCompValido = false;
                while (!numeroCompValido)
                {
                    query = "select count(*) from  " + GlobalVar.PrefijoTablaCG + "PRB01 ";
                    query += "where CCIAP3 = '" + compania + "' and SAPCP3 = " + aapp + " and TICOP3 = " + tipo;
                    query += " and NUCOP3 = " + noComp;

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
  
        #endregion
    }
}