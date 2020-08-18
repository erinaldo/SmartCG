namespace ModSII
{
    partial class frmSiiSituacionGral
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiSituacionGral));
            this.lblResult = new System.Windows.Forms.Label();
            this.tgGridSituacion = new ObjectModel.TGGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.lblLibro = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.gbListaCiasFiscales = new System.Windows.Forms.GroupBox();
            this.chkCiasFiscalesTodas = new System.Windows.Forms.CheckBox();
            this.btnQuitarCiaFiscal = new System.Windows.Forms.Button();
            this.btnAddCiaFiscal = new System.Windows.Forms.Button();
            this.lbCiasFiscales = new System.Windows.Forms.ListBox();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.btnTodos = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridSituacion)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.gbBuscador.SuspendLayout();
            this.gbListaCiasFiscales.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblResult.Location = new System.Drawing.Point(38, 210);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(273, 13);
            this.lblResult.TabIndex = 122;
            this.lblResult.Text = "No existen facturas para el criterio de selección indicado";
            this.lblResult.Visible = false;
            // 
            // tgGridSituacion
            // 
            this.tgGridSituacion.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridSituacion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridSituacion.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridSituacion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridSituacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridSituacion.ComboValores = null;
            this.tgGridSituacion.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridSituacion.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridSituacion.DSDatos = null;
            this.tgGridSituacion.Location = new System.Drawing.Point(33, 210);
            this.tgGridSituacion.Name = "tgGridSituacion";
            this.tgGridSituacion.NombreTabla = "";
            this.tgGridSituacion.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridSituacion.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridSituacion.RowNumber = false;
            this.tgGridSituacion.Size = new System.Drawing.Size(915, 406);
            this.tgGridSituacion.TabIndex = 121;
            this.tgGridSituacion.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonExportar,
            this.toolStripSeparator1,
            this.toolStripButtonGrabarPeticion,
            this.toolStripButtonCargarPeticion,
            this.toolStripSeparator2,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(978, 25);
            this.toolStrip1.TabIndex = 81;
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
            // toolStripButtonGrabarPeticion
            // 
            this.toolStripButtonGrabarPeticion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrabarPeticion.Image")));
            this.toolStripButtonGrabarPeticion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrabarPeticion.Name = "toolStripButtonGrabarPeticion";
            this.toolStripButtonGrabarPeticion.Size = new System.Drawing.Size(108, 22);
            this.toolStripButtonGrabarPeticion.Text = "&Grabar Petición";
            this.toolStripButtonGrabarPeticion.Click += new System.EventHandler(this.toolStripButtonGrabarPeticion_Click);
            // 
            // toolStripButtonCargarPeticion
            // 
            this.toolStripButtonCargarPeticion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCargarPeticion.Image")));
            this.toolStripButtonCargarPeticion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCargarPeticion.Name = "toolStripButtonCargarPeticion";
            this.toolStripButtonCargarPeticion.Size = new System.Drawing.Size(108, 22);
            this.toolStripButtonCargarPeticion.Text = "&Cargar Petición";
            this.toolStripButtonCargarPeticion.Click += new System.EventHandler(this.toolStripButtonCargarPeticion_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.txtEjercicio);
            this.gbBuscador.Controls.Add(this.cmbPeriodo);
            this.gbBuscador.Controls.Add(this.cmbLibro);
            this.gbBuscador.Controls.Add(this.lblLibro);
            this.gbBuscador.Controls.Add(this.lblPeriodo);
            this.gbBuscador.Controls.Add(this.lblEjercicio);
            this.gbBuscador.Controls.Add(this.gbListaCiasFiscales);
            this.gbBuscador.Controls.Add(this.btnTodos);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbBuscador.Location = new System.Drawing.Point(33, 41);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(915, 153);
            this.gbBuscador.TabIndex = 80;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtEjercicio.Location = new System.Drawing.Point(515, 34);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 35;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(634, 34);
            this.cmbPeriodo.MaxLength = 2;
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(53, 21);
            this.cmbPeriodo.TabIndex = 45;
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbLibro.FormattingEnabled = true;
            this.cmbLibro.Location = new System.Drawing.Point(515, 72);
            this.cmbLibro.Name = "cmbLibro";
            this.cmbLibro.Size = new System.Drawing.Size(172, 21);
            this.cmbLibro.TabIndex = 85;
            this.cmbLibro.SelectedIndexChanged += new System.EventHandler(this.cmbLibro_SelectedIndexChanged);
            // 
            // lblLibro
            // 
            this.lblLibro.AutoSize = true;
            this.lblLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblLibro.Location = new System.Drawing.Point(463, 77);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(30, 13);
            this.lblLibro.TabIndex = 80;
            this.lblLibro.Text = "Libro";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblPeriodo.Location = new System.Drawing.Point(585, 37);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 40;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(462, 36);
            this.lblEjercicio.Name = "lblEjercicio";
            this.lblEjercicio.Size = new System.Drawing.Size(47, 13);
            this.lblEjercicio.TabIndex = 30;
            this.lblEjercicio.Text = "Ejercicio";
            // 
            // gbListaCiasFiscales
            // 
            this.gbListaCiasFiscales.Controls.Add(this.chkCiasFiscalesTodas);
            this.gbListaCiasFiscales.Controls.Add(this.btnQuitarCiaFiscal);
            this.gbListaCiasFiscales.Controls.Add(this.btnAddCiaFiscal);
            this.gbListaCiasFiscales.Controls.Add(this.lbCiasFiscales);
            this.gbListaCiasFiscales.Controls.Add(this.tgTexBoxSelCiaFiscal);
            this.gbListaCiasFiscales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbListaCiasFiscales.Location = new System.Drawing.Point(20, 16);
            this.gbListaCiasFiscales.Name = "gbListaCiasFiscales";
            this.gbListaCiasFiscales.Size = new System.Drawing.Size(379, 122);
            this.gbListaCiasFiscales.TabIndex = 116;
            this.gbListaCiasFiscales.TabStop = false;
            this.gbListaCiasFiscales.Text = " Lista de Compañías Fiscales";
            // 
            // chkCiasFiscalesTodas
            // 
            this.chkCiasFiscalesTodas.AutoSize = true;
            this.chkCiasFiscalesTodas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkCiasFiscalesTodas.Location = new System.Drawing.Point(26, 96);
            this.chkCiasFiscalesTodas.Name = "chkCiasFiscalesTodas";
            this.chkCiasFiscalesTodas.Size = new System.Drawing.Size(170, 17);
            this.chkCiasFiscalesTodas.TabIndex = 20;
            this.chkCiasFiscalesTodas.Text = "Todas las Compañías Fiscales";
            this.chkCiasFiscalesTodas.UseVisualStyleBackColor = true;
            // 
            // btnQuitarCiaFiscal
            // 
            this.btnQuitarCiaFiscal.Enabled = false;
            this.btnQuitarCiaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnQuitarCiaFiscal.Location = new System.Drawing.Point(121, 56);
            this.btnQuitarCiaFiscal.Name = "btnQuitarCiaFiscal";
            this.btnQuitarCiaFiscal.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarCiaFiscal.TabIndex = 15;
            this.btnQuitarCiaFiscal.Text = "Quitar";
            this.btnQuitarCiaFiscal.UseVisualStyleBackColor = true;
            this.btnQuitarCiaFiscal.Click += new System.EventHandler(this.btnQuitarCiaFiscal_Click);
            // 
            // btnAddCiaFiscal
            // 
            this.btnAddCiaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnAddCiaFiscal.Location = new System.Drawing.Point(21, 56);
            this.btnAddCiaFiscal.Name = "btnAddCiaFiscal";
            this.btnAddCiaFiscal.Size = new System.Drawing.Size(75, 23);
            this.btnAddCiaFiscal.TabIndex = 10;
            this.btnAddCiaFiscal.Text = "Añadir";
            this.btnAddCiaFiscal.UseVisualStyleBackColor = true;
            this.btnAddCiaFiscal.Click += new System.EventHandler(this.btnAddCiaFiscal_Click);
            // 
            // lbCiasFiscales
            // 
            this.lbCiasFiscales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbCiasFiscales.FormattingEnabled = true;
            this.lbCiasFiscales.Location = new System.Drawing.Point(257, 18);
            this.lbCiasFiscales.Name = "lbCiasFiscales";
            this.lbCiasFiscales.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbCiasFiscales.Size = new System.Drawing.Size(102, 95);
            this.lbCiasFiscales.Sorted = true;
            this.lbCiasFiscales.TabIndex = 25;
            this.lbCiasFiscales.Enter += new System.EventHandler(this.lbCiasFiscales_Enter);
            // 
            // tgTexBoxSelCiaFiscal
            // 
            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 0;
            this.tgTexBoxSelCiaFiscal.CentrarFormSel = true;
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = null;
            this.tgTexBoxSelCiaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.tgTexBoxSelCiaFiscal.FrmPadre = null;
            this.tgTexBoxSelCiaFiscal.Location = new System.Drawing.Point(21, 18);
            this.tgTexBoxSelCiaFiscal.LocationFormSel = new System.Drawing.Point(0, 0);
            this.tgTexBoxSelCiaFiscal.Name = "tgTexBoxSelCiaFiscal";
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 0;
            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = null;
            this.tgTexBoxSelCiaFiscal.QueryFormSel = null;
            this.tgTexBoxSelCiaFiscal.SeparadorCampos = "-";
            this.tgTexBoxSelCiaFiscal.Size = new System.Drawing.Size(176, 30);
            this.tgTexBoxSelCiaFiscal.TabIndex = 5;
            this.tgTexBoxSelCiaFiscal.TituloFormSel = null;
            this.tgTexBoxSelCiaFiscal.TodoMayuscula = true;
            // 
            // btnTodos
            // 
            this.btnTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnTodos.Location = new System.Drawing.Point(607, 112);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(80, 21);
            this.btnTodos.TabIndex = 105;
            this.btnTodos.Text = "Todos";
            this.btnTodos.UseVisualStyleBackColor = true;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(465, 113);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 100;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(53, 622);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 123;
            // 
            // frmSiiSituacionGral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 656);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.tgGridSituacion);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbBuscador);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiSituacionGral";
            this.Text = "Situación General SII";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiSituacionGral_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiSituacionGral_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiSituacionGral_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridSituacion)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbListaCiasFiscales.ResumeLayout(false);
            this.gbListaCiasFiscales.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.GroupBox gbListaCiasFiscales;
        private System.Windows.Forms.CheckBox chkCiasFiscalesTodas;
        private System.Windows.Forms.Button btnQuitarCiaFiscal;
        private System.Windows.Forms.Button btnAddCiaFiscal;
        private System.Windows.Forms.ListBox lbCiasFiscales;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.Label lblResult;
        private ObjectModel.TGGrid tgGridSituacion;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel1;
    }
}