namespace AppAdminModulos
{
    partial class frmClienteConsultarClave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClienteConsultarClave));
            this.lblClave = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lbModulos = new System.Windows.Forms.ListBox();
            this.lblModulos = new System.Windows.Forms.Label();
            this.lblFechaValor = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblLicUsuariosValor = new System.Windows.Forms.Label();
            this.lblLicUsuarios = new System.Windows.Forms.Label();
            this.lblLicBbddValor = new System.Windows.Forms.Label();
            this.lblLicBbdd = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIdDisco = new System.Windows.Forms.Label();
            this.lblCaducidadValor = new System.Windows.Forms.Label();
            this.lblCaducidad = new System.Windows.Forms.Label();
            this.lblClienteValor = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new System.Drawing.Point(23, 34);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(34, 13);
            this.lblClave.TabIndex = 0;
            this.lblClave.Text = "Clave";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(63, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(315, 20);
            this.textBox1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(120, 78);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(258, 77);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lbModulos);
            this.gbInfo.Controls.Add(this.lblModulos);
            this.gbInfo.Controls.Add(this.lblFechaValor);
            this.gbInfo.Controls.Add(this.lblFecha);
            this.gbInfo.Controls.Add(this.lblLicUsuariosValor);
            this.gbInfo.Controls.Add(this.lblLicUsuarios);
            this.gbInfo.Controls.Add(this.lblLicBbddValor);
            this.gbInfo.Controls.Add(this.lblLicBbdd);
            this.gbInfo.Controls.Add(this.label1);
            this.gbInfo.Controls.Add(this.lblIdDisco);
            this.gbInfo.Controls.Add(this.lblCaducidadValor);
            this.gbInfo.Controls.Add(this.lblCaducidad);
            this.gbInfo.Controls.Add(this.lblClienteValor);
            this.gbInfo.Controls.Add(this.lblCliente);
            this.gbInfo.Location = new System.Drawing.Point(26, 125);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(415, 471);
            this.gbInfo.TabIndex = 4;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "  Información  ";
            this.gbInfo.Visible = false;
            // 
            // lbModulos
            // 
            this.lbModulos.FormattingEnabled = true;
            this.lbModulos.Location = new System.Drawing.Point(21, 248);
            this.lbModulos.Name = "lbModulos";
            this.lbModulos.Size = new System.Drawing.Size(372, 199);
            this.lbModulos.TabIndex = 13;
            // 
            // lblModulos
            // 
            this.lblModulos.AutoSize = true;
            this.lblModulos.Location = new System.Drawing.Point(18, 213);
            this.lblModulos.Name = "lblModulos";
            this.lblModulos.Size = new System.Drawing.Size(47, 13);
            this.lblModulos.TabIndex = 12;
            this.lblModulos.Text = "Módulos";
            // 
            // lblFechaValor
            // 
            this.lblFechaValor.AutoSize = true;
            this.lblFechaValor.Location = new System.Drawing.Point(115, 168);
            this.lblFechaValor.Name = "lblFechaValor";
            this.lblFechaValor.Size = new System.Drawing.Size(103, 13);
            this.lblFechaValor.TabIndex = 11;
            this.lblFechaValor.Text = "Info Fecha Creación";
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(18, 169);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(37, 13);
            this.lblFecha.TabIndex = 10;
            this.lblFecha.Text = "Fecha";
            // 
            // lblLicUsuariosValor
            // 
            this.lblLicUsuariosValor.AutoSize = true;
            this.lblLicUsuariosValor.Location = new System.Drawing.Point(112, 143);
            this.lblLicUsuariosValor.Name = "lblLicUsuariosValor";
            this.lblLicUsuariosValor.Size = new System.Drawing.Size(203, 13);
            this.lblLicUsuariosValor.TabIndex = 9;
            this.lblLicUsuariosValor.Text = "Info No. Licencias Usuarios Concurrentes";
            // 
            // lblLicUsuarios
            // 
            this.lblLicUsuarios.AutoSize = true;
            this.lblLicUsuarios.Location = new System.Drawing.Point(18, 144);
            this.lblLicUsuarios.Name = "lblLicUsuarios";
            this.lblLicUsuarios.Size = new System.Drawing.Size(88, 13);
            this.lblLicUsuarios.TabIndex = 8;
            this.lblLicUsuarios.Text = "No. Lic. Usuarios";
            // 
            // lblLicBbddValor
            // 
            this.lblLicBbddValor.AutoSize = true;
            this.lblLicBbddValor.Location = new System.Drawing.Point(112, 117);
            this.lblLicBbddValor.Name = "lblLicBbddValor";
            this.lblLicBbddValor.Size = new System.Drawing.Size(180, 13);
            this.lblLicBbddValor.TabIndex = 7;
            this.lblLicBbddValor.Text = "Info No. Licencias Instalación BBDD";
            // 
            // lblLicBbdd
            // 
            this.lblLicBbdd.AutoSize = true;
            this.lblLicBbdd.Location = new System.Drawing.Point(18, 117);
            this.lblLicBbdd.Name = "lblLicBbdd";
            this.lblLicBbdd.Size = new System.Drawing.Size(77, 13);
            this.lblLicBbdd.TabIndex = 6;
            this.lblLicBbdd.Text = "No. Lic. BBDD";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Info Identificador disco";
            // 
            // lblIdDisco
            // 
            this.lblIdDisco.AutoSize = true;
            this.lblIdDisco.Location = new System.Drawing.Point(18, 92);
            this.lblIdDisco.Name = "lblIdDisco";
            this.lblIdDisco.Size = new System.Drawing.Size(49, 13);
            this.lblIdDisco.TabIndex = 4;
            this.lblIdDisco.Text = "Id. Disco";
            // 
            // lblCaducidadValor
            // 
            this.lblCaducidadValor.AutoSize = true;
            this.lblCaducidadValor.Location = new System.Drawing.Point(112, 66);
            this.lblCaducidadValor.Name = "lblCaducidadValor";
            this.lblCaducidadValor.Size = new System.Drawing.Size(82, 13);
            this.lblCaducidadValor.TabIndex = 3;
            this.lblCaducidadValor.Text = "Info  Caducidad";
            // 
            // lblCaducidad
            // 
            this.lblCaducidad.AutoSize = true;
            this.lblCaducidad.Location = new System.Drawing.Point(18, 66);
            this.lblCaducidad.Name = "lblCaducidad";
            this.lblCaducidad.Size = new System.Drawing.Size(58, 13);
            this.lblCaducidad.TabIndex = 2;
            this.lblCaducidad.Text = "Caducidad";
            // 
            // lblClienteValor
            // 
            this.lblClienteValor.AutoSize = true;
            this.lblClienteValor.Location = new System.Drawing.Point(112, 39);
            this.lblClienteValor.Name = "lblClienteValor";
            this.lblClienteValor.Size = new System.Drawing.Size(60, 13);
            this.lblClienteValor.TabIndex = 1;
            this.lblClienteValor.Text = "Info Cliente";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(18, 39);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(39, 13);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente";
            // 
            // frmClienteConsultarClave
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(467, 616);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblClave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClienteConsultarClave";
            this.Text = "Consultar Clave";
            this.Load += new System.EventHandler(this.frmClienteConsultarClave_Load);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label lblClienteValor;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label lblCaducidadValor;
        private System.Windows.Forms.Label lblCaducidad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIdDisco;
        private System.Windows.Forms.Label lblLicBbddValor;
        private System.Windows.Forms.Label lblLicBbdd;
        private System.Windows.Forms.Label lblLicUsuariosValor;
        private System.Windows.Forms.Label lblLicUsuarios;
        private System.Windows.Forms.Label lblFechaValor;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.ListBox lbModulos;
        private System.Windows.Forms.Label lblModulos;
    }
}