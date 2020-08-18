namespace ModConsultaInforme
{
    partial class frmWebBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebBrowser));
            this.saveFileGrabarInforme = new System.Windows.Forms.SaveFileDialog();
            this.radSaveFileDialogGuardar = new Telerik.WinControls.UI.RadSaveFileDialog();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnGrabar = new Telerik.WinControls.UI.RadButton();
            this.webB = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGrabar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancel.Location = new System.Drawing.Point(532, 597);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 44);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Salir";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.BtnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.BtnCancel_MouseLeave);
            // 
            // btnGrabar
            // 
            this.btnGrabar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.btnGrabar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGrabar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnGrabar.Location = new System.Drawing.Point(324, 598);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(138, 44);
            this.btnGrabar.TabIndex = 2;
            this.btnGrabar.Text = "Guardar";
            this.btnGrabar.Click += new System.EventHandler(this.BtnGrabar_Click);
            this.btnGrabar.MouseEnter += new System.EventHandler(this.BtnGrabar_MouseEnter);
            this.btnGrabar.MouseLeave += new System.EventHandler(this.BtnGrabar_MouseLeave);
            // 
            // webB
            // 
            this.webB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webB.Location = new System.Drawing.Point(0, 14);
            this.webB.MinimumSize = new System.Drawing.Size(20, 20);
            this.webB.Name = "webB";
            this.webB.Size = new System.Drawing.Size(930, 552);
            this.webB.TabIndex = 1;
            // 
            // frmWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(930, 677);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.webB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWebBrowser";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visor de Informe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWebBrowser_FormClosing);
            this.Load += new System.EventHandler(this.FrmWebBrowser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGrabar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.WebBrowser webB;
        private Telerik.WinControls.UI.RadButton btnGrabar;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private System.Windows.Forms.SaveFileDialog saveFileGrabarInforme;
        private Telerik.WinControls.UI.RadSaveFileDialog radSaveFileDialogGuardar;
    }
}