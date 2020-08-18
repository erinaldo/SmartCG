using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;

namespace SmartCG
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log4net.GlobalContext.Properties["pid"] = System.Diagnostics.Process.GetCurrentProcess().Id;

                //Inicializar el objeto log
                GlobalVar.Log = log4net.LogManager.GetLogger("Generico");

                //Instanciar el usuario que está logado en el PC
                GlobalVar.UsuarioEnv = new Usuario();
                try
                {
                    GlobalVar.UsuarioEnv.LeerInfo();

                    //Instanciar la variable Global LanguageProvider al idioma del usuario
                    GlobalVar.LanguageProvider = GlobalVar.UsuarioEnv.IdiomaApp;
                }
                catch
                {
                    //Log.Error(Utiles.CreateExceptionString(ex));

                    //Instanciar la variable Global LanguageProvider al idioma último seleccionado
                    string idioma = ConfigurationManager.AppSettings["idioma"];
                    GlobalVar.LanguageProvider = idioma;
                }
            }
            catch
            {
                //Si hubo error en iniciar el idioma de la parametrización, poner el castellano por defecto
                GlobalVar.LanguageProvider = "es-Es";

                //Log.Error(Utiles.CreateExceptionString(ex));
            }

            //Inicializar la variable Global UsuarioLogadoCG a vacío
            GlobalVar.UsuarioLogadoCG = "";

            //Inicializa la lista de autorizaciones

            //Application.Run(new frmPrincipal());
            Application.Run(new RadFrmMain());
        }
    }
}
