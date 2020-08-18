namespace SmartCG
{
    partial class RadFrmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadFrmConfig));
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radLabelExportarDatosDefecto = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownListExportarDatosDefecto = new Telerik.WinControls.UI.RadDropDownList();
            this.radCheckBoxExportarDatosDefectoView = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBoxSolicitarEntornoInicio = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelExportarDatosDefecto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListExportarDatosDefecto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxExportarDatosDefectoView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxSolicitarEntornoInicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(264, 231);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 44);
            this.radButtonSave.TabIndex = 10;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.RadButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.RadButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.RadButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(510, 231);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(139, 44);
            this.radButtonExit.TabIndex = 11;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.RadButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.RadButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.RadButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelExportarDatosDefecto
            // 
            this.radLabelExportarDatosDefecto.Location = new System.Drawing.Point(51, 66);
            this.radLabelExportarDatosDefecto.Name = "radLabelExportarDatosDefecto";
            this.radLabelExportarDatosDefecto.Size = new System.Drawing.Size(202, 19);
            this.radLabelExportarDatosDefecto.TabIndex = 12;
            this.radLabelExportarDatosDefecto.Text = "Formato exportar datos por defecto";
            // 
            // radDropDownListExportarDatosDefecto
            // 
            this.radDropDownListExportarDatosDefecto.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.radDropDownListExportarDatosDefecto.Location = new System.Drawing.Point(51, 101);
            this.radDropDownListExportarDatosDefecto.Name = "radDropDownListExportarDatosDefecto";
            this.radDropDownListExportarDatosDefecto.Padding = new System.Windows.Forms.Padding(3);
            this.radDropDownListExportarDatosDefecto.Size = new System.Drawing.Size(169, 27);
            this.radDropDownListExportarDatosDefecto.TabIndex = 126;
            // 
            // radCheckBoxExportarDatosDefectoView
            // 
            this.radCheckBoxExportarDatosDefectoView.Location = new System.Drawing.Point(373, 66);
            this.radCheckBoxExportarDatosDefectoView.Name = "radCheckBoxExportarDatosDefectoView";
            this.radCheckBoxExportarDatosDefectoView.Size = new System.Drawing.Size(224, 19);
            this.radCheckBoxExportarDatosDefectoView.TabIndex = 127;
            this.radCheckBoxExportarDatosDefectoView.Text = "Visualizar el fichero al exportar datos";
            // 
            // radCheckBoxSolicitarEntornoInicio
            // 
            this.radCheckBoxSolicitarEntornoInicio.Location = new System.Drawing.Point(51, 153);
            this.radCheckBoxSolicitarEntornoInicio.Name = "radCheckBoxSolicitarEntornoInicio";
            this.radCheckBoxSolicitarEntornoInicio.Size = new System.Drawing.Size(250, 19);
            this.radCheckBoxSolicitarEntornoInicio.TabIndex = 128;
            this.radCheckBoxSolicitarEntornoInicio.Text = "Solicitar entorno al entrar en la aplicación";
            // 
            // RadFrmConfig
            // 
            this.AcceptButton = this.radButtonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.radButtonExit;
            this.ClientSize = new System.Drawing.Size(800, 310);
            this.Controls.Add(this.radCheckBoxSolicitarEntornoInicio);
            this.Controls.Add(this.radCheckBoxExportarDatosDefectoView);
            this.Controls.Add(this.radDropDownListExportarDatosDefecto);
            this.Controls.Add(this.radLabelExportarDatosDefecto);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RadFrmConfig";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Smart CG Configuración";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RadFrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.RadFrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelExportarDatosDefecto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListExportarDatosDefecto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxExportarDatosDefectoView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxSolicitarEntornoInicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadLabel radLabelExportarDatosDefecto;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListExportarDatosDefecto;
        private Telerik.WinControls.UI.RadCheckBox radCheckBoxExportarDatosDefectoView;
        private Telerik.WinControls.UI.RadCheckBox radCheckBoxSolicitarEntornoInicio;
    }
}