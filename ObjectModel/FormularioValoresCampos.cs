using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace ObjectModel
{
    public class FormularioValoresCampos
    {
        protected string userlogado = System.Environment.UserName.ToUpper();

        protected string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

        public bool LeerParametros(string formCode, ref string valores)
        {
            bool result = false;

            valores = "";

            IDataReader dr = null;

            try
            {
                string userWindows = userlogado;
                if (userWindows.Length > 10) userWindows = userWindows.Substring(0, 10);

                string query = "select PARML1 from " + GlobalVar.PrefijoTablaCG + "GLL01 ";
                query += "where PGMPL1 = '" + formCode + "' and IDUSL1 = '";
                query += userWindows + "' and USERL1 = '" + GlobalVar.UsuarioLogadoCG + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    valores = dr.GetValue(dr.GetOrdinal("PARML1")).ToString();
                    result = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }

        public bool GrabarParametros(string formCode, string valores)
        {
            bool result = false;
            try
            {
                string valoresTabla = "";

                DateTime fechaHora = System.DateTime.Now;
                string anno = fechaHora.Date.Year.ToString();
                if (anno.Length > 2) anno = anno.Substring(anno.Length-2, 2);
                string fecha = fechaHora.Date.Day.ToString() + fechaHora.Date.Month.ToString() + anno;
                string hora = fechaHora.Hour.ToString() + fechaHora.Minute.ToString() + fechaHora.Second.ToString();
                decimal fechaDec = Convert.ToDecimal(fecha);
                decimal horaDec = Convert.ToDecimal(hora);

                if (LeerParametros(formCode, ref valoresTabla))
                {
                    //Actualizar Parametros
                    this.ActualizarParametros(formCode, valores, fechaDec, horaDec);
                }
                else
                {
                    //Insertar Parametros
                    this.InsertarParametros(formCode, valores, fechaDec, horaDec);
                }
                result = true;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        private void InsertarParametros(string formCode, string valores, decimal fecha, decimal hora)
        {
            try
            {
                string userWindows = userlogado;
                if (userWindows.Length > 10) userWindows = userWindows.Substring(0, 10);

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLL01";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "PGMPL1, IDUSL1, USERL1, FECHL1, HORAL1, PARML1) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + formCode + "', '" + userWindows + "', '" + GlobalVar.UsuarioLogadoCG + "', " + fecha + ", " + hora + ", '" + valores + "')";

                GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void ActualizarParametros(string formCode, string valores, decimal fecha, decimal hora)
        {
            try
            {
                string userWindows = userlogado;
                if (userWindows.Length > 10) userWindows = userWindows.Substring(0, 10);

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLL01 set ";
                query += "FECHL1 = " + fecha + ", HORAL1 = " + hora + ", PARML1 = '" + valores + "' ";
                query += "where PGMPL1 = '" + formCode + "' and IDUSL1 = '" + userWindows + "' and USERL1 = '" + GlobalVar.UsuarioLogadoCG + "'";

                GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }
    }
}
