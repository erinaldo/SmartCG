namespace ModFinanzas
{
    partial class frmActivarOpciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActivarOpciones));
            this.btnSalir = new System.Windows.Forms.Button();
            this.grBoxNoActiva = new System.Windows.Forms.GroupBox();
            this.treeViewNoActiva = new System.Windows.Forms.TreeView();
            this.lblResult = new System.Windows.Forms.Label();
            this.grBoxNoActiva.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalir
            // 
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Location = new System.Drawing.Point(477, 485);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // grBoxNoActiva
            // 
            this.grBoxNoActiva.Controls.Add(this.treeViewNoActiva);
            this.grBoxNoActiva.Location = new System.Drawing.Point(32, 24);
            this.grBoxNoActiva.Name = "grBoxNoActiva";
            this.grBoxNoActiva.Size = new System.Drawing.Size(520, 439);
            this.grBoxNoActiva.TabIndex = 0;
            this.grBoxNoActiva.TabStop = false;
            this.grBoxNoActiva.Text = "Opciones No Activas";
            // 
            // treeViewNoActiva
            // 
            this.treeViewNoActiva.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewNoActiva.Location = new System.Drawing.Point(23, 28);
            this.treeViewNoActiva.Name = "treeViewNoActiva";
            this.treeViewNoActiva.Size = new System.Drawing.Size(471, 385);
            this.treeViewNoActiva.TabIndex = 0;
            this.treeViewNoActiva.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewNoActiva_MouseClick);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(32, 21);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(165, 13);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "Todas las opciones están activas";
            this.lblResult.Visible = false;
            // 
            // frmActivarOpciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSalir;
            this.ClientSize = new System.Drawing.Size(579, 531);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.grBoxNoActiva);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmActivarOpciones";
            this.Text = "Activar Opciones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmActivarOpciones_FormClosing);
            this.Load += new System.EventHandler(this.frmActivar_Load);
            this.grBoxNoActiva.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grBoxNoActiva;
        private System.Windows.Forms.TreeView treeViewNoActiva;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblResult;
    }
}