namespace ModSII
{
    partial class frmSiiSuministroInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiSuministroInfo));
            this.gbResultados = new System.Windows.Forms.GroupBox();
            this.btnEnviarTodo = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.tgGridPdteEnvio = new ObjectModel.TGGrid();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.txtMaskFechaDocHasta = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaDocHasta = new System.Windows.Forms.Label();
            this.txtMaskFechaDocDesde = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaDocDesde = new System.Windows.Forms.Label();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.cmbEjercicio = new System.Windows.Forms.ComboBox();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.cmbOperacion = new System.Windows.Forms.ComboBox();
            this.lblOperacion = new System.Windows.Forms.Label();
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
            this.txtFactura = new System.Windows.Forms.TextBox();
            this.lblFactura = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbResultados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridPdteEnvio)).BeginInit();
            this.gbBuscador.SuspendLayout();
            this.gbListaCiasFiscales.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbResultados
            // 
            this.gbResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResultados.Controls.Add(this.btnEnviarTodo);
            this.gbResultados.Controls.Add(this.lblResult);
            this.gbResultados.Controls.Add(this.tgGridPdteEnvio);
            this.gbResultados.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbResultados.Location = new System.Drawing.Point(33, 231);
            this.gbResultados.Name = "gbResultados";
            this.gbResultados.Size = new System.Drawing.Size(915, 401);
            this.gbResultados.TabIndex = 110;
            this.gbResultados.TabStop = false;
            this.gbResultados.Text = " Pendiente de enviar";
            this.gbResultados.Visible = false;
            // 
            // btnEnviarTodo
            // 
            this.btnEnviarTodo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEnviarTodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnEnviarTodo.Image = ((System.Drawing.Image)(resources.GetObject("btnEnviarTodo.Image")));
            this.btnEnviarTodo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnviarTodo.Location = new System.Drawing.Point(439, 365);
            this.btnEnviarTodo.Name = "btnEnviarTodo";
            this.btnEnviarTodo.Size = new System.Drawing.Size(134, 21);
            this.btnEnviarTodo.TabIndex = 120;
            this.btnEnviarTodo.Text = "Enviar Todo";
            this.btnEnviarTodo.UseVisualStyleBackColor = true;
            this.btnEnviarTodo.Visible = false;
            this.btnEnviarTodo.Click += new System.EventHandler(this.btnEnviarTodo_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblResult.Location = new System.Drawing.Point(6, 22);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(293, 13);
            this.lblResult.TabIndex = 120;
            this.lblResult.Text = "No existen movimientos para el criterio de selección indicado";
            this.lblResult.Visible = false;
            // 
            // tgGridPdteEnvio
            // 
            this.tgGridPdteEnvio.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridPdteEnvio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridPdteEnvio.BackgroundColor = System.Drawing.Color.White;
            this.tgGridPdteEnvio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridPdteEnvio.ComboValores = null;
            this.tgGridPdteEnvio.ContextMenuStripGrid = null;
            this.tgGridPdteEnvio.DSDatos = null;
            this.tgGridPdteEnvio.Location = new System.Drawing.Point(6, 22);
            this.tgGridPdteEnvio.MultiSelect = false;
            this.tgGridPdteEnvio.Name = "tgGridPdteEnvio";
            this.tgGridPdteEnvio.NombreTabla = "";
            this.tgGridPdteEnvio.RowHeaderInitWidth = 41;
            this.tgGridPdteEnvio.RowNumber = false;
            this.tgGridPdteEnvio.Size = new System.Drawing.Size(903, 325);
            this.tgGridPdteEnvio.TabIndex = 115;
            this.tgGridPdteEnvio.Visible = false;
            this.tgGridPdteEnvio.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridPdteEnvio_CellContentClick);
            // 
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.txtMaskFechaDocHasta);
            this.gbBuscador.Controls.Add(this.lblFechaDocHasta);
            this.gbBuscador.Controls.Add(this.txtMaskFechaDocDesde);
            this.gbBuscador.Controls.Add(this.lblFechaDocDesde);
            this.gbBuscador.Controls.Add(this.txtEjercicio);
            this.gbBuscador.Controls.Add(this.cmbEjercicio);
            this.gbBuscador.Controls.Add(this.cmbPeriodo);
            this.gbBuscador.Controls.Add(this.cmbOperacion);
            this.gbBuscador.Controls.Add(this.lblOperacion);
            this.gbBuscador.Controls.Add(this.cmbLibro);
            this.gbBuscador.Controls.Add(this.lblLibro);
            this.gbBuscador.Controls.Add(this.lblPeriodo);
            this.gbBuscador.Controls.Add(this.lblEjercicio);
            this.gbBuscador.Controls.Add(this.gbListaCiasFiscales);
            this.gbBuscador.Controls.Add(this.btnTodos);
            this.gbBuscador.Controls.Add(this.txtFactura);
            this.gbBuscador.Controls.Add(this.lblFactura);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbBuscador.Location = new System.Drawing.Point(33, 41);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(915, 179);
            this.gbBuscador.TabIndex = 79;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // txtMaskFechaDocHasta
            // 
            this.txtMaskFechaDocHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtMaskFechaDocHasta.Location = new System.Drawing.Point(730, 70);
            this.txtMaskFechaDocHasta.Mask = "00-00-0000";
            this.txtMaskFechaDocHasta.Name = "txtMaskFechaDocHasta";
            this.txtMaskFechaDocHasta.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaDocHasta.TabIndex = 75;
            // 
            // lblFechaDocHasta
            // 
            this.lblFechaDocHasta.AutoSize = true;
            this.lblFechaDocHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaDocHasta.Location = new System.Drawing.Point(629, 77);
            this.lblFechaDocHasta.Name = "lblFechaDocHasta";
            this.lblFechaDocHasta.Size = new System.Drawing.Size(92, 13);
            this.lblFechaDocHasta.TabIndex = 70;
            this.lblFechaDocHasta.Text = "Fecha Exp. Hasta";
            // 
            // txtMaskFechaDocDesde
            // 
            this.txtMaskFechaDocDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtMaskFechaDocDesde.Location = new System.Drawing.Point(519, 72);
            this.txtMaskFechaDocDesde.Mask = "00-00-0000";
            this.txtMaskFechaDocDesde.Name = "txtMaskFechaDocDesde";
            this.txtMaskFechaDocDesde.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaDocDesde.TabIndex = 65;
            // 
            // lblFechaDocDesde
            // 
            this.lblFechaDocDesde.AutoSize = true;
            this.lblFechaDocDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaDocDesde.Location = new System.Drawing.Point(418, 77);
            this.lblFechaDocDesde.Name = "lblFechaDocDesde";
            this.lblFechaDocDesde.Size = new System.Drawing.Size(95, 13);
            this.lblFechaDocDesde.TabIndex = 60;
            this.lblFechaDocDesde.Text = "Fecha Exp. Desde";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtEjercicio.Location = new System.Drawing.Point(471, 38);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 35;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            // 
            // cmbEjercicio
            // 
            this.cmbEjercicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbEjercicio.FormattingEnabled = true;
            this.cmbEjercicio.Location = new System.Drawing.Point(859, 148);
            this.cmbEjercicio.Name = "cmbEjercicio";
            this.cmbEjercicio.Size = new System.Drawing.Size(50, 21);
            this.cmbEjercicio.TabIndex = 32;
            this.cmbEjercicio.Visible = false;
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(590, 38);
            this.cmbPeriodo.MaxLength = 2;
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(53, 21);
            this.cmbPeriodo.TabIndex = 45;
            this.cmbPeriodo.Leave += new System.EventHandler(this.cmbPeriodo_Leave);
            // 
            // cmbOperacion
            // 
            this.cmbOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbOperacion.FormattingEnabled = true;
            this.cmbOperacion.Location = new System.Drawing.Point(730, 101);
            this.cmbOperacion.Name = "cmbOperacion";
            this.cmbOperacion.Size = new System.Drawing.Size(168, 21);
            this.cmbOperacion.TabIndex = 95;
            // 
            // lblOperacion
            // 
            this.lblOperacion.AutoSize = true;
            this.lblOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblOperacion.Location = new System.Drawing.Point(665, 104);
            this.lblOperacion.Name = "lblOperacion";
            this.lblOperacion.Size = new System.Drawing.Size(56, 13);
            this.lblOperacion.TabIndex = 90;
            this.lblOperacion.Text = "Operación";
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbLibro.FormattingEnabled = true;
            this.cmbLibro.Location = new System.Drawing.Point(471, 101);
            this.cmbLibro.Name = "cmbLibro";
            this.cmbLibro.Size = new System.Drawing.Size(172, 21);
            this.cmbLibro.TabIndex = 85;
            this.cmbLibro.SelectedIndexChanged += new System.EventHandler(this.cmbLibro_SelectedIndexChanged);
            // 
            // lblLibro
            // 
            this.lblLibro.AutoSize = true;
            this.lblLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblLibro.Location = new System.Drawing.Point(418, 104);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(30, 13);
            this.lblLibro.TabIndex = 80;
            this.lblLibro.Text = "Libro";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblPeriodo.Location = new System.Drawing.Point(540, 41);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 40;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(418, 41);
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
            this.btnQuitarCiaFiscal.Location = new System.Drawing.Point(107, 56);
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
            this.btnAddCiaFiscal.Location = new System.Drawing.Point(26, 56);
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
            this.btnTodos.Location = new System.Drawing.Point(471, 148);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(80, 21);
            this.btnTodos.TabIndex = 105;
            this.btnTodos.Text = "Todos";
            this.btnTodos.UseVisualStyleBackColor = true;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // txtFactura
            // 
            this.txtFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtFactura.Location = new System.Drawing.Point(730, 38);
            this.txtFactura.MaxLength = 25;
            this.txtFactura.Name = "txtFactura";
            this.txtFactura.Size = new System.Drawing.Size(172, 20);
            this.txtFactura.TabIndex = 55;
            // 
            // lblFactura
            // 
            this.lblFactura.AutoSize = true;
            this.lblFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFactura.Location = new System.Drawing.Point(658, 41);
            this.lblFactura.Name = "lblFactura";
            this.lblFactura.Size = new System.Drawing.Size(63, 13);
            this.lblFactura.TabIndex = 50;
            this.lblFactura.Text = "No. Factura";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(351, 148);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 100;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGrabarPeticion,
            this.toolStripButtonCargarPeticion,
            this.toolStripSeparator2,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(978, 25);
            this.toolStrip1.TabIndex = 78;
            this.toolStrip1.Text = "toolStrip1";
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
            this.panel1.Location = new System.Drawing.Point(39, 638);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 111;
            // 
            // frmSiiSuministroInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 656);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbResultados);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiSuministroInfo";
            this.Text = "Suministro Información";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiSuministroInfo_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiSuministroInfo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiSuministroInfo_KeyDown);
            this.gbResultados.ResumeLayout(false);
            this.gbResultados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridPdteEnvio)).EndInit();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbListaCiasFiscales.ResumeLayout(false);
            this.gbListaCiasFiscales.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.TextBox txtFactura;
        private System.Windows.Forms.Label lblFactura;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox gbListaCiasFiscales;
        private System.Windows.Forms.CheckBox chkCiasFiscalesTodas;
        private System.Windows.Forms.Button btnQuitarCiaFiscal;
        private System.Windows.Forms.Button btnAddCiaFiscal;
        private System.Windows.Forms.ListBox lbCiasFiscales;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.ComboBox cmbOperacion;
        private System.Windows.Forms.Label lblOperacion;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.GroupBox gbResultados;
        private System.Windows.Forms.Label lblResult;
        private ObjectModel.TGGrid tgGridPdteEnvio;
        private System.Windows.Forms.Button btnEnviarTodo;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.ComboBox cmbEjercicio;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaDocHasta;
        private System.Windows.Forms.Label lblFechaDocHasta;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaDocDesde;
        private System.Windows.Forms.Label lblFechaDocDesde;
        private System.Windows.Forms.Panel panel1;
    }
}