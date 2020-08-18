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
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLT06Sel: frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOTCOMSE";

        private string autClaseElemento = "004";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        //private string autOperAlta = "";

        private string codTipoComp = "";

        private Dictionary<string, string> displayNames;
        private DataTable dtTiposComp = new DataTable();

        public frmMtoGLT06Sel()
        {
            InitializeComponent();

            this.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Traducir los literales de las filas del DataGrid
            //this.TraducirLiteralesFilasDataGrid();
        }

        private void FrmMtoGLT06Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Tipos de Comprobantes Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Crear el TGBuscador
            //this.BuildtgBuscadorTiposComp();

            //Crear el TGGrid
            this.BuildtgGridTiposComp();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            this.radGridViewTiposComp.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewTiposComp.AllowSearchRow = true;
            this.radGridViewTiposComp.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewTiposComp.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewTiposComp.AllowEditRow = false;
            this.radGridViewTiposComp.EnableFiltering = true;

            //Poner el focus en la 1ra fila
            if (this.radGridViewTiposComp.Rows != null && this.radGridViewTiposComp.Rows.Count > 0)
            {
                this.radGridViewTiposComp.Rows[0].IsCurrent = true;
                this.radGridViewTiposComp.Rows[0].IsSelected = true;

                GridTableElement tableElementDel = (GridTableElement)this.radGridViewTiposComp.CurrentView;

                if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                this.radGridViewTiposComp.Focus();
            }
        }

        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                /*
                //Eliminar las filas del DataTable
                if (e.Operacion == OperacionMtoTipo.Eliminar)
                {
                    DataRow[] dtr = this.dtTiposComp.Select("TIVOTV=81");
                    foreach (var drow in dtr)
                    {
                        drow.Delete();
                    }
                    dtTiposComp.AcceptChanges();

                    //this.radGridViewTiposComp.MasterTemplate.Refresh(null);

                    this.radGridViewTiposComp.DataSource = null;
                    this.radGridViewTiposComp.DataSource = dtTiposComp;

                    /*
                    this.radGridViewTiposComp.RefreshEdit();

                    this.radGridViewTiposComp.Rows.Clear();
                    this.radGridViewTiposComp.BindingContext = new BindingContext();
                    this.radGridViewTiposComp.DataSource = dtTiposComp;
                    
                    */
                    //this.radGridViewTiposComp.Refresh();
                    //return;
                //

                //this.dtTiposComp.Clear();

                this.FillDataGrid();

                if (e != null && this.radGridViewTiposComp != null && this.radGridViewTiposComp.Rows != null && this.radGridViewTiposComp.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewTiposComp.Rows.Count; i++)
                            {
                                if (this.radGridViewTiposComp.Rows[i].Cells["TIVOTV"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewTiposComp.Rows[i].IsCurrent = true;
                                    this.radGridViewTiposComp.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewTiposComp.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewTiposComp.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewTiposComp.Rows[0].IsCurrent = true;
                            this.radGridViewTiposComp.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewTiposComp.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewTiposComp.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void TgTiposComp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarTipoComp();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoTipoComp();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarTipoComp();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarTipoComp();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadGridViewTiposComp_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarTipoComp();
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

        private void RadButtonExport_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExport);
        }

        private void RadButtonExport_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExport);
        }

        private void FrmMtoGLT06Sel_Shown(object sender, EventArgs e)
        {
            this.radGridViewTiposComp.Focus();
        }

        private void RadGridViewTiposComp_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewTiposComp, ref this.selectAll);
        }

        private void FrmMtoGLT06Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Tipos de Comprobantes Gestión");
        }
        private void radGridViewTiposComp_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = "   " + this.LP.GetText("lblfrmMtoGLT06Sel", "Mantenimiento de Tipos de Comprobantes");   //Falta traducir

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
        }

        /// <summary>
        /// Devuelve el literal para el campo posición del modo de trabajo
        /// </summary>
        /// <returns></returns>
        private string ValorCampoCODITVIdioma(string valorCampoCODITV)
        {
            string result = "";
            switch (valorCampoCODITV)
            {
                case "0":
                    result = this.LP.GetText("lblGLT06Interactivo", "Interactivo");     //Falta traducir
                    break;
                case "1":
                default:
                    result = this.LP.GetText("lblGLT06Batch", "Batch");     //Falta traducir
                    break;
            }
            return (result);
        }

        /// <summary>
        /// Devuelve el literal para el campo validar documento
        /// </summary>
        /// <returns></returns>
        private string ValorCampoDEFDTVIdioma(string valorCampoDEFDTV)
        {
            string result = "";
            switch (valorCampoDEFDTV)
            {
                case "1":
                    result = this.LP.GetText("lblGLT06ValDocSi", "Sí");     //Falta traducir
                    break;
                case "2":
                    result = this.LP.GetText("lblGLT06ValDocSoloCancel", "Sólo si cancelación");     //Falta traducir
                    break;
                case "0":
                default:
                    result = this.LP.GetText("lblGLT06ValDocNo", "No");     //Falta traducir
                    break;
            }
            return (result);
        }

        /*
        /// <summary>
        /// Traduce los literales de las filas del DataGrid
        /// </summary>
        private void TraducirLiteralesFilasDataGrid()
        {
            string valorCampoEstado = "";
            string valorCampoCODITV = "";
            string valorCampoDEFDTV = "";
            for (int i = 0; i < this.tgTiposComp.Rows.Count; i++)
            {
                valorCampoEstado = this.tgTiposComp.Rows[i].Cells["STATTV"].Value.ToString();

                if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                else valorCampoEstado = this.estadoActivo;

                this.tgTiposComp.Rows[i].Cells["STATTV"].Value = valorCampoEstado;

                valorCampoCODITV = this.tgTiposComp.Rows[i].Cells["CODITV"].Value.ToString();
                valorCampoCODITV = this.ValorCampoCODITVIdioma(valorCampoCODITV);
                this.tgTiposComp.Rows[i].Cells["CODITV"].Value = valorCampoCODITV;

                valorCampoDEFDTV = this.tgTiposComp.Rows[i].Cells["DEFDTV"].Value.ToString();
                valorCampoDEFDTV = this.ValorCampoDEFDTVIdioma(valorCampoDEFDTV);
                this.tgTiposComp.Rows[i].Cells["DEFDTV"].Value = valorCampoDEFDTV;
            }
        }
        */

        /*
        /// <summary>
        /// Construir el control para hacer búquedas de TiposComp
        /// </summary>
        private void BuildtgBuscadorTiposComp()
        {
            this.tgBuscadorTiposComp.ProveedorDatosForm = GlobalVar.ConexionCG;
            this.tgBuscadorTiposComp.Datos = null;
            this.tgBuscadorTiposComp.TituloGrupo = " Buscador Tipos de Comprobantes ";  //Falta traducir
            this.tgBuscadorTiposComp.BuscarFormResult += new TGBuscador.BuscarFormResultCommandEventHandler(tgBuscadorTiposComp_BuscarFormResult);
            this.tgBuscadorTiposComp.FrmPadre = this;
        }
        */

        /// <summary>
        /// Construir el control de la Grid que contiene las TiposComp
        /// </summary>
        private void BuildtgGridTiposComp()
        {
            //Crear el DataGrid
            this.dtTiposComp.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            this.dtTiposComp.Columns.Add("STATTV", typeof(string));
            this.dtTiposComp.Columns.Add("TIVOTV", typeof(string));
            this.dtTiposComp.Columns.Add("NOMBTV", typeof(string));
            this.dtTiposComp.Columns.Add("CODITV", typeof(string));
            this.dtTiposComp.Columns.Add("DEFDTV", typeof(string));

            this.radGridViewTiposComp.DataSource = this.dtTiposComp;
        }

        /// <summary>
        /// Verificar autorizaciones para habilitar/deshabilitar botones
        /// </summary>
        private void VerificarAutorizaciones()
        {
            try
            {
                //Botón Nuevo
                if (!aut.CrearElemento(this.autClaseElemento)) utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                else utiles.ButtonEnabled(ref this.radButtonNuevo, true);

                bool operarModificar = aut.Operar(this.autClaseElemento, this.autGrupo, this.autOperModifica);

                //Botón Editar y Botón Activar/Desactivar
                if (operarModificar)
                {
                    //this.toolStripButtonEditar.Enabled = true;
                    //this.toolStripButtonActivarDesactivar.Enabled = true;
                    this.radButtonEditar.Text = "Editar";     //Falta traducir
                }
                else
                {
                    //this.toolStripButtonEditar.Enabled = false;
                    //this.toolStripButtonActivarDesactivar.Enabled = false;
                    this.radButtonEditar.Text = "Consultar";           //Falta traducir
                }
                utiles.ButtonEnabled(ref this.radButtonEditar, true);
                utiles.ButtonEnabled(ref this.radButtonExport, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Llamada a Editar un tipo de comprobante
        /// </summary>
        private void EditarTipoComp()
        {
            if (this.radGridViewTiposComp.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewTiposComp.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewTiposComp.CurrentRow.IsExpanded) this.radGridViewTiposComp.CurrentRow.IsExpanded = false;
                else this.radGridViewTiposComp.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewTiposComp.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewTiposComp.CurrentRow.Index;
                int indice = this.radGridViewTiposComp.Rows.IndexOf(this.radGridViewTiposComp.CurrentRow);
                this.codTipoComp = this.radGridViewTiposComp.Rows[indice].Cells["TIVOTV"].Value.ToString();

                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codTipoComp, this.autOperModifica);

                if (operarModificar)
                {
                    string nombre = this.radGridViewTiposComp.Rows[indice].Cells["NOMBTV"].Value.ToString();

                    string modoTrabajoAux = this.radGridViewTiposComp.Rows[indice].Cells["CODITV"].Value.ToString();

                    string modoTrabajoAux0 = this.LP.GetText("lblGLT06Interactivo", "Interactivo"); //0
                    string modoTrabajoAux1 = this.LP.GetText("lblGLT06Batch", "Batch");     //1

                    string posModoTrabajo = "1";
                    if (modoTrabajoAux == modoTrabajoAux0) posModoTrabajo = "0";

                    string validarDocAux = this.radGridViewTiposComp.Rows[indice].Cells["DEFDTV"].Value.ToString();

                    string validarDocAux0 = this.LP.GetText("lblGLT06ValDocNo", "No");
                    string validarDocAux1 = this.LP.GetText("lblGLT06ValDocSi", "Sí");
                    string validarDocAux2 = this.LP.GetText("lblGLT06ValDocSoloCancel", "Sólo si cancelación");

                    string posValidarDoc = "0";
                    if (validarDocAux == validarDocAux1) posValidarDoc = "1";
                    else if (validarDocAux == validarDocAux2) posValidarDoc = "2";

                    string estado = this.radGridViewTiposComp.Rows[indice].Cells["STATTV"].Value.ToString();

                    if (estado == this.estadoInactivo) estado = "*";
                    else estado = "V";

                    frmMtoGLT06 frmMtoTipoComp = new frmMtoGLT06
                    {
                        Nuevo = false,
                        Codigo = this.codTipoComp,
                        Nombre = nombre,
                        Estado = estado,
                        PosModoTrabajo = posModoTrabajo,
                        PosValidarDoc = posValidarDoc,
                        FrmPadre = this
                    };
                    frmMtoTipoComp.Show(this);
                    frmMtoTipoComp.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                }
            }
          
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a crear nuevo tipo de extracontable
        /// </summary>
        private void NuevoTipoComp()
        {
            Cursor.Current = Cursors.WaitCursor;

            string error = this.LP.GetText("errValTitulo", "Error");

            frmMtoGLT06 frmMtoTipoComp = new frmMtoGLT06
            {
                Nuevo = true,
                FrmPadre = this
            };
            frmMtoTipoComp.Show(this);
            frmMtoTipoComp.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }
        
        /// <summary>
        /// Carga los datos de las TiposComp en la grid
        /// </summary>
        private void FillDataGrid()
        {
            this.radGridViewTiposComp.Visible = false;

            try
            {
                string query = "select STATTV, TIVOTV, NOMBTV, CODITV, DEFDTV from ";
                query += GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "order by TIVOTV";

                this.dtTiposComp.Clear();
                //this.radGridViewTiposComp.Rows.Clear();
                //this.radGridViewTiposComp.DataSource = null;

                DataTable dtTipComp = new DataTable();
                dtTipComp = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                string valorCampoEstado = "";
                string valorCampoCODITV = "";
                string valorCampoDEFDTV = "";
                int cont = 0;

                foreach (DataRow dr in dtTipComp.Rows)
                {
                    valorCampoEstado = dr["STATTV"].ToString();
                    valorCampoCODITV = dr["CODITV"].ToString();
                    valorCampoDEFDTV = dr["DEFDTV"].ToString();

                    if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                    else valorCampoEstado = this.estadoActivo;

                    valorCampoCODITV = this.ValorCampoCODITVIdioma(valorCampoCODITV);

                    valorCampoDEFDTV = this.ValorCampoDEFDTVIdioma(valorCampoDEFDTV);

                    this.dtTiposComp.Rows.Add(dr.ItemArray);
                    this.dtTiposComp.Rows[cont]["STATTV"] = valorCampoEstado;
                    this.dtTiposComp.Rows[cont]["CODITV"] = valorCampoCODITV;
                    this.dtTiposComp.Rows[cont]["DEFDTV"] = valorCampoDEFDTV;

                    cont++;
                }

                this.radGridViewTiposComp.DataSource = this.dtTiposComp;
                this.RadGridViewHeader();

                if (this.radGridViewTiposComp.Rows != null && this.radGridViewTiposComp.Rows.Count > 0)
                {
                    //this.tgBuscadorTiposComp.Enabled = true;
                    //this.tgTiposComp.Visible = true;

                    for (int i = 0; i < this.radGridViewTiposComp.Columns.Count; i++)
                    {
                        this.radGridViewTiposComp.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewTiposComp.Columns[i].Width = 600;
                    }

                    this.radGridViewTiposComp.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewTiposComp.AllowSearchRow = true;
                    this.radGridViewTiposComp.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewTiposComp.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewTiposComp.AllowEditRow = false;
                    this.radGridViewTiposComp.EnableFiltering = true;

                    this.radGridViewTiposComp.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewTiposComp.Rows[0].IsCurrent = true;
                    this.radGridViewTiposComp.Focus();
                    this.radGridViewTiposComp.Select();

                    this.radGridViewTiposComp.Refresh();

                    this.radGridViewTiposComp.Visible = true;

                    //Habilitar/Deshabilitar botones según autorizaciones
                    this.VerificarAutorizaciones();
                }
                else
                {
                }
                
                /*
                if (this.tgTiposComp.dsDatos != null && this.tgTiposComp.dsDatos.Tables["Tabla"] != null &&
                    this.tgTiposComp.dsDatos.Tables["Tabla"].Rows.Count > 0)
                {
                    this.tgBuscadorTiposComp.Enabled = true;
                    this.tgTiposComp.Visible = true;

                    //Buscador
                    this.tgBuscadorTiposComp.Query = query;
                    if (this.tgBuscadorTiposComp.Datos != null) this.tgBuscadorTiposComp.Datos.Clear();
                    this.tgBuscadorTiposComp.Datos = null;

                    string nombreColumnas = "";
                    ArrayList camposGridYDesc = new ArrayList();

                    string[] campos1GridYDesc = new string[2];
                    campos1GridYDesc[0] = this.LP.GetText("lblListaCampoEstado", "Estado");
                    nombreColumnas += campos1GridYDesc[0];
                    campos1GridYDesc[1] = "STATTV";
                    camposGridYDesc.Add(campos1GridYDesc);

                    string[] campos2GridYDesc = new string[2];
                    campos2GridYDesc[0] = this.LP.GetText("lblListaCampoTipo", "Tipo");
                    nombreColumnas += ", " + campos2GridYDesc[0];
                    campos2GridYDesc[1] = "TIVOTV";
                    camposGridYDesc.Add(campos2GridYDesc);

                    string[] campos3GridYDesc = new string[2];
                    campos3GridYDesc[0] = this.LP.GetText("lblListaCampoNombre", "Nombre");
                    nombreColumnas += ", " + campos3GridYDesc[0];
                    campos3GridYDesc[1] = "NOMBTV";
                    camposGridYDesc.Add(campos3GridYDesc);

                    string[] campos4GridYDesc = new string[2];
                    campos4GridYDesc[0] = this.LP.GetText("lblListaCampoModoTrabajo", "Modo de trabajo");
                    nombreColumnas += ", " + campos4GridYDesc[0];
                    campos4GridYDesc[1] = "CODITV";
                    camposGridYDesc.Add(campos4GridYDesc);

                    string[] campos5GridYDesc = new string[2];
                    campos5GridYDesc[0] = this.LP.GetText("lblListaCampoValDoc", "Validar documento");
                    nombreColumnas += ", " + campos5GridYDesc[0];
                    campos5GridYDesc[1] = "DEFDTV";
                    camposGridYDesc.Add(campos5GridYDesc);

                    this.tgBuscadorTiposComp.NombreColumnas = nombreColumnas;
                    this.tgBuscadorTiposComp.NombreColumnasCampos = camposGridYDesc;

                    string todasEtiqueta = this.LP.GetText("lblEtiquetaTodasColumnas", "Todas");
                    this.tgBuscadorTiposComp.NombreColumnasSel = todasEtiqueta;
                    this.tgBuscadorTiposComp.TodasEtiqueta = todasEtiqueta;

                    if (this.tgTiposComp.dsDatos != null && this.tgTiposComp.dsDatos.Tables["Tabla"] != null &&
                        this.tgTiposComp.dsDatos.Tables["Tabla"].Rows.Count > 0)
                    {
                        //Habilitar/Deshabilitar botones según autorizaciones
                        this.VerificarAutorizaciones();
                    }
                }
                else
                {
                }
                */
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina el comprobante
        /// </summary>
        /// <returns></returns>
        private bool EliminarComp()
        {
            bool result = true;

            try
            {
                string query = "select count(TICOIC) from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "where TICOIC = '" + this.codTipoComp + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrTipoCompEnUso", "El tipo de comprobante está en uso"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    result = false;
                    return (result);
                }

                query = "select count(TICOP3) from " + GlobalVar.PrefijoTablaCG + "PRB01 ";
                query += "where TICOP3 = '" + this.codTipoComp + "'";

                cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrTipoCompEnUso", "El tipo de comprobante está en uso"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    result = false;
                    return (result);
                }

                query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT06 where ";
                query += " TIVOTV = '" + this.codTipoComp + "'";

                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (cantRegistros != 1)
                {
                    string mensaje = "No fue posible eliminar el Tipo de comprobante.";
                    RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                }
                else
                {
                    //Eliminarlo de las tablas de autorizaciones
                    try
                    {
                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT06", this.codTipoComp, null);

                        query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                        query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + this.codTipoComp + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (cantRegistros > 0) utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM07", autClaseElemento, this.codTipoComp);

                        query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "where CLELAG = '" + autClaseElemento + "' and ELEMAG = '" + this.codTipoComp + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(ex.Message + " " + this.GetType() + "(radButtonEliminar_Click)"); }

                    //Cerrar el formulario
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string mensaje = this.LP.GetText("lblErrExcEliminarComp", "Error eliminando el tipo de comprobante ");  //Falta traducir 
                RadMessageBox.Show(mensaje + "( " + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                result = false;
            }

            return (result);
        }

        /// <summary>
        /// Llamada a Eliminar un tipo de comprobante
        /// </summary>
        private void EliminarTipoComp()
        {
            if (this.radGridViewTiposComp.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.radGridViewTiposComp.CurrentRow.Index;
            int indice = this.radGridViewTiposComp.Rows.IndexOf(this.radGridViewTiposComp.CurrentRow);
            this.codTipoComp = this.radGridViewTiposComp.Rows[indice].Cells["TIVOTV"].Value.ToString();

            //Pedir confirmación y eliminar el tipo de comprobante seleccionado
            //this.LP.GetText("wrnDeleteConfirm"       
            string mensaje = "Se va a eliminar el tipo de comprobante " + this.codTipoComp;  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                bool resultElimComp = this.EliminarComp();

                if (resultElimComp)
                {
                    //Eliminar la entrada del DataSet
                    //this.radGridViewTiposComp.Rows.RemoveAt(indice);

                    /*
                    if (this.tgTiposComp.dsDatos.Tables["Tabla"].Rows.Count == 0)
                    {
                        this.tgTiposComp.Visible = false;
                        this.lblblNoExisteCompAsoc.Visible = true;
                    }
                    */
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewTiposComp, "Mantenimiento de Tipos de Comprobantes");
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewTiposComp.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewTiposComp.Columns.Contains(item.Key)) this.radGridViewTiposComp.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                campo = "STATTV";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);

                campo = "TIVOTV";
                header = this.LP.GetText("lblListaCampoTipo", "Tipo");
                this.displayNames.Add(campo, header);

                campo = "NOMBTV";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "CODITV";
                header = this.LP.GetText("lblListaCampoModoTrabajo", "Modo de trabajo");
                this.displayNames.Add(campo, header);

                campo = "DEFDTV";
                header = this.LP.GetText("lblListaCampoValDoc", "Validar documento");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        #endregion

    }
}
