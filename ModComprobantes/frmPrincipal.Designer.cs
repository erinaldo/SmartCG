namespace ModComprobantes
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
            this.menuItemCompContables = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemGestionCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemEditLotesCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemModelosCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemTransferirCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemTransferirCompEditadosCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemTransferirArchivoCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemImportarCompCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemListaComp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCompExtraContables = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemGestionCompExtCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemTransferirCompExtCont = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemImportarCompExtCont = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCompContables,
            this.menuItemCompExtraContables,
            this.menuItemConfig,
            this.menuItemAbout,
            this.menuItemSalir});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(566, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemCompContables
            // 
            this.menuItemCompContables.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemGestionCompCont,
            this.subMenuItemEditLotesCompCont,
            this.subMenuItemModelosCompCont,
            this.subMenuItemTransferirCompCont,
            this.subMenuItemImportarCompCont,
            this.subMenuItemListaComp});
            this.menuItemCompContables.Name = "menuItemCompContables";
            this.menuItemCompContables.Size = new System.Drawing.Size(154, 20);
            this.menuItemCompContables.Text = "Comprobantes Con&tables";
            // 
            // subMenuItemGestionCompCont
            // 
            this.subMenuItemGestionCompCont.Name = "subMenuItemGestionCompCont";
            this.subMenuItemGestionCompCont.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemGestionCompCont.Text = "&Gestión";
            this.subMenuItemGestionCompCont.Click += new System.EventHandler(this.subMenuItemGestionCompCont_Click);
            // 
            // subMenuItemEditLotesCompCont
            // 
            this.subMenuItemEditLotesCompCont.Name = "subMenuItemEditLotesCompCont";
            this.subMenuItemEditLotesCompCont.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemEditLotesCompCont.Text = "Gestión de L&otes";
            this.subMenuItemEditLotesCompCont.Click += new System.EventHandler(this.subMenuItemEditLotesCompCont_Click);
            // 
            // subMenuItemModelosCompCont
            // 
            this.subMenuItemModelosCompCont.Name = "subMenuItemModelosCompCont";
            this.subMenuItemModelosCompCont.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemModelosCompCont.Text = "&Modelos";
            this.subMenuItemModelosCompCont.Click += new System.EventHandler(this.subMenuItemModelosCompCont_Click);
            // 
            // subMenuItemTransferirCompCont
            // 
            this.subMenuItemTransferirCompCont.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemTransferirCompEditadosCompCont,
            this.subMenuItemTransferirArchivoCompCont});
            this.subMenuItemTransferirCompCont.Name = "subMenuItemTransferirCompCont";
            this.subMenuItemTransferirCompCont.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemTransferirCompCont.Text = "&Transferir a finanzas";
            // 
            // subMenuItemTransferirCompEditadosCompCont
            // 
            this.subMenuItemTransferirCompEditadosCompCont.Name = "subMenuItemTransferirCompEditadosCompCont";
            this.subMenuItemTransferirCompEditadosCompCont.Size = new System.Drawing.Size(201, 22);
            this.subMenuItemTransferirCompEditadosCompCont.Text = "&Comprobantes editados";
            this.subMenuItemTransferirCompEditadosCompCont.Click += new System.EventHandler(this.subMenuItemTransferirCompEditadosCompCont_Click);
            // 
            // subMenuItemTransferirArchivoCompCont
            // 
            this.subMenuItemTransferirArchivoCompCont.Name = "subMenuItemTransferirArchivoCompCont";
            this.subMenuItemTransferirArchivoCompCont.Size = new System.Drawing.Size(201, 22);
            this.subMenuItemTransferirArchivoCompCont.Text = "Desde &archivo externo";
            this.subMenuItemTransferirArchivoCompCont.Click += new System.EventHandler(this.subMenuItemTransferirArchivoCompCont_Click);
            // 
            // subMenuItemImportarCompCont
            // 
            this.subMenuItemImportarCompCont.Name = "subMenuItemImportarCompCont";
            this.subMenuItemImportarCompCont.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemImportarCompCont.Text = "&Importar de finanzas";
            this.subMenuItemImportarCompCont.Click += new System.EventHandler(this.subMenuItemImportarCompCont_Click);
            // 
            // subMenuItemListaComp
            // 
            this.subMenuItemListaComp.Name = "subMenuItemListaComp";
            this.subMenuItemListaComp.Size = new System.Drawing.Size(194, 22);
            this.subMenuItemListaComp.Text = "&Lista de comprobantes";
            this.subMenuItemListaComp.Click += new System.EventHandler(this.subMenuItemListaComp_Click);
            // 
            // menuItemCompExtraContables
            // 
            this.menuItemCompExtraContables.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemGestionCompExtCont,
            this.subMenuItemTransferirCompExtCont,
            this.subMenuItemImportarCompExtCont});
            this.menuItemCompExtraContables.Name = "menuItemCompExtraContables";
            this.menuItemCompExtraContables.Size = new System.Drawing.Size(177, 20);
            this.menuItemCompExtraContables.Text = "Comprobantes &Extracontables";
            // 
            // subMenuItemGestionCompExtCont
            // 
            this.subMenuItemGestionCompExtCont.Name = "subMenuItemGestionCompExtCont";
            this.subMenuItemGestionCompExtCont.Size = new System.Drawing.Size(184, 22);
            this.subMenuItemGestionCompExtCont.Text = "&Gestión";
            this.subMenuItemGestionCompExtCont.Click += new System.EventHandler(this.subMenuItemGestionCompExtCont_Click);
            // 
            // subMenuItemTransferirCompExtCont
            // 
            this.subMenuItemTransferirCompExtCont.Name = "subMenuItemTransferirCompExtCont";
            this.subMenuItemTransferirCompExtCont.Size = new System.Drawing.Size(184, 22);
            this.subMenuItemTransferirCompExtCont.Text = "&Transferir a Finanzas";
            this.subMenuItemTransferirCompExtCont.Click += new System.EventHandler(this.subMenuItemTransferirCompExtCont_Click);
            // 
            // subMenuItemImportarCompExtCont
            // 
            this.subMenuItemImportarCompExtCont.Name = "subMenuItemImportarCompExtCont";
            this.subMenuItemImportarCompExtCont.Size = new System.Drawing.Size(184, 22);
            this.subMenuItemImportarCompExtCont.Text = "&Importar de Finanzas";
            this.subMenuItemImportarCompExtCont.Click += new System.EventHandler(this.subMenuItemImportarCompExtCont_Click);
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
            this.ClientSize = new System.Drawing.Size(566, 370);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPrincipal";
            this.Text = "Módulo de Comprobantes";
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
        private System.Windows.Forms.ToolStripMenuItem menuItemCompContables;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemGestionCompCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemModelosCompCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemTransferirCompCont;
        private System.Windows.Forms.ToolStripMenuItem menuItemCompExtraContables;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemSalir;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemImportarCompCont;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfig;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemTransferirCompEditadosCompCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemTransferirArchivoCompCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemGestionCompExtCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemTransferirCompExtCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemImportarCompExtCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemEditLotesCompCont;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemListaComp;
    }
}