
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

namespace ModMantenimientos
{
    public partial class frmMtoATM05Sel: frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "005";
        private string autGrupo = "";
        private string autOperModifica = "";
        //private string autOperAlta = "";

        private string codUsuario = "";

        //private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 42);

        Dictionary<string, string> displayNames;
        DataTable dtUsuarios = new DataTable();

        public frmMtoATM05Sel()
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

        private void FrmMtoATM05Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Usuarios");
            
            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            this.radGridViewUsuarios.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewUsuarios.AllowSearchRow = true;
            this.radGridViewUsuarios.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewUsuarios.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewUsuarios.AllowEditRow = false;
            this.radGridViewUsuarios.EnableFiltering = true;
        }

        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Eliminar las filas del DataTable
                this.dtUsuarios.Clear();

                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid();

                if (e != null && this.radGridViewUsuarios != null && this.radGridViewUsuarios.Rows != null && this.radGridViewUsuarios.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewUsuarios.Rows.Count; i++)
                            {
                                if (this.radGridViewUsuarios.Rows[i].Cells["IDUSMO"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewUsuarios.Rows[i].IsCurrent = true;
                                    this.radGridViewUsuarios.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewUsuarios.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewUsuarios.Focus();
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoUsuario();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarUsuario();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, Telerik.WinControls.UI.TreeNodeEditorInitializedEventArgs e)
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

        private void RadDataFilterGridInfo_NodeFormatting(object sender, Telerik.WinControls.UI.TreeNodeFormattingEventArgs e)
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

        private void RadGridViewUsuarios_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarUsuario();
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

        private void RadGridViewUsuarios_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewUsuarios, ref this.selectAll);
        }

        private void FrmMtoATM05Sel_Shown(object sender, EventArgs e)
        {
            this.radGridViewUsuarios.Focus();
        }

        private void FrmMtoATM05Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Usuarios");
        }

        private void radGridViewUsuarios_ViewCellFormatting(object sender, CellFormattingEventArgs e)
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
            this.Text = this.LP.GetText("lblfrmMtoATM05Sel", "Mantenimiento de Usuarios");   //Falta traducir

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
        }
        
        /// <summary>
        /// Devuelve el literal para el campo Administrador CG
        /// </summary>
        /// <returns></returns>
        private string ValorCampoUADMMOIdioma(string valorCampoUADMMO)
        {
            string result = "";
            switch (valorCampoUADMMO)
            {
                case "0":
                    result = this.LP.GetText("lblATM050No", "No");     //Falta traducir
                    break;
                case "1":
                    result = this.LP.GetText("lblATM051Seg", "Seguridad");     //Falta traducir
                    break;
                case "2":
                    result = this.LP.GetText("lblATM051Sistemas", "Sistemas");     //Falta traducir
                    break;
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
        /// Llamada a Editar un usuario
        /// </summary>
        private void EditarUsuario()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewUsuarios.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewUsuarios.CurrentRow.IsExpanded) this.radGridViewUsuarios.CurrentRow.IsExpanded = false;
                else this.radGridViewUsuarios.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewUsuarios.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewUsuarios.CurrentRow.Index;
                int indice = this.radGridViewUsuarios.Rows.IndexOf(this.radGridViewUsuarios.CurrentRow);
                this.codUsuario = this.radGridViewUsuarios.Rows[indice].Cells["IDUSMO"].Value.ToString();
                /*string nombre = this.radGridViewUsuarios.Rows[indice].Cells["NOMBMO"].Value.ToString();

                string numPwdUnicas = this.radGridViewUsuarios.Rows[indice].Cells["NCONMO"].Value.ToString();
                string diasValidezPwd = this.radGridViewUsuarios.Rows[indice].Cells["DVALMO"].Value.ToString();
                string tiempoConexMin = this.radGridViewUsuarios.Rows[indice].Cells["TCONMO"].Value.ToString();
                string maxDiasInactividad = this.radGridViewUsuarios.Rows[indice].Cells["MDIAMO"].Value.ToString();
                string UADMMOValor = this.radGridViewUsuarios.Rows[indice].Cells["APNAAH"].Value.ToString();

                string UADMMO0 = this.LP.GetText("lblATM050No", "No");
                string UADMMO1 = this.LP.GetText("lblATM051Seg", "Seguridad");
                string UADMMO2 = this.LP.GetText("lblATM051Sistemas", "Sistemas");

                string posUADMMO = "0";
                if (UADMMOValor == UADMMO1) posUADMMO = "1";
                else if (UADMMOValor == UADMMO2) posUADMMO = "2";

                string estado = this.radGridViewUsuarios.Rows[indice].Cells["STATMO"].Value.ToString();

                if (estado == this.estadoInactivo) estado = "*";
                else estado = "V";
                */
                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codUsuario, this.autOperModifica);

                if (operarModificar)
                {
                    frmMtoATM05 frmMtoUser = new frmMtoATM05();
                    frmMtoUser.Nuevo = false;
                    frmMtoUser.Codigo = this.codUsuario;
                    /*frmMtoUser.Nombre = nombre;
                    frmMtoUser.Estado = estado;
                    frmMtoUser.PosTipoAux = posTipoAux;*/
                    frmMtoUser.FrmPadre = this;
                    frmMtoUser.Show(this);
                    frmMtoUser.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                }
                else
                {
                    MessageBox.Show(this.LP.GetText("lblErrUserNoAut", "Usuario no autorizado a este elemento"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                }
            }
          
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a crear nuevo usuario
        /// </summary>
        private void NuevoUsuario()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Comprobar sobre ese nuevo tipo de extracontable si tiene autorización a dar de alta

            string error = this.LP.GetText("errValTitulo", "Error");
            
            frmMtoATM05 frmMtoUser = new frmMtoATM05();
            frmMtoUser.Nuevo = true;
            frmMtoUser.FrmPadre = this;
            frmMtoUser.Show(this);
            frmMtoUser.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };
            
            Cursor.Current = Cursors.Default;
        }
        
        /// <summary>
        /// Carga los datos de las TiposExt en la grid
        /// </summary>
        private void FillDataGrid()
        {
            this.radGridViewUsuarios.Visible = false;
            try
            {
                string query = "select STATMO, IDUSMO, NOMBMO, NCONMO, DVALMO, TCONMO, MDIAMO, UADMMO from ";
                query +=  GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "order by IDUSMO";

                this.radGridViewUsuarios.DataSource = null;

                dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Cambiar la columna APNAAH de tipo
                DataTable dtCloned = dtUsuarios.Clone();
                //dtCloned.Columns["APNAAH"].DataType = typeof(String);
                foreach (DataRow row in dtUsuarios.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                string valorCampoEstado = "";
                string valorCampoUADMMO = "";
                int cont = 0;
                foreach (DataRow dr in dtCloned.Rows)
                {
                    valorCampoEstado = dr["STATMO"].ToString();
                    valorCampoUADMMO = dr["UADMMO"].ToString();

                    if (valorCampoEstado.Trim() == "*") valorCampoEstado = this.estadoInactivo;
                    else valorCampoEstado = this.estadoActivo;

                    valorCampoUADMMO = this.ValorCampoUADMMOIdioma(valorCampoUADMMO);

                    dtCloned.Rows[cont]["STATMO"] = valorCampoEstado;
                    dtCloned.Rows[cont]["UADMMO"] = valorCampoUADMMO;

                    cont++;
                }

                this.radGridViewUsuarios.DataSource = dtCloned;                
                this.RadGridViewHeader();

                if (this.radGridViewUsuarios.Rows != null && this.radGridViewUsuarios.Rows.Count > 0)
                {
                    for (int i = 0; i < this.radGridViewUsuarios.Columns.Count; i++)
                    {
                        this.radGridViewUsuarios.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewUsuarios.Columns[i].Width = 600;
                    }

                    this.radGridViewUsuarios.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewUsuarios.AllowSearchRow = true;
                    this.radGridViewUsuarios.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewUsuarios.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewUsuarios.AllowEditRow = false;
                    this.radGridViewUsuarios.EnableFiltering = true;

                    this.radGridViewUsuarios.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewUsuarios.Rows[0].IsCurrent = true;
                    this.radGridViewUsuarios.Focus();
                    this.radGridViewUsuarios.Select();

                    this.radGridViewUsuarios.Refresh();

                    this.radGridViewUsuarios.Visible = true;

                    //Habilitar/Deshabilitar botones según autorizaciones
                    this.VerificarAutorizaciones();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewUsuarios.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewUsuarios.Columns.Contains(item.Key)) this.radGridViewUsuarios.Columns[item.Key].HeaderText = item.Value;
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

                campo = "STATMO";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);

                campo = "IDUSMO";
                header = this.LP.GetText("lblListaIdUserCG", "Id. Usuario CG");
                this.displayNames.Add(campo, header);

                campo = "NOMBMO";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "NCONMO";
                header = this.LP.GetText("lblListaNumPwdUnicos", "Núm.contraseñas únicas");
                this.displayNames.Add(campo, header);

                campo = "DVALMO";
                header = this.LP.GetText("lblListaDiasValidosPwd", "Días válidos contraseña");
                this.displayNames.Add(campo, header);

                campo = "TCONMO";
                header = this.LP.GetText("lblListaTiempoConexMin", "Tiempo de conexión en minutos");
                this.displayNames.Add(campo, header);

                campo = "MDIAMO";
                header = this.LP.GetText("lblListaNumDiasInactividad", "Número de días de inactividad");
                this.displayNames.Add(campo, header);

                campo = "UADMMO";
                header = this.LP.GetText("lblListaAdminCG", "Administrador CG");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewUsuarios, "Mantenimiento de Usuarios");
        }
        #endregion

    }
}