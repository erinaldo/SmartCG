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
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM05 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCTAAUX";

        private bool nuevo;
        private string codigo;

        private string nombreCuentaAux;
        private string codigoTipoAux;
        private string nombreTipoAux;

        private string tipoDir;
        private string tipoMemo;

        private bool tipoAuxExterno;

        private bool soloConsulta = false;

        private string[] valoresSemana;
        private string[] valoresMes;

        //Zona
        private bool zonaJerarq;
        private string claseZonaNivel1;
        private string claseZonaNivel2;
        private string claseZonaNivel3;
        private string claseZonaNivel4;
        private string etiquetaZonaNivel1;
        private string etiquetaZonaNivel2;
        private string etiquetaZonaNivel3;
        private string etiquetaZonaNivel4;
        private int longZonaNivel1;
        private int longZonaNivel2;
        private int longZonaNivel3;
        private int longZonaNivel4;
        private bool validarZonaNivel1;
        private bool validarZonaNivel2;
        private bool validarZonaNivel3;
        private bool validarZonaNivel4;
        private string valorZonaNivel1;
        private string valorZonaNivel2;
        private string valorZonaNivel3;
        private string valorZonaNivel4;

        private string codigoPais;
        private string codigoGrupoCuentas;
        private string codigoZona;
        private string codigoReservadoUser;

        private string proveedorTipo;

        private const string autClaseElemento = "006";
        private const string autGrupo = "01";
        private const string autOperConsulta = "";        //Todos podrán siempre consultar
        private const string autOperModifica = "20";
        private const string autOperAlta = "10";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        private bool existenDatosDireccion = false;
        private bool existenDatosMemo = false;

        //Grupo de cuentas de auxiliar
        private const string autClaseElementoGLT08 = "007";
        private const string autGrupoGLT08 = "01";
        private const string autOperModificaGLT08 = "20";
        private const string autOperAltaGLT08 = "10";

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

        public string CodigoTipoAuxiliar
        {
            get
            {
                return (this.codigoTipoAux);
            }
            set
            {
                this.codigoTipoAux = value;
            }
        }

        public string NombreTipoAuxiliar
        {
            get
            {
                return (this.nombreTipoAux);
            }
            set
            {
                this.nombreTipoAux = value;
            }
        }

        public string NombreCuentaAuxiliar
        {
            get
            {
                return (this.nombreCuentaAux);
            }
            set
            {
                this.nombreCuentaAux = value;
            }
        }

        public bool TipoAuxExterno
        {
            get
            {
                return (this.tipoAuxExterno);
            }
            set
            {
                this.tipoAuxExterno = value;
            }
        }

        public frmMtoGLM05()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.gbZona.ElementTree.EnableApplicationThemeName = false;
            this.gbZona.ThemeName = "ControlDefault";

            this.gbFormaPago.ElementTree.EnableApplicationThemeName = false;
            this.gbFormaPago.ThemeName = "ControlDefault";

            this.gbMemoFormTerFormaPagoTipoPlazo.ElementTree.EnableApplicationThemeName = false;
            this.gbMemoFormTerFormaPagoTipoPlazo.ThemeName = "ControlDefault";

            this.gbMemoFormTerFormaPagoTipoDiaFijo.ElementTree.EnableApplicationThemeName = false;
            this.gbMemoFormTerFormaPagoTipoDiaFijo.ThemeName = "ControlDefault";

            this.gbDomiciliacionB.ElementTree.EnableApplicationThemeName = false;
            this.gbDomiciliacionB.ThemeName = "ControlDefault";

            //Eliminar los botones (close y navegación) del control RadPageView
            Telerik.WinControls.UI.RadPageViewStripElement stripElement = (Telerik.WinControls.UI.RadPageViewStripElement)this.radPageViewDatos.ViewElement;
            stripElement.StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmMtoGLM05_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Auxiliar Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Leer el proveedor de tipo de datos
            this.proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Control de selección del tipo de auxiliar
            this.radButtonTextBoxTipoAux.Text = this.nombreTipoAux;
            this.radButtonTextBoxTipoAux.Enabled = false;

            //Inicializar los desplegables
            string[] valores = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoNumPlazosPrimVto, ref valores);
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoAnoPrimVto, ref valores);

            valoresMes = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            valoresSemana = new string[] { "00", "01", "02", "03", "04", "05", "06" };
            //utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valores);
            //utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valores);
            //utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valores);
            this.rbMemoFormTerFormaPagoTipoDiaFijoMes.IsChecked = true;
            this.cmbMemoFormTerFormaPagoDiasFijos1.Items.Clear();
            this.cmbMemoFormTerFormaPagoDiasFijos2.Items.Clear();
            this.cmbMemoFormTerFormaPagoDiasFijos3.Items.Clear();
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valoresMes);
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valoresMes);
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valoresMes);
            this.cmbMemoFormTerFormaPagoDiasFijos1.SelectedIndex = 0;
            this.cmbMemoFormTerFormaPagoDiasFijos2.SelectedIndex = 0;
            this.cmbMemoFormTerFormaPagoDiasFijos3.SelectedIndex = 0;

            valores = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72" };
            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoNumPlazos, ref valores);

            //Obtiene los datos necesarios del tipo de auxiliar (zona, direccion, y habilita la pestaña que corresponda)
            this.ObtenerDatosTipoAuxiliar();

            this.codigoZona = "";

            //Verificar autorización sobre crear grupo de cuentas   ??????  Es OK ??
            this.radButtonCrearGrupoCuentas.Visible = aut.CrearElemento(autClaseElementoGLT08);

            if (this.tipoAuxExterno) this.lblExterno.Visible = true;
            else this.lblExterno.Visible = false;

            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                utiles.ButtonEnabled(ref this.radButtonCrearGrupoCuentas, false);
                
                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCtaAux;
                this.txtCtaAux.Select(0, 0);
                this.txtCtaAux.Focus();
            }
            else
            {
                this.txtCtaAux.Text = this.codigo;
                this.txtCtaAux.IsReadOnly = true;
                this.txtNombreCuentaAux.Text = this.nombreCuentaAux;

                //Recuperar la información del tipos de auxiliar y mostrarla en los controles
                this.CargarInfoCuentaAuxiliar();
               
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigoTipoAux, autOperModifica);
                this.autEditar = operarModificar;
                if (!operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombreCuentaAux;
                    this.txtNombreCuentaAux.Select(0, 0);
                    this.txtNombreCuentaAux.Focus();

                    //Habilitar o no la opción de crear grupos de cuentas
                    this.HabilitarSiNoCrearGrupoCtas();

                    //Habilitar o no la opción de eliminar cuentas de auxiliar
                    bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                    if (operarEliminar && !this.tipoAuxExterno) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                    else utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                }
            }

            //Tipo de Auxiliar Externo, desactivar campos del formulario que lo requieran
            if (this.tipoAuxExterno && !this.soloConsulta) this.TipoAuxExternoActualizarCamposForm();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCtaAux_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void txtCtaAux_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codCuentaAux = this.txtCtaAux.Text.Trim();

                if (codCuentaAux == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCtaAux.Text = "";
                    this.txtCtaAux.Focus();

                    RadMessageBox.Show("Código de cuenta auxiliar obligatorio", this.LP.GetText("errValCodCtaAux", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codCuentaAuxOk = true;
                if (this.nuevo) codCuentaAuxOk = this.CodigoCuentaAuxValido();    //Verificar que el codigo no exista

                if (codCuentaAuxOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                    }
                    this.txtCtaAux.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                    utiles.ButtonEnabled(ref this.radButtonCrearGrupoCuentas, true);

                    this.codigo = this.txtCtaAux.Text;
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCtaAux.Focus();
                    RadMessageBox.Show("Código de cuenta auxiliar ya existe", this.LP.GetText("errValCodCuentaAuxExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                    this.codigo = null;
                }
            }
            bTabulador = false;
        }

        private void tgTexBoxSelNumIdTributariaPais_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtNumIdTributaria_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtNumTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtNumTelefono, ref sender, ref e, false);
        }

        private void txtPrimDigVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtMemoFormTerCodCta1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerCodCta1, ref sender, ref e, false);
        }

        private void txtMemoFormTerCodCta2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerCodCta2, ref sender, ref e, false);
        }

        private void txtMemoFormTerCodCta3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerCodCta3, ref sender, ref e, false);
        }

        private void txtMemoFormTerCodCta4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerCodCta4, ref sender, ref e, false);
        }

        private void txtMemoFormTerLimiteCred1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números. Permite negativos
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerLimiteCred1, ref sender, ref e, true);
        }

        private void txtMemoFormTerLimiteCred2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números. Permite negativos
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormTerLimiteCred2, ref sender, ref e, true);
        }

        private void txtMemoFormBancosCtaAbono1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAbono1, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAbono2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAbono2, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAbono3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAbono3, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAbono4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAbono4, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAdeudo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAdeudo1, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAdeudo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAdeudo2, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAdeudo3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAdeudo3, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaAdeudo4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaAdeudo4, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaImpagados1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaImpagados1, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaImpagados2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaImpagados2, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaImpagados3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaImpagados3, ref sender, ref e, false);
        }

        private void txtMemoFormBancosCtaImpagados4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosCtaImpagados4, ref sender, ref e, false);
        }

        private void txtMemoFormBancosLimiteDto_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números. Permite negativos
            utiles.ValidarNumeroKeyPress(ref this.txtMemoFormBancosLimiteDto, ref sender, ref e, true);
        }

        private void txtZonaNivel1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtZonaNivel2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtZonaNivel3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtZonaNivel4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void cmbMemoFormTerFormaPagoNumPlazos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)      //Número
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cmbMemoFormTerFormaPagoDiasFijos1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)      //Número
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cmbMemoFormTerFormaPagoDiasFijos2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)      //Número
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cmbMemoFormTerFormaPagoDiasFijos3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) //Retroceso
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)      //Número
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            frmZonaJerarqSel selZona = new frmZonaJerarqSel();
            selZona.ClaseZona = this.claseZonaNivel1;
            selZona.FrmPadre = this;
            selZona.ShowDialog();

            if (selZona.ZonaSel != "") this.VisualizarZona(selZona.ZonaSel);
        }

        private void toolStripButtonCrearGrupoCtas_Click(object sender, EventArgs e)
        {
            this.CrearGrupoCtas();
        }

        private void toolStripButtonEliminar_Click(object sender, EventArgs e)
        {
            this.Eliminar();
        }

        private void frmMtoGLM05_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void frmMtoGLM05_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtCtaAux.Text.Trim() != this.txtCtaAux.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    this.txtNombreCuentaAux.Text.Trim() != this.txtNombreCuentaAux.Tag.ToString().Trim() ||
                    (this.radButtonTextBoxNumIdTributariaPais.Text.Trim() != null && this.radButtonTextBoxNumIdTributariaPais.Text.Trim() != this.radButtonTextBoxNumIdTributariaPais.Tag.ToString()) ||
                    this.txtNumTelefono.Text.Trim() != this.txtNumTelefono.Tag.ToString() ||
                    this.txtEmail.Text.Trim() != this.txtEmail.Tag.ToString() ||
                    this.txtCP.Text.Trim() != this.txtCP.Tag.ToString() ||
                    this.txtApdoPostal.Text.Trim() != this.txtApdoPostal.Tag.ToString() ||
                    this.txtPrimDigVal.Text.Trim() != this.txtPrimDigVal.Tag.ToString() ||
                    (this.codigoGrupoCuentas != null && this.codigoGrupoCuentas.Trim() != this.radButtonTextBoxGrupoCtas.Tag.ToString()) ||
                    this.ZonasCambio() ||
                    this.DireccionCambio() ||
                    this.MemoCambio()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonSave.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Cuentas de Auxiliar Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM05TituloALta", "Mantenimiento de Cuentas de Auxiliar - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM05TituloEdit", "Mantenimiento de Cuentas de Auxiliar - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar"); ;
            this.radButtonCrearGrupoCuentas.Text = this.LP.GetText("lblCrearGruposCtas", "Crear Grupo de Cuentas"); 
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
            
            //Traducir los campos del formulario
            //------ Apartado General -----
            this.radPageViewPageGeneral.Text = this.LP.GetText("lblGLM05General", "General");
            this.lblTipoAux.Text = this.LP.GetText("lblGLM05TipoAux", "Tipo de Auxiliar");
            //this.lblEstado.Text = this.LP.GetText("lblEstado", "Activa");
            this.lblCtaAux.Text = this.LP.GetText("lblGLM05CtaAux", "Cuenta de Auxiliar");
            this.lblNumIdTributaria.Text = this.LP.GetText("lblGLM05NumIdTrib", "Número Id. Tributaria");
            this.lblNumTelefono.Text = this.LP.GetText("lblGLM05NumTelefono", "Número de teléfono");
            this.lblEmail.Text = this.LP.GetText("lblGLM05Email", "Correo electrónico");
            this.lblCP.Text = this.LP.GetText("lblGLM05CP", "Código postal");
            this.lblApdoPostal.Text = this.LP.GetText("lblGLM05ApartadoPostal", "Apartado postal");
            this.lblPrimDigVal.Text = this.LP.GetText("lblGLM05PrimDigCompVal", "1er  dígito de compañías válidas");
            this.lblGrupoCtas.Text = this.LP.GetText("lblGLM05ApartadoPostal", "Grupo de cuentas a que pertenece");
            this.gbZona.Text = this.LP.GetText("lblGLM05Zona", "Zona");
            this.btnBuscar.Text = this.LP.GetText("lblGLM05Buscar", "Buscar");

            //------ Apartado Dirección sin formato ------
            this.radPageViewPageDirReqSN.Text = this.LP.GetText("lblGLM05Direccion", "Dirección");
            this.lblIndiqueDir.Text = this.LP.GetText("lblGLM05IndicarDir", "Indique la dirección para esta cuenta");

            //------ Apartado Dirección Formateada ------
            this.radPageViewPageDirFormateada.Text = this.LP.GetText("lblGLM05Direccion", "Dirección");
            this.lblDirForSiglas.Text = this.LP.GetText("lblGLM05Siglas", "Siglas vía pública");
            this.lblDirForNombreVia.Text = this.LP.GetText("lblGLM05NombreVia", "Nombre vía pública");
            this.lblDirForNumCasa.Text = this.LP.GetText("lblGLM05NumCasa", "Número de la casa");
            this.lblDirForCP.Text = this.LP.GetText("lblGLM05CP", "Código postal");
            this.lblDirForMunicipio.Text = this.LP.GetText("lblGLM05Municipio", "Municipio");
            this.lblDirForLinea3.Text = this.LP.GetText("lblGLM05Linea3", "Dirección ( línea 3 )");
            this.lblDirForLinea4.Text = this.LP.GetText("lblGLM05Linea4", "Dirección ( línea 4 )");
            this.lblDirForLinea5.Text = this.LP.GetText("lblGLM05Linea5", "Dirección ( línea 5 )");

            //------ Apartado Memo sin formato ------
            this.radPageViewPageMemoFormNo.Text = this.LP.GetText("lblGLM05Memo", "Memo");
            this.lblMemoFormNoIndiqueDir.Text = this.LP.GetText("lblGLM05IndicarMemo", "Indique los memos para esta cuenta");

            //------ Apartado Memo Terceros ------
            this.radPageViewPageMemoFormTerceros.Text = this.LP.GetText("lblGLM05Memo", "Memo");
            this.lblMemoFormTerMemo.Text = this.LP.GetText("lblGLM05Memo", "Memo");
            this.gbDomiciliacionB.Text = this.LP.GetText("lblGLM05DomBancaria", "Domiciliación Bancaria");
            this.lblMemoFormTerNombre.Text = this.LP.GetText("lblGLM05NombreBanco", "Nombre del banco");
            this.lblMemoFormTerDir.Text = this.LP.GetText("lblGLM05Direccion", "Dirección");
            this.lblMemoFormTerPoblacion.Text = this.LP.GetText("lblGLM05Poblacion", "Población");
            this.lblMemoFormTerCodCta.Text = this.LP.GetText("lblGLM05CodCtaCorr", "Código cuenta corriente");
            this.lblMemoFormTerIBAN.Text = this.LP.GetText("lblGLM05Iban", "Código IBAN");
            this.lblMemoFormTerSwift.Text = this.LP.GetText("lblGLM05Swift", "Código SWIFT");
            this.lblMemoFormTerMandatoUnico.Text = this.LP.GetText("lblGLM05MandatoUnico", "Mandato único");
            this.lblMemoFormTerReservadoUser.Text = this.LP.GetText("lblGLM05ReservadoUser", "Reservado usuario");
            this.gbFormaPago.Text = this.LP.GetText("lblGLM05FormaPago", "Forma de pago");
            this.lblMemoFormTerFormaPagoAnoPrimVto.Text = this.LP.GetText("lblGLM05Anos1Vto", "Años hasta el primer vencimiento");
            this.lblMemoFormTerFormaPagoNumPlazosPrimVto.Text = this.LP.GetText("lblGLM05NumPlazos1Vto", "Núm. plazos hasta 1er. vencimiento");
            this.lblMemoFormTerFormaPagoTipoPlazo.Text = this.LP.GetText("lblGLM05TipoPlazo", "Tipo de plazo");
            this.lblMemoFormTerFormaPagoNumPlazos.Text = this.LP.GetText("lblGLM05NumPlazos", "Número de plazos");
            //this.lblMemoFormTerFormaPagoNumPlazosPrimVto.Text = this.LP.GetText("lblGLM05FormaPago", "Días fijos de pago");
            this.lblMemoFormTerFormaPagoTipoDiaFijo.Text = this.LP.GetText("lblGLM05TipoDiaFijo", "Tipo de día fijo");
            this.lblMemoFormTerLimiteCred.Text = this.LP.GetText("lblGLM05LimitesCred", "Límites de crédito");

            //------ Apartado Memo Bancos ------
            this.radPageViewPageMemoFormBancos.Text = this.LP.GetText("lblGLM05Memo", "Memo");
            this.lblPageMemoFormBancosIBAN.Text = this.LP.GetText("lblGLM05Iban", "Código IBAN");
            this.lblPageMemoFormBancosSWIFT.Text = this.LP.GetText("lblGLM05SwiftBic", "Código SWIFT/BIC");
            this.lblPageMemoFormBancosCtaAbono.Text = this.LP.GetText("lblGLM05CtaAbono", "Cuenta abono");
            this.lblPageMemoFormBancosCtaAdeudo.Text = this.LP.GetText("lblGLM05CtaAdeudo", "Cuenta adeudo");
            this.lblPageMemoFormBancosCtaImpagados.Text = this.LP.GetText("lblGLM05CtaImpagados", "Cuenta impagados");
            this.lblPageMemoFormBancosLimiteDto.Text = this.LP.GetText("lblGLM05LimiteDto", "Límite descuento");            
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el tipo de auxiliar (al dar de alta un tipo de auxiliar)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.radPageViewDatos.Enabled = valor;
        }

        /// <summary>
        /// Rellena los controles con los datos de la cuenta de auxiliar (modo edición)
        /// </summary>
        private void CargarInfoCuentaAuxiliar()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                query += "where TAUXMA = '" + this.codigoTipoAux + "' and CAUXMA = '" +  this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                bool existeCtaAux = false;
                string zona = "";
                string cuentaAux = "";
                string cuentaAuxDesc = "";
                string filtro = "";
                if (dr.Read())
                {
                    existeCtaAux = true;
                    string estado = dr.GetValue(dr.GetOrdinal("STATMA")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;

                    this.txtNombreCuentaAux.Text = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString().TrimEnd();
                    this.radButtonTextBoxNumIdTributariaPais.Text = dr.GetValue(dr.GetOrdinal("PCIFMA")).ToString().TrimEnd();
                    this.codigoPais = this.radButtonTextBoxNumIdTributariaPais.Text.Trim();
                    this.txtNumIdTributaria.Text = dr.GetValue(dr.GetOrdinal("NNITMA")).ToString().TrimEnd();
                    this.txtNumTelefono.Text = dr.GetValue(dr.GetOrdinal("FONOMA")).ToString().TrimEnd();
                    this.txtCP.Text = dr.GetValue(dr.GetOrdinal("POSTMA")).ToString().TrimEnd();
                    this.txtApdoPostal.Text = dr.GetValue(dr.GetOrdinal("APARMA")).ToString().TrimEnd();
                    this.txtPrimDigVal.Text = dr.GetValue(dr.GetOrdinal("GRUPMA")).ToString().TrimEnd();

                    cuentaAux = dr.GetValue(dr.GetOrdinal("GRCTMA")).ToString().TrimEnd();
                    this.codigoGrupoCuentas = cuentaAux.Trim();
                    if (cuentaAux != "")
                    {
                        filtro = "and TAUXGA = '" + this.codigoTipoAux + "'";
                        cuentaAuxDesc = utilesCG.ObtenerDescDadoCodigo("GLT08", "GRCTGA", "NOMBGA", cuentaAux, false, filtro).Trim();
                        if (cuentaAuxDesc != "") cuentaAux = cuentaAux + " " + separadorDesc + " " + cuentaAuxDesc;
                    }                        
                    this.radButtonTextBoxGrupoCtas.Text = cuentaAux;

                    zona = dr.GetValue(dr.GetOrdinal("ZONAMA")).ToString().TrimEnd();
                    this.VisualizarZona(zona);
                }

                dr.Close();

                if (existeCtaAux)
                {
                    //Cargar los valores del tipo de direccion que procede
                    switch (this.tipoDir)
                    {
                        case "F":
                            this.CargarInfoDireccionFormateada();
                            break;
                        case "S":
                        case "N":
                            this.CargarInfoDireccionRequeridaSN();
                            break;
                    }

                    //Cargar los valores del memo que procede
                    switch (tipoMemo)
                    {
                        case "0":
                            this.CargarInfoMemoSinFormato();
                            break;
                        case "1":
                            this.CargarInfoMemoTerceros();
                            break;
                        case "2":
                            this.CargarInfoMemoBancos();
                            break;
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
        }

        /// <summary>
        /// Obtiene el tipo de dirección del tipo de auxiliar. Activa la pestaña correspondiente a dicha dirección
        /// </summary>
        private void ObtenerDatosTipoAuxiliar()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + this.codigoTipoAux + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.tipoDir = dr.GetValue(dr.GetOrdinal("RQDAMT")).ToString().TrimEnd();
                    this.tipoMemo = dr.GetValue(dr.GetOrdinal("FMEMMT")).ToString().TrimEnd();

                    this.claseZonaNivel1 = dr.GetValue(dr.GetOrdinal("CLS1MT")).ToString().TrimEnd();
                    this.claseZonaNivel2 = dr.GetValue(dr.GetOrdinal("CLS2MT")).ToString().TrimEnd();
                    this.claseZonaNivel3 = dr.GetValue(dr.GetOrdinal("CLS3MT")).ToString().TrimEnd();
                    this.claseZonaNivel4 = dr.GetValue(dr.GetOrdinal("CLS4MT")).ToString().TrimEnd();
                }

                dr.Close();

                this.radPageViewDatos.Pages.Remove(this.radPageViewPageDirReqSN);
                this.radPageViewDatos.Pages.Remove(this.radPageViewPageDirFormateada);
                this.radPageViewDatos.Pages.Remove(this.radPageViewPageMemoFormNo);
                this.radPageViewDatos.Pages.Remove(this.radPageViewPageMemoFormTerceros);
                this.radPageViewDatos.Pages.Remove(this.radPageViewPageMemoFormBancos);

                if (this.tipoDir != "")
                {
                    switch (tipoDir)
                    {
                        case "F":
                            this.radPageViewDatos.Pages.Insert(1, this.radPageViewPageDirFormateada);
                            this.txtCP.IsReadOnly = true;
                            break;
                        case "S":
                        case "N":
                            this.radPageViewDatos.Pages.Insert(1, this.radPageViewPageDirReqSN);
                            this.txtCP.IsReadOnly = false;
                            break;
                    }
                }

                if (this.tipoMemo != "")
                {
                    switch (tipoMemo)
                    {
                        case "0":
                            this.radPageViewDatos.Pages.Insert(2, this.radPageViewPageMemoFormNo);
                            break;
                        case "1":
                            this.radPageViewDatos.Pages.Insert(2, this.radPageViewPageMemoFormTerceros);
                            break;
                        case "2":
                            this.radPageViewDatos.Pages.Insert(2, this.radPageViewPageMemoFormBancos);
                            break;
                    }
                }

                //Zona
                if (this.claseZonaNivel1 == "" && this.claseZonaNivel2 == "" && this.claseZonaNivel3 == "" && this.claseZonaNivel4 == "")
                {
                    //No existen zonas
                    this.gbZona.Visible = false;
                    //Redefinir tamaño del control TabPage y del formulario
                }
                else
                {
                    this.gbZona.Visible = true;

                    if (this.claseZonaNivel1 != "")
                    {
                        this.etiquetaZonaNivel1 = "";
                        this.etiquetaZonaNivel2 = "";
                        this.etiquetaZonaNivel3 = "";
                        this.etiquetaZonaNivel4 = "";

                        this.zonaJerarq = utilesCG.isClaseZonaJerarquica(this.claseZonaNivel1);
                        if (this.zonaJerarq) this.btnBuscar.Visible = true;
                        else this.btnBuscar.Visible = false;

                        string estructura = "";
                        if (zonaJerarq)
                        {
                            query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 where ";
                            query += "CLASZ0 = '" + this.claseZonaNivel1 + "' ";

                            dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                            if (dr.Read())
                            {
                                this.etiquetaZonaNivel1 = dr.GetValue(dr.GetOrdinal("NNV1Z0")).ToString().TrimEnd();
                                this.etiquetaZonaNivel2 = dr.GetValue(dr.GetOrdinal("NNV2Z0")).ToString().TrimEnd();
                                this.etiquetaZonaNivel3 = dr.GetValue(dr.GetOrdinal("NNV3Z0")).ToString().TrimEnd();
                                this.etiquetaZonaNivel4 = dr.GetValue(dr.GetOrdinal("NNV4Z0")).ToString().TrimEnd();

                                estructura = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();
                                if (estructura.Length < 4) estructura.PadRight(4, '0');
                                this.longZonaNivel1 = Convert.ToInt16(estructura.Substring(0, 1));
                                this.longZonaNivel2 = Convert.ToInt16(estructura.Substring(1, 1));
                                this.longZonaNivel3 = Convert.ToInt16(estructura.Substring(2, 1));
                                this.longZonaNivel4 = Convert.ToInt16(estructura.Substring(3, 1));

                                string validaZona = dr.GetValue(dr.GetOrdinal("UTILZ0")).ToString().Trim();
                                this.validarZonaNivel1 = (validaZona == "S") ? true : false;
                            }
                            dr.Close();
                        }
                        else
                        {
                            //Obtener los datos de los niveles de la zona no jerárquica
                            this.ObtenerDatosZonaNoJerarquica(this.claseZonaNivel1, ref this.etiquetaZonaNivel1, ref this.longZonaNivel1, ref this.validarZonaNivel1);

                            if (this.claseZonaNivel2 != "")
                            {
                                this.ObtenerDatosZonaNoJerarquica(this.claseZonaNivel2, ref this.etiquetaZonaNivel2, ref this.longZonaNivel2, ref this.validarZonaNivel2);
                            }

                            if (this.claseZonaNivel3 != "")
                            {
                                this.ObtenerDatosZonaNoJerarquica(this.claseZonaNivel3, ref this.etiquetaZonaNivel3, ref this.longZonaNivel3, ref this.validarZonaNivel3);
                            }

                            if (this.claseZonaNivel4 != "")
                            {
                                this.ObtenerDatosZonaNoJerarquica(this.claseZonaNivel4, ref this.etiquetaZonaNivel4, ref this.longZonaNivel4, ref this.validarZonaNivel4);                                
                            }
                        }

                        int cantZonas = 0;
                        int ancho = 0;
                        if (this.etiquetaZonaNivel1 != "")
                        {
                            this.lblZonaNivel1.Text = this.etiquetaZonaNivel1;

                            if (!this.zonaJerarq)
                            {
                                //this.CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel1, this.claseZonaNivel1, this.longZonaNivel1);
                                this.txtZonaNivel1.Visible = false;
                            }
                            else
                            {
                                this.txtZonaNivel1.MaxLength = this.longZonaNivel1;
                                ancho = 40 + ((this.longZonaNivel1 - 1) * 4);
                                this.txtZonaNivel1.Size = new Size(ancho, 30);
                                this.radButtonTextBoxZonaNivel1.Visible = false;
                            }
                            cantZonas++;
                        }
                        else
                        {
                            this.lblZonaNivel1.Visible = false;
                            this.txtZonaNivel1.Visible = false;
                            this.radButtonTextBoxZonaNivel1.Visible = false;
                        }

                        if (this.etiquetaZonaNivel2 != "")
                        {
                            this.lblZonaNivel2.Text = this.etiquetaZonaNivel2;

                            if (!this.zonaJerarq)
                            {
                                //this.CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel2, this.claseZonaNivel2, this.longZonaNivel2);
                                this.txtZonaNivel2.Visible = false;
                            }
                            else
                            {
                                this.txtZonaNivel2.MaxLength = this.longZonaNivel2;
                                ancho = 40 + ((this.longZonaNivel2 - 1) * 4);
                                this.txtZonaNivel2.Size = new Size(ancho, 30);
                                this.radButtonTextBoxZonaNivel2.Visible = false;
                            }
                            cantZonas++;
                        }
                        else
                        {
                            this.lblZonaNivel2.Visible = false;
                            this.txtZonaNivel2.Visible = false;
                            this.radButtonTextBoxZonaNivel2.Visible = false;
                        }

                        if (this.etiquetaZonaNivel3 != "")
                        {
                            this.lblZonaNivel3.Text = this.etiquetaZonaNivel3;

                            if (!this.zonaJerarq)
                            {
                                //this.CrearControlSeleccionZona(ref radButtonTextBoxZonaNivel3, this.claseZonaNivel3, this.longZonaNivel3);
                                this.txtZonaNivel3.Visible = false;
                            }
                            else
                            {
                                this.txtZonaNivel3.MaxLength = this.longZonaNivel3;
                                ancho = 40 + ((this.longZonaNivel3 - 1) * 4);
                                this.txtZonaNivel3.Size = new Size(ancho, 30);
                                this.radButtonTextBoxZonaNivel3.Visible = false;
                            }
                            cantZonas++;
                        }
                        else
                        {
                            this.lblZonaNivel3.Visible = false;
                            this.txtZonaNivel3.Visible = false;
                            this.radButtonTextBoxZonaNivel3.Visible = false;
                        }

                        if (this.etiquetaZonaNivel4 != "")
                        {
                            this.lblZonaNivel4.Text = this.etiquetaZonaNivel4;

                            if (!this.zonaJerarq)
                            {
                                //this.CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel4, this.claseZonaNivel4, this.longZonaNivel4);
                                this.txtZonaNivel4.Visible = false;
                            }
                            else
                            {
                                this.txtZonaNivel4.MaxLength = this.longZonaNivel4;
                                ancho = 40 + ((this.longZonaNivel4 - 1) * 4);
                                this.txtZonaNivel4.Size = new Size(ancho, 30);
                                this.radButtonTextBoxZonaNivel4.Visible = false;
                            }
                            cantZonas++;
                        }
                        else
                        {
                            this.lblZonaNivel4.Visible = false;
                            this.txtZonaNivel4.Visible = false;
                            this.radButtonTextBoxZonaNivel4.Visible = false;
                        }
                    }


                    //Ajustar tamaño zona y del formulario
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Inicializa las variables de la zona nivel que corresponda
        /// </summary>
        /// <param name="claseZonaNivel">Valor de la clase de zona</param>
        /// <param name="etiquetaZonaNivel">Variable que almacena el nombre de la zona</param>
        /// <param name="longitudZonaNivel">Variable que almacena la longitud de la zona</param>
        /// <param name="validarZonaNivel">Variable que almacena si se valida o no la zona </param>
        private void ObtenerDatosZonaNoJerarquica(string claseZonaNivel, ref string etiquetaZonaNivel, ref int longitudZonaNivel, ref bool validarZonaNivel)
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 where ";
                query += "CLASZ0 = '" + claseZonaNivel + "' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    etiquetaZonaNivel = dr.GetValue(dr.GetOrdinal("NNV1Z0")).ToString().TrimEnd();
                    string estructura = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();
                    if (estructura.Length < 4) estructura.PadRight(4, '0');
                    longitudZonaNivel = Convert.ToInt16(estructura.Substring(0, 1));
                    string validaZona = dr.GetValue(dr.GetOrdinal("UTILZ0")).ToString().Trim();
                    validarZonaNivel = (validaZona == "S") ? true : false;
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
        /// Crea el control que permite seleccionar una zona
        /// </summary>
        /// <param name="radButtonTextBoxZonaNivel"></param>
        /// <param name="claseZonaNivel"></param>
        /// <param name="longZonaNivel"></param>
        private void CrearControlSeleccionZona(ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxZonaNivel, string claseZonaNivel, int longZonaNivel)
        {
            string query = "select ZONAZ1, NOMBZ1 from ";
            query += GlobalVar.PrefijoTablaCG + "GLM11 ";
            query += " where CLASZ1 = '" + claseZonaNivel + "' and TIPOZ1 = 'D'";
            query += "order by ZONAZ1";

            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));

            string result = this.BuscarElementos("Seleccionar zona", query, nombreColumnas, 2);
            if (result != "")
            {
                radButtonTextBoxZonaNivel.Text = result;
                this.ActiveControl = radButtonTextBoxZonaNivel;
                radButtonTextBoxZonaNivel.Select(0, 0);
                radButtonTextBoxZonaNivel.Focus();
            }
        }

        /// <summary>
        /// Crea el control que permite seleccionar reservado usuario
        /// </summary>
        /// <param name="radButtonTextBoxZonaNivel"></param>
        /// <param name="claseZonaNivel"></param>
        /// <param name="longZonaNivel"></param>
        private void CrearControlSeleccionReservadoUsuario()
        {
            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel();

            //Título del Formulario de Selección de Elementos
            frmElementosSel.TituloForm = "Seleccionar reservado usuario";
            //Coordenadas donde se dibujará el Formulario de Selección de Elementos
            //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
            frmElementosSel.LocationForm = new Point(0, 0);
            //Si se centrar el Formulario o no
            frmElementosSel.CentrarForm = true;
            //Pasar la conexión a la bbdd
            frmElementosSel.ProveedorDatosForm = GlobalVar.ConexionCG;
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CODI3F, DESC3F from ";
            query += GlobalVar.PrefijoTablaCG + "G3T03 ";
            query += "order by CODI3F";
            frmElementosSel.Query = query;

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList();
            nombreColumnasPlan.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnasPlan.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));
            frmElementosSel.ColumnasCaption = nombreColumnasPlan;
            //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
            frmElementosSel.FrmPadre = this;

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

                if (result != "")
                {
                    this.radButtonTextBoxReservadoUser.Text = result;
                    this.ActiveControl = this.radButtonTextBoxReservadoUser;
                    this.radButtonTextBoxReservadoUser.Select(0, 0);
                    this.radButtonTextBoxReservadoUser.Focus();
                }
            }
        }

        /// <summary>
        /// Carga los datos de la dirección sin formato
        /// </summary>
        private void CargarInfoDireccionRequeridaSN()
        {
            IDataReader dr = null;
            try
            {
                string dauxad = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLDM5 ";
                query += "where TAUXAD= '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existenDatosDireccion = true;
                    dauxad = dr.GetValue(dr.GetOrdinal("DAUXAD")).ToString().PadRight(180, ' ');
                }

                dr.Close();

                if (dauxad != "")
                {
                    this.txtDirReqLinea1.Text = dauxad.Substring(0, 36).TrimEnd();
                    this.txtDirReqLinea2.Text = dauxad.Substring(36, 36).TrimEnd();
                    this.txtDirReqLinea3.Text = dauxad.Substring(72, 36).TrimEnd();
                    this.txtDirReqLinea4.Text = dauxad.Substring(108, 36).TrimEnd();
                    this.txtDirReqLinea5.Text = dauxad.Substring(144, 36).TrimEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos de la dirección formateada
        /// </summary>
        private void CargarInfoDireccionFormateada()
        {
            IDataReader dr = null;
            try
            {
                string dauxad = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLDM5 ";
                query += "where TAUXAD= '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existenDatosDireccion = true;
                    dauxad = dr.GetValue(dr.GetOrdinal("DAUXAD")).ToString().PadRight(180, ' ');
                }

                dr.Close();

                if (dauxad != "")
                {
                    this.txtDirForSiglas.Text = dauxad.Substring(0, 2).TrimEnd();
                    this.txtDirForNombreVia.Text = dauxad.Substring(2, 25).TrimEnd();
                    this.txtDirForNumCasa.Text = dauxad.Substring(31, 5).TrimEnd();
                    this.txtDirForMunicipio.Text = dauxad.Substring(36, 24).TrimEnd();
                    this.txtDirForCP.Text = this.txtCP.Text;
                    this.txtDirForLinea3.Text = dauxad.Substring(62, 36).TrimEnd();
                    this.txtDirForLinea4.Text = dauxad.Substring(98, 36).TrimEnd();
                    this.txtDirForLinea5.Text = dauxad.Substring(134, 36).TrimEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos de la dirección formateada
        /// </summary>
        private void CargarInfoMemoSinFormato()
        {
            IDataReader dr = null;
            try
            {
                string meaxad = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMM5 ";
                query += "where TAUXAD= '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existenDatosMemo = true;
                    meaxad = dr.GetValue(dr.GetOrdinal("MEAXAD")).ToString().PadRight(252, ' ');
                    this.txtEmail.Text = dr.GetValue(dr.GetOrdinal("FILLGB")).ToString().Trim();
                }

                dr.Close();

                if (meaxad != "")
                {
                    try
                    {
                        if (meaxad.Length != 252) meaxad = meaxad.PadRight(252, ' ');
                        this.txtMemoFormNoLinea1.Text = meaxad.Substring(0, 36).TrimEnd();
                        this.txtMemoFormNoLinea2.Text = meaxad.Substring(36, 36).TrimEnd();
                        this.txtMemoFormNoLinea3.Text = meaxad.Substring(72, 36).TrimEnd();
                        this.txtMemoFormNoLinea4.Text = meaxad.Substring(108, 36).TrimEnd();
                        this.txtMemoFormNoLinea5.Text = meaxad.Substring(144, 36).TrimEnd();
                        this.txtMemoFormNoLinea6.Text = meaxad.Substring(180, 36).TrimEnd();
                        this.txtMemoFormNoLinea7.Text = meaxad.Substring(216, 36).TrimEnd();
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos del memo de Terceros
        /// </summary>
        private void CargarInfoMemoTerceros()
        {
            IDataReader dr = null;
            try
            {
                ///Construir el control de seleccion de reservado usuario
                //this.CrearControlSeleccionReservadoUsuario();

                string meaxad = "";
                string mem1gb = "";
                string mem2gb = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMM5 ";
                query += "where TAUXAD= '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existenDatosMemo = true;
                    meaxad = dr.GetValue(dr.GetOrdinal("MEAXAD")).ToString().PadRight(252, ' ');
                    mem1gb = dr.GetValue(dr.GetOrdinal("MEM1GB")).ToString();
                    mem2gb = dr.GetValue(dr.GetOrdinal("MEM2GB")).ToString();
                    this.txtEmail.Text = dr.GetValue(dr.GetOrdinal("FILLGB")).ToString().Trim();
                }

                dr.Close();

                if (meaxad != "")
                {
                    if (meaxad.Length != 252) meaxad = meaxad.PadRight(252, ' ');
                    this.txtMemoFormTerMemo1.Text = meaxad.Substring(0, 31).TrimEnd();
                    this.txtMemoFormTerMemo2.Text = meaxad.Substring(31, 31).TrimEnd();
                    this.txtMemoFormTerMemo3.Text = meaxad.Substring(62, 30).TrimEnd();
                    this.txtMemoFormTerNombre.Text = meaxad.Substring(92, 36).TrimEnd();
                    this.txtMemoFormTerDir.Text = meaxad.Substring(128, 34).TrimEnd();
                    this.txtMemoFormTerPoblacion.Text = meaxad.Substring(162, 30).TrimEnd();
                    this.txtMemoFormTerCodCta1.Text = meaxad.Substring(192, 4).TrimEnd();
                    this.txtMemoFormTerCodCta2.Text = meaxad.Substring(196, 4).TrimEnd();
                    this.txtMemoFormTerCodCta3.Text = meaxad.Substring(200, 2).TrimEnd();
                    this.txtMemoFormTerCodCta4.Text = meaxad.Substring(202, 10).TrimEnd();
                    
                    string reservadoUser = meaxad.Substring(212, 2).TrimEnd();
                    this.codigoReservadoUser = reservadoUser;
                    if (reservadoUser != "")
                    {
                        string reservadoUserDesc = utilesCG.ObtenerDescDadoCodigo("G3T03", "CODI3F", "DESC3F", reservadoUser, false, "").Trim();
                        if (reservadoUserDesc != "") reservadoUser = reservadoUser + " " + separadorDesc + " " + reservadoUserDesc;
                    }
                    this.radButtonTextBoxReservadoUser.Text = reservadoUser;

                    try
                    {
                        string anoPrimVto = meaxad.Substring(214, 1);
                        if (anoPrimVto == " ") anoPrimVto = "0";
                        this.cmbMemoFormTerFormaPagoAnoPrimVto.SelectedIndex = this.cmbMemoFormTerFormaPagoAnoPrimVto.Items.IndexOf(anoPrimVto);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.cmbMemoFormTerFormaPagoAnoPrimVto.SelectedIndex = 0; 
                    }

                    try
                    {
                        string numPlazosPrimVto = meaxad.Substring(215, 1);
                        if (numPlazosPrimVto == " ") numPlazosPrimVto = "0";
                        this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.SelectedIndex = this.cmbMemoFormTerFormaPagoAnoPrimVto.Items.IndexOf(numPlazosPrimVto);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.cmbMemoFormTerFormaPagoAnoPrimVto.SelectedIndex = 0; 
                    }

                    string tipoPlazo = meaxad.Substring(216, 1);
                    if (tipoPlazo == "2") this.rbMemoFormTerFormaPagoTipoPlazo15.IsChecked = true;
                    else this.rbMemoFormTerFormaPagoTipoPlazo30.IsChecked = true;

                    try
                    {
                        string numPlazos = meaxad.Substring(217, 2);
                        if (!(numPlazos == "  " || numPlazos == "00")) this.cmbMemoFormTerFormaPagoNumPlazos.SelectedIndex = this.cmbMemoFormTerFormaPagoNumPlazos.Items.IndexOf(numPlazos);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        string tipoDiaFijjo = meaxad.Substring(225, 1);
                        if (tipoDiaFijjo == "1")
                        {
                            this.rbMemoFormTerFormaPagoTipoDiaFijoSemana.IsChecked = true;
                            this.cmbMemoFormTerFormaPagoDiasFijos1.Items.Clear();
                            this.cmbMemoFormTerFormaPagoDiasFijos2.Items.Clear();
                            this.cmbMemoFormTerFormaPagoDiasFijos3.Items.Clear();
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valoresSemana);
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valoresSemana);
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valoresSemana);
                        }
                        else
                        {
                            this.rbMemoFormTerFormaPagoTipoDiaFijoMes.IsChecked = true;
                            this.cmbMemoFormTerFormaPagoDiasFijos1.Items.Clear();
                            this.cmbMemoFormTerFormaPagoDiasFijos2.Items.Clear();
                            this.cmbMemoFormTerFormaPagoDiasFijos3.Items.Clear();
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valoresMes);
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valoresMes);
                            utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valoresMes);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        string diasFijosPago1 = meaxad.Substring(219, 2);
                        if (!(diasFijosPago1 == "  " || diasFijosPago1 == "00")) this.cmbMemoFormTerFormaPagoDiasFijos1.SelectedIndex = this.cmbMemoFormTerFormaPagoDiasFijos1.Items.IndexOf(diasFijosPago1);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        string diasFijosPago2 = meaxad.Substring(221, 2);
                        if (!(diasFijosPago2 == "  " || diasFijosPago2 == "00")) this.cmbMemoFormTerFormaPagoDiasFijos2.SelectedIndex = this.cmbMemoFormTerFormaPagoDiasFijos2.Items.IndexOf(diasFijosPago2);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    try
                    {
                        string diasFijosPago3 = meaxad.Substring(223, 2);
                        if (!(diasFijosPago3 == "  " || diasFijosPago3 == "00")) this.cmbMemoFormTerFormaPagoDiasFijos3.SelectedIndex = this.cmbMemoFormTerFormaPagoDiasFijos3.Items.IndexOf(diasFijosPago3);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    string limiteSup = meaxad.Substring(226, 13);
                    try
                    {
                        if (limiteSup.Trim() != "")
                        {
                            Int64 limiteSupInt = Convert.ToInt64(limiteSup);
                            if (limiteSupInt == 0) limiteSup = "";
                            else limiteSup = limiteSupInt.ToString();
                        }
                        else limiteSup = "";
                    }
                    catch(Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        limiteSup = "";
                    }
                    this.txtMemoFormTerLimiteCred2.Text = limiteSup;

                    string limiteInf = meaxad.Substring(239, 13);
                    try
                    {
                        if (limiteInf.Trim() != "")
                        {
                            Int64 limiteInfInt = Convert.ToInt64(limiteInf);
                            if (limiteInfInt == 0) limiteInf = "";
                            else limiteInf = limiteInfInt.ToString();
                        }
                        else limiteInf = "";
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        limiteInf = "";
                    }
                    this.txtMemoFormTerLimiteCred1.Text = limiteInf;                 
                }

                //this.txtMemoFormTerIBAN.Text = mem1gb.Trim();
                this.tgIBANMemoFormTer.CargarValor(mem1gb.Trim());

                if (mem2gb.Length < 33) mem2gb = mem2gb.PadRight(33, ' ');

                
                this.txtMemoFormTerSWIFT.Text = mem2gb.Substring(0, 11).TrimEnd();
                this.txtMemoFormTerMandatoUnico.Text = mem2gb.Substring(11, 22).TrimEnd();

            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos del memo de Bancos
        /// </summary>
        private void CargarInfoMemoBancos()
        {
            IDataReader dr = null;
            try
            {
                string mem1gb = "";
                string mem2gb = "";
                string mem3gb = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMM5 ";
                query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.existenDatosMemo = true;
                    mem1gb = dr.GetValue(dr.GetOrdinal("MEM1GB")).ToString();
                    mem2gb = dr.GetValue(dr.GetOrdinal("MEM2GB")).ToString();
                    mem3gb = dr.GetValue(dr.GetOrdinal("MEM3GB")).ToString();

                    this.txtMemoFormBancosCtaAbono1.Text = dr.GetValue(dr.GetOrdinal("ABO1GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaAbono2.Text = dr.GetValue(dr.GetOrdinal("ABO2GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaAbono3.Text = dr.GetValue(dr.GetOrdinal("ABO3GB")).ToString().Trim().PadLeft(2, '0');
                    this.txtMemoFormBancosCtaAbono4.Text = dr.GetValue(dr.GetOrdinal("ABO4GB")).ToString().Trim().PadLeft(10, '0');

                    this.txtMemoFormBancosCtaAdeudo1.Text = dr.GetValue(dr.GetOrdinal("GAS1GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaAdeudo2.Text = dr.GetValue(dr.GetOrdinal("GAS2GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaAdeudo3.Text = dr.GetValue(dr.GetOrdinal("GAS3GB")).ToString().Trim().PadLeft(2, '0');
                    this.txtMemoFormBancosCtaAdeudo4.Text = dr.GetValue(dr.GetOrdinal("GAS4GB")).ToString().Trim().PadLeft(10, '0');

                    this.txtMemoFormBancosCtaImpagados1.Text = dr.GetValue(dr.GetOrdinal("IMP1GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaImpagados2.Text = dr.GetValue(dr.GetOrdinal("IMP2GB")).ToString().Trim().PadLeft(4, '0');
                    this.txtMemoFormBancosCtaImpagados3.Text = dr.GetValue(dr.GetOrdinal("IMP3GB")).ToString().Trim().PadLeft(2, '0');
                    this.txtMemoFormBancosCtaImpagados4.Text = dr.GetValue(dr.GetOrdinal("IMP4GB")).ToString().Trim().PadLeft(10, '0');

                    this.txtMemoFormBancosLimiteDto.Text = dr.GetValue(dr.GetOrdinal("LIMDGB")).ToString().Trim();

                    this.txtEmail.Text = dr.GetValue(dr.GetOrdinal("FILLGB")).ToString().Trim();
                }

                dr.Close();

                this.txtMemoFormBancosMemo.Text = mem1gb.TrimEnd();
                //this.txtMemoFormBancosIBAN.Text = mem2gb.TrimEnd();
                this.tgIBANMemoFormBancos.CargarValor(mem2gb.Trim());
                this.txtMemoFormBancosSWIFT.Text = mem3gb.TrimEnd();

            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El tipo de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);
            utiles.ButtonEnabled(ref this.radButtonEliminar, false);
            utiles.ButtonEnabled(ref this.radButtonCrearGrupoCuentas, false);
            
            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtCtaAux.Enabled = false;

            for (int i = 0; i < this.radPageViewPageGeneral.Controls.Count; i++)
            {
                this.radPageViewPageGeneral.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageDirReqSN.Controls.Count; i++)
            {
                this.radPageViewPageDirReqSN.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageDirFormateada.Controls.Count; i++)
            {
                this.radPageViewPageDirFormateada.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageMemoFormNo.Controls.Count; i++)
            {
                this.radPageViewPageMemoFormNo.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageMemoFormTerceros.Controls.Count; i++)
            {
                this.radPageViewPageMemoFormTerceros.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageMemoFormBancos.Controls.Count; i++)
            {
                this.radPageViewPageMemoFormBancos.Controls[i].Enabled = false;
            }

            this.soloConsulta = true;
        }

        /// <summary>
        /// Valida que no exista el código de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private bool CodigoCuentaAuxValido()
        {
            bool result = false;

            try
            {
                string codCuentaAux = this.txtCtaAux.Text.Trim();

                if (codCuentaAux != "")
                {
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + this.codigoTipoAux + "' and CAUXMA = '" + codCuentaAux + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Dado una zona, la trocea y rellena los controles de texto que almacenan la info
        /// Si la zona es jerárquica se visualizan en los controles TextBox, si no lo es se muestran en los TGTextBoxSel
        /// </summary>
        /// <param name="zona"></param>
        private void VisualizarZona(string zona)
        {
            try
            {
                zona = zona.PadRight(8, ' ');

                if (this.zonaJerarq)
                {
                    this.txtZonaNivel1.Text = zona.Substring(0, this.longZonaNivel1).Trim();
                    if (this.longZonaNivel1 != 0) this.txtZonaNivel2.Text = zona.Substring(this.longZonaNivel1, this.longZonaNivel2).Trim();
                    if (this.longZonaNivel2 != 0) this.txtZonaNivel3.Text = zona.Substring(this.longZonaNivel1 + this.longZonaNivel2, this.longZonaNivel3).Trim();
                    if (this.longZonaNivel3 != 0) this.txtZonaNivel4.Text = zona.Substring(this.longZonaNivel1 + this.longZonaNivel2 + this.longZonaNivel3, this.longZonaNivel4).Trim();
                }
                else
                {
                    this.valorZonaNivel1 = zona.Substring(0, this.longZonaNivel1).Trim();
                    if (this.valorZonaNivel1 != "")
                    {
                        string zonaNivel1 = this.valorZonaNivel1;
                        string filtroNivel1 = " and CLASZ1 = '" + this.claseZonaNivel1 + "' and TIPOZ1 = 'D'";
                        string zonaNivel1Desc = utilesCG.ObtenerDescDadoCodigo("GLM11", "ZONAZ1", "NOMBZ1", this.valorZonaNivel1, false, filtroNivel1).Trim();
                        if (zonaNivel1Desc != "") zonaNivel1 = zonaNivel1 + " " + separadorDesc + " " + zonaNivel1Desc;
                        this.radButtonTextBoxZonaNivel1.Text = zonaNivel1.Trim();
                    }
                    else
                    {
                        this.radButtonTextBoxZonaNivel1.Text = this.valorZonaNivel1;
                    }

                    if (this.longZonaNivel1 != 0)
                    {
                        this.valorZonaNivel2 = zona.Substring(this.longZonaNivel1, this.longZonaNivel2).Trim(); 
                        if (this.valorZonaNivel2 != "")
                        {
                            string zonaNivel2 = this.valorZonaNivel2;
                            string filtroNivel2 = " and CLASZ1 = '" + this.claseZonaNivel2 + "' and TIPOZ1 = 'D'";
                            string zonaNivel2Desc = utilesCG.ObtenerDescDadoCodigo("GLM11", "ZONAZ1", "NOMBZ1", this.valorZonaNivel2, false, filtroNivel2).Trim();
                            if (zonaNivel2Desc != "") zonaNivel2 = zonaNivel2 + " " + separadorDesc + " " + zonaNivel2Desc;
                            this.radButtonTextBoxZonaNivel2.Text = zonaNivel2.Trim();
                        }
                        else this.radButtonTextBoxZonaNivel2.Text = this.valorZonaNivel2;
                    }

                    if (this.longZonaNivel2 != 0)
                    {
                        this.valorZonaNivel3 = zona.Substring(this.longZonaNivel1 + this.longZonaNivel2, this.longZonaNivel3).Trim();
                        if (this.valorZonaNivel3 != "")
                        {
                            string zonaNivel3 = this.valorZonaNivel3;
                            string filtroNivel3 = " and CLASZ1 = '" + this.claseZonaNivel3 + "' and TIPOZ1 = 'D'";
                            string zonaNivel3Desc = utilesCG.ObtenerDescDadoCodigo("GLM11", "ZONAZ1", "NOMBZ1", this.valorZonaNivel3, false, filtroNivel3).Trim();
                            if (zonaNivel3Desc != "") zonaNivel3 = zonaNivel3 + " " + separadorDesc + " " + zonaNivel3Desc;
                            this.radButtonTextBoxZonaNivel3.Text = zonaNivel3.Trim();
                        }
                        else this.radButtonTextBoxZonaNivel3.Text = this.valorZonaNivel3;
                    }

                    if (this.longZonaNivel3 != 0)
                    {
                        this.valorZonaNivel4 = zona.Substring(this.longZonaNivel1 + this.longZonaNivel2 + this.longZonaNivel3, this.longZonaNivel4).Trim();
                        if (this.valorZonaNivel4 != "")
                        {
                            string zonaNivel4 = this.valorZonaNivel4;
                            string filtroNivel4 = " and CLASZ1 = '" + this.claseZonaNivel4 + "' and TIPOZ1 = 'D'";
                            string zonaNivel4Desc = utilesCG.ObtenerDescDadoCodigo("GLM11", "ZONAZ1", "NOMBZ1", this.valorZonaNivel4, false, filtroNivel4).Trim();
                            if (zonaNivel4Desc != "") zonaNivel4 = zonaNivel4 + " " + separadorDesc + " " + zonaNivel4Desc;
                            this.radButtonTextBoxZonaNivel4.Text = zonaNivel4.Trim();
                        }
                        else this.radButtonTextBoxZonaNivel4.Text = this.valorZonaNivel4;
                    }

                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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

            try
            {
                //Activar la página de Datos Generales
                this.radPageViewDatos.SelectedPage = this.radPageViewPageGeneral;

                //-------------- Validar el nombre de la cuenta de auxiliar ------------------
                if (this.txtNombreCuentaAux.Text.Trim() == "")
                {
                    errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtNombreCuentaAux;
                    this.txtNombreCuentaAux.Select();
                    this.txtNombreCuentaAux.Focus();
                }

                //-------------- Validar el código del país ------------------
                string codPais = this.radButtonTextBoxNumIdTributariaPais.Text.Trim();
                if (codPais == "") this.codigoPais = this.radButtonTextBoxNumIdTributariaPais.Text.Trim();
                if (codPais != "") 
                {
                    string resultValidarCodPais = this.ValidarCodigoPais();
                    if (resultValidarCodPais != "")
                    {
                        errores += "- " + resultValidarCodPais + "\n\r";
                        this.ActiveControl = this.radButtonTextBoxNumIdTributariaPais;
                        this.radButtonTextBoxNumIdTributariaPais.Select();
                        this.radButtonTextBoxNumIdTributariaPais.Focus();
                    }
                }

                //-------------- Validar el número de identificación de la agencia tributaria si el país es España ------------------
                if (codPais == "ES")
                {
                    string idAgenciaTrib = this.txtNumIdTributaria.Text.Trim();
                    if (idAgenciaTrib != "")
                    {
                        string resultValidarNIF = "";
                        if (!CheckNif.Check(idAgenciaTrib, ref resultValidarNIF))
                        {
                            errores += "- Número de Identificación Tributaria no válido\n\r";     //Falta traducir
                            this.ActiveControl = this.txtNumIdTributaria;
                            this.txtNumIdTributaria.Select();
                            this.txtNumIdTributaria.Focus();
                        }
                    }
                }

                //-------------- Validar el correo electrónico ------------------
                string email = this.txtEmail.Text.Trim();
                if (email != "")
                {
                    if (!utiles.IsValidEmail(email))
                    {
                        errores += "- Correo electrónico no válido\n\r";     //Falta traducir
                        this.ActiveControl = this.txtEmail;
                        this.txtEmail.Select();
                        this.txtEmail.Focus();
                    }
                }

                //-------------- Validar grupo de cuentas ------------------
                if (this.radButtonTextBoxGrupoCtas.Text.Trim() != "")
                {
                    string resultValidarGrupoCuentas = this.ValidarGrupoCuentas();
                    if (resultValidarGrupoCuentas != "")
                    {
                        errores += "- " + resultValidarGrupoCuentas + "\n\r";
                        this.ActiveControl = this.radButtonTextBoxGrupoCtas;
                        this.radButtonTextBoxGrupoCtas.Select();
                        this.radButtonTextBoxGrupoCtas.Focus();
                    }
                    else
                    {
                        //Autorizado a asignarlo   (operacion 10)
                        string elemento = this.codigoTipoAux + this.codigoGrupoCuentas;
                        bool operarAltaGrupoCtas = aut.Validar(autClaseElementoGLT08, autGrupoGLT08, elemento, autOperAltaGLT08);
                        if (!operarAltaGrupoCtas)
                        {
                            errores += "- Usuario no autorizado a dar de alta a grupos de cuentas \n\r";        //Falta traducir
                            this.ActiveControl = this.radButtonTextBoxGrupoCtas;
                            this.radButtonTextBoxGrupoCtas.Select();
                            this.radButtonTextBoxGrupoCtas.Focus();
                        }   
                    }
                }
                else this.codigoGrupoCuentas = "";

                //-------------- Validar zona ------------------
                if (this.gbZona.Visible)
                {
                    string resultValidarZona = this.ValidarZona();
                    if (resultValidarZona != "")
                    {
                        errores += "- " + resultValidarZona + "\n\r";
                    }
                }
                else this.codigoZona = "";

                //Validar la pestaña de dirección que corresponda
                string resultValidarDir = "";
                switch (this.tipoDir)
                {
                    case "F":
                        resultValidarDir = this.ValidarDireccionFormateada();
                        break;
                    case "S":
                        resultValidarDir = this.ValidarDireccionRequerida();
                        break;
                }
                if (resultValidarDir != "") errores += resultValidarDir;

                //Validar la pestaña de memo que corresponda
                string resultValidarMemo = "";
                switch (tipoMemo)
                {
                    case "1":
                        resultValidarMemo = this.ValidarMemoTerceros();
                        break;
                    case "2":
                        resultValidarMemo = this.ValidarMemoBancos();
                        break;
                }
                if (resultValidarMemo != "") errores += resultValidarMemo;

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));
            else this.radPageViewDatos.SelectedPage = pageActiva;

            return (result);
        }

        /// <summary>
        /// Valida que el código del país exista en la tabla GLT30
        /// </summary>
        private string ValidarCodigoPais()
        {
            string result = "";
            try
            {
                this.codigoPais = this.radButtonTextBoxNumIdTributariaPais.Text.Trim();

                if (codigoPais != "")
                {
                    string prefijoTabla = "";
                    if (this.proveedorTipo == "DB2")
                    {
                        prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                        if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                    }
                    else prefijoTabla = GlobalVar.PrefijoTablaCG;

                    string query = "select count(PCIF30) from " + prefijoTabla + "GLT30 ";
                    query += "where PCIF30 = '" + this.codigoPais + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = "Código de país no existe\n\r";   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código del país (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Valida el código del grupo de cuentas
        /// </summary>
        /// <returns></returns>
        private string ValidarGrupoCuentas()
        {
            string result = "";
            
            try
            {
                this.codigoGrupoCuentas = this.radButtonTextBoxGrupoCtas.Text.Trim();
                int posSeparador = this.codigoGrupoCuentas.IndexOf('-');
                if (posSeparador != -1) this.codigoGrupoCuentas = this.codigoGrupoCuentas.Substring(0, posSeparador - 1).Trim();

                if (this.codigoGrupoCuentas != "")
                {
                    string query = "select count(GRCTGA) from " + GlobalVar.PrefijoTablaCG + "GLT08 ";
                    query += "where TAUXGA = '" + this.codigoTipoAux + "' and GRCTGA = '" + this.codigoGrupoCuentas + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = "Código de grupo no válido\n\r";   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de grupo (" + ex.Message + ")";
            }

            if (result != "") this.codigoGrupoCuentas = "";

            return (result);
        }

        /// <summary>
        /// Valida los códigos de zonas
        /// </summary>
        /// <returns></returns>
        private string ValidarZona()
        {
            string result = "";

            try
            {
                string zonaNivel1 = "";
                string zonaNivel2 = "";
                string zonaNivel3 = "";
                string zonaNivel4 = "";

                if (this.zonaJerarq)
                {
                    zonaNivel1 = this.txtZonaNivel1.Text.Trim();
                    zonaNivel2 = this.txtZonaNivel2.Text.Trim();
                    zonaNivel3 = this.txtZonaNivel3.Text.Trim();
                    zonaNivel4 = this.txtZonaNivel4.Text.Trim();

                    //Validar las longitudes
                    result = this.ValidarLongitudNivelZona(1, zonaNivel1, this.longZonaNivel1, this.etiquetaZonaNivel1, ref this.txtZonaNivel1);
                    result += this.ValidarLongitudNivelZona(2, zonaNivel2, this.longZonaNivel2, this.etiquetaZonaNivel2, ref this.txtZonaNivel2);
                    result += this.ValidarLongitudNivelZona(3, zonaNivel3, this.longZonaNivel3, this.etiquetaZonaNivel3, ref this.txtZonaNivel3);
                    result += this.ValidarLongitudNivelZona(4, zonaNivel4, this.longZonaNivel4, this.etiquetaZonaNivel4, ref this.txtZonaNivel4);

                    string zona = zonaNivel1 + zonaNivel2 + zonaNivel3 + zonaNivel4;
                    if (zona != "" && result == "")
                    {
                        zona = zona.PadRight(8, ' ');

                        if (this.validarZonaNivel1)
                        {
                            string query = "select count(ZONAZ1) from " + GlobalVar.PrefijoTablaCG + "GLM11 ";
                            query += " where CLASZ1 = '" + this.claseZonaNivel1 + "' ";
                            query += " and ZONAZ1 = '" + zona + "' and TIPOZ1 = 'D'";

                            int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                            if (cantRegistros == 0)
                            {
                                result = "Código de zona no válido";   //Falta traducir
                                this.ActiveControl = this.txtZonaNivel1;
                                this.txtZonaNivel1.Select();
                                this.txtZonaNivel1.Focus();
                            }
                            else this.codigoZona = zona;
                        }
                        else this.codigoZona = zona;
                    }
                }
                else
                {
                    this.codigoZona = "";
                    string resultValidarCodigoZonaNivel1 = this.ValidarCodigoZonaNoJerarq(1, ref this.radButtonTextBoxZonaNivel1, this.longZonaNivel1, this.etiquetaZonaNivel1, this.claseZonaNivel1, this.validarZonaNivel1);
                    if (resultValidarCodigoZonaNivel1 != "")
                    {
                        result += resultValidarCodigoZonaNivel1;
                        return (result);
                    }

                    if (this.claseZonaNivel2 != "")
                    {
                        string resultValidarCodigoZonaNivel2 = this.ValidarCodigoZonaNoJerarq(2, ref this.radButtonTextBoxZonaNivel2, this.longZonaNivel2, this.etiquetaZonaNivel2, this.claseZonaNivel2, this.validarZonaNivel2);
                        if (resultValidarCodigoZonaNivel2 != "")
                        {
                            result += resultValidarCodigoZonaNivel2;
                            return (result);
                        }
                    }

                    if (this.claseZonaNivel3 != "")
                    {
                        string resultValidarCodigoZonaNivel3 = this.ValidarCodigoZonaNoJerarq(3, ref this.radButtonTextBoxZonaNivel3, this.longZonaNivel3, this.etiquetaZonaNivel3, this.claseZonaNivel3, this.validarZonaNivel3);
                        if (resultValidarCodigoZonaNivel3 != "")
                        {
                            result += resultValidarCodigoZonaNivel3;
                            return (result);
                        }
                    }

                    if (this.claseZonaNivel4 != "")
                    {
                        string resultValidarCodigoZonaNivel4 = this.ValidarCodigoZonaNoJerarq(4, ref this.radButtonTextBoxZonaNivel4, this.longZonaNivel4, this.etiquetaZonaNivel4, this.claseZonaNivel4, this.validarZonaNivel4);
                        if (resultValidarCodigoZonaNivel4 != "")
                        {
                            result += resultValidarCodigoZonaNivel4;
                            return (result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de zona (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida la longitud del código de zona
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="valorZonaNivel"></param>
        /// <param name="longZonaNivel"></param>
        /// <param name="etiquetaZonaNivel"></param>
        /// <param name="controlZonaNivel"></param>
        /// <returns></returns>
        private string ValidarLongitudNivelZona(int nivel, string valorZonaNivel, int longZonaNivel, string etiquetaZonaNivel, ref Telerik.WinControls.UI.RadTextBoxControl controlZonaNivel)
        {
            string result = "";
            try
            {
                bool error = false;
                if (etiquetaZonaNivel != "")
                {
                    if (valorZonaNivel.Trim() == "")
                    {
                        result = "Es obligatorio informar el código de zona " + nivel + " no válido";   //Falta traducir
                        error = true;
                    }
                    else
                    {
                        if (valorZonaNivel.Length != longZonaNivel)
                        {
                            result = "Longitud del código de zona " + nivel + " no válido";   //Falta traducir
                            error = true;
                        }
                        else
                        {
                            if (valorZonaNivel.Replace(" ", string.Empty).Length != longZonaNivel)
                            {
                                result = "No se admiten blancos intercalados del código de zona " + nivel + " no válido";   //Falta traducir
                                error = true;
                            }
                        }
                    }
                    if (error)
                    {
                        this.ActiveControl = (Control)controlZonaNivel;
                        ((Control)controlZonaNivel).Select();
                        ((Control)controlZonaNivel).Focus();
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando la longitud del código de zona (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida ún código de zona no jerárquica
        /// </summary>
        /// <param name="tgTexBoxSelZona"></param>
        /// <returns></returns>
        private string ValidarCodigoZonaNoJerarq(int zonaNivel, ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxSelZona, int longZonaNivel, string etiquetaZonaNivel, string claseZonaNivel, bool validarZona)
        {
            string result = "";
            bool actualizavalorZonaNivel = false;
            try
            {
                string zona = radButtonTextBoxSelZona.Text.Trim();
                int posSeparador = zona.IndexOf('-');
                if (posSeparador != -1) zona = zona.Substring(0, posSeparador - 1).Trim();

                bool error = false;
                if (zona.Trim() == "")
                {
                    result = "Es obligatorio informar el código de zona";   //Falta traducir
                    error = true;
                }
                else
                {
                    if (zona.Length != longZonaNivel)
                    {
                        result = "Longitud del código de zona no válido";   //Falta traducir
                        error = true;
                    }
                    else
                    {
                        if (zona.Replace(" ", string.Empty).Length != longZonaNivel)
                        {
                            result = "No se admiten blancos intercalados del código de zona";   //Falta traducir
                            error = true;
                        }
                    }
                }

                if (!error)
                {
                    if (validarZona)
                    {
                        //Telerik.WinControls.UI.RadTextBoxControl textBoxZona = tgTexBoxSelZona.Textbox;       QUITAR !!!!
                        //result = this.ValidarLongitudNivelZona(zonaNivel, zona, longZonaNivel, etiquetaZonaNivel, ref textBoxZona);   QUITAR!!!!!

                        //Verificar el código de la zona
                        string query = "select count(ZONAZ1) from " + GlobalVar.PrefijoTablaCG + "GLM11 ";
                        query += " where CLASZ1 = '" + claseZonaNivel + "' ";
                        query += " and ZONAZ1 = '" + zona + "' and TIPOZ1 = 'D'";

                        int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                        if (cantRegistros == 0)
                        {
                            result = "Código de zona no válido";   //Falta traducir
                            this.ActiveControl = radButtonTextBoxSelZona;
                            radButtonTextBoxSelZona.Select();
                            radButtonTextBoxSelZona.Focus();
                            this.codigoZona = "";
                            return (result);
                        }
                        else
                        {
                            this.codigoZona += zona;
                            actualizavalorZonaNivel = true;
                        }
                    }
                    else
                    {
                        this.codigoZona += zona;
                        actualizavalorZonaNivel = true;
                    }
                }
                else
                {
                    this.ActiveControl = radButtonTextBoxSelZona;
                    radButtonTextBoxSelZona.Select();
                    radButtonTextBoxSelZona.Focus();
                    this.codigoZona = "";
                    return (result);
                }

                if (actualizavalorZonaNivel)
                {
                    switch (zonaNivel)
                    {
                        case 1:
                            this.valorZonaNivel1 = zona;
                            break;
                        case 2:
                            this.valorZonaNivel2 = zona;
                            break;
                        case 3:
                            this.valorZonaNivel3 = zona;
                            break;
                        case 4:
                            this.valorZonaNivel4 = zona;
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de zona (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Valida el código de reservado usuario
        /// </summary>
        /// <returns></returns>
        private string ValidarCodigoReservadoUser()
        {
            string result = "";

            try
            {
                this.codigoReservadoUser= this.radButtonTextBoxReservadoUser.Text.Trim();
                int posSeparador = this.codigoReservadoUser.IndexOf('-');
                if (posSeparador != -1) this.codigoReservadoUser = this.codigoReservadoUser.Substring(0, posSeparador - 1);

                string query = "select count(CODI3F) from " + GlobalVar.PrefijoTablaCG + "G3T03 ";
                query += "where CODI3F = '" + this.codigoReservadoUser + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Código de reservado usuario no válido\n\r";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de reservado usuario (" + ex.Message + ")";
            }

            if (result != "") this.codigoReservadoUser = "  ";

            return (result);
        }


        /// <summary>
        /// Validar pestaña Dirección Requerida
        /// </summary>
        /// <returns></returns>
        private string ValidarDireccionRequerida()
        {
            string result = "";

            try
            {
                string direccion1 = this.txtDirReqLinea1.Text.Trim();
                if (direccion1.Length == 0)
                {
                    result = "- Dirección de auxiliar no válida\n\r";     //Falta traducir
                    this.radPageViewDatos.SelectedPage = this.radPageViewPageDirReqSN;
                    this.ActiveControl = this.txtDirReqLinea1;
                    this.txtDirReqLinea1.Select();
                    this.txtDirReqLinea1.Focus();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Validar pestaña Dirección Formateada
        /// </summary>
        /// <returns></returns>
        private string ValidarDireccionFormateada()
        {
            string result = "";

            try
            {
                string tipoVia = this.txtDirForSiglas.Text.Trim();
                string nombreVia = this.txtDirForNombreVia.Text.Trim();

                if (tipoVia == "" && nombreVia == "")
                {
                    //Activar la pestaña de Dirección Formateada
                    this.radPageViewDatos.SelectedPage = this.radPageViewPageDirFormateada;

                    if (tipoVia == "")
                    {
                        result = "- Siglas de la vía pública no válida\n\r";     //Falta traducir
                        this.ActiveControl = this.txtDirForSiglas;
                        this.txtDirForSiglas.Select();
                        this.txtDirForSiglas.Focus();
                    }
                    else
                    {
                        result = "- Nombre de la vía pública no válido\n\r";     //Falta traducir
                        this.ActiveControl = this.txtDirForNombreVia;
                        this.txtDirForNombreVia.Select();
                        this.txtDirForNombreVia.Focus();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Validar pestaña Memo Terceros
        /// </summary>
        /// <returns></returns>
        private string ValidarMemoTerceros()
        {
            string result = "";

            //-------------- Validar IBAN ------------------
            if (!this.tgIBANMemoFormTer.IsValid())
            {
                result += "- Memo Código IBAN no válido\n\r";    //Falta traducir
                this.ActiveControl = this.tgIBANMemoFormTer;
                this.tgIBANMemoFormTer.Select();
                this.tgIBANMemoFormTer.Focus();
            }
            
            //-------------- Validar Cuenta Corriente ------------------    (DUDA Sólo si es una cuenta bancaria española ??)
            string banco = this.txtMemoFormTerCodCta1.Text.Trim();
            string oficina = this.txtMemoFormTerCodCta2.Text.Trim();
            string dc = this.txtMemoFormTerCodCta3.Text.Trim();
            string cuenta = this.txtMemoFormTerCodCta4.Text.Trim();

            if (banco != "" || oficina != "" || dc != "" || cuenta != "")
            {
                string resultValidarCuenta = this.ValidarCuentaCorrienteES(banco, oficina, dc, cuenta, ref this.txtMemoFormTerCodCta1, ref this.txtMemoFormTerCodCta2, ref this.txtMemoFormTerCodCta3, ref this.txtMemoFormTerCodCta4, this.lblMemoFormTerCodCta.Text);
                if (resultValidarCuenta != "") result += resultValidarCuenta;
            }

            //-------------- Validar Reservado Usuario ------------------
            string codReservadoUser = this.radButtonTextBoxReservadoUser.Text.Trim();
            if (codReservadoUser != "")
            {
                string resultValidarCodReservadoUser = this.ValidarCodigoReservadoUser();
                if (resultValidarCodReservadoUser != "")
                {
                    result += "- " + resultValidarCodReservadoUser + "\n\r";
                    this.ActiveControl = this.radButtonTextBoxReservadoUser;
                    this.radButtonTextBoxReservadoUser.Select();
                    this.radButtonTextBoxReservadoUser.Focus();
                }
            }

            //-------------- Validar Números de plazos ------------------
            string errorNumerosPlazos = "";
            string numerosPlazos = this.cmbMemoFormTerFormaPagoNumPlazos.Text;
            if (numerosPlazos != "")
            {
                try
                {
                    int numPlazos = Convert.ToInt16(numerosPlazos);
                    if (numPlazos > 72) errorNumerosPlazos = "- Memo Error en Forma de pago. Número de plazos no válido\n\r";    //Falta traducir
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    errorNumerosPlazos = "- Memo Error en Forma de pago. Número de plazos no válido\n\r";    //Falta traducir
                }
                if (errorNumerosPlazos != "")
                {
                    result += errorNumerosPlazos;
                    this.ActiveControl = this.cmbMemoFormTerFormaPagoNumPlazos;
                    this.cmbMemoFormTerFormaPagoNumPlazos.Select();
                    this.cmbMemoFormTerFormaPagoNumPlazos.Focus();
                }
            }

            //-------------- Validar Días fijos de pago 1 ------------------
            string errorDiasFijosPago = "";
            string diasFijosPago = this.cmbMemoFormTerFormaPagoDiasFijos1.Text;
            if (diasFijosPago != "")
            {
                try
                {
                    int diasFijo = Convert.ToInt16(diasFijosPago);
                    if (diasFijo > 31) errorDiasFijosPago = "- Memo Error en Forma de pago. Primer día fijo de pago no válido\n\r";    //Falta traducir
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    errorDiasFijosPago = "- Memo Error en Forma de pago. Primer día fijo de pago no válido\n\r";    //Falta traducir
                }
                if (errorDiasFijosPago != "")
                {
                    result += errorDiasFijosPago;
                    this.ActiveControl = this.cmbMemoFormTerFormaPagoDiasFijos1;
                    this.cmbMemoFormTerFormaPagoDiasFijos1.Select();
                    this.cmbMemoFormTerFormaPagoDiasFijos1.Focus();
                }
            }

            //-------------- Validar Días fijos de pago 2 ------------------
            errorDiasFijosPago = "";
            diasFijosPago = this.cmbMemoFormTerFormaPagoDiasFijos2.Text;
            if (diasFijosPago != "")
            {
                try
                {
                    int diasFijo = Convert.ToInt16(diasFijosPago);
                    if (diasFijo > 31) errorDiasFijosPago = "- Memo Error en Forma de pago. Segundo día de pago no válido\n\r";    //Falta traducir
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    errorDiasFijosPago = "- Memo Error en Forma de pago. Segundo día de pago no válido\n\r";    //Falta traducir
                }
                if (errorDiasFijosPago != "")
                {
                    result += errorDiasFijosPago;
                    this.ActiveControl = this.cmbMemoFormTerFormaPagoDiasFijos2;
                    this.cmbMemoFormTerFormaPagoDiasFijos2.Select();
                    this.cmbMemoFormTerFormaPagoDiasFijos2.Focus();
                }
            }

            //-------------- Validar Días fijos de pago 3 ------------------
            errorDiasFijosPago = "";
            diasFijosPago = this.cmbMemoFormTerFormaPagoDiasFijos3.Text;
            if (diasFijosPago != "")
            {
                try
                {
                    int diasFijo = Convert.ToInt16(diasFijosPago);
                    if (diasFijo > 31) errorDiasFijosPago = "- Memo Error en Forma de pago. Tercer día de pago no válido\n\r";    //Falta traducir
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    errorDiasFijosPago = "- Memo Error en Forma de pago. Tercer día de pago no válido\n\r";    //Falta traducir
                }
                if (errorDiasFijosPago != "")
                {
                    result += errorDiasFijosPago;
                    this.ActiveControl = this.cmbMemoFormTerFormaPagoDiasFijos3;
                    this.cmbMemoFormTerFormaPagoDiasFijos3.Select();
                    this.cmbMemoFormTerFormaPagoDiasFijos3.Focus();
                }
            }

            //-------------- Límites de crédito ------------------
            Int64 limiteInf = 0;
            Int64 limiteSup = 0;
            try
            {
                if (this.txtMemoFormTerLimiteCred1.Text.Trim() != "") limiteInf = Convert.ToInt64(this.txtMemoFormTerLimiteCred1.Text.Trim());
                if (this.txtMemoFormTerLimiteCred2.Text.Trim() != "") limiteSup = Convert.ToInt64(this.txtMemoFormTerLimiteCred2.Text.Trim());

                if (limiteInf > limiteSup && limiteSup > 0)
                {
                    result += "- Memo El límite de crédito inferior no puede ser mayor que el límite de crédito superior\n\r"; //Falta traducir
                    this.ActiveControl = this.txtMemoFormTerLimiteCred1;
                    this.txtMemoFormTerLimiteCred1.Select();
                    this.txtMemoFormTerLimiteCred1.Focus();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //Activar la pestaña de memo de Terceros si hubo error
            if (result != "") this.radPageViewDatos.SelectedPage = this.radPageViewPageMemoFormTerceros;

            return (result);
        }

        /// <summary>
        /// Validar pestaña Memo Bancos
        /// </summary>
        /// <returns></returns>
        private string ValidarMemoBancos()
        {
            string result = "";

            //-------------- Validar IBAN ------------------
            if (!this.tgIBANMemoFormBancos.IsValid())
            {
                result += "- Memo Código IBAN no válido\n\r";    //Falta traducir
                this.ActiveControl = this.tgIBANMemoFormBancos;
                this.tgIBANMemoFormBancos.Select();
                this.tgIBANMemoFormBancos.Focus();
            }

            //-------------- Validar Cuenta Corriente de Abonos ------------------    (DUDA Sólo si es una cuenta bancaria española ??)
            string resultValidarCuenta = "";
            string banco = this.txtMemoFormBancosCtaAbono1.Text.Trim();
            string oficina = this.txtMemoFormBancosCtaAbono2.Text.Trim();
            string dc = this.txtMemoFormBancosCtaAbono3.Text.Trim();
            string cuenta = this.txtMemoFormBancosCtaAbono4.Text.Trim();

            if (banco != "" || oficina != "" || dc != "" || cuenta != "")
            {
                resultValidarCuenta = this.ValidarCuentaCorrienteES(banco, oficina, dc, cuenta, ref this.txtMemoFormBancosCtaAbono1, ref this.txtMemoFormBancosCtaAbono2, ref this.txtMemoFormBancosCtaAbono3, ref this.txtMemoFormBancosCtaAbono4, this.lblPageMemoFormBancosCtaAbono.Text);
                if (resultValidarCuenta != "") result += resultValidarCuenta;
            }

            //-------------- Validar Cuenta Corriente de Adeudos ------------------    (DUDA Sólo si es una cuenta bancaria española ??)
            banco = this.txtMemoFormBancosCtaAdeudo1.Text.Trim();
            oficina = this.txtMemoFormBancosCtaAdeudo2.Text.Trim();
            dc = this.txtMemoFormBancosCtaAdeudo3.Text.Trim();
            cuenta = this.txtMemoFormBancosCtaAdeudo4.Text.Trim();

            if (banco != "" || oficina != "" || dc != "" || cuenta != "")
            {
                resultValidarCuenta = this.ValidarCuentaCorrienteES(banco, oficina, dc, cuenta, ref this.txtMemoFormBancosCtaAdeudo1, ref this.txtMemoFormBancosCtaAdeudo2, ref this.txtMemoFormBancosCtaAdeudo3, ref this.txtMemoFormBancosCtaAdeudo4, this.lblPageMemoFormBancosCtaAdeudo.Text);
                if (resultValidarCuenta != "") result += resultValidarCuenta;
            }

            //-------------- Validar Cuenta Corriente de Impagados  ------------------    (DUDA Sólo si es una cuenta bancaria española ??)
            banco = this.txtMemoFormBancosCtaImpagados1.Text.Trim();
            oficina = this.txtMemoFormBancosCtaImpagados2.Text.Trim();
            dc = this.txtMemoFormBancosCtaImpagados3.Text.Trim();
            cuenta = this.txtMemoFormBancosCtaImpagados4.Text.Trim();

            if (banco != "" || oficina != "" || dc != "" || cuenta != "")
            {
                resultValidarCuenta = this.ValidarCuentaCorrienteES(banco, oficina, dc, cuenta, ref this.txtMemoFormBancosCtaImpagados1, ref this.txtMemoFormBancosCtaImpagados2, ref this.txtMemoFormBancosCtaImpagados3, ref this.txtMemoFormBancosCtaImpagados4, this.lblPageMemoFormBancosCtaImpagados.Text);
                if (resultValidarCuenta != "") result += resultValidarCuenta;
            }

            //Activar la pestaña de memo de Bancos si hubo error
            if (result != "") this.radPageViewDatos.SelectedPage = this.radPageViewPageMemoFormBancos;

            return (result);
        }

        /// <summary>
        /// Valida una cuenta corriente (troceada) española
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="oficina"></param>
        /// <param name="dc"></param>
        /// <param name="cuenta"></param>
        /// <param name="txtCodCta1"></param>
        /// <param name="txtCodCta2"></param>
        /// <param name="txtCodCta3"></param>
        /// <param name="txtCodCta4"></param>
        /// <returns></returns>
        private string ValidarCuentaCorrienteES(string banco, string oficina, string dc, string cuenta, ref Telerik.WinControls.UI.RadTextBoxControl txtCodCta1, ref Telerik.WinControls.UI.RadTextBoxControl txtCodCta2, ref Telerik.WinControls.UI.RadTextBoxControl txtCodCta3, ref Telerik.WinControls.UI.RadTextBoxControl txtCodCta4, string etiquetaCuenta)
        {
            string result = "";

            try
            {
                if ((banco == "" || banco == "0000") && (oficina == "" || oficina == "0000") &&
                    (dc == "" || dc == "00") && (cuenta == "" || cuenta == "0000000000"))
                    return (result); //Cuenta no informada

                string resultValidarCuenta = "";

                if (banco == "" || banco == "0000" || banco.Length != 4) 
                {
                    resultValidarCuenta += "- Memo " + etiquetaCuenta + " Banco no válido\n\r";    //Falta traducir
                    this.ActiveControl = txtCodCta1;
                    txtCodCta1.Select();
                    txtCodCta1.Focus();
                }
                if (oficina == "" || oficina == "0000" || oficina.Length != 4)
                {
                    resultValidarCuenta += "- Memo " + etiquetaCuenta + " Oficina no válida\n\r";    //Falta traducir
                    this.ActiveControl = txtCodCta2;
                    txtCodCta2.Select();
                    txtCodCta2.Focus();
                }
                if (dc == "" || dc == "00" || dc.Length != 2)
                {
                    resultValidarCuenta += "- Memo " + etiquetaCuenta + " Dígito Control no válido\n\r";    //Falta traducir
                    this.ActiveControl = txtCodCta3;
                    txtCodCta3.Select();
                    txtCodCta3.Focus();
                }
                if (cuenta == "" || cuenta == "0000000000" || cuenta.Length != 10)
                {
                    resultValidarCuenta += "- Memo " + etiquetaCuenta + " Cuenta no válida\n\r";    //Falta traducir
                    this.ActiveControl = txtCodCta4;
                    txtCodCta4.Select();
                    txtCodCta4.Focus();
                }

                if (resultValidarCuenta == "")
                {
                    //Validar la cuenta
                    string cuentaCompleta = banco + oficina + dc + cuenta;
                    if ((!CuentasBancarias.ValidaCuentaBancaria(banco, oficina, dc, cuenta)) || (cuentaCompleta.Length != 20))
                    {
                        result += "- Memo " + etiquetaCuenta + " errónea\n\r";    //Falta traducir
                        this.ActiveControl = txtCodCta1;
                        txtCodCta1.Select();
                        txtCodCta1.Focus();
                    }
                }
                else
                {
                    result += resultValidarCuenta;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        #region Alta Cuenta de Auxiliar
        /// <summary>
        /// Devuelve los valores del formulario general que se insertarán o actualizarán en la tabla GLM05
        /// </summary>
        /// <param name="dauxad"></param>
        /// <returns></returns>
        private string GeneralDatosFormToTabla(ref string fonoma, ref string postma, ref string aparma, ref string zonama,
                                               ref string grupma, ref string pcifma, ref string nnitma, ref string grctma)
        {
            string result = "";

            try
            {
                fonoma = this.txtNumTelefono.Text.Trim();
                fonoma = (fonoma == "") ? " " : fonoma;

                if (this.tipoDir == "F")
                {
                    this.txtCP.Text = this.txtDirForCP.Text.Trim();
                }
                postma = this.txtCP.Text.Trim();
                postma = (postma == "") ? " " : postma;

                aparma = this.txtApdoPostal.Text.Trim();
                aparma = (aparma == "") ? " " : aparma;

                zonama = this.codigoZona.Trim();
                zonama = (zonama == "") ? " " : zonama;

                grupma = this.txtPrimDigVal.Text;
                grupma = (grupma == "") ? " " : grupma;

                if (this.codigoPais == null) pcifma = " ";
                else
                {
                    pcifma = this.codigoPais.Trim();
                    pcifma = (pcifma == "") ? " " : pcifma;
                }

                nnitma = this.txtNumIdTributaria.Text.Trim();
                nnitma = (nnitma == "") ? " " : nnitma;

                if (this.codigoGrupoCuentas == null) grctma = "";
                else
                {
                    grctma = this.codigoGrupoCuentas.Trim();
                    grctma = (grctma == "") ? " " : grctma;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error obteniendo los datos del formulario (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a una cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string fonoma = "";
                string postma = "";
                string aparma = "";
                string zonama = "";
                string grupma = "";
                string pcifma = "";
                string nnitma = "";
                string grctma = "";

                //Obtener los valores del formulario general que se insertarán en la tabla GLM05
                result = this.GeneralDatosFormToTabla(ref fonoma, ref postma, ref aparma, ref zonama,
                                                      ref grupma, ref pcifma, ref nnitma, ref grctma);
                if (result == "")
                {
                    string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                    //Dar de alta a la cuenta de auxiliar en la tabla del maestro de cuentas de auxiliares (GLM05)
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM05";
                    string query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "STATMA, TAUXMA, CAUXMA, NOMBMA, FONOMA, POSTMA, APARMA, ZONAMA, GRUPMA, PCIFMA, NNITMA, GRCTMA) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "'" + estado + "', '" + this.codigoTipoAux + "', '" + this.codigo + "', '";
                    query += this.txtNombreCuentaAux.Text + "', '" + fonoma + "', '" + postma + "', '";
                    query += aparma + "', '" + zonama + "', '" + grupma + "', '" + pcifma + "', '" + nnitma + "', '" + grctma + "')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM05", this.codigoTipoAux, this.codigo);

                    //Dar alta a la direccion
                    switch (this.tipoDir)
                    {
                        case "F":
                            result = this.AltaInfoDireccionFormateada();
                            break;
                        case "S":
                        case "N":
                            result = this.AltaInfoDireccionRequeridaSN();
                            break;
                    }

                    //Dar alta al memo
                    switch (tipoMemo)
                    {
                        case "0":
                            result += this.AltaInfoMemoSinFormato();
                            break;
                        case "1":
                            result += this.AltaInfoMemoTerceros();
                            break;
                        case "2":
                            result += this.AltaInfoMemoBancos();
                            break;
                    }
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
        /// Devuelve los valores del formulario del apartado Direccion Formateada que se insertarán o actualizarán en la tabla GLDM5
        /// </summary>
        /// <param name="dauxad"></param>
        /// <param name="alta">true - si se va a hacer un insert del registro   false - si es un update</param>
        /// <returns></returns>
        private string DireccionFormateadaDatosFormToTabla(ref string dauxad, bool alta)
        {
            string result = "";

            try
            {
                bool insertUpdateInfo = true;

                //Verificar que existan campos rellenos para insertar
                if (alta && 
                    (this.txtDirForSiglas.Text.Trim() == "" && 
                    this.txtDirForNombreVia.Text.Trim() == "" &&
                    this.txtDirForNumCasa.Text.Trim() == "" &&
                    this.txtDirForMunicipio.Text.Trim() == "" &&
                    this.txtDirForLinea3.Text.Trim() == "" &&
                    this.txtDirForLinea4.Text.Trim() == "" &&
                    this.txtDirForLinea5.Text.Trim() == ""))
                {
                    //No existe registro en la tabla y no se han informado los campos no hay que insertar el registro en la tabla
                    insertUpdateInfo = false;
                }

                if (insertUpdateInfo)
                {
                    string aux = "";
                    dauxad = this.txtDirForSiglas.Text.PadRight(2, ' ');
                    dauxad += this.txtDirForNombreVia.Text.PadRight(25, ' ');
                    dauxad += aux.PadRight(4, ' ');
                    dauxad += this.txtDirForNumCasa.Text.PadLeft(5, '0');
                    dauxad += this.txtDirForMunicipio.Text.PadRight(24, ' ');
                    dauxad += aux.PadRight(2, ' ');
                    dauxad += this.txtDirForLinea3.Text.PadRight(36, ' ');
                    dauxad += this.txtDirForLinea4.Text.PadRight(36, ' ');
                    dauxad += this.txtDirForLinea5.Text.PadRight(36, ' ');
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a la direccion formateada de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfoDireccionFormateada()
        {
            string result = "";

            try
            {
                string dauxad = "";

                //Obtener los valores del formulario del apartado Direccion Formateada que se insertarán en la tabla GLDM5
                result = this.DireccionFormateadaDatosFormToTabla(ref dauxad, true);

                if (result == "" && dauxad != "")
                {
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLDM5";
                    string query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "TAUXAD, CAUXAD, DAUXAD) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "'" + this.codigoTipoAux + "', '" + this.codigo + "', '" + dauxad + "')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos de la dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Direccion RequeridaSN que se insertarán o actualizarán en la tabla GLDM5
        /// </summary>
        /// <param name="dauxad"></param>
        /// <param name="alta">true - si se va a hacer un insert del registro   false - si es un update</param>
        /// <returns></returns>
        private string DireccionRequeridaSNDatosFormToTabla(ref string dauxad, bool alta)
        {
            string result = "";

            try
            {
                bool insertUpdateInfo = true;

                //Verificar que existan campos rellenos para insertar
                if (alta &&
                    (this.txtDirReqLinea1.Text.Trim() == "" &&
                    this.txtDirReqLinea2.Text.Trim() == "" &&
                    this.txtDirForLinea3.Text.Trim() == "" &&
                    this.txtDirForLinea4.Text.Trim() == "" &&
                    this.txtDirForLinea5.Text.Trim() == ""))
                {
                    //No existe registro en la tabla y no se han informado los campos no hay que insertar el registro en la tabla
                    insertUpdateInfo = false;
                }

                if (insertUpdateInfo)
                {
                    dauxad = this.txtDirReqLinea1.Text.PadRight(36, ' ');
                    dauxad += this.txtDirReqLinea2.Text.PadRight(36, ' ');
                    dauxad += this.txtDirReqLinea3.Text.PadRight(36, ' ');
                    dauxad += this.txtDirReqLinea4.Text.PadRight(36, ' ');
                    dauxad += this.txtDirReqLinea5.Text.PadRight(36, ' ');
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a la direccion sin formato de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfoDireccionRequeridaSN()
        {
            string result = "";
            try
            {
                string dauxad = "";
                
                //Obtener los valores del formulario del apartado Direccion RequeridaSN que se insertarán en la tabla GLDM5
                result = this.DireccionRequeridaSNDatosFormToTabla(ref dauxad, true);

                if (result == "" && dauxad != "")
                {
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLDM5";
                    string query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "TAUXAD, CAUXAD, DAUXAD) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += "'" + this.codigoTipoAux + "', '" + this.codigo + "', '" + dauxad + "')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos de la dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta el memo en la tabla GLMM5
        /// </summary>
        /// <param name="glmm5"></param>
        /// <returns></returns>
        private string AltaInfoMemoGLMM5(GLMM5ValoresVacios glmm5)
        {
            string result = "";

            try
            {
                glmm5.fillgb = this.txtEmail.Text.Trim();

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLMM5";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TAUXAD, CAUXAD, MEAXAD, MEM1GB, MEM2GB, MEM3GB, ";
                query += "ABO1GB, ABO2GB, ABO3GB, ABO4GB, GAS1GB, GAS2GB, GAS3GB, GAS4GB, IMP1GB, IMP2GB, IMP3GB, IMP4GB, ";
                query += "LIMDGB, COMDGB, CORDGB, CORSGB, DIAIGB, DIAMGB, DIAPGB, ";
                query += "FOR1GB, FOR2GB, FOR3GB, FOR4GB, FOR5GB, FOR6GB, ";
                query += "FOD1GB, FOD2GB, FOD3GB, FOD4GB, FOD5GB, FOD6GB, ";
                query += "FOM1GB, FOM2GB, FOM3GB, FOM4GB, FOM5GB, FOM6GB, FILLGB) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigoTipoAux + "', '" + this.codigo + "', '" + glmm5.meaxad + "', ";
                query += "'" + glmm5.mem1gb + "', '" + glmm5.mem2gb + "', '" + glmm5.mem3gb + "', ";
                query += glmm5.abo1gb + ", " + glmm5.abo2gb + ", " + glmm5.abo3gb + ", " + glmm5.abo4gb + ", ";
                query += glmm5.gas1gb + ", " + glmm5.gas2gb + ", " + glmm5.gas3gb + ", " + glmm5.gas4gb + ", ";
                query += glmm5.imp1gb + ", " + glmm5.imp2gb + ", " + glmm5.imp3gb + ", " + glmm5.imp4gb + ", ";
                query += glmm5.limdgb + ", " + glmm5.comdgb + ", " + glmm5.cordgb + ", " + glmm5.corsgb + ", ";
                query += glmm5.diaigb + ", " + glmm5.diamgb + ", " + glmm5.diapgb + ", ";
                query += glmm5.for1gb + ", " + glmm5.for2gb + ", " + glmm5.for3gb + ", " + glmm5.for4gb + ", " + glmm5.for5gb + ", " + glmm5.for6gb + ", ";
                query += glmm5.fod1gb + ", " + glmm5.fod2gb + ", " + glmm5.fod3gb + ", " + glmm5.fod4gb + ", " + glmm5.fod5gb + ", " + glmm5.fod6gb + ", ";
                query += glmm5.fom1gb + ", " + glmm5.fom2gb + ", " + glmm5.fom3gb + ", " + glmm5.fom4gb + ", " + glmm5.fom5gb + ", " + glmm5.fom6gb + ", ";
                query += "'" + glmm5.fillgb + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Memo Sin Formato que se insertarán o actualizarán en la tabla GLMM5
        /// </summary>
        /// <param name="meaxad"></param>
        /// <param name="alta">true - si se va a hacer un insert del registro   false - si es un update</param>
        /// <returns></returns>
        private string MemoSinFormatoDatosFormToTabla(ref string meaxad, bool alta)
        {
            string result = "";

            try
            {
                bool insertUpdateInfo = true;

                //Verificar que existan campos rellenos para insertar
                if (alta &&
                    (this.txtMemoFormNoLinea1.Text.Trim() == "" &&
                    this.txtMemoFormNoLinea2.Text.Trim() == "" &&
                    this.txtMemoFormNoLinea3.Text.Trim() == "" &&
                    this.txtMemoFormNoLinea4.Text.Trim() == "" &&
                    this.txtMemoFormNoLinea5.Text.Trim() == ""))
                {
                    //No existe registro en la tabla y no se han informado los campos no hay que insertar el registro en la tabla
                    insertUpdateInfo = false;
                }

                if (insertUpdateInfo)
                {
                    meaxad = this.txtMemoFormNoLinea1.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea2.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea3.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea4.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea5.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea6.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormNoLinea7.Text.PadRight(36, ' ');
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta al memo sin formato de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfoMemoSinFormato()
        {
            string result = "";

            try
            {
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Sin Formato que se insertarán en la tabla GLMM5
                result = this.MemoSinFormatoDatosFormToTabla(ref glmm5.meaxad, true);

                if (result == "" && glmm5.meaxad != " ") result = this.AltaInfoMemoGLMM5(glmm5);
                
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Memo Terceros que se insertarán o actualizarán en la tabla GLMM5
        /// </summary>
        /// <param name="meaxad"></param>
        /// <param name="mem1gb"></param>
        /// <param name="mem2gb"></param>
        /// <param name="alta">true - si se va a hacer un insert del registro   false - si es un update</param>
        /// <returns></returns>
        private string MemoTercerosDatosFormToTabla(ref string meaxad, ref string mem1gb, ref string mem2gb, bool alta)
        {
            string result = "";

            try
            {
                bool insertUpdateInfo = true;

                //Verificar que existan campos rellenos para insertar
                if (alta &&
                    (this.txtMemoFormTerMemo1.Text.Trim() == "" &&
                    this.txtMemoFormTerMemo2.Text.Trim() == "" &&
                    this.txtMemoFormTerMemo3.Text.Trim() == "" &&
                    this.txtMemoFormTerNombre.Text.Trim() == "" &&
                    this.txtMemoFormTerDir.Text.Trim() == "" &&
                    this.txtMemoFormTerCodCta1.Text.Trim() == "" &&
                    this.txtMemoFormTerCodCta2.Text.Trim() == "" &&
                    this.txtMemoFormTerCodCta3.Text.Trim() == "" &&
                    this.txtMemoFormTerCodCta4.Text.Trim() == "" &&
                    this.codigoReservadoUser == null &&
                    this.cmbMemoFormTerFormaPagoAnoPrimVto.Text.Trim() == "" &&
                    this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Text.Trim() == "" &&
                    this.cmbMemoFormTerFormaPagoNumPlazos.Text.Trim() == "" &&
                    this.cmbMemoFormTerFormaPagoDiasFijos1.Text.Trim() == "00" &&
                    this.cmbMemoFormTerFormaPagoDiasFijos2.Text.Trim() == "00" &&
                    this.cmbMemoFormTerFormaPagoDiasFijos3.Text.Trim() == "00" &&
                    this.txtMemoFormTerLimiteCred1.Text.Trim() == "" &&
                    this.txtMemoFormTerLimiteCred2.Text.Trim() == "" &&
                    this.tgIBANMemoFormTer.IBANCodigo == "" &&
                    this.txtMemoFormTerSWIFT.Text.Trim() == "" &&
                    this.txtMemoFormTerMandatoUnico.Text.Trim() == ""))
                {
                    //No existe registro en la tabla y no se han informado los campos no hay que insertar el registro en la tabla
                    insertUpdateInfo = false;
                }

                if (insertUpdateInfo)
                {
                    meaxad = this.txtMemoFormTerMemo1.Text.PadRight(31, ' ');
                    meaxad += this.txtMemoFormTerMemo2.Text.PadRight(31, ' ');
                    meaxad += this.txtMemoFormTerMemo3.Text.PadRight(30, ' ');
                    meaxad += this.txtMemoFormTerNombre.Text.PadRight(36, ' ');
                    meaxad += this.txtMemoFormTerDir.Text.PadRight(34, ' ');
                    meaxad += this.txtMemoFormTerPoblacion.Text.PadRight(30, ' ');
                    meaxad += this.txtMemoFormTerCodCta1.Text.PadRight(4, ' ');
                    meaxad += this.txtMemoFormTerCodCta2.Text.PadRight(4, ' ');
                    meaxad += this.txtMemoFormTerCodCta3.Text.PadRight(2, ' ');
                    meaxad += this.txtMemoFormTerCodCta4.Text.PadRight(10, ' ');
                    if (this.codigoReservadoUser == null) meaxad += "  ";
                    else meaxad += this.codigoReservadoUser.PadRight(2, ' ');

                    string anoPrimVto = this.cmbMemoFormTerFormaPagoAnoPrimVto.Text.Trim();
                    if (anoPrimVto == "") anoPrimVto = "0";
                    meaxad += anoPrimVto;

                    string numPlazosPrimVto = this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Text.Trim();
                    if (numPlazosPrimVto == "") numPlazosPrimVto = "0";
                    meaxad += numPlazosPrimVto;

                    string tipoPlazo = "1";
                    if (this.rbMemoFormTerFormaPagoTipoPlazo15.IsChecked) tipoPlazo = "2";
                    meaxad += tipoPlazo;

                    string numPlazos = this.cmbMemoFormTerFormaPagoNumPlazos.Text.Trim();
                    if (numPlazos == "") numPlazos = "01";
                    meaxad += numPlazos.PadLeft(2, '0');

                    string diasFijosPago1 = this.cmbMemoFormTerFormaPagoDiasFijos1.Text.Trim();
                    if (diasFijosPago1 == "") diasFijosPago1 = "00";
                    meaxad += diasFijosPago1.PadLeft(2, '0');

                    string diasFijosPago2 = this.cmbMemoFormTerFormaPagoDiasFijos2.Text.Trim();
                    if (diasFijosPago2 == "") diasFijosPago2 = "00";
                    meaxad += diasFijosPago2.PadLeft(2, '0');

                    string diasFijosPago3 = this.cmbMemoFormTerFormaPagoDiasFijos3.Text.Trim();
                    if (diasFijosPago3 == "") diasFijosPago3 = "00";
                    meaxad += diasFijosPago3.PadLeft(2, '0');

                    string tipoDiaFijjo = "0";
                    if (this.rbMemoFormTerFormaPagoTipoDiaFijoSemana.IsChecked) tipoDiaFijjo = "1";
                    meaxad += tipoDiaFijjo;

                    string limiteSup = this.txtMemoFormTerLimiteCred2.Text.Trim();
                    if (limiteSup.Length < 13) limiteSup = limiteSup.PadRight(13, ' ');
                    meaxad += limiteSup;

                    string limiteInf = this.txtMemoFormTerLimiteCred1.Text.Trim();
                    if (limiteInf.Length < 13) limiteInf = limiteInf.PadRight(13, ' ');
                    meaxad += limiteInf;

                    //mem1gb = this.txtMemoFormTerIBAN.Text;
                    mem1gb = this.tgIBANMemoFormTer.IBANCodigo;
                    if (mem1gb == "") mem1gb = " ";

                    mem2gb = this.txtMemoFormTerSWIFT.Text.PadRight(11, ' ') + this.txtMemoFormTerMandatoUnico.Text.PadRight(22, ' ');
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error obteniendo los datos del formulario del apartado memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta al memo de terceros de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfoMemoTerceros()
        {
            string result = "";

            try
            {
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Terceros que se insertarán en la tabla GLMM5
                result = this.MemoTercerosDatosFormToTabla(ref glmm5.meaxad, ref glmm5.mem1gb, ref glmm5.mem2gb, true);

                if (result == "" && glmm5.meaxad != " ") result = this.AltaInfoMemoGLMM5(glmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve los valores del formulario del apartado Memo Bancos que se insertarán o actualizarán en la tabla GLMM5
        /// </summary>
        /// <param name="abo1gb"></param>
        /// <param name="abo2gb"></param>
        /// <param name="abo3gb"></param>
        /// <param name="abo4gb"></param>
        /// <param name="gas1gb"></param>
        /// <param name="gas2gb"></param>
        /// <param name="gas3gb"></param>
        /// <param name="gas4gb"></param>
        /// <param name="imp1gb"></param>
        /// <param name="imp2gb"></param>
        /// <param name="imp3gb"></param>
        /// <param name="imp4gb"></param>
        /// <param name="limdgb"></param>
        /// <param name="mem1gb"></param>
        /// <param name="mem2gb"></param>
        /// <param name="mem3gb"></param>
        /// <param name="alta">true - si se va a hacer un insert del registro   false - si es un update</param>
        /// <returns></returns>
        private string MemoBancosDatosFormToTabla(ref string abo1gb, ref string abo2gb, ref string abo3gb, ref string abo4gb, 
                                                  ref string gas1gb, ref string gas2gb, ref string gas3gb, ref string gas4gb, 
                                                  ref string imp1gb, ref string imp2gb, ref string imp3gb, ref string imp4gb, 
                                                  ref string limdgb, ref string mem1gb, ref string mem2gb, ref string mem3gb,
                                                  ref bool alta)
        {
            string result = "";

            try
            {
                bool insertUpdateInfo = true;

                //Verificar que existan campos rellenos para insertar
                if (alta &&
                    (this.txtMemoFormBancosCtaAbono1.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAbono2.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAbono3.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAbono4.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAdeudo1.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAdeudo2.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAdeudo3.Text.Trim() == "" &&
                    this.txtMemoFormBancosCtaAdeudo4.Text.Trim() == "" &&
                    this.txtMemoFormBancosLimiteDto.Text.Trim() == "" &&
                    this.txtMemoFormBancosMemo.Text.Trim() == "" &&
                    this.tgIBANMemoFormBancos.IBANCodigo.Trim() == "" &&
                    this.txtMemoFormBancosSWIFT.Text.Trim() == ""
                    ))
                {
                    //No existe registro en la tabla y no se han informado los campos no hay que insertar el registro en la tabla
                    insertUpdateInfo = false;
                }

                if (insertUpdateInfo)
                {
                    abo1gb = this.txtMemoFormBancosCtaAbono1.Text.PadRight(4, '0');
                    abo2gb = this.txtMemoFormBancosCtaAbono2.Text.PadRight(4, '0');
                    abo3gb = this.txtMemoFormBancosCtaAbono3.Text.PadRight(2, '0');
                    abo4gb = this.txtMemoFormBancosCtaAbono4.Text.PadRight(10, '0');

                    gas1gb = this.txtMemoFormBancosCtaAdeudo1.Text.PadRight(4, '0');
                    gas2gb = this.txtMemoFormBancosCtaAdeudo2.Text.PadRight(4, '0');
                    gas3gb = this.txtMemoFormBancosCtaAdeudo3.Text.PadRight(2, '0');
                    gas4gb = this.txtMemoFormBancosCtaAdeudo4.Text.PadRight(10, '0');

                    imp1gb = this.txtMemoFormBancosCtaAbono1.Text.PadRight(4, '0');
                    imp2gb = this.txtMemoFormBancosCtaAbono2.Text.PadRight(4, '0');
                    imp3gb = this.txtMemoFormBancosCtaAbono3.Text.PadRight(2, '0');
                    imp4gb = this.txtMemoFormBancosCtaAbono4.Text.PadRight(10, '0');

                    limdgb = this.txtMemoFormBancosLimiteDto.Text.Trim();
                    if (limdgb == "") limdgb = "0";

                    mem1gb = this.txtMemoFormBancosMemo.Text.PadRight(33, ' ');
                    //mem2gb = this.txtMemoFormBancosIBAN.Text.PadRight(33, ' ');
                    mem2gb = this.tgIBANMemoFormBancos.IBANCodigo.PadRight(33, ' ');
                    mem3gb = this.txtMemoFormBancosSWIFT.Text.PadRight(33, ' ');
                }

                alta = insertUpdateInfo;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result = "Error obteniendo los datos del formulario del apartado memo (" + ex.Message + ")";   //Falta traducir
                alta = false;
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta al memo de bancos de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfoMemoBancos()
        {
            string result = "";

            try
            {
                bool alta = true;
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Bancos que se insertarán en la tabla GLMM5
                result = this.MemoBancosDatosFormToTabla(ref glmm5.abo1gb, ref glmm5.abo2gb, ref glmm5.abo3gb, ref glmm5.abo4gb,
                                                         ref glmm5.gas1gb, ref glmm5.gas2gb, ref glmm5.gas3gb, ref glmm5.gas4gb,
                                                         ref glmm5.imp1gb, ref glmm5.imp2gb, ref glmm5.imp3gb, ref glmm5.imp4gb,
                                                         ref glmm5.limdgb, ref glmm5.mem1gb, ref glmm5.mem2gb, ref glmm5.mem3gb,
                                                         ref alta);

                if (result == "" && alta) result = this.AltaInfoMemoGLMM5(glmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        #endregion

        #region Actualizar Cuenta de Auxiliar
        /// <summary>
        /// Actualizar una cuenta de actualizar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string fonoma = "";
                string postma = "";
                string aparma = "";
                string zonama = "";
                string grupma = "";
                string pcifma = "";
                string nnitma = "";
                string grctma = "";

                //Obtener los valores del formulario general que se insertarán en la tabla GLM05
                result = this.GeneralDatosFormToTabla(ref fonoma, ref postma, ref aparma, ref zonama,
                                                      ref grupma, ref pcifma, ref nnitma, ref grctma);

                if (result == "")
                {
                    string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                    //Actualizar la cuenta de auxiliar en la tabla del maestro de cuentas de auxiliares (GLM05)
                    string query = "update " + GlobalVar.PrefijoTablaCG + "GLM05 set ";
                    query += "STATMA = '" + estado + "', ";
                    query += "NOMBMA = '" + this.txtNombreCuentaAux.Text + "', ";
                    query += "FONOMA = '" + fonoma + "', ";
                    query += "POSTMA = '" + postma + "', ";
                    query += "APARMA = '" + aparma + "', ";
                    query += "ZONAMA = '" + zonama + "', ";
                    query += "GRUPMA = '" + grupma + "', ";
                    query += "PCIFMA = '" + pcifma + "', ";
                    query += "NNITMA = '" + nnitma + "', ";
                    query += "GRCTMA = '" + grctma + "' ";
                    query += "where TAUXMA = '" + this.codigoTipoAux + "' and CAUXMA = '" + this.codigo + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM05", this.codigoTipoAux, this.codigo);

                    //Verificar si existe el registro para actualizar, sino insertarlo !!!!! AQUI MODIFICAR

                    //Actualizar la direccion o insertarla
                    switch (this.tipoDir)
                    {
                        case "F":
                            if (existenDatosDireccion) result = this.ActualizarInfoDireccionFormateada();
                            else result = this.AltaInfoDireccionFormateada();
                            break;
                        case "S":
                        case "N":
                            if (existenDatosDireccion) result = this.ActualizarInfoDireccionRequeridaSN();
                            else result = this.AltaInfoDireccionRequeridaSN();
                            break;
                    }

                    //Actualizar el memo
                    switch (tipoMemo)
                    {
                        case "0":
                            if (existenDatosMemo) result += this.ActualizarInfoMemoSinFormato();
                            else result += this.AltaInfoMemoSinFormato();
                            break;
                        case "1":
                            if (existenDatosMemo) result += this.ActualizarInfoMemoTerceros();
                            else result += this.AltaInfoMemoTerceros();
                            break;
                        case "2":
                            if (existenDatosMemo) result += this.ActualizarInfoMemoBancos();
                            else result += this.AltaInfoMemoBancos();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar la direccion formateada de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoDireccionFormateada()
        {
            string result = "";

            try
            {
                string dauxad = "";

                //Obtener los valores del formulario del apartado Direccion Formateada que se insertarán en la tabla GLDM5
                result = this.DireccionFormateadaDatosFormToTabla(ref dauxad, false);

                if (result == "")
                {
                    string query = "update " + GlobalVar.PrefijoTablaCG + "GLDM5 set ";
                    query += "DAUXAD = '" + dauxad + "' ";
                    query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos de la dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar la direccion sin formato de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoDireccionRequeridaSN()
        {
            string result = "";

            try
            {
                string dauxad = "";

                //Obtener los valores del formulario del apartado Direccion RequeridaSN que se insertarán en la tabla GLDM5
                result = this.DireccionRequeridaSNDatosFormToTabla(ref dauxad, false);

                if (result == "")
                {
                    string query = "update " + GlobalVar.PrefijoTablaCG + "GLDM5 set ";
                    query += "DAUXAD = '" + dauxad + "' ";
                    query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos de la dirección (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }


        /// <summary>
        /// Inserta el memo en la tabla GLMM5
        /// </summary>
        /// <param name="glmm5"></param>
        /// <returns></returns>
        private string AlctualizarInfoMemoGLMM5(GLMM5ValoresVacios glmm5)
        {
            string result = "";

            try
            {
                glmm5.fillgb = this.txtEmail.Text.Trim();

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLMM5 set ";
                query += "MEAXAD = '" + glmm5.meaxad + "', ";
                query += "MEM1GB = '" + glmm5.mem1gb + "', ";
                query += "MEM2GB = '" + glmm5.mem2gb + "', ";
                query += "MEM3GB = '" + glmm5.mem3gb + "', ";
                query += "ABO1GB = " + glmm5.abo1gb + ", ";
                query += "ABO2GB = " + glmm5.abo2gb + ", ";
                query += "ABO3GB = " + glmm5.abo3gb + ", ";
                query += "ABO4GB = " + glmm5.abo4gb + ", ";
                query += "GAS1GB = " + glmm5.gas1gb + ", ";
                query += "GAS2GB = " + glmm5.gas2gb + ", ";
                query += "GAS3GB = " + glmm5.gas3gb + ", ";
                query += "GAS4GB = " + glmm5.gas4gb + ", ";
                query += "IMP1GB = " + glmm5.imp1gb + ", ";
                query += "IMP2GB = " + glmm5.imp2gb + ", ";
                query += "IMP3GB = " + glmm5.imp3gb + ", ";
                query += "IMP4GB = " + glmm5.imp4gb + ", ";
                query += "LIMDGB = " + glmm5.limdgb + ", ";
                query += "COMDGB = " + glmm5.comdgb + ", ";
                query += "CORDGB = " + glmm5.cordgb + ", ";
                query += "CORSGB = " + glmm5.corsgb + ", ";
                query += "DIAIGB = " + glmm5.diaigb + ", ";
                query += "DIAMGB = " + glmm5.diamgb + ", ";
                query += "DIAPGB = " + glmm5.diapgb + ", ";
                query += "FOR1GB = " + glmm5.for1gb + ", ";
                query += "FOR2GB = " + glmm5.for2gb + ", ";
                query += "FOR3GB = " + glmm5.for3gb + ", ";
                query += "FOR4GB = " + glmm5.for4gb + ", ";
                query += "FOR5GB = " + glmm5.for5gb + ", ";
                query += "FOR6GB = " + glmm5.for6gb + ", ";
                query += "FOD1GB = " + glmm5.fod1gb + ", ";
                query += "FOD2GB = " + glmm5.fod2gb + ", ";
                query += "FOD3GB = " + glmm5.fod3gb + ", ";
                query += "FOD4GB = " + glmm5.fod4gb + ", ";
                query += "FOD5GB = " + glmm5.fod5gb + ", ";
                query += "FOD6GB = " + glmm5.fod6gb + ", ";
                query += "FOM1GB = " + glmm5.fom1gb + ", ";
                query += "FOM2GB = " + glmm5.fom2gb + ", ";
                query += "FOM3GB = " + glmm5.fom3gb + ", ";
                query += "FOM4GB = " + glmm5.fom4gb + ", ";
                query += "FOM5GB = " + glmm5.fom5gb + ", ";
                query += "FOM6GB = " + glmm5.fom6gb + ", ";
                query += "FILLGB = '" + glmm5.fillgb + "' ";
                query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualizar el memo sin formato de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoMemoSinFormato()
        {
            string result = "";

            try
            {
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Sin Formato que se insertarán en la tabla GLMM5
                result = this.MemoSinFormatoDatosFormToTabla(ref glmm5.meaxad, false);

                if (result == "") result = this.AlctualizarInfoMemoGLMM5(glmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar el memo de terceros de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoMemoTerceros()
        {
            string result = "";

            try
            {
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Terceros que se insertarán en la tabla GLMM5
                result = this.MemoTercerosDatosFormToTabla(ref glmm5.meaxad, ref glmm5.mem1gb, ref glmm5.mem2gb, false);

                if (result == "") result = this.AlctualizarInfoMemoGLMM5(glmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar el memo de bancos de la cuenta de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfoMemoBancos()
        {
            string result = "";

            try
            {
                bool alta = false;
                GLMM5ValoresVacios glmm5 = new GLMM5ValoresVacios();

                //Obtener los valores del formulario del apartado Memo Bancos que se insertarán en la tabla GLMM5
                result = this.MemoBancosDatosFormToTabla(ref glmm5.abo1gb, ref glmm5.abo2gb, ref glmm5.abo3gb, ref glmm5.abo4gb,
                                                         ref glmm5.gas1gb, ref glmm5.gas2gb, ref glmm5.gas3gb, ref glmm5.gas4gb,
                                                         ref glmm5.imp1gb, ref glmm5.imp2gb, ref glmm5.imp3gb, ref glmm5.imp4gb,
                                                         ref glmm5.limdgb, ref glmm5.mem1gb, ref glmm5.mem2gb, ref glmm5.mem3gb, 
                                                         ref alta);

                if (result == "") result = this.AlctualizarInfoMemoGLMM5(glmm5);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos del memo (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        #endregion

        /// <summary>
        /// Junta los niveles de zona y los devuelve en un solo campo
        /// </summary>
        /// <returns></returns>
        private string ObtenerZona()
        {
            string zona = "";

            try
            {
                if (this.zonaJerarq)
                {
                    zona += this.txtZonaNivel1.Text.PadRight(this.longZonaNivel1, ' ');
                    zona += this.txtZonaNivel2.Text.PadRight(this.longZonaNivel2, ' ');
                    zona += this.txtZonaNivel3.Text.PadRight(this.longZonaNivel3, ' ');
                    zona += this.txtZonaNivel4.Text.PadRight(this.longZonaNivel4, ' ');
                }
                else
                {
                    zona += this.radButtonTextBoxZonaNivel1.Text.PadRight(this.longZonaNivel1, ' ');
                    zona += this.radButtonTextBoxZonaNivel2.Text.PadRight(this.longZonaNivel2, ' ');
                    zona += this.radButtonTextBoxZonaNivel3.Text.PadRight(this.longZonaNivel3, ' ');
                    zona += this.radButtonTextBoxZonaNivel4.Text.PadRight(this.longZonaNivel4, ' ');
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (zona);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HabilitarSiNoCrearGrupoCtas()
        {
            bool crearGrupoCtas = aut.CrearElemento(autClaseElementoGLT08);
            if (crearGrupoCtas) utiles.ButtonEnabled(ref this.radButtonCrearGrupoCuentas, true);                
            else utiles.ButtonEnabled(ref this.radButtonCrearGrupoCuentas, false);

            if (this.radButtonTextBoxGrupoCtas.Text.Trim() != "")
            {
                //Verificar si el usuario tiene autorización a modificar el grupo de cuentas de auxiliar
                string elemento = this.codigoTipoAux + this.codigoGrupoCuentas;

                bool operarModificarGrupoCtas = aut.Validar(autClaseElementoGLT08, autGrupoGLT08, elemento, autOperModificaGLT08);
                if (!operarModificarGrupoCtas) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombreCuentaAux;
                    this.txtNombreCuentaAux.Select(0, 0);
                    this.txtNombreCuentaAux.Focus();
                }
            }
            else
            {
                this.ActiveControl = this.txtNombreCuentaAux;
                this.txtNombreCuentaAux.Select(0, 0);
                this.txtNombreCuentaAux.Focus();
            }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de Direccion RequeridaSN
        /// </summary>
        private void ActualizaValoresOrigenControlesDireccionRequeridaSN()
        {
            this.txtDirReqLinea1.Tag = this.txtDirReqLinea1.Text;
            this.txtDirReqLinea2.Tag = this.txtDirReqLinea2.Text;
            this.txtDirReqLinea3.Tag = this.txtDirReqLinea3.Text;
            this.txtDirReqLinea4.Tag = this.txtDirReqLinea4.Text;
            this.txtDirReqLinea5.Tag = this.txtDirReqLinea5.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de Direccion Formateada
        /// </summary>
        private void ActualizaValoresOrigenControlesDireccionFormateada()
        {
            this.txtDirForSiglas.Tag = this.txtDirForSiglas.Text;
            this.txtDirForNombreVia.Tag = this.txtDirForNombreVia.Text;
            this.txtDirForNumCasa.Tag = this.txtDirForNumCasa.Text;
            this.txtDirForCP.Tag = this.txtDirForCP.Text;
            this.txtDirForMunicipio.Tag = this.txtDirForMunicipio.Text;
            this.txtDirForLinea3.Tag = this.txtDirForLinea3.Text;
            this.txtDirForLinea4.Tag = this.txtDirForLinea4.Text;
            this.txtDirForLinea5.Tag = this.txtDirForLinea5.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de MemoSinFormato
        /// </summary>
        private void ActualizaValoresOrigenControlesMemoSinFormato()
        {
            this.txtMemoFormNoLinea1.Tag = this.txtMemoFormNoLinea1.Text;
            this.txtMemoFormNoLinea2.Tag = this.txtMemoFormNoLinea2.Text;
            this.txtMemoFormNoLinea3.Tag = this.txtMemoFormNoLinea3.Text;
            this.txtMemoFormNoLinea4.Tag = this.txtMemoFormNoLinea4.Text;
            this.txtMemoFormNoLinea5.Tag = this.txtMemoFormNoLinea5.Text;
            this.txtMemoFormNoLinea6.Tag = this.txtMemoFormNoLinea6.Text;
            this.txtMemoFormNoLinea7.Tag = this.txtMemoFormNoLinea7.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de MemoTerceros
        /// </summary>
        private void ActualizaValoresOrigenControlesMemoTerceros()
        {
            this.txtMemoFormTerMemo1.Tag = this.txtMemoFormTerMemo1.Text;
            this.txtMemoFormTerMemo2.Tag = this.txtMemoFormTerMemo2.Text;
            this.txtMemoFormTerMemo3.Tag = this.txtMemoFormTerMemo3.Text;
            this.txtMemoFormTerNombre.Tag = this.txtMemoFormTerNombre.Text;
            this.txtMemoFormTerDir.Tag = this.txtMemoFormTerDir.Text;
            this.txtMemoFormTerPoblacion.Tag = this.txtMemoFormTerPoblacion.Text;
            this.txtMemoFormTerCodCta1.Tag = this.txtMemoFormTerCodCta1.Text;
            this.txtMemoFormTerCodCta2.Tag = this.txtMemoFormTerCodCta2.Text;
            this.txtMemoFormTerCodCta3.Tag = this.txtMemoFormTerCodCta3.Text;
            this.txtMemoFormTerCodCta4.Tag = this.txtMemoFormTerCodCta4.Text;
            this.tgIBANMemoFormTer.Tag = this.tgIBANMemoFormTer.IBANCodigo;
            this.txtMemoFormTerSWIFT.Tag = this.txtMemoFormTerSWIFT.Text;
            this.txtMemoFormTerMandatoUnico.Tag = this.txtMemoFormTerMandatoUnico.Text;
            this.radButtonTextBoxReservadoUser.Tag = this.codigoReservadoUser;

            string anoPrimVto = this.cmbMemoFormTerFormaPagoAnoPrimVto.Text.Trim();
            if (anoPrimVto == "") anoPrimVto = "0";
            this.cmbMemoFormTerFormaPagoAnoPrimVto.Tag = anoPrimVto;

            string numPlazosPrimVto = this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Text.Trim();
            if (numPlazosPrimVto == "") numPlazosPrimVto = "0";
            this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Tag = numPlazosPrimVto;

            string tipoPlazo = "1";
            if (this.rbMemoFormTerFormaPagoTipoPlazo15.IsChecked) tipoPlazo = "2";
            this.gbMemoFormTerFormaPagoTipoPlazo.Tag = tipoPlazo;

            string numPlazos = this.cmbMemoFormTerFormaPagoNumPlazos.Text.Trim();
            if (numPlazos == "") numPlazos = "01";
            this.cmbMemoFormTerFormaPagoNumPlazos.Tag = numPlazos.PadLeft(2, '0');

            string diasFijosPago1 = this.cmbMemoFormTerFormaPagoDiasFijos1.Text.Trim();
            if (diasFijosPago1 == "") diasFijosPago1 = "00";
            this.cmbMemoFormTerFormaPagoDiasFijos1.Tag = diasFijosPago1.PadLeft(2, '0');

            string diasFijosPago2 = this.cmbMemoFormTerFormaPagoDiasFijos2.Text.Trim();
            if (diasFijosPago2 == "") diasFijosPago2 = "00";
            this.cmbMemoFormTerFormaPagoDiasFijos2.Tag = diasFijosPago2.PadLeft(2, '0');

            string diasFijosPago3 = this.cmbMemoFormTerFormaPagoDiasFijos3.Text.Trim();
            if (diasFijosPago3 == "") diasFijosPago3 = "00";
            this.cmbMemoFormTerFormaPagoDiasFijos3.Tag = diasFijosPago3.PadLeft(2, '0');

            string tipoDiaFijjo = "0";
            if (this.rbMemoFormTerFormaPagoTipoDiaFijoSemana.IsChecked) tipoDiaFijjo = "1";
            this.gbMemoFormTerFormaPagoTipoDiaFijo.Tag = tipoDiaFijjo;

            string limiteSup = this.txtMemoFormTerLimiteCred2.Text.Trim();
            if (limiteSup.Length < 13) limiteSup = limiteSup.PadRight(13, ' ');
            this.txtMemoFormTerLimiteCred2.Tag = limiteSup;

            string limiteInf = this.txtMemoFormTerLimiteCred1.Text.Trim();
            if (limiteInf.Length < 13) limiteInf = limiteInf.PadRight(13, ' ');
            this.txtMemoFormTerLimiteCred1.Tag = limiteInf;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles en la pestaña de MemoBancos
        /// </summary>
        private void ActualizaValoresOrigenControlesMemoBancos()
        {
            this.txtMemoFormBancosMemo.Tag = this.txtMemoFormBancosMemo.Text;
            this.tgIBANMemoFormBancos.Tag = this.tgIBANMemoFormBancos.IBANCodigo;
            this.txtMemoFormBancosSWIFT.Tag = this.txtMemoFormBancosSWIFT.Text;
            this.txtMemoFormBancosCtaAbono1.Tag = this.txtMemoFormBancosCtaAbono1.Text;
            this.txtMemoFormBancosCtaAbono2.Tag = this.txtMemoFormBancosCtaAbono2.Text;
            this.txtMemoFormBancosCtaAbono3.Tag = this.txtMemoFormBancosCtaAbono3.Text;
            this.txtMemoFormBancosCtaAbono4.Tag = this.txtMemoFormBancosCtaAbono4.Text;
            this.txtMemoFormBancosCtaAdeudo1.Tag = this.txtMemoFormBancosCtaAdeudo1.Text;
            this.txtMemoFormBancosCtaAdeudo2.Tag = this.txtMemoFormBancosCtaAdeudo2.Text;
            this.txtMemoFormBancosCtaAdeudo3.Tag = this.txtMemoFormBancosCtaAdeudo3.Text;
            this.txtMemoFormBancosCtaAdeudo4.Tag = this.txtMemoFormBancosCtaAdeudo4.Text;
            this.txtMemoFormBancosCtaImpagados1.Tag = this.txtMemoFormBancosCtaImpagados1.Text;
            this.txtMemoFormBancosCtaImpagados2.Tag = this.txtMemoFormBancosCtaImpagados2.Text;
            this.txtMemoFormBancosCtaImpagados3.Tag = this.txtMemoFormBancosCtaImpagados3.Text;
            this.txtMemoFormBancosCtaImpagados4.Tag = this.txtMemoFormBancosCtaImpagados4.Text;
            string limiteDto = this.txtMemoFormBancosLimiteDto.Text.Trim();
            if (limiteDto.Length < 13) limiteDto = limiteDto.PadRight(13, ' ');
            this.txtMemoFormBancosLimiteDto.Tag = limiteDto;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCtaAux.Tag = this.txtCtaAux.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombreCuentaAux.Tag = this.txtNombreCuentaAux.Text;
            this.radButtonTextBoxNumIdTributariaPais.Tag = this.radButtonTextBoxNumIdTributariaPais.Text;
            this.txtNumIdTributaria.Tag = this.txtNumIdTributaria.Text;
            this.txtNumTelefono.Tag = this.txtNumTelefono.Text;
            this.txtEmail.Tag = this.txtEmail.Text;
            this.txtCP.Tag = this.txtCP.Text;
            this.txtApdoPostal.Tag = this.txtApdoPostal.Text;
            this.txtPrimDigVal.Tag = this.txtPrimDigVal.Text;
            this.radButtonTextBoxGrupoCtas.Tag = this.codigoGrupoCuentas;

            if (this.gbZona.Visible)
            {
                if (this.zonaJerarq)
                {
                    this.txtZonaNivel1.Tag = this.txtZonaNivel1.Text;
                    this.txtZonaNivel2.Tag = this.txtZonaNivel2.Text;
                    this.txtZonaNivel3.Tag = this.txtZonaNivel3.Text;
                    this.txtZonaNivel4.Tag = this.txtZonaNivel4.Text;
                }
                else
                {
                    this.radButtonTextBoxZonaNivel1.Tag = this.valorZonaNivel1;
                    this.radButtonTextBoxZonaNivel2.Tag = this.valorZonaNivel2;
                    this.radButtonTextBoxZonaNivel3.Tag = this.valorZonaNivel3;
                    this.radButtonTextBoxZonaNivel4.Tag = this.valorZonaNivel4;
                }
            }

            //Dirección
            switch (this.tipoDir)
            {
                case "F":
                    this.ActualizaValoresOrigenControlesDireccionFormateada();
                    break;
                case "S":
                case "N":
                    this.ActualizaValoresOrigenControlesDireccionRequeridaSN();
                    break;
            }

            //Memo
            switch (tipoMemo)
            {
                case "0":
                    this.ActualizaValoresOrigenControlesMemoSinFormato();
                    break;
                case "1":
                    this.ActualizaValoresOrigenControlesMemoTerceros();
                    break;
                case "2":
                    this.ActualizaValoresOrigenControlesMemoBancos();
                    break;
            }
        }

        /// <summary>
        /// Verifica si se han modificado las zonas (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool ZonasCambio()
        {
            bool result = false;

            try
            {
                if (!this.gbZona.Visible) return (result);

                if (this.zonaJerarq)
                {
                    //Zona Jerárquica
                    if (this.txtZonaNivel1.Text.Trim() != this.txtZonaNivel1.Tag.ToString().Trim() ||
                        (this.longZonaNivel2 != 0 && this.txtZonaNivel2.Text.Trim() != this.txtZonaNivel2.Tag.ToString().Trim()) ||
                        (this.longZonaNivel3 != 0 && this.txtZonaNivel3.Text.Trim() != this.txtZonaNivel3.Tag.ToString().Trim()) ||
                        (this.longZonaNivel4 != 0 && this.txtZonaNivel4.Text.Trim() != this.txtZonaNivel4.Tag.ToString().Trim())
                        )
                    {
                        result = true;
                    }
                }
                else
                {
                    //Zona NO Jerárquica
                    if (this.valorZonaNivel1.Trim() != this.radButtonTextBoxZonaNivel1.Tag.ToString() ||
                        (this.longZonaNivel2 != 0 && this.valorZonaNivel2.Trim() != this.radButtonTextBoxZonaNivel2.Tag.ToString()) ||
                        (this.longZonaNivel3 != 0 && this.valorZonaNivel3.Trim() != this.radButtonTextBoxZonaNivel3.Tag.ToString()) ||
                        (this.longZonaNivel4 != 0 && this.valorZonaNivel4.Trim() != this.radButtonTextBoxZonaNivel4.Tag.ToString())
                        )
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return(result);
        }

        /// <summary>
        /// Verifica si se ha modificado la dirección (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool DireccionCambio()
        {
            bool result = false;
            
            try
            {
                switch (this.tipoDir)
                {
                    case "F":
                        //Direccion Formateada
                        if (this.txtDirForSiglas.Text.Trim() != this.txtDirForSiglas.Tag.ToString().Trim() ||
                            this.txtDirForNombreVia.Text.Trim() != this.txtDirForNombreVia.Tag.ToString().Trim() ||
                            this.txtDirForNumCasa.Text.Trim() != this.txtDirForNumCasa.Tag.ToString().Trim() ||
                            this.txtDirForCP.Text.Trim() != this.txtDirForCP.Tag.ToString().Trim() ||
                            this.txtDirForMunicipio.Text.Trim() != this.txtDirForMunicipio.Tag.ToString().Trim() ||
                            this.txtDirForLinea3.Text.Trim() != this.txtDirForLinea3.Tag.ToString().Trim() ||
                            this.txtDirForLinea4.Text.Trim() != this.txtDirForLinea4.Tag.ToString().Trim() ||
                            this.txtDirForLinea5.Text.Trim() != this.txtDirForLinea5.Tag.ToString().Trim()
                            )
                        {
                            result = true;
                        }
                        break;
                    case "S":
                    case "N":
                        //Direccion Requerida SN
                        if (this.txtDirReqLinea1.Text.Trim() != this.txtDirReqLinea1.Tag.ToString().Trim() ||
                            this.txtDirReqLinea2.Text.Trim() != this.txtDirReqLinea2.Tag.ToString().Trim() ||
                            this.txtDirReqLinea3.Text.Trim() != this.txtDirReqLinea3.Tag.ToString().Trim() ||
                            this.txtDirReqLinea4.Text.Trim() != this.txtDirReqLinea4.Tag.ToString().Trim() ||
                            this.txtDirReqLinea5.Text.Trim() != this.txtDirReqLinea5.Tag.ToString().Trim()
                            )
                        {
                            result = true;
                        }
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return(result);
        }

        /// <summary>
        /// Verifica si se han modificado el memo (validación antes de cerrar el formulario para avisar de posibles pérdidas de info si no se graba)
        /// </summary>
        /// <returns></returns>
        private bool MemoCambio()
        {
            bool result = false;

            try
            {
                switch (tipoMemo)
                {
                    case "0":
                        //Memo Sin Formato
                        if (this.txtMemoFormNoLinea1.Text.Trim() != this.txtMemoFormNoLinea1.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea2.Text.Trim() != this.txtMemoFormNoLinea2.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea3.Text.Trim() != this.txtMemoFormNoLinea3.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea4.Text.Trim() != this.txtMemoFormNoLinea4.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea5.Text.Trim() != this.txtMemoFormNoLinea5.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea6.Text.Trim() != this.txtMemoFormNoLinea6.Tag.ToString().Trim() ||
                            this.txtMemoFormNoLinea7.Text.Trim() != this.txtMemoFormNoLinea7.Tag.ToString().Trim()
                            )
                        {
                            result = true;
                        }
                        break;
                    case "1":
                        //Memo Terceros
                        string tipoPlazo = "1";
                        if (this.rbMemoFormTerFormaPagoTipoPlazo15.IsChecked) tipoPlazo = "2";

                        string anoPrimVto = this.cmbMemoFormTerFormaPagoAnoPrimVto.Text.Trim();
                        if (anoPrimVto == "") anoPrimVto = "0";
            
                        string numPlazosPrimVto = this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Text.Trim();
                        if (numPlazosPrimVto == "") numPlazosPrimVto = "0";
            
                        string numPlazos = this.cmbMemoFormTerFormaPagoNumPlazos.Text.Trim();
                        if (numPlazos == "") numPlazos = "01";
                        else numPlazos = numPlazos.PadLeft(2, '0');

                        string diasFijosPago1 = this.cmbMemoFormTerFormaPagoDiasFijos1.Text.Trim();
                        if (diasFijosPago1 == "") diasFijosPago1 = "00";
                        else diasFijosPago1 = diasFijosPago1.PadLeft(2, '0');

                        string diasFijosPago2 = this.cmbMemoFormTerFormaPagoDiasFijos2.Text.Trim();
                        if (diasFijosPago2 == "") diasFijosPago2 = "00";
                        else diasFijosPago2 = diasFijosPago2.PadLeft(2, '0');

                        string diasFijosPago3 = this.cmbMemoFormTerFormaPagoDiasFijos3.Text.Trim();
                        if (diasFijosPago3 == "") diasFijosPago3 = "00";
                        else diasFijosPago3 = diasFijosPago3.PadLeft(2, '0');

                        string tipoDiaFijjo = "0";
                        if (this.rbMemoFormTerFormaPagoTipoDiaFijoSemana.IsChecked) tipoDiaFijjo = "1";

                        if (this.txtMemoFormTerMemo1.Text.Trim() != this.txtMemoFormTerMemo1.Tag.ToString().Trim() ||
                            this.txtMemoFormTerMemo2.Text.Trim() != this.txtMemoFormTerMemo2.Tag.ToString().Trim() ||
                            this.txtMemoFormTerMemo3.Text.Trim() != this.txtMemoFormTerMemo3.Tag.ToString().Trim() ||
                            this.txtMemoFormTerNombre.Text.Trim() != this.txtMemoFormTerNombre.Tag.ToString().Trim() ||
                            this.txtMemoFormTerDir.Text.Trim() != this.txtMemoFormTerDir.Tag.ToString().Trim() ||
                            this.txtMemoFormTerPoblacion.Text.Trim() != this.txtMemoFormTerPoblacion.Tag.ToString().Trim() ||
                            this.txtMemoFormTerCodCta1.Text.Trim() != this.txtMemoFormTerCodCta1.Tag.ToString().Trim() ||
                            this.txtMemoFormTerCodCta2.Text.Trim() != this.txtMemoFormTerCodCta2.Tag.ToString().Trim() ||
                            this.txtMemoFormTerCodCta3.Text.Trim() != this.txtMemoFormTerCodCta3.Tag.ToString().Trim() ||
                            this.txtMemoFormTerCodCta4.Text.Trim() != this.txtMemoFormTerCodCta4.Tag.ToString().Trim() ||
                            this.tgIBANMemoFormTer.IBANCodigo.Trim() != this.tgIBANMemoFormTer.Tag.ToString().Trim() ||
                            this.txtMemoFormTerSWIFT.Text.Trim() != this.txtMemoFormTerSWIFT.Tag.ToString().Trim() ||
                            this.txtMemoFormTerMandatoUnico.Text.Trim() != this.txtMemoFormTerMandatoUnico.Tag.ToString().Trim() ||
                            (this.codigoReservadoUser != null && this.codigoReservadoUser.Trim() != this.radButtonTextBoxReservadoUser.Tag.ToString()) ||
                            anoPrimVto != this.cmbMemoFormTerFormaPagoAnoPrimVto.Tag.ToString() ||
                            numPlazosPrimVto != this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Tag.ToString() ||
                            tipoPlazo != this.gbMemoFormTerFormaPagoTipoPlazo.Tag.ToString() ||
                            numPlazos != this.cmbMemoFormTerFormaPagoNumPlazos.Tag.ToString().Trim() ||
                            diasFijosPago1 != this.cmbMemoFormTerFormaPagoDiasFijos1.Tag.ToString().Trim() ||
                            diasFijosPago2 != this.cmbMemoFormTerFormaPagoDiasFijos2.Tag.ToString().Trim() ||
                            diasFijosPago3 != this.cmbMemoFormTerFormaPagoDiasFijos3.Tag.ToString().Trim() ||
                            tipoDiaFijjo != this.gbMemoFormTerFormaPagoTipoDiaFijo.Tag.ToString() ||
                            this.txtMemoFormTerLimiteCred2.Text.Trim() != this.txtMemoFormTerLimiteCred2.Tag.ToString().Trim() ||
                            this.txtMemoFormTerLimiteCred1.Text.Trim() != this.txtMemoFormTerLimiteCred1.Tag.ToString().Trim()
                            )
                        {
                            result = true;
                        }

                        break;
                    case "2":
                        //Memo Bancos
                        if (this.txtMemoFormBancosMemo.Text.Trim() != this.txtMemoFormBancosMemo.Tag.ToString().Trim() ||
                            this.tgIBANMemoFormBancos.IBANCodigo.Trim() != this.tgIBANMemoFormBancos.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosSWIFT.Text.Trim() != this.txtMemoFormBancosSWIFT.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAbono1.Text.Trim() != this.txtMemoFormBancosCtaAbono1.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAbono2.Text.Trim() != this.txtMemoFormBancosCtaAbono2.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAbono3.Text.Trim() != this.txtMemoFormBancosCtaAbono3.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAbono4.Text.Trim() != this.txtMemoFormBancosCtaAbono4.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAdeudo1.Text.Trim() != this.txtMemoFormBancosCtaAdeudo1.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAdeudo2.Text.Trim() != this.txtMemoFormBancosCtaAdeudo2.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAdeudo3.Text.Trim() != this.txtMemoFormBancosCtaAdeudo3.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaAdeudo4.Text.Trim() != this.txtMemoFormBancosCtaAdeudo4.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaImpagados1.Text.Trim() != this.txtMemoFormBancosCtaImpagados1.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaImpagados2.Text.Trim() != this.txtMemoFormBancosCtaImpagados2.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaImpagados3.Text.Trim() != this.txtMemoFormBancosCtaImpagados3.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosCtaImpagados4.Text.Trim() != this.txtMemoFormBancosCtaImpagados4.Tag.ToString().Trim() ||
                            this.txtMemoFormBancosLimiteDto.Text.Trim() != this.txtMemoFormBancosLimiteDto.Tag.ToString().Trim()
                            )
                        {
                            result = true;
                        }

                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return(result);
        }

        /// <summary>
        /// Tipo de Auxiliar Externo, desactivar campos del formulario que lo requieran
        /// </summary>
        private void TipoAuxExternoActualizarCamposForm()
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLC07 where ";
                query += "TAUXCA = '" + this.codigoTipoAux + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                string campo = "";
                while (dr.Read())
                {
                    campo = dr.GetValue(dr.GetOrdinal("AXNMCA")).ToString().TrimEnd();

                    switch (campo)
                    {
                        case "NOMBMA":
                            this.txtNombreCuentaAux.Enabled = false;
                            break;
                        case "PCIFMA":
                            this.radButtonTextBoxNumIdTributariaPais.Enabled = false;
                            break;
                        case "NNITMA":
                            this.txtNumIdTributaria.Enabled = false;
                            break;
                        case "FILLGB":
                            this.txtEmail.Enabled = false;
                            break;
                        case "POSTMA":
                            this.txtCP.Enabled = false;
                            break;
                        case "APARMA":
                            this.txtApdoPostal.Enabled = false;
                            break;
                        case "GRUPMA":
                            this.txtPrimDigVal.Enabled = false;
                            break;
                        case "GRCTMA":
                            this.radButtonTextBoxGrupoCtas.Enabled = false;
                            break;
                        case "ZONAMA":
                            this.gbZona.Enabled = false;
                            break;
                        case "DAUXAD":
                            this.radPageViewPageDirReqSN.Enabled = false;
                            this.radPageViewPageDirFormateada.Enabled = false;
                            break;
                        case "MEAXAD":
                            this.radPageViewPageMemoFormNo.Enabled = false;
                            this.radPageViewPageMemoFormTerceros.Enabled = false;
                            this.radPageViewPageMemoFormBancos.Enabled = false;
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
        }

        /// <summary>
        /// Graba una cuenta de auxiliar
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtNombreCuentaAux.Text.Trim();

                        bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                        else utiles.ButtonEnabled(ref this.radButtonEliminar, false);
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
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar una cuenta de auxiliar
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la cuenta de auxiliar " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si la cuenta de auxiliar tiene saldos
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "PRH01 ";
                    query += "where TAUXH1 = '" + this.codigoTipoAux + "' and CAUXH1 = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en saldos
                        mensaje = "No es posible eliminar la Cuenta de Auxiliar porque existen saldos asociados.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        return;
                    }

                    //Buscar si la cuenta de auxiliar tiene comprobantes
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                    query += "where (TAUXDT = '" + this.codigoTipoAux + "' and CAUXDT = '" + this.codigo + "') or ";
                    query += "(TAAD01 = '" + this.codigoTipoAux + "' and AUAD01 = '" + this.codigo + "') or ";
                    query += "(TAAD02 = '" + this.codigoTipoAux + "' and AUAD02 = '" + this.codigo + "')";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en los asientos contables
                        mensaje = "No es posible eliminar la Cuenta de Auxiliar porque existen asientos asociados.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar la cuenta de auxiliar (GLM05 + GLDM5 + GLMM5)
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLDM5 ";
                        query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLDM5", this.codigoTipoAux, this.codigo);

                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLMM5 ";
                        query += "where TAUXAD = '" + this.codigoTipoAux + "' and CAUXAD = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLMM5", this.codigoTipoAux, this.codigo);

                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                        query += "where TAUXMA = '" + this.codigoTipoAux + "' and CAUXMA = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLM05", this.codigoTipoAux, this.codigo);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar la Cuenta de Auxiliar.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Cerrar el formulario
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Llamar al formulario que permite crear grupo de cuentas
        /// </summary>
        private void CrearGrupoCtas()
        {
            frmMtoGLT08 frmEditGLT08 = new frmMtoGLT08();
            frmEditGLT08.Nuevo = true;
            frmEditGLT08.EditarTipoAux = false;
            frmEditGLT08.GrabarClose = true;
            frmEditGLT08.Codigo = "";
            frmEditGLT08.CodigoTipoAux = this.codigoTipoAux;
            frmEditGLT08.FrmPadre = this;
            frmEditGLT08.Show(this);

            if (frmEditGLT08.CodigoDesGrupoAux != "") this.radButtonTextBoxGrupoCtas.Text = frmEditGLT08.CodigoDesGrupoAux;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCtaAux.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = "V";
            this.txtNombreCuentaAux.Tag = "";
            this.radButtonTextBoxNumIdTributariaPais.Tag = null;
            this.txtNumIdTributaria.Tag = "";
            this.txtNumTelefono.Tag = "";
            this.txtEmail.Tag = "";
            this.txtCP.Tag = "";
            this.txtApdoPostal.Tag = "";
            this.txtPrimDigVal.Tag = "";
            this.radButtonTextBoxGrupoCtas.Tag = "";

            if (this.gbZona.Visible)
            {
                if (this.zonaJerarq)
                {
                    this.txtZonaNivel1.Tag = "";
                    this.txtZonaNivel2.Tag = "";
                    this.txtZonaNivel3.Tag = "";
                    this.txtZonaNivel4.Tag = "";
                }
                else
                {
                    this.radButtonTextBoxZonaNivel1.Tag = "";
                    this.radButtonTextBoxZonaNivel2.Tag = "";
                    this.radButtonTextBoxZonaNivel3.Tag = "";
                    this.radButtonTextBoxZonaNivel4.Tag = "";
                }
            }

            //Dirección
            switch (this.tipoDir)
            {
                case "F":
                    this.ActualizaValoresOrigenTAGControlesDireccionFormateada();
                    break;
                case "S":
                case "N":
                    this.ActualizaValoresOrigenTAGControlesDireccionRequeridaSN();
                    break;
            }

            //Memo
            switch (tipoMemo)
            {
                case "0":
                    this.ActualizaValoresOrigenTAGControlesMemoSinFormato();
                    break;
                case "1":
                    this.ActualizaValoresOrigenTAGControlesMemoTerceros();
                    break;
                case "2":
                    this.ActualizaValoresOrigenTAGControlesMemoBancos();
                    break;
            }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) en la pestaña de Direccion Formateada
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesDireccionFormateada()
        {
            this.txtDirForSiglas.Tag = "";
            this.txtDirForNombreVia.Tag = "";
            this.txtDirForNumCasa.Tag = "";
            this.txtDirForCP.Tag = "";
            this.txtDirForMunicipio.Tag = "";
            this.txtDirForLinea3.Tag = "";
            this.txtDirForLinea4.Tag = "";
            this.txtDirForLinea5.Tag = "";
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento) en la pestaña de Direccion RequeridaSN
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesDireccionRequeridaSN()
        {
            this.txtDirReqLinea1.Tag = "";
            this.txtDirReqLinea2.Tag = "";
            this.txtDirReqLinea3.Tag = "";
            this.txtDirReqLinea4.Tag = "";
            this.txtDirReqLinea5.Tag = "";
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor (opción nuevo elemento) en la pestaña de MemoSinFormato
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesMemoSinFormato()
        {
            this.txtMemoFormNoLinea1.Tag = "";
            this.txtMemoFormNoLinea2.Tag = "";
            this.txtMemoFormNoLinea3.Tag = "";
            this.txtMemoFormNoLinea4.Tag = "";
            this.txtMemoFormNoLinea5.Tag = "";
            this.txtMemoFormNoLinea6.Tag = "";
            this.txtMemoFormNoLinea7.Tag = "";
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor (opción nuevo elemento) en la pestaña de MemoTerceros
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesMemoTerceros()
        {
            this.txtMemoFormTerMemo1.Tag = "";
            this.txtMemoFormTerMemo2.Tag = "";
            this.txtMemoFormTerMemo3.Tag = "";
            this.txtMemoFormTerNombre.Tag = "";
            this.txtMemoFormTerDir.Tag = "";
            this.txtMemoFormTerPoblacion.Tag = "";
            this.txtMemoFormTerCodCta1.Tag = "";
            this.txtMemoFormTerCodCta2.Tag = "";
            this.txtMemoFormTerCodCta3.Tag = "";
            this.txtMemoFormTerCodCta4.Tag = "";
            this.tgIBANMemoFormTer.Tag = "";
            this.txtMemoFormTerSWIFT.Tag = "";
            this.txtMemoFormTerMandatoUnico.Tag = "";
            this.radButtonTextBoxReservadoUser.Tag = "";
            this.cmbMemoFormTerFormaPagoAnoPrimVto.Tag = "0";
            this.cmbMemoFormTerFormaPagoNumPlazosPrimVto.Tag = "0";
            this.gbMemoFormTerFormaPagoTipoPlazo.Tag = "1";
            this.cmbMemoFormTerFormaPagoNumPlazos.Tag = "01";
            this.cmbMemoFormTerFormaPagoDiasFijos1.Tag = "00";
            this.cmbMemoFormTerFormaPagoDiasFijos2.Tag = "00";
            this.cmbMemoFormTerFormaPagoDiasFijos3.Tag = "00";
            this.gbMemoFormTerFormaPagoTipoDiaFijo.Tag = "0";
            this.txtMemoFormTerLimiteCred2.Tag = "";
            this.txtMemoFormTerLimiteCred1.Tag = "";
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor (opción nuevo elemento) en la pestaña de MemoBancos
        /// </summary>
        private void ActualizaValoresOrigenTAGControlesMemoBancos()
        {
            this.txtMemoFormBancosMemo.Tag = "";
            this.tgIBANMemoFormBancos.Tag = "";
            this.txtMemoFormBancosSWIFT.Tag = "";
            this.txtMemoFormBancosCtaAbono1.Tag = "";
            this.txtMemoFormBancosCtaAbono2.Tag = "";
            this.txtMemoFormBancosCtaAbono3.Tag = "";
            this.txtMemoFormBancosCtaAbono4.Tag = "";
            this.txtMemoFormBancosCtaAdeudo1.Tag = "";
            this.txtMemoFormBancosCtaAdeudo2.Tag = "";
            this.txtMemoFormBancosCtaAdeudo3.Tag = "";
            this.txtMemoFormBancosCtaAdeudo4.Tag = "";
            this.txtMemoFormBancosCtaImpagados1.Tag = "";
            this.txtMemoFormBancosCtaImpagados2.Tag = "";
            this.txtMemoFormBancosCtaImpagados3.Tag = "";
            this.txtMemoFormBancosCtaImpagados4.Tag = "";

            this.txtMemoFormBancosLimiteDto.Tag = "";
        }
        #endregion

        /// <summary>
        /// Clase que construye un objeto con los valores vacíos para la tabla GLMM5
        /// </summary>
        public class GLMM5ValoresVacios
        {
            public string meaxad = " ";
            public string mem1gb = " ";
            public string mem2gb = " ";
            public string mem3gb = " ";
            public string abo1gb = "0";
            public string abo2gb = "0";
            public string abo3gb = "0";
            public string abo4gb = "0";
            public string gas1gb = "0";
            public string gas2gb = "0";
            public string gas3gb = "0";
            public string gas4gb = "0";
            public string imp1gb = "0";
            public string imp2gb = "0";
            public string imp3gb = "0";
            public string imp4gb = "0";
            public string limdgb = "0";
            public string comdgb = "0";
            public string cordgb = "0";
            public string corsgb = "0";
            public string diaigb = "0";
            public string diamgb = "0";
            public string diapgb = "0";
            public string for1gb = "0";
            public string for2gb = "0";
            public string for3gb = "0";
            public string for4gb = "0";
            public string for5gb = "0";
            public string for6gb = "0";
            public string fod1gb = "0";
            public string fod2gb = "0";
            public string fod3gb = "0";
            public string fod4gb = "0";
            public string fod5gb = "0";
            public string fod6gb = "0";
            public string fom1gb = "0";
            public string fom2gb = "0";
            public string fom3gb = "0";
            public string fom4gb = "0";
            public string fom5gb = "0";
            public string fom6gb = "0";
            public string fillgb = " ";

            public GLMM5ValoresVacios()
            {
            }
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonCrearGrupoCuentas_Click(object sender, EventArgs e)
        {
            this.CrearGrupoCtas();
        }

        private void radButtonEliminar_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs();
            args.Codigo = "";
            args.Operacion = OperacionMtoTipo.Eliminar;
            DoUpdateDataForm(args);
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs();
            args.Codigo = this.codigo;
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void radButtonElementGrupoCtas_Click(object sender, EventArgs e)
        {
            string query = "select GRCTGA, NOMBGA from ";
            query += GlobalVar.PrefijoTablaCG + "GLT08 ";
            query += "where TAUXGA = '" + this.codigoTipoAux + "' ";
            query += "order by GRCTGA";

            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));

            string result = this.BuscarElementos("Seleccionar grupos de cuentas", query, nombreColumnas, 2);
            if (result != "")
            { 
                this.radButtonTextBoxGrupoCtas.Text = result;
                this.ActiveControl = this.radButtonTextBoxGrupoCtas;
                this.radButtonTextBoxGrupoCtas.Select(0, 0);
                this.radButtonTextBoxGrupoCtas.Focus();
            }
        }

        private void radButtonElementNumIdTributariaPais_Click(object sender, EventArgs e)
        {
            string prefijoTabla = "";
            if (this.proveedorTipo == "DB2")
            {
                prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
            }
            else prefijoTabla = GlobalVar.PrefijoTablaCG;

            string query = "select PCIF30, NOMB30 from ";
            query += prefijoTabla + "GLT30 ";
            query += "order by NOMB30";

            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));

            string result = this.BuscarElementos("Seleccionar país", query, nombreColumnas, 1); 
            if (result != "")
            {
                this.radButtonTextBoxNumIdTributariaPais.Text = result;
                this.ActiveControl = this.radButtonTextBoxNumIdTributariaPais;
                this.radButtonTextBoxNumIdTributariaPais.Select(0, 0);
                this.radButtonTextBoxNumIdTributariaPais.Focus();
            }
        }
        
        private string BuscarElementos(string titulo, string query, ArrayList nombreColumnas, int cantidadColumnasResult)
        {
            string result = "";

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel();

            //Título del Formulario de Selección de Elementos
            frmElementosSel.TituloForm = titulo;
            //Coordenadas donde se dibujará el Formulario de Selección de Elementos
            //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
            frmElementosSel.LocationForm = new Point(0, 0);
            //Si se centrar el Formulario o no
            frmElementosSel.CentrarForm = true;
            //Pasar la conexión a la bbdd
            frmElementosSel.ProveedorDatosForm = GlobalVar.ConexionCG;
            //Consulta que se ejecutará para obtener los Elementos
            frmElementosSel.Query = query;

            frmElementosSel.ColumnasCaption = nombreColumnas;
            //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
            frmElementosSel.FrmPadre = this;

            frmElementosSel.ShowDialog();

            string separadorCampos = "-";
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
            }

            return (result);
        }

        private void radButtonElementZonaNivel1_Click(object sender, EventArgs e)
        {
            CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel1, this.claseZonaNivel1, this.longZonaNivel1);
        }

        private void radButtonElementZonaNivel2_Click(object sender, EventArgs e)
        {
            CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel2, this.claseZonaNivel2, this.longZonaNivel2);
        }

        private void radButtonElementZonaNivel3_Click(object sender, EventArgs e)
        {
            CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel3, this.claseZonaNivel3, this.longZonaNivel3);
        }

        private void radButtonElementZonaNivel4_Click(object sender, EventArgs e)
        {
            CrearControlSeleccionZona(ref this.radButtonTextBoxZonaNivel4, this.claseZonaNivel4, this.longZonaNivel4);
        }

        private void radButtonElementReservadoUser_Click(object sender, EventArgs e)
        {
            this.CrearControlSeleccionReservadoUsuario();
        }

        private void radPageViewDatos_SelectedPageChanged(object sender, EventArgs e)
        {
            //Activar el primer control de la pestaña seleccionada
            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageGeneral.Name])
            {
                this.txtNombreCuentaAux.Select();
                return;
            }

            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageDirReqSN.Name])
            {
                this.txtDirReqLinea1.Select();
                return;
            }

            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageDirFormateada.Name])
            {
                this.txtDirForSiglas.Select();
                return;
            }

            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageMemoFormNo.Name])
            {
                this.txtMemoFormNoLinea1.Select();
                return;
            }

            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageMemoFormTerceros.Name])
            {
                this.txtMemoFormTerMemo1.Select();
                return;
            }

            if (this.radPageViewDatos.SelectedPage == this.radPageViewDatos.Pages[this.radPageViewPageMemoFormBancos.Name])
            {
                this.txtMemoFormBancosMemo.Select();
                return;
            }
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void radButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
        }

        private void radButtonCrearGrupoCuentas_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCrearGrupoCuentas);
        }

        private void radButtonCrearGrupoCuentas_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCrearGrupoCuentas);
        }

        private void radButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void radButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void txtMemoFormBancosCtaAbono4_Leave(object sender, EventArgs e)
        {
            tgIBANMemoFormBancos.IBANUnCampo = false;
            if (tgIBANMemoFormBancos.IsEmpty())
            {
                string pais = radButtonTextBoxNumIdTributariaPais.Text;
                if (pais.Trim() == "") pais = "ES";
                string sIBAN = utiles.calcularIban(pais, txtMemoFormBancosCtaAbono1.Text + txtMemoFormBancosCtaAbono2.Text +
                           txtMemoFormBancosCtaAbono3.Text + txtMemoFormBancosCtaAbono4.Text);

                tgIBANMemoFormBancos.CargarValor(sIBAN);
            }
        }

        private void txtMemoFormTerCodCta4_Leave(object sender, EventArgs e)
        {
            tgIBANMemoFormTer.IBANUnCampo = false;
            if (tgIBANMemoFormTer.IsEmpty())
            {
                string pais = radButtonTextBoxNumIdTributariaPais.Text;
                if (pais.Trim() == "") pais = "ES";
                string sIBAN = utiles.calcularIban(pais, txtMemoFormTerCodCta1.Text + txtMemoFormTerCodCta2.Text +
                           txtMemoFormTerCodCta3.Text + txtMemoFormTerCodCta4.Text);

                tgIBANMemoFormTer.CargarValor(sIBAN);
            }
        }

        private void rbMemoFormTerFormaPagoTipoDiaFijoMes_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (this.rbMemoFormTerFormaPagoTipoDiaFijoMes.IsChecked == true)
            {
                this.cmbMemoFormTerFormaPagoDiasFijos1.Items.Clear();
                this.cmbMemoFormTerFormaPagoDiasFijos2.Items.Clear();
                this.cmbMemoFormTerFormaPagoDiasFijos3.Items.Clear();
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valoresMes);
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valoresMes);
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valoresMes);
                this.cmbMemoFormTerFormaPagoDiasFijos1.SelectedIndex = 0;
                this.cmbMemoFormTerFormaPagoDiasFijos2.SelectedIndex = 0;
                this.cmbMemoFormTerFormaPagoDiasFijos3.SelectedIndex = 0;
            }
        }

        private void rbMemoFormTerFormaPagoTipoDiaFijoSemana_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (this.rbMemoFormTerFormaPagoTipoDiaFijoSemana.IsChecked == true)
            {
                this.cmbMemoFormTerFormaPagoDiasFijos1.Items.Clear();
                this.cmbMemoFormTerFormaPagoDiasFijos2.Items.Clear();
                this.cmbMemoFormTerFormaPagoDiasFijos3.Items.Clear();
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos1, ref valoresSemana);
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos2, ref valoresSemana);
                utiles.CreateRadDropDownListElement(ref this.cmbMemoFormTerFormaPagoDiasFijos3, ref valoresSemana);
                this.cmbMemoFormTerFormaPagoDiasFijos1.SelectedIndex = 0;
                this.cmbMemoFormTerFormaPagoDiasFijos2.SelectedIndex = 0;
                this.cmbMemoFormTerFormaPagoDiasFijos3.SelectedIndex = 0;
            }

        }

        private void frmMtoGLM05_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}