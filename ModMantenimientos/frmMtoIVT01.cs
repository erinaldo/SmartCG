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
using Telerik.WinControls.UI;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoIVT01 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCODIVA";

        private bool nuevo;
        private bool copiar;
        private string codigo;

        private string nombreCodIVA;
        private string codigoPlan;
        private string nombrePlan;
        private string codigoIVACopiar;
        private bool planActivo;

        private string codigoCtaMayor;
        private string codigoTipoAux;
        private string codigoCtaMayorCont;
        private string codigoIVACont;
        private string codigoIVAAsociado;

        private string codigoCLMod303GrupoOperac;
        private string codigoCLMod390GrupoOperac;
        private string codigoCLMod340LibroRegistro;
        private string codigoCLMod340ClaveOperac;
        private string codigoCLMod349TipoOperac;
        private string codigoCLMod190ClavePercep;
        private string codigoCLMod111ClavePercep;        
        
        private string proveedorTipo;

        private string prefijoTablaCGCDES;

        private bool isCheckedCLDinerario = false;
        private bool isCheckedCLEspecie = false;

        private bool existeCodigoIVA = false;
        private bool modificarEliminarCodigoIVA = true;

        //private DataTable dtTiposIVA;
        private DataTable dtTiposIVAGrid;

        private bool cargarInfoDeTiposIVA = true;
        private bool nuevoRegistro = false;
        //bool grabarTipoIVA = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;
        public Boolean bEliminar = false;
        public Boolean bGrabar = false;

        //TextBox que se utiliza en el DataGridView de Detalles para validar que las monedas y el importe sean sólo numéricos
        TextBox tb;

        private const string SIILibroFactExpedidas = "Facturas Expedidas";
        private const string SIILibroFactRecibidas = "Facturas Recibidas";
        private const string SIILibroOperacionesSeguro = "Negocio Operaciones de Seguro";
        private const string SIILibroOperacionesIntracomunitarias = "Determinadas Operaciones Intracomunitarias";

        private const string SIILibroFactExpedidas_TIpoDesgloseFactura = "Facturas";
        private const string SIILibroFactExpedidas_TIpoDesgloseEntrega = "Operación de Entrega";
        private const string SIILibroFactExpedidas_TIpoDesgloseOpServicio = "Operación de Servicios";

        private const string SIILibroFactRecibidas_TIpoDesgloseIVA = "IVA";
        private const string SIILibroFactRecibidas_TIpoDesgloseInvSujPasivo = "Inversión Sujeto Pasivo";

        private string sii_agenciaTributaria;

        private string sii_factE_R_tipoFactura;
        private string sii_factE_R_claveRegEspeTrasc;
        private string sii_factE_R_claveRegEspeTrascAdicional1;
        private string sii_factE_R_claveRegEspeTrascAdicional2;
        private string sii_factE_R_tipoFacturaRectif;
        private string sii_factE_R_tipoDesglose;
        private string sii_factE_R_factSimplArt72;

        private string sii_opSeg_tipoOperacion;
        private string sii_opIntracom_tipoOperacion;

        private string sii_factE_tipoOperacionSujetaNoExenta;
        private string sii_factE_causaExencionOpSujetasExentas;
        private string sii_factE_tipoOperacNoSujeta;
        private string sii_factE_cobroMetalico;
        private string sii_factE_cupon;
        private string sii_factE_taxFree;
        private string sii_factE_viajeros;

        private string sii_factR_bienesInversion;
        private string sii_factR_prorrata;
        private string sii_factR_excluirBaseTotalFacturaProrrata;
        private decimal sii_factR_tipoImpositivoProrrata;

        private string sii_factR_campoLibre;
        private string sii_factE_campoLibre;

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

        public string CodigoPlan
        {
            get
            {
                return (this.codigoPlan);
            }
            set
            {
                this.codigoPlan = value;
            }
        }

        public string NombrePlan
        {
            get
            {
                return (this.nombrePlan);
            }
            set
            {
                this.nombrePlan = value;
            }
        }

        public string NombreCodIVA
        {
            get
            {
                return (this.nombreCodIVA);
            }
            set
            {
                this.nombreCodIVA = value;
            }
        }

        public bool Copiar
        {
            get
            {
                return (this.copiar);
            }
            set
            {
                this.copiar = value;
            }
        }


        public string CodigoIVACopiar
        {
            get
            {
                return (this.codigoIVACopiar);
            }
            set
            {
                this.codigoIVACopiar = value;
            }
        }

        public bool PlanActivo
        {
            get
            {
                return (this.planActivo);
            }
            set
            {
                this.planActivo = value;
            }
        }

        public frmMtoIVT01()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchCuadreCuotas.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchCuadreCuotas.ThemeName = "MaterialBlueGrey";

            this.gbCLMod180ModalPercep.ElementTree.EnableApplicationThemeName = false;
            this.gbCLMod180ModalPercep.ThemeName = "ControlDefault";

            this.gbContrapartida.ElementTree.EnableApplicationThemeName = false;
            this.gbContrapartida.ThemeName = "ControlDefault";

            this.gbDeducibleNoDed.ElementTree.EnableApplicationThemeName = false;
            this.gbDeducibleNoDed.ThemeName = "ControlDefault";

            this.gbRepercSopor.ElementTree.EnableApplicationThemeName = false;
            this.gbRepercSopor.ThemeName = "ControlDefault";

            this.gbTipoIVA.ElementTree.EnableApplicationThemeName = false;
            this.gbTipoIVA.ThemeName = "ControlDefault";

            //Eliminar los botones (close y navegación) del control RadPageView
            Telerik.WinControls.UI.RadPageViewStripElement stripElement = (Telerik.WinControls.UI.RadPageViewStripElement)this.radPageViewDatos.ViewElement;
            stripElement.StripButtons = Telerik.WinControls.UI.StripViewButtons.None;

            this.radPanelSiiFactExpedidas.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;

            this.radToggleSwitchSiiCobroMetalico.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiCobroMetalico.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiCupon.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiCupon.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiFactSimpArt72_73.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiFactSimpArt72_73.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiTaxFree.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiTaxFree.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiViajeros.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiViajeros.ThemeName = "MaterialBlueGrey";

            this.radPanelSiiFactRecibidas.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;

            this.radToggleSwitchSiiBienesInversionFactRecibida.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiBienesInversionFactRecibida.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiProrrataFactRecibida.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiProrrataFactRecibida.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.ThemeName = "MaterialBlueGrey";

            this.radPanelSiiOperacionesSeguros.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;

            this.radPanelSiiOperacionesIntracomunitarias.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales(true);
        }

        private void FrmMtoIVT01_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Códigos de IVA Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Leer el proveedor de tipo de datos
            this.proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Traducir los literales del formulario
            this.TraducirLiterales(false);

            //Ajustar el control de selección del plan
            this.radTextBoxControlPlan.Text = this.nombrePlan;
            this.radTextBoxControlPlan.IsReadOnly = true;
            
            //Construir el DataGrid
            this.BuildDataGridTiposIVA();

            //Inicializar los desplegables del apartado del SII Libros
            string[] valores = new string[] { " ", SIILibroFactExpedidas, SIILibroFactRecibidas, SIILibroOperacionesSeguro, SIILibroOperacionesIntracomunitarias };
            utiles.CreateRadDropDownListElement(ref this.cmbSiiLibros, ref valores);

            valores = new string[] { SIILibroFactExpedidas_TIpoDesgloseFactura, SIILibroFactExpedidas_TIpoDesgloseEntrega, SIILibroFactExpedidas_TIpoDesgloseOpServicio};
            utiles.CreateRadDropDownListElement(ref this.radDropDownListSiiTipoDesglose, ref valores);

            this.radDropDownListSiiTipoDesglose.SelectedIndex = 0;

            valores = new string[] { SIILibroFactRecibidas_TIpoDesgloseIVA, SIILibroFactRecibidas_TIpoDesgloseInvSujPasivo };
            utiles.CreateRadDropDownListElement(ref this.radDropDownListSiiTipoDesgloseFactRecibida, ref valores);

            this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex = 0;

            //Ocultar los paneles del apatado del SII Libros
            this.radPanelSiiFactExpedidas.Visible = false;
            this.radPanelSiiFactRecibidas.Visible = false;
            this.radPanelSiiOperacionesSeguros.Visible = false;
            this.radPanelSiiOperacionesIntracomunitarias.Visible = false;

            //Prefijo para la tabla de Descripciones (CGCDES)
            this.prefijoTablaCGCDES = "";
            if (this.proveedorTipo == "DB2")
            {
                this.prefijoTablaCGCDES = ConfigurationManager.AppSettings["bbddCGAPP"];
                if (this.prefijoTablaCGCDES != null && this.prefijoTablaCGCDES != "") this.prefijoTablaCGCDES += ".";
            }
            else this.prefijoTablaCGCDES = GlobalVar.PrefijoTablaCG;

            for (int i = 0; i < this.radGridViewTiposIVA.Columns.Count; i++)
            {
                this.radGridViewTiposIVA.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
            }

            this.radGridViewTiposIVA.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewTiposIVA.MasterTemplate.BestFitColumns();

            this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;

            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                this.txtMaxDescuadreCuotas.Text = "0,00";

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
                if (this.copiar)
                {
                    //------------ COPIAR --------------
                    //this.HabilitarDeshabilitarControles(false);
                    utiles.ButtonEnabled(ref this.radButtonSave, false);
                    utiles.ButtonEnabled(ref this.radButtonDelete, false);
                    
                    //Mostrar la información de la cuenta en los controles
                    this.CargarInfoCodigoIVA(this.codigoIVACopiar);

                    this.ActiveControl = this.txtCodigo;
                    this.txtCodigo.Select(0, 0);
                    this.txtCodigo.Focus();
                }
                else
                {
                    this.txtCodigo.Text = this.codigo;
                    this.txtCodigo.IsReadOnly = true;

                    //Recuperar la información del código de IVA y mostrarla en los controles
                    this.CargarInfoCodigoIVA(this.Codigo);

                    this.ActiveControl = this.txtDescripcion;
                    this.txtDescripcion.Select(0, 0);
                    this.txtDescripcion.Focus();
                
                }

            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized; //SMR
        }

        private void TxtLibro_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);

            //Valida que sólo se introduzca letra o número
            utiles.ValidarSoloLetraNumeroKeyPress(ref this.txtLibro, ref sender, ref e, true);
        }

        private void TxtSerie_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);

            //Valida que sólo se introduzca letra o número
            utiles.ValidarSoloLetraNumeroKeyPress(ref this.txtSerie, ref sender, ref e, true);
        }

        private void TxtMaxDescuadreCuotas_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números con 2 posiciones decimales
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtMaxDescuadreCuotas, false, ref sender, ref e);
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codIVAAux = this.txtCodigo.Text.Trim();

            //SMR if (!this.nuevo && codIVAAux == "")
            if (this.nuevo && codIVAAux == "")
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCodigo.Text = "";
                this.txtCodigo.Focus();

                RadMessageBox.Show(this.LP.GetText("errCodIVAOblig", "Código de iva obligatorio"), this.LP.GetText("errValCodIVA", "Error"));
                bTabulador = false;
                return;
            }

            if (codIVAAux != "")
            {
                bool codIVAAuxOk = true;
                if (this.nuevo) codIVAAuxOk = this.CodigoIVAValido();    //Verificar que el codigo no exista

                if (codIVAAuxOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActivo.Value = true;
                        this.radToggleSwitchEstadoActivo.ReadOnly = true;

                        //Ocultar los controles de claves legales para repercutido
                        this.MostrarControlesRepercutido(false);
                    }
                    this.txtCodigo.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);

                    this.codigo = this.txtCodigo.Text;
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show(this.LP.GetText("errCodIVAExiste", "Código de iva ya existe"), this.LP.GetText("errValCodIVAExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                    this.codigo = null;
                }
            }
            bTabulador = false;
        }

        private void RbSoportado_CheckedChanged(object sender, EventArgs e)
        {
            //Cambiar las selects de selección que están filtradas por repercutido / soportado
            this.radButtonTextBoxCLMod303GrupoOperac.Text = "";
            //this.tgTexBoxSelCLMod303GrupoOperac.QueryFormSel = this.ObtenerQueryCLMod303GrupoOperac();
            this.radButtonTextBoxCLMod390GrupoOperac.Text = "";
            //this.tgTexBoxSelCLMod390GrupoOperac.QueryFormSel = this.ObtenerQueryCLMod390GrupoOperac();

            if (this.rbRepercutido.IsChecked) MostrarControlesRepercutido(true);
            else MostrarControlesRepercutido(false);
        }

        private void RbRepercutido_CheckedChanged(object sender, EventArgs e)
        {
            //Cambiar las selects de selección que están filtradas por repercutido / soportado
            this.radButtonTextBoxCLMod303GrupoOperac.Text = "";
            //this.tgTexBoxSelCLMod303GrupoOperac.QueryFormSel = this.ObtenerQueryCLMod303GrupoOperac();
            this.radButtonTextBoxCLMod390GrupoOperac.Text = "";
            //this.tgTexBoxSelCLMod390GrupoOperac.QueryFormSel = this.ObtenerQueryCLMod390GrupoOperac();

            if (this.rbRepercutido.IsChecked)
            {
                MostrarControlesRepercutido(true);
                this.rbNoDeducible.IsChecked = true;
                this.gbDeducibleNoDed.Enabled = false;
            }
            else
            {
                MostrarControlesRepercutido(false);
                this.gbDeducibleNoDed.Enabled = true;
            }
        }

        private void RbCLDinerario_CheckedChanged(object sender, EventArgs e)
        {
            isCheckedCLDinerario = this.rbCLDinerario.IsChecked;
        }

        private void RbCLDinerario_Click(object sender, EventArgs e)
        {
            if (this.rbCLDinerario.IsChecked && !isCheckedCLDinerario)
                this.rbCLDinerario.IsChecked = false;
            else
            {
                this.rbCLDinerario.IsChecked = true;
                isCheckedCLDinerario = false;
            }
        }

        private void RbCLEspecie_CheckedChanged(object sender, EventArgs e)
        {
            isCheckedCLEspecie = this.rbCLEspecie.IsChecked;
        }

        private void RbCLEspecie_Click(object sender, EventArgs e)
        {
            if (this.rbCLEspecie.IsChecked && !isCheckedCLEspecie)
                this.rbCLEspecie.IsChecked = false;
            else
            {
                this.rbCLEspecie.IsChecked = true;
                isCheckedCLEspecie = false;
            }
        }
        /*
        private void dgTipoIVA_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            TGGrid tgGridTiposIVA = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridTiposIVA.CurrentCell;

            string columnName = tgGridTiposIVA.Columns[celdaActiva.ColumnIndex].Name;

            e.Control.KeyPress -= tb_KeyPress;
            e.Control.KeyPress -= tb_NADA_KeyPress;

            switch (columnName)
            {
                case "PorcIVA":
                case "PorcRecEquiv":
                    e.Control.KeyPress -= tb_KeyPress;
                    e.Control.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                    break;
                default:
                    e.Control.KeyPress -= tb_KeyPress;
                    e.Control.KeyPress += new KeyPressEventHandler(tb_NADA_KeyPress);
                    break;
            }
        }
        */
        void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb = sender as TextBox;
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.tb, true, ref sender, ref e);
        }

        void Tb_NADA_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = e.KeyChar;
        }

        private void TxtLibro_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtSerie_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtSiiTipoImpositivoProrrataFactRecibida_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números con 2 posiciones decimales
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtSiiTipoImpositivoProrrataFactRecibida, false, ref sender, ref e);
        }

        private void RadButtonTextBoxCtaMayor_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codCtaMayor = this.radButtonTextBoxCtaMayor.Text.Trim();
            if (codCtaMayor != "")
            {
                Cursor.Current = Cursors.WaitCursor;

                string[] cuentaDesc = codCtaMayor.Split('-');

                if (cuentaDesc.Length >= 1) this.codigoCtaMayor = cuentaDesc[0].Trim();
                else
                {
                    if (codCtaMayor.Length <= 15) this.codigoCtaMayor = this.radButtonTextBoxCtaMayor.Text;
                    else this.codigoCtaMayor = this.radButtonTextBoxCtaMayor.Text.Substring(0, 15);
                }

                string result = ValidarCuentaMayor(this.codigoCtaMayor);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCtaMayor.Focus();
                }
                else
                {
                    codCtaMayor = this.codigoCtaMayor;
                    string codCtaMayorDesc = utilesCG.ObtenerDescDadoCodigo("GLM03", "CUENMC", "NOABMC", codCtaMayor, false, "").Trim();
                    if (codCtaMayorDesc != "") codCtaMayor += " " + separadorDesc + " " + codCtaMayorDesc;

                    this.radButtonTextBoxCtaMayor.Text = codCtaMayor;
                }

                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonTextBoxTipoAux_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigoTipoAux = this.radButtonTextBoxTipoAux.Text.Trim();
            if (codigoTipoAux != "")
            {
                Cursor.Current = Cursors.WaitCursor;

                if (codigoTipoAux.Length <= 2) this.codigoTipoAux = this.radButtonTextBoxTipoAux.Text;
                else codigoTipoAux = this.radButtonTextBoxTipoAux.Text.Substring(0, 2);

                string result = ValidarTipoAux(codigoTipoAux);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxTipoAux.Focus();
                }
                else
                {
                    string tipoAux = codigoTipoAux;
                    string tipoAuxDesc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", codigoTipoAux, false, "").Trim();
                    if (tipoAuxDesc != "") tipoAux += " " + separadorDesc + " " + tipoAuxDesc;

                    this.radButtonTextBoxTipoAux.Text = tipoAux;
                }

                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonTextBoxCtaMayorContrap_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigoCtaMayorContrapartida = this.radButtonTextBoxCtaMayorContrap.Text.Trim();

            if (codigoCtaMayorContrapartida != "")
            {
                Cursor.Current = Cursors.WaitCursor;
                
                if (codigoCtaMayorContrapartida.Length <= 15) this.codigoCtaMayorCont = this.radButtonTextBoxCtaMayorContrap.Text;
                else this.codigoCtaMayorCont = this.radButtonTextBoxCtaMayorContrap.Text.Substring(0, 15);

                // jl 30/06 if (codigoCtaMayorContrapartida.Substring(0,1) != "+") jl 30/06
                // jl 30/06 {
                    string result = ValidarCuentaMayorContrap(this.codigoCtaMayorCont);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        bTabulador = false;
                        this.radButtonTextBoxCtaMayorContrap.Focus();
                    }
                    else
                    {
                        codigoCtaMayorContrapartida = this.codigoCtaMayorCont;
                        string codigoCtaMayorContrapartidaDesc = utilesCG.ObtenerDescDadoCodigo("GLM03", "CUENMC", "NOLAAD", codigoCtaMayorContrapartida, false, "").Trim();
                        if (codigoCtaMayorContrapartidaDesc != "") codigoCtaMayorContrapartida += " " + separadorDesc + " " + codigoCtaMayorContrapartidaDesc;

                        this.radButtonTextBoxCtaMayorContrap.Text = codigoCtaMayorContrapartida;
                    }
                // jl 30/06 }
                // jl 30/06 else  
                // jl 30/06 {
                // jl 30/06 string result = ValidarCodigoIVAContrap(this.codigoCtaMayorCont.Substring(1, 2));

                // jl 30/06 if (result != "")
                // jl 30/06 {
                // jl 30/06 string error = this.LP.GetText("errValTitulo", "Error");
                // jl 30/06 RadMessageBox.Show(result, error);
                // jl 30/06 bTabulador = false;
                // jl 30/06 this.radButtonTextBoxCodIVAContrap.Focus();
                // jl 30/06 }
                // jl 30/06 }

                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonTextBoxCodIVAContrap_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigoIVAContrapartida = this.radButtonTextBoxCodIVAContrap.Text.Trim();

            if (codigoIVAContrapartida != "")
            {
                Cursor.Current = Cursors.WaitCursor;

                if (codigoIVAContrapartida.Length <= 2) this.codigoIVACont = this.radButtonTextBoxCodIVAContrap.Text;
                else this.codigoIVACont = this.radButtonTextBoxCodIVAContrap.Text.Substring(0, 2);

                string result = ValidarCodigoIVAContrap(this.codigoIVACont);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCodIVAContrap.Focus();
                }
                else
                {
                    codigoIVAContrapartida = this.codigoIVACont;
                    string codigoIVAContrapartidaDesc = utilesCG.ObtenerDescDadoCodigo("IVT01", "COIVCI", "NOMBCI", codigoIVAContrapartida, false, "").Trim();
                    if (codigoIVAContrapartidaDesc != "") codigoIVAContrapartida += " " + separadorDesc + " " + codigoIVAContrapartidaDesc;

                    this.radButtonTextBoxCodIVAContrap.Text = codigoIVAContrapartida;
                }

                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonTextBoxCodIVAAsociado_Leave(object sender, EventArgs e)
        {

            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigoIVAAsoc = this.radButtonTextBoxCodIVAAsociado.Text.Trim();

            if (codigoIVAAsoc != "")
            {
                Cursor.Current = Cursors.WaitCursor;

                if (codigoIVAAsoc.Length <= 2) this.codigoIVAAsociado = this.radButtonTextBoxCodIVAAsociado.Text;
                else this.codigoIVAAsociado = this.radButtonTextBoxCodIVAAsociado.Text.Substring(0, 2);

                string result = ValidarCodigoIVAAsociado(this.codigoIVAAsociado);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCodIVAAsociado.Focus();
                }
                else
                {
                    codigoIVAAsoc = this.codigoIVAAsociado;
                    string codigoIVAAsocDesc = utilesCG.ObtenerDescDadoCodigo("IVT01", "COIVCI", "NOMBCI", codigoIVAAsoc, false, "").Trim();
                    if (codigoIVAAsocDesc != "") codigoIVAAsoc += " " + separadorDesc + " " + codigoIVAAsocDesc;

                    this.radButtonTextBoxCodIVAAsociado.Text = codigoIVAAsoc;
                }

                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonTextBoxCLMod303GrupoOperac_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod303GrupoOperac.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (codigo.Length <= 2) this.codigoCLMod303GrupoOperac = this.radButtonTextBoxCLMod303GrupoOperac.Text;
                else this.codigoCLMod303GrupoOperac = this.radButtonTextBoxCLMod303GrupoOperac.Text.Substring(0, 2);

                string codigoCLMod303GrupoOperacDesc = "";
                string result = this.ValidarCLMod303GrupoOperacDesc(this.codigoCLMod303GrupoOperac, ref codigoCLMod303GrupoOperacDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod303GrupoOperac.Focus();
                }
                else
                {
                    string codigoCLMod303GrupoOperacAux = this.codigoCLMod303GrupoOperac;
                    if (codigoCLMod303GrupoOperacDesc != "") codigoCLMod303GrupoOperacAux += " " + separadorDesc + " " + codigoCLMod303GrupoOperacDesc;

                    this.radButtonTextBoxCLMod303GrupoOperac.Text = codigoCLMod303GrupoOperacAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
                //this.radButtonTextBoxCLMod303GrupoOperac.Focus();
            //}
        }

        private void RadButtonTextBoxCLMod390GrupoOperac_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod390GrupoOperac.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.codigoCLMod390GrupoOperac = this.radButtonTextBoxCLMod390GrupoOperac.Text;
                else this.codigoCLMod390GrupoOperac = this.radButtonTextBoxCLMod390GrupoOperac.Text.Substring(0, 2);

                string codigoCLMod390GrupoOperacDesc = "";
                string result = this.ValidarCLMod390GrupoOperacDesc(this.codigoCLMod390GrupoOperac, ref codigoCLMod390GrupoOperacDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod390GrupoOperac.Focus();
                }
                else
                {
                    string codigoCLMod390GrupoOperacAux = this.codigoCLMod390GrupoOperac;
                    if (codigoCLMod390GrupoOperacDesc != "") codigoCLMod390GrupoOperacAux += " " + separadorDesc + " " + codigoCLMod390GrupoOperacDesc;

                    this.radButtonTextBoxCLMod390GrupoOperac.Text = codigoCLMod390GrupoOperacAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxCLMod390GrupoOperac.Focus();
            //}
        }

        private void RadButtonTextBoxCLMod340LibroRegistro_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod340LibroRegistro.Text.Trim();
            if (codigo != "") // && codigo.Length >= 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 1) this.codigoCLMod340LibroRegistro = this.radButtonTextBoxCLMod340LibroRegistro.Text;
                else this.codigoCLMod340LibroRegistro = this.radButtonTextBoxCLMod340LibroRegistro.Text.Substring(0, 1);

                string codigoCLMod340LibroRegistroDesc = "";
                string result = this.ValidarCLMod340LibroRegistroDesc(this.codigoCLMod340LibroRegistro, ref codigoCLMod340LibroRegistroDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod340LibroRegistro.Focus();
                }
                else
                {
                    string codigoCLMod340LibroRegistroAux = this.codigoCLMod340LibroRegistro;
                    if (codigoCLMod340LibroRegistroDesc != "") codigoCLMod340LibroRegistroAux += " " + separadorDesc + " " + codigoCLMod340LibroRegistroDesc;

                    this.radButtonTextBoxCLMod340LibroRegistro.Text = codigoCLMod340LibroRegistroAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
                //this.radButtonTextBoxCLMod340LibroRegistro.Focus();
            //}
        }

        private void RadButtonTextBoxCLMod340ClaveOperac_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod340ClaveOperac.Text.Trim();
            if (codigo != "" ) //&& codigo.Length >= 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 1) this.codigoCLMod340ClaveOperac = this.radButtonTextBoxCLMod340ClaveOperac.Text;
                else this.codigoCLMod340ClaveOperac = this.radButtonTextBoxCLMod340ClaveOperac.Text.Substring(0, 1);

                string codigoCLMod340ClaveOperacDesc = "";
                string result = this.ValidarCLMod340ClaveOperacDesc(this.codigoCLMod340ClaveOperac, ref codigoCLMod340ClaveOperacDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod340ClaveOperac.Focus();
                }
                else
                {
                    string codigoCLMod340ClaveOperacAux = this.codigoCLMod340ClaveOperac;
                    if (codigoCLMod340ClaveOperacDesc != "") codigoCLMod340ClaveOperacAux += " " + separadorDesc + " " + codigoCLMod340ClaveOperacDesc;

                    this.radButtonTextBoxCLMod340ClaveOperac.Text = codigoCLMod340ClaveOperacAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
                //this.radButtonTextBoxCLMod340ClaveOperac.Focus();
            //}
        }

        private void RadButtonTextBoxlCLMod349TipoOperac_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxlCLMod349TipoOperac.Text.Trim();
            if (codigo != "") //&& codigo.Length >= 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 1) this.codigoCLMod349TipoOperac = this.radButtonTextBoxlCLMod349TipoOperac.Text;
                else this.codigoCLMod349TipoOperac = this.radButtonTextBoxlCLMod349TipoOperac.Text.Substring(0, 1);

                string codigoCLMod349TipoOperacDesc = "";
                string result = this.ValidarCLMod349TipoOperacDesc(this.codigoCLMod349TipoOperac, ref codigoCLMod349TipoOperacDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxlCLMod349TipoOperac.Focus();
                }
                else
                {
                    string codigoCLMod349TipoOperacAux = this.codigoCLMod349TipoOperac;
                    if (codigoCLMod349TipoOperacDesc != "") codigoCLMod349TipoOperacAux += " " + separadorDesc + " " + codigoCLMod349TipoOperacDesc;

                    this.radButtonTextBoxlCLMod349TipoOperac.Text = codigoCLMod349TipoOperacAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
                //this.radButtonTextBoxlCLMod349TipoOperac.Focus();
            //}
        }

        private void RadButtonTextBoxCLMod190ClavePercep_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod190ClavePercep.Text.Trim();
            if (codigo != "") // && codigo.Length >= 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 1) this.codigoCLMod190ClavePercep = this.radButtonTextBoxCLMod190ClavePercep.Text;
                else this.codigoCLMod190ClavePercep = this.radButtonTextBoxCLMod190ClavePercep.Text.Substring(0, 1);

                string codigoCLMod190ClavePercepDesc = "";
                string result = this.ValidarCLMod190ClavePercepDesc(this.codigoCLMod190ClavePercep, ref codigoCLMod190ClavePercepDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod190ClavePercep.Focus();
                    this.cmbCLMod190SubclavePercep.Enabled = false;
                }
                else
                {
                    string codigoCLMod190ClavePercepAux = this.codigoCLMod190ClavePercep;
                    if (codigoCLMod190ClavePercepDesc != "") codigoCLMod190ClavePercepAux += " " + separadorDesc + " " + codigoCLMod190ClavePercepDesc;

                    this.radButtonTextBoxCLMod190ClavePercep.Text = codigoCLMod190ClavePercepAux;


                    this.FillcmbCLMod190SubclavePercep();

                    this.cmbCLMod190SubclavePercep.Enabled = true;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                //this.radButtonTextBoxCLMod190ClavePercep.Focus();
                this.cmbCLMod190SubclavePercep.Enabled = false;
            }
        }

        private void RadButtonTextBoxCLMod111ClavePercep_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxCLMod111ClavePercep.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.codigoCLMod111ClavePercep = this.radButtonTextBoxCLMod111ClavePercep.Text;
                else this.codigoCLMod111ClavePercep = this.radButtonTextBoxCLMod111ClavePercep.Text.Substring(0, 2);

                string codigoCLMod111ClavePercepDesc = "";
                string result = this.ValidarCLMod111ClavePercepDesc(this.codigoCLMod111ClavePercep, ref codigoCLMod111ClavePercepDesc);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxCLMod111ClavePercep.Focus();
                }
                else
                {
                    string codigoCLMod111ClavePercepAux = this.codigoCLMod111ClavePercep;
                    if (codigoCLMod111ClavePercepDesc != "") codigoCLMod111ClavePercepAux += " " + separadorDesc + " " + codigoCLMod111ClavePercepDesc;

                    this.radButtonTextBoxCLMod111ClavePercep.Text = codigoCLMod111ClavePercepAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
                //this.radButtonTextBoxCLMod111ClavePercep.Focus();
            //}
        }

        private void RadButtonTextBoxSiiTipoFactura_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoFactura.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_tipoFactura = this.radButtonTextBoxSiiTipoFactura.Text;
                else this.sii_factE_R_tipoFactura = this.radButtonTextBoxSiiTipoFactura.Text.Substring(0, 2);

                string sii_factE_R_tipoFacturaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref sii_factE_R_tipoFacturaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoFactura.Focus();
                }
                else
                {
                    string sii_factE_R_tipoFacturaDescAux = this.sii_factE_R_tipoFactura;
                    if (sii_factE_R_tipoFacturaDesc != "") sii_factE_R_tipoFacturaDescAux += " " + separadorDesc + " " + sii_factE_R_tipoFacturaDesc;

                    this.radButtonTextBoxSiiTipoFactura.Text = sii_factE_R_tipoFacturaDescAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoFactura.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrasc_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrasc.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrasc = this.radButtonTextBoxSiiClaveRegEspTrasc.Text;
                else this.sii_factE_R_claveRegEspeTrasc = this.radButtonTextBoxSiiClaveRegEspTrasc.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "E", ref sii_factE_R_claveRegEspeTrascDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrasc.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAux = this.sii_factE_R_claveRegEspeTrasc;
                    if (sii_factE_R_claveRegEspeTrascDesc != "") sii_factE_R_claveRegEspeTrascAux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascDesc;

                    this.radButtonTextBoxSiiClaveRegEspTrasc.Text = sii_factE_R_claveRegEspeTrascAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrasc.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrascAdicional1_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrascAdicional1 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text;
                else this.sii_factE_R_claveRegEspeTrascAdicional1 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascAdicional1Desc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "E", ref sii_factE_R_claveRegEspeTrascAdicional1Desc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAdicional1Aux = this.sii_factE_R_claveRegEspeTrascAdicional1;
                    if (sii_factE_R_claveRegEspeTrascAdicional1Desc != "") sii_factE_R_claveRegEspeTrascAdicional1Aux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascAdicional1Desc;

                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text = sii_factE_R_claveRegEspeTrascAdicional1Aux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrascAdicional2_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrascAdicional2 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text;
                else this.sii_factE_R_claveRegEspeTrascAdicional2 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascAdicional2Desc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "E", ref sii_factE_R_claveRegEspeTrascAdicional2Desc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAdicional2Aux = this.sii_factE_R_claveRegEspeTrascAdicional2;
                    if (sii_factE_R_claveRegEspeTrascAdicional2Desc != "") sii_factE_R_claveRegEspeTrascAdicional2Aux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascAdicional2Desc;

                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text = sii_factE_R_claveRegEspeTrascAdicional2Aux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Focus();
            //}
        }

        private void RadButtonTextBoxSiiTipoFacturaRectif_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoFacturaRectif.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_tipoFacturaRectif = this.radButtonTextBoxSiiTipoFacturaRectif.Text;
                else this.sii_factE_R_tipoFacturaRectif = this.radButtonTextBoxSiiTipoFacturaRectif.Text.Substring(0, 2);

                string sii_factE_R_tipoFacturaRectifDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref sii_factE_R_tipoFacturaRectifDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
                }
                else
                {
                    string sii_factE_R_tipoFacturaRectifAux = this.sii_factE_R_tipoFacturaRectif;
                    if (sii_factE_R_tipoFacturaRectifDesc != "") sii_factE_R_tipoFacturaRectifAux += " " + separadorDesc + " " + sii_factE_R_tipoFacturaRectifDesc;

                    this.radButtonTextBoxSiiTipoFacturaRectif.Text = sii_factE_R_tipoFacturaRectifAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
            //}
        }

        private void radButtonTextBoxSiiTipoOperSujNoExe_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoOperSujNoExe.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_tipoOperacionSujetaNoExenta = this.radButtonTextBoxSiiTipoOperSujNoExe.Text;
                else this.sii_factE_tipoOperacionSujetaNoExenta = this.radButtonTextBoxSiiTipoOperSujNoExe.Text.Substring(0, 2);

                string sii_factE_tipoOperacionSujetaNoExentaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_tipoOperacionSujetaNoExenta, "H", ref sii_factE_tipoOperacionSujetaNoExentaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoOperSujNoExe.Focus();
                }
                else
                {
                    string sii_factE_tipoOperacionSujetaNoExentaAux = this.sii_factE_tipoOperacionSujetaNoExenta;
                    if (sii_factE_tipoOperacionSujetaNoExentaDesc != "") sii_factE_tipoOperacionSujetaNoExentaAux += " " + separadorDesc + " " + sii_factE_tipoOperacionSujetaNoExentaDesc;

                    this.radButtonTextBoxSiiTipoOperSujNoExe.Text = sii_factE_tipoOperacionSujetaNoExentaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoOperSujNoExe.Focus();
            //}

        }

        private void radButtonTextBoxSiiTipoOperacNoSujeta_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoOperacNoSujeta.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_tipoOperacNoSujeta = this.radButtonTextBoxSiiTipoOperacNoSujeta.Text;
                else this.sii_factE_tipoOperacNoSujeta = this.radButtonTextBoxSiiTipoOperacNoSujeta.Text.Substring(0, 2);

                string sii_factE_tipoOperacNoSujetaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_tipoOperacNoSujeta, "K", ref sii_factE_tipoOperacNoSujetaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoOperacNoSujeta.Focus();
                }
                else
                {
                    string sii_factE_tipoOperacNoSujetaAux = this.sii_factE_tipoOperacNoSujeta;
                    if (sii_factE_tipoOperacNoSujetaDesc != "") sii_factE_tipoOperacNoSujetaAux += " " + separadorDesc + " " + sii_factE_tipoOperacNoSujetaDesc;

                    this.radButtonTextBoxSiiTipoOperacNoSujeta.Text = sii_factE_tipoOperacNoSujetaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoOperacNoSujeta.Focus();
            //}
        }

        private void radButtonTextBoxSiiCausaExeOperSujetaYExenta_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_causaExencionOpSujetasExentas = this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text;
                else this.sii_factE_causaExencionOpSujetasExentas = this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text.Substring(0, 2);

                string sii_factE_causaExencionOpSujetasExentasDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_causaExencionOpSujetasExentas, "B", ref sii_factE_causaExencionOpSujetasExentasDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Focus();
                }
                else
                {
                    string sii_factE_causaExencionOpSujetasExentasAux = this.sii_factE_causaExencionOpSujetasExentas;
                    if (sii_factE_causaExencionOpSujetasExentasDesc != "") sii_factE_causaExencionOpSujetasExentasAux += " " + separadorDesc + " " + sii_factE_causaExencionOpSujetasExentasDesc;

                    this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text = sii_factE_causaExencionOpSujetasExentasAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Focus();
            //}
        }

        private void RadButtonTextBoxSiiAgenciaTributaria_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiAgenciaTributaria.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributaria.Text;
                else this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributaria.Text.Substring(0, 2);

                string sii_agenciaTributariaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref sii_agenciaTributariaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiAgenciaTributaria.Focus();
                }
                else
                {
                    string sii_agenciaTributariaAux = this.sii_agenciaTributaria;
                    if (sii_agenciaTributariaDesc != "") sii_agenciaTributariaAux += " " + separadorDesc + " " + sii_agenciaTributariaDesc;

                    this.radButtonTextBoxSiiAgenciaTributaria.Text = sii_agenciaTributariaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiAgenciaTributaria.Focus();
            //}
        }

        private void RadButtonTextBoxlSiiTipoFacturaRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxlSiiTipoFacturaRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_tipoFactura = this.radButtonTextBoxlSiiTipoFacturaRecibida.Text;
                else this.sii_factE_R_tipoFactura = this.radButtonTextBoxlSiiTipoFacturaRecibida.Text.Substring(0, 2);

                string sii_factE_R_tipoFacturaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref sii_factE_R_tipoFacturaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxlSiiTipoFacturaRecibida.Focus();
                }
                else
                {
                    string sii_factE_R_tipoFacturaAux = this.sii_factE_R_tipoFactura;
                    if (sii_factE_R_tipoFacturaDesc != "") sii_factE_R_tipoFacturaAux += " " + separadorDesc + " " + sii_factE_R_tipoFacturaDesc;

                    this.radButtonTextBoxlSiiTipoFacturaRecibida.Text = sii_factE_R_tipoFacturaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrascFactRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrasc = this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text;
                else this.sii_factE_R_claveRegEspeTrasc = this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "F", ref sii_factE_R_claveRegEspeTrascDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAux = this.sii_factE_R_claveRegEspeTrasc;
                    if (sii_factE_R_claveRegEspeTrascDesc != "") sii_factE_R_claveRegEspeTrascAux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascDesc;

                    this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text = sii_factE_R_claveRegEspeTrascAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrascAdicional1 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text;
                else this.sii_factE_R_claveRegEspeTrascAdicional1 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascAdicional1Desc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "F", ref sii_factE_R_claveRegEspeTrascAdicional1Desc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAdicional1Aux = this.sii_factE_R_claveRegEspeTrascAdicional1;
                    if (sii_factE_R_claveRegEspeTrascAdicional1Desc != "") sii_factE_R_claveRegEspeTrascAdicional1Aux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascAdicional1Desc;

                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text = sii_factE_R_claveRegEspeTrascAdicional1Aux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Focus();
            //}
        }

        private void RadButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_claveRegEspeTrascAdicional2 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text;
                else this.sii_factE_R_claveRegEspeTrascAdicional2 = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text.Substring(0, 2);

                string sii_factE_R_claveRegEspeTrascAdicional2Desc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "F", ref sii_factE_R_claveRegEspeTrascAdicional2Desc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Focus();
                }
                else
                {
                    string sii_factE_R_claveRegEspeTrascAdicional2Aux = this.sii_factE_R_claveRegEspeTrascAdicional2;
                    if (sii_factE_R_claveRegEspeTrascAdicional2Desc != "") sii_factE_R_claveRegEspeTrascAdicional2Aux += " " + separadorDesc + " " + sii_factE_R_claveRegEspeTrascAdicional2Desc;

                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text = sii_factE_R_claveRegEspeTrascAdicional2Aux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Focus();
            //}
        }

        private void RadButtonTextBoxSiiTipoFactRectifFactRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_factE_R_tipoFacturaRectif = this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text;
                else this.sii_factE_R_tipoFacturaRectif = this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text.Substring(0, 2);

                string sii_factE_R_tipoFacturaRectifDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref sii_factE_R_tipoFacturaRectifDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Focus();
                }
                else
                {
                    string sii_factE_R_tipoFacturaRectifAux = this.sii_factE_R_tipoFacturaRectif;
                    if (sii_factE_R_tipoFacturaRectifDesc != "") sii_factE_R_tipoFacturaRectifAux += " " + separadorDesc + " " + sii_factE_R_tipoFacturaRectifDesc;

                    this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text = sii_factE_R_tipoFacturaRectifAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Focus();
            //}
        }

        private void RadButtonTextBoxSiiAgenciaTributariaFactRecibida_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text;
                else this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text.Substring(0, 2);

                string sii_agenciaTributariaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref sii_agenciaTributariaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    bTabulador = false;
                    RadMessageBox.Show(result, error);
                    this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Focus();
                }
                else
                {
                    string sii_agenciaTributariaAux = this.sii_agenciaTributaria;
                    if (sii_agenciaTributariaDesc != "") sii_agenciaTributariaAux += " " + separadorDesc + " " + sii_agenciaTributariaDesc;

                    this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text = sii_agenciaTributariaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Focus();
            //}
        }

        private void RadButtonTextBoxSiiTipoOpSeguro_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoOpSeguro.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_opSeg_tipoOperacion = this.radButtonTextBoxSiiTipoOpSeguro.Text;
                else this.sii_opSeg_tipoOperacion = this.radButtonTextBoxSiiTipoOpSeguro.Text.Substring(0, 2);

                string sii_opSeg_tipoOperacionDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_opSeg_tipoOperacion, "X", ref sii_opSeg_tipoOperacionDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoOpSeguro.Focus();
                }
                else
                {
                    string sii_opSeg_tipoOperacionAux = this.sii_opSeg_tipoOperacion;
                    if (sii_opSeg_tipoOperacionDesc != "") sii_opSeg_tipoOperacionAux += " " + separadorDesc + " " + sii_opSeg_tipoOperacionDesc;

                    this.radButtonTextBoxSiiTipoOpSeguro.Text = sii_opSeg_tipoOperacionAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoOpSeguro.Focus();
            //}
        }

        private void RadButtonTextBoxSiiTipoOpIntracom_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiTipoOpIntracom.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_opIntracom_tipoOperacion = this.radButtonTextBoxSiiTipoOpIntracom.Text;
                else this.sii_opIntracom_tipoOperacion = this.radButtonTextBoxSiiTipoOpIntracom.Text.Substring(0, 2);

                string sii_opIntracom_tipoOperacionDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_opIntracom_tipoOperacion, "D", ref sii_opIntracom_tipoOperacionDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiTipoOpIntracom.Focus();
                }
                else
                {
                    string sii_opIntracom_tipoOperacionAux = this.sii_opIntracom_tipoOperacion;
                    if (sii_opIntracom_tipoOperacionDesc != "") sii_opIntracom_tipoOperacionAux += " " + separadorDesc + " " + sii_opIntracom_tipoOperacionDesc;

                    this.radButtonTextBoxSiiTipoOpIntracom.Text = sii_opIntracom_tipoOperacionAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiTipoOpIntracom.Focus();
            //}
        }

        private void RadButtonTextBoxSiiAgenciaTributariaOpSeguro_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text;
                else this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text.Substring(0, 2);

                string sii_agenciaTributariaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref sii_agenciaTributariaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Focus();
                }
                else
                {
                    string sii_agenciaTributariaAux = this.sii_agenciaTributaria;
                    if (sii_agenciaTributariaDesc != "") sii_agenciaTributariaAux += " " + separadorDesc + " " + sii_agenciaTributariaDesc;

                    this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text = sii_agenciaTributariaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Focus();
            //}
        }

        private void RadButtonTextBoxSiiAgenciaTributariaOpIntracom_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            string codigo = this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text.Trim();
            if (codigo != "") // && codigo.Length >= 2)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (codigo.Length <= 2) this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text;
                else this.sii_agenciaTributaria = this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text.Substring(0, 2);

                string sii_agenciaTributariaDesc = "";
                string result = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref sii_agenciaTributariaDesc);
                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    bTabulador = false;
                    this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Focus();
                }
                else
                {
                    string sii_agenciaTributariaAux = this.sii_agenciaTributaria;
                    if (sii_agenciaTributariaDesc != "") sii_agenciaTributariaAux += " " + separadorDesc + " " + sii_agenciaTributariaDesc;

                    this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text = sii_agenciaTributariaAux;
                }
                bTabulador = false;
                Cursor.Current = Cursors.Default;
            }
            //else
            //{
            //    this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Focus();
            //}
        }

        private void RadButtonEliminarLibro_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string mensaje = this.LP.GetText("wrnSuprimirCodIVASiiLibro", "Se va a eliminar el libro del SII para el código de IVA") + " " + this.codigo.Trim();
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");

            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string tdoca3 = this.LibroSIIActivoCodigo();

                    string query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTA3 ";
                    query += "where TIPLA3 = '" + this.codigoPlan + "' and COIVA3 = '" + this.codigo + "' and ";
                    query += "TDOCA3 = '" + tdoca3 + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Limpiar todos los campos
                    this.radButtonTextBoxSiiTipoOpSeguro.Text = "";
                    this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text = "";

                    this.radButtonTextBoxSiiTipoOpIntracom.Text = "";
                    this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text = "";

                    this.radButtonTextBoxSiiTipoFactura.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrasc.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text = "";
                    this.radButtonTextBoxSiiTipoFacturaRectif.Text = "";
                    this.radDropDownListSiiTipoDesglose.SelectedIndex = 0;
                    this.radButtonTextBoxSiiTipoOperSujNoExe.Text = "";
                    this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text = "";
                    this.radButtonTextBoxSiiTipoOperacNoSujeta.Text = "";
                    this.radButtonTextBoxSiiAgenciaTributaria.Text = "";
                    this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text = "";
                    this.radToggleSwitchSiiCobroMetalico.Value = false;
                    this.radToggleSwitchSiiCupon.Value = false;
                    this.radToggleSwitchSiiTaxFree.Value = false;
                    this.radToggleSwitchSiiViajeros.Value = false;
                    this.radToggleSwitchSiiFactSimpArt72_73.Value = false;

                    this.radButtonTextBoxlSiiTipoFacturaRecibida.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text = "";
                    this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text = "";
                    //this.radDropDownListSiiTipoDesglose.SelectedIndex = 0; jl
                    this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex = 0;

                    this.radToggleSwitchSiiBienesInversionFactRecibida.Value = false;
                    this.radToggleSwitchSiiProrrataFactRecibida.Value = false;
                    this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value = false;
                    this.txtSiiTipoImpositivoProrrataFactRecibida.Text = "0,00";  
                    this.radButtonTextBoxSiiAgenciaTributaria.Text = "";
                    this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value = false;

                    this.radButtonTextBoxSiiCampoLibreExp.Text = "";
                    this.radButtonTextBoxSiiCampoLibreRec.Text = "";


                    //Activar sin libros
                    this.cmbSiiLibros.SelectedIndex = 0;
                }
                catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            }

            Cursor.Current = Cursors.Default;

            //Pedir confirmacion
            //Eliminar fisicamente de la tabla

            this.cmbSiiLibros.SelectedIndex = 0;
            this.cmbSiiLibros.ReadOnly = false;
            this.cmbSiiLibros.Enabled = true;
            this.radButtonEliminarLibro.Visible = false;
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            bCancelar = false;

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadPageViewDatos_SelectedPageChanged(object sender, EventArgs e)
        {
            //Activar el primer control de la pestaña seleccionada
            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageGeneral.Name])
            {
                this.txtDescripcion.Select();
                return;
            }
            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageClavesLegales.Name])
            {
                this.radButtonTextBoxCLMod303GrupoOperac.Select();
                return;
            }
        }

        private void RadToggleSwitchCuadreCuotas_ValueChanged(object sender, EventArgs e)
        {
            if (this.radToggleSwitchCuadreCuotas.Value)
            {
                this.txtMaxDescuadreCuotas.Enabled = true;
                this.txtMaxDescuadreCuotas.Text = "0,00";
            }
            else
            {
                this.txtMaxDescuadreCuotas.Enabled = false;
                this.txtMaxDescuadreCuotas.Text = "";
            }
        }

        private void RadButtonElementTipoAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar tipo de auxiliar",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxTipoAux.Text = result;
                this.ActiveControl = this.radButtonTextBoxTipoAux;
                this.radButtonTextBoxTipoAux.Select(0, 0);
                this.radButtonTextBoxTipoAux.Focus();
            }
        }

        private void RadButtonElementCtaMayor_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select min(CUENMC) CUENMC , max(NOLAAD) NOLAAD, min(CEDTMC) CEDTMC from ";
            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
            query += "where TIPLMC = '" + this.codigoPlan + "' and STATMC = 'V' and TCUEMC = 'D' and RNITMC = 'R' ";
            query += "group by CEDTMC ";
            query += "order by CEDTMC";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasCtaMayor = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción"),
                this.LP.GetText("lblListaCampoCtaMayor", "Cuenta Mayor")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar cuenta de mayor",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasCtaMayor,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCtaMayor.Text = result;
                this.ActiveControl = this.radButtonTextBoxCtaMayor;
                this.radButtonTextBoxCtaMayor.Select(0, 0);
                this.radButtonTextBoxCtaMayor.Focus();
            }
        }

        private void RadButtonElementCtaMayorContrap_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select min(CUENMC) CUENMC , max(NOLAAD) NOLAAD, min(CEDTMC) CEDTMC from ";
            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
            query += "where TIPLMC = '" + this.codigoPlan + "' and STATMC = 'V' and TCUEMC = 'D' and RNITMC <> 'R' ";
            query += "group by CEDTMC ";
            query += "order by CEDTMC";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasCtaMayor = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción"),
                this.LP.GetText("lblListaCampoCtaMayor", "Cuenta Mayor")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar cuenta de mayor",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasCtaMayor,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCtaMayorContrap.Text = result;
                this.ActiveControl = this.radButtonTextBoxCtaMayorContrap;
                this.radButtonTextBoxCtaMayorContrap.Select(0, 0);
                this.radButtonTextBoxCtaMayorContrap.Focus();
            }
        }

        private void RadButtonElementCodIVAContrap_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select COIVCI, NOMBCI from ";
            query += GlobalVar.PrefijoTablaCG + "IVT01 ";
            // query += "where TIPLCI = '" + this.codigoPlan + "' and STATCI = 'V' and RESOCI <> '" + this.codigo + "' "; jl
            query += "where TIPLCI = '" + this.codigoPlan + "' and STATCI = 'V' and COIVCI <> '" + this.codigo + "' ";  //jl
            query += " and RESOCI <> '" + (this.rbRepercutido.IsChecked ? "R" : "S") + "' and COIACI = '' ";
            query += "order by COIVCI";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCodIVAContrap.Text = result;
                this.ActiveControl = this.radButtonTextBoxCodIVAContrap;
                this.radButtonTextBoxCodIVAContrap.Select(0, 0);
                this.radButtonTextBoxCodIVAContrap.Focus();
            }
        }

        private void RadButtonTextBoxCtaMayorContrap_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCodIVAContrap_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonElementCodIVAAsociado_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select COIVCI, NOMBCI from ";
            query += GlobalVar.PrefijoTablaCG + "IVT01 ";
            //query += "where TIPLCI = '" + this.codigoPlan + "' and STATCI = 'V' and RESOCI <> '" + this.codigo + "' and ";
            query += "where TIPLCI = '" + this.codigoPlan + "' and STATCI = 'V' and ";
            //query += "STATCI = 'V' and RESOCI <> '" + (this.rbRepercutido.IsChecked ? "R" : "S") + "' ";
            query += " RESOCI <> '" + (this.rbRepercutido.IsChecked ? "R" : "S") + "' and COIACI = '' ";
            query += "order by COIVCI";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCodIVAAsociado.Text = result;
                this.ActiveControl = this.radButtonTextBoxCodIVAAsociado;
                this.radButtonTextBoxCodIVAAsociado.Select(0, 0);
                this.radButtonTextBoxCodIVAAsociado.Focus();
            }
        }

        private void RadButtonTextBoxCodIVAAsociado_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonElementCLMod303GrupoOperac_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='M' and CODCD <> '00' and ";
            query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";
            query += "order by DESCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod303GrupoOperac.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod303GrupoOperac;
                this.radButtonTextBoxCLMod303GrupoOperac.Select(0, 0);
                this.radButtonTextBoxCLMod303GrupoOperac.Focus();
            }
        }

        private void RadButtonElementCLMod390GrupoOperac_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='A' and CODCD <> '00' and ";
            query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod390GrupoOperac.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod390GrupoOperac;
                this.radButtonTextBoxCLMod390GrupoOperac.Select(0, 0);
                this.radButtonTextBoxCLMod390GrupoOperac.Focus();
            }
        }

        private void RadButtonElementCLMod340LibroRegistro_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='L' and CODCD <> '00' ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod340LibroRegistro.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod340LibroRegistro;
                this.radButtonTextBoxCLMod340LibroRegistro.Select(0, 0);
                this.radButtonTextBoxCLMod340LibroRegistro.Focus();
            }
        }

        private void RadButtonElementCLMod111ClavePercep_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='R' and CODCD <> '00' ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod111ClavePercep.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod111ClavePercep;
                this.radButtonTextBoxCLMod111ClavePercep.Select(0, 0);
                this.radButtonTextBoxCLMod111ClavePercep.Focus();
            }
        }

        private void RadButtonElementCLMod340ClaveOperac_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='O' and CODCD <> '00' ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod340ClaveOperac.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod340ClaveOperac;
                this.radButtonTextBoxCLMod340ClaveOperac.Select(0, 0);
                this.radButtonTextBoxCLMod340ClaveOperac.Focus();
            }
        }

        private void RadButtonElementlCLMod349TipoOperac_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='T' and CODCD <> '00' ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxlCLMod349TipoOperac.Text = result;
                this.ActiveControl = this.radButtonTextBoxlCLMod349TipoOperac;
                this.radButtonTextBoxlCLMod349TipoOperac.Select(0, 0);
                this.radButtonTextBoxlCLMod349TipoOperac.Focus();
            }
        }

        private void RadButtonElementCLMod190ClavePercep_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='P' and CODCD <> '00' ";
            query += "order by CODCD";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar código de IVA",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                this.radButtonTextBoxCLMod190ClavePercep.Text = result;
                this.ActiveControl = this.radButtonTextBoxCLMod190ClavePercep;
                this.radButtonTextBoxCLMod190ClavePercep.Select(0, 0);
                this.radButtonTextBoxCLMod190ClavePercep.Focus();
            }
        }

        private void RadButtonTextBoxCLMod303GrupoOperac_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCLMod390GrupoOperac_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCLMod340LibroRegistro_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCLMod340ClaveOperac_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxlCLMod349TipoOperac_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCLMod190ClavePercep_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadButtonTextBoxCLMod111ClavePercep_TextChanged(object sender, EventArgs e)
        {

        }

        private void RadGridViewTiposIVA_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells["Eliminar"].Value = global::ModMantenimientos.Properties.Resources.Eliminar;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewTiposIVA_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (sender is Telerik.WinControls.UI.GridImageCellElement imageCellElement)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (this.radGridViewTiposIVA.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex < this.radGridViewTiposIVA.Rows.Count)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        string fechaEfectivaOrigen = "";
                        string porcIVAOrigen = "";
                        string porcRecEquivOrigen = "";
                        string descripcionOrigen = "";

                        if (this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["FechaEfectivaOrigen"].Value != null)
                            fechaEfectivaOrigen = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["FechaEfectivaOrigen"].Value.ToString();

                        if (this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcIVAOrigen"].Value != null)
                            porcIVAOrigen = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcIVAOrigen"].Value.ToString();

                        if (this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcRecEquivOrigen"].Value != null)
                            porcRecEquivOrigen = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcRecEquivOrigen"].Value.ToString();

                        if (this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["DescripcionOrigen"].Value != null)
                            descripcionOrigen = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["DescripcionOrigen"].Value.ToString();

                        if (fechaEfectivaOrigen == "" && porcIVAOrigen == "" && porcRecEquivOrigen == "" && descripcionOrigen == "")
                        {
                            //Eliminarla del DataTable
                            this.radGridViewTiposIVA.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            //Actualiza el valor de PermiteModificar de la fila  1-> si es posible   0 -> en otro caso
                            this.ActualizarRowPermiteModificar(e.RowIndex);

                            string error = this.LP.GetText("errValTitulo", "Error");
                            if (this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PermiteModificar"].Value.ToString() == "0")
                            {
                                RadMessageBox.Show("Este tipo de IVA tiene movimientos asociados", error);     //Falta traducir
                                this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["Eliminar"].IsSelected = false;
                                return;
                            }

                            //Verificar que existan más filas de Tipos de IVA (es obligatorio al menos una)
                            int contRowsTiposIVAEnTabla = 0;
                            for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                            {
                                if (this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value.ToString() != "") contRowsTiposIVAEnTabla++;
                            }

                            if (contRowsTiposIVAEnTabla == 1)
                            {
                                RadMessageBox.Show("No es posible eliminar el tipo de IVA, sólo modificar", error);     //Falta traducir
                                this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["Eliminar"].IsSelected = false;

                                return;
                            }

                            string fechaEfectivaActual = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["FechaEfectiva"].Value.ToString();
                            string porcIVA = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcIVA"].Value.ToString();
                            string porcRecEquiv = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["PorcRecEquiv"].Value.ToString();
                            string descripcion = this.radGridViewTiposIVA.Rows[e.RowIndex].Cells["Descripcion"].Value.ToString();   //jl

                            DateTime dt = Convert.ToDateTime(fechaEfectivaOrigen);
                            int fechaEfectivaCG = utiles.FechaToFormatoCG(dt, true);

                            //Pedir confirmación y eliminar el cambio seleccionado
                            //this.LP.GetText("wrnDeleteConfirm"       
                            ///SMRstring mensaje = "Se va a eliminar el registro con valores en la tabla: fecha efectiva: " + fechaEfectivaActual + " porcentaje IVA: " + porcIVA;  //Falta traducir
                            ///mensaje += " y porcentaje de recargo de equivalencia : " + porcRecEquiv + " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");      //Falta traducir
                            ///
                            ///DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                            ///if (result == DialogResult.Yes)
                            ///{
                            ///    porcIVAOrigen = porcIVAOrigen.Replace(',', '.');
                            ///
                            ///    if (porcRecEquivOrigen == "") porcRecEquivOrigen = "0";
                            ///    else porcRecEquivOrigen = porcRecEquivOrigen.Replace(',', '.');
                            ///
                            ///    string query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTX1 where";
                            ///    query += " TIPLCX = '" + this.codigoPlan + "' and COIVCX = '" + this.codigo + "' and FEIVCX = " + fechaEfectivaCG.ToString();
                            ///    query += " and TPIVCX = " + porcIVAOrigen + " and RECGCX = " + porcRecEquivOrigen;
                            ///    query += " and NOMBCX = '" + descripcionOrigen + "'";
                            ///
                            ///    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            ///
                            ///    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "IVT01", this.codigoPlan, this.codigo);
                            ///
                            ///    //Eliminar la entrada del DataSet
                            ///    this.radGridViewTiposIVA.Rows.RemoveAt(e.RowIndex);
                            ///SMR}

                            //Eliminar la entrada del DataSet SMR
                            this.radGridViewTiposIVA.Rows.RemoveAt(e.RowIndex);

                            this.radGridViewTiposIVA.Refresh();
                            this.radGridViewTiposIVA.ClearSelection();
                        }

                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void RadGridViewTiposIVA_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            var rowInfo = radGridViewTiposIVA.CurrentCell.RowInfo;

            if (rowInfo.Index != -1)
            {
                int rowCurrent = rowInfo.Index;

                if (this.radGridViewTiposIVA.Rows[rowCurrent].Cells["PermiteModificar"].Value != null &&
                    this.radGridViewTiposIVA.Rows[rowCurrent].Cells["PermiteModificar"].Value.ToString() == "0")
                {
                    e.Cancel = true;
                }
            }
        }

        private void RadGridViewTiposIVA_CurrentRowChanging(object sender, CurrentRowChangingEventArgs e)
        {
            if (!this.cargarInfoDeTiposIVA && !this.nuevoRegistro)
            {
                if (this.radGridViewTiposIVA.RowCount != 0)
                {
                    string result = "";
                    string error = this.LP.GetText("errValTitulo", "Error");

                    var rowActual = e.CurrentRow;
                    if (rowActual != null)
                    {
                        if (rowActual.Cells["FechaEfectiva"].Value != null)
                        {
                            string fechaEfectiva = rowActual.Cells["FechaEfectiva"].Value.ToString();
                            if (fechaEfectiva != "")
                            {
                                result = ValidarFecha(fechaEfectiva);
                                if (result != "")
                                {
                                    RadMessageBox.Show(result, error);
                                    //e.Cancel = true;
                                }
                            }
                        }

                        if (rowActual.Cells["PorcIVA"].Value != null)
                        {
                            string porcIVA = rowActual.Cells["PorcIVA"].Value.ToString();
                            if (porcIVA != "")
                            {
                                string valorFormat = "";
                                result = ValidarPorcentaje(porcIVA, ref valorFormat);
                                if (result != "")
                                {
                                    RadMessageBox.Show(result, error);
                                    //e.Cancel = true;

                                    //this.dgDetalle.Rows[e.RowIndex].ErrorText = String.Empty;
                                }
                            }
                        }

                        if (rowActual.Cells["PorcRecEquiv"].Value != null)
                        {
                            string porcRecEquiv = rowActual.Cells["PorcRecEquiv"].Value.ToString();
                            if (porcRecEquiv != "")
                            {
                                string valorFormat = "";
                                result = ValidarPorcentaje(porcRecEquiv, ref valorFormat);
                                if (result != "")
                                {
                                    RadMessageBox.Show(result, error);
                                    //e.Cancel = true;

                                    //this.dgDetalle.Rows[e.RowIndex].ErrorText = String.Empty;
                                }
                            }
                        }
                    }
                }
            }

            if (this.nuevoRegistro) this.nuevoRegistro = false;
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void MostrarValores(string titulo, string tipcd, string resod, ref Telerik.WinControls.UI.RadButtonTextBox control)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = this.ObtenerQuery(tipcd, resod);

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                //frmElementosSel.TituloForm = "Seleccionar código de IVA";
                TituloForm = titulo,
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 2;
            string separadorCampos = "-";
            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (cantidadColumnasResult > i + 1 && cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + separadorCampos + " ";
                    }
                }
                control.Text = result;
                this.ActiveControl = control;
                control.Select(0, 0);
                control.Focus();
            }
        }

        private void CmbSiiLibros_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            switch (this.cmbSiiLibros.SelectedIndex)
            {
                case 1:
                    //SMR this.radPanelSiiFactExpedidas.Size = new Size(872, 442);
                    this.radPanelSiiFactExpedidas.Visible = true;
                    this.radPanelSiiFactRecibidas.Visible = false;
                    this.radPanelSiiOperacionesSeguros.Visible = false;
                    this.radPanelSiiOperacionesIntracomunitarias.Visible = false;
                    radButtonTextBoxSiiTipoFactura.Focus();
                    break;
                case 2:
                    this.radPanelSiiFactExpedidas.Visible = false;
                    this.radPanelSiiFactRecibidas.Visible = true;
                    this.radPanelSiiOperacionesSeguros.Visible = false;
                    this.radPanelSiiOperacionesIntracomunitarias.Visible = false;

                    this.radPanelSiiFactRecibidas.Location = new Point(this.radPanelSiiFactExpedidas.Location.X, this.radPanelSiiFactExpedidas.Location.Y);
                    radButtonTextBoxlSiiTipoFacturaRecibida.Focus();
                    break;
                case 3:
                    this.radPanelSiiFactExpedidas.Visible = false;
                    this.radPanelSiiFactRecibidas.Visible = false;
                    this.radPanelSiiOperacionesSeguros.Visible = true;
                    this.radPanelSiiOperacionesIntracomunitarias.Visible = false;

                    this.radPanelSiiOperacionesSeguros.Location = new Point(this.radPanelSiiFactExpedidas.Location.X, this.radPanelSiiFactExpedidas.Location.Y);
                    radButtonTextBoxSiiTipoOpSeguro.Focus();
                    break;
                case 4:
                    this.radPanelSiiFactExpedidas.Visible = false;
                    this.radPanelSiiFactRecibidas.Visible = false;
                    this.radPanelSiiOperacionesSeguros.Visible = false;
                    this.radPanelSiiOperacionesIntracomunitarias.Visible = true;

                    this.radPanelSiiOperacionesIntracomunitarias.Location = new Point(this.radPanelSiiFactExpedidas.Location.X, this.radPanelSiiFactExpedidas.Location.Y);
                    radButtonTextBoxSiiTipoOpIntracom.Focus();
                    break;
                default:
                    this.radPanelSiiFactExpedidas.Visible = false;
                    this.radPanelSiiFactRecibidas.Visible = false;
                    this.radPanelSiiOperacionesSeguros.Visible = false;
                    this.radPanelSiiOperacionesIntracomunitarias.Visible = false;
                    break;
            }
        }

        #region SII - Libro Facturas Expedidas
        private void RadButtonElementSiiTipoFactura_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de factura", "U", " ", ref this.radButtonTextBoxSiiTipoFactura);
        }

        private void RadButtonElementSiiClaveRegEspTrasc_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrasc);
        }

        private void RadButtonElementSiiClaveRegEspTrascAdicional1_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia adicional 1", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrascAdicional1);
        }

        private void RadButtonElementSiiClaveRegEspTrascAdicional2_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia adicional 2", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrascAdicional2);
        }

        private void RadButtonElementSiiTipoFacturaRectif_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de factura rectificativa", "G", null, ref this.radButtonTextBoxSiiTipoFacturaRectif);
        }

        private void RadButtonElementSiiTipoOperSujNoExe_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de operación Sujeta/No exenta", "H", null, ref this.radButtonTextBoxSiiTipoOperSujNoExe);
        }

        private void RadButtonElementSiiCausaExeOperSujetaYExenta_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar causa exención operaciones Sujetas y Exentas", "B", null, ref this.radButtonTextBoxSiiCausaExeOperSujetaYExenta);
        }

        private void RadButtonElementSiiTipoOperacNoSujeta_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de operación No sujeta", "K", null, ref this.radButtonTextBoxSiiTipoOperacNoSujeta);
        }

        private void RadButtonElementSiiAgenciaTributaria_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar Agencia Tributaria", "6", null, ref this.radButtonTextBoxSiiAgenciaTributaria);
        }
        #endregion

        #region SII - Libro Facturas Recibidas
        private void RadButtonElementlSiiTipoFacturaRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de factura", "U", " ", ref this.radButtonTextBoxlSiiTipoFacturaRecibida);
        }

        private void RadButtonElementSiiClaveRegEspTrascFactRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida);
        }

        private void RadButtonElementSiiClaveRegEspTrascAdicional1FactRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia adicional 1", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida);
        }

        private void RadButtonElementSiiClaveRegEspTrascAdicional2FactRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar clave de régimen especial o trascendencia adicional 2", "E", null, ref this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida);
        }

        private void RadButtonElementSiiTipoFactRectifFactRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de factura rectificativa", "G", null, ref this.radButtonTextBoxSiiTipoFactRectifFactRecibida);
        }

        private void RadButtonElementSiiAgenciaTributariaFactRecibida_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar Agencia Tributaria", "6", null, ref this.radButtonTextBoxSiiAgenciaTributariaFactRecibida);
        }
        #endregion

        #region SII - Libro Operaciones de Seguros
        private void RadButtonElementSiiTipoOpSeguro_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de operación de seguro", "X", null, ref this.radButtonTextBoxSiiTipoOpSeguro);
        }

        private void RadButtonElementSiiAgenciaTributariaOpSeguro_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar Agencia Tributaria", "6", null, ref this.radButtonTextBoxSiiAgenciaTributariaOpSeguro);
        }
        #endregion

        #region SII - Libro Operaciones Intracomunitarias
        private void RadButtonElementSiiTipoOpIntracom_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar tipo de operación intracomunitaria", "D", null, ref this.radButtonTextBoxSiiTipoOpIntracom);
        }

        private void RadButtonElementSiiAgenciaTributariaOpIntracom_Click(object sender, EventArgs e)
        {
            this.MostrarValores("Seleccionar Agencia Tributaria", "6", null, ref this.radButtonTextBoxSiiAgenciaTributariaOpIntracom);
        }
        #endregion

        private void FrmMtoIVT01_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoIVT01_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void FrmMtoIVT01_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtDescripcion.Text.Trim() != this.txtDescripcion.Tag.ToString().Trim() ||
                    ((this.rbRepercutido.IsChecked && this.gbRepercSopor.Tag.ToString() == "S") ||
                    (this.rbSoportado.IsChecked && this.gbRepercSopor.Tag.ToString() == "R")) ||
                    ((this.rbDeducible.IsChecked && this.gbDeducibleNoDed.Tag.ToString() == "N") ||
                    (this.rbNoDeducible.IsChecked && this.gbDeducibleNoDed.Tag.ToString() == "D")) ||
                    this.radButtonTextBoxCtaMayor.Text != this.radButtonTextBoxCtaMayor.Tag.ToString() ||
                    this.radButtonTextBoxTipoAux.Text != this.radButtonTextBoxTipoAux.Tag.ToString() ||
                    this.txtLibro.Text.Trim() != this.txtLibro.Tag.ToString().Trim() ||
                    this.txtSerie.Text.Trim() != this.txtSerie.Tag.ToString().Trim() ||
                    //SMR this.radToggleSwitchCuadreCuotas.Value != (bool)(this.radToggleSwitchCuadreCuotas.Tag) ||
                    this.txtMaxDescuadreCuotas.Text.Trim() != this.txtMaxDescuadreCuotas.Tag.ToString().Trim() ||
                    this.radButtonTextBoxCtaMayorContrap.Text != this.radButtonTextBoxCtaMayorContrap.Tag.ToString() ||
                    this.radButtonTextBoxCodIVAContrap.Text != this.radButtonTextBoxCodIVAContrap.Tag.ToString() ||
                    this.radButtonTextBoxCodIVAAsociado.Text != this.radButtonTextBoxCodIVAAsociado.Tag.ToString() ||
                    //SMR this.TiposIVACambio() ||
                    this.ClavesLegalesCambio() ||
                    this.SiiLibroCambio()
                    )
                {
                    if (bEliminar == true || bGrabar == true) return;  //jl
                    //if (bCancelar == false) //jl
                    //{                       //jl
                        string mensaje = this.LP.GetText("ConfirmarCambio", "Se han detectado cambios en el formulario, ¿desea salir?");

                        DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            // this.radButtonSave.PerformClick();
                            e.Cancel = false;
                            cerrarForm = false;
                    }
                        else if (result == DialogResult.No)
                        {
                            e.Cancel = true;
                            
                        }
                        // else e.Cancel = false;
                    //}
                    //else
                    //{
                        //e.Cancel = false;                        
                    //}
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Códigos de IVA Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        /// <param name="traducirComboHijos">Si se traducen o no los literales del Combo de Hijos</param>
        private void TraducirLiterales(bool traducirComboHijos)
        {
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoIVT01TituloAlta", "Mantenimiento de Códigos de IVA - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoIVT01TituloEdit", "Mantenimiento de Códigos de IVA - Edición");           //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar"); 
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblPlan.Text = this.LP.GetText("lblIVT01Plan", "Plan de cuentas");
            this.lblCodigo.Text = this.LP.GetText("lblIVT01CodIVA", "Código de IVA");

            //------ Apartado General -----
            this.radPageViewPageGeneral.Text = this.LP.GetText("lblIVT01General", "General");
            this.lblDescripcion.Text = this.LP.GetText("lblIVT01Desc", "Descripcion");
            this.lblReperSopor.Text = this.LP.GetText("lblIVT01ReperSopor", "Repercutido/soportado");
            this.rbRepercutido.Text = this.LP.GetText("lblIVT01Repercutido", "repercutido");
            this.rbSoportado.Text = this.LP.GetText("lblIVT01Soportado", "soportado");
            this.lblDeducibleNoDeduc.Text = this.LP.GetText("lblIVT01DeducNoDeduc", "Deducible/no deducible");
            this.rbDeducible.Text = this.LP.GetText("lblIVT01Deduc", "deducible");
            this.rbNoDeducible.Text = this.LP.GetText("lblIVT01NoDeduc", "no deducible");
            this.lblCtaMayor.Text = this.LP.GetText("lblIVT01CtaMayor", "Cuenta de Mayor");
            this.lblTipoAux.Text = this.LP.GetText("lblIVT01TipoAux", "Tipo de Auxiliar");
            this.lblLibro.Text = this.LP.GetText("lblIVT01Libro", "Libro");
            this.lblSerie.Text = this.LP.GetText("lblIVT01Serie", "Serie");
            this.lblCuadreCuotas.Text = "Cuadre de cuotas";
            this.lblMaxDescuadreCuotas.Text = this.LP.GetText("lblIVT01CuadreCuotas", "Máximo descuadre cuotas");
            this.lblCtaMayorContrap.Text = this.LP.GetText("lblIVT01CtaMayor", "Cuenta de Mayor");
            this.lblCodIVAContrap.Text = this.LP.GetText("lblIVT01CodIVA", "Código IVA");
            this.lblCodIVAAsociado.Text = this.LP.GetText("lblIVT01CodIVAAsoc", "Código IVA asociado");
            this.gbTipoIVA.Text = this.LP.GetText("lblIVT01TipoIVA", "Tipo de IVA");

            //------ Apartado Claves Legales -----
            this.radPageViewPageClavesLegales.Text = this.LP.GetText("lblIVT01ClavesLegales", "Claves Legales");
            this.lblCLMod303GrupoOperac.Text = this.LP.GetText("lblIVT01Mod303GrupoOperac", "Modelo 303   Grupo de operaciones");
            this.lblCLMod390GrupoOperac.Text = this.LP.GetText("lblIVT01Mod390GrupoOperacNuevo", "Modelo 303   Grupo de operaciones asociadas al último periodo de liquidación");
            this.lblCLMod340LibroRegistro.Text = this.LP.GetText("lblIVT01Mod340LibroReg", "Modelo 340   Libro registro");
            this.lblCLMod340ClaveOperac.Text = this.LP.GetText("lblIVT01Mod340ClaveOperac", "Modelo 340   Clave operaciones");
            this.lblCLMod349TipoOperac.Text = this.LP.GetText("lblIVT01Mod349TipoOperac", "Modelo 349   Tipo operación");
            this.lblCLMod180ModalPercep.Text = this.LP.GetText("lblIVT01Mod180ModalidadPercep", "Modelo 180   Modalidad de percepción");
            this.rbCLDinerario.Text = this.LP.GetText("lblIVT01Dinerario", "Dinerario");
            this.rbCLEspecie.Text = this.LP.GetText("lblIVT01Especie", "Especie");
            this.lblCLMod190ClavePercep.Text = this.LP.GetText("lblIVT01Mod190ClavePercep", "Modelo 190   Clave percepción");
            this.lblCLMod190SubclavePercep.Text = this.LP.GetText("lblIVT01Mod190SubclavePercep", "Modelo 190   Subclave percepción");
            this.lblCLMod111ClavePercep.Text = this.LP.GetText("lblIVT01Mod111ClavePercep", "Modelo 111   Clave percepción");
        }

        #region Validar Claves Legales
        /// <summary>
        /// Devuelve la sentencia SQL para obtener los datos para el Modelo 303 del grupo de operaciones
        /// </summary>
        /// <returns></returns>
        private string ObtenerQueryCLMod303GrupoOperac()
        {
            string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='M' and ";
            query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";
            query += "order by CODCD";

            return (query);
        }

        /// <summary>
        /// Devuelve la sentencia SQL para obtener los datos para el Modelo 390 del grupo de operaciones
        /// </summary>
        /// <returns></returns>
        private string ObtenerQueryCLMod390GrupoOperac()
        {
            string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

            string query = "select CODCD, DESCD from ";
            query += this.prefijoTablaCGCDES + "CGCDES ";
            query += "where TIPCD ='A' and ";
            query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";
            query += "order by CODCD";

            return (query);
        }
        #endregion

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el código de IVA (al dar de alta un código de IVA)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radPageViewDatos.Enabled = valor;
        }
        
        /// <summary>
        /// Rellena los controles con los datos del código de IVA (modo edición)
        /// </summary>
        private void CargarInfoCodigoIVA(string codIVA)
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + codIVA + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                this.existeCodigoIVA = false;
                string estado = "";
                string RESOCI = "";
                string DEDUCI = "";
                string CUADCI = "";
                string contrapartida = "";

                if (dr.Read())
                {
                    this.existeCodigoIVA = true;

                    estado = dr.GetValue(dr.GetOrdinal("STATCI")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                    else this.radToggleSwitchEstadoActivo.Value = false;

                    this.txtDescripcion.Text = dr.GetValue(dr.GetOrdinal("NOMBCI")).ToString().TrimEnd();
                    
                    RESOCI = dr.GetValue(dr.GetOrdinal("RESOCI")).ToString().TrimEnd();
                    if (RESOCI == "R") this.rbRepercutido.IsChecked = true;
                    else
                    {
                        //Ocultar los controles de claves legales para repercutido
                        this.MostrarControlesRepercutido(false);
                    }

                    DEDUCI = dr.GetValue(dr.GetOrdinal("DEDUCI")).ToString().TrimEnd();
                    if (DEDUCI == "N") this.rbNoDeducible.IsChecked = true;
                    
                    string cuentaMayor  = dr.GetValue(dr.GetOrdinal("CUENCI")).ToString().TrimEnd();
                    this.codigoCtaMayor = cuentaMayor.Trim();
                    if (cuentaMayor != "")
                    {
                        string cuentaMayorDesc = utilesCG.ObtenerDescDadoCodigo("GLM03", "CUENMC", "NOABMC", cuentaMayor, false, "").Trim();
                        if (cuentaMayorDesc != "") cuentaMayor = cuentaMayor + " " + separadorDesc + " " + cuentaMayorDesc;
                    }
                    this.radButtonTextBoxCtaMayor.Text = cuentaMayor;

                    string tipoAux = dr.GetValue(dr.GetOrdinal("TAUXCI")).ToString().TrimEnd();
                    this.codigoTipoAux = tipoAux.Trim();
                    if (tipoAux != "")
                    {
                        string tipoAuxDesc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", tipoAux, false, "").Trim();
                        if (tipoAuxDesc != "") tipoAux = tipoAux + " " + separadorDesc + " " + tipoAuxDesc;
                    }
                    this.radButtonTextBoxTipoAux.Text = tipoAux;

                    this.txtLibro.Text = dr.GetValue(dr.GetOrdinal("LIBRCI")).ToString().TrimEnd();
                    this.txtSerie.Text = dr.GetValue(dr.GetOrdinal("SERICI")).ToString().TrimEnd();
                    
                    CUADCI = dr.GetValue(dr.GetOrdinal("CUADCI")).ToString().TrimEnd();
                    if (CUADCI == "S") this.radToggleSwitchCuadreCuotas.Value = true;
                    else this.radToggleSwitchCuadreCuotas.Value = false;

                    if (this.radToggleSwitchCuadreCuotas.Value)
                    {
                        this.txtMaxDescuadreCuotas.Text = dr.GetValue(dr.GetOrdinal("MXAJCI")).ToString().TrimEnd();
                        if (this.txtMaxDescuadreCuotas.Text == "") this.txtMaxDescuadreCuotas.Text = "0,00";
                    }
                    else this.txtMaxDescuadreCuotas.Enabled = false;

                    string filtro = "and TIPLCI = '" + this.codigoPlan + "'"; 

                    contrapartida = dr.GetValue(dr.GetOrdinal("CUECCI")).ToString().TrimEnd();
                    if (contrapartida != "")
                    {
                        if (contrapartida.Substring(0, 1) == "+")
                        {
                            //Código de IVA de contrapartida
                            contrapartida = contrapartida.Substring(1, contrapartida.Length - 1);   //Eliminar el signo de +
                            string codIVAContrap = contrapartida;
                            this.codigoIVACont = codIVAContrap;
                            if (codIVAContrap != "")
                            {
                                string codIVAContrapDesc = utilesCG.ObtenerDescDadoCodigo("IVT01", "COIVCI", "NOMBCI", codIVAContrap, false, filtro).Trim();
                                if (codIVAContrapDesc != "") codIVAContrap = codIVAContrap + " " + separadorDesc + " " + codIVAContrapDesc;
                            }
                            this.radButtonTextBoxCodIVAContrap.Text = codIVAContrap;
                            this.codigoCtaMayorCont = "";
                        }
                        else
                        {
                            //Código de Cuenta de Mayor de contrapartida
                            string cuentaMayorContrap = contrapartida;
                            this.codigoCtaMayorCont = cuentaMayorContrap;
                            if (cuentaMayorContrap != "")
                            {
                                string cuentaMayorContrapDesc = utilesCG.ObtenerDescDadoCodigo("GLM03", "CUENMC", "NOABMC", cuentaMayorContrap, false, "").Trim();
                                if (cuentaMayorContrapDesc != "") cuentaMayorContrap = cuentaMayorContrap + " " + separadorDesc + " " + cuentaMayorContrapDesc;
                            }
                            this.radButtonTextBoxCtaMayorContrap.Text = cuentaMayorContrap;
                            this.codigoIVACont = "";
                        }
                    }
                    else
                    {
                        this.codigoCtaMayorCont = "";
                        this.codigoIVACont = "";
                    }

                    string codIVAAsociado = dr.GetValue(dr.GetOrdinal("COIACI")).ToString().TrimEnd();
                    this.codigoIVAAsociado = codIVAAsociado;
                    if (codIVAAsociado != "")
                    {
                        string codIVAAsociadoDesc = utilesCG.ObtenerDescDadoCodigo("IVT01", "COIVCI", "NOMBCI", codIVAAsociado, false, filtro).Trim();
                        if (codIVAAsociadoDesc != "") codIVAAsociado = codIVAAsociado + " " + separadorDesc + " " + codIVAAsociadoDesc;
                    }
                    this.radButtonTextBoxCodIVAAsociado.Text = codIVAAsociado;
                }

                dr.Close();

                if (this.existeCodigoIVA)
                {
                    //Cargar los Tipos de IVA
                    this.CargarInfoTiposIVA(codIVA);

                    //Cargar las claves legales
                    this.CargarInfoClavesLegales(codIVA);

                    //Cargar Sii Libro
                    this.CargarInfoSiiLibro(codIVA);

                    if (!this.copiar)
                    {
                        //Comprobar si el codigo de IVA se puede modificar o suprimir
                        this.modificarEliminarCodigoIVA = this.CodigoIVAPermiteModificarEliminar();

                        if (!this.modificarEliminarCodigoIVA)
                        {
                            this.gbRepercSopor.Enabled = false;
                            this.gbDeducibleNoDed.Enabled = false;
                            this.radButtonTextBoxCtaMayor.Enabled = false;
                            this.radButtonTextBoxTipoAux.Enabled = false;
                            this.txtLibro.Enabled = false;
                            this.txtSerie.Enabled = false;
                            this.radToggleSwitchCuadreCuotas.Enabled = false;
                            this.txtMaxDescuadreCuotas.Enabled = false;
                        }
                        else //jl
                        {
                            this.gbRepercSopor.Enabled = false;
                            this.gbDeducibleNoDed.Enabled = false;
                            this.radButtonTextBoxCtaMayor.Enabled = false;
                            this.radButtonTextBoxTipoAux.Enabled = false;
                            this.txtLibro.Enabled = false;
                            this.txtSerie.Enabled = false;
                        } //jl
                    }
                }

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            this.cargarInfoDeTiposIVA = false;
        }

        /// <summary>
        /// Crea el DataGrid que contiene los tipos de IVA
        /// </summary>
        private void BuildDataGridTiposIVA()
        {
            /*
            //Create the data source and fill some data
            this.dtTiposIVA = new DataTable();
            this.dtTiposIVA.Columns.Add("FechaEfectiva", typeof(string));
            this.dtTiposIVA.Columns.Add("PorcIVA", typeof(decimal));
            this.dtTiposIVA.Columns.Add("PorcRecEquiv", typeof(decimal));
            this.dtTiposIVA.Columns.Add("Descripcion", typeof(string));
            this.dtTiposIVA.Columns.Add("Eliminar", typeof(Image));
            this.dtTiposIVA.Columns.Add("FechaEfectivaOrigen", typeof(string));
            this.dtTiposIVA.Columns.Add("PorcIVAOrigen", typeof(string));
            this.dtTiposIVA.Columns.Add("PorcRecEquivOrigen", typeof(string));
            this.dtTiposIVA.Columns.Add("DescripcionOrigen", typeof(string));
            this.dtTiposIVA.Columns.Add("PermiteModificar", typeof(string));
            */

            //allow the grid to genetate its columns
            this.radGridViewTiposIVA.MasterTemplate.AutoGenerateColumns = false;

            GridViewDateTimeColumn colFecha = new GridViewDateTimeColumn
            {
                Name = "FechaEfectiva",
                HeaderText = this.LP.GetText("lblfrmMtoIVT01FechaEfectiva", "Fecha Efectiva"),

                FormatString = "{0:dd/MM/yyyy}", // specify cell's format
                Format = DateTimePickerFormat.Custom,//specify the editor's format
                CustomFormat = "dd/MM/yyyy"
            };

            this.radGridViewTiposIVA.Columns.Add(colFecha);

            GridViewDecimalColumn colPorcIVA = new GridViewDecimalColumn
            {
                Name = "PorcIVA",
                HeaderText = "Porcentaje IVA"
            };
            //colPorcIVA.DecimalPlaces = 7;
            //colTasaCambio.ThousandsSeparator = true;
            colPorcIVA.DecimalPlaces = 2;
            colPorcIVA.FormatString = "{0:N2}";
            this.radGridViewTiposIVA.Columns.Add(colPorcIVA);

            //GridViewTextBoxColumn colPorcRecEquiv = new GridViewTextBoxColumn
            GridViewDecimalColumn colPorcRecEquiv = new GridViewDecimalColumn
            {
                Name = "PorcRecEquiv",
                HeaderText = "Porcentaje Recargo Equivalencia"
            };
            colPorcRecEquiv.DecimalPlaces = 2;
            colPorcRecEquiv.FormatString = "{0:N2}";
            this.radGridViewTiposIVA.Columns.Add(colPorcRecEquiv);

            GridViewTextBoxColumn colDescripcion = new GridViewTextBoxColumn
            {
                Name = "Descripcion",
                HeaderText = "Descripción"
            };
            this.radGridViewTiposIVA.Columns.Add(colDescripcion);

            this.radGridViewTiposIVA.Columns.Add(new GridViewImageColumn("Eliminar"));

            GridViewTextBoxColumn colFechaEfectivaOrigen = new GridViewTextBoxColumn
            {
                Name = "FechaEfectivaOrigen",
                HeaderText = "FechaEfectivaOrigen",
                IsVisible = false
            };
            this.radGridViewTiposIVA.Columns.Add(colFechaEfectivaOrigen);

            GridViewTextBoxColumn colPorcIVAOrigen = new GridViewTextBoxColumn
            {
                Name = "PorcIVAOrigen",
                HeaderText = "PorcIVAOrigen",
                IsVisible = false
            };
            this.radGridViewTiposIVA.Columns.Add(colPorcIVAOrigen);

            GridViewTextBoxColumn colPorcRecEquivOrigen = new GridViewTextBoxColumn
            {
                Name = "PorcRecEquivOrigen",
                HeaderText = "PorcRecEquivOrigen",
                IsVisible = false
            };
            this.radGridViewTiposIVA.Columns.Add(colPorcRecEquivOrigen);

            GridViewTextBoxColumn colDescripcionOrigen = new GridViewTextBoxColumn
            {
                Name = "DescripcionOrigen",
                HeaderText = "DescripcionOrigen",
                IsVisible = false
            };
            this.radGridViewTiposIVA.Columns.Add(colDescripcionOrigen);

            GridViewTextBoxColumn colPermiteModificar = new GridViewTextBoxColumn
            {
                Name = "PermiteModificar",
                HeaderText = "PermiteModificar",
                IsVisible = false
            };
            this.radGridViewTiposIVA.Columns.Add(colPermiteModificar);
            
            /*
            foreach (var column in this.dgTipoIVA.Columns)
            {
                if (column is DataGridViewImageColumn)
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
            }
            */
        }

        /// <summary>
        /// Carla los tipos der iva en los controles
        /// </summary>
        private void CargarInfoTiposIVA(string codIVA)
        {
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTX1 ";
                query += "where TIPLCX = '" + this.CodigoPlan + "' and COIVCX ='" + codIVA + "'";

                //this.dtTiposIVA = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                this.dtTiposIVAGrid = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                string recEquiv = "";

                if (this.dtTiposIVAGrid != null && this.dtTiposIVAGrid.Rows.Count >= 1)
                {
                    GridViewDataRowInfo rowInfo;
                    string fecha = "";

                    DateTime fechaFormato;
                    
                    for (int i = 0; i < this.dtTiposIVAGrid.Rows.Count; i++)
                    {
                        rowInfo = new GridViewDataRowInfo(this.radGridViewTiposIVA.MasterView);

                        fecha = this.dtTiposIVAGrid.Rows[i]["FEIVCX"].ToString();
                        fechaFormato = utiles.FormatoCGToFecha(fecha);

                        //rowInfo.Cells["Fecha"].Value = this.FechaConFormato(fecha);
                        rowInfo.Cells["FechaEfectiva"].Value = fechaFormato;

                        rowInfo.Cells["PorcIVA"].Value = this.dtTiposIVAGrid.Rows[i]["TPIVCX"].ToString();

                        //recEquiv = this.dtTiposIVAGrid.Rows[i]["RECGCX"].ToString();
                        //if (recEquiv != "")
                        //{
                        //    try
                        //    {
                        //        decimal recEquivDec = Convert.ToDecimal(recEquiv);
                        //        if (recEquivDec == 0) recEquiv = "";
                        //    }
                        //    catch { }
                        //}

                        rowInfo.Cells["PorcRecEquiv"].Value = this.dtTiposIVAGrid.Rows[i]["RECGCX"].ToString();
                        rowInfo.Cells["Descripcion"].Value = this.dtTiposIVAGrid.Rows[i]["NOMBCX"].ToString();
                        rowInfo.Cells["Eliminar"].Value = global::ModMantenimientos.Properties.Resources.Eliminar;

                        if (this.copiar)
                        {
                            rowInfo.Cells["FechaEfectivaOrigen"].Value = "";
                            rowInfo.Cells["PorcIVAOrigen"].Value = "";
                            rowInfo.Cells["PorcRecEquivOrigen"].Value = "";
                            rowInfo.Cells["DescripcionOrigen"].Value = "";
                        }
                        else
                        {
                            rowInfo.Cells["FechaEfectivaOrigen"].Value = rowInfo.Cells["FechaEfectiva"].ToString();
                            rowInfo.Cells["PorcIVAOrigen"].Value = rowInfo.Cells["PorcIVA"].ToString();
                            rowInfo.Cells["PorcRecEquivOrigen"].Value = rowInfo.Cells["PorcRecEquiv"].ToString();
                            rowInfo.Cells["DescripcionOrigen"].Value = rowInfo.Cells["Descripcion"].ToString();
                        }

                        rowInfo.Cells["PermiteModificar"].Value = "1";

                        this.radGridViewTiposIVA.Rows.Add(rowInfo);

                        /*
                        tasaCambioActual = Convert.ToDecimal(this.dtTasaCambioGrid.Rows[i]["CAMBMF"]);
                        rowInfo.Cells["TasaCambio"].Value = tasaCambioActual.ToString("N7");*/
                    }
                }


                this.radGridViewTiposIVA.Refresh();

                if (this.radGridViewTiposIVA.Rows != null && this.radGridViewTiposIVA.Rows.Count > 0)
                {
                    for (int i = 0; i < this.radGridViewTiposIVA.Columns.Count; i++)
                    {
                        this.radGridViewTiposIVA.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    }

                    this.radGridViewTiposIVA.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewTiposIVA.MasterTemplate.BestFitColumns();

                    this.radGridViewTiposIVA.CurrentRow = null;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Carga las claves legales en los controles
        /// </summary>
        private void CargarInfoClavesLegales(string codIVA)
        {
            IDataReader dr = null;

            this.codigoCLMod303GrupoOperac = "";
            this.codigoCLMod390GrupoOperac = "";
            this.codigoCLMod340LibroRegistro = "";
            this.codigoCLMod340ClaveOperac = "";
            this.codigoCLMod349TipoOperac = "";
            this.codigoCLMod190ClavePercep = "";
            this.codigoCLMod111ClavePercep = "";
            
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTA1 ";
                query += "where TIPLAI = '" + this.CodigoPlan + "' and COIVAI ='" + codIVA + "'";

                string descResult = "";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

                if (dr.Read())
                {
                    string CLMod303GrupoOperac = dr.GetValue(dr.GetOrdinal("CIVMAI")).ToString().TrimEnd();
                    this.codigoCLMod303GrupoOperac = CLMod303GrupoOperac.Trim();
                    if (this.codigoCLMod303GrupoOperac != "")
                    {
                        string codigoCLMod303GrupoOperacDesc = "";
                        descResult = this.ValidarCLMod303GrupoOperacDesc(this.codigoCLMod303GrupoOperac, ref codigoCLMod303GrupoOperacDesc);
                        if (codigoCLMod303GrupoOperacDesc != "") CLMod303GrupoOperac = CLMod303GrupoOperac + " " + separadorDesc + " " + codigoCLMod303GrupoOperacDesc;
                    }
                    this.radButtonTextBoxCLMod303GrupoOperac.Text = CLMod303GrupoOperac;

                    string CLMod390GrupoOperac = dr.GetValue(dr.GetOrdinal("CIVAAI")).ToString().TrimEnd();
                    this.codigoCLMod390GrupoOperac = CLMod390GrupoOperac.Trim();
                    if (this.codigoCLMod390GrupoOperac != "")
                    {
                        string codigoCLMod390GrupoOperacDesc = "";
                        descResult = this.ValidarCLMod390GrupoOperacDesc(this.codigoCLMod390GrupoOperac, ref codigoCLMod390GrupoOperacDesc);
                        if (codigoCLMod390GrupoOperacDesc != "") CLMod390GrupoOperac = CLMod390GrupoOperac + " " + separadorDesc + " " + codigoCLMod390GrupoOperacDesc;
                    }
                    this.radButtonTextBoxCLMod390GrupoOperac.Text = CLMod390GrupoOperac;
                }

                dr.Close();

                query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTA2 ";
                query += "where TIPLA2 = '" + this.CodigoPlan + "' and COIVA2 ='" + codIVA + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    string CLMod340LibroRegistro = dr.GetValue(dr.GetOrdinal("LIBRA2")).ToString();
                    this.codigoCLMod340LibroRegistro = CLMod340LibroRegistro;
                    if (this.codigoCLMod340LibroRegistro != "")
                    {
                        string codigoCLMod340LibroRegistroDesc = "";
                        descResult = this.ValidarCLMod340LibroRegistroDesc(CLMod340LibroRegistro, ref codigoCLMod340LibroRegistroDesc);
                        if (codigoCLMod340LibroRegistroDesc != "") CLMod340LibroRegistro = CLMod340LibroRegistro + " " + separadorDesc + " " + codigoCLMod340LibroRegistroDesc;
                    }
                    this.radButtonTextBoxCLMod340LibroRegistro.Text = CLMod340LibroRegistro;

                    string CLMod340ClaveOperac = dr.GetValue(dr.GetOrdinal("COPEA2")).ToString();
                    this.codigoCLMod340ClaveOperac = CLMod340ClaveOperac;
                    if (this.codigoCLMod340ClaveOperac != "")
                    {                      
                        string codigoCLMod340ClaveOperacDesc = "";
                        descResult = this.ValidarCLMod340ClaveOperacDesc(CLMod340ClaveOperac, ref codigoCLMod340ClaveOperacDesc);
                        if (codigoCLMod340ClaveOperacDesc != "") CLMod340ClaveOperac = CLMod340ClaveOperac + " " + separadorDesc + " " + codigoCLMod340ClaveOperacDesc;
                    }
                    this.radButtonTextBoxCLMod340ClaveOperac.Text = CLMod340ClaveOperac;

                    if (this.rbRepercutido.IsChecked)
                    {
                        string CLMod349TipoOperac = dr.GetValue(dr.GetOrdinal("TOPEA2")).ToString();
                        this.codigoCLMod349TipoOperac = CLMod349TipoOperac;
                        if (this.codigoCLMod349TipoOperac != "")
                        {
                            string codigoCLMod349TipoOperacDesc = "";
                            descResult = this.ValidarCLMod349TipoOperacDesc(CLMod349TipoOperac, ref codigoCLMod349TipoOperacDesc);
                            if (codigoCLMod349TipoOperacDesc != "") CLMod349TipoOperac = CLMod349TipoOperac + " " + separadorDesc + " " + codigoCLMod349TipoOperacDesc;
                        }
                        this.radButtonTextBoxlCLMod349TipoOperac.Text = CLMod349TipoOperac;

                        string dinerarioEspecie = dr.GetValue(dr.GetOrdinal("IND3A2")).ToString();
                        switch (dinerarioEspecie)
                        {
                            case "":
                            case "0":
                                this.rbCLDinerario.IsChecked = false;
                                this.rbCLEspecie.IsChecked = false;
                                break;
                            case "1":
                                this.rbCLDinerario.IsChecked = true;
                                this.rbCLEspecie.IsChecked = false;
                                break;
                            case "2":
                                this.rbCLDinerario.IsChecked = false;
                                this.rbCLEspecie.IsChecked = true;
                                break;
                        }

                        string CLMod190ClavePercep = dr.GetValue(dr.GetOrdinal("IND4A2")).ToString();
                        this.codigoCLMod190ClavePercep = CLMod190ClavePercep;
                        if (this.codigoCLMod190ClavePercep != "")
                        {
                            string codigoCLMod190ClavePercepDesc = "";
                            descResult = this.ValidarCLMod190ClavePercepDesc(CLMod190ClavePercep, ref codigoCLMod190ClavePercepDesc);
                            if (codigoCLMod190ClavePercepDesc != "") CLMod190ClavePercep = CLMod190ClavePercep + " " + separadorDesc + " " + codigoCLMod190ClavePercepDesc;
                        }
                        this.radButtonTextBoxCLMod190ClavePercep.Text = CLMod190ClavePercep;

                        if (this.radButtonTextBoxCLMod190ClavePercep.Text != "")
                        {
                            //Rellenar el desplegable con las posibles subclaves
                            this.FillcmbCLMod190SubclavePercep();
                            this.cmbCLMod190SubclavePercep.Enabled = true;

                            string subclave = dr.GetValue(dr.GetOrdinal("IND1A2")).ToString();

                            if (subclave != "")
                            {
                                for (int i = 0; i < this.cmbCLMod190SubclavePercep.Items.Count; i++)
                                {
                                    if (this.cmbCLMod190SubclavePercep.Items[i].ToString() == subclave)
                                    {
                                        this.cmbCLMod190SubclavePercep.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else this.cmbCLMod190SubclavePercep.Enabled = false;

                        string CLMod111ClavePercep = dr.GetValue(dr.GetOrdinal("IND2A2")).ToString();
                        this.codigoCLMod111ClavePercep = CLMod111ClavePercep;
                        if (this.codigoCLMod111ClavePercep != "")
                        {
                            string codigoCLMod111ClavePercepDesc = "";
                            descResult = this.ValidarCLMod111ClavePercepDesc(CLMod111ClavePercep, ref codigoCLMod111ClavePercepDesc);
                            if (codigoCLMod111ClavePercepDesc != "") CLMod111ClavePercep = CLMod111ClavePercep + " " + separadorDesc + " " + codigoCLMod111ClavePercepDesc;
                        }
                        this.radButtonTextBoxCLMod111ClavePercep.Text = CLMod111ClavePercep;
                    }
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }


        /// <summary>
        /// Carga las claves legales en los controles
        /// </summary>
        private void CargarInfoSiiLibro(string codIVA)
        {
            IDataReader dr = null;
            bool existeLibro = false;

            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTA3 ";
                query += "where TIPLA3 = '" + this.CodigoPlan + "' and COIVA3 ='" + codIVA + "'";

                string descResult = "";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    string codigoAux = "";
                    string agenciaTributariaDesc = "";

                    string siiLibro = dr.GetValue(dr.GetOrdinal("TDOCA3")).ToString().TrimEnd();
                    string siitdsg = dr.GetValue(dr.GetOrdinal("TDSGA3")).ToString().TrimEnd();

                    switch (siiLibro)
                    {
                        case "S":   //Operaciones de seguros
                            this.cmbSiiLibros.SelectedIndex = 3;
                            existeLibro = true;

                            this.sii_opSeg_tipoOperacion = dr.GetValue(dr.GetOrdinal("SEGUA3")).ToString().TrimEnd();
                            string tipoOperacionSeguroDesc = "";
                            if (this.sii_opSeg_tipoOperacion != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_opSeg_tipoOperacion, "X", ref tipoOperacionSeguroDesc);
                                if (tipoOperacionSeguroDesc != "") tipoOperacionSeguroDesc = this.sii_opSeg_tipoOperacion + " " + separadorDesc + " " + tipoOperacionSeguroDesc;
                            }
                            this.radButtonTextBoxSiiTipoOpSeguro.Text = tipoOperacionSeguroDesc;

                            codigoAux = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            if (codigoAux.Length >= 2) this.sii_agenciaTributaria = codigoAux.Substring(1, 1).TrimEnd();
                            else this.sii_agenciaTributaria = "";

                            if (this.sii_agenciaTributaria != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref agenciaTributariaDesc);
                                if (agenciaTributariaDesc != "") agenciaTributariaDesc = this.sii_agenciaTributaria + " " + separadorDesc + " " + agenciaTributariaDesc;
                            }
                            this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text = agenciaTributariaDesc;

                            break;
                        case "U":   //Operaciones intracomunitarias
                            this.cmbSiiLibros.SelectedIndex = 4;
                            existeLibro = true;

                            this.sii_opIntracom_tipoOperacion = dr.GetValue(dr.GetOrdinal("TOPIA3")).ToString().TrimEnd();
                            string tipoOperacionIntracomDesc = "";
                            if (this.sii_opIntracom_tipoOperacion != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_opIntracom_tipoOperacion, "D", ref tipoOperacionIntracomDesc);
                                if (tipoOperacionIntracomDesc != "") tipoOperacionIntracomDesc = this.sii_opIntracom_tipoOperacion + " " + separadorDesc + " " + tipoOperacionIntracomDesc;
                            }
                            this.radButtonTextBoxSiiTipoOpIntracom.Text = tipoOperacionIntracomDesc;

                            codigoAux = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            if (codigoAux.Length >= 2) this.sii_agenciaTributaria = codigoAux.Substring(1, 1).TrimEnd();
                            else this.sii_agenciaTributaria = "";

                            if (this.sii_agenciaTributaria != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref agenciaTributariaDesc);
                                if (agenciaTributariaDesc != "") agenciaTributariaDesc = this.sii_agenciaTributaria + " " + separadorDesc + " " + agenciaTributariaDesc;
                            }
                            this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text = agenciaTributariaDesc;

                            break;
                        case "E":   //Facturas expedidas
                            this.cmbSiiLibros.SelectedIndex = 1;
                            existeLibro = true;

                            this.sii_factE_R_tipoFactura = dr.GetValue(dr.GetOrdinal("TFACA3")).ToString().TrimEnd();
                            string tipoFacturaDesc = "";
                            if (this.sii_factE_R_tipoFactura != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref tipoFacturaDesc);
                                if (tipoFacturaDesc != "") tipoFacturaDesc = this.sii_factE_R_tipoFactura + " " + separadorDesc + " " + tipoFacturaDesc;
                            }
                            this.radButtonTextBoxSiiTipoFactura.Text = tipoFacturaDesc;

                            this.sii_factE_R_claveRegEspeTrasc = dr.GetValue(dr.GetOrdinal("COPEA3")).ToString().TrimEnd();
                            string claveRegEspeTrascDesc = "";
                            if (this.sii_factE_R_claveRegEspeTrasc != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "E", ref claveRegEspeTrascDesc);
                                if (claveRegEspeTrascDesc != "") claveRegEspeTrascDesc = this.sii_factE_R_claveRegEspeTrasc + " " + separadorDesc + " " + claveRegEspeTrascDesc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrasc.Text = claveRegEspeTrascDesc;

                            this.sii_factE_R_claveRegEspeTrascAdicional1 = dr.GetValue(dr.GetOrdinal("COP1A3")).ToString().TrimEnd();
                            string claveRegEspeTrascAdicional1Desc = "";
                            if (this.sii_factE_R_claveRegEspeTrascAdicional1 != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "E", ref claveRegEspeTrascAdicional1Desc);
                                if (claveRegEspeTrascAdicional1Desc != "") claveRegEspeTrascAdicional1Desc = this.sii_factE_R_claveRegEspeTrascAdicional1 + " " + separadorDesc + " " + claveRegEspeTrascAdicional1Desc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text = claveRegEspeTrascAdicional1Desc;

                            this.sii_factE_R_claveRegEspeTrascAdicional2 = dr.GetValue(dr.GetOrdinal("COP2A3")).ToString().TrimEnd();
                            string claveRegEspeTrascAdicional2Desc = "";
                            if (this.sii_factE_R_claveRegEspeTrascAdicional2 != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "E", ref claveRegEspeTrascAdicional2Desc);
                                if (claveRegEspeTrascAdicional2Desc != "") claveRegEspeTrascAdicional2Desc = this.sii_factE_R_claveRegEspeTrascAdicional2 + " " + separadorDesc + " " + claveRegEspeTrascAdicional2Desc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text = claveRegEspeTrascAdicional2Desc;

                            this.sii_factE_R_tipoFacturaRectif = dr.GetValue(dr.GetOrdinal("TFARA3")).ToString().TrimEnd();
                            string tipoFacturaRectifDesc = "";
                            if (this.sii_factE_R_tipoFacturaRectif != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref tipoFacturaRectifDesc);
                                if (tipoFacturaRectifDesc != "") tipoFacturaRectifDesc = this.sii_factE_R_tipoFacturaRectif + " " + separadorDesc + " " + tipoFacturaRectifDesc;
                            }
                            this.radButtonTextBoxSiiTipoFacturaRectif.Text = tipoFacturaRectifDesc;

                            this.sii_factE_R_tipoDesglose = dr.GetValue(dr.GetOrdinal("TDSGA3")).ToString().TrimEnd();
                            this.radDropDownListSiiTipoDesglose.SelectedIndex = 0;
                            switch (this.sii_factE_R_tipoDesglose)
                            {
                                case "F":
                                    this.radDropDownListSiiTipoDesglose.SelectedIndex = 0;
                                    break;
                                case "E":
                                    this.radDropDownListSiiTipoDesglose.SelectedIndex = 1;
                                    break;
                                case "S":
                                    this.radDropDownListSiiTipoDesglose.SelectedIndex = 2;
                                    break;
                            }

                            this.sii_factE_tipoOperacionSujetaNoExenta = dr.GetValue(dr.GetOrdinal("TSNEA3")).ToString().TrimEnd();
                            string tipoOperacionSujetaNoExentaDesc = "";
                            if (this.sii_factE_tipoOperacionSujetaNoExenta != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_tipoOperacionSujetaNoExenta, "H", ref tipoOperacionSujetaNoExentaDesc);
                                if (tipoOperacionSujetaNoExentaDesc != "") tipoOperacionSujetaNoExentaDesc = this.sii_factE_tipoOperacionSujetaNoExenta + " " + separadorDesc + " " + tipoOperacionSujetaNoExentaDesc;
                            }
                            this.radButtonTextBoxSiiTipoOperSujNoExe.Text = tipoOperacionSujetaNoExentaDesc;

                            this.sii_factE_causaExencionOpSujetasExentas = dr.GetValue(dr.GetOrdinal("TEXEA3")).ToString().TrimEnd();
                            string causaExencionOpSujetasExentasDesc = "";
                            if (this.sii_factE_causaExencionOpSujetasExentas != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_causaExencionOpSujetasExentas, "B", ref causaExencionOpSujetasExentasDesc);
                                if (causaExencionOpSujetasExentasDesc != "") causaExencionOpSujetasExentasDesc = this.sii_factE_causaExencionOpSujetasExentas + " " + separadorDesc + " " + causaExencionOpSujetasExentasDesc;
                            }
                            this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text = causaExencionOpSujetasExentasDesc;

                            this.sii_factE_tipoOperacNoSujeta = dr.GetValue(dr.GetOrdinal("TNSJA3")).ToString().TrimEnd();
                            string tipoOperacNoSujetaDesc = "";
                            if (this.sii_factE_tipoOperacNoSujeta != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_tipoOperacNoSujeta, "K", ref tipoOperacNoSujetaDesc);
                                if (tipoOperacNoSujetaDesc != "") tipoOperacNoSujetaDesc = this.sii_factE_tipoOperacNoSujeta + " " + separadorDesc + " " + tipoOperacNoSujetaDesc;
                            }
                            this.radButtonTextBoxSiiTipoOperacNoSujeta.Text = tipoOperacNoSujetaDesc;

                            codigoAux = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            if (codigoAux.Length >= 2) this.sii_agenciaTributaria = codigoAux.Substring(1, 1).TrimEnd();
                            else this.sii_agenciaTributaria = "";

                            if (this.sii_agenciaTributaria != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref agenciaTributariaDesc);
                                if (agenciaTributariaDesc != "") agenciaTributariaDesc = this.sii_agenciaTributaria + " " + separadorDesc + " " + agenciaTributariaDesc;
                            }
                            this.radButtonTextBoxSiiAgenciaTributaria.Text = agenciaTributariaDesc;

                            this.sii_factE_cobroMetalico = dr.GetValue(dr.GetOrdinal("COMEA3")).ToString();
                            this.radToggleSwitchSiiCobroMetalico.Value = this.sii_factE_cobroMetalico == "S" ? true : false;

                            this.sii_factE_cupon = dr.GetValue(dr.GetOrdinal("CUPOA3")).ToString();
                            this.radToggleSwitchSiiCupon.Value = this.sii_factE_cupon == "S" ? true : false;

                            this.sii_factE_taxFree = dr.GetValue(dr.GetOrdinal("IND1A3")).ToString();
                            this.sii_factE_taxFree = this.sii_factE_taxFree.Substring(0, 1);
                            this.radToggleSwitchSiiTaxFree.Value = this.sii_factE_taxFree == "S" ? true : false;

                            this.sii_factE_viajeros = dr.GetValue(dr.GetOrdinal("IND1A3")).ToString();
                            this.sii_factE_viajeros = this.sii_factE_viajeros.Substring(1, 1);
                            this.radToggleSwitchSiiViajeros.Value = this.sii_factE_viajeros == "S" ? true : false;

                            this.sii_factE_R_factSimplArt72 = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            this.sii_factE_R_factSimplArt72 = this.sii_factE_R_factSimplArt72.Substring(0, 1);
                            this.radToggleSwitchSiiFactSimpArt72_73.Value = this.sii_factE_R_factSimplArt72 == "S" ? true : false;

                            this.sii_factE_campoLibre = dr.GetValue(dr.GetOrdinal("FILLA3")).ToString();
                            this.radButtonTextBoxSiiCampoLibreExp.Text = sii_factE_campoLibre;

                            break;
                        case "R":   //Facturas recibidas
                            this.cmbSiiLibros.SelectedIndex = 2;
                            existeLibro = true;

                            this.sii_factE_R_tipoFactura = dr.GetValue(dr.GetOrdinal("TFACA3")).ToString().TrimEnd();
                            string tipoFacturaRecibidaDesc = "";
                            if (this.sii_factE_R_tipoFactura != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref tipoFacturaRecibidaDesc);
                                if (tipoFacturaRecibidaDesc != "") tipoFacturaRecibidaDesc = this.sii_factE_R_tipoFactura + " " + separadorDesc + " " + tipoFacturaRecibidaDesc;
                            }
                            this.radButtonTextBoxlSiiTipoFacturaRecibida.Text = tipoFacturaRecibidaDesc;

                            this.sii_factE_R_claveRegEspeTrasc = dr.GetValue(dr.GetOrdinal("COPEA3")).ToString().TrimEnd();
                            string claveRegEspeTrascFactRecibidaDesc = "";
                            if (this.sii_factE_R_claveRegEspeTrasc != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "F", ref claveRegEspeTrascFactRecibidaDesc);
                                if (claveRegEspeTrascFactRecibidaDesc != "") claveRegEspeTrascFactRecibidaDesc = this.sii_factE_R_claveRegEspeTrasc + " " + separadorDesc + " " + claveRegEspeTrascFactRecibidaDesc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text = claveRegEspeTrascFactRecibidaDesc;

                            this.sii_factE_R_claveRegEspeTrascAdicional1 = dr.GetValue(dr.GetOrdinal("COP1A3")).ToString().TrimEnd();
                            string claveRegEspeTrascAdicional1FactRecibidaDesc = "";
                            if (this.sii_factE_R_claveRegEspeTrascAdicional1 != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "F", ref claveRegEspeTrascAdicional1FactRecibidaDesc);
                                if (claveRegEspeTrascAdicional1FactRecibidaDesc != "") claveRegEspeTrascAdicional1FactRecibidaDesc = this.sii_factE_R_claveRegEspeTrascAdicional1 + " " + separadorDesc + " " + claveRegEspeTrascAdicional1FactRecibidaDesc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text = claveRegEspeTrascAdicional1FactRecibidaDesc;

                            this.sii_factE_R_claveRegEspeTrascAdicional2 = dr.GetValue(dr.GetOrdinal("COP2A3")).ToString().TrimEnd();
                            string claveRegEspeTrascAdicional2FactRecibidaDesc = "";
                            if (this.sii_factE_R_claveRegEspeTrascAdicional2 != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "F", ref claveRegEspeTrascAdicional2FactRecibidaDesc);
                                if (claveRegEspeTrascAdicional2FactRecibidaDesc != "") claveRegEspeTrascAdicional2FactRecibidaDesc = this.sii_factE_R_claveRegEspeTrascAdicional2 + " " + separadorDesc + " " + claveRegEspeTrascAdicional2FactRecibidaDesc;
                            }
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text = claveRegEspeTrascAdicional2FactRecibidaDesc;

                            this.sii_factE_R_tipoFacturaRectif = dr.GetValue(dr.GetOrdinal("TFARA3")).ToString().TrimEnd();
                            string tipoFacturaRectifFactRecibidaDesc = "";
                            if (this.sii_factE_R_tipoFacturaRectif != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref tipoFacturaRectifFactRecibidaDesc);
                                if (tipoFacturaRectifFactRecibidaDesc != "") tipoFacturaRectifFactRecibidaDesc = this.sii_factE_R_tipoFacturaRectif + " " + separadorDesc + " " + tipoFacturaRectifFactRecibidaDesc;
                            }
                            this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text = tipoFacturaRectifFactRecibidaDesc;

                            this.sii_factE_R_tipoDesglose = dr.GetValue(dr.GetOrdinal("TDSGA3")).ToString().TrimEnd();
                            this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex = 0;
                            switch (this.sii_factE_R_tipoDesglose)
                            {
                                case "I":
                                    this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex = 0;
                                    // this.radDropDownListSiiTipoDesglose.SelectedIndex = 0;
                                    break;
                                case "P":
                                    this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex = 1;
                                    // this.radDropDownListSiiTipoDesglose.SelectedIndex = 1;
                                    break;
                            }

                            this.sii_factR_bienesInversion = dr.GetValue(dr.GetOrdinal("BINVA3")).ToString();
                            this.radToggleSwitchSiiBienesInversionFactRecibida.Value = this.sii_factR_bienesInversion == "S" ? true : false;

                            this.sii_factR_prorrata = dr.GetValue(dr.GetOrdinal("PRTAA3")).ToString();
                            this.radToggleSwitchSiiProrrataFactRecibida.Value = this.sii_factR_prorrata == "S" ? true : false;

                            this.sii_factR_excluirBaseTotalFacturaProrrata = dr.GetValue(dr.GetOrdinal("NACUA3")).ToString();
                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value = this.sii_factR_excluirBaseTotalFacturaProrrata == "S" ? true : false;

                            //OJO - tienes que ser decimal y no booleano. ARREGLAR!!!!
                            string valorAux = dr.GetValue(dr.GetOrdinal("TIVPA3")).ToString();
                            try
                            {
                                this.sii_factR_tipoImpositivoProrrata = Convert.ToDecimal(valorAux);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                            }

                            txtSiiTipoImpositivoProrrataFactRecibida.Text = Convert.ToString(sii_factR_tipoImpositivoProrrata);

                            codigoAux = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            if (codigoAux.Length >= 2) this.sii_agenciaTributaria = codigoAux.Substring(1, 1).TrimEnd();
                            else this.sii_agenciaTributaria = "";

                            if (this.sii_agenciaTributaria != "")
                            {
                                descResult = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref agenciaTributariaDesc);
                                if (agenciaTributariaDesc != "") agenciaTributariaDesc = this.sii_agenciaTributaria + " " + separadorDesc + " " + agenciaTributariaDesc;
                            }
                            this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text = agenciaTributariaDesc;

                            this.sii_factE_R_factSimplArt72 = dr.GetValue(dr.GetOrdinal("IND2A3")).ToString();
                            this.sii_factE_R_factSimplArt72 = this.sii_factE_R_factSimplArt72.Substring(0, 1);
                            this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value = this.sii_factE_R_factSimplArt72 == "S" ? true : false;

                            this.sii_factR_campoLibre = dr.GetValue(dr.GetOrdinal("FILLA3")).ToString();
                            this.radButtonTextBoxSiiCampoLibreRec.Text = sii_factR_campoLibre;

                            break;
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            if (existeLibro)
            {
                this.cmbSiiLibros.ReadOnly = true;
                this.cmbSiiLibros.Enabled = false;
                this.radButtonEliminarLibro.Visible = true;
            }
            else
            {
                this.cmbSiiLibros.ReadOnly = false;
                this.cmbSiiLibros.Enabled = true;
                this.radButtonEliminarLibro.Visible = false;
            }
        }
        
        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            Telerik.WinControls.UI.RadPageViewPage pageActiva = this.radPageViewDatos.SelectedPage;

            bool tabGeneralError = false;
            bool tabClavesLegalesError = false;
            bool tabSiiLibroError = false;

            try
            {
                //Activar la pestaña de Datos Generales
                this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;

                //Terminar la edición de los tipos de IVA
                this.radGridViewTiposIVA.EndEdit();

                //-------------- Validar el nombre del código de IVA ------------------
                if (this.txtDescripcion.Text.Trim() == "")
                {
                    errores += "- La descripción no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtDescripcion;
                    this.txtDescripcion.Select();
                    this.txtDescripcion.Focus();
                    tabGeneralError = true;
                }

                //-------------- Validar la cuenta de mayor ------------------
                if (this.radButtonTextBoxCtaMayor.Text.Trim() != "")
                {
                    this.codigoCtaMayor = this.DevuelveCodigo(this.radButtonTextBoxCtaMayor.Text.Trim());
                    string validarCompania = this.ValidarCuentaMayor(this.codigoCtaMayor);
                    if (validarCompania != "")
                    {
                        errores += "- " + validarCompania + "\n\r";
                        this.ActiveControl = this.radButtonTextBoxCtaMayor;
                        this.radButtonTextBoxCtaMayor.Select();
                        this.radButtonTextBoxCtaMayor.Focus();
                        tabGeneralError = true;
                    }
                } else this.codigoCtaMayor = "";

                //-------------- Validar el tipo de auxiliar ------------------
                if (this.radButtonTextBoxTipoAux.Text.Trim() != "")
                {
                    this.codigoTipoAux = this.DevuelveCodigo(this.radButtonTextBoxTipoAux.Text.Trim());
                    string validarTipoAux = this.ValidarTipoAux(this.codigoTipoAux);
                    if (validarTipoAux != "")
                    {
                        errores += "- " + validarTipoAux + "\n\r";
                        this.ActiveControl = this.radButtonTextBoxTipoAux;
                        this.radButtonTextBoxTipoAux.Select();
                        this.radButtonTextBoxTipoAux.Focus();
                        tabGeneralError = true;
                    }
                } else this.codigoTipoAux = "";

                //-------------- Validar el libro ------------------
                if (this.txtLibro.Text.Trim() == "")
                {
                    errores += "- El libro no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtLibro;
                    this.txtLibro.Select();
                    this.txtLibro.Focus();
                    tabGeneralError = true;
                }

                //-------------- Validar la serie ------------------
                if (this.txtSerie.Text.Trim() == "")
                {
                    errores += "- La serie no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtSerie;
                    this.txtSerie.Select();
                    this.txtSerie.Focus();
                    tabGeneralError = true;
                }

                //-------------- Validar cuadre de cuotas  ------------------
                if (this.radToggleSwitchCuadreCuotas.Value && this.txtMaxDescuadreCuotas.Text == "")
                {
                    errores += "- El máximo descuadre de cuotas no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtMaxDescuadreCuotas;
                    this.txtSerie.Select();
                    this.txtSerie.Focus();
                    tabGeneralError = true;
                }

                //-------------- Validar contrapartida (cuenta mayor o código de IVA)  ------------------
                string ctaMayorContrap = this.radButtonTextBoxCtaMayorContrap.Text.Trim();
                string codIVAContrap = this.radButtonTextBoxCodIVAContrap.Text.Trim();
                if (ctaMayorContrap == "" && codigoIVACont == "")
                {
                    this.codigoCtaMayorCont = "";
                    this.codigoIVACont = "";
                }
                else
                if (ctaMayorContrap != "" && codIVAContrap != "")
                {
                    errores += "- La cuenta de mayor de contrapartida y el código de IVA de contrapartida están informados. Sólo se puede indicar uno de los dos \n\r";      //Falta traducir
                    this.ActiveControl = this.radButtonTextBoxCtaMayorContrap;
                    this.radButtonTextBoxCtaMayorContrap.Select();
                    this.radButtonTextBoxCtaMayorContrap.Focus();
                    tabGeneralError = true;
                }
                else
                {
                    if(ctaMayorContrap != "" && ctaMayorContrap.Substring(0, 1) == "+" && codIVAContrap == ""  )
                    {
                        codIVAContrap = ctaMayorContrap.Substring(1, 2);
                        ctaMayorContrap = "";
                    }
                    
                    if (ctaMayorContrap != "")
                    {
                        this.codigoIVACont = "";
                        this.codigoCtaMayorCont = this.DevuelveCodigo(ctaMayorContrap);
                        string validarCtaMayorCont = this.ValidarCuentaMayorContrap(this.codigoCtaMayorCont);
                        if (validarCtaMayorCont != "")
                        {
                            errores += "- " + validarCtaMayorCont + "\n\r";
                            this.ActiveControl = this.radButtonTextBoxCtaMayorContrap;
                            this.radButtonTextBoxCtaMayorContrap.Select();
                            this.radButtonTextBoxCtaMayorContrap.Focus();
                            tabGeneralError = true;
                        }
                    }
                    else
                    {
                        if (codIVAContrap != "")
                        {
                            this.codigoCtaMayorCont = "";
                            this.codigoIVACont = this.DevuelveCodigo(codIVAContrap);
                            string validarCodIVACont = this.ValidarCodigoIVAContrap(this.codigoIVACont);
                            if (validarCodIVACont != "")
                            {
                                errores += "- " + validarCodIVACont + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCodIVAContrap;
                                this.radButtonTextBoxCodIVAContrap.Select();
                                this.radButtonTextBoxCodIVAContrap.Focus();
                                tabGeneralError = true;
                            }
                        }
                    }
                }

                //-------------- Validar código IVA asociado  ------------------
                if (this.radButtonTextBoxCodIVAAsociado.Text.Trim() != "")
                {
                    this.codigoIVAAsociado = this.DevuelveCodigo(this.radButtonTextBoxCodIVAAsociado.Text.Trim());
                    string validarCodIVAAsociado = this.ValidarCodigoIVAAsociado(this.codigoIVAAsociado);
                    if (validarCodIVAAsociado != "")
                    {
                        errores += "- " + validarCodIVAAsociado + "\n\r";
                        this.ActiveControl = this.radButtonTextBoxCodIVAAsociado;
                        this.radButtonTextBoxCodIVAAsociado.Select();
                        this.radButtonTextBoxCodIVAAsociado.Focus();
                        tabGeneralError = true;
                    }
                } else this.codigoIVAAsociado = "";

                //Validar Tipos IVA
                errores += this.ValidarTiposIVA();
                if (errores != "") tabGeneralError = true;

                if (this.ClavesLegalesCambio())
                {
                    //Activar la pestaña de Claves Legales
                    this.radPageViewDatos.SelectedPage = this.radPageViewPageClavesLegales;

                    //Si existen cambios en la pestaña de Claves Legales, validar los campos

                    try
                    {
                        //-------------- Validar Modelo 303 Grupo de Operaciones ------------------
                        if (this.radButtonTextBoxCLMod303GrupoOperac.Text.Trim() != "")
                        {
                            this.codigoCLMod303GrupoOperac = this.DevuelveCodigo(this.radButtonTextBoxCLMod303GrupoOperac.Text);
                            string validarCLMod303GrupoOperac = this.ValidarCLMod303GrupoOperac(this.codigoCLMod303GrupoOperac);
                            if (validarCLMod303GrupoOperac != "")
                            {
                                errores += "- " + validarCLMod303GrupoOperac + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod303GrupoOperac;
                                this.radButtonTextBoxCLMod303GrupoOperac.Select();
                                this.radButtonTextBoxCLMod303GrupoOperac.Focus();
                                tabClavesLegalesError = true;
                            }
                        } else this.codigoCLMod303GrupoOperac = "";

                        //-------------- Validar Modelo 390 Grupo de Operaciones ------------------
                        if (this.radButtonTextBoxCLMod390GrupoOperac.Text.Trim() != "")
                        {
                            this.codigoCLMod390GrupoOperac = this.DevuelveCodigo(this.radButtonTextBoxCLMod390GrupoOperac.Text);
                            string validarCLMod390GrupoOperac = this.ValidarCLMod390GrupoOperac(this.codigoCLMod390GrupoOperac);
                            if (validarCLMod390GrupoOperac != "")
                            {
                                errores += "- " + validarCLMod390GrupoOperac + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod390GrupoOperac;
                                this.radButtonTextBoxCLMod390GrupoOperac.Select();
                                this.radButtonTextBoxCLMod390GrupoOperac.Focus();
                                tabClavesLegalesError = true;
                            }
                        } else this.codigoCLMod390GrupoOperac = "";
                        
                        //-------------- Validar Modelo 349 Libro Registro ------------------
                        if (this.radButtonTextBoxCLMod340LibroRegistro.Text.Trim() != "")
                        {
                            this.codigoCLMod340LibroRegistro = this.DevuelveCodigo(this.radButtonTextBoxCLMod340LibroRegistro.Text);
                            string validarCLMod340LibroRegistro = this.ValidarCLMod340LibroRegistro(this.codigoCLMod340LibroRegistro);
                            if (validarCLMod340LibroRegistro != "")
                            {
                                errores += "- " + validarCLMod340LibroRegistro + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod340LibroRegistro;
                                this.radButtonTextBoxCLMod340LibroRegistro.Select();
                                this.radButtonTextBoxCLMod340LibroRegistro.Focus();
                                tabClavesLegalesError = true;
                            }
                        } else this.codigoCLMod340LibroRegistro = "";

                        //-------------- Validar Modelo 340 Clave operación ------------------
                        if (this.radButtonTextBoxCLMod340ClaveOperac.Text.Trim() != "")
                        {
                            this.codigoCLMod340ClaveOperac = this.DevuelveCodigo(this.radButtonTextBoxCLMod340ClaveOperac.Text);
                            string validarCLMod340ClaveOperac = this.ValidarCLMod340ClaveOperac(this.codigoCLMod340ClaveOperac);
                            if (validarCLMod340ClaveOperac != "")
                            {
                                errores += "- " + validarCLMod340ClaveOperac + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod340ClaveOperac;
                                this.radButtonTextBoxCLMod340ClaveOperac.Select();
                                this.radButtonTextBoxCLMod340ClaveOperac.Focus();
                                tabClavesLegalesError = true;
                            }
                        }
                        else this.codigoCLMod340ClaveOperac = "";

                        //-------------- Validar Modelo 349 Tipo operación ------------------
                        if (this.radButtonTextBoxlCLMod349TipoOperac.Text.Trim() != "")
                        {
                            this.codigoCLMod349TipoOperac = this.DevuelveCodigo(this.radButtonTextBoxlCLMod349TipoOperac.Text);
                            string validarCLMod349TipoOperac = this.ValidarCLMod349TipoOperac(this.codigoCLMod349TipoOperac);
                            if (validarCLMod349TipoOperac != "")
                            {
                                errores += "- " + validarCLMod349TipoOperac + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxlCLMod349TipoOperac;
                                this.radButtonTextBoxlCLMod349TipoOperac.Select();
                                this.radButtonTextBoxlCLMod349TipoOperac.Focus();
                                tabClavesLegalesError = true;
                            }
                        }
                        else this.codigoCLMod349TipoOperac = "";

                        //-------------- Validar Modelo 190 Clave percepción ------------------
                        if (this.radButtonTextBoxCLMod190ClavePercep.Text.Trim() != "")
                        {
                            this.codigoCLMod190ClavePercep = this.DevuelveCodigo(this.radButtonTextBoxCLMod190ClavePercep.Text);
                            string validarCLMod190ClavePercep = this.ValidarCLMod190ClavePercep(this.codigoCLMod190ClavePercep);
                            if (validarCLMod190ClavePercep != "")
                            {
                                errores += "- " + validarCLMod190ClavePercep + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod190ClavePercep;
                                this.radButtonTextBoxCLMod190ClavePercep.Select();
                                this.radButtonTextBoxCLMod190ClavePercep.Focus();
                                tabClavesLegalesError = true;
                            }
                        }
                        else this.codigoCLMod190ClavePercep = "";

                        //-------------- Validar Modelo 190 Clave percepción ------------------
                        if (this.radButtonTextBoxCLMod111ClavePercep.Text.Trim() != "")
                        {
                            this.codigoCLMod111ClavePercep = this.DevuelveCodigo(this.radButtonTextBoxCLMod111ClavePercep.Text);
                            string validarCLMod111ClavePercep = this.ValidarCLMod111ClavePercep(this.codigoCLMod111ClavePercep);
                            if (validarCLMod111ClavePercep != "")
                            {
                                errores += "- " + validarCLMod111ClavePercep + "\n\r";
                                this.ActiveControl = this.radButtonTextBoxCLMod111ClavePercep;
                                this.radButtonTextBoxCLMod111ClavePercep.Select();
                                this.radButtonTextBoxCLMod111ClavePercep.Focus();
                                tabClavesLegalesError = true;
                            }
                        }
                        else this.codigoCLMod111ClavePercep = "";
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                if (SiiLibroCambio())
                {
                    //Activar la pestaña de Claves Legales
                    this.radPageViewDatos.SelectedPage = this.radPageViewPageSIILibros;

                    //Si existen cambios en la pestaña de Claves Legales, validar los campos
                    string codigoSIILibro = this.LibroSIIActivoCodigo();
                    if (codigoSIILibro != "")
                    try
                    {
                            string desc = "";
                            switch (codigoSIILibro)
                            {
                                case "S":
                                    if (this.radButtonTextBoxSiiTipoOpSeguro.Text.Trim() != "")
                                    {
                                        this.sii_opSeg_tipoOperacion = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoOpSeguro.Text);
                                        string validarSiiTipoOpSeg = this.ValidarCodigoDesc(this.sii_opSeg_tipoOperacion, "X", ref desc);

                                        if (validarSiiTipoOpSeg != "")
                                        {
                                            errores += "- SII Libro Operacion Seguro campo Tipo operación seguro: " + validarSiiTipoOpSeg + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoOpSeguro;
                                            this.radButtonTextBoxSiiTipoOpSeguro.Select();
                                            this.radButtonTextBoxSiiTipoOpSeguro.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_opSeg_tipoOperacion = "";

                                    if (this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text.Trim() != "")
                                    {
                                        this.sii_agenciaTributaria = this.DevuelveCodigo(this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text);
                                        string validarSiiAgenciaTributaria = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref desc);
                                        if (validarSiiAgenciaTributaria != "")
                                        {
                                            errores += "- - SII Libro Operacion Seguro campo Agencia Tributaria: " + validarSiiAgenciaTributaria + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiAgenciaTributariaOpSeguro;
                                            this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Select();
                                            this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_agenciaTributaria = "";

                                    break;
                                case "U":
                                    if (this.radButtonTextBoxSiiTipoOpIntracom.Text.Trim() != "")
                                    {
                                        this.sii_opIntracom_tipoOperacion = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoOpIntracom.Text);
                                        string validarSiiTipoOpIntracom = this.ValidarCodigoDesc(this.sii_opIntracom_tipoOperacion, "D", ref desc);

                                        if (validarSiiTipoOpIntracom != "")
                                        {
                                            errores += "- SII Libro Operacion Intracomunitaria campo Tipo operación intracomunitaria: " + validarSiiTipoOpIntracom + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoOpIntracom;
                                            this.radButtonTextBoxSiiTipoOpIntracom.Select();
                                            this.radButtonTextBoxSiiTipoOpIntracom.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_opIntracom_tipoOperacion = "";

                                    if (this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text.Trim() != "")
                                    {
                                        this.sii_agenciaTributaria = this.DevuelveCodigo(this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text);
                                        string validarSiiAgenciaTributaria = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref desc);
                                        if (validarSiiAgenciaTributaria != "")
                                        {
                                            errores += "- - SII Libro Operacion Intracomunitaria campo Agencia Tributaria: " + validarSiiAgenciaTributaria + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoOpIntracom;
                                            this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Select();
                                            this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_agenciaTributaria = "";

                                    break;
                                case "E":
                                    if (this.radButtonTextBoxSiiTipoFactura.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_tipoFactura = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoFactura.Text);
                                        string validarSiiTipoFactura = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref desc);

                                        if (validarSiiTipoFactura != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Tipo factura: " + validarSiiTipoFactura + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoFactura;
                                            this.radButtonTextBoxSiiTipoFactura.Select();
                                            this.radButtonTextBoxSiiTipoFactura.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_tipoFactura = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrasc.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrasc = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrasc.Text);
                                        string validarSiiClaveRegEspeTrasc = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "E", ref desc);

                                        if (validarSiiClaveRegEspeTrasc != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Clave régimen especial o trascendencia: " + validarSiiClaveRegEspeTrasc + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrasc;
                                            this.radButtonTextBoxSiiClaveRegEspTrasc.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrasc.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrasc = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrascAdicional1 = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text);
                                        string validarSiiClaveRegEspeTrascAdicional1 = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "E", ref desc);

                                        if (validarSiiClaveRegEspeTrascAdicional1 != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Clave régimen especial o trascendencia adicional 1: " + validarSiiClaveRegEspeTrascAdicional1 + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrascAdicional1 = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text.Trim() != "")
                                    {
                                        string validarSiiClaveRegEspeTrascAdicional01 = this.ValidarCodigoDescFactEnt1(this.sii_factE_R_claveRegEspeTrasc, this.sii_factE_R_claveRegEspeTrascAdicional1);
                                        if (validarSiiClaveRegEspeTrascAdicional01 != "")
                                        {
                                            errores += validarSiiClaveRegEspeTrascAdicional01 + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrascAdicional2 = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text);
                                        string validarSiiClaveRegEspeTrascAdicional2 = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "E", ref desc);

                                        if (validarSiiClaveRegEspeTrascAdicional2 != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Clave régimen especial o trascendencia adicional 2: " + validarSiiClaveRegEspeTrascAdicional2 + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrascAdicional2 = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text.Trim() != "")
                                    {
                                        string validarSiiClaveRegEspeTrascAdicional02 = this.ValidarCodigoDescFactEnt2(this.sii_factE_R_claveRegEspeTrasc, this.sii_factE_R_claveRegEspeTrascAdicional1, this.sii_factE_R_claveRegEspeTrascAdicional2);
                                        if (validarSiiClaveRegEspeTrascAdicional02 != "")
                                        {
                                            errores += validarSiiClaveRegEspeTrascAdicional02 + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }

                                    if (this.radButtonTextBoxSiiTipoFacturaRectif.Text.Trim() != "")
                                        if (this.sii_factE_R_tipoFactura != "R1" && this.sii_factE_R_tipoFactura != "R2" &&
                                            this.sii_factE_R_tipoFactura != "R3" && this.sii_factE_R_tipoFactura != "R4" &&
                                            this.sii_factE_R_tipoFactura != "R5")
                                        {
                                            errores += "- SII Libro Factura Expedida: Campo Tipo Factura rectificativa no es compatible con tipo factura " + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoFacturaRectif;
                                            this.radButtonTextBoxSiiTipoFacturaRectif.Select();
                                            this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
                                            tabSiiLibroError = true;
                                        }
                                        else
                                        {
                                            this.sii_factE_R_tipoFacturaRectif = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoFacturaRectif.Text);
                                            string validarSiiTipoFacturaRectif = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref desc);

                                            if (validarSiiTipoFacturaRectif != "")
                                            {
                                                errores += "- SII Libro Factura Expedida campo Factura rectificativa: " + validarSiiTipoFacturaRectif + "\n\r";
                                                this.ActiveControl = this.radButtonTextBoxSiiTipoFacturaRectif;
                                                this.radButtonTextBoxSiiTipoFacturaRectif.Select();
                                                this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
                                                tabSiiLibroError = true;
                                            }
                                        }
                                    else
                                        if (this.sii_factE_R_tipoFactura == "R1" || this.sii_factE_R_tipoFactura == "R2" ||
                                            this.sii_factE_R_tipoFactura == "R3" || this.sii_factE_R_tipoFactura == "R4" ||
                                            this.sii_factE_R_tipoFactura == "R5")
                                    {
                                        errores += "- SII Libro Factura Expedida: Campo Tipo factura rectificativa debe de informarse para el tipo factura Rx" + "\n\r";
                                        this.ActiveControl = this.radButtonTextBoxSiiTipoFacturaRectif;
                                        this.radButtonTextBoxSiiTipoFacturaRectif.Select();
                                        this.radButtonTextBoxSiiTipoFacturaRectif.Focus();
                                        tabSiiLibroError = true;
                                    }
                                    else
                                        this.sii_factE_R_tipoFacturaRectif = "";

                                    //Validar Tipo Desglose !!!!!! FALTA !!!!
                                    if (this.radDropDownListSiiTipoDesglose.SelectedIndex == 0) sii_factE_R_tipoDesglose = "F";
                                    if (this.radDropDownListSiiTipoDesglose.SelectedIndex == 1) sii_factE_R_tipoDesglose = "E";
                                    if (this.radDropDownListSiiTipoDesglose.SelectedIndex == 2) sii_factE_R_tipoDesglose = "S";

                                    if (this.radButtonTextBoxSiiTipoOperSujNoExe.Text.Trim() != "")
                                    {
                                        this.sii_factE_tipoOperacionSujetaNoExenta = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoOperSujNoExe.Text);
                                        string validarSiiTipoOperacionSujetaNoExenta = this.ValidarCodigoDesc(this.sii_factE_tipoOperacionSujetaNoExenta, "H", ref desc);

                                        if (validarSiiTipoOperacionSujetaNoExenta != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Tipo operación sujeta no exenta: " + validarSiiTipoOperacionSujetaNoExenta + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoOperSujNoExe;
                                            this.radButtonTextBoxSiiTipoOperSujNoExe.Select();
                                            this.radButtonTextBoxSiiTipoOperSujNoExe.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_tipoOperacionSujetaNoExenta = "";

                                    if (this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text.Trim() != "")
                                    {
                                        this.sii_factE_causaExencionOpSujetasExentas = this.DevuelveCodigo(this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text);
                                        string validarSiiCausaExencionOpSujetasExentas = this.ValidarCodigoDesc(this.sii_factE_causaExencionOpSujetasExentas, "B", ref desc);

                                        if (validarSiiCausaExencionOpSujetasExentas != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Causa exención operaciones sujetas y exentas: " + validarSiiCausaExencionOpSujetasExentas + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiCausaExeOperSujetaYExenta;
                                            this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Select();
                                            this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_causaExencionOpSujetasExentas = "";

                                    if (this.radButtonTextBoxSiiTipoOperacNoSujeta.Text.Trim() != "")
                                    {
                                        this.sii_factE_tipoOperacNoSujeta = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoOperacNoSujeta.Text);
                                        string validarSiiTipoOperacNoSujeta = this.ValidarCodigoDesc(this.sii_factE_tipoOperacNoSujeta, "K", ref desc);

                                        if (validarSiiTipoOperacNoSujeta != "")
                                        {
                                            errores += "- SII Libro Factura Expedida campo Tipo operación no sujeta: " + validarSiiTipoOperacNoSujeta + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoOperacNoSujeta;
                                            this.radButtonTextBoxSiiTipoOperacNoSujeta.Select();
                                            this.radButtonTextBoxSiiTipoOperacNoSujeta.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_tipoOperacNoSujeta = "";

                                    if (this.radButtonTextBoxSiiAgenciaTributaria.Text.Trim() != "")
                                    {
                                        this.sii_agenciaTributaria = this.DevuelveCodigo(this.radButtonTextBoxSiiAgenciaTributaria.Text);
                                        string validarSiiAgenciaTributaria = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref desc);
                                        if (validarSiiAgenciaTributaria != "")
                                        {
                                            errores += "- - SII Libro Factura Expedida campo Agencia Tributaria: " + validarSiiAgenciaTributaria + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiAgenciaTributaria;
                                            this.radButtonTextBoxSiiAgenciaTributaria.Select();
                                            this.radButtonTextBoxSiiAgenciaTributaria.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_agenciaTributaria = "";

                                    //Falta validar que solo puede informar el campo Tipo operacion sujeta no exenta o
                                    //el campo causa exencion operacion sujeta y exenta o
                                    //tipo de operacion no sujeta
                                    //Los 3 a la vez no se pueden informar -> ERROR   !!!!! Falta
                                    if (this.sii_factE_tipoOperacionSujetaNoExenta != "" && this.sii_factE_causaExencionOpSujetasExentas != "" ||
                                        this.sii_factE_tipoOperacionSujetaNoExenta != "" && this.sii_factE_tipoOperacNoSujeta != "" ||
                                        this.sii_factE_causaExencionOpSujetasExentas != "" && this.sii_factE_tipoOperacNoSujeta != "")
                                    {
                                        errores += "- SII Libro Factura Expedida campos Tipo operación sujeta exenta/no exenta y no sujeta, solo se puede informar un campo "
                                            + "\n\r";
                                        this.ActiveControl = this.radButtonTextBoxSiiTipoOperSujNoExe;
                                        this.radButtonTextBoxSiiTipoOperSujNoExe.Select();
                                        this.radButtonTextBoxSiiTipoOperSujNoExe.Focus();
                                        tabSiiLibroError = true;
                                    }
                                    if (this.radToggleSwitchSiiCupon.Value == true &&
                                        (this.sii_factE_R_tipoFactura != "R5" && this.sii_factE_R_tipoFactura != "F4" && this.sii_factE_R_tipoFactura != "R1")
                                        )
                                    {
                                        errores += "- SII Libro Factura Expedida valor campo Cupón no compatible con tipo factura " + "\n\r";
                                        tabSiiLibroError = true;
                                    }
                                    if (this.radToggleSwitchSiiTaxFree.Value == true &&
                                        (this.sii_factE_R_claveRegEspeTrasc != "02" || this.sii_factE_causaExencionOpSujetasExentas != "E2")
                                        )
                                    {
                                        errores += "- SII Libro Factura Expedida valor campo Tax Free no compatible con Clave Reg.Esp.Transc.Pral y Causa Exección " + "\n\r";
                                        tabSiiLibroError = true;
                                    }
                                    if (this.radToggleSwitchSiiFactSimpArt72_73.Value == true &&
                                        (this.sii_factE_R_tipoFactura != "F1" && this.sii_factE_R_tipoFactura != "F3" && this.sii_factE_R_tipoFactura != "R1" &&
                                         this.sii_factE_R_tipoFactura != "R2" && this.sii_factE_R_tipoFactura != "R3" && this.sii_factE_R_tipoFactura != "R4" &&
                                            this.sii_factE_R_tipoFactura != "R5")
                                        )
                                    {
                                        errores += "- SII Libro Factura expedida: valor campo Factura simplificada artículo 7.2/7.3 no compatible con tipo factura " + "\n\r";
                                        tabSiiLibroError = true;
                                    }
                                    break;
                                case "R":
                                    if (this.radButtonTextBoxlSiiTipoFacturaRecibida.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_tipoFactura = this.DevuelveCodigo(this.radButtonTextBoxlSiiTipoFacturaRecibida.Text);
                                        string validarSiiTipoFacturaRecibida = this.ValidarCodigoDesc(this.sii_factE_R_tipoFactura, "U", ref desc);

                                        if (validarSiiTipoFacturaRecibida != "")
                                        {
                                            errores += "- SII Libro Factura Recibida campo Tipo factura: " + validarSiiTipoFacturaRecibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxlSiiTipoFacturaRecibida;
                                            this.radButtonTextBoxlSiiTipoFacturaRecibida.Select();
                                            this.radButtonTextBoxlSiiTipoFacturaRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_tipoFactura = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrasc = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text);
                                        string validarSiiClaveRegEspeTrascrecibida = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrasc, "E", ref desc);

                                        if (validarSiiClaveRegEspeTrascrecibida != "")
                                        {
                                            errores += "- SII Libro Factura Recibida campo Clave régimen especial o trascendencia: " + validarSiiClaveRegEspeTrascrecibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida;
                                            this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrasc = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrascAdicional1 = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text);
                                        string validarSiiClaveRegEspeTrascAdicional1Recibida = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional1, "E", ref desc);

                                        if (validarSiiClaveRegEspeTrascAdicional1Recibida != "")
                                        {
                                            errores += "- SII Libro Factura Recibida campo Clave régimen especial o trascendencia adicional 1: " + validarSiiClaveRegEspeTrascAdicional1Recibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrascAdicional1 = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text.Trim() != "")
                                    {
                                        string validarSiiClaveRegEspeTrascAdicional01Recibida = this.ValidarCodigoDescFactRec1(this.sii_factE_R_claveRegEspeTrasc, this.sii_factE_R_claveRegEspeTrascAdicional1);
                                        if (validarSiiClaveRegEspeTrascAdicional01Recibida != "")
                                        {
                                            errores += validarSiiClaveRegEspeTrascAdicional01Recibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text.Trim() != "")
                                    {
                                        this.sii_factE_R_claveRegEspeTrascAdicional2 = this.DevuelveCodigo(this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text);
                                        string radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida = this.ValidarCodigoDesc(this.sii_factE_R_claveRegEspeTrascAdicional2, "E", ref desc);

                                        if (radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida != "")
                                        {
                                            errores += "- SII Libro Factura Recibida campo Clave régimen especial o trascendencia adicional 2: " + radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_factE_R_claveRegEspeTrascAdicional2 = "";

                                    if (this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text.Trim() != "")
                                    {
                                        string validarSiiClaveRegEspeTrascAdicional02Recibida = this.ValidarCodigoDescFactRec2(this.sii_factE_R_claveRegEspeTrasc, this.sii_factE_R_claveRegEspeTrascAdicional1,
                                            this.sii_factE_R_claveRegEspeTrascAdicional2);
                                        if (validarSiiClaveRegEspeTrascAdicional02Recibida != "")
                                        {
                                            errores += validarSiiClaveRegEspeTrascAdicional02Recibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida;
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Select();
                                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }


                                    if (this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text.Trim() != "")
                                        if (this.sii_factE_R_tipoFactura != "R1" && this.sii_factE_R_tipoFactura != "R2" &&
                                            this.sii_factE_R_tipoFactura != "R3" && this.sii_factE_R_tipoFactura != "R4" &&
                                            this.sii_factE_R_tipoFactura != "R5")
                                        {
                                            errores += "- SII Libro Factura Recibida: Campo Tipo Factura rectificativa no es compatible con tipo factura " + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiTipoFactRectifFactRecibida;
                                            this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Select();
                                            this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                        else
                                        {
                                            this.sii_factE_R_tipoFacturaRectif = this.DevuelveCodigo(this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text);
                                            string validarSiiTipoFacturaRectifRecibida = this.ValidarCodigoDesc(this.sii_factE_R_tipoFacturaRectif, "G", ref desc);

                                            if (validarSiiTipoFacturaRectifRecibida != "")
                                            {
                                                errores += "- SII Libro Factura Recibida campo Factura rectificativa: " + validarSiiTipoFacturaRectifRecibida + "\n\r";
                                                this.ActiveControl = this.radButtonTextBoxSiiTipoFactRectifFactRecibida;
                                                this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Select();
                                                this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Focus();
                                                tabSiiLibroError = true;
                                            }
                                        }
                                    else
                                        if (this.sii_factE_R_tipoFactura == "R1" || this.sii_factE_R_tipoFactura == "R2" ||
                                            this.sii_factE_R_tipoFactura == "R3" || this.sii_factE_R_tipoFactura == "R4" ||
                                            this.sii_factE_R_tipoFactura == "R5")
                                    {
                                        errores += "- SII Libro Factura Recibida: Campo Tipo factura rectificativa debe de informarse para el tipo factura Rx" + "\n\r";
                                        this.ActiveControl = this.radButtonTextBoxSiiTipoFactRectifFactRecibida;
                                        this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Select();
                                        this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Focus();
                                        tabSiiLibroError = true;
                                    }
                                    else this.sii_factE_R_tipoFacturaRectif = "";

                                    //Validar Tipo Desglose !!!!!! FALTA !!!!
                                    if (this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex == 0) sii_factE_R_tipoDesglose = "I";
                                    if (this.radDropDownListSiiTipoDesgloseFactRecibida.SelectedIndex == 1) sii_factE_R_tipoDesglose = "P";
                                    //Validar el campo decimal

                                    if (this.radToggleSwitchSiiProrrataFactRecibida.Value != true)
                                    { 

                                        if (this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value == true)
                                        {
                                            errores += "- SII Libro Factura Recibida: Campo Excluir Base en total factura (Prorrata), erróneo" + "\n\r";
                                            this.ActiveControl = this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida;
                                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Select();
                                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                        Decimal valor = Convert.ToDecimal(txtSiiTipoImpositivoProrrataFactRecibida.Text);
                                        if (valor> 0)
                                        {
                                            errores += "- SII Libro Factura Recibida: Tipo Impositivo (Prorrata), erróneo" + "\n\r";
                                            this.ActiveControl = this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida;
                                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Select();
                                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }

                                    if (this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text.Trim() != "")
                                    {
                                        this.sii_agenciaTributaria = this.DevuelveCodigo(this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text);
                                        string validarSiiAgenciaTributariaFactRecibida = this.ValidarCodigoDesc(this.sii_agenciaTributaria, "6", ref desc);
                                        if (validarSiiAgenciaTributariaFactRecibida != "")
                                        {
                                            errores += "- - SII Libro Factura Recibida campo Agencia Tributaria: " + validarSiiAgenciaTributariaFactRecibida + "\n\r";
                                            this.ActiveControl = this.radButtonTextBoxSiiAgenciaTributariaFactRecibida;
                                            this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Select();
                                            this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Focus();
                                            tabSiiLibroError = true;
                                        }
                                    }
                                    else this.sii_agenciaTributaria = "";

                                    if (this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value == true &&
                                        (this.sii_factE_R_tipoFactura != "F1" && this.sii_factE_R_tipoFactura != "F3" && this.sii_factE_R_tipoFactura != "R1" &&
                                         this.sii_factE_R_tipoFactura != "R2" && this.sii_factE_R_tipoFactura != "R3" && this.sii_factE_R_tipoFactura != "R4" &&
                                            this.sii_factE_R_tipoFactura != "R5")
                                        )
                                    {
                                        errores += "- SII Libro Factura Recibidas: valor campo Factura simplificada artículo 7.2/7.3 no compatible con tipo factura " + "\n\r";
                                        tabSiiLibroError = true;
                                    }

                                    break;
                            }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "")
            {
                if (tabGeneralError) this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;
                else if (tabClavesLegalesError) this.radPageViewDatos.SelectedPage = this.radPageViewPageClavesLegales;
                     else if (tabSiiLibroError) this.radPageViewDatos.SelectedPage = this.radPageViewPageSIILibros;

                RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));
            }
            else this.radPageViewDatos.SelectedPage = pageActiva;

            return (result);
        }

        /// <summary>
        /// Valida la Grid de Tipos de IVA
        /// </summary>
        /// <returns></returns>
        private string ValidarTiposIVA()
        {
            string result = "";
            bool existeFila = false;
            try
            {
                string fechaEfectiva = "";
                string porcIVA = "";
                //string porcRecEquiv = "";
                string descripcion = "";
                string errorFila = "";
                string mensaje = "";

                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    existeFila = true;
                    errorFila = "";

                    bool validarFilasTiposIVA = false;
                    if (this.nuevo)
                    {
                        bool todaFilaBlancoAux = true;

                        for (int j = 0; j < this.radGridViewTiposIVA.Columns.Count; j++)
                        {
                            if (this.radGridViewTiposIVA.Rows[i].Cells[j].ToString() != "")
                            {
                                todaFilaBlancoAux = false;
                                break;
                            }
                        }

                        validarFilasTiposIVA = todaFilaBlancoAux ? false : true;
                    }
                    //SMR else validarFilasTiposIVA = TodaFilaEnBlanco(this.dtTiposIVAGrid, i) ? false : true;
                    else validarFilasTiposIVA = TodaFilaEnBlanco(this.radGridViewTiposIVA, i) ? false : true;

                    if (validarFilasTiposIVA)
                    {
                        fechaEfectiva = this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value.ToString();
                        fechaEfectiva = fechaEfectiva.Replace("/", "");

                        if (fechaEfectiva.Trim() == "")
                        {
                            errorFila = " - Tipos de IVA Fila " + (i + 1) + ": Fecha Efectiva no puede estar en blanco.";      //Falta traducir
                            this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                            this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["FechaEfectiva"];
                        }
                        else
                        {
                            try
                            {
                                DateTime dt = Convert.ToDateTime(this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value);
                            }
                            catch
                            {
                                errorFila = " - Tipos de IVA Fila " + (i + 1) + ": Fecha Efectiva no tiene un formato correcto.";
                                this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                                this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["FechaEfectiva"];
                            }
                        }

                        /*
                        porcIVA = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString();
                        if (porcIVA.Trim() == "")
                        {
                            mensaje = " Porcentaje de IVA no puede estar en blanco. ";
                            if (errorFila == "") errorFila += " - Tipos de IVA Fila " + (i + 1) + ": " + mensaje + ".";      //Falta traducir
                            else errorFila += mensaje;
                            this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                            this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["PorcIVA"];
                        }
                        */
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value == null)
                        {
                            mensaje = " Porcentaje de IVA no puede estar en blanco. ";
                            if (errorFila == "") errorFila += " - Tipos de IVA Fila " + (i + 1) + ": " + mensaje + ".";      //Falta traducir
                            else errorFila += mensaje;
                            this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                            this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["PorcIVA"];
                        }

                        /*
                        porcRecEquiv = this.dgTipoIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString();
                        if (porcRecEquiv.Trim() == "")
                        {
                            mensaje = " Porcentaje de recargo de equivalencia no puede estar en blanco.";
                            if (errorFila == "") errorFila += " - Tipos de IVA Fila " + (i + 1) + ": " + mensaje;      //Falta traducir
                            else errorFila += mensaje;
                            this.dgTipoIVA.CurrentCell = this.dgTipoIVA.Rows[i].Cells["PorcRecEquiv"];
                        }
                        */
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value == null) this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value = 0;
                        // jlg 30/06 if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value == null)
                        // jlg 30/06{
                        // jlg 30/06mensaje = " Porcentaje de recargo de equivalencia no puede estar en blanco.";
                        // jlg 30/06if (errorFila == "") errorFila += " - Tipos de IVA Fila " + (i + 1) + ": " + mensaje;      //Falta traducir
                        // jlg 30/06else errorFila += mensaje;
                        // jlg 30/06this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                        // jlg 30/06this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["PorcRecEquiv"];
                        // jlg 30/06}

                        if (this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value !=null)
                        {
                            descripcion = this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value.ToString();
                        }
                        
                        //if (descripcion == "")
                        else
                        {
                            mensaje = " Descripción no puede estar en blanco.";
                            if (errorFila == "") errorFila += " - Tipos de IVA Fila " + (i + 1) + ": " + mensaje;      //Falta traducir
                            else errorFila += mensaje;
                            this.radGridViewTiposIVA.CurrentRow = this.radGridViewTiposIVA.Rows[i];
                            this.radGridViewTiposIVA.CurrentColumn = this.radGridViewTiposIVA.Columns["Descripcion"];
                        }

                        if (errorFila != "")
                        {
                            errorFila += " \n\r";
                            result += errorFila;
                            this.radGridViewTiposIVA.Rows[i].IsSelected = true;
                        }
                    }
                }

                if (!existeFila) result = "- El Tipo de IVA no puede estar en blanco \n\r";      //Falta traducir
                else if (errorFila != "") { result = errorFila; }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            
            return (result);
        }

        /// <summary>
        /// Devuelve si todas las columnas están vacías dado una fila de un DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="row">fila</param>
        /// <returns></returns>
        //SMR private bool TodaFilaEnBlanco(DataTable table, int row)
        private bool TodaFilaEnBlanco(RadGridView table, int row)
        {
            bool todaFilaBlanco = true;

            if (this.nuevo)
            {
                for (int j = 0; j < this.radGridViewTiposIVA.Columns.Count; j++)
                {
                    if (this.radGridViewTiposIVA.Rows[row].Cells[j].ToString() != "")
                    {
                        todaFilaBlanco = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    //SMR if (table.Rows[row][i].ToString() != "")
                    if (table.Rows[row].Cells[i].ToString() != "")
                    {
                        todaFilaBlanco = false;
                        break;
                    }
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// Devuelve el código para los controles tgTextBoxSel que almacenan el texto en el formato codigo - descripcion
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private string DevuelveCodigo(string valor)
        {
            string result = "";
            try
            {
                int indexSep = valor.IndexOf(separadorDesc);
                if (indexSep == -1) result = valor;
                else
                {
                    switch (indexSep)
                    {
                        case 0:
                            result = "";
                            break;
                        case 1:
                            result = valor.Substring(0, 1);
                            break;
                        default:
                            result = valor.Substring(0, indexSep-1);
                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Grabar un código de IVA
        /// </summary>
        private void Grabar()
        {
            bGrabar = false;
            bEliminar = false;
            if (this.FormValid())
            {

                Cursor.Current = Cursors.WaitCursor;
                string mensaje = this.LP.GetText("wrngrabarCodIVA", "Se va a actualizar el código de IVA") + " " + this.codigo.Trim();
                mensaje += " " + this.LP.GetText("conguardarCambios", "¿Desea guardar los cambios?");

                DialogResult yesno = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                if (yesno == DialogResult.No)
                {
                    //Cerrar el formulario
                    bGrabar = true;
                    this.Close();
                }

                if (yesno == DialogResult.Yes)
                {
                    // jlg 30/06 if (this.FormValid())
                    // jlg 30 / 06 {
                    string result = "";
                    bGrabar = true;

                    if (this.nuevo || this.copiar)
                    {
                        result = this.AltaInfo();
                        if (result == "")
                        {
                            //this.nuevo = false;
                            this.codigo = this.txtDescripcion.Text.Trim();
                        }
                    }
                    else result = this.ActualizarInfo();

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
                    // jlg 30 / 06 }
                }
                bGrabar = false;
            }
        }

        /// <summary>
        /// Eliminar un código de IVA
        /// </summary>
        private void Eliminar()
        {
            Cursor.Current = Cursors.WaitCursor;
            bEliminar = false;
            bGrabar = false;

            string mensaje = this.LP.GetText("wrnSuprimirCodIVA", "Se va a eliminar el código de IVA") + " " + this.codigo.Trim();
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");

            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.No)
            {
                //Cerrar el formulario
                bEliminar = true;
                this.Close();
            }
            if (result == DialogResult.Yes)
            {
                bEliminar = true;
                try
                {
                    //Comprobar si el codigo de IVA se puede eliminar (no existan datos contables en la tabla GLB01)
                    if (!this.modificarEliminarCodigoIVA)
                    {
                        RadMessageBox.Show(this.LP.GetText("errEliminarCodIVAEnUso", "No es posible eliminar el código de IVA porque está en uso en los datos contables."), this.LP.GetText("errValTitulo", "Error"));
                        return;
                    }

                    string query = "";
                    int cantRegistros = 0;

                    //Eliminar las claves fiscales 
                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTA1 ";
                        query += "where TIPLAI = '" + this.codigoPlan + "' and COIVAI = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTA2 ";
                        query += "where TIPLA2 = '" + this.codigoPlan + "' and COIVA2 = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    //Eliminar de la tabla de Tipos de IVA
                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTX1 ";
                        query += "where TIPLCX = '" + this.codigoPlan + "' and COIVCX = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    //Eliminar el Libro del SII 
                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTA3 ";
                        query += "where TIPLA3 = '" + this.codigoPlan + "' and COIVA3 = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }


                    //Eliminar el Codigo de IVA
                    try
                    {
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                        query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (cantRegistros != 1)
                        {
                            RadMessageBox.Show(this.LP.GetText("errEliminarCodIVANoPosible", "No se eliminó el código de IVA."), this.LP.GetText("errValTitulo", "Error"));
                        }
                        else
                        {
                            //Cerrar el formulario
                            this.Close();
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                bEliminar = false;
                return;
            }

            Cursor.Current = Cursors.Default;
            bEliminar = false;
        }

        #region Rutinas para Validar la pestaña General
        /// <summary>
        /// Valida la descripcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        private string ValidarDescripcion(string valor)
        {
            string result = "";
            
                if (this.txtDescripcion.Text.Trim() == "")
                {
                    result = this.LP.GetText("lblDescripcion", "Descripción debe de informarse");
                }
            
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la cuenta de mayor
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCuentaMayor(string valor)
        {
            string result = "";
            try
            {
                //Comprobar que la cuenta de mayor es válida
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + valor + "' and STATMC = 'V' and TCUEMC = 'D' and RNITMC = 'R' ";
                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCuentaMayorErrComp", "La cuenta de mayor no es válida");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCuentaMayorErrCompExcep", "Error al validar la cuenta de mayor") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la cuenta de mayor
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCuentaMayorDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                //Comprobar que la cuenta de mayor es válida
                string query = "select NOABMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + valor + "' and STATMC = 'V' and TCUEMC = 'D' and RNITMC = 'R' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCuentaMayorErrComp", "La cuenta de mayor no es válida");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCuentaMayorErrCompExcep", "Error al validar la cuenta de mayor") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del tipo de auxiliar
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarTipoAux(string valor)
        {
            string result = "";
            try
            {
                //Comprobar que la cuenta de mayor es válida
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + valor + "' and STATMT = 'V' ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblTipoAuxErrComp", "El tipo de auxiliar no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblTipoAuxErrCompExcep", "Error al validar el tipo de auxiliar") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de la cuenta de mayor de contrapartida
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCuentaMayorContrap(string valor)
        {
            string result = "";
            try
            {
                //Comprobar que la cuenta de mayor de contrapartida es válida
                string[] CtaContSplit = valor.Split('-');
                string CtaCont = CtaContSplit[0].Trim();
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + CtaCont + "' and STATMC = 'V' and TCUEMC = 'D' and RNITMC <> 'R' "; //jl 

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCuentaMayorContErrComp", "La cuenta de mayor de contrapartida no es válida");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCuentaMayorContErrCompExcep", "Error al validar la cuenta de mayor de contrapartida") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código de IVA de contrapartida
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCodigoIVAContrap(string valor)
        {
            string result = "";
            try
            {
                //Comprobar que el código de IVA de contrapartida es válido
                string ReperSoport = this.rbRepercutido.IsChecked ? "S" : "R";
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + valor + "' and COIACI = '' and ";
                query += "STATCI = 'V' and RESOCI <> '" + (this.rbRepercutido.IsChecked ? "R" : "S") + "' ";
                //query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + valor + "' and  ";
                // query += "STATCI = 'V' and RESOCI <> '" + this.codigo + "' ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCodIVAContErrComp", "El código de IVA de contrapartida no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCodIVAContErrCompExcep", "Error al validar el código de IVA de contrapartida") + " (" + ex.Message + ")";
            }
            if (bCancelar == false && this.codigoIVACont == this.codigo) result = this.LP.GetText("lblCodIVAContErrCompExcep", 
                "El código de IVA contrapartida no puede ser el mismo que se está definiendo");
            
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código de IVA asociado
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCodigoIVAAsociado(string valor)
        {
            string result = "";
            string IvaContra = "";
            try
            {
                //Comprobar que el código de IVA asociado es válido
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + valor + "' and ";
                // query += "STATCI = 'V' and RESOCI <> '" + this.codigo + "' and ";   // jl
                query += "STATCI = 'V' and RESOCI <> '" + (this.rbRepercutido.IsChecked ? "R" : "S") + "' and ";   // jl
                query += "COIACI = '' ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCodIVAAsociadoErrComp", "El código de IVA asociado no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCodIVAAsociadoErrCompExcep", "Error al validar el código de IVA asociado") + " (" + ex.Message + ")";
            }
            //if (bCancelar == false && this.codigoIVAAsociado != "" && this.codigoIVACont != "")
            if (bCancelar == false && this.codigoIVAAsociado != "")
            { 
                if (this.codigoIVAAsociado == this.codigo) result = this.LP.GetText("lblCodIVAAsociadoErrComp", "El código de IVA asociado no puede ser el mismo que se está definiendo");
                else
                {
                    IvaContra = "";
                    if (this.radButtonTextBoxCtaMayorContrap.Text != "" && this.radButtonTextBoxCtaMayorContrap.Text.Substring(0,1)== "+") IvaContra = this.radButtonTextBoxCtaMayorContrap.Text.Substring(1, 2);
                    if (this.radButtonTextBoxCodIVAContrap.Text != "" && this.radButtonTextBoxCodIVAContrap.Text != " ") IvaContra = this.radButtonTextBoxCodIVAContrap.Text.Substring(0, 2);
                    
                    if (IvaContra !="" && this.codigoIVAAsociado != IvaContra)
                                result = this.LP.GetText("lblCodIVAAsociadoErrComp", "El código IVA contrapartida debe ser el mismo que el código de IVA asociado");
                    
                }
            }
            return (result);
        }
        #endregion


        #region Rutinas para Validar la pestaña Claves Legales
        /// <summary>
        /// Valida la existencia o no del código del Modelo 303 Grupo de operaciones
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod303GrupoOperac(string valor)
        {
            string result = "";
            try
            {
                string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='M' and CODCD = '" + valor + "' and ";
                query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";
                
                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod303GrupoOperacErrComp", "El grupo de operaciones para el Modelo 303 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod303GrupoOperacErrCompExcep", "Error al validar el grupo de operaciones para el Modelo 303 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 303 Grupo de operaciones
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod303GrupoOperacDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and ";
                query += "TIPCD ='M' and ";
                query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod303GrupoOperacErrComp", "El grupo de operaciones para el Modelo 303 no es válido");

                dr.Close();

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod303GrupoOperacErrCompExcep", "Error al validar el grupo de operaciones para el Modelo 303 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 390 Grupo de operaciones
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod390GrupoOperac(string valor)
        {
            string result = "";
            try
            {
                string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";

                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='A' and CODCD = '" + valor + "' and ";
                query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod390GrupoOperacErrComp", "El grupo de operaciones para el Modelo 303 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod303GrupoOperacErrCompExcep", "Error al validar el grupo de operaciones para el Modelo 303 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 390 Grupo de operaciones
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod390GrupoOperacDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string reper_soport = this.rbRepercutido.IsChecked ? "R" : "S";
                
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where ";
                query += "CODCD = '" + valor + "' and TIPCD ='A' and ";
                query += "(RESOD = '' or RESOD = '" + reper_soport + "') ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod390GrupoOperacErrComp", "El grupo de operaciones para el Modelo 303 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod303GrupoOperacErrCompExcep", "Error al validar el grupo de operaciones para el Modelo 303 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 340 Libro registro
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod340LibroRegistro(string valor)
        {
            string result = "";
            try
            {
                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='L' and CODCD = '" + valor + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod340LibroRegistroErrComp", "El libro registro para el Modelo 340 no es válido"); 
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod340LibroRegistroErrCompExcep", "Error al validar el libro registro para el Modelo 340 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 340 Libro registro
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod340LibroRegistroDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and TIPCD ='L' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod340LibroRegistroErrComp", "El libro registro para el Modelo 340 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                
                if (dr != null) dr.Close();
                
                result = this.LP.GetText("lblCLMod340LibroRegistroErrCompExcep", "Error al validar el libro registro para el Modelo 340 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 340 Clave operación
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod340ClaveOperac(string valor)
        {
            string result = "";
            try
            {
                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='O' and CODCD = '" + valor + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod340ClaveOperacErrComp", "La clave operación para el Modelo 340 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod340ClaveOperacErrCompExcep", "Error al validar la clave operación para el Modelo 340 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 340 Clave operación
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod340ClaveOperacDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and TIPCD ='O' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod340ClaveOperacErrComp", "La clave operación para el Modelo 340 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod340ClaveOperacErrCompExcep", "Error al validar la clave operación para el Modelo 340 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 349 Tipo operación
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod349TipoOperac(string valor)
        {
            string result = "";
            try
            {
                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='T' and CODCD = '" + valor + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod349TipoOperacErrComp", "El tipo operación para el Modelo 349 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod349TipoOperacErrCompExcep", "Error al validar el tipo operación para el Modelo 349 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 349 Tipo operación
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod349TipoOperacDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and TIPCD ='T' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod349TipoOperacErrComp", "El tipo operación para el Modelo 349 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod349TipoOperacErrCompExcep", "Error al validar el tipo operación para el Modelo 349 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 190 Clave percepción
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod190ClavePercep(string valor)
        {
            string result = "";
            try
            {
                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='P' and CODCD = '" + valor + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod190ClavePercepErrComp", "La clave de percepción para el Modelo 190 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod190ClavePercepErrCompExcep", "Error al validar la clave de percepción para el Modelo 349 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 190 Clave percepción
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod190ClavePercepDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and TIPCD ='P' ";
            
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod190ClavePercepErrComp", "La clave de percepción para el Modelo 190 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod190ClavePercepErrCompExcep", "Error al validar la clave de percepción para el Modelo 349 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 111 Clave percepción
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCLMod111ClavePercep(string valor)
        {
            string result = "";
            try
            {
                string query = "select count(*) from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='R' and CODCD = '" + valor + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("lblCLMod111ClavePercepErrComp", "La clave de percepción para el Modelo 111 no es válido");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblCLMod111ClavePercepErrCompExcep", "Error al validar la clave de percepción para el Modelo 111 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del código del Modelo 111 Clave percepción
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCLMod111ClavePercepDesc(string valor, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where CODCD = '" + valor + "' and TIPCD ='R' ";
            
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = this.LP.GetText("lblCLMod111ClavePercepErrComp", "La clave de percepción para el Modelo 111 no es válido");

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = this.LP.GetText("lblCLMod111ClavePercepErrCompExcep", "Error al validar la clave de percepción para el Modelo 111 ") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida la existencia o no de un código para un tipo especificado en la tabla de descripciones
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string ValidarCodigoDesc(string valor, string tipo, ref string desc)
        {
            string result = "";
            IDataReader dr = null;
            desc = "";
            try
            {
                string query = "select CODCD, DESCD from ";
                query += this.prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD ='" + tipo + "' and CODCD = '" + valor + "'"  ;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCD")).ToString().Trim();
                }
                else result = "Código no válido";
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result = "Error al validar el código (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Valida codigo desc1
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valor1"></param>
        private string ValidarCodigoDescFactEnt1(string valor, string valor1)
        {
            string result = "";
            try
            {
                // verifico igualdades de códigos y/o códigos en blanco
                if (valor == "" && valor1 != "")
                {
                    result += "Clave régimen especial o trascendencia debe de informarse, si Clave régimen especial o trascendencia adicional 1 está informado.";
                }

                if (valor == valor1)
                {
                    result += "Clave régimen especial o trascendencia no debe de ser igual a clave régimen especial o trascendencia adicional 1.";
                }
                // verifico compatibilidades de códigos
                    if (valor == "01")
                    {
                        if (valor1 != "" && valor1 != "02")
                        {
                            result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                        }
                    }
                    if (valor == "03" || valor == "08")
                    {
                        if (valor1 != "" && valor1 != "01")
                        {
                            result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                        }

                    }
                    if (valor == "05")
                    {
                        if (valor1 != "" && valor1 != "01" && valor1 != "11" && valor1 != "12" && valor1 != "13" && valor1 != "06"
                        && valor1 != "08")
                        {
                            result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                        }
                    }

                if (valor == "06")
                {
                    if (valor1 != "" && valor1 != "11" && valor1 != "12" && valor1 != "13" && valor1 != "14" && valor1 != "15")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }
                }

                if (valor == "07")
                {
                    if (valor1 != "" && valor1 != "01" && valor1 != "03" && valor1 != "05" && valor1 != "09" && valor1 != "11"
                        && valor1 != "12" && valor1 != "13" && valor1 !="14" && valor1 != "15")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }

                    if (valor == "11" || valor=="12" || valor=="13")
                    {
                        if (valor1 != "" && valor1 != "08" && valor1 != "15")
                        {
                            result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error al validar el código (" + ex.Message + ")";
            }
            return (result);
        }
        /// <summary>
        /// Valida codigo desc2
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        private string ValidarCodigoDescFactEnt2(string valor, string valor1, string valor2)
        {
            string result = "";
            try
            {
                if (valor == valor2)
                {
                    result += "Clave régimen especial o trascendencia no debe de ser igual a Clave régimen especial o trascendencia adicional 2.";
                }
                
                if (valor1 == valor2)
                {
                    result += "Clave régimen especial o trascendencia adicional 1 no debe de ser igual a Clave régimen especial o trascendencia adicional 2.";
                }

                if (valor == "" && valor2 != "")
                {
                    result += "Clave régimen especial o trascendencia debe de informarse, si código transcendencia 2 está informado.";
                }
                if (valor1 == "" && valor2 != "")
                {
                    result += "Clave régimen especial o trascendencia adicional 1 debe de informarse, si Clave régimen especial o trascendencia adicional 2 está informado.";
                }
                // verifico compatibilidades de códigos
                if (valor == "01" || valor1 == "01")
                {
                    if (valor2 != "" && valor2 != "02")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }
                }  
                 if (valor == "03" || valor == "08" || valor1 == "03" || valor1 == "08")
                 {
                        if (valor2 != "" && valor2 != "01")
                        {
                            result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                        }

                 }
                 if (valor == "05" || valor1 == "05")
                 {
                    if (valor2 != "" && valor2 != "01" && valor2 != "11" && valor2 != "12" && valor2 != "13" && valor2 != "06"
                        && valor2 != "08")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }
                  }

                if (valor == "06" || valor1 == "06")
                {
                    if (valor2 != "" && valor2 != "11" && valor2 != "12" && valor2 != "13" && valor2 != "14" && valor2 != "15")
                    {
                            result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                        }
                }
                if (valor == "07" || valor1 == "07")
                {
                    if (valor2 != "" && valor2 != "01" && valor2 != "03" && valor2 != "05" && valor2 != "09" && valor2 != "11"
                        && valor2 != "12" && valor2 != "13" && valor2 != "14" && valor2 != "15")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }
                }

                if (valor == "11" || valor == "12" || valor == "13" || valor1 == "11" || valor1 == "12" || valor1 == "13")
                {
                    if (valor2 != "" && valor2 != "08" && valor2 != "15")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error al validar el código (" + ex.Message + ")";
            }
            return (result);
        }
        /// <summary>
        /// Valida codigo desc1
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valor1"></param>
        private string ValidarCodigoDescFactRec1(string valor, string valor1)
        {
            string result = "";
            try
            {
                // verifico igualdades de códigos y/o códigos en blanco
                if (valor == "" && valor1 != "")
                {
                    result += "Clave régimen especial o trascendencia debe de informarse, si Clave régimen especial o trascendencia adicional 1 está informado.";
                }

                if (valor == valor1)
                {
                    result += "Clave régimen especial o trascendencia no debe de ser igual a clave régimen especial o trascendencia adicional 1.";
                }
                // verifico compatibilidades de códigos
                
                if (valor == "03" || valor == "08")
                {
                    if (valor1 != "" && valor1 != "01")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }

                }
                if (valor == "05")
                {
                    if (valor1 != "" && valor1 != "01" && valor1 != "12" && valor1 != "06"  && valor1 != "08")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }
                }

                if (valor == "06")
                {
                    if (valor1 != "" && valor1 != "12")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }
                }

                if (valor == "07")
                {
                    if (valor1 != "" && valor1 != "01" && valor1 != "03" && valor1 != "05" && valor1 != "12")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                    }

                    if (valor == "12")
                    {
                        if (valor1 != "" && valor1 != "08")
                        {
                            result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 1.";
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error al validar el código (" + ex.Message + ")";
            }
            return (result);
        }
        /// <summary>
        /// Valida codigo desc2
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        private string ValidarCodigoDescFactRec2(string valor, string valor1, string valor2)
        {
            string result = "";
            try
            {
                if (valor == valor2)
                {
                    result += "Clave régimen especial o trascendencia no debe de ser igual a Clave régimen especial o trascendencia adicional 2.";
                }

                if (valor1 == valor2)
                {
                    result += "Clave régimen especial o trascendencia adicional 1 no debe de ser igual a Clave régimen especial o trascendencia adicional 2.";
                }

                if (valor == "" && valor2 != "")
                {
                    result += "Clave régimen especial o trascendencia debe de informarse, si código transcendencia 2 está informado.";
                }
                if (valor1 == "" && valor2 != "")
                {
                    result += "Clave régimen especial o trascendencia adicional 1 debe de informarse, si Clave régimen especial o trascendencia adicional 2 está informado.";
                }
                // verifico compatibilidades de códigos
                
                if (valor == "03" || valor == "08" || valor1 == "03" || valor1 == "08")
                {
                    if (valor2 != "" && valor2 != "01")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }

                }
                if (valor == "05" || valor1 == "05")
                {
                    if (valor2 != "" && valor2 != "01" && valor2 != "12" && valor2 != "06" && valor2 != "08")
                    {
                        result += "Valor clave régimen especial, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }
                }

                if (valor == "06" || valor1 == "06")
                {
                    if (valor2 != "" && valor2 != "12")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }
                }
                if (valor == "07" || valor1 == "07")
                {
                    if (valor2 != "" && valor2 != "01" && valor2 != "03" && valor2 != "05" && valor2 != "12")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }
                }

                if (valor == "12")
                {
                    if (valor2 != "" && valor2 != "08")
                    {
                        result += "Valor clave régimen especial o trascendencia adicional 1, incompatible con valor clave régimen especial o trascendencia adicional 2.";
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error al validar el código (" + ex.Message + ")";
            }
            return (result);
        }
        #endregion
        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de Claves Legales
        /// </summary>
        private void ActualizaValoresOrigenControlesClavesLegales()
        {
            this.radButtonTextBoxCLMod303GrupoOperac.Tag = this.radButtonTextBoxCLMod303GrupoOperac.Text;
            this.radButtonTextBoxCLMod390GrupoOperac.Tag = this.radButtonTextBoxCLMod390GrupoOperac.Text;
            this.radButtonTextBoxCLMod340LibroRegistro.Tag = this.radButtonTextBoxCLMod340LibroRegistro.Text;
            this.radButtonTextBoxCLMod340ClaveOperac.Tag = this.radButtonTextBoxCLMod340ClaveOperac.Text;
            this.radButtonTextBoxlCLMod349TipoOperac.Tag = this.radButtonTextBoxlCLMod349TipoOperac.Text;

            string dinerario_especie = "0";
            if (this.rbCLDinerario.IsChecked) dinerario_especie = "1";
            else if (this.rbCLEspecie.IsChecked) dinerario_especie = "2";
            this.gbCLMod180ModalPercep.Tag = dinerario_especie;

            this.radButtonTextBoxCLMod190ClavePercep.Tag = this.radButtonTextBoxCLMod190ClavePercep.Text;

            this.cmbCLMod190SubclavePercep.Tag = this.cmbCLMod190SubclavePercep.Text.Trim();

            this.radButtonTextBoxCLMod111ClavePercep.Tag = this.radButtonTextBoxCLMod111ClavePercep.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de SII Libros
        /// </summary>
        private void ActualizaValoresOrigenControlesSiiLibros()
        {
            try
            {
                this.cmbSiiLibros.Tag = this.cmbSiiLibros.Text;

                //Libro - Facturas Expedidas
                this.radButtonTextBoxSiiTipoFactura.Tag = this.radButtonTextBoxSiiTipoFactura.Text; ;
                this.radButtonTextBoxSiiClaveRegEspTrasc.Tag = this.radButtonTextBoxSiiClaveRegEspTrasc.Text;
                this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Tag = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text;
                this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Tag = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text;
                this.radButtonTextBoxSiiTipoFacturaRectif.Tag = this.radButtonTextBoxSiiTipoFacturaRectif.Text;
                this.radDropDownListSiiTipoDesglose.Tag = this.radDropDownListSiiTipoDesglose.Text;
                this.radButtonTextBoxSiiTipoOperSujNoExe.Tag = this.radButtonTextBoxSiiTipoOperSujNoExe.Text;
                this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Tag = this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text;
                this.radButtonTextBoxSiiTipoOperacNoSujeta.Tag = this.radButtonTextBoxSiiTipoOperacNoSujeta.Text;
                this.radButtonTextBoxSiiAgenciaTributaria.Tag = this.radButtonTextBoxSiiAgenciaTributaria.Text;
                this.radToggleSwitchSiiCobroMetalico.Tag = this.radToggleSwitchSiiCobroMetalico.Value;
                this.radToggleSwitchSiiCupon.Tag = this.radToggleSwitchSiiCupon.Value;
                this.radToggleSwitchSiiFactSimpArt72_73.Tag = this.radToggleSwitchSiiFactSimpArt72_73.Value;
                this.radToggleSwitchSiiTaxFree.Tag = this.radToggleSwitchSiiTaxFree.Value;
                this.radToggleSwitchSiiViajeros.Tag = this.radToggleSwitchSiiViajeros.Value;
                this.radButtonTextBoxSiiCampoLibreExp.Tag = this.radButtonTextBoxSiiCampoLibreExp.Text;

                //Libro - Facturas Recibidas
                //  this.radButtonTextBoxlSiiTipoFacturaRecibida.Tag = this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text; jl
                this.radButtonTextBoxlSiiTipoFacturaRecibida.Tag = this.radButtonTextBoxlSiiTipoFacturaRecibida.Text;
                this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Tag = this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text;
                this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Tag = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text;
                this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Tag = this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text;
                this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Tag = this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text;
                //this.radDropDownListSiiTipoDesgloseFactRecibida.Tag = this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text; jl
                this.radDropDownListSiiTipoDesgloseFactRecibida.Tag = this.radDropDownListSiiTipoDesgloseFactRecibida.Text;
                this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Tag = this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text;
                this.radToggleSwitchSiiBienesInversionFactRecibida.Tag = this.radToggleSwitchSiiBienesInversionFactRecibida.Value;
                this.radToggleSwitchSiiProrrataFactRecibida.Tag = this.radToggleSwitchSiiProrrataFactRecibida.Value;
                this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Tag = this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value;
                this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Tag = this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value;
                this.txtSiiTipoImpositivoProrrataFactRecibida.Tag = this.txtSiiTipoImpositivoProrrataFactRecibida.Text;
                this.radButtonTextBoxSiiCampoLibreRec.Tag = this.radButtonTextBoxSiiCampoLibreRec.Text;
                //Libro - Operaciones de seguros
                this.radButtonTextBoxSiiTipoOpSeguro.Tag = this.radButtonTextBoxSiiTipoOpSeguro.Text;
                this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Tag = this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text;

                //Libro - Operaciones intracomunitarias
                this.radButtonTextBoxSiiTipoOpIntracom.Tag = this.radButtonTextBoxSiiTipoOpIntracom.Text;
                this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Tag = this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text;

                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles para la grid de Tipos de IVA
        /// </summary>
        private void ActualizaValoresOrigenControlesTiposIVA()
        {
            try
            {
                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    //SMR if (!TodaFilaEnBlanco(this.dtTiposIVAGrid, i))
                    if (!TodaFilaEnBlanco(this.radGridViewTiposIVA, i))
                    {
                        if ((this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value) != null) this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value = this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value.ToString();
                        if ((this.radGridViewTiposIVA.Rows[i].Cells["PorcIVAOrigen"].Value) != null) this.radGridViewTiposIVA.Rows[i].Cells["PorcIVAOrigen"].Value = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString();
                        if ((this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquivOrigen"].Value) != null) this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquivOrigen"].Value = this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString();
                        if ((this.radGridViewTiposIVA.Rows[i].Cells["DescripcionOrigen"].Value) != null) this.radGridViewTiposIVA.Rows[i].Cells["DescripcionOrigen"].Value = this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value.ToString();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtDescripcion.Tag = this.txtDescripcion.Text;

            string reperc_soport = "S";
            if (this.rbRepercutido.IsChecked) reperc_soport = "R";
            this.gbRepercSopor.Tag = reperc_soport;

            string deduc_noDeduc = "D";
            if (this.rbNoDeducible.IsChecked) deduc_noDeduc = "N";
            this.gbDeducibleNoDed.Tag = deduc_noDeduc;

            this.radButtonTextBoxCtaMayor.Tag = this.radButtonTextBoxCtaMayor.Text;
            this.radButtonTextBoxTipoAux.Tag = this.radButtonTextBoxTipoAux.Text;

            this.txtLibro.Tag = this.txtLibro.Text;
            this.txtSerie.Tag = this.txtSerie.Text;

            string cuadrecuotasi_no = "S";
            if (this.radToggleSwitchCuadreCuotas.Value == false) cuadrecuotasi_no = "N";
            this.radToggleSwitchCuadreCuotas.Tag = cuadrecuotasi_no == "S" ? true : false;

            this.txtMaxDescuadreCuotas.Tag = this.txtMaxDescuadreCuotas.Text;

            this.radButtonTextBoxCtaMayorContrap.Tag = this.radButtonTextBoxCtaMayorContrap.Text;
            this.radButtonTextBoxCodIVAContrap.Tag = this.radButtonTextBoxCodIVAContrap.Text;

            this.radButtonTextBoxCodIVAAsociado.Tag = this.radButtonTextBoxCodIVAAsociado.Text;

            //Tipos de IVA
            this.ActualizaValoresOrigenControlesTiposIVA();

            //Claves Legales
            this.ActualizaValoresOrigenControlesClavesLegales();

            //SII Libro
            this.ActualizaValoresOrigenControlesSiiLibros();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) 
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActivo.Tag = true;
            this.txtDescripcion.Tag = "";

            this.gbRepercSopor.Tag = "S";
            this.gbDeducibleNoDed.Tag = "D";
            
            this.radButtonTextBoxCtaMayor.Tag = "";
            this.radButtonTextBoxTipoAux.Tag = "";

            this.txtLibro.Tag = "";
            this.txtSerie.Tag = "";

            this.radToggleSwitchCuadreCuotas.Tag = true;

            this.txtMaxDescuadreCuotas.Tag = "0,00";
            this.radButtonTextBoxCtaMayorContrap.Tag = "";
            this.radButtonTextBoxCodIVAContrap.Tag = "";
            this.radButtonTextBoxCodIVAAsociado.Tag = "";

            //Claves Legales
            this.ActualizaValoresOrigenTAGControlesClavesLegales();

            //SII Libros
            this.ActualizaValoresOrigenTAGControlesSiiLibro();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) en la pestaña de Claves Legales
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesClavesLegales()
        {
            this.radButtonTextBoxCLMod303GrupoOperac.Tag = "";
            this.radButtonTextBoxCLMod390GrupoOperac.Tag = "";
            this.radButtonTextBoxCLMod340LibroRegistro.Tag = "";
            this.radButtonTextBoxCLMod340ClaveOperac.Tag = "";
            this.radButtonTextBoxlCLMod349TipoOperac.Tag = "";
            this.gbCLMod180ModalPercep.Tag = "0";
            this.radButtonTextBoxCLMod190ClavePercep.Tag = "";
            this.cmbCLMod190SubclavePercep.Tag = "";
            this.radButtonTextBoxCLMod111ClavePercep.Tag = "";
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) en la pestaña de SII Libros
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesSiiLibro()
        {
            this.cmbSiiLibros.Tag = "";

            //Libro - Facturas Recibidas
            this.radButtonTextBoxSiiTipoFactura.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrasc.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Tag = "";
            this.radButtonTextBoxSiiTipoFacturaRectif.Tag = "";
            this.radDropDownListSiiTipoDesglose.Tag = "";
            this.radButtonTextBoxSiiTipoOperSujNoExe.Tag = "";
            this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Tag = "";
            this.radButtonTextBoxSiiTipoOperacNoSujeta.Tag = "";
            this.radButtonTextBoxSiiAgenciaTributaria.Tag = "";
            this.radToggleSwitchSiiCobroMetalico.Value = false;
            this.radToggleSwitchSiiCupon.Value = false;
            this.radToggleSwitchSiiFactSimpArt72_73.Value = false;
            this.radToggleSwitchSiiTaxFree.Value = false;
            this.radToggleSwitchSiiViajeros.Value = false;

            //Libro - Facturas Recibidas
            this.radButtonTextBoxlSiiTipoFacturaRecibida.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Tag = "";
            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Tag = "";
            this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Tag = "";
            this.radDropDownListSiiTipoDesgloseFactRecibida.Tag = "";
            this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Tag = "";
            this.radToggleSwitchSiiBienesInversionFactRecibida.Value = false;
            this.radToggleSwitchSiiProrrataFactRecibida.Value = false;
            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value = false;
            this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value = false;

            //Libro - Operaciones de seguros
            this.radButtonTextBoxSiiTipoOpSeguro.Tag = "";
            this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Tag = "";

            //Libro - Operaciones intracomunitarias
            this.radButtonTextBoxSiiTipoOpIntracom.Tag = "";
            this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Tag = "";
        }

        /// <summary>
        /// Verifica si se han modificado los tipos de iva (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool TiposIVACambio()
        {
            bool result = false;

            try
            {
                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    if (this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value.ToString() != this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value.ToString())
                    {
                        result = true;
                        break;
                    }

                    if (this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString() != this.radGridViewTiposIVA.Rows[i].Cells["PorcIVAOrigen"].Value.ToString())
                    {
                        result = true;
                        break;
                    }

                    if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString() != this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquivOrigen"].Value.ToString())
                    {
                        result = true;
                        break;
                    }

                    if (this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value.ToString() != this.radGridViewTiposIVA.Rows[i].Cells["DescripcionOrigen"].Value.ToString())
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica si se han modificado las claves legales (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool ClavesLegalesCambio()
        {
            bool result = false;

            try
            {
                string dinerario_especie = "0";

                if (this.nuevo)
                {
                    this.radButtonTextBoxCLMod303GrupoOperac.Tag = "";
                    this.radButtonTextBoxCLMod390GrupoOperac.Tag = "";
                    this.radButtonTextBoxCLMod340LibroRegistro.Tag = "";
                    this.radButtonTextBoxCLMod340ClaveOperac.Tag = "";
                    this.radButtonTextBoxlCLMod349TipoOperac.Tag = "";
                    this.gbCLMod180ModalPercep.Tag = "0";
                    this.radButtonTextBoxCLMod190ClavePercep.Tag = "";
                    this.cmbCLMod190SubclavePercep.Tag = "";
                    this.radButtonTextBoxCLMod111ClavePercep.Tag = "";
                }
                else
                {
                    if (this.rbSoportado.IsChecked)
                    {
                        this.radButtonTextBoxlCLMod349TipoOperac.Text = "";
                        this.radButtonTextBoxlCLMod349TipoOperac.Tag = "";
                        this.gbCLMod180ModalPercep.Tag = "0";
                        this.radButtonTextBoxCLMod190ClavePercep.Text = "";
                        this.radButtonTextBoxCLMod190ClavePercep.Tag = "";
                        this.cmbCLMod190SubclavePercep.Text = "";
                        this.cmbCLMod190SubclavePercep.Tag = "";
                        this.radButtonTextBoxCLMod111ClavePercep.Text = "";
                        this.radButtonTextBoxCLMod111ClavePercep.Tag = "";
                    }
                    else
                    {
                        if (this.rbCLDinerario.IsChecked) dinerario_especie = "1";
                        else if (this.rbCLEspecie.IsChecked) dinerario_especie = "2";
                    }
                }

                if (this.radButtonTextBoxCLMod303GrupoOperac.Text.Trim() != this.radButtonTextBoxCLMod303GrupoOperac.Tag.ToString().Trim() ||
                    this.radButtonTextBoxCLMod390GrupoOperac.Text.Trim() != this.radButtonTextBoxCLMod390GrupoOperac.Tag.ToString().Trim() ||
                    this.radButtonTextBoxCLMod340LibroRegistro.Text.Trim() != this.radButtonTextBoxCLMod340LibroRegistro.Tag.ToString().Trim() ||
                    this.radButtonTextBoxCLMod340ClaveOperac.Text.Trim() != this.radButtonTextBoxCLMod340ClaveOperac.Tag.ToString().Trim() ||
                    this.radButtonTextBoxlCLMod349TipoOperac.Text.Trim() != this.radButtonTextBoxlCLMod349TipoOperac.Tag.ToString().Trim() ||
                    dinerario_especie != this.gbCLMod180ModalPercep.Tag.ToString() ||
                    this.radButtonTextBoxCLMod190ClavePercep.Text.Trim() != this.radButtonTextBoxCLMod190ClavePercep.Tag.ToString().Trim() ||
                    this.cmbCLMod190SubclavePercep.Text.Trim() != this.cmbCLMod190SubclavePercep.Tag.ToString().Trim() ||
                    this.radButtonTextBoxCLMod111ClavePercep.Text.Trim() != this.radButtonTextBoxCLMod111ClavePercep.Tag.ToString().Trim()
                    )
                {
                    result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica si se ha modificado el libro del SII (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool SiiLibroCambio()
        {
            bool result = false;
            
            try
            {
                if (this.nuevo)
                {
                    //Facturas Emitidas
                    this.radButtonTextBoxSiiTipoFactura.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrasc.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Tag = "";
                    this.radButtonTextBoxSiiTipoFacturaRectif.Tag = "";
                    this.radDropDownListSiiTipoDesglose.Tag = "";
                    this.radButtonTextBoxSiiTipoOperSujNoExe.Tag = "";
                    this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Tag = "";
                    this.radButtonTextBoxSiiTipoOperacNoSujeta.Tag = "";
                    this.radButtonTextBoxSiiAgenciaTributaria.Tag = "";
                    this.radToggleSwitchSiiCobroMetalico.Value = false;
                    this.radToggleSwitchSiiCupon.Value = false;
                    this.radToggleSwitchSiiFactSimpArt72_73.Value = false;
                    this.radToggleSwitchSiiTaxFree.Value = false;
                    this.radToggleSwitchSiiViajeros.Value = false;

                    //Libro - Facturas Recibidas
                    this.radButtonTextBoxlSiiTipoFacturaRecibida.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Tag = "";
                    this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Tag = "";
                    this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Tag = "";
                    this.radDropDownListSiiTipoDesgloseFactRecibida.Tag = "";
                    this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Tag = "";
                    this.radToggleSwitchSiiBienesInversionFactRecibida.Value = false;
                    this.txtSiiTipoImpositivoProrrataFactRecibida.Text = "0,00";
                    this.radToggleSwitchSiiProrrataFactRecibida.Value = false;
                    this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value = false;
                    this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value = false;
                    
                    //Libro - Operaciones de seguros
                    this.radButtonTextBoxSiiTipoOpSeguro.Tag = "";
                    this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Tag = "";

                    //Libro - Operaciones intracomunitarias
                    this.radButtonTextBoxSiiTipoOpIntracom.Tag = "";
                    this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Tag = "";
                }

                string codigoLibro = LibroSIIActivoCodigo();
                switch (codigoLibro)
                {
                    case "E":   //Facturas Emitidas
                        if (this.cmbSiiLibros.Text.ToString().Trim() != this.cmbSiiLibros.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoFactura.Text.Trim() != this.radButtonTextBoxSiiTipoFactura.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrasc.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrasc.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrascAdicional1.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrascAdicional2.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoFacturaRectif.Text.Trim() != this.radButtonTextBoxSiiTipoFacturaRectif.Tag.ToString().Trim() ||
                            this.radDropDownListSiiTipoDesglose.Text.Trim() != this.radDropDownListSiiTipoDesglose.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoOperSujNoExe.Text.Trim() != this.radButtonTextBoxSiiTipoOperSujNoExe.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Text.Trim() != this.radButtonTextBoxSiiCausaExeOperSujetaYExenta.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoOperacNoSujeta.Text.Trim() != this.radButtonTextBoxSiiTipoOperacNoSujeta.Tag.ToString().Trim() ||
                            this.radToggleSwitchSiiCobroMetalico.Value != (bool)(this.radToggleSwitchSiiCobroMetalico.Tag) ||
                            this.radToggleSwitchSiiCupon.Value != (bool)(this.radToggleSwitchSiiCupon.Tag) ||
                            this.radToggleSwitchSiiFactSimpArt72_73.Value != (bool)(this.radToggleSwitchSiiFactSimpArt72_73.Tag) ||
                            this.radToggleSwitchSiiTaxFree.Value != (bool)(this.radToggleSwitchSiiTaxFree.Tag) ||
                            this.radToggleSwitchSiiViajeros.Value != (bool)(this.radToggleSwitchSiiViajeros.Tag) ||
                            this.radButtonTextBoxSiiAgenciaTributaria.Text.Trim() != this.radButtonTextBoxSiiAgenciaTributaria.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiCampoLibreExp.Text.Trim() != this.radButtonTextBoxSiiCampoLibreExp.Tag.ToString().Trim() )
                        {
                            result = true;
                        }
                        break;

                    case "R":   //Facturas Recibidas
                        if (this.cmbSiiLibros.Text.ToString().Trim() != this.cmbSiiLibros.Tag.ToString().Trim() ||
                            this.radButtonTextBoxlSiiTipoFacturaRecibida.Text.Trim() != this.radButtonTextBoxlSiiTipoFacturaRecibida.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrascFactRecibida.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Text.Trim() != this.radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Text.ToString() != this.radButtonTextBoxSiiTipoFactRectifFactRecibida.Tag.ToString().Trim() ||
                            this.radDropDownListSiiTipoDesgloseFactRecibida.Text.Trim() != this.radDropDownListSiiTipoDesgloseFactRecibida.Tag.ToString().Trim() ||
                            this.radToggleSwitchSiiBienesInversionFactRecibida.Value != (bool)(this.radToggleSwitchSiiBienesInversionFactRecibida.Tag) ||
                            this.txtSiiTipoImpositivoProrrataFactRecibida.Text.Trim() != this.txtSiiTipoImpositivoProrrataFactRecibida.Tag.ToString().Trim() ||
                            this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value != (bool)(this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Tag) ||
                            this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value != (bool)(this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Tag) ||
                            this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Text.Trim() != this.radButtonTextBoxSiiAgenciaTributariaFactRecibida.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiCampoLibreRec.Text.Trim() != this.radButtonTextBoxSiiCampoLibreRec.Tag.ToString().Trim())
                        {
                            result = true;
                        }
                        break;

                    case "S":   //Operaciones de seguros
                        if (this.cmbSiiLibros.Text.ToString().Trim() != this.cmbSiiLibros.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoOpSeguro.Text.Trim() != this.radButtonTextBoxSiiTipoOpSeguro.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Text.Trim() != this.radButtonTextBoxSiiAgenciaTributariaOpSeguro.Tag.ToString().Trim() )
                        {
                            result = true;
                        }
                        break;
                    case "U":   //Operaciones intracomunitarias
                        if (this.cmbSiiLibros.Text.Trim() != this.cmbSiiLibros.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiTipoOpIntracom.Text.Trim() != this.radButtonTextBoxSiiTipoOpIntracom.Tag.ToString().Trim() ||
                            this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Text.Trim() != this.radButtonTextBoxSiiAgenciaTributariaOpIntracom.Tag.ToString().Trim())
                        {
                            result = true;
                        }
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica si existe el código de IVA para el plan indicado en la tabla IVTA3 (SII Libros)
        /// </summary>
        /// <returns></returns>
        private bool SiiLibroExisteCodigoIVA()
        {
            IDataReader dr = null;
            bool existeLibro = false;

            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVTA3 ";
                query += "where TIPLA3 = '" + this.CodigoPlan + "' and COIVA3 ='" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read()) existeLibro = true;
                
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (existeLibro);
        }

        /// <summary>
        /// Llena el desplegable para el modelo 190 con las subclaves de percepción en función de la clave introducida
        /// </summary>
        private void FillcmbCLMod190SubclavePercep()
        {
            try
            {
                //Eliminar elementos del combo
                this.cmbCLMod190SubclavePercep.Items.Clear();

                this.cmbCLMod190SubclavePercep.Items.Add("");

                string clave = "";
                if (this.radButtonTextBoxCLMod190ClavePercep.Text.Length <= 1) clave = this.radButtonTextBoxCLMod190ClavePercep.Text;
                else clave = this.radButtonTextBoxCLMod190ClavePercep.Text.Substring(0, 1);

                switch (clave)
                {
                    case "A":
                    case "C":
                    case "D":
                    case "E":
                    case "J":
                        this.cmbCLMod190SubclavePercep.Items.Add("00");
                        break;
                    case "B":
                    case "G":
                        this.cmbCLMod190SubclavePercep.Items.Add("01");
                        this.cmbCLMod190SubclavePercep.Items.Add("02");
                        this.cmbCLMod190SubclavePercep.Items.Add("03");
                        break;
                    case "F":
                    case "I":
                    case "K":
                        this.cmbCLMod190SubclavePercep.Items.Add("01");
                        this.cmbCLMod190SubclavePercep.Items.Add("02");
                        break;
                    case "H":
                        this.cmbCLMod190SubclavePercep.Items.Add("01");
                        this.cmbCLMod190SubclavePercep.Items.Add("02");
                        this.cmbCLMod190SubclavePercep.Items.Add("03");
                        this.cmbCLMod190SubclavePercep.Items.Add("04");
                        break;
                    case "L":
                        string valor = "";
                        for (int i = 1; i <= 21; i++)
                        {
                            if (i < 10) valor = "0" + i.ToString();
                            else valor = i.ToString();
                            this.cmbCLMod190SubclavePercep.Items.Add(valor);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Valida que no exista el código de iva
        /// </summary>
        /// <returns></returns>
        private bool CodigoIVAValido()
        {
            bool result = false;

            try
            {
                string codIVAAux = this.txtCodigo.Text.Trim();

                if (codIVAAux != "")
                {
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                    query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + codIVAAux + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Mostrar/Ocultar los controles relacionados con las claves legalaes para el repercutido
        /// </summary>
        /// <param name="valor"></param>
        private void MostrarControlesRepercutido(bool valor)
        {
            this.lblCLMod349TipoOperac.Visible = valor;
            this.radButtonTextBoxlCLMod349TipoOperac.Visible = valor;
            this.lblCLMod180ModalPercep.Visible = valor;
            this.gbCLMod180ModalPercep.Visible = valor;
            this.lblCLMod190ClavePercep.Visible = valor;
            this.radButtonTextBoxCLMod190ClavePercep.Visible = valor;
            this.lblCLMod190SubclavePercep.Visible = valor;
            this.cmbCLMod190SubclavePercep.Visible = valor;
            this.lblCLMod111ClavePercep.Visible = valor;
            this.radButtonTextBoxCLMod111ClavePercep.Visible = valor;
        }

        #region Alta Código de IVA
        /// <summary>
        /// Devuelve los valores del formulario general que se insertarán o actualizarán en la tabla IVT01
        /// </summary>
        /// <param name="nombci"></param>
        /// <param name="resoci"></param>
        /// <param name="deduci"></param>
        /// <param name="cuenci"></param>
        /// <param name="tauxci"></param>
        /// <param name="librci"></param>
        /// <param name="serici"></param>
        /// <param name="cuadci"></param>
        /// <param name="mxajci"></param>
        /// <param name="cuecci"></param>
        /// <param name="coiaci"></param>
        /// <param name="tpivci"></param>
        /// <param name="recgci"></param>
        /// <returns></returns>
        private string GeneralDatosFormToTabla(ref string nombci, ref string resoci, ref string deduci, ref string cuenci, ref string tauxci, 
                                               ref string librci, ref string serici, ref string cuadci, ref string mxajci, ref string cuecci,
                                               ref string coiaci, ref string tpivci, ref string recgci)
        {
            string result = "";

            try
            {
                nombci = this.txtDescripcion.Text.Trim();
                nombci = (nombci == "") ? " " : nombci;

                resoci = "S";
                if (this.rbRepercutido.IsChecked) resoci = "R";

                deduci = "D";
                if (this.rbNoDeducible.IsChecked) deduci = "N";

                cuenci = this.codigoCtaMayor.Trim();
                cuenci = (cuenci == "") ? " " : cuenci;

                //Buscar la cuenta a último nivel
                if (cuenci != " " && cuenci != "")
                {
                    string errorMsg = "";
                    DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivel(cuenci, this.codigoPlan, ref errorMsg);

                    if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0)
                    {
                        cuenci = dtCtasUltimoNivel.Rows[dtCtasUltimoNivel.Rows.Count - 1]["CUENMC"].ToString().Trim();
                    }
                }

                tauxci = this.codigoTipoAux.Trim();
                tauxci = (tauxci == "") ? " " : tauxci;
     
                librci = this.txtLibro.Text;
                librci = (librci == "") ? " " : librci;

                serici = this.txtSerie.Text;
                serici = (serici == "") ? " " : serici;

                cuadci = "S";
                if (this.radToggleSwitchCuadreCuotas.Value == false) cuadci = "N";

                mxajci = "0,00";
                if (cuadci == "S") 
                {
                  if (this.txtMaxDescuadreCuotas.Text.Trim() != "") mxajci = this.txtMaxDescuadreCuotas.Text.Trim();
                }

                cuecci = " ";
                if (this.codigoCtaMayorCont != null && this.codigoCtaMayorCont != "") cuecci = this.codigoCtaMayorCont;
                else
                {
                    if (radButtonTextBoxCodIVAContrap.Tag != null && radButtonTextBoxCodIVAContrap.Tag != "" &&
                        (radButtonTextBoxCodIVAContrap.Text == " " || radButtonTextBoxCodIVAContrap.Text == "") &&
                        (radButtonTextBoxCtaMayorContrap.Text == " " || radButtonTextBoxCtaMayorContrap.Text == "")) cuecci = "";
                    else
                    {
                        if (this.codigoIVACont != null && this.codigoIVACont != "") cuecci = "+" + this.codigoIVACont;
                    }
                }
                
                coiaci = " ";
                if (this.codigoIVAAsociado != null && this.codigoIVAAsociado != "") coiaci = this.codigoIVAAsociado;

                //Buscar los valores del porcentaje del tipo de iva y del recargo de equivalencia
                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    //SMR if (!TodaFilaEnBlanco(this.dtTiposIVAGrid, i))
                    if (!TodaFilaEnBlanco(this.radGridViewTiposIVA, i))
                    {
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value != null) tpivci = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString();
                        else tpivci = "";
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value != null) recgci = this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString();
                        else recgci = "";
                        break;
                    }
                }

                tpivci = tpivci.Replace(',', '.');
                
                if (recgci == "") recgci = "0";
                else recgci = recgci.Replace(',', '.');

                mxajci = mxajci.Replace(',', '.');
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Claves Legales que se insertarán o actualizarán en las tablas IVTA01 e IVTA02
        /// </summary>
        /// <param name="civmai"></param>
        /// <param name="civaai"></param>
        /// <param name="libra2"></param>
        /// <param name="copea2"></param>
        /// <param name="topea2"></param>
        /// <param name="ind4a2"></param>
        /// <param name="ind1a2"></param>
        /// <param name="ind2a2"></param>
        /// <param name="ind3a2"></param>
        /// <returns></returns>
        /// 
        private string ClavesLegalesFormToTabla(ref string civmai, ref string civaai, ref string libra2, ref string copea2, ref string topea2, 
                                                ref string ind4a2, ref string ind1a2, ref string ind2a2, ref string ind3a2)
        {
            string result = "";

            try
            {
                /*if (this.nuevo)
                {
                    civmai = this.DevuelveCodigo(this.tgTexBoxSelCLMod303GrupoOperac.Textbox.Text);
                    civaai = this.DevuelveCodigo(this.tgTexBoxSelCLMod390GrupoOperac.Textbox.Text);
                    libra2 = this.DevuelveCodigo(this.tgTexBoxSelCLMod340LibroRegistro.Textbox.Text);
                    copea2 = this.DevuelveCodigo(this.tgTexBoxSelCLMod340ClaveOperac.Textbox.Text); 
                    topea2 = this.DevuelveCodigo(this.tgTexBoxSelCLMod349TipoOperac.Textbox.Text);
                    ind4a2 = this.DevuelveCodigo(this.tgTexBoxSelCLMod190ClavePercep.Textbox.Text);
                    ind2a2 = this.DevuelveCodigo(this.tgTexBoxSelCLMod111ClavePercepss.Textbox.Text);
                }
                else
                {*/
                if (this.codigoCLMod303GrupoOperac ==null)
                {
                    civmai = "";
                }
                    else
                {
                    civmai = this.codigoCLMod303GrupoOperac.Trim();
                }
                if (this.codigoCLMod390GrupoOperac == null)
                {
                    civaai = "";
                }
                else
                {
                    civaai = this.codigoCLMod390GrupoOperac.Trim();
                }
                if (this.codigoCLMod340LibroRegistro == null)
                {
                    libra2 = "";
                }
                else
                {
                    libra2 = this.codigoCLMod340LibroRegistro.Trim();
                }
                if (this.codigoCLMod340ClaveOperac == null)
                {
                    copea2 = "";
                }
                else
                {
                    copea2 = this.codigoCLMod340ClaveOperac.Trim();
                }
                if (this.codigoCLMod349TipoOperac == null)
                {
                    topea2 = "";
                }
                else
                {
                    topea2 = this.codigoCLMod349TipoOperac.Trim();
                }
                if (this.codigoCLMod190ClavePercep == null)
                {
                    ind4a2 = "";
                }
                else
                {
                    ind4a2 = this.codigoCLMod190ClavePercep.Trim();
                }
                if (this.cmbCLMod190SubclavePercep.Text == null)
                {
                    ind1a2 = "";
                }
                else
                {
                    ind1a2 = this.cmbCLMod190SubclavePercep.Text.Trim();
                }
                if (this.codigoCLMod111ClavePercep == null)
                {
                    ind2a2 = "";
                }
                else
                {
                    ind2a2 = this.codigoCLMod111ClavePercep.Trim();
                }
                
                //}

                civmai = (civmai == "") ? " " : civmai;

                civaai = (civaai == "") ? " " : civaai;
                
                libra2 = (libra2 == "") ? " " : libra2;

                copea2 = (copea2 == "") ? " " : copea2;

                ind3a2 = "0";
                if (this.rbCLDinerario.IsChecked) ind3a2 = "1";
                else if (this.rbCLEspecie.IsChecked) ind3a2 = "2";

                topea2 = (topea2 == "") ? " " : topea2;
                
                ind4a2 = (ind4a2 == "") ? " " : ind4a2;
                
                ind1a2 = this.cmbCLMod190SubclavePercep.Text.Trim();
                ind1a2 = (ind1a2 == "") ? " " : ind1a2;

                ind2a2 = (ind2a2 == "") ? " " : ind2a2;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado Claves Legales (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Sii Libro que se insertarán o actualizarán en las tablas IVTA01 e IVTA02
        /// </summary>
        /// <returns></returns>
        /// 
        private string SiiLibroFormToTabla(ref string copea3, ref string cop1a3, ref string cop2a3, ref string tfaca3, 
                                           ref string tdsga3, ref string texea3, ref string tsnea3, ref string tnsja3, 
                                           ref string tfara3, ref string topia3, ref string binva3, ref string segua3, 
                                           ref string comea3, ref string nacua3, ref string prtaa3, ref decimal tivpa3, 
                                           ref string ind1a3, ref string ind2a3, ref string filla3, ref string cupoa3)
        {
            string result = "";
            
            try
            {
                string siiLibro = this.cmbSiiLibros.SelectedIndex.ToString();

                switch (siiLibro)
                {
                    case "1":   //Facturas expedidas
                        copea3 = sii_factE_R_claveRegEspeTrasc;
                        cop1a3 = sii_factE_R_claveRegEspeTrascAdicional1;
                        cop2a3 = sii_factE_R_claveRegEspeTrascAdicional2;
                        tfaca3 = sii_factE_R_tipoFactura;
                        tdsga3 = sii_factE_R_tipoDesglose;
                        texea3 = sii_factE_causaExencionOpSujetasExentas;
                        tsnea3 = sii_factE_tipoOperacionSujetaNoExenta;
                        tnsja3 = sii_factE_tipoOperacNoSujeta;
                        tfara3 = sii_factE_R_tipoFacturaRectif;
                        topia3 = " ";
                        binva3 = " ";
                        segua3 = " ";
                        // comea3 = sii_factE_cobroMetalico;
                        comea3 = (this.radToggleSwitchSiiCobroMetalico.Value) ? "S" : "N";
                        cupoa3 = (this.radToggleSwitchSiiCupon.Value) ? "S" : "N";
                        nacua3 = " ";
                        prtaa3 = " ";
                        tivpa3 = 0;
                        this.sii_factE_taxFree = (this.radToggleSwitchSiiTaxFree.Value) ? "S" : "N";
                        this.sii_factE_viajeros = (this.radToggleSwitchSiiCobroMetalico.Value) ? "S" : "N";
                        this.sii_factE_R_factSimplArt72 = (this.radToggleSwitchSiiFactSimpArt72_73.Value) ? "S" : "N";
                        this.sii_factE_viajeros= (this.radToggleSwitchSiiViajeros.Value) ? "S" : "N";
                        if (this.sii_agenciaTributaria == "") this.sii_agenciaTributaria = " ";
                        ind1a3 = this.sii_factE_taxFree + this.sii_factE_viajeros;
                        ind2a3 = this.sii_factE_R_factSimplArt72 + this.sii_agenciaTributaria;
                        filla3 = this.radButtonTextBoxSiiCampoLibreExp.Text;
                        break;
                    case "2":   //Facturas recibidas
                        copea3 = sii_factE_R_claveRegEspeTrasc;
                        cop1a3 = sii_factE_R_claveRegEspeTrascAdicional1;
                        cop2a3 = sii_factE_R_claveRegEspeTrascAdicional2;
                        tfaca3 = sii_factE_R_tipoFactura;
                        tdsga3 = sii_factE_R_tipoDesglose;
                        texea3 = " ";
                        tsnea3 = " ";
                        tnsja3 = " ";
                        tfara3 = sii_factE_R_tipoFacturaRectif;
                        topia3 = " ";
                        //binva3 = sii_factR_bienesInversion;
                        binva3 = (this.radToggleSwitchSiiBienesInversionFactRecibida.Value) ? "S" : "N";
                        segua3 = " ";
                        comea3 = " ";
                        cupoa3 = " ";
                        // nacua3 = sii_factR_excluirBaseTotalFacturaProrrata;
                        nacua3 = (this.radToggleSwitchSiiExcluirBaseTotalProrrataFactRecibida.Value) ? "S" : "N";
                        // prtaa3 = sii_factR_prorrata;
                        prtaa3 = (this.radToggleSwitchSiiProrrataFactRecibida.Value) ? "S" : "N";
                        //tivpa3 = sii_factR_tipoImpositivoProrrata;
                        if (txtSiiTipoImpositivoProrrataFactRecibida.Text == "") txtSiiTipoImpositivoProrrataFactRecibida.Text = "0"; 
                        tivpa3 = Convert.ToDecimal(txtSiiTipoImpositivoProrrataFactRecibida.Text);
                        ind1a3 = " ";
                        this.sii_factE_R_factSimplArt72 = (this.radToggleSwitchSiiFactSimplificadaArticulo72FactRecibida.Value) ? "S" : "N";
                        if (this.sii_factE_R_factSimplArt72 == "") ind2a3 = " " + this.sii_agenciaTributaria;
                        else ind2a3 = this.sii_factE_R_factSimplArt72 + this.sii_agenciaTributaria;
                        // ind2a3 = this.sii_factE_R_factSimplArt72 + this.sii_agenciaTributaria; jl
                        filla3 = this.radButtonTextBoxSiiCampoLibreRec.Text;
                        break;
                    case "3":   //Operaciones de seguros
                        copea3 = " ";
                        cop1a3 = " ";
                        cop2a3 = " ";
                        tfaca3 = " ";
                        tdsga3 = " ";
                        texea3 = " ";
                        tsnea3 = " ";
                        tnsja3 = " ";
                        tfara3 = " ";
                        topia3 = " ";
                        binva3 = " ";
                        segua3 = sii_opSeg_tipoOperacion;
                        comea3 = " ";
                        cupoa3 = " ";
                        nacua3 = " ";
                        prtaa3 = " ";
                        tivpa3 = 0;
                        ind1a3 = " ";
                        ind2a3 = " " + this.sii_agenciaTributaria;
                        break;
                    case "4":   //Operaciones intracomunitarias
                        copea3 = " ";
                        cop1a3 = " ";
                        cop2a3 = " ";
                        tfaca3 = " ";
                        tdsga3 = " ";
                        texea3 = " ";
                        tsnea3 = " ";
                        tnsja3 = " ";
                        tfara3 = " ";
                        topia3 = sii_opIntracom_tipoOperacion;
                        binva3 = " ";
                        segua3 = " ";
                        comea3 = " ";
                        cupoa3 = " ";
                        nacua3 = " ";
                        prtaa3 = " ";
                        tivpa3 = 0;
                        ind1a3 = " ";
                        ind2a3 = " " + this.sii_agenciaTributaria;
                        break;   
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado Sii Libro (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a un código de IVA
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string nombci = "";
                string resoci = "";
                string deduci = "";
                string cuenci = "";
                string tauxci = "";
                string librci = "";
                string serici = "";
                string cuadci = "";
                string mxajci = "";
                string cuecci = "";
                string coiaci = "";
                string tpivci = "0";
                string recgci = "0";
               
                //Obtener los valores del formulario general que se insertarán en la tabla IVT01
                result = this.GeneralDatosFormToTabla(ref nombci, ref resoci, ref deduci, ref cuenci, ref tauxci, 
                                               ref librci, ref serici, ref cuadci, ref mxajci, ref cuecci,
                                               ref coiaci, ref tpivci, ref recgci);

                if (result == "")
                {   
                    string errorMsg = "";
                    DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivel(cuenci, this.codigoPlan, ref errorMsg);

                    if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0 && cuenci !="" && cuenci != " ")
                    {
                        cuenci = dtCtasUltimoNivel.Rows[dtCtasUltimoNivel.Rows.Count - 1]["CUENMC"].ToString().Trim();
                    }

                    string estado = (this.radToggleSwitchEstadoActivo.Value) ? estado = "V" : estado = "*";

                    //Dar de alta a un código de IVA en la IVT01
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "IVT01";
                    string query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "STATCI, TIPLCI, COIVCI, RESOCI, DEDUCI, CUENCI, TAUXCI, TPIVCI, RECGCI, LIBRCI, SERICI, CUADCI, ";
                    query += "MXAJCI, CUECCI, COIACI, NOMBCI) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "'" + estado + "', '" + this.codigoPlan + "', '" + this.codigo + "', '";
                    query += resoci + "', '" + deduci + "', '" + cuenci + "', '";
                    query += tauxci + "', " + tpivci + ", " + recgci + ", '" + librci + "', '" + serici  + "', '";
                    query += cuadci + "', " + mxajci + ", '" + cuecci + "', '" + coiaci + "', '" + nombci + "')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "IVT01", this.codigoPlan, this.codigo);

                    //Dar de alta a los Tipos de IVA
                    this.AltaActualizaInfoTiposIVA();

                    //Dar alta a las claves legales
                    this.AltaInfoClavesLegales();

                    //Dar de alta al libro SII
                    this.AltaInfoSiiLibro();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta las claves legales en las tablas IVTA1 e IVTA2
        /// </summary>
        /// <param name="civmai"></param>
        /// <param name="civaai"></param>
        /// <param name="libra2"></param>
        /// <param name="copea2"></param>
        /// <param name="topea2"></param>
        /// <param name="ind4a2"></param>
        /// <param name="ind1a2"></param>
        /// <param name="ind2a2"></param>
        /// <param name="ind3a2"></param>
        /// <returns></returns>
        private string AltaInfoIVTA(string civmai, string civaai, string libra2, string copea2, string topea2,
                                    string ind4a2, string ind1a2, string ind2a2, string ind3a2)
        {
            string result = "";

            try
            {
                //Insertar en la tabla IVTA1
                string tipoai = " ";
                string coprai = " ";
                string fillai = " ";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVTA1";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLAI, COIVAI, TIPOAI, COPRAI, CIVMAI, CIVAAI, FILLAI) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigoPlan + "', '" + this.codigo + "', '" + tipoai + "', '";
                query += coprai + "', '" + civmai + "', '" + civaai + "', '";
                query += fillai + "')";
                
                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                nombreTabla = GlobalVar.PrefijoTablaCG + "IVTA2";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLA2, COIVA2, LIBRA2, TOPEA2, COPEA2, IND1A2, IND2A2, IND3A2, IND4A2) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigoPlan + "', '" + this.codigo + "', '" + libra2 + "', '";
                query += topea2 + "', '" + copea2 + "', '" + ind1a2 + "', '" + ind2a2 + "', '";
                query += ind3a2 + "', '" + ind4a2 + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta/actualizar los tipos de IVA
        /// </summary>
        /// <returns></returns>
        private string AltaActualizaInfoTiposIVA()
        {
            string result = "";
            string query = "";
            int cantRegistros = 0;

            try
            {
                string fechaEfectiva = "";
                string porcIVA = "";
                string porcRecEquiv = "";
                string descripcion = "";

                try
                {
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "IVTX1 ";
                    query += "where TIPLCX = '" + this.codigoPlan + "' and COIVCX = '" + this.codigo + "'";

                    cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    //if (!TodaFilaEnBlanco(this.dtTiposIVAGrid, i))
                    if (!TodaFilaEnBlanco(this.radGridViewTiposIVA, i))
                    {
                        fechaEfectiva = this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value.ToString();
                        porcIVA = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString();

                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value != null) porcRecEquiv = this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString();
                        else porcRecEquiv = "";

                        if (this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value != null) descripcion = this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value.ToString();
                        else descripcion = "";

                        porcIVA = porcIVA.Replace(',', '.');
                        porcRecEquiv = porcRecEquiv.Replace(',', '.');

                        
                            if (porcRecEquiv.Trim() == "") porcRecEquiv = "0";
                            result = this.AltaInfoIVTX1(fechaEfectiva, porcIVA, porcRecEquiv, descripcion);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los tipos de IVA (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta/actualizar los tipos de IVA
        /// </summary>
        /// <returns></returns>
        private string AltaActualizaInfoTiposIVA_Ant()
        {
            string result = "";

            try
            {
                string fechaEfectiva = "";
                string porcIVA = "";
                string porcRecEquiv = "";
                string descripcion = "";

                string fechaEfectivaOrigen = "";
                string porcIVAOrigen = "";
                string porcRecEquivOrigen = "";
                string descripcionOrigen = "";

                for (int i = 0; i < this.radGridViewTiposIVA.Rows.Count; i++)
                {
                    //if (!TodaFilaEnBlanco(this.dtTiposIVAGrid, i))
                    if (!TodaFilaEnBlanco(this.radGridViewTiposIVA, i))
                    {
                        fechaEfectiva = this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectiva"].Value.ToString();
                        porcIVA = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVA"].Value.ToString();

                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value != null) porcRecEquiv = this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquiv"].Value.ToString();
                        else porcRecEquiv = "";

                        if (this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value != null) descripcion = this.radGridViewTiposIVA.Rows[i].Cells["Descripcion"].Value.ToString();
                        else descripcion = "";

                        if (this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value != null)
                            fechaEfectivaOrigen = this.radGridViewTiposIVA.Rows[i].Cells["FechaEfectivaOrigen"].Value.ToString();
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcIVAOrigen"].Value != null)
                            porcIVAOrigen = this.radGridViewTiposIVA.Rows[i].Cells["PorcIVAOrigen"].Value.ToString();
                        if (this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquivOrigen"].Value != null)
                            porcRecEquivOrigen = this.radGridViewTiposIVA.Rows[i].Cells["PorcRecEquivOrigen"].Value.ToString();
                        if (this.radGridViewTiposIVA.Rows[i].Cells["DescripcionOrigen"].Value != null)
                            descripcionOrigen = this.radGridViewTiposIVA.Rows[i].Cells["DescripcionOrigen"].Value.ToString();

                        porcIVA = porcIVA.Replace(',', '.');
                        porcRecEquiv = porcRecEquiv.Replace(',', '.');

                        if (fechaEfectivaOrigen == "" && porcIVAOrigen == "" && porcRecEquivOrigen == "" && descripcionOrigen == "")
                        {
                            if (porcRecEquiv.Trim() == "") porcRecEquiv = "0";
                            result = this.AltaInfoIVTX1(fechaEfectiva, porcIVA, porcRecEquiv, descripcion);
                        }
                        else
                        {
                            porcIVAOrigen = porcIVAOrigen.Replace(',', '.');
                            porcRecEquivOrigen = porcRecEquivOrigen.Replace(',', '.');

                            if (porcRecEquiv.Trim() == "") porcRecEquiv = "0";

                            result = this.ActualizarInfoIVTX1(fechaEfectiva, porcIVA, porcRecEquiv, descripcion,
                                                              fechaEfectivaOrigen, porcIVAOrigen, porcRecEquivOrigen, descripcionOrigen);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los tipos de IVA (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta las claves del libro del SII en las tabla IVTA3
        /// </summary>
        /// <returns></returns>
        private string AltaInfoIVTA3(string copea3, string cop1a3, string cop2a3, string tfaca3,
                                     string tdsga3, string texea3, string tsnea3, string tnsja3,
                                     string tfara3, string topia3, string binva3, string segua3,
                                     string comea3, string nacua3, string prtaa3, decimal tivpa3,
                                     string ind1a3, string ind2a3, string filla3, string cupoa3)
        {
            string result = "";

            try
            {
                string tdoca3 = this.LibroSIIActivoCodigo();

                //Insertar en la tabla IVTA3
                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVTA3";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLA3, COIVA3, TDOCA3, COPEA3, COP1A3, COP2A3, TFACA3, TDSGA3, TEXEA3, TSNEA3, ";
                query += "TNSJA3, TFARA3, TOPIA3, BINVA3, SEGUA3, COMEA3, NACUA3, PRTAA3, TIVPA3, IND1A3, ";
                query += "IND2A3, FILLA3, CUPOA3) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigoPlan + "', '" + this.codigo + "', '" + tdoca3 + "', '";
                query += copea3 + "', '" + cop1a3 + "', '" + cop2a3 + "', '" + tfaca3 + "', '" + tdsga3 + "', '";
                query += texea3 + "', '" + tsnea3 + "', '" + tnsja3 + "', '" + tfara3 + "', '" + topia3 + "', '";
                query += binva3 + "', '" + segua3 + "', '" + comea3 + "', '" + nacua3 + "', '" + prtaa3 + "', ";
                query += tivpa3 + ", '" + ind1a3 + "', '" + ind2a3 + "', '" + filla3 + "', '" + cupoa3 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }


        /// <summary>
        /// Inserta el tipo de IVA en la tabla IVTX1
        /// </summary>
        /// <param name="fechaEfectiva"></param>
        /// <param name="porcIVA"></param>
        /// <param name="porcRecEquiv"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        private string AltaInfoIVTX1(string fechaEfectiva, string porcIVA, string porcRecEquiv, string descripcion)
        {
            string result = "";

            if (porcIVA == null) porcIVA = "0";
            if (porcRecEquiv == null) porcRecEquiv = "0";

            try
            {
                //Insertar en la tabla IVTX1
                DateTime dt = Convert.ToDateTime(fechaEfectiva);
                int fechaEfectivaCG = utiles.FechaToFormatoCG(dt, true);

                string nombreTabla = GlobalVar.PrefijoTablaCG + "IVTX1";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATCX, TIPLCX, COIVCX, FEIVCX, TPIVCX, RECGCX, NOMBCX) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'V', '" + this.codigoPlan + "', '" + this.codigo + "', " + fechaEfectivaCG.ToString() + ", ";
                query += porcIVA + ", " + porcRecEquiv+ ", '" + descripcion+ "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta las claves legales
        /// </summary>
        /// <returns></returns>
        private string AltaInfoClavesLegales()
        {
            string result = "";

            try
            {
                string civmai = "";
                string civaai = "";
                string libra2 = "";
                string copea2 = "";
                string topea2 = "";
                string ind4a2 = "";
                string ind1a2 = "";
                string ind2a2 = "";
                string ind3a2 = "";

                //Obtener los valores del formulario del apartado Datos Fiscales que se insertarán en las tablas IVTA1, IVTA2
                result = this.ClavesLegalesFormToTabla(ref civmai, ref civaai, ref libra2, ref copea2, ref topea2,
                                                       ref ind4a2, ref ind1a2, ref ind2a2, ref ind3a2);

                if (result == "") result = this.AltaInfoIVTA(civmai, civaai, libra2, copea2, topea2, ind4a2, ind1a2, ind2a2, ind3a2);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los claves legales (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta al libbro de Facturas del Sii
        /// </summary>
        /// <returns></returns>
        private string AltaInfoSiiLibro()
        {
            string result = "";

            try
            {
                string copea3 = "";
                string cop1a3 = "";
                string cop2a3 = "";
                string tfaca3 = "";
                string tdsga3 = "";
                string texea3 = "";
                string tsnea3 = "";
                string tnsja3 = "";
                string tfara3 = "";
                string topia3 = "";
                string binva3 = "";
                string segua3 = "";
                string comea3 = "";
                string cupoa3 = "";
                string nacua3 = "";
                string prtaa3 = "";
                decimal tivpa3 = 0;
                string ind1a3 = "";
                string ind2a3 = "";
                string filla3 = "";

                //Obtener los valores del formulario del apartado Sii Libro que se insertarán en las tabla IVTA3
                result = this.SiiLibroFormToTabla(ref copea3, ref cop1a3, ref cop2a3, ref tfaca3, ref tdsga3, 
                                                  ref texea3, ref tsnea3, ref tnsja3, ref tfara3, ref topia3,
                                                  ref binva3, ref segua3, ref comea3, ref nacua3, ref prtaa3,
                                                  ref tivpa3, ref ind1a3, ref ind2a3, ref filla3, ref cupoa3);

                if (result == "") result = this.AltaInfoIVTA3(copea3, cop1a3, cop2a3, tfaca3, tdsga3, texea3, tsnea3, 
                                                              tnsja3, tfara3, topia3, binva3, segua3, comea3, nacua3, 
                                                              prtaa3, tivpa3, ind1a3, ind2a3, filla3,cupoa3);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando el libro del SII (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        #endregion

        #region Actualizar código de IVA
        /// <summary>
        /// Actualizar un código de IVA
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string nombci = "";
                string resoci = "";
                string deduci = "";
                string cuenci = "";
                string tauxci = "";
                string librci = "";
                string serici = "";
                string cuadci = "";
                string mxajci = "";
                string cuecci = "";
                string coiaci = "";
                string tpivci = "";
                string recgci = "";

                //Obtener los valores del formulario general que se insertarán en la tabla IVT01
                result = this.GeneralDatosFormToTabla(ref nombci, ref resoci, ref deduci, ref cuenci, ref tauxci,
                                                      ref librci, ref serici, ref cuadci, ref mxajci, ref cuecci, ref coiaci,
                                                      ref tpivci, ref recgci);

                if (result == "")
                {
                    if (tpivci == "") tpivci = "0";
                    if (recgci == "") recgci = "0";

                    string estado = (this.radToggleSwitchEstadoActivo.Value) ? estado = "V" : estado = "*";

                    //Actualizar el código de IVA en la tabla IVT01
                    string query = "update " + GlobalVar.PrefijoTablaCG + "IVT01 set ";
                    query += "STATCI = '" + estado + "', ";
                    query += "RESOCI = '" + resoci + "', ";
                    query += "DEDUCI = '" + deduci + "', ";
                    query += "CUENCI = '" + cuenci + "', ";
                    query += "TAUXCI = '" + tauxci + "', ";
                    query += "TPIVCI = " + tpivci + ", ";
                    query += "RECGCI = " + recgci + ", ";
                    query += "LIBRCI = '" + librci + "', ";
                    query += "SERICI = '" + serici + "', ";
                    query += "CUADCI = '" + cuadci + "', ";
                    query += "MXAJCI = " + mxajci + ", ";
                    query += "CUECCI = '" + cuecci + "', ";
                    query += "COIACI = '" + coiaci + "', ";
                    query += "NOMBCI = '" + nombci + "' ";
                    query += "where TIPLCI = '" + this.codigoPlan + "' and COIVCI = '" + this.codigo + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "IVT01", this.codigoPlan, this.codigo);

                    //Dar de alta/actualizar los Tipos de IVA
                    result += AltaActualizaInfoTiposIVA();

                    if (ClavesLegalesCambio())
                    {
                        if (this.existeCodigoIVA)
                        {
                            //Actualizar las claves legales
                            result += this.ActualizarInfoClavesLegales();
                        }
                        else
                        {
                            //Insertar las claves legales
                            result += this.AltaInfoClavesLegales();
                        }
                    }

                    if (this.SiiLibroCambio())
                    {
                        if (this.SiiLibroExisteCodigoIVA())
                        {
                            //Actualizar el Libro del SII
                            result += this.ActualizarInfoSiiLibro();
                        }
                        else
                        {
                            //Insertar el Libro del SII
                            result += this.AltaInfoSiiLibro();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errActDatos", "Error actualizando los datos") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Inserta la clave legal en las tablas IVTA1, IVTA2
        /// </summary>
        /// <param name="civmai"></param>
        /// <param name="civaai"></param>
        /// <param name="libra2"></param>
        /// <param name="copea2"></param>
        /// <param name="topea2"></param>
        /// <param name="ind4a2"></param>
        /// <param name="ind1a2"></param>
        /// <param name="ind2a2"></param>
        /// <param name="ind3a2"></param>
        /// <returns></returns>
        private string AlctualizarInfoIVTA(string civmai, string civaai, string libra2, string copea2, string topea2,
                                           string ind4a2, string ind1a2, string ind2a2, string ind3a2)
        {
            string result = "";

            try
            {
                string tipoai = "I";
                string coprai = " ";
                string fillai = " ";

                string query = "update " + GlobalVar.PrefijoTablaCG + "IVTA1 set ";
                query += "TIPOAI = '" + tipoai + "', ";
                query += "COPRAI = '" + coprai + "', ";
                query += "CIVMAI = '" + civmai + "', ";
                query += "CIVAAI = '" + civaai + "', ";
                query += "FILLAI = '" + fillai + "' ";
                query += "where TIPLAI = '" + this.codigoPlan + "' and COIVAI = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                query = "update " + GlobalVar.PrefijoTablaCG + "IVTA2 set ";
                query += "LIBRA2 = '" + libra2 + "', ";
                query += "TOPEA2 = '" + topea2 + "', ";
                query += "COPEA2 = '" + copea2 + "', ";
                query += "IND1A2 = '" + ind1a2 + "', ";
                query += "IND2A2 = '" + ind2a2 + "', ";
                query += "IND3A2 = '" + ind3a2 + "', ";
                query += "IND4A2 = '" + ind4a2 + "' ";
                query += "where TIPLA2 = '" + this.codigoPlan + "' and COIVA2 = '" + this.codigo + "'";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualizar el tipo de IVA en la tabla IVTX1
        /// </summary>
        /// <param name="fechaEfectiva"></param>
        /// <param name="porcIVA"></param>
        /// <param name="porcRecEquiv"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaEfectivaOrigen"></param>
        /// <param name="porcIVAOrigen"></param>
        /// <param name="porcRecEquivOrigen"></param>
        /// <param name="descripcionOrigen"></param>
        /// <returns></returns>
        private string ActualizarInfoIVTX1(string fechaEfectiva, string porcIVA, string porcRecEquiv, string descripcion,
                                           string fechaEfectivaOrigen, string porcIVAOrigen, string porcRecEquivOrigen, string descripcionOrigen)
        {
            string result = "";

            try
            {
                if (porcRecEquiv == "") porcRecEquiv = "0";
                if (porcRecEquivOrigen == "") porcRecEquivOrigen = "0";

                //Insertar en la tabla IVTA1
                porcIVAOrigen = porcIVAOrigen.Replace(',', '.');
                porcRecEquivOrigen = porcRecEquivOrigen.Replace(',', '.');

                DateTime dt = Convert.ToDateTime(fechaEfectiva);
                int fechaEfectivaCG = utiles.FechaToFormatoCG(dt, true);

                dt = Convert.ToDateTime(fechaEfectivaOrigen);
                int fechaEfectivaOrigenCG = utiles.FechaToFormatoCG(dt, true);

                string query = "update " + GlobalVar.PrefijoTablaCG + "IVTX1 set ";
                query += "FEIVCX = " + fechaEfectivaCG.ToString() + ", ";
                query += "TPIVCX = " + porcIVA + ", ";
                query += "RECGCX = '" + porcRecEquiv + "', ";
                query += "NOMBCX = '" + descripcion + "' ";
                query += "where TIPLCX = '" + this.codigoPlan + "' and COIVCX = '" + this.codigo + "' and ";
                query += "FEIVCX = " + fechaEfectivaOrigenCG.ToString() + " and TPIVCX = " + porcIVAOrigen + " and ";
                query += "RECGCX = " + porcRecEquivOrigen + " and NOMBCX = '" + descripcionOrigen + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualizar las claves legales del código de IVA
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoClavesLegales()
        {
            string result = "";

            try
            {
                string civmai = "";
                string civaai = "";
                string libra2 = "";
                string copea2 = "";
                string topea2 = "";
                string ind4a2 = "";
                string ind1a2 = "";
                string ind2a2 = "";
                string ind3a2 = "";

                //Obtener los valores del formulario del apartado Claves Legales que se insertarán en las tablas IVTA1, IVTA2
                result = this.ClavesLegalesFormToTabla(ref civmai, ref civaai, ref libra2, ref copea2, ref topea2,
                                                       ref ind4a2, ref ind1a2, ref ind2a2, ref ind3a2);

                if (result == "") result = this.AlctualizarInfoIVTA(civmai, civaai, libra2, copea2, topea2, ind4a2, ind1a2, ind2a2, ind3a2);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errActClavesLegales", "Error actualizando las claves legales") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Actualiza las claves del libro del SII en las tabla IVTA3
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoIVTA3(string copea3, string cop1a3, string cop2a3, string tfaca3,
                                            string tdsga3, string texea3, string tsnea3, string tnsja3,
                                            string tfara3, string topia3, string binva3, string segua3,
                                            string comea3, string nacua3, string prtaa3, decimal tivpa3,
                                            string ind1a3, string ind2a3, string filla3, string cupoa3)
        {
            string result = "";

            try
            {
                string tdoca3 = this.LibroSIIActivoCodigo();
                
                string query = "update " + GlobalVar.PrefijoTablaCG + "IVTA3 set ";
                query += "COPEA3 = '" + copea3 + "', ";
                query += "COP1A3 = '" + cop1a3 + "', ";
                query += "COP2A3 = '" + cop2a3 + "', ";
                query += "TFACA3 = '" + tfaca3 + "', ";
                query += "TDSGA3 = '" + tdsga3 + "', ";
                query += "TEXEA3 = '" + texea3 + "', ";
                query += "TSNEA3 = '" + tsnea3 + "', ";
                query += "TNSJA3 = '" + tnsja3 + "', ";
                query += "TFARA3 = '" + tfara3 + "', ";
                query += "TOPIA3 = '" + topia3 + "', ";
                query += "BINVA3 = '" + binva3 + "', ";
                query += "SEGUA3 = '" + segua3 + "', ";
                query += "COMEA3 = '" + comea3 + "', ";
                query += "NACUA3 = '" + nacua3 + "', ";
                query += "PRTAA3 = '" + prtaa3 + "', ";
                query += "TIVPA3 = '" + tivpa3 + "', ";
                query += "IND1A3 = '" + ind1a3 + "', "; 
                //query += "CUPOA3 = '" + cupoa3 + "', "; jl
                query += "IND2A3 = '" + ind2a3 + "', ";
                query += "FILLA3 = '" + filla3 + "', ";
                query += "CUPOA3 = '" + cupoa3 + "' ";

                query += "where TIPLA3 = '" + this.codigoPlan + "' and COIVA3 = '" + this.codigo + "' and ";
                query += "TDOCA3 = '" + tdoca3 + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualizar el libro del SII
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoSiiLibro()
        {
            string result = "";

            try
            {
                string copea3 = "";
                string cop1a3 = "";
                string cop2a3 = "";
                string tfaca3 = "";
                string tdsga3 = "";
                string texea3 = "";
                string tsnea3 = "";
                string tnsja3 = "";
                string tfara3 = "";
                string topia3 = "";
                string binva3 = "";
                string segua3 = "";
                string comea3 = "";
                string cupoa3 = "";
                string nacua3 = "";
                string prtaa3 = "";
                decimal tivpa3 = 0;
                string ind1a3 = "";
                string ind2a3 = "";
                string filla3 = "";

                //Obtener los valores del formulario del apartado Sii Libro que se insertarán en las tabla IVTA3
                result = this.SiiLibroFormToTabla(ref copea3, ref cop1a3, ref cop2a3, ref tfaca3, ref tdsga3,
                                                  ref texea3, ref tsnea3, ref tnsja3, ref tfara3, ref topia3,
                                                  ref binva3, ref segua3, ref comea3, ref nacua3, ref prtaa3,
                                                  ref tivpa3, ref ind1a3, ref ind2a3, ref filla3, ref cupoa3);

                if (result == "") result = this.ActualizarInfoIVTA3(copea3, cop1a3, cop2a3, tfaca3, tdsga3, texea3, 
                                                                    tsnea3, tnsja3, tfara3, topia3, binva3, segua3, 
                                                                    comea3, nacua3, prtaa3, tivpa3, ind1a3, ind2a3, 
                                                                    filla3,cupoa3);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errActLibroSII", "Error actualizando el libro del SII") + " (" + ex.Message + ")";
            }

            return (result);
        }
        #endregion


        /// <summary>
        /// Devuelve true -> si el código de IVA no se está utilizando en ningín comprobante (GLB01)  false -> en caso contrario
        /// </summary>
        /// <returns></returns>
        private bool CodigoIVAPermiteModificarEliminar()
        {
            bool result = true;

            try
            {
                string query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where TIPLDT = '" + this.codigoPlan + "' and CDIVDT = '" + this.codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }

        /// <summary>
        /// Devuelve true -> si la fecha del tipo del código de IVA no se está utilizando en ningín comprobante (GLB01)  false -> en caso contrario
        /// </summary>
        /// <returns></returns>
        private bool TipoIVAPermiteModificarEliminar(string fecha)
        {
            bool result = true;

            try
            {
                string query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where TIPLDT = '" + this.codigoPlan + "' and CDIVDT = '" + this.codigo + "' and ";
                query += "FDOCDT >= '" + fecha + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }

        /// <summary>
        /// Actualiza el valor de la celda PermiteModificar de la Grid
        /// 1-> permite modificar/eliminar el tipo de IVA
        /// 0-> no se permite (el código de IVA tiene movimientos asociados en la tabla GLB01)
        /// </summary>
        /// <param name="indice"></param>
        private void ActualizarRowPermiteModificar(int indice)
        {
            try
            {
                if (this.radGridViewTiposIVA.Rows[indice].Cells["PermiteModificar"].Value.ToString() == "1")
                {
                    if (!this.copiar)
                    {
                        //Verificar si existen comprobantes contables para ese codigo de IVA por fecha
                        string fechaOrigen = "";
                        if (this.radGridViewTiposIVA.Rows[indice].Cells["FechaEfectivaOrigen"].Value != null)
                            fechaOrigen = this.radGridViewTiposIVA.Rows[indice].Cells["FechaEfectivaOrigen"].Value.ToString();

                        if (fechaOrigen != "")
                        {
                            DateTime fechaDT;
                            string fechaCG = "0";
                            try
                            {
                                fechaDT = Convert.ToDateTime(fechaOrigen);
                                fechaCG = utiles.FechaToFormatoCG(fechaDT, true).ToString();
                                
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            if (fechaCG != "0")
                            {
                                if (!TipoIVAPermiteModificarEliminar(fechaCG))
                                {
                                    this.radGridViewTiposIVA.Rows[indice].Cells["PermiteModificar"].Value = "0";
                                }
                            }

                        }
                        else this.radGridViewTiposIVA.Rows[indice].Cells["PermiteModificar"].Value = "0";

                        /*if (this.radGridViewTiposIVA.Rows[indice].Cells["PermiteModificar"].Value.ToString() == "0")
                        {
                            this.radGridViewTiposIVA.Rows[indice].ReadOnly = true;
                        }*/
                    }
                }
            }
            catch (Exception ex)  { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve el código del libro del SII seleccionado en el desplegable de libro de facturas
        /// </summary>
        /// <returns></returns>
        private string LibroSIIActivoCodigo()
        {
            string result = "";
            switch (this.cmbSiiLibros.SelectedIndex)
            {
                case 1:
                    result = "E";
                    break;
                case 2:
                    result = "R";
                    break;
                case 3:
                    result = "S";
                    break;
                case 4:
                    result = "U";
                    break;
            }
            return (result);
        }

        public string ObtenerQuery(string tipcd, string resod)
        {
            string query = "";

            try
            {
                //Prefijo para la tabla de Descripciones (CGCDES)
                string prefijoTablaCGCDES = "";

                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2)
                {
                    prefijoTablaCGCDES = ConfigurationManager.AppSettings["bbddCGAPP"];
                    if (prefijoTablaCGCDES != null && prefijoTablaCGCDES != "") prefijoTablaCGCDES += ".";
                }
                else prefijoTablaCGCDES = GlobalVar.PrefijoTablaCG;

                query = "select CODCD, DESCD from " + prefijoTablaCGCDES + "CGCDES ";
                query += "where TIPCD = '" + tipcd + "' ";

                if (resod != null) query += "and RESOD = '" + resod + "' ";
                query += "and CODCD <> '00' order by CODCD";
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
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
        /// Valida los porcentajes (tipo de IVA y recargo de equivalencia)
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valorFormat"></param>
        /// <returns></returns>
        private string ValidarPorcentaje(string valor, ref string valorFormat)
        {
            string result = "";

            try
            {
                decimal porcentaje = Convert.ToDecimal(valor);

                if (porcentaje != 0)
                {
                    int parteEntera = Convert.ToInt32(porcentaje);

                    decimal parteDecimal = porcentaje - Convert.ToDecimal(parteEntera);

                    string parteEnteraStr = parteEntera.ToString();
                    if (parteEnteraStr.Length > 8) result = "El porcentaje no tiene formato correcto. La parte entera no puede tener más de 2 dígitos";     //Falta traducir
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        private void radGridViewTiposIVA_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            GridSpinEditor spinEditor = this.radGridViewTiposIVA.ActiveEditor as GridSpinEditor;
            if (spinEditor != null)
            {
                if (spinEditor.Value == null)
                {
                    spinEditor.Value = 0.00;
                }
            }
        }

        private void radButtonTextBoxCtaMayor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxTipoAux_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCtaMayorContrap_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCodIVAContrap_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCodIVAAsociado_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod303GrupoOperac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod390GrupoOperac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod340LibroRegistro_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod340ClaveOperac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod190ClavePercep_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCLMod111ClavePercep_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoFactura_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxlSiiTipoFacturaRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrascFactRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrasc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrascAdicional1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrascAdicional2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoFacturaRectif_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiAgenciaTributaria_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrascAdicional1FactRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiClaveRegEspTrascAdicional2FactRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoFactRectifFactRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiAgenciaTributariaFactRecibida_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoOpSeguro_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoOpIntracom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiAgenciaTributariaOpSeguro_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiAgenciaTributariaOpIntracom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoOperSujNoExe_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiCausaExeOperSujetaYExenta_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxSiiTipoOperacNoSujeta_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxlCLMod349TipoOperac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;

        }

        private void radDropDownListSiiTipoDesglose_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }
    }
}