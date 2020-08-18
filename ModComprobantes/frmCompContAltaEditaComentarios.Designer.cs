namespace ModComprobantes
{
    partial class frmCompContAltaEditaComentarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompContAltaEditaComentarios));
            this.txtMaskAAPP = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.txtNoComprobante = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblNoComprobante = new Telerik.WinControls.UI.RadLabel();
            this.lblTipo = new Telerik.WinControls.UI.RadLabel();
            this.lblAAPP = new Telerik.WinControls.UI.RadLabel();
            this.lblCompania = new Telerik.WinControls.UI.RadLabel();
            this.txtCompania = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtTipo = new Telerik.WinControls.UI.RadTextBoxControl();
            this.gbComentarios = new Telerik.WinControls.UI.RadGroupBox();
            this.txtDesc9 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc8 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc7 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc6 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc5 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc4 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc3 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc2 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.txtDesc1 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.radButtonGuardar = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompania)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompania)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbComentarios)).BeginInit();
            this.gbComentarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonGuardar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMaskAAPP
            // 
            this.txtMaskAAPP.Location = new System.Drawing.Point(505, 82);
            this.txtMaskAAPP.Mask = "00-00";
            this.txtMaskAAPP.Name = "txtMaskAAPP";
            this.txtMaskAAPP.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaskAAPP.ReadOnly = true;
            this.txtMaskAAPP.Size = new System.Drawing.Size(58, 28);
            this.txtMaskAAPP.TabIndex = 20;
            this.txtMaskAAPP.TabStop = false;
            // 
            // txtNoComprobante
            // 
            this.txtNoComprobante.IsReadOnly = true;
            this.txtNoComprobante.Location = new System.Drawing.Point(505, 141);
            this.txtNoComprobante.MaxLength = 6;
            this.txtNoComprobante.Name = "txtNoComprobante";
            this.txtNoComprobante.Padding = new System.Windows.Forms.Padding(5);
            this.txtNoComprobante.Size = new System.Drawing.Size(102, 30);
            this.txtNoComprobante.TabIndex = 40;
            // 
            // lblNoComprobante
            // 
            this.lblNoComprobante.Location = new System.Drawing.Point(505, 116);
            this.lblNoComprobante.Name = "lblNoComprobante";
            this.lblNoComprobante.Size = new System.Drawing.Size(102, 19);
            this.lblNoComprobante.TabIndex = 35;
            this.lblNoComprobante.Text = "No Comprobante";
            // 
            // lblTipo
            // 
            this.lblTipo.Location = new System.Drawing.Point(61, 116);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(30, 19);
            this.lblTipo.TabIndex = 25;
            this.lblTipo.Text = "Tipo";
            // 
            // lblAAPP
            // 
            this.lblAAPP.Location = new System.Drawing.Point(505, 57);
            this.lblAAPP.Name = "lblAAPP";
            this.lblAAPP.Size = new System.Drawing.Size(76, 19);
            this.lblAAPP.TabIndex = 15;
            this.lblAAPP.Text = "Año-Período";
            // 
            // lblCompania
            // 
            this.lblCompania.Location = new System.Drawing.Point(61, 57);
            this.lblCompania.Name = "lblCompania";
            this.lblCompania.Size = new System.Drawing.Size(62, 19);
            this.lblCompania.TabIndex = 5;
            this.lblCompania.Text = "Compañía";
            // 
            // txtCompania
            // 
            this.txtCompania.IsReadOnly = true;
            this.txtCompania.Location = new System.Drawing.Point(61, 82);
            this.txtCompania.Name = "txtCompania";
            this.txtCompania.Padding = new System.Windows.Forms.Padding(5);
            this.txtCompania.Size = new System.Drawing.Size(337, 30);
            this.txtCompania.TabIndex = 10;
            // 
            // txtTipo
            // 
            this.txtTipo.IsReadOnly = true;
            this.txtTipo.Location = new System.Drawing.Point(61, 141);
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.Padding = new System.Windows.Forms.Padding(5);
            this.txtTipo.Size = new System.Drawing.Size(337, 30);
            this.txtTipo.TabIndex = 30;
            // 
            // gbComentarios
            // 
            this.gbComentarios.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbComentarios.Controls.Add(this.txtDesc9);
            this.gbComentarios.Controls.Add(this.txtDesc8);
            this.gbComentarios.Controls.Add(this.txtDesc7);
            this.gbComentarios.Controls.Add(this.txtDesc6);
            this.gbComentarios.Controls.Add(this.txtDesc5);
            this.gbComentarios.Controls.Add(this.txtDesc4);
            this.gbComentarios.Controls.Add(this.txtDesc3);
            this.gbComentarios.Controls.Add(this.txtDesc2);
            this.gbComentarios.Controls.Add(this.txtDesc1);
            this.gbComentarios.HeaderText = " Comentarios ";
            this.gbComentarios.Location = new System.Drawing.Point(61, 194);
            this.gbComentarios.Name = "gbComentarios";
            this.gbComentarios.Size = new System.Drawing.Size(602, 389);
            this.gbComentarios.TabIndex = 45;
            this.gbComentarios.TabStop = false;
            this.gbComentarios.Text = " Comentarios ";
            // 
            // txtDesc9
            // 
            this.txtDesc9.Location = new System.Drawing.Point(34, 341);
            this.txtDesc9.MaxLength = 72;
            this.txtDesc9.Name = "txtDesc9";
            this.txtDesc9.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc9.Size = new System.Drawing.Size(533, 30);
            this.txtDesc9.TabIndex = 90;
            // 
            // txtDesc8
            // 
            this.txtDesc8.Location = new System.Drawing.Point(34, 303);
            this.txtDesc8.MaxLength = 72;
            this.txtDesc8.Name = "txtDesc8";
            this.txtDesc8.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc8.Size = new System.Drawing.Size(533, 30);
            this.txtDesc8.TabIndex = 85;
            // 
            // txtDesc7
            // 
            this.txtDesc7.Location = new System.Drawing.Point(34, 265);
            this.txtDesc7.MaxLength = 72;
            this.txtDesc7.Name = "txtDesc7";
            this.txtDesc7.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc7.Size = new System.Drawing.Size(533, 30);
            this.txtDesc7.TabIndex = 80;
            // 
            // txtDesc6
            // 
            this.txtDesc6.Location = new System.Drawing.Point(34, 227);
            this.txtDesc6.MaxLength = 72;
            this.txtDesc6.Name = "txtDesc6";
            this.txtDesc6.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc6.Size = new System.Drawing.Size(533, 30);
            this.txtDesc6.TabIndex = 75;
            // 
            // txtDesc5
            // 
            this.txtDesc5.Location = new System.Drawing.Point(34, 189);
            this.txtDesc5.MaxLength = 72;
            this.txtDesc5.Name = "txtDesc5";
            this.txtDesc5.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc5.Size = new System.Drawing.Size(533, 30);
            this.txtDesc5.TabIndex = 70;
            // 
            // txtDesc4
            // 
            this.txtDesc4.Location = new System.Drawing.Point(34, 151);
            this.txtDesc4.MaxLength = 72;
            this.txtDesc4.Name = "txtDesc4";
            this.txtDesc4.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc4.Size = new System.Drawing.Size(533, 30);
            this.txtDesc4.TabIndex = 65;
            // 
            // txtDesc3
            // 
            this.txtDesc3.Location = new System.Drawing.Point(34, 113);
            this.txtDesc3.MaxLength = 72;
            this.txtDesc3.Name = "txtDesc3";
            this.txtDesc3.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc3.Size = new System.Drawing.Size(533, 30);
            this.txtDesc3.TabIndex = 60;
            // 
            // txtDesc2
            // 
            this.txtDesc2.Location = new System.Drawing.Point(34, 75);
            this.txtDesc2.MaxLength = 72;
            this.txtDesc2.Name = "txtDesc2";
            this.txtDesc2.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc2.Size = new System.Drawing.Size(533, 30);
            this.txtDesc2.TabIndex = 55;
            // 
            // txtDesc1
            // 
            this.txtDesc1.Location = new System.Drawing.Point(34, 37);
            this.txtDesc1.MaxLength = 72;
            this.txtDesc1.Name = "txtDesc1";
            this.txtDesc1.Padding = new System.Windows.Forms.Padding(5);
            this.txtDesc1.Size = new System.Drawing.Size(533, 30);
            this.txtDesc1.TabIndex = 50;
            // 
            // radButtonSalir
            // 
            this.radButtonSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSalir.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSalir.Location = new System.Drawing.Point(375, 610);
            this.radButtonSalir.Name = "radButtonSalir";
            this.radButtonSalir.Size = new System.Drawing.Size(145, 44);
            this.radButtonSalir.TabIndex = 100;
            this.radButtonSalir.Text = "Salir";
            this.radButtonSalir.Click += new System.EventHandler(this.RadButtonSalir_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSalir.GetChildAt(0))).Text = "Salir";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSalir.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonGuardar
            // 
            this.radButtonGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonGuardar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonGuardar.Location = new System.Drawing.Point(193, 610);
            this.radButtonGuardar.Name = "radButtonGuardar";
            this.radButtonGuardar.Size = new System.Drawing.Size(145, 44);
            this.radButtonGuardar.TabIndex = 95;
            this.radButtonGuardar.Text = "Guardar";
            this.radButtonGuardar.Click += new System.EventHandler(this.RadButtonGuardar_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonGuardar.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonGuardar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmCompContAltaEditaComentarios
            // 
            this.AcceptButton = this.radButtonGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.CancelButton = this.radButtonSalir;
            this.ClientSize = new System.Drawing.Size(723, 674);
            this.Controls.Add(this.radButtonGuardar);
            this.Controls.Add(this.radButtonSalir);
            this.Controls.Add(this.gbComentarios);
            this.Controls.Add(this.txtTipo);
            this.Controls.Add(this.txtCompania);
            this.Controls.Add(this.txtMaskAAPP);
            this.Controls.Add(this.txtNoComprobante);
            this.Controls.Add(this.lblNoComprobante);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblAAPP);
            this.Controls.Add(this.lblCompania);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompContAltaEditaComentarios";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Editar Comentarios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCompContAltaEditaComentarios_FormClosing);
            this.Load += new System.EventHandler(this.FrmCompContAltaEditaComentarios_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCompContAltaEditaComentarios_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtMaskAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompania)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompania)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbComentarios)).EndInit();
            this.gbComentarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonGuardar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadMaskedEditBox txtMaskAAPP;
        private Telerik.WinControls.UI.RadTextBoxControl txtNoComprobante;
        private Telerik.WinControls.UI.RadLabel lblNoComprobante;
        private Telerik.WinControls.UI.RadLabel lblTipo;
        private Telerik.WinControls.UI.RadLabel lblAAPP;
        private Telerik.WinControls.UI.RadLabel lblCompania;
        private Telerik.WinControls.UI.RadTextBoxControl txtCompania;
        private Telerik.WinControls.UI.RadTextBoxControl txtTipo;
        private Telerik.WinControls.UI.RadGroupBox gbComentarios;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc1;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc5;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc4;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc3;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc2;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc7;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc6;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc9;
        private Telerik.WinControls.UI.RadTextBoxControl txtDesc8;
        private Telerik.WinControls.UI.RadButton radButtonSalir;
        private Telerik.WinControls.UI.RadButton radButtonGuardar;
    }
}