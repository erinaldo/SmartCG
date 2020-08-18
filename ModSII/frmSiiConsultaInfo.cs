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
    public partial class frmSiiConsultaInfo : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIICINF";
        public string ficheroExtension = "sic";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIICINF
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
            public string fechaExpedicion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaPresentacionDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaPresentacionHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string facturaModifica;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estadoCuadre;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string identificacionBien;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string claveOperacion;
        }

        FormularioValoresCampos valoresFormulario;

        private ArrayList periodoArray;
        private ArrayList librosArray;
        private ArrayList tipoIdentificacionArray;
        private ArrayList facturaModificaArray;
        private ArrayList estadoCuadreArray;
        private ArrayList claveOperacionArray;
        private ArrayList paisArray;

        private DataSet dsConsultaRespuesta;

        private string codigoCompania = "";
        private string ejercicioCG = "";
        private string tipoPeriodoImpositivo = "";

        private string consultaFiltroEstado = "";
        private string consultaFiltroFecha = "";
        private string consultaFiltroFechaPresentacionDesde = "";
        private string consultaFiltroFechaPresentacionHasta = "";

        public frmSiiConsultaInfo()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiConsultaInfo_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta Datos Hacienda");

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

            //Crear el desplegable de Factura Modifica (Si/No)
            this.CrearComboFacturaModifica();

            //Crear el desplegable de Estado de Cuadre (No Contrastable/En proceso de contraste/...)
            this.CrearComboEstadoCuadre();

            //Crear el desplegable de Clave Operacion (A/B/...)
            this.CrearComboClaveOperacion();

            //Crear el TGGrid
            this.BuiltgConsultaInfo();

            //Construir el DataSet con el resultado del envio
            this.dsConsultaRespuesta = new DataSet();

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
                        FacturaIdentificador facturaID = new FacturaIdentificador();
                        facturaID.EmisorFacturaNIF = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDNIF"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROCodigoPais"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROIdType"].Value.ToString();
                        facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROId"].Value.ToString();

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.Rows[e.RowIndex].Cells["NumSerieFacturaEmisor"].Value.ToString();
                        else facturaID.NumeroSerie = "";

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.Rows[e.RowIndex].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                        else facturaID.FechaDocumento = "";

                        string iDEmisorFactura;
                        string libro = this.cmbLibro.SelectedValue.ToString();

                        if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDEmisorFactura"].Value.ToString();
                        else iDEmisorFactura = "";

                        switch(columnName)
                        {
                            case "VER":
                                switch (libro)
                                {
                                    case LibroUtiles.LibroID_FacturasEmitidas:
                                        this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_FacturasRecibidas:
                                        this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_BienesInversion:
                                        this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID);
                                        break;
                                    case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                                        this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID);
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
                                frmListaMovs.Show(this);
                                break;
                            case "PAGO":
                                string nombreRazonSocial = "";
                                if (this.dsConsultaRespuesta.Tables["MasInfo"].Columns.Contains("ContraparteNombreRazonSocial"))
                                {
                                    //Buscar la factura (por si se ordenó por columnas)
                                    string filtro = "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                                    filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                                    filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

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
                                if (this.dsConsultaRespuesta.Tables["MasInfo"].Columns.Contains("ContraparteNombreRazonSocial")) nombreRazonSocial = this.dsConsultaRespuesta.Tables["MasInfo"].Rows[e.RowIndex]["ContraparteNombreRazonSocial"].ToString();
                                frmSiiConsultaPagosRecibidas frmPago = new frmSiiConsultaPagosRecibidas();
                                frmPago.Compania = this.codigoCompania;
                                frmPago.FacturaID = facturaID;
                                frmPago.IDEmisorFactura = iDEmisorFactura;
                                frmPago.NombreRazonSocial = nombreRazonSocial;
                                frmPago.Show(this);
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
                if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count &&
                    this.tgGridConsulta.dsDatos.Tables[0].Columns.Count > 1)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    FacturaIdentificador facturaID = new FacturaIdentificador();
                    facturaID.EmisorFacturaNIF = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDNIF"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROCodigoPais"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROIdType"].Value.ToString();
                    facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROId"].Value.ToString();

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.Rows[e.RowIndex].Cells["NumSerieFacturaEmisor"].Value.ToString();
                    else facturaID.NumeroSerie = "";

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.Rows[e.RowIndex].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                    else facturaID.FechaDocumento = "";

                    string iDEmisorFactura;
                    string libro = this.cmbLibro.SelectedValue.ToString();

                    if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDEmisorFactura"].Value.ToString();
                    else iDEmisorFactura = "";

                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                            this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID);
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
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("TipoFactura")) this.tgGridConsulta.Columns["TipoFactura"].Visible = false;
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ClaveRegimenEspecialOTrascendencia")) this.tgGridConsulta.Columns["ClaveRegimenEspecialOTrascendencia"].Visible = false;
                            this.CambiarColumnasEncabezadosFacturasEmitidas();
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("TipoFactura")) this.tgGridConsulta.Columns["TipoFactura"].Visible = false;
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ClaveRegimenEspecialOTrascendencia")) this.tgGridConsulta.Columns["ClaveRegimenEspecialOTrascendencia"].Visible = false;
                            this.CambiarColumnasEncabezadosFacturasRecibidas();
                            break;
                        case LibroUtiles.LibroID_PagosRecibidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("PagoMedio")) this.tgGridConsulta.Columns["PagoMedio"].Visible = false;
                            this.CambiarColumnasEncabezadosPagosRecibidas();
                            break;
                        /*case LibroUtiles.LibroID_CobrosEmitidas:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("CobroMedio")) this.tgGridConsulta.Columns["CobroMedio"].Visible = false;
                            break;
                        */
                        case LibroUtiles.LibroID_CobrosMetalico:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            this.CambiarColumnasEncabezadosCobrosMetalico();
                            break;
                        case LibroUtiles.LibroID_OperacionesSeguros:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("ClaveOperacion")) this.tgGridConsulta.Columns["ClaveOperacion"].Visible = false;
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            this.CambiarColumnasEncabezadosOperacionesSeguros();
                            break;
                        case LibroUtiles.LibroID_AgenciasViajes:
                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.Columns["IDEmisorFactura"].Visible = false;
                            this.CambiarColumnasEncabezadosAgenciasViajes();
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.CambiarColumnasEncabezadosBienesInversion();
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                            this.CambiarColumnasEncabezadosDetOperacionesIntracomunitarias();
                            break;
                    }
                }
            }
        }

        private void tgGridConsulta_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count)
                {
                    string columnName = this.tgGridConsulta.Columns[e.ColumnIndex].Name;

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
                        if (this.tgGridConsulta.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridConsulta.dsDatos.Tables[0].Rows.Count &&
                            this.tgGridConsulta.dsDatos.Tables[0].Columns.Count > 1)
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            FacturaIdentificador facturaID = new FacturaIdentificador();
                            facturaID.EmisorFacturaNIF = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDNIF"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroCodPais = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROCodigoPais"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroIdType = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROIdType"].Value.ToString();
                            facturaID.EmisorFacturaIdOtroId = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDOTROId"].Value.ToString();

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("NumSerieFacturaEmisor")) facturaID.NumeroSerie = this.tgGridConsulta.Rows[e.RowIndex].Cells["NumSerieFacturaEmisor"].Value.ToString();
                            else facturaID.NumeroSerie = "";

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("FechaExpedicionFacturaEmisor")) facturaID.FechaDocumento = this.tgGridConsulta.Rows[e.RowIndex].Cells["FechaExpedicionFacturaEmisor"].Value.ToString();
                            else facturaID.FechaDocumento = "";

                            string iDEmisorFactura;

                            if (this.tgGridConsulta.dsDatos.Tables["DatosGenerales"].Columns.Contains("IDEmisorFactura")) iDEmisorFactura = this.tgGridConsulta.Rows[e.RowIndex].Cells["IDEmisorFactura"].Value.ToString();
                            else iDEmisorFactura = "";

                            switch (libro)
                            {
                                case LibroUtiles.LibroID_FacturasEmitidas:
                                    this.ConsultaViewFacturaEmitida(e.RowIndex, iDEmisorFactura, facturaID);
                                    break;
                                case LibroUtiles.LibroID_FacturasRecibidas:
                                    this.ConsultaViewFacturaRecibida(e.RowIndex, iDEmisorFactura, facturaID);
                                    break;
                                case LibroUtiles.LibroID_BienesInversion:
                                    this.ConsultaViewBienInversion(e.RowIndex, iDEmisorFactura, facturaID);
                                    break;
                                case LibroUtiles.LibroID_OperacionesIntracomunitarias:    //Determinadas Operaciones Intracomunitarias
                                    this.ConsultaViewOperacionIntracomunitaria(e.RowIndex, iDEmisorFactura, facturaID);
                                    break;
                            }
                            break;
                        }
                    }
                    break;
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
            this.txtMaskFechaExpedicion.Text = "";
            this.cmbFactModifica.Text = "";
            this.txtMaskFechaPresentacionDesde.Text = "";
            this.txtMaskFechaPresentacionHasta.Text = "";
            this.cmbEstadoCuadre.Text = "";
            this.rbEstadoTodas.Checked = true;
            this.txtIdentBien.Text = "";
            this.cmbClaveOperacion.Text = "";
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

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            //this.GridExportar();  //Exporta Directamente a Excel

            this.GridExportarHTML();    //Exporta a un HTML temporal y despúes se muestra en un Excel
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

        private void frmSiiConsultaInfo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta Datos Hacienda");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiConsultaInfoTitulo", "Consulta Datos Hacienda");
            this.Text += this.FormTituloAgenciaEntorno();

            this.gbBuscador.Text = " " + this.LP.GetText("lblBuscador", "Buscador") + " ";
            this.lblLibro.Text = this.LP.GetText("lblLibro", "Libro");
            this.lblCompania.Text = this.LP.GetText("lblCompaniaFiscal", "Compañía Fiscal");
            this.lblEjercicio.Text = this.LP.GetText("lblEjercicio", "Ejercicio");
            this.lblPeriodo.Text = this.LP.GetText("lblPeriodo", "Periodo");

            this.lblTipoIdentificacion.Text = this.LP.GetText("lblTipoIdentif", "Tipo de identificación");
            this.rbNIF.Text = this.LP.GetText("lblListaCampoNIF", "NIF");
            this.rbOtro.Text = this.LP.GetText("lblListaCampoOtro", "Otro");

            this.lblNIF.Text = this.LP.GetText("lblNIFDest", "NIF Destinatario");
            this.lblCodPais.Text = this.LP.GetText("lblCodPaisDest", "Cod. Pais Destinatario");
            this.lblTipoIden.Text = this.LP.GetText("lblTipoIdentifDest", "Tipo Identif. Dest.");
            this.lblNombreRazon.Text = this.LP.GetText("lblNombreRazonDest", "Nombre o Razón Social Dest.");
            this.lblNumSerieFactura.Text = this.LP.GetText("lblNumSerieFact", "Número Serie Factura");
            this.lblFechaExpedicion.Text = this.LP.GetText("lblFechaExpedicion", "Fecha Expedición");
            this.lblFactModifica.Text = this.LP.GetText("lblFactModifica", "Factura Modificada");
            this.lblFechaPresentacionDesde.Text = this.LP.GetText("lblFechaPresentacionDesde", "Fecha Presentación Desde");
            this.lblFechaPresentacionHasta.Text = this.LP.GetText("lblFechaPresentacionHasta", "Fecha Presentación Hasta");
            this.lblEstadoCuadre.Text = this.LP.GetText("lblEstadoCuadre", "Estado Cuadre");
            //this.lblEstadoFactura.Text = this.LP.GetText("lblEstadoFactura", "Estado Facturas");
            this.rbEstadoCorrecto.Text = this.LP.GetText("lblEstadoCorrecta", "Correcta");
            this.rbEstadoAceptadoErrores.Text = this.LP.GetText("lblEstadoAceptadaConErrores", "Aceptada con errores");
            this.rbEstadoAnulada.Text = this.LP.GetText("lblEstadoAnulada", "Anulada");
            this.lblIdentBien.Text = this.LP.GetText("lblIdentBien", "Identificación Bien");
            this.lblClaveOperacion.Text = this.LP.GetText("lblClaveOperacion", "Clave Operación");

            this.gbResultado.Text = " " + this.LP.GetText("gbResultado", "Resultado") + " ";
            this.lblNoInfo.Text = this.LP.GetText("lblNoExistenFacturas", "No existen facturas que cumplan el criterio seleccionado");

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

                this.cmbTipoIdentif.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Factura Modifica
        /// </summary>
        private void CrearComboFacturaModifica()
        {
            try
            {
                facturaModificaArray = new ArrayList();
                facturaModificaArray.Add(new AddValue("", ""));
                facturaModificaArray.Add(new AddValue("Si", "S"));
                facturaModificaArray.Add(new AddValue("No", "N"));

                this.cmbFactModifica.DataSource = facturaModificaArray;
                this.cmbFactModifica.DisplayMember = "Display";
                this.cmbFactModifica.ValueMember = "Value";

                this.cmbFactModifica.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Factura Modifica
        /// </summary>
        private void CrearComboEstadoCuadre()
        {
            try
            {
                estadoCuadreArray = new ArrayList();
                estadoCuadreArray.Add(new AddValue("", ""));
                estadoCuadreArray.Add(new AddValue("No contrastable", "1"));
                estadoCuadreArray.Add(new AddValue("En proceso de contraste", "2"));
                estadoCuadreArray.Add(new AddValue("No contrastada", "3"));
                estadoCuadreArray.Add(new AddValue("Parcialmente contrastada", "4"));
                estadoCuadreArray.Add(new AddValue("Contrastada", "5"));

                this.cmbEstadoCuadre.DataSource = estadoCuadreArray;
                this.cmbEstadoCuadre.DisplayMember = "Display";
                this.cmbEstadoCuadre.ValueMember = "Value";

                this.cmbEstadoCuadre.SelectedIndex = 0;
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
                DateTime fechaDateTimeDesde = new DateTime();
                DateTime fechaDateTimeHasta = new DateTime();

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

                string filtrofechaDesde = "";
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                filtrofechaDesde = this.txtMaskFechaPresentacionDesde.Text.Trim();
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (filtrofechaDesde != "")
                {
                    fechaStr = this.txtMaskFechaPresentacionDesde.Text;
                    fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaDateTimeDesde);
                    if (fechaStr != this.txtMaskFechaPresentacionDesde.Text) this.txtMaskFechaPresentacionDesde.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de presentación desde no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionDesde.Focus();
                        return (false);
                    }
                }

                string filtrofechaHasta = "";
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                filtrofechaHasta = this.txtMaskFechaPresentacionHasta.Text.Trim();
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (filtrofechaHasta != "")
                {
                    fechaStr = this.txtMaskFechaPresentacionHasta.Text;
                    fechaValida = LibroUtiles.FormatoFechaSiiValid(ref fechaStr, ref fechaDateTimeHasta);
                    if (fechaStr != this.txtMaskFechaPresentacionHasta.Text) this.txtMaskFechaPresentacionHasta.Text = fechaStr;
                    if (!fechaValida)
                    {
                        MessageBox.Show("La fecha de presentación hasta no es válida", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionHasta.Focus();
                        return (false);
                    }
                }

                if (filtrofechaDesde != "" && filtrofechaHasta != "")
                {
                    if (fechaDateTimeDesde > fechaDateTimeHasta)
                    {
                        MessageBox.Show("La fecha de presentación desde no puede ser posterior a la fecha de presentación hasta", "Error");   //Falta traducir
                        this.txtMaskFechaPresentacionDesde.Focus();
                        return (false);
                    }
                }

                switch (libro)
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

                        if (this.rbOtro.Checked && this.cmbPais.SelectedValue.ToString().Trim() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02")
                        {
                            MessageBox.Show("Es obligatorio informar el Código del País del destinatario", "Error");   //Falta traducir
                            this.cmbPais.Focus();
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

                        //Validar Identificador de la factura si procede
                        if (this.rbOtro.Checked)
                        {
                            bool errorIdExtranjero = false;
                            if (this.txtNIF.Text.Trim() == "" ||
                                this.cmbTipoIdentif.SelectedValue.ToString() == "" ||
                                (this.cmbPais.SelectedValue.ToString().Trim() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02") ||
                                this.txtNombreRazon.Text == "")
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

                                if (this.cmbPais.SelectedValue.ToString().Trim() == "" && this.cmbTipoIdentif.SelectedValue.ToString() != "02")
                                {
                                    MessageBox.Show("Es obligatorio informar el Código del País", "Error");   //Falta traducir
                                    this.cmbPais.Focus();
                                    return (false);
                                }

                                if (this.txtNombreRazon.Text == "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Nombre o Razón Social", "Error");   //Falta traducir
                                    this.txtNombreRazon.Focus();
                                    return (false);
                                }
                            }
                        }
                        else
                        {
                            if (this.txtNIF.Text == "" && this.txtNombreRazon.Text != "")
                            {
                                MessageBox.Show("Es obligatorio informar el NIF", "Error");   //Falta traducir
                                this.txtNIF.Focus();
                                return (false);
                            }
                            else
                            {
                                if (this.txtNombreRazon.Text == "" && this.txtNIF.Text != "")
                                {
                                    MessageBox.Show("Es obligatorio informar el Nombre o Razón Social", "Error");   //Falta traducir
                                    this.txtNombreRazon.Focus();
                                    return (false);
                                }
                            }
                        }
                        break;
                }

                this.consultaFiltroEstado = this.ObtenerEstado();
                
                if (this.consultaFiltroFecha != "") this.consultaFiltroFecha = this.txtMaskFechaExpedicion.Text;

                this.consultaFiltroFechaPresentacionDesde = "";
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaPresentacionDesde = this.txtMaskFechaPresentacionDesde.Text.Trim();
                this.txtMaskFechaPresentacionDesde.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (this.consultaFiltroFechaPresentacionDesde != "") this.consultaFiltroFechaPresentacionDesde = this.txtMaskFechaPresentacionDesde.Text;

                this.consultaFiltroFechaPresentacionHasta = "";
                //coger el valor sin la máscara
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.consultaFiltroFechaPresentacionHasta = this.txtMaskFechaPresentacionHasta.Text.Trim();
                this.txtMaskFechaPresentacionHasta.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (this.consultaFiltroFechaPresentacionHasta != "") this.consultaFiltroFechaPresentacionHasta = this.txtMaskFechaPresentacionHasta.Text;
                
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
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("EstadoCuadre")) this.tgGridConsulta.dsDatos.Tables.Remove("EstadoCuadre");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resultado")) this.tgGridConsulta.dsDatos.Tables.Remove("Resultado");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resumen")) this.tgGridConsulta.dsDatos.Tables.Remove("Resumen");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleExenta")) this.tgGridConsulta.dsDatos.Tables.Remove("DetalleExenta");
                    }

                    //Eliminar todas las tablas del dataset
                    if (this.dsConsultaRespuesta != null && this.dsConsultaRespuesta.Tables != null && this.dsConsultaRespuesta.Tables.Count > 0)
                    {
                        this.dsConsultaRespuesta.Tables.Clear();
                        this.dsConsultaRespuesta.Clear();
                    }

                    /*
                    //Eliminar todas las tablas del dataset
                    if (this.dsConsultaRespuesta != null && this.dsConsultaRespuesta.Tables != null)
                    {
                        while (this.dsConsultaRespuesta.Tables.Count > 0)
                        {
                            DataTable table = this.dsConsultaRespuesta.Tables[0];
                            if (this.dsConsultaRespuesta.Tables.CanRemove(table))
                            {
                                this.dsConsultaRespuesta.Tables.Remove(table);
                            }
                        }
                    }
                    */

                    string periodo = this.cmbPeriodo.SelectedValue.ToString();

                    switch (libro)
                    {
                        case LibroUtiles.LibroID_FacturasEmitidas:
                            //DataSet dsRespuesta = this.ConsultaInformacionFacturasEmitidas(compania, this.yearSel, this.periodoSel);
                            this.dsConsultaRespuesta = this.ConsultaInformacionFacturasEmitidas(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_FacturasRecibidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionFacturasRecibidas(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_BienesInversion:
                            this.dsConsultaRespuesta = this.ConsultaInformacionBienesInversion(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                            this.dsConsultaRespuesta = this.ConsultaInformacionDetOperacionesIntracomunitarias(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_PagosRecibidas:
                            this.dsConsultaRespuesta = this.ConsultaInformacionPagosRecibidas(this.codigoCompania);
                            break;
                        case LibroUtiles.LibroID_CobrosMetalico:
                            this.dsConsultaRespuesta = this.ConsultaInformacionCobrosMetalico(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_OperacionesSeguros:
                            this.dsConsultaRespuesta = this.ConsultaInformacionOperacionesSeguros(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        case LibroUtiles.LibroID_AgenciasViajes:
                            this.dsConsultaRespuesta = this.ConsultaInformacionAgenciasViajes(this.codigoCompania, this.ejercicioCG, periodo);
                            break;
                        //case LibroUtiles.LibroID_CobrosEmitidas:
                        //    this.dsConsultaRespuesta = this.ConsultaInformacionCobrosEmitidas(this.codigoCompania);
                        //    break;
                          
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
        private DataSet ConsultaInformacionFacturasEmitidas(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();
                string estadoCuadre = "";
                if (this.cmbEstadoCuadre != null && this.cmbEstadoCuadre.SelectedValue != null) estadoCuadre = this.cmbEstadoCuadre.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRFacturasEmitidas(this.codigoCompania, this.ejercicioCG, periodo,
                                                                              this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                              this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                              this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                              this.consultaFiltroFecha, this.consultaFiltroEstado,
                                                                              this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                              factModifica,
                                                                              estadoCuadre);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    /*
                    if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                    {
                        //Eliminar los resultados de la búsqueda anterior
                        this.tgGridConsulta.Visible = false;

                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridConsulta.dsDatos.Tables.Remove("DatosGenerales");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("MasInfo")) this.tgGridConsulta.dsDatos.Tables.Remove("MasInfo");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("DetalleIVA")) this.tgGridConsulta.dsDatos.Tables.Remove("DetalleIVA");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("EstadoCuadre")) this.tgGridConsulta.dsDatos.Tables.Remove("EstadoCuadre");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resultado")) this.tgGridConsulta.dsDatos.Tables.Remove("Resultado");
                    }
                    */

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
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["EstadoCuadre"].Copy());

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

                        //Salian en un orden erroneo, asi fuerzo que aparezcan al final
                        int totalColumnas = this.tgGridConsulta.Columns.Count;
                        this.tgGridConsulta.Columns["VER"].DisplayIndex = totalColumnas - 2;
                        this.tgGridConsulta.Columns["MOV"].DisplayIndex = totalColumnas - 1;

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled= true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = true;
                            this.lblBaseImponibleTotal.Visible = true;
                            this.lblCuotaDeducibleTotalValor.Visible = true;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                MessageBox.Show(ex.Message, "Error");
            }

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
        private void ConsultaViewFacturaEmitida(int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactEmitida frmViewFacturaEnviada = new frmSiiConsultaViewFactEmitida();
                frmViewFacturaEnviada.Ejercicio = this.txtEjercicio.Text;
                frmViewFacturaEnviada.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewFacturaEnviada.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro =  "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";
                
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

                try
                {
                    DataRow rowEstadoCuadre = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("EstadoCuadre") && this.tgGridConsulta.dsDatos.Tables["EstadoCuadre"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["EstadoCuadre"].Select(filtro);

                        if (rows.Length == 1) rowEstadoCuadre = rows[0];
                    }
                    frmViewFacturaEnviada.RowEstadoCuadre = rowEstadoCuadre;
                }
                catch (Exception ex)
                {
                    frmViewFacturaEnviada.RowEstadoCuadre = null;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                frmViewFacturaEnviada.Show(this);
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
        private DataSet ConsultaInformacionFacturasRecibidas(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();
                string estadoCuadre = "";
                if (this.cmbEstadoCuadre != null && this.cmbEstadoCuadre.SelectedValue != null) estadoCuadre = this.cmbEstadoCuadre.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRFacturasRecibidas(this.codigoCompania, this.ejercicioCG, periodo,
                                                                               this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(), 
                                                                               this.cmbTipoIdentif.SelectedValue.ToString(), 
                                                                               this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                               this.consultaFiltroFecha, this.consultaFiltroEstado,
                                                                               this.consultaFiltroFechaPresentacionDesde, 
                                                                               this.consultaFiltroFechaPresentacionHasta,
                                                                               factModifica,
                                                                               estadoCuadre);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DetalleIVA"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["EstadoCuadre"].Copy());

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

                        //Tabla Resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            string totalReg = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            string totalImporte = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            string totalBaseImponible = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            string totalCuotaDeducible = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                        }

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblImporteTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalImp"].ToString();
                            this.lblBaseImponibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalBaseImponible"].ToString();
                            this.lblCuotaDeducibleTotalValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalCuotaDeducible"].ToString();
                            this.lblImporteTotalValor.Visible = true;
                            this.lblImporteTotal.Visible = true;
                            this.lblBaseImponibleTotalValor.Visible = true;
                            this.lblBaseImponibleTotal.Visible = true;
                            this.lblCuotaDeducibleTotalValor.Visible = true;
                            this.lblCuotaTotal.Visible = true;
                            this.gbResultado.Visible = true;
                        }
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("TipoFacturaDesc")) this.tgGridConsulta.CambiarColumnHeader("TipoFacturaDesc", "Tipo Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ClaveRegimenEspecialOTrascendenciaDesc")) this.tgGridConsulta.CambiarColumnHeader("ClaveRegimenEspecialOTrascendenciaDesc", "Clave Regimen Especial O Trascendencia");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionOperacion")) this.tgGridConsulta.CambiarColumnHeader("DescripcionOperacion", "Descripcion Operacion");  //Falta traducir
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
        private void ConsultaViewFacturaRecibida(int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewFactRecibida frmViewFacturaRecibida = new frmSiiConsultaViewFactRecibida();
                frmViewFacturaRecibida.Ejercicio = this.txtEjercicio.Text;
                frmViewFacturaRecibida.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewFacturaRecibida.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

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

                try
                {
                    DataRow rowEstadoCuadre = null;
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("EstadoCuadre") && this.tgGridConsulta.dsDatos.Tables["EstadoCuadre"].Rows.Count > 0)
                    {
                        DataRow[] rows = this.tgGridConsulta.dsDatos.Tables["EstadoCuadre"].Select(filtro);

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
        #endregion

        #region ----- Determinadas Operaciones Intracomunitarias -----
        /// <summary>
        /// Invoca la consulta de las operaciones intracomunitarias enviadas a la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionDetOperacionesIntracomunitarias(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRDetOperIntracomunitarias(this.codigoCompania, this.ejercicioCG, periodo,
                                                                                      this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(), 
                                                                                      this.cmbTipoIdentif.SelectedValue.ToString(), 
                                                                                      this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                                      this.consultaFiltroFecha, this.consultaFiltroEstado,
                                                                                      this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                                      factModifica);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());

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
                        //this.CambiarColumnasEncabezadosDetOperacionesIntracomunitarias();

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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
        private void ConsultaViewOperacionIntracomunitaria(int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewDetOperacIntracomunitaria frmViewOperacionIntracomunitaria = new frmSiiConsultaViewDetOperacIntracomunitaria();
                frmViewOperacionIntracomunitaria.Ejercicio = this.txtEjercicio.Text;
                frmViewOperacionIntracomunitaria.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewOperacionIntracomunitaria.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

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

                frmViewOperacionIntracomunitaria.Show(this);
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
        private DataSet ConsultaInformacionBienesInversion(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRBienesInversion(this.codigoCompania, this.ejercicioCG, periodo,
                                                                             this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                             this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                             this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                             this.consultaFiltroFecha, this.consultaFiltroEstado,
                                                                             this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                             factModifica, 
                                                                             this.txtIdentBien.Text);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["MasInfo"].Copy());

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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridConsulta.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridConsulta.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("IdentificacionBien")) this.tgGridConsulta.CambiarColumnHeader("IdentificacionBien", "Identificación Bien");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("FechaInicioUtilizacion")) this.tgGridConsulta.CambiarColumnHeader("FechaInicioUtilizacion", "Fecha Inicio Utilización");  //Falta traducir
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
        private void ConsultaViewBienInversion(int indice, string iDEmisorFactura, FacturaIdentificador facturaID)
        {
            try
            {
                //Visualizar la información
                frmSiiConsultaViewBienInversion frmViewBienInversion = new frmSiiConsultaViewBienInversion();
                frmViewBienInversion.Ejercicio = this.txtEjercicio.Text;
                frmViewBienInversion.Periodo = this.cmbPeriodo.SelectedValue.ToString();
                frmViewBienInversion.FacturaID = facturaID;

                //Buscar la factura (por si se ordenó por columnas)
                string filtro = "(IDEmisorFactura='" + iDEmisorFactura + "') AND ";
                filtro += "(NumSerieFacturaEmisor='" + facturaID.NumeroSerie + "') AND ";
                filtro += "(FechaExpedicionFacturaEmisor ='" + facturaID.FechaDocumento + "') ";

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

                frmViewBienInversion.Show(this);
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
        private DataSet ConsultaInformacionPagosRecibidas(string compania)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRPagosRecibidas(this.codigoCompania, this.txtNIF.Text,
                                                                            this.cmbPais.SelectedValue.ToString(), this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                            this.txtNombreRazon.Text, this.txtNumSerieFactura.Text,
                                                                            this.consultaFiltroFecha);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
        private DataSet ConsultaInformacionCobrosMetalico(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRCobrosMetalico(this.codigoCompania, this.ejercicioCG, periodo,
                                                                            this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(), 
                                                                            this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                            this.txtNombreRazon.Text, this.consultaFiltroEstado,
                                                                            this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                            factModifica);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
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
        private DataSet ConsultaInformacionOperacionesSeguros(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();
                string claveOperacion = "";
                if (this.cmbClaveOperacion != null && this.cmbClaveOperacion.SelectedValue != null) claveOperacion = this.cmbClaveOperacion.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLROperacionesSeguros(this.codigoCompania, this.ejercicioCG, periodo,
                                                                                this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                                this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                                this.txtNombreRazon.Text, this.consultaFiltroEstado, 
                                                                                this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                                claveOperacion,
                                                                                factModifica);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ClaveOperacionDesc")) this.tgGridConsulta.CambiarColumnHeader("ClaveOperacionDesc", "Clave Operación");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNIF")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNIF", "Entidad Sucedida NIF");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EntidadSucedidaNombreRazonSocial")) this.tgGridConsulta.CambiarColumnHeader("EntidadSucedidaNombreRazonSocial", "Entidad Sucedida Nombre Razón SocialF");  //Falta traducir
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion

        /*
        #region ----- Cobros Emitidas -----
        /// <summary>
        /// Invoca la consulta de cobros emitidas de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionCobrosEmitidas(string compania)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                this.dsConsultaRespuesta = wsTGsii.ConsultaLRCobrosEmitidas(this.codigoCompania, 
                                                                            this.txtNIF.Text, this.txtNumSerieFactura.Text, 
                                                                            this.consultaFiltroFecha, this.consultaFiltroEstado);

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

                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }

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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
        */

        #region ----- Agencias de Viajes -----
        /// <summary>
        /// Invoca la consulta de agencias de viajes de la AEAT
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionAgenciasViajes(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string factModifica = "";
                if (this.cmbFactModifica != null && this.cmbFactModifica.SelectedValue != null) factModifica = this.cmbFactModifica.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRAgenciasViajes(this.codigoCompania, this.ejercicioCG, periodo,
                                                                            this.txtNIF.Text, this.cmbPais.SelectedValue.ToString(),
                                                                            this.cmbTipoIdentif.SelectedValue.ToString(),
                                                                            this.txtNombreRazon.Text, this.consultaFiltroEstado,
                                                                            this.consultaFiltroFechaPresentacionDesde, this.consultaFiltroFechaPresentacionHasta,
                                                                            factModifica);

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar la columna Ver al DataTable de Datos Generales
                        this.AdicionarColumnaVerTablaDatosGenerales();

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
                            this.lblCuotaDeducibleTotalValor.Visible = false;
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNifIdOtro")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNifIdOtro", "NIF Contraparte");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNombreRazon")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNombreRazon", "Nombre o Razón");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ContraparteNIFRepresentante")) this.tgGridConsulta.CambiarColumnHeader("ContraparteNIFRepresentante", "NIF Representante");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ImporteTotal")) this.tgGridConsulta.CambiarColumnHeader("ImporteTotal", "Importe Total");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("EstadoRegistro")) this.tgGridConsulta.CambiarColumnHeader("EstadoRegistro", "Estado");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridConsulta.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
                if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("MOV")) this.tgGridConsulta.CambiarColumnHeader("MOV", "Ver Movimientos");  //Falta traducir
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
                    this.gbTipoIdentificacion.Enabled = true;
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
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Factura Modificada";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = true;
                    this.cmbEstadoCuadre.Visible = true;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled= true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
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
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Factura Modificada";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = true;
                    this.cmbEstadoCuadre.Visible = true;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
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
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Factura Modificada";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = true;
                    this.txtIdentBien.Visible = true;
                    this.lblIdentBien.Location = new Point(this.lblIdentBien.Location.X, this.lblEstadoCuadre.Location.Y);
                    this.txtIdentBien.Location = new Point(this.txtIdentBien.Location.X, this.cmbEstadoCuadre.Location.Y);
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    this.rbOtro.Text = "NIF-IVA";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblCodPais.Text = "Cód. País Dest/Emisor";
                    this.lblTipoIden.Text = "Tipo Identif. Dest/Emisor";

                    if (this.rbOtro.Checked) this.cmbTipoIdentif.SelectedValue = "02";
                    this.lblTipoIden.Enabled = false;
                    this.cmbTipoIdentif.Enabled = false;
                    this.lblNombreRazon.Text = "Nombre o Razón Soc. Dest/Emisor";

                    if (this.rbOtro.Checked) this.lblNIF.Text = "Identif. Dest/Emisor";
                    else this.lblNIF.Text = "NIF Dest./Emisor";
                    
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
                    this.cmbPeriodo.Enabled = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Factura Modificada";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.rbEstadoTodas.Checked = true;
                    if (this.cmbPeriodo.SelectedValue != null && this.cmbPeriodo.SelectedValue.ToString() == LibroUtiles.PeriodoAnual) this.cmbPeriodo.SelectedIndex = 0;
                    this.lblEjercicio.Enabled = false;
                    this.txtEjercicio.Enabled = false;
                    this.txtEjercicio.Text = "";
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.Text = "";
                    this.gbEstadoFacturas.Enabled = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFactModifica.Visible = false;
                    this.cmbFactModifica.Visible = false;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = false;
                    this.txtMaskFechaPresentacionDesde.Enabled = false;
                    this.lblFechaPresentacionHasta.Enabled = false;
                    this.txtMaskFechaPresentacionHasta.Enabled = false;
                    this.lblEstadoFactura.Enabled = false;
                    this.gbEstadoFacturas.Enabled = false;
                    this.lblNumSerieFactura.Enabled = true;
                    this.txtNumSerieFactura.Enabled = true;
                    this.lblFechaExpedicion.Enabled = true;
                    this.txtMaskFechaExpedicion.Enabled = true;
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
                    this.lblFechaExpedicion.Enabled = false;
                    this.txtMaskFechaExpedicion.Text = "";
                    this.txtMaskFechaExpedicion.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Cobro Modificado";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    this.rbOtro.Text = "Otro";
                    this.gbTipoIdentificacion.Enabled = true;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.lblNombreRazon.Enabled = true;
                    this.txtNombreRazon.Enabled = true;
                    this.lblNumSerieFactura.Enabled = false;
                    this.txtNumSerieFactura.Text = "";
                    this.txtNumSerieFactura.Enabled = false;
                    this.lblFechaExpedicion.Enabled = false;
                    this.txtMaskFechaExpedicion.Text = "";
                    this.txtMaskFechaExpedicion.Enabled = false;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Operación Modificada";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = true;
                    this.cmbClaveOperacion.Visible = true;
                    this.lblClaveOperacion.Location = new Point(this.lblClaveOperacion.Location.X, this.lblEstadoCuadre.Location.Y);
                    this.cmbClaveOperacion.Location = new Point(this.cmbClaveOperacion.Location.X, this.cmbEstadoCuadre.Location.Y);
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
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
                    this.lblEjercicio.Enabled = false;
                    this.txtEjercicio.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.rbEstadoTodas.Checked = true;
                    this.gbEstadoFacturas.Enabled = false;
                    this.lblFactModifica.Visible = false;
                    this.cmbFactModifica.Visible = false;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = false;
                    this.txtMaskFechaPresentacionDesde.Enabled = false;
                    this.lblFechaPresentacionHasta.Enabled = false;
                    this.txtMaskFechaPresentacionHasta.Enabled = false;
                    this.lblEstadoFactura.Enabled = false;
                    this.gbEstadoFacturas.Enabled = false;
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
                    this.lblFechaExpedicion.Enabled = false;
                    this.txtMaskFechaExpedicion.Text = "";
                    this.txtMaskFechaExpedicion.Enabled = false;
                    this.cmbPeriodo.Enabled = false;
                    this.cmbPeriodo.SelectedValue = LibroUtiles.PeriodoAnual;
                    this.lblEjercicio.Enabled = true;
                    this.txtEjercicio.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    this.lblFactModifica.Text = "Registro Modificado";
                    this.lblFactModifica.Visible = true;
                    this.cmbFactModifica.Visible = true;
                    this.lblEstadoCuadre.Visible = false;
                    this.cmbEstadoCuadre.Visible = false;
                    this.lblIdentBien.Visible = false;
                    this.txtIdentBien.Text = "";
                    this.txtIdentBien.Visible = false;
                    this.lblClaveOperacion.Visible = false;
                    this.cmbClaveOperacion.Visible = false;
                    this.lblFechaPresentacionDesde.Enabled = true;
                    this.txtMaskFechaPresentacionDesde.Enabled = true;
                    this.lblFechaPresentacionHasta.Enabled = true;
                    this.txtMaskFechaPresentacionHasta.Enabled = true;
                    this.lblEstadoFactura.Enabled = true;
                    this.gbEstadoFacturas.Enabled = true;
                    break;
            }

            if (!this.txtMaskFechaPresentacionDesde.Enabled) this.txtMaskFechaPresentacionDesde.Text = "";
            if (!this.txtMaskFechaPresentacionHasta.Enabled) this.txtMaskFechaPresentacionHasta.Text = "";

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
                IntPtr pBuf = Marshal.StringToBSTR(valores.PadRight(174,' '));
                StructGLL01_MISIICINF myStruct = (StructGLL01_MISIICINF)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIICINF));

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

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio.Trim();

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.porNifIdOtro == "1") this.rbNIF.Checked = true;
                else
                {
                    this.rbNIF.Checked = false;
                    this.rbOtro.Checked = true;
                }

                if (myStruct.nif.Trim() != "") this.txtNIF.Text = myStruct.nif.Trim();

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

                if (myStruct.nombreRazonSocial.Trim() != "") this.txtNombreRazon.Text = myStruct.nombreRazonSocial.Trim();

                if (myStruct.noFactura.Trim() != "") this.txtNumSerieFactura.Text = myStruct.noFactura.Trim();

                if (myStruct.fechaExpedicion.Trim() != "") this.txtMaskFechaExpedicion.Text = myStruct.fechaExpedicion;

                if (myStruct.fechaPresentacionDesde.Trim() != "") this.txtMaskFechaPresentacionDesde.Text = myStruct.fechaPresentacionDesde;

                if (myStruct.fechaPresentacionHasta.Trim() != "") this.txtMaskFechaPresentacionHasta.Text = myStruct.fechaPresentacionHasta;

                switch (myStruct.estado)
                {
                    case "W":
                        this.rbEstadoAceptadoErrores.Checked = true;
                        break;
                    case "V":
                        this.rbEstadoCorrecto.Checked = true;
                        break;
                    case "B":
                        this.rbEstadoAnulada.Checked = true;
                        break;
                    default:        //Todas
                        this.rbEstadoTodas.Checked = true;
                        break;
                }

                try
                {
                    if (myStruct.facturaModifica.Trim() != "") this.cmbFactModifica.SelectedValue = myStruct.facturaModifica.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.estadoCuadre.Trim() != "") this.cmbEstadoCuadre.SelectedValue = myStruct.estadoCuadre.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.identificacionBien.Trim() != "") this.txtIdentBien.Text = myStruct.identificacionBien.Trim();

                try
                {
                    if (myStruct.claveOperacion.Trim() != "") this.cmbClaveOperacion.SelectedValue = myStruct.claveOperacion.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

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
                StructGLL01_MISIICINF myStruct;

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

                myStruct.fechaExpedicion = this.txtMaskFechaExpedicion.Text.PadRight(10, ' ');

                myStruct.fechaPresentacionDesde = this.txtMaskFechaPresentacionDesde.Text.PadRight(10, ' ');

                myStruct.fechaPresentacionHasta = this.txtMaskFechaPresentacionHasta.Text.PadRight(10, ' ');

                myStruct.estado = this.ObtenerEstado();

                if (this.cmbFactModifica.Visible) myStruct.facturaModifica = this.cmbFactModifica.SelectedValue.ToString().PadRight(1, ' ');
                else myStruct.facturaModifica = cadenaVacia.PadRight(1, ' ');

                if (this.cmbEstadoCuadre.Visible)
                {
                    if (this.cmbEstadoCuadre.SelectedValue != null) myStruct.estadoCuadre = this.cmbEstadoCuadre.SelectedValue.ToString().PadRight(1, ' ');
                    else myStruct.estadoCuadre = cadenaVacia.PadRight(1, ' '); 
                }
                else myStruct.estadoCuadre = cadenaVacia.PadRight(1, ' ');

                string identificacionBien = "";
                if (this.txtIdentBien.Visible) myStruct.identificacionBien = this.txtIdentBien.Text.PadRight(40, ' ');
                else myStruct.identificacionBien = identificacionBien.PadRight(40, ' ');

                if (this.cmbClaveOperacion.Visible)
                {
                    if (this.cmbClaveOperacion.SelectedValue != null) myStruct.claveOperacion = this.cmbClaveOperacion.SelectedValue.ToString().PadRight(1, ' ');
                    else myStruct.claveOperacion = cadenaVacia.PadRight(1, ' '); 
                }
                else myStruct.claveOperacion = cadenaVacia.PadRight(1, ' '); 

                result = myStruct.libro + myStruct.compania + myStruct.ejercicio + myStruct.periodo + myStruct.porNifIdOtro + myStruct.nif;
                result += myStruct.pais + myStruct.tipoIdentificacion + myStruct.nombreRazonSocial + myStruct.noFactura + myStruct.fechaExpedicion;
                result += myStruct.fechaPresentacionDesde + myStruct.fechaPresentacionHasta + myStruct.estado;
                result += myStruct.facturaModifica + myStruct.estadoCuadre + myStruct.identificacionBien + myStruct.claveOperacion;
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
                    dictControles.Add("País", "cmbPais");
                    dictControles.Add("Tipo Identificacion", "cmbTipoIdentif");
                    dictControles.Add("Fecha Presentación Desde", "txtMaskFechaPresentacionDesde");
                    dictControles.Add("Fecha Presentación Hasta", "txtMaskFechaPresentacionHasta");
                    dictControles.Add("Estado Todas", "rbEstadoTodas");
                    dictControles.Add("Estado Correcto", "rbEstadoCorrecto");
                    dictControles.Add("Estado AceptadoErrores", "rbEstadoAceptadoErrores");

                    List<string> columnNoVisible = new List<string>(new string[] { "txtMaskFechaPresentacionDesde", "txtMaskFechaPresentacionHasta", 
                                                                                   "cmbTipoIdentif", "cmbPais", "txtNombreRazon",
                                                                                   "txtNIF", "rbNIF", "rbOtro",
                                                                                   "rbEstadoTodas", 
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
        /// Exporta la consulta de Datos en Presentados a la AEAT a Excel, pasando por un fichero HTML
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Exportar la consulta de Datos en Local a Excel, pasando por un fichero HTML
                LibroUtiles.GridExportarHTML(ref this.tgGridConsulta, this.cmbLibro.SelectedValue.ToString(), "AEAT");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion
    }
}
