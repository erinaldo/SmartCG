namespace ModMantenimientos
{
    partial class frmMtoATM05Sel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoATM05Sel));
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewUsuarios = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewUsuarios.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
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
            this.radPanelAcciones.TabIndex = 34;
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
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1025, 45);
            this.radPanelMenuPath.TabIndex = 188;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(380, 29);
            this.radLabelTitulo.TabIndex = 28;
            this.radLabelTitulo.Text = "Mantenimientos / Autorizaciones / Usuarios";
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radGridViewUsuarios);
            this.radPanelApp.Location = new System.Drawing.Point(169, 61);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 474);
            this.radPanelApp.TabIndex = 189;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewUsuarios
            // 
            this.radGridViewUsuarios.AutoScroll = true;
            this.radGridViewUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewUsuarios.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridViewUsuarios.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewUsuarios.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewUsuarios.MasterTemplate.AllowEditRow = false;
            this.radGridViewUsuarios.MasterTemplate.MultiSelect = true;
            this.radGridViewUsuarios.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewUsuarios.Name = "radGridViewUsuarios";
            this.radGridViewUsuarios.Size = new System.Drawing.Size(836, 474);
            this.radGridViewUsuarios.TabIndex = 187;
            this.radGridViewUsuarios.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewUsuarios_ViewCellFormatting);
            this.radGridViewUsuarios.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewUsuarios_CellClick);
            this.radGridViewUsuarios.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewUsuarios_CellDoubleClick);
            this.radGridViewUsuarios.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radGridViewUsuarios_KeyPress);
            // 
            // frmMtoATM05Sel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 850);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radPanelAcciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoATM05Sel";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Mantenimiento de Usuarios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMtoATM05Sel_FormClosing);
            this.Load += new System.EventHandler(this.FrmMtoATM05Sel_Load);
            this.Shown += new System.EventHandler(this.FrmMtoATM05Sel_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewUsuarios.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewUsuarios;
        private Telerik.WinControls.UI.RadButton radButtonExport;
    }
}