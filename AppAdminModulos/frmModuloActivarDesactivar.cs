using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppAdminModulos
{
    public partial class frmModuloActivarDesactivar : frmPlantilla
    {
        public frmModuloActivarDesactivar()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmModuloActivarDesactivar_Load(object sender, EventArgs e)
        {
            try
            {
                //Cargar los módulos si aún no se ha hecho
                string result = this.CargarModulos();
                if (result != "")
                {
                    MessageBox.Show("Error cargando los módulos (" + result + ")", "Error");
                    return;
                }
                
                if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0 && dsModulosApp.Tables["Modulo"].Rows.Count > 0)
                {
                    //Cargar la información del DataSet de Módulos al DataGrid
                    dataGridModulos.DataSource = dsModulosApp;
                    dataGridModulos.DataMember = "Modulo";

                    // Set a cell padding to provide space for the top of the focus  
                    // rectangle and for the content that spans multiple columns
                    int CUSTOM_CONTENT_HEIGHT = 15;
                    Padding newPadding = new Padding(0, 1, 0, CUSTOM_CONTENT_HEIGHT);
                    this.dataGridModulos.RowTemplate.DefaultCellStyle.Padding = newPadding;

                    //Ajustar columnas al tamaño de la ventana
                    this.dataGridModulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    //this.dataGridIdiomas.Dock = DockStyle.Fill;

                    this.dataGridModulos.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    this.dataGridModulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Adjust the row heights to accommodate the normal cell content. 
                    this.dataGridModulos.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

                    //Estilo para la fila header
                    this.dataGridModulos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    DataGridViewCellStyle style = this.dataGridModulos.ColumnHeadersDefaultCellStyle;
                    style.BackColor = Color.Navy;
                    style.ForeColor = Color.White;
                    style.Font = new Font(this.dataGridModulos.Font, FontStyle.Bold);

                    //this.dataGridIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    this.dataGridModulos.Columns["id"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //Centrar los textos de la fila de Encabezados
                    this.dataGridModulos.Columns["id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridModulos.Columns["nombre"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridModulos.Columns["nombreDll"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridModulos.Columns["activo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //Textos de la fila del encabezado del DataGrid
                    this.dataGridModulos.Columns["id"].HeaderCell.Value = "Identificador";
                    this.dataGridModulos.Columns["nombre"].HeaderCell.Value = "Nombre";
                    this.dataGridModulos.Columns["nombreDll"].HeaderCell.Value = "Dll";
                    this.dataGridModulos.Columns["activo"].HeaderCell.Value = "Activo";

                    //Columnas de solo lectura
                    this.dataGridModulos.Columns["id"].ReadOnly = true;
                    this.dataGridModulos.Columns["nombre"].ReadOnly = true;
                    this.dataGridModulos.Columns["nombreDll"].ReadOnly = true;

                    //Columnas no visibles
                    this.dataGridModulos.Columns["formulario"].Visible = false;
                    this.dataGridModulos.Columns["imagen"].Visible = false;
                    this.dataGridModulos.Columns["basico"].Visible = false;

                    //Centrar los valores de la columna de identificadores
                    this.dataGridModulos.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //Controles Visibles
                    this.dataGridModulos.Visible = true;
                    this.btnAceptar.Visible = true;

                }
                else
                {
                    //PDTE: informar que no existen módulos dados de alta.
                    this.dataGridModulos.Visible = false;
                    this.btnAceptar.Visible = false;

                    MessageBox.Show("Aún no se han definido módulos. Por favor, vaya a la opción Alta.", "Error");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error (" + ex.Message + ")", "Error");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                bool activo;
                string query = "";
                int cont = 0;
                //Actualizar la tabla de la bbdd y la tabla del DataSet
                for (int i=0; i < this.dataGridModulos.Rows.Count; i++ )
                {
                    activo = (bool)this.dataGridModulos.Rows[i].Cells["activo"].Value;

                    query = "update Modulos set activo = " + (activo ? "1" : "0");
                    query += " where id = " + this.dataGridModulos.Rows[i].Cells["id"].Value.ToString();

                    //Actualizar la bbdd
                    cont = proveedorDatos.ExecuteNonQuery(query, proveedorDatos.GetConnectionValue);

                    //Actualizar el DataSet
                    dsModulosApp.Tables["Modulo"].Rows[i]["activo"] = (bool)this.dataGridModulos.Rows[i].Cells["activo"].Value;
                }

                //dsModulosApp.WriteXml(filePath, XmlWriteMode.IgnoreSchema);

                MessageBox.Show("Los módulos se han actualizado correctamente.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Se ha producido un error (" + ex.Message + ")", "Error");
            }
        }
        #endregion
    }
}
