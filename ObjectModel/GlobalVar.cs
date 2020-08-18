using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using log4net;

namespace ObjectModel
{
    /// <summary>
    /// Contains global variables para la lanzadera y los módulos
    /// </summary>
    public static class GlobalVar
    {
        /// <summary>
        /// Usuario registrado en Windows)
        /// </summary>
        static Usuario _usuarioEnv;

        /// <summary>
        /// Acceso a la variable que contiene el objeto usuario
        /// </summary>
        public static Usuario UsuarioEnv
        {
            get
            {
                return _usuarioEnv;
            }
            set
            {
                _usuarioEnv = value;
            }
        }

        /// <summary>
        /// Usuario registrado en Windows)
        /// </summary>
        static Entorno _entornoActivo;

        /// <summary>
        /// Acceso a la variable que contiene el objeto usuario
        /// </summary>
        public static Entorno EntornoActivo
        {
            get
            {
                return _entornoActivo;
            }
            set
            {
                _entornoActivo = value;
            }
        }
        /// <summary>
        /// Forma de acceso a la base de datos (OleDb,Odbc,SqlClient)
        /// </summary>
        static string _proveedorDatosCG;

        /// <summary>
        /// Acceso a la variable que contiene la cadena de conexión a la bbdd.
        /// </summary>
        public static string ProveedorDatosCG
        {
            get
            {
                return _proveedorDatosCG;
            }
            set
            {
                _proveedorDatosCG = value;
            }
        }

        /// <summary>
        /// Motor de base de datos (DB2,SQLServer)
        /// </summary>
        static string _tipoBaseDatosCG;

        /// <summary>
        /// Acceso a la variable que contiene el motor de bbdd
        /// </summary>
        public static string TipoBaseDatosCG
        {
            get
            {
                return _tipoBaseDatosCG;
            }
            set
            {
                _tipoBaseDatosCG = value;
            }
        }


        /// <summary>
        /// Cadena de conexión a la base de datos (OleDb,Odbc,SqlClient)
        /// </summary>
        static string _cadenaConexionCG;

        /// <summary>
        /// Acceso a la variable que contiene la cadena de conexión a la bbdd
        /// </summary>
        public static string CadenaConexionCG
        {
            get
            {
                return _cadenaConexionCG;
            }
            set
            {
                _cadenaConexionCG = value;
            }
        }

        /// <summary>
        /// Prefijo de las tablas de CG
        /// </summary>
        static string _prefijoTablaCG;

        /// <summary>
        /// Acceso a la variable que contiene el prefijo a las tablas
        /// </summary>
        public static string PrefijoTablaCG
        {
            get
            {
                return _prefijoTablaCG;
            }
            set
            {
                _prefijoTablaCG = value;
            }
        }


        /// <summary>
        /// Prefijo de las tablas de CG
        /// </summary>
        static ProveedorDatos _conexionCG;

        /// <summary>
        /// Acceso a la variable que contiene el prefijo a las tablas
        /// </summary>
        public static ProveedorDatos ConexionCG
        {
            get
            {
                return _conexionCG;
            }
            set
            {
                _conexionCG = value;
            }
        }

        /// <summary>
        /// Idioma seleccionado
        /// </summary>
        static string _languageProvider;

        /// <summary>
        /// Acceso a la variable que contiene el idioma
        /// </summary>
        public static string LanguageProvider
        {
            get
            {
                return _languageProvider;
            }
            set
            {
                _languageProvider = value;
            }
        }

        /// <summary>
        /// Indica el código del usuario que se ha logado a la base de datos
        /// </summary>
        static string _usuarioLogadoCG_BBDD;

        /// <summary>
        /// Acceso a la variable que contiene el código del usuario que se ha logado a la base de datos
        /// </summary>
        public static string UsuarioLogadoCG_BBDD
        {
            get
            {
                return _usuarioLogadoCG_BBDD;
            }
            set
            {
                _usuarioLogadoCG_BBDD = value;
            }
        }

        /// <summary>
        /// Indica el código del usuario que se ha logado a la aplicación CG
        /// </summary>
        static string _usuarioLogadoCG;

        /// <summary>
        /// Acceso a la variable que contiene el código del usuario que se ha logado a la aplicación CG
        /// </summary>
        public static string UsuarioLogadoCG
        {
            get
            {
                return _usuarioLogadoCG;
            }
            set
            {
                _usuarioLogadoCG = value;
            }
        }

        /// <summary>
        /// Indica el nombre del usuario que se ha logado a la aplicación CG
        /// </summary>
        static string _usuarioLogadoCG_Nombre;

        /// <summary>
        /// Acceso a la variable que contiene el nombre del usuario que se ha logado a la aplicación CG
        /// </summary>
        public static string UsuarioLogadoCG_Nombre
        {
            get
            {
                return _usuarioLogadoCG_Nombre;
            }
            set
            {
                _usuarioLogadoCG_Nombre = value;
            }
        }

        /// <summary>
        /// Indica el tipo de seguridad del usuario que se ha logado a la aplicación CG (corresponde al campo UADMMO de la tabla ATM05)
        /// </summary>
        static string _usuarioLogadoCG_TipoSeguridad;

        /// <summary>
        /// Acceso a la variable que contiene el tipo de seguridad del usuario que se ha logado a la aplicación CG
        /// </summary>
        public static string UsuarioLogadoCG_TipoSeguridad
        {
            get
            {
                return _usuarioLogadoCG_TipoSeguridad;
            }
            set
            {
                _usuarioLogadoCG_TipoSeguridad = value;
            }
        }

        /// <summary>
        /// Indica el usuario referencia del usuario que se ha logado a la aplicación CG
        /// </summary>
        static string _usuarioReferenciaCG;

        /// <summary>
        /// Acceso a la variable que indica el usuario referencia del usuario que se ha logado a la aplicación CG
        /// </summary>
        public static string UsuarioReferenciaCG
        {
            get
            {
                return _usuarioReferenciaCG;
            }
            set
            {
                _usuarioReferenciaCG = value;
            }
        }

        /// <summary>
        /// Contiene el elemento seleccionado que devuelve el formulario (ObjectModel.TGElementoSel)
        /// </summary>
        static ArrayList _elementosSel;

        /// <summary>
        /// Acceso a la variable que contiene el elemento seleccionado (ObjectModel.TGElementoSel)
        /// </summary>
        public static ArrayList ElementosSel
        {
            get
            {
                return _elementosSel;
            }
            set
            {
                _elementosSel = value;
            }
        }

        /// <summary>
        /// Formato de las fechas que utiliza CG
        /// </summary>
        static string _cgFormatoFecha;

        /// <summary>
        /// Acceso a la variable que contiene el formato de las fechas que utiliza CG
        /// </summary>
        public static string CGFormatoFecha
        {
            get
            {
                return _cgFormatoFecha;
            }
            set
            {
                _cgFormatoFecha = value;
            }
        }


        /// <summary>
        /// Contiene la instancia de un objeto logger
        /// </summary>
        static ILog _logger;

        /// <summary>
        /// Acceso a la variable que contiene la instancia de un objeto logger
        /// </summary>
        public static ILog Log
        {
            get
            {
                return _logger;
            }
            set
            {
                _logger = value;
            }
        }

        /// <summary>
        /// Contiene la lista de autorizaciones que se han ido consultando para el usuario logado (en los informes y en las consultas)
        /// </summary>
        static ArrayList _listaAutorizaciones;

        /// <summary>
        /// Acceso a la variable que contiene la lista de autorizaciones consultadas para el usuario
        /// </summary>
        public static ArrayList ListaAutorizaciones
        {
            get
            {
                return _listaAutorizaciones;
            }
            set
            {
                _listaAutorizaciones = value;
            }
        }

    }
}
