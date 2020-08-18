using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using ObjectModel;
using log4net;

namespace ObjectModel
{
    public class XContro
    {
        private string iden;
        private string equipname;
        private string usuario;
        private ColasName colaname;
        private string prioridad;
        private string proctipo;
        private string fecha;
        private string hora;
        private string horaini;
        private string procname;
        private string descname;
        private string parm;
        private string status;
        private string pid;
        private string pidmenu;
        private string pidpanta;

        private string lda_BTCLDA;
        private string lda_Variable;

        private string prefijoTabla;

        private Utiles utiles;
        //private LanguageProvider LP;
        private ILog Log;

        #region Properties
        public string Iden
        {
            get
            {
                return (this.iden);
            }
            set
            {
                this.iden = value;
            }
        }

        public string EquipName
        {
            get
            {
                return (this.equipname);
            }
            set
            {
                this.equipname = value;
            }
        }

        public string Usuario
        {
            get
            {
                return (this.usuario);
            }
            set
            {
                this.usuario = value;
            }
        }

        public ColasName ColaName
        {
            get
            {
                return (this.colaname);
            }
            set
            {
                this.colaname = value;
            }
        }

        public string Prioridad
        {
            get
            {
                return (this.prioridad);
            }
            set
            {
                this.prioridad = value;
            }
        }

        public string ProcTipo
        {
            get
            {
                return (this.proctipo);
            }
            set
            {
                this.proctipo = value;
            }
        }

        public string Fecha
        {
            get
            {
                return (this.fecha);
            }
            set
            {
                this.fecha = value;
            }
        }

        public string Hora
        {
            get
            {
                return (this.hora);
            }
            set
            {
                this.hora = value;
            }
        }

        public string HoraIni
        {
            get
            {
                return (this.horaini);
            }
            set
            {
                this.horaini = value;
            }
        }

        public string ProcName
        {
            get
            {
                return (this.procname);
            }
            set
            {
                this.procname = value;
            }
        }

        public string DescName
        {
            get
            {
                return (this.descname);
            }
            set
            {
                this.descname = value;
            }
        }

        public string Parm
        {
            get
            {
                return (this.parm);
            }
            set
            {
                this.parm = value;
            }
        }

        public string Status
        {
            get
            {
                return (this.status);
            }
            set
            {
                this.status = value;
            }
        }

        public string Pid
        {
            get
            {
                return (this.pid);
            }
            set
            {
                this.pid = value;
            }
        }

        public string PidMenu
        {
            get
            {
                return (this.pidmenu);
            }
            set
            {
                this.pidmenu = value;
            }
        }

        public string PidPanta
        {
            get
            {
                return (this.pidpanta);
            }
            set
            {
                this.pidpanta = value;
            }
        }

        public string LDA_BTCLDA
        {
            get
            {
                return (this.lda_BTCLDA);
            }
            set
            {
                this.lda_BTCLDA = value;
            }
        }

        public string LDA_Variable
        {
            get
            {
                return (this.lda_Variable);
            }
            set
            {
                this.lda_Variable = value;
            }
        }
        #endregion

        public XContro()
        {
            this.utiles = new Utiles();
            this.Log = log4net.LogManager.GetLogger(this.GetType());

            this.iden = "";
            this.equipname = "";
            this.usuario = "";
            //this.colaname
            this.prioridad = "";
            this.proctipo = "";
            this.fecha = "";
            this.hora = "";
            this.horaini = "";
            this.procname = "";
            this.descname = "";
            this.parm = "";
            this.status = "";
            this.pid = "";
            this.pidmenu = "";
            this.pidpanta = "";

            this.lda_BTCLDA = "";

            this.prefijoTabla = "";
            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            if (tipoBaseDatosCG == "DB2")
            {
                this.prefijoTabla = ConfigurationManager.AppSettings["bbddCGUF"];
                if (this.prefijoTabla != null && this.prefijoTabla != "") this.prefijoTabla += ".";
            }
        }

        #region Métodos públicos
        /// <summary>
        /// Inserta un registro en la tabla XContro
        /// </summary>
        /// <returns></returns>
        public string Insertar()
        {
            string result = "";
            try
            {
                //Buscar el EquipName partiendo de la cola
                XCola xcola = new XCola
                {
                    ColaName = this.colaname
                };
                string resultLeerCola = xcola.LeerCola();
                if (resultLeerCola == "") this.equipname = xcola.EquipName;
                 
                //Buscar valores fecha actual
                DateTime fecha = DateTime.Now;
                string hora = fecha.ToString("HH:mm:ss");
                this.hora = hora.Replace(":", "");
                this.fecha = fecha.Year.ToString() + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0');

                string lda = this.CrearLDACabecera(fecha) + this.lda_Variable;
                this.parm += lda;

                //Calcular campo iden
                this.iden = this.CalcularCampoIden();
                
                string nombreTabla = this.prefijoTabla + "XCONTRO"; //jl

                //string query = "insert into " + this.prefijoTabla + "XCONTRO (IDEN, EQUIPNAME, USUARIO, COLANAME, PRIORIDAD, PROCTIPO, FECHA, HORA, HORAINI, PROCNAME, DESCNAME, PARM, STATUS, PID, PIDMENU, PIDPANTA) ";
                string query = "insert into " + nombreTabla + " (IDEN, EQUIPNAME, USUARIO, COLANAME, PRIORIDAD, PROCTIPO, FECHA, HORA, HORAINI, PROCNAME, DESCNAME, PARM, STATUS, PID, PIDMENU, PIDPANTA) ";
                query += "values ('" + this.iden + "', '" + this.equipname + "', '" + this.usuario + "', '" + this.colaname + "', '" + this.prioridad + "', '";
                query += this.proctipo + "', '" + this.fecha + "', '" + this.hora + "', '" + this.horaini + "', '" + this.procname + "', '";
                query += this.descname + "', '" + this.parm + "', '" + this.status + "', '" + this.pid + "', '" + this.pidmenu + "', '";
                query += this.pidpanta;
                query += "')";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) 
            {
                result = "Error (" + ex.Message + ")";
                Log.Error(Utiles.CreateExceptionString(ex)); 
            }

            return (result);
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Devuelve el identificador que le corresponde a la cola solicitada
        /// </summary>
        /// <returns></returns>
        private string CalcularCampoIden()
        {
            string campoIden = "1";
            IDataReader dr = null;

            try
            {
                string query = "select max(IDEN) as MaxIden from " + this.prefijoTabla + "XCONTRO where COLANAME='" + this.colaname.ToString() + "'";

                string iden = "";
                int idenInt = 1;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    iden = dr.GetValue(dr.GetOrdinal("MaxIden")).ToString();
                }

                dr.Close();

                if (iden != "")
                {
                    try
                    {
                        idenInt = Convert.ToInt32(iden);
                        idenInt += 1;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                campoIden = idenInt.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            campoIden = campoIden.PadLeft(8, '0');

            return (campoIden);
        }


        /// <summary>
        /// Crea la parte inicial "fija" de la LDA
        /// </summary>
        /// <returns></returns>
        private string CrearLDACabecera(DateTime fecha)
        {
            string ldaIni = "";
            try
            {
                ldaIni = "@[LDA]*CGJOB";
                ldaIni += "CGIFS".PadRight(10, ' ');
                string aux = fecha.Year.ToString();
                if (aux.Length == 4) aux = aux.Substring(2, 2);
                ldaIni += fecha.Day.ToString().PadLeft(2, '0') + "/" + fecha.Month.ToString().PadLeft(2, '0')  + "/" + aux;
                ldaIni += "CGIFS".PadRight(10, ' ');
                aux = GlobalVar.UsuarioLogadoCG.PadRight(8, ' ');
                ldaIni += aux;
                ldaIni += aux;
                aux = "CGMENUFM".PadRight(10, ' ');
                ldaIni += aux;
                ldaIni += "1";
                ldaIni += aux;
                ldaIni += "M";
                aux = "*DFT".PadRight(20, ' ');
                ldaIni += aux;
                ldaIni += "0";
                ldaIni += "000001663860000";
                ldaIni += "*";
                ldaIni += "1";
                ldaIni += "4";
                ldaIni += "PTA";
                ldaIni += "EUR";
                ldaIni += "00001";
                ldaIni += this.lda_BTCLDA.PadRight(10, ' ');
                ldaIni += "0";
                ldaIni += CGParametrosGrles.GLC01_FECHRC.PadRight(1, ' ');
                ldaIni += CGParametrosGrles.GLC01_EDITRC.PadRight(1, ' ');
                ldaIni += CGParametrosGrles.GLC01_MCIARC.PadRight(1, ' ');
                ldaIni += CGParametrosGrles.GLC01_REFURC.PadRight(1, ' ');
                ldaIni += CGParametrosGrles.GLC01_PASWRC.PadRight(1, ' ');
                ldaIni += "  ";
                ldaIni += CGParametrosGrles.GLC01_CHKCRC.PadRight(1, ' ');
                ldaIni += " ";
                ldaIni += CGParametrosGrles.GLC01_CANTRC.PadRight(1, ' ');
                ldaIni += "0";
                ldaIni += "0";
                ldaIni += CGParametrosGrles.GLC01_ALSIRC.PadRight(1, ' ');
                ldaIni += "0";
                ldaIni += " ";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (ldaIni);
        }
        #endregion
    }
}
