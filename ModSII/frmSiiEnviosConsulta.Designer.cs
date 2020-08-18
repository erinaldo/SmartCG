namespace ModSII
{
    partial class frmSiiEnviosConsulta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiEnviosConsulta));
            this.btnCerrarFacturas = new System.Windows.Forms.Button();
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tgGridFacturas = new ObjectModel.TGGrid();
            this.tgGridResumen = new ObjectModel.TGGrid();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.cmbOperacion = new System.Windows.Forms.ComboBox();
            this.lblOperacion = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.txtMaskFechaPresentacionHasta = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaPresentacionHasta = new System.Windows.Forms.Label();
            this.txtMaskFechaPresentacionDesde = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaPresentacionDesde = new System.Windows.Forms.Label();
            this.gbEstadoFacturas = new System.Windows.Forms.GroupBox();
            this.rbEstadoIncorrecto = new System.Windows.Forms.RadioButton();
            this.rbEstadoAceptadoErrores = new System.Windows.Forms.RadioButton();
            this.rbEstadoCorrecto = new System.Windows.Forms.RadioButton();
            this.rbEstadoTodos = new System.Windows.Forms.RadioButton();
            this.lblEstadoFactura = new System.Windows.Forms.Label();
            this.lblCompania = new System.Windows.Forms.Label();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.lblLibro = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).BeginInit();
            this.gbBuscador.SuspendLayout();
            this.gbEstadoFacturas.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            this.lblNoInfo.Location = new System.Drawing.Point(30, 221);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(273, 13);
            this.lblNoInfo.TabIndex = 92;
            this.lblNoInfo.Text = "No existen facturas que cumplan el criterio seleccionado";
            this.lblNoInfo.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(452, 221);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 91;
            this.lblInfo.Text = "Su petición se está procesando, espere por favor.";
            this.lblInfo.Visible = false;
            // 
            // tgGridFacturas
            // 
            this.tgGridFacturas.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridFacturas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
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
            this.tgGridFacturas.Size = new System.Drawing.Size(1085, 283);
            this.tgGridFacturas.TabIndex = 85;
            this.tgGridFacturas.Visible = false;
            this.tgGridFacturas.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridFacturas_DataBindingComplete);
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
            this.tgGridResumen.Location = new System.Drawing.Point(33, 210);
            this.tgGridResumen.Name = "tgGridResumen";
            this.tgGridResumen.NombreTabla = "";
            this.tgGridResumen.RowHeaderInitWidth = 41;
            this.tgGridResumen.RowNumber = false;
            this.tgGridResumen.Size = new System.Drawing.Size(1085, 452);
            this.tgGridResumen.TabIndex = 84;
            this.tgGridResumen.Visible = false;
            this.tgGridResumen.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellClick);
            this.tgGridResumen.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridResumen_CellDoubleClick);
            this.tgGridResumen.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridResumen_DataBindingComplete);
            this.tgGridResumen.SelectionChanged += new System.EventHandler(this.tgGridResumen_SelectionChanged);
            // 
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.cmbOperacion);
            this.gbBuscador.Controls.Add(this.lblOperacion);
            this.gbBuscador.Controls.Add(this.btnLimpiar);
            this.gbBuscador.Controls.Add(this.txtMaskFechaPresentacionHasta);
            this.gbBuscador.Controls.Add(this.lblFechaPresentacionHasta);
            this.gbBuscador.Controls.Add(this.txtMaskFechaPresentacionDesde);
            this.gbBuscador.Controls.Add(this.lblFechaPresentacionDesde);
            this.gbBuscador.Controls.Add(this.gbEstadoFacturas);
            this.gbBuscador.Controls.Add(this.lblEstadoFactura);
            this.gbBuscador.Controls.Add(this.lblCompania);
            this.gbBuscador.Controls.Add(this.tgTexBoxSelCiaFiscal);
            this.gbBuscador.Controls.Add(this.cmbLibro);
            this.gbBuscador.Controls.Add(this.lblLibro);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbBuscador.Location = new System.Drawing.Point(33, 41);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(804, 162);
            this.gbBuscador.TabIndex = 83;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // cmbOperacion
            // 
            this.cmbOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbOperacion.FormattingEnabled = true;
            this.cmbOperacion.Location = new System.Drawing.Point(610, 19);
            this.cmbOperacion.Name = "cmbOperacion";
            this.cmbOperacion.Size = new System.Drawing.Size(175, 21);
            this.cmbOperacion.TabIndex = 102;
            // 
            // lblOperacion
            // 
            this.lblOperacion.AutoSize = true;
            this.lblOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblOperacion.Location = new System.Drawing.Point(505, 24);
            this.lblOperacion.Name = "lblOperacion";
            this.lblOperacion.Size = new System.Drawing.Size(56, 13);
            this.lblOperacion.TabIndex = 101;
            this.lblOperacion.Text = "Operación";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLimpiar.Location = new System.Drawing.Point(428, 129);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(80, 21);
            this.btnLimpiar.TabIndex = 100;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // txtMaskFechaPresentacionHasta
            // 
            this.txtMaskFechaPresentacionHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtMaskFechaPresentacionHasta.Location = new System.Drawing.Point(610, 67);
            this.txtMaskFechaPresentacionHasta.Mask = "00-00-0000";
            this.txtMaskFechaPresentacionHasta.Name = "txtMaskFechaPresentacionHasta";
            this.txtMaskFechaPresentacionHasta.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaPresentacionHasta.TabIndex = 70;
            // 
            // lblFechaPresentacionHasta
            // 
            this.lblFechaPresentacionHasta.AutoSize = true;
            this.lblFechaPresentacionHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaPresentacionHasta.Location = new System.Drawing.Point(445, 72);
            this.lblFechaPresentacionHasta.Name = "lblFechaPresentacionHasta";
            this.lblFechaPresentacionHasta.Size = new System.Drawing.Size(133, 13);
            this.lblFechaPresentacionHasta.TabIndex = 74;
            this.lblFechaPresentacionHasta.Text = "Fecha Presentación Hasta";
            // 
            // txtMaskFechaPresentacionDesde
            // 
            this.txtMaskFechaPresentacionDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtMaskFechaPresentacionDesde.Location = new System.Drawing.Point(163, 67);
            this.txtMaskFechaPresentacionDesde.Mask = "00-00-0000";
            this.txtMaskFechaPresentacionDesde.Name = "txtMaskFechaPresentacionDesde";
            this.txtMaskFechaPresentacionDesde.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaPresentacionDesde.TabIndex = 65;
            // 
            // lblFechaPresentacionDesde
            // 
            this.lblFechaPresentacionDesde.AutoSize = true;
            this.lblFechaPresentacionDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaPresentacionDesde.Location = new System.Drawing.Point(24, 70);
            this.lblFechaPresentacionDesde.Name = "lblFechaPresentacionDesde";
            this.lblFechaPresentacionDesde.Size = new System.Drawing.Size(136, 13);
            this.lblFechaPresentacionDesde.TabIndex = 72;
            this.lblFechaPresentacionDesde.Text = "Fecha Presentación Desde";
            // 
            // gbEstadoFacturas
            // 
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoIncorrecto);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoAceptadoErrores);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoCorrecto);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoTodos);
            this.gbEstadoFacturas.Location = new System.Drawing.Point(163, 91);
            this.gbEstadoFacturas.Name = "gbEstadoFacturas";
            this.gbEstadoFacturas.Size = new System.Drawing.Size(415, 31);
            this.gbEstadoFacturas.TabIndex = 80;
            this.gbEstadoFacturas.TabStop = false;
            // 
            // rbEstadoIncorrecto
            // 
            this.rbEstadoIncorrecto.AutoSize = true;
            this.rbEstadoIncorrecto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoIncorrecto.Location = new System.Drawing.Point(325, 10);
            this.rbEstadoIncorrecto.Name = "rbEstadoIncorrecto";
            this.rbEstadoIncorrecto.Size = new System.Drawing.Size(73, 17);
            this.rbEstadoIncorrecto.TabIndex = 4;
            this.rbEstadoIncorrecto.Text = "Incorrecto";
            this.rbEstadoIncorrecto.UseVisualStyleBackColor = true;
            // 
            // rbEstadoAceptadoErrores
            // 
            this.rbEstadoAceptadoErrores.AutoSize = true;
            this.rbEstadoAceptadoErrores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoAceptadoErrores.Location = new System.Drawing.Point(176, 10);
            this.rbEstadoAceptadoErrores.Name = "rbEstadoAceptadoErrores";
            this.rbEstadoAceptadoErrores.Size = new System.Drawing.Size(127, 17);
            this.rbEstadoAceptadoErrores.TabIndex = 3;
            this.rbEstadoAceptadoErrores.Text = "Aceptado con errores";
            this.rbEstadoAceptadoErrores.UseVisualStyleBackColor = true;
            // 
            // rbEstadoCorrecto
            // 
            this.rbEstadoCorrecto.AutoSize = true;
            this.rbEstadoCorrecto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoCorrecto.Location = new System.Drawing.Point(89, 10);
            this.rbEstadoCorrecto.Name = "rbEstadoCorrecto";
            this.rbEstadoCorrecto.Size = new System.Drawing.Size(65, 17);
            this.rbEstadoCorrecto.TabIndex = 2;
            this.rbEstadoCorrecto.Text = "Correcto";
            this.rbEstadoCorrecto.UseVisualStyleBackColor = true;
            // 
            // rbEstadoTodos
            // 
            this.rbEstadoTodos.AutoSize = true;
            this.rbEstadoTodos.Checked = true;
            this.rbEstadoTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoTodos.Location = new System.Drawing.Point(17, 10);
            this.rbEstadoTodos.Name = "rbEstadoTodos";
            this.rbEstadoTodos.Size = new System.Drawing.Size(55, 17);
            this.rbEstadoTodos.TabIndex = 0;
            this.rbEstadoTodos.TabStop = true;
            this.rbEstadoTodos.Text = "Todos";
            this.rbEstadoTodos.UseVisualStyleBackColor = true;
            // 
            // lblEstadoFactura
            // 
            this.lblEstadoFactura.AutoSize = true;
            this.lblEstadoFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEstadoFactura.Location = new System.Drawing.Point(24, 102);
            this.lblEstadoFactura.Name = "lblEstadoFactura";
            this.lblEstadoFactura.Size = new System.Drawing.Size(69, 13);
            this.lblEstadoFactura.TabIndex = 67;
            this.lblEstadoFactura.Text = "Estado envio";
            // 
            // lblCompania
            // 
            this.lblCompania.AutoSize = true;
            this.lblCompania.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCompania.Location = new System.Drawing.Point(24, 49);
            this.lblCompania.Name = "lblCompania";
            this.lblCompania.Size = new System.Drawing.Size(86, 13);
            this.lblCompania.TabIndex = 10;
            this.lblCompania.Text = "Compañía Fiscal";
            // 
            // tgTexBoxSelCiaFiscal
            // 
            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 0;
            this.tgTexBoxSelCiaFiscal.CentrarFormSel = true;
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = null;
            this.tgTexBoxSelCiaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.tgTexBoxSelCiaFiscal.FrmPadre = null;
            this.tgTexBoxSelCiaFiscal.Location = new System.Drawing.Point(158, 37);
            this.tgTexBoxSelCiaFiscal.LocationFormSel = new System.Drawing.Point(0, 0);
            this.tgTexBoxSelCiaFiscal.Name = "tgTexBoxSelCiaFiscal";
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 0;
            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = null;
            this.tgTexBoxSelCiaFiscal.QueryFormSel = null;
            this.tgTexBoxSelCiaFiscal.SeparadorCampos = "-";
            this.tgTexBoxSelCiaFiscal.Size = new System.Drawing.Size(176, 30);
            this.tgTexBoxSelCiaFiscal.TabIndex = 10;
            this.tgTexBoxSelCiaFiscal.TituloFormSel = null;
            this.tgTexBoxSelCiaFiscal.TodoMayuscula = true;
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbLibro.FormattingEnabled = true;
            this.cmbLibro.Location = new System.Drawing.Point(163, 16);
            this.cmbLibro.Name = "cmbLibro";
            this.cmbLibro.Size = new System.Drawing.Size(199, 21);
            this.cmbLibro.TabIndex = 5;
            this.cmbLibro.SelectedIndexChanged += new System.EventHandler(this.cmbLibro_SelectedIndexChanged);
            // 
            // lblLibro
            // 
            this.lblLibro.AutoSize = true;
            this.lblLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblLibro.Location = new System.Drawing.Point(24, 24);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(30, 13);
            this.lblLibro.TabIndex = 40;
            this.lblLibro.Text = "Libro";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(309, 129);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 95;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
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
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(60, 668);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 104;
            // 
            // frmSiiEnviosConsulta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 691);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCerrarFacturas);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tgGridFacturas);
            this.Controls.Add(this.tgGridResumen);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiEnviosConsulta";
            this.Text = "Consultar Envios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiEnviosConsulta_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiEnviosConsulta_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiEnviosConsulta_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridResumen)).EndInit();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbEstadoFacturas.ResumeLayout(false);
            this.gbEstadoFacturas.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaPresentacionHasta;
        private System.Windows.Forms.Label lblFechaPresentacionHasta;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaPresentacionDesde;
        private System.Windows.Forms.Label lblFechaPresentacionDesde;
        private System.Windows.Forms.GroupBox gbEstadoFacturas;
        private System.Windows.Forms.RadioButton rbEstadoIncorrecto;
        private System.Windows.Forms.RadioButton rbEstadoAceptadoErrores;
        private System.Windows.Forms.RadioButton rbEstadoCorrecto;
        private System.Windows.Forms.RadioButton rbEstadoTodos;
        private System.Windows.Forms.Label lblEstadoFactura;
        private System.Windows.Forms.Label lblCompania;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cmbOperacion;
        private System.Windows.Forms.Label lblOperacion;
        private ObjectModel.TGGrid tgGridResumen;
        private ObjectModel.TGGrid tgGridFacturas;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNoInfo;
        private System.Windows.Forms.Button btnCerrarFacturas;
        private System.Windows.Forms.Panel panel1;
    }
}