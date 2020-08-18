namespace FinanzasNet
{
    partial class frmModulo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModulo));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonActivar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDesactivar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEntrarClave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonActivar,
            this.toolStripButtonDesactivar,
            this.toolStripSeparator1,
            this.toolStripButtonEntrarClave,
            this.toolStripSeparator2,
            this.toolStripButtonSalir});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(761, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButtonActivar
            // 
            this.toolStripButtonActivar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonActivar.Image")));
            this.toolStripButtonActivar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonActivar.Name = "toolStripButtonActivar";
            this.toolStripButtonActivar.Size = new System.Drawing.Size(64, 22);
            this.toolStripButtonActivar.Text = "Activar";
            // 
            // toolStripButtonDesactivar
            // 
            this.toolStripButtonDesactivar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDesactivar.Image")));
            this.toolStripButtonDesactivar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDesactivar.Name = "toolStripButtonDesactivar";
            this.toolStripButtonDesactivar.Size = new System.Drawing.Size(81, 22);
            this.toolStripButtonDesactivar.Text = "Desactivar";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonEntrarClave
            // 
            this.toolStripButtonEntrarClave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEntrarClave.Image")));
            this.toolStripButtonEntrarClave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEntrarClave.Name = "toolStripButtonEntrarClave";
            this.toolStripButtonEntrarClave.Size = new System.Drawing.Size(90, 22);
            this.toolStripButtonEntrarClave.Text = "Entrar Clave";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSalir.Image")));
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonSalir.Text = "Salir";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(81, 83);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(523, 330);
            this.treeView1.TabIndex = 2;
            // 
            // frmModulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 514);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip2);
            this.Name = "frmModulo";
            this.Text = "Gestionar Módulos";
            this.Load += new System.EventHandler(this.frmModulo_Load);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButtonActivar;
        private System.Windows.Forms.ToolStripButton toolStripButtonDesactivar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonEntrarClave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.TreeView treeView1;


    }
}