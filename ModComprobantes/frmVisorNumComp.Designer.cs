namespace ModComprobantes
{
    partial class frmVisorNumComp
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVisorNumComp));
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.radGridViewInfo = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSalir
            // 
            this.radButtonSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSalir.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSalir.Location = new System.Drawing.Point(421, 356);
            this.radButtonSalir.Name = "radButtonSalir";
            this.radButtonSalir.Size = new System.Drawing.Size(145, 44);
            this.radButtonSalir.TabIndex = 101;
            this.radButtonSalir.Text = "Salir";
            this.radButtonSalir.Click += new System.EventHandler(this.RadButtonSalir_Click);
            this.radButtonSalir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RadButtonSalir_KeyDown);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSalir.GetChildAt(0))).Text = "Salir";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSalir.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radGridViewInfo
            // 
            this.radGridViewInfo.Location = new System.Drawing.Point(43, 42);
            // 
            // 
            // 
            this.radGridViewInfo.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewInfo.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewInfo.MasterTemplate.AllowEditRow = false;
            this.radGridViewInfo.MasterTemplate.AllowSearchRow = true;
            this.radGridViewInfo.MasterTemplate.EnableFiltering = true;
            this.radGridViewInfo.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewInfo.Name = "radGridViewInfo";
            this.radGridViewInfo.Size = new System.Drawing.Size(855, 270);
            this.radGridViewInfo.TabIndex = 102;
            // 
            // frmVisorNumComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(946, 440);
            this.Controls.Add(this.radGridViewInfo);
            this.Controls.Add(this.radButtonSalir);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmVisorNumComp";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Números de Comprobantes Generados";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVisorNumComp_FormClosing);
            this.Load += new System.EventHandler(this.FrmVisorNumComp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadButton radButtonSalir;
        private Telerik.WinControls.UI.RadGridView radGridViewInfo;
    }
}