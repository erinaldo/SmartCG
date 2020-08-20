namespace ModConsultaInforme
{
    partial class frmInfoListaInformesGenerados
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblProcesandoInfo = new Telerik.WinControls.UI.RadLabel();
            this.pBarProcesandoInfo = new ObjectModel.NewProgressBar();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewInformesGenerados = new Telerik.WinControls.UI.RadGridView();
            this.radLabelNoHayInfo = new Telerik.WinControls.UI.RadLabel();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonEliminarInforme = new Telerik.WinControls.UI.RadButton();
            this.radButtonActualizarLista = new Telerik.WinControls.UI.RadButton();
            this.radButtonDescargarInforme = new Telerik.WinControls.UI.RadButton();
            this.radPanelMenuPath = new Telerik.WinControls.UI.RadPanel();
            this.radLabelTitulo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.lblProcesandoInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformesGenerados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformesGenerados.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminarInforme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizarLista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDescargarInforme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).BeginInit();
            this.radPanelMenuPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // lblProcesandoInfo
            // 
            this.lblProcesandoInfo.Location = new System.Drawing.Point(345, 233);
            this.lblProcesandoInfo.Name = "lblProcesandoInfo";
            this.lblProcesandoInfo.Size = new System.Drawing.Size(116, 19);
            this.lblProcesandoInfo.TabIndex = 141;
            this.lblProcesandoInfo.Text = "Procesando informe";
            this.lblProcesandoInfo.Visible = false;
            // 
            // pBarProcesandoInfo
            // 
            this.pBarProcesandoInfo.Location = new System.Drawing.Point(502, 230);
            this.pBarProcesandoInfo.Maximum = 10000000;
            this.pBarProcesandoInfo.Name = "pBarProcesandoInfo";
            this.pBarProcesandoInfo.Size = new System.Drawing.Size(305, 23);
            this.pBarProcesandoInfo.TabIndex = 142;
            this.pBarProcesandoInfo.Visible = false;
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radGridViewInformesGenerados);
            this.radPanelApp.Controls.Add(this.radLabelNoHayInfo);
            this.radPanelApp.Location = new System.Drawing.Point(163, 61);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(836, 474);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewInformesGenerados
            // 
            this.radGridViewInformesGenerados.AutoScroll = true;
            this.radGridViewInformesGenerados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewInformesGenerados.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridViewInformesGenerados.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewInformesGenerados.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewInformesGenerados.MasterTemplate.AllowEditRow = false;
            this.radGridViewInformesGenerados.MasterTemplate.AllowSearchRow = true;
            this.radGridViewInformesGenerados.MasterTemplate.EnableFiltering = true;
            this.radGridViewInformesGenerados.MasterTemplate.MultiSelect = true;
            this.radGridViewInformesGenerados.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewInformesGenerados.Name = "radGridViewInformesGenerados";
            this.radGridViewInformesGenerados.Size = new System.Drawing.Size(836, 474);
            this.radGridViewInformesGenerados.TabIndex = 10;
            this.radGridViewInformesGenerados.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridViewInformesGenerados_ViewCellFormatting);
            this.radGridViewInformesGenerados.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInformesGenerados_CellClick);
            this.radGridViewInformesGenerados.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewInformesGenerados_CellDoubleClick);
            this.radGridViewInformesGenerados.Leave += new System.EventHandler(this.radGridViewInformesGenerados_Leave);
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
            this.radPanelAcciones.Controls.Add(this.radButtonEliminarInforme);
            this.radPanelAcciones.Controls.Add(this.radButtonActualizarLista);
            this.radPanelAcciones.Controls.Add(this.radButtonDescargarInforme);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 45);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(163, 251);
            this.radPanelAcciones.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonEliminarInforme
            // 
            this.radButtonEliminarInforme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonEliminarInforme.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonEliminarInforme.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonEliminarInforme.Location = new System.Drawing.Point(13, 76);
            this.radButtonEliminarInforme.Name = "radButtonEliminarInforme";
            this.radButtonEliminarInforme.Size = new System.Drawing.Size(138, 44);
            this.radButtonEliminarInforme.TabIndex = 23;
            this.radButtonEliminarInforme.Text = "Eliminar Informe";
            this.radButtonEliminarInforme.Click += new System.EventHandler(this.RadButtonEliminarInforme_Click);
            this.radButtonEliminarInforme.MouseEnter += new System.EventHandler(this.RadButtonEliminarInforme_MouseEnter);
            this.radButtonEliminarInforme.MouseLeave += new System.EventHandler(this.RadButtonEliminarInforme_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonEliminarInforme.GetChildAt(0))).Text = "Eliminar Informe";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonEliminarInforme.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonActualizarLista
            // 
            this.radButtonActualizarLista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonActualizarLista.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonActualizarLista.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonActualizarLista.Location = new System.Drawing.Point(13, 136);
            this.radButtonActualizarLista.Name = "radButtonActualizarLista";
            this.radButtonActualizarLista.Size = new System.Drawing.Size(138, 44);
            this.radButtonActualizarLista.TabIndex = 24;
            this.radButtonActualizarLista.Text = "Actualizar Lista";
            this.radButtonActualizarLista.Click += new System.EventHandler(this.RadButtonActualizarLista_Click);
            this.radButtonActualizarLista.MouseEnter += new System.EventHandler(this.RadButtonActualizarLista_MouseEnter);
            this.radButtonActualizarLista.MouseLeave += new System.EventHandler(this.RadButtonActualizarLista_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonActualizarLista.GetChildAt(0))).Text = "Actualizar Lista";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonActualizarLista.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonDescargarInforme
            // 
            this.radButtonDescargarInforme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonDescargarInforme.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonDescargarInforme.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonDescargarInforme.Location = new System.Drawing.Point(13, 16);
            this.radButtonDescargarInforme.Name = "radButtonDescargarInforme";
            this.radButtonDescargarInforme.Size = new System.Drawing.Size(138, 44);
            this.radButtonDescargarInforme.TabIndex = 20;
            this.radButtonDescargarInforme.Text = "Descargar Informe";
            this.radButtonDescargarInforme.Click += new System.EventHandler(this.RadButtonDescargarInforme_Click);
            this.radButtonDescargarInforme.MouseEnter += new System.EventHandler(this.RadButtonDescargarInforme_MouseEnter);
            this.radButtonDescargarInforme.MouseLeave += new System.EventHandler(this.RadButtonDescargarInforme_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonDescargarInforme.GetChildAt(0))).Text = "Descargar Informe";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonDescargarInforme.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelMenuPath
            // 
            this.radPanelMenuPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMenuPath.Controls.Add(this.radLabelTitulo);
            this.radPanelMenuPath.Location = new System.Drawing.Point(0, 0);
            this.radPanelMenuPath.Name = "radPanelMenuPath";
            this.radPanelMenuPath.Size = new System.Drawing.Size(997, 45);
            this.radPanelMenuPath.TabIndex = 25;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMenuPath.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitulo
            // 
            this.radLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.radLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radLabelTitulo.Location = new System.Drawing.Point(13, 15);
            this.radLabelTitulo.Name = "radLabelTitulo";
            this.radLabelTitulo.Size = new System.Drawing.Size(271, 29);
            this.radLabelTitulo.TabIndex = 30;
            this.radLabelTitulo.Text = "Informes / Informes generados";
            // 
            // frmInfoListaInformesGenerados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1249, 974);
            this.Controls.Add(this.lblProcesandoInfo);
            this.Controls.Add(this.pBarProcesandoInfo);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelMenuPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInfoListaInformesGenerados";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Informes Generados";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInfoListaInformesGenerados_FormClosing);
            this.Load += new System.EventHandler(this.FrmInfoListaInformesGenerados_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lblProcesandoInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformesGenerados.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewInformesGenerados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelNoHayInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonEliminarInforme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonActualizarLista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDescargarInforme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMenuPath)).EndInit();
            this.radPanelMenuPath.ResumeLayout(false);
            this.radPanelMenuPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadButton radButtonDescargarInforme;
        private Telerik.WinControls.UI.RadPanel radPanelMenuPath;
        private Telerik.WinControls.UI.RadLabel radLabelTitulo;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewInformesGenerados;
        private Telerik.WinControls.UI.RadLabel radLabelNoHayInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadLabel lblProcesandoInfo;
        private ObjectModel.NewProgressBar pBarProcesandoInfo;
        private Telerik.WinControls.UI.RadButton radButtonActualizarLista;
        private Telerik.WinControls.UI.RadButton radButtonEliminarInforme;
    }
}