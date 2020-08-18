using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectModel
{
    class ClaveGenerar
    {
        int[] modulosIdTodos;
        int[] modulosIdSel;

        public int[] ModulosIdTodos
        {
            get
            {
                return (this.modulosIdTodos);
            }
            set
            {
                this.modulosIdTodos = value;
            }
        }


        public int[] ModulosIdSel
        {
            get
            {
                return (this.modulosIdSel);
            }
            set
            {
                this.modulosIdSel = value;
            }
        }

        public ClaveGenerar()
        {
        }

        public string Generar()
        {
            string clave = "";

            int claveNum = 0;
            int currentID = 1;

            string moduloActual = "";
            bool moduloSeleccionado = false;

            for (int i = 1; i <= this.modulosIdTodos.Length; i++)
            {
                moduloActual = this.ModulosIdTodos[i - 1].ToString();
                moduloSeleccionado = false;

                for (int j = 1; j <= this.modulosIdSel.Length; j++)
                {
                    if (this.modulosIdSel[j - 1].ToString() == moduloActual)
                    {
                        moduloSeleccionado = true;
                        break;
                    }
                }

                if (moduloSeleccionado)
                {
                    claveNum = claveNum | currentID;
                }

                currentID = currentID * 2;
            }
            
            
            int claveDecimal = Convert.ToInt32(clave);

            //Convertir a hexadecimal
            string claveHexadecimal = Convert.ToString(claveDecimal, 16);

            //Completar la cadena con 0 a la izquierda hasta alcanzar las 15 posiciones
            claveHexadecimal = claveHexadecimal.PadLeft(16, '0');
            //string claveHexadecimal = clave.ToString("X");

            //De hexadecimal a entero
            int num = Int32.Parse(claveHexadecimal, System.Globalization.NumberStyles.HexNumber);

            int claveDecimalAgain = Convert.ToInt32(claveHexadecimal);


            clave = claveHexadecimal;

            return clave;
        }
    }
}
