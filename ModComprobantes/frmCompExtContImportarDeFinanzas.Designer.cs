namespace ModComprobantes
{
    partial class frmCompExtContImportarDeFinanzas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompExtContImportarDeFinanzas));
            this.btnBuscar = new Telerik.WinControls.UI.RadButton();
            this.radGridViewComprobantes = new Telerik.WinControls.UI.RadGridView();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelHeader = new Telerik.WinControls.UI.RadLabel();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonImportar = new Telerik.WinControls.UI.RadButton();
            this.lblResult = new Telerik.WinControls.UI.RadLabel();
            this.progressBarEspera = new ModComprobantes.NewProgressBar();
            this.gbCabecera = new Telerik.WinControls.UI.RadGroupBox();
            this.cmbTipo = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbCompania = new Telerik.WinControls.UI.RadDropDownList();
            this.txtNoComprobante = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblNoComprobante = new Telerik.WinControls.UI.RadLabel();
            this.txtMaskAAPP = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.lblTipo = new Telerik.WinControls.UI.RadLabel();
            this.lblAAPP = new Telerik.WinControls.UI.RadLabel();
            this.lblCompania = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.btnBuscar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewComprobantes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewComprobantes.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonImportar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbCabecera)).BeginInit();
            this.gbCabecera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompania)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompania)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBuscar.Location = new System.Drawing.Point(663, 32);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(102, 39);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            this.btnBuscar.MouseEnter += new System.EventHandler(this.BtnBuscar_MouseEnter);
            this.btnBuscar.MouseLeave += new System.EventHandler(this.BtnBuscar_MouseLeave);
            // 
            // radGridViewComprobantes
            // 
            this.radGridViewComprobantes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridViewComprobantes.AutoScroll = true;
            this.radGridViewComprobantes.Location = new System.Drawing.Point(36, 163);
            // 
            // 
            // 
            this.radGridViewComprobantes.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewComprobantes.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewComprobantes.MasterTemplate.AllowEditRow = false;
            this.radGridViewComprobantes.MasterTemplate.AllowSearchRow = true;
            this.radGridViewComprobantes.MasterTemplate.EnableFiltering = true;
            this.radGridViewComprobantes.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewComprobantes.Name = "radGridViewComprobantes";
            this.radGridViewComprobantes.Size = new System.Drawing.Size(910, 406);
            this.radGridViewComprobantes.TabIndex = 194;
            this.radGridViewComprobantes.Visible = false;
            this.radGridViewComprobantes.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewComprobantes_CellDoubleClick);
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelHeader);
            this.radPanelMenuPath.Location = new System.Drawing.Point(1, 3);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(945, 45);
            this.radPanelMenuPath.TabIndex = 193;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelHeader
            // 
            this.radLabelHeader.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelHeader.Location = new System.Drawing.Point(13, 15);
            this.radLabelHeader.Name = "radLabelHeader";
            this.radLabelHeader.Size = new System.Drawing.Size(459, 29);
            this.radLabelHeader.TabIndex = 167;
            this.radLabelHeader.Text = "Comprobantes extracontables / Importar de Finanzas";
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(828, 110);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(115, 39);
            this.radButtonExit.TabIndex = 191;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.RadButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.RadButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.RadButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonImportar
            // 
            this.radButtonImportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonImportar.Enabled = false;
            this.radButtonImportar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonImportar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonImportar.Location = new System.Drawing.Point(828, 65);
            this.radButtonImportar.Name = "radButtonImportar";
            this.radButtonImportar.Size = new System.Drawing.Size(115, 39);
            this.radButtonImportar.TabIndex = 190;
            this.radButtonImportar.Text = "Importar";
            this.radButtonImportar.Click += new System.EventHandler(this.RadButtonImportar_Click);
            this.radButtonImportar.MouseEnter += new System.EventHandler(this.RadButtonImportar_MouseEnter);
            this.radButtonImportar.MouseLeave += new System.EventHandler(this.RadButtonImportar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonImportar.GetChildAt(0))).Text = "Importar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonImportar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // lblResult
            // 
            this.lblResult.Location = new System.Drawing.Point(33, 170);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(33, 19);
            this.lblResult.TabIndex = 3;
            this.lblResult.Text = "Error";
            this.lblResult.Visible = false;
            // 
            // progressBarEspera
            // 
            this.progressBarEspera.Location = new System.Drawing.Point(320, 171);
            this.progressBarEspera.Maximum = 1000;
            this.progressBarEspera.Name = "progressBarEspera";
            this.progressBarEspera.Size = new System.Drawing.Size(244, 23);
            this.progressBarEspera.TabIndex = 6;
            this.progressBarEspera.Visible = false;
            // 
            // gbCabecera
            // 
            this.gbCabecera.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbCabecera.Controls.Add(this.cmbTipo);
            this.gbCabecera.Controls.Add(this.cmbCompania);
            this.gbCabecera.Controls.Add(this.txtNoComprobante);
            this.gbCabecera.Controls.Add(this.lblNoComprobante);
            this.gbCabecera.Controls.Add(this.txtMaskAAPP);
            this.gbCabecera.Controls.Add(this.btnBuscar);
            this.gbCabecera.Controls.Add(this.lblTipo);
            this.gbCabecera.Controls.Add(this.lblAAPP);
            this.gbCabecera.Controls.Add(this.lblCompania);
            this.gbCabecera.HeaderText = " Buscador ";
            this.gbCabecera.Location = new System.Drawing.Point(36, 57);
            this.gbCabecera.Name = "gbCabecera";
            this.gbCabecera.Size = new System.Drawing.Size(775, 92);
            this.gbCabecera.TabIndex = 0;
            this.gbCabecera.TabStop = false;
            this.gbCabecera.Text = " Buscador ";
            // 
            // cmbTipo
            // 
            this.cmbTipo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbTipo.Location = new System.Drawing.Point(369, 48);
            this.cmbTipo.MaxLength = 2;
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Padding = new System.Windows.Forms.Padding(3);
            this.cmbTipo.Size = new System.Drawing.Size(163, 27);
            this.cmbTipo.TabIndex = 185;
            // 
            // cmbCompania
            // 
            this.cmbCompania.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbCompania.Location = new System.Drawing.Point(20, 48);
            this.cmbCompania.MaxLength = 2;
            this.cmbCompania.Name = "cmbCompania";
            this.cmbCompania.Padding = new System.Windows.Forms.Padding(3);
            this.cmbCompania.Size = new System.Drawing.Size(258, 27);
            this.cmbCompania.TabIndex = 184;
            // 
            // txtNoComprobante
            // 
            this.txtNoComprobante.Location = new System.Drawing.Point(549, 48);
            this.txtNoComprobante.MaxLength = 6;
            this.txtNoComprobante.Name = "txtNoComprobante";
            this.txtNoComprobante.Padding = new System.Windows.Forms.Padding(5);
            this.txtNoComprobante.Size = new System.Drawing.Size(99, 30);
            this.txtNoComprobante.TabIndex = 57;
            this.txtNoComprobante.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtNoComprobante_KeyPress);
            // 
            // lblNoComprobante
            // 
            this.lblNoComprobante.Location = new System.Drawing.Point(546, 26);
            this.lblNoComprobante.Name = "lblNoComprobante";
            this.lblNoComprobante.Size = new System.Drawing.Size(102, 19);
            this.lblNoComprobante.TabIndex = 56;
            this.lblNoComprobante.Text = "No Comprobante";
            // 
            // txtMaskAAPP
            // 
            this.txtMaskAAPP.Location = new System.Drawing.Point(290, 48);
            this.txtMaskAAPP.Mask = "00-00";
            this.txtMaskAAPP.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.txtMaskAAPP.Name = "txtMaskAAPP";
            this.txtMaskAAPP.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaskAAPP.Size = new System.Drawing.Size(66, 28);
            this.txtMaskAAPP.TabIndex = 49;
            this.txtMaskAAPP.TabStop = false;
            this.txtMaskAAPP.Text = "__-__";
            // 
            // lblTipo
            // 
            this.lblTipo.Location = new System.Drawing.Point(369, 26);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(30, 19);
            this.lblTipo.TabIndex = 50;
            this.lblTipo.Text = "Tipo";
            // 
            // lblAAPP
            // 
            this.lblAAPP.Location = new System.Drawing.Point(290, 26);
            this.lblAAPP.Name = "lblAAPP";
            this.lblAAPP.Size = new System.Drawing.Size(76, 19);
            this.lblAAPP.TabIndex = 48;
            this.lblAAPP.Text = "Año-Período";
            // 
            // lblCompania
            // 
            this.lblCompania.Location = new System.Drawing.Point(20, 26);
            this.lblCompania.Name = "lblCompania";
            this.lblCompania.Size = new System.Drawing.Size(62, 19);
            this.lblCompania.TabIndex = 46;
            this.lblCompania.Text = "Compañía";
            // 
            // frmCompExtContImportarDeFinanzas
            // 
            this.AcceptButton = this.btnBuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(1109, 757);
            this.Controls.Add(this.radGridViewComprobantes);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonImportar);
            this.Controls.Add(this.progressBarEspera);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.gbCabecera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompExtContImportarDeFinanzas";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Importar Comprobantes Extracontables de Finanzas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCompExtContImportarDeFinanzas_FormClosing);
            this.Load += new System.EventHandler(this.FrmCompExtContImportarDeFinanzas_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCompExtContImportarDeFinanzas_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.btnBuscar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewComprobantes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewComprobantes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonImportar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbCabecera)).EndInit();
            this.gbCabecera.ResumeLayout(false);
            this.gbCabecera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompania)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompania)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Telerik.WinControls.UI.RadGroupBox gbCabecera;
        private Telerik.WinControls.UI.RadMaskedEditBox txtMaskAAPP;
        private Telerik.WinControls.UI.RadLabel lblTipo;
        private Telerik.WinControls.UI.RadLabel lblAAPP;
        private Telerik.WinControls.UI.RadLabel lblCompania;
        private Telerik.WinControls.UI.RadTextBoxControl txtNoComprobante;
        private Telerik.WinControls.UI.RadLabel lblNoComprobante;
        private Telerik.WinControls.UI.RadButton btnBuscar;
        private Telerik.WinControls.UI.RadLabel lblResult;
        private NewProgressBar progressBarEspera;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonImportar;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelHeader;
        private Telerik.WinControls.UI.RadDropDownList cmbTipo;
        private Telerik.WinControls.UI.RadDropDownList cmbCompania;
        private Telerik.WinControls.UI.RadGridView radGridViewComprobantes;
    }
}