namespace ModConsultaInforme
{
    partial class frmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItemInformes = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemDiarioDetallado = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemDiarioResFecha = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemDiarioResPeriodo = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemBalanceSumSaldos = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemMayorContab = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemMovimientosAux = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemMovIVA = new System.Windows.Forms.ToolStripMenuItem();
            this.gruposDeInformesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solicitudDeInformesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listaDeInformesGeneradosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConsultas = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsAuxiliar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInformes,
            this.menuItemConsultas,
            this.menuItemConfig,
            this.menuItemAbout,
            this.menuItemSalir});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(581, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemInformes
            // 
            this.menuItemInformes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemDiarioDetallado,
            this.subMenuItemDiarioResFecha,
            this.subMenuItemDiarioResPeriodo,
            this.subMenuItemBalanceSumSaldos,
            this.subMenuItemMayorContab,
            this.subMenuItemMovimientosAux,
            this.subMenuItemMovIVA,
            this.gruposDeInformesToolStripMenuItem});
            this.menuItemInformes.Name = "menuItemInformes";
            this.menuItemInformes.Size = new System.Drawing.Size(66, 20);
            this.menuItemInformes.Text = "&Informes";
            // 
            // subMenuItemDiarioDetallado
            // 
            this.subMenuItemDiarioDetallado.Name = "subMenuItemDiarioDetallado";
            this.subMenuItemDiarioDetallado.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemDiarioDetallado.Text = "Diario &Detallado";
            this.subMenuItemDiarioDetallado.Click += new System.EventHandler(this.subMenuItemDiarioDetallado_Click);
            // 
            // subMenuItemDiarioResFecha
            // 
            this.subMenuItemDiarioResFecha.Name = "subMenuItemDiarioResFecha";
            this.subMenuItemDiarioResFecha.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemDiarioResFecha.Text = "Diario Resumido &Fecha";
            this.subMenuItemDiarioResFecha.Click += new System.EventHandler(this.subMenuItemDiarioResFecha_Click);
            // 
            // subMenuItemDiarioResPeriodo
            // 
            this.subMenuItemDiarioResPeriodo.Name = "subMenuItemDiarioResPeriodo";
            this.subMenuItemDiarioResPeriodo.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemDiarioResPeriodo.Text = "Diario Resumido &Periodo";
            this.subMenuItemDiarioResPeriodo.Click += new System.EventHandler(this.subMenuItemDiarioResPeriodo_Click);
            // 
            // subMenuItemBalanceSumSaldos
            // 
            this.subMenuItemBalanceSumSaldos.Name = "subMenuItemBalanceSumSaldos";
            this.subMenuItemBalanceSumSaldos.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemBalanceSumSaldos.Text = "&Balance Sumas y Saldos";
            this.subMenuItemBalanceSumSaldos.Click += new System.EventHandler(this.subMenuItemBalanceSumSaldos_Click);
            // 
            // subMenuItemMayorContab
            // 
            this.subMenuItemMayorContab.Name = "subMenuItemMayorContab";
            this.subMenuItemMayorContab.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemMayorContab.Text = "&Mayor de Contabilidad";
            this.subMenuItemMayorContab.Click += new System.EventHandler(this.subMenuItemMayorContab_Click);
            // 
            // subMenuItemMovimientosAux
            // 
            this.subMenuItemMovimientosAux.Name = "subMenuItemMovimientosAux";
            this.subMenuItemMovimientosAux.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemMovimientosAux.Text = "Movimientos de &Auxiliar";
            this.subMenuItemMovimientosAux.Click += new System.EventHandler(this.subMenuItemMovimientosAux_Click);
            // 
            // subMenuItemMovIVA
            // 
            this.subMenuItemMovIVA.Name = "subMenuItemMovIVA";
            this.subMenuItemMovIVA.Size = new System.Drawing.Size(205, 22);
            this.subMenuItemMovIVA.Text = "Movimientos de &IVA";
            this.subMenuItemMovIVA.Click += new System.EventHandler(this.subMenuItemMovIVA_Click);
            // 
            // gruposDeInformesToolStripMenuItem
            // 
            this.gruposDeInformesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solicitudDeInformesToolStripMenuItem,
            this.listaDeInformesGeneradosToolStripMenuItem});
            this.gruposDeInformesToolStripMenuItem.Name = "gruposDeInformesToolStripMenuItem";
            this.gruposDeInformesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.gruposDeInformesToolStripMenuItem.Text = "&Grupos de Informes";
            // 
            // solicitudDeInformesToolStripMenuItem
            // 
            this.solicitudDeInformesToolStripMenuItem.Name = "solicitudDeInformesToolStripMenuItem";
            this.solicitudDeInformesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.solicitudDeInformesToolStripMenuItem.Text = "&Solicitud de informes";
            this.solicitudDeInformesToolStripMenuItem.Click += new System.EventHandler(this.solicitudDeInformesToolStripMenuItem_Click);
            // 
            // listaDeInformesGeneradosToolStripMenuItem
            // 
            this.listaDeInformesGeneradosToolStripMenuItem.Name = "listaDeInformesGeneradosToolStripMenuItem";
            this.listaDeInformesGeneradosToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.listaDeInformesGeneradosToolStripMenuItem.Text = "&Lista de informes generados";
            this.listaDeInformesGeneradosToolStripMenuItem.Click += new System.EventHandler(this.listaDeInformesGeneradosToolStripMenuItem_Click);
            // 
            // menuItemConsultas
            // 
            this.menuItemConsultas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemConsAuxiliar});
            this.menuItemConsultas.Name = "menuItemConsultas";
            this.menuItemConsultas.Size = new System.Drawing.Size(71, 20);
            this.menuItemConsultas.Text = "Consul&tas";
            // 
            // subMenuItemConsAuxiliar
            // 
            this.subMenuItemConsAuxiliar.Name = "subMenuItemConsAuxiliar";
            this.subMenuItemConsAuxiliar.Size = new System.Drawing.Size(184, 22);
            this.subMenuItemConsAuxiliar.Text = "Consultas de &Auxiliar";
            this.subMenuItemConsAuxiliar.Click += new System.EventHandler(this.subMenuItemConsAuxiliar_Click);
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Name = "menuItemConfig";
            this.menuItemConfig.Size = new System.Drawing.Size(95, 20);
            this.menuItemConfig.Text = "&Configuración";
            this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(71, 20);
            this.menuItemAbout.Text = "&Acerca de";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemSalir
            // 
            this.menuItemSalir.Name = "menuItemSalir";
            this.menuItemSalir.Size = new System.Drawing.Size(41, 20);
            this.menuItemSalir.Text = "&Salir";
            this.menuItemSalir.Click += new System.EventHandler(this.menuItemSalir_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(581, 346);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPrincipal";
            this.Text = "Módulo de Consultas e Informes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPrincipal_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemInformes;
        private System.Windows.Forms.ToolStripMenuItem menuItemConsultas;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemSalir;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemDiarioDetallado;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemDiarioResFecha;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemDiarioResPeriodo;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemBalanceSumSaldos;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemMayorContab;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemMovimientosAux; 
        private System.Windows.Forms.ToolStripMenuItem subMenuItemMovIVA;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfig;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsAuxiliar;
        private System.Windows.Forms.ToolStripMenuItem gruposDeInformesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solicitudDeInformesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listaDeInformesGeneradosToolStripMenuItem;
    }
}