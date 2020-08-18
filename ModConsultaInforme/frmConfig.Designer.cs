namespace ModConsultaInforme
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
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.radOpenFolderDialog1 = new Telerik.WinControls.UI.RadOpenFolderDialog();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radDropDownListTipoArchivo = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabelTipoArchivo = new Telerik.WinControls.UI.RadLabel();
            this.btnSeRutaConsulta = new System.Windows.Forms.Button();
            this.radButtonSelDirectorio = new Telerik.WinControls.UI.RadButton();
            this.radLabelRutaArchivos = new Telerik.WinControls.UI.RadLabel();
            this.txtRutaArchivo = new Telerik.WinControls.UI.RadTextBox();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListTipoArchivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTipoArchivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelDirectorio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaArchivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRutaArchivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radDropDownListTipoArchivo);
            this.radPanelApp.Controls.Add(this.radLabelTipoArchivo);
            this.radPanelApp.Controls.Add(this.btnSeRutaConsulta);
            this.radPanelApp.Controls.Add(this.radButtonSelDirectorio);
            this.radPanelApp.Controls.Add(this.radLabelRutaArchivos);
            this.radPanelApp.Controls.Add(this.txtRutaArchivo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(853, 752);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radDropDownListTipoArchivo
            // 
            this.radDropDownListTipoArchivo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.radDropDownListTipoArchivo.Location = new System.Drawing.Point(10, 150);
            this.radDropDownListTipoArchivo.Name = "radDropDownListTipoArchivo";
            this.radDropDownListTipoArchivo.Padding = new System.Windows.Forms.Padding(3);
            this.radDropDownListTipoArchivo.Size = new System.Drawing.Size(218, 27);
            this.radDropDownListTipoArchivo.TabIndex = 30;
            // 
            // radLabelTipoArchivo
            // 
            this.radLabelTipoArchivo.Location = new System.Drawing.Point(10, 115);
            this.radLabelTipoArchivo.Name = "radLabelTipoArchivo";
            this.radLabelTipoArchivo.Size = new System.Drawing.Size(92, 19);
            this.radLabelTipoArchivo.TabIndex = 25;
            this.radLabelTipoArchivo.Text = "Tipo de Archivo";
            // 
            // btnSeRutaConsulta
            // 
            this.btnSeRutaConsulta.Image = ((System.Drawing.Image)(resources.GetObject("btnSeRutaConsulta.Image")));
            this.btnSeRutaConsulta.Location = new System.Drawing.Point(718, 51);
            this.btnSeRutaConsulta.Name = "btnSeRutaConsulta";
            this.btnSeRutaConsulta.Size = new System.Drawing.Size(39, 31);
            this.btnSeRutaConsulta.TabIndex = 60;
            this.btnSeRutaConsulta.UseVisualStyleBackColor = true;
            this.btnSeRutaConsulta.Visible = false;
            // 
            // radButtonSelDirectorio
            // 
            this.radButtonSelDirectorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSelDirectorio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radButtonSelDirectorio.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSelDirectorio.Location = new System.Drawing.Point(526, 51);
            this.radButtonSelDirectorio.Name = "radButtonSelDirectorio";
            this.radButtonSelDirectorio.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonSelDirectorio.Size = new System.Drawing.Size(186, 27);
            this.radButtonSelDirectorio.TabIndex = 20;
            this.radButtonSelDirectorio.Text = "Seleccionar directorio";
            this.radButtonSelDirectorio.Click += new System.EventHandler(this.RadButtonSelDirectorio_Click);
            this.radButtonSelDirectorio.MouseEnter += new System.EventHandler(this.RadButtonSelDirectorio_MouseEnter);
            this.radButtonSelDirectorio.MouseLeave += new System.EventHandler(this.RadButtonSelDirectorio_MouseLeave);
            // 
            // radLabelRutaArchivos
            // 
            this.radLabelRutaArchivos.Location = new System.Drawing.Point(10, 16);
            this.radLabelRutaArchivos.Name = "radLabelRutaArchivos";
            this.radLabelRutaArchivos.Size = new System.Drawing.Size(110, 19);
            this.radLabelRutaArchivos.TabIndex = 10;
            this.radLabelRutaArchivos.Text = "Directorio Archivos";
            // 
            // txtRutaArchivo
            // 
            this.txtRutaArchivo.Location = new System.Drawing.Point(10, 51);
            this.txtRutaArchivo.Name = "txtRutaArchivo";
            this.txtRutaArchivo.Padding = new System.Windows.Forms.Padding(5);
            this.txtRutaArchivo.Size = new System.Drawing.Size(496, 28);
            this.txtRutaArchivo.TabIndex = 15;
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1028, 45);
            this.radPanelMenuPath.TabIndex = 45;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(325, 29);
            this.radLabelTitulo.TabIndex = 50;
            this.radLabelTitulo.Text = "Consultas o Informes / Configuración";
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonSave);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 735);
            this.radPanelAcciones.TabIndex = 35;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(13, 16);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 44);
            this.radButtonSave.TabIndex = 40;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.RadButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.RadButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.RadButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1184, 788);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelMenuPath);
            this.Controls.Add(this.radPanelAcciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmConfig";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Configuración";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListTipoArchivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTipoArchivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelDirectorio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaArchivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRutaArchivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSeRutaConsulta;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadButton radButtonSelDirectorio;
        private Telerik.WinControls.UI.RadLabel radLabelRutaArchivos;
        private Telerik.WinControls.UI.RadTextBox txtRutaArchivo;
        private Telerik.WinControls.UI.RadLabel radLabelTipoArchivo;
        private Telerik.WinControls.UI.RadOpenFolderDialog radOpenFolderDialog1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListTipoArchivo;
    }
}