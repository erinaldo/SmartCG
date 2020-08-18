namespace AppAdminModulos
{
    partial class frmModuloGenerarFichero
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModuloGenerarFichero));
            this.lblSeleccionar = new System.Windows.Forms.Label();
            this.dataGridViewModulos = new System.Windows.Forms.DataGridView();
            this.chkEncriptar = new System.Windows.Forms.CheckBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnViewFichero = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModulos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSeleccionar
            // 
            this.lblSeleccionar.AutoSize = true;
            this.lblSeleccionar.Location = new System.Drawing.Point(23, 21);
            this.lblSeleccionar.Name = "lblSeleccionar";
            this.lblSeleccionar.Size = new System.Drawing.Size(121, 13);
            this.lblSeleccionar.TabIndex = 0;
            this.lblSeleccionar.Text = "Seleccionar los módulos";
            // 
            // dataGridViewModulos
            // 
            this.dataGridViewModulos.AllowUserToAddRows = false;
            this.dataGridViewModulos.AllowUserToDeleteRows = false;
            this.dataGridViewModulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModulos.Location = new System.Drawing.Point(35, 46);
            this.dataGridViewModulos.Name = "dataGridViewModulos";
            this.dataGridViewModulos.Size = new System.Drawing.Size(540, 323);
            this.dataGridViewModulos.TabIndex = 1;
            // 
            // chkEncriptar
            // 
            this.chkEncriptar.AutoSize = true;
            this.chkEncriptar.Checked = true;
            this.chkEncriptar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEncriptar.Location = new System.Drawing.Point(26, 388);
            this.chkEncriptar.Name = "chkEncriptar";
            this.chkEncriptar.Size = new System.Drawing.Size(103, 17);
            this.chkEncriptar.TabIndex = 2;
            this.chkEncriptar.Text = "Encriptar fichero";
            this.chkEncriptar.UseVisualStyleBackColor = true;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(191, 416);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(328, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(23, 477);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(175, 13);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "El fichero se generó correctamente.";
            this.lblResult.Visible = false;
            // 
            // btnViewFichero
            // 
            this.btnViewFichero.Location = new System.Drawing.Point(231, 472);
            this.btnViewFichero.Name = "btnViewFichero";
            this.btnViewFichero.Size = new System.Drawing.Size(141, 22);
            this.btnViewFichero.TabIndex = 7;
            this.btnViewFichero.Text = "Ver Fichero";
            this.btnViewFichero.UseVisualStyleBackColor = true;
            this.btnViewFichero.Visible = false;
            this.btnViewFichero.Click += new System.EventHandler(this.btnNoEncriptado_Click);
            // 
            // frmModuloGenerarFichero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(608, 524);
            this.Controls.Add(this.btnViewFichero);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.chkEncriptar);
            this.Controls.Add(this.dataGridViewModulos);
            this.Controls.Add(this.lblSeleccionar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmModuloGenerarFichero";
            this.Text = "Generar fichero con los módulos  para clientes";
            this.Load += new System.EventHandler(this.frmModuloGenerarFichero_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModulos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSeleccionar;
        private System.Windows.Forms.DataGridView dataGridViewModulos;
        private System.Windows.Forms.CheckBox chkEncriptar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnViewFichero;
    }
}