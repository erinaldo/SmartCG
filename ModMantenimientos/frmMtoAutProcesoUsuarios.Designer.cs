namespace ModMantenimientos
{
    partial class frmMtoAutProcesoUsuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoAutProcesoUsuarios));
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelHeader = new Telerik.WinControls.UI.RadLabel();
            this.radButtonBuscarAut = new Telerik.WinControls.UI.RadButton();
            this.radMultiColumnComboBoxProcesos = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radPanelUsuarios = new Telerik.WinControls.UI.RadPanel();
            this.radListControlUsuarioAutoriz = new Telerik.WinControls.UI.RadListControl();
            this.radButtonUsuarioDeleteAll = new Telerik.WinControls.UI.RadButton();
            this.radListControlUsuario = new Telerik.WinControls.UI.RadListControl();
            this.radButtonUsuarioAddAll = new Telerik.WinControls.UI.RadButton();
            this.radButtonUsuarioDelete = new Telerik.WinControls.UI.RadButton();
            this.radButtonUsuarioAdd = new Telerik.WinControls.UI.RadButton();
            this.radLabelUsuariosAutorizadas = new Telerik.WinControls.UI.RadLabel();
            this.radLabelUsuarios = new Telerik.WinControls.UI.RadLabel();
            this.txtCodigo = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radLabelProceso = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonBuscarAut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelUsuarios)).BeginInit();
            this.radPanelUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlUsuarioAutoriz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioDeleteAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioAddAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUsuariosAutorizadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProceso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(723, 54);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 39);
            this.radButtonSave.TabIndex = 186;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.radButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.radButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.radButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelHeader);
            this.radPanelMenuPath.Location = new System.Drawing.Point(1, 3);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1041, 45);
            this.radPanelMenuPath.TabIndex = 189;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelHeader
            // 
            this.radLabelHeader.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelHeader.Location = new System.Drawing.Point(13, 15);
            this.radLabelHeader.Name = "radLabelHeader";
            this.radLabelHeader.Size = new System.Drawing.Size(444, 29);
            this.radLabelHeader.TabIndex = 167;
            this.radLabelHeader.Text = "Mantenimiento de Autorizaciones / Sobre procesos";
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
            // radMultiColumnComboBoxProcesos
            // 
            // 
            // radMultiColumnComboBoxProcesos.NestedRadGridView
            // 
            this.radMultiColumnComboBoxProcesos.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxProcesos.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxProcesos.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxProcesos.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radMultiColumnComboBoxProcesos.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxProcesos.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxProcesos.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxProcesos.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxProcesos.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxProcesos.Location = new System.Drawing.Point(136, 58);
            this.radMultiColumnComboBoxProcesos.Name = "radMultiColumnComboBoxProcesos";
            this.radMultiColumnComboBoxProcesos.Padding = new System.Windows.Forms.Padding(5);
            this.radMultiColumnComboBoxProcesos.Size = new System.Drawing.Size(328, 31);
            this.radMultiColumnComboBoxProcesos.TabIndex = 188;
            this.radMultiColumnComboBoxProcesos.TabStop = false;
            this.radMultiColumnComboBoxProcesos.Text = "Procesos";
            this.radMultiColumnComboBoxProcesos.SelectedValueChanged += new System.EventHandler(this.radMultiColumnComboBoxUsuarios_SelectedValueChanged);
            // 
            // radPanelUsuarios
            // 
            this.radPanelUsuarios.Controls.Add(this.radListControlUsuarioAutoriz);
            this.radPanelUsuarios.Controls.Add(this.radButtonUsuarioDeleteAll);
            this.radPanelUsuarios.Controls.Add(this.radListControlUsuario);
            this.radPanelUsuarios.Controls.Add(this.radButtonUsuarioAddAll);
            this.radPanelUsuarios.Controls.Add(this.radButtonUsuarioDelete);
            this.radPanelUsuarios.Controls.Add(this.radButtonUsuarioAdd);
            this.radPanelUsuarios.Controls.Add(this.radLabelUsuariosAutorizadas);
            this.radPanelUsuarios.Controls.Add(this.radLabelUsuarios);
            this.radPanelUsuarios.Location = new System.Drawing.Point(35, 101);
            this.radPanelUsuarios.Name = "radPanelUsuarios";
            this.radPanelUsuarios.Size = new System.Drawing.Size(885, 383);
            this.radPanelUsuarios.TabIndex = 184;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelUsuarios.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radListControlUsuarioAutoriz
            // 
            this.radListControlUsuarioAutoriz.Location = new System.Drawing.Point(538, 42);
            this.radListControlUsuarioAutoriz.Name = "radListControlUsuarioAutoriz";
            this.radListControlUsuarioAutoriz.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlUsuarioAutoriz.Size = new System.Drawing.Size(326, 327);
            this.radListControlUsuarioAutoriz.TabIndex = 186;
            this.radListControlUsuarioAutoriz.DoubleClick += new System.EventHandler(this.radListControlUsuarioAutoriz_DoubleClick);
            // 
            // radButtonUsuarioDeleteAll
            // 
            this.radButtonUsuarioDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonUsuarioDeleteAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonUsuarioDeleteAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonUsuarioDeleteAll.Location = new System.Drawing.Point(355, 277);
            this.radButtonUsuarioDeleteAll.Name = "radButtonUsuarioDeleteAll";
            this.radButtonUsuarioDeleteAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonUsuarioDeleteAll.TabIndex = 177;
            this.radButtonUsuarioDeleteAll.Text = "<< No autorizar ninguno";
            this.radButtonUsuarioDeleteAll.Click += new System.EventHandler(this.radButtonUsuarioDeleteAll_Click);
            this.radButtonUsuarioDeleteAll.MouseEnter += new System.EventHandler(this.radButtonProcesoDeleteAll_MouseEnter);
            this.radButtonUsuarioDeleteAll.MouseLeave += new System.EventHandler(this.radButtonProcesoDeleteAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonUsuarioDeleteAll.GetChildAt(0))).Text = "<< No autorizar ninguno";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonUsuarioDeleteAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radListControlUsuario
            // 
            this.radListControlUsuario.Location = new System.Drawing.Point(15, 42);
            this.radListControlUsuario.Name = "radListControlUsuario";
            this.radListControlUsuario.Padding = new System.Windows.Forms.Padding(5);
            this.radListControlUsuario.Size = new System.Drawing.Size(326, 327);
            this.radListControlUsuario.TabIndex = 185;
            this.radListControlUsuario.DoubleClick += new System.EventHandler(this.radListControlUsuario_DoubleClick);
            // 
            // radButtonUsuarioAddAll
            // 
            this.radButtonUsuarioAddAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonUsuarioAddAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonUsuarioAddAll.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonUsuarioAddAll.Location = new System.Drawing.Point(355, 221);
            this.radButtonUsuarioAddAll.Name = "radButtonUsuarioAddAll";
            this.radButtonUsuarioAddAll.Size = new System.Drawing.Size(171, 39);
            this.radButtonUsuarioAddAll.TabIndex = 176;
            this.radButtonUsuarioAddAll.Text = ">> Autorizar Todos       ";
            this.radButtonUsuarioAddAll.Click += new System.EventHandler(this.radButtonUsuarioAddAll_Click);
            this.radButtonUsuarioAddAll.MouseEnter += new System.EventHandler(this.radButtonProcesoAddAll_MouseEnter);
            this.radButtonUsuarioAddAll.MouseLeave += new System.EventHandler(this.radButtonProcesoAddAll_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonUsuarioAddAll.GetChildAt(0))).Text = ">> Autorizar Todos       ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonUsuarioAddAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonUsuarioDelete
            // 
            this.radButtonUsuarioDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonUsuarioDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonUsuarioDelete.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonUsuarioDelete.Location = new System.Drawing.Point(355, 150);
            this.radButtonUsuarioDelete.Name = "radButtonUsuarioDelete";
            this.radButtonUsuarioDelete.Size = new System.Drawing.Size(171, 39);
            this.radButtonUsuarioDelete.TabIndex = 175;
            this.radButtonUsuarioDelete.Text = "<  No autorizar";
            this.radButtonUsuarioDelete.Click += new System.EventHandler(this.radButtonUsuarioDelete_Click);
            this.radButtonUsuarioDelete.MouseEnter += new System.EventHandler(this.radButtonProcesoDelete_MouseEnter);
            this.radButtonUsuarioDelete.MouseLeave += new System.EventHandler(this.radButtonProcesoDelete_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonUsuarioDelete.GetChildAt(0))).Text = "<  No autorizar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonUsuarioDelete.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonUsuarioAdd
            // 
            this.radButtonUsuarioAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonUsuarioAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonUsuarioAdd.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonUsuarioAdd.Location = new System.Drawing.Point(355, 97);
            this.radButtonUsuarioAdd.Name = "radButtonUsuarioAdd";
            this.radButtonUsuarioAdd.Size = new System.Drawing.Size(171, 39);
            this.radButtonUsuarioAdd.TabIndex = 174;
            this.radButtonUsuarioAdd.Text = ">  Autorizar     ";
            this.radButtonUsuarioAdd.Click += new System.EventHandler(this.radButtonUsuarioAdd_Click);
            this.radButtonUsuarioAdd.MouseEnter += new System.EventHandler(this.radButtonProcesoAdd_MouseEnter);
            this.radButtonUsuarioAdd.MouseLeave += new System.EventHandler(this.radButtonProcesoAdd_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonUsuarioAdd.GetChildAt(0))).Text = ">  Autorizar     ";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonUsuarioAdd.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelUsuariosAutorizadas
            // 
            this.radLabelUsuariosAutorizadas.Location = new System.Drawing.Point(538, 21);
            this.radLabelUsuariosAutorizadas.Name = "radLabelUsuariosAutorizadas";
            this.radLabelUsuariosAutorizadas.Size = new System.Drawing.Size(201, 19);
            this.radLabelUsuariosAutorizadas.TabIndex = 1;
            this.radLabelUsuariosAutorizadas.Text = "Usuarios autorizados a los procesos";
            // 
            // radLabelUsuarios
            // 
            this.radLabelUsuarios.Location = new System.Drawing.Point(15, 21);
            this.radLabelUsuarios.Name = "radLabelUsuarios";
            this.radLabelUsuarios.Size = new System.Drawing.Size(53, 19);
            this.radLabelUsuarios.TabIndex = 0;
            this.radLabelUsuarios.Text = "Usuarios";
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
            // radLabelProceso
            // 
            this.radLabelProceso.Location = new System.Drawing.Point(50, 65);
            this.radLabelProceso.Name = "radLabelProceso";
            this.radLabelProceso.Size = new System.Drawing.Size(50, 19);
            this.radLabelProceso.TabIndex = 0;
            this.radLabelProceso.Text = "Proceso";
            // 
            // frmMtoProcesoUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1004, 779);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radButtonBuscarAut);
            this.Controls.Add(this.radMultiColumnComboBoxProcesos);
            this.Controls.Add(this.radPanelUsuarios);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.radLabelProceso);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoAutProcesoUsuarios";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Autorización para crear elementos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMtoAutProcesoUsuarios_FormClosing);
            this.Load += new System.EventHandler(this.frmMtoAutProcesoUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonBuscarAut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProcesos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelUsuarios)).EndInit();
            this.radPanelUsuarios.ResumeLayout(false);
            this.radPanelUsuarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlUsuarioAutoriz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioDeleteAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioAddAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonUsuarioAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUsuariosAutorizadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelProceso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabelProceso;
        private Telerik.WinControls.UI.RadTextBoxControl txtCodigo;
        private Telerik.WinControls.UI.RadPanel radPanelUsuarios;
        private Telerik.WinControls.UI.RadLabel radLabelUsuariosAutorizadas;
        private Telerik.WinControls.UI.RadLabel radLabelUsuarios;
        private Telerik.WinControls.UI.RadButton radButtonUsuarioDeleteAll;
        private Telerik.WinControls.UI.RadButton radButtonUsuarioAddAll;
        private Telerik.WinControls.UI.RadButton radButtonUsuarioDelete;
        private Telerik.WinControls.UI.RadButton radButtonUsuarioAdd;
        private Telerik.WinControls.UI.RadListControl radListControlUsuario;
        private Telerik.WinControls.UI.RadListControl radListControlUsuarioAutoriz;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxProcesos;
        private Telerik.WinControls.UI.RadButton radButtonBuscarAut;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelHeader;
    }
}