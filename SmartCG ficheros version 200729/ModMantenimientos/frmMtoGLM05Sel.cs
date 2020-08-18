using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM05Sel : frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "006";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        private string autOperAlta = "10";
        private bool userAutEditar = false;

        private string codTipoAux = "";
        private string codCtaAux = "";

        private string proveedorTipo;

        //private bool campoEstadoAccesible;

        //private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 112);

        Dictionary<string, string> displayNames;
        DataTable dtCuentasAux = new DataTable();

        public frmMtoGLM05Sel()
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
                this.dtCuentasAux.Clear();

                this.TipoAuxBuscador();

                if (e != null && this.radGridViewCuentasAux != null && this.radGridViewCuentasAux.Rows != null && this.radGridViewCuentasAux.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewCuentasAux.Rows.Count; i++)
                            {
                                if (this.radGridViewCuentasAux.Rows[i].Cells["CAUXMA"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewCuentasAux.Rows[i].IsCurrent = true;
                                    this.radGridViewCuentasAux.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewCuentasAux.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewCuentasAux.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewCuentasAux.Rows[0].IsCurrent = true;
                            this.radGridViewCuentasAux.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewCuentasAux.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewCuentasAux.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FrmMtoGLM05Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Auxiliar Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            this.radGridViewCuentasAux.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewCuentasAux.AllowSearchRow = true;
            this.radGridViewCuentasAux.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewCuentasAux.EnableFiltering = true;

            this.radGridViewCuentasAux.Visible = false;

            utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonExport, false);

            this.proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            this.radGridViewCuentasAux.MultiSelect = false;
            this.radGridViewCuentasAux.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewCuentasAux.AllowEditRow = false;

            //if (this.radGridViewCuentasAux.Rows.Count > 0) this.radGridViewCuentasAux.Rows[0].IsCurrent = true;
            this.radButtonTextBoxTipoAux.Select();
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevaCuentaAux();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarCuentaAux();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadButtonTextBoxTipoAux_TextChanged(object sender, EventArgs e)
        {
            this.TipoAuxBuscador();
        }

        private void RadButtonElementTipoAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

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
                TituloForm = "Seleccionar tipo de auxiliar",
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
                this.radButtonTextBoxTipoAux.Text = result;
                this.ActiveControl = this.radButtonTextBoxTipoAux;
                this.radButtonTextBoxTipoAux.Select(0, 0);
                //this.radButtonTextBoxTipoAux.Focus();
                this.radGridViewCuentasAux.Focus();
            }
        }

        private void RadButtonTextBoxTipoAux_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                this.TipoAuxBuscador();
            //this.EditarCuentaAux();
        }

        private void RadGridViewCuentasAux_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarCuentaAux();
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

        private void RadGridViewCuentasAux_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewCuentasAux, ref this.selectAll);
        }

        private void FrmMtoGLM05Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Cuentas de Auxiliar Gestión");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoGLM05Titulo", "Mantenimiento de Cuentas de Auxiliar");   //Falta traducir
            this.radLabelTitulo.Text = "Mantenimientos / Tablas maestras / Cuentas de Auxiliar";

            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");

            //Traducir los campos del formulario
            this.lblTipoAux.Text = this.LP.GetText("lblGLM05TipoAux", "Tipo de Auxiliar");
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                campo = "CAUXMA";
                header = this.LP.GetText("lblListaCampoCodigoCtaAux", "Cta. Auxiliar");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[0].HeaderText = header;

                campo = "NOMBMA";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[1].HeaderText = header;

                campo = "PCIFMA";
                header = this.LP.GetText("lblListaCampoPais", "País");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[2].HeaderText = header;

                campo = "NNITMA";
                header = this.LP.GetText("lblListaCampoNIT", "NIT");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[3].HeaderText = header;

                campo = "ZONAMA";
                header = this.LP.GetText("lblListaCampoZona", "Zona");
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[4].HeaderText = header;
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
                bool operarCrear = aut.Validar(this.autClaseElemento, this.autGrupo, this.codTipoAux, this.autOperAlta);
                bool operarModificar = aut.Validar(this.autClaseElemento, this.autGrupo, this.codTipoAux, this.autOperModifica);

                //Botón Nuevo
                if (operarCrear) utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                else utiles.ButtonEnabled(ref this.radButtonNuevo, false);
              
                //Botón Editar y Botón Activar/Desactivar
                if (operarModificar)
                {
                    this.userAutEditar = true;
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonExport, true);

                    this.radLabelNoHayInfo.Text = "No existen cuentas de auxiliar";
                    this.radGridViewCuentasAux.Visible = true;
                }
                else
                {
                    this.userAutEditar = false;
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonExport, true);

                    this.radLabelNoHayInfo.Text = "Usuario no autorizado a este tipode auxiliar";
                    this.radGridViewCuentasAux.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida la existencia o no del tipo de auxiliar
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarTipoAux(string codigo)
        {
            string result = "";
            try
            {
                string query = "select count(TAUXMT) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Tipo de auxiliar no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmGLM05ValTipoAuxExcep", "Error al validar el tipo de auxiliar") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del tipo de auxiliar. Si existe devuelve el nombre (codigo - nombre)
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarTipoAuxNombre(string codigo, ref string nombre)
        {
            string result = "";
            nombre = "";

            IDataReader dr = null;
            try
            {
                //Validar q este activo .... FALTA !!!
                string query = "select TAUXMT, NOMBMT from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    nombre = dr.GetValue(dr.GetOrdinal("TAUXMT")).ToString() + " - " + dr.GetValue(dr.GetOrdinal("NOMBMT")).ToString();
                }

                dr.Close();

                if (nombre == "") result = "Tipo de auxiliar no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = this.LP.GetText("lblfrmGLM05ValTipoAuxExcep", "Error al validar el tipo de auxiliar") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Editar la cuenta de auxiliar
        /// </summary>
        private void EditarCuentaAux()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxTipoAux.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Tipo de Auxiliar no puede estar en blanco", error);
                this.radButtonTextBoxTipoAux.Focus();
                return;
            }

            if (this.radGridViewCuentasAux.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un registro", "Error");  //Falta traducir
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string nombre = "";
            string result = ValidarTipoAuxNombre(this.codTipoAux, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxTipoAux.Focus();
                return;
            }

            if (this.radGridViewCuentasAux.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewCuentasAux.CurrentRow.IsExpanded) this.radGridViewCuentasAux.CurrentRow.IsExpanded = false;
                else this.radGridViewCuentasAux.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewCuentasAux.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewCuentasAux.CurrentRow.Index;
                int indice = this.radGridViewCuentasAux.Rows.IndexOf(this.radGridViewCuentasAux.CurrentRow);
                this.codCtaAux = this.radGridViewCuentasAux.Rows[indice].Cells["CAUXMA"].Value.ToString();
                string nombreCtaAux = this.radGridViewCuentasAux.Rows[indice].Cells["NOMBMA"].Value.ToString();

                //bool operarConsulta = aut.Operar(this.autClaseElemento, this.autGrupo, this.autOperConsulta, codigoAut);
                //bool operarModificar = aut.Operar(this.autClaseElemento, this.autGrupo, this.autOperModifica, codigoAut);

                frmMtoGLM05 frmMtoCtasAux = new frmMtoGLM05
                {
                    Nuevo = false,
                    CodigoTipoAuxiliar = this.codTipoAux,
                    NombreTipoAuxiliar = nombre,
                    Codigo = this.codCtaAux,
                    NombreCuentaAuxiliar = nombreCtaAux,
                    TipoAuxExterno = this.lblExterno.Visible,
                    FrmPadre = this
                };
                frmMtoCtasAux.Show(this);
                frmMtoCtasAux.UpdateDataForm += (o, e) =>
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
            this.ExportarGrid(ref this.radGridViewCuentasAux, "Mantenimiento de Cuentas de Auxiliar");
        }

        /// <summary>
        /// Crear nueva cuenta de auxiliar
        /// </summary>
        private void NuevaCuentaAux()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Comprobar sobre ese tipo de auxiliar si tiene autorización a dar de alta

            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxTipoAux.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Tipo de Auxiliar no puede estar en blanco", error);
                this.radButtonTextBoxTipoAux.Focus();
                return;
            }

            string nombre = "";
            string result = ValidarTipoAuxNombre(this.codTipoAux, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxTipoAux.Focus();
                return;
            }

            frmMtoGLM05 frmMtoCtasAux = new frmMtoGLM05
            {
                Nuevo = true,
                CodigoTipoAuxiliar = this.codTipoAux,
                NombreTipoAuxiliar = nombre,
                TipoAuxExterno = this.lblExterno.Visible,
                FrmPadre = this
            };
            frmMtoCtasAux.Show(this);
            frmMtoCtasAux.UpdateDataForm += (o, e) =>
            {
                ActualizaListaElementos(e);
            };

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Verifica si el Tipo de auxiliar ex externo
        /// </summary>
        /// <returns></returns>
        private bool VerificarTipoAuxExterno()
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLC05 ";
                query += "where TAUXIN = '" + this.codTipoAux + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                string castin = "";
                if (dr.Read())
                {
                    castin = dr.GetValue(dr.GetOrdinal("CASTIN")).ToString().TrimEnd();
                }

                dr.Close();

                if (castin != "")
                {
                    //this.campoEstadoAccesible = false;
                    result = true;
                }
                else result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }

        #endregion

        
        private void TipoAuxBuscador()
        {
            string codigo = this.radButtonTextBoxTipoAux.Text.Trim();
            if (codigo != "")
            {
                //this.tgBuscadorCtasAux.ValorFiltro.Text = "";
                if (codigo.Length >= 2)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (codigo.Length <= 2) this.codTipoAux = this.radButtonTextBoxTipoAux.Text;
                    else this.codTipoAux = this.radButtonTextBoxTipoAux.Text.Substring(0, 2);

                    string result = ValidarTipoAux(this.codTipoAux);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radButtonTextBoxTipoAux.Focus();

                        //this.tgBuscadorCtasAux.Enabled = false;
                        this.radGridViewCuentasAux.Visible = false;

                        utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                        utiles.ButtonEnabled(ref this.radButtonEditar, false);
                        utiles.ButtonEnabled(ref this.radButtonExport, false);
                    }
                    else
                    {
                        this.VerificarAutorizaciones();

                        if (this.userAutEditar)
                        {
                            string tipoAux = this.codTipoAux;
                            string tipoAuxDesc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", this.codTipoAux, false, "").Trim();
                            if (tipoAuxDesc != "") tipoAux += " " + separadorDesc + " " + tipoAuxDesc;

                            this.radButtonTextBoxTipoAux.Text = tipoAux;

                            //----------------------- Buscador y Grid -------------------------------
                            string query = "select CAUXMA, NOMBMA, PCIFMA, NNITMA, ZONAMA from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM05 ";
                            query += "where TAUXMA = '" + this.codTipoAux + "' ";
                            query += "order by CAUXMA";

                            dtCuentasAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                            if (dtCuentasAux.Rows != null && dtCuentasAux.Rows.Count == 0)
                            {
                                utiles.ButtonEnabled(ref this.radButtonEditar, false);
                                utiles.ButtonEnabled(ref this.radButtonExport, false);
                                this.radGridViewCuentasAux.Visible = false;

                                this.radLabelNoHayInfo.Visible = true;
                            }
                            else
                            {
                                this.radLabelNoHayInfo.Visible = false;

                                this.radGridViewCuentasAux.DataSource = dtCuentasAux;
                                this.RadGridViewHeader();

                                this.radGridViewCuentasAux.Visible = false;

                                for (int i = 0; i < this.radGridViewCuentasAux.Columns.Count; i++)
                                {
                                    this.radGridViewCuentasAux.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                                    this.radGridViewCuentasAux.Columns[i].Width = 600;
                                }

                                this.radGridViewCuentasAux.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                                this.radGridViewCuentasAux.AllowSearchRow = true;
                                this.radGridViewCuentasAux.MasterView.TableSearchRow.IsVisible = false;
                                this.radGridViewCuentasAux.TableElement.SearchHighlightColor = Color.Aqua;
                                this.radGridViewCuentasAux.AllowEditRow = false;
                                this.radGridViewCuentasAux.EnableFiltering = true;

                                this.radGridViewCuentasAux.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                                this.radGridViewCuentasAux.Rows[0].IsCurrent = true;
                                this.radGridViewCuentasAux.Focus();
                                this.radGridViewCuentasAux.Select();

                                this.radGridViewCuentasAux.Refresh();

                                this.radGridViewCuentasAux.Visible = true;
                            }

                            //Verificar si es un tipo de auxiliar externo (si la bbdd es DB2)
                            if (this.proveedorTipo == "DB2")
                            {
                                bool tipoAuxExterno = this.VerificarTipoAuxExterno();
                                if (tipoAuxExterno)
                                {
                                    this.lblExterno.Visible = true;
                                    utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                                }
                                else this.lblExterno.Visible = false;
                            }
                            else this.lblExterno.Visible = false;

                            this.radGridViewCuentasAux.Focus();
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.radButtonTextBoxTipoAux.Focus();

                    this.radGridViewCuentasAux.Visible = false;
                }
            }
            else
            {
                this.radGridViewCuentasAux.Visible = false;

                utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                utiles.ButtonEnabled(ref this.radButtonEditar, false);
                utiles.ButtonEnabled(ref this.radButtonExport, false);
            }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCuentasAux.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCuentasAux.Columns.Contains(item.Key)) this.radGridViewCuentasAux.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }

        private void radPanelApp_Resize(object sender, EventArgs e)
        {
            this.radGridViewCuentasAux.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 121);
        }

        private void radGridViewCuentasAux_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
    }
}
