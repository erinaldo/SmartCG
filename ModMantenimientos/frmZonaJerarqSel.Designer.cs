namespace ModMantenimientos
{
    partial class frmZonaJerarqSel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmZonaJerarqSel));
            this.lblClaseZona = new System.Windows.Forms.Label();
            this.lblClaseZonaDesc = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.listViewElementos = new System.Windows.Forms.ListView();
            this.tgBuscadorZona = new ObjectModel.TGBuscador();
            this.SuspendLayout();
            // 
            // lblClaseZona
            // 
            this.lblClaseZona.AutoSize = true;
            this.lblClaseZona.Location = new System.Drawing.Point(28, 29);
            this.lblClaseZona.Name = "lblClaseZona";
            this.lblClaseZona.Size = new System.Drawing.Size(74, 13);
            this.lblClaseZona.TabIndex = 0;
            this.lblClaseZona.Text = "Clase de zona";
            // 
            // lblClaseZonaDesc
            // 
            this.lblClaseZonaDesc.AutoSize = true;
            this.lblClaseZonaDesc.Location = new System.Drawing.Point(124, 29);
            this.lblClaseZonaDesc.Name = "lblClaseZonaDesc";
            this.lblClaseZonaDesc.Size = new System.Drawing.Size(65, 13);
            this.lblClaseZonaDesc.TabIndex = 1;
            this.lblClaseZonaDesc.Text = "Valor - Desc";
            // 
            // btnSalir
            // 
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Location = new System.Drawing.Point(331, 457);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(220, 457);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // listViewElementos
            // 
            this.listViewElementos.Location = new System.Drawing.Point(29, 187);
            this.listViewElementos.Name = "listViewElementos";
            this.listViewElementos.Size = new System.Drawing.Size(614, 243);
            this.listViewElementos.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewElementos.TabIndex = 3;
            this.listViewElementos.UseCompatibleStateImageBehavior = false;
            this.listViewElementos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewElementos_ColumnClick);
            this.listViewElementos.DoubleClick += new System.EventHandler(this.ListViewElementos_DoubleClick);
            // 
            // tgBuscadorZona
            // 
            this.tgBuscadorZona.CamposBusqueda = "";
            this.tgBuscadorZona.FrmPadre = null;
            this.tgBuscadorZona.Location = new System.Drawing.Point(26, 65);
            this.tgBuscadorZona.Name = "tgBuscadorZona";
            this.tgBuscadorZona.NombreColumnas = "";
            this.tgBuscadorZona.NombreColumnasCampos = null;
            this.tgBuscadorZona.NombreColumnasSel = "";
            this.tgBuscadorZona.ProveedorDatosForm = null;
            this.tgBuscadorZona.Query = null;
            this.tgBuscadorZona.Size = new System.Drawing.Size(628, 104);
            this.tgBuscadorZona.TabIndex = 6;
            this.tgBuscadorZona.TituloGrupo = "Buscador";
            this.tgBuscadorZona.TodasColumnas = true;
            this.tgBuscadorZona.TodasEtiqueta = "Todas";
            // 
            // frmZonaJerarqSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 511);
            this.Controls.Add(this.tgBuscadorZona);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.listViewElementos);
            this.Controls.Add(this.lblClaseZonaDesc);
            this.Controls.Add(this.lblClaseZona);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmZonaJerarqSel";
            this.Text = "Lista de Zonas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmZonaJerarqSel_FormClosing);
            this.Load += new System.EventHandler(this.FrmZonaJerarqSel_Load);
            this.ResizeEnd += new System.EventHandler(this.FrmZonaJerarqSel_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClaseZona;
        private System.Windows.Forms.Label lblClaseZonaDesc;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.ListView listViewElementos;
        private ObjectModel.TGBuscador tgBuscadorZona;
    }
}