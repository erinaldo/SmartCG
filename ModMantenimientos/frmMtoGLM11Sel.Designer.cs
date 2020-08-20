namespace ModMantenimientos
{
    partial class frmMtoGLM11Sel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoGLM11Sel));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radDataFilterGridInfo = new Telerik.WinControls.UI.RadDataFilter();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.lblClaseZona = new Telerik.WinControls.UI.RadLabel();
            this.radButtonTextBoxClaseZona = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonElementClaseZona = new Telerik.WinControls.UI.RadButtonElement();
            this.radGridViewZonas = new Telerik.WinControls.UI.RadGridView();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilterGridInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClaseZona)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxClaseZona)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewZonas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewZonas.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radDataFilterGridInfo
            // 
            this.radDataFilterGridInfo.AllowDragDrop = false;
            this.radDataFilterGridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDataFilterGridInfo.LineColor = System.Drawing.Color.Gray;
            this.radDataFilterGridInfo.Location = new System.Drawing.Point(0, 0);
            this.radDataFilterGridInfo.Name = "radDataFilterGridInfo";
            this.radDataFilterGridInfo.Size = new System.Drawing.Size(558, 172);
            this.radDataFilterGridInfo.TabIndex = 2;
            this.radDataFilterGridInfo.EditorInitialized += new Telerik.WinControls.UI.TreeNodeEditorInitializedEventHandler(this.RadDataFilterGridInfo_EditorInitialized);
            this.radDataFilterGridInfo.Edited += new Telerik.WinControls.UI.TreeNodeEditedEventHandler(this.RadDataFilterGridInfo_Edited);
            this.radDataFilterGridInfo.NodeFormatting += new Telerik.WinControls.UI.TreeNodeFormattingEventHandler(this.RadDataFilterGridInfo_NodeFormatting);
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Controls.Add(this.lblClaseZona);
            this.radPanelApp.Controls.Add(this.radButtonTextBoxClaseZona);
            this.radPanelApp.Controls.Add(this.radGridViewZonas);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 550);
            this.radPanelApp.TabIndex = 188;
            this.radPanelApp.Resize += new System.EventHandler(this.radPanelApp_Resize);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(22, 94);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(99, 19);
            this.radLabelNoHayInfo.TabIndex = 186;
            this.radLabelNoHayInfo.Text = "No existen zonas";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // lblClaseZona
            // 
            this.lblClaseZona.Location = new System.Drawing.Point(19, 16);
            this.lblClaseZona.Name = "lblClaseZona";
            this.lblClaseZona.Size = new System.Drawing.Size(82, 19);
            this.lblClaseZona.TabIndex = 11;
            this.lblClaseZona.Text = "Clase de zona";
            // 
            // radButtonTextBoxClaseZona
            // 
            this.radButtonTextBoxClaseZona.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radButtonTextBoxClaseZona.Location = new System.Drawing.Point(19, 43);
            this.radButtonTextBoxClaseZona.MaxLength = 3;
            this.radButtonTextBoxClaseZona.Name = "radButtonTextBoxClaseZona";
            this.radButtonTextBoxClaseZona.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonTextBoxClaseZona.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElementClaseZona});
            this.radButtonTextBoxClaseZona.Size = new System.Drawing.Size(285, 30);
            this.radButtonTextBoxClaseZona.TabIndex = 183;
            this.radButtonTextBoxClaseZona.TextChanged += new System.EventHandler(this.RadButtonTextBoxClaseZona_TextChanged);
            // 
            // radButtonElementClaseZona
            // 
            this.radButtonElementClaseZona.AutoSize = true;
            this.radButtonElementClaseZona.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radButtonElementClaseZona.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElementClaseZona.Image")));
            this.radButtonElementClaseZona.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonElementClaseZona.Name = "radButtonElementClaseZona";
            this.radButtonElementClaseZona.Text = "";
            this.radButtonElementClaseZona.UseCompatibleTextRendering = false;
            this.radButtonElementClaseZona.Click += new System.EventHandler(this.RadButtonElementClaseZona_Click);
            // 
            // radGridViewZonas
            // 
            this.radGridViewZonas.AutoScroll = true;
            this.radGridViewZonas.Location = new System.Drawing.Point(19, 97);
            // 
            // 
            // 
            this.radGridViewZonas.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewZonas.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewZonas.MasterTemplate.AllowEditRow = false;
            this.radGridViewZonas.MasterTemplate.MultiSelect = true;
            this.radGridViewZonas.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewZonas.Name = "radGridViewZonas";
            this.radGridViewZonas.Size = new System.Drawing.Size(770, 415);
            this.radGridViewZonas.TabIndex = 185;
            this.radGridViewZonas.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewZonas_ViewCellFormatting);
            this.radGridViewZonas.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewZonas_CellClick);
            this.radGridViewZonas.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewZonas_CellDoubleClick);
            this.radGridViewZonas.Leave += new System.EventHandler(this.radGridViewZonas_Leave);
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonExport);
            this.radPanelAcciones.Controls.Add(this.radButtonNuevo);
            this.radPanelAcciones.Controls.Add(this.radButtonEditar);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 442);
            this.radPanelAcciones.TabIndex = 187;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonExport
            // 
            this.radButtonExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExport.ForeColor = System.Drawing.SystemColors.Window;
            this.radButtonExport.Location = new System.Drawing.Point(13, 138);
            this.radButtonExport.Name = "radButtonExport";
            this.radButtonExport.Size = new System.Drawing.Size(138, 44);
            this.radButtonExport.TabIndex = 28;
            this.radButtonExport.Text = "Exportar";
            this.radButtonExport.Click += new System.EventHandler(this.RadButtonExport_Click);
            this.radButtonExport.MouseEnter += new System.EventHandler(this.RadButtonExport_MouseEnter);
            this.radButtonExport.MouseLeave += new System.EventHandler(this.RadButtonExport_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExport.GetChildAt(0))).Text = "Exportar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExport.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonNuevo
            // 
            this.radButtonNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonNuevo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonNuevo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonNuevo.Location = new System.Drawing.Point(13, 16);
            this.radButtonNuevo.Name = "radButtonNuevo";
            this.radButtonNuevo.Size = new System.Drawing.Size(138, 44);
            this.radButtonNuevo.TabIndex = 26;
            this.radButtonNuevo.Text = "Nuevo";
            this.radButtonNuevo.Click += new System.EventHandler(this.RadButtonNuevo_Click);
            this.radButtonNuevo.MouseEnter += new System.EventHandler(this.RadButtonNuevo_MouseEnter);
            this.radButtonNuevo.MouseLeave += new System.EventHandler(this.RadButtonNuevo_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNuevo.GetChildAt(0))).Text = "Nuevo";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonNuevo.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEditar
            // 
            this.radButtonEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEditar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEditar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEditar.Location = new System.Drawing.Point(13, 77);
            this.radButtonEditar.Name = "radButtonEditar";
            this.radButtonEditar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEditar.TabIndex = 27;
            this.radButtonEditar.Text = "Editar";
            this.radButtonEditar.Click += new System.EventHandler(this.RadButtonEditar_Click);
            this.radButtonEditar.MouseEnter += new System.EventHandler(this.RadButtonEditar_MouseEnter);
            this.radButtonEditar.MouseLeave += new System.EventHandler(this.RadButtonEditar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEditar.GetChildAt(0))).Text = "Editar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEditar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1011, 45);
            this.radPanelMenuPath.TabIndex = 186;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(361, 29);
            this.radLabelTitulo.TabIndex = 29;
            this.radLabelTitulo.Text = "Mantenimientos / Tablas Maestras / Zona";
            // 
            // frmMtoGLM11Sel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1143, 881);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoGLM11Sel";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Mantenimiento de Zonas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMtoGLM11Sel_FormClosing);
            this.Load += new System.EventHandler(this.FrmMtoGLM11Sel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilterGridInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClaseZona)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxClaseZona)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewZonas.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewZonas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel lblClaseZona;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxClaseZona;
        private Telerik.WinControls.UI.RadButtonElement radButtonElementClaseZona;
        //private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanelDataFilter;
        private Telerik.WinControls.UI.RadGridView radGridViewZonas;
        private Telerik.WinControls.UI.RadDataFilter radDataFilterGridInfo;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private Telerik.WinControls.UI.RadButton radButtonExport;
    }
}