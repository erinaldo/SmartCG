namespace ModMantenimientos
{
    partial class frmMtoIVT01Sel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoIVT01Sel));
            this.lblPlan = new System.Windows.Forms.Label();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radButtonCopiarCodigoIVA = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.radGridViewCodIva = new Telerik.WinControls.UI.RadGridView();
            this.radButtonTextBoxPlan = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonElementPlan = new Telerik.WinControls.UI.RadButtonElement();
            this.radDataFilterGridInfo = new Telerik.WinControls.UI.RadDataFilter();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCopiarCodigoIVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCodIva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCodIva.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilterGridInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPlan
            // 
            this.lblPlan.AutoSize = true;
            this.lblPlan.Location = new System.Drawing.Point(19, 16);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(30, 15);
            this.lblPlan.TabIndex = 11;
            this.lblPlan.Text = "Plan";
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonExport);
            this.radPanelAcciones.Controls.Add(this.radButtonNuevo);
            this.radPanelAcciones.Controls.Add(this.radButtonEditar);
            this.radPanelAcciones.Controls.Add(this.radButtonCopiarCodigoIVA);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 442);
            this.radPanelAcciones.TabIndex = 33;
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
            this.radButtonExport.TabIndex = 30;
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
            this.radButtonNuevo.TabIndex = 28;
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
            this.radButtonEditar.TabIndex = 29;
            this.radButtonEditar.Text = "Editar";
            this.radButtonEditar.Click += new System.EventHandler(this.RadButtonEditar_Click);
            this.radButtonEditar.MouseEnter += new System.EventHandler(this.RadButtonEditar_MouseEnter);
            this.radButtonEditar.MouseLeave += new System.EventHandler(this.RadButtonEditar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEditar.GetChildAt(0))).Text = "Editar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEditar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonCopiarCodigoIVA
            // 
            this.radButtonCopiarCodigoIVA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCopiarCodigoIVA.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCopiarCodigoIVA.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCopiarCodigoIVA.Location = new System.Drawing.Point(13, 199);
            this.radButtonCopiarCodigoIVA.Name = "radButtonCopiarCodigoIVA";
            this.radButtonCopiarCodigoIVA.Size = new System.Drawing.Size(138, 44);
            this.radButtonCopiarCodigoIVA.TabIndex = 31;
            this.radButtonCopiarCodigoIVA.Text = "Copiar Código IVA";
            this.radButtonCopiarCodigoIVA.Click += new System.EventHandler(this.RadButtonCopiarCodigoIVA_Click);
            this.radButtonCopiarCodigoIVA.MouseEnter += new System.EventHandler(this.RadButtonCopiarCodigoIVA_MouseEnter);
            this.radButtonCopiarCodigoIVA.MouseLeave += new System.EventHandler(this.RadButtonCopiarCodigoIVA_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCopiarCodigoIVA.GetChildAt(0))).Text = "Copiar Código IVA";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCopiarCodigoIVA.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1011, 45);
            this.radPanelMenuPath.TabIndex = 34;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(450, 29);
            this.radLabelTitulo.TabIndex = 32;
            this.radLabelTitulo.Text = "Mantenimientos / Tablas Maestras / Códigos de IVA";
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Controls.Add(this.radGridViewCodIva);
            this.radPanelApp.Controls.Add(this.radButtonTextBoxPlan);
            this.radPanelApp.Controls.Add(this.lblPlan);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(789, 518);
            this.radPanelApp.TabIndex = 35;
            this.radPanelApp.Resize += new System.EventHandler(this.RadPanelApp_Resize);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(19, 81);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(150, 19);
            this.radLabelNoHayInfo.TabIndex = 186;
            this.radLabelNoHayInfo.Text = "No existen códigos de IVA";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // radGridViewCodIva
            // 
            this.radGridViewCodIva.AutoScroll = true;
            this.radGridViewCodIva.Location = new System.Drawing.Point(19, 92);
            // 
            // 
            // 
            this.radGridViewCodIva.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewCodIva.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewCodIva.MasterTemplate.AllowEditRow = false;
            this.radGridViewCodIva.MasterTemplate.MultiSelect = true;
            this.radGridViewCodIva.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewCodIva.Name = "radGridViewCodIva";
            this.radGridViewCodIva.Size = new System.Drawing.Size(770, 409);
            this.radGridViewCodIva.TabIndex = 185;
            this.radGridViewCodIva.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewCodIva_ViewCellFormatting);
            this.radGridViewCodIva.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewCodIva_CellClick);
            this.radGridViewCodIva.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewCodIva_CellDoubleClick);
            // 
            // radButtonTextBoxPlan
            // 
            this.radButtonTextBoxPlan.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radButtonTextBoxPlan.Location = new System.Drawing.Point(22, 42);
            this.radButtonTextBoxPlan.MaxLength = 1;
            this.radButtonTextBoxPlan.Name = "radButtonTextBoxPlan";
            this.radButtonTextBoxPlan.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonTextBoxPlan.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElementPlan});
            this.radButtonTextBoxPlan.Size = new System.Drawing.Size(285, 30);
            this.radButtonTextBoxPlan.TabIndex = 183;
            this.radButtonTextBoxPlan.TextChanged += new System.EventHandler(this.RadButtonTextBoxPlan_TextChanged);
            // 
            // radButtonElementPlan
            // 
            this.radButtonElementPlan.AutoSize = true;
            this.radButtonElementPlan.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radButtonElementPlan.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElementPlan.Image")));
            this.radButtonElementPlan.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonElementPlan.Name = "radButtonElementPlan";
            this.radButtonElementPlan.Text = "";
            this.radButtonElementPlan.UseCompatibleTextRendering = false;
            this.radButtonElementPlan.Click += new System.EventHandler(this.RadButtonElementPlan_Click);
            // 
            // radDataFilterGridInfo
            // 
            this.radDataFilterGridInfo.AllowDragDrop = false;
            this.radDataFilterGridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDataFilterGridInfo.LineColor = System.Drawing.Color.Gray;
            this.radDataFilterGridInfo.Location = new System.Drawing.Point(0, 0);
            this.radDataFilterGridInfo.Name = "radDataFilterGridInfo";
            this.radDataFilterGridInfo.Size = new System.Drawing.Size(558, 172);
            this.radDataFilterGridInfo.TabIndex = 1;
            this.radDataFilterGridInfo.NodeFormatting += new Telerik.WinControls.UI.TreeNodeFormattingEventHandler(this.RadDataFilterGridInfo_NodeFormatting);
            // 
            // frmMtoIVT01Sel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1215, 1003);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radPanelAcciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoIVT01Sel";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Mantenimiento de Códigos de IVA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMtoIVT01Sel_FormClosing);
            this.Load += new System.EventHandler(this.FrmMtoIVT01Sel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCopiarCodigoIVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCodIva.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCodIva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilterGridInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPlan;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadButton radButtonCopiarCodigoIVA;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxPlan;
        private Telerik.WinControls.UI.RadButtonElement radButtonElementPlan;
        private Telerik.WinControls.UI.RadDataFilter radDataFilterGridInfo;
        private Telerik.WinControls.UI.RadGridView radGridViewCodIva;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private Telerik.WinControls.UI.RadButton radButtonExport;
    }
}