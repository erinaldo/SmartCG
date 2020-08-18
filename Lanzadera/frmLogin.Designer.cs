namespace SmartCG
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.btnAceptar = new Telerik.WinControls.UI.RadButton();
            this.radLabelDSN = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownListDSN = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabelUser = new Telerik.WinControls.UI.RadLabel();
            this.radLabelPwd = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxControlPwd = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radTextBoxControlUser = new Telerik.WinControls.UI.RadTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelDSN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListDSN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            resources.ApplyResources(this.btnAceptar, "btnAceptar");
            this.btnAceptar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            this.btnAceptar.MouseEnter += new System.EventHandler(this.BtnAceptar_MouseEnter);
            this.btnAceptar.MouseLeave += new System.EventHandler(this.BtnAceptar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnAceptar.GetChildAt(0))).Text = resources.GetString("resource.Text");
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btnAceptar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelDSN
            // 
            resources.ApplyResources(this.radLabelDSN, "radLabelDSN");
            this.radLabelDSN.Name = "radLabelDSN";
            // 
            // radDropDownListDSN
            // 
            this.radDropDownListDSN.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            resources.ApplyResources(this.radDropDownListDSN, "radDropDownListDSN");
            this.radDropDownListDSN.Name = "radDropDownListDSN";
            // 
            // radLabelUser
            // 
            resources.ApplyResources(this.radLabelUser, "radLabelUser");
            this.radLabelUser.Name = "radLabelUser";
            // 
            // radLabelPwd
            // 
            resources.ApplyResources(this.radLabelPwd, "radLabelPwd");
            this.radLabelPwd.Name = "radLabelPwd";
            // 
            // radTextBoxControlPwd
            // 
            resources.ApplyResources(this.radTextBoxControlPwd, "radTextBoxControlPwd");
            this.radTextBoxControlPwd.Name = "radTextBoxControlPwd";
            this.radTextBoxControlPwd.PasswordChar = '*';
            // 
            // radTextBoxControlUser
            // 
            resources.ApplyResources(this.radTextBoxControlUser, "radTextBoxControlUser");
            this.radTextBoxControlUser.Name = "radTextBoxControlUser";
            this.radTextBoxControlUser.Click += new System.EventHandler(this.RadTextBoxControlUser_Click);
            this.radTextBoxControlUser.DoubleClick += new System.EventHandler(this.RadTextBoxControlUser_DoubleClick);
            this.radTextBoxControlUser.Enter += new System.EventHandler(this.RadTextBoxControlUser_Enter);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnAceptar;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.radTextBoxControlUser);
            this.Controls.Add(this.radTextBoxControlPwd);
            this.Controls.Add(this.radLabelPwd);
            this.Controls.Add(this.radLabelUser);
            this.Controls.Add(this.radDropDownListDSN);
            this.Controls.Add(this.radLabelDSN);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogin_FormClosing);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelDSN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListDSN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private Telerik.WinControls.UI.RadLabel radLabelDSN;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListDSN;
        private Telerik.WinControls.UI.RadLabel radLabelUser;
        private Telerik.WinControls.UI.RadLabel radLabelPwd;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlPwd;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlUser;
        private Telerik.WinControls.UI.RadButton btnAceptar;
    }
}