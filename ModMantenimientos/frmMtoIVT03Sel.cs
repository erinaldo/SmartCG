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
    public partial class frmMtoIVT03Sel: frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "";
        private string autGrupo = "";
        private string autOperModifica = "";
        //private string autOperAlta = "";

        private string codigo = "";
    
        private Dictionary<string, string> displayNames;
        private DataTable dtCompFiscales = new DataTable();

        public frmMtoIVT03Sel()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Traducir los literales de las filas del DataGrid
            this.TraducirLiteralesFilasDataGrid();
        }

        private void FrmMtoIVT03Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Compañías Fiscales Gestión");

            this.radGridViewCompFiscales.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewCompFiscales.AllowSearchRow = true;
            this.radGridViewCompFiscales.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewCompFiscales.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewCompFiscales.AllowEditRow = false;
            this.radGridViewCompFiscales.EnableFiltering = true;

            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Crear el TGGrid
            this.BuildtgGridCompFiscales();

            //Cargar los datos de la Grid
            this.FillDataGrid();
        }

        /*
        private void tgBuscadorCompFiscales_BuscarFormResult(TGBuscador.BuscarFormResultCommandEventArgs e)
        {
            if (this.tgBuscadorCompFiscales.Datos != null)
            {
                try
                {
                    if (this.tgCompFiscales.dsDatos.Tables["Tabla"] != null) this.tgCompFiscales.dsDatos.Tables["Tabla"].Clear();

                    string valorCampoCompFiscal = "";
                    string valorCampoNombre = "";
                    string valorCampoCompContable = "";
                    int cont = 0;
                    foreach (DataRow dr in this.tgBuscadorCompFiscales.Datos.Rows)
                    {
                        this.tgCompFiscales.dsDatos.Tables["Tabla"].Rows.Add(dr.ItemArray);

                        valorCampoCompFiscal = dr["CIAFT3"].ToString();
                        valorCampoNombre = dr["NOMBT3"].ToString();
                        valorCampoCompContable = dr["CCIAT2"].ToString();

                        this.tgCompFiscales.dsDatos.Tables["Tabla"].Rows[cont]["CIAFT3"] = valorCampoCompFiscal;
                        this.tgCompFiscales.dsDatos.Tables["Tabla"].Rows[cont]["NOMBT3"] = valorCampoNombre;
                        this.tgCompFiscales.dsDatos.Tables["Tabla"].Rows[cont]["CCIAT2"] = valorCampoCompContable;

                        cont++;
                    }

                    if (this.tgBuscadorCompFiscales.Datos.Rows.Count == 0) utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    else utiles.ButtonEnabled(ref this.radButtonEditar, true);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
        }
        */

        private void TgGridCompFiscales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarCompFiscal();
        }

        private void TgBuscadorCompFiscal_BuscarFormResult(TGBuscador.BuscarFormResultCommandEventArgs e)
        {
            /*
            if (this.tgBuscadorCompFiscales.Datos != null)
            {
                try
                {
                    if (this.tgCompFiscales.dsDatos.Tables["Tabla"] != null) this.tgCompFiscales.dsDatos.Tables["Tabla"].Clear();

                    foreach (DataRow dr in this.tgBuscadorCompFiscales.Datos.Rows)
                    {
                        this.tgCompFiscales.dsDatos.Tables["Tabla"].Rows.Add(dr.ItemArray);
                    }

                    if (this.tgBuscadorCompFiscales.Datos.Rows.Count == 0) utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    else utiles.ButtonEnabled(ref this.radButtonEditar, true);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            */
        }
        
        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta o una actualización de un elementos
        //void IFormListaGenerica.ActualizaListaElementos()
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Eliminar las filas del DataTable
                this.dtCompFiscales.Clear();

                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid();

                if (e != null && this.radGridViewCompFiscales!= null && this.radGridViewCompFiscales.Rows != null && this.radGridViewCompFiscales.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewCompFiscales.Rows.Count; i++)
                            {
                                if (this.radGridViewCompFiscales.Rows[i].Cells["CIAFT3"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewCompFiscales.Rows[i].IsCurrent = true;
                                    this.radGridViewCompFiscales.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewCompFiscales.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewCompFiscales.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewCompFiscales.Rows[0].IsCurrent = true;
                            this.radGridViewCompFiscales.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewCompFiscales.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewCompFiscales.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void TgCompFiscales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarCompFiscal();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevaCompFiscal();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarCompFiscal();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarCompFiscalCall();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadDataFilterGridInfo_Edited(object sender, Telerik.WinControls.UI.TreeNodeEditedEventArgs e)
        {
            this.radDataFilterGridInfo.ApplyFilter();
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, Telerik.WinControls.UI.TreeNodeEditorInitializedEventArgs e)
        {
            DataFilterCriteriaElement criteriaElement = e.NodeElement as DataFilterCriteriaElement;

            if (e.Editor is TreeViewDropDownListEditor editor && criteriaElement != null)
            {
                if (criteriaElement.EditingElement is DataFilterFieldEditorElement)
                {
                    var element = editor.EditorElement as BaseDropDownListEditorElement;
                    element.DataSource = displayNames;
                    element.ValueMember = "Key";
                    element.DisplayMember = "Value";
                }
            }
        }

        private void RadDataFilterGridInfo_NodeFormatting(object sender, Telerik.WinControls.UI.TreeNodeFormattingEventArgs e)
        {
            try
            {
                if (e.NodeElement is DataFilterCriteriaElement dataExpressionFilterElement)
                {
                    var node = dataExpressionFilterElement.Data as DataFilterCriteriaNode;
                    if (displayNames.ContainsKey(node.PropertyName))
                    {
                        dataExpressionFilterElement.FieldElement.Text = displayNames[node.PropertyName];
                    }
                }
            }
            catch { }
        }

        private void RadGridViewCompFiscales_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarCompFiscal();
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

        private void FrmMtoIVT03Sel_Shown(object sender, EventArgs e)
        {
            this.radGridViewCompFiscales.Focus();
        }

        private void RadGridViewCompFiscales_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewCompFiscales, ref this.selectAll);
        }

        private void FrmMtoIVT03Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Compañías Fiscales Gestión");
        }

        private void radGridViewCompFiscales_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        private void radGridViewCompFiscales_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (this.radGridViewCompFiscales.Rows.IndexOf(this.radGridViewCompFiscales.CurrentRow) >= 0)
                {
                    this.EditarCompFiscal();
                }
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            this.Text = this.LP.GetText("lblfrmMtoIVT03Sel", "Mantenimiento de Compañías Fiscales");   //Falta traducir

            //Traducir los Literales de los ToolStrip
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

        /// <summary>
        /// Traduce los literales de las filas del DataGrid
        /// </summary>
        private void TraducirLiteralesFilasDataGrid()
        {
            /*
            string valorCampoEstado = "";
            string valorCampoCODITV = "";
            string valorCampoDEFDTV = "";
            for (int i = 0; i < this.tgCompFiscales.Rows.Count; i++)
            {
                valorCampoEstado = this.tgCompFiscales.Rows[i].Cells["STATTV"].Value.ToString();

                if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                else valorCampoEstado = this.estadoActivo;

                this.tgCompFiscales.Rows[i].Cells["STATTV"].Value = valorCampoEstado;

                valorCampoCODITV = this.tgCompFiscales.Rows[i].Cells["CODITV"].Value.ToString();
                valorCampoCODITV = this.ValorCampoCODITVIdioma(valorCampoCODITV);
                this.tgCompFiscales.Rows[i].Cells["CODITV"].Value = valorCampoCODITV;

                valorCampoDEFDTV = this.tgCompFiscales.Rows[i].Cells["DEFDTV"].Value.ToString();
                valorCampoDEFDTV = this.ValorCampoDEFDTVIdioma(valorCampoDEFDTV);
                this.tgCompFiscales.Rows[i].Cells["DEFDTV"].Value = valorCampoDEFDTV;
            }
            */
        }

        /// <summary>
        /// Construir el control de la Grid que contiene las Compañías Fiscales
        /// </summary>
        private void BuildtgGridCompFiscales()
        {
            //Crear el DataGrid
            this.dtCompFiscales.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            this.dtCompFiscales.Columns.Add("CIAFT3", typeof(string));
            this.dtCompFiscales.Columns.Add("NOMBT3", typeof(string));
            this.dtCompFiscales.Columns.Add("CCIAT2", typeof(string));
            this.dtCompFiscales.Columns.Add("NIFDT3", typeof(string));
            this.dtCompFiscales.Columns.Add("ULMCT3", typeof(string));

            this.radGridViewCompFiscales.DataSource = this.dtCompFiscales;
        }

        /// <summary>
        /// Verificar autorizaciones para habilitar/deshabilitar botones
        /// </summary>
        private void VerificarAutorizaciones()
        {
            try
            {
                //bool operarCrear = aut.Validar(this.autClaseElemento, this.autGrupo, this.codClaseZona, this.autOperAlta);
                //bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codClaseZona, this.autOperModifica);

                bool operarCrear = true;
                bool operarModificar = true;

                //Botón Nuevo
                if (operarCrear) utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                else utiles.ButtonEnabled(ref this.radButtonNuevo, false);

                //Botón Editar y Botón Activar/Desactivar
                if (operarModificar) this.radButtonEditar.Text = "Editar";     //Falta traducir
                else this.radButtonEditar.Text = "Consultar";           //Falta traducir

                utiles.ButtonEnabled(ref this.radButtonEditar, true);
                utiles.ButtonEnabled(ref this.radButtonExport, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Llamada a Editar una compañía fiscal
        /// </summary>
        private void EditarCompFiscal()
        {
            if (this.radGridViewCompFiscales.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewCompFiscales.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewCompFiscales.CurrentRow.IsExpanded) this.radGridViewCompFiscales.CurrentRow.IsExpanded = false;
                else this.radGridViewCompFiscales.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewCompFiscales.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewCompFiscales.CurrentRow.Index;
                int indice = this.radGridViewCompFiscales.Rows.IndexOf(this.radGridViewCompFiscales.CurrentRow);
                this.codigo = this.radGridViewCompFiscales.Rows[indice].Cells["CIAFT3"].Value.ToString();
                //string nombre = this.tgCompFiscales.Rows[indice].Cells["NOMBTV"].Value.ToString();

                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codigo, this.autOperModifica);

                //if (operarModificar)
                //{
                frmMtoIVT03 frmMtoCompFiscal = new frmMtoIVT03
                {
                    Nuevo = false,
                    Codigo = this.codigo,
                    //frmMtoTipoComp.Nombre = nombre;
                    //frmMtoTipoComp.Estado = estado;
                    //frmMtoTipoComp.PosModoTrabajo = posModoTrabajo;
                    //frmMtoTipoComp.PosValidarDoc = posValidarDoc;
                    FrmPadre = this
                };
                frmMtoCompFiscal.Show(this);
                frmMtoCompFiscal.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
                //}
                //else
                //{
                //    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                //}
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a crear una nueva compañía fiscal
        /// </summary>
        private void NuevaCompFiscal()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Comprobar sobre esa nueva zona si tiene autorización a dar de alta

            string error = this.LP.GetText("errValTitulo", "Error");

            frmMtoIVT03 frmMtoCompFiscal = new frmMtoIVT03
            {
                Nuevo = true,
                FrmPadre = this
            };
            frmMtoCompFiscal.Show(this);
            frmMtoCompFiscal.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Eliminar una compañía fiscal
        /// </summary>
        private void EliminarCompFiscalCall()
        {
            if (this.radGridViewCompFiscales.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.radGridViewCompFiscales.CurrentRow.Index;
            int indice = this.radGridViewCompFiscales.Rows.IndexOf(this.radGridViewCompFiscales.CurrentRow);
            this.codigo = this.radGridViewCompFiscales.Rows[indice].Cells["CIAFT3"].Value.ToString();

            //Pedir confirmación y eliminar la compañía fiscal
            //this.LP.GetText("wrnDeleteConfirm"       
            string mensaje = "Se va a eliminar la compañía fiscal " + this.codigo;  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                bool resultElimComp = this.EliminarCompFiscal();

                if (resultElimComp)
                {
                    //Eliminar la entrada del DataSet
                    this.radGridViewCompFiscales.Rows.RemoveAt(indice);

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
            this.ExportarGrid(ref this.radGridViewCompFiscales, "Mantenimiento de Compañías Fiscales");
        }

        /// <summary>
        /// Carga los datos de las compañías fiscales en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                string query = "";
                if (this.tipoBaseDatosCG == "DB2")
                {
                    query = "select CIAFT3, NOMBT3, CCIAT2, NIFDT3, case ULMCT3 when 0 then ' ' else SUBSTR((ULMCT3+190000), 1, 4) || '-' || SUBSTR((ULMCT3+190000), 5, 2) end ULMCT3 from ";
                    query += GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "LEFT OUTER JOIN " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "ON CIAFT2 = CIAFT3 ";
                    query += "order by CIAFT3, CCIAT2";
                }
                if (this.tipoBaseDatosCG == "SQLServer")
                {
                    query = "select CIAFT3, NOMBT3, CCIAT2, NIFDT3, case ULMCT3 when 0 then ' ' else SUBSTRING(CONVERT(varchar, ULMCT3+190000), 1, 4) + '-' +  SUBSTRING(CONVERT(varchar, ULMCT3+190000), 5, 2) end ULMCT3 from ";
                    query += GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "LEFT OUTER JOIN " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "ON CIAFT2 = CIAFT3 ";
                    query += "order by CIAFT3, CCIAT2";
                }
                if (this.tipoBaseDatosCG == "Oracle")
                {
                    query = "select CIAFT3, NOMBT3, CCIAT2, NIFDT3, case ULMCT3 when 0 then ' ' else SUBSTR((ULMCT3+190000), 1, 4) || '-' || SUBSTR((ULMCT3+190000), 5, 2) end ULMCT3 from ";
                    query += GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "LEFT OUTER JOIN " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "ON CIAFT2 = CIAFT3 ";
                    query += "order by CIAFT3, CCIAT2";
                }

                //string query = "select CIAFT3, NOMBT3, CCIAT2, NIFDT3, ULMCT3 from ";
                //query += GlobalVar.PrefijoTablaCG + "IVT03 ";
                //query += "LEFT OUTER JOIN " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                //query += "ON CIAFT2 = CIAFT3 ";
                //query += "order by CIAFT3, CCIAT2";

                DataTable dtCompFisc = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                this.dtCompFiscales.Clear();
                this.radGridViewCompFiscales.DataSource = null;

                string compFiscal = "";
                string compFiscalActual = "";
                string compFiscalNombre = "";
                string compFiscalNombreActual = "";
                string listaCompaniaContables = "";
                string strNIF = "";
                string strNIFActual = "";
                string strMesCerrado = "";
                string strMesCerradoActual = "";
                int maxLenCompaniaContables = 0;

                DataRow currentRow;

                for (int i = 0; i < dtCompFisc.Rows.Count; i++)
                {
                    compFiscalActual = dtCompFisc.Rows[i]["CIAFT3"].ToString();
                    compFiscalNombreActual = dtCompFisc.Rows[i]["NOMBT3"].ToString();
                    strNIFActual = dtCompFisc.Rows[i]["NIFDT3"].ToString();
                    strMesCerradoActual = dtCompFisc.Rows[i]["ULMCT3"].ToString();

                    if (compFiscal == compFiscalActual || i == 0)
                    {
                        if (listaCompaniaContables != "") listaCompaniaContables += "  ";
                        listaCompaniaContables += dtCompFisc.Rows[i]["CCIAT2"].ToString();
                        compFiscal = compFiscalActual;
                        compFiscalNombre = compFiscalNombreActual;
                        strNIF = strNIFActual;
                        strMesCerrado = strMesCerradoActual;
                    }
                    else
                    {
                        currentRow = this.dtCompFiscales.NewRow();
                        currentRow["CIAFT3"] = compFiscal;
                        currentRow["NOMBT3"] = compFiscalNombre;
                        currentRow["CCIAT2"] = listaCompaniaContables;
                        currentRow["NIFDT3"] = strNIF;
                        currentRow["ULMCT3"] = strMesCerrado;

                        if (listaCompaniaContables.Length > maxLenCompaniaContables) maxLenCompaniaContables = listaCompaniaContables.Length;                      

                        this.dtCompFiscales.Rows.Add(currentRow);

                        listaCompaniaContables = dtCompFisc.Rows[i]["CCIAT2"].ToString();
                        compFiscal = compFiscalActual;
                        compFiscalNombre = compFiscalNombreActual;
                        strNIF = strNIFActual;
                        strMesCerrado = strMesCerradoActual;
                    }

                    //Escribir el ultimo registro
                    if ((i + 1) == dtCompFisc.Rows.Count)
                    {
                        currentRow = this.dtCompFiscales.NewRow();
                        currentRow["CIAFT3"] = compFiscal;
                        currentRow["NOMBT3"] = compFiscalNombre;
                        currentRow["CCIAT2"] = listaCompaniaContables;
                        currentRow["NIFDT3"] = strNIF;
                        currentRow["ULMCT3"] = strMesCerrado;

                        this.dtCompFiscales.Rows.Add(currentRow);

                        if (listaCompaniaContables.Length > maxLenCompaniaContables) maxLenCompaniaContables = listaCompaniaContables.Length;
                    }
                }

                /*
                if (maxLenCompaniaContables > 0)
                {
                    //Ajustamos la celda a su contenido.
                    DataGridViewColumn col = this.tgCompFiscales.Columns["CCIAT2"];
                    if ((maxLenCompaniaContables * 8) > col.Width) col.Width = maxLenCompaniaContables * 8;
                    col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                */

                this.radGridViewCompFiscales.Visible = false;
                this.radGridViewCompFiscales.DataSource = this.dtCompFiscales;
                this.radDataFilterGridInfo.DataSource = this.radGridViewCompFiscales.DataSource;
                this.RadGridViewHeader();

                if (this.radGridViewCompFiscales.Rows != null && this.radGridViewCompFiscales.Rows.Count > 0)
                {
                    this.radGridViewCompFiscales.Visible = true;

                    for (int i = 0; i < this.radGridViewCompFiscales.Columns.Count; i++)
                    {
                        this.radGridViewCompFiscales.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewCompFiscales.Columns[i].Width = 600;
                    }

                    this.radGridViewCompFiscales.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewCompFiscales.AllowSearchRow = true;
                    this.radGridViewCompFiscales.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewCompFiscales.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewCompFiscales.AllowEditRow = false;
                    this.radGridViewCompFiscales.EnableFiltering = true;

                    this.radGridViewCompFiscales.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewCompFiscales.Rows[0].IsCurrent = true;
                    this.radGridViewCompFiscales.Focus();
                    this.radGridViewCompFiscales.Select();

                    this.radGridViewCompFiscales.Refresh();

                    this.radGridViewCompFiscales.Visible = true;

                    //Habilitar/Deshabilitar botones según autorizaciones
                    this.VerificarAutorizaciones();
                }
                else
                {
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Eliminar la compañía fiscal
        /// </summary>
        /// <returns></returns>
        private bool EliminarCompFiscal()
        {
            bool result = true;

            try
            {
                //Verificar que la Compañía fiscal no tenga movimientos vigentes
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                query += "where CIAFB3 = '" + this.codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrCompFiscalMovVigentes", "La compañía fiscal tiene movimientos vigentes"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    return (false);
                }

                //Eliminar las asociaciones de las compañías contables con la compañía fiscal
                query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                query += "where CIAFT2 = '" + this.codigo + "'";

                cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue));

                //Eliminar la compañía fiscal
                query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                query += "where CIAFT3 = '" + this.codigo + "'";

                cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue));

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string mensaje = this.LP.GetText("lblErrExcEliminarCompFiscal", "Error eliminando la compañía fiscal ");  //Falta traducir 
                RadMessageBox.Show(mensaje + "( " + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                result = false;
            }

            return (result);
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCompFiscales.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCompFiscales.Columns.Contains(item.Key)) this.radGridViewCompFiscales.Columns[item.Key].HeaderText = item.Value;
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

                campo = "CIAFT3";
                header = this.LP.GetText("lblListaCampoCompFiscal", "Compañía Fiscal");
                this.displayNames.Add(campo, header);

                campo = "NOMBT3";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "CCIAT2";
                header = this.LP.GetText("lblListaCampoCompContable", "Compañía Contable");
                this.displayNames.Add(campo, header);

                campo = "NIFDT3";
                header = this.LP.GetText("lblListaCampoNIF", "Id.Fiscal");
                this.displayNames.Add(campo, header);

                campo = "ULMCT3";
                header = this.LP.GetText("lblListaCampoUltMes", "Ult. año/mes cerrado");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }

        #endregion

    }
}
