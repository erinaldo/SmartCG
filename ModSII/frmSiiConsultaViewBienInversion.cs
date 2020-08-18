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
    public partial class frmSiiConsultaViewBienInversion : frmPlantilla, IReLocalizable
    {
        private string ejercicio;
        private string periodo;
        private DataRow rowDatosGrles;
        private DataRow rowMasInfo;
        private FacturaIdentificador facturaID;

        private bool datosLocal = false;

        #region Propiedades
        public string Ejercicio
        {
            get
            {
                return (this.ejercicio);
            }
            set
            {
                this.ejercicio = value;
            }
        }

        public string Periodo
        {
            get
            {
                return (this.periodo);
            }
            set
            {
                this.periodo = value;
            }
        }

        public DataRow RowDatosGrles
        {
            get
            {
                return (this.rowDatosGrles);
            }
            set
            {
                this.rowDatosGrles = value;
            }
        }

        public DataRow RowMasInfo
        {
            get
            {
                return (this.rowMasInfo);
            }
            set
            {
                this.rowMasInfo = value;
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

        public frmSiiConsultaViewBienInversion()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiConsultaViewBienInversion_Load(object sender, EventArgs e)
        {
            if (this.datosLocal)
            {
                Log.Info("INICIO SII Consulta Ver Bienes de Inversión (datos en local)");
            }
            else
            {
                Log.Info("INICIO SII Consulta Ver Bienes de Inversión (datos en Hacienda)");
            }

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Escribe la información de la cabecera de la factura
            this.EscribirDatosCabecera();

            //Escribe la información del apartado de Datos Generales de la factura
            this.EscribirDatosGenerales();

            this.ActiveControl = this.lblEjercicio;
        }

        private void toolStripButtonVerMovimientos_Click(object sender, EventArgs e)
        {
            frmSiiConsultaListaMovimientos frmListaMovs = new frmSiiConsultaListaMovimientos();
            frmListaMovs.LibroID = LibroUtiles.LibroID_BienesInversion;
            frmListaMovs.FacturaID = this.facturaID;
            frmListaMovs.Ejercicio = this.ejercicio;
            frmListaMovs.Periodo = this.periodo;
            frmListaMovs.IDEmisorFactura = this.txtNIFEmisor.Text;
            frmListaMovs.Show(this);
        }

        private void frmSiiConsultaViewBienInversion_Shown(object sender, EventArgs e)
        {
            //if (true)
            if (this.agencia == "C")
            {
                this.lblNumRegistroAcuerdoFacturacion.Text = "Número Registro Autorización Facturación";
            }
        }

        private void frmSiiConsultaViewBienInversion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaViewBienInversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.datosLocal)
            {
                Log.Info("FIN SII Consulta Ver Bienes de Inversión (datos en local)");
            }
            else
            {
                Log.Info("FIN SII Consulta Ver Bienes de Inversión (datos en Hacienda)");
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            if (this.datosLocal)
            {
                this.Text = this.LP.GetText("lblfrmSiiConsultaViewBienesInversionLocalTitulo", "Consulta de Bienes de Inversión (datos en local)");     //Falta traducir
            }
            else
            {
                this.Text = this.LP.GetText("lblfrmSiiConsultaViewBienesInversionTitulo", "Consulta de Bienes de Inversión (datos en Hacienda)");     //Falta traducir
            }
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Escribe los datos de la cabecera de la factura
        /// </summary>
        private void EscribirDatosCabecera()
        {
            try
            {
                this.txtEjercicio.Text = this.ejercicio;
                this.txtPeriodo.Text = this.periodo;

                if (this.rowDatosGrles != null)
                {
                    this.txtNIFEmisor.Text = this.rowDatosGrles["IDEmisorFactura"].ToString();
                    this.txtNoFact.Text = this.rowDatosGrles["NumSerieFacturaEmisor"].ToString();
                    this.txtFechaDoc.Text = this.rowDatosGrles["FechaExpedicionFacturaEmisor"].ToString();

                    this.txtEstadoFactura.Text = this.rowDatosGrles["EstadoFactura"].ToString();
                    this.txtFechaUltimaModificacion.Text = this.rowDatosGrles["TimestampUltimaModificacion"].ToString();

                    if (this.txtEstadoFactura.Text == "Correcta")
                    {
                        this.txtError.Text = "";
                        this.lblError.Enabled = false;
                    }
                    else
                    {
                        string codigoError = this.rowDatosGrles["CodigoErrorRegistro"].ToString();
                        string error = this.rowDatosGrles["DescripcionErrorRegistro"].ToString();

                        if (codigoError != "" && error != "") this.txtError.Text = codigoError + " - " + error;
                        else
                        {
                            if (codigoError != "" && error == "") this.txtError.Text = codigoError;
                            else if (codigoError == "" && error != "") this.txtError.Text = error;
                        }
                        this.lblError.Enabled = true;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe los datos del apartado de Datos Generales de la factura
        /// </summary>
        private void EscribirDatosGenerales()
        {
            try
            {
                if (this.rowDatosGrles != null)
                {
                    this.txtIdentificacionBien.Text = this.rowDatosGrles["IdentificacionBien"].ToString();
                    this.txtFechaInicioUtilizacion.Text = this.rowDatosGrles["FechaInicioUtilizacion"].ToString();
                    this.txtRefExterna.Text = this.rowDatosGrles["RefExterna"].ToString();

                    if (this.datosLocal)
                    {
                        this.txtProrrataAnualDefinitiva.Text = this.rowDatosGrles["ProrrataAnualDefinitiva"].ToString();
                        this.txtRegulacionAnualDeduccion.Text = this.rowDatosGrles["RegulacionAnualDeduccion"].ToString();
                        this.txtRegularizacionDeduccionEfectuada.Text = this.rowDatosGrles["RegularizacionDeduccionEfectuada"].ToString();
                    }
                }

                if (this.rowMasInfo != null)
                {
                    this.txtIdentificacionEntrega.Text = this.rowMasInfo["IdentificacionEntrega"].ToString();
                    this.txtDatosPresentacionNIF.Text = this.rowMasInfo["NIFPresentador"].ToString();
                    this.txtDatosPresentacionFechaHora.Text = this.rowMasInfo["TimestampPresentacion"].ToString();
                    this.txtNumRegistroAcuerdoFacturacion.Text = this.rowMasInfo["NumRegistroAcuerdoFacturacion"].ToString();

                    if (!this.datosLocal)
                    {
                        this.txtProrrataAnualDefinitiva.Text = this.rowMasInfo["ProrrataAnualDefinitiva"].ToString();
                        this.txtRegulacionAnualDeduccion.Text = this.rowMasInfo["RegulacionAnualDeduccion"].ToString();
                        this.txtRegularizacionDeduccionEfectuada.Text = this.rowMasInfo["RegularizacionDeduccionEfectuada"].ToString();
                    }

                    this.txtEntidadSucedidaNIF.Text = this.rowMasInfo["EntidadSucedidaNIF"].ToString();
                    this.txtEntidadSucedidaNombreRazonSocial.Text = this.rowMasInfo["EntidadSucedidaNombreRazonSocial"].ToString();
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion        
    }
}
