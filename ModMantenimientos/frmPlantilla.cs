using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using ObjectModel;
using log4net;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;

namespace ModMantenimientos
{
    /// <summary>
    /// Enumera los proveedores soportados
    /// </summary>
    public enum OperacionMtoTipo
    {
        Alta,
        Eliminar,
        Modificar
    }

    public partial class frmPlantilla : Telerik.WinControls.UI.RadForm, IReLocalizable
    {
        protected ILog Log;

        protected LanguageProvider LP;

        protected Utiles utiles;

        protected UtilesCGConsultas utilesCG;

        protected Autorizaciones aut;

        protected string estadoActivo;
        protected string estadoInactivo;
        protected string estadoActiva;
        protected string estadoInactiva;

        //Separador de campos que se utilizará en los objetos de tipo TGTextBoxSel
        protected const string separadorDesc = "-";

        protected string tipoBaseDatosCG = "";

        protected bool selectAll = false;

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

        /*
        public event EventHandler UpdateDataForm;
        protected void DoUpdateDataForm()
        {
            UpdateDataForm(this, new EventArgs());
        }
        */

        public event EventHandler<UpdateDataFormEventArgs> UpdateDataForm;
        protected void DoUpdateDataForm(UpdateDataFormEventArgs e)
        {
            UpdateDataForm?.Invoke(this, e);

            //UpdateDataForm(this, new UpdateDataFormEventArgs());
        }

        /*
        public event DoUpdateDate UpdateDataForm;
        public delegate void DoUpdateDate(UpdateDateFormEventArgs e);

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class UpdateDateFormEventArgs
        {
            public OperacionMtoTipo Operacion { get; protected set; }
            public string Codigo { get; protected set; }

            public UpdateDateFormEventArgs(OperacionMtoTipo operacion, string codigo)
            {
                this.Operacion = operacion;
                this.Codigo = codigo;
            }
        }

        public void DoUpdateDataForm(OperacionMtoTipo operacion, string codigo)
        {
            if (UpdateDataForm != null)
                UpdateDataForm(new UpdateDateFormEventArgs(operacion, codigo));
        }
        */

        public frmPlantilla()
        {
            ThemeResolutionService.ApplicationThemeName = "Office2013Light";

            InitializeComponent();

            //Inicializar el objeto log
            Log = log4net.LogManager.GetLogger(this.GetType());

            //Traducir literales propios de la Grid
            RadGridLocalizationProvider.CurrentProvider = new RadGridLocalizationProviderES();

            //Traducir literales propios de los DataFilter
            DataFilterLocalizationProvider.CurrentProvider = new RadDataFilterLocalizationProviderES();

            this.selectAll = false;
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }

        private void FrmPlantilla_Load(object sender, EventArgs e)
        {
            LP = new LanguageProvider();

            utiles = new Utiles();

            utilesCG = new UtilesCGConsultas();

            aut = new Autorizaciones();

            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

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

            this.estadoActivo = this.LP.GetText("lblEstadoActivo", "Activo");
            this.estadoInactivo = this.LP.GetText("lblEstadoInactivo", "Inactivo");
            this.estadoActiva = this.LP.GetText("lblEstadoActiva", "Activa");
            this.estadoInactiva = this.LP.GetText("lblEstadoInactiva", "Inactiva");
        }

        /// <summary>
        /// Devuelve el literal en el idioma que corresponda, dado el valor que se le pasa
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string TypeSiNoLiteral(string valor)
        {
            string result = "";
            switch (valor)
            {
                case "S":
                    result = this.LP.GetText("lblSi", "Sí");    //Falta traducir
                    break;
                case "N":
                    result = this.LP.GetText("lblNo", "No");    //Falta traducir
                    break;
            }
            return (result);
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        public void ExportarGrid(ref Telerik.WinControls.UI.RadGridView grid, string tituloMto)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            bool ningunaFilaSeleccionada = false;
            if (grid.SelectedRows.Count == 0)
            {
                //Exportar Todo
                grid.SelectAll();
                ningunaFilaSeleccionada = true;
            }
            else
            {
                if (grid.CurrentRow is GridViewGroupRowInfo)
                {
                    if (grid.CurrentRow.IsExpanded) grid.CurrentRow.IsExpanded = false;
                    else grid.CurrentRow.IsExpanded = true;
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            if (grid.CurrentRow is GridViewDataRowInfo || ningunaFilaSeleccionada)
            {
                ExportTelerik exportarConTelerik = new ExportTelerik(ref grid)
                {
                    Titulo = tituloMto,
                    ExportToMemory = GlobalVar.UsuarioEnv.ExportarVisualizarFicheroDefecto,
                    ExportType = GlobalVar.UsuarioEnv.ExportarTipoFicheroDefecto,
                    SelectAll = this.selectAll
                };

                if (exportarConTelerik.ExportType == ExportFileType.EXCEL) exportarConTelerik.NombreHojaExcel_CaptionText = "Mantenimiento";

                string result = exportarConTelerik.Export();
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }
    }

    public class UpdateDataFormEventArgs : EventArgs
    {
        public OperacionMtoTipo Operacion { get; set; }
        public string Codigo{ get; set; }
    }
}
