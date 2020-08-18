namespace ObjectModel
{
    partial class TGGridBuscar
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageBuscar = new System.Windows.Forms.TabPage();
            this.gbB_Opciones = new System.Windows.Forms.GroupBox();
            this.chkB_OpcionesCeldasComp = new System.Windows.Forms.CheckBox();
            this.chkB_OpcionesMayMin = new System.Windows.Forms.CheckBox();
            this.cmbB_OpcionesBuscar = new System.Windows.Forms.ComboBox();
            this.lblB_OpcionesBuscar = new System.Windows.Forms.Label();
            this.txtB_Buscar = new System.Windows.Forms.TextBox();
            this.lblB_Buscar = new System.Windows.Forms.Label();
            this.tabPageReemplazar = new System.Windows.Forms.TabPage();
            this.txtR_Reemplazar = new System.Windows.Forms.TextBox();
            this.lblR_Reemplazar = new System.Windows.Forms.Label();
            this.gbR_Opciones = new System.Windows.Forms.GroupBox();
            this.chkR_OpcionesCeldasComp = new System.Windows.Forms.CheckBox();
            this.chkR_OpcionesMayMin = new System.Windows.Forms.CheckBox();
            this.cmbR_OpcionesBuscar = new System.Windows.Forms.ComboBox();
            this.lblR_OpcionesBuscar = new System.Windows.Forms.Label();
            this.txtR_Buscar = new System.Windows.Forms.TextBox();
            this.lblR_Buscar = new System.Windows.Forms.Label();
            this.btnBuscarSgte = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnReemplazar = new System.Windows.Forms.Button();
            this.btnReemplazarTodos = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageBuscar.SuspendLayout();
            this.gbB_Opciones.SuspendLayout();
            this.tabPageReemplazar.SuspendLayout();
            this.gbR_Opciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageBuscar);
            this.tabControl.Controls.Add(this.tabPageReemplazar);
            this.tabControl.Location = new System.Drawing.Point(25, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(668, 224);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageBuscar
            // 
            this.tabPageBuscar.Controls.Add(this.gbB_Opciones);
            this.tabPageBuscar.Controls.Add(this.txtB_Buscar);
            this.tabPageBuscar.Controls.Add(this.lblB_Buscar);
            this.tabPageBuscar.Location = new System.Drawing.Point(4, 22);
            this.tabPageBuscar.Name = "tabPageBuscar";
            this.tabPageBuscar.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBuscar.Size = new System.Drawing.Size(660, 198);
            this.tabPageBuscar.TabIndex = 0;
            this.tabPageBuscar.Text = "Buscar";
            this.tabPageBuscar.UseVisualStyleBackColor = true;
            // 
            // gbB_Opciones
            // 
            this.gbB_Opciones.Controls.Add(this.chkB_OpcionesCeldasComp);
            this.gbB_Opciones.Controls.Add(this.chkB_OpcionesMayMin);
            this.gbB_Opciones.Controls.Add(this.cmbB_OpcionesBuscar);
            this.gbB_Opciones.Controls.Add(this.lblB_OpcionesBuscar);
            this.gbB_Opciones.Location = new System.Drawing.Point(24, 97);
            this.gbB_Opciones.Name = "gbB_Opciones";
            this.gbB_Opciones.Size = new System.Drawing.Size(610, 80);
            this.gbB_Opciones.TabIndex = 5;
            this.gbB_Opciones.TabStop = false;
            this.gbB_Opciones.Text = "Opciones de búsqueda";
            // 
            // chkB_OpcionesCeldasComp
            // 
            this.chkB_OpcionesCeldasComp.AutoSize = true;
            this.chkB_OpcionesCeldasComp.Location = new System.Drawing.Point(294, 48);
            this.chkB_OpcionesCeldasComp.Name = "chkB_OpcionesCeldasComp";
            this.chkB_OpcionesCeldasComp.Size = new System.Drawing.Size(229, 17);
            this.chkB_OpcionesCeldasComp.TabIndex = 3;
            this.chkB_OpcionesCeldasComp.Text = "Coincidir con el Contenido de toda la Celda";
            this.chkB_OpcionesCeldasComp.UseVisualStyleBackColor = true;
            this.chkB_OpcionesCeldasComp.CheckedChanged += new System.EventHandler(this.chkB_OpcionesCeldasComp_CheckedChanged);
            // 
            // chkB_OpcionesMayMin
            // 
            this.chkB_OpcionesMayMin.AutoSize = true;
            this.chkB_OpcionesMayMin.Location = new System.Drawing.Point(294, 25);
            this.chkB_OpcionesMayMin.Name = "chkB_OpcionesMayMin";
            this.chkB_OpcionesMayMin.Size = new System.Drawing.Size(189, 17);
            this.chkB_OpcionesMayMin.TabIndex = 2;
            this.chkB_OpcionesMayMin.Text = "Coincidir Mayúsculas y Minúsculas";
            this.chkB_OpcionesMayMin.UseVisualStyleBackColor = true;
            this.chkB_OpcionesMayMin.CheckedChanged += new System.EventHandler(this.chkB_OpcionesMayMin_CheckedChanged);
            // 
            // cmbB_OpcionesBuscar
            // 
            this.cmbB_OpcionesBuscar.FormattingEnabled = true;
            this.cmbB_OpcionesBuscar.Location = new System.Drawing.Point(84, 31);
            this.cmbB_OpcionesBuscar.Name = "cmbB_OpcionesBuscar";
            this.cmbB_OpcionesBuscar.Size = new System.Drawing.Size(121, 21);
            this.cmbB_OpcionesBuscar.TabIndex = 1;
            this.cmbB_OpcionesBuscar.SelectedIndexChanged += new System.EventHandler(this.cmbB_OpcionesBuscar_SelectedIndexChanged);
            // 
            // lblB_OpcionesBuscar
            // 
            this.lblB_OpcionesBuscar.AutoSize = true;
            this.lblB_OpcionesBuscar.Location = new System.Drawing.Point(18, 34);
            this.lblB_OpcionesBuscar.Name = "lblB_OpcionesBuscar";
            this.lblB_OpcionesBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblB_OpcionesBuscar.TabIndex = 0;
            this.lblB_OpcionesBuscar.Text = "Buscar";
            // 
            // txtB_Buscar
            // 
            this.txtB_Buscar.Location = new System.Drawing.Point(89, 25);
            this.txtB_Buscar.Name = "txtB_Buscar";
            this.txtB_Buscar.Size = new System.Drawing.Size(424, 20);
            this.txtB_Buscar.TabIndex = 1;
            this.txtB_Buscar.TextChanged += new System.EventHandler(this.txtB_Buscar_TextChanged);
            // 
            // lblB_Buscar
            // 
            this.lblB_Buscar.AutoSize = true;
            this.lblB_Buscar.Location = new System.Drawing.Point(21, 30);
            this.lblB_Buscar.Name = "lblB_Buscar";
            this.lblB_Buscar.Size = new System.Drawing.Size(40, 13);
            this.lblB_Buscar.TabIndex = 0;
            this.lblB_Buscar.Text = "Buscar";
            // 
            // tabPageReemplazar
            // 
            this.tabPageReemplazar.Controls.Add(this.txtR_Reemplazar);
            this.tabPageReemplazar.Controls.Add(this.lblR_Reemplazar);
            this.tabPageReemplazar.Controls.Add(this.gbR_Opciones);
            this.tabPageReemplazar.Controls.Add(this.txtR_Buscar);
            this.tabPageReemplazar.Controls.Add(this.lblR_Buscar);
            this.tabPageReemplazar.Location = new System.Drawing.Point(4, 22);
            this.tabPageReemplazar.Name = "tabPageReemplazar";
            this.tabPageReemplazar.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReemplazar.Size = new System.Drawing.Size(660, 198);
            this.tabPageReemplazar.TabIndex = 1;
            this.tabPageReemplazar.Text = "Reemplazar";
            this.tabPageReemplazar.UseVisualStyleBackColor = true;
            // 
            // txtR_Reemplazar
            // 
            this.txtR_Reemplazar.Location = new System.Drawing.Point(89, 57);
            this.txtR_Reemplazar.Name = "txtR_Reemplazar";
            this.txtR_Reemplazar.Size = new System.Drawing.Size(424, 20);
            this.txtR_Reemplazar.TabIndex = 9;
            // 
            // lblR_Reemplazar
            // 
            this.lblR_Reemplazar.AutoSize = true;
            this.lblR_Reemplazar.Location = new System.Drawing.Point(21, 62);
            this.lblR_Reemplazar.Name = "lblR_Reemplazar";
            this.lblR_Reemplazar.Size = new System.Drawing.Size(63, 13);
            this.lblR_Reemplazar.TabIndex = 8;
            this.lblR_Reemplazar.Text = "Reemplazar";
            // 
            // gbR_Opciones
            // 
            this.gbR_Opciones.Controls.Add(this.chkR_OpcionesCeldasComp);
            this.gbR_Opciones.Controls.Add(this.chkR_OpcionesMayMin);
            this.gbR_Opciones.Controls.Add(this.cmbR_OpcionesBuscar);
            this.gbR_Opciones.Controls.Add(this.lblR_OpcionesBuscar);
            this.gbR_Opciones.Location = new System.Drawing.Point(24, 97);
            this.gbR_Opciones.Name = "gbR_Opciones";
            this.gbR_Opciones.Size = new System.Drawing.Size(610, 80);
            this.gbR_Opciones.TabIndex = 11;
            this.gbR_Opciones.TabStop = false;
            this.gbR_Opciones.Text = "Opciones de búsqueda";
            // 
            // chkR_OpcionesCeldasComp
            // 
            this.chkR_OpcionesCeldasComp.AutoSize = true;
            this.chkR_OpcionesCeldasComp.Location = new System.Drawing.Point(294, 48);
            this.chkR_OpcionesCeldasComp.Name = "chkR_OpcionesCeldasComp";
            this.chkR_OpcionesCeldasComp.Size = new System.Drawing.Size(229, 17);
            this.chkR_OpcionesCeldasComp.TabIndex = 12;
            this.chkR_OpcionesCeldasComp.Text = "Coincidir con el Contenido de toda la Celda";
            this.chkR_OpcionesCeldasComp.UseVisualStyleBackColor = true;
            this.chkR_OpcionesCeldasComp.CheckedChanged += new System.EventHandler(this.chkR_OpcionesCeldasComp_CheckedChanged);
            // 
            // chkR_OpcionesMayMin
            // 
            this.chkR_OpcionesMayMin.AutoSize = true;
            this.chkR_OpcionesMayMin.Location = new System.Drawing.Point(294, 25);
            this.chkR_OpcionesMayMin.Name = "chkR_OpcionesMayMin";
            this.chkR_OpcionesMayMin.Size = new System.Drawing.Size(189, 17);
            this.chkR_OpcionesMayMin.TabIndex = 11;
            this.chkR_OpcionesMayMin.Text = "Coincidir Mayúsculas y Minúsculas";
            this.chkR_OpcionesMayMin.UseVisualStyleBackColor = true;
            this.chkR_OpcionesMayMin.CheckedChanged += new System.EventHandler(this.chkR_OpcionesMayMin_CheckedChanged);
            // 
            // cmbR_OpcionesBuscar
            // 
            this.cmbR_OpcionesBuscar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbR_OpcionesBuscar.FormattingEnabled = true;
            this.cmbR_OpcionesBuscar.Location = new System.Drawing.Point(84, 31);
            this.cmbR_OpcionesBuscar.Name = "cmbR_OpcionesBuscar";
            this.cmbR_OpcionesBuscar.Size = new System.Drawing.Size(121, 21);
            this.cmbR_OpcionesBuscar.TabIndex = 10;
            this.cmbR_OpcionesBuscar.SelectedIndexChanged += new System.EventHandler(this.cmbR_OpcionesBuscar_SelectedIndexChanged);
            // 
            // lblR_OpcionesBuscar
            // 
            this.lblR_OpcionesBuscar.AutoSize = true;
            this.lblR_OpcionesBuscar.Location = new System.Drawing.Point(18, 34);
            this.lblR_OpcionesBuscar.Name = "lblR_OpcionesBuscar";
            this.lblR_OpcionesBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblR_OpcionesBuscar.TabIndex = 0;
            this.lblR_OpcionesBuscar.Text = "Buscar";
            // 
            // txtR_Buscar
            // 
            this.txtR_Buscar.Location = new System.Drawing.Point(89, 25);
            this.txtR_Buscar.Name = "txtR_Buscar";
            this.txtR_Buscar.Size = new System.Drawing.Size(424, 20);
            this.txtR_Buscar.TabIndex = 7;
            this.txtR_Buscar.TextChanged += new System.EventHandler(this.txtR_Buscar_TextChanged);
            // 
            // lblR_Buscar
            // 
            this.lblR_Buscar.AutoSize = true;
            this.lblR_Buscar.Location = new System.Drawing.Point(21, 30);
            this.lblR_Buscar.Name = "lblR_Buscar";
            this.lblR_Buscar.Size = new System.Drawing.Size(40, 13);
            this.lblR_Buscar.TabIndex = 6;
            this.lblR_Buscar.Text = "Buscar";
            // 
            // btnBuscarSgte
            // 
            this.btnBuscarSgte.Location = new System.Drawing.Point(454, 265);
            this.btnBuscarSgte.Name = "btnBuscarSgte";
            this.btnBuscarSgte.Size = new System.Drawing.Size(134, 23);
            this.btnBuscarSgte.TabIndex = 15;
            this.btnBuscarSgte.Text = "Buscar siguiente";
            this.btnBuscarSgte.UseVisualStyleBackColor = true;
            this.btnBuscarSgte.Click += new System.EventHandler(this.btnBuscarSgte_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(604, 265);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnReemplazar
            // 
            this.btnReemplazar.Location = new System.Drawing.Point(347, 265);
            this.btnReemplazar.Name = "btnReemplazar";
            this.btnReemplazar.Size = new System.Drawing.Size(75, 23);
            this.btnReemplazar.TabIndex = 14;
            this.btnReemplazar.Text = "Reemplazar";
            this.btnReemplazar.UseVisualStyleBackColor = true;
            this.btnReemplazar.Click += new System.EventHandler(this.btnReemplazar_Click);
            // 
            // btnReemplazarTodos
            // 
            this.btnReemplazarTodos.Location = new System.Drawing.Point(194, 265);
            this.btnReemplazarTodos.Name = "btnReemplazarTodos";
            this.btnReemplazarTodos.Size = new System.Drawing.Size(134, 23);
            this.btnReemplazarTodos.TabIndex = 13;
            this.btnReemplazarTodos.Text = "Reemplazar todos";
            this.btnReemplazarTodos.UseVisualStyleBackColor = true;
            this.btnReemplazarTodos.Click += new System.EventHandler(this.btnReemplazarTodos_Click);
            // 
            // TGGridBuscar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 302);
            this.Controls.Add(this.btnReemplazarTodos);
            this.Controls.Add(this.btnReemplazar);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnBuscarSgte);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TGGridBuscar";
            this.ShowIcon = false;
            this.Text = "Buscar y reemplazar";
            this.Load += new System.EventHandler(this.TGGridBuscar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TGGridBuscar_KeyDown);
            this.tabControl.ResumeLayout(false);
            this.tabPageBuscar.ResumeLayout(false);
            this.tabPageBuscar.PerformLayout();
            this.gbB_Opciones.ResumeLayout(false);
            this.gbB_Opciones.PerformLayout();
            this.tabPageReemplazar.ResumeLayout(false);
            this.tabPageReemplazar.PerformLayout();
            this.gbR_Opciones.ResumeLayout(false);
            this.gbR_Opciones.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageBuscar;
        private System.Windows.Forms.TextBox txtB_Buscar;
        private System.Windows.Forms.Label lblB_Buscar;
        private System.Windows.Forms.TabPage tabPageReemplazar;
        private System.Windows.Forms.GroupBox gbB_Opciones;
        private System.Windows.Forms.CheckBox chkB_OpcionesCeldasComp;
        private System.Windows.Forms.CheckBox chkB_OpcionesMayMin;
        private System.Windows.Forms.ComboBox cmbB_OpcionesBuscar;
        private System.Windows.Forms.Label lblB_OpcionesBuscar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnBuscarSgte;
        private System.Windows.Forms.GroupBox gbR_Opciones;
        private System.Windows.Forms.CheckBox chkR_OpcionesCeldasComp;
        private System.Windows.Forms.CheckBox chkR_OpcionesMayMin;
        private System.Windows.Forms.ComboBox cmbR_OpcionesBuscar;
        private System.Windows.Forms.Label lblR_OpcionesBuscar;
        private System.Windows.Forms.TextBox txtR_Buscar;
        private System.Windows.Forms.Label lblR_Buscar;
        private System.Windows.Forms.TextBox txtR_Reemplazar;
        private System.Windows.Forms.Label lblR_Reemplazar;
        private System.Windows.Forms.Button btnReemplazar;
        private System.Windows.Forms.Button btnReemplazarTodos;
    }
}