namespace SmartCG
{
    partial class frmValidaClave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmValidaClave));
            this.radButtonCancelar = new Telerik.WinControls.UI.RadButton();
            this.radLabelFechaCaducidadValor = new Telerik.WinControls.UI.RadLabel();
            this.radLlblCaducidad = new Telerik.WinControls.UI.RadLabel();
            this.radLlblClave = new Telerik.WinControls.UI.RadLabel();
            this.radButtonActualizar = new Telerik.WinControls.UI.RadButton();
            this.radTextBoxNuevaClave = new Telerik.WinControls.UI.RadTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelFechaCaducidadValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblCaducidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblClave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxNuevaClave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonCancelar
            // 
            this.radButtonCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCancelar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCancelar.Location = new System.Drawing.Point(295, 260);
            this.radButtonCancelar.Name = "radButtonCancelar";
            this.radButtonCancelar.Size = new System.Drawing.Size(132, 44);
            this.radButtonCancelar.TabIndex = 30;
            this.radButtonCancelar.Text = "Cancelar";
            this.radButtonCancelar.Click += new System.EventHandler(this.RadButtonCancelar_Click);
            this.radButtonCancelar.MouseEnter += new System.EventHandler(this.RadButtonCancelar_MouseEnter);
            this.radButtonCancelar.MouseLeave += new System.EventHandler(this.RadButtonCancelar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCancelar.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCancelar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelFechaCaducidadValor
            // 
            this.radLabelFechaCaducidadValor.Location = new System.Drawing.Point(161, 84);
            this.radLabelFechaCaducidadValor.Name = "radLabelFechaCaducidadValor";
            this.radLabelFechaCaducidadValor.Size = new System.Drawing.Size(129, 19);
            this.radLabelFechaCaducidadValor.TabIndex = 10;
            this.radLabelFechaCaducidadValor.Text = "fecha Caducidad Valor";
            // 
            // radLlblCaducidad
            // 
            this.radLlblCaducidad.Location = new System.Drawing.Point(161, 48);
            this.radLlblCaducidad.Name = "radLlblCaducidad";
            this.radLlblCaducidad.Size = new System.Drawing.Size(114, 19);
            this.radLlblCaducidad.TabIndex = 5;
            this.radLlblCaducidad.Text = "Fecha de caducidad";
            // 
            // radLlblClave
            // 
            this.radLlblClave.Location = new System.Drawing.Point(161, 139);
            this.radLlblClave.Name = "radLlblClave";
            this.radLlblClave.Size = new System.Drawing.Size(123, 19);
            this.radLlblClave.TabIndex = 15;
            this.radLlblClave.Text = "Clave de la aplicación";
            // 
            // radButtonActualizar
            // 
            this.radButtonActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonActualizar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonActualizar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonActualizar.Location = new System.Drawing.Point(133, 260);
            this.radButtonActualizar.Name = "radButtonActualizar";
            this.radButtonActualizar.Size = new System.Drawing.Size(132, 44);
            this.radButtonActualizar.TabIndex = 25;
            this.radButtonActualizar.Text = "Actualizar Clave";
            this.radButtonActualizar.Click += new System.EventHandler(this.RadButtonActualizar_Click);
            this.radButtonActualizar.MouseEnter += new System.EventHandler(this.RadButtonActualizar_MouseEnter);
            this.radButtonActualizar.MouseLeave += new System.EventHandler(this.RadButtonActualizar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonActualizar.GetChildAt(0))).Text = "Actualizar Clave";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonActualizar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radTextBoxNuevaClave
            // 
            this.radTextBoxNuevaClave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.radTextBoxNuevaClave.Location = new System.Drawing.Point(161, 165);
            this.radTextBoxNuevaClave.MaxLength = 24;
            this.radTextBoxNuevaClave.Name = "radTextBoxNuevaClave";
            this.radTextBoxNuevaClave.Padding = new System.Windows.Forms.Padding(5);
            this.radTextBoxNuevaClave.Size = new System.Drawing.Size(234, 40);
            this.radTextBoxNuevaClave.TabIndex = 20;
            // 
            // frmValidaClave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.CancelButton = this.radButtonCancelar;
            this.ClientSize = new System.Drawing.Size(578, 355);
            this.Controls.Add(this.radButtonCancelar);
            this.Controls.Add(this.radLabelFechaCaducidadValor);
            this.Controls.Add(this.radLlblCaducidad);
            this.Controls.Add(this.radLlblClave);
            this.Controls.Add(this.radButtonActualizar);
            this.Controls.Add(this.radTextBoxNuevaClave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmValidaClave";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Actualización de la clave";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmValidaClave_FormClosing);
            this.Load += new System.EventHandler(this.FrmValidaClave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelFechaCaducidadValor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblCaducidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblClave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxNuevaClave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxNuevaClave;
        private Telerik.WinControls.UI.RadButton radButtonActualizar;
        private Telerik.WinControls.UI.RadLabel radLlblClave;
        private Telerik.WinControls.UI.RadLabel radLlblCaducidad;
        private Telerik.WinControls.UI.RadLabel radLabelFechaCaducidadValor;
        private Telerik.WinControls.UI.RadButton radButtonCancelar;
    }
}
