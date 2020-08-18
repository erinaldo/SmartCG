namespace ModComprobantes
{
    partial class frmModeloCompContLista
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModeloCompContLista));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuGridClickDerecho = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuGridButtonNuevo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGridButtonEditar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGridButtonSuprimir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGridButtonAjustar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGridButtonImprimir = new System.Windows.Forms.ToolStripMenuItem();
            this.lblNoHayModeloComp = new System.Windows.Forms.Label();
            this.dgModelosComprobante = new System.Windows.Forms.DataGridView();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.cmbPlan = new System.Windows.Forms.ComboBox();
            this.lblPlan = new System.Windows.Forms.Label();
            this.txtReferenciaBuscador = new System.Windows.Forms.TextBox();
            this.lblReferencia = new System.Windows.Forms.Label();
            this.btnTodos = new System.Windows.Forms.Button();
            this.txtDescripcionBuscador = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNuevo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEditar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSuprimir = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAjustar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImprimir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.menuGridClickDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgModelosComprobante)).BeginInit();
            this.gbBuscador.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuGridClickDerecho
            // 
            this.menuGridClickDerecho.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGridButtonNuevo,
            this.menuGridButtonEditar,
            this.menuGridButtonSuprimir,
            this.menuGridButtonAjustar,
            this.menuGridButtonImprimir});
            this.menuGridClickDerecho.Name = "menuGridClickDerecho";
            this.menuGridClickDerecho.Size = new System.Drawing.Size(169, 114);
            // 
            // menuGridButtonNuevo
            // 
            this.menuGridButtonNuevo.Image = ((System.Drawing.Image)(resources.GetObject("menuGridButtonNuevo.Image")));
            this.menuGridButtonNuevo.Name = "menuGridButtonNuevo";
            this.menuGridButtonNuevo.Size = new System.Drawing.Size(168, 22);
            this.menuGridButtonNuevo.Text = "Nuevo";
            this.menuGridButtonNuevo.Click += new System.EventHandler(this.ToolStripButtonNuevo_Click);
            // 
            // menuGridButtonEditar
            // 
            this.menuGridButtonEditar.Image = ((System.Drawing.Image)(resources.GetObject("menuGridButtonEditar.Image")));
            this.menuGridButtonEditar.Name = "menuGridButtonEditar";
            this.menuGridButtonEditar.Size = new System.Drawing.Size(168, 22);
            this.menuGridButtonEditar.Text = "Editar";
            this.menuGridButtonEditar.Click += new System.EventHandler(this.ToolStripButtonEditar_Click);
            // 
            // menuGridButtonSuprimir
            // 
            this.menuGridButtonSuprimir.Image = ((System.Drawing.Image)(resources.GetObject("menuGridButtonSuprimir.Image")));
            this.menuGridButtonSuprimir.Name = "menuGridButtonSuprimir";
            this.menuGridButtonSuprimir.Size = new System.Drawing.Size(168, 22);
            this.menuGridButtonSuprimir.Text = "Suprimir";
            this.menuGridButtonSuprimir.Click += new System.EventHandler(this.ToolStripSuprimir_Click);
            // 
            // menuGridButtonAjustar
            // 
            this.menuGridButtonAjustar.Image = ((System.Drawing.Image)(resources.GetObject("menuGridButtonAjustar.Image")));
            this.menuGridButtonAjustar.Name = "menuGridButtonAjustar";
            this.menuGridButtonAjustar.Size = new System.Drawing.Size(168, 22);
            this.menuGridButtonAjustar.Text = "Ajustar Columnas";
            this.menuGridButtonAjustar.Click += new System.EventHandler(this.ToolStripAjustar_Click);
            // 
            // menuGridButtonImprimir
            // 
            this.menuGridButtonImprimir.Image = ((System.Drawing.Image)(resources.GetObject("menuGridButtonImprimir.Image")));
            this.menuGridButtonImprimir.Name = "menuGridButtonImprimir";
            this.menuGridButtonImprimir.Size = new System.Drawing.Size(168, 22);
            this.menuGridButtonImprimir.Text = "Imprimir";
            // 
            // lblNoHayModeloComp
            // 
            this.lblNoHayModeloComp.AutoSize = true;
            this.lblNoHayModeloComp.Location = new System.Drawing.Point(28, 130);
            this.lblNoHayModeloComp.Name = "lblNoHayModeloComp";
            this.lblNoHayModeloComp.Size = new System.Drawing.Size(163, 13);
            this.lblNoHayModeloComp.TabIndex = 31;
            this.lblNoHayModeloComp.Text = "NoExistenModelosComprobantes";
            this.lblNoHayModeloComp.Visible = false;
            // 
            // dgModelosComprobante
            // 
            this.dgModelosComprobante.AllowUserToAddRows = false;
            this.dgModelosComprobante.AllowUserToOrderColumns = true;
            this.dgModelosComprobante.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgModelosComprobante.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgModelosComprobante.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgModelosComprobante.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgModelosComprobante.ContextMenuStrip = this.menuGridClickDerecho;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgModelosComprobante.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgModelosComprobante.Location = new System.Drawing.Point(31, 130);
            this.dgModelosComprobante.Name = "dgModelosComprobante";
            this.dgModelosComprobante.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgModelosComprobante.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgModelosComprobante.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgModelosComprobante.Size = new System.Drawing.Size(760, 480);
            this.dgModelosComprobante.TabIndex = 6;
            // 
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.cmbPlan);
            this.gbBuscador.Controls.Add(this.lblPlan);
            this.gbBuscador.Controls.Add(this.txtReferenciaBuscador);
            this.gbBuscador.Controls.Add(this.lblReferencia);
            this.gbBuscador.Controls.Add(this.btnTodos);
            this.gbBuscador.Controls.Add(this.txtDescripcionBuscador);
            this.gbBuscador.Controls.Add(this.lblDescripcion);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Location = new System.Drawing.Point(31, 32);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(760, 82);
            this.gbBuscador.TabIndex = 29;
            this.gbBuscador.TabStop = false;
            // 
            // cmbPlan
            // 
            this.cmbPlan.FormattingEnabled = true;
            this.cmbPlan.Location = new System.Drawing.Point(347, 16);
            this.cmbPlan.MaxLength = 1;
            this.cmbPlan.Name = "cmbPlan";
            this.cmbPlan.Size = new System.Drawing.Size(255, 21);
            this.cmbPlan.TabIndex = 2;
            this.cmbPlan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CmbPlan_KeyPress);
            // 
            // lblPlan
            // 
            this.lblPlan.AutoSize = true;
            this.lblPlan.Location = new System.Drawing.Point(313, 22);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(28, 13);
            this.lblPlan.TabIndex = 33;
            this.lblPlan.Text = "Plan";
            // 
            // txtReferenciaBuscador
            // 
            this.txtReferenciaBuscador.Location = new System.Drawing.Point(75, 50);
            this.txtReferenciaBuscador.Name = "txtReferenciaBuscador";
            this.txtReferenciaBuscador.Size = new System.Drawing.Size(189, 20);
            this.txtReferenciaBuscador.TabIndex = 3;
            // 
            // lblReferencia
            // 
            this.lblReferencia.AutoSize = true;
            this.lblReferencia.Location = new System.Drawing.Point(6, 57);
            this.lblReferencia.Name = "lblReferencia";
            this.lblReferencia.Size = new System.Drawing.Size(59, 13);
            this.lblReferencia.TabIndex = 31;
            this.lblReferencia.Text = "Referencia";
            // 
            // btnTodos
            // 
            this.btnTodos.Location = new System.Drawing.Point(669, 49);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(80, 21);
            this.btnTodos.TabIndex = 5;
            this.btnTodos.Text = "Todos";
            this.btnTodos.UseVisualStyleBackColor = true;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // txtDescripcionBuscador
            // 
            this.txtDescripcionBuscador.Location = new System.Drawing.Point(75, 19);
            this.txtDescripcionBuscador.Name = "txtDescripcionBuscador";
            this.txtDescripcionBuscador.Size = new System.Drawing.Size(189, 20);
            this.txtDescripcionBuscador.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(6, 23);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 28;
            this.lblDescripcion.Text = "Descripción";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(669, 15);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNuevo,
            this.toolStripButtonEditar,
            this.toolStripButtonSuprimir,
            this.toolStripButtonAjustar,
            this.toolStripButtonImprimir,
            this.toolStripSeparator1,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(814, 29);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNuevo
            // 
            this.toolStripButtonNuevo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNuevo.Image")));
            this.toolStripButtonNuevo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNuevo.Name = "toolStripButtonNuevo";
            this.toolStripButtonNuevo.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonNuevo.Size = new System.Drawing.Size(64, 26);
            this.toolStripButtonNuevo.Text = "Nuevo";
            this.toolStripButtonNuevo.Click += new System.EventHandler(this.ToolStripButtonNuevo_Click);
            // 
            // toolStripButtonEditar
            // 
            this.toolStripButtonEditar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEditar.Image")));
            this.toolStripButtonEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEditar.Name = "toolStripButtonEditar";
            this.toolStripButtonEditar.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonEditar.Size = new System.Drawing.Size(59, 26);
            this.toolStripButtonEditar.Text = "Editar";
            this.toolStripButtonEditar.Click += new System.EventHandler(this.ToolStripButtonEditar_Click);
            // 
            // toolStripButtonSuprimir
            // 
            this.toolStripButtonSuprimir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSuprimir.Image")));
            this.toolStripButtonSuprimir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSuprimir.Name = "toolStripButtonSuprimir";
            this.toolStripButtonSuprimir.Size = new System.Drawing.Size(72, 26);
            this.toolStripButtonSuprimir.Text = "Suprimir";
            this.toolStripButtonSuprimir.Click += new System.EventHandler(this.ToolStripSuprimir_Click);
            // 
            // toolStripButtonAjustar
            // 
            this.toolStripButtonAjustar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAjustar.Image")));
            this.toolStripButtonAjustar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAjustar.Name = "toolStripButtonAjustar";
            this.toolStripButtonAjustar.Size = new System.Drawing.Size(121, 26);
            this.toolStripButtonAjustar.Text = "Ajustar Columnas";
            this.toolStripButtonAjustar.Click += new System.EventHandler(this.ToolStripAjustar_Click);
            // 
            // toolStripButtonImprimir
            // 
            this.toolStripButtonImprimir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImprimir.Image")));
            this.toolStripButtonImprimir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImprimir.Name = "toolStripButtonImprimir";
            this.toolStripButtonImprimir.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.toolStripButtonImprimir.Size = new System.Drawing.Size(75, 26);
            this.toolStripButtonImprimir.Text = "Imprimir";
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
            this.toolStripButtonSalir.Click += new System.EventHandler(this.ToolStripButtonSalir_Click);
            // 
            // frmModeloCompContLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 632);
            this.Controls.Add(this.lblNoHayModeloComp);
            this.Controls.Add(this.dgModelosComprobante);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmModeloCompContLista";
            this.Text = "Lista de Modelos de Comprobantes Contables";
            this.Load += new System.EventHandler(this.FrmModeloCompContLista_Load);
            this.menuGridClickDerecho.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgModelosComprobante)).EndInit();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNuevo;
        private System.Windows.Forms.ToolStripButton toolStripButtonEditar;
        private System.Windows.Forms.ToolStripButton toolStripButtonSuprimir;
        private System.Windows.Forms.ToolStripButton toolStripButtonAjustar;
        private System.Windows.Forms.ToolStripButton toolStripButtonImprimir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.TextBox txtDescripcionBuscador;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgModelosComprobante;
        private System.Windows.Forms.Label lblNoHayModeloComp;
        private System.Windows.Forms.ContextMenuStrip menuGridClickDerecho;
        private System.Windows.Forms.ToolStripMenuItem menuGridButtonNuevo;
        private System.Windows.Forms.ToolStripMenuItem menuGridButtonEditar;
        private System.Windows.Forms.ToolStripMenuItem menuGridButtonSuprimir;
        private System.Windows.Forms.ToolStripMenuItem menuGridButtonAjustar;
        private System.Windows.Forms.ToolStripMenuItem menuGridButtonImprimir;
        private System.Windows.Forms.TextBox txtReferenciaBuscador;
        private System.Windows.Forms.Label lblReferencia;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.ComboBox cmbPlan;
    }
}