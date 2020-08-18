using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;

namespace SmartCG
{
    public partial class RadFrmConfig : frmPlantilla, IReLocalizable
    {

        private DataTable dtTiposFichero;

        public RadFrmConfig()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void RadFrmConfig_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Configurar parámetro generales de Smart CG");

            //Crear Tabla Tipo de Transaccion
            this.dtTiposFichero = new System.Data.DataTable();
            this.dtTiposFichero.Columns.Add("valor", typeof(string));
            this.dtTiposFichero.Columns.Add("desc", typeof(string));

            //Crear el desplegable con los tipos de Ficheros
            this.CrearComboTiposFichero();

            //Cargar la información
            this.CargarInformacion();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.GrabarInformacion();
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadFrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.radDropDownListExportarDatosDefecto.SelectedValue.ToString() != this.radDropDownListExportarDatosDefecto.Tag.ToString() ||
                    this.radCheckBoxExportarDatosDefectoView.Checked.ToString() != this.radCheckBoxExportarDatosDefectoView.Tag.ToString() ||
                    this.radCheckBoxSolicitarEntornoInicio.Checked.ToString() != this.radCheckBoxSolicitarEntornoInicio.Tag.ToString())
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonSave.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Configurar parámetro generales de Smart CG");
        }
        #endregion

        #region Métrodos Privados
        /// <summary>
        /// Crea el desplegable de tipos de fichero
        /// </summary>
        private void CrearComboTiposFichero()
        {
            DataRow row;
            int cont = 0;

            try
            {
                if (this.dtTiposFichero.Rows.Count > 0) this.dtTiposFichero.Rows.Clear();

                foreach (ExportFileType tipo in Enum.GetValues(typeof(ExportFileType)))
                {
                    row = this.dtTiposFichero.NewRow();
                    row["valor"] = tipo;
                    row["desc"] = tipo;   //Falta traducir
                    this.dtTiposFichero.Rows.Add(row);
                    cont++;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListExportarDatosDefecto.DataSource = this.dtTiposFichero;
            this.radDropDownListExportarDatosDefecto.ValueMember = "valor";
            this.radDropDownListExportarDatosDefecto.DisplayMember = "desc";
            this.radDropDownListExportarDatosDefecto.Refresh();
            if (cont > 0) this.radDropDownListExportarDatosDefecto.SelectedIndex = 0;
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        private void CargarInformacion()
        {
            try
            {
                try
                {
                    ExportFileType tipoFicheroDefecto = GlobalVar.UsuarioEnv.ExportarTipoFicheroDefecto;
                    this.radDropDownListExportarDatosDefecto.SelectedValue = tipoFicheroDefecto.ToString();
                }
                catch (Exception ex) {Log.Error(Utiles.CreateExceptionString(ex)); }
                this.radDropDownListExportarDatosDefecto.Tag = this.radDropDownListExportarDatosDefecto.SelectedValue.ToString();

                try
                {
                    bool ficheroDefectoMemoria = GlobalVar.UsuarioEnv.ExportarVisualizarFicheroDefecto;
                    this.radCheckBoxExportarDatosDefectoView.Checked = ficheroDefectoMemoria;
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    this.radCheckBoxExportarDatosDefectoView.Checked = true;
                }
                this.radCheckBoxExportarDatosDefectoView.Tag = this.radCheckBoxExportarDatosDefectoView.Checked.ToString();

                try
                {
                    bool cargarListaEntornosInicio = GlobalVar.UsuarioEnv.CargarListaEntornosInicio;
                    this.radCheckBoxSolicitarEntornoInicio.Checked = cargarListaEntornosInicio;
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    this.radCheckBoxSolicitarEntornoInicio.Checked = true;
                }
                this.radCheckBoxSolicitarEntornoInicio.Tag = this.radCheckBoxSolicitarEntornoInicio.Checked.ToString();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba la información
        /// </summary>
        private void GrabarInformacion()
        {
            try
            {
                if (this.radDropDownListExportarDatosDefecto.SelectedValue.ToString() != this.radDropDownListExportarDatosDefecto.Tag.ToString() ||
                    this.radCheckBoxExportarDatosDefectoView.Checked.ToString() != this.radCheckBoxExportarDatosDefectoView.Tag.ToString() ||
                    this.radCheckBoxSolicitarEntornoInicio.Checked.ToString() != this.radCheckBoxSolicitarEntornoInicio.Tag.ToString())
                {
                    if (this.radDropDownListExportarDatosDefecto.SelectedValue.ToString() != this.radDropDownListExportarDatosDefecto.Tag.ToString())
                    {
                        GlobalVar.UsuarioEnv.GrabarUsuario("exportarTipoFicheroDefecto", this.radDropDownListExportarDatosDefecto.SelectedValue.ToString());
                        this.radDropDownListExportarDatosDefecto.Tag = this.radDropDownListExportarDatosDefecto.SelectedValue.ToString();
                        GlobalVar.UsuarioEnv.ExportarTipoFicheroDefecto = (ExportFileType)System.Enum.Parse(typeof(ExportFileType), this.radDropDownListExportarDatosDefecto.SelectedValue.ToString());

                    }
                    if (this.radCheckBoxExportarDatosDefectoView.Checked.ToString() != this.radCheckBoxExportarDatosDefectoView.Tag.ToString())
                    {
                        GlobalVar.UsuarioEnv.GrabarUsuario("exportarVisualizarFicheroDefecto", this.radCheckBoxExportarDatosDefectoView.Checked ? "1" : "0");
                        this.radCheckBoxExportarDatosDefectoView.Tag = this.radCheckBoxExportarDatosDefectoView.Checked.ToString();
                        if (this.radCheckBoxExportarDatosDefectoView.Checked) GlobalVar.UsuarioEnv.ExportarVisualizarFicheroDefecto = true;
                        else GlobalVar.UsuarioEnv.ExportarVisualizarFicheroDefecto = false;
                    }
                    if (this.radCheckBoxSolicitarEntornoInicio.Checked.ToString() != this.radCheckBoxSolicitarEntornoInicio.Tag.ToString())
                    {
                        GlobalVar.UsuarioEnv.GrabarUsuario("solicitarEntornoInicio", this.radCheckBoxSolicitarEntornoInicio.Checked ? "1" : "0");
                        this.radCheckBoxSolicitarEntornoInicio.Tag = this.radCheckBoxSolicitarEntornoInicio.Checked.ToString();
                        if (this.radCheckBoxSolicitarEntornoInicio.Checked) GlobalVar.UsuarioEnv.CargarListaEntornosInicio = true;
                        else GlobalVar.UsuarioEnv.CargarListaEntornosInicio = false;
                    }
                    RadMessageBox.Show("La información se actualizó con éxito.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                RadMessageBox.Show("Error al actualizar la información. Para más información consulte el fichero de Log.");
            }
        }
        #endregion
    }
}
