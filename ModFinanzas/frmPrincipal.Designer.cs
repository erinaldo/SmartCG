namespace ModFinanzas
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
            this.lblResult = new System.Windows.Forms.Label();
            this.gbFinanzas = new System.Windows.Forms.GroupBox();
            this.treeViewMenu = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarClaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.gbFinanzas.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(40, 35);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(284, 13);
            this.lblResult.TabIndex = 3;
            this.lblResult.Text = "No se pueden cargar las opciones del Módulo de Finanzas";
            this.lblResult.Visible = false;
            // 
            // gbFinanzas
            // 
            this.gbFinanzas.BackColor = System.Drawing.Color.AliceBlue;
            this.gbFinanzas.Controls.Add(this.treeViewMenu);
            this.gbFinanzas.Location = new System.Drawing.Point(40, 45);
            this.gbFinanzas.Name = "gbFinanzas";
            this.gbFinanzas.Size = new System.Drawing.Size(538, 557);
            this.gbFinanzas.TabIndex = 2;
            this.gbFinanzas.TabStop = false;
            this.gbFinanzas.Text = "  Opciones Finanzas  ";
            // 
            // treeViewMenu
            // 
            this.treeViewMenu.AllowDrop = true;
            this.treeViewMenu.BackColor = System.Drawing.Color.AliceBlue;
            this.treeViewMenu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewMenu.Location = new System.Drawing.Point(16, 30);
            this.treeViewMenu.Name = "treeViewMenu";
            this.treeViewMenu.Size = new System.Drawing.Size(500, 505);
            this.treeViewMenu.TabIndex = 1;
            this.treeViewMenu.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewMenu_ItemDrag);
            this.treeViewMenu.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMenu_NodeMouseDoubleClick);
            this.treeViewMenu.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewMenu_DragDrop);
            this.treeViewMenu.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewMenu_DragEnter);
            this.treeViewMenu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewMenu_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemConfig,
            this.menuItemAbout,
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(617, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registrarClaveToolStripMenuItem,
            this.activarToolStripMenuItem});
            this.menuItemConfig.Name = "menuItemConfig";
            this.menuItemConfig.Size = new System.Drawing.Size(95, 20);
            this.menuItemConfig.Text = "Configuración";
            // 
            // registrarClaveToolStripMenuItem
            // 
            this.registrarClaveToolStripMenuItem.Name = "registrarClaveToolStripMenuItem";
            this.registrarClaveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.registrarClaveToolStripMenuItem.Text = "Registrar Clave";
            // 
            // activarToolStripMenuItem
            // 
            this.activarToolStripMenuItem.Name = "activarToolStripMenuItem";
            this.activarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.activarToolStripMenuItem.Text = "Activar Opciones";
            this.activarToolStripMenuItem.Click += new System.EventHandler(this.activarToolStripMenuItem_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(71, 20);
            this.menuItemAbout.Text = "Acerca de";
            this.menuItemAbout.Visible = false;
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(41, 20);
            this.toolStripMenuItem2.Text = "Salir";
            this.toolStripMenuItem2.Visible = false;
            this.toolStripMenuItem2.Click += new System.EventHandler(this.menuItemSalir_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(617, 639);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.gbFinanzas);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipal";
            this.Text = "Módulo Finanzas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPrincipal_KeyDown);
            this.gbFinanzas.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfig;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.TreeView treeViewMenu;
        private System.Windows.Forms.GroupBox gbFinanzas;
        private System.Windows.Forms.ToolStripMenuItem registrarClaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activarToolStripMenuItem;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}

