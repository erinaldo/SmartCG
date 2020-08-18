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
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiSuministroInfo : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIISINF";
        public string ficheroExtension = "sis";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIISINF
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string ejercicio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string periodo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string noFactura;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string operacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string todasCompanias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string companias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaDocDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaDocHasta;
        }

        FormularioValoresCampos valoresFormulario;

        //private ArrayList ejerciciosArray;
        private ArrayList librosArray;
        private ArrayList operacionesArray;

        private bool librosAnualesVisible = false;      //Bienes de inversión y Conbros en Metálico sólo se envían en enero hasta el día 30

        private string tipoPeriodoImpositivo = "";

        //private string yearActual = "";
        //private string yearAnterior = "";

        private string yearSel = "";
        private string periodoSel = "";

        private string filtroFechaDocDesde = "";
        private string filtroFechaDocHasta = "";

        private DateTime filtroFechaDesde;
        private DateTime filtroFechaHasta;

        private DataTable resultEnvio;

        public frmSiiSuministroInfo()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiSuministroInfo_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Suministro Información");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Crear el desplegable de Ejercicios
            //this.CrearComboEjercicio();

            //this.librosAnualesVisible = this.EnviarLibrosAnuales();
            this.librosAnualesVisible = true;

            //Crear el desplegable de Periodos
            this.CrearComboPeriodos();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el TGGrid
            this.BuiltgGridPdteEnvio();

            //Construir el DataTable con el resultado del envio
            this.resultEnvio = new DataTable();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (this.CargarValoresUltimaPeticion(valores)) {}
            }
      
            this.tgTexBoxSelCiaFiscal.Textbox.Select();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
            this.lbCiasFiscales.Items.Clear();
            this.chkCiasFiscalesTodas.Checked = true;
            this.cmbPeriodo.SelectedIndex = 0;
            this.txtFactura.Text = "";
            this.txtEjercicio.Text = "";
            //if (this.cmbEjercicio.Items.Count > 0) this.cmbEjercicio.SelectedIndex = 0;
            if (this.cmbLibro.Items.Count > 0) this.cmbLibro.SelectedIndex = 0;
            if (this.cmbOperacion.Items.Count > 0) this.cmbOperacion.SelectedIndex = 0;

            this.txtMaskFechaDocDesde.Text = "";
            this.txtMaskFechaDocHasta.Text = "";

            //Llamar al buscador sin filtro
            this.Buscar();
        }

        private void cmbLibro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string libro = this.cmbLibro.SelectedValue.ToString();
            //string libroValor = this.cmbLibro.SelectedText;
            //if (libroValor != "") libroValor = libro;

            this.CrearComboOperacion(libro);

            //Habilitar o Deshabilitar campo Periodo
            switch(libro)
            {
                case LibroUtiles.LibroID_BienesInversion:       //Bienes de Inversión
                case LibroUtiles.LibroID_CobrosMetalico:        //Cobros en Metálico
                case LibroUtiles.LibroID_OperacionesSeguros:    //Operaciones de Seguros
                case LibroUtiles.LibroID_AgenciasViajes:        //Agencias de Viajes
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.cmbPeriodo.Enabled = false;
                    break;
                default:
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.cmbPeriodo.Enabled = true;
                    break;
            }
        }

        private void btnAddCiaFiscal_Click(object sender, EventArgs e)
        {
            if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
            {
                string descCiaFiscal = "";
                string nif = "";
                string validarCompFiscal = this.ValidarCompaniaFiscal(this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim(), ref descCiaFiscal, ref nif, ref this.tipoPeriodoImpositivo);

                string error = this.LP.GetText("errValTitulo", "Error");
                if (validarCompFiscal != "")
                {
                    MessageBox.Show(validarCompFiscal, error);
                    this.tgTexBoxSelCiaFiscal.Textbox.Select();
                    return;
                }

                string result = this.AddToListBox(this.tgTexBoxSelCiaFiscal.Textbox.Text, ref this.lbCiasFiscales);
                switch (result)
                {
                    case "":
                        this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
                        this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                        this.chkCiasFiscalesTodas.Checked = false;
                        break;
                    case "1":
                        MessageBox.Show(this.LP.GetText("errCiaFiscalExiste", "La compañía fiscal ya está en la lista"), error);    //Falta traducir
                        this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                        break;
                }
            }
        }

        private void btnQuitarCiaFiscal_Click(object sender, EventArgs e)
        {
            this.lbCiasFiscales.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbCiasFiscales.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbCiasFiscales.Items.Remove(item);
            }
            this.lbCiasFiscales.EndUpdate();
            if (this.lbCiasFiscales.Items.Count == 0)
            {
                this.tgTexBoxSelCiaFiscal.Textbox.Select();
                this.btnQuitarCiaFiscal.Enabled = false;
            }
            else this.lbCiasFiscales.Select();
        }

        private void btnEnviarTodo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string cia = "";
                string ejercicio = "";
                string periodo = "";
                string libro = "";
                string operacion = "";

                string libroDesc = "";
                string operacionDesc = "";

                DataTable dtRespuestaEnviarTodo = null;
                DataTable dtEnvioLibro;
                DataTable dtEnvioResumen = null;
                DataTable dtRespuestaEnviarTodoResumen = null;
                DataSet dsRespuesta = null;
                DataSet dsRespuestaEnviarTodo = null;

                bool existeContraparte = false;
                bool existeClaveOperacion = false;

                //Enviar todos los registros
                for (int i = 0; i < this.tgGridPdteEnvio.Rows.Count; i++)
                {
                    try
                    {
                        dtEnvioLibro = null;

                        cia = this.tgGridPdteEnvio.Rows[i].Cells["CIAFS1"].Value.ToString();
                        ejercicio = this.tgGridPdteEnvio.Rows[i].Cells["EJERCICIO"].Value.ToString();
                        periodo = this.tgGridPdteEnvio.Rows[i].Cells["PERIODO"].Value.ToString();

                        libro = this.tgGridPdteEnvio.Rows[i].Cells["TDOCS1"].Value.ToString();
                        operacion = this.tgGridPdteEnvio.Rows[i].Cells["TCOMS1"].Value.ToString();
                        
                        libroDesc = this.tgGridPdteEnvio.Rows[i].Cells["LIBRO"].Value.ToString();
                        operacionDesc = this.tgGridPdteEnvio.Rows[i].Cells["OPERACION"].Value.ToString();

                        //Enviar libro y operacion
                        dsRespuesta = this.Enviar(cia, ejercicio, periodo, libro, operacion);
                        if (dsRespuesta != null && dsRespuesta.Tables != null && dsRespuesta.Tables.Count >0) 
                        {
                            if (dsRespuesta.Tables.Count > 1)
                            {
                                if (dtRespuestaEnviarTodoResumen == null) dtRespuestaEnviarTodoResumen = CrearDataTableResumenEnvio();
                                dtEnvioResumen = dsRespuesta.Tables["Resumen"];
                                dtRespuestaEnviarTodoResumen.Merge(dtEnvioResumen);

                                if (dtRespuestaEnviarTodo == null) dtRespuestaEnviarTodo = this.CrearDataTableResultado();
                                dtEnvioLibro = dsRespuesta.Tables["Resultado"];
                                dtRespuestaEnviarTodo.Merge(dtEnvioLibro);
                            }
                            else
                            {
                                if (dsRespuesta.Tables[0].TableName == "Resumen")
                                {
                                    if (dtRespuestaEnviarTodoResumen == null) dtRespuestaEnviarTodoResumen = CrearDataTableResumenEnvio();
                                    dtEnvioResumen = dsRespuesta.Tables["Resumen"];
                                    dtRespuestaEnviarTodoResumen.Merge(dtEnvioResumen);
                                }
                                else if (dsRespuesta.Tables[0].TableName == "Resultado")
                                {
                                    if (dtRespuestaEnviarTodo == null) dtRespuestaEnviarTodo = this.CrearDataTableResultado();
                                    dtEnvioLibro = dsRespuesta.Tables["Resultado"];
                                    dtRespuestaEnviarTodo.Merge(dtEnvioLibro);
                                }
                            }


                            if (existeClaveOperacion == false || existeContraparte == false)
                            {
                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_CobrosMetalico:
                                        existeContraparte = true;
                                        break;
                                    case LibroUtiles.LibroID_OperacionesSeguros:
                                        existeContraparte = true;
                                        existeClaveOperacion = true;
                                        break;
                                    case LibroUtiles.LibroID_AgenciasViajes:
                                        existeContraparte = true;
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (dtRespuestaEnviarTodo == null) dtRespuestaEnviarTodo = this.CrearDataTableResultado();
                        this.InsertarRegistroTableResultado(ref dtRespuestaEnviarTodo, cia, libroDesc, operacionDesc, "Error General " + ex.Message, "", "", "1");
                    }
                }

                if (dtRespuestaEnviarTodo != null)
                {
                    //Visualizar respuesta
                    frmSiiSuministroInfoRespuesta frmInfoRespuesta = new frmSiiSuministroInfoRespuesta();
                    frmInfoRespuesta.Titulo = "Todos los libros";
                    frmInfoRespuesta.Compania = cia;
                    //frmInfoRespuesta.Ejercicio = ejercicio;
                    //frmInfoRespuesta.Periodo = periodo;
                    frmInfoRespuesta.ExisteContraparte = existeContraparte;
                    frmInfoRespuesta.ExisteClaveOperacion = existeClaveOperacion;
                    
                    //frmInfoRespuesta.DTRespuesta = dtRespuestaEnviarTodo;
                    dsRespuestaEnviarTodo = new DataSet();
                    try
                    {
                        dsRespuestaEnviarTodo.Tables.Add(dtRespuestaEnviarTodoResumen);
                        dsRespuestaEnviarTodo.Tables.Add(dtRespuestaEnviarTodo);
                    }

                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                    frmInfoRespuesta.DSRespuesta = dsRespuestaEnviarTodo;
                    frmInfoRespuesta.Show();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        private void txtEjercicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtEjercicio, false, ref sender, ref e);
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void lbCiasFiscales_Enter(object sender, EventArgs e)
        {
            if (this.lbCiasFiscales.Items.Count > 0) this.btnQuitarCiaFiscal.Enabled = true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void frmSiiSuministroInfo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void frmSiiSuministroInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Suministro Información");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiSuministroInfoTitulo", "Suministro de Información");
            this.Text += this.FormTituloAgenciaEntorno();

            this.toolStripButtonGrabarPeticion.Text = "&" + this.LP.GetText("toolStripButtonGrabarPeticion", "Grabar Petición");
            this.toolStripButtonCargarPeticion.Text = "&" + this.LP.GetText("toolStripButtonCargarPeticion", "Cargar Petición");
            this.toolStripButtonSalir.Text = "&" + this.LP.GetText("toolStripButtonSalir", "Salir");

            this.gbBuscador.Text = " " + this.LP.GetText("lblBuscador", "Buscador") + " ";
            this.gbListaCiasFiscales.Text = " " + this.LP.GetText("lblListaCiaFiscales", "Lista de Compañías Fiscales") + " ";
            this.btnAddCiaFiscal.Text = this.LP.GetText("lblAdd", "Añadir");
            this.btnQuitarCiaFiscal.Text = this.LP.GetText("lblDel", "Quitar");
            this.chkCiasFiscalesTodas.Text = this.LP.GetText("lblTodasCiasFiscales", "Todas las Compañías Fiscales");

            this.lblEjercicio.Text = this.LP.GetText("lblEjercicio", "Ejercicio");
            this.lblPeriodo.Text = this.LP.GetText("lblPeriodo", "Periodo");
            this.lblFactura.Text = this.LP.GetText("lblNoFactura", "No. Factura");
            this.lblFechaDocDesde.Text = this.LP.GetText("lblFechaExpDesde", "Fecha Exp. Desde");
            this.lblFechaDocHasta.Text = this.LP.GetText("lblFechaExpHasta", "Fecha Exp. Hasta");
            this.lblLibro.Text = this.LP.GetText("lblLibro", "Libro");
            this.lblOperacion.Text = this.LP.GetText("lblOperacion", "Operación");

            this.btnBuscar.Text = this.LP.GetText("lblBuscar", "Buscar");
            this.btnTodos.Text = this.LP.GetText("lblTodos", "Todos");

            this.gbResultados.Text = " " + this.LP.GetText("lblfrmSiiSumInfoPdteEnviar", "Pendiente de enviar") + " ";
            this.lblResult.Text = this.LP.GetText("lblfrmSiiSumInfoResult", "No existen movimientos para el criterio de selección indicado");

            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));
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
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 15;
            this.tgTexBoxSelCiaFiscal.AjustarTamanoTextBox();

            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 1;
            this.tgTexBoxSelCiaFiscal.Textbox.MaxLength = 2;

            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 4;

            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = GlobalVar.ConexionCG;
            
            this.tgTexBoxSelCiaFiscal.QueryFormSel = this.ObtenerQueryListaCompaniasFiscales();

            this.tgTexBoxSelCiaFiscal.FrmPadre = this;
        }

        private bool EnviarLibrosAnuales()
        {
            bool result = false;

            int mesActual = DateTime.Now.Month;

            if (mesActual == 1)
            {
                int diaActual = DateTime.Now.Day;

                if (diaActual < 31) result = true;
            }

            return (result);
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

                if (this.librosAnualesVisible) librosArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));

                librosArray.Add(new AddValue("1T", "1T"));
                librosArray.Add(new AddValue("2T", "2T"));
                librosArray.Add(new AddValue("3T", "3T"));
                librosArray.Add(new AddValue("4T", "4T"));

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
                string textoValor0 = this.LP.GetText("lblTodos", "Todos");
                string textoValor1 = this.LP.GetText("lblLibroFactEmitidas", "Facturas Emitidas");
                string textoValor2 = this.LP.GetText("lblLibroFacturasRecibidas", "Facturas Recibidas");
                string textoValor3 = this.LP.GetText("lblLibroBienesInversion", "Bienes de Inversión");
                string textoValor4 = this.LP.GetText("lblLibroDetOperaIntra", "Det. Operaciones Intracomunitarias");
                //string textoValor5 = this.LP.GetText("lblLibroCobros", "Cobros Emitidas");
                string textoValor6 = this.LP.GetText("lblLibroPagos", "Pagos Recibidas RECC");
                string textoValor7 = this.LP.GetText("lblLibroCobrosMetálico", "Cobros en metálico");
                string textoValor8 = this.LP.GetText("lblLibroOperacionesSeguros", "Operaciones de seguros");
                string textoValor9 = this.LP.GetText("lblLibroAgenciasViajes", "Agencias de viajes");

                librosArray = new ArrayList();
                librosArray.Add(new AddValue(textoValor0, "0"));    //Todos
                librosArray.Add(new AddValue(textoValor1, LibroUtiles.LibroID_FacturasEmitidas));       //Facturas Emitidas
                librosArray.Add(new AddValue(textoValor2, LibroUtiles.LibroID_FacturasRecibidas));      //Facturas Recibidas

                //if (this.librosAnualesVisible) librosArray.Add(new AddValue(textoValor3, LibroID_BienesInversion));     //Bienes de inversión -> solo en enero
                librosArray.Add(new AddValue(textoValor3, LibroUtiles.LibroID_BienesInversion));        //Bienes de inversión

                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor4, LibroUtiles.LibroID_OperacionesIntracomunitarias));   //Determinadas Operaciones Intracomunitarias
                //librosArray.Add(new AddValue(textoValor5, LibroUtiles.LibroID_CobrosEmitidas));         //Cobro Emitidas
                librosArray.Add(new AddValue(textoValor6, LibroUtiles.LibroID_PagosRecibidas));         //Pagos Recibidas

                //if (this.librosAnualesVisible) librosArray.Add(new AddValue(textoValor7, LibroID_CobrosMetalico));     //Cobros en metálico -> solo en enero
                librosArray.Add(new AddValue(textoValor7, LibroUtiles.LibroID_CobrosMetalico));         //Cobros en metálico
                if (this.agencia != "C") librosArray.Add(new AddValue(textoValor8, LibroUtiles.LibroID_OperacionesSeguros));     //Operaciones de seguros

                //if (this.librosAnualesVisible) librosArray.Add(new AddValue(textoValor9, LibroID_AgenciasViajes));     //Agencias de viajes
                librosArray.Add(new AddValue(textoValor9, LibroUtiles.LibroID_AgenciasViajes));         //Agencias de viajes

                this.cmbLibro.DataSource = librosArray;
                this.cmbLibro.DisplayMember = "Display";
                this.cmbLibro.ValueMember = "Value";

                this.cmbLibro.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /*
        /// <summary>
        /// Crea el desplegable de Ejercicios
        /// </summary>
        private void CrearComboEjercicio()
        {
            //Seleccionamos el mes y el año actual.
            //int mesActual = DateTime.Now.Month;
            //int annoActual = DateTime.Now.Year;
            //if (mesActual == 1) annoActual = annoActual - 1;

            int annoActual = DateTime.Now.Year;
            this.yearActual = annoActual.ToString();
            this.yearAnterior = (annoActual-1).ToString();

            ejerciciosArray = new ArrayList();
            ejerciciosArray.Add(new AddValue("", "-1"));
            ejerciciosArray.Add(new AddValue(this.yearActual, this.yearActual));
            ejerciciosArray.Add(new AddValue(this.yearAnterior, this.yearAnterior));

            this.cmbEjercicio.DataSource = ejerciciosArray;
            this.cmbEjercicio.DisplayMember = "Display";
            this.cmbEjercicio.ValueMember = "Value";

            this.cmbEjercicio.SelectedIndex = 0;
        }
        */

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

                switch(libro)
                {
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
        /// Construir el control de la Grid que contiene la información pendiente de enviar
        /// </summary>
        private void BuiltgGridPdteEnvio()
        {
            //Crear el DataGrid
            this.tgGridPdteEnvio.dsDatos = new DataSet();
            this.tgGridPdteEnvio.dsDatos.DataSetName = "PdteEnviar";
            this.tgGridPdteEnvio.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridPdteEnvio.ReadOnly = true;
            this.tgGridPdteEnvio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.tgGridPdteEnvio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridPdteEnvio.AllowUserToAddRows = false;
            this.tgGridPdteEnvio.AllowUserToOrderColumns = false;
            this.tgGridPdteEnvio.AutoGenerateColumns = false;
            this.tgGridPdteEnvio.NombreTabla = "PdeEnviar";

            DataTable dt = new DataTable();
            dt.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            dt.Columns.Add("CIAFS1", typeof(string));
            dt.Columns.Add("EJERCICIO", typeof(string));
            dt.Columns.Add("PERIODO", typeof(string));
            dt.Columns.Add("LIBRO", typeof(string));
            dt.Columns.Add("OPERACION", typeof(string));
            dt.Columns.Add("CANTIDAD", typeof(string));
            dt.Columns.Add("ENVIAR", typeof(Image));
            dt.Columns.Add("VER", typeof(Image));
            dt.Columns.Add("TDOCS1", typeof(string));
            dt.Columns.Add("TCOMS1", typeof(string));
            
            //Crear la columnas del DataGrid
            this.tgGridPdteEnvio.AddTextBoxColumn(0, "CIAFS1", this.LP.GetText("dgHeaderCia", "Compañía"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddTextBoxColumn(1, "EJERCICIO", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddTextBoxColumn(2, "PERIODO", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddTextBoxColumn(3, "LIBRO", this.LP.GetText("lblLibro", "Libro"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddTextBoxColumn(4, "OPERACION", this.LP.GetText("lblOperacion", "Operación"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddTextBoxColumn(5, "CANTIDAD", this.LP.GetText("dgHeaderCantidad", "Total Registros"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);
            this.tgGridPdteEnvio.AddImageColumn(6, "ENVIAR", "Enviar", 50, DataGridViewContentAlignment.TopCenter);
            this.tgGridPdteEnvio.AddImageColumn(7, "VER", this.LP.GetText("dgHeaderListar", "Listar"), 50, DataGridViewContentAlignment.TopCenter);
            this.tgGridPdteEnvio.AddTextBoxColumn(8, "TDOCS1", "TDOCS1", 2, 2, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);
            this.tgGridPdteEnvio.AddTextBoxColumn(9, "TCOMS1", "TCOMS1", 2, 2, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);

            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridPdteEnvio.dsDatos.Tables.Add(dt);

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridPdteEnvio.DataSource = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"];

            foreach (var column in this.tgGridPdteEnvio.Columns)
            {
                if (column is DataGridViewImageColumn)
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
            }
        }

        /// <summary>
        /// Valida el formulario de búsqueda
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            if (this.lbCiasFiscales.Items.Count == 0)
            {
                this.chkCiasFiscalesTodas.Checked = true;
                this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
            }

            if (this.txtEjercicio.Text.Trim() != "")
            {
                if (this.txtEjercicio.Text.Length == 1 || this.txtEjercicio.Text.Length == 3)
                {
                    MessageBox.Show(this.LP.GetText("errEjercicioFormato", "El ejercicio no tiene un formato válido (aa o aaaa)"), 
                                    this.LP.GetText("lblError", "Error"));
                    this.txtEjercicio.Focus();
                    return (false);
                }
                string ejercicio = this.txtEjercicio.Text;
                if (this.txtEjercicio.Text.Length == 2)
                {
                    //Completar el ejercicio a formato aaaa
                    ejercicio = "20" + ejercicio;
                }
                //El ejercicio no puede ser mayor que el año actual
                DateTime fechaActual = DateTime.Now;
                int ejercicioInt = Convert.ToInt16(ejercicio);
                if (ejercicioInt > fechaActual.Date.Year)
                {
                    MessageBox.Show(this.LP.GetText("errEjercicioMayorAnno", "El ejercicio no puede ser mayor que el año en curso"),
                                    this.LP.GetText("lblError", "Error"));
                    this.txtEjercicio.Focus();
                    return (false);
                }
            }

            bool fechaValida = true;
            string fechaStr = "";
            this.filtroFechaDesde = new DateTime();
            this.filtroFechaHasta = new DateTime();

            this.filtroFechaDocDesde = "";
            //coger el valor sin la máscara
            this.txtMaskFechaDocDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            this.filtroFechaDocDesde = this.txtMaskFechaDocDesde.Text.Trim();
            this.txtMaskFechaDocDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

            if (this.filtroFechaDocDesde != "")
            {
                fechaStr = this.txtMaskFechaDocDesde.Text;
                fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref this.filtroFechaDesde);
                if (fechaStr != this.txtMaskFechaDocDesde.Text) this.txtMaskFechaDocDesde.Text = fechaStr;
                if (!fechaValida)
                {
                    MessageBox.Show(this.LP.GetText("errFechaDocDesde", "La fecha del documento desde no es válida"),
                                    this.LP.GetText("lblError", "Error"));
                    this.txtMaskFechaDocDesde.Focus();
                    return (false);
                }
            }

            this.filtroFechaDocHasta = "";
            //coger el valor sin la máscara
            this.txtMaskFechaDocHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            this.filtroFechaDocHasta = this.txtMaskFechaDocHasta.Text.Trim();
            this.txtMaskFechaDocHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

            if (this.filtroFechaDocHasta != "")
            {
                fechaStr = this.txtMaskFechaDocHasta.Text;
                fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref this.filtroFechaHasta);
                if (fechaStr != this.txtMaskFechaDocDesde.Text) this.txtMaskFechaDocHasta.Text = fechaStr;
                if (!fechaValida)
                {
                    MessageBox.Show(this.LP.GetText("errFechaDocHasta", "La fecha del documento hasta no es válida"),
                                    this.LP.GetText("lblError", "Error"));
                    this.txtMaskFechaDocHasta.Focus();
                    return (false);
                }
            }

            if (this.filtroFechaDocDesde != "" && this.filtroFechaDocHasta != "")
            {
                if (this.filtroFechaDesde > this.filtroFechaHasta)
                {
                    MessageBox.Show(this.LP.GetText("errFechaDocDesdeHasta", "La fecha del documento desde no puede ser posterior a la fecha del documento hasta"),
                                    this.LP.GetText("lblError", "Error"));
                    this.txtMaskFechaDocDesde.Focus();
                    return (false);
                }
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

            return (true);
        }

            /// <summary>
            /// Devuelve el filtro para la búsqueda por libro y/o operacion
            /// </summary>
            /// <param name="libro"></param>
            /// <param name="operacion"></param>
            /// <returns></returns>
            private string ObtenerFiltroLibroOperacion(string libro, string operacion)
            {
                string filtro = "";

                if (libro != "0")
                {
                    filtro = "and TDOCS1 = '" + libro + "' ";
                    if (operacion != "0")
                    {
                        switch (operacion)
                        {
                            case "1":   //Alta
                                filtro += "and TCOMS1 = 'A0' ";
                                break;
                            case "2":   //Modificar
                                filtro += "and TCOMS1 = 'A1' ";
                                break;
                            case "4":   //Modificar Regimen Viajeros
                                filtro += "and TCOMS1 = 'A4' ";           //A4 -> Modificación Factura Régimen de Viajeros
                                break;
                            case "3":   //Anular
                                filtro += "and BAJAS1 = 'B' ";
                                break;
                            case "5":   //Alta devoluciones IVA de viajeros
                                filtro += "and TCOMS1 = 'A5' ";
                                break;
                            case "6":   //Modificar devoluciones IVA de viajeros
                                filtro += "and TCOMS1 = 'A6' ";
                                break;
                        }
                    }
                }

                return (filtro);
            }

        /// <summary>
        /// Devuelve la consulta dado los criterios de busqueda
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsulta()
        {
            string queryAltaMod = "";
            string filtroAltaMod = "";
            
            string queryBaja = "";
            string filtroBaja = "";

            string filtroCompania = "";
            string filtroLibroOperacion = "";
            string filtroFechas = "";
            string fechaAux = "";

            string query = "";
            try
            {
                //Compañías
                if (!this.chkCiasFiscalesTodas.Checked)
                {
                    for (int i = 0; i < this.lbCiasFiscales.Items.Count; i++)
                    {
                        if (i == 0) 
                        {
                            filtroCompania += " and (";
                            filtroCompania += " CIAFS1 = '" + this.lbCiasFiscales.Items[i].ToString() + "' ";
                        }
                        else filtroCompania += " or CIAFS1 = '" + this.lbCiasFiscales.Items[i].ToString() + "' ";

                        if (i + 1 == this.lbCiasFiscales.Items.Count) filtroCompania += ") ";
                    }
                }

                //Ejercicio
                string ejercicio = this.txtEjercicio.Text.Trim();
                if (ejercicio != "") 
                {
                    if (ejercicio.Length == 4) ejercicio = ejercicio.Substring(2, 2);    
                }
                this.yearSel = ejercicio;

                //Periodo
                string periodo = this.cmbPeriodo.Text.Trim();
                this.periodoSel = periodo;

                //Fechas
                if (this.filtroFechaDocDesde != "")
                {
                    //filtro fecha !!!
                    this.filtroFechaDocDesde = utiles.FechaAno4DigitosToFormatoCG(this.filtroFechaDesde);
                    filtroFechas = " and FDOCS1 >= " + this.filtroFechaDocDesde + " ";
                }
                if (this.filtroFechaDocHasta != "")
                {
                    this.filtroFechaDocHasta = utiles.FechaAno4DigitosToFormatoCG(this.filtroFechaHasta);
                    filtroFechas += " and FDOCS1 <= " + this.filtroFechaDocHasta + " ";
                }

                //Libro
                string libro = this.cmbLibro.SelectedValue.ToString();
                //Operacion
                string operacion = "0";
                if (this.cmbOperacion.SelectedValue != null) operacion = this.cmbOperacion.SelectedValue.ToString();
                if (libro != "0" || operacion != "0") filtroLibroOperacion = this.ObtenerFiltroLibroOperacion(libro, operacion);

                //Número Factura
                string numFactura = this.txtFactura.Text.Trim();

                //Todas las operaciones u operaciones de Alta o Modificar
                if (operacion == "0" || operacion != "3")
                {
                    queryAltaMod = "select CIAFS1, TDOCS1, EJERS1 as ejercicio, PERIS1 as periodo, TCOMS1 as operacion, count(*) as cantidad ";

                    if (filtroFechas != "") queryAltaMod += ", max(FDOCS1) as fecha ";

                    queryAltaMod += "from " + GlobalVar.PrefijoTablaCG + "IVSII1, ";
                    queryAltaMod += GlobalVar.PrefijoTablaCG + "IVT03 ";
                    queryAltaMod += "where STATS1 = ' ' and BAJAS1 = ' ' ";
                    queryAltaMod += "and CIAFS1 = CIAFT3  ";
                    queryAltaMod += "and DEDUS1 = '" + this.agencia + "' ";

                    switch (GlobalVar.ConexionCG.TipoBaseDatos)
                    {
                        case ProveedorDatos.DBTipos.DB2:
                            queryAltaMod += "and (digits(ULMCT3) < ('1' || EJERS1 || PERIS1) or PERIS1='0A') ";
                            break;
                        case ProveedorDatos.DBTipos.SQLServer:
                            queryAltaMod += "and (CAST(ULMCT3 AS CHAR(5))<('1' + EJERS1 + PERIS1) or PERIS1='0A' ) ";
                            break;
                        case ProveedorDatos.DBTipos.Oracle:
                            queryAltaMod += "and (ULMCT3 < ('1' || EJERS1 || PERIS1) or PERIS1='0A') "; 
                            break;
                    }

                    if (filtroCompania != "") queryAltaMod += filtroCompania;

                    if (ejercicio != "") filtroAltaMod += " and EJERS1 = '" + ejercicio + "'"; //filtroBaja += " and EJBAS1 = '" + ejercicio + "'";

                    if (periodo != "") filtroAltaMod += " and PERIS1 = '" + periodo + "' ";  //filtroBaja += " and PEBAS1 = '" + ejercicio + "'";

                    if (filtroLibroOperacion != "") filtroAltaMod += filtroLibroOperacion;

                    if (numFactura != "") filtroAltaMod += " and NSFES1 LIKE '%" + numFactura + "%' ";

                    if (filtroFechas != "" ) filtroAltaMod += filtroFechas;
                    
                    queryAltaMod += filtroAltaMod;

                    queryAltaMod += " group by CIAFS1, TDOCS1, EJERS1, PERIS1, TCOMS1 ";
                }

                //Todas las operaciones u operaciones de Baja
                if (operacion == "0" || operacion == "3")
                {
                    queryBaja = "select CIAFS1, TDOCS1, EJBAS1 as ejercicio, PEBAS1 as periodo, BAJAS1 as operacion, count(*) as cantidad ";

                    if (filtroFechas != "") queryBaja += ", max(FDOCS1) as fecha ";

                    queryBaja += " from " + GlobalVar.PrefijoTablaCG + "IVSII1, ";
                    queryBaja += GlobalVar.PrefijoTablaCG + "IVT03 ";
                    queryBaja += "where STATS1 = ' ' and BAJAS1 = 'B' ";
                    queryBaja += "and CIAFS1 = CIAFT3  ";
                    queryBaja += "and DEDUS1 = '" + this.agencia + "' ";

                    switch (GlobalVar.ConexionCG.TipoBaseDatos)
                    {
                        case ProveedorDatos.DBTipos.DB2:
                            queryBaja += "and (digits(ULMCT3) < ('1' || EJBAS1 || PEBAS1) or PEBAS1='0A') ";
                            break;
                        case ProveedorDatos.DBTipos.SQLServer:
                            queryBaja += "and (CAST(ULMCT3 AS CHAR(5))<('1' + EJBAS1 + PEBAS1) or PEBAS1='0A') ";
                            break;
                        case ProveedorDatos.DBTipos.Oracle:
                            queryBaja += "and (ULMCT3 < ('1' || EJBAS1 || PEBAS1) or PEBAS1='0A') ";
                            break;
                    }

                    if (filtroCompania != "") queryBaja += filtroCompania;

                    if (ejercicio != "") filtroBaja += " and EJBAS1 = '" + ejercicio + "'";

                    if (periodo != "") filtroBaja += " and PEBAS1 = '" + periodo + "' ";

                    if (filtroLibroOperacion != "") filtroBaja += filtroLibroOperacion;

                    if (numFactura != "") filtroBaja += " and NSFES1 LIKE '%" + numFactura + "%' ";

                    if (filtroFechas != "") filtroBaja += filtroFechas;

                    queryBaja += filtroBaja;

                    queryBaja += " group by CIAFS1, TDOCS1, EJBAS1, PEBAS1, BAJAS1 ";
                }

                if (queryAltaMod != "" && queryBaja != "")
                {
                    query = queryAltaMod + " UNION " + queryBaja;
                    query += " order by CIAFS1, ejercicio, periodo, TDOCS1, operacion";
                }
                else
                {
                    if (queryAltaMod != "")
                    {
                        query = queryAltaMod;
                        query += " order by CIAFS1, ejercicio, periodo, TDOCS1, operacion";
                    }
                    else
                    {
                        query = queryBaja;
                        query += " order by CIAFS1, ejercicio, periodo, TDOCS1, operacion";
                    }
                }
            }
            catch (Exception ex) { query = "";  Log.Error(Utiles.CreateExceptionString(ex)); }
            return (query);
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGrid()
        {
            IDataReader dr = null;
            try
            {
                if (this.tgGridPdteEnvio.Rows.Count > 0)
                {
                    //Eliminar los resultados de la búsqueda anterior
                    this.tgGridPdteEnvio.Visible = false;

                    try { this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Clear(); }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                //Obtener la consulta
                string query = this.ObtenerConsulta();

                if (query != "")
                {
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    DataRow row;
                    int cont = 0;

                    string libroCod = "";
                    string libroDesc = "";
                    string operacionCod = "";
                    string operacionDesc = "";

                    //string desodLibro = "";
                    //string codOperacion = "";
                    while (dr.Read())
                    {
                        row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                        row["CIAFS1"] = dr.GetValue(dr.GetOrdinal("CIAFS1")).ToString();
                        row["EJERCICIO"] = dr.GetValue(dr.GetOrdinal("ejercicio")).ToString();
                        row["PERIODO"] = dr.GetValue(dr.GetOrdinal("periodo")).ToString();
                        libroCod = dr.GetValue(dr.GetOrdinal("TDOCS1")).ToString();
                        libroDesc = this.ObtenerDescripcionLibro(libroCod);
                        row["LIBRO"] = libroDesc;
                        operacionCod = dr.GetValue(dr.GetOrdinal("operacion")).ToString();
                        operacionDesc = this.ObtenerDescripcionOperacion(operacionCod);
                        row["OPERACION"] = operacionDesc;
                        row["CANTIDAD"] = dr.GetValue(dr.GetOrdinal("CANTIDAD")).ToString();
                        row["ENVIAR"] = global::ModSII.Properties.Resources.Enviar;
                        row["VER"] = global::ModSII.Properties.Resources.Buscar;
                        row["TDOCS1"] = dr.GetValue(dr.GetOrdinal("TDOCS1")).ToString();
                        row["TCOMS1"] = operacionCod;

                        this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                        cont++;
                    }

                    this.gbResultados.Visible = true;

                    if (cont > 0)
                    {    
                        this.lblResult.Visible = false;

                        //Ningún registro seleccionado
                        this.tgGridPdteEnvio.ClearSelection();
                        this.tgGridPdteEnvio.Refresh();
                        this.tgGridPdteEnvio.Visible = true;
                        this.btnEnviarTodo.Visible = true;

                        //Ajustar todas las columnas de la Grid
                        this.AjustarColumnasGrid(ref this.tgGridPdteEnvio, -1);
                    }
                    else
                    {
                        this.lblResult.Text = this.LP.GetText("errNoExisteInfoPdteEnvio", "No existe información pendiente de envio para el criterio de selección indicado");
                        this.lblResult.Visible = true;
                        this.tgGridPdteEnvio.Visible = false;
                        this.btnEnviarTodo.Visible = false;
                    }}
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }


        /// <summary>
        /// Ajustar las columnas de un objeto DataGridView
        /// </summary>
        /// <param name="grid">Objeto DataGridView</param>
        /// <param name="indiceColumna">-1 todas las columnas; en caso contrario, la columna deseada</param>
        public void AjustarColumnasGrid(ref TGGrid grid, int indiceColumna)
        {
            if (indiceColumna != -1)
            {
                if (indiceColumna >= 0 && indiceColumna < grid.ColumnCount)
                {
                    //Ajustar la columna indicada (indiceColumna)
                    grid.AutoResizeColumn(indiceColumna);
                }
            }
            else
            {
                //Ajustar todas las columnas
                for (int i = 0; i < grid.ColumnCount; i++)
                {
                    grid.AutoResizeColumn(i);
                }
            }
        }

        /// <summary>
        /// Realiza la búsqueda de libros/operaciones pendientes de envío según los criterios seleccionados
        /// </summary>
        private void Buscar()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (this.FormValid())
                {
                    this.FillDataGrid();

                    //Grabar la petición
                    string valores = this.ValoresPeticion();

                    this.valoresFormulario.GrabarParametros(formCode, valores);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;
        }

        public string AddToListBox(string texto, ref ListBox controlListBox)
        {
            string result = "";

            for (int i = 0; i < controlListBox.Items.Count; i++)
            {
                if (controlListBox.Items[i].ToString() == texto)
                {
                    result = "1";   //El texto ya existe en la lista
                    break;
                }
            }

            if (result == "")
            {
                controlListBox.Items.Add(texto);
            }

            return (result);
        }

        private void tgGridPdteEnvio_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (e.RowIndex < this.tgGridPdteEnvio.dsDatos.Tables[0].Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string cia = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["CIAFS1"].Value.ToString();
                    string ejercicio = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["EJERCICIO"].Value.ToString();
                    string periodo = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["PERIODO"].Value.ToString();
                    string libro = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["TDOCS1"].Value.ToString();
                    string operacion = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["TCOMS1"].Value.ToString();

                    string libroDesc = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["LIBRO"].Value.ToString();
                    string operacionDesc = this.tgGridPdteEnvio.Rows[e.RowIndex].Cells["OPERACION"].Value.ToString();

                    if (this.tgGridPdteEnvio.Columns[e.ColumnIndex].Name == "ENVIAR")
                    {
                        //Enviar libro y operacion
                        DataSet dsRespuesta = this.Enviar(cia, ejercicio, periodo, libro, operacion);

                        if (dsRespuesta != null)
                        {
                            //Visualizar respuesta
                            frmSiiSuministroInfoRespuesta frmInfoRespuesta = new frmSiiSuministroInfoRespuesta();
                            frmInfoRespuesta.Titulo = libroDesc;
                            frmInfoRespuesta.Compania = cia;
                            //frmInfoRespuesta.Ejercicio = ejercicio;
                            //frmInfoRespuesta.Periodo = periodo;
                            frmInfoRespuesta.LibroCodigo = libro;

                            switch (libro)
                            {
                                case LibroUtiles.LibroID_CobrosMetalico:
                                case LibroUtiles.LibroID_AgenciasViajes:
                                    frmInfoRespuesta.ExisteContraparte = true;
                                    frmInfoRespuesta.ExisteClaveOperacion = false;
                                    break;
                                case LibroUtiles.LibroID_OperacionesSeguros:
                                    frmInfoRespuesta.ExisteContraparte = true;
                                    frmInfoRespuesta.ExisteClaveOperacion = true;
                                    break;
                                default:
                                    frmInfoRespuesta.ExisteContraparte = false;
                                    frmInfoRespuesta.ExisteClaveOperacion = false;
                                    break;
                            }

                            //frmInfoRespuesta.DTRespuesta = dtRespuesta;
                            frmInfoRespuesta.DSRespuesta = dsRespuesta;
                            frmInfoRespuesta.ShowDialog();

                            //Refrescar los resultados del grid
                            this.Buscar();
                        }
                    }
                    else if (this.tgGridPdteEnvio.Columns[e.ColumnIndex].Name == "VER")
                    {
                        //Visualizar la información
                        frmSiiSuministroInfoView frmView = new frmSiiSuministroInfoView();
                        frmView.Titulo = libroDesc;
                        frmView.Compania = cia;
                        frmView.Ejercicio = ejercicio;
                        frmView.Periodo = periodo;
                        frmView.LibroCodigo = libro;
                        frmView.LibroDescripcion = libroDesc;
                        frmView.OperacionCodigo = operacion;
                        frmView.OperacionDescripcion = operacionDesc;
                        string factura = this.txtFactura.Text.Trim();
                        if (factura != "") frmView.NumeroFactura = factura;
                        string fechaDesde = filtroFechaDocDesde;
                        if (fechaDesde != "") frmView.FechaDocCGDesde = fechaDesde;
                        string fechaHasta = filtroFechaDocHasta;
                        if (fechaHasta != "") frmView.FechaDocCGHasta = fechaHasta;

                        frmView.ShowDialog(this);
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        #region Respuesta Envios
        /// <summary>
        /// Devuelve un DataTable con la estructura de la respuesta del resumen del WebService
        /// </summary>
        /// <returns></returns>
        public static DataTable CrearDataTableResumenEnvio()
        {
            DataTable result = new DataTable();
            try
            {
                result.TableName = "Resumen"; 

                //Adicionar las columnas al DataTable
                result.Columns.Add("TotalFactEnviadas", typeof(int));
                result.Columns.Add("TotalFactCorrectas", typeof(int));
                result.Columns.Add("TotalFactAceptadasError", typeof(int));
                result.Columns.Add("TotalFactErrores", typeof(int));
                result.Columns.Add("TotalFactNoEnviadas", typeof(int));
                result.Columns.Add("CSV", typeof(string));
                result.Columns.Add("NifPresentador", typeof(string));
                result.Columns.Add("FechaPresentacion", typeof(string));
                result.Columns.Add("LibroCodigo", typeof(string));
                result.Columns.Add("OperacionCodigo", typeof(string));
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve un DataTable con la estructura de la respuesta del WebService
        /// </summary>
        /// <returns></returns>
        public DataTable CrearDataTableResultado()
        {
            DataTable result = new DataTable();
            try
            {
                result.TableName = "Resultado";

                //Adicionar las columnas al DataTable
                result.Columns.Add("Compania", typeof(string));
                //result.Columns.Add("Ejercicio", typeof(string));
                //result.Columns.Add("Periodo", typeof(string));
                result.Columns.Add("Libro", typeof(string));
                result.Columns.Add("Operacion", typeof(string));
                result.Columns.Add("Estado", typeof(string));
                result.Columns.Add("NoFactura", typeof(string));
                result.Columns.Add("FechaDoc", typeof(string));
                result.Columns.Add("RowResumen", typeof(string));
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }

        /// <summary>
        /// Inserta un registro en el DataTable de resultado
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="compania"></param>
        /// <param name="libro"></param>
        /// <param name="operacion"></param>
        /// <param name="estado"></param>
        /// <param name="noFactura"></param>
        /// <param name="fechaDoc"></param>
        /// <param name="marcaResumenEnvio">1 -> Fila Resumen de Envio o de Errores   0 -> Fila factura</param>
        public void InsertarRegistroTableResultado(ref DataTable dt, string compania, string libro,
                                                          string operacion, string estado, string noFactura, string fechaDoc,
                                                          string marcaResumenEnvio)
        {
            try
            {
                DataRow row;

                row = dt.NewRow();
                row["Compania"] = compania;
                //row["Ejercicio"] = ejercicio;
                //row["Periodo"] = periodo;
                row["Libro"] = libro;
                row["Operacion"] = operacion;
                row["Estado"] = estado;
                row["NoFactura"] = noFactura;
                row["FechaDoc"] = fechaDoc;
                row["RowResumen"] = marcaResumenEnvio;

                dt.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
        #endregion

        private void cmbPeriodo_Leave(object sender, EventArgs e)
        {
            this.cmbPeriodo.Text = this.cmbPeriodo.Text.ToUpper();

            if (!LibroUtiles.ValidarPeriodo(this.cmbPeriodo.Text))
            {
                string mensaje = this.LP.GetText("errNoExisteInfoPdteEnvio", "Por favor, entre un periodo válido");
                mensaje += " (01-12 o " + LibroUtiles.PeriodoAnual + "o 1T-4T)";
                MessageBox.Show(mensaje);
                this.cmbPeriodo.Focus();
                return;
            }
            else if (this.cmbPeriodo.Text.Length == 1) this.cmbPeriodo.Text = "0" + this.cmbPeriodo.Text;
        }

        private DataSet Enviar(string compania, string ejercicio, string periodo, string libro, string operacion)
        {
            DataSet dsResult = null;

            operacion = operacion.Trim();

            try
            {
                switch (libro)
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:      //Facturas Emitidas
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                            case "A4":    //Modificacion Regimen Viajeros
                            case "A5":    //Alta devoluciones IVA de Viajeros
                            case "A6":    //Modificacion devoluciones IVA de Viajeros
                                dsResult = this.SuministroLRFacturasEmitidas(compania, ejercicio, periodo, operacion, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLRFacturasEmitidas(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_FacturasRecibidas:     //Facturas Recibidas
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                            case "A4":    //Modificacion Regimen Viajeros
                                dsResult = this.SuministroLRFacturasRecibidas(compania, ejercicio, periodo, operacion, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLRFacturasRecibidas(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:   //Operaciones Intracomunitarias
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                                dsResult = this.SuministroLRDetOperacionIntracomunitaria(compania, ejercicio, periodo, operacion, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLRDetOperacionIntracomunitaria(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_BienesInversion:       //Bienes de Inversión
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                                dsResult = this.SuministroLRBienesInversion(compania, ejercicio, periodo, operacion, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                            case "B":      //Anular
                                dsResult = this.AnulacionLRBienesInversion(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_CobrosEmitidas:        //Cobros Emitidas
                        MessageBox.Show("Cobros Emitidos RACC no desarrollado", "Error");
                        /*
                        switch (operacion)
                        {
                            case "A0":    //Alta
                                result = this.SuministroLRCobrosEmitidas(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        */
                        break;
                    case LibroUtiles.LibroID_PagosRecibidas:        //Pagos Recibidas
                        switch (operacion)
                        {
                            case "A0":    //Alta
                                dsResult = this.SuministroLRPagosRecibidas(compania, ejercicio, periodo, this.txtFactura.Text, this.filtroFechaDocDesde, this.filtroFechaDocHasta);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_CobrosMetalico:        //Cobros en metálico
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                                dsResult = this.SuministroLRCobrosMetalico(compania, ejercicio, periodo, operacion, this.txtFactura.Text);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLRCobrosMetalico(compania, ejercicio, periodo, this.txtFactura.Text);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_OperacionesSeguros:    //Operaciones Seguros
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                                dsResult = this.SuministroLROperacionesSeguros(compania, ejercicio, periodo, operacion, this.txtFactura.Text);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLROperacionesSeguros(compania, ejercicio, periodo, this.txtFactura.Text);
                                break;
                        }
                        break;
                    case LibroUtiles.LibroID_AgenciasViajes:        //Agencias de Viajes
                        switch (operacion)
                        {
                            case "A0":    //Alta
                            case "A1":    //Modificacion
                                dsResult = this.SuministroLRAgenciasViajes(compania, ejercicio, periodo, operacion, this.txtFactura.Text);
                                break;
                            case "B":     //Anular
                                dsResult = this.AnulacionLRAgenciasViajes(compania, ejercicio, periodo, this.txtFactura.Text);
                                break;
                        }
                        break;
                }

                //Quitar la marca de aviso
                this.QuitarMarcaAviso(compania, libro, operacion, ref dsResult);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        /// <summary>
        /// Quitar la marca de aviso para el envio
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="libro"></param>
        /// <param name="operacion"></param>
        /// <param name="dsResult"></param>
        private void QuitarMarcaAviso(string compania, string libro, string operacion, ref DataSet dsResult)
        {
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables.Contains("Resumen"))
                {
                    string query = "";

                    string userlogado = System.Environment.UserName.ToUpper();
                    if (userlogado.Length > 10) userlogado = userlogado.Substring(0, 20);

                    string csv = "";
                    string nif = ""; 
                    string fecha = "";
                    int registros = 0;
                    for (int i = 0; i < dsResult.Tables["Resumen"].Rows.Count; i++)
                    {
                        csv = dsResult.Tables["Resumen"].Rows[i]["CSV"].ToString();
                        nif = dsResult.Tables["Resumen"].Rows[i]["NifPresentador"].ToString();
                        fecha = dsResult.Tables["Resumen"].Rows[i]["FechaPresentacion"].ToString();

                        query = "update " + GlobalVar.PrefijoTablaCG + "IVRSII set "; 
                        query += "AVISR1 = 'N', WUSER1 = '" + userlogado + "', " ;
                        query += "DUSER1 = DATER1 where ";
                        query += "AVISR1= 'S' and "; 
                        query += "NIFDR1 = '" + nif + "' and ";                        
                        query += "DATER1 = " + fecha + " and ";
                        query += "TDOCR1 = '" + libro + "' and ";
                        query += "COPSR1 = '" + operacion + "'";

                        registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private DataSet SuministroLRFacturasEmitidas(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRFacturasEmitidas(compania, ejercicio, periodo, operacion, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLRFacturasRecibidas(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRFacturasRecibidas(compania, ejercicio, periodo, operacion, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLRDetOperacionIntracomunitaria(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRDetOperacionIntracomunitaria(compania, ejercicio, periodo, operacion, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLRBienesInversion(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRBienesInversion(compania, ejercicio, periodo, operacion, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        /*
        private DataTable SuministroLRCobrosEmitidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataTable dtResult = null;
            IDataReader dr = null;
            try
            {
                dtResult = wsTGsii.SuministroLRCobrosEmitidas(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (dtResult);
        }
        */

        private DataSet SuministroLRPagosRecibidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRPagosRecibidas(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLRCobrosMetalico(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRCobrosMetalico(compania, ejercicio, periodo, operacion, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLROperacionesSeguros(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLROperacionesSeguros(compania, ejercicio, periodo, operacion, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet SuministroLRAgenciasViajes(string compania, string ejercicio, string periodo, string operacion, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRAgenciasViajes(compania, ejercicio, periodo, operacion, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRFacturasEmitidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRFacturasEmitidas(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRFacturasRecibidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            IDataReader dr = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRFacturasRecibidas(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRDetOperacionIntracomunitaria(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRDetOperacionIntracomunitaria(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRBienesInversion(string compania, string ejercicio, string periodo, string numeroFacturaBuscador, string fechaCGDesde, string fechaCGHasta)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRBienesInversion(compania, ejercicio, periodo, numeroFacturaBuscador, fechaCGDesde, fechaCGHasta);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        /*
        private DataTable AnulacionLRCobrosEmitidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador)
        {
            DataTable dtResult = null;
            IDataReader dr = null;
            try
            {
                dtResult = wsTGsii.SuministroLRCobrosEmitidas(compania, ejercicio, periodo, numeroFacturaBuscador, "", "");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (dtResult);
        }
        */

        private DataSet AnulacionLRPagosRecibidas(string compania, string ejercicio, string periodo, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.SuministroLRPagosRecibidas(compania, ejercicio, periodo, numeroFacturaBuscador, "", "");
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRCobrosMetalico(string compania, string ejercicio, string periodo, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRCobrosMetalico(compania, ejercicio, periodo, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLROperacionesSeguros(string compania, string ejercicio, string periodo, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLROperacionesSeguros(compania, ejercicio, periodo, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
        }

        private DataSet AnulacionLRAgenciasViajes(string compania, string ejercicio, string periodo, string numeroFacturaBuscador)
        {
            DataSet dsResult = null;
            try
            {
                dsResult = this.serviceSII.WSTGsii.AnulacionLRAgenciasViajes(compania, ejercicio, periodo, numeroFacturaBuscador);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (dsResult);
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
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MISIISINF myStruct = (StructGLL01_MISIISINF)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIISINF));

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio;

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.noFactura.Trim() != "") this.txtFactura.Text = myStruct.noFactura.Trim();

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

                if (myStruct.todasCompanias == "1") this.chkCiasFiscalesTodas.Checked = true;
                else this.chkCiasFiscalesTodas.Checked = false;

                if (myStruct.companias.Trim() != "")
                {
                    string companias = myStruct.companias.Trim();
                    string[] aCompanias = companias.Split(';');
                    this.lbCiasFiscales.Items.Clear();
                    string compania = "";

                    string desc = "";
                    string[] datosCompaniaFiscal;
                    for (int i = 0; i < aCompanias.Length; i++)
                    {
                        compania = aCompanias[i].Trim();
                        if (compania != "")
                        {
                            this.lbCiasFiscales.Items.Add(aCompanias[i]);

                            //Adicionar en el array de compañias fiscales
                            datosCompaniaFiscal = new string[2];
                            datosCompaniaFiscal[0] = compania;
                            datosCompaniaFiscal[1] = desc;
                            //this.aCompFiscales.Add(datosCompaniaFiscal);
                        }
                    }
                }

                if (myStruct.fechaDocDesde.Trim() != "") this.txtMaskFechaDocDesde.Text = myStruct.fechaDocDesde;

                if (myStruct.fechaDocHasta.Trim() != "") this.txtMaskFechaDocHasta.Text = myStruct.fechaDocHasta;

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
                StructGLL01_MISIISINF myStruct;

                myStruct.ejercicio = this.txtEjercicio.Text.PadRight(4, ' ');
                myStruct.periodo = this.cmbPeriodo.SelectedValue.ToString().PadRight(2, ' ');
                myStruct.noFactura = this.txtFactura.Text.PadRight(25, ' ');
                myStruct.libro = this.cmbLibro.SelectedValue.ToString().PadRight(2, ' ');
                myStruct.operacion = this.cmbOperacion.SelectedValue.ToString().PadRight(2, ' ');

                if (this.chkCiasFiscalesTodas.Checked) myStruct.todasCompanias = "1";
                else myStruct.todasCompanias = "0";

                //compañías fiscales
                myStruct.companias = "";
                for (int i = 0; i < this.lbCiasFiscales.Items.Count; i++)
                {
                    myStruct.companias += this.lbCiasFiscales.Items[i] + ";";
                }

                if (myStruct.companias.Length > 100) myStruct.companias = myStruct.companias.Substring(0, 99);
                else myStruct.companias = myStruct.companias.PadRight(100, ' '); 

                myStruct.fechaDocDesde = this.txtMaskFechaDocDesde.Text.PadRight(10, ' ');
                myStruct.fechaDocHasta = this.txtMaskFechaDocHasta.Text.PadRight(10, ' ');

                result = myStruct.ejercicio + myStruct.periodo + myStruct.noFactura + myStruct.libro + myStruct.operacion + myStruct.todasCompanias;
                result += myStruct.companias + myStruct.fechaDocDesde + myStruct.fechaDocHasta;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
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
                    dictControles.Add(this.LP.GetText("dgHeaderCia", "Compañía"), "tgTexBoxSelCiaFiscal");
                    dictControles.Add(this.LP.GetText("lblLibro", "Libro"), "txtLibroDesc");
                    dictControles.Add("LibroCod", "cmbLibro");
                    dictControles.Add(this.LP.GetText("lblEjercicio", "Ejercicio"), "txtEjercicio");
                    dictControles.Add(this.LP.GetText("lblPeriodo", "Periodo"), "cmbPeriodo");
                    dictControles.Add(this.LP.GetText("lblNoFactura", "No. Factura"), "txtFactura");
                    dictControles.Add("OperacionCod", "cmbOperacion");
                    dictControles.Add(this.LP.GetText("lblOperacion", "Operación"), "txtOperacionDesc");
                    dictControles.Add(this.LP.GetText("dgHeaderListaCiaFiscales", "Lista Compañías Fiscales"), "lbCiasFiscales");
                    dictControles.Add(this.LP.GetText("lblFechaExpDesde", "Fecha Exp. Desde"), "txtMaskFechaDocDesde");
                    dictControles.Add(this.LP.GetText("lblFechaExpHasta", "Fecha Exp. Hasta"), "txtMaskFechaDocHasta");
                    
                    List<string> columnNoVisible = new List<string>(new string[] { "cmbLibro", "cmbOperacion", "txtElemento",
                                                                                  "cmbEjercicio", "chkCiasFiscalesTodas"});

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
                    MessageBox.Show(this.LP.GetText("errNoPeticionesGuardadas", "No existen peticiones guardadas"));
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

            this.tgGridPdteEnvio.Visible = false;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //Ajustar todas las columnas de la Grid
            this.AjustarColumnasGrid(ref this.tgGridPdteEnvio, -1);
        }

        #endregion
    }
 }
