using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace ModMantenimientos
{
    public partial class frmMtoAutCambioPropietario : frmPlantilla, IReLocalizable
    {
        private string codigo;

        private string prefijoTabla = "";

        private DataTable dtClaseElementoPropietario = new DataTable();
        List<GridViewRowInfo> checkedRows = null;
         
        private DataTable dtUsuarios = new DataTable();
        private DataTable dtUsuariosAux = new DataTable();

        private ArrayList usuariosAutInicio = new ArrayList();

        private DataTable dtUsers = new DataTable();
        private Dictionary<string, string> displayNames;

        public frmMtoAutCambioPropietario()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            //this.radListControlClase.AutoSizeItems = true;
            //this.radListControlClaseAutoriz.AutoSizeItems = true;

            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            if (tipoBaseDatosCG == "DB2")
            {
                this.prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                if (this.prefijoTabla != null && this.prefijoTabla != "") this.prefijoTabla += ".";
            }
            else this.prefijoTabla = GlobalVar.PrefijoTablaCG;

            this.radMultiColumnComboBoxClases.AutoSizeDropDownToBestFit = true;

            //Crear el DataSet
            this.dtClaseElementoPropietario.TableName = "Tabla";
            this.dtClaseElementoPropietario.Columns.Add("CLELAF", typeof(string));
            this.dtClaseElementoPropietario.Columns.Add("ELEMAF", typeof(string));
            this.dtClaseElementoPropietario.Columns.Add("DESCRI", typeof(string));
            this.dtClaseElementoPropietario.Columns.Add("IDUSAF", typeof(string));
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void FrmMtoAutCambioPropietario_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Autorización cambio de propietario");

            this.radMultiColumnComboBoxClases.Visible = true;

            utiles.ButtonEnabled(ref this.radButtonSave, false);

            //Cargar la lista de clases
            this.CargarListaClases();

            this.radGridViewClaseElementoPropietario.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewClaseElementoPropietario.MultiSelect = false;
            this.radGridViewClaseElementoPropietario.AllowSearchRow = true;
            this.radGridViewClaseElementoPropietario.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewClaseElementoPropietario.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewClaseElementoPropietario.AllowEditRow = false;
            this.radGridViewClaseElementoPropietario.EnableFiltering = true;
        }
        
        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            //Actualiza el propietario
            this.GrabarCambioPropietario();
        }

        private void RadButtonBuscarAut_Click(object sender, EventArgs e)
        {
            if (this.radMultiColumnComboBoxClases.SelectedValue != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.codigo = this.radMultiColumnComboBoxClases.SelectedValue.ToString().Trim();

                utiles.ButtonEnabled(ref this.radButtonBuscar, false);

                this.BuildDisplayNames();

                //Carga el DataGrid con la info de clases elementos y propietarios
                this.CargarInfoClaseElementoPropietario();
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadMultiColumnComboBoxUsuarios_SelectedValueChanged(object sender, EventArgs e)
        {
            //Eliminar las filas del DataTable
            this.dtClaseElementoPropietario.Clear();

            this.radGridViewClaseElementoPropietario.Visible = false;
            this.radLabelNoHayInfo.Visible = false;
            this.radPanelUsuarios.Visible = false;

            utiles.ButtonEnabled(ref this.radButtonBuscar, true);
            utiles.ButtonEnabled(ref this.radButtonSave, false);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonBuscar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonBuscar);
        }

        private void RadButtonBuscar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonBuscar);
        }

        private void radListControlUsuarioAutoriz_Leave(object sender, EventArgs e)
        {
            /*
            if (this.radListControlUsuario.Items != null && this.radListControlUsuario.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, false);
                utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, false);
            }*/
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, TreeNodeEditorInitializedEventArgs e)
        {
            var editor = e.Editor as Telerik.WinControls.UI.TreeViewDropDownListEditor;
            Telerik.WinControls.UI.DataFilterCriteriaElement criteriaElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;

            if (editor != null && criteriaElement != null)
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

        private void RadDataFilterGridInfo_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            try
            {
                Telerik.WinControls.UI.DataFilterCriteriaElement dataExpressionFilterElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;
                if (dataExpressionFilterElement != null)
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

        private void FrmMtoAutCambioPropietario_Shown(object sender, EventArgs e)
        {
            this.radGridViewClaseElementoPropietario.Focus();
        }

        private void FrmMtoAutCambioPropietario_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                /*
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtPwd.Text.Trim() != this.txtPwd.Tag.ToString().Trim() ||
                    this.txtNombre.Text != this.txtNombre.Tag.ToString() ||
                    this.radSpinEditorNumPwdUnicas.Text != this.radSpinEditorNumPwdUnicas.Tag.ToString() ||
                    this.radSpinEditorDiasValidezPwd.Text != this.radSpinEditorDiasValidezPwd.Tag.ToString() ||
                    this.radSpinEditorNumMaxDiasInactividad.Text != this.radSpinEditorNumMaxDiasInactividad.Tag.ToString() ||
                    this.cmbUserAdminCG.Text.ToString().Trim() != this.cmbUserAdminCG.Tag.ToString().Trim()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
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
                */
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Autorización cambio de propietario");
        }
        #endregion

        #region Métodos Privados
        private void MoveAllItems(RadListControl sourceListBox, RadListControl targetListBox)
        {
            for (int i = 0; i < sourceListBox.Items.Count;)
            {
                RadListDataItem item = sourceListBox.Items[i];
                sourceListBox.Items.Remove(item);
                targetListBox.Items.Add(item);
            }
        }

        private void MoveToTargetListBox(RadListControl sourceListBox, RadListControl targetListBox)
        {
            if (sourceListBox.Items.Count == 0) { return; }
            if (sourceListBox.SelectedItem == null) { return; }

            RadListDataItem item = sourceListBox.SelectedItem;
            sourceListBox.Items.Remove(item);
            targetListBox.Items.Add(item);
        }

        /// <summary>
        /// Construye el DataTable de todos los usuarios (dtUsuariosAux)
        /// </summary>
        private void BuscarUsuariosTodos()
        {
            try
            {
                //Buscar todos los usuarios
                string query = "select IDUSMO, NOMBMO from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "order by IDUSMO";

                this.dtUsuariosAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtUsuariosAux.Rows != null && dtUsuariosAux.Rows.Count > 0)
                {
                    DataRow dr = dtUsuariosAux.NewRow();
                    dr["IDUSMO"] = "*";
                    dr["NOMBMO"] = "Todas los usuarios";
                    this.dtUsuariosAux.Rows.InsertAt(dr, 0);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                campo = "CLELAF";
                header = this.LP.GetText("lblListaCampoClase", "Clase");
                this.displayNames.Add(campo, header);

                campo = "ELEMAF";
                header = this.LP.GetText("lblListaCampoElemento", "Elemento");
                this.displayNames.Add(campo, header);

                campo = "DESCRI";
                header = this.LP.GetText("lblListaCampoDescripcion", "Descripción");
                this.displayNames.Add(campo, header);

                campo = "IDUSAF";
                header = this.LP.GetText("lblListaCampoPropietarioActual", "Propietario actual");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Carga la lista de usuarios
        /// </summary>
        private void CargarListaUsuarios()
        {
            try
            {
                RadMultiColumnComboBoxElement multiColumnComboElement = this.radMultiColumnComboBoxClases.MultiColumnComboBoxElement;
                multiColumnComboElement.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                multiColumnComboElement.DropDownMinSize = new Size(420, 300);

                multiColumnComboElement.EditorControl.MasterTemplate.AutoGenerateColumns = false;

                GridViewTextBoxColumn column = new GridViewTextBoxColumn("IDUSMO")
                {
                    HeaderText = "Usuario"
                };
                multiColumnComboElement.Columns.Add(column);
                column = new GridViewTextBoxColumn("NOMBMO")
                {
                    HeaderText = "Nombre"
                };
                multiColumnComboElement.Columns.Add(column);

                this.radMultiColumnComboBoxUsuarios.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;

                //Buscar todos los usuarios
                string query = "select IDUSMO, NOMBMO from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "where IDUSMO <> 'CGAUDIT' ";
                query += "order by IDUSMO";

                this.dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                this.radMultiColumnComboBoxUsuarios.DataSource = this.dtUsuarios;

                FilterDescriptor descriptor = new FilterDescriptor(this.radMultiColumnComboBoxUsuarios.DisplayMember, FilterOperator.StartsWith, string.Empty);
                this.radMultiColumnComboBoxUsuarios.EditorControl.FilterDescriptors.Add(descriptor);
                this.radMultiColumnComboBoxUsuarios.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;

                if (this.dtUsuarios != null && this.dtUsuarios.Rows != null && this.dtUsuarios.Rows.Count > 0) utiles.ButtonEnabled(ref this.radButtonSave, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información en el DataGrid (clases/Elementos/Propietarios)
        /// </summary>
        private void CargarInfoClaseElementoPropietario()
        {
            try
            {
                //Si es el usuario CGIFS o admin (si campo UADMMO = 1 del usuario logado) no se filtra
                //Sino mostrar solo los elementos que el propietario sea el mismo que el logado
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "ATM07 ";

                string queryFiltroClase = "";
                string queryFiltroSeguridad = "";

                if (this.codigo.Trim() != "*") queryFiltroClase += "CLELAF ='" + this.codigo + "' ";
                if (GlobalVar.UsuarioLogadoCG != "CGIFS")
                    if (GlobalVar.UsuarioLogadoCG_TipoSeguridad != "1") queryFiltroSeguridad = "IDUSAF = '" + GlobalVar.UsuarioLogadoCG + "'";

                if (queryFiltroClase != "" || queryFiltroSeguridad != "")
                {
                    query += "where ";
                    if (queryFiltroClase != "") query += queryFiltroClase;
                    if (queryFiltroSeguridad != "")
                    {
                        if (queryFiltroClase != "") query += "and " + queryFiltroSeguridad;
                        else query += queryFiltroSeguridad;
                    }
                }

                query += " order by CLELAF";

                DataTable dtAux = new DataTable();
                dtAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtAux != null && dtAux.Rows != null && dtAux.Rows.Count > 0)
                {
                    DataRow row;
                    string clase = "";
                    string elemento = "";
                    string descripcion = "";
                    ElementoClase elementoClase = new ElementoClase();

                    for (int i = 0; i < dtAux.Rows.Count; i++)
                    {
                        row = this.dtClaseElementoPropietario.NewRow();
                        clase = dtAux.Rows[i]["CLELAF"].ToString();
                        row["CLELAF"] = clase;
                        elemento = dtAux.Rows[i]["ELEMAF"].ToString();
                        row["ELEMAF"] = elemento;
                        elementoClase.Clase = clase.Trim();
                        elementoClase.Elemento = elemento.Trim();
                        descripcion = elementoClase.GetDescripcion();
                        row["DESCRI"] = descripcion;
                        row["IDUSAF"] = dtAux.Rows[i]["IDUSAF"].ToString();
                        this.dtClaseElementoPropietario.Rows.Add(row);
                    }

                    this.radGridViewClaseElementoPropietario.AutoGenerateColumns = true;
                    this.radGridViewClaseElementoPropietario.DataSource = this.dtClaseElementoPropietario;

                    if (!this.radGridViewClaseElementoPropietario.Columns.Contains("SEL"))
                    {
                        var checkBoxColumn = new CustomCheckBoxColumn
                        {
                            FieldName = "SEL",
                            Name = "SEL",
                            Width = 10,
                            IsVisible = true,
                            DataType = typeof(string),
                            DataTypeConverter = new ToggleStateConverter()
                        };
                        this.radGridViewClaseElementoPropietario.Columns.Insert(0, checkBoxColumn);

                        this.radGridViewClaseElementoPropietario.Columns["CLELAF"].HeaderText = "Clase";
                        this.radGridViewClaseElementoPropietario.Columns["CLELAF"].ReadOnly = true;
                        this.radGridViewClaseElementoPropietario.Columns["ELEMAF"].HeaderText = "Elemento";
                        this.radGridViewClaseElementoPropietario.Columns["ELEMAF"].ReadOnly = true;
                        this.radGridViewClaseElementoPropietario.Columns["DESCRI"].HeaderText = "Descripción";
                        this.radGridViewClaseElementoPropietario.Columns["DESCRI"].ReadOnly = true;
                        this.radGridViewClaseElementoPropietario.Columns["IDUSAF"].HeaderText = "Propietario actual";
                        this.radGridViewClaseElementoPropietario.Columns["IDUSAF"].ReadOnly = true;
                    }

                    this.radGridViewClaseElementoPropietario.Visible = true;                    
                    this.radPanelUsuarios.Visible = true;

                    //Cargar el desplegable de usuarios
                    this.CargarListaUsuarios();

                    this.radGridViewClaseElementoPropietario.Focus();
                }
                else
                {
                    this.radLabelNoHayInfo.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba el cambio de propietario
        /// </summary>
        private void GrabarCambioPropietario()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Obtener las filas seleccionadas para realizar el cambio de propietario
                this.checkedRows = this.GetCheckedRows(this.radGridViewClaseElementoPropietario);

                if (this.checkedRows.Count == 0)
                {
                    RadMessageBox.Show("Debe seleccionar un elemento", "Error");
                    return;
                }

                string clase = "";
                string elemento = "";
                string usuarioActual = "";
                string nuevoUsuario = this.radMultiColumnComboBoxUsuarios.SelectedValue.ToString().Trim();
                int registros = 0;
                int cont = 0;
                string query = "";

                foreach (GridViewRowInfo row in this.checkedRows)
                {
                    if (row.Cells["CLELAF"].Value != null) clase = row.Cells["CLELAF"].Value.ToString();
                    else clase = "";

                    if (row.Cells["ELEMAF"].Value != null) elemento = row.Cells["ELEMAF"].Value.ToString();
                    else elemento = "";

                    if (row.Cells["IDUSAF"].Value != null) usuarioActual = row.Cells["IDUSAF"].Value.ToString();
                    else usuarioActual = "";

                    if (clase != "" && elemento != "" && usuarioActual != "")
                    {
                        //Actualizar el propietario
                        query = "update " + GlobalVar.PrefijoTablaCG + "ATM07 set IDUSAF = '" + nuevoUsuario;
                        query += "' where CLELAF = '" + clase + "' and ELEMAF = '" + elemento + "' and IDUSAF = '";
                        query += usuarioActual + "'";

                        registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (registros == 1)
                        {
                            cont++;
                            row.Cells["IDUSAF"].Value = nuevoUsuario;
                        }
                    }                
                }

                RadMessageBox.Show("Se han cambiado " + cont.ToString() + " elementos", "Resultado");
                this.dtClaseElementoPropietario.AcceptChanges();

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                RadMessageBox.Show("Se ha producido un error, por favor revise el archivo de Log", "Error");
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Carga la lista de clases
        /// </summary>
        private void CargarListaClases()
        {
            try
            {
                RadMultiColumnComboBoxElement multiColumnComboElement = this.radMultiColumnComboBoxClases.MultiColumnComboBoxElement;
                multiColumnComboElement.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                multiColumnComboElement.DropDownMinSize = new Size(420, 300);

                multiColumnComboElement.EditorControl.MasterTemplate.AutoGenerateColumns = false;

                GridViewTextBoxColumn column = new GridViewTextBoxColumn("CLELAA")
                {
                    HeaderText = "Clase"
                };
                multiColumnComboElement.Columns.Add(column);
                column = new GridViewTextBoxColumn("NOMBAA")
                {
                    HeaderText = "Nombre"
                };
                multiColumnComboElement.Columns.Add(column);

                this.radMultiColumnComboBoxClases.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                
                string query = "select CLELAA, NOMBAA from " + this.prefijoTabla + "ATM02 ";
                query += "where CLELAA < '800' ";
                query += "order by CLELAA";

                this.dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtUsuarios.Rows != null && dtUsuarios.Rows.Count > 0)
                {
                    DataRow dr = dtUsuarios.NewRow();
                    dr["CLELAA"] = "*";
                    dr["NOMBAA"] = "Todas las clases";
                    this.dtUsuarios.Rows.InsertAt(dr, 0);
                    //this.radPanelUsuarios.Enabled = true;
                }

                this.radButtonBuscar.Visible = true;

                this.radMultiColumnComboBoxClases.DataSource = this.dtUsuarios;

                FilterDescriptor descriptor = new FilterDescriptor(this.radMultiColumnComboBoxClases.DisplayMember, FilterOperator.StartsWith, string.Empty);
                this.radMultiColumnComboBoxClases.EditorControl.FilterDescriptors.Add(descriptor);
                this.radMultiColumnComboBoxClases.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve las filas seleccionadas
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        List<GridViewRowInfo> GetCheckedRows(RadGridView grid)
        {
            checkedRows = new List<GridViewRowInfo>();
            foreach (GridViewRowInfo row in grid.Rows)
            {
                if (row.Cells["SEL"].Value != null)
                {
                    if (row.Cells["SEL"].Value.ToString() == "Y")
                    {
                        checkedRows.Add(row);
                    }
                }
            }
            return checkedRows;
        }
        #endregion
    }
}