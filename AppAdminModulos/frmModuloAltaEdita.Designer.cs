namespace AppAdminModulos
{
    partial class frmModuloAltaEdita
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModuloAltaEdita));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.gbDatos = new System.Windows.Forms.GroupBox();
            this.groupBoxActivo = new System.Windows.Forms.GroupBox();
            this.rbActivo = new System.Windows.Forms.RadioButton();
            this.rbActivoNo = new System.Windows.Forms.RadioButton();
            this.groupBoxBasico = new System.Windows.Forms.GroupBox();
            this.rbBasico = new System.Windows.Forms.RadioButton();
            this.rbBasicoNo = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNombreModulo = new System.Windows.Forms.Label();
            this.txtNombreModulo = new System.Windows.Forms.TextBox();
            this.lblNombreDll = new System.Windows.Forms.Label();
            this.lblActivo = new System.Windows.Forms.Label();
            this.txtNombreDll = new System.Windows.Forms.TextBox();
            this.txtImagen = new System.Windows.Forms.TextBox();
            this.lblFormInicio = new System.Windows.Forms.Label();
            this.lblImagen = new System.Windows.Forms.Label();
            this.txtFormInicio = new System.Windows.Forms.TextBox();
            this.lblModulos = new System.Windows.Forms.Label();
            this.cmbModulos = new System.Windows.Forms.ComboBox();
            this.gbDatos.SuspendLayout();
            this.groupBoxActivo.SuspendLayout();
            this.groupBoxBasico.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(367, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(232, 372);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 14;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // gbDatos
            // 
            this.gbDatos.Controls.Add(this.groupBoxActivo);
            this.gbDatos.Controls.Add(this.groupBoxBasico);
            this.gbDatos.Controls.Add(this.label1);
            this.gbDatos.Controls.Add(this.lblNombreModulo);
            this.gbDatos.Controls.Add(this.txtNombreModulo);
            this.gbDatos.Controls.Add(this.lblNombreDll);
            this.gbDatos.Controls.Add(this.lblActivo);
            this.gbDatos.Controls.Add(this.txtNombreDll);
            this.gbDatos.Controls.Add(this.txtImagen);
            this.gbDatos.Controls.Add(this.lblFormInicio);
            this.gbDatos.Controls.Add(this.lblImagen);
            this.gbDatos.Controls.Add(this.txtFormInicio);
            this.gbDatos.Location = new System.Drawing.Point(12, 52);
            this.gbDatos.Name = "gbDatos";
            this.gbDatos.Size = new System.Drawing.Size(635, 265);
            this.gbDatos.TabIndex = 13;
            this.gbDatos.TabStop = false;
            this.gbDatos.Text = "  Datos del Módulo  ";
            // 
            // groupBoxActivo
            // 
            this.groupBoxActivo.Controls.Add(this.rbActivo);
            this.groupBoxActivo.Controls.Add(this.rbActivoNo);
            this.groupBoxActivo.Location = new System.Drawing.Point(144, 216);
            this.groupBoxActivo.Name = "groupBoxActivo";
            this.groupBoxActivo.Size = new System.Drawing.Size(104, 33);
            this.groupBoxActivo.TabIndex = 17;
            this.groupBoxActivo.TabStop = false;
            // 
            // rbActivo
            // 
            this.rbActivo.AutoSize = true;
            this.rbActivo.Location = new System.Drawing.Point(6, 12);
            this.rbActivo.Name = "rbActivo";
            this.rbActivo.Size = new System.Drawing.Size(36, 17);
            this.rbActivo.TabIndex = 11;
            this.rbActivo.TabStop = true;
            this.rbActivo.Text = "Sí";
            this.rbActivo.UseVisualStyleBackColor = true;
            // 
            // rbActivoNo
            // 
            this.rbActivoNo.AutoSize = true;
            this.rbActivoNo.Location = new System.Drawing.Point(63, 12);
            this.rbActivoNo.Name = "rbActivoNo";
            this.rbActivoNo.Size = new System.Drawing.Size(39, 17);
            this.rbActivoNo.TabIndex = 12;
            this.rbActivoNo.TabStop = true;
            this.rbActivoNo.Text = "No";
            this.rbActivoNo.UseVisualStyleBackColor = true;
            // 
            // groupBoxBasico
            // 
            this.groupBoxBasico.Controls.Add(this.rbBasico);
            this.groupBoxBasico.Controls.Add(this.rbBasicoNo);
            this.groupBoxBasico.Location = new System.Drawing.Point(144, 179);
            this.groupBoxBasico.Name = "groupBoxBasico";
            this.groupBoxBasico.Size = new System.Drawing.Size(104, 32);
            this.groupBoxBasico.TabIndex = 16;
            this.groupBoxBasico.TabStop = false;
            // 
            // rbBasico
            // 
            this.rbBasico.AutoSize = true;
            this.rbBasico.Location = new System.Drawing.Point(6, 9);
            this.rbBasico.Name = "rbBasico";
            this.rbBasico.Size = new System.Drawing.Size(36, 17);
            this.rbBasico.TabIndex = 14;
            this.rbBasico.TabStop = true;
            this.rbBasico.Text = "Sí";
            this.rbBasico.UseVisualStyleBackColor = true;
            // 
            // rbBasicoNo
            // 
            this.rbBasicoNo.AutoSize = true;
            this.rbBasicoNo.Location = new System.Drawing.Point(63, 9);
            this.rbBasicoNo.Name = "rbBasicoNo";
            this.rbBasicoNo.Size = new System.Drawing.Size(39, 17);
            this.rbBasicoNo.TabIndex = 15;
            this.rbBasicoNo.TabStop = true;
            this.rbBasicoNo.Text = "No";
            this.rbBasicoNo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Módulo básico";
            // 
            // lblNombreModulo
            // 
            this.lblNombreModulo.AutoSize = true;
            this.lblNombreModulo.Location = new System.Drawing.Point(19, 44);
            this.lblNombreModulo.Name = "lblNombreModulo";
            this.lblNombreModulo.Size = new System.Drawing.Size(82, 13);
            this.lblNombreModulo.TabIndex = 2;
            this.lblNombreModulo.Text = "Nombre Módulo";
            // 
            // txtNombreModulo
            // 
            this.txtNombreModulo.Location = new System.Drawing.Point(144, 36);
            this.txtNombreModulo.Name = "txtNombreModulo";
            this.txtNombreModulo.Size = new System.Drawing.Size(462, 20);
            this.txtNombreModulo.TabIndex = 3;
            // 
            // lblNombreDll
            // 
            this.lblNombreDll.AutoSize = true;
            this.lblNombreDll.Location = new System.Drawing.Point(19, 81);
            this.lblNombreDll.Name = "lblNombreDll";
            this.lblNombreDll.Size = new System.Drawing.Size(67, 13);
            this.lblNombreDll.TabIndex = 4;
            this.lblNombreDll.Text = "Nombre DLL";
            // 
            // lblActivo
            // 
            this.lblActivo.AutoSize = true;
            this.lblActivo.Location = new System.Drawing.Point(19, 233);
            this.lblActivo.Name = "lblActivo";
            this.lblActivo.Size = new System.Drawing.Size(37, 13);
            this.lblActivo.TabIndex = 10;
            this.lblActivo.Text = "Activo";
            // 
            // txtNombreDll
            // 
            this.txtNombreDll.Location = new System.Drawing.Point(144, 74);
            this.txtNombreDll.Name = "txtNombreDll";
            this.txtNombreDll.Size = new System.Drawing.Size(462, 20);
            this.txtNombreDll.TabIndex = 5;
            // 
            // txtImagen
            // 
            this.txtImagen.Location = new System.Drawing.Point(144, 152);
            this.txtImagen.Name = "txtImagen";
            this.txtImagen.Size = new System.Drawing.Size(462, 20);
            this.txtImagen.TabIndex = 9;
            // 
            // lblFormInicio
            // 
            this.lblFormInicio.AutoSize = true;
            this.lblFormInicio.Location = new System.Drawing.Point(19, 121);
            this.lblFormInicio.Name = "lblFormInicio";
            this.lblFormInicio.Size = new System.Drawing.Size(83, 13);
            this.lblFormInicio.TabIndex = 6;
            this.lblFormInicio.Text = "Formulario Inicio";
            // 
            // lblImagen
            // 
            this.lblImagen.AutoSize = true;
            this.lblImagen.Location = new System.Drawing.Point(19, 160);
            this.lblImagen.Name = "lblImagen";
            this.lblImagen.Size = new System.Drawing.Size(80, 13);
            this.lblImagen.TabIndex = 8;
            this.lblImagen.Text = "Fichero Imagen";
            // 
            // txtFormInicio
            // 
            this.txtFormInicio.Location = new System.Drawing.Point(144, 114);
            this.txtFormInicio.Name = "txtFormInicio";
            this.txtFormInicio.Size = new System.Drawing.Size(462, 20);
            this.txtFormInicio.TabIndex = 7;
            // 
            // lblModulos
            // 
            this.lblModulos.AutoSize = true;
            this.lblModulos.Location = new System.Drawing.Point(31, 20);
            this.lblModulos.Name = "lblModulos";
            this.lblModulos.Size = new System.Drawing.Size(47, 13);
            this.lblModulos.TabIndex = 1;
            this.lblModulos.Text = "Módulos";
            // 
            // cmbModulos
            // 
            this.cmbModulos.FormattingEnabled = true;
            this.cmbModulos.Location = new System.Drawing.Point(156, 13);
            this.cmbModulos.Name = "cmbModulos";
            this.cmbModulos.Size = new System.Drawing.Size(462, 21);
            this.cmbModulos.TabIndex = 0;
            this.cmbModulos.SelectedIndexChanged += new System.EventHandler(this.cmbModulos_SelectedIndexChanged);
            // 
            // frmModuloAltaEdita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(659, 413);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.gbDatos);
            this.Controls.Add(this.lblModulos);
            this.Controls.Add(this.cmbModulos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmModuloAltaEdita";
            this.Text = "frmModuloAltaEdita";
            this.Load += new System.EventHandler(this.frmModuloAltaEdita_Load);
            this.gbDatos.ResumeLayout(false);
            this.gbDatos.PerformLayout();
            this.groupBoxActivo.ResumeLayout(false);
            this.groupBoxActivo.PerformLayout();
            this.groupBoxBasico.ResumeLayout(false);
            this.groupBoxBasico.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbModulos;
        private System.Windows.Forms.Label lblModulos;
        private System.Windows.Forms.Label lblNombreModulo;
        private System.Windows.Forms.TextBox txtNombreModulo;
        private System.Windows.Forms.Label lblNombreDll;
        private System.Windows.Forms.TextBox txtNombreDll;
        private System.Windows.Forms.Label lblFormInicio;
        private System.Windows.Forms.TextBox txtFormInicio;
        private System.Windows.Forms.Label lblImagen;
        private System.Windows.Forms.TextBox txtImagen;
        private System.Windows.Forms.Label lblActivo;
        private System.Windows.Forms.RadioButton rbActivo;
        private System.Windows.Forms.RadioButton rbActivoNo;
        private System.Windows.Forms.GroupBox gbDatos;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbBasicoNo;
        private System.Windows.Forms.RadioButton rbBasico;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxBasico;
        private System.Windows.Forms.GroupBox groupBoxActivo;
    }
}