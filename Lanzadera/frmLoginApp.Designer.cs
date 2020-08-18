namespace SmartCG
{
    partial class frmLoginApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginApp));
            this.radTextBoxControlPwd = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radTextBoxControlUser = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radLblPwd = new Telerik.WinControls.UI.RadLabel();
            this.radLlblUser = new Telerik.WinControls.UI.RadLabel();
            this.radButtonLogin = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxControlPwd
            // 
            this.radTextBoxControlPwd.Location = new System.Drawing.Point(133, 165);
            this.radTextBoxControlPwd.Name = "radTextBoxControlPwd";
            this.radTextBoxControlPwd.Padding = new System.Windows.Forms.Padding(5);
            this.radTextBoxControlPwd.PasswordChar = '*';
            this.radTextBoxControlPwd.Size = new System.Drawing.Size(234, 50);
            this.radTextBoxControlPwd.TabIndex = 65;
            this.radTextBoxControlPwd.Click += new System.EventHandler(this.RadTextBoxControlPwd_Click);
            this.radTextBoxControlPwd.DoubleClick += new System.EventHandler(this.RadTextBoxControlPwd_DoubleClick);
            this.radTextBoxControlPwd.Enter += new System.EventHandler(this.RadTextBoxControlPwd_Enter);
            this.radTextBoxControlPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RadTextBoxControlPwd_KeyPress);
            // 
            // radTextBoxControlUser
            // 
            this.radTextBoxControlUser.Location = new System.Drawing.Point(133, 70);
            this.radTextBoxControlUser.Name = "radTextBoxControlUser";
            this.radTextBoxControlUser.Padding = new System.Windows.Forms.Padding(5);
            this.radTextBoxControlUser.Size = new System.Drawing.Size(234, 50);
            this.radTextBoxControlUser.TabIndex = 63;
            this.radTextBoxControlUser.Click += new System.EventHandler(this.RadTextBoxControlUser_Click);
            this.radTextBoxControlUser.DoubleClick += new System.EventHandler(this.RadTextBoxControlUser_DoubleClick);
            this.radTextBoxControlUser.Enter += new System.EventHandler(this.RadTextBoxControlUser_Enter);
            this.radTextBoxControlUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RadTextBoxControlUser_KeyPress);
            // 
            // radLblPwd
            // 
            this.radLblPwd.Location = new System.Drawing.Point(133, 143);
            this.radLblPwd.Name = "radLblPwd";
            this.radLblPwd.Size = new System.Drawing.Size(68, 19);
            this.radLblPwd.TabIndex = 64;
            this.radLblPwd.Text = "Contraseña";
            // 
            // radLlblUser
            // 
            this.radLlblUser.Location = new System.Drawing.Point(133, 48);
            this.radLlblUser.Name = "radLlblUser";
            this.radLlblUser.Size = new System.Drawing.Size(48, 19);
            this.radLlblUser.TabIndex = 61;
            this.radLlblUser.Text = "Usuario";
            // 
            // radButtonLogin
            // 
            this.radButtonLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonLogin.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonLogin.Location = new System.Drawing.Point(153, 269);
            this.radButtonLogin.Name = "radButtonLogin";
            this.radButtonLogin.Size = new System.Drawing.Size(190, 44);
            this.radButtonLogin.TabIndex = 68;
            this.radButtonLogin.Text = "Iniciar sesión";
            this.radButtonLogin.Click += new System.EventHandler(this.RadButtonLogin_Click);
            this.radButtonLogin.MouseEnter += new System.EventHandler(this.RadButtonLogin_MouseEnter);
            this.radButtonLogin.MouseLeave += new System.EventHandler(this.RadButtonLogin_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonLogin.GetChildAt(0))).Text = "Iniciar sesión";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonLogin.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmLoginApp
            // 
            this.AcceptButton = this.radButtonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(474, 355);
            this.Controls.Add(this.radButtonLogin);
            this.Controls.Add(this.radTextBoxControlPwd);
            this.Controls.Add(this.radTextBoxControlUser);
            this.Controls.Add(this.radLblPwd);
            this.Controls.Add(this.radLlblUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginApp";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "  Conectar a la aplicación";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogin_FormClosing);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLoginApp_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLlblUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion     
        private Telerik.WinControls.UI.RadLabel radLlblUser;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlUser;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlPwd;
        private Telerik.WinControls.UI.RadLabel radLblPwd;
        private Telerik.WinControls.UI.RadButton radButtonLogin;
    }
}