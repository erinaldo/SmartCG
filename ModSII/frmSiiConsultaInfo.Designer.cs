namespace ModSII
{
    partial class frmSiiConsultaInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiConsultaInfo));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.cmbPais = new System.Windows.Forms.ComboBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cmbClaveOperacion = new System.Windows.Forms.ComboBox();
            this.lblClaveOperacion = new System.Windows.Forms.Label();
            this.txtIdentBien = new System.Windows.Forms.TextBox();
            this.lblIdentBien = new System.Windows.Forms.Label();
            this.cmbEstadoCuadre = new System.Windows.Forms.ComboBox();
            this.lblEstadoCuadre = new System.Windows.Forms.Label();
            this.cmbFactModifica = new System.Windows.Forms.ComboBox();
            this.lblFactModifica = new System.Windows.Forms.Label();
            this.txtMaskFechaPresentacionHasta = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaPresentacionHasta = new System.Windows.Forms.Label();
            this.txtMaskFechaPresentacionDesde = new System.Windows.Forms.MaskedTextBox();
            this.lblFechaPresentacionDesde = new System.Windows.Forms.Label();
            this.txtMaskFechaExpedicion = new System.Windows.Forms.MaskedTextBox();
            this.gbEstadoFacturas = new System.Windows.Forms.GroupBox();
            this.rbEstadoAnulada = new System.Windows.Forms.RadioButton();
            this.rbEstadoAceptadoErrores = new System.Windows.Forms.RadioButton();
            this.rbEstadoCorrecto = new System.Windows.Forms.RadioButton();
            this.rbEstadoTodas = new System.Windows.Forms.RadioButton();
            this.lblEstadoFactura = new System.Windows.Forms.Label();
            this.lblFechaExpedicion = new System.Windows.Forms.Label();
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
            this.lblCompania = new System.Windows.Forms.Label();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.lblLibro = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.gbResultado = new System.Windows.Forms.GroupBox();
            this.lblCuotaDeducibleTotalValor = new System.Windows.Forms.Label();
            this.lblCuotaTotal = new System.Windows.Forms.Label();
            this.lblBaseImponibleTotalValor = new System.Windows.Forms.Label();
            this.lblBaseImponibleTotal = new System.Windows.Forms.Label();
            this.lblImporteTotalValor = new System.Windows.Forms.Label();
            this.lblImporteTotal = new System.Windows.Forms.Label();
            this.lblTotalRegValor = new System.Windows.Forms.Label();
            this.lblTotalReg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNoInfo = new System.Windows.Forms.Label();
            this.tgGridConsulta = new ObjectModel.TGGrid();
            this.toolStrip1.SuspendLayout();
            this.gbBuscador.SuspendLayout();
            this.gbEstadoFacturas.SuspendLayout();
            this.gbTipoIdentificacion.SuspendLayout();
            this.gbResultado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(468, 315);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 89;
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
            this.gbBuscador.Controls.Add(this.cmbPais);
            this.gbBuscador.Controls.Add(this.btnLimpiar);
            this.gbBuscador.Controls.Add(this.cmbClaveOperacion);
            this.gbBuscador.Controls.Add(this.lblClaveOperacion);
            this.gbBuscador.Controls.Add(this.txtIdentBien);
            this.gbBuscador.Controls.Add(this.lblIdentBien);
            this.gbBuscador.Controls.Add(this.cmbEstadoCuadre);
            this.gbBuscador.Controls.Add(this.lblEstadoCuadre);
            this.gbBuscador.Controls.Add(this.cmbFactModifica);
            this.gbBuscador.Controls.Add(this.lblFactModifica);
            this.gbBuscador.Controls.Add(this.txtMaskFechaPresentacionHasta);
            this.gbBuscador.Controls.Add(this.lblFechaPresentacionHasta);
            this.gbBuscador.Controls.Add(this.txtMaskFechaPresentacionDesde);
            this.gbBuscador.Controls.Add(this.lblFechaPresentacionDesde);
            this.gbBuscador.Controls.Add(this.txtMaskFechaExpedicion);
            this.gbBuscador.Controls.Add(this.gbEstadoFacturas);
            this.gbBuscador.Controls.Add(this.lblEstadoFactura);
            this.gbBuscador.Controls.Add(this.lblFechaExpedicion);
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
            this.gbBuscador.Controls.Add(this.lblCompania);
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
            this.gbBuscador.Size = new System.Drawing.Size(1083, 255);
            this.gbBuscador.TabIndex = 5;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // cmbPais
            // 
            this.cmbPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPais.Enabled = false;
            this.cmbPais.FormattingEnabled = true;
            this.cmbPais.Location = new System.Drawing.Point(573, 81);
            this.cmbPais.Name = "cmbPais";
            this.cmbPais.Size = new System.Drawing.Size(199, 21);
            this.cmbPais.TabIndex = 101;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLimpiar.Location = new System.Drawing.Point(568, 222);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(80, 21);
            this.btnLimpiar.TabIndex = 100;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cmbClaveOperacion
            // 
            this.cmbClaveOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClaveOperacion.FormattingEnabled = true;
            this.cmbClaveOperacion.Location = new System.Drawing.Point(876, 210);
            this.cmbClaveOperacion.MaxLength = 2;
            this.cmbClaveOperacion.Name = "cmbClaveOperacion";
            this.cmbClaveOperacion.Size = new System.Drawing.Size(166, 21);
            this.cmbClaveOperacion.TabIndex = 90;
            this.cmbClaveOperacion.Visible = false;
            // 
            // lblClaveOperacion
            // 
            this.lblClaveOperacion.AutoSize = true;
            this.lblClaveOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblClaveOperacion.Location = new System.Drawing.Point(783, 213);
            this.lblClaveOperacion.Name = "lblClaveOperacion";
            this.lblClaveOperacion.Size = new System.Drawing.Size(86, 13);
            this.lblClaveOperacion.TabIndex = 82;
            this.lblClaveOperacion.Text = "Clave Operación";
            this.lblClaveOperacion.Visible = false;
            // 
            // txtIdentBien
            // 
            this.txtIdentBien.Location = new System.Drawing.Point(876, 186);
            this.txtIdentBien.MaxLength = 40;
            this.txtIdentBien.Name = "txtIdentBien";
            this.txtIdentBien.Size = new System.Drawing.Size(166, 20);
            this.txtIdentBien.TabIndex = 85;
            // 
            // lblIdentBien
            // 
            this.lblIdentBien.AutoSize = true;
            this.lblIdentBien.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblIdentBien.Location = new System.Drawing.Point(775, 189);
            this.lblIdentBien.Name = "lblIdentBien";
            this.lblIdentBien.Size = new System.Drawing.Size(94, 13);
            this.lblIdentBien.TabIndex = 81;
            this.lblIdentBien.Text = "Identificación Bien";
            // 
            // cmbEstadoCuadre
            // 
            this.cmbEstadoCuadre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoCuadre.FormattingEnabled = true;
            this.cmbEstadoCuadre.Location = new System.Drawing.Point(876, 162);
            this.cmbEstadoCuadre.MaxLength = 2;
            this.cmbEstadoCuadre.Name = "cmbEstadoCuadre";
            this.cmbEstadoCuadre.Size = new System.Drawing.Size(166, 21);
            this.cmbEstadoCuadre.TabIndex = 75;
            // 
            // lblEstadoCuadre
            // 
            this.lblEstadoCuadre.AutoSize = true;
            this.lblEstadoCuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEstadoCuadre.Location = new System.Drawing.Point(792, 165);
            this.lblEstadoCuadre.Name = "lblEstadoCuadre";
            this.lblEstadoCuadre.Size = new System.Drawing.Size(77, 13);
            this.lblEstadoCuadre.TabIndex = 78;
            this.lblEstadoCuadre.Text = "Estado Cuadre";
            // 
            // cmbFactModifica
            // 
            this.cmbFactModifica.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFactModifica.FormattingEnabled = true;
            this.cmbFactModifica.Location = new System.Drawing.Point(876, 134);
            this.cmbFactModifica.MaxLength = 2;
            this.cmbFactModifica.Name = "cmbFactModifica";
            this.cmbFactModifica.Size = new System.Drawing.Size(53, 21);
            this.cmbFactModifica.TabIndex = 60;
            // 
            // lblFactModifica
            // 
            this.lblFactModifica.AutoSize = true;
            this.lblFactModifica.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFactModifica.Location = new System.Drawing.Point(771, 137);
            this.lblFactModifica.Name = "lblFactModifica";
            this.lblFactModifica.Size = new System.Drawing.Size(98, 13);
            this.lblFactModifica.TabIndex = 76;
            this.lblFactModifica.Text = "Factura Modificada";
            // 
            // txtMaskFechaPresentacionHasta
            // 
            this.txtMaskFechaPresentacionHasta.Location = new System.Drawing.Point(574, 162);
            this.txtMaskFechaPresentacionHasta.Mask = "00-00-0000";
            this.txtMaskFechaPresentacionHasta.Name = "txtMaskFechaPresentacionHasta";
            this.txtMaskFechaPresentacionHasta.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaPresentacionHasta.TabIndex = 70;
            // 
            // lblFechaPresentacionHasta
            // 
            this.lblFechaPresentacionHasta.AutoSize = true;
            this.lblFechaPresentacionHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaPresentacionHasta.Location = new System.Drawing.Point(432, 165);
            this.lblFechaPresentacionHasta.Name = "lblFechaPresentacionHasta";
            this.lblFechaPresentacionHasta.Size = new System.Drawing.Size(133, 13);
            this.lblFechaPresentacionHasta.TabIndex = 74;
            this.lblFechaPresentacionHasta.Text = "Fecha Presentación Hasta";
            // 
            // txtMaskFechaPresentacionDesde
            // 
            this.txtMaskFechaPresentacionDesde.Location = new System.Drawing.Point(165, 162);
            this.txtMaskFechaPresentacionDesde.Mask = "00-00-0000";
            this.txtMaskFechaPresentacionDesde.Name = "txtMaskFechaPresentacionDesde";
            this.txtMaskFechaPresentacionDesde.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaPresentacionDesde.TabIndex = 65;
            // 
            // lblFechaPresentacionDesde
            // 
            this.lblFechaPresentacionDesde.AutoSize = true;
            this.lblFechaPresentacionDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaPresentacionDesde.Location = new System.Drawing.Point(25, 165);
            this.lblFechaPresentacionDesde.Name = "lblFechaPresentacionDesde";
            this.lblFechaPresentacionDesde.Size = new System.Drawing.Size(136, 13);
            this.lblFechaPresentacionDesde.TabIndex = 72;
            this.lblFechaPresentacionDesde.Text = "Fecha Presentación Desde";
            // 
            // txtMaskFechaExpedicion
            // 
            this.txtMaskFechaExpedicion.Location = new System.Drawing.Point(573, 134);
            this.txtMaskFechaExpedicion.Mask = "00-00-0000";
            this.txtMaskFechaExpedicion.Name = "txtMaskFechaExpedicion";
            this.txtMaskFechaExpedicion.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaExpedicion.TabIndex = 55;
            // 
            // gbEstadoFacturas
            // 
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoAnulada);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoAceptadoErrores);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoCorrecto);
            this.gbEstadoFacturas.Controls.Add(this.rbEstadoTodas);
            this.gbEstadoFacturas.Location = new System.Drawing.Point(164, 184);
            this.gbEstadoFacturas.Name = "gbEstadoFacturas";
            this.gbEstadoFacturas.Size = new System.Drawing.Size(400, 31);
            this.gbEstadoFacturas.TabIndex = 80;
            this.gbEstadoFacturas.TabStop = false;
            // 
            // rbEstadoAnulada
            // 
            this.rbEstadoAnulada.AutoSize = true;
            this.rbEstadoAnulada.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoAnulada.Location = new System.Drawing.Point(325, 10);
            this.rbEstadoAnulada.Name = "rbEstadoAnulada";
            this.rbEstadoAnulada.Size = new System.Drawing.Size(64, 17);
            this.rbEstadoAnulada.TabIndex = 4;
            this.rbEstadoAnulada.Text = "Anulada";
            this.rbEstadoAnulada.UseVisualStyleBackColor = true;
            // 
            // rbEstadoAceptadoErrores
            // 
            this.rbEstadoAceptadoErrores.AutoSize = true;
            this.rbEstadoAceptadoErrores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoAceptadoErrores.Location = new System.Drawing.Point(176, 10);
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
            this.rbEstadoCorrecto.Location = new System.Drawing.Point(89, 10);
            this.rbEstadoCorrecto.Name = "rbEstadoCorrecto";
            this.rbEstadoCorrecto.Size = new System.Drawing.Size(65, 17);
            this.rbEstadoCorrecto.TabIndex = 2;
            this.rbEstadoCorrecto.Text = "Correcta";
            this.rbEstadoCorrecto.UseVisualStyleBackColor = true;
            // 
            // rbEstadoTodas
            // 
            this.rbEstadoTodas.AutoSize = true;
            this.rbEstadoTodas.Checked = true;
            this.rbEstadoTodas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rbEstadoTodas.Location = new System.Drawing.Point(17, 10);
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
            this.lblEstadoFactura.Location = new System.Drawing.Point(25, 195);
            this.lblEstadoFactura.Name = "lblEstadoFactura";
            this.lblEstadoFactura.Size = new System.Drawing.Size(84, 13);
            this.lblEstadoFactura.TabIndex = 67;
            this.lblEstadoFactura.Text = "Estado Facturas";
            // 
            // lblFechaExpedicion
            // 
            this.lblFechaExpedicion.AutoSize = true;
            this.lblFechaExpedicion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaExpedicion.Location = new System.Drawing.Point(473, 139);
            this.lblFechaExpedicion.Name = "lblFechaExpedicion";
            this.lblFechaExpedicion.Size = new System.Drawing.Size(92, 13);
            this.lblFechaExpedicion.TabIndex = 65;
            this.lblFechaExpedicion.Text = "Fecha Expedición";
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
            this.lblNumSerieFactura.Location = new System.Drawing.Point(25, 139);
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
            this.cmbTipoIdentif.Location = new System.Drawing.Point(876, 81);
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
            this.lblNombreRazon.Location = new System.Drawing.Point(418, 112);
            this.lblNombreRazon.Name = "lblNombreRazon";
            this.lblNombreRazon.Size = new System.Drawing.Size(147, 13);
            this.lblNombreRazon.TabIndex = 60;
            this.lblNombreRazon.Text = "Nombre o Razón Social Dest.";
            // 
            // lblTipoIden
            // 
            this.lblTipoIden.AutoSize = true;
            this.lblTipoIden.Enabled = false;
            this.lblTipoIden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTipoIden.Location = new System.Drawing.Point(778, 86);
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
            this.lblCodPais.Location = new System.Drawing.Point(454, 86);
            this.lblCodPais.Name = "lblCodPais";
            this.lblCodPais.Size = new System.Drawing.Size(111, 13);
            this.lblCodPais.TabIndex = 56;
            this.lblCodPais.Text = "Cod. Pais Destinatario";
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
            this.rbOtro.Location = new System.Drawing.Point(78, 10);
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
            this.rbNIF.Location = new System.Drawing.Point(15, 10);
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
            this.lblTipoIdentificacion.Location = new System.Drawing.Point(25, 83);
            this.lblTipoIdentificacion.Name = "lblTipoIdentificacion";
            this.lblTipoIdentificacion.Size = new System.Drawing.Size(119, 13);
            this.lblTipoIdentificacion.TabIndex = 52;
            this.lblTipoIdentificacion.Text = "Tipo de Identificación";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Location = new System.Drawing.Point(573, 50);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 15;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            // 
            // lblCompania
            // 
            this.lblCompania.AutoSize = true;
            this.lblCompania.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCompania.Location = new System.Drawing.Point(24, 53);
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
            this.cmbPeriodo.Location = new System.Drawing.Point(876, 49);
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
            this.lblPeriodo.Location = new System.Drawing.Point(826, 53);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 30;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(518, 53);
            this.lblEjercicio.Name = "lblEjercicio";
            this.lblEjercicio.Size = new System.Drawing.Size(47, 13);
            this.lblEjercicio.TabIndex = 20;
            this.lblEjercicio.Text = "Ejercicio";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(449, 222);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 95;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // gbResultado
            // 
            this.gbResultado.Controls.Add(this.lblCuotaDeducibleTotalValor);
            this.gbResultado.Controls.Add(this.lblCuotaTotal);
            this.gbResultado.Controls.Add(this.lblBaseImponibleTotalValor);
            this.gbResultado.Controls.Add(this.lblBaseImponibleTotal);
            this.gbResultado.Controls.Add(this.lblImporteTotalValor);
            this.gbResultado.Controls.Add(this.lblImporteTotal);
            this.gbResultado.Controls.Add(this.lblTotalRegValor);
            this.gbResultado.Controls.Add(this.lblTotalReg);
            this.gbResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbResultado.Location = new System.Drawing.Point(33, 299);
            this.gbResultado.Name = "gbResultado";
            this.gbResultado.Size = new System.Drawing.Size(1083, 39);
            this.gbResultado.TabIndex = 88;
            this.gbResultado.TabStop = false;
            this.gbResultado.Text = " Resultado ";
            this.gbResultado.Visible = false;
            // 
            // lblCuotaDeducibleTotalValor
            // 
            this.lblCuotaDeducibleTotalValor.AutoSize = true;
            this.lblCuotaDeducibleTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.lblCuotaDeducibleTotalValor.Location = new System.Drawing.Point(884, 17);
            this.lblCuotaDeducibleTotalValor.Name = "lblCuotaDeducibleTotalValor";
            this.lblCuotaDeducibleTotalValor.Size = new System.Drawing.Size(125, 13);
            this.lblCuotaDeducibleTotalValor.TabIndex = 19;
            this.lblCuotaDeducibleTotalValor.Text = "valor cuota ded total";
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
            this.lblBaseImponibleTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
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
            this.lblImporteTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
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
            this.lblTotalRegValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
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
            this.panel1.Location = new System.Drawing.Point(44, 728);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 90;
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNoInfo.Location = new System.Drawing.Point(57, 341);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(273, 13);
            this.lblNoInfo.TabIndex = 83;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridConsulta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridConsulta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridConsulta.ComboValores = null;
            this.tgGridConsulta.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridConsulta.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridConsulta.DSDatos = null;
            this.tgGridConsulta.Location = new System.Drawing.Point(33, 344);
            this.tgGridConsulta.Name = "tgGridConsulta";
            this.tgGridConsulta.NombreTabla = "";
            this.tgGridConsulta.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridConsulta.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridConsulta.RowNumber = false;
            this.tgGridConsulta.Size = new System.Drawing.Size(1083, 378);
            this.tgGridConsulta.TabIndex = 55;
            this.tgGridConsulta.Visible = false;
            this.tgGridConsulta.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridConsulta_CellContentClick);
            this.tgGridConsulta.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridConsulta_CellDoubleClick);
            this.tgGridConsulta.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tgGridConsulta_CellFormatting);
            this.tgGridConsulta.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridConsulta_DataBindingComplete);
            // 
            // frmSiiConsultaInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 749);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tgGridConsulta);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.gbResultado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiConsultaInfo";
            this.Text = "Consulta Datos Hacienda";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiConsultaInfo_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiConsultaInfo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiConsultaInfo_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbEstadoFacturas.ResumeLayout(false);
            this.gbEstadoFacturas.PerformLayout();
            this.gbTipoIdentificacion.ResumeLayout(false);
            this.gbTipoIdentificacion.PerformLayout();
            this.gbResultado.ResumeLayout(false);
            this.gbResultado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.Label lblCompania;
        private ObjectModel.TGGrid tgGridConsulta;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.Label lblTipoIdentificacion;
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
        private System.Windows.Forms.TextBox txtNumSerieFactura;
        private System.Windows.Forms.Label lblNumSerieFactura;
        private System.Windows.Forms.Label lblFechaExpedicion;
        private System.Windows.Forms.GroupBox gbEstadoFacturas;
        private System.Windows.Forms.RadioButton rbEstadoAceptadoErrores;
        private System.Windows.Forms.RadioButton rbEstadoCorrecto;
        private System.Windows.Forms.RadioButton rbEstadoTodas;
        private System.Windows.Forms.Label lblEstadoFactura;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaExpedicion;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaPresentacionHasta;
        private System.Windows.Forms.Label lblFechaPresentacionHasta;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaPresentacionDesde;
        private System.Windows.Forms.Label lblFechaPresentacionDesde;
        private System.Windows.Forms.TextBox txtIdentBien;
        private System.Windows.Forms.Label lblIdentBien;
        private System.Windows.Forms.ComboBox cmbEstadoCuadre;
        private System.Windows.Forms.Label lblEstadoCuadre;
        private System.Windows.Forms.ComboBox cmbFactModifica;
        private System.Windows.Forms.Label lblFactModifica;
        private System.Windows.Forms.ComboBox cmbClaveOperacion;
        private System.Windows.Forms.Label lblClaveOperacion;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.GroupBox gbResultado;
        private System.Windows.Forms.Label lblCuotaDeducibleTotalValor;
        private System.Windows.Forms.Label lblCuotaTotal;
        private System.Windows.Forms.Label lblBaseImponibleTotalValor;
        private System.Windows.Forms.Label lblBaseImponibleTotal;
        private System.Windows.Forms.Label lblImporteTotalValor;
        private System.Windows.Forms.Label lblImporteTotal;
        private System.Windows.Forms.Label lblTotalRegValor;
        private System.Windows.Forms.Label lblTotalReg;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.RadioButton rbEstadoAnulada;
        private System.Windows.Forms.ComboBox cmbPais;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNoInfo;
    }
}