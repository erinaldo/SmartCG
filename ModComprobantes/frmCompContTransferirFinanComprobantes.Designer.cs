namespace ModComprobantes
{
    partial class frmCompContTransferirFinanComprobantes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompContTransferirFinanComprobantes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTransferir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.lblResultado = new System.Windows.Forms.Label();
            this.gbTransferencia = new System.Windows.Forms.GroupBox();
            this.chkVerNocomp = new System.Windows.Forms.CheckBox();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.rbGenerarLoteAdiciona = new System.Windows.Forms.RadioButton();
            this.rbGenerarLote = new System.Windows.Forms.RadioButton();
            this.gbLote = new System.Windows.Forms.GroupBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtBibliotecaCola = new System.Windows.Forms.TextBox();
            this.lblBiliotecaCola = new System.Windows.Forms.Label();
            this.txtCola = new System.Windows.Forms.TextBox();
            this.lblCola = new System.Windows.Forms.Label();
            this.txtBibliotecaPrefijo = new System.Windows.Forms.TextBox();
            this.lblBibliotecaPrefijo = new System.Windows.Forms.Label();
            this.txtPrefijo = new System.Windows.Forms.TextBox();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.gbComprobante = new System.Windows.Forms.GroupBox();
            this.lblNoHayComp = new System.Windows.Forms.Label();
            this.dgComprobantes = new System.Windows.Forms.DataGridView();
            this.rbEstadoTodos = new System.Windows.Forms.RadioButton();
            this.rbEstadoNoTrans = new System.Windows.Forms.RadioButton();
            this.lblEstado = new System.Windows.Forms.Label();
            this.btnSelCompContable = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.gbTransferencia.SuspendLayout();
            this.gbLote.SuspendLayout();
            this.gbComprobante.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgComprobantes)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTransferir,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(865, 29);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonTransferir
            // 
            this.toolStripButtonTransferir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTransferir.Image")));
            this.toolStripButtonTransferir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTransferir.Name = "toolStripButtonTransferir";
            this.toolStripButtonTransferir.Size = new System.Drawing.Size(75, 26);
            this.toolStripButtonTransferir.Text = "Transferir";
            this.toolStripButtonTransferir.Click += new System.EventHandler(this.toolStripButtonTransferir_Click);
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
            this.toolStripButtonSalir.Visible = false;
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(40, 559);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(55, 13);
            this.lblResultado.TabIndex = 9;
            this.lblResultado.Text = "Resultado";
            this.lblResultado.Visible = false;
            // 
            // gbTransferencia
            // 
            this.gbTransferencia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTransferencia.Controls.Add(this.chkVerNocomp);
            this.gbTransferencia.Controls.Add(this.cmbEstado);
            this.gbTransferencia.Controls.Add(this.rbGenerarLoteAdiciona);
            this.gbTransferencia.Controls.Add(this.rbGenerarLote);
            this.gbTransferencia.Location = new System.Drawing.Point(477, 395);
            this.gbTransferencia.Name = "gbTransferencia";
            this.gbTransferencia.Size = new System.Drawing.Size(366, 145);
            this.gbTransferencia.TabIndex = 4;
            this.gbTransferencia.TabStop = false;
            this.gbTransferencia.Text = " Tipo de Transferencia";
            // 
            // chkVerNocomp
            // 
            this.chkVerNocomp.AutoSize = true;
            this.chkVerNocomp.Location = new System.Drawing.Point(24, 119);
            this.chkVerNocomp.Name = "chkVerNocomp";
            this.chkVerNocomp.Size = new System.Drawing.Size(181, 17);
            this.chkVerNocomp.TabIndex = 3;
            this.chkVerNocomp.Text = "Ver los números de comprobante";
            this.chkVerNocomp.UseVisualStyleBackColor = true;
            // 
            // cmbEstado
            // 
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Enabled = false;
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "No Aprobado/s",
            "Aprobado/s",
            "Contabilizado/s"});
            this.cmbEstado.Location = new System.Drawing.Point(44, 71);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(150, 21);
            this.cmbEstado.TabIndex = 2;
            // 
            // rbGenerarLoteAdiciona
            // 
            this.rbGenerarLoteAdiciona.AutoSize = true;
            this.rbGenerarLoteAdiciona.Location = new System.Drawing.Point(24, 50);
            this.rbGenerarLoteAdiciona.Name = "rbGenerarLoteAdiciona";
            this.rbGenerarLoteAdiciona.Size = new System.Drawing.Size(142, 17);
            this.rbGenerarLoteAdiciona.TabIndex = 1;
            this.rbGenerarLoteAdiciona.TabStop = true;
            this.rbGenerarLoteAdiciona.Text = "Generar Lote y Adicionar";
            this.rbGenerarLoteAdiciona.UseVisualStyleBackColor = true;
            this.rbGenerarLoteAdiciona.CheckedChanged += new System.EventHandler(this.rbGenerarLoteAdiciona_CheckedChanged);
            // 
            // rbGenerarLote
            // 
            this.rbGenerarLote.AutoSize = true;
            this.rbGenerarLote.Location = new System.Drawing.Point(24, 26);
            this.rbGenerarLote.Name = "rbGenerarLote";
            this.rbGenerarLote.Size = new System.Drawing.Size(111, 17);
            this.rbGenerarLote.TabIndex = 0;
            this.rbGenerarLote.TabStop = true;
            this.rbGenerarLote.Text = "Solo Generar Lote";
            this.rbGenerarLote.UseVisualStyleBackColor = true;
            this.rbGenerarLote.CheckedChanged += new System.EventHandler(this.rbGenerarLote_CheckedChanged);
            // 
            // gbLote
            // 
            this.gbLote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbLote.Controls.Add(this.txtDescripcion);
            this.gbLote.Controls.Add(this.lblDescripcion);
            this.gbLote.Controls.Add(this.txtBibliotecaCola);
            this.gbLote.Controls.Add(this.lblBiliotecaCola);
            this.gbLote.Controls.Add(this.txtCola);
            this.gbLote.Controls.Add(this.lblCola);
            this.gbLote.Controls.Add(this.txtBibliotecaPrefijo);
            this.gbLote.Controls.Add(this.lblBibliotecaPrefijo);
            this.gbLote.Controls.Add(this.txtPrefijo);
            this.gbLote.Controls.Add(this.lblPrefijo);
            this.gbLote.Location = new System.Drawing.Point(21, 395);
            this.gbLote.Name = "gbLote";
            this.gbLote.Size = new System.Drawing.Size(366, 145);
            this.gbLote.TabIndex = 1;
            this.gbLote.TabStop = false;
            this.gbLote.Text = " Lote Batch ";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(88, 20);
            this.txtDescripcion.MaxLength = 36;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(268, 20);
            this.txtDescripcion.TabIndex = 9;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(19, 24);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 8;
            this.lblDescripcion.Text = "Descripción";
            // 
            // txtBibliotecaCola
            // 
            this.txtBibliotecaCola.Enabled = false;
            this.txtBibliotecaCola.Location = new System.Drawing.Point(166, 116);
            this.txtBibliotecaCola.MaxLength = 10;
            this.txtBibliotecaCola.Name = "txtBibliotecaCola";
            this.txtBibliotecaCola.Size = new System.Drawing.Size(128, 20);
            this.txtBibliotecaCola.TabIndex = 7;
            this.txtBibliotecaCola.Visible = false;
            // 
            // lblBiliotecaCola
            // 
            this.lblBiliotecaCola.AutoSize = true;
            this.lblBiliotecaCola.Enabled = false;
            this.lblBiliotecaCola.Location = new System.Drawing.Point(163, 100);
            this.lblBiliotecaCola.Name = "lblBiliotecaCola";
            this.lblBiliotecaCola.Size = new System.Drawing.Size(53, 13);
            this.lblBiliotecaCola.TabIndex = 6;
            this.lblBiliotecaCola.Text = "Biblioteca";
            this.lblBiliotecaCola.Visible = false;
            // 
            // txtCola
            // 
            this.txtCola.Enabled = false;
            this.txtCola.Location = new System.Drawing.Point(22, 116);
            this.txtCola.MaxLength = 10;
            this.txtCola.Name = "txtCola";
            this.txtCola.Size = new System.Drawing.Size(128, 20);
            this.txtCola.TabIndex = 5;
            this.txtCola.Visible = false;
            // 
            // lblCola
            // 
            this.lblCola.AutoSize = true;
            this.lblCola.Enabled = false;
            this.lblCola.Location = new System.Drawing.Point(19, 100);
            this.lblCola.Name = "lblCola";
            this.lblCola.Size = new System.Drawing.Size(75, 13);
            this.lblCola.TabIndex = 4;
            this.lblCola.Text = "Cola de Salida";
            this.lblCola.Visible = false;
            // 
            // txtBibliotecaPrefijo
            // 
            this.txtBibliotecaPrefijo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBibliotecaPrefijo.Location = new System.Drawing.Point(71, 70);
            this.txtBibliotecaPrefijo.MaxLength = 10;
            this.txtBibliotecaPrefijo.Name = "txtBibliotecaPrefijo";
            this.txtBibliotecaPrefijo.Size = new System.Drawing.Size(128, 20);
            this.txtBibliotecaPrefijo.TabIndex = 3;
            // 
            // lblBibliotecaPrefijo
            // 
            this.lblBibliotecaPrefijo.AutoSize = true;
            this.lblBibliotecaPrefijo.Location = new System.Drawing.Point(68, 54);
            this.lblBibliotecaPrefijo.Name = "lblBibliotecaPrefijo";
            this.lblBibliotecaPrefijo.Size = new System.Drawing.Size(53, 13);
            this.lblBibliotecaPrefijo.TabIndex = 2;
            this.lblBibliotecaPrefijo.Text = "Biblioteca";
            // 
            // txtPrefijo
            // 
            this.txtPrefijo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPrefijo.Location = new System.Drawing.Point(22, 70);
            this.txtPrefijo.MaxLength = 2;
            this.txtPrefijo.Name = "txtPrefijo";
            this.txtPrefijo.Size = new System.Drawing.Size(33, 20);
            this.txtPrefijo.TabIndex = 1;
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Location = new System.Drawing.Point(19, 54);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(36, 13);
            this.lblPrefijo.TabIndex = 0;
            this.lblPrefijo.Text = "Prefijo";
            // 
            // gbComprobante
            // 
            this.gbComprobante.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbComprobante.Controls.Add(this.lblNoHayComp);
            this.gbComprobante.Controls.Add(this.dgComprobantes);
            this.gbComprobante.Controls.Add(this.rbEstadoTodos);
            this.gbComprobante.Controls.Add(this.rbEstadoNoTrans);
            this.gbComprobante.Controls.Add(this.lblEstado);
            this.gbComprobante.Controls.Add(this.btnSelCompContable);
            this.gbComprobante.Controls.Add(this.txtPath);
            this.gbComprobante.Controls.Add(this.lblPath);
            this.gbComprobante.Location = new System.Drawing.Point(21, 59);
            this.gbComprobante.Name = "gbComprobante";
            this.gbComprobante.Size = new System.Drawing.Size(822, 330);
            this.gbComprobante.TabIndex = 0;
            this.gbComprobante.TabStop = false;
            this.gbComprobante.Text = " Comprobantes ";
            // 
            // lblNoHayComp
            // 
            this.lblNoHayComp.AutoSize = true;
            this.lblNoHayComp.Location = new System.Drawing.Point(20, 83);
            this.lblNoHayComp.Name = "lblNoHayComp";
            this.lblNoHayComp.Size = new System.Drawing.Size(123, 13);
            this.lblNoHayComp.TabIndex = 10;
            this.lblNoHayComp.Text = "NoExistenComrpobantes";
            this.lblNoHayComp.Visible = false;
            // 
            // dgComprobantes
            // 
            this.dgComprobantes.AllowUserToAddRows = false;
            this.dgComprobantes.AllowUserToDeleteRows = false;
            this.dgComprobantes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgComprobantes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgComprobantes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgComprobantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgComprobantes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgComprobantes.Location = new System.Drawing.Point(23, 83);
            this.dgComprobantes.Name = "dgComprobantes";
            this.dgComprobantes.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgComprobantes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgComprobantes.Size = new System.Drawing.Size(781, 241);
            this.dgComprobantes.TabIndex = 8;
            // 
            // rbEstadoTodos
            // 
            this.rbEstadoTodos.AutoSize = true;
            this.rbEstadoTodos.Location = new System.Drawing.Point(257, 51);
            this.rbEstadoTodos.Name = "rbEstadoTodos";
            this.rbEstadoTodos.Size = new System.Drawing.Size(55, 17);
            this.rbEstadoTodos.TabIndex = 7;
            this.rbEstadoTodos.Text = "Todos";
            this.rbEstadoTodos.UseVisualStyleBackColor = true;
            // 
            // rbEstadoNoTrans
            // 
            this.rbEstadoNoTrans.AutoSize = true;
            this.rbEstadoNoTrans.Checked = true;
            this.rbEstadoNoTrans.Location = new System.Drawing.Point(80, 51);
            this.rbEstadoNoTrans.Name = "rbEstadoNoTrans";
            this.rbEstadoNoTrans.Size = new System.Drawing.Size(100, 17);
            this.rbEstadoNoTrans.TabIndex = 6;
            this.rbEstadoNoTrans.TabStop = true;
            this.rbEstadoNoTrans.Text = "No Transferidos";
            this.rbEstadoNoTrans.UseVisualStyleBackColor = true;
            this.rbEstadoNoTrans.CheckedChanged += new System.EventHandler(this.rbEstadoNoTrans_CheckedChanged);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(20, 52);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(40, 13);
            this.lblEstado.TabIndex = 5;
            this.lblEstado.Text = "Estado";
            // 
            // btnSelCompContable
            // 
            this.btnSelCompContable.Image = ((System.Drawing.Image)(resources.GetObject("btnSelCompContable.Image")));
            this.btnSelCompContable.Location = new System.Drawing.Point(526, 16);
            this.btnSelCompContable.Name = "btnSelCompContable";
            this.btnSelCompContable.Size = new System.Drawing.Size(39, 31);
            this.btnSelCompContable.TabIndex = 4;
            this.btnSelCompContable.UseVisualStyleBackColor = true;
            this.btnSelCompContable.Click += new System.EventHandler(this.btnSelCompContable_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(80, 21);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(425, 20);
            this.txtPath.TabIndex = 1;
            this.txtPath.Leave += new System.EventHandler(this.txtPath_Leave);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(19, 25);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(55, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Ubicación";
            // 
            // frmCompContTransferirFinanComprobantes
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 592);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.gbTransferencia);
            this.Controls.Add(this.gbLote);
            this.Controls.Add(this.gbComprobante);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompContTransferirFinanComprobantes";
            this.Text = "Transferir Comprobantes a Finanzas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCompContTransferirFinanComprobantes_FormClosing);
            this.Load += new System.EventHandler(this.frmCompContTransferirFinanComprobantes_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCompContTransferirFinanComprobantes_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbTransferencia.ResumeLayout(false);
            this.gbTransferencia.PerformLayout();
            this.gbLote.ResumeLayout(false);
            this.gbLote.PerformLayout();
            this.gbComprobante.ResumeLayout(false);
            this.gbComprobante.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgComprobantes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbComprobante;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnSelCompContable;
        private System.Windows.Forms.RadioButton rbEstadoTodos;
        private System.Windows.Forms.RadioButton rbEstadoNoTrans;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DataGridView dgComprobantes;
        private System.Windows.Forms.GroupBox gbLote;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.TextBox txtBibliotecaCola;
        private System.Windows.Forms.Label lblBiliotecaCola;
        private System.Windows.Forms.TextBox txtCola;
        private System.Windows.Forms.Label lblCola;
        private System.Windows.Forms.TextBox txtBibliotecaPrefijo;
        private System.Windows.Forms.Label lblBibliotecaPrefijo;
        private System.Windows.Forms.TextBox txtPrefijo;
        private System.Windows.Forms.GroupBox gbTransferencia;
        private System.Windows.Forms.CheckBox chkVerNocomp;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.RadioButton rbGenerarLoteAdiciona;
        private System.Windows.Forms.RadioButton rbGenerarLote;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblNoHayComp;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransferir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
    }
}