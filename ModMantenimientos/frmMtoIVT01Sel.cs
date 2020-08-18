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
    public partial class frmMtoIVT01Sel : frmPlantilla, IReLocalizable
    {
        private string codPlan = "";
        private string codIVA = "";

        private bool planActivo = false;

        private Dictionary<string, string> displayNames;

        private DataTable dtCodIVA = new DataTable();

        public frmMtoIVT01Sel()
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
                this.dtCodIVA.Clear();

                this.FillDataGrid();

                if (e != null && this.radGridViewCodIva!= null && this.radGridViewCodIva.Rows != null && this.radGridViewCodIva.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewCodIva.Rows.Count; i++)
                            {
                                if (this.radGridViewCodIva.Rows[i].Cells["COIVCI"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewCodIva.Rows[i].IsCurrent = true;
                                    this.radGridViewCodIva.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewCodIva.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewCodIva.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewCodIva.Rows[0].IsCurrent = true;
                            this.radGridViewCodIva.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewCodIva.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewCodIva.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FrmMtoIVT01Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Códigos de IVA Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            this.radGridViewCodIva.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewCodIva.AllowSearchRow = true;
            this.radGridViewCodIva.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewCodIva.EnableFiltering = true;

            this.radGridViewCodIva.Visible = false;

            this.radGridViewCodIva.AllowSearchRow = true;
            this.radGridViewCodIva.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewCodIva.AllowEditRow = false;

            utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonExport, false);
            utiles.ButtonEnabled(ref this.radButtonCopiarCodigoIVA, false);

            this.radButtonTextBoxPlan.Select();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoCodigoIVA();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarCodigoIVA();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadButtonCopiarCodigoIVA_Click(object sender, EventArgs e)
        {
            this.CopiarCodigoIVA();
        }

        private void RadButtonElementPlan_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIPLMP, NOMBMP from ";
            query += GlobalVar.PrefijoTablaCG + "GLM02 ";
            query += "order by TIPLMP";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar plan",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
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

                this.radButtonTextBoxPlan.Text = result;
                this.ActiveControl = this.radButtonTextBoxPlan;
                this.radButtonTextBoxPlan.Select(0, 0);
                //this.radButtonTextBoxPlan.Focus();
                this.radGridViewCodIva.Focus();
            }
        }

        private void RadButtonTextBoxPlan_TextChanged(object sender, EventArgs e)
        {
            string codigo = this.radButtonTextBoxPlan.Text.Trim();

            if (codigo != "")
            {
                //this.tgBuscadorCodIVA.ValorFiltro.Text = "";

                if (codigo.Length >= 1)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (codigo.Length <= 1) this.codPlan = this.radButtonTextBoxPlan.Text;
                    else this.codPlan = this.radButtonTextBoxPlan.Text.Substring(0, 1);

                    string result = ValidarPlan(this.codPlan);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        //this.radButtonTextBoxPlan.Focus();
                        this.radGridViewCodIva.Focus();

                        this.radGridViewCodIva.Visible = true;

                        utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                        utiles.ButtonEnabled(ref this.radButtonEditar, false);
                        utiles.ButtonEnabled(ref this.radButtonExport, false);
                        utiles.ButtonEnabled(ref this.radButtonCopiarCodigoIVA, false);
                    }
                    else
                    {
                        string plan = this.codPlan;
                        string planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", this.codPlan, false, "").Trim();
                        if (planDesc != "") plan += " " + separadorDesc + " " + planDesc;

                        this.radButtonTextBoxPlan.Text = plan;

                        //Carga los datos de la Grid
                        this.FillDataGrid();

                        this.radGridViewCodIva.Focus();
                    }

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.radButtonTextBoxPlan.Focus();

                    this.radGridViewCodIva.Visible = false;
                }
            }
            else
            {
                //this.radGridViewCodIva.Enabled = false;

                utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                utiles.ButtonEnabled(ref this.radButtonEditar, false);
                utiles.ButtonEnabled(ref this.radButtonExport, false);
                utiles.ButtonEnabled(ref this.radButtonCopiarCodigoIVA, false);
            }
        }

        private void RadGridViewCodIva_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarCodigoIVA();
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

        private void RadButtonCopiarCodigoIVA_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCopiarCodigoIVA);
        }

        private void RadButtonCopiarCodigoIVA_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCopiarCodigoIVA);
        }

        private void RadGridViewCodIva_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewCodIva, ref this.selectAll);
        }

        private void RadPanelApp_Resize(object sender, EventArgs e)
        {
            this.radGridViewCodIva.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 121);
        }

        private void FrmMtoIVT01Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Códigos de IVA Gestión");
        }

        private void radGridViewCodIva_ViewCellFormatting(object sender, CellFormattingEventArgs e)
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
            this.Text = this.LP.GetText("lblfrmMtoIVT01Titulo", "Mantenimiento de Códigos de IVA");   //Falta traducir

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.radButtonCopiarCodigoIVA.Text = this.LP.GetText("toolStripCopiarCodIVA", "Copiar Código IVA");

            //Traducir los campos del formulario
            this.lblPlan.Text = this.LP.GetText("lblIVT01Plan", "Plan");
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                campo = "STATCI";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);

                campo = "COIVCI";
                header = this.LP.GetText("lblListaCampoCod", "Código");
                this.displayNames.Add(campo, header);

                campo = "NOMBCI";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "LIBRCI";
                header = this.LP.GetText("lblListaCampoLibro", "Libro");
                this.displayNames.Add(campo, header);

                campo = "SERICI";
                header = this.LP.GetText("lblListaCampoSerie", "Serie");
                this.displayNames.Add(campo, header);

                campo = "RESOCI";
                header = this.LP.GetText("lblListaCampoRepercSoport", "Repercutido/Soportado");
                this.displayNames.Add(campo, header);

                campo = "DEDUCI";
                header = this.LP.GetText("lblListaCampoDedNoDeducibleCuenta", "Deducible/No deducible");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        
        /// <summary>
        /// Verificar autorizaciones para habilitar/deshabilitar botones
        /// </summary>
        private void VerificarAutorizaciones()
        {
            try
            {
                utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                utiles.ButtonEnabled(ref this.radButtonEditar, true);
                utiles.ButtonEnabled(ref this.radButtonExport, true);
                utiles.ButtonEnabled(ref this.radButtonCopiarCodigoIVA, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida la existencia o no del plan
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarPlan(string codigo)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    string estado = dr.GetValue(dr.GetOrdinal("STATMP")).ToString();
                    if (estado == "V") this.planActivo = true;
                    else this.planActivo = false;
                }
                else result = "Plan no existe";   //Falta traducir

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmIVT01ValPlanExcep", "Error al validar el plan") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del plan. Si existe devuelve el nombre (codigo - nombre)
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarPlanNombre(string codigo, ref string nombre)
        {
            string result = "";
            nombre = "";

            IDataReader dr = null;
            try
            {
                //Validar q este activo .... FALTA !!!
                string query = "select NOMBMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    nombre = codigo + " - " + dr.GetValue(dr.GetOrdinal("NOMBMP")).ToString();
                }

                dr.Close();

                if (nombre == "") result = "Plan no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = this.LP.GetText("lblfrmIVT01ValPlanExcep", "Error al validar el plan") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Llamada a Editar un código de IVA
        /// </summary>
        private void EditarCodigoIVA()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlan.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            if (this.radGridViewCodIva.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string nombre = "";
            string result = ValidarPlanNombre(this.codPlan, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            if (this.radGridViewCodIva.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewCodIva.CurrentRow.IsExpanded) this.radGridViewCodIva.CurrentRow.IsExpanded = false;
                else this.radGridViewCodIva.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewCodIva.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewCodIva.CurrentRow.Index;
                int indice = this.radGridViewCodIva.Rows.IndexOf(this.radGridViewCodIva.CurrentRow);
                this.codIVA = this.radGridViewCodIva.Rows[indice].Cells["COIVCI"].Value.ToString();
                string nombreCodIVA = this.radGridViewCodIva.Rows[indice].Cells["NOMBCI"].Value.ToString();

                frmMtoIVT01 frmMtoCodigoIVA = new frmMtoIVT01
                {
                    Nuevo = false,
                    Copiar = false,
                    CodigoPlan = this.codPlan,
                    NombrePlan = nombre,
                    Codigo = this.codIVA,
                    NombreCodIVA = nombreCodIVA,
                    PlanActivo = this.planActivo,
                    FrmPadre = this
                };
                frmMtoCodigoIVA.Show(this);
                frmMtoCodigoIVA.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewCodIva, "Mantenimiento de Códigos de IVA");
        }

        /// <summary>
        /// Llamada a crear un nuevo código de IVA
        /// </summary>
        private void NuevoCodigoIVA()
        {
            Cursor.Current = Cursors.WaitCursor;

            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlan.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            string nombre = "";
            string result = ValidarPlanNombre(this.codPlan, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            if (!this.planActivo)
            {
                RadMessageBox.Show("Plan de cuenta inactivo", error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            frmMtoIVT01 frmMtoIVa = new frmMtoIVT01
            {
                Nuevo = true,
                Copiar = false,
                CodigoPlan = this.codPlan,
                NombrePlan = nombre,
                Codigo = "",
                PlanActivo = this.planActivo,
                FrmPadre = this
            };
            frmMtoIVa.Show(this);
            frmMtoIVa.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Copiar un código de IVA
        /// </summary>
        private void CopiarCodigoIVA()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlan.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            if (this.radGridViewCodIva.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string nombre = "";
            string result = ValidarPlanNombre(this.codPlan, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            if (!this.planActivo)
            {
                RadMessageBox.Show("Plan de cuenta inactivo", error);
                this.radButtonTextBoxPlan.Focus();
                return;
            }

            //int indice = this.radGridViewCodIva.CurrentRow.Index;
            int indice = this.radGridViewCodIva.Rows.IndexOf(this.radGridViewCodIva.CurrentRow);
            this.codIVA = this.radGridViewCodIva.Rows[indice].Cells["COIVCI"].Value.ToString();
            string nombreCodIVA = this.radGridViewCodIva.Rows[indice].Cells["NOMBCI"].Value.ToString();

            frmMtoIVT01 frmMtoCodIVA = new frmMtoIVT01
            {
                Nuevo = false,
                Copiar = true,
                CodigoPlan = this.codPlan,
                NombrePlan = nombre,
                Codigo = "",
                NombreCodIVA = nombreCodIVA,
                CodigoIVACopiar = this.codIVA,
                PlanActivo = this.planActivo,
                FrmPadre = this
            };
            frmMtoCodIVA.Show(this);
            frmMtoCodIVA.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Carga los datos de las zonas en la grid
        /// </summary>
        /// <param name="query"></param>
        private void FillDataGrid()
        {
            try
            {
                string query = "select STATCI, COIVCI, NOMBCI, LIBRCI, SERICI, RESOCI, DEDUCI from ";
                query += GlobalVar.PrefijoTablaCG + "IVT01 ";
                query += "where TIPLCI = '" + this.codPlan + "' ";
                query += "order by COIVCI";

                dtCodIVA = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                
                //Habilitar/Deshabilitar botones según autorizaciones
                this.VerificarAutorizaciones();
                
                if (dtCodIVA.Rows != null && dtCodIVA.Rows.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonExport, false);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCodigoIVA, false);
                    this.radGridViewCodIva.Visible = false;
                    this.radLabelNoHayInfo.Visible = true;
                }
                else
                {
                    this.radGridViewCodIva.DataSource = dtCodIVA;
                    this.radDataFilterGridInfo.DataSource = this.radGridViewCodIva.DataSource;
                    this.RadGridViewHeader();

                    this.radGridViewCodIva.Visible = false;
                    this.radLabelNoHayInfo.Visible = false;

                    for (int i = 0; i < this.radGridViewCodIva.Columns.Count; i++)
                    {
                        this.radGridViewCodIva.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewCodIva.Columns[i].Width = 600;
                    }

                    this.radGridViewCodIva.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewCodIva.AllowSearchRow = true;
                    this.radGridViewCodIva.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewCodIva.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewCodIva.AllowEditRow = false;
                    this.radGridViewCodIva.EnableFiltering = true;

                    this.radGridViewCodIva.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewCodIva.Rows[0].IsCurrent = true;
                    this.radGridViewCodIva.Focus();
                    this.radGridViewCodIva.Select();

                    this.radGridViewCodIva.Refresh();

                    this.radGridViewCodIva.Visible = true;
                }
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCodIva.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCodIva.Columns.Contains(item.Key)) this.radGridViewCodIva.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

    }
}
