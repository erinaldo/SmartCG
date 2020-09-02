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

namespace ModConsultaInforme
{
    public partial class frmInfoSolicitudInforme :  frmPlantilla, IReLocalizable
    {
        Dictionary<string, string> displayNames;
        DataTable dtSolicitudes = new DataTable();

        public frmInfoSolicitudInforme()
        {
            InitializeComponent();

            this.radGridViewSolicitudes.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoSolicitudInforme_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Solicitar Grupo de Informes");

            this.BuildDisplayNames();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            //cargar layout
            utiles.cargarLayout(this.Name, ref radGridViewSolicitudes);
        }

        private void RadButtonListaInformes_Click(object sender, EventArgs e)
        {
            this.SolicitarInfLista();
        }

        private void RadButtonListaInformes_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonListaInformes);
        }

        private void RadButtonListaInformes_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonListaInformes);
        }

        private void RadGridViewSolicitudes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.SolicitarInfLista();
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, TreeNodeEditorInitializedEventArgs e)
        {
            DataFilterCriteriaElement criteriaElement = e.NodeElement as Telerik.WinControls.UI.DataFilterCriteriaElement;

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

        private void RadDataFilterGridInfo_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
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

        private void FrmInfoSolicitudInforme_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Solicitar Grupo de Informes");
        }

        private void radGridViewSolicitudes_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
                //185; 219; 245
                //e.CellElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(219)))), ((int)(((byte)(245)))));
            }
        }

        private void radGridViewSolicitudes_Leave(object sender, EventArgs e)
        {
            utiles.guardarLayout(this.Name, ref radGridViewSolicitudes);
        }

        private void radGridViewSolicitudes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (this.radGridViewSolicitudes.Rows.IndexOf(this.radGridViewSolicitudes.CurrentRow) >= 0)
                {
                    this.SolicitarInfLista();
                }
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            //this.Text = this.LP.GetText("subMenuItemSolicitarGrupoInformes", "Solicitar Grupo de Informes");

            this.radButtonListaInformes.Text = this.LP.GetText("lblListaInformes", "Lista de informes");   //Falta traducir
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>
                {
                    { "GRUPP4", this.LP.GetText("lblListaCampoGrupo", "Grupo") },
                    { "NOMBP4", this.LP.GetText("lblListaCampoDesc", "Descripción") }
                };
            }
            catch {}
        }

        /// <summary>
        /// Carga los datos de las Solicitudes en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                if (this.dtSolicitudes != null && this.dtSolicitudes.Rows != null && this.dtSolicitudes.Rows.Count > 0) this.dtSolicitudes.Clear();

                string query = "select GRUPP4, NOMBP4 from " + GlobalVar.PrefijoTablaCG + "PRT04 ";
                query += "order by GRUPP4";

                this.dtSolicitudes = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (this.dtSolicitudes != null && this.dtSolicitudes.Rows != null && this.dtSolicitudes.Rows.Count > 0)
                {
                    this.radGridViewSolicitudes.DataSource = this.dtSolicitudes;
                    this.RadGridViewHeader();
                    this.radLabelNoHayInfo.Visible = false;
                    //this.radGridViewSolicitudes.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonListaInformes, true);

                    for (int i = 0; i < this.radGridViewSolicitudes.Columns.Count; i++)
                    {
                        this.radGridViewSolicitudes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewSolicitudes.Columns[i].Width = 600;
                    }

                    this.radGridViewSolicitudes.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewSolicitudes.AllowSearchRow = true;
                    this.radGridViewSolicitudes.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewSolicitudes.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewSolicitudes.AllowEditRow = false;
                    this.radGridViewSolicitudes.EnableFiltering = true;

                    this.radGridViewSolicitudes.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    if (this.radGridViewSolicitudes.Groups.Count == 0) this.radGridViewSolicitudes.Rows[0].IsCurrent = true;
                    this.radGridViewSolicitudes.Focus();
                    this.radGridViewSolicitudes.Select();

                    this.radGridViewSolicitudes.Refresh();

                    //this.radGridViewSolicitudes.Visible = true;
                }
                else
                {
                    this.radGridViewSolicitudes.Visible = false;
                    this.radLabelNoHayInfo.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonListaInformes, false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewSolicitudes.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewSolicitudes.Columns.Contains(item.Key)) this.radGridViewSolicitudes.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch {}
        }

        /// <summary>
        /// Verifica que el usuario logado tenga permisos para operar con el grupo de solicitud de informe
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        private bool UsuarioAutorizado(string grupo)
        {
            bool operarConsulta = false;

            try
            {
                string autClaseElemento = "014";
                string autGrupo = "01";
                string autOperConsulta = "10";

                operarConsulta = aut.Validar(autClaseElemento, autGrupo, grupo, autOperConsulta);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (operarConsulta);
        }

        /// <summary>
        /// Carga el formulario para solicitar la lista de informes
        /// </summary>
        private void SolicitarInfLista()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewSolicitudes.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewSolicitudes.CurrentRow.IsExpanded) this.radGridViewSolicitudes.CurrentRow.IsExpanded = false;
                else this.radGridViewSolicitudes.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewSolicitudes.CurrentRow is GridViewDataRowInfo)
            {
                int indice = this.radGridViewSolicitudes.Rows.IndexOf(this.radGridViewSolicitudes.CurrentRow);
                string codGrupo = this.radGridViewSolicitudes.Rows[indice].Cells["GRUPP4"].Value.ToString();
                string grupoDesc = this.radGridViewSolicitudes.Rows[indice].Cells["NOMBP4"].Value.ToString();

                frmInfoSolicitudInfLista frmInfoSolInfLista = new frmInfoSolicitudInfLista
                {
                    CodGrupoInf = codGrupo,
                    GrupoInfDesc = grupoDesc,
                    FrmPadre = this
                };
                frmInfoSolInfLista.Show(this);
            }
            Cursor.Current = Cursors.Default;
        }

        #endregion

    }
}
