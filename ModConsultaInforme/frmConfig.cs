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

namespace ModConsultaInforme
{
    public partial class frmConfig : frmPlantilla, IReLocalizable
    {
        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum ConsultaInformeConfigTipo
        {
            Consulta,
            Informe
        }

        private string logTitulo = "";
        private ConsultaInformeConfigTipo configTipo;
        private System.Data.DataTable dtTipoArchivo;

        public string ConfigTipoStr
        {
            set
            {
                switch (value)
                {
                    case "CO":
                        this.configTipo = ConsultaInformeConfigTipo.Consulta;
                        break;
                    case "IN":
                        this.configTipo = ConsultaInformeConfigTipo.Informe;
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
            //Recuperar literales del formulario
            switch (this.configTipo)
            {
                case ConsultaInformeConfigTipo.Consulta:
                    logTitulo = " Consultas";
                    break;
                case ConsultaInformeConfigTipo.Informe:
                    logTitulo = " Informes";
                    break;
            }

            Log.Info("INICIO Configuración" + this.logTitulo);

            this.TraducirLiterales();

            //Leer las variables de configuración de las rutas de las ficheros y de los tipos e incializar los controles
            string pathFicheros = "";
            string tipoFicheros = "";

            switch (this.configTipo)
            {
                case ConsultaInformeConfigTipo.Consulta:
                    pathFicheros = ConfigurationManager.AppSettings["ModConsInfo_PathFicherosConsultas"];
                    tipoFicheros = ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosConsultas"];
                    break;
                case ConsultaInformeConfigTipo.Informe:
                    pathFicheros = ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"];
                    tipoFicheros = ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosInformes"];
                    break;
            }

            //Crear Tabla Tipo de Archivo
            this.dtTipoArchivo= new System.Data.DataTable();
            this.dtTipoArchivo.Columns.Add("valor", typeof(string));
            this.dtTipoArchivo.Columns.Add("desc", typeof(string));

            //Crear el desplegable con el Tipo de Archivo
            this.CrearComboTipoArchivo();

            if (pathFicheros != null && pathFicheros != "") this.txtRutaArchivo.Text = pathFicheros;
            if (tipoFicheros != null && tipoFicheros != "") 
            {
                if (tipoFicheros == "HTML") this.radDropDownListTipoArchivo.SelectedIndex = 1;
                else this.radDropDownListTipoArchivo.SelectedIndex = 0;
            }
            else this.radDropDownListTipoArchivo.SelectedIndex = 0;

            this.txtRutaArchivo.Focus();
            this.txtRutaArchivo.Select();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string pathFicheros = this.txtRutaArchivo.Text.Trim();
                if (pathFicheros == "")
                {
                    RadMessageBox.Show("Debe seleccionar un directorio para los archivos", "Error");
                    this.txtRutaArchivo.Focus();
                    return;
                }

                if (!Directory.Exists(pathFicheros))
                {
                    RadMessageBox.Show("El directorio para los archivos no es válido", "Error");
                    this.txtRutaArchivo.Focus();
                    return;
                }

                string tipoArchivo = "";
                if (this.radDropDownListTipoArchivo.SelectedValue != null) tipoArchivo = this.radDropDownListTipoArchivo.SelectedValue.ToString();
                else tipoArchivo = "EXCEL";

                switch (this.configTipo)
                {
                    case ConsultaInformeConfigTipo.Consulta:
                        utiles.ModificarappSettings("ModConsInfo_PathFicherosConsultas", this.txtRutaArchivo.Text);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModConsInfo_PathFicherosConsultas", this.txtRutaArchivo.Text);
                        GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosConsultas = this.txtRutaArchivo.Text;
                        utiles.ModificarappSettings("ModConsInfo_TipoFicherosConsultas", tipoArchivo);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModConsInfo_TipoFicherosConsultas", tipoArchivo);
                        GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes = tipoArchivo;
                        break;
                    case ConsultaInformeConfigTipo.Informe:
                        utiles.ModificarappSettings("ModConsInfo_PathFicherosInformes", this.txtRutaArchivo.Text);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModConsInfo_PathFicherosInformes", this.txtRutaArchivo.Text);
                        GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosInformes = this.txtRutaArchivo.Text;
                        utiles.ModificarappSettings("ModConsInfo_TipoFicherosInformes", tipoArchivo);
                        GlobalVar.UsuarioEnv.GrabarUsuario("ModConsInfo_TipoFicherosInformes", tipoArchivo);
                        GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes = tipoArchivo;
                        break;
                }

                RadMessageBox.Show("La configuración se guardó correctamente");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                RadMessageBox.Show("Error guardando la configuración " + ex.Message + "error");
            }
        }

        private void RadButtonSelDirectorio_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath();
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonSelDirectorio_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelDirectorio);
        }

        private void RadButtonSelDirectorio_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelDirectorio);
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Configuración" + this.logTitulo);
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            switch (this.configTipo)
            {
                case ConsultaInformeConfigTipo.Consulta:
                    this.radLabelTitulo.Text = "Consultas / Configuración";
                    break;
                case ConsultaInformeConfigTipo.Informe:
                    this.radLabelTitulo.Text = "Informes / Configuración";
                    break;
            }
        }

        /// <summary>
        /// Seleccionar la ruta del fichero que corresponda
        /// </summary>
        private void SeleccionarPath()
        {
            if (this.txtRutaArchivo.Text.Trim() != "") this.radOpenFolderDialog1.InitialDirectory = this.txtRutaArchivo.Text.Trim();
            
            if (this.radOpenFolderDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtRutaArchivo.Text = this.radOpenFolderDialog1.FileName;
            }
        }

        /// <summary>
        /// Crea el desplegable de tipo de archivo
        /// </summary>
        private void CrearComboTipoArchivo()
        {
            DataRow row;

            try
            {
                if (this.dtTipoArchivo.Rows.Count > 0) this.dtTipoArchivo.Rows.Clear();

                row = this.dtTipoArchivo.NewRow();
                row["valor"] = "EXCEL";
                row["desc"] = "EXCEL";
                this.dtTipoArchivo.Rows.Add(row);

                row = this.dtTipoArchivo.NewRow();
                row["valor"] = "HTML";
                row["desc"] = "HTML"; 
                this.dtTipoArchivo.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListTipoArchivo.DataSource = this.dtTipoArchivo;
            this.radDropDownListTipoArchivo.ValueMember = "valor";
            this.radDropDownListTipoArchivo.DisplayMember = "desc";
            this.radDropDownListTipoArchivo.Refresh();
            this.radDropDownListTipoArchivo.SelectedIndex = 0;
        }
        #endregion
    }
}
