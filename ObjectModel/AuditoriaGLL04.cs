using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace ObjectModel
{
    public class AuditoriaGLL04
    {
        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum OperacionTipo
        {
            Alta,
            Eliminar,
            Modificar
        }

        protected string ordenadorNombre_JBNAL4;
        protected string tabla_FILEL4;
        protected OperacionTipo operacion;
        protected string operacion_OPERL4;
        protected string usuarioWin_JBUSL4;
        protected string pid_JBNML4;
        protected string programa_PGMPL4;
        protected string usuarioApp_USERL4;
        protected string fecha_FECIL4;
        protected string hora_HORIL4;
        protected string CODIL4;
        protected string valorOld_PRM1L4;
        protected string valorNew_PRM2L4;
        protected string SEC1L4;
        protected string clave1_KEY1L4;
        protected string clave2_KEY2L4;
        protected string clave3_KEY3L4;

        #region Properties
        public string Tabla_FILEL4
        {
            get
            {
                return (this.tabla_FILEL4);
            }
            set
            {
                this.tabla_FILEL4 = value;
            }
        }

        public OperacionTipo Operacion
        {
            get
            {
                return (this.operacion);
            }
            set
            {
                this.operacion = value;
            }
        }

        public string Programa_PGMPL4
        {
            get
            {
                return (this.programa_PGMPL4);
            }
            set
            {
                this.programa_PGMPL4 = value;
            }
        }

        public string Clave1_KEY1L4
        {
            get
            {
                return (this.clave1_KEY1L4);
            }
            set
            {
                this.clave1_KEY1L4 = value;
            }
        }

        public string Clave2_KEY2L4
        {
            get
            {
                return (this.clave2_KEY2L4);
            }
            set
            {
                this.clave2_KEY2L4 = value;
            }
        }

        public string Clave3_KEY3L4
        {
            get
            {
                return (this.clave3_KEY3L4);
            }
            set
            {
                this.clave3_KEY3L4 = value;
            }
        }

        public string ValorOld_PRM1L4
        {
            get
            {
                return (this.valorOld_PRM1L4);
            }
            set
            {
                this.valorOld_PRM1L4 = value;
            }
        }

        public string ValorNew_PRM2L4
        {
            get
            {
                return (this.valorNew_PRM2L4);
            }
            set
            {
                this.valorNew_PRM2L4 = value;
            }
        }
        #endregion

        protected string userlogado = Environment.UserName.ToUpper();

        protected string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

        public AuditoriaGLL04()
        {
            this.ordenadorNombre_JBNAL4 = Environment.MachineName;
            this.tabla_FILEL4 = " ";
            this.operacion_OPERL4 = " ";
            this.usuarioWin_JBUSL4 = userlogado;

            try
            {
                System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                string pid = currentProcess.Id.ToString();
                this.pid_JBNML4 = pid;
            }
            catch (Exception ex) { this.pid_JBNML4 = " ";   GlobalVar.Log.Error(ex.Message); }
           
            this.programa_PGMPL4 = " ";
            this.usuarioApp_USERL4 = GlobalVar.UsuarioLogadoCG;
            this.fecha_FECIL4 = "0";
            this.hora_HORIL4 = "0";
            this.CODIL4 = " ";
            this.valorOld_PRM1L4 = " ";
            this.valorNew_PRM2L4 = " ";
            this.SEC1L4 = "";
            this.clave1_KEY1L4 = " ";
            this.clave2_KEY2L4 = " ";
            this.clave3_KEY3L4 = " ";
        }

        public string InsertarGLL04()
        {
            string result = "";

            try
            {
                Utiles utiles = new Utiles();

                if (this.ordenadorNombre_JBNAL4 != null && this.ordenadorNombre_JBNAL4.Length > 10) this.ordenadorNombre_JBNAL4 = this.ordenadorNombre_JBNAL4.Substring(0, 10);

                switch (this.operacion)
                {
                    case OperacionTipo.Alta:
                        this.operacion_OPERL4 = "A";
                        break;
                    case OperacionTipo.Eliminar:
                        this.operacion_OPERL4 = "D";
                        break;
                    case OperacionTipo.Modificar:
                        this.operacion_OPERL4 = "U";
                        break;
                }

                DateTime fechaHoraAcual = DateTime.Now;
                this.fecha_FECIL4 = utiles.FechaToFormatoCG(fechaHoraAcual, true).ToString();
                this.hora_HORIL4 = fechaHoraAcual.ToString("HHmmss");

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLL04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "JBNAL4, FILEL4, OPERL4, JBUSL4, JBNML4, PGMPL4, USERL4, FECIL4, HORIL4, CODIL4, PRM1L4, ";
                query += "PRM2L4, SEC1L4, KEY1L4, KEY2L4, KEY3L4) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.ordenadorNombre_JBNAL4 + "', '" + this.tabla_FILEL4 + "', '" + this.operacion_OPERL4 + "', '";
                query += usuarioWin_JBUSL4 + "', '" + this.pid_JBNML4 + "', '" + this.programa_PGMPL4 + "', '";
                query += this.usuarioApp_USERL4 + "', " + this.fecha_FECIL4 + ", " + this.hora_HORIL4 + ", '";
                query += this.CODIL4 + "', '" + this.valorOld_PRM1L4 + "', '" + this.valorNew_PRM2L4+ "', '" + this.SEC1L4 + "', '";
                query += this.clave1_KEY1L4 + "', '" + this.clave2_KEY2L4 + "', '" + this.clave3_KEY3L4 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return result;
        }
    }
}
