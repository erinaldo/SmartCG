namespace ModSII
{
    partial class frmSiiAlerta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiAlerta));
            this.btnCerrarFacturas = new System.Windows.Forms.Button();
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.chkMarcarDesmTodo = new System.Windows.Forms.CheckBox();
            this.btnConfirmarAlerta = new System.Windows.Forms.Button();
            this.tgGridFacturas = new ObjectModel.TGGrid();
            this.tgGridResumen = new ObjectModel.TGGrid();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCerrarFacturas
            // 
            this.btnCerrarFacturas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnCerrarFacturas.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrarFacturas.Image")));
            this.btnCerrarFacturas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarFacturas.Location = new System.Drawing.Point(33, 356);
            this.btnCerrarFacturas.Name = "btnCerrarFacturas";
            this.btnCerrarFacturas.Size = new System.Drawing.Size(143, 21);
            this.btnCerrarFacturas.TabIndex = 103;
            this.btnCerrarFacturas.Text = "   Cerrar Lista Facturas";
            this.btnCerrarFacturas.UseVisualStyleBackColor = true;
            this.btnCerrarFacturas.Visible = false;
            this.btnCerrarFacturas.Click += new System.EventHandler(this.btnCerrarFacturas_Click);
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNoInfo.Location = new System.Drawing.Point(30, 82);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(146, 13);
            this.lblNoInfo.TabIndex = 92;
            this.lblNoInfo.Text = "No existen alertas pendientes";
            this.lblNoInfo.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(452, 82);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 91;
            this.lblInfo.Text = "Su petición se está procesando, espere por favor.";
            this.lblInfo.Visible = false;
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
            this.toolStrip1.Size = new System.Drawing.Size(1146, 25);
            this.toolStrip1.TabIndex = 82;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonExportar
            // 
            this.toolStripButtonExportar.Enabled = false;
            this.toolStripButtonExportar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExportar.Image")));
            this.toolStripButtonExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExportar.Name = "toolStripButtonExportar";
            this.toolStripButtonExportar.Size = new System.Drawing.Size(71, 22);
            this.toolStripButtonExportar.Text = "Exportar";
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
            this.toolStripButtonSalir.Visible = false;
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // chkMarcarDesmTodo
            // 
            this.chkMarcarDesmTodo.AutoSize = true;
            this.chkMarcarDesmTodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkMarcarDesmTodo.Location = new System.Drawing.Point(33, 48);
            this.chkMarcarDesmTodo.Name = "chkMarcarDesmTodo";
            this.chkMarcarDesmTodo.Size = new System.Drawing.Size(188, 17);
            this.chkMarcarDesmTodo.TabIndex = 104;
            this.chkMarcarDesmTodo.Text = "Seleccionar/No Seleccionar Todo";
            this.chkMarcarDesmTodo.UseVisualStyleBackColor = true;
            this.chkMarcarDesmTodo.CheckedChanged += new System.EventHandler(this.chkMarcarDesmTodo_CheckedChanged);
            // 
            // btnConfirmarAlerta
            // 
            this.btnConfirmarAlerta.Enabled = false;
            this.btnConfirmarAlerta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnConfirmarAlerta.Location = new System.Drawing.Point(238, 44);
            this.btnConfirmarAlerta.Name = "btnConfirmarAlerta";
            this.btnConfirmarAlerta.Size = new System.Drawing.Size(131, 23);
            this.btnConfirmarAlerta.TabIndex = 105;
            this.btnConfirmarAlerta.Text = "Confirmar Alerta";
            this.btnConfirmarAlerta.UseVisualStyleBackColor = true;
            this.btnConfirmarAlerta.Click += new System.EventHandler(this.btnConfirmarAlerta_Click);
            // 
            // tgGridFacturas
            // 
            this.tgGridFacturas.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridFacturas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridFacturas.BackgroundColor = System.Drawing.Color.White;
            this.tgGridFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridFacturas.ComboValores = null;
            this.tgGridFacturas.ContextMenuStripGrid = null;
            this.tgGridFacturas.DSDatos = null;
            this.tgGridFacturas.Location = new System.Drawing.Point(33, 379);
            this.tgGridFacturas.Name = "tgGridFacturas";
            this.tgGridFacturas.NombreTabla = "";
            this.tgGridFacturas.RowHeaderInitWidth = 41;
            this.tgGridFacturas.RowNumber = false;
            this.tgGridFacturas.Size = new System.Drawing.Size(1085, 300);
            this.tgGridFacturas.TabIndex = 85;
            this.tgGridFacturas.Visible = false;
            this.tgGridFacturas.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridFacturas_DataBindingComplete);
            // 
            // tgGridResumen
            // 
            this.tgGridResumen.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridResumen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridResumen.BackgroundColor = System.Drawing.Color.White;
            this.tgGridResumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridResumen.ComboValores = null;
            this.tgGridResumen.ContextMenuStripGrid = null;
            this.tgGridResumen.DSDatos = null;
            this.tgGridResumen.Location = new System.Drawing.Point(33, 71);
            this.tgGridResumen.MultiSelect = false;
            this.tgGridResumen.Name = "tgGridResumen";
            this.tgGridResumen.NombreTabla = "";
            this.tgGridResumen.RowHeaderInitWidth = 41;
            this.tgGridResumen.RowNumber = false;
            this.tgGridResumen.Size = new System.Drawing.Size(1085, 608);
            this.tgGridResumen.TabIndex = 84;
            this.tgGridResumen.Visible = false;
            this.tgGridResumen.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellClick);
            this.tgGridResumen.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellContentClick);
            this.tgGridResumen.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellContentDoubleClick);
            this.tgGridResumen.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellDoubleClick);
            this.tgGridResumen.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridResumen_DataBindingComplete);
            this.tgGridResumen.SelectionChanged += new System.EventHandler(this.tgGridResumen_SelectionChanged);
            // 
            // frmSiiAlerta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 691);
            this.Controls.Add(this.btnConfirmarAlerta);
            this.Controls.Add(this.chkMarcarDesmTodo);
            this.Controls.Add(this.btnCerrarFacturas);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tgGridFacturas);
            this.Controls.Add(this.tgGridResumen);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiAlerta";
            this.Text = "Alertas de Envios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiAlerta_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiAlerta_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiAlerta_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private ObjectModel.TGGrid tgGridResumen;
        private ObjectModel.TGGrid tgGridFacturas;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNoInfo;
        private System.Windows.Forms.Button btnCerrarFacturas;
        private System.Windows.Forms.CheckBox chkMarcarDesmTodo;
        private System.Windows.Forms.Button btnConfirmarAlerta;
    }
}