using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls;
using System.IO;

namespace ModComprobantes
{
    public partial class frmConfig : frmPlantilla, IReLocalizable
    {
        /// <summary>
        /// Enumera los tipos de comprobantes
        /// </summary>
        public enum ComprobanteTipo
        {
            Contable,
            ExtraContable
        }

        private string titulo;
        private ComprobanteTipo comprobanteTipo;

        public string Titulo
        {
            get
            {
                return (this.titulo);
            }
            set
            {
                this.titulo = value;
            }
        }

        public string ComprobanteTipoStr
        {
            set
            {
                switch (value)
                {
                    case "CO":
                        this.comprobanteTipo = ComprobanteTipo.Contable;
                        break;
                    case "EX":
                        this.comprobanteTipo = ComprobanteTipo.ExtraContable;
                        break;
                }
            }
        }

        public frmConfig()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Configuración " + this.titulo);

            this.TraducirLiterales();

            //Leer las variables de configuración de las rutas de las ficheros e incializar los controles
            switch (this.comprobanteTipo)
            {
                case ComprobanteTipo.Contable:
                    string pathFicherosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];
                    string pathFicherosModelosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosModelosCompContables"];

                    if (pathFicherosCompContables != null && pathFicherosCompContables != "") this.txtComprobante.Text = pathFicherosCompContables;
                    if (pathFicherosModelosCompContables != null && pathFicherosModelosCompContables != "") this.txtModelo.Text = pathFicherosModelosCompContables;
                    break;
                case ComprobanteTipo.ExtraContable:
                    string pathFicherosCompExtraContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtraContables"];
                    string pathFicherosModelosCompExtraContables = ConfigurationManager.AppSettings["ModComp_PathFicherosModelosCompExtraContables"];

                    if (pathFicherosCompExtraContables != null && pathFicherosCompExtraContables != "") this.txtComprobante.Text = pathFicherosCompExtraContables;
                    if (pathFicherosModelosCompExtraContables != null && pathFicherosModelosCompExtraContables != "") this.txtModelo.Text = pathFicherosModelosCompExtraContables;
                    break;
            }

            this.txtComprobante.Focus();
            this.txtComprobante.Select();
        }

        private void BtnSelCompContable_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath(this.txtComprobante);
        }

        private void BtnSelModeloCont_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath(this.txtModelo);
        }
        
        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string pathFicheros = this.txtComprobante.Text.Trim();
                if (pathFicheros == "")
                {
                    RadMessageBox.Show("Debe seleccionar un directorio para los archivos", "Error");
                    this.txtComprobante.Focus();
                    return;
                }

                if (!Directory.Exists(pathFicheros))
                {
                    RadMessageBox.Show("El directorio para los archivos no es válido", "Error");
                    this.txtComprobante.Focus();
                    return;
                }

                string pathModelo = this.txtModelo.Text.Trim();
                if (pathModelo != "" && !Directory.Exists(pathModelo))
                {
                    RadMessageBox.Show("El directorio para los modelos no es válido", "Error");
                    this.txtModelo.Focus();
                    return;
                }

                //Validar el formulario
                switch (this.comprobanteTipo)
                {
                    case ComprobanteTipo.Contable:
                        utiles.ModificarappSettings("ModComp_PathFicherosCompContables", pathFicheros);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModComp_PathFicherosCompContables", pathFicheros);
                        utiles.ModificarappSettings("ModComp_PathFicherosModelosCompContables", pathModelo);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModComp_PathFicherosModelosCompContables", pathModelo);
                        break;
                    case ComprobanteTipo.ExtraContable:
                        utiles.ModificarappSettings("ModComp_PathFicherosCompExtraContables", pathFicheros);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModComp_PathFicherosCompContables", pathFicheros);
                        utiles.ModificarappSettings("ModComp_PathFicherosModelosCompExtraContables", pathModelo);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModComp_PathFicherosModelosCompContables", pathModelo);
                        break;
                }
                RadMessageBox.Show("La configuración se almacenó correctamente", "Resultado");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                RadMessageBox.Show("Error grabando la configuración " + ex.Message, "Error");
            }
        }

        private void RadButtonSelComprobantes_Click(object sender, EventArgs e)
        {
            if (this.txtComprobante.Text.Trim() != "") this.radOpenFolderDialog1.InitialDirectory = this.txtComprobante.Text.Trim();

            this.SeleccionarPath(this.txtComprobante);
        }

        private void RadButtonSelModelos_Click(object sender, EventArgs e)
        {
            if (this.txtModelo.Text.Trim() != "") this.radOpenFolderDialog1.InitialDirectory = this.txtModelo.Text.Trim();

            this.SeleccionarPath(this.txtModelo);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonSelComprobantes_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelComprobantes);
        }

        private void RadButtonSelComprobantes_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelComprobantes);
        }

        private void RadButtonSelModelos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelModelos);
        }

        private void RadButtonSelModelos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelModelos);
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Configuración " + this.titulo);
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmConfigTitulo", "Configuración");
            this.radLabelTitulo.Text = this.titulo + "/ Configuración";
        }

        /// <summary>
        /// Seleccionar la ruta del fichero que corresponda
        /// </summary>
        /// <param name="txtControl"></param>
        private void SeleccionarPath(TextBox txtControl)
        {
            this.folder.Description = this.LP.GetText("lblConfigSelPath", "Seleccionar una carpeta");
            //this.folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.folder.ShowNewFolderButton = false;

            DialogResult result = this.folder.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtControl.Text = this.folder.SelectedPath;
                Environment.SpecialFolder root = folder.RootFolder;
            }
        }

        /// <summary>
        /// Seleccionar la ruta del fichero que corresponda
        /// </summary>
        /// <param name="txtControl"></param>
        private void SeleccionarPath(Telerik.WinControls.UI.RadTextBox txtControl)
        {
            if (this.radOpenFolderDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtComprobante.Text = this.radOpenFolderDialog1.FileName;
            }
        }
        #endregion
    }
}
