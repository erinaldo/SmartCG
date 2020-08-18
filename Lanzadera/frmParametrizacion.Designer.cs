namespace FinanzasNet
{
    partial class frmParametrizacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmParametrizacion));
            this.btnSalir = new System.Windows.Forms.Button();
            this.tabParametrizacion = new System.Windows.Forms.TabControl();
            this.tabPageBBDD = new System.Windows.Forms.TabPage();
            this.groupBoxCadenaConexion = new System.Windows.Forms.GroupBox();
            this.btnAceptarCadenaConexion = new System.Windows.Forms.Button();
            this.txtCadenaConexion = new System.Windows.Forms.TextBox();
            this.groupBoxBBDD = new System.Windows.Forms.GroupBox();
            this.txtPrefijo = new System.Windows.Forms.TextBox();
            this.btnGeneralAceptar = new System.Windows.Forms.Button();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.txtNombreBbdd = new System.Windows.Forms.TextBox();
            this.txtIpNombreServidor = new System.Windows.Forms.TextBox();
            this.lblNombreBbdd = new System.Windows.Forms.Label();
            this.lblIpNombreServidor = new System.Windows.Forms.Label();
            this.cmbTipoAcceso = new System.Windows.Forms.ComboBox();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblTipoAcceso = new System.Windows.Forms.Label();
            this.lblTipoBbdd = new System.Windows.Forms.Label();
            this.tabPageIdioma = new System.Windows.Forms.TabPage();
            this.lblNoExistenIdiomas = new System.Windows.Forms.Label();
            this.btnIdiomaAceptar = new System.Windows.Forms.Button();
            this.dataGridIdiomas = new System.Windows.Forms.DataGridView();
            this.tabParametrizacion.SuspendLayout();
            this.tabPageBBDD.SuspendLayout();
            this.groupBoxCadenaConexion.SuspendLayout();
            this.groupBoxBBDD.SuspendLayout();
            this.tabPageIdioma.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIdiomas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSalir
            // 
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Location = new System.Drawing.Point(594, 510);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // tabParametrizacion
            // 
            this.tabParametrizacion.Controls.Add(this.tabPageBBDD);
            this.tabParametrizacion.Controls.Add(this.tabPageIdioma);
            this.tabParametrizacion.Location = new System.Drawing.Point(21, 22);
            this.tabParametrizacion.Name = "tabParametrizacion";
            this.tabParametrizacion.SelectedIndex = 0;
            this.tabParametrizacion.Size = new System.Drawing.Size(648, 473);
            this.tabParametrizacion.TabIndex = 0;
            // 
            // tabPageBBDD
            // 
            this.tabPageBBDD.Controls.Add(this.groupBoxCadenaConexion);
            this.tabPageBBDD.Controls.Add(this.groupBoxBBDD);
            this.tabPageBBDD.Location = new System.Drawing.Point(4, 22);
            this.tabPageBBDD.Name = "tabPageBBDD";
            this.tabPageBBDD.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBBDD.Size = new System.Drawing.Size(640, 447);
            this.tabPageBBDD.TabIndex = 0;
            this.tabPageBBDD.Text = "BBDD";
            this.tabPageBBDD.UseVisualStyleBackColor = true;
            // 
            // groupBoxCadenaConexion
            // 
            this.groupBoxCadenaConexion.Controls.Add(this.btnAceptarCadenaConexion);
            this.groupBoxCadenaConexion.Controls.Add(this.txtCadenaConexion);
            this.groupBoxCadenaConexion.Location = new System.Drawing.Point(15, 171);
            this.groupBoxCadenaConexion.Name = "groupBoxCadenaConexion";
            this.groupBoxCadenaConexion.Size = new System.Drawing.Size(598, 185);
            this.groupBoxCadenaConexion.TabIndex = 2;
            this.groupBoxCadenaConexion.TabStop = false;
            this.groupBoxCadenaConexion.Text = " Cadena Conexión Bbdd Contabilidad ";
            // 
            // btnAceptarCadenaConexion
            // 
            this.btnAceptarCadenaConexion.Location = new System.Drawing.Point(269, 148);
            this.btnAceptarCadenaConexion.Name = "btnAceptarCadenaConexion";
            this.btnAceptarCadenaConexion.Size = new System.Drawing.Size(75, 23);
            this.btnAceptarCadenaConexion.TabIndex = 4;
            this.btnAceptarCadenaConexion.Text = "Aceptar";
            this.btnAceptarCadenaConexion.UseVisualStyleBackColor = true;
            this.btnAceptarCadenaConexion.Click += new System.EventHandler(this.btnAceptarCadenaConexion_Click);
            // 
            // txtCadenaConexion
            // 
            this.txtCadenaConexion.Location = new System.Drawing.Point(17, 33);
            this.txtCadenaConexion.Multiline = true;
            this.txtCadenaConexion.Name = "txtCadenaConexion";
            this.txtCadenaConexion.Size = new System.Drawing.Size(563, 94);
            this.txtCadenaConexion.TabIndex = 3;
            // 
            // groupBoxBBDD
            // 
            this.groupBoxBBDD.Controls.Add(this.txtPrefijo);
            this.groupBoxBBDD.Controls.Add(this.btnGeneralAceptar);
            this.groupBoxBBDD.Controls.Add(this.lblPrefijo);
            this.groupBoxBBDD.Controls.Add(this.txtNombreBbdd);
            this.groupBoxBBDD.Controls.Add(this.txtIpNombreServidor);
            this.groupBoxBBDD.Controls.Add(this.lblNombreBbdd);
            this.groupBoxBBDD.Controls.Add(this.lblIpNombreServidor);
            this.groupBoxBBDD.Controls.Add(this.cmbTipoAcceso);
            this.groupBoxBBDD.Controls.Add(this.cmbTipo);
            this.groupBoxBBDD.Controls.Add(this.lblTipoAcceso);
            this.groupBoxBBDD.Controls.Add(this.lblTipoBbdd);
            this.groupBoxBBDD.Location = new System.Drawing.Point(15, 15);
            this.groupBoxBBDD.Name = "groupBoxBBDD";
            this.groupBoxBBDD.Size = new System.Drawing.Size(598, 133);
            this.groupBoxBBDD.TabIndex = 0;
            this.groupBoxBBDD.TabStop = false;
            this.groupBoxBBDD.Text = " Bbdd Contabilidad ";
            // 
            // txtPrefijo
            // 
            this.txtPrefijo.Location = new System.Drawing.Point(144, 99);
            this.txtPrefijo.MaxLength = 25;
            this.txtPrefijo.Name = "txtPrefijo";
            this.txtPrefijo.Size = new System.Drawing.Size(146, 20);
            this.txtPrefijo.TabIndex = 9;
            // 
            // btnGeneralAceptar
            // 
            this.btnGeneralAceptar.Location = new System.Drawing.Point(322, 96);
            this.btnGeneralAceptar.Name = "btnGeneralAceptar";
            this.btnGeneralAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnGeneralAceptar.TabIndex = 1;
            this.btnGeneralAceptar.Text = "Aceptar";
            this.btnGeneralAceptar.UseVisualStyleBackColor = true;
            this.btnGeneralAceptar.Click += new System.EventHandler(this.btnGeneralAceptar_Click);
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Location = new System.Drawing.Point(20, 102);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(97, 13);
            this.lblPrefijo.TabIndex = 8;
            this.lblPrefijo.Text = "Prefijo tablas bbdd:";
            // 
            // txtNombreBbdd
            // 
            this.txtNombreBbdd.Enabled = false;
            this.txtNombreBbdd.Location = new System.Drawing.Point(410, 63);
            this.txtNombreBbdd.MaxLength = 25;
            this.txtNombreBbdd.Name = "txtNombreBbdd";
            this.txtNombreBbdd.Size = new System.Drawing.Size(162, 20);
            this.txtNombreBbdd.TabIndex = 7;
            // 
            // txtIpNombreServidor
            // 
            this.txtIpNombreServidor.Enabled = false;
            this.txtIpNombreServidor.Location = new System.Drawing.Point(144, 63);
            this.txtIpNombreServidor.MaxLength = 25;
            this.txtIpNombreServidor.Name = "txtIpNombreServidor";
            this.txtIpNombreServidor.Size = new System.Drawing.Size(146, 20);
            this.txtIpNombreServidor.TabIndex = 6;
            // 
            // lblNombreBbdd
            // 
            this.lblNombreBbdd.AutoSize = true;
            this.lblNombreBbdd.Enabled = false;
            this.lblNombreBbdd.Location = new System.Drawing.Point(323, 63);
            this.lblNombreBbdd.Name = "lblNombreBbdd";
            this.lblNombreBbdd.Size = new System.Drawing.Size(74, 13);
            this.lblNombreBbdd.TabIndex = 5;
            this.lblNombreBbdd.Text = "Nombre bbdd:";
            // 
            // lblIpNombreServidor
            // 
            this.lblIpNombreServidor.AutoSize = true;
            this.lblIpNombreServidor.Enabled = false;
            this.lblIpNombreServidor.Location = new System.Drawing.Point(20, 63);
            this.lblIpNombreServidor.Name = "lblIpNombreServidor";
            this.lblIpNombreServidor.Size = new System.Drawing.Size(124, 13);
            this.lblIpNombreServidor.TabIndex = 4;
            this.lblIpNombreServidor.Text = "IP o nombre del servidor:";
            // 
            // cmbTipoAcceso
            // 
            this.cmbTipoAcceso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoAcceso.FormattingEnabled = true;
            this.cmbTipoAcceso.Location = new System.Drawing.Point(410, 25);
            this.cmbTipoAcceso.Name = "cmbTipoAcceso";
            this.cmbTipoAcceso.Size = new System.Drawing.Size(162, 21);
            this.cmbTipoAcceso.TabIndex = 3;
            this.cmbTipoAcceso.SelectedIndexChanged += new System.EventHandler(this.cmbTipoAcceso_SelectedIndexChanged);
            // 
            // cmbTipo
            // 
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.FormattingEnabled = true;
            this.cmbTipo.Location = new System.Drawing.Point(51, 25);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(239, 21);
            this.cmbTipo.TabIndex = 2;
            this.cmbTipo.SelectedIndexChanged += new System.EventHandler(this.cmbTipo_SelectedIndexChanged);
            // 
            // lblTipoAcceso
            // 
            this.lblTipoAcceso.AutoSize = true;
            this.lblTipoAcceso.Location = new System.Drawing.Point(319, 29);
            this.lblTipoAcceso.Name = "lblTipoAcceso";
            this.lblTipoAcceso.Size = new System.Drawing.Size(84, 13);
            this.lblTipoAcceso.TabIndex = 1;
            this.lblTipoAcceso.Text = "Tipo de acceso:";
            // 
            // lblTipoBbdd
            // 
            this.lblTipoBbdd.AutoSize = true;
            this.lblTipoBbdd.Location = new System.Drawing.Point(18, 29);
            this.lblTipoBbdd.Name = "lblTipoBbdd";
            this.lblTipoBbdd.Size = new System.Drawing.Size(31, 13);
            this.lblTipoBbdd.TabIndex = 0;
            this.lblTipoBbdd.Text = "Tipo:";
            // 
            // tabPageIdioma
            // 
            this.tabPageIdioma.Controls.Add(this.lblNoExistenIdiomas);
            this.tabPageIdioma.Controls.Add(this.btnIdiomaAceptar);
            this.tabPageIdioma.Controls.Add(this.dataGridIdiomas);
            this.tabPageIdioma.Location = new System.Drawing.Point(4, 22);
            this.tabPageIdioma.Name = "tabPageIdioma";
            this.tabPageIdioma.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIdioma.Size = new System.Drawing.Size(640, 447);
            this.tabPageIdioma.TabIndex = 1;
            this.tabPageIdioma.Text = "Idiomas";
            this.tabPageIdioma.UseVisualStyleBackColor = true;
            // 
            // lblNoExistenIdiomas
            // 
            this.lblNoExistenIdiomas.AutoSize = true;
            this.lblNoExistenIdiomas.Location = new System.Drawing.Point(27, 20);
            this.lblNoExistenIdiomas.Name = "lblNoExistenIdiomas";
            this.lblNoExistenIdiomas.Size = new System.Drawing.Size(140, 13);
            this.lblNoExistenIdiomas.TabIndex = 2;
            this.lblNoExistenIdiomas.Text = "No existen idiomas definidos";
            this.lblNoExistenIdiomas.Visible = false;
            // 
            // btnIdiomaAceptar
            // 
            this.btnIdiomaAceptar.Location = new System.Drawing.Point(283, 278);
            this.btnIdiomaAceptar.Name = "btnIdiomaAceptar";
            this.btnIdiomaAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnIdiomaAceptar.TabIndex = 1;
            this.btnIdiomaAceptar.Text = "Aceptar";
            this.btnIdiomaAceptar.UseVisualStyleBackColor = true;
            this.btnIdiomaAceptar.Click += new System.EventHandler(this.btnIdiomaAceptar_Click);
            // 
            // dataGridIdiomas
            // 
            this.dataGridIdiomas.AllowUserToAddRows = false;
            this.dataGridIdiomas.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridIdiomas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridIdiomas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridIdiomas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridIdiomas.Location = new System.Drawing.Point(27, 20);
            this.dataGridIdiomas.MultiSelect = false;
            this.dataGridIdiomas.Name = "dataGridIdiomas";
            this.dataGridIdiomas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridIdiomas.Size = new System.Drawing.Size(587, 201);
            this.dataGridIdiomas.TabIndex = 0;
            this.dataGridIdiomas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridIdiomas_CellContentClick);
            this.dataGridIdiomas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridIdiomas_CellValueChanged);
            this.dataGridIdiomas.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridIdiomas_DataBindingComplete);
            // 
            // frmParametrizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSalir;
            this.ClientSize = new System.Drawing.Size(690, 555);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.tabParametrizacion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmParametrizacion";
            this.Text = "frmParametrizacion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmParametrizacion_FormClosing);
            this.Load += new System.EventHandler(this.frmParametrizacion_Load);
            this.tabParametrizacion.ResumeLayout(false);
            this.tabPageBBDD.ResumeLayout(false);
            this.groupBoxCadenaConexion.ResumeLayout(false);
            this.groupBoxCadenaConexion.PerformLayout();
            this.groupBoxBBDD.ResumeLayout(false);
            this.groupBoxBBDD.PerformLayout();
            this.tabPageIdioma.ResumeLayout(false);
            this.tabPageIdioma.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIdiomas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabParametrizacion;
        private System.Windows.Forms.TabPage tabPageBBDD;
        private System.Windows.Forms.TabPage tabPageIdioma;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnGeneralAceptar;
        private System.Windows.Forms.GroupBox groupBoxBBDD;
        private System.Windows.Forms.TextBox txtPrefijo;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.TextBox txtNombreBbdd;
        private System.Windows.Forms.TextBox txtIpNombreServidor;
        private System.Windows.Forms.Label lblNombreBbdd;
        private System.Windows.Forms.Label lblIpNombreServidor;
        private System.Windows.Forms.ComboBox cmbTipoAcceso;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblTipoAcceso;
        private System.Windows.Forms.Label lblTipoBbdd;
        private System.Windows.Forms.TextBox txtCadenaConexion;
        private System.Windows.Forms.GroupBox groupBoxCadenaConexion;
        private System.Windows.Forms.Button btnAceptarCadenaConexion;
        private System.Windows.Forms.Button btnIdiomaAceptar;
        private System.Windows.Forms.DataGridView dataGridIdiomas;
        private System.Windows.Forms.Label lblNoExistenIdiomas;
    }
}