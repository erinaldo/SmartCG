namespace ModConsultaInforme
{
    partial class frmInfoConsFicheroGeneradoView
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
            this.radGridViewInfConsGenerados = new Telerik.WinControls.UI.RadGridView();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonActualizarLista = new Telerik.WinControls.UI.RadButton();
            this.radButtonEliminar = new Telerik.WinControls.UI.RadButton();
            this.radButtonView = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfConsGenerados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfConsGenerados.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizarLista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonView)).BeginInit();
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
            this.radPanelApp.Controls.Add(this.radGridViewInfConsGenerados);
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 61);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 474);
            this.radPanelApp.TabIndex = 163;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewInfConsGenerados
            // 
            this.radGridViewInfConsGenerados.AutoScroll = true;
            this.radGridViewInfConsGenerados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewInfConsGenerados.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridViewInfConsGenerados.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewInfConsGenerados.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewInfConsGenerados.MasterTemplate.AllowEditRow = false;
            this.radGridViewInfConsGenerados.MasterTemplate.AllowSearchRow = true;
            this.radGridViewInfConsGenerados.MasterTemplate.EnableFiltering = true;
            this.radGridViewInfConsGenerados.MasterTemplate.MultiSelect = true;
            this.radGridViewInfConsGenerados.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewInfConsGenerados.Name = "radGridViewInfConsGenerados";
            this.radGridViewInfConsGenerados.Size = new System.Drawing.Size(836, 474);
            this.radGridViewInfConsGenerados.TabIndex = 10;
            this.radGridViewInfConsGenerados.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewInfConsGenerados_ViewCellFormatting);
            this.radGridViewInfConsGenerados.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInfConsGenerados_CellClick);
            this.radGridViewInfConsGenerados.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInfConsGenerados_CellDoubleClick);
            this.radGridViewInfConsGenerados.Leave += new System.EventHandler(this.radGridViewInfConsGenerados_Leave);
            // 
            // radLabelNoHayInfo
            // 
            this.radLabelNoHayInfo.ForeColor = System.Drawing.Color.Red;
            this.radLabelNoHayInfo.Location = new System.Drawing.Point(19, 30);
            this.radLabelNoHayInfo.Name = "radLabelNoHayInfo";
            this.radLabelNoHayInfo.Size = new System.Drawing.Size(176, 19);
            this.radLabelNoHayInfo.TabIndex = 196;
            this.radLabelNoHayInfo.Text = "No existen informes generados";
            this.radLabelNoHayInfo.Visible = false;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.Controls.Add(this.radButtonActualizarLista);
            this.radPanelAcciones.Controls.Add(this.radButtonEliminar);
            this.radPanelAcciones.Controls.Add(this.radButtonView);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 251);
            this.radPanelAcciones.TabIndex = 162;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonActualizarLista
            // 
            this.radButtonActualizarLista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonActualizarLista.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonActualizarLista.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonActualizarLista.Location = new System.Drawing.Point(12, 137);
            this.radButtonActualizarLista.Name = "radButtonActualizarLista";
            this.radButtonActualizarLista.Size = new System.Drawing.Size(145, 44);
            this.radButtonActualizarLista.TabIndex = 162;
            this.radButtonActualizarLista.Text = "Actualizar Lista";
            this.radButtonActualizarLista.Click += new System.EventHandler(this.radButtonActualizarLista_Click);
            this.radButtonActualizarLista.MouseEnter += new System.EventHandler(this.radButtonActualizarLista_MouseEnter);
            this.radButtonActualizarLista.MouseLeave += new System.EventHandler(this.radButtonActualizarLista_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonActualizarLista.GetChildAt(0))).Text = "Actualizar Lista";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonActualizarLista.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonEliminar
            // 
            this.radButtonEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEliminar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEliminar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEliminar.Location = new System.Drawing.Point(13, 77);
            this.radButtonEliminar.Name = "radButtonEliminar";
            this.radButtonEliminar.Size = new System.Drawing.Size(145, 44);
            this.radButtonEliminar.TabIndex = 161;
            this.radButtonEliminar.Text = "Eliminar";
            this.radButtonEliminar.Click += new System.EventHandler(this.RadButtonEliminar_Click);
            this.radButtonEliminar.MouseEnter += new System.EventHandler(this.RadButtonEliminar_MouseEnter);
            this.radButtonEliminar.MouseLeave += new System.EventHandler(this.RadButtonEliminar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEliminar.GetChildAt(0))).Text = "Eliminar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEliminar.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonView
            // 
            this.radButtonView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonView.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonView.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonView.Location = new System.Drawing.Point(13, 16);
            this.radButtonView.Name = "radButtonView";
            this.radButtonView.Size = new System.Drawing.Size(145, 44);
            this.radButtonView.TabIndex = 20;
            this.radButtonView.Text = "Mostrar";
            this.radButtonView.Click += new System.EventHandler(this.RadButtonView_Click);
            this.radButtonView.MouseEnter += new System.EventHandler(this.RadButtonView_MouseEnter);
            this.radButtonView.MouseLeave += new System.EventHandler(this.RadButtonView_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonView.GetChildAt(0))).Text = "Mostrar";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonView.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(1012, 45);
            this.radPanelMenuPath.TabIndex = 161;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(181, 29);
            this.radLabelTitulo.TabIndex = 165;
            this.radLabelTitulo.Text = "Informes / Visualizar";
            // 
            // frmInfoConsFicheroGeneradoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1482, 1158);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInfoConsFicheroGeneradoView";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Informes / Consultas generados Visualizar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInfoConsFicheroGeneradoView_FormClosing);
            this.Load += new System.EventHandler(this.FrmInfoConsFicheroGeneradoView_Load);
            this.Shown += new System.EventHandler(this.FrmInfoConsFicheroGeneradoView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfConsGenerados.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInfConsGenerados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizarLista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonView)).EndInit();
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
        private Telerik.WinControls.UI.RadButton radButtonView;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewInfConsGenerados;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private Telerik.WinControls.UI.RadButton radButtonEliminar;
        private Telerik.WinControls.UI.RadButton radButtonActualizarLista;
    }
}
