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

namespace ModComprobantes
{
    public partial class frmCompContRecepcionLotes :  frmPlantilla, IReLocalizable
    {
        Dictionary<string, string> displayNames;
        DataTable dtLotes = new DataTable();

        private Dictionary<string, string> literalesDict;

        public frmCompContRecepcionLotes()
        {
            InitializeComponent();

            this.radGridViewLotes.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContRecepcionLotes_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Recepción de Lotes");

            //Obtener Literales para la Recepción de Lotes
            this.literalesDict = this.ObtenerValoresFLMITXRecepcionLotes();

            this.BuildDisplayNamesLotes();

            //Crear el DataTable que almacenara los lotes recepcionados
            this.BuildDataGridtgGridEditarLotes();

            //Cargar los datos de la Grid
            this.FillDataGrid();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoLote();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.EliminarLote();
        }

        private void RadButtonActualizar_Click(object sender, EventArgs e)
        {
            this.FillDataGrid();
        }

        private void RadButtonNuevo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNuevo);
        }

        private void RadButtonNuevo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNuevo);
        }

        private void RadButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void RadButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
        }

        private void RadButtonActualizar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonActualizar);
        }

        private void RadButtonActualizar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonActualizar);
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

        private void RadGridViewLotes_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewLotes.Columns.Contains("APROC4")) this.radGridViewLotes.Columns["APROC4"].IsVisible = false;
            if (this.radGridViewLotes.Columns.Contains("STATC4")) this.radGridViewLotes.Columns["STATC4"].IsVisible = false;
            if (this.radGridViewLotes.Columns.Contains("NUM")) this.radGridViewLotes.Columns["NUM"].IsVisible = false;
        }

        private void FrmCompContRecepcionLotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Recepción de Lotes");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            //this.Text = this.LP.GetText("subMenuItemSolicitarGrupoInformes", "Solicitar Grupo de Informes");
        }

        private void BuildDisplayNamesLotes()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>
                {
                    { "PREFC4", "Codigo prefijo lote" },
                    { "LIBLC4", "Biblioteca" },
                    { "DESCC4", "Descripción" },
                    { "USERC4", "Usuario" },
                    { "APROC4_DESC", "Adicionar" },
                    { "STATC4_DESC", "Estado" },
                    { "LOTE_AMP", "Lote Ampliado" },
                    { "APROC4", "AdicionarCod" },
                    { "STATC4", "EstadoCod" },
                    { "NUM", "Num. Registro" }
                };
            }
            catch {}
        }

        /// <summary>
        /// Carga los datos de la recepcion de lotes
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                if (this.dtLotes.Rows != null && this.dtLotes.Rows.Count > 0) this.dtLotes.Rows.Clear();

                string selectIni = "";
                string selectOrder = "";

                switch (GlobalVar.ConexionCG.TipoBaseDatos)
                {
                    case ProveedorDatos.DBTipos.DB2:
                        selectIni = "select PREFC4, LIBLC4, DESCC4, USERC4, APROC4, STATC4, RRN(GLC04) NUM from ";
                        selectOrder = "order by NUM";
                        break;
                    case ProveedorDatos.DBTipos.SQLServer:
                        selectIni = "select PREFC4, LIBLC4, DESCC4, USERC4, APROC4, STATC4, GERIDENTI as NUM from ";
                        selectOrder = "order by GERIDENTI";
                        break;
                    case ProveedorDatos.DBTipos.Oracle:
                        selectIni = "select PREFC4, LIBLC4, DESCC4, USERC4, APROC4, STATC4, ID_TIGSA_GLC04 as NUM from ";
                        selectOrder = "order by ID_TIGSA_GLC04";
                        break;
                }

                string query = selectIni;
                query += GlobalVar.PrefijoTablaCG + "GLC04 ";
                query += selectOrder;

                DataTable dtAux = null;
                dtAux  = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtAux != null && dtAux.Rows != null && dtAux.Rows.Count > 0)
                {
                    DataRow row;
                    string estadoAct = "";
                    string descEstadoAct = "";
                    string adicionarAct = "";
                    string adicionaDescAct = "";
                    string loteAmpliado = "";

                    for (int i = 0; i < dtAux.Rows.Count; i++)
                    {
                        row = this.dtLotes.NewRow();

                        row["PREFC4"] = dtAux.Rows[i]["PREFC4"].ToString();
                        row["LIBLC4"] = dtAux.Rows[i]["LIBLC4"].ToString();
                        row["DESCC4"] = dtAux.Rows[i]["DESCC4"].ToString();
                        row["USERC4"] = dtAux.Rows[i]["USERC4"].ToString();
                        row["PREFC4"] = dtAux.Rows[i]["PREFC4"].ToString();

                        adicionarAct = dtAux.Rows[i]["APROC4"].ToString().Trim();
                        adicionaDescAct = this.ObtenerDescAdicionarActual(adicionarAct);
                        row["APROC4_DESC"] = adicionaDescAct;

                        estadoAct = dtAux.Rows[i]["STATC4"].ToString().Trim();
                        descEstadoAct = this.ObtenerDescEstadoActual(estadoAct);
                        row["STATC4_DESC"] = descEstadoAct;

                        loteAmpliado = "No";
                        if (adicionarAct == "s" || adicionarAct == "n" || adicionarAct == "c") loteAmpliado = "Si";
                        row["LOTE_AMP"] = loteAmpliado;

                        row["APROC4"] = adicionarAct;
                        row["STATC4"] = estadoAct;

                        row["NUM"] = dtAux.Rows[i]["NUM"].ToString(); ;

                        this.dtLotes.Rows.Add(row);
                    }

                    this.radGridViewLotes.DataSource = this.dtLotes;
                    this.radGridViewLotes.Visible = true;
                    this.radLabelNoHayInfo.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonEliminar, true);

                    for (int i = 0; i < this.radGridViewLotes.Columns.Count; i++)
                    {
                        this.radGridViewLotes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    }

                    this.radGridViewLotes.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewLotes.MasterTemplate.BestFitColumns();
                }
                else
                {
                    this.radGridViewLotes.Visible = false;
                    this.radLabelNoHayInfo.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve la descripcióm para el código de adicionar solicitado
        /// </summary>
        /// <param name="adicionarAct"></param>
        /// <returns></returns>
        private string ObtenerDescAdicionarActual(string adicionarAct)
        {
            string descAdicionarActual = "";
            string aux = "";

            try
            {
                switch (adicionarAct)
                {
                    case "S":
                    case "s":
                        descAdicionarActual = "Aprobado";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0751").Trim();
                        if (aux != "") descAdicionarActual = aux;
                        break;
                    case "N":
                    case "n":
                        descAdicionarActual = "No aprobado";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0752").Trim();
                        if (aux != "") descAdicionarActual = aux;
                        break;
                    case "C":
                    case "c":
                        descAdicionarActual = "Contabilizado";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0753").Trim();
                        if (aux != "") descAdicionarActual = aux;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (descAdicionarActual);
        }

        /// <summary>
        /// Devuelve la descripcióm para el código de estado solicitado
        /// </summary>
        /// <param name="estadoAct"></param>
        /// <returns></returns>
        private string ObtenerDescEstadoActual(string estadoAct)
        {
            string descEstadoActual = "";
            string aux = "";

            try
            {
                switch (estadoAct)
                {
                    case "1":
                        descEstadoActual = "En control previo a validar";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STCON").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                    case "2":
                        descEstadoActual = "En validación";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STVAL").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                    case "3":
                        descEstadoActual = "En extracción de comprobantes erróneos";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STERR").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                    case "4":
                        descEstadoActual = "En adición";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STADD").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                    case "5":
                        descEstadoActual = "Está finalizando el proceso";
                         aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STCOM").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                    default:  //Blanco
                        descEstadoActual = "En espera de proceso";
                        aux = utiles.FindFirstValueByKey(ref this.literalesDict, "L$STESP").Trim();
                        if (aux != "") descEstadoActual = aux;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (descEstadoActual);
        }

        private void RadGridViewLotesHeader()
        {
            try
            {
                if (this.radGridViewLotes.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewLotes.Columns.Contains(item.Key)) this.radGridViewLotes.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch {}
        }

        /// <summary>
        /// Crea la Grid para la edición de lotes
        /// </summary>
        private void BuildDataGridtgGridEditarLotes()
        {
            try
            {
                this.dtLotes.TableName = "Tabla";

                //Adicionar las columnas al DataTable
                this.dtLotes.Columns.Add("PREFC4", typeof(string));
                this.dtLotes.Columns.Add("LIBLC4", typeof(string));
                this.dtLotes.Columns.Add("DESCC4", typeof(string));
                this.dtLotes.Columns.Add("USERC4", typeof(string));
                this.dtLotes.Columns.Add("APROC4_DESC", typeof(string));
                this.dtLotes.Columns.Add("STATC4_DESC", typeof(string));
                this.dtLotes.Columns.Add("LOTE_AMP", typeof(string));
                this.dtLotes.Columns.Add("APROC4", typeof(string));
                this.dtLotes.Columns.Add("STATC4", typeof(string));
                this.dtLotes.Columns.Add("NUM", typeof(string));

                this.radGridViewLotes.DataSource = this.dtLotes;
                //Escribe el encabezado de la Grid de EditarLotes
                this.BuildDisplayNamesLotes();
                this.RadGridViewLotesHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina un lote de la tabla de recepcion de lotes
        /// </summary>
        private void EliminarLote()
        {
            Cursor.Current = Cursors.WaitCursor;
            IDataReader dr = null;
            try
            {
                if (this.radGridViewLotes.CurrentRow is GridViewGroupRowInfo)
                {
                    if (this.radGridViewLotes.CurrentRow.IsExpanded) this.radGridViewLotes.CurrentRow.IsExpanded = false;
                    else this.radGridViewLotes.CurrentRow.IsExpanded = true;
                }
                else if (this.radGridViewLotes.CurrentRow is GridViewDataRowInfo)
                {
                    string mensaje = "Se va a eliminar el lote seleccionado ";  //Falta traducir
                    mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        int indice = this.radGridViewLotes.Rows.IndexOf(this.radGridViewLotes.CurrentRow);
                        string prefijo = this.radGridViewLotes.Rows[indice].Cells["PREFC4"].Value.ToString();
                        string libreria = this.radGridViewLotes.Rows[indice].Cells["LIBLC4"].Value.ToString();

                        //Comprobar existencia en GLC04 y solo si STATC4=' '
                        string mensajeError = "";
                        string query = "select * from ";
                        query += GlobalVar.PrefijoTablaCG + "GLC04 ";
                        query += "where PREFC4 = '" + prefijo + "' and LIBLC4 = '" + libreria + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            string estado = dr.GetValue(dr.GetOrdinal("STATC4")).ToString().Trim();

                            if (estado != "") mensajeError = "El lote no se puede eliminar, su estado es: " + this.ObtenerDescEstadoActual(estado);
                        }
                        else mensajeError = "El lote ya no existe";
                        dr.Close();

                        if (mensajeError == "")
                        {
                            query = "delete from ";
                            query += GlobalVar.PrefijoTablaCG + "GLC04 ";
                            query += "where PREFC4 = '" + prefijo + "' and LIBLC4 = '" + libreria + "'";

                            int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                            if (cantRegistros > 0)
                            {
                                //Refrescar la GRID
                                //this.dtLotes.
                                GridViewRowInfo row = this.radGridViewLotes.CurrentRow;
                                this.radGridViewLotes.Rows.Remove(row);
                                this.dtLotes.AcceptChanges();       //OJO !!!!!! no se si funcionara
                            }
                        }
                        else
                        {
                            RadMessageBox.Show(mensajeError, "Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            Cursor.Current = Cursors.Default;
        }
        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta/modificar/eliminar un elementos
        public void ActualizaListaElementos(UpdateDataFormEventArgs e)
        {
            try
            {
                //Volver a cargar los valores de los lotes
                this.FillDataGrid();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Inserta un nuevo registro en la tabla de recepcion de lotes
        /// </summary>
        private void NuevoLote()
        {
            try
            {
                frmCompContRecepcionLotesNuevo frmNuevo = new frmCompContRecepcionLotesNuevo
                {
                    LiteralesDict = this.literalesDict
                };
                frmNuevo.Show();
                frmNuevo.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}