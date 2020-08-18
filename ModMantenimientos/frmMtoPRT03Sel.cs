
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
    public partial class frmMtoPRT03Sel: frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "005";
        private string autGrupo = "";
        private string autOperModifica = "";
        //private string autOperAlta = "";

        private string codTipoExt = "";

        private Dictionary<string, string> displayNames;
        private DataTable dtTipExt = new DataTable();

        public frmMtoPRT03Sel()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Traducir los literales de las filas del DataGrid
            //this.TraducirLiteralesFilasDataGrid();
        }

        private void FrmMtoPRT03Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Tipos de Extracontable Gestión");
            
            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            this.radGridViewTiposExt.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewTiposExt.AllowSearchRow = true;
            this.radGridViewTiposExt.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewTiposExt.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewTiposExt.AllowEditRow = false;
            this.radGridViewTiposExt.EnableFiltering = true;
        }

        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Eliminar las filas del DataTable
                this.dtTipExt.Clear();

                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid();

                if (e != null && this.radGridViewTiposExt != null && this.radGridViewTiposExt.Rows != null && this.radGridViewTiposExt.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewTiposExt.Rows.Count; i++)
                            {
                                if (this.radGridViewTiposExt.Rows[i].Cells["TDATAH"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewTiposExt.Rows[i].IsCurrent = true;
                                    this.radGridViewTiposExt.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewTiposExt.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewTiposExt.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewTiposExt.Rows[0].IsCurrent = true;
                            this.radGridViewTiposExt.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewTiposExt.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewTiposExt.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoTipoExt();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarTipoExt();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarTipoExt();     //EliminarTipoExtCall
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, Telerik.WinControls.UI.TreeNodeEditorInitializedEventArgs e)
        {
            Telerik.WinControls.UI.DataFilterCriteriaElement criteriaElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;

            if (e.Editor is Telerik.WinControls.UI.TreeViewDropDownListEditor editor && criteriaElement != null)
            {
                if (criteriaElement.EditingElement is Telerik.WinControls.UI.DataFilterFieldEditorElement)
                {
                    var element = editor.EditorElement as Telerik.WinControls.UI.BaseDropDownListEditorElement;
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
                if (e.NodeElement is Telerik.WinControls.UI.DataFilterCriteriaElement dataExpressionFilterElement)
                {
                    var node = dataExpressionFilterElement.Data as Telerik.WinControls.UI.DataFilterCriteriaNode;
                    if (displayNames.ContainsKey(node.PropertyName))
                    {
                        dataExpressionFilterElement.FieldElement.Text = displayNames[node.PropertyName];
                    }
                }
            }
            catch { }
        }

        private void RadGridViewTiposExt_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarTipoExt();
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

        private void FrmMtoPRT03Sel_Shown(object sender, EventArgs e)
        {
            this.radGridViewTiposExt.Focus();
        }

        private void RadGridViewTiposExt_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewTiposExt, ref this.selectAll);
        }

        private void FrmMtoPRT03Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Tipos de Extracontable Gestión");
        }

        private void radGridViewTiposExt_ViewCellFormatting(object sender, CellFormattingEventArgs e)
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
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoPRT03Sel", "Mantenimiento de Tipos de Extracontables");   //Falta traducir

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
        }

        /// <summary>
        /// Devuelve el literal para el campo posición del tipo de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ValorCampoAPNAAHIdioma(string valorCampoAPNAAH)
        {
            string result = "";
            switch (valorCampoAPNAAH)
            {
                case "1":
                    result = this.LP.GetText("lblPRT03Solo1", "Sólo 1ro");     //Falta traducir
                    break;
                case "2":
                    result = this.LP.GetText("lblPRT03Solo2", "Sólo 2do");     //Falta traducir
                    break;
                case "3":
                    result = this.LP.GetText("lblPRT03Solo3", "Sólo 3ro");     //Falta traducir
                    break;
                case "0":
                default:
                    result = this.LP.GetText("lblPRT03Todos", "Todos");     //Falta traducir
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
            string valorCampoAPNAAH = "";
            for (int i = 0; i < this.tgTiposExt.Rows.Count; i++)
            {
                valorCampoEstado = this.tgTiposExt.Rows[i].Cells["STATAH"].Value.ToString();

                if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                else valorCampoEstado = this.estadoActivo;

                this.tgTiposExt.Rows[i].Cells["STATAH"].Value = valorCampoEstado;

                valorCampoAPNAAH = this.tgTiposExt.Rows[i].Cells["APNAAH"].Value.ToString();
                valorCampoAPNAAH = this.ValorCampoAPNAAHIdioma(valorCampoAPNAAH);
                this.tgTiposExt.Rows[i].Cells["APNAAH"].Value = valorCampoAPNAAH;
            }
        }
        */

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
                if (operarModificar)
                {
                    //this.toolStripButtonEditar.Enabled = true;
                    //this.toolStripButtonActivarDesactivar.Enabled = true;
                    this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
                }
                else
                {
                    //this.toolStripButtonEditar.Enabled = false;
                    //this.toolStripButtonActivarDesactivar.Enabled = false;
                    this.radButtonEditar.Text = this.LP.GetText("toolStripConsultar", "Consultar");
                }
                utiles.ButtonEnabled(ref this.radButtonEditar, true);
                utiles.ButtonEnabled(ref this.radButtonExport, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Llamada a Editar un tipo de extracontable
        /// </summary>
        private void EditarTipoExt()
        {
            if (this.radGridViewTiposExt.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewTiposExt.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewTiposExt.CurrentRow.IsExpanded) this.radGridViewTiposExt.CurrentRow.IsExpanded = false;
                else this.radGridViewTiposExt.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewTiposExt.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewTiposExt.CurrentRow.Index;
                int indice = this.radGridViewTiposExt.Rows.IndexOf(this.radGridViewTiposExt.CurrentRow);
                this.codTipoExt = this.radGridViewTiposExt.Rows[indice].Cells["TDATAH"].Value.ToString();
                string nombre = this.radGridViewTiposExt.Rows[indice].Cells["NOMBAH"].Value.ToString();
                string informarTipoAux = this.radGridViewTiposExt.Rows[indice].Cells["APNAAH"].Value.ToString();

                string informarTipoAux1 = this.LP.GetText("lblPRT03Solo1", "Sólo 1ro");
                string informarTipoAux2 = this.LP.GetText("lblPRT03Solo2", "Sólo 2do");
                string informarTipoAux3 = this.LP.GetText("lblPRT03Solo3", "Sólo 3ro");
                string informarTipoAuxTodos = this.LP.GetText("lblPRT03Todos", "Todos");

                string posTipoAux = "0";
                if (informarTipoAux == informarTipoAux1) posTipoAux = "1";
                else if (informarTipoAux == informarTipoAux2) posTipoAux = "2";
                else if (informarTipoAux == informarTipoAux3) posTipoAux = "3";

                string estado = this.radGridViewTiposExt.Rows[indice].Cells["STATAH"].Value.ToString();

                if (estado == this.estadoInactivo) estado = "*";
                else estado = "V";

                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codTipoExt, this.autOperModifica);

                if (operarModificar)
                {
                    frmMtoPRT03 frmMtoTipoExt = new frmMtoPRT03
                    {
                        Nuevo = false,
                        Codigo = this.codTipoExt,
                        Nombre = nombre,
                        Estado = estado,
                        PosTipoAux = posTipoAux,
                        FrmPadre = this
                    };
                    frmMtoTipoExt.Show(this);
                    frmMtoTipoExt.UpdateDataForm += (o, e) =>
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
        private void NuevoTipoExt()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Comprobar sobre ese nuevo tipo de extracontable si tiene autorización a dar de alta

            string error = this.LP.GetText("errValTitulo", "Error");

            frmMtoPRT03 frmMtoTipoExt = new frmMtoPRT03
            {
                Nuevo = true,
                FrmPadre = this
            };
            frmMtoTipoExt.Show(this);
            frmMtoTipoExt.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a eliminar tipo de extracontable
        /// </summary>
        private void EliminarTipoExtCall()
        {
            if (this.radGridViewTiposExt.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.radGridViewTiposExt.CurrentRow.Index;
            int indice = this.radGridViewTiposExt.Rows.IndexOf(this.radGridViewTiposExt.CurrentRow);
            this.codTipoExt = this.radGridViewTiposExt.Rows[indice].Cells["TDATAH"].Value.ToString();

            //Pedir confirmación y eliminar el tipo de comprobante seleccionado
            //this.LP.GetText("wrnDeleteConfirm"       
            string mensaje = "Se va a eliminar el tipo de dato extracontable " + this.codTipoExt;  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                bool resultElimComp = this.EliminarTipoExt();

                if (resultElimComp)
                {
                    //Eliminar la entrada del DataSet
                    this.radGridViewTiposExt.Rows.RemoveAt(indice);

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
            this.ExportarGrid(ref this.radGridViewTiposExt, "Mantenimiento de Tipos de Extracontable");
        }

        /// <summary>
        /// Carga los datos de las TiposExt en la grid
        /// </summary>
        private void FillDataGrid()
        {
            this.radGridViewTiposExt.Visible = false;
            try
            {
                string codigosNoValidos = "'A', 'C', 'D', 'E', 'I', 'M', 'R', 'U', 'Z', 'E1', 'R1', 'U1', 'E2', 'R2', 'U2'";
                string query = "select STATAH, TDATAH, NOMBAH, APNAAH from ";
                query += GlobalVar.PrefijoTablaCG + "PRT03 ";
                query += "where TDATAH not in (" + codigosNoValidos + ") ";
                query += "order by TDATAH";

                this.radGridViewTiposExt.DataSource = null;

                dtTipExt = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Cambiar la columna APNAAH de tipo
                DataTable dtCloned = dtTipExt.Clone();
                dtCloned.Columns["APNAAH"].DataType = typeof(String);
                foreach (DataRow row in dtTipExt.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                string valorCampoEstado = "";
                string valorCampoAPNAAH = "";
                int cont = 0;
                foreach (DataRow dr in dtCloned.Rows)
                {
                    valorCampoEstado = dr["STATAH"].ToString();
                    valorCampoAPNAAH = dr["APNAAH"].ToString();

                    if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                    else valorCampoEstado = this.estadoActivo;

                    valorCampoAPNAAH = this.ValorCampoAPNAAHIdioma(valorCampoAPNAAH);

                    dtCloned.Rows[cont]["STATAH"] = valorCampoEstado;
                    dtCloned.Rows[cont]["APNAAH"] = valorCampoAPNAAH;

                    cont++;
                }

                this.radGridViewTiposExt.DataSource = dtCloned;
                this.RadGridViewHeader();

                if (this.radGridViewTiposExt.Rows != null && this.radGridViewTiposExt.Rows.Count > 0)
                {
                    for (int i = 0; i < this.radGridViewTiposExt.Columns.Count; i++)
                    {
                        this.radGridViewTiposExt.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewTiposExt.Columns[i].Width = 600;
                    }

                    this.radGridViewTiposExt.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewTiposExt.AllowSearchRow = true;
                    this.radGridViewTiposExt.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewTiposExt.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewTiposExt.AllowEditRow = false;
                    this.radGridViewTiposExt.EnableFiltering = true;

                    this.radGridViewTiposExt.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewTiposExt.Rows[0].IsCurrent = true;
                    this.radGridViewTiposExt.Focus();
                    this.radGridViewTiposExt.Select();

                    this.radGridViewTiposExt.Refresh();

                    this.radGridViewTiposExt.Visible = true;

                    //Habilitar/Deshabilitar botones según autorizaciones
                    this.VerificarAutorizaciones();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina un tipo de extracontable
        /// </summary>
        /// <returns></returns>
        private bool EliminarTipoExt()
        {
            bool result = true;

            try
            {
                //Buscar si existen entradas en los asientos de extracontables
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "PRB01 ";
                query += "where TIDAP3 = '" + this.codTipoExt + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrTipoExtEnUso", "El tipo de dato extracontable está en uso"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    result = false;
                    return (result);
                }

                //Eliminar el tipo de comprobante
                query = "delete from " + GlobalVar.PrefijoTablaCG + "PRT03 ";
                query += "where TDATAH = '" + this.codTipoExt + "'";

                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (cantRegistros != 1)
                {
                    RadMessageBox.Show("No fue posible eliminar el Tipo de extracontable.", this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                }
                else
                {
                    //Eliminarlo de las tablas de autorizaciones
                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                        query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + this.codTipoExt + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                        query += "where CLELAG = '" + autClaseElemento + "' and ELEMAG = '" + this.codTipoExt + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string mensaje = this.LP.GetText("lblErrExcEliminarTipoExt", "Error eliminando el tipo de dato extracontable");  //Falta traducir 
                RadMessageBox.Show(mensaje + "( " + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                result = false;
            }

            return (result);
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewTiposExt.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewTiposExt.Columns.Contains(item.Key)) this.radGridViewTiposExt.Columns[item.Key].HeaderText = item.Value;
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

                campo = "STATAH";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);

                campo = "TDATAH";
                header = this.LP.GetText("lblListaCampoTipo", "Tipo");
                this.displayNames.Add(campo, header);

                campo = "NOMBAH";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "APNAAH";
                header = this.LP.GetText("lblListaCampoPosTipoAux", "Informar Tipos Aux.");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        #endregion

    }
}