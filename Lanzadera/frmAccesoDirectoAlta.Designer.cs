namespace FinanzasNet
{
    partial class frmAccesoDirectoAlta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccesoDirectoAlta));
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSelPrograma = new System.Windows.Forms.Button();
            this.txtADPrograma = new System.Windows.Forms.TextBox();
            this.lblPrograma = new System.Windows.Forms.Label();
            this.txtADNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(174, 128);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(312, 128);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSelPrograma
            // 
            this.btnSelPrograma.Image = ((System.Drawing.Image)(resources.GetObject("btnSelPrograma.Image")));
            this.btnSelPrograma.Location = new System.Drawing.Point(516, 58);
            this.btnSelPrograma.Name = "btnSelPrograma";
            this.btnSelPrograma.Size = new System.Drawing.Size(39, 31);
            this.btnSelPrograma.TabIndex = 6;
            this.btnSelPrograma.UseVisualStyleBackColor = true;
            this.btnSelPrograma.Click += new System.EventHandler(this.btnSelPrograma_Click);
            // 
            // txtADPrograma
            // 
            this.txtADPrograma.Location = new System.Drawing.Point(109, 64);
            this.txtADPrograma.Name = "txtADPrograma";
            this.txtADPrograma.Size = new System.Drawing.Size(388, 20);
            this.txtADPrograma.TabIndex = 3;
            // 
            // lblPrograma
            // 
            this.lblPrograma.AutoSize = true;
            this.lblPrograma.Location = new System.Drawing.Point(23, 72);
            this.lblPrograma.Name = "lblPrograma";
            this.lblPrograma.Size = new System.Drawing.Size(52, 13);
            this.lblPrograma.TabIndex = 2;
            this.lblPrograma.Text = "Programa";
            // 
            // txtADNombre
            // 
            this.txtADNombre.Location = new System.Drawing.Point(109, 30);
            this.txtADNombre.Name = "txtADNombre";
            this.txtADNombre.Size = new System.Drawing.Size(388, 20);
            this.txtADNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(20, 30);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmAccesoDirectoAlta
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(584, 183);
            this.Controls.Add(this.btnSelPrograma);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtADPrograma);
            this.Controls.Add(this.lblPrograma);
            this.Controls.Add(this.txtADNombre);
            this.Controls.Add(this.lblNombre);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAccesoDirectoAlta";
            this.Text = "Crear Acceso Directo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAccesoDirectoAlta_FormClosing);
            this.Load += new System.EventHandler(this.frmAccesoDirectoAlta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtADNombre;
        private System.Windows.Forms.Label lblPrograma;
        private System.Windows.Forms.TextBox txtADPrograma;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSelPrograma;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}