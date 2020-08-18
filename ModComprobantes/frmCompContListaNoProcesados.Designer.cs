namespace ModComprobantes
{
    partial class frmCompContListaNoProcesados
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompContListaNoProcesados));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEdicionLotes = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCompErrores = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEditar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAjustar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.txtPrefijo = new System.Windows.Forms.TextBox();
            this.lblBilioteca = new System.Windows.Forms.Label();
            this.txtBiblioteca = new System.Windows.Forms.TextBox();
            this.gbEdicionLotes = new System.Windows.Forms.GroupBox();
            this.gbFormatoAmpliado = new System.Windows.Forms.GroupBox();
            this.rbFormatoAmpNo = new System.Windows.Forms.RadioButton();
            this.rbFormatoAmpSi = new System.Windows.Forms.RadioButton();
            this.lblFormatoAmpliado = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tgGridCompErrores = new ObjectModel.TGGrid();
            this.tgGridEditarLotes = new ObjectModel.TGGrid();
            this.toolStrip1.SuspendLayout();
            this.gbEdicionLotes.SuspendLayout();
            this.gbFormatoAmpliado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridCompErrores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridEditarLotes)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEdicionLotes,
            this.toolStripButtonCompErrores,
            this.toolStripSeparator2,
            this.toolStripButtonEditar,
            this.toolStripButtonAjustar,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 29);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonEdicionLotes
            // 
            this.toolStripButtonEdicionLotes.Enabled = false;
            this.toolStripButtonEdicionLotes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdicionLotes.Image")));
            this.toolStripButtonEdicionLotes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdicionLotes.Name = "toolStripButtonEdicionLotes";
            this.toolStripButtonEdicionLotes.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonEdicionLotes.Size = new System.Drawing.Size(115, 26);
            this.toolStripButtonEdicionLotes.Text = "Edición de Lotes";
            this.toolStripButtonEdicionLotes.Click += new System.EventHandler(this.toolStripButtonEdicionLotes_Click);
            // 
            // toolStripButtonCompErrores
            // 
            this.toolStripButtonCompErrores.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCompErrores.Image")));
            this.toolStripButtonCompErrores.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCompErrores.Name = "toolStripButtonCompErrores";
            this.toolStripButtonCompErrores.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonCompErrores.Size = new System.Drawing.Size(147, 26);
            this.toolStripButtonCompErrores.Text = "Comprobantes Errores";
            this.toolStripButtonCompErrores.Click += new System.EventHandler(this.toolStripButtonCompErrores_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonEditar
            // 
            this.toolStripButtonEditar.Enabled = false;
            this.toolStripButtonEditar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEditar.Image")));
            this.toolStripButtonEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEditar.Name = "toolStripButtonEditar";
            this.toolStripButtonEditar.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonEditar.Size = new System.Drawing.Size(59, 26);
            this.toolStripButtonEditar.Text = "Editar";
            this.toolStripButtonEditar.Click += new System.EventHandler(this.toolStripButtonEditar_Click);
            // 
            // toolStripButtonAjustar
            // 
            this.toolStripButtonAjustar.Enabled = false;
            this.toolStripButtonAjustar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAjustar.Image")));
            this.toolStripButtonAjustar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAjustar.Name = "toolStripButtonAjustar";
            this.toolStripButtonAjustar.Size = new System.Drawing.Size(121, 26);
            this.toolStripButtonAjustar.Text = "Ajustar Columnas";
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
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Location = new System.Drawing.Point(31, 33);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(36, 13);
            this.lblPrefijo.TabIndex = 5;
            this.lblPrefijo.Text = "Prefijo";
            // 
            // txtPrefijo
            // 
            this.txtPrefijo.Location = new System.Drawing.Point(79, 29);
            this.txtPrefijo.Name = "txtPrefijo";
            this.txtPrefijo.Size = new System.Drawing.Size(35, 20);
            this.txtPrefijo.TabIndex = 10;
            // 
            // lblBilioteca
            // 
            this.lblBilioteca.AutoSize = true;
            this.lblBilioteca.Location = new System.Drawing.Point(146, 32);
            this.lblBilioteca.Name = "lblBilioteca";
            this.lblBilioteca.Size = new System.Drawing.Size(53, 13);
            this.lblBilioteca.TabIndex = 15;
            this.lblBilioteca.Text = "Biblioteca";
            // 
            // txtBiblioteca
            // 
            this.txtBiblioteca.Enabled = false;
            this.txtBiblioteca.Location = new System.Drawing.Point(216, 28);
            this.txtBiblioteca.Name = "txtBiblioteca";
            this.txtBiblioteca.Size = new System.Drawing.Size(94, 20);
            this.txtBiblioteca.TabIndex = 20;
            // 
            // gbEdicionLotes
            // 
            this.gbEdicionLotes.Controls.Add(this.gbFormatoAmpliado);
            this.gbEdicionLotes.Controls.Add(this.lblFormatoAmpliado);
            this.gbEdicionLotes.Controls.Add(this.btnAceptar);
            this.gbEdicionLotes.Controls.Add(this.txtBiblioteca);
            this.gbEdicionLotes.Controls.Add(this.lblPrefijo);
            this.gbEdicionLotes.Controls.Add(this.lblBilioteca);
            this.gbEdicionLotes.Controls.Add(this.txtPrefijo);
            this.gbEdicionLotes.Location = new System.Drawing.Point(40, 57);
            this.gbEdicionLotes.Name = "gbEdicionLotes";
            this.gbEdicionLotes.Size = new System.Drawing.Size(730, 70);
            this.gbEdicionLotes.TabIndex = 5;
            this.gbEdicionLotes.TabStop = false;
            this.gbEdicionLotes.Text = " Edición de Lotes ";
            // 
            // gbFormatoAmpliado
            // 
            this.gbFormatoAmpliado.Controls.Add(this.rbFormatoAmpNo);
            this.gbFormatoAmpliado.Controls.Add(this.rbFormatoAmpSi);
            this.gbFormatoAmpliado.Location = new System.Drawing.Point(453, 12);
            this.gbFormatoAmpliado.Name = "gbFormatoAmpliado";
            this.gbFormatoAmpliado.Size = new System.Drawing.Size(116, 43);
            this.gbFormatoAmpliado.TabIndex = 30;
            this.gbFormatoAmpliado.TabStop = false;
            // 
            // rbFormatoAmpNo
            // 
            this.rbFormatoAmpNo.AutoSize = true;
            this.rbFormatoAmpNo.Checked = true;
            this.rbFormatoAmpNo.Location = new System.Drawing.Point(67, 17);
            this.rbFormatoAmpNo.Name = "rbFormatoAmpNo";
            this.rbFormatoAmpNo.Size = new System.Drawing.Size(39, 17);
            this.rbFormatoAmpNo.TabIndex = 1;
            this.rbFormatoAmpNo.TabStop = true;
            this.rbFormatoAmpNo.Text = "No";
            this.rbFormatoAmpNo.UseVisualStyleBackColor = true;
            // 
            // rbFormatoAmpSi
            // 
            this.rbFormatoAmpSi.AutoSize = true;
            this.rbFormatoAmpSi.Location = new System.Drawing.Point(12, 17);
            this.rbFormatoAmpSi.Name = "rbFormatoAmpSi";
            this.rbFormatoAmpSi.Size = new System.Drawing.Size(36, 17);
            this.rbFormatoAmpSi.TabIndex = 0;
            this.rbFormatoAmpSi.Text = "Sí";
            this.rbFormatoAmpSi.UseVisualStyleBackColor = true;
            // 
            // lblFormatoAmpliado
            // 
            this.lblFormatoAmpliado.AutoSize = true;
            this.lblFormatoAmpliado.Location = new System.Drawing.Point(352, 31);
            this.lblFormatoAmpliado.Name = "lblFormatoAmpliado";
            this.lblFormatoAmpliado.Size = new System.Drawing.Size(90, 13);
            this.lblFormatoAmpliado.TabIndex = 25;
            this.lblFormatoAmpliado.Text = "Formato ampliado";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(632, 29);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 35;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(39, 147);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(91, 13);
            this.lblInfo.TabIndex = 15;
            this.lblInfo.Text = "Info de resultados";
            // 
            // tgGridCompErrores
            // 
            this.tgGridCompErrores.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridCompErrores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridCompErrores.BackgroundColor = System.Drawing.Color.White;
            this.tgGridCompErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridCompErrores.ComboValores = null;
            this.tgGridCompErrores.ContextMenuStripGrid = null;
            this.tgGridCompErrores.DSDatos = null;
            this.tgGridCompErrores.Location = new System.Drawing.Point(39, 144);
            this.tgGridCompErrores.Name = "tgGridCompErrores";
            this.tgGridCompErrores.NombreTabla = "";
            this.tgGridCompErrores.RowHeaderInitWidth = 41;
            this.tgGridCompErrores.RowNumber = false;
            this.tgGridCompErrores.Size = new System.Drawing.Size(731, 377);
            this.tgGridCompErrores.TabIndex = 12;
            this.tgGridCompErrores.Visible = false;
            // 
            // tgGridEditarLotes
            // 
            this.tgGridEditarLotes.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridEditarLotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridEditarLotes.BackgroundColor = System.Drawing.Color.White;
            this.tgGridEditarLotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridEditarLotes.ComboValores = null;
            this.tgGridEditarLotes.ContextMenuStripGrid = null;
            this.tgGridEditarLotes.DSDatos = null;
            this.tgGridEditarLotes.Location = new System.Drawing.Point(39, 144);
            this.tgGridEditarLotes.Name = "tgGridEditarLotes";
            this.tgGridEditarLotes.NombreTabla = "";
            this.tgGridEditarLotes.RowHeaderInitWidth = 41;
            this.tgGridEditarLotes.RowNumber = false;
            this.tgGridEditarLotes.Size = new System.Drawing.Size(730, 377);
            this.tgGridEditarLotes.TabIndex = 10;
            this.tgGridEditarLotes.Visible = false;
            // 
            // frmCompContListaNoProcesados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 562);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tgGridCompErrores);
            this.Controls.Add(this.tgGridEditarLotes);
            this.Controls.Add(this.gbEdicionLotes);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompContListaNoProcesados";
            this.Text = "Lista de comprobantes contables no procesados";
            this.Load += new System.EventHandler(this.frmCompContListaNoProcesados_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbEdicionLotes.ResumeLayout(false);
            this.gbEdicionLotes.PerformLayout();
            this.gbFormatoAmpliado.ResumeLayout(false);
            this.gbFormatoAmpliado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridCompErrores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridEditarLotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdicionLotes;
        private System.Windows.Forms.ToolStripButton toolStripButtonCompErrores;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonEditar;
        private System.Windows.Forms.ToolStripButton toolStripButtonAjustar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.TextBox txtPrefijo;
        private System.Windows.Forms.Label lblBilioteca;
        private System.Windows.Forms.TextBox txtBiblioteca;
        private System.Windows.Forms.GroupBox gbEdicionLotes;
        private System.Windows.Forms.Button btnAceptar;
        private ObjectModel.TGGrid tgGridEditarLotes;
        private ObjectModel.TGGrid tgGridCompErrores;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox gbFormatoAmpliado;
        private System.Windows.Forms.RadioButton rbFormatoAmpNo;
        private System.Windows.Forms.RadioButton rbFormatoAmpSi;
        private System.Windows.Forms.Label lblFormatoAmpliado;
    }
}