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
    /// <summary>
    /// Enumera las colas soportados
    /// </summary>
    public enum ColasName
    {
        CGPOST,
        CGAUTO,
        CGJOBD,
        CGJOBQDC,
        EXCLUSIVE
    }

    public class XCola
    {
        private string iden;
        private string equipname;
        private ColasName colaname;
        private string estado;
        private string programa;
        private string subsist;
        private string pid;
        private string fiprogra;
        private string numproce;

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

        public string Estado
        {
            get
            {
                return (this.estado);
            }
            set
            {
                this.estado = value;
            }
        }

        public string Programa
        {
            get
            {
                return (this.programa);
            }
            set
            {
                this.programa = value;
            }
        }

        public string Subsist
        {
            get
            {
                return (this.subsist);
            }
            set
            {
                this.subsist = value;
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

        public string Fiprogra
        {
            get
            {
                return (this.fiprogra);
            }
            set
            {
                this.fiprogra = value;
            }
        }

        public string Numproce
        {
            get
            {
                return (this.numproce);
            }
            set
            {
                this.numproce = value;
            }
        }
        #endregion

        public XCola()
        {
            this.utiles = new Utiles();
            this.Log = log4net.LogManager.GetLogger(this.GetType());

            this.iden = "";
            this.equipname = "";
            this.estado = "";
            this.programa = "";
            this.subsist = "";
            this.pid = "";
            this.fiprogra = "";
            this.numproce = "";
        }

        #region Métodos públicos
        public string LeerCola()
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

                string prefijoTabla = "";
                if (tipoBaseDatosCG == "DB2")
                {
                    prefijoTabla = ConfigurationManager.AppSettings["bbddCGUF"];
                    if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                }

                string query = "select * from " + prefijoTabla + "XCOLA where COLANAME='" + this.colaname.ToString() + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.iden = dr.GetValue(dr.GetOrdinal("IDEN")).ToString();
                    this.equipname = dr.GetValue(dr.GetOrdinal("EQUIPNAME")).ToString();
                    this.estado = dr.GetValue(dr.GetOrdinal("ESTADO")).ToString();
                    this.programa = dr.GetValue(dr.GetOrdinal("PROGRAMA")).ToString();
                    this.subsist = dr.GetValue(dr.GetOrdinal("SUBSIST")).ToString();
                    this.pid = dr.GetValue(dr.GetOrdinal("PID")).ToString();
                    this.fiprogra = dr.GetValue(dr.GetOrdinal("FIPROGRA")).ToString();
                    this.numproce = dr.GetValue(dr.GetOrdinal("NUMPROCE")).ToString();
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = "Error (" + ex.Message + ")";
            }

            return (result);
        }
        #endregion
    }
}
