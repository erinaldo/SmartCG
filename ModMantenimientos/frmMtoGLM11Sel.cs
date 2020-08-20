
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
    public partial class frmMtoGLM11Sel : frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "024";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        private string autOperAlta = "20";
        private bool autEditar = false;

        private string codClaseZona = "";
        private string codZona = "";

        private bool zonaJerarq;

        //private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 112);
        //private static int radCollapsiblePanelDataFilterExpandedHeight = 0;

        Dictionary<string, string> displayNames;
        DataTable dtZonas = new DataTable();

        public frmMtoGLM11Sel()
        {
            InitializeComponent();

        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM11Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Zonas Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            this.radGridViewZonas.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewZonas.AllowSearchRow = true;
            this.radGridViewZonas.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewZonas.EnableFiltering = true;

            this.radGridViewZonas.Visible = false;

            utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonExport, false);

            this.radButtonTextBoxClaseZona.Select();
        }

        private void TgGridZonas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarZona();
        }

        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Eliminar las filas del DataTable
                this.dtZonas.Clear();

                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid();

                if (e != null && this.radGridViewZonas != null && this.radGridViewZonas.Rows != null && this.radGridViewZonas.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewZonas.Rows.Count; i++)
                            {
                                if (this.radGridViewZonas.Rows[i].Cells["ZONAZ1"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewZonas.Rows[i].IsCurrent = true;
                                    this.radGridViewZonas.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewZonas.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewZonas.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewZonas.Rows[0].IsCurrent = true;
                            this.radGridViewZonas.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewZonas.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewZonas.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevaZona();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarZona();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadButtonElementClaseZona_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CLASZ0, NOMBZ0 from ";
            query += GlobalVar.PrefijoTablaCG + "GLM10 ";
            query += "order by CLASZ0";

            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar clase de zona",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnas,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            string separadorCampos = "-";
            string result = "";
            int cantidadColumnasResult = 2;
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
            }

            this.radButtonTextBoxClaseZona.Text = result;
            this.ActiveControl = this.radButtonTextBoxClaseZona;
            this.radButtonTextBoxClaseZona.Select(0, 0);
            //this.radButtonTextBoxClaseZona.Focus();
            this.radGridViewZonas.Focus();
        }

        private void RadButtonTextBoxClaseZona_TextChanged(object sender, EventArgs e)
        {
            string codigo = this.radButtonTextBoxClaseZona.Text.Trim();
            if (codigo != "")
            {
                if (codigo != "" && codigo.Length >= 3)
                {
                    //para evitar que salte 2 veces el evento al asignar la descripcion
                    if (this.radButtonTextBoxClaseZona.Tag != null && this.radButtonTextBoxClaseZona.Text != null)
                    {
                        if (this.radButtonTextBoxClaseZona.Tag.ToString().Trim() != "" && this.radButtonTextBoxClaseZona.Text.Trim() != "")
                        {
                            if (this.radButtonTextBoxClaseZona.Tag.ToString().Substring(0, 3) == this.radButtonTextBoxClaseZona.Text.Substring(0, 3)) return;
                        }
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    if (codigo.Length <= 3) this.codClaseZona = this.radButtonTextBoxClaseZona.Text;
                    else this.codClaseZona = this.radButtonTextBoxClaseZona.Text.Substring(0, 3);

                    string result = ValidarClaseZona(this.codClaseZona);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radButtonTextBoxClaseZona.Focus();

                        this.radGridViewZonas.Visible = false;

                        utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                        utiles.ButtonEnabled(ref this.radButtonEditar, false);
                        utiles.ButtonEnabled(ref this.radButtonExport, false);
                    }
                    else
                    {
                        string claseZona = this.codClaseZona;
                        string claseZonaDesc = utilesCG.ObtenerDescDadoCodigo("GLM10", "CLASZ0", "NOMBZ0", this.codClaseZona, false, "").Trim();
                        if (claseZonaDesc != "") claseZona += " " + separadorDesc + " " + claseZonaDesc;

                        this.radButtonTextBoxClaseZona.Tag = this.radButtonTextBoxClaseZona.Text;
                        this.radButtonTextBoxClaseZona.Text = claseZona;

                        this.VerificarAutorizaciones();

                        //Cargar los datos de la Grid
                        if (this.autEditar) this.FillDataGrid();
                        this.radGridViewZonas.Focus();
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.radButtonTextBoxClaseZona.Focus();
                    this.radButtonTextBoxClaseZona.Tag = "";

                    this.radGridViewZonas.Visible = false;
                }
            }
        }

        private void RadGridViewZonas_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.selectAll = false;
            this.EditarZona();
        }

        private void RadDataFilterGridInfo_Edited(object sender, Telerik.WinControls.UI.TreeNodeEditedEventArgs e)
        {
            this.radDataFilterGridInfo.ApplyFilter();
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

        private void RadGridViewZonas_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewZonas, ref this.selectAll);
        }

        private void FrmMtoGLM11Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Zonas Gestión");
        }

        private void radGridViewZonas_Leave(object sender, EventArgs e)
        {
            //utiles.guardarLayout(this.Name, ref radGridViewZonas);
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoGLM11Titulo", "Mantenimiento de Zonas");   //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");

            //Traducir los campos del formulario
            this.lblClaseZona.Text = this.LP.GetText("lblGLM11ClaseZona", "Clase de zona");
        }

        /// <summary>
        /// Verificar autorizaciones para habilitar/deshabilitar botones
        /// </summary>
        private void VerificarAutorizaciones()
        {
            try
            {
                bool operarCrear = aut.Validar(this.autClaseElemento, this.autGrupo, this.codClaseZona, this.autOperAlta);

                bool operarModificar = false;
                //SMR if (this.radGridViewZonas.Rows != null && this.radGridViewZonas.Rows.Count > 0)
                //SMR {
                operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codClaseZona, this.autOperModifica);
                this.autEditar = operarModificar; //SMR
                //SMR }

                //Botón Nuevo
                if (operarCrear) utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                else utiles.ButtonEnabled(ref this.radButtonNuevo, false);

                utiles.ButtonEnabled(ref this.radButtonEditar, operarModificar);
                utiles.ButtonEnabled(ref this.radButtonExport, true);

                if (!operarModificar)
                {
                    this.radLabelNoHayInfo.Visible = true;
                    this.radLabelNoHayInfo.Text = "Usuario no autorizado a esta clase de zona";
                    this.radGridViewZonas.Visible = false;
                }
                else
                {
                    this.radLabelNoHayInfo.Visible = false;
                    this.radLabelNoHayInfo.Text = "No existen zonas";
                    this.radGridViewZonas.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida la existencia o no de la clase de zona
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarClaseZona(string codigo)
        {
            string result = "";
            try
            {
                string query = "select count(CLASZ0) from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Clase de zona no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmGLM11ValClaseZonaExcep", "Error al validar la clase de zona") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la clase de zona. Si existe devuelve el nombre (codigo - nombre)
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarClaseZonaNombre(string codigo, ref string nombre)
        {
            string result = "";
            nombre = "";

            IDataReader dr = null;
            try
            {
                //Validar q este activo .... FALTA !!!
                string query = "select CLASZ0, NOMBZ0 from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    nombre = dr.GetValue(dr.GetOrdinal("CLASZ0")).ToString() + " - " + dr.GetValue(dr.GetOrdinal("NOMBZ0")).ToString();
                }

                dr.Close();

                if (nombre == "") result = "Clase de zona no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = this.LP.GetText("lblfrmGLM15ValClaseZonaExcep", "Error al validar la clase de zona") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Llamada a Editar una zona
        /// </summary>
        private void EditarZona()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxClaseZona.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Clase de Zona no puede estar en blanco", error);
                this.radButtonTextBoxClaseZona.Focus();
                return;
            }

            if (this.radGridViewZonas.SelectedRows.Count == 0)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            if (this.radGridViewZonas.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string nombre = "";
            string result = ValidarClaseZonaNombre(this.codClaseZona, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxClaseZona.Focus();
                return;
            }

            if (this.radGridViewZonas.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewZonas.CurrentRow.IsExpanded) this.radGridViewZonas.CurrentRow.IsExpanded = false;
                else this.radGridViewZonas.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewZonas.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewZonas.CurrentRow.Index;
                int indice = this.radGridViewZonas.Rows.IndexOf(this.radGridViewZonas.CurrentRow);
                this.codZona = this.radGridViewZonas.Rows[indice].Cells["ZONAZ1"].Value.ToString();
                string nombreZona = this.radGridViewZonas.Rows[indice].Cells["NOMBZ1"].Value.ToString();

                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codZona, this.autOperModifica);

                if (operarModificar)
                {
                    if (this.zonaJerarq)
                    {
                        frmMtoGLM11ZonaJerarq frmMtoZonaJeraq = new frmMtoGLM11ZonaJerarq
                        {
                            Nuevo = false,
                            CodigoClaseZona = this.codClaseZona,
                            NombreClaseZona = nombre,
                            Codigo = this.codZona,
                            NombreZona = nombreZona,
                            FrmPadre = this
                        };
                        frmMtoZonaJeraq.Show(this);
                        frmMtoZonaJeraq.UpdateDataForm += (o, e) =>
                        {
                            ActualizaListaElementos(e);
                        };
                    }
                    else
                    {
                        frmMtoGLM11ZonaNOJerarq frmMtoZonaNOJeraq = new frmMtoGLM11ZonaNOJerarq
                        {
                            Nuevo = false,
                            CodigoClaseZona = this.codClaseZona,
                            NombreClaseZona = nombre,
                            Codigo = this.codZona,
                            NombreZona = nombreZona,
                            FrmPadre = this
                        };
                        frmMtoZonaNOJeraq.Show(this);
                        frmMtoZonaNOJeraq.UpdateDataForm += (o, e) =>
                        {
                            ActualizaListaElementos(e);
                        };
                    }
                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), error);   //Falta traducir
                }
            }
          
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewZonas, "Mantenimiento de Zonas");
        }

        /// <summary>
        /// Llamada a crear nueva zona
        /// </summary>
        private void NuevaZona()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Comprobar sobre esa nueva zona si tiene autorización a dar de alta

            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxClaseZona.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Clase de Zona no puede estar en blanco", error);
                this.radButtonTextBoxClaseZona.Focus();
                return;
            }

            string nombre = "";
            string result = ValidarClaseZonaNombre(this.codClaseZona, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxClaseZona.Focus();
                return;
            }

            this.zonaJerarq = utilesCG.isClaseZonaJerarquica(this.codClaseZona);

            if (this.zonaJerarq)
            {
                frmMtoGLM11ZonaJerarq frmMtoZonaJeraq = new frmMtoGLM11ZonaJerarq
                {
                    Nuevo = true,
                    CodigoClaseZona = this.codClaseZona,
                    NombreClaseZona = nombre,
                    Codigo = this.codZona,
                    NombreZona = "",
                    FrmPadre = this
                };
                frmMtoZonaJeraq.Show(this);
                frmMtoZonaJeraq.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
            }
            else
            {
                frmMtoGLM11ZonaNOJerarq frmMtoZonaNOJeraq = new frmMtoGLM11ZonaNOJerarq
                {
                    Nuevo = true,
                    CodigoClaseZona = this.codClaseZona,
                    NombreClaseZona = nombre,
                    Codigo = this.codZona,
                    NombreZona = "",
                    FrmPadre = this
                };
                frmMtoZonaNOJeraq.Show(this);
                frmMtoZonaNOJeraq.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
            }

            Cursor.Current = Cursors.Default;
        }
        
        /// <summary>
        /// Carga los datos de las zonas en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                string query = "select STATZ1, ZONAZ1, TIPOZ1, NOMBZ1 from ";
                query += GlobalVar.PrefijoTablaCG + "GLM11 ";
                query += "where CLASZ1 = '" + this.codClaseZona + "' ";
                query += "order by ZONAZ1";

                string valorCampoEstado = "";

                dtZonas = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                int cont = 0;
                foreach (DataRow dr in dtZonas.Rows)
                {
                    valorCampoEstado = dr["STATZ1"].ToString();
                    
                    if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactiva;
                    else valorCampoEstado = this.estadoActiva;

                    //this.dtTiposExt.Rows.Add(dr.ItemArray);
                    dtZonas.Rows[cont]["STATZ1"] = valorCampoEstado;
                    cont++;
                }

                this.radGridViewZonas.DataSource = null;
                this.radGridViewZonas.DataSource = dtZonas;
                this.radDataFilterGridInfo.DataSource = this.radGridViewZonas.DataSource;

                //SMR this.RadGridViewHeader();

                if (this.radGridViewZonas.Rows != null && this.radGridViewZonas.Rows.Count > 0)
                {
                    this.radGridViewZonas.Visible = false;

                    this.zonaJerarq = utilesCG.isClaseZonaJerarquica(this.codClaseZona);

                    this.BuildDisplayNames();

                    this.RadGridViewHeader(); //SMR

                    //Ocultar las columna de tipo
                    if (!this.zonaJerarq) this.radGridViewZonas.Columns["TIPOZ1"].IsVisible = false;
                    else this.radGridViewZonas.Columns["TIPOZ1"].IsVisible = true;

                    for (int i = 0; i < this.radGridViewZonas.Columns.Count; i++)
                    {
                        this.radGridViewZonas.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewZonas.Columns[i].Width = 600;
                    }

                    this.radGridViewZonas.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewZonas.AllowSearchRow = true;
                    this.radGridViewZonas.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewZonas.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewZonas.AllowEditRow = false;
                    this.radGridViewZonas.EnableFiltering = true;

                    this.radGridViewZonas.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    if (this.radGridViewZonas.Groups.Count == 0) this.radGridViewZonas.Rows[0].IsCurrent = true;
                    this.radGridViewZonas.Focus();
                    this.radGridViewZonas.Select();

                    //cargar layout
                    //utiles.cargarLayout(this.Name, ref radGridViewZonas);

                    this.radGridViewZonas.Refresh();

                    this.radGridViewZonas.Visible = true;
                    this.radLabelNoHayInfo.Visible = false;

                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonExport, true);
                }
                else
                {
                    this.radLabelNoHayInfo.Visible = true;
                    this.radGridViewZonas.Visible = false;

                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonExport, false);
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

                campo = "STATZ1";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[0].HeaderText = header;

                campo = "ZONAZ1";
                header = this.LP.GetText("lblListaCampoZona", "Zona");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[1].HeaderText = header;

                if (this.zonaJerarq)
                {
                    campo = "TIPOZ1";
                    header = this.LP.GetText("lblListaCampoTipo", "Tipo");
                    this.displayNames.Add(campo, header);
                }

                campo = "NOMBZ1";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[2].HeaderText = header;
            }
            catch
            {
            }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewZonas.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewZonas.Columns.Contains(item.Key)) this.radGridViewZonas.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        private void radPanelApp_Resize(object sender, EventArgs e)
        {
            this.radGridViewZonas.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 121);
        }

        private void radGridViewZonas_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        
    }
}
