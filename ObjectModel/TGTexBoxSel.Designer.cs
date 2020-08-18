namespace ObjectModel
{
    partial class TGTexBoxSel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtElemento = new System.Windows.Forms.TextBox();
            this.btnSel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtElemento
            // 
            this.txtElemento.Location = new System.Drawing.Point(4, 5);
            this.txtElemento.Name = "txtElemento";
            this.txtElemento.Size = new System.Drawing.Size(131, 20);
            this.txtElemento.TabIndex = 0;
            this.txtElemento.TextChanged += new System.EventHandler(this.txtElemento_TextChanged);
            this.txtElemento.Enter += new System.EventHandler(this.txtElemento_Enter);
            this.txtElemento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtElemento_KeyPress);
            // 
            // btnSel
            // 
            this.btnSel.AllowDrop = true;
            this.btnSel.Image = global::ObjectModel.Properties.Resources.Seleccionar;
            this.btnSel.Location = new System.Drawing.Point(141, 4);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(27, 23);
            this.btnSel.TabIndex = 1;
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // TGTexBoxSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSel);
            this.Controls.Add(this.txtElemento);
            this.Name = "TGTexBoxSel";
            this.Size = new System.Drawing.Size(185, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtElemento;
        private System.Windows.Forms.Button btnSel;
    }
}
