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
    public partial class frmMtoIVT03 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCIAFIS";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "";
        private const string autGrupo = "";
        private const string autOperConsulta = "";
        private const string autOperModifica = "";

        private DataTable dtCompaniasAsociadas;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        Dictionary<string, string> displayNames;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public frmMtoIVT03()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbDireccion.ElementTree.EnableApplicationThemeName = false;
            this.gbDireccion.ThemeName = "ControlDefault";

            this.radGroupBoxAgenciaTributaria.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxAgenciaTributaria.ThemeName = "ControlDefault";

            this.radGroupBoxTipoDeclaracion.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxTipoDeclaracion.ThemeName = "ControlDefault";

            this.radGroupBoxPeriodoImpositivo.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxPeriodoImpositivo.ThemeName = "ControlDefault";

            this.gbDireccion.ElementTree.EnableApplicationThemeName = false;
            this.gbDireccion.ThemeName = "ControlDefault";

            this.radToggleSwitchActivarExtIVA.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchActivarExtIVA.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchActivarExtSII.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchActivarExtSII.ThemeName = "MaterialBlueGrey";

            //this.radCollapsiblePanelCompaniasContablesAsociadas.IsExpanded = false;
            this.radCollapsiblePanelCompaniasContablesAsociadas.EnableAnimation = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoIVT03_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Compañías Fiscales Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC'
            this.KeyPreview = true;

            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Construir el DataGrid de Compañias Contables Asociadas
            //this.BuildDataGridCompaniasAsociadas();

            utiles.ButtonEnabled(ref this.btnAddCompania, false);

            if (this.nuevo)
            {
                this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = false;

                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                //this.ActiveControl = this.txtCodigo;
                //this.txtCodigo.Select(0, 0);
                //this.txtCodigo.Focus();
                this.ActiveControl = this.radButtonTextBoxCodigo;
                this.radButtonTextBoxCodigo.Select(0, 0);
                this.radButtonTextBoxCodigo.Focus();

                //this.Size = new System.Drawing.Size(567, 176); 
            }
            else
            {
                //this.txtCodigo.IsReadOnly = true;
                this.radButtonTextBoxCodigo.ReadOnly = true;
                this.radButtonElementradButtonCompania.Enabled = false;

                this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = true;

                //Recuperar la información de la compañía fiscaly mostrarla en los controles
                this.CargarInfoCompaniaFiscal();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtIdFiscal_DNI;
                    this.txtIdFiscal_DNI.Select(0, 0);
                    this.txtIdFiscal_DNI.Focus();
                }

                //this.Size = new System.Drawing.Size(567, 653); 
            }
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void DgCompania_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.dgCompania.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.dgCompania.dsDatos.Tables[0].Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string compania = this.dgCompania.Rows[e.RowIndex].Cells["CCIAMG"].Value.ToString();

                    //Pedir confirmación y eliminar el cambio seleccionado
                    //this.LP.GetText("wrnDeleteConfirm"       
                    string mensaje = "Se va a eliminar la compañía asociada " + compania;  //Falta traducir
                    mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        //Verificar que la compañía fiscal no tenga movimientos vigentes para esa compañía
                        string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                        query += "where CIAFB3 = '" + this.codigo + "' and ";
                        query += "CCIAB3 = '" + compania + "' ";

                        int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                        if (cantRegistros > 0)
                        {
                            RadMessageBox.Show("La compañía fiscal tiene movimientos vigentes", this.LP.GetText("errEliminarAsocCompContableMovVig", "Error"));  //Falta traducir
                        }
                        else
                        {
                            query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT02 where ";
                            query += " CIAFT2 = '" + this.codigo + "' and CCIAT2 = '" + compania + "'";

                            int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                            //Eliminar la entrada del DataSet
                            this.dgCompania.dsDatos.Tables["Tabla"].Rows.RemoveAt(e.RowIndex);

                            if (this.dgCompania.dsDatos.Tables["Tabla"].Rows.Count == 0)
                            {
                                this.dgCompania.Visible = false;
                                this.lblNoExisteCompAsoc.Visible = true;
                            }

                            //Actualiza el listado de elementos del formulario frmMtoIVT03Sel
                            this.ActualizarFormularioListaElementos();
                        }
                    }

                    this.dgCompania.ClearSelection();

                    Cursor.Current = Cursors.Default;
                }
            }
            */
        }

        private void BtnAddCompania_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = "";
                string result = this.CodigoCompaniaAsociarValido(ref nombre);
                if (result != "")
                {
                    RadMessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
                    this.radButtonTextBoxCompania.Focus();
                    return;
                }

                //Insertar la compañía a la lista de compañías asociadas 
                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVT02";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATT2, CCIAT2, CIAFT2, FILLT2) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "' ', '" + this.radButtonTextBoxCompania.Text.Trim() + "', '" + this.codigo + "', ' ')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Insertar la compañía en la grid
                DataRow row = this.dtCompaniasAsociadas.NewRow();
                row["CCIAMG"] = this.radButtonTextBoxCompania.Text.Trim();
                row["NCIAMG"] = nombre;
                row["Eliminar"] = global::ModMantenimientos.Properties.Resources.Eliminar;
                this.dtCompaniasAsociadas.Rows.Add(row);

                this.lblNoExisteCompAsoc.Visible = false;
                this.radGridViewCompania.Visible = true;
                this.radGridViewCompania.ClearSelection();

                this.radButtonTextBoxCompania.Text = "";
                this.btnAddCompania.Enabled = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FrmMtoIVT03_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null); 
        }

        private void TxtIdFiscal_DNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtTelefono, false, ref sender, ref e);
        }

        private void TxtNumCasa_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtNumCasa, false, ref sender, ref e);
        }

        private void TxtCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtCP, false, ref sender, ref e);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonElementCompania_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CCIAMG, NCIAMG from ";
            query += GlobalVar.PrefijoTablaCG + "GLM01 ";
            query += "where STATMG='V' ";
            query += "order by CCIAMG";

            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar compañias",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnas,
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
            }
            this.radButtonTextBoxCompania.Text = result;
            this.ActiveControl = this.radButtonTextBoxCompania;
            this.radButtonTextBoxCompania.Select(0, 0);
            this.radButtonTextBoxCompania.Focus();
        }

        private void RadButtonTextBoxCompania_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxCompania.Text.Trim() == "") utiles.ButtonEnabled(ref this.btnAddCompania, false);
            else utiles.ButtonEnabled(ref this.btnAddCompania, true);
        }

        private void RadGridViewCompania_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //GridCheckBoxCellElement checkBoxCellElement = sender as GridCheckBoxCellElement;

            if (sender is GridImageCellElement imageCellElement)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (this.radGridViewCompania.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.radGridViewCompania.Rows.Count)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        string compania = this.radGridViewCompania.Rows[e.RowIndex].Cells["CCIAMG"].Value.ToString();

                        //Pedir confirmación y eliminar el cambio seleccionado
                        //this.LP.GetText("wrnDeleteConfirm"       
                        string mensaje = "Se va a eliminar la compañía asociada " + compania;  //Falta traducir
                        mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                        DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            //Verificar que la compañía fiscal no tenga movimientos vigentes para esa compañía
                            string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                            query += "where CIAFB3 = '" + this.codigo + "' and ";
                            query += "CCIAB3 = '" + compania + "' ";

                            int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                            if (cantRegistros > 0)
                            {
                                RadMessageBox.Show("La compañía fiscal tiene movimientos vigentes", this.LP.GetText("errEliminarAsocCompContableMovVig", "Error"));  //Falta traducir
                            }
                            else
                            {
                                query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT02 where ";
                                query += " CIAFT2 = '" + this.codigo + "' and CCIAT2 = '" + compania + "'";

                                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                                //Eliminar la entrada del DataSet
                                this.dtCompaniasAsociadas.Rows.RemoveAt(e.RowIndex);

                                if (this.radGridViewCompania.Rows.Count == 0)
                                {
                                    this.radGridViewCompania.Visible = false;
                                    this.lblNoExisteCompAsoc.Visible = true;
                                }
                            }
                        }

                        this.radGridViewCompania.ClearSelection();

                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            /*
            if (checkBoxCellElement != null)
            {
                RadCheckBoxEditorElement element = checkBoxCellElement.Children[0] as RadCheckBoxEditorElement;
                Point mousPos = element.ElementTree.Control.PointToClient(Control.MousePosition);

                if (element != null && element.Checkmark.ControlBoundingRectangle.Contains(mousPos))
                {
                    //checkbox is clicked
                }
            }
            */
        }

        private void RadGridViewCompania_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.IsCurrent)
            {
                e.CellElement.DrawFill = false;
            }
            e.CellElement.DrawBorder = false;
        }

        private void RadCollapsiblePanelCompaniasContablesAsociadas_Expanded(object sender, EventArgs e)
        {
            /*
            int heightExpanded = this.Height + this.radCollapsiblePanelCompaniasContablesAsociadas.Height + this.radButtonDelete.Height + 10;
            this.Size = new System.Drawing.Size(902, heightExpanded);

            int heightButton = this.Size.Height - 70 - this.AutoScrollPosition.Y;

            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, heightButton);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, heightButton);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, heightButton);
            
            this.AutoScroll = true;

            this.CentrarForm();
            */

            //this.AutoScroll = true;
            /*
            this.Size = new System.Drawing.Size(851, 850);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);

            this.CentrarForm();*/
        }

        private void RadCollapsiblePanelCompaniasContablesAsociadas_Collapsed(object sender, EventArgs e)
        {
            /*
            this.Size = new System.Drawing.Size(902, 700);

            this.AutoScroll = true;

            int heightButton = this.Size.Height - 70;

            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, heightButton);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, heightButton);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, heightButton);
            
            this.CentrarForm();
            */

            /*
            //this.AutoScroll = false;
            /*this.Size = new System.Drawing.Size(902; 710);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);
            this.radButtonDelete.Location = new Point(this.radButtonDelete.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 70 - this.AutoScrollPosition.Y);

            this.CentrarForm();*/
        }

        private void FrmMtoIVT03_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void BtnAddCompania_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAddCompania);
        }

        private void BtnAddCompania_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAddCompania);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmMtoIVT03_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.radButtonTextBoxCodigo.Text.ToString() != this.radButtonTextBoxCodigo.Tag.ToString() ||
                    this.txtIdFiscal_DNI.Text.ToString() != this.txtIdFiscal_DNI.Tag.ToString() ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtTelefono.Text != this.txtTelefono.Tag.ToString() ||
                    this.txtApellidoNombre.Text != this.txtApellidoNombre.Tag.ToString() ||
                    this.txtSiglas.Text != this.txtSiglas.Tag.ToString() ||
                    this.txtNombreVia.Text != this.txtNombreVia.Tag.ToString() ||
                    this.txtNumCasa.Text != this.txtNumCasa.Tag.ToString() ||
                    this.txtCP.Text != this.txtCP.Tag.ToString() ||
                    this.txtMunicipio.Text != this.txtMunicipio.Tag.ToString()
                )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
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
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Compañías Fiscales Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoIVT03TituloALta", "Mantenimiento de Compañías Fiscales - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoIVT03TituloEdit", "Mantenimiento de Compañías Fiscales - Edición");           //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblCodigo.Text = this.LP.GetText("lblIVT03CompFiscal", "Compañía Fiscal"); //Falta traducir
            this.lblIDFiscal_DNI.Text = this.LP.GetText("lblIVT03IdFiscal_DNI", "Id. Fiscal o DNI");    //Falta 
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.txtIdFiscal_DNI.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.txtTelefono.Enabled = valor;
            this.txtApellidoNombre.Enabled = valor;

            //this.txtUltAnoMesCerrado.Enabled = valor;
            //this.txtUltEjercFiscal.Enabled = valor;

            this.txtSiglas.Enabled = valor;
            this.txtNombreVia.Enabled = valor;
            this.txtNumCasa.Enabled = valor;
            this.txtCP.Enabled = valor;
            this.txtMunicipio.Enabled = valor;

            this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La compañía fiscal está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.txtIdFiscal_DNI.IsReadOnly = true;
            this.txtNombre.IsReadOnly = true;
            this.txtTelefono.IsReadOnly = true;
            this.txtApellidoNombre.IsReadOnly = true;

            //this.txtUltAnoMesCerrado.ReadOnly = true;
            //this.txtUltEjercFiscal.ReadOnly = true;

            this.txtSiglas.IsReadOnly = true;
            this.txtNombreVia.IsReadOnly = true;
            this.txtNumCasa.IsReadOnly = true;
            this.txtCP.IsReadOnly = true;
            this.txtMunicipio.IsReadOnly = true;

            this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = false;
        }

        /// <summary>
        /// Valida que no exista el código de la compañía fiscal
        /// </summary>
        /// <returns></returns>
        private bool CodigoCompaniaFiscalValido()
        {
            bool result = false;

            try
            {
                //string codCompania = this.txtCodigo.Text.Trim();
                string codCompania = this.radButtonTextBoxCodigo.Text.Trim();

                if (codCompania != "")
                {
                    string query = "select count(CIAFT3) from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "where CIAFT3 = '" + codCompania + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private bool CodigoCompaniaFiscalExiste()
        {
            bool result = false;

            try
            {
                //string codCompania = this.txtCodigo.Text.Trim();
                string codCompania = this.radButtonTextBoxCodigo.Text.Trim();

                if (codCompania != "")
                {
                    string query = "select count(CCIAMG) from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                    query += "where CCIAMG = '" + codCompania + "' and STATMG = 'V'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros != 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Carga la información de la lista de compañías contables asociadas
        /// </summary>
        private void CargarInfoCompaniasAsociadas()
        {
            try
            {
                string query = "select CCIAMG, NCIAMG ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLM01, " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                query += "where CIAFT2 = '" + this.codigo + "' and ";
                query += "CCIAT2 = CCIAMG ";
                query += "order by CCIAMG";

                this.dtCompaniasAsociadas = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                this.dtCompaniasAsociadas.Columns.Add("Eliminar", typeof(Image));

                if (this.dtCompaniasAsociadas != null && this.dtCompaniasAsociadas.Rows.Count > 0)
                {
                    //SMR this.dtCompaniasAsociadas.Columns.Add("Eliminar", typeof(Image));

                    for (int i = 0; i < this.dtCompaniasAsociadas.Rows.Count; i++)
                    {
                        this.dtCompaniasAsociadas.Rows[i]["Eliminar"] = global::ModMantenimientos.Properties.Resources.Eliminar;
                    }

                    this.radGridViewCompania.DataSource = this.dtCompaniasAsociadas;

                    if (this.radGridViewCompania.Rows != null && this.radGridViewCompania.Rows.Count > 0)
                    {
                        this.RadGridViewHeader();

                        for (int i = 0; i < this.radGridViewCompania.Columns.Count; i++)
                        {
                            this.radGridViewCompania.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewCompania.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewCompania.MasterTemplate.BestFitColumns();
                    }
                    this.radGridViewCompania.Visible = true;
                    this.radGridViewCompania.ClearSelection();
                    this.lblNoExisteCompAsoc.Visible = false;
                }
                else
                {
                    this.radGridViewCompania.DataSource = this.dtCompaniasAsociadas; //SMR

                    this.radGridViewCompania.Visible = false;
                    this.lblNoExisteCompAsoc.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Rellena los controles con los datos de las compañías fiscales (modo edición)
        /// </summary>
        private void CargarInfoCompaniaFiscal()
        {
            IDataReader dr = null;
            try
            {
                //this.txtCodigo.Text = this.codigo;
                this.radButtonTextBoxCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                query += "where CIAFT3 = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtIdFiscal_DNI.Text = dr.GetValue(dr.GetOrdinal("NIFDT3")).ToString().Trim();
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBT3")).ToString().Trim();
                    this.txtTelefono.Text = dr.GetValue(dr.GetOrdinal("PRINT3")).ToString().Trim().TrimStart('0') +
                                            dr.GetValue(dr.GetOrdinal("TLINT3")).ToString().Trim();
                    this.txtApellidoNombre.Text = dr.GetValue(dr.GetOrdinal("APINT3")).ToString().Trim();

                    string ultAnoMesCerrado = dr.GetValue(dr.GetOrdinal("ULMCT3")).ToString().Trim();
                    if (ultAnoMesCerrado.Length == 5) ultAnoMesCerrado = ultAnoMesCerrado.Substring(1, 4);
                    else if (ultAnoMesCerrado == "0") ultAnoMesCerrado = "";
                    this.radMaskedEditBoxUltAnoMesCerrado.Value = ultAnoMesCerrado;
                    this.txtUltEjercFiscal.Text = dr.GetValue(dr.GetOrdinal("EJRCT3")).ToString().Trim();

                    this.txtSiglas.Text = dr.GetValue(dr.GetOrdinal("SIGLT3")).ToString().Trim();
                    this.txtNombreVia.Text = dr.GetValue(dr.GetOrdinal("VIAPT3")).ToString().Trim();
                    this.txtNumCasa.Text = dr.GetValue(dr.GetOrdinal("NCAST3")).ToString().Trim();
                    string cp = dr.GetValue(dr.GetOrdinal("POSTT3")).ToString().Trim();
                    if (cp != "") cp = cp.PadLeft(5, '0');
                    this.txtCP.Text = cp;
                    this.txtMunicipio.Text = dr.GetValue(dr.GetOrdinal("MUNCT3")).ToString().Trim();

                    string activarExtIVA = dr.GetValue(dr.GetOrdinal("APP1T3")).ToString().Trim();
                    if (activarExtIVA == "S") this.radToggleSwitchActivarExtIVA.Value = true;
                    else this.radToggleSwitchActivarExtIVA.Value = false;

                    string activarExtSII = dr.GetValue(dr.GetOrdinal("APP2T3")).ToString().Trim();
                    if (activarExtSII == "S") this.radToggleSwitchActivarExtSII.Value = true;
                    else this.radToggleSwitchActivarExtSII.Value = false;

                    string EPG2T3 = dr.GetValue(dr.GetOrdinal("EPG2T3")).ToString();
                    string caracter1 = " ";
                    string caracter2 = " ";
                    if (EPG2T3 != "")
                    { 
                        caracter1 = EPG2T3.Substring(0, 1);
                        if (EPG2T3.Length >=2) caracter2 = EPG2T3.Substring(1, 1);
                    }

                    switch (caracter1)
                    {
                        case "A":
                            this.rbAgenciaAEAT.IsChecked = true;
                            this.rbAgenciaCanarias.IsChecked = false;
                            this.rbAgenciaForalViz.IsChecked = false;
                            this.rbAgenciaIndefinido.IsChecked = false;
                            break;
                        case "C":
                            this.rbAgenciaAEAT.IsChecked = false;
                            this.rbAgenciaCanarias.IsChecked = true;
                            this.rbAgenciaForalViz.IsChecked = false;
                            this.rbAgenciaIndefinido.IsChecked = false;
                            break;
                        case "V":
                            this.rbAgenciaAEAT.IsChecked = false;
                            this.rbAgenciaCanarias.IsChecked = false;
                            this.rbAgenciaForalViz.IsChecked = true;
                            this.rbAgenciaIndefinido.IsChecked = false;
                            break;
                        case " ":
                        default:
                            this.rbAgenciaAEAT.IsChecked = false;
                            this.rbAgenciaCanarias.IsChecked = false;
                            this.rbAgenciaForalViz.IsChecked = false;
                            this.rbAgenciaIndefinido.IsChecked = true;
                            break;
                    }

                    switch (caracter2)
                    {
                        case "P":
                            this.rbTipoProduccion.IsChecked = true;
                            this.rbTipoTest.IsChecked = false;
                            this.rbTipoIndefinido.IsChecked = false;
                            break;
                        case "T":
                            this.rbTipoProduccion.IsChecked = false;
                            this.rbTipoTest.IsChecked = true;
                            this.rbTipoIndefinido.IsChecked = false;
                            break;
                        case " ":
                        default:
                            this.rbTipoProduccion.IsChecked = false;
                            this.rbTipoTest.IsChecked = false;
                            this.rbTipoIndefinido.IsChecked = true;
                            break;
                    }

                    string EPG1T3 = dr.GetValue(dr.GetOrdinal("EPG1T3")).ToString();
                    caracter1 = " ";
                    if (EPG1T3 != "") caracter1 = EPG1T3.Substring(0, 1);

                    switch (caracter1)
                    {
                        case "T":
                            this.rbPeriodoImpTrimestral.IsChecked = true;
                            this.rbPeriodoImpMensual.IsChecked = false;
                            break;
                        case "M":
                        default:
                            this.rbPeriodoImpTrimestral.IsChecked = false;
                            this.rbPeriodoImpMensual.IsChecked = true;
                            break;
                    }

                    //Cargar Info Compañias Asociadas
                    this.CargarInfoCompaniasAsociadas();
                }

                dr.Close();

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;
            string errores = "";

            if (this.txtIdFiscal_DNI.Text.Trim() == "")
            {
                errores += "- La identificación fiscal o DNI no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtIdFiscal_DNI.Focus();
            }

            if (this.txtNombre.Text.Trim() == "")
            {
                errores += "- La razón social o nombre no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtNombre.Focus();
            }

            if (this.txtTelefono.Text.Trim() == "")
            {
                errores += "- El teléfono no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtTelefono.Focus();
            }
            else
            {
                if (this.txtTelefono.Text.Length != 9)
                {
                    errores += "- El teléfono no tiene un formato correcto \n\r";      //Falta traducir
                    result = false;
                    this.txtTelefono.Focus();
                }
            }

            if (this.txtApellidoNombre.Text.Trim() == "")
            {
                errores += "- Los apellidos y nombres de la persona de contacto no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtApellidoNombre.Focus();
            }

            if (this.txtSiglas.Text.Trim() == "")
            {
                errores += "- Las siglas de la vía pública no pueden estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtNombreVia.Focus();
            }

            if (this.txtNombreVia.Text.Trim() == "")
            {
                errores += "- El nombre de la vía pública no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtNombreVia.Focus();
            }

            if (this.txtNumCasa.Text.Trim() == "")
            {
                errores += "- El número de la casa no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtNombreVia.Focus();
            }

            if (this.txtCP.Text.Trim() == "")
            {
                errores += "- El código postal no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtCP.Focus();
            }

            if (this.txtMunicipio.Text.Trim() == "")
            {
                errores += "- El municipio no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtMunicipio.Focus();
            }

            if (this.radGridViewCompania.Rows.Count == 0)
            {
                errores += "- No existen compañías contables asociadas \n\r";      //Falta traducir
                result = false;
                this.radButtonTextBoxCompania.Focus();
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Dar de alta a la compañía fiscal
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string STATT3 = " ";
                string CIAFT3 = this.codigo;
                string ULMCT3 = "0";
                string ultAnoMesCerrado = this.radMaskedEditBoxUltAnoMesCerrado.Value.ToString();
                if (ultAnoMesCerrado.Trim() != "")
                {
                    ULMCT3 = utiles.SigloDadoAnno(ultAnoMesCerrado.Substring(0,2), CGParametrosGrles.GLC01_ALSIRC) + ultAnoMesCerrado;
                }
                string DLGHT3 = "0";
                string ADMHT3 = "0";
                string NIFDT3 = this.txtIdFiscal_DNI.Text.Trim() == "" ? " " : this.txtIdFiscal_DNI.Text;
                string NOMBT3 = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;
                string SIGLT3 = this.txtSiglas.Text.Trim() == "" ? " " : this.txtSiglas.Text;
                string VIAPT3 = this.txtNombreVia.Text.Trim() == "" ? " " : this.txtNombreVia.Text;
                string NCAST3 = this.txtNumCasa.Text.Trim() == "" ? "0" : this.txtNumCasa.Text;
                string POSTT3 = this.txtCP.Text.Trim() == "" ? "0" : this.txtCP.Text;
                string MUNCT3 = this.txtMunicipio.Text.Trim() == "" ? " " : this.txtMunicipio.Text;
                string APP1T3 = this.radToggleSwitchActivarExtIVA.Value ? "S" : "N";
                string APP2T3 = this.radToggleSwitchActivarExtSII.Value ? "S" : "N";
                string EPG1T3 = "M";
                if (this.rbPeriodoImpTrimestral.IsChecked) EPG1T3 = "T";

                string EPG2T3 = " ";
                if (this.rbAgenciaAEAT.IsChecked) EPG2T3 = "A";
                else if (this.rbAgenciaCanarias.IsChecked) EPG2T3 = "C";
                else if (this.rbAgenciaForalViz.IsChecked) EPG2T3 = "V";

                if (this.rbTipoIndefinido.IsChecked) EPG2T3 += " ";
                else if (this.rbTipoProduccion.IsChecked) EPG2T3 += "P";
                else if (this.rbTipoTest.IsChecked) EPG2T3 += "T";

                string EJRCT3 = "0";

                string PRINT3 = " ";
                string TLINT3 = " ";
                
                string telefono = this.txtTelefono.Text.Trim();
                if (telefono.Length <= 2) PRINT3 = " " +telefono.Substring(0, telefono.Length);
                else
                {
                    PRINT3 = " " + telefono.Substring(0, 2);
                    TLINT3 = telefono.Substring(2, telefono.Length-2);
                }

                string APINT3 = this.txtApellidoNombre.Text.Trim() == "" ? " " : this.txtApellidoNombre.Text;
                string STPRT3 = "N";
                string AAMDT3 = "0";
                string AAMHT3 = "0";
                string START3 = "N";

                //Dar de alta a la compañía fiscal en tabla de compañías fiscales (IVT03)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVT03";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATT3, CIAFT3, ULMCT3, DLGHT3, ADMHT3, NIFDT3, NOMBT3, SIGLT3, VIAPT3, NCAST3, POSTT3, MUNCT3,";
                query += "APP1T3, EPG1T3, APP2T3, EPG2T3, EJRCT3, PRINT3, TLINT3, APINT3, STPRT3, AAMDT3, AAMHT3, START3) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + STATT3  + "', '" + CIAFT3 + "', " + ULMCT3 + ", " + DLGHT3 + ", " + ADMHT3 + ", ";
                query += "'" + NIFDT3 + "', '" + NOMBT3 + "', '" + SIGLT3 + "', '" + VIAPT3 + "', '" + NCAST3 + "', ";
                query += "'" + POSTT3  + "', '" + MUNCT3 + "', '" + APP1T3 + "', '" + EPG1T3 + "', '" + APP2T3 + "', ";
                query += "'" + EPG2T3  + "', " + EJRCT3 + ", '" + PRINT3 + "', '" + TLINT3 + "', '" + APINT3 + "', ";
                query += "'" + STPRT3  + "', " + AAMDT3 + ", " + AAMHT3 + ", '" + START3 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "IVT03", CIAFT3, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Actualizar la compañía fiscal
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string NIFDT3 = this.txtIdFiscal_DNI.Text.Trim() == "" ? " " : this.txtIdFiscal_DNI.Text;
                string NOMBT3 = this.txtNombre.Text.Trim() == "" ? " " : this.txtNombre.Text;
                string SIGLT3 = this.txtSiglas.Text.Trim() == "" ? " " : this.txtSiglas.Text;
                string VIAPT3 = this.txtNombreVia.Text.Trim() == "" ? " " : this.txtNombreVia.Text;
                string NCAST3 = this.txtNumCasa.Text.Trim() == "" ? "0" : this.txtNumCasa.Text;
                string POSTT3 = this.txtCP.Text.Trim() == "" ? "0" : this.txtCP.Text;
                string MUNCT3 = this.txtMunicipio.Text.Trim() == "" ? " " : this.txtMunicipio.Text;
                string PRINT3 = " ";
                string TLINT3 = " ";

                string telefono = this.txtTelefono.Text.Trim();
                if (telefono.Length <= 2) PRINT3 = " " + telefono.Substring(0, telefono.Length);
                else
                {
                    PRINT3 = " " + telefono.Substring(0, 2);
                    TLINT3 = telefono.Substring(2, telefono.Length - 2);
                }
                
                string APINT3 = this.txtApellidoNombre.Text.Trim() == "" ? " " : this.txtApellidoNombre.Text;

                string APP1T3 = this.radToggleSwitchActivarExtIVA.Value ? "S" : "N";
                string APP2T3 = this.radToggleSwitchActivarExtSII.Value ? "S" : "N";
                string EPG1T3 = "M";
                if (this.rbPeriodoImpTrimestral.IsChecked) EPG1T3 = "T";

                string EPG2T3 = " ";
                if (this.rbAgenciaAEAT.IsChecked) EPG2T3 = "A";
                else if (this.rbAgenciaCanarias.IsChecked) EPG2T3 = "C";
                else if (this.rbAgenciaForalViz.IsChecked) EPG2T3 = "V";

                if (this.rbTipoIndefinido.IsChecked) EPG2T3 += " ";
                else if (this.rbTipoProduccion.IsChecked) EPG2T3 += "P";
                else if (this.rbTipoTest.IsChecked) EPG2T3 += "T";

                string ULMCT3 = "0";
                string ultAnoMesCerrado = this.radMaskedEditBoxUltAnoMesCerrado.Value.ToString();
                if (ultAnoMesCerrado.Trim() != "")
                {
                    ULMCT3 = utiles.SigloDadoAnno(ultAnoMesCerrado.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + ultAnoMesCerrado;
                }

                //Actualizar la compañía fiscal en tabla de compañías fiscales (IVT03)          
                string query = "update " + GlobalVar.PrefijoTablaCG + "IVT03 set ";
                query += "ULMCT3 = " + ULMCT3 + ", ";
                query += "NIFDT3 = '" + NIFDT3 + "', NOMBT3 = '" + NOMBT3 + "', SIGLT3 = '" + SIGLT3 + "', VIAPT3 = '" + VIAPT3 + "', ";
                query += "NCAST3 = '" + NCAST3 + "', POSTT3 = '" + POSTT3 + "', MUNCT3 = '" + MUNCT3 + "', PRINT3 = '" + PRINT3 + "', ";
                query += "TLINT3 = '" + TLINT3 + "', APINT3 = '" + APINT3 + "', APP1T3 = '" + APP1T3 + "', APP2T3 = '" + APP2T3 + "', ";
                query += "EPG1T3 = '" + EPG1T3 + "', EPG2T3 = '" + EPG2T3 + "'";
                query += "where CIAFT3 = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "IVT03", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Valida que el código de compañía a asociar no exista en la lista, que sea un código de compañía válido y que no esté asociado a otra compañía fiscal
        /// </summary>
        /// <returns></returns>
        private string CodigoCompaniaAsociarValido(ref string nombre)
        {
            string result = "";
            nombre = "";
            IDataReader dr = null;
            try
            {
                string codCompania = this.radButtonTextBoxCompania.Text.Trim();

                if (codCompania != "")
                {
                    //Chequear que no exista en el listado de compañías asociadas
                    if (this.radGridViewCompania != null && this.radGridViewCompania.Rows.Count > 0)
                    {
                        string codCompAct = "";
                        for (int i = 0; i < this.radGridViewCompania.Rows.Count; i++)
                        {
                            codCompAct = this.radGridViewCompania.Rows[i].Cells["CCIAMG"].Value.ToString();

                            if (codCompAct == codCompania)
                            {
                                result = "La compañía ya está asociada";
                                return (result);
                            }
                            break;
                        }
                    }

                    //Chequear que el código de compañía sea válido
                    string query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                    query += "where CCIAMG = '" + codCompania + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nombre = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString();
                        dr.Close();
                    }
                    else
                    {
                        result = "Código de compañía no válido";
                        dr.Close();
                        return (result);
                    }
                    
                    //Verificar que no pertenezca a otra compañía fiscal
                    query = "select CIAFT2 from " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "where CCIAT2 = '" + codCompania + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    string compFiscalPertenece = "";
                    if (dr.Read())
                    {
                        compFiscalPertenece = dr.GetValue(dr.GetOrdinal("CIAFT2")).ToString();
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        return (result);
                    }

                    //Compañía contable pertenece a otra compañía fiscal. Verificar que que dicha compañía fiscal no tenga movimientos vigentes
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                    query += "where CIAFB3 = '" + compFiscalPertenece + "' and ";
                    query += "CCIAB3 = '" + codCompania + "' ";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        result = "Compañía contable está asociada a la compañía fiscal " + compFiscalPertenece + " que tiene movimientos vigentes"; //Falta traducir
                        return (result);
                    }

                    //Pedir confirmación para mover la compañía contable de una compañía fiscal a otra
                    //this.LP.GetText("wrnDeleteConfirm"       
                    string mensaje = "La compañía contable está asociada a la compañía fiscal " + compFiscalPertenece + ". Se va a mover la compañía contable a la compañía fiscal actual " + this.codigo;  //Falta traducir
                    mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                    DialogResult resultConf = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                    if (resultConf == DialogResult.Yes)
                    {
                        //Eliminar la asociación de la compañía contable con su actual compañía fiscal
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                        query += "where CCIAT2 = '" + codCompania + "' and ";
                        query += "CIAFT2 = '" + compFiscalPertenece +"'";

                        cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue));
                    }
                    else
                    {
                        result = "La compañía contable no se ha asociado";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            return (result);
        }

        /// <summary>
        /// Llamada a grabar una compañía fiscal
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                bool cerrarForm = true;

                if (this.nuevo)
                {
                    //this.codigo = this.txtCodigo.Text.Trim();
                    this.codigo = this.radButtonTextBoxCodigo.Text.Trim();
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = true;
                        this.radButtonTextBoxCompania.Select();
                        utiles.ButtonEnabled(ref this.radButtonDelete, true);
                        //SMR cerrarForm = false;
                        cerrarForm = true;
                    }
                }
                else result = this.ActualizarInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();
                    
                    //Cerrar el formulario
                    if (cerrarForm) this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a eliminar una compañía fiscal
        /// </summary>
        private void Eliminar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Pedir confirmación
            string mensaje = "Se va a eliminar la compañía fiscal " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Verificar que la Compañía fiscal no tenga movimientos vigentes
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                    query += "where CIAFB3 = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrCompFiscalMovVigentes", "La compañía fiscal tiene movimientos vigentes"), this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                        return;
                    }

                    //Eliminar las asociaciones de las compañías contables con la compañía fiscal
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT02 ";
                    query += "where CIAFT2 = '" + this.codigo + "'";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue));

                    //Eliminar la compañía fiscal
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
                    query += "where CIAFT3 = '" + this.codigo + "'";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        mensaje = "No fue posible eliminar la compañía fiscal.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Cerrar el formulario
                        this.Close();
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            //this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radButtonTextBoxCodigo.Tag = this.radButtonTextBoxCodigo.Text;
            this.txtIdFiscal_DNI.Tag = this.txtIdFiscal_DNI.Text;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtTelefono.Tag = this.txtTelefono.Text;
            this.txtApellidoNombre.Tag = this.txtApellidoNombre.Text;
            this.txtSiglas.Tag = this.txtSiglas.Text;
            this.txtNombreVia.Tag = this.txtNombreVia.Text;
            this.txtNumCasa.Tag = this.txtNumCasa.Text;
            this.txtCP.Tag = this.txtCP.Text;
            this.txtMunicipio.Tag = this.txtMunicipio.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            //this.txtCodigo.Tag = "";
            this.radButtonTextBoxCodigo.Tag = "";
            this.txtIdFiscal_DNI.Tag = "";
            this.txtNombre.Tag = "";
            this.txtTelefono.Tag = "";
            this.txtApellidoNombre.Tag = "";
            this.txtSiglas.Tag = "";
            this.txtNombreVia.Tag = "";
            this.txtNumCasa.Tag = "";
            this.txtCP.Tag = "";
            this.txtMunicipio.Tag = this.txtMunicipio.Text;
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewCompania.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewCompania.Columns.Contains(item.Key)) this.radGridViewCompania.Columns[item.Key].HeaderText = item.Value;
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

                campo = "CCIAMG";
                header = "Compañía";
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[0].HeaderText = header;

                campo = "NCIAMG";
                header = "Nombre";
                this.displayNames.Add(campo, header);
                //this.radGridViewCuentasAux.Columns[1].HeaderText = header;
            }
            catch
            {
            }
        }

        private void CentrarForm()
        {
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundHeight = Screen.PrimaryScreen.Bounds.Height - this.AutoScrollPosition.Y;
            int x = boundWidth - this.Width;
            int y = boundHeight - this.Height;
            this.Location = new Point(x / 2, y / 2);
        }
        #endregion

        private void radButtonTextBoxCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
        }

        private void radButtonTextBoxCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codCompFiscal = this.radButtonTextBoxCodigo.Text.Trim();

            if (codCompFiscal == "")
            {
                this.HabilitarDeshabilitarControles(false);
                this.radButtonTextBoxCodigo.Text = "";
                this.radButtonTextBoxCodigo.Focus();

                RadMessageBox.Show("Código de compañía obligatorio", this.LP.GetText("errValCodCompFiscal", "Error"));  //Falta traducir
                bTabulador = false;
                return;
            }

            bool codCompaniaOk = true;
            bool codCompaniaGLM01Ok = true;
            if (this.nuevo)
            {
                codCompaniaOk = this.CodigoCompaniaFiscalValido();    //Verificar que el codigo no exista
                if(codCompaniaOk) codCompaniaGLM01Ok = this.CodigoCompaniaFiscalExiste();    //Verificar que el codigo sea de una compañia existente
            }
            if (codCompaniaOk && codCompaniaGLM01Ok)
            {
                this.HabilitarDeshabilitarControles(true);

                this.radButtonTextBoxCodigo.ReadOnly = true;
                this.radButtonElementradButtonCompania.Enabled = false;
                //this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = false;
                this.radCollapsiblePanelCompaniasContablesAsociadas.Enabled = true;

                utiles.ButtonEnabled(ref this.radButtonSave, true);

                txtIdFiscal_DNI.Focus();
            }
            else
            {
                this.HabilitarDeshabilitarControles(false);
                this.radButtonTextBoxCodigo.Focus();
                if(codCompaniaOk == false) RadMessageBox.Show("Código de compañía fiscal ya existe", this.LP.GetText("errValCodCompFiscalExiste", "Error"));  //Falta traducir
                if(codCompaniaGLM01Ok == false) RadMessageBox.Show("Código de compañía no existe", this.LP.GetText("errValCodCompNoExiste", "Error"));  //Falta traducir
                bTabulador = false;
            }
            bTabulador = false;

            this.codigo = this.radButtonTextBoxCodigo.Text.Trim();

            //Cargar Info Compañias Asociadas
            this.CargarInfoCompaniasAsociadas();
        }

        private void radButtonElementradButtonCompania_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CCIAMG, NCIAMG, TIPLMG from ";
            query += GlobalVar.PrefijoTablaCG + "GLM01 ";
            query += "where STATMG='V' ";
            query += "order by CCIAMG";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción"),
                this.LP.GetText("lblGLM01Plan", "Plan de cuentas")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar compañias",
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

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
                //utiles.ButtonEnabled(ref this.radButtonAddCompania, true);
            }
            this.radButtonTextBoxCodigo.Text = result;
            this.ActiveControl = this.radButtonTextBoxCodigo;
            this.radButtonTextBoxCodigo.Select(0, 0);
            this.radButtonTextBoxCodigo.Focus();
        }

        private void radMaskedEditBoxUltAnoMesCerrado_Leave(object sender, EventArgs e)
        {
            if (bCancelar == true && bTabulador == false) return;

            string ultAnoMesCerrado = this.radMaskedEditBoxUltAnoMesCerrado.Value.ToString();

            if (ultAnoMesCerrado == "") return;

            if (ultAnoMesCerrado.Length != 4)
            {
                RadMessageBox.Show("Último año-mes cerrado no es correcto", this.LP.GetText("errValFechaCierre", "Error"));
                this.radMaskedEditBoxUltAnoMesCerrado.Focus();
                return;
            }

            int mes = Convert.ToInt32(ultAnoMesCerrado.Substring(2, 2));
            if((mes < 1) || (mes > 12))
            {
                RadMessageBox.Show("Último año-mes cerrado no es correcto", this.LP.GetText("errValFechaCierre", "Error"));
                this.radMaskedEditBoxUltAnoMesCerrado.Focus();
                bTabulador = false;
                return;
            }
            bTabulador = false;
        }

        private void txtIdFiscal_DNI_Leave(object sender, EventArgs e)
        {
            if (bCancelar == true && bTabulador == false) return;

            bool nifCorrecto = false;
            string resultValidarNIF = "";
            if (CheckNif.Check(txtIdFiscal_DNI.Text.ToString(), ref resultValidarNIF)) nifCorrecto = true;

            if (!nifCorrecto)
            {
                RadMessageBox.Show("Id Fiscal o DNI no válido", this.LP.GetText("errValNIF", "Error"));
                this.txtIdFiscal_DNI.Focus();
                bTabulador = false;
                return;
            }
            bTabulador = false;
        }
    }
}
