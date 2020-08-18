using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace ModMantenimientos
{
    public partial class frmMtoAutElementos : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOAUTELE";

        private string codigo;
        private string codigoUsuario;

        private string prefijoTabla = "";

        private DataTable dtClases = new DataTable();
        private DataTable dtElementos = new DataTable();
        private DataTable dtGrupoOperac = new DataTable();
        private DataTable dtUsuarios = new DataTable();
        private DataTable dtResultadoBusqueda = new DataTable();
        private DataTable dtResultadoBusquedaAux = new DataTable();

        //private DataTable dtOperaciones = new DataTable();

        private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(50, 130);
        private static Size gridInfoSize = new Size(868, 250); 
        private static int radCollapsiblePanelBuscadorExpandedHeight = 0;

        private static bool grabar = false;
        private static bool preguntar = true;

        public string CodigoUsuario
        {
            get
            {
                return (this.codigoUsuario);
            }
            set
            {
                if (value == "-1") value = "";
                this.codigoUsuario = value;
            }
        }

        public frmMtoAutElementos()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            //RadItemDragDropManager dragDropMan1 = new RadItemDragDropManager(this.radListControlElementoAut, this.radListControlElementoAut.Items, this.radListControlElementoSel, this.radListControlElementoSel.Items);
            //RadItemDragDropManager dragDropMan2 = new RadItemDragDropManager(this.radListControlGrupoOperac, this.radListControlGrupoOperac.Items, this.radListControlGrupoOperacSel, this.radListControlGrupoOperacSel.Items);

            radCollapsiblePanelBuscadorExpandedHeight = this.radCollapsiblePanelBuscador.Height;
            this.radCollapsiblePanelBuscador.IsExpanded = false;
            this.radCollapsiblePanelBuscador.EnableAnimation = false;

            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            if (tipoBaseDatosCG == "DB2")
            {
                this.prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                if (this.prefijoTabla != null && this.prefijoTabla != "") this.prefijoTabla += ".";
            }
            else this.prefijoTabla = GlobalVar.PrefijoTablaCG;

            this.radMultiColumnComboBoxClases.AutoSizeDropDownToBestFit = true;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void frmMtoAutElementos_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Autorizaciones sobre Elementos");

            if (this.codigoUsuario != "")
            {
                //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
                this.KeyPreview = true;

                this.radPanelMenuPath.Visible = false;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.BackColor = Color.White;
                this.radButtonExit.Visible = false;
            }

            this.radMultiColumnComboBoxClases.Visible = true;

            this.ButtonElementosActivarDesactivar(false);
            this.radPanelElementos.Enabled = false;

            this.ButtonGrupoOperacActivarDesactivar(false);
            this.radPanelGrupoOperac.Enabled = false;

            this.ButtonUsuarioActivarDesactivar(false);
            this.radPanelUsuarios.Enabled = false;

            utiles.ButtonEnabled(ref this.radButtonBuscar, false);
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            //Construir el DataTable de elementos (dtElementos)
            this.BuildDtElementos();

            //Construir el DataTable del resultado de la búsqueda (dtResultadoBusqueda)
            this.BuildDtResultadoBusqueda();

            //Llena el desplegable de clases
            this.CargarListaClases();

            //Buscar todos los usuarios (excluyendo CGIFS y CGAUDIT)
            this.BuscarUsuariosTodos();
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            //Grabar las autorizaciones sobre elementos
            this.GrabarAutSobreElementos();
        }

        private void radButtonBuscar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Verificar que no exista info pendiente de grabar
                if (this.radGridViewAutSobreElementos.Visible && grabar && preguntar)
                {
                    //Pedir confirmación de que perderá la información que no ha grabado
                    DialogResult result = RadMessageBox.Show(this, "Hay información pendiente de grabar, si continúa se perderán. ¿Desea continuar?", this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        preguntar = false;
                        grabar = false;
                    }
                }

                if (!grabar)
                {
                    //Buscar todas las operaciones para la clase seleccionada
                    //this.BuscarOperacionesTodas();

                    //Eliminar la información de la Grid
                    this.dtResultadoBusquedaAux.Clear();
                    this.dtResultadoBusqueda.Clear();
                    this.radGridViewAutSobreElementos.Visible = false;

                    DataRow row;
                    string OPERAG = "";
                    string ELEMAG = "";
                    string GROPAG = "";
                    string DESCRI = "";
                    string IDUSAG = "";
                    string NOMBRE = "";
                    ElementoClase elementoClase = new ElementoClase();

                    //FormValid()
                    string query = "select OPERAG, ELEMAG, GROPAG, IDUSAG from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                    query += " where CLELAG = '" + this.codigo + "' ";

                    string queryElementos = "";
                    if (this.radListControlElementoSel.Items != null && this.radListControlElementoSel.Items.Count > 0)
                        queryElementos += this.QueryATM08Where(ref this.radListControlElementoSel, "ELEMAG");

                    string queryGrupoOperac = "";
                    if (this.radListControlGrupoOperacSel.Items != null && this.radListControlGrupoOperacSel.Items.Count > 0)
                        queryGrupoOperac += this.QueryATM08Where(ref this.radListControlGrupoOperacSel, "GROPAG");

                    string queryUsuarios = "";
                    if (this.radListControlUsuarioSel.Items != null && this.radListControlUsuarioSel.Items.Count > 0)
                        queryUsuarios += this.QueryATM08Where(ref this.radListControlUsuarioSel, "IDUSAG");

                    if (queryElementos != "") query += queryElementos;
                    if (queryGrupoOperac != "") query += queryGrupoOperac;
                    if (queryUsuarios != "") query += queryUsuarios;

                    query += "order by CLELAG, ELEMAG, GROPAG, IDUSAG";

                    //Buscar las autorizaciones sobre elementos que existen el la tabla ATM08 para la búsqueda realizada
                    DataTable dtAux = new DataTable();
                    dtAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dtAux != null && dtAux.Rows != null && dtAux.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAux.Rows.Count; i++)
                        {
                            row = this.dtResultadoBusquedaAux.NewRow();
                            OPERAG = dtAux.Rows[i]["OPERAG"].ToString();
                            row["OPERAG"] = OPERAG;
                            ELEMAG = dtAux.Rows[i]["ELEMAG"].ToString();
                            row["ELEMAG"] = ELEMAG;
                            GROPAG = dtAux.Rows[i]["GROPAG"].ToString();
                            row["GROPAG"] = GROPAG;
                            //elementoClase.Clase = this.codigo.Trim();
                            //elementoClase.Elemento = ELEMAG.Trim();
                            //DESCRI = elementoClase.GetDescripcion();
                            row["DESCRI"] = "";
                            IDUSAG = dtAux.Rows[i]["IDUSAG"].ToString();
                            row["IDUSAG"] = IDUSAG;
                            //NOMBRE = this.NombreUsuario(IDUSAG);
                            row["NOMBRE"] = "";
                            row["OPERAGINI"] = OPERAG;
                            this.dtResultadoBusquedaAux.Rows.Add(row);
                        }
                    }

                    //Buscar todas las posibles autorizaciones para la búsqueda realizada (producto cartesiano)
                    var listElementos = new List<string>();
                    if (this.radListControlElementoSel.Items != null && this.radListControlElementoSel.Items.Count > 0)
                        foreach (var item in this.radListControlElementoSel.Items) listElementos.Add(item.Tag.ToString());
                    else
                    {
                        if (this.radListControlElementoAut.Items != null && this.radListControlElementoAut.Items.Count > 0)
                            foreach (var item in this.radListControlElementoAut.Items) listElementos.Add(item.Tag.ToString());
                    }

                    var listGrupoOperac = new List<string>();
                    if (this.radListControlGrupoOperacSel.Items != null && this.radListControlGrupoOperacSel.Items.Count > 0)
                        foreach (var item in this.radListControlGrupoOperacSel.Items) listGrupoOperac.Add(item.Tag.ToString());
                    else
                    {
                        if (this.radListControlGrupoOperac.Items != null && this.radListControlGrupoOperac.Items.Count > 0)
                            foreach (var item in this.radListControlGrupoOperac.Items) listGrupoOperac.Add(item.Tag.ToString());
                    }

                    var listUsuarios = new List<string>();
                    if (this.radListControlUsuarioSel.Items != null && this.radListControlUsuarioSel.Items.Count > 0)
                        foreach (var item in this.radListControlUsuarioSel.Items) listUsuarios.Add(item.Tag.ToString());
                    else
                    {
                        if (this.radListControlUsuario.Items != null && this.radListControlUsuario.Items.Count > 0)
                            foreach (var item in this.radListControlUsuario.Items) listUsuarios.Add(item.Tag.ToString());
                    }

                    if (listElementos.Count > 0 && listGrupoOperac.Count > 0 && listUsuarios.Count > 0)
                    {
                        for (int i = 0; i < listElementos.Count; i++)
                        {
                            ELEMAG = listElementos[i];
                            for (int j = 0; j < listGrupoOperac.Count; j++)
                            {
                                GROPAG = listGrupoOperac[j];
                                elementoClase.Clase = this.codigo.Trim();
                                elementoClase.Elemento = ELEMAG.Trim();
                                DESCRI = elementoClase.GetDescripcion();
                                for (int z = 0; z < listUsuarios.Count; z++)
                                {
                                    row = this.dtResultadoBusqueda.NewRow();
                                    row["ELEMAG"] = ELEMAG;
                                    row["GROPAG"] = GROPAG;
                                    row["DESCRI"] = DESCRI;
                                    IDUSAG = listUsuarios[z];
                                    row["IDUSAG"] = IDUSAG;
                                    NOMBRE = this.NombreUsuario(IDUSAG);
                                    row["NOMBRE"] = NOMBRE;
                                    OPERAG = this.ExisteATM08Aut(ELEMAG, GROPAG, IDUSAG);
                                    row["OPERAG"] = OPERAG;
                                    row["OPERAGINI"] = OPERAG;
                                    this.dtResultadoBusqueda.Rows.Add(row);
                                }
                            }
                        }

                        this.radGridViewAutSobreElementos.AutoGenerateColumns = true;
                        this.radGridViewAutSobreElementos.DataSource = this.dtResultadoBusqueda;
                        this.radGridViewAutSobreElementos.Visible = true;

                        utiles.ButtonEnabled(ref this.radButtonSave, true);

                        this.RadGridViewHeader();
                        this.radGridViewAutSobreElementos.Rows[0].IsSelected = true;
                        this.radGridViewAutSobreElementos.Rows[0].IsCurrent = true;
                        this.radGridViewAutSobreElementos.Rows[0].EnsureVisible();
                        this.VerticalScroll.Value = this.VerticalScroll.Maximum;

                        this.radCollapsiblePanelBuscador.Collapse();
                    }
                    else
                    {
                        //Error
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Dado un código de usuario, devuelve su nombre buscandólo en la lista de usuarios
        /// </summary>
        private string NombreUsuario(string codigo)
        {
            codigo = codigo.Trim();
            string nombre = "";
            try
            {
                if (this.dtUsuarios != null || this.dtUsuarios.Rows != null && this.dtUsuarios.Rows.Count == 0)
                {
                    for (int i = 0; i < this.dtUsuarios.Rows.Count; i++)
                    {
                        if (codigo == this.dtUsuarios.Rows[i]["IDUSMO"].ToString().Trim())
                        {
                            nombre = this.dtUsuarios.Rows[i]["NOMBMO"].ToString().Trim();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (nombre);
        }

        /// <summary>
        /// Devuelve la operación autorizada para el elemento, grupo y usuario consultado (si existe), sino devuelve la cadena vacía
        /// </summary>
        /// <param name="ELEMAG"></param>
        /// <param name="GROPAG"></param>
        /// <param name="IDUSAG"></param>
        /// <returns></returns>
        private string ExisteATM08Aut(string ELEMAG, string GROPAG, string IDUSAG)
        {
            string OPERAG = "";
            ELEMAG = ELEMAG.Trim();
            GROPAG = GROPAG.Trim();
            IDUSAG = IDUSAG.Trim();
            try
            {
                if (this.dtResultadoBusquedaAux != null || this.dtResultadoBusquedaAux.Rows != null && this.dtResultadoBusquedaAux.Rows.Count == 0)
                {
                    for (int i = 0; i < this.dtResultadoBusquedaAux.Rows.Count; i++)
                    {
                        if (ELEMAG == this.dtResultadoBusquedaAux.Rows[i]["ELEMAG"].ToString().Trim() &&
                            GROPAG == this.dtResultadoBusquedaAux.Rows[i]["GROPAG"].ToString().Trim() &&
                            IDUSAG == this.dtResultadoBusquedaAux.Rows[i]["IDUSAG"].ToString().Trim())
                        {
                            OPERAG = this.dtResultadoBusquedaAux.Rows[i]["OPERAG"].ToString().Trim();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (OPERAG);
        }

        /// <summary>
        /// Devuelve el filtro para la consulta de la tabla ATM08 en dependencia de la lista y campo consultado
        /// </summary>
        /// <param name="radControlList"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        private string QueryATM08Where(ref RadListControl radControlList, string campo)
        {
            string queryWhere = "";

            try
            {
                for (int i = 0; i < radControlList.Items.Count; i++)
                {
                    if (i == 0)
                    {
                        queryWhere += " and (";
                        queryWhere += campo + " = '" + radControlList.Items[i].Tag.ToString() + "' ";
                    }
                    else queryWhere += " or " + campo + " = '" + radControlList.Items[i].Tag.ToString() + "' ";

                    if (i + 1 == radControlList.Items.Count) queryWhere += ") ";
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (queryWhere);
        }

        /// <summary>
        /// Headers de las columnas de la Grid y permite editar solo la 1ra columna
        /// </summary>
        private void RadGridViewHeader()
        {
            if (this.radGridViewAutSobreElementos.Columns.Contains("OPERAG") ||
                this.radGridViewAutSobreElementos.Columns.Contains("Operacion"))
                this.radGridViewAutSobreElementos.Columns["OPERAG"].HeaderText = "Operación";

            if (this.radGridViewAutSobreElementos.Columns.Contains("ELEMAG"))
            {
                this.radGridViewAutSobreElementos.Columns["ELEMAG"].HeaderText = "Elemento";
                this.radGridViewAutSobreElementos.Columns["ELEMAG"].ReadOnly = true;
            }

            if (this.radGridViewAutSobreElementos.Columns.Contains("GROPAG"))
            {
                this.radGridViewAutSobreElementos.Columns["GROPAG"].HeaderText = "Grupo de operación";
                this.radGridViewAutSobreElementos.Columns["GROPAG"].ReadOnly = true;
            }

            if (this.radGridViewAutSobreElementos.Columns.Contains("DESCRI"))
            {
                this.radGridViewAutSobreElementos.Columns["DESCRI"].HeaderText = "Descripción";
                this.radGridViewAutSobreElementos.Columns["DESCRI"].ReadOnly = true;
            }

            if (this.radGridViewAutSobreElementos.Columns.Contains("IDUSAG"))
            {
                this.radGridViewAutSobreElementos.Columns["IDUSAG"].HeaderText = "Usuario";
                this.radGridViewAutSobreElementos.Columns["IDUSAG"].ReadOnly = true;
            }

            if (this.radGridViewAutSobreElementos.Columns.Contains("NOMBRE"))
            {
                this.radGridViewAutSobreElementos.Columns["NOMBRE"].HeaderText = "Nombre";
                this.radGridViewAutSobreElementos.Columns["NOMBRE"].ReadOnly = true;
            }

            if (this.radGridViewAutSobreElementos.Columns.Contains("OPERAGINI")) this.radGridViewAutSobreElementos.Columns["OPERAGINI"].IsVisible = false;          
        }

        private void frmMtoAutElementos_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                /*
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtPwd.Text.Trim() != this.txtPwd.Tag.ToString().Trim() ||
                    this.txtNombre.Text != this.txtNombre.Tag.ToString() ||
                    this.radSpinEditorNumPwdUnicas.Text != this.radSpinEditorNumPwdUnicas.Tag.ToString() ||
                    this.radSpinEditorDiasValidezPwd.Text != this.radSpinEditorDiasValidezPwd.Tag.ToString() ||
                    this.radSpinEditorNumMaxDiasInactividad.Text != this.radSpinEditorNumMaxDiasInactividad.Tag.ToString() ||
                    this.cmbUserAdminCG.Text.ToString().Trim() != this.cmbUserAdminCG.Tag.ToString().Trim()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
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
                */
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Autorizaciones sobre Elementos");
        }
        #endregion

        #region Métodos Privados
        private void MoveAllItems(RadListControl sourceListBox, RadListControl targetListBox)
        {
            for (int i = 0; i < sourceListBox.Items.Count;)
            {
                RadListDataItem item = sourceListBox.Items[i];
                sourceListBox.Items.Remove(item);
                targetListBox.Items.Add(item);
            }
        }

        private void MoveToTargetListBox(RadListControl sourceListBox, RadListControl targetListBox)
        {
            if (sourceListBox.Items.Count == 0) { return; }
            if (sourceListBox.SelectedItem == null) { return; }

            RadListDataItem item = sourceListBox.SelectedItem;
            sourceListBox.Items.Remove(item);
            targetListBox.Items.Add(item);
        }

        /// <summary>
        /// Construye el DataTable de todos los elementos (dtElementos)
        /// </summary>
        private void BuscarElementosTodos()
        {
            try
            {
                this.dtElementos.Clear();

                //Si es el usuario CGIFS o admin (si campo UADMMO = 1 del usuario logado) no se filtra
                //Sino mostrar solo los elementos que el propietario sea el mismo que el logado
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                query += "where CLELAF ='" + this.codigo + "' ";

                if (GlobalVar.UsuarioLogadoCG != "CGIFS")
                    if (GlobalVar.UsuarioLogadoCG_TipoSeguridad != "1") query += "and IDUSAF = '" + GlobalVar.UsuarioLogadoCG + "' ";

                query += "order by ELEMAF";
                
                DataTable dtAux = new DataTable();
                dtAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtAux != null && dtAux.Rows != null && dtAux.Rows.Count > 0)
                {
                    DataRow row;
                    string clase = "";

                    if (this.radMultiColumnComboBoxClases.SelectedValue != null &&
                    this.radMultiColumnComboBoxClases.SelectedValue.ToString() != "-- Seleccionar --")
                        clase = this.radMultiColumnComboBoxClases.SelectedValue.ToString();

                    string elemento = "";
                    string descripcion = "";
                    ElementoClase elementoClase = new ElementoClase();

                    for (int i = 0; i < dtAux.Rows.Count; i++)
                    {
                        row = this.dtElementos.NewRow();
                        elemento = dtAux.Rows[i]["ELEMAF"].ToString();
                        row["ELEMAF"] = elemento;
                        elementoClase.Clase = clase.Trim();
                        elementoClase.Elemento = elemento.Trim();
                        descripcion = elementoClase.GetDescripcion();
                        row["DESCRI"] = descripcion;
                        this.dtElementos.Rows.Add(row);
                    }

                    if (dtElementos.Rows != null && dtElementos.Rows.Count > 0)
                    {
                        if (GlobalVar.UsuarioLogadoCG == "CGIFS" || GlobalVar.UsuarioLogadoCG_TipoSeguridad == "1")
                        {
                            DataRow dr = dtElementos.NewRow();
                            dr["ELEMAF"] = "*";
                            dr["DESCRI"] = "Todos";
                            this.dtElementos.Rows.InsertAt(dr, 0);
                        }

                        this.radPanelBusqueda.Visible = true;
                        this.radCollapsiblePanelBuscador.Enabled = true;
                        this.radCollapsiblePanelBuscador.Visible = true;

                        this.radPanelElementos.Enabled = true;
                        this.radPanelElementos.Visible = true;
                        this.radListControlElementoAut.Enabled = true;
                        this.radListControlElementoAut.Visible = true;
                        this.radListControlElementoSel.Enabled = true;
                        this.radListControlElementoSel.Visible = true;
                        this.ButtonElementosActivarDesactivar(true);

                        this.radPanelGrupoOperac.Enabled = true;
                        this.radLabelGrupoOperac.Enabled = true;
                        this.radLabelGrupoOperacSel.Enabled = true;
                        this.radListControlGrupoOperac.Enabled = true;
                        this.radListControlGrupoOperacSel.Enabled = true;
                        this.radPanelUsuarios.Enabled = true;
                        this.radLabelUsuarios.Enabled = true;
                        this.radLabelUsuariosSel.Enabled = true;
                        this.radListControlUsuario.Enabled = true;
                        this.radListControlUsuarioSel.Enabled = true;

                        this.radLabelNoHayInfoElementos.Visible = false;
                    }
                }
                else
                {
                    this.radPanelBusqueda.Visible = true;
                    this.radCollapsiblePanelBuscador.Enabled = false;
                    this.radPanelElementos.Visible = true;
                    this.radListControlElementoAut.Visible = false;
                    this.radListControlElementoSel.Visible = false;
                    this.ButtonElementosActivarDesactivar(false);
                    this.radPanelGrupoOperac.Enabled = false;
                    this.radLabelGrupoOperac.Enabled = false;
                    this.radLabelGrupoOperacSel.Enabled = false;
                    this.radListControlGrupoOperac.Enabled = false;
                    this.radListControlGrupoOperacSel.Enabled = false;
                    this.ButtonGrupoOperacActivarDesactivar(false);
                    this.radPanelUsuarios.Enabled = false;
                    this.radLabelUsuarios.Enabled = false;
                    this.radLabelUsuariosSel.Enabled = false;
                    this.radListControlUsuario.Enabled = false;
                    this.radListControlUsuarioSel.Enabled = false;
                    this.ButtonUsuarioActivarDesactivar(false);
                    this.radLabelNoHayInfoElementos.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonBuscar, false);  //No es posible realizar la búsqueda, no tieme autoriazación sobre elementos
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        /// <summary>
        /// Carga la información en la lista de elementos autorizados
        /// </summary>
        private void CargarInfoElementos()
        {
            try
            {
                //Buscar todos los elementos
                this.BuscarElementosTodos();

                RadListDataItem radListDataItemAct;
                string ELEMAF = "";
                string DESCRI = "";

                if (this.dtElementos.Rows != null && this.dtElementos.Rows.Count > 0)
                {
                    //Llenar la lista de elementos autorizados 
                    for (int i = 0; i < this.dtElementos.Rows.Count; i++)
                    {
                        radListDataItemAct = new RadListDataItem();
                        ELEMAF = this.dtElementos.Rows[i]["ELEMAF"].ToString();
                        DESCRI = this.dtElementos.Rows[i]["DESCRI"].ToString();
                        radListDataItemAct.Text = ELEMAF + " - " + DESCRI;
                        radListDataItemAct.Tag = ELEMAF;
                        this.radListControlElementoAut.Items.Add(radListDataItemAct);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable de todos los grupos de operaciones (dtGrupoOperac)
        /// </summary>
        private void BuscarGrupoOperacionesTodos()
        {
            try
            {
                string query = "select GROPAB, NOMBAB from " + this.prefijoTabla + "ATM03 ";
                query += "where CLELAB ='" + this.codigo + "' ";
                query += "order by GROPAB";

                this.dtGrupoOperac = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (this.dtGrupoOperac != null && this.dtGrupoOperac.Rows != null && this.dtGrupoOperac.Rows.Count > 0)
                {
                   this.radPanelGrupoOperac.Enabled = true;
                   this.radPanelGrupoOperac.Visible = true;
                   this.radListControlGrupoOperac.Visible = true;
                   this.radListControlGrupoOperacSel.Visible = true;

                   if (!this.radLabelNoHayInfoElementos.Visible) this.ButtonGrupoOperacActivarDesactivar(true);
                   //this.radLabelNoHayInfoElementos.Visible = false;
                }
                else
                {
                    this.radPanelGrupoOperac.Visible = true;
                    this.radListControlGrupoOperac.Visible = false;
                    this.radListControlGrupoOperacSel.Visible = false;
                    this.ButtonGrupoOperacActivarDesactivar(false);
                    //this.radLabelNoHayInfoElementos.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información en las lista de grupo de operaciones
        /// </summary>
        private void CargarInfoGrupoOperaciones()
        {
            try
            {
                //Buscar todos los grupos de operaciones
                this.BuscarGrupoOperacionesTodos();

                RadListDataItem radListDataItemAct;
                string GROPAB = "";
                string NOMBAB = "";

                if (this.dtGrupoOperac.Rows != null && this.dtGrupoOperac.Rows.Count > 0)
                {
                    //Llenar la lista de grupos de operaciones
                    for (int i = 0; i < this.dtGrupoOperac.Rows.Count; i++)
                    {
                        radListDataItemAct = new RadListDataItem();
                        GROPAB = this.dtGrupoOperac.Rows[i]["GROPAB"].ToString();
                        NOMBAB = this.dtGrupoOperac.Rows[i]["NOMBAB"].ToString();
                        radListDataItemAct.Text = GROPAB + " - " + NOMBAB;
                        radListDataItemAct.Tag = GROPAB;
                        this.radListControlGrupoOperac.Items.Add(radListDataItemAct);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /*
        /// <summary>
        /// Construye el DataTable de todas las operaciones posibles para la clase seleccionada
        /// </summary>
        private void BuscarOperacionesTodas()
        {
            try
            {
                this.dtOperaciones.Clear();

                //Buscar todas las operaciones para la clase seleccionada
                string query = "select CLELAC, GROPAC, OPERAC, NOMBAC from " + this.prefijoTabla + "ATM04 ";
                query += "where CLELAC = '" + this.codigo + "' ";

                //Filtrar por grupo si fuera necesario
                string queryGrupoOperac = "";
                if (this.radListControlGrupoOperacSel.Items != null && this.radListControlGrupoOperacSel.Items.Count > 0)
                    queryGrupoOperac += this.QueryATM08Where(ref this.radListControlGrupoOperacSel, "GROPAC");

                if (queryGrupoOperac != "") query += queryGrupoOperac;

                query += "order by CLELAC, GROPAC, OPERAC";

                this.dtOperaciones = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        */

        /// <summary>
        /// Construye el DataTable de todos los usuarios (dtUsuarios)
        /// </summary>
        private void BuscarUsuariosTodos()
        {
            try
            {
                //Buscar todos los usuarios
                string query = "select IDUSMO, NOMBMO from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "where IDUSMO <> 'CGIFS' and IDUSMO <> 'CGAUDIT' ";
                query += "order by IDUSMO";

                this.dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtUsuarios.Rows != null && dtUsuarios.Rows.Count > 0)
                {
                    DataRow dr = dtUsuarios.NewRow();
                    dr["IDUSMO"] = "*";
                    dr["NOMBMO"] = "Acceso público";
                    this.dtUsuarios.Rows.InsertAt(dr, 0);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información en la lista de usuarios
        /// </summary>
        private void CargarInfoUsuarios()
        {
            try
            {
                RadListDataItem radListDataItemAct;
                string IDUSMO = "";
                string NOMBMO = "";

                if (this.dtUsuarios.Rows != null && this.dtUsuarios.Rows.Count > 0)
                {
                    //Llenar la lista de usuarios 
                    for (int i = 0; i < this.dtUsuarios.Rows.Count; i++)
                    {
                        IDUSMO = this.dtUsuarios.Rows[i]["IDUSMO"].ToString();
                        NOMBMO = this.dtUsuarios.Rows[i]["NOMBMO"].ToString();

                        if (this.CodigoUsuario == "")
                        {
                            radListDataItemAct = new RadListDataItem();
                            radListDataItemAct.Text = IDUSMO + " - " + NOMBMO;
                            radListDataItemAct.Tag = IDUSMO;
                            this.radListControlUsuario.Items.Add(radListDataItemAct);
                        }
                        else
                        {
                            if (IDUSMO.Trim() == this.codigoUsuario.Trim())
                            {
                                radListDataItemAct = new RadListDataItem();
                                radListDataItemAct.Text = IDUSMO + " - " + NOMBMO;
                                radListDataItemAct.Tag = IDUSMO;
                                this.radListControlUsuarioSel.Items.Add(radListDataItemAct);
                            }
                            else
                            {
                                radListDataItemAct = new RadListDataItem();
                                radListDataItemAct.Text = IDUSMO + " - " + NOMBMO;
                                radListDataItemAct.Tag = IDUSMO;
                                this.radListControlUsuario.Items.Add(radListDataItemAct);
                            }
                        }
                    }

                    if (this.CodigoUsuario == "")
                    {
                        this.radPanelUsuarios.Enabled = true;
                        this.radPanelUsuarios.Visible = true;
                        this.radListControlUsuario.Visible = true;
                        this.radListControlUsuarioSel.Visible = true;
                        if (!this.radLabelNoHayInfoElementos.Visible) this.ButtonUsuarioActivarDesactivar(true);
                    }
                    else
                    {
                        this.radPanelUsuarios.Visible = true;
                        this.radPanelUsuarios.Enabled = false;
                        this.radListControlUsuario.Enabled = false;
                        this.radListControlUsuarioSel.Enabled = false;
                        this.ButtonUsuarioActivarDesactivar(false);
                    }
                }
                else
                {
                    this.radPanelUsuarios.Visible = false;
                    this.radListControlUsuario.Visible = false;
                    this.radListControlUsuarioSel.Visible = false;
                    this.ButtonUsuarioActivarDesactivar(false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba las autorizaciones sobre elementos
        /// </summary>
        private void GrabarAutSobreElementos()
        {
            Cursor.Current = Cursors.WaitCursor;
            int contNuevo = 0;
            int contModificar = 0;
            int contEliminar = 0;
            int cont = 0;
            try
            {
                if (this.dtResultadoBusqueda != null && this.dtResultadoBusqueda.Rows != null && this.dtResultadoBusqueda.Rows.Count > 0)
                {
                    string operacion = "";
                    string operacionIni = "";
                    string elemento = "";
                    string grupoOperac = "";
                    string usuario = "";
                    for (int i = 0; i < this.dtResultadoBusqueda.Rows.Count; i++)
                    {
                        operacion = this.dtResultadoBusqueda.Rows[i]["OPERAG"].ToString().Trim();
                        operacionIni = this.dtResultadoBusqueda.Rows[i]["OPERAGINI"].ToString().Trim();
                        elemento = this.dtResultadoBusqueda.Rows[i]["ELEMAG"].ToString();
                        grupoOperac = this.dtResultadoBusqueda.Rows[i]["GROPAG"].ToString();
                        usuario = this.dtResultadoBusqueda.Rows[i]["IDUSAG"].ToString();

                        if (operacion == "")
                        {
                            if (operacionIni != "")
                            {
                                cont = this.EliminarAutorizacionElemento(operacionIni, this.codigo, elemento, grupoOperac, usuario);
                                if (cont > 0)
                                {
                                    contEliminar += cont;
                                    //Actualkizar en el datatable la operacion inicial para dicho registro
                                    this.dtResultadoBusqueda.Rows[i]["OPERAGINI"] = "";
                                }
                            }
                        }
                        else
                        {
                            if (operacionIni == "")
                            {
                                cont = this.InsertarAutorizacionElemento(operacion, this.codigo, elemento, grupoOperac, usuario);
                                if (cont > 0)
                                {
                                    contNuevo += cont;
                                    //Actualkizar en el datatable la operacion inicial para dicho registro
                                    this.dtResultadoBusqueda.Rows[i]["OPERAGINI"] = operacion;
                                }
                            }
                            else if (operacion != operacionIni)
                            {
                                cont = this.ActualizarAutorizacionElemento(operacion, this.codigo, elemento, grupoOperac, usuario, operacionIni);
                                if (cont > 0)
                                {
                                    contModificar += cont;
                                    //Actualkizar en el datatable la operacion inicial para dicho registro
                                    this.dtResultadoBusqueda.Rows[i]["OPERAGINI"] = operacion;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            string mensaje = "";
            if (contNuevo > 0) mensaje += "Se han insertado " + contNuevo + " registros.";
            if (contModificar > 0)
            {
                if (mensaje != "") mensaje += "\n\r";
                mensaje += "Se han cambiado " + contModificar + " registros.";
            }
            if (contEliminar > 0)
            {
                if (mensaje != "") mensaje += "\n\r";
                mensaje += "Se han suprimido " + contEliminar + " registros.";
            }

            grabar = false;
            Cursor.Current = Cursors.Default;

            RadMessageBox.Show(mensaje, "Resultado");
        }

        /// <summary>
        /// Elimina la autorización sobre el elemento
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="clase"></param>
        /// <param name="elemento"></param>
        /// <param name="grupoOperac"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private int EliminarAutorizacionElemento(string operacion, string clase, string elemento, 
                                                 string grupoOperac, string usuario)
        {
            int cont = 0;

            try
            {
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                query += "where CLELAG = '" + this.codigo + "' and ";
                query += "OPERAG = '" + operacion + "' and ";
                query += "ELEMAG = '" + elemento + "' and ";
                query += "GROPAG = '" + grupoOperac + "' and ";
                query += "IDUSAG = '" + usuario + "'";

                cont = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (cont > 0)
                {
                    string clave1 = this.codigo.PadLeft(3, '0') + elemento; //CLELAG + ELEMAG
                    string clave2 = grupoOperac; //GROPAG
                    string clave3 = usuario; //IDUSAG
                    string valorOld = operacion;
                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM08", clave1, clave2, clave3, valorOld, null);

                    if (this.codigo == "001")
                    {
                        DataTable dtCompanias = null;
                        try
                        {
                            string compania = "";
                            string CLELAG = "002";

                            //Buscar las autorizaciones de las compañías que pertenecen al grupo de compañías
                            query = "select ELEMAG from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                            query += "where CLELAG = '" + CLELAG + "' and ";
                            query += "OPERAG = '" + operacion + "' and ";
                            query += "GROPAG = '" + grupoOperac + "' and ";
                            query += "IDUSAG = '" + usuario + "' and ";
                            query += "CLEIAG = '001' and ";
                            query += "ELEIAG = '" + elemento + "'";
                            dtCompanias = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                            //Eliminar las autorizaciones de las compañías que pertenecen al grupo de compañías
                            query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                            query += "where CLELAG = '" + CLELAG + "' and ";
                            query += "OPERAG = '" + operacion + "' and ";
                            query += "GROPAG = '" + grupoOperac + "' and ";
                            query += "IDUSAG = '" + usuario + "' and ";
                            query += "CLEIAG = '001' and ";
                            query += "ELEIAG = '" + elemento + "'";

                            int contCia = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            if (contCia > 0)
                            {
                                if (dtCompanias != null && dtCompanias.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCompanias.Rows.Count; i++)
                                    {
                                        //Insertar la auditoria para cada compañía
                                        compania = dtCompanias.Rows[i]["ELEMAG"].ToString();
                                        clave1 = CLELAG + compania; //CLELAG + ELEMAG
                                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM08", clave1, clave2, clave3, valorOld, null);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (cont);
        }

        /// <summary>
        /// Inserta la autorización sobre el elemento
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="clase"></param>
        /// <param name="elemento"></param>
        /// <param name="grupoOperac"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private int InsertarAutorizacionElemento(string operacion, string clase, string elemento,
                                                 string grupoOperac, string usuario)
        {
            int cont = 0;

            try
            {
                string CLEIAG = " ";
                string ELEIAG = " ";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "ATM08";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAG, GROPAG, ELEMAG, IDUSAG, OPERAG, CLEIAG, ELEIAG) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigo + "', '" + grupoOperac + "', '" + elemento + "', '" + usuario;
                query += "', '" + operacion + "', '" + CLEIAG + "', '" + ELEIAG + "')";

                cont = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                
                if (cont > 0)
                {
                    string clave1 = this.codigo.PadLeft(3,'0') + elemento; //CLELAG + ELEMAG
                    string clave2 = grupoOperac; //GROPAG
                    string clave3 = usuario; //IDUSAG
                    string valorNew = operacion;
                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM08", clave1, clave2, clave3, null, valorNew);

                    //Si la clase de elemento es 001 (grupo de compañias), añadir un registro en la tabla ATM08 para cada compañia asociada al grupo de compañias seleccionado
                    if (this.codigo == "001")
                    {
                        query = "select CCIAAI from " + GlobalVar.PrefijoTablaCG + "GLM08 ";
                        query += "where GRUPAI = '" + elemento + "'";

                        IDataReader dr = null;
                        try
                        {
                            dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                            string compania = "";
                            string CLELAG = "002";
                            CLEIAG = "001";
                            while (dr.Read())
                            {
                                compania = dr.GetValue(dr.GetOrdinal("CCIAAI")).ToString();
                                query = "insert into " + nombreTabla + " (";
                                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                                query += "CLELAG, GROPAG, ELEMAG, IDUSAG, OPERAG, CLEIAG, ELEIAG) values (";
                                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                                query += "'" + CLELAG + "', '" + grupoOperac + "', '" + compania + "', '" + usuario;
                                query += "', '" + operacion + "', '" + CLEIAG + "', '" + elemento + "')";

                                int contCia = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                                if (contCia > 0)
                                {
                                    clave1 = CLELAG + compania; //CLELAG + ELEMAG
                                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM08", clave1, clave2, clave3, null, valorNew);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                            if (dr != null) dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (cont);
        }

        /// <summary>
        /// Actualiza la autorización sobre el elemento
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="clase"></param>
        /// <param name="elemento"></param>
        /// <param name="grupoOperac"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private int ActualizarAutorizacionElemento(string operacion, string clase, string elemento,
                                                   string grupoOperac, string usuario, string operacionIni)
        {
            int cont = 0;

            try
            {
                string query = "update " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                query += "set OPERAG = '" + operacion + "' where ";
                query += "CLELAG = '" + this.codigo + "' and GROPAG = '" + grupoOperac + "' and ";
                query += "ELEMAG = '" + elemento + "' and IDUSAG = '" + usuario + "' and OPERAG ='" + operacionIni + "'";

                cont = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (cont > 0)
                {
                    string clave1 = this.codigo.PadLeft(3, '0') + elemento; //CLELAG + ELEMAG
                    string clave2 = grupoOperac; //GROPAG
                    string clave3 = usuario; //IDUSAG
                    string valorOld = operacionIni;
                    string valorNew = operacion;
                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "ATM08", clave1, clave2, clave3, valorOld, valorNew);

                    if (this.codigo == "001")
                    {
                        DataTable dtCompanias = null;
                        try
                        {
                            string compania = "";
                            string CLELAG = "002";

                            //Buscar las autorizaciones de las compañías que pertenecen al grupo de compañías
                            query = "select ELEMAG from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                            query += "where CLELAG = '" + CLELAG + "' and ";
                            query += "OPERAG = '" + operacionIni + "' and ";
                            query += "GROPAG = '" + grupoOperac + "' and ";
                            query += "IDUSAG = '" + usuario + "' and ";
                            query += "CLEIAG = '001' and ";
                            query += "ELEIAG = '" + elemento + "'";
                            dtCompanias = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                            //Actualizar las autorizaciones de las compañías que pertenecen al grupo de compañías
                            query = "update " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                            query += "set OPERAG = '" + operacion + "' ";
                            query += "where CLELAG = '" + CLELAG + "' and ";
                            query += "OPERAG = '" + operacionIni + "' and ";
                            query += "GROPAG = '" + grupoOperac + "' and ";
                            query += "IDUSAG = '" + usuario + "' and ";
                            query += "CLEIAG = '001' and ";
                            query += "ELEIAG = '" + elemento + "'";

                            int contCia = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            if (contCia > 0)
                            {
                                if (dtCompanias != null && dtCompanias.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCompanias.Rows.Count; i++)
                                    {
                                        //Insertar la auditoria para cada compañía
                                        compania = dtCompanias.Rows[i]["ELEMAG"].ToString();
                                        clave1 = CLELAG + compania; //CLELAG + ELEMAG
                                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "ATM08", clave1, clave2, clave3, valorOld, valorNew);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (cont);
        }

        /// <summary>
        /// Activa/Desactiva los botones de los elementos
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonElementosActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonElementoAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonElementoDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonElementoAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonElementoDeleteAll, valor);
        }

        /// <summary>
        /// Activa/Desactiva los botones de los grupos de operaciones
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonGrupoOperacActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonGrupoOperacAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonGrupoOperacDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonGrupoOperacAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonGrupoOperacDeleteAll, valor);
        }

        /// <summary>
        /// Activa/Desactiva los botones de los usuarios
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonUsuarioActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonUsuarioAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, valor);
        }

        /// <summary>
        /// Elimina todos los elementos de todas las listas (elementos, elementos autorizados, grupos de operaciones, grupos de operaciones autorizados)
        /// </summary>
        private void VaciarListasElementosGruposOperac()
        {
            try
            {
                this.radListControlElementoAut.Items.Clear();
                this.radListControlElementoSel.Items.Clear();
                this.radListControlGrupoOperac.Items.Clear();
                this.radListControlGrupoOperacSel.Items.Clear();

                this.ButtonElementosActivarDesactivar(false);
                this.radPanelElementos.Enabled = false;

                this.ButtonGrupoOperacActivarDesactivar(false);
                this.radPanelGrupoOperac.Enabled = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina todos los elementos de todas las listas de usuarios
        /// </summary>
        private void VaciarListasUsuarios()
        {
            try
            {
                this.radListControlUsuario.Items.Clear();
                this.radListControlUsuarioSel.Items.Clear();
                
                this.ButtonUsuarioActivarDesactivar(false);
                this.radPanelUsuarios.Enabled = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        private void radButtonBuscarAut_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonBuscar);
        }

        private void radButtonBuscarAut_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonBuscar);
        }

        private void radButtonElementoDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonElementoDelete);
        }

        private void radButtonElementoDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonElementoDelete);
        }

        private void radButtonElementoAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonElementoAddAll);
        }

        private void radButtonElementoAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonElementoAddAll);
        }

        private void radButtonElementoDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonElementoDeleteAll);
        }

        private void radButtonElementoDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonElementoDeleteAll);
        }

        private void radButtonGrupoOperacAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrupoOperacAdd);
        }

        private void radButtonGrupoOperacAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrupoOperacAdd);
        }

        private void radButtonGrupoOperacDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrupoOperacDelete);
        }

        private void radButtonGrupoOperacDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrupoOperacDelete);
        }

        private void radButtonGrupoOperacAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrupoOperacAddAll);
        }

        private void radButtonGrupoOperacAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrupoOperacAddAll);
        }

        private void radButtonGrupoOperacDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrupoOperacDeleteAll);
        }

        private void radButtonGrupoOperacDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrupoOperacDeleteAll);
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void radButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void frmMtoAutElementos_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.radButtonExit.Visible && e.KeyValue == 27) this.radButtonExit_Click(sender, null);
        }

        /// <summary>
        /// Habilita/Desabilita los botones de elementos en dependencia de las listas
        /// </summary>
        private void ActualizarButtonElementos()
        {
            try
            {
                bool activarPanel = false;
                if (this.radListControlElementoAut.Items != null && this.radListControlElementoAut.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonElementoAdd, false);
                    utiles.ButtonEnabled(ref this.radButtonElementoAddAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonElementoAdd, true);
                    utiles.ButtonEnabled(ref this.radButtonElementoAddAll, true);
                    activarPanel = true;
                }

                if (this.radListControlElementoSel.Items != null && this.radListControlElementoSel.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonElementoDelete, false);
                    utiles.ButtonEnabled(ref this.radButtonElementoDeleteAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonElementoDelete, true);
                    utiles.ButtonEnabled(ref this.radButtonElementoDeleteAll, true);
                    activarPanel = true;
                }

                if (activarPanel) this.radPanelElementos.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Habilita/Desabilita los botones de grupo de ioeraciones en dependencia de las listas
        /// </summary>
        private void ActualizarButtonGrupoOperac()
        {
            try
            {
                bool activarPanel = false;
                if (this.radListControlGrupoOperac.Items != null && this.radListControlGrupoOperac.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacAdd, false);
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacAddAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacAdd, true);
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacAddAll, true);
                    activarPanel = true;
                }

                if (this.radListControlGrupoOperacSel.Items != null && this.radListControlGrupoOperacSel.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacDelete, false);
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacDeleteAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacDelete, true);
                    utiles.ButtonEnabled(ref this.radButtonGrupoOperacDeleteAll, true);
                    activarPanel = true;
                }

                if (activarPanel) this.radPanelGrupoOperac.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Habilita/Desabilita los botones de usuarios en dependencia de las listas
        /// </summary>
        private void ActualizarButtonUsuarios()
        {
            try
            {
                bool activarPanel = false;
                if (this.radListControlUsuario.Items != null && this.radListControlUsuario.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonUsuarioAdd, false);
                    utiles.ButtonEnabled(ref this.radButtonUsuarioAddAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonUsuarioAdd, true);
                    utiles.ButtonEnabled(ref this.radButtonUsuarioAddAll, true);
                    activarPanel = true;
                }

                if (this.radListControlUsuarioSel.Items != null && this.radListControlUsuarioSel.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, false);
                    utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, true);
                    utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, true);
                    activarPanel = true;
                }

                if (activarPanel) this.radPanelUsuarios.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable de Elementos (contiene todos los Elementos autorizados para el usuario logado)
        /// </summary>
        private void BuildDtElementos()
        {
            try
            {
                this.dtElementos.TableName = "Elementos";
                this.dtElementos.Columns.Add("ELEMAF", typeof(string));
                this.dtElementos.Columns.Add("DESCRI", typeof(string));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable del resultado de la búsqueda y el dataTable Auxiliar
        /// </summary>
        private void BuildDtResultadoBusqueda()
        {
            try
            {
                this.dtResultadoBusqueda.TableName = "ResultadoBusq";
                this.dtResultadoBusqueda.Columns.Add("OPERAG", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("ELEMAG", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("GROPAG", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("DESCRI", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("IDUSAG", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("NOMBRE", typeof(string));
                this.dtResultadoBusqueda.Columns.Add("OPERAGINI", typeof(string));

                this.dtResultadoBusquedaAux.TableName = "ResultadoBusqAux";
                this.dtResultadoBusquedaAux.Columns.Add("OPERAG", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("ELEMAG", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("GROPAG", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("DESCRI", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("IDUSAG", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("NOMBRE", typeof(string));
                this.dtResultadoBusquedaAux.Columns.Add("OPERAGINI", typeof(string));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        

        /// <summary>
        /// Carga la lista de clases
        /// </summary>
        private void CargarListaClases()
        {
            try
            {
                RadMultiColumnComboBoxElement multiColumnComboElement = this.radMultiColumnComboBoxClases.MultiColumnComboBoxElement;
                multiColumnComboElement.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                multiColumnComboElement.DropDownMinSize = new Size(420, 300);

                multiColumnComboElement.EditorControl.MasterTemplate.AutoGenerateColumns = false;

                GridViewTextBoxColumn column = new GridViewTextBoxColumn("CLELAA");
                column.HeaderText = "Clase";
                multiColumnComboElement.Columns.Add(column);
                column = new GridViewTextBoxColumn("NOMBAA");
                column.HeaderText = "Nombre";
                multiColumnComboElement.Columns.Add(column);

                this.radMultiColumnComboBoxClases.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;

                string query = "select CLELAA, NOMBAA from " + this.prefijoTabla + "ATM02 ";
                query += "where CLELAA < '800' ";
                query += "order by CLELAA";

                this.dtClases = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtClases.Rows != null && dtClases.Rows.Count > 0)
                {
                    DataRow dr = dtClases.NewRow();
                    dr["CLELAA"] = "-- Seleccionar --";
                    dr["NOMBAA"] = "Seleccionar una clase";
                    this.dtClases.Rows.InsertAt(dr, 0);
                }

                //this.radButtonBuscar.Visible = true;

                this.radMultiColumnComboBoxClases.DataSource = this.dtClases;

                FilterDescriptor descriptor = new FilterDescriptor(this.radMultiColumnComboBoxClases.DisplayMember, FilterOperator.StartsWith, string.Empty);
                this.radMultiColumnComboBoxClases.EditorControl.FilterDescriptors.Add(descriptor);
                this.radMultiColumnComboBoxClases.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void radListControlElementoSel_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlElementoSel, this.radListControlElementoAut);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();

        }

        private void radListControlElementoAut_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlElementoAut, this.radListControlElementoSel);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();
        }

        private void radButtonElementoAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de clases a la lista de clases autorizadas
            MoveToTargetListBox(this.radListControlElementoAut, this.radListControlElementoSel);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();
        }

        private void radButtonElementoAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonElementoAdd);
        }

        private void radButtonElementoAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonElementoAdd);
        }

        private void radListControlGrupoOperac_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlGrupoOperac, this.radListControlGrupoOperacSel);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radListControlGrupoOperacSel_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlGrupoOperacSel, this.radListControlGrupoOperac);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radButtonGrupoOperacAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de procesos a la lista de procesos autorizadas
            MoveToTargetListBox(this.radListControlGrupoOperac, this.radListControlGrupoOperacSel);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radButtonGrupoOperacDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de procesos a la lista de procesos autorizadas
            MoveToTargetListBox(this.radListControlGrupoOperacSel, this.radListControlGrupoOperac);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radButtonGrupoOperacAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlGrupoOperac, this.radListControlGrupoOperacSel);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radButtonGrupoOperacDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlGrupoOperacSel, this.radListControlGrupoOperac);

            //Habilita/Desabilita los botones de grupos de operaciones en dependencia de las listas
            this.ActualizarButtonGrupoOperac();
        }

        private void radListControlUsuario_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlUsuario, this.radListControlUsuarioSel);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radListControlUsuarioSel_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlUsuarioSel, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de usuarios a la lista de usuarios autorizados
            MoveToTargetListBox(this.radListControlUsuario, this.radListControlUsuarioSel);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioAdd);
        }

        private void radButtonUsuarioAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioAdd);
        }

        private void radButtonUsuarioDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de usuarios autorizados a la lista de usuarios
            MoveToTargetListBox(this.radListControlUsuarioSel, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioDelete);
        }

        private void radButtonUsuarioDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioDelete);
        }

        private void radButtonUsuarioAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de usuarios a la lista de usuarios autorizados
            MoveAllItems(this.radListControlUsuario, this.radListControlUsuarioSel);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioAddAll);
        }

        private void radButtonUsuarioAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioAddAll);
        }

        private void radButtonUsuarioDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de usuarios autorizados a la lista de usuarios
            MoveAllItems(this.radListControlUsuarioSel, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioDeleteAll);
        }

        private void radButtonUsuarioDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioDeleteAll);
        }

        private void radMultiColumnComboBoxClase_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.radMultiColumnComboBoxClases.SelectedValue != null &&
                this.radMultiColumnComboBoxClases.SelectedValue.ToString() != "-- Seleccionar --")
            {
                //Verificar si existe información pendiente de grabar
                if (this.radGridViewAutSobreElementos.Visible && grabar && preguntar)
                {
                    //Pedir confirmación de que perderá la información que no ha grabado
                    DialogResult result = RadMessageBox.Show(this, "Hay información pendiente de grabar, si continúa se perderán. ¿Desea continuar?", this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        preguntar = false;
                        grabar = false;
                    }
                }

                if (!grabar)
                {
                    utiles.ButtonEnabled(ref this.radButtonBuscar, true);

                    //Vaciar listas (elementos autorizados, elementos seleccionados, grupos de operaciones autorizados, grupos de operaciones seleccionados)
                    this.VaciarListasElementosGruposOperac();

                    //Clase seleccionada
                    this.codigo = this.radMultiColumnComboBoxClases.SelectedValue.ToString().Trim();

                    //Cargar la lista de Elementos autorizados
                    this.CargarInfoElementos();

                    //Cargar la lista de Grupo de Operaciones
                    this.CargarInfoGrupoOperaciones();

                    //Vaciar listas (usuarios)
                    this.VaciarListasUsuarios();

                    //Cargar la lista de Usuarios
                    this.CargarInfoUsuarios();

                    //Buscar todas las operaciones para la clase seleccionada
                    //this.BuscarOperacionesTodas();

                    this.radGridViewAutSobreElementos.Visible = false;
                    this.radCollapsiblePanelBuscador.Expand();
                }
                else
                {
                    try
                    {
                        if (!preguntar)
                        {
                            preguntar = true;
                            return;
                        }
                        else
                        {
                            GridViewRowInfo rowOld = ((Telerik.WinControls.UI.CurrentRowChangedEventArgs)e).OldRow;
                            grabar = true;
                            preguntar = false;
                            this.radMultiColumnComboBoxClases.SelectedIndex = rowOld.Index;
                        }
                    }
                    catch { }
                }
            }
            else
            {
                utiles.ButtonEnabled(ref this.radButtonBuscar, false);
                this.radGridViewAutSobreElementos.Visible = false;
                utiles.ButtonEnabled(ref this.radButtonSave, false);
            }
        }

        private void radButtonElementoDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de clases autorizadas a la lista de clases 
            MoveToTargetListBox(this.radListControlElementoSel, this.radListControlElementoAut);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();
        }

        private void radButtonElementoAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlElementoAut, this.radListControlElementoSel);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();
        }

        private void radButtonElementoDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases autorizadas a la lista de clases 
            MoveAllItems(this.radListControlElementoSel, this.radListControlElementoAut);

            //Habilita/Desabilita los botones de elementos en dependencia de las listas
            this.ActualizarButtonElementos();
        }

        private void radGridViewAutSobreElementos_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (e.Column.Name == "Operacion")
                {
                    var dropDownEditor = this.radGridViewAutSobreElementos.ActiveEditor as RadDropDownListEditor;
                    var dropDownEditorElement = dropDownEditor.EditorElement as RadDropDownListEditorElement;
                    //var operac = new OperacionesCollection(Convert.ToInt32(e.Value));

                    int indice = this.radGridViewAutSobreElementos.Rows.IndexOf(this.radGridViewAutSobreElementos.CurrentRow);
                    string grupoOperac = this.radGridViewAutSobreElementos.Rows[indice].Cells["GROPAG"].Value.ToString();
                    //string operacActual = this.radGridViewAutSobreElementos.Rows[indice].Cells["OPERAG"].Value.ToString();

                    string query = "select CLELAC, GROPAC, OPERAC, NOMBAC from " + this.prefijoTabla + "ATM04 ";
                    query += "where CLELAC = '" + this.codigo + "' and GROPAC = '" + grupoOperac + "'";
                    query += "order by CLELAC, GROPAC, OPERAC";

                    DataTable dtOperacionesClaseGrupo = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //string _sqlWhere = "GROPAC = '" + grupoOperac + "' and OPERAC = '" + operacActual + "'";
                    //string _sqlWhere = "GROPAC = '" + grupoOperac + "'";
                    //string _sqlOrder = "OPERAC ASC";
                    //DataTable dtSelect = this.dtOperaciones.Select(_sqlWhere, _sqlOrder).CopyToDataTable();
                    //var operac = new OperacionesCollection(dtSelect);

                    var operac = new OperacionesCollection(dtOperacionesClaseGrupo);
                    dropDownEditorElement.DataSource = operac;
                    dropDownEditorElement.DisplayMember = "Name";
                    dropDownEditorElement.ValueMember = "Id";

                    if (dtOperacionesClaseGrupo != null && dtOperacionesClaseGrupo.Rows != null && dtOperacionesClaseGrupo.Rows.Count > 0)
                    {
                        string operag = this.radGridViewAutSobreElementos.Rows[indice].Cells["OPERAG"].Value.ToString();
                        bool existe = false;
                        for (int i = 0; i < dtOperacionesClaseGrupo.Rows.Count; i++)
                        {
                            if (dtOperacionesClaseGrupo.Rows[i]["OPERAC"].ToString() == operag)
                            {
                                dropDownEditorElement.SelectedIndex = i + 1;
                                existe = true;
                                break;
                            }
                            if (!existe) dropDownEditorElement.SelectedIndex = -1;
                        }
                    }
                    else dropDownEditorElement.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void radGridViewAutSobreElementos_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            this.radGridViewAutSobreElementos.Columns["OPERAG"].IsVisible = false;

            var column = new GridViewComboBoxColumn("Operacion", "Operacion");
            //column.DataSource = new OperacionesCollection(this.dtOperaciones);
            column.ValueMember = "Id";
            column.FieldName = "OPERAG";
            column.DisplayMember = "Name";
            column.Width = 80;
            this.radGridViewAutSobreElementos.Columns.Add(column);

            this.radGridViewAutSobreElementos.Columns.Move(this.radGridViewAutSobreElementos.Columns.Count-1, 0);

            this.radGridViewAutSobreElementos.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            //this.radGridViewAutSobreElementos.BestFitColumns();
        }

        private void radCollapsiblePanelBuscador_Expanded(object sender, EventArgs e)
        {
            if (this.radGridViewAutSobreElementos.Visible)
            {
                this.radGridViewAutSobreElementos.Location = new Point(this.radGridViewAutSobreElementos.Location.X, gridInfoLocation.Y + radCollapsiblePanelBuscadorExpandedHeight);
                this.radGridViewAutSobreElementos.Size = gridInfoSize;

                //this.radGridViewAutSobreElementos.Location = new Point(this.radGridViewAutSobreElementos.Location.X, this.radGridViewAutSobreElementos.Location.Y + this.radCollapsiblePanelBuscador.Size.Height);
                //this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, this.radGridViewAutSobreElementos.Size.Height - this.radCollapsiblePanelBuscador.Size.Height);
                //this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, this.radGridViewAutSobreElementos.Size.Height - this.radCollapsiblePanelBuscador.Size.Height);
            }
        }

        private void radCollapsiblePanelBuscador_Collapsed(object sender, EventArgs e)
        {
            if (primeraLlamada) primeraLlamada = false;
            else
            {
                this.radGridViewAutSobreElementos.Location = gridInfoLocation;
                //Es necesario volver asignar el mismo valor porque en la primera ejecución no lo asigna bien (resta 54 al valor inicial)
                this.radGridViewAutSobreElementos.Location = gridInfoLocation;  

                this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, radCollapsiblePanelBuscadorExpandedHeight);

                //this.radGridViewAutSobreElementos.Size = new Size(this.radGridViewAutSobreElementos.Size.Width, this.radGridViewAutSobreElementos.Size.Height + radCollapsiblePanelBuscadorExpandedHeight);
            }
        }

        private void radGridViewAutSobreElementos_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Column.Name == "Operacion")
            {
                ((RadDropDownListEditorElement)((RadDropDownListEditor)e.ActiveEditor).EditorElement).SelectedIndexChanged -= FrmMtoAutElementos_SelectedIndexChanged;
                ((RadDropDownListEditorElement)((RadDropDownListEditor)e.ActiveEditor).EditorElement).SelectedIndexChanged += FrmMtoAutElementos_SelectedIndexChanged;
            }
        }

        private void FrmMtoAutElementos_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            grabar = true;
            preguntar = true;
        }
    }

    #region Clase Operacion
    public class Operacion
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Clase
        {
            get;
            set;
        }

        public string Grupo
        {
            get;
            set;
        }

        public Operacion(string id, string name, string clase, string grupo)
        {
            this.Id = id;
            this.Name = name;
            this.Clase = clase;
            this.Grupo = grupo;
        }
    }

    public class OperacionesCollection : BindingList<Operacion>
    {
        public OperacionesCollection(DataTable dt)
        {
            Operacion operacActual;
            string clase = "";
            string grupo = "";
            string operac = "";
            string descripcion = "";

            operacActual = new Operacion(" ", " ", " ", " ");
            this.Add(operacActual);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clase = dt.Rows[i]["CLELAC"].ToString();
                grupo = dt.Rows[i]["GROPAC"].ToString();
                operac = dt.Rows[i]["OPERAC"].ToString();
                descripcion = operac + " - " + dt.Rows[i]["NOMBAC"].ToString();
                operacActual = new Operacion(operac, descripcion, clase, grupo);
                this.Add(operacActual);
            }
        }
    }
    #endregion
} 