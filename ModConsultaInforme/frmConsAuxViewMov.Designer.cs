namespace ModConsultaInforme
{
    partial class frmConsAuxViewMov
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsAuxViewMov));
            this.grBoxProgressBar = new System.Windows.Forms.GroupBox();
            this.progressBarEspera = new ObjectModel.NewProgressBar();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewMov = new Telerik.WinControls.UI.RadGridView();
            this.ucConsAuxCab = new ModConsultaInforme.ucfrmConsAuxiliarCabecera();
            this.lblResult = new System.Windows.Forms.Label();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radLabelOpciones = new Telerik.WinControls.UI.RadLabel();
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.radButtonExportar = new Telerik.WinControls.UI.RadButton();
            this.radBtnMenuMostrarOcultar = new Telerik.WinControls.UI.RadButton();
            this.circleShape1 = new Telerik.WinControls.CircleShape();
            this.radButtonComprobante = new Telerik.WinControls.UI.RadButton();
            this.radContextMenuClickDerecho = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.grBoxProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewMov)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewMov.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // grBoxProgressBar
            // 
            this.grBoxProgressBar.Controls.Add(this.progressBarEspera);
            this.grBoxProgressBar.Location = new System.Drawing.Point(264, 551);
            this.grBoxProgressBar.Name = "grBoxProgressBar";
            this.grBoxProgressBar.Size = new System.Drawing.Size(339, 63);
            this.grBoxProgressBar.TabIndex = 126;
            this.grBoxProgressBar.TabStop = false;
            this.grBoxProgressBar.Text = " Procesando";
            this.grBoxProgressBar.Visible = false;
            // 
            // progressBarEspera
            // 
            this.progressBarEspera.Location = new System.Drawing.Point(40, 22);
            this.progressBarEspera.Maximum = 1000;
            this.progressBarEspera.Name = "progressBarEspera";
            this.progressBarEspera.Size = new System.Drawing.Size(264, 23);
            this.progressBarEspera.TabIndex = 0;
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.radPanelApp.Controls.Add(this.radGridViewMov);
            this.radPanelApp.Controls.Add(this.ucConsAuxCab);
            this.radPanelApp.Controls.Add(this.lblResult);
            this.radPanelApp.Location = new System.Drawing.Point(192, 49);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(1000, 664);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewMov
            // 
            this.radGridViewMov.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridViewMov.AutoScroll = true;
            this.radGridViewMov.Location = new System.Drawing.Point(30, 196);
            // 
            // 
            // 
            this.radGridViewMov.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewMov.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewMov.MasterTemplate.AllowEditRow = false;
            this.radGridViewMov.MasterTemplate.AllowSearchRow = true;
            this.radGridViewMov.MasterTemplate.EnableFiltering = true;
            this.radGridViewMov.MasterTemplate.MultiSelect = true;
            this.radGridViewMov.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewMov.Name = "radGridViewMov";
            this.radGridViewMov.Size = new System.Drawing.Size(920, 432);
            this.radGridViewMov.TabIndex = 10;
            this.radGridViewMov.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewMov_CellClick);
            this.radGridViewMov.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.RadGridViewMov_ContextMenuOpening);
            this.radGridViewMov.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.RadGridViewMov_DataBindingComplete);
            // 
            // ucConsAuxCab
            // 
            this.ucConsAuxCab.AAPPDesde = null;
            this.ucConsAuxCab.AAPPHasta = null;
            this.ucConsAuxCab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucConsAuxCab.AutoScroll = true;
            this.ucConsAuxCab.CompaniaCodigo = null;
            this.ucConsAuxCab.CompaniaDesc = null;
            this.ucConsAuxCab.CtaAuxCodigo = null;
            this.ucConsAuxCab.CtaAuxDesc = null;
            this.ucConsAuxCab.CtaMayorCodigo = null;
            this.ucConsAuxCab.CtaMayorDesc = null;
            this.ucConsAuxCab.Documentos = null;
            this.ucConsAuxCab.DocumentosDesc = null;
            this.ucConsAuxCab.GrupoCodigo = null;
            this.ucConsAuxCab.GrupoDesc = null;
            this.ucConsAuxCab.Location = new System.Drawing.Point(3, 16);
            this.ucConsAuxCab.Lp = null;
            this.ucConsAuxCab.MostrarCuentas = null;
            this.ucConsAuxCab.MostrarDocumentos = true;
            this.ucConsAuxCab.MostrarDocumentosDesc = null;
            this.ucConsAuxCab.MostrarGrupoSaldosTotales = true;
            this.ucConsAuxCab.MostrarGrupoSaldosTotalesME = false;
            this.ucConsAuxCab.Name = "ucConsAuxCab";
            this.ucConsAuxCab.PlanCodigo = null;
            this.ucConsAuxCab.PlanDesc = null;
            this.ucConsAuxCab.PosAux = null;
            this.ucConsAuxCab.PosAuxDesc = null;
            this.ucConsAuxCab.SaldoFinalDesc = null;
            this.ucConsAuxCab.SaldoFinalMEDesc = null;
            this.ucConsAuxCab.SaldoInicialDesc = null;
            this.ucConsAuxCab.SaldoInicialMEDesc = null;
            this.ucConsAuxCab.Size = new System.Drawing.Size(972, 178);
            this.ucConsAuxCab.TabIndex = 45;
            this.ucConsAuxCab.TipoAuxCodigo = null;
            this.ucConsAuxCab.TipoAuxDesc = null;
            this.ucConsAuxCab.TotalDebeDesc = null;
            this.ucConsAuxCab.TotalDebeMEDesc = null;
            this.ucConsAuxCab.TotalDebeMEVisible = false;
            this.ucConsAuxCab.TotalDebeVisible = true;
            this.ucConsAuxCab.TotalHaberDesc = null;
            this.ucConsAuxCab.TotalHaberMEDesc = null;
            this.ucConsAuxCab.TotalHaberMEVisible = false;
            this.ucConsAuxCab.TotalHaberVisible = true;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(32, 209);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(331, 15);
            this.lblResult.TabIndex = 82;
            this.lblResult.Text = "No existen movimientos para el criterio de selección indicado";
            this.lblResult.Visible = false;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.radPanelAcciones.Controls.Add(this.radLabelOpciones);
            this.radPanelAcciones.Controls.Add(this.radButtonSalir);
            this.radPanelAcciones.Controls.Add(this.radButtonExportar);
            this.radPanelAcciones.Controls.Add(this.radBtnMenuMostrarOcultar);
            this.radPanelAcciones.Controls.Add(this.radButtonComprobante);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 0);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(192, 809);
            this.radPanelAcciones.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelOpciones
            // 
            this.radLabelOpciones.BackColor = System.Drawing.Color.Transparent;
            this.radLabelOpciones.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radLabelOpciones.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.radLabelOpciones.Location = new System.Drawing.Point(0, 56);
            this.radLabelOpciones.Name = "radLabelOpciones";
            this.radLabelOpciones.Padding = new System.Windows.Forms.Padding(2);
            this.radLabelOpciones.Size = new System.Drawing.Size(29, 189);
            this.radLabelOpciones.TabIndex = 40;
            this.radLabelOpciones.Text = "O   P   C   I   O   N   E   S";
            this.radLabelOpciones.Visible = false;
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabelOpciones.GetChildAt(0))).Text = "O   P   C   I   O   N   E   S";
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabelOpciones.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(2);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelOpciones.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextOrientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // radButtonSalir
            // 
            this.radButtonSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSalir.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSalir.Location = new System.Drawing.Point(40, 180);
            this.radButtonSalir.Name = "radButtonSalir";
            this.radButtonSalir.Size = new System.Drawing.Size(145, 44);
            this.radButtonSalir.TabIndex = 30;
            this.radButtonSalir.Text = "Salir";
            this.radButtonSalir.Click += new System.EventHandler(this.RadButtonSalir_Click);
            this.radButtonSalir.MouseEnter += new System.EventHandler(this.RadButtonSalir_MouseEnter);
            this.radButtonSalir.MouseLeave += new System.EventHandler(this.RadButtonSalir_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSalir.GetChildAt(0))).Text = "Salir";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSalir.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonExportar
            // 
            this.radButtonExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonExportar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonExportar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonExportar.Location = new System.Drawing.Point(40, 119);
            this.radButtonExportar.Name = "radButtonExportar";
            this.radButtonExportar.Size = new System.Drawing.Size(145, 44);
            this.radButtonExportar.TabIndex = 25;
            this.radButtonExportar.Text = "Exportar";
            this.radButtonExportar.Click += new System.EventHandler(this.RadButtonExportar_Click);
            this.radButtonExportar.MouseEnter += new System.EventHandler(this.RadButtonExportar_MouseEnter);
            this.radButtonExportar.MouseLeave += new System.EventHandler(this.RadButtonExportar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExportar.GetChildAt(0))).Text = "Exportar";
            // 
            // radBtnMenuMostrarOcultar
            // 
            this.radBtnMenuMostrarOcultar.BackColor = System.Drawing.Color.Transparent;
            this.radBtnMenuMostrarOcultar.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.radBtnMenuMostrarOcultar.Image = ((System.Drawing.Image)(resources.GetObject("radBtnMenuMostrarOcultar.Image")));
            this.radBtnMenuMostrarOcultar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radBtnMenuMostrarOcultar.Location = new System.Drawing.Point(0, 12);
            this.radBtnMenuMostrarOcultar.Name = "radBtnMenuMostrarOcultar";
            // 
            // 
            // 
            this.radBtnMenuMostrarOcultar.RootElement.Shape = this.circleShape1;
            this.radBtnMenuMostrarOcultar.Size = new System.Drawing.Size(30, 29);
            this.radBtnMenuMostrarOcultar.TabIndex = 35;
            this.radBtnMenuMostrarOcultar.Click += new System.EventHandler(this.RadBtnMenuMostrarOcultar_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).Shape = this.circleShape1;
            // 
            // circleShape1
            // 
            this.circleShape1.IsRightToLeft = false;
            // 
            // radButtonComprobante
            // 
            this.radButtonComprobante.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonComprobante.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonComprobante.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonComprobante.Location = new System.Drawing.Point(40, 58);
            this.radButtonComprobante.Name = "radButtonComprobante";
            this.radButtonComprobante.Size = new System.Drawing.Size(145, 44);
            this.radButtonComprobante.TabIndex = 20;
            this.radButtonComprobante.Text = "Ver el comprobante";
            this.radButtonComprobante.Click += new System.EventHandler(this.RadButtonComprobante_Click);
            this.radButtonComprobante.MouseEnter += new System.EventHandler(this.RadButtonComprobante_MouseEnter);
            this.radButtonComprobante.MouseLeave += new System.EventHandler(this.RadButtonComprobante_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonComprobante.GetChildAt(0))).Text = "Ver el comprobante";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonComprobante.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmConsAuxViewMov
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(1242, 757);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsAuxViewMov";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta de Auxiliar - Movimientos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConsAuxViewMov_FormClosing);
            this.Load += new System.EventHandler(this.FrmConsAuxViewMov_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmConsAuxViewMov_KeyDown);
            this.grBoxProgressBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewMov.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewMov)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            this.radPanelAcciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ucfrmConsAuxiliarCabecera ucConsAuxCab;
        private System.Windows.Forms.Label lblResult;
        private ObjectModel.NewProgressBar progressBarEspera;
        private System.Windows.Forms.GroupBox grBoxProgressBar;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadLabel radLabelOpciones;
        private Telerik.WinControls.UI.RadButton radButtonSalir;
        private Telerik.WinControls.UI.RadButton radButtonExportar;
        private Telerik.WinControls.UI.RadButton radButtonComprobante;
        private Telerik.WinControls.UI.RadButton radBtnMenuMostrarOcultar;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.CircleShape circleShape1;
        private Telerik.WinControls.UI.RadGridView radGridViewMov;
        private Telerik.WinControls.UI.RadContextMenu radContextMenuClickDerecho;
    }
}