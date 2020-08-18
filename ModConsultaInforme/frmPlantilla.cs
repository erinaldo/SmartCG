using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;
using log4net;
using System.IO;
using Telerik.WinControls.FileDialogs;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;

namespace ModConsultaInforme
{
    public partial class frmPlantilla : Telerik.WinControls.UI.RadForm, IReLocalizable
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        protected UtilesCGConsultas utilesCG;

        protected Autorizaciones aut;

        //Separador para los campos del formulario que son del tipo codigo - descripcion
        protected string separadorDesc = " - ";

        protected bool selectAll = false;

        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre
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

        public frmPlantilla()
        {
            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            //Traducir literales propios del selector de directorios
            FileDialogsLocalizationProvider.CurrentProvider = new RadOpenFolderDialogLocalizationProviderES();

            //Traducir literales propios de la Grid
            RadGridLocalizationProvider.CurrentProvider = new RadGridLocalizationProviderES();
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }

        private void FrmPlantilla_Load(object sender, EventArgs e)
        {
            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();
        }

        /// <summary>
        /// Valida los caracteres del nombre del fichero. Si encuentra caracteres no validos los sustituye por '_'
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MakeValidFileName(string name)
        {
            /*string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
            */
            string fileNAmeOK = "";
            fileNAmeOK = Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '_'));

            return (fileNAmeOK);
        }

        /// <summary>
        /// Devuelve el nombre del fichero temporal que se creará. Elimina los ficheros creados anteriormente
        /// </summary>
        /// <param name="codigoFormFicheros">Codificación para los ficheros según tipo de informe</param>
        /// <param name="path">Ruta donde se encuentran los ficheros generados anteriormente</param>
        /// <returns></returns>
        public string InformeNombreFichero(string codigoFormFicheros, string path, string titulo)
        {
            //string nombreFicheroComun = "SmartCGFileView_";

            /*
            try
            {
                //Eliminar todos los ficheros que se han generado anteriormente
                string[] ficherosCarpeta = System.IO.Directory.GetFiles(@path);
                foreach (string ficheroActual in ficherosCarpeta)
                {
                    if (ficheroActual.Contains(nombreFicheroComun))
                    {
                        try
                        {
                            System.IO.File.Delete(ficheroActual);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }*/

            if (path != "" && path[(path.Length-1)] != '\\') path += @"\";

            //string nombreFichero = nombreFicheroComun;
            string nombreFichero = "";
            titulo = titulo.Trim();
            if (titulo != "")
            {
                nombreFichero += codigoFormFicheros + "_" + MakeValidFileName(titulo);
            }
            else
            {
                nombreFichero += codigoFormFicheros + "_" + System.Environment.UserName.ToUpper();
            }

            //Devolver el nombre del fichero
            string result = path + nombreFichero + ".html";

            //if (FileInUse(result))
            //if (File.Exists(result))
            //{
                //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                DateTime localDate = DateTime.Now;
                string fecha = localDate.Year.ToString() + localDate.Month.ToString().PadLeft(2, '0') + localDate.Day.ToString().PadLeft(2, '0').ToString() + "_";
                string hora = DateTime.Now.ToString("HH:mm:ss");
                hora = hora.Replace(":", "");
                fecha += hora;

                result = path + nombreFichero + "_" + fecha + ".html";
                //error = "Archivo en uso";
            //}

            return (result);
        }

        /// <summary>
        /// Devuelve el nombre del fichero temporal que se creará. Elimina los ficheros creados anteriormente
        /// </summary>
        /// <param name="formCode">Nombre del fichero</param>
        /// <param name="path">Ruta donde se encuentran los ficheros generados anteriormente</param>
        /// <returns></returns>
        public string InformeSoloNombreFichero(string formCode, string path, string titulo)
        {
            /*
try
{
//Eliminar todos los ficheros que se han generado anteriormente
string[] ficherosCarpeta = System.IO.Directory.GetFiles(@path);
foreach (string ficheroActual in ficherosCarpeta)
{
if (ficheroActual.Contains("nombreFicheroComun"))
{
try
{
System.IO.File.Delete(ficheroActual);
}
catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
}
}
}
catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }*/

            if (path != "" && path[(path.Length - 1)] != '\\') path += @"\";
            titulo = titulo.Trim();

            string nombreFichero;
            if (titulo != "")
            {
                nombreFichero = MakeValidFileName(titulo);
            }
            else
            {
                nombreFichero = formCode + "_" + System.Environment.UserName.ToUpper();
            }

            //Devolver el nombre del fichero
            string resultAux = path + nombreFichero + ".html";

            string result;
            if (FileInUse(resultAux))
            {
                //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                DateTime localDate = DateTime.Now;
                string fecha = localDate.Year.ToString() + localDate.Month.ToString() + localDate.Day.ToString() + "_";
                string hora = DateTime.Now.ToString("hh:mm:ss");
                hora = hora.Replace(":", "");
                fecha += hora;

                result = nombreFichero + "_" + fecha;
                //error = "Archivo en uso";
            }
            else result = nombreFichero;

            return (result);
        }

        /// <summary>
        /// Devuelve el nombre del fichero que se creará (temporal) o que se sugiere como nombre. Elimina los ficheros creados anteriormente
        /// </summary>
        /// <param name="formCode">Nombre del fichero</param>
        /// <param name="path">Ruta donde se encuentran los ficheros generados anteriormente</param>
        /// <returns></returns>
        public string InformeGetNombreFichero(string formCode, string path, string titulo)
        {
            try
            {
                //Eliminar todos los ficheros que se han generado anteriormente
                string[] ficherosCarpeta = System.IO.Directory.GetFiles(@path);
                foreach (string ficheroActual in ficherosCarpeta)
                {
                    try
                    {
                        System.IO.File.Delete(ficheroActual);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (path != "" && path[(path.Length - 1)] != '\\') path += @"\";
            titulo = titulo.Trim();

            string nombreFichero;
            if (titulo != "")
            {
                nombreFichero = MakeValidFileName(titulo);
            }
            else
            {
                nombreFichero = formCode + "_" + System.Environment.UserName.ToUpper();
            }

            //Devolver el nombre del fichero
            string pathNombreFichero = path + nombreFichero + ".html";

            string result;
            if (FileInUse(pathNombreFichero))
            {
                //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                DateTime localDate = DateTime.Now;
                string fecha = localDate.Year.ToString() + localDate.Month.ToString() + localDate.Day.ToString() + "_";
                string hora = DateTime.Now.ToString("hh:mm:ss");
                hora = hora.Replace(":", "");
                fecha += hora;

                result = nombreFichero + "_" + fecha;
            }
            else result = nombreFichero;

            return (result);
        }

        /// <summary>
        /// Devuelve si un fichero est
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool FileInUse(string path)
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
        /// Exportar los elementos seleccionados
        /// </summary>
        public void ExportarGrid(ref Telerik.WinControls.UI.RadGridView grid, string tituloInformeConsulta, bool informe, string nombreFichero, string hojaExcelNombre, ref ArrayList gridColumnas, string subtitulo)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            bool ningunaFilaSeleccionada = false;
            if (grid.SelectedRows.Count == 0)
            {
                //Exportar Todo
                grid.SelectAll();
                ningunaFilaSeleccionada = true;
            }
            else
            {
                if (grid.CurrentRow is GridViewGroupRowInfo)
                {
                    if (grid.CurrentRow.IsExpanded) grid.CurrentRow.IsExpanded = false;
                    else grid.CurrentRow.IsExpanded = true;
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            if (grid.CurrentRow is GridViewDataRowInfo || ningunaFilaSeleccionada)
            {
                ExportFileType exportarTipoFichero = ExportFileType.EXCEL;
                string exportarTipoFicheroStr;
                try
                {
                    if (informe)
                        exportarTipoFicheroStr = GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes;
                    else
                        exportarTipoFicheroStr = GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosConsultas;

                    //Verificar que sean de los valores posibles (EXCEL/HTML/PDF)
                    exportarTipoFichero = (ExportFileType)System.Enum.Parse(typeof(ExportFileType), exportarTipoFicheroStr);
                }
                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                string pathFichero = "";
                try
                {
                    if (informe)
                        pathFichero = GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosInformes;
                    else
                        pathFichero = GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosConsultas;
                }
                catch (Exception ex)
                {
                    GlobalVar.Log.Error(ex.Message);
                }

                if (informe) this.selectAll = true;

                bool visualizarFichero = false;
                /*try
                {
                    if (informe)
                        visualizarFichero = GlobalVar.UsuarioEnv.ModConsInfo_VisualizarFicheroInformes;
                    else
                        visualizarFichero = GlobalVar.UsuarioEnv.ModConsInfo_VisualizarFicheroConsultas;
                }
                catch (Exception ex)
                {
                    GlobalVar.Log.Error(ex.Message);
                }*/

                ExportTelerik exportarConTelerik = new ExportTelerik(ref grid)
                {
                    Titulo = tituloInformeConsulta,
                    ExportToMemory = visualizarFichero,
                    ExportType = exportarTipoFichero,
                    PathFichero = pathFichero,
                    SelectAll = this.selectAll
                };

                if (nombreFichero != "" && nombreFichero != null) exportarConTelerik.NombreFichero = nombreFichero;

                if (exportarConTelerik.ExportType == ExportFileType.EXCEL)
                {
                    exportarConTelerik.NombreHojaExcel_CaptionText = hojaExcelNombre;
                    if (gridColumnas != null) exportarConTelerik.GridColumnas = gridColumnas;
                }

                if (subtitulo != "" || subtitulo != null) exportarConTelerik.Subtitulo = subtitulo;
                _ = exportarConTelerik.Export();
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        #region Formularios Informes
        public string AddToListBox(string texto, ref ListBox controlListBox)
        {
            string result = "";

            for (int i = 0; i < controlListBox.Items.Count; i++)
            {
                if (controlListBox.Items[i].ToString() == texto)
                {
                    result = "1";   //El texto ya existe en la lista
                    break;
                }
            }

            if (result == "")
            {
                controlListBox.Items.Add(texto);
            }

            return (result);
        }

        public string AddToListBox(string texto, ref Telerik.WinControls.UI.RadListControl controlListBox)
        {
            string result = "";

            for (int i = 0; i < controlListBox.Items.Count; i++)
            {
                if (controlListBox.Items[i].ToString() == texto)
                {
                    result = "1";   //El texto ya existe en la lista
                    break;
                }
            }

            if (result == "")
            {
                controlListBox.Items.Add(texto);
            }

            return (result);
        }

        /// <summary>
        /// Llenar un Desplegable
        /// </summary>
        /// <param name="query">Sentencia SQL</param>
        /// <param name="campoCodigo">campo codigo de la select</param>
        /// <param name="campoDesc">campo descripción de la select</param>
        /// <param name="control">control ComboBox (se pasa por referencia)</param>
        /// <param name="CodDesc">True si se visualiza codigo - descripcion y False si solol se visualiza descripcion</param>
        /// <param name="indiceSel">Indice del ComboBox que se activará</param>
        /// <returns></returns>
        public string FillComboBox(string query, string campoCodigo, string campoDesc, ref ComboBox control, bool CodDesc, int indiceSel)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                ArrayList elementos = new ArrayList();
                string nombre = "";
                string codigo = "";
                while (dr.Read())
                {
                    //Falta chequear autorizacion
                    nombre = dr[campoDesc].ToString().Trim();
                    codigo = dr[campoCodigo].ToString().Trim();
                    if (CodDesc) elementos.Add(new AddValue(codigo + " - " + nombre, codigo));
                    else elementos.Add(new AddValue(nombre, codigo));
                }

                dr.Close();

                control.DisplayMember = "Display";
                control.ValueMember = "Value";
                control.DataSource = elementos;

                control.SelectedIndex = indiceSel;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                //Error obteniendo los grupos
                result = ex.Message;

                if (dr != null) dr.Close();
            }

            return(result);
        }

        /// <summary>
        /// Obtiene las compañias
        /// </summary>
        /// <param name="result">Cadena vacia si no hubo error, en caso contrario se devuelve el mensaje de la excepción</param>
        /// <returns>DataReader con la compañias</returns>
        public IDataReader ObtenerCompanias(ref string result)
        {
            result = "";
            string query = "Select CCIAMG, NCIAMG From " + GlobalVar.PrefijoTablaCG  + "GLM01 Order by CCIAMG";

            IDataReader dr = null;
                
            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = ex.Message;
            }
            
            return(dr);
        }

        /// <summary>
        /// Obtiene los grupos de ompañias
        /// </summary>
        /// <param name="result">Cadena vacia si no hubo error, en caso contrario se devuelve el mensaje de la excepción</param>
        /// <returns>DataReader con los grupos de compañias</returns>
        public IDataReader ObtenerGrupos(ref string result)
        {
            result = "";
            
            IDataReader dr = null;

            try
            {
                string query = "Select GRUPGR, NOMBGR From " + GlobalVar.PrefijoTablaCG + "GLM07 Order by GRUPGR";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = ex.Message;
            }

            return (dr);
        }

        /// <summary>
        /// Devuelve el código y la descripción de la cadena que se le pasa con el formato codigo - descripcion
        /// </summary>
        /// <param name="codigo">devuelve el código</param>
        /// <param name="desc">devuelve la descripción</param>
        /// <param name="valor">valor, cadena seleccionada</param>
        /// <param name="caracter">caracter separador</param>
        protected void ObtenerCodigoDescDadoCombo(ref string codigo, ref string desc, string valor, char caracter)
        {
            try
            {
                string[] aValor = valor.Split(caracter);
                if (aValor.Length >= 2)
                {
                    codigo = aValor[0].Trim();
                    desc = aValor[1].Trim();
                }
                else
                {
                    codigo = "";
                    desc = "";
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Valida la existencia o no de la compañía</returns>


        /// <summary>
        /// Valida la existencia o no de la compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        /// <param name="descripcion">descripción de la compañía</param>
        /// <param name="saldos">true -> si es consulta de saldos    false -> si es consulta de movimientos</param>
        /// <returns></returns>
        public string ValidarCompania(string codigo, ref string descripcion, bool saldos)
        {
            IDataReader dr = null;

            string result;
            try
            {
                //Comprobar que la compañía es válida
                string query = "select NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString().Trim();
                }
                else
                {
                    //Error la compañía no es válida
                    result = this.LP.GetText("lblfrmCompContErrComp", "La compañía no es válida");
                }

                dr.Close();

                if (result == "")
                {
                    //Verificar autorizaciones
                    string grupo = "02";
                    if (saldos) grupo = "05";
                    bool operarConsulta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "002", grupo, "10", codigo);
                    if (!operarConsulta)
                    {
                        //Error usuario no autorizado
                        result = this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a esta compañía");   //Falta traducir
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        /// <param name="descripcion">descripción de la compañía</param>
        /// <param name="codigoPlan">código del plan</param>
        /// <param name="saldos">true -> si es consulta de saldos    false -> si es consulta de movimientos</param>
        /// <returns>Valida la existencia o no de la compañía</returns>
        public string ValidarCompaniaCodPlan(string codigo, ref string descripcion, ref string codigoPlan, bool saldos)
        {
            IDataReader dr = null;
            codigoPlan = "";

            string result;
            try
            {
                //Comprobar que la compañía es válida
                string query = "select NCIAMG, TIPLMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString().Trim();
                    codigoPlan = dr.GetValue(dr.GetOrdinal("TIPLMG")).ToString().Trim();
                }
                else
                {
                    //Error la compañía no es válida
                    result = this.LP.GetText("lblfrmCompContErrComp", "La compañía no es válida");
                }

                dr.Close();

                if (result == "")
                {
                    //Verificar autorizaciones
                    string grupo = "02";
                    if (saldos) grupo = "05";
                    bool operarConsulta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "002", grupo, "10", codigo);
                    if (!operarConsulta)
                    {
                        //Error usuario no autorizado
                        result = this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a esta compañía");   //Falta traducir
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del grupo de compañías
        /// </summary>
        /// <param name="codigo">código del grupo de compañías</param>
        /// <param name="descripcion">descripción del grupo de compañías</param>
        /// <param name="saldos">true -> si es consulta de saldos    false -> si es consulta de movimientos</param>
        /// <returns>Valida la existencia o no del grupo de compañías</returns>
        public string ValidarGrupo(string codigo, ref string descripcion, bool saldos)
        {
            IDataReader dr = null;

            string result;
            try
            {
                //Comprobar que el grupo de compañías es válido
                string query = "select NOMBGR from " + GlobalVar.PrefijoTablaCG + "GLM07 ";
                query += "where GRUPGR = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NOMBGR")).ToString().Trim();
                }
                else
                {
                    //Error el grupo de compañías no es válidó
                    result = this.LP.GetText("lblfrmCompContErrGrupo", "El grupo de compañías no es válido");   //Falta traducir
                }

                dr.Close();

                //Verificar autorizaciones
                string grupo = "02";
                if (saldos) grupo = "05";
                bool operarConsulta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "001", grupo, "10", codigo);
                if (!operarConsulta)
                {
                    //Error usuario no autorizado
                    result = this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este grupo");   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrGrupoExcep", "Error al validar el grupo de compañías") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del plan
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Valida la existencia o no del plan de compañías</returns>
        public string ValidarPlan(string codigo, ref string descripcion)
        {
            IDataReader dr = null;

            string result;
            try
            {
                //Comprobar que el plan es válido
                string query = "select NOMBMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NOMBMP")).ToString().Trim();
                }
                else
                {
                    //Error el plan no es válido
                    result = this.LP.GetText("lblfrmCompContErrPlan", "El plan no es válido");   //Falta traducir
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrPlanExcep", "Error al validar el plan") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del tipo de auxiliar
        /// </summary>
        /// <param name="codigo">código del tipo de auxiliar</param>
        /// <param name="codigo">descripción del tipo de auxiliar</param>
        /// <param name="saldos">true -> si es consulta de saldos    false -> si es consulta de movimientos</param>
        /// <returns>Valida la existencia o no del tipo de auxiliar</returns>
        public string ValidarTipoAuxiliar(string codigo, ref string descripcion, bool saldos)
        {
            IDataReader dr = null;

            string result;
            try
            {
                //Comprobar que el tipo de auxiliar es válido
                string query = "select NOMBMT from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NOMBMT")).ToString().Trim();
                }
                else
                {
                    //Error el tipo de auxiliar no es válido
                    result = this.LP.GetText("lblfrmCompContErrTipoAux", "El tipo de auxiliar no es válido");   //Falta traducir
                }

                dr.Close();

                if (result == "")
                {
                    //Verificar autorizaciones
                    string grupo = "02";
                    if (saldos) grupo = "03";
                    bool operarConsulta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", grupo, "10", codigo);
                    if (!operarConsulta)
                    {
                        //Error usuario no autorizado
                        result = this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a esta compañía");   //Falta traducir
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrTipoExcep", "Error al validar el tipo de auxiliar") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la compañía fiscal
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Valida la existencia o no de la compañía fiscal</returns>
        public string ValidarCompaniaFiscal(string codigo, ref string descripcion)
        {
            IDataReader dr = null;

            string result;
            try
            {
                //Comprobar que la compañía fiscal es válido
                string query = "select NOMBT3 from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                query += "where CIAFT3 = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = "";
                    descripcion = dr.GetValue(dr.GetOrdinal("NOMBT3")).ToString().Trim();
                }
                else
                {
                    //Error el tipo de auxiliar no es válido
                    result = this.LP.GetText("lblfrmCompContErrCompFiscal", "La compañía fiscal no es válida");   //Falta traducir
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompFiscalExcep", "Error al validar la compañía fiscal") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }  


        /// <summary>
        /// Validar un período en el formulario de entrada de datos
        /// </summary>
        /// <param name="txtPeriodo"></param>
        /// <returns></returns>
        public string ValidarPeriodo(ref TextBox txtPeriodo)
        {
            string result = "";

            if (txtPeriodo.Text.Trim() == "")
            {
                result = this.LP.GetText("errPeriodoDesdeObl", "Es obligatorio informar el periodo Desde");
                txtPeriodo.Focus();
                return (result);
            }

            //Validar los periodos
            if (txtPeriodo.Text.Length != 4)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            string periodo = txtPeriodo.Text.Substring(2, 2);
            bool errorPeriodo = false;
            try
            {
                int periodoInt = Convert.ToInt16(periodo);
                if (!(periodoInt >= 1 && periodoInt <= 99))
                {
                    errorPeriodo = true;
                }
            }
            catch (Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errorPeriodo = true; 
            }

            if (errorPeriodo)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            return (result);
        }

        /// <summary>
        /// Validar un período en el formulario de entrada de datos
        /// </summary>
        /// <param name="txtPeriodo"></param>
        /// <returns></returns>
        public string ValidarPeriodoFormato(ref MaskedTextBox txtPeriodo, bool periodoDesde, ref string saapp)
        {
            string result = "";
            txtPeriodo.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string periodo = txtPeriodo.Text;
            txtPeriodo.TextMaskFormat = MaskFormat.IncludeLiterals;

            if (periodo.Trim() == "")
            {
                if (periodoDesde) result = this.LP.GetText("errPeriodoDesdeObl", "Es obligatorio informar el periodo Desde");
                else result = this.LP.GetText("errPeriodoHastaObl", "Es obligatorio informar el periodo Hasta");
                txtPeriodo.Focus();
                return (result);
            }

            //Validar los periodos
            if (periodo.Length != 4)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            string periodoAA = periodo.Substring(0, 2);
            string periodoPP = periodo.Substring(2, 2);
            bool errorPeriodo = false;
            try
            {
                int periodoInt = Convert.ToInt16(periodoPP);
                if (!(periodoInt >= 1 && periodoInt <= 99))
                {
                    errorPeriodo = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errorPeriodo = true;
            }

            if (errorPeriodo)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            string siglo = utiles.SigloDadoAnno(periodoAA, CGParametrosGrles.GLC01_ALSIRC);
            saapp = siglo + periodoAA + periodoPP;

            return (result);
        }

        /// <summary>
        /// Validar un período en el formulario de entrada de datos
        /// </summary>
        /// <param name="txtPeriodo"></param>
        /// <returns></returns>
        public string ValidarPeriodoFormato(ref Telerik.WinControls.UI.RadMaskedEditBox txtPeriodo, bool periodoDesde, ref string saapp)
        {
            string result = "";
            txtPeriodo.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string periodo = txtPeriodo.Value.ToString().Trim();
            txtPeriodo.TextMaskFormat = MaskFormat.IncludeLiterals;

            if (periodo.Trim() == "")
            {
                if (periodoDesde) result = this.LP.GetText("errPeriodoDesdeObl", "Es obligatorio informar el periodo Desde");
                else result = this.LP.GetText("errPeriodoHastaObl", "Es obligatorio informar el periodo Hasta");
                txtPeriodo.Focus();
                return (result);
            }

            //Validar los periodos
            if (periodo.Length != 4)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            string periodoAA = periodo.Substring(0, 2);
            string periodoPP = periodo.Substring(2, 2);
            bool errorPeriodo = false;
            try
            {
                int periodoInt = Convert.ToInt16(periodoPP);
                if (!(periodoInt >= 1 && periodoInt <= 99))
                {
                    errorPeriodo = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errorPeriodo = true;
            }

            if (errorPeriodo)
            {
                result = this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto");
                txtPeriodo.Focus();
                return (result);
            }

            string siglo = utiles.SigloDadoAnno(periodoAA, CGParametrosGrles.GLC01_ALSIRC);
            saapp = siglo + periodoAA + periodoPP;

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuentas de mayor
        /// </summary>
        /// <param name="lbCuentasMayor">Lista de cuentas de mayor</param>
        /// <param name="cuentasAux">cuenta de mayor no válida</param>
        /// <param name="indice">índice de la cuenta de mayor no válida dentro de la lista </param>
        /// <param name="codAux">código del plan </param>
        /// <returns></returns>
        public string ValidarCuentasMayor(ref ListBox lbCuentasMayor, ref string cuentasMayor, ref int indice, string codPlan)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                string query = "";
                string cuentaActual = "";
                for (int i = 0; i < lbCuentasMayor.Items.Count; i++)
                {
                    cuentaActual = lbCuentasMayor.Items[i].ToString();

                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaActual + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (!dr.Read())
                    {
                        cuentasMayor = cuentaActual;
                        indice = i;
                        dr.Close();
                        result = this.LP.GetText("lblfrmPlantillaErrCtasMayor", "La cuenta de mayor no es válida (" + cuentaActual + ")");   //Falta traducir
                        break;
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaMayorExcep", "Error al validar la cuenta de mayor ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuentas de mayor
        /// </summary>
        /// <param name="lbCuentasMayor">Lista de cuentas de mayor</param>
        /// <param name="cuentasAux">cuenta de mayor no válida</param>
        /// <param name="indice">índice de la cuenta de mayor no válida dentro de la lista </param>
        /// <param name="codAux">código del plan </param>
        /// <returns></returns>
        public string ValidarCuentasMayor(ref Telerik.WinControls.UI.RadListControl lbCuentasMayor, ref string cuentasMayor, ref int indice, string codPlan)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                string query = "";
                string cuentaActual = "";
                for (int i = 0; i < lbCuentasMayor.Items.Count; i++)
                {
                    cuentaActual = lbCuentasMayor.Items[i].ToString();

                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaActual + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (!dr.Read())
                    {
                        cuentasMayor = cuentaActual;
                        indice = i;
                        dr.Close();
                        result = this.LP.GetText("lblfrmPlantillaErrCtasMayor", "La cuenta de mayor no es válida (" + cuentaActual + ")");   //Falta traducir
                        break;
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaMayorExcep", "Error al validar la cuenta de mayor ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuentas de auxiliar
        /// </summary>
        /// <param name="lbCuentasMayor">Lista de cuentas auxiliares</param>
        /// <param name="cuentasAux">cuenta de auxiliar no válida</param>
        /// <param name="indice">índice de la cuenta de auxiliar no válida dentro de la lista </param>
        /// <param name="codAux">código del tipo de auxiliar </param>
        /// <returns></returns>
        public string ValidarCuentasAuxiliar(ref Telerik.WinControls.UI.RadListControl lbCuentasAux, ref string cuentasAux, ref int indice, string codAux)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                string query = "";
                string cuentaActual = "";
                for (int i = 0; i < lbCuentasAux.Items.Count; i++)
                {
                    cuentaActual = lbCuentasAux.Items[i].ToString();

                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + codAux + "' and CAUXMA = '" + cuentaActual + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (!dr.Read())
                    {
                        cuentasAux = cuentaActual;
                        indice = i;
                        dr.Close();
                        result = this.LP.GetText("lblfrmPlantillaErrCtasAux ", "La cuenta de auxiliar no es válida (" + cuentaActual + ")");   //Falta traducir
                        break;
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaAuxExcep", "Error al validar la cuenta de auxiliar ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuenta de mayor
        /// </summary>
        /// <param name="cuentaMayor">cuenta de mayor</param>
        /// <param name="codPlan">código del plan</param>
        /// <param name="tipoCta">tipo de la cuenta de mayor (T -> titulo, D -> detalle)</param>
        /// <returns></returns>
        public string ValidarCuentaMayor(string cuentaMayor, string codPlan, ref string tipoCta)
        {
            string result = "";
            tipoCta = "";
            IDataReader dr = null;

            try
            {
                /*string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03, " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                query += "where TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaMayor + "' and ";
                query += "TIPLMC = TIPLMX and CUENMC = CUENMX";
                */

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "left join " + GlobalVar.PrefijoTablaCG + "GLMX3 on ";
                query += "TIPLMC = TIPLMX and CUENMC = CUENMX where ";
                query += "TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaMayor + "'";

                string sconmc = "";
                string grctmx = "";
                bool autCuenta = false;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    sconmc = dr["SCONMC"].ToString();
                    grctmx = dr["GRCTMX"].ToString().Trim();
                    tipoCta = dr["TCUEMC"].ToString().Trim();
                }
                else result = this.LP.GetText("lblfrmPlantillaErrCtaMayor", "La cuenta de mayor no es válida");   //Falta traducir
 
                dr.Close();

                if (result == "")
                {
                    //Chequear autorizaciones
                    if (sconmc != "0") autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "003", "02", "0" + sconmc, codPlan);
                    else autCuenta = true;

                    if (!autCuenta) result = "Usuario no autorizado para algunas cuentas de mayor";  //Falta traducir
                    else
                    {
                        //Chequear autorización a grupos de cuentas de mayor
                        if (grctmx != "")
                        {
                            autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "008", "02", "10", codPlan + grctmx);
                            if (!autCuenta) result = "Usuario no autorizado para algunos grupos de cuentas de mayor";  //Falta traducir
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaMayorExcep", "Error al validar la cuenta de mayor ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuenta de mayor
        /// </summary>
        /// <param name="cuentasMayor">cuenta de mayor no válida</param>
        /// <param name="codAux">código del plan </param>
        /// <param name="descripcion">nombre de la cuenta</param>
        /// <param name="descripcion">posicion del código de auxiliar (1-primero, 2-segundo, 3-tercero o -1-indiferente)</param>
        /// <param name="tipoCta">tipo de la cuenta de mayor (T -> titulo, D -> detalle)</param>
        /// <returns></returns>
        public string ValidarCuentaMayor(string cuentaMayor, string codPlan, ref string descripcion, string tipoAux, int posAux, ref string tipoCta)
        {
            tipoCta = "";
            IDataReader dr = null;

            string result;
            try
            {
                result = this.ValidarCuentaMayor(cuentaMayor, codPlan, ref tipoCta);
                if (result != "") return (result);

                if (tipoAux != "")
                {
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaMayor + "'";

                    switch (posAux)
                    {
                        case 1:
                            query += " and TAU1MC = '" + tipoAux + "'";
                            break;
                        case 2:
                            query += " and TAU2MC = '" + tipoAux + "'";
                            break;
                        case 3:
                            query += " and TAU3MC = '" + tipoAux + "'";
                            break;
                        default:
                            query += " and (TAU1MC = '" + tipoAux + "' or TAU2MC = '" + tipoAux + "' or TAU3MC = '" + tipoAux + "')";
                            break;
                    }

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        result = "";
                        descripcion = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();
                    }
                    else
                    {
                        result = this.LP.GetText("lblfrmPlantillaErrCtaMayorAux", "La cuenta de mayor no utiliza el tipo de auxiliar o no en la posición indicada");   //Falta traducir
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaMayorExcep", "Error al validar la cuenta de mayor ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de las cuenta de mayor y devuelve el tipo (titulo, detalle) y documento (si lleva o no documento)
        /// </summary>
        /// <param name="cuentasMayor">cuenta de mayor no válida</param>
        /// <param name="codPlan">código del plan </param>
        /// <param name="descripcion">nombre de la cuenta</param>
        /// <param name="tipoAux">código del tipo de auxiliar</param>
        /// <param name="posAux">posicion del código de auxiliar (1-primero, 2-segundo, 3-tercero o -1-indiferente)</param>
        /// <param name="tipoCta">tipo de la cuenta de mayor (T -> titulo, D -> detalle)</param>
        /// <param name="documento">S -> cuenta de mayor con documento, N -> cuenta de mayor sin documento</param>
        /// <returns></returns>
        public string ValidarCuentaMayorTipoDocumento(string cuentaMayor, string codPlan, ref string descripcion, string tipoAux, int posAux, 
                                                      ref string tipoCuenta, ref string documento)
        {
            tipoCuenta = "";
            documento = "";

            IDataReader dr = null;

            string result;
            try
            {
                result = this.ValidarCuentaMayor(cuentaMayor, codPlan, ref tipoCuenta);
                if (result != "") return (result);

                if (tipoAux != "")
                {
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + codPlan + "' and CUENMC = '" + cuentaMayor + "'";

                    if (tipoCuenta == "D")
                    {
                        switch (posAux)
                        {
                            case 1:
                                query += " and TAU1MC = '" + tipoAux + "'";
                                break;
                            case 2:
                                query += " and TAU2MC = '" + tipoAux + "'";
                                break;
                            case 3:
                                query += " and TAU3MC = '" + tipoAux + "'";
                                break;
                            default:
                                query += " and (TAU1MC = '" + tipoAux + "' or TAU2MC = '" + tipoAux + "' or TAU3MC = '" + tipoAux + "')";
                                break;
                        }
                    }

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        result = "";
                        descripcion = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();

                        tipoCuenta = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString().Trim();
                        documento = dr.GetValue(dr.GetOrdinal("TDOCMC")).ToString().Trim();
                    }
                    else
                    {
                        result = this.LP.GetText("lblfrmPlantillaErrCtaMayorAux", "La cuenta de mayor no utiliza el tipo de auxiliar o no en la posición indicada");   //Falta traducir
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaMayorExcep", "Error al validar la cuenta de mayor ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la cuenta de auxiliar
        /// </summary>
        /// <param name="cuentasAux">cuenta de auxiliar no válida</param>
        /// <param name="codAux">código del tipo de auxiliar </param>
        /// <returns></returns>
        public string ValidarCuentaAuxiliar(string cuentaAux, string codAux)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (cuentaAux != "99999999")
                {
                    string grctma = "";
                    bool autCuenta = false;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + codAux + "' and CAUXMA = '" + cuentaAux + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        grctma = dr["GRCTMA"].ToString().Trim();
                    }
                    else result = this.LP.GetText("lblfrmPlantillaErrCtaAux ", "La cuenta de auxiliar no es válida");   //Falta traducir 

                    dr.Close();

                    if (result == "")
                    {
                        //Chequear autorización a grupos de cuentas de auxiliar
                        if (grctma != "")
                        {
                            autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "007", "03", "10", codAux + grctma);       //saldos ?? o movimientos ??
                            if (!autCuenta) result = "Usuario no autorizado para algunos grupos de cuentas de auxiliar";  //Falta traducir
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaAuxExcep", "Error al validar la cuenta de auxiliar ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// Valida la existencia o no de la cuenta de auxiliar
        /// </summary>
        /// <param name="cuentasAux">cuenta de auxiliar no válida</param>
        /// <param name="codAux">código del tipo de auxiliar </param>
        /// <param name="descripcion">nombre de la cuenta de auxiliar </param>
        /// <returns></returns>
        public string ValidarCuentaAuxiliar(string cuentaAux, string codAux, ref string descripcion)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (cuentaAux != "99999999")
                {
                    string grctma = "";
                    bool autCuenta = false;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + codAux + "' and CAUXMA = '" + cuentaAux + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        descripcion = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString().Trim();
                        grctma = dr["GRCTMA"].ToString().Trim();
                    }
                    else
                    {
                        result = this.LP.GetText("lblfrmPlantillaErrCtaAux ", "La cuenta de auxiliar no es válida");   //Falta traducir
                    }

                    if (result == "")
                    {
                        //Chequear autorización a grupos de cuentas de auxiliar
                        if (grctma != "")
                        {
                            autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "007", "03", "10", codAux + grctma);       //saldos ?? o movimientos ??
                            if (!autCuenta) result = "Usuario no autorizado para algunos grupos de cuentas de auxiliar";  //Falta traducir
                        }
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmPlantillaErrCtaAuxExcep", "Error al validar la cuenta de auxiliar ") + " (" + ex.Message + ")";  //Falta traducir
                if (dr != null) dr.Close();
            }

            return (result);
        }
        #endregion

        #region Barra Progreso
        /// <summary>
        /// Inicializa la barra de progreso
        /// </summary>
        protected void IniciaBarraProgreso(ref NewProgressBar pBarProcesandoInfo, ref Telerik.WinControls.UI.RadLabel lblProcesandoInfo, ProgressBarStyle estilo)
        {        
            pBarProcesandoInfo.Value = 0;
            pBarProcesandoInfo.MarqueeAnimationSpeed = 30;
            pBarProcesandoInfo.Style = estilo;
            pBarProcesandoInfo.Visible = true;
            lblProcesandoInfo.Text = this.LP.GetText("lblProcesandoInfo", "Procesando informe");
            lblProcesandoInfo.Visible = true;
            lblProcesandoInfo.Refresh();
        }

        /// <summary>
        /// Inicializa la barra de progreso
        /// </summary>
        protected void IniciaBarraProgreso(ref ProgressBar pBarProcesandoInfo, ref Telerik.WinControls.UI.RadLabel lblProcesandoInfo, ProgressBarStyle estilo)
        {
            pBarProcesandoInfo.Value = 0;
            pBarProcesandoInfo.MarqueeAnimationSpeed = 30;
            pBarProcesandoInfo.Style = estilo;
            pBarProcesandoInfo.Visible = true;
            lblProcesandoInfo.Text = this.LP.GetText("lblProcesandoInfo", "Procesando informe");
            lblProcesandoInfo.Visible = true;
            lblProcesandoInfo.Refresh();
        }

        /// <summary>
        /// Oculta la barra de progreso
        /// </summary>
        protected void FinalizaBarraProgreso(ref NewProgressBar pBarProcesandoInfo, ref Telerik.WinControls.UI.RadLabel lblProcesandoInfo)
        {
            pBarProcesandoInfo.Visible = false;
            lblProcesandoInfo.Visible = false;
        }

        /// <summary>
        /// Oculta la barra de progreso
        /// </summary>
        protected void FinalizaBarraProgreso(ref ProgressBar pBarProcesandoInfo, ref Telerik.WinControls.UI.RadLabel lblProcesandoInfo)
        {
            pBarProcesandoInfo.Visible = false;
            lblProcesandoInfo.Visible = false;
        }
        #endregion

        #region HTML
        protected void InformeHTMLCrear(ref StringBuilder documento_HTML)
        {
            try
            {
                documento_HTML = new StringBuilder();
                documento_HTML.Append("<html>\n");
                documento_HTML.Append(" <head>\n");
                documento_HTML.Append(" <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\"/>\n");
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        protected void InformeHTMLEscribirAjustesGlobales(ref StringBuilder documento_HTML)
        {
            documento_HTML.Append(" </body>\n");
            documento_HTML.Append("</html>\n");

            /*
            if (mostrarFichero)
            {
                //Visualizar el informe en el webbrowser
                frmWebBrowser frmWebB = new frmWebBrowser
                {
                    Documento = documento_HTML.ToString(),
                    FrmPadre = this
                };
                documento_HTML.Clear();
                frmWebB.ShowDialog();
            }*/
        }

        protected void InformeHTMLEscribirTagTable(ref StringBuilder documento_HTML, bool final)
        {
            if (final) documento_HTML.Append("     </table>\n");
            else documento_HTML.Append("     <table width =\"100%\">\n");
        }
        #endregion

        #region Excel
        protected void InformeExcelCrear(ref Microsoft.Office.Interop.Excel.Application excelApp, 
                                         ref Microsoft.Office.Interop.Excel.Workbook objLibroExcel,
                                         ref Microsoft.Office.Interop.Excel.Worksheet objHojaExcel,
                                         string nombreHoja)
        {
            try
            {
                //http://www.elguille.info/colabora/puntoNET/ELMoreno_ExcelReports.htm
                excelApp = new Microsoft.Office.Interop.Excel.Application
                {
                    //excelApp.Visible = true;
                    Visible = false
                };

                //Creamos una instancia del Workbooks de Excel
                //Creamos una instancia de la primera hoja de trabajo de Excel
                objLibroExcel = excelApp.Workbooks.Add();
                objHojaExcel = objLibroExcel.Worksheets[1];
                objHojaExcel.Name = nombreHoja;
                objHojaExcel.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}
