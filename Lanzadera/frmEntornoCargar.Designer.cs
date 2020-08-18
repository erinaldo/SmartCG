namespace SmartCG
{
    partial class frmEntornoCargar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEntornoCargar));
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.cmbEntorno = new System.Windows.Forms.ComboBox();
            this.lblEntorno = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            resources.ApplyResources(this.btnAceptar, "btnAceptar");
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancelar, "btnCancelar");
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // cmbEntorno
            // 
            this.cmbEntorno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEntorno.FormattingEnabled = true;
            resources.ApplyResources(this.cmbEntorno, "cmbEntorno");
            this.cmbEntorno.Name = "cmbEntorno";
            // 
            // lblEntorno
            // 
            resources.ApplyResources(this.lblEntorno, "lblEntorno");
            this.lblEntorno.Name = "lblEntorno";
            // 
            // frmEntornoCargar
            // 
            this.AcceptButton = this.btnAceptar;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.cmbEntorno);
            this.Controls.Add(this.lblEntorno);
            this.MaximizeBox = false;
            this.Name = "frmEntornoCargar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEntornoCargar_FormClosing);
            this.Load += new System.EventHandler(this.FrmEntornoCargar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEntorno;
        private System.Windows.Forms.ComboBox cmbEntorno;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}