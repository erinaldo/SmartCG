namespace ModSII
{
    partial class frmSiiLocalDescuadre
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiLocalDescuadre));
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblResultInfo = new System.Windows.Forms.Label();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.cmbPais = new System.Windows.Forms.ComboBox();
            this.txtNombreRazonSocial = new System.Windows.Forms.TextBox();
            this.lblNombreRazonSocial = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.txtMaskFechaExpedicion = new System.Windows.Forms.MaskedTextBox();
            this.txtNumSerieFactura = new System.Windows.Forms.TextBox();
            this.chkSoloDescuadre = new System.Windows.Forms.CheckBox();
            this.lblNumSerieFactura = new System.Windows.Forms.Label();
            this.cmbTipoIdentif = new System.Windows.Forms.ComboBox();
            this.lblTipoIden = new System.Windows.Forms.Label();
            this.lblCodPais = new System.Windows.Forms.Label();
            this.txtNIF = new System.Windows.Forms.TextBox();
            this.lblNIF = new System.Windows.Forms.Label();
            this.btnDescuadre = new System.Windows.Forms.Button();
            this.lblFechaExpedicion = new System.Windows.Forms.Label();
            this.gbTipoIdentificacion = new System.Windows.Forms.GroupBox();
            this.rbOtro = new System.Windows.Forms.RadioButton();
            this.rbNIF = new System.Windows.Forms.RadioButton();
            this.lblTipoIdentificacion = new System.Windows.Forms.Label();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.lblLibro = new System.Windows.Forms.Label();
            this.gbFacturasTodas = new System.Windows.Forms.GroupBox();
            this.lblResultInfoLocal = new System.Windows.Forms.Label();
            this.lblResultInfoAEAT = new System.Windows.Forms.Label();
            this.tgGridAEAT = new ObjectModel.TGGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalFactLocal = new System.Windows.Forms.Label();
            this.tgGridLocal = new ObjectModel.TGGrid();
            this.lblTotalFactAEAT = new System.Windows.Forms.Label();
            this.lblTotalDescuadre = new System.Windows.Forms.Label();
            this.tgGridDiff = new ObjectModel.TGGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonActualizarEstado = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbBuscador.SuspendLayout();
            this.gbTipoIdentificacion.SuspendLayout();
            this.gbFacturasTodas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridAEAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridLocal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridDiff)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(411, 232);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 105;
            this.lblInfo.Text = "Su petición se está procesando, espere por favor.";
            this.lblInfo.Visible = false;
            // 
            // lblResultInfo
            // 
            this.lblResultInfo.AutoSize = true;
            this.lblResultInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblResultInfo.Location = new System.Drawing.Point(29, 232);
            this.lblResultInfo.Name = "lblResultInfo";
            this.lblResultInfo.Size = new System.Drawing.Size(329, 13);
            this.lblResultInfo.TabIndex = 145;
            this.lblResultInfo.Text = "No existen facturas a comparar que cumplan el criterio seleccionado";
            this.lblResultInfo.Visible = false;
            // 
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.cmbPais);
            this.gbBuscador.Controls.Add(this.txtNombreRazonSocial);
            this.gbBuscador.Controls.Add(this.lblNombreRazonSocial);
            this.gbBuscador.Controls.Add(this.btnLimpiar);
            this.gbBuscador.Controls.Add(this.txtMaskFechaExpedicion);
            this.gbBuscador.Controls.Add(this.txtNumSerieFactura);
            this.gbBuscador.Controls.Add(this.chkSoloDescuadre);
            this.gbBuscador.Controls.Add(this.lblNumSerieFactura);
            this.gbBuscador.Controls.Add(this.cmbTipoIdentif);
            this.gbBuscador.Controls.Add(this.lblTipoIden);
            this.gbBuscador.Controls.Add(this.lblCodPais);
            this.gbBuscador.Controls.Add(this.txtNIF);
            this.gbBuscador.Controls.Add(this.lblNIF);
            this.gbBuscador.Controls.Add(this.btnDescuadre);
            this.gbBuscador.Controls.Add(this.lblFechaExpedicion);
            this.gbBuscador.Controls.Add(this.gbTipoIdentificacion);
            this.gbBuscador.Controls.Add(this.lblTipoIdentificacion);
            this.gbBuscador.Controls.Add(this.txtEjercicio);
            this.gbBuscador.Controls.Add(this.label6);
            this.gbBuscador.Controls.Add(this.tgTexBoxSelCiaFiscal);
            this.gbBuscador.Controls.Add(this.cmbPeriodo);
            this.gbBuscador.Controls.Add(this.cmbLibro);
            this.gbBuscador.Controls.Add(this.lblPeriodo);
            this.gbBuscador.Controls.Add(this.lblEjercicio);
            this.gbBuscador.Controls.Add(this.lblLibro);
            this.gbBuscador.Location = new System.Drawing.Point(21, 32);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(1055, 180);
            this.gbBuscador.TabIndex = 5;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // cmbPais
            // 
            this.cmbPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPais.Enabled = false;
            this.cmbPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbPais.FormattingEnabled = true;
            this.cmbPais.Location = new System.Drawing.Point(530, 78);
            this.cmbPais.Name = "cmbPais";
            this.cmbPais.Size = new System.Drawing.Size(199, 21);
            this.cmbPais.TabIndex = 70;
            // 
            // txtNombreRazonSocial
            // 
            this.txtNombreRazonSocial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtNombreRazonSocial.Location = new System.Drawing.Point(529, 105);
            this.txtNombreRazonSocial.MaxLength = 20;
            this.txtNombreRazonSocial.Name = "txtNombreRazonSocial";
            this.txtNombreRazonSocial.Size = new System.Drawing.Size(490, 20);
            this.txtNombreRazonSocial.TabIndex = 100;
            // 
            // lblNombreRazonSocial
            // 
            this.lblNombreRazonSocial.AutoSize = true;
            this.lblNombreRazonSocial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNombreRazonSocial.Location = new System.Drawing.Point(376, 110);
            this.lblNombreRazonSocial.Name = "lblNombreRazonSocial";
            this.lblNombreRazonSocial.Size = new System.Drawing.Size(147, 13);
            this.lblNombreRazonSocial.TabIndex = 95;
            this.lblNombreRazonSocial.Text = "Nombre o Razón Social Dest.";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLimpiar.Location = new System.Drawing.Point(585, 154);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(80, 21);
            this.btnLimpiar.TabIndex = 130;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // txtMaskFechaExpedicion
            // 
            this.txtMaskFechaExpedicion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtMaskFechaExpedicion.Location = new System.Drawing.Point(529, 128);
            this.txtMaskFechaExpedicion.Mask = "00-00-0000";
            this.txtMaskFechaExpedicion.Name = "txtMaskFechaExpedicion";
            this.txtMaskFechaExpedicion.Size = new System.Drawing.Size(64, 20);
            this.txtMaskFechaExpedicion.TabIndex = 120;
            // 
            // txtNumSerieFactura
            // 
            this.txtNumSerieFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtNumSerieFactura.Location = new System.Drawing.Point(146, 127);
            this.txtNumSerieFactura.MaxLength = 45;
            this.txtNumSerieFactura.Name = "txtNumSerieFactura";
            this.txtNumSerieFactura.Size = new System.Drawing.Size(198, 20);
            this.txtNumSerieFactura.TabIndex = 110;
            // 
            // chkSoloDescuadre
            // 
            this.chkSoloDescuadre.AutoSize = true;
            this.chkSoloDescuadre.Checked = true;
            this.chkSoloDescuadre.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSoloDescuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chkSoloDescuadre.Location = new System.Drawing.Point(529, 14);
            this.chkSoloDescuadre.Name = "chkSoloDescuadre";
            this.chkSoloDescuadre.Size = new System.Drawing.Size(103, 17);
            this.chkSoloDescuadre.TabIndex = 20;
            this.chkSoloDescuadre.Text = "Sólo Diferencias";
            this.chkSoloDescuadre.UseVisualStyleBackColor = true;
            // 
            // lblNumSerieFactura
            // 
            this.lblNumSerieFactura.AutoSize = true;
            this.lblNumSerieFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNumSerieFactura.Location = new System.Drawing.Point(8, 131);
            this.lblNumSerieFactura.Name = "lblNumSerieFactura";
            this.lblNumSerieFactura.Size = new System.Drawing.Size(110, 13);
            this.lblNumSerieFactura.TabIndex = 105;
            this.lblNumSerieFactura.Text = "Número Serie Factura";
            // 
            // cmbTipoIdentif
            // 
            this.cmbTipoIdentif.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoIdentif.Enabled = false;
            this.cmbTipoIdentif.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbTipoIdentif.FormattingEnabled = true;
            this.cmbTipoIdentif.Location = new System.Drawing.Point(847, 78);
            this.cmbTipoIdentif.Name = "cmbTipoIdentif";
            this.cmbTipoIdentif.Size = new System.Drawing.Size(199, 21);
            this.cmbTipoIdentif.TabIndex = 80;
            // 
            // lblTipoIden
            // 
            this.lblTipoIden.AutoSize = true;
            this.lblTipoIden.Enabled = false;
            this.lblTipoIden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTipoIden.Location = new System.Drawing.Point(750, 82);
            this.lblTipoIden.Name = "lblTipoIden";
            this.lblTipoIden.Size = new System.Drawing.Size(91, 13);
            this.lblTipoIden.TabIndex = 75;
            this.lblTipoIden.Text = "Tipo Identif. Dest.";
            // 
            // lblCodPais
            // 
            this.lblCodPais.AutoSize = true;
            this.lblCodPais.Enabled = false;
            this.lblCodPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCodPais.Location = new System.Drawing.Point(411, 82);
            this.lblCodPais.Name = "lblCodPais";
            this.lblCodPais.Size = new System.Drawing.Size(111, 13);
            this.lblCodPais.TabIndex = 65;
            this.lblCodPais.Text = "Cod. Pais Destinatario";
            // 
            // txtNIF
            // 
            this.txtNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtNIF.Location = new System.Drawing.Point(147, 103);
            this.txtNIF.MaxLength = 20;
            this.txtNIF.Name = "txtNIF";
            this.txtNIF.Size = new System.Drawing.Size(143, 20);
            this.txtNIF.TabIndex = 90;
            // 
            // lblNIF
            // 
            this.lblNIF.AutoSize = true;
            this.lblNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNIF.Location = new System.Drawing.Point(8, 108);
            this.lblNIF.Name = "lblNIF";
            this.lblNIF.Size = new System.Drawing.Size(83, 13);
            this.lblNIF.TabIndex = 85;
            this.lblNIF.Text = "NIF Destinatario";
            // 
            // btnDescuadre
            // 
            this.btnDescuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnDescuadre.Location = new System.Drawing.Point(447, 154);
            this.btnDescuadre.Name = "btnDescuadre";
            this.btnDescuadre.Size = new System.Drawing.Size(75, 23);
            this.btnDescuadre.TabIndex = 125;
            this.btnDescuadre.Text = "Analizar";
            this.btnDescuadre.UseVisualStyleBackColor = true;
            this.btnDescuadre.Click += new System.EventHandler(this.btnDescuadre_Click);
            // 
            // lblFechaExpedicion
            // 
            this.lblFechaExpedicion.AutoSize = true;
            this.lblFechaExpedicion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblFechaExpedicion.Location = new System.Drawing.Point(431, 131);
            this.lblFechaExpedicion.Name = "lblFechaExpedicion";
            this.lblFechaExpedicion.Size = new System.Drawing.Size(92, 13);
            this.lblFechaExpedicion.TabIndex = 115;
            this.lblFechaExpedicion.Text = "Fecha Expedición";
            // 
            // gbTipoIdentificacion
            // 
            this.gbTipoIdentificacion.Controls.Add(this.rbOtro);
            this.gbTipoIdentificacion.Controls.Add(this.rbNIF);
            this.gbTipoIdentificacion.Location = new System.Drawing.Point(146, 68);
            this.gbTipoIdentificacion.Name = "gbTipoIdentificacion";
            this.gbTipoIdentificacion.Size = new System.Drawing.Size(142, 31);
            this.gbTipoIdentificacion.TabIndex = 60;
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
            this.lblTipoIdentificacion.Location = new System.Drawing.Point(8, 82);
            this.lblTipoIdentificacion.Name = "lblTipoIdentificacion";
            this.lblTipoIdentificacion.Size = new System.Drawing.Size(119, 13);
            this.lblTipoIdentificacion.TabIndex = 55;
            this.lblTipoIdentificacion.Text = "Tipo de Identificación";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtEjercicio.Location = new System.Drawing.Point(530, 43);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 40;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label6.Location = new System.Drawing.Point(8, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Compañía Fiscal";
            // 
            // tgTexBoxSelCiaFiscal
            // 
            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 0;
            this.tgTexBoxSelCiaFiscal.CentrarFormSel = true;
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = null;
            this.tgTexBoxSelCiaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.tgTexBoxSelCiaFiscal.FrmPadre = null;
            this.tgTexBoxSelCiaFiscal.Location = new System.Drawing.Point(146, 38);
            this.tgTexBoxSelCiaFiscal.LocationFormSel = new System.Drawing.Point(0, 0);
            this.tgTexBoxSelCiaFiscal.Name = "tgTexBoxSelCiaFiscal";
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 0;
            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = null;
            this.tgTexBoxSelCiaFiscal.QueryFormSel = null;
            this.tgTexBoxSelCiaFiscal.SeparadorCampos = "-";
            this.tgTexBoxSelCiaFiscal.Size = new System.Drawing.Size(176, 30);
            this.tgTexBoxSelCiaFiscal.TabIndex = 30;
            this.tgTexBoxSelCiaFiscal.TituloFormSel = null;
            this.tgTexBoxSelCiaFiscal.TodoMayuscula = true;
            this.tgTexBoxSelCiaFiscal.Enter += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Enter);
            this.tgTexBoxSelCiaFiscal.Leave += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Leave);
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(847, 43);
            this.cmbPeriodo.MaxLength = 2;
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(53, 21);
            this.cmbPeriodo.TabIndex = 50;
            this.cmbPeriodo.Leave += new System.EventHandler(this.cmbPeriodo_Leave);
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbLibro.FormattingEnabled = true;
            this.cmbLibro.Location = new System.Drawing.Point(147, 11);
            this.cmbLibro.Name = "cmbLibro";
            this.cmbLibro.Size = new System.Drawing.Size(199, 21);
            this.cmbLibro.TabIndex = 15;
            this.cmbLibro.SelectedIndexChanged += new System.EventHandler(this.cmbLibro_SelectedIndexChanged);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblPeriodo.Location = new System.Drawing.Point(798, 46);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 45;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(475, 46);
            this.lblEjercicio.Name = "lblEjercicio";
            this.lblEjercicio.Size = new System.Drawing.Size(47, 13);
            this.lblEjercicio.TabIndex = 35;
            this.lblEjercicio.Text = "Ejercicio";
            // 
            // lblLibro
            // 
            this.lblLibro.AutoSize = true;
            this.lblLibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblLibro.Location = new System.Drawing.Point(8, 18);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(30, 13);
            this.lblLibro.TabIndex = 10;
            this.lblLibro.Text = "Libro";
            // 
            // gbFacturasTodas
            // 
            this.gbFacturasTodas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFacturasTodas.Controls.Add(this.lblResultInfoLocal);
            this.gbFacturasTodas.Controls.Add(this.lblResultInfoAEAT);
            this.gbFacturasTodas.Controls.Add(this.tgGridAEAT);
            this.gbFacturasTodas.Controls.Add(this.label1);
            this.gbFacturasTodas.Controls.Add(this.label2);
            this.gbFacturasTodas.Controls.Add(this.lblTotalFactLocal);
            this.gbFacturasTodas.Controls.Add(this.tgGridLocal);
            this.gbFacturasTodas.Controls.Add(this.lblTotalFactAEAT);
            this.gbFacturasTodas.Location = new System.Drawing.Point(21, 536);
            this.gbFacturasTodas.Name = "gbFacturasTodas";
            this.gbFacturasTodas.Size = new System.Drawing.Size(1055, 173);
            this.gbFacturasTodas.TabIndex = 104;
            this.gbFacturasTodas.TabStop = false;
            this.gbFacturasTodas.Visible = false;
            // 
            // lblResultInfoLocal
            // 
            this.lblResultInfoLocal.AutoSize = true;
            this.lblResultInfoLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblResultInfoLocal.Location = new System.Drawing.Point(535, 25);
            this.lblResultInfoLocal.Name = "lblResultInfoLocal";
            this.lblResultInfoLocal.Size = new System.Drawing.Size(313, 13);
            this.lblResultInfoLocal.TabIndex = 190;
            this.lblResultInfoLocal.Text = "No existen facturas en local que cumplan el criterio seleccionado";
            // 
            // lblResultInfoAEAT
            // 
            this.lblResultInfoAEAT.AutoSize = true;
            this.lblResultInfoAEAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblResultInfoAEAT.Location = new System.Drawing.Point(8, 24);
            this.lblResultInfoAEAT.Name = "lblResultInfoAEAT";
            this.lblResultInfoAEAT.Size = new System.Drawing.Size(391, 13);
            this.lblResultInfoAEAT.TabIndex = 170;
            this.lblResultInfoAEAT.Text = "No existen facturas presentados en la AEAT que cumplan el criterio seleccionado";
            this.lblResultInfoAEAT.Visible = false;
            // 
            // tgGridAEAT
            // 
            this.tgGridAEAT.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridAEAT.AllowUserToAddRows = false;
            this.tgGridAEAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tgGridAEAT.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridAEAT.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tgGridAEAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridAEAT.ComboValores = null;
            this.tgGridAEAT.ContextMenuStripGrid = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridAEAT.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridAEAT.DSDatos = null;
            this.tgGridAEAT.Location = new System.Drawing.Point(8, 24);
            this.tgGridAEAT.Name = "tgGridAEAT";
            this.tgGridAEAT.NombreTabla = "AEAT";
            this.tgGridAEAT.ReadOnly = true;
            this.tgGridAEAT.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridAEAT.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridAEAT.RowNumber = false;
            this.tgGridAEAT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tgGridAEAT.Size = new System.Drawing.Size(512, 137);
            this.tgGridAEAT.TabIndex = 165;
            this.tgGridAEAT.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridAEAT_CellContentClick);
            this.tgGridAEAT.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridAEAT_CellContentDoubleClick);
            this.tgGridAEAT.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tgGridAEAT_CellFormatting);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 155;
            this.label1.Text = "Facturas AEAT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label2.Location = new System.Drawing.Point(535, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 175;
            this.label2.Text = "Facturas Local";
            // 
            // lblTotalFactLocal
            // 
            this.lblTotalFactLocal.AutoSize = true;
            this.lblTotalFactLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTotalFactLocal.Location = new System.Drawing.Point(630, 8);
            this.lblTotalFactLocal.Name = "lblTotalFactLocal";
            this.lblTotalFactLocal.Size = new System.Drawing.Size(78, 13);
            this.lblTotalFactLocal.TabIndex = 180;
            this.lblTotalFactLocal.Text = "Total Registros";
            this.lblTotalFactLocal.Visible = false;
            // 
            // tgGridLocal
            // 
            this.tgGridLocal.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridLocal.AllowUserToAddRows = false;
            this.tgGridLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridLocal.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridLocal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tgGridLocal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridLocal.ComboValores = null;
            this.tgGridLocal.ContextMenuStripGrid = null;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridLocal.DefaultCellStyle = dataGridViewCellStyle5;
            this.tgGridLocal.DSDatos = null;
            this.tgGridLocal.Location = new System.Drawing.Point(536, 24);
            this.tgGridLocal.Name = "tgGridLocal";
            this.tgGridLocal.NombreTabla = "Local";
            this.tgGridLocal.ReadOnly = true;
            this.tgGridLocal.RowHeaderInitWidth = 41;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridLocal.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tgGridLocal.RowNumber = false;
            this.tgGridLocal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tgGridLocal.Size = new System.Drawing.Size(512, 137);
            this.tgGridLocal.TabIndex = 195;
            this.tgGridLocal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridLocal_CellContentClick);
            this.tgGridLocal.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgGridLocal_CellContentDoubleClick);
            // 
            // lblTotalFactAEAT
            // 
            this.lblTotalFactAEAT.AutoSize = true;
            this.lblTotalFactAEAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTotalFactAEAT.Location = new System.Drawing.Point(110, 8);
            this.lblTotalFactAEAT.Name = "lblTotalFactAEAT";
            this.lblTotalFactAEAT.Size = new System.Drawing.Size(78, 13);
            this.lblTotalFactAEAT.TabIndex = 160;
            this.lblTotalFactAEAT.Text = "Total Registros";
            this.lblTotalFactAEAT.Visible = false;
            // 
            // lblTotalDescuadre
            // 
            this.lblTotalDescuadre.AutoSize = true;
            this.lblTotalDescuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTotalDescuadre.Location = new System.Drawing.Point(29, 215);
            this.lblTotalDescuadre.Name = "lblTotalDescuadre";
            this.lblTotalDescuadre.Size = new System.Drawing.Size(78, 13);
            this.lblTotalDescuadre.TabIndex = 140;
            this.lblTotalDescuadre.Text = "Total Registros";
            this.lblTotalDescuadre.Visible = false;
            // 
            // tgGridDiff
            // 
            this.tgGridDiff.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridDiff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgGridDiff.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridDiff.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.tgGridDiff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tgGridDiff.ComboValores = null;
            this.tgGridDiff.ContextMenuStripGrid = null;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridDiff.DefaultCellStyle = dataGridViewCellStyle8;
            this.tgGridDiff.DSDatos = null;
            this.tgGridDiff.Location = new System.Drawing.Point(29, 230);
            this.tgGridDiff.Name = "tgGridDiff";
            this.tgGridDiff.NombreTabla = "Diff";
            this.tgGridDiff.RowHeaderInitWidth = 41;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridDiff.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.tgGridDiff.RowNumber = false;
            this.tgGridDiff.Size = new System.Drawing.Size(1036, 300);
            this.tgGridDiff.TabIndex = 150;
            this.tgGridDiff.Visible = false;
            this.tgGridDiff.SelectionChanged += new System.EventHandler(this.tgGridDiff_SelectionChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonActualizarEstado,
            this.toolStripButtonExportar,
            this.toolStripSeparator1,
            this.toolStripButtonGrabarPeticion,
            this.toolStripButtonCargarPeticion,
            this.toolStripSeparator2,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1084, 25);
            this.toolStrip1.TabIndex = 87;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonActualizarEstado
            // 
            this.toolStripButtonActualizarEstado.Enabled = false;
            this.toolStripButtonActualizarEstado.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonActualizarEstado.Image")));
            this.toolStripButtonActualizarEstado.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonActualizarEstado.Name = "toolStripButtonActualizarEstado";
            this.toolStripButtonActualizarEstado.Size = new System.Drawing.Size(164, 22);
            this.toolStripButtonActualizarEstado.Text = "Actualizar Estado en Local";
            this.toolStripButtonActualizarEstado.Visible = false;
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
            this.panel1.Location = new System.Drawing.Point(58, 716);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 151;
            // 
            // frmSiiLocalDescuadre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 729);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblResultInfo);
            this.Controls.Add(this.gbBuscador);
            this.Controls.Add(this.gbFacturasTodas);
            this.Controls.Add(this.lblTotalDescuadre);
            this.Controls.Add(this.tgGridDiff);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiLocalDescuadre";
            this.Text = "Comprobación entre Datos presentados y Datos Local";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiLocalDescuadre_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiLocalDescuadre_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiLocalDescuadre_KeyDown);
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.gbTipoIdentificacion.ResumeLayout(false);
            this.gbTipoIdentificacion.PerformLayout();
            this.gbFacturasTodas.ResumeLayout(false);
            this.gbFacturasTodas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridAEAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridLocal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridDiff)).EndInit();
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
        private System.Windows.Forms.Button btnDescuadre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalFactAEAT;
        private System.Windows.Forms.Label lblTotalFactLocal;
        private System.Windows.Forms.Label lblTotalDescuadre;
        private System.Windows.Forms.CheckBox chkSoloDescuadre;
        private System.Windows.Forms.GroupBox gbFacturasTodas;
        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.MaskedTextBox txtMaskFechaExpedicion;
        private System.Windows.Forms.TextBox txtNumSerieFactura;
        private System.Windows.Forms.Label lblNumSerieFactura;
        private System.Windows.Forms.ComboBox cmbTipoIdentif;
        private System.Windows.Forms.Label lblTipoIden;
        private System.Windows.Forms.Label lblCodPais;
        private System.Windows.Forms.TextBox txtNIF;
        private System.Windows.Forms.Label lblNIF;
        private System.Windows.Forms.Label lblFechaExpedicion;
        private System.Windows.Forms.GroupBox gbTipoIdentificacion;
        private System.Windows.Forms.RadioButton rbOtro;
        private System.Windows.Forms.RadioButton rbNIF;
        private System.Windows.Forms.Label lblTipoIdentificacion;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.Label label6;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.Button btnLimpiar;
        private ObjectModel.TGGrid tgGridDiff;
        private ObjectModel.TGGrid tgGridAEAT;
        private ObjectModel.TGGrid tgGridLocal;
        private System.Windows.Forms.ToolStripButton toolStripButtonActualizarEstado;
        private System.Windows.Forms.Label lblResultInfo;
        private System.Windows.Forms.Label lblResultInfoLocal;
        private System.Windows.Forms.Label lblResultInfoAEAT;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtNombreRazonSocial;
        private System.Windows.Forms.Label lblNombreRazonSocial;
        private System.Windows.Forms.ComboBox cmbPais;
        private System.Windows.Forms.Panel panel1;
    }
}