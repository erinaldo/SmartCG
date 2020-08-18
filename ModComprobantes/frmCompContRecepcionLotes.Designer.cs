namespace ModComprobantes
{
    partial class frmCompContRecepcionLotes
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
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewLotes = new Telerik.WinControls.UI.RadGridView();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonActualizar = new Telerik.WinControls.UI.RadButton();
            this.radButtonEliminar = new Telerik.WinControls.UI.RadButton();
            this.radButtonNuevo = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewLotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewLotes.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radGridViewLotes);
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 45);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(884, 490);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewLotes
            // 
            this.radGridViewLotes.AutoScroll = true;
            this.radGridViewLotes.Location = new System.Drawing.Point(19, 16);
            // 
            // 
            // 
            this.radGridViewLotes.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewLotes.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewLotes.MasterTemplate.AllowEditRow = false;
            this.radGridViewLotes.MasterTemplate.AllowSearchRow = true;
            this.radGridViewLotes.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewLotes.MasterTemplate.EnableFiltering = true;
            this.radGridViewLotes.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewLotes.Name = "radGridViewLotes";
            this.radGridViewLotes.Size = new System.Drawing.Size(818, 471);
            this.radGridViewLotes.TabIndex = 10;
            this.radGridViewLotes.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.RadGridViewLotes_DataBindingComplete);
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(19, 30);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(176, 19);
            this.radLabelNoHayInfo.TabIndex = 196;
            this.radLabelNoHayInfo.Text = "No existen lotes recepcionados";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonActualizar);
            this.radPanelAcciones.Controls.Add(this.radButtonEliminar);
            this.radPanelAcciones.Controls.Add(this.radButtonNuevo);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 284);
            this.radPanelAcciones.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonActualizar
            // 
            this.radButtonActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonActualizar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonActualizar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonActualizar.Location = new System.Drawing.Point(13, 138);
            this.radButtonActualizar.Name = "radButtonActualizar";
            this.radButtonActualizar.Size = new System.Drawing.Size(138, 44);
            this.radButtonActualizar.TabIndex = 22;
            this.radButtonActualizar.Text = "Actualizar";
            this.radButtonActualizar.Click += new System.EventHandler(this.RadButtonActualizar_Click);
            this.radButtonActualizar.MouseEnter += new System.EventHandler(this.RadButtonActualizar_MouseEnter);
            this.radButtonActualizar.MouseLeave += new System.EventHandler(this.RadButtonActualizar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonActualizar.GetChildAt(0))).Text = "Actualizar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonActualizar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEliminar
            // 
            this.radButtonEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEliminar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEliminar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEliminar.Location = new System.Drawing.Point(13, 77);
            this.radButtonEliminar.Name = "radButtonEliminar";
            this.radButtonEliminar.Size = new System.Drawing.Size(138, 44);
            this.radButtonEliminar.TabIndex = 21;
            this.radButtonEliminar.Text = "Eliminar";
            this.radButtonEliminar.Click += new System.EventHandler(this.RadButtonEliminar_Click);
            this.radButtonEliminar.MouseEnter += new System.EventHandler(this.RadButtonEliminar_MouseEnter);
            this.radButtonEliminar.MouseLeave += new System.EventHandler(this.RadButtonEliminar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEliminar.GetChildAt(0))).Text = "Eliminar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEliminar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonNuevo
            // 
            this.radButtonNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonNuevo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonNuevo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonNuevo.Location = new System.Drawing.Point(13, 16);
            this.radButtonNuevo.Name = "radButtonNuevo";
            this.radButtonNuevo.Size = new System.Drawing.Size(138, 44);
            this.radButtonNuevo.TabIndex = 20;
            this.radButtonNuevo.Text = "Nuevo";
            this.radButtonNuevo.Click += new System.EventHandler(this.RadButtonNuevo_Click);
            this.radButtonNuevo.MouseEnter += new System.EventHandler(this.RadButtonNuevo_MouseEnter);
            this.radButtonNuevo.MouseLeave += new System.EventHandler(this.RadButtonNuevo_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNuevo.GetChildAt(0))).Text = "Nuevo";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonNuevo.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1046, 45);
            this.radPanelMenuPath.TabIndex = 25;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(407, 29);
            this.radLabelTitulo.TabIndex = 30;
            this.radLabelTitulo.Text = "Comprobantes Contables / Recepción de Lotes";
            // 
            // frmCompContRecepcionLotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1153, 788);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCompContRecepcionLotes";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Recepción de Lotes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCompContRecepcionLotes_FormClosing);
            this.Load += new System.EventHandler(this.FrmCompContRecepcionLotes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewLotes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewLotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNuevo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonNuevo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewLotes;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private Telerik.WinControls.UI.RadButton radButtonActualizar;
        private Telerik.WinControls.UI.RadButton radButtonEliminar;
    }
}