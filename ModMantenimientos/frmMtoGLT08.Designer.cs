namespace ModMantenimientos
{
    partial class frmMtoGLT08
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtoGLT08));
            this.materialBlueGreyTheme1 = new Telerik.WinControls.Themes.MaterialBlueGreyTheme();
            this.radButtonTextBoxTipoAux = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonElementTipoAux = new Telerik.WinControls.UI.RadButtonElement();
            this.radToggleSwitchEstadoActiva = new Telerik.WinControls.UI.RadToggleSwitch();
            this.radButtonEliminar = new Telerik.WinControls.UI.RadButton();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.txtNombre = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblNombre = new Telerik.WinControls.UI.RadLabel();
            this.lblEstado = new Telerik.WinControls.UI.RadLabel();
            this.txtCodigo = new Telerik.WinControls.UI.RadTextBoxControl();
            this.lblGrupoCuentas = new Telerik.WinControls.UI.RadLabel();
            this.lblTipoAux = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxTipoAux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radToggleSwitchEstadoActiva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrupoCuentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoAux)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonTextBoxTipoAux
            // 
            this.radButtonTextBoxTipoAux.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radButtonTextBoxTipoAux.Location = new System.Drawing.Point(47, 85);
            this.radButtonTextBoxTipoAux.MaxLength = 2;
            this.radButtonTextBoxTipoAux.Name = "radButtonTextBoxTipoAux";
            this.radButtonTextBoxTipoAux.Padding = new System.Windows.Forms.Padding(5);
            this.radButtonTextBoxTipoAux.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElementTipoAux});
            this.radButtonTextBoxTipoAux.Size = new System.Drawing.Size(285, 30);
            this.radButtonTextBoxTipoAux.TabIndex = 10;
            this.radButtonTextBoxTipoAux.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radButtonTextBoxTipoAux_KeyPress);
            this.radButtonTextBoxTipoAux.Leave += new System.EventHandler(this.radButtonTextBoxTipoAux_Leave);
            // 
            // radButtonElementTipoAux
            // 
            this.radButtonElementTipoAux.AutoSize = true;
            this.radButtonElementTipoAux.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radButtonElementTipoAux.Image = ((System.Drawing.Image)(resources.GetObject("radButtonElementTipoAux.Image")));
            this.radButtonElementTipoAux.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonElementTipoAux.Name = "radButtonElementTipoAux";
            this.radButtonElementTipoAux.Text = "";
            this.radButtonElementTipoAux.UseCompatibleTextRendering = false;
            this.radButtonElementTipoAux.Click += new System.EventHandler(this.RadButtonElementTipoAux_Click);
            // 
            // radToggleSwitchEstadoActiva
            // 
            this.radToggleSwitchEstadoActiva.Location = new System.Drawing.Point(405, 55);
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
            this.radButtonEliminar.Location = new System.Drawing.Point(202, 304);
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
            this.radButtonExit.Location = new System.Drawing.Point(365, 304);
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
            this.radButtonSave.Location = new System.Drawing.Point(45, 304);
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
            this.txtNombre.Size = new System.Drawing.Size(310, 30);
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
            this.lblEstado.Location = new System.Drawing.Point(345, 58);
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
            this.txtCodigo.Size = new System.Drawing.Size(75, 30);
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
            // lblTipoAux
            // 
            this.lblTipoAux.Location = new System.Drawing.Point(47, 58);
            this.lblTipoAux.Name = "lblTipoAux";
            this.lblTipoAux.Size = new System.Drawing.Size(90, 19);
            this.lblTipoAux.TabIndex = 6;
            this.lblTipoAux.Text = "Tipo de auxiliar";
            // 
            // frmMtoGLT08
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(547, 384);
            this.Controls.Add(this.radButtonTextBoxTipoAux);
            this.Controls.Add(this.radToggleSwitchEstadoActiva);
            this.Controls.Add(this.radButtonEliminar);
            this.Controls.Add(this.radButtonExit);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblGrupoCuentas);
            this.Controls.Add(this.lblTipoAux);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMtoGLT08";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "   Mantenimiento de Grupos de Cuentas de Auxiliar";
            this.Load += new System.EventHandler(this.FrmMtoGLT08_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMtoGLT08_KeyDown);
            this.MouseEnter += new System.EventHandler(this.frmMtoGLT08_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBoxTipoAux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radToggleSwitchEstadoActiva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrupoCuentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoAux)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel lblTipoAux;
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
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxTipoAux;
        private Telerik.WinControls.UI.RadButtonElement radButtonElementTipoAux;
    }
}