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
    public partial class frmSiiDatosLocal : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIIDATL";
        public string ficheroExtension = "cdl";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIIDATL
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string nombreRazonSocial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string noFactura;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaExpedicionDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string claveOperacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaExpedicionHasta;
        }

        FormularioValoresCampos valoresFormulario;

        private ArrayList periodoArray;
        private ArrayList librosArray;
        private ArrayList tipoIdentificacionArray;
        private ArrayList claveOperacionArray;
        private ArrayList paisArray;

        private DataSet dsConsultaRespuesta;
        private BindingSource bindingConsultaRespuesta;

        //private int newSortColumn;
        //private ListSortDirection newColumnDirection = ListSortDirection.Ascending;

        private string codigoCompania = "";
        private string ejercicioCG = "";
        private string tipoPeriodoImpositivo = "";

        private string consultaFiltroEstado = "";
        private string consultaFiltroFechaDesde = "";
        private string consultaFiltroFechaHasta = "";

        public frmSiiDatosLocal()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiDatosLocal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta Datos Local");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
            this.tgTexBoxSelCiaFiscal.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelCiaFiscal_ValueChanged);

            //Crear el desplegable de Periodos
            //this.CrearComboPeriodos();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el desplegable de Pais
            this.CrearComboPais(false, "");

            //Crear el desplegable de Tipo de Identificación (Destinatario/Emisor/...)
            this.CrearComboTipoIdentificacion();

            //Crear el desplegable de Clave Operacion (A/B/...)
            this.CrearComboClaveOperacion();

            //Crear el TGGrid
            this.BuiltgConsultaInfo();

            //Construir el DataSet con el resultado del envio
            this.dsConsultaRespuesta = new DataSet();
            this.bindingConsultaRespuesta = new BindingSource();

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

        /// <summary>
        /// Método donde se hará uso de los parámetro del argumento del evento ValueChangedCommandEventArgs
        /// </summary>
        /// <param name="e"></param>
        private void tgTexBoxSelCiaFiscal_ValueChanged(TGTexBoxSel.ValueChangedCommandEventArgs e)
        {
            if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "")
            {
                //Deshabilitar los controles del formulario, al haber introducido la compañía
                //this.ControlesHabilitarDeshabilitar(false);

                //No existe valor para la compañía
                //this.codigoCompania = "";
            }
            else
            {
                //Habilitar los controles del formulario, al haber introducido la compañía
                //this.ControlesHabilitarDeshabilitar(true);

                this.tgTexBoxSelCiaFiscal.Textbox.Focus();

                this.tgTexBoxSelCiaFiscal.Textbox.Modified = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Consultar();
        }

        private void tgGridConsulta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string columnName = this.tgGridConsulta.Columns[e.ColumnIndex].Name;

                    if (columnName == "VER" || columnName == "MOV" || columnName == "PAGO")
                    {
                        //int rowIndex = this.tgGridConsulta.CurrentCell.RowIndex;

                        FacturaIdentificador facturaID = new FacturaIdentificador();                        
                        /*facturaID.EmisorFacturaNIF = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDNIF"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROCodigoPais"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROIdType"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROId"].Value.ToString();
                        */
                        facturaID.EmisorFacturaNIF = this.tgGridConsulta.SelectedRows[0].Cells["IDNIF"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROCodigoPais"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROIdType"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROId"].Value.ToString();

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.SelectedRows[0].Cells["NumSerieFacturaEmisor"].Value.ToString();
                        else facturaID.NumeroSerie = "";

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.SelectedRows[0].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                        else facturaID.FechaDocumento = "";

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) facturaID.CargoAbono = this.tgGridConsulta.SelectedRows[0].Cells["CargoAbono"].Value.ToString();
                        else facturaID.CargoAbono = "";

                        string iDEmisorFactura;
                        string libro = this.cmbLibro.SelectedValue.ToString();

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.SelectedRows[0].Cells["IDEmisorFactura"].Value.ToString();
                        else iDEmisorFactura = "";

                        string periodoActual = "";
                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("Periodo")) periodoActual = this.tgGridConsulta.SelectedRows[0].Cells["Periodo"].Value.ToString();

                        switch (columnName)
                        {
                            case "VER":
                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_FacturasEmitidas:
                                        this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                        break;
                                    case LibroUtiles.LibroID_FacturasRecibidas:
                                        this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                        break;
                                    case LibroUtiles.LibroID_BienesInversion:
                                        this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                        break;
                                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                                        this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
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
                            case "PAGO":
                                string nombreRazonSocial = "";
                                if (this.dsConsultaRespuesta.Tables["MasInfo"].Columns.Contains("ContraparteNombreRazonSocial"))
                                {
                                    //Buscar la factura (por si se ordenó por columnas)
                                    string filtro = "(Compania='" + this.codigoCompania + "') AND ";
                                    filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                                    //filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";
                                    filtro += "(Periodo='" + periodoActual + "') AND ";
                                    filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                                    filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                                    filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') AND ";
                                    filtro += "(CargoAbono ='" + facturaID.CargoAbono + "') ";

                                    try
                                    {
                                        if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo") && this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                                        {
                                            DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Select(filtro);

                                            if (rows.Length == 1)
                                            {
                                                nombreRazonSocial = rows[0]["ContraparteNombreRazonSocial"].ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(Utiles.CreateExceptionString(ex));
                                    }
                                }
                                frmSiiConsultaPagosRecibidas frmPago = new frmSiiConsultaPagosRecibidas();
                                frmPago.Compania = this.codigoCompania;
                                frmPago.FacturaID = facturaID;
                                frmPago.IDEmisorFactura = iDEmisorFactura;
                                frmPago.NombreRazonSocial = nombreRazonSocial;
                                frmPago.DatosLocal = true;
                                frmPago.Show();
                                break;
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void tgGridConsulta_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //int rowIndex = this.tgGridConsulta.CurrentCell.RowIndex;

                    FacturaIdentificador facturaID = new FacturaIdentificador();
                    facturaID.EmisorFacturaNIF = this.tgGridConsulta.SelectedRows[0].Cells["IDNIF"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROCodigoPais"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROIdType"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROId"].Value.ToString();

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.SelectedRows[0].Cells["NumSerieFacturaEmisor"].Value.ToString();
                    else facturaID.NumeroSerie = "";

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.SelectedRows[0].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                    else facturaID.FechaDocumento = "";

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) facturaID.CargoAbono = this.tgGridConsulta.SelectedRows[0].Cells["CargoAbono"].Value.ToString();
                    else facturaID.CargoAbono = "";

                    string iDEmisorFactura;
                    string libro = this.cmbLibro.SelectedValue.ToString();

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.SelectedRows[0].Cells["IDEmisorFactura"].Value.ToString();
                    else iDEmisorFactura = "";

                    string periodoActual = "";
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("Periodo")) periodoActual = this.tgGridConsulta.SelectedRows[0].Cells["Periodo"].Value.ToString();


                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                            this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                            break;
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void tgGridConsulta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridConsulta.Rows.Count > 0)
            {
                //Ocultar las columnas necesarias

                this.tgGridConsulta.Rows[0].Selected = false;

                if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales"))
                {
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDNIF")) this.tgGridConsulta.Columns["IDNIF"].Visible = false;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDOTROCodigoPais")) this.tgGridConsulta.Columns["IDOTROCodigoPais"].Visible = false;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDOTROIdType")) this.tgGridConsulta.Columns["IDOTROIdType"].Visible = false;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDOTROId")) this.tgGridConsulta.Columns["IDOTROId"].Visible = false;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) this.tgGridConsulta.Columns["CargoAbono"].Visible = false;
                    //if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("SumaImporte")) this.tgGridConsulta.Columns["SumaImporte"].Visible = false;

                    //Alinear a la derecha la columnas de importes
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ImporteTotal")) 
                        this.tgGridConsulta.Columns["ImporteTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    else if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("PagoImporte")) 
                        this.tgGridConsulta.Columns["PagoImporte"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("BaseImponible")) this.tgGridConsulta.Columns["BaseImponible"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("Cuota")) this.tgGridConsulta.Columns["Cuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CuotaDeducible")) this.tgGridConsulta.Columns["CuotaDeducible"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    string libro = this.cmbLibro.SelectedValue.ToString();

                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("TipoFactura")) this.tgGridConsulta.Columns["TipoFactura"].Visible = false;
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ClaveRegimenEspecialOTrascendencia")) this.tgGridConsulta.Columns["ClaveRegimenEspecialOTrascendencia"].Visible = false;
                            break;
                        case LibroUtiles.LibroID_PagosRecibidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("PagoMedio")) this.tgGridConsulta.Columns["PagoMedio"].Visible = false;
                            break;
                        case LibroUtiles.LibroID_CobrosEmitidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CobroMedio")) this.tgGridConsulta.Columns["CobroMedio"].Visible = false;
                            break;
                        case LibroUtiles.LibroID_CobrosMetalico:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            break;
                        case LibroUtiles.LibroID_OperacionesSeguros:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ClaveOperacion")) this.tgGridConsulta.Columns["ClaveOperacion"].Visible = false;
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            break;
                        case LibroUtiles.LibroID_AgenciasViajes:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            break;
                    }

                    /*
                    //Marcar las filas de las facturas que no suman de color gris y en negrita
                    //Posibles registros para actualizar el estado
                    var rowsUpdate = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].AsEnumerable().Cast<DataRow>().Where(row =>
                                                                            (row["SumaImporte"].ToString() == "N")).ToArray();

                    //Loop through and remove the rows that meet the condition
                    int indice = 0;

                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.Font = new Font(this.tgGridConsulta.Font, FontStyle.Bold);
                    style.ForeColor = Color.DarkGray;

                    foreach (DataRow dr in rowsUpdate)
                    {
                        //Mostrar todos los registros                        
                        indice = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows.IndexOf(dr);
                        this.tgGridConsulta.Rows[indice].DefaultCellStyle = style;
                        this.tgGridConsulta.Rows[indice].ReadOnly = false;
                    }
                     */ 
                }
            }
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
                else
                {
                    //Crear el desplegable de Periodos
                    this.CrearComboPeriodos();
                }
            }
            else
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "" && periodoArray != null && periodoArray.Count > 1)
                {
                    //Eliminar periodos, falta seleccionar la compañía
                    this.periodoArray.Clear();

                    this.cmbPeriodo.DataSource = null;
                    this.cmbPeriodo.DataBindings.Clear();
                    this.cmbPeriodo.Refresh();
                }
            Cursor.Current = Cursors.Default;
        }

        private void cmbLibro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();
        }

        private void rbNIF_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbNIF.Checked)
            {
                this.lblNIF.Text = "NIF Destinatario";
                this.txtNIF.MaxLength = 9;
                this.lblCodPais.Enabled = false;
                this.cmbPais.Enabled = false;
                this.lblTipoIden.Enabled = false;
                this.cmbTipoIdentif.Enabled = false;
                this.lblNombreRazon.Enabled = false;
                this.txtNombreRazon.Enabled = false;
                this.cmbTipoIdentif.SelectedValue = "";
                this.cmbPais.SelectedValue = "";

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
                this.lblNombreRazon.Enabled = true;
                this.txtNombreRazon.Enabled = true;

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

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            //this.GridExportar();  //Exporta Directamente a Excel

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
            this.txtNombreRazon.Text = "";
            this.txtNumSerieFactura.Text = "";
            this.txtMaskFechaExpedicionDesde.Text = "";
            this.txtMaskFechaExpedicionHasta.Text = "";
            this.cmbClaveOperacion.Text = "";
            this.rbEstadoTodas.Checked = true;
            this.gbResultado.Visible = false;
            this.tgGridConsulta.Visible = false;
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void txtEjercicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtEjercicio, false, ref sender, ref e);
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

        private void frmSiiDatosLocal_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiDatosLocal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta Datos Local");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiDatosLocalTitulo", "Consulta Datos Local");
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
                string periodoActual = "";

                if (this.cmbPeriodo.SelectedValue != null)
                {
                    periodoActual = this.cmbPeriodo.SelectedValue.ToString();
                }
                
                periodoArray = new ArrayList();
                periodoArray.Add(new AddValue("", ""));

                if (this.tipoPeriodoImpositivo == "T")
                {
                    periodoArray.Add(new AddValue("1T", "1T"));
                    periodoArray.Add(new AddValue("2T", "2T"));
                    periodoArray.Add(new AddValue("3T", "3T"));
                    periodoArray.Add(new AddValue("4T", "4T"));
                }
                else
                {
                    periodoArray.Add(new AddValue("01", "01"));
                    periodoArray.Add(new AddValue("02", "02"));
                    periodoArray.Add(new AddValue("03", "03"));
                    periodoArray.Add(new AddValue("04", "04"));
                    periodoArray.Add(new AddValue("05", "05"));
                    periodoArray.Add(new AddValue("06", "06"));
                    periodoArray.Add(new AddValue("07", "07"));
                    periodoArray.Add(new AddValue("08", "08"));
                    periodoArray.Add(new AddValue("09", "09"));
                    periodoArray.Add(new AddValue("10", "10"));
                    periodoArray.Add(new AddValue("11", "11"));
                    periodoArray.Add(new AddValue("12", "12"));
                    periodoArray.Add(new AddValue(LibroUtiles.PeriodoAnual, LibroUtiles.PeriodoAnual));
                }

                this.cmbPeriodo.DataSource = periodoArray;
                this.cmbPeriodo.DisplayMember = "Display";
                this.cmbPeriodo.ValueMember = "Value";

                //this.cmbPeriodo.SelectedIndex = 0;
                this.cmbPeriodo.SelectedValue = periodoActual;
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
                //librosArray.Add(new AddValue(textoValor5, LibroUtiles.LibroID_CobrosEmitidas));         //Cobro Emitidas
                librosArray.Add(new AddValue(textoValor6, LibroUtiles.LibroID_PagosRecibidas));         //Pagos Recibidas
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
                string valorSel = "";
                try
                {
                    if (this.cmbTipoIdentif.SelectedValue != null) valorSel = this.cmbTipoIdentif.SelectedValue.ToString();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                tipoIdentificacionArray = new ArrayList();
                tipoIdentificacionArray.Add(new AddValue("", ""));
                tipoIdentificacionArray.Add(new AddValue("NIF-IVA", "02"));
                tipoIdentificacionArray.Add(new AddValue("Pasaporte", "03"));
                tipoIdentificacionArray.Add(new AddValue("Documento oficial de identificación expedido por el país o territorio de residencia", "04"));
                tipoIdentificacionArray.Add(new AddValue("Certificado de residencia", "05"));
                tipoIdentificacionArray.Add(new AddValue("Otro documento probatorio", "06"));

                string libro = "";
                if (this.cmbLibro.SelectedValue != null) libro = this.cmbLibro.SelectedValue.ToString();

                switch (libro)
                {
                    case LibroUtiles.LibroID_FacturasEmitidas:
                    case LibroUtiles.LibroID_OperacionesSeguros:
                    case LibroUtiles.LibroID_CobrosMetalico:
                        tipoIdentificacionArray.Add(new AddValue("No censado", "07"));
                        break;
                }

                this.cmbTipoIdentif.DataSource = tipoIdentificacionArray;
                this.cmbTipoIdentif.DisplayMember = "Display";
                this.cmbTipoIdentif.ValueMember = "Value";

                try
                {
                    this.cmbTipoIdentif.SelectedValue = valorSel;
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Clave Operacion (Operaciones de Seguros)
        /// </summary>
        private void CrearComboClaveOperacion()
        {
            try
            {
                claveOperacionArray = new ArrayList();
                claveOperacionArray.Add(new AddValue("", ""));
                claveOperacionArray.Add(new AddValue("Indem. o prest. satisfechas sup. a 3005,06", "A"));
                claveOperacionArray.Add(new AddValue("Primas o contrap. percibidas sup. a 3005,06", "B"));

                this.cmbClaveOperacion.DataSource = claveOperacionArray;
                this.cmbClaveOperacion.DisplayMember = "Display";
                this.cmbClaveOperacion.ValueMember = "Value";

                this.cmbClaveOperacion.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuiltgConsultaInfo()
        {
            //Crear el DataGrid
            this.tgGridConsulta.dsDatos = new DataSet();
            this.tgGridConsulta.dsDatos.DataSetName = "Consulta";
            this.tgGridConsulta.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridConsulta.ReadOnly = true;
            this.tgGridConsulta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridConsulta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridConsulta.AllowUserToAddRows = false;
            this.tgGridConsulta.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridConsulta.NombreTabla = "Consulta";
        }

        private bool FormValid(string libro)
        {
            try
            {
                this.txtNumSerieFactura.Text = this.txtNumSerieFactura.Text.Trim();

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
                this.txtMaskFechaExpedicionDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaDesde = this.txtMaskFechaExpedicionDesde.Text.Trim();
                this.txtMaskFechaExpedicionDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (this.consultaFiltroFechaDesde != "")
                {
                    string fechaStr = this.txtMaskFechaExpedicionDesde.Text;
                    bool fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaDesdeDateTime);
                    if (fechaStr != this.txtMaskFechaExpedicionDesde.Text) this.txtMaskFechaExpedicionDesde.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de expedición desde no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaExpedicionDesde.Focus();
                        return (false);
                    }
                }

                this.consultaFiltroFechaHasta = "";
                DateTime fechaHastaDateTime =  new DateTime();
                //coger el valor sin la máscara
                this.txtMaskFechaExpedicionHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaHasta = this.txtMaskFechaExpedicionHasta.Text.Trim();
                this.txtMaskFechaExpedicionHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (this.consultaFiltroFechaHasta != "")
                {
                    string fechaStr = this.txtMaskFechaExpedicionHasta.Text;
                    bool fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaHastaDateTime);
                    if (fechaStr != this.txtMaskFechaExpedicionHasta.Text) this.txtMaskFechaExpedicionHasta.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de expedición hasta no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaExpedicionHasta.Focus();
                        return (false);
                    }
                }

                if (consultaFiltroFechaDesde != "" && consultaFiltroFechaHasta != "")
                {
                    if (fechaDesdeDateTime > fechaHastaDateTime)
                    {
                        MessageBox.Show("La fecha de expedición desde no puede ser posterior a la fecha de expedición hasta", "Error");   //Falta traducir
                        this.txtMaskFechaExpedicionDesde.Focus();
                        return (false);
                    }
                }

                /*
                switch (libro)
                {
                    case LibroUtiles.LibroID_PagosRecibidas:
                    case LibroUtiles.LibroID_CobrosEmitidas:
                        if (this.txtNIF.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el NIF del destinatario", "Error");   //Falta traducir
                            this.txtNIF.Focus();
                            return (false);
                        }

                        if (this.rbOtro.Checked && this.cmbTipoIdentif.SelectedValue.ToString() == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Tipo de Identificación del destinatario", "Error");   //Falta traducir
                            this.cmbTipoIdentif.Focus();
                            return (false);
                        }

                        if (this.txtNombreRazon.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Nombre o Razón Social del destinatario", "Error");   //Falta traducir
                            this.txtNombreRazon.Focus();
                            return (false);
                        }

                        if (this.txtNumSerieFactura.Text == "")
                        {
                            MessageBox.Show("Es obligatorio informar el Número de Serie de la Factura", "Error");   //Falta traducir
                            this.txtNumSerieFactura.Focus();
                            return (false);
                        }

                        if (this.consultaFiltroFechaDesde == "")
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
                        else
                        {
                            this.txtEjercicio.Text = ejercicio;
                            if (ejercicio.Length == 4) this.ejercicioCG = ejercicio.Substring(2, 2);
                        }

                        //Periodo
                        if (this.cmbPeriodo.SelectedIndex == 0)
                        {
                            MessageBox.Show("Debe indicar el periodo", "Error");   //Falta traducir
                            this.cmbPeriodo.Focus();
                            return (false);
                        }
                        break;
                }
                */

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
                else
                {
                    this.txtEjercicio.Text = ejercicio;
                    if (ejercicio.Length == 4) this.ejercicioCG = ejercicio.Substring(2, 2);
                }

                //Periodo
                if (this.cmbPeriodo.SelectedIndex == 0 && libro != LibroUtiles.LibroID_PagosRecibidas &&
                    this.consultaFiltroFechaDesde == "" && this.consultaFiltroFechaHasta == "" &&
                    this.txtNumSerieFactura.Text == "" && this.txtNIF.Text.Trim() == "" &&
                    this.cmbPais.SelectedValue.ToString() == "" && this.cmbTipoIdentif.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Si no se informa periodo, se debe indicar algún criterio adicional de búsqueda", "Error");   //Falta traducir
                    this.cmbPeriodo.Focus();
                    return (false);
                }

                this.consultaFiltroEstado = this.ObtenerEstado();

                if (this.consultaFiltroFechaDesde != "") this.consultaFiltroFechaDesde = this.txtMaskFechaExpedicionDesde.Text;

                if (this.consultaFiltroFechaHasta != "") this.consultaFiltroFechaHasta = this.txtMaskFechaExpedicionHasta.Text;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (true);
        }

        /// <summary>
        /// Devuelve si el DataSet con los resultados de la consulta sólo contiene datos en la tabla de errores
        /// </summary>
        /// <returns></returns>
        private bool SoloExisteTablaErrores()
        {
            bool result = false;

            try
            {
                if (this.dsConsultaRespuesta.Tables.Contains("Resultado") && this.dsConsultaRespuesta.Tables["Resultado"].Rows.Count > 0)
                {
                    for (int i = 0; i < this.dsConsultaRespuesta.Tables.Count; i++)
                    {
                        if (this.dsConsultaRespuesta.Tables[i].TableName != "Resultado")
                            if (this.dsConsultaRespuesta.Tables[i].Rows.Count > 0)
                            {
                                break;
                            }
                    }

                    result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Si se produce un error en la petición de la consulta, se visualizará un registro en la tabla Resultado
        /// </summary>
        private void ConsultaVerError()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                {
                    //Eliminar los resultados de la búsqueda anterior o los datos generales
                    this.tgGridConsulta.Visible = false;

                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridConsulta.dsDatos.Tables.Remove("DatosGenerales");
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("Resultado")) this.tgGridConsulta.dsDatos.Tables.Remove("Resultado");
                }

                //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resultado"].Copy());

                //Eliminar columnas
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Compania")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Compania");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Libro")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Libro");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Operacion")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Operacion");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NIFIdEmisor")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NIFIdEmisor");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NoFactura")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NoFactura");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("FechaDoc")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("FechaDoc");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("RowResumen")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("RowResumen");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NombreRazonSocial")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NombreRazonSocial");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("ClaveOperacion")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("ClaveOperacion");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NIF")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NIF");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroCodPais")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroCodPais");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroTipo")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroTipo");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroId")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroId");

                this.tgGridConsulta.AutoGenerateColumns = true;

                //Poner como DataSource del DataGrid el DataTable creado
                this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["Resultado"];

                //Cambiar los headers de las columnas del DataGrid de Error
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Estado")) this.tgGridConsulta.CambiarColumnHeader("Estado", "Error");  //Falta traducir

                this.tgGridConsulta.Refresh();

                this.tgGridConsulta.Visible = true;
                this.lblNoInfo.Visible = false;
                this.toolStripButtonExportar.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        //Adiciona la columna VER (llama a mostrar los datos de la factura)
        private void AdicionarColumnaVerTablaDatosGenerales()
        {
            try
            {
                System.Data.DataColumn columnaVer = new System.Data.DataColumn("VER", typeof(System.Drawing.Bitmap));
                columnaVer.DefaultValue = global::ModSII.Properties.Resources.Buscar;

                System.Data.DataColumn columnaMov = new System.Data.DataColumn("MOV", typeof(System.Drawing.Bitmap));
                columnaMov.DefaultValue = global::ModSII.Properties.Resources.Movimientos;

                string codLibro = this.cmbLibro.SelectedValue.ToString();
                switch (codLibro)
                {
                    case LibroUtiles.LibroID_FacturasRecibidas:
                        //Adicionar la columna Ver al DataTable de Datos Generales
                        //this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("VER", typeof(System.Drawing.Image));
                        //this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("MOV", typeof(System.Drawing.Image));

                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaVer);
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaMov);

                        bool existeClaveRegimenEspecial07 = false;
                        string claveRegimenEspecial07 = "";

                        System.Data.DataColumn columnaPago = new System.Data.DataColumn("PAGO", typeof(System.Drawing.Bitmap));
                        columnaPago.DefaultValue = null;
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaPago);
                        //this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("PAGO", typeof(System.Drawing.Image));

                        for (int i = 0; i < this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count; i++)
                        {
                            //this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["Ver"] = global::ModSII.Properties.Resources.Buscar;
                            //this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["MOV"] = global::ModSII.Properties.Resources.Movimientos;

                            claveRegimenEspecial07 = this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["ClaveRegimenEspecialOTrascendencia"].ToString();

                            if (claveRegimenEspecial07 == "07")
                            {
                                existeClaveRegimenEspecial07 = true;
                                this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["PAGO"] = global::ModSII.Properties.Resources.PagosCobros;
                            }
                            //else this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["PAGO"] = null;
                        }

                        try
                        {
                            if (!existeClaveRegimenEspecial07) this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Remove("PAGO");
                        }
                        catch { }
                        break;
                    case LibroUtiles.LibroID_FacturasEmitidas:
                    case LibroUtiles.LibroID_BienesInversion:
                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaVer);
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaMov);
                        /*
                        System.Data.DataColumn newColumn = new System.Data.DataColumn("VER", typeof(System.Drawing.Bitmap));
                        newColumn.DefaultValue = global::ModSII.Properties.Resources.Buscar;
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(newColumn);

                        newColumn = new System.Data.DataColumn("MOV", typeof(System.Drawing.Bitmap));
                        newColumn.DefaultValue = global::ModSII.Properties.Resources.Movimientos;
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(newColumn);
                        */
                        /*this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("VER", typeof(System.Drawing.Image));
                        this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("MOV", typeof(System.Drawing.Image));
                        for (int i = 0; i < this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count; i++)
                        {
                            this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["Ver"] = global::ModSII.Properties.Resources.Buscar;
                            this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["MOV"] = global::ModSII.Properties.Resources.Movimientos;
                        }*/
                        break;
                    default:
                        if (!(codLibro == LibroUtiles.LibroID_PagosRecibidas || codLibro == LibroUtiles.LibroID_CobrosMetalico || codLibro == LibroUtiles.LibroID_OperacionesSeguros || codLibro == LibroUtiles.LibroID_AgenciasViajes))
                        {
                            //Adicionar la columna Ver al DataTable de Datos Generales
                            this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add(columnaMov);
                            /*this.dsConsultaRespuesta.Tables["DatosGenerales"].Columns.Add("MOV", typeof(System.Drawing.Image));
                            for (int i = 0; i < this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count; i++)
                                this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows[i]["MOV"] = global::ModSII.Properties.Resources.Movimientos;
                             */ 
                        }
                        break;
                }
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
                this.tgGridConsulta.Visible = false;
                this.gbResultado.Visible = false;
                this.lblNoInfo.Visible = false;

                string libro = this.cmbLibro.SelectedValue.ToString();
                if (this.FormValid(libro))
                {
                    this.lblInfo.Visible = true;
                    this.lblInfo.Update();

                    //Eliminar los resultados de la búsqueda anteriorde la grid
                    if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                    {
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridConsulta.dsDatos.Tables.Remove("DatosGenerales");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo")) this.tgGridConsulta.dsDatos.Tables.Remove("MasInfo");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleIVA")) this.tgGridConsulta.dsDatos.Tables.Remove("DetalleIVA");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleExenta")) this.tgGridConsulta.dsDatos.Tables.Remove("DetalleExenta");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resumen")) this.tgGridConsulta.dsDatos.Tables.Remove("Resumen");
                    }

                    //Eliminar todas las tablas del dataset
                    if (this.dsConsultaRespuesta != null && this.dsConsultaRespuesta.Tables != null && this.dsConsultaRespuesta.Tables.Count > 0)
                    {
                        this.dsConsultaRespuesta.Tables.Clear();
                        this.dsConsultaRespuesta.Clear();
                    }

                    /*
                    bool eliminarTablas = false;
                    while (this.dsConsultaRespuesta.Tables.Count > 0)
                    {
                        DataTable table = this.dsConsultaRespuesta.Tables[0];
                        if (this.dsConsultaRespuesta.Tables.CanRemove(table))
                        {
                            table.Rows.Clear();
                            this.dsConsultaRespuesta.Tables.Remove(table);
                            eliminarTablas = true;
                        }
                    }

                    if (eliminarTablas)
                    {
                        this.dsConsultaRespuesta.Tables.Clear();
                        this.dsConsultaRespuesta.Clear();
                    }
                    */

                    string periodo = this.cmbPeriodo.SelectedValue.ToString();
                    string codPais = "";
                    string codTipoIdent = "";
                    if (this.cmbPais.SelectedValue != null) codPais = this.cmbPais.SelectedValue.ToString();
                    if (this.cmbTipoIdentif.SelectedValue != null) codTipoIdent = this.cmbTipoIdentif.SelectedValue.ToString();

                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionFacturasEmitidas(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent, this.agencia);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionFacturasRecibidas(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent, this.agencia);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.dsConsultaRespuesta = this.ConsultaInformacionBienesInversion(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                            this.dsConsultaRespuesta = this.ConsultaInformacionDetOperacionesIntracomunitarias(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                        case LibroUtiles.LibroID_PagosRecibidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionPagosRecibidas(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                        case LibroUtiles.LibroID_CobrosMetalico:
                            this.dsConsultaRespuesta = this.ConsultaInformacionCobrosMetalico(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                        case LibroUtiles.LibroID_OperacionesSeguros:
                            this.dsConsultaRespuesta = this.ConsultaInformacionOperacionesSeguros(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                        case LibroUtiles.LibroID_CobrosEmitidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionCobrosEmitidas(this.codigoCompania);
                            break;
                        case LibroUtiles.LibroID_AgenciasViajes:
                            this.dsConsultaRespuesta = this.ConsultaInformacionAgenciasViajes(this.codigoCompania, this.ejercicioCG, periodo, codPais, codTipoIdent);
                            break;
                    }

                    //Grabar la petición
                    string valores = this.ValoresPeticion();

                    this.valoresFormulario.GrabarParametros(formCode, valores);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;

            this.lblInfo.Visible = false;
            this.lblInfo.Update();
        }

        #region ----- Facturas Emitidas -----
        /// <summary>
        /// Invoca la consulta de las facturas emitidas enviadas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionFacturasEmitidas(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent, string agencia)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroFacturasExpedidas libroFactExpedidas = new LibroFacturasExpedidas(Log, utiles, LP, agencia);
                this.dsConsultaRespuesta = libroFactExpedidas.ObtenerDatosFacturasEmitidas(this.codigoCompania, this.ejercicioCG, periodo,
                                                                                           this.txtNIF.Text, codPais, codTipoIdent, this.txtNombreRazon.Text,
                                                                                           this.txtNumSerieFactura.Text, 
                                                                                           this.consultaFiltroFechaDesde, this.consultaFiltroFechaHasta,
                                                                                           this.consultaFiltroEstado, this.agencia);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DetalleIVA"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DetalleExenta"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;
                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */
                        //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                        this.CambiarColumnasEncabezadosFacturasEmitidas();

                        //Salian en un orden erroneo, asi fuerzo que aparezcan al final
                        int totalColumnas = this.tgGridConsulta.Columns.Count;
                        this.tgGridConsulta.Columns["VER"].DisplayIndex = totalColumnas - 2;
                        this.tgGridConsulta.Columns["MOV"].DisplayIndex = totalColumnas - 1;

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            this.lblCuotaTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = true;
                            this.lblBaseImponibleTotal.Visible = true;
                            this.lblCuotaTotalValor.Visible = true;
                            this.lblCuotaTotal.Visible = true;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen facturas emitidas que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen facturas emitidas que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas Enviadas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturasEmitidas()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDDestinatario")) this.tgGridConsulta.CambiarColumnHeader("IDDestinatario", "ID destinatario");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TipoFacturaDesc")) this.tgGridConsulta.CambiarColumnHeader("TipoFacturaDesc", "Tipo Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ClaveRegimenEspecialOTrascendenciaDesc")) this.tgGridConsulta.CambiarColumnHeader("ClaveRegimenEspecialOTrascendenciaDesc", "Clave Regimen Especial O Trascendencia");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionOperacion")) this.tgGridConsulta.CambiarColumnHeader("DescripcionOperacion", "Descripción Operación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("BaseImponible")) this.tgGridConsulta.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridConsulta.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampUltimaModificacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("VER")) this.tgGridConsulta.CambiarColumnHeader("VER", "Ver Detalle");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("SumaImporte")) this.tgGridConsulta.CambiarColumnHeader("SumaImporte", "Acumula Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RefExterna")) this.tgGridConsulta.CambiarColumnHeader("RefExterna", "Referencia Externa");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la factura emitida enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewFacturaEmitida(int indice, string iDEmisorFactura, FacturaIdentificador facturaID, string periodoActual)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactEmitida frmViewFacturaEnviada = new frmSiiConsultaViewFactEmitida();
                if (this.ejercicioCG.Length == 2) frmViewFacturaEnviada.Ejercicio = "20" + this.ejercicioCG;
                else frmViewFacturaEnviada.Ejercicio = this.ejercicioCG;
                //frmViewFacturaEnviada.Periodo = this.cmbPeriodo.SelectedValue.ToString(); 
                frmViewFacturaEnviada.Periodo = periodoActual;
                frmViewFacturaEnviada.FacturaID = facturaID;
                frmViewFacturaEnviada.DatosLocal = true;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(Compania='" + this.codigoCompania + "') AND ";
                filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                //filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";
                filtro += "(Periodo='" + periodoActual + "') AND ";
                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') AND ";
                filtro += "(CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales") && this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Select(filtro);

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
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo") && this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Select(filtro);

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

                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleIVA") && this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Select(filtro);

                        if (rows.Length != 0) dtIVA = rows.CopyToDataTable();
                    }
                    frmViewFacturaEnviada.DtDetallesIVA = dtIVA;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.DtDetallesIVA = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    DataTable dtExenta = null;

                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleExenta") && this.tgGridConsulta.dsDatos.Tables["DetalleExenta"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DetalleExenta"].Select(filtro);

                        if (rows.Length != 0) dtExenta = rows.CopyToDataTable();
                    }
                    frmViewFacturaEnviada.DtDetallesExenta = dtExenta;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.DtDetallesExenta = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaEnviada.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Facturas Recibidas -----
        /// <summary>
        /// Invoca la consulta de las facturas recibidas enviadas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionFacturasRecibidas(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent, string agencia)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroFacturasRecibidas libFactRecibidas = new LibroFacturasRecibidas(Log, utiles, LP, agencia);
                this.dsConsultaRespuesta = libFactRecibidas.ObtenerDatosFacturasRecibidas(this.codigoCompania, this.ejercicioCG, 
                                                                                          periodo, this.txtNIF.Text,
                                                                                          codPais, codTipoIdent, 
                                                                                          this.txtNombreRazon.Text, this.txtNumSerieFactura.Text, 
                                                                                          this.consultaFiltroFechaDesde, this.consultaFiltroFechaHasta,
                                                                                          this.consultaFiltroEstado, this.agencia);
                
                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DetalleIVA"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }

                        //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                        this.CambiarColumnasEncabezadosFacturasRecibidas();

                        //Salian en un orden erroneo, asi fuerzo que aparezcan al final
                        int totalColumnas = this.tgGridConsulta.Columns.Count;
                        if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PAGO"))
                        {
                            this.tgGridConsulta.Columns["VER"].DisplayIndex = totalColumnas - 3;
                            this.tgGridConsulta.Columns["MOV"].DisplayIndex = totalColumnas - 2;
                            this.tgGridConsulta.Columns["PAGO"].DisplayIndex = totalColumnas - 1;
                        }
                        else
                        {
                            this.tgGridConsulta.Columns["VER"].DisplayIndex = totalColumnas - 2;
                            this.tgGridConsulta.Columns["MOV"].DisplayIndex = totalColumnas - 1;
                        }

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            this.lblCuotaTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = true;
                            this.lblBaseImponibleTotal.Visible = true;
                            this.lblCuotaTotalValor.Visible = true;
                            this.lblCuotaTotal.Visible = true;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen facturas recibidas que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen facturas recibidas que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturasRecibidas()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TipoFacturaDesc")) this.tgGridConsulta.CambiarColumnHeader("TipoFacturaDesc", "Tipo Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ClaveRegimenEspecialOTrascendenciaDesc")) this.tgGridConsulta.CambiarColumnHeader("ClaveRegimenEspecialOTrascendenciaDesc", "Clave Regimen Especial O Trascendencia");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionOperacion")) this.tgGridConsulta.CambiarColumnHeader("DescripcionOperacion", "Descripción Operación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("BaseImponible")) this.tgGridConsulta.CambiarColumnHeader("BaseImponible", "Base Imponible");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CuotaDeducible")) this.tgGridConsulta.CambiarColumnHeader("CuotaDeducible", "Cuota");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridConsulta.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampUltimaModificacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("VER")) this.tgGridConsulta.CambiarColumnHeader("VER", "Ver Detalle");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PAGO")) this.tgGridConsulta.CambiarColumnHeader("PAGO", "Ver Pagos");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("SumaImporte")) this.tgGridConsulta.CambiarColumnHeader("SumaImporte", "Acumula Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RefExterna")) this.tgGridConsulta.CambiarColumnHeader("RefExterna", "Referencia Externa");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la factura recibida enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewFacturaRecibida(int indice, string iDEmisorFactura, FacturaIdentificador facturaID, string periodoActual)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactRecibida frmViewFacturaRecibida = new frmSiiConsultaViewFactRecibida();
                if (this.ejercicioCG.Length == 2) frmViewFacturaRecibida.Ejercicio = "20" + this.ejercicioCG;
                else frmViewFacturaRecibida.Ejercicio = this.ejercicioCG;
                //frmViewFacturaRecibida.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewFacturaRecibida.Periodo = periodoActual;
                frmViewFacturaRecibida.FacturaID = facturaID;
                frmViewFacturaRecibida.DatosLocal = true;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(Compania='" + this.codigoCompania + "') AND ";
                filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                //filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";
                filtro += "(Periodo='" + periodoActual + "') AND ";
                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') AND ";
                filtro += "(CargoAbono ='" + facturaID.CargoAbono + "') ";

                /*
                int fechaCG;
                if (facturaID.FechaDocumento != "")
                {
                    fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(facturaID.FechaDocumento), true);
                    if (fechaCG != -1) filtro += "AND (FechaExpedicionFacturaEmisor ='" + fechaCG + "')";
                }
                */

                try
                {
                    DataRow rowDatosGrles = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales") && this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Select(filtro);

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
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo") && this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Select(filtro);

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

                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleIVA") && this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Select(filtro);

                        if (rows.Length != 0) dtIVA = rows.CopyToDataTable();
                    }
                    frmViewFacturaRecibida.DtDetallesIVA = dtIVA;
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.DtDetallesIVA = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaRecibida.Show();


                /*
                string compania = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows[indice]["Compania"].ToString();
                string ejercicio = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows[indice]["Ejercicio"].ToString();
                string periodo = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows[indice]["Periodo"].ToString();

                //Visualizar la información
                frmSiiConsultaViewFactRecibida frmViewFacturaRecibida = new frmSiiConsultaViewFactRecibida();
                if (ejercicio.Length == 2) frmViewFacturaRecibida.Ejercicio = "20" + ejercicio;
                else frmViewFacturaRecibida.Ejercicio = ejercicio;
                frmViewFacturaRecibida.Periodo = periodo;
                frmViewFacturaRecibida.FacturaID = facturaID;
                frmViewFacturaRecibida.DatosLocal = true;

                try
                {
                    frmViewFacturaRecibida.RowDatosGrles = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows[indice];
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowDatosGrles = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    frmViewFacturaRecibida.RowMasInfo = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows[indice];
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                try
                {
                    string filtro = "(Compania='" + compania + "') AND ";
                    filtro += "(Ejercicio='" + ejercicio + "') AND ";
                    filtro += "(Periodo='" + periodo + "') AND ";
                    filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                    filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') ";

                    int fechaCG;
                    if (facturaID.FechaDocumento != "")
                    {
                        fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(facturaID.FechaDocumento), true);
                        if (fechaCG != -1) filtro += "AND (FechaExpedicionFacturaEmisor ='" + fechaCG + "')";
                    }

                    try
                    {
                        DataTable dtIVA = null;

                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleIVA") && this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Rows.Count > 0)
                        {
                            DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DetalleIVA"].Select(filtro);
                            
                            if (rows.Length != 0 )  dtIVA = rows.CopyToDataTable();
                        }
                        frmViewFacturaRecibida.DtDetallesIVA = dtIVA;
                    }
                    catch (Exception ex)
                    {
                        frmViewFacturaRecibida.DtDetallesIVA = null;
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }
                }
                catch (Exception ex)
                {
                    frmViewFacturaRecibida.RowMasInfo = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaRecibida.Show();
                */ 

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Determinadas Operaciones Intracomunitarias -----
        /// <summary>
        /// Invoca la consulta de las operaciones intracomunitarias enviadas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionDetOperacionesIntracomunitarias(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroDetOperacIntracomunitarias libOperacIntracomunitarias = new LibroDetOperacIntracomunitarias(Log, utiles, LP);
                this.dsConsultaRespuesta = libOperacIntracomunitarias.ObtenerDatosOperacIntracomunitarias(
                                                                                          this.codigoCompania, this.ejercicioCG,
                                                                                          periodo, this.txtNIF.Text,
                                                                                          codPais, codTipoIdent,
                                                                                          this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                                          this.consultaFiltroFechaDesde, this.consultaFiltroFechaHasta, 
                                                                                          this.consultaFiltroEstado, this.agencia);

                
                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */

                        //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                        this.CambiarColumnasEncabezadosDetOperacionesIntracomunitarias();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            //this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = false;
                            this.lblImporteTotal.Visible = false;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen operaciones intracomunitarias que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen operaciones intracomunitarias que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Detminadas Operaciones Intracomunitarias
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosDetOperacionesIntracomunitarias()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Destinatario / Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Contraparte Nombre Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridConsulta.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampUltimaModificacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("VER")) this.tgGridConsulta.CambiarColumnHeader("VER", "Ver Detalle");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RefExterna")) this.tgGridConsulta.CambiarColumnHeader("RefExterna", "Referencia Externa");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza la operación intracomunitaria enviada seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewOperacionIntracomunitaria(int indice, string iDEmisorFactura, FacturaIdentificador facturaID, string periodoActual)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewDetOperacIntracomunitaria frmViewOperacionIntracomunitaria = new frmSiiConsultaViewDetOperacIntracomunitaria();
                frmViewOperacionIntracomunitaria.Ejercicio = this.txtEjercicio.Text;
                //frmViewOperacionIntracomunitaria.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewOperacionIntracomunitaria.Periodo = periodoActual;
                frmViewOperacionIntracomunitaria.FacturaID = facturaID;
                frmViewOperacionIntracomunitaria.DatosLocal = true;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(Compania='" + this.codigoCompania + "') AND ";
                filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                //filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";
                filtro += "(Periodo='" + periodoActual + "') AND ";
                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') AND ";
                filtro += "(CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales") && this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Select(filtro);

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
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo") && this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Select(filtro);

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
        #endregion

        #region ----- Bienes de Inversión -----
        /// <summary>
        /// Invoca la consulta de bienes de inversión enviadas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionBienesInversion(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroBienesInversion libBienesInversion = new LibroBienesInversion(Log, utiles, LP);
                this.dsConsultaRespuesta = libBienesInversion.ObtenerDatosBienesInversion(this.codigoCompania, this.ejercicioCG,
                                                                                          periodo, this.txtNIF.Text,
                                                                                          codPais, codTipoIdent,
                                                                                          this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                                          this.consultaFiltroFechaDesde, this.consultaFiltroFechaHasta,
                                                                                          this.consultaFiltroEstado, this.agencia);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;
                        
                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */
                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosBienesInversion();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;
                        
                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            //this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = false;
                            this.lblImporteTotal.Visible = false;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen bienes de inversión que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen bienes de inversión que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Bienes de Inversión
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosBienesInversion()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IdentificacionBien")) this.tgGridConsulta.CambiarColumnHeader("IdentificacionBien", "Identificación Bien");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaInicioUtilizacion")) this.tgGridConsulta.CambiarColumnHeader("FechaInicioUtilizacion", "Fecha Inicio Utilización");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ProrrataAnualDefinitiva")) this.tgGridConsulta.CambiarColumnHeader("ProrrataAnualDefinitiva", "Prorrata Anual Definitiva");  //Falta 
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RegulacionAnualDeduccion")) this.tgGridConsulta.CambiarColumnHeader("RegulacionAnualDeduccion", "Regulación Anual Deducción");  //Falta 
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RegularizacionDeduccionEfectuada")) this.tgGridConsulta.CambiarColumnHeader("RegularizacionDeduccionEfectuada", "Regularización Deducción Efectuada");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridConsulta.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir         
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampUltimaModificacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampUltimaModificacion", "Fecha/Hora Última Modificación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("VER")) this.tgGridConsulta.CambiarColumnHeader("VER", "Ver Detalle");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("RefExterna")) this.tgGridConsulta.CambiarColumnHeader("RefExterna", "Referencia Externa");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llama al formulario que visualiza el bien inversión enviado seleccionada
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="iDEmisorFactura"></param>
        /// <param name="facturaID"></param>
        private void ConsultaViewBienInversion(int indice, string iDEmisorFactura, FacturaIdentificador facturaID, string periodoActual)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewBienInversion frmViewBienInversion = new frmSiiConsultaViewBienInversion();
                frmViewBienInversion.Ejercicio = this.txtEjercicio.Text;
                //frmViewBienInversion.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewBienInversion.Periodo = periodoActual;
                frmViewBienInversion.FacturaID = facturaID;
                frmViewBienInversion.DatosLocal = true;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(Compania='" + this.codigoCompania + "') AND ";
                filtro += "(Ejercicio='" + this.ejercicioCG + "') AND ";
                //filtro += "(Periodo='" + this.cmbPeriodo.SelectedValue.ToString() + "') AND ";
                filtro += "(Periodo='" + periodoActual + "') AND ";
                filtro += "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') AND ";
                filtro += "(CargoAbono ='" + facturaID.CargoAbono + "') ";

                try
                {
                    DataRow rowDatosGrles = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales") && this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Select(filtro);

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
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo") && this.tgGridConsulta.dsDatos.Tables["MasInfo"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["MasInfo"].Select(filtro);

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
        #endregion

        #region ----- Pagos Recibidas -----
        /// <summary>
        /// Invoca la consulta de pagos recibidas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionPagosRecibidas(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroPagoRecibidas libPagosRecibidas = new LibroPagoRecibidas(Log, utiles, LP);
                this.dsConsultaRespuesta = libPagosRecibidas.ObtenerDatosPagosRecibidas(this.codigoCompania, this.ejercicioCG, periodo,
                                                                                        this.txtNombreRazon.Text, this.txtNIF.Text,
                                                                                        codPais, codTipoIdent,
                                                                                        this.txtNumSerieFactura.Text,
                                                                                        this.consultaFiltroFechaDesde, this.consultaFiltroFechaHasta,
                                                                                        this.agencia);
                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosPagosRecibidas();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen pagos recibidas que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen pagos recibidas que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Pagos Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosPagosRecibidas()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PagoFecha")) this.tgGridConsulta.CambiarColumnHeader("PagoFecha", "Fecha de Pago");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PagoImporte")) this.tgGridConsulta.CambiarColumnHeader("PagoImporte", "Importe");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PagoMedioDesc")) this.tgGridConsulta.CambiarColumnHeader("PagoMedioDesc", "Medio de Pago");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("PagoCuentaOMedio")) this.tgGridConsulta.CambiarColumnHeader("PagoCuentaOMedio", "Cuenta o Medio");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NIFPresentador")) this.tgGridConsulta.CambiarColumnHeader("NIFPresentador", "NIF Presentador");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampPresentacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampPresentacion", "Fecha y Hora");  //Falta traducir         
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Cobros en Metálico -----
        /// <summary>
        /// Invoca la consulta de cobros en metálico de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionCobrosMetalico(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroCobrosMetalico libCobrosMetalico = new LibroCobrosMetalico(Log, utiles, LP);
                this.dsConsultaRespuesta = libCobrosMetalico.ObtenerDatosCobrosMetalico(this.codigoCompania, this.ejercicioCG,
                                                                                          periodo, this.txtNIF.Text,
                                                                                          codPais, codTipoIdent,
                                                                                          this.txtNombreRazon.Text, this.consultaFiltroEstado,
                                                                                          this.agencia);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */
                        
                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosCobrosMetalico();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen cobros en metálico que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen cobros en metálico que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Cobros en Metálico
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosCobrosMetalico()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("SumaImporte")) this.tgGridConsulta.CambiarColumnHeader("SumaImporte", "Acumula Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNIF")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNIF", "Entidad Sucedida NIF");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNombreRazonSocial")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNombreRazonSocial", "Entidad Sucedida Nombre Razón SocialF");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Operaciones de Seguros -----
        /// <summary>
        /// Invoca la consulta de operaciones de seguros de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionOperacionesSeguros(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroOperacionesSeguros libOperacionesSeguros = new LibroOperacionesSeguros(Log, utiles, LP);
                this.dsConsultaRespuesta = libOperacionesSeguros.ObtenerDatosOperacionesSeguros(
                                                                                          this.codigoCompania, this.ejercicioCG,
                                                                                          periodo, this.txtNIF.Text,
                                                                                          codPais, codTipoIdent,
                                                                                          this.txtNombreRazon.Text, this.consultaFiltroEstado,
                                                                                          this.cmbClaveOperacion.SelectedValue.ToString(),
                                                                                          this.agencia);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                   if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                       /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                       */
                       
                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosOperacionesSeguros();

                        //Salian en un orden erroneo, asi fuerzo que aparezcan al final
                        //int totalColumnas = this.tgGridConsulta.Columns.Count;
                        //this.tgGridConsulta.Columns["MOV"].DisplayIndex = totalColumnas - 1;

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen operaciones de seguros que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen operaciones de seguros que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Operaciones de Seguros
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosOperacionesSeguros()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ClaveOperacionDesc")) this.tgGridConsulta.CambiarColumnHeader("ClaveOperacionDesc", "Clave Operación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("SumaImporte")) this.tgGridConsulta.CambiarColumnHeader("SumaImporte", "Acumula Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNIF")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNIF", "Entidad Sucedida NIF");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNombreRazonSocial")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNombreRazonSocial", "Entidad Sucedida Nombre Razón SocialF");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Cobros Emitidas -----
        /// <summary>
        /// Invoca la consulta de cobros emitidas de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionCobrosEmitidas(string compania)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                /*
                tgSIIWebService.TGsiiService wsTGsii = new tgSIIWebService.TGsiiService();
                this.dsConsultaRespuesta = wsTGsii.ConsultaLRCobrosEmitidas(this.codigoCompania, ejercicioCG, "",
                                                                            this.txtNIF.Text, this.txtNumSerieFactura.Text,
                                                                            this.consultaFiltroFechaDesde, this.consultaFiltroEstado);

                 * */
                //OJO FALRA programar dicho libro 1!!
                /*
                LibroCobrosEmitidas libCobrosEmitidas = new LibroCobrosEmitidas(Log, utiles);
                this.dsConsultaRespuesta = libCobrosEmitidas.ObtenerDatosOperacionesSeguros(
                                                                                          this.codigoCompania, this.ejercicioCG,
                                                                                          periodo, this.txtNIF.Text,
                                                                                          this.txtCodPais.Text, this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                                          this.txtNombreRazon.Text, this.consultaFiltroEstado);

                */

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                    {
                        //Eliminar los resultados de la búsqueda anterior
                        this.tgGridConsulta.Visible = false;

                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridConsulta.dsDatos.Tables.Remove("DatosGenerales");
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */

                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosCobrosEmitidas();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen cobros emitidas que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen cobros emitidas que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Cobros Emitidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosCobrosEmitidas()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CobroFecha")) this.tgGridConsulta.CambiarColumnHeader("CobroFecha", "Fecha de Cobro");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CobroImporte")) this.tgGridConsulta.CambiarColumnHeader("CobroImporte", "Importe");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CobroMedioDesc")) this.tgGridConsulta.CambiarColumnHeader("CobroMedioDesc", "Medio de Cobro");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CobroCuentaOMedio")) this.tgGridConsulta.CambiarColumnHeader("CobroCuentaOMedio", "Cuenta o Medio");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NIFPresentador")) this.tgGridConsulta.CambiarColumnHeader("NIFPresentador", "NIF Presentador");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TimestampPresentacion")) this.tgGridConsulta.CambiarColumnHeader("TimestampPresentacion", "Fecha y Hora");  //Falta traducir         
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        #region ----- Agencias Viajes -----
        /// <summary>
        /// Invoca la consulta de agencias de viajes de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionAgenciasViajes(string compania, string ejercicio, string periodo, string codPais, string codTipoIdent)
        {
            this.dsConsultaRespuesta = null;
            try
            {
                LibroAgenciasViajes libAgenciasViajes = new LibroAgenciasViajes(Log, utiles, LP);
                this.dsConsultaRespuesta = libAgenciasViajes.ObtenerDatosAgenciasViajes(this.codigoCompania, this.ejercicioCG,
                                                                                        periodo, this.txtNIF.Text,
                                                                                        codPais, codTipoIdent,
                                                                                        this.txtNombreRazon.Text, this.consultaFiltroEstado,
                                                                                        this.agencia);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */

                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosAgenciasViajes();

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            //this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            //this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = false;
                            this.lblBaseImponibleTotal.Visible = false;
                            this.lblCuotaTotalValor.Visible = false;
                            this.lblCuotaTotal.Visible = false;
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen agencias de viajes que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen agencias de viajes que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Agencias de Viajes
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosAgenciasViajes()
        {
            try
            {
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("Compania")) this.tgGridConsulta.CambiarColumnHeader("Compania", "Compañía");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("SumaImporte")) this.tgGridConsulta.CambiarColumnHeader("SumaImporte", "Acumula Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNIF")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNIF", "Entidad Sucedida NIF");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNombreRazonSocial")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNombreRazonSocial", "Entidad Sucedida Nombre Razón SocialF");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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
                if (this.rbEstadoTodas.Checked) estado = "T";
                else if (this.rbEstadoPdteEnvio.Checked) estado = " ";
                else if (this.rbEstadoSoloErrores.Checked) estado = "E";
                else if (this.rbEstadoAceptadoErrores.Checked) estado = "W";
                else if (this.rbEstadoAnulada.Checked) estado = "B";
                else estado = "V";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            return (estado);
        }

        /// <summary>
        /// Actualiza los controles (habilitado/deshabilitado, caption, ...) dado el libro seleccionado
        /// </summary>
        private void ActualizarControlesFromLibro()
        {
            switch (this.cmbLibro.SelectedValue.ToString())
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = false;
                    this.lblCodPais.Text = "Cód. País Destinatario";
                    this.lblTipoIden.Text = "Tipo Identif. Dest.";
                    this.lblNombreRazon.Text = "Nombre o Razón Social Dest.";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Dest.";
                    }
                    else this.lblNIF.Text = "NIF Destinatario";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicionDesde.Enabled = true;
                    this.txtMaskFechaExpedicionDesde.Enabled = true;
                    this.lblFechaExpedicionHasta.Enabled = true;
                    this.txtMaskFechaExpedicionHasta.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
                case LibroUtiles.LibroID_FacturasRecibidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Emisor";
                    this.lblNombreRazon.Text = "Nombre o Razón Social Emisor";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Emisor";
                    }
                    else this.lblNIF.Text = "NIF Emisor";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicionDesde.Enabled = true;
                    this.txtMaskFechaExpedicionDesde.Enabled = true;
                    this.lblFechaExpedicionHasta.Enabled = true;
                    this.txtMaskFechaExpedicionHasta.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
                case LibroUtiles.LibroID_BienesInversion:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Emisor";
                    this.lblNombreRazon.Text = "Nombre o Razón Social Emisor";
                    if (this.rbOtro.Checked)
                    {
                        this.lblTipoIden.Enabled = true;
                        this.cmbTipoIdentif.Enabled = true;
                        this.lblNIF.Text = "Identificación Emisor";
                    }
                    else this.lblNIF.Text = "NIF Emisor";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicionDesde.Enabled = true;
                    this.txtMaskFechaExpedicionDesde.Enabled = true;
                    this.lblFechaExpedicionHasta.Enabled = true;
                    this.txtMaskFechaExpedicionHasta.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    this.rbOtro.Text = "NIF-IVA";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Dest./Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Dest./Emisor";

                    if (this.rbOtro.Checked) this.cmbTipoIdentif.SelectedValue = "02";
                    this.lblTipoIden.Enabled = false;
                    this.cmbTipoIdentif.Enabled = false;
                    this.lblNombreRazon.Text = "Nombre o Razón Soc. Dest./Emisor";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;

                    if (this.rbOtro.Checked) this.lblNIF.Text = "Identif. Dest./Emisor";
                    else this.lblNIF.Text = "NIF Dest./Emisor";

                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicionDesde.Enabled = true;
                    this.txtMaskFechaExpedicionDesde.Enabled = true;
                    this.lblFechaExpedicionHasta.Enabled = true;
                    this.txtMaskFechaExpedicionHasta.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    this.rbEstadoTodas.Checked = true;
                    this.gbEstadoFacturas.Enabled = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
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
                    this.lblNombreRazon.Text = "Nombre o Razón Soc. Contrap.";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicionDesde.Enabled = false;
                    this.txtMaskFechaExpedicionDesde.Text = "";
                    this.txtMaskFechaExpedicionDesde.Enabled = false;
                    this.lblFechaExpedicionHasta.Enabled = false;
                    this.txtMaskFechaExpedicionHasta.Text = "";
                    this.txtMaskFechaExpedicionHasta.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicionDesde.Enabled = false;
                    this.txtMaskFechaExpedicionDesde.Text = "";
                    this.txtMaskFechaExpedicionDesde.Enabled = false;
                    this.lblFechaExpedicionHasta.Enabled = false;
                    this.txtMaskFechaExpedicionHasta.Text = "";
                    this.txtMaskFechaExpedicionHasta.Enabled = false;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = true;
                    this.cmbClaveOperacion.Visible = true;
                    break;
                case LibroUtiles.LibroID_CobrosEmitidas:
                    this.rbOtro.Text = "Otro";
                    if (this.rbOtro.Checked) this.rbNIF.Checked = true;
                    this.gbTipoIdentificacion.Enabled = false;
                    this.lblNIF.Text = "NIF Emisor";
                    this.lblNombreRazon.Enabled = false;
                    this.txtNombreRazon.Enabled = false;
                    this.txtEjercicio.Text = "";
                    this.cmbPeriodo.SelectedIndex = 0;
                    this.txtEjercicio.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.rbEstadoTodas.Checked = true;
                    this.gbEstadoFacturas.Enabled = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
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
                    this.lblNombreRazon.Text = "Nombre o Razón Soc. Contrap.";
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicionDesde.Enabled = false;
                    this.txtMaskFechaExpedicionDesde.Text = "";
                    this.txtMaskFechaExpedicionDesde.Enabled = false;
                    this.lblFechaExpedicionHasta.Enabled = false;
                    this.txtMaskFechaExpedicionHasta.Text = "";
                    this.txtMaskFechaExpedicionHasta.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    break;
            }

            //Actualizar los elementos del desplegable de Tipos de Identificación (el elemento No censado solo está disponible para los libros
            //de Facturas Emitidas, Operaciones de seguros y de Cobros en Metálico)
            this.CrearComboTipoIdentificacion();
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
                StructGLL01_MISIIDATL myStruct = (StructGLL01_MISIIDATL)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIIDATL));

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
                    else
                    {
                        //Crear el desplegable de Periodos
                        this.CrearComboPeriodos();
                    }
                }

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio;

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.porNifIdOtro == "1")
                {
                    this.rbNIF.Checked = true;
                    this.rbOtro.Checked = false;
                }
                else
                {
                    this.rbNIF.Checked = false;
                    this.rbOtro.Checked = true;
                }

                if (myStruct.nif.Trim() != "") this.txtNIF.Text = myStruct.nif;

                try
                {
                    if (myStruct.pais.Trim() != "") this.cmbPais.SelectedValue = myStruct.pais.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.tipoIdentificacion.Trim() != "") this.cmbTipoIdentif.SelectedValue = myStruct.tipoIdentificacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.nombreRazonSocial.Trim() != "") this.txtNombreRazon.Text = myStruct.nombreRazonSocial;

                if (myStruct.noFactura.Trim() != "") this.txtNumSerieFactura.Text = myStruct.noFactura.Trim();

                if (myStruct.fechaExpedicionDesde.Trim() != "") this.txtMaskFechaExpedicionDesde.Text = myStruct.fechaExpedicionDesde;

                switch (myStruct.estado)
                {
                    case " ":
                        this.rbEstadoPdteEnvio.Checked = true;
                        break;
                    case "E":
                        this.rbEstadoSoloErrores.Checked = true;
                        break;
                    case "W":
                        this.rbEstadoAceptadoErrores.Checked = true;
                        break;
                    case "V":
                        this.rbEstadoCorrecto.Checked = true;
                        break;
                    default:        //Todas
                        this.rbEstadoTodas.Checked = true;
                        break;
                }

                try
                {
                    if (myStruct.claveOperacion.Trim() != "") this.cmbClaveOperacion.SelectedValue = myStruct.claveOperacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.fechaExpedicionHasta.Trim() != "") this.txtMaskFechaExpedicionHasta.Text = myStruct.fechaExpedicionHasta;

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
                StructGLL01_MISIIDATL myStruct;

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

                myStruct.nombreRazonSocial = this.txtNombreRazon.Text.PadRight(40, ' ');

                myStruct.noFactura = this.txtNumSerieFactura.Text.PadRight(25, ' ');

                myStruct.fechaExpedicionDesde = this.txtMaskFechaExpedicionDesde.Text.PadRight(10, ' ');

                myStruct.estado = this.ObtenerEstado();

                if (this.cmbClaveOperacion.Visible)
                {
                    if (this.cmbClaveOperacion.SelectedValue != null) myStruct.claveOperacion = this.cmbClaveOperacion.SelectedValue.ToString().PadRight(1, ' ');
                    else myStruct.claveOperacion = cadenaVacia.PadRight(1, ' ');
                }
                else myStruct.claveOperacion = cadenaVacia.PadRight(1, ' ');

                myStruct.fechaExpedicionHasta = this.txtMaskFechaExpedicionHasta.Text.PadRight(10, ' ');

                result = myStruct.libro + myStruct.compania + myStruct.ejercicio + myStruct.periodo + myStruct.porNifIdOtro + myStruct.nif;
                result += myStruct.pais + myStruct.tipoIdentificacion + myStruct.nombreRazonSocial + myStruct.noFactura + myStruct.fechaExpedicionDesde;
                result += myStruct.estado + myStruct.claveOperacion + myStruct.fechaExpedicionHasta;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

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
                    dictControles.Add("Nombre o Razón Social", "txtNombreRazon");
                    dictControles.Add("País", "txtCodPais");
                    dictControles.Add("Tipo Identificacion", "cmbTipoIdentif");
                    dictControles.Add("Estado Todas", "rbEstadoTodas");
                    dictControles.Add("Estado PendienteEnvio", "rbEstadoPdteEnvio");
                    dictControles.Add("Estado SoloErrores", "rbEstadoSoloErrores");
                    dictControles.Add("Estado Correcto", "rbEstadoCorrecto");
                    dictControles.Add("Estado AceptadoErrores", "rbEstadoAceptadoErrores");

                    List<string> columnNoVisible = new List<string>(new string[] { "cmbTipoIdentif", "txtCodPais", "txtNombreRazon",
                                                                                   "txtNIF", "rbNIF", "rbOtro",
                                                                                   "rbEstadoTodas", "rbEstadoPdteEnvio", "rbEstadoSoloErrores",
                                                                                   "rbEstadoCorrecto", "rbEstadoAceptadoErrores",
                                                                                   "cmbLibro", "txtElemento"});

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

            //Actualiza Controles dado Libro
            this.ActualizarControlesFromLibro();

            this.tgGridConsulta.Visible = false;
            this.toolStripButtonExportar.Enabled = false;
        }

        #region Respuesta Envios
        /// <summary>
        /// Devuelve un DataTable con la estructura de la respuesta del WebService
        /// </summary>
        /// <returns></returns>
        public static DataTable CrearDataTableResultado()
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
                result.Columns.Add("NIFIdEmisor", typeof(string)); //Cabecera Grid: NIF/Id Emisor  (too vendra contraparte nif)
                result.Columns.Add("NoFactura", typeof(string));
                result.Columns.Add("FechaDoc", typeof(string));
                result.Columns.Add("NombreRazonSocial", typeof(string));        //Mostrar solo si es un libro donde se utiliza
                result.Columns.Add("ClaveOperacion", typeof(string));           //Mostrar solo para el libro de operaciones de seguro
                result.Columns.Add("NIF", typeof(string));
                result.Columns.Add("IdOtroCodPais", typeof(string));
                result.Columns.Add("IdOtroTipo", typeof(string));
                result.Columns.Add("IdOtroId", typeof(string));
                result.Columns.Add("RowResumen", typeof(string));
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

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
        public static void InsertarRegistroTableResultado(ref DataTable dt, string compania, string libro,
                                                          string operacion, string estado,
                                                          FacturaIdentificador facturaID,
                                                          bool resumenEnvio)
        {
            try
            {
                InsertarRegistroTableResultado(ref dt, compania, libro, operacion, estado, facturaID, "", "", resumenEnvio);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
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
        public static void InsertarRegistroTableResultado(ref DataTable dt, string compania, string libro,
                                                           string operacion, string estado,
                                                           FacturaIdentificador facturaID,
                                                           string nombreRazonSocial, string claveOperacion,
                                                           bool resumenEnvio)
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

                if (facturaID != null)
                {
                    row["NIF"] = facturaID.EmisorFacturaNIF;
                    row["IdOtroCodPais"] = facturaID.EmisorFacturaIdOtroCodPais;
                    row["IdOtroTipo"] = facturaID.EmisorFacturaIdOtroIdType;
                    row["IdOtroId"] = facturaID.EmisorFacturaIdOtroId;
                    row["NoFactura"] = facturaID.NumeroSerie;
                    row["FechaDoc"] = facturaID.FechaDocumento;

                    if (facturaID.EmisorFacturaNIF != "") row["NIFIdEmisor"] = facturaID.EmisorFacturaNIF;
                    else
                    {
                        if (facturaID.EmisorFacturaIdOtroCodPais != "") row["NIFIdEmisor"] = facturaID.EmisorFacturaIdOtroCodPais + "-" + facturaID.EmisorFacturaIdOtroIdType + "-" + facturaID.EmisorFacturaIdOtroId;
                        else row["NIFIdEmisor"] = facturaID.EmisorFacturaIdOtroIdType + "-" + facturaID.EmisorFacturaIdOtroId;
                    }
                }
                else
                {
                    row["NIF"] = "";
                    row["IdOtroCodPais"] = "";
                    row["IdOtroTipo"] = "";
                    row["IdOtroId"] = "";
                    row["NoFactura"] = "";
                    row["FechaDoc"] = "";
                    row["NIFIdEmisor"] = "";
                }

                row["NombreRazonSocial"] = nombreRazonSocial;
                row["ClaveOperacion"] = claveOperacion;
                row["RowResumen"] = (resumenEnvio == true) ? "1" : "0"; 

                dt.Rows.Add(row);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void GridExportar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                ExcelExportImport excelImport = new ExcelExportImport();
                excelImport.DateTableDatos = this.tgGridConsulta.dsDatos.Tables["DatosGenerales"];

                //Titulo
                excelImport.Titulo = this.Text;
                excelImport.Cabecera = true;

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridConsulta.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridConsulta.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";

                    switch (nombreTipoVisible[0])
                    {
                        case "Detalle":
                        case "Movimientos":
                        case "Pago":
                            nombreTipoVisible[2] = "0";   //No Visible
                            break;
                        default:
                            nombreTipoVisible[2] = this.tgGridConsulta.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.tgGridConsulta.SelectedRows.Count > 0 && this.tgGridConsulta.SelectedRows.Count < this.tgGridConsulta.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.tgGridConsulta.SelectedRows.Count; i++)
                    {
                        indice = this.tgGridConsulta.SelectedRows[i].Index;

                        if (tgGridConsulta.Rows.Count - 1 == indice)
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
        /// Exporta la consulta de Datos en Local a Excel, pasando por un fichero HTML
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Exportar la consulta de Datos en Local a Excel, pasando por un fichero HTML
                LibroUtiles.GridExportarHTML(ref this.tgGridConsulta, this.cmbLibro.SelectedValue.ToString(), "Local");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));
            }

            Cursor.Current = Cursors.Default;
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
        #endregion

        private void tgGridConsulta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Visualizar la factura seleccionada (igual que hacer click en la lupa, si la fila la tiene)

            string libro = this.cmbLibro.SelectedValue.ToString();

            switch (libro)
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                case LibroUtiles.LibroID_FacturasRecibidas:
                case LibroUtiles.LibroID_BienesInversion:
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    if (e.RowIndex > -1 && e.ColumnIndex > -1)
                    {
                        if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count)
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            FacturaIdentificador facturaID = new FacturaIdentificador();
                            facturaID.EmisorFacturaNIF = this.tgGridConsulta.SelectedRows[0].Cells["IDNIF"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROCodigoPais"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROIdType"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.SelectedRows[0].Cells["IDOTROId"].Value.ToString();

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.SelectedRows[0].Cells["NumSerieFacturaEmisor"].Value.ToString();
                            else facturaID.NumeroSerie = "";

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.SelectedRows[0].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                            else facturaID.FechaDocumento = "";

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CargoAbono")) facturaID.CargoAbono = this.tgGridConsulta.SelectedRows[0].Cells["CargoAbono"].Value.ToString();
                            else facturaID.CargoAbono = "";

                            string iDEmisorFactura;

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.SelectedRows[0].Cells["IDEmisorFactura"].Value.ToString();
                            else iDEmisorFactura = "";

                            string periodoActual = "";
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("Periodo")) periodoActual = this.tgGridConsulta.SelectedRows[0].Cells["Periodo"].Value.ToString();

                            switch (libro)
                            {
                                case LibroUtiles.LibroID_FacturasEmitidas:
                                    this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                    break;
                                case LibroUtiles.LibroID_FacturasRecibidas:
                                    this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                    break;
                                case LibroUtiles.LibroID_BienesInversion:
                                    this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                    break;
                                case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                                    this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID, periodoActual);
                                    break;
                            }

                        }
                    }
                    break;
            }
        }
        #endregion

        /*
        private void tgGridConsulta_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.tgGridConsulta.Columns[e.ColumnIndex].SortMode != DataGridViewColumnSortMode.NotSortable)
            {
                if (e.ColumnIndex == newSortColumn)
                {
                    if (newColumnDirection == ListSortDirection.Ascending)
                        newColumnDirection = ListSortDirection.Descending;
                    else
                        newColumnDirection = ListSortDirection.Ascending;
                }

                newSortColumn = e.ColumnIndex;

                switch (newColumnDirection)
                {
                    case ListSortDirection.Ascending:
                        this.tgGridConsulta.Sort(this.tgGridConsulta.Columns[newSortColumn], ListSortDirection.Ascending);
                        break;
                    case ListSortDirection.Descending:
                        this.tgGridConsulta.Sort(this.tgGridConsulta.Columns[newSortColumn], ListSortDirection.Descending);
                        break;
                }
            }
        }
         */ 
    }
}
