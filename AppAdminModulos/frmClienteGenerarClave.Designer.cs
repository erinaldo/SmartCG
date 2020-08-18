namespace AppAdminModulos
{
    partial class frmClienteGenerarClave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClienteGenerarClave));
            this.txtIdDisco = new System.Windows.Forms.TextBox();
            this.lblIdDisco = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnGenerarClave = new System.Windows.Forms.Button();
            this.btnQuitarAll = new System.Windows.Forms.Button();
            this.btnAdicionarAll = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.lblModulosSel = new System.Windows.Forms.Label();
            this.listboxModulosSel = new System.Windows.Forms.ListBox();
            this.lblModActivos = new System.Windows.Forms.Label();
            this.listboxModulosActivos = new System.Windows.Forms.ListBox();
            this.numeroUserConcurrentes = new System.Windows.Forms.NumericUpDown();
            this.numeroServidores = new System.Windows.Forms.NumericUpDown();
            this.lblLicUsuarios = new System.Windows.Forms.Label();
            this.lblLicBbdd = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.lblCaducidad = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblClaveGenerada = new System.Windows.Forms.Label();
            this.gbClaveGenerada = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numeroUserConcurrentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeroServidores)).BeginInit();
            this.gbClaveGenerada.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIdDisco
            // 
            this.txtIdDisco.Location = new System.Drawing.Point(235, 95);
            this.txtIdDisco.Name = "txtIdDisco";
            this.txtIdDisco.Size = new System.Drawing.Size(131, 20);
            this.txtIdDisco.TabIndex = 19;
            // 
            // lblIdDisco
            // 
            this.lblIdDisco.AutoSize = true;
            this.lblIdDisco.Location = new System.Drawing.Point(23, 102);
            this.lblIdDisco.Name = "lblIdDisco";
            this.lblIdDisco.Size = new System.Drawing.Size(95, 13);
            this.lblIdDisco.TabIndex = 18;
            this.lblIdDisco.Text = "Identificador Disco";
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(394, 556);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(106, 23);
            this.btnSalir.TabIndex = 17;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGenerarClave
            // 
            this.btnGenerarClave.Location = new System.Drawing.Point(190, 556);
            this.btnGenerarClave.Name = "btnGenerarClave";
            this.btnGenerarClave.Size = new System.Drawing.Size(98, 23);
            this.btnGenerarClave.TabIndex = 16;
            this.btnGenerarClave.Text = "Generar Clave";
            this.btnGenerarClave.UseVisualStyleBackColor = true;
            this.btnGenerarClave.Click += new System.EventHandler(this.btnGenerarClave_Click);
            // 
            // btnQuitarAll
            // 
            this.btnQuitarAll.Location = new System.Drawing.Point(314, 422);
            this.btnQuitarAll.Name = "btnQuitarAll";
            this.btnQuitarAll.Size = new System.Drawing.Size(65, 23);
            this.btnQuitarAll.TabIndex = 15;
            this.btnQuitarAll.Text = "   < <    ";
            this.btnQuitarAll.UseVisualStyleBackColor = true;
            this.btnQuitarAll.Click += new System.EventHandler(this.btnQuitarAll_Click);
            // 
            // btnAdicionarAll
            // 
            this.btnAdicionarAll.Location = new System.Drawing.Point(314, 382);
            this.btnAdicionarAll.Name = "btnAdicionarAll";
            this.btnAdicionarAll.Size = new System.Drawing.Size(63, 23);
            this.btnAdicionarAll.TabIndex = 14;
            this.btnAdicionarAll.Text = "  > >   ";
            this.btnAdicionarAll.UseVisualStyleBackColor = true;
            this.btnAdicionarAll.Click += new System.EventHandler(this.btnAdicionarAll_Click);
            // 
            // btnQuitar
            // 
            this.btnQuitar.Location = new System.Drawing.Point(314, 340);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(63, 23);
            this.btnQuitar.TabIndex = 13;
            this.btnQuitar.Text = "    <    ";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Location = new System.Drawing.Point(314, 299);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(63, 23);
            this.btnAdicionar.TabIndex = 12;
            this.btnAdicionar.Text = "    >    ";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // lblModulosSel
            // 
            this.lblModulosSel.AutoSize = true;
            this.lblModulosSel.Location = new System.Drawing.Point(468, 229);
            this.lblModulosSel.Name = "lblModulosSel";
            this.lblModulosSel.Size = new System.Drawing.Size(120, 13);
            this.lblModulosSel.TabIndex = 11;
            this.lblModulosSel.Text = "Módulos Seleccionados";
            // 
            // listboxModulosSel
            // 
            this.listboxModulosSel.FormattingEnabled = true;
            this.listboxModulosSel.Location = new System.Drawing.Point(394, 261);
            this.listboxModulosSel.Name = "listboxModulosSel";
            this.listboxModulosSel.Size = new System.Drawing.Size(262, 251);
            this.listboxModulosSel.Sorted = true;
            this.listboxModulosSel.TabIndex = 10;
            // 
            // lblModActivos
            // 
            this.lblModActivos.AutoSize = true;
            this.lblModActivos.Location = new System.Drawing.Point(107, 229);
            this.lblModActivos.Name = "lblModActivos";
            this.lblModActivos.Size = new System.Drawing.Size(85, 13);
            this.lblModActivos.TabIndex = 9;
            this.lblModActivos.Text = "Módulos Activos";
            // 
            // listboxModulosActivos
            // 
            this.listboxModulosActivos.AllowDrop = true;
            this.listboxModulosActivos.FormattingEnabled = true;
            this.listboxModulosActivos.Location = new System.Drawing.Point(26, 261);
            this.listboxModulosActivos.Name = "listboxModulosActivos";
            this.listboxModulosActivos.Size = new System.Drawing.Size(262, 251);
            this.listboxModulosActivos.Sorted = true;
            this.listboxModulosActivos.TabIndex = 8;
            // 
            // numeroUserConcurrentes
            // 
            this.numeroUserConcurrentes.Location = new System.Drawing.Point(235, 162);
            this.numeroUserConcurrentes.Name = "numeroUserConcurrentes";
            this.numeroUserConcurrentes.Size = new System.Drawing.Size(131, 20);
            this.numeroUserConcurrentes.TabIndex = 7;
            this.numeroUserConcurrentes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numeroServidores
            // 
            this.numeroServidores.Location = new System.Drawing.Point(235, 129);
            this.numeroServidores.Name = "numeroServidores";
            this.numeroServidores.Size = new System.Drawing.Size(131, 20);
            this.numeroServidores.TabIndex = 6;
            this.numeroServidores.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLicUsuarios
            // 
            this.lblLicUsuarios.AutoSize = true;
            this.lblLicUsuarios.Location = new System.Drawing.Point(23, 169);
            this.lblLicUsuarios.Name = "lblLicUsuarios";
            this.lblLicUsuarios.Size = new System.Drawing.Size(182, 13);
            this.lblLicUsuarios.TabIndex = 5;
            this.lblLicUsuarios.Text = "No. Licencias Usuarios Concurrentes";
            // 
            // lblLicBbdd
            // 
            this.lblLicBbdd.AutoSize = true;
            this.lblLicBbdd.Location = new System.Drawing.Point(23, 136);
            this.lblLicBbdd.Name = "lblLicBbdd";
            this.lblLicBbdd.Size = new System.Drawing.Size(159, 13);
            this.lblLicBbdd.TabIndex = 4;
            this.lblLicBbdd.Text = "No. Licencias Instalación BBDD";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(235, 61);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(421, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // lblCaducidad
            // 
            this.lblCaducidad.AutoSize = true;
            this.lblCaducidad.Location = new System.Drawing.Point(23, 69);
            this.lblCaducidad.Name = "lblCaducidad";
            this.lblCaducidad.Size = new System.Drawing.Size(58, 13);
            this.lblCaducidad.TabIndex = 2;
            this.lblCaducidad.Text = "Caducidad";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(235, 26);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(421, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(23, 34);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(39, 13);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente";
            // 
            // lblClaveGenerada
            // 
            this.lblClaveGenerada.AutoSize = true;
            this.lblClaveGenerada.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClaveGenerada.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblClaveGenerada.Location = new System.Drawing.Point(137, 23);
            this.lblClaveGenerada.Name = "lblClaveGenerada";
            this.lblClaveGenerada.Size = new System.Drawing.Size(36, 13);
            this.lblClaveGenerada.TabIndex = 21;
            this.lblClaveGenerada.Text = "Valor";
            this.lblClaveGenerada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClaveGenerada.Visible = false;
            // 
            // gbClaveGenerada
            // 
            this.gbClaveGenerada.Controls.Add(this.lblClaveGenerada);
            this.gbClaveGenerada.Location = new System.Drawing.Point(26, 589);
            this.gbClaveGenerada.Name = "gbClaveGenerada";
            this.gbClaveGenerada.Size = new System.Drawing.Size(630, 51);
            this.gbClaveGenerada.TabIndex = 22;
            this.gbClaveGenerada.TabStop = false;
            this.gbClaveGenerada.Text = "  Clave Generada  ";
            this.gbClaveGenerada.Visible = false;
            // 
            // frmClienteGenerarClave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 653);
            this.Controls.Add(this.gbClaveGenerada);
            this.Controls.Add(this.txtIdDisco);
            this.Controls.Add(this.lblIdDisco);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnGenerarClave);
            this.Controls.Add(this.btnQuitarAll);
            this.Controls.Add(this.btnAdicionarAll);
            this.Controls.Add(this.btnQuitar);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.lblModulosSel);
            this.Controls.Add(this.listboxModulosSel);
            this.Controls.Add(this.lblModActivos);
            this.Controls.Add(this.listboxModulosActivos);
            this.Controls.Add(this.numeroUserConcurrentes);
            this.Controls.Add(this.numeroServidores);
            this.Controls.Add(this.lblLicUsuarios);
            this.Controls.Add(this.lblLicBbdd);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.lblCaducidad);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblCliente);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClienteGenerarClave";
            this.Text = "Generar Clave";
            this.Load += new System.EventHandler(this.frmClienteGenerarClave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numeroUserConcurrentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeroServidores)).EndInit();
            this.gbClaveGenerada.ResumeLayout(false);
            this.gbClaveGenerada.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblCaducidad;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label lblLicBbdd;
        private System.Windows.Forms.Label lblLicUsuarios;
        private System.Windows.Forms.NumericUpDown numeroServidores;
        private System.Windows.Forms.NumericUpDown numeroUserConcurrentes;
        private System.Windows.Forms.ListBox listboxModulosActivos;
        private System.Windows.Forms.Label lblModActivos;
        private System.Windows.Forms.Label lblModulosSel;
        private System.Windows.Forms.ListBox listboxModulosSel;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnAdicionarAll;
        private System.Windows.Forms.Button btnQuitarAll;
        private System.Windows.Forms.Button btnGenerarClave;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblIdDisco;
        private System.Windows.Forms.TextBox txtIdDisco;
        private System.Windows.Forms.Label lblClaveGenerada;
        private System.Windows.Forms.GroupBox gbClaveGenerada;
    }
}