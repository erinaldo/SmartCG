namespace AppAdminModulos
{
    partial class frmClienteAltaEdita
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClienteAltaEdita));
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbIdSrvBBDD = new System.Windows.Forms.GroupBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.listBoxIdDisco = new System.Windows.Forms.ListBox();
            this.txtIdDisco = new System.Windows.Forms.TextBox();
            this.lblIdDisco = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbActivo = new System.Windows.Forms.RadioButton();
            this.rbActivoNo = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.cmbClientes = new System.Windows.Forms.ComboBox();
            this.lblClientes = new System.Windows.Forms.Label();
            this.gbIdSrvBBDD.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(317, 411);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbIdSrvBBDD
            // 
            this.gbIdSrvBBDD.Controls.Add(this.btnEliminar);
            this.gbIdSrvBBDD.Controls.Add(this.btnAdicionar);
            this.gbIdSrvBBDD.Controls.Add(this.listBoxIdDisco);
            this.gbIdSrvBBDD.Controls.Add(this.txtIdDisco);
            this.gbIdSrvBBDD.Controls.Add(this.lblIdDisco);
            this.gbIdSrvBBDD.Location = new System.Drawing.Point(27, 207);
            this.gbIdSrvBBDD.Name = "gbIdSrvBBDD";
            this.gbIdSrvBBDD.Size = new System.Drawing.Size(513, 154);
            this.gbIdSrvBBDD.TabIndex = 18;
            this.gbIdSrvBBDD.TabStop = false;
            this.gbIdSrvBBDD.Text = " Identificador Disco Servidor BBDD";
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(405, 67);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Location = new System.Drawing.Point(68, 67);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(75, 23);
            this.btnAdicionar.TabIndex = 3;
            this.btnAdicionar.Text = "Añadir";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // listBoxIdDisco
            // 
            this.listBoxIdDisco.FormattingEnabled = true;
            this.listBoxIdDisco.Location = new System.Drawing.Point(227, 25);
            this.listBoxIdDisco.Name = "listBoxIdDisco";
            this.listBoxIdDisco.Size = new System.Drawing.Size(155, 108);
            this.listBoxIdDisco.TabIndex = 2;
            // 
            // txtIdDisco
            // 
            this.txtIdDisco.Location = new System.Drawing.Point(68, 25);
            this.txtIdDisco.Name = "txtIdDisco";
            this.txtIdDisco.Size = new System.Drawing.Size(100, 20);
            this.txtIdDisco.TabIndex = 1;
            // 
            // lblIdDisco
            // 
            this.lblIdDisco.AutoSize = true;
            this.lblIdDisco.Location = new System.Drawing.Point(16, 32);
            this.lblIdDisco.Name = "lblIdDisco";
            this.lblIdDisco.Size = new System.Drawing.Size(46, 13);
            this.lblIdDisco.TabIndex = 0;
            this.lblIdDisco.Text = "Id Disco";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(182, 412);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 16;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtNombre);
            this.groupBox2.Controls.Add(this.lblNombre);
            this.groupBox2.Controls.Add(this.txtCodigo);
            this.groupBox2.Controls.Add(this.lblCodigo);
            this.groupBox2.Location = new System.Drawing.Point(27, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(513, 133);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "  Datos del Cliente  ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbActivo);
            this.groupBox3.Controls.Add(this.rbActivoNo);
            this.groupBox3.Location = new System.Drawing.Point(149, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(104, 32);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            // 
            // rbActivo
            // 
            this.rbActivo.AutoSize = true;
            this.rbActivo.Location = new System.Drawing.Point(6, 9);
            this.rbActivo.Name = "rbActivo";
            this.rbActivo.Size = new System.Drawing.Size(36, 17);
            this.rbActivo.TabIndex = 14;
            this.rbActivo.TabStop = true;
            this.rbActivo.Text = "Sí";
            this.rbActivo.UseVisualStyleBackColor = true;
            // 
            // rbActivoNo
            // 
            this.rbActivoNo.AutoSize = true;
            this.rbActivoNo.Location = new System.Drawing.Point(63, 9);
            this.rbActivoNo.Name = "rbActivoNo";
            this.rbActivoNo.Size = new System.Drawing.Size(39, 17);
            this.rbActivoNo.TabIndex = 15;
            this.rbActivoNo.TabStop = true;
            this.rbActivoNo.Text = "No";
            this.rbActivoNo.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Activo";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(149, 56);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(322, 20);
            this.txtNombre.TabIndex = 3;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(16, 63);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(149, 24);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(322, 20);
            this.txtCodigo.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(16, 32);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(40, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código";
            // 
            // cmbClientes
            // 
            this.cmbClientes.FormattingEnabled = true;
            this.cmbClientes.Location = new System.Drawing.Point(133, 14);
            this.cmbClientes.Name = "cmbClientes";
            this.cmbClientes.Size = new System.Drawing.Size(322, 21);
            this.cmbClientes.TabIndex = 1;
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.Location = new System.Drawing.Point(40, 22);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(44, 13);
            this.lblClientes.TabIndex = 0;
            this.lblClientes.Text = "Clientes";
            // 
            // frmClienteAltaEdita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(566, 473);
            this.Controls.Add(this.gbIdSrvBBDD);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmbClientes);
            this.Controls.Add(this.lblClientes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClienteAltaEdita";
            this.Text = "Cliente - ";
            this.Load += new System.EventHandler(this.frmClienteAltaEdita_Load);
            this.gbIdSrvBBDD.ResumeLayout(false);
            this.gbIdSrvBBDD.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientes;
        private System.Windows.Forms.ComboBox cmbClientes;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbActivo;
        private System.Windows.Forms.RadioButton rbActivoNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.GroupBox gbIdSrvBBDD;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.ListBox listBoxIdDisco;
        private System.Windows.Forms.TextBox txtIdDisco;
        private System.Windows.Forms.Label lblIdDisco;
    }
}