using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;

namespace ObjectModel
{
    public class Usuario
    {
        private Utiles utiles;
        private LanguageProvider LP;

        //Nombre de los ficheros de Usuarios (comienzan por Usuario_)
        private const string nombreFichero = "Usuario_";
        private string usuarioXMLPathFichero;

        private string userNameEnv;
        private string userFichero;
        private string entorno;
        private string idiomaApp;
        private string userNameServidor;
        private string userNameApp;
        private string lastConexion;

        //Módulo de Comprobantes
        private string modComp_PathFicherosCompContables;
        private string modComp_PathFicherosCompExtraContables;
        private string modComp_PathFicherosModelosCompContables;
        private string modComp_PathFicherosModelosCompExtraContables;

        //Módulo de Consulta e Informes
        private string modConsInfo_PathFicherosConsultas;
        private string modConsInfo_PathFicherosInformes;
        private string modConsInfo_TipoFicherosConsultas;
        private string modConsInfo_TipoFicherosInformes;

        //Peticiones de los formularios que se soliciten, path donde se almacenarán
        private string pathFicherosPeticiones;
        
        //Exportar Datos Fichero por Defecto
        private ExportFileType exportarTipoFicheroDefecto;
        private bool exportarVisualizarFicheroDefecto;

        //Cargar lista de entornos al incio antes de entrar a la aplicación
        private bool cargarListaEntornosInicio;

        private DataSet dsUsuario;
        private DataTable dtUsuario;

        private ArrayList listaAutorizaciones;
        private ArrayList listaNoAutorizado;

        #region Properties
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

        public string UserNameEnv
        {
            get
            {
                return (this.userNameEnv);
            }
            set
            {
                this.userNameEnv = value;
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

                try
                {
                    if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0) this.dtUsuario.Rows[0]["entorno"] = this.entorno;
                }
                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
            }
        }

        public string IdiomaApp
        {
            get
            {
                return (this.idiomaApp);
            }
            set
            {
                this.idiomaApp = value;

                try
                {
                    if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0) this.dtUsuario.Rows[0]["idioma"] = this.idiomaApp;
                }
                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
            }
        }

        public string UserNameServidor
        {
            get
            {
                return (this.userNameServidor);
            }
            set
            {
                this.userNameServidor = value;

                try
                {
                    if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0) this.dtUsuario.Rows[0]["lastUserServidor"] = this.userNameServidor;
                }
                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
            }
        }

        public string UserNameApp
        {
            get
            {
                return (this.userNameApp);
            }
            set
            {
                this.userNameApp = value;

                try
                {
                    if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0) this.dtUsuario.Rows[0]["lastUserApp"] = this.userNameApp;
                }
                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
            }
        }

        public string LastConexion
        {
            get
            {
                return (this.lastConexion);
            }
            set
            {
                this.lastConexion = value;
            }
        }

        public string ModComp_PathFicherosCompContables
        {
            get
            {
                return (this.modComp_PathFicherosCompContables);
            }
            set
            {
                this.modComp_PathFicherosCompContables = value;
            }
        }

        public string ModComp_PathFicherosCompExtraContables
        {
            get
            {
                return (this.modComp_PathFicherosCompExtraContables);
            }
            set
            {
                this.modComp_PathFicherosCompExtraContables = value;
            }
        }

        public string ModComp_PathFicherosModelosCompContables
        {
            get
            {
                return (this.modComp_PathFicherosModelosCompContables);
            }
            set
            {
                this.modComp_PathFicherosModelosCompContables = value;
            }
        }

        public string ModComp_PathFicherosModelosCompExtraContables
        {
            get
            {
                return (this.modComp_PathFicherosModelosCompExtraContables);
            }
            set
            {
                this.modComp_PathFicherosModelosCompExtraContables = value;
            }
        }

        public string ModConsInfo_PathFicherosConsultas
        {
            get
            {
                return (this.modConsInfo_PathFicherosConsultas);
            }
            set
            {
                this.modConsInfo_PathFicherosConsultas = value;
            }
        }

        public string ModConsInfo_PathFicherosInformes
        {
            get
            {
                return (this.modConsInfo_PathFicherosInformes);
            }
            set
            {
                this.modConsInfo_PathFicherosInformes = value;
            }
        }

        public string ModConsInfo_TipoFicherosConsultas
        {
            get
            {
                return (this.modConsInfo_TipoFicherosConsultas);
            }
            set
            {
                this.modConsInfo_TipoFicherosConsultas = value;
            }
        }

        public string ModConsInfo_TipoFicherosInformes
        {
            get
            {
                return (this.modConsInfo_TipoFicherosInformes);
            }
            set
            {
                this.modConsInfo_TipoFicherosInformes = value;
            }
        }

        public string PathFicherosPeticiones
        {
            get
            {
                return (this.pathFicherosPeticiones);
            }
            set
            {
                this.pathFicherosPeticiones = value;
            }
        }

        public ExportFileType ExportarTipoFicheroDefecto
        {
            get
            {
                return (this.exportarTipoFicheroDefecto);
            }
            set
            {
                this.exportarTipoFicheroDefecto = value;
            }
        }

        public bool ExportarVisualizarFicheroDefecto
        {
            get
            {
                return (this.exportarVisualizarFicheroDefecto);
            }
            set
            {
                this.exportarVisualizarFicheroDefecto = value;
            }
        }

        public bool CargarListaEntornosInicio
        {
            get
            {
                return (this.cargarListaEntornosInicio);
            }
            set
            {
                this.cargarListaEntornosInicio = value;
            }
        }

        public DataTable DTUsuario
        {
            get
            {
                return (this.dtUsuario);
            }
            set
            {
                this.dtUsuario = value;
            }
        }

        /// <summary>
        /// Acceso a la variable que contiene la lista de permisos autorizados de los consultados para el usuario
        /// </summary>
        public ArrayList ListaAutorizaciones
        {
            get
            {
                return listaAutorizaciones;
            }
            set
            {
                listaAutorizaciones = value;
            }
        }

        /// <summary>
        /// Acceso a la variable que contiene la lista de permisos no autorizados de los consultados para el usuario
        /// </summary>
        public ArrayList ListaNoAutorizado
        {
            get
            {
                return listaNoAutorizado;
            }
            set
            {
                listaNoAutorizado = value;
            }
        }
        #endregion

        public Usuario()
        {
            try
            {
                this.utiles = new Utiles();

                this.listaAutorizaciones = new ArrayList();
                this.listaNoAutorizado = new ArrayList();

                //Inicializar la variable que contiene el path a los ficheros de usuario
                this.usuarioXMLPathFichero = System.Windows.Forms.Application.StartupPath;
                string varPathFicheroUsuarios = System.Configuration.ConfigurationManager.AppSettings["pathFicherosUsuario"];

                if (varPathFicheroUsuarios != null) this.usuarioXMLPathFichero = this.usuarioXMLPathFichero + "\\" + varPathFicheroUsuarios;

                this.userNameEnv = System.Environment.UserName.ToUpper();
                this.userFichero = this.usuarioXMLPathFichero + "\\" + nombreFichero + this.userNameEnv + ".xml";

                //Construir el DataTable
                this.BuildDataTableInfo();
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Crea el DataTable que contiene la información del usuario
        /// </summary>
        private void BuildDataTableInfo()
        {
            try
            {
                this.dsUsuario = new DataSet
                {
                    DataSetName = "definicion"
                };

                this.dtUsuario = new DataTable
                {
                    TableName = "usuario"
                };

                //Adicionar las columnas al DataTable               
                DataColumn col = new DataColumn("entorno", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("idioma", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("lastUserServidor", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("lastUserApp", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("lastConexion", typeof(System.String));
                this.dtUsuario.Columns.Add(col);

                col = new DataColumn("ModComp_PathFicherosCompContables", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModComp_PathFicherosCompExtraContables", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModComp_PathFicherosModelosCompContables", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModComp_PathFicherosModelosCompExtraContables", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModConsInfo_PathFicherosConsultas", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("modConsInfo_PathFicherosInformes", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModConsInfo_TipoFicherosConsultas", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("ModConsInfo_TipoFicherosInformes", typeof(System.String));
                this.dtUsuario.Columns.Add(col);

                col = new DataColumn("PathFicherosPeticiones", typeof(System.String));
                this.dtUsuario.Columns.Add(col);

                col = new DataColumn("exportarTipoFicheroDefecto", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
                col = new DataColumn("exportarVisualizarFicheroDefecto", typeof(System.String));
                this.dtUsuario.Columns.Add(col);

                col = new DataColumn("solicitarEntornoInicio", typeof(System.String));
                this.dtUsuario.Columns.Add(col);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Lee la información del usuario
        /// </summary>
        /// <returns></returns>
        public bool LeerInfo()
        {
            bool result = true;
            
            string resultLeerEntorno;
            string resultCargarEntorno;

            try
            {
                Entorno entornoUsuario = new Entorno();

                //Chequear si existe el fichero
                if (!File.Exists(this.userFichero))
                {
                    //Crear el fichero de usuario
                    try
                    {
                        //Leer último entorno cargado
                        string entornoActual = ConfigurationManager.AppSettings["entornoActual"];
                        string idiomaActual = ConfigurationManager.AppSettings["idioma"];

                        try
                        {
                            resultLeerEntorno = entornoUsuario.LeerEntorno(entornoActual, true);
                            resultCargarEntorno = entornoUsuario.CargarEntorno();
                        }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                        //Insertar una fila en el DataTable
                        DataRow dr = this.dtUsuario.NewRow();
                        dr["entorno"] = entornoActual;
                        dr["idioma"] = idiomaActual;
                        dr["lastUserServidor"] = entornoUsuario.UserNameServidor;
                        dr["lastUserApp"] = entornoUsuario.UserNameApp;
                        dr["lastConexion"] = "";

                        dr["ModComp_PathFicherosCompContables"] = "";
                        dr["ModComp_PathFicherosCompExtraContables"] = "";
                        dr["ModComp_PathFicherosModelosCompContables"] = "";
                        dr["ModComp_PathFicherosModelosCompExtraContables"] = "";
                        dr["ModConsInfo_PathFicherosConsultas"] = "";
                        dr["ModConsInfo_PathFicherosInformes"] = "";
                        dr["ModConsInfo_TipoFicherosConsultas"] = "";
                        dr["ModConsInfo_TipoFicherosInformes"] = "";

                        dr["PathFicherosPeticiones"] = "";

                        dr["exportarTipoFicheroDefecto"] = "EXCEL";
                        dr["exportarVisualizarFicheroDefecto"] = "1";

                        dr["solicitarEntornoInicio"] = "0";

                        this.dtUsuario.Rows.Add(dr);

                        this.GrabarUsuario();
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }
                else
                {
                    //Leer el fichero de usuario
                    DataSet ds = new DataSet();
                    ds.ReadXml(this.userFichero);

                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["usuario"].Rows.Count > 0)
                    {
                        string entornoUser = "";
                        string idioma = "";
                        string lastUserServidor = "";
                        string lastUserApp = "";
                        string lastConexion = "";
                        
                        string modComp_PathFicherosCompContables = "";
                        string modComp_PathFicherosCompExtraContables = "";
                        string modComp_PathFicherosModelosCompContables = "";
                        string modComp_PathFicherosModelosCompExtraContables = "";
                        string modConsInfo_PathFicherosConsultas = "";
                        string modConsInfo_PathFicherosInformes = "";
                        string modConsInfo_TipoFicherosConsultas = "";
                        string modConsInfo_TipoFicherosInformes = "";

                        string pathFicherosPeticiones = "";

                        string exportarTipoFicheroDefectoStr = "";
                        string exportarVisualizarFicheroDefectoStr = "";

                        string cargarListaEntornosInicioStr = "";

                        try { entornoUser = ds.Tables["usuario"].Rows[0]["entorno"].ToString().Trim(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { idioma = ds.Tables["usuario"].Rows[0]["idioma"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { lastUserServidor = ds.Tables["usuario"].Rows[0]["lastUserServidor"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { lastUserApp = ds.Tables["usuario"].Rows[0]["lastUserApp"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { lastConexion = ds.Tables["usuario"].Rows[0]["lastConexion"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                        try { modComp_PathFicherosCompContables = ds.Tables["usuario"].Rows[0]["ModComp_PathFicherosCompContables"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modComp_PathFicherosCompExtraContables = ds.Tables["usuario"].Rows[0]["ModComp_PathFicherosCompExtraContables"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modComp_PathFicherosModelosCompContables = ds.Tables["usuario"].Rows[0]["ModComp_PathFicherosModelosCompContables"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modComp_PathFicherosModelosCompExtraContables = ds.Tables["usuario"].Rows[0]["ModComp_PathFicherosModelosCompExtraContables"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modConsInfo_PathFicherosConsultas = ds.Tables["usuario"].Rows[0]["ModConsInfo_PathFicherosConsultas"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modConsInfo_PathFicherosInformes = ds.Tables["usuario"].Rows[0]["ModConsInfo_PathFicherosInformes"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modConsInfo_TipoFicherosConsultas = ds.Tables["usuario"].Rows[0]["ModConsInfo_TipoFicherosConsultas"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { modConsInfo_TipoFicherosInformes = ds.Tables["usuario"].Rows[0]["ModConsInfo_TipoFicherosInformes"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        try { pathFicherosPeticiones = ds.Tables["usuario"].Rows[0]["PathFicherosPeticiones"].ToString(); }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                        try {
                            exportarTipoFicheroDefectoStr = ds.Tables["usuario"].Rows[0]["exportarTipoFicheroDefecto"].ToString();
                            //Verificar que sean de los valores posibles (EXCEL/HTML/PDF)
                            this.exportarTipoFicheroDefecto = (ExportFileType)System.Enum.Parse(typeof(ExportFileType), exportarTipoFicheroDefectoStr);
                        }
                        catch (Exception ex) {
                            exportarTipoFicheroDefectoStr = ExportFileType.EXCEL.ToString();
                            this.exportarTipoFicheroDefecto = ExportFileType.EXCEL;
                            GlobalVar.Log.Error(ex.Message);
                        }

                        try {
                            exportarVisualizarFicheroDefectoStr = ds.Tables["usuario"].Rows[0]["exportarVisualizarFicheroDefecto"].ToString();
                            if (exportarVisualizarFicheroDefectoStr == "0") this.exportarVisualizarFicheroDefecto = false;
                            else this.exportarVisualizarFicheroDefecto = true;
                        }
                        catch (Exception ex) {
                            exportarVisualizarFicheroDefectoStr = "1";
                            this.exportarVisualizarFicheroDefecto = true;
                            GlobalVar.Log.Error(ex.Message);
                        }

                        try
                        {
                            cargarListaEntornosInicioStr = ds.Tables["usuario"].Rows[0]["solicitarEntornoInicio"].ToString();
                            if (cargarListaEntornosInicioStr == "0") this.cargarListaEntornosInicio = false;
                            else this.cargarListaEntornosInicio = true;
                        }
                        catch (Exception ex)
                        {
                            cargarListaEntornosInicioStr = "0";
                            this.cargarListaEntornosInicio = false;
                            GlobalVar.Log.Error(ex.Message);
                        }

                        //Insertar una fila en el DataTable
                        DataRow dr = this.dtUsuario.NewRow();
                        dr["entorno"] = entornoUser;
                        dr["idioma"] = idioma;
                        dr["lastUserServidor"] = lastUserServidor;
                        dr["lastUserApp"] = lastUserApp;
                        dr["lastConexion"] = lastConexion;

                        dr["ModComp_PathFicherosCompContables"] = modComp_PathFicherosCompContables;
                        dr["ModComp_PathFicherosCompExtraContables"] = modComp_PathFicherosCompExtraContables;
                        dr["ModComp_PathFicherosModelosCompContables"] = modComp_PathFicherosModelosCompContables;
                        dr["ModComp_PathFicherosModelosCompExtraContables"] = modComp_PathFicherosModelosCompExtraContables;
                        dr["ModConsInfo_PathFicherosConsultas"] = modConsInfo_PathFicherosConsultas;
                        dr["ModConsInfo_PathFicherosInformes"] = modConsInfo_PathFicherosInformes;
                        dr["ModConsInfo_TipoFicherosConsultas"] = modConsInfo_TipoFicherosConsultas;
                        dr["ModConsInfo_TipoFicherosInformes"] = modConsInfo_TipoFicherosInformes;
                        dr["PathFicherosPeticiones"] = pathFicherosPeticiones;
                        dr["exportarTipoFicheroDefecto"] = exportarTipoFicheroDefectoStr;
                        dr["exportarVisualizarFicheroDefecto"] = exportarVisualizarFicheroDefectoStr;
                        dr["solicitarEntornoInicio"] = cargarListaEntornosInicioStr;
                        
                        this.dtUsuario.Rows.Add(dr);

                        this.entorno = entornoUser;
                        this.idiomaApp = idioma;
                        this.userNameServidor = lastUserServidor;
                        this.userNameApp = lastUserApp;
                        this.lastConexion = lastConexion;

                        this.modComp_PathFicherosCompContables = modComp_PathFicherosCompContables;
                        this.modComp_PathFicherosCompExtraContables = modComp_PathFicherosCompExtraContables;
                        this.modComp_PathFicherosModelosCompContables = modComp_PathFicherosModelosCompContables;
                        this.modComp_PathFicherosModelosCompExtraContables = modComp_PathFicherosModelosCompExtraContables;
                        this.modConsInfo_PathFicherosConsultas = modConsInfo_PathFicherosConsultas;
                        this.modConsInfo_PathFicherosInformes = modConsInfo_PathFicherosInformes;
                        this.modConsInfo_TipoFicherosConsultas = modConsInfo_TipoFicherosConsultas;
                        this.modConsInfo_TipoFicherosInformes = modConsInfo_TipoFicherosInformes;

                        this.pathFicherosPeticiones = pathFicherosPeticiones;

                        //Actualizar las variables de la aplicación con los valores del usuario que se va a logar
                        utiles.ModificarappSettings("entornoActual", this.entorno);
                        utiles.ModificarappSettings("idioma", this.idiomaApp);

                        //utiles.ModificarappSettings("lastUserContab", this.userNameServidor);
                        //utiles.ModificarappSettings("lastUserApp", this.userNameApp);

                        utiles.ModificarappSettings("ModComp_PathFicherosCompContables", this.modComp_PathFicherosCompContables);
                        utiles.ModificarappSettings("ModComp_PathFicherosCompExtraContables", this.modComp_PathFicherosCompExtraContables);
                        utiles.ModificarappSettings("ModComp_PathFicherosModelosCompContables", this.modComp_PathFicherosModelosCompContables);
                        utiles.ModificarappSettings("ModComp_PathFicherosModelosCompExtraContables", this.modComp_PathFicherosModelosCompExtraContables);
                        utiles.ModificarappSettings("ModConsInfo_PathFicherosConsultas", this.modConsInfo_PathFicherosConsultas);
                        utiles.ModificarappSettings("ModConsInfo_PathFicherosInformes", this.modConsInfo_PathFicherosInformes);
                        utiles.ModificarappSettings("ModConsInfo_TipoFicherosConsultas", this.modConsInfo_TipoFicherosConsultas);
                        utiles.ModificarappSettings("ModConsInfo_TipoFicherosInformes", this.modConsInfo_TipoFicherosInformes);

                        utiles.ModificarappSettings("PathFicherosPeticiones", this.pathFicherosPeticiones);

                        if (!this.cargarListaEntornosInicio)
                        {
                            //Leer el entorno
                            try
                            {
                                resultLeerEntorno = entornoUsuario.LeerEntorno(entornoUser, true);
                                resultCargarEntorno = entornoUsuario.CargarEntorno();
                            }
                            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Graba el fichero de usuario
        /// </summary>
        /// <returns></returns>
        public string GrabarUsuario()
        {
            string result = "";

            try
            {
                //Actualizar lastConexion
                this.lastConexion = System.DateTime.Now.ToString();
                if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0)
                {
                    this.dtUsuario.Rows[0]["lastConexion"] = this.lastConexion;
                    this.dtUsuario.WriteXml(this.userFichero);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Graba el fichero de usuario el parametro enviado
        /// </summary>
        /// <returns></returns>
        public string GrabarUsuario(string parametro, string valor)
        {
            string result = "";

            try
            {
                //Actualizar parametro
                if (this.dtUsuario != null && this.dtUsuario.Rows.Count > 0)
                {
                    this.dtUsuario.Rows[0][parametro] = valor;
                    this.dtUsuario.WriteXml(this.userFichero);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Devuelve si el usuario está o no autorizado
        /// </summary>
        /// <param name="clase"></param>
        /// <param name="grupo"></param>
        /// <param name="operacion"></param>
        /// <param name="elemento"></param>
        /// <returns></returns>
        public bool UsuarioAutorizado(Autorizaciones aut, string clase, string grupo, string operacion, string elemento)
        {
            try
            {
                string permiso = clase + grupo + operacion + elemento;

                if (this.listaAutorizaciones.Count > 0)
                {
                    if (this.listaAutorizaciones.Contains(permiso)) return (true);
                }
 
                //Verificar si no tiene autorización (previamente pudo ser validado)
                if (this.listaNoAutorizado.Count > 0)
                {
                    if (this.listaNoAutorizado.Contains(permiso)) return (false);
                }

                //Verificar si tiene autorización
                if (aut.Validar(clase, grupo, elemento, operacion))
                {
                    listaAutorizaciones.Add(permiso);
                    return (true);
                }
                else
                {
                    listaNoAutorizado.Add(permiso);
                    return (false);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (false);
        }

    }
}