using System;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace ObjectModel
{
	/// <summary>
	/// Descripción breve de LanguageProvider.
	/// </summary>
	public class LanguageProvider
	{
		private string LanguageFileName = "Language";
		private ResourceManager MyResourceManager;
        public CultureInfo MyCultureInfo;

		public LanguageProvider()
		{
            try
            {
                // Cargamos el archivo de recursos (la del está situada dentro de UniclassPortalServer\bin\<this._cultureInfo>\)
                Assembly MyAssembly = Assembly.GetCallingAssembly();
                string baseName = string.Format("{0}.{1}", MyAssembly.GetName().Name, this.LanguageFileName);
                this.MyResourceManager = new ResourceManager(baseName, MyAssembly);
                this.MyResourceManager.IgnoreCase = true;

                this.MyCultureInfo = new CultureInfo(GlobalVar.LanguageProvider);
                Thread.CurrentThread.CurrentCulture = this.MyCultureInfo;
                Thread.CurrentThread.CurrentUICulture = this.MyCultureInfo;
            }
            catch //(Exception ex)
            {
                //GlobalVar.Log.Error(ex.Message);
                // ¿Hacer algo con el error?
            }
		}

		public string GetText(string key)
		{
			try
			{
				return this.MyResourceManager.GetString(key);
			}
			catch //(Exception ex)
			{
                //GlobalVar.Log.Warn("\"" + key + "\" - "+ ex.Message);
                //GlobalVar.Log.Warn(Utiles.CreateExceptionString(ex));

				return "";
			}
		}
		public string GetText(string key, string defaultValue)
		{
			string TMP = GetText(key);
			return (TMP == "") ? defaultValue : TMP;
		}
	}
}
