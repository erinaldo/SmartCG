namespace FinanzasNet
{
    partial class frmAccesoDirectoEliminar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccesoDirectoEliminar));
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.cmbAccesosDirectos = new System.Windows.Forms.ComboBox();
            this.lblNombreEliminarAD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(104, 81);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 8;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(242, 81);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // cmbAccesosDirectos
            // 
            this.cmbAccesosDirectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccesosDirectos.FormattingEnabled = true;
            this.cmbAccesosDirectos.Location = new System.Drawing.Point(85, 22);
            this.cmbAccesosDirectos.Name = "cmbAccesosDirectos";
            this.cmbAccesosDirectos.Size = new System.Drawing.Size(289, 21);
            this.cmbAccesosDirectos.TabIndex = 1;
            // 
            // lblNombreEliminarAD
            // 
            this.lblNombreEliminarAD.AutoSize = true;
            this.lblNombreEliminarAD.Location = new System.Drawing.Point(21, 30);
            this.lblNombreEliminarAD.Name = "lblNombreEliminarAD";
            this.lblNombreEliminarAD.Size = new System.Drawing.Size(44, 13);
            this.lblNombreEliminarAD.TabIndex = 0;
            this.lblNombreEliminarAD.Text = "Nombre";
            // 
            // frmAccesoDirectoEliminar
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(444, 135);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.cmbAccesosDirectos);
            this.Controls.Add(this.lblNombreEliminarAD);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAccesoDirectoEliminar";
            this.Text = "Eliminar Acceso Directo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAccesoDirectoEliminar_FormClosing);
            this.Load += new System.EventHandler(this.frmAccesoDirectoEliminar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombreEliminarAD;
        private System.Windows.Forms.ComboBox cmbAccesosDirectos;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAceptar;
    }
}