namespace AppAdminModulos
{
    partial class frmConfigFichero
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigFichero));
            this.lblPath = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblFicheroGenerado = new System.Windows.Forms.Label();
            this.txtFicheroGenerado = new System.Windows.Forms.TextBox();
            this.lblClaveEncriptar = new System.Windows.Forms.Label();
            this.txtClaveEncriptar = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(27, 31);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(75, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Path Ficheros:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(169, 24);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(333, 20);
            this.txtPath.TabIndex = 1;
            // 
            // lblFicheroGenerado
            // 
            this.lblFicheroGenerado.AutoSize = true;
            this.lblFicheroGenerado.Location = new System.Drawing.Point(27, 74);
            this.lblFicheroGenerado.Name = "lblFicheroGenerado";
            this.lblFicheroGenerado.Size = new System.Drawing.Size(135, 13);
            this.lblFicheroGenerado.TabIndex = 2;
            this.lblFicheroGenerado.Text = "Nombre Fichero Generado:";
            // 
            // txtFicheroGenerado
            // 
            this.txtFicheroGenerado.Location = new System.Drawing.Point(169, 67);
            this.txtFicheroGenerado.Name = "txtFicheroGenerado";
            this.txtFicheroGenerado.Size = new System.Drawing.Size(333, 20);
            this.txtFicheroGenerado.TabIndex = 3;
            // 
            // lblClaveEncriptar
            // 
            this.lblClaveEncriptar.AutoSize = true;
            this.lblClaveEncriptar.Location = new System.Drawing.Point(27, 115);
            this.lblClaveEncriptar.Name = "lblClaveEncriptar";
            this.lblClaveEncriptar.Size = new System.Drawing.Size(120, 13);
            this.lblClaveEncriptar.TabIndex = 6;
            this.lblClaveEncriptar.Text = "Clave Encriptar Fichero:";
            // 
            // txtClaveEncriptar
            // 
            this.txtClaveEncriptar.Location = new System.Drawing.Point(169, 108);
            this.txtClaveEncriptar.Name = "txtClaveEncriptar";
            this.txtClaveEncriptar.Size = new System.Drawing.Size(333, 20);
            this.txtClaveEncriptar.TabIndex = 7;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(170, 181);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 8;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(303, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmConfigFichero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(536, 251);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtClaveEncriptar);
            this.Controls.Add(this.lblClaveEncriptar);
            this.Controls.Add(this.txtFicheroGenerado);
            this.Controls.Add(this.lblFicheroGenerado);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfigFichero";
            this.Text = "Configuración Generar Fichero";
            this.Load += new System.EventHandler(this.frmConfigFichero_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblFicheroGenerado;
        private System.Windows.Forms.TextBox txtFicheroGenerado;
        private System.Windows.Forms.Label lblClaveEncriptar;
        private System.Windows.Forms.TextBox txtClaveEncriptar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancel;
    }
}