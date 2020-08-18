using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;


namespace ObjectModel
{
    public class ProveedorDatos
    {
        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum DBProveedores
        {
            OleDb,
            Odbc,
            SqlClient
        }

        /// <summary>
        /// Enumera los tipos de bases de datos soportados
        /// </summary>
        public enum DBTipos
        {
            DB2,
            SQLServer,
            Oracle
        }

        #region Attributes
        protected IDbConnection _dbconn;
        //protected IDbTransaction _trans;
        protected IDbCommand _selectcommand;
        //protected IDataAdapter _da;
        protected DBProveedores _proveedor;
        protected DBTipos _tipoBaseDatos;
        protected string _connectionString;
        //protected string query;
        #endregion

        #region Properties
        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        public DBTipos TipoBaseDatos
        {
            get
            {
                return this._tipoBaseDatos;
            }
            set
            {
                this._tipoBaseDatos = value;
            }
        }

        public IDbConnection GetConnectionValue
        {
            get
            {
                return this._dbconn;
            }
        }

        public IDbCommand GetCommandProperty
        {
            get
            {
                return this._selectcommand;
            }
        }
        #endregion

        #region Constructors
        public ProveedorDatos(DBProveedores proveedor)
		{
			this._proveedor = proveedor;
		}

        public ProveedorDatos(DBProveedores proveedor, string connectionString)
		{
			this._proveedor = proveedor;
			this._connectionString = connectionString;
		}
		#endregion

        #region Métodos Públicos de la clase
        /// <summary>
        /// Abre una conexión de forma explícita contra el servidor de base de datos
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                this._dbconn = this.GetConnection();
                this._dbconn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cerrar la conexión de la base de datos de forma explícita
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (this._dbconn != null) this._dbconn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta comandos SQL que devuelven filas (abre y cierra la conexión).
        /// </summary>
        /// <param name="cmdText">Texto de comando</param>
        /// <returns>DataReader que contiene los datos</returns>
        public IDataReader ExecuteReader(string cmdText)
        {
            try
            {
                GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
                GlobalVar.Log.Debug(cmdText);

                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                this._dbconn = this.GetConnection();
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                this._dbconn.Open();
                // Uso command behavior para cerrar automáticamente la conexión
                // cuando se cierra el reader.  Es necesario dejar la conexión abierta
                // hasta que todos los datos han sido extraídos del origen.
                return this._selectcommand.ExecuteReader(
                    System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Ejecuta comandos SQL que devuelven filas (se le pasa una conexión abierta).
        /// </summary>
        /// <param name="cmdText">Texto de comando</param>
        /// <param name="conn">Conexión a bbdd (abierta)</param>
        /// <returns>DataReader que contiene los datos</returns>
        public IDataReader ExecuteReader(string cmdText, IDbConnection conn)
        {
            try
            {
                GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
                GlobalVar.Log.Debug(cmdText);

                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                if (conn == null)
                {
                    conn = this.GetConnection();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                this._dbconn = conn;
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                return this._selectcommand.ExecuteReader();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable FillDataTable(string cmdText, IDbConnection conn)
        {
            GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
            GlobalVar.Log.Debug(cmdText);

            DataTable data = null;

            IDataAdapter adapter = null;  // Adaptador de datos

            try
            {
                if (conn == null)
                {
                    conn = this.GetConnection();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                this._dbconn = conn;    // Conexion a BB.DD. 

                this._selectcommand = this.GetCommand(cmdText, this._dbconn);   // Comando de base de datos
                adapter = this.GetDataAdapter(this._selectcommand);

                data = new DataTable();

                switch (this._proveedor)
                {
                    case DBProveedores.Odbc:
                        OdbcDataAdapter odbcda = adapter as OdbcDataAdapter;
                        odbcda.Fill(data);
                        break;
                    case DBProveedores.OleDb:
                        OleDbDataAdapter oleda = adapter as OleDbDataAdapter;
                        oleda.Fill(data);
                        break;
                    case DBProveedores.SqlClient:
                        SqlDataAdapter sqlda = adapter as SqlDataAdapter;
                        sqlda.Fill(data);
                        break;
                }
            }
            catch(Exception ex)
            {
                //if (this._dbconn != null) this._dbconn.Close();
                throw new Exception(ex.Message);
            }
            return (data);
        }



        public void FillDataTable(string cmdText, IDbConnection conn, string nombreTabla, ref DataSet ds)
        {
            GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
            GlobalVar.Log.Debug(cmdText);

            IDataAdapter adapter = null;  // Adaptador de datos

            try
            {
                if (conn == null)
                {
                    conn = this.GetConnection();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                this._dbconn = conn;    // Conexion a BB.DD. 

                this._selectcommand = this.GetCommand(cmdText, this._dbconn);   // Comando de base de datos
                adapter = this.GetDataAdapter(this._selectcommand);

                switch (this._proveedor)
                {
                    case DBProveedores.Odbc:
                        OdbcDataAdapter odbcda = adapter as OdbcDataAdapter;
                        odbcda.Fill(ds, nombreTabla);
                        break;
                    case DBProveedores.OleDb:
                        OleDbDataAdapter oleda = adapter as OleDbDataAdapter;
                        oleda.Fill(ds, nombreTabla);
                        break;
                    case DBProveedores.SqlClient:
                        SqlDataAdapter sqlda = adapter as SqlDataAdapter;
                        sqlda.Fill(ds, nombreTabla);
                        break;
                }
            }
            catch (Exception ex)
            {
                //if (this._dbconn != null) this._dbconn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta una sentencia SQL que no retorna valor: INSERT,DELETE,UPDATE
        /// </summary>
        public int ExecuteNonQuery(string cmdText)
        {
            GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
            GlobalVar.Log.Debug(cmdText);

            try
            {
                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                this._dbconn = this.GetConnection();
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                this._dbconn.Open();
                return this._selectcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta una sentencia SQL que no retorna valor: INSERT,DELETE,UPDATE
        /// </summary>
        public int ExecuteNonQuery(string cmdText, IDbConnection conn)
        {
            GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
            GlobalVar.Log.Debug(cmdText);

            try
            {
                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                if (conn == null)
                {
                    conn = this.GetConnection();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                this._dbconn = conn;
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                return this._selectcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un único valor (por ejemplo, un valor agregado) 
        /// de una base de datos.  Obtiene el primer valor de la primera fila
        /// del conjunto de resultados.
        /// </summary>
        /// <param name="cmdText">Texto de comando</param>
        /// <returns>El valor obtenido en tipo object</returns>
        public Object ExecuteScalar(string cmdText)
        {
            GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
            GlobalVar.Log.Debug(cmdText);

            try
            {
                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                this._dbconn = this.GetConnection();
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                this._dbconn.Open();
                return this._selectcommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un único valor (por ejemplo, un valor agregado) 
        /// de una base de datos.  Obtiene el primer valor de la primera fila
        /// del conjunto de resultados.
        /// </summary>
        /// <param name="cmdText">Texto de comando</param>
        /// <param name="conn">Conexión a la bbdd</param>
        /// <returns>El valor obtenido en tipo object</returns>
        public Object ExecuteScalar(string cmdText, IDbConnection conn)
        {
            try
            {
                GlobalVar.Log = log4net.LogManager.GetLogger(this.GetType());
                GlobalVar.Log.Debug(cmdText);

                this._selectcommand = null;    // Comando de base de datos
                this._dbconn = null;  // Conexion a BB.DD.

                if (conn == null)
                {
                    conn = this.GetConnection();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                this._dbconn = conn;
                this._selectcommand = this.GetCommand(cmdText, this._dbconn);
                return this._selectcommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Métodos Privados de la clase
        /// <summary>
        /// Obtiene una conexión a la base de datos
        /// </summary>
        /// <returns></returns>
        private IDbConnection GetConnection()
        {
            IDbConnection conn = null;  // Conexión a base de datos

            try
            {
                switch (this._proveedor)
                {
                    case DBProveedores.Odbc:
                        conn = new OdbcConnection(this._connectionString);
                        break;
                    case DBProveedores.OleDb:
                        conn = new OleDbConnection(this._connectionString);
                        break;
                    case DBProveedores.SqlClient:
                        conn = new SqlConnection(this._connectionString);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return conn;
        }

        /// <summary>
        /// Obtiene un comando de base de datos específico del proveedor de datos
        /// </summary>
        /// <param name="CmdText"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        private IDbCommand GetCommand()
        {
            IDbCommand command = null;
            switch (this._proveedor)
            {
                case DBProveedores.Odbc:
                    command = new OdbcCommand();
                    break;
                case DBProveedores.OleDb:
                    command = new OleDbCommand();
                    break;
                case DBProveedores.SqlClient:
                    command = new SqlCommand();
                    break;
            }
            return command;
        }

        /// <summary>
        /// Obtiene un comando de base de datos específico del proveedor de datos
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            IDbCommand command = null;
            switch (this._proveedor)
            {
                case DBProveedores.Odbc:
                    command = new OdbcCommand(cmdText, (OdbcConnection)connection);
                    break;
                case DBProveedores.OleDb:
                    command = new OleDbCommand(cmdText, (OleDbConnection)connection);
                    break;
                case DBProveedores.SqlClient:
                    command = new SqlCommand(cmdText, (SqlConnection)connection);
                    break;
            }
            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private IDataAdapter GetDataAdapter(IDbCommand command)
        {
            IDataAdapter adapter = null;
            switch (this._proveedor)
            {
                case DBProveedores.Odbc:
                    adapter = new OdbcDataAdapter((OdbcCommand)command);
                    break;
                case DBProveedores.OleDb:
                    adapter = new OleDbDataAdapter((OleDbCommand)command);
                    break;
                case DBProveedores.SqlClient:
                    adapter = new SqlDataAdapter((SqlCommand)command);
                    break;
            }
            return adapter;
        }
        #endregion 
    }
}
