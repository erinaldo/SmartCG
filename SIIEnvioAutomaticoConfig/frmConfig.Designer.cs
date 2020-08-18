namespace SIIEnvioAutomaticoConfig
{
    partial class frmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            this.txtUrlWebService = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGrabar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.txtEjercicio = new System.Windows.Forms.TextBox();
            this.btnAddCiaFiscal = new System.Windows.Forms.Button();
            this.gbListaCiasFiscales = new System.Windows.Forms.GroupBox();
            this.txtCompania = new System.Windows.Forms.TextBox();
            this.btnQuitarCiaFiscal = new System.Windows.Forms.Button();
            this.lbCiasFiscales = new System.Windows.Forms.ListBox();
            this.gbListaLibros = new System.Windows.Forms.GroupBox();
            this.cmbLibro = new System.Windows.Forms.ComboBox();
            this.chkLibrosTodos = new System.Windows.Forms.CheckBox();
            this.btnQuitarLibro = new System.Windows.Forms.Button();
            this.btnAddLibro = new System.Windows.Forms.Button();
            this.lbLibros = new System.Windows.Forms.ListBox();
            this.gbListaOperaciones = new System.Windows.Forms.GroupBox();
            this.cmbOperacion = new System.Windows.Forms.ComboBox();
            this.chkOperacionesTodas = new System.Windows.Forms.CheckBox();
            this.btnQuitarOperacion = new System.Windows.Forms.Button();
            this.btnAddOperacion = new System.Windows.Forms.Button();
            this.lbOperaciones = new System.Windows.Forms.ListBox();
            this.gbListaEjercicios = new System.Windows.Forms.GroupBox();
            this.btnQuitarEjercicio = new System.Windows.Forms.Button();
            this.btnAddEjercicio = new System.Windows.Forms.Button();
            this.lbEjercicios = new System.Windows.Forms.ListBox();
            this.gbListaPeriodos = new System.Windows.Forms.GroupBox();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.chkPeriodosTodos = new System.Windows.Forms.CheckBox();
            this.btnQuitarPeriodo = new System.Windows.Forms.Button();
            this.btnAddPeriodo = new System.Windows.Forms.Button();
            this.lbPeriodos = new System.Windows.Forms.ListBox();
            this.gbRespuestaEnvio = new System.Windows.Forms.GroupBox();
            this.btnSelRutaFicheroRespuestaEnvio = new System.Windows.Forms.Button();
            this.txtRutaFicheroRespuestaEnvio = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkActivarRespuestaEnvio = new System.Windows.Forms.CheckBox();
            this.gbFiheroLog = new System.Windows.Forms.GroupBox();
            this.btnSelRutaFicheroLog = new System.Windows.Forms.Button();
            this.txtRutaFicheroLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkActivarLog = new System.Windows.Forms.CheckBox();
            this.gbCorreo = new System.Windows.Forms.GroupBox();
            this.txtMailBody = new System.Windows.Forms.TextBox();
            this.lblMailBody = new System.Windows.Forms.Label();
            this.txtMailSubject = new System.Windows.Forms.TextBox();
            this.lblMailSubject = new System.Windows.Forms.Label();
            this.txtMailTo = new System.Windows.Forms.TextBox();
            this.lblMailTo = new System.Windows.Forms.Label();
            this.txtMailFrom = new System.Windows.Forms.TextBox();
            this.lblMailFrom = new System.Windows.Forms.Label();
            this.gbSMTP = new System.Windows.Forms.GroupBox();
            this.chkSMTPEnableSSL = new System.Windows.Forms.CheckBox();
            this.txtSMTPContrasena = new System.Windows.Forms.TextBox();
            this.lblAMTPContrasena = new System.Windows.Forms.Label();
            this.txtSMTPUsuario = new System.Windows.Forms.TextBox();
            this.lblSMTPUsuario = new System.Windows.Forms.Label();
            this.txtSMTPPuerto = new System.Windows.Forms.TextBox();
            this.lblSMTPPuerto = new System.Windows.Forms.Label();
            this.txtSMTPServidor = new System.Windows.Forms.TextBox();
            this.lblSMTPServidor = new System.Windows.Forms.Label();
            this.chkMailActivoAlerta = new System.Windows.Forms.CheckBox();
            this.chkMailActivo = new System.Windows.Forms.CheckBox();
            this.lblUrlWebService = new System.Windows.Forms.Label();
            this.folderBrowserDialogRespuesta = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialogLog = new System.Windows.Forms.FolderBrowserDialog();
            this.gbVariosIntentos = new System.Windows.Forms.GroupBox();
            this.txtNumeroIntentos = new System.Windows.Forms.TextBox();
            this.lblNumeroIntentos = new System.Windows.Forms.Label();
            this.txtTiempoEspera = new System.Windows.Forms.TextBox();
            this.lblTiempoEspera = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.gbListaCiasFiscales.SuspendLayout();
            this.gbListaLibros.SuspendLayout();
            this.gbListaOperaciones.SuspendLayout();
            this.gbListaEjercicios.SuspendLayout();
            this.gbListaPeriodos.SuspendLayout();
            this.gbRespuestaEnvio.SuspendLayout();
            this.gbFiheroLog.SuspendLayout();
            this.gbCorreo.SuspendLayout();
            this.gbSMTP.SuspendLayout();
            this.gbVariosIntentos.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrlWebService
            // 
            this.txtUrlWebService.Location = new System.Drawing.Point(168, 40);
            this.txtUrlWebService.Name = "txtUrlWebService";
            this.txtUrlWebService.Size = new System.Drawing.Size(548, 20);
            this.txtUrlWebService.TabIndex = 9;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGrabar,
            this.toolStripSeparator3,
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(851, 29);
            this.toolStrip1.TabIndex = 10;
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSalir.Image")));
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(49, 26);
            this.toolStripButtonSalir.Text = "Salir";
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // txtEjercicio
            // 
            this.txtEjercicio.Location = new System.Drawing.Point(26, 23);
            this.txtEjercicio.MaxLength = 2;
            this.txtEjercicio.Name = "txtEjercicio";
            this.txtEjercicio.Size = new System.Drawing.Size(30, 20);
            this.txtEjercicio.TabIndex = 118;
            this.txtEjercicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEjercicio_KeyPress);
            this.txtEjercicio.Leave += new System.EventHandler(this.txtEjercicio_Leave);
            // 
            // btnAddCiaFiscal
            // 
            this.btnAddCiaFiscal.Enabled = false;
            this.btnAddCiaFiscal.Location = new System.Drawing.Point(26, 56);
            this.btnAddCiaFiscal.Name = "btnAddCiaFiscal";
            this.btnAddCiaFiscal.Size = new System.Drawing.Size(75, 23);
            this.btnAddCiaFiscal.TabIndex = 10;
            this.btnAddCiaFiscal.Text = "Añadir";
            this.btnAddCiaFiscal.UseVisualStyleBackColor = true;
            this.btnAddCiaFiscal.Click += new System.EventHandler(this.btnAddCiaFiscal_Click);
            // 
            // gbListaCiasFiscales
            // 
            this.gbListaCiasFiscales.Controls.Add(this.txtCompania);
            this.gbListaCiasFiscales.Controls.Add(this.btnQuitarCiaFiscal);
            this.gbListaCiasFiscales.Controls.Add(this.btnAddCiaFiscal);
            this.gbListaCiasFiscales.Controls.Add(this.lbCiasFiscales);
            this.gbListaCiasFiscales.Location = new System.Drawing.Point(37, 65);
            this.gbListaCiasFiscales.Name = "gbListaCiasFiscales";
            this.gbListaCiasFiscales.Size = new System.Drawing.Size(379, 122);
            this.gbListaCiasFiscales.TabIndex = 125;
            this.gbListaCiasFiscales.TabStop = false;
            this.gbListaCiasFiscales.Text = " Lista de Compañías Fiscales";
            // 
            // txtCompania
            // 
            this.txtCompania.Location = new System.Drawing.Point(26, 23);
            this.txtCompania.MaxLength = 2;
            this.txtCompania.Name = "txtCompania";
            this.txtCompania.Size = new System.Drawing.Size(56, 20);
            this.txtCompania.TabIndex = 119;
            this.txtCompania.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCompania_KeyPress);
            this.txtCompania.Leave += new System.EventHandler(this.txtCompania_Leave);
            // 
            // btnQuitarCiaFiscal
            // 
            this.btnQuitarCiaFiscal.Enabled = false;
            this.btnQuitarCiaFiscal.Location = new System.Drawing.Point(107, 56);
            this.btnQuitarCiaFiscal.Name = "btnQuitarCiaFiscal";
            this.btnQuitarCiaFiscal.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarCiaFiscal.TabIndex = 15;
            this.btnQuitarCiaFiscal.Text = "Quitar";
            this.btnQuitarCiaFiscal.UseVisualStyleBackColor = true;
            this.btnQuitarCiaFiscal.Click += new System.EventHandler(this.btnQuitarCiaFiscal_Click);
            // 
            // lbCiasFiscales
            // 
            this.lbCiasFiscales.FormattingEnabled = true;
            this.lbCiasFiscales.Location = new System.Drawing.Point(257, 18);
            this.lbCiasFiscales.Name = "lbCiasFiscales";
            this.lbCiasFiscales.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbCiasFiscales.Size = new System.Drawing.Size(102, 95);
            this.lbCiasFiscales.Sorted = true;
            this.lbCiasFiscales.TabIndex = 25;
            this.lbCiasFiscales.Enter += new System.EventHandler(this.lbCiasFiscales_Enter);
            // 
            // gbListaLibros
            // 
            this.gbListaLibros.Controls.Add(this.cmbLibro);
            this.gbListaLibros.Controls.Add(this.chkLibrosTodos);
            this.gbListaLibros.Controls.Add(this.btnQuitarLibro);
            this.gbListaLibros.Controls.Add(this.btnAddLibro);
            this.gbListaLibros.Controls.Add(this.lbLibros);
            this.gbListaLibros.Location = new System.Drawing.Point(37, 187);
            this.gbListaLibros.Name = "gbListaLibros";
            this.gbListaLibros.Size = new System.Drawing.Size(379, 122);
            this.gbListaLibros.TabIndex = 126;
            this.gbListaLibros.TabStop = false;
            this.gbListaLibros.Text = " Lista de Libros";
            // 
            // cmbLibro
            // 
            this.cmbLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLibro.FormattingEnabled = true;
            this.cmbLibro.Location = new System.Drawing.Point(26, 23);
            this.cmbLibro.Name = "cmbLibro";
            this.cmbLibro.Size = new System.Drawing.Size(172, 21);
            this.cmbLibro.TabIndex = 86;
            // 
            // chkLibrosTodos
            // 
            this.chkLibrosTodos.AutoSize = true;
            this.chkLibrosTodos.Location = new System.Drawing.Point(26, 96);
            this.chkLibrosTodos.Name = "chkLibrosTodos";
            this.chkLibrosTodos.Size = new System.Drawing.Size(103, 17);
            this.chkLibrosTodos.TabIndex = 27;
            this.chkLibrosTodos.Text = "Todos los Libros";
            this.chkLibrosTodos.UseVisualStyleBackColor = true;
            // 
            // btnQuitarLibro
            // 
            this.btnQuitarLibro.Enabled = false;
            this.btnQuitarLibro.Location = new System.Drawing.Point(107, 56);
            this.btnQuitarLibro.Name = "btnQuitarLibro";
            this.btnQuitarLibro.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarLibro.TabIndex = 15;
            this.btnQuitarLibro.Text = "Quitar";
            this.btnQuitarLibro.UseVisualStyleBackColor = true;
            this.btnQuitarLibro.Click += new System.EventHandler(this.btnQuitarLibro_Click);
            // 
            // btnAddLibro
            // 
            this.btnAddLibro.Location = new System.Drawing.Point(26, 56);
            this.btnAddLibro.Name = "btnAddLibro";
            this.btnAddLibro.Size = new System.Drawing.Size(75, 23);
            this.btnAddLibro.TabIndex = 10;
            this.btnAddLibro.Text = "Añadir";
            this.btnAddLibro.UseVisualStyleBackColor = true;
            this.btnAddLibro.Click += new System.EventHandler(this.btnAddLibro_Click);
            // 
            // lbLibros
            // 
            this.lbLibros.FormattingEnabled = true;
            this.lbLibros.Location = new System.Drawing.Point(257, 18);
            this.lbLibros.Name = "lbLibros";
            this.lbLibros.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbLibros.Size = new System.Drawing.Size(102, 95);
            this.lbLibros.Sorted = true;
            this.lbLibros.TabIndex = 25;
            this.lbLibros.Enter += new System.EventHandler(this.lbLibros_Enter);
            // 
            // gbListaOperaciones
            // 
            this.gbListaOperaciones.Controls.Add(this.cmbOperacion);
            this.gbListaOperaciones.Controls.Add(this.chkOperacionesTodas);
            this.gbListaOperaciones.Controls.Add(this.btnQuitarOperacion);
            this.gbListaOperaciones.Controls.Add(this.btnAddOperacion);
            this.gbListaOperaciones.Controls.Add(this.lbOperaciones);
            this.gbListaOperaciones.Location = new System.Drawing.Point(451, 187);
            this.gbListaOperaciones.Name = "gbListaOperaciones";
            this.gbListaOperaciones.Size = new System.Drawing.Size(379, 122);
            this.gbListaOperaciones.TabIndex = 127;
            this.gbListaOperaciones.TabStop = false;
            this.gbListaOperaciones.Text = " Lista de Operaciones";
            // 
            // cmbOperacion
            // 
            this.cmbOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperacion.FormattingEnabled = true;
            this.cmbOperacion.Location = new System.Drawing.Point(26, 23);
            this.cmbOperacion.Name = "cmbOperacion";
            this.cmbOperacion.Size = new System.Drawing.Size(172, 21);
            this.cmbOperacion.TabIndex = 86;
            // 
            // chkOperacionesTodas
            // 
            this.chkOperacionesTodas.AutoSize = true;
            this.chkOperacionesTodas.Location = new System.Drawing.Point(26, 96);
            this.chkOperacionesTodas.Name = "chkOperacionesTodas";
            this.chkOperacionesTodas.Size = new System.Drawing.Size(133, 17);
            this.chkOperacionesTodas.TabIndex = 27;
            this.chkOperacionesTodas.Text = "Todas las operaciones";
            this.chkOperacionesTodas.UseVisualStyleBackColor = true;
            // 
            // btnQuitarOperacion
            // 
            this.btnQuitarOperacion.Enabled = false;
            this.btnQuitarOperacion.Location = new System.Drawing.Point(107, 56);
            this.btnQuitarOperacion.Name = "btnQuitarOperacion";
            this.btnQuitarOperacion.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarOperacion.TabIndex = 15;
            this.btnQuitarOperacion.Text = "Quitar";
            this.btnQuitarOperacion.UseVisualStyleBackColor = true;
            this.btnQuitarOperacion.Click += new System.EventHandler(this.btnQuitarOperacion_Click);
            // 
            // btnAddOperacion
            // 
            this.btnAddOperacion.Location = new System.Drawing.Point(26, 56);
            this.btnAddOperacion.Name = "btnAddOperacion";
            this.btnAddOperacion.Size = new System.Drawing.Size(75, 23);
            this.btnAddOperacion.TabIndex = 10;
            this.btnAddOperacion.Text = "Añadir";
            this.btnAddOperacion.UseVisualStyleBackColor = true;
            this.btnAddOperacion.Click += new System.EventHandler(this.btnAddOperacion_Click);
            // 
            // lbOperaciones
            // 
            this.lbOperaciones.FormattingEnabled = true;
            this.lbOperaciones.Location = new System.Drawing.Point(257, 18);
            this.lbOperaciones.Name = "lbOperaciones";
            this.lbOperaciones.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbOperaciones.Size = new System.Drawing.Size(102, 95);
            this.lbOperaciones.Sorted = true;
            this.lbOperaciones.TabIndex = 25;
            this.lbOperaciones.Enter += new System.EventHandler(this.lbOperaciones_Enter);
            // 
            // gbListaEjercicios
            // 
            this.gbListaEjercicios.Controls.Add(this.btnQuitarEjercicio);
            this.gbListaEjercicios.Controls.Add(this.btnAddEjercicio);
            this.gbListaEjercicios.Controls.Add(this.lbEjercicios);
            this.gbListaEjercicios.Controls.Add(this.txtEjercicio);
            this.gbListaEjercicios.Location = new System.Drawing.Point(37, 311);
            this.gbListaEjercicios.Name = "gbListaEjercicios";
            this.gbListaEjercicios.Size = new System.Drawing.Size(379, 122);
            this.gbListaEjercicios.TabIndex = 128;
            this.gbListaEjercicios.TabStop = false;
            this.gbListaEjercicios.Text = " Lista de Ejercicios";
            // 
            // btnQuitarEjercicio
            // 
            this.btnQuitarEjercicio.Location = new System.Drawing.Point(107, 56);
            this.btnQuitarEjercicio.Name = "btnQuitarEjercicio";
            this.btnQuitarEjercicio.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarEjercicio.TabIndex = 15;
            this.btnQuitarEjercicio.Text = "Quitar";
            this.btnQuitarEjercicio.UseVisualStyleBackColor = true;
            this.btnQuitarEjercicio.Click += new System.EventHandler(this.btnQuitarEjercicio_Click);
            // 
            // btnAddEjercicio
            // 
            this.btnAddEjercicio.Location = new System.Drawing.Point(26, 56);
            this.btnAddEjercicio.Name = "btnAddEjercicio";
            this.btnAddEjercicio.Size = new System.Drawing.Size(75, 23);
            this.btnAddEjercicio.TabIndex = 10;
            this.btnAddEjercicio.Text = "Añadir";
            this.btnAddEjercicio.UseVisualStyleBackColor = true;
            this.btnAddEjercicio.Click += new System.EventHandler(this.btnAddEjercicio_Click);
            // 
            // lbEjercicios
            // 
            this.lbEjercicios.FormattingEnabled = true;
            this.lbEjercicios.Location = new System.Drawing.Point(257, 18);
            this.lbEjercicios.Name = "lbEjercicios";
            this.lbEjercicios.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbEjercicios.Size = new System.Drawing.Size(102, 95);
            this.lbEjercicios.Sorted = true;
            this.lbEjercicios.TabIndex = 25;
            this.lbEjercicios.Enter += new System.EventHandler(this.lbEjercicios_Enter);
            // 
            // gbListaPeriodos
            // 
            this.gbListaPeriodos.Controls.Add(this.cmbPeriodo);
            this.gbListaPeriodos.Controls.Add(this.chkPeriodosTodos);
            this.gbListaPeriodos.Controls.Add(this.btnQuitarPeriodo);
            this.gbListaPeriodos.Controls.Add(this.btnAddPeriodo);
            this.gbListaPeriodos.Controls.Add(this.lbPeriodos);
            this.gbListaPeriodos.Location = new System.Drawing.Point(451, 311);
            this.gbListaPeriodos.Name = "gbListaPeriodos";
            this.gbListaPeriodos.Size = new System.Drawing.Size(379, 122);
            this.gbListaPeriodos.TabIndex = 129;
            this.gbListaPeriodos.TabStop = false;
            this.gbListaPeriodos.Text = " Lista de Periodos";
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriodo.FormattingEnabled = true;
            this.cmbPeriodo.Location = new System.Drawing.Point(26, 23);
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(172, 21);
            this.cmbPeriodo.TabIndex = 86;
            // 
            // chkPeriodosTodos
            // 
            this.chkPeriodosTodos.AutoSize = true;
            this.chkPeriodosTodos.Location = new System.Drawing.Point(26, 96);
            this.chkPeriodosTodos.Name = "chkPeriodosTodos";
            this.chkPeriodosTodos.Size = new System.Drawing.Size(115, 17);
            this.chkPeriodosTodos.TabIndex = 27;
            this.chkPeriodosTodos.Text = "Todos los periodos";
            this.chkPeriodosTodos.UseVisualStyleBackColor = true;
            // 
            // btnQuitarPeriodo
            // 
            this.btnQuitarPeriodo.Enabled = false;
            this.btnQuitarPeriodo.Location = new System.Drawing.Point(107, 56);
            this.btnQuitarPeriodo.Name = "btnQuitarPeriodo";
            this.btnQuitarPeriodo.Size = new System.Drawing.Size(75, 23);
            this.btnQuitarPeriodo.TabIndex = 15;
            this.btnQuitarPeriodo.Text = "Quitar";
            this.btnQuitarPeriodo.UseVisualStyleBackColor = true;
            this.btnQuitarPeriodo.Click += new System.EventHandler(this.btnQuitarPeriodo_Click);
            // 
            // btnAddPeriodo
            // 
            this.btnAddPeriodo.Location = new System.Drawing.Point(26, 56);
            this.btnAddPeriodo.Name = "btnAddPeriodo";
            this.btnAddPeriodo.Size = new System.Drawing.Size(75, 23);
            this.btnAddPeriodo.TabIndex = 10;
            this.btnAddPeriodo.Text = "Añadir";
            this.btnAddPeriodo.UseVisualStyleBackColor = true;
            this.btnAddPeriodo.Click += new System.EventHandler(this.btnAddPeriodo_Click);
            // 
            // lbPeriodos
            // 
            this.lbPeriodos.FormattingEnabled = true;
            this.lbPeriodos.Location = new System.Drawing.Point(257, 18);
            this.lbPeriodos.Name = "lbPeriodos";
            this.lbPeriodos.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbPeriodos.Size = new System.Drawing.Size(102, 95);
            this.lbPeriodos.Sorted = true;
            this.lbPeriodos.TabIndex = 25;
            this.lbPeriodos.Enter += new System.EventHandler(this.lbPeriodos_Enter);
            // 
            // gbRespuestaEnvio
            // 
            this.gbRespuestaEnvio.Controls.Add(this.btnSelRutaFicheroRespuestaEnvio);
            this.gbRespuestaEnvio.Controls.Add(this.txtRutaFicheroRespuestaEnvio);
            this.gbRespuestaEnvio.Controls.Add(this.label1);
            this.gbRespuestaEnvio.Controls.Add(this.chkActivarRespuestaEnvio);
            this.gbRespuestaEnvio.Location = new System.Drawing.Point(37, 435);
            this.gbRespuestaEnvio.Name = "gbRespuestaEnvio";
            this.gbRespuestaEnvio.Size = new System.Drawing.Size(793, 52);
            this.gbRespuestaEnvio.TabIndex = 129;
            this.gbRespuestaEnvio.TabStop = false;
            this.gbRespuestaEnvio.Text = " Fichero Respuesta de Envio";
            // 
            // btnSelRutaFicheroRespuestaEnvio
            // 
            this.btnSelRutaFicheroRespuestaEnvio.Image = ((System.Drawing.Image)(resources.GetObject("btnSelRutaFicheroRespuestaEnvio.Image")));
            this.btnSelRutaFicheroRespuestaEnvio.Location = new System.Drawing.Point(734, 14);
            this.btnSelRutaFicheroRespuestaEnvio.Name = "btnSelRutaFicheroRespuestaEnvio";
            this.btnSelRutaFicheroRespuestaEnvio.Size = new System.Drawing.Size(39, 31);
            this.btnSelRutaFicheroRespuestaEnvio.TabIndex = 31;
            this.btnSelRutaFicheroRespuestaEnvio.UseVisualStyleBackColor = true;
            this.btnSelRutaFicheroRespuestaEnvio.Click += new System.EventHandler(this.btnSelRutaFicheroRespuestaEnvio_Click);
            // 
            // txtRutaFicheroRespuestaEnvio
            // 
            this.txtRutaFicheroRespuestaEnvio.Location = new System.Drawing.Point(164, 20);
            this.txtRutaFicheroRespuestaEnvio.Name = "txtRutaFicheroRespuestaEnvio";
            this.txtRutaFicheroRespuestaEnvio.Size = new System.Drawing.Size(564, 20);
            this.txtRutaFicheroRespuestaEnvio.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Ruta";
            // 
            // chkActivarRespuestaEnvio
            // 
            this.chkActivarRespuestaEnvio.AutoSize = true;
            this.chkActivarRespuestaEnvio.Location = new System.Drawing.Point(26, 22);
            this.chkActivarRespuestaEnvio.Name = "chkActivarRespuestaEnvio";
            this.chkActivarRespuestaEnvio.Size = new System.Drawing.Size(59, 17);
            this.chkActivarRespuestaEnvio.TabIndex = 28;
            this.chkActivarRespuestaEnvio.Text = "Activar";
            this.chkActivarRespuestaEnvio.UseVisualStyleBackColor = true;
            this.chkActivarRespuestaEnvio.CheckedChanged += new System.EventHandler(this.chkActivarRespuestaEnvio_CheckedChanged);
            // 
            // gbFiheroLog
            // 
            this.gbFiheroLog.Controls.Add(this.btnSelRutaFicheroLog);
            this.gbFiheroLog.Controls.Add(this.txtRutaFicheroLog);
            this.gbFiheroLog.Controls.Add(this.label2);
            this.gbFiheroLog.Controls.Add(this.chkActivarLog);
            this.gbFiheroLog.Location = new System.Drawing.Point(37, 489);
            this.gbFiheroLog.Name = "gbFiheroLog";
            this.gbFiheroLog.Size = new System.Drawing.Size(793, 49);
            this.gbFiheroLog.TabIndex = 130;
            this.gbFiheroLog.TabStop = false;
            this.gbFiheroLog.Text = " Fichero Log";
            // 
            // btnSelRutaFicheroLog
            // 
            this.btnSelRutaFicheroLog.Image = ((System.Drawing.Image)(resources.GetObject("btnSelRutaFicheroLog.Image")));
            this.btnSelRutaFicheroLog.Location = new System.Drawing.Point(734, 14);
            this.btnSelRutaFicheroLog.Name = "btnSelRutaFicheroLog";
            this.btnSelRutaFicheroLog.Size = new System.Drawing.Size(39, 31);
            this.btnSelRutaFicheroLog.TabIndex = 31;
            this.btnSelRutaFicheroLog.UseVisualStyleBackColor = true;
            this.btnSelRutaFicheroLog.Click += new System.EventHandler(this.btnSelRutaFicheroLog_Click);
            // 
            // txtRutaFicheroLog
            // 
            this.txtRutaFicheroLog.Location = new System.Drawing.Point(164, 20);
            this.txtRutaFicheroLog.Name = "txtRutaFicheroLog";
            this.txtRutaFicheroLog.Size = new System.Drawing.Size(564, 20);
            this.txtRutaFicheroLog.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Ruta";
            // 
            // chkActivarLog
            // 
            this.chkActivarLog.AutoSize = true;
            this.chkActivarLog.Location = new System.Drawing.Point(26, 22);
            this.chkActivarLog.Name = "chkActivarLog";
            this.chkActivarLog.Size = new System.Drawing.Size(59, 17);
            this.chkActivarLog.TabIndex = 28;
            this.chkActivarLog.Text = "Activar";
            this.chkActivarLog.UseVisualStyleBackColor = true;
            this.chkActivarLog.CheckedChanged += new System.EventHandler(this.chkActivarLog_CheckedChanged);
            // 
            // gbCorreo
            // 
            this.gbCorreo.Controls.Add(this.txtMailBody);
            this.gbCorreo.Controls.Add(this.lblMailBody);
            this.gbCorreo.Controls.Add(this.txtMailSubject);
            this.gbCorreo.Controls.Add(this.lblMailSubject);
            this.gbCorreo.Controls.Add(this.txtMailTo);
            this.gbCorreo.Controls.Add(this.lblMailTo);
            this.gbCorreo.Controls.Add(this.txtMailFrom);
            this.gbCorreo.Controls.Add(this.lblMailFrom);
            this.gbCorreo.Controls.Add(this.gbSMTP);
            this.gbCorreo.Controls.Add(this.chkMailActivoAlerta);
            this.gbCorreo.Controls.Add(this.chkMailActivo);
            this.gbCorreo.Location = new System.Drawing.Point(37, 543);
            this.gbCorreo.Name = "gbCorreo";
            this.gbCorreo.Size = new System.Drawing.Size(793, 252);
            this.gbCorreo.TabIndex = 131;
            this.gbCorreo.TabStop = false;
            this.gbCorreo.Text = " Correo";
            // 
            // txtMailBody
            // 
            this.txtMailBody.Location = new System.Drawing.Point(213, 203);
            this.txtMailBody.Multiline = true;
            this.txtMailBody.Name = "txtMailBody";
            this.txtMailBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMailBody.Size = new System.Drawing.Size(560, 36);
            this.txtMailBody.TabIndex = 139;
            // 
            // lblMailBody
            // 
            this.lblMailBody.AutoSize = true;
            this.lblMailBody.Location = new System.Drawing.Point(23, 206);
            this.lblMailBody.Name = "lblMailBody";
            this.lblMailBody.Size = new System.Drawing.Size(47, 13);
            this.lblMailBody.TabIndex = 138;
            this.lblMailBody.Text = "Mensaje";
            // 
            // txtMailSubject
            // 
            this.txtMailSubject.Location = new System.Drawing.Point(213, 180);
            this.txtMailSubject.Name = "txtMailSubject";
            this.txtMailSubject.Size = new System.Drawing.Size(560, 20);
            this.txtMailSubject.TabIndex = 137;
            // 
            // lblMailSubject
            // 
            this.lblMailSubject.AutoSize = true;
            this.lblMailSubject.Location = new System.Drawing.Point(23, 183);
            this.lblMailSubject.Name = "lblMailSubject";
            this.lblMailSubject.Size = new System.Drawing.Size(40, 13);
            this.lblMailSubject.TabIndex = 136;
            this.lblMailSubject.Text = "Asunto";
            // 
            // txtMailTo
            // 
            this.txtMailTo.Location = new System.Drawing.Point(213, 158);
            this.txtMailTo.Name = "txtMailTo";
            this.txtMailTo.Size = new System.Drawing.Size(333, 20);
            this.txtMailTo.TabIndex = 135;
            // 
            // lblMailTo
            // 
            this.lblMailTo.AutoSize = true;
            this.lblMailTo.Location = new System.Drawing.Point(23, 161);
            this.lblMailTo.Name = "lblMailTo";
            this.lblMailTo.Size = new System.Drawing.Size(179, 13);
            this.lblMailTo.TabIndex = 134;
            this.lblMailTo.Text = "Dirección de correo electrónico para";
            // 
            // txtMailFrom
            // 
            this.txtMailFrom.Location = new System.Drawing.Point(213, 135);
            this.txtMailFrom.Name = "txtMailFrom";
            this.txtMailFrom.Size = new System.Drawing.Size(333, 20);
            this.txtMailFrom.TabIndex = 133;
            // 
            // lblMailFrom
            // 
            this.lblMailFrom.AutoSize = true;
            this.lblMailFrom.Location = new System.Drawing.Point(23, 138);
            this.lblMailFrom.Name = "lblMailFrom";
            this.lblMailFrom.Size = new System.Drawing.Size(170, 13);
            this.lblMailFrom.TabIndex = 132;
            this.lblMailFrom.Text = "Dirección de correo electrónico de";
            // 
            // gbSMTP
            // 
            this.gbSMTP.Controls.Add(this.chkSMTPEnableSSL);
            this.gbSMTP.Controls.Add(this.txtSMTPContrasena);
            this.gbSMTP.Controls.Add(this.lblAMTPContrasena);
            this.gbSMTP.Controls.Add(this.txtSMTPUsuario);
            this.gbSMTP.Controls.Add(this.lblSMTPUsuario);
            this.gbSMTP.Controls.Add(this.txtSMTPPuerto);
            this.gbSMTP.Controls.Add(this.lblSMTPPuerto);
            this.gbSMTP.Controls.Add(this.txtSMTPServidor);
            this.gbSMTP.Controls.Add(this.lblSMTPServidor);
            this.gbSMTP.Location = new System.Drawing.Point(24, 40);
            this.gbSMTP.Name = "gbSMTP";
            this.gbSMTP.Size = new System.Drawing.Size(747, 89);
            this.gbSMTP.TabIndex = 131;
            this.gbSMTP.TabStop = false;
            this.gbSMTP.Text = " SMTP";
            // 
            // chkSMTPEnableSSL
            // 
            this.chkSMTPEnableSSL.AutoSize = true;
            this.chkSMTPEnableSSL.Location = new System.Drawing.Point(26, 67);
            this.chkSMTPEnableSSL.Name = "chkSMTPEnableSSL";
            this.chkSMTPEnableSSL.Size = new System.Drawing.Size(87, 17);
            this.chkSMTPEnableSSL.TabIndex = 37;
            this.chkSMTPEnableSSL.Text = "Habilitar SSL";
            this.chkSMTPEnableSSL.UseVisualStyleBackColor = true;
            // 
            // txtSMTPContrasena
            // 
            this.txtSMTPContrasena.Location = new System.Drawing.Point(507, 42);
            this.txtSMTPContrasena.Name = "txtSMTPContrasena";
            this.txtSMTPContrasena.PasswordChar = '*';
            this.txtSMTPContrasena.Size = new System.Drawing.Size(218, 20);
            this.txtSMTPContrasena.TabIndex = 36;
            // 
            // lblAMTPContrasena
            // 
            this.lblAMTPContrasena.AutoSize = true;
            this.lblAMTPContrasena.Location = new System.Drawing.Point(440, 45);
            this.lblAMTPContrasena.Name = "lblAMTPContrasena";
            this.lblAMTPContrasena.Size = new System.Drawing.Size(61, 13);
            this.lblAMTPContrasena.TabIndex = 35;
            this.lblAMTPContrasena.Text = "Contraseña";
            // 
            // txtSMTPUsuario
            // 
            this.txtSMTPUsuario.Location = new System.Drawing.Point(69, 42);
            this.txtSMTPUsuario.Name = "txtSMTPUsuario";
            this.txtSMTPUsuario.Size = new System.Drawing.Size(333, 20);
            this.txtSMTPUsuario.TabIndex = 34;
            // 
            // lblSMTPUsuario
            // 
            this.lblSMTPUsuario.AutoSize = true;
            this.lblSMTPUsuario.Location = new System.Drawing.Point(23, 45);
            this.lblSMTPUsuario.Name = "lblSMTPUsuario";
            this.lblSMTPUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblSMTPUsuario.TabIndex = 33;
            this.lblSMTPUsuario.Text = "Usuarrio";
            // 
            // txtSMTPPuerto
            // 
            this.txtSMTPPuerto.Location = new System.Drawing.Point(507, 18);
            this.txtSMTPPuerto.Name = "txtSMTPPuerto";
            this.txtSMTPPuerto.Size = new System.Drawing.Size(43, 20);
            this.txtSMTPPuerto.TabIndex = 32;
            // 
            // lblSMTPPuerto
            // 
            this.lblSMTPPuerto.AutoSize = true;
            this.lblSMTPPuerto.Location = new System.Drawing.Point(440, 21);
            this.lblSMTPPuerto.Name = "lblSMTPPuerto";
            this.lblSMTPPuerto.Size = new System.Drawing.Size(38, 13);
            this.lblSMTPPuerto.TabIndex = 31;
            this.lblSMTPPuerto.Text = "Puerto";
            // 
            // txtSMTPServidor
            // 
            this.txtSMTPServidor.Location = new System.Drawing.Point(69, 18);
            this.txtSMTPServidor.Name = "txtSMTPServidor";
            this.txtSMTPServidor.Size = new System.Drawing.Size(333, 20);
            this.txtSMTPServidor.TabIndex = 30;
            // 
            // lblSMTPServidor
            // 
            this.lblSMTPServidor.AutoSize = true;
            this.lblSMTPServidor.Location = new System.Drawing.Point(23, 21);
            this.lblSMTPServidor.Name = "lblSMTPServidor";
            this.lblSMTPServidor.Size = new System.Drawing.Size(46, 13);
            this.lblSMTPServidor.TabIndex = 29;
            this.lblSMTPServidor.Text = "Servidor";
            // 
            // chkMailActivoAlerta
            // 
            this.chkMailActivoAlerta.AutoSize = true;
            this.chkMailActivoAlerta.Location = new System.Drawing.Point(134, 18);
            this.chkMailActivoAlerta.Name = "chkMailActivoAlerta";
            this.chkMailActivoAlerta.Size = new System.Drawing.Size(191, 17);
            this.chkMailActivoAlerta.TabIndex = 29;
            this.chkMailActivoAlerta.Text = "Enviar correo sólo si existen alertas";
            this.chkMailActivoAlerta.UseVisualStyleBackColor = true;
            // 
            // chkMailActivo
            // 
            this.chkMailActivo.AutoSize = true;
            this.chkMailActivo.Location = new System.Drawing.Point(26, 18);
            this.chkMailActivo.Name = "chkMailActivo";
            this.chkMailActivo.Size = new System.Drawing.Size(89, 17);
            this.chkMailActivo.TabIndex = 28;
            this.chkMailActivo.Text = "Enviar correo";
            this.chkMailActivo.UseVisualStyleBackColor = true;
            // 
            // lblUrlWebService
            // 
            this.lblUrlWebService.AutoSize = true;
            this.lblUrlWebService.Location = new System.Drawing.Point(34, 43);
            this.lblUrlWebService.Name = "lblUrlWebService";
            this.lblUrlWebService.Size = new System.Drawing.Size(119, 13);
            this.lblUrlWebService.TabIndex = 8;
            this.lblUrlWebService.Text = "Dirección Servicio Web";
            // 
            // gbVariosIntentos
            // 
            this.gbVariosIntentos.Controls.Add(this.label5);
            this.gbVariosIntentos.Controls.Add(this.txtTiempoEspera);
            this.gbVariosIntentos.Controls.Add(this.lblTiempoEspera);
            this.gbVariosIntentos.Controls.Add(this.txtNumeroIntentos);
            this.gbVariosIntentos.Controls.Add(this.lblNumeroIntentos);
            this.gbVariosIntentos.Location = new System.Drawing.Point(35, 800);
            this.gbVariosIntentos.Name = "gbVariosIntentos";
            this.gbVariosIntentos.Size = new System.Drawing.Size(793, 53);
            this.gbVariosIntentos.TabIndex = 132;
            this.gbVariosIntentos.TabStop = false;
            this.gbVariosIntentos.Text = " Varios intentos (si error) ";
            // 
            // txtNumeroIntentos
            // 
            this.txtNumeroIntentos.Location = new System.Drawing.Point(133, 23);
            this.txtNumeroIntentos.MaxLength = 2;
            this.txtNumeroIntentos.Name = "txtNumeroIntentos";
            this.txtNumeroIntentos.Size = new System.Drawing.Size(36, 20);
            this.txtNumeroIntentos.TabIndex = 30;
            this.txtNumeroIntentos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumeroIntentos_KeyPress);
            // 
            // lblNumeroIntentos
            // 
            this.lblNumeroIntentos.AutoSize = true;
            this.lblNumeroIntentos.Location = new System.Drawing.Point(24, 26);
            this.lblNumeroIntentos.Name = "lblNumeroIntentos";
            this.lblNumeroIntentos.Size = new System.Drawing.Size(99, 13);
            this.lblNumeroIntentos.TabIndex = 29;
            this.lblNumeroIntentos.Text = "Número de intentos";
            // 
            // txtTiempoEspera
            // 
            this.txtTiempoEspera.Location = new System.Drawing.Point(405, 23);
            this.txtTiempoEspera.MaxLength = 7;
            this.txtTiempoEspera.Name = "txtTiempoEspera";
            this.txtTiempoEspera.Size = new System.Drawing.Size(58, 20);
            this.txtTiempoEspera.TabIndex = 32;
            this.txtTiempoEspera.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTiempoEspera_KeyPress);
            // 
            // lblTiempoEspera
            // 
            this.lblTiempoEspera.AutoSize = true;
            this.lblTiempoEspera.Location = new System.Drawing.Point(301, 26);
            this.lblTiempoEspera.Name = "lblTiempoEspera";
            this.lblTiempoEspera.Size = new System.Drawing.Size(92, 13);
            this.lblTiempoEspera.TabIndex = 31;
            this.lblTiempoEspera.Text = "Tiempo de espera";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(464, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "(milisegundos)";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(868, 657);
            this.Controls.Add(this.gbVariosIntentos);
            this.Controls.Add(this.gbCorreo);
            this.Controls.Add(this.gbFiheroLog);
            this.Controls.Add(this.gbRespuestaEnvio);
            this.Controls.Add(this.gbListaPeriodos);
            this.Controls.Add(this.gbListaEjercicios);
            this.Controls.Add(this.gbListaOperaciones);
            this.Controls.Add(this.gbListaLibros);
            this.Controls.Add(this.gbListaCiasFiscales);
            this.Controls.Add(this.txtUrlWebService);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblUrlWebService);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfig";
            this.Text = "Configurar Envio Automático SII";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbListaCiasFiscales.ResumeLayout(false);
            this.gbListaCiasFiscales.PerformLayout();
            this.gbListaLibros.ResumeLayout(false);
            this.gbListaLibros.PerformLayout();
            this.gbListaOperaciones.ResumeLayout(false);
            this.gbListaOperaciones.PerformLayout();
            this.gbListaEjercicios.ResumeLayout(false);
            this.gbListaEjercicios.PerformLayout();
            this.gbListaPeriodos.ResumeLayout(false);
            this.gbListaPeriodos.PerformLayout();
            this.gbRespuestaEnvio.ResumeLayout(false);
            this.gbRespuestaEnvio.PerformLayout();
            this.gbFiheroLog.ResumeLayout(false);
            this.gbFiheroLog.PerformLayout();
            this.gbCorreo.ResumeLayout(false);
            this.gbCorreo.PerformLayout();
            this.gbSMTP.ResumeLayout(false);
            this.gbSMTP.PerformLayout();
            this.gbVariosIntentos.ResumeLayout(false);
            this.gbVariosIntentos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrlWebService;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.TextBox txtEjercicio;
        private System.Windows.Forms.Button btnAddCiaFiscal;
        private System.Windows.Forms.GroupBox gbListaCiasFiscales;
        private System.Windows.Forms.Button btnQuitarCiaFiscal;
        private System.Windows.Forms.ListBox lbCiasFiscales;
        private System.Windows.Forms.GroupBox gbListaLibros;
        private System.Windows.Forms.Button btnQuitarLibro;
        private System.Windows.Forms.Button btnAddLibro;
        private System.Windows.Forms.ListBox lbLibros;
        private System.Windows.Forms.CheckBox chkLibrosTodos;
        private System.Windows.Forms.ComboBox cmbLibro;
        private System.Windows.Forms.GroupBox gbListaOperaciones;
        private System.Windows.Forms.ComboBox cmbOperacion;
        private System.Windows.Forms.CheckBox chkOperacionesTodas;
        private System.Windows.Forms.Button btnQuitarOperacion;
        private System.Windows.Forms.Button btnAddOperacion;
        private System.Windows.Forms.ListBox lbOperaciones;
        private System.Windows.Forms.GroupBox gbListaEjercicios;
        private System.Windows.Forms.Button btnQuitarEjercicio;
        private System.Windows.Forms.Button btnAddEjercicio;
        private System.Windows.Forms.ListBox lbEjercicios;
        private System.Windows.Forms.GroupBox gbListaPeriodos;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.CheckBox chkPeriodosTodos;
        private System.Windows.Forms.Button btnQuitarPeriodo;
        private System.Windows.Forms.Button btnAddPeriodo;
        private System.Windows.Forms.ListBox lbPeriodos;
        private System.Windows.Forms.TextBox txtCompania;
        private System.Windows.Forms.GroupBox gbRespuestaEnvio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkActivarRespuestaEnvio;
        private System.Windows.Forms.Button btnSelRutaFicheroRespuestaEnvio;
        private System.Windows.Forms.TextBox txtRutaFicheroRespuestaEnvio;
        private System.Windows.Forms.GroupBox gbFiheroLog;
        private System.Windows.Forms.Button btnSelRutaFicheroLog;
        private System.Windows.Forms.TextBox txtRutaFicheroLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkActivarLog;
        private System.Windows.Forms.GroupBox gbCorreo;
        private System.Windows.Forms.CheckBox chkMailActivo;
        private System.Windows.Forms.CheckBox chkMailActivoAlerta;
        private System.Windows.Forms.GroupBox gbSMTP;
        private System.Windows.Forms.TextBox txtSMTPServidor;
        private System.Windows.Forms.Label lblSMTPServidor;
        private System.Windows.Forms.TextBox txtSMTPPuerto;
        private System.Windows.Forms.Label lblSMTPPuerto;
        private System.Windows.Forms.TextBox txtSMTPContrasena;
        private System.Windows.Forms.Label lblAMTPContrasena;
        private System.Windows.Forms.TextBox txtSMTPUsuario;
        private System.Windows.Forms.Label lblSMTPUsuario;
        private System.Windows.Forms.CheckBox chkSMTPEnableSSL;
        private System.Windows.Forms.TextBox txtMailFrom;
        private System.Windows.Forms.Label lblMailFrom;
        private System.Windows.Forms.TextBox txtMailTo;
        private System.Windows.Forms.Label lblMailTo;
        private System.Windows.Forms.TextBox txtMailSubject;
        private System.Windows.Forms.Label lblMailSubject;
        private System.Windows.Forms.TextBox txtMailBody;
        private System.Windows.Forms.Label lblMailBody;
        private System.Windows.Forms.Label lblUrlWebService;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogRespuesta;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogLog;
        private System.Windows.Forms.GroupBox gbVariosIntentos;
        private System.Windows.Forms.TextBox txtNumeroIntentos;
        private System.Windows.Forms.Label lblNumeroIntentos;
        private System.Windows.Forms.TextBox txtTiempoEspera;
        private System.Windows.Forms.Label lblTiempoEspera;
        private System.Windows.Forms.Label label5;
    }
}

