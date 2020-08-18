using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM02 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOPLANCT";

        //Size peq : 962; 420
        //Size grande : 962; 644
        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "003";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "30";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        //private bool visibleCamposExt = false;
        private bool grabarInfoCamposExt = true;
        private bool existeInfoCamposExt = false;

        private string mascaraInicial = "";

        ArrayList nombreColumnas = new ArrayList();

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

        public frmMtoGLM02()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchDigitoAutoverif.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchDigitoAutoverif.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchActivoPrefDoc.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchActivoPrefDoc.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchActivoNumFactAmp.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchActivoNumFactAmp.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchActivoNumFactRectif.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchActivoNumFactRectif.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchFechaServicio.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchFechaServicio.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchAlfa1.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa1.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa2.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa2.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa3.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa3.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa4.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa4.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa5.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa5.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa6.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa6.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa7.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa7.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchAlfa8.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchAlfa8.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchNum1.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchNum1.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchNum2.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchNum2.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchFecha1.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchFecha1.ThemeName = "MaterialBlueGrey";
            this.radToggleSwitchFecha2.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchFecha2.ThemeName = "MaterialBlueGrey";
            
            this.radCollapsiblePanelCamposExtendidos.IsExpanded = false;
            this.radCollapsiblePanelCamposExtendidos.EnableAnimation = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM02_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Planes de Cuentas Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Acceso a campos extendidos
            //this.visibleCamposExt = false;
            this.radCollapsiblePanelCamposExtendidos.Visible = false;            
            //this.Size = new System.Drawing.Size(873, 400); 

            if (this.nuevo)
            {
                this.autEditar = true;

                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);

                //Botón acceso a campos extendidos
                this.radCollapsiblePanelCamposExtendidos.Visible = false;

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                //Botón acceso a campos extendidos
                this.radCollapsiblePanelCamposExtendidos.Visible = true;

                this.txtCodigo.Text = this.codigo;
                this.txtCodigo.IsReadOnly = true;

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    //Activa o no los controles para editar la estructura de la cuenta de mayor
                    this.EditarEstructuraCtaMayor();

                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }

                //Recuperar la información del plan de cuentas y mostrarla en los controles
                this.CargarInfoPlanCtas();
            }
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codPlan = this.txtCodigo.Text.Trim();

                if (codPlan == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Código de plan obligatorio", this.LP.GetText("errValCodPlan", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;

                }

                bool codPlanOk = true;
                if (this.nuevo) codPlanOk = this.CodigoPlanValido();    //Verificar que el codigo no exista

                if (codPlanOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActivo.Value = true;
                        //this.radToggleSwitchEstadoActivo.Enabled = false;
                        this.radToggleSwitchEstadoActivo.Enabled = true;
                    }
                    this.txtCodigo.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);

                    if (!this.nuevo)
                    {
                        this.radCollapsiblePanelCamposExtendidos.Visible = true;
                    }
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de plan ya existe", this.LP.GetText("errValCodPlanExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtEstNivel_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sólo caracteres numéricos
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void TxtMascara_KeyPress(object sender, KeyPressEventArgs e)
        {
            //No pueden ser caracteres numéricos ni letras
            e.Handled = (char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar)) && !char.IsControl(e.KeyChar);
        }

        private void TxtLongAlfa_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sólo caracteres numéricos
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void FrmMtoGLM02_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadCollapsiblePanelCamposExtendidos_Expanded(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(883, 680);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 100);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 100);

            this.CentrarForm();
        }

        private void RadCollapsiblePanelCamposExtendidos_Collapsed(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(883, 450);
            this.radButtonSave.Location = new Point(this.radButtonSave.Location.X, this.Size.Height - 100);
            this.radButtonExit.Location = new Point(this.radButtonExit.Location.X, this.Size.Height - 100);

            this.CentrarForm();

            //Size peq : 962; 420
            //Size grande : 962; 644
            /*
            this.radCollapsiblePanel1.IsExpanded = false;
            this.radCollapsiblePanel1.ControlsContainer.AutoScroll = true;*/
        }

        private void RadButtonElementTipoMonedaExt_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIMOME, DESCME from ";
            query += GlobalVar.PrefijoTablaCG + "GLT03 ";
            query += "order by DESCME";

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar tipo de moneda extranjera",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnas,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
                radButtonTextBoxTipoMonedaExt.Text = result;
            }
        }

        private void RadButtonElementSelAlfa1_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa1);
        }

        private void RadButtonElementSelAlfa2_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa2);
        }

        private void RadButtonElementSelAlfa3_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa3);
        }

        private void RadButtonElementSelAlfa4_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa4);
        }

        private void RadButtonElementSelAlfa5_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa5);
        }

        private void RadButtonElementSelAlfa6_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa6);
        }

        private void RadButtonElementSelAlfa7_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa7);
        }

        private void RadButtonElementSelAlfa8_Click(object sender, EventArgs e)
        {
            this.SeleccionarCamposExtendidosTipoAuxiliar(ref this.radButtonTextBoxSelAlfa8);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonCancelar_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmMtoGLM02_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtEstNivel1.Text != this.txtEstNivel1.Tag.ToString() ||
                    this.txtEstNivel2.Text != this.txtEstNivel2.Tag.ToString() ||
                    this.txtEstNivel3.Text != this.txtEstNivel3.Tag.ToString() ||
                    this.txtEstNivel4.Text != this.txtEstNivel4.Tag.ToString() ||
                    this.txtEstNivel5.Text != this.txtEstNivel5.Tag.ToString() ||
                    this.txtEstNivel6.Text != this.txtEstNivel6.Tag.ToString() ||
                    this.txtEstNivel7.Text != this.txtEstNivel7.Tag.ToString() ||
                    this.txtEstNivel8.Text != this.txtEstNivel8.Tag.ToString() ||
                    this.txtEstNivel9.Text != this.txtEstNivel9.Tag.ToString() ||
                    this.txtMascara1.Text != this.txtMascara1.Tag.ToString() ||
                    this.txtMascara2.Text != this.txtMascara2.Tag.ToString() ||
                    this.txtMascara3.Text != this.txtMascara3.Tag.ToString() ||
                    this.txtMascara4.Text != this.txtMascara4.Tag.ToString() ||
                    this.txtMascara5.Text != this.txtMascara5.Tag.ToString() ||
                    this.txtMascara6.Text != this.txtMascara6.Tag.ToString() ||
                    this.txtMascara7.Text != this.txtMascara7.Tag.ToString() ||
                    this.txtMascara8.Text != this.txtMascara8.Tag.ToString() ||
                    this.radButtonTextBoxTipoMonedaExt.Text != this.radButtonTextBoxTipoMonedaExt.Tag.ToString() ||
                    this.radToggleSwitchDigitoAutoverif.Value != (bool)(this.radToggleSwitchDigitoAutoverif.Tag) ||
                    this.txtPgmValPrefDoc.Text != this.txtPgmValPrefDoc.Tag.ToString() ||
                    this.txtPgmValNumFactAmp.Text != this.txtPgmValNumFactAmp.Tag.ToString() ||
                    this.txtPgmValNumFactRectif.Text != this.txtPgmValNumFactRectif.Tag.ToString() ||
                    this.txtPgmValFechaServicio.Text != this.txtPgmValFechaServicio.Tag.ToString() ||
                    this.radToggleSwitchActivoPrefDoc.Value != (bool)(this.radToggleSwitchActivoPrefDoc.Tag) ||
                    this.radToggleSwitchActivoNumFactAmp.Value != (bool)(this.radToggleSwitchActivoNumFactAmp.Tag) ||
                    this.radToggleSwitchActivoNumFactRectif.Value != (bool)(this.radToggleSwitchActivoNumFactRectif.Tag) ||
                    this.radToggleSwitchFechaServicio.Value != (bool)(this.radToggleSwitchFechaServicio.Tag) ||
                    this.txtAlfa1.Text != this.txtAlfa1.Tag.ToString() ||
                    this.txtAlfa2.Text != this.txtAlfa2.Tag.ToString() ||
                    this.txtAlfa3.Text != this.txtAlfa3.Tag.ToString() ||
                    this.txtAlfa4.Text != this.txtAlfa4.Tag.ToString() ||
                    this.txtAlfa5.Text != this.txtAlfa5.Tag.ToString() ||
                    this.txtAlfa6.Text != this.txtAlfa6.Tag.ToString() ||
                    this.txtAlfa7.Text != this.txtAlfa7.Tag.ToString() ||
                    this.txtAlfa8.Text != this.txtAlfa8.Tag.ToString() ||
                    this.txtLongAlfa1.Text != this.txtLongAlfa1.Tag.ToString() ||
                    this.txtLongAlfa2.Text != this.txtLongAlfa2.Tag.ToString() ||
                    this.txtLongAlfa3.Text != this.txtLongAlfa3.Tag.ToString() ||
                    this.txtLongAlfa4.Text != this.txtLongAlfa4.Tag.ToString() ||
                    this.txtLongAlfa5.Text != this.txtLongAlfa5.Tag.ToString() ||
                    this.txtLongAlfa6.Text != this.txtLongAlfa6.Tag.ToString() ||
                    this.txtLongAlfa7.Text != this.txtLongAlfa7.Tag.ToString() ||
                    this.txtLongAlfa8.Text != this.txtLongAlfa8.Tag.ToString() ||
                    this.txtPgmValAlfa1.Text != this.txtPgmValAlfa1.Tag.ToString() ||
                    this.txtPgmValAlfa2.Text != this.txtPgmValAlfa2.Tag.ToString() ||
                    this.txtPgmValAlfa3.Text != this.txtPgmValAlfa3.Tag.ToString() ||
                    this.txtPgmValAlfa4.Text != this.txtPgmValAlfa4.Tag.ToString() ||
                    this.txtPgmValAlfa5.Text != this.txtPgmValAlfa5.Tag.ToString() ||
                    this.txtPgmValAlfa6.Text != this.txtPgmValAlfa6.Tag.ToString() ||
                    this.txtPgmValAlfa7.Text != this.txtPgmValAlfa7.Tag.ToString() ||
                    this.txtPgmValAlfa8.Text != this.txtPgmValAlfa8.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa1.Text != this.radButtonTextBoxSelAlfa1.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa2.Text != this.radButtonTextBoxSelAlfa2.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa3.Text != this.radButtonTextBoxSelAlfa3.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa4.Text != this.radButtonTextBoxSelAlfa4.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa5.Text != this.radButtonTextBoxSelAlfa5.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa6.Text != this.radButtonTextBoxSelAlfa6.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa7.Text != this.radButtonTextBoxSelAlfa7.Tag.ToString() ||
                    this.radButtonTextBoxSelAlfa8.Text != this.radButtonTextBoxSelAlfa8.Tag.ToString() ||
                    this.radToggleSwitchAlfa1.Value != (bool)(this.radToggleSwitchAlfa1.Tag) ||
                    this.radToggleSwitchAlfa2.Value != (bool)(this.radToggleSwitchAlfa2.Tag) ||
                    this.radToggleSwitchAlfa3.Value != (bool)(this.radToggleSwitchAlfa3.Tag) ||
                    this.radToggleSwitchAlfa4.Value != (bool)(this.radToggleSwitchAlfa4.Tag) ||
                    this.radToggleSwitchAlfa5.Value != (bool)(this.radToggleSwitchAlfa5.Tag) ||
                    this.radToggleSwitchAlfa6.Value != (bool)(this.radToggleSwitchAlfa6.Tag) ||
                    this.radToggleSwitchAlfa7.Value != (bool)(this.radToggleSwitchAlfa7.Tag) ||
                    this.radToggleSwitchAlfa8.Value != (bool)(this.radToggleSwitchAlfa8.Tag) ||
                    this.txtNum1.Text != this.txtNum1.Tag.ToString() ||
                    this.txtNum2.Text != this.txtNum2.Tag.ToString() ||
                    this.txtPgmValNum1.Text != this.txtPgmValNum1.Tag.ToString() ||
                    this.txtPgmValNum2.Text != this.txtPgmValNum2.Tag.ToString() ||
                    this.radToggleSwitchNum1.Value != (bool)(this.radToggleSwitchNum1.Tag) ||
                    this.radToggleSwitchNum2.Value != (bool)(this.radToggleSwitchNum2.Tag) ||
                    this.txtFecha1.Text != this.txtFecha1.Tag.ToString() ||
                    this.txtFecha2.Text != this.txtFecha2.Tag.ToString() ||
                    this.txtPgmValFecha1.Text != this.txtPgmValFecha1.Tag.ToString() ||
                    this.txtPgmValFecha2.Text != this.txtPgmValFecha2.Tag.ToString() ||
                    this.radToggleSwitchFecha1.Value != (bool)(this.radToggleSwitchFecha1.Tag) ||
                    this.radToggleSwitchFecha2.Value != (bool)(this.radToggleSwitchFecha2.Tag)
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Planes de Cuentas Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            try
            {
                //Falta traducir todos los campos !!!
                //Recuperar literales del formulario
                if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM02TituloALta", "Mantenimiento de Planes de Cuentas - Alta");
                else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM02TituloEdit", "Mantenimiento de Planes de Cuentas - Edición");

                //Traducir los Literales de los ToolStrip
                this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
                this.radCollapsiblePanelCamposExtendidos.HeaderText = this.LP.GetText("lblGLM02CamposExt", "Campos extendidos");
                this.radButtonExit.Text = this.LP.GetText("toolStripSalir", "Cancelar");

                //Traducir los campos del formulario
                this.lblPlanCuentas.Text = this.LP.GetText("lblGLM02Plan", "Plan de cuentas");
                //this.lblEstado.Text = this.LP.GetText("lblEstado", "Activoo");
                this.lblNombre.Text = this.LP.GetText("lblNombre", "Nombre");
                this.lblEstructura.Text = this.LP.GetText("lblGLM02EstCtaMayor", "Estructura de cuenta de Mayor");
                this.lblMascaraEdicion.Text = this.LP.GetText("lblGLM02MascaraEdi", "Máscara de edición");
                this.lblTipoMonedaExt.Text = this.LP.GetText("lblGLM02TipoMonedaExt", "Tipo de moneda extranjera");
                this.lblDigitoAutoVerif.Text = this.LP.GetText("lblGLM02DigitoAuto", "Dígito autoverificación");

                //Traducir los campos extendidos del formulario
                this.radCollapsiblePanelCamposExtendidos.Text = this.LP.GetText("lblGLM02CamposExt", "Campos extendidos");
                this.lblTipoCampo.Text = this.LP.GetText("lblGLM02TipoCampo", "Tipo de campo");
                this.lblNombreCampo.Text = this.LP.GetText("lblGLM02NomCampo", "Nombre de campo");
                this.lblLongMax.Text = this.LP.GetText("lblGLM02LongMax", "Long. Max.");
                this.lblLong.Text = this.LP.GetText("lblGLM02Longitud", "Longitud");

                this.lblPgmValidUsuario.Text = this.LP.GetText("lblGLM02PgmValUser", "Pgm. Validar Usuario");
                this.lblTipoAux.Text = this.LP.GetText("lblGLM02TipoAux", "Tipo auxiliar");
                this.lblCampoActivo.Text = this.LP.GetText("lblGLM02CampoActivo", "Campo Activo");
                this.lblFijo.Text = this.LP.GetText("lblGLM0Fijo", "Fijo");
                this.lblPrefijoDoc.Text = this.LP.GetText("lblGLM02PrefDoc", "Prefijo de documento");
                this.lblNumFactAmp.Text = this.LP.GetText("lblGLM02NumFactAmp", "Número factura ampliado");
                this.lblNumFactRectif.Text = this.LP.GetText("lblGLM0NumFactRect", "Número factura rectificativa");
                this.lblFechaServicio.Text = this.LP.GetText("lblGLM02FechaServ", "Fecha servicio");
                this.lblAlfa.Text = this.LP.GetText("lblGLM02Alfa", "Alfanumérico");
                this.lblNumerico.Text = this.LP.GetText("lblGLM02Numerico", "Numérico");
                this.lblFecha.Text = this.LP.GetText("lblGLM02Fecha", "Fecha");

            } catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActivo.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.txtEstNivel1.Enabled = valor;
            this.txtEstNivel2.Enabled = valor;
            this.txtEstNivel3.Enabled = valor;
            this.txtEstNivel4.Enabled = valor;
            this.txtEstNivel5.Enabled = valor;
            this.txtEstNivel6.Enabled = valor;
            this.txtEstNivel7.Enabled = valor;
            this.txtEstNivel8.Enabled = valor;
            this.txtEstNivel9.Enabled = valor;
            this.txtMascara1.Enabled = valor;
            this.txtMascara2.Enabled = valor;
            this.txtMascara3.Enabled = valor;
            this.txtMascara4.Enabled = valor;
            this.txtMascara5.Enabled = valor;
            this.txtMascara6.Enabled = valor;
            this.txtMascara7.Enabled = valor;
            this.txtMascara8.Enabled = valor;
            this.radButtonTextBoxTipoMonedaExt.Enabled = valor;
            this.radButtonElementTipoMonedaExt.Enabled = valor;
            this.radToggleSwitchDigitoAutoverif.Enabled = valor;
        }

        private void CargarInfoPlanCtas()
        {
            IDataReader dr = null;
            try
            {
                string estado = "";
                string digitoVerif = "";
                string estructura = "";
                string mascara = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where TIPLMP = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBMP")).ToString().Trim();

                    estado = dr.GetValue(dr.GetOrdinal("STATMP")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                    else this.radToggleSwitchEstadoActivo.Value = false;

                    digitoVerif = dr.GetValue(dr.GetOrdinal("DIGIMP")).ToString().Trim();
                    if (digitoVerif == "N") this.radToggleSwitchDigitoAutoverif.Value = false;
                    else this.radToggleSwitchDigitoAutoverif.Value = true;
                    
                    //Campos estructura
                    estructura = dr.GetValue(dr.GetOrdinal("ECMP")).ToString();
                    this.CargarInfoEstructura(estructura);
                    
                    //Campos máscara
                    mascara = dr.GetValue(dr.GetOrdinal("EMMP")).ToString();
                    this.mascaraInicial = mascara;
                    this.CargarInfoMascara(mascara);

                    //Campo Moneda Extranjera
                    this.radButtonTextBoxTipoMonedaExt.Text = dr.GetValue(dr.GetOrdinal("TIMOMP")).ToString().Trim();

                    //Cargar los valores de los campos extendidos
                    this.CargarInfoCamposExtendidos();
                }

                dr.Close();

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Carga los datos de la estructura en los controles que corresponda
        /// </summary>
        /// <param name="estructura"></param>
        private void CargarInfoEstructura(string estructura)
        {
            try
            {
                if (estructura != "")
                {
                    int valor;
                    int posi = 0;
                    for (int i = 0; i < estructura.Length; i++)
                    {
                        posi++;
                        char aux = estructura[i];
                        valor = Convert.ToInt16(aux.ToString());

                        if (aux != '0')
                        {
                            switch (posi)
                            {
                                case 1:
                                    this.txtEstNivel1.Text = valor.ToString();
                                    break;
                                case 2:
                                    this.txtEstNivel2.Text = valor.ToString();
                                    break;
                                case 3:
                                    this.txtEstNivel3.Text = valor.ToString();
                                    break;
                                case 4:
                                    this.txtEstNivel4.Text = valor.ToString();
                                    break;
                                case 5:
                                    this.txtEstNivel5.Text = valor.ToString();
                                    break;
                                case 6:
                                    this.txtEstNivel6.Text = valor.ToString();
                                    break;
                                case 7:
                                    this.txtEstNivel7.Text = valor.ToString();
                                    break;
                                case 8:
                                    this.txtEstNivel8.Text = valor.ToString();
                                    break;
                                case 9:
                                    this.txtEstNivel9.Text = valor.ToString();
                                    break;
                            }
                        }
                        else
                        {
                            switch (posi)
                            {
                                case 1:
                                    if (this.txtEstNivel1.IsReadOnly) this.txtEstNivel1.Visible = false;
                                    break;
                                case 2:
                                    if (this.txtEstNivel2.IsReadOnly) this.txtEstNivel2.Visible = false;
                                    break;
                                case 3:
                                    if (this.txtEstNivel3.IsReadOnly) this.txtEstNivel3.Visible = false;
                                    break;
                                case 4:
                                    if (this.txtEstNivel4.IsReadOnly) this.txtEstNivel4.Visible = false;
                                    break;
                                case 5:
                                    if (this.txtEstNivel5.IsReadOnly) this.txtEstNivel5.Visible = false;
                                    break;
                                case 6:
                                    if (this.txtEstNivel6.IsReadOnly) this.txtEstNivel6.Visible = false;
                                    break;
                                case 7:
                                    if (this.txtEstNivel7.IsReadOnly) this.txtEstNivel7.Visible = false;
                                    break;
                                case 8:
                                    if (this.txtEstNivel8.IsReadOnly) this.txtEstNivel8.Visible = false;
                                    break;
                                case 9:
                                    if (this.txtEstNivel9.IsReadOnly) this.txtEstNivel9.Visible = false;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

        }

        /// <summary>
        /// Carga los datos de la máscara en los controles que corresponda
        /// </summary>
        /// <param name="mascara"></param>
        private void CargarInfoMascara(string mascara)
        {
            try
            {
                if (mascara != "")
                {
                    int posi = 0;
                    for (int i = 0; i < mascara.Length; i++)
                    {
                        posi++;
                        char aux = mascara[i];
                        if (aux != ' ')
                        {
                            switch (posi)
                            {
                                case 1:
                                    this.txtMascara1.Text = aux.ToString();
                                    break;
                                case 2:
                                    this.txtMascara2.Text = aux.ToString();
                                    break;
                                case 3:
                                    this.txtMascara3.Text = aux.ToString();
                                    break;
                                case 4:
                                    this.txtMascara4.Text = aux.ToString();
                                    break;
                                case 5:
                                    this.txtMascara5.Text = aux.ToString();
                                    break;
                                case 6:
                                    this.txtMascara6.Text = aux.ToString();
                                    break;
                                case 7:
                                    this.txtMascara7.Text = aux.ToString();
                                    break;
                                case 8:
                                    this.txtMascara8.Text = aux.ToString();
                                    break;
                            }
                        }
                        else
                        {
                            switch (posi)
                            {
                                case 1:
                                    if (this.txtEstNivel1.IsReadOnly) this.txtMascara1.Visible = false;
                                    break;
                                case 2:
                                    if (this.txtEstNivel2.IsReadOnly) this.txtMascara2.Visible = false;
                                    break;
                                case 3:
                                    if (this.txtEstNivel3.IsReadOnly) this.txtMascara3.Visible = false;
                                    break;
                                case 4:
                                    if (this.txtEstNivel4.IsReadOnly) this.txtMascara4.Visible = false;
                                    break;
                                case 5:
                                    if (this.txtEstNivel5.IsReadOnly) this.txtMascara5.Visible = false;
                                    break;
                                case 6:
                                    if (this.txtEstNivel6.IsReadOnly) this.txtMascara6.Visible = false;
                                    break;
                                case 7:
                                    if (this.txtEstNivel7.IsReadOnly) this.txtMascara7.Visible = false;
                                    break;
                                case 8:
                                    if (this.txtEstNivel8.IsReadOnly) this.txtMascara8.Visible = false;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga los datos de la máscara en los controles que corresponda
        /// </summary>
        private void CargarInfoCamposExtendidos()
        {
            IDataReader dr = null;
            try
            {
                
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                query += "where TIPLPX = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                string activo;
                if (dr.Read())
                {
                    this.existeInfoCamposExt = true;

                    //----------------- Campos fijos ----------------
                    //-------------- Prefijo Documento --------------
                    this.txtPgmValPrefDoc.Text = dr.GetValue(dr.GetOrdinal("PGPRPX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FGPRPX")).ToString().Trim();
                    this.radToggleSwitchActivoPrefDoc.Value = (activo == "1") ? true : false;
                    //-------------- Factura Ampliada --------------
                    this.txtPgmValNumFactAmp.Text = dr.GetValue(dr.GetOrdinal("PGFAPX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FGFAPX")).ToString().Trim();
                    this.radToggleSwitchActivoNumFactAmp.Value = (activo == "1") ? true : false;
                    //-------------- Factura Rectificativa --------------
                    this.txtPgmValNumFactRectif.Text = dr.GetValue(dr.GetOrdinal("PGFRPX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FGFRPX")).ToString().Trim();
                    this.radToggleSwitchActivoNumFactRectif.Value = (activo == "1") ? true : false;
                    //-------------- Fecha Servicio --------------
                    this.txtPgmValFechaServicio.Text = dr.GetValue(dr.GetOrdinal("PGDVPX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FGDVPX")).ToString().Trim();
                    this.radToggleSwitchFechaServicio.Value = (activo == "1") ? true : false;
                    //----------------- Campos Alfanuméricos ----------------
                    //-------------- Alfa 1 -------------- 
                    this.txtAlfa1.Text = dr.GetValue(dr.GetOrdinal("NM01PX")).ToString().Trim();
                    this.txtLongAlfa1.Text = dr.GetValue(dr.GetOrdinal("MX01PX")).ToString().Trim();
                    this.txtPgmValAlfa1.Text = dr.GetValue(dr.GetOrdinal("PG01PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa1.Text = dr.GetValue(dr.GetOrdinal("TA01PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG01PX")).ToString().Trim();
                    this.radToggleSwitchAlfa1.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 2 -------------- 
                    this.txtAlfa2.Text = dr.GetValue(dr.GetOrdinal("NM02PX")).ToString().Trim();
                    this.txtLongAlfa2.Text = dr.GetValue(dr.GetOrdinal("MX02PX")).ToString().Trim();
                    this.txtPgmValAlfa2.Text = dr.GetValue(dr.GetOrdinal("PG02PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa2.Text = dr.GetValue(dr.GetOrdinal("TA02PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG02PX")).ToString().Trim();
                    this.radToggleSwitchAlfa2.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 3 -------------- 
                    this.txtAlfa3.Text = dr.GetValue(dr.GetOrdinal("NM03PX")).ToString().Trim();
                    this.txtLongAlfa3.Text = dr.GetValue(dr.GetOrdinal("MX03PX")).ToString().Trim();
                    this.txtPgmValAlfa3.Text = dr.GetValue(dr.GetOrdinal("PG03PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa3.Text = dr.GetValue(dr.GetOrdinal("TA03PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG03PX")).ToString().Trim();
                    this.radToggleSwitchAlfa3.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 4 -------------- 
                    this.txtAlfa4.Text = dr.GetValue(dr.GetOrdinal("NM04PX")).ToString().Trim();
                    this.txtLongAlfa4.Text = dr.GetValue(dr.GetOrdinal("MX04PX")).ToString().Trim();
                    this.txtPgmValAlfa4.Text = dr.GetValue(dr.GetOrdinal("PG04PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa4.Text = dr.GetValue(dr.GetOrdinal("TA04PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG02PX")).ToString().Trim();
                    this.radToggleSwitchAlfa4.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 5 -------------- 
                    this.txtAlfa5.Text = dr.GetValue(dr.GetOrdinal("NM05PX")).ToString().Trim();
                    this.txtLongAlfa5.Text = dr.GetValue(dr.GetOrdinal("MX05PX")).ToString().Trim();
                    this.txtPgmValAlfa5.Text = dr.GetValue(dr.GetOrdinal("PG05PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa5.Text = dr.GetValue(dr.GetOrdinal("TA05PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG05PX")).ToString().Trim();
                    this.radToggleSwitchAlfa5.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 6 -------------- 
                    this.txtAlfa6.Text = dr.GetValue(dr.GetOrdinal("NM06PX")).ToString().Trim();
                    this.txtLongAlfa6.Text = dr.GetValue(dr.GetOrdinal("MX06PX")).ToString().Trim();
                    this.txtPgmValAlfa6.Text = dr.GetValue(dr.GetOrdinal("PG06PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa6.Text = dr.GetValue(dr.GetOrdinal("TA06PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG06PX")).ToString().Trim();
                    this.radToggleSwitchAlfa6.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 7 -------------- 
                    this.txtAlfa7.Text = dr.GetValue(dr.GetOrdinal("NM07PX")).ToString().Trim();
                    this.txtLongAlfa7.Text = dr.GetValue(dr.GetOrdinal("MX07PX")).ToString().Trim();
                    this.txtPgmValAlfa7.Text = dr.GetValue(dr.GetOrdinal("PG07PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa7.Text = dr.GetValue(dr.GetOrdinal("TA07PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG07PX")).ToString().Trim();
                    this.radToggleSwitchAlfa7.Value = (activo == "1") ? true : false;
                    //-------------- Alfa 8 -------------- 
                    this.txtAlfa8.Text = dr.GetValue(dr.GetOrdinal("NM08PX")).ToString().Trim();
                    this.txtLongAlfa8.Text = dr.GetValue(dr.GetOrdinal("MX08PX")).ToString().Trim();
                    this.txtPgmValAlfa8.Text = dr.GetValue(dr.GetOrdinal("PG08PX")).ToString().Trim();
                    this.radButtonTextBoxSelAlfa8.Text = dr.GetValue(dr.GetOrdinal("TA08PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG08PX")).ToString().Trim();
                    this.radToggleSwitchAlfa8.Value = (activo == "1") ? true : false;

                    //----------------- Campos Numéricos ----------------
                    //-------------- Numérico 1 -------------- 
                    this.txtNum1.Text = dr.GetValue(dr.GetOrdinal("NM09PX")).ToString().Trim();
                    this.txtPgmValNum1.Text = dr.GetValue(dr.GetOrdinal("PG09PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG09PX")).ToString().Trim();
                    this.radToggleSwitchNum1.Value = (activo == "1") ? true : false;
                    //-------------- Numérico 2 -------------- 
                    this.txtNum2.Text = dr.GetValue(dr.GetOrdinal("NM10PX")).ToString().Trim();
                    this.txtPgmValNum2.Text = dr.GetValue(dr.GetOrdinal("PG10PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG10PX")).ToString().Trim();
                    this.radToggleSwitchNum2.Value = (activo == "1") ? true : false;

                    //----------------- Campos Fechas ----------------
                    //-------------- Fecha 1 -------------- 
                    this.txtFecha1.Text = dr.GetValue(dr.GetOrdinal("NM11PX")).ToString().Trim();
                    this.txtPgmValFecha1.Text = dr.GetValue(dr.GetOrdinal("PG11PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG11PX")).ToString().Trim();
                    this.radToggleSwitchFecha1.Value = (activo == "1") ? true : false;
                    //-------------- Fecha 2 -------------- 
                    this.txtFecha2.Text = dr.GetValue(dr.GetOrdinal("NM12PX")).ToString().Trim();
                    this.txtPgmValFecha2.Text = dr.GetValue(dr.GetOrdinal("PG12PX")).ToString().Trim();
                    activo = dr.GetValue(dr.GetOrdinal("FG12PX")).ToString().Trim();
                    this.radToggleSwitchFecha2.Value = (activo == "1") ? true : false;
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
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

            try
            {
                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "- El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.txtNombre.Focus();
                }

                if (!this.radButtonTextBoxTipoMonedaExt.ReadOnly)
                {
                    if (this.radButtonTextBoxTipoMonedaExt.Text.Trim() != "")
                    {
                        string resultValidarTipoMoneda = this.ValidarTipoMoneda();
                        if (resultValidarTipoMoneda != "")
                        {
                            errores += "- " + resultValidarTipoMoneda + "\n\r";
                            this.radButtonTextBoxTipoMonedaExt.Focus();
                        }
                    }
                }

                //Validar la estructura
                int estNiv1 = (this.txtEstNivel1.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel1.Text.Trim());
                int estNiv2 = (this.txtEstNivel2.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel2.Text.Trim());
                int estNiv3 = (this.txtEstNivel3.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel3.Text.Trim());
                int estNiv4 = (this.txtEstNivel4.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel4.Text.Trim());
                int estNiv5 = (this.txtEstNivel5.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel5.Text.Trim());
                int estNiv6 = (this.txtEstNivel6.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel6.Text.Trim());
                int estNiv7 = (this.txtEstNivel7.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel7.Text.Trim());
                int estNiv8 = (this.txtEstNivel8.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel8.Text.Trim());
                int estNiv9 = (this.txtEstNivel9.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtEstNivel9.Text.Trim());

                string resultValidarEstructura = this.ValidarEstructura(estNiv1, estNiv2, estNiv3, estNiv4, estNiv5, estNiv6, estNiv7, estNiv8, estNiv9);
                if (resultValidarEstructura != "")
                {
                    errores += resultValidarEstructura + "\n\r";
                    this.txtEstNivel1.Focus();
                }
                else
                {
                    //Validar la máscara
                    string resultValidarMascara = this.ValidarMascara(estNiv1, estNiv2, estNiv3, estNiv4, estNiv5, estNiv6, estNiv7, estNiv8, estNiv9);
                    if (resultValidarMascara != "")
                    {
                        errores += resultValidarMascara + "\n\r";
                        this.txtMascara1.Focus();
                    }
                }

                errores += this.ValidarCamposExtendidos();

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Valida que el tipo de moneda sea correcto
        /// </summary>
        /// <returns></returns>
        private string ValidarTipoMoneda()
        {
            string result = "";
            try
            {
                string tipoMoneda = this.radButtonTextBoxTipoMonedaExt.Text.Trim();

                if (tipoMoneda != "")
                {
                    string query = "select count(TIMOME) from " + GlobalVar.PrefijoTablaCG + "GLT03 ";
                    query += "where TIMOME = '" + tipoMoneda + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = "Tipo de moneda extranjera no existe";   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el tipo de moneda extranjera (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida que la estructura sea correcta
        /// </summary>
        /// <returns></returns>
        private string ValidarEstructura(int estNiv1, int estNiv2, int estNiv3, int estNiv4, int estNiv5, int estNiv6, int estNiv7, int estNiv8, int estNiv9)
        {
            string result = "";

            if (estNiv1 == 0)
            {
                result = "- " + "Código no válido para la estructura \n\r";     //Falta traducir
                this.txtEstNivel1.Focus();
                return(result);
            }

            if (estNiv2 == 0)
            {
                result = "- " + "Mínimo dos niveles para la estructura \n\r";     //Falta traducir
                this.txtEstNivel2.Focus();
                return(result);
            }

            if ((estNiv3 == 0 && (estNiv4 > 0 || estNiv5 > 0 || estNiv6 > 0 || estNiv7 > 0 || estNiv8 > 0 || estNiv9 > 0)) ||
                (estNiv4 == 0 && (estNiv5 > 0 || estNiv6 > 0 || estNiv7 > 0 || estNiv8 > 0 || estNiv9 > 0)) ||
                (estNiv5 == 0 && (estNiv6 > 0 || estNiv7 > 0 || estNiv8 > 0 || estNiv9 > 0)) ||
                (estNiv6 == 0 && (estNiv7 > 0 || estNiv8 > 0 || estNiv9 > 0)) ||
                (estNiv7 == 0 && (estNiv8 > 0 || estNiv9 > 0)) ||
                (estNiv8 == 0 && (estNiv9 > 0)))
            {
                result = "- " + "Código inválido \n\r";     //Falta traducir
                this.txtEstNivel2.Focus();
                return(result);
            }

            int suma = estNiv1 + estNiv2 + estNiv3 + estNiv4 + estNiv5 + estNiv6 + estNiv7 + estNiv8 + estNiv9;
            if (suma > 15)
            {
                result = "- " + "Estructura inválida, suma más de 15 \n\r"; //Falta traducir
                this.txtEstNivel2.Focus();
            }

            return(result);
        }

        /// <summary>
        /// Valida que la mascara sea correcta
        /// </summary>
        /// <returns></returns>
        private string ValidarMascara(int estNiv1, int estNiv2, int estNiv3, int estNiv4, int estNiv5, int estNiv6, int estNiv7, int estNiv8, int estNiv9)
        {
            string result = "";

            int cantNiveles = 0;
 
            cantNiveles = (estNiv1 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv2 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv3 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv4 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv5 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv6 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv7 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv8 > 0) ? cantNiveles + 1 : cantNiveles;
            cantNiveles = (estNiv9 > 0) ? cantNiveles + 1 : cantNiveles;

            string mascara1 = this.txtMascara1.Text.Trim();
            string mascara2 = this.txtMascara2.Text.Trim();
            string mascara3 = this.txtMascara3.Text.Trim();
            string mascara4 = this.txtMascara4.Text.Trim();
            string mascara5 = this.txtMascara5.Text.Trim();
            string mascara6 = this.txtMascara6.Text.Trim();
            string mascara7 = this.txtMascara7.Text.Trim();
            string mascara8 = this.txtMascara8.Text.Trim();

            string mascaraActual = "";
            for (int i = 1; i < cantNiveles; i++)
            {
                switch (i)
                {
                    case 1:
                        mascaraActual = mascara1;
                        break;
                    case 2:
                        mascaraActual = mascara2;
                        break;
                    case 3:
                        mascaraActual = mascara3;
                        break;
                    case 4:
                        mascaraActual = mascara4;
                        break;
                    case 5:
                        mascaraActual = mascara5;
                        break;
                    case 6:
                        mascaraActual = mascara6;
                        break;
                    case 7:
                        mascaraActual = mascara7;
                        break;
                    case 8:
                        mascaraActual = mascara8;
                        break;
                }

                if (mascaraActual == "")
                {
                    result = "- " + "Estructura de máscara inválida \n\r"; //Falta traducir
                    this.txtMascara1.Focus();
                    return(result);
                }
            }

            if (cantNiveles < 9)
            {
                bool error = false;
                for (int i = cantNiveles+1; i < 9; i++)
                {
                    switch (i)
                    {
                        case 3:
                            if (mascara3 != "")
                            {
                                error = true;
                                this.txtMascara3.Focus();
                            }
                            break;
                        case 4:
                            if (mascara4 != "")
                            {
                                error = true;
                                this.txtMascara4.Focus();
                            }
                            break;
                        case 5:
                            if (mascara5 != "")
                            {
                                error = true;
                                this.txtMascara5.Focus();
                            }
                            break;
                        case 6:
                            if (mascara6 != "")
                            {
                                error = true;
                                this.txtMascara6.Focus();
                            }
                            break;
                        case 7:
                            if (mascara7 != "")
                            {
                                error = true;
                                this.txtMascara7.Focus();
                            }
                            break;
                        case 8:
                            if (mascara8 != "")
                            {
                                error = true;
                                this.txtMascara8.Focus();
                            }
                            break;
                    }

                    if (error)
                    {
                        result = "- " + "Estructura de máscara inválida \n\r"; //Falta traducir
                        this.txtMascara1.Focus();
                        return (result);
                    }
                }
            }

            return (result);
        }

        /// <summary>
        /// Validar campos extendidos
        /// </summary>
        /// <returns></returns>
        private string ValidarCamposExtendidos()
        {
            string result = "";

            //--------------------- Alfanumérico Líneas Tipo de auxiliar ---------------------
            string validarTipoAux = "";
            if (this.radButtonTextBoxSelAlfa1.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa1.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa1.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa2.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa2.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa2.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa3.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa3.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa3.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa4.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa4.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa4.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa5.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa5.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa5.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa6.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa6.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa6.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa7.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa7.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa7.Focus();
                }
            }

            if (this.radButtonTextBoxSelAlfa8.Text.Trim() != "")
            {
                validarTipoAux = this.ValidarTipoAuxiliar(this.radButtonTextBoxSelAlfa8.Text.Trim());
                if (validarTipoAux != "")
                {
                    result += validarTipoAux + "\n\r";
                    this.radButtonTextBoxSelAlfa8.Focus();
                }
            }

            //---------------- Alfanumérico Líneas campo Activo, nombre del campo no puede estar en blanco   --------------------
            try
            {
                if (this.radToggleSwitchAlfa1.Value && this.txtAlfa1.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 1 no está informado" + "\n\r";
                    this.txtAlfa1.Focus();
                }
                if (this.radToggleSwitchAlfa2.Value && this.txtAlfa2.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 2 no está informado" + "\n\r";
                    this.txtAlfa2.Focus();
                }
                if (this.radToggleSwitchAlfa3.Value && this.txtAlfa3.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 3 no está informado" + "\n\r";
                    this.txtAlfa3.Focus();
                }
                if (this.radToggleSwitchAlfa4.Value && this.txtAlfa4.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 4 no está informado" + "\n\r";
                    this.txtAlfa4.Focus();
                }
                if (this.radToggleSwitchAlfa5.Value && this.txtAlfa5.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 5 no está informado" + "\n\r";
                    this.txtAlfa5.Focus();
                }
                if (this.radToggleSwitchAlfa6.Value && this.txtAlfa6.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 6 no está informado" + "\n\r";
                    this.txtAlfa6.Focus();
                }
                if (this.radToggleSwitchAlfa7.Value && this.txtAlfa7.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 7 no está informado" + "\n\r";
                    this.txtAlfa7.Focus();
                }
                if (this.radToggleSwitchAlfa8.Value && this.txtAlfa8.Text.Trim() == "")
                {
                    result += "Campo alfanumérico 8 no está informado" + "\n\r";
                    this.txtAlfa8.Focus();
                }
                if (this.radToggleSwitchNum1.Value && this.txtNum1.Text.Trim() == "")
                {
                    result += "Campo numérico 1 no está informado" + "\n\r";
                    this.txtNum1.Focus();
                }
                if (this.radToggleSwitchNum2.Value && this.txtNum2.Text.Trim() == "")
                {
                    result += "Campo numérico 2 no está informado" + "\n\r";
                    this.txtNum2.Focus();
                }
                if (this.radToggleSwitchFecha1.Value && this.txtFecha1.Text.Trim() == "")
                {
                    result += "Campo fecha 1 no está informado" + "\n\r";
                    this.txtFecha1.Focus();
                }
                if (this.radToggleSwitchFecha2.Value && this.txtFecha2.Text.Trim() == "")
                {
                    result += "Campo fecha 2 no está informado" + "\n\r";
                    this.txtFecha2.Focus();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //---------------- Alfanumérico Líneas campo longitud  --------------------
            try
            {
                int longAlfa1 = (this.txtLongAlfa1.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa1.Text.Trim());
                int longAlfa2 = (this.txtLongAlfa2.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa2.Text.Trim());
                int longAlfa3 = (this.txtLongAlfa3.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa3.Text.Trim());
                int longAlfa4 = (this.txtLongAlfa4.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa4.Text.Trim());
                int longAlfa5 = (this.txtLongAlfa5.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa5.Text.Trim());
                int longAlfa6 = (this.txtLongAlfa6.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa6.Text.Trim());
                int longAlfa7 = (this.txtLongAlfa7.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa7.Text.Trim());
                int longAlfa8 = (this.txtLongAlfa8.Text.Trim() == "") ? 0 : Convert.ToInt16(this.txtLongAlfa8.Text.Trim());

                string validarCampoLongitud = (longAlfa1 == 0) ? "" : this.ValidarCampoLongitud(longAlfa1, this.lblLongMaxAlfa1.Text, ref this.txtLongAlfa1, this.radButtonTextBoxSelAlfa1.Text.Trim(), ref this.txtAlfa1, 1);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa2 == 0) ? "" : this.ValidarCampoLongitud(longAlfa2, this.lblLongMaxAlfa2.Text, ref this.txtLongAlfa2, this.radButtonTextBoxSelAlfa2.Text.Trim(), ref this.txtAlfa2, 2);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa3 == 0) ? "" : this.ValidarCampoLongitud(longAlfa3, this.lblLongMaxAlfa3.Text, ref this.txtLongAlfa3, this.radButtonTextBoxSelAlfa3.Text.Trim(), ref this.txtAlfa3, 3);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa4 == 0) ? "" : this.ValidarCampoLongitud(longAlfa4, this.lblLongMaxAlfa4.Text, ref this.txtLongAlfa4, this.radButtonTextBoxSelAlfa4.Text.Trim(), ref this.txtAlfa4, 4);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa5 == 0) ? "" : this.ValidarCampoLongitud(longAlfa5, this.lblLongMaxAlfa5.Text, ref this.txtLongAlfa5, this.radButtonTextBoxSelAlfa5.Text.Trim(), ref this.txtAlfa5, 5);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa6 == 0) ? "" : this.ValidarCampoLongitud(longAlfa6, this.lblLongMaxAlfa6.Text, ref this.txtLongAlfa6, this.radButtonTextBoxSelAlfa6.Text.Trim(), ref this.txtAlfa6, 6);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa7 == 0) ? "" : this.ValidarCampoLongitud(longAlfa7, this.lblLongMaxAlfa7.Text, ref this.txtLongAlfa7, this.radButtonTextBoxSelAlfa7.Text.Trim(), ref this.txtAlfa7, 7);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
                validarCampoLongitud = (longAlfa8 == 0) ? "" : this.ValidarCampoLongitud(longAlfa8, this.lblLongMaxAlfa8.Text, ref this.txtLongAlfa8, this.radButtonTextBoxSelAlfa8.Text.Trim(), ref this.txtAlfa8, 8);
                if (validarCampoLongitud != "") result += validarCampoLongitud + "\n\r";
             }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //---------------- Alfanumérico Líneas bien informadas  --------------------
            string validarLinea = this.ValidarLineaAlfaBienInformada(1, ref this.txtAlfa1, this.txtLongAlfa1.Text.Trim(), this.txtPgmValAlfa1.Text.Trim(), this.radButtonTextBoxSelAlfa1.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(2, ref this.txtAlfa1, this.txtLongAlfa2.Text.Trim(), this.txtPgmValAlfa2.Text.Trim(), this.radButtonTextBoxSelAlfa2.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(3, ref this.txtAlfa1, this.txtLongAlfa3.Text.Trim(), this.txtPgmValAlfa3.Text.Trim(), this.radButtonTextBoxSelAlfa3.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(4, ref this.txtAlfa1, this.txtLongAlfa4.Text.Trim(), this.txtPgmValAlfa4.Text.Trim(), this.radButtonTextBoxSelAlfa4.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(5, ref this.txtAlfa1, this.txtLongAlfa5.Text.Trim(), this.txtPgmValAlfa5.Text.Trim(), this.radButtonTextBoxSelAlfa5.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(6, ref this.txtAlfa1, this.txtLongAlfa6.Text.Trim(), this.txtPgmValAlfa6.Text.Trim(), this.radButtonTextBoxSelAlfa6.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(7, ref this.txtAlfa1, this.txtLongAlfa7.Text.Trim(), this.txtPgmValAlfa7.Text.Trim(), this.radButtonTextBoxSelAlfa7.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";
            validarLinea = this.ValidarLineaAlfaBienInformada(8, ref this.txtAlfa1, this.txtLongAlfa8.Text.Trim(), this.txtPgmValAlfa8.Text.Trim(), this.radButtonTextBoxSelAlfa8.Text.Trim());
            if (validarLinea != "") result += validarLinea + "\n\r";

            //---------------------- Alfanumérico Líneas no alternas ---------------------
            if (validarLinea == "")
            {
                if ((this.txtAlfa1.Text.Trim() == "" && (this.txtAlfa2.Text.Trim() != "" || this.txtAlfa3.Text.Trim() != "" || this.txtAlfa4.Text.Trim() != "")) ||
                    (this.txtAlfa2.Text.Trim() == "" && (this.txtAlfa3.Text.Trim() != "" || this.txtAlfa4.Text.Trim() != "")) ||
                    (this.txtAlfa3.Text.Trim() == "" && (this.txtAlfa4.Text.Trim() != "")) ||
                    (this.txtAlfa5.Text.Trim() == "" && (this.txtAlfa6.Text.Trim() != "" || this.txtAlfa7.Text.Trim() != "" || this.txtAlfa8.Text.Trim() != "")) ||
                    (this.txtAlfa6.Text.Trim() == "" && (this.txtAlfa7.Text.Trim() != "" || this.txtAlfa8.Text.Trim() != "")) ||
                    (this.txtAlfa7.Text.Trim() == "" && (this.txtAlfa8.Text.Trim() != ""))
                    )
                {
                    result += "Alfanumérico: Los campos no están bien informados. Tienen que estar consecutivos"+ "\n\r";   //Falta traducir ¿?¿?
                    this.txtAlfa1.Focus();
                }
            }

            //---------------- Numérico Líneas bien informadas  --------------------
            string tipo = "Numérico";   //Falta traducir
            string validarLineaNum = this.ValidarLineaNumFechaBienInformada(1, tipo, ref this.txtNum1, this.txtPgmValNum1.Text.Trim());
            if (validarLineaNum != "") result += validarLineaNum + "\n\r";
            validarLineaNum = this.ValidarLineaNumFechaBienInformada(2, tipo, ref this.txtNum2, this.txtPgmValNum2.Text.Trim());
            if (validarLineaNum != "") result += validarLineaNum + "\n\r";

            //---------------------- Numérico Líneas no alternas ---------------------
            if (validarLineaNum == "" && (this.txtNum1.Text.Trim() == "" && this.txtNum2.Text.Trim() != ""))
            {
                result += "Numérico: Los campos no están bien informados. Tienen que estar consecutivos" + "\n\r";   //Falta traducir ¿?¿?
                this.txtNum1.Focus();
            }

            //---------------- Fecha Líneas bien informadas  --------------------
            tipo = "Fecha";   //Falta traducir
            string validarLineaFecha = this.ValidarLineaNumFechaBienInformada(1, tipo, ref this.txtFecha1, this.txtPgmValFecha1.Text.Trim());
            if (validarLineaFecha != "") result += validarLineaFecha + "\n\r";
            validarLineaFecha = this.ValidarLineaNumFechaBienInformada(2, tipo, ref this.txtFecha2, this.txtPgmValFecha2.Text.Trim());
            if (validarLineaFecha != "") result += validarLineaFecha + "\n\r";

            //---------------------- Numérico Líneas no alternas ---------------------
            if (validarLineaFecha == "" && (this.txtFecha1.Text.Trim() == "" && this.txtFecha2.Text.Trim() != ""))
            {
                result += "Fecha: Los campos no están bien informados. Tienen que estar consecutivos" + "\n\r";   //Falta traducir ¿?¿?
                this.txtFecha1.Focus();
            }

            //Mostrar los campos extendidos
            if (result != "")
            {
                this.radCollapsiblePanelCamposExtendidos.Expand();
                this.radCollapsiblePanelCamposExtendidos.Visible = true;
                //this.Size = new System.Drawing.Size(873, 578);
                //this.visibleCamposExt = true;
            }

            return (result);
        }

        /// <summary>
        /// Valida los tipos de auxiliar
        /// </summary>
        /// <param name="tipoAux"></param>
        /// <returns></returns>
        private string ValidarTipoAuxiliar(string tipoAux)
        {
            string result = "";

            try
            {
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + tipoAux + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Tipo de auxiliar (" + tipoAux + ") no existe";   //Falta traducir
                
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el tipo de auxiliar (" + tipoAux + ")(" + ex.Message + ")";   //Falta traducir
            }

            return(result);
        }

        /// <summary>
        /// Valida el campo longitud del apartado de campos extendidos
        /// </summary>
        /// <param name="longAlfa">Longitud entrada por el usuario</param>
        /// <param name="campoLongMaxAlfa">Longitud máxima del campo</param>
        /// <param name="campoTxtLongAlfa">TextBox de la longitud que entra el usuario</param>
        /// <param name="tipoAux">Tipo de Auxiliar</param>
        /// <returns></returns>
        private string ValidarCampoLongitud(int longAlfa, string campoLongMaxAlfa, ref Telerik.WinControls.UI.RadTextBoxControl campoTxtLongAlfa, string tipoAux, ref Telerik.WinControls.UI.RadTextBoxControl campotxtAlfa, int campoAlfanumericoPosicion)
        {
            string result = "";

            try
            {
                int lonMaxCampoInt = Convert.ToInt16(campoLongMaxAlfa);
                string campotxtAlfaMostrar = campotxtAlfa.Text.Trim();
                if (campotxtAlfaMostrar == "") campotxtAlfaMostrar = campoAlfanumericoPosicion.ToString();

                if (longAlfa > lonMaxCampoInt)
                {
                    result += "- Campo alfanumérico " + campotxtAlfaMostrar + ": La longitud no puede ser mayor que " + campoLongMaxAlfa;   //Falta traducir
                    campoTxtLongAlfa.Focus();
                }
                else
                {
                    if (longAlfa > 8 && tipoAux != "")
                    {
                        result += "- Campo alfanumérico " + campotxtAlfaMostrar + ": La longitud no puede ser mayor que 8";   //Falta traducir
                        campoTxtLongAlfa.Focus();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Valida que estén informados los campos de una línea de campos alfanuméricos
        /// </summary>
        /// <param name="campoAlfa">índice campo alfa</param>
        /// <param name="txtAlfa">TextBox Alfa</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="programaVal">Programa de validación</param>
        /// <param name="tipoAux">Tipo de auxiliar</param>
        /// <returns></returns>
        private string ValidarLineaAlfaBienInformada(int campoAlfa, ref Telerik.WinControls.UI.RadTextBoxControl txtAlfa, string longitud, string programaVal, string tipoAux)
        {
            string result = "";

            if (txtAlfa.Text.Trim() == "" && (longitud != "" || programaVal != "" || tipoAux != ""))
            {
                int longCampo = 15;
                if (campoAlfa > 4) longCampo = 25;

                if (longitud != "" )
                {
                    if (longitud != longCampo.ToString())
                    {
                        result = "Alfanumérico: línea " + campoAlfa + " falta informar campos";
                        txtAlfa.Focus();
                    }
                }
                else
                {
                    result = "Alfanumérico: línea " + campoAlfa + " falta informar campos";
                    txtAlfa.Focus();
                }
            }

            return (result);
        }

        /// <summary>
        /// Valida que estén informados los campos de una línea de campos numéricos o campos fecha
        /// </summary>
        /// <param name="campoAlfa">índice campo numérico o campo fecha</param>
        /// <param name="txtAlfa">TextBox Numerico o Fecha</param>
        /// <param name="programaVal">Programa de validación</param>
        /// <returns></returns>
        private string ValidarLineaNumFechaBienInformada(int campoNumFecha, string tipo, ref Telerik.WinControls.UI.RadTextBoxControl txtNumFecha, string programaVal)
        {
            string result = "";

            if (txtNumFecha.Text.Trim() == "" && programaVal != "")
            {
                result = tipo + ": línea " + campoNumFecha + " informar campos";    //Falta traducir
                txtNumFecha.Focus();
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a una compañía
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string estructura = this.ObtenerEstructura();
                string mascara = this.ObtenerMascara();
                string digitoVerificacion = this.radToggleSwitchDigitoAutoverif.Value == false ? "N" : "S";
                string CTINMP = " ";

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? estado = "V" : estado = "*";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM02";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATMP, TIPLMP, ECMP, EMMP, DIGIMP, TIMOMP, NOMBMP, CTINMP) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', " + estructura + ", '" + mascara;
                query += "','" + digitoVerificacion + "', '" + this.radButtonTextBoxTipoMonedaExt.Text.Trim() + "', '";
                query += this.txtNombre.Text + "', '"  + CTINMP + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM02", this.codigo, null);

                //Insertarlo como propietario del elemento
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + this.codigo + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, this.codigo);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una compañía
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string estructura = this.ObtenerEstructura();
                string mascara = this.ObtenerMascara();
                string digitoVerificacion = this.radToggleSwitchDigitoAutoverif.Value == false ? "N" : "S";
                string CTINMP = " ";

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? estado = "V" : estado = "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM02 set ";
                query += "STATMP = '" + estado + "', ECMP = " + estructura + ", EMMP = '" + mascara + "', ";
                query += "DIGIMP = '" + digitoVerificacion + "', TIMOMP = '" + this.radButtonTextBoxTipoMonedaExt.Text.Trim() + "', ";
                query += "NOMBMP = '" + this.txtNombre.Text + "', CTINMP = '" + CTINMP + "' ";
                query += "where TIPLMP = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM02", this.codigo, null);

                if (this.grabarInfoCamposExt)
                {
                    if (this.existeInfoCamposExt) this.ActualizarCamposExtendidos();
                    else this.InsertarCamposExtendidos();
                }

                //Cuentas de Mayor, actualizar campo cuenta editada si la máscara de edición cambia
                if (this.mascaraInicial != mascara) this.ActualizarCuentaFormateada(mascara, estructura);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta la información de los campos extendidos en la tabla GLX02
        /// </summary>
        private string  InsertarCamposExtendidos()
        {
            string result = "";
            try
            {
                string activoPrefDoc = this.radToggleSwitchActivoPrefDoc.Value ? "1" : "0";
                string activoFactAmp = this.radToggleSwitchActivoNumFactAmp.Value ? "1" : "0";
                string activoFactRec = this.radToggleSwitchActivoNumFactRectif.Value ? "1" : "0";
                string activoFecSer = this.radToggleSwitchFechaServicio.Value ? "1" : "0";
                string activoAlfa1 = this.radToggleSwitchAlfa1.Value ? "1" : "0";
                string activoAlfa2 = this.radToggleSwitchAlfa2.Value ? "1" : "0";
                string activoAlfa3 = this.radToggleSwitchAlfa3.Value ? "1" : "0";
                string activoAlfa4 = this.radToggleSwitchAlfa4.Value ? "1" : "0";
                string activoAlfa5 = this.radToggleSwitchAlfa5.Value ? "1" : "0";
                string activoAlfa6 = this.radToggleSwitchAlfa6.Value ? "1" : "0";
                string activoAlfa7 = this.radToggleSwitchAlfa7.Value ? "1" : "0";
                string activoAlfa8 = this.radToggleSwitchAlfa8.Value ? "1" : "0";
                string activoNum1 = this.radToggleSwitchNum1.Value ? "1" : "0";
                string activoNum2 = this.radToggleSwitchNum2.Value ? "1" : "0";
                string activoFecha1 = this.radToggleSwitchFecha1.Value ? "1" : "0";
                string activoFecha2 = this.radToggleSwitchFecha2.Value ? "1" : "0";

                string longAlfa1 = this.txtLongAlfa1.Text.Trim();
                longAlfa1 = (longAlfa1 != "") ? longAlfa1 : "15";
                string longAlfa2 = this.txtLongAlfa2.Text.Trim();
                longAlfa2 = (longAlfa2 != "") ? longAlfa2 : "15";
                string longAlfa3 = this.txtLongAlfa3.Text.Trim();
                longAlfa3 = (longAlfa3 != "") ? longAlfa3 : "15";
                string longAlfa4 = this.txtLongAlfa4.Text.Trim();
                longAlfa4 = (longAlfa4 != "") ? longAlfa4 : "15";

                string longAlfa5 = this.txtLongAlfa5.Text.Trim();
                longAlfa5 = (longAlfa5 != "") ? longAlfa5 : "25";
                string longAlfa6 = this.txtLongAlfa6.Text.Trim();
                longAlfa6 = (longAlfa6 != "") ? longAlfa6 : "25";
                string longAlfa7 = this.txtLongAlfa7.Text.Trim();
                longAlfa7 = (longAlfa7 != "") ? longAlfa7 : "25";
                string longAlfa8 = this.txtLongAlfa8.Text.Trim();
                longAlfa8 = (longAlfa8 != "") ? longAlfa8 : "25";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLMX2";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLPX, FGPRPX, PGPRPX, FGFAPX, PGFAPX, FGFRPX, PGFRPX, FGDVPX, PGDVPX, ";
                query += "NM01PX, MX01PX, PG01PX, TA01PX, FG01PX, NM02PX, MX02PX, PG02PX, TA02PX, FG02PX, ";
                query += "NM03PX, MX03PX, PG03PX, TA03PX, FG03PX, NM04PX, MX04PX, PG04PX, TA04PX, FG04PX, ";
                query += "NM05PX, MX05PX, PG05PX, TA05PX, FG05PX, NM06PX, MX06PX, PG06PX, TA06PX, FG06PX, ";
                query += "NM07PX, MX07PX, PG07PX, TA07PX, FG07PX, NM08PX, MX08PX, PG08PX, TA08PX, FG08PX, ";
                query += "NM09PX, PG09PX, FG09PX, NM10PX, PG10PX, FG10PX, ";
                query += "NM11PX, PG11PX, FG11PX, NM12PX, PG12PX, FG12PX) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.codigo + "', '" + activoPrefDoc + "', '" + this.txtPgmValPrefDoc.Text.Trim() + "', '" + activoFactAmp + "', '";
                query += this.txtPgmValNumFactAmp.Text.Trim() + "', '" + activoFactRec + "', '" + this.txtPgmValNumFactRectif.Text.Trim() + "', '" ;
                query += activoFecSer + "', '" + this.txtPgmValFechaServicio.Text.Trim() + "', '";
                query += this.txtAlfa1.Text.Trim() + "', " + longAlfa1 + ", '" + this.txtPgmValAlfa1.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa1.Text.Trim() + "', '" + activoAlfa1 + "', '";
                query += this.txtAlfa2.Text.Trim() + "', " + longAlfa2 + ", '" + this.txtPgmValAlfa2.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa2.Text.Trim() + "', '" + activoAlfa2 + "', '";
                query += this.txtAlfa3.Text.Trim() + "', " + longAlfa3 + ", '" + this.txtPgmValAlfa3.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa3.Text.Trim() + "', '" + activoAlfa3 + "', '";
                query += this.txtAlfa4.Text.Trim() + "', " + longAlfa4 + ", '" + this.txtPgmValAlfa4.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa4.Text.Trim() + "', '" + activoAlfa4 + "', '";
                query += this.txtAlfa5.Text.Trim() + "', " + longAlfa5 + ", '" + this.txtPgmValAlfa5.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa5.Text.Trim() + "', '" + activoAlfa5 + "', '";
                query += this.txtAlfa6.Text.Trim() + "', " + longAlfa6 + ", '" + this.txtPgmValAlfa6.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa6.Text.Trim() + "', '" + activoAlfa6 + "', '";
                query += this.txtAlfa7.Text.Trim() + "', " + longAlfa7 + ", '" + this.txtPgmValAlfa7.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa7.Text.Trim() + "', '" + activoAlfa7 + "', '";
                query += this.txtAlfa8.Text.Trim() + "', " + longAlfa8 + ", '" + this.txtPgmValAlfa8.Text.Trim() + "', '";
                query += this.radButtonTextBoxSelAlfa8.Text.Trim() + "', '" + activoAlfa8 + "', '";
                query += this.txtNum1.Text.Trim() + "', '" + this.txtPgmValNum1.Text.Trim() + "', '" + activoNum1 + "', '";
                query += this.txtNum2.Text.Trim() + "', '" + this.txtPgmValNum2.Text.Trim() + "', '" + activoNum2 + "', '";
                query += this.txtFecha1.Text.Trim() + "', '" + this.txtPgmValFecha1.Text.Trim() + "', '" + activoFecha1 + "', '";
                query += this.txtFecha2.Text.Trim() + "', '" + this.txtPgmValFecha2.Text.Trim() + "', '" + activoFecha2 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos de los campos extendidos (" + ex.Message + ")";   //Falta traducir
            }

            return(result);
        }

        /// <summary>
        /// Actualiza la información de los campos extendidos en la tabla GLX02
        /// </summary>
        private string ActualizarCamposExtendidos()
        {
            string result = "";
            try
            {
                string activoPrefDoc = this.radToggleSwitchActivoPrefDoc.Value ? "1" : "0";
                string activoFactAmp = this.radToggleSwitchActivoNumFactAmp.Value ? "1" : "0";
                string activoFactRec = this.radToggleSwitchActivoNumFactRectif.Value ? "1" : "0";
                string activoFecSer = this.radToggleSwitchFechaServicio.Value ? "1" : "0";
                string activoAlfa1 = this.radToggleSwitchAlfa1.Value ? "1" : "0";
                string activoAlfa2 = this.radToggleSwitchAlfa2.Value ? "1" : "0";
                string activoAlfa3 = this.radToggleSwitchAlfa3.Value ? "1" : "0";
                string activoAlfa4 = this.radToggleSwitchAlfa4.Value ? "1" : "0";
                string activoAlfa5 = this.radToggleSwitchAlfa5.Value ? "1" : "0";
                string activoAlfa6 = this.radToggleSwitchAlfa6.Value ? "1" : "0";
                string activoAlfa7 = this.radToggleSwitchAlfa7.Value ? "1" : "0";
                string activoAlfa8 = this.radToggleSwitchAlfa8.Value ? "1" : "0";
                string activoNum1 = this.radToggleSwitchNum1.Value ? "1" : "0";
                string activoNum2 = this.radToggleSwitchNum2.Value ? "1" : "0";
                string activoFecha1 = this.radToggleSwitchFecha1.Value ? "1" : "0";
                string activoFecha2 = this.radToggleSwitchFecha2.Value ? "1" : "0";

                string longAlfa1 = this.txtLongAlfa1.Text.Trim();
                longAlfa1 = (longAlfa1 != "") ? longAlfa1 : "15";
                string longAlfa2 = this.txtLongAlfa2.Text.Trim();
                longAlfa2 = (longAlfa2 != "") ? longAlfa2 : "15";
                string longAlfa3 = this.txtLongAlfa3.Text.Trim();
                longAlfa3 = (longAlfa3 != "") ? longAlfa3 : "15";
                string longAlfa4 = this.txtLongAlfa4.Text.Trim();
                longAlfa4 = (longAlfa4 != "") ? longAlfa4 : "15";

                string longAlfa5 = this.txtLongAlfa5.Text.Trim();
                longAlfa5 = (longAlfa5 != "") ? longAlfa5 : "25";
                string longAlfa6 = this.txtLongAlfa6.Text.Trim();
                longAlfa6 = (longAlfa6 != "") ? longAlfa6 : "25";
                string longAlfa7 = this.txtLongAlfa7.Text.Trim();
                longAlfa7= (longAlfa7 != "") ? longAlfa7 : "25";
                string longAlfa8 = this.txtLongAlfa8.Text.Trim();
                longAlfa8 = (longAlfa8 != "") ? longAlfa8 : "25";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLMX2 set ";
                query += "FGPRPX = '" + activoPrefDoc + "', PGPRPX = '" + this.txtPgmValPrefDoc.Text.Trim() + "', ";
                query += "FGFAPX = '" + activoFactAmp + "', PGFAPX = '" + this.txtPgmValNumFactAmp.Text.Trim() + "', ";
                query += "FGFRPX = '" + activoFactRec + "', PGFRPX = '" + this.txtPgmValNumFactRectif.Text.Trim() + "', ";
                query += "FGDVPX = '" + activoFecSer + "', PGDVPX = '" + this.txtPgmValFechaServicio.Text.Trim() + "', ";
                query += "NM01PX = '" + this.txtAlfa1.Text.Trim() + "', MX01PX = " + longAlfa1 + ", ";
                query += "PG01PX = '" + this.txtPgmValAlfa1.Text.Trim() + "', TA01PX = '" + this.radButtonTextBoxSelAlfa1.Text.Trim() + "', ";
                query += "FG01PX = '" + activoAlfa1 + "', ";
                query += "NM02PX = '" + this.txtAlfa2.Text.Trim() + "', MX02PX = " + longAlfa2 + ", ";
                query += "PG02PX = '" + this.txtPgmValAlfa2.Text.Trim() + "', TA02PX = '" + this.radButtonTextBoxSelAlfa2.Text.Trim() + "', ";
                query += "FG02PX = '" + activoAlfa2 + "', ";
                query += "NM03PX = '" + this.txtAlfa3.Text.Trim() + "', MX03PX = " + longAlfa3 + ", ";
                query += "PG03PX = '" + this.txtPgmValAlfa3.Text.Trim() + "', TA03PX = '" + this.radButtonTextBoxSelAlfa3.Text.Trim() + "', ";
                query += "FG03PX = '" + activoAlfa3 + "', ";
                query += "NM04PX = '" + this.txtAlfa4.Text.Trim() + "', MX04PX = " + longAlfa4 + ", ";
                query += "PG04PX = '" + this.txtPgmValAlfa4.Text.Trim() + "', TA04PX = '" + this.radButtonTextBoxSelAlfa4.Text.Trim() + "', ";
                query += "FG04PX = '" + activoAlfa4 + "', ";
                query += "NM05PX = '" + this.txtAlfa5.Text.Trim() + "', MX05PX = " + longAlfa5 + ", ";
                query += "PG05PX = '" + this.txtPgmValAlfa5.Text.Trim() + "', TA05PX = '" + this.radButtonTextBoxSelAlfa5.Text.Trim() + "', ";
                query += "FG05PX = '" + activoAlfa5 + "', ";
                query += "NM06PX = '" + this.txtAlfa6.Text.Trim() + "', MX06PX = " + longAlfa6 + ", ";
                query += "PG06PX = '" + this.txtPgmValAlfa6.Text.Trim() + "', TA06PX = '" + this.radButtonTextBoxSelAlfa6.Text.Trim() + "', ";
                query += "FG06PX = '" + activoAlfa6 + "', ";
                query += "NM07PX = '" + this.txtAlfa7.Text.Trim() + "', MX07PX = " + longAlfa7 + ", ";
                query += "PG07PX = '" + this.txtPgmValAlfa7.Text.Trim() + "', TA07PX = '" + this.radButtonTextBoxSelAlfa7.Text.Trim() + "', ";
                query += "FG07PX = '" + activoAlfa7 + "', ";
                query += "NM08PX = '" + this.txtAlfa8.Text.Trim() + "', MX08PX = " + longAlfa8 + ", ";
                query += "PG08PX = '" + this.txtPgmValAlfa8.Text.Trim() + "', TA08PX = '" + this.radButtonTextBoxSelAlfa8.Text.Trim() + "', ";
                query += "FG08PX = '" + activoAlfa8 + "', ";
                query += "NM09PX = '" + this.txtNum1.Text.Trim() + "', PG09PX = '" + this.txtPgmValNum1.Text.Trim() + "', FG09PX = '" + activoNum1 + "', ";
                query += "NM10PX = '" + this.txtNum2.Text.Trim() + "', PG10PX = '" + this.txtPgmValNum2.Text.Trim() + "', FG10PX = '" + activoNum2 + "', ";
                query += "NM11PX = '" + this.txtFecha1.Text.Trim() + "', PG11PX = '" + this.txtPgmValFecha1.Text.Trim() + "', FG11PX = '" + activoFecha1 + "', ";
                query += "NM12PX = '" + this.txtFecha2.Text.Trim() + "', PG12PX = '" + this.txtPgmValFecha2.Text.Trim() + "', FG12PX = '" + activoFecha1 + "' ";
                query += "where TIPLPX = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex)); 

                result = "Error actualizando los datos de los campos extendidos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        private void ActualizarCuentaFormateada(string mascara, string estructura)
        {
            IDataReader dr = null;
            try
            {
                this.mascaraInicial = mascara;
                if (estructura.Length == 10) estructura = estructura.Substring(0, 9);

                string query = "select CUENMC, CEDTMC, TCUEMC, NIVEMC from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigo + "' ";
                query += "order by TIPLMC, CUENMC, NIVEMC";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string cuenta = "";
                string cuentaFormateada = "";
                int reg = 0;
                string error = "";
                int nivel = 0;

                string tipo = "";
                int nivemc = 0;
                int nivemcAnt = 0;
                bool primeraDetalle = false;

                while (dr.Read())
                {
                    cuenta = dr.GetValue(dr.GetOrdinal("CUENMC")).ToString().Trim();
                    tipo = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString().Trim();
                    nivemc = Convert.ToInt16(dr.GetValue(dr.GetOrdinal("NIVEMC")).ToString().Trim());
                    try
                    {
                        if (tipo == "T")
                        {
                            cuentaFormateada = utilesCG.CuentaFormatear(cuenta, this.codigo, estructura, mascara, ref error, ref nivel);
                            primeraDetalle = false;
                        }
                        else
                        {
                            if (!primeraDetalle)
                            {
                                cuentaFormateada = utilesCG.CuentaFormatear(cuenta, this.codigo, estructura, mascara, ref error, ref nivel);
                                nivemcAnt = nivemc;
                                primeraDetalle = true;
                            }
                            else
                            {
                                if (Convert.ToInt16(nivemc) <= nivemcAnt)
                                {
                                    cuentaFormateada = utilesCG.CuentaFormatear(cuenta, this.codigo, estructura, mascara, ref error, ref nivel);
                                    nivemcAnt = nivemc;
                                }
                            }
                        }

                        query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 set ";
                        query += "CEDTMC = '" + cuentaFormateada + "' where ";
                        query += "TIPLMC = '" + this.codigo + "' and ";
                        query += "CUENMC = '" + cuenta + "'";

                        reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Obtiene la estructura a partir de los 9 controles que almacenan la estructura
        /// </summary>
        /// <returns></returns>
        private string ObtenerEstructura()
        {
            string result = "";

            result  = this.txtEstNivel1.Text.Trim() == "" ? "0" : this.txtEstNivel1.Text;
            result += this.txtEstNivel2.Text.Trim() == "" ? "0" : this.txtEstNivel2.Text;
            result += this.txtEstNivel3.Text.Trim() == "" ? "0" : this.txtEstNivel3.Text;
            result += this.txtEstNivel4.Text.Trim() == "" ? "0" : this.txtEstNivel4.Text;
            result += this.txtEstNivel5.Text.Trim() == "" ? "0" : this.txtEstNivel5.Text;
            result += this.txtEstNivel6.Text.Trim() == "" ? "0" : this.txtEstNivel6.Text;
            result += this.txtEstNivel7.Text.Trim() == "" ? "0" : this.txtEstNivel7.Text;
            result += this.txtEstNivel8.Text.Trim() == "" ? "0" : this.txtEstNivel8.Text;
            result += this.txtEstNivel9.Text.Trim() == "" ? "0" : this.txtEstNivel9.Text;
            result += "0";

            return (result);
        }

        /// <summary>
        /// Obtiene la máscara a partir de los 8 controles que almacenan la máscara 
        /// </summary>
        /// <returns></returns>
        private string ObtenerMascara()
        {
            string result = "";

            result  = this.txtMascara1.Text.Trim() == "" ? " " : this.txtMascara1.Text;
            result += this.txtMascara2.Text.Trim() == "" ? " " : this.txtMascara2.Text;
            result += this.txtMascara3.Text.Trim() == "" ? " " : this.txtMascara3.Text;
            result += this.txtMascara4.Text.Trim() == "" ? " " : this.txtMascara4.Text;
            result += this.txtMascara5.Text.Trim() == "" ? " " : this.txtMascara5.Text;
            result += this.txtMascara6.Text.Trim() == "" ? " " : this.txtMascara6.Text;
            result += this.txtMascara7.Text.Trim() == "" ? " " : this.txtMascara7.Text;
            result += this.txtMascara8.Text.Trim() == "" ? " " : this.txtMascara8.Text;

            return (result);
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El código de plan está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActivo.Enabled = false;
            this.txtNombre.IsReadOnly = true;
            this.txtEstNivel1.IsReadOnly = true;
            this.txtEstNivel2.IsReadOnly = true;
            this.txtEstNivel3.IsReadOnly = true;
            this.txtEstNivel4.IsReadOnly = true;
            this.txtEstNivel5.IsReadOnly = true;
            this.txtEstNivel6.IsReadOnly = true;
            this.txtEstNivel7.IsReadOnly = true;
            this.txtEstNivel8.IsReadOnly = true;
            this.txtEstNivel9.IsReadOnly = true;
            this.txtMascara1.IsReadOnly = true;
            this.txtMascara2.IsReadOnly = true;
            this.txtMascara3.IsReadOnly = true;
            this.txtMascara4.IsReadOnly = true;
            this.txtMascara5.IsReadOnly = true;
            this.txtMascara6.IsReadOnly = true;
            this.txtMascara7.IsReadOnly = true;
            this.txtMascara8.IsReadOnly = true;
            this.radButtonTextBoxTipoMonedaExt.ReadOnly = true;
            this.radButtonElementTipoMonedaExt.Enabled = false;
            this.radToggleSwitchDigitoAutoverif.Enabled = false;

            //Campos extendidos
            this.txtPgmValPrefDoc.IsReadOnly = true;
            this.radToggleSwitchActivoPrefDoc.Enabled = false;
            this.txtPgmValNumFactAmp.IsReadOnly = true;
            this.radToggleSwitchActivoNumFactAmp.Enabled = false;
            this.txtPgmValNumFactRectif.IsReadOnly = true;
            this.radToggleSwitchActivoNumFactRectif.Enabled = false;
            this.txtPgmValFechaServicio.Enabled = false;
            this.radToggleSwitchFechaServicio.Enabled = false;
            
            this.txtAlfa1.IsReadOnly = true;
            this.txtLongAlfa1.IsReadOnly = true;
            this.txtPgmValAlfa1.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa1.ReadOnly = true;
            this.radButtonElementSelAlfa1.Enabled = false;
            this.radToggleSwitchAlfa1.Enabled = false;
            this.txtAlfa2.IsReadOnly = true;
            this.txtLongAlfa2.IsReadOnly = true;
            this.txtPgmValAlfa2.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa2.ReadOnly = true;
            this.radButtonElementSelAlfa2.Enabled = false;
            this.radToggleSwitchAlfa2.Enabled = false;
            this.txtAlfa3.IsReadOnly = true;
            this.txtLongAlfa3.IsReadOnly = true;
            this.txtPgmValAlfa3.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa3.ReadOnly = true;
            this.radButtonElementSelAlfa3.Enabled = false;
            this.radToggleSwitchAlfa3.Enabled = false;
            this.txtAlfa4.IsReadOnly = true;
            this.txtLongAlfa4.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa4.ReadOnly = true;
            this.radButtonElementSelAlfa4.Enabled = false;
            this.radToggleSwitchAlfa4.Enabled = false;
            this.txtAlfa5.IsReadOnly = true;
            this.txtLongAlfa5.IsReadOnly = true;
            this.txtPgmValAlfa5.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa5.ReadOnly = true;
            this.radButtonElementSelAlfa5.Enabled = false;
            this.radToggleSwitchAlfa5.Enabled = false;
            this.txtAlfa6.IsReadOnly = true;
            this.txtLongAlfa6.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa6.ReadOnly = true;
            this.radButtonElementSelAlfa6.Enabled = false;
            this.radToggleSwitchAlfa6.Enabled = false;
            this.txtAlfa7.IsReadOnly = true;
            this.txtLongAlfa7.IsReadOnly = true;
            this.txtPgmValAlfa7.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa7.ReadOnly = true;
            this.radButtonElementSelAlfa7.Enabled = false;
            this.radToggleSwitchAlfa7.Enabled = false;
            this.txtAlfa8.IsReadOnly = true;
            this.txtLongAlfa8.IsReadOnly = true;
            this.txtPgmValAlfa8.IsReadOnly = true;
            this.radButtonTextBoxSelAlfa8.ReadOnly = true;
            this.radButtonElementSelAlfa8.Enabled = false;
            this.radToggleSwitchAlfa8.Enabled = false;
            this.txtNum1.IsReadOnly = true;
            this.txtPgmValNum1.IsReadOnly = true;
            this.radToggleSwitchNum1.Enabled = false;
            this.txtNum2.IsReadOnly = true;
            this.txtPgmValNum2.IsReadOnly = true;
            this.radToggleSwitchNum2.Enabled = false;
            this.txtFecha1.IsReadOnly = true;
            this.txtPgmValFecha1.IsReadOnly = true;
            this.radToggleSwitchFecha1.Enabled = false;
            this.txtFecha2.IsReadOnly = true;
            this.txtPgmValFecha2.IsReadOnly = true;
            this.radToggleSwitchFecha2.Enabled = false;
        }

        /// <summary>
        /// Valida que no exista el código del plan
        /// </summary>
        /// <returns></returns>
        private bool CodigoPlanValido()
        {
            bool result = false;

            try
            {
                string codPlan = this.txtCodigo.Text.Trim();

                if (codPlan != "")
                {
                    string query = "select count(TIPLMP) from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                    query += "where TIPLMP = '" + codPlan + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Activa o no los controles de la estructura de la cuenta de mayor
        /// (en dependencia de si ya existen comprobantes o no para el plan contable)
        /// </summary>
        private void EditarEstructuraCtaMayor()
        {
            bool valor = false;

            try
            {
                //Chequear si existen cuentas asociadas a la estructura
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigo.Trim() + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) valor = true;
                
                this.txtEstNivel1.IsReadOnly = valor;
                this.txtEstNivel2.IsReadOnly = valor;
                this.txtEstNivel3.IsReadOnly = valor;
                this.txtEstNivel4.IsReadOnly = valor;
                this.txtEstNivel5.IsReadOnly = valor;
                this.txtEstNivel6.IsReadOnly = valor;
                this.txtEstNivel7.IsReadOnly = valor;
                this.txtEstNivel8.IsReadOnly = valor;
                this.txtEstNivel9.IsReadOnly = valor;

                this.radButtonTextBoxTipoMonedaExt.Enabled = !valor;
                this.radButtonElementTipoMonedaExt.Enabled = !valor;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba la info del plan de cuentas actual
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    this.codigo = this.txtCodigo.Text.Trim();
                    result = this.AltaInfo();
                    //if (result == "") this.nuevo = false;
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

        /*
        /// <summary>
        /// Opción campos extendidos
        /// </summary>
        private void CamposExtendidos()
        {
            if (this.visibleCamposExt)
            {
                this.radCollapsiblePanelCamposExtendidos.Visible = false;
                //this.Size = new System.Drawing.Size(873, 400);
                this.visibleCamposExt = false;
            }
            else
            {
                this.radCollapsiblePanelCamposExtendidos.Visible = true;
                //this.Size = new System.Drawing.Size(873, 578);
                this.visibleCamposExt = true;
            }
        }
        */

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtNombre.Tag = this.txtNombre.Text;

            this.txtEstNivel1.Tag = this.txtEstNivel1.Text;
            this.txtEstNivel2.Tag = this.txtEstNivel2.Text;
            this.txtEstNivel3.Tag = this.txtEstNivel3.Text;
            this.txtEstNivel4.Tag = this.txtEstNivel4.Text;
            this.txtEstNivel5.Tag = this.txtEstNivel5.Text;
            this.txtEstNivel6.Tag = this.txtEstNivel6.Text;
            this.txtEstNivel7.Tag = this.txtEstNivel7.Text;
            this.txtEstNivel8.Tag = this.txtEstNivel8.Text;
            this.txtEstNivel9.Tag = this.txtEstNivel9.Text;

            this.txtMascara1.Tag = this.txtMascara1.Text;
            this.txtMascara2.Tag = this.txtMascara2.Text;
            this.txtMascara3.Tag = this.txtMascara3.Text;
            this.txtMascara4.Tag = this.txtMascara4.Text;
            this.txtMascara5.Tag = this.txtMascara5.Text;
            this.txtMascara6.Tag = this.txtMascara6.Text;
            this.txtMascara7.Tag = this.txtMascara7.Text;
            this.txtMascara8.Tag = this.txtMascara8.Text;

            this.radButtonTextBoxTipoMonedaExt.Tag = this.radButtonTextBoxTipoMonedaExt.Text;

            this.radToggleSwitchDigitoAutoverif.Tag = this.radToggleSwitchDigitoAutoverif.Value;

            this.txtPgmValPrefDoc.Tag = this.txtPgmValPrefDoc.Text;
            this.txtPgmValNumFactAmp.Tag = this.txtPgmValNumFactAmp.Text;
            this.txtPgmValNumFactRectif.Tag = this.txtPgmValNumFactRectif.Text;
            this.txtPgmValFechaServicio.Tag = this.txtPgmValFechaServicio.Text;

            this.radToggleSwitchActivoPrefDoc.Tag = this.radToggleSwitchActivoPrefDoc.Value;
            this.radToggleSwitchActivoNumFactAmp.Tag = this.radToggleSwitchActivoNumFactAmp.Value;
            this.radToggleSwitchActivoNumFactRectif.Tag = this.radToggleSwitchActivoNumFactRectif.Value;
            this.radToggleSwitchFechaServicio.Tag = this.radToggleSwitchFechaServicio.Value;

            this.txtAlfa1.Tag = this.txtAlfa1.Text;
            this.txtAlfa2.Tag = this.txtAlfa2.Text;
            this.txtAlfa3.Tag = this.txtAlfa3.Text;
            this.txtAlfa4.Tag = this.txtAlfa4.Text;
            this.txtAlfa5.Tag = this.txtAlfa5.Text;
            this.txtAlfa6.Tag = this.txtAlfa6.Text;
            this.txtAlfa7.Tag = this.txtAlfa7.Text;
            this.txtAlfa8.Tag = this.txtAlfa8.Text;

            this.txtLongAlfa1.Tag = this.txtLongAlfa1.Text;
            this.txtLongAlfa2.Tag = this.txtLongAlfa2.Text;
            this.txtLongAlfa3.Tag = this.txtLongAlfa3.Text;
            this.txtLongAlfa4.Tag = this.txtLongAlfa4.Text;
            this.txtLongAlfa5.Tag = this.txtLongAlfa5.Text;
            this.txtLongAlfa6.Tag = this.txtLongAlfa6.Text;
            this.txtLongAlfa7.Tag = this.txtLongAlfa7.Text;
            this.txtLongAlfa8.Tag = this.txtLongAlfa8.Text;

            this.txtPgmValAlfa1.Tag = this.txtPgmValAlfa1.Text;
            this.txtPgmValAlfa2.Tag = this.txtPgmValAlfa2.Text;
            this.txtPgmValAlfa3.Tag = this.txtPgmValAlfa3.Text;
            this.txtPgmValAlfa4.Tag = this.txtPgmValAlfa4.Text;
            this.txtPgmValAlfa5.Tag = this.txtPgmValAlfa5.Text;
            this.txtPgmValAlfa6.Tag = this.txtPgmValAlfa6.Text;
            this.txtPgmValAlfa7.Tag = this.txtPgmValAlfa7.Text;
            this.txtPgmValAlfa8.Tag = this.txtPgmValAlfa8.Text;

            this.radButtonTextBoxSelAlfa1.Tag = this.radButtonTextBoxSelAlfa1.Text;
            this.radButtonTextBoxSelAlfa2.Tag = this.radButtonTextBoxSelAlfa2.Text;
            this.radButtonTextBoxSelAlfa3.Tag = this.radButtonTextBoxSelAlfa3.Text;
            this.radButtonTextBoxSelAlfa4.Tag = this.radButtonTextBoxSelAlfa4.Text;
            this.radButtonTextBoxSelAlfa5.Tag = this.radButtonTextBoxSelAlfa5.Text;
            this.radButtonTextBoxSelAlfa6.Tag = this.radButtonTextBoxSelAlfa6.Text;
            this.radButtonTextBoxSelAlfa7.Tag = this.radButtonTextBoxSelAlfa7.Text;
            this.radButtonTextBoxSelAlfa8.Tag = this.radButtonTextBoxSelAlfa8.Text;

            this.radToggleSwitchAlfa1.Tag = this.radToggleSwitchAlfa1.Value;
            this.radToggleSwitchAlfa2.Tag = this.radToggleSwitchAlfa2.Value;
            this.radToggleSwitchAlfa3.Tag = this.radToggleSwitchAlfa3.Value;
            this.radToggleSwitchAlfa4.Tag = this.radToggleSwitchAlfa4.Value;
            this.radToggleSwitchAlfa5.Tag = this.radToggleSwitchAlfa5.Value;
            this.radToggleSwitchAlfa6.Tag = this.radToggleSwitchAlfa6.Value;
            this.radToggleSwitchAlfa7.Tag = this.radToggleSwitchAlfa7.Value;
            this.radToggleSwitchAlfa8.Tag = this.radToggleSwitchAlfa8.Value;

            this.txtNum1.Tag = this.txtNum1.Text;
            this.txtNum2.Tag = this.txtNum2.Text;

            this.txtPgmValNum1.Tag = this.txtPgmValNum1.Text;
            this.txtPgmValNum2.Tag = this.txtPgmValNum2.Text;

            this.radToggleSwitchNum1.Tag = this.radToggleSwitchNum1.Value;
            this.radToggleSwitchNum2.Tag = this.radToggleSwitchNum2.Value;

            this.txtFecha1.Tag = this.txtFecha1.Text;
            this.txtFecha2.Tag = this.txtFecha2.Text;

            this.txtPgmValFecha1.Tag = this.txtPgmValFecha1.Text;
            this.txtPgmValFecha2.Tag = this.txtPgmValFecha2.Text;

            this.radToggleSwitchFecha1.Tag = this.radToggleSwitchFecha1.Value;
            this.radToggleSwitchFecha2.Tag = this.radToggleSwitchFecha2.Value;
        }

        /// <summary>
        /// Seleccionar tipo de auxiliar
        /// </summary>
        /// <param name="radButtonTextBoxTipoAux"></param>
        private void SeleccionarCamposExtendidosTipoAuxiliar(ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxTipoAux)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

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
                ColumnasCaption = nombreColumnas,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
                radButtonTextBoxTipoAux.Text = result;
            }
        }

        private void CentrarForm()
        {
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundHeight = Screen.PrimaryScreen.Bounds.Height;
            int x = boundWidth - this.Width;
            int y = boundHeight - this.Height;
            this.Location = new Point(x / 2, y / 2);
        }
        #endregion

        private void frmMtoGLM02_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
