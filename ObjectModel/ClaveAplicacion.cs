using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModel
{
    public static class ClaveAplicacion
    {
        static string _claveEncriptada;
        public static string claveEncriptada
        {
            get
            {
                return (_claveEncriptada);
            }
            set
            {
                _claveEncriptada = value;
            }
        }

        static string _codigoClienteDesencriptado;
        public static string codigoClienteDesencriptado
        {
            get
            {
                return (_codigoClienteDesencriptado);
            }
            set
            {
                _codigoClienteDesencriptado = value;
            }
        }

        static string _fechaDesencriptada;
        public static string fechaDesencriptada
        {
            get
            {
                return (_fechaDesencriptada);
            }
            set
            {
                _fechaDesencriptada = value;
            }
        }

        static Boolean  _claveCorrecta;
        public static Boolean claveCorrecta
        {
            get
            {
                return (_claveCorrecta);
            }
            set
            {
                _claveCorrecta = value;
            }
        }

        public static void Encriptar(string strCaracteres)
        {
            string Ristra = "PJKAAR0RTP1EVSDYV2YDIR1LGAMN9ATTLZW8PZ8J3ZO0K33SA1ADAYZEA55NEEPBM76L2GW9SDAXMMF3KPWU0JWFOIIGCE928Z2EDH5H9MJ5XNIE3SGRLT0723B40JNJT26OGC5PXJ3REGH7B3L6X3F9VR9O931BSA8H9C1H7VLRXBJ2MEY4XMC5H7Z41CJXJEUTOPZCHEP6FJTJFI8MHOVDAA5J8LDVH6OQMS5T7Y22SDG4461PJK7GQREWQ3M";
            //string Ristra2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string keyGrabar = "";
            keyGrabar = Ristra.Substring(0,3) + 
                        strCaracteres.Substring(0, 1) +
                        Ristra.Substring(4, 3) +
                        strCaracteres.Substring(1, 1) +
                        Ristra.Substring(8, 7) +
                        strCaracteres.Substring(2, 1) +
                        Ristra.Substring(16, 15) +
                        strCaracteres.Substring(3, 1) +
                        Ristra.Substring(32, 31) +
                        strCaracteres.Substring(5, 1) +
                        Ristra.Substring(64, 63) +
                        strCaracteres.Substring(6, 1) +
                        Ristra.Substring(128, 123) +
                        strCaracteres.Substring(20, 1) +
                        strCaracteres.Substring(21, 1) +
                        strCaracteres.Substring(22, 1) +
                        strCaracteres.Substring(23, 1);

            _claveEncriptada = keyGrabar;
        }

        public static void DesencriptarMAF(string strCodigoCliente, string strCaracteres)
        {
            //string Ristra = "PJKAAR0RTP1EVSDYV2YDIR1LGAMN9ATTLZW8PZ8J3ZO0K33SA1ADAYZEA55NEEPBM76L2GW9SDAXMMF3KPWU0JWFOIIGCE928Z2EDH5H9MJ5XNIE3SGRLT0723B40JNJT26OGC5PXJ3REGH7B3L6X3F9VR9O931BSA8H9C1H7VLRXBJ2MEY4XMC5H7Z41CJXJEUTOPZCHEP6FJTJFI8MHOVDAA5J8LDVH6OQMS5T7Y22SDG4461PJK7GQREWQ3M";
            string Ristra2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string sFechaCaducidad;
            //DateTime dFechaCaducidad;
            long lClave;
            string sClave;
            long numero = 0;
            int numero1 = 0;
            int numero2 = 0;
            int numero3 = 0;
            int numero4 = 0;
            int numero5 = 0;
            int numero6 = 0;

            sClave = strCaracteres.Substring(3, 1) + strCaracteres.Substring(7, 1) + strCaracteres.Substring(15, 1) +
                strCaracteres.Substring(31, 1) + strCaracteres.Substring(63, 1) + strCaracteres.Substring(127, 1);

            numero1 = Ristra2.IndexOf(strCaracteres.Substring(3, 1));
            numero2 = Ristra2.IndexOf(strCaracteres.Substring(7, 1));
            numero3 = Ristra2.IndexOf(strCaracteres.Substring(15, 1));
            numero4 = Ristra2.IndexOf(strCaracteres.Substring(31, 1));
            numero5 = Ristra2.IndexOf(strCaracteres.Substring(63, 1));
            numero6 = Ristra2.IndexOf(strCaracteres.Substring(127, 1));

            numero = (((((((((numero6 * 36) + numero5) * 36) + numero4) * 36) + numero3) * 36) + numero2) * 36) + numero1;

            lClave = numero - 70000000;
            sClave = lClave.ToString();
            sClave = sClave.PadLeft(9,'0');

            //' D2 M2 A4 B2 U B1 A3 M1 D1
            sFechaCaducidad = "20" + sClave.Substring(6, 1) + sClave.Substring(2, 1) + sClave.Substring(7, 1) +
                                    sClave.Substring(1, 1) + sClave.Substring(8, 1) + sClave.Substring(0, 1);

            _fechaDesencriptada = sFechaCaducidad;
        }

        public static void Desencriptar(string strCaracteres)
        {
            //string Ristra = "PJKAAR0RTP1EVSDYV2YDIR1LGAMN9ATTLZW8PZ8J3ZO0K33SA1ADAYZEA55NEEPBM76L2GW9SDAXMMF3KPWU0JWFOIIGCE928Z2EDH5H9MJ5XNIE3SGRLT0723B40JNJT26OGC5PXJ3REGH7B3L6X3F9VR9O931BSA8H9C1H7VLRXBJ2MEY4XMC5H7Z41CJXJEUTOPZCHEP6FJTJFI8MHOVDAA5J8LDVH6OQMS5T7Y22SDG4461PJK7GQREWQ3M";
            string Ristra2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string sFechaCaducidad;
            //DateTime dFechaCaducidad;
            
            int numero1 = 0;
            int numero2 = 0;
            int numero3 = 0;
            int numero4 = 0;
            int numero5 = 0;
            int numero6 = 0;
            int codCli1 = 0;
            int codCli2 = 0;
            int codCli3 = 0;
            int codCli4 = 0;
            string strCodCliEntero;
            int intCodCliUlt;

            //codigo cliente
            codCli1 = Ristra2.IndexOf(strCaracteres.Substring(251, 1)) - 16;
            codCli2 = Ristra2.IndexOf(strCaracteres.Substring(252, 1)) - 17;
            codCli3 = Ristra2.IndexOf(strCaracteres.Substring(253, 1)) - 18;
            codCli4 = Ristra2.IndexOf(strCaracteres.Substring(254, 1)) - 19;
            strCodCliEntero = codCli1.ToString() + codCli2.ToString() + codCli3.ToString() + codCli4.ToString();
            intCodCliUlt = int.Parse(strCodCliEntero.Substring(3,1));

            numero1 = Ristra2.IndexOf(strCaracteres.Substring(3, 1)) - intCodCliUlt - 2;
            if (numero1 < 0) numero1 += 36;
            numero2 = Ristra2.IndexOf(strCaracteres.Substring(7, 1)) - intCodCliUlt - 3;
            if (numero2 < 0) numero2 += 36;
            numero3 = Ristra2.IndexOf(strCaracteres.Substring(15, 1)) - intCodCliUlt - 5;
            if (numero3 < 0) numero3 += 36;
            numero4 = Ristra2.IndexOf(strCaracteres.Substring(31, 1)) - intCodCliUlt - 7;
            if (numero4 < 0) numero4 += 36;
            numero5 = Ristra2.IndexOf(strCaracteres.Substring(63, 1)) - intCodCliUlt - 11;
            if (numero5 < 0) numero5 += 36;
            numero6 = Ristra2.IndexOf(strCaracteres.Substring(127, 1)) - intCodCliUlt - 13;
            if (numero6 < 0) numero6 += 36;

            //' D2 M2 A4 B2 U B1 A3 M1 D1
            sFechaCaducidad = "20" + Ristra2.Substring(numero1, 1) + Ristra2.Substring(numero2, 1) + 
                            Ristra2.Substring(numero3, 1) +  Ristra2.Substring(numero4, 1) + 
                            Ristra2.Substring(numero5, 1) + Ristra2.Substring(numero6, 1);

            _fechaDesencriptada = sFechaCaducidad;
            _codigoClienteDesencriptado = strCodCliEntero;
        }

        public static void ValidarClave(string strCodigoCliente, string strCaracteres)
        {
            DateTime fechaSistema;

            if(strCaracteres.Trim() == "")
            {
                _claveCorrecta = false;
                return;
            }

            Desencriptar(strCaracteres);

            if (strCodigoCliente.PadLeft(4,'0') != _codigoClienteDesencriptado.PadLeft(4, '0'))
            {
                _fechaDesencriptada = "Error en código.";
                _claveCorrecta = false;
                return;
            }

            fechaSistema = DateTime.Today;

            try
            {
                System.DateTime fecDes = new DateTime(Int32.Parse(_fechaDesencriptada.Substring(0, 4)),
                Int32.Parse(_fechaDesencriptada.Substring(4, 2)),
                Int32.Parse(_fechaDesencriptada.Substring(6, 2)));

                if (fechaSistema > fecDes)
                {
                    _claveCorrecta = false;
                }
                else
                {
                    _claveCorrecta = true;
                }
            }
            catch (Exception ex)
            {
                _fechaDesencriptada = "";
                _claveCorrecta = false;
            }            
        }
    }
}
