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
    public partial class frmSiiSituacionGral : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIISIGRL";
        public string ficheroExtension = "grl";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIISIGRL
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string ejercicio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string periodo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string todasCompanias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string companias;
        }

        FormularioValoresCampos valoresFormulario;
        private ArrayList librosArray;
        private ArrayList periodosArray;

        private string tipoPeriodoImpositivo = "";

        public frmSiiSituacionGral()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiSituacionGral_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Situación General SII");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Crear el desplegable de Periodos
            this.CrearComboPeriodos();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el TGGrid
            this.BuiltgGridSituacionGral();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (this.CargarValoresUltimaPeticion(valores)) { }
            }

            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();

            this.tgTexBoxSelCiaFiscal.Textbox.Select();
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

        private void cmbLibro_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActualizarControlesFromLibro();
        }

        private void lbCiasFiscales_Enter(object sender, EventArgs e)
        {
            if (this.lbCiasFiscales.Items.Count > 0) this.btnQuitarCiaFiscal.Enabled = true;
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
            this.lbCiasFiscales.Items.Clear();
            this.chkCiasFiscalesTodas.Checked = true;
            this.cmbPeriodo.SelectedIndex = 0;
            this.txtEjercicio.Text = "";
            //if (this.cmbEjercicio.Items.Count > 0) this.cmbEjercicio.SelectedIndex = 0;
            if (this.cmbLibro.Items.Count > 0) this.cmbLibro.SelectedIndex = 0;

            //Llamar al buscador sin filtro
            this.Buscar();
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();    //Exporta a un HTML temporal y despúes se muestra en un Excel
        }
        
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEjercicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtEjercicio, false, ref sender, ref e);
        }

        private void frmSiiSituacionGral_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void frmSiiSituacionGral_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Situación General SII");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiSituacionGralTitulo", "Situación General del SII");
            this.Text += this.FormTituloAgenciaEntorno();

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

            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 3;

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
                periodosArray = new ArrayList();
                periodosArray.Add(new AddValue("", ""));
                periodosArray.Add(new AddValue("01", "01"));
                periodosArray.Add(new AddValue("02", "02"));
                periodosArray.Add(new AddValue("03", "03"));
                periodosArray.Add(new AddValue("04", "04"));
                periodosArray.Add(new AddValue("05", "05"));
                periodosArray.Add(new AddValue("06", "06"));
                periodosArray.Add(new AddValue("07", "07"));
                periodosArray.Add(new AddValue("08", "08"));
                periodosArray.Add(new AddValue("09", "09"));
                periodosArray.Add(new AddValue("10", "10"));
                periodosArray.Add(new AddValue("11", "11"));
                periodosArray.Add(new AddValue("12", "12"));
                periodosArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));

                librosArray.Add(new AddValue("1T", "1T"));
                librosArray.Add(new AddValue("2T", "2T"));
                librosArray.Add(new AddValue("3T", "3T"));
                librosArray.Add(new AddValue("4T", "4T"));

                this.cmbPeriodo.DataSource = periodosArray;
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
                string textoValor0 = this.LP.GetText("lblLibroTodos", "Todos");
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

        /// <summary>
        /// Construir el control de la Grid que contiene la información de la situación general del SII
        /// </summary>
        private void BuiltgGridSituacionGral()
        {
            //Crear el DataGrid
            this.tgGridSituacion.dsDatos = new DataSet();
            this.tgGridSituacion.dsDatos.DataSetName = "SituacionGral";
            this.tgGridSituacion.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridSituacion.ReadOnly = true;
            this.tgGridSituacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridSituacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridSituacion.AllowUserToAddRows = false;
            this.tgGridSituacion.AllowUserToOrderColumns = false;
            this.tgGridSituacion.AutoGenerateColumns = false;
            
            this.tgGridSituacion.NombreTabla = "Situacion";

            DataTable dtSituacion = new DataTable();
            dtSituacion.TableName = "Situacion";

            //Adicionar las columnas al DataTable
            dtSituacion.Columns.Add("CIAFS1", typeof(string));
            dtSituacion.Columns.Add("EJERS1", typeof(string));
            dtSituacion.Columns.Add("PERIS1", typeof(string));
            dtSituacion.Columns.Add("TDOCS1", typeof(string));
            dtSituacion.Columns.Add("Total", typeof(int));
            dtSituacion.Columns.Add("PdteEnvio", typeof(int));
            dtSituacion.Columns.Add("Correcta", typeof(int));
            dtSituacion.Columns.Add("AceptadaErrores", typeof(int));
            dtSituacion.Columns.Add("Incorrecta", typeof(int));
            dtSituacion.Columns.Add("Anulada", typeof(int));


            //Crear la columnas del DataGrid
            this.tgGridSituacion.AddTextBoxColumn(0, "CIAFS1", this.LP.GetText("dgHeaderCia", "Compañía"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(1, "EJERS1", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(2, "PERIS1", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(3, "TDOCS1", this.LP.GetText("dgHeaderLibro", "Libro"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(4, "Total", this.LP.GetText("dgHeaderTotalReg", "Total Registros"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(5, "PdteEnvio", this.LP.GetText("dgHeaderPdteEnvio", "Pendientes de Envío"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(6, "Incorrecta", this.LP.GetText("dgHeaderIncorrecta", "Incorrectas"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(7, "AceptadaErrores", this.LP.GetText("dgHeaderAceptadaErrores", "Aceptadas con errores"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            this.tgGridSituacion.AddTextBoxColumn(8, "Correcta", this.LP.GetText("dgHeaderCorrecta", "Correctas"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir         
            this.tgGridSituacion.AddTextBoxColumn(9, "Anulada", this.LP.GetText("dgHeaderAnuladas", "Anuladas"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir

            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridSituacion.dsDatos.Tables.Add(dtSituacion);

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridSituacion.DataSource = this.tgGridSituacion.dsDatos.Tables["Situacion"];

            DataTable dt = new DataTable();
            dt.TableName = "DatosTabla";

            //Adicionar las columnas al DataTable
            dt.Columns.Add("CIAFS1", typeof(string));
            dt.Columns.Add("EJERS1", typeof(string));
            dt.Columns.Add("PERIS1", typeof(string));
            dt.Columns.Add("TDOCS1", typeof(string));
            dt.Columns.Add("STATS1", typeof(string));
            dt.Columns.Add("BAJAS1", typeof(string));
            dt.Columns.Add("TOTAL", typeof(int));

            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridSituacion.dsDatos.Tables.Add(dt);
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
                    MessageBox.Show("El ejercicio no tiene un formato válido (aa o aaaa)", "Error");   //Falta traducir
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
                    MessageBox.Show("El ejercicio no puede ser mayor que el año en curso", "Error");   //Falta traducir
                    this.txtEjercicio.Focus();
                    return (false);
                }
            }

            return (true);
        }

        /// <summary>
        /// Realiza la búsqueda de la situación actual delSII según los criterios seleccionados
        /// </summary>
        private void Buscar()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (this.FormValid())
                {
                    this.FillDataGrid();

                    if (this.tgGridSituacion != null && this.tgGridSituacion.Rows.Count > 0) this.tgGridSituacion.ClearSelection();
                    
                    //Grabar la petición
                    string valores = this.ValoresPeticion();

                    this.valoresFormulario.GrabarParametros(formCode, valores);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Devuelve la consulta dado los criterios de busqueda
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsulta()
        {
            string queryNoBaja = "";
            string filtroNoBaja = "";

            string queryBaja = "";
            string filtroBaja = "";

            string filtroCompania = "";

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
                            filtroCompania += " (";
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

                //Periodo
                string periodo = this.cmbPeriodo.Text.Trim();

                //Libro
                string libro = this.cmbLibro.SelectedValue.ToString();

                if (ejercicio == "" && periodo == "")
                {
                    query = "select CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1, count(*) as total ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "IVSII1 ";

                    string filtro = "";
                    if (filtroCompania != "") filtro += filtroCompania;
                    if (ejercicio != "") 
                        if (filtro != "") filtro += " and EJERS1 = '" + ejercicio + "'";
                        else filtro += " EJERS1 = '" + ejercicio + "'";
                    if (periodo != "")
                        if (filtro != "") filtro += " and PERIS1 = '" + periodo + "' ";
                        else filtro += " PERIS1 = '" + periodo + "' ";
                    if (libro != "0")
                        if (filtro != "") filtro += " and TDOCS1 = '" + libro + "' ";
                        else filtro += " TDOCS1 = '" + libro + "' ";
                    if (filtro != "") query += "where " + filtro;

                    query += "group by CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1 ";
                    query += "order by CIAFS1, EJERS1, PERIS1, TDOCS1";
                }
                else
                {
                    //Facturas No Bajas
                    queryNoBaja = "select CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1, count(*) as total ";
                    queryNoBaja += "from " + GlobalVar.PrefijoTablaCG + "IVSII1 ";
                    queryNoBaja += "where BAJAS1 = ' ' ";

                    if (filtroCompania != "") filtroNoBaja += " and  " + filtroCompania;
                    if (ejercicio != "") filtroNoBaja += " and EJERS1 = '" + ejercicio + "'";
                    if (periodo != "") filtroNoBaja += " and PERIS1 = '" + periodo + "' ";
                    if (libro != "0") filtroNoBaja += " and TDOCS1 = '" + libro + "' ";
                    queryNoBaja += filtroNoBaja;

                    queryNoBaja += "group by CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1 ";

                    //Facturas Bajas
                    queryBaja = "select CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1, count(*) as total ";
                    queryBaja += " from " + GlobalVar.PrefijoTablaCG + "IVSII1 ";
                    queryBaja += "where BAJAS1 = 'B' ";

                    if (filtroCompania != "") filtroBaja += " and " + filtroCompania;
                    if (ejercicio != "") filtroBaja += " and EJBAS1 = '" + ejercicio + "'";
                    if (periodo != "") filtroBaja += " and PEBAS1 = '" + periodo + "' ";
                    if (libro != "0") filtroBaja += " and TDOCS1 = '" + libro + "' ";
                    queryBaja += filtroBaja;
                    queryBaja += "group by CIAFS1, EJERS1, PERIS1, TDOCS1, STATS1, BAJAS1 ";

                    if (queryNoBaja != "" && queryBaja != "")
                    {
                        query = queryNoBaja + " UNION " + queryBaja;
                        query += "order by CIAFS1, EJERS1, PERIS1, TDOCS1";
                    }
                    else
                    {
                        if (queryNoBaja != "")
                        {
                            query = queryNoBaja;
                            query += "order by CIAFS1, EJERS1, PERIS1, TDOCS1";
                        }
                        else
                        {
                            query = queryBaja;
                            query += "order by CIAFS1, EJERS1, PERIS1, TDOCS1";
                        }
                    }
                }
            }
            catch (Exception ex) { query = ""; Log.Error(Utiles.CreateExceptionString(ex)); }
            return (query);
        }

        private void FillDataGrid()
        {
            IDataReader dr = null;
            try
            {
                if (this.tgGridSituacion.Rows.Count > 0)
                {
                    //Eliminar los resultados de la búsqueda anterior
                    this.tgGridSituacion.Visible = false;

                    try 
                    {
                        if (this.tgGridSituacion.dsDatos != null && this.tgGridSituacion.dsDatos.Tables != null &&
                            this.tgGridSituacion.dsDatos.Tables.Count > 0)
                        {
                            this.tgGridSituacion.dsDatos.Tables["Situacion"].Rows.Clear();
                            this.tgGridSituacion.dsDatos.Tables["DatosTabla"].Rows.Clear();
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                //Obtener la consulta
                string query = this.ObtenerConsulta();
                
                if (query != "")
                {
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    DataRow row;
                    int cont = 0;

                    string companiaActual = "";
                    string ejercicioActual = "";
                    string periodoActual = "";
                    string libroCodActual = "";
                    string estadoActual = "";
                    string bajaActual = "";
                    int totalActual = 0;

                    string compania = "";
                    string ejercicio = "";
                    string periodo = "";
                    string libroCod = "";
                    string estado = "";
                    string baja = "";

                    int totalFacturas = 0;
                    int totalPdtesEnvio = 0;
                    int totalIncorrectas = 0;
                    int totalAceptadaErrores = 0;
                    int totalCorrectas = 0;
                    int totalAnuladas = 0;

                    bool primera = true;
                    bool pdteProcesar = true;
                    while (dr.Read())
                    {
                        companiaActual = dr.GetValue(dr.GetOrdinal("CIAFS1")).ToString();
                        ejercicioActual = dr.GetValue(dr.GetOrdinal("EJERS1")).ToString();
                        periodoActual = dr.GetValue(dr.GetOrdinal("PERIS1")).ToString();
                        libroCodActual = dr.GetValue(dr.GetOrdinal("TDOCS1")).ToString();
                        estadoActual = dr.GetValue(dr.GetOrdinal("STATS1")).ToString().Trim();
                        bajaActual = dr.GetValue(dr.GetOrdinal("BAJAS1")).ToString();
                        totalActual = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("TOTAL")));

                        if (companiaActual != compania ||
                            ejercicioActual != ejercicio ||
                            periodoActual != periodo ||
                            libroCodActual != libroCod)
                        {
                            if (!primera)
                            {
                                //Escribir registro
                                row = this.tgGridSituacion.dsDatos.Tables["Situacion"].NewRow();

                                row["CIAFS1"] = compania;
                                row["EJERS1"] = ejercicio;
                                row["PERIS1"] = periodo;
                                row["TDOCS1"] = this.ObtenerDescripcionLibro(libroCod); 
                                row["Total"] = totalFacturas;
                                row["PdteEnvio"] = totalPdtesEnvio;
                                row["Correcta"] = totalCorrectas;
                                row["AceptadaErrores"] = totalAceptadaErrores;
                                row["Incorrecta"] = totalIncorrectas;
                                row["Anulada"] = totalAnuladas;

                                this.tgGridSituacion.dsDatos.Tables["Situacion"].Rows.Add(row);
                                cont++;

                                totalFacturas = 0;
                                totalPdtesEnvio = 0;
                                totalCorrectas = 0;
                                totalAnuladas = 0;
                                totalAceptadaErrores = 0;
                                totalIncorrectas = 0;
                            }
                        }

                        compania = companiaActual;
                        ejercicio = ejercicioActual;
                        periodo = periodoActual;
                        libroCod = libroCodActual;
                        estado = estadoActual;
                        baja = bajaActual;
                        totalFacturas += totalActual;

                        //Acumular
                        switch (estadoActual)
                        {
                            case "":
                                totalPdtesEnvio += totalActual;
                                break;
                            case "V":
                                if (bajaActual == "B") totalAnuladas += totalActual;
                                else totalCorrectas += totalActual;
                                break;
                            case "W":
                                totalAceptadaErrores += totalActual;
                                break;
                            case "E":
                                totalIncorrectas += totalActual;
                                break;
                        }
                        primera = false;
                        pdteProcesar = true;
                    }

                    if (pdteProcesar && !primera)
                    {
                        //Escribir el ultimo registro
                        //Escribir registro
                        row = this.tgGridSituacion.dsDatos.Tables["Situacion"].NewRow();

                        row["CIAFS1"] = compania;
                        row["EJERS1"] = ejercicio;
                        row["PERIS1"] = periodo;
                        row["TDOCS1"] = this.ObtenerDescripcionLibro(libroCod);
                        row["Total"] = totalFacturas;
                        row["PdteEnvio"] = totalPdtesEnvio;
                        row["Correcta"] = totalCorrectas;
                        row["AceptadaErrores"] = totalAceptadaErrores;
                        row["Incorrecta"] = totalIncorrectas;
                        row["Anulada"] = totalAnuladas;

                        this.tgGridSituacion.dsDatos.Tables["Situacion"].Rows.Add(row);
                        cont++;
                    }

                    if (cont > 0)
                    {
                        this.lblResult.Visible = false;

                        //Ningún registro seleccionado
                        this.tgGridSituacion.ClearSelection();
                        this.tgGridSituacion.Refresh();
                        this.tgGridSituacion.Visible = true;
                        this.toolStripButtonExportar.Enabled = true;
                    }
                    else
                    {
                        this.lblResult.Text = "No existen facturas para el criterio de selección indicado";
                        this.lblResult.Visible = true;
                        this.tgGridSituacion.Visible = false;
                        this.toolStripButtonExportar.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Actualiza los controles (habilitado/deshabilitado, caption, ...) dado el libro seleccionado
        /// </summary>
        private void ActualizarControlesFromLibro()
        {
            string libro = this.cmbLibro.SelectedValue.ToString();

            try
            {
                //Habilitar o Deshabilitar campo Periodo
                switch (libro)
                {
                    case LibroUtiles.LibroID_BienesInversion:       //Bienes de Inversión
                    case LibroUtiles.LibroID_CobrosMetalico:        //Cobros en Metálico
                    case LibroUtiles.LibroID_OperacionesSeguros:    //Operaciones de Seguros
                    case LibroUtiles.LibroID_AgenciasViajes:        //Agencias de Viajes
                        if (periodosArray.Count > 0 && ((AddValue)this.periodosArray[periodosArray.Count - 1]).Value.ToString() != LibroUtiles.PeriodoAnual)
                        {
                            periodosArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));
                            this.cmbPeriodo.DataSource = null;
                            this.cmbPeriodo.DataSource = this.periodosArray;
                            this.cmbPeriodo.DisplayMember = "Display";
                            this.cmbPeriodo.ValueMember = "Value";
                        }
                        this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                        this.cmbPeriodo.Enabled = false;
                        this.txtEjercicio.Enabled = true;
                        break;
                    case "0":
                        if (periodosArray.Count > 0 && ((AddValue)this.periodosArray[periodosArray.Count - 1]).Value.ToString() != LibroUtiles.PeriodoAnual)
                        {
                            periodosArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));
                            this.cmbPeriodo.DataSource = null;
                            this.cmbPeriodo.DataSource = this.periodosArray;
                            this.cmbPeriodo.DisplayMember = "Display";
                            this.cmbPeriodo.ValueMember = "Value";
                        }
                        else
                        {
                            if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedIndex == periodosArray.Count - 1) this.cmbPeriodo.SelectedIndex = 0;
                        }
                        this.cmbPeriodo.Enabled = true;
                        break;
                    default:
                        if (this.cmbPeriodo.SelectedIndex != 0 && ((AddValue)this.periodosArray[periodosArray.Count - 1]).Value.ToString() == LibroUtiles.PeriodoAnual)
                        {
                            if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedIndex == periodosArray.Count - 1) this.cmbPeriodo.SelectedIndex = 0;

                            int cantElementos = periodosArray.Count;
                            this.periodosArray.RemoveAt(cantElementos - 1);
                            this.cmbPeriodo.DataSource = null;
                            this.cmbPeriodo.DataSource = this.periodosArray;
                            this.cmbPeriodo.DisplayMember = "Display";
                            this.cmbPeriodo.ValueMember = "Value";
                        }
                        this.cmbPeriodo.Enabled = true;
                        this.txtEjercicio.Enabled = true;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Exportar la Grid de situación general del SII a un fichero HTML que se visualizará en Excel 
        /// </summary>
        private void GridExportar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                /*this.grBoxProgressBar.Text = this.LP.GetText("lblCompContExportando", "Exportando");
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();
                */
                ExcelExportImport excelImport = new ExcelExportImport();
                excelImport.DateTableDatos = this.tgGridSituacion.dsDatos.Tables["Situacion"];

                //Titulo
                excelImport.Titulo = this.Text;
                excelImport.Cabecera = true;

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridSituacion.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridSituacion.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";
                    nombreTipoVisible[2] = this.tgGridSituacion.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.tgGridSituacion.SelectedRows.Count > 0 && this.tgGridSituacion.SelectedRows.Count < this.tgGridSituacion.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.tgGridSituacion.SelectedRows.Count; i++)
                    {
                        indice = this.tgGridSituacion.SelectedRows[i].Index;

                        if (tgGridSituacion.Rows.Count - 1 == indice)
                        {
                            //Linea Totales
                            if (aIndice.Count > 0)
                            {
                                aIndice.Add(indice);
                            }
                            else
                            {
                                //Solo linea de Totales, no se adiciona y se exportan todas las filas
                            }
                        }
                        else aIndice.Add(indice);
                    }

                    excelImport.IndiceFilasSeleccionadas = aIndice;
                }

                string result = excelImport.ExportarMemoria();

                //this.progressBarEspera.Visible = false;
                //this.grBoxProgressBar.Visible = false;

                if (result != "" && result != "CANCELAR")
                {
                    MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + result + ")", error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", error);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exportar la Grid de situación general del SII a un fichero HTML que se visualizará en Excel 
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string titulo = "Situación General SII";

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridSituacion.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridSituacion.Columns[i].HeaderText;                   //Nombre de la columna

                    switch (this.tgGridSituacion.Columns[i].Name)
                    {
                        case "EJERS1":
                        case "Total":
                        case "PdteEnvio":
                        case "Correcta":
                        case "AceptadaErrores":
                        case "Incorrecta":
                        case "Anulada":
                            nombreTipoVisible[1] = "numero";
                            nombreTipoVisible[2] = this.tgGridSituacion.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = this.tgGridSituacion.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridSituacion);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIISituacionGral");

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
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
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

                    List<string> columnNoVisible = new List<string>(new string[] { "cmbLibro", "cmbEjercicio", "chkCiasFiscalesTodas", 
                                                                                   "lbCiasFiscales", "txtElemento" });

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
                IntPtr pBuf = Marshal.StringToBSTR(valores.PadRight(109,' '));
                StructGLL01_MISIISIGRL myStruct = (StructGLL01_MISIISIGRL)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIISIGRL));

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio;

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim(); 
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.libro.Trim() != "") this.cmbLibro.SelectedValue = myStruct.libro.Trim();
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
                StructGLL01_MISIISIGRL myStruct;

                myStruct.ejercicio = this.txtEjercicio.Text.PadRight(4, ' ');
                myStruct.periodo = this.cmbPeriodo.SelectedValue.ToString().PadRight(2, ' ');
                myStruct.libro = this.cmbLibro.SelectedValue.ToString().PadRight(2, ' ');

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

                result = myStruct.ejercicio + myStruct.periodo + myStruct.libro + myStruct.todasCompanias;
                result += myStruct.companias ;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void frmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion();
            frmPeticion.FormCode = this.formCode;
            frmPeticion.FicheroExtension = this.ficheroExtension;
            frmPeticion.Formulario = this;
            string result = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));
        }
        #endregion

        #endregion
    }
}
