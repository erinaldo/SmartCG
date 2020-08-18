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

namespace ModComprobantes
{
    public partial class frmCompContListaNoProcesados : frmPlantilla, IReLocalizable
    {
        private string tipoBaseDatosCG; 

        private bool bbddDB2 = false;

        public frmCompContListaNoProcesados()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmCompContListaNoProcesados_Load(object sender, EventArgs e)
        {
            //Habilitar Edicion de Lotes
            this.gbEdicionLotes.Visible = true;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.tgGridEditarLotes.Visible = false;
            this.tgGridCompErrores.Visible = false;
            this.lblInfo.Visible = false;

            //Si no es AS inhabilitar bibilioteca
            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            if (tipoBaseDatosCG == "DB2") this.bbddDB2 = true;
            else this.bbddDB2 = false;

            if (this.bbddDB2) this.txtBiblioteca.Enabled = true;
            else this.txtBiblioteca.Enabled = false;

            //Crear el data grid para la edición de lotes
            this.BuildDataGridtgGridEditarLotes();

            //Crear el data grid para los comprobantes con errores
            this.BuildDataGridtgGridCompErrores();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonCompErrores_Click(object sender, EventArgs e)
        {
            //Habilitar Edicion de listado de errores
            this.gbEdicionLotes.Visible = false;
            this.toolStripButtonCompErrores.Enabled = false;
            this.toolStripButtonEdicionLotes.Enabled = true;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.tgGridEditarLotes.Visible = false;
            this.tgGridCompErrores.Visible = true;
            this.lblInfo.Visible = false;

            this.FillDataGridtgGridCompErrores();
            //Mover coordenados del tgGridCompErrores
            this.tgGridCompErrores.Location = new Point(40, 60);
        }

        private void toolStripButtonEdicionLotes_Click(object sender, EventArgs e)
        {
            //Habilitar Edicion de Lotes
            this.gbEdicionLotes.Visible = true;
            this.gbEdicionLotes.BringToFront();
            this.toolStripButtonCompErrores.Enabled = true;
            this.toolStripButtonEdicionLotes.Enabled = false;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.tgGridEditarLotes.Visible = false;
            this.tgGridCompErrores.Visible = false;
            this.lblInfo.Visible = false;

            if (this.bbddDB2) this.txtBiblioteca.Enabled = true;
            else this.txtBiblioteca.Enabled = false;

            //this.FilltgGridCompErrores();

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validar que existan las tablas de lotes
            string result = this.FormValid();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                return;
            }

            //Cargar los datos de la edicion de lotes
            this.FillDataGridtgGridEditarLotes();
        }

        private void toolStripButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarComprobante();
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContListaNoProcesados", "Lista de Comprobantes Contables No Procesados");    //Falta traducir
            

        }

        /// <summary>
        /// Crea la Grid para la edición de lotes
        /// </summary>
        private void BuildDataGridtgGridEditarLotes()
        {   
            try
            {
                this.tgGridEditarLotes.dsDatos = new DataSet();
                this.tgGridEditarLotes.dsDatos.DataSetName = "EdicionLotes";
                this.tgGridEditarLotes.AddUltimaFilaSiNoHayDisponile = false;
                this.tgGridEditarLotes.ReadOnly = true;
                this.tgGridEditarLotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.tgGridEditarLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.tgGridEditarLotes.AllowUserToAddRows = false;
                this.tgGridEditarLotes.AllowUserToOrderColumns = false;
                this.tgGridEditarLotes.AutoGenerateColumns = false;
                //Ocultar la columna de encabezados de fila
                this.tgGridEditarLotes.RowHeadersVisible = false;

                DataTable dt = new DataTable();
                dt.TableName = "Tabla";

                //Adicionar las columnas al DataTable
                dt.Columns.Add("CCIAWS", typeof(string));
                dt.Columns.Add("AAPP", typeof(string));
                dt.Columns.Add("DOCDWS", typeof(string));
                dt.Columns.Add("TICOWS", typeof(string));
                dt.Columns.Add("NUCOWS", typeof(string));
                dt.Columns.Add("FECHA", typeof(string));
                dt.Columns.Add("TVOUWS", typeof(string));
                dt.Columns.Add("TASCWS", typeof(string));
                dt.Columns.Add("AÑOCWS", typeof(string));
                dt.Columns.Add("LAPSWS", typeof(string));
                dt.Columns.Add("DIAEWS", typeof(string));
                dt.Columns.Add("MESEWS", typeof(string));
                dt.Columns.Add("AÑOEWS", typeof(string));

                this.tgGridEditarLotes.AddTextBoxColumn(0, "CCIAWS", this.LP.GetText("CompContdgHeaderCompania", "Compañía"), 100, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(1, "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP"), 50, 4, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(2, "DOCDWS", this.LP.GetText("CompContdgHeaderDescripcion", "Descripción"), 200, 36, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(3, "TICOWS", this.LP.GetText("CompContdgHeaderTipo", "Tipo"), 50, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(4, "NUCOWS", this.LP.GetText("CompContdgHeaderNoComp", "No Comp"), 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(5, "FECHA", this.LP.GetText("CompContdgHeaderFecha", "Fecha"), 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(6, "TVOUWS", this.LP.GetText("CompContdgHeaderClase", "Clase"), 50, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(7, "TASCWS", this.LP.GetText("CompContdgHeaderTasa", "Tasa"), 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(8, "AÑOCWS", "PeriodoAA", 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                this.tgGridEditarLotes.AddTextBoxColumn(9, "LAPSWS", "PeriodoPP", 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                this.tgGridEditarLotes.AddTextBoxColumn(10, "DIAEWS", "Dia", 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                this.tgGridEditarLotes.AddTextBoxColumn(11, "MESEWS", "Mes", 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                this.tgGridEditarLotes.AddTextBoxColumn(12, "AÑOEWS", "Ano", 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                
                /*for (int i = 0; i < this.tgGridEditarLotes.ColumnCount; i++)
                {
                    this.tgGridEditarLotes.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                }*/

                //Adicionar el DataTable al DataSet del DataGrid
                this.tgGridEditarLotes.dsDatos.Tables.Add(dt);

                //Poner como DataSource del DataGrid el DataTable creado
                this.tgGridEditarLotes.DataSource = this.tgGridEditarLotes.dsDatos.Tables["Tabla"];
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// Crea la Grid para los comprobantes con errores
        /// </summary>
        private void BuildDataGridtgGridCompErrores()
        {
            try
            {
                this.tgGridCompErrores.dsDatos = new DataSet();
                this.tgGridCompErrores.dsDatos.DataSetName = "CompErrores";
                this.tgGridCompErrores.AddUltimaFilaSiNoHayDisponile = false;
                this.tgGridCompErrores.ReadOnly = true;
                this.tgGridCompErrores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.tgGridCompErrores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.tgGridCompErrores.AllowUserToAddRows = false;
                this.tgGridCompErrores.AllowUserToOrderColumns = false;
                this.tgGridCompErrores.AutoGenerateColumns = false;
                //Ocultar la columna de encabezados de fila
                this.tgGridCompErrores.RowHeadersVisible = false;

                DataTable dt = new DataTable();
                dt.TableName = "Tabla";

                //Adicionar las columnas al DataTable
                dt.Columns.Add("PREF24", typeof(string));
                dt.Columns.Add("LIBL24", typeof(string));
                dt.Columns.Add("DESC24", typeof(string));
                dt.Columns.Add("DATE24", typeof(string));
                dt.Columns.Add("TIME24", typeof(string));
                dt.Columns.Add("ERRO24", typeof(string));


                this.tgGridCompErrores.AddTextBoxColumn(0, "PREF24", this.LP.GetText("CompContdgHeaderPrefijo", "Prefijo"), 100, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir??
                this.tgGridCompErrores.AddTextBoxColumn(1, "LIBL24", this.LP.GetText("CompContdgHeaderBiblioteca", "Biblioteca"), 100, 4, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);   //Falta traducir ??
                this.tgGridCompErrores.AddTextBoxColumn(2, "DESC24", this.LP.GetText("CompContdgHeaderDescripcion", "Descripción"), 100, 36, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridCompErrores.AddTextBoxColumn(3, "DATE24", this.LP.GetText("CompContdgHeaderFecha", "Fecha"), 100, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridCompErrores.AddTextBoxColumn(4, "TIME24", this.LP.GetText("CompContdgHeaderHora", "Hora"), 100, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);      //Falta traducir ??
                this.tgGridCompErrores.AddTextBoxColumn(5, "ERRO24", this.LP.GetText("CompContdgHeaderNumErrores", "Número de Errores"), 100, 6, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir ??
                /*this.tgGridEditarLotes.AddTextBoxColumn(6, "Clase", this.LP.GetText("CompContdgHeaderClase", "Clase"), 100, 1, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.tgGridEditarLotes.AddTextBoxColumn(7, "Tasa", this.LP.GetText("CompContdgHeaderTasa", "Tasa"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(8, "DebeML", this.LP.GetText("CompContdgHeaderDebeML", "Debe ML"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(9, "HaberML", this.LP.GetText("CompContdgHeaderHaberML", "Haber ML"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(10, "DebeME", this.LP.GetText("CompContdgHeaderDebeME", "Debe ME"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(11, "HaberME", this.LP.GetText("CompContdgHeaderHaberME", "Haber ME"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(12, "NoMovimiento", this.LP.GetText("CompContdgHeaderNoMovimiento", "No Mov"), 100, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(13, "Estado", this.LP.GetText("lblCompTransEstado", "Estado"), 100, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgComprobantes.AddTextBoxColumn(14, "FECOIC", "FECOIC", 50, 6, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                this.dgComprobantes.AddTextBoxColumn(15, "SAPRIC", "SAPRIC", 50, 5, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);
                */


                //Adicionar el DataTable al DataSet del DataGrid
                this.tgGridCompErrores.dsDatos.Tables.Add(dt);

                //Poner como DataSource del DataGrid el DataTable creado
                this.tgGridCompErrores.DataSource = this.tgGridCompErrores.dsDatos.Tables["Tabla"];
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// Valida que existan las tablas de lotes
        /// </summary>
        /// <returns></returns>
        private string FormValid()
        {
            string result = "";
            
            try
            {
                string prefijo = this.txtPrefijo.Text.Trim();
                string biblioteca = "";

                if (prefijo == "")
                {
                    result += "- El prefijo no puede estar en blanco";
                    this.txtPrefijo.Focus();
                }
                else prefijo = prefijo.ToUpper();

                if (this.bbddDB2)
                {
                    biblioteca = this.txtBiblioteca.Text.Trim();
                    if (biblioteca == "") 
                    {
                        if (result != "") result += "\n\r";
                        result += "- La biblioteca no puede estar en blanco";
                        this.txtBiblioteca.Focus();
                    }
                }

                if (result != "") return (result);

                string bibliotecaTablasLoteAS = "";
                
                if (this.bbddDB2) 
                {
                    if (biblioteca != "") bibliotecaTablasLoteAS = biblioteca + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                bool extendido = false; 
                if (this.rbFormatoAmpSi.Checked) extendido = true;

                try
                {
                    if (!extendido)
                    {
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W00")) result += "No existe la tabla cabecera de lotes";
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W01")) result += "No existe la tabla detalles de lotes";

                        //Si existe la tabla W30
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W30")) result += "";

                        //Si existe la tabla W31
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W31")) result += "";
                    }

                    if (extendido)
                    {
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W10")) result += "No existe la tabla cabecera de lotes";
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W11")) result += "No existe la tabla detalles de lotes";

                        //Si existe la tabla W40
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W40")) result += "";

                        //Si existe la tabla W41
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, bibliotecaTablasLoteAS + prefijo + "W41")) result += "";
                    }
                }
                catch (Exception ex)
                {
                    result += " - " + this.LP.GetText("errVerificarTablaLote", "No existe la tabla de lote") + " (" + ex.Message + ")" + "\n\r";
                }
            }
            catch (Exception ex)
            {
                result += " - " + this.LP.GetText("errVerificarDatosLote", "Error verificando si existen datos del lote") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }

        private void FillDataGridtgGridEditarLotes()
        {
            try
            {
                string prefijo = this.txtPrefijo.Text.Trim().ToUpper();
                string biblioteca = "";

                string bibliotecaTablasLoteAS = "";
                
                if (this.bbddDB2) 
                {
                    biblioteca = this.txtBiblioteca.Text.Trim();

                    if (biblioteca != "") bibliotecaTablasLoteAS = biblioteca + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                string tabla = "W00";
                if (this.rbFormatoAmpSi.Checked) tabla = "W10";
                string query = "select * from " + GlobalVar.PrefijoTablaCG + bibliotecaTablasLoteAS + prefijo + tabla;

                DataTable dtLotes = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                this.tgGridEditarLotes.dsDatos.Tables["Tabla"].Clear();

                DataRow row;

                string sigloanoper = "";
                string anows = "";
                string fecha = "";
                string dia = "";
                string mes = "";
                string ano = "";

                for (int i = 0; i < dtLotes.Rows.Count; i++)
                {
                    row = this.tgGridEditarLotes.dsDatos.Tables["Tabla"].NewRow();

                    row["CCIAWS"] = dtLotes.Rows[i]["CCIAWS"].ToString();
                    
                    anows = dtLotes.Rows[i]["AÑOCWS"].ToString().PadLeft(2, '0');
                    sigloanoper =  anows + "-" + dtLotes.Rows[i]["LAPSWS"].ToString().PadLeft(2, '0');

                    row["AAPP"] = sigloanoper;
                    row["DOCDWS"] = dtLotes.Rows[i]["DOCDWS"].ToString();
                    row["TICOWS"] = dtLotes.Rows[i]["TICOWS"].ToString();
                    row["NUCOWS"] = dtLotes.Rows[i]["NUCOWS"].ToString();

                    dia = dtLotes.Rows[i]["DIAEWS"].ToString();
                    if (dia == "") dia = dia.PadLeft(2, ' ');
                    else if (dia.Length == 1) dia = dia.PadLeft(2, '0');
                    mes = dtLotes.Rows[i]["MESEWS"].ToString();
                    if (mes == "") mes = mes.PadLeft(2, ' ');
                    else if (mes.Length == 1) mes = mes.PadLeft(2, '0');
                    ano = dtLotes.Rows[i]["AÑOEWS"].ToString();
                    if (ano == "") ano = ano.PadLeft(2, ' ');
                    else if (ano.Length == 1) ano = ano.PadLeft(2, '0');

                    row["FECHA"] = dia + "/" + mes + "/" + ano;

                    row["TVOUWS"] = dtLotes.Rows[i]["TVOUWS"].ToString();
                    row["TASCWS"] = dtLotes.Rows[i]["TASCWS"].ToString();

                    row["AÑOCWS"] = dtLotes.Rows[i]["AÑOCWS"].ToString();
                    row["LAPSWS"] = dtLotes.Rows[i]["LAPSWS"].ToString();
                    row["DIAEWS"] = dtLotes.Rows[i]["DIAEWS"].ToString();
                    row["MESEWS"] = dtLotes.Rows[i]["MESEWS"].ToString();
                    row["AÑOEWS"] = dtLotes.Rows[i]["AÑOEWS"].ToString();

                    this.tgGridEditarLotes.dsDatos.Tables["Tabla"].Rows.Add(row);
                 }

                if (dtLotes.Rows.Count > 0)
                {
                    this.toolStripButtonAjustar.Enabled = true;
                    this.toolStripButtonEditar.Enabled = true;
                    this.tgGridEditarLotes.Visible = true;
                    this.tgGridEditarLotes.Focus();
                }
                else
                {
                    this.tgGridEditarLotes.Visible = false;
                    this.toolStripButtonAjustar.Enabled = false;
                    this.toolStripButtonEditar.Enabled = false;
                    this.lblInfo.Text = "No existen lotes";
                    this.lblInfo.Focus();
                }
            }
            catch(Exception ex)
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(ex.Message, error);
            }
        }


        private void FillDataGridtgGridCompErrores()
        {
            try
            {
                //Falta Extendido ?????
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLC24";
                query += " where NUEV24 = 1";

                DataTable dtErrores = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                this.tgGridCompErrores.dsDatos.Tables["Tabla"].Clear();

                DataRow row;

                for (int i = 0; i < dtErrores.Rows.Count; i++)
                {
                    row = this.tgGridCompErrores.dsDatos.Tables["Tabla"].NewRow();

                    row["PREF24"] = dtErrores.Rows[i]["PREF24"].ToString();
                    row["LIBL24"] = dtErrores.Rows[i]["LIBL24"].ToString();
                    row["DESC24"] = dtErrores.Rows[i]["DESC24"].ToString();
                    row["DATE24"] = dtErrores.Rows[i]["DATE24"].ToString();
                    row["TIME24"] = dtErrores.Rows[i]["TIME24"].ToString();
                    row["ERRO24"] = dtErrores.Rows[i]["ERRO24"].ToString();

                    this.tgGridCompErrores.dsDatos.Tables["Tabla"].Rows.Add(row);
                }

                if (dtErrores.Rows.Count > 0)
                {
                    this.toolStripButtonAjustar.Enabled = true;
                    this.toolStripButtonEditar.Enabled = true;
                    this.tgGridCompErrores.Visible = true;
                    this.tgGridCompErrores.Focus();
                }
                else
                {
                    this.tgGridCompErrores.Visible = false;
                    this.toolStripButtonAjustar.Enabled = false;
                    this.toolStripButtonEditar.Enabled = false;
                    this.lblInfo.Text = "No existen comprobantes con errores";
                    this.lblInfo.Focus();
                }
            }
            catch (Exception ex)
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(ex.Message, error);
            }
        }

        private void EditarComprobante()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            if (this.tgGridEditarLotes.Visible)
            {
                //Editar Comprobante Lote
                this.EditarComprobanteLote();
            }
            else if (tgGridCompErrores.Visible)
            {
                //Editar Comprobante Error
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Crea el comprobante para llamar al formulario de Edicion de comprobantes contables
        /// </summary>
        /// <returns></returns>
        private string EditarComprobanteLote()
        {
            
            string result = "";

            try
            {
                //Seleccionar la fila activa
                if (this.tgGridEditarLotes.SelectedRows.Count == 1)
                {
                    DataGridViewRow row = this.tgGridEditarLotes.SelectedRows[0];

                    //Crear el comprobante contable
                    ComprobanteContable comprobante = new ComprobanteContable();

                    string codigo = row.Cells["CCIAWS"].Value.ToString();
                    comprobante.Cab_compania = codigo;

                    string aapp = row.Cells["AAPP"].Value.ToString();
                    int separador = aapp.IndexOf('-');
                    if (separador != -1) aapp = aapp.Remove(separador, 1);
                    comprobante.Cab_anoperiodo = aapp;

                    comprobante.Cab_descripcion = row.Cells["DOCDWS"].Value.ToString();

                    string tipo = row.Cells["TICOWS"].Value.ToString();
                    comprobante.Cab_tipo = tipo.PadLeft(2, '0');
                    comprobante.Cab_noComprobante = row.Cells["NUCOWS"].Value.ToString();

                    string fecha = "";
                    fecha += row.Cells["AÑOEWS"].Value.ToString().Trim();

                    if (fecha == "") fecha += "00";
                    else
                    {
                        fecha = fecha.PadLeft(2, '0');
                        int anoCorte = Convert.ToInt16(CGParametrosGrles.GLC01_ALSIRC);

                        int anoOrigenInt = Convert.ToInt32(fecha);
                        if (anoOrigenInt < anoCorte) anoOrigenInt = 2000 + anoOrigenInt;
                        else if (anoOrigenInt >= anoCorte) anoOrigenInt = 1900 + anoOrigenInt;

                        fecha = anoOrigenInt.ToString();
                    }
                    
                    fecha += row.Cells["MESEWS"].Value.ToString().PadLeft(2, '0');
                    fecha += row.Cells["DIAEWS"].Value.ToString().PadLeft(2, '0');

                    comprobante.Cab_fecha = fecha;
                    comprobante.Cab_clase = row.Cells["TVOUWS"].Value.ToString();
                    comprobante.Cab_tasa = row.Cells["TASCWS"].Value.ToString();

                    //Verificar si el comprobante tiene campos extendidos
                    bool extendido = false;
                    if (this.rbFormatoAmpSi.Checked)
                    {
                        comprobante.Cab_extendido = "1";
                        extendido = true;
                    }
                    else comprobante.Cab_extendido = "0";

                    string ano = row.Cells["AÑOCWS"].Value.ToString();
                    string periodo = row.Cells["LAPSWS"].Value.ToString();

                    //Obtener los detalles del comprobante a importar
                    comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(comprobante.Cab_compania, ano, periodo,
                                                                                       tipo, comprobante.Cab_noComprobante,
                                                                                       extendido);


                    //Cerrar el formulario actual ???
                    frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita();
                    frmCompCont.ImportarComprobante = true;
                    frmCompCont.ComprobanteContableImportar = comprobante;
                    frmCompCont.NombreComprobante = comprobante.Cab_descripcion;
                    frmCompCont.Show();

                }
            }
            catch
            {
            }


            return (result);
        }

        /// <summary>
        /// Busca los detalles del comrpobante a editar
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="anoperiodo"></param>
        /// <param name="tipo"></param>
        /// <param name="noComprobante"></param>
        /// <returns></returns>
        private DataTable ObtenerDetallesComprobanteImportar(string compania, string ano, string periodo, string tipo, string noComprobante, bool extendido)
        {
            DataTable dtDetalle = new DataTable();
            dtDetalle.TableName = "Detalle";

            dtDetalle.Columns.Add("Cuenta", typeof(string));
            dtDetalle.Columns.Add("Auxiliar1", typeof(string));
            dtDetalle.Columns.Add("Auxiliar2", typeof(string));
            dtDetalle.Columns.Add("Auxiliar3", typeof(string));
            dtDetalle.Columns.Add("DH", typeof(string));
            dtDetalle.Columns.Add("MonedaLocal", typeof(string));
            dtDetalle.Columns.Add("MonedaExt", typeof(string));
            dtDetalle.Columns.Add("RU", typeof(string));
            dtDetalle.Columns.Add("Descripcion", typeof(string));
            dtDetalle.Columns.Add("Documento", typeof(string));
            dtDetalle.Columns.Add("Fecha", typeof(string));
            dtDetalle.Columns.Add("Vencimiento", typeof(string));
            dtDetalle.Columns.Add("Documento2", typeof(string));
            dtDetalle.Columns.Add("Importe3", typeof(string));
            dtDetalle.Columns.Add("Iva", typeof(string));
            dtDetalle.Columns.Add("CifDni", typeof(string));

            if (extendido)
            {
                dtDetalle.Columns.Add("PrefijoDoc", typeof(string));
                dtDetalle.Columns.Add("NumFactAmp", typeof(string));
                dtDetalle.Columns.Add("NumFactRectif", typeof(string));
                dtDetalle.Columns.Add("FechaServIVA", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa1", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa2", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa3", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa4", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa5", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa6", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa7", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa8", typeof(string));
                dtDetalle.Columns.Add("CampoUserNum1", typeof(string));
                dtDetalle.Columns.Add("CampoUserNum2", typeof(string));
                dtDetalle.Columns.Add("CampoUserFecha1", typeof(string));
                dtDetalle.Columns.Add("CampoUserFecha2", typeof(string));
            }

            DataRow row;

            string prefijo = this.txtPrefijo.Text.Trim().ToUpper();

            string tabla = "W01";
            if (extendido) tabla = "W11";
                
            string query = "select * from " + GlobalVar.PrefijoTablaCG + prefijo + tabla;
            query += " where CCIAWS ='" + compania + "' and ";
            query += "AÑOCWS =" + ano + " and ";
            query += "LAPSWS =" + periodo + " and ";
            query += "TICOWS =" + tipo + " and ";
            query += "NUCOWS =" + noComprobante;

            IDataReader dr = null;

            string simidt = "";

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string CLDODT = "";
                string NDOCDT = "";
                string FDOCDT = "";
                string FEVEDT = "";
                string CDDOAD = "";
                string NDDOAD = "";
                while (dr.Read())
                {
                    //Mover la barra de progreso
                    //if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                    //else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                    //this.progressBarEspera.Refresh();

                    row = dtDetalle.NewRow();

                    row["Cuenta"] = dr["CUENWS"].ToString().Trim();
                    row["Auxiliar1"] = dr["CAUXWS"].ToString().Trim();
                    row["Auxiliar2"] = dr["AUA1WS"].ToString().Trim();
                    row["Auxiliar3"] = dr["AUA2WS"].ToString().Trim();
                    row["DH"] = dr["TMOVWS"].ToString().Trim();
                    row["MonedaLocal"] = dr["MONTWS"].ToString().Trim();
                    row["MonedaExt"] = dr["MOSMWS"].ToString().Trim();
                    row["RU"] = dr["TEINWS"].ToString().Trim();
                    row["Descripcion"] = dr["DESCWS"].ToString().Trim();
                    CLDODT = dr["CLDOWS"].ToString().Trim();
                    NDOCDT = dr["NDOCWS"].ToString().Trim();
                    NDOCDT = NDOCDT.PadLeft(7, '0');
                    //if (CLDODT != "" && CLDODT != "0" && NDOCDT != "" && NDOCDT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    if (CLDODT != "" && CLDODT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    FDOCDT = dr["FDOCWS"].ToString().Trim();
                    if (FDOCDT != "" && FDOCDT != "0") row["Fecha"] = FDOCDT;
                    FEVEDT = dr["FEVEWS"].ToString().Trim();
                    if (FEVEDT != "" && FEVEDT != "0") row["Vencimiento"] = FEVEDT;
                    CDDOAD = dr["CDDOWS"].ToString().Trim();
                    NDDOAD = dr["NDDOWS"].ToString().Trim();
                    NDDOAD = NDDOAD.PadLeft(9, '0');
                    //if (NDDOAD != "" && NDDOAD != "0") row["Documento2"] = NDDOAD;
                    if (CDDOAD != "" && CDDOAD != "0") row["Documento2"] = CDDOAD + "-" + NDDOAD;
                    row["Importe3"] = dr["TERCWS"].ToString().Trim();
                    row["Iva"] = dr["CDIVWS"].ToString().Trim();
                    row["CifDni"] = dr["NNITWS"].ToString().Trim();

                    if (extendido)
                    {
                        //Si el compobante tiene campos extendidos, leer los valores de los campos extendidos para la línea de detalle
                        //simidt = dr["SIMIDT"].ToString().Trim();
                        //this.ObtenerDetalleCamposExtendidos(ref row, compania, ano, periodo, tipo, noComprobante, simidt);
                        this.ObtenerDetalleCamposExtendidos(ref row, compania, ano, periodo, tipo, noComprobante);
                    }

                    dtDetalle.Rows.Add(row);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                string error = ex.Message;
            }

            return (dtDetalle);
        }

        /// <summary>
        /// Obtiene los campos extendidos para una línea de detalle
        /// </summary>
        /// <param name="row">fila de la línea de detalle del DataRow</param>
        /// <param name="compania">código de la compañía</param>
        /// <param name="anoperiodo">sigloanoperiodo</param>
        /// <param name="tipo">tipo de comprobante</param>
        /// <param name="noComprobante">número de comprobante</param>
        /// <param name="simidt">línea del comprobante</param>
        /// <returns></returns>
        //private void ObtenerDetalleCamposExtendidos(ref DataRow row, string compania, string anoperiodo, string tipo, string noComprobante, string simidt)
        private void ObtenerDetalleCamposExtendidos(ref DataRow row, string compania, string ano, string periodo, string tipo, string noComprobante)
        {
            string prefijo = this.txtPrefijo.Text.Trim().ToUpper();

            string tabla = "W10";
            if (this.rbFormatoAmpSi.Checked) tabla = "W11";

            string query = "select * from " + GlobalVar.PrefijoTablaCG + prefijo + tabla;
            query += " where CCIAWS ='" + compania + "' and ";
            query += "AÑOCWS =" + ano + " and ";
            query += "LAPSWS =" + periodo + " and ";
            query += "TICOWS =" + tipo + " and ";
            query += "NUCOWS =" + noComprobante;
            //query += "NUCOWS =" + noComprobante + " and ";
            //query += "SIMIDX =" + simidt;

            IDataReader dr = null;

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    row["PrefijoDoc"] = dr["PRFDWS"].ToString().Trim();
                    row["NumFactAmp"] = dr["NFAAWS"].ToString().Trim();
                    row["NumFactRectif"] = dr["NFARWS"].ToString().Trim();
                    row["FechaServIVA"] = dr["FIVAWS"].ToString().Trim();
                    row["CampoUserAlfa1"] = dr["USA1WS"].ToString().Trim();
                    row["CampoUserAlfa2"] = dr["USA2WS"].ToString().Trim();
                    row["CampoUserAlfa3"] = dr["USA3WS"].ToString().Trim();
                    row["CampoUserAlfa4"] = dr["USA4WS"].ToString().Trim();
                    row["CampoUserAlfa5"] = dr["USA5WS"].ToString().Trim();
                    row["CampoUserAlfa6"] = dr["USA6WS"].ToString().Trim();
                    row["CampoUserAlfa7"] = dr["USA7WS"].ToString().Trim();
                    row["CampoUserAlfa8"] = dr["USA8WS"].ToString().Trim();
                    row["CampoUserNum1"] = dr["USN1WS"].ToString().Trim();
                    row["CampoUserNum2"] = dr["USN2WS"].ToString().Trim();
                    row["CampoUserFecha1"] = dr["USF1WS"].ToString().Trim();
                    row["CampoUserFecha2"] = dr["USF2WS"].ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                string error = ex.Message;
            }
        }
        #endregion
    }
}
