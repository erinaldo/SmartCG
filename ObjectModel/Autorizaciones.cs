using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

namespace ObjectModel
{
    public class Autorizaciones
    {
        /// <summary>
        /// Autorización sobre elemento (actualización, consulta, ...)
        /// </summary>
        /// <param name="CLEA05">Clase</param>
        /// <param name="GROA05">Grupo</param>
        /// <param name="ELEA05">Elemento</param>
        /// <param name="OPEA05">Operación</param>
        /// <returns></returns>
        public bool Validar(string CLEA05, string GROA05, string ELEA05, string OPEA05)
        {
            bool result = false;

            IDataReader dr = null;
            string query = "";
            try
            {
                //------------- Debe estar el Usuario y/o Usuario Referencia cumplimentado  ----------------
                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "" &&
                    GlobalVar.UsuarioReferenciaCG != null && GlobalVar.UsuarioReferenciaCG != "")
                {
                    //------------- Es CGIFS  ----------------
                    string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return (true);

                    string CLEA05_PadR3 = CLEA05.PadLeft(3, '0');
                    string GROA05_PadR2 = GROA05.PadLeft(2, '0');
                    string OPEA05_PadR2 = OPEA05.PadLeft(2, '0');
                    string ELEA05_PadR8 = ELEA05.PadRight(8, ' ');
                    string usuarioReferenciaCG_PadR8 = GlobalVar.UsuarioReferenciaCG.PadRight(8, ' ');

                    //------------- Es CGAUDIT  ----------------
                    usuario = ConfigurationManager.AppSettings["USER_CGAUDIT"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario)
                    {
                        if (CLEA05.Trim() == "003" && GROA05.Trim() == "02" && OPEA05.Trim() == "00") return (true);
                        else
                        {
                            string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                            string prefijoTabla = "";
                            if (proveedorTipo == "DB2")
                            {
                                prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                                if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                            }
                            else prefijoTabla = GlobalVar.PrefijoTablaCG;
                            query = "Select INACAC From " + prefijoTabla + "ATM04 ";
                            query += "Where CLELAC = '" + CLEA05_PadR3 + "' and ";
                            query += "GROPAC = '" + GROA05_PadR2 + "' and ";
                            query += "OPERAC = '" + OPEA05_PadR2;

                            dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                            if (dr.Read())
                            {
                                if (dr.GetValue(dr.GetOrdinal("INACAC")).ToString() == "0")
                                {
                                    dr.Close();
                                    return (true);
                                }
                            }
                            dr.Close();
                        }
                    }

                    //------------- Es dueño  ----------------
                    query = "Select IDUSAF From " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                    query += "where CLELAF = '" + CLEA05_PadR3 + "' and ";
                    query += "ELEMAF = '" + ELEA05_PadR8 + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        if (dr.GetValue(dr.GetOrdinal("IDUSAF")).ToString().TrimEnd() == GlobalVar.UsuarioLogadoCG.TrimEnd())
                        {
                            dr.Close();
                            return (true);
                        }
                    }
                    dr.Close();

                    //------------- Autorización Directa  ----------------
                    //------------- * Idus  ----------------
                    //------------- * Elem  ----------------
                    //------------- * Idus + Elem  ----------------
                    //query = "Select MAX(OPERAG) nOPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query += "where CLELAG = '" + CLEA05_PadR3 + "' and ";
                    query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                    query += "((ELEMAG = '" + ELEA05_PadR8 + "' and " + "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "') or ";
                    query += "(ELEMAG = '" + ELEA05_PadR8 + "' and " + "IDUSAG = '*       ') or ";
                    query += "(ELEMAG = '*       ' and " + "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "') or ";
                    query += "(ELEMAG = '*       ' and " + "IDUSAG = '*       ')) ";
                    query += "order by OPERAG desc";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        //if (dr.GetValue(dr.GetOrdinal("nOPERAG")).ToString().TrimEnd() != "") {
                        if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                        {
                            dr.Close();
                            return (true);
                        }
                        //}
                    }
                    dr.Close();

                    //------------- Autorización a Cia.Genérica  ----------------
                    if (CLEA05.TrimEnd() == "002")
                    {
                        //Usuario autorizado a X*
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '" + ELEA05.Substring(0, 1) + "*      ' and ";
                        query += "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();

                        //Usuario autorizado a X*
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '" + ELEA05.Substring(0, 1) + "*      ' and ";
                        query += "IDUSAG = '*       ' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();

                        //Usuario autorizado a *X
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '*" + ELEA05.Substring(1, 1) + "      ' and ";
                        query += "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();

                        //Usuario autorizado a *X
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '*" + ELEA05.Substring(1, 1) + "      ' and ";
                        query += "IDUSAG = '*       ' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();
                    }

                    //------------- Autorización a Tipo Comprobante Genérico  ----------------
                    if (CLEA05.TrimEnd() == "004")
                    {
                        //Usuario autorizado a X*
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '" + ELEA05.Substring(0, 1) + "*      ' and ";
                        query += "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();

                        //Usuario autorizado a X*
                        query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "Where CLELAG = '" + CLEA05_PadR3 + "' and ";
                        query += "GROPAG = '" + GROA05_PadR2 + "' and ";
                        query += "ELEMAG = '" + ELEA05.Substring(0, 1) + "*      ' and ";
                        query += "IDUSAG = '*       ' ";
                        query += "order by OPERAG desc";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("OPERAG")).ToString().TrimEnd()) >= Convert.ToInt32(OPEA05_PadR2))
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Autorización para crear elemento
        /// </summary>
        /// <param name="CLELAC">Clase</param>
        /// <returns></returns>
        public bool CrearElemento(string CLELAC)
        {
            bool result = false;
            try
            {
                //------------- Debe estar el Usuario y/o Usuario Referencia cumplimentado  ----------------
                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "" &&
                    GlobalVar.UsuarioReferenciaCG != null && GlobalVar.UsuarioReferenciaCG != "")
                {
                    //------------- Es CGIFS  ----------------
                    string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return (true);

                    string CLELAC_PadR3 = CLELAC.PadLeft(3, '0');

                    string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                    string query = "Select count (*) From " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                    query += "Where (CLELAE = '" + CLELAC_PadR3 + "' or CLELAE = '*') and ";
                    query += "(IDUSAE = '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "' or IDUSAE = '*') ";

                    int reg = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (reg >= 1) result = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Autorización para operar con un grupo y una operación sin importar el elemento
        /// </summary>
        /// <param name="CLELAG"></param>
        /// <param name="GROPAG"></param>
        /// <param name="OPERAG"></param>
        /// <returns></returns>
        public bool Operar(string CLELAG, string GROPAG, string OPERAG)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //------------- Debe estar el Usuario y/o Usuario Referencia cumplimentado  ----------------
                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "" &&
                    GlobalVar.UsuarioReferenciaCG != null && GlobalVar.UsuarioReferenciaCG != "")
                {
                    //------------- Es CGIFS  ----------------
                    string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
                    if (usuario != null) usuario = usuario.ToUpper();
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return (true);

                    string CLELAG_PadR3 = CLELAG.PadLeft(3, '0');
                    string GROPAG_PadR2 = GROPAG.PadLeft(2, '0');

                    string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                   
                    string query = "Select count (*) From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query += "Where (CLELAG = '" + CLELAG_PadR3 + "' or CLELAG = '*') and ";
                    query += "GROPAG = '" + GROPAG_PadR2 + "' and ";
                    query += "OPERAG >= '" + OPERAG + "' and ";
                    query += "(IDUSAG = '" + GlobalVar.UsuarioReferenciaCG.ToUpper() + "' or IDUSAG = '*') ";

                    int reg = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (reg >= 1) result = true;
                    else
                    {
                        //------------- Es dueño  ----------------
                        query = "Select count(*) From " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                        query += "where CLELAF = '" + CLELAG_PadR3 + "' and ";
                        query += "(IDUSAF = '" + GlobalVar.UsuarioReferenciaCG.TrimEnd() + "' or IDUSAF = '*') ";

                        reg = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                        if (reg >= 1) result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }


        /// <summary>
        /// Autorización para eliminar elemento
        /// </summary>
        /// <param name="CLASE">Clase</param>
        /// <param name="ELEME">Elemento</param>
        /// <returns></returns>
        public bool SuprimirElemento(string CLASE, string ELEME)
        {
            bool result = false;
            try
            {
                //------------- Debe estar el Usuario y/o Usuario Referencia cumplimentado  ----------------
                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "" &&
                    GlobalVar.UsuarioReferenciaCG != null && GlobalVar.UsuarioReferenciaCG != "")
                {
                    //------------- Es CGIFS  ----------------
                    string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return (true);

                    string CLASE_PadR3 = CLASE.PadLeft(3, '0');
                    string ELEME_PadR8 = CLASE.PadRight(8, ' ');

                    string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                    string query = "Select IDUSAF From " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                    query += "Where CLELAF = '" + CLASE_PadR3 + "' and ELEMAF = '" + ELEME_PadR8 + "' and ";
                    query += "IDUSAF ='" + GlobalVar.UsuarioLogadoCG + "'";

                    int reg = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (reg >= 1) result = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Autorización a un proceso
        /// </summary>
        /// <param name="CLASE">Clase</param>
        /// <returns></returns>
        public bool SobreProceso(string CLASE)
        {
            bool result = false;
            IDataReader dr = null;

            string query = "";
            try
            {
                //------------- Debe estar el Usuario y/o Usuario Referencia cumplimentado  ----------------
                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "" &&
                    GlobalVar.UsuarioReferenciaCG != null && GlobalVar.UsuarioReferenciaCG != "")
                {
                    //------------- Es CGIFS  ----------------
                    string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return (true);

                    string CLASE_PadR3 = CLASE.PadLeft(3, '0');
                    string usuarioReferenciaCG_PadR8 = GlobalVar.UsuarioReferenciaCG.PadRight(8, ' ');

                    //------------- Es CGAUDIT  ----------------
                    usuario = ConfigurationManager.AppSettings["USER_CGAUDIT"];
                    if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario)
                    {
                        string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                        string prefijoTabla = "";
                        if (proveedorTipo == "DB2")
                        {
                            prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                            if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                        }
                        else prefijoTabla = GlobalVar.PrefijoTablaCG;

                        query = "Select INACAC From " + prefijoTabla + "ATM04 ";
                        query += "Where CLELAC = '" + CLASE_PadR3 + "' and ";
                        query += "GROPAC = '01' and ";
                        query += "OPERAC = '10'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            if (dr.GetValue(dr.GetOrdinal("INACAC")).ToString() == "0")
                            {
                                dr.Close();
                                return (true);
                            }
                        }
                        dr.Close();
                    }

                    //------------- Autorización Directa  ----------------
                    query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query += "where CLELAG = '" + CLASE_PadR3 + "' and ";
                    query += "GROPAG = '01' and ";
                    query += "ELEMAG = '*' and ";
                    query += "IDUSAG = '" + usuarioReferenciaCG_PadR8 + "' and ";
                    query += "OPERAG = '10'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        dr.Close();
                        return (true);
                    }
                    dr.Close();

                    //------------- Acceso Público ----------------
                    query = "Select OPERAG From " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query += "where CLELAG = '" + CLASE_PadR3 + "' and ";
                    query += "GROPAG = '01' and ";
                    query += "ELEMAG = '*' and ";
                    query += "IDUSAG = '*' and ";
                    query += "OPERAG = '10'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        dr.Close();
                        return (true);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) 
            {
                if (dr != null) dr.Close();

                GlobalVar.Log.Error(ex.Message); 
            }

            return (result);
        }
    }
}
