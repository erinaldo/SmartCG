namespace ModSII
{
    partial class frmSiiConsultaListaMovimientos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiConsultaListaMovimientos));
            this.gbCabecera = new System.Windows.Forms.GroupBox();
            this.txtFechaDoc = new System.Windows.Forms.TextBox();
            this.lblFechaDoc = new System.Windows.Forms.Label();
            this.txtNoFact = new System.Windows.Forms.TextBox();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.txtNIFEmisor = new System.Windows.Forms.TextBox();
            this.lblIdEmisorFactura = new System.Windows.Forms.Label();
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.tgGridMovimientos = new ObjectModel.TGGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.gbCabecera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridMovimientos)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCabecera
            // 
            this.gbCabecera.Controls.Add(this.txtFechaDoc);
            this.gbCabecera.Controls.Add(this.lblFechaDoc);
            this.gbCabecera.Controls.Add(this.txtNoFact);
            this.gbCabecera.Controls.Add(this.lblFechaFactura);
            this.gbCabecera.Controls.Add(this.txtNIFEmisor);
            this.gbCabecera.Controls.Add(this.lblIdEmisorFactura);
            this.gbCabecera.Location = new System.Drawing.Point(35, 28);
            this.gbCabecera.Name = "gbCabecera";
            this.gbCabecera.Size = new System.Drawing.Size(907, 78);
            this.gbCabecera.TabIndex = 89;
            this.gbCabecera.TabStop = false;
            // 
            // txtFechaDoc
            // 
            this.txtFechaDoc.Location = new System.Drawing.Point(759, 30);
            this.txtFechaDoc.Name = "txtFechaDoc";
            this.txtFechaDoc.ReadOnly = true;
            this.txtFechaDoc.Size = new System.Drawing.Size(91, 20);
            this.txtFechaDoc.TabIndex = 93;
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.AutoSize = true;
            this.lblFechaDoc.Location = new System.Drawing.Point(651, 35);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(95, 13);
            this.lblFechaDoc.TabIndex = 92;
            this.lblFechaDoc.Text = "Fecha Documento";
            // 
            // txtNoFact
            // 
            this.txtNoFact.Location = new System.Drawing.Point(417, 30);
            this.txtNoFact.Name = "txtNoFact";
            this.txtNoFact.ReadOnly = true;
            this.txtNoFact.Size = new System.Drawing.Size(161, 20);
            this.txtNoFact.TabIndex = 91;
            // 
            // lblFechaFactura
            // 
            this.lblFechaFactura.AutoSize = true;
            this.lblFechaFactura.Location = new System.Drawing.Point(342, 35);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(63, 13);
            this.lblFechaFactura.TabIndex = 90;
            this.lblFechaFactura.Text = "No. Factura";
            // 
            // txtNIFEmisor
            // 
            this.txtNIFEmisor.Location = new System.Drawing.Point(98, 30);
            this.txtNIFEmisor.Name = "txtNIFEmisor";
            this.txtNIFEmisor.ReadOnly = true;
            this.txtNIFEmisor.Size = new System.Drawing.Size(184, 20);
            this.txtNIFEmisor.TabIndex = 89;
            // 
            // lblIdEmisorFactura
            // 
            this.lblIdEmisorFactura.AutoSize = true;
            this.lblIdEmisorFactura.Location = new System.Drawing.Point(19, 35);
            this.lblIdEmisorFactura.Name = "lblIdEmisorFactura";
            this.lblIdEmisorFactura.Size = new System.Drawing.Size(58, 13);
            this.lblIdEmisorFactura.TabIndex = 88;
            this.lblIdEmisorFactura.Text = "NIF Emisor";
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Location = new System.Drawing.Point(32, 116);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(118, 13);
            this.lblNoInfo.TabIndex = 91;
            this.lblNoInfo.Text = "No existen movimientos";
            this.lblNoInfo.Visible = false;
            // 
            // tgGridMovimientos
            // 
            this.tgGridMovimientos.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridMovimientos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridMovimientos.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridMovimientos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridMovimientos.ComboValores = null;
            this.tgGridMovimientos.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridMovimientos.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridMovimientos.DSDatos = null;
            this.tgGridMovimientos.Location = new System.Drawing.Point(35, 116);
            this.tgGridMovimientos.MultiSelect = false;
            this.tgGridMovimientos.Name = "tgGridMovimientos";
            this.tgGridMovimientos.NombreTabla = "";
            this.tgGridMovimientos.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridMovimientos.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridMovimientos.RowNumber = false;
            this.tgGridMovimientos.Size = new System.Drawing.Size(907, 298);
            this.tgGridMovimientos.TabIndex = 90;
            this.tgGridMovimientos.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(971, 25);
            this.toolStrip1.TabIndex = 92;
            this.toolStrip1.Text = "toolStrip1";
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
            // frmSiiConsultaListaMovimientos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(971, 426);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.tgGridMovimientos);
            this.Controls.Add(this.gbCabecera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiConsultaListaMovimientos";
            this.Text = "Lista de Movimientos - LIBRO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmListaMovimientos_FormClosing);
            this.Load += new System.EventHandler(this.frmListaMovimientos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmListaMovimientos_KeyDown);
            this.gbCabecera.ResumeLayout(false);
            this.gbCabecera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridMovimientos)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCabecera;
        private System.Windows.Forms.TextBox txtFechaDoc;
        private System.Windows.Forms.Label lblFechaDoc;
        private System.Windows.Forms.TextBox txtNoFact;
        private System.Windows.Forms.Label lblFechaFactura;
        private System.Windows.Forms.TextBox txtNIFEmisor;
        private System.Windows.Forms.Label lblIdEmisorFactura;
        private System.Windows.Forms.Label lblNoInfo;
        private ObjectModel.TGGrid tgGridMovimientos;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
    }
}