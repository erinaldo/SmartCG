using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace ObjectModel
{
    public class Entorno
    {
        private Utiles utiles;
        private LanguageProvider LP;

        private string entornoXMLPathFichero;

        private string ficheroEntorno;
        private string nombre;
        private string proveedorDatos;
        private string tipoBaseDatos;
        private string cadenaConexion;
        private string iPoNombreServidor;
        private string nombrebbdd;
        private string userDSN;
        private string userNameServidor;
        private string userNameApp;
        private string prefijoTabla;
        private string bbddCGAPP;
        private string bbddCGUF;
        private string userCGIFS;

        //SII agencia/entorno
        private string siiAgencia;
        private string siiEntorno;

        private DataSet dsEntorno;
        private DataTable dtEntorno;

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

        public string EntornoXMLPathFichero
        {
            get
            {
                return (this.entornoXMLPathFichero);
            }
            set
            {
                this.entornoXMLPathFichero = value;
            }
        }

        public string FicheroEntorno
        {
            get
            {
                return (this.ficheroEntorno);
            }
            set
            {
                this.ficheroEntorno = value;
            }
        }

        public string Nombre
        {
            get
            {
                return (this.nombre);
            }
            set
            {
                this.nombre = value;
            }
        }

        public string ProveedorDatos
        {
            get
            {
                return (this.proveedorDatos);
            }
            set
            {
                this.proveedorDatos = value;
            }
        }

        public string TipoBaseDatos
        {
            get
            {
                return (this.tipoBaseDatos);
            }
            set
            {
                this.tipoBaseDatos = value;
            }
        }

        public string CadenaConexion
        {
            get
            {
                return (this.cadenaConexion);
            }
            set
            {
                this.cadenaConexion = value;
            }
        }

        public string IPoNombreServidor
        {
            get
            {
                return (this.iPoNombreServidor);
            }
            set
            {
                this.iPoNombreServidor = value;
            }
        }

        public string NombreBBDD
        {
            get
            {
                return (this.nombrebbdd);
            }
            set
            {
                this.nombrebbdd = value;
            }
        }

        public string UserDSN
        {
            get
            {
                return (this.userDSN);
            }
            set
            {
                this.userDSN = value;
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
            }
        }

        public string PrefijoTabla
        {
            get
            {
                return (this.prefijoTabla);
            }
            set
            {
                this.prefijoTabla = value;
            }
        }

        public string BbddCGAPP
        {
            get
            {
                return (this.bbddCGAPP);
            }
            set
            {
                this.bbddCGAPP = value;
            }
        }

        public string BbddCGUF
        {
            get
            {
                return (this.bbddCGUF);
            }
            set
            {
                this.bbddCGUF = value;
            }
        }

        public string UserCGIFS
        {
            get
            {
                return (this.userCGIFS);
            }
            set
            {
                this.userCGIFS = value;
            }
        }

        public DataTable DTEntorno
        {
            get
            {
                return (this.dtEntorno);
            }
            set
            {
                this.dtEntorno = value;
            }
        }

        public string SiiAgencia
        {
            get
            {
                return (this.siiAgencia);
            }
            set
            {
                this.siiAgencia = value;
            }
        }

        public string SiiEntorno
        {
            get
            {
                return (this.siiEntorno);
            }
            set
            {
                this.siiEntorno = value;
            }
        }
        #endregion

        public Entorno()
        {
            try
            {
                this.utiles = new Utiles();

                //Inicializar la variable que contiene el path a los ficheros de entorno
                this.entornoXMLPathFichero = System.Windows.Forms.Application.StartupPath;
                string varPathFicherosEntornos = System.Configuration.ConfigurationManager.AppSettings["pathFicherosEntornos"];

                if (varPathFicherosEntornos != null) this.entornoXMLPathFichero = this.entornoXMLPathFichero + "\\" + varPathFicherosEntornos;

                //Construir el DataTable
                this.BuildDataTableInfo();

                //Blanquea todos los valores del entorno
                this.InicializarEntorno(true);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        #region Métodos Privados
        /// <summary>
        /// Inicializa las propiedades del entorno a vacías
        /// </summary>
        /// <param name="eliminarLista"></param>
        public void InicializarEntorno(bool eliminarLista)
        {
            this.nombre = "";
            this.proveedorDatos = "";
            this.tipoBaseDatos = "";
            this.cadenaConexion = "";
            this.iPoNombreServidor = "";
            this.nombrebbdd = "";
            this.userDSN = "";
            this.userNameServidor = "";
            this.userNameApp = "";
            this.prefijoTabla = "";
            this.bbddCGAPP = "";
            this.bbddCGUF = "";
            this.userCGIFS = "";
            this.siiAgencia = "A";
            this.siiEntorno = "T";

            if (eliminarLista && this.dtEntorno != null && this.dtEntorno.Rows.Count > 0) this.dtEntorno.Clear();
        }

        /// <summary>
        /// Crea el DataTable que contiene la información del o de los entornos
        /// </summary>
        private void BuildDataTableInfo()
        {
            try
            {
                this.dsEntorno = new DataSet
                {
                    DataSetName = "definicion"
                };

                this.dtEntorno = new DataTable
                {
                    TableName = "entorno"
                };

                //Adicionar las columnas al DataTable               
                DataColumn col = new DataColumn("archivo", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("nombre", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("proveedorDatos", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("tipoBaseDatos", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("IPoNombreServidor", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("nombrebbdd", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("cadenaConexion", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("lastDSNContab", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("lastUserContab", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("lastUserApp", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("prefijoTabla", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("bbddCGAPP", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("bbddCGUF", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("USER_CGIFS", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("activo", typeof(bool));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("siiAgencia", typeof(System.String));
                this.dtEntorno.Columns.Add(col);
                col = new DataColumn("siiEntorno", typeof(System.String));
                this.dtEntorno.Columns.Add(col);

                this.dsEntorno.Tables.Add(this.dtEntorno);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Lee un entorno
        /// </summary>
        /// <param name="ficheroEntorno">Nombre del fichero de entorno</param>
        /// <param name="eliminarLista">true -> elimina los entornos de la lista  false adiciona el entorno a la lista ya existente de entornos></param>
        /// <returns></returns>
        public string LeerEntorno(string ficheroEntorno, bool eliminarLista)
        {
            string result = "";

            try
            {
                this.InicializarEntorno(eliminarLista);

                this.ficheroEntorno = ficheroEntorno;
                string activo = "0";

                string pathFicheroEntorno = this.entornoXMLPathFichero + "\\" + this.ficheroEntorno;
                DataSet ds = new DataSet();
                ds.ReadXml(pathFicheroEntorno);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["entorno"].Rows.Count > 0)
                {
                    try { this.nombre = ds.Tables["entorno"].Rows[0]["nombre"].ToString().Trim(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.proveedorDatos = ds.Tables["entorno"].Rows[0]["proveedorDatos"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.tipoBaseDatos = ds.Tables["entorno"].Rows[0]["tipoBaseDatos"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.iPoNombreServidor = ds.Tables["entorno"].Rows[0]["IPoNombreServidor"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.nombrebbdd = ds.Tables["entorno"].Rows[0]["nombrebbdd"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.cadenaConexion = ds.Tables["entorno"].Rows[0]["cadenaConexion"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.userDSN = ds.Tables["entorno"].Rows[0]["lastDSNContab"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.userNameServidor = ds.Tables["entorno"].Rows[0]["lastUserContab"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.userNameApp = ds.Tables["entorno"].Rows[0]["lastUserApp"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.prefijoTabla = ds.Tables["entorno"].Rows[0]["prefijoTabla"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.bbddCGAPP = ds.Tables["entorno"].Rows[0]["bbddCGAPP"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.bbddCGUF = ds.Tables["entorno"].Rows[0]["bbddCGUF"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.userCGIFS = ds.Tables["entorno"].Rows[0]["USER_CGIFS"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.siiAgencia = ds.Tables["entorno"].Rows[0]["siiAgencia"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.siiEntorno = ds.Tables["entorno"].Rows[0]["siiEntorno"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    try
                    {
                        string entornoActual = System.Configuration.ConfigurationManager.AppSettings["entornoActual"];

                        if (ficheroEntorno == entornoActual) activo = "1";
                        else activo = "0";
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    //Insertar una fila en el DataTable
                    if (eliminarLista) this.dtEntorno.Rows.Clear();

                    DataRow dr = this.dtEntorno.NewRow();
                    dr["archivo"] = this.ficheroEntorno;
                    dr["nombre"] = this.nombre;
                    dr["proveedorDatos"] = this.proveedorDatos;
                    dr["tipoBaseDatos"] = this.tipoBaseDatos;
                    dr["IPoNombreServidor"] = this.iPoNombreServidor;
                    dr["nombrebbdd"] = this.nombrebbdd;
                    dr["cadenaConexion"] = this.cadenaConexion;
                    dr["lastDSNContab"] = this.userDSN;
                    dr["lastUserContab"] = this.userNameServidor;
                    dr["lastUserApp"] = this.userNameApp;
                    dr["prefijoTabla"] = this.prefijoTabla;
                    dr["bbddCGAPP"] = this.bbddCGAPP;
                    dr["bbddCGUF"] = this.bbddCGUF;
                    dr["USER_CGIFS"] = this.userCGIFS;
                    if (activo == "1") dr["activo"] = true;
                    else dr["activo"] = false;
                    dr["siiAgencia"] = this.siiAgencia;
                    dr["siiEntorno"] = this.siiEntorno;

                    this.dtEntorno.Rows.Add(dr);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Lee todos los entornos (ficheros xml) que están definidos en la carpeta de entornos
        /// </summary>
        /// <returns></returns>
        public string LeerTodosEntornos()
        {
            string result = "";

            try
            {
                this.dtEntorno.Clear();
                DirectoryInfo dir = new DirectoryInfo(this.entornoXMLPathFichero);
                FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo FI in fileList)
                {
                    try 
                    { 
                        this.LeerEntorno(FI.Name, false); 
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Pequeñas validaciones para comprobar que el fichero de entorno esté bien formado
        /// </summary>
        /// <returns></returns>
        public bool ValidarEntorno()
        {
            bool result = true;

            return (result);
        }

        /// <summary>
        /// Carga el entorno, a partir de ahora estará como activo
        /// </summary>
        /// <returns></returns>
        public string CargarEntorno()
        {
            string result = "";

            try
            {
                if (this.ValidarEntorno())
                {
                    //Actualizar las variables de configuración del appConfig
                    utiles.ModificarappSettings("proveedorDatosCG", this.proveedorDatos);
                    utiles.ModificarappSettings("tipoBaseDatosCG", this.tipoBaseDatos);
                    utiles.ModificarappSettings("cadenaConexionCG", this.cadenaConexion);
                    utiles.ModificarappSettings("prefijoTablaCG", this.prefijoTabla);

                    //utiles.ModificarappSettings("lastDSNContab", this.userDSN);
                    //utiles.ModificarappSettings("lastUserContab", this.userNameServidor);
                    //utiles.ModificarappSettings("lastUserApp", this.userNameApp);

                    utiles.ModificarappSettings("bbddCGAPP", this.bbddCGAPP);
                    utiles.ModificarappSettings("bbddCGUF", this.bbddCGUF);
                    utiles.ModificarappSettings("USER_CGIFS", this.userCGIFS);

                    utiles.ModificarappSettings("entornoActual", this.ficheroEntorno);

                    GlobalVar.EntornoActivo = this;

                    try
                    {
                        //Actualizar valores del usuario
                        if (GlobalVar.UsuarioEnv.Entorno != this.ficheroEntorno)
                        {
                            GlobalVar.UsuarioEnv.Entorno = this.ficheroEntorno;
                            GlobalVar.UsuarioEnv.UserNameServidor = this.userNameServidor;
                            GlobalVar.UsuarioEnv.UserNameApp = this.userNameApp;
                            GlobalVar.UsuarioEnv.GrabarUsuario();
                        }
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    //Blanquear las variables de memoria
                    try
                    {
                        if (GlobalVar.ConexionCG != null) GlobalVar.ConexionCG.GetConnectionValue.Close();
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    GlobalVar.CadenaConexionCG = "";

                    GlobalVar.UsuarioLogadoCG_BBDD = "";
                    GlobalVar.UsuarioLogadoCG = "";

                    GlobalVar.ConexionCG = null;
                    GlobalVar.PrefijoTablaCG = this.prefijoTabla;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Graba el entorno
        /// </summary>
        /// <returns></returns>
        public string GrabarEntorno()
        {
            string result = "";

            try
            {
                string pathFicheroEntorno = this.entornoXMLPathFichero + "\\" + this.ficheroEntorno;

                DataTable aux = this.dtEntorno.Copy();
                aux.Columns.Remove("archivo");
                aux.Columns.Remove("activo");
                aux.WriteXml(pathFicheroEntorno);
                aux.Dispose();
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Instancia un entorno recuperando uno de los de la lista (se utiliza en el formulario de cargar entornos y en el de listar entorno)
        /// </summary>
        /// <param name="entorno">entorno</param>
        /// <param name="indice">índice dentro de la lista de entornos</param>
        /// <returns></returns>
        public string InstanciarEntorno(Entorno entorno, int indice)
        {
            string result = "";

            try
            {
                if (entorno.dtEntorno != null && indice <= entorno.dtEntorno.Rows.Count)
                {
                    this.nombre = entorno.dtEntorno.Rows[indice]["nombre"].ToString();
                    this.proveedorDatos = entorno.dtEntorno.Rows[indice]["proveedorDatos"].ToString();
                    this.tipoBaseDatos = entorno.dtEntorno.Rows[indice]["tipoBaseDatos"].ToString();
                    this.cadenaConexion = entorno.dtEntorno.Rows[indice]["cadenaConexion"].ToString();
                    this.iPoNombreServidor = entorno.dtEntorno.Rows[indice]["IPoNombreServidor"].ToString();
                    this.nombrebbdd = entorno.dtEntorno.Rows[indice]["nombrebbdd"].ToString();
                    this.prefijoTabla = entorno.dtEntorno.Rows[indice]["prefijoTabla"].ToString();
                    this.userDSN = entorno.dtEntorno.Rows[indice]["lastDSNContab"].ToString();
                    this.userNameServidor = entorno.dtEntorno.Rows[indice]["lastUserContab"].ToString();
                    this.userNameApp = entorno.dtEntorno.Rows[indice]["lastUserApp"].ToString();
                    this.bbddCGAPP = entorno.dtEntorno.Rows[indice]["bbddCGAPP"].ToString();
                    this.bbddCGUF = entorno.dtEntorno.Rows[indice]["bbddCGUF"].ToString();
                    this.userCGIFS = entorno.dtEntorno.Rows[indice]["USER_CGIFS"].ToString();
                    this.ficheroEntorno = entorno.dtEntorno.Rows[indice]["archivo"].ToString();
                    this.siiAgencia = entorno.dtEntorno.Rows[indice]["siiAgencia"].ToString();
                    this.siiEntorno = entorno.dtEntorno.Rows[indice]["siiEntorno"].ToString();
                }
                else
                {
                    result = "No se pudo instanciar el entorno";    //Falta traducir
                }
            }
            catch(Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                result = "No se pudo instanciar el entorno (" + ex.Message + ")";       //Falta traducir
            }
        
            return (result);
        }

        /// <summary>
        /// Adiciona un entorno a la lista de entornos
        /// </summary>
        /// <param name="entornoAdd"></param>
        /// <returns></returns>
        public string AdicionarEntornoLista(Entorno entornoAdd)
        {
            string result = "";

            try
            {
                DataRow dr = this.dtEntorno.NewRow();
                dr["archivo"] = entornoAdd.ficheroEntorno;
                dr["nombre"] = entornoAdd.nombre;
                dr["proveedorDatos"] = entornoAdd.proveedorDatos;
                dr["tipoBaseDatos"] = entornoAdd.tipoBaseDatos;
                dr["IPoNombreServidor"] = entornoAdd.iPoNombreServidor;
                dr["nombrebbdd"] = entornoAdd.nombrebbdd;
                dr["cadenaConexion"] = entornoAdd.cadenaConexion;
                dr["lastDSNContab"] = entornoAdd.userDSN;
                dr["lastUserContab"] = entornoAdd.userNameServidor;
                dr["lastUserApp"] = entornoAdd.userNameApp;
                dr["prefijoTabla"] = entornoAdd.prefijoTabla;
                dr["bbddCGAPP"] = entornoAdd.bbddCGAPP;
                dr["bbddCGUF"] = entornoAdd.bbddCGUF;
                dr["USER_CGIFS"] = entornoAdd.userCGIFS;
                dr["activo"] = false;
                dr["siiAgencia"] = entornoAdd.siiAgencia;
                dr["siiEntorno"] = entornoAdd.siiEntorno;

                this.dtEntorno.Rows.Add(dr);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        #endregion
    }
}
