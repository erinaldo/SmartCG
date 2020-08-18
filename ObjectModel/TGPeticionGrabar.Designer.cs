namespace ObjectModel
{
    partial class TGPeticionGrabar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGPeticionGrabar));
            this.btnSelFichero = new System.Windows.Forms.Button();
            this.txtFichero = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblRutaConsulta = new Telerik.WinControls.UI.RadLabel();
            this.label1 = new Telerik.WinControls.UI.RadLabel();
            this.txtDescripcion = new Telerik.WinControls.UI.RadTextBoxControl();
            this.saveFileDialogGrabar = new System.Windows.Forms.SaveFileDialog();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radButtonSelNombreArchivo = new Telerik.WinControls.UI.RadButton();
            this.radOpenFolderDialog1 = new Telerik.WinControls.UI.RadOpenFolderDialog();
            this.radSaveFileDialogGuardar = new Telerik.WinControls.UI.RadSaveFileDialog();
            this.radLabelDirectorio = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxControlDirectorio = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxControlExtension = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radButtonSelDirectorio = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtFichero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRutaConsulta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelNombreArchivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelDirectorio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlDirectorio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlExtension)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelDirectorio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelFichero
            // 
            this.btnSelFichero.Image = ((System.Drawing.Image)(resources.GetObject("btnSelFichero.Image")));
            this.btnSelFichero.Location = new System.Drawing.Point(747, 111);
            this.btnSelFichero.Name = "btnSelFichero";
            this.btnSelFichero.Size = new System.Drawing.Size(39, 31);
            this.btnSelFichero.TabIndex = 170;
            this.btnSelFichero.UseVisualStyleBackColor = true;
            this.btnSelFichero.Visible = false;
            this.btnSelFichero.Click += new System.EventHandler(this.btnSelFichero_Click);
            // 
            // txtFichero
            // 
            this.txtFichero.Location = new System.Drawing.Point(104, 147);
            this.txtFichero.Name = "txtFichero";
            this.txtFichero.Padding = new System.Windows.Forms.Padding(5);
            this.txtFichero.Size = new System.Drawing.Size(496, 30);
            this.txtFichero.TabIndex = 169;
            this.txtFichero.TextChanged += new System.EventHandler(this.txtFichero_TextChanged);
            // 
            // lblRutaConsulta
            // 
            this.lblRutaConsulta.Location = new System.Drawing.Point(104, 122);
            this.lblRutaConsulta.Name = "lblRutaConsulta";
            this.lblRutaConsulta.Size = new System.Drawing.Size(96, 19);
            this.lblRutaConsulta.TabIndex = 168;
            this.lblRutaConsulta.Text = "Nombre Archivo";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(104, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 19);
            this.label1.TabIndex = 171;
            this.label1.Text = "Descripción";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(104, 226);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Padding = new System.Windows.Forms.Padding(5);
            this.txtDescripcion.Size = new System.Drawing.Size(496, 30);
            this.txtDescripcion.TabIndex = 172;
            // 
            // saveFileDialogGrabar
            // 
            this.saveFileDialogGrabar.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogGrabar_FileOk);
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(462, 312);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(138, 44);
            this.radButtonExit.TabIndex = 174;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.radButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.radButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.radButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(268, 312);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 44);
            this.radButtonSave.TabIndex = 173;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.radButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.radButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.radButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonSelNombreArchivo
            // 
            this.radButtonSelNombreArchivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSelNombreArchivo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radButtonSelNombreArchivo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSelNombreArchivo.Location = new System.Drawing.Point(627, 148);
            this.radButtonSelNombreArchivo.Name = "radButtonSelNombreArchivo";
            this.radButtonSelNombreArchivo.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonSelNombreArchivo.Size = new System.Drawing.Size(162, 27);
            this.radButtonSelNombreArchivo.TabIndex = 175;
            this.radButtonSelNombreArchivo.Text = "Seleccionar archivo";
            this.radButtonSelNombreArchivo.Click += new System.EventHandler(this.radButtonSelNombreArchivo_Click);
            this.radButtonSelNombreArchivo.MouseEnter += new System.EventHandler(this.radButtonSelNombreArchivo_MouseEnter);
            this.radButtonSelNombreArchivo.MouseLeave += new System.EventHandler(this.radButtonSelNombreArchivo_MouseLeave);
            // 
            // radLabelDirectorio
            // 
            this.radLabelDirectorio.Location = new System.Drawing.Point(104, 42);
            this.radLabelDirectorio.Name = "radLabelDirectorio";
            this.radLabelDirectorio.Size = new System.Drawing.Size(61, 19);
            this.radLabelDirectorio.TabIndex = 176;
            this.radLabelDirectorio.Text = "Directorio";
            // 
            // radTextBoxControlDirectorio
            // 
            this.radTextBoxControlDirectorio.Enabled = false;
            this.radTextBoxControlDirectorio.Location = new System.Drawing.Point(104, 67);
            this.radTextBoxControlDirectorio.Name = "radTextBoxControlDirectorio";
            this.radTextBoxControlDirectorio.Padding = new System.Windows.Forms.Padding(5);
            this.radTextBoxControlDirectorio.Size = new System.Drawing.Size(496, 30);
            this.radTextBoxControlDirectorio.TabIndex = 177;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(627, 201);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(59, 19);
            this.radLabel1.TabIndex = 178;
            this.radLabel1.Text = "Extensión";
            // 
            // radTextBoxControlExtension
            // 
            this.radTextBoxControlExtension.Enabled = false;
            this.radTextBoxControlExtension.Location = new System.Drawing.Point(627, 226);
            this.radTextBoxControlExtension.Name = "radTextBoxControlExtension";
            this.radTextBoxControlExtension.Padding = new System.Windows.Forms.Padding(5);
            this.radTextBoxControlExtension.Size = new System.Drawing.Size(71, 30);
            this.radTextBoxControlExtension.TabIndex = 179;
            // 
            // radButtonSelDirectorio
            // 
            this.radButtonSelDirectorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSelDirectorio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radButtonSelDirectorio.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSelDirectorio.Location = new System.Drawing.Point(624, 67);
            this.radButtonSelDirectorio.Name = "radButtonSelDirectorio";
            this.radButtonSelDirectorio.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonSelDirectorio.Size = new System.Drawing.Size(162, 27);
            this.radButtonSelDirectorio.TabIndex = 180;
            this.radButtonSelDirectorio.Text = "Seleccionar directorio";
            this.radButtonSelDirectorio.Click += new System.EventHandler(this.radButtonSelDirectorio_Click);
            this.radButtonSelDirectorio.MouseEnter += new System.EventHandler(this.radButtonSelDirectorio_MouseEnter);
            this.radButtonSelDirectorio.MouseLeave += new System.EventHandler(this.radButtonSelDirectorio_MouseLeave);
            // 
            // TGPeticionGrabar
            // 
            this.AcceptButton = this.radButtonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.CancelButton = this.radButtonExit;
            this.ClientSize = new System.Drawing.Size(895, 389);
            this.Controls.Add(this.radButtonSelDirectorio);
            this.Controls.Add(this.radTextBoxControlExtension);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radTextBoxControlDirectorio);
            this.Controls.Add(this.radLabelDirectorio);
            this.Controls.Add(this.radButtonSelNombreArchivo);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelFichero);
            this.Controls.Add(this.txtFichero);
            this.Controls.Add(this.lblRutaConsulta);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TGPeticionGrabar";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guardar Petición";
            this.Load += new System.EventHandler(this.TGPeticionGrabar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TGPeticionGrabar_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtFichero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRutaConsulta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelNombreArchivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelDirectorio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlDirectorio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlExtension)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelDirectorio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSelFichero;
        private Telerik.WinControls.UI.RadTextBoxControl txtFichero;
        private Telerik.WinControls.UI.RadLabel lblRutaConsulta;
        private Telerik.WinControls.UI.RadLabel label1;
        private Telerik.WinControls.UI.RadTextBoxControl txtDescripcion;
        private System.Windows.Forms.SaveFileDialog saveFileDialogGrabar;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadButton radButtonSelNombreArchivo;
        private Telerik.WinControls.UI.RadOpenFolderDialog radOpenFolderDialog1;
        private Telerik.WinControls.UI.RadSaveFileDialog radSaveFileDialogGuardar;
        private Telerik.WinControls.UI.RadLabel radLabelDirectorio;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlDirectorio;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlExtension;
        private Telerik.WinControls.UI.RadButton radButtonSelDirectorio;
    }
}