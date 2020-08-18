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
    public partial class frmMtoATM05Autorizaciones : frmPlantilla, IReLocalizable
    {
        private bool nuevo;
        private string codigo;

        private string prefijoTabla = "";

        private DataTable dtClases = new DataTable();
        private DataTable dtClasesAux = new DataTable();
        private DataTable dtClasesAut = new DataTable();
        private DataTable dtClasesAutAux = new DataTable();

        private DataTable dtProcesos = new DataTable();
        private DataTable dtProcesosAux = new DataTable();
        private DataTable dtProcesosAut = new DataTable();
        private DataTable dtProcesosAutAux = new DataTable();

        private ArrayList clasesAutInicio = new ArrayList();
        private ArrayList procesosAutInicio = new ArrayList();

        private bool autTodasClases = false;
        private bool autTodosProcesos = false;

        private int contClases = 0;
        private int contProcesos = 0;

        private DataTable dtUsuarios = new DataTable();

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
                if (value == "-1") value = "";
                this.codigo = value;
            }
        }

        public frmMtoATM05Autorizaciones()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            //RadItemDragDropManager dragDropMan1 = new RadItemDragDropManager(this.radListControlClase, this.radListControlClase.Items, this.radListControlClaseAutoriz, this.radListControlClaseAutoriz.Items);
            //RadItemDragDropManager dragDropMan2 = new RadItemDragDropManager(this.radListControlProceso, this.radListControlProceso.Items, this.radListControlProcesoAutoriz, this.radListControlProcesoAutoriz.Items);

            //this.radListControlClase.AutoSizeItems = true;
            //this.radListControlClaseAutoriz.AutoSizeItems = true;

            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            if (tipoBaseDatosCG == "DB2")
            {
                this.prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                if (this.prefijoTabla != null && this.prefijoTabla != "") this.prefijoTabla += ".";
            }
            else this.prefijoTabla = GlobalVar.PrefijoTablaCG;

            this.radMultiColumnComboBoxUsuarios.AutoSizeDropDownToBestFit = true;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void frmMtoATM05Autorizaciones_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Autorizaciones de Usuarios");

            if (this.codigo != "")
            {
                //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
                this.KeyPreview = true;

                this.radPanelMenuPath.Visible = false;
                this.txtCodigo.Text = this.codigo;
                this.txtCodigo.Visible = true;
                this.radMultiColumnComboBoxUsuarios.Visible = false;
                this.radButtonBuscarAut.Visible = false;

                //Carga las listas de clases (las posibles y las autorizadas)
                this.CargarInfoClase();

                //Carga las listas de procesos (las posibles y las autorizadas)
                this.CargarInfoProcesos();
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.BackColor = Color.White;
                this.radButtonExit.Visible = false;
                this.radButtonSave.Location = new Point(723, 54);
                this.radButtonSave.Size = new Size(this.radButtonSave.Size.Width, 39);

                this.txtCodigo.Visible = false;
                this.radMultiColumnComboBoxUsuarios.Visible = true;

                this.ButtonClasesActivarDesactivar(false);
                this.radPanelClases.Enabled = false;

                this.ButtonProcesosActivarDesactivar(false);
                this.radPanelProcesos.Enabled = false;

                utiles.ButtonEnabled(ref this.radButtonSave, false);

                //Cargar la lista de usuarios
                this.CargarListaUsuarios();
            }
        }

        private void radButtonClaseAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de clases a la lista de clases autorizadas
            MoveToTargetListBox(this.radListControlClase, this.radListControlClaseAutoriz);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radButtonClaseDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de clases autorizadas a la lista de clases 
            MoveToTargetListBox(this.radListControlClaseAutoriz, this.radListControlClase);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radButtonClaseAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlClase, this.radListControlClaseAutoriz);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radButtonClaseDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases autorizadas a la lista de clases 
            MoveAllItems(this.radListControlClaseAutoriz, this.radListControlClase);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radListControlClase_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlClase, this.radListControlClaseAutoriz);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radListControlClaseAutoriz_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlClaseAutoriz, this.radListControlClase);

            //Habilita/Desabilita los botones de clases en dependencia de las listas
            this.ActualizarButtonClases();
        }

        private void radButtonProcesoAdd_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de procesos a la lista de procesos autorizadas
            MoveToTargetListBox(this.radListControlProceso, this.radListControlProcesoAutoriz);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radButtonProcesoDelete_Click(object sender, EventArgs e)
        {
            //Mueve el elemento seleccionado de la lista de procesos a la lista de procesos autorizadas
            MoveToTargetListBox(this.radListControlProcesoAutoriz, this.radListControlProceso);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radButtonProcesoAddAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlProceso, this.radListControlProcesoAutoriz);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radButtonProcesoDeleteAll_Click(object sender, EventArgs e)
        {
            //Mueve todos los elementos de la lista de clases a la lista de clases autorizadas
            MoveAllItems(this.radListControlProcesoAutoriz, this.radListControlProceso);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radListControlProceso_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlProceso, this.radListControlProcesoAutoriz);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radListControlProcesoAutoriz_DoubleClick(object sender, EventArgs e)
        {
            MoveToTargetListBox(this.radListControlProcesoAutoriz, this.radListControlProceso);

            //Habilita/Desabilita los botones de procesos en dependencia de las listas
            this.ActualizarButtonProcesos();
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            contClases = 0;
            contProcesos = 0;
            
            //Clases
            this.GrabarClases();

            //Procesos
            this.GrabarProcesos();

            string mensaje = "Se han actualizado " + contClases + " clases y " + contProcesos + " procesos";

            RadMessageBox.Show(mensaje, "Resultado");
        }

        private void radButtonBuscarAut_Click(object sender, EventArgs e)
        {
            if (this.radMultiColumnComboBoxUsuarios.SelectedValue != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.codigo = this.radMultiColumnComboBoxUsuarios.SelectedValue.ToString().Trim();

                utiles.ButtonEnabled(ref this.radButtonBuscarAut, false);

                //Carga las listas de clases (las posibles y las autorizadas)
                this.CargarInfoClase();

                //Carga las listas de procesos (las posibles y las autorizadas)
                this.CargarInfoProcesos();
                Cursor.Current = Cursors.Default;
            }
        }

        private void radMultiColumnComboBoxUsuarios_SelectedValueChanged(object sender, EventArgs e)
        {
            //Vaciar listas (clases, clases autorizadas, procesos, procesos autorizados)
            this.VaciarListasClasesProcesos();

            utiles.ButtonEnabled(ref this.radButtonBuscarAut, true);
        }

        private void frmMtoATM05Autorizaciones_FormClosing(object sender, FormClosingEventArgs e)
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

            if (cerrarForm) Log.Info("FIN Autorizaciones de Usuarios");
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
        /// Construye el DataTable de todas las clases (dtClasesAux)
        /// </summary>
        private void BuscarClasesTodas()
        {
            try
            {
                //Buscar todas las clases
                string queryClases = "select * from " + this.prefijoTabla + "ATM02 ";
                queryClases += "where CLELAA < '800' ";
                queryClases += "order by CLELAA";

                this.dtClasesAux = GlobalVar.ConexionCG.FillDataTable(queryClases, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtClasesAux.Rows != null && dtClasesAux.Rows.Count > 0)
                {
                    DataRow dr = dtClasesAux.NewRow();
                    dr["CLELAA"] = "*";
                    dr["NOMBAA"] = "Todas las clases";
                    this.dtClasesAux.Rows.InsertAt(dr, 0);
                    this.radPanelClases.Enabled = true;
                    this.ButtonClasesActivarDesactivar(true);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable de las clases autorizadas (dtClasesAut)
        /// </summary>
        private void BuscarClasesAutorizadas()
        {
            try
            {
                //Buscar todas las autorizaciones a clases
                string queryClasesAut = "select * from " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                queryClasesAut += "where IDUSAE = '*' or IDUSAE = '" + this.codigo + "' ";
                queryClasesAut += "order by CLELAE";

                this.dtClasesAutAux = GlobalVar.ConexionCG.FillDataTable(queryClasesAut, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información en las listas de clases (las disponibles y las autorizadas)
        /// </summary>
        private void CargarInfoClase()
        {
            try
            {
                //Buscar todas las clases
                this.BuscarClasesTodas();

                RadListDataItem radListDataItemAct;
                string CLELAA = "";
                string NOMBAA = "";

                if (this.dtClasesAux.Rows != null && this.dtClasesAux.Rows.Count > 0)
                    if (this.nuevo)
                    {
                        radListDataItemAct = new RadListDataItem();
                        for (int i = 0; i < this.dtClasesAux.Rows.Count; i++)
                        {
                            radListDataItemAct = new RadListDataItem();
                            CLELAA = this.dtClasesAux.Rows[i]["CLELAA"].ToString();
                            NOMBAA = this.dtClasesAux.Rows[i]["NOMBAA"].ToString();
                            radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                            radListDataItemAct.Tag = CLELAA;
                            this.radListControlClase.Items.Add(radListDataItemAct);
                            this.autTodasClases = true;
                        }
                    }
                    else
                    {
                        //Busca las clases autorizadas para el usuario indicado
                        this.BuscarClasesAutorizadas();

                        try
                        {
                            DataRow rowClase;
                            DataRow rowClaseAut;

                            dtClases = this.dtClasesAux.Clone();
                            dtClasesAut = this.dtClasesAux.Clone();
                            bool claseAut = false;
                            foreach (DataRow row in this.dtClasesAux.Rows)
                            {
                                if (this.dtClasesAutAux.Rows != null && this.dtClasesAutAux.Rows.Count > 0)
                                {
                                    foreach (DataRow rowClaseAutAux in this.dtClasesAutAux.Rows)
                                    {
                                        if (rowClaseAutAux["CLELAE"].ToString() == row["CLELAA"].ToString())
                                        {
                                            //Insertar la clase autorizada en el DataSet de clases autorizadas
                                            rowClaseAut = dtClasesAut.NewRow();
                                            rowClaseAut["CLELAA"] = row["CLELAA"].ToString();
                                            rowClaseAut["NOMBAA"] = row["NOMBAA"].ToString();
                                            this.dtClasesAut.Rows.Add(rowClaseAut);
                                            claseAut = true;
                                            break;
                                        }
                                    }

                                    if (claseAut) claseAut = false;
                                    else
                                    {
                                        //Insertar la clase en el DataTable de clases
                                        rowClase = dtClases.NewRow();
                                        rowClase["CLELAA"] = row["CLELAA"].ToString();
                                        rowClase["NOMBAA"] = row["NOMBAA"].ToString();
                                        this.dtClases.Rows.Add(rowClase);
                                    }
                                }
                                else
                                {
                                    //Insertar la clase en el DataTable de clases
                                    rowClase = dtClases.NewRow();
                                    rowClase["CLELAA"] = row["CLELAA"].ToString();
                                    rowClase["NOMBAA"] = row["NOMBAA"].ToString();
                                    this.dtClases.Rows.Add(rowClase);
                                }
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        try
                        {
                            //Llenar la lista de clases 
                            if (this.dtClases.Rows != null && this.dtClases.Rows.Count > 0)
                            {
                                for (int i = 0; i < this.dtClases.Rows.Count; i++)
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    CLELAA = this.dtClases.Rows[i]["CLELAA"].ToString();
                                    NOMBAA = this.dtClases.Rows[i]["NOMBAA"].ToString();

                                    if (CLELAA.Trim() == "*") this.autTodasClases = true;

                                    radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                                    radListDataItemAct.Tag = CLELAA;
                                    this.radListControlClase.Items.Add(radListDataItemAct);
                                }

                                utiles.ButtonEnabled(ref this.radButtonSave, true);
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        try
                        {
                            //Llenar la lista de clases autorizadas del usuario
                            if (this.dtClasesAut.Rows != null && this.dtClasesAut.Rows.Count > 0)
                            {
                                for (int i = 0; i < this.dtClasesAut.Rows.Count; i++)
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    CLELAA = this.dtClasesAut.Rows[i]["CLELAA"].ToString();
                                    NOMBAA = this.dtClasesAut.Rows[i]["NOMBAA"].ToString();
                                    radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                                    radListDataItemAct.Tag = CLELAA;
                                    this.radListControlClaseAutoriz.Items.Add(radListDataItemAct);
                                    this.clasesAutInicio.Add(CLELAA);
                                }
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        this.ActualizarButtonClases();
                    }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        /// <summary>
        /// Construye el DataTable de todos las procesos (dtProcesos)
        /// </summary>
        private void BuscarProcesosTodos()
        {
            try
            {
                //Buscar todos los procesos
                string queryProc = "select * from " + this.prefijoTabla + "ATM02 ";
                queryProc += "where CLELAA > '800' ";
                queryProc += "order by CLELAA";

                this.dtProcesosAux = GlobalVar.ConexionCG.FillDataTable(queryProc, GlobalVar.ConexionCG.GetConnectionValue);

                if (this.dtProcesosAux.Rows != null && this.dtProcesosAux.Rows.Count > 0)
                {
                    DataRow dr = this.dtProcesosAux.NewRow();
                    dr["CLELAA"] = "*";
                    dr["NOMBAA"] = "Todos los procesos";
                    this.dtProcesosAux.Rows.InsertAt(dr, 0);
                    this.radPanelProcesos.Enabled = true;
                    this.ButtonProcesosActivarDesactivar(true);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construye el DataTable de los procesos autorizados (dtProcesosAut)
        /// </summary>
        private void BuscarProcesosAutorizados()
        {
            try
            {
                //Buscar todas las autorizaciones a procesos
                string queryClasesAut = "select * from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                queryClasesAut += "where CLELAG > 800 and IDUSAG = '" + this.codigo + "' and GROPAG = '01' ";
                queryClasesAut += "order by CLELAG";

                this.dtProcesosAutAux =  GlobalVar.ConexionCG.FillDataTable(queryClasesAut, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la información en las listas de procesos (los disponibles y los autorizados)
        /// </summary>
        private void CargarInfoProcesos()
        {
            try
            {
                //Buscar todos los procesos
                this.BuscarProcesosTodos();

                RadListDataItem radListDataItemAct;
                string CLELAA = "";
                string NOMBAA = "";

                if (this.dtProcesosAux.Rows != null && this.dtProcesosAux.Rows.Count > 0)
                    if (this.nuevo)
                    {
                        radListDataItemAct = new RadListDataItem();
                        for (int i = 0; i < this.dtProcesosAux.Rows.Count; i++)
                        {
                            radListDataItemAct = new RadListDataItem();
                            CLELAA = this.dtProcesosAux.Rows[i]["CLELAA"].ToString();
                            NOMBAA = this.dtProcesosAux.Rows[i]["NOMBAA"].ToString();
                            radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                            radListDataItemAct.Tag = CLELAA;
                            this.radListControlProceso.Items.Add(radListDataItemAct);
                        }
                    }
                    else
                    {
                        //Busca los procesos autorizados para el usuario indicado
                        this.BuscarProcesosAutorizados();

                        try
                        {
                            DataRow rowProceso;
                            DataRow rowProcesoAut;

                            this.dtProcesos = this.dtProcesosAux.Clone();
                            this.dtProcesosAut = this.dtProcesosAux.Clone();
                            bool procesoAut = false;
                            foreach (DataRow row in this.dtProcesosAux.Rows)
                            {
                                if (this.dtProcesosAutAux.Rows != null && this.dtProcesosAutAux.Rows.Count > 0)
                                {
                                    foreach (DataRow rowProcesoAutAux in this.dtProcesosAutAux.Rows)
                                    {
                                        if (rowProcesoAutAux["CLELAG"].ToString() == row["CLELAA"].ToString())
                                        {
                                            //Insertar el proceso autorizado en el DataTable de procesos autorizados
                                            rowProcesoAut = this.dtProcesosAut.NewRow();
                                            rowProcesoAut["CLELAA"] = row["CLELAA"].ToString();
                                            rowProcesoAut["NOMBAA"] = row["NOMBAA"].ToString();
                                            this.dtProcesosAut.Rows.Add(rowProcesoAut);
                                            procesoAut = true;
                                            break;
                                        }
                                    }

                                    if (procesoAut) procesoAut = false;
                                    else
                                    {
                                        //Insertar el proceso en el DataTable de procesos
                                        rowProceso = dtProcesos.NewRow();
                                        rowProceso["CLELAA"] = row["CLELAA"].ToString();
                                        rowProceso["NOMBAA"] = row["NOMBAA"].ToString();
                                        this.dtProcesos.Rows.Add(rowProceso);
                                    }
                                }
                                else
                                {
                                    //Insertar el proceso en el DataTable de procesos
                                    rowProceso = dtProcesos.NewRow();
                                    rowProceso["CLELAA"] = row["CLELAA"].ToString();
                                    rowProceso["NOMBAA"] = row["NOMBAA"].ToString();
                                    this.dtProcesos.Rows.Add(rowProceso);
                                }
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        try
                        {
                            //Llenar la lista de procesos
                            if (this.dtProcesos.Rows != null && this.dtProcesos.Rows.Count > 0)
                            {
                                for (int i = 0; i < this.dtProcesos.Rows.Count; i++)
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    CLELAA = this.dtProcesos.Rows[i]["CLELAA"].ToString();
                                    NOMBAA = this.dtProcesos.Rows[i]["NOMBAA"].ToString();
                                    radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                                    radListDataItemAct.Tag = CLELAA;
                                    this.radListControlProceso.Items.Add(radListDataItemAct);
                                }

                                utiles.ButtonEnabled(ref this.radButtonSave, true);
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        try
                        {
                            //Llenar la lista de procesos autorizados del usuario
                            if (this.dtProcesosAut.Rows != null && this.dtProcesosAut.Rows.Count > 0)
                            {
                                for (int i = 0; i < this.dtProcesosAut.Rows.Count; i++)
                                {
                                    radListDataItemAct = new RadListDataItem();
                                    CLELAA = this.dtProcesosAut.Rows[i]["CLELAA"].ToString();
                                    NOMBAA = this.dtProcesosAut.Rows[i]["NOMBAA"].ToString();
                                    radListDataItemAct.Text = CLELAA.PadRight(3, ' ') + " - " + NOMBAA;
                                    radListDataItemAct.Tag = CLELAA;
                                    this.radListControlProcesoAutoriz.Items.Add(radListDataItemAct);
                                    this.procesosAutInicio.Add(CLELAA);
                                }
                            }
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        this.ActualizarButtonProcesos();
                    }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba las autorizaciones sobre clases
        /// </summary>
        private void GrabarClases()
        {
            try
            {
                bool eliminarAutClases = false;
                bool grabarAutClases = false;

                if (this.clasesAutInicio.Count == 0)
                {
                    if (this.radListControlClaseAutoriz.Items != null && this.radListControlClaseAutoriz.Items.Count > 0) grabarAutClases = true;
                }
                else
                {
                    if (this.radListControlClaseAutoriz.Items == null || this.radListControlClaseAutoriz.Items.Count == 0) eliminarAutClases = true;
                    else
                    {
                        //Verificar si hay cambios para grabar la informacion
                        if (this.clasesAutInicio.Count != this.radListControlClaseAutoriz.Items.Count)
                        {
                            eliminarAutClases = true;
                            grabarAutClases = true;
                        }
                        else
                        {
                            bool existe = false;
                            for (int i = 0; i < this.clasesAutInicio.Count; i++)
                            {
                                existe = false;
                                for (int j = 0; j < this.radListControlClaseAutoriz.Items.Count; j++)
                                {
                                    if (this.clasesAutInicio[i].ToString() == this.radListControlClaseAutoriz.Items[j].ToString())
                                    {
                                        existe = true;
                                        break;
                                    }
                                }
                                if (!existe) break;
                            }

                            if (existe)
                            {
                                eliminarAutClases = false;
                                grabarAutClases = false;
                            }
                        }
                    }    
                }

                if (eliminarAutClases) this.EliminarAutClases();

                if (grabarAutClases) this.GrabarAutClases();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina todas las clases que tenía el usuario anteriormente
        /// </summary>
        private void EliminarAutClases()
        {
            try
            {
                //Eliminar todas las clases autorizadas para el usuario
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                query += "where IDUSAE = '" + this.codigo + "' ";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (reg > 0) contClases += reg;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba todas las clases autorizadas para el usuario
        /// </summary>
        private void GrabarAutClases()
        {
            try
            {
                string claseActual = "";
                RadListDataItem item;

                this.autTodasClases = false;

                //Verificar si está seleccionada la autorización a * - Todas las clases
                for (int i = 0; i < this.radListControlClaseAutoriz.Items.Count; i++)
                {
                    item = this.radListControlClaseAutoriz.Items[i];
                    claseActual = item.Tag.ToString().Trim();
                    if (claseActual == "*")
                    {
                        this.autTodasClases = true;
                        break;
                    }
                }

                //Grabar todas las clases autorizadas para el usuario
                if (this.autTodasClases)
                {
                    if (this.dtClasesAux.Rows != null && this.dtClasesAux.Rows.Count > 1)
                    {
                        for (int i = 1; i < this.dtClasesAux.Rows.Count; i++)
                        {
                            claseActual = this.dtClasesAux.Rows[i]["CLELAA"].ToString().PadLeft(3, '0');
                            this.InsertarATM06(claseActual, this.codigo);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.radListControlClaseAutoriz.Items.Count; i++)
                    {
                        item = this.radListControlClaseAutoriz.Items[i];
                        claseActual = item.Tag.ToString().PadLeft(3, '0');
                        this.InsertarATM06(claseActual, this.codigo);
                    }
                }
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
                //string query = "insert into " + GlobalVar.PrefijoTablaCG + "ATM06 ";
                //query += "(CLELAE, IDUSAE) values ('" + clase + "', '" + usuario + "')";
                string nombreTabla = GlobalVar.PrefijoTablaCG + "ATM06";

                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAE, IDUSAE) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + clase + "', '" + usuario + "')";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (reg > 0) contClases += reg;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba las autorizaciones sobre procesos
        /// </summary>
        private void GrabarProcesos()
        {
            try
            {
                bool eliminarAutProcesos = false;
                bool grabarAutProcesos = false;

                if (this.procesosAutInicio.Count == 0)
                {
                    if (this.radListControlProcesoAutoriz.Items != null && this.radListControlProcesoAutoriz.Items.Count > 0) grabarAutProcesos = true;
                }
                else
                {
                    if (this.radListControlClaseAutoriz.Items == null || this.radListControlClaseAutoriz.Items.Count == 0) eliminarAutProcesos = true;
                    else
                    {
                        //Verificar si hay cambios para grabar la informacion
                        if (this.procesosAutInicio.Count != this.radListControlProcesoAutoriz.Items.Count)
                        {
                            eliminarAutProcesos = true;
                            grabarAutProcesos = true;
                        }
                        else
                        {
                            bool existe = false;
                            for (int i = 0; i < this.procesosAutInicio.Count; i++)
                            {
                                existe = false;
                                for (int j = 0; j < this.radListControlProcesoAutoriz.Items.Count; j++)
                                {
                                    if (this.procesosAutInicio[i].ToString() == this.radListControlProcesoAutoriz.Items[j].ToString())
                                    {
                                        existe = true;
                                        break;
                                    }
                                }
                                if (!existe) break;
                            }

                            if (existe)
                            {
                                eliminarAutProcesos = false;
                                grabarAutProcesos = false;
                            }
                        }
                    }
                }

                if (eliminarAutProcesos) this.EliminarAutProcesos();

                if (grabarAutProcesos) this.GrabarAutProcesos();

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina todos los procesos que tenía el usuario anteriormente
        /// </summary>
        private void EliminarAutProcesos()
        {
            try
            {
                string GROPAG = "01";
                string ELEMAG = "*";
                string OPERAG = "01";

                //Eliminar todos los procesos autorizados para el usuario
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                query += "where CLELAG > 800 and GROPAG = '" + GROPAG + "' and ELEMAG = '" + ELEMAG + "' and ";
                query += "OPERAG = '" + OPERAG + "' and IDUSAG = '" + this.codigo + "' ";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (reg > 0) contProcesos += reg;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba todos los procesos autorizadas para el usuario
        /// </summary>
        private void GrabarAutProcesos()
        {
            try
            {
                string procesoActual = "";
                RadListDataItem item;

                this.autTodosProcesos = false;

                //Verificar si está seleccionada la autorización a * - Todos los procesos
                for (int i = 0; i < this.radListControlProcesoAutoriz.Items.Count; i++)
                {
                    item = this.radListControlProcesoAutoriz.Items[i];
                    procesoActual = item.Tag.ToString().Trim();
                    if (procesoActual == "*")
                    {
                        this.autTodosProcesos = true;
                        break;
                    }
                }

                //Grabar todos los procesos autorizados para el usuario
                if (this.autTodosProcesos)
                {
                    if (this.dtProcesosAux.Rows != null && this.dtProcesosAux.Rows.Count > 1)
                    {
                        for (int i = 1; i < this.dtProcesosAux.Rows.Count; i++)
                        {
                            procesoActual = this.dtProcesosAux.Rows[i]["CLELAA"].ToString().PadLeft(3, '0');
                            this.InsertarATM08(procesoActual, this.codigo);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.radListControlProcesoAutoriz.Items.Count; i++)
                    {
                        item = this.radListControlProcesoAutoriz.Items[i];
                        procesoActual = item.Tag.ToString().PadLeft(3, '0');
                        this.InsertarATM08(procesoActual, this.codigo);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Inserta un registro en la tabla ATM08 (autorizacíón sobre procesos)
        /// </summary>
        /// <param name="proceso"></param>
        /// <param name="usuario"></param>
        private void InsertarATM08(string proceso, string usuario)
        {
            try
            {
                string GROPAG = "01";
                string ELEMAG = "*";
                string OPERAG = "01";
                string CLEIAG = " ";
                string ELEIAG = " ";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "ATM08";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAG, GROPAG, ELEMAG, IDUSAG, OPERAG, CLEIAG, ELEIAG) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + proceso + "', '" + GROPAG + "', '" + ELEMAG + "', '" + usuario + "', '";
                query += OPERAG + "', '" + CLEIAG + "', '" + ELEIAG + "')";

                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (reg > 0) contProcesos += reg;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la lista de usuarios
        /// </summary>
        private void CargarListaUsuarios()
        {
            try
            {
                RadMultiColumnComboBoxElement multiColumnComboElement = this.radMultiColumnComboBoxUsuarios.MultiColumnComboBoxElement;
                multiColumnComboElement.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                multiColumnComboElement.DropDownMinSize = new Size(420, 300);

                multiColumnComboElement.EditorControl.MasterTemplate.AutoGenerateColumns = false;

                GridViewTextBoxColumn column = new GridViewTextBoxColumn("IDUSMO");
                column.HeaderText = "ID Usuario";
                multiColumnComboElement.Columns.Add(column);
                column = new GridViewTextBoxColumn("NOMBMO");
                column.HeaderText = "Nombre";
                multiColumnComboElement.Columns.Add(column);

                this.radMultiColumnComboBoxUsuarios.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;

                string query = "select IDUSMO, NOMBMO from " + GlobalVar.PrefijoTablaCG + "ATM05 ";
                query += "order by NOMBMO";
                this.dtUsuarios = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                DataRow row = this.dtUsuarios.NewRow();
                row["IDUSMO"] = "*";
                row["NOMBMO"] = "Acceso público";
                this.dtUsuarios.Rows.Add(row);

                this.radButtonBuscarAut.Visible = true;


                /*
                NorthwindDataSet nwindDataSet = new NorthwindDataSet();
                CustomersTableAdapter customersTableAdapter = new CustomersTableAdapter();
                customersTableAdapter.Fill(nwindDataSet.Customers);
                */

                this.radMultiColumnComboBoxUsuarios.DataSource = this.dtUsuarios;
                
                FilterDescriptor descriptor = new FilterDescriptor(this.radMultiColumnComboBoxUsuarios.DisplayMember, FilterOperator.StartsWith, string.Empty);
                this.radMultiColumnComboBoxUsuarios.EditorControl.FilterDescriptors.Add(descriptor);
                this.radMultiColumnComboBoxUsuarios.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Activa/Desactiva los botones de las clases
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonClasesActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonClaseAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonClaseDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonClaseAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonClaseDeleteAll, valor);
        }

        /// <summary>
        /// Activa/Desactiva los botones de los procesos
        /// </summary>
        /// <param name="valor"></param>
        private void ButtonProcesosActivarDesactivar(bool valor)
        {
            utiles.ButtonEnabled(ref this.radButtonProcesoAdd, valor);
            utiles.ButtonEnabled(ref this.radButtonProcesoDelete, valor);
            utiles.ButtonEnabled(ref this.radButtonProcesoAddAll, valor);
            utiles.ButtonEnabled(ref this.radButtonProcesoDeleteAll, valor);
        }

        /// <summary>
        /// Elimina todos los elementos de todas las listas (clases, clases autorizadas, procesos, procesos autorizados)
        /// </summary>
        private void VaciarListasClasesProcesos()
        {
            try
            {
                this.radListControlClase.Items.Clear();
                this.radListControlClaseAutoriz.Items.Clear();
                this.radListControlProceso.Items.Clear();
                this.radListControlProcesoAutoriz.Items.Clear();

                this.ButtonClasesActivarDesactivar(false);
                this.radPanelClases.Enabled = false;

                this.ButtonProcesosActivarDesactivar(false);
                this.radPanelProcesos.Enabled = false;

                utiles.ButtonEnabled(ref this.radButtonSave, false);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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

        private void radButtonClaseAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonClaseAdd);
        }

        private void radButtonClaseAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonClaseAdd);
        }

        private void radButtonClaseDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonClaseDelete);
        }

        private void radButtonClaseDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonClaseDelete);
        }

        private void radButtonClaseAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonClaseAddAll);
        }

        private void radButtonClaseAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonClaseAddAll);
        }

        private void radButtonClaseDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonClaseDeleteAll);
        }

        private void radButtonClaseDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonClaseDeleteAll);
        }

        private void radButtonProcesoAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonProcesoAdd);
        }

        private void radButtonProcesoAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonProcesoAdd);
        }

        private void radButtonProcesoDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonProcesoDelete);
        }

        private void radButtonProcesoDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonProcesoDelete);
        }

        private void radButtonProcesoAddAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonProcesoAddAll);
        }

        private void radButtonProcesoAddAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonProcesoAddAll);
        }

        private void radButtonProcesoDeleteAll_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonProcesoDeleteAll);
        }

        private void radButtonProcesoDeleteAll_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonProcesoDeleteAll);
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

        private void frmMtoATM05Autorizaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.radButtonExit.Visible && e.KeyValue == 27) this.radButtonExit_Click(sender, null);
        }

        /// <summary>
        /// Habilita/Desabilita los botones de clases en dependencia de las listas
        /// </summary>
        private void ActualizarButtonClases()
        {
            try
            {
                bool activarPanel = false;
                if (this.radListControlClase.Items != null && this.radListControlClase.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonClaseAdd, false);
                    utiles.ButtonEnabled(ref this.radButtonClaseAddAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonClaseAdd, true);
                    utiles.ButtonEnabled(ref this.radButtonClaseAddAll, true);
                    activarPanel = true;
                }

                if (this.radListControlClaseAutoriz.Items != null && this.radListControlClaseAutoriz.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonClaseDelete, false);
                    utiles.ButtonEnabled(ref this.radButtonClaseDeleteAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonClaseDelete, true);
                    utiles.ButtonEnabled(ref this.radButtonClaseDeleteAll, true);
                    activarPanel = true;
                }

                if (activarPanel) this.radPanelClases.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Habilita/Desabilita los botones de procesos en dependencia de las listas
        /// </summary>
        private void ActualizarButtonProcesos()
        {
            try
            {
                bool activarPanel = false;
                if (this.radListControlProceso.Items != null && this.radListControlProceso.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonProcesoAdd, false);
                    utiles.ButtonEnabled(ref this.radButtonProcesoAddAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonProcesoAdd, true);
                    utiles.ButtonEnabled(ref this.radButtonProcesoAddAll, true);
                    activarPanel = true;
                }

                if (this.radListControlProcesoAutoriz.Items != null && this.radListControlProcesoAutoriz.Items.Count == 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonProcesoDelete, false);
                    utiles.ButtonEnabled(ref this.radButtonProcesoDeleteAll, false);
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonProcesoDelete, true);
                    utiles.ButtonEnabled(ref this.radButtonProcesoDeleteAll, true);
                    activarPanel = true;
                }

                if (activarPanel) this.radPanelProcesos.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
    }
}