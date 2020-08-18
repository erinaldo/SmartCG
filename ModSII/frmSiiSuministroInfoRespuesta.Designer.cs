namespace ModSII
{
    partial class frmSiiSuministroInfoRespuesta
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiSuministroInfoRespuesta));
            this.tgGridRespuesta = new ObjectModel.TGGrid();
            this.gbCabecera = new System.Windows.Forms.GroupBox();
            this.lblOperacionValor = new System.Windows.Forms.Label();
            this.lblOperacion = new System.Windows.Forms.Label();
            this.lblLibroValor = new System.Windows.Forms.Label();
            this.lblLibro = new System.Windows.Forms.Label();
            this.lblPeriodoValor = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicioValor = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.lblCompaniaValor = new System.Windows.Forms.Label();
            this.lblCompania = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.gbResumenEnvio = new System.Windows.Forms.GroupBox();
            this.tgGridResumen = new ObjectModel.TGGrid();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridRespuesta)).BeginInit();
            this.gbCabecera.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbResumenEnvio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).BeginInit();
            this.SuspendLayout();
            // 
            // tgGridRespuesta
            // 
            this.tgGridRespuesta.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridRespuesta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridRespuesta.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridRespuesta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridRespuesta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridRespuesta.ComboValores = null;
            this.tgGridRespuesta.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridRespuesta.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridRespuesta.DSDatos = null;
            this.tgGridRespuesta.Location = new System.Drawing.Point(39, 179);
            this.tgGridRespuesta.Name = "tgGridRespuesta";
            this.tgGridRespuesta.NombreTabla = "";
            this.tgGridRespuesta.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridRespuesta.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridRespuesta.RowNumber = false;
            this.tgGridRespuesta.Size = new System.Drawing.Size(874, 404);
            this.tgGridRespuesta.TabIndex = 89;
            this.tgGridRespuesta.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.tgGridRespuesta_RowPrePaint);
            // 
            // gbCabecera
            // 
            this.gbCabecera.Controls.Add(this.lblOperacionValor);
            this.gbCabecera.Controls.Add(this.lblOperacion);
            this.gbCabecera.Controls.Add(this.lblLibroValor);
            this.gbCabecera.Controls.Add(this.lblLibro);
            this.gbCabecera.Controls.Add(this.lblPeriodoValor);
            this.gbCabecera.Controls.Add(this.lblPeriodo);
            this.gbCabecera.Controls.Add(this.lblEjercicioValor);
            this.gbCabecera.Controls.Add(this.lblEjercicio);
            this.gbCabecera.Controls.Add(this.lblCompaniaValor);
            this.gbCabecera.Controls.Add(this.lblCompania);
            this.gbCabecera.Location = new System.Drawing.Point(39, 45);
            this.gbCabecera.Name = "gbCabecera";
            this.gbCabecera.Size = new System.Drawing.Size(877, 74);
            this.gbCabecera.TabIndex = 88;
            this.gbCabecera.TabStop = false;
            this.gbCabecera.Visible = false;
            // 
            // lblOperacionValor
            // 
            this.lblOperacionValor.AutoSize = true;
            this.lblOperacionValor.Location = new System.Drawing.Point(321, 46);
            this.lblOperacionValor.Name = "lblOperacionValor";
            this.lblOperacionValor.Size = new System.Drawing.Size(80, 13);
            this.lblOperacionValor.TabIndex = 9;
            this.lblOperacionValor.Text = "valor operacion";
            // 
            // lblOperacion
            // 
            this.lblOperacion.AutoSize = true;
            this.lblOperacion.Location = new System.Drawing.Point(248, 46);
            this.lblOperacion.Name = "lblOperacion";
            this.lblOperacion.Size = new System.Drawing.Size(56, 13);
            this.lblOperacion.TabIndex = 8;
            this.lblOperacion.Text = "Operación";
            // 
            // lblLibroValor
            // 
            this.lblLibroValor.AutoSize = true;
            this.lblLibroValor.Location = new System.Drawing.Point(112, 47);
            this.lblLibroValor.Name = "lblLibroValor";
            this.lblLibroValor.Size = new System.Drawing.Size(52, 13);
            this.lblLibroValor.TabIndex = 7;
            this.lblLibroValor.Text = "valor libro";
            // 
            // lblLibro
            // 
            this.lblLibro.AutoSize = true;
            this.lblLibro.Location = new System.Drawing.Point(39, 48);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(30, 13);
            this.lblLibro.TabIndex = 6;
            this.lblLibro.Text = "Libro";
            // 
            // lblPeriodoValor
            // 
            this.lblPeriodoValor.AutoSize = true;
            this.lblPeriodoValor.Location = new System.Drawing.Point(527, 22);
            this.lblPeriodoValor.Name = "lblPeriodoValor";
            this.lblPeriodoValor.Size = new System.Drawing.Size(68, 13);
            this.lblPeriodoValor.TabIndex = 5;
            this.lblPeriodoValor.Text = "valor periodo";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(463, 22);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 4;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicioValor
            // 
            this.lblEjercicioValor.AutoSize = true;
            this.lblEjercicioValor.Location = new System.Drawing.Point(321, 22);
            this.lblEjercicioValor.Name = "lblEjercicioValor";
            this.lblEjercicioValor.Size = new System.Drawing.Size(72, 13);
            this.lblEjercicioValor.TabIndex = 3;
            this.lblEjercicioValor.Text = "valor ejercicio";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Location = new System.Drawing.Point(248, 23);
            this.lblEjercicio.Name = "lblEjercicio";
            this.lblEjercicio.Size = new System.Drawing.Size(47, 13);
            this.lblEjercicio.TabIndex = 2;
            this.lblEjercicio.Text = "Ejercicio";
            // 
            // lblCompaniaValor
            // 
            this.lblCompaniaValor.AutoSize = true;
            this.lblCompaniaValor.Location = new System.Drawing.Point(112, 21);
            this.lblCompaniaValor.Name = "lblCompaniaValor";
            this.lblCompaniaValor.Size = new System.Drawing.Size(79, 13);
            this.lblCompaniaValor.TabIndex = 1;
            this.lblCompaniaValor.Text = "valor compania";
            // 
            // lblCompania
            // 
            this.lblCompania.AutoSize = true;
            this.lblCompania.Location = new System.Drawing.Point(39, 22);
            this.lblCompania.Name = "lblCompania";
            this.lblCompania.Size = new System.Drawing.Size(56, 13);
            this.lblCompania.TabIndex = 0;
            this.lblCompania.Text = "Compañía";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonExportar,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(957, 25);
            this.toolStrip1.TabIndex = 87;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonExportar
            // 
            this.toolStripButtonExportar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExportar.Image")));
            this.toolStripButtonExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExportar.Name = "toolStripButtonExportar";
            this.toolStripButtonExportar.Size = new System.Drawing.Size(71, 22);
            this.toolStripButtonExportar.Text = "&Exportar";
            this.toolStripButtonExportar.Click += new System.EventHandler(this.toolStripButtonExportar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSalir.Image")));
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonSalir.Text = "&Salir";
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // gbResumenEnvio
            // 
            this.gbResumenEnvio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResumenEnvio.Controls.Add(this.tgGridResumen);
            this.gbResumenEnvio.Location = new System.Drawing.Point(28, 45);
            this.gbResumenEnvio.Name = "gbResumenEnvio";
            this.gbResumenEnvio.Size = new System.Drawing.Size(896, 132);
            this.gbResumenEnvio.TabIndex = 90;
            this.gbResumenEnvio.TabStop = false;
            this.gbResumenEnvio.Text = " Resumen del envio ";
            // 
            // tgGridResumen
            // 
            this.tgGridResumen.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridResumen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridResumen.BackgroundColor = System.Drawing.Color.White;
            this.tgGridResumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridResumen.ComboValores = null;
            this.tgGridResumen.ContextMenuStripGrid = null;
            this.tgGridResumen.DSDatos = null;
            this.tgGridResumen.Location = new System.Drawing.Point(11, 19);
            this.tgGridResumen.Name = "tgGridResumen";
            this.tgGridResumen.NombreTabla = "";
            this.tgGridResumen.RowHeaderInitWidth = 41;
            this.tgGridResumen.RowNumber = false;
            this.tgGridResumen.Size = new System.Drawing.Size(874, 109);
            this.tgGridResumen.TabIndex = 0;
            // 
            // frmSiiSuministroInfoRespuesta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(957, 607);
            this.Controls.Add(this.gbResumenEnvio);
            this.Controls.Add(this.tgGridRespuesta);
            this.Controls.Add(this.gbCabecera);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiSuministroInfoRespuesta";
            this.Text = "Respuesta Factura Emitida / Factura Recibida / Bienes de Inversion ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiSuministroInfoRespuesta_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiSuministroInfoRespuesta_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiSuministroInfoRespuesta_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridRespuesta)).EndInit();
            this.gbCabecera.ResumeLayout(false);
            this.gbCabecera.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbResumenEnvio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ObjectModel.TGGrid tgGridRespuesta;
        private System.Windows.Forms.GroupBox gbCabecera;
        private System.Windows.Forms.Label lblOperacionValor;
        private System.Windows.Forms.Label lblOperacion;
        private System.Windows.Forms.Label lblLibroValor;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Label lblPeriodoValor;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicioValor;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.Label lblCompaniaValor;
        private System.Windows.Forms.Label lblCompania;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox gbResumenEnvio;
        private ObjectModel.TGGrid tgGridResumen;
    }
}