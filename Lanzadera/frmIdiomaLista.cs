using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;
using System.Collections;

namespace FinanzasNet
{
    public partial class frmIdiomaLista : frmPlantilla, IReLocalizable
    {
        private DataTable tablaIdiomas;

        public frmIdiomaLista()
        {
            InitializeComponent();
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Traducir los nombres de los idiomas que aparecen en la Grid
            this.TraducirIdiomasGrid();
        }

        #region Eventos
        private void frmIdiomaLista_Load(object sender, EventArgs e)
        { 
            Log.Info("INICIO Lista de Idiomas");
        
            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //LLenar el Grid de idiomas
            this.FillDataGridIdiomas();

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridIdiomas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Ocultar la columna Id del idioma
            this.dataGridIdiomas.Columns[0].Visible = false;
        }

        private void dataGridIdiomas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex == this.dataGridIdiomas.Columns["Defecto"].Index)
                {
                    //Parar de editar la celda
                    this.dataGridIdiomas.EndEdit();

                    //Permitir sólo una fila con la columna Defecto marcada
                    if ((bool)this.dataGridIdiomas.Rows[e.RowIndex].Cells["Defecto"].Value)
                    {
                        for (int i = 0; i < this.dataGridIdiomas.Rows.Count; i++)
                        {
                            if (i != e.RowIndex)
                            {
                                if ((bool)this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value)
                                {
                                    this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value = false;
                                }
                            }
                        }

                        //Marcar el idioma por defecto como activo
                        this.dataGridIdiomas.Rows[e.RowIndex].Cells["Activo"].Value = true;
                    }
                }
                else
                    if (e.ColumnIndex == this.dataGridIdiomas.Columns["Activo"].Index)
                    {
                        this.dataGridIdiomas.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void dataGridIdiomas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Columna Activo seleccionada y no es la fila Header
                if (e.RowIndex != -1 && e.ColumnIndex == 2)
                {
                    //Si columna Activo no está marcada, la columna Defecto tampoco lo podrá estar
                    if (!((bool)this.dataGridIdiomas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        this.dataGridIdiomas.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = false;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void btnIdiomaAceptar_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Texto de los controles en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmIdiomaListaTitulo", "Lista de Idiomas");

            this.dataGridIdiomas.Columns["Descripcion"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderIdioma", "Idioma");
            this.dataGridIdiomas.Columns["Activo"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderActivo", "Activo");
            this.dataGridIdiomas.Columns["Defecto"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderDefecto", "Defecto");
            this.lblNoExistenIdiomas.Text = this.LP.GetText("lblIdiomaNoDefinidos", "No se han definido idiomas. Se trabajará con el idioma por defecto (español)");
        }

        /// <summary>
        /// Traducir los nombres de los idiomas que aparecen en la Grid
        /// </summary>
        private void TraducirIdiomasGrid()
        {
            try
            {
                int fila = 0;
                string descripcion;

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    descripcion = this.LP.GetText("lblIdioma" + idioma.Cultura, idioma.Descripcion); ;
                    this.dataGridIdiomas.Rows[fila].Cells["Descripcion"].Value = descripcion;
                    
                    fila++;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Llena el DataGrid de idiomas a partir de la sección de idiomas del app.config
        /// </summary>
        private void FillDataGridIdiomas()
        {
            try
            {
                string idiomaDefecto = ConfigurationManager.AppSettings["idioma"];

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

                this.tablaIdiomas = new DataTable("Idiomas");
                this.tablaIdiomas.Columns.Add("Id", typeof(Int16));
                this.tablaIdiomas.Columns.Add("Descripcion", typeof(String));
                this.tablaIdiomas.Columns.Add("Activo", typeof(bool));
                this.tablaIdiomas.Columns.Add("Defecto", typeof(bool));
                string cultura = "";
                DataRow row;

                CheckBox colCheckboxActivo = new CheckBox();
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    row = this.tablaIdiomas.NewRow();
                    cultura = idioma.Cultura;
                    row["Id"] = idioma.Id;
                    row["Descripcion"] = this.LP.GetText("lblIdioma" + cultura, idioma.Descripcion);
                    if (idioma.Activo == 1) row["Activo"] = true;
                    else row["Activo"] = false;

                    if (idiomaDefecto != null && idiomaDefecto != "")
                    {
                        if (cultura == idiomaDefecto)
                        {
                            row["Defecto"] = true;
                        }
                        else row["Defecto"] = false;
                    }
                    else row["Defecto"] = false;

                    this.tablaIdiomas.Rows.Add(row);
                }

                this.dataGridIdiomas.DataSource = this.tablaIdiomas;


                if (idiomaSection.Idiomas.Count > 0)
                {
                    // Set a cell padding to provide space for the top of the focus  
                    // rectangle and for the content that spans multiple columns
                    //int CUSTOM_CONTENT_HEIGHT = 15;
                    //Padding newPadding = new Padding(0, 1, 0, CUSTOM_CONTENT_HEIGHT);
                    //this.dataGridIdiomas.RowTemplate.DefaultCellStyle.Padding = newPadding;

                    //Ajustar columnas al tamaño de la ventana
                    //this.dataGridIdiomas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    //this.dataGridIdiomas.Dock = DockStyle.Fill;

                    //this.dataGridIdiomas.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    //this.dataGridIdiomas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Adjust the row heights to accommodate the normal cell content. 
                    //this.dataGridIdiomas.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

                    //Estilo para la fila header
                    //this.dataGridIdiomas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    //DataGridViewCellStyle style = this.dataGridIdiomas.ColumnHeadersDefaultCellStyle;
                    //style.BackColor = Color.Navy;
                    //style.ForeColor = Color.White;
                    //style.Font = new Font(this.dataGridIdiomas.Font, FontStyle.Bold);

                    //this.dataGridIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    this.dataGridIdiomas.Columns["Descripcion"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //De la fila Header, centrar el texto de las columnas de los checkbox
                    this.dataGridIdiomas.Columns["Activo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridIdiomas.Columns["Defecto"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //La columna descripción es de solo lectura
                    this.dataGridIdiomas.Columns["Descripcion"].ReadOnly = true;

                    this.dataGridIdiomas.Visible = true;
                    this.lblNoExistenIdiomas.Visible = false;
                }
                else
                {
                    this.dataGridIdiomas.Visible = false;
                    this.lblNoExistenIdiomas.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Validar si el formualario es correcto para grabar los valores
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormulario()
        {
            bool result = true;

            bool existeActivo = false;
            bool existeDefecto = false;

            int cantDefecto = 0;
            int indiceDefecto = -1;

            string msgError = this.LP.GetText("errValTitulo", "Error");
            try
            {
                //Verificar que exista al menos un idioma activo y uno por defecto
                for (int i = 0; i < this.dataGridIdiomas.Rows.Count; i++)
                {
                    //if (existeActivo && existeDefecto) break;

                    //if (((bool)(this.tablaIdiomas.Rows[i]["Activo"]))) existeActivo = true;
                    if (Convert.ToBoolean(this.dataGridIdiomas.Rows[i].Cells["Activo"].Value) == true) existeActivo = true;
                    //if ((bool)this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value)
                    //if (((bool)(this.tablaIdiomas.Rows[i]["Defecto"])))
                    if (Convert.ToBoolean(this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value) == true)
                    {
                        existeDefecto = true;
                        indiceDefecto = i;
                        cantDefecto++;
                    } 
                }

                if (cantDefecto > 1)
                {
                    MessageBox.Show(this.LP.GetText("errValIdiomasDefectoMas", "Sólo se permite un idioma por defecto"), msgError); //Falta traducir
                    return (false);
                }

                if (!existeActivo)
                {
                    MessageBox.Show(this.LP.GetText("errValIdiomasActivos", "Debe indicar al menos un idioma activo"), msgError);
                    result = false;
                }
                else
                {
                    if (!existeDefecto)
                    {
                        MessageBox.Show(this.LP.GetText("errValIdiomaDefecto", "Debe indicar el idioma por defecto"), msgError);
                        result = false;
                    }
                    else
                    {
                        if (indiceDefecto != -1 && (!(bool)this.dataGridIdiomas.Rows[indiceDefecto].Cells["Activo"].Value))
                        {
                            MessageBox.Show(this.LP.GetText("errValIdiomaDefectoActivo", "El idioma por defecto debe estar activo"), msgError);
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msg = this.LP.GetText("errValIdiomaDefecto", "Se ha producido un error validando el formulario") + " (" + ex.Message + ")";
                MessageBox.Show(msg, msgError);
                result = false;
            }

            if (!result) this.dataGridIdiomas.Focus();

            return (result);
        }
        #endregion

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonGrabar_Click(object sender, EventArgs e)
        {
            //Validar el formulario
            if (this.ValidarFormulario())
            {
                try
                {
                    string culturaIdiomaDefectoActual = ConfigurationManager.AppSettings["idioma"];
                    string culturaIdiomaDefecto = "";

                    IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

                    Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    IdiomaSection section = (IdiomaSection)cfg.Sections["idiomaSection"];

                    if (section != null)
                    {

                        // You need to remove the old settings object before you can replace it
                        //cfg.Sections.Remove("idiomaSection");

                        int fila = 0;

                        foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                        {
                            section.Idiomas[fila].Id = idioma.Id;
                            section.Idiomas[fila].Descripcion = idioma.Descripcion;
                            section.Idiomas[fila].Cultura = idioma.Cultura;
                            //section.Idiomas[fila].Imagen = idioma.Imagen;

                            if ((bool)this.dataGridIdiomas.Rows[fila].Cells["Activo"].Value)
                            {
                                section.Idiomas[fila].Activo = 1;
                            }
                            else section.Idiomas[fila].Activo = 0;

                            if ((bool)this.dataGridIdiomas.Rows[fila].Cells["Defecto"].Value) culturaIdiomaDefecto = idioma.Cultura;

                            fila++;
                            cfg.Save();
                        }

                    }

                    //Actualizar los valores de la sección de idiomas del app.config
                    utiles.ModificarappSettings("idioma", culturaIdiomaDefecto);

                    if (culturaIdiomaDefectoActual != culturaIdiomaDefecto)
                    {
                        //cambiar el idioma de los formularios
                        //Actualizar la variable global de idioma
                        GlobalVar.LanguageProvider = culturaIdiomaDefecto;

                        try
                        {
                            //Recargar todos los formularios abiertos
                            System.Globalization.CultureInfo nuevaCultura = new System.Globalization.CultureInfo(culturaIdiomaDefecto);
                            System.Threading.Thread.CurrentThread.CurrentUICulture = nuevaCultura;
                            foreach (Form f in Application.OpenForms)
                                if (f is IReLocalizable)
                                    ((IReLocalizable)f).ReLocalize();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    string msgError = this.LP.GetText("errValTitulo", "Error");
                    string msg = this.LP.GetText("errGenActualizandoValores", "Se ha producido un error actualizando los valores") + " (" + ex.Message + ")";
                    MessageBox.Show(msg, msgError);
                }
            }
        }

        private void frmIdiomaLista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                toolStripButtonSalir_Click(sender, null);
            }
        }

        private void frmIdiomaLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de Idiomas");
        }
    }
}
