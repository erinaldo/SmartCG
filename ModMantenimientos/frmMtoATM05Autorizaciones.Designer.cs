namespace ModMantenimientos
{
    partial class frmMtoATM05Autorizaciones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoATM05Autorizaciones));
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radPanelProcesos = new Telerik.WinControls.UI.RadPanel();
            this.radListControlProcesoAutoriz = new Telerik.WinControls.UI.RadListControl();
            this.radButtonProcesoDeleteAll = new Telerik.WinControls.UI.RadButton();
            this.radListControlProceso = new Telerik.WinControls.UI.RadListControl();
            this.radButtonProcesoAddAll = new Telerik.WinControls.UI.RadButton();
            this.radButtonProcesoDelete = new Telerik.WinControls.UI.RadButton();
            this.radButtonProcesoAdd = new Telerik.WinControls.UI.RadButton();
            this.radLabelProcesosAut = new Telerik.WinControls.UI.RadLabel();
            this.radLabelProcesos = new Telerik.WinControls.UI.RadLabel();
            this.radPanelClases = new Telerik.WinControls.UI.RadPanel();
            this.radListControlClaseAutoriz = new Telerik.WinControls.UI.RadListControl();
            this.radButtonClaseDeleteAll = new Telerik.WinControls.UI.RadButton();
            this.radListControlClase = new Telerik.WinControls.UI.RadListControl();
            this.radButtonClaseAddAll = new Telerik.WinControls.UI.RadButton();
            this.radButtonClaseDelete = new Telerik.WinControls.UI.RadButton();
            this.radButtonClaseAdd = new Telerik.WinControls.UI.RadButton();
            this.radLabelClasesAutorizadas = new Telerik.WinControls.UI.RadLabel();
            this.radLabelClases = new Telerik.WinControls.UI.RadLabel();
            this.txtCodigo = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radMultiColumnComboBoxUsuarios = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radButtonBuscarAut = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelHeader = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelProcesos)).BeginInit();
            this.radPanelProcesos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlProcesoAutoriz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoDeleteAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlProceso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoAddAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProcesosAut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProcesos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelClases)).BeginInit();
            this.radPanelClases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlClaseAutoriz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseDeleteAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlClase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseAddAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelClasesAutorizadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelClases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonBuscarAut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(594, 584);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(138, 44);
            this.radButtonExit.TabIndex = 187;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.radButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.radButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.radButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(250, 584);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 44);
            this.radButtonSave.TabIndex = 186;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.radButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.radButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.radButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelProcesos
            // 
            this.radPanelProcesos.Controls.Add(this.radListControlProcesoAutoriz);
            this.radPanelProcesos.Controls.Add(this.radButtonProcesoDeleteAll);
            this.radPanelProcesos.Controls.Add(this.radListControlProceso);
            this.radPanelProcesos.Controls.Add(this.radButtonProcesoAddAll);
            this.radPanelProcesos.Controls.Add(this.radButtonProcesoDelete);
            this.radPanelProcesos.Controls.Add(this.radButtonProcesoAdd);
            this.radPanelProcesos.Controls.Add(this.radLabelProcesosAut);
            this.radPanelProcesos.Controls.Add(this.radLabelProcesos);
            this.radPanelProcesos.Location = new System.Drawing.Point(36, 338);
            this.radPanelProcesos.Name = "radPanelProcesos";
            this.radPanelProcesos.Size = new System.Drawing.Size(885, 233);
            this.radPanelProcesos.TabIndex = 185;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelProcesos.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radListControlProcesoAutoriz
            // 
            this.radListControlProcesoAutoriz.Location = new System.Drawing.Point(538, 26);
            this.radListControlProcesoAutoriz.Name = "radListControlProcesoAutoriz";
            this.radListControlProcesoAutoriz.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlProcesoAutoriz.Size = new System.Drawing.Size(326, 197);
            this.radListControlProcesoAutoriz.TabIndex = 186;
            this.radListControlProcesoAutoriz.DoubleClick += new System.EventHandler(this.radListControlProcesoAutoriz_DoubleClick);
            // 
            // radButtonProcesoDeleteAll
            // 
            this.radButtonProcesoDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonProcesoDeleteAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonProcesoDeleteAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonProcesoDeleteAll.Location = new System.Drawing.Point(355, 179);
            this.radButtonProcesoDeleteAll.Name = "radButtonProcesoDeleteAll";
            this.radButtonProcesoDeleteAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonProcesoDeleteAll.TabIndex = 177;
            this.radButtonProcesoDeleteAll.Text = "<< No autorizar ninguno";
            this.radButtonProcesoDeleteAll.Click += new System.EventHandler(this.radButtonProcesoDeleteAll_Click);
            this.radButtonProcesoDeleteAll.MouseEnter += new System.EventHandler(this.radButtonProcesoDeleteAll_MouseEnter);
            this.radButtonProcesoDeleteAll.MouseLeave += new System.EventHandler(this.radButtonProcesoDeleteAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonProcesoDeleteAll.GetChildAt(0))).Text = "<< No autorizar ninguno";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonProcesoDeleteAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radListControlProceso
            // 
            this.radListControlProceso.Location = new System.Drawing.Point(15, 26);
            this.radListControlProceso.Name = "radListControlProceso";
            this.radListControlProceso.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlProceso.Size = new System.Drawing.Size(326, 197);
            this.radListControlProceso.TabIndex = 185;
            this.radListControlProceso.DoubleClick += new System.EventHandler(this.radListControlProceso_DoubleClick);
            // 
            // radButtonProcesoAddAll
            // 
            this.radButtonProcesoAddAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonProcesoAddAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonProcesoAddAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonProcesoAddAll.Location = new System.Drawing.Point(355, 134);
            this.radButtonProcesoAddAll.Name = "radButtonProcesoAddAll";
            this.radButtonProcesoAddAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonProcesoAddAll.TabIndex = 176;
            this.radButtonProcesoAddAll.Text = ">> Autorizar Todos       ";
            this.radButtonProcesoAddAll.Click += new System.EventHandler(this.radButtonProcesoAddAll_Click);
            this.radButtonProcesoAddAll.MouseEnter += new System.EventHandler(this.radButtonProcesoAddAll_MouseEnter);
            this.radButtonProcesoAddAll.MouseLeave += new System.EventHandler(this.radButtonProcesoAddAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonProcesoAddAll.GetChildAt(0))).Text = ">> Autorizar Todos       ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonProcesoAddAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonProcesoDelete
            // 
            this.radButtonProcesoDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonProcesoDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonProcesoDelete.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonProcesoDelete.Location = new System.Drawing.Point(355, 75);
            this.radButtonProcesoDelete.Name = "radButtonProcesoDelete";
            this.radButtonProcesoDelete.Size = new System.Drawing.Size(171, 39);
            this.radButtonProcesoDelete.TabIndex = 175;
            this.radButtonProcesoDelete.Text = "<  No autorizar";
            this.radButtonProcesoDelete.Click += new System.EventHandler(this.radButtonProcesoDelete_Click);
            this.radButtonProcesoDelete.MouseEnter += new System.EventHandler(this.radButtonProcesoDelete_MouseEnter);
            this.radButtonProcesoDelete.MouseLeave += new System.EventHandler(this.radButtonProcesoDelete_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonProcesoDelete.GetChildAt(0))).Text = "<  No autorizar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonProcesoDelete.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonProcesoAdd
            // 
            this.radButtonProcesoAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonProcesoAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonProcesoAdd.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonProcesoAdd.Location = new System.Drawing.Point(355, 31);
            this.radButtonProcesoAdd.Name = "radButtonProcesoAdd";
            this.radButtonProcesoAdd.Size = new System.Drawing.Size(171, 39);
            this.radButtonProcesoAdd.TabIndex = 174;
            this.radButtonProcesoAdd.Text = ">  Autorizar     ";
            this.radButtonProcesoAdd.Click += new System.EventHandler(this.radButtonProcesoAdd_Click);
            this.radButtonProcesoAdd.MouseEnter += new System.EventHandler(this.radButtonProcesoAdd_MouseEnter);
            this.radButtonProcesoAdd.MouseLeave += new System.EventHandler(this.radButtonProcesoAdd_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonProcesoAdd.GetChildAt(0))).Text = ">  Autorizar     ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonProcesoAdd.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelProcesosAut
            // 
            this.radLabelProcesosAut.Location = new System.Drawing.Point(538, 5);
            this.radLabelProcesosAut.Name = "radLabelProcesosAut";
            this.radLabelProcesosAut.Size = new System.Drawing.Size(122, 19);
            this.radLabelProcesosAut.TabIndex = 1;
            this.radLabelProcesosAut.Text = "Procesos autorizados";
            // 
            // radLabelProcesos
            // 
            this.radLabelProcesos.Location = new System.Drawing.Point(15, 5);
            this.radLabelProcesos.Name = "radLabelProcesos";
            this.radLabelProcesos.Size = new System.Drawing.Size(55, 19);
            this.radLabelProcesos.TabIndex = 0;
            this.radLabelProcesos.Text = "Procesos";
            // 
            // radPanelClases
            // 
            this.radPanelClases.Controls.Add(this.radListControlClaseAutoriz);
            this.radPanelClases.Controls.Add(this.radButtonClaseDeleteAll);
            this.radPanelClases.Controls.Add(this.radListControlClase);
            this.radPanelClases.Controls.Add(this.radButtonClaseAddAll);
            this.radPanelClases.Controls.Add(this.radButtonClaseDelete);
            this.radPanelClases.Controls.Add(this.radButtonClaseAdd);
            this.radPanelClases.Controls.Add(this.radLabelClasesAutorizadas);
            this.radPanelClases.Controls.Add(this.radLabelClases);
            this.radPanelClases.Location = new System.Drawing.Point(35, 101);
            this.radPanelClases.Name = "radPanelClases";
            this.radPanelClases.Size = new System.Drawing.Size(885, 233);
            this.radPanelClases.TabIndex = 184;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelClases.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radListControlClaseAutoriz
            // 
            this.radListControlClaseAutoriz.Location = new System.Drawing.Point(538, 26);
            this.radListControlClaseAutoriz.Name = "radListControlClaseAutoriz";
            this.radListControlClaseAutoriz.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlClaseAutoriz.Size = new System.Drawing.Size(326, 197);
            this.radListControlClaseAutoriz.TabIndex = 186;
            this.radListControlClaseAutoriz.DoubleClick += new System.EventHandler(this.radListControlClaseAutoriz_DoubleClick);
            // 
            // radButtonClaseDeleteAll
            // 
            this.radButtonClaseDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonClaseDeleteAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonClaseDeleteAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonClaseDeleteAll.Location = new System.Drawing.Point(355, 179);
            this.radButtonClaseDeleteAll.Name = "radButtonClaseDeleteAll";
            this.radButtonClaseDeleteAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonClaseDeleteAll.TabIndex = 177;
            this.radButtonClaseDeleteAll.Text = "<< No autorizar ninguna";
            this.radButtonClaseDeleteAll.Click += new System.EventHandler(this.radButtonClaseDeleteAll_Click);
            this.radButtonClaseDeleteAll.MouseEnter += new System.EventHandler(this.radButtonClaseDeleteAll_MouseEnter);
            this.radButtonClaseDeleteAll.MouseLeave += new System.EventHandler(this.radButtonClaseDeleteAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonClaseDeleteAll.GetChildAt(0))).Text = "<< No autorizar ninguna";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonClaseDeleteAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radListControlClase
            // 
            this.radListControlClase.Location = new System.Drawing.Point(15, 26);
            this.radListControlClase.Name = "radListControlClase";
            this.radListControlClase.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlClase.Size = new System.Drawing.Size(326, 197);
            this.radListControlClase.TabIndex = 185;
            this.radListControlClase.DoubleClick += new System.EventHandler(this.radListControlClase_DoubleClick);
            // 
            // radButtonClaseAddAll
            // 
            this.radButtonClaseAddAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonClaseAddAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonClaseAddAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonClaseAddAll.Location = new System.Drawing.Point(355, 134);
            this.radButtonClaseAddAll.Name = "radButtonClaseAddAll";
            this.radButtonClaseAddAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonClaseAddAll.TabIndex = 176;
            this.radButtonClaseAddAll.Text = ">> Autorizar Todas       ";
            this.radButtonClaseAddAll.Click += new System.EventHandler(this.radButtonClaseAddAll_Click);
            this.radButtonClaseAddAll.MouseEnter += new System.EventHandler(this.radButtonClaseAddAll_MouseEnter);
            this.radButtonClaseAddAll.MouseLeave += new System.EventHandler(this.radButtonClaseAddAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonClaseAddAll.GetChildAt(0))).Text = ">> Autorizar Todas       ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonClaseAddAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonClaseDelete
            // 
            this.radButtonClaseDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonClaseDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonClaseDelete.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonClaseDelete.Location = new System.Drawing.Point(355, 75);
            this.radButtonClaseDelete.Name = "radButtonClaseDelete";
            this.radButtonClaseDelete.Size = new System.Drawing.Size(171, 39);
            this.radButtonClaseDelete.TabIndex = 175;
            this.radButtonClaseDelete.Text = "<  No autorizar";
            this.radButtonClaseDelete.Click += new System.EventHandler(this.radButtonClaseDelete_Click);
            this.radButtonClaseDelete.MouseEnter += new System.EventHandler(this.radButtonClaseDelete_MouseEnter);
            this.radButtonClaseDelete.MouseLeave += new System.EventHandler(this.radButtonClaseDelete_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonClaseDelete.GetChildAt(0))).Text = "<  No autorizar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonClaseDelete.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonClaseAdd
            // 
            this.radButtonClaseAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonClaseAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonClaseAdd.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonClaseAdd.Location = new System.Drawing.Point(355, 31);
            this.radButtonClaseAdd.Name = "radButtonClaseAdd";
            this.radButtonClaseAdd.Size = new System.Drawing.Size(171, 39);
            this.radButtonClaseAdd.TabIndex = 174;
            this.radButtonClaseAdd.Text = ">  Autorizar     ";
            this.radButtonClaseAdd.Click += new System.EventHandler(this.radButtonClaseAdd_Click);
            this.radButtonClaseAdd.MouseEnter += new System.EventHandler(this.radButtonClaseAdd_MouseEnter);
            this.radButtonClaseAdd.MouseLeave += new System.EventHandler(this.radButtonClaseAdd_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonClaseAdd.GetChildAt(0))).Text = ">  Autorizar     ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonClaseAdd.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelClasesAutorizadas
            // 
            this.radLabelClasesAutorizadas.Location = new System.Drawing.Point(538, 5);
            this.radLabelClasesAutorizadas.Name = "radLabelClasesAutorizadas";
            this.radLabelClasesAutorizadas.Size = new System.Drawing.Size(225, 19);
            this.radLabelClasesAutorizadas.TabIndex = 1;
            this.radLabelClasesAutorizadas.Text = "Clases autorizadas para crear elementos";
            // 
            // radLabelClases
            // 
            this.radLabelClases.Location = new System.Drawing.Point(15, 5);
            this.radLabelClases.Name = "radLabelClases";
            this.radLabelClases.Size = new System.Drawing.Size(118, 19);
            this.radLabelClases.TabIndex = 0;
            this.radLabelClases.Text = "Clases de elementos";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Enabled = false;
            this.txtCodigo.Location = new System.Drawing.Point(136, 58);
            this.txtCodigo.MaxLength = 8;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Padding = new System.Windows.Forms.Padding(5);
            this.txtCodigo.Size = new System.Drawing.Size(99, 30);
            this.txtCodigo.TabIndex = 181;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(50, 65);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(48, 19);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Usuario";
            // 
            // radMultiColumnComboBoxUsuarios
            // 
            // 
            // radMultiColumnComboBoxUsuarios.NestedRadGridView
            // 
            this.radMultiColumnComboBoxUsuarios.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxUsuarios.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxUsuarios.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxUsuarios.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radMultiColumnComboBoxUsuarios.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxUsuarios.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxUsuarios.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxUsuarios.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxUsuarios.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxUsuarios.Location = new System.Drawing.Point(136, 58);
            this.radMultiColumnComboBoxUsuarios.Name = "radMultiColumnComboBoxUsuarios";
            this.radMultiColumnComboBoxUsuarios.Padding = new System.Windows.Forms.Padding(5);
            this.radMultiColumnComboBoxUsuarios.Size = new System.Drawing.Size(328, 31);
            this.radMultiColumnComboBoxUsuarios.TabIndex = 188;
            this.radMultiColumnComboBoxUsuarios.TabStop = false;
            this.radMultiColumnComboBoxUsuarios.Text = "Usuarios";
            this.radMultiColumnComboBoxUsuarios.SelectedValueChanged += new System.EventHandler(this.radMultiColumnComboBoxUsuarios_SelectedValueChanged);
            // 
            // radButtonBuscarAut
            // 
            this.radButtonBuscarAut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonBuscarAut.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonBuscarAut.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonBuscarAut.Location = new System.Drawing.Point(504, 54);
            this.radButtonBuscarAut.Name = "radButtonBuscarAut";
            this.radButtonBuscarAut.Size = new System.Drawing.Size(179, 39);
            this.radButtonBuscarAut.TabIndex = 175;
            this.radButtonBuscarAut.Text = "Buscar  Autorizaciones";
            this.radButtonBuscarAut.Click += new System.EventHandler(this.radButtonBuscarAut_Click);
            this.radButtonBuscarAut.MouseEnter += new System.EventHandler(this.radButtonBuscarAut_MouseEnter);
            this.radButtonBuscarAut.MouseLeave += new System.EventHandler(this.radButtonBuscarAut_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonBuscarAut.GetChildAt(0))).Text = "Buscar  Autorizaciones";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonBuscarAut.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelHeader);
            this.radPanelMenuPath.Location = new System.Drawing.Point(1, 3);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1033, 45);
            this.radPanelMenuPath.TabIndex = 189;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelHeader
            // 
            this.radLabelHeader.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelHeader.Location = new System.Drawing.Point(13, 15);
            this.radLabelHeader.Name = "radLabelHeader";
            this.radLabelHeader.Size = new System.Drawing.Size(462, 29);
            this.radLabelHeader.TabIndex = 167;
            this.radLabelHeader.Text = "Mantenimiento de Autorizaciones / Clases y procesos";
            // 
            // frmMtoATM05Autorizaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(972, 656);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radButtonBuscarAut);
            this.Controls.Add(this.radMultiColumnComboBoxUsuarios);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radPanelProcesos);
            this.Controls.Add(this.radPanelClases);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.radLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoATM05Autorizaciones";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Autorizaciones de Usuarios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMtoATM05Autorizaciones_FormClosing);
            this.Load += new System.EventHandler(this.frmMtoATM05Autorizaciones_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMtoATM05Autorizaciones_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelProcesos)).EndInit();
            this.radPanelProcesos.ResumeLayout(false);
            this.radPanelProcesos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlProcesoAutoriz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoDeleteAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlProceso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoAddAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonProcesoAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProcesosAut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProcesos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelClases)).EndInit();
            this.radPanelClases.ResumeLayout(false);
            this.radPanelClases.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlClaseAutoriz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseDeleteAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlClase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseAddAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonClaseAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelClasesAutorizadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelClases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonBuscarAut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBoxControl txtCodigo;
        private Telerik.WinControls.UI.RadPanel radPanelClases;
        private Telerik.WinControls.UI.RadLabel radLabelClasesAutorizadas;
        private Telerik.WinControls.UI.RadLabel radLabelClases;
        private Telerik.WinControls.UI.RadButton radButtonClaseDeleteAll;
        private Telerik.WinControls.UI.RadButton radButtonClaseAddAll;
        private Telerik.WinControls.UI.RadButton radButtonClaseDelete;
        private Telerik.WinControls.UI.RadButton radButtonClaseAdd;
        private Telerik.WinControls.UI.RadListControl radListControlClase;
        private Telerik.WinControls.UI.RadListControl radListControlClaseAutoriz;
        private Telerik.WinControls.UI.RadPanel radPanelProcesos;
        private Telerik.WinControls.UI.RadListControl radListControlProcesoAutoriz;
        private Telerik.WinControls.UI.RadButton radButtonProcesoDeleteAll;
        private Telerik.WinControls.UI.RadListControl radListControlProceso;
        private Telerik.WinControls.UI.RadButton radButtonProcesoAddAll;
        private Telerik.WinControls.UI.RadButton radButtonProcesoDelete;
        private Telerik.WinControls.UI.RadButton radButtonProcesoAdd;
        private Telerik.WinControls.UI.RadLabel radLabelProcesosAut;
        private Telerik.WinControls.UI.RadLabel radLabelProcesos;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxUsuarios;
        private Telerik.WinControls.UI.RadButton radButtonBuscarAut;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelHeader;
    }
}