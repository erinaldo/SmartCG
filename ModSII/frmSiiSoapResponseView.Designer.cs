namespace ModSII
{
    partial class frmSiiSoapResponseView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSiiSoapResponseView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.btnView = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.btnSelSoapRespuesta = new System.Windows.Forms.Button();
            this.txtFicheroSoapRespuesta = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gbDatosGrles = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCSV = new System.Windows.Forms.TextBox();
            this.txtCIFPresentador = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFechaHoraPresentacion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNombreRazonSocial = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVersionSII = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.gbDatosGrles.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(987, 25);
            this.toolStrip1.TabIndex = 79;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSalir.Image")));
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonSalir.Text = "&Salir";
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(727, 57);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "Ver";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(39, 63);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(96, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "Fichero Respuesta";
            // 
            // btnSelSoapRespuesta
            // 
            this.btnSelSoapRespuesta.Image = ((System.Drawing.Image)(resources.GetObject("btnSelSoapRespuesta.Image")));
            this.btnSelSoapRespuesta.Location = new System.Drawing.Point(600, 54);
            this.btnSelSoapRespuesta.Name = "btnSelSoapRespuesta";
            this.btnSelSoapRespuesta.Size = new System.Drawing.Size(39, 31);
            this.btnSelSoapRespuesta.TabIndex = 81;
            this.btnSelSoapRespuesta.UseVisualStyleBackColor = true;
            this.btnSelSoapRespuesta.Click += new System.EventHandler(this.btnSelSoapRespuesta_Click);
            // 
            // txtFicheroSoapRespuesta
            // 
            this.txtFicheroSoapRespuesta.Location = new System.Drawing.Point(154, 59);
            this.txtFicheroSoapRespuesta.Name = "txtFicheroSoapRespuesta";
            this.txtFicheroSoapRespuesta.Size = new System.Drawing.Size(425, 20);
            this.txtFicheroSoapRespuesta.TabIndex = 80;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // gbDatosGrles
            // 
            this.gbDatosGrles.Controls.Add(this.txtVersionSII);
            this.gbDatosGrles.Controls.Add(this.label5);
            this.gbDatosGrles.Controls.Add(this.txtNombreRazonSocial);
            this.gbDatosGrles.Controls.Add(this.label4);
            this.gbDatosGrles.Controls.Add(this.txtFechaHoraPresentacion);
            this.gbDatosGrles.Controls.Add(this.label3);
            this.gbDatosGrles.Controls.Add(this.txtCIFPresentador);
            this.gbDatosGrles.Controls.Add(this.label2);
            this.gbDatosGrles.Controls.Add(this.txtCSV);
            this.gbDatosGrles.Controls.Add(this.label1);
            this.gbDatosGrles.Location = new System.Drawing.Point(42, 107);
            this.gbDatosGrles.Name = "gbDatosGrles";
            this.gbDatosGrles.Size = new System.Drawing.Size(903, 149);
            this.gbDatosGrles.TabIndex = 82;
            this.gbDatosGrles.TabStop = false;
            this.gbDatosGrles.Text = " Datos Generales del Envío ";
            this.gbDatosGrles.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CSV";
            // 
            // txtCSV
            // 
            this.txtCSV.Location = new System.Drawing.Point(112, 29);
            this.txtCSV.Name = "txtCSV";
            this.txtCSV.ReadOnly = true;
            this.txtCSV.Size = new System.Drawing.Size(298, 20);
            this.txtCSV.TabIndex = 1;
            // 
            // txtCIFPresentador
            // 
            this.txtCIFPresentador.Location = new System.Drawing.Point(112, 65);
            this.txtCIFPresentador.Name = "txtCIFPresentador";
            this.txtCIFPresentador.ReadOnly = true;
            this.txtCIFPresentador.Size = new System.Drawing.Size(130, 20);
            this.txtCIFPresentador.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "CIF Presentador";
            // 
            // txtFechaHoraPresentacion
            // 
            this.txtFechaHoraPresentacion.Location = new System.Drawing.Point(112, 108);
            this.txtFechaHoraPresentacion.Name = "txtFechaHoraPresentacion";
            this.txtFechaHoraPresentacion.ReadOnly = true;
            this.txtFechaHoraPresentacion.Size = new System.Drawing.Size(214, 20);
            this.txtFechaHoraPresentacion.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fecha/Hora envío";
            // 
            // txtNombreRazonSocial
            // 
            this.txtNombreRazonSocial.Location = new System.Drawing.Point(423, 68);
            this.txtNombreRazonSocial.Name = "txtNombreRazonSocial";
            this.txtNombreRazonSocial.ReadOnly = true;
            this.txtNombreRazonSocial.Size = new System.Drawing.Size(463, 20);
            this.txtNombreRazonSocial.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(297, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Nombre o Razón Social";
            // 
            // txtVersionSII
            // 
            this.txtVersionSII.Location = new System.Drawing.Point(423, 112);
            this.txtVersionSII.Name = "txtVersionSII";
            this.txtVersionSII.ReadOnly = true;
            this.txtVersionSII.Size = new System.Drawing.Size(40, 20);
            this.txtVersionSII.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(341, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Versión del SII";
            // 
            // frmSiiSoapResponseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 433);
            this.Controls.Add(this.gbDatosGrles);
            this.Controls.Add(this.btnSelSoapRespuesta);
            this.Controls.Add(this.txtFicheroSoapRespuesta);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.lblFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSiiSoapResponseView";
            this.Text = "Visualizar SOAP Respuesta Suministro AEAT";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbDatosGrles.ResumeLayout(false);
            this.gbDatosGrles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.Button btnSelSoapRespuesta;
        private System.Windows.Forms.TextBox txtFicheroSoapRespuesta;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox gbDatosGrles;
        private System.Windows.Forms.TextBox txtNombreRazonSocial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFechaHoraPresentacion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCIFPresentador;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCSV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVersionSII;
        private System.Windows.Forms.Label label5;
    }
}