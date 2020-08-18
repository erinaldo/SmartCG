using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLT03 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOMONEXT";

        private bool nuevo;
        private string codigo;

        private DataTable dtTasaCambioGrid;

        //TextBox que se utiliza en el DataGridView de Detalles para validar que las monedas y el importe sean sólo numéricos
        TextBox tb;

        private bool grabarTasasCambio = false;
        private bool gridChange = false;

        private bool cargarInfoTasasCambio = true;
        private bool nuevoRegistro = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public frmMtoGLT03()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchImportesDecimales.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchImportesDecimales.ThemeName = "MaterialBlueGrey";

            this.gbTasaCambio.ElementTree.EnableApplicationThemeName = false;
            this.gbTasaCambio.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmMtoGLT03_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Monedas Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);
                
                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();

                this.txtCambioInicial.Tag = "";

                this.gbTasaCambio.Enabled = false;

            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                this.gbTasaCambio.Enabled = true;

                //Construir el DataGrid
                this.BuildDataGridTasaCambio();

                //Recuperar la información de la compañía y mostrarla en los controles
                this.CargarInfoMoneda();

                //Llenar el DataGrid
                this.CargarInfoTasaCambio();

                this.radGridViewTasaCambio.ClearSelection();

                this.ActiveControl = this.txtCambioInicial;
                this.txtCambioInicial.Select(0, 0);
                this.txtCambioInicial.Focus();

            }
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
        }

        private void txtCodInt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtCambioInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            //tb = sender as TextBox;
            //utiles.ValidarNumeroConDecimalesKeyPress(7, ref this.tb, false, ref sender, ref e);
            utiles.ValidarNumeroConDecimalesKeyPress(7, ref this.txtCambioInicial, false, ref sender, ref e);
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            //string codMoneda = this.txtCodigo.Text.Trim();
            string codMoneda = this.txtCodigo.Text;

            if (codMoneda == "")
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCodigo.Text = "";
                this.txtCodigo.Focus();

                RadMessageBox.Show("Código de moneda obligatorio", this.LP.GetText("errValCodMoneda", "Error"));  //Falta traducir
                bTabulador = false;
                return;
            }

            bool codMonedaOk = true;
            if (this.nuevo) codMonedaOk = this.CodigoMonedaValido();    //Verificar que el codigo no exista

            if (codMonedaOk)
            {
                this.HabilitarDeshabilitarControles(true);
                this.txtCodigo.IsReadOnly = true;

                utiles.ButtonEnabled(ref this.radButtonSave, true);

                this.gbTasaCambio.Enabled = true;

                //Construir el DataGrid
                this.BuildDataGridTasaCambio();
            }
            else
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCodigo.Focus();
                RadMessageBox.Show("Código de moneda ya existe", this.LP.GetText("errValCodMonedaExiste", "Error"));  //Falta traducir
                bTabulador = false;
            }
            bTabulador = false;
        }


        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb = sender as TextBox;
            utiles.ValidarNumeroConDecimalesKeyPress(7, ref this.tb, false, ref sender, ref e);
        }

        private void frmMtoGLT03_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                string tasaCambioInicial = this.txtCambioInicial.Tag.ToString().Trim();
                if (this.nuevo) tasaCambioInicial = "00000000,0000000";

                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.txtCambioInicial.Text.Trim() != tasaCambioInicial ||
                    this.radToggleSwitchImportesDecimales.Value != (bool)(this.radToggleSwitchImportesDecimales.Tag) ||
                    this.txtCodInt.Text != this.txtCodInt.Tag.ToString() ||
                    this.txtDesc.Text != this.txtDesc.Tag.ToString() ||
                    this.gridChange
                )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonSave.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Monedas Alta/Edita");
        }

        private void frmMtoGLT03_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) radButtonExit_Click(sender, null);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLT03TituloALta", "Mantenimiento de Monedas extranjeras - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT03TituloEdit", "Mantenimiento de Monedas extranjeras - Edición");           //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblCodigo.Text = this.LP.GetText("lblGLT03CodMoneda", "Código de la moneda");
            this.lblCambioInicial.Text = this.LP.GetText("lblGLT03CambIni", "Cambio inicial (7 decimales)");
            this.lblImporteDec.Text = this.LP.GetText("lblImpDec", "Importes con decimales");
            this.lblCodInt.Text = this.LP.GetText("lblGLT03CodInt", "Código internacional");
            this.lblDesc.Text = this.LP.GetText("lblGLT03Desc", "Descripción");
            this.gbTasaCambio.Text = this.LP.GetText("lblGLT03TasaCambio", "Tasa de cambio");
        }
        
        /// <summary>
        /// Carga la información de la moneda solicitada
        /// </summary>
        private void CargarInfoMoneda()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;
      
                string importesDecimales = "";

                bool existeMoneda = false;

                decimal tasaCambioInicialDec = 0;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT03 ";
                query += "where TIMOME = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    existeMoneda = true;
                    this.txtCambioInicial.Text = dr.GetValue(dr.GetOrdinal("CAMBME")).ToString().Trim();
                    this.txtCambioInicial.Tag = this.txtCambioInicial.Text;

                    try
                    {
                        tasaCambioInicialDec = Convert.ToDecimal(this.txtCambioInicial.Text);
                        this.txtCambioInicial.Text = tasaCambioInicialDec.ToString("N7");
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    importesDecimales = dr.GetValue(dr.GetOrdinal("MASCME")).ToString().Trim();
                    if (importesDecimales == "N") this.radToggleSwitchImportesDecimales.Value = false;
                    else this.radToggleSwitchImportesDecimales.Value = true;
                    
                    this.txtCodInt.Text = dr.GetValue(dr.GetOrdinal("CINTME")).ToString().Trim();
                    this.txtDesc.Text = dr.GetValue(dr.GetOrdinal("DESCME")).ToString().Trim();
                }

                dr.Close();

                if (existeMoneda)
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT13 ";
                    query += "where TIMOMF = '" + this.codigo + "' ";
                    query += "order by FCAMMF desc";

                    this.dtTasaCambioGrid = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (this.dtTasaCambioGrid != null && this.dtTasaCambioGrid.Rows.Count > 0)
                    {
                        //string tasaCambio = this.dtTasaCambioGrid.Rows[0]["CAMBMF"].ToString();
                        string tasaCambio = this.dtTasaCambioGrid.Rows[this.dtTasaCambioGrid.Rows.Count - 1]["CAMBMF"].ToString();
                        this.txtCambioInicial.Text = tasaCambio;
                    }

                }

                //Actualizar los valores originales de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.txtCambioInicial.Enabled = valor;
            this.radToggleSwitchImportesDecimales.Enabled = valor;
            this.txtCodInt.Enabled = valor;
            this.txtDesc.Enabled = valor;
        }

        /// <summary>
        /// Valida que no exista el código de la moneda
        /// </summary>
        /// <returns></returns>
        private bool CodigoMonedaValido()
        {
            bool result = false;

            try
            {
                string codMoneda = this.txtCodigo.Text.Trim();

                if (codMoneda != "")
                {
                    string query = "select count(TIMOME) from " + GlobalVar.PrefijoTablaCG + "GLT03 ";
                    query += "where TIMOME = '" + codMoneda + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Crea el DataGrid que contiene las tasas de cambio de la moneda
        /// </summary>
        private void BuildDataGridTasaCambio()
        {
            //Create the data source and fill some data
            this.dtTasaCambioGrid = new DataTable();
            this.dtTasaCambioGrid.Columns.Add("Fecha", typeof(string));
            this.dtTasaCambioGrid.Columns.Add("TasaCambio", typeof(decimal));
            this.dtTasaCambioGrid.Columns.Add("EspecialEuro", typeof(string));
            this.dtTasaCambioGrid.Columns.Add("Eliminar", typeof(Image));
            this.dtTasaCambioGrid.Columns.Add("TasaCambioOrigen", typeof(decimal));
            this.dtTasaCambioGrid.Columns.Add("FechaOrigen", typeof(string));
            this.dtTasaCambioGrid.Columns.Add("EspecialEuroOrigen", typeof(string));
            //allow the grid to genetate its columns
            this.radGridViewTasaCambio.MasterTemplate.AutoGenerateColumns = false;
            //set the grid data source
            //this.radGridViewTasaCambio.DataSource = this.dtTasaCambioGrid;

            GridViewDateTimeColumn colFecha = new GridViewDateTimeColumn();
            colFecha.Name = "Fecha";
            colFecha.HeaderText = "Fecha";

            colFecha.FormatString = "{0:dd/MM/yyyy}"; // specify cell's format
            colFecha.Format = DateTimePickerFormat.Custom;//specify the editor's format
            colFecha.CustomFormat = "dd/MM/yyyy";
 
            this.radGridViewTasaCambio.Columns.Add(colFecha);

            GridViewDecimalColumn colTasaCambio = new GridViewDecimalColumn();
            colTasaCambio.Name = "TasaCambio";
            //colTasaCambio.DataType = Type.GetType("System.Decimal");
            colTasaCambio.HeaderText = "Tasa cambio";
            colTasaCambio.DecimalPlaces = 7;
            //colTasaCambio.ThousandsSeparator = true;
            this.radGridViewTasaCambio.Columns.Add(colTasaCambio);

            //Valores del ComboBox para el campo Especial Euro
            DataTable tableSource = new DataTable("tableSource");
            tableSource.Columns.AddRange(new DataColumn[] {
                new DataColumn("id"),
                new DataColumn("desc") });
            tableSource.Rows.Add(" ", " ");
            tableSource.Rows.Add("E", "E");
            tableSource.Rows.Add("0", "0");
            tableSource.Rows.Add("1", "1");
            tableSource.Rows.Add("2", "2");
            tableSource.Rows.Add("3", "3");
            tableSource.Rows.Add("4", "4");
            tableSource.Rows.Add("5", "5");
            tableSource.Rows.Add("6", "6");
            tableSource.Rows.Add("7", "7");
            tableSource.Rows.Add("8", "8");

            GridViewComboBoxColumn columnEspecialEuro = new GridViewComboBoxColumn();
            columnEspecialEuro.Name = "EspecialEuro";
            columnEspecialEuro.HeaderText = "Especial Euro";
            //columnEspecialEuro.DataPropertyName = "EspecialEuro";
            columnEspecialEuro.FieldName = "EspecialEuro";
            columnEspecialEuro.Width = 90;
            //columnEspecialEuro.DropDownWidth = 160;
            //columnEspecialEuro.MaxDropDownItems = 3;
            //columnEspecialEuro.FlatStyle = FlatStyle.Flat;

            columnEspecialEuro.DataSource = tableSource;
            columnEspecialEuro.DisplayMember = "desc";
            columnEspecialEuro.ValueMember = "id";
            //columnEspecialEuro.ValueType = typeof(string);
            this.radGridViewTasaCambio.Columns.Add(columnEspecialEuro);


            this.radGridViewTasaCambio.Columns.Add(new GridViewImageColumn("Eliminar"));

            GridViewTextBoxColumn colTasaCambioOrigen = new GridViewTextBoxColumn();
            colTasaCambioOrigen.Name = "TasaCambioOrigen";
            colTasaCambioOrigen.HeaderText = "TasaCambioOrigen";
            colTasaCambioOrigen.DataType = Type.GetType("System.Decimal");
            colTasaCambioOrigen.IsVisible = false;
            radGridViewTasaCambio.Columns.Add(colTasaCambioOrigen);

            GridViewTextBoxColumn colFechaOrigen = new GridViewTextBoxColumn();
            colFechaOrigen.Name = "FechaOrigen";
            colFechaOrigen.HeaderText = "FechaOrigen";
            colFechaOrigen.IsVisible = false;
            this.radGridViewTasaCambio.Columns.Add(colFechaOrigen);
            
            GridViewTextBoxColumn colEspecialEuroOrigen = new GridViewTextBoxColumn();
            colEspecialEuroOrigen.Name = "EspecialEuroOrigen";
            colEspecialEuroOrigen.HeaderText = "EspecialEuroOrigen";
            colEspecialEuroOrigen.IsVisible = false;
            this.radGridViewTasaCambio.Columns.Add(colEspecialEuroOrigen);
        }

        /// <summary>
        /// Carga la información de las tasas de cambio de la moneda
        /// </summary>
        private void CargarInfoTasaCambio()
        {
            try
            {
                if (this.dtTasaCambioGrid != null && this.dtTasaCambioGrid.Rows.Count > 1)
                {
                    GridViewDataRowInfo rowInfo;
                    string fecha = "";

                    DateTime fechaFormato;
                    decimal tasaCambioActual = 0;

                    //for (int i = 1; i < this.dtTasaCambioGrid.Rows.Count; i++)
                    for (int i = 0; i < this.dtTasaCambioGrid.Rows.Count - 1; i++)
                        {
                        rowInfo = new GridViewDataRowInfo(this.radGridViewTasaCambio.MasterView);

                        fecha = this.dtTasaCambioGrid.Rows[i]["FCAMMF"].ToString();
                        fechaFormato = utiles.FechaToFormatoCG(fecha);

                        //rowInfo.Cells["Fecha"].Value = this.FechaConFormato(fecha);
                        rowInfo.Cells["Fecha"].Value = fechaFormato;

                        tasaCambioActual = Convert.ToDecimal(this.dtTasaCambioGrid.Rows[i]["CAMBMF"]);
                        rowInfo.Cells["TasaCambio"].Value = tasaCambioActual.ToString("N7");
                        //rowInfo.Cells["TasaCambio"].Value = Convert.ToDecimal(this.dtTasaCambio.Rows[i]["CAMBMF"]);
                        rowInfo.Cells["TasaCambioOrigen"].Value = this.dtTasaCambioGrid.Rows[i]["CAMBMF"];
                        rowInfo.Cells["FechaOrigen"].Value = fecha;
                        rowInfo.Cells["EspecialEuro"].Value = this.dtTasaCambioGrid.Rows[i]["EUROMF"].ToString();
                        rowInfo.Cells["EspecialEuroOrigen"].Value = rowInfo.Cells["EspecialEuro"].Value;
                        rowInfo.Cells["Eliminar"].Value = global::ModMantenimientos.Properties.Resources.Eliminar;

                        this.radGridViewTasaCambio.Rows.Add(rowInfo);
                    }
                }

                this.radGridViewTasaCambio.Refresh();

                if (this.radGridViewTasaCambio.Rows != null && this.radGridViewTasaCambio.Rows.Count > 0)
                {
                    for (int i = 0; i < this.radGridViewTasaCambio.Columns.Count; i++)
                    {
                        this.radGridViewTasaCambio.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    }

                    this.radGridViewTasaCambio.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewTasaCambio.MasterTemplate.BestFitColumns();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.cargarInfoTasasCambio = false;
        }

        /// <summary>
        /// Valida la tasa de cambio
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valorFormat"></param>
        /// <returns></returns>
        private string ValidarTasaCambio(string valor, ref string valorFormat)
        {
            string result = "";

            try
            {
                decimal tasaCambio = Convert.ToDecimal(valor);

                if (tasaCambio == 0) result = "Tasa de cambio inválida";    //Falta traducir
                else
                {
                    int parteEntera = Convert.ToInt32(tasaCambio);

                    decimal parteDecimal = tasaCambio - Convert.ToDecimal(parteEntera);

                    string parteEnteraStr = parteEntera.ToString();
                    if (parteEnteraStr.Length > 8) result = "La tasa de cambio no tiene formato correcto. La parte entera no puede tener más de 8 dígitos";     //Falta traducir
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private string ValidarFecha(string valor)
        {
            string result = "";

            valor = valor.Trim();

            if (valor != "" && valor != "  /  /")
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(valor);
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    result = this.LP.GetText("lblErrFechaFormato", "La fecha desde no tiene un formato válido");   //Falta traducir
                }
            }

            return (result);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            try
            {
                this.grabarTasasCambio = false;

                //Validar cambio inicial si procede (parte entera no puede tener más de 8 dígitos y la parte decimal no puede tener más de 7 dígitos)
                decimal cambioInicialDec = 0;
                decimal cambioInicialTagDec = 0;
                try
                {
                    cambioInicialDec = (this.txtCambioInicial.Text.Trim() == "") ? 0 : Convert.ToDecimal(this.txtCambioInicial.Text);

                    if (cambioInicialDec == 0)
                    {
                        errores += "- Tasa de cambio inválida \n\r";        //Falta traducir
                        this.txtCambioInicial.Focus();
                    }
                    else
                    {
                        try
                        {
                            cambioInicialTagDec = (this.txtCambioInicial.Tag.ToString() == "") ? 0 : Convert.ToDecimal(this.txtCambioInicial.Tag);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        if (cambioInicialDec != cambioInicialTagDec)
                        {
                            string tasaCambioFormat = "";
                            string resultValTasaCambio = ValidarTasaCambio(this.txtCambioInicial.Text, ref tasaCambioFormat);
                            if (resultValTasaCambio != "")
                            {
                                errores += "- " + resultValTasaCambio + "\n\r";
                            }
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Validar filas del datagrid
                if (!this.nuevo)
                {
                    string resultFila = "";
                    for (int i = 0; i < this.radGridViewTasaCambio.Rows.Count; i++)
                    {
                        resultFila = this.ValidarTasaCambioFila(i, this.radGridViewTasaCambio.Rows[i]);
                        if (resultFila != "")
                        {
                            errores += "- " + resultFila + "\n\r";
                        }
                    }

                    //Verificar que las fechas de las tasas de cambio no se repitan
                    string fechaActual;
                    string fechaAux;
                    bool fechaRepe = false;
                    for (int i = 0; i < this.radGridViewTasaCambio.Rows.Count; i++)
                    {
                        fechaActual = this.radGridViewTasaCambio.Rows[i].Cells["Fecha"].Value.ToString();

                        for (int j = 0; j < this.radGridViewTasaCambio.Rows.Count; j++)
                        {
                            fechaAux = this.radGridViewTasaCambio.Rows[j].Cells["Fecha"].Value.ToString();

                            if (i != j && fechaActual == fechaAux)
                            {
                                errores += "- " + "Fila " + (j + 1) + ": " + "La fecha de cambio ya existe \n\r";       //Falta traducir
                                this.radGridViewTasaCambio.Rows[j].IsSelected = true;

                                this.radGridViewTasaCambio.CurrentRow = this.radGridViewTasaCambio.Rows[j];
                                this.radGridViewTasaCambio.CurrentColumn = this.radGridViewTasaCambio.Columns["Fecha"];
                                fechaRepe = true;
                                break;
                            }
                        }

                        if (fechaRepe) break;
                    }

                    if (resultFila != "" && this.grabarTasasCambio) this.grabarTasasCambio = false;
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Valida los datos de una fila de la Grid de Tasas de cambio
        /// </summary>
        /// <returns></returns>
        private string ValidarTasaCambioFila(int fila, GridViewRowInfo row)
        {
            string result = "";
            bool validarFila = false;

            //inicializar por si hay null
            if (row.Cells["TasaCambio"].Value == null) row.Cells["TasaCambio"].Value = 0;
            if (row.Cells["TasaCambioOrigen"].Value == null) row.Cells["TasaCambioOrigen"].Value = 0;
            if (row.Cells["Fecha"].Value == null) row.Cells["fecha"].Value = "";
            if (row.Cells["FechaOrigen"].Value == null) row.Cells["FechaOrigen"].Value = "";
            if (row.Cells["EspecialEuro"].Value == null) row.Cells["EspecialEuro"].Value = "";
            if (row.Cells["EspecialEuroOrigen"].Value == null) row.Cells["EspecialEuroOrigen"].Value = "";

            string tasaCambioOrigen = row.Cells["TasaCambioOrigen"].Value.ToString();
            string fechaOrigen = row.Cells["FechaOrigen"].Value.ToString();
            string especialEuroOrigen = row.Cells["EspecialEuroOrigen"].Value.ToString();

            string fechaOrigenFormato = fechaOrigen == "" ? fechaOrigenFormato = "" : this.FechaConFormato(fechaOrigen);

            string tasaCambio = row.Cells["TasaCambio"].Value.ToString();
            string fecha = row.Cells["Fecha"].Value.ToString();
            string especialEuro = row.Cells["EspecialEuro"].Value.ToString();

            if (tasaCambioOrigen == "" && fechaOrigen == "" && especialEuroOrigen == "" &&
                (tasaCambio != "" || fecha != "" || especialEuro != ""))
                validarFila = true;    //Fila nueva y con valores
            else
            {
                if (tasaCambioOrigen != tasaCambio || fechaOrigenFormato != fecha || especialEuroOrigen != especialEuro)
                    validarFila = true;     //Fila modificada
            }

            if (validarFila)
            {
                if (tasaCambio != "")
                {
                    string tasaCambioReferencia = "";
                    string validaTasaCambio = this.ValidarTasaCambio(tasaCambio, ref tasaCambioReferencia);
                    if (validaTasaCambio != "")
                    {
                        result += "La tasa de cambio no tiene un formato correcto";     //Falta traducir
                        this.radGridViewTasaCambio.Rows[fila].IsSelected = true;

                        this.radGridViewTasaCambio.CurrentRow = this.radGridViewTasaCambio.Rows[fila];
                        this.radGridViewTasaCambio.CurrentColumn = this.radGridViewTasaCambio.Columns["TasaCambio"];
                    }
                }
                else
                {
                    result += "La tasa de cambio no está indicada";     //Falta traducir
                    this.radGridViewTasaCambio.Rows[fila].IsSelected = true;
                    this.radGridViewTasaCambio.CurrentRow = this.radGridViewTasaCambio.Rows[fila];
                    this.radGridViewTasaCambio.CurrentColumn = this.radGridViewTasaCambio.Columns["TasaCambio"];
                }

                string validaFecha = this.ValidarFecha(fecha);
                if (validaFecha != "")
                {
                    result += "La fecha no tiene un formato correcto";     //Falta traducir
                    this.radGridViewTasaCambio.Rows[fila].IsSelected = true;
                    this.radGridViewTasaCambio.CurrentRow = this.radGridViewTasaCambio.Rows[fila];
                    this.radGridViewTasaCambio.CurrentColumn = this.radGridViewTasaCambio.Columns["Fecha"];
                }

                if (result != "") result = "Fila " + (fila + 1) + ": " + result + "\n\r";
                else this.grabarTasasCambio = true;
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a una moneda
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string importesDecimales = this.radToggleSwitchImportesDecimales.Value ? "S" : "N";
                string cambioInicialActual = this.txtCambioInicial.Text;

                string codigoInt = this.txtCodInt.Text.Trim();
                codigoInt = (codigoInt == "") ? " " : codigoInt;

                string desc = this.txtDesc.Text.Trim();
                desc = (desc == "") ? " " : desc;

                try
                {
                    decimal cambioActualDec = Convert.ToDecimal(cambioInicialActual);
                    cambioInicialActual = cambioActualDec.ToString("N7");
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                cambioInicialActual = cambioInicialActual.Replace(',', '.');

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT03";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIMOME, CAMBME, MASCME, CINTME, DESCME) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.txtCodigo.Text + "', " + cambioInicialActual + ", '" + importesDecimales + "', '" + codigoInt;
                query += "','" + desc + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLT03", this.txtCodigo.Text, null);

                //Dar de alta el cambio inicial en la tabla GLT13
                nombreTabla = GlobalVar.PrefijoTablaCG + "GLT13";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIMOMF, FCAMMF, CAMBMF, EUROMF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.txtCodigo.Text + "', 0, " + cambioInicialActual + ",' ')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una moneda
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo_Ant()
        {
            string result = "";
            try
            {
                string importesDecimales = this.radToggleSwitchImportesDecimales.Value ? "S" : "N";
                string cambioInicialActual = this.txtCambioInicial.Text;

                string codigoInt = this.txtCodInt.Text.Trim();
                codigoInt = (codigoInt == "") ? " " : codigoInt;

                string desc = this.txtDesc.Text.Trim();
                desc = (desc == "") ? " " : desc;
                decimal cambioActualDec = 0;

                try
                {
                    cambioActualDec = Convert.ToDecimal(cambioInicialActual);
                    cambioInicialActual = cambioActualDec.ToString("N7");
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                cambioInicialActual = cambioInicialActual.Replace(',', '.');

                //Actualizar la tabla de monedas GLT03
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT03 set ";
                query += "CAMBME = " + cambioInicialActual + ", MASCME = '" + importesDecimales + "', CINTME = '" + codigoInt + "', ";
                query += "DESCME = '" + desc + "' ";
                query += "where TIMOME = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT03", this.txtCodigo.Text, null);

                //Actualizar la tabla de tasas de cambio GLT13
                //Cambio inicial
                decimal cambioInicialDec = 0;
                try
                {
                    cambioInicialDec = Convert.ToDecimal(this.txtCambioInicial.Tag);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (cambioInicialDec != cambioActualDec)
                {
                    //Se modificó el cambio inicial
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLT13 set ";
                    query += "CAMBMF = " + cambioInicialActual;
                    query += " where TIMOMF = '" + this.codigo + "' and FCAMMF = 0";

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    this.txtCambioInicial.Tag = this.txtCambioInicial.Text;
                }

                if (this.grabarTasasCambio)
                {
                    string tasaCambioActual = "";
                    decimal tasaCambioActualDec = 0;
                    string fechaActual = "";
                    string fechaFormatoBBDD = "";
                    string especialEuro = "";
                    string tasaCambioOrigen = "";
                    string fechaOrigen = "";
                    string especialEuroOrigen = "";

                    string ano = "";
                    string nombreTabla = "";
                    for (int i = 0; i < this.radGridViewTasaCambio.Rows.Count - 1; i++)
                    {
                        //SMR fechaActual = this.radGridViewTasaCambio.Rows[i].Cells["Fecha"].Value.ToString();
                        DateTime dt = Convert.ToDateTime(this.radGridViewTasaCambio.Rows[i].Cells["Fecha"].Value.ToString());
                        fechaActual = dt.Day.ToString().PadLeft(2, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Year.ToString().PadLeft(4, '0');

                        if (fechaActual.Length == 8)
                        {
                            ano = fechaActual.Substring(6, 2);
                            ano = utiles.SigloDadoAnno(ano, CGParametrosGrles.GLC01_ALSIRC) + ano;
                            fechaFormatoBBDD = ano + fechaActual.Substring(2, 2) + fechaActual.Substring(0, 2);
                        }

                        tasaCambioActual = this.radGridViewTasaCambio.Rows[i].Cells["TasaCambio"].Value.ToString();
                        try
                        {
                            tasaCambioActualDec = Convert.ToDecimal(tasaCambioActual);
                            tasaCambioActual = tasaCambioActualDec.ToString("N7");
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        tasaCambioActual = tasaCambioActual.Replace(',', '.');

                        especialEuro = this.radGridViewTasaCambio.Rows[i].Cells["EspecialEuro"].Value.ToString();
                        if (especialEuro == "") especialEuro = " ";

                        tasaCambioOrigen = this.radGridViewTasaCambio.Rows[i].Cells["TasaCambioOrigen"].Value.ToString();
                        fechaOrigen = this.radGridViewTasaCambio.Rows[i].Cells["FechaOrigen"].Value.ToString();
                        especialEuroOrigen = this.radGridViewTasaCambio.Rows[i].Cells["EspecialEuroOrigen"].Value.ToString();

                        if (tasaCambioOrigen == "" && fechaOrigen == "" && especialEuroOrigen == "")
                        {
                            //Insertar tasa de cambio
                            nombreTabla = GlobalVar.PrefijoTablaCG + "GLT13";
                            query = "insert into " + nombreTabla + " (";
                            if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                            query += "TIMOMF, FCAMMF, CAMBMF, EUROMF) values (";
                            if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                            query += "'" + this.codigo + "', " + fechaFormatoBBDD + ", " + tasaCambioActual + ",'" + especialEuro + "')";

                            registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                            //Actualizar los campos ocultos con los nuevos valores
                            this.radGridViewTasaCambio.Rows[i].Cells["TasaCambioOrigen"].Value = tasaCambioActual;
                            this.radGridViewTasaCambio.Rows[i].Cells["FechaOrigen"].Value = fechaFormatoBBDD;
                            this.radGridViewTasaCambio.Rows[i].Cells["EspecialEuroOrigen"].Value = especialEuro;
                        }
                        else
                        {
                            //actualizar tasa de cambio
                            tasaCambioOrigen = tasaCambioOrigen.Replace(',', '.');

                            query = "update " + GlobalVar.PrefijoTablaCG + "GLT13 set ";
                            query += "CAMBMF = " + tasaCambioActual + ", FCAMMF = " + fechaFormatoBBDD + ", EUROMF = '" + especialEuro + "' ";
                            query += " where TIMOMF = '" + this.codigo + "' and FCAMMF = " + fechaOrigen;
                            query += " and CAMBMF = " + tasaCambioOrigen + " and EUROMF = '" + especialEuroOrigen + "'";

                            registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                            //Actualizar los campos ocultos con los nuevos valores
                            this.radGridViewTasaCambio.Rows[i].Cells["TasaCambioOrigen"].Value = tasaCambioActual;
                            this.radGridViewTasaCambio.Rows[i].Cells["FechaOrigen"].Value = fechaFormatoBBDD;
                            this.radGridViewTasaCambio.Rows[i].Cells["EspecialEuroOrigen"].Value = especialEuro;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una moneda
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string importesDecimales = this.radToggleSwitchImportesDecimales.Value ? "S" : "N";
                string cambioInicialActual = this.txtCambioInicial.Text;

                string codigoInt = this.txtCodInt.Text.Trim();
                codigoInt = (codigoInt == "") ? " " : codigoInt;

                string desc = this.txtDesc.Text.Trim();
                desc = (desc == "") ? " " : desc;
                decimal cambioActualDec = 0;

                try
                {
                    cambioActualDec = Convert.ToDecimal(cambioInicialActual);
                    cambioInicialActual = cambioActualDec.ToString("N7");
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                cambioInicialActual = cambioInicialActual.Replace(',', '.');

                //Actualizar la tabla de monedas GLT03
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT03 set ";
                query += "CAMBME = " + cambioInicialActual + ", MASCME = '" + importesDecimales + "', CINTME = '" + codigoInt + "', ";
                query += "DESCME = '" + desc + "' ";
                query += "where TIMOME = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT03", this.txtCodigo.Text, null);

                //Actualizar la tabla de tasas de cambio GLT13
                //Cambio inicial
                decimal cambioInicialDec = 0;
                try
                {
                    cambioInicialDec = Convert.ToDecimal(this.txtCambioInicial.Tag);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (cambioInicialDec != cambioActualDec)
                {
                    //Se modificó el cambio inicial
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLT13 set ";
                    query += "CAMBMF = " + cambioInicialActual;
                    query += " where TIMOMF = '" + this.codigo + "' and FCAMMF = 0";

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    this.txtCambioInicial.Tag = this.txtCambioInicial.Text;
                }

                //eliminar las tasa de cambio
                query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT13 ";
                query += "where TIMOMF = '" + this.codigo + "' and FCAMMF <> 0";

                try
                {
                    int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //instertar las tasa de cambio
                if (this.grabarTasasCambio)
                {
                    string tasaCambioActual = "";
                    decimal tasaCambioActualDec = 0;
                    string fechaActual = "";
                    string fechaFormatoBBDD = "";
                    string especialEuro = "";

                    string ano = "";
                    string nombreTabla = "";
                    for (int i = 0; i < this.radGridViewTasaCambio.Rows.Count; i++)
                    {
                        //SMR fechaActual = this.radGridViewTasaCambio.Rows[i].Cells["Fecha"].Value.ToString();
                        DateTime dt = Convert.ToDateTime(this.radGridViewTasaCambio.Rows[i].Cells["Fecha"].Value.ToString());
                        fechaActual = dt.Day.ToString().PadLeft(2, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Year.ToString().PadLeft(4, '0');

                        if (fechaActual.Length == 8)
                        {
                            ano = fechaActual.Substring(6, 2);
                            ano = utiles.SigloDadoAnno(ano, CGParametrosGrles.GLC01_ALSIRC) + ano;
                            fechaFormatoBBDD = ano + fechaActual.Substring(2, 2) + fechaActual.Substring(0, 2);
                        }

                        tasaCambioActual = this.radGridViewTasaCambio.Rows[i].Cells["TasaCambio"].Value.ToString();
                        try
                        {
                            tasaCambioActualDec = Convert.ToDecimal(tasaCambioActual);
                            tasaCambioActual = tasaCambioActualDec.ToString("N7");
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        tasaCambioActual = tasaCambioActual.Replace(',', '.');

                        especialEuro = this.radGridViewTasaCambio.Rows[i].Cells["EspecialEuro"].Value.ToString();
                        if (especialEuro == "") especialEuro = " ";


                        //Insertar tasa de cambio
                        nombreTabla = GlobalVar.PrefijoTablaCG + "GLT13";
                        query = "insert into " + nombreTabla + " (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                        query += "TIMOMF, FCAMMF, CAMBMF, EUROMF) values (";
                        if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                        query += "'" + this.codigo + "', " + fechaFormatoBBDD + ", " + tasaCambioActual + ",'" + especialEuro + "')";

                        registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dado una fecha en formato aammdd o saammdd devuelve la fecha en formato dd/mm/aa
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private string FechaConFormato(string fecha)
        {
            try
            {
                if (fecha.Length == 7) fecha = fecha.Substring(1, 6);
                if (fecha.Length == 6)
                {
                    fecha = fecha.Substring(4, 1) + fecha.Substring(5, 1) + "/" + fecha.Substring(2, 1) + fecha.Substring(3, 1) + "/" + fecha.Substring(0, 1) + fecha.Substring(1, 1);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (fecha);
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.txtCambioInicial.Tag = this.txtCambioInicial.Text;

            this.radToggleSwitchImportesDecimales.Tag = this.radToggleSwitchImportesDecimales.Value;
            
            this.txtCodInt.Tag = this.txtCodInt.Text;
            this.txtDesc.Tag = this.txtDesc.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.txtCambioInicial.Tag = "";

            this.radToggleSwitchImportesDecimales.Tag = true;

            this.txtCodInt.Tag = "";
            this.txtDesc.Tag = "";
        }

        /// <summary>
        /// Grabar una moneda
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtCodigo.Text.Trim();
                        this.txtCambioInicial.Tag = this.txtCambioInicial.Text;

                        this.txtCambioInicial.Focus();
                        this.Size = new System.Drawing.Size(571, 625);

                        //Habilitar Grid para las tasas de cambio
                        this.gbTasaCambio.Visible = true;

                        //Construir el DataGrid
                        this.BuildDataGridTasaCambio();
                        
                        this.radGridViewTasaCambio.ClearSelection();
                    }
                }
                else
                {
                    result = this.ActualizarInfo();

                    if (result == "") this.txtCambioInicial.Tag = this.txtCambioInicial.Text;
                }

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar una Moneda
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la Moneda " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en los planes contables
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                    query += "where TIMOMP = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en planes contables
                        mensaje = "No es posible eliminar la Moneda porque está en uso en planes contables.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        return;
                    }

                    //Buscar si existen entradas en el maestro de cuentas de mayor
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIMOMC = '" + this.codigo + "'";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en las cuentas de mayor
                        mensaje = "No es posible eliminar el Tipo de auxiliar porque está en uso en cuentas de mayor.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        return;
                    }

                    //Buscar si existen entradas en las cabeceras de comprobantes
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "where TIMOIC = '" + this.codigo + "'";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en las cabeceras de comprobantes
                        mensaje = "No es posible eliminar el Tipo de auxiliar porque está en uso en cabeceras de comprobantes.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar en la tabla de cambios
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT13 ";
                        query += "where TIMOMF = '" + this.codigo + "'";

                        try
                        {
                            cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        //Eliminar la moneda
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT03 ";
                        query += "where TIMOME = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT03", this.codigo, null);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar la Moneda.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Cerrar el formulario
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }
        #endregion

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs();
            args.Codigo = this.codigo;
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void radButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs();
            args.Codigo = "";
            args.Operacion = OperacionMtoTipo.Alta;
            DoUpdateDataForm(args);
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radGridViewTasaCambio_CellClick(object sender, GridViewCellEventArgs e)
        {
            Telerik.WinControls.UI.GridImageCellElement imageCellElement = sender as Telerik.WinControls.UI.GridImageCellElement;

            if (imageCellElement != null)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (this.radGridViewTasaCambio.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.radGridViewTasaCambio.Rows.Count)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        string tasaCambioOrigen = "";
                        string fechaOrigen = "";
                        string especialEuroOrigen = "";

                        if (this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["TasaCambioOrigen"].Value != null)
                            tasaCambioOrigen = this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["TasaCambioOrigen"].Value.ToString();

                        if (this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["FechaOrigen"].Value != null)
                            fechaOrigen = this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["FechaOrigen"].Value.ToString();

                        if (this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["EspecialEuroOrigen"].Value != null)
                            especialEuroOrigen = this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["EspecialEuroOrigen"].Value.ToString();

                        if (tasaCambioOrigen == "" && fechaOrigen == "" && especialEuroOrigen == "")
                        {
                            //Eliminarla del DataSet
                            //this.dtTasaCambioGrid.Rows.RemoveAt(e.RowIndex);
                            this.radGridViewTasaCambio.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            string tasaCambioActual = this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["TasaCambio"].Value.ToString();
                            string fechaActual = this.radGridViewTasaCambio.Rows[e.RowIndex].Cells["Fecha"].Value.ToString();

                            //Pedir confirmación y eliminar el cambio seleccionado
                            //this.LP.GetText("wrnDeleteConfirm"       

                            ///SMR elimina al guardar
                            ///
                            ///string mensaje = "Se va a eliminar el registro con valores en la tabla: fecha: " + this.FechaConFormato(fechaOrigen) + " tasa de cambio: " + tasaCambioOrigen;  //Falta traducir
                            ///mensaje += " y valores actuales fecha: " + this.FechaConFormato(fechaActual) + " tasa de cambio: " + tasaCambioActual + " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                            ///DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                            ///if (result == DialogResult.Yes)
                            ///{
                            ///    string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT13 where ";
                            ///    query += " TIMOMF = '" + this.codigo + "' and FCAMMF = " + fechaOrigen;
                            ///    query += " and CAMBMF = " + tasaCambioOrigen.Replace(',', '.') + " and EUROMF = '" + especialEuroOrigen + "'";
                            ///
                            ///    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            ///
                            ///    //Eliminar la entrada del DataSet
                            ///    this.dtTasaCambioGrid.Rows.RemoveAt(e.RowIndex);
                            ///}

                            this.radGridViewTasaCambio.Rows.RemoveAt(e.RowIndex);

                            this.radGridViewTasaCambio.Refresh();
                            this.radGridViewTasaCambio.ClearSelection();
                        }
                    }
                }
            }
        }

        private void radGridViewTasaCambio_CurrentRowChanging(object sender, CurrentRowChangingEventArgs e)
        {        
            if (!this.cargarInfoTasasCambio & !this.nuevoRegistro)
            {
                if (this.radGridViewTasaCambio.RowCount != 0)
                {
                    string result = "";
                    string error = this.LP.GetText("errValTitulo", "Error");

                    var rowActual = e.CurrentRow;

                    if (rowActual != null)
                    {
                        string fechaActual = rowActual.Cells["Fecha"].Value.ToString();
                        if (fechaActual != "")
                        {
                            result = ValidarFecha(fechaActual);
                            if (result != "")
                            {
                                RadMessageBox.Show(result, error);
                                return;
                            }
                        }

                        string tasaCambioActual = rowActual.Cells["TasaCambio"].Value.ToString();
                        if (tasaCambioActual != "")
                        {
                            string valorFormat = "";
                            result = ValidarTasaCambio(tasaCambioActual, ref valorFormat);
                            if (result != "")
                            {
                                RadMessageBox.Show(result, error);
                                return;
                            }
                            else
                            {
                                try
                                {
                                    decimal valorActual = Convert.ToDecimal(rowActual.Cells["TasaCambio"].Value);
                                    e.CurrentRow.Cells["TasaCambio"].Value = valorActual.ToString("N7");
                                }
                                catch
                                {
                                    e.CurrentRow.Cells["TasaCambio"].Value = valorFormat;
                                }
                            }
                        }
                    }
                }
            }

            if (this.nuevoRegistro) this.nuevoRegistro = false;
        }

        private void radGridViewTasaCambio_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            string result = "";
            string error = this.LP.GetText("errValTitulo", "Error");

            try
            {
                var rowActual = e.Row;

                if (rowActual.Cells["Fecha"].Value != null)
                {
                    string fechaActual = rowActual.Cells["Fecha"].Value.ToString();
                    if (fechaActual != "")
                    {
                        result = ValidarFecha(fechaActual);
                        if (result != "")
                        {
                            RadMessageBox.Show(result, error);
                            return;
                        }
                    }
                }

                if (rowActual.Cells["TasaCambio"].Value != null)
                {
                    string tasaCambioActual = rowActual.Cells["TasaCambio"].Value.ToString();
                    if (tasaCambioActual != "")
                    {
                        string valorFormat = "";
                        result = ValidarTasaCambio(tasaCambioActual, ref valorFormat);
                        if (result != "")
                        {
                            RadMessageBox.Show(result, error);
                            return;
                        }
                        else
                        {
                            try
                            {
                                decimal valorActual = Convert.ToDecimal(rowActual.Cells["TasaCambio"].Value);
                                rowActual.Cells["TasaCambio"].Value = valorActual.ToString("N7");
                            }
                            catch
                            {
                                rowActual.Cells["TasaCambio"].Value = valorFormat;
                            }
                        }
                    }
                }

                rowActual.Cells["Eliminar"].Value = global::ModMantenimientos.Properties.Resources.Eliminar;

                //Insertar en el DataSet   ???
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.nuevoRegistro = true;
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void radButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void radButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void radButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void frmMtoGLT03_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
