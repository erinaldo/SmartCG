namespace ModMantenimientos
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
            this.menuItemTablasMaestras = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemCompanias = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemGruposCompanias = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemPlanesCtas = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemTiposAuxiliares = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemClaseZona = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemZona = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemCuentasAux = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemCuentasMayor = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemMaestroCIF_DNI = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemCompaniasFiscales = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemCodigosIVA = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTablasAux = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemTablaMonedasExt = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemTablaCalendarios = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemGruposDeCuentasDeAuxiliar = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemGruposDeCuentasDeMayor = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemTiposExtracontables = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuItemTiposComprobantes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTablasMaestras,
            this.menuItemTablasAux,
            this.menuItemAbout,
            this.menuItemSalir});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(566, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemTablasMaestras
            // 
            this.menuItemTablasMaestras.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuItemCompanias,
            this.submenuItemGruposCompanias,
            this.submenuItemPlanesCtas,
            this.submenuItemTiposAuxiliares,
            this.submenuItemClaseZona,
            this.submenuItemZona,
            this.submenuItemCuentasAux,
            this.submenuItemCuentasMayor,
            this.submenuItemMaestroCIF_DNI,
            this.submenuItemCompaniasFiscales,
            this.submenuItemCodigosIVA});
            this.menuItemTablasMaestras.Name = "menuItemTablasMaestras";
            this.menuItemTablasMaestras.Size = new System.Drawing.Size(103, 20);
            this.menuItemTablasMaestras.Text = "Tablas &Maestras";
            // 
            // submenuItemCompanias
            // 
            this.submenuItemCompanias.Name = "submenuItemCompanias";
            this.submenuItemCompanias.Size = new System.Drawing.Size(189, 22);
            this.submenuItemCompanias.Text = "Compañías";
            this.submenuItemCompanias.Click += new System.EventHandler(this.submenuItemCompanias_Click);
            // 
            // submenuItemGruposCompanias
            // 
            this.submenuItemGruposCompanias.Name = "submenuItemGruposCompanias";
            this.submenuItemGruposCompanias.Size = new System.Drawing.Size(189, 22);
            this.submenuItemGruposCompanias.Text = "Grupos de compañías";
            this.submenuItemGruposCompanias.Click += new System.EventHandler(this.submenuItemGruposCompanias_Click);
            // 
            // submenuItemPlanesCtas
            // 
            this.submenuItemPlanesCtas.Name = "submenuItemPlanesCtas";
            this.submenuItemPlanesCtas.Size = new System.Drawing.Size(189, 22);
            this.submenuItemPlanesCtas.Text = "Planes de cuentas";
            this.submenuItemPlanesCtas.Click += new System.EventHandler(this.submenuItemPlanesCtas_Click);
            // 
            // submenuItemTiposAuxiliares
            // 
            this.submenuItemTiposAuxiliares.Name = "submenuItemTiposAuxiliares";
            this.submenuItemTiposAuxiliares.Size = new System.Drawing.Size(189, 22);
            this.submenuItemTiposAuxiliares.Text = "Tipos de auxiliares";
            this.submenuItemTiposAuxiliares.Click += new System.EventHandler(this.submenuItemTiposAuxiliares_Click);
            // 
            // submenuItemClaseZona
            // 
            this.submenuItemClaseZona.Name = "submenuItemClaseZona";
            this.submenuItemClaseZona.Size = new System.Drawing.Size(189, 22);
            this.submenuItemClaseZona.Text = "Clase de zona";
            this.submenuItemClaseZona.Click += new System.EventHandler(this.submenuItemClaseZona_Click);
            // 
            // submenuItemZona
            // 
            this.submenuItemZona.Name = "submenuItemZona";
            this.submenuItemZona.Size = new System.Drawing.Size(189, 22);
            this.submenuItemZona.Text = "Zona";
            this.submenuItemZona.Click += new System.EventHandler(this.submenuItemZona_Click);
            // 
            // submenuItemCuentasAux
            // 
            this.submenuItemCuentasAux.Name = "submenuItemCuentasAux";
            this.submenuItemCuentasAux.Size = new System.Drawing.Size(189, 22);
            this.submenuItemCuentasAux.Text = "Cuentas de auxiliar";
            this.submenuItemCuentasAux.Click += new System.EventHandler(this.submenuItemCuentasAux_Click);
            // 
            // submenuItemCuentasMayor
            // 
            this.submenuItemCuentasMayor.Name = "submenuItemCuentasMayor";
            this.submenuItemCuentasMayor.Size = new System.Drawing.Size(189, 22);
            this.submenuItemCuentasMayor.Text = "Cuentas de mayor";
            this.submenuItemCuentasMayor.Click += new System.EventHandler(this.submenuItemCuentasMayor_Click);
            // 
            // submenuItemMaestroCIF_DNI
            // 
            this.submenuItemMaestroCIF_DNI.Name = "submenuItemMaestroCIF_DNI";
            this.submenuItemMaestroCIF_DNI.Size = new System.Drawing.Size(189, 22);
            this.submenuItemMaestroCIF_DNI.Text = "Maestro de CIF / DNI";
            this.submenuItemMaestroCIF_DNI.Click += new System.EventHandler(this.submenuItemMaestroCIF_DNI_Click);
            // 
            // submenuItemCompaniasFiscales
            // 
            this.submenuItemCompaniasFiscales.Name = "submenuItemCompaniasFiscales";
            this.submenuItemCompaniasFiscales.Size = new System.Drawing.Size(189, 22);
            this.submenuItemCompaniasFiscales.Text = "Compañías fiscales";
            this.submenuItemCompaniasFiscales.Click += new System.EventHandler(this.submenuItemCompaniasFiscales_Click);
            // 
            // submenuItemCodigosIVA
            // 
            this.submenuItemCodigosIVA.Name = "submenuItemCodigosIVA";
            this.submenuItemCodigosIVA.Size = new System.Drawing.Size(189, 22);
            this.submenuItemCodigosIVA.Text = "Códigos de IVA";
            this.submenuItemCodigosIVA.Click += new System.EventHandler(this.submenuItemCodigosIVA_Click);
            // 
            // menuItemTablasAux
            // 
            this.menuItemTablasAux.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuItemTablaMonedasExt,
            this.submenuItemTablaCalendarios,
            this.submenuItemGruposDeCuentasDeAuxiliar,
            this.submenuItemGruposDeCuentasDeMayor,
            this.submenuItemTiposExtracontables,
            this.submenuItemTiposComprobantes});
            this.menuItemTablasAux.Name = "menuItemTablasAux";
            this.menuItemTablasAux.Size = new System.Drawing.Size(106, 20);
            this.menuItemTablasAux.Text = "&Tablas Auxiliares";
            // 
            // submenuItemTablaMonedasExt
            // 
            this.submenuItemTablaMonedasExt.Name = "submenuItemTablaMonedasExt";
            this.submenuItemTablaMonedasExt.Size = new System.Drawing.Size(230, 22);
            this.submenuItemTablaMonedasExt.Text = "Tabla de monedas extranjeras";
            this.submenuItemTablaMonedasExt.Click += new System.EventHandler(this.submenuItemTablaMonedasExt_Click);
            // 
            // submenuItemTablaCalendarios
            // 
            this.submenuItemTablaCalendarios.Name = "submenuItemTablaCalendarios";
            this.submenuItemTablaCalendarios.Size = new System.Drawing.Size(230, 22);
            this.submenuItemTablaCalendarios.Text = "Tabla de calendarios";
            this.submenuItemTablaCalendarios.Click += new System.EventHandler(this.submenuItemTablaCalendarios_Click);
            // 
            // submenuItemGruposDeCuentasDeAuxiliar
            // 
            this.submenuItemGruposDeCuentasDeAuxiliar.Name = "submenuItemGruposDeCuentasDeAuxiliar";
            this.submenuItemGruposDeCuentasDeAuxiliar.Size = new System.Drawing.Size(230, 22);
            this.submenuItemGruposDeCuentasDeAuxiliar.Text = "Grupos de cuentas de auxiliar";
            this.submenuItemGruposDeCuentasDeAuxiliar.Click += new System.EventHandler(this.gruposDeCuentasDeAuxiliarToolStripMenuItem_Click);
            // 
            // submenuItemGruposDeCuentasDeMayor
            // 
            this.submenuItemGruposDeCuentasDeMayor.Name = "submenuItemGruposDeCuentasDeMayor";
            this.submenuItemGruposDeCuentasDeMayor.Size = new System.Drawing.Size(230, 22);
            this.submenuItemGruposDeCuentasDeMayor.Text = "Grupos de cuentas de mayor";
            this.submenuItemGruposDeCuentasDeMayor.Click += new System.EventHandler(this.gruposDeCuentasDeMayorToolStripMenuItem_Click);
            // 
            // submenuItemTiposExtracontables
            // 
            this.submenuItemTiposExtracontables.Name = "submenuItemTiposExtracontables";
            this.submenuItemTiposExtracontables.Size = new System.Drawing.Size(230, 22);
            this.submenuItemTiposExtracontables.Text = "Tipos de extracontables";
            this.submenuItemTiposExtracontables.Click += new System.EventHandler(this.submenuItemTiposExtracontables_Click);
            // 
            // submenuItemTiposComprobantes
            // 
            this.submenuItemTiposComprobantes.Name = "submenuItemTiposComprobantes";
            this.submenuItemTiposComprobantes.Size = new System.Drawing.Size(230, 22);
            this.submenuItemTiposComprobantes.Text = "Tipos de comprobantes";
            this.submenuItemTiposComprobantes.Click += new System.EventHandler(this.submenuItemTiposComprobantes_Click);
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
            this.Name = "frmPrincipal";
            this.Text = "Módulo Mantenimientos";
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
        private System.Windows.Forms.ToolStripMenuItem menuItemTablasMaestras;
        private System.Windows.Forms.ToolStripMenuItem menuItemTablasAux;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemSalir;
        private System.Windows.Forms.ToolStripMenuItem submenuItemCompanias;
        private System.Windows.Forms.ToolStripMenuItem submenuItemPlanesCtas;
        private System.Windows.Forms.ToolStripMenuItem submenuItemGruposCompanias;
        private System.Windows.Forms.ToolStripMenuItem submenuItemTiposAuxiliares;
        private System.Windows.Forms.ToolStripMenuItem submenuItemClaseZona;
        private System.Windows.Forms.ToolStripMenuItem submenuItemTablaMonedasExt;
        private System.Windows.Forms.ToolStripMenuItem submenuItemTablaCalendarios;
        private System.Windows.Forms.ToolStripMenuItem submenuItemCuentasAux;
        private System.Windows.Forms.ToolStripMenuItem submenuItemGruposDeCuentasDeAuxiliar;
        private System.Windows.Forms.ToolStripMenuItem submenuItemZona;
        private System.Windows.Forms.ToolStripMenuItem submenuItemCuentasMayor;
        private System.Windows.Forms.ToolStripMenuItem submenuItemGruposDeCuentasDeMayor;
        private System.Windows.Forms.ToolStripMenuItem submenuItemTiposExtracontables;
        private System.Windows.Forms.ToolStripMenuItem submenuItemTiposComprobantes;
        private System.Windows.Forms.ToolStripMenuItem submenuItemMaestroCIF_DNI;
        private System.Windows.Forms.ToolStripMenuItem submenuItemCompaniasFiscales;
        private System.Windows.Forms.ToolStripMenuItem submenuItemCodigosIVA;
    }
}