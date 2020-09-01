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
    public partial class frmMtoGLT04Sel : frmPlantilla, IReLocalizable //, IFormGLT04Sel
    {
        private string codCalendario = "";

        private Dictionary<string, string> displayNames;

        public frmMtoGLT04Sel()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLT04Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Calendarios Contables Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Llenar el TGGrid
            this.FillDataGrid();

            this.radGridViewCalendarios.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            //this.radGridViewCalendarios.MultiSelect = false;
            this.radGridViewCalendarios.AllowSearchRow = true;
            this.radGridViewCalendarios.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewCalendarios.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewCalendarios.AllowEditRow = false;
            this.radGridViewCalendarios.EnableFiltering = true;
        }

        /*
        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta o una actualización de un elementos
        void IFormGLT04Sel.ActualizaListaElementos()
        {
            try
            {
                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        */

        private void TgGridCalendario_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarCalendario();
        }

        private void RadGridViewCalendarios_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarCalendario();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoCalendario();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarCalendario();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarCalendario();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            //this.Exportar();
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

        private void FrmMtoGLT04Sel_Shown(object sender, EventArgs e)
        {
            this.radGridViewCalendarios.Focus();
        }

        private void FrmMtoGLT04Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Calendarios Contables Gestión");
        }

        private void radGridViewCalendarios_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        private void radGridViewCalendarios_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewCalendarios, ref this.selectAll);
        }
        private void radGridViewCalendarios_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (this.radGridViewCalendarios.Rows.IndexOf(this.radGridViewCalendarios.CurrentRow) >= 0)
                {
                    this.EditarCalendario();
                }
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoGLT04SelTitulo", "Mantenimiento de Calendarios Contables");   //Falta traducir

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
        }
        
        /// <summary>
        /// Carga los datos de las zonas en la grid
        /// </summary>
        private void FillDataGrid()
        {
            this.radGridViewCalendarios.Visible = false;

            try
            {
                string query = "select distinct(TITAFL) TITAFL, CCIAMG from ";
                query += GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "LEFT OUTER JOIN " + GlobalVar.PrefijoTablaCG + "GLM01 ON TITAMG = TITAFL ";
                query += "order by TITAFL, CCIAMG";

                DataTable dtCalendarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                string calendario = "";
                string calendarioActual = "";
                string listaCompania = "";
                DataRow row;

                //Cambiar la columna APNAAH de tipo
                DataTable dtCloned = dtCalendarios.Clone();

                for (int i = 0; i < dtCalendarios.Rows.Count; i++)
                {
                    calendarioActual = dtCalendarios.Rows[i]["TITAFL"].ToString();

                    if (calendario == calendarioActual || i == 0)
                    {
                        if (listaCompania != "") listaCompania += "  ";
                        listaCompania += dtCalendarios.Rows[i]["CCIAMG"].ToString();
                        calendario = calendarioActual;
                    }
                    else
                    {
                        row = dtCloned.NewRow();

                        row["TITAFL"] = calendario;
                        row["CCIAMG"] = listaCompania;

                        dtCloned.Rows.Add(row);

                        listaCompania = dtCalendarios.Rows[i]["CCIAMG"].ToString();
                        calendario = calendarioActual;
                    }

                    //Escribir el ultimo registro
                    if ((i + 1) == dtCalendarios.Rows.Count)
                    {
                        row = dtCloned.NewRow();

                        row["TITAFL"] = calendarioActual;
                        row["CCIAMG"] = listaCompania;

                        dtCloned.Rows.Add(row);
                    }
                }

                this.radGridViewCalendarios.DataSource = dtCloned;
                this.RadGridViewHeader();
                
                if (this.radGridViewCalendarios.Rows != null && this.radGridViewCalendarios.Rows.Count > 0)
                {
                    //this.tgBuscadorTiposComp.Enabled = true;
                    //this.tgTiposComp.Visible = true;

                    for (int p = 0; p < this.radGridViewCalendarios.Columns.Count; p++)
                    {
                        this.radGridViewCalendarios.Columns[p].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewCalendarios.Columns[p].Width = 600;
                    }

                    this.radGridViewCalendarios.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewCalendarios.AllowSearchRow = true;
                    this.radGridViewCalendarios.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewCalendarios.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewCalendarios.AllowEditRow = false;
                    this.radGridViewCalendarios.EnableFiltering = true;

                    this.radGridViewCalendarios.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewCalendarios.Rows[0].IsCurrent = true;
                    this.radGridViewCalendarios.Focus();
                    this.radGridViewCalendarios.Select();

                    this.radGridViewCalendarios.Refresh();

                    this.radGridViewCalendarios.Visible = true;
                }
                else
                {
                }
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llamada a Editar una zona
        /// </summary>
        private void EditarCalendario()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewCalendarios.CurrentRow is GridViewGroupRowInfo)
                {
                    if (this.radGridViewCalendarios.CurrentRow.IsExpanded) this.radGridViewCalendarios.CurrentRow.IsExpanded = false;
                    else this.radGridViewCalendarios.CurrentRow.IsExpanded = true;
                }
                else if (this.radGridViewCalendarios.CurrentRow is GridViewDataRowInfo)
                {
                    //int indice = this.radGridViewCalendarios.CurrentRow.Index;
                    int indice = this.radGridViewCalendarios.Rows.IndexOf(this.radGridViewCalendarios.CurrentRow);
                    this.codCalendario = this.radGridViewCalendarios.Rows[indice].Cells["TITAFL"].Value.ToString();

                    frmMtoGLT04 frmEditGLT04 = new frmMtoGLT04
                    {
                        Nuevo = false,
                        Codigo = this.codCalendario,
                        FrmPadre = this
                    };
                    frmEditGLT04.Show(this);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Nuevo Calendario
        /// </summary>
        private void NuevoCalendario()
        {
            frmMtoGLT04 frmEditGLT04 = new frmMtoGLT04
            {
                Nuevo = true,
                Codigo = "",
                FrmPadre = this
            };
            frmEditGLT04.Show(this);
        }

        /// <summary>
        /// Llamada a Eliminar un Calendario
        /// </summary>
        private void EliminarCalendario()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Pedir confirmación y eliminar el cambio seleccionado
                string mensaje = "Se va a eliminar el calendario seleccionado. ";  //Falta traducir
                mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Telerik.WinControls.UI.GridDataCellElement celdaActiva = this.radGridViewCalendarios.CurrentCell;
                    string codCalendario = this.radGridViewCalendarios.Rows[celdaActiva.RowIndex].Cells["TITAFL"].Value.ToString();
                    string companias = this.radGridViewCalendarios.Rows[celdaActiva.RowIndex].Cells["CCIAMG"].Value.ToString();

                    if (companias != "")
                    {
                        RadMessageBox.Show("Existen compañías asociadas al calendario indicado. No es posible eliminar el calendario.", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
                        return;
                    }
                    else
                    {
                        //Eliminar todos los periodos del calendario
                        string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT04 where ";
                        query += " TITAFL = '" + codCalendario + "'";

                        int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (registros > 0)
                        {
                            /*
                            IFormGLT04Sel formInterface = this;

                            if (formInterface != null)
                                formInterface.ActualizaListaElementos();
                                */
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                RadMessageBox.Show("Error eliminado el calendario (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Verificar que el calendario no se este utilizando en ningún apunte contable introducido
        /// </summary>
        /// <returns></returns>
        private bool ExisteApunteContable(string titafl)
        {
            bool result = true;

            try
            {
                //Busca todas las compañías que tengan el calendario seleccionado en la tabla (GLM01) y 
                //Para esas compañías, buscar en la tabla de comprobantes contables (GLB01) que no exista el saprfl
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where CCIADT in (";
                query += "select distinct CCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where TITAMG = '" + titafl + "') ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros <= 0) result = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCalendarios.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCalendarios.Columns.Contains(item.Key)) this.radGridViewCalendarios.Columns[item.Key].HeaderText = item.Value;
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
                
                campo = "TITAFL";
                header = this.LP.GetText("lblListaCampoCalendario", "Calendario");
                this.displayNames.Add(campo, header);

                campo = "CCIAMG";
                header = this.LP.GetText("lblListaCampoCompania", "Compañía");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        #endregion

    }
}
