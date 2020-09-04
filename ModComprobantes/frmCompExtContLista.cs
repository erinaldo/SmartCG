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
using System.Collections;
using System.Runtime.InteropServices;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ModComprobantes
{
    public partial class frmCompExtContLista : frmPlantilla, IReLocalizable, IForm
    {
        public string formCode = "MCELISTA";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCELISTA
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string descripcion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string prefijo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string bilbiotecaPrefijo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string colaSalida;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string bibliotecaColaSalida;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string generarLoteBatch;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string generarLoteAdiciona;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string verNumerosComp;
        }

        FormularioValoresCampos valoresFormulario;

        DataTable dataTable;
        string pathFicherosCompExtContables = "";
        private string tipoBaseDatosCG = "";

        private string prefijoLote = "";

        ComprobanteExtContableTransferir compExtContTransf;
		
		private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 27);
        private static Size gridInfoSize = new Size(758, 480);
        private static int radCollapsiblePanelBuscadorExpandedHeight = 0;

        public frmCompExtContLista()
        {
            InitializeComponent();
			
            this.gbLote.ElementTree.EnableApplicationThemeName = false;
            this.gbLote.ThemeName = "ControlDefault";

            this.gbTransferencia.ElementTree.EnableApplicationThemeName = false;
            this.gbTransferencia.ThemeName = "ControlDefault";

            this.gbTransferirComp.ElementTree.EnableApplicationThemeName = false;
            this.gbTransferirComp.ThemeName = "ControlDefault";

            this.radGridViewComprobantes.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        //Actualizar el listado de comprobantes desde el formulario frmCompExtContAltaEdita.cs después de un alta o una actualización de comprobante
        void IForm.ActualizaListaComprobantes()
        {
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
            //this.dgComprobantes.Rows.Clear();
            this.FillDataGrid();
        }

        private void FrmCompExtContLista_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de Comprobantes Extracontables");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            //this.KeyPreview = true;

            //Tipo de Base de Datos (dato necesario para Trasferir Comprobantes a Finanzas)
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores)) this.rbGenerarLote.IsChecked = true;
            }
            else this.rbGenerarLote.IsChecked = true;

            compExtContTransf = new ComprobanteExtContableTransferir
            {
                tipoBaseDatosCG = this.tipoBaseDatosCG
            };

            //Crear el DataGrid y sus columnas
            this.BuildDataGrid();

            //Llenar el DataGrid
            this.FillDataGrid();

            //Chequear que existan comprobantes
            this.VerificarExistenciaComprobantes();

            this.TraducirLiterales();

            //Cargar compañías
            this.FillCompanias();

            //Cargar Tipos
            this.FillTiposComprobantes();
        }
        
        private void DgComprobantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarComprobante(this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow));
        }

        private void MenuGridButtonNuevo_Click(object sender, EventArgs e)
        {
            this.Nuevo();
        }

        private void MenuGridButtonEditar_Click(object sender, EventArgs e)
        {
            this.Editar();
        }

        private void MenuGridButtonSuprimir_Click(object sender, EventArgs e)
        {
            this.EliminarComprobantes();
        }

        private void MenuGridButtonImportar_Click(object sender, EventArgs e)
        {
            this.ImportarComprobantes();
        }

        private void MenuGridButtonTransferir_Click(object sender, EventArgs e)
        {
            this.TransferirVisualizarControles();
        }

        private void MenuGridButtonAjustar_Click(object sender, EventArgs e)
        {
        }

        private void BtnTransferir_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            this.prefijoLote = this.txtPrefijo.Text.Trim().ToUpper();

            string result = this.ValidarForm();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                return;
            }

            compExtContTransf.prefijoLote = this.txtPrefijo.Text.Trim();
            compExtContTransf.bibliotecaPrefijo = this.txtBibliotecaPrefijo.Text.Trim().ToUpper();
            compExtContTransf.biliotecaCola = this.txtBibliotecaCola.Text.Trim().ToUpper();
            compExtContTransf.cola = this.txtCola.Text;
            compExtContTransf.descripcion = this.txtDescripcion.Text;

            result = compExtContTransf.VerificarExistenDatosLote();
            bool procesarComp = true;
            if (result != "")
            {
                //Hay datos del lote, pedir confirmación para borrarlos
                string mensaje = result + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿Desea eliminar los lotes?");
                DialogResult resultDialog = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                result = "";
                if (resultDialog == DialogResult.No) procesarComp = false;
                else
                {
                    //Eliminar los lotes de todas las tablas
                    try
                    {
                        result = "";
                        string eliminar = "";
                        for (int i = 0; i < compExtContTransf.tablasLotes.Count; i++)
                        {
                            eliminar += compExtContTransf.EliminarDatosLoteTabla(compExtContTransf.tablasLotes[i].ToString());
                        }

                        if (eliminar != "")
                        {
                            result += eliminar;
                            procesarComp = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        result += this.LP.GetText("errDelLotesTabla", "Error eliminando lotes de las tablas") + " (" + ex.Message + ")" + "\n\r";
                        procesarComp = false;
                    }
                }
            }

            result += this.VerificarNumerosDocumentos();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                return;
            }

            if (procesarComp)
            {
                string compTransferidos = this.ProcesarComprobantes();
                if (compTransferidos != "" && compTransferidos != "-1")
                {
                    //Error transfiriendo los comprobantes
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(compTransferidos, error);
                }
                else
                {
                    if (this.rbGenerarLoteAdiciona.IsChecked && compTransferidos == "")
                    {
                        //Insertar registro en la tabla PRC04
                        compExtContTransf.InsertarRegistroPRC04();
                    }

                    if (this.chkVerNocomp.Checked)
                    {
                        if (compTransferidos != "-1")
                        {
                            if (compExtContTransf.numCompGenerados != null && compExtContTransf.numCompGenerados.Rows.Count > 0)
                            {
                                //Mostrar los números de comprobantes generados
                                frmVisorNumComp frmVerNumComp = new frmVisorNumComp
                                {
                                    Datos = compExtContTransf.numCompGenerados,
                                    Extracontable = 1,
                                    FrmPadre = this
                                };
                                frmVerNumComp.ShowDialog();
                                compExtContTransf.numCompGenerados.Rows.Clear();

                            }
                            else
                            {
                                //Mostrar mensaje de transferido OK
                                if (this.radGridViewComprobantes.SelectedRows.Count == 1) RadMessageBox.Show(this.LP.GetText("lblCompTransferidoOK", "El comprobante se transfirió correctamente"), "");
                                else if (this.radGridViewComprobantes.SelectedRows.Count > 1) RadMessageBox.Show(this.LP.GetText("lblCompsTransferidosOK", "Los comprobantes se transfirieron correctamente"), "");
                            }
                        }
                        else
                        {
                            //Mostrar mensaje de NO transferido
                            if (this.radGridViewComprobantes.SelectedRows.Count == 1) RadMessageBox.Show(this.LP.GetText("lblCompNoTransferido", "El comprobante no se pudo transferir"), this.LP.GetText("errValTitulo", "Error"));
                            else if (this.radGridViewComprobantes.SelectedRows.Count > 1) RadMessageBox.Show(this.LP.GetText("lblCompsNoTransferidos", "Existen comprobantes que no se pudieron transferir"), this.LP.GetText("errValTitulo", "Error"));
                        }
                    }
                    else
                    {
                        //Mostrar mensaje de transferido OK
                        if (this.radGridViewComprobantes.SelectedRows.Count == 1) RadMessageBox.Show(this.LP.GetText("lblCompTransferidoOK", "El comprobante se transfirió correctamente"), "");
                        else if (this.radGridViewComprobantes.SelectedRows.Count > 1) RadMessageBox.Show(this.LP.GetText("lblCompsTransferidosOK", "Los comprobantes se transfirieron correctamente"), "");
                    }

                    //Actualizar la Grid
                    if (this.dataTable != null)
                    {
                        if (this.radGridViewComprobantes.SelectedRows.Count > 1 || compTransferidos != "-1")
                        {
                            //Al cambiar la seleccion del estado de los comprobantes (transferidos o no transferidos) volver a buscar los comprobantes
                            this.dataTable.Rows.Clear();
                            this.FillDataGrid();
                        }
                    }
                }
            }

            //Grabar la petición
            string valores = this.ValoresPeticion();

            this.valoresFormulario.GrabarParametros(formCode, valores);

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            //this.radGridViewComprobantes.Height = 480;
            this.gbTransferirComp.Visible = false;
            utiles.ButtonEnabled(ref this.radButtonTransferir, true);
            //this.Refresh();

            this.radPanelApp.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            this.radPanelApp.Height = this.radPanelApp.Height + this.gbTransferirComp.Height;
            //this.radPanelApp.Refresh();
            //this.Refresh();
        }

        private void RbGenerarLoteAdiciona_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLoteAdiciona.IsChecked)
            {
                this.rbGenerarLote.IsChecked = false;
            }
        }
		
		private void RbGenerarLote_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLote.IsChecked)
            {
                this.rbGenerarLoteAdiciona.IsChecked = false;
            }
        }
		
        private void TxtPrefijo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracter = e.KeyChar.ToString().ToUpper();
            e.KeyChar = Convert.ToChar(caracter);
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.Nuevo();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.Editar();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarComprobantes();
        }

        private void RadButtonImportar_Click(object sender, EventArgs e)
        {
            this.ImportarComprobantes();
        }

        private void RadButtonTransferir_Click(object sender, EventArgs e)
        {
            this.TransferirVisualizarControles();
        }

        private void RadCollapsiblePanelBuscador_Collapsed(object sender, EventArgs e)
        {
            if (primeraLlamada) primeraLlamada = false;
            else
            {
                this.radGridViewComprobantes.Location = gridInfoLocation;
                //Es necesario volver asignar el mismo valor porque en la primera ejecución no lo asigna bien (resta 54 al valor inicial)
                this.radGridViewComprobantes.Location = gridInfoLocation;

                this.radGridViewComprobantes.Size = gridInfoSize;

                if (this.gbTransferirComp.Visible) this.gbTransferirComp.Location = new Point(this.gbTransferirComp.Location.X, this.radGridViewComprobantes.Location.Y + this.radGridViewComprobantes.Size.Height + 10);

                //this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, this.radGridViewAutSobreElementos.Size.Height + radCollapsiblePanelBuscadorExpandedHeight);
            }
        }

        private void RadCollapsiblePanelBuscador_Expanded(object sender, EventArgs e)
        {
            if (this.radGridViewComprobantes.Visible)
            {
                this.radGridViewComprobantes.Location = new Point(this.radGridViewComprobantes.Location.X, gridInfoLocation.Y + radCollapsiblePanelBuscadorExpandedHeight);
                if (this.gbTransferirComp.Visible) this.gbTransferirComp.Location = new Point(this.gbTransferirComp.Location.X, this.radGridViewComprobantes.Location.Y + this.radGridViewComprobantes.Size.Height + 10);

                this.radGridViewComprobantes.Refresh();
            }
        }

        private void BtnCancelar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnCancelar);
        }

        private void BtnCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnCancelar);
        }

        private void BtnTransferir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnTransferir);
        }

        private void BtnTransferir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnTransferir);
        }

        private void RadButtonNuevo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNuevo);
        }

        private void RadButtonNuevo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNuevo);
        }

        private void RadButtonEditar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEditar);
        }

        private void RadButtonEditar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEditar);
        }

        private void RadButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void RadButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
        }

        private void RadButtonImportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonImportar);
        }

        private void RadButtonImportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonImportar);
        }

        private void RadButtonTransferir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonTransferir);
        }

        private void RadButtonTransferir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonTransferir);
        }

        private void RadGridViewComprobantes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarComprobante(this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow));
        }

        private void RadGridViewComprobantes_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewComprobantes.Columns.Contains("fecha")) this.radGridViewComprobantes.Columns["fecha"].FormatString = "{0:dd/MM/yyyy}";
        }

        private void MenuGridButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtBibliotecaPrefijo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracter = e.KeyChar.ToString().ToUpper();
            e.KeyChar = Convert.ToChar(caracter);
        }
        
        private void FrmCompExtContLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de Comprobantes Extracontables");
        }

        private void radGridViewComprobantes_Leave(object sender, EventArgs e)
        {
            utiles.guardarLayout(this.Name, ref radGridViewComprobantes);
        }

        private void radButtonActualizarLista_Click(object sender, EventArgs e)
        {
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
            this.FillDataGrid();
        }

        private void radGridViewComprobantes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow) >= 0)
                {
                    this.EditarComprobante(this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow));
                }
            }
        }

        private void radGridViewComprobantes_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompExtContListaTitulo", "Lista de Comprobantes Extracontables");

            //Traducir los Literales de los ToolStrip
            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "&Nuevo"); ;
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "&Editar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripSuprimir", "E&liminar");
            this.radButtonImportar.Text = this.LP.GetText("toolStripImportar", "&Importar"); 
            this.radButtonTransferir.Text = this.LP.GetText("toolStripTransferir", "&Transferir");
            
            //Traducir los literales de los encabezados de las columnas
            this.TraducirLiteralesDataGridHeader();

            this.gbLote.Text = this.LP.GetText("lblCompTransFinLoteBatch", "Lote Batch");
            this.lblPrefijo.Text = this.LP.GetText("lblCompTransFinPrefijo", "Prefijo");
            this.lblBibliotecaPrefijo.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinCola", "Cola de Salida");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.gbTransferencia.Text = this.LP.GetText("lblCompTransFinTipoTransf", "Tipo de Transferencia");
            this.rbGenerarLote.Text = this.LP.GetText("lblCompTransFinGenerarLoteB", "Generar Lote Batch");
            this.rbGenerarLoteAdiciona.Text = this.LP.GetText("lblCompTransFinGenerarLoteBA", "Generar Lote Batch y Adicionar");

            this.chkVerNocomp.Text = this.LP.GetText("lblCompTransFinVerNoComp", "Ver los números de comprobate");
            this.btnTransferir.Text = this.LP.GetText("lblCompTransFinTransferir", "Transferir");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }

        /// <summary>
        /// Construye el dataGrid
        /// </summary>
        private void BuildDataGrid()
        {
            this.dataTable = new DataTable();

            try
            {
                //Adicionar las columnas al DataTable
                this.compExtContTransf.CrearColumnasDataTableComprobantes(ref this.dataTable);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radGridViewComprobantes.DataSource = this.dataTable;
        }

        /// <summary>
        /// Traduce los literales del Encabezado del DataGrid
        /// </summary>
        private void TraducirLiteralesDataGridHeader()
        {
			this.radGridViewComprobantes.Columns["archivo"].HeaderText = this.LP.GetText("CompContdgHeaderArchivo", "Archivo");
            this.radGridViewComprobantes.Columns["compania"].HeaderText = this.LP.GetText("CompContdgHeaderCompania", "Compañía");
            this.radGridViewComprobantes.Columns["aapp"].HeaderText = this.LP.GetText("CompContdgHeaderAAPP", "AA-PP");
            this.radGridViewComprobantes.Columns["descripcion"].HeaderText = this.LP.GetText("CompContdgHeaderDescripcion", "Descripción");
            this.radGridViewComprobantes.Columns["tipo"].HeaderText = this.LP.GetText("CompContdgHeaderTipo", "Tipo");
            this.radGridViewComprobantes.Columns["noComp"].HeaderText = this.LP.GetText("CompContdgHeaderNoComp", "No Comp.");
            this.radGridViewComprobantes.Columns["fecha"].HeaderText = this.LP.GetText("CompContdgHeaderFecha", "Fecha");
            this.radGridViewComprobantes.Columns["debeML"].HeaderText = this.LP.GetText("CompContdgHeaderDebeML", "Debe ML");
            this.radGridViewComprobantes.Columns["haberML"].HeaderText = this.LP.GetText("CompContdgHeaderHaberML", "Haber ML");
            this.radGridViewComprobantes.Columns["noMovimiento"].HeaderText = this.LP.GetText("CompContdgHeaderNoMovimiento", "No Movimiento");
            this.radGridViewComprobantes.Columns["transferido"].HeaderText = this.LP.GetText("CompContdgHeaderTransferido", "Transferido");
        }

        /// <summary>
        /// Rellena el DataGrid con la información correspondiente
        /// </summary>
        private void FillDataGrid()
        {
            //Leer todos los ficheros con extensión xml que existan dentro de la carpeta ModComp_PathFicherosCompContables
            try
            {
                pathFicherosCompExtContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtraContables"];
                DirectoryInfo dir = new DirectoryInfo(pathFicherosCompExtContables);
                FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo FI in fileList)
                {
                    try { this.compExtContTransf.ProcesarFichero(FI.FullName, FI.Name, ref this.dataTable, false, true); }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
				
				if (this.radGridViewComprobantes.Rows.Count > 0)    
                {
                    /*
                    for (int i = 0; i < this.radGridViewComprobantes.Columns.Count; i++)
                        this.radGridViewComprobantes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    this.radGridViewComprobantes.MasterTemplate.BestFitColumns();
                    this.radGridViewComprobantes.Rows[0].IsCurrent = true;
                    */
                    for (int i = 0; i < this.radGridViewComprobantes.Columns.Count; i++)
                    {
                        this.radGridViewComprobantes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        //this.radGridViewInfo.Columns[i].BestFit();
                        this.radGridViewComprobantes.Columns[i].Width = 600;
                    }

                    this.radGridViewComprobantes.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewComprobantes.AllowSearchRow = true;
                    this.radGridViewComprobantes.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewComprobantes.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewComprobantes.AllowEditRow = false;
                    this.radGridViewComprobantes.EnableFiltering = true;
                    //this.radGridViewInfo.MasterView.TableFilteringRow.IsVisible = false;

                    //this.radGridViewInfo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    //this.radGridViewInfo.MasterTemplate.BestFitColumns();
                    this.radGridViewComprobantes.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    if (this.radGridViewComprobantes.Groups.Count == 0) this.radGridViewComprobantes.Rows[0].IsCurrent = true;
                    this.radGridViewComprobantes.Focus();
                    this.radGridViewComprobantes.Select();
                    //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radPanelApp.Size.Height - this.radCollapsiblePanelDataFilter.Size.Height - 3);
                    //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, 609);

                    //cargar layout
                    utiles.cargarLayout(this.Name, ref radGridViewComprobantes);

                    this.radGridViewComprobantes.Refresh();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Verifica que existan comprobantes. Si no existen, lo informa en el formulario
        /// </summary>
        private void VerificarExistenciaComprobantes()
        {
            try
            {
				if (this.radGridViewComprobantes.Rows.Count <= 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                    utiles.ButtonEnabled(ref this.radButtonImportar, false);
                    utiles.ButtonEnabled(ref this.radButtonTransferir, false);
                    this.radGridViewComprobantes.Visible = false;

                    this.lblNoHayComp.Text = this.LP.GetText("errNoExistencomprobantes", "No existen comprobantes en la ruta") + " " + pathFicherosCompExtContables;
                    this.lblNoHayComp.Visible = true;

                    //Desactivar los controles del buscador
                    this.ActivarDesactivarBuscador(false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                    utiles.ButtonEnabled(ref this.radButtonImportar, true);
                    utiles.ButtonEnabled(ref this.radButtonTransferir, true);
                    this.radGridViewComprobantes.Visible = true;

                    this.lblNoHayComp.Text = "";
                    this.lblNoHayComp.Visible = false;

                    //Activar los controles del buscador
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
        }

        /// <summary>
        /// Llama al formulario de Editar Comprobante
        /// </summary>
        /// <param name="rowIndex">índice de la fila seleccionada de la Grid de comprobantes</param>
        private void EditarComprobante(int rowIndex)
        {
			if (this.radGridViewComprobantes.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewComprobantes.CurrentRow.IsExpanded) this.radGridViewComprobantes.CurrentRow.IsExpanded = false;
                else this.radGridViewComprobantes.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewComprobantes.CurrentRow is GridViewDataRowInfo)
            {
                string archivo = this.radGridViewComprobantes.Rows[rowIndex].Cells["archivo"].Value.ToString();
                string descripcion = this.radGridViewComprobantes.Rows[rowIndex].Cells["descripcion"].Value.ToString();

                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                frmCompExtContAltaEdita frmAltaEdita = new frmCompExtContAltaEdita
                {
                    NuevoComprobante = false,
                    NombreComprobante = descripcion,
                    ArchivoComprobante = archivo,
                    FrmPadre = this
                };
                frmAltaEdita.Show(this);

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
        }

        /// <summary>
        /// Cargar los tipos de comprobantes
        /// </summary>
        private void FillTiposComprobantes()
        {
        }

        /// <summary>
        /// Importa el comprobante seleccionado
        /// </summary>
        private void ImportarComprobantes()
        {
            //Llama al formulario para importar comprobantes
            frmCompExtContImportarDeFinanzas frmImportarDeFinanzas = new frmCompExtContImportarDeFinanzas
            {
                FrmPadre = this,
                VentanaFlotante = true
            };
            frmImportarDeFinanzas.ShowDialog();

            //Reacargar los comprobantes
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
            //this.dgComprobantes.Rows.Clear();
            this.FillDataGrid();
        }

        /// <summary>
        /// Elimina los comprobantes seleccionados
        /// </summary>
        private void EliminarComprobantes()
        {
            try
            {
                bool varios = false;
                if (this.radGridViewComprobantes.SelectedRows.Count > 1) varios = true;  //Varios comprobantes
                else varios = false;    //Un solo comprobante

                //Pedir confirmación
                string mensaje = "";
                string advertencia1 = "";
                string advertencia2 = this.LP.GetText("wrnSuprimirComprobantePregunta", "¿Desea continuar?");

                string archivo = "";

                if (varios)
                {
                    advertencia1 = this.LP.GetText("wrnSuprimirVariosComprobantes", "comprobantes se van a eliminar");
                    mensaje = this.radGridViewComprobantes.SelectedRows.Count.ToString() + " " + advertencia1 + ". " + advertencia2;
                }
                else
                {
                    advertencia1 = this.LP.GetText("wrnSuprimirComprobante", "Se va a eliminar el comprobante");
                    archivo = this.radGridViewComprobantes.Rows[this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow)].Cells["archivo"].Value.ToString();
                    mensaje = advertencia1 + " \"" + archivo + " \"." + advertencia2;
                }

                string advertenciaTitulo = this.LP.GetText("wrnTitulo", "Advertencia");
                DialogResult res = MessageBox.Show(mensaje, advertenciaTitulo, MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
					using (this.radGridViewComprobantes.DeferRefresh())
                    {
                        foreach (Telerik.WinControls.UI.GridViewRowInfo row in this.radGridViewComprobantes.SelectedRows)
                        {
                            archivo = row.Cells["archivo"].Value.ToString();
                            //Eliminar el archivo físico del comprobante
                            try
                            {
                                string fichero = ConfigurationManager.AppSettings["ModComp_PathFicherosCompExtContables"] + "\\" + archivo;
                                File.Delete(fichero);
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.radGridViewComprobantes.Rows.Remove(row);
                        }
                    }

                    //Verificar existencia de comprobantes
                    this.VerificarExistenciaComprobantes();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(this.LP.GetText("errDelComp", "Error eliminando comprobantes") + " (" + ex.Message + ")", error);
            }
        }

        /// <summary>
        /// Visualiza los controles para la transferencia
        /// </summary>
        private void TransferirVisualizarControles()
        {
            this.radPanelApp.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.radPanelApp.Height = this.radPanelApp.Height - this.gbTransferirComp.Height;
            //this.radPanelApp.Refresh();

            //this.radGridViewComprobantes.Height = gridInfoSize.Height - this.gbTransferirComp.Height;
            this.gbTransferirComp.Location = new Point(this.gbTransferirComp.Location.X, this.radGridViewComprobantes.Location.Y + this.radGridViewComprobantes.Size.Height + 70);
            this.gbTransferirComp.Visible = true;
            //this.Refresh();

            this.txtDescripcion.Focus();

            utiles.ButtonEnabled(ref this.radButtonTransferir, false);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private string ValidarForm()
        {
            string result = "";

            if (this.radGridViewComprobantes.SelectedRows.Count <= 0)
            {
                result += " - " + this.LP.GetText("errSelUnComp", "Debe seleccionar al menos un comprobante") + " \n\r";
                this.radGridViewComprobantes.Focus();
            }

            if (this.prefijoLote == "")
            {
                result += " - " + this.LP.GetText("errPrefijoVacio", "El prefijo no puede estar en blanco") + " \n\r";
                this.txtPrefijo.Focus();
            }

            if (tipoBaseDatosCG == "DB2")
            {
                if (this.txtBibliotecaPrefijo.Text == "")
                {
                    result += " - " + this.LP.GetText("errBibliotecaVacia", "La biblioteca no puede estar en blanco") + " \n\r";
                    this.txtBibliotecaPrefijo.Focus();
                }

                if (this.txtCola.Text != "" && this.txtBibliotecaCola.Text == "")
                {
                    result += " - " + this.LP.GetText("errColaSalidaSinBilio", "Si la Cola de salida está informada, también tiene que estar informada la biblioteca") + " \n\r";
                    this.txtBibliotecaCola.Focus();
                }
            }

            string prefijo = this.txtPrefijo.Text.Trim().ToUpper();
            if (prefijo == "CS" || prefijo == "EX" || prefijo == "GL" || prefijo == "PR")
            {
                result += " - " + this.LP.GetText("errPrefLoteReservado", "Prefijo de lote reservado") + " \n\r";
                this.txtPrefijo.Focus();
            }

            return (result);
        }

        /// <summary>
        /// Verifica que exista la tabla 'prefijo'W00
        /// </summary>
        /// <returns></returns>
        private string VerificarExistaTablaPrefijoLote()
        {
            string result = "";

            try
            {
                bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, this.prefijoLote + "W00");
                if (!existeTabla)
                {
                    result = " - " + this.LP.GetText("errNoExisteTablaParaPrefLote", "No existen las tablas para el prefijo del lote") + " \n\r";
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result += " - " + this.LP.GetText("errVerificarExisteTablaPrefLote", "Error chequeando que existan las tablas para el prefijo del lote") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }

        /// <summary>
        /// Verifica que los números de documentos que han sido informados, sean correctos
        /// </summary>
        /// <returns></returns>
        private string VerificarNumerosDocumentos()
        {
            string result = "";

            IDataReader dr = null;

            int currentRow;
            string numComprobante;
            string ccia;
            string sapr;
            string tico;
            string query;
            try
            {
                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    //currentRow = this.radGridViewComprobantes.SelectedRows[i].Index;
                    currentRow = this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.SelectedRows[i]);
                    numComprobante = this.radGridViewComprobantes.Rows[currentRow].Cells["noComp"].Value.ToString();

                    if (numComprobante != "")
                    {
                        //Verificar que el numero de comprobante no este en uso
                        ccia = this.radGridViewComprobantes.Rows[currentRow].Cells["compania"].Value.ToString().ToUpper();
                        sapr = this.radGridViewComprobantes.Rows[currentRow].Cells["aapp"].Value.ToString().Trim();
                        sapr = sapr.Replace("-", "");
                        sapr = utiles.SigloDadoAnno(sapr.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + sapr;
                        tico = this.radGridViewComprobantes.Rows[currentRow].Cells["tipo"].Value.ToString();

                        query = "select NUCOIC from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                        query += "where CCIAIC = '" + ccia + "' and SAPRIC =" + sapr + " and TICOIC = " + tico;
                        query += " and NUCOIC = " + numComprobante;

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result += " - " + this.LP.GetText("errNumCompExiste", "El número de comprobante ya está asignado") + " (" + numComprobante + ")" + "\n\r";
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result += " - " + this.LP.GetText("errVerificarNumComp", "Error verificando los números de comprobantes") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }


        /// <summary>
        /// Transferir los comprobantes marcados
        /// </summary>
        private string ProcesarComprobantes()
        {
            string compTranferidos = "-1";

            try
            {
                string archivo = "";
                string descCompArchivo = "";
                int currentRow;

                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    //currentRow = this.radGridViewComprobantes.SelectedRows[i].Index;
                    currentRow = this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.SelectedRows[i]);

                    archivo = this.radGridViewComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                    compExtContTransf.archivo = archivo;
                    descCompArchivo = this.radGridViewComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString();
                    compExtContTransf.descCompArchivo = descCompArchivo;

                    //Leer el comprobante
                    DataSet ds = new DataSet();
                    ds.ReadXml(pathFicherosCompExtContables + archivo);

                    compExtContTransf.dsComprobante = ds;

                    compTranferidos = compExtContTransf.TransferirComprobante();

                    //Actualizar el fichero xml para que el campo transferido tenga el valor 1
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                    {
                        ds.Tables["Cabecera"].Rows[0]["Transferido"] = "1";
                        //Escribir el número de documento si fue calculado  (NO SE HACE)
                        //if (noComp != "") ds.Tables["Cabecera"].Rows[0]["Numero"] = noComp.ToString();
                        //ds.WriteXml(this.txtPath.Text + archivo);
                        ds.WriteXml(pathFicherosCompExtContables + archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                compTranferidos = this.LP.GetText("errTransferComp", "Error transfiriendo los comprobantes") + " (" + ex.Message + ")";
            }

            return (compTranferidos);
        }

        /// <summary>
        /// Actualiza los controles con los valores de la última petición
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCELISTA myStruct = (StructGLL01_MCELISTA)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCELISTA));

                this.txtDescripcion.Text = myStruct.descripcion.Trim();
                this.txtPrefijo.Text = myStruct.prefijo.Trim();
                this.txtBibliotecaPrefijo.Text = myStruct.bilbiotecaPrefijo.Trim();
                this.txtCola.Text = myStruct.colaSalida.Trim();
                this.txtBibliotecaCola.Text = myStruct.bibliotecaColaSalida.Trim();

                if (myStruct.generarLoteBatch == "1")
                {
                    this.rbGenerarLote.IsChecked = true;
                    this.rbGenerarLoteAdiciona.IsChecked = false;
                }

                if (myStruct.generarLoteAdiciona == "1")
                {
                    this.rbGenerarLote.IsChecked = false;
                    this.rbGenerarLoteAdiciona.IsChecked = true;
                }

                if (myStruct.verNumerosComp == "1") this.chkVerNocomp.Checked = true;
                else this.chkVerNocomp.Checked = false;

                result = true;

                Marshal.FreeBSTR(pBuf);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve una  cadena con todos los valores del formulario para grabar en la tabla de peticiones GLL01
        /// </summary>
        /// <returns></returns>
        private string ValoresPeticion()
        {
            string result = "";
            try
            {
                StructGLL01_MCELISTA myStruct;

                myStruct.descripcion = this.txtDescripcion.Text.PadRight(36, ' ');
                myStruct.prefijo = this.txtPrefijo.Text.PadRight(2, ' ');
                myStruct.bilbiotecaPrefijo = this.txtBibliotecaPrefijo.Text.PadRight(10, ' ');
                myStruct.colaSalida = this.txtCola.Text.PadRight(10, ' ');
                myStruct.bibliotecaColaSalida = this.txtBibliotecaCola.Text.PadRight(10, ' ');

                if (this.rbGenerarLote.IsChecked)
                {
                    myStruct.generarLoteBatch = "1";
                    myStruct.generarLoteAdiciona = "0";
                }
                else
                {
                    myStruct.generarLoteBatch = "0";
                    myStruct.generarLoteAdiciona = "1";
                }

                if (this.chkVerNocomp.Checked) myStruct.verNumerosComp = "1";
                else myStruct.verNumerosComp = "0";

                result = myStruct.descripcion + myStruct.prefijo + myStruct.bilbiotecaPrefijo + myStruct.colaSalida;
                result += myStruct.bibliotecaColaSalida + myStruct.generarLoteBatch + myStruct.generarLoteAdiciona;
                result += myStruct.verNumerosComp;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCELISTA));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }


        /// <summary>
        /// Llama al formulario de entrada de un nuevo comprobante
        /// </summary>
        private void Nuevo()
        {
            frmCompExtContAltaEdita frmAltaEdita = new frmCompExtContAltaEdita
            {
                NuevoComprobante = true,
                FrmPadre = this
            };
            frmAltaEdita.Show(this);
        }

        /// <summary>
        /// Llama al formulario de edición de un comprobante
        /// </summary>
        private void Editar()
        {
            if (this.radGridViewComprobantes.SelectedRows.Count > 1)
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(this.LP.GetText("lblErrSelSoloUnComp", "Debe seleccionar un solo comprobante"), error);
            }
            else this.EditarComprobante(this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow));
        }
        #endregion

    }
}