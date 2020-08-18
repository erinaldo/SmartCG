using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModel
{
    public class ElementoClase
    {
        private string clase;
        private string elemento;
        private string descripcion;
        private string errorMsg;

        public string Clase
        {
            get
            {
                return (this.clase);
            }
            set
            {
                this.clase = value;
            }
        }

        public string Elemento
        {
            get
            {
                return (this.elemento);
            }
            set
            {
                this.elemento = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return (this.descripcion);
            }
        }

        public string ErrorMsg
        {
            get
            {
                return (this.errorMsg);
            }
        }

        public ElementoClase()
        {
            this.Init("", "");
        }

        public ElementoClase(string claseValor, string elementoValor)
        {
            this.Init(claseValor, elementoValor);
        }

        #region Métodos Privados
        /// <summary>
        /// Inicializa el objeto
        /// </summary>
        /// <param name="claseValor"></param>
        /// <param name="elementoValor"></param>
        private void Init(string claseValor, string elementoValor)
        {
            this.clase = claseValor;
            this.elemento = elementoValor;
            this.descripcion = "";
            this.errorMsg = "";
        }

        /// <summary>
        /// Devuelve la descripción del elemento consultandola en la tabla y campos correspondientes
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campoElemento"></param>
        /// <param name="campoDesc"></param>
        /// <param name="campoClave2"></param>
        /// <returns></returns>
        private string ObtenerDescTabla(string tabla, string campoElemento, string campoDesc, string campoClave2)
        {
            string desc = "";
            IDataReader dr = null;

            try
            {
                string query = "";
                if (campoClave2 == null)
                {
                    query = "select " + campoDesc + " from " + GlobalVar.PrefijoTablaCG + tabla;
                    query += " where " + campoElemento + " = '" + this.elemento + "'";
                }
                else
                {
                    string valor = this.elemento.Trim();
                    string valor1 = "";
                    string valor2 = "";

                    switch (tabla)
                    {
                        case "GLT08":
                            if (valor.Length >= 2)
                            {
                                valor1 = valor.Substring(0, 2);
                                if (valor.Length > 2) valor2 = valor.Substring(2, valor.Length-2);
                            }
                            break;
                        case "GLT22":
                            if (valor.Length >= 1)
                            {
                                valor1 = valor.Substring(0, 1);
                                if (valor.Length > 1) valor2 = valor.Substring(1, valor.Length - 1);
                            }
                            break;
                    }

                    query = "select " + campoDesc + " from " + GlobalVar.PrefijoTablaCG + tabla;
                    query += " where " + campoElemento + " = '" + valor1 + "' and ";
                    query += campoClave2 + " = '" + valor2 + "'";
                }

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal(campoDesc)).ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;

                if (dr != null) dr.Close();
            }

            return (desc);
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Devuelve la descripción del elemento dado la clase a la que pertenece
        /// </summary>
        /// <returns></returns>
        public string GetDescripcion()
        {
            string result = "";

            try
            {
                if (this.clase == "")
                {
                    this.errorMsg = "La clase tiene que estar informada";
                    return (result);
                }

                if (this.elemento == "")
                {
                    this.errorMsg = "El elemento tiene que estar informada";
                    return (result);
                }

                switch (this.clase)
                {
                    case "001":
                        result = this.ObtenerDescTabla("GLM07", "GRUPGR", "NOMBGR", null);
                        break;
                    case "002":
                        result = this.ObtenerDescTabla("GLM01", "CCIAMG", "NCIAMG", null);
                        break;
                    case "003":
                        result = this.ObtenerDescTabla("GLM02", "TIPLMP", "NOMBMP", null);
                        break;
                    case "004":
                        result = this.ObtenerDescTabla("GLT06", "TIVOTV", "NOMBTV", null);
                        break;
                    case "005":
                        result = this.ObtenerDescTabla("PRT03", "TDATAH", "NOMBAH", null);
                        break;
                    case "006":
                        result = this.ObtenerDescTabla("GLM04", "TAUXMT", "NOMBMT", null);
                        break;
                    case "007":
                        result = this.ObtenerDescTabla("GLT08", "TAUXGA", "NOMBGA", "GRCTGA");
                        break;
                    case "008":
                        result = this.ObtenerDescTabla("GLT22", "TIPLGC", "NOMBGC", "GRCTGC");
                        break;
                    case "009":
                        result = this.ObtenerDescTabla("EFM04", "TIRETR", "TITUTR", null);
                        break;
                    case "010":
                        result = this.ObtenerDescTabla("PRT02", "CORET2", "NOMBT2", null);
                        break;
                    case "012":
                        result = this.ObtenerDescTabla("EXT02", "IDEXT2", "NOMBT2", null);
                        break;
                    case "013":
                        result = this.ObtenerDescTabla("GLT24", "GRUP24", "NOMB24", null);
                        break;
                    case "014":
                        result = this.ObtenerDescTabla("PRT04", "GRUPP4", "NOMBP4", null);
                        break;
                    case "015":
                        result = this.ObtenerDescTabla("DCM01", "CICLCD", "DESCCD", null);
                        break;
                    case "017":
                        result = this.ObtenerDescTabla("CEM01", "CNFGBB", "NOMBBB", null);
                        break;
                    case "020":
                        result = this.ObtenerDescTabla("G3T01", "CODI3D", "DESC3D", null);
                        break;
                    case "021":
                        result = this.ObtenerDescTabla("G3T02", "CODI3E", "DESC3E", null);
                        break;
                    case "022":
                        result = this.ObtenerDescTabla("G3T70", "CODI3O", "DESC3O", null);
                        break;
                    case "023":
                        result = this.ObtenerDescTabla("G3T41", "CODI3X", "DESC3X", null);
                        break;
                    case "024":
                        result = this.ObtenerDescTabla("GLM10", "CLASZ0", "NOMBZ0", null);
                        break;
                }

                this.descripcion = result;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
            }

            return (result);
        }
        #endregion
    }
}
