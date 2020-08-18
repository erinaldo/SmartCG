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
    public partial class frmMtoGLM03Sel : frmPlantilla, IReLocalizable
    {
        private string autClaseElemento = "003";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        //private string autOperAlta = "20";
        private bool autEditar = false;

        private string codPlan = "";
        private string codCuenta = "";

        private bool planActivo = false;

        //private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 112);

        Dictionary<string, string> displayNames;
        DataTable dtCuentasMayor = new DataTable();

        public frmMtoGLM03Sel()
        {
            InitializeComponent();

            this.gbTipoCuenta.ElementTree.EnableApplicationThemeName = false;
            this.gbTipoCuenta.ThemeName = "ControlDefault";
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
                this.dtCuentasMayor.Clear();

                this.FillDataGrid();

                this.radGridViewCuentasMayor.Focus();

                if (e != null && this.radGridViewCuentasMayor!= null && this.radGridViewCuentasMayor.Rows != null && this.radGridViewCuentasMayor.Rows.Count > 0)
                {
                    OperacionMtoTipo operacion = e.Operacion;
                    string codigo = e.Codigo.Trim();
                    switch (operacion)
                    {
                        case OperacionMtoTipo.Alta:
                        case OperacionMtoTipo.Modificar:
                            for (int i = 0; i < this.radGridViewCuentasMayor.Rows.Count; i++)
                            {
                                if (this.radGridViewCuentasMayor.Rows[i].Cells["CUENMC"].Value.ToString().Trim() == codigo)
                                {
                                    this.radGridViewCuentasMayor.Rows[i].IsCurrent = true;
                                    this.radGridViewCuentasMayor.Rows[i].IsSelected = true;

                                    GridTableElement tableElement = (GridTableElement)this.radGridViewCuentasMayor.CurrentView;
                                    //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                                    if (tableElement != null) tableElement.ScrollToRow(i);
                                    this.radGridViewCuentasMayor.Focus();
                                    break;
                                }
                            }
                            break;
                        case OperacionMtoTipo.Eliminar:
                            this.radGridViewCuentasMayor.Rows[0].IsCurrent = true;
                            this.radGridViewCuentasMayor.Rows[0].IsSelected = true;

                            GridTableElement tableElementDel = (GridTableElement)this.radGridViewCuentasMayor.CurrentView;
                            //GridViewRowInfo row = this.radGridViewInfo.CurrentRow;

                            if (tableElementDel != null) tableElementDel.ScrollToRow(0);
                            this.radGridViewCuentasMayor.Focus();

                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        private void FrmMtoGLM03Sel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Mayor Gestión");

            this.TraducirLiterales();

            this.BuildDisplayNames();

            this.radGridViewCuentasMayor.Visible = false;

            this.radGridViewCuentasMayor.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewCuentasMayor.AllowSearchRow = true;
            this.radGridViewCuentasMayor.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewCuentasMayor.EnableFiltering = true;

            utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonExport, false);
            utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
            utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);

            this.radButtonTextBoxPlanCuentas.Select();
        }

        /*
        private void tgBuscadorCuentaMayor_BuscarFormResult(TGBuscador.BuscarFormResultCommandEventArgs e)
        {
            if (this.tgBuscadorCuentaMayor.Datos != null)
            {
                try
                {
                    if (this.tgGridCuentasMayor.dsDatos.Tables["Tabla"] != null) this.tgGridCuentasMayor.dsDatos.Tables["Tabla"].Clear();

                    foreach (DataRow dr in this.tgBuscadorCuentaMayor.Datos.Rows)
                    {
                        if (dr["STATMC"].ToString().Trim() == "*") dr["STATMC"] = this.estadoInactiva;
                        else dr["STATMC"] = this.estadoActiva;

                        this.tgGridCuentasMayor.dsDatos.Tables["Tabla"].Rows.Add(dr.ItemArray);
                    }

                    if (this.tgBuscadorCuentaMayor.Datos.Rows.Count == 0)
                    {
                        utiles.ButtonEnabled(ref this.radButtonEditar, false);
                        utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
                        utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);
                    }
                    else
                    {
                        utiles.ButtonEnabled(ref this.radButtonEditar, true);
                        utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, true);
                        utiles.ButtonEnabled(ref this.radButtonCambiarTipo, true);
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
        }
        */

        private void TgGridCuentasMayor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.EditarCuentaMayor();
        }

        /*
        private void tgTexBoxSelPlan_ValueChanged(TGTexBoxSel.ValueChangedCommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.tgTexBoxSelPlan.Textbox.Modified)
            {
                if (this.tgTexBoxSelPlan.Textbox.Text.Trim() != "")
                {
                    this.tgBuscadorCuentaMayor.ValorFiltro.Text = "";

                    this.tgTexBoxSelPlan.Textbox.Modified = true;

                    string codigo = this.tgTexBoxSelPlan.Textbox.Text.Trim();

                    if (codigo != "" && codigo.Length >= 1)
                    {
                        if (codigo.Length <= 1) this.codPlan = this.tgTexBoxSelPlan.Textbox.Text;
                        else this.codPlan = this.tgTexBoxSelPlan.Textbox.Text.Substring(0, 1);

                        string result = ValidarPlan(this.codPlan);

                        if (result != "")
                        {
                            string error = this.LP.GetText("errValTitulo", "Error");
                            RadMessageBox.Show(result, error);
                            this.tgTexBoxSelPlan.Textbox.Focus();

                            this.tgBuscadorCuentaMayor.Enabled = false;
                            this.tgGridCuentasMayor.Visible = false;

                            utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                            utiles.ButtonEnabled(ref this.radButtonEditar, false);
                            utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
                            utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);
                        }
                        else
                        {
                            string plan = this.codPlan;
                            string planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", this.codPlan, false, "").Trim();
                            if (planDesc != "") plan += " " + separadorDesc + " " + planDesc;

                            this.tgTexBoxSelPlan.Textbox.Text = plan;

                            //----------------------- Buscador y Grid -------------------------------
                            string query = "select STATMC, min(CEDTMC) CEDTMC, TCUEMC, max(NOLAAD) NOLAAD, min(CUENMC) CUENMC from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TIPLMC = '" + this.codPlan + "' ";
                            query += "group by STATMC, CEDTMC, TCUEMC ";
                            query += "order by CEDTMC";

                            //Carga los datos de la Grid
                            this.FillDataGrid();

                            //Buscador
                            this.tgBuscadorCuentaMayor.Query = query;
                            if (this.tgBuscadorCuentaMayor.Datos != null) this.tgBuscadorCuentaMayor.Datos.Clear();
                            this.tgBuscadorCuentaMayor.Datos = null;

                            this.tgBuscadorCuentaMayor.Enabled = true;
                            this.tgGridCuentasMayor.Visible = true;

                            if (this.tgGridCuentasMayor.dsDatos != null && this.tgGridCuentasMayor.dsDatos.Tables["Tabla"] != null &&
                                this.tgGridCuentasMayor.dsDatos.Tables["Tabla"].Rows.Count > 0)
                            {
                                //Habilitar/Deshabilitar botones según autorizaciones
                                this.VerificarAutorizaciones();
                            }
                        }
                    }
                    else
                    {
                        this.tgTexBoxSelPlan.Textbox.Focus();

                        this.tgBuscadorCuentaMayor.Enabled = false;
                        this.tgGridCuentasMayor.Visible = false;
                    }
                }
                else
                {
                    this.tgGridCuentasMayor.Visible = false;
                    this.tgBuscadorCuentaMayor.Enabled = false;

                    utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
                    utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);
                }
            }

            Cursor.Current = Cursors.Default;
        }
        */

        /*
        //Actualizar el listado de elementos desde el formulario de edición que corresponda después de un alta o una actualización de un elementos
        void IFormGLM03Sel.ActualizaListaElementos()
        {
            try
            {
                //Eliminar las filas del DataTable
                this.tgGridCuentasMayor.dsDatos.Tables["Tabla"].Rows.Clear();

                string query = "select ZONAZ1, TIPOZ1, NOMBZ1 from ";
                query += GlobalVar.PrefijoTablaCG + "GLM11 ";
                query += "where CLASZ1 = '" + this.codPlan + "' ";
                query += "order by ZONAZ1";

                //Volver a cargar los valores de la tabla solicitada
                this.FillDataGrid(query);

                //Ajustar las columnas de la Grid
                this.tgGridCuentasMayor.AutoResizeColumns();
                this.tgGridCuentasMayor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch
            {
            }
        }
        */

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevaCuentaMayor();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarCuentaMayor();
        }

        private void RadButtonExport_Click(object sender, EventArgs e)
        {
            this.Exportar();
        }

        private void RadButtonCopiarCuenta_Click(object sender, EventArgs e)
        {
            this.CopiarCuentaMayor();
        }

        private void RadButtonCambiarTipo_Click(object sender, EventArgs e)
        {
            this.CambiarTipoCuentaMayor();
        }

        private void RadButtonElementPlanCuentas_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIPLMP, NOMBMP from ";
            query += GlobalVar.PrefijoTablaCG + "GLM02 ";
            query += "order by TIPLMP";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {

                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar plan de cuentas",
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
                this.radButtonTextBoxPlanCuentas.Text = result;
                this.ActiveControl = this.radButtonTextBoxPlanCuentas;
                this.radButtonTextBoxPlanCuentas.Select(0, 0);
                ///this.radButtonTextBoxPlanCuentas.Focus();
                this.radGridViewCuentasMayor.Focus();
            }
        }

        private void RadButtonTextBoxPlanCuentas_TextChanged(object sender, EventArgs e)
        {
            string codigo = this.radButtonTextBoxPlanCuentas.Text.Trim();
            if (codigo != "")
            {
                if (codigo.Length >= 1)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (codigo.Length <= 1) this.codPlan = this.radButtonTextBoxPlanCuentas.Text;
                    else this.codPlan = this.radButtonTextBoxPlanCuentas.Text.Substring(0, 1);

                    string result = ValidarPlan(this.codPlan);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radButtonTextBoxPlanCuentas.Focus();

                        this.radGridViewCuentasMayor.Visible = false;
                        this.gbTipoCuenta.Visible = false;
                        this.lblTipoCuenta.Visible = false;

                        utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                        utiles.ButtonEnabled(ref this.radButtonEditar, false);
                        utiles.ButtonEnabled(ref this.radButtonExport, false);
                    }
                    else
                    {
                        string plan = this.codPlan;
                        string planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", this.codPlan, false, "").Trim();
                        if (planDesc != "") plan += " " + separadorDesc + " " + planDesc;

                        this.radButtonTextBoxPlanCuentas.Text = plan;

                        this.VerificarAutorizaciones();

                        if (this.autEditar) this.FillDataGrid();

                        this.radGridViewCuentasMayor.Focus();
                    }

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.radButtonTextBoxPlanCuentas.Focus();
                }
            }
            else
            {
                /*this.radGridViewCuentasAux.Visible = false;*/

                utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                utiles.ButtonEnabled(ref this.radButtonEditar, false);
                utiles.ButtonEnabled(ref this.radButtonExport, false);
                this.gbTipoCuenta.Visible = false;
                this.lblTipoCuenta.Visible = false;
            }
        }

        private void RadGridViewCuentasMayor_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarCuentaMayor();
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

        private void RadButtonCopiarCuenta_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCopiarCuenta);
        }

        private void RadButtonCopiarCuenta_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCopiarCuenta);
        }

        private void RadButtonCambiarTipo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCambiarTipo);
        }

        private void RadButtonCambiarTipo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCambiarTipo);
        }

        private void RadPanelApp_Resize(object sender, EventArgs e)
        {
            this.radGridViewCuentasMayor.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 121);
        }

        private void RadGridViewCuentasMayor_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewCuentasMayor, ref this.selectAll);
        }

        private void FrmMtoGLM03Sel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Cuentas de Mayor Gestión");
        }

        private void radGridViewCuentasMayor_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
                //185; 219; 245
                //e.CellElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(219)))), ((int)(((byte)(245)))));
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            try
            {
                //Falta traducir todos los campos !!!
                //Recuperar literales del formulario
                this.Text = this.LP.GetText("lblfrmMtoGLM03Titulo", "Mantenimiento de Cuentas de Mayor");   //Falta traducir

                //Traducir los Literales de los ToolStrip
                this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
                this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");                
                this.radButtonCopiarCuenta.Text = this.LP.GetText("toolStripCopiarCuenta", "Copiar Cuenta");
                this.radButtonCambiarTipo.Text = this.LP.GetText("toolStripCambiarTipo", "Cambiar Tipo Cuenta");

                //Traducir los campos del formulario
                this.lblPlan.Text = this.LP.GetText("lblGLM03Plan", "Plan");
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        /*
        /// <summary>
        /// Construir el control para hacer búquedas de cuentas de mayor
        /// </summary>
        private void BuildtgBuscadorCuentasMayor()
        {
            this.tgBuscadorCuentaMayor.ProveedorDatosForm = GlobalVar.ConexionCG;
            this.tgBuscadorCuentaMayor.Datos = null;
            this.tgBuscadorCuentaMayor.TituloGrupo = " Buscador Cuentas de Mayor ";  //Falta traducir
            this.tgBuscadorCuentaMayor.CamposBusqueda = "STATMC, CEDTMC, TCUEMC, NOLAAD, CUENMC";

            string nombreColumnas = "";
            ArrayList camposGridYDesc = new ArrayList();

            string[] campos1GridYDesc = new string[2];
            campos1GridYDesc[0] = this.LP.GetText("lblListaCampoEstado", "Estado");
            nombreColumnas += campos1GridYDesc[0];
            campos1GridYDesc[1] = "STATMC";
            camposGridYDesc.Add(campos1GridYDesc);

            string[] campos2GridYDesc = new string[2];
            campos2GridYDesc[0] = this.LP.GetText("lblListaCampoCuentaMayor", "Cuenta Mayor");
            nombreColumnas += ", " + campos2GridYDesc[0];
            campos2GridYDesc[1] = "CEDTMC";
            camposGridYDesc.Add(campos2GridYDesc);

            string[] campos3GridYDesc = new string[2];
            campos3GridYDesc[0] = this.LP.GetText("lblListaCampoTipo", "Tipo");
            nombreColumnas += ", " + campos3GridYDesc[0];
            campos3GridYDesc[1] = "TCUEMC";
            camposGridYDesc.Add(campos3GridYDesc);

            string[] campos4GridYDesc = new string[2];
            campos4GridYDesc[0] = this.LP.GetText("lblListaCampoNombre", "Nombre");
            nombreColumnas += ", " + campos4GridYDesc[0];
            campos4GridYDesc[1] = "NOLAAD";
            camposGridYDesc.Add(campos4GridYDesc);

            string[] campos5GridYDesc = new string[2];
            campos5GridYDesc[0] = this.LP.GetText("lblListaCampoCuenta", "Cuenta");
            nombreColumnas += ", " + campos5GridYDesc[0];
            campos5GridYDesc[1] = "CUENMC";
            camposGridYDesc.Add(campos5GridYDesc);

            this.tgBuscadorCuentaMayor.NombreColumnas = nombreColumnas;
            this.tgBuscadorCuentaMayor.NombreColumnasCampos = camposGridYDesc;

            string todasEtiqueta = this.LP.GetText("lblEtiquetaTodasColumnas", "Todas");
            this.tgBuscadorCuentaMayor.NombreColumnasSel = todasEtiqueta;
            this.tgBuscadorCuentaMayor.TodasEtiqueta = todasEtiqueta;

            this.tgBuscadorCuentaMayor.BuscarFormResult += new TGBuscador.BuscarFormResultCommandEventHandler(tgBuscadorCuentaMayor_BuscarFormResult);
            this.tgBuscadorCuentaMayor.FrmPadre = this;
        }
        */
        /*
        /// <summary>
        /// Construir el control de la Grid que contiene las zonas
        /// </summary>
        private void BuildtgGridCuentasMayor()
        {
            //Crear el DataGrid
            this.tgGridCuentasMayor.dsDatos = new DataSet();
            this.tgGridCuentasMayor.dsDatos.DataSetName = "CuentasMayor";
            this.tgGridCuentasMayor.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridCuentasMayor.ReadOnly = true;
            this.tgGridCuentasMayor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridCuentasMayor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridCuentasMayor.AllowUserToAddRows = false;
            this.tgGridCuentasMayor.AllowUserToOrderColumns = false;
            this.tgGridCuentasMayor.AutoGenerateColumns = false;

            DataTable dt = new DataTable();
            dt.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            dt.Columns.Add("STATMC", typeof(string));
            dt.Columns.Add("CEDTMC", typeof(string));
            dt.Columns.Add("TCUEMC", typeof(string));
            dt.Columns.Add("NOLAAD", typeof(string));
            dt.Columns.Add("CUENMC", typeof(string));

            //Crear la columnas del DataGrid
            this.tgGridCuentasMayor.AddTextBoxColumn(0, "STATMC", this.LP.GetText("lblListaCampoEstado", "Estado"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridCuentasMayor.AddTextBoxColumn(1, "CEDTMC", this.LP.GetText("lblListaCampoCuentaMayor", "Cuenta Mayor"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);      //Falta traducir
            this.tgGridCuentasMayor.AddTextBoxColumn(2, "TCUEMC", this.LP.GetText("lblListaCampoTipo", "Tipo"), 40, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);       //Falta traducir
            this.tgGridCuentasMayor.AddTextBoxColumn(3, "NOLAAD", this.LP.GetText("lblListaCampoNombre", "Nombre"), 300, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);       //Falta traducir
            //this.tgGridCuentasMayor.AddTextBoxColumn(4, "CUENMC", this.LP.GetText("lblListaCampoCuenta", "Cuenta"), 0, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);       //Falta traducir
            this.tgGridCuentasMayor.AddTextBoxColumn(4, "CUENMC", this.LP.GetText("lblListaCampoCuenta", "Cuenta"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);       //Falta traducir
            
            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridCuentasMayor.dsDatos.Tables.Add(dt);

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridCuentasMayor.DataSource = this.tgGridCuentasMayor.dsDatos.Tables["Tabla"];
        }
        */

        /// <summary>
        /// Verificar autorizaciones para habilitar/deshabilitar botones
        /// </summary>
        private void VerificarAutorizaciones()
        {
            try
            {
                this.autEditar = aut.Validar(autClaseElemento, autGrupo, this.codPlan, autOperModifica);
                if (!this.autEditar)
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevo, false);
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
                    utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);
                    this.radLabelNoHayInfo.Visible = true;
                    this.radLabelNoHayInfo.Text = "Usuario no autorizado a este plan de cuentas";
                    this.radGridViewCuentasMayor.Visible = false;
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonNuevo, true);
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, true);
                    utiles.ButtonEnabled(ref this.radButtonCambiarTipo, true);
                    this.radLabelNoHayInfo.Visible = false;
                    this.radLabelNoHayInfo.Text = "No existen cuentas de mayor";
                    this.radGridViewCuentasMayor.Visible = true;
                }
                utiles.ButtonEnabled(ref this.radButtonExport, true);
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

                result = this.LP.GetText("lblfrmGLM03ValPlanExcep", "Error al validar el plan") + " (" + ex.Message + ")";
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
                result = this.LP.GetText("lblfrmGLM03ValPlanExcep", "Error al validar el plan") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Llamada a Editar una cuenta de mayor
        /// </summary>
        private void EditarCuentaMayor()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (this.radGridViewCuentasMayor.SelectedRows.Count > 1)
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
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (this.radGridViewCuentasMayor.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewCuentasMayor.CurrentRow.IsExpanded) this.radGridViewCuentasMayor.CurrentRow.IsExpanded = false;
                else this.radGridViewCuentasMayor.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewCuentasMayor.CurrentRow is GridViewDataRowInfo)
            {
                //int indice = this.radGridViewCuentasMayor.CurrentRow.Index;
                int indice = this.radGridViewCuentasMayor.Rows.IndexOf(this.radGridViewCuentasMayor.CurrentRow);
                this.codCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["CUENMC"].Value.ToString();
                string nombreCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["NOLAAD"].Value.ToString();
                string tipoCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["TCUEMC"].Value.ToString();

                switch (tipoCuenta)
                {
                    case "T":
                        frmMtoGLM03Titulo frmMtoCtaMayorTit = new frmMtoGLM03Titulo
                        {
                            Nuevo = false,
                            Copiar = false,
                            CodigoPlan = this.codPlan,
                            NombrePlan = nombre,
                            Codigo = this.codCuenta,
                            NombreCuenta = nombreCuenta,
                            TipoCuenta = tipoCuenta,
                            PlanActivo = this.planActivo,
                            FrmPadre = this
                        };
                        frmMtoCtaMayorTit.Show(this);
                        frmMtoCtaMayorTit.UpdateDataForm += (o, e) =>
                        {
                            ActualizaListaElementos(e);
                        };
                        break;
                    case "D":
                        frmMtoGLM03Detalle frmMtoCtaMayorDet = new frmMtoGLM03Detalle
                        {
                            Nuevo = false,
                            Copiar = false,
                            CodigoPlan = this.codPlan,
                            NombrePlan = nombre,
                            Codigo = this.codCuenta,
                            NombreCuenta = nombreCuenta,
                            TipoCuenta = tipoCuenta,
                            PlanActivo = this.planActivo,
                            FrmPadre = this
                        };
                        frmMtoCtaMayorDet.Show(this);
                        frmMtoCtaMayorDet.UpdateDataForm += (o, e) =>
                        {
                            ActualizaListaElementos(e);
                        };
                        break;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar los elementos seleccionados
        /// </summary>
        private void Exportar()
        {
            this.ExportarGrid(ref this.radGridViewCuentasMayor, "Mantenimiento de Cuentas de Mayor");
        }

        /// <summary>
        /// Llamada a crear nueva cuenta de mayor
        /// </summary>
        private void NuevaCuentaMayor()
        {
            Cursor.Current = Cursors.WaitCursor;

            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            string nombre = "";
            string result = ValidarPlanNombre(this.codPlan, ref nombre);
            if (result != "")
            {
                RadMessageBox.Show(result, error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (!this.planActivo)
            {
                RadMessageBox.Show("Plan de cuenta inactivo", error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (this.rbTitulo.IsChecked == false && this.rbDetalle.IsChecked == false)
            {
                RadMessageBox.Show("Debe indicar el tipo de cuenta (título o detalle)", error);
                this.rbTitulo.Focus();
                return;
            }

            if (this.rbTitulo.IsChecked)
            {
                frmMtoGLM03Titulo frmMtoCtaMayorTit = new frmMtoGLM03Titulo
                {
                    Nuevo = true,
                    Copiar = false,
                    CodigoPlan = this.codPlan,
                    NombrePlan = nombre,
                    Codigo = "",
                    NombreCuenta = "",
                    TipoCuenta = "T",
                    CodigoCuentaCopiar = "",
                    PlanActivo = this.planActivo,
                    FrmPadre = this
                };
                frmMtoCtaMayorTit.Show(this);
                frmMtoCtaMayorTit.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
            }
            else
            {
                frmMtoGLM03Detalle frmMtoCtaMayorDet = new frmMtoGLM03Detalle
                {
                    Nuevo = true,
                    Copiar = false,
                    CodigoPlan = this.codPlan,
                    NombrePlan = nombre,
                    Codigo = "",
                    NombreCuenta = "",
                    TipoCuenta = "D",
                    CodigoCuentaCopiar = "",
                    PlanActivo = this.planActivo,
                    FrmPadre = this
                };
                frmMtoCtaMayorDet.Show(this);
                frmMtoCtaMayorDet.UpdateDataForm += (o, e) =>
                {
                    ActualizaListaElementos(e);
                };
            }
            
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Copiar una cuenta de mayor
        /// </summary>
        private void CopiarCuentaMayor()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
            {
                RadMessageBox.Show("Código de Plan no puede estar en blanco", error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (this.radGridViewCuentasMayor.SelectedRows.Count > 1)
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
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            if (!this.planActivo)
            {
                RadMessageBox.Show("Plan de cuenta inactivo", error);
                this.radButtonTextBoxPlanCuentas.Focus();
                return;
            }

            //int indice = this.radGridViewCuentasMayor.CurrentRow.Index;
            int indice = this.radGridViewCuentasMayor.Rows.IndexOf(this.radGridViewCuentasMayor.CurrentRow);
            string tipoCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["TCUEMC"].Value.ToString();

            if ((this.rbTitulo.IsChecked == true && tipoCuenta == "D") ||
                (this.rbDetalle.IsChecked == true && tipoCuenta == "T"))
            {
                RadMessageBox.Show("El tipo de cuenta seleccionado (título o detalle) no corresponde con el de la cuenta a copiar", error);
                this.rbTitulo.Focus();
                return;
            }

            this.codCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["CUENMC"].Value.ToString();
            string nombreCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["NOLAAD"].Value.ToString();
            
            switch (tipoCuenta)
            {
                case "T":
                    frmMtoGLM03Titulo frmMtoCtaMayorTit = new frmMtoGLM03Titulo
                    {
                        Nuevo = false,
                        Copiar = true,
                        CodigoPlan = this.codPlan,
                        NombrePlan = nombre,
                        Codigo = "",
                        NombreCuenta = nombreCuenta,
                        TipoCuenta = tipoCuenta,
                        CodigoCuentaCopiar = this.codCuenta,
                        PlanActivo = this.planActivo,
                        FrmPadre = this
                    };
                    frmMtoCtaMayorTit.Show(this);
                    frmMtoCtaMayorTit.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
                case "D":
                    frmMtoGLM03Detalle frmMtoCtaMayorDet = new frmMtoGLM03Detalle
                    {
                        Nuevo = false,
                        Copiar = true,
                        CodigoPlan = this.codPlan,
                        NombrePlan = nombre,
                        Codigo = this.codCuenta,
                        NombreCuenta = nombreCuenta,
                        TipoCuenta = tipoCuenta,
                        CodigoCuentaCopiar = this.codCuenta,
                        PlanActivo = this.planActivo,
                        FrmPadre = this
                    };
                    frmMtoCtaMayorDet.Show(this);
                    frmMtoCtaMayorDet.UpdateDataForm += (o, e) =>
                    {
                        ActualizaListaElementos(e);
                    };
                    break;
            }

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
                string query = "select STATMC, min(CEDTMC) CEDTMC, TCUEMC, max(NOLAAD) NOLAAD, min(CUENMC) CUENMC from ";
                query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codPlan + "' ";
                query += "group by STATMC, CEDTMC, TCUEMC ";
                query += "order by CEDTMC";

                dtCuentasMayor = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Habilitar/Deshabilitar botones según autorizaciones
                //this.VerificarAutorizaciones();

                if (dtCuentasMayor != null && dtCuentasMayor.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCuentasMayor.Rows)
                    {
                        if (dr["STATMC"].ToString().Trim() == "*") dr["STATMC"] = this.estadoInactiva;
                        else dr["STATMC"] = this.estadoActiva;
                    }
                }

                this.radGridViewCuentasMayor.DataSource = null;
                this.radGridViewCuentasMayor.DataSource = dtCuentasMayor;
                this.radGridViewCuentasMayor.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";

                if (this.radGridViewCuentasMayor.Rows != null && this.radGridViewCuentasMayor.Rows.Count > 0)
                {
                    this.RadGridViewHeader();

                    this.radGridViewCuentasMayor.Visible = false;
                    this.radLabelNoHayInfo.Visible = false;
                    this.gbTipoCuenta.Visible = true;
                    this.lblTipoCuenta.Visible = true;

                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonExport, true);
                    utiles.ButtonEnabled(ref this.radButtonCambiarTipo, true);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, true);
                    
                    for (int i = 0; i < this.radGridViewCuentasMayor.Columns.Count; i++)
                    {
                        this.radGridViewCuentasMayor.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewCuentasMayor.Columns[i].Width = 600;
                    }

                    this.radGridViewCuentasMayor.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewCuentasMayor.AllowSearchRow = true;
                    this.radGridViewCuentasMayor.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewCuentasMayor.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewCuentasMayor.AllowEditRow = false;
                    this.radGridViewCuentasMayor.EnableFiltering = true;
                    
                    this.radGridViewCuentasMayor.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewCuentasMayor.Rows[0].IsCurrent = true;
                    this.radGridViewCuentasMayor.Focus();
                    this.radGridViewCuentasMayor.Select();

                    this.radGridViewCuentasMayor.Refresh();
                    
                    this.radGridViewCuentasMayor.Visible = true;
                }
                else
                {
                    this.radLabelNoHayInfo.Visible = true;
                    this.radGridViewCuentasMayor.Visible = false;
                    this.gbTipoCuenta.Visible = true;
                    this.lblTipoCuenta.Visible = true;

                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonExport, false);
                    utiles.ButtonEnabled(ref this.radButtonCambiarTipo, false);
                    utiles.ButtonEnabled(ref this.radButtonCopiarCuenta, false);
                }
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Funcionalidad de Cambiar de Tipo la Cuenta de Mayor
        /// </summary>
        private void CambiarTipoCuentaMayor()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //int indice = this.radGridViewCuentasMayor.CurrentRow.Index;
                int indice = this.radGridViewCuentasMayor.Rows.IndexOf(this.radGridViewCuentasMayor.CurrentRow);
                this.codCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["CUENMC"].Value.ToString();
                string nombreCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["NOLAAD"].Value.ToString();
                string tipoCuenta = this.radGridViewCuentasMayor.Rows[indice].Cells["TCUEMC"].Value.ToString();
                string estado = this.radGridViewCuentasMayor.Rows[indice].Cells["STATMC"].Value.ToString();
                
                string error = this.LP.GetText("errValTitulo", "Error");

                if (estado == this.estadoInactiva)
                {    
                    RadMessageBox.Show("Cuenta inactiva. No se puede cambiar de tipo", error);
                    return;
                }

                bool cambiarTipo = true;

                //Pedir confirmación para cambiar de tipo
                string mensajeDeseaCont = " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                string mensajeTD = this.LP.GetText("confCambiarTipoTD", "Cuenta de título pasa a detalle.") + " " + mensajeDeseaCont;
                string mensajeDT = this.LP.GetText("confCambiarTipoDT", "Cuenta de detalle pasa a título.") + " " + mensajeDeseaCont;
                DialogResult resultDlg;

                if (tipoCuenta == "T") resultDlg = RadMessageBox.Show(mensajeTD, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                else resultDlg = RadMessageBox.Show(mensajeDT, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);

                if (resultDlg == DialogResult.No) cambiarTipo = false;

                if (cambiarTipo)
                {
                    string result = "";

                    switch (tipoCuenta)
                    {
                        case "T":
                            result = this.CambiarTipoDeTD(indice);
                            break;
                        case "D":
                            result = this.CambiarTipoDeDT(indice);
                            break;
                    }

                    if (result != "")
                    {
                        RadMessageBox.Show(result, error);
                        return;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Cambia el tipo de la cuenta de mayor de titulo a detalle
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        private string CambiarTipoDeTD(int indice)
        {
            IDataReader dr = null;
            string result = "";
            try
            {
                string CEDTMC = "";

                //Chequear que no sea una cuenta de primer nivel
                string query = "select NIVEMC, CEDTMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + this.codCuenta;
                query += "' and TCUEMC = 'T'";

                string nivCta = "";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    nivCta = dr.GetValue(dr.GetOrdinal("NIVEMC")).ToString();
                    CEDTMC = dr.GetValue(dr.GetOrdinal("CEDTMC")).ToString();
                }
                dr.Close();

                if (nivCta == "1")
                {
                    result = "Una cuenta de primer nivel no puede ser de detalle";      //Falta traducir
                    return (result);
                }

                //Chequear si la cuenta siguiente en el nivel es de título
                string errores = "";
                string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codPlan);
                string[] cuentas = utilesCG.CuentaCompletarNiveles(this.codCuenta.Trim(), this.codPlan, estructuraMascaraPlan[0], ref errores);
                int cantCuentas = 0;

                if (cuentas[0] != "")
                {
                    try
                    {
                        cantCuentas = Convert.ToInt16(cuentas[0]);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                string cuentaSgte = "";
                if (cantCuentas >= 1)
                {
                    cuentaSgte = cuentas[2];
                }

                if (cuentaSgte != "")
                {
                    query = "select TCUEMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + cuentaSgte + "'";

                    string tipoCta = "";
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        tipoCta = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString();
                    }
                    dr.Close();

                    if (tipoCta == "T")
                    {
                        result = "Cuenta tiene cuentas de título de nivel superior";      //Falta traducir
                        return (result);
                    }
                }

                //Verificar si la cuenta tiene hijos que el numero de cuenta es diferente de 0
                DataTable dtCuenta = utilesCG.ObtenerEstructuraCuenta(this.codCuenta.Trim(), this.codPlan, ref result);

                string niv = "";
                int nivel = 0;
                if (dtCuenta != null && dtCuenta.Rows.Count > 0)
                {
                    niv = dtCuenta.Rows[dtCuenta.Rows.Count - 1]["NIVEMC"].ToString().Trim();

                    if (niv != "") nivel = Convert.ToInt16(niv);
                }

                if (nivel != 0)
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + this.codPlan + "' and CUENMC like '" + this.codCuenta.Trim() + "%' and ";
                    query += "NIVEMC = " + (nivel + 1);

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    int cont = 0;
                    string cuentaActual = "";
                    while (dr.Read())
                    {
                        cuentaActual = dr.GetValue(dr.GetOrdinal("CUENMC")).ToString();
                        cont++;
                    }
                    dr.Close();

                    if (cont > 1)
                    {
                        result = "Cuenta no puede pasar a título. Tiene cuentas de detalles sin ceros.";
                        return (result);
                    }
                    else if (cont == 1 && cuentaActual != "")
                    {
                        cuentaActual = cuentaActual.Trim();
                        if (cuentaActual.Substring(cuentaActual.Length - 1, 1) != "0")
                        {
                            result = "Cuenta no puede pasar a título. Cuentas de detalle sin ceros.";
                            return (result);
                        }
                    }
                }

                //Recuperar los datos de la cuenta de último nivel
                string ADICMC = "";
                string FEVEMC = "";
                string NDDOMC = "";
                string TERMMC = "";
                string TIMOMC = "";
                string TAU1MC = "";
                string TAU2MC = "";
                string TAU3MC = "";
                string TDOCMC = "";
                string GRUPMC = "";
                string DEAUMC = "";
                string RNITMC = "";
                string CNITMC = "";
                string MASCMC = "";

                string errorMsg = "";

                int fila = 0;
                bool ctaUltNivel = false;

                DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivelValoresCampos(this.codCuenta.Trim(), this.codPlan, ref errorMsg);

                if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0)
                {
                    fila = dtCtasUltimoNivel.Rows.Count - 1;
                    ADICMC = dtCtasUltimoNivel.Rows[fila]["ADICMC"].ToString();
                    FEVEMC = dtCtasUltimoNivel.Rows[fila]["FEVEMC"].ToString();
                    NDDOMC = dtCtasUltimoNivel.Rows[fila]["NDDOMC"].ToString();
                    TERMMC = dtCtasUltimoNivel.Rows[fila]["TERMMC"].ToString();
                    TIMOMC = dtCtasUltimoNivel.Rows[fila]["TIMOMC"].ToString();
                    TAU1MC = dtCtasUltimoNivel.Rows[fila]["TAU1MC"].ToString();
                    TAU2MC = dtCtasUltimoNivel.Rows[fila]["TAU2MC"].ToString();
                    TAU3MC = dtCtasUltimoNivel.Rows[fila]["TAU3MC"].ToString();
                    TDOCMC = dtCtasUltimoNivel.Rows[fila]["TDOCMC"].ToString();
                    GRUPMC = dtCtasUltimoNivel.Rows[fila]["GRUPMC"].ToString();
                    DEAUMC = dtCtasUltimoNivel.Rows[fila]["DEAUMC"].ToString();
                    RNITMC = dtCtasUltimoNivel.Rows[fila]["RNITMC"].ToString();
                    CNITMC = dtCtasUltimoNivel.Rows[fila]["CNITMC"].ToString();
                    MASCMC = dtCtasUltimoNivel.Rows[fila]["MASCMC"].ToString();
                    //CEDTMC = dtCtasUltimoNivel.Rows[fila]["CEDTMC"].ToString();

                    ctaUltNivel = true;
                }

                if (ctaUltNivel)
                {
                    //Pasar la cuenta de título a detalle
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "set TCUEMC = 'D', ADICMC = '" + ADICMC + "', FEVEMC = '" + FEVEMC + "', NDDOMC = '" + NDDOMC + "', TERMMC = '" + TERMMC + "', ";
                    query += "TIMOMC = '" + TIMOMC + "', TAU1MC = '" + TAU1MC + "', TAU2MC = '" + TAU2MC + "', TAU3MC = '" + TAU3MC + "', ";
                    query += "TDOCMC = '" + TDOCMC + "', GRUPMC = '" + GRUPMC + "', DEAUMC = '" + DEAUMC + "', RNITMC = '" + RNITMC + "', ";
                    query += "CNITMC = '" + CNITMC + "', MASCMC = '" + MASCMC + "', CEDTMC = '" + CEDTMC + "' ";
                    query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + this.codCuenta;
                    query += "' and TCUEMC = 'T'";

                    int cantReg = 0;
                    cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Actualizar los datos en la grid
                    this.radGridViewCuentasMayor.Rows[indice].Cells["TCUEMC"].Value = "D";
                    this.radGridViewCuentasMayor.Rows[indice].Cells["CEDTMC"].Value = CEDTMC;

                    //Actualizar el campo cuenta formateada de las cuentas del resto de niveles
                    string codCuentaAux = "";
                    for (int i = 0; i < cantCuentas; i++)
                    {
                        codCuentaAux = cuentas[2 + i].ToString();
                        query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                        query += "set CEDTMC = '" + CEDTMC + "' ";
                        query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + codCuentaAux + "' and TCUEMC = 'D'";

                        cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    
                    this.FillDataGrid();

                    this.radGridViewCuentasMayor.Focus();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmGLM03CambiarTipoDeTD", "Error al cambiar el tipo de cuenta de titulo a detalle") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Cambia el tipo de la cuenta de mayor de detalle a título
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        private string CambiarTipoDeDT(int indice)
        {
            IDataReader dr = null;
            string result = "";
            try
            {
                //Chequear que la cuenta no sea de último nivel del plan
                int[] nivelLongitud = utilesCG.ObtenerNivelLongitudDadoPlan(this.codPlan);
                int nivelPlanCuentas = nivelLongitud[0];

                string query = "select NIVEMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + this.codCuenta;
                query += "' and TCUEMC = 'D'";

                string nivCta = "";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    nivCta = dr.GetValue(dr.GetOrdinal("NIVEMC")).ToString();
                }
                dr.Close();

                if (nivCta == "")
                {
                    result = "No se pudo recuperar la cuenta";      //Falta traducir
                    return (result);
                }

                int nivelCuenta = Convert.ToInt16(nivCta);

                if (nivelCuenta == nivelPlanCuentas)
                {
                    result = "Una cuenta de último nivel no puede ser de título";       //Falta traducir
                    return (result);
                }

                string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codPlan);
                string error = "";
                int nivel = 0;
                string cuentaActualFormateada = utilesCG.CuentaFormatear(this.codCuenta, this.codPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);

                //Campos CEDTMC - cuenta formateada y FCRTMC - fecha como quedarian??
                query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "set TCUEMC = 'T', CEDTMC = '" + cuentaActualFormateada + "' , ";
                query += "ADICMC = ' ', FEVEMC = ' ', NDDOMC = ' ', TERMMC = ' ', TIMOMC = ' ', ";
                query += "TAU1MC = ' ', TAU2MC = ' ', TAU3MC = ' ', TDOCMC = ' ', GRUPMC = ' ', ";
                query += "DEAUMC = ' ', RNITMC = ' ', CNITMC = ' ', MASCMC = ' ' ";
                query += "where TIPLMC = '" + this.codPlan + "' and CUENMC = '" + this.codCuenta;
                query += "' and TCUEMC = 'D'";
                //Campos extendidos ??

                int cantReg = 0;
                cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Buscar las cuentas de detalles de niveles siguientes para actualizar el campo CEDTMC - cuenta editada
                //Chequear si la cuenta siguiente en el nivel es de título
                string errores = "";
                //string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codPlan);
                string[] cuentas = utilesCG.CuentaCompletarNiveles(this.codCuenta.Trim(), this.codPlan, estructuraMascaraPlan[0], ref errores);
                int cantCuentas = 0;
                string nuevaCuentaAEditar = "";
                string CEDTMC = "";

                if (cuentas[0] != "")
                {
                    try
                    {
                        cantCuentas = Convert.ToInt16(cuentas[0]);
                        if (cantCuentas >= 1)
                        {
                            //Coger la siguiente cuenta de detalle
                            nuevaCuentaAEditar = cuentas[2].ToString();

                            //Formatear Cuenta
                            string cuentaFormateada = utilesCG.CuentaFormatear(nuevaCuentaAEditar, this.codPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);
                            CEDTMC = cuentaFormateada;

                            query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "set CEDTMC = '" + cuentaFormateada + "' ";
                            query += "where TIPLMC = '" + this.codPlan + "' and TCUEMC = 'D' ";

                            string codCuentaAux = "";
                            string queryAux = "";
                            
                            for (int i = 0; i < cantCuentas; i++)
                            {
                                codCuentaAux = cuentas[2 + i].ToString();
                                queryAux = query + " and CUENMC = '" + codCuentaAux + "'";

                                cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(queryAux, GlobalVar.ConexionCG.GetConnectionValue);
                            }

                            //Volver a cargar los datos de la Grid
                            query = "select STATMC, min(CEDTMC) CEDTMC, TCUEMC, max(NOLAAD) NOLAAD, min(CUENMC) CUENMC from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TIPLMC = '" + this.codPlan + "' ";
                            query += "group by STATMC, CEDTMC, TCUEMC ";
                            query += "order by CEDTMC";

                            this.FillDataGrid();

                            this.radGridViewCuentasMayor.Focus();
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmGLM03CambiarTipoDeDT", "Error al cambiar el tipo de cuenta de detalle a titulo") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCuentasMayor.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCuentasMayor.Columns.Contains(item.Key)) this.radGridViewCuentasMayor.Columns[item.Key].HeaderText = item.Value;
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

                campo = "STATMC";
                header = this.LP.GetText("lblListaCampoEstado", "Estado");
                this.displayNames.Add(campo, header);

                campo = "CEDTMC";
                header = this.LP.GetText("lblListaCampoCuentaMayor", "Cuenta Mayor");
                this.displayNames.Add(campo, header);

                campo = "TCUEMC";
                header = this.LP.GetText("lblListaCampoTipo", "Tipo");
                this.displayNames.Add(campo, header);

                campo = "NOLAAD";
                header = this.LP.GetText("lblListaCampoNombre", "Nombre");
                this.displayNames.Add(campo, header);

                campo = "CUENMC";
                header = this.LP.GetText("lblListaCampoCuenta", "Cuenta");
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        #endregion

    }
}
