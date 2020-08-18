namespace ModConsultaInforme
{
    partial class frmConsAuxViewDoc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsAuxViewDoc));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.grBoxProgressBar = new Telerik.WinControls.UI.RadGroupBox();
            this.progressBarEspera = new ObjectModel.NewProgressBar();
            this.radContextMenuClickDerecho = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.radPanelAcciones = new Telerik.WinControls.UI.RadPanel();
            this.radLabelOpciones = new Telerik.WinControls.UI.RadLabel();
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.radButtonExportar = new Telerik.WinControls.UI.RadButton();
            this.radBtnMenuMostrarOcultar = new Telerik.WinControls.UI.RadButton();
            this.circleShape1 = new Telerik.WinControls.CircleShape();
            this.radButtonCalcularPrdoMedioPago = new Telerik.WinControls.UI.RadButton();
            this.radButtonMovimientos = new Telerik.WinControls.UI.RadButton();
            this.radPanelApp = new Telerik.WinControls.UI.RadPanel();
            this.radGridViewDoc = new Telerik.WinControls.UI.RadGridView();
            this.ucConsAuxCab = new ModConsultaInforme.ucfrmConsAuxiliarCabecera();
            this.lblResult = new Telerik.WinControls.UI.RadLabel();
            this.gbFiltro = new Telerik.WinControls.UI.RadGroupBox();
            this.btnFiltroDocTodos = new Telerik.WinControls.UI.RadButton();
            this.btnFiltroDocPeriodo = new Telerik.WinControls.UI.RadButton();
            this.lblFiltro = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grBoxProgressBar)).BeginInit();
            this.grBoxProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).BeginInit();
            this.radPanelAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCalcularPrdoMedioPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonMovimientos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).BeginInit();
            this.radPanelApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDoc.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFiltro)).BeginInit();
            this.gbFiltro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnFiltroDocTodos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFiltroDocPeriodo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // grBoxProgressBar
            // 
            this.grBoxProgressBar.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.grBoxProgressBar.Controls.Add(this.progressBarEspera);
            this.grBoxProgressBar.HeaderText = " Procesando";
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
            // radPanelAcciones
            // 
            this.radPanelAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radPanelAcciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.radPanelAcciones.Controls.Add(this.radLabelOpciones);
            this.radPanelAcciones.Controls.Add(this.radButtonSalir);
            this.radPanelAcciones.Controls.Add(this.radButtonExportar);
            this.radPanelAcciones.Controls.Add(this.radBtnMenuMostrarOcultar);
            this.radPanelAcciones.Controls.Add(this.radButtonCalcularPrdoMedioPago);
            this.radPanelAcciones.Controls.Add(this.radButtonMovimientos);
            this.radPanelAcciones.Location = new System.Drawing.Point(0, 0);
            this.radPanelAcciones.Name = "radPanelAcciones";
            this.radPanelAcciones.Size = new System.Drawing.Size(192, 4121);
            this.radPanelAcciones.TabIndex = 25;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelAcciones.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
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
            this.radButtonSalir.Location = new System.Drawing.Point(40, 241);
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
            this.radButtonExportar.Location = new System.Drawing.Point(40, 180);
            this.radButtonExportar.Name = "radButtonExportar";
            this.radButtonExportar.Size = new System.Drawing.Size(145, 44);
            this.radButtonExportar.TabIndex = 40;
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
            this.radBtnMenuMostrarOcultar.TabIndex = 50;
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
            // radButtonCalcularPrdoMedioPago
            // 
            this.radButtonCalcularPrdoMedioPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonCalcularPrdoMedioPago.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonCalcularPrdoMedioPago.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonCalcularPrdoMedioPago.Location = new System.Drawing.Point(40, 119);
            this.radButtonCalcularPrdoMedioPago.Name = "radButtonCalcularPrdoMedioPago";
            this.radButtonCalcularPrdoMedioPago.Size = new System.Drawing.Size(145, 44);
            this.radButtonCalcularPrdoMedioPago.TabIndex = 35;
            this.radButtonCalcularPrdoMedioPago.Text = "Calcular periodo medio de pago";
            this.radButtonCalcularPrdoMedioPago.TextWrap = true;
            this.radButtonCalcularPrdoMedioPago.Click += new System.EventHandler(this.RadButtonCalcularPrdoMedioPago_Click);
            this.radButtonCalcularPrdoMedioPago.MouseEnter += new System.EventHandler(this.RadButtonCalcularPrdoMedioPago_MouseEnter);
            this.radButtonCalcularPrdoMedioPago.MouseLeave += new System.EventHandler(this.RadButtonCalcularPrdoMedioPago_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonCalcularPrdoMedioPago.GetChildAt(0))).Text = "Calcular periodo medio de pago";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonCalcularPrdoMedioPago.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radButtonMovimientos
            // 
            this.radButtonMovimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonMovimientos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonMovimientos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonMovimientos.Location = new System.Drawing.Point(40, 58);
            this.radButtonMovimientos.Name = "radButtonMovimientos";
            this.radButtonMovimientos.Size = new System.Drawing.Size(145, 44);
            this.radButtonMovimientos.TabIndex = 30;
            this.radButtonMovimientos.Text = "Ver los movimientos";
            this.radButtonMovimientos.Click += new System.EventHandler(this.RadButtonMovimientos_Click);
            this.radButtonMovimientos.MouseEnter += new System.EventHandler(this.RadButtonMovimientos_MouseEnter);
            this.radButtonMovimientos.MouseLeave += new System.EventHandler(this.RadButtonMovimientos_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonMovimientos.GetChildAt(0))).Text = "Ver los movimientos";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonMovimientos.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // radPanelApp
            // 
            this.radPanelApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelApp.AutoScroll = true;
            this.radPanelApp.Controls.Add(this.radGridViewDoc);
            this.radPanelApp.Controls.Add(this.ucConsAuxCab);
            this.radPanelApp.Controls.Add(this.lblResult);
            this.radPanelApp.Controls.Add(this.gbFiltro);
            this.radPanelApp.Location = new System.Drawing.Point(192, 49);
            this.radPanelApp.Name = "radPanelApp";
            this.radPanelApp.Size = new System.Drawing.Size(1159, 664);
            this.radPanelApp.TabIndex = 163;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelApp.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radGridViewDoc
            // 
            this.radGridViewDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridViewDoc.Location = new System.Drawing.Point(30, 197);
            // 
            // 
            // 
            this.radGridViewDoc.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewDoc.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewDoc.MasterTemplate.AllowEditRow = false;
            this.radGridViewDoc.MasterTemplate.AllowSearchRow = true;
            this.radGridViewDoc.MasterTemplate.EnableFiltering = true;
            this.radGridViewDoc.MasterTemplate.MultiSelect = true;
            this.radGridViewDoc.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewDoc.Name = "radGridViewDoc";
            this.radGridViewDoc.Size = new System.Drawing.Size(1087, 440);
            this.radGridViewDoc.TabIndex = 5;
            this.radGridViewDoc.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.RadGridViewDoc_ViewCellFormatting);
            this.radGridViewDoc.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridViewDoc_CellClick);
            this.radGridViewDoc.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.RadGridViewDoc_ContextMenuOpening);
            this.radGridViewDoc.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.RadGridViewDoc_DataBindingComplete);
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
            this.ucConsAuxCab.Location = new System.Drawing.Point(3, 1);
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
            this.ucConsAuxCab.Size = new System.Drawing.Size(1141, 159);
            this.ucConsAuxCab.TabIndex = 60;
            this.ucConsAuxCab.TipoAuxCodigo = null;
            this.ucConsAuxCab.TipoAuxDesc = null;
            this.ucConsAuxCab.TotalDebeDesc = null;
            this.ucConsAuxCab.TotalDebeMEDesc = null;
            this.ucConsAuxCab.TotalDebeMEVisible = false;
            this.ucConsAuxCab.TotalDebeVisible = false;
            this.ucConsAuxCab.TotalHaberDesc = null;
            this.ucConsAuxCab.TotalHaberMEDesc = null;
            this.ucConsAuxCab.TotalHaberMEVisible = false;
            this.ucConsAuxCab.TotalHaberVisible = false;
            // 
            // lblResult
            // 
            this.lblResult.Location = new System.Drawing.Point(32, 209);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(338, 19);
            this.lblResult.TabIndex = 82;
            this.lblResult.Text = "No existen documentos para el criterio de selección indicado";
            this.lblResult.Visible = false;
            // 
            // gbFiltro
            // 
            this.gbFiltro.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiltro.Controls.Add(this.btnFiltroDocTodos);
            this.gbFiltro.Controls.Add(this.btnFiltroDocPeriodo);
            this.gbFiltro.Controls.Add(this.lblFiltro);
            this.gbFiltro.HeaderText = " Filtro ";
            this.gbFiltro.Location = new System.Drawing.Point(30, 156);
            this.gbFiltro.Name = "gbFiltro";
            this.gbFiltro.Size = new System.Drawing.Size(1087, 39);
            this.gbFiltro.TabIndex = 10;
            this.gbFiltro.TabStop = false;
            this.gbFiltro.Text = " Filtro ";
            // 
            // btnFiltroDocTodos
            // 
            this.btnFiltroDocTodos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.btnFiltroDocTodos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFiltroDocTodos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFiltroDocTodos.Location = new System.Drawing.Point(614, 12);
            this.btnFiltroDocTodos.Name = "btnFiltroDocTodos";
            this.btnFiltroDocTodos.Size = new System.Drawing.Size(181, 23);
            this.btnFiltroDocTodos.TabIndex = 20;
            this.btnFiltroDocTodos.Text = "Todos los Documentos";
            this.btnFiltroDocTodos.Click += new System.EventHandler(this.BtnFiltroDocTodos_Click);
            this.btnFiltroDocTodos.MouseEnter += new System.EventHandler(this.BtnFiltroDocTodos_MouseEnter);
            this.btnFiltroDocTodos.MouseLeave += new System.EventHandler(this.BtnFiltroDocTodos_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnFiltroDocTodos.GetChildAt(0))).Text = "Todos los Documentos";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btnFiltroDocTodos.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // btnFiltroDocPeriodo
            // 
            this.btnFiltroDocPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.btnFiltroDocPeriodo.Enabled = false;
            this.btnFiltroDocPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFiltroDocPeriodo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFiltroDocPeriodo.Location = new System.Drawing.Point(369, 12);
            this.btnFiltroDocPeriodo.Name = "btnFiltroDocPeriodo";
            this.btnFiltroDocPeriodo.Size = new System.Drawing.Size(181, 23);
            this.btnFiltroDocPeriodo.TabIndex = 15;
            this.btnFiltroDocPeriodo.Text = "Documentos de Periodo";
            this.btnFiltroDocPeriodo.Click += new System.EventHandler(this.BtnFiltroDocPeriodo_Click);
            this.btnFiltroDocPeriodo.MouseEnter += new System.EventHandler(this.BtnFiltroDocPeriodo_MouseEnter);
            this.btnFiltroDocPeriodo.MouseLeave += new System.EventHandler(this.BtnFiltroDocPeriodo_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnFiltroDocPeriodo.GetChildAt(0))).Text = "Documentos de Periodo";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btnFiltroDocPeriodo.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // lblFiltro
            // 
            this.lblFiltro.Location = new System.Drawing.Point(50, 16);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(142, 19);
            this.lblFiltro.TabIndex = 0;
            this.lblFiltro.Text = "Documentos del Periodo";
            // 
            // frmConsAuxViewDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(1378, 757);
            this.Controls.Add(this.radPanelAcciones);
            this.Controls.Add(this.radPanelApp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsAuxViewDoc";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta de Auxiliar - Documentos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConsAuxViewDoc_FormClosing);
            this.Load += new System.EventHandler(this.FrmConsAuxViewDoc_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmConsAuxViewDoc_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grBoxProgressBar)).EndInit();
            this.grBoxProgressBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAcciones)).EndInit();
            this.radPanelAcciones.ResumeLayout(false);
            this.radPanelAcciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelOpciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExportar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnMenuMostrarOcultar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCalcularPrdoMedioPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonMovimientos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelApp)).EndInit();
            this.radPanelApp.ResumeLayout(false);
            this.radPanelApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDoc.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFiltro)).EndInit();
            this.gbFiltro.ResumeLayout(false);
            this.gbFiltro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnFiltroDocTodos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFiltroDocPeriodo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private ucfrmConsAuxiliarCabecera ucConsAuxCab;
        private Telerik.WinControls.UI.RadLabel lblResult;
        private ObjectModel.NewProgressBar progressBarEspera;
        private Telerik.WinControls.UI.RadGroupBox grBoxProgressBar;
        private Telerik.WinControls.UI.RadGroupBox gbFiltro;
        private Telerik.WinControls.UI.RadButton btnFiltroDocTodos;
        private Telerik.WinControls.UI.RadButton btnFiltroDocPeriodo;
        private Telerik.WinControls.UI.RadLabel lblFiltro;
        private Telerik.WinControls.UI.RadPanel radPanelAcciones;
        private Telerik.WinControls.UI.RadLabel radLabelOpciones;
        private Telerik.WinControls.UI.RadButton radButtonSalir;
        private Telerik.WinControls.UI.RadButton radButtonExportar;
        private Telerik.WinControls.UI.RadButton radButtonCalcularPrdoMedioPago;
        private Telerik.WinControls.UI.RadButton radButtonMovimientos;
        private Telerik.WinControls.UI.RadButton radBtnMenuMostrarOcultar;
        private Telerik.WinControls.UI.RadPanel radPanelApp;
        private Telerik.WinControls.CircleShape circleShape1;
        private Telerik.WinControls.UI.RadGridView radGridViewDoc;
        private Telerik.WinControls.UI.RadContextMenu radContextMenuClickDerecho;
    }
}