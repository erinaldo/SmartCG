namespace SmartCG
{
    partial class frmEntornoLista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEntornoLista));
            this.radButtonSuprimir = new Telerik.WinControls.UI.RadButton();
            this.radButtonCopiarFormato = new Telerik.WinControls.UI.RadButton();
            this.radButtonEditar = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radButtonCargar = new Telerik.WinControls.UI.RadButton();
            this.radGridViewEntornos = new Telerik.WinControls.UI.RadGridView();
            this.lblNoHayEntorno = new System.Windows.Forms.Label();
            this.radButtonCancelar = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSuprimir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCopiarFormato)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCargar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewEntornos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewEntornos.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSuprimir
            // 
            this.radButtonSuprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSuprimir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSuprimir.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSuprimir.Location = new System.Drawing.Point(18, 298);
            this.radButtonSuprimir.Name = "radButtonSuprimir";
            this.radButtonSuprimir.Size = new System.Drawing.Size(138, 44);
            this.radButtonSuprimir.TabIndex = 12;
            this.radButtonSuprimir.Text = "Suprimir";
            this.radButtonSuprimir.Click += new System.EventHandler(this.RadButtonSuprimir_Click);
            this.radButtonSuprimir.MouseEnter += new System.EventHandler(this.RadButtonSuprimir_MouseEnter);
            this.radButtonSuprimir.MouseLeave += new System.EventHandler(this.RadButtonSuprimir_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSuprimir.GetChildAt(0))).Text = "Suprimir";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSuprimir.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonCopiarFormato
            // 
            this.radButtonCopiarFormato.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCopiarFormato.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCopiarFormato.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCopiarFormato.Location = new System.Drawing.Point(18, 237);
            this.radButtonCopiarFormato.Name = "radButtonCopiarFormato";
            this.radButtonCopiarFormato.Size = new System.Drawing.Size(138, 44);
            this.radButtonCopiarFormato.TabIndex = 11;
            this.radButtonCopiarFormato.Text = "  Copiar Entorno";
            this.radButtonCopiarFormato.Click += new System.EventHandler(this.RadButtonCopiarFormato_Click);
            this.radButtonCopiarFormato.MouseEnter += new System.EventHandler(this.RadButtonCopiarFormato_MouseEnter);
            this.radButtonCopiarFormato.MouseLeave += new System.EventHandler(this.RadButtonCopiarFormato_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCopiarFormato.GetChildAt(0))).Text = "  Copiar Entorno";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCopiarFormato.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEditar
            // 
            this.radButtonEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEditar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEditar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEditar.Location = new System.Drawing.Point(18, 177);
            this.radButtonEditar.Name = "radButtonEditar";
            this.radButtonEditar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEditar.TabIndex = 10;
            this.radButtonEditar.Text = "Editar";
            this.radButtonEditar.Click += new System.EventHandler(this.RadButtonEditar_Click);
            this.radButtonEditar.MouseEnter += new System.EventHandler(this.RadButtonEditar_MouseEnter);
            this.radButtonEditar.MouseLeave += new System.EventHandler(this.RadButtonEditar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEditar.GetChildAt(0))).Text = "Editar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEditar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonNuevo
            // 
            this.radButtonNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonNuevo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonNuevo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonNuevo.Location = new System.Drawing.Point(17, 116);
            this.radButtonNuevo.Name = "radButtonNuevo";
            this.radButtonNuevo.Size = new System.Drawing.Size(139, 44);
            this.radButtonNuevo.TabIndex = 9;
            this.radButtonNuevo.Text = "Nuevo";
            this.radButtonNuevo.Click += new System.EventHandler(this.RadButtonNuevo_Click);
            this.radButtonNuevo.MouseEnter += new System.EventHandler(this.RadButtonNuevo_MouseEnter);
            this.radButtonNuevo.MouseLeave += new System.EventHandler(this.RadButtonNuevo_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNuevo.GetChildAt(0))).Text = "Nuevo";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonNuevo.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonCargar
            // 
            this.radButtonCargar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCargar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCargar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCargar.Location = new System.Drawing.Point(18, 55);
            this.radButtonCargar.Name = "radButtonCargar";
            this.radButtonCargar.Size = new System.Drawing.Size(138, 44);
            this.radButtonCargar.TabIndex = 8;
            this.radButtonCargar.Text = "Cargar";
            this.radButtonCargar.Click += new System.EventHandler(this.RadButtonCargar_Click);
            this.radButtonCargar.MouseEnter += new System.EventHandler(this.RadButtonCargar_MouseEnter);
            this.radButtonCargar.MouseLeave += new System.EventHandler(this.RadButtonCargar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCargar.GetChildAt(0))).Text = "Cargar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCargar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radGridViewEntornos
            // 
            this.radGridViewEntornos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridViewEntornos.Location = new System.Drawing.Point(177, 56);
            // 
            // 
            // 
            this.radGridViewEntornos.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewEntornos.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewEntornos.MasterTemplate.AllowEditRow = false;
            this.radGridViewEntornos.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewEntornos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewEntornos.Name = "radGridViewEntornos";
            this.radGridViewEntornos.Size = new System.Drawing.Size(713, 377);
            this.radGridViewEntornos.TabIndex = 7;
            this.radGridViewEntornos.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewEntornos_CellDoubleClick);
            // 
            // lblNoHayEntorno
            // 
            this.lblNoHayEntorno.AutoSize = true;
            this.lblNoHayEntorno.ForeColor = System.Drawing.Color.Red;
            this.lblNoHayEntorno.Location = new System.Drawing.Point(177, 56);
            this.lblNoHayEntorno.Name = "lblNoHayEntorno";
            this.lblNoHayEntorno.Size = new System.Drawing.Size(107, 15);
            this.lblNoHayEntorno.TabIndex = 6;
            this.lblNoHayEntorno.Text = "NoExistenEntornos";
            this.lblNoHayEntorno.Visible = false;
            // 
            // radButtonCancelar
            // 
            this.radButtonCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCancelar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCancelar.Location = new System.Drawing.Point(18, 358);
            this.radButtonCancelar.Name = "radButtonCancelar";
            this.radButtonCancelar.Size = new System.Drawing.Size(138, 44);
            this.radButtonCancelar.TabIndex = 13;
            this.radButtonCancelar.Text = "Cancelar";
            this.radButtonCancelar.Click += new System.EventHandler(this.RadButtonCancelar_Click);
            this.radButtonCancelar.MouseEnter += new System.EventHandler(this.RadButtonCancelar_MouseEnter);
            this.radButtonCancelar.MouseLeave += new System.EventHandler(this.RadButtonCancelar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCancelar.GetChildAt(0))).Text = "Cancelar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCancelar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmEntornoLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(902, 470);
            this.Controls.Add(this.lblNoHayEntorno);
            this.Controls.Add(this.radButtonCancelar);
            this.Controls.Add(this.radButtonSuprimir);
            this.Controls.Add(this.radButtonCopiarFormato);
            this.Controls.Add(this.radButtonEditar);
            this.Controls.Add(this.radButtonNuevo);
            this.Controls.Add(this.radButtonCargar);
            this.Controls.Add(this.radGridViewEntornos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEntornoLista";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "   Lista de entornos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEntornoLista_FormClosing);
            this.Load += new System.EventHandler(this.FrmEntorno_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmEntornoLista_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSuprimir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCopiarFormato)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEditar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCargar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewEntornos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewEntornos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblNoHayEntorno;
        private Telerik.WinControls.UI.RadGridView radGridViewEntornos;
        private Telerik.WinControls.UI.RadButton radButtonCargar;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadButton radButtonEditar;
        private Telerik.WinControls.UI.RadButton radButtonCopiarFormato;
        private Telerik.WinControls.UI.RadButton radButtonSuprimir;
        private Telerik.WinControls.UI.RadButton radButtonCancelar;
    }
}