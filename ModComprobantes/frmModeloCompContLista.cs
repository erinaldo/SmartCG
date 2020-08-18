using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using ObjectModel;

namespace ModComprobantes
{
    public partial class frmModeloCompContLista : frmPlantilla, IReLocalizable, IFormModelo
    {
        public string formCode = "MMCLISTA";

        DataTable dataTable;
        string pathFicherosModelosCompContables = "";

        public frmModeloCompContLista()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        //Actualizar el listado de modelos de comprobante desde el formulario frmModelosCompContAltaEdita.cs después de un alta o una actualización
        void IFormModelo.ActualizaListaModelosComprobante() 
        {
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
            //this.dgComprobantes.Rows.Clear();
            this.FillDataGrid();
        }

        private void FrmModeloCompContLista_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de modelos de comprobantes contables");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Inicializar los valores del formulario
            //

            //Crear el DataGrid y sus columnas
            this.BuildDataGrid();

            //Llenar el DataGrid
            this.FillDataGrid();

            //Chequear que existan modelos de comprobante
            this.VerificarExistenciaModelosComprobante();

            this.TraducirLiterales();

            //Cargar planes de cuentas
            this.FillPlanes();

            //Ajustar todas las columnas de la Grid
            this.AjustarColumnas();

        }

        private void DgModelosComprobante_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*SMR
            this.EditarModelosComprobante(this.dgModelosComprobante.CurrentRow.Index);
            SMR*/
        }

        private void ToolStripButtonNuevo_Click(object sender, EventArgs e)
        {
            /*SMR
            frmModelosCompContAltaEdita frmAltaEdita = new frmModelosCompContAltaEdita();
            frmAltaEdita.NuevoComprobante = true;
            frmAltaEdita.FrmPadre = this;
            frmAltaEdita.Show(this);
            SMR*/
        }

        private void ToolStripButtonEditar_Click(object sender, EventArgs e)
        {
            /*SMR
            this.EditarModelosComprobante(this.dgModelosComprobante.CurrentRow.Index);
            SMR*/
        }

        private void ToolStripSuprimir_Click(object sender, EventArgs e)
        {
            this.EliminarModelosComprobante();
        }

        private void ToolStripAjustar_Click(object sender, EventArgs e)
        {
            //Ajustar todas las columnas
            this.AjustarColumnas();
        }

        private void ToolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuGridButtonNuevo_Click(object sender, EventArgs e)
        {
            /*SMR
            frmModelosCompContAltaEdita frmAltaEdita = new frmModelosCompContAltaEdita();
            frmAltaEdita.NuevoComprobante = true;
            frmAltaEdita.FrmPadre = this;
            frmAltaEdita.Show(this);
            SMR*/
        }

        private void MenuGridButtonEditar_Click(object sender, EventArgs e)
        {
            if (this.dgModelosComprobante.SelectedRows.Count > 1)
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("lblErrSelSoloUnModeloComp", "Debe seleccionar un solo modelo de comprobante"), error);
            }
            else this.EditarModeloComprobante(this.dgModelosComprobante.CurrentRow.Index);
        }

        private void MenuGridButtonSuprimir_Click(object sender, EventArgs e)
        {
            this.EliminarModelosComprobante();
        }

        private void MenuGridButtonAjustar_Click(object sender, EventArgs e)
        {
            this.AjustarColumnas();
        }

        private void CmbPlan_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracter = e.KeyChar.ToString().ToUpper();
            e.KeyChar = Convert.ToChar(caracter);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmModelosCompContListaTitulo", "Lista de Modelos de Comprobantes Contables");

            this.toolStripButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.toolStripButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.toolStripButtonSuprimir.Text = this.LP.GetText("toolStripSuprimir", "Suprimir");
            this.toolStripButtonAjustar.Text = this.LP.GetText("toolStripAjustar", "Ajustar");
            this.toolStripButtonImprimir.Text = this.LP.GetText("toolStripImprimir", "Imprimir");
            this.toolStripButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");

            this.menuGridButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.menuGridButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.menuGridButtonSuprimir.Text = this.LP.GetText("toolStripSuprimir", "Suprimir");
            this.menuGridButtonAjustar.Text = this.LP.GetText("toolStripAjustar", "Ajustar");
            this.menuGridButtonImprimir.Text = this.LP.GetText("toolStripImprimir", "Imprimir");

            //Traducir los literales de los encabezados de las columnas
            this.TraducirLiteralesDataGridHeader();
        }

        /// <summary>
        /// Construye el dataGrid
        /// </summary>
        private void BuildDataGrid()
        {
            this.dataTable = new DataTable();

            try
            {
                //Adicionar las columnas al DataGrid
                this.AddColumn("archivo", typeof(System.String));
                this.AddColumn("descripcion", typeof(System.String));
                this.AddColumn("plan", typeof(System.String));
                this.AddColumn("referencia", typeof(System.String));
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            this.dgModelosComprobante.DataSource = this.dataTable;
        }

        /// <summary>
        /// Traduce los literales del Encabezado del DataGrid
        /// </summary>
        private void TraducirLiteralesDataGridHeader()
        {
            this.dgModelosComprobante.Columns["archivo"].HeaderText = this.LP.GetText("ModeloCompContdgHeaderArchivo", "Archivo");
            this.dgModelosComprobante.Columns["descripcion"].HeaderText = this.LP.GetText("ModeloCompContdgHeaderDescripcion", "Descripción");
            this.dgModelosComprobante.Columns["plan"].HeaderText = this.LP.GetText("ModeloCompContdgHeaderPlan", "Plan");
            this.dgModelosComprobante.Columns["referencia"].HeaderText = this.LP.GetText("ModeloCompContdgHeaderReferencia", "Referencia");
        }

        private void AddColumn(string nombre, Type tipo)
        {
            DataColumn col = new DataColumn(nombre, tipo);
            //string headerColumna = this.LP.GetText(literalIdioma, literalIdiomaDefecto);
            //col.Caption = headerColumna;
            //this.dgComprobantes.Columns[nombre].HeaderText = headerColumna;
            this.dataTable.Columns.Add(col);
        }

        /// <summary>
        /// Rellena el DataGrid con la información correspondiente
        /// </summary>
        private void FillDataGrid()
        {
            //Leer todos los ficheros con extensión xml que existan dentro de la carpeta ModComp_PathFicherosCompContables

            try
            {
                pathFicherosModelosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosModelosCompContables"];
                DirectoryInfo dir = new DirectoryInfo(pathFicherosModelosCompContables);
                FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo FI in fileList)
                {
                    this.ProcesarFichero(FI.FullName, FI.Name);
                    //Console.WriteLine(FI.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Dado un fichero de comprobante, busca su información y la adiciona en la Grid de comprobantes
        /// </summary>
        /// <param name="pathFichero">ruta de los ficheros de comprobantes contables</param>
        /// <param name="nombreFichero">nombre del fichero que contiene el comprobante contable</param>
        private void ProcesarFichero(string pathFichero, string nombreFichero)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(pathFichero);

                //Verificar que exista la tabla de Cabecera
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                {
                    //Verificar la marca contable para asegurar que sea un comprobante contable
                    if (ds.Tables["Cabecera"].Rows[0]["ModeloContable"].ToString() == "1")
                    {
                        string descripcion = "";
                        string plan = "";
                        string referencia = "";

                        descripcion = ds.Tables["Cabecera"].Rows[0]["Descripcion"].ToString();
                        plan = ds.Tables["Cabecera"].Rows[0]["Plan"].ToString();
                        referencia = ds.Tables["Cabecera"].Rows[0]["Referencia"].ToString();

                        //Insertar una fila en el DataTable
                        DataRow dr = this.dataTable.NewRow();
                        dr["archivo"] = nombreFichero;
                        dr["descripcion"] = descripcion;
                        dr["plan"] = plan;
                        dr["referencia"] = referencia;

                        this.dataTable.Rows.Add(dr);

                        //Refrescar el DataGrid para que inserte la nueva fila
                        this.dgModelosComprobante.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// Verifica que existan comprobantes. Si no existen, lo informa en el formulario
        /// </summary>
        private void VerificarExistenciaModelosComprobante()
        {
            try
            {
                if (this.dgModelosComprobante.Rows.Count <= 0)
                {
                    this.toolStrip1.Items["toolStripButtonEditar"].Enabled = false;
                    this.toolStrip1.Items["toolStripButtonSuprimir"].Enabled = false;
                    this.toolStrip1.Items["toolStripButtonImprimir"].Enabled = false;
                    this.toolStrip1.Items["toolStripButtonAjustar"].Enabled = false;
                    this.dgModelosComprobante.Visible = false;

                    this.lblNoHayModeloComp.Text = this.LP.GetText("errNoExistenModelosDeComprobante", "No existen modelos de comprobante en la ruta") + " " + pathFicherosModelosCompContables;
                    this.lblNoHayModeloComp.Visible = true;

                    //Desactivar los controles del buscador
                    this.ActivarDesactivarBuscador(false);
                }
                else
                {
                    this.toolStrip1.Items["toolStripButtonEditar"].Enabled = true;
                    this.toolStrip1.Items["toolStripButtonSuprimir"].Enabled = true;
                    this.toolStrip1.Items["toolStripButtonImprimir"].Enabled = true;
                    this.toolStrip1.Items["toolStripButtonAjustar"].Enabled = true;
                    this.dgModelosComprobante.Visible = true;

                    this.lblNoHayModeloComp.Text = "";
                    this.lblNoHayModeloComp.Visible = false;

                    //Desactivar los controles del buscador
                    this.ActivarDesactivarBuscador(true);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

        }

        /// <summary>
        /// Activa o desactiva los controles del buscador de comprobantes
        /// </summary>
        /// <param name="valor">true -> activa los controles  false -> desactiva los controles</param>
        private void ActivarDesactivarBuscador(bool valor)
        {

            lblDescripcion.Visible = valor;
            lblPlan.Visible = valor;
            lblReferencia.Visible = valor;

            txtDescripcionBuscador.Visible = valor;
            txtReferenciaBuscador.Visible = valor;

            cmbPlan.Visible = valor;

            this.btnBuscar.Visible = valor;
            this.btnTodos.Visible = valor;
        }

        /// <summary>
        /// Cargar los planes de cuentas
        /// </summary>
        private void FillPlanes()
        {
            string query = "Select TIPLMP, NOMBMP From " + GlobalVar.PrefijoTablaCG + "GLM02 Order by TIPLMP";
            string result = this.FillComboBox(query, "TIPLMP", "NOMBMP", ref this.cmbPlan, true, -1);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetPlanes", "Error obteniendo los planes de cuentas") + " (" + result + ")";
                MessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Ajusta las columnas de la Grid
        /// </summary>
        private void AjustarColumnas()
        {
            //Ajustar todas las columnas de la Grid
            //utiles.AjustarColumnasGrid(ref dgComprobantes, -1);

            //Ajustar todas las columnas
            for (int i = 0; i < this.dgModelosComprobante.ColumnCount; i++)
            {
                this.dgModelosComprobante.AutoResizeColumn(i);
            }
        }

        /// <summary>
        /// Elimina los comprobantes seleccionados
        /// </summary>
        private void EliminarModelosComprobante()
        {
            try
            {
                bool varios = false;
                if (this.dgModelosComprobante.SelectedRows.Count > 1) varios = true;  //Varios comprobantes
                else varios = false;    //Un solo comprobante

                //Pedir confirmación
                string mensaje = "";
                string advertencia1 = "";
                string advertencia2 = this.LP.GetText("wrnSuprimirModelosComprobantePregunta", "¿Desea continuar?");

                string archivo = "";

                if (varios)
                {
                    advertencia1 = this.LP.GetText("wrnSuprimirVariosModelosComprobantes", "modelos de comprobante se van a eliminar");
                    mensaje = this.dgModelosComprobante.SelectedRows.Count.ToString() + " " + advertencia1 + ". " + advertencia2;
                }
                else
                {
                    advertencia1 = this.LP.GetText("wrnSuprimirModeloComprobante", "Se va a eliminar el modelo de comprobante");
                    archivo = this.dgModelosComprobante.Rows[this.dgModelosComprobante.CurrentRow.Index].Cells["archivo"].Value.ToString();
                    mensaje = advertencia1 + " \"" + archivo + " \"." + advertencia2;
                }

                string advertenciaTitulo = this.LP.GetText("wrnTitulo", "Advertencia");
                DialogResult res = MessageBox.Show(mensaje, advertenciaTitulo, MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {

                    //Suprimir todos los comprobantes seleccionados
                    foreach (DataGridViewRow row in this.dgModelosComprobante.SelectedRows)
                        if (!row.IsNewRow)
                        {
                            archivo = this.dgModelosComprobante.Rows[row.Index].Cells["archivo"].Value.ToString();
                            //Eliminar el archivo físico del comprobante
                            try
                            {
                                string fichero = ConfigurationManager.AppSettings["ModComp_PathFicherosModelosCompContables"] + "\\" + archivo;
                                File.Delete(fichero);
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.dgModelosComprobante.Rows.Remove(row);
                        }

                    //Verificar existencia de comprobantes
                    this.VerificarExistenciaModelosComprobante();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errDelModelosComp", "Error eliminando modelos de comprobante") + " (" + ex.Message + ")", error);
            }
        }

        /// <summary>
        /// Llama al formulario de Editar Comprobante
        /// </summary>
        /// <param name="rowIndex">índice de la fila seleccionada de la Grid de comprobantes</param>
        private void EditarModeloComprobante(int rowIndex)
        {
            string archivo = this.dgModelosComprobante.Rows[rowIndex].Cells["archivo"].Value.ToString();
            string descripcion = this.dgModelosComprobante.Rows[rowIndex].Cells["descripcion"].Value.ToString();

            /*frmCompContAltaEdita frmAltaEdita = new frmCompContAltaEdita();
            frmAltaEdita.NuevoComprobante = false;
            frmAltaEdita.NombreComprobante = descripcion;
            frmAltaEdita.ArchivoComprobante = archivo;
            frmAltaEdita.Show();*/
        }

        /// <summary>
        /// Elimina el comprobante seleccionado
        /// </summary>
        /// <param name="rowIndex">índice de la fila seleccionada de la Grid de comprobantes</param>
        private void SuprimirModeloComprobante(int rowIndex)
        {
            //Pedir confirmación
            string descripcion = this.dgModelosComprobante.Rows[rowIndex].Cells["descripcion"].Value.ToString();
            string archivo = this.dgModelosComprobante.Rows[rowIndex].Cells["archivo"].Value.ToString();

            string advertenciaTitulo = this.LP.GetText("wrnTitulo", "Advertencia");
            string advertencia1 = this.LP.GetText("wrnSuprimirModeloComprobante", "Se va a eliminar el modelo de comprobante");
            string advertencia2 = this.LP.GetText("wrnSuprimirModeloComprobantePregunta", "¿Desea continuar?");
            DialogResult res = MessageBox.Show(advertencia1 + " \"" + archivo + " \"." + advertencia2, advertenciaTitulo, MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                //Eliminar el modelo de comprobante de la Grid
                for (int i = 0; i < this.dataTable.Rows.Count; i++)
                {
                    if (this.dataTable.Rows[i]["archivo"].ToString() == archivo)
                    {
                        this.dataTable.Rows.RemoveAt(i);
                        this.dgModelosComprobante.Refresh();
                        break;
                    }
                }

                //Eliminar el archivo físico del modelo de comprobante
                try
                {
                    string fichero = ConfigurationManager.AppSettings["ModComp_PathFicherosModelosCompContables"] + "\\" + archivo;
                    File.Delete(fichero);
                }
                catch
                {
                }

                //Verificar existencia de los modelos de comprobante
                this.VerificarExistenciaModelosComprobante();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Filtrar la Grid
                string filtro = "";
                string descripcion = "";
                string referencia = "";
                string plan = "";

                descripcion = this.txtDescripcionBuscador.Text.Trim();
                
                referencia = this.txtReferenciaBuscador.Text.Trim();

                plan = this.cmbPlan.Text.Trim();
                if (plan.Length > 1) plan = plan.Substring(0, 1);

                string errorTitulo = this.LP.GetText("errValTitulo", "Error");


                if (descripcion != "")
                {
                    filtro = string.Format("descripcion LIKE '%{0}%'", descripcion);
                }

                if (referencia != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("referencia LIKE '%{0}%'", referencia);
                }

                if (plan != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("plan LIKE '%{0}%'", plan);
                }
                
                this.dataTable.DefaultView.RowFilter = filtro;
                this.dgModelosComprobante.Refresh();

                if (this.dataTable.DefaultView.Count == 0 && filtro != "")
                {
                    this.dgModelosComprobante.Visible = false;

                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(this.LP.GetText("lblErrNoModeloComp", "No existen modelos de comprobante para el criterio de búsqueda utilizado"), error);
                    this.txtDescripcionBuscador.Select();
                }
                else this.dgModelosComprobante.Visible = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            this.cmbPlan.Text = "";
            this.cmbPlan.SelectedIndex = -1;
            
            this.txtDescripcionBuscador.Text = "";

            this.txtReferenciaBuscador.Text = "";

            this.dataTable.DefaultView.RowFilter = "";
            this.dgModelosComprobante.Visible = true;
            this.Refresh();
        }

        private void frmModeloCompContLista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) ToolStripButtonSalir_Click(sender, null);
        }

        private void frmModelosCompContLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de modelos de comprobante contable");
        }

        #endregion
    }
}