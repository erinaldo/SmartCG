namespace FinanzasNet
{
    partial class frmIdiomaLista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIdiomaLista));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGrabar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.lblNoExistenIdiomas = new System.Windows.Forms.Label();
            this.dataGridIdiomas = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIdiomas)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGrabar,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(690, 29);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonGrabar
            // 
            this.toolStripButtonGrabar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrabar.Image")));
            this.toolStripButtonGrabar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrabar.Name = "toolStripButtonGrabar";
            this.toolStripButtonGrabar.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonGrabar.Size = new System.Drawing.Size(64, 26);
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
            // lblNoExistenIdiomas
            // 
            this.lblNoExistenIdiomas.AutoSize = true;
            this.lblNoExistenIdiomas.Location = new System.Drawing.Point(51, 66);
            this.lblNoExistenIdiomas.Name = "lblNoExistenIdiomas";
            this.lblNoExistenIdiomas.Size = new System.Drawing.Size(140, 13);
            this.lblNoExistenIdiomas.TabIndex = 4;
            this.lblNoExistenIdiomas.Text = "No existen idiomas definidos";
            this.lblNoExistenIdiomas.Visible = false;
            // 
            // dataGridIdiomas
            // 
            this.dataGridIdiomas.AllowUserToAddRows = false;
            this.dataGridIdiomas.AllowUserToDeleteRows = false;
            this.dataGridIdiomas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridIdiomas.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridIdiomas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridIdiomas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridIdiomas.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridIdiomas.Location = new System.Drawing.Point(51, 65);
            this.dataGridIdiomas.MultiSelect = false;
            this.dataGridIdiomas.Name = "dataGridIdiomas";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridIdiomas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridIdiomas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridIdiomas.Size = new System.Drawing.Size(587, 249);
            this.dataGridIdiomas.TabIndex = 3;
            this.dataGridIdiomas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridIdiomas_CellContentClick);
            this.dataGridIdiomas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridIdiomas_CellValueChanged);
            this.dataGridIdiomas.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridIdiomas_DataBindingComplete);
            // 
            // frmIdiomaLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 380);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblNoExistenIdiomas);
            this.Controls.Add(this.dataGridIdiomas);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmIdiomaLista";
            this.Text = "Lista de Idiomas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmIdiomaLista_FormClosing);
            this.Load += new System.EventHandler(this.frmIdiomaLista_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmIdiomaLista_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIdiomas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNoExistenIdiomas;
        private System.Windows.Forms.DataGridView dataGridIdiomas;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
    }
}