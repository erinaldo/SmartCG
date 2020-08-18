namespace AppAdminModulos
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
            this.gBoxClaves = new System.Windows.Forms.GroupBox();
            this.btnClaveGenerar = new System.Windows.Forms.Button();
            this.btnClaveConsultar = new System.Windows.Forms.Button();
            this.gBoxModulos = new System.Windows.Forms.GroupBox();
            this.btnModuloGenerarFicheroModulo = new System.Windows.Forms.Button();
            this.btnModuloAlta = new System.Windows.Forms.Button();
            this.btnModuloEditar = new System.Windows.Forms.Button();
            this.btnModuloActivarDesactivar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.menuPrincipal = new System.Windows.Forms.MenuStrip();
            this.modulosMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altaSubMenuModulos = new System.Windows.Forms.ToolStripMenuItem();
            this.editarSubMenuModulos = new System.Windows.Forms.ToolStripMenuItem();
            this.activarDesactivarSubMenuModulos = new System.Windows.Forms.ToolStripMenuItem();
            this.generarFicheroSubMenuModulos = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altaSubMenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.editarSubMenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.generarClavesSubMenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarClaveSubMenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.listarClavesSubMenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.configuracionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bbbddSubMenuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.ficherosSubMenuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.salirMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBoxClaves.SuspendLayout();
            this.gBoxModulos.SuspendLayout();
            this.menuPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxClaves
            // 
            this.gBoxClaves.Controls.Add(this.btnClaveGenerar);
            this.gBoxClaves.Controls.Add(this.btnClaveConsultar);
            this.gBoxClaves.Location = new System.Drawing.Point(16, 398);
            this.gBoxClaves.Name = "gBoxClaves";
            this.gBoxClaves.Size = new System.Drawing.Size(236, 46);
            this.gBoxClaves.TabIndex = 7;
            this.gBoxClaves.TabStop = false;
            this.gBoxClaves.Text = " Claves ";
            this.gBoxClaves.Visible = false;
            // 
            // btnClaveGenerar
            // 
            this.btnClaveGenerar.Location = new System.Drawing.Point(38, 25);
            this.btnClaveGenerar.Name = "btnClaveGenerar";
            this.btnClaveGenerar.Size = new System.Drawing.Size(172, 33);
            this.btnClaveGenerar.TabIndex = 3;
            this.btnClaveGenerar.Text = "Generar Clave";
            this.btnClaveGenerar.UseVisualStyleBackColor = true;
            // 
            // btnClaveConsultar
            // 
            this.btnClaveConsultar.Location = new System.Drawing.Point(38, 71);
            this.btnClaveConsultar.Name = "btnClaveConsultar";
            this.btnClaveConsultar.Size = new System.Drawing.Size(172, 33);
            this.btnClaveConsultar.TabIndex = 4;
            this.btnClaveConsultar.Text = "Consultar Clave";
            this.btnClaveConsultar.UseVisualStyleBackColor = true;
            // 
            // gBoxModulos
            // 
            this.gBoxModulos.Controls.Add(this.btnModuloGenerarFicheroModulo);
            this.gBoxModulos.Controls.Add(this.btnModuloAlta);
            this.gBoxModulos.Controls.Add(this.btnModuloEditar);
            this.gBoxModulos.Controls.Add(this.btnModuloActivarDesactivar);
            this.gBoxModulos.Location = new System.Drawing.Point(288, 398);
            this.gBoxModulos.Name = "gBoxModulos";
            this.gBoxModulos.Size = new System.Drawing.Size(236, 66);
            this.gBoxModulos.TabIndex = 6;
            this.gBoxModulos.TabStop = false;
            this.gBoxModulos.Text = " Módulos ";
            this.gBoxModulos.Visible = false;
            // 
            // btnModuloGenerarFicheroModulo
            // 
            this.btnModuloGenerarFicheroModulo.Location = new System.Drawing.Point(37, 160);
            this.btnModuloGenerarFicheroModulo.Name = "btnModuloGenerarFicheroModulo";
            this.btnModuloGenerarFicheroModulo.Size = new System.Drawing.Size(173, 33);
            this.btnModuloGenerarFicheroModulo.TabIndex = 8;
            this.btnModuloGenerarFicheroModulo.Text = "Generar Fichero";
            this.btnModuloGenerarFicheroModulo.UseVisualStyleBackColor = true;
            this.btnModuloGenerarFicheroModulo.Click += new System.EventHandler(this.btnModuloGenerarFicheroModulo_Click);
            // 
            // btnModuloAlta
            // 
            this.btnModuloAlta.Location = new System.Drawing.Point(38, 29);
            this.btnModuloAlta.Name = "btnModuloAlta";
            this.btnModuloAlta.Size = new System.Drawing.Size(172, 33);
            this.btnModuloAlta.TabIndex = 0;
            this.btnModuloAlta.Text = "Alta";
            this.btnModuloAlta.UseVisualStyleBackColor = true;
            this.btnModuloAlta.Click += new System.EventHandler(this.btnModuloAlta_Click);
            // 
            // btnModuloEditar
            // 
            this.btnModuloEditar.Location = new System.Drawing.Point(38, 73);
            this.btnModuloEditar.Name = "btnModuloEditar";
            this.btnModuloEditar.Size = new System.Drawing.Size(172, 33);
            this.btnModuloEditar.TabIndex = 1;
            this.btnModuloEditar.Text = "Editar";
            this.btnModuloEditar.UseVisualStyleBackColor = true;
            this.btnModuloEditar.Click += new System.EventHandler(this.btnModuloEditar_Click);
            // 
            // btnModuloActivarDesactivar
            // 
            this.btnModuloActivarDesactivar.Location = new System.Drawing.Point(38, 117);
            this.btnModuloActivarDesactivar.Name = "btnModuloActivarDesactivar";
            this.btnModuloActivarDesactivar.Size = new System.Drawing.Size(172, 33);
            this.btnModuloActivarDesactivar.TabIndex = 2;
            this.btnModuloActivarDesactivar.Text = "Activar/Desactivar";
            this.btnModuloActivarDesactivar.UseVisualStyleBackColor = true;
            this.btnModuloActivarDesactivar.Click += new System.EventHandler(this.btnModuloActivarDesactivar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Location = new System.Drawing.Point(57, 463);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(172, 31);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Visible = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // menuPrincipal
            // 
            this.menuPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modulosMenuItem,
            this.clientesMenuItem,
            this.configuracionMenuItem,
            this.salirMenuItem});
            this.menuPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuPrincipal.Name = "menuPrincipal";
            this.menuPrincipal.Size = new System.Drawing.Size(491, 24);
            this.menuPrincipal.TabIndex = 8;
            this.menuPrincipal.Text = "menuStrip1";
            // 
            // modulosMenuItem
            // 
            this.modulosMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.altaSubMenuModulos,
            this.editarSubMenuModulos,
            this.activarDesactivarSubMenuModulos,
            this.generarFicheroSubMenuModulos});
            this.modulosMenuItem.Name = "modulosMenuItem";
            this.modulosMenuItem.Size = new System.Drawing.Size(66, 20);
            this.modulosMenuItem.Text = "Módulos";
            // 
            // altaSubMenuModulos
            // 
            this.altaSubMenuModulos.Name = "altaSubMenuModulos";
            this.altaSubMenuModulos.Size = new System.Drawing.Size(176, 22);
            this.altaSubMenuModulos.Text = "Alta";
            this.altaSubMenuModulos.Click += new System.EventHandler(this.altaSubMenuModulos_Click);
            // 
            // editarSubMenuModulos
            // 
            this.editarSubMenuModulos.Name = "editarSubMenuModulos";
            this.editarSubMenuModulos.Size = new System.Drawing.Size(176, 22);
            this.editarSubMenuModulos.Text = "Editar";
            this.editarSubMenuModulos.Click += new System.EventHandler(this.editarSubMenuModulos_Click);
            // 
            // activarDesactivarSubMenuModulos
            // 
            this.activarDesactivarSubMenuModulos.Name = "activarDesactivarSubMenuModulos";
            this.activarDesactivarSubMenuModulos.Size = new System.Drawing.Size(176, 22);
            this.activarDesactivarSubMenuModulos.Text = "Activar / Desactivar";
            this.activarDesactivarSubMenuModulos.Click += new System.EventHandler(this.activarDesactivarSubMenuModulos_Click);
            // 
            // generarFicheroSubMenuModulos
            // 
            this.generarFicheroSubMenuModulos.Name = "generarFicheroSubMenuModulos";
            this.generarFicheroSubMenuModulos.Size = new System.Drawing.Size(176, 22);
            this.generarFicheroSubMenuModulos.Text = "Generar Fichero";
            this.generarFicheroSubMenuModulos.Click += new System.EventHandler(this.generarFicheroSubMenuModulos_Click);
            // 
            // clientesMenuItem
            // 
            this.clientesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.altaSubMenuClientes,
            this.editarSubMenuClientes,
            this.generarClavesSubMenuClientes,
            this.consultarClaveSubMenuClientes,
            this.listarClavesSubMenuClientes});
            this.clientesMenuItem.Name = "clientesMenuItem";
            this.clientesMenuItem.Size = new System.Drawing.Size(61, 20);
            this.clientesMenuItem.Text = "Clientes";
            // 
            // altaSubMenuClientes
            // 
            this.altaSubMenuClientes.Name = "altaSubMenuClientes";
            this.altaSubMenuClientes.Size = new System.Drawing.Size(157, 22);
            this.altaSubMenuClientes.Text = "Alta";
            this.altaSubMenuClientes.Click += new System.EventHandler(this.altaSubMenuClientes_Click);
            // 
            // editarSubMenuClientes
            // 
            this.editarSubMenuClientes.Name = "editarSubMenuClientes";
            this.editarSubMenuClientes.Size = new System.Drawing.Size(157, 22);
            this.editarSubMenuClientes.Text = "Editar";
            this.editarSubMenuClientes.Click += new System.EventHandler(this.editarSubMenuClientes_Click);
            // 
            // generarClavesSubMenuClientes
            // 
            this.generarClavesSubMenuClientes.Name = "generarClavesSubMenuClientes";
            this.generarClavesSubMenuClientes.Size = new System.Drawing.Size(157, 22);
            this.generarClavesSubMenuClientes.Text = "Generar Clave";
            this.generarClavesSubMenuClientes.Click += new System.EventHandler(this.generarClavesSubMenuClientes_Click);
            // 
            // consultarClaveSubMenuClientes
            // 
            this.consultarClaveSubMenuClientes.Name = "consultarClaveSubMenuClientes";
            this.consultarClaveSubMenuClientes.Size = new System.Drawing.Size(157, 22);
            this.consultarClaveSubMenuClientes.Text = "Consultar Clave";
            this.consultarClaveSubMenuClientes.Click += new System.EventHandler(this.consultarClaveSubMenuClientes_Click);
            // 
            // listarClavesSubMenuClientes
            // 
            this.listarClavesSubMenuClientes.Name = "listarClavesSubMenuClientes";
            this.listarClavesSubMenuClientes.Size = new System.Drawing.Size(157, 22);
            this.listarClavesSubMenuClientes.Text = "Listar Claves";
            this.listarClavesSubMenuClientes.Click += new System.EventHandler(this.listarClavesSubMenuClientes_Click);
            // 
            // configuracionMenuItem
            // 
            this.configuracionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bbbddSubMenuConfig,
            this.ficherosSubMenuConfig});
            this.configuracionMenuItem.Name = "configuracionMenuItem";
            this.configuracionMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configuracionMenuItem.Text = "Configuración";
            // 
            // bbbddSubMenuConfig
            // 
            this.bbbddSubMenuConfig.Name = "bbbddSubMenuConfig";
            this.bbbddSubMenuConfig.Size = new System.Drawing.Size(146, 22);
            this.bbbddSubMenuConfig.Text = "Base de datos";
            this.bbbddSubMenuConfig.Click += new System.EventHandler(this.bbbddSubMenuConfig_Click);
            // 
            // ficherosSubMenuConfig
            // 
            this.ficherosSubMenuConfig.Name = "ficherosSubMenuConfig";
            this.ficherosSubMenuConfig.Size = new System.Drawing.Size(146, 22);
            this.ficherosSubMenuConfig.Text = "Ficheros";
            this.ficherosSubMenuConfig.Click += new System.EventHandler(this.ficherosSubMenuConfig_Click);
            // 
            // salirMenuItem
            // 
            this.salirMenuItem.Name = "salirMenuItem";
            this.salirMenuItem.Size = new System.Drawing.Size(41, 20);
            this.salirMenuItem.Text = "Salir";
            this.salirMenuItem.Click += new System.EventHandler(this.salirMenuItem_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(491, 502);
            this.Controls.Add(this.gBoxClaves);
            this.Controls.Add(this.gBoxModulos);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.menuPrincipal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuPrincipal;
            this.Name = "frmPrincipal";
            this.Text = "Administrar Módulos";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.gBoxClaves.ResumeLayout(false);
            this.gBoxModulos.ResumeLayout(false);
            this.menuPrincipal.ResumeLayout(false);
            this.menuPrincipal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnModuloAlta;
        private System.Windows.Forms.Button btnModuloEditar;
        private System.Windows.Forms.Button btnModuloActivarDesactivar;
        private System.Windows.Forms.Button btnClaveGenerar;
        private System.Windows.Forms.Button btnClaveConsultar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.GroupBox gBoxModulos;
        private System.Windows.Forms.GroupBox gBoxClaves;
        private System.Windows.Forms.Button btnModuloGenerarFicheroModulo;
        private System.Windows.Forms.MenuStrip menuPrincipal;
        private System.Windows.Forms.ToolStripMenuItem modulosMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altaSubMenuModulos;
        private System.Windows.Forms.ToolStripMenuItem editarSubMenuModulos;
        private System.Windows.Forms.ToolStripMenuItem activarDesactivarSubMenuModulos;
        private System.Windows.Forms.ToolStripMenuItem generarFicheroSubMenuModulos;
        private System.Windows.Forms.ToolStripMenuItem clientesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altaSubMenuClientes;
        private System.Windows.Forms.ToolStripMenuItem editarSubMenuClientes;
        private System.Windows.Forms.ToolStripMenuItem generarClavesSubMenuClientes;
        private System.Windows.Forms.ToolStripMenuItem configuracionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultarClaveSubMenuClientes;
        private System.Windows.Forms.ToolStripMenuItem listarClavesSubMenuClientes;
        private System.Windows.Forms.ToolStripMenuItem bbbddSubMenuConfig;
        private System.Windows.Forms.ToolStripMenuItem ficherosSubMenuConfig;
    }
}

