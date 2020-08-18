namespace ModConsultaInforme
{
    partial class frmConsAuxViewSaldos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsAuxViewSaldos));
            this.grBoxProgressBar = new System.Windows.Forms.GroupBox();
            this.progressBarEspera = new ObjectModel.NewProgressBar();
            this.radContextMenuClickDerecho = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewSaldos = new Telerik.WinControls.UI.RadGridView();
            this.ucConsAuxCab = new ModConsultaInforme.ucfrmConsAuxiliarCabecera();
            this.lblResult = new System.Windows.Forms.Label();
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radButtonMovimientos = new Telerik.WinControls.UI.RadButton();
            this.radBtnMenuMostrarOcultar = new Telerik.WinControls.UI.RadButton();
            this.circleShape1 = new Telerik.WinControls.CircleShape();
            this.radButtonDocTodos = new Telerik.WinControls.UI.RadButton();
            this.radLabelOpciones = new Telerik.WinControls.UI.RadLabel();
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.radButtonExportar = new Telerik.WinControls.UI.RadButton();
            this.radButtonDocNoCancel = new Telerik.WinControls.UI.RadButton();
            this.radButtonDocCancel = new Telerik.WinControls.UI.RadButton();
            this.grBoxProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSaldos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSaldos.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonMovimientos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocTodos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocNoCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocCancel)).BeginInit();
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
            this.radPanelApp.Controls.Add(this.radGridViewSaldos);
            this.radPanelApp.Controls.Add(this.ucConsAuxCab);
            this.radPanelApp.Controls.Add(this.lblResult);
            this.radPanelApp.Location = new System.Drawing.Point(192, 49);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(1159, 664);
            this.radPanelApp.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewSaldos
            // 
            this.radGridViewSaldos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridViewSaldos.AutoScroll = true;
            this.radGridViewSaldos.Location = new System.Drawing.Point(33, 207);
            // 
            // 
            // 
            this.radGridViewSaldos.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewSaldos.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewSaldos.MasterTemplate.AllowEditRow = false;
            this.radGridViewSaldos.MasterTemplate.AllowSearchRow = true;
            this.radGridViewSaldos.MasterTemplate.MultiSelect = true;
            this.radGridViewSaldos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewSaldos.Name = "radGridViewSaldos";
            this.radGridViewSaldos.Size = new System.Drawing.Size(1078, 424);
            this.radGridViewSaldos.TabIndex = 10;
            this.radGridViewSaldos.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewSaldos_CellClick);
            this.radGridViewSaldos.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.RadGridViewSaldos_ContextMenuOpening);
            this.radGridViewSaldos.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.RadGridViewSaldos_DataBindingComplete);
            // 
            // ucConsAuxCab
            // 
            this.ucConsAuxCab.AAPPDesde = null;
            this.ucConsAuxCab.AAPPHasta = null;
            this.ucConsAuxCab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ucConsAuxCab.Location = new System.Drawing.Point(6, 3);
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
            this.ucConsAuxCab.Size = new System.Drawing.Size(1131, 198);
            this.ucConsAuxCab.TabIndex = 60;
            this.ucConsAuxCab.TipoAuxCodigo = null;
            this.ucConsAuxCab.TipoAuxDesc = null;
            this.ucConsAuxCab.TotalDebeDesc = null;
            this.ucConsAuxCab.TotalDebeMEDesc = null;
            this.ucConsAuxCab.TotalDebeMEVisible = true;
            this.ucConsAuxCab.TotalDebeVisible = true;
            this.ucConsAuxCab.TotalHaberDesc = null;
            this.ucConsAuxCab.TotalHaberMEDesc = null;
            this.ucConsAuxCab.TotalHaberMEVisible = false;
            this.ucConsAuxCab.TotalHaberVisible = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(35, 213);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(288, 13);
            this.lblResult.TabIndex = 84;
            this.lblResult.Text = "No existen saldos para el criterio de selección indicado";
            this.lblResult.Visible = false;
            // 
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.radPanelAcciones.Controls.Add(this.radButtonMovimientos);
            this.radPanelAcciones.Controls.Add(this.radBtnMenuMostrarOcultar);
            this.radPanelAcciones.Controls.Add(this.radButtonDocTodos);
            this.radPanelAcciones.Controls.Add(this.radLabelOpciones);
            this.radPanelAcciones.Controls.Add(this.radButtonSalir);
            this.radPanelAcciones.Controls.Add(this.radButtonExportar);
            this.radPanelAcciones.Controls.Add(this.radButtonDocNoCancel);
            this.radPanelAcciones.Controls.Add(this.radButtonDocCancel);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 0);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(192, 678);
            this.radPanelAcciones.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonMovimientos
            // 
            this.radButtonMovimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonMovimientos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonMovimientos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonMovimientos.Location = new System.Drawing.Point(40, 241);
            this.radButtonMovimientos.Name = "radButtonMovimientos";
            this.radButtonMovimientos.Size = new System.Drawing.Size(145, 44);
            this.radButtonMovimientos.TabIndex = 35;
            this.radButtonMovimientos.Text = "Ver los Movimientos";
            this.radButtonMovimientos.TextWrap = true;
            this.radButtonMovimientos.Click += new System.EventHandler(this.RadButtonMovimientos_Click);
            this.radButtonMovimientos.MouseEnter += new System.EventHandler(this.RadButtonMovimientos_MouseEnter);
            this.radButtonMovimientos.MouseLeave += new System.EventHandler(this.RadButtonMovimientos_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonMovimientos.GetChildAt(0))).Text = "Ver los Movimientos";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonMovimientos.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
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
            this.radBtnMenuMostrarOcultar.TabIndex = 50;
            this.radBtnMenuMostrarOcultar.Click += new System.EventHandler(this.RadBtnMenuMostrarOcultar_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnMenuMostrarOcultar.GetChildAt(0))).Shape = this.circleShape1;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnMenuMostrarOcultar.GetChildAt(0).GetChildAt(0))).Shape = this.circleShape1;
            // 
            // circleShape1
            // 
            this.circleShape1.IsRightToLeft = false;
            // 
            // radButtonDocTodos
            // 
            this.radButtonDocTodos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonDocTodos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonDocTodos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonDocTodos.Location = new System.Drawing.Point(40, 180);
            this.radButtonDocTodos.Name = "radButtonDocTodos";
            this.radButtonDocTodos.Size = new System.Drawing.Size(145, 44);
            this.radButtonDocTodos.TabIndex = 30;
            this.radButtonDocTodos.Text = "Ver Todos los Documentos";
            this.radButtonDocTodos.TextWrap = true;
            this.radButtonDocTodos.Click += new System.EventHandler(this.RadButtonDocTodos_Click);
            this.radButtonDocTodos.MouseEnter += new System.EventHandler(this.RadButtonDocTodos_MouseEnter);
            this.radButtonDocTodos.MouseLeave += new System.EventHandler(this.RadButtonDocTodos_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonDocTodos.GetChildAt(0))).Text = "Ver Todos los Documentos";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonDocTodos.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radLabelOpciones
            // 
            this.radLabelOpciones.BackColor = System.Drawing.Color.Transparent;
            this.radLabelOpciones.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radLabelOpciones.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.radLabelOpciones.Location = new System.Drawing.Point(0, 84);
            this.radLabelOpciones.Name = "radLabelOpciones";
            this.radLabelOpciones.Padding = new System.Windows.Forms.Padding(2);
            this.radLabelOpciones.Size = new System.Drawing.Size(29, 189);
            this.radLabelOpciones.TabIndex = 55;
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
            this.radButtonSalir.Location = new System.Drawing.Point(40, 363);
            this.radButtonSalir.Name = "radButtonSalir";
            this.radButtonSalir.Size = new System.Drawing.Size(145, 44);
            this.radButtonSalir.TabIndex = 45;
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
            this.radButtonExportar.Location = new System.Drawing.Point(40, 302);
            this.radButtonExportar.Name = "radButtonExportar";
            this.radButtonExportar.Size = new System.Drawing.Size(145, 44);
            this.radButtonExportar.TabIndex = 40;
            this.radButtonExportar.Text = "Exportar";
            this.radButtonExportar.Click += new System.EventHandler(this.RadButtonExportar_Click);
            this.radButtonExportar.MouseEnter += new System.EventHandler(this.RadButtonExportar_MouseEnter);
            this.radButtonExportar.MouseLeave += new System.EventHandler(this.RadButtonExportar_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonExportar.GetChildAt(0))).Text = "Exportar";
            // 
            // radButtonDocNoCancel
            // 
            this.radButtonDocNoCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonDocNoCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonDocNoCancel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonDocNoCancel.Location = new System.Drawing.Point(40, 119);
            this.radButtonDocNoCancel.Name = "radButtonDocNoCancel";
            this.radButtonDocNoCancel.Size = new System.Drawing.Size(145, 44);
            this.radButtonDocNoCancel.TabIndex = 25;
            this.radButtonDocNoCancel.Text = "Ver Documentos No Cancelados";
            this.radButtonDocNoCancel.TextWrap = true;
            this.radButtonDocNoCancel.Click += new System.EventHandler(this.RadButtonDocNoCancel_Click);
            this.radButtonDocNoCancel.MouseEnter += new System.EventHandler(this.RadButtonDocNoCancel_MouseEnter);
            this.radButtonDocNoCancel.MouseLeave += new System.EventHandler(this.RadButtonDocNoCancel_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonDocNoCancel.GetChildAt(0))).Text = "Ver Documentos No Cancelados";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonDocNoCancel.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonDocCancel
            // 
            this.radButtonDocCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonDocCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonDocCancel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonDocCancel.Location = new System.Drawing.Point(40, 58);
            this.radButtonDocCancel.Name = "radButtonDocCancel";
            this.radButtonDocCancel.Size = new System.Drawing.Size(145, 44);
            this.radButtonDocCancel.TabIndex = 20;
            this.radButtonDocCancel.Text = "Ver Documentos Cancelados";
            this.radButtonDocCancel.TextWrap = true;
            this.radButtonDocCancel.Click += new System.EventHandler(this.RadButtonDocCancel_Click);
            this.radButtonDocCancel.MouseEnter += new System.EventHandler(this.RadButtonDocCancel_MouseEnter);
            this.radButtonDocCancel.MouseLeave += new System.EventHandler(this.RadButtonDocCancel_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonDocCancel.GetChildAt(0))).Text = "Ver Documentos Cancelados";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonDocCancel.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // frmConsAuxViewSaldos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(1378, 757);
            this.Controls.Add(this.radPanelApp);
            this.Controls.Add(this.radPanelAcciones);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsAuxViewSaldos";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Saldos de las Cuentas de Mayor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConsAuxViewSaldos_FormClosing);
            this.Load += new System.EventHandler(this.FrmConsAuxViewSaldos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmConsAuxViewSaldos_KeyDown);
            this.grBoxProgressBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSaldos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSaldos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            this.radPanelAcciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonMovimientos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocTodos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocNoCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonDocCancel)).EndInit();
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
        private Telerik.WinControls.UI.RadButton radButtonDocNoCancel;
        private Telerik.WinControls.UI.RadButton radButtonDocCancel;
        private Telerik.WinControls.UI.RadButton radBtnMenuMostrarOcultar;
        private Telerik.WinControls.UI.RadButton radButtonMovimientos;
        private Telerik.WinControls.UI.RadButton radButtonDocTodos;
        private Telerik.WinControls.CircleShape circleShape1;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.UI.RadGridView radGridViewSaldos;
        private Telerik.WinControls.UI.RadContextMenu radContextMenuClickDerecho;
    }
}