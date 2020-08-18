using System.Drawing;

namespace ModConsultaInforme
{
    partial class frmConsAuxViewDocInfPrdoPago
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsAuxViewDocInfPrdoPago));
            this.radButtonSalir = new Telerik.WinControls.UI.RadButton();
            this.ucConsAuxCab = new ModConsultaInforme.ucfrmConsAuxiliarCabecera();
            this.tablaInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblValorTotalPagosPdtes = new Telerik.WinControls.UI.RadLabel();
            this.lblValorTotalPagos = new Telerik.WinControls.UI.RadLabel();
            this.lblValorRatioOperPdtesPago = new Telerik.WinControls.UI.RadLabel();
            this.lblValorRatioOperPagadas = new Telerik.WinControls.UI.RadLabel();
            this.lblValorDiasPromPago = new Telerik.WinControls.UI.RadLabel();
            this.lblTotalPagosPdtes = new Telerik.WinControls.UI.RadLabel();
            this.lblTotalPagos = new Telerik.WinControls.UI.RadLabel();
            this.lblImporte = new Telerik.WinControls.UI.RadLabel();
            this.lblRatioOpPdtePago = new Telerik.WinControls.UI.RadLabel();
            this.lblRatioOpPagadas = new Telerik.WinControls.UI.RadLabel();
            this.lblPrdoMedioPago = new Telerik.WinControls.UI.RadLabel();
            this.lblDias = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).BeginInit();
            this.tablaInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorTotalPagosPdtes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorTotalPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorRatioOperPdtesPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorRatioOperPagadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorDiasPromPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalPagosPdtes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblImporte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRatioOpPdtePago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRatioOpPagadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPrdoMedioPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSalir
            // 
            this.radButtonSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radButtonSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(223)))));
            this.radButtonSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radButtonSalir.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radButtonSalir.Location = new System.Drawing.Point(443, 566);
            this.radButtonSalir.Name = "radButtonSalir";
            this.radButtonSalir.Size = new System.Drawing.Size(145, 44);
            this.radButtonSalir.TabIndex = 5;
            this.radButtonSalir.Text = "Salir";
            this.radButtonSalir.Click += new System.EventHandler(this.RadButtonSalir_Click);
            this.radButtonSalir.MouseEnter += new System.EventHandler(this.RadButtonSalir_MouseEnter);
            this.radButtonSalir.MouseLeave += new System.EventHandler(this.RadButtonSalir_MouseLeave);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonSalir.GetChildAt(0))).Text = "Salir";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radButtonSalir.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
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
            this.ucConsAuxCab.Location = new System.Drawing.Point(1, 45);
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
            this.ucConsAuxCab.Size = new System.Drawing.Size(998, 198);
            this.ucConsAuxCab.TabIndex = 97;
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
            // tablaInfo
            // 
            this.tablaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablaInfo.AutoScroll = true;
            this.tablaInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tablaInfo.ColumnCount = 2;
            this.tablaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tablaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tablaInfo.Controls.Add(this.lblValorTotalPagosPdtes, 1, 6);
            this.tablaInfo.Controls.Add(this.lblValorTotalPagos, 1, 5);
            this.tablaInfo.Controls.Add(this.lblValorRatioOperPdtesPago, 1, 3);
            this.tablaInfo.Controls.Add(this.lblValorRatioOperPagadas, 1, 2);
            this.tablaInfo.Controls.Add(this.lblValorDiasPromPago, 1, 1);
            this.tablaInfo.Controls.Add(this.lblTotalPagosPdtes, 0, 6);
            this.tablaInfo.Controls.Add(this.lblTotalPagos, 0, 5);
            this.tablaInfo.Controls.Add(this.lblImporte, 1, 4);
            this.tablaInfo.Controls.Add(this.lblRatioOpPdtePago, 0, 3);
            this.tablaInfo.Controls.Add(this.lblRatioOpPagadas, 0, 2);
            this.tablaInfo.Controls.Add(this.lblPrdoMedioPago, 0, 1);
            this.tablaInfo.Controls.Add(this.lblDias, 1, 0);
            this.tablaInfo.Location = new System.Drawing.Point(92, 249);
            this.tablaInfo.Name = "tablaInfo";
            this.tablaInfo.RowCount = 7;
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tablaInfo.Size = new System.Drawing.Size(746, 261);
            this.tablaInfo.TabIndex = 96;
            // 
            // lblValorTotalPagosPdtes
            // 
            this.lblValorTotalPagosPdtes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorTotalPagosPdtes.Location = new System.Drawing.Point(525, 226);
            this.lblValorTotalPagosPdtes.Name = "lblValorTotalPagosPdtes";
            this.lblValorTotalPagosPdtes.Size = new System.Drawing.Size(217, 31);
            this.lblValorTotalPagosPdtes.TabIndex = 10;
            this.lblValorTotalPagosPdtes.Text = "ImportePagosPdtes";
            // 
            // lblValorTotalPagos
            // 
            this.lblValorTotalPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorTotalPagos.Location = new System.Drawing.Point(525, 189);
            this.lblValorTotalPagos.Name = "lblValorTotalPagos";
            this.lblValorTotalPagos.Size = new System.Drawing.Size(217, 30);
            this.lblValorTotalPagos.TabIndex = 9;
            this.lblValorTotalPagos.Text = "ImporteTotalPagos";
            // 
            // lblValorRatioOperPdtesPago
            // 
            this.lblValorRatioOperPdtesPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorRatioOperPdtesPago.Location = new System.Drawing.Point(525, 115);
            this.lblValorRatioOperPdtesPago.Name = "lblValorRatioOperPdtesPago";
            this.lblValorRatioOperPdtesPago.Size = new System.Drawing.Size(217, 30);
            this.lblValorRatioOperPdtesPago.TabIndex = 8;
            this.lblValorRatioOperPdtesPago.Text = "DiasRatioOperPdtesPago";
            // 
            // lblValorRatioOperPagadas
            // 
            this.lblValorRatioOperPagadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorRatioOperPagadas.Location = new System.Drawing.Point(525, 78);
            this.lblValorRatioOperPagadas.Name = "lblValorRatioOperPagadas";
            this.lblValorRatioOperPagadas.Size = new System.Drawing.Size(217, 30);
            this.lblValorRatioOperPagadas.TabIndex = 7;
            this.lblValorRatioOperPagadas.Text = "DiasRatioOperPagadas";
            // 
            // lblValorDiasPromPago
            // 
            this.lblValorDiasPromPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorDiasPromPago.Location = new System.Drawing.Point(525, 41);
            this.lblValorDiasPromPago.Name = "lblValorDiasPromPago";
            this.lblValorDiasPromPago.Size = new System.Drawing.Size(217, 30);
            this.lblValorDiasPromPago.TabIndex = 6;
            this.lblValorDiasPromPago.Text = "DiasPromPago";
            // 
            // lblTotalPagosPdtes
            // 
            this.lblTotalPagosPdtes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalPagosPdtes.Location = new System.Drawing.Point(4, 226);
            this.lblTotalPagosPdtes.Name = "lblTotalPagosPdtes";
            this.lblTotalPagosPdtes.Size = new System.Drawing.Size(514, 31);
            this.lblTotalPagosPdtes.TabIndex = 5;
            this.lblTotalPagosPdtes.Text = "Total pagos pendientes";
            // 
            // lblTotalPagos
            // 
            this.lblTotalPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalPagos.Location = new System.Drawing.Point(4, 189);
            this.lblTotalPagos.Name = "lblTotalPagos";
            this.lblTotalPagos.Size = new System.Drawing.Size(514, 30);
            this.lblTotalPagos.TabIndex = 5;
            this.lblTotalPagos.Text = "Total pagos realizados";
            // 
            // lblImporte
            // 
            this.lblImporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblImporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImporte.Location = new System.Drawing.Point(525, 152);
            this.lblImporte.Name = "lblImporte";
            this.lblImporte.Size = new System.Drawing.Size(217, 30);
            this.lblImporte.TabIndex = 4;
            this.lblImporte.Text = "Importe";
            // 
            // lblRatioOpPdtePago
            // 
            this.lblRatioOpPdtePago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRatioOpPdtePago.Location = new System.Drawing.Point(4, 115);
            this.lblRatioOpPdtePago.Name = "lblRatioOpPdtePago";
            this.lblRatioOpPdtePago.Size = new System.Drawing.Size(514, 30);
            this.lblRatioOpPdtePago.TabIndex = 3;
            this.lblRatioOpPdtePago.Text = "Ratio de operaciones pendientes de pago";
            // 
            // lblRatioOpPagadas
            // 
            this.lblRatioOpPagadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRatioOpPagadas.Location = new System.Drawing.Point(4, 78);
            this.lblRatioOpPagadas.Name = "lblRatioOpPagadas";
            this.lblRatioOpPagadas.Size = new System.Drawing.Size(514, 30);
            this.lblRatioOpPagadas.TabIndex = 2;
            this.lblRatioOpPagadas.Text = "Ratio de operaciones pagadas";
            // 
            // lblPrdoMedioPago
            // 
            this.lblPrdoMedioPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrdoMedioPago.Location = new System.Drawing.Point(4, 41);
            this.lblPrdoMedioPago.Name = "lblPrdoMedioPago";
            this.lblPrdoMedioPago.Size = new System.Drawing.Size(514, 30);
            this.lblPrdoMedioPago.TabIndex = 1;
            this.lblPrdoMedioPago.Text = "Periodo medio de pago a proveedores";
            // 
            // lblDias
            // 
            this.lblDias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDias.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDias.Location = new System.Drawing.Point(525, 4);
            this.lblDias.Name = "lblDias";
            this.lblDias.Size = new System.Drawing.Size(217, 30);
            this.lblDias.TabIndex = 0;
            this.lblDias.Text = "Días";
            // 
            // frmConsAuxViewDocInfPrdoPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(1000, 649);
            this.Controls.Add(this.radButtonSalir);
            this.Controls.Add(this.ucConsAuxCab);
            this.Controls.Add(this.tablaInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsAuxViewDocInfPrdoPago";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Información sobre el período de pago a proveedores";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConsAuxViewDocInfPrdoPago_FormClosing);
            this.Load += new System.EventHandler(this.FrmConsAuxViewDocInfPrdoPago_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmConsAuxViewDocInfPrdoPago_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSalir)).EndInit();
            this.tablaInfo.ResumeLayout(false);
            this.tablaInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorTotalPagosPdtes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorTotalPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorRatioOperPdtesPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorRatioOperPagadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblValorDiasPromPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalPagosPdtes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblImporte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRatioOpPdtePago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRatioOpPagadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPrdoMedioPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tablaInfo;
        private Telerik.WinControls.UI.RadLabel lblDias;
        private Telerik.WinControls.UI.RadLabel lblPrdoMedioPago;
        private Telerik.WinControls.UI.RadLabel lblRatioOpPagadas;
        private Telerik.WinControls.UI.RadLabel lblRatioOpPdtePago;
        private Telerik.WinControls.UI.RadLabel lblImporte;
        private Telerik.WinControls.UI.RadLabel lblTotalPagos;
        private Telerik.WinControls.UI.RadLabel lblTotalPagosPdtes;
        private Telerik.WinControls.UI.RadLabel lblValorDiasPromPago;
        private Telerik.WinControls.UI.RadLabel lblValorRatioOperPagadas;
        private Telerik.WinControls.UI.RadLabel lblValorRatioOperPdtesPago;
        private Telerik.WinControls.UI.RadLabel lblValorTotalPagos;
        private Telerik.WinControls.UI.RadLabel lblValorTotalPagosPdtes;
        private ucfrmConsAuxiliarCabecera ucConsAuxCab;
        private Telerik.WinControls.UI.RadButton radButtonSalir;
    }
}