using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModel
{
    public static class CGParametrosGrles
    {
        /// <summary>
        /// Status del sistema
        /// * - bloqueado
        /// </summary>
        static string _GLC01_STSIRC;
        public static string GLC01_STSIRC
        {
            get
            {
                return (_GLC01_STSIRC);
            }
            set
            {
                _GLC01_STSIRC = value;
            }
        }


        //Permitir entrar o no lo valores en minúsculas 
        //"0" no deja entrar minúsculas
        //"1" si deja entrar minúsculas 
        static string _GLC01_MCIARC;
        public static string GLC01_MCIARC
        {
            get
            {
                return (_GLC01_MCIARC);
            }
            set
            {
                _GLC01_MCIARC = value;
            }
        }

        //Formato de las fechas
        //"D" ->  dd/mm/yy
        //"M" ->  mm/dd/yy
        //"Y" ->  yy/mm/dd
        static string _GLC01_FECHRC;
        public static string GLC01_FECHRC
        {
            get
            {
                return (_GLC01_FECHRC);
            }
            set
            {
                _GLC01_FECHRC = value;
            }
        }


        //Año de corte
        // >40 ->  siglo 0
        static string _GLC01_ALSIRC;
        public static string GLC01_ALSIRC
        {
            get
            {
                return (_GLC01_ALSIRC);
            }
            set
            {
                _GLC01_ALSIRC = value;
            }
        }

        //Verificar CIF/DNI
        //"0" -> No
        //"1" -> Si
        static string _GLC01_CHKCRC;
        public static string GLC01_CHKCRC
        {
            get
            {
                return (_GLC01_CHKCRC);
            }
            set
            {
                _GLC01_CHKCRC = value;
            }
        }

        //Separador del Punto decimal
        //"C" -> Coma
        //"P" -> Punto
        static string _GLC01_EDITRC;
        public static string GLC01_EDITRC
        {
            get
            {
                return (_GLC01_EDITRC);
            }
            set
            {
                _GLC01_EDITRC = value;
            }
        }

        //Si utilizan usuarios de referencia
        //"0" -> No
        //"1" -> Si
        static string _GLC01_REFURC;
        public static string GLC01_REFURC
        {
            get
            {
                return (_GLC01_REFURC);
            }
            set
            {
                _GLC01_REFURC = value;
            }
        }

        //Pedir el password cuando
        //"N" -> cuando entras a la aplicación (solo la 1ra vez)
        //"U" -> cada vez que se accede a una opción solicita user/pwd
        //"P" -> mantiene user y pide pwd
        static string _GLC01_PASWRC;
        public static string GLC01_PASWRC
        {
            get
            {
                return (_GLC01_PASWRC);
            }
            set
            {
                _GLC01_PASWRC = value;
            }
        }

        //Numero de separadores de miles en la edición de importes  (0 ... 4)
        static string _GLC01_CANTRC;
        public static string GLC01_CANTRC
        {
            get
            {
                return (_GLC01_CANTRC);
            }
            set
            {
                _GLC01_CANTRC = value;
            }
        }


        public static void CargarParametrosGenerales(ProveedorDatos proveedorDatos)
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from ";
                query += GlobalVar.PrefijoTablaCG + "GLC01 ";

                dr = proveedorDatos.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    _GLC01_STSIRC = dr["STSIRC"].ToString().Trim();
                    _GLC01_MCIARC = dr["MCIARC"].ToString().Trim();
                    _GLC01_FECHRC = dr["FECHRC"].ToString().Trim();
                    _GLC01_ALSIRC = dr["ALSIRC"].ToString().Trim();
                    _GLC01_CHKCRC = dr["CHKCRC"].ToString().Trim();
                    _GLC01_EDITRC = dr["EDITRC"].ToString().Trim();
                    _GLC01_REFURC = dr["REFURC"].ToString().Trim();
                    _GLC01_PASWRC = dr["PASWRC"].ToString().Trim();
                    _GLC01_CANTRC = dr["CANTRC"].ToString().Trim();
                }

                dr.Close();

                //Inicializar la variable global (CGFormatoFecha) con el formato de las fechas de CG
                switch (_GLC01_FECHRC)
                {
                    case "D":
                        GlobalVar.CGFormatoFecha = "dd/MM/yyyy";
                        break;
                    case "M":
                        GlobalVar.CGFormatoFecha = "MM/dd/yyyy";
                        break;
                    case "Y":
                        GlobalVar.CGFormatoFecha = "yyyy/MM/dd";
                        break;
                    default:
                        GlobalVar.CGFormatoFecha = "dd/MM/yyyy";
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                if (dr != null) dr.Close();
                GlobalVar.CGFormatoFecha = "dd/MM/yyyy";
            }
        }
    }
}
