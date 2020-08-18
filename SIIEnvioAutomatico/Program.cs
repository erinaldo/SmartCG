using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using log4net;
using System.Net.Mail;

namespace SIIEnvioAutomatico
{
    class Program
    {
        internal static ILog Log = null;
        internal static tgSIIWebService.TGsiiService wsTGsii = null;
        internal static string logActivo = "";
        internal static string compania = "";
        internal static string libro = "";
        internal static string operacion = "";
        internal static string ejercicio = "";
        internal static string periodo = "";     
        internal static string resultActivo = "0";
        internal static string resultPath = "";
        internal static string mailActivo = "";
        internal static string mailActivoSoloAlerta = "";

        static void Main(string[] args)
        {
            logActivo = LeerAppSetting("logActivo", "0");

            //Inicializar el objeto log
            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["pid"] = System.Diagnostics.Process.GetCurrentProcess().Id;
            Log = log4net.LogManager.GetLogger("SIIEnvioAutomatico");

            try
            {
                if (logActivo == "1") Log.Info("INICIO SII Envío Automático");

                //Leer parámetros de configuración
                string logPath = LeerAppSetting("logPath", "");
                compania = LeerAppSetting("compania", "");
                libro = LeerAppSetting("libro", "");
                operacion = LeerAppSetting("operacion", "");
                ejercicio = LeerAppSetting("ejercicio", "");
                periodo = LeerAppSetting("periodo", "");
                resultActivo = LeerAppSetting("resultActivo", "0");
                resultPath = LeerAppSetting("resultPath", "");

                string SIITimeOutWS = LeerAppSetting("SIITimeOutWS", "0");
                int siiWSTimeOut = Convert.ToInt32(SIITimeOutWS);

                mailActivo = LeerAppSetting("mailActivo", "");
                mailActivoSoloAlerta = LeerAppSetting("mailActivoSoloAlerta", "");

                //Inicializar el webservice
                wsTGsii = new tgSIIWebService.TGsiiService();
                if (siiWSTimeOut == 0) wsTGsii.Timeout = System.Threading.Timeout.Infinite;
                else wsTGsii.Timeout = siiWSTimeOut;

                try
                {
                    bool realizarOtroEnvio = true;
                    int numeroEnvio = 0;

                    //----- Falta parametrizar el numero de intentos y el tiempo de espera
                    //Si el numero de intentos no existe o está definido con 0, inicializarlo a 1 !!!

                    string numeroIntentosStr = LeerAppSetting("numeroIntentosSiError", "5");
                    int numeroIntentos = Convert.ToInt32(numeroIntentosStr);        //Número de intentos por si hay excepción
                    if (numeroIntentos == 0) numeroIntentos = 1;        //Para que se cumpla la condición y se pueda hacer una vez

                    string tiempoEsperaStr = LeerAppSetting("tiempoEsperaSiError", "900000");
                    int tiempoEspera = Convert.ToInt32(tiempoEsperaStr);  ////Tiempo de espera en milisegundos, por defecto 15 min (900 000 milisegundos)

                    while (realizarOtroEnvio && numeroEnvio < numeroIntentos)
                    {
                        if (numeroEnvio > 0) System.Threading.Thread.Sleep(tiempoEspera);
                        //Realiza el Envio Automatico
                        realizarOtroEnvio = RealizarEnvioAuto(numeroEnvio);
                        numeroEnvio++;
                    }
                }
                catch (Exception ex) { if (logActivo == "1") Log.Error(CreateExceptionString(ex)); }

                if (logActivo == "1") Log.Info("FIN SII Envío Automático");
            }
            catch (Exception ex) { if (logActivo == "1") Log.Error(CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Realiza el envio automatico
        /// (devuelve si hubo alguna excepcion en el envio, por si hubiese que hacer otro intento)
        /// </summary>
        /// <returns></returns>
        private static bool RealizarEnvioAuto(int numeroIntento)
        {
            bool existeErrorExcepcion = false;
            try
            {
                int totalFacturasEnviadas = 0;
                int totalAceptadasErrores = 0;
                int totalErroneas = 0;
                int totalNoEnviadas = 0;
                int totalCorrectas = 0;

                //Coger la Fecha y Hora del inicio del envio automático
                string fechaInicioEnvioAutomatico = "";
                string horaInicioEnvioAutomatico = "";
                FechaHoraServidor(ref fechaInicioEnvioAutomatico, ref horaInicioEnvioAutomatico);

                DataSet dsResult = wsTGsii.EnviarTodo(compania, libro, operacion, ejercicio, periodo);

                //Coger la Fecha y Hora del fin del envio automático
                string fechaFinEnvioAutomatico = "";
                string horaFinEnvioAutomatico = "";
                FechaHoraServidor(ref fechaFinEnvioAutomatico, ref horaFinEnvioAutomatico);

                //Grabar la respuesta
                if (resultActivo == "1")
                {
                    string nombreFichero = GenerarNombreFichero(compania, libro, operacion, ejercicio, periodo);
                    if (resultPath != "") nombreFichero = resultPath + "\\" + nombreFichero;
                    System.IO.StreamWriter xmlSW = new System.IO.StreamWriter(nombreFichero);
                    dsResult.WriteXml(xmlSW, XmlWriteMode.WriteSchema);
                    xmlSW.Close();
                }

                //Enviar mail
                if (mailActivo == "1")
                {
                    string estado = "";
                    string estadoEnvioActual = "";

                    bool envioError = false;
                    bool envioCorrecto = false;
                    bool envioParcialmenteCorrecto = false;

                    string estadoEnvioCorrecto = "Correcto";
                    string estadoEnvioParcialmenteCorrecto = "ParcialmenteCorrecto";
                    string estadoEnvioIncorrecto = "Incorrecto";

                    //Procesar el resultado
                    string mensajeBody = LeerAppSetting("mailBody", "En el proceso de suministro automático de facturas al SII se han realizado los envíos de cada uno de los libros de facturas siguientes:");
                    mensajeBody += "\n\n";

                    if (dsResult != null && dsResult.Tables != null)
                    {
                        if (dsResult.Tables.Contains("Resumen"))
                        {
                            bool procesar = true;
                            string[] aClave;
                            string ejerc = "";
                            string peri = "";

                            for (int i = 0; i < dsResult.Tables["Resumen"].Rows.Count; i++)
                            {
                                totalAceptadasErrores = 0;
                                totalErroneas = 0;
                                totalNoEnviadas = 0;

                                totalFacturasEnviadas = 0;
                                totalCorrectas = 0;

                                try { totalAceptadasErrores = Convert.ToInt32(dsResult.Tables["Resumen"].Rows[i]["TotalFactAceptadasError"].ToString()); }
                                catch { }
                                try { totalErroneas = Convert.ToInt32(dsResult.Tables["Resumen"].Rows[i]["TotalFactErrores"].ToString()); }
                                catch { }
                                try { totalNoEnviadas = Convert.ToInt32(dsResult.Tables["Resumen"].Rows[i]["TotalFactNoEnviadas"].ToString()); }
                                catch { }
                                try { totalCorrectas = Convert.ToInt32(dsResult.Tables["Resumen"].Rows[i]["TotalFactCorrectas"].ToString()); }
                                catch { }
                                try { totalFacturasEnviadas = Convert.ToInt32(dsResult.Tables["Resumen"].Rows[i]["TotalFactEnviadas"].ToString()); }
                                catch { }

                                if (!envioError && !envioParcialmenteCorrecto)
                                {
                                    if (totalErroneas > 0 || totalNoEnviadas > 0) envioError = true;
                                    else if (totalAceptadasErrores > 0) envioParcialmenteCorrecto = true;
                                    else if (totalCorrectas > 0) envioCorrecto = true;
                                }

                                if (mailActivoSoloAlerta == "1")
                                {
                                    //Sólo escribir del resumen las alertas (contiene facturas que deben ser revisadas)
                                    if (totalAceptadasErrores > 0 || totalErroneas > 0 || totalNoEnviadas > 0) procesar = true;
                                    else procesar = false;
                                }
                                else procesar = true;

                                if (procesar)
                                {
                                    mensajeBody += "     Compañía: \t\t\t\t" + dsResult.Tables["Resumen"].Rows[i]["Compania"].ToString() + " (" + dsResult.Tables["Resumen"].Rows[i]["NifPresentador"].ToString() + ") \n";
                                    mensajeBody += "     Libro: \t\t\t\t" + dsResult.Tables["Resumen"].Rows[i]["Libro"].ToString() + "\n";
                                    mensajeBody += "     Operación: \t\t\t\t" + dsResult.Tables["Resumen"].Rows[i]["Operacion"].ToString() + "\n";
                                    mensajeBody += "     Total Facturas Enviadas: \t\t" + dsResult.Tables["Resumen"].Rows[i]["TotalFactEnviadas"].ToString() + "\n";
                                    mensajeBody += "     Total Facturas Correctas: \t\t" + dsResult.Tables["Resumen"].Rows[i]["TotalFactCorrectas"].ToString() + "\n";
                                    mensajeBody += "     Total Facturas Aceptadas con Error: \t" + dsResult.Tables["Resumen"].Rows[i]["TotalFactAceptadasError"].ToString() + "\n";
                                    mensajeBody += "     Total Facturas con Errores: \t\t" + dsResult.Tables["Resumen"].Rows[i]["TotalFactErrores"].ToString() + "\n";
                                    mensajeBody += "     Total Facturas No Enviadas: \t\t" + dsResult.Tables["Resumen"].Rows[i]["TotalFactNoEnviadas"].ToString() + "\n";
                                    mensajeBody += "     CSV: \t\t\t\t\t" + dsResult.Tables["Resumen"].Rows[i]["CSV"].ToString() + "\n";

                                    try
                                    {
                                        estadoEnvioActual = "";
                                        string clave = dsResult.Tables["Resumen"].Rows[i].RowError;

                                        if (dsResult.Tables.Contains("Resultado"))
                                        {
                                            for (int j = 0; i < dsResult.Tables["Resultado"].Rows.Count; j++)
                                            {
                                                if (dsResult.Tables["Resultado"].Rows[j].RowError == clave)
                                                {
                                                    estadoEnvioActual = dsResult.Tables["Resultado"].Rows[j]["Estado"].ToString();
                                                    if (!(estadoEnvioActual == estadoEnvioCorrecto ||
                                                        estadoEnvioActual == estadoEnvioParcialmenteCorrecto ||
                                                        estadoEnvioActual == estadoEnvioIncorrecto))
                                                    {
                                                        existeErrorExcepcion = true;
                                                    }
                                                    break;
                                                }
                                            }
                                            mensajeBody += "     Estado del envío: \t\t\t" + estadoEnvioActual + "\n";
                                        }

                                        //ejercicio y periodo
                                        aClave = clave.Split('-');
                                        if (aClave.Length == 5)
                                        {
                                            ejerc = aClave[3].Trim();
                                            if (ejerc != "")
                                            {
                                                mensajeBody += "     Ejercicio: \t\t\t\t" + ejerc + "\n";
                                            }
                                            peri = aClave[4].Trim();
                                            if (peri != "")
                                            {
                                                mensajeBody += "     Periodo: \t\t\t\t" + peri + "\n";
                                            }
                                        }
                                    }
                                    catch (Exception ex) { if (logActivo == "1") Log.Error(CreateExceptionString(ex)); }

                                    mensajeBody += "\n";
                                }
                            }
                        }
                        else
                        {
                            if (dsResult.Tables.Contains("Envio"))
                            {
                                if (dsResult.Tables["Envio"].Rows != null && dsResult.Tables["Envio"].Rows.Count > 0)
                                {
                                    mensajeBody = "El proceso de suministro automático de facturas al SII ha terminado. \n";
                                    string textoMensaje = dsResult.Tables["Envio"].Rows[0]["Mensaje"].ToString().Trim();
                                    mensajeBody += dsResult.Tables["Envio"].Rows[0]["Mensaje"].ToString() + " \n\n";

                                    if (textoMensaje != "No existen facturas pendientes de envio") existeErrorExcepcion = true;
                                }
                                else existeErrorExcepcion = true;
                            }
                            else
                            {
                                existeErrorExcepcion = true;
                                mensajeBody += "No existe info sobre el resumen del envío. Por favor consulte los logs de la aplicación y del servicio web. \n\n";
                            }
                        }
                    }
                    else
                    {
                        existeErrorExcepcion = true;
                        mensajeBody += "No existe resumen del envío. Por favor consulte los logs de la aplicación y del servicio web. \n\n";
                    }

                    mensajeBody += "Inicio del envío:\t\t" + fechaInicioEnvioAutomatico + "    " + horaInicioEnvioAutomatico + "\n";
                    mensajeBody += "Fin del envío:\t\t" + fechaFinEnvioAutomatico + "    " + horaFinEnvioAutomatico + "\n";

                    if (existeErrorExcepcion)
                    {
                        estado = "posibles incidencias, debe revisar el envío";
                        if (numeroIntento > 0) estado += " - intento " + numeroIntento++;
                    }
                    else
                    {
                        if (envioError) estado = "con errores";
                        else if (envioParcialmenteCorrecto) estado = "aceptadas con errores";
                        else if (envioCorrecto) estado = "correcto";
                        else estado = "sin facturas";
                    }

                    //Enviar el mail
                    EnviarMensaje(mensajeBody, estado);
                }
            }
            catch (Exception ex) {
                existeErrorExcepcion = true;
                if (logActivo == "1") Log.Error(CreateExceptionString(ex)); 
            }

            return (existeErrorExcepcion);
        }

        /// <summary>
        /// Lee un parámetro del fichero de configuración
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string LeerAppSetting(string param, string valorDefecto)
        {
            var valor  = ConfigurationManager.AppSettings[param];
            if (!string.IsNullOrEmpty(valor)) return valor.ToString();
            else return (valorDefecto);
        }

        /// <summary>
        /// Recibe una excepción y devuelve una cadena con la info de la excepción
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string CreateExceptionString(Exception e)
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
        /// Generar el nombre de fichero para la Respuesta xml de la petición aitomática de envío
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="libro"></param>
        /// <param name="operacion"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private static string GenerarNombreFichero(string compania, string libro, string operacion, string ejercicio, string periodo)
        {
            string nombre = "";
            try
            {
                if (compania != "" && compania.IndexOf(',') == -1) nombre += "_" + compania;
                if (libro != "" && libro.IndexOf(',') == -1) nombre += "_" + libro;
                if (operacion != "" && operacion.IndexOf(',') == -1) nombre += "_" + operacion;
                if (ejercicio != "" && ejercicio.IndexOf(',') == -1) nombre += "_" + ejercicio;
                if (periodo != "" && periodo.IndexOf(',') == -1) nombre += "_" + periodo;

                DateTime fechaHora = DateTime.Now;
                string fecha = fechaHora.Date.Year.ToString() + "-" + fechaHora.Date.Month.ToString().PadLeft(2, '0') + "-" + fechaHora.Date.Day.ToString().PadLeft(2, '0');
                string hora = fechaHora.TimeOfDay.Hours.ToString().PadLeft(2, '0') + "-" + fechaHora.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + "-" + fechaHora.TimeOfDay.Seconds.ToString().PadLeft(2, '0');
                string fileNameFechaHora = fecha + "_" + hora;

                if (nombre != "") nombre = fileNameFechaHora + nombre + "_Respuesta.xml";
                else nombre = fileNameFechaHora + "_Respuesta.xml";
            }
            catch
            {
                nombre = "Respuesta.xml";
            }
            return (nombre);
        }

        /// <summary>
        /// Devuelve la fecha (dd-mm-aaaa) y la hora (hh:mm:ss) del servidor
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        public static void FechaHoraServidor(ref string fecha, ref string hora)
        {
            try
            {
                DateTime fechaHora = DateTime.Now;
                fecha = fechaHora.Date.Day.ToString().PadLeft(2, '0') + "-" + fechaHora.Date.Month.ToString().PadLeft(2, '0') + "-" + fechaHora.Date.Year.ToString();
                hora = fechaHora.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + fechaHora.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + ":" + fechaHora.TimeOfDay.Seconds.ToString().PadLeft(2, '0');
            }
            catch { }
        }

        /// <summary>
        /// Enviar correo electrónico con el resultado del envío automático
        /// </summary>
        /// <param name="bodyMensaje"></param>
        public static void EnviarMensaje(string bodyMensaje, string estado)
        {
            try
            {
                //Leer Parametrización
                string smtpHost = LeerAppSetting("smtpHost", "");
                string smtpPort = LeerAppSetting("smtpPort", "0");
                string smtpUser = LeerAppSetting("smtpUser", "");
                string smtpPwd = LeerAppSetting("smtpPwd", "");
                string smtpEnableSsl = LeerAppSetting("smtpEnableSsl", "");
                string mailFrom = LeerAppSetting("mailFrom", "");
                string mailTo = LeerAppSetting("mailTo", "");
                string mailSubject = LeerAppSetting("mailSubject", "Resultado Suministro SII");

                if (estado != "") mailSubject  = mailSubject + " (" + estado + ")";

                //Smtp Cliente Parametrización
                SmtpClient server = new SmtpClient();
                server.Host = smtpHost;
                server.Port = Convert.ToInt16(smtpPort);
                //Si el usuario esta informado, instanciar Credentials y la propiedad UseDefaultCredentials con valor false. Sino se dejarán ambas propiedades con el valor por defecto
                if (smtpUser.Trim() != "")
                {
                    server.UseDefaultCredentials = false;
                    server.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPwd);
                }
                //Si la variable smtpEnableSsl está a true, activar la propiedad EnableSsl, sino dejarla con el valor por defecto
                if (smtpEnableSsl == "true") server.EnableSsl = true;
                else server.EnableSsl = false;

                //Mensaje
                MailMessage mensaje = new MailMessage();
                mensaje.Subject = mailSubject;

                if (mailTo.Contains(";"))
                {
                    //Multiples direcciones de envio de alertas envio automatico
                    string[] multi = mailTo.Split(';');
                            
                    foreach (string multiEmail in multi)
                    {
                        mensaje.To.Add(new MailAddress(multiEmail));
                    }
                }
                else mensaje.To.Add(new MailAddress(mailTo));

                mensaje.From = new MailAddress(mailFrom);
                //mensaje.IsBodyHtml -> true si el body se escribe en formato html

                /* Si deseamos Adjuntar algún archivo*/
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));

                //mensaje.Body = " Otro Mensaje de Prueba \n\n Enviado desde C#\n\n *VER EL ARCHIVO ADJUNTO*";
                mensaje.Body = bodyMensaje;

                server.Send(mensaje);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
        }
    }
}
