using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace ObjectModel
{
    public class UtilesCGConsultas
    {
        /// <summary>
        /// Devuelve el tipo de calendario para la compañía solicitada
        /// posicion 0  -> tipo calendario
        ///          1  -> plan de la compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        /// <returns></returns>
        public string[] ObtenerTipoCalendarioCompania(string codigo)
        {
            string[] result = new string[2];

            IDataReader dr = null;
            try
            {
                string query = " select * from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG ='" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result[0] = dr.GetValue(dr.GetOrdinal("TITAMG")).ToString();
                    result[1] = dr.GetValue(dr.GetOrdinal("TIPLMG")).ToString();
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

        /// <summary>
        /// Devuelve la fecha de inicio y la fecha fin del calendario
        /// posicion    0 -> fecha inicio
        ///             1 -> fecha fin
        /// </summary>
        /// <param name="tipoCalendario">Tipo de Calendario para la compañía</param>
        /// <param name="sapr">Siglo año periodo</param>
        /// <returns></returns>
        public string[] ObtenerFechasCalendarioDadoSAPR(string tipoCalendario, string sapr)
        {
            string[] fechas = new string[2];

            IDataReader dr = null;
            try
            {
                string query = " select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL ='" + tipoCalendario + "' and SAPRFL = " + sapr;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    fechas[0] = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    fechas[1] = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (fechas);
        }

        /// <summary>
        /// Devuelve la fecha final del período igual o más cercano al periodo solicitado
        /// </summary>
        /// <param name="tipoCalendario">Tipo de Calendario para la compañía</param>
        /// <param name="sapr">Siglo año periodo cualquiera (puede estar definido o no en la tabla de calendarios)</param>
        /// <returns></returns>
        public string ObtenerFechaFinCalendarioDadoPeriodo(string tipoCalendario, string sapr)
        {
            string fecha = "";

            IDataReader dr = null;
            try
            {
                string query = " select MAX(FINLFL) FINLFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL ='" + tipoCalendario + "' and SAPRFL <= " + sapr;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    fecha = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (fecha);
        }


        /// <summary>
        /// Devuelve el siglo año periodo correspondiente a una fecha y un calendario
        /// </summary>
        /// <param name="tipoCalendario">Tipo de Calendario para la compañía</param>
        /// <param name="fecha">Fecha en formato CG</param>
        /// <returns></returns>
        public string ObtenerSAPRCalendarioDadoFecha(string tipoCalendario, string fecha)
        {
            string sapr = "";

            IDataReader dr = null;
            try
            {
                string query = " select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL ='" + tipoCalendario + "' and ";
                query += "INLAFL <= " + fecha + " and FINLFL >= " + fecha;
                query += " order by INLAFL";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    sapr = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (sapr);
        }

        /// <summary>
        /// Devuelve el último siglo-año-periodo cerrado para la compañía solicitada
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        /// <returns></returns>
        public string ObtenerUltAAPPCerradoCompania(string codigo)
        {
            string result = "";

            IDataReader dr = null;
            try
            {
                string query = " select * from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG ='" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("ULACMG")).ToString();
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


        /// <summary>
        /// Devuelve si el siglo-año-periodo para la compañía está cerrado o ono
        /// </summary>
        /// <param name="codigoComp">código de la compañía</param>
        /// <param name="sap">siglo año periodo</param>
        /// <returns></returns>
        public bool SAAPPCerrado(string codigoComp, string sap)
        {
            bool result = false;

            IDataReader dr = null;
            try
            {
                string ultSAPCerrado = this.ObtenerUltAAPPCerradoCompania(codigoComp);

                int periodoCerrado = 0;

                if (ultSAPCerrado != "") periodoCerrado =  Convert.ToInt32(ultSAPCerrado);

                int periodoActual = Convert.ToInt32(sap);

                if (periodoCerrado != 0 && periodoActual <= periodoCerrado) result = true;  
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }


        /// <summary>
        /// Devueve los códigos de compañías de un grupo y los nombres de las compañías
        /// </summary>
        /// <param name="codigoGrupo">código de grupo de compañías</param>
        /// <param name="plan">plan de cuentas</param>
        /// <returns></returns>
        public ArrayList ObtenerCodEmpresasDelGrupo(string codigoGrupo, string plan)
        {
            ArrayList codigos = new ArrayList();
            string[] codDes;

            IDataReader dr = null;
            try
            {
                string query = " select distinct * from " + GlobalVar.PrefijoTablaCG + "GLM01J ";
                query += "where STATMG='V' and GRUPAI='" + codigoGrupo+ "' ";
                
                if (plan != "") query += "and TIPLMG='" + plan + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    codDes = new string[2];
                    codDes[0] = dr.GetValue(dr.GetOrdinal("CCIAAI")).ToString();
                    codDes[1] = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString().Trim();
                    codigos.Add(codDes);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (codigos);
        }

        /// <summary>
        /// Devuelve datos de la cuenta solicitada
        /// posicion    0 -> NOLAAD
        ///             1 -> CEDTMC
        /// </summary>
        /// <param name="cuenta">código de la cuenta</param>
        /// <param name="plan">código del plan</param>
        /// <returns></returns>
        public ArrayList ObtenerDatosCuenta(string cuenta, string plan)
        {
            ArrayList cuentaFormat = new ArrayList();

            IDataReader dr = null;
            try
            {
                string query = " select NOLAAD, CEDTMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where CUENMC ='" + cuenta + "' and ";
                query += "TIPLMC = '" + plan + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    cuentaFormat.Add(dr.GetValue(dr.GetOrdinal("NOLAAD")).ToString());
                    cuentaFormat.Add(dr.GetValue(dr.GetOrdinal("CEDTMC")).ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (cuentaFormat);
        }

        /// <summary>
        /// Devuelve la descripción del comprobante solicitado
        /// </summary>
        /// <param name="compania">código de la compañía</param>
        /// <param name="sigloanoperiodo">siglo año período</param>
        /// <param name="tico">tipo de comprobante</param>
        /// <param name="nuco">número de comprobante</param>
        /// <returns></returns>
        public string ObtenerDescripcionComprobante(string compania, string sigloanoperiodo, string tico, string nuco)
        {
            string desc = "";

            IDataReader dr = null;
            try
            {
                string query = "select COHEAD from " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                query += "where CCIAAD ='" + compania + "' and ";
                query += "SAPRAD = " + sigloanoperiodo + " and ";
                query += "TICOAD = " + tico + " and ";
                query += "NUCOAD = " + nuco;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("COHEAD")).ToString().Trim();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (desc);
        }


        /// <summary>
        /// Devuelve el nivel y la longitud máxima dado un plan de cuentas
        /// posicion    1 -> Nivel
        ///             2 -> Longitud máxima
        /// </summary>
        /// <param name="plan">código del plan</param>
        /// <returns></returns>
        public int[] ObtenerNivelLongitudDadoPlan(string plan)
        {
            int[] result = {-1, -1};

            IDataReader dr = null;
            try
            {
                string ecmp = "";

                string query = " select ECMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + plan + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    ecmp = dr.GetValue(dr.GetOrdinal("ECMP")).ToString();
                }
                    
                dr.Close();

                if (ecmp != "")
                {
                    char aux;
                    int nivel = 0;
                    int longitudMax = 0;
                    int valor;
                    for (int i = 0; i < ecmp.Length; i++)
                    {
                        aux = ecmp[i];
                        valor = Convert.ToInt16(aux.ToString());
                        if (aux != '0')
                        {
                            nivel++;
                            longitudMax += valor;
                        }
                    }
                    result[0] = nivel;
                    result[1] = longitudMax;
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
        /// Devuelve el nivel, la longitud máxima y las longitudes dado un plan de cuentas
        /// posicion    1 -> Nivel
        ///             2 -> Longitud máxima
        ///             3 - 11 -> Longitud de cada nivel
        /// </summary>
        /// <param name="plan">código del plan</param>
        /// <returns></returns>
        public int[] ObtenerNivelLongitudesDadoPlan(string plan)
        {
            int[] result = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            IDataReader dr = null;
            try
            {
                string ecmp = "";

                string query = " select ECMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + plan + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    ecmp = dr.GetValue(dr.GetOrdinal("ECMP")).ToString();
                }

                dr.Close();

                if (ecmp != "")
                {
                    char aux;
                    int nivel = 0;
                    int longitudMax = 0;
                    int valor;
                    for (int i = 0; i < ecmp.Length-1; i++)
                    {
                        aux = ecmp[i];
                        valor = Convert.ToInt16(aux.ToString());
                        if (aux != '0')
                        {
                            nivel++;
                            longitudMax += valor;
                        }
                        result[i + 2] = valor;
                    }
                    result[0] = nivel;
                    result[1] = longitudMax;
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
        /// Devuelve la estructura y la máscara de edición dado un plan de cuentas
        /// posicion    1 -> Estructura
        ///             2 -> Máscara de Edición
        /// </summary>
        /// <param name="plan">código del plan</param>
        /// <returns></returns>
        public string[] ObtenerEstructuraMascaraDadoPlan(string plan)
        {
            string[] result = { "", "" };

            IDataReader dr = null;
            try
            {
                string ecmp = "";
                string emmp = "";

                string query = "select ECMP, EMMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + plan + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    ecmp = dr.GetValue(dr.GetOrdinal("ECMP")).ToString();
                    emmp = dr.GetValue(dr.GetOrdinal("EMMP")).ToString();
                }

                dr.Close();

                result[0] = ecmp;
                result[1] = emmp;
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (result);
        }


        /// <summary>
        /// Devuelve el saldo
        /// posición    1 -> saldo debe
        ///             2 -> saldo haber
        ///             3 -> saldo debe + haber
        /// </summary>
        /// <param name="codigoCompania">código de la compañía</param>
        /// <param name="plan">código del plan contable</param>
        /// <param name="sigloanoperiodoDesde">siglo año período desde</param>
        /// <param name="sigloanoperiodoHasta">siglo año período hasta</param>
        /// <param name="cuenta">código de la cuenta</param>
        /// <param name="tipoDato">tipo de Dato ('R ', 'E ', 'U ', <extracontables>, ....)</param>
        /// <param name="taux">tipo de auxiliar</param>
        /// <param name="caux">cuenta de auxiliar</param>
        /// <returns>posicion 0-> debe 1-> haber 2-> debe + haber</returns>
        public decimal[] ObtenerSaldo(string codigoCompania, string plan, string sigloanoperiodoDesde, string sigloanoperiodoHasta, string cuenta, string tipoDato, string taux, string caux)
        {
            decimal[] saldo = {-1, -1, -1};

            IDataReader dr = null;
            try
            {
                string sigloanoDesde = sigloanoperiodoDesde.Substring(0,3);
                string periodoDesde = sigloanoperiodoDesde.Substring(3, 2);
                string sigloanoHasta = sigloanoperiodoHasta.Substring(0, 3);
                string periodoHasta = sigloanoperiodoHasta.Substring(3, 2);
                
                decimal nfrcDesde;
                nfrcDesde = System.Math.Ceiling(System.Math.Truncate(Convert.ToDecimal(periodoDesde) / 15))+1;

                decimal nfrcHasta;
                nfrcHasta = System.Math.Ceiling(System.Math.Truncate(Convert.ToDecimal(periodoHasta) / 15))+1;

                decimal noColumnaInicio = Convert.ToDecimal(periodoDesde) % 15;
                decimal noColumnaFin = Convert.ToDecimal(periodoHasta) % 15;

                decimal sigloanoNFRCHInicio = Convert.ToDecimal(sigloanoDesde + nfrcDesde.ToString());
                decimal sigloanoNFRCHFin = Convert.ToDecimal(sigloanoHasta + nfrcHasta.ToString());

                decimal sigloanoNFRCHActual;
                decimal saldoDebe = 0;
                decimal saldoHaber = 0;

                int columnaInicio;
                int columnaFin;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "PRH01 ";
                query += "where TIREH1='" + tipoDato + "' and ";
                query += "CCIAH1='" + codigoCompania + "' and ";
                if (plan != "") query += "TIPLH1='" + plan + "' and ";
                query += "CUENH1 like '" + cuenta + "%' and ";
                if (taux == "" && caux == "") query += "TAUXH1=' ' and CAUXH1 =' ' and ";
                else query += "TAUXH1='" + taux + "' and CAUXH1 ='" + caux + "' and ";
                query += "SANCH1>=" + sigloanoDesde + " and SANCH1<=" + sigloanoHasta + " ";
                query += "order by TIREH1, CCIAH1, CUENH1, TAUXH1, CAUXH1, SANCH1, NFRCH1";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    sigloanoNFRCHActual = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("SANCH1")).ToString() + dr.GetValue(dr.GetOrdinal("NFRCH1")).ToString());

                    if (sigloanoNFRCHActual >=  sigloanoNFRCHInicio && sigloanoNFRCHActual <= sigloanoNFRCHFin)
                    {
                        if (sigloanoNFRCHActual  > sigloanoNFRCHInicio) columnaInicio = 1;
	                    else columnaInicio = Convert.ToInt16(noColumnaInicio);

                        if (sigloanoNFRCHActual < sigloanoNFRCHFin) columnaFin = 15;
                        else columnaFin = Convert.ToInt16(noColumnaFin);

                        string indicecolumnaActual;
                        string nombrecolumnaActualDebe;
                        string nombrecolumnaActualHaber;

                        for (int i = columnaInicio; i <= columnaFin; i++)
	                    {
                            indicecolumnaActual = i.ToString().PadLeft(2, '0');
                            nombrecolumnaActualDebe = "MD" + indicecolumnaActual + "H1";
                            nombrecolumnaActualHaber = "MH" + indicecolumnaActual + "H1";
                            saldoDebe += Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(nombrecolumnaActualDebe)).ToString());
                            saldoHaber += Convert.ToDecimal(dr.GetValue(dr.GetOrdinal(nombrecolumnaActualHaber)).ToString());
	                    }
                    }
                }

                dr.Close();

                saldo[0] = saldoDebe;
                saldo[1] = saldoHaber;
                saldo[2] = saldoDebe + saldoHaber;
            }
            catch(Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return saldo;
        }

        /// <summary>
        /// Devuelve el saldo de los movimientos de GLB01 anteriores a la fecha de inicio y que están dentro del período
        /// posición    1 -> saldo debe
        ///             2 -> saldo haber
        ///             3 -> saldo debe + haber
        /// </summary>
        /// <param name="codigoCompania">código de la compañía</param>
        /// <param name="sapr">siglo año período</param>
        /// <param name="plan">código del plan</param>
        /// <param name="cuenta">código de la cuenta</param>
        /// <param name="fecha">fecha en formato CG</param>
        /// <returns></returns>
        public decimal[] ObtenerSaldoFechaAnteriorFechaInicioPeriodo(string codigoCompania, string sapr, string cuenta, string plan, string fecha)
        {
            decimal[] saldo = { -1, -1, -1 };
            IDataReader dr = null;

            try
            {
                string tmovdt = "";
                decimal saldoDebe = 0;
                decimal saldoHaber = 0;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where STATDT = 'E' and CCIADT='" + codigoCompania + "' and ";
                query += "TIPLDT = '" + plan + "' and ";
                query += "CUENDT = '" + cuenta + "' and ";
                query += "SAPRDT = " + sapr + " and ";
                query += "FECODT < " + fecha;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    tmovdt = dr["TMOVDT"].ToString();
                    switch (tmovdt)
                    {
                        case "D":
                            try
                            {
                                saldoDebe += Convert.ToDecimal(dr["MONTDT"].ToString());
                            }
                            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                            break;
                        case "H":
                            try
                            {
                                saldoHaber += Convert.ToDecimal(dr["MONTDT"].ToString());
                            }
                            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                            break;
                    }
                }

                dr.Close();

                saldo[0] = saldoDebe;
                saldo[1] = saldoHaber;
                saldo[2] = saldoDebe + saldoHaber;
            }
            catch (Exception ex) 
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (saldo);
        }

        /// <summary>
        /// Verifica si existe una tabla en la bbdd
        /// </summary>
        /// <param name="nombreTabla"></param>
        /// <returns></returns>
        public bool ExisteTabla(string tipoBaseDatosCG, string nombreTabla)
        {
            bool result = true;

            try
            {
                string query = "";
                switch (tipoBaseDatosCG)
                {
                    case "DB2":
                        //query = "select count(*) from SYSIBM.SYSTABLES where NAME='" + nombreTabla + "'";
                        query = "select count(*) from " + nombreTabla;
                        break;
                    case "SQLServer":
                        //query = "select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME = '" + nombreTabla + "'";
                        query = "select count(*) from " + GlobalVar.PrefijoTablaCG + nombreTabla;
                        break;
                    case "Oracle":
                        //query = "select count(*) from user_tables where table_name = '" + nombreTabla + "'";
                        query = "select count(*) from " + GlobalVar.PrefijoTablaCG + nombreTabla;
                        break;
                }

                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (registros >= 0)
                {
                    result = true;
                }


                /*
                 ----- SQLSERVER -----
              
                 string sCmd =
                 "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " +
                 "WHERE TABLE_TYPE = 'BASE TABLE' " +
                 "AND TABLE_NAME = @nombreTabla";
        
                 // Comprobamos si está
                 // Devuelve 1 si ya existe
                 // o 0 si no existe
                 int n = (int)cmd.ExecuteScalar();

                 con.Close();

                 return n > 0;
            
                 ----  DB2 -----
                 SELECT count(*) FROM SYSIBM.SYSTABLES WHERE NAME='NOMBRE DE LA TABLA'
                
                 ----  Oracle ----
                 select count(*) into c from user_tables where table_name = upper('table_name');
                */
            }
            catch (Exception ex)
            {
                result = false;
                
                GlobalVar.Log.Error(ex.Message);
                
                //throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Verifica si la clase de zona es jerárquica
        /// </summary>
        /// <param name="claseZona"></param>
        /// <returns></returns>
        public bool isClaseZonaJerarquica(string claseZona)
        {
            bool claseZonaJerarquica = false;

            IDataReader dr = null;
            try
            {
                string query = "select ESTRZ0 from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + claseZona + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    string estrz0 = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString();

                    if (estrz0.Length == 4)
                        if (estrz0.Substring(0, 1) != "0" &&
                            estrz0.Substring(1, 1) == "0" && estrz0.Substring(2, 1) == "0" && estrz0.Substring(3, 1) == "0") claseZonaJerarquica = false;
                        else claseZonaJerarquica = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
            }

            return (claseZonaJerarquica);
        }

        /// <summary>
        /// Devuelve el campo descripcion de una tabla, dado el campo codigo
        /// </summary>
        /// <param name="tabla">Nombre de la tabla</param>
        /// <param name="campoCodigo">Nombre del campo codigo de la tabla</param>
        /// <param name="campoDesc">Nombre del campo descripcion de la tabla</param>
        /// <param name="codigo">Valor del código para hacer la búsqueda del registro</param>
        /// <param name="campoCodigoNumerico">true -> si el valor del campo código es numérico    false -> si es un campo alfanumérico</param>
        /// <param name="filtro">si existen filtros para el where</param>
        /// <returns></returns>
        public string ObtenerDescDadoCodigo(string tabla, string campoCodigo, string campoDesc, string codigo, bool campoCodigoNumerico, string filtro)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select " + campoDesc + " from " + GlobalVar.PrefijoTablaCG + tabla;

                if (campoCodigoNumerico) query += " where " + campoCodigo + " = " + codigo + "";
                else query += " where " + campoCodigo + " = '" + codigo + "' ";

                if (filtro != "") query += filtro;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(0).ToString();
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

        /// <summary>
        /// Dado una cuenta devuelve todas sus cuentas padres
        /// </summary>
        /// <param name="codigoCuenta">código de la cuenta</param>
        /// <param name="plan">código plan</param>
        /// <returns></returns>
        public string[] ObtenerCuentasPadres(string codigoCuenta, string plan, ref string errorMsg)
        {
            errorMsg = "";
            string[] cuentas = { "", "", "", "", "", "", "", "", "" };
            try
            {
                //Buscar el nivel, la longitud máxima y las longitudes dado un plan de cuentas
                int[] result = ObtenerNivelLongitudesDadoPlan(plan);

                if (codigoCuenta.Length <= result[1])
                {
                    int nivel = 0;
                    int longNivel = 0;
                    int longNivelSum = 0;
                    string cuentaParcial = "";
                    if (result[0] != -1)
                    {
                        for (int i = 0; i < result[0]; i++)
                        {
                            nivel++;
                            if (result[i + 2] != 0 && result[i + 2] != -1)
                            {
                                longNivel = result[i + 2];
                                longNivelSum += longNivel;
                                if (longNivelSum <= codigoCuenta.Length)
                                {
                                    cuentaParcial = codigoCuenta.Substring(0, longNivelSum);
                                    cuentas[i] = cuentaParcial;
                                }
                                else break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                errorMsg = "Error obteniendo las cuentas padres de la cuenta actual (" + ex.Message + ")";      //Falta traducir
            }
            return (cuentas);
        }

        /// <summary>
        /// Devuelve un DataTable con la estructura de todas la cuentas padres de esta cuenta
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public DataTable ObtenerEstructuraCuenta(string codigoCuenta, string plan, ref string errorMsg)
        {
            errorMsg = "";
            DataTable dtEstructura = new DataTable
            {
                TableName = "Estructura"
            };

            IDataReader dr = null;
            try
            {
                //Dado una cuenta devuelve todas sus cuentas padres
                string[] cuentas = this.ObtenerCuentasPadres(codigoCuenta, plan, ref errorMsg);

                if (errorMsg != "") return (dtEstructura);

                string filtro = "";
                for (int i = 0; i < cuentas.Length; i++)
                {
                    if (cuentas[i].Trim() != "")
                    {
                        if (i != 0) filtro += ", ";
                        filtro += "'" + cuentas[i] + "'";
                    }
                }

                if (filtro != "")
                {
                    string query = "select CUENMC, TCUEMC, NIVEMC, NOABMC, NOLAAD, CEDTMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC ='" + plan + "' and CUENMC in (" + filtro + ") ";
                    query += "order by NIVEMC";

                    dtEstructura = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch(Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();

                errorMsg = "Error obteniendo las structura de la cuenta (" + ex.Message + ")";      //Falta traducir
            }

            return (dtEstructura);
        }

        /// <summary>
        /// Devuelve un DataTable con la cuenta a último nivel
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public DataTable ObtenerCuentaUltimoNivel(string codigoCuenta, string plan, ref string errorMsg)
        {
            errorMsg = "";
            DataTable dtCtasUltimoNivel = new DataTable
            {
                TableName = "CuentaUltNivel"
            };

            IDataReader dr = null;
            try
            {
                codigoCuenta = codigoCuenta.Trim();

                string query = "select STATMC, max(CUENMC) CUENMC, TCUEMC, max(NOLAAD) NOLAAD, max(CEDTMC) CEDTMC from ";
                query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC='" + plan + "' and CUENMC like '" + codigoCuenta + "%' ";
                query += "group by STATMC, TCUEMC, CEDTMC ";
                query += "order by CEDTMC";

                dtCtasUltimoNivel = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();

                errorMsg = "Error obteniendo las cuentas a último nivel (" + ex.Message + ")";      //Falta traducir
            }

            return (dtCtasUltimoNivel);
        }

        /// <summary>
        /// Devuelve un DataTable con la cuenta a último nivel con todos los datos necesarios para pasar la cuenta de Titulo a Detalle
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public DataTable ObtenerCuentaUltimoNivelValoresCampos(string codigoCuenta, string plan, ref string errorMsg)
        {
            errorMsg = "";
            DataTable dtCtasUltimoNivel = new DataTable
            {
                TableName = "CuentaUltNivel"
            };

            IDataReader dr = null;
            try
            {
                codigoCuenta = codigoCuenta.Trim();

                string query = "select STATMC, max(CUENMC) CUENMC, TCUEMC, max(NOLAAD) NOLAAD, max(CEDTMC) CEDTMC, ";
                query += "ADICMC, FEVEMC, NDDOMC, TERMMC, TIMOMC, TAU1MC, TAU2MC, TAU3MC, TDOCMC, GRUPMC, DEAUMC, RNITMC, CNITMC, MASCMC from ";
                query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC='" + plan + "' and CUENMC like '" + codigoCuenta + "%' ";
                query += "group by STATMC, TCUEMC, CEDTMC, ADICMC, FEVEMC, NDDOMC, TERMMC, TIMOMC, TAU1MC, TAU2MC, TAU3MC, TDOCMC, GRUPMC, DEAUMC, RNITMC, CNITMC, MASCMC ";
                query += "order by CEDTMC";

                dtCtasUltimoNivel = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();

                errorMsg = "Error obteniendo las cuentas a último nivel (" + ex.Message + ")";      //Falta traducir
            }

            return (dtCtasUltimoNivel);
        }

        /// <summary>
        /// Devuelve el código de la cuenta a último nivel
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public string ObtenerCodigoCuentaUltimoNivel(string codigoCuenta, string plan, ref string errorMsg)
        {
            string codCtaUltNivel = "";
            try
            {
                int fila = 0;
                errorMsg = "";
                
                DataTable dtCtasUltimoNivel = this.ObtenerCuentaUltimoNivelValoresCampos(codigoCuenta, plan, ref errorMsg);

                if (errorMsg == "" && (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0))
                {
                    fila = dtCtasUltimoNivel.Rows.Count - 1;
                    codCtaUltNivel = dtCtasUltimoNivel.Rows[fila]["CUENMC"].ToString();
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (codCtaUltNivel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <param name="errorMsg"></param>
        /// <param name="datosPlan"></param>
        /// <param name="dtEstructuraPadreCuenta"></param>
        /// <returns></returns>
        public bool CuentaValidaEnPlanCuentas(string codigoCuenta, string plan, ref string errorMsg, int[] datosPlan, ref string[] estructuraPadreCuentas)
        {
            bool result = false;
            string[] cuentas = { "", "", "", "", "", "", "", "", "", "" };

            try
            {
                //Buscar el nivel, la longitud máxima y las longitudes dado un plan de cuentas
                if (datosPlan == null) datosPlan = ObtenerNivelLongitudesDadoPlan(plan);

                if (codigoCuenta.Length > datosPlan[1]) return (result); //Cuenta mayor que la longitud del plan

                int nivel = 0;
                int longNivel = 0;
                int longNivelSum = 0;
                string cuentaParcial = "";
                if (datosPlan[0] != -1)
                {
                    for (int i = 0; i < datosPlan[0]; i++)
                    {
                        nivel++;
                        if (datosPlan[i + 2] != 0 && datosPlan[i + 2] != -1)
                        {
                            longNivel = datosPlan[i + 2];
                            longNivelSum += longNivel;
                            if (longNivelSum <= codigoCuenta.Length)
                            {
                                cuentaParcial = codigoCuenta.Substring(0, longNivelSum);
                                if (cuentaParcial == codigoCuenta) result = true;
                                cuentas[i + 1] = cuentaParcial;
                            }
                            else
                            {
                                nivel--; ///SMR restar 1
                                break;
                            }
                        }
                    }
                }

                if (codigoCuenta.Length == 1) cuentas[0] = "1";
                else cuentas[0] = nivel.ToString();
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            estructuraPadreCuentas = cuentas;
            return (result);
        }

        /// <summary>
        /// Dado una cuenta la devuelve formateada
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public string CuentaFormatear(string codigoCuenta, string plan, string estructura, string mascaraEdicion, ref string errorMsg, ref int nivel)
        {
            codigoCuenta = codigoCuenta.Trim();
            string cuentaFormateada = "";
            errorMsg = "";
            nivel = 0;

            //SMR - quitar los ceros a la derecha de la estructura
            estructura = estructura.Replace("0", "");
            //SMR - quitar los blancos a la derecha de la mascara
            mascaraEdicion = mascaraEdicion.Replace(" ", "");

            try
            {
                if (estructura.Length == 0 || mascaraEdicion.Length == 0)
                {
                    //Recuperar estructura y mascara de la tabla con el parámetro plan   FALTA  !!!!
                    errorMsg = "La estructura o la máscara de edición no es correcta";  //Falta traducir
                    return (cuentaFormateada);
                }

                if (mascaraEdicion.Length + 1 != estructura.Length)
                {
                    errorMsg = "La máscara de edición no es correcta";  //Falta traducir
                    return (cuentaFormateada);
                }

                int longNivel = 0;
                int longNivelSum = 0;
                string mascara = "";

                for (int i = 0; i <= estructura.Length - 1; i++)
                {
                    nivel++;
                    longNivel = Convert.ToInt16(estructura[i].ToString());

                    if (i != 0) mascara = mascaraEdicion[i - 1].ToString();

                    if (longNivel == 0) break;

                    if (longNivelSum <= codigoCuenta.Length)
                    {
                        if (i != 0)
                        {
                            if (codigoCuenta.Length >= longNivelSum + longNivel) cuentaFormateada += mascara + codigoCuenta.Substring(longNivelSum, longNivel);
                            else
                            {
                                nivel--; // SMR restar 1
                                break;
                            }
                        }
                        else cuentaFormateada += codigoCuenta.Substring(0, longNivel);
                    }
                    else
                    {
                        nivel--; // SMR restar 1
                        break;
                    }

                    longNivelSum += longNivel;
                }

            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                errorMsg = "Error formateando la cuenta (" + ex.Message + ")";      //Falta traducir
            }

            return (cuentaFormateada);
        }

        /// <summary>
        /// Dado una cuenta, completa la cuenta según los niveles
        /// posicion 0  -> cantidad de cuentas
        ///          1  -> nivel de la 1ra cuenta
        /// </summary>
        /// <param name="codigoCuenta"></param>
        /// <param name="plan"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public string[] CuentaCompletarNiveles(string codigoCuenta, string plan, string estructura, ref string errorMsg)
        {
            string[] cuentasACompletar = {"0", "", "", "", "", "", "", "", ""};
            errorMsg = "";
            try
            {
                if (estructura.Length == 0)
                {
                    //Recuperar estructura y mascara de la tabla con el parámetro plan   FALTA  !!!!
                    errorMsg = "La estructura o la máscara de edición no es correcta";  //Falta traducir
                    return (cuentasACompletar);
                }

                int longNivel = 0;
                int longNivelSum = 0;
                string cuentaAux = codigoCuenta;
                int cantCuentas = 0;
                int nivel = 0;
                string aux = "";
                for (int i = 0; i < estructura.Length - 1; i++)
                {
                    nivel++;
                    longNivel = Convert.ToInt16(estructura[i].ToString());

                    if (longNivel == 0) break;

                    aux = "";

                    if (longNivelSum >= codigoCuenta.Length)
                    {
                        if (cuentaAux == codigoCuenta) cuentasACompletar[cantCuentas + 1] = nivel.ToString();
                        aux = aux.PadRight(longNivel, '0');
                        cuentaAux += aux;
                        cuentasACompletar[cantCuentas + 2] = cuentaAux;
                        cantCuentas++;
                    }

                    longNivelSum += longNivel;
                }

                cuentasACompletar[0] = cantCuentas.ToString();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                errorMsg = "Error formateando la cuenta (" + ex.Message + ")";      //Falta traducir
            }

            return (cuentasACompletar);
        }

        /// <summary>
        /// Devuelve los tipos de auxiliares para la cuenta solicitada
        /// posicion 0  -> tipo de auxiliar 1
        ///          1  -> tipo de auxiliar 2
        ///          2  -> tipo de auxiliar 3
        /// </summary>
        /// <param name="plan">código del plan de cuentas de la compañía</param>
        /// <param name="plan">código de la cuenta de mayor</param>
        /// <returns></returns>
        public string[] ObtenerTiposAuxiliarCuentaMayor(string plan, string cuenta)
        {
            //string[] result = new string[3];
            string[] result = new string[] { "", "", "" };
            IDataReader dr = null;
            try
            {
                string query = " select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC ='" + plan + "' and CUENMC = '" + cuenta + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result[0] = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString();
                    result[1] = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString();
                    result[2] = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString();
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
 

        /// <summary>
        /// Verifica si la compañía utiliza los campos extendidos
        /// </summary>
        /// <returns></returns>
        public bool PlanCamposExtendidos(string codPlan, string tipoBaseDatosCG, ref DataTable dtGLMX2)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Verificar que exista la tabla GLMX2
                bool existeTabla = this.ExisteTabla(tipoBaseDatosCG, "GLMX2");

                if (!existeTabla) return (result);

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                query += "where TIPLPX = '" + codPlan + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    string FGPRPX = dr.GetValue(dr.GetOrdinal("FGPRPX")).ToString();
                    string FGFAPX = dr.GetValue(dr.GetOrdinal("FGFAPX")).ToString();
                    string FGFRPX = dr.GetValue(dr.GetOrdinal("FGFRPX")).ToString();
                    string FGDVPX = dr.GetValue(dr.GetOrdinal("FGDVPX")).ToString();
                    string FG01PX = dr.GetValue(dr.GetOrdinal("FG01PX")).ToString();
                    string FG02PX = dr.GetValue(dr.GetOrdinal("FG02PX")).ToString();
                    string FG03PX = dr.GetValue(dr.GetOrdinal("FG03PX")).ToString();
                    string FG04PX = dr.GetValue(dr.GetOrdinal("FG04PX")).ToString();
                    string FG05PX = dr.GetValue(dr.GetOrdinal("FG05PX")).ToString();
                    string FG06PX = dr.GetValue(dr.GetOrdinal("FG06PX")).ToString();
                    string FG07PX = dr.GetValue(dr.GetOrdinal("FG07PX")).ToString();
                    string FG08PX = dr.GetValue(dr.GetOrdinal("FG08PX")).ToString();
                    string FG09PX = dr.GetValue(dr.GetOrdinal("FG09PX")).ToString();
                    string FG10PX = dr.GetValue(dr.GetOrdinal("FG10PX")).ToString();
                    string FG11PX = dr.GetValue(dr.GetOrdinal("FG11PX")).ToString();
                    string FG12PX = dr.GetValue(dr.GetOrdinal("FG12PX")).ToString();

                    //Chequear que al menos exista una columna visible
                    if (FGPRPX == "0" && FGFAPX == "0" && FGFRPX == "0" && FGDVPX == "0" && FG01PX == "0" && FG01PX == "0" &&
                        FG02PX == "0" && FG03PX == "0" && FG04PX == "0" && FG05PX == "0" && FG06PX == "0" && FG06PX == "0" &&
                        FG07PX == "0" && FG08PX == "0" && FG08PX == "0" && FG09PX == "0" && FG10PX == "0" && FG11PX == "0" &&
                        FG12PX == "0")
                    {
                        dr.Close();
                        return (result);
                    }

                    if (dtGLMX2 != null && dtGLMX2.Rows.Count > 0) dtGLMX2.Clear();
                    dtGLMX2 = new DataTable
                    {
                        TableName = "GLMX2"
                    };

                    dtGLMX2.Columns.Add("FGPRPX", typeof(string));
                    dtGLMX2.Columns.Add("PGPRPX", typeof(string));
                    dtGLMX2.Columns.Add("FGFAPX", typeof(string));
                    dtGLMX2.Columns.Add("PGFAPX", typeof(string));
                    dtGLMX2.Columns.Add("FGFRPX", typeof(string));
                    dtGLMX2.Columns.Add("PGFRPX", typeof(string));
                    dtGLMX2.Columns.Add("FGDVPX", typeof(string));
                    dtGLMX2.Columns.Add("PGDVPX", typeof(string));
                    dtGLMX2.Columns.Add("NM01PX", typeof(string));
                    dtGLMX2.Columns.Add("MX01PX", typeof(string));
                    dtGLMX2.Columns.Add("PG01PX", typeof(string));
                    dtGLMX2.Columns.Add("TA01PX", typeof(string));
                    dtGLMX2.Columns.Add("FG01PX", typeof(string));
                    dtGLMX2.Columns.Add("NM02PX", typeof(string));
                    dtGLMX2.Columns.Add("MX02PX", typeof(string));
                    dtGLMX2.Columns.Add("PG02PX", typeof(string));
                    dtGLMX2.Columns.Add("TA02PX", typeof(string));
                    dtGLMX2.Columns.Add("FG02PX", typeof(string));
                    dtGLMX2.Columns.Add("NM03PX", typeof(string));
                    dtGLMX2.Columns.Add("MX03PX", typeof(string));
                    dtGLMX2.Columns.Add("PG03PX", typeof(string));
                    dtGLMX2.Columns.Add("TA03PX", typeof(string));
                    dtGLMX2.Columns.Add("FG03PX", typeof(string));
                    dtGLMX2.Columns.Add("NM04PX", typeof(string));
                    dtGLMX2.Columns.Add("MX04PX", typeof(string));
                    dtGLMX2.Columns.Add("PG04PX", typeof(string));
                    dtGLMX2.Columns.Add("TA04PX", typeof(string));
                    dtGLMX2.Columns.Add("FG04PX", typeof(string));
                    dtGLMX2.Columns.Add("NM05PX", typeof(string));
                    dtGLMX2.Columns.Add("MX05PX", typeof(string));
                    dtGLMX2.Columns.Add("PG05PX", typeof(string));
                    dtGLMX2.Columns.Add("TA05PX", typeof(string));
                    dtGLMX2.Columns.Add("FG05PX", typeof(string));
                    dtGLMX2.Columns.Add("NM06PX", typeof(string));
                    dtGLMX2.Columns.Add("MX06PX", typeof(string));
                    dtGLMX2.Columns.Add("PG06PX", typeof(string));
                    dtGLMX2.Columns.Add("TA06PX", typeof(string));
                    dtGLMX2.Columns.Add("FG06PX", typeof(string));
                    dtGLMX2.Columns.Add("NM07PX", typeof(string));
                    dtGLMX2.Columns.Add("MX07PX", typeof(string));
                    dtGLMX2.Columns.Add("PG07PX", typeof(string));
                    dtGLMX2.Columns.Add("TA07PX", typeof(string));
                    dtGLMX2.Columns.Add("FG07PX", typeof(string));
                    dtGLMX2.Columns.Add("NM08PX", typeof(string));
                    dtGLMX2.Columns.Add("MX08PX", typeof(string));
                    dtGLMX2.Columns.Add("PG08PX", typeof(string));
                    dtGLMX2.Columns.Add("TA08PX", typeof(string));
                    dtGLMX2.Columns.Add("FG08PX", typeof(string));
                    dtGLMX2.Columns.Add("NM09PX", typeof(string));
                    dtGLMX2.Columns.Add("PG09PX", typeof(string));
                    dtGLMX2.Columns.Add("FG09PX", typeof(string));
                    dtGLMX2.Columns.Add("NM10PX", typeof(string));
                    dtGLMX2.Columns.Add("PG10PX", typeof(string));
                    dtGLMX2.Columns.Add("FG10PX", typeof(string));
                    dtGLMX2.Columns.Add("NM11PX", typeof(string));
                    dtGLMX2.Columns.Add("PG11PX", typeof(string));
                    dtGLMX2.Columns.Add("FG11PX", typeof(string));
                    dtGLMX2.Columns.Add("NM12PX", typeof(string));
                    dtGLMX2.Columns.Add("PG12PX", typeof(string));
                    dtGLMX2.Columns.Add("FG12PX", typeof(string));

                    DataRow row = dtGLMX2.NewRow();

                    row["FGPRPX"] = FGPRPX;
                    row["PGPRPX"] = dr.GetValue(dr.GetOrdinal("PGPRPX")).ToString();
                    row["FGFAPX"] = FGFAPX;
                    row["PGFAPX"] = dr.GetValue(dr.GetOrdinal("PGFAPX")).ToString();
                    row["FGFRPX"] = FGFRPX;
                    row["PGFRPX"] = dr.GetValue(dr.GetOrdinal("PGFRPX")).ToString();
                    row["FGDVPX"] = FGDVPX;
                    row["PGDVPX"] = dr.GetValue(dr.GetOrdinal("PGDVPX")).ToString();
                    row["NM01PX"] = dr.GetValue(dr.GetOrdinal("NM01PX")).ToString();
                    row["MX01PX"] = dr.GetValue(dr.GetOrdinal("MX01PX")).ToString();
                    row["PG01PX"] = dr.GetValue(dr.GetOrdinal("PG01PX")).ToString();
                    row["TA01PX"] = dr.GetValue(dr.GetOrdinal("TA01PX")).ToString();
                    row["FG01PX"] = FG01PX;
                    row["NM02PX"] = dr.GetValue(dr.GetOrdinal("NM02PX")).ToString();
                    row["MX02PX"] = dr.GetValue(dr.GetOrdinal("MX02PX")).ToString();
                    row["PG02PX"] = dr.GetValue(dr.GetOrdinal("PG02PX")).ToString();
                    row["TA02PX"] = dr.GetValue(dr.GetOrdinal("TA02PX")).ToString();
                    row["FG02PX"] = FG02PX;
                    row["NM03PX"] = dr.GetValue(dr.GetOrdinal("NM03PX")).ToString();
                    row["MX03PX"] = dr.GetValue(dr.GetOrdinal("MX03PX")).ToString();
                    row["PG03PX"] = dr.GetValue(dr.GetOrdinal("PG03PX")).ToString();
                    row["TA03PX"] = dr.GetValue(dr.GetOrdinal("TA03PX")).ToString();
                    row["FG03PX"] = FG03PX;
                    row["NM04PX"] = dr.GetValue(dr.GetOrdinal("NM04PX")).ToString();
                    row["MX04PX"] = dr.GetValue(dr.GetOrdinal("MX04PX")).ToString();
                    row["PG04PX"] = dr.GetValue(dr.GetOrdinal("PG04PX")).ToString();
                    row["TA04PX"] = dr.GetValue(dr.GetOrdinal("TA04PX")).ToString();
                    row["FG04PX"] = FG04PX;
                    row["NM05PX"] = dr.GetValue(dr.GetOrdinal("NM05PX")).ToString();
                    row["MX05PX"] = dr.GetValue(dr.GetOrdinal("MX05PX")).ToString();
                    row["PG05PX"] = dr.GetValue(dr.GetOrdinal("PG05PX")).ToString();
                    row["TA05PX"] = dr.GetValue(dr.GetOrdinal("TA05PX")).ToString();
                    row["FG05PX"] = FG05PX;
                    row["NM06PX"] = dr.GetValue(dr.GetOrdinal("NM06PX")).ToString();
                    row["MX06PX"] = dr.GetValue(dr.GetOrdinal("MX06PX")).ToString();
                    row["PG06PX"] = dr.GetValue(dr.GetOrdinal("PG06PX")).ToString();
                    row["TA06PX"] = dr.GetValue(dr.GetOrdinal("TA06PX")).ToString();
                    row["FG06PX"] = FG06PX;
                    row["NM07PX"] = dr.GetValue(dr.GetOrdinal("NM07PX")).ToString();
                    row["MX07PX"] = dr.GetValue(dr.GetOrdinal("MX07PX")).ToString();
                    row["PG07PX"] = dr.GetValue(dr.GetOrdinal("PG07PX")).ToString();
                    row["TA07PX"] = dr.GetValue(dr.GetOrdinal("TA07PX")).ToString();
                    row["FG07PX"] = FG07PX;
                    row["NM08PX"] = dr.GetValue(dr.GetOrdinal("NM08PX")).ToString();
                    row["MX08PX"] = dr.GetValue(dr.GetOrdinal("MX08PX")).ToString();
                    row["PG08PX"] = dr.GetValue(dr.GetOrdinal("PG08PX")).ToString();
                    row["TA08PX"] = dr.GetValue(dr.GetOrdinal("TA08PX")).ToString();
                    row["FG08PX"] = FG08PX;
                    row["NM09PX"] = dr.GetValue(dr.GetOrdinal("NM09PX")).ToString();
                    row["PG09PX"] = dr.GetValue(dr.GetOrdinal("PG09PX")).ToString();
                    row["FG09PX"] = FG09PX;
                    row["NM10PX"] = dr.GetValue(dr.GetOrdinal("NM10PX")).ToString();
                    row["PG10PX"] = dr.GetValue(dr.GetOrdinal("PG10PX")).ToString();
                    row["FG10PX"] = FG10PX;
                    row["NM11PX"] = dr.GetValue(dr.GetOrdinal("NM11PX")).ToString();
                    row["PG11PX"] = dr.GetValue(dr.GetOrdinal("PG11PX")).ToString();
                    row["FG11PX"] = FG11PX;
                    row["NM12PX"] = dr.GetValue(dr.GetOrdinal("NM12PX")).ToString();
                    row["PG12PX"] = dr.GetValue(dr.GetOrdinal("PG12PX")).ToString();
                    row["FG12PX"] = FG12PX;

                    dtGLMX2.Rows.Add(row);

                    result = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
                if (dr != null) dr.Close();
            }
            return (result);
        }

        /// <summary>
        /// Devolver los valores de la tabla GLMX3
        /// </summary>
        /// <returns></returns>
        public bool CamposExtendidosGLMX3(string codPlan, string tipoBaseDatosCG, string cuenta, ref DataTable dtGLMX3)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Verificar que exista la tabla GLMX3
                bool existeTabla = this.ExisteTabla(tipoBaseDatosCG, "GLMX3");

                if (!existeTabla) return (result);

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                query += "where TIPLMX = '" + codPlan + "' and CUENMX =' " + cuenta + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    if (dtGLMX3 != null && dtGLMX3.Rows.Count > 0) dtGLMX3.Clear();
                    dtGLMX3 = new DataTable
                    {
                        TableName = "GLMX3"
                    };

                    dtGLMX3.Columns.Add("FGPRMX", typeof(string));
                    dtGLMX3.Columns.Add("FGFAMX", typeof(string));
                    dtGLMX3.Columns.Add("FGFRMX", typeof(string));
                    dtGLMX3.Columns.Add("FGDVMX", typeof(string));
                    dtGLMX3.Columns.Add("FG01MX", typeof(string));
                    dtGLMX3.Columns.Add("FG02MX", typeof(string));
                    dtGLMX3.Columns.Add("FG03MX", typeof(string));
                    dtGLMX3.Columns.Add("FG04MX", typeof(string));
                    dtGLMX3.Columns.Add("FG05MX", typeof(string));
                    dtGLMX3.Columns.Add("FG06MX", typeof(string));
                    dtGLMX3.Columns.Add("FG07MX", typeof(string));
                    dtGLMX3.Columns.Add("FG08MX", typeof(string));
                    dtGLMX3.Columns.Add("FG09MX", typeof(string));
                    dtGLMX3.Columns.Add("FG10MX", typeof(string));
                    dtGLMX3.Columns.Add("FG11MX", typeof(string));
                    dtGLMX3.Columns.Add("FG12MX", typeof(string));

                    DataRow row = dtGLMX3.NewRow();

                    row["FGPRPX"] = dr.GetValue(dr.GetOrdinal("FGPRMX")).ToString();
                    row["FGFAMX"] = dr.GetValue(dr.GetOrdinal("FGFAMX")).ToString();
                    row["FGFRMX"] = dr.GetValue(dr.GetOrdinal("FGFRMX")).ToString();
                    row["FGDVMX"] = dr.GetValue(dr.GetOrdinal("FGDVMX")).ToString();
                    row["FG01MX"] = dr.GetValue(dr.GetOrdinal("FG01MX")).ToString();
                    row["FG02MX"] = dr.GetValue(dr.GetOrdinal("FG02MX")).ToString();
                    row["FG03MX"] = dr.GetValue(dr.GetOrdinal("FG03MX")).ToString();
                    row["FG04MX"] = dr.GetValue(dr.GetOrdinal("FG04MX")).ToString();
                    row["FG05MX"] = dr.GetValue(dr.GetOrdinal("FG05MX")).ToString();
                    row["FG06MX"] = dr.GetValue(dr.GetOrdinal("FG06MX")).ToString();
                    row["FG07MX"] = dr.GetValue(dr.GetOrdinal("FG07MX")).ToString();
                    row["FG08MX"] = dr.GetValue(dr.GetOrdinal("FG08MX")).ToString();
                    row["FG09MX"] = dr.GetValue(dr.GetOrdinal("FG09MX")).ToString();
                    row["FG10MX"] = dr.GetValue(dr.GetOrdinal("FG10MX")).ToString();
                    row["FG11MX"] = dr.GetValue(dr.GetOrdinal("FG11MX")).ToString();
                    row["FG12MX"] = dr.GetValue(dr.GetOrdinal("FG12MX")).ToString();

                    dtGLMX3.Rows.Add(row);

                    result = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
                if (dr != null) dr.Close();
            }
            return (result);
        }
    }
}