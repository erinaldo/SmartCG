namespace ModSII
{
    partial class frmSiiConsultaPagosRecibidas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiConsultaPagosRecibidas));
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.tgGridPagos = new ObjectModel.TGGrid();
            this.gbCabecera = new System.Windows.Forms.GroupBox();
            this.txtNombreRazonSocial = new System.Windows.Forms.TextBox();
            this.lblNombreRazonSocial = new System.Windows.Forms.Label();
            this.txtFechaDoc = new System.Windows.Forms.TextBox();
            this.lblFechaDoc = new System.Windows.Forms.Label();
            this.txtNoFact = new System.Windows.Forms.TextBox();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.txtNIFEmisor = new System.Windows.Forms.TextBox();
            this.lblIdEmisorFactura = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridPagos)).BeginInit();
            this.gbCabecera.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Location = new System.Drawing.Point(25, 127);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(264, 13);
            this.lblNoInfo.TabIndex = 95;
            this.lblNoInfo.Text = "No existen pagos que cumplan el criterio seleccionado";
            this.lblNoInfo.Visible = false;
            // 
            // tgGridPagos
            // 
            this.tgGridPagos.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridPagos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridPagos.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridPagos.ComboValores = null;
            this.tgGridPagos.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridPagos.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridPagos.DSDatos = null;
            this.tgGridPagos.Location = new System.Drawing.Point(28, 127);
            this.tgGridPagos.MultiSelect = false;
            this.tgGridPagos.Name = "tgGridPagos";
            this.tgGridPagos.NombreTabla = "";
            this.tgGridPagos.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridPagos.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridPagos.RowNumber = false;
            this.tgGridPagos.Size = new System.Drawing.Size(906, 329);
            this.tgGridPagos.TabIndex = 94;
            this.tgGridPagos.Visible = false;
            this.tgGridPagos.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridPagos_DataBindingComplete);
            // 
            // gbCabecera
            // 
            this.gbCabecera.Controls.Add(this.txtNombreRazonSocial);
            this.gbCabecera.Controls.Add(this.lblNombreRazonSocial);
            this.gbCabecera.Controls.Add(this.txtFechaDoc);
            this.gbCabecera.Controls.Add(this.lblFechaDoc);
            this.gbCabecera.Controls.Add(this.txtNoFact);
            this.gbCabecera.Controls.Add(this.lblFechaFactura);
            this.gbCabecera.Controls.Add(this.txtNIFEmisor);
            this.gbCabecera.Controls.Add(this.lblIdEmisorFactura);
            this.gbCabecera.Location = new System.Drawing.Point(28, 41);
            this.gbCabecera.Name = "gbCabecera";
            this.gbCabecera.Size = new System.Drawing.Size(907, 72);
            this.gbCabecera.TabIndex = 92;
            this.gbCabecera.TabStop = false;
            // 
            // txtNombreRazonSocial
            // 
            this.txtNombreRazonSocial.Location = new System.Drawing.Point(143, 40);
            this.txtNombreRazonSocial.Name = "txtNombreRazonSocial";
            this.txtNombreRazonSocial.ReadOnly = true;
            this.txtNombreRazonSocial.Size = new System.Drawing.Size(517, 20);
            this.txtNombreRazonSocial.TabIndex = 95;
            // 
            // lblNombreRazonSocial
            // 
            this.lblNombreRazonSocial.AutoSize = true;
            this.lblNombreRazonSocial.Location = new System.Drawing.Point(19, 45);
            this.lblNombreRazonSocial.Name = "lblNombreRazonSocial";
            this.lblNombreRazonSocial.Size = new System.Drawing.Size(119, 13);
            this.lblNombreRazonSocial.TabIndex = 94;
            this.lblNombreRazonSocial.Text = "Nombre o Razón Social";
            // 
            // txtFechaDoc
            // 
            this.txtFechaDoc.Location = new System.Drawing.Point(793, 12);
            this.txtFechaDoc.Name = "txtFechaDoc";
            this.txtFechaDoc.ReadOnly = true;
            this.txtFechaDoc.Size = new System.Drawing.Size(91, 20);
            this.txtFechaDoc.TabIndex = 93;
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.AutoSize = true;
            this.lblFechaDoc.Location = new System.Drawing.Point(688, 19);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(95, 13);
            this.lblFechaDoc.TabIndex = 92;
            this.lblFechaDoc.Text = "Fecha Documento";
            // 
            // txtNoFact
            // 
            this.txtNoFact.Location = new System.Drawing.Point(526, 14);
            this.txtNoFact.Name = "txtNoFact";
            this.txtNoFact.ReadOnly = true;
            this.txtNoFact.Size = new System.Drawing.Size(134, 20);
            this.txtNoFact.TabIndex = 91;
            // 
            // lblFechaFactura
            // 
            this.lblFechaFactura.AutoSize = true;
            this.lblFechaFactura.Location = new System.Drawing.Point(451, 19);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(63, 13);
            this.lblFechaFactura.TabIndex = 90;
            this.lblFechaFactura.Text = "No. Factura";
            // 
            // txtNIFEmisor
            // 
            this.txtNIFEmisor.Location = new System.Drawing.Point(143, 14);
            this.txtNIFEmisor.Name = "txtNIFEmisor";
            this.txtNIFEmisor.ReadOnly = true;
            this.txtNIFEmisor.Size = new System.Drawing.Size(278, 20);
            this.txtNIFEmisor.TabIndex = 89;
            // 
            // lblIdEmisorFactura
            // 
            this.lblIdEmisorFactura.AutoSize = true;
            this.lblIdEmisorFactura.Location = new System.Drawing.Point(19, 19);
            this.lblIdEmisorFactura.Name = "lblIdEmisorFactura";
            this.lblIdEmisorFactura.Size = new System.Drawing.Size(58, 13);
            this.lblIdEmisorFactura.TabIndex = 88;
            this.lblIdEmisorFactura.Text = "NIF Emisor";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(960, 25);
            this.toolStrip1.TabIndex = 93;
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
            // frmSiiConsultaPagosRecibidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(960, 481);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.tgGridPagos);
            this.Controls.Add(this.gbCabecera);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiConsultaPagosRecibidas";
            this.Text = "Listado de Pagos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiConsultaPagosRecibidas_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiConsultaPagosRecibidas_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiConsultaPagosRecibidas_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridPagos)).EndInit();
            this.gbCabecera.ResumeLayout(false);
            this.gbCabecera.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFechaDoc;
        private System.Windows.Forms.Label lblFechaDoc;
        private System.Windows.Forms.TextBox txtNoFact;
        private System.Windows.Forms.Label lblFechaFactura;
        private System.Windows.Forms.TextBox txtNIFEmisor;
        private System.Windows.Forms.Label lblIdEmisorFactura;
        private System.Windows.Forms.GroupBox gbCabecera;
        private System.Windows.Forms.TextBox txtNombreRazonSocial;
        private System.Windows.Forms.Label lblNombreRazonSocial;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label lblNoInfo;
        private ObjectModel.TGGrid tgGridPagos;
    }
}