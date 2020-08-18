using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiLocalDescuadre : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIICUAD";
        public string ficheroExtension = "cdr";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIICUAD
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string ejercicio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string periodo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string porNifIdOtro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string nif;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string pais;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string tipoIdentificacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string noFactura;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaExpedicion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string soloDiferencias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string nombreRazonSocial;
        }

        FormularioValoresCampos valoresFormulario;

        private string libro;

        private ArrayList librosArray;
        private ArrayList tipoIdentificacionArray;
        private ArrayList paisArray;

        private string codigoCompania = "";
        private string ejercicioCG = "";
        private string tipoPeriodoImpositivo = "";

        private string consultaFiltroFecha = "";

        private string libroSel = "";
        private string soloDiferenciasSel = "";
        private string companiaSel = "";
        private string ejercicioSel = "";
        private string periodoSel = "";
        private string tipoIdentifNifIdOtroSel = "";
        private string nifSel = "";
        private string codPaisSel = "";
        private string tipoIdentifSel = "";
        private string nombreRazonSocialSel = "";
        private string numFacturaSel = "";
        private string fechaFacturaSel = "";

        private DataSet dsDatosAEAT = null;
        private DataSet dsDatosLocal = null;

        private DataTable dtDatosAEATYLocal = null;

        string errorAEAT = "";
       
        public frmSiiLocalDescuadre()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiLocalDescuadre_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Descuadre entre Datos Presentados y Datos Local");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Crear el desplegable de Periodos
            this.CrearComboPeriodos();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el desplegable de Pais
            this.CrearComboPais(false, "");

            //Crear el desplegable de Tipo de Identificación (Destinatario/Emisor/...)
            this.CrearComboTipoIdentificacion();

            //Crear el TGGrid de Diferencias
            this.BuiltgGridDiff();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (this.CargarValoresUltimaPeticion(valores)) { }
            }

            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();

            this.cmbLibro.Select();
        }

        private void tgTexBoxSelCiaFiscal_Enter(object sender, EventArgs e)
        {
            this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;
        }

        private void tgTexBoxSelCiaFiscal_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.tgTexBoxSelCiaFiscal.Textbox.Modified && this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
            {
                this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;

                this.codigoCompania = this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim();
                string result = this.ValidarCompania();
                if (result != "") MessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtEjercicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números con 7 posiciones decimales
            this.ValidarNumeroKeyPress(ref this.txtEjercicio, ref sender, ref e, false);
        }

        private void cmbPeriodo_Leave(object sender, EventArgs e)
        {
            this.cmbPeriodo.Text = this.cmbPeriodo.Text.ToUpper();

            if (!LibroUtiles.ValidarPeriodo(this.cmbPeriodo.Text))
            {
                MessageBox.Show("Por favor, entre un periodo válido (01-12 o " + LibroUtiles.PeriodoAnual + "o 1T-4T)");     //Falta traducir
                this.cmbPeriodo.Focus();
                return;
            }
            else if (this.cmbPeriodo.Text.Length == 1) this.cmbPeriodo.Text = "0" + this.cmbPeriodo.Text;
        }

        private void btnDescuadre_Click(object sender, EventArgs e)
        {
            this.Descuadre();
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void frmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion();
            frmPeticion.FormCode = this.formCode;
            frmPeticion.FicheroExtension = this.ficheroExtension;
            frmPeticion.Formulario = this;
            string result = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));

            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();

            this.tgGridDiff.Visible = false;
            this.gbFacturasTodas.Visible = false;
            this.lblTotalDescuadre.Visible = false;

            this.toolStripButtonExportar.Enabled = false;
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();    //Exporta a un HTML temporal y despúes se muestra en un Excel
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtEjercicio.Text = "";
            if (this.cmbPeriodo.Enabled) this.cmbPeriodo.Text = "";
            this.rbNIF.Checked = true;
            this.txtNIF.Text = "";
            this.cmbPais.Text = "";
            this.cmbTipoIdentif.Text = "";
            this.txtNumSerieFactura.Text = "";
            this.txtMaskFechaExpedicion.Text = "";
            this.chkSoloDescuadre.Checked = true;
            this.txtNombreRazonSocial.Text = "";

            this.lblResultInfo.Visible = false;
            this.tgGridDiff.Visible = false;
            this.lblTotalDescuadre.Visible = false;
            this.toolStripButtonExportar.Enabled = false;
            this.gbFacturasTodas.Visible = false;
        }

        private void tgGridAEAT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Chequea si la celda donde se ha hecho click es la de visualizar la factura o la de ver los movimientos de la factura
            this.FacturaVerMov(sender, e);
        }

        private void tgGridLocal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Chequea si la celda donde se ha hecho click es la de visualizar la factura o la de ver los movimientos de la factura
            this.FacturaVerMov(sender, e);
        }

        private void tgGridAEAT_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Visualizar el detalle de la factura al hacer doble click en la grid de la AEAT o en la grid de Datos en Local
            this.FacturaVerDetalle(sender, e);
        }

        private void tgGridLocal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Visualizar el detalle de la factura al hacer doble click en la grid de la AEAT o en la grid de Datos en Local
            this.FacturaVerDetalle(sender, e);
        }

        private void cmbLibro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();
        }

        private void tgGridAEAT_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridAEAT.Rows.Count >= 1 && e.RowIndex <= this.tgGridAEAT.Rows.Count)
                {
                    string columnName = this.tgGridAEAT.Columns[e.ColumnIndex].Name;

                    switch (columnName)
                    {
                        case "ImporteTotal":
                        case "BaseImponible":
                        case "CuotaDeducible":
                        case "Cuota":
                            try
                            {
                                if (e.Value != null)
                                {
                                    string valor = e.Value.ToString().Trim();
                                    if (valor != "")
                                    {
                                        decimal d = decimal.Parse(valor, System.Globalization.CultureInfo.InvariantCulture);
                                        e.Value = d.ToString("N2");
                                    }
                                }
                            }
                            catch { }
                            break;
                    }
                }
            }
        }

        private void rbNIF_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbNIF.Checked)
            {
                this.lblNIF.Text = "NIF Destinatario";
                this.txtNIF.MaxLength = 9;
                this.lblCodPais.Enabled = false;
                this.cmbPais.Enabled = false;
                this.lblCodPais.Enabled = false;
                this.cmbPais.Enabled = false;
                this.lblTipoIden.Enabled = false;                
                this.cmbTipoIdentif.Enabled = false;
                //this.lblNombreRazon.Enabled = false;
                //this.txtNombreRazon.Enabled = false;
                this.cmbTipoIdentif.SelectedValue = "";

                switch (this.cmbLibro.SelectedValue.ToString())
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:
                        this.lblNIF.Text = "NIF Destinatario";
                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:
                    case LibroUtiles.LibroID_BienesInversion:
                        this.lblNIF.Text = "NIF Emisor";
                        break;
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        this.lblNIF.Text = "NIF Dest./Emisor";
                        break;
                    case LibroUtiles.LibroID_PagosRecibidas:
                        break;
                    case LibroUtiles.LibroID_CobrosMetalico:
                        this.lblNIF.Text = "NIF Contraparte";
                        break;
                    case LibroUtiles.LibroID_OperacionesSeguros:
                        this.lblNIF.Text = "NIF Contraparte";
                        break;
                    case LibroUtiles.LibroID_AgenciasViajes:
                        this.lblNIF.Text = "NIF Contraparte";
                        break;
                    case LibroUtiles.LibroID_CobrosEmitidas:
                        break;
                }
            }
            else
            {
                this.lblNIF.Text = "Identificador Dest.";
                this.txtNIF.MaxLength = 20;
                this.lblCodPais.Enabled = true;
                this.cmbPais.Enabled = true;
                this.lblTipoIden.Enabled = true;
                this.cmbTipoIdentif.Enabled = true;
                //this.lblNombreRazon.Enabled = true;
                //this.txtNombreRazon.Enabled = true;

                if (this.cmbLibro.SelectedValue.ToString() == LibroUtiles.LibroID_OperacionesIntracomunitarias)  //Operaciones Intracomunitarias
                {
                    this.cmbTipoIdentif.SelectedValue = "02";
                    this.cmbTipoIdentif.Enabled = false;
                    this.lblTipoIden.Enabled = false;
                }
                else
                {
                    this.cmbTipoIdentif.Enabled = true;
                    this.lblTipoIden.Enabled = true;
                }
            }
        }

        private void frmSiiLocalDescuadre_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void frmSiiLocalDescuadre_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Descuadre entre Datos Presentados y Datos Local");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiLocalDescuadreTitulo", "Comprobación entre Datos presentados y Datos Local");
            this.Text += this.FormTituloAgenciaEntorno();

            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoNombreRazonSocial", "Nombre o razón social"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoNIF", "NIF"));
            nombreColumnas.Add("Año-Mes Cerrado IVA");
            nombreColumnas.Add("Agencia");

            if (this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel != null) this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel.Clear();
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = nombreColumnas;
        }

        /// <summary>
        /// Construir el control de seleccion de compañías fiscales
        /// </summary>
        private void BuildtgTexBoxSelCiaFiscal()
        {
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 25;
            this.tgTexBoxSelCiaFiscal.AjustarTamanoTextBox();

            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 4;
            this.tgTexBoxSelCiaFiscal.Textbox.MaxLength = 2;

            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = GlobalVar.ConexionCG;
            
            this.tgTexBoxSelCiaFiscal.QueryFormSel = this.ObtenerQueryListaCompaniasFiscales();

            this.tgTexBoxSelCiaFiscal.FrmPadre = this;
        }

        /// <summary>
        /// Crea el desplegable de Periodos
        /// </summary>
        private void CrearComboPeriodos()
        {
            try
            {
                librosArray = new ArrayList();
                librosArray.Add(new AddValue("", ""));
                librosArray.Add(new AddValue("01", "01"));
                librosArray.Add(new AddValue("02", "02"));
                librosArray.Add(new AddValue("03", "03"));
                librosArray.Add(new AddValue("04", "04"));
                librosArray.Add(new AddValue("05", "05"));
                librosArray.Add(new AddValue("06", "06"));
                librosArray.Add(new AddValue("07", "07"));
                librosArray.Add(new AddValue("08", "08"));
                librosArray.Add(new AddValue("09", "09"));
                librosArray.Add(new AddValue("10", "10"));
                librosArray.Add(new AddValue("11", "11"));
                librosArray.Add(new AddValue("12", "12"));
                librosArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));

                this.cmbPeriodo.DataSource = librosArray;
                this.cmbPeriodo.DisplayMember = "Display";
                this.cmbPeriodo.ValueMember = "Value";

                this.cmbPeriodo.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Libros
        /// </summary>
        private void CrearComboLibros()
        {
            try
            {
                //string textoValor0 = this.LP.GetText("lblLibroTodos", "Todos");
                string textoValor1 = this.LP.GetText("lblLibroFactEmitidas", "Facturas Emitidas");
                string textoValor2 = this.LP.GetText("lblLibroFacturasRecibidas", "Facturas Recibidas");
                string textoValor3 = this.LP.GetText("lblLibroBienesInversion", "Bienes de Inversión");
                string textoValor4 = this.LP.GetText("lblLibroDetOperaIntra", "Operaciones Intracomunitarias");
                //string textoValor5 = this.LP.GetText("lblLibroCobros", "Cobros Emitidas");
                string textoValor6 = this.LP.GetText("lblLibroPagos", "Pagos Recibidas RECC");
                string textoValor7 = this.LP.GetText("lblLibroCobrosMetálico", "Cobros en metálico");
                string textoValor8 = this.LP.GetText("lblLibroOperacionesSeguros", "Operaciones de seguros");
                string textoValor9 = this.LP.GetText("lblLibroAgenciasViajes", "Agencias de viajes");

                librosArray = new ArrayList();
                //librosArray.Add(new AddValue(textoValor0, "0"));                          //Todos
                librosArray.Add(new AddValue(textoValor1, LibroUtiles.LibroID_FacturasEmitidas));       //Facturas Emitidas
                librosArray.Add(new AddValue(textoValor2, LibroUtiles.LibroID_FacturasRecibidas));      //Facturas Recibidas
                librosArray.Add(new AddValue(textoValor3, LibroUtiles.LibroID_BienesInversion));        //Bienes de inversión
                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor4, LibroUtiles.LibroID_OperacionesIntracomunitarias));       //Determinadas Operaciones Intracomunitarias
                librosArray.Add(new AddValue(textoValor6, LibroUtiles.LibroID_PagosRecibidas));         //Pagos Recibidas
                //librosArray.Add(new AddValue(textoValor5, LibroUtiles.LibroID_CobrosEmitidas));         //Cobro Emitidas
                librosArray.Add(new AddValue(textoValor7, LibroUtiles.LibroID_CobrosMetalico));         //Cobros en metálico
                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor8, LibroUtiles.LibroID_OperacionesSeguros));     //Operaciones de seguros
                librosArray.Add(new AddValue(textoValor9, LibroUtiles.LibroID_AgenciasViajes));         //Agencias de viajes
                
                this.cmbLibro.DataSource = librosArray;
                this.cmbLibro.DisplayMember = "Display";
                this.cmbLibro.ValueMember = "Value";

                this.cmbLibro.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Tipos de Identificación
        /// </summary>
        private void CrearComboPais(bool filtro, string valorFiltro)
        {
            try
            {
                paisArray = new ArrayList();
                paisArray.Add(new AddValue("", ""));
                this.ObtenerPaises(filtro, valorFiltro, ref paisArray);
                
                this.cmbPais.DataSource = paisArray;
                this.cmbPais.DisplayMember = "Display";
                this.cmbPais.ValueMember = "Value";

                this.cmbPais.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Tipos de Identificación
        /// </summary>
        private void CrearComboTipoIdentificacion()
        {
            try
            {
                tipoIdentificacionArray = new ArrayList();
                tipoIdentificacionArray.Add(new AddValue("", ""));
                tipoIdentificacionArray.Add(new AddValue("NIF-IVA", "02"));
                tipoIdentificacionArray.Add(new AddValue("Pasaporte", "03"));
                tipoIdentificacionArray.Add(new AddValue("Documento oficial de identificación expedido por el país o territorio de residencia", "04"));
                tipoIdentificacionArray.Add(new AddValue("Certificado de residencia", "05"));
                tipoIdentificacionArray.Add(new AddValue("Otro documento probatorio", "06"));
                tipoIdentificacionArray.Add(new AddValue("No censado", "07"));

                this.cmbTipoIdentif.DataSource = tipoIdentificacionArray;
                this.cmbTipoIdentif.DisplayMember = "Display";
                this.cmbTipoIdentif.ValueMember = "Value";

                this.cmbTipoIdentif.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Construis la Grid de comparación
        /// </summary>
        private void BuiltgGridDiff()
        {
            //Crear el DataGrid
            this.tgGridDiff.dsDatos = new DataSet();
            this.tgGridDiff.dsDatos.DataSetName = "Comprobacion";
            this.tgGridDiff.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridDiff.ReadOnly = true;
            this.tgGridDiff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridDiff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridDiff.AllowUserToAddRows = false;
            this.tgGridDiff.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridDiff.NombreTabla = "Comprobacion";
        }

        /// <summary>
        /// Valida que sólo se introduzcan números
        /// </summary>
        /// <param name="textbox">Control TextBox</param>
        /// <param name="sender">el sender (object) del argumento del evento KeyPress</param>
        /// <param name="e">el e (KeyPressEventArgs) del argumento del evento KeyPress</param>
        /// <param name="negalivo">true -> permite números negativos    false -> no permite números negativos</param>
        public void ValidarNumeroKeyPress(ref System.Windows.Forms.TextBox textbox, ref object sender, ref System.Windows.Forms.KeyPressEventArgs e, bool negativo)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)                 //Número
                e.Handled = false;
            else
                if (negativo && (e.KeyChar == 45 && textbox.Text.Length == 0))    //Negativo (Menos) y cadena vacía, es decir el signo negativo en primera posición
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// Valida la compañia del formulario (valor compañía que exista en la variable this.codigoCompania)
        /// </summary>
        /// <returns></returns>
        private string ValidarCompania()
        {
            string result = "";
            try
            {
                if (this.codigoCompania == "")
                {
                    result = "La compañia no puede estar en blanco";
                    return (result);
                }

                if (this.codigoCompania.Length >= 2)
                {
                    if (this.codigoCompania.Length > 2) this.codigoCompania = this.codigoCompania.Substring(0, 2);

                    string companiaDesc = "";
                    string companiaNIF = "";
                    result = this.ValidarCompaniaFiscal(this.codigoCompania, ref companiaDesc, ref companiaNIF, ref this.tipoPeriodoImpositivo);

                    if (result != "")
                    {
                        //string error = this.LP.GetText("errValTitulo", "Error");
                        //MessageBox.Show(result, error);
                        this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
                        this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    }
                    else
                    {
                        string codigoComp = this.codigoCompania;
                        if (companiaDesc != "") codigoComp += " - " + companiaDesc;
                        if (companiaNIF != "") codigoComp += " - " + companiaNIF;

                        this.tgTexBoxSelCiaFiscal.Textbox.Text = codigoComp;
                    }
                }
                else
                {
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    result = "La compañía no es válida";
                    //MessageBox.Show("La compañía no es válida", "Error");   //Falta traducir
                    //this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                }
            }
            catch (Exception ex)
            {
                result = "Error validando la compañía";
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }

        /// <summary>
        /// Validar el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            try
            {
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "")
                {
                    MessageBox.Show("Es obligatorio informar la compañía", "Error");   //Falta traducir
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    return (false);
                }
                else
                {
                    this.codigoCompania = this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim();
                    string result = this.ValidarCompania();
                    if (result != "")
                    {
                        MessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
                        return (false);
                    }
                }

                bool fechaValida = true;
                string fechaStr = "";
                DateTime fechaDateTime = new DateTime();

                this.consultaFiltroFecha = "";
                //coger el valor sin la máscara
                this.txtMaskFechaExpedicion.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFecha = this.txtMaskFechaExpedicion.Text.Trim();
                this.txtMaskFechaExpedicion.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (this.consultaFiltroFecha != "")
                {
                    fechaStr = this.txtMaskFechaExpedicion.Text;
                    fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaDateTime);
                    if (fechaStr != this.txtMaskFechaExpedicion.Text) this.txtMaskFechaExpedicion.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de expedición no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaExpedicion.Focus();
                        return (false);
                    }
                }

                string ejercicio = this.txtEjercicio.Text.Trim();
                if (this.txtEjercicio.Text.Length == 2)
                {
                    //Completar el ejercicio a formato aaaa
                    ejercicio = "20" + ejercicio;
                }

                DateTime fechaActual = DateTime.Now;
                int ejercicioInt = 0;
                if (ejercicio != "")
                {
                    //El ejercicio no puede ser mayor que el año actual
                    ejercicioInt = Convert.ToInt16(ejercicio);
                }

                switch (this.libro)
                {
                    case LibroUtiles.LibroID_PagosRecibidas:
                    case LibroUtiles.LibroID_CobrosEmitidas:
                        if (this.txtNIF.Text == "")
                        {
                            if (this.rbOtro.Checked) MessageBox.Show("Es obligatorio informar el Identificador", "Error");   //Falta traducir
                            else MessageBox.Show("Es obligatorio informar el NIF del destinatario", "Error");   //Falta traducir
                            this.txtNIF.Focus();
                            return (false);
                        }

                        if (this.rbOtro.Checked && this.cmbTipoIdentif.SelectedValue.ToString() == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Tipo de Identificación del destinatario", "Error");   //Falta traducir
                            this.cmbTipoIdentif.Focus();
                            return (false);
                        }

                        if (this.rbOtro.Checked && this.cmbPais.SelectedValue.ToString() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02")
                        {
                            MessageBox.Show("Es obligatorio informar el Código del País del destinatario", "Error");   //Falta traducir
                            this.cmbPais.Focus();
                            return (false);
                        }
                        
                        if (this.txtNombreRazonSocial.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Nombre o Razón Social del destinatario", "Error");   //Falta traducir
                            this.txtNombreRazonSocial.Focus();
                            return (false);
                        }
                        
                        if (this.txtNumSerieFactura.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Número de Serie de la Factura", "Error");   //Falta traducir
                            this.txtNumSerieFactura.Focus();
                            return (false);
                        }

                        if (this.consultaFiltroFecha == "")
                        {
                            MessageBox.Show("Es obligatorio informar la Fecha de Expedición", "Error");   //Falta traducir
                            this.txtMaskFechaExpedicion.Focus();
                            return (false);
                        }
                         
                        break;
                    default:
                        if (this.txtEjercicio.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el ejercicio", "Error");   //Falta traducir
                            this.txtEjercicio.Focus();
                            return (false);
                        }
                        if (this.txtEjercicio.Text.Length == 1 || this.txtEjercicio.Text.Length == 3)
                        {
                            MessageBox.Show("El ejercicio no tiene un formato válido (aa o aaaa)", "Error");   //Falta traducir
                            this.txtEjercicio.Focus();
                            return (false);
                        }
                        
                        if (ejercicioInt > fechaActual.Date.Year)
                        {
                            MessageBox.Show("El ejercicio no puede ser mayor que el año en curso", "Error");   //Falta traducir
                            this.txtEjercicio.Focus();
                            return (false);
                        }

                        //Periodo
                        if (this.cmbPeriodo.SelectedIndex == 0)
                        {
                            MessageBox.Show("Debe indicar el periodo", "Error");   //Falta traducir
                            this.cmbPeriodo.Focus();
                            return (false);
                        }

                        //Validar Identificador de la factura si procede
                        if (this.rbOtro.Checked)
                        {
                            bool errorIdExtranjero = false;
                            if (this.txtNIF.Text.Trim() == "" ||
                                this.cmbTipoIdentif.SelectedValue.ToString() == "" ||
                                (this.cmbPais.SelectedValue.ToString() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02"))
                                //||this.txtNombreRazon.Text == ""
                                errorIdExtranjero = true;

                            if (errorIdExtranjero)
                            {
                                if (this.txtNIF.Text.Trim() == "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Identificador", "Error");   //Falta traducir
                                    this.txtNIF.Focus();
                                    return (false);
                                }

                                if (this.cmbTipoIdentif.SelectedValue.ToString() == "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Tipo de Identificación", "Error");   //Falta traducir
                                    this.cmbTipoIdentif.Focus();
                                    return (false);
                                }

                                if (this.cmbPais.SelectedValue.ToString() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02")
                                {
                                    MessageBox.Show("Es obligatorio informar el Código del País", "Error");   //Falta traducir
                                    this.cmbPais.Focus();
                                    return (false);
                                }

                                if (this.txtNombreRazonSocial.Text == "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Nombre o Razón Social", "Error");   //Falta traducir
                                    this.txtNombreRazonSocial.Focus();
                                    return (false);
                                }
                            }
                        }
                        else
                        {
                            if (this.txtNIF.Text == "" && this.txtNombreRazonSocial.Text != "")
                            {
                                MessageBox.Show("Es obligatorio informar el NIF", "Error");   //Falta traducir
                                this.txtNIF.Focus();
                                return (false);
                            }
                            else
                            {
                                if (this.txtNombreRazonSocial.Text == "" && this.txtNIF.Text != "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Nombre o Razón Social", "Error");   //Falta traducir
                                    this.txtNombreRazonSocial.Focus();
                                    return (false);
                                }
                            }
                        }
                        break;
                }

                //Verificar que esté informada y sea correcta la url del servicio web del sii
                if (this.serviceSII.URL == null || this.serviceSII.URL == "")
                {
                    MessageBox.Show("La dirección del servicio web que comunica con el SII no está informada. Por favor contacte con el administrador del sistema", "Error");   //Falta traducir
                    return (false);
                }
                if (!LibroUtiles.IsReachableUri(this.serviceSII.URL))
                {
                    MessageBox.Show("La dirección del servicio web que comunica con el SII no es correcta. Por favor contacte con el administrador del sistema", "Error");   //Falta traducir
                    return (false);
                }

                this.txtEjercicio.Text = ejercicio;
                if (ejercicio.Length == 4) this.ejercicioCG = ejercicio.Substring(2, 2);

                if (this.consultaFiltroFecha != "") this.consultaFiltroFecha = this.txtMaskFechaExpedicion.Text;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.libroSel = this.libro;
            this.soloDiferenciasSel = this.chkSoloDescuadre.Checked.ToString();
            this.companiaSel = this.codigoCompania;
            this.ejercicioSel = this.txtEjercicio.Text.Trim();
            this.periodoSel = this.cmbPeriodo.SelectedValue.ToString(); ;
            this.tipoIdentifNifIdOtroSel = this.rbOtro.Checked.ToString();
            this.nifSel = this.txtNIF.Text.Trim();
            this.codPaisSel = this.cmbPais.SelectedValue.ToString();
            this.tipoIdentifSel = this.cmbTipoIdentif.SelectedValue.ToString();
            this.nombreRazonSocialSel = this.txtNombreRazonSocial.Text.Trim();
            this.numFacturaSel = this.txtNumSerieFactura.Text.Trim();
            this.fechaFacturaSel = this.txtMaskFechaExpedicion.Text;

            return (true);
        }

        /// <summary>
        /// Llama a la calcular el descuadre correspondiente según el libro solicitado
        /// </summary>
        private void Descuadre()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.lblInfo.Visible = true;
                this.lblInfo.Update();

                this.lblTotalDescuadre.Visible = false;
                this.tgGridDiff.Visible = false;
                this.lblResultInfo.Visible = false;
                this.gbFacturasTodas.Visible = false;
                this.toolStripButtonExportar.Enabled = false;

                this.libro = this.cmbLibro.SelectedValue.ToString();
                string periodo = this.cmbPeriodo.SelectedValue.ToString();

                this.errorAEAT = "";

                if (this.libroSel == this.libro && this.companiaSel == this.codigoCompania &&
                    this.ejercicioSel == this.txtEjercicio.Text.Trim() && this.periodoSel == this.cmbPeriodo.SelectedValue.ToString() &&
                    this.tipoIdentifNifIdOtroSel == this.rbOtro.Checked.ToString() && this.nifSel == this.txtNIF.Text.Trim() &&
                    this.codPaisSel == this.cmbPais.SelectedValue.ToString() &&
                    this.tipoIdentifSel == this.cmbTipoIdentif.SelectedValue.ToString() &&
                    this.nombreRazonSocialSel == this.txtNombreRazonSocial.Text.Trim() &&
                    this.numFacturaSel == this.txtNumSerieFactura.Text.Trim() &&
                    this.fechaFacturaSel == this.txtMaskFechaExpedicion.Text &&
                    this.soloDiferenciasSel != this.chkSoloDescuadre.Checked.ToString())
                {
                    this.BuscarDescuadre(this.codigoCompania, this.ejercicioCG, periodo, false);
                    this.soloDiferenciasSel = this.chkSoloDescuadre.Checked.ToString();
                }
                else
                    if (this.FormValid())
                    {
                        this.BuscarDescuadre(this.codigoCompania, this.ejercicioCG, periodo, true);

                        //Grabar la petición
                        string valores = this.ValoresPeticion();

                        this.valoresFormulario.GrabarParametros(formCode, valores);
                    }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;

            if (this.tgGridDiff != null && this.tgGridDiff.Rows.Count > 0) this.tgGridDiff.ClearSelection();

            this.lblInfo.Visible = false;
            this.lblInfo.Update();
        }

        /// <summary>
        /// Busca posibles descuadres
        /// </summary>
        private void BuscarDescuadre(string compania, string ejercicio, string periodo, bool buscarDatos)
        {
            try
            {
                if (this.tgGridDiff.dsDatos.Tables.Count > 0 && buscarDatos)
                {
                    if (this.tgGridDiff.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridDiff.dsDatos.Tables.Remove("DatosGenerales");
                    if (this.tgGridDiff.dsDatos.Tables.Contains("MasInfo")) this.tgGridDiff.dsDatos.Tables.Remove("MasInfo");
                    if (this.tgGridDiff.dsDatos.Tables.Contains("DetalleIVA")) this.tgGridDiff.dsDatos.Tables.Remove("DetalleIVA");
                    if (this.tgGridDiff.dsDatos.Tables.Contains("EstadoCuadre")) this.tgGridDiff.dsDatos.Tables.Remove("EstadoCuadre");
                    if (this.tgGridDiff.dsDatos.Tables.Contains("Resultado")) this.tgGridDiff.dsDatos.Tables.Remove("Resultado");
                    if (this.tgGridDiff.dsDatos.Tables.Contains("Resumen")) this.tgGridDiff.dsDatos.Tables.Remove("Resumen");
                }

                if (buscarDatos)                
                {
                    //Eliminar todas las tablas del dataset de la AEAT
                    if (dsDatosAEAT != null && this.dsDatosAEAT.Tables != null && this.dsDatosAEAT.Tables.Count > 0)
                    {
                        this.dsDatosAEAT.Tables.Clear();
                        this.dsDatosAEAT.Clear();
                    }

                    //Eliminar todas las tablas del dataset de datos Local
                    if (this.dsDatosLocal != null && this.dsDatosLocal.Tables != null && this.dsDatosLocal.Tables.Count > 0)
                    {
                        this.dsDatosLocal.Tables.Clear();
                        this.dsDatosLocal.Clear();
                    }

                    this.dsDatosAEAT = this.ObtenerDatosAEAT(compania, ejercicio, periodo);
                    this.dsDatosLocal = this.ObtenerDatosLocal(compania, ejercicio, periodo, this.agencia);

                    DataTable dtDatosCompararAEAT = new DataTable();
                    DataTable dtDatosCompararLocal = new DataTable();

                    this.dtDatosAEATYLocal = new DataTable();

                    switch (this.libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalFacturasEmitidas(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, buscarDatos);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalFacturasRecibidas(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, buscarDatos);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalBienesInversion(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, buscarDatos);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalDetOperacionesIntracomunitarias(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, buscarDatos);
                            break;
                        case LibroUtiles.LibroID_CobrosMetalico:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalCobrosMetalico(ref dtDatosCompararAEAT, ref dtDatosCompararLocal);
                            break;
                        case LibroUtiles.LibroID_AgenciasViajes:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalAgenciasViaje(ref dtDatosCompararAEAT, ref dtDatosCompararLocal);
                            break;
                        case LibroUtiles.LibroID_OperacionesSeguros:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalOperacionesSeguros(ref dtDatosCompararAEAT, ref dtDatosCompararLocal);
                            break;
                        case LibroUtiles.LibroID_PagosRecibidas:
                            dtDatosAEATYLocal = this.ObtenerDatosAEATYLocalPagosRecibidas(ref dtDatosCompararAEAT, ref dtDatosCompararLocal);
                            break;
                    }

                    if (libro != LibroUtiles.LibroID_PagosRecibidas)
                    {
                        //Buscar la info manual de la comparación (importes, estados, cantidades en local pueden ser mas de 1 por cargo/abono)
                        this.ComparaDataTable(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, ref dtDatosAEATYLocal);
                    }
                    else
                    {
                        //Buscar la info manual de la comparación para el libro de pagos recibidas
                        this.ComparaDataTablePagosRecibidas(ref dtDatosCompararAEAT, ref dtDatosCompararLocal, ref dtDatosAEATYLocal);
                    }

                    this.tgGridDiff.DataSource = dtDatosAEATYLocal;

                    if (this.chkSoloDescuadre.Checked)
                    {
                        //Mostrar sólo los registros que descuadran
                        DataTable dtDatosGridDiff = this.ObtenerFacturasSoloDiferencia();
                        this.tgGridDiff.DataSource = dtDatosGridDiff;
                    }

                    /*
                    //Marcar las filas posibles de actualizar de color negro y con letras en negrita
                    //Posibles registros para actualizar el estado
                    var rowsUpdate = dtDatosAEATYLocal.AsEnumerable().Cast<DataRow>().Where(row =>
                                                                            (row["EstadoSII"].ToString() != row["EstadoLocal"].ToString() &&
                                                                            Convert.ToInt16(row["CantidadSII"]) == 1 && Convert.ToInt16(row["CantidadLocal"]) >= 1
                                                                            )).ToArray();

                    //Loop through and remove the rows that meet the condition
                    int indice = 0;
                    int contador = 0;

                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.Font = new Font(this.tgGridDiff.Font, FontStyle.Bold);
                    style.ForeColor = Color.Black;

                    foreach (DataRow dr in rowsUpdate)
                    {
                        //Mostrar todos los registros                        
                        indice = dtDatosAEATYLocal.Rows.IndexOf(dr);
                        this.tgGridDiff.Rows[indice].DefaultCellStyle = style;
                        this.tgGridDiff.Rows[indice].ReadOnly = false;
                        contador++;
                    }
                    */
                }
                else
                {
                    //Refrescar las filas de la Grid, dependiendo del filtro de solo diferencias o no
                    if (this.chkSoloDescuadre.Checked)
                    {
                        //Mostrar sólo los registros que descuadran
                        DataTable dtDatosGridDiff = this.ObtenerFacturasSoloDiferencia();
                        this.tgGridDiff.DataSource = dtDatosGridDiff;
                    }
                    else
                    {
                        //Mostrar todas las facturas
                        this.tgGridDiff.DataSource = dtDatosAEATYLocal;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Error(Utiles.CreateExceptionString(ex));
                string error = ex.Message;
            }

            try
            {
                bool existenDatosAEAT = (this.tgGridAEAT.Rows != null && this.tgGridAEAT.Rows.Count > 0) ? true : false;
                bool existenDatosLocal = (this.tgGridLocal.Rows != null && this.tgGridLocal.Rows.Count > 0) ? true : false;
                bool existenDatosDiff = (this.tgGridDiff.Rows != null && this.tgGridDiff.Rows.Count > 0) ? true : false;

                if (existenDatosDiff)
                {
                    if (this.libro != LibroUtiles.LibroID_PagosRecibidas)
                    {
                        //Ocultar y cambiar nombres de las columnas
                        this.GridColumnas(ref this.tgGridDiff);
                        if (this.tgGridAEAT.Columns.Contains("CargoAbono")) this.tgGridAEAT.Columns["CargoAbono"].Visible = false;

                        if (this.tgGridDiff.Columns.Contains("CargoAbono"))
                        {
                            switch (this.libro)
                            {
                                case LibroUtiles.LibroID_FacturasEmitidas:
                                case LibroUtiles.LibroID_FacturasRecibidas:
                                    this.tgGridDiff.Columns["CargoAbono"].Visible = true;
                                    break;
                                default:
                                    this.tgGridDiff.Columns["CargoAbono"].Visible = false;
                                    break;
                            }
                        }

                        if (this.tgGridAEAT.Columns.Contains("ClaveOperacion")) this.tgGridAEAT.Columns["ClaveOperacion"].Visible = false;
                        if (this.tgGridDiff.Columns.Contains("ClaveOperacionDesc"))
                        {
                            switch (this.libro)
                            {
                                case LibroUtiles.LibroID_OperacionesSeguros:
                                    this.tgGridDiff.Columns["ClaveOperacionDesc"].Visible = true;
                                    this.tgGridDiff.Columns["ClaveOperacionDesc"].DisplayIndex = 4;
                                    break;
                                default:
                                    this.tgGridDiff.Columns["ClaveOperacionDesc"].Visible = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Ocultar y cambiar nombres de las columnas
                        this.GridColumnasPagoRecibidas(ref this.tgGridDiff);
                    }

                    if (this.libro == LibroUtiles.LibroID_OperacionesSeguros)
                    {
                        if (this.tgGridDiff.Columns.Contains("IDDestinatario_Emisor")) this.tgGridDiff.Columns["IDDestinatario_Emisor"].DisplayIndex = 0;
                    }

                    //Ajustar todas las columnas de la Grid
                    this.tgGridDiff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    int indice = this.tgGridDiff.Columns.Count - 1;
                    for (int i = indice; i > 0; i--)
                    {
                        if (this.tgGridDiff.Columns[i].Visible == true)
                        {
                            indice = i;
                            break;
                        }                    
                    }
                    this.tgGridDiff.Columns[indice].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    this.lblResultInfo.Visible = false;
                    this.tgGridDiff.Visible = true;

                    this.lblTotalDescuadre.Text = "Total Registros: " + this.tgGridDiff.Rows.Count;
                    this.lblTotalDescuadre.Visible = true;

                    this.toolStripButtonExportar.Enabled = true;

                    this.gbFacturasTodas.Location = new Point(this.gbFacturasTodas.Location.X, 536);
                    //this.gbFacturasTodas.Size = new Size(this.gbFacturasTodas.Width, 317);
                }
                else
                {
                    if (existenDatosLocal && existenDatosAEAT && this.chkSoloDescuadre.Checked) this.lblResultInfo.Text = "No existen diferencias para el criterio seleccionado";   //Falta traducir
                    else this.lblResultInfo.Text = "No existen facturas a comparar que cumplan el criterio seleccionado";   //Falta traducir
                    this.lblResultInfo.Visible = true;
                    this.tgGridDiff.Visible = false;
                    this.lblTotalDescuadre.Visible = false;
                    this.toolStripButtonExportar.Enabled = false;

                    //this.gbFacturasTodas.Location = new Point(this.gbFacturasTodas.Location.X, this.tgGridDiff.Location.Y + 20);
                    //this.gbFacturasTodas.Size = new Size(this.gbFacturasTodas.Width, this.gbFacturasTodas.Height + this.tgGridDiff.Height - 20);
                }

                if (existenDatosLocal || existenDatosAEAT)
                {
                    this.gbFacturasTodas.Visible = true;

                    if (existenDatosAEAT)
                    {
                        //Ocultar y cambiar nombres de las columnas
                        if (this.libro != LibroUtiles.LibroID_PagosRecibidas) this.GridColumnas(ref this.tgGridAEAT);
                        else this.GridColumnasPagoRecibidas(ref this.tgGridAEAT);

                        this.tgGridAEAT.ClearSelection();
                        //Ajustar todas las columnas de la Grid
                        /*this.tgGridAEAT.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        int indice = this.tgGridAEAT.Columns.Count - 1;
                        for (int i = indice; i > 0; i--)
                        {
                            if (this.tgGridAEAT.Columns[i].Visible == true)
                            {
                                indice = i;
                                break;
                            }
                        }
                        this.tgGridAEAT.Columns[indice].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;*/

                        this.lblResultInfoAEAT.Visible = false;
                        this.tgGridAEAT.Visible = true;

                        this.lblTotalFactAEAT.Text = "Total Registros: " + this.tgGridAEAT.Rows.Count;
                        this.lblTotalFactAEAT.Visible = true;
                    }
                    else
                    {
                        this.lblResultInfoAEAT.Visible = true;
                        this.tgGridAEAT.Visible = false;
                        this.lblTotalFactAEAT.Visible = false;

                        if (errorAEAT != "") this.lblResultInfoAEAT.Text = errorAEAT;
                    }

                    if (existenDatosLocal)
                    {
                        //Ocultar y cambiar nombres de las columnas
                        if (this.libro != LibroUtiles.LibroID_PagosRecibidas) this.GridColumnas(ref this.tgGridLocal);
                        else this.GridColumnasPagoRecibidas(ref this.tgGridLocal);

                        this.tgGridLocal.ClearSelection();
                        this.lblResultInfoLocal.Visible = false;

                        this.tgGridLocal.ClearSelection();
                        /*
                        //Ajustar todas las columnas de la Grid
                        this.tgGridLocal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        int indice = this.tgGridLocal.Columns.Count - 1;
                        for (int i = indice; i > 0; i--)
                        {
                            if (this.tgGridLocal.Columns[i].Visible == true)
                            {
                                indice = i;
                                break;
                            }
                        }
                        this.tgGridLocal.Columns[indice].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        */
                        this.tgGridLocal.Visible = true;

                        this.lblTotalFactLocal.Text = "Total Registros: " + this.tgGridLocal.Rows.Count;
                        this.lblTotalFactLocal.Visible = true;
                    }
                    else
                    {
                        this.lblResultInfoLocal.Visible = true;
                        this.tgGridLocal.Visible = false;
                        this.lblTotalFactLocal.Visible = false;
                    }
                }
                else
                {
                    if (errorAEAT != "")
                    {
                        this.tgGridAEAT.Visible = false;
                        this.gbFacturasTodas.Visible = true;
                        this.lblResultInfoAEAT.Text = errorAEAT;
                        this.lblResultInfoAEAT.MaximumSize = new Size(400, 0);
                        this.lblResultInfoAEAT.AutoSize = true;
                        this.lblResultInfoAEAT.Visible = true;

                        this.tgGridLocal.Visible = false;
                        this.lblResultInfoLocal.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Error(Utiles.CreateExceptionString(ex));
                string error = ex.Message;
            }
        }

        /// <summary>
        /// Devuelve un DataTable con las facturas que tienen diferencias entre la AEAT y en Local
        /// </summary>
        /// <returns></returns>
        private DataTable ObtenerFacturasSoloDiferencia()
        {
            DataTable dtDatosGridDiff = null;
            try
            {
                dtDatosGridDiff = this.dtDatosAEATYLocal.Copy();
                if (this.libro != LibroUtiles.LibroID_PagosRecibidas)
                {
                    dtDatosGridDiff.Rows.Cast<DataRow>().Where(
                    row => (
                                row["EstadoSII"].ToString() == row["EstadoLocal"].ToString() &&
                                (row["ImporteSII"].ToString() == row["ImporteLocal"].ToString() ||
                                row["ImporteSII"].ToString().Trim() == "" && row["ImporteLocal"].ToString() == "0,00" ||
                                row["ImporteSII"].ToString().Trim() == "0,00" && row["ImporteLocal"].ToString() == ""
                                )
                                ) ||
                                (Convert.ToInt16(row["CantidadSII"]) == 0 && Convert.ToInt16(row["CantidadLocal"]) >= 1 && row["EstadoLocal"].ToString() == "PendienteEnvio")
                                ).ToList().ForEach(row => row.Delete());
                }
                else
                {
                    //Pagos Recibidas
                    dtDatosGridDiff.Rows.Cast<DataRow>().Where(
                    row => (
                                row["PagoFechaSII"].ToString() == row["PagoFechaLocal"].ToString() &&
                                row["pagoImporteSII"].ToString() == row["pagoImporteLocal"].ToString() &&
                                row["pagoMedioSII"].ToString() == row["pagoMedioLocal"].ToString() &&
                                row["PagoCuentaOMedioSII"].ToString() == row["pagoCuentaOMedioLocal"].ToString()
                                )).ToList().ForEach(row => row.Delete());
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosGridDiff);
        }


        /// <summary>
        /// Seleccionar datos de las facturas emitidas de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="addColumnVerMov"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalFacturasEmitidas(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, bool addColumnVerMov)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDDestinatario"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString().Replace('-', '/'),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = (e1["ImporteTotal"].ToString().Trim() == "") ? e1["ImporteTotal"].ToString() : Convert.ToDecimal(e1["ImporteTotal"], System.Globalization.CultureInfo.InvariantCulture).ToString("F2"),
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                if (addColumnVerMov)
                {
                    DataColumn columnaVerAEAT = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerAEAT.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovAEAT = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovAEAT.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosAEAT.Tables[0].Columns.Add(columnaVerAEAT);
                    dsDatosAEAT.Tables[0].Columns.Add(columnaMovAEAT);
                    //int columnCount = dsDatosAEAT.Tables[0].Columns.Count;
                    //columnaVerAEAT.SetOrdinal(columnCount);
                    //columnaMovAEAT.SetOrdinal(columnCount+1);
                }

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosFacturasEmitidas(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDDestinatario"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString(),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = e1["ImporteTotal"].ToString(),
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                if (addColumnVerMov)
                {
                    DataColumn columnaVerLocal = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerLocal.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovLocal = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovLocal.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosLocal.Tables[0].Columns.Add(columnaVerLocal);
                    dsDatosLocal.Tables[0].Columns.Add(columnaMovLocal);
                    //columnaVerLocal.SetOrdinal(columnCount);
                    //columnaMovLocal.SetOrdinal(columnCount + 1);
                }

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosFacturasEmitidas(ref this.tgGridLocal);

                /*
                DataTable dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                    {
                        IDNIF = x.IDNIF,
                        IDOTROCodigoPais = x.IDOTROCodigoPais,
                        IDOTROIdType = x.IDOTROIdType,
                        IDOTROId = x.IDOTROId,
                        NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                        FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                    }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = datosCompararAEAT.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Select(row => row.Estado).FirstOrDefault(),
                            EstadoLocal = datosCompararLocal.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Select(row => row.Estado).FirstOrDefault(),
                            ImporteSII = datosCompararAEAT.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Select(row => row.Importe).FirstOrDefault(),
                            ImporteLocal = datosCompararLocal.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Select(row => row.Importe).FirstOrDefault(),
                            CargoAbono = datosCompararLocal.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Select(row => row.CargoAbono).FirstOrDefault(),
                            CantidadSII = datosCompararAEAT.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Count(),
                            CantidadLocal = datosCompararLocal.AsEnumerable().Where(row => (row.IDNIF == x.Key.IDNIF
                                        && row.NumSerieFacturaEmisor == x.Key.NumSerieFacturaEmisor
                                        && row.FechaExpedicionFacturaEmisor == x.Key.FechaExpedicionFacturaEmisor)).Count(),
                            ActulizarEstado = 0
                        };
                    }
                    ).PropertiesToDataTable<Items>();
                */

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas recibidas de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="addColumnVerMov"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalFacturasRecibidas(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, bool addColumnVerMov)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString().Replace('-', '/'),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = (e1["ImporteTotal"].ToString().Trim() == "") ? e1["ImporteTotal"].ToString() : Convert.ToDecimal(e1["ImporteTotal"], System.Globalization.CultureInfo.InvariantCulture).ToString("F2"),
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);
                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;

                if (addColumnVerMov)
                {
                    DataColumn columnaVerAEAT = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerAEAT.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovAEAT = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovAEAT.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosAEAT.Tables[0].Columns.Add(columnaVerAEAT);
                    dsDatosAEAT.Tables[0].Columns.Add(columnaMovAEAT);
                }

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosFacturasRecibidas(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString(),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = e1["ImporteTotal"].ToString(),
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                if (addColumnVerMov)
                {
                    DataColumn columnaVerLocal = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerLocal.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovLocal = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovLocal.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosLocal.Tables[0].Columns.Add(columnaVerLocal);
                    dsDatosLocal.Tables[0].Columns.Add(columnaMovLocal);
                }

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosFacturasRecibidas(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de bienes de inversión de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="addColumnVerMov"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalBienesInversion(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, bool addColumnVerMov)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString().Replace('-', '/'),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = "",
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                if (addColumnVerMov)
                {
                    DataColumn columnaVerAEAT = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerAEAT.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovAEAT = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovAEAT.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosAEAT.Tables[0].Columns.Add(columnaVerAEAT);
                    dsDatosAEAT.Tables[0].Columns.Add(columnaMovAEAT);
                }

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosBienesInversion(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString(),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = "",
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                if (addColumnVerMov)
                {
                    DataColumn columnaVerLocal = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerLocal.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovLocal = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovLocal.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosLocal.Tables[0].Columns.Add(columnaVerLocal);
                    dsDatosLocal.Tables[0].Columns.Add(columnaMovLocal);
                }

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosBienesInversion(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de operaciones intracomunitarias de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="addColumnVerMov"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalDetOperacionesIntracomunitarias(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, bool addColumnVerMov)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString().Replace('-', '/'),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = "",
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                if (addColumnVerMov)
                {
                    DataColumn columnaVerAEAT = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerAEAT.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovAEAT = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovAEAT.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosAEAT.Tables[0].Columns.Add(columnaVerAEAT);
                    dsDatosAEAT.Tables[0].Columns.Add(columnaMovAEAT);
                }

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosDetOperacionesIntracomunitarias(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = e1["NumSerieFacturaEmisor"].ToString(),
                    FechaExpedicionFacturaEmisor = e1["FechaExpedicionFacturaEmisor"].ToString(),
                    Estado = e1["EstadoFactura"].ToString(),
                    Importe = "",
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                if (addColumnVerMov)
                {
                    DataColumn columnaVerLocal = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerLocal.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovLocal = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovLocal.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosLocal.Tables[0].Columns.Add(columnaVerLocal);
                    dsDatosLocal.Tables[0].Columns.Add(columnaMovLocal);
                }

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;

                this.CambiarColumnasEncabezadosDetOperacionesIntracomunitarias(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de cobros en metálico de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalCobrosMetalico(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = (e1["ImporteTotal"].ToString().Trim() == "") ? e1["ImporteTotal"].ToString() : Convert.ToDecimal(e1["ImporteTotal"], System.Globalization.CultureInfo.InvariantCulture).ToString("F2"),
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                /*
                if (addColumnVerMov)
                {
                    DataColumn columnaVerAEAT = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerAEAT.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovAEAT = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovAEAT.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosAEAT.Tables[0].Columns.Add(columnaVerAEAT);
                    dsDatosAEAT.Tables[0].Columns.Add(columnaMovAEAT);
                }
                */

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                /*int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;
                */

                this.CambiarColumnasEncabezadosCobrosMetalico(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = e1["ImporteTotal"].ToString(),
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                /*if (addColumnVerMov)
                {
                    DataColumn columnaVerLocal = new DataColumn("VER", typeof(System.Drawing.Bitmap));
                    columnaVerLocal.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                    DataColumn columnaMovLocal = new DataColumn("MOV", typeof(System.Drawing.Bitmap));
                    columnaMovLocal.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                    dsDatosLocal.Tables[0].Columns.Add(columnaVerLocal);
                    dsDatosLocal.Tables[0].Columns.Add(columnaMovLocal);
                }*/

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                /*
                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;
                */
                this.CambiarColumnasEncabezadosCobrosMetalico(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de agencias de viajes de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalAgenciasViaje(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = (e1["ImporteTotal"].ToString().Trim() == "") ? e1["ImporteTotal"].ToString() : Convert.ToDecimal(e1["ImporteTotal"], System.Globalization.CultureInfo.InvariantCulture).ToString("F2"),
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                this.CambiarColumnasEncabezadosAgenciasViaje(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = e1["ImporteTotal"].ToString(),
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = "",
                    ClaveOperacionDesc = ""
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                this.CambiarColumnasEncabezadosAgenciasViaje(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = "",
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de operaciones de seguros de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalOperacionesSeguros(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal)
        {
            DataTable dtDatosAEATYLocal = null;
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = (e1["ImporteTotal"].ToString().Trim() == "") ? e1["ImporteTotal"].ToString() : Convert.ToDecimal(e1["ImporteTotal"], System.Globalization.CultureInfo.InvariantCulture).ToString("F2"),
                    CargoAbono = "",
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = e1["ClaveOperacion"].ToString(),
                    ClaveOperacionDesc = e1["ClaveOperacionDesc"].ToString()
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);

                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                this.CambiarColumnasEncabezadosOperacionesSeguros(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    IDNIF = e1["IDNIF"].ToString(),
                    IDOTROCodigoPais = e1["IDOTROCodigoPais"].ToString(),
                    IDOTROIdType = e1["IDOTROIdType"].ToString(),
                    IDOTROId = e1["IDOTROId"].ToString(),
                    IDDestinatario_Emisor = e1["IDEmisorFactura"].ToString(),
                    NumSerieFacturaEmisor = "",
                    FechaExpedicionFacturaEmisor = "",
                    Estado = e1["EstadoRegistro"].ToString(),
                    Importe = e1["ImporteTotal"].ToString(),
                    CargoAbono = e1["CargoAbono"].ToString(),
                    Cantidad = 1,
                    CodError = e1["CodigoErrorRegistro"].ToString(),
                    MensajeError = e1["DescripcionErrorRegistro"].ToString(),
                    ClaveOperacion = e1["ClaveOperacion"].ToString(),
                    ClaveOperacionDesc = e1["ClaveOperacionDesc"].ToString()
                }).OrderBy(id => id.IDNIF).ThenBy(n => n.NumSerieFacturaEmisor).ThenBy(f => f.FechaExpedicionFacturaEmisor).ThenBy(c => c.CargoAbono);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);
                //this.tgGridLocal.DataSource = dtDatosCompararLocal;

                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                this.CambiarColumnasEncabezadosOperacionesSeguros(ref this.tgGridLocal);

                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    IDNIF = x.IDNIF,
                    IDOTROCodigoPais = x.IDOTROCodigoPais,
                    IDOTROIdType = x.IDOTROIdType,
                    IDOTROId = x.IDOTROId,
                    IDDestinatario_Emisor = x.IDDestinatario_Emisor,
                    NumSerieFacturaEmisor = x.NumSerieFacturaEmisor,
                    FechaExpedicionFacturaEmisor = x.FechaExpedicionFacturaEmisor,
                    ClaveOperacion = x.ClaveOperacion
                }
                    ).Select(x =>
                    {
                        return new Items
                        {
                            IDNIF = x.Key.IDNIF,
                            IDOTROCodigoPais = x.Key.IDOTROCodigoPais,
                            IDOTROIdType = x.Key.IDOTROIdType,
                            IDOTROId = x.Key.IDOTROId,
                            IDDestinatario_Emisor = x.Key.IDDestinatario_Emisor,
                            NumSerieFacturaEmisor = x.Key.NumSerieFacturaEmisor,
                            FechaExpedicionFacturaEmisor = x.Key.FechaExpedicionFacturaEmisor,
                            EstadoSII = " ",
                            EstadoLocal = " ",
                            ImporteSII = " ",
                            ImporteLocal = " ",
                            CargoAbono = " ",
                            CantidadSII = 0,
                            CantidadLocal = 0,
                            ActulizarEstado = 0,
                            CodError = " ",
                            MensajeError = " ",
                            ClaveOperacion = x.Key.ClaveOperacion,
                            ClaveOperacionDesc = ""
                        };
                    }
                    ).PropertiesToDataTable<Items>();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Seleccionar datos de las facturas de pagos recibidas de hacienda y de datos en local que serán los que se utilicen para comparar los descuadres
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <returns></returns>
        private DataTable ObtenerDatosAEATYLocalPagosRecibidas(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal)
        {
            DataTable dtDatosAEATYLocal = null;

            //importeTotalActualDec.ToString("N2", this.LP.MyCultureInfo)
            try
            {
                //Seleccionar datos de las facturas de hacienda que serán los que se utilicen para comparar los descuadres
                var datosCompararAEAT = dsDatosAEAT.Tables[0].AsEnumerable().Select(e1 => new
                {
                    PagoFecha = e1["PagoFecha"].ToString().Replace('-','/'),
                    PagoImporte = (e1["PagoImporte"].ToString().Trim() == "") ? e1["PagoImporte"].ToString() : Convert.ToDecimal(e1["PagoImporte"], System.Globalization.CultureInfo.InvariantCulture).ToString("N2"),
                    PagoMedio = e1["PagoMedio"].ToString(),
                    PagoMedioDesc = e1["PagoMedioDesc"].ToString(),
                    PagoCuentaOMedio = e1["PagoCuentaOMedio"].ToString()
                }).OrderBy(id => id.PagoFecha);
                dtDatosCompararAEAT = LINQResultToDataTable(datosCompararAEAT);
               
                //this.tgGridAEAT.DataSource = dtDatosCompararAEAT;
                this.tgGridAEAT.dsDatos = new DataSet();
                this.tgGridAEAT.dsDatos = dsDatosAEAT;
                this.tgGridAEAT.DataSource = dsDatosAEAT.Tables[0];

                /*int totalColumnasGrid = this.tgGridAEAT.Columns.Count;
                this.tgGridAEAT.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridAEAT.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;
                */

                this.CambiarColumnasEncabezadosPagosRecibidas(ref this.tgGridAEAT);

                //Seleccionar datos de las facturas en local que serán los que se utilicen para comparar los descuadres
                var datosCompararLocal = dsDatosLocal.Tables[0].AsEnumerable().Select(e1 => new
                {
                    PagoFecha = e1["PagoFecha"].ToString(),
                    PagoImporte = e1["PagoImporte"].ToString(),
                    PagoMedio = e1["PagoMedio"].ToString(),
                    PagoMedioDesc = e1["PagoMedioDesc"].ToString(),
                    PagoCuentaOMedio = e1["PagoCuentaOMedio"].ToString()
                }).OrderBy(id => id.PagoFecha);
                dtDatosCompararLocal = LINQResultToDataTable(datosCompararLocal);

                //this.tgGridLocal.DataSource = dtDatosCompararLocal;
                this.tgGridLocal.dsDatos = new DataSet();
                this.tgGridLocal.dsDatos = dsDatosLocal;
                this.tgGridLocal.DataSource = dsDatosLocal.Tables[0];

                /*
                totalColumnasGrid = this.tgGridLocal.Columns.Count;
                this.tgGridLocal.Columns["VER"].DisplayIndex = totalColumnasGrid - 2;
                this.tgGridLocal.Columns["MOV"].DisplayIndex = totalColumnasGrid - 1;
                */
                this.CambiarColumnasEncabezadosPagosRecibidas(ref this.tgGridLocal);

                /*
                dtDatosAEATYLocal = datosCompararAEAT.AsEnumerable().Union(datosCompararLocal.AsEnumerable())
                .GroupBy(x => new
                {
                    PagoFechaSII = x.PagoFecha,
                    PagoImporteSII = x.PagoImporte,
                    PagoMedioSII = x.PagoMedioDesc,
                    PagoCuentaOMedioSII = x.PagoCuentaOMedio
                }
                    ).Select(x =>
                    {
                        return new ItemsPagoRecibidas
                        {
                            PagoFechaSII = x.Key.PagoFechaSII,
                            PagoImporteSII = x.Key.PagoImporteSII,
                            PagoMedioSII = x.Key.PagoMedioSII,
                            PagoCuentaOMedioSII = x.Key.PagoCuentaOMedioSII,
                            PagoFechaLocal = "",
                            PagoImporteLocal = "",
                            PagoMedioLocal = "",
                            PagoCuentaOMedioLocal = ""
                        };
                    }
                    ).PropertiesToDataTable<ItemsPagoRecibidas>();
                 */ 
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (dtDatosAEATYLocal);
        }

        /// <summary>
        /// Realiza la consulta de hacienda
        /// </summary>
        /// <returns></returns>
        private DataSet ObtenerDatosAEAT(string compania, string ejercicio, string periodo)
        {
            DataSet dsConsultaRespuesta = null;

            string nif_id = this.txtNIF.Text.Trim();
            string codPais = this.cmbPais.SelectedValue.ToString().Trim();
            string tipoIdent = this.cmbTipoIdentif.SelectedValue.ToString().Trim();
            string numFactura = this.txtNumSerieFactura.Text.Trim();
            string fecha = this.consultaFiltroFecha.Trim();
            string nombreRazonSocial = this.txtNombreRazonSocial.Text.Trim();
            
            try
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRFacturasEmitidas(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              nombreRazonSocial, this.txtNumSerieFactura.Text,
                                                                              this.consultaFiltroFecha, "",
                                                                              "", "",
                                                                              "",
                                                                              "");
                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRFacturasRecibidas(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              nombreRazonSocial, this.txtNumSerieFactura.Text,
                                                                              this.consultaFiltroFecha, "",
                                                                              "", "",
                                                                              "",
                                                                              "");
                        break;
                    case LibroUtiles.LibroID_BienesInversion:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRBienesInversion(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              nombreRazonSocial, this.txtNumSerieFactura.Text,
                                                                              this.consultaFiltroFecha, "",
                                                                              "", "", "", "");
                        break;
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRDetOperIntracomunitarias(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              nombreRazonSocial, this.txtNumSerieFactura.Text,
                                                                              this.consultaFiltroFecha, "",
                                                                              "", "", "");
                        break;
                    case LibroUtiles.LibroID_CobrosMetalico:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRCobrosMetalico(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              "", "",
                                                                              "", "",
                                                                              "");
                        break;
                    case LibroUtiles.LibroID_AgenciasViajes:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRAgenciasViajes(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              "", "",
                                                                              "", "",
                                                                              "");
                        break;
                    case LibroUtiles.LibroID_OperacionesSeguros:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLROperacionesSeguros(compania, ejercicio, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              "", "",
                                                                              "", "",
                                                                              "", "");
                        break;
                    case LibroUtiles.LibroID_PagosRecibidas:
                        dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRPagosRecibidas(compania, this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              nombreRazonSocial, this.txtNumSerieFactura.Text, 
                                                                              this.consultaFiltroFecha);
                        break;
                }

                //Chequear si hay error en la respuesta de la AEAT
                if (dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (dsConsultaRespuesta.Tables.Contains("DatosGenerales") && dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count == 0)
                    {
                        if (dsConsultaRespuesta.Tables.Contains("Resultado") && dsConsultaRespuesta.Tables["Resultado"].Rows.Count > 0)
                        {
                            this.errorAEAT = dsConsultaRespuesta.Tables["Resultado"].Rows[0]["Estado"].ToString();
                        }
                    }
                }
            }
            catch { }

            return (dsConsultaRespuesta);
        }

        /// <summary>
        /// Realiza la consulta de datos en local
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ObtenerDatosLocal(string compania, string ejercicio, string periodo, string agencia)
        {
            DataSet dsConsultaRespuesta = null;
            try
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:
                        LibroFacturasExpedidas libroFactExpedidas = new LibroFacturasExpedidas(Log, utiles, LP, agencia);
                        dsConsultaRespuesta = libroFactExpedidas.ObtenerDatosFacturasEmitidas(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "",
                                                                                              this.txtNumSerieFactura.Text,
                                                                                              "", "",
                                                                                              "", this.agencia);

                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:
                        LibroFacturasRecibidas libroFactRecibidas = new LibroFacturasRecibidas(Log, utiles, LP, this.agencia);
                        dsConsultaRespuesta = libroFactRecibidas.ObtenerDatosFacturasRecibidas(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "",
                                                                                              this.txtNumSerieFactura.Text,
                                                                                              "", "",
                                                                                              "", this.agencia);
                        break;
                    case LibroUtiles.LibroID_BienesInversion:
                        LibroBienesInversion libroBienesInversion = new LibroBienesInversion(Log, utiles, LP);
                        dsConsultaRespuesta = libroBienesInversion.ObtenerDatosBienesInversion(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "",
                                                                                              this.txtNumSerieFactura.Text,
                                                                                              "", "",
                                                                                              "", this.agencia);
                        break;
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        LibroDetOperacIntracomunitarias libroOpIntracomunitarias = new LibroDetOperacIntracomunitarias(Log, utiles, LP);
                        dsConsultaRespuesta = libroOpIntracomunitarias.ObtenerDatosOperacIntracomunitarias(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "",
                                                                                              this.txtNumSerieFactura.Text,
                                                                                              "", "",
                                                                                              "", this.agencia);
                        break;
                    case LibroUtiles.LibroID_CobrosMetalico:
                        LibroCobrosMetalico libroCobrosMetalico = new LibroCobrosMetalico(Log, utiles, LP);
                        dsConsultaRespuesta = libroCobrosMetalico.ObtenerDatosCobrosMetalico(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "", "",
                                                                                              this.agencia);
                        break;
                    case LibroUtiles.LibroID_AgenciasViajes:
                        LibroAgenciasViajes libroAgenciasViaje = new LibroAgenciasViajes(Log, utiles, LP);
                        dsConsultaRespuesta = libroAgenciasViaje.ObtenerDatosAgenciasViajes(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "", "",
                                                                                              this.agencia);
                        break;
                    case LibroUtiles.LibroID_OperacionesSeguros:
                        LibroOperacionesSeguros libroOperacionesSeguros = new LibroOperacionesSeguros(Log, utiles, LP);
                        dsConsultaRespuesta = libroOperacionesSeguros.ObtenerDatosOperacionesSeguros(compania, ejercicio, periodo,
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(), "", "", "",
                                                                                              this.agencia);
                        break;
                    case LibroUtiles.LibroID_PagosRecibidas:
                        LibroPagoRecibidas libroOperacionesPagosRecibidas = new LibroPagoRecibidas(Log, utiles, LP);
                        dsConsultaRespuesta = libroOperacionesPagosRecibidas.ObtenerDatosPagosRecibidas(compania, ejercicio, periodo, "",
                                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                                              this.txtNumSerieFactura.Text, "", "",
                                                                                              this.agencia);
                        break;
                }
            }
            catch { }

            return (dsConsultaRespuesta);

        }

        /// <summary>
        /// Buscar la info manual de la comparación entre datos de la aeat y en local (importes, estados, cantidades en local pueden ser mas de 1 por cargo/abono)
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="dtDatosAEATYLocal"></param>
        private void ComparaDataTable(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, ref DataTable dtDatosAEATYLocal)
        {
            try
            {
                string nifValor = "";
                string IDOTROCodigoPaisValor = "";
                string IDOTROIdTypeValor = "";
                string IDOTROIdValor = "";
                string numSerieFacturaEmisorValor = "";
                string fechaExpedicionFacturaEmisorValor = "";
                string claveOperacion = "";

                int indice;
                decimal importe = 0;
                string importeStr = "";

                for (int i = 0; i < dtDatosAEATYLocal.Rows.Count; i++)
                {
                    try
                    {
                        nifValor = dtDatosAEATYLocal.Rows[i]["IDNIF"].ToString();

                        IDOTROCodigoPaisValor = dtDatosAEATYLocal.Rows[i]["IDOTROCodigoPais"].ToString();
                        IDOTROIdTypeValor = dtDatosAEATYLocal.Rows[i]["IDOTROIdType"].ToString();
                        IDOTROIdValor = dtDatosAEATYLocal.Rows[i]["IDOTROId"].ToString();
                        numSerieFacturaEmisorValor = dtDatosAEATYLocal.Rows[i]["NumSerieFacturaEmisor"].ToString();
                        fechaExpedicionFacturaEmisorValor = dtDatosAEATYLocal.Rows[i]["FechaExpedicionFacturaEmisor"].ToString();
                        claveOperacion = dtDatosAEATYLocal.Rows[i]["ClaveOperacion"].ToString();

                        try
                        {
                            //Datos AEAT
                            var rowAEAT = dtDatosCompararAEAT.AsEnumerable().Cast<DataRow>().Where(row =>
                                                                                                      row["IDNIF"].ToString() == nifValor &&
                                                                                                      row["IDOTROCodigoPais"].ToString() == IDOTROCodigoPaisValor &&
                                                                                                      row["IDOTROIdType"].ToString() == IDOTROIdTypeValor &&
                                                                                                      row["IDOTROId"].ToString() == IDOTROIdValor &&
                                                                                                      row["NumSerieFacturaEmisor"].ToString() == numSerieFacturaEmisorValor &&
                                                                                                      row["FechaExpedicionFacturaEmisor"].ToString() == fechaExpedicionFacturaEmisorValor &&
                                                                                                      row["ClaveOperacion"].ToString() == claveOperacion).ToArray();

                            if (rowAEAT.Length == 0)
                            {
                                dtDatosAEATYLocal.Rows[i]["EstadoSII"] = "";
                                dtDatosAEATYLocal.Rows[i]["ImporteSII"] = "";
                                dtDatosAEATYLocal.Rows[i]["CantidadSII"] = 0;
                                dtDatosAEATYLocal.Rows[i]["CodError"] = "";
                                dtDatosAEATYLocal.Rows[i]["MensajeError"] = "";
                                dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"] = "";
                            }
                            else
                            {
                                foreach (DataRow dr in rowAEAT)
                                {
                                    indice = dtDatosCompararAEAT.Rows.IndexOf(dr);
                                    dtDatosAEATYLocal.Rows[i]["EstadoSII"] = dtDatosCompararAEAT.Rows[indice]["Estado"].ToString();

                                    importeStr = dtDatosCompararAEAT.Rows[indice]["Importe"].ToString();
                                    try
                                    {
                                        if (importeStr != "")
                                        {
                                            importe = Convert.ToDecimal(importeStr);
                                            importeStr = importe.ToString("N2");
                                        }
                                    }
                                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

                                    dtDatosAEATYLocal.Rows[i]["ImporteSII"] = importeStr;
                                    dtDatosAEATYLocal.Rows[i]["CantidadSII"] = 1;
                                    dtDatosAEATYLocal.Rows[i]["CodError"] = dtDatosCompararAEAT.Rows[indice]["CodError"].ToString();
                                    dtDatosAEATYLocal.Rows[i]["MensajeError"] = dtDatosCompararAEAT.Rows[indice]["MensajeError"].ToString();
                                    dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"] = dtDatosCompararAEAT.Rows[indice]["ClaveOperacionDesc"].ToString();
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            //Datos LOCAL
                            var rowLocal = dtDatosCompararLocal.AsEnumerable().Cast<DataRow>().Where(row =>
                                                                                                      row["IDNIF"].ToString() == nifValor &&
                                                                                                      row["IDOTROCodigoPais"].ToString() == IDOTROCodigoPaisValor &&
                                                                                                      row["IDOTROIdType"].ToString() == IDOTROIdTypeValor &&
                                                                                                      row["IDOTROId"].ToString() == IDOTROIdValor &&
                                                                                                      row["NumSerieFacturaEmisor"].ToString() == numSerieFacturaEmisorValor &&
                                                                                                      row["FechaExpedicionFacturaEmisor"].ToString() == fechaExpedicionFacturaEmisorValor &&
                                                                                                      row["ClaveOperacion"].ToString() == claveOperacion).ToArray();

                            switch (rowLocal.Length)
                            {
                                case 0:
                                    dtDatosAEATYLocal.Rows[i]["EstadoLocal"] = "";
                                    dtDatosAEATYLocal.Rows[i]["ImporteLocal"] = "";
                                    dtDatosAEATYLocal.Rows[i]["CantidadLocal"] = 0;

                                    break;
                                case 1:
                                    foreach (DataRow dr in rowLocal)
                                    {
                                        indice = dtDatosCompararLocal.Rows.IndexOf(dr);
                                        dtDatosAEATYLocal.Rows[i]["EstadoLocal"] = dtDatosCompararLocal.Rows[indice]["Estado"].ToString();
                                        dtDatosAEATYLocal.Rows[i]["ImporteLocal"] = dtDatosCompararLocal.Rows[indice]["Importe"].ToString();
                                        dtDatosAEATYLocal.Rows[i]["CargoAbono"] = dtDatosCompararLocal.Rows[indice]["CargoAbono"].ToString();
                                        dtDatosAEATYLocal.Rows[i]["CantidadLocal"] = rowLocal.Length;

                                        if (dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"] = dtDatosCompararLocal.Rows[indice]["ClaveOperacionDesc"].ToString();
                                        
                                        if (dtDatosAEATYLocal.Rows[i]["CodError"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["CodError"] = dtDatosCompararLocal.Rows[indice]["CodError"].ToString();
                                        if (dtDatosAEATYLocal.Rows[i]["MensajeError"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["MensajeError"] = dtDatosCompararLocal.Rows[indice]["MensajeError"].ToString();
                                    }
                                    break;
                                default:
                                    foreach (DataRow dr in rowLocal)
                                    {
                                        //FALTA analizar cargo/abono
                                        indice = dtDatosCompararLocal.Rows.IndexOf(dr);
                                        dtDatosAEATYLocal.Rows[i]["EstadoLocal"] = dtDatosCompararLocal.Rows[indice]["Estado"].ToString();
                                        dtDatosAEATYLocal.Rows[i]["ImporteLocal"] = dtDatosCompararLocal.Rows[indice]["Importe"].ToString();
                                        dtDatosAEATYLocal.Rows[i]["CargoAbono"] = dtDatosCompararLocal.Rows[indice]["CargoAbono"].ToString();

                                        if (dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["ClaveOperacionDesc"] = dtDatosCompararLocal.Rows[indice]["ClaveOperacionDesc"].ToString();

                                        if (dtDatosAEATYLocal.Rows[i]["CodError"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["CodError"] = dtDatosCompararLocal.Rows[indice]["CodError"].ToString();
                                        if (dtDatosAEATYLocal.Rows[i]["MensajeError"].ToString().Trim() == "") dtDatosAEATYLocal.Rows[i]["MensajeError"] = dtDatosCompararLocal.Rows[indice]["MensajeError"].ToString();
                                        break;
                                    }
                                    dtDatosAEATYLocal.Rows[i]["CantidadLocal"] = rowLocal.Length;
                                    break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                    }

                    if (dtDatosAEATYLocal.Rows[i]["EstadoSII"].ToString() != dtDatosAEATYLocal.Rows[i]["EstadoLocal"].ToString() &&
                        Convert.ToInt16(dtDatosAEATYLocal.Rows[i]["CantidadSII"]) == 1 && Convert.ToInt16(dtDatosAEATYLocal.Rows[i]["CantidadLocal"]) >= 1)
                    {
                        dtDatosAEATYLocal.Rows[i]["ActulizarEstado"] = 1;
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// Buscar la info manual de la comparación entre datos de la aeat y en local del libro de pagos recibidas)
        /// </summary>
        /// <param name="dtDatosCompararAEAT"></param>
        /// <param name="dtDatosCompararLocal"></param>
        /// <param name="dtDatosAEATYLocal"></param>
        private void ComparaDataTablePagosRecibidas(ref DataTable dtDatosCompararAEAT, ref DataTable dtDatosCompararLocal, ref DataTable dtDatosAEATYLocal)
        {
            try
            {
                dtDatosAEATYLocal = new DataTable();
                dtDatosAEATYLocal.TableName = "DatosAEATYLocal";
                dtDatosAEATYLocal.Columns.Add("PagoFechaSII", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoImporteSII", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoMedioSII", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoCuentaOMedioSII", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoFechaLocal", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoImporteLocal", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoMedioLocal", typeof(string));
                dtDatosAEATYLocal.Columns.Add("PagoCuentaOMedioLocal", typeof(string));

                string pagoFechaSII = "";
                string pagoImporteSII = "";
                string pagoMedioSII = "";
                string pagoCuentaOMedioSII = "";
                string pagoFechaLocal = "";
                string pagoImporteLocal = "";
                string pagoMedioLocal = "";
                string pagoCuentaOMedioLocal = "";
                bool existePago = false;
                ArrayList indiceExisteLocal = new ArrayList();
                DataRow row;

                if (dtDatosCompararAEAT != null && dtDatosCompararAEAT.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDatosCompararAEAT.Rows.Count; i++)
                    {
                        row = dtDatosAEATYLocal.NewRow();

                        pagoFechaSII = dtDatosCompararAEAT.Rows[i]["PagoFecha"].ToString().Trim();
                        pagoImporteSII = dtDatosCompararAEAT.Rows[i]["PagoImporte"].ToString().Trim();
                        pagoMedioSII = dtDatosCompararAEAT.Rows[i]["PagoMedioDesc"].ToString().Trim();
                        pagoCuentaOMedioSII = dtDatosCompararAEAT.Rows[i]["PagoCuentaOMedio"].ToString().Trim();

                        row["PagoFechaSII"] = pagoFechaSII;
                        row["PagoImporteSII"] = pagoImporteSII;
                        row["PagoMedioSII"] = pagoMedioSII;
                        row["PagoCuentaOMedioSII"] = pagoCuentaOMedioSII;

                        if (dtDatosCompararLocal != null && dtDatosCompararLocal.Rows.Count > 0)
                        {
                            existePago = false;

                            pagoFechaLocal = "";
                            pagoImporteLocal = "";
                            pagoMedioLocal = "";
                            pagoCuentaOMedioLocal = "";

                            for (int j = 0; j < dtDatosCompararLocal.Rows.Count; j++)
                            {
                                pagoFechaLocal = dtDatosCompararLocal.Rows[i]["PagoFecha"].ToString().Trim();
                                pagoImporteLocal = dtDatosCompararLocal.Rows[i]["PagoImporte"].ToString().Trim();
                                pagoMedioLocal = dtDatosCompararLocal.Rows[i]["PagoMedioDesc"].ToString().Trim();
                                pagoCuentaOMedioLocal = dtDatosCompararLocal.Rows[i]["PagoCuentaOMedio"].ToString().Trim();

                                if (pagoFechaSII == pagoFechaLocal && pagoImporteSII == pagoImporteLocal &&
                                    pagoMedioSII == pagoMedioLocal && pagoCuentaOMedioSII == pagoCuentaOMedioLocal)
                                {
                                    existePago = true;
                                    //Insertar en el array de indices tratados
                                    indiceExisteLocal.Add(j);
                                    break;
                                }
                            }

                            if (existePago)
                            {
                                row["PagoFechaLocal"] = pagoFechaLocal;
                                row["PagoImporteLocal"] = pagoImporteLocal;
                                row["PagoMedioLocal"] = pagoMedioLocal;
                                row["PagoCuentaOMedioLocal"] = pagoCuentaOMedioLocal;
                            }
                            else
                            {
                                row["PagoFechaLocal"] = "";
                                row["PagoImporteLocal"] = "";
                                row["PagoMedioLocal"] = "";
                                row["PagoCuentaOMedioLocal"] = "";
                            }

                        }
                        else
                        {
                            row["PagoFechaLocal"] = "";
                            row["PagoImporteLocal"] = "";
                            row["PagoMedioLocal"] = "";
                            row["PagoCuentaOMedioLocal"] = "";
                        }

                        dtDatosAEATYLocal.Rows.Add(row);
                    }

                    //Verificar si existen pagos en local que no hayan sido analizados
                    if (dtDatosCompararLocal != null && dtDatosCompararLocal.Rows.Count > 0 && dtDatosCompararLocal.Rows.Count != indiceExisteLocal.Count)
                    {
                        for (int i = 0; i < dtDatosCompararLocal.Rows.Count; i++)
                        {
                            if (!indiceExisteLocal.Contains(i))
                            {
                                row = dtDatosAEATYLocal.NewRow();

                                pagoFechaLocal = dtDatosCompararLocal.Rows[i]["PagoFecha"].ToString().Trim();
                                pagoImporteLocal = dtDatosCompararLocal.Rows[i]["PagoImporte"].ToString().Trim();
                                pagoMedioLocal = dtDatosCompararLocal.Rows[i]["PagoMedioDesc"].ToString().Trim();
                                pagoCuentaOMedioLocal = dtDatosCompararLocal.Rows[i]["PagoCuentaOMedio"].ToString().Trim();

                                row["PagoFechaLocal"] = pagoFechaLocal;
                                row["PagoImporteLocal"] = pagoImporteLocal;
                                row["PagoMedioLocal"] = pagoMedioLocal;
                                row["PagoCuentaOMedioLocal"] = pagoCuentaOMedioLocal;

                                row["PagoFechaSII"] = "";
                                row["PagoImporteSII"] = "";
                                row["PagoMedioSII"] = "";
                                row["PagoCuentaOMedioSII"] = "";

                                dtDatosAEATYLocal.Rows.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    if (dtDatosCompararLocal != null && dtDatosCompararLocal.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDatosCompararLocal.Rows.Count; i++)
                        {
                            row = dtDatosAEATYLocal.NewRow();

                            pagoFechaLocal = dtDatosCompararLocal.Rows[i]["PagoFecha"].ToString().Trim();
                            pagoImporteLocal = dtDatosCompararLocal.Rows[i]["PagoImporte"].ToString().Trim();
                            pagoMedioLocal = dtDatosCompararLocal.Rows[i]["PagoMedioDesc"].ToString().Trim();
                            pagoCuentaOMedioLocal = dtDatosCompararLocal.Rows[i]["PagoCuentaOMedio"].ToString().Trim();

                            row["PagoFechaLocal"] = pagoFechaLocal;
                            row["PagoImporteLocal"] = pagoImporteLocal;
                            row["PagoMedioLocal"] = pagoMedioLocal;
                            row["PagoCuentaOMedioLocal"] = pagoCuentaOMedioLocal;

                            row["PagoFechaSII"] = "";
                            row["PagoImporteSII"] = "";
                            row["PagoMedioSII"] = "";
                            row["PagoCuentaOMedioSII"] = "";

                            dtDatosAEATYLocal.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// LINQ to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Linqlist"></param>
        /// <returns></returns>
        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Cambia el Texto de las columnas y oculta las que sean necesarias (de la grid de comparación, grid AEAT y grid Local)
        /// </summary>
        /// <param name="grid"></param>
        private void GridColumnas(ref ObjectModel.TGGrid grid)
        {
            bool libroFacturasExpedidas = (this.libro == LibroUtiles.LibroID_FacturasEmitidas);

            if (grid.Columns.Contains("Compania")) grid.Columns["Compania"].Visible = false;
            if (grid.Columns.Contains("Ejercicio")) grid.Columns["Ejercicio"].Visible = false;
            if (grid.Columns.Contains("Periodo")) grid.Columns["Periodo"].Visible = false;

            if (grid.Columns.Contains("IDNIF")) grid.Columns["IDNIF"].Visible = false;            
            if (grid.Columns.Contains("IDOTROCodigoPais")) grid.Columns["IDOTROCodigoPais"].Visible = false;
            if (grid.Columns.Contains("IDOTROIdType")) grid.Columns["IDOTROIdType"].Visible = false;
            if (grid.Columns.Contains("IDOTROId")) grid.Columns["IDOTROId"].Visible = false;

            if (grid.Columns.Contains("IDDestinatario_Emisor")) grid.CambiarColumnHeader("IDDestinatario_Emisor", this.lblNIF.Text);  //Falta traducir

            if (grid.Columns.Contains("NumSerieFacturaEmisor"))
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_CobrosMetalico:
                    case LibroUtiles.LibroID_OperacionesSeguros:
                    case LibroUtiles.LibroID_AgenciasViajes:
                        grid.Columns["NumSerieFacturaEmisor"].Visible = false;
                        break;
                    default:
                        grid.CambiarColumnHeader("NumSerieFacturaEmisor", "Num. Factura");  //Falta traducir
                        grid.Columns["NumSerieFacturaEmisor"].Visible = true;
                        break;
                }
            }

            if (grid.Columns.Contains("FechaExpedicionFacturaEmisor"))
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_CobrosMetalico:
                    case LibroUtiles.LibroID_OperacionesSeguros:
                    case LibroUtiles.LibroID_AgenciasViajes:
                        grid.Columns["FechaExpedicionFacturaEmisor"].Visible = false;
                        break;
                    default:
                        grid.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Factura");  //Falta traducir
                        grid.Columns["FechaExpedicionFacturaEmisor"].Visible = true;
                        break;
                }
            }

            if (grid.Columns.Contains("EstadoSII")) grid.CambiarColumnHeader("EstadoSII", "Estado SII");  //Falta traducir
            if (grid.Columns.Contains("EstadoLocal")) grid.CambiarColumnHeader("EstadoLocal", "Estado Local");  //Falta traducir

            if (grid.Columns.Contains("TipoFactura")) grid.Columns["TipoFactura"].Visible = false;
            if (grid.Columns.Contains("ClaveRegimenEspecialOTrascendencia")) grid.Columns["ClaveRegimenEspecialOTrascendencia"].Visible = false;
            if (grid.Columns.Contains("TipoFactura")) grid.Columns["TipoFactura"].Visible = false;

            if (grid.Columns.Contains("ImporteSII"))
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_BienesInversion:
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        grid.Columns["ImporteSII"].Visible = false;
                        break;
                    default:
                        grid.CambiarColumnHeader("ImporteSII", "Importe SII");  //Falta traducir
                        //Alinear a la derecha la columnas de importes
                        grid.Columns["ImporteSII"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        grid.Columns["ImporteSII"].Visible = true;
                        break;
                }
            }

            if (grid.Columns.Contains("ImporteLocal"))
            {
                switch (this.libro)
                {
                    case LibroUtiles.LibroID_BienesInversion:
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        grid.Columns["ImporteLocal"].Visible = false;
                        break;
                    default:
                        grid.CambiarColumnHeader("ImporteLocal", "Importe Local");  //Falta traducir
                        //Alinear a la derecha la columnas de importes
                        grid.Columns["ImporteLocal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        grid.Columns["ImporteLocal"].Visible = true;
                        break;
                }
            }

            if (grid.Columns.Contains("CargoAbono")) grid.CambiarColumnHeader("CargoAbono", "Cargo/Abono");  //Falta traducir

            if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
            if (grid.Columns.Contains("ClaveOperacionDesc")) grid.CambiarColumnHeader("ClaveOperacionDesc", "Clave Operación");  //Falta traducir

            if (grid.Columns.Contains("CantidadSII"))
            {
                //grid.CambiarColumnHeader("CantidadSII", "No. Registros SII");  //Falta traducir
                grid.Columns["CantidadSII"].Visible = false;
            }
            if (grid.Columns.Contains("CantidadLocal"))
            {
                //grid.CambiarColumnHeader("CantidadLocal", "No. Registros Local");  //Falta traducir
                grid.Columns["CantidadLocal"].Visible = false;
            }
            if (grid.Columns.Contains("Cantidad"))
            {
                grid.Columns["Cantidad"].Visible = false;
            }
            if (grid.Columns.Contains("ActulizarEstado"))
            {
                //grid.CambiarColumnHeader("ActulizarEstado", "Actualizar Estado Local");  //Falta traducir
                grid.Columns["ActulizarEstado"].Visible = false;
            }
            if (grid.Columns.Contains("CodError")) grid.CambiarColumnHeader("CodError", "Código Error");  //Falta traducir
            if (grid.Columns.Contains("MensajeError")) grid.CambiarColumnHeader("MensajeError", "Mensaje Error");  //Falta traducir
        }


        /// <summary>
        /// Cambia el Texto de las columnas y oculta las que sean necesarias para los pagos recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void GridColumnasPagoRecibidas(ref ObjectModel.TGGrid grid)
        {
            if (grid.Columns.Contains("TimestampPresentacion")) grid.CambiarColumnHeader("TimestampPresentacion", "Fecha/Hora presentación");  //Falta traducir
            if (grid.Columns.Contains("PagoFecha")) grid.CambiarColumnHeader("PagoFecha", "Fecha de Pago");  //Falta traducir
            if (grid.Columns.Contains("PagoImporte")) grid.CambiarColumnHeader("PagoImporte", "Importe de Pago");  //Falta traducir
            if (grid.Columns.Contains("PagoMedio")) grid.Columns["PagoMedio"].Visible = false;
            if (grid.Columns.Contains("PagoMedioDesc")) grid.CambiarColumnHeader("PagoMedioDesc", "Medio de Pago");  //Falta traducir
            if (grid.Columns.Contains("PagoCuentaOMedio")) grid.CambiarColumnHeader("PagoCuentaOMedio", "Cuenta o Medio de Pago");  //Falta traducir
            if (grid.Columns.Contains("NIFPresentador")) grid.CambiarColumnHeader("NIFPresentador", "NIF presentador");  //Falta traducir
            if (grid.Columns.Contains("TimestampPresentacion")) grid.CambiarColumnHeader("TimestampPresentacion", "Fecha/Hora presentación");  //Falta traducir
            if (grid.Columns.Contains("IDNIF")) grid.Columns["IDNIF"].Visible = false;
            if (grid.Columns.Contains("IDOTROCodigoPais")) grid.Columns["IDOTROCodigoPais"].Visible = false;
            if (grid.Columns.Contains("IDOTROIdType")) grid.Columns["IDOTROIdType"].Visible = false;
            if (grid.Columns.Contains("IDOTROId")) grid.Columns["IDOTROId"].Visible = false;
            if (grid.Columns.Contains("IDEmisorFactura")) grid.Columns["IDEmisorFactura"].Visible = false;
            if (grid.Columns.Contains("NumSerieFacturaEmisor")) grid.Columns["NumSerieFacturaEmisor"].Visible = false;
            if (grid.Columns.Contains("FechaExpedicionFacturaEmisor")) grid.Columns["FechaExpedicionFacturaEmisor"].Visible = false;

            if (grid.Columns.Contains("PagoFechaSII")) grid.CambiarColumnHeader("PagoFechaSII", "Fecha de Pago SII");  //Falta traducir
            if (grid.Columns.Contains("PagoImporteSII")) grid.CambiarColumnHeader("PagoImporteSII", "Importe de Pago SII");  //Falta traducir
            if (grid.Columns.Contains("PagoMedioSII")) grid.CambiarColumnHeader("PagoMedioSII", "Medio de Pago SII");  //Falta traducir
            if (grid.Columns.Contains("PagoCuentaOMedioSII")) grid.CambiarColumnHeader("PagoCuentaOMedioSII", "Cuenta o Medio de Pago SII");  //Falta traducir

            if (grid.Columns.Contains("PagoFechaLocal")) grid.CambiarColumnHeader("PagoFechaLocal", "Fecha de Pago Local");  //Falta traducir
            if (grid.Columns.Contains("PagoImporteLocal")) grid.CambiarColumnHeader("PagoImporteLocal", "Importe de Pago Local");  //Falta traducir
            if (grid.Columns.Contains("PagoMedioLocal")) grid.CambiarColumnHeader("PagoMedioLocal", "Medio de Pago Local");  //Falta traducir
            if (grid.Columns.Contains("PagoCuentaOMedioLocal")) grid.CambiarColumnHeader("PagoCuentaOMedioLocal", "Cuenta o Medio de Pago Local");  //Falta traducir
        }

        #region Valores Petición Formulario
        /// <summary>
        /// Carga los valores de la última petición del formulario
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores.PadRight(111,' '));
                StructGLL01_MISIICUAD myStruct = (StructGLL01_MISIICUAD)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIICUAD));

                try
                {
                    if (myStruct.libro.Trim() != "") this.cmbLibro.SelectedValue = myStruct.libro.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.compania.Trim() != "")
                {
                    this.codigoCompania = myStruct.compania.Trim();
                    this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;

                    string resultValComp = this.ValidarCompania();
                    if (resultValComp != "") MessageBox.Show(resultValComp, this.LP.GetText("errValTitulo", "Error"));
                }

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio;

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.porNifIdOtro == "1") this.rbNIF.Checked = true;
                else this.rbNIF.Checked = false;

                if (myStruct.nif.Trim() != "") this.txtNIF.Text = myStruct.nif;

                try
                {
                    if (myStruct.pais.Trim() != "") this.cmbPais.SelectedValue = myStruct.tipoIdentificacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.tipoIdentificacion.Trim() != "") this.cmbTipoIdentif.SelectedValue = myStruct.tipoIdentificacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.noFactura.Trim() != "") this.txtNumSerieFactura.Text = myStruct.noFactura.Trim();

                if (myStruct.fechaExpedicion.Trim() != "") this.txtMaskFechaExpedicion.Text = myStruct.fechaExpedicion;

                if (myStruct.soloDiferencias == "1") this.chkSoloDescuadre.Checked = true;
                else this.chkSoloDescuadre.Checked = false;
                
                if (myStruct.nombreRazonSocial.Trim() != "") this.txtNombreRazonSocial.Text = myStruct.nombreRazonSocial.Trim();

                result = true;

                Marshal.FreeBSTR(pBuf);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve una  cadena con todos los valores del formulario para grabar en la tabla de peticiones GLL01
        /// </summary>
        /// <returns></returns>
        private string ValoresPeticion()
        {
            string result = "";
            try
            {
                StructGLL01_MISIICUAD myStruct;

                myStruct.libro = this.cmbLibro.SelectedValue.ToString().PadRight(2, ' ');

                string codigo = "";
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
                {
                    myStruct.compania = codigoCompania.PadRight(2, ' ');
                }
                else myStruct.compania = codigo.PadRight(2, ' ');

                myStruct.ejercicio = this.txtEjercicio.Text.PadRight(4, ' ');

                string cadenaVacia = "";
                if (this.cmbPeriodo.SelectedValue != null) myStruct.periodo = this.cmbPeriodo.SelectedValue.ToString().PadRight(2, ' ');
                else myStruct.periodo = cadenaVacia.PadRight(2, ' ');

                if (this.rbNIF.Checked) myStruct.porNifIdOtro = "1";
                else myStruct.porNifIdOtro = "0";

                myStruct.nif = this.txtNIF.Text.PadRight(20, ' ');

                if (this.cmbPais.SelectedValue != null) myStruct.pais = this.cmbPais.SelectedValue.ToString().PadRight(2, ' ');
                else myStruct.pais = cadenaVacia.PadRight(2, ' ');

                if (this.cmbTipoIdentif.SelectedValue != null) myStruct.tipoIdentificacion = this.cmbTipoIdentif.SelectedValue.ToString().PadRight(2, ' ');
                else myStruct.tipoIdentificacion = cadenaVacia.PadRight(2, ' ');

                myStruct.noFactura = this.txtNumSerieFactura.Text.PadRight(25, ' ');

                myStruct.fechaExpedicion = this.txtMaskFechaExpedicion.Text.PadRight(10, ' ');

                myStruct.soloDiferencias = this.chkSoloDescuadre.Checked ? "1" : "0";

                myStruct.nombreRazonSocial = this.txtNombreRazonSocial.Text.PadRight(40, ' ');

                result = myStruct.libro + myStruct.compania + myStruct.ejercicio + myStruct.periodo + myStruct.porNifIdOtro + myStruct.nif;
                result += myStruct.pais + myStruct.tipoIdentificacion + myStruct.noFactura + myStruct.fechaExpedicion;
                result += myStruct.soloDiferencias + myStruct.nombreRazonSocial;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        /// <summary>
        /// Cargar el listado de las peticiones
        /// </summary>
        private void CargarPeticiones()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Control[] ctrls = this.Controls.Find("txtLibroDesc", false);
                if (ctrls == null || ctrls.Length == 0)
                {
                    TextBox txtLibroDesc = new TextBox();
                    txtLibroDesc.Name = "txtLibroDesc";
                    txtLibroDesc.Visible = false;
                    txtLibroDesc.Text = this.cmbLibro.Text;
                    this.Controls.Add(txtLibroDesc);
                }
                else ((TextBox)ctrls[0]).Text = this.cmbLibro.SelectedText;

                FormularioPeticion frmPeticion = new FormularioPeticion();
                frmPeticion.Path = ConfigurationManager.AppSettings["PathFicherosPeticiones"];
                frmPeticion.FormCode = this.formCode;
                frmPeticion.FicheroExtension = this.ficheroExtension;
                frmPeticion.Formulario = this;

                DataTable dtPeticiones = frmPeticion.ListarPeticion();

                if (dtPeticiones.Rows.Count > 0)
                {
                    Dictionary<string, string> dictControles = new Dictionary<string, string>();
                    dictControles.Add("Libro", "txtLibroDesc");
                    dictControles.Add("LibroCod", "cmbLibro");
                    dictControles.Add("Compañía", "tgTexBoxSelCiaFiscal");
                    dictControles.Add("Ejercicio", "txtEjercicio");
                    dictControles.Add("Periodo", "cmbPeriodo");
                    dictControles.Add("Número Serie Factura", "txtNumSerieFactura");
                    dictControles.Add("Fecha Expedicion", "txtMaskFechaExpedicion");
                    dictControles.Add("Por NIF", "rbNIF");
                    dictControles.Add("Por Otro", "rbOtro");
                    dictControles.Add("NIF", "txtNIF");
                    dictControles.Add("Nombre o Razón Social", "txtNombreRazonSocial");
                    dictControles.Add("País", "cmbPais");
                    dictControles.Add("Tipo Identificacion", "cmbTipoIdentif");
                    dictControles.Add("Solo Descuadre", "chkSoloDescuadre");

                    List<string> columnNoVisible = new List<string>(new string[] { "cmbTipoIdentif", "cmbPais", "txtNombreRazonSocial",
                                                                                   "txtNIF", "rbNIF", "rbOtro",
                                                                                   "chkSoloDescuadre", "cmbLibro", "txtElemento"});

                    TGPeticionesListar frmListarPeticiones = new TGPeticionesListar();
                    frmListarPeticiones.DtPeticiones = dtPeticiones;
                    frmListarPeticiones.CentrarForm = true;
                    frmListarPeticiones.Headers = dictControles;
                    frmListarPeticiones.ColumnNoVisible = columnNoVisible;
                    frmListarPeticiones.FrmPadre = this;
                    frmListarPeticiones.OkForm += new TGPeticionesListar.OkFormCommandEventHandler(frmListarPeticiones_OkForm);
                    frmListarPeticiones.Show();
                }
                else
                {
                    MessageBox.Show("No existen peticiones guardadas");    //Falta traducir
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Grabar la petición
        /// </summary>
        private void GrabarPeticion()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Añadir control con la descripción del libro para que después aparezca en el listado de peticiones posibles para cargar
                Control[] ctrls = this.Controls.Find("lblLibroDesc", false);
                if (ctrls == null || ctrls.Length == 0)
                {
                    TextBox txtLibroDesc = new TextBox();
                    txtLibroDesc.Name = "txtLibroDesc";
                    txtLibroDesc.Visible = false;
                    txtLibroDesc.Text = this.cmbLibro.Text;
                    this.Controls.Add(txtLibroDesc);
                }
                else ((TextBox)ctrls[0]).Text = this.cmbLibro.SelectedText;

                TGPeticionGrabar frmGrabarPeticion = new TGPeticionGrabar();
                frmGrabarPeticion.FormCode = this.formCode;
                frmGrabarPeticion.FrmPadre = this;
                frmGrabarPeticion.FicheroExtension = this.ficheroExtension;
                frmGrabarPeticion.ShowDialog();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Actualiza los controles (habilitado/deshabilitado, caption, ...) dado el libro seleccionado
        /// </summary>
        private void ActualizarControlesFromLibro()
        {
            if (this.cmbTipoIdentif.Items.Count > 0) this.cmbTipoIdentif.SelectedIndex = 0;
            if (this.rbOtro.Checked) this.rbNIF.Checked = true;
            switch (this.cmbLibro.SelectedValue.ToString())
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = false;
                    this.lblCodPais.Text = "Cód. País Destinatario";
                    this.lblTipoIden.Text = "Tipo Identif. Dest.";
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Social Dest.";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Dest.";
                    }
                    else this.lblNIF.Text = "NIF Destinatario";
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_FacturasRecibidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Emisor";
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Social Emisor";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Emisor";
                    }
                    else this.lblNIF.Text = "NIF Emisor";
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;   
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_BienesInversion:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Emisor";
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Social Emisor";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Emisor";
                    }
                    else this.lblNIF.Text = "NIF Emisor";
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    this.rbOtro.Text = "NIF-IVA";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Dest./Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Dest./Emisor";

                    if (this.rbOtro.Checked) this.cmbTipoIdentif.SelectedValue = "02";
                    this.lblTipoIden.Enabled = false;
                    this.cmbTipoIdentif.Enabled = false;
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Soc. Dest/Emisor";

                    if (this.rbOtro.Checked) this.lblNIF.Text = "Identif. Dest./Emisor";
                    else this.lblNIF.Text = "NIF Dest./Emisor";

                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Contraparte";
                    this.lblTipoIden.Text = "Tipo Contraparte";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identif. Contraparte";
                    }
                    else this.lblNIF.Text = "NIF Contraparte";
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_CobrosMetalico:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Contraparte";
                    this.lblTipoIden.Text = "Tipo Contraparte";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identif. Contraparte";
                    }
                    else this.lblNIF.Text = "NIF Contraparte";
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Soc. Contrap.";
                    this.lblNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    break;
                case LibroUtiles.LibroID_CobrosEmitidas:
                    this.rbOtro.Text = "Otro";
                    if (this.rbOtro.Checked) this.rbNIF.Checked = true;
                    this.gbTipoIdentificacion.Enabled = false;
                    this.lblNIF.Text = "NIF Emisor";
                    this.txtEjercicio.Text = "";
                    this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = false;
                    this.lblNombreRazonSocial.Enabled = true;
                    this.txtNombreRazonSocial.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    break;
                case LibroUtiles.LibroID_AgenciasViajes:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Contraparte";
                    this.lblTipoIden.Text = "Tipo Contraparte";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identif. Contraparte";
                    }
                    else this.lblNIF.Text = "NIF Contraparte";
                    this.lblNombreRazonSocial.Text = "Nombre o Razón Soc. Contrap.";
                    this.lblNombreRazonSocial.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Exportar la Grid de comparación a un fichero HTML que se visualizará en Excel 
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string titulo = "Comprobación entre Datos presentados y Datos Local";

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridDiff.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridDiff.Columns[i].HeaderText;  //Nombre de la columna

                    switch (nombreTipoVisible[0])
                    {
                        case "ImporteSII":
                        case "ImporteLocal":
                            nombreTipoVisible[1] = "decimal";
                            nombreTipoVisible[2] = this.tgGridDiff.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = this.tgGridDiff.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }
                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridDiff);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = ConsultaNombreFichero();

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "EXCEL";
                    //startInfo.FileName = "EXCEL.EXE";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
                catch // si no puede levantar excel, levantar html
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "IEXPLORE";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Devuelve el nombre del fichero temporal que se creará para la comprobación de datos entre la AEAT y Local
        /// </summary>
        /// <param name="formCode">Nombre del fichero</param>
        /// <param name="path">Ruta donde se encuentran los ficheros generados anteriormente</param>
        /// <returns></returns>
        private static string ConsultaNombreFichero()
        {
            string result = "";
            string path = System.Windows.Forms.Application.StartupPath;

            //Verificar que exista la carpeta tmp, sino crearla
            string pathTmp = "";
            try
            {
                //Chequear si no existe la carpeta
                if (!System.IO.Directory.Exists(path + "\\tmp")) System.IO.Directory.CreateDirectory(path + "\\tmp");

                pathTmp = "\\tmp";
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            if (pathTmp != "") path = path + pathTmp;

            string nombre = "SIICompDatos";
            try
            {
                string[] extensions = new[] { ".html" };
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] files =
                    dirInfo.EnumerateFiles()
                         .Where(f => extensions.Contains(f.Extension.ToLower()))
                         .ToArray();

                foreach (System.IO.FileInfo ficheroActual in files)
                {
                    try
                    {
                        //Chequear que la extension sea .html
                        ficheroActual.Delete();
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }

            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            //Devolver el nombre del fichero
            result = path + "\\" + nombre + "_" + System.Environment.UserName.ToUpper() + ".html";

            if (LibroUtiles.FileInUse(result))
            {
                //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                DateTime localDate = DateTime.Now;
                string fecha = localDate.Year.ToString() + localDate.Month.ToString() + localDate.Day.ToString() + "_";
                string hora = DateTime.Now.ToString("hh:mm:ss");
                hora = hora.Replace(":", "");
                fecha = fecha + hora;

                result = path + "\\" + nombre + "_" + System.Environment.UserName.ToUpper() + "_" + fecha + ".html";
            }

            return (result);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Bienes de inversión
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosBienesInversion(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("IDEmisorFactura")) grid.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (grid.Columns.Contains("NumSerieFacturaEmisor")) grid.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (grid.Columns.Contains("FechaExpedicionFacturaEmisor")) grid.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (grid.Columns.Contains("IdentificacionBien")) grid.CambiarColumnHeader("IdentificacionBien", "Identificación Bien");  //Falta traducir
                if (grid.Columns.Contains("FechaInicioUtilizacion")) grid.CambiarColumnHeader("FechaInicioUtilizacion", "Fecha Inicio Utilización");  //Falta traducir
                if (grid.Columns.Contains("EstadoFactura")) grid.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("TimestampUltimaModificacion")) grid.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 
                if (grid.Columns.Contains("VER")) grid.CambiarColumnHeader("VER", "Detalle");  //Falta traducir
                if (grid.Columns.Contains("MOV")) grid.CambiarColumnHeader("MOV", "Movimientos");  //Falta traducir

                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detminadas Operaciones Intracomunitarias
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetOperacionesIntracomunitarias(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("IDEmisorFactura")) grid.CambiarColumnHeader("IDEmisorFactura", "NIF Destinatario / Emisor");  //Falta traducir
                if (grid.Columns.Contains("NumSerieFacturaEmisor")) grid.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura");  //Falta traducir
                if (grid.Columns.Contains("FechaExpedicionFacturaEmisor")) grid.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNombreRazon")) grid.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (grid.Columns.Contains("EstadoFactura")) grid.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("TimestampUltimaModificacion")) grid.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 
                if (grid.Columns.Contains("VER")) grid.CambiarColumnHeader("VER", "Detalle");  //Falta traducir
                if (grid.Columns.Contains("MOV")) grid.CambiarColumnHeader("MOV", "Movimientos");  //Falta traducir

                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas de Cobros en Metálico
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosCobrosMetalico(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNifIdOtro")) grid.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNombreRazon")) grid.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNIFRepresentante")) grid.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal")) grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (grid.Columns.Contains("EstadoRegistro")) grid.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 

                //if (grid.Columns.Contains("IDEmisorFactura")) grid.Columns["IDEmisorFactura"].Visible = false;
                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
                if (grid.Columns.Contains("Ver")) grid.Columns["Ver"].Visible = false;
                if (grid.Columns.Contains("Mov")) grid.Columns["Mov"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Agencias de Viajes
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosAgenciasViaje(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNifIdOtro")) grid.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNombreRazon")) grid.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNIFRepresentante")) grid.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal")) grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (grid.Columns.Contains("EstadoRegistro")) grid.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 

                //if (grid.Columns.Contains("IDEmisorFactura")) grid.Columns["IDEmisorFactura"].Visible = false;
                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
                if (grid.Columns.Contains("Ver")) grid.Columns["Ver"].Visible = false;
                if (grid.Columns.Contains("Mov")) grid.Columns["Mov"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Operaciones de Seguros
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosOperacionesSeguros(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNifIdOtro")) grid.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNombreRazon")) grid.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNIFRepresentante")) grid.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal")) grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (grid.Columns.Contains("EstadoRegistro")) grid.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.CambiarColumnHeader("ClaveOperacionDesc", "Clave Operación");  //Falta 

                //if (grid.Columns.Contains("IDEmisorFactura")) grid.Columns["IDEmisorFactura"].Visible = false;
                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = true;
                if (grid.Columns.Contains("Ver")) grid.Columns["Ver"].Visible = false;
                if (grid.Columns.Contains("Mov")) grid.Columns["Mov"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturasRecibidas(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("IDEmisorFactura")) grid.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (grid.Columns.Contains("NumSerieFacturaEmisor")) grid.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (grid.Columns.Contains("FechaExpedicionFacturaEmisor")) grid.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (grid.Columns.Contains("TipoFacturaDesc")) grid.CambiarColumnHeader("TipoFacturaDesc", "Tipo Factura");  //Falta traducir
                if (grid.Columns.Contains("ClaveRegimenEspecialOTrascendenciaDesc")) grid.CambiarColumnHeader("ClaveRegimenEspecialOTrascendenciaDesc", "Clave Regimen Especial O Trascendencia");  //Falta traducir
                if (grid.Columns.Contains("DescripcionOperacion")) grid.CambiarColumnHeader("DescripcionOperacion", "Descripción Operación");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal")) grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (grid.Columns.Contains("BaseImponible")) grid.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (grid.Columns.Contains("CuotaDeducible")) grid.CambiarColumnHeader("CuotaDeducible", "Cuota");  //Falta traducir
                if (grid.Columns.Contains("EstadoFactura")) grid.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("TimestampUltimaModificacion")) grid.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir

                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;

                if (grid.Columns.Contains("VER")) grid.CambiarColumnHeader("VER", "Detalle");  //Falta traducir
                if (grid.Columns.Contains("MOV")) grid.CambiarColumnHeader("MOV", "Movimientos");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturasEmitidas(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("IDEmisorFactura")) grid.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (grid.Columns.Contains("IDEmisorFactura")) grid.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (grid.Columns.Contains("IDDestinatario")) grid.CambiarColumnHeader("IDDestinatario", "ID destinatario");  //Falta traducir
                if (grid.Columns.Contains("NumSerieFacturaEmisor")) grid.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (grid.Columns.Contains("FechaExpedicionFacturaEmisor")) grid.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (grid.Columns.Contains("TipoFacturaDesc")) grid.CambiarColumnHeader("TipoFacturaDesc", "Tipo Factura");  //Falta traducir
                if (grid.Columns.Contains("ClaveRegimenEspecialOTrascendenciaDesc")) grid.CambiarColumnHeader("ClaveRegimenEspecialOTrascendenciaDesc", "Clave Regimen Especial O Trascendencia");  //Falta traducir
                if (grid.Columns.Contains("DescripcionOperacion")) grid.CambiarColumnHeader("DescripcionOperacion", "Descripción Operación");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal"))
                {
                    grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                    grid.Columns["ImporteTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (grid.Columns.Contains("BaseImponible"))
                {
                    grid.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                    grid.Columns["BaseImponible"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (grid.Columns.Contains("Cuota")) grid.Columns["Cuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (grid.Columns.Contains("EstadoFactura")) grid.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("TimestampUltimaModificacion")) grid.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (grid.Columns.Contains("VER")) grid.CambiarColumnHeader("VER", "Detalle");  //Falta traducir
                if (grid.Columns.Contains("MOV")) grid.CambiarColumnHeader("MOV", "Movimientos");  //Falta traducir

                if (grid.Columns.Contains("TipoFactura")) grid.Columns["TipoFactura"].Visible = false;
                if (grid.Columns.Contains("ClaveRegimenEspecialOTrascendencia")) grid.Columns["ClaveRegimenEspecialOTrascendencia"].Visible = false;

                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas de Pagos Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosPagosRecibidas(ref TGGrid grid)
        {
            try
            {
                if (grid.Columns.Contains("Compania")) grid.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNifIdOtro")) grid.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNombreRazon")) grid.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (grid.Columns.Contains("ContraparteNIFRepresentante")) grid.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (grid.Columns.Contains("ImporteTotal")) grid.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (grid.Columns.Contains("EstadoRegistro")) grid.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir         
                if (grid.Columns.Contains("CodigoErrorRegistro")) grid.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (grid.Columns.Contains("DescripcionErrorRegistro")) grid.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta 

                if (grid.Columns.Contains("IDEmisorFactura")) grid.Columns["IDEmisorFactura"].Visible = false;
                if (grid.Columns.Contains("CargoAbono")) grid.Columns["CargoAbono"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacion")) grid.Columns["ClaveOperacion"].Visible = false;
                if (grid.Columns.Contains("ClaveOperacionDesc")) grid.Columns["ClaveOperacionDesc"].Visible = false;
                if (grid.Columns.Contains("Ver")) grid.Columns["Ver"].Visible = false;
                if (grid.Columns.Contains("Mov")) grid.Columns["Mov"].Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Chequea si la celda donde se ha hecho click es la de visualizar la factura o la de ver los movimientos de la factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FacturaVerMov(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                TGGrid grid = (TGGrid)sender;

                if (grid.Rows.Count >= 1 && e.RowIndex <= grid.Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string columnName = grid.Columns[e.ColumnIndex].Name;

                    if (columnName == "VER" || columnName == "MOV")
                    {
                        //int rowIndex = this.tgGridConsulta.CurrentCell.RowIndex;

                        FacturaIdentificador facturaID = new FacturaIdentificador();
                        facturaID.EmisorFacturaNIF = grid.SelectedRows[0].Cells["IDNIF"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroCodPais = grid.SelectedRows[0].Cells["IDOTROCodigoPais"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroIdType = grid.SelectedRows[0].Cells["IDOTROIdType"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroId = grid.SelectedRows[0].Cells["IDOTROId"].Value.ToString();

                        if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = grid.SelectedRows[0].Cells["NumSerieFacturaEmisor"].Value.ToString();
                        else facturaID.NumeroSerie = "";

                        if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = grid.SelectedRows[0].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                        else facturaID.FechaDocumento = "";

                        if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) facturaID.CargoAbono = grid.SelectedRows[0].Cells["CargoAbono"].Value.ToString();
                        else facturaID.CargoAbono = "";

                        string iDEmisorFactura;
                        string libro = this.cmbLibro.SelectedValue.ToString();

                        if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = grid.SelectedRows[0].Cells["IDEmisorFactura"].Value.ToString();
                        else iDEmisorFactura = "";

                        switch (columnName)
                        {
                            case "VER":
                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_FacturasEmitidas:
                                        this.ConsultaViewFacturaEmitida(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_FacturasRecibidas:
                                        this.ConsultaViewFacturaRecibida(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_BienesInversion:
                                        this.ConsultaViewBienInversion(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                                        this.ConsultaViewOperacionIntracomunitaria(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                }
                                break;
                            case "MOV":
                                frmSiiConsultaListaMovimientos frmListaMovs = new frmSiiConsultaListaMovimientos();
                                frmListaMovs.LibroID = libro;
                                frmListaMovs.FacturaID = facturaID;
                                frmListaMovs.IDEmisorFactura = iDEmisorFactura;
                                //frmListaMovs.Ejercicio = this.ejercicio;
                                //frmListaMovs.Periodo = this.periodo;
                                frmListaMovs.Show();
                                break;
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Llama al formulario que visualiza la factura emitida enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewFacturaEmitida(ref TGGrid grid, int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactEmitida frmViewFacturaEnviada = new frmSiiConsultaViewFactEmitida();
                if (this.ejercicioCG.Length == 2) frmViewFacturaEnviada.Ejercicio = "20" + this.ejercicioCG;
                else frmViewFacturaEnviada.Ejercicio = this.ejercicioCG;
                frmViewFacturaEnviada.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewFacturaEnviada.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "";

                if (grid.Name == "tgGridLocal")
                {
                    filtro += "(Compania='" + this.codigoCompania + "') AND ";
                    filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                    filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";

                    frmViewFacturaEnviada.DatosLocal = true;
                }
                else frmViewFacturaEnviada.DatosLocal = false;

                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

                if (grid.Name == "tgGridLocal") filtro += "AND (CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (grid.dsDatos.Tables.Contains("DatosGenerales") && grid.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DatosGenerales"].Select(filtro);

                        if (rows.Length == 1) rowDatosGrles = rows[0];
                    }
                    frmViewFacturaEnviada.RowDatosGrles = rowDatosGrles;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.RowDatosGrles = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataRow rowDatosMasInfo = null;
                    if (grid.dsDatos.Tables.Contains("MasInfo") && grid.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["MasInfo"].Select(filtro);

                        if (rows.Length == 1) rowDatosMasInfo = rows[0];
                    }
                    frmViewFacturaEnviada.RowMasInfo = rowDatosMasInfo;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataTable dtIVA = null;

                    if (grid.dsDatos.Tables.Contains("DetalleIVA") && grid.dsDatos.Tables["DetalleIVA"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DetalleIVA"].Select(filtro);

                        if (rows.Length != 0) dtIVA = rows.CopyToDataTable();
                    }
                    frmViewFacturaEnviada.DtDetallesIVA = dtIVA;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.DtDetallesIVA = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaEnviada.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la factura recibida enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewFacturaRecibida(ref TGGrid grid, int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactRecibida frmViewFacturaRecibida = new frmSiiConsultaViewFactRecibida();
                frmViewFacturaRecibida.Ejercicio = this.txtEjercicio.Text;
                frmViewFacturaRecibida.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewFacturaRecibida.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "";

                if (grid.Name == "tgGridLocal")
                {
                    filtro += "(Compania='" + this.codigoCompania + "') AND ";
                    filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                    filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";

                    frmViewFacturaRecibida.DatosLocal = true;
                }
                else frmViewFacturaRecibida.DatosLocal = false;

                filtro = "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

                if (grid.Name == "tgGridLocal") filtro += "AND (CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (grid.dsDatos.Tables.Contains("DatosGenerales") && grid.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DatosGenerales"].Select(filtro);

                        if (rows.Length == 1) rowDatosGrles = rows[0];
                    }
                    frmViewFacturaRecibida.RowDatosGrles = rowDatosGrles;
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowDatosGrles = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataRow rowDatosMasInfo = null;
                    if (grid.dsDatos.Tables.Contains("MasInfo") && grid.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["MasInfo"].Select(filtro);

                        if (rows.Length == 1) rowDatosMasInfo = rows[0];
                    }
                    frmViewFacturaRecibida.RowMasInfo = rowDatosMasInfo;
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataTable dtIVA = null;

                    if (grid.dsDatos.Tables.Contains("DetalleIVA") && grid.dsDatos.Tables["DetalleIVA"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DetalleIVA"].Select(filtro);

                        if (rows.Length != 0) dtIVA = rows.CopyToDataTable();
                    }
                    frmViewFacturaRecibida.DtDetallesIVA = dtIVA;
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.DtDetallesIVA = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataRow rowEstadoCuadre = null;
                    if (grid.dsDatos.Tables.Contains("EstadoCuadre") && grid.dsDatos.Tables["EstadoCuadre"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["EstadoCuadre"].Select(filtro);

                        if (rows.Length == 1) rowEstadoCuadre = rows[0];
                    }
                    frmViewFacturaRecibida.RowEstadoCuadre = rowEstadoCuadre;
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowEstadoCuadre = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaRecibida.Show(this);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la operación bien de inversión seleccionado
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewBienInversion(ref TGGrid grid, int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewBienInversion frmViewBienInversion = new frmSiiConsultaViewBienInversion();
                frmViewBienInversion.Ejercicio = this.txtEjercicio.Text;
                frmViewBienInversion.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewBienInversion.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "";

                if (grid.Name == "tgGridLocal")
                {
                    filtro += "(Compania='" + this.codigoCompania + "') AND ";
                    filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                    filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";

                    frmViewBienInversion.DatosLocal = true;
                }
                else frmViewBienInversion.DatosLocal = false;

                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

                if (grid.Name == "tgGridLocal") filtro += "AND (CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (grid.Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DatosGenerales"].Select(filtro);

                        if (rows.Length == 1) rowDatosGrles = rows[0];
                    }
                    frmViewBienInversion.RowDatosGrles = rowDatosGrles;
                }
                catch (Exception ex)
                {
                    frmViewBienInversion.RowDatosGrles = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataRow rowDatosMasInfo = null;
                    if (grid.dsDatos.Tables.Contains("MasInfo") && grid.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["MasInfo"].Select(filtro);

                        if (rows.Length == 1) rowDatosMasInfo = rows[0];
                    }
                    frmViewBienInversion.RowMasInfo = rowDatosMasInfo;
                }
                catch (Exception ex)
                {
                    frmViewBienInversion.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewBienInversion.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la operación intracomunitaria enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewOperacionIntracomunitaria(ref TGGrid grid, int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewDetOperacIntracomunitaria frmViewOperacionIntracomunitaria = new frmSiiConsultaViewDetOperacIntracomunitaria();
                frmViewOperacionIntracomunitaria.Ejercicio = this.txtEjercicio.Text;
                frmViewOperacionIntracomunitaria.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewOperacionIntracomunitaria.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "";

                if (grid.Name == "tgGridLocal")
                {
                    filtro += "(Compania='" + this.codigoCompania + "') AND ";
                    filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                    filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";

                    frmViewOperacionIntracomunitaria.DatosLocal = true;
                }
                else frmViewOperacionIntracomunitaria.DatosLocal = false;

                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

                if (grid.Name == "tgGridLocal") filtro += "AND (CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (grid.Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["DatosGenerales"].Select(filtro);

                        if (rows.Length == 1) rowDatosGrles = rows[0];
                    }
                    frmViewOperacionIntracomunitaria.RowDatosGrles = rowDatosGrles;
                }
                catch (Exception ex)
                {
                    frmViewOperacionIntracomunitaria.RowDatosGrles = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataRow rowDatosMasInfo = null;
                    if (grid.dsDatos.Tables.Contains("MasInfo") && grid.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = grid.dsDatos.Tables["MasInfo"].Select(filtro);

                        if (rows.Length == 1) rowDatosMasInfo = rows[0];
                    }
                    frmViewOperacionIntracomunitaria.RowMasInfo = rowDatosMasInfo;
                }
                catch (Exception ex)
                {
                    frmViewOperacionIntracomunitaria.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewOperacionIntracomunitaria.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Visualizar el detalle de la factura al hacer doble click en la grid de la AEAT o en la grid de Datos en Local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FacturaVerDetalle(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                TGGrid grid = (TGGrid)sender;

                if (grid.Rows.Count >= 1 && e.RowIndex <= grid.Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    FacturaIdentificador facturaID = new FacturaIdentificador();
                    facturaID.EmisorFacturaNIF = grid.SelectedRows[0].Cells["IDNIF"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroCodPais = grid.SelectedRows[0].Cells["IDOTROCodigoPais"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroIdType = grid.SelectedRows[0].Cells["IDOTROIdType"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroId = grid.SelectedRows[0].Cells["IDOTROId"].Value.ToString();

                    if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = grid.SelectedRows[0].Cells["NumSerieFacturaEmisor"].Value.ToString();
                    else facturaID.NumeroSerie = "";

                    if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = grid.SelectedRows[0].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                    else facturaID.FechaDocumento = "";

                    if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) facturaID.CargoAbono = grid.SelectedRows[0].Cells["CargoAbono"].Value.ToString();
                    else facturaID.CargoAbono = "";

                    string iDEmisorFactura;
                    string libro = this.cmbLibro.SelectedValue.ToString();

                    if (grid.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = grid.SelectedRows[0].Cells["IDEmisorFactura"].Value.ToString();
                    else iDEmisorFactura = "";

                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            this.ConsultaViewFacturaEmitida(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            this.ConsultaViewFacturaRecibida(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.ConsultaViewBienInversion(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                            this.ConsultaViewOperacionIntracomunitaria(ref grid, e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }
        #endregion

        private void tgGridDiff_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tgGridAEAT != null && this.tgGridAEAT.Rows.Count > 0) this.tgGridAEAT.ClearSelection();
                if (this.tgGridLocal != null && this.tgGridLocal.Rows.Count > 0) this.tgGridLocal.ClearSelection();

                if (this.tgGridDiff.SelectedRows.Count == 1)
                {
                    int currentRowGridDiff = this.tgGridDiff.CurrentRow.Index;

                    if (this.libro != LibroUtiles.LibroID_PagosRecibidas)
                    {

                        string idNif = this.tgGridDiff.Rows[currentRowGridDiff].Cells["IDNIF"].Value.ToString();
                        string idOtroCodigoPais = this.tgGridDiff.Rows[currentRowGridDiff].Cells["IDOTROCodigoPais"].Value.ToString();
                        string idOtroIdType = this.tgGridDiff.Rows[currentRowGridDiff].Cells["IDOTROIdType"].Value.ToString();
                        string idOtroId = this.tgGridDiff.Rows[currentRowGridDiff].Cells["IDOTROId"].Value.ToString();
                        string numSerieFacturaEmisor = this.tgGridDiff.Rows[currentRowGridDiff].Cells["NumSerieFacturaEmisor"].Value.ToString();
                        string fechaExpedicionFacturaEmisor = this.tgGridDiff.Rows[currentRowGridDiff].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                        string claveOperacion = this.tgGridDiff.Rows[currentRowGridDiff].Cells["ClaveOperacion"].Value.ToString();
                        int cantidadSII = Convert.ToInt16(this.tgGridDiff.Rows[currentRowGridDiff].Cells["CantidadSII"].Value);
                        int cantidadLocal = Convert.ToInt16(this.tgGridDiff.Rows[currentRowGridDiff].Cells["CantidadLocal"].Value);

                        if (cantidadSII > 0 && this.tgGridAEAT != null && this.tgGridAEAT.Rows.Count > 0)
                        {
                            try
                            {
                                int rowIndexAEAT = -1;

                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_CobrosMetalico:
                                    case LibroUtiles.LibroID_AgenciasViajes:
                                        DataGridViewRow rowAEAT = this.tgGridAEAT.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId)
                                                        )
                                                   )
                                            .First();

                                        rowIndexAEAT = rowAEAT.Index;

                                        this.tgGridAEAT.Rows[rowIndexAEAT].Selected = true;
                                        this.tgGridAEAT.FirstDisplayedScrollingRowIndex = rowIndexAEAT;
                                        break;
                                    case LibroUtiles.LibroID_OperacionesSeguros:
                                        DataGridViewRow rowAEATOpSeguros = this.tgGridAEAT.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId) &&
                                                         r.Cells["ClaveOperacion"].Value.ToString().Equals(claveOperacion)
                                                        )
                                                   )
                                            .First();

                                        rowIndexAEAT = rowAEATOpSeguros.Index;

                                        this.tgGridAEAT.Rows[rowIndexAEAT].Selected = true;
                                        this.tgGridAEAT.FirstDisplayedScrollingRowIndex = rowIndexAEAT;
                                        break;
                                    case LibroUtiles.LibroID_PagosRecibidas:
                                        DataGridViewRow rowAEATPagosRecibidas = this.tgGridAEAT.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId)
                                                        )
                                                   )
                                            .First();

                                        rowIndexAEAT = rowAEATPagosRecibidas.Index;

                                        this.tgGridAEAT.Rows[rowIndexAEAT].Selected = true;
                                        this.tgGridAEAT.FirstDisplayedScrollingRowIndex = rowIndexAEAT;
                                        break;
                                    default:
                                        string fechaExpedicionFacturaEmisorAEAT = fechaExpedicionFacturaEmisor.Replace('/', '-');

                                        DataGridViewRow rowAEATDefault = this.tgGridAEAT.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId) &&
                                                         r.Cells["NumSerieFacturaEmisor"].Value.ToString().Equals(numSerieFacturaEmisor) &&
                                                         r.Cells["FechaExpedicionFacturaEmisor"].Value.ToString().Equals(fechaExpedicionFacturaEmisorAEAT)
                                                        )
                                                   )
                                            .First();

                                        rowIndexAEAT = rowAEATDefault.Index;

                                        this.tgGridAEAT.Rows[rowIndexAEAT].Selected = true;
                                        this.tgGridAEAT.FirstDisplayedScrollingRowIndex = rowIndexAEAT;
                                        break;
                                }
                            }
                            catch { }
                        }

                        if (cantidadLocal > 0 && this.tgGridLocal != null && this.tgGridLocal.Rows.Count > 0)
                        {
                            try
                            {
                                int rowIndexLocal = -1;

                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_CobrosMetalico:
                                    case LibroUtiles.LibroID_AgenciasViajes:
                                    case LibroUtiles.LibroID_PagosRecibidas:
                                        DataGridViewRow rowLocal = this.tgGridLocal.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId)
                                                        )
                                                   )
                                            .First();

                                        rowIndexLocal = rowLocal.Index;

                                        this.tgGridLocal.Rows[rowIndexLocal].Selected = true;
                                        this.tgGridLocal.FirstDisplayedScrollingRowIndex = rowIndexLocal;
                                        break;
                                    case LibroUtiles.LibroID_OperacionesSeguros:
                                        DataGridViewRow rowLocalOpSeguros = this.tgGridLocal.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId) &&
                                                         r.Cells["ClaveOperacion"].Value.ToString().Equals(claveOperacion)
                                                        )
                                                   )
                                            .First();

                                        rowIndexLocal = rowLocalOpSeguros.Index;

                                        this.tgGridLocal.Rows[rowIndexLocal].Selected = true;
                                        this.tgGridLocal.FirstDisplayedScrollingRowIndex = rowIndexLocal;
                                        break;
                                    default:
                                        DataGridViewRow rowLocalDefault = this.tgGridLocal.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => (r.Cells["IDNIF"].Value.ToString().Equals(idNif) &&
                                                         r.Cells["IDOTROCodigoPais"].Value.ToString().Equals(idOtroCodigoPais) &&
                                                         r.Cells["IDOTROIdType"].Value.ToString().Equals(idOtroIdType) &&
                                                         r.Cells["IDOTROId"].Value.ToString().Equals(idOtroId) &&
                                                         r.Cells["NumSerieFacturaEmisor"].Value.ToString().Equals(numSerieFacturaEmisor) &&
                                                         r.Cells["FechaExpedicionFacturaEmisor"].Value.ToString().Equals(fechaExpedicionFacturaEmisor)
                                                        )
                                                   )
                                            .First();

                                        rowIndexLocal = rowLocalDefault.Index;

                                        this.tgGridLocal.Rows[rowIndexLocal].Selected = true;
                                        this.tgGridLocal.FirstDisplayedScrollingRowIndex = rowIndexLocal;
                                        break;
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        //Libro Pagos Recibidas
                        string pagoFechaSII = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoFechaSII"].Value.ToString();
                        pagoFechaSII = pagoFechaSII.Replace('/', '-');
                        string pagoImporteSII = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoImporteSII"].Value.ToString();
                        pagoImporteSII = pagoImporteSII.Replace(".",string.Empty).Replace(",", ".");
                        string pagoMedioSII = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoMedioSII"].Value.ToString();
                        string pagoCuentaOMedioSII = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoCuentaOMedioSII"].Value.ToString();

                        if (this.tgGridAEAT != null && this.tgGridAEAT.Rows.Count > 0)
                        {
                            try
                            {
                                int rowIndexAEAT = -1;
                                
                                DataGridViewRow rowAEAT = this.tgGridAEAT.Rows
                                    .Cast<DataGridViewRow>()
                                    .Where(r => (r.Cells["PagoFecha"].Value.ToString().Equals(pagoFechaSII) &&
                                                 r.Cells["PagoImporte"].Value.ToString().Equals(pagoImporteSII) &&
                                                 r.Cells["PagoMedioDesc"].Value.ToString().Equals(pagoMedioSII) &&
                                                 r.Cells["PagoCuentaOMedio"].Value.ToString().Equals(pagoCuentaOMedioSII)
                                                )
                                           )
                                    .First();

                                rowIndexAEAT = rowAEAT.Index;

                                this.tgGridAEAT.Rows[rowIndexAEAT].Selected = true;
                                this.tgGridAEAT.FirstDisplayedScrollingRowIndex = rowIndexAEAT;
                            }
                            catch(Exception ex) 
                            {
                                string error = ex.Message;
                            }
                        }
                        
                        string pagoFechaLocal = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoFechaLocal"].Value.ToString();
                        string pagoImporteLocal = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoImporteLocal"].Value.ToString();
                        string pagoMedioLocal = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoMedioLocal"].Value.ToString();
                        string pagoCuentaOMedioLocal = this.tgGridDiff.Rows[currentRowGridDiff].Cells["PagoCuentaOMedioLocal"].Value.ToString();

                        if (this.tgGridLocal != null && this.tgGridLocal.Rows.Count > 0)
                        {
                            try
                            {
                                int rowIndexLocal = -1;

                                DataGridViewRow rowLocal = this.tgGridLocal.Rows
                                    .Cast<DataGridViewRow>()
                                    .Where(r => (r.Cells["PagoFecha"].Value.ToString().Equals(pagoFechaLocal) &&
                                                 r.Cells["PagoImporte"].Value.ToString().Equals(pagoImporteLocal) &&
                                                 r.Cells["PagoMedioDesc"].Value.ToString().Equals(pagoMedioLocal) &&
                                                 r.Cells["PagoCuentaOMedio"].Value.ToString().Equals(pagoCuentaOMedioLocal)
                                                )
                                           )
                                    .First();

                                rowIndexLocal = rowLocal.Index;

                                this.tgGridLocal.Rows[rowIndexLocal].Selected = true;
                                this.tgGridLocal.FirstDisplayedScrollingRowIndex = rowIndexLocal;
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }

    public class DatosComparar
    {
        public string IDNIF { get; set; }
        public string IDOTROCodigoPais { get; set; }
        public string IDOTROIdType { get; set; }
        public string IDOTROId { get; set; }
        public string IDDestinatario { get; set; }
        public string NumSerieFacturaEmisor { get; set; }
        public string FechaExpedicionFacturaEmisor { get; set; }
        public string Estado { get; set; }
        public string Importe { get; set; }
        public string CargoAbono { get; set; }
        public string CodError { get; set; }
        public string MensajeError { get; set; }
        public string ClaveOperacion { get; set; }
        public string ClaveOperacionDesc { get; set; }
    }

    public class Items
    {
        public string IDNIF { get; set; }
        public string IDOTROCodigoPais { get; set; }
        public string IDOTROIdType { get; set; }
        public string IDOTROId { get; set; }
        public string IDDestinatario_Emisor { get; set; }
        public string NumSerieFacturaEmisor { get; set; }
        public string FechaExpedicionFacturaEmisor { get; set; }
        public string EstadoSII { get; set; }
        public string EstadoLocal { get; set; }
        public string ImporteSII { get; set; }
        public string ImporteLocal { get; set; }
        public string CargoAbono { get; set; }
        public int CantidadSII { get; set; }
        public int CantidadLocal { get; set; }
        public int ActulizarEstado { get; set; }
        public string CodError { get; set; }
        public string MensajeError { get; set; }
        public string ClaveOperacion { get; set; }
        public string ClaveOperacionDesc { get; set; }
    }

    public class ItemsPagoRecibidas
    {
        public string PagoFechaSII { get; set; }
        public string PagoImporteSII { get; set; }
        public string PagoMedioSII { get; set; }
        public string PagoCuentaOMedioSII { get; set; }
        public string PagoFechaLocal { get; set; }
        public string PagoImporteLocal { get; set; }
        public string PagoMedioLocal { get; set; }
        public string PagoCuentaOMedioLocal { get; set; }
    }
}
