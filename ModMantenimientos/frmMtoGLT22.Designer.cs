namespace ModMantenimientos
{
    partial class frmMtoGLT22
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoGLT22));
            this.materialBlueGreyTheme1 = new Telerik.WinControls.Themes.MaterialBlueGreyTheme();
            this.radButtonTextBoxPlanCuentas = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonElementPlanCuentas = new Telerik.WinControls.UI.RadButtonElement();
            this.radToggleSwitchEstadoActiva = new Telerik.WinControls.UI.RadToggleSwitch();
            this.radButtonEliminar = new Telerik.WinControls.UI.RadButton();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.txtNombre = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblNombre = new Telerik.WinControls.UI.RadLabel();
            this.lblEstado = new Telerik.WinControls.UI.RadLabel();
            this.txtCodigo = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblGrupoCuentas = new Telerik.WinControls.UI.RadLabel();
            this.lblPlan = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxPlanCuentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radToggleSwitchEstadoActiva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrupoCuentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonTextBoxPlanCuentas
            // 
            this.radButtonTextBoxPlanCuentas.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radButtonTextBoxPlanCuentas.Location = new System.Drawing.Point(47, 81);
            this.radButtonTextBoxPlanCuentas.MaxLength = 1;
            this.radButtonTextBoxPlanCuentas.Name = "radButtonTextBoxPlanCuentas";
            this.radButtonTextBoxPlanCuentas.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonTextBoxPlanCuentas.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElementPlanCuentas});
            this.radButtonTextBoxPlanCuentas.Size = new System.Drawing.Size(251, 30);
            this.radButtonTextBoxPlanCuentas.TabIndex = 10;
            this.radButtonTextBoxPlanCuentas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radButtonTextBoxPlanCuentas_KeyPress);
            this.radButtonTextBoxPlanCuentas.Leave += new System.EventHandler(this.radButtonTextBoxPlanCuentas_Leave);
            // 
            // radButtonElementPlanCuentas
            // 
            this.radButtonElementPlanCuentas.AutoSize = true;
            this.radButtonElementPlanCuentas.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radButtonElementPlanCuentas.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElementPlanCuentas.Image")));
            this.radButtonElementPlanCuentas.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonElementPlanCuentas.Name = "radButtonElementPlanCuentas";
            this.radButtonElementPlanCuentas.Text = "";
            this.radButtonElementPlanCuentas.UseCompatibleTextRendering = false;
            this.radButtonElementPlanCuentas.Click += new System.EventHandler(this.RadButtonElementPlanCuentas_Click);
            // 
            // radToggleSwitchEstadoActiva
            // 
            this.radToggleSwitchEstadoActiva.Location = new System.Drawing.Point(381, 58);
            this.radToggleSwitchEstadoActiva.Name = "radToggleSwitchEstadoActiva";
            this.radToggleSwitchEstadoActiva.OffText = "";
            this.radToggleSwitchEstadoActiva.OnText = "";
            this.radToggleSwitchEstadoActiva.Size = new System.Drawing.Size(50, 20);
            this.radToggleSwitchEstadoActiva.TabIndex = 20;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";
            // 
            // radButtonEliminar
            // 
            this.radButtonEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEliminar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEliminar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEliminar.Location = new System.Drawing.Point(200, 313);
            this.radButtonEliminar.Name = "radButtonEliminar";
            this.radButtonEliminar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEliminar.TabIndex = 60;
            this.radButtonEliminar.Text = "Eliminar";
            this.radButtonEliminar.Click += new System.EventHandler(this.RadButtonEliminar_Click);
            this.radButtonEliminar.MouseEnter += new System.EventHandler(this.RadButtonEliminar_MouseEnter);
            this.radButtonEliminar.MouseLeave += new System.EventHandler(this.RadButtonEliminar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEliminar.GetChildAt(0))).Text = "Eliminar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEliminar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExit.Location = new System.Drawing.Point(360, 313);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(138, 44);
            this.radButtonExit.TabIndex = 70;
            this.radButtonExit.Text = "Cancelar";
            this.radButtonExit.Click += new System.EventHandler(this.RadButtonExit_Click);
            this.radButtonExit.MouseEnter += new System.EventHandler(this.RadButtonExit_MouseEnter);
            this.radButtonExit.MouseLeave += new System.EventHandler(this.RadButtonExit_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExit.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            // 
            // radButtonSave
            // 
            this.radButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSave.Location = new System.Drawing.Point(40, 313);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(138, 44);
            this.radButtonSave.TabIndex = 50;
            this.radButtonSave.Text = "Guardar";
            this.radButtonSave.Click += new System.EventHandler(this.RadButtonSave_Click);
            this.radButtonSave.MouseEnter += new System.EventHandler(this.RadButtonSave_MouseEnter);
            this.radButtonSave.MouseLeave += new System.EventHandler(this.RadButtonSave_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSave.GetChildAt(0))).Text = "Guardar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(47, 221);
            this.txtNombre.MaxLength = 40;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Padding = new System.Windows.Forms.Padding(5);
            this.txtNombre.Size = new System.Drawing.Size(320, 30);
            this.txtNombre.TabIndex = 40;
            // 
            // lblNombre
            // 
            this.lblNombre.Location = new System.Drawing.Point(47, 194);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(52, 19);
            this.lblNombre.TabIndex = 66;
            this.lblNombre.Text = "Nombre";
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(321, 58);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(41, 19);
            this.lblEstado.TabIndex = 64;
            this.lblEstado.Text = "Activo";
            // 
            // txtCodigo
            // 
            this.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigo.Location = new System.Drawing.Point(47, 149);
            this.txtCodigo.MaxLength = 6;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Padding = new System.Windows.Forms.Padding(5);
            this.txtCodigo.Size = new System.Drawing.Size(80, 30);
            this.txtCodigo.TabIndex = 30;
            this.txtCodigo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCodigo_KeyPress);
            this.txtCodigo.Leave += new System.EventHandler(this.TxtCodigo_Leave);
            // 
            // lblGrupoCuentas
            // 
            this.lblGrupoCuentas.Location = new System.Drawing.Point(47, 126);
            this.lblGrupoCuentas.Name = "lblGrupoCuentas";
            this.lblGrupoCuentas.Size = new System.Drawing.Size(103, 19);
            this.lblGrupoCuentas.TabIndex = 62;
            this.lblGrupoCuentas.Text = "Grupo de cuentas";
            // 
            // lblPlan
            // 
            this.lblPlan.Location = new System.Drawing.Point(47, 58);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(30, 19);
            this.lblPlan.TabIndex = 6;
            this.lblPlan.Text = "Plan";
            // 
            // frmMtoGLT22
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(542, 394);
            this.Controls.Add(this.radButtonTextBoxPlanCuentas);
            this.Controls.Add(this.radToggleSwitchEstadoActiva);
            this.Controls.Add(this.radButtonEliminar);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblGrupoCuentas);
            this.Controls.Add(this.lblPlan);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoGLT22";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "   Mantenimiento de Grupos de Cuentas de Mayor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMtoGLT22_FormClosing);
            this.Load += new System.EventHandler(this.FrmMtoGLT22_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMtoGLT22_KeyDown);
            this.MouseEnter += new System.EventHandler(this.frmMtoGLT22_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxPlanCuentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radToggleSwitchEstadoActiva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrupoCuentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel lblPlan;
        private Telerik.WinControls.UI.RadLabel lblGrupoCuentas;
        private Telerik.WinControls.UI.RadTextBoxControl txtCodigo;
        private Telerik.WinControls.UI.RadLabel lblEstado;
        private Telerik.WinControls.UI.RadLabel lblNombre;
        private Telerik.WinControls.UI.RadTextBoxControl txtNombre;
        private Telerik.WinControls.UI.RadButton radButtonEliminar;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private Telerik.WinControls.UI.RadToggleSwitch radToggleSwitchEstadoActiva;
        private Telerik.WinControls.Themes.MaterialBlueGreyTheme materialBlueGreyTheme1;
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxPlanCuentas;
        private Telerik.WinControls.UI.RadButtonElement radButtonElementPlanCuentas;
    }
}