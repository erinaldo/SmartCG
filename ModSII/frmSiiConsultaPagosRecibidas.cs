using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiConsultaPagosRecibidas : frmPlantilla, IReLocalizable
    {
        private string compania;
        private FacturaIdentificador facturaID;
        private string iDEmisorFactura;
        private string nombreRazonSocial;

        private DataSet dsConsultaRespuesta;

        private bool datosLocal = false;

        #region Properties
        public string Compania
        {
            get
            {
                return (this.compania);
            }
            set
            {
                this.compania = value;
            }
        }

        public FacturaIdentificador FacturaID
        {
            get
            {
                return (this.facturaID);
            }
            set
            {
                this.facturaID = value;
            }
        }

        public string IDEmisorFactura
        {
            get
            {
                return (this.iDEmisorFactura);
            }
            set
            {
                this.iDEmisorFactura = value;
            }
        }

        public string NombreRazonSocial
        {
            get
            {
                return (this.nombreRazonSocial);
            }
            set
            {
                this.nombreRazonSocial = value;
            }
        }

        public bool DatosLocal
        {
            get
            {
                return (this.datosLocal);
            }
            set
            {
                this.datosLocal = value;
            }
        }
        #endregion

        public frmSiiConsultaPagosRecibidas()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiConsultaPagosRecibidas_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta Pagos Recibidas");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Escribe la información de la cabecera de la factura
            this.EscribirDatosCabecera();

            //Crear el TGGrid
            this.BuiltgGridPagos();

            //Llama a la consulta de pagos y muestra la lista
            this.ConsultarPagos();

            this.ActiveControl = this.lblIdEmisorFactura;
        }

        private void tgGridPagos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Eliminar columnas
            if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("PagoMedio")) this.tgGridPagos.Columns["PagoMedio"].Visible = false;
            if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("IDNIF")) this.tgGridPagos.Columns["IDNIF"].Visible = false;
            if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("IDOTROCodigoPais")) this.tgGridPagos.Columns["IDOTROCodigoPais"].Visible = false;
            if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("IDOTROIdType")) this.tgGridPagos.Columns["IDOTROIdType"].Visible = false;
            if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("IDOTROId")) this.tgGridPagos.Columns["IDOTROId"].Visible = false;

        }

        private void frmSiiConsultaPagosRecibidas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaPagosRecibidas_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta Pagos Recibidas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmConsultaPagosRecibidas", "Lista de Pagos");
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Escribe los datos de la cabecera de la factura
        /// </summary>
        private void EscribirDatosCabecera()
        {
            try
            {
                this.txtNIFEmisor.Text = this.iDEmisorFactura;
                this.txtNoFact.Text = this.facturaID.NumeroSerie;
                this.txtFechaDoc.Text = this.facturaID.FechaDocumento;
                this.txtNombreRazonSocial.Text = this.nombreRazonSocial;
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BuiltgGridPagos()
        {
            //Crear el DataGrid
            this.tgGridPagos.dsDatos = new DataSet();
            this.tgGridPagos.dsDatos.DataSetName = "Consulta";
            this.tgGridPagos.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridPagos.ReadOnly = true;
            this.tgGridPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.tgGridPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridPagos.AllowUserToAddRows = false;
            this.tgGridPagos.AllowUserToOrderColumns = false;
            //this.tgGridPagos.AutoGenerateColumns = false;
            this.tgGridPagos.NombreTabla = "Consulta";
        }

        private DataSet ConsultarPagos()
        {
            this.dsConsultaRespuesta = null;
            try
            {
                string nif_id = "";
                if (facturaID.EmisorFacturaNIF.Trim() != "") nif_id = facturaID.EmisorFacturaNIF;
                else nif_id = facturaID.EmisorFacturaIdOtroId;
                if (!this.datosLocal)
                {
                    /*tgSIIWebService.TGsiiService wsTGsii = new tgSIIWebService.TGsiiService();
                    
                    this.dsConsultaRespuesta = wsTGsii.ConsultaLRPagosRecibidas(this.compania, nif_id,
                                                                                this.facturaID.EmisorFacturaIdOtroCodPais, this.facturaID.EmisorFacturaIdOtroIdType,
                                                                                this.nombreRazonSocial,
                                                                                this.facturaID.NumeroSerie, this.facturaID.FechaDocumento);
                    */

                    //Verificar que esté informada y sea correcta la url del servicio web del sii
                    bool urlOk = true;
                    string urlMensaje = "";
                    if (this.serviceSII.URL == null || this.serviceSII.URL == "")
                    {
                        urlMensaje = "La dirección del servicio web que comunica con el SII no está informada. Por favor contacte con el administrador del sistema";
                        urlOk = false;
                    }
                    else 
                        if (!LibroUtiles.IsReachableUri(this.serviceSII.URL))
                        {
                            urlMensaje = "La dirección del servicio web que comunica con el SII no es correcta. Por favor contacte con el administrador del sistema";
                            urlOk = false;
                        }

                    if (!urlOk)
                    {
                        this.tgGridPagos.Visible = false;
                        this.lblNoInfo.Text = urlMensaje; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        return (this.dsConsultaRespuesta);
                    }
                    else
                    {
                        this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRPagosRecibidas(this.compania, nif_id,
                                                                                this.facturaID.EmisorFacturaIdOtroCodPais, this.facturaID.EmisorFacturaIdOtroIdType,
                                                                                this.nombreRazonSocial,
                                                                                this.facturaID.NumeroSerie, this.facturaID.FechaDocumento);
                    }
                }
                else
                {
                    LibroPagoRecibidas libroPagoRecib = new LibroPagoRecibidas(Log, utiles, LP);
                    this.dsConsultaRespuesta = libroPagoRecib.ObtenerDatosPagosRecibidas(this.compania, "", "" , this.nombreRazonSocial, nif_id,
                                                                                         this.facturaID.EmisorFacturaIdOtroCodPais, this.facturaID.EmisorFacturaIdOtroIdType,
                                                                                         this.facturaID.NumeroSerie, this.facturaID.FechaDocumento, this.facturaID.FechaDocumento,
                                                                                         this.agencia);
                }

                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.tgGridPagos.dsDatos.Tables.Count > 0)
                    {
                        //Eliminar los resultados de la búsqueda anterior
                        this.tgGridPagos.Visible = false;

                        if (this.tgGridPagos.dsDatos.Tables.Contains("DatosGenerales")) this.tgGridPagos.dsDatos.Tables.Remove("DatosGenerales");
                        if (this.tgGridPagos.dsDatos.Tables.Contains("Resumen")) this.tgGridPagos.dsDatos.Tables.Remove("Resumen");
                    }

                    if (this.dsConsultaRespuesta.Tables["DatosGenerales"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridPagos.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["DatosGenerales"].Copy());

                        //if (this.datosLocal) this.tgGridPagos.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resumen"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridPagos.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridPagos.DataSource = this.tgGridPagos.dsDatos.Tables["DatosGenerales"];
                        //this.tgGridPagos.dsDatos.DataSource = SBind;

                        foreach (var column in this.tgGridPagos.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }

                        //Cambiar los headers de las columnas del DataGrid 
                        this.CambiarColumnasEncabezadosPagosRecibidas();

                        this.tgGridPagos.Refresh();

                        this.tgGridPagos.Visible = true;
                        this.lblNoInfo.Visible = false;

                        //Falta Visualizar los totales
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridPagos.Visible = false;
                        this.lblNoInfo.Text = "No existen pagos recibidas que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridPagos.Visible = false;
                    this.lblNoInfo.Text = "No existen pagos recibidas que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsConsultaRespuesta);
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
                //Eliminar los resultados de la búsqueda anterior
                if (this.tgGridPagos.dsDatos.Tables.Contains("Resultado")) this.tgGridPagos.dsDatos.Tables.Remove("Resultado");

                //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                this.tgGridPagos.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resultado"].Copy());

                //Eliminar columnas
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("Compania")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("Compania");
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("Libro")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("Libro");
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("Operacion")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("Operacion");
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("NoFactura")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("NoFactura");
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("FechaDoc")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("FechaDoc");
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("RowResumen")) this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Remove("RowResumen");

                this.tgGridPagos.AutoGenerateColumns = true;

                //Poner como DataSource del DataGrid el DataTable creado
                this.tgGridPagos.DataSource = this.tgGridPagos.dsDatos.Tables["Resultado"];

                //Cambiar los headers de las columnas del DataGrid de Error
                if (this.tgGridPagos.dsDatos.Tables["Resultado"].Columns.Contains("Estado")) this.tgGridPagos.CambiarColumnHeader("Estado", "Error");  //Falta traducir

                this.tgGridPagos.Refresh();

                this.tgGridPagos.Visible = true;
                this.lblNoInfo.Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Pagos Recibidas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosPagosRecibidas()
        {
            try
            {
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("PagoFecha")) this.tgGridPagos.CambiarColumnHeader("PagoFecha", "Fecha de Pago");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("PagoImporte")) this.tgGridPagos.CambiarColumnHeader("PagoImporte", "Importe");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("PagoMedioDesc")) this.tgGridPagos.CambiarColumnHeader("PagoMedioDesc", "Medio de Pago");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("PagoCuentaOMedio")) this.tgGridPagos.CambiarColumnHeader("PagoCuentaOMedio", "Cuenta o Medio");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("NIFPresentador")) this.tgGridPagos.CambiarColumnHeader("NIFPresentador", "NIF Presentador");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("TimestampPresentacion")) this.tgGridPagos.CambiarColumnHeader("TimestampPresentacion", "Fecha y Hora");  //Falta traducir         
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridPagos.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("NumSerieFacturaEmisor")) this.tgGridPagos.CambiarColumnHeader("NumSerieFacturaEmisor", "No. Factura Emisor");  //Falta traducir
                if (this.tgGridPagos.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFacturaEmisor")) this.tgGridPagos.CambiarColumnHeader("FechaExpedicionFacturaEmisor", "Fecha Expedición Factura");  //Falta traducir         
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}
