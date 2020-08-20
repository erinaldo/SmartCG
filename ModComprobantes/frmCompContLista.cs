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
    public partial class frmCompContLista : frmPlantilla, IReLocalizable, IForm
    {
        public string formCode = "MCCLISTA";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCCLISTA
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
            public string estado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string verNumerosComp;
        }

        FormularioValoresCampos valoresFormulario;

        DataTable dataTable;
        string pathFicherosCompContables = "";
        private string tipoBaseDatosCG = "";

        private string prefijoLote = "";

        ComprobanteContableTransferir compContTransf;

        private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 27);
        private static Size gridInfoSize = new Size(758, 480);
        private static int radCollapsiblePanelBuscadorExpandedHeight = 0;

        public frmCompContLista()
        {
            InitializeComponent();

            radCollapsiblePanelBuscadorExpandedHeight = this.radCollapsiblePanelBuscador.Height;
            this.radCollapsiblePanelBuscador.IsExpanded = false;
            this.radCollapsiblePanelBuscador.EnableAnimation = false;

            this.txtMaskAAPP.AutoSize = false;
            this.txtMaskAAPP.Size = new Size(this.txtMaskAAPP.Width, 30);

            this.txtMaskFechaDesde.AutoSize = false;
            this.txtMaskFechaDesde.Size = new Size(this.txtMaskFechaDesde.Width, 30);

            this.txtMaskFechaHasta.AutoSize = false;
            this.txtMaskFechaHasta.Size = new Size(this.txtMaskFechaHasta.Width, 30);

            this.cmbCompania.AutoSize = false;
            this.cmbCompania.Size = new Size(this.cmbCompania.Width, 30);

            this.cmbTipo.AutoSize = false;
            this.cmbTipo.Size = new Size(this.cmbTipo.Width, 30);

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

        //Actualizar el listado de comprobantes desde el formulario frmCompContAltaEdita.cs después de un alta o una actualización de comprobante
        void IForm.ActualizaListaComprobantes()
        {
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
            this.FillDataGrid();
        }

        private void FrmCompContLista_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de comprobantes contables");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            //this.KeyPreview = true;

            //Tipo de Base de Datos (dato necesario para Trasferir Comprobantes a Finanzas)
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    this.cmbEstado.SelectedIndex = 0;
                    this.rbGenerarLote.IsChecked= true;
                }
            }
            else
            {
                this.cmbEstado.SelectedIndex = 0;
                this.rbGenerarLote.IsChecked = true;
            }

            compContTransf = new ComprobanteContableTransferir
            {
                tipoBaseDatosCG = this.tipoBaseDatosCG,
                LP = this.LP
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

            //Cargar MEnu Click Derecho de la Grid
            this.BuildMenuClickDerecho();

            if (tipoBaseDatosCG != "DB2")
            {
                this.txtBibliotecaPrefijo.Text = ""; 
                this.txtCola.Text = "";
                this.txtBibliotecaCola.Text = "";
                this.txtBibliotecaPrefijo.Enabled = false;
                this.txtCola.Enabled = false;
                this.txtBibliotecaCola.Enabled = false;
            }
        }
       
        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Nuevo();
        }
        
        private void DgComprobantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarComprobante(this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow));
        }

        private void BtnTodos_Click(object sender, EventArgs e)
        {
            this.cmbCompania.Text = "";
            this.cmbCompania.SelectedIndex = -1;
            this.txtMaskAAPP.Value = null;
            this.cmbTipo.Text = "";
            this.cmbTipo.SelectedIndex = -1;
            this.txtMaskFechaDesde.Text = "";
            this.txtMaskFechaHasta.Text = "";
            this.txtDescripcionBusador.Text = "";

            this.chkNoTransferidos.Checked = false;
            this.chkTransferidos.Checked = false;

            this.dataTable.DefaultView.RowFilter = "";

            this.radCollapsiblePanelBuscador.Collapse();

            this.radGridViewComprobantes.Visible = true;
            this.Refresh();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Filtrar la Grid
                string filtro = "";
                string compania = "";
                string aapp = "";
                string tipo = "";
                string fechaDesde = "";
                string fechaHasta = "";
                string descripcion = "";

                compania = this.cmbCompania.Text.Trim();
                if (compania.Length > 2) compania = compania.Substring(0, 2);

                tipo = this.cmbTipo.Text.Trim();
                if (tipo.Length > 2) tipo = tipo.Substring(0, 2);

                string errorTitulo = this.LP.GetText("errValTitulo", "Error");
                DateTime dt;

                this.txtMaskFechaDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                fechaDesde = this.txtMaskFechaDesde.Value.ToString();
                this.txtMaskFechaDesde.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (fechaDesde != "")
                {
                    fechaDesde = this.txtMaskFechaDesde.Text.Trim();
                    try
                    {
                        dt = Convert.ToDateTime(fechaDesde);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        RadMessageBox.Show(this.LP.GetText("lblErrFechaDebeFormato", "La fecha desde no tiene un formato válido"), errorTitulo);
                        this.txtMaskFechaDesde.Focus();
                        return;
                    }
                }

                this.txtMaskFechaHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                fechaHasta = this.txtMaskFechaHasta.Value.ToString();
                this.txtMaskFechaHasta.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (fechaHasta != "")
                {
                    fechaHasta = this.txtMaskFechaHasta.Text.Trim();
                    try
                    {
                        dt = Convert.ToDateTime(fechaHasta);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        RadMessageBox.Show(this.LP.GetText("lblErrFechaHastaFormato", "La fecha hasta no tiene un formato válido"), errorTitulo);
                        this.txtMaskFechaHasta.Focus();
                        return;
                    }
                }

                descripcion = this.txtDescripcionBusador.Text.Trim();

                if (compania != "")
                {
                    filtro = string.Format("compania LIKE '%{0}%'", compania);
                }

                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                aapp = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (aapp != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("aapp LIKE '%{0}%'", aapp);
                }

                if (tipo != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("tipo LIKE '%{0}%'", tipo);
                }

                if (fechaDesde != "")
                {
                    if (fechaHasta != "")
                    {
                        //fecha entre desde y hasta
                        if (filtro != "") filtro += " AND ";
                        filtro += string.Format("fecha >= '{0}' AND fecha <= '{1}'", fechaDesde, fechaHasta);
                    }
                    else
                    {
                        //fecha mayor o igual que desde
                        if (filtro != "") filtro += " AND ";
                        filtro += string.Format("fecha >= '{0}'", fechaDesde);
                    }
                }
                else
                    if (fechaHasta != "")
                    {
                        //fecha menor o igual que hasta
                        if (filtro != "") filtro += " AND ";
                        filtro += string.Format("fecha <= '{0}'", fechaHasta);
                    }

                if (descripcion != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("descripcion LIKE '%{0}%'", descripcion);
                }

                if (chkTransferidos.Checked && chkNoTransferidos.Checked)
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += "transferido = 1 OR transferido = 0";
                }
                else
                {
                    if (chkNoTransferidos.Checked)
                    {
                        if (filtro != "") filtro += " AND ";
                        filtro += "transferido = 0";
                    }
                    else if (chkTransferidos.Checked)
                    {
                        if (filtro != "") filtro += " AND ";
                        filtro += "transferido = 1";
                    }
                }

                this.dataTable.DefaultView.RowFilter = filtro;
                this.radGridViewComprobantes.Refresh();

                if (this.dataTable.DefaultView.Count == 0 && filtro != "")
                {
                    this.radGridViewComprobantes.Visible = false;

                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(this.LP.GetText("lblErrNoComp", "No existen comprobantes para el criterio de búsqueda utilizado"), error);
                    this.cmbCompania.Select();
                }
                else
                {
                    this.radGridViewComprobantes.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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
                RadMessageBox.Show(result, error);
                return;
            }

            compContTransf.prefijoLote = this.txtPrefijo.Text;
            compContTransf.bibliotecaPrefijo = this.txtBibliotecaPrefijo.Text;
            compContTransf.biliotecaCola = this.txtBibliotecaCola.Text;
            compContTransf.cola = this.txtCola.Text;
            compContTransf.descripcion = this.txtDescripcion.Text;
            compContTransf.extendido = Convert.ToBoolean(this.radGridViewComprobantes.CurrentRow.Cells["extendido"].Value);

            result = compContTransf.VerificarExistenDatosLote();
            bool procesarComp = true;
            if (result != "")
            {
                //Hay datos del lote, pedir confirmación para borrarlos
                string mensaje = result + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿Desea eliminar los lotes?");
                DialogResult resultDialog = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (resultDialog == DialogResult.No) procesarComp = false;
                else
                {
                    //Eliminar los lotes de todas las tablas
                    try
                    {
                        result = "";
                        string eliminar = "";
                        for (int i = 0; i < compContTransf.tablasLotes.Count; i++)
                        {
                            eliminar += compContTransf.EliminarDatosLoteTabla(compContTransf.tablasLotes[i].ToString());
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
                RadMessageBox.Show(result, error);
                return;
            }

            if (procesarComp)
            {
                string compTransferidos = this.ProcesarComprobantes();
                if (compTransferidos != "" && compTransferidos != "-1")
                {
                    //Error transfiriendo los comprobantes
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(compTransferidos, error);
                }
                else
                {
                    if (this.rbGenerarLoteAdiciona.IsChecked && compTransferidos == "")
                    {
                        //Insertar registro en la tabla GLC04
                        compContTransf.tipoContabilizacion = this.cmbEstado.SelectedIndex + 1;
                        compContTransf.InsertarRegistroGLC04(); 
                    }

                    if (this.chkVerNocomp.Checked)
                    {
                        if (compTransferidos != "-1")
                        {
                            if (compContTransf.numCompGenerados != null && compContTransf.numCompGenerados.Rows.Count > 0)
                            {
                                //Mostrar los números de comprobantes generados
                                frmVisorNumComp frmVerNumComp = new frmVisorNumComp
                                {
                                    Datos = compContTransf.numCompGenerados,
                                    Extracontable = 0,
                                    FrmPadre = this
                                };
                                frmVerNumComp.ShowDialog();
                                compContTransf.numCompGenerados.Rows.Clear();
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
            this.radGridViewComprobantes.Height = 480;
            this.gbTransferirComp.Visible = false;
        }

        private void RbGenerarLoteAdiciona_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLoteAdiciona.IsChecked)
            {
                this.cmbEstado.Enabled = true;
                this.rbGenerarLote.IsChecked = false;
            }
            else
            {
                this.cmbEstado.Enabled = false;
            }
        }

        private void RbGenerarLote_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLote.IsChecked)
            {
                this.rbGenerarLoteAdiciona.IsChecked = false;
                this.cmbEstado.Enabled = false;
            }
        }

        private void TxtPrefijo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracter = e.KeyChar.ToString().ToUpper();
            e.KeyChar = Convert.ToChar(caracter);
        }

        private void TxtBibliotecaPrefijo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void BtnTransferir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnTransferir);
        }

        private void BtnTransferir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnTransferir);
        }

        private void BtnCancelar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnCancelar);
        }

        private void BtnCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnCancelar);
        }

        private void BtnBuscar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnBuscar);
        }

        private void BtnBuscar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnBuscar);
        }

        private void BtnTodos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnTodos);
        }

        private void BtnTodos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnTodos);
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

                this.radGridViewComprobantes.Focus();
                //this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, this.radGridViewAutSobreElementos.Size.Height + radCollapsiblePanelBuscadorExpandedHeight);
            }
        }

        private void RadCollapsiblePanelBuscador_Expanded(object sender, EventArgs e)
        {
            if (this.radGridViewComprobantes.Visible)
            {
                this.radGridViewComprobantes.Location = new Point(this.radGridViewComprobantes.Location.X, gridInfoLocation.Y + radCollapsiblePanelBuscadorExpandedHeight);
                if (this.gbTransferirComp.Visible) this.gbTransferirComp.Location = new Point(this.gbTransferirComp.Location.X, this.radGridViewComprobantes.Location.Y + this.radGridViewComprobantes.Size.Height + 10);

                this.cmbCompania.Focus();
                this.cmbCompania.Select();
                this.radGridViewComprobantes.Refresh();
            }
        }

        private void RbGenerarLoteAdiciona_Click(object sender, EventArgs e)
        {
            if (this.cmbEstado.Text == "") this.cmbEstado.SelectedIndex = 0;
        }

        private void RadGridViewComprobantes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarComprobante(this.radGridViewComprobantes.CurrentRow.Index);
        }

        private void RadGridViewComprobantes_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewComprobantes.Columns.Contains("fecha")) this.radGridViewComprobantes.Columns["fecha"].FormatString = "{0:dd/MM/yyyy}";
        }

        private void RadGridViewComprobantes_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void FrmCompContLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de comprobantes contables");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContListaTitulo", "Lista de Comprobantes Contables");

            //Traducir los literales de los encabezados de las columnas
            this.TraducirLiteralesDataGridHeader();

            //Traducir los literales de la transferencia de comprobantes
            this.gbLote.Text = this.LP.GetText("lblCompTransFinLoteBatch", "Lote Batch");
            this.lblPrefijo.Text = this.LP.GetText("lblCompTransFinPrefijo", "Prefijo");
            this.lblBibliotecaPrefijo.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinCola", "Cola de Salida");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.gbTransferencia.Text = this.LP.GetText("lblCompTransFinTipoTransf", "Tipo de Transferencia");
            this.rbGenerarLote.Text = this.LP.GetText("lblCompTransFinGenerarLoteB", "Solo Generar Lote");
            this.rbGenerarLoteAdiciona.Text = this.LP.GetText("lblCompTransFinGenerarLoteBA", "Generar Lote y Adicionar");

            //Desplegable de Estados (en el idioma que corresponda)

            this.cmbEstado.Items.Clear();
            var valores = new List<string>() {
                                  this.LP.GetText("lblCompTransFinNoAprob", "No Aprobado/s"),
                                  this.LP.GetText("lblCompTransFinAprob", "Aprobado/s"),
                                  this.LP.GetText("lblCompTransFinContab", "Contabilizado/s") };
            this.cmbEstado.Items.AddRange(valores);

            this.chkVerNocomp.Text = this.LP.GetText("lblCompTransFinVerNoComp", "Ver los números de comprobante");
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
                this.compContTransf.CrearColumnasDataTableComprobantes(ref this.dataTable);
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
            this.radGridViewComprobantes.Columns["clase"].HeaderText = this.LP.GetText("CompContdgHeaderClase", "Clase");
            this.radGridViewComprobantes.Columns["tasa"].HeaderText = this.LP.GetText("CompContdgHeaderTasa", "Tasa");
            this.radGridViewComprobantes.Columns["debeML"].HeaderText = this.LP.GetText("CompContdgHeaderDebeML", "Debe ML");
            this.radGridViewComprobantes.Columns["haberML"].HeaderText = this.LP.GetText("CompContdgHeaderHaberML", "Haber ML");
            this.radGridViewComprobantes.Columns["debeME"].HeaderText = this.LP.GetText("CompContdgHeaderDebeME", "Debe ME");
            this.radGridViewComprobantes.Columns["haberME"].HeaderText = this.LP.GetText("CompContdgHeaderHaberME", "Haber ME");
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
                pathFicherosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];
                //pathFicherosCompContables = GlobalVar.UsuarioEnv.ModComp_PathFicherosCompContables;

               DirectoryInfo dir = new DirectoryInfo(pathFicherosCompContables);
                FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo FI in fileList)
                { 
                    try { this.compContTransf.ProcesarFichero(FI.FullName, FI.Name, ref this.dataTable, false, true); }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }   
                }

                if (this.radGridViewComprobantes.Rows.Count > 0)    
                {
                    this.radGridViewComprobantes.Columns["extendido"].IsVisible = false;
                    this.radGridViewComprobantes.Columns["revertir"].IsVisible = false;
                    for (int i = 0; i < this.radGridViewComprobantes.Columns.Count; i++)
                        this.radGridViewComprobantes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    this.radGridViewComprobantes.MasterTemplate.BestFitColumns();
                    this.radGridViewComprobantes.Rows[0].IsCurrent = true;
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

                    this.lblNoHayComp.Text = this.LP.GetText("errNoExistencomprobantes", "No existen comprobantes en la ruta") + " " + pathFicherosCompContables;
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
            this.radLabelCompania.Visible = valor;
            this.cmbCompania.Visible = valor;
            this.lblAAPP.Visible = valor;
            this.txtMaskAAPP.Visible = valor;
            this.lblTipo.Visible = valor;
            this.cmbTipo.Visible = valor;
            this.lblFecha.Visible = valor;
            this.txtMaskFechaDesde.Visible = valor;
            this.txtMaskFechaHasta.Visible = valor;
            this.btnBuscar.Visible = valor;
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
                 
                frmCompContAltaEdita frmAltaEdita = new frmCompContAltaEdita
                {
                    NuevoComprobante = false,
                    //frmAltaEdita.NombreComprobante = descripcion;
                    NombreComprobante = archivo,
                    ArchivoComprobante = archivo,
                    Batch = true,
                    BatchLote = false,
                    BatchLoteError = false,
                    Compania = this.radGridViewComprobantes.Rows[rowIndex].Cells["compania"].Value.ToString(),
                    FrmPadre = this
                };
                frmAltaEdita.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
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
            string query = "Select CCIAMG, NCIAMG From " + GlobalVar.PrefijoTablaCG + "GLM01 Order by CCIAMG";
            string result = this.FillComboBox(query, "CCIAMG", "NCIAMG", ref this.cmbCompania, true, -1, false);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetCompanias", "Error obteniendo las compañías") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Cargar los tipos de comprobantes
        /// </summary>
        private void FillTiposComprobantes()
        {
            // desde Gestion de comprobantes,
            string query = "Select TIVOTV, NOMBTV From " + GlobalVar.PrefijoTablaCG + "GLT06 ";
            query += " WHERE CODITV = '0'";
            query += "Order by TIVOTV";
            string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, false);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de comprobantes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Importa el comprobante seleccionado
        /// </summary>
        private void ImportarComprobantes()
        {
            //Llama al formulario para importar comprobantes
            frmCompContImportarDeFinanzas frmImportarDeFinanzas = new frmCompContImportarDeFinanzas
            {
                FrmPadre = this,
                VentanaFlotante = true
            };
            frmImportarDeFinanzas.Show();

            //Reacargar los comprobantes
            //Volver a cargar la lista de comprobantes
            this.dataTable.Rows.Clear();
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
                DialogResult res = RadMessageBox.Show(mensaje, advertenciaTitulo, MessageBoxButtons.YesNo);
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
                                string fichero = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"] + "\\" + archivo;
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
            if (this.radCollapsiblePanelBuscador.IsExpanded) this.radCollapsiblePanelBuscador.Collapse();
            this.radGridViewComprobantes.Height = gridInfoSize.Height - this.gbTransferirComp.Height;
            this.gbTransferirComp.Location = new Point(this.gbTransferirComp.Location.X, this.radGridViewComprobantes.Location.Y + this.radGridViewComprobantes.Size.Height + 10);
            this.gbTransferirComp.Visible = true;
            this.txtDescripcion.Focus();
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
                    currentRow = this.radGridViewComprobantes.SelectedRows[i].Index;
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
            string result = "";

            string compTranferidos = "-1";
            try
            {
                string archivo = "";
                string descCompArchivo = "";
                int currentRow;

                string revertir = "";
                bool transfComp = true;
                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    currentRow = this.radGridViewComprobantes.SelectedRows[i].Index;

                    revertir = this.radGridViewComprobantes.Rows[currentRow].Cells["revertir"].Value.ToString();

                    if (revertir == "S" || revertir == "T") 
                    {
                        //Pedir confirmacion para la reversión
                        string mensaje = this.LP.GetText("wrn1RervertirComp", "El comprobante") + " " + this.radGridViewComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString() + " " + this.LP.GetText("wrn2RervertirComp", "es de reversión. ¿Desea transferirlo?");
                        DialogResult resultDialog = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (resultDialog == DialogResult.No) transfComp = false;
                        else transfComp = true;

                    }
                    else transfComp = true;

                    if (transfComp)
                    {
                        archivo = this.radGridViewComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                        compContTransf.archivo = archivo;
                        descCompArchivo = this.radGridViewComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString();
                        compContTransf.descCompArchivo = descCompArchivo;

                        result = this.TransferirCabecera(currentRow, false);

                        if (result == "")
                        {
                            //Transferir los detalles
                            compTranferidos = "";

                            //Leer el comprobante
                            DataSet ds = new DataSet();
                            ds.ReadXml(pathFicherosCompContables + "\\" + archivo);

                            //Verificar que exista la tabla de Detalle
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables["Detalle"].Rows.Count; j++)
                                {
                                    if (!TodaFilaEnBlanco(ds.Tables["Detalle"], j))
                                    {
                                        result = this.TransferirDetalle(ds.Tables["Detalle"].Rows[j], false);

                                        if (result != "")
                                        {
                                            string error = this.LP.GetText("errValTitulo", "Error");
                                            RadMessageBox.Show(result, error);
                                            break;
                                        }
                                    }
                                }
                            }

                            //Actualizar el fichero xml para que el campo transferido tenga el valor 1
                            if (result == "" && ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                            {
                                ds.Tables["Cabecera"].Rows[0]["Transferido"] = "1";
                                ds.WriteXml(pathFicherosCompContables + "\\" + archivo);
                            }
                        }
                        else
                        {
                            string error = this.LP.GetText("errValTitulo", "Error");
                            RadMessageBox.Show(result, error);
                        }

                        if (revertir == "S" || revertir == "T")
                        {
                            //SMR - Revertir
                            archivo = this.radGridViewComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                            compContTransf.archivo = archivo;
                            descCompArchivo = this.radGridViewComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString();
                            compContTransf.descCompArchivo = descCompArchivo;

                            result = this.TransferirCabecera(currentRow, true);

                            if (result == "")
                            {
                                //Transferir los detalles
                                compTranferidos = "";

                                //Leer el comprobante
                                DataSet ds = new DataSet();
                                ds.ReadXml(pathFicherosCompContables + "\\" + archivo);

                                //Verificar que exista la tabla de Detalle
                                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds.Tables["Detalle"].Rows.Count; j++)
                                    {
                                        if (!TodaFilaEnBlanco(ds.Tables["Detalle"], j))
                                        {
                                            result = this.TransferirDetalle(ds.Tables["Detalle"].Rows[j], true);

                                            if (result != "")
                                            {
                                                string error = this.LP.GetText("errValTitulo", "Error");
                                                RadMessageBox.Show(result, error);
                                                break;
                                            }
                                        }
                                    }
                                }

                                //Actualizar el fichero xml para que el campo transferido tenga el valor 1
                                if (result == "" && ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                                {
                                    ds.Tables["Cabecera"].Rows[0]["Transferido"] = "1";
                                    ds.WriteXml(pathFicherosCompContables + "\\" + archivo);
                                }
                            }
                            else
                            {
                                string error = this.LP.GetText("errValTitulo", "Error");
                                RadMessageBox.Show(result, error);
                            }
                        }
                        //END SMR - Revertir
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
                if (table.Rows[row][i].ToString() != "")
                {
                    todaFilaBlanco = false;
                    break;
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// Transfiere la cabecera del comprobante
        /// </summary>
        /// <param name="currentRow">fila de la grid de comprobantes</param>
        /// <returns></returns>
        private string TransferirCabecera(int currentRow, Boolean esReversion)
        {
            string result = "";
            try
            {
                string aapp = "";
                string fechaCad = "";
                DateTime fecha;

                compContTransf.TTRAWS = "1";

                compContTransf.revertir = this.radGridViewComprobantes.Rows[currentRow].Cells["revertir"].Value.ToString();
                compContTransf.CCIAWS = this.radGridViewComprobantes.Rows[currentRow].Cells["compania"].Value.ToString().ToUpper();
                aapp = this.radGridViewComprobantes.Rows[currentRow].Cells["aapp"].Value.ToString();
                aapp = aapp.Replace("-", "");
                if (aapp.Length >= 2)
                {
                    compContTransf.ANOCWS = aapp.Substring(0, 2);
                    compContTransf.LAPSWS = aapp.Substring(aapp.Length - 2, 2);
                }

                compContTransf.TICOWS = this.radGridViewComprobantes.Rows[currentRow].Cells["tipo"].Value.ToString();
                compContTransf.NUCOWS = this.radGridViewComprobantes.Rows[currentRow].Cells["noComp"].Value.ToString();
                compContTransf.TVOUWS = this.radGridViewComprobantes.Rows[currentRow].Cells["clase"].Value.ToString();

                fechaCad = this.radGridViewComprobantes.Rows[currentRow].Cells["fecha"].Value.ToString();
                if (fechaCad != "")
                {
                    try
                    {
                        //fecha = Convert.ToDateTime(utiles.FormatoCGToFecha(fechaCad));
                        fecha = Convert.ToDateTime(fechaCad);
                        compContTransf.DIAEWS = fecha.Day.ToString();
                        compContTransf.MESEWS = fecha.Month.ToString();
                        compContTransf.ANOEWS = fecha.Year.ToString();

                        if (compContTransf.ANOEWS.Length > 2) compContTransf.ANOEWS = compContTransf.ANOEWS.Substring(compContTransf.ANOEWS.Length - 2, 2);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        compContTransf.DIAEWS = "";
                        compContTransf.MESEWS = "";
                        compContTransf.ANOEWS = "";
                    }
                }

                compContTransf.TASCWS = this.radGridViewComprobantes.Rows[currentRow].Cells["tasa"].Value.ToString();
                compContTransf.DOCDWS = this.radGridViewComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString().Replace("'", "''");
                /*compContTransf.DOCDWS = this.txtDescripcion.Text.Replace("'", "''");*/
                compContTransf.extendido = Convert.ToBoolean(this.radGridViewComprobantes.Rows[currentRow].Cells["extendido"].Value);

                result = compContTransf.TransferirCabecera(esReversion);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errTransferCabComp", "Error transfiriendo la cabecera del comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Transfiere la cabecera del comprobante
        /// </summary>
        /// <param name="indexRow">fila de la grid de comprobantes</param>
        /// <returns></returns>
        private string TransferirDetalle(DataRow row, Boolean esReversion)
        {
            string result = "";
            try
            {
                string documento = "";
                compContTransf.TTRAWS = "2";

                try { compContTransf.CUENWS = row["Cuenta"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    //compContTransf.PRFDWS = "";     //¿?¿?PRFDWS o CUENWS ??
                    compContTransf.CUENWS = "";
                }
                try { compContTransf.CAUXWS = row["Auxiliar1"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.CAUXWS = "";
                }
                try { compContTransf.DESCWS = row["Descripcion"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.DESCWS = "";
                }
                compContTransf.DESCWS = compContTransf.DESCWS.Replace("'", "''");
                try { compContTransf.MONTWS = row["MonedaLocal"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.MONTWS = "";
                }
                try { compContTransf.TMOVWS = row["DH"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TMOVWS = "";
                }
                try { compContTransf.MOSMWS = row["MonedaExt"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.MOSMWS = "";
                }
                try { documento = row["Documento"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    documento = "";
                }

                if (documento.Length > 2)
                {
                    compContTransf.CLDOWS = documento.Substring(0, 2).ToUpper();
                    compContTransf.NDOCWS = documento.Substring(2, documento.Length - 3);
                    if (compContTransf.NDOCWS == "") compContTransf.NDOCWS = "0";
                }
                else
                {
                    compContTransf.CLDOWS = documento.ToUpper();
                    compContTransf.NDOCWS = "0";
                }

                try { compContTransf.FDOCWS = row["Fecha"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.FDOCWS = "0";
                }
                if (compContTransf.FDOCWS == "") compContTransf.FDOCWS = "0";
                try { compContTransf.FEVEWS = row["Vencimiento"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.FEVEWS = "0";
                }
                if (compContTransf.FEVEWS == "") compContTransf.FEVEWS = "0";

                //try
                //{
                //    if (compContTransf.FDOCWS != "")
                //    {
                //        fecha = Convert.ToDateTime(compContTransf.FDOCWS);
                //        compContTransf.FDOCWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                //    }
                //    else compContTransf.FDOCWS = "0";
                //    if (compContTransf.FEVEWS != "")
                //    {
                //        fecha = Convert.ToDateTime(compContTransf.FEVEWS);
                //        compContTransf.FEVEWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                //    }
                //    else compContTransf.FEVEWS = "0";
                //}
                //catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try { compContTransf.TEINWS = row["RU"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TEINWS = "";
                }
                try { compContTransf.NNITWS = row["CifDni"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.NNITWS = "";
                }
                try { compContTransf.AUA1WS = row["Auxiliar2"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.AUA1WS = "";
                }
                try { compContTransf.AUA2WS = row["Auxiliar3"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.AUA2WS = "";
                }
                try { documento = row["Documento2"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    documento = "";
                }

                if (documento.Length > 2)
                {
                    compContTransf.CDDOWS = documento.Substring(0, 2).ToString();
                    compContTransf.NDDOWS = documento.Substring(2, documento.Length - 3);
                    if (compContTransf.NDDOWS == "") compContTransf.NDDOWS = "0";
                }
                else
                {
                    compContTransf.CDDOWS = documento.ToString();
                    compContTransf.NDDOWS = "0";
                }
                try { compContTransf.TERCWS = row["Importe3"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TERCWS = "";
                }
                try { compContTransf.CDIVWS = row["Iva"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.CDIVWS = "";
                }

                //Insertar detalle del comprobante
                if (compContTransf.extendido)
                {
                    try { compContTransf.PRFDWS = row["PrefijoDoc"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.PRFDWS = "";
                    }
                    try { compContTransf.NFAAWS = row["NumFactAmp"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.NFAAWS = "";
                    }
                    try { compContTransf.NFARWS = row["NumFactRectif"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.NFARWS = "";
                    }
                    try { compContTransf.FIVAWS = row["FechaServIVA"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.FIVAWS = "0";
                    }
                    if (compContTransf.FIVAWS == "") compContTransf.FIVAWS = "0";
                    try { compContTransf.USA1WS = row["CampoUserAlfa1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA1WS = "";
                    }
                    try { compContTransf.USA2WS = row["CampoUserAlfa2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA2WS = "";
                    }
                    try { compContTransf.USA3WS = row["CampoUserAlfa3"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA3WS = "";
                    }
                    try { compContTransf.USA4WS = row["CampoUserAlfa4"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA4WS = "";
                    }
                    try { compContTransf.USA5WS = row["CampoUserAlfa5"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA5WS = "";
                    }
                    try { compContTransf.USA6WS = row["CampoUserAlfa6"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA6WS = "";
                    }
                    try { compContTransf.USA7WS = row["CampoUserAlfa7"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA7WS = "";
                    }
                    try { compContTransf.USA8WS = row["CampoUserAlfa8"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA8WS = "";
                    }
                    try { compContTransf.USN1WS = row["CampoUserNum1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USN1WS = "";
                    }
                    try { compContTransf.USN2WS = row["CampoUserNum2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USN2WS = "";
                    }
                    try { compContTransf.USF1WS = row["CampoUserFecha1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USF1WS = "0";
                    }
                    if (compContTransf.USF1WS == "") compContTransf.USF1WS = "0";
                    try { compContTransf.USF2WS = row["CampoUserFecha2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USF2WS = "0";
                    }
                    if (compContTransf.USF2WS == "") compContTransf.USF2WS = "0";
                }

                result = compContTransf.TransferirDetalle(esReversion);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error transfiriendo el detalle del comprobante (" + ex.Message + ")"; //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Llama al formulario de entrada de un nuevo comprobante
        /// </summary>
        private void Nuevo()
        {
            frmCompContAltaEdita frmAltaEdita = new frmCompContAltaEdita
            {
                NuevoComprobante = true,
                Batch = true,
                FrmPadre = this
            };
            frmAltaEdita.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
            frmAltaEdita.Show(this);
        }
        private void ActualizaListaComprobantes_ArgSel(frmCompContAltaEdita.ActualizaListaComprobantesArgs e)
        {
            try
            {
                string Cab_compania = e.Valor[0].ToString().Trim();
                string Cab_anoperiodo = e.Valor[1].ToString().Trim();
                string Cab_tipo = e.Valor[2].ToString().Trim();
                string Cab_noComprobante = e.Valor[3].ToString().Trim();
                string Cab_fecha = utiles.FechaToFormatoCG(e.Valor[4].ToString().Trim()).ToShortDateString();
                //string Cab_fecha = e.Valor[4].ToString().Trim();
                string Cab_compania_ant = e.Valor[5].ToString().Trim();
                string Cab_anoperiodo_ant = e.Valor[6].ToString().Trim();
                string Cab_tipo_ant = e.Valor[7].ToString().Trim();
                string Cab_noComprobante_ant = e.Valor[8].ToString().Trim();
                DateTime fecha = utiles.FechaCadenaToDateTime(e.Valor[9].ToString());
                string Cab_fecha_ant = fecha.ToString();
                string Cab_extendido = e.Valor[10].ToString().Trim();
                string lblTotalDebe = e.Valor[11].ToString().Trim();
                string lblTotalHaber = e.Valor[12].ToString().Trim();
                string lblExtDebe = e.Valor[13].ToString().Trim();
                string lblExtHaber = e.Valor[14].ToString().Trim();
                string lblImporte3Debe = e.Valor[15].ToString().Trim();
                string lblImporte3Haber = e.Valor[16].ToString().Trim();
                string numapuntes = e.Valor[17].ToString().Trim();
                string descripcion = e.Valor[18].ToString().Trim();
                string clase = e.Valor[19].ToString().Substring(0, 1).Trim();
                string tasa = e.Valor[20].ToString().Trim();

                // busco fila seleccionada.
                for (int i = 0; i < this.radGridViewComprobantes.Rows.Count; i++)
                {
                    if ((this.radGridViewComprobantes.Rows[i].Cells["compania"].Value.ToString().Trim() == Cab_compania_ant)
                    && (this.radGridViewComprobantes.Rows[i].Cells["aapp"].Value.ToString().Trim() == Cab_anoperiodo_ant)
                    && (this.radGridViewComprobantes.Rows[i].Cells["tipo"].Value.ToString().Trim() == Cab_tipo_ant)
                    && (this.radGridViewComprobantes.Rows[i].Cells["noComp"].Value.ToString().Trim() == Cab_noComprobante_ant)
                    && (this.radGridViewComprobantes.Rows[i].Cells["fecha"].Value.ToString().Trim() == Cab_fecha_ant)
                    )
                    {
                        this.radGridViewComprobantes.Rows[i].Cells["compania"].Value = Cab_compania;
                        this.radGridViewComprobantes.Rows[i].Cells["aapp"].Value = Cab_anoperiodo;
                        this.radGridViewComprobantes.Rows[i].Cells["tipo"].Value = Cab_tipo;
                        this.radGridViewComprobantes.Rows[i].Cells["noComp"].Value = Cab_noComprobante;
                        this.radGridViewComprobantes.Rows[i].Cells["fecha"].Value = Cab_fecha;
                        this.radGridViewComprobantes.Rows[i].Cells["Clase"].Value = clase;
                        this.radGridViewComprobantes.Rows[i].Cells["Tasa"].Value = tasa;
                        this.radGridViewComprobantes.Rows[i].Cells["descripcion"].Value = descripcion;
                        this.radGridViewComprobantes.Rows[i].Cells["DebeML"].Value = lblTotalDebe.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["HaberML"].Value = lblTotalHaber.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["DebeME"].Value = lblExtDebe.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["HaberME"].Value = lblExtHaber.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["noMovimiento"].Value = numapuntes;
                    }
                }

                /* MessageBox.Show("Compañia:" + Cab_compania + "\n\r" +
                                "Periodo:" + Cab_anoperiodo + "\n\r" +
                                "Tipo CMP:" + Cab_tipo + "\n\r" +
                                "Num. CMP:" + Cab_noComprobante + "\n\r" +
                                "Fecha:" + Cab_fecha + "\n\r" +
                                "Cmp Ext:" + Cab_extendido + "\n\r" +
                                "Total Debe:" + lblTotalDebe + "\n\r" +
                                "Total Haber:" + lblTotalHaber + "\n\r" +
                                "Mon.Extr. Debe:" + lblExtDebe + "\n\r" +
                                "Mon.Extr. Haber:" + lblExtHaber + "\n\r" +
                                "Importe3 Debe:" + lblImporte3Debe + "\n\r" +
                                "Importe3 Haber:" + lblImporte3Haber + "\n\r" +
                                "Num.Apuntes:" + numapuntes); */

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
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
            else this.EditarComprobante(this.radGridViewComprobantes.CurrentRow.Index);
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
                StructGLL01_MCCLISTA myStruct = (StructGLL01_MCCLISTA)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCCLISTA));

                this.txtDescripcion.Text = myStruct.descripcion.Trim();
                this.txtPrefijo.Text = myStruct.prefijo.Trim();
                this.txtBibliotecaPrefijo.Text = myStruct.bilbiotecaPrefijo.Trim();
                this.txtCola.Text = myStruct.colaSalida.Trim();
                this.txtBibliotecaCola.Text = myStruct.bibliotecaColaSalida.Trim();

                if (myStruct.generarLoteBatch == "1")
                {
                    this.rbGenerarLote.IsChecked = true;
                    this.rbGenerarLoteAdiciona.IsChecked = false;
                    this.cmbEstado.SelectedIndex = -1;
                    this.cmbEstado.Enabled = false;
                }

                if (myStruct.generarLoteAdiciona == "1")
                {
                    this.rbGenerarLote.IsChecked = false;
                    this.rbGenerarLoteAdiciona.IsChecked = true;
                    try
                    {
                        if (myStruct.estado.Trim() != "") this.cmbEstado.SelectedValue = myStruct.estado.Trim();
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.cmbEstado.Enabled = true;
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
                StructGLL01_MCCLISTA myStruct;

                myStruct.descripcion = this.txtDescripcion.Text.PadRight(36, ' ');
                myStruct.prefijo = this.txtPrefijo.Text.PadRight(2, ' ');
                myStruct.bilbiotecaPrefijo = this.txtBibliotecaPrefijo.Text.PadRight(10, ' ');
                myStruct.colaSalida = this.txtCola.Text.PadRight(10, ' ');
                myStruct.bibliotecaColaSalida = this.txtBibliotecaCola.Text.PadRight(10, ' ');

                if (this.rbGenerarLote.IsChecked)
                {
                    myStruct.generarLoteBatch = "1";
                    myStruct.generarLoteAdiciona = "0";
                    myStruct.estado = " ";
                }
                else
                {
                    myStruct.generarLoteBatch = "0";
                    myStruct.generarLoteAdiciona = "1";
                    myStruct.estado = this.cmbEstado.SelectedValue.ToString();
                }

                if (this.chkVerNocomp.Checked) myStruct.verNumerosComp = "1";
                else myStruct.verNumerosComp = "0";

                result = myStruct.descripcion + myStruct.prefijo + myStruct.bilbiotecaPrefijo + myStruct.colaSalida;
                result += myStruct.bibliotecaColaSalida + myStruct.generarLoteBatch + myStruct.generarLoteAdiciona;
                result += myStruct.estado + myStruct.verNumerosComp;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCCLISTA));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemNuevo = new RadMenuItem("Nuevo");
            menuItemNuevo.Click += new EventHandler(RadButtonNuevo_Click);
            RadMenuItem menuItemEditar = new RadMenuItem("Editar");
            menuItemEditar.Click += new EventHandler(RadButtonEditar_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemNuevo);
            this.radContextMenuClickDerecho.Items.Add(menuItemEditar);
        }
        #endregion
    }
}
