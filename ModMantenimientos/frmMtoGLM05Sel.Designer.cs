namespace ModMantenimientos
{
    partial class frmMtoGLM05Sel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoGLM05Sel));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.lblTipoAux = new Telerik.WinControls.UI.RadLabel();
            this.lblExterno = new Telerik.WinControls.UI.RadLabel();
            this.radButtonTextBoxTipoAux = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonElementTipoAux = new Telerik.WinControls.UI.RadButtonElement();
            this.radGridViewCuentasAux = new Telerik.WinControls.UI.RadGridView();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoAux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExterno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxTipoAux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCuentasAux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCuentasAux.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Controls.Add(this.lblTipoAux);
            this.radPanelApp.Controls.Add(this.lblExterno);
            this.radPanelApp.Controls.Add(this.radButtonTextBoxTipoAux);
            this.radPanelApp.Controls.Add(this.radGridViewCuentasAux);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 550);
            this.radPanelApp.TabIndex = 186;
            this.radPanelApp.Resize += new System.EventHandler(this.radPanelApp_Resize);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(19, 81);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(169, 19);
            this.radLabelNoHayInfo.TabIndex = 184;
            this.radLabelNoHayInfo.Text = "No existen cuentas de auxiliar";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // lblTipoAux
            // 
            this.lblTipoAux.Location = new System.Drawing.Point(19, 16);
            this.lblTipoAux.Name = "lblTipoAux";
            this.lblTipoAux.Size = new System.Drawing.Size(91, 19);
            this.lblTipoAux.TabIndex = 5;
            this.lblTipoAux.Text = "Tipo de Auxiliar";
            // 
            // lblExterno
            // 
            this.lblExterno.Location = new System.Drawing.Point(454, 15);
            this.lblExterno.Name = "lblExterno";
            this.lblExterno.Size = new System.Drawing.Size(59, 19);
            this.lblExterno.TabIndex = 23;
            this.lblExterno.Text = "EXTERNO";
            this.lblExterno.Visible = false;
            // 
            // radButtonTextBoxTipoAux
            // 
            this.radButtonTextBoxTipoAux.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radButtonTextBoxTipoAux.Location = new System.Drawing.Point(19, 43);
            this.radButtonTextBoxTipoAux.MaxLength = 3;
            this.radButtonTextBoxTipoAux.Name = "radButtonTextBoxTipoAux";
            this.radButtonTextBoxTipoAux.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonTextBoxTipoAux.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElementTipoAux});
            this.radButtonTextBoxTipoAux.Size = new System.Drawing.Size(285, 30);
            this.radButtonTextBoxTipoAux.TabIndex = 182;
            this.radButtonTextBoxTipoAux.TextChanged += new System.EventHandler(this.RadButtonTextBoxTipoAux_TextChanged);
            this.radButtonTextBoxTipoAux.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RadButtonTextBoxTipoAux_KeyPress);
            // 
            // radButtonElementTipoAux
            // 
            this.radButtonElementTipoAux.AutoSize = true;
            this.radButtonElementTipoAux.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radButtonElementTipoAux.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElementTipoAux.Image")));
            this.radButtonElementTipoAux.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonElementTipoAux.Name = "radButtonElementTipoAux";
            this.radButtonElementTipoAux.Text = "";
            this.radButtonElementTipoAux.UseCompatibleTextRendering = false;
            this.radButtonElementTipoAux.Click += new System.EventHandler(this.RadButtonElementTipoAux_Click);
            // 
            // radGridViewCuentasAux
            // 
            this.radGridViewCuentasAux.AutoScroll = true;
            this.radGridViewCuentasAux.Location = new System.Drawing.Point(19, 92);
            // 
            // 
            // 
            this.radGridViewCuentasAux.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewCuentasAux.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewCuentasAux.MasterTemplate.AllowEditRow = false;
            this.radGridViewCuentasAux.MasterTemplate.MultiSelect = true;
            this.radGridViewCuentasAux.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewCuentasAux.Name = "radGridViewCuentasAux";
            this.radGridViewCuentasAux.Size = new System.Drawing.Size(770, 415);
            this.radGridViewCuentasAux.TabIndex = 183;
            this.radGridViewCuentasAux.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewCuentasAux_ViewCellFormatting);
            this.radGridViewCuentasAux.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewCuentasAux_CellClick);
            this.radGridViewCuentasAux.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewCuentasAux_CellDoubleClick);
            this.radGridViewCuentasAux.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radGridViewCuentasAux_KeyPress);
            this.radGridViewCuentasAux.Leave += new System.EventHandler(this.radGridViewCuentasAux_Leave);
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonExport);
            this.radPanelAcciones.Controls.Add(this.radButtonEditar);
            this.radPanelAcciones.Controls.Add(this.radButtonNuevo);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 442);
            this.radPanelAcciones.TabIndex = 185;
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
            this.radButtonExport.TabIndex = 31;
            this.radButtonExport.Text = "Exportar";
            this.radButtonExport.Click += new System.EventHandler(this.RadButtonExport_Click);
            this.radButtonExport.MouseEnter += new System.EventHandler(this.RadButtonExport_MouseEnter);
            this.radButtonExport.MouseLeave += new System.EventHandler(this.RadButtonExport_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExport.GetChildAt(0))).Text = "Exportar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExport.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEditar
            // 
            this.radButtonEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEditar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEditar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEditar.Location = new System.Drawing.Point(13, 77);
            this.radButtonEditar.Name = "radButtonEditar";
            this.radButtonEditar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEditar.TabIndex = 25;
            this.radButtonEditar.Text = "Editar";
            this.radButtonEditar.Click += new System.EventHandler(this.RadButtonEditar_Click);
            this.radButtonEditar.MouseEnter += new System.EventHandler(this.RadButtonEditar_MouseEnter);
            this.radButtonEditar.MouseLeave += new System.EventHandler(this.RadButtonEditar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEditar.GetChildAt(0))).Text = "Editar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEditar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonNuevo
            // 
            this.radButtonNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonNuevo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonNuevo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonNuevo.Location = new System.Drawing.Point(13, 16);
            this.radButtonNuevo.Name = "radButtonNuevo";
            this.radButtonNuevo.Size = new System.Drawing.Size(138, 44);
            this.radButtonNuevo.TabIndex = 24;
            this.radButtonNuevo.Text = "Nuevo";
            this.radButtonNuevo.Click += new System.EventHandler(this.RadButtonNuevo_Click);
            this.radButtonNuevo.MouseEnter += new System.EventHandler(this.RadButtonNuevo_MouseEnter);
            this.radButtonNuevo.MouseLeave += new System.EventHandler(this.RadButtonNuevo_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNuevo.GetChildAt(0))).Text = "Nuevo";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonNuevo.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1011, 45);
            this.radPanelMenuPath.TabIndex = 184;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(481, 29);
            this.radLabelTitulo.TabIndex = 28;
            this.radLabelTitulo.Text = "Mantenimientos / Tablas Maestras / Cuentas de Auxiliar";
            // 
            // frmMtoGLM05Sel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1127, 943);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoGLM05Sel";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Mantenimiento de Cuentas de Auxiliar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMtoGLM05Sel_FormClosing);
            this.Load += new System.EventHandler(this.FrmMtoGLM05Sel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoAux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExterno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxTipoAux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCuentasAux.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewCuentasAux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel lblTipoAux;
        private Telerik.WinControls.UI.RadLabel lblExterno;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxTipoAux;
        private Telerik.WinControls.UI.RadButtonElement radButtonElementTipoAux;
        private Telerik.WinControls.UI.RadGridView radGridViewCuentasAux;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private Telerik.WinControls.UI.RadButton radButtonExport;
    }
}