namespace ObjectModel
{
    partial class TGBuscador
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGBuscador));
            this.gbBuscador = new Telerik.WinControls.UI.RadGroupBox();
            this.lblColumnasNombres = new Telerik.WinControls.UI.RadLabel();
            this.lnkColumns = new System.Windows.Forms.LinkLabel();
            this.gbModo = new System.Windows.Forms.GroupBox();
            this.rbModoComienza = new System.Windows.Forms.RadioButton();
            this.rbModoContiene = new System.Windows.Forms.RadioButton();
            this.lblCriterio = new Telerik.WinControls.UI.RadLabel();
            this.btnTodos = new Telerik.WinControls.UI.RadButton();
            this.btnBuscar = new Telerik.WinControls.UI.RadButton();
            this.txtFiltro = new Telerik.WinControls.UI.RadTextBox();
            this.lblFiltro = new Telerik.WinControls.UI.RadLabel();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.gbBuscador)).BeginInit();
            this.gbBuscador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblColumnasNombres)).BeginInit();
            this.gbModo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCriterio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTodos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBuscar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiltro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).BeginInit();
            this.SuspendLayout();
            // 
            // gbBuscador
            // 
            this.gbBuscador.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.gbBuscador.Controls.Add(this.lblColumnasNombres);
            this.gbBuscador.Controls.Add(this.lnkColumns);
            this.gbBuscador.Controls.Add(this.gbModo);
            this.gbBuscador.Controls.Add(this.lblCriterio);
            this.gbBuscador.Controls.Add(this.btnTodos);
            this.gbBuscador.Controls.Add(this.btnBuscar);
            this.gbBuscador.Controls.Add(this.txtFiltro);
            this.gbBuscador.Controls.Add(this.lblFiltro);
            this.gbBuscador.HeaderText = " Buscador ";
            this.gbBuscador.Location = new System.Drawing.Point(2, 0);
            this.gbBuscador.Name = "gbBuscador";
            this.gbBuscador.Size = new System.Drawing.Size(615, 101);
            this.gbBuscador.TabIndex = 0;
            this.gbBuscador.TabStop = false;
            this.gbBuscador.Text = " Buscador ";
            this.gbBuscador.ThemeName = "Office2013Light";
            // 
            // lblColumnasNombres
            // 
            this.lblColumnasNombres.Location = new System.Drawing.Point(79, 65);
            this.lblColumnasNombres.Name = "lblColumnasNombres";
            this.lblColumnasNombres.Size = new System.Drawing.Size(39, 19);
            this.lblColumnasNombres.TabIndex = 32;
            this.lblColumnasNombres.Text = "Todas";
            this.lblColumnasNombres.ThemeName = "Office2013Light";
            // 
            // lnkColumns
            // 
            this.lnkColumns.AutoSize = true;
            this.lnkColumns.Location = new System.Drawing.Point(26, 66);
            this.lnkColumns.Name = "lnkColumns";
            this.lnkColumns.Size = new System.Drawing.Size(61, 15);
            this.lnkColumns.TabIndex = 31;
            this.lnkColumns.TabStop = true;
            this.lnkColumns.Text = "Columnas";
            this.lnkColumns.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkColumns_LinkClicked);
            // 
            // gbModo
            // 
            this.gbModo.Controls.Add(this.rbModoComienza);
            this.gbModo.Controls.Add(this.rbModoContiene);
            this.gbModo.Location = new System.Drawing.Point(298, 11);
            this.gbModo.Name = "gbModo";
            this.gbModo.Size = new System.Drawing.Size(198, 43);
            this.gbModo.TabIndex = 20;
            this.gbModo.TabStop = false;
            // 
            // rbModoComienza
            // 
            this.rbModoComienza.AutoSize = true;
            this.rbModoComienza.Location = new System.Drawing.Point(103, 16);
            this.rbModoComienza.Name = "rbModoComienza";
            this.rbModoComienza.Size = new System.Drawing.Size(78, 19);
            this.rbModoComienza.TabIndex = 1;
            this.rbModoComienza.Text = "Comienza";
            this.rbModoComienza.UseVisualStyleBackColor = true;
            // 
            // rbModoContiene
            // 
            this.rbModoContiene.AutoSize = true;
            this.rbModoContiene.Checked = true;
            this.rbModoContiene.Location = new System.Drawing.Point(21, 16);
            this.rbModoContiene.Name = "rbModoContiene";
            this.rbModoContiene.Size = new System.Drawing.Size(73, 19);
            this.rbModoContiene.TabIndex = 0;
            this.rbModoContiene.TabStop = true;
            this.rbModoContiene.Text = "Contiene";
            this.rbModoContiene.UseVisualStyleBackColor = true;
            // 
            // lblCriterio
            // 
            this.lblCriterio.Location = new System.Drawing.Point(244, 26);
            this.lblCriterio.Name = "lblCriterio";
            this.lblCriterio.Size = new System.Drawing.Size(47, 19);
            this.lblCriterio.TabIndex = 15;
            this.lblCriterio.Text = "Criterio";
            this.lblCriterio.ThemeName = "Office2013Light";
            // 
            // btnTodos
            // 
            this.btnTodos.Location = new System.Drawing.Point(534, 59);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(75, 23);
            this.btnTodos.TabIndex = 30;
            this.btnTodos.Text = "Todos";
            this.btnTodos.ThemeName = "Office2013Light";
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.Location = new System.Drawing.Point(534, 20);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 25;
            this.btnBuscar.Text = "   Buscar";
            this.btnBuscar.ThemeName = "Office2013Light";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(79, 23);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(135, 21);
            this.txtFiltro.TabIndex = 10;
            this.txtFiltro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFiltro_KeyDown);
            this.txtFiltro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiltro_KeyPress);
            this.txtFiltro.Leave += new System.EventHandler(this.txtFiltro_Leave);
            // 
            // lblFiltro
            // 
            this.lblFiltro.Location = new System.Drawing.Point(26, 26);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(35, 19);
            this.lblFiltro.TabIndex = 5;
            this.lblFiltro.Text = "Valor";
            this.lblFiltro.ThemeName = "Office2013Light";
            // 
            // TGBuscador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbBuscador);
            this.Name = "TGBuscador";
            this.Size = new System.Drawing.Size(623, 104);
            this.Load += new System.EventHandler(this.TGBuscador_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbBuscador)).EndInit();
            this.gbBuscador.ResumeLayout(false);
            this.gbBuscador.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblColumnasNombres)).EndInit();
            this.gbModo.ResumeLayout(false);
            this.gbModo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCriterio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTodos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBuscar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiltro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox gbBuscador;
        private Telerik.WinControls.UI.RadButton btnTodos;
        private Telerik.WinControls.UI.RadButton btnBuscar;
        private Telerik.WinControls.UI.RadTextBox txtFiltro;
        private Telerik.WinControls.UI.RadLabel lblFiltro;
        private Telerik.WinControls.UI.RadLabel lblCriterio;
        private System.Windows.Forms.GroupBox gbModo;
        private System.Windows.Forms.RadioButton rbModoComienza;
        private System.Windows.Forms.RadioButton rbModoContiene;
        private Telerik.WinControls.UI.RadLabel lblColumnasNombres;
        private System.Windows.Forms.LinkLabel lnkColumns;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
    }
}
