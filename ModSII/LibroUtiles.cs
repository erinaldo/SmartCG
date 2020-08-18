using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    static class LibroUtiles
    {
        //Código internos para los libros
        public const string LibroID_FacturasEmitidas = "01";
        public const string LibroID_FacturasRecibidas = "03";
        public const string LibroID_BienesInversion = "05";
        public const string LibroID_CobrosMetalico = "07";
        public const string LibroID_OperacionesIntracomunitarias = "09";
        public const string LibroID_CobrosEmitidas = "11";
        public const string LibroID_PagosRecibidas = "13";
        public const string LibroID_OperacionesSeguros = "15";
        public const string LibroID_AgenciasViajes = "17";

        //Código del período Anual
        public const string PeriodoAnual = "0A";

        public static Dictionary<string, string> ListaSIIDesc = new Dictionary<string, string>();

        /// <summary>
        /// Devuelve la descripción del valor solicitado (para las listas fijas de hacienda)
        /// </summary>
        /// <returns></returns>
        public static string ListaSII_Descripcion(string tipcd, string codcd, string resod)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                //Formar la clave para buscar en el diccionario (tipcd + codcd a 2 posiciones + resod si no es nulo)
                string codigo2Pos = codcd.Trim().PadLeft(2, '0');
                string listaKey = tipcd + codigo2Pos;

                if (resod != null) listaKey += resod;

                //Buscar en código en el diccionario
                if (ListaSIIDesc.Count > 0 && ListaSIIDesc.ContainsKey(listaKey))
                {
                    if(ListaSIIDesc.TryGetValue(listaKey, out result)) return (result);
                }

                //Prefijo para la tabla de Descripciones (CGCDES)
                string prefijoTablaCGCDES = "";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2)
                {
                    prefijoTablaCGCDES = ConfigurationManager.AppSettings["bbddCGAPP"];
                    if (prefijoTablaCGCDES != null && prefijoTablaCGCDES != "") prefijoTablaCGCDES += ".";
                }
                else prefijoTablaCGCDES = GlobalVar.PrefijoTablaCG;

                string query = "select * from " + prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD = '" + tipcd + "' and ";
                query += "CODCD = '" + codcd + "' ";

                if (resod != null) query += "and RESOD = '" + resod + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }

                dr.Close();

                //Insertarlo en el Diccionario
                ListaSIIDesc.Add(listaKey, result);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve Si (valor = S), (valor = N) o cadena vacía si valor distinto a los anteriores
        /// </summary>
        /// <returns></returns>
        public static string ListaSII_SiNo(string valor)
        {
            string result = "";

            try
            {
                switch (valor)
                {
                    case "S":
                        result = "Si";
                        break;
                    case "N":
                        result = "No";
                        break;
                    default:
                        result = valor;
                        break;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Dado la hora de la tabla de Log, la devuelve con formato (hh:mm:ss)
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static string HoraLogFormato(string hora)
        {
            string result = hora;
            try
            {
                if (hora != "")
                {
                    string aux = hora.ToString().PadLeft(6, '0');
                    result = aux.Substring(0, 2) + ":" + aux.Substring(2, 2) + ":" + aux.Substring(4, 2);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el último movimiento de la factura
        /// </summary>
        public static DataTable ObtenerUltimoMovimiento(string idLibro, string emisorFacturaNif, 
                                                        string emisorFacturaIdOtroCodPais, string emisorFacturaIdOtroIdType,
                                                        string emisorFacturaIdOtroId, string numeroFactura,
                                                        string fechaFactura, string cargoAbono)
        {
            IDataReader dr = null;
            DataTable dataTable = null;
            try
            {
                //Obtener la consulta
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVLSII  ";
                query += "where TDOCL1 = '" + idLibro + "' ";

                if (emisorFacturaNif != "") query += "and NIFEL1 = '" + emisorFacturaNif + "' ";
                else
                {
                    if (emisorFacturaIdOtroCodPais != "") query += "and PAISL1 = '" + emisorFacturaIdOtroCodPais + "' ";
                    query += "and TIDEL1 = '" + emisorFacturaIdOtroIdType + "' ";
                    query += "and IDOEL1 = '" + emisorFacturaIdOtroId + "' ";
                }

                if (numeroFactura != "") query += "and NSFEL1 = '" + numeroFactura + "' ";

                if (fechaFactura != "") query += "and FDOCL1 = " + fechaFactura + " ";

                if (cargoAbono != "") query += "and TPCGL1 = '" + cargoAbono + "' ";

                query += "order by DATEL1 DESC, TIMEL1 DESC";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                DataRow dataRow;

                if (dr.Read())
                {
                    DataTable schemaTable = dr.GetSchemaTable();
                    dataTable = new DataTable();

                    foreach (DataRow row in schemaTable.Rows)
                    {
                        string colName = row.Field<string>("ColumnName");
                        Type t = row.Field<Type>("DataType");
                        dataTable.Columns.Add(colName, t);
                    }
                    
                    dataRow = dataTable.NewRow();

                    foreach (DataColumn col in dataTable.Columns)
                    {
                        dataRow[col.ColumnName] = dr[col.ColumnName];
                    }

                    dataTable.Rows.Add(dataRow);
                }               
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dataTable);
        }

        /// <summary>
        /// Dado una fecha en formato CG (saammdd, aammdd, aaaammdd) devuelve la fecha en formato SII (dd-mm-yyyy)
        /// </summary>
        /// <param name="fecha">Fecha Formato CG</param>
        /// <returns></returns>
        public static string FormatoCGToFechaSii(string fecha)
        {
            string fechaSii = "";

            try
            {
                if (fecha != "")
                {
                    if (fecha.Length == 6 || fecha.Length == 7)
                    {
                        int fechaAux = Convert.ToInt32(fecha) + 19000000;
                        string fechaRes = fechaAux.ToString();

                        fechaSii = fechaRes.Substring(6, 2) + "-" + fechaRes.Substring(4, 2) + "-" + fechaRes.Substring(0, 4);
                    }
                    else if (fecha.Length == 8)
                    {
                        fechaSii = fecha.Substring(6, 2) + "-" + fecha.Substring(4, 2) + "-" + fecha.Substring(0, 4);
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (fechaSii);
        }

        /// <summary>
        /// Valida si una fecha en formato SII (dd-mm-yyyy) es correcta
        /// </summary>
        /// <param name="fechaSii">Fecha en formato SII</param>
        /// <param name="fechaDateTime">Devuleve la fecha SII en formato DateTime</param>
        /// <returns></returns>
        public static bool FormatoFechaSiiValid(ref string fechaSii, ref DateTime fechaDateTime)
        {
            bool result = true;
            fechaDateTime = new DateTime();
            try
            {
                if (fechaSii != "")
                {
                    string dia = "";
                    string mes = "";
                    string anno = "";
                    bool completarAnno = false;

                    if (fechaSii.Length < 8) return (false);
                    
                    dia = fechaSii.Substring(0, 2);
                    mes = fechaSii.Substring(3, 2);

                    if (fechaSii.Length == 8) 
                    {
                        anno = "20" + fechaSii.Substring(6, 2);
                        completarAnno = true;
                    }
                    else anno = fechaSii.Substring(6, 4);

                    fechaDateTime = new DateTime(Convert.ToInt16(anno), Convert.ToInt16(mes), Convert.ToInt16(dia));

                    if (completarAnno) fechaSii = dia + "-" + mes + "-" + anno;
                }
            }
            catch (Exception ex) 
            { 
                result = false;
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); 
            }

            return (result);
        }

        /// <summary>
        /// Devuelve la descripción dado un valor de estado de los posibles (" ", "V", "E", "W")
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string EstadoDescripcion(string valor)
        {
            string result = "";

            switch (valor)
            {
                case " ":
                case "":
                    result = "PendienteEnvio";
                    break;
                case "V":
                    result = "Correcta";
                    break;
                case "W":
                    result = "AceptadaConErrores";
                    break;
                case "E":
                    result = "Incorrecta";
                    break;
            }

            return (result);
        }

        /// <summary>
        /// Valida si el periodo es valid o no
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public static bool ValidarPeriodo(string periodo)
        {
            bool result = true;

            if (periodo.Trim() != "")
            {
                if (periodo.Length == 1) periodo = "0" + periodo;

                string[] periodoArray = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", PeriodoAnual, "1T", "2T", "3T", "4T" };

                if (!periodoArray.Any(periodo.Contains)) result = false;

            }
            return (result);
        }

        #region Respuesta Envios
        /// <summary>
        /// Devuelve un DataTable con la estructura de la respuesta del WebService
        /// </summary>
        /// <returns></returns>
        public static DataTable CrearDataTableResultado()
        {
            DataTable result = new DataTable();
            try
            {
                result.TableName = "Resultado";

                //Adicionar las columnas al DataTable
                result.Columns.Add("Compania", typeof(string));
                //result.Columns.Add("Ejercicio", typeof(string));
                //result.Columns.Add("Periodo", typeof(string));
                result.Columns.Add("Libro", typeof(string));
                result.Columns.Add("Operacion", typeof(string));
                result.Columns.Add("Estado", typeof(string));
                result.Columns.Add("NIFIdEmisor", typeof(string)); //Cabecera Grid: NIF/Id Emisor  (too vendra contraparte nif)
                result.Columns.Add("NoFactura", typeof(string));
                result.Columns.Add("FechaDoc", typeof(string));
                result.Columns.Add("NombreRazonSocial", typeof(string));        //Mostrar solo si es un libro donde se utiliza
                result.Columns.Add("ClaveOperacion", typeof(string));           //Mostrar solo para el libro de operaciones de seguro
                result.Columns.Add("NIF", typeof(string));
                result.Columns.Add("IdOtroCodPais", typeof(string));
                result.Columns.Add("IdOtroTipo", typeof(string));
                result.Columns.Add("IdOtroId", typeof(string));
                result.Columns.Add("RowResumen", typeof(string));
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        #region Exportar Consulta Datos via HTML
        public static void GridExportarHTML(ref TGGrid grid, string libro, string tipoConsulta)
        {
            try
            {
                string titulo = "Consulta Datos " + tipoConsulta;
                
                switch (libro)
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:
                        titulo += " - Facturas Emitidas";
                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:
                        titulo += " - Facturas Recibidas";
                        break;
                    case LibroUtiles.LibroID_BienesInversion:
                        titulo += " - Bienes de Inversión";
                        break;
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        titulo += " - Operaciones Intracomunitarias";
                        break;
                    case LibroUtiles.LibroID_PagosRecibidas:
                        titulo += " - Pagos Recibidas RECC";
                        break;
                    case LibroUtiles.LibroID_CobrosMetalico:
                        titulo += " - Cobros en metálico";
                        break;
                    case LibroUtiles.LibroID_OperacionesSeguros:
                        titulo += " - Operaciones de seguros";
                        break;
                    /*case LibroUtiles.LibroID_CobrosEmitidas:
                        titulo += " - Cobros Emitidas";
                        break;*/
                    case LibroUtiles.LibroID_AgenciasViajes:
                        titulo += " - Agencias de viajes";
                        break;
                }

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < grid.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = grid.Columns[i].HeaderText;  //Nombre de la columna

                    switch (grid.Columns[i].Name)
                    {
                        case "VER":
                        case "MOV":
                        case "PAGO":
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = "0";   //No Visible
                            break;
                        case "ImporteTotal":
                        case "BaseImponible":
                        case "Cuota":
                        case "CuotaDeducible":
                        case "Importe":
                        case "PagoImporte":
                            nombreTipoVisible[1] = "decimal";
                            nombreTipoVisible[2] = grid.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        case "CodigoErrorRegistro":
                            nombreTipoVisible[1] = "numero";
                            nombreTipoVisible[2] = grid.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = grid.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                HTMLCrear(ref documento_HTML);

                HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                HTMLDatos(ref documento_HTML, descColumnas, ref grid);

                HTMLFin(ref documento_HTML);

                string ficheroHTML = ConsultaNombreFichero("SIIConsulta" + tipoConsulta);

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "EXCEL";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
                catch // si no puede levantar excel, levantar html
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "IEXPLORE";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve el nombre del fichero temporal que se creará para la consulta de datos en local o de la AEAT. Elimina los ficheros creados anteriormente
        /// </summary>
        /// <param name="formCode">Nombre del fichero</param>
        /// <param name="path">Ruta donde se encuentran los ficheros generados anteriormente</param>
        /// <returns></returns>
        public static string ConsultaNombreFichero(string nombre)
        {
            string result = "";
            string path = System.Windows.Forms.Application.StartupPath;

            //Verificar que exista la carpeta tmp, sino crearla
            string pathTmp = "";
            try
            {
                //Chequear si no existe la carpeta
                if (!System.IO.Directory.Exists(path + "\\tmp")) System.IO.Directory.CreateDirectory(path + "\\tmp");

                pathTmp = "\\tmp";
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            if (pathTmp != "") path = path + pathTmp;

            try
            {
                string[] extensions = new[] { ".html" };
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] files =
                    dirInfo.EnumerateFiles()
                         .Where(f => extensions.Contains(f.Extension.ToLower()))
                         .ToArray();

                foreach (System.IO.FileInfo ficheroActual in files)
                {
                    try
                    {
                        //Chequear que la extension sea .html
                        ficheroActual.Delete();
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }

            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            //Devolver el nombre del fichero
            result = path + "\\" + nombre + "_" + System.Environment.UserName.ToUpper() + ".html";

            if (FileInUse(result))
            {
                //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                DateTime localDate = DateTime.Now;
                string fecha = localDate.Year.ToString() + localDate.Month.ToString() + localDate.Day.ToString() + "_";
                string hora = DateTime.Now.ToString("hh:mm:ss");
                hora = hora.Replace(":", "");
                fecha = fecha + hora;

                result = path + "\\" + nombre + "_" + System.Environment.UserName.ToUpper() + "_" + fecha + ".html";
            }

            return (result);
        }

        /// <summary>
        /// Devuelve si un fichero está en uso
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileInUse(string path)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate))
                {
                    bool canW = fs.CanWrite;
                }
                return false;
            }
            catch { return true; }
        }

        /// <summary>
        /// Crear los tags iniciales del fichero HTML
        /// </summary>
        /// <param name="documento_HTML"></param>
        public static void HTMLCrear(ref StringBuilder documento_HTML)
        {
            try
            {
                documento_HTML.Append("<html>\n");
                documento_HTML.Append(" <head>\n");
                documento_HTML.Append(" <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\"/>\n");
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crear el encabezado del HTML
        /// </summary>
        /// <param name="documento_HTML"></param>
        /// <param name="columnas"></param>
        /// <param name="titulo"></param>
        public static void HTMLEncabezado(ref StringBuilder documento_HTML, ArrayList columnas, string titulo)
        {
            documento_HTML.Append("     <title>" + titulo + "</title>\n");
            documento_HTML.Append("     <style>\n");
            documento_HTML.Append("        .NumeroCG {mso-number-format:\\#\\,\\#\\#0\\.00;text-align=right;}\n");
            documento_HTML.Append("        .NumeroCGLeft {mso-number-format:\"0\";text-align:left;}\n");
            documento_HTML.Append(@"        .Texto    { mso-number-format:\@; }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTIT    { mso-number-format:\@;font-weight:700; background-color:#D8D8D8 }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTITCab    { mso-number-format:\@;font-weight:700; background-color:#C5D9F1}");
            documento_HTML.Append("\n");
            documento_HTML.Append("     </style>\n");
            documento_HTML.Append(" </head>\n");
            documento_HTML.Append(" <body>\n");
            documento_HTML.Append("     <b>" + titulo + "</b>\n");

            documento_HTML.Append("     <table width =\"100%\">\n");
            documento_HTML.Append("         <tr>\n");

            string[] desColumna;
            string nombreColumna = "";
            for (int i = 0; i < columnas.Count; i++)
            {
                desColumna = (string[])columnas[i];
                if (desColumna[2] == "1")   //columna visible
                {
                    nombreColumna = ((string[])columnas[i])[0];
                    documento_HTML.Append("             <td class=TextoTITCab valign=\"top\">" + nombreColumna + "</td>\n");
                }
            }

            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("     </table>\n");
        }

        /// <summary>
        /// ordenar selectedrows
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int DataGridViewRowIndexCompare(System.Windows.Forms.DataGridViewRow x, System.Windows.Forms.DataGridViewRow y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Index.CompareTo(y.Index);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        return x.Index.CompareTo(y.Index);
                    }
                }
            }
        }

        /// <summary>
        /// Escribir la tabla con los datos de la Grid en el HTML según la selección
        /// </summary>
        /// <param name="documento_HTML"></param>
        /// <param name="columnas"></param>
        /// <param name="grid"></param>
        public static void HTMLDatos(ref StringBuilder documento_HTML, ArrayList columnas, ref TGGrid grid)
        {
            if (grid.SelectedRows.Count <= 0) grid.SelectAll();

            //Devolver las filas seleccionadas con el mismo orden que fueron seleccionadas en la Grid
            List<System.Windows.Forms.DataGridViewRow> SelectedRows = new List<System.Windows.Forms.DataGridViewRow>();
            foreach (System.Windows.Forms.DataGridViewRow dr in grid.SelectedRows)
                SelectedRows.Add(dr);
            SelectedRows.Sort(DataGridViewRowIndexCompare);

            documento_HTML.Append("     <table width =\"100%\">\n");

            string[] desColumna;
            object valorStr;
            decimal valor;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("es-ES");

            for (int i = 0; i < grid.SelectedRows.Count; i++)
            {
                documento_HTML.Append("         <tr>\n");
                for (int j = 0; j < columnas.Count; j++)
                {
                    desColumna = (string[])columnas[j];
                    if (desColumna[2] == "1")   //columna visible
                    {
                        switch (desColumna[1])
                        {
                            case "decimal":
                                valorStr = SelectedRows[i].Cells[j].Value;
                                try
                                {
                                    string valorStrTemp = valorStr.ToString();
                                    int pos = valorStrTemp.IndexOf(",");
                                    if (valorStrTemp.Length - pos > 3)
                                    {
                                        valor = Convert.ToDecimal(valorStr, System.Globalization.CultureInfo.InvariantCulture);
                                        documento_HTML.Append("             <td class=NumeroCG valign=\"top\" align=\"right\">" + valor.ToString("N2", ci) + "</td>\n");
                                    }
                                    else
                                    {
                                        valor = Convert.ToDecimal(valorStr);
                                        documento_HTML.Append("             <td class=NumeroCG valign=\"top\" align=\"right\">" + valor.ToString("N2") + "</td>\n");
                                    }
                                }
                                catch
                                {
                                    documento_HTML.Append("             <td class=NumeroCG valign=\"top\" align=\"right\">" + valorStr + "</td>\n");
                                }
                                break;
                            case "numero":
                                documento_HTML.Append("             <td class=NumeroCGLeft valign=\"top\" align=\"left\">" + SelectedRows[i].Cells[j].Value + "</td>\n");
                                break;
                            default:
                                documento_HTML.Append("             <td class=Texto valign=\"top\">" + SelectedRows[i].Cells[j].Value + "</td>\n");
                                break;
                        }
                    }
                }
                documento_HTML.Append("         </tr>\n");
            }

            documento_HTML.Append("     </table>\n");
        }

        /// <summary>
        /// Escribir los tags finales del HTML
        /// </summary>
        /// <param name="documento_HTML"></param>
        public static void HTMLFin(ref StringBuilder documento_HTML)
        {
            documento_HTML.Append(" </body>\n");
            documento_HTML.Append("</html>\n");
        }
        #endregion

        /// <summary>
        /// Crea un DataTable con las Propiedades (en este caso, con los atributos de una clase)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable PropertiesToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable dt = new DataTable();
            var props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                DataColumn dc = dt.Columns.Add(prop.Name, prop.PropertyType);
                dc.Caption = prop.DisplayName;
                dc.ReadOnly = prop.IsReadOnly;
            }
            foreach (T item in source)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor prop in props)
                {
                    dr[prop.Name] = prop.GetValue(item);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Dado un código de compañía fiscal, devuelve el NIF de la misma
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static string NifCompania(string codigo)
        {
            string nif = "";
            IDataReader dr = null;

            try
            {
                if (codigo != "")
                {
                    string query = "select NIFDT3 from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "where CIAFT3 = '" + codigo + "' ";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nif = dr.GetValue(dr.GetOrdinal("NIFDT3")).ToString().Trim();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }

            return nif;
        }

        /// <summary>
        /// Devuelve el nombre o razon social de la compañía fiscal dado el nif de la misma
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        public static string ObtenerNombreRazonSocialCiaFiscalDadoNIF(string nif)
        {
            string nombreRazonSocial = "";
            IDataReader dr = null;

            try
            {
                if (nif != "")
                {
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "where NIFDT3 = '" + nif + "' ";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nombreRazonSocial = dr.GetValue(dr.GetOrdinal("NOMBT3")).ToString().Trim();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }
            return (nombreRazonSocial);
        }

        /// <summary>
        /// Devuelve la url del servicio web que conecta con el SII)
        /// </summary>
        /// <returns></returns>
        /// <param name="agencia"></param>
        /// <param name="entorno"></param>
        /// <returns></returns>
        public static string ObtenerURLServicioWebActivo(string agencia, string entorno)
        {
            string result = "";
            IDataReader dr = null;
            
            try
            {
                string query = "select URENAG from " + GlobalVar.PrefijoTablaCG + "IVSAGE ";
                query += "where AGENAG = '" + agencia + "' and ";
                query += "TIPOAG = '" + entorno + "' ";
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("URENAG")).ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica que la url sel servicio web exista
        /// </summary>
        /// <param name="uriInput"></param>
        /// <returns></returns>
        public static bool IsReachableUri(string uriInput)
        {
            // Variable to Return
            bool testStatus = false;
            try
            {
                // Create a request for the URL.
                System.Net.WebRequest request = System.Net.WebRequest.Create(uriInput);
                request.Timeout = 15000; // 15 Sec

                System.Net.WebResponse response;
                try
                {
                    response = request.GetResponse();
                    testStatus = true; // Uri does exist                 
                    response.Close();
                }
                catch
                {
                    testStatus = false; // Uri does not exist
                }

                return (testStatus);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (testStatus);
        }

        /// <summary>
        /// Graba la nueva dirección del servicio web
        /// </summary>
        public static void GrabarURLServicioWebSIIConfig(string uriServicioWebSII)
        {
            try
            {
                //Actualizar la ruta para los ficheros
                // Get the current configuration file.
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;

                ConfigurationSectionGroup applicationSectionGroup = config.GetSectionGroup("applicationSettings");

                ConfigurationSection applicationConfigSection = applicationSectionGroup.Sections["ModSII.Properties.Settings"];
                ClientSettingsSection clientSection = (ClientSettingsSection)applicationConfigSection;

                //WebService Configuration Setting
                SettingElement applicationSetting = clientSection.Settings.Get("ModSII_tgSIIWebService_TGsiiService");
                //Falta verificar la URL que sea válida
                applicationSetting.Value.ValueXml.InnerXml = uriServicioWebSII;

                applicationConfigSection.SectionInformation.ForceSave = true;
                config.Save();
                //ConfigurationManager.RefreshSection("applicationSettings/ModSII.Properties.Settings");
                Properties.Settings.Default.Reload();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
    }
}
