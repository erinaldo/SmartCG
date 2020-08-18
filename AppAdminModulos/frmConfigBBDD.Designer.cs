namespace AppAdminModulos
{
    partial class frmConfigBBDD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigBBDD));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.gbCadenaConexion = new System.Windows.Forms.GroupBox();
            this.txtCadenaConexion = new System.Windows.Forms.TextBox();
            this.txtNombreBbdd = new System.Windows.Forms.TextBox();
            this.lblNombreBbdd = new System.Windows.Forms.Label();
            this.txtIpNombreServidor = new System.Windows.Forms.TextBox();
            this.lblIpNombreServidor = new System.Windows.Forms.Label();
            this.cmbTipoAcceso = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.gbCadenaConexion.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(238, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(105, 369);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 13;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // gbCadenaConexion
            // 
            this.gbCadenaConexion.Controls.Add(this.txtCadenaConexion);
            this.gbCadenaConexion.Location = new System.Drawing.Point(34, 198);
            this.gbCadenaConexion.Name = "gbCadenaConexion";
            this.gbCadenaConexion.Size = new System.Drawing.Size(343, 148);
            this.gbCadenaConexion.TabIndex = 12;
            this.gbCadenaConexion.TabStop = false;
            this.gbCadenaConexion.Text = " Cadena de conexión ";
            // 
            // txtCadenaConexion
            // 
            this.txtCadenaConexion.Location = new System.Drawing.Point(23, 31);
            this.txtCadenaConexion.Multiline = true;
            this.txtCadenaConexion.Name = "txtCadenaConexion";
            this.txtCadenaConexion.ReadOnly = true;
            this.txtCadenaConexion.Size = new System.Drawing.Size(307, 99);
            this.txtCadenaConexion.TabIndex = 0;
            // 
            // txtNombreBbdd
            // 
            this.txtNombreBbdd.Location = new System.Drawing.Point(165, 159);
            this.txtNombreBbdd.Name = "txtNombreBbdd";
            this.txtNombreBbdd.Size = new System.Drawing.Size(188, 20);
            this.txtNombreBbdd.TabIndex = 8;
            // 
            // lblNombreBbdd
            // 
            this.lblNombreBbdd.AutoSize = true;
            this.lblNombreBbdd.Location = new System.Drawing.Point(44, 162);
            this.lblNombreBbdd.Name = "lblNombreBbdd";
            this.lblNombreBbdd.Size = new System.Drawing.Size(74, 13);
            this.lblNombreBbdd.TabIndex = 6;
            this.lblNombreBbdd.Text = "Nombre bbdd:";
            // 
            // txtIpNombreServidor
            // 
            this.txtIpNombreServidor.Location = new System.Drawing.Point(165, 122);
            this.txtIpNombreServidor.Name = "txtIpNombreServidor";
            this.txtIpNombreServidor.Size = new System.Drawing.Size(188, 20);
            this.txtIpNombreServidor.TabIndex = 5;
            // 
            // lblIpNombreServidor
            // 
            this.lblIpNombreServidor.AutoSize = true;
            this.lblIpNombreServidor.Location = new System.Drawing.Point(44, 125);
            this.lblIpNombreServidor.Name = "lblIpNombreServidor";
            this.lblIpNombreServidor.Size = new System.Drawing.Size(108, 13);
            this.lblIpNombreServidor.TabIndex = 4;
            this.lblIpNombreServidor.Text = "Ip o nombre Servidor:";
            // 
            // cmbTipoAcceso
            // 
            this.cmbTipoAcceso.FormattingEnabled = true;
            this.cmbTipoAcceso.Location = new System.Drawing.Point(165, 83);
            this.cmbTipoAcceso.Name = "cmbTipoAcceso";
            this.cmbTipoAcceso.Size = new System.Drawing.Size(188, 21);
            this.cmbTipoAcceso.TabIndex = 3;
            this.cmbTipoAcceso.SelectedIndexChanged += new System.EventHandler(this.cmbTipoAcceso_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Vía de Acceso:";
            // 
            // cmbTipo
            // 
            this.cmbTipo.FormattingEnabled = true;
            this.cmbTipo.Location = new System.Drawing.Point(165, 43);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(188, 21);
            this.cmbTipo.TabIndex = 1;
            this.cmbTipo.SelectedIndexChanged += new System.EventHandler(this.cmbTipo_SelectedIndexChanged);
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(44, 49);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(31, 13);
            this.lblTipo.TabIndex = 0;
            this.lblTipo.Text = "Tipo:";
            // 
            // frmConfigBBDD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(408, 413);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.gbCadenaConexion);
            this.Controls.Add(this.txtNombreBbdd);
            this.Controls.Add(this.lblNombreBbdd);
            this.Controls.Add(this.txtIpNombreServidor);
            this.Controls.Add(this.lblIpNombreServidor);
            this.Controls.Add(this.cmbTipoAcceso);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbTipo);
            this.Controls.Add(this.lblTipo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfigBBDD";
            this.Text = "Configuración Base de Datos";
            this.Load += new System.EventHandler(this.frmConfigBBDD_Load);
            this.gbCadenaConexion.ResumeLayout(false);
            this.gbCadenaConexion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTipoAcceso;
        private System.Windows.Forms.Label lblIpNombreServidor;
        private System.Windows.Forms.TextBox txtIpNombreServidor;
        private System.Windows.Forms.Label lblNombreBbdd;
        private System.Windows.Forms.TextBox txtNombreBbdd;
        private System.Windows.Forms.GroupBox gbCadenaConexion;
        private System.Windows.Forms.TextBox txtCadenaConexion;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancel;
    }
}