using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiEnviosConsulta : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIIENCO";
        public string ficheroExtension = "enc";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIIENCO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string operacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaPresentacionDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaPresentacionHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estado;
        }

        FormularioValoresCampos valoresFormulario;

        private ArrayList librosArray;
        private ArrayList operacionesArray;

        private string tipoPeriodoImpositivo = "";

        private string nifCompania = "";
        private string codigoCompania = "";
        private string consultaFiltroEstado = "";
        private string consultaFiltroFechaDesde = "";
        private string consultaFiltroFechaHasta = "";

        private DataSet dsEnvios;

        bool enviosExpanded = true;

        public frmSiiEnviosConsulta()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiEnviosConsulta_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta Envíos al SII");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el TGGrid de Resumen
            this.BuiltgEnviosResumenInfo();

            //Crear el TGGrid de Facturas
            this.BuiltgEnviosFacturasInfo();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (this.CargarValoresUltimaPeticion(valores)) { }
            }

            //Actualiza Controles dado Libro
            //this.ActualizarControlesFromLibro();

            this.cmbLibro.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Consultar();
        }

        private void tgGridResumen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridResumen.Rows.Count > 0)
            {
                this.tgGridResumen.Rows[0].Selected = false;

                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("DATER1")) this.tgGridResumen.Columns["DATER1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TIMER1")) this.tgGridResumen.Columns["TIMER1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TDOCR1")) this.tgGridResumen.Columns["TDOCR1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("COPSR1")) this.tgGridResumen.Columns["COPSR1"].Visible = false;
            }
        }

        private void tgGridResumen_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tgGridResumen_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridResumen.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridResumen.dsDatos.Tables[0].Rows.Count)
                {
                    this.BuscarFacturasEnviadas();
                }
            }
        }

        private void tgGridResumen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridResumen.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridResumen.dsDatos.Tables[0].Rows.Count)
                {
                    if (!this.enviosExpanded)
                    {
                        //Buscar las facturas del envio
                        this.BuscarFacturasEnviadas();
                    }
                }
            }
        }

        private void btnCerrarFacturas_Click(object sender, EventArgs e)
        {
            this.enviosExpanded = true;
            this.tgGridFacturas.Visible = false;
            this.toolStripButtonExportar.Enabled = false;
            //Grid Resumen modificar tamaño (más pequeño)
            this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 469);
        }

        private void tgGridResumen_SelectionChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!this.enviosExpanded)
            {
                //Buscar las facturas del envio
                this.BuscarFacturasEnviadas();
            }

            Cursor.Current = Cursors.Default;
        }
        
        private void tgGridFacturas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridFacturas.Rows.Count > 0)
            {
                this.tgGridFacturas.Rows[0].Selected = false;

                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdNif")) this.tgGridFacturas.Columns["IdNif"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroPais")) this.tgGridFacturas.Columns["IdOtroPais"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroTipo")) this.tgGridFacturas.Columns["IdOtroTipo"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroId")) this.tgGridFacturas.Columns["IdOtroId"].Visible = false;

                //Si el libro seleccionado es Operaciones de seguros
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("ClaveOperacion"))
                {
                    if (this.cmbLibro.SelectedValue.ToString() == LibroUtiles.LibroID_OperacionesSeguros)
                        this.tgGridFacturas.Columns["ClaveOperacion"].Visible = true;
                    else this.tgGridFacturas.Columns["ClaveOperacion"].Visible = false;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (this.cmbLibro.Items.Count > 0) this.cmbLibro.SelectedIndex = 0;
            if (this.cmbOperacion.Items.Count > 0) this.cmbOperacion.SelectedIndex = 0;
            this.txtMaskFechaPresentacionDesde.Text = "";
            this.txtMaskFechaPresentacionHasta.Text = "";
            this.rbEstadoTodos.Checked = true;
            this.tgGridResumen.Visible = false;
            this.tgGridFacturas.Visible = false;
            this.toolStripButtonExportar.Enabled = false;
            this.btnCerrarFacturas.Visible = false;
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void cmbLibro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string libro = this.cmbLibro.SelectedValue.ToString();

            this.CrearComboOperacion(libro);
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();
        }

        private void frmSiiEnviosConsulta_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiEnviosConsulta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta Envíos al SII");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiEnviosConsultacalTitulo", "Consulta de envíos");
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
        /// Crea el desplegable de Libros
        /// </summary>
        private void CrearComboLibros()
        {
            try
            {
                string textoValor0 = this.LP.GetText("lblLibroTodos", "Todos");    
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
                librosArray.Add(new AddValue(textoValor0, "0"));                          //Todos
                librosArray.Add(new AddValue(textoValor1, LibroUtiles.LibroID_FacturasEmitidas));       //Facturas Emitidas
                librosArray.Add(new AddValue(textoValor2, LibroUtiles.LibroID_FacturasRecibidas));      //Facturas Recibidas
                librosArray.Add(new AddValue(textoValor3, LibroUtiles.LibroID_BienesInversion));        //Bienes de inversión
                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor4, LibroUtiles.LibroID_OperacionesIntracomunitarias));       //Determinadas Operaciones Intracomunitarias
                //librosArray.Add(new AddValue(textoValor5, LibroUtiles.LibroID_CobrosEmitidas));         //Cobro Emitidas
                librosArray.Add(new AddValue(textoValor6, LibroUtiles.LibroID_PagosRecibidas));         //Pagos Recibidas
                librosArray.Add(new AddValue(textoValor7, LibroUtiles.LibroID_CobrosMetalico));         //Cobros en metálico
                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor8, LibroUtiles.LibroID_OperacionesSeguros));     //Operaciones de seguros
                librosArray.Add(new AddValue(textoValor9, LibroUtiles.LibroID_AgenciasViajes));         //Agencias de viajes

                this.cmbLibro.DataSource = librosArray;
                this.cmbLibro.DisplayMember = "Display";
                this.cmbLibro.ValueMember = "Value";

                this.cmbLibro.SelectedIndex = 0;

                this.CrearComboOperacion(this.cmbLibro.SelectedValue.ToString());
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama a la consulta correspondiente según el libro solicitado
        /// </summary>
        private void Consultar()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.tgGridResumen.Visible = false;
                this.tgGridFacturas.Visible = false;
                this.toolStripButtonExportar.Enabled = false;
                this.lblNoInfo.Visible = false;
                this.btnCerrarFacturas.Visible = false;

                //string libro = this.cmbLibro.SelectedValue.ToString();
                if (this.FormValid())
                {
                    this.lblInfo.Visible = true;
                    this.lblInfo.Update();
                    
                    if (this.tgGridResumen.dsDatos.Tables.Count > 0 && this.tgGridResumen.dsDatos.Tables.Contains("Resumen")) this.tgGridResumen.dsDatos.Tables.Remove("Resumen");
                    
                    if (this.tgGridFacturas.dsDatos.Tables.Count > 0 && this.tgGridFacturas.dsDatos.Tables.Contains("Facturas")) this.tgGridFacturas.dsDatos.Tables.Remove("Facturas");

                    //Eliminar todas las tablas del dataset
                    if (this.dsEnvios != null && this.dsEnvios.Tables != null && this.dsEnvios.Tables.Count > 0)
                    {
                        this.dsEnvios.Tables.Clear();
                        this.dsEnvios.Clear();
                    }

                    //Consultar la información de los envios
                    this.ConsultaInformacionEnvios();

                    //Grabar la petición
                    string valores = this.ValoresPeticion();

                    this.valoresFormulario.GrabarParametros(formCode, valores);
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = ex.Message;
                if (msgError == "Error consultando la información. Para más detalle consulte el fichero de Log.")
                {
                    this.lblNoInfo.Text = ex.Message;
                    this.lblNoInfo.Visible = true;
                }
            }
            Cursor.Current = Cursors.Default;

            this.lblInfo.Visible = false;
            this.lblInfo.Update();
        }

        private void BuiltgEnviosResumenInfo()
        {
            //Crear el DataGrid
            this.tgGridResumen.dsDatos = new DataSet();
            this.tgGridResumen.dsDatos.DataSetName = "Resumen";
            this.tgGridResumen.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridResumen.ReadOnly = true;
            this.tgGridResumen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridResumen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridResumen.AllowUserToAddRows = false;
            this.tgGridResumen.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridResumen.NombreTabla = "Resumen";
        }

        private void BuiltgEnviosFacturasInfo()
        {
            //Crear el DataGrid
            this.tgGridFacturas.dsDatos = new DataSet();
            this.tgGridFacturas.dsDatos.DataSetName = "Facturas";
            this.tgGridFacturas.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridFacturas.ReadOnly = true;
            this.tgGridFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridFacturas.AllowUserToAddRows = false;
            this.tgGridFacturas.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridFacturas.NombreTabla = "Facturas";
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <param name="libro"></param>
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

                this.consultaFiltroFechaDesde = "";
                DateTime fechaDesdeDateTime = new DateTime();
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaDesde = this.txtMaskFechaPresentacionDesde.Text.Trim();
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (this.consultaFiltroFechaDesde != "")
                {
                    string fechaStr = this.txtMaskFechaPresentacionDesde.Text;
                    bool fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaDesdeDateTime);
                    if (fechaStr != this.txtMaskFechaPresentacionDesde.Text) this.txtMaskFechaPresentacionDesde.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de presentación desde no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionDesde.Focus();
                        return (false);
                    }
                }

                this.consultaFiltroFechaHasta = "";
                DateTime fechaHastaDateTime = new DateTime();
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaHasta = this.txtMaskFechaPresentacionHasta.Text.Trim();
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (this.consultaFiltroFechaHasta != "")
                {
                    string fechaStr = this.txtMaskFechaPresentacionHasta.Text;
                    bool fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaHastaDateTime);
                    if (fechaStr != this.txtMaskFechaPresentacionHasta.Text) this.txtMaskFechaPresentacionHasta.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de presentación hasta no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionHasta.Focus();
                        return (false);
                    }
                }

                if (consultaFiltroFechaDesde != "" && consultaFiltroFechaHasta != "")
                {
                    if (fechaDesdeDateTime > fechaHastaDateTime)
                    {
                        MessageBox.Show("La fecha de presentación desde no puede ser posterior a la fecha de presentación hasta", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionDesde.Focus();
                        return (false);
                    }
                }

                if (this.consultaFiltroFechaDesde != "") this.consultaFiltroFechaDesde = this.txtMaskFechaPresentacionDesde.Text;

                if (this.consultaFiltroFechaHasta != "") this.consultaFiltroFechaHasta = this.txtMaskFechaPresentacionHasta.Text;

                this.consultaFiltroEstado = this.ObtenerEstado();

                //Buscar el nif para la compañia seleccionada
                this.nifCompania = LibroUtiles.NifCompania(this.codigoCompania);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (true);
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
                IntPtr pBuf = Marshal.StringToBSTR(valores.PadRight(27,' '));
                StructGLL01_MISIIENCO myStruct = (StructGLL01_MISIIENCO)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIIENCO));

                try
                {
                    if (myStruct.libro.Trim() != "") this.cmbLibro.SelectedValue = myStruct.libro.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.operacion.Trim() != "") this.cmbOperacion.SelectedValue = myStruct.operacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.compania.Trim() != "")
                {
                    this.codigoCompania = myStruct.compania.Trim();
                    this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;

                    string resultValComp = this.ValidarCompania();
                    if (resultValComp != "") MessageBox.Show(resultValComp, this.LP.GetText("errValTitulo", "Error"));
                }

                if (myStruct.fechaPresentacionDesde.Trim() != "") this.txtMaskFechaPresentacionDesde.Text = myStruct.fechaPresentacionDesde;

                if (myStruct.fechaPresentacionHasta.Trim() != "") this.txtMaskFechaPresentacionHasta.Text = myStruct.fechaPresentacionHasta;

                switch (myStruct.estado)
                {
                    case " ":
                        this.rbEstadoTodos.Checked = true;
                        break;
                    case "E":
                        this.rbEstadoIncorrecto.Checked = true;
                        break;
                    case "W":
                        this.rbEstadoAceptadoErrores.Checked = true;
                        break;
                    case "V":
                        this.rbEstadoCorrecto.Checked = true;
                        break;
                    default:        //Todos
                        this.rbEstadoTodos.Checked = true;
                        break;
                }

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
                StructGLL01_MISIIENCO myStruct;

                myStruct.libro = this.cmbLibro.SelectedValue.ToString().PadRight(2, ' ');
                myStruct.operacion = this.cmbOperacion.SelectedValue.ToString().PadRight(2, ' ');

                string codigo = "";
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
                {
                    myStruct.compania = codigoCompania.PadRight(2, ' ');
                }
                else myStruct.compania = codigo.PadRight(2, ' ');

                myStruct.fechaPresentacionDesde = this.txtMaskFechaPresentacionDesde.Text.PadRight(10, ' ');
                myStruct.fechaPresentacionHasta = this.txtMaskFechaPresentacionHasta.Text.PadRight(10, ' ');

                myStruct.estado = this.ObtenerEstado();

                result = myStruct.libro + myStruct.operacion + myStruct.compania + myStruct.fechaPresentacionDesde + myStruct.fechaPresentacionHasta;
                result += myStruct.estado;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        /// <summary>
        /// Devuelve el valor del estado indicado en el filtro para la consulta de las facturas enviadas al SII
        /// </summary>
        /// <returns></returns>
        private string ObtenerEstado()
        {
            string estado = "";
            try
            {
                if (this.rbEstadoTodos.Checked) estado = "T";
                else if (this.rbEstadoAceptadoErrores.Checked) estado = "W";
                else if (this.rbEstadoIncorrecto.Checked) estado = "E";
                else if (this.rbEstadoCorrecto.Checked) estado = "V";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (estado);
        }

        private void ConsultaInformacionEnvios()
        {
            IDataReader dr = null;
            try
            {
                //Inicializar el DataTable de Resultado
                this.dsEnvios = this.CrearDataSetResultadoConsultaEnvios();

                string libro = this.cmbLibro.SelectedValue.ToString();

                string sqlResumen = "select * from " +  GlobalVar.PrefijoTablaCG + "IVRSII ";
                sqlResumen += "where NIFDR1 = '" + this.nifCompania + "' ";
                
                //libro
                if (libro != "0") sqlResumen += "and TDOCR1 = '" + libro + "' ";

                //Operacion
                string filtroOperacion = "";
                string operacion = "0";
                if (this.cmbOperacion.SelectedValue != null) operacion = this.cmbOperacion.SelectedValue.ToString();
                if (operacion != "0") filtroOperacion = this.ObtenerFiltroOperacion(operacion);

                if (filtroOperacion != "") sqlResumen += filtroOperacion;

                string fechaCGStr = "";
                int fechaCG = -1;
                if (consultaFiltroFechaDesde != "")
                {
                    //fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde), true);
                    fechaCGStr = utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaDesde));
                    try{
                        fechaCG = -1;
                        if (fechaCGStr != "") fechaCG = Convert.ToInt32(fechaCGStr);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    if (fechaCG != -1) sqlResumen += "and DATER1 >= " + fechaCG + " ";
                }

                if (consultaFiltroFechaHasta != "")
                {
                    //fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaHasta), true);
                    fechaCGStr = utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(consultaFiltroFechaHasta));
                    try
                    {
                        fechaCG = -1;
                        if (fechaCGStr != "") fechaCG = Convert.ToInt32(fechaCGStr);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    if (fechaCG != -1) sqlResumen += "and DATER1 <= " + fechaCG + " ";
                }

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    switch (consultaFiltroEstado)
                    {
                        case "V":
                            sqlResumen += "and TOTVR1 > 0 ";
                            break;
                        case "W":
                            sqlResumen += "and TOTWR1 > 0 ";
                            break;
                        case "E":
                            sqlResumen += "and TOTER1 > 0 ";
                            break;
                    }
                }

                sqlResumen += "order by NIFDR1, DATER1 DESC, TIMER1 DESC";

                DataRow row;
                string fechaEnvio = "";
                string fechaEnvioFormatoSII = "";
                string horaEnvio = "";
                string horaEnvioFormatoSII = "";
                string libroCod = "";
                string libroDesc = "";
                string operacionCod = "";
                string operacionDesc = "";

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(sqlResumen, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.dsEnvios.Tables["Resumen"].NewRow();

                    row["CSV"] = dr.GetValue(dr.GetOrdinal("CSVER1")).ToString().Trim();
                    row["NIF"] = dr.GetValue(dr.GetOrdinal("NIFDR1")).ToString().Trim();
                                        
                    fechaEnvio = dr.GetValue(dr.GetOrdinal("DATER1")).ToString().Trim();
                    fechaEnvioFormatoSII = utiles.FormatoCGToFecha(fechaEnvio).ToShortDateString();
                    row["Fecha"] = fechaEnvioFormatoSII;
                    row["DATER1"] = fechaEnvio;

                    horaEnvio = dr.GetValue(dr.GetOrdinal("TIMER1")).ToString().Trim();
                    row["TIMER1"] = horaEnvio;
                    if (horaEnvio != "0")
                    {
                        if (horaEnvio.Length < 6) horaEnvio = horaEnvio.PadLeft(6, '0'); 
                        horaEnvioFormatoSII = horaEnvio.Substring(0, 2) + ":" + horaEnvio.Substring(2, 2) + ":" + horaEnvio.Substring(4, 2);
                    }
                    else horaEnvioFormatoSII = "";
                    row["Hora"] = horaEnvioFormatoSII;
                    
                    libroCod = dr.GetValue(dr.GetOrdinal("TDOCR1")).ToString().Trim();
                    row["TDOCR1"] = libroCod;
                    libroDesc = this.ObtenerDescripcionLibro(libroCod);
                    row["Libro"] = libroDesc;
                    
                    operacionCod = dr.GetValue(dr.GetOrdinal("COPSR1")).ToString().Trim();
                    row["COPSR1"] = operacionCod; 
                    operacionDesc = this.ObtenerDescripcionOperacion(operacionCod);
                    row["Operacion"] = operacionDesc;

                    row["TotalFacturas"] = dr.GetValue(dr.GetOrdinal("TOTRR1")).ToString().Trim();
                    row["TotalFacturasCorrectas"] = dr.GetValue(dr.GetOrdinal("TOTVR1")).ToString().Trim();
                    row["TotalFacturasAcepErrores"] = dr.GetValue(dr.GetOrdinal("TOTWR1")).ToString().Trim();
                    row["TotalFacturasErroneas"] = dr.GetValue(dr.GetOrdinal("TOTER1")).ToString().Trim();
                    row["TotalFacturasNoEnviadas"] = dr.GetValue(dr.GetOrdinal("TOTNR1")).ToString().Trim();

                    this.dsEnvios.Tables["Resumen"].Rows.Add(row);
                }

                dr.Close();

                if (this.dsEnvios.Tables.Count > 0)
                {
                    if (this.dsEnvios.Tables["Resumen"].Rows.Count > 0)
                    {
                        //Existen resumenes de envios
                        
                        //Adicionar el DataTable Resumen al DataSet del DataGrid
                        this.tgGridResumen.dsDatos.Tables.Add(this.dsEnvios.Tables["Resumen"].Copy());

                        this.tgGridResumen.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridResumen.DataSource = this.tgGridResumen.dsDatos.Tables["Resumen"];
                        
                        //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                        this.CambiarColumnasEncabezadosResumen();

                        this.tgGridResumen.Refresh();
                        this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 469);

                        this.tgGridResumen.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.enviosExpanded = true;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridResumen.Visible = false;
                        this.enviosExpanded = false;
                        this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridResumen.Visible = false;
                    this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.tgGridFacturas.Visible = false;
                    this.toolStripButtonExportar.Enabled = false;
                    this.btnCerrarFacturas.Visible = false;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                throw new Exception("Error consultando la información. Para más detalle consulte el fichero de Log.");
            }
        }

        /// <summary>
        /// Devuelve el filtro para la búsqueda por operacion
        /// </summary>
        /// <param name="operacion"></param>
        /// <returns></returns>
        private string ObtenerFiltroOperacion(string operacion)
        {
            string filtro = "";

            if (operacion != "0")
            {
                switch (operacion)
                {
                    case "1":   //Alta
                        filtro += "and COPSR1 = 'A0' ";
                        break;
                    case "2":   //Modificar
                        filtro += "and COPSR1 = 'A1' ";
                        break;
                    case "4":   //Modificar Regimen Viajeros
                        filtro += "and COPSR1 = 'A4' ";           //A4 -> Modificación Factura Régimen de Viajeros
                        break;
                    case "3":   //Anular
                        filtro += "and COPSR1 = 'B' ";
                        break;
                    case "5":   //Alta devoluciones IVA de viajeros
                        filtro += "and TCOMS1 = 'A5' ";
                        break;
                    case "6":   //Modificar devoluciones IVA de viajeros
                        filtro += "and TCOMS1 = 'A6' ";
                        break;
                }
            }

            return (filtro);
        }

        /// <summary>
        /// Crear los Datatables Resumen y Facturas que almacenan la información de los envios
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaEnvios()
        {
            this.dsEnvios = new DataSet();
            try
            {
                DataTable dtResumen = new DataTable();
                dtResumen.TableName = "Resumen";

                dtResumen.Columns.Add("CSV", typeof(string));
                dtResumen.Columns.Add("NIF", typeof(string));
                dtResumen.Columns.Add("Fecha", typeof(string));
                dtResumen.Columns.Add("Hora", typeof(string));
                dtResumen.Columns.Add("Libro", typeof(string));
                dtResumen.Columns.Add("Operacion", typeof(string));
                dtResumen.Columns.Add("TotalFacturas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasCorrectas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasAcepErrores", typeof(string));
                dtResumen.Columns.Add("TotalFacturasErroneas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasNoEnviadas", typeof(string));
                dtResumen.Columns.Add("DATER1", typeof(string));
                dtResumen.Columns.Add("TIMER1", typeof(string));
                dtResumen.Columns.Add("TDOCR1", typeof(string));
                dtResumen.Columns.Add("COPSR1", typeof(string));
                this.dsEnvios.Tables.Add(dtResumen);

                DataTable dtFacturas = new DataTable();
                dtFacturas.TableName = "Facturas";
                dtFacturas.Columns.Add("NumSerieFactura", typeof(string));
                dtFacturas.Columns.Add("FechaExpedicionFactura", typeof(string));
                dtFacturas.Columns.Add("IDEmisorFactura", typeof(string));
                dtFacturas.Columns.Add("IdNif", typeof(string));
                dtFacturas.Columns.Add("IdOtroPais", typeof(string));
                dtFacturas.Columns.Add("IdOtroTipo", typeof(string));
                dtFacturas.Columns.Add("IdOtroId", typeof(string));
                dtFacturas.Columns.Add("ClaveOperacion", typeof(string));
                dtFacturas.Columns.Add("CargoAbono", typeof(string));
                dtFacturas.Columns.Add("EstadoFactura", typeof(string));
                dtFacturas.Columns.Add("CodigoErrorRegistro", typeof(string));
                dtFacturas.Columns.Add("DescripcionErrorRegistro", typeof(string));
                //dtFacturas.Columns.Add("HORA", typeof(string));
                this.dsEnvios.Tables.Add(dtFacturas);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsEnvios);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Resumen
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosResumen()
        {
            try
            {
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("Operacion")) this.tgGridResumen.CambiarColumnHeader("Operacion", "Operación");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturas", "Total Facturas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasCorrectas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasCorrectas", "Total Fact. Correctas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasAcepErrores")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasAcepErrores", "Total Fact. Aceptadas con errores");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasErroneas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasErroneas", "Total Fac. incorrectas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasNoEnviadas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasNoEnviadas", "Total Fact. no enviadas");  //Falta traducir

                /*if (this.tgGridResumen.Columns.Contains("CSV")) this.tgGridResumen.Columns["CSV"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("NifPresentador")) this.tgGridResumen.Columns["NifPresentador"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("FechaPresentacion")) this.tgGridResumen.Columns["FechaPresentacion"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("HoraPresentacion")) this.tgGridResumen.Columns["HoraPresentacion"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("LibroCodigo")) this.tgGridResumen.Columns["LibroCodigo"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("OperacionCodigo")) this.tgGridResumen.Columns["OperacionCodigo"].Visible = false;*/
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturas()
        {
            try
            {
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("NumSerieFactura")) this.tgGridFacturas.CambiarColumnHeader("NumSerieFactura", "No. Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFactura")) this.tgGridFacturas.CambiarColumnHeader("FechaExpedicionFactura", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridFacturas.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("ClaveOperacion")) this.tgGridFacturas.CambiarColumnHeader("ClaveOperacion", "Clave Operación");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("CargoAbono")) this.tgGridFacturas.CambiarColumnHeader("CargoAbono", "Cargo/Abono");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridFacturas.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridFacturas.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridFacturas.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas
        /// </summary>
        /// <param name="cvs"></param>
        /// <param name="nifCia"></param>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="libro"></param>
        /// <param name="operacion"></param>
        private void ConsultaInformacionFacturasEnvio(string cvs, string nifCia, string fecha, string hora, string libro, string operacion)
        {
            IDataReader dr = null;
            try
            {
                string sqlFacturas = "select * from " + GlobalVar.PrefijoTablaCG + "IVLSII ";
                sqlFacturas += "where CSVEL1 = '" + cvs + "' and ";
                sqlFacturas += "NIFDL1 = '" + nifCia + "' and ";
                sqlFacturas += "DATEL1 = '" + fecha + "' and ";
                sqlFacturas += "TIMEL1 = '" + hora + "' and ";
                sqlFacturas += "TDOCL1 = '" + libro + "' and ";
                sqlFacturas += "COPSL1 = '" + operacion + "' ";

                if (consultaFiltroEstado != "" && consultaFiltroEstado != "T")
                {
                    switch (consultaFiltroEstado)
                    {
                        case "V":
                            sqlFacturas += "and SFACL1 = 'Correcto' ";
                            break;
                        case "W":
                            sqlFacturas += "and SFACL1 = 'AceptadoConErrores' ";
                            break;
                        case "E":
                            sqlFacturas += "and SFACL1 = 'Incorrecto' ";
                            break;
                    }
                }

                sqlFacturas += "order by NSFEL1";

                DataRow row;

                string fechaExpedicionFactura = "";
                string fechaExpedicionFacturaSII = "";

                string iDEmisorFacturaNIF = "";
                string idOtroCodPais = " ";
                string idOtroIdType = " ";
                string idOtroId = " ";
                
                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(sqlFacturas, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.dsEnvios.Tables["Facturas"].NewRow();

                    row["NumSerieFactura"] = dr.GetValue(dr.GetOrdinal("NSFEL1")).ToString().Trim();

                    fechaExpedicionFactura = dr.GetValue(dr.GetOrdinal("FDOCL1")).ToString().Trim();
                    fechaExpedicionFacturaSII = utiles.FormatoCGToFecha(fechaExpedicionFactura).ToShortDateString();
                    row["FechaExpedicionFactura"] = fechaExpedicionFacturaSII;

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFEL1")).ToString().Trim();

                    if (iDEmisorFacturaNIF != "")
                    {
                        row["IdNif"] = iDEmisorFacturaNIF;
                        row["IdOtroPais"] = "";
                        row["IdOtroTipo"] = "";
                        row["IdOtroId"] = "";

                        idOtroCodPais = " ";
                        idOtroIdType = " ";
                        idOtroId = " ";
                    }
                    else
                    {
                        row["IdNif"] = "";

                        idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAISL1")).ToString().Trim();
                        if (idOtroCodPais != "")
                        {
                            row["IdOtroPais"] = idOtroCodPais;
                            iDEmisorFacturaNIF += idOtroCodPais + "-";
                        }
                        row["IdOtroPais"] = idOtroCodPais;

                        idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDEL1")).ToString().Trim();
                        row["IdOtroTipo"] = idOtroIdType;
                        idOtroId = dr.GetValue(dr.GetOrdinal("IDOEL1")).ToString().Trim();
                        row["IdOtroId"] = idOtroId;
                        iDEmisorFacturaNIF += idOtroIdType + "-" + idOtroId;
                    }

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["ClaveOperacion"] = dr.GetValue(dr.GetOrdinal("CLOSL1")).ToString().Trim();
                    row["CargoAbono"] = dr.GetValue(dr.GetOrdinal("TPCGL1")).ToString().Trim();
                    row["EstadoFactura"] = dr.GetValue(dr.GetOrdinal("SFACL1")).ToString().Trim();
                    row["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    row["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();
                    //row["HORA"] = dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString().Trim();

                    this.dsEnvios.Tables["Facturas"].Rows.Add(row);
                }

                dr.Close();

                if (this.dsEnvios.Tables.Count > 0)
                {
                    if (this.dsEnvios.Tables["Facturas"].Rows.Count > 0)
                    {
                        //Existen resumenes de envios

                        //Adicionar el DataTable Resumen al DataSet del DataGrid
                        this.tgGridFacturas.dsDatos.Tables.Add(this.dsEnvios.Tables["Facturas"].Copy());

                        this.tgGridFacturas.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridFacturas.DataSource = this.tgGridFacturas.dsDatos.Tables["Facturas"];

                        //Cambiar los headers de las columnas del DataGrid de Facturas
                        this.CambiarColumnasEncabezadosFacturas();

                        this.tgGridFacturas.Refresh();
                        //this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 469);

                        this.btnCerrarFacturas.Visible = true;
                        this.tgGridFacturas.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;
                        this.enviosExpanded = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.btnCerrarFacturas.Visible = false;
                        this.tgGridFacturas.Visible = false;
                        this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.enviosExpanded = true;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridResumen.Visible = false;
                    this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Buscar las facturas del envio seleccionado
        /// </summary>
        private void BuscarFacturasEnviadas()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Grid Resumen modificar tamaño (más pequeño)
                this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 140);

                //if (!this.btnCerrarFacturas.Visible)
                if (this.enviosExpanded)
                {
                    //Posicionar el envío seleccionado al principio de la Grid
                    int rowIndexResumen = this.tgGridResumen.CurrentRow.Index;
                    this.tgGridResumen.FirstDisplayedScrollingRowIndex = rowIndexResumen;
                }

                //Eliminar la tabla de Facturas del dataset de la Grid de Facturas
                if (this.tgGridFacturas.dsDatos.Tables.Count > 0 && this.tgGridFacturas.dsDatos.Tables.Contains("Facturas")) this.tgGridFacturas.dsDatos.Tables.Remove("Facturas");

                //Eliminar todas la tablas Facturas del dataset
                if (this.dsEnvios != null && this.dsEnvios.Tables != null && this.dsEnvios.Tables.Count > 0 && this.dsEnvios.Tables.Contains("Facturas")) this.dsEnvios.Tables["Facturas"].Rows.Clear();

                string csv = this.tgGridResumen.SelectedRows[0].Cells["CSV"].Value.ToString();
                string nifCia = this.tgGridResumen.SelectedRows[0].Cells["NIF"].Value.ToString();
                string fecha = this.tgGridResumen.SelectedRows[0].Cells["DATER1"].Value.ToString();
                string hora = this.tgGridResumen.SelectedRows[0].Cells["TIMER1"].Value.ToString();
                string libro = this.tgGridResumen.SelectedRows[0].Cells["TDOCR1"].Value.ToString();
                string operacion = this.tgGridResumen.SelectedRows[0].Cells["COPSR1"].Value.ToString();

                //Buscar las facturas del envío
                this.ConsultaInformacionFacturasEnvio(csv, nifCia, fecha, hora, libro, operacion);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Crea el desplegable de Libros
        /// </summary>
        private void CrearComboOperacion(string libro)
        {
            try
            {
                string textoValor0 = this.LP.GetText("lblOperacionTodas", "Todas");
                string textoValor1 = this.LP.GetText("lblOperacionAlta", "Alta");
                string textoValor2 = this.LP.GetText("lblOperacionModificar", "Modificar");
                string textoValor3 = this.LP.GetText("lblOperacionAnular", "Anular");
                string textoValor4 = this.LP.GetText("lblOperacionModificarRegViajeros", "Modificar Reg. Viajeros");
                string textoValor5 = this.LP.GetText("lblOperacionAltaDevIVAViajeros", "Alta devolución IVA Viajeros");    //Falta traducir
                string textoValor6 = this.LP.GetText("lblOperacionModificarDevIVAViajeros", "Modificar devolución IVA Viajeros");    //Falta traducir

                operacionesArray = new ArrayList();

                switch (libro)
                {
                    case "0":
                    case LibroUtiles.LibroID_FacturasEmitidas:      //Facturas Emitidas
                        operacionesArray.Add(new AddValue(textoValor0, "0"));
                        operacionesArray.Add(new AddValue(textoValor1, "1"));
                        operacionesArray.Add(new AddValue(textoValor2, "2"));
                        operacionesArray.Add(new AddValue(textoValor4, "4"));
                        operacionesArray.Add(new AddValue(textoValor3, "3"));
                        operacionesArray.Add(new AddValue(textoValor5, "5"));
                        operacionesArray.Add(new AddValue(textoValor6, "6"));
                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:     //Facturas Recibidas
                        operacionesArray.Add(new AddValue(textoValor0, "0"));
                        operacionesArray.Add(new AddValue(textoValor1, "1"));
                        operacionesArray.Add(new AddValue(textoValor2, "2"));
                        operacionesArray.Add(new AddValue(textoValor4, "4"));
                        operacionesArray.Add(new AddValue(textoValor3, "3"));
                        break;
                    case LibroUtiles.LibroID_BienesInversion:       //Bienes de Inversión
                    case LibroUtiles.LibroID_CobrosMetalico:        //Cobros en Metálico
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:  //Operaciones Intracomunitarias
                    case LibroUtiles.LibroID_OperacionesSeguros:    //Operaciones de Seguros
                    case LibroUtiles.LibroID_AgenciasViajes:        //Agencias de Viajes
                        operacionesArray.Add(new AddValue(textoValor0, "0"));
                        operacionesArray.Add(new AddValue(textoValor1, "1"));
                        operacionesArray.Add(new AddValue(textoValor2, "2"));
                        operacionesArray.Add(new AddValue(textoValor3, "3"));
                        break;
                    case LibroUtiles.LibroID_CobrosEmitidas:        //Cobros Emitidos
                    case LibroUtiles.LibroID_PagosRecibidas:        //Pagos Recibidos
                        operacionesArray.Add(new AddValue(textoValor0, "0"));
                        operacionesArray.Add(new AddValue(textoValor1, "1"));
                        break;
                    default:
                        operacionesArray.Add(new AddValue(textoValor0, "0"));
                        break;
                }

                this.cmbOperacion.DataSource = operacionesArray;
                this.cmbOperacion.DisplayMember = "Display";
                this.cmbOperacion.ValueMember = "Value";

                this.cmbOperacion.SelectedIndex = 0;

                this.cmbOperacion.Refresh();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Exporta la consulta de Datos en Local a Excel, pasando por un fichero HTML
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string titulo = "Lista de Facturas Enviadas";

                try
                {
                    if (this.tgGridResumen.SelectedRows != null)
                    {
                        //Mostrar los datos del envío en el títlulo de la exportación (libro operacion fechaEnvio horaEnvio CSV)
                        string csv = this.tgGridResumen.SelectedRows[0].Cells["CSV"].Value.ToString();
                        string nifCia = this.tgGridResumen.SelectedRows[0].Cells["NIF"].Value.ToString();
                        string fecha = this.tgGridResumen.SelectedRows[0].Cells["DATER1"].Value.ToString();
                        string fechaEnvioFormatoSII = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                        string hora = this.tgGridResumen.SelectedRows[0].Cells["TIMER1"].Value.ToString();
                        string horaEnvioFormatoSII = hora.Trim();
                        if (horaEnvioFormatoSII != "0" && horaEnvioFormatoSII != "")
                        {
                            if (hora.Length < 6) hora = hora.PadLeft(6, '0');
                            horaEnvioFormatoSII = hora.Substring(0, 2) + ":" + hora.Substring(2, 2) + ":" + hora.Substring(4, 2);
                        }
                        else horaEnvioFormatoSII = "";
                        string libro = this.tgGridResumen.SelectedRows[0].Cells["TDOCR1"].Value.ToString();
                        string libroDesc = this.ObtenerDescripcionLibro(libro);
                        string operacion = this.tgGridResumen.SelectedRows[0].Cells["COPSR1"].Value.ToString();
                        string operacionDesc = this.ObtenerDescripcionOperacion(operacion);

                        titulo += " - " + libroDesc + " " + operacionDesc + " " + fechaEnvioFormatoSII + " " + horaEnvioFormatoSII + " " + csv;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridFacturas.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridFacturas.Columns[i].HeaderText;  //Nombre de la columna

                    switch (this.tgGridFacturas.Columns[i].Name)
                    {
                        case "CodigoErrorRegistro":
                            nombreTipoVisible[1] = "numero";
                            nombreTipoVisible[2] = this.tgGridFacturas.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = this.tgGridFacturas.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridFacturas);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIIConsultaEnvio");

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "EXCEL";
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

                //Añadir control con la descripción de la operación para que después aparezca en el listado de peticiones posibles para cargar
                ctrls = this.Controls.Find("txtOperacionDesc", false);
                if (ctrls == null || ctrls.Length == 0)
                {
                    TextBox txtOperacionDesc = new TextBox();
                    txtOperacionDesc.Name = "txtOperacionDesc";
                    txtOperacionDesc.Visible = false;
                    txtOperacionDesc.Text = this.cmbOperacion.Text;
                    this.Controls.Add(txtOperacionDesc);
                }
                else ((TextBox)ctrls[0]).Text = this.cmbOperacion.SelectedText;

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

                ctrls = this.Controls.Find("txtOperacionDesc", false);
                if (ctrls == null || ctrls.Length == 0)
                {
                    TextBox txtOperacionDesc = new TextBox();
                    txtOperacionDesc.Name = "txtOperacionDesc";
                    txtOperacionDesc.Visible = false;
                    txtOperacionDesc.Text = this.cmbOperacion.Text;
                    this.Controls.Add(txtOperacionDesc);
                }
                else ((TextBox)ctrls[0]).Text = this.cmbOperacion.SelectedText;

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
                    dictControles.Add("OperacionCod", "cmbOperacion");
                    dictControles.Add(this.LP.GetText("lblOperacion", "Operación"), "txtOperacionDesc");
                    dictControles.Add("Compañía", "tgTexBoxSelCiaFiscal");
                    dictControles.Add("Fecha Pres. Desde", "txtMaskFechaPresentacionDesde");
                    dictControles.Add("Fecha Pres. Hasta", "txtMaskFechaPresentacionHasta");
                    dictControles.Add("Estado Todos", "rbEstadoTodos");
                    dictControles.Add("Estado Correcto", "rbEstadoCorrecto");
                    dictControles.Add("Estado AceptadoErrores", "rbEstadoAceptadoErrores");
                    dictControles.Add("Estado Incorrecto", "rbEstadoIncorrecto");

                    List<string> columnNoVisible = new List<string>(new string[] { "cmbLibro", "cmbOperacion", "gbEstadoFacturas", "txtElemento" });

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

        private void frmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion();
            frmPeticion.FormCode = this.formCode;
            frmPeticion.FicheroExtension = this.ficheroExtension;
            frmPeticion.Formulario = this;
            string result = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));

            this.tgGridResumen.Visible = false;
            this.tgGridFacturas.Visible = false;
            this.btnCerrarFacturas.Visible = false;
            this.lblNoInfo.Visible = false;
        }
        #endregion
    }
}