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
    public partial class frmMtoAutClaseUsuarios : frmPlantilla, IReLocalizable
    {
        private string codigo;

        private string prefijoTabla = "";

        private DataTable dtUsuarios = new DataTable();
        private DataTable dtUsuariosAux = new DataTable();
        private DataTable dtUsuariosAut = new DataTable();
        private DataTable dtUsuariosAutAux = new DataTable();

        private ArrayList usuariosAutInicio = new ArrayList();

        //private bool autTodosUsuarios = false;

        private DataTable dtUsers = new DataTable();

        private int contRegInsert = 0;
        private int contRegDelete = 0;

        public frmMtoAutClaseUsuarios()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            RadItemDragDropManager dragDropMan1 = new RadItemDragDropManager(this.radListControlUsuario, this.radListControlUsuario.Items, this.radListControlUsuarioAutoriz, this.radListControlUsuarioAutoriz.Items);

            //this.radListControlClase.AutoSizeItems = true;
            //this.radListControlClaseAutoriz.AutoSizeItems = true;

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

        private void frmMtoAutClaseUsuarios_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Autorización para crear elementos");

            this.radMultiColumnComboBoxClases.Visible = true;

            this.ButtonUsuariosActivarDesactivar(false);
            this.radPanelUsuarios.Enabled = false;

            utiles.ButtonEnabled(ref this.radButtonSave, false);

            //Cargar la lista de clases
            this.CargarListaClases();
        }

        private void radButtonUsuarioAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de usuarios a la lista de usuarios autorizados
            MoveToTargetListBox(this.radListControlUsuario, this.radListControlUsuarioAutoriz);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de usuarios autorizados a la lista de usuarios 
            MoveToTargetListBox(this.radListControlUsuarioAutoriz, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de usuarios a la lista de usuarios autorizados
            MoveAllItems(this.radListControlUsuario, this.radListControlUsuarioAutoriz);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonUsuarioDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de usuarios autorizados a la lista de usuarios 
            MoveAllItems(this.radListControlUsuarioAutoriz, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radListControlUsuario_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlUsuario, this.radListControlUsuarioAutoriz);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radListControlUsuarioAutoriz_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlUsuarioAutoriz, this.radListControlUsuario);

            //Habilita/Desabilita los botones de usuarios en dependencia de las listas
            this.ActualizarButtonUsuarios();
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            //Usuarios
            this.GrabarUsuarios();
        }

        private void radButtonBuscarAut_Click(object sender, EventArgs e)
        {
            if (this.radMultiColumnComboBoxClases.SelectedValue != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.codigo = this.radMultiColumnComboBoxClases.SelectedValue.ToString().Trim();

                utiles.ButtonEnabled(ref this.radButtonBuscarAut, false);

                //Carga las listas de usuarios (las posibles y las autorizadas)
                this.CargarInfoUsuarios();

                //Actualiza el panel de usuarios y sus botones
                this.ActualizarButtonUsuarios();
                Cursor.Current = Cursors.Default;
            }
        }

        private void radMultiColumnComboBoxUsuarios_SelectedValueChanged(object sender, EventArgs e)
        {
            //Vaciar listas usuarios
            this.VaciarListasUsuarios();

            utiles.ButtonEnabled(ref this.radButtonBuscarAut, true);
        }

        private void frmMtoAutClaseUsuarios_FormClosing(object sender, FormClosingEventArgs e)
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

            if (cerrarForm) Log.Info("FIN Autorización para crear elementos");
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
        /// Construye el DataTable de todos los usuarios (dtUsuariosAux)
        /// </summary>
        private void BuscarUsuariosTodos()
        {
            try
            {
                //Buscar todos los usuarios
                string query = "select IDUSMO, NOMBMO from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "order by IDUSMO";

                this.dtUsuariosAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtUsuariosAux.Rows != null && dtUsuariosAux.Rows.Count > 0)
                {
                    DataRow dr = dtUsuariosAux.NewRow();
                    dr["IDUSMO"] = "*";
                    dr["NOMBMO"] = "Acceso público";
                    this.dtUsuariosAux.Rows.InsertAt(dr, 0);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable de las usuarios autorizados (dtUsuariosAutAux)
        /// </summary>
        private void BuscarUsuariosAutorizados()
        {
            try
            {
                //Buscar todos los usuarios autorizados a crear elementos de la clase
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                query += "where CLELAE = '" + this.codigo + "' ";
                query += "order by IDUSAE";

                this.dtUsuariosAutAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Dado un código de usuario devuleve su nombre
        /// </summary>
        /// <param name="usuarioCodigo"></param>
        /// <returns></returns>
        private string UsuarioNombre(string usuarioCodigo)
        {
            string result = "";
            try
            {
                if (dtUsuariosAux.Rows != null && this.dtUsuariosAux.Rows.Count > 0)
                {
                    for (int i = 0; i < this.dtUsuariosAux.Rows.Count; i++)
                    {
                        if (this.dtUsuariosAux.Rows[i]["IDUSMO"].ToString() == usuarioCodigo)
                        {
                            result = this.dtUsuariosAux.Rows[i]["NOMBMO"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Carga la información en las listas de usuarios (las disponibles y las autorizadas)
        /// </summary>
        private void CargarInfoUsuarios()
        {
            try
            {
                //Buscar todos los usuarios
                this.BuscarUsuariosTodos();

                RadListDataItem radListDataItemAct;
                string IDUSMO = "";
                string NOMBMO = "";

                if (this.dtUsuariosAux.Rows != null && this.dtUsuariosAux.Rows.Count > 0)
                {
                    //Busca las clases autorizadas para el usuario indicado
                    this.BuscarUsuariosAutorizados();

                    try
                    {
                        DataRow rowUsuario;
                        DataRow rowUsuarioAut;

                        dtUsuarios = this.dtUsuariosAux.Clone();
                        dtUsuariosAut = this.dtUsuariosAux.Clone();
                        bool usuarioAut = false;
                        foreach (DataRow row in this.dtUsuariosAux.Rows)
                        {
                            if (this.dtUsuariosAutAux.Rows != null && this.dtUsuariosAutAux.Rows.Count > 0)
                            {
                                foreach (DataRow rowUsuarioAutAux in this.dtUsuariosAutAux.Rows)
                                {
                                    if (rowUsuarioAutAux["IDUSAE"].ToString().Trim() == row["IDUSMO"].ToString().Trim())
                                    {
                                        //Insertar la clase autorizada en el DataSet de clases autorizadas
                                        rowUsuarioAut = dtUsuariosAut.NewRow();
                                        rowUsuarioAut["IDUSMO"] = row["IDUSMO"].ToString();
                                        rowUsuarioAut["NOMBMO"] = row["NOMBMO"].ToString();
                                        this.dtUsuariosAut.Rows.Add(rowUsuarioAut);
                                        usuarioAut = true;
                                        break;
                                    }
                                }

                                if (usuarioAut) usuarioAut = false;
                                else
                                {
                                    //Insertar el usuario en el DataTable de usuarios
                                    rowUsuario = dtUsuarios.NewRow();
                                    rowUsuario["IDUSMO"] = row["IDUSMO"].ToString();
                                    rowUsuario["NOMBMO"] = row["NOMBMO"].ToString();
                                    this.dtUsuarios.Rows.Add(rowUsuario);
                                }
                            }
                            else
                            {
                                //Insertar el usuario en el DataTable de usuarios
                                rowUsuario = dtUsuarios.NewRow();
                                rowUsuario["IDUSMO"] = row["IDUSMO"].ToString();
                                rowUsuario["NOMBMO"] = row["NOMBMO"].ToString();
                                this.dtUsuarios.Rows.Add(rowUsuario);
                            }
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    string usuarioAct = "";
                    try
                    {
                        //Llenar la lista de usuarios
                        if (this.dtUsuarios.Rows != null && this.dtUsuarios.Rows.Count > 0)
                        {
                            for (int i = 0; i < this.dtUsuarios.Rows.Count; i++)
                            {
                                usuarioAct = this.dtUsuarios.Rows[i]["IDUSMO"].ToString().Trim();
                                if (usuarioAct != "CGIFS" && usuarioAct != "CGAUDIT")
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    IDUSMO = this.dtUsuarios.Rows[i]["IDUSMO"].ToString();
                                    NOMBMO = this.dtUsuarios.Rows[i]["NOMBMO"].ToString();

                                    //if (IDUSMO.Trim() == "*") this.autTodosUsuarios = true;

                                    radListDataItemAct.Text = IDUSMO.PadRight(8, ' ') + " - " + NOMBMO;
                                    //radListDataItemAct.Text = NOMBMO;
                                    radListDataItemAct.Tag = IDUSMO;
                                    this.radListControlUsuario.Items.Add(radListDataItemAct);
                                }
                            }

                            utiles.ButtonEnabled(ref this.radButtonSave, true);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        //Llenar la lista de usuarios autorizadas del usuario
                        if (this.dtUsuariosAut.Rows != null && this.dtUsuariosAut.Rows.Count > 0)
                        {
                            for (int i = 0; i < this.dtUsuariosAut.Rows.Count; i++)
                            {
                                usuarioAct = this.dtUsuariosAut.Rows[i]["IDUSMO"].ToString().Trim();
                                if (usuarioAct != "CGIFS" && usuarioAct != "CGAUDIT")
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    IDUSMO = this.dtUsuariosAut.Rows[i]["IDUSMO"].ToString();
                                    NOMBMO = this.UsuarioNombre(IDUSMO);
                                    radListDataItemAct.Text = IDUSMO.PadRight(8, ' ') + " - " + NOMBMO;
                                    //radListDataItemAct.Text = NOMBMO;
                                    radListDataItemAct.Tag = IDUSMO;
                                    this.radListControlUsuarioAutoriz.Items.Add(radListDataItemAct);
                                    this.usuariosAutInicio.Add(IDUSMO);
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba las autorizaciones sobre los usuarios
        /// </summary>
        private void GrabarUsuarios()
        {
            contRegInsert = 0;
            contRegDelete = 0;
            try
            {
                bool eliminarAutUsuarios = false;
                bool grabarAutUsuarios = false;

                if (this.usuariosAutInicio.Count == 0)
                {
                    if (this.radListControlUsuarioAutoriz.Items != null && this.radListControlUsuarioAutoriz.Items.Count > 0) grabarAutUsuarios = true;
                }
                else
                {
                    if (this.radListControlUsuarioAutoriz.Items == null || this.radListControlUsuarioAutoriz.Items.Count == 0) eliminarAutUsuarios = true;
                    else
                    {
                        //Verificar si hay cambios para grabar la informacion
                        if (this.usuariosAutInicio.Count != this.radListControlUsuarioAutoriz.Items.Count)
                        {
                            eliminarAutUsuarios = true;
                            grabarAutUsuarios = true;
                        }
                        else
                        {
                            bool existe = false;
                            for (int i = 0; i < this.usuariosAutInicio.Count; i++)
                            {
                                existe = false;
                                for (int j = 0; j < this.radListControlUsuarioAutoriz.Items.Count; j++)
                                {
                                    if (this.usuariosAutInicio[i].ToString() == this.radListControlUsuarioAutoriz.Items[j].ToString())
                                    {
                                        existe = true;
                                        break;
                                    }
                                }
                                if (!existe) break;
                            }

                            if (existe)
                            {
                                eliminarAutUsuarios = false;
                                grabarAutUsuarios = false;
                            }
                        }
                    }
                }

                if (eliminarAutUsuarios) this.EliminarAutUsuarios();

                if (grabarAutUsuarios) this.GrabarAutUsuarios();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            int contReg = contRegInsert;
            if (contRegInsert == 0 && contRegDelete > 0) contReg = contRegDelete;

            string mensaje = "Se han actualizado " + contReg + " usuarios";

            RadMessageBox.Show(mensaje, "Resultado");
        }

        /// <summary>
        /// Elimina todos los usuarios que tenía la clase anteriormente
        /// </summary>
        private void EliminarAutUsuarios()
        {
            try
            {
                //Eliminar todas las clases autorizadas para el usuario
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                query += "where CLELAE = '" + this.codigo + "' ";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (reg > 0) contRegDelete += reg;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba todos los usuarios autorizados para la clase
        /// </summary>
        private void GrabarAutUsuarios()
        {
            try
            {
                string usuarioActual = "";
                RadListDataItem item;
                /*
                //Verificar si está seleccionada la autorización a * - Acceso público
                for (int i = 0; i < this.radListControlUsuarioAutoriz.Items.Count; i++)
                {
                    item = this.radListControlUsuarioAutoriz.Items[i];
                    usuarioActual = item.Tag.ToString().Trim();
                    if (usuarioActual == "*")
                    {
                        this.autTodosUsuarios = true;
                        break;
                    }
                }

                //Grabar para todos las usuarios la clase autorizada
                if (this.autTodosUsuarios)
                {
                    if (this.dtUsuariosAux.Rows != null && this.dtUsuariosAux.Rows.Count > 1)
                    {
                        for (int i = 1; i < this.dtUsuariosAux.Rows.Count; i++)
                        {
                            usuarioActual = this.dtUsuariosAux.Rows[i]["IDUSMO"].ToString();
                            this.InsertarATM06(this.codigo, usuarioActual);
                        }
                    }
                }
                else
                {*/
                    for (int i = 0; i < this.radListControlUsuarioAutoriz.Items.Count; i++)
                    {
                        item = this.radListControlUsuarioAutoriz.Items[i];
                        usuarioActual = item.Tag.ToString().Trim();
                        if (usuarioActual != "*") usuarioActual = usuarioActual.PadLeft(3, '0');
                        this.InsertarATM06(this.codigo, usuarioActual);
                    }
                //}
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Inserta un registro en la tabla ATM06 (autorizacíón para crear elementos de una clase)
        /// </summary>
        /// <param name="clase"></param>
        /// <param name="usuario"></param>
        private void InsertarATM06(string clase, string usuario)
        {
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "ATM06";

                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAE, IDUSAE) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + clase + "', '" + usuario + "')";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (reg > 0) contRegInsert += reg;
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

                this.dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtUsuarios.Rows != null && dtUsuarios.Rows.Count > 0)
                {
                    DataRow dr = dtUsuarios.NewRow();
                    dr["CLELAA"] = "*";
                    dr["NOMBAA"] = "Todas las clases";
                    this.dtUsuarios.Rows.InsertAt(dr, 0);
                    this.radPanelUsuarios.Enabled = true;
                }

                this.radButtonBuscarAut.Visible = true;

                this.radMultiColumnComboBoxClases.DataSource = this.dtUsuarios;

                FilterDescriptor descriptor = new FilterDescriptor(this.radMultiColumnComboBoxClases.DisplayMember, FilterOperator.StartsWith, string.Empty);
                this.radMultiColumnComboBoxClases.EditorControl.FilterDescriptors.Add(descriptor);
                this.radMultiColumnComboBoxClases.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Activa/Desactiva los botones de los usuarios
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonUsuariosActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonUsuarioAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, valor);
        }
        #endregion

        private void radButtonBuscarAut_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonBuscarAut);
        }

        private void radButtonBuscarAut_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonBuscarAut);
        }

        private void radButtonUsuarioAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioAdd);
        }

        private void radButtonUsuarioAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioAdd);
        }

        private void radButtonUsuarioDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioDelete);
        }

        private void radButtonUsuarioDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioDelete);
        }

        private void radButtonUsuarioAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioAddAll);
        }

        private void radButtonUsuarioAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioAddAll);
        }

        private void radButtonUsuarioDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonUsuarioDeleteAll);
        }

        private void radButtonUsuarioDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonUsuarioDeleteAll);
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radListControlUsuarioAutoriz_Leave(object sender, EventArgs e)
        {
            /*
            if (this.radListControlUsuario.Items != null && this.radListControlUsuario.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.radButtonUsuarioDelete, false);
                utiles.ButtonEnabled(ref this.radButtonUsuarioDeleteAll, false);
            }*/
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

                if (this.radListControlUsuarioAutoriz.Items != null && this.radListControlUsuarioAutoriz.Items.Count == 0)
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
        /// Elimina todos los elementos de todas las listas de usuarios
        /// </summary>
        private void VaciarListasUsuarios()
        {
            try
            {
                this.radListControlUsuario.Items.Clear();
                this.radListControlUsuarioAutoriz.Items.Clear();

                this.ButtonUsuariosActivarDesactivar(false);
                this.radPanelUsuarios.Enabled = false;

                utiles.ButtonEnabled(ref this.radButtonSave, false);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
    }
}