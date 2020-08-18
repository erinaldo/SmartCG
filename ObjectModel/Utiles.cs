using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;

//Para leer procedimientos externos radicados en librerías de windows
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace ObjectModel
{
    /// <summary>
    /// Crea una pareja nombre, valor
    /// que después se puede adicionar a un ArrayList
    /// Es útil para llenar ComboBox
    /// </summary>
    public class AddValue
    {
        private string m_Display;
        private string m_Value;
        public AddValue(string Display, string Value)
        {
            m_Display = Display;
            m_Value = Value;
        }
        public string Display
        {
            get { return m_Display; }
        }
        public string Value
        {
            get { return m_Value; }
        }
    }
    
    public class Utiles
    {
        //Declaración de constantes necesarias (valores en hexadecimal)
        protected const int MF_BYPOSITION = 0x400;
        protected const int MF_REMOVE = 0x1000;
        protected const int MF_DISABLED = 0x2;

        //Importación de procedimientos externos almacenados
        //en la librería de Windows USER32.DLL

        //Quitar elementos del menú de sistema
        [DllImport("user32.Dll")]
        public static extern IntPtr RemoveMenu(int hMenu, int nPosition, int wFlags);

        //Obtener el menú de sistema
        [DllImport("User32.Dll")]
        public static extern IntPtr GetSystemMenu(int hWnd, bool bRevert);

        //Obtener el número de elementos del menú de sistema
        [DllImport("User32.Dll")]
        public static extern IntPtr GetMenuItemCount(int hMenu);

        //Redibujar la barra de título de la ventana
        [DllImport("User32.Dll")]
        public static extern IntPtr DrawMenuBar(int hwnd);
        
        public static class OdbcWrapper
        {
            [DllImport("odbc32.dll")]
            public static extern int SQLDataSources(int EnvHandle, int Direction, StringBuilder ServerName, int ServerNameBufferLenIn,
                         ref int ServerNameBufferLenOut, StringBuilder Driver, int DriverBufferLenIn, ref int DriverBufferLenOut);

            [DllImport("odbc32.dll")]
            public static extern int SQLAllocEnv(ref int EnvHandle);
        }

        /// <summary>
        /// Devuelve un arrayList con los nombres de las DSNs
        /// </summary>
        public ArrayList ListODBCsources()
        {
            ArrayList odbcArray = new ArrayList();

            int envHandle = 0;
            const int SQL_FETCH_NEXT = 1;
            const int SQL_FETCH_FIRST_SYSTEM = 32;

            if (OdbcWrapper.SQLAllocEnv(ref envHandle) != -1)
            {
                int ret;
                StringBuilder serverName = new StringBuilder(1024);
                StringBuilder driverName = new StringBuilder(1024);
                int snLen = 0;
                int driverLen = 0;
                ret = OdbcWrapper.SQLDataSources(envHandle, SQL_FETCH_FIRST_SYSTEM, serverName, serverName.Capacity, ref snLen,
                            driverName, driverName.Capacity, ref driverLen);

                string dsn = "";
                
                while (ret == 0)
                {
                    dsn = serverName.ToString().ToUpper();
                    odbcArray.Add(dsn);

                    //System.Windows.Forms.MessageBox.Show(serverName + System.Environment.NewLine + driverName);
                    ret = OdbcWrapper.SQLDataSources(envHandle, SQL_FETCH_NEXT, serverName, serverName.Capacity, ref snLen,
                            driverName, driverName.Capacity, ref driverLen);
                }
            }

            return (odbcArray);
        }

        /// <summary>
        /// Actualiza una variable del appSettings
        /// </summary>
        /// <param name="nombre">Nombre de la variable</param>
        /// <param name="valor">Valor para la variable</param>
        public void ModificarappSettings(string nombre, string valor)
        {
            //Actualizar la ruta para los ficheros
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[nombre].Value = valor;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Crea una variable del appSettings
        /// </summary>
        /// <param name="nombre">Nombre de la variable</param>
        /// <param name="valor">Valor para la variable</param>
        public void CrearappSettings(string nombre, string valor)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(nombre, valor);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Método que desactiva el botón X (cerrar)
        /// </summary>
        /// <param name="hWnd"></param>
        public void DisableCloseButton(int hWnd)
        {
            IntPtr hMenu;
            IntPtr menuItemCount;
            //Obtener el manejador del menú de sistema del formulario
            hMenu = GetSystemMenu(hWnd, false);
            //Obtener la cuenta de los ítems del menú de sistema.
            //Es el menú que aparece al pulsar sobre el icono a la izquierda
            //de la Barra de título de la ventana, consta de los ítems: Restaurar, Mover,
            //Tamaño,Minimizar,  Maximizar, Separador, Cerrar
            menuItemCount = GetMenuItemCount(hMenu.ToInt32());
            //Quitar el ítem Close (Cerrar), que es el último de ese menú
            RemoveMenu(hMenu.ToInt32(), menuItemCount.ToInt32() - 1, MF_DISABLED | MF_BYPOSITION);
            //Quitar el ítem Separador, el penúltimo de ese menú, entre Maximizar y Cerrar
            RemoveMenu(hMenu.ToInt32(), menuItemCount.ToInt32() - 2, MF_DISABLED | MF_BYPOSITION);
            //Redibujar la barra de menú
            DrawMenuBar(hWnd);
        }


        /// <summary>
        /// Devuelve que siglo le corresponde al año
        /// </summary>
        /// <param name="aa">Año (los dos últimos dígitos)</param>
        /// <param name="GLC01_ALSIRC">Año de corte</param>
        /// <returns></returns>
        public string SigloDadoAnno(string aa, string GLC01_ALSIRC)
        {
            string siglo = "1";
            try
            {
                int iAA = int.Parse(aa);
                int corte = int.Parse(GLC01_ALSIRC);
                if (iAA < corte)
                {
                    siglo = "1";
                }
                else
                {
                    siglo = "0";
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return siglo;
        }

        /// <summary>
        /// Valida que sólo se introduzcan números con posicionesDecimales como posiciones decimales
        /// </summary>
        /// <param name="posicionesDecimales">Posiciones Decimales</param>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="e">true -> acepta valoresnegativos  false -> no acepta valores negativos</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        public void ValidarNumeroConDecimalesKeyPress(int posicionesDecimales, ref System.Windows.Forms.TextBox textbox, bool aceptaNegativo, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar == 45 && !aceptaNegativo) //Negativo y no lo acepta
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 45) //Negativo
            {
                if (textbox.Text == "")
                {
                    e.Handled = false;
                    return;
                }

                try
                {
                    decimal textboxDec = Convert.ToDecimal(textbox.Text) * -1;
                    textbox.Text = textboxDec.ToString();
                    textbox.SelectionStart = textbox.Text.Length;
                }
                catch {}

                e.Handled = true;
                return;
            }
            
            if (e.KeyChar == 44) //coma 
            {
                if ((textbox.Text.Length - textbox.SelectionStart) >= posicionesDecimales + 1)
                {
                    e.Handled = true;
                    return;
                }
            }

            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < textbox.Text.Length; i++)
            {
                if (textbox.Text[i] == ',') IsDec = true;     //(textbox.Text[i] == '.' || textbox.Text[i] == ',')

                if ((IsDec && nroDec++ >= posicionesDecimales) && ((textbox.Text.Length - textbox.SelectionStart) < nroDec))
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)                 //Número
            {
                if (!aceptaNegativo || textbox.Text.Length < 1) e.Handled = false;
                else
                {
                    if (textbox.Text.Substring(0, 1) == "-" && textbox.SelectionStart == 0) e.Handled = true;
                    else e.Handled = false;
                }
            }
            else if (e.KeyChar == 44)            //Coma  (e.KeyChar == 46  Punto)
            {
                if (posicionesDecimales > 0) e.Handled = IsDec ? true : false;
                else e.Handled = true;
            }
            else
                if (e.KeyChar == 45 && textbox.Text.Length == 0)    //Negativo (Menos) y cadena vacía, es decir el signo negativo en primera posición
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// Valida que sólo se introduzcan números con posicionesDecimales como posiciones decimales
        /// </summary>
        /// <param name="posicionesDecimales">Posiciones Decimales</param>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="e">true -> acepta valoresnegativos  false -> no acepta valores negativos</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        public void ValidarNumeroConDecimalesKeyPress(int posicionesDecimales, ref Telerik.WinControls.UI.RadTextBoxControl radTextBoxControl, bool aceptaNegativo, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar == 45 && !aceptaNegativo) //Negativo y no lo acepta
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 45) //Negativo
            {
                if (radTextBoxControl.Text == "")
                {
                    e.Handled = false;
                    return;
                }

                try
                {
                    decimal textboxDec = Convert.ToDecimal(radTextBoxControl.Text) * -1;
                    radTextBoxControl.Text = textboxDec.ToString();
                    radTextBoxControl.SelectionStart = radTextBoxControl.Text.Length;
                }
                catch { }

                e.Handled = true;
                return;
            }

            if (e.KeyChar == 44) //coma 
            {
                if ((radTextBoxControl.Text.Length - radTextBoxControl.SelectionStart) >= posicionesDecimales + 1)
                {
                    e.Handled = true;
                    return;
                }
            }

            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < radTextBoxControl.Text.Length; i++)
            {
                if (radTextBoxControl.Text[i] == ',') IsDec = true;     //(textbox.Text[i] == '.' || textbox.Text[i] == ',')

                if ((IsDec && nroDec++ >= posicionesDecimales) && ((radTextBoxControl.Text.Length - radTextBoxControl.SelectionStart) < nroDec))
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)                 //Número
            {
                if (!aceptaNegativo || radTextBoxControl.Text.Length < 1) e.Handled = false;
                else
                {
                    if (radTextBoxControl.Text.Substring(0, 1) == "-" && radTextBoxControl.SelectionStart == 0) e.Handled = true;
                    else e.Handled = false;
                }
            }
            else if (e.KeyChar == 44)            //Coma  (e.KeyChar == 46  Punto)
            {
                if (posicionesDecimales > 0) e.Handled = IsDec;
                else e.Handled = true;
            }
            else
                if (e.KeyChar == 45 && radTextBoxControl.Text.Length == 0)    //Negativo (Menos) y cadena vacía, es decir el signo negativo en primera posición
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Valida que sólo se introduzcan números
        /// </summary>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        /// <param name="negalivo">true -> permite números negativos    false -> no permite números negativos</param>
        public void ValidarNumeroKeyPress(ref Telerik.WinControls.UI.RadTextBoxControl textbox, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e, bool negativo)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)                 //Número
                e.Handled = false;
            else
                if (negativo && (e.KeyChar == 45 && textbox.Text.Length == 0))    //Negativo (Menos) y cadena vacía, es decir el signo negativo en primera posición
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// Valida que sólo se introduzcan letras o números
        /// </summary>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        /// <param name="soloMayuscula">true -> permite sólo letras mayúsculas    false -> permite letras mayúsculas y minúsculas</param>
        public void ValidarSoloLetraNumeroKeyPress(ref System.Windows.Forms.TextBox textbox, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e, bool soloMayuscula)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            string patron = "";
            if (soloMayuscula) patron = "^[A-Z0-9]+$";
            else patron = "^[a-zA-Z0-9]+$";

            bool letraoNum = System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @patron);

            if (letraoNum)
            {
                e.Handled = false;
                return;
            }
            else e.Handled = true;
        }

        /// <summary>
        /// Valida que sólo se introduzcan letras o números
        /// </summary>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        /// <param name="soloMayuscula">true -> permite sólo letras mayúsculas    false -> permite letras mayúsculas y minúsculas</param>
        public void ValidarSoloLetraNumeroKeyPress(ref Telerik.WinControls.UI.RadTextBoxControl textboxControl, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e, bool soloMayuscula)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            string patron = "";
            if (soloMayuscula) patron = "^[A-Z0-9]+$";
            else patron = "^[a-zA-Z0-9]+$";

            bool letraoNum = System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @patron);

            if (letraoNum)
            {
                e.Handled = false;
                return;
            }
            else e.Handled = true;
        }

        /// <summary>
        /// Ajustar las columnas de un objeto DataGridView
        /// </summary>
        /// <param name="grid">Objeto DataGridView</param>
        /// <param name="indiceColumna">-1 todas las columnas; en caso contrario, la columna deseada</param>
        public void AjustarColumnasGrid(ref TGGrid grid, int indiceColumna)
        {
            if (indiceColumna != -1)
            {
                if (indiceColumna >= 0 && indiceColumna < grid.ColumnCount)
                {
                    //Ajustar la columna indicada (indiceColumna)
                    grid.AutoResizeColumn(indiceColumna);
                }
            }
            else
            {
                //Ajustar todas las columnas
                for (int i = 0; i < grid.ColumnCount; i++)
                {
                    grid.AutoResizeColumn(i);
                }
            }
        }

        /// <summary>
        /// Dado una fecha Windows, la devuelve en formato CG para almacenar en la bbdd (aammdd)
        /// </summary>
        /// <param name="fecha">fecha formato Windows</param>
        /// <param name="siglo">Si la fecha se devuelve con siglo o no</param>
        /// <param name="longitud">longitud del campo fecha en la bbdd</param>
        /// <returns></returns>
        public int FechaToFormatoCG(DateTime fecha, bool siglo, int longitud)
        {
            int fechaRes = -1;
            string fechaAux = "";

            if (fecha != null)
            {
                if (longitud == 8)
                {
                    //No hacer tratamiento de la fecha
                    fechaAux = fecha.ToString("yyMMdd");
                    //fechaAux = fecha.Year.ToString() + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0');
                    fechaRes = Convert.ToInt32(fechaAux);
                }
                else
                {
                    string aa = fecha.Year.ToString();
                    if (aa.Length == 4) aa = aa.Substring(2, 2);

                    string sigloRes = "";
                    if (siglo)
                    //if (!siglo)
                    {
                        sigloRes = SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                    }

                    //string mm = fecha.Month.ToString().PadLeft(2, '0');
                    //string dd = fecha.Day.ToString().PadLeft(2, '0');

                    //fechaAux = sigloRes + aa + mm + dd;
                    fechaAux = sigloRes + fecha.ToString("yyMMdd");

                    fechaRes = Convert.ToInt32(fechaAux);
                }
            }

            return (fechaRes);
        }

        /// <summary>
        /// Deado una fecha con el año de 4 digitos, devuelve la fecha en formato CG sigloaammdd
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string FechaAno4DigitosToFormatoCG(DateTime fecha)
        {
            string fechaFormatoCG = "";
            try
            {
                string fechaAAAAMMDD = fecha.Year.ToString().PadLeft(4, '0') + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0');
                int fechaAux = Convert.ToInt32(fechaAAAAMMDD) - 19000000;
                fechaFormatoCG = fechaAux.ToString();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (fechaFormatoCG);
        }

        /// <summary>
        /// Dado una fecha Windows, la devuelve en formato CG para almacenar en la bbdd
        /// </summary>
        /// <param name="fecha">fecha formato Windows</param>
        /// <param name="siglo">Si la fecha se devuelve con siglo o no</param>
        /// <returns></returns>
        public int FechaToFormatoCG(DateTime fecha, bool siglo)
        {
            //Llamar a la funcion que devuelve la fecha en formato siglo, pasándole ninguna longitud
            return (FechaToFormatoCG(fecha, siglo, -1));
        }

        /// <summary>
        /// Dado una fecha en formato CG (saammdd, aammdd, aaaammdd) devuelve la fecha en formato que esté utilizando el usuario en windows
        /// </summary>
        /// <param name="fecha">Fecha Formato CG</param>
        /// <returns></returns>
        public DateTime FechaToFormatoCG(string fecha)
        {
            DateTime result = new DateTime();

            try
            {
                if (fecha != "")
                {
                    if (fecha.Length == 6 || fecha.Length == 7)
                    {
                        int fechaAux = Convert.ToInt32(fecha) + 19000000;
                        string fechaRes = fechaAux.ToString();

                        result = new DateTime(Convert.ToInt16(fechaRes.Substring(0, 4)), Convert.ToInt16(fechaRes.Substring(4, 2)), Convert.ToInt16(fechaRes.Substring(6, 2)));
                    }
                    else if (fecha.Length == 8)
                    {
                        result = new DateTime(Convert.ToInt16(fecha.Substring(0, 4)), Convert.ToInt16(fecha.Substring(4, 2)), Convert.ToInt16(fecha.Substring(6, 2)));                        
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Dado una fecha en formato (dd/mm/aa) devuelve la fecha convertida a DateTime
        /// </summary>
        /// <param name="fecha">Fecha Formato Windows</param>
        /// <returns></returns>
        public DateTime FechaCadenaToDateTime(string fecha)
        {
            DateTime result = new DateTime();

            try
            {
                result = Convert.ToDateTime(fecha);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Dado una hora en formato CG (999999) devuelve la hora en formato hh:mm:ss
        /// </summary>
        /// <param name="hora">Hora Formato CG</param>
        /// <returns></returns>
        public string FormatoCGToTime(string hora)
        {
            string result = "";

            try
            {
                if (hora != "")
                {
                    int horaAux = 999999 - Convert.ToInt32(hora);
                    string aux = horaAux.ToString().PadLeft(6, '0');
                    result = aux.Substring(0, 2) + ":" + aux.Substring(2, 2) + ":" + aux.Substring(4, 2);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Dado un siglo año periodo o un año y periodo devuelve el formato aa-pp
        /// </summary>
        /// <param name="aapp">Siglo Año Periodo o Año Periodo</param>
        /// <returns></returns>
        public string AAPPConFormato(string aapp)
        {
            string result = "";

            try
            {
                aapp = aapp.Trim();

                if (aapp != "")
                {
                    if (aapp.Length == 5) aapp = aapp.Substring(1, 4);      //Quitarle el siglo
                    if (aapp.Length > 2) aapp = aapp.Substring(0, 2) + "-" + aapp.Substring(2, aapp.Length - 2);
                }

                result = aapp;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Importe con formato decimal
        /// </summary>
        /// <param name="importeStr"></param>
        /// <returns></returns>
        public string ImporteFormato(string importeStr, System.Globalization.CultureInfo culture)
        {
            decimal importe = 0;
            if (importeStr != "")
            {
                try
                {
                    importe = Convert.ToDecimal(importeStr);
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            
            return (importe.ToString("N2", culture));
        }

        /// <summary>
        /// Valida si una dirección de email es válida o no
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);

                return true;
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Valida si un iban es válido o no
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidIBAN(string iban)
        {
            try
            {
                iban = iban.ToUpper(); //IN ORDER TO COPE WITH THE REGEX BELOW
                if (String.IsNullOrEmpty(iban))
                    return false;
                else if (Regex.IsMatch(iban, "^[A-Z0-9]"))
                {
                    iban = iban.Replace(" ", String.Empty);
                    string bank =
                    iban.Substring(4, iban.Length - 4) + iban.Substring(0, 4);
                    int asciiShift = 55;
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in bank)
                    {
                        int v;
                        if (Char.IsLetter(c)) v = c - asciiShift;
                        else v = int.Parse(c.ToString());
                        sb.Append(v);
                    }
                    string checkSumString = sb.ToString();
                    int checksum = int.Parse(checkSumString.Substring(0, 1));
                    for (int i = 1; i < checkSumString.Length; i++)
                    {
                        int v = int.Parse(checkSumString.Substring(i, 1));
                        checksum *= 10;
                        checksum += v;
                        checksum %= 97;
                    }
                    return checksum == 1;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                return false;
            }
        }

        // <summary>
        /// Calcula IBAN dada un cuenta
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string calcularIban(string pais, string cuenta)
        {
            // Calculamos el IBAN
            string ccc = cuenta.Trim();
            if (ccc.Length != 20)
            {
                return "";
            }
            // Le añadimos el codigo del pais al ccc
            //ccc = ccc + "142800";

            string Letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int x = Letras.IndexOf(pais.Substring(0, 1), 0) + 1 + 9;
            int z = Letras.IndexOf(pais.Substring(1, 1), 0) + 1 + 9;

            ccc = ccc + x.ToString() + z.ToString() + "00";

            // Troceamos el ccc en partes (26 digitos)
            string[] partesCCC = new string[5];
            partesCCC[0] = ccc.Substring(0, 5);
            partesCCC[1] = ccc.Substring(5, 5);
            partesCCC[2] = ccc.Substring(10, 5);
            partesCCC[3] = ccc.Substring(15, 5);
            partesCCC[4] = ccc.Substring(20, 6);

            int iResultado = int.Parse(partesCCC[0]) % 97;
            string resultado = iResultado.ToString();
            for (int i = 0; i < partesCCC.Length - 1; i++)
            {
                iResultado = int.Parse(resultado + partesCCC[i + 1]) % 97;
                resultado = iResultado.ToString();
            }
            // Le restamos el resultado a 98
            int iRestoIban = 98 - int.Parse(resultado);
            string restoIban = iRestoIban.ToString();
            if (restoIban.Length == 1)
                restoIban = "0" + restoIban;

            return pais + restoIban + cuenta;
        }

        /// <summary>
        /// Valida si un código swift es válido o no
        /// </summary>
        /// <param name="swift"></param>
        /// <returns></returns>
        public bool isValidSWIFT(string swift)
        {
            string patron = "^([a-zA-Z]){4}([a-zA-Z]){2}([0-9a-zA-Z]){2}([0-9a-zA-Z]{3})?$";

            return (Regex.IsMatch(swift, patron));
        }

        /// <summary>
        /// Valida si el año está dentro de los ejercicios permitidos
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        public bool EjercicioValido(string ano)
        {
            bool result = false;

            try
            {
                string anoCorte = CGParametrosGrles.GLC01_ALSIRC;
                int anoCorteInt = Convert.ToInt16(anoCorte);
                int anoInt = Convert.ToInt16(ano);

                int anoFrom = 1900 + anoCorteInt;
                int anoTo = 1999 + anoCorteInt;

                if (anoInt >= anoFrom && anoInt <= anoTo) result = true;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Centra el formulario hijo con respecto a su formulario padre
        /// </summary>
        /// <param name="frmPadre"></param>
        /// <param name="frmHijo"></param>
        public void CentrarFormHijo(System.Windows.Forms.Form frmPadre, System.Windows.Forms.Form frmHijo)
        {
            try
            {
                //frmHijo.StartPosition = FormStartPosition.CenterParent;
                frmHijo.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

                System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

                int x = frmPadre.Location.X + (frmPadre.Width - frmHijo.Width) / 2;

                if (x < 0) x = 0;
                else
                {
                    if (x + frmHijo.Width > rect.Width) x = rect.Width - frmHijo.Width;
                }

                int y = frmPadre.Location.Y + (frmPadre.Height - frmHijo.Height) / 2;

                if (y < 0) y = 0;
                else
                {
                    if (y + frmHijo.Height > rect.Height) y = rect.Height - frmHijo.Height;
                }

                frmHijo.Location = new System.Drawing.Point(x, y);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Recibe una excepción y devuelve una cadena con la info de la excepción
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string CreateExceptionString(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            CreateExceptionString(sb, e, string.Empty);

            return sb.ToString();
        }

        private static void CreateExceptionString(StringBuilder sb, Exception e, string indent)
        {
            if (indent == null)
            {
                indent = string.Empty;
            }
            else if (indent.Length > 0)
            {
                sb.AppendFormat("{0}Inner ", indent);
            }

            sb.AppendFormat("Exception Found:\n{0}Type: {1}", indent, e.GetType().FullName);
            sb.AppendFormat("\n{0}Message: {1}", indent, e.Message);
            sb.AppendFormat("\n{0}Source: {1}", indent, e.Source);
            sb.AppendFormat("\n{0}Stacktrace: {1}", indent, e.StackTrace);

            if (e.InnerException != null)
            {
                sb.Append("\n");
                CreateExceptionString(sb, e.InnerException, indent + "  ");
            }
        }

        /// <summary>
        /// Devuelve el siglo año periodo con formato (aa-pp)
        /// </summary>
        /// <param name="saapp"></param>
        /// <param name="separador"></param>
        /// <returns></returns>
        public string SAAPPFormat(string saapp, string separador)
        {
            string result = saapp;
            try
            {
                if (saapp != "")
                {
                    if (saapp.Length == 5) saapp = saapp.Substring(1, 4);

                    if (saapp.Length == 4) result = saapp.Substring(0, 2) + separador + saapp.Substring(2, 2);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        public void CreateRadDropDownListElement(ref Telerik.WinControls.UI.RadDropDownList dropDownList,
                                                                           ref string[] valores)
        {
            foreach (string valor in valores)
            {
                dropDownList.Items.Add(new Telerik.WinControls.UI.RadListDataItem(valor));
            }
        }

        /// <summary>
        /// Seleccionar/Deseleccionar todas las filas de la grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="grid"></param>
        /// <param name="selectAll"></param>
        public void SelectUnselectAllRadGridViewRows(ref object sender, ref Telerik.WinControls.UI.RadGridView grid, ref bool selectAll)
        {
            if (sender is Telerik.WinControls.UI.GridTableHeaderCellElement headerCell)
            {
                if (!selectAll)
                {
                    grid.SelectAll();
                    selectAll = true;
                }
                else
                {
                    grid.ClearSelection();
                    grid.CurrentRow = null; 
                    selectAll = false;
                }
            }
            else selectAll = false;
        }

        #region RadButton
        public System.Drawing.Color colorEnable = System.Drawing.Color.FromArgb(0, 159, 223);
        public System.Drawing.Color colorNotEnable = System.Drawing.Color.FromArgb(128, 195, 245);
        public System.Drawing.Color colorMouseOver = System.Drawing.Color.FromArgb(153, 204, 255);

        /// <summary>
        /// Método que cambia el color del botón en dependencia de si está activo o no
        /// </summary>
        /// <param name="radButton"></param>
        /// <param name="enable"></param>
        public void ButtonEnabled(ref Telerik.WinControls.UI.RadButton radButton, bool enable)
        {
            radButton.Enabled = enable;
            if (enable) radButton.BackColor = colorEnable;
            else radButton.BackColor = colorNotEnable;
        }

        /// <summary>
        /// Método que cambia el comportamiento del botón cuando el cursor está sobre el botón
        /// </summary>
        /// <param name="radButton"></param>
        public void ButtonMouseEnter(ref Telerik.WinControls.UI.RadButton radButton)
        {
            radButton.ButtonElement.ButtonFillElement.BackColor = colorMouseOver;
            radButton.ButtonElement.ButtonFillElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            radButton.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        /// <summary>
        /// Método que cambia el comportamiento del botón cuando el cursor está sobre el botón
        /// </summary>
        /// <param name="radButton"></param>
        public void ButtonMouseLeave(ref Telerik.WinControls.UI.RadButton radButton)
        {
            radButton.ButtonElement.ButtonFillElement.ResetValue(Telerik.WinControls.Primitives.FillPrimitive.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local);
            radButton.ButtonElement.ButtonFillElement.ResetValue(Telerik.WinControls.Primitives.FillPrimitive.GradientStyleProperty, Telerik.WinControls.ValueResetFlags.Local);
            radButton.Cursor = System.Windows.Forms.Cursors.Default;
        }
        #endregion

        #region ShortCut Menu/Submenuo/MenuClickDerecho/...
        /// <summary>
        /// Devuelve el caracter de shortcut del literal
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public string ObtenerCaracterShortCut(string literal)
        {
            string result = "";

            try
            {
                int pos = literal.IndexOf("&");
                if (pos != -1)
                {
                    if (pos + 1 <= literal.Length) result = literal.Substring(pos + 1, 1).ToUpper();
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el literal para el ToolTipText
        /// </summary>
        /// <param name="literal"></param>
        /// <param name="escape">true -> shortcup escape tambien</param>
        /// <returns></returns>
        public string ObtenerToolTipText(string literal, bool escape)
        {
            string result = "";

            try
            {
                string literalShortCut = this.ObtenerCaracterShortCut(literal);
                if (literalShortCut != "")
                {
                    string literalSinShortCut = literal.Replace("&", "");

                    if (escape) result = literalSinShortCut + " (ALT + " + literalShortCut + " | Esc)";
                    else result = literalSinShortCut + " (ALT + " + literalShortCut + ")";
                }
                else
                {
                    if (escape) result = literal + " (Esc)";
                    else result = literal;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el literal para el ToolTipText
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public string ObtenerToolTipText(string literal)
        {
            //Devuelve el literal para el ToolTipText (con escape a falso)
            string result = this.ObtenerToolTipText(literal, false);

            return (result);
        }
        #endregion

        #region Diccionario
        /// <summary>
        /// Busca en el diccionario por valor y si la encuentra devuelve la clave, sino retorna el valor
        /// </summary>
        /// <param name="dict">Diccionario</param>
        /// <param name="value">Valor a buscar</param>
        /// <returns></returns>
        public string FindFirstKeyByValue(ref Dictionary<string, string> dict, string value)
        {
            try
            {
                if (dict.ContainsValue(value))
                {
                    foreach (string key in dict.Keys)
                    {
                        if (dict[key].Equals(value)) return key;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return value;
        }

        /// <summary>
        /// Busca en el diccionario por clave y si no la encuentra devuelve el valor, sino retorna la clave
        /// </summary>
        /// <param name="dict">Diccionario</param>
        /// <param name="key">Valor de la clave a buscar</param>
        /// <returns></returns>
        public string FindFirstValueByKey(ref Dictionary<string, string> dict, string key)
        {
            try
            {
                if (dict.ContainsKey(key)) return (dict[key].ToString());
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return key;
        }
        #endregion

        /// <summary>
        /// Inserta en la tabla de Auditoria GLL04 el alta/modificación/eliminacion
        /// Se invoca desde el módulo de mantenimiento
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="codigo"></param>
        public void Auditoria(string formulario, AuditoriaGLL04.OperacionTipo operacion, string tabla, string clave1, string clave2, string clave3, string valorOld, string valorNew)
        {
            AuditoriaGLL04 auditoria = new AuditoriaGLL04();
            try
            {
                //Insertar en la tabla de auditoria
                auditoria.Operacion = operacion;
                auditoria.Tabla_FILEL4 = tabla;
                auditoria.Programa_PGMPL4 = formulario;
                auditoria.Clave1_KEY1L4 = clave1;
                if (clave2 != null) auditoria.Clave2_KEY2L4 = clave2;
                if (clave3 != null) auditoria.Clave3_KEY3L4 = clave3;
                if (valorOld != null) auditoria.ValorOld_PRM1L4 = valorOld;
                if (valorNew != null) auditoria.ValorNew_PRM2L4 = valorNew;
                auditoria.InsertarGLL04();
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Inserta en la tabla de Auditoria GLL04 el alta/modificación/eliminacion
        /// Se invoca desde el módulo de mantenimiento
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="codigo"></param>
        public void Auditoria(string formulario, AuditoriaGLL04.OperacionTipo operacion, string tabla, string clave1, string clave2)
        {
            try
            {
                Auditoria(formulario, operacion, tabla, clave1, clave2, null, null, null);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        #region Encriptar Password
        /// <summary>
        /// Convierte de código ANSI a EBCDIC (decimales)
        /// </summary>
        /// <param name="input"></param>
        public void ANSI2EBCDIC(ref byte[] input)
        {   //1    -> 0   - 15  
            //2    -> 16  - 31
            //3    -> 32  - 47
            //4    -> 48  - 63
            //5    -> 64  - 79
            //6    -> 80  - 95
            //7    -> 96  - 111
            //8    -> 112 - 127
            //9    -> 128 - 143
            //10   -> 144 - 159
            //11   -> 160 - 175
            //12   -> 176 - 191
            //13   -> 192 - 207
            //14   -> 208 - 223
            //15   -> 224 - 239
            //16   -> 240 - 255
            int[] A2E = new int[256] {0,1,2,3,55,45,46,47,22,5,37,11,12,13,14,15,
                                    16,17,18,19,60,61,50,38,24,25,63,39,28,29,30,31,
                                    64,187,127,105,91,108,80,125,77,93,92,78,107,96,75,97,
                                    240,241,242,243,244,245,246,247,248,249,122,94,76,126,110,111,
                                    124,193,194,195,196,197,198,199,200,201,209,210,211,212,213,214,
                                    215,216,217,226,227,228,229,230,231,232,233,74,224,90,186,109,
                                    121,129,130,131,132,133,134,135,136,137,145,146,147,148,149,150,
                                    151,152,153,162,163,164,165,166,167,168,169,192,79,208,189,7,
                                    32,33,34,35,36,21,6,23,40,41,42,43,44,9,10,27,
                                    48,49,26,51,52,53,54,8,56,57,58,59,4,20,62,255,
                                    65,170,176,177,159,178,73,181,161,180,154,138,95,202,175,188,
                                    144,143,234,250,190,160,182,179,157,218,155,139,183,184,185,171,
                                    100,101,98,102,99,103,158,104,116,113,114,115,120,117,118,119,
                                    172,123,237,238,235,239,236,191,128,253,254,251,252,173,174,89,
                                    68,69,66,70,67,71,156,72,84,81,82,83,88,85,86,87,
                                    140,106,205,206,203,207,204,225,112,221,222,219,220,141,142,223};

            for (int counter = 0; counter < input.Length; counter++)
            {
                input[counter] = (byte)A2E[input[counter]];
            }
        }

        /// <summary>
        /// Convierte de código EBCDIC a ANSI (decimales)
        /// </summary>
        /// <param name="input"></param>
        public void EBCDIC2ANSI(ref byte[] input)
        {   //1    -> 0   - 15  
            //2    -> 16  - 31
            //3    -> 32  - 47
            //4    -> 48  - 63
            //5    -> 64  - 79
            //6    -> 80  - 95
            //7    -> 96  - 111
            //8    -> 112 - 127
            //9    -> 128 - 143
            //10   -> 144 - 159
            //11   -> 160 - 175
            //12   -> 176 - 191
            //13   -> 192 - 207
            //14   -> 208 - 223
            //15   -> 224 - 239
            //16   -> 240 - 255
            int[] E2A = new int[256] {0,1,2,3,156,9,134,127,151,141,142,11,12,13,14,15,
                                    16,17,18,19,157,133,8,135,24,25,146,143,28,29,30,31,
                                    128,129,130,131,132,10,23,27,136,137,138,139,140,5,6,7,
                                    144,145,22,147,148,149,150,4,152,153,154,155,20,21,158,26,
                                    32,160,226,228,224,225,227,229,231,166,91,46,60,40,43,124,
                                    38,233,234,235,232,237,238,239,236,223,93,36,42,41,59,172,
                                    45,47,194,196,192,193,195,197,199,35,241,44,37,95,62,63,
                                    248,201,202,203,200,205,206,207,204,96,58,209,64,39,61,34,
                                    216,97,98,99,100,101,102,103,104,105,171,187,240,253,254,177,
                                    176,106,107,108,109,110,111,112,113,114,170,186,230,184,198,164,
                                    181,168,115,116,117,118,119,120,121,122,161,191,208,221,222,174,
                                    162,163,165,183,169,167,182,188,189,190,94,33,175,126,180,215,
                                    123,65,66,67,68,69,70,71,72,73,173,244,246,242,243,245,
                                    125,74,75,76,77,78,79,80,81,82,185,251,252,249,250,255,
                                    92,247,83,84,85,86,87,88,89,90,178,212,214,210,211,213,
                                    48,49,50,51,52,53,54,55,56,57,179,219,220,217,218,159};

            for (int counter = 0; counter < input.Length; counter++)
            {
                input[counter] = (byte)E2A[input[counter]];
            }
        }

        /// <summary>
        /// Devuelve la cadena encriptada (para DB2 y SQLServer)
        /// </summary>
        /// <param name="pwd">Contraseña</param>
        /// <param name="tipoBaseDatosCG">Tipo de base de datos de CG (DB2)</param>
        /// <returns></returns>
        public byte[] EncriptarPwd(string pwd, string tipoBaseDatosCG)
        {
            pwd = pwd.PadRight(10, ' ').ToUpper();

            byte[] pwdAux = this.StringToByte(pwd, tipoBaseDatosCG);
            byte[] result = new byte[10];

            byte[] aux = new byte[1];
            aux[0] = pwdAux[0];
            if (tipoBaseDatosCG == "DB2")
            {
                //Convertir el 1er caracter a Ebcdic
                this.ANSI2EBCDIC(ref aux);
            }

            //Un XOR con el caracter 128
            result[0] = Convert.ToByte(aux[0] ^ 0x80);

            for (int i = 1; i < 10; i++)
            {
                aux[0] = pwdAux[i];
                if (tipoBaseDatosCG == "DB2")
                {
                    //Convertir el caracter a Ebcdic
                    this.ANSI2EBCDIC(ref aux);
                }
                //Con el 1er caracter XOR con el caracter 128 hacer XOR con el resto de la cadena
                result[i] = Convert.ToByte(aux[0] ^ result[0]);
            }

            if (tipoBaseDatosCG == "DB2")
            {
                //Converir de Ebcdic a ANSI
                this.EBCDIC2ANSI(ref result);
            }

            return (result);
        }

        public byte[] StringToByte(string InString, string tipoBaseDatosCG)
        {

            byte[] ByteOut = new byte[10];

            char[] charString = InString.ToCharArray();

            for (int i = 0; i < charString.Length; i++)
            {

                if (tipoBaseDatosCG == "SQLServer")
                {
                    //buscar el codigo ascii del caracter, en SQLServer llegan mal!!!!
                    //string query = "select PASWMO, ASCII('" + charString[i] + "') nASCII FROM " + GlobalVar.PrefijoTablaCG + "ATM05";
                    string query = "select ASCII('" + charString[i] + "') nASCII";

                    IDataReader drASCII = null;
                    drASCII = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (drASCII.Read())
                    {
                        ByteOut[i] = Convert.ToByte(drASCII["nASCII"]);
                    }

                    drASCII.Close();
                }
                else
                {
                    ByteOut[i] = Convert.ToByte(charString[i]);
                }
            }

            return ByteOut;
        }

        /// <summary>
        /// Devuelve la contraseña encriptada para Oracle
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string ObtenerPwdEncriptadoOracle(string pwd)
        {
            string pwdEncriptado = "";

            try
            {
                //string cadenaOrigen = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                //string cadenaResult = "QWERTYUIOPASDFGHJKL0987654321ZXCVBNMabcdefghijklmnopqrstuvwxyz";

                string cadenaOrigen = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string cadenaResult = "QWERTYUIOPASDFGHJKL0987654321ZXCVBNMabcdefghij";

                int posCadenaOrigen = 0;
                char caracterActual;
                for (int i = 0; i < pwd.Length; i++)
                {
                    //caracterActual = pwd.Substring(i, 1);
                    caracterActual = pwd[i];
                    posCadenaOrigen = cadenaOrigen.IndexOf(caracterActual);

                    if (posCadenaOrigen == -1) pwdEncriptado += caracterActual;
                    else
                    {
                        if (posCadenaOrigen + i + 1 <= cadenaResult.Length) pwdEncriptado += cadenaResult[posCadenaOrigen + i + 1];
                    }
                }

            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (pwdEncriptado);
        }

        /// <summary>
        /// Llenar un Desplegable
        /// </summary>
        /// <param name="query">Sentencia SQL</param>
        /// <param name="campoCodigo">campo codigo de la select</param>
        /// <param name="campoDesc">campo descripción de la select</param>
        /// <param name="control">control ComboBox (se pasa por referencia)</param>
        /// <param name="CodDesc">True si se visualiza codigo - descripcion y False si solo se visualiza descripcion</param>
        /// <param name="indiceSel">Indice del ComboBox que se activará</param>
        /// <param name="elementoVacio">True si adiciona entrada vacia y False si solo muestra los datos de la tabla</param>
        /// <returns></returns>
        public string FillComboBox(string query, string campoCodigo, string campoDesc, ref Telerik.WinControls.UI.RadDropDownList control, bool CodDesc, int indiceSel, bool elementoVacio)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                ArrayList elementos = new ArrayList();
                string nombre = "";
                string codigo = "";

                int cont = 0;
                while (dr.Read())
                {
                    //Adicionar elemento vacío si es necesario
                    if (elementoVacio && cont == 0)
                    {
                        nombre = "";
                        codigo = "";
                        elementos.Add(new AddValue(nombre, codigo));
                    }
                    cont++;

                    //Falta chequear autorizacion
                    nombre = dr[campoDesc].ToString().Trim();
                    codigo = dr[campoCodigo].ToString().Trim();
                    if (CodDesc) elementos.Add(new AddValue(codigo + " - " + nombre, codigo));
                    else elementos.Add(new AddValue(nombre, codigo));
                }

                dr.Close();

                control.DisplayMember = "Display";
                control.ValueMember = "Value";
                control.DataSource = elementos;

                control.SelectedIndex = indiceSel;
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));

                //Error obteniendo los grupos
                result = ex.Message;

                if (dr != null) dr.Close();
            }

            return (result);
        }
        #endregion
    }

    #region Clase CheckNif
    /// <summary>
    /// Chequea si es válido un CIF o NIF o NIE
    /// </summary>
    public static class CheckNif
    {
        const string c_NIFNIE = "TRWAGMYFPDXBNJZSQVHLCKE";
        const string c_CIF = "ABCDEFGHIJ";


        public static bool Check(string NIF, ref string error)
        {
            error = "";

            int lon = NIF.Trim().Length;
            int xlon = 0, numero = 0;
            string txtnum = string.Empty;
            NIF = NIF.Trim();
            short res = 0;

            if (lon != 9)
            {
                error = "CIF/NIF/NIE Incorrecto";
                return false;//Si la longitud no es 9 Error
            }

            string CC = NIF.Substring(lon - 1, 1);
            if (Int16.TryParse(NIF.Substring(0, 1), out res)) //Si es un Nº el primer Carácter
            {
                xlon = lon - 1;
                txtnum = NIF.Substring(0, xlon);
                if (checkErrorNumber(xlon, txtnum, ref error)) return false;
                numero = Convert.ToInt32(NIF.Substring(0, xlon));
                if (!NIF_NIE(numero, CC, ref error)) return false;
            }
            else //Si es una Letra el primer Carácter
            {
                string PC = NIF.Substring(0, 1);
                xlon = lon - 2;
                txtnum = NIF.Substring(1, xlon);
                if (checkErrorNumber(xlon, txtnum, ref error)) return false;
                numero = Convert.ToInt32(NIF.Substring(1, xlon));
                char[] keys = { 'L', 'K', 'X', 'Y', 'M', 'Z' };
                if (PC.IndexOfAny(keys) == 0)
                {
                    if (PC == "Y") numero += 10000000;
                    if (PC == "Z") numero += 20000000; //!!!!!!
                    if (!NIF_NIE(numero, CC, ref error)) return false;
                }
                else
                    if (!CIF(txtnum, CC, PC, ref error)) return false;
            }
            return true;
        }

        private static bool CIF(string txtnum, string CC, string PC, ref string error)
        {
            int pares = 0, impares = 0, operando = 0;
            string letra = string.Empty;
            for (int i = 0; i < txtnum.Length; i++)
            {
                if ((((decimal)i + 1) / 2) - (int)((i + 1) / 2) == 0)//Pares
                    pares += Convert.ToInt32(txtnum.Substring(i, 1));
                else //Impares
                {
                    operando = Convert.ToInt32(txtnum.Substring(i, 1)) * 2;
                    operando = (int)((int)(operando / 10) + ((((decimal)operando / 10) - (int)(operando / 10)) * 10));
                    impares += operando;
                }
            }
            operando = pares + impares;
            operando = (int)((((decimal)operando / 10) - (int)(operando / 10)) * 10);
            operando = 10 - operando;
            if (operando == 0) operando = 10;
            char[] keys = { 'N', 'P', 'Q', 'R', 'S', 'W' };
            if (PC.IndexOfAny(keys) == 0)//Si es un Carácter
                letra = c_CIF.Substring(operando - 1, 1);
            else //Si es un Nº
            {
                letra = c_CIF.Substring(operando - 1, 1);
                int j = c_CIF.IndexOf(letra) + 1;
                if (j == 10) j = 0;
                letra = j.ToString();
            }
            if (letra != CC)
            {
                error = "CIF/NIF/NIE Incorrecto";
                return false;
            }
            return true;
        }

        private static bool NIF_NIE(int numero, string CC, ref string error)
        {
            int modulo = numero - ((int)(numero / 23) * 23) + 1;
            if (c_NIFNIE.Substring(modulo - 1, 1) != CC)
            {
                error = "CIF/NIF/NIE Incorrecto";
                return false;
            }
            return true;
        }

        private static bool checkErrorNumber(int xlon, string txtnum, ref string error)
        {
            short res = 0;
            for (int i = 0; i < xlon; i++)
            {
                if (!Int16.TryParse(txtnum.Substring(i, 1), out res))
                {
                    error = "CIF/NIF/NIE Incorrecto";
                    return true;
                }
            }
            return false;
        }
    }
    #endregion

    #region Clase CuentasBancarias
    /// <summary>
    /// Servicios de validación de las cuentas bancarias españolas
    /// </summary>
    public static class CuentasBancarias
    {
        /// <summary>
        /// Validación de una cuenta bancaria española
        /// </summary>
        /// <param name="banco">Código del banco en formato "0000"</param>
        /// <param name="oficina">Código de la sucursal en formato "0000"</param>
        /// <param name="dc">Dígito de control en formato "00"</param>
        /// <param name="cuenta">Número de cuenta en formato "0000000000"</param>
        /// <returns>true si el número de cuenta es correcto</returns>
        public static bool ValidaCuentaBancaria(string banco, string oficina, string dc, string cuenta)
        {
            // Se comprueba que realmente sean números los datos pasados como parámetros y que las longitudes
            // sean correctas
            if (!IsNumeric(banco) || banco.Length != 4)
                throw new ArgumentException("El banco no tiene un formato adecuado", "errorBancoFormato");

            if (!IsNumeric(oficina) || oficina.Length != 4)
                throw new ArgumentException("La oficina no tiene un formato adecuado", "errorOficinaFormato");

            if (!IsNumeric(dc) || dc.Length != 2)
                throw new ArgumentException("El dígito de control no tiene un formato adecuado", "errorDCFormato");

            if (!IsNumeric(cuenta) || cuenta.Length != 10)
                throw new ArgumentException("El número de cuenta no tiene un formato adecuado", "errorNumeroCtaFormato");

            return CompruebaCuenta(banco, oficina, dc, cuenta);
        }

        /// <summary>
        /// Validación de una cuenta bancaria española
        /// </summary>
        /// <param name="cuentaCompleta">Número de cuenta completa con carácteres numéricos y 20 dígitos</param>
        /// <returns>true si el número de cuenta es correcto</returns>
        public static bool ValidaCuentaBancaria(string cuentaCompleta)
        {
            // Comprobaciones de la cadena
            if (cuentaCompleta.Length != 20)
                throw new ArgumentException("El número de cuenta no el formato adecuado", "errorFormato");

            string banco = cuentaCompleta.Substring(0, 4);
            string oficina = cuentaCompleta.Substring(4, 4);
            string dc = cuentaCompleta.Substring(8, 2);
            string cuenta = cuentaCompleta.Substring(10, 10);

            return ValidaCuentaBancaria(banco, oficina, dc, cuenta);

        }

        /// <summary>
        /// Validación de una cuenta bancaria española
        /// </summary>
        /// <param name="banco">Código del banco</param>
        /// <param name="oficina">Código de la oficina</param>
        /// <param name="dc">Dígito de control</param>
        /// <param name="cuenta">Número de cuenta</param>
        /// <returns>true si el número de cuenta es correcto</returns>
        public static bool ValidaCuentaBancaria(UInt64 banco, UInt64 oficina, UInt64 dc, UInt64 cuenta)
        {
            return ValidaCuentaBancaria(
                                banco.ToString("0000")
                                , oficina.ToString("0000")
                                , dc.ToString("00")
                                , cuenta.ToString("0000000000")
                                );
        }

        /// <summary>
        /// Comprueba que la cadena sólo incluya números
        /// </summary>
        /// <param name="numero">Cadena de texto en formato número</param>
        /// <returns>true si <paramref name="numero"/> contiene sólo números</returns>
        /// <remarks>No se contemplan decimales</remarks>
        private static bool IsNumeric(string numero)
        {
            Regex regex = new Regex("[0-9]");
            return regex.Match(numero).Success;
        }

        /// <summary>
        /// Una cuenta bancaria está validada si los dígitos de control calculados coinciden con los
        /// que se han pasado en los argumentos
        /// </summary>
        private static bool CompruebaCuenta(string banco, string oficina, string dc, string cuenta)
        {
            return GetDigitoControl("00" + banco + oficina) + GetDigitoControl(cuenta) == dc;
        }

        /// <summary>
        /// Obtiene el dígito de control de una cuenta bancaria. La función sólo devuelve un número
        /// que corresponderá a una de las dos opciones.
        ///     - Codigo + CódigoOficina
        ///     - CuentaBancaria
        /// </summary>
        private static string GetDigitoControl(string CadenaNumerica)
        {
            int[] pesos = { 1, 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            UInt32 suma = 0;
            UInt32 resto;

            for (int i = 0; i < pesos.Length; i++)
            {
                suma += (UInt32)pesos[i] * UInt32.Parse(CadenaNumerica.Substring(i, 1));
            }
            resto = 11 - (suma % 11);

            if (resto == 10) resto = 1;
            if (resto == 11) resto = 0;

            return resto.ToString("0");
        }
    }
    #endregion

    #region Clase NewProgressBar
    public class NewProgressBar : ProgressBar
    {
        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;

            //var brush = new SolidColorBrush(Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223))))));

            e.Graphics.FillRectangle(Brushes.Blue, 2, 2, rec.Width, rec.Height);
        }
    }
    #endregion
}
