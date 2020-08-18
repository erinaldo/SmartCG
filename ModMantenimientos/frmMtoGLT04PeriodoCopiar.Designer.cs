namespace ModMantenimientos
{
    partial class frmMtoGLT04PeriodoCopiar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoGLT04PeriodoCopiar));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCopiar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.txtAnoOrigen = new System.Windows.Forms.TextBox();
            this.lblAnoOrigen = new System.Windows.Forms.Label();
            this.txtAnoDestino = new System.Windows.Forms.TextBox();
            this.lblAnoDestino = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCopiar,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(490, 29);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonCopiar
            // 
            this.toolStripButtonCopiar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopiar.Image")));
            this.toolStripButtonCopiar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopiar.Name = "toolStripButtonCopiar";
            this.toolStripButtonCopiar.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonCopiar.Size = new System.Drawing.Size(64, 26);
            this.toolStripButtonCopiar.Text = "Copiar";
            this.toolStripButtonCopiar.Click += new System.EventHandler(this.toolStripButtonCopiar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSalir.Image")));
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(49, 26);
            this.toolStripButtonSalir.Text = "Salir";
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // txtAnoOrigen
            // 
            this.txtAnoOrigen.Location = new System.Drawing.Point(198, 53);
            this.txtAnoOrigen.MaxLength = 2;
            this.txtAnoOrigen.Name = "txtAnoOrigen";
            this.txtAnoOrigen.Size = new System.Drawing.Size(36, 20);
            this.txtAnoOrigen.TabIndex = 12;
            this.txtAnoOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAnoOrigen
            // 
            this.lblAnoOrigen.AutoSize = true;
            this.lblAnoOrigen.Location = new System.Drawing.Point(40, 60);
            this.lblAnoOrigen.Name = "lblAnoOrigen";
            this.lblAnoOrigen.Size = new System.Drawing.Size(58, 13);
            this.lblAnoOrigen.TabIndex = 11;
            this.lblAnoOrigen.Text = "Año origen";
            // 
            // txtAnoDestino
            // 
            this.txtAnoDestino.Location = new System.Drawing.Point(198, 95);
            this.txtAnoDestino.MaxLength = 2;
            this.txtAnoDestino.Name = "txtAnoDestino";
            this.txtAnoDestino.Size = new System.Drawing.Size(36, 20);
            this.txtAnoDestino.TabIndex = 14;
            this.txtAnoDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAnoDestino
            // 
            this.lblAnoDestino.AutoSize = true;
            this.lblAnoDestino.Location = new System.Drawing.Point(40, 102);
            this.lblAnoDestino.Name = "lblAnoDestino";
            this.lblAnoDestino.Size = new System.Drawing.Size(63, 13);
            this.lblAnoDestino.TabIndex = 13;
            this.lblAnoDestino.Text = "Año destino";
            // 
            // frmMtoGLT04PeriodoCopiar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 230);
            this.Controls.Add(this.txtAnoDestino);
            this.Controls.Add(this.lblAnoDestino);
            this.Controls.Add(this.txtAnoOrigen);
            this.Controls.Add(this.lblAnoOrigen);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoGLT04PeriodoCopiar";
            this.Text = "Mantenimiento de Calendarios Contables - Periodo Copiar";
            this.Load += new System.EventHandler(this.frmMtoGLT04PeriodoCopiar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMtoGLT04PeriodoCopiar_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopiar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.TextBox txtAnoOrigen;
        private System.Windows.Forms.Label lblAnoOrigen;
        private System.Windows.Forms.TextBox txtAnoDestino;
        private System.Windows.Forms.Label lblAnoDestino;
    }
}