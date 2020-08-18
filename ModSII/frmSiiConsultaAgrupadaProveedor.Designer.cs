namespace ModSII
{
    partial class frmSiiConsultaAgrupadaProveedor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiConsultaAgrupadaProveedor));
            this.lblInfo = new System.Windows.Forms.Label();
            this.tgGridConsulta = new ObjectModel.TGGrid();
            this.gbResultado = new System.Windows.Forms.GroupBox();
            this.lblTotalNumFacturasValor = new System.Windows.Forms.Label();
            this.lblTotalNumFacturas = new System.Windows.Forms.Label();
            this.lblTotalRegValor = new System.Windows.Forms.Label();
            this.lblTotalReg = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonExportar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGrabarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCargarPeticion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.gbBuscador = new System.Windows.Forms.GroupBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cmbEstadoCuadre = new System.Windows.Forms.ComboBox();
            this.lblEstadoCuadre = new System.Windows.Forms.Label();
            this.txtNombreRazon = new System.Windows.Forms.TextBox();
            this.lblNombreRazon = new System.Windows.Forms.Label();
            this.txtNIF = new System.Windows.Forms.TextBox();
            this.lblNIF = new System.Windows.Forms.Label();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.lblCompania = new System.Windows.Forms.Label();
            this.tgTexBoxSelCiaFiscal = new ObjectModel.TGTexBoxSel();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblEjercicio = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNoInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).BeginInit();
            this.gbResultado.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbBuscador.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblInfo.Location = new System.Drawing.Point(467, 211);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 13);
            this.lblInfo.TabIndex = 15;
            this.lblInfo.Text = "Su petición se está procesando, espere por favor.";
            this.lblInfo.Visible = false;
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tgGridConsulta.DefaultCellStyle = dataGridViewCellStyle2;
            this.tgGridConsulta.DSDatos = null;
            this.tgGridConsulta.Location = new System.Drawing.Point(32, 227);
            this.tgGridConsulta.Name = "tgGridConsulta";
            this.tgGridConsulta.NombreTabla = "";
            this.tgGridConsulta.RowHeaderInitWidth = 41;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tgGridConsulta.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tgGridConsulta.RowNumber = false;
            this.tgGridConsulta.Size = new System.Drawing.Size(1083, 442);
            this.tgGridConsulta.TabIndex = 20;
            this.tgGridConsulta.Visible = false;
            this.tgGridConsulta.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tgGridConsulta_DataBindingComplete);
            // 
            // gbResultado
            // 
            this.gbResultado.Controls.Add(this.lblTotalNumFacturasValor);
            this.gbResultado.Controls.Add(this.lblTotalNumFacturas);
            this.gbResultado.Controls.Add(this.lblTotalRegValor);
            this.gbResultado.Controls.Add(this.lblTotalReg);
            this.gbResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbResultado.Location = new System.Drawing.Point(31, 182);
            this.gbResultado.Name = "gbResultado";
            this.gbResultado.Size = new System.Drawing.Size(1083, 39);
            this.gbResultado.TabIndex = 10;
            this.gbResultado.TabStop = false;
            this.gbResultado.Text = " Resultado ";
            this.gbResultado.Visible = false;
            // 
            // lblTotalNumFacturasValor
            // 
            this.lblTotalNumFacturasValor.AutoSize = true;
            this.lblTotalNumFacturasValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.lblTotalNumFacturasValor.Location = new System.Drawing.Point(383, 17);
            this.lblTotalNumFacturasValor.Name = "lblTotalNumFacturasValor";
            this.lblTotalNumFacturasValor.Size = new System.Drawing.Size(117, 13);
            this.lblTotalNumFacturasValor.TabIndex = 15;
            this.lblTotalNumFacturasValor.Text = "valor num fact total";
            // 
            // lblTotalNumFacturas
            // 
            this.lblTotalNumFacturas.AutoSize = true;
            this.lblTotalNumFacturas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblTotalNumFacturas.Location = new System.Drawing.Point(268, 17);
            this.lblTotalNumFacturas.Name = "lblTotalNumFacturas";
            this.lblTotalNumFacturas.Size = new System.Drawing.Size(115, 13);
            this.lblTotalNumFacturas.TabIndex = 14;
            this.lblTotalNumFacturas.Text = "Total Número Facturas";
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
            this.lblTotalReg.Location = new System.Drawing.Point(24, 17);
            this.lblTotalReg.Name = "lblTotalReg";
            this.lblTotalReg.Size = new System.Drawing.Size(78, 13);
            this.lblTotalReg.TabIndex = 12;
            this.lblTotalReg.Text = "Total Registros";
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
            // gbBuscador
            // 
            this.gbBuscador.Controls.Add(this.btnLimpiar);
            this.gbBuscador.Controls.Add(this.cmbEstadoCuadre);
            this.gbBuscador.Controls.Add(this.lblEstadoCuadre);
            this.gbBuscador.Controls.Add(this.txtNombreRazon);
            this.gbBuscador.Controls.Add(this.lblNombreRazon);
            this.gbBuscador.Controls.Add(this.txtNIF);
            this.gbBuscador.Controls.Add(this.lblNIF);
            this.gbBuscador.Controls.Add(this.txtEjercicio);
            this.gbBuscador.Controls.Add(this.lblCompania);
            this.gbBuscador.Controls.Add(this.tgTexBoxSelCiaFiscal);
            this.gbBuscador.Controls.Add(this.cmbPeriodo);
            this.gbBuscador.Controls.Add(this.lblPeriodo);
            this.gbBuscador.Controls.Add(this.lblEjercicio);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.gbBuscador.Location = new System.Drawing.Point(33, 41);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(1083, 137);
            this.gbBuscador.TabIndex = 5;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLimpiar.Location = new System.Drawing.Point(567, 104);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(80, 21);
            this.btnLimpiar.TabIndex = 125;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cmbEstadoCuadre
            // 
            this.cmbEstadoCuadre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoCuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbEstadoCuadre.FormattingEnabled = true;
            this.cmbEstadoCuadre.Location = new System.Drawing.Point(163, 71);
            this.cmbEstadoCuadre.MaxLength = 2;
            this.cmbEstadoCuadre.Name = "cmbEstadoCuadre";
            this.cmbEstadoCuadre.Size = new System.Drawing.Size(166, 21);
            this.cmbEstadoCuadre.TabIndex = 75;
            // 
            // lblEstadoCuadre
            // 
            this.lblEstadoCuadre.AutoSize = true;
            this.lblEstadoCuadre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEstadoCuadre.Location = new System.Drawing.Point(24, 74);
            this.lblEstadoCuadre.Name = "lblEstadoCuadre";
            this.lblEstadoCuadre.Size = new System.Drawing.Size(77, 13);
            this.lblEstadoCuadre.TabIndex = 70;
            this.lblEstadoCuadre.Text = "Estado Cuadre";
            // 
            // txtNombreRazon
            // 
            this.txtNombreRazon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtNombreRazon.Location = new System.Drawing.Point(601, 45);
            this.txtNombreRazon.MaxLength = 40;
            this.txtNombreRazon.Name = "txtNombreRazon";
            this.txtNombreRazon.Size = new System.Drawing.Size(453, 20);
            this.txtNombreRazon.TabIndex = 55;
            // 
            // lblNombreRazon
            // 
            this.lblNombreRazon.AutoSize = true;
            this.lblNombreRazon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNombreRazon.Location = new System.Drawing.Point(424, 50);
            this.lblNombreRazon.Name = "lblNombreRazon";
            this.lblNombreRazon.Size = new System.Drawing.Size(171, 13);
            this.lblNombreRazon.TabIndex = 50;
            this.lblNombreRazon.Text = "Nombre o Razón Social Proveedor";
            // 
            // txtNIF
            // 
            this.txtNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtNIF.Location = new System.Drawing.Point(163, 45);
            this.txtNIF.MaxLength = 20;
            this.txtNIF.Name = "txtNIF";
            this.txtNIF.Size = new System.Drawing.Size(143, 20);
            this.txtNIF.TabIndex = 45;
            // 
            // lblNIF
            // 
            this.lblNIF.AutoSize = true;
            this.lblNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNIF.Location = new System.Drawing.Point(24, 48);
            this.lblNIF.Name = "lblNIF";
            this.lblNIF.Size = new System.Drawing.Size(76, 13);
            this.lblNIF.TabIndex = 40;
            this.lblNIF.Text = "NIF Proveedor";
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.txtEjercicio.Location = new System.Drawing.Point(601, 17);
            this.txtEjercicio.MaxLength = 4;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(37, 20);
            this.txtEjercicio.TabIndex = 25;
            // 
            // lblCompania
            // 
            this.lblCompania.AutoSize = true;
            this.lblCompania.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblCompania.Location = new System.Drawing.Point(24, 20);
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
            this.tgTexBoxSelCiaFiscal.Location = new System.Drawing.Point(158, 12);
            this.tgTexBoxSelCiaFiscal.LocationFormSel = new System.Drawing.Point(0, 0);
            this.tgTexBoxSelCiaFiscal.Name = "tgTexBoxSelCiaFiscal";
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 0;
            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = null;
            this.tgTexBoxSelCiaFiscal.QueryFormSel = null;
            this.tgTexBoxSelCiaFiscal.SeparadorCampos = "-";
            this.tgTexBoxSelCiaFiscal.Size = new System.Drawing.Size(176, 30);
            this.tgTexBoxSelCiaFiscal.TabIndex = 15;
            this.tgTexBoxSelCiaFiscal.TituloFormSel = null;
            this.tgTexBoxSelCiaFiscal.TodoMayuscula = true;
            this.tgTexBoxSelCiaFiscal.Enter += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Enter);
            this.tgTexBoxSelCiaFiscal.Leave += new System.EventHandler(this.tgTexBoxSelCiaFiscal_Leave);
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(903, 16);
            this.cmbPeriodo.MaxLength = 2;
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(53, 21);
            this.cmbPeriodo.TabIndex = 35;
            this.cmbPeriodo.Leave += new System.EventHandler(this.cmbPeriodo_Leave);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblPeriodo.Location = new System.Drawing.Point(854, 20);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(43, 13);
            this.lblPeriodo.TabIndex = 30;
            this.lblPeriodo.Text = "Periodo";
            // 
            // lblEjercicio
            // 
            this.lblEjercicio.AutoSize = true;
            this.lblEjercicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblEjercicio.Location = new System.Drawing.Point(548, 19);
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
            this.btnBuscar.Location = new System.Drawing.Point(448, 104);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 21);
            this.btnBuscar.TabIndex = 120;
            this.btnBuscar.Text = "    Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(58, 678);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 1);
            this.panel1.TabIndex = 91;
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblNoInfo.Location = new System.Drawing.Point(40, 200);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.Size = new System.Drawing.Size(273, 13);
            this.lblNoInfo.TabIndex = 92;
            this.lblNoInfo.Text = "No existen facturas que cumplan el criterio seleccionado";
            this.lblNoInfo.Visible = false;
            // 
            // frmSiiConsultaAgrupadaProveedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 691);
            this.Controls.Add(this.lblNoInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tgGridConsulta);
            this.Controls.Add(this.gbResultado);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbBuscador);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiConsultaAgrupadaProveedor";
            this.Text = "Consulta de Facturas Informadas por Proveedor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSiiConsultaAgrupadaProveedor_FormClosing);
            this.Load += new System.EventHandler(this.frmSiiConsultaAgrupadaProveedor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSiiConsultaAgrupadaProveedor_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tgGridConsulta)).EndInit();
            this.gbResultado.ResumeLayout(false);
            this.gbResultado.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBuscador;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.ComboBox cmbEstadoCuadre;
        private System.Windows.Forms.Label lblEstadoCuadre;
        private System.Windows.Forms.TextBox txtNombreRazon;
        private System.Windows.Forms.Label lblNombreRazon;
        private System.Windows.Forms.TextBox txtNIF;
        private System.Windows.Forms.Label lblNIF;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.Label lblCompania;
        private ObjectModel.TGTexBoxSel tgTexBoxSelCiaFiscal;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblEjercicio;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabarPeticion;
        private System.Windows.Forms.ToolStripButton toolStripButtonCargarPeticion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.Label lblInfo;
        private ObjectModel.TGGrid tgGridConsulta;
        private System.Windows.Forms.GroupBox gbResultado;
        private System.Windows.Forms.Label lblTotalNumFacturasValor;
        private System.Windows.Forms.Label lblTotalNumFacturas;
        private System.Windows.Forms.Label lblTotalRegValor;
        private System.Windows.Forms.Label lblTotalReg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNoInfo;
    }
}