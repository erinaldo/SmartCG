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
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM07 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOGRPCIA";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "001";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "20";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;
        public Boolean bGrabar = false;

        private DataTable dtCompaniasAsociadas;
        private DataTable dtGruposAsociados;

        Dictionary<string, string> displayNames;
        Dictionary<string, string> displayNamesGrupos;

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

        public frmMtoGLM07()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.gbCompanias.ElementTree.EnableApplicationThemeName = false;
            this.gbCompanias.ThemeName = "ControlDefault";

            this.gbGrupos.ElementTree.EnableApplicationThemeName = false;
            this.gbGrupos.ThemeName = "ControlDefault";

            //this.radGridViewCompania.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM07_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Grupo de Compañías Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC'
            this.KeyPreview = true;

            this.TraducirLiterales();

            this.BuildDisplayNames();

            this.BuildDisplayNamesGrupos();

            utiles.ButtonEnabled(ref this.radButtonAddCompania, false);
            utiles.ButtonEnabled(ref this.radButtonAddGrupo, false);

            if (this.nuevo)
            {
                this.autEditar = true;
                this.gbCompanias.Enabled = false;
                this.gbGrupos.Enabled = false;

                this.radGridViewGrupo.Visible = false;
                this.radGridViewCompania.Visible = false;

                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                this.gbCompanias.Enabled = true;
                this.gbGrupos.Enabled = true;

                //Recuperar la información de la compañía y mostrarla en los controles
                this.CargarInfoGrupoCompanias();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }
            }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codGrupo = this.txtCodigo.Text.Trim();

                if (codGrupo == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Código de grupo obligatorio", this.LP.GetText("errValCodGrupo", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;

                }

                bool codGrupoOk = true;
                if (this.nuevo) codGrupoOk = this.CodigoGrupoValido();    //Verificar que el codigo no exista

                if (codGrupoOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                    }
                    this.txtCodigo.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de grupo ya existe", this.LP.GetText("errValCodGrupoExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
                bTabulador = false;

                ////
                this.codigo = this.txtCodigo.Text.Trim();
                //Cargar Info Compañias Asociadas
                this.CargarInfoCompaniasAsociadas();

                this.radGridViewCompania.CurrentRow = null;

                //Cargar Info Grupos Compañias Asociados
                this.CargarInfoGruposAsociados();

                this.radGridViewGrupo.CurrentRow = null;
                ////
            }
        }

        private void BtnAddCompania_Click(object sender, EventArgs e)
        {
            this.AdicionarCompania();
        }

        private void BtnAdicionarGrupo_Click(object sender, EventArgs e)
        {
            this.AdicionarGrupo();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonElementradButtonCompania_Click(object sender, EventArgs e)
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
            this.radButtonTextBoxCompania.Text = result;
            this.ActiveControl = this.radButtonTextBoxCompania;
            this.radButtonTextBoxCompania.Select(0, 0);
            this.radButtonTextBoxCompania.Focus();
        }

        private void RadButtonAddCompania_Click(object sender, EventArgs e)
        {
            this.AdicionarCompania();
        }

        private void RadButtonAddGrupo_Click(object sender, EventArgs e)
        {
            this.AdicionarGrupo();
        }

        private void RadButtonTextBoxCompania_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxCompania.Text.Trim() == "") utiles.ButtonEnabled(ref this.radButtonAddCompania, false);
            else utiles.ButtonEnabled(ref this.radButtonAddCompania, true);
        }

        private void RadButtonTextBoxGrupo_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxGrupo.Text.Trim() == "") utiles.ButtonEnabled(ref this.radButtonAddGrupo, false);
            else utiles.ButtonEnabled(ref this.radButtonAddGrupo, true);
        }

        private void RadButtonElementGrupo_Click(object sender, EventArgs e)
        {
            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {

                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar grupo de compañias",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG
            };
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select GRUPGR, NOMBGR from ";
            query += GlobalVar.PrefijoTablaCG + "GLM07 ";
            query += "where STATGR='V' ";
            query += "order by GRUPGR";
            frmElementosSel.Query = query;

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };
            frmElementosSel.ColumnasCaption = nombreColumnas;
            //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
            frmElementosSel.FrmPadre = this;
            frmElementosSel.ShowDialog();

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
                //utiles.ButtonEnabled(ref this.radButtongr, true);
            }
            this.radButtonTextBoxGrupo.Text = result;
            this.ActiveControl = this.radButtonTextBoxGrupo;
            this.radButtonTextBoxGrupo.Select(0, 0);
            this.radButtonTextBoxGrupo.Focus();
        }

        private void RadGridViewCompania_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (sender is Telerik.WinControls.UI.GridImageCellElement imageCellElement)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (this.radGridViewCompania.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.radGridViewCompania.Rows.Count)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        ///Eliminar la entrada del DataSet
                        this.dtCompaniasAsociadas.Rows.RemoveAt(e.RowIndex);

                        if (this.radGridViewCompania.Rows.Count == 0)
                        {
                            this.radGridViewCompania.Visible = false;
                            this.lblNoExisteCompAsoc.Visible = true;
                        }

                        this.radGridViewCompania.CurrentRow = null;

                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void RadGridViewGrupo_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (sender is Telerik.WinControls.UI.GridImageCellElement imageCellElement)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (this.radGridViewGrupo.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.radGridViewGrupo.Rows.Count)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        string grupo = this.radGridViewGrupo.Rows[e.RowIndex].Cells["GRINAJ"].Value.ToString();

                        //Eliminar la entrada del DataSet
                        this.dtGruposAsociados.Rows.RemoveAt(e.RowIndex);

                        if (this.radGridViewGrupo.Rows.Count == 0)
                        {
                            this.radGridViewGrupo.Visible = false;
                            this.lblNoExisteGrupoAsoc.Visible = true;
                        }

                        this.radGridViewGrupo.CurrentRow = null;

                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
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

        private void RadButtonAddCompania_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAddCompania);
        }

        private void RadButtonAddCompania_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAddCompania);
        }

        private void RadButtonAddGrupo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAddGrupo);
        }

        private void RadButtonAddGrupo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAddGrupo);
        }

        private void FrmMtoGLM07_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLM07_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    //this.radToggleSwitchEstadoActiva.Value.ToString() != this.radToggleSwitchEstadoActiva.Tag.ToString() ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() 
                    )
                {
                    if (bGrabar == true) return;  //jl

                    string mensaje = this.LP.GetText("ConfirmarCambio", "Se han detectado cambios en el formulario, ¿desea salir?");
                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        //this.radButtonSave.PerformClick();
                        e.Cancel = false;
                        cerrarForm = false;
                    }
                    else if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    // else e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Grupo de Compañías Alta/Edita");
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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM07TituloALta", "Mantenimiento de Grupos de Compañías - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM07TituloEdit", "Mantenimiento de Grupos de Compañías - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonExit.Text = this.LP.GetText("toolStripSalir", "Cancelar");
            this.radButtonAddCompania.Text = this.LP.GetText("lblfrmMtoGLM07AddCompania", "Adicionar Compañía");

            //Traducir los campos del formulario
            this.lblCodigo.Text = this.LP.GetText("lblGLM07GrupoCompania", "Grupo de compañías");
            //this.lblEstado.Text = this.LP.GetText("lblEstado", "Activo");
            this.lblNombre.Text = this.LP.GetText("lblNombre", "Nombre");
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.gbCompanias.Enabled = valor;
            this.gbGrupos.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La compañía está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNombre.IsReadOnly = true;
            this.gbCompanias.Enabled = false;
            this.gbGrupos.Enabled = false;
        }

        /// <summary>
        /// Carga la información de la lista de compañías asociadas
        /// </summary>
        private void CargarInfoCompaniasAsociadas()
        {
            try
            {
                string query = "select grupocomp.CCIAAI, compania.NCIAMG ";

                if (this.tipoBaseDatosCG == "Oracle") query += "from " + GlobalVar.PrefijoTablaCG + "GLM08 grupocomp, " + GlobalVar.PrefijoTablaCG + "GLM01 compania ";
                else query += "from " + GlobalVar.PrefijoTablaCG + "GLM08 as grupocomp, " + GlobalVar.PrefijoTablaCG + "GLM01 as compania ";

                query += "where grupocomp.GRUPAI = '" + this.codigo + "' and ";
                query += "(grupocomp.ORIGAI = '' or grupocomp.ORIGAI = ' ' or grupocomp.ORIGAI = '  ') and ";
                query += "compania.CCIAMG = grupocomp.CCIAAI and compania.STATMG = 'V' ";
                query += "order by grupocomp.CCIAAI";

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
                    this.radGridViewCompania.CurrentRow = null;
                    this.lblNoExisteCompAsoc.Visible = false;
                }
                else
                {
                    this.radGridViewCompania.DataSource = this.dtCompaniasAsociadas;

                    //if (this.radGridViewCompania.Rows != null && this.radGridViewCompania.Rows.Count > 0)
                    //{
                        this.RadGridViewHeader();

                        for (int i = 0; i < this.radGridViewCompania.Columns.Count; i++)
                        {
                            this.radGridViewCompania.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewCompania.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewCompania.MasterTemplate.BestFitColumns();
                    //}

                    this.radGridViewCompania.Visible = false;
                    this.radGridViewCompania.CurrentRow = null;
                    this.lblNoExisteCompAsoc.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información de la lista de grupos asociados
        /// </summary>
        private void CargarInfoGruposAsociados()
        {
            try
            {
                //cambiado por indicacion de LIS:
                //-nuevo-
                //select grupogrupo.GRINAJ, grupo.NOMBGR 
                //from TIGSA_GLM09 grupogrupo, TIGSA_GLM07 grupo 
                //where grupogrupo.GRSPAJ = 'LL' and grupo.GRUPGR = grupogrupo.GRSPAJ order by grupogrupo.GRINAJ
                //-antes-
                //string query = "select distinct(grupocomp.ORIGAI), grupo.NOMBGR ";
                //
                //if (this.tipoBaseDatosCG == "Oracle") query += "from " + GlobalVar.PrefijoTablaCG + "GLM08 grupocomp, " + GlobalVar.PrefijoTablaCG + "GLM07 grupo ";
                //else query += "from " + GlobalVar.PrefijoTablaCG + "GLM08 as grupocomp, " + GlobalVar.PrefijoTablaCG + "GLM07 as grupo ";
                //
                //query += "where grupocomp.GRUPAI = '" + this.codigo + "' and ";
                //query += "(grupocomp.ORIGAI <> '' and grupocomp.ORIGAI <> ' ' and grupocomp.ORIGAI <> '  ') and ";
                //query += "grupo.GRUPGR = grupocomp.ORIGAI ";
                //query += "order by grupocomp.ORIGAI";

                string query = "select grupogrupo.GRINAJ, grupo.NOMBGR ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLM09 as grupogrupo, " + GlobalVar.PrefijoTablaCG + "GLM07 as grupo ";
                query += "where grupogrupo.GRSPAJ = '" + this.codigo + "' and ";
                query += "grupo.GRUPGR = grupogrupo.GRSPAJ order by grupogrupo.GRINAJ";

                this.dtGruposAsociados = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                this.dtGruposAsociados.Columns.Add("Eliminar", typeof(Image));

                if (this.dtGruposAsociados != null && this.dtGruposAsociados.Rows.Count > 0)
                {
                    //SMR this.dtGruposAsociados.Columns.Add("Eliminar", typeof(Image));
                    for (int i = 0; i < this.dtGruposAsociados.Rows.Count; i++)
                    {
                        this.dtGruposAsociados.Rows[i]["Eliminar"] = global::ModMantenimientos.Properties.Resources.Eliminar;
                    }
                    this.radGridViewGrupo.DataSource = this.dtGruposAsociados;

                    if (this.radGridViewGrupo.Rows != null && this.radGridViewGrupo.Rows.Count > 0)
                    {
                        this.RadGridViewHeaderGrupo();

                        for (int i = 0; i < this.radGridViewGrupo.Columns.Count; i++)
                        {
                            this.radGridViewGrupo.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewGrupo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewGrupo.MasterTemplate.BestFitColumns();
                    }
                    this.radGridViewGrupo.Visible = true;
                    this.radGridViewGrupo.CurrentRow = null;
                    this.lblNoExisteGrupoAsoc.Visible = false;
                }
                else
                {
                    this.radGridViewGrupo.DataSource = this.dtGruposAsociados;

                    //if (this.radGridViewGrupo.Rows != null && this.radGridViewGrupo.Rows.Count > 0)
                    //{
                    this.RadGridViewHeaderGrupo();

                        for (int i = 0; i < this.radGridViewGrupo.Columns.Count; i++)
                        {
                            this.radGridViewGrupo.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewGrupo.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewGrupo.MasterTemplate.BestFitColumns();
                    //}

                    this.radGridViewGrupo.Visible = false;
                    this.radGridViewGrupo.CurrentRow = null;
                    this.lblNoExisteGrupoAsoc.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Rellena los controles con los datos de la compañía (modo edición)
        /// </summary>
        private void CargarInfoGrupoCompanias()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;

                string estado = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM07 ";
                query += "where GRUPGR = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBGR")).ToString().Trim();

                    estado = dr.GetValue(dr.GetOrdinal("STATGR")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;

                    //Cargar Info Compañias Asociadas
                    this.CargarInfoCompaniasAsociadas();

                    this.radGridViewCompania.CurrentRow = null;

                    //Cargar Info Grupos Compañias Asociados
                    this.CargarInfoGruposAsociados();

                    this.radGridViewGrupo.CurrentRow = null;
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
        /// Valida que no exista el código del grupo
        /// </summary>
        /// <returns></returns>
        private bool CodigoGrupoValido()
        {
            bool result = false;

            try
            {
                string codGrupo = this.txtCodigo.Text.Trim();

                if (codGrupo != "")
                {
                    string query = "select count(GRUPGR) from " + GlobalVar.PrefijoTablaCG + "GLM07 ";
                    query += "where GRUPGR = '" + codGrupo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;
            string errores = "";

            if (this.txtNombre.Text.Trim() == "")
            {
                errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                result = false;
                this.txtNombre.Focus();
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);

        }

        /// <summary>
        /// Dar de alta a un grupo
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                //Dar de alta al grupo en la tabla de maestros de grupos (GLM07)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM07";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATGR, GRUPGR, NOMBGR) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtNombre.Text + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM07", this.codigo, null);

                //Insertar al usuario como propietario del elemento
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + this.codigo + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Actualizar un grupo
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM07 set ";
                query += "STATGR = '" + estado + "', NOMBGR = '" + this.txtNombre.Text + "' ";
                query += "where GRUPGR = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM07", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Valida que el código de compañía a asociar no exista en la lista y que sea un código de compañía válido
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
                    if (this.radGridViewCompania != null && this.radGridViewCompania.Rows != null && this.radGridViewCompania.Rows.Count > 0)
                    {
                        string codCompAct = "";
                        for (int i = 0; i < this.radGridViewCompania.Rows.Count; i++)
                        {
                            codCompAct = this.radGridViewCompania.Rows[i].Cells["CCIAAI"].Value.ToString();

                            if (codCompAct == codCompania)
                            {
                                result = "La compañía ya está asociada";
                                return (result);
                            }
                        }
                    }

                    //Chequear que el código de compañía sea válido
                    string query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                    query += "where CCIAMG = '" + codCompania + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nombre = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString();
                    }
                    else result = "Código de compañía no válido";

                    dr.Close();
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
        /// Valida que el código de grupo a asociar no exista en la lista y que sea un código de grupo válido
        /// </summary>
        /// <returns></returns>
        private string CodigoGrupoAsociarValido(ref string nombre)
        {
            string result = "";
            nombre = "";
            IDataReader dr = null;
            try
            {
                string codGrupo = this.radButtonTextBoxGrupo.Text.Trim();

                if (codGrupo != "")
                {
                    //Chequear que no exista en el listado de grupos asociadas
                    //if (this.dtGruposAsociados != null && this.dtGruposAsociados.Rows.Count > 0)
                    if (this.radGridViewGrupo != null && this.radGridViewGrupo.Rows != null && this.radGridViewGrupo.Rows.Count > 0)
                    {
                        string codGrupoAct = "";
                        for (int i = 0; i < this.radGridViewGrupo.Rows.Count; i++)
                        {
                            //codGrupoAct = this.radGridViewGrupo.Rows[i].Cells["ORIGAI"].Value.ToString();
                            codGrupoAct = this.radGridViewGrupo.Rows[i].Cells["GRINAJ"].Value.ToString();

                            if (codGrupoAct == codGrupo)
                            {
                                result = "El grupo ya está asociado";
                                return (result);
                            }
                        }
                    }

                    //Chequear que el código de grupo sea válido
                    string query = "select GRUPGR, NOMBGR from " + GlobalVar.PrefijoTablaCG + "GLM07 ";
                    query += "where GRUPGR = '" + codGrupo + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nombre = dr.GetValue(dr.GetOrdinal("NOMBGR")).ToString();
                    }
                    else result = "Código de grupo no válido";

                    dr.Close();
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
        /// Actualizar GLM08
        /// </summary>
        /// <returns></returns>
        private string ActualizarGLM08Cia()
        {
            IDataReader dr = null;

            string result = "";
            
            //borrar registros de GLM08
            string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM08 where ";
            query += " GRUPAI = '" + this.codigo + "' and (ORIGAI = '' or ORIGAI = ' ' or ORIGAI = '  ')";

            int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            //borrar registros de los grupos asociados
            query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM08 where ";
            query += " ORIGAI = '" + this.codigo +"'";

            registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            //insertar registros en GLM08
            //Buscar todas los grupos que tiene a este grupo como asociado
            query = "select GRSPAJ from " + GlobalVar.PrefijoTablaCG + "GLM09 ";
            query += "where GRINAJ = '" + this.codigo + "'";

            string codGrupoPadre = "";
            int cantGrupoPadre = 0;

            ArrayList acodGrupoPadre = new ArrayList();

            dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

            while (dr.Read())
            {
                codGrupoPadre = dr.GetValue(dr.GetOrdinal("GRSPAJ")).ToString();
                acodGrupoPadre.Add(codGrupoPadre);
                cantGrupoPadre++;
            }
            dr.Close();
            string compania = "";

            for (int i = 0; i < this.radGridViewCompania.Rows.Count; i++)
            {
                compania = this.radGridViewCompania.Rows[i].Cells["CCIAAI"].Value.ToString();
                //Insertar la compañía a la lista de compañías asociadas 
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM08";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "GRUPAI, CCIAAI, ORIGAI) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigo + "', '" + compania + "', ' ')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM08", this.codigo, compania);

                //insertar las compañías asociadas en los grupos que tienen a este como grupo asociado
                if(cantGrupoPadre != 0)
                {
                    for (int j = 0; j < acodGrupoPadre.Count; j++)
                    {
                        //Insertar el grupo asociado 
                        query = "insert into " + nombreTabla + " (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                        query += "GRUPAI, CCIAAI, ORIGAI) values (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                        query += "'" + acodGrupoPadre[j].ToString() + "', '" + compania + "', '" + this.codigo + "')";

                        registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM08", this.codigo, acodGrupoPadre[j].ToString());
                    }
                }
            }
            return (result);
        }

        /// <summary>
        /// Actualizar GLM09
        /// </summary>
        /// <returns></returns>
        private string ActualizarGLM08Grupo()
        {
            IDataReader dr = null;

            string result = "";

            //borrar registros de GLM08
            string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM08 where ";
            query += " GRUPAI = '" + this.codigo + "' and (ORIGAI <> '' and ORIGAI <> ' ' or ORIGAI <> '  ')";

            int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            //borrar registros de GLM09
            query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM09 where ";
            query += " GRSPAJ = '" + this.codigo + "'";

            registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            //insertar registros en GLM08
            string codigoGrupoAsoc = "";

            for (int i = 0; i < this.radGridViewGrupo.Rows.Count; i++)
            {
                codigoGrupoAsoc = this.radGridViewGrupo.Rows[i].Cells["GRINAJ"].Value.ToString();

                //Buscar todas las compañías que están asociadas al grupo que se desea asociar
                query = "select CCIAAI from " + GlobalVar.PrefijoTablaCG + "GLM08 ";
                query += "where GRUPAI = '" + codigoGrupoAsoc + "'";

                string codCompania = "";
                int cantCompanias = 0;

                ArrayList acodCompania = new ArrayList();

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    codCompania = dr.GetValue(dr.GetOrdinal("CCIAAI")).ToString();
                    acodCompania.Add(codCompania);
                    cantCompanias++;
                }
                dr.Close();

                if (cantCompanias > 0)
                {
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM08";

                    for (int j = 0; j < acodCompania.Count; j++)
                    {
                        //Insertar el grupo asociado 
                        query = "insert into " + nombreTabla + " (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                        query += "GRUPAI, CCIAAI, ORIGAI) values (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                        query += "'" + this.codigo + "', '" + acodCompania[j].ToString() + "', '" + codigoGrupoAsoc + "')";

                        registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM08", this.codigo, acodCompania[j].ToString());
                    }

                    //Insertar grupo en GLM09
                    string nombreTablaGLM09 = GlobalVar.PrefijoTablaCG + "GLM09";

                    query = "insert into " + nombreTablaGLM09 + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTablaGLM09 + ", ";
                    query += "GRSPAJ, GRINAJ) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTablaGLM09 + ".nextval, ";
                    query += "'" + this.codigo + "', '" + codigoGrupoAsoc + "')";

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

            }
            return (result);
        }

        /// <summary>
        /// Graba un Grupo de compañías
        /// </summary>
        private void Grabar()
        {
            bGrabar = false;
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string mensaje = this.LP.GetText("wrnGrpCIA", "Se va a actualizar el Grupo de Compañias") + " " + this.codigo.Trim();
                mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                DialogResult yesno = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

                if (yesno == DialogResult.No)
                {
                    //Cerrar el formulario
                    bGrabar = true;
                    this.Close();
                }
                if (yesno == DialogResult.Yes)
                {
                    string result = "";
                    bGrabar = true;

                    if (this.nuevo)
                    {
                        this.codigo = this.txtCodigo.Text.Trim();
                        result = this.AltaInfo();
                        //if (result == "") this.nuevo = false;
                    }
                    else result = this.ActualizarInfo();

                //Actualizar tablas GLM08 y GLM09
                    result = this.ActualizarGLM08Cia();
                    result = this.ActualizarGLM08Grupo();

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
                        this.Close();
                    }
                }

                Cursor.Current = Cursors.Default;
                bGrabar = false;
            }
        }

        /// <summary>
        /// Adiciona una compañía al Grupo de compañías
        /// </summary>
        private void AdicionarCompania()
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

                string compania = this.radButtonTextBoxCompania.Text.Trim();

                //Insertar la compañía en el DataTable de compañías asociadas para que aparezca en la grid
                DataRow row = this.dtCompaniasAsociadas.NewRow();

                row["CCIAAI"] = this.radButtonTextBoxCompania.Text.Trim();
                row["NCIAMG"] = nombre;
                row["Eliminar"] = global::ModMantenimientos.Properties.Resources.Eliminar;

                this.dtCompaniasAsociadas.Rows.Add(row);

                this.lblNoExisteCompAsoc.Visible = false;
                this.radGridViewCompania.Visible = true;

                this.radButtonTextBoxCompania.Text = "";
                utiles.ButtonEnabled(ref this.radButtonAddCompania, false);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Adiciona un grupo al Grupo de compañías
        /// </summary>
        private void AdicionarGrupo()
        {
            //IDataReader dr = null;
            try
            {
                string nombre = "";
                string result = this.CodigoGrupoAsociarValido(ref nombre);
                if (result != "")
                {
                    RadMessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
                    this.radButtonTextBoxGrupo.Focus();
                    return;
                }

                string codigoGrupoAsoc = this.radButtonTextBoxGrupo.Text.ToString();

                //no permitir el mismo grupo
                if (this.codigo == codigoGrupoAsoc)
                {
                    RadMessageBox.Show("No se puede asociar el mismo grupo.", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    this.radButtonTextBoxGrupo.Focus();
                    return;
                }

                //Validar jerarquias de grupo:  1- grupo asociado no puede tener a su vez grupos asociados (GRSPAJ de GLM09).
                //                              2- si el grupo ya esta sociado a otro grupo no podra tener grupos asociados (GRINAG de GLM09).
                string query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "GLM09 ";
                query += "where GRSPAJ = '" + codigoGrupoAsoc + "'";
                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (cantRegistros != 0)
                {
                    RadMessageBox.Show("El grupo " + codigoGrupoAsoc + " tiene grupos asociados.", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    this.radButtonTextBoxGrupo.Focus();
                    return;
                }
                query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "GLM09 ";
                query += "where GRINAJ = '" + this.codigo + "'";
                cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (cantRegistros != 0)
                {
                    RadMessageBox.Show("El grupo " + this.codigo + " está asociado a otro grupo.", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    this.radButtonTextBoxGrupo.Focus();
                    return;
                }

                //Verificar que el grupo tenga compañias asociadas
                query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM08 ";
                query += "where GRUPAI = '" + codigoGrupoAsoc + "'";

                cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                if (cantRegistros == 0)
                {
                    RadMessageBox.Show("El grupo " + codigoGrupoAsoc + " no tiene compañía asociada, no es posible añadirlo", this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                    this.radButtonTextBoxGrupo.Focus();
                    return;
                }

                //Insertar el grupo en el DataSet de grupos asociadas para que aparezca en la grid
                DataRow row = this.dtGruposAsociados.NewRow();

                row["GRINAJ"] = codigoGrupoAsoc;
                row["NOMBGR"] = nombre;
                row["Eliminar"] = global::ModMantenimientos.Properties.Resources.Eliminar;

                this.dtGruposAsociados.Rows.Add(row);

                this.lblNoExisteGrupoAsoc.Visible = false;
                this.radGridViewGrupo.Visible = true;

                this.radButtonTextBoxGrupo.Text = "";
                utiles.ButtonEnabled(ref this.radButtonAddGrupo, false);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
            
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;
            this.txtNombre.Tag = "";
            
        }

        private void RadGridViewHeaderGrupo()
        {
            try
            {
                if (this.radGridViewGrupo.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesGrupos)
                    {
                        if (this.radGridViewGrupo.Columns.Contains(item.Key)) this.radGridViewGrupo.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch
            {
            }
        }

        private void BuildDisplayNamesGrupos()
        {
            try
            {
                this.displayNamesGrupos = new Dictionary<string, string>();

                string campo = "";
                string header = "";

                //campo = "ORIGAI";
                campo = "GRINAJ";
                header = "Grupo";
                this.displayNamesGrupos.Add(campo, header);

                campo = "NOMBGR";
                header = "Nombre";
                this.displayNamesGrupos.Add(campo, header);

                campo = "Eliminar";
                header = "Eliminar";
                this.displayNamesGrupos.Add(campo, header);
            }
            catch
            {
            }
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

                campo = "CCIAAI";
                header = "Compañía";
                this.displayNames.Add(campo, header);

                campo = "NCIAMG";
                header = "Nombre";
                this.displayNames.Add(campo, header);

                campo = "Eliminar";
                header = "Eliminar";
                this.displayNames.Add(campo, header);
            }
            catch
            {
            }
        }
        #endregion

        private void frmMtoGLM07_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.txtNombre.Text.Trim() == "")
            {
                RadMessageBox.Show("- El nombre no puede estar en blanco");
                bTabulador = false;
                this.txtNombre.Focus();
            }
            bTabulador = false;
            Cursor.Current = Cursors.Default;
        }

        private void txtNombre_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }
    }
}
