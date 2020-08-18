namespace ModComprobantes
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
            this.btnSelModeloCont = new System.Windows.Forms.Button();
            this.txtModelo = new Telerik.WinControls.UI.RadTextBox();
            this.btnSelCompContable = new System.Windows.Forms.Button();
            this.txtComprobante = new Telerik.WinControls.UI.RadTextBox();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radButtonSelModelos = new Telerik.WinControls.UI.RadButton();
            this.radButtonSelComprobantes = new Telerik.WinControls.UI.RadButton();
            this.radLabelRutaModelos = new Telerik.WinControls.UI.RadLabel();
            this.radLabelRutaArchivos = new Telerik.WinControls.UI.RadLabel();
            this.radOpenFolderDialog1 = new Telerik.WinControls.UI.RadOpenFolderDialog();
            ((System.ComponentModel.ISupportInitialize)(this.txtModelo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelModelos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelComprobantes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaModelos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaArchivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelModeloCont
            // 
            this.btnSelModeloCont.Image = ((System.Drawing.Image)(resources.GetObject("btnSelModeloCont.Image")));
            this.btnSelModeloCont.Location = new System.Drawing.Point(742, 146);
            this.btnSelModeloCont.Name = "btnSelModeloCont";
            this.btnSelModeloCont.Size = new System.Drawing.Size(39, 31);
            this.btnSelModeloCont.TabIndex = 85;
            this.btnSelModeloCont.UseVisualStyleBackColor = true;
            this.btnSelModeloCont.Visible = false;
            this.btnSelModeloCont.Click += new System.EventHandler(this.BtnSelModeloCont_Click);
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new System.Drawing.Point(10, 149);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Padding = new System.Windows.Forms.Padding(5);
            this.txtModelo.Size = new System.Drawing.Size(496, 28);
            this.txtModelo.TabIndex = 30;
            // 
            // btnSelCompContable
            // 
            this.btnSelCompContable.Image = ((System.Drawing.Image)(resources.GetObject("btnSelCompContable.Image")));
            this.btnSelCompContable.Location = new System.Drawing.Point(742, 47);
            this.btnSelCompContable.Name = "btnSelCompContable";
            this.btnSelCompContable.Size = new System.Drawing.Size(39, 31);
            this.btnSelCompContable.TabIndex = 80;
            this.btnSelCompContable.UseVisualStyleBackColor = true;
            this.btnSelCompContable.Visible = false;
            this.btnSelCompContable.Click += new System.EventHandler(this.BtnSelCompContable_Click);
            // 
            // txtComprobante
            // 
            this.txtComprobante.Location = new System.Drawing.Point(10, 51);
            this.txtComprobante.Name = "txtComprobante";
            this.txtComprobante.Padding = new System.Windows.Forms.Padding(5);
            this.txtComprobante.Size = new System.Drawing.Size(496, 28);
            this.txtComprobante.TabIndex = 15;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonSave);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 473);
            this.radPanelAcciones.TabIndex = 40;
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
            this.radButtonSave.TabIndex = 45;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.RadButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.RadButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.RadButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1011, 45);
            this.radPanelMenuPath.TabIndex = 50;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(358, 29);
            this.radLabelTitulo.TabIndex = 55;
            this.radLabelTitulo.Text = "Comprobantes contables / Configuración";
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radButtonSelModelos);
            this.radPanelApp.Controls.Add(this.radButtonSelComprobantes);
            this.radPanelApp.Controls.Add(this.radLabelRutaModelos);
            this.radPanelApp.Controls.Add(this.btnSelModeloCont);
            this.radPanelApp.Controls.Add(this.radLabelRutaArchivos);
            this.radPanelApp.Controls.Add(this.txtModelo);
            this.radPanelApp.Controls.Add(this.btnSelCompContable);
            this.radPanelApp.Controls.Add(this.txtComprobante);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 490);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonSelModelos
            // 
            this.radButtonSelModelos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSelModelos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radButtonSelModelos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSelModelos.Location = new System.Drawing.Point(526, 150);
            this.radButtonSelModelos.Name = "radButtonSelModelos";
            this.radButtonSelModelos.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonSelModelos.Size = new System.Drawing.Size(186, 27);
            this.radButtonSelModelos.TabIndex = 35;
            this.radButtonSelModelos.Text = "Seleccionar directorio";
            this.radButtonSelModelos.Click += new System.EventHandler(this.RadButtonSelModelos_Click);
            this.radButtonSelModelos.MouseEnter += new System.EventHandler(this.RadButtonSelModelos_MouseEnter);
            this.radButtonSelModelos.MouseLeave += new System.EventHandler(this.RadButtonSelModelos_MouseLeave);
            // 
            // radButtonSelComprobantes
            // 
            this.radButtonSelComprobantes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSelComprobantes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radButtonSelComprobantes.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSelComprobantes.Location = new System.Drawing.Point(526, 51);
            this.radButtonSelComprobantes.Name = "radButtonSelComprobantes";
            this.radButtonSelComprobantes.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonSelComprobantes.Size = new System.Drawing.Size(186, 27);
            this.radButtonSelComprobantes.TabIndex = 20;
            this.radButtonSelComprobantes.Text = "Seleccionar directorio";
            this.radButtonSelComprobantes.Click += new System.EventHandler(this.RadButtonSelComprobantes_Click);
            this.radButtonSelComprobantes.MouseEnter += new System.EventHandler(this.RadButtonSelComprobantes_MouseEnter);
            this.radButtonSelComprobantes.MouseLeave += new System.EventHandler(this.RadButtonSelComprobantes_MouseLeave);
            // 
            // radLabelRutaModelos
            // 
            this.radLabelRutaModelos.Location = new System.Drawing.Point(10, 115);
            this.radLabelRutaModelos.Name = "radLabelRutaModelos";
            this.radLabelRutaModelos.Size = new System.Drawing.Size(112, 19);
            this.radLabelRutaModelos.TabIndex = 25;
            this.radLabelRutaModelos.Text = "Directorio Modelos";
            // 
            // radLabelRutaArchivos
            // 
            this.radLabelRutaArchivos.Location = new System.Drawing.Point(10, 16);
            this.radLabelRutaArchivos.Name = "radLabelRutaArchivos";
            this.radLabelRutaArchivos.Size = new System.Drawing.Size(110, 19);
            this.radLabelRutaArchivos.TabIndex = 10;
            this.radLabelRutaArchivos.Text = "Directorio Archivos";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1111, 819);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtModelo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelModelos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSelComprobantes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaModelos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRutaArchivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSelCompContable;
        private Telerik.WinControls.UI.RadTextBox txtComprobante;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.Button btnSelModeloCont;
        private Telerik.WinControls.UI.RadTextBox txtModelo;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadLabel radLabelRutaArchivos;
        private Telerik.WinControls.UI.RadLabel radLabelRutaModelos;
        private Telerik.WinControls.UI.RadOpenFolderDialog radOpenFolderDialog1;
        private Telerik.WinControls.UI.RadButton radButtonSelComprobantes;
        private Telerik.WinControls.UI.RadButton radButtonSelModelos;
    }
}