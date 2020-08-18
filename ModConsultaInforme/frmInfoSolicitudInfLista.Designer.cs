namespace ModConsultaInforme
{
    partial class frmInfoSolicitudInfLista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfoSolicitudInfLista));
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonEjecutarInforme = new Telerik.WinControls.UI.RadButton();
            this.chkMonedaAlternativa = new Telerik.WinControls.UI.RadCheckBox();
            this.txtDiasPermanencia = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblDiasPermanencia = new Telerik.WinControls.UI.RadLabel();
            this.txtNumPeriodosAno = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblNumPeriodosAno = new Telerik.WinControls.UI.RadLabel();
            this.txtMaskAAPP = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.lblAAPP = new Telerik.WinControls.UI.RadLabel();
            this.groupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGridViewInformeLista = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEjecutarInforme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMonedaAlternativa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiasPermanencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDiasPermanencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumPeriodosAno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNumPeriodosAno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformeLista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformeLista.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(475, 519);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(138, 44);
            this.radButtonExit.TabIndex = 55;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.RadButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.RadButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.RadButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEjecutarInforme
            // 
            this.radButtonEjecutarInforme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEjecutarInforme.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonEjecutarInforme.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEjecutarInforme.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEjecutarInforme.Location = new System.Drawing.Point(281, 519);
            this.radButtonEjecutarInforme.Name = "radButtonEjecutarInforme";
            this.radButtonEjecutarInforme.Size = new System.Drawing.Size(138, 44);
            this.radButtonEjecutarInforme.TabIndex = 50;
            this.radButtonEjecutarInforme.Text = "Ejecutar Informe";
            this.radButtonEjecutarInforme.Click += new System.EventHandler(this.RadButtonEjecutarInforme_Click);
            this.radButtonEjecutarInforme.MouseEnter += new System.EventHandler(this.RadButtonEjecutarInforme_MouseEnter);
            this.radButtonEjecutarInforme.MouseLeave += new System.EventHandler(this.RadButtonEjecutarInforme_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEjecutarInforme.GetChildAt(0))).Text = "Ejecutar Informe";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEjecutarInforme.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // chkMonedaAlternativa
            // 
            this.chkMonedaAlternativa.Location = new System.Drawing.Point(689, 417);
            this.chkMonedaAlternativa.Name = "chkMonedaAlternativa";
            this.chkMonedaAlternativa.Size = new System.Drawing.Size(131, 19);
            this.chkMonedaAlternativa.TabIndex = 45;
            this.chkMonedaAlternativa.Text = "Moneda Alternativa";
            // 
            // txtDiasPermanencia
            // 
            this.txtDiasPermanencia.Location = new System.Drawing.Point(480, 440);
            this.txtDiasPermanencia.MaxLength = 3;
            this.txtDiasPermanencia.Name = "txtDiasPermanencia";
            this.txtDiasPermanencia.Padding = new System.Windows.Forms.Padding(5);
            this.txtDiasPermanencia.Size = new System.Drawing.Size(44, 30);
            this.txtDiasPermanencia.TabIndex = 40;
            this.txtDiasPermanencia.Text = "010";
            this.txtDiasPermanencia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDiasPermanencia_KeyPress);
            // 
            // lblDiasPermanencia
            // 
            this.lblDiasPermanencia.Location = new System.Drawing.Point(477, 417);
            this.lblDiasPermanencia.Name = "lblDiasPermanencia";
            this.lblDiasPermanencia.Size = new System.Drawing.Size(121, 19);
            this.lblDiasPermanencia.TabIndex = 35;
            this.lblDiasPermanencia.Text = "Días de permanencia";
            // 
            // txtNumPeriodosAno
            // 
            this.txtNumPeriodosAno.Location = new System.Drawing.Point(262, 440);
            this.txtNumPeriodosAno.MaxLength = 2;
            this.txtNumPeriodosAno.Name = "txtNumPeriodosAno";
            this.txtNumPeriodosAno.Padding = new System.Windows.Forms.Padding(5);
            this.txtNumPeriodosAno.Size = new System.Drawing.Size(37, 30);
            this.txtNumPeriodosAno.TabIndex = 30;
            this.txtNumPeriodosAno.Text = "13";
            this.txtNumPeriodosAno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtNumPeriodosAno_KeyPress);
            // 
            // lblNumPeriodosAno
            // 
            this.lblNumPeriodosAno.Location = new System.Drawing.Point(259, 417);
            this.lblNumPeriodosAno.Name = "lblNumPeriodosAno";
            this.lblNumPeriodosAno.Size = new System.Drawing.Size(166, 19);
            this.lblNumPeriodosAno.TabIndex = 25;
            this.lblNumPeriodosAno.Text = "Número de periodos por año";
            // 
            // txtMaskAAPP
            // 
            this.txtMaskAAPP.Location = new System.Drawing.Point(72, 440);
            this.txtMaskAAPP.Mask = "00-00";
            this.txtMaskAAPP.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.txtMaskAAPP.Name = "txtMaskAAPP";
            this.txtMaskAAPP.Padding = new System.Windows.Forms.Padding(3);
            this.txtMaskAAPP.Size = new System.Drawing.Size(45, 24);
            this.txtMaskAAPP.TabIndex = 20;
            this.txtMaskAAPP.TabStop = false;
            this.txtMaskAAPP.Text = "__-__";
            // 
            // lblAAPP
            // 
            this.lblAAPP.Location = new System.Drawing.Point(69, 417);
            this.lblAAPP.Name = "lblAAPP";
            this.lblAAPP.Size = new System.Drawing.Size(105, 19);
            this.lblAAPP.TabIndex = 15;
            this.lblAAPP.Text = "Año-Periodo Base";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.groupBox1.Controls.Add(this.radGridViewInformeLista);
            this.groupBox1.HeaderText = " Informes ";
            this.groupBox1.Location = new System.Drawing.Point(46, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(784, 331);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Informes ";
            // 
            // radGridViewInformeLista
            // 
            this.radGridViewInformeLista.Location = new System.Drawing.Point(37, 34);
            // 
            // 
            // 
            this.radGridViewInformeLista.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewInformeLista.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewInformeLista.MasterTemplate.AllowEditRow = false;
            this.radGridViewInformeLista.MasterTemplate.MultiSelect = true;
            this.radGridViewInformeLista.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewInformeLista.Name = "radGridViewInformeLista";
            this.radGridViewInformeLista.Size = new System.Drawing.Size(715, 274);
            this.radGridViewInformeLista.TabIndex = 10;
            this.radGridViewInformeLista.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInformeLista_CellClick);
            // 
            // frmInfoSolicitudInfLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.CancelButton = this.radButtonExit;
            this.ClientSize = new System.Drawing.Size(880, 593);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonEjecutarInforme);
            this.Controls.Add(this.chkMonedaAlternativa);
            this.Controls.Add(this.txtDiasPermanencia);
            this.Controls.Add(this.lblDiasPermanencia);
            this.Controls.Add(this.txtNumPeriodosAno);
            this.Controls.Add(this.lblNumPeriodosAno);
            this.Controls.Add(this.txtMaskAAPP);
            this.Controls.Add(this.lblAAPP);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInfoSolicitudInfLista";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista de informes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInfoSolicitudInfLista_FormClosing);
            this.Load += new System.EventHandler(this.FrmInfoSolicitudInfLista_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmInfoSolicitudInfLista_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEjecutarInforme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMonedaAlternativa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiasPermanencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDiasPermanencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumPeriodosAno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNumPeriodosAno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformeLista.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformeLista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBox1;
        private Telerik.WinControls.UI.RadLabel lblAAPP;
        private Telerik.WinControls.UI.RadMaskedEditBox txtMaskAAPP;
        private Telerik.WinControls.UI.RadLabel lblNumPeriodosAno;
        private Telerik.WinControls.UI.RadTextBoxControl txtNumPeriodosAno;
        private Telerik.WinControls.UI.RadLabel lblDiasPermanencia;
        private Telerik.WinControls.UI.RadTextBoxControl txtDiasPermanencia;
        private Telerik.WinControls.UI.RadCheckBox chkMonedaAlternativa;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonEjecutarInforme;
        private Telerik.WinControls.UI.RadGridView radGridViewInformeLista;
    }
}