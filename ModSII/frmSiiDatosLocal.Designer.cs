namespace ModSII
{
    partial class frmSiiDatosLocal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiDatosLocal));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.tgGridConsulta = new ObjectModel.TGGrid();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.cmbPais = new System.Windows.Forms.ComboBox();
            this.txtMaskFechaExpedicionHasta = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaExpedicionHasta = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cmbClaveOperacion = new System.Windows.Forms.ComboBox();
            this.lblClaveOperacion = new System.Windows.Forms.Label();
            this.txtMaskFechaExpedicionDesde = new System.Windows.Forms.MaskedTextBox();
            this.gbEstadoFacturas = new System.Windows.Forms.GroupBox();
            this.rbEstadoAnulada = new System.Windows.Forms.RadioButton();
            this.rbEstadoPdteEnvio = new System.Windows.Forms.RadioButton();
            this.rbEstadoAceptadoErrores = new System.Windows.Forms.RadioButton();
            this.rbEstadoCorrecto = new System.Windows.Forms.RadioButton();
            this.rbEstadoSoloErrores = new System.Windows.Forms.RadioButton();
            this.rbEstadoTodas = new System.Windows.Forms.RadioButton();
            this.lblEstadoFactura = new System.Windows.Forms.Label();
            this.lblFechaExpedicionDesde = new System.Windows.Forms.Label();
            this.txtNumSerieFactura = new System.Windows.Forms.TextBox();
            this.lblNumSerieFactura = new System.Windows.Forms.Label();
            this.cmbTipoIdentif = new System.Windows.Forms.ComboBox();
            this.txtNombreRazon = new System.Windows.Forms.TextBox();
            this.lblNombreRazon = new System.Windows.Forms.Label();
            this.lblTipoIden = new System.Windows.Forms.Label();
            this.lblCodPais = new System.Windows.Forms.Label();
            this.txtNIF = new System.Windows.Forms.TextBox();
            this.lblNIF = new System.Windows.Forms.Label();
            this.gbTipoIdentificacion = new System.Windows.Forms.GroupBox();
            this.rbOtro = new System.Windows.Forms.RadioButton();
            this.rbNIF = new System.Windows.Forms.RadioButton();
            this.lblTipoIdentificacion = new System.Windows.Forms.Label();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.lblCompaniaFiscal = new System.Windows.Forms.Label();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.lblLibro = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.gbResultado = new System.Windows.Forms.GroupBox();
            this.lblCuotaTotalValor = new System.Windows.Forms.Label();
            this.lblCuotaTotal = new System.Windows.Forms.Label();
            this.lblBaseImponibleTotalValor = new System.Windows.Forms.Label();
            this.lblBaseImponibleTotal = new System.Windows.Forms.Label();
            this.lblImporteTotalValor = new System.Windows.Forms.Label();
            this.lblImporteTotal = new System.Windows.Forms.Label();
            this.lblTotalRegValor = new System.Windows.Forms.Label();
            this.lblTotalReg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).BeginInit();
            this.gbBuscador.SuspendLayout();
            this.gbEstadoFacturas.SuspendLayout();
            this.gbTipoIdentificacion.SuspendLayout();
            this.gbResultado.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(460, 319);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 90;
            this.lblInfo.Text = "Su petición se está procesando, espere por favor.";
            this.lblInfo.Visible = false;
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
            this.toolStrip1.TabIndex = 86;
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
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNoInfo.Location = new System.Drawing.Point(33, 287);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(273, 13);
            this.lblNoInfo.TabIndex = 85;
            this.lblNoInfo.Text = "No existen facturas que cumplan el criterio seleccionado";
            this.lblNoInfo.Visible = false;
            // 
            // tgGridConsulta
            // 
            this.tgGridConsulta.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridConsulta.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridConsulta.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridConsulta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tgGridConsulta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridConsulta.ComboValores = null;
            this.tgGridConsulta.ContextMenuStripGrid = null;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridConsulta.DefaultCellStyle = dataGridViewCellStyle5;
            this.tgGridConsulta.DSDatos = null;
            this.tgGridConsulta.Location = new System.Drawing.Point(33, 324);
            this.tgGridConsulta.Name = "tgGridConsulta";
            this.tgGridConsulta.NombreTabla = "Consulta";
            this.tgGridConsulta.RowHeaderInitWidth = 41;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridConsulta.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tgGridConsulta.RowNumber = false;
            this.tgGridConsulta.Size = new System.Drawing.Size(1083, 395);
            this.tgGridConsulta.TabIndex = 84;
            this.tgGridConsulta.Visible = false;
            this.tgGridConsulta.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridConsulta_CellContentClick);
            this.tgGridConsulta.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridConsulta_CellDoubleClick);
            this.tgGridConsulta.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridConsulta_DataBindingComplete);
            // 
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.cmbPais);
            this.gbBuscador.Controls.Add(this.txtMaskFechaExpedicionHasta);
            this.gbBuscador.Controls.Add(this.lblFechaExpedicionHasta);
            this.gbBuscador.Controls.Add(this.btnLimpiar);
            this.gbBuscador.Controls.Add(this.cmbClaveOperacion);
            this.gbBuscador.Controls.Add(this.lblClaveOperacion);
            this.gbBuscador.Controls.Add(this.txtMaskFechaExpedicionDesde);
            this.gbBuscador.Controls.Add(this.gbEstadoFacturas);
            this.gbBuscador.Controls.Add(this.lblEstadoFactura);
            this.gbBuscador.Controls.Add(this.lblFechaExpedicionDesde);
            this.gbBuscador.Controls.Add(this.txtNumSerieFactura);
            this.gbBuscador.Controls.Add(this.lblNumSerieFactura);
            this.gbBuscador.Controls.Add(this.cmbTipoIdentif);
            this.gbBuscador.Controls.Add(this.txtNombreRazon);
            this.gbBuscador.Controls.Add(this.lblNombreRazon);
            this.gbBuscador.Controls.Add(this.lblTipoIden);
            this.gbBuscador.Controls.Add(this.lblCodPais);
            this.gbBuscador.Controls.Add(this.txtNIF);
            this.gbBuscador.Controls.Add(this.lblNIF);
            this.gbBuscador.Controls.Add(this.gbTipoIdentificacion);
            this.gbBuscador.Controls.Add(this.lblTipoIdentificacion);
            this.gbBuscador.Controls.Add(this.txtEjercicio);
            this.gbBuscador.Controls.Add(this.lblCompaniaFiscal);
            this.gbBuscador.Controls.Add(this.tgTexBoxSelCiaFiscal);
            this.gbBuscador.Controls.Add(this.cmbPeriodo);
            this.gbBuscador.Controls.Add(this.cmbLibro);
            this.gbBuscador.Controls.Add(this.lblLibro);
            this.gbBuscador.Controls.Add(this.lblPeriodo);
            this.gbBuscador.Controls.Add(this.lblEjercicio);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbBuscador.Location = new System.Drawing.Point(33, 41);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(1083, 232);
            this.gbBuscador.TabIndex = 83;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // cmbPais
            // 
            this.cmbPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPais.Enabled = false;
            this.cmbPais.FormattingEnabled = true;
            this.cmbPais.Location = new System.Drawing.Point(574, 78);
            this.cmbPais.Name = "cmbPais";
            this.cmbPais.Size = new System.Drawing.Size(199, 21);
            this.cmbPais.TabIndex = 96;
            // 
            // txtMaskFechaExpedicionHasta
            // 
            this.txtMaskFechaExpedicionHasta.Location = new System.Drawing.Point(874, 134);
            this.txtMaskFechaExpedicionHasta.Mask = "00-00-0000";
            this.txtMaskFechaExpedicionHasta.Name = "txtMaskFechaExpedicionHasta";
            this.txtMaskFechaExpedicionHasta.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaExpedicionHasta.TabIndex = 60;
            // 
            // lblFechaExpedicionHasta
            // 
            this.lblFechaExpedicionHasta.AutoSize = true;
            this.lblFechaExpedicionHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaExpedicionHasta.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblFechaExpedicionHasta.Location = new System.Drawing.Point(749, 137);
            this.lblFechaExpedicionHasta.Name = "lblFechaExpedicionHasta";
            this.lblFechaExpedicionHasta.Size = new System.Drawing.Size(123, 13);
            this.lblFechaExpedicionHasta.TabIndex = 88;
            this.lblFechaExpedicionHasta.Text = "Fecha Expedición Hasta";
            this.lblFechaExpedicionHasta.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLimpiar.Location = new System.Drawing.Point(568, 197);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(80, 21);
            this.btnLimpiar.TabIndex = 95;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cmbClaveOperacion
            // 
            this.cmbClaveOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClaveOperacion.FormattingEnabled = true;
            this.cmbClaveOperacion.Location = new System.Drawing.Point(876, 164);
            this.cmbClaveOperacion.MaxLength = 2;
            this.cmbClaveOperacion.Name = "cmbClaveOperacion";
            this.cmbClaveOperacion.Size = new System.Drawing.Size(166, 21);
            this.cmbClaveOperacion.TabIndex = 85;
            this.cmbClaveOperacion.Visible = false;
            // 
            // lblClaveOperacion
            // 
            this.lblClaveOperacion.AutoSize = true;
            this.lblClaveOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblClaveOperacion.Location = new System.Drawing.Point(789, 169);
            this.lblClaveOperacion.Name = "lblClaveOperacion";
            this.lblClaveOperacion.Size = new System.Drawing.Size(86, 13);
            this.lblClaveOperacion.TabIndex = 84;
            this.lblClaveOperacion.Text = "Clave Operación";
            this.lblClaveOperacion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblClaveOperacion.Visible = false;
            // 
            // txtMaskFechaExpedicionDesde
            // 
            this.txtMaskFechaExpedicionDesde.Location = new System.Drawing.Point(573, 134);
            this.txtMaskFechaExpedicionDesde.Mask = "00-00-0000";
            this.txtMaskFechaExpedicionDesde.Name = "txtMaskFechaExpedicionDesde";
            this.txtMaskFechaExpedicionDesde.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaExpedicionDesde.TabIndex = 55;
            // 
            // gbEstadoFacturas
            // 
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoAnulada);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoPdteEnvio);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoAceptadoErrores);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoCorrecto);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoSoloErrores);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoTodas);
            this.gbEstadoFacturas.Location = new System.Drawing.Point(164, 156);
            this.gbEstadoFacturas.Name = "gbEstadoFacturas";
            this.gbEstadoFacturas.Size = new System.Drawing.Size(573, 31);
            this.gbEstadoFacturas.TabIndex = 70;
            this.gbEstadoFacturas.TabStop = false;
            // 
            // rbEstadoAnulada
            // 
            this.rbEstadoAnulada.AutoSize = true;
            this.rbEstadoAnulada.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoAnulada.Location = new System.Drawing.Point(498, 10);
            this.rbEstadoAnulada.Name = "rbEstadoAnulada";
            this.rbEstadoAnulada.Size = new System.Drawing.Size(64, 17);
            this.rbEstadoAnulada.TabIndex = 5;
            this.rbEstadoAnulada.Text = "Anulada";
            this.rbEstadoAnulada.UseVisualStyleBackColor = true;
            // 
            // rbEstadoPdteEnvio
            // 
            this.rbEstadoPdteEnvio.AutoSize = true;
            this.rbEstadoPdteEnvio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoPdteEnvio.Location = new System.Drawing.Point(73, 10);
            this.rbEstadoPdteEnvio.Name = "rbEstadoPdteEnvio";
            this.rbEstadoPdteEnvio.Size = new System.Drawing.Size(119, 17);
            this.rbEstadoPdteEnvio.TabIndex = 4;
            this.rbEstadoPdteEnvio.Text = "Pendiente de envío";
            this.rbEstadoPdteEnvio.UseVisualStyleBackColor = true;
            // 
            // rbEstadoAceptadoErrores
            // 
            this.rbEstadoAceptadoErrores.AutoSize = true;
            this.rbEstadoAceptadoErrores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoAceptadoErrores.Location = new System.Drawing.Point(360, 10);
            this.rbEstadoAceptadoErrores.Name = "rbEstadoAceptadoErrores";
            this.rbEstadoAceptadoErrores.Size = new System.Drawing.Size(127, 17);
            this.rbEstadoAceptadoErrores.TabIndex = 3;
            this.rbEstadoAceptadoErrores.Text = "Aceptada con errores";
            this.rbEstadoAceptadoErrores.UseVisualStyleBackColor = true;
            // 
            // rbEstadoCorrecto
            // 
            this.rbEstadoCorrecto.AutoSize = true;
            this.rbEstadoCorrecto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoCorrecto.Location = new System.Drawing.Point(282, 10);
            this.rbEstadoCorrecto.Name = "rbEstadoCorrecto";
            this.rbEstadoCorrecto.Size = new System.Drawing.Size(65, 17);
            this.rbEstadoCorrecto.TabIndex = 2;
            this.rbEstadoCorrecto.Text = "Correcta";
            this.rbEstadoCorrecto.UseVisualStyleBackColor = true;
            // 
            // rbEstadoSoloErrores
            // 
            this.rbEstadoSoloErrores.AutoSize = true;
            this.rbEstadoSoloErrores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoSoloErrores.Location = new System.Drawing.Point(199, 10);
            this.rbEstadoSoloErrores.Name = "rbEstadoSoloErrores";
            this.rbEstadoSoloErrores.Size = new System.Drawing.Size(73, 17);
            this.rbEstadoSoloErrores.TabIndex = 1;
            this.rbEstadoSoloErrores.Text = "Incorrecta";
            this.rbEstadoSoloErrores.UseVisualStyleBackColor = true;
            // 
            // rbEstadoTodas
            // 
            this.rbEstadoTodas.AutoSize = true;
            this.rbEstadoTodas.Checked = true;
            this.rbEstadoTodas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoTodas.Location = new System.Drawing.Point(11, 10);
            this.rbEstadoTodas.Name = "rbEstadoTodas";
            this.rbEstadoTodas.Size = new System.Drawing.Size(55, 17);
            this.rbEstadoTodas.TabIndex = 0;
            this.rbEstadoTodas.TabStop = true;
            this.rbEstadoTodas.Text = "Todas";
            this.rbEstadoTodas.UseVisualStyleBackColor = true;
            // 
            // lblEstadoFactura
            // 
            this.lblEstadoFactura.AutoSize = true;
            this.lblEstadoFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEstadoFactura.Location = new System.Drawing.Point(25, 167);
            this.lblEstadoFactura.Name = "lblEstadoFactura";
            this.lblEstadoFactura.Size = new System.Drawing.Size(81, 13);
            this.lblEstadoFactura.TabIndex = 67;
            this.lblEstadoFactura.Text = "Estado facturas";
            // 
            // lblFechaExpedicionDesde
            // 
            this.lblFechaExpedicionDesde.AutoSize = true;
            this.lblFechaExpedicionDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaExpedicionDesde.Location = new System.Drawing.Point(441, 137);
            this.lblFechaExpedicionDesde.Name = "lblFechaExpedicionDesde";
            this.lblFechaExpedicionDesde.Size = new System.Drawing.Size(126, 13);
            this.lblFechaExpedicionDesde.TabIndex = 65;
            this.lblFechaExpedicionDesde.Text = "Fecha Expedición Desde";
            this.lblFechaExpedicionDesde.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNumSerieFactura
            // 
            this.txtNumSerieFactura.Location = new System.Drawing.Point(165, 134);
            this.txtNumSerieFactura.MaxLength = 45;
            this.txtNumSerieFactura.Name = "txtNumSerieFactura";
            this.txtNumSerieFactura.Size = new System.Drawing.Size(198, 20);
            this.txtNumSerieFactura.TabIndex = 50;
            // 
            // lblNumSerieFactura
            // 
            this.lblNumSerieFactura.AutoSize = true;
            this.lblNumSerieFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNumSerieFactura.Location = new System.Drawing.Point(25, 137);
            this.lblNumSerieFactura.Name = "lblNumSerieFactura";
            this.lblNumSerieFactura.Size = new System.Drawing.Size(110, 13);
            this.lblNumSerieFactura.TabIndex = 63;
            this.lblNumSerieFactura.Text = "Número Serie Factura";
            // 
            // cmbTipoIdentif
            // 
            this.cmbTipoIdentif.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoIdentif.Enabled = false;
            this.cmbTipoIdentif.FormattingEnabled = true;
            this.cmbTipoIdentif.Location = new System.Drawing.Point(874, 79);
            this.cmbTipoIdentif.Name = "cmbTipoIdentif";
            this.cmbTipoIdentif.Size = new System.Drawing.Size(199, 21);
            this.cmbTipoIdentif.TabIndex = 40;
            // 
            // txtNombreRazon
            // 
            this.txtNombreRazon.Enabled = false;
            this.txtNombreRazon.Location = new System.Drawing.Point(573, 108);
            this.txtNombreRazon.MaxLength = 40;
            this.txtNombreRazon.Name = "txtNombreRazon";
            this.txtNombreRazon.Size = new System.Drawing.Size(453, 20);
            this.txtNombreRazon.TabIndex = 45;
            // 
            // lblNombreRazon
            // 
            this.lblNombreRazon.AutoSize = true;
            this.lblNombreRazon.Enabled = false;
            this.lblNombreRazon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNombreRazon.Location = new System.Drawing.Point(420, 112);
            this.lblNombreRazon.Name = "lblNombreRazon";
            this.lblNombreRazon.Size = new System.Drawing.Size(147, 13);
            this.lblNombreRazon.TabIndex = 60;
            this.lblNombreRazon.Text = "Nombre o Razón Social Dest.";
            this.lblNombreRazon.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTipoIden
            // 
            this.lblTipoIden.AutoSize = true;
            this.lblTipoIden.Enabled = false;
            this.lblTipoIden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTipoIden.Location = new System.Drawing.Point(781, 81);
            this.lblTipoIden.Name = "lblTipoIden";
            this.lblTipoIden.Size = new System.Drawing.Size(91, 13);
            this.lblTipoIden.TabIndex = 58;
            this.lblTipoIden.Text = "Tipo Identif. Dest.";
            // 
            // lblCodPais
            // 
            this.lblCodPais.AutoSize = true;
            this.lblCodPais.Enabled = false;
            this.lblCodPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCodPais.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCodPais.Location = new System.Drawing.Point(457, 82);
            this.lblCodPais.Name = "lblCodPais";
            this.lblCodPais.Size = new System.Drawing.Size(111, 13);
            this.lblCodPais.TabIndex = 56;
            this.lblCodPais.Text = "Cod. Pais Destinatario";
            this.lblCodPais.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNIF
            // 
            this.txtNIF.Location = new System.Drawing.Point(164, 108);
            this.txtNIF.MaxLength = 20;
            this.txtNIF.Name = "txtNIF";
            this.txtNIF.Size = new System.Drawing.Size(143, 20);
            this.txtNIF.TabIndex = 30;
            // 
            // lblNIF
            // 
            this.lblNIF.AutoSize = true;
            this.lblNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNIF.Location = new System.Drawing.Point(25, 112);
            this.lblNIF.Name = "lblNIF";
            this.lblNIF.Size = new System.Drawing.Size(83, 13);
            this.lblNIF.TabIndex = 54;
            this.lblNIF.Text = "NIF Destinatario";
            this.lblNIF.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbTipoIdentificacion
            // 
            this.gbTipoIdentificacion.Controls.Add(this.rbOtro);
            this.gbTipoIdentificacion.Controls.Add(this.rbNIF);
            this.gbTipoIdentificacion.Location = new System.Drawing.Point(163, 73);
            this.gbTipoIdentificacion.Name = "gbTipoIdentificacion";
            this.gbTipoIdentificacion.Size = new System.Drawing.Size(142, 31);
            this.gbTipoIdentificacion.TabIndex = 25;
            this.gbTipoIdentificacion.TabStop = false;
            // 
            // rbOtro
            // 
            this.rbOtro.AutoSize = true;
            this.rbOtro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbOtro.Location = new System.Drawing.Point(81, 10);
            this.rbOtro.Name = "rbOtro";
            this.rbOtro.Size = new System.Drawing.Size(45, 17);
            this.rbOtro.TabIndex = 1;
            this.rbOtro.Text = "Otro";
            this.rbOtro.UseVisualStyleBackColor = true;
            // 
            // rbNIF
            // 
            this.rbNIF.AutoSize = true;
            this.rbNIF.Checked = true;
            this.rbNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbNIF.Location = new System.Drawing.Point(18, 10);
            this.rbNIF.Name = "rbNIF";
            this.rbNIF.Size = new System.Drawing.Size(42, 17);
            this.rbNIF.TabIndex = 0;
            this.rbNIF.TabStop = true;
            this.rbNIF.Text = "NIF";
            this.rbNIF.UseVisualStyleBackColor = true;
            this.rbNIF.CheckedChanged += new System.EventHandler(this.rbNIF_CheckedChanged);
            // 
            // lblTipoIdentificacion
            // 
            this.lblTipoIdentificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTipoIdentificacion.Location = new System.Drawing.Point(25, 81);
            this.lblTipoIdentificacion.Name = "lblTipoIdentificacion";
            this.lblTipoIdentificacion.Size = new System.Drawing.Size(119, 13);
            this.lblTipoIdentificacion.TabIndex = 52;
            this.lblTipoIdentificacion.Text = "Tipo de Identificación";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Location = new System.Drawing.Point(574, 47);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 15;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            // 
            // lblCompaniaFiscal
            // 
            this.lblCompaniaFiscal.AutoSize = true;
            this.lblCompaniaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCompaniaFiscal.Location = new System.Drawing.Point(25, 50);
            this.lblCompaniaFiscal.Name = "lblCompaniaFiscal";
            this.lblCompaniaFiscal.Size = new System.Drawing.Size(86, 13);
            this.lblCompaniaFiscal.TabIndex = 10;
            this.lblCompaniaFiscal.Text = "Compañía Fiscal";
            // 
            // tgTexBoxSelCiaFiscal
            // 
            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 0;
            this.tgTexBoxSelCiaFiscal.CentrarFormSel = true;
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = null;
            this.tgTexBoxSelCiaFiscal.FrmPadre = null;
            this.tgTexBoxSelCiaFiscal.Location = new System.Drawing.Point(161, 43);
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
            this.tgTexBoxSelCiaFiscal.Enter += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Enter);
            this.tgTexBoxSelCiaFiscal.Leave += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Leave);
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(874, 46);
            this.cmbPeriodo.MaxLength = 2;
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(53, 21);
            this.cmbPeriodo.TabIndex = 20;
            this.cmbPeriodo.Leave += new System.EventHandler(this.cmbPeriodo_Leave);
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblPeriodo.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPeriodo.Location = new System.Drawing.Point(825, 50);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 30;
            this.lblPeriodo.Text = "Periodo";
            this.lblPeriodo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(521, 50);
            this.lblEjercicio.Name = "lblEjercicio";
            this.lblEjercicio.Size = new System.Drawing.Size(47, 13);
            this.lblEjercicio.TabIndex = 20;
            this.lblEjercicio.Text = "Ejercicio";
            this.lblEjercicio.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(449, 197);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 90;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // gbResultado
            // 
            this.gbResultado.Controls.Add(this.lblCuotaTotalValor);
            this.gbResultado.Controls.Add(this.lblCuotaTotal);
            this.gbResultado.Controls.Add(this.lblBaseImponibleTotalValor);
            this.gbResultado.Controls.Add(this.lblBaseImponibleTotal);
            this.gbResultado.Controls.Add(this.lblImporteTotalValor);
            this.gbResultado.Controls.Add(this.lblImporteTotal);
            this.gbResultado.Controls.Add(this.lblTotalRegValor);
            this.gbResultado.Controls.Add(this.lblTotalReg);
            this.gbResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbResultado.Location = new System.Drawing.Point(33, 279);
            this.gbResultado.Name = "gbResultado";
            this.gbResultado.Size = new System.Drawing.Size(1083, 39);
            this.gbResultado.TabIndex = 87;
            this.gbResultado.TabStop = false;
            this.gbResultado.Text = " Resultado ";
            this.gbResultado.Visible = false;
            // 
            // lblCuotaTotalValor
            // 
            this.lblCuotaTotalValor.AutoSize = true;
            this.lblCuotaTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuotaTotalValor.Location = new System.Drawing.Point(884, 17);
            this.lblCuotaTotalValor.Name = "lblCuotaTotalValor";
            this.lblCuotaTotalValor.Size = new System.Drawing.Size(100, 13);
            this.lblCuotaTotalValor.TabIndex = 19;
            this.lblCuotaTotalValor.Text = "valor cuota total";
            // 
            // lblCuotaTotal
            // 
            this.lblCuotaTotal.AutoSize = true;
            this.lblCuotaTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCuotaTotal.Location = new System.Drawing.Point(813, 17);
            this.lblCuotaTotal.Name = "lblCuotaTotal";
            this.lblCuotaTotal.Size = new System.Drawing.Size(62, 13);
            this.lblCuotaTotal.TabIndex = 18;
            this.lblCuotaTotal.Text = "Total Cuota";
            // 
            // lblBaseImponibleTotalValor
            // 
            this.lblBaseImponibleTotalValor.AutoSize = true;
            this.lblBaseImponibleTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseImponibleTotalValor.Location = new System.Drawing.Point(626, 17);
            this.lblBaseImponibleTotalValor.Name = "lblBaseImponibleTotalValor";
            this.lblBaseImponibleTotalValor.Size = new System.Drawing.Size(118, 13);
            this.lblBaseImponibleTotalValor.TabIndex = 17;
            this.lblBaseImponibleTotalValor.Text = "valor base imp total";
            // 
            // lblBaseImponibleTotal
            // 
            this.lblBaseImponibleTotal.AutoSize = true;
            this.lblBaseImponibleTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblBaseImponibleTotal.Location = new System.Drawing.Point(516, 17);
            this.lblBaseImponibleTotal.Name = "lblBaseImponibleTotal";
            this.lblBaseImponibleTotal.Size = new System.Drawing.Size(106, 13);
            this.lblBaseImponibleTotal.TabIndex = 16;
            this.lblBaseImponibleTotal.Text = "Total Base Imponible";
            // 
            // lblImporteTotalValor
            // 
            this.lblImporteTotalValor.AutoSize = true;
            this.lblImporteTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImporteTotalValor.Location = new System.Drawing.Point(348, 17);
            this.lblImporteTotalValor.Name = "lblImporteTotalValor";
            this.lblImporteTotalValor.Size = new System.Drawing.Size(87, 13);
            this.lblImporteTotalValor.TabIndex = 15;
            this.lblImporteTotalValor.Text = "valor imp total";
            // 
            // lblImporteTotal
            // 
            this.lblImporteTotal.AutoSize = true;
            this.lblImporteTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblImporteTotal.Location = new System.Drawing.Point(268, 17);
            this.lblImporteTotal.Name = "lblImporteTotal";
            this.lblImporteTotal.Size = new System.Drawing.Size(69, 13);
            this.lblImporteTotal.TabIndex = 14;
            this.lblImporteTotal.Text = "Total Importe";
            // 
            // lblTotalRegValor
            // 
            this.lblTotalRegValor.AutoSize = true;
            this.lblTotalRegValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRegValor.Location = new System.Drawing.Point(104, 17);
            this.lblTotalRegValor.Name = "lblTotalRegValor";
            this.lblTotalRegValor.Size = new System.Drawing.Size(86, 13);
            this.lblTotalRegValor.TabIndex = 13;
            this.lblTotalRegValor.Text = "valor total reg";
            // 
            // lblTotalReg
            // 
            this.lblTotalReg.AutoSize = true;
            this.lblTotalReg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTotalReg.Location = new System.Drawing.Point(24, 17);
            this.lblTotalReg.Name = "lblTotalReg";
            this.lblTotalReg.Size = new System.Drawing.Size(78, 13);
            this.lblTotalReg.TabIndex = 12;
            this.lblTotalReg.Text = "Total Registros";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(47, 736);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 91;
            // 
            // frmSiiDatosLocal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 749);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.tgGridConsulta);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.gbResultado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiDatosLocal";
            this.Text = "Consulta Datos Local";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiDatosLocal_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiDatosLocal_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiDatosLocal_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).EndInit();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbEstadoFacturas.ResumeLayout(false);
            this.gbEstadoFacturas.PerformLayout();
            this.gbTipoIdentificacion.ResumeLayout(false);
            this.gbTipoIdentificacion.PerformLayout();
            this.gbResultado.ResumeLayout(false);
            this.gbResultado.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNoInfo;
        private ObjectModel.TGGrid tgGridConsulta;
        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaExpedicionDesde;
        private System.Windows.Forms.GroupBox gbEstadoFacturas;
        private System.Windows.Forms.RadioButton rbEstadoAceptadoErrores;
        private System.Windows.Forms.RadioButton rbEstadoCorrecto;
        private System.Windows.Forms.RadioButton rbEstadoSoloErrores;
        private System.Windows.Forms.RadioButton rbEstadoTodas;
        private System.Windows.Forms.Label lblEstadoFactura;
        private System.Windows.Forms.Label lblFechaExpedicionDesde;
        private System.Windows.Forms.TextBox txtNumSerieFactura;
        private System.Windows.Forms.Label lblNumSerieFactura;
        private System.Windows.Forms.ComboBox cmbTipoIdentif;
        private System.Windows.Forms.TextBox txtNombreRazon;
        private System.Windows.Forms.Label lblNombreRazon;
        private System.Windows.Forms.Label lblTipoIden;
        private System.Windows.Forms.Label lblCodPais;
        private System.Windows.Forms.TextBox txtNIF;
        private System.Windows.Forms.Label lblNIF;
        private System.Windows.Forms.GroupBox gbTipoIdentificacion;
        private System.Windows.Forms.RadioButton rbOtro;
        private System.Windows.Forms.RadioButton rbNIF;
        private System.Windows.Forms.Label lblTipoIdentificacion;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.Label lblCompaniaFiscal;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.RadioButton rbEstadoPdteEnvio;
        private System.Windows.Forms.ComboBox cmbClaveOperacion;
        private System.Windows.Forms.Label lblClaveOperacion;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox gbResultado;
        private System.Windows.Forms.Label lblImporteTotalValor;
        private System.Windows.Forms.Label lblImporteTotal;
        private System.Windows.Forms.Label lblTotalRegValor;
        private System.Windows.Forms.Label lblTotalReg;
        private System.Windows.Forms.Label lblCuotaTotalValor;
        private System.Windows.Forms.Label lblCuotaTotal;
        private System.Windows.Forms.Label lblBaseImponibleTotalValor;
        private System.Windows.Forms.Label lblBaseImponibleTotal;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaExpedicionHasta;
        private System.Windows.Forms.Label lblFechaExpedicionHasta;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.RadioButton rbEstadoAnulada;
        private System.Windows.Forms.ComboBox cmbPais;
        private System.Windows.Forms.Panel panel1;
    }
}