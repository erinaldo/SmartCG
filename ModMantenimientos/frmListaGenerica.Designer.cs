namespace ModMantenimientos
{
    partial class frmListaGenerica
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListaGenerica));
            this.lblNoHayInfo = new System.Windows.Forms.Label();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewInfo = new Telerik.WinControls.UI.RadGridView();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelHeader = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNoHayInfo
            // 
            this.lblNoHayInfo.AutoSize = true;
            this.lblNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.lblNoHayInfo.Location = new System.Drawing.Point(-3, 0);
            this.lblNoHayInfo.Name = "lblNoHayInfo";
            this.lblNoHayInfo.Size = new System.Drawing.Size(107, 15);
            this.lblNoHayInfo.TabIndex = 30;
            this.lblNoHayInfo.Text = "NoExistenRegistros";
            this.lblNoHayInfo.Visible = false;
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radGridViewInfo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 61);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 474);
            this.radPanelApp.TabIndex = 187;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewInfo
            // 
            this.radGridViewInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewInfo.Location = new System.Drawing.Point(0, 0);
            this.radGridViewInfo.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            // 
            // 
            // 
            this.radGridViewInfo.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewInfo.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewInfo.MasterTemplate.MultiSelect = true;
            this.radGridViewInfo.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewInfo.Name = "radGridViewInfo";
            this.radGridViewInfo.Size = new System.Drawing.Size(836, 474);
            this.radGridViewInfo.TabIndex = 34;
            this.radGridViewInfo.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewInfo_ViewCellFormatting);
            this.radGridViewInfo.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInfo_CellClick);
            this.radGridViewInfo.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInfo_CellDoubleClick);
            this.radGridViewInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radGridViewInfo_KeyPress);
            this.radGridViewInfo.Leave += new System.EventHandler(this.radGridViewInfo_Leave);
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
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 284);
            this.radPanelAcciones.TabIndex = 186;
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
            this.radButtonExport.TabIndex = 167;
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
            this.radButtonNuevo.ForeColor = System.Drawing.SystemColors.Window;
            this.radButtonNuevo.Location = new System.Drawing.Point(13, 16);
            this.radButtonNuevo.Name = "radButtonNuevo";
            this.radButtonNuevo.Size = new System.Drawing.Size(138, 44);
            this.radButtonNuevo.TabIndex = 165;
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
            this.radButtonEditar.ForeColor = System.Drawing.SystemColors.Window;
            this.radButtonEditar.Location = new System.Drawing.Point(13, 77);
            this.radButtonEditar.Name = "radButtonEditar";
            this.radButtonEditar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEditar.TabIndex = 166;
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
            this.radPanelMenuPath.Controls.Add(this.radLabelHeader);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1025, 45);
            this.radPanelMenuPath.TabIndex = 185;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelHeader
            // 
            this.radLabelHeader.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelHeader.Location = new System.Drawing.Point(13, 15);
            this.radLabelHeader.Name = "radLabelHeader";
            this.radLabelHeader.Size = new System.Drawing.Size(165, 29);
            this.radLabelHeader.TabIndex = 168;
            this.radLabelHeader.Text = "Mantenimiento de";
            // 
            // frmListaGenerica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1265, 1097);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmListaGenerica";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmListaGenerica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmListaGenerica_FormClosing);
            this.Load += new System.EventHandler(this.FrmListaGenerica_Load);
            this.Shown += new System.EventHandler(this.FrmListaGenerica_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblNoHayInfo;
        private Telerik.WinControls.UI.RadGridView radGridViewInfo;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadLabel radLabelHeader;
        //private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanelDataFilter;
        //private Telerik.WinControls.UI.RadDataFilter radDataFilterGridInfo;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadButton radButtonExport;
    }
}