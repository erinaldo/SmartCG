using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiSoapResponseView : frmPlantilla, IReLocalizable
    {
        private DataTable dtRespuesta = null;

        public frmSiiSoapResponseView()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (this.FormValid())
            {
                //Crear tabla de resultados
                this.dtRespuesta = LibroUtiles.CrearDataTableResultado();

                //Buscar la info en el SOAP para cada una de las facturas recibidas enviadas
                this.VerRespuestaFacturasRecibidas();

                if (dtRespuesta != null && dtRespuesta.Rows.Count > 0)
                {
                    this.gbDatosGrles.Visible = true;

                    //Visualizar respuesta
                    frmSiiSuministroInfoRespuesta frmInfoRespuesta = new frmSiiSuministroInfoRespuesta();
                    frmInfoRespuesta.Titulo = "Facturas Recibidas";
                    frmInfoRespuesta.LibroCodigo = LibroUtiles.LibroID_FacturasRecibidas;

                    frmInfoRespuesta.ExisteContraparte = false;
                    frmInfoRespuesta.ExisteClaveOperacion = false;

                    frmInfoRespuesta.DTRespuesta = this.dtRespuesta;
                    frmInfoRespuesta.ShowDialog();
                }
                else this.gbDatosGrles.Visible = false;
            }
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            //this.Text = this.LP.GetText("lblfrmSiiSuministroInfoTitulo", "Suministro de Información");
        }

        private bool FormValid()
        {
            return (true);
        }

        private void VerRespuestaFacturasRecibidas()
        {
            try
            {
                //Leer el SOAP en memoria
                //string content = new StreamReader(@"C:\Temp\SuministroLRFacturasRecibidas_respuesta.log", Encoding.UTF8).ReadToEnd();
                //string content = new StreamReader(@"C:\Temp\SIISoapRAW\2017-06-21_09-09-40_siiService_SuministroLRFacturasRecibidas_respuesta.log", Encoding.UTF8).ReadToEnd();

                string content = new StreamReader(this.txtFicheroSoapRespuesta.Text, Encoding.UTF8).ReadToEnd();
                

                XDocument doc = XDocument.Parse(content);
                XNamespace nssiiR = "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/fact/ws/RespuestaSuministro.xsd";
                XNamespace nssii = "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/fact/ws/SuministroInformacion.xsd";

                DataRow row;

                string csvEnvio = "";
                string timeStampEnvio = "";
                string idVersionSII = "";
                string companiaNombreRazonSocial = "";

                string companiaNIF = "";
                string companiaCodigo = "";
                string libro = "Facturas Recibidas";
                string operacion = "";
                string operacionDesc = "";
                string estadoEnvioCompleto = "";

                string nif = "";
                string idOtroCodPais = "";
                string idOtroIdType = "";
                string idOtroId = "";

                string numFactura = "";
                string fechaFactura = "";

                string estado = "";
                
                IEnumerable<XElement> RespuestaLRFacturasRecibidas = doc.Descendants(nssiiR + "RespuestaLRFacturasRecibidas");
                IEnumerable<XElement> responseDatosPresentacion;
                IEnumerable<XElement> responseCabecera;
                IEnumerable<XElement> responseTitular;

                IEnumerable<XElement> responsesLineas;
                IEnumerable<XElement> responseIDFactura;
                IEnumerable<XElement> responseIDEmisorFactura;
                IEnumerable<XElement> responseIDOtro;

                foreach (XElement respuestaLRFacturasRecibidas in RespuestaLRFacturasRecibidas)
                {
                    //Resumen del envío

                    //CSV
                    if (respuestaLRFacturasRecibidas.Element(nssiiR + "CSV") != null) csvEnvio = (string)respuestaLRFacturasRecibidas.Element(nssiiR + "CSV");
                    else csvEnvio = "";

                    //Datos de Presentacion
                    responseDatosPresentacion = respuestaLRFacturasRecibidas.Descendants(nssiiR + "DatosPresentacion");
                    foreach (XElement datosPresentacion in responseDatosPresentacion)
                    {
                        if (datosPresentacion.Element(nssii + "TimestampPresentacion") != null) timeStampEnvio = (string)datosPresentacion.Element(nssii + "TimestampPresentacion");
                    }

                    //Cabecera
                    responseCabecera = respuestaLRFacturasRecibidas.Descendants(nssiiR + "Cabecera");
                    foreach (XElement cabecera in responseCabecera)
                    {
                        //Version del SII
                        if (cabecera.Element(nssii + "IDVersionSii") != null) idVersionSII = (string)cabecera.Element(nssii + "IDVersionSii");

                        //Titular
                        responseTitular = cabecera.Descendants(nssii + "Titular");
                        foreach (XElement titular in responseTitular)
                        {
                            if (titular.Element(nssii + "NIF") != null)
                            {
                                companiaNIF = (string)titular.Element(nssii + "NIF");
                                if (companiaNIF != "") companiaCodigo = this.CompaniaFiscalCodigoDadoCIF(companiaNIF);
                            }

                            if (titular.Element(nssii + "NombreRazon") != null) companiaNombreRazonSocial = (string)titular.Element(nssii + "NombreRazon");
                        }

                        //Operacion
                        if (cabecera.Element(nssii + "TipoComunicacion") != null) operacion = (string)cabecera.Element(nssii + "TipoComunicacion");
                        if (operacion != "") operacionDesc = this.ObtenerDescripcionOperacion(operacion);
                    }

                    this.txtCSV.Text = csvEnvio;
                    this.txtCIFPresentador.Text = companiaNIF;
                    this.txtNombreRazonSocial.Text = companiaNombreRazonSocial;
                    this.txtFechaHoraPresentacion.Text = timeStampEnvio;
                    this.txtVersionSII.Text = idVersionSII;

                    //Estado envio completo
                    if (respuestaLRFacturasRecibidas.Element(nssiiR + "EstadoEnvio") != null) estadoEnvioCompleto = (string)respuestaLRFacturasRecibidas.Element(nssiiR + "EstadoEnvio");

                    //Fila Resumen
                    row = this.dtRespuesta.NewRow();
                    row["Compania"] = companiaCodigo;
                    row["Libro"] = libro;
                    row["Operacion"] = operacionDesc;
                    row["Estado"] = estadoEnvioCompleto;
                    row["NIFIdEmisor"] = "";
                    row["NoFactura"] = "";
                    row["FechaDoc"] = "";
                    row["NombreRazonSocial"] = "";
                    row["ClaveOperacion"] = "";   
                    row["NIF"] = "";
                    row["IdOtroCodPais"] = "";
                    row["IdOtroTipo"] = "";
                    row["IdOtroId"] = "";
                    row["RowResumen"] = "1";
                    this.dtRespuesta.Rows.Add(row);

                    //Tratamiento de las lineas de respuesta
                    responsesLineas = doc.Descendants(nssiiR + "RespuestaLinea");

                    //Para cada una de las líneas de respuesta
                    foreach (XElement responseLinea in responsesLineas)
                    {
                        //Identificador de la factura
                        responseIDFactura = responseLinea.Descendants(nssiiR + "IDFactura");
                        foreach (XElement idFactura in responseIDFactura)
                        {
                            nif = "";
                            idOtroCodPais = "";
                            idOtroIdType = "";
                            idOtroId = "";

                            //Identificador del emisor de la factura
                            responseIDEmisorFactura = idFactura.Descendants(nssii + "IDEmisorFactura");
                            foreach (XElement idEmisorFactura in responseIDEmisorFactura)
                            {
                                if (idEmisorFactura.Element(nssii + "NIF") != null)
                                {
                                    //NIF
                                    if (idEmisorFactura.Element(nssii + "NIF") != null) nif = (string)idEmisorFactura.Element(nssii + "NIF");
                                }
                                else
                                {
                                    //IDOtro
                                    if (idEmisorFactura.Element(nssii + "IDOtro") != null)
                                    {
                                        responseIDOtro = idEmisorFactura.Descendants(nssii + "IDOtro");

                                        foreach (XElement idOtro in responseIDOtro)
                                        {
                                            if (idOtro.Element(nssii + "CodigoPais") != null) idOtroCodPais = (string)idOtro.Element(nssii + "CodigoPais");

                                            if (idOtro.Element(nssii + "IDType") != null) idOtroIdType = (string)idOtro.Element(nssii + "IDType");

                                            if (idOtro.Element(nssii + "ID") != null) idOtroId = (string)idFactura.Element(nssii + "ID");
                                        }
                                    }
                                }
                            }

                            //Número de Serie de la Factura
                            if (idFactura.Element(nssii + "NumSerieFacturaEmisor") != null) numFactura = (string)idFactura.Element(nssii + "NumSerieFacturaEmisor");
                            else numFactura = "";

                            //Fecha de Expedicion de la factura
                            if (idFactura.Element(nssii + "FechaExpedicionFacturaEmisor") != null) fechaFactura = (string)idFactura.Element(nssii + "FechaExpedicionFacturaEmisor");
                            else fechaFactura = "";
                        }

                        //Estado de la factura
                        if (responseLinea.Element(nssiiR + "EstadoRegistro") != null) estado = (string)responseLinea.Element(nssiiR + "EstadoRegistro");
                        else estado = "";

                        //Fila Factura
                        row = this.dtRespuesta.NewRow();
                        row["Compania"] = companiaCodigo;
                        row["Libro"] = libro;
                        row["Operacion"] = operacionDesc;
                        row["Estado"] = estado;

                        if (nif != "") row["NIFIdEmisor"] = nif;
                        else
                        {
                            if (idOtroCodPais != "") row["NIFIdEmisor"] = idOtroCodPais + "-" + idOtroIdType + "-" + idOtroId;
                            else row["NIFIdEmisor"] = idOtroIdType + "-" + idOtroId;
                        }

                        row["NoFactura"] = numFactura;
                        row["FechaDoc"] = fechaFactura;
                        row["NombreRazonSocial"] = "";
                        row["ClaveOperacion"] = "";
                        row["NIF"] = nif;
                        row["IdOtroCodPais"] = idOtroCodPais;
                        row["IdOtroTipo"] = idOtroIdType;
                        row["IdOtroId"] = idOtroId;
                        row["RowResumen"] = "0";
                        this.dtRespuesta.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve el código de la compañía fiscal dado un cif, si no lo encuentra devuelve el cif
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Valida la existencia o no de la compañía fiscal</returns>
        public string CompaniaFiscalCodigoDadoCIF(string cif)
        {
            string result = cif;
            IDataReader dr = null;

            try
            {
                string query = "select CIAFS0 from " + GlobalVar.PrefijoTablaCG + "IVSII0 ";
                query += "where NIFDS0 = '" + cif + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result = dr.GetValue(dr.GetOrdinal("CIAFS0")).ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        private void btnSelSoapRespuesta_Click(object sender, EventArgs e)
        {
            //this.openFileDialog1.Filter = "Archivos W00|*W00.txt";
            this.openFileDialog1.Title = this.LP.GetText("lblSelArchSoapRespuesta", "Seleccionar archivos SOAP con la respuesta de la AEAT");

            this.openFileDialog1.FileName = "";

            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.txtFicheroSoapRespuesta.Text = this.openFileDialog1.FileName;
            }
        }
    }
}