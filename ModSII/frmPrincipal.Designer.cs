namespace ModSII
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
            this.subMenuItemSIIGestion = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemEnviarInfoSII = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.subMenuItemHcoEnviosSII = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarEnviosSII = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarInfoSII = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarFacturasPorCliente = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarFacturasAgrupadasPorCliente = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarFacturasPorProveedor = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.subMenuComprobarCuadre = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemSituacionGralSII = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemAlertasEnvios = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSoap = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemSoapRespuestaVisualizar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemSIIGestion,
            this.menuItemConfig,
            this.menuItemSoap,
            this.menuItemAbout,
            this.menuItemSalir});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(776, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // subMenuItemSIIGestion
            // 
            this.subMenuItemSIIGestion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemEnviarInfoSII,
            this.toolStripSeparator1,
            this.subMenuItemHcoEnviosSII,
            this.subMenuItemConsultarInfoSII,
            this.subMenuItemConsultarFacturasPorCliente,
            this.subMenuItemConsultarFacturasAgrupadasPorCliente,
            this.subMenuItemConsultarFacturasPorProveedor,
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor,
            this.subMenuItemConsultarEnviosSII,
            this.toolStripSeparator2,
            this.subMenuComprobarCuadre,
            this.subMenuItemSituacionGralSII,
            this.subMenuItemAlertasEnvios});
            this.subMenuItemSIIGestion.Name = "subMenuItemSIIGestion";
            this.subMenuItemSIIGestion.Size = new System.Drawing.Size(74, 20);
            this.subMenuItemSIIGestion.Text = "&SII Gestión";
            // 
            // subMenuItemEnviarInfoSII
            // 
            this.subMenuItemEnviarInfoSII.Name = "subMenuItemEnviarInfoSII";
            this.subMenuItemEnviarInfoSII.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemEnviarInfoSII.Text = "&Enviar Información al SII";
            this.subMenuItemEnviarInfoSII.Click += new System.EventHandler(this.subMenuItemEnviarInfoSII_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(353, 6);
            // 
            // subMenuItemHcoEnviosSII
            // 
            this.subMenuItemHcoEnviosSII.Name = "subMenuItemHcoEnviosSII";
            this.subMenuItemHcoEnviosSII.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemHcoEnviosSII.Text = "Consultar Datos en &Local";
            this.subMenuItemHcoEnviosSII.Click += new System.EventHandler(this.subMenuItemHcoEnviosSII_Click);
            // 
            // subMenuItemConsultarEnviosSII
            // 
            this.subMenuItemConsultarEnviosSII.Name = "subMenuItemConsultarEnviosSII";
            this.subMenuItemConsultarEnviosSII.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarEnviosSII.Text = "Consultar &Envíos al SII";
            this.subMenuItemConsultarEnviosSII.Click += new System.EventHandler(this.subMenuItemConsultarEnviosSII_Click);
            // 
            // subMenuItemConsultarInfoSII
            // 
            this.subMenuItemConsultarInfoSII.Name = "subMenuItemConsultarInfoSII";
            this.subMenuItemConsultarInfoSII.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarInfoSII.Text = "&Consultar Datos en el SII";
            this.subMenuItemConsultarInfoSII.Click += new System.EventHandler(this.subMenuItemConsultarInfoSII_Click);
            // 
            // subMenuItemConsultarFacturasPorCliente
            // 
            this.subMenuItemConsultarFacturasPorCliente.Name = "subMenuItemConsultarFacturasPorCliente";
            this.subMenuItemConsultarFacturasPorCliente.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarFacturasPorCliente.Text = "Consultar Facturas Informadas por Cliente en el SII";
            this.subMenuItemConsultarFacturasPorCliente.Click += new System.EventHandler(this.subMenuItemConsultarFacturasPorCliente_Click);
            // 
            // subMenuItemConsultarFacturasAgrupadasPorCliente
            // 
            this.subMenuItemConsultarFacturasAgrupadasPorCliente.Name = "subMenuItemConsultarFacturasAgrupadasPorCliente";
            this.subMenuItemConsultarFacturasAgrupadasPorCliente.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarFacturasAgrupadasPorCliente.Text = "Consultar Facturas Agrupadas por Cliente en el SII";
            this.subMenuItemConsultarFacturasAgrupadasPorCliente.Click += new System.EventHandler(this.subMenuItemConsultarFacturasAgrupadasPorCliente_Click);
            // 
            // subMenuItemConsultarFacturasPorProveedor
            // 
            this.subMenuItemConsultarFacturasPorProveedor.Name = "subMenuItemConsultarFacturasPorProveedor";
            this.subMenuItemConsultarFacturasPorProveedor.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarFacturasPorProveedor.Text = "Consultar Facturas Informadas por Proveedor en el SII";
            this.subMenuItemConsultarFacturasPorProveedor.Click += new System.EventHandler(this.subMenuItemConsultarFacturasPorProveedor_Click);
            // 
            // subMenuItemConsultarFacturasAgrupadasPorProveedor
            // 
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor.Name = "subMenuItemConsultarFacturasAgrupadasPorProveedor";
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor.Text = "Consultar Facturas Agrupadas por Proveedor en el SII";
            this.subMenuItemConsultarFacturasAgrupadasPorProveedor.Click += new System.EventHandler(this.subMenuItemConsultarFacturasAgrupadasPorProveedor_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(353, 6);
            // 
            // subMenuComprobarCuadre
            // 
            this.subMenuComprobarCuadre.Name = "subMenuComprobarCuadre";
            this.subMenuComprobarCuadre.Size = new System.Drawing.Size(356, 22);
            this.subMenuComprobarCuadre.Text = "Comprobar Datos &Presentados y Datos Local";
            this.subMenuComprobarCuadre.Click += new System.EventHandler(this.subMenuComprobarCuadre_Click);
            // 
            // subMenuItemSituacionGralSII
            // 
            this.subMenuItemSituacionGralSII.Name = "subMenuItemSituacionGralSII";
            this.subMenuItemSituacionGralSII.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemSituacionGralSII.Text = "&Situación General SII";
            this.subMenuItemSituacionGralSII.Click += new System.EventHandler(this.subMenuItemSituacionGralSII_Click);
            // 
            // subMenuItemAlertasEnvios
            // 
            this.subMenuItemAlertasEnvios.Name = "subMenuItemAlertasEnvios";
            this.subMenuItemAlertasEnvios.Size = new System.Drawing.Size(356, 22);
            this.subMenuItemAlertasEnvios.Text = "Alertas de envíos";
            this.subMenuItemAlertasEnvios.Click += new System.EventHandler(this.subMenuItemAlertasEnvios_Click);
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Name = "menuItemConfig";
            this.menuItemConfig.Size = new System.Drawing.Size(95, 20);
            this.menuItemConfig.Text = "&Configuración";
            this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
            // 
            // menuItemSoap
            // 
            this.menuItemSoap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItemSoapRespuestaVisualizar});
            this.menuItemSoap.Name = "menuItemSoap";
            this.menuItemSoap.Size = new System.Drawing.Size(45, 20);
            this.menuItemSoap.Text = "Soap";
            // 
            // subMenuItemSoapRespuestaVisualizar
            // 
            this.subMenuItemSoapRespuestaVisualizar.Name = "subMenuItemSoapRespuestaVisualizar";
            this.subMenuItemSoapRespuestaVisualizar.Size = new System.Drawing.Size(179, 22);
            this.subMenuItemSoapRespuestaVisualizar.Text = "Respuesta Visualizar";
            this.subMenuItemSoapRespuestaVisualizar.Visible = false;
            this.subMenuItemSoapRespuestaVisualizar.Click += new System.EventHandler(this.subMenuItemSoapRespuestaVisualizar_Click);
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
            this.ClientSize = new System.Drawing.Size(776, 421);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipal";
            this.Text = "Módulo SII (Suministro Inmediato de la Información)";
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
        private System.Windows.Forms.ToolStripMenuItem subMenuItemSIIGestion;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemEnviarInfoSII;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfig;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemSalir;
        private System.Windows.Forms.ToolStripMenuItem menuItemSoap;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemSoapRespuestaVisualizar;
        private System.Windows.Forms.ToolStripMenuItem subMenuComprobarCuadre;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemSituacionGralSII;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemAlertasEnvios;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemHcoEnviosSII;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarInfoSII;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarEnviosSII;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarFacturasPorCliente;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarFacturasAgrupadasPorCliente;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarFacturasPorProveedor;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemConsultarFacturasAgrupadasPorProveedor;
    }
}

