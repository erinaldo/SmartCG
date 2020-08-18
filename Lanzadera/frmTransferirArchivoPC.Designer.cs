namespace FinanzasNet
{
    partial class frmTransferirArchivoPC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransferirArchivoPC));
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.lnkFile = new System.Windows.Forms.LinkLabel();
            this.lblResult = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelDestino = new System.Windows.Forms.Button();
            this.txtPathFile = new System.Windows.Forms.TextBox();
            this.lblPathFile = new System.Windows.Forms.Label();
            this.gbOrigen = new System.Windows.Forms.GroupBox();
            this.txtBiblioteca = new System.Windows.Forms.TextBox();
            this.lblBiblioteca = new System.Windows.Forms.Label();
            this.txtMiembro = new System.Windows.Forms.TextBox();
            this.lblMiembro = new System.Windows.Forms.Label();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTransferir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCargar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGrabar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialogTransferir = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogTransferirConfig = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.gbOrigen.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkFile
            // 
            this.lnkFile.AutoSize = true;
            this.lnkFile.Location = new System.Drawing.Point(37, 364);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Size = new System.Drawing.Size(143, 13);
            this.lnkFile.TabIndex = 16;
            this.lnkFile.TabStop = true;
            this.lnkFile.Text = "Para visualizarlo pinche aquí";
            this.lnkFile.Visible = false;
            this.lnkFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFile_LinkClicked);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(37, 341);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(181, 13);
            this.lblResult.TabIndex = 15;
            this.lblResult.Text = "El fichero se transfirió correctamente.";
            this.lblResult.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelDestino);
            this.groupBox1.Controls.Add(this.txtPathFile);
            this.groupBox1.Controls.Add(this.lblPathFile);
            this.groupBox1.Location = new System.Drawing.Point(40, 239);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 74);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Destino ";
            // 
            // btnSelDestino
            // 
            this.btnSelDestino.Image = ((System.Drawing.Image)(resources.GetObject("btnSelDestino.Image")));
            this.btnSelDestino.Location = new System.Drawing.Point(559, 26);
            this.btnSelDestino.Name = "btnSelDestino";
            this.btnSelDestino.Size = new System.Drawing.Size(39, 31);
            this.btnSelDestino.TabIndex = 8;
            this.btnSelDestino.UseVisualStyleBackColor = true;
            this.btnSelDestino.Click += new System.EventHandler(this.btnSelDestino_Click);
            // 
            // txtPathFile
            // 
            this.txtPathFile.Location = new System.Drawing.Point(124, 32);
            this.txtPathFile.MaxLength = 260;
            this.txtPathFile.Name = "txtPathFile";
            this.txtPathFile.Size = new System.Drawing.Size(430, 20);
            this.txtPathFile.TabIndex = 7;
            // 
            // lblPathFile
            // 
            this.lblPathFile.AutoSize = true;
            this.lblPathFile.Location = new System.Drawing.Point(32, 35);
            this.lblPathFile.Name = "lblPathFile";
            this.lblPathFile.Size = new System.Drawing.Size(77, 13);
            this.lblPathFile.TabIndex = 6;
            this.lblPathFile.Text = "Ruta y Archivo";
            // 
            // gbOrigen
            // 
            this.gbOrigen.Controls.Add(this.txtBiblioteca);
            this.gbOrigen.Controls.Add(this.lblBiblioteca);
            this.gbOrigen.Controls.Add(this.txtMiembro);
            this.gbOrigen.Controls.Add(this.lblMiembro);
            this.gbOrigen.Controls.Add(this.txtArchivo);
            this.gbOrigen.Controls.Add(this.lblArchivo);
            this.gbOrigen.Location = new System.Drawing.Point(40, 74);
            this.gbOrigen.Name = "gbOrigen";
            this.gbOrigen.Size = new System.Drawing.Size(628, 136);
            this.gbOrigen.TabIndex = 13;
            this.gbOrigen.TabStop = false;
            this.gbOrigen.Text = " Origen ";
            // 
            // txtBiblioteca
            // 
            this.txtBiblioteca.Location = new System.Drawing.Point(124, 96);
            this.txtBiblioteca.MaxLength = 10;
            this.txtBiblioteca.Name = "txtBiblioteca";
            this.txtBiblioteca.Size = new System.Drawing.Size(179, 20);
            this.txtBiblioteca.TabIndex = 5;
            this.txtBiblioteca.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBiblioteca_KeyPress);
            // 
            // lblBiblioteca
            // 
            this.lblBiblioteca.AutoSize = true;
            this.lblBiblioteca.Location = new System.Drawing.Point(32, 99);
            this.lblBiblioteca.Name = "lblBiblioteca";
            this.lblBiblioteca.Size = new System.Drawing.Size(53, 13);
            this.lblBiblioteca.TabIndex = 4;
            this.lblBiblioteca.Text = "Biblioteca";
            // 
            // txtMiembro
            // 
            this.txtMiembro.Location = new System.Drawing.Point(124, 64);
            this.txtMiembro.MaxLength = 10;
            this.txtMiembro.Name = "txtMiembro";
            this.txtMiembro.Size = new System.Drawing.Size(179, 20);
            this.txtMiembro.TabIndex = 3;
            this.txtMiembro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMiembro_KeyPress);
            // 
            // lblMiembro
            // 
            this.lblMiembro.AutoSize = true;
            this.lblMiembro.Location = new System.Drawing.Point(32, 67);
            this.lblMiembro.Name = "lblMiembro";
            this.lblMiembro.Size = new System.Drawing.Size(47, 13);
            this.lblMiembro.TabIndex = 2;
            this.lblMiembro.Text = "Miembro";
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(124, 32);
            this.txtArchivo.MaxLength = 10;
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.Size = new System.Drawing.Size(179, 20);
            this.txtArchivo.TabIndex = 1;
            this.txtArchivo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtArchivo_KeyPress);
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(32, 35);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(43, 13);
            this.lblArchivo.TabIndex = 0;
            this.lblArchivo.Text = "Archivo";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTransferir,
            this.toolStripSeparator2,
            this.toolStripButtonCargar,
            this.toolStripButtonGrabar,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(702, 29);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonTransferir
            // 
            this.toolStripButtonTransferir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTransferir.Image")));
            this.toolStripButtonTransferir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTransferir.Name = "toolStripButtonTransferir";
            this.toolStripButtonTransferir.Size = new System.Drawing.Size(77, 26);
            this.toolStripButtonTransferir.Text = "Transferir";
            this.toolStripButtonTransferir.Click += new System.EventHandler(this.toolStripButtonTransferir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonCargar
            // 
            this.toolStripButtonCargar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCargar.Image")));
            this.toolStripButtonCargar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCargar.Name = "toolStripButtonCargar";
            this.toolStripButtonCargar.Size = new System.Drawing.Size(62, 26);
            this.toolStripButtonCargar.Text = "Cargar";
            this.toolStripButtonCargar.Click += new System.EventHandler(this.toolStripButtonCargar_Click);
            // 
            // toolStripButtonGrabar
            // 
            this.toolStripButtonGrabar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrabar.Image")));
            this.toolStripButtonGrabar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrabar.Name = "toolStripButtonGrabar";
            this.toolStripButtonGrabar.Size = new System.Drawing.Size(62, 26);
            this.toolStripButtonGrabar.Text = "Grabar";
            this.toolStripButtonGrabar.Click += new System.EventHandler(this.toolStripButtonGrabar_Click);
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
            this.toolStripButtonSalir.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonSalir.Size = new System.Drawing.Size(51, 26);
            this.toolStripButtonSalir.Text = "Salir";
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // openFileDialogTransferirConfig
            // 
            this.openFileDialogTransferirConfig.FileName = "openFileDialog1";
            // 
            // frmTransferirArchivoPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 396);
            this.Controls.Add(this.lnkFile);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbOrigen);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTransferirArchivoPC";
            this.Text = "Transferir Archivos PC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTransferirArchivoPC_FormClosing);
            this.Load += new System.EventHandler(this.frmTransferirArchivoPC_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTransferirArchivoPC_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbOrigen.ResumeLayout(false);
            this.gbOrigen.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransferir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargar;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox gbOrigen;
        private System.Windows.Forms.TextBox txtMiembro;
        private System.Windows.Forms.Label lblMiembro;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.TextBox txtBiblioteca;
        private System.Windows.Forms.Label lblBiblioteca;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPathFile;
        private System.Windows.Forms.Label lblPathFile;
        private System.Windows.Forms.Button btnSelDestino;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.LinkLabel lnkFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogTransferir;
        private System.Windows.Forms.OpenFileDialog openFileDialogTransferirConfig;
    }
}