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
using System.Text.RegularExpressions;
using System.Collections;
using ObjectModel;
using Telerik.WinControls;

namespace ModComprobantes
{
    public partial class frmCompExtContAltaEdita : frmPlantilla, IReLocalizable
    {
        protected bool menuLateralExpanded = true;
        protected static int collapseWidth = 0;

        private bool nuevoComprobante;
        private bool importarComprobante;
        private string nombreComprobante;
        private string archivoComprobante;
        private ComprobanteExtContable comprobanteExtContableImportar;

        private DataSet ds;
        
        private Telerik.WinControls.UI.RadLabel lblTotalDebe;
        private Telerik.WinControls.UI.RadLabel lblTotalHaber;
        private Telerik.WinControls.UI.RadLabel lblMonedaLocal;

        private Telerik.WinControls.UI.RadLabel lblMonedaLocal_Debe;
        private Telerik.WinControls.UI.RadLabel lblMonedaLocal_Haber;

        private Telerik.WinControls.UI.RadLabel lblNoApuntes;
        private Telerik.WinControls.UI.RadLabel lblNoApuntes_Valor;
        
        //Valores necesarios de la compañía
        private string GLM01_NCIAMG;
        private string GLM01_TITAMG;
        private string GLM01_FELAMG;
        private string GLM01_TIPLMG;

        //Descripción del tipo
        private string GLT06_NOMBTV;

        private const string separadorDesc = "-";

        private const string prefijoColumnaPeriodo = "PRD";
        private const string cuentaAuxiliarGlobal = "99999999";

        //TextBox que se utiliza en el DataGridView de Detalles para validar que las monedas y el importe sean sólo numéricos
        TextBox tb;

        private ComprobanteExtContable comp = null;

        private string tipoBaseDatosCG = "";

        private ArrayList periodosGenerados;

        private bool gridChange = false;

        private DataTable dtImporte;

        public bool NuevoComprobante
        {
            get
            { 
                return (this.nuevoComprobante);
            }
            set
            {
                this.nuevoComprobante = value;
            }
        }

        public bool ImportarComprobante
        {
            get
            {
                return (this.importarComprobante);
            }
            set
            {
                this.importarComprobante = value;
            }
        }

        public string NombreComprobante
        {
            get
            {
                return (this.nombreComprobante);
            }
            set
            {
                this.nombreComprobante = value;
            }
        }

        public string ArchivoComprobante
        {
            get
            {
                return (this.archivoComprobante);
            }
            set
            {
                this.archivoComprobante = value;
            }
        }

        public ComprobanteExtContable ComprobanteExtContableImportar
        {
            get
            {
                return (this.comprobanteExtContableImportar);
            }
            set
            {
                this.comprobanteExtContableImportar = value;
            }
        }

        public frmCompExtContAltaEdita()
        {
            InitializeComponent();

            periodosGenerados = new ArrayList();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbCabecera.ElementTree.EnableApplicationThemeName = false;
            this.gbCabecera.ThemeName = "ControlDefault";

            this.gbTotales.ElementTree.EnableApplicationThemeName = false;
            this.gbTotales.ThemeName = "ControlDefault";

            this.grBoxProgressBar.ElementTree.EnableApplicationThemeName = false;
            this.grBoxProgressBar.ThemeName = "ControlDefault";

            this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.InnerColor = Color.Transparent;
            this.radBtnMenuMostrarOcultar.ElementTree.EnableApplicationThemeName = false;
            //this.radBtnMenuMostrarOcultar.ThemeName = "Office2013Light";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales(true);
        }

        private void FrmCompExtContAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Alta / Edita comprobantes extracontables");

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Cargar compañías
            this.FillCompanias();

            //Cargar Tipos
            this.FillTiposComprobantes();

            //Cargar Tipos extracontable por defecto
            this.FillTiposDefecto();

            //Crear el datatimepicker con el formato para las fechas que está parametrizado en el CG
            this.CrearFechaConFormatoCG();

            //Crear Tabla de Importes
            this.dtImporte = new DataTable();
            this.dtImporte.Columns.Add("valor", typeof(string));
            this.dtImporte.Columns.Add("desc", typeof(string));

            //Cargar importes
            this.CrearComboImportes();

            //Crear las etiquetas para la tabla de totales
            this.CrearTablaTotales();

            //Crear el Data Grid
            this.CrearDataGrid();

            if (this.importarComprobante)
            {
                this.CrearTablasDataSetVacias();
                //Inicializar los valores
                this.ImportarDatosComprobante();
                //Calcular los totales
                this.CalcularTotales();
            }
            else
            if (!this.nuevoComprobante)
            {
                this.CargarDatosComprobante();

                //Actualizar los atributos TAG de los controles de la cabecera
                ActualizaValoresOrigenControles();

                this.ControlesHabilitarDeshabilitar(true);
            }
            else
            {
                dateTimePickerFecha.Value = DateTime.Now;
                this.CrearTablasDataSetVacias();
                this.ControlesHabilitarDeshabilitar(false);
                this.radDropDownListImportes.SelectedValue = "1";
            }

            //XX-9999999

            //El botón de selección tiene que estar por encima de la Grid
            this.btnSel.BringToFront();

            //Poner en el idioma correspondiente todos los literales
            this.TraducirLiterales(false);

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
            // DUDA this.tgTexBoxSelCompania.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelCompania_ValueChangedFormClose);

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
            // DUDA this.tgTexBoxSelTipo.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelTipo_ValueChanged);

            //this.dgDetalle.CurrentCell = this.dgDetalle[4, 0];
            this.dgDetalle.ClearSelection();
            //this.dgDetalle.Width = (this.dgDetalle.Width - this.dgDetalle.RowHeaderInitWidth) / this.dgDetalle.Columns.Count;

            //Ocultar el botón de selección de la Grid
            this.btnSel.Visible = false;

            //Ajustar las columnas de la Grid de Detalles
            //utiles.AjustarColumnasGrid(ref this.dgDetalle, -1);
            //this.dgDetalle.Refresh();

            //En el menú derecho en el elemento de insertar filas, poner por defecto 1 fila (1er número del desplegable)
            this.toolStripInsertarFilacmbFilas.SelectedIndex = 0;

            //Ajustar las columnas de la Grid al tamaño de la Grid si son pocas
            if (dgDetalle.ColumnCount <= 8) this.dgDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            else this.dgDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            this.menuGridButtonBuscar.Visible = false;

            this.WindowState = FormWindowState.Maximized;

            this.cmbCompania.Select();
        }
        
        private void MenuGridButtonInsertarFila_Click(object sender, EventArgs e)
        {
            if (this.toolStripInsertarFilacmbFilas.SelectedItem != null)
            {
                this.dgDetalle.InsertarFila(Convert.ToInt16(this.toolStripInsertarFilacmbFilas.SelectedItem));
                this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
            }            
        }

        private void MenuGridButtonSuprimirFila_Click(object sender, EventArgs e)
        {
            if (dgDetalle.IsCurrentCellInEditMode == false)
            {
                this.dgDetalle.SuprimirFila();
                this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                this.dgDetalle.ClearSelection();

                //Recalcular Totales
                this.CalcularTotales();

                this.btnSel.Visible = false;
            }
        }

        private void MenuGridButtonAdicionarFila_Click(object sender, EventArgs e)
        {
            this.dgDetalle.AdicionarFila();
            this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
        }

        private void MenuGridButtonCopiar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.CopiarDetalles();
            this.dgDetalle.Refresh();
        }

        private void MenuGridButtonPegar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.PegarDetalles();
            /*
            //Actualizar columnas
            //Habilitar / Deshabilitar Columnas
            for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
            {
                this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
            }*/
            this.dgDetalle.Refresh();
        }

        private void MenuGridButtonCortar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.CortarDetalles();
            this.dgDetalle.Refresh();
        }

        private void MenuGridButtonBorrar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.BorrarDetalles();
            this.dgDetalle.Refresh();
        }

        private static int DataGridViewRowIndexCompare(DataGridViewRow x, DataGridViewRow y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Index.CompareTo(y.Index);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.Index.CompareTo(y.Index);
                    }
                }
            }
        }
        
        private void BtnSel_Click(object sender, EventArgs e)
        {
            DataGridViewCell celdaActiva = this.dgDetalle.CurrentCell;
            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;

            string columnName = this.dgDetalle.Columns[celdaActiva.ColumnIndex].Name;

            string titulo = "";
            string error = "";
            string query = QuerySelElementos(celdaActiva.ColumnIndex, celdaActiva.RowIndex, ref titulo, ref error, columnName);

            if (error == "")
            {
                TGElementosSel frmElementosSel = new TGElementosSel
                {
                    TituloForm = titulo,

                    //Coordenadas donde aparecerá el formulario para seleccionar los elementos
                    /*int coordX = 0;
                    int coordY = 0;
                    if (this.btnSel.Location.X + this.btnSel.Width + frmElementosSel.Size.Width <= this.Size.Width) coordX = this.btnSel.Location.X + this.btnSel.Width;
                    else coordX = this.btnSel.Location.X - frmElementosSel.Size.Width;

                    if (this.btnSel.Location.Y + frmElementosSel.Size.Height <= this.Size.Height) coordY = this.btnSel.Location.Y;
                    else coordY = this.btnSel.Location.Y - frmElementosSel.Size.Height;

                    //frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 95, this.btnSel.Location.Y + 98);
                    //frmElementosSel.LocationForm = new Point(coordX, coordY);
                    */
                    //Centrar el formulario
                    CentrarForm = true,
                    FrmPadre = this,

                    //Pasar la conexión a la bbdd
                    ProveedorDatosForm = GlobalVar.ConexionCG,
                    //Consulta que se ejecutará para obtener los elementos
                    Query = query
                };
                //Definir la cabecera de las columnas
                ArrayList nombreColumnas = new ArrayList
                {
                    this.LP.GetText("lblListaCampoCodigo", "Código"),
                    this.LP.GetText("lblListaCampoDescripcion", "Descripción")
                };
                frmElementosSel.ColumnasCaption = nombreColumnas;

                //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
                //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
                frmElementosSel.OkForm += new TGElementosSel.OkFormCommandEventHandler(FrmElementosSel_OkForm);
                frmElementosSel.ShowDialog();
            }

            this.dgDetalle.Focus();
        }

        private void FrmElementosSel_OkForm(TGElementosSel.OkFormCommandEventArgs e)
        {
            DataGridViewCell celdaActiva = this.dgDetalle.CurrentCell;
            this.dgDetalle.CurrentCell.Value = e.Valor[0].ToString().Trim();

            DataGridViewCellEventArgs arg = new DataGridViewCellEventArgs(celdaActiva.ColumnIndex, celdaActiva.RowIndex);
            //dgDetalle_CellLeave(this.dgDetalle, arg);
            this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex + 1, celdaActiva.RowIndex];
        }

        private void TxtMaskAAPP_TextChanged(object sender, EventArgs e)
        {
            txtMaskAAPP.Modified = true;
        }

        private void TxtMaskAAPP_Enter(object sender, EventArgs e)
        {
            txtMaskAAPP.Modified = false;
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TgTexBoxSelExtDefecto_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtNoComprobante_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void CmbClase_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtTasa_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtDescripcion_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void DateTimePickerFecha_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void DgDetalle_KeyDown(object sender, KeyEventArgs e)
        {
            //tecla suprimir
            if (e.KeyCode == Keys.Delete)
            {
                //no en edicion 
                if (dgDetalle.IsCurrentCellInEditMode == false && this.dgDetalle.SelectedRows.Count > 0)
                {
                    this.dgDetalle.SuprimirFila();

                    /*
                    //borrar toda la seleccion
                    for (int i = 0; i < this.dgDetalle.SelectedCells.Count; i++)
                    {
                        int row = this.dgDetalle.SelectedCells[i].RowIndex;
                        int col = this.dgDetalle.SelectedCells[i].ColumnIndex;
                        this.dgDetalle.Rows[row].Cells[col].Value = "";
                    }
                     */
                }
                else
                {
                    for (int i = 0; i < this.dgDetalle.SelectedCells.Count; i++)
                    {
                        this.dgDetalle.SelectedCells[i].Value = "";
                    }
                }
                this.dgDetalle.Refresh();
            }
            else
                if (e.Control && e.KeyCode == Keys.C)
                {
                    this.dgDetalle.CopiarDetalles();
                }
                else
                    if (e.Control && e.KeyCode == Keys.V)
                    {
                        this.dgDetalle.PegarDetalles();

                        //Actualizar columnas
                        //Habilitar / Deshabilitar Columnas
                        for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                        {
                            this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                        }
                        this.dgDetalle.Refresh();
                    }
                    else
                        if (e.Control && e.KeyCode == Keys.X)
                        {
                            this.dgDetalle.CortarDetalles();
                            this.dgDetalle.Refresh();
                        }

            this.gridChange = true;
            this.CalcularTotales();
        }

        private void DgDetalle_CurrentCellChanged(object sender, EventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    //bool columnaProcesada = false;
                    switch (columnName)
                    {
                        case "Cuenta":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            //this.btnSel.Select();
                            //columnaProcesada = true;
                            break;
                        case "DH" :
                            //this.CalcularTotales();
                            //columnaProcesada = true;
                            break;
                        case "TipoExt":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            //columnaProcesada = true;
                            break;
                        default:
                            this.btnSel.Visible = false;
                            break;
                    }

                    /*
                    if (!columnaProcesada && columnName.Length > 3)
                    {
                        //Verificar si es una columna variable para recalcular los totales
                        if (columnName.Substring(0, 3) == prefijoColumnaPeriodo)
                        {
                            this.CalcularTotales();
                        }
                    }
                     */ 
                }
            } 
        }

        private void DgDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                this.gridChange = true;

                string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                bool columnaProcesada = false;
                switch (columnName)
                {
                    case "Cuenta":
                        columnaProcesada = true;
                        break;
                    case "DH":
                        columnaProcesada = false;
                        break;
                    case "TipoExt":
                        columnaProcesada = true;
                        break;
                }

                if (!columnaProcesada && columnName.Length > 3)
                {
                    //Verificar si es una columna variable para recalcular los totales
                    if (columnName.Substring(0, 3) == prefijoColumnaPeriodo)
                    {
                        try
                        {
                            string valorStr = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[columnName].Value.ToString().Trim();
                            decimal valor = 0;
                            if (valorStr != "")
                            {
                                try
                                {
                                    valor = Convert.ToDecimal(valorStr);
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            }
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[columnName].Value = valor.ToString("N2", this.LP.MyCultureInfo);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        this.CalcularTotales();
                    }
                }                
            }

            //SMR - necesario para que al validar tome el ultimo valor de la celda, no el anterior
            this.dgDetalle.BindingContext[this.dgDetalle.DataSource].EndCurrentEdit();
        }

        private void DgDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (e.ColumnIndex == -1)
                {
                    this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (e.RowIndex != -1)
                    {
                        this.dgDetalle.Rows[e.RowIndex].Selected = true;
                        this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex, e.RowIndex];
                    }
                    return;
                }

                if (e.RowIndex == -1)
                {
                    //this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                    this.dgDetalle.Columns[e.ColumnIndex].Selected = true;
                    this.dgDetalle.CurrentCell = this.dgDetalle[e.ColumnIndex, celdaActiva.RowIndex];
                    return;
                }

                this.dgDetalle.SelectionMode = DataGridViewSelectionMode.CellSelect;
                this.dgDetalle.ClearSelection();
                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex, celdaActiva.RowIndex];

                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    switch (columnName)
                    {
                        case "Cuenta":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            //this.btnSel.Select();
                            break;
                        case "Auxiliar1":
                        case "Auxiliar2":
                        case "Auxiliar3":
                            //Verificar que la cuenta de mayor se ha introducido
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Cuenta"].Value.ToString() != "")
                            {
                                //Mostrar el botón de Selección en las coordenadas que le corresponde
                                this.BtnSelPosicion(tgGridDetalles);
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = false;
                                //this.btnSel.Select();
                            }
                            else
                            {
                                this.btnSel.Visible = false;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = true;
                            }
                            break;
                        case "TipoExt":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        default:
                            this.btnSel.Visible = false;
                            break;
                    }
                }
            }
        }

        private void DgDetalle_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_Sorted(object sender, EventArgs e)
        {
            //Ocultar el botón ??? OJO ???
            //this.btnSel.Visible = false;

            //O posicionarlo en la celda [0,0]   ??? OJO ???
            this.dgDetalle.CurrentCell = this.dgDetalle[0, 0];
            this.dgDetalle.Rows[0].Cells[0].Selected = true;
            this.dgDetalle.Focus();
            this.BtnSelPosicion(this.dgDetalle);
        }

        private void DgDetalle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = this.dgDetalle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)this.dgDetalle.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    //((DataGridViewComboBoxColumn)this.dgDetalle.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }
        
        private void TxtMaskAAPP_Leave(object sender, EventArgs e)
        {
            //DUDA if (this.txtMaskAAPP.Modified == true || this.tgTexBoxSelCompania.Textbox.Modified == true)
            if (this.txtMaskAAPP.Modified == true)
            {
                string result = this.ValidarAAPP();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                    //this.txtMaskAAPP.Focus();
                }
            }
        }

        private void DateTimePickerFecha_Leave(object sender, EventArgs e)
        {
            string result = this.ValidarFecha();

            if (result != "")
            {
                string titulo = "";
                if (this.GLM01_FELAMG != null && this.GLM01_FELAMG != "")
                {
                    //Ver el valor del campo
                    switch (this.GLM01_FELAMG)
                    {
                        case "W":   //ADVERTENCIA
                            titulo = this.LP.GetText("wrnTitulo", "Advertencia");
                            break;
                        case "T":   //ERROR
                        default:
                            titulo = this.LP.GetText("errValTitulo", "Error");
                            break;
                    }
                }

                MessageBox.Show(result, titulo);
            }
        }

        private void DgDetalle_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            /*
                    // Validate the CompanyName entry by disallowing empty strings.
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "CompanyName")
                    {
                        if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText =
                                "Company Name must not be empty";
                            e.Cancel = true;
                        }
                    }
             
                    void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
                    {
                        // Clear the row error in case the user presses ESC.   
                        dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
                    }             
             */
            if (this.dgDetalle.IsCurrentCellDirty)
            {
                string result = "";
                string error = this.LP.GetText("errValTitulo", "Error");

                string valor = e.FormattedValue.ToString();
                string cuentaMayor = this.dgDetalle.Rows[e.RowIndex].Cells["Cuenta"].Value.ToString();
                switch (this.dgDetalle.Columns[e.ColumnIndex].Name)
                {
                    case "Cuenta":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaMayor(valor);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;

                                //this.dgDetalle.Rows[e.RowIndex].ErrorText = String.Empty;
                            }
                        }
                        break;
                    case "Auxiliar1":
                        if (valor != "" && valor != cuentaAuxiliarGlobal)
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 1);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "Auxiliar2":
                        if (valor != "" && valor != cuentaAuxiliarGlobal)
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 2);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "Auxiliar3":
                        if (valor != "" && valor != cuentaAuxiliarGlobal)
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 3);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "DH":
                        //FALTA !!!! SOlo leer 1 caracter que tiene que ser D / H
                        break;
                    case "Descripcion":
                        //FALTA !!!! SOlo leer 36 caracteres
                        break;
                    case "TipoExt":
                        if (valor != "")
                        {
                            result = ValidarTipoExtracontable(valor);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                }
            }
        }


        private void DgDetalle_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //string columnName = this.dgDetalle.Columns[this.dgDetalle.CurrentCell.ColumnIndex].Name;

            switch (this.dgDetalle.Columns[e.ColumnIndex].Name)
            {
                case "Cuenta":
                    //Comprobar si la cuenta de mayor tiene auxiliares
                    string valor = this.dgDetalle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string result = UpdateEstadoColumnasDadoCuentaMayor(valor, e.RowIndex);

                    //poner la celda activa la que toque
                    break;
                case "Auxiliar1":
                    break;
                case "Auxiliar2":
                    break;
                case "Auxiliar3":
                    break;
                case "DH":
                    break;
                case "TipoExt":
                    break;
                case "Descripcion":
                    break;
            }
        }

        private void DgDetalle_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;

            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    switch (columnName)
                    {
                        case "Cuenta":     //Cta Mayor
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            this.btnSel.Visible = true;
                            //this.btnSel.Select();
                            break;
                        case "Auxiliar1":     //Auxiliar 1 
                        case "Auxiliar2":     //Auxiliar 2
                        case "Auxiliar3":     //Auxiliar 3
                            //Verificar que la cuenta de mayor se ha introducido
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Cuenta"].Value.ToString() != "")
                            {
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = false;
                                this.BtnSelPosicion(tgGridDetalles);
                                this.btnSel.Visible = true;
                                //this.btnSel.Select();
                            }
                            else
                            {
                                this.btnSel.Visible = false;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = true;
                            }
                            break;
                        case "TipoExt": //Tipo de extracontable
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            this.btnSel.Visible = true;
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Value.ToString().Trim() == "" &&
                                this.radDropDownListTipoDefecto.Text != "")
                            {
                                string codigo = this.radDropDownListTipoDefecto.SelectedValue.ToString();
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Value = codigo;
                            }
                            break;
                        default:
                            this.btnSel.Visible = false;
                            break;
                    }
                }
            }
        }

        private void DgDetalle_Enter(object sender, EventArgs e)
        {
            this.BtnSelPosicion((TGGrid)sender);
        }

        private void DgDetalle_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
            bool columnaProcesada = false;

            e.Control.KeyPress -= Tb_KeyPress;
            e.Control.KeyPress -= Tb_May_KeyPress;
            e.Control.KeyPress -= Tb_NADA_KeyPress;

            switch (columnName)
            {                
                //Todo caracter a mayúsculas
                case "Cuenta":
                case "Auxiliar1":
                case "Auxiliar2":
                case "Auxiliar3":
                case "TipoExt": 
                case "DH":
                    e.Control.KeyPress += new KeyPressEventHandler(Tb_May_KeyPress);
                    columnaProcesada = true;
                    break;
                default:
                    e.Control.KeyPress += new KeyPressEventHandler(Tb_NADA_KeyPress);
                    break;
            }


            if (!columnaProcesada && columnName.Length > 3)
            {
                //Verificar si es una columna variable para recalcular los totales
                if (columnName.Substring(0, 3) == prefijoColumnaPeriodo)
                {
                    e.Control.KeyPress -= Tb_KeyPress;
                    e.Control.KeyPress -= Tb_May_KeyPress;
                    e.Control.KeyPress -= Tb_NADA_KeyPress;

                    e.Control.KeyPress += new KeyPressEventHandler(Tb_KeyPress);
                }
            }
        }

        void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb = sender as TextBox;
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.tb, true, ref sender, ref e);
        }

        void Tb_May_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracter = e.KeyChar.ToString().ToUpper();
            e.KeyChar = Convert.ToChar(caracter);
        }

        void Tb_NADA_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = e.KeyChar;
        }

        private void TxtNoComprobante_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtNoComprobante, false, ref sender, ref e);
        }

        private void DgDetalle_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
        }
        
        private void DgErrores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                string tipo = this.dgErrores.Rows[row].Cells["CodiTipo"].Value.ToString();
                string control_celda = this.dgErrores.Rows[row].Cells["CtrlCelda"].Value.ToString();
                string rowDetalle = "";
                int rowDet;
                switch (tipo)
                {
                    case "C":       //CABECERA
                        if (control_celda != "")
                        {
                            //Control[] ctrl = this.Controls.Find(control_celda, true);
                            Control[] ctrl = this.Controls["gbCabecera"].Controls.Find(control_celda, true);

                            if (ctrl.Length > 0)
                            {
                                if (ctrl[0].GetType().Name == "TGTexBoxSel")
                                {
                                    TGTexBoxSel tt = ((TGTexBoxSel)ctrl[0]);
                                    tt.Textbox.Select();
                                }
                                else ctrl[0].Select();
                            }
                        }
                        break;
                    case "D":       //DETALLE
                        rowDetalle = this.dgErrores.Rows[row].Cells["Linea"].Value.ToString();
                        if (rowDetalle != "")
                        {
                            rowDet = Convert.ToInt32(rowDetalle) - 1;

                            if (control_celda != "")
                            {
                                this.dgDetalle.CurrentCell = this.dgDetalle.Rows[rowDet].Cells[control_celda];
                                this.dgDetalle.BeginEdit(true);
                            }
                            else
                            {
                                this.dgDetalle.CurrentCell = this.dgDetalle.Rows[rowDet].Cells[0];
                                this.dgDetalle.Rows[0].Selected = true;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void DgDetalle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void FrmCompExtContAltaEdita_SizeChanged(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
        }

        private void BtnGenerarPeriodos_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string annoperiodoDesde = "";
                string annoperiodoHasta = "";

                if (this.ValidarPeriodos(ref annoperiodoDesde, ref annoperiodoHasta))
                {
                    //Crear las columnas correspondientes
                    this.ObtenerPeriodos(annoperiodoDesde, annoperiodoHasta);

                    bool ordenar = false;
                    string nombreColumnaActual = "";

                    if (this.periodosGenerados.Count > 0)
                    {
                        string nombreColumnaPeriodo = "";

                        bool existeColumna = false;
                        int cantidadColumnas = this.dgDetalle.Columns.Count;
                        string periodoConFormato = "";

                        //Chequear si ya existe la columna del periodo en la Grid
                        for (int i = 0; i < this.periodosGenerados.Count; i++)
                        {
                            existeColumna = false;
                            nombreColumnaPeriodo = prefijoColumnaPeriodo + this.periodosGenerados[i].ToString();
                            for (int j = 6; j < this.dgDetalle.Columns.Count; j++)
                            {
                                nombreColumnaActual = this.dgDetalle.Columns[j].Name;
                                if (nombreColumnaActual == nombreColumnaPeriodo)
                                {
                                    cantidadColumnas = this.dgDetalle.Columns.Count;
                                    existeColumna = true;
                                    break;
                                }
                                if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                { 
                                    int compPRD = String.Compare(nombreColumnaActual, nombreColumnaPeriodo);
                                    if (compPRD > 0)
                                    {
                                        cantidadColumnas = j + 1;
                                        break;
                                    }
                                }
                            }

                            if (!existeColumna)
                            {
                                //Adicionar la columna en el DataSet y en el DataGrid
                                this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add(prefijoColumnaPeriodo + this.periodosGenerados[i].ToString(), typeof(string));

                                periodoConFormato = this.periodosGenerados[i].ToString();
                                if (periodoConFormato.Length < 4) periodoConFormato.PadRight(4, '0');
                                periodoConFormato = periodoConFormato.Substring(0, 2) + "-" + periodoConFormato.Substring(2, 2);

                                //this.dgDetalle.AddTextBoxColumn(cantidadColumnas, (prefijoColumnaPeriodo + this.periodosGenerados[i]).ToString(), periodoConFormato, 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                                this.dgDetalle.AddTextBoxColumn(cantidadColumnas-1, (prefijoColumnaPeriodo + this.periodosGenerados[i]).ToString(), periodoConFormato, 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                                
                                cantidadColumnas++;

                                foreach (DataGridViewColumn column in dgDetalle.Columns)
                                {
                                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                                }

                                foreach (DataRow row in this.dgDetalle.dsDatos.Tables["Detalle"].Rows)
                                {
                                    //Rellenar todas las filas de la columnas adicionadas en blanco  
                                    row[prefijoColumnaPeriodo + this.periodosGenerados[i]] = "";
                                }

                                //ordenar = true;
                                ordenar = false;
                            }
                        }
                    }

                    //Eliminar las columnas de periodos de la Grid que no existan en el Array de Periodos
                    string periodoActual = "";
                    bool existePeriodo;
                    string columnasAEliminar = "";
                    for (int i = 0; i < this.dgDetalle.Columns.Count; i++)
                    {
                        existePeriodo = false;
                        nombreColumnaActual = this.dgDetalle.Columns[i].Name;
                        if (nombreColumnaActual.Length > 3)
                            if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                            {
                                periodoActual = nombreColumnaActual.Substring(3, nombreColumnaActual.Length - 3);
                                for (int j = 0; j < this.periodosGenerados.Count; j++)
                                {
                                    if (this.periodosGenerados[j].ToString() == periodoActual)
                                    {
                                        existePeriodo = true;
                                        break;
                                    }
                                }
                                if (!existePeriodo) columnasAEliminar += nombreColumnaActual + ",";
                            }
                    }

                    if (columnasAEliminar != "")
                    {
                        columnasAEliminar = columnasAEliminar.Remove(columnasAEliminar.Length-1);
                        string[] nombreColumna = columnasAEliminar.Split(',');
                        for (int i = 0; i < nombreColumna.Length; i++)
                        {
                            this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove(nombreColumna[i]);
                            this.dgDetalle.Columns.Remove(nombreColumna[i]);
                            //ordenar = true;
                            ordenar = false;
                        }
                    }

                    if (ordenar)
                    {
                        this.GridOrdenarColumnas();

                        if (this.periodosGenerados.Count < 5) this.dgDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        else
                        {
                            this.dgDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                            //Ajustar todas las columnas de la Grid
                            utiles.AjustarColumnasGrid(ref dgDetalle, -1);
                        }
                    }
                }
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadBtnMenuMostrarOcultar_Click(object sender, EventArgs e)
        {
            if (this.menuLateralExpanded)
            {
                int sizePanel = this.radPanelAcciones.Size.Width;
                int sizeButton = this.radBtnMenuMostrarOcultar.Width;
                collapseWidth = sizePanel - sizeButton - 4;

                this.menuLateralExpanded = false;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width - collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = true;

                this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width + collapseWidth, this.radPanelMenuPath.Height);
                this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X - collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width + collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X - collapseWidth, this.radPanelApp.Location.Y);
            }
            else
            {
                this.menuLateralExpanded = true;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width + collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = false;

                this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width - collapseWidth, this.radPanelMenuPath.Height);
                this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X + collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width - collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X + collapseWidth, this.radPanelApp.Location.Y);
            }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            frmCompExtContAltaEdita frmAltaEdita = new frmCompExtContAltaEdita
            {
                NuevoComprobante = true,
                FrmPadre = this
            };
            frmAltaEdita.Show();
        }

        private void RadButtonGrabar_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            string validarGrabarFormulario = ValidarGrabarFormulario();

            bool grabar = true;
            if (validarGrabarFormulario != "")
            {
                //Hay errores, pedir confirmación para grabar
                string mensaje = this.LP.GetText("errGrabarErrores", "Se han encontrado los siguientes errores") + ": \n\r" + validarGrabarFormulario + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿De todas formas desea grabar el fichero?");
                DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.No) grabar = false;
            }

            if (grabar)
            {
                // Actualizar valores de tag para que no pregunte por cambios al salir
                this.cmbCompania.Tag = this.cmbCompania.SelectedValue.ToString();
                this.txtMaskAAPP.Tag = this.txtMaskAAPP.Text.ToString();
                this.dateTimePickerFecha.Tag = Convert.ToDateTime(this.dateTimePickerFecha.Value);
                this.cmbTipo.Tag = this.cmbTipo.SelectedValue.ToString();
                this.txtNoComprobante.Tag = this.txtNoComprobante.Text.ToString();
                this.txtMaskAAPPDesde.Tag = this.txtMaskAAPPDesde.Text.ToString();
                this.txtMaskAAPPHasta.Tag = this.txtMaskAAPPHasta.Text.ToString();
                this.radDropDownListTipoDefecto.Tag = this.radDropDownListTipoDefecto.Text.ToString();
                this.txtDescripcion.Tag = this.txtDescripcion.Text.ToString();
                this.gridChange = false;

                //Actualizar Tabla Cabecera
                this.ActualizarTablaCabeceraDesdeForm();

                //Actualizar Tabla Totales
                this.ActualizarTablaTotalesDesdeForm();

                if (this.nuevoComprobante || this.importarComprobante)
                {
                    //Graba el comprobante en formato xml
                    this.GrabarNuevoComprobante(false);
                }
                else
                {
                    this.ActualizarComprobante();
                }

                //Actualizar los atributos TAG de los controles de la cabecera
                ActualizaValoresOrigenControles();
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonGrabarComo_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            string validarGrabarFormulario = ValidarGrabarFormulario();

            bool grabar = true;
            if (validarGrabarFormulario != "")
            {
                //Hay errores, pedir confirmación para grabar
                string mensaje = this.LP.GetText("errGrabarErrores", "Se han encontrado los siguientes errores") + ": \n\r" + validarGrabarFormulario + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿De todas formas desea grabar el fichero?");
                DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.No) grabar = false;
            }

            if (grabar)
            {
                //Actualizar Tabla Cabecera
                this.ActualizarTablaCabeceraDesdeForm();

                //Actualizar Tabla Totales
                this.ActualizarTablaTotalesDesdeForm();

                //Graba el comprobante en formato xml
                this.GrabarNuevoComprobante(true);
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonValidar_Click(object sender, EventArgs e)
        {
            //SMR - necesario para que al validar tome el ultimo valor de la celda, no el anterior
            this.dgDetalle.BindingContext[this.dgDetalle.DataSource].EndCurrentEdit();

            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.dgErrores.Visible == true) this.dgDetalle.Height = this.dgDetalle.Height + this.dgErrores.Height;
                utiles.ButtonEnabled(ref this.radButtonValidar, false);
                utiles.ButtonEnabled(ref this.radButtonValidarErrores, false);
                this.dgErrores.Visible = false;

                //Crear el comprobante y llamar a las funciones de validacion
                comp = new ComprobanteExtContable
                {
                    CuentaAuxiliarGlobal = cuentaAuxiliarGlobal,

                    Cab_compania = this.cmbCompania.SelectedValue.ToString()
                };

                //coger el valor sin la máscara
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                comp.Cab_anoperiodo = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                comp.Cab_fecha = this.dateTimePickerFecha.Value.ToShortDateString();

                comp.Cab_tipo = this.cmbTipo.SelectedValue.ToString();

                comp.Cab_noComprobante = this.txtNoComprobante.Text;
                //comp.Cab_descripcion = this.txtDescripcion.Text;

                comp.Det_detalles = this.dgDetalle.dsDatos.Tables["Detalle"].Clone();

                string debe_haber = "";

                ArrayList filas = new ArrayList();
                if (this.dgDetalle.SelectedRows.Count > 0)
                {
                    //    Create a generics list to hold selected rows so it can be sorted later
                    List<DataGridViewRow> dgSelectedRows = new List<DataGridViewRow>();

                    foreach (DataGridViewRow dgvr in this.dgDetalle.SelectedRows)
                        dgSelectedRows.Add(dgvr);

                    //    Sort list based on DataGridViewRow.Index    
                    dgSelectedRows.Sort(DataGridViewRowIndexCompare);

                    int indiceDetComp = 0;
                    foreach (DataGridViewRow row in dgSelectedRows)
                    {
                        int selectedIndex = row.Index;
                        filas.Add(selectedIndex);
                        //Copiar solo las filas seleccionadas
                        comp.Det_detalles.ImportRow(((DataRowView)(row.DataBoundItem)).Row);
                        debe_haber = row.Cells["DH"].Value.ToString();
                        comp.Det_detalles.Rows[indiceDetComp]["DH"] = debe_haber;
                        indiceDetComp++;
                    }
                }
                else
                {
                    int i = 0;
                    foreach (DataRow row in this.dgDetalle.dsDatos.Tables["Detalle"].Rows)
                    {
                        filas.Add(i);
                        debe_haber = row["DH"].ToString();
                        comp.Det_detalles.ImportRow(row);
                        comp.Det_detalles.Rows[i]["DH"] = debe_haber;
                        i++;
                    }
                }

                comp.LPValor = this.LP;

                this.grBoxProgressBar.Text = "Validando";   //Falta traducir
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Blocks;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = comp.Det_detalles.Rows.Count;
                this.progressBarEspera.Visible = true;
                this.Refresh();

                bool resultCabecera = comp.ValidarCabecera();

                bool resultAux = true;
                bool resultDetalle = true;
                for (int i = 0; i < comp.Det_detalles.Rows.Count; i++)
                {
                    resultAux = comp.ValidarDetalle(i, Convert.ToInt16(filas[i]) + 1);
                    if (!resultAux) resultDetalle = false;

                    //Mover la barra de progreso
                    this.progressBarEspera.Value = this.progressBarEspera.Value + 1;
                    this.progressBarEspera.Refresh();
                }

                this.progressBarEspera.Visible = false;
                this.grBoxProgressBar.Visible = false;

                if (!resultCabecera || !resultDetalle)
                {
                    utiles.ButtonEnabled(ref this.radButtonValidarErrores, true);

                    //Mostrar el Grid de Errores
                    this.ViewGridErrores();

                    /*ScrollableMessageBox scrollMsgBox = new ScrollableMessageBox();
                    scrollMsgBox.Titulo = "Validación comprobante";
                    scrollMsgBox.TextoBotonAceptar = "Aceptar";
                    scrollMsgBox.TextoMensaje = this.comp.Errores;
                    scrollMsgBox.ShowDialog();*/
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonValidarErrores, false);
                    MessageBox.Show(this.LP.GetText("lblValidarOk", "Validación correcta"), "Comprobante");   //Falta traducir comprobante
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonValidarErrores_Click(object sender, EventArgs e)
        {
            if (this.comp != null)
            {
                if (this.dgErrores.Visible)
                {
                    this.dgDetalle.Height = this.dgDetalle.Size.Height + this.dgErrores.Size.Height;
                    this.dgErrores.Visible = false;
                }
                else
                {
                    if (this.comp.DSErrores.Tables["Errores"].Rows.Count > 0)
                    {
                        //Mostrar el Grid de Errores
                        this.ViewGridErrores();
                    }
                }
            }
        }

        private void RadButtonRevertir_Click(object sender, EventArgs e)
        {
            //Multiplicar por -1 las columnas de periodos si existen
            string monedaLocal = "";
            decimal monedaLocalDec;
            string nombreColumnaActual = "";
            bool change = false;

            try
            {
                for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                {
                    for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                    {
                        nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[j].ColumnName;
                        if (nombreColumnaActual.Length > 3)
                            if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                            {
                                monedaLocal = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual].ToString().Trim();
                                if (monedaLocal != "")
                                {
                                    monedaLocalDec = Convert.ToDecimal(monedaLocal);
                                    monedaLocalDec = monedaLocalDec * -1;
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual] = monedaLocalDec;
                                    change = true;
                                }
                            }
                    }
                }

                //Recalcular la tabla de totales
                if (change) this.CalcularTotales();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonImportar_Click(object sender, EventArgs e)
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                this.grBoxProgressBar.Text = "Importando";   //Falta traducir
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                ExcelExportImport excelImport = new ExcelExportImport();
                string cabecera = ConfigurationManager.AppSettings["ModComp_Excel_Cabecera"];
                if (cabecera != null)
                {
                    excelImport.Cabecera = (cabecera == "Yes" ? true : false);
                }
                string result = excelImport.Importar();

                if (result == "")
                {
                    this.dgDetalle.AddUltimaFilaSiNoHayDisponile = false;

                    //Eliminar Todas las filas del DataGrid
                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Clear();

                    for (int i = 0; i <= excelImport.DateTableDatos.Rows.Count; i++)
                    {
                        //Mover la barra de progreso
                        if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                        else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                        this.progressBarEspera.Refresh();

                        DataRow row = this.dgDetalle.dsDatos.Tables["Detalle"].NewRow();

                        //Insertar filas con el valor de la fila del excel
                        if (i < excelImport.DateTableDatos.Rows.Count)
                        {
                            for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                            {
                                if (j < excelImport.DateTableDatos.Columns.Count)
                                {
                                    row[j] = excelImport.DateTableDatos.Rows[i][j].ToString();
                                }
                                else break;
                            }
                        }
                        else
                        {
                            //Insertar fila en blanco
                            for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                            {
                                if (j < excelImport.DateTableDatos.Columns.Count)
                                {
                                    row[j] = "";
                                }
                                else break;
                            }
                        }

                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add(row);

                        //Habilitar / Deshabilitar Columnas
                        this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                    }

                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);

                    //Recalcular Totales
                    this.CalcularTotales();
                }
                else
                {
                    if (result != "CANCELAR")
                    {
                        MessageBox.Show(this.LP.GetText("errImportExcel", "Error importando fichero excel") + " (" + result + ")", error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errImportExcel", "Error importando fichero excel") + " (" + ex.Message + ")", error);
            }

            this.progressBarEspera.Visible = false;
            this.grBoxProgressBar.Visible = false;

            this.dgDetalle.AddUltimaFilaSiNoHayDisponile = true;

            if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count == 0) this.dgDetalle.AdicionarFila();
        }

        private void RadButtonExportar_Click(object sender, EventArgs e)
        {
            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                this.grBoxProgressBar.Text = "Exportando";   //Falta traducir
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                ExcelExportImport excelImport = new ExcelExportImport
                {
                    DateTableDatos = this.dgDetalle.dsDatos.Tables["Detalle"]
                };
                string cabecera = ConfigurationManager.AppSettings["ModComp_Excel_Cabecera"];
                if (cabecera != null)
                {
                    excelImport.Cabecera = (cabecera == "Yes" ? true : false);
                    //if (excelImport.Cabecera) excelImport.GridColumnas = this.dgDetalle.Columns;
                }

                string result = excelImport.ExportarMemoria();

                this.progressBarEspera.Visible = false;
                this.grBoxProgressBar.Visible = false;

                if (result != "" && result != "CANCELAR")
                {
                    MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + result + ")", error);
                }

                /*
                //Exportar a Excel 
                string stOutput = "";
                // Export titles:
                string sHeaders = "";

                for (int j = 0; j < this.dgDetalle.Columns.Count; j++)
                    sHeaders = sHeaders.ToString() + Convert.ToString(this.dgDetalle.Columns[j].HeaderText) + "\t";
                stOutput += sHeaders + "\r\n";
                // Export data.
                for (int i = 0; i < this.dgDetalle.RowCount - 1; i++)
                {
                    string stLine = "";
                    for (int j = 0; j < this.dgDetalle.Rows[i].Cells.Count; j++)
                        stLine = stLine.ToString() + Convert.ToString(this.dgDetalle.Rows[i].Cells[j].Value) + "\t";
                    stOutput += stLine + "\r\n";
                }
                Encoding utf16 = Encoding.GetEncoding(1254);
                byte[] output = utf16.GetBytes(stOutput);
                string filename = @"C:\VS2010_Projects\FinanzasNet\ModComprobantes\comprobantesContables\export.xls";
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(output, 0, output.Length); //write the encoded file
                bw.Flush();
                bw.Close();
                fs.Close();
                 */
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", error);
            }
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmbCompania_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.cmbCompania.SelectedValue != null)
            {
                string codigo = this.cmbCompania.SelectedValue.ToString();

                string result = this.ValidarCompania(codigo);
            }
        }

        private void RadButtonNuevo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNuevo);
        }

        private void RadButtonNuevo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNuevo);
        }

        private void RadButtonGrabar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabar);
        }

        private void RadButtonGrabar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabar);
        }

        private void RadButtonGrabarComo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabarComo);
        }

        private void RadButtonGrabarComo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabarComo);
        }

        private void RadButtonValidar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonValidar);
        }

        private void RadButtonValidar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonValidar);
        }

        private void RadButtonValidarErrores_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonValidarErrores);
        }

        private void RadButtonValidarErrores_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonValidarErrores);
        }

        private void RadButtonRevertir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonRevertir);
        }

        private void RadButtonRevertir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonRevertir);
        }

        private void RadButtonImportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonImportar);
        }

        private void RadButtonImportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonImportar);
        }

        private void RadButtonExportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExportar);
        }

        private void RadButtonExportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExportar);
        }

        private void RadButtonSalir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSalir);
        }

        private void RadButtonSalir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSalir);
        }

        private void FrmCompExtContAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.cmbCompania.Text != this.cmbCompania.Tag.ToString() ||
                    this.txtMaskAAPP.Text != this.txtMaskAAPP.Tag.ToString() ||
                    this.dateTimePickerFecha.Value != Convert.ToDateTime(this.dateTimePickerFecha.Tag) ||
                    this.cmbTipo.Text != this.cmbTipo.Tag.ToString() ||
                    this.txtNoComprobante.Text != this.txtNoComprobante.Tag.ToString() ||
                    this.txtMaskAAPPDesde.Text != this.txtMaskAAPPDesde.Tag.ToString() ||
                    this.txtMaskAAPPHasta.Text != this.txtMaskAAPPHasta.Tag.ToString() ||
                    this.radDropDownListTipoDefecto.Text != this.radDropDownListTipoDefecto.Tag.ToString() ||
                    this.txtDescripcion.Text != this.txtDescripcion.Tag.ToString() ||
                    this.gridChange
                )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonGrabar.PerformClick();
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Alta / Edita comprobantes extracontables ");
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        /// <param name="traducirComboClase">Si se traducen o no los literales del Combo de Clase</param>
        private void TraducirLiterales(bool traducirComboClase)
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompExtContAltaEditaTitulo", "Comprobante extracontable");	//Falta grabar literal en los ficheros de idiomas
            if (this.nuevoComprobante)
            {
                this.radLabelTitulo.Text = this.Text + " / " + this.LP.GetText("lblfrmCompContAltaEditaTituloNuevo", "Nuevo");
                utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            }
            else
            {
                this.radLabelTitulo.Text = this.Text + " / " + this.nombreComprobante;
                utiles.ButtonEnabled(ref this.radButtonNuevo, true);
            }

            //Traducir las etiquetas de la Cabecera
            this.gbCabecera.Text = this.LP.GetText("lblCabecera", "Cabecera");
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            this.lblFecha.Text = this.LP.GetText("lblfrmCompContAECabeceraFecha", "Fecha");
            this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
            this.lblAnoPeriodoDesde.Text = this.LP.GetText("lblfrmCompExtContAECabeceraAAPPDesde", "Año-Periodo Desde");    //Falta Traducir
            this.lblAnoPeriodoHasta.Text = this.LP.GetText("lblfrmCompExtContAECabeceraAAPPHasta", "Año-Periodo Hasta");    //Falta Traducir
            this.lblExtDefecto.Text = this.LP.GetText("lblfrmCompExtContAECabeceraEstDefecto", "Tipo Ext. por defecto");    //Falta Traducir
            this.lblDescripcion.Text = this.LP.GetText("lblfrmCompContAECabeceraDescripcion", "Descripción");

            //Traducir las etiquetas de la tabla de totales
            this.gbTotales.Text = this.LP.GetText("lblfrmCompContAETablaTotalesGroup", "Totales");
            this.lblTotalDebe.Text = this.LP.GetText("lblfrmCompContAETablaTotalesTotalDebe", "Total Debe");
            this.lblTotalHaber.Text = this.LP.GetText("lblfrmCompContAETablaTotalesTotalHaber", "Total Haber");
            this.lblMonedaLocal.Text = this.LP.GetText("lblfrmCompContAETablaTotalesMonedaLocal", "Moneda Local");
            this.lblNoApuntes.Text = this.LP.GetText("lblfrmCompContAETablaTotalesNoApuntes", "No. Apuntes");

            //Traducir botón de generar periodos
            this.btnGenerarPeriodos.Text = this.LP.GetText("lblfrmCompExtContAEGenerarPeriodos", "Generar Periodos"); //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonGrabar.Text = this.LP.GetText("lblfrmCompContBotGrabar", "Grabar");
            this.radButtonGrabarComo.Text = this.LP.GetText("lblfrmCompContBotGrabarComo", "Grabar Como");
            //this.toolStripButtonSelecModelo.Text = this.LP.GetText("lblfrmCompContBotSelModelo", "Seleccionar Modelo");
            this.radButtonValidar.Text = this.LP.GetText("lblfrmCompContBotValidar", "Validar");
            this.radButtonRevertir.Text = this.LP.GetText("lblfrmCompContBotRevertir", "Revertir importes");
            this.radButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");
            this.radButtonExportar.Text = this.LP.GetText("lblfrmCompContClickDerExportar", "Exportar");
            this.radButtonImportar.Text = this.LP.GetText("lblfrmCompContClickDerImportar", "Importar");

            //Traducir los Literales del menuGridClickDerecho
            this.menuGridButtonAdicionarFila.Text = this.LP.GetText("lblfrmCompContClickDerAdicionarFila", "Añadir fila");
            this.menuGridButtonInsertarFila.Text = this.LP.GetText("lblfrmCompContClickDerInsertarFila", "Insertar fila");
            this.menuGridButtonSuprimirFila.Text = this.LP.GetText("lblfrmCompContClickDerSuprimirFila", "Suprimir fila");
            this.menuGridButtonBuscar.Text = this.LP.GetText("lblfrmCompContClickDerBuscar", "Buscar");
            this.menuGridButtonCortar.Text = this.LP.GetText("lblfrmCompContClickDerCortar", "Cortar");
            this.menuGridButtonCopiar.Text = this.LP.GetText("lblfrmCoContClickDerCopiar", "Copiar");
            this.menuGridButtonPegar.Text = this.LP.GetText("lblfrmCompContClickDerPegar", "Pegar");
            this.menuGridButtonBorrar.Text = this.LP.GetText("lblfrmCompContClickDerBorrar", "Borrar");

            //Traducir los encabezados de las columnas
            if (this.dgDetalle != null)
            {
                this.dgDetalle.CambiarColumnHeader("Cuenta", this.LP.GetText("lblfrmCompContdgCuenta", "Cuenta"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar1", this.LP.GetText("lblfrmCompContdgAux1", "Auxiliar-1"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar2", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-2"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar3", this.LP.GetText("lblfrmCompContdgAux3", "Auxiliar-3"));
                this.dgDetalle.CambiarColumnHeader("TipoExt", this.LP.GetText("lblfrmCompExtContdgExtracontable", "Extracontable"));  //Falta Traducir
                this.dgDetalle.CambiarColumnHeader("DH", this.LP.GetText("lblfrmCompContdgDH", "D/H"));
                this.dgDetalle.CambiarColumnHeader("Descripcion", this.LP.GetText("lblfrmCompContdgDesc", "Descripción"));
            }
        }
        
        /// <summary>
        /// Crear las etiquetas para la tabla de totales
        /// </summary>
        private void CrearTablaTotales()
        {
            Font fontTabla = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);

            //this.lblTotalDebe.Text = "Total Debe";
            this.lblTotalDebe = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleCenter,
                Font = fontTabla
            };

            //this.lblTotalHaber.Text = "Total Haber";
            this.lblTotalHaber = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleCenter,
                Font = fontTabla
            };

            //this.lblMonedaLocal.Text = "Moneda Local";
            this.lblMonedaLocal = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                Font = fontTabla
            };

            //this.lblMonedaLocal_Debe.Text = "300.00";
            this.lblMonedaLocal_Debe = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblMonedaLocal_Haber.Text = "20.00";
            this.lblMonedaLocal_Haber = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            this.tablaTotales.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            this.tablaTotales.Controls.Add(this.lblTotalDebe, 1, 0);
            this.tablaTotales.Controls.Add(this.lblTotalHaber, 2, 0);
            this.tablaTotales.Controls.Add(this.lblMonedaLocal, 0, 1);

            this.tablaTotales.Controls.Add(this.lblMonedaLocal_Debe, 1, 1);
            this.tablaTotales.Controls.Add(this.lblMonedaLocal_Haber, 2, 1);

            //this.lblNoApuntes.Text = "No. Apuntes";
            this.lblNoApuntes = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblNoApuntes_Valor.Text = "5";
            this.lblNoApuntes_Valor = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.None,
                TextAlignment = ContentAlignment.TopCenter,
                AutoSize = true,
                Font = fontTabla
            };

            this.tablaNoApuntes.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            this.tablaNoApuntes.Controls.Add(this.lblNoApuntes, 0, 0);
            this.tablaNoApuntes.Controls.Add(this.lblNoApuntes_Valor, 1, 0);
        }


        /// <summary>
        /// Construye el DataGrid
        /// </summary>
        private void CrearDataGrid()
        {
            this.dgDetalle.dsDatos = new DataSet
            {
                DataSetName = "Comprobante"
            };
            this.dgDetalle.NombreTabla = "Detalle";
            this.dgDetalle.AddUltimaFilaSiNoHayDisponile = true;

            this.dgDetalle.Name = "dgDetalle";
            this.dgDetalle.AllowUserToAddRows = false;
            this.dgDetalle.AllowUserToOrderColumns = false;
            this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            //this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetalle.ContextMenuStrip = this.menuGridClickDerecho;

            this.dgDetalle.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            this.dgDetalle.AutoGenerateColumns = false;

            this.dgDetalle.RowNumber = true;

            try
            {
                this.dgDetalle.AddTextBoxColumn(0, "Cuenta", this.LP.GetText("lblfrmCompContdgCuenta", "Cuenta"), 90, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(1, "Auxiliar1", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-1"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(2, "Auxiliar2", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-2"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(3, "Auxiliar3", this.LP.GetText("lblfrmCompContdgAux3", "Auxiliar-3"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(4, "TipoExt", this.LP.GetText("lblfrmCompExtContdgExtracontable", "Extracontable"), 90, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);

                //Valores del ComboBox para el Debe / Habe
                DataTable tableSource = new DataTable("tableSource");
                tableSource.Columns.AddRange(new DataColumn[] {
                new DataColumn("id"),
                new DataColumn("desc") });
                tableSource.Rows.Add("", "");
                tableSource.Rows.Add("D", this.LP.GetText("lblCompContDebe", "Debe"));
                tableSource.Rows.Add("H", this.LP.GetText("lblCompContHaber", "Haber"));

                DataGridViewComboBoxColumn columnComboBoxDH = new DataGridViewComboBoxColumn
                {
                    Name = "DH",
                    DataPropertyName = "DH",
                    Width = 60,
                    DropDownWidth = 160,
                    MaxDropDownItems = 3,
                    FlatStyle = FlatStyle.Flat,

                    DataSource = tableSource,
                    DisplayMember = "desc",
                    ValueMember = "id",
                    ValueType = typeof(string)
                };
                this.dgDetalle.Columns.Insert(5, columnComboBoxDH);

                this.dgDetalle.AddTextBoxColumn(6, "Descripcion", this.LP.GetText("lblfrmCompContdgDesc", "Descripción"), 100, 36, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                
                //Fijar las columnas hasta la columna de Moneda Local
                //this.dgDetalle.Columns["MonedaLocal"].Frozen = true;

                for (int i = 0; i < this.dgDetalle.ColumnCount; i++)
                {
                    this.dgDetalle.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //Crear la propiedad ComboValores para las columnas de tipo DataGridViewComboBoxColumn
             //               tableSource.Rows.Add("D", "Debe");
              //  tableSource.Rows.Add("H", "Haber")
            
            string[,] valoresCombo = new string[,]
	        {
	            {this.LP.GetText("lblCompContDebe", "Debe"), "D"},
	            {this.LP.GetText("lblCompContHaber", "Haber"), "H"}
	        
	        };
            this.dgDetalle.ComboValores = valoresCombo;

            /*
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetalle.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDetalle.DefaultCellStyle = dataGridViewCellStyle2;
            
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetalle.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            */
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
            try
            {
                string query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 where STATMG='V' order by CCIAMG";
                string result = this.FillComboBox(query, "CCIAMG", "NCIAMG", ref this.cmbCompania, true, -1, false);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    string mensaje = this.LP.GetText("errGetCompanias", "Error obteniendo las compañías") + " (" + result + ")";
                    RadMessageBox.Show(mensaje, error);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los tipos de comprobantes
        /// </summary>
        private void FillTiposComprobantes()
        {
            string query = "select TIVOTV, NOMBTV  from " + GlobalVar.PrefijoTablaCG + "GLT06 where CODITV='1' and STATTV='V' order by TIVOTV";
            string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, false);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de comprobantes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Cargar los tipos de extracontables por defecto
        /// </summary>
        private void FillTiposDefecto()
        {
            string query = "select TDATAH, NOMBAH from " + GlobalVar.PrefijoTablaCG + "PRT03 where STATAH = 'V' order by TDATAH";
            string result = this.FillComboBox(query, "TDATAH", "NOMBAH", ref this.radDropDownListTipoDefecto, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de extracontables por defecto") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Crea el desplegable de Importes
        /// </summary>
        private void CrearComboImportes()
        {
            DataRow row;

            try
            {
                if (this.dtImporte.Rows.Count > 0) this.dtImporte.Rows.Clear();

                row = dtImporte.NewRow();
                row["valor"] = "1";
                row["desc"] = "Acumular";
                dtImporte.Rows.Add(row);

                row = dtImporte.NewRow();
                row["valor"] = "0";
                row["desc"] = "Sustituir";
                dtImporte.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListImportes.DataSource = dtImporte;
            this.radDropDownListImportes.ValueMember = "valor";
            this.radDropDownListImportes.DisplayMember = "desc";
            this.radDropDownListImportes.Refresh();
        }

        /// <summary>
        /// Formato para las fechas (dado parámetro de CG)
        /// </summary>
        private void CrearFechaConFormatoCG()
        {
            this.dateTimePickerFecha.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerFecha.CustomFormat = GlobalVar.CGFormatoFecha;
        }

        /// <summary>
        /// Recupera el comprobante y llena los controles del formulario con los datos del comprobante
        /// </summary>
        private void CargarDatosComprobante()
        {
            try
            {
                //Leer el comprobante
                string ficheroComp = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtraContables"];
                ficheroComp = ficheroComp + "\\" + this.archivoComprobante;

                ds = new DataSet();
                ds.ReadXml(ficheroComp);

                //Verificar que exista la tabla de Cabecera
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                {
                    //Verificar la marca contable para asegurar que sea un comprobante extracontable
                    if (ds.Tables["Cabecera"].Rows[0]["Contable"].ToString() == "0")
                    {
                        //Validar la compañia
                        string codigo = ds.Tables["Cabecera"].Rows[0]["Compania"].ToString();
                        string validarCompania = this.ValidarCompania(codigo);
                        if (validarCompania == "") this.cmbCompania.SelectedValue = codigo;

                        this.txtMaskAAPP.Text = ds.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                        
                        codigo = ds.Tables["Cabecera"].Rows[0]["Tipo"].ToString();
                        if (codigo.Length < 2) codigo = codigo.PadLeft(2, '0');
                        string validarTipo = this.ValidarTipo(codigo);
                        if (validarTipo == "") this.cmbTipo.SelectedValue = codigo;

                        this.txtNoComprobante.Text = ds.Tables["Cabecera"].Rows[0]["Numero"].ToString();
                        string fecha = ds.Tables["Cabecera"].Rows[0]["Fecha"].ToString();
                        this.dateTimePickerFecha.Text = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                        this.txtDescripcion.Text = ds.Tables["Cabecera"].Rows[0]["Descripcion"].ToString();

                        this.txtMaskAAPPDesde.Text = ds.Tables["Cabecera"].Rows[0]["AnoPeriodoDesde"].ToString();
                        this.txtMaskAAPPHasta.Text = ds.Tables["Cabecera"].Rows[0]["AnoPeriodoHasta"].ToString();
                        
                        codigo = ds.Tables["Cabecera"].Rows[0]["TipoExtDefecto"].ToString();
                        this.radDropDownListTipoDefecto.SelectedValue = codigo;
                        
                        string importesAcumular = ds.Tables["Cabecera"].Rows[0]["AcumularImportes"].ToString();
                        if (importesAcumular == "1") this.radDropDownListImportes.SelectedValue = "1";
                        else this.radDropDownListImportes.SelectedValue = "0";
                        
                        //Verificar que exista la tabla de Totales
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Totales"].Rows.Count > 0)
                        {
                            this.lblMonedaLocal_Debe.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString(), this.LP.MyCultureInfo);
                            this.lblMonedaLocal_Haber.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString(), this.LP.MyCultureInfo);
                            //this.lblNoApuntes_Valor.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString(), this.LP.MyCultureInfo);
                            this.lblNoApuntes_Valor.Text = ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString();
                        }

                        //Verificar que exista la tabla de Detalle
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                        {
                            this.dgDetalle.dsDatos = ds;
                            this.dgDetalle.DataSource = ds.Tables["Detalle"];

                            //Si existen columnas de periodos en el fichero, crearlas en la grid
                            string nombreColumnaActual = "";
                            string periodoActual = "";
                            string periodoActualFormato = "";
                            int cantidadColumnas = this.dgDetalle.Columns.Count;
                            bool ordenarCol = false;

                            for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; i++)
                            {
                                nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[i].ColumnName;
                                if (nombreColumnaActual.Length > 3)
                                {
                                    if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                    {
                                        periodoActual = nombreColumnaActual.Substring(3, nombreColumnaActual.Length - 3);
                                        if (periodoActual.Length != 4) periodoActualFormato.PadRight(4, '0');
                                        else periodoActualFormato = periodoActual;
                                        periodoActualFormato = periodoActualFormato.Substring(0, 2) + "-" + periodoActualFormato.Substring(2, 2);

                                        this.periodosGenerados.Add(periodoActual);
                                        //this.dgDetalle.AddTextBoxColumn(cantidadColumnas, (prefijoColumnaPeriodo + periodoActual).ToString(), periodoActualFormato, 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                                        this.dgDetalle.AddTextBoxColumn(cantidadColumnas-1, (prefijoColumnaPeriodo + periodoActual).ToString(), periodoActualFormato, 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                                        cantidadColumnas++;

                                        foreach (DataGridViewColumn column in dgDetalle.Columns)
                                        {
                                            column.SortMode = DataGridViewColumnSortMode.NotSortable;
                                        }

                                        //ordenarCol = true;
                                        ordenarCol = false;
                                    }
                                }
                            }

                            //Formatear columnas de importes
                            string valor = "";
                            for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                            {
                                if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i))
                                {
                                    for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                                    {
                                        nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[j].ColumnName;
                                        if (nombreColumnaActual.Length > 3)
                                        {
                                            if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                            {
                                                valor = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual].ToString();
                                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual] = utiles.ImporteFormato(valor, this.LP.MyCultureInfo);
                                            }
                                        }
                                    }
                                }
                            }

                            if (ordenarCol) this.GridOrdenarColumnas();

                            bool existeDetalle = false;
                            if (this.ds.Tables["Detalle"].Rows.Count == 1)
                            {
                                if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], 0))
                                {
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    existeDetalle = true;
                                }
                                else
                                {
                                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                                }
                            }
                            else
                            {
                                if (this.ds.Tables["Detalle"].Rows.Count > 1)
                                {
                                    existeDetalle = true;
                                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], this.ds.Tables["Detalle"].Rows.Count-1))
                                    {
                                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    }
                                }
                                else
                                {
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    existeDetalle = true;
                                }
                            }

                            if (existeDetalle)
                            {
                                //Habilitar / Deshabilitar Columnas
                                bool actualizarColumnasDadoCtaMayor = (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count <= 500);

                                if (actualizarColumnasDadoCtaMayor)
                                {
                                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                                    {
                                        this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                                    }
                                }

                                this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void ImportarDatosComprobante()
        {
            try
            {
                string compania = this.comprobanteExtContableImportar.Cab_compania.Trim();
                if (compania != "")
                {
                    try
                    {
                        this.cmbCompania.SelectedValue = compania;
                        if (this.cmbCompania.SelectedValue == null)
                        {
                            RadMessageBox.Show("La compañía " + compania + "no es válida. Debe seleccionar una.");
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("La compañía " + compania + "no es válida. Debe seleccionar una.");
                    }
                }
                else RadMessageBox.Show("Debe seleccionar la compañía.");

                string sigloanoper = this.comprobanteExtContableImportar.Cab_anoperiodo;
                if (sigloanoper.Length == 5) sigloanoper = sigloanoper.Substring(1, 4);
                this.txtMaskAAPP.Text = sigloanoper;
                //this.dateTimePickerFecha.Value = utiles.FormatoCGToFecha(this.comprobanteContableImportar.Cab_fecha);
                this.dateTimePickerFecha.Value = Convert.ToDateTime(this.comprobanteExtContableImportar.Cab_fecha);

                string tipo = this.comprobanteExtContableImportar.Cab_tipo.Trim();
                if (tipo != "")
                {
                    try
                    {
                        this.cmbTipo.SelectedValue = compania;
                        if (this.cmbTipo.SelectedValue == null)
                        {
                            RadMessageBox.Show("El tipo de comprobante " + tipo + "no es válido. Debe seleccionar uno.");
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("El tipo de comprobante " + tipo + "no es válido. Debe seleccionar uno.");
                    }
                }
                else RadMessageBox.Show("Debe seleccionar un tipo de comprobante.");

                this.txtNoComprobante.Text = this.comprobanteExtContableImportar.Cab_noComprobante;
                this.txtMaskAAPPDesde.Text = this.comprobanteExtContableImportar.Cab_periodoDesde;
                this.txtMaskAAPPHasta.Text = this.comprobanteExtContableImportar.Cab_periodoHasta;

                string tipoDefecto = this.comprobanteExtContableImportar.Cab_TipoExtDefecto.Trim();
                if (tipoDefecto != "")
                {
                    try
                    {
                        this.radDropDownListTipoDefecto.SelectedValue = tipoDefecto;
                        if (this.radDropDownListTipoDefecto.SelectedValue == null)
                        {
                            RadMessageBox.Show("El tipo de extracontable por defecto " + tipoDefecto + "no es válido. Debe seleccionar uno.");
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("El tipo de extracontable por defecto " + tipoDefecto + "no es válido. Debe seleccionar uno.");
                    }
                }
                else RadMessageBox.Show("Debe seleccionar un tipo de extracontable por defecto.");

                //Detalles
                this.dgDetalle.dsDatos.Tables.Add(this.comprobanteExtContableImportar.Det_detalles);
                this.dgDetalle.DataSource = this.dgDetalle.dsDatos.Tables["Detalle"];

                bool existeDetalle = false;

                if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count == 1)
                {
                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], 0))
                    {
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                        existeDetalle = true;
                    }
                    else
                    {
                        //Se podría dejar sin seleccionar la fila pero no lo consigo
                        //this.dgDetalle.CurrentRow.Selected = false;
                        //this.dgDetalle.Rows[0].Selected = false;
                    }
                }
                else
                {
                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                    existeDetalle = true;
                }

                if (existeDetalle)
                {
                    //Crear periodos si es necesario
                    if (this.comprobanteExtContableImportar.Cab_periodoDesde != "" || this.comprobanteExtContableImportar.Cab_periodoHasta != "")
                    {
                        //Generar los periodos
                        string nombreColumnaActual = "";
                        string periodoActual = "";
                        string periodoActualFormato = "";
                        int cantidadColumnas = this.dgDetalle.Columns.Count;

                        for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; i++)
                        {
                            nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[i].ColumnName;
                            if (nombreColumnaActual.Length > 3)
                            {
                                if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                {
                                    periodoActual = nombreColumnaActual.Substring(3, nombreColumnaActual.Length - 3);
                                    if (periodoActual.Length != 4) periodoActualFormato.PadRight(4, '0');
                                    else periodoActualFormato = periodoActual;
                                    periodoActualFormato = periodoActualFormato.Substring(0, 2) + "-" + periodoActualFormato.Substring(2, 2);

                                    this.periodosGenerados.Add(periodoActual);
                                    this.dgDetalle.AddTextBoxColumn(cantidadColumnas, (prefijoColumnaPeriodo + periodoActual).ToString(), periodoActualFormato, 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                                    cantidadColumnas++;
                                }
                            }
                        }

                        //Formatear columnas de importes
                        string valor = "";
                        for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                        {
                            if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i))
                            {
                                for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                                {
                                    nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[j].ColumnName;
                                    if (nombreColumnaActual.Length > 3)
                                    {
                                        if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo)
                                        {
                                            valor = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual].ToString();
                                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual] = utiles.ImporteFormato(valor, this.LP.MyCultureInfo);
                                        }
                                    }
                                }
                            }
                        }

                        this.GridOrdenarColumnas();
                    }

                    //Validar compañia para recuperar el campo GLM01.TIPLMG
                    string result = this.ValidarCompania(this.comprobanteExtContableImportar.Cab_compania);

                    //Habilitar / Deshabilitar Columnas
                    
                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                    {
                        this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                    }

                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                }

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea la tabla Cabecera del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaCabeceraVacia()
        {
            DataTable dtCabecera = new DataTable
            {
                TableName = "Cabecera"
            };

            dtCabecera.Columns.Add("Contable", typeof(string));
            dtCabecera.Columns.Add("Transferido", typeof(string));
            dtCabecera.Columns.Add("Compania", typeof(string));
            dtCabecera.Columns.Add("AnoPeriodo", typeof(string));
            dtCabecera.Columns.Add("Tipo", typeof(string));
            dtCabecera.Columns.Add("Numero", typeof(string));
            dtCabecera.Columns.Add("Fecha", typeof(string));
            dtCabecera.Columns.Add("AnoPeriodoDesde", typeof(string));
            dtCabecera.Columns.Add("AnoPeriodoHasta", typeof(string));
            dtCabecera.Columns.Add("Descripcion", typeof(string));
            dtCabecera.Columns.Add("TipoExtDefecto", typeof(string));
            dtCabecera.Columns.Add("AcumularImportes", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtCabecera);

            DataRow row = this.dgDetalle.dsDatos.Tables["Cabecera"].NewRow();
            row["Contable"] = "1";
            row["Transferido"] = "0";
            row["Compania"] = "";
            row["AnoPeriodo"] = "";
            row["Tipo"] = "";
            row["Numero"] = "";
            row["Fecha"] = "";
            row["AnoPeriodoDesde"] = "";
            row["AnoPeriodoHasta"] = "";
            row["Descripcion"] = "";
            row["TipoExtDefecto"] = "";
            row["AcumularImportes"] = "";
            this.dgDetalle.dsDatos.Tables["Cabecera"].Rows.Add(row);

        }

        /// <summary>
        /// Crea la tabla Totales del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaTotalesVacia()
        {
            DataTable dtTotales = new DataTable
            {
                TableName = "Totales"
            };

            dtTotales.Columns.Add("MonedaLocalDebe", typeof(string));
            dtTotales.Columns.Add("MonedaLocalHaber", typeof(string));
            dtTotales.Columns.Add("NumeroApuntes", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtTotales);

            DataRow row = this.dgDetalle.dsDatos.Tables["Totales"].NewRow();
            row["MonedaLocalDebe"] = "";
            row["MonedaLocalHaber"] = "";
            row["NumeroApuntes"] = "";

            this.dgDetalle.dsDatos.Tables["Totales"].Rows.Add(row);
        }

        /// <summary>
        /// Crea la tabla Detalle del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaDetalleVacia()
        {
            DataTable dtDetalle = new DataTable
            {
                TableName = "Detalle"
            };

            dtDetalle.Columns.Add("Cuenta", typeof(string));
            dtDetalle.Columns.Add("Auxiliar1", typeof(string));
            dtDetalle.Columns.Add("Auxiliar2", typeof(string));
            dtDetalle.Columns.Add("Auxiliar3", typeof(string));
            dtDetalle.Columns.Add("DH", typeof(string));
            dtDetalle.Columns.Add("TipoExt", typeof(string));
            dtDetalle.Columns.Add("Descripcion", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtDetalle);

            DataRow row = this.dgDetalle.dsDatos.Tables["Detalle"].NewRow();
            
            row["Cuenta"] = "";
            row["Auxiliar1"] = "";
            row["Auxiliar2"] = "";
            row["Auxiliar3"] = "";
            row["DH"] = "";
            row["TipoExt"] = "";
            row["Descripcion"] = "";

            this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add(row);

            //Poner el data source del Grid a la tabla detalle del DataSet
            this.dgDetalle.DataSource = this.dgDetalle.dsDatos.Tables["Detalle"];

            this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
        } 

        /// <summary>
        /// Crea las tablas del dataset vacías
        /// </summary>
        private void CrearTablasDataSetVacias()
        {
            try
            {
                //Crea la tabla Cabecera
                this.DataSetCrearTablaCabeceraVacia();

                //Crea la tabla Totales
                this.DataSetCrearTablaTotalesVacia();

                if (!this.importarComprobante)
                {
                    //Crea la tabla Detalle
                    this.DataSetCrearTablaDetalleVacia();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve si todas las columnas están vacías dado una fila de un DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="row">fila</param>
        /// <returns></returns>
        private bool TodaFilaEnBlanco(DataTable table, int row)
        {
            bool todaFilaBlanco = true;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Rows[row][i].ToString() != "" && table.Columns[i].ColumnName != "TipoExt")
                {
                    todaFilaBlanco = false;
                    break;
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarTablaCabeceraDesdeForm()
        {
            try
            {
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Contable"] = "0";
                //Siempre se guardará como No Transferido
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Transferido"] = "0";

                string codigo = this.cmbCompania.SelectedValue.ToString();
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Compania"] = codigo;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                codigo = this.cmbTipo.SelectedValue.ToString();
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tipo"] = codigo;
                
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Numero"] = this.txtNoComprobante.Text;
                string fecha = this.dateTimePickerFecha.Value.Year.ToString() + this.dateTimePickerFecha.Value.Month.ToString().PadLeft(2, '0') +
                               this.dateTimePickerFecha.Value.Day.ToString().PadLeft(2, '0');
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Fecha"] = fecha;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Descripcion"] = this.txtDescripcion.Text;

                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoDesde"] = this.txtMaskAAPPDesde.Text;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoHasta"] = this.txtMaskAAPPHasta.Text;

                codigo = this.radDropDownListTipoDefecto.SelectedValue.ToString();
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["TipoExtDefecto"] = codigo;

                if (this.radDropDownListImportes.SelectedValue.ToString() == "1") this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AcumularImportes"] = "1";
                else this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AcumularImportes"] = "0";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarTablaTotalesDesdeForm()
        {
            try
            {
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"] = this.lblMonedaLocal_Debe.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"] = this.lblMonedaLocal_Haber.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["NumeroApuntes"] = this.lblNoApuntes_Valor.Text;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GrabarNuevoComprobante(bool duplicar)
        {
            //Directorio donde se almacenan los comprobantes contables
            string pathFicherosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtraContables"];

            this.saveFileDialogGrabar = new SaveFileDialog
            {
                //Recuperar el directorio por defecto que está en la configuarción
                InitialDirectory = pathFicherosCompContables,
                DefaultExt = "xml",
                //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                Filter = "ficheros xml (*.xml)|*.xml"
            };

            if (duplicar) this.saveFileDialogGrabar.FileName = this.txtDescripcion.Text + " " + "copia";   //traducir
            else this.saveFileDialogGrabar.FileName = this.txtDescripcion.Text;
            
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            if (DialogResult.OK == this.saveFileDialogGrabar.ShowDialog())
            {
                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoDesde"] = this.txtMaskAAPPDesde.Value.ToString();
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoHasta"] = this.txtMaskAAPPHasta.Value.ToString();
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Grabar el nuevo comprobante
                this.dgDetalle.dsDatos.WriteXml(this.saveFileDialogGrabar.FileName);

                //Actualizar el titulo del comprobante con la descripcion del comprobante
                if (!duplicar) this.Text += this.txtDescripcion.Text;

                //Actualizar el listado de comprobantes del formulario frmCompExtContLista
                this.ActualizarFormularioListaComprobantes();
            }
            else
            {
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarComprobante()
        {
            try
            {
                //Leer el comprobante
                string ficheroComp = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtraContables"];
                ficheroComp = ficheroComp + "\\" + this.archivoComprobante;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoDesde"] = this.txtMaskAAPPDesde.Value.ToString();
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodoHasta"] = this.txtMaskAAPPHasta.Value.ToString();
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Grabar el nuevo comprobante
                this.dgDetalle.dsDatos.WriteXml(ficheroComp);

                //Actualizar el listado de comprobantes del formulario frmCompContLista
                this.ActualizarFormularioListaComprobantes();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el listado de comprobantes del formulario frmCompExtContLista
        /// </summary>
        private void ActualizarFormularioListaComprobantes()
        {
            if (Application.OpenForms["frmCompExtContLista"] != null)
            {
                if (this.Owner is IForm formInterface)
                    formInterface.ActualizaListaComprobantes();
            }
        }

        /// <summary>
        /// Habilitar / Deshabilitar los controles del formulario
        /// </summary>
        /// <param name="valor"></param>
        private void ControlesHabilitarDeshabilitar(bool valor)
        {
            this.txtMaskAAPP.Enabled = valor;
            this.dateTimePickerFecha.Enabled = valor;
            this.cmbTipo.Enabled = valor;
            this.txtNoComprobante.Enabled = valor;
            this.txtDescripcion.Enabled = valor;
            this.dgDetalle.Enabled = valor;

            utiles.ButtonEnabled(ref this.radButtonGrabar, valor);
            utiles.ButtonEnabled(ref this.radButtonGrabarComo, valor);
            utiles.ButtonEnabled(ref this.radButtonRevertir, valor);
            utiles.ButtonEnabled(ref this.radButtonValidar, valor);

            utiles.ButtonEnabled(ref this.radButtonExportar, valor);
            utiles.ButtonEnabled(ref this.radButtonImportar, valor);

            if (valor == false && this.dgErrores.Visible)
            {
                this.dgErrores.Visible = false;
                this.dgDetalle.Height = this.dgDetalle.Height + this.dgErrores.Height;
                utiles.ButtonEnabled(ref this.radButtonValidarErrores, valor);
            }
            
            this.menuGridButtonAdicionarFila.Enabled = valor;
            this.menuGridButtonBorrar.Enabled = valor;
            this.menuGridButtonBuscar.Enabled = valor;
            this.menuGridButtonCopiar.Enabled = valor;
            this.menuGridButtonCortar.Enabled = valor;
            this.menuGridButtonInsertarFila.Enabled = valor;
            this.menuGridButtonPegar.Enabled = valor;
            this.menuGridButtonSuprimirFila.Enabled = valor;
            this.toolStripInsertarFilacmbFilas.Enabled = valor;

            if (!valor) this.btnSel.Visible = false;
        }

        /// <summary>
        /// //Mostrar el botón de Selección en las coordenadas que le corresponde
        /// </summary>
        /// <param name="tgGridDetalles">Grid de detalles</param>
        private void BtnSelPosicion(TGGrid tgGridDetalles)
        {
            this.btnSel.Visible = false;
            //Application.DoEvents();

            DataGridViewCell cellPrimera = tgGridDetalles.FirstDisplayedCell;
            int widthOutScroll = tgGridDetalles.FirstDisplayedScrollingColumnHiddenWidth;
            int firstColumn = tgGridDetalles.FirstDisplayedScrollingColumnIndex;
            int firstRow = tgGridDetalles.FirstDisplayedScrollingRowIndex;

            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                //Calcular número de Fila a posicionar el botón
                this.btnSel.Top = this.dgDetalle.Top + tgGridDetalles.Rows[0].Height;
                long posLong = this.btnSel.Top;

                for (int i = 1; i < tgGridDetalles.Rows.Count; i++)
                {
                    if (celdaActiva.RowIndex == 0) break;
                    else
                    {
                        //posLong = this.btnSel.Top + tgGridDetalles.Rows[i].Height; SMR
                        posLong = posLong + tgGridDetalles.Rows[i].Height;
                        if (i == celdaActiva.RowIndex) break;
                    }
                }

                if (firstRow != 0)
                {
                    for (int i = 0; i < firstRow; i++)
                    {
                        posLong = posLong - tgGridDetalles.Rows[i].Height;
                    }
                }

                this.btnSel.Top = Convert.ToInt32(posLong) + 2;

                //------ SCROLL HORIZONTAL ------

                //Falta tener en cuenta la 1ra columna
                //Calcular número de Columna a posicionar el botón
                this.btnSel.Left = tgGridDetalles.Left + tgGridDetalles.Columns[0].Width - widthOutScroll;
                for (int i = 1; i < tgGridDetalles.Columns.Count; i++)
                {
                    if (celdaActiva.ColumnIndex == 0)
                    {
                        this.btnSel.Left = this.btnSel.Left + 15;
                        break;
                    }
                    else
                    {
                        //this.btnSel.Left = this.btnSel.Left + tgGridDetalles.Columns[i].Width; SMR
                        if (tgGridDetalles.Columns[i].Visible == true) this.btnSel.Left = this.btnSel.Left + tgGridDetalles.Columns[i].Width;
                        if (i == celdaActiva.ColumnIndex)
                        {
                            this.btnSel.Left = this.btnSel.Left + 15;
                            break;
                        }
                    }
                }

                if (firstColumn != 0)
                {
                    for (int i = 0; i < firstColumn; i++)
                    {
                        this.btnSel.Left = this.btnSel.Left - tgGridDetalles.Columns[i].Width;
                    }
                }

                //this.btnSel.Left = this.btnSel.Left + 5
                this.btnSel.Left = this.btnSel.Left + (tgGridDetalles.RowHeadersWidth - 35);

                this.btnSel.Visible = true;

                if (firstColumn > celdaActiva.ColumnIndex) this.btnSel.Visible = false;
                else
                {
                    //Analizar el scroll por la derecha
                    int widthVisible = 0;
                    if (firstColumn != -1)
                    {
                        for (int i = firstColumn; i < celdaActiva.ColumnIndex; i++)
                        {
                            widthVisible = widthVisible + this.dgDetalle.Columns[i].Width;
                        }
                    }

                    if (widthVisible > this.dgDetalle.Width) this.btnSel.Visible = false;
                }

                //------ SCROLL VERTICAL ------
                if (firstRow > celdaActiva.RowIndex) this.btnSel.Visible = false;
                else
                {
                    //Analizar el scroll por abajo
                    int altoVisible = 0;
                    for (int i = firstRow; i < celdaActiva.RowIndex; i++)
                    {
                        altoVisible = altoVisible + this.dgDetalle.Rows[i].Height;
                    }
                    //int altoHeader = this.dgDetalle.ColumnHeadersHeight;
                    if (altoVisible > this.dgDetalle.Height) this.btnSel.Visible = false;
                }
            }
        }

        /// <summary>
        /// Valida la existencia o no de la compañía. Si la encuentra recupera los valores NCIAMG, TITAMG, FELAMG, TIPLMG
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Valida la existencia o no de la compañía. Si la encuentra recupera los valores NCIAMG, TITAMG, FELAMG, TIPLMG</returns>
        private string ValidarCompania(string codigo)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                //Comprobar que la compañía es válida
                string query = "select NCIAMG, TITAMG, FELAMG, TIPLMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where STATMG = 'V' and CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue); 

                if (dr.Read())
                {
                    //Inicializar los valores que se necesitan en función de la compañía
                    this.GLM01_NCIAMG = dr["NCIAMG"].ToString().Trim();
                    this.GLM01_TITAMG = dr["TITAMG"].ToString().Trim();
                    this.GLM01_FELAMG = dr["FELAMG"].ToString().Trim();
                    this.GLM01_TIPLMG = dr["TIPLMG"].ToString().Trim();

                    result = "";
                }
                else
                {
                    //Error la compañía no es válida
                    result = this.LP.GetText("lblfrmCompContErrComp", "La compañía no es válida");
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que el año/período esté definido para la compañía
        /// </summary>
        /// <returns></returns>
        private string ValidarAAPP()
        {
            string result = "";

            //coger el valor sin la máscara
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string sap = this.txtMaskAAPP.Value.ToString();
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

            IDataReader dr = null;

            //Validar que el año/período esté definido para la compañía
            try
            {
                if (sap.Length == 0)
                {
                    result = this.LP.GetText("lblfrmCompContErrAAPPVacio", "Debe introducir el Año-Período");
                }
                else
                if (this.GLM01_TITAMG != null && this.GLM01_TITAMG != "")
                {
                    //Si la compañía es válida y está definido el TITAMG
                    //Buscar el siglo dado el año
                    string aa = sap.Substring(0, 2);
                    sap = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + sap;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where SAPRFL =" + sap + " and TITAFL = '" + this.GLM01_TITAMG + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue); 

                    if (!dr.Read())
                    {
                        result = this.LP.GetText("lblfrmCompContErrAAPP", "El Año-Período no es válido para la compañía seleccionada");
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrAAPPExcep", "Error al validar el Año-Período") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que el tipo sea correcto
        /// </summary>
        /// <returns></returns>
        private string ValidarTipo(string codigo)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select NOMBTV from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where CODITV = '1' and STATTV = 'V' and TIVOTV = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.GLT06_NOMBTV = dr["NOMBTV"].ToString().Trim();
                }
                else
                {
                    result = this.LP.GetText("lblfrmCompContErrTipo", "El tipo no es válido");
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrTipoExcep", "Error al validar el Tipo") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que la fecha sea correcta
        /// </summary>
        /// <returns></returns>
        private string ValidarFecha()
        {
            string result = "";
            IDataReader dr = null;

            //coger el valor sin la máscara
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string sap = this.txtMaskAAPP.Value.ToString();
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

            //Para esta validacion importa el formato de la fecha ??? o de la misma forma que se entra es en la que se almacena ???
            try
            {
                //Si hay SAAP definido y la compañía es válida y está definido el TITAMG
                if (sap.Length != 0 && this.GLM01_TITAMG != null && this.GLM01_TITAMG != "")
                {
                    //Coger el campo año período con siglo 
                    string aaCampoAP = sap.Substring(0, 2);
                    sap = utiles.SigloDadoAnno(aaCampoAP, CGParametrosGrles.GLC01_ALSIRC) + sap;

                    //Coger el dateTimePickerFecha y convertirlo a syymmdd para la select
                    int year = this.dateTimePickerFecha.Value.Year;
                    string aa = year.ToString();
                    if (aa.Length > 2) aa = aa.Substring(aa.Length - 2, 2);

                    string mes = this.dateTimePickerFecha.Value.Month.ToString();
                    mes = mes.PadLeft(2, '0');

                    string dia = this.dateTimePickerFecha.Value.Day.ToString();
                    dia = dia.PadLeft(2, '0');

                    string samd = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa + mes + dia;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + this.GLM01_TITAMG + "' and SAPRFL = " + sap + " and ";
                    query += "INLAFL <= " + samd + " and ";
                    query += "FINLFL >= " + samd;

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (!dr.Read())
                    {
                        result = this.LP.GetText("lblfrmCompContErrFecha", "La Fecha no está dentro del período");
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrFechaExcep", "Error al validar la Fecha") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar cuenta de mayor
        /// </summary>
        /// <returns></returns>
        private string ValidarCuentaMayor(string codigo)
        {
            string result = "";
            try
            {
                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where CUENMC = '" + codigo + "' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC = 'V'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        result = this.LP.GetText("lblfrmCompContErrCtaMayor", "La cuenta de mayor no es válida");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCtaMayorExcep", "Error al validar la cuenta de mayor") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Validar cuenta de auxiliar
        /// </summary>
        /// <param name="codigo">Cuenta de auxiliar</param>
        /// <param name="cuentaMayor">Cuenta de Mayor</param>
        /// <param name="ctaAuxiliar">Indica cual es la cuenta de auxiliar (1, 2 ó 3)</param>
        /// <returns></returns>
        private string ValidarCuentaAuxiliar(string codigo, string cuentaMayor, int ctaAuxiliar)
        {
            string result = "";
            codigo = codigo.ToUpper();
            try
            {
                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string queryTAUXMA = "";
                    switch (ctaAuxiliar)
                    {
                        case 1:
                            queryTAUXMA = "TAUXMA = TAU1MC";
                            break;
                        case 2:
                            queryTAUXMA = "TAUXMA = TAU2MC";
                            break;
                        case 3:
                            queryTAUXMA = "TAUXMA = TAU3MC";
                            break;
                    }

                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05, ";
                    query += GlobalVar.PrefijoTablaCG + "GLM03 " ;
                    query += "where CAUXMA = '" + codigo + "' and " + queryTAUXMA + " and " ;
                    query += "TIPLMC = '" + this.GLM01_TIPLMG + "' and CUENMC = '" + cuentaMayor + "' and ";
                    query += "STATMC = 'V'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        result = this.LP.GetText("lblfrmCompContErrCtaAuxiliar", "La cuenta de auxiliar no es válida");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCtaAuxiliarExcep", "Error al validar la cuenta de auxiliar") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Validar tipo de extracontable
        /// </summary>
        /// <param name="codigo">Código del extracontable</param>
        /// <returns></returns>
        private string ValidarTipoExtracontable(string codigo)
        {
            string result = "";
            codigo = codigo.ToUpper();
            try
            {
                string query = "select count (*) from ";
                query += GlobalVar.PrefijoTablaCG + "PRT03 ";
                query += "where TDATAH = '" + codigo + "' and " ;
                query += "STATAH = 'V' ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (!(cantRegistros > 0))
                {
                    result = this.LP.GetText("lblfrmCompExtContErrTipoExt", "El tipo de extracontable no es válido");  //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompExtContErrTipoExtExcep", "Error al validar el tipo de extracontable") + " (" + ex.Message + ")";    //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Descripcion del Tipo de Extracontable
        /// </summary>
        /// <param name="codigo">código del extracontable</param>
        /// <returns></returns>
        private string DescripcionTipoExtracontable(string codigo)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                codigo = codigo.ToUpper();

                //PRT03_NOMBAH
                string query = "select NOMBAH from ";
                query += GlobalVar.PrefijoTablaCG + "PRT03 ";
                query += "where TDATAH = '" + codigo + "' and ";
                query += "STATAH = 'V' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("NOMBAH")).ToString().Trim();
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
        
        /// <summary>
        /// Valida si es posible grabar el formulario
        /// </summary>
        /// <returns></returns>
        private string ValidarGrabarFormulario()
        {
            string result = "";

            string codigo = this.cmbCompania.SelectedValue.ToString();
            string validarCompania = this.ValidarCompania(codigo);
            if (validarCompania != "") validarCompania = "  - " + validarCompania + "\n\r";

            string validarAAPP = this.ValidarAAPP();
            if (validarAAPP != "") validarAAPP = "  - " + validarAAPP + "\n\r";

            string validarFecha = this.ValidarFecha();
            if (validarFecha != "") validarFecha = "  - " + validarFecha + "\n\r";

            result = validarCompania + validarAAPP + validarFecha;

            return (result);
        }

        /// <summary>
        /// Devuelve la query para el objeto TGElementosSel que se dibujará en la celda correspondiente 
        /// (dependiendo de la celda donde se hace el click)
        /// </summary>
        /// <param name="columna">Columna activa del Grid</param>
        /// <param name="fila">Fila activa del Grid</param>
        /// <param name="titulo">Devuelve el título para el formulario de selección</param>
        /// <param name="error">Devuelve si hubo error</param>
        /// <returns></returns>
        private string QuerySelElementos(int columna, int fila, ref string titulo, ref string error, string columnName)
        {
            string query = "";

            string cuentaMayor = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[fila][0].ToString();

            try
            {
                switch (columnName)
                {
                    case "Cuenta":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaMayor", "Seleccionar Cuenta de Mayor");

                            query = "select min(CUENMC) CUENMC, max(NOLAAD) NOLAAD, CEDTMC from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TCUEMC = 'D' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC != '*' ";
                            query += "group by CEDTMC order by CUENMC";
                            error = "";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoPlanCompania", "No está definido el tipo de plan para la compañía seleccionada");
                        }
                        break;
                    case "Auxiliar1":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux1", "Seleccionar Cuenta Auxiliar1");

                            query = "select CAUXMA, NOMBMA from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TAUXMA = TAU1MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            query += "order by NOMBMA";
                            error = "";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux1", "No está definido la cuenta de auxiliar1 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "Auxiliar2":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux2", "Seleccionar Cuenta Auxiliar2");

                            query = "select CAUXMA, NOMBMA from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TAUXMA = TAU2MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            query += "order by NOMBMA";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux2", "No está definido la cuenta de auxiliar2 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "Auxiliar3":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux3", "Seleccionar Cuenta Auxiliar3");

                            query = "select CAUXMA, NOMBMA from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TAUXMA = TAU3MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            query += "order by NOMBMA";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux3", "No está definido la cuenta de auxiliar3 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "TipoExt":
                        titulo = this.LP.GetText("lblSelTipoExtracontable", "Lista de Tipos de Extracontables");  //Falta traducir

                        query = "select TDATAH, NOMBAH from ";
                        query += GlobalVar.PrefijoTablaCG + "PRT03 ";
                        query += "where STATAH = 'V' ";
                        query += "order by TDATAH";

                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }

        /// <summary>
        /// Dado una cuenta y una fila, procesa las columnas (activas o no y valores por defecto)
        /// </summary>
        /// <param name="cuentaMayor">Código de la cuenta de mayor</param>
        /// <param name="filaIndex">Índice de la fila</param>
        /// <returns></returns>
        private string UpdateEstadoColumnasDadoCuentaMayor(string cuentaMayor, int filaIndex)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (cuentaMayor != "" && this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string query = "select TAU1MC, TAU2MC, TAU3MC, TDOCMC, NDDOMC, FEVEMC, ADICMC, TERMMC, RNITMC ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TCUEMC = 'D' and CUENMC = '" + cuentaMayor + "' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC = 'V'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    string TAU1MC = "";
                    string TAU2MC = "";
                    string TAU3MC = "";
                    string TDOCMC = "";
                    string NDDOMC = "";
                    string FEVEMC = "";
                    string ADICMC = "";
                    string TERMMC = "";
                    string RNITMC = "";
                    if (dr.Read())
                    {
                        TAU1MC = dr["TAU1MC"].ToString().Trim();
                        TAU2MC = dr["TAU2MC"].ToString().Trim();
                        TAU3MC = dr["TAU3MC"].ToString().Trim();
                        TDOCMC = dr["TDOCMC"].ToString().Trim();
                        NDDOMC = dr["NDDOMC"].ToString().Trim();
                        FEVEMC = dr["FEVEMC"].ToString().Trim();
                        ADICMC = dr["ADICMC"].ToString().Trim();
                        TERMMC = dr["TERMMC"].ToString().Trim();
                        RNITMC = dr["RNITMC"].ToString().Trim();
                    }
                    dr.Close();

                    if (TAU1MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar1", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar1");

                    if (TAU2MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar2", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar2");

                    if (TAU3MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar3", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar3");
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = this.LP.GetText("lblfrmCompContErrCtaMayorExcep", "Error al procesar la cuenta de mayor") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Calcula todos los totales partiendo de la Tabla de Detalle
        /// </summary>
        private void CalcularTotales()
        {
            try
            {
                decimal monedaLocal_Debe = 0;
                decimal monedaLocal_Haber = 0;
                string monedaLocal = "";
                decimal monedaLocalDec = 0;
                int noApuntes = 0;

                string nombreColumnaActual = "";
                if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
                {
                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                    {     
                        for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                        {
                            nombreColumnaActual = this.dgDetalle.dsDatos.Tables["Detalle"].Columns[j].ColumnName;
                            if (nombreColumnaActual.Length > 3)
                                if (nombreColumnaActual.Substring(0, 3) == prefijoColumnaPeriodo) 
                                {
                                    monedaLocal = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i][nombreColumnaActual].ToString().Trim();
                                    if ((monedaLocal != "") && (decimal.Parse(monedaLocal) != 0))
                                    {
                                        noApuntes++;

                                        try { monedaLocalDec = Convert.ToDecimal(monedaLocal); }
                                        catch(Exception ex) 
                                        {
                                            Log.Error(Utiles.CreateExceptionString(ex)); 
                                            monedaLocalDec = 0;
                                        }

                                        if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"] != null && this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"].ToString().Trim() != "")
                                        {
                                            //Es una columna de Periodos
                                            switch (this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"].ToString())
                                            {
                                                case "D":
                                                    monedaLocal_Debe += monedaLocalDec;
                                                    break;
                                                case "H":
                                                    monedaLocal_Haber += monedaLocalDec;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //No se especifica si el detalle va al Debe o al Haber. Se suma de acuerdo al signo del importe
                                            if (monedaLocalDec > 0) monedaLocal_Debe += monedaLocalDec;
                                            else if (monedaLocalDec < 0) monedaLocal_Haber += monedaLocalDec;
                                        }
                                    }
                                }                           
                        }
                    }
                }

                //Actualizar la tabla de Totales
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"] = monedaLocal_Debe.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"] = monedaLocal_Haber.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["NumeroApuntes"] = noApuntes.ToString();

                //Actualizar las etiquetas que almacenan los valores de los totales
                this.lblMonedaLocal_Debe.Text = monedaLocal_Debe.ToString("N2", this.LP.MyCultureInfo);
                this.lblMonedaLocal_Haber.Text = monedaLocal_Haber.ToString("N2", this.LP.MyCultureInfo);

                this.lblNoApuntes_Valor.Text = noApuntes.ToString();
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Muestra el Grid con los Errores de la Validación
        /// </summary>
        private void ViewGridErrores()
        {
            try
            {
                this.dgErrores.DataSource = this.comp.DSErrores.Tables["Errores"];
                this.dgErrores.Columns["CodiTipo"].Visible = false;
                this.dgErrores.Columns["CtrlCelda"].Visible = false;

                //Ajustar las columnas
                this.dgErrores.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Linea"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Error"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                this.dgDetalle.Height = this.dgDetalle.Size.Height - this.dgErrores.Size.Height;

                //Columnas
                this.dgErrores.Columns["Tipo"].HeaderText = this.LP.GetText("lblGridErrorCabTipo", "Tipo");
                this.dgErrores.Columns["Linea"].HeaderText = this.LP.GetText("lblGridErrorCabLinea", "Línea");
                this.dgErrores.Columns["Error"].HeaderText = this.LP.GetText("lblGridErrorCabError", "Error");

                this.dgErrores.Visible = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida que los periodos desde - hasta sean correctos
        /// </summary>
        /// <returns></returns>
        private bool ValidarPeriodos(ref string annoperiodoDesde, ref string annoperiodoHasta)
        {
            bool result = false;
            try
            {
                string error = this.LP.GetText("errValTitulo", "Error");

                //Validar periodo Desde
                annoperiodoDesde = "";
                //coger el valor sin la máscara
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                annoperiodoDesde = this.txtMaskAAPPDesde.Value.ToString();
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                string anno = "";
                string periodo = "";
                if (annoperiodoDesde.Length >= 4)
                {
                    anno = annoperiodoDesde.Substring(0, 2).Trim();
                    periodo = annoperiodoDesde.Substring(2, 2).Trim();
                }

                if (annoperiodoDesde.Trim() == "" || anno == "" || periodo == "")
                {
                    MessageBox.Show(this.LP.GetText("errPeriodoDesdeIncorecto", "Periodo desde incorrecto"), error);    //Falta traducir   132
                    this.txtMaskAAPPDesde.Focus();
                    return (result);
                }

                if (anno.Length < 2) anno = "0" + anno;
                if (periodo.Length < 2) periodo = "0" + periodo;

                annoperiodoDesde = utiles.SigloDadoAnno(anno, CGParametrosGrles.GLC01_ALSIRC) + annoperiodoDesde;

                if (!this.BuscarPeriodo(annoperiodoDesde))
                {
                    MessageBox.Show(this.LP.GetText("errPeriodoDesdeNoExiste", "Periodo desde inexistente"), error);    //Falta traducir  133
                    this.txtMaskAAPPDesde.Focus();
                    return (result);
                }

                //Validar periodo Hasta
                annoperiodoHasta = "";
                //coger el valor sin la máscara
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                annoperiodoHasta = this.txtMaskAAPPHasta.Value.ToString();
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (annoperiodoHasta.Length >= 4)
                {
                    anno = annoperiodoHasta.Substring(0, 2).Trim();
                    periodo = annoperiodoHasta.Substring(2, 2).Trim();
                }

                if (annoperiodoHasta.Trim() == "" || anno == "" || periodo == "")
                {
                    MessageBox.Show(this.LP.GetText("errPeriodoHastaIncorecto", "Periodo hasta incorrecto"), error);    //Falta traducir   134
                    this.txtMaskAAPPHasta.Focus();
                    return (result);
                }

                if (anno.Length < 2) anno = "0" + anno;
                if (periodo.Length < 2) periodo = "0" + periodo;

                annoperiodoHasta = utiles.SigloDadoAnno(anno, CGParametrosGrles.GLC01_ALSIRC) + annoperiodoHasta;

                if (!this.BuscarPeriodo(annoperiodoHasta))
                {
                    MessageBox.Show(this.LP.GetText("errPeriodoHastaNoExiste", "Periodo hasta inexistente"), error);    //Falta traducir   135
                    this.txtMaskAAPPHasta.Focus();
                    return (result);
                }

                if (Convert.ToInt32(annoperiodoDesde) > Convert.ToInt32(annoperiodoHasta))
                {
                    MessageBox.Show(this.LP.GetText("errPeriodoDesdeMayorPeriodoHasta", "Periodo desde mayor a periodo hasta"), error);    //Falta traducir  136
                    this.txtMaskAAPPDesde.Focus();
                    return (result);
                }

                result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Busca si el periodo está definido para la compañía
        /// </summary>
        /// <param name="annoperiodo"></param>
        /// <returns></returns>
        private bool BuscarPeriodo(string annoperiodo)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL='" + this.GLM01_TITAMG + "' and SAPRFL=" + annoperiodo;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = true;
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

        /// <summary>
        /// Busca todos los periodos comprendidos entre el periodo desde hasta el periodo hasta
        /// </summary>
        /// <param name="annoperiodoDesde"></param>
        /// <param name="annoperiodoHasta"></param>
        /// <returns></returns>
        private DataTable BuscaTodosPeriodos(string annoperiodoDesde, string annoperiodoHasta)
        {
            DataTable result = null;
            try
            {
                string query = "select SAPRFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL='" + this.GLM01_TITAMG + "' and ";
                query += "SAPRFL >=" + annoperiodoDesde + " and SAPRFL <=" + annoperiodoHasta;

                result = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Obtiene los periodos que serán generados
        /// </summary>
        /// <param name="annoperiodoDesde"></param>
        /// <param name="annoperiodoHasta"></param>
        private void ObtenerPeriodos(string annoperiodoDesde, string annoperiodoHasta)
        {
            try
            {
                //Eliminar periodos generados previamente
                this.periodosGenerados.Clear();
                DataTable todosPeriodos = this.BuscaTodosPeriodos(annoperiodoDesde, annoperiodoHasta);
                if (todosPeriodos != null)
                {
                    string periodoActual;
                    for (int i = 0; i < todosPeriodos.Rows.Count; i++)
                    {
                        periodoActual = todosPeriodos.Rows[i]["SAPRFL"].ToString();

                        if (periodoActual.Length == 5) periodoActual = periodoActual.Substring(1, 4);
                        this.periodosGenerados.Add(periodoActual);

                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Ordena las columnas de la Grid después de insertar columnas de periodos
        /// </summary>
        private void GridOrdenarColumnas()
        {
            try
            {
                int indiceColDesc = -1;
                int indiceActual = 0;
                string nombreColumnaActual = "";

                for (int i = 0; i < this.dgDetalle.Columns.Count; i++)
                {
                    nombreColumnaActual = this.dgDetalle.Columns[i].Name;
                    if (nombreColumnaActual == "Descripcion")
                    {
                        indiceColDesc = i;
                    }
                    else
                    {
                        this.dgDetalle.Columns[i].DisplayIndex = indiceActual;
                        indiceActual++;
                    }
                }

                if (indiceColDesc != -1) this.dgDetalle.Columns[indiceColDesc].DisplayIndex = indiceActual;
                this.dgDetalle.Refresh();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            try
            {
                if (this.cmbCompania.SelectedValue != null) this.cmbCompania.Tag = this.cmbCompania.SelectedValue.ToString();
                this.txtMaskAAPP.Tag = this.txtMaskAAPP.Text;
                this.dateTimePickerFecha.Tag = this.dateTimePickerFecha.Value;
                if (this.cmbTipo.SelectedValue != null) this.cmbTipo.Tag = this.cmbTipo.SelectedValue.ToString();
                this.txtNoComprobante.Tag = this.txtNoComprobante.Text;
                this.txtMaskAAPPDesde.Tag = this.txtMaskAAPPDesde.Text;
                this.txtMaskAAPPHasta.Tag = this.txtMaskAAPPHasta.Text;
                if (this.radDropDownListTipoDefecto.SelectedValue != null) this.radDropDownListTipoDefecto.Tag = this.radDropDownListTipoDefecto.SelectedValue.ToString();
                this.txtDescripcion.Tag = this.txtDescripcion.Text;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}