using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;
using log4net;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using Telerik.WinControls.FileDialogs;
using System.Configuration;

namespace ModComprobantes
{
    /// <summary>
    /// Enumera los proveedores soportados
    /// </summary>
    public enum OperacionCompTipo
    {
        Alta,
        Eliminar,
        Modificar
    }

    public partial class frmPlantilla : Telerik.WinControls.UI.RadForm
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        protected UtilesCGConsultas utilesCG;

        protected Autorizaciones aut;

        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
        /// </summary>
        public Form FrmPadre
        {
            get
            {
                return (this._frmPadre);
            }
            set
            {
                this._frmPadre = value;
            }
        }

        public event EventHandler<UpdateDataFormEventArgs> UpdateDataForm;
        protected void DoUpdateDataForm(UpdateDataFormEventArgs e)
        {
            UpdateDataForm?.Invoke(this, e);

            //UpdateDataForm(this, new UpdateDataFormEventArgs());
        }

        public frmPlantilla()
        {
            ThemeResolutionService.ApplicationThemeName = "Office2013Light";

            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            //Traducir literales propios del selector de directorios
            FileDialogsLocalizationProvider.CurrentProvider = new RadOpenFolderDialogLocalizationProviderES();

            //Traducir literales propios de la Grid
            RadGridLocalizationProvider.CurrentProvider = new RadGridLocalizationProviderES();

            //Traducir literales propios de los DataFilter
            DataFilterLocalizationProvider.CurrentProvider = new RadDataFilterLocalizationProviderES();
        }

        private void frmPlantilla_Load(object sender, EventArgs e)
        {
            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();

            /*
            //Centrar formulario
            if (this._frmPadre == null)
            {
                //Centrar formulario respecto a la pantalla completa
                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Top = (rect.Height / 2) - (this.Height / 2);
                this.Left = (rect.Width / 2) - (this.Width / 2);
            }
            else
            {
                //Centrar el formulario respecto al formulario padre
                utiles.CentrarFormHijo(this._frmPadre, this);
            }
            */
        }

        /// <summary>
        /// Llenar un Desplegable
        /// </summary>
        /// <param name="query">Sentencia SQL</param>
        /// <param name="campoCodigo">campo codigo de la select</param>
        /// <param name="campoDesc">campo descripción de la select</param>
        /// <param name="control">control ComboBox (se pasa por referencia)</param>
        /// <param name="CodDesc">True si se visualiza codigo - descripcion y False si solol se visualiza descripcion</param>
        /// <param name="indiceSel">Indice del ComboBox que se activará</param>
        /// <returns></returns>
        public string FillComboBox(string query, string campoCodigo, string campoDesc, ref ComboBox control, bool CodDesc, int indiceSel)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                ArrayList elementos = new ArrayList();
                string nombre = "";
                string codigo = "";
                while (dr.Read())
                {
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
                Log.Error(Utiles.CreateExceptionString(ex));

                //Error obteniendo los grupos
                result = ex.Message;

                if (dr != null) dr.Close();
            }

            return (result);
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
        public string FillComboBox(string query, string campoCodigo, string campoDesc, ref RadDropDownList control, bool CodDesc, int indiceSel, bool elementoVacio)
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
                Log.Error(Utiles.CreateExceptionString(ex));

                //Error obteniendo los grupos
                result = ex.Message;

                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores de la tabla FLMITX que se consultan desde la Recepción de Lotes 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ObtenerValoresFLMITXRecepcionLotes()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IDataReader dr = null;
            try
            {
                string prefijoTabla = "";
                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2)
                {
                    prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                    if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                }
                else prefijoTabla = GlobalVar.PrefijoTablaCG;

                string query = "select KEYACC, LITERL from " + prefijoTabla + "FLMITX ";
                query += "where KEYACC in ('L$STESP', 'L$STCON', 'L$STVAL', 'L$STERR', 'L$STADD', 'L$STCOM', 'TÑ0751', 'TÑ0752', 'TÑ0753')";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string key = "";
                while (dr.Read())
                {
                    key = dr.GetValue(dr.GetOrdinal("KEYACC")).ToString().Trim();
                    result.Add(key, dr.GetValue(dr.GetOrdinal("LITERL")).ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            
            return (result);
        }
    }

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

    public class UpdateDataFormEventArgs : EventArgs
    {
        public OperacionCompTipo Operacion { get; set; }
        public string Codigo { get; set; }
    }
}