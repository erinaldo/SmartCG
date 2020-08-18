using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Collections;
using ObjectModel;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmListaGenerica : frmPlantilla, IReLocalizable
    {
        private string tabla;
        private string titulo;

        private string autClaseElemento;
        private string autGrupo;
        private string autOperConsulta;
        private string autOperModifica;

        private string campoCodigo;
        private string campoEstado;
        private string campoEstadoValor;

        private string camposGrid;
        private string camposOrdenar;
        private string camposGridDesc;
        private ArrayList camposGridYDesc;
       
        private const string nombreFicheroInicial = "Tabla-";

        private XmlDocument tablaXML;

        private DataTable dtDatos;

        //private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 42);
        //private static int radCollapsiblePanelDataFilterExpandedHeight = 0;

        Dictionary<string, string> displayNames;
        
        public string Tabla
        {
            get
            {
                return (this.tabla);
            }
            set
            {
                this.tabla = value;
            }
        }

        public string Titulo
        {
            get
            {
                return (this.titulo);
            }
            set
            {
                this.titulo = value;
            }
        }

        public string AutClaseElemento
        {
            get
            {
                return (this.autClaseElemento);
            }
            set
            {
                this.autClaseElemento = value;
            }
        }

        public string AutGrupo
        {
            get
            {
                return (this.autGrupo);
            }
            set
            {
                this.autGrupo = value;
            }
        }

        public string AutOperConsulta
        {
            get
            {
                return (this.autOperConsulta);
            }
            set
            {
                this.autOperConsulta = value;
            }
        }

        public string AutOperModifica
        {
            get
            {
                return (this.autOperModifica);
            }
            set
            {
                this.autOperModifica = value;
            }
        }

        public frmListaGenerica()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Eliminar las filas del DataTable
                this.dtDatos.Clear();
    
                //Volver a cargar los valores de la tabla solicitada
                string result = this.FillDataGrid(camposGrid, camposOrdenar, campoEstado, campoEstadoValor);
                
                if (e!= null && this.radGridViewInfo != null && this.radGridViewInfo.Rows != null && this.radGridViewInfo.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewInfo.Rows.Count; i++)
                            {
                                if (this.radGridViewInfo.Rows[i].Cells[this.campoCodigo].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewInfo.Rows[i].IsCurrent = true;
                                    this.radGridViewInfo.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewInfo.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;
                                    
                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewInfo.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewInfo.Rows[0].IsCurrent = true;
                            this.radGridViewInfo.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewInfo.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewInfo.Focus();

                            break;
                    }  
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FrmListaGenerica_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO " + this.titulo + " Gestión ");

            //Traducir los literarels del formulario
            this.TraducirLiterales();

            this.Text = this.titulo;

            //Cargar la información de la tabla
            string result = this.CargarInfo();

            if (this.radGridViewInfo.Rows.Count > 0)
            {
                for (int i = 0; i < this.radGridViewInfo.Columns.Count; i++)
                {
                    this.radGridViewInfo.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    //this.radGridViewInfo.Columns[i].BestFit();
                    this.radGridViewInfo.Columns[i].Width = 600;
                }

                this.radGridViewInfo.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                this.radGridViewInfo.AllowSearchRow = true;
                this.radGridViewInfo.MasterView.TableSearchRow.IsVisible = false;
                this.radGridViewInfo.TableElement.SearchHighlightColor = Color.Aqua;
                this.radGridViewInfo.AllowEditRow = false;
                this.radGridViewInfo.EnableFiltering = true;
                //this.radGridViewInfo.MasterView.TableFilteringRow.IsVisible = false;

                //this.radGridViewInfo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                //this.radGridViewInfo.MasterTemplate.BestFitColumns();
                this.radGridViewInfo.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                this.radGridViewInfo.Rows[0].IsCurrent = true;
                this.radGridViewInfo.Focus();
                this.radGridViewInfo.Select();
                //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radPanelApp.Size.Height - this.radCollapsiblePanelDataFilter.Size.Height - 3);
                //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, 609);

                this.radGridViewInfo.Refresh();
            }

            //no mostrar columna de busqueda al inicar
            //GridViewSearchRowInfo row = radGridViewInfo.MasterView.TableSearchRow;
            //row.IsVisible = !row.IsVisible;
        }

        private void MenuGridButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoElemento();
        }

        private void MenuGridButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarElemento();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void TxtIdFiscal_DNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadGridViewInfo_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarElemento();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoElemento();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarElemento();
        }
        
        //private void radCollapsiblePanelDataFilter_Expanded(object sender, EventArgs e)
        //{
        //    radCollapsiblePanelDataFilterExpandedHeight = this.radCollapsiblePanelDataFilter.Height;
        //    this.radGridViewInfo.Location = new Point(this.radGridViewInfo.Location.X, this.radGridViewInfo.Location.Y + this.radCollapsiblePanelDataFilter.Size.Height);
        //    this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radGridViewInfo.Size.Height - this.radCollapsiblePanelDataFilter.Size.Height);
        //}

        //private void radCollapsiblePanelDataFilter_Collapsed(object sender, EventArgs e)
        //{
        //    this.radGridViewInfo.Location = gridInfoLocation;
        //    if (primeraLlamada) primeraLlamada = false;
        //    else this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radGridViewInfo.Size.Height + radCollapsiblePanelDataFilterExpandedHeight);
        //}

        //private void radDataFilterGridInfo_Edited(object sender, Telerik.WinControls.UI.TreeNodeEditedEventArgs e)
        //{
        //    this.radDataFilterGridInfo.ApplyFilter();
        //}

        //private void radDataFilterGridInfo_NodeRemoved(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        //{
        //    this.radDataFilterGridInfo.ApplyFilter();
        //}

        //private void radDataFilterGridInfo_EditorInitialized(object sender, Telerik.WinControls.UI.TreeNodeEditorInitializedEventArgs e)
        //{
        //    var editor = e.Editor as Telerik.WinControls.UI.TreeViewDropDownListEditor;
        //    Telerik.WinControls.UI.DataFilterCriteriaElement criteriaElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;

        //    if (editor != null && criteriaElement != null)
        //    {
        //        if (criteriaElement.EditingElement is Telerik.WinControls.UI.DataFilterFieldEditorElement)
        //        {
        //            var element = editor.EditorElement as Telerik.WinControls.UI.BaseDropDownListEditorElement;
        //            element.DataSource = displayNames;
        //            element.ValueMember = "Key";
        //            element.DisplayMember = "Value";
        //        }
        //    }
        //}

        //private void radDataFilterGridInfo_NodeFormatting(object sender, Telerik.WinControls.UI.TreeNodeFormattingEventArgs e)
        //{
        //    try
        //    {
        //        if (e.NodeElement != null)
        //        {
        //            Telerik.WinControls.UI.DataFilterCriteriaElement dataExpressionFilterElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;
        //            if (dataExpressionFilterElement != null)
        //            {
        //                var node = dataExpressionFilterElement.Data as Telerik.WinControls.UI.DataFilterCriteriaNode;
        //                if (node != null)
        //                {
        //                    if (displayNames != null && node.PropertyName != null && displayNames.ContainsKey(node.PropertyName))
        //                    {
        //                        if (dataExpressionFilterElement.FieldElement != null) dataExpressionFilterElement.FieldElement.Text = displayNames[node.PropertyName];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

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

        private void RadButtonExport_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExport);
        }

        private void RadButtonExport_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExport);
        }

        private void FrmListaGenerica_Shown(object sender, EventArgs e)
        {
            //if (this.radGridViewInfo.Size.Height > 8)
            //    this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radGridViewInfo.Size.Height - 8);
            //this.radGridViewInfo.Refresh();

            //this.radCollapsiblePanelDataFilter.IsExpanded = false;
            //this.radCollapsiblePanelDataFilter.EnableAnimation = false;

            this.radGridViewInfo.Focus();
        }

        private void RadGridViewInfo_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewInfo, ref this.selectAll);
        }

        private void FrmListaGenerica_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN " + this.titulo + " Gestión ");
        }

        private void radGridViewInfo_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            try
            {
                this.radLabelHeader.Text = this.titulo;

                //Traducir los Literales de los ToolStrip
                this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
                this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el DataGrid y carga los datos del mantenimiento
        /// </summary>
        /// <returns></returns>
        private string CargarInfo()
        {
            string result = "";

            string campo = "";
            string nombreTabla = "";
            this.camposGrid = "";
            this.camposOrdenar = "";
            this.campoEstado = "";
            this.campoEstadoValor = "";
            this.campoCodigo = "";
            try
            {
                string path = Application.StartupPath + "\\app\\mtos\\";
                string fichero = path + nombreFicheroInicial + this.tabla + ".xml";

                this.tablaXML = new XmlDocument();
                //Leer el fichero XML.
                this.tablaXML.Load(fichero);

                //Recorrer los nodos del fichero XML que describe la tabla
                XmlNodeList baseNodeList = this.tablaXML.SelectNodes("Tabla");

                foreach (XmlNode xmlnodeRoot in baseNodeList)
                {
                    nombreTabla = xmlnodeRoot["nombre"].InnerText.Trim();

                    if (nombreTabla == "") 
                    {
                        result = "Error al cargar el mantenimiento. No existe el nombre de la tabla";   //Falta traducir
                        return (result);
                    }

                    //Chequear que sea el XMl de la tabla solicitada
                    if (nombreTabla.ToUpper() != this.tabla)
                    {
                        result = "El fichero no corresponde con el mantenimiento de tabla solicitado"; //FALTA TRADUCIR
                        return(result);
                    }

                    try { this.camposGrid = xmlnodeRoot["lista"].InnerText; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    try { this.camposOrdenar = xmlnodeRoot["orden"].InnerText; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    try { this.campoCodigo = xmlnodeRoot["codigo"].InnerText; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    try { this.campoEstado = xmlnodeRoot["estado"].InnerText; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    try { this.campoEstadoValor = xmlnodeRoot["estadoValor"].InnerText; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    break;
                }

                if (this.tabla == "GLM01") //caso ULACMGAAAA-PP de GLM01, reemplazar campo para dar formato a la columna
                {
                    //Leer configuración del Tipo de Base de Datos
                    string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                    if (proveedorTipo == "DB2")
                    {
                        this.camposGrid = this.camposGrid.Replace("ULACMGAAAA-PP", "case ULACMG when 0 then ' ' else SUBSTR((ULACMG+190000), 1, 4) || '-' || SUBSTR((ULACMG+190000), 5, 2) end ULACMG");
                    }
                    if (proveedorTipo == "SQLServer")
                    {
                        this.camposGrid = this.camposGrid.Replace("ULACMGAAAA-PP", "case ULACMG when 0 then ' ' else SUBSTRING(CONVERT(varchar, ULACMG+190000), 1, 4) + '-' +  SUBSTRING(CONVERT(varchar, ULACMG+190000), 5, 2) end ULACMG");
                    }
                    if (proveedorTipo == "Oracle")
                    {
                        this.camposGrid = this.camposGrid.Replace("ULACMGAAAA-PP", "case ULACMG when 0 then ' ' else SUBSTR((ULACMG+190000), 1, 4) || '-' || SUBSTR((ULACMG+190000), 5, 2) end ULACMG");
                    }
                }

                if (result == "")
                {
                    this.dtDatos = new DataTable
                    {
                        TableName = "Tabla"
                    };

                    string tipo = "";
                    string longitud = "";
                    string descIdioma = "";
                    string descIdiomaDefecto = "desc_es-ES";
                    string descIdiomaActual = ConfigurationManager.AppSettings["idioma"];
                    if (descIdiomaActual == "") descIdiomaActual = "es-ES";
                    descIdiomaActual = "desc_" + descIdiomaActual;

                    //Título del formulario
                    this.TituloForm();

                    int contador = 0;

                    //Iterar por todos los nodos de tipo Campo
                    baseNodeList = this.tablaXML.SelectNodes("Tabla/Campo");

                    this.camposGridDesc = "";

                    this.camposGridYDesc = new ArrayList();

                    foreach (XmlNode xmlnodeCampo in baseNodeList)
                    {
                        try
                        {
                            campo = xmlnodeCampo["nombre"].InnerText;
                            tipo = xmlnodeCampo["tipo"].InnerText;
                            longitud = xmlnodeCampo["longitud"].InnerText;
                            try
                            {
                                descIdioma = xmlnodeCampo[descIdiomaActual].InnerText;
                            }
                            catch(Exception ex)
                            {
                                Log.Warn(Utiles.CreateExceptionString(ex));

                                descIdioma = xmlnodeCampo[descIdiomaDefecto].InnerText;
                            }

                            //Adicionar la columna al DataTable
                            this.dtDatos.Columns.Add(campo, typeof(string));
                            //Crear la columna del DataGrid
                            //this.dgInfo.AddTextBoxColumn(contador, campo, descIdioma, 100, Convert.ToInt16(longitud), typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
                            contador++;

                            if (this.camposGridDesc != "") this.camposGridDesc += ", " + descIdioma;
                            else this.camposGridDesc += descIdioma;

                            string[] camposGridYDescAux = new string[2];
                            camposGridYDescAux[0] = descIdioma;
                            camposGridYDescAux[1] = campo;

                            this.camposGridYDesc.Add(camposGridYDescAux);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    this.radGridViewInfo.DataSource = this.dtDatos;
                    //this.radDataFilterGridInfo.DataSource = this.radGridViewInfo.DataSource;

                    //Llenar el DataGrid con los valores de la tabla
                    result = this.FillDataGrid(camposGrid, camposOrdenar, campoEstado, campoEstadoValor);

                    //Control autorizaciones (Nuevo y Editar)
                    this.VerificarAutorizaciones();

                    this.radGridViewInfo.ClearSelection();

                    displayNames = new Dictionary<string, string>();

                    try
                    {
                        int cantcolumnas = this.camposGridYDesc.Count;
                        string[] nombreColumnaCampo;
                        for (int i = 0; i < this.camposGridYDesc.Count; i++)
                        {
                            nombreColumnaCampo = (string[])this.camposGridYDesc[i];
                            this.radGridViewInfo.Columns[i].HeaderText = nombreColumnaCampo[0];

                            /*
                            displayMember = nombreColumnaCampo[0];  //header columna grid
                            valueMember = nombreColumnaCampo[1];  //valor columna grid
                            */

                            displayNames.Add(nombreColumnaCampo[1], nombreColumnaCampo[0]);
                        }

                        /*
                        Telerik.WinControls.UI.DataFilterComboDescriptorItem comboDescriptorItem = new Telerik.WinControls.UI.DataFilterComboDescriptorItem();
                        comboDescriptorItem.DataSource = categoriesBindingSource;
                        comboDescriptorItem.DisplayMember = "CategoryName";
                        comboDescriptorItem.ValueMember = "CategoryID";
                        comboDescriptorItem.Name = "CategoryID";
                        comboDescriptorItem.DescriptorName = "CategoryID";
                        comboDescriptorItem.DescriptorType = typeof(int);
                        this.radDataFilterGridInfo.Descriptors.Add(comboDescriptorItem);
                        */

                        /*
                        if (this.radGridViewInfo.Rows != null && this.radGridViewInfo.Rows.Count > 0)
                        {
                            for (int i = 0; i < this.radGridViewInfo.Columns.Count; i++)
                            {
                                this.radGridViewInfo.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                            }

                            //this.radGridViewInfo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                            //this.radGridViewInfo.MasterTemplate.BestFitColumns();

                            this.radGridViewInfo.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                            this.radGridViewInfo.Refresh();
                        }
                        */
                    }
                    catch
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = ex.Message;
            }

            return(result);
        }

        /// <summary>
        /// Llena el DataGrid con los valores de la tabla del mantenimiento solicitado
        /// </summary>
        /// <param name="nombreTabla">Nombre de la tabla</param>
        /// <param name="camposGrid">Campos a visualizar</param>
        /// <param name="camposOrdenar">Criterio de ordenación</param>
        /// <returns></returns>
        private string FillDataGrid(string camposGrid, string camposOrdenar, string campoEstado, string campoEstadoValor)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (this.radGridViewInfo.Columns.Count > 0)
                {
                    //Ejecutar la consulta
                    string query = "";
                    if (camposGrid != "") query = "select " + camposGrid + " from " + GlobalVar.PrefijoTablaCG + this.tabla;
                    else query = "select " + camposGrid + " from " + GlobalVar.PrefijoTablaCG + this.tabla;

                    if (camposOrdenar != "") query += " order by " + camposOrdenar;

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    DataRow row;
                    //Telerik.WinControls.UI.GridViewRowInfo row;
                    string columnName = "";
                    string valor = "";
                    while (dr.Read())
                    {
                        row = this.dtDatos.NewRow();
                        for (int i = 0; i < this.dtDatos.Columns.Count; i++)
                            row[i] = "";

                        for (int i = 0; i < this.dtDatos.Columns.Count; i++)
                        {
                            columnName = this.dtDatos.Columns[i].ColumnName;
                            try
                            {
                                valor = dr.GetValue(dr.GetOrdinal(columnName)).ToString();
                                if (columnName == campoEstado)
                                {
                                    switch (campoEstadoValor)
                                    {
                                        case "estadoActiva":
                                            if (valor.Trim() == "*")  valor = this.estadoInactiva;
                                            else valor = this.estadoActiva;
                                            break;
                                        case "estadoActivo":
                                            if (valor.Trim() == "*") valor = this.estadoInactivo;
                                            else valor = this.estadoActivo;
                                            break;
                                    }
                                }
                                row[i] = valor;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }

                        this.dtDatos.Rows.Add(row);
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = ex.Message;
            }

            return (result);
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

                bool operarConsulta = false;
                bool operarModificar = false;

                bool hayDatos = false;
                if (this.radGridViewInfo.Rows != null && this.radGridViewInfo.Rows.Count > 0)
                {
                    hayDatos = true;

                    operarConsulta = aut.Operar(this.autClaseElemento, this.autGrupo, this.autOperConsulta);
                    operarModificar = aut.Operar(this.autClaseElemento, this.autGrupo, this.autOperModifica);
                }

                //Botón Editar
                if (!hayDatos || (!operarConsulta && !operarModificar))
                {
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    if (!hayDatos) utiles.ButtonEnabled(ref this.radButtonExport, false);
                    else utiles.ButtonEnabled(ref this.radButtonExport, true);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonExport, true);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Edita el elemento seleccionado
        /// </summary>
        private void EditarElemento()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewInfo.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                Cursor.Current = Cursors.Default;
                return;
            }

            if (this.radGridViewInfo.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewInfo.CurrentRow.IsExpanded) this.radGridViewInfo.CurrentRow.IsExpanded = false;
                else this.radGridViewInfo.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewInfo.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewInfo.CurrentRow.Index;
                int indice = this.radGridViewInfo.Rows.IndexOf(this.radGridViewInfo.CurrentRow);
                string codigo = this.radGridViewInfo.Rows[indice].Cells[this.campoCodigo].Value.ToString();

                string codigoAut = codigo;
                if (this.tabla == "GLT08") codigoAut = this.radGridViewInfo.Rows[indice].Cells["TAUXGA"].Value.ToString() + codigo;

                if (this.tabla == "GLT22") codigoAut = this.radGridViewInfo.Rows[indice].Cells["TIPLGC"].Value.ToString() + codigo;

                bool operarConsulta = aut.Validar(this.autClaseElemento, this.autGrupo, codigoAut, this.autOperConsulta);
                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, codigoAut, this.autOperModifica);

                if (operarConsulta || operarModificar)
                {
                    switch (this.tabla)
                    {
                        case "GLM01":   //Mto de Compañías
                            frmMtoGLM01 frmEdit = new frmMtoGLM01
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEdit.Show(this);
                            frmEdit.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLM02":   //Planes de cuentas contables
                            frmMtoGLM02 frmEditGLM02 = new frmMtoGLM02
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditGLM02.Show(this);
                            frmEditGLM02.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLT03":   //Tabla de monedas
                            frmMtoGLT03 frmEditGLT03 = new frmMtoGLT03
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditGLT03.Show(this);
                            frmEditGLT03.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLM04":   //Mto de Tipos de Auxiliar
                            frmMtoGLM04 frmEditGLM04 = new frmMtoGLM04
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditGLM04.Show(this);
                            frmEditGLM04.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLM10":   //Mto de Clases de Zona
                            frmMtoGLM10 frmEditGLM10 = new frmMtoGLM10
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditGLM10.Show(this);
                            frmEditGLM10.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLM07":   //Mto de Grupos de Compañías
                            frmMtoGLM07 frmEditGLM07 = new frmMtoGLM07
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditGLM07.Show(this);
                            frmEditGLM07.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLT08":   //Mto de Grupos de Cuentas de Auxiliar
                            string codigoTipoAux = this.radGridViewInfo.Rows[indice].Cells["TAUXGA"].Value.ToString();
                            frmMtoGLT08 frmEditGLT08 = new frmMtoGLT08
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                CodigoTipoAux = codigoTipoAux,
                                FrmPadre = this
                            };
                            
                            frmEditGLT08.Show(this);
                            frmEditGLT08.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "GLT22":   //Mto de Grupos de Cuentas de Mayor
                            string codigoPlan = this.radGridViewInfo.Rows[indice].Cells["TIPLGC"].Value.ToString();
                            frmMtoGLT22 frmEditGLT22 = new frmMtoGLT22
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                CodigoPlan = codigoPlan,
                                FrmPadre = this
                            };
                            
                            frmEditGLT22.Show(this);
                            frmEditGLT22.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                        case "IVM05":   //Mto de Maestro de CIF / DNI
                            frmMtoIVM05 frmEditIVM05 = new frmMtoIVM05
                            {
                                Nuevo = false,
                                Codigo = codigo,
                                FrmPadre = this
                            };
                            frmEditIVM05.Show(this);
                            frmEditIVM05.UpdateDataForm += (o, e) =>
                            {
                                ActualizaListaElementos(e);
                            };
                            break;
                    }
                }
                else
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), error);   //Falta traducir
                }
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewInfo, this.titulo);
        }

        /// <summary>
        /// Activa o desactiva el elemento seleccionado
        /// </summary>
        private void ActivarDesactivarElemento()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;


            /*if (this.dgComprobantes.SelectedRows.Count > 1)
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(this.LP.GetText("lblErrSelSoloUnComp", "Debe seleccionar un solo comprobante"), error);
            }
            else this.EditarComprobante(this.dgComprobantes.CurrentRow.Index);
            */


            //int indice = this.radGridViewInfo.CurrentRow.Index;
            int indice = this.radGridViewInfo.Rows.IndexOf(this.radGridViewInfo.CurrentRow);
            string codigo = this.radGridViewInfo.Rows[indice].Cells[this.campoCodigo].Value.ToString();

            bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, codigo, this.autOperModifica);

            if (operarModificar)
            {
                //if (this.dgInfo.SelectedRows.Count == 1)
                //{
                string valorEstado = this.radGridViewInfo.Rows[indice].Cells[this.campoEstado].Value.ToString();
                string nuevoValor = "";

                if (valorEstado == this.estadoActiva || valorEstado == this.estadoActivo) nuevoValor = "*";
                else if (valorEstado == this.estadoInactiva || valorEstado == this.estadoInactivo) nuevoValor = "V";

                if (nuevoValor != "")
                {

                    string valorCodigo = this.radGridViewInfo.Rows[indice].Cells[this.campoCodigo].Value.ToString();

                    string query = "update " + GlobalVar.PrefijoTablaCG + this.tabla + " set " + this.campoEstado + " = '" + nuevoValor;
                    query += "' where " + this.campoCodigo + " = '" + valorCodigo + "'";

                    RadMessageBox.Show(this.LP.GetText("lblErrEstadoAct", "FALTA PROGRAMAR LA FUNCIONALIDAD"), "");   //Falta traducir
                }
                else
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(this.LP.GetText("lblErrEstadoAct", "No se pudieron recuperar los valores del estado"), error);   //Falta traducir
                }
                /*}
                else
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(this.LP.GetText("lblErrSelElemento", "Debe seleccionar un elemento"), error);   //Falta traducir
                }*/
            }
            else
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), error);   //Falta traducir

            }
        }

        /// <summary>
        /// Llama al formulario de creación de elementos que corresponda
        /// </summary>
        private void NuevoElemento()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            switch (this.tabla)
            {
                case "GLM01":   //Mto de Compañías
                    frmMtoGLM01 frmEdit = new frmMtoGLM01
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEdit.Show(this);
                    frmEdit.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLM02":   //Planes de cuentas contables
                    frmMtoGLM02 frmEditGLM02 = new frmMtoGLM02
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditGLM02.Show(this);
                    frmEditGLM02.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLT03":   //Tabla de monedas
                    frmMtoGLT03 frmEditGLT03 = new frmMtoGLT03
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditGLT03.Show(this);
                    frmEditGLT03.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLM04":   //Mto de Compañías
                    frmMtoGLM04 frmEditGLM04 = new frmMtoGLM04
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditGLM04.Show(this);
                    frmEditGLM04.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLM10":   //Mto de Clases de Zona
                    frmMtoGLM10 frmEditGLM10 = new frmMtoGLM10
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditGLM10.Show(this);
                    frmEditGLM10.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLM07":   //Mto de Grupos de Compañías
                    frmMtoGLM07 frmEditGLM07 = new frmMtoGLM07
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditGLM07.Show(this);
                    frmEditGLM07.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLT08":   //Mto de Grupos de Cuentas de Auxiliar
                    frmMtoGLT08 frmEditGLT08 = new frmMtoGLT08
                    {
                        Nuevo = true,
                        Codigo = "",
                        CodigoTipoAux = "",
                        FrmPadre = this
                    };
                    frmEditGLT08.Show(this);
                    frmEditGLT08.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "GLT22":   //Mto de Grupos de Cuentas de Mayor
                    frmMtoGLT22 frmEditGLT22 = new frmMtoGLT22
                    {
                        Nuevo = true,
                        Codigo = "",
                        CodigoPlan = "",
                        FrmPadre = this
                    };
                    frmEditGLT22.Show(this);
                    frmEditGLT22.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "IVM05":   //Mto de Compañías
                    frmMtoIVM05 frmEditIVM05 = new frmMtoIVM05
                    {
                        Nuevo = true,
                        Codigo = "",
                        FrmPadre = this
                    };
                    frmEditIVM05.Show(this);
                    frmEditIVM05.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Inicializa la variable que contiene el título del formulario
        /// </summary>
        private void TituloForm()
        {
            switch (this.tabla)
            {
                case "GLM01":   //Mto de Compañías
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Compañías";
                    break;
                case "GLM02":   //Planes de cuentas contables
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Planes de cuentas";
                    break;
                case "GLT03":   //Tabla de monedas
                    this.radLabelHeader.Text = "Mantenimientos / Tablas auxiliares / Monedas extranjeras";
                    break;
                case "GLM04":   //Mto de Tipos de auxiliares
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Tipos de auxiliares";
                    break;
                case "GLM10":   //Mto de Clases de Zona
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Clase de zona";
                    break;
                case "GLM07":   //Mto de Grupos de Compañías
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Grupos de compañías";
                    break;
                case "GLT08":   //Mto de Grupos de Cuentas de Auxiliar
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Cuentas de Auxiliar";
                    break;
                case "GLT22":   //Mto de Grupos de Cuentas de Mayor
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Cuentas de Mayor";
                    break;
                case "IVM05":   //Mto de Compañías
                    this.radLabelHeader.Text = "Mantenimientos / Tablas maestras / Maestro de CIF/DNI";
                    break;
            }
        }

        #endregion
    }
}
