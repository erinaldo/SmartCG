namespace ModConsultaInforme
{
    partial class frmInfoSolicitudInforme
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
            this.radGridViewSolicitudes = new Telerik.WinControls.UI.RadGridView();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonListaInformes = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSolicitudes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSolicitudes.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonListaInformes)).BeginInit();
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
            this.radPanelApp.Controls.Add(this.radGridViewSolicitudes);
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 61);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(884, 534);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewSolicitudes
            // 
            this.radGridViewSolicitudes.AutoScroll = true;
            this.radGridViewSolicitudes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewSolicitudes.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridViewSolicitudes.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewSolicitudes.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewSolicitudes.MasterTemplate.AllowEditRow = false;
            this.radGridViewSolicitudes.MasterTemplate.AllowSearchRow = true;
            this.radGridViewSolicitudes.MasterTemplate.EnableFiltering = true;
            this.radGridViewSolicitudes.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewSolicitudes.Name = "radGridViewSolicitudes";
            this.radGridViewSolicitudes.Size = new System.Drawing.Size(884, 534);
            this.radGridViewSolicitudes.TabIndex = 10;
            this.radGridViewSolicitudes.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewSolicitudes_ViewCellFormatting);
            this.radGridViewSolicitudes.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewSolicitudes_CellDoubleClick);
            this.radGridViewSolicitudes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radGridViewSolicitudes_KeyPress);
            this.radGridViewSolicitudes.Leave += new System.EventHandler(this.radGridViewSolicitudes_Leave);
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(19, 30);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(193, 19);
            this.radLabelNoHayInfo.TabIndex = 196;
            this.radLabelNoHayInfo.Text = "No existen solicitudes de informes";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonListaInformes);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 284);
            this.radPanelAcciones.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonListaInformes
            // 
            this.radButtonListaInformes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonListaInformes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonListaInformes.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonListaInformes.Location = new System.Drawing.Point(13, 16);
            this.radButtonListaInformes.Name = "radButtonListaInformes";
            this.radButtonListaInformes.Size = new System.Drawing.Size(145, 44);
            this.radButtonListaInformes.TabIndex = 20;
            this.radButtonListaInformes.Text = "Lista de Informes";
            this.radButtonListaInformes.Click += new System.EventHandler(this.RadButtonListaInformes_Click);
            this.radButtonListaInformes.MouseEnter += new System.EventHandler(this.RadButtonListaInformes_MouseEnter);
            this.radButtonListaInformes.MouseLeave += new System.EventHandler(this.RadButtonListaInformes_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonListaInformes.GetChildAt(0))).Text = "Lista de Informes";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonListaInformes.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
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
            this.radLabelTitulo.Size = new System.Drawing.Size(281, 29);
            this.radLabelTitulo.TabIndex = 30;
            this.radLabelTitulo.Text = "Informes / Solicitud de Informes";
            // 
            // frmInfoSolicitudInforme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1241, 1066);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInfoSolicitudInforme";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Solicitar Grupo de Informes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInfoSolicitudInforme_FormClosing);
            this.Load += new System.EventHandler(this.FrmInfoSolicitudInforme_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSolicitudes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSolicitudes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonListaInformes)).EndInit();
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
        private Telerik.WinControls.UI.RadButton radButtonListaInformes;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewSolicitudes;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
    }
}