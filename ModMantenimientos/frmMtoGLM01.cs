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
    public partial class frmMtoGLM01 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCIA";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "002";
        private const string autGrupo = "01";
        private const string autOperConsulta = "10";
        private const string autOperModifica = "20";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;
        public Boolean bGrabar = false;

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

        public frmMtoGLM01()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radButtonTextBoxPlanCuentas.AutoSize = false;
            this.radButtonTextBoxPlanCuentas.Size = new Size(this.radButtonTextBoxPlanCuentas.Width, 30);

            this.radButtonTextBoxCalendario.AutoSize = false;
            this.radButtonTextBoxCalendario.Size = new Size(this.radButtonTextBoxCalendario.Width, 30);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchImportesDecimales.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchImportesDecimales.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchSaldos.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchSaldos.ThemeName = "MaterialBlueGrey";

            this.radGroupBoxValFechaPeriodo.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxValFechaPeriodo.ThemeName = "ControlDefault";

            this.radMaskedEditBoxUltAnoCerrado.AutoSize = false;
            this.radMaskedEditBoxUltAnoCerrado.Size = new Size(this.radMaskedEditBoxUltAnoCerrado.Width, 30);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM01_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Compañías Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            this.BuildRadMultiColumnComboBoxPlanCuentas();

            if (this.nuevo)
            {
                this.autEditar = true;

                this.lblUltAnoPrdCerrado.Visible = false;
                this.radMaskedEditBoxUltAnoCerrado.Visible = false;

                /*this.lblUltAnoPrdCerrado.Visible = false;
                this.txtUltAnoCerrado.Visible = false;
                this.lblSepUltAnoPrdCerrado.Visible = false;
                this.txtUltPrdCerrado.Visible = false;
                */

                this.lblUltAnoPrdReorgMvtos.Visible = false;
                this.radMaskedEditBoxUltAnoReorgMvtos.Visible = false;

                this.lblUltAnoPrdReorgSaldos.Visible = false;
                this.radMaskedEditBoxUltAnoReorgSaldos.Visible = false;

                /*
                this.txtUltAnoReorgMvtos.Visible = false;
                this.lblSepUltAnoPrdReorgMvtos.Visible = false;
                this.txtUltPrdReorgMvtos.Visible = false;
                this.txtUltAnoReorgSaldos.Visible = false;
                this.lblSepUltAnoPrdReorgSaldos.Visible = false;
                this.txtUltPrdReorgSaldos.Visible = false;
                */

                this.radButtonTextBoxPlanCuentas.ReadOnly = false;
                this.radButtonElementPlanCuentas.Enabled = true;
                this.radButtonTextBoxCalendario.ReadOnly = false;
                this.radButtonElementradButtonCalendario.Enabled = true;

                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                this.lblUltAnoPrdCerrado.Visible = true;
                this.radMaskedEditBoxUltAnoCerrado.Visible = true;
                /*
                this.txtUltAnoCerrado.Visible = true;
                this.lblSepUltAnoPrdCerrado.Visible = true;
                this.txtUltPrdCerrado.Visible = true;
                */

                this.lblUltAnoPrdReorgMvtos.Visible = true;
                this.radMaskedEditBoxUltAnoReorgMvtos.Visible = true;

                this.lblUltAnoPrdReorgSaldos.Visible = true;
                this.radMaskedEditBoxUltAnoReorgSaldos.Visible = true;

                /*
                this.lblUltAnoPrdReorgSaldos.Visible = true;
                this.txtUltAnoReorgMvtos.Visible = true;
                this.lblSepUltAnoPrdReorgMvtos.Visible = true;
                this.txtUltPrdReorgMvtos.Visible = true;
                this.txtUltAnoReorgSaldos.Visible = true;
                this.lblSepUltAnoPrdReorgSaldos.Visible = true;
                this.txtUltPrdReorgSaldos.Visible = true;
                */

                //Verificar si existen comprobantes contables o extracontables para la compañía
                if (this.ExistenComprobantes())
                {
                    this.radButtonTextBoxPlanCuentas.ReadOnly = true;
                    this.radButtonElementPlanCuentas.Enabled = false;
                    this.radButtonTextBoxCalendario.ReadOnly = true;
                    this.radButtonElementradButtonCalendario.Enabled = false;
                }
                else
                {
                    this.radButtonTextBoxPlanCuentas.ReadOnly = false;
                    this.radButtonElementPlanCuentas.Enabled = true;
                    this.radButtonTextBoxCalendario.ReadOnly = false;
                    this.radButtonElementradButtonCalendario.Enabled = true;
                }

                //Recuperar la información de la compañía y mostrarla en los controles
                this.CargarInfoCompania();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }
            }
        }

        private void TxtCif_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codCompania = this.txtCodigo.Text.Trim();

                if (codCompania == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Código de compañía obligatorio", this.LP.GetText("errValCodCompania", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codCompaniaOk = true;
                if (this.nuevo) codCompaniaOk = this.CodigoCompaniaValido();    //Verificar que el codigo no exista

                if (codCompaniaOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                    }
                    this.txtCodigo.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de compañía ya existe", this.LP.GetText("errValCodCompaniaExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        private void TxtPrdCierreEjerc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sólo caracteres numéricos
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonElementradButtonCalendario_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select distinct(TITAFL) as codigo from ";
            query += GlobalVar.PrefijoTablaCG + "GLT04 ";
            query += "order by TITAFL";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasCalendario = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar calendarios",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasCalendario,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            int cantidadColumnasResult = 1;
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
                this.radButtonTextBoxCalendario.Text = result;
                this.ActiveControl = this.radButtonTextBoxCalendario;
                this.radButtonTextBoxCalendario.Select(0, 0);
                this.radButtonTextBoxCalendario.Focus();
                /*
                    //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar y cerrar el formulario .
                    //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                    if (ValueChanged != null)
                        ValueChanged(new ValueChangedCommandEventArgs(GlobalVar.ElementosSel));*/

            }
        }

        private void RadButtonElementPlanCuentas_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIPLMP, NOMBMP from ";
            query += GlobalVar.PrefijoTablaCG + "GLM02 ";
            query += "order by TIPLMP";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar plan de cuentas",
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
                this.radButtonTextBoxPlanCuentas.Text = result;
                this.ActiveControl = this.radButtonTextBoxPlanCuentas;
                this.radButtonTextBoxPlanCuentas.Select(0, 0);
                this.radButtonTextBoxPlanCuentas.Focus();
            }
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

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadButtonTextBoxCalendario_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadButtonTextBoxPlanCuentas_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void FrmMtoGLM01_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtCif.Text != this.txtCif.Tag.ToString() ||
                    this.radToggleSwitchImportesDecimales.Value != (bool)(this.radToggleSwitchImportesDecimales.Tag) ||
                    this.radButtonTextBoxPlanCuentas.Text != this.radButtonTextBoxPlanCuentas.Tag.ToString() ||
                    this.radButtonTextBoxCalendario.Text != this.radButtonTextBoxCalendario.Tag.ToString() ||
                    this.txtPrdCierreEjerc.Text != this.txtPrdCierreEjerc.Tag.ToString() ||
                    (this.rbValFechaPerTer.IsChecked && !(Convert.ToBoolean(this.rbValFechaPerTer.Tag))) ||
                    (!this.rbValFechaPerTer.IsChecked && (Convert.ToBoolean(this.rbValFechaPerTer.Tag))) ||
                    this.radToggleSwitchSaldos.Value != (bool)(this.radToggleSwitchSaldos.Tag)
                )
                {
                    if (bGrabar == true) return;  //jl

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
                    // else e.Cancel = false;  jl
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Compañías Alta/Edita");
        }

        private void FrmMtoGLM01_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
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
                if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM01TituloALta", "Mantenimiento de Compañías - Alta");   //Falta traducir
                else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM01TituloEdit", "Mantenimiento de Compañías - Edición");           //Falta traducir

                //Traducir los Literales de los ToolStrip
                this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Guardar");
                this.radButtonExit.Text = this.LP.GetText("toolStripSalir", "Cancelar");

                //Traducir los campos del formulario
                this.lblCodigo.Text = this.LP.GetText("lblGLM01Compania", "Compañía");
                this.lblEstado.Text = "Activa";     //Falta traducir
                this.lblNombre.Text = this.LP.GetText("lblNombre", "Nombre");
                this.lblCif.Text = this.LP.GetText("lblGLM01Cif", "Número Identificación Tributaria");
                this.lblImporteDecimales.Text = this.LP.GetText("lblImpDec", "Importes con decimales");
                this.lblPlan.Text = this.LP.GetText("lblGLM01Plan", "Plan de cuentas");
                this.lblCalendario.Text = this.LP.GetText("lblGLM01Calendario", "Calendario Contable");
                this.lblPrdCierre.Text = this.LP.GetText("lblGLM01PrdCierreEjerc", "Periodo de cierre de ejercicio");
                this.lblValFechaPrd.Text = this.LP.GetText("lblGLM01ValFechaPrd", "Validación fecha/periodo");
                this.rbValFechaPerTer.Text = this.LP.GetText("lblGLM01Terminate", "Terminate"); //TRADUCIr!!
                this.rbValFechaPerTer.Text = "Terminante"; 
                this.rbValFechaPerInf.Text = this.LP.GetText("lblGLM01Informativo", "Informativo");
                this.lblSaldos.Text = this.LP.GetText("lblGLM01SaldosUltNiv", "Saldos a último nivel");
                this.lblUltAnoPrdCerrado.Text = this.LP.GetText("lblGLM01UltAnPrdCerrado", "Último año/periodo cerrado");
                //this.lblUltAnoPrdReorgSaldos.Text = this.LP.GetText("lblGLM01UltAnPrdReorg", "Último año/periodo reorganizado (mvtos/saldos)");

                this.lblUltAnoPrdReorgMvtos.Text = "Último año/periodo reorganizado movimientos";      //Falta traducir
                this.lblUltAnoPrdReorgSaldos.Text = "Último año/periodo reorganizado saldos";      //Falta traducir

                //Traducir estado   FALTA
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compañía (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.txtCif.Enabled = valor;
            this.radToggleSwitchImportesDecimales.Enabled = valor;
            this.radButtonTextBoxPlanCuentas.Enabled = valor;
            this.radButtonElementPlanCuentas.Enabled = valor;
            this.radButtonTextBoxCalendario.Enabled = valor;
            this.radButtonElementradButtonCalendario.Enabled = valor;
            this.txtPrdCierreEjerc.Enabled = valor;
            this.rbValFechaPerInf.Enabled = valor;
            this.rbValFechaPerTer.Enabled = valor;
            this.radToggleSwitchSaldos.Enabled = valor;
        }

        /// <summary>
        /// Verifica si existen comprobantes contables o extracontables de la compañía
        /// </summary>
        /// <returns></returns>
        private bool ExistenComprobantes()
        {
            bool result = false;
            IDataReader dr = null;

            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "where CCIAIC = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = true;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = true;
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Rellena los controles con los datos de la compañía (modo edición)
        /// </summary>
        private void CargarInfoCompania()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;

                string importesDecimales = "";
                string validacionFechaPrd = "";
                string saldosUltNivel = "";
                string ultAnoPrdCerrado = "";
                string ultAnoPrdReorgMvtos = "";
                string ultAnoPrdReorgSaldos = "";

                string plan = "";
                string planDesc = "";

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NCIAMG")).ToString().Trim();

                    string estado = dr.GetValue(dr.GetOrdinal("STATMG")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;

                    this.txtCif.Text = dr.GetValue(dr.GetOrdinal("NNITMG")).ToString().Trim();
                    importesDecimales = dr.GetValue(dr.GetOrdinal("MASCMG")).ToString().Trim();
                    if (importesDecimales == "N") this.radToggleSwitchImportesDecimales.Value = false;
                    else this.radToggleSwitchImportesDecimales.Value = true;

                    validacionFechaPrd = dr.GetValue(dr.GetOrdinal("FELAMG")).ToString().Trim();
                    if (validacionFechaPrd == "T")
                    {
                        this.rbValFechaPerTer.IsChecked = true;
                        this.rbValFechaPerInf.IsChecked = false;
                    }
                    else
                    {
                        this.rbValFechaPerInf.IsChecked = true;
                        this.rbValFechaPerTer.IsChecked = false;
                    }

                    plan = dr.GetValue(dr.GetOrdinal("TIPLMG")).ToString().Trim();
                    if (plan != "")
                    {
                        planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", plan, false, "").Trim();
                        if (planDesc != "") plan = plan + " " + separadorDesc + " " + planDesc;
                    }
                    this.radButtonTextBoxPlanCuentas.Text = plan;

                    this.radButtonTextBoxCalendario.Text = dr.GetValue(dr.GetOrdinal("TITAMG")).ToString().Trim();
                    this.txtPrdCierreEjerc.Text = dr.GetValue(dr.GetOrdinal("LACIMG")).ToString().Trim();

                    saldosUltNivel = dr.GetValue(dr.GetOrdinal("MSLDMG")).ToString().Trim();
                    if (saldosUltNivel == "1") this.radToggleSwitchSaldos.Value = true;
                    else this.radToggleSwitchSaldos.Value = false;

                    ultAnoPrdCerrado = dr.GetValue(dr.GetOrdinal("ULACMG")).ToString().Trim();
                    if (ultAnoPrdCerrado != "0")
                    {
                        if (ultAnoPrdCerrado.Length == 4) ultAnoPrdCerrado = "0" + ultAnoPrdCerrado;
                        else if (ultAnoPrdCerrado.Length < 4) ultAnoPrdCerrado = ultAnoPrdCerrado.PadRight(5, ' ');

                        //this.txtUltAnoCerrado.Text = ultAnoPrdCerrado.Substring(1, 2);
                        //this.txtUltPrdCerrado.Text = ultAnoPrdCerrado.Substring(3, 2);

                        this.radMaskedEditBoxUltAnoCerrado.Text = ultAnoPrdCerrado.Substring(1, 2) + "/" + ultAnoPrdCerrado.Substring(3, 2);
                    }
                    ultAnoPrdReorgMvtos = dr.GetValue(dr.GetOrdinal("ULREMG")).ToString().Trim();
                    if (ultAnoPrdReorgMvtos != "0")
                    {
                        if (ultAnoPrdReorgMvtos.Length == 4) ultAnoPrdReorgMvtos = "0" + ultAnoPrdReorgMvtos;
                        else if (ultAnoPrdReorgMvtos.Length < 4) ultAnoPrdReorgMvtos = ultAnoPrdReorgMvtos.PadRight(5, ' ');
                        this.radMaskedEditBoxUltAnoReorgMvtos.Text = ultAnoPrdReorgMvtos.Substring(1, 2) + "/" + ultAnoPrdReorgMvtos.Substring(3, 2); ;

                    }
                    ultAnoPrdReorgSaldos = dr.GetValue(dr.GetOrdinal("USREMG")).ToString().Trim();
                    if (ultAnoPrdReorgSaldos != "0")
                    {
                        if (ultAnoPrdReorgSaldos.Length == 4) ultAnoPrdReorgSaldos = "0" + ultAnoPrdReorgSaldos;
                        else if (ultAnoPrdReorgSaldos.Length < 4) ultAnoPrdReorgSaldos = ultAnoPrdReorgSaldos.PadRight(5, ' ');
                        this.radMaskedEditBoxUltAnoReorgSaldos.Text = ultAnoPrdReorgSaldos.Substring(1, 2) + "/" + ultAnoPrdReorgSaldos.Substring(3, 2);
                    }
                }

                dr.Close();

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

                if (!this.radButtonTextBoxPlanCuentas.ReadOnly)
                {
                    if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
                    {
                        errores += "- El plan de cuentas no puede estar en blanco \n\r";     //Falta traducir
                        this.radButtonTextBoxPlanCuentas.Focus();
                    }
                    else
                    {
                        string resultValidarCodPlan = this.ValidarCodigoPlan();
                        if (resultValidarCodPlan != "")
                        {
                            errores += "- " + resultValidarCodPlan + "\n\r";
                            this.radButtonTextBoxPlanCuentas.Focus();
                        }
                    }
                }

                if (!this.radButtonTextBoxCalendario.ReadOnly)
                {
                    if (this.radButtonTextBoxCalendario.Text.Trim() == "")
                    {
                        errores += "- El calendario contable no puede estar en blanco \n\r";     //Falta traducir
                        this.radButtonTextBoxCalendario.Focus();
                    }
                    else
                    {
                        string resultValidarCodCalendario = this.ValidarCodigoCalendario();
                        if (resultValidarCodCalendario != "")
                        {
                            errores += "- " + resultValidarCodCalendario + "\n\r";
                            this.radButtonTextBoxCalendario.Focus();
                        }
                    }
                }

                int prdCierreInt = 0;
                string prdCierre = this.txtPrdCierreEjerc.Text.Trim();
                if (prdCierre == "")
                {
                    errores += "- El periodo de cierre de ejercicio no puede estar en blanco \n\r";   //Falta traducir
                    this.txtPrdCierreEjerc.Focus();
                }
                else
                {
                    try
                    {
                        prdCierreInt = Convert.ToInt16(this.txtPrdCierreEjerc.Text);
                        if (prdCierreInt == 0)
                        {
                            errores += "- El periodo de cierre de ejercicio no puede ser 0 \n\r";   //Falta traducir
                            this.txtPrdCierreEjerc.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        if (prdCierreInt == 0)
                        {
                            errores += "- El periodo de cierre de ejercicio no es válido \n\r";   //Falta traducir
                            this.txtPrdCierreEjerc.Focus();
                        }
                    }
                }

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
        /// Valida que el código del plan contable exista en la tabla GLM02
        /// </summary>
        private string ValidarCodigoPlan()
        {
            string result = "";
            try
            {
                string codPlan = this.radButtonTextBoxPlanCuentas.Text.Trim();
                if (codPlan.Length > 1) codPlan = codPlan.Substring(0, 1);

                if (codPlan != "")
                {
                    string query = "select count(TIPLMP) from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                    query += "where TIPLMP = '" + codPlan + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = "Plan de cuentas no existe";   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el plan de cuentas (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida que el código del calendario exista en la tabla GLM02
        /// </summary>
        private string ValidarCodigoCalendario()
        {
            string result = "";
            try
            {
                string codCalendario = this.radButtonTextBoxCalendario.Text.Trim();
                if (codCalendario.Length > 1) codCalendario = codCalendario.Substring(0, 1);

                if (codCalendario != "")
                {
                    string query = "select count(TITAFL) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + codCalendario + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = "Calendario contable no existe";   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el calendario contable (" + ex.Message + ")";   //Falta traducir
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
                string cif = this.txtCif.Text.Trim();
                cif = (cif == "") ? " " : cif;
                string importesDecimales = this.radToggleSwitchImportesDecimales.Value ? "S" : "N";
                string validacionFechaPrd = this.rbValFechaPerTer.IsChecked ? "T" : "W";
                string saldosUltNivel = this.radToggleSwitchSaldos.Value ? "1" : "0";
                string ultAnoPrdCerrado = "0";
                string ultAnoPrdReorgMvtos = "0";
                string ultAnoPrdReorgSaldos = "0";

                string plan = this.radButtonTextBoxPlanCuentas.Text.Trim();
                int posSeparador = plan.IndexOf(separadorDesc);
                if (posSeparador != -1) plan = plan.Substring(0, posSeparador - 1).Trim();

                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                //Dar de alta a la compañía en la tabla del maestro de compañías (GLM01)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM01";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATMG, CCIAMG, NCIAMG, ULACMG, LACIMG, MASCMG, FELAMG, TIPLMG, TITAMG, SKIPMG, ULREMG, NNITMG, ";
                query += "MSLDMG, REHAMG, DLTCMG, USREMG, RSHAMG, XTRAMG) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtNombre.Text + "', " + ultAnoPrdCerrado;
                query += ", " + this.txtPrdCierreEjerc.Text + ", '" + importesDecimales + "', '" + validacionFechaPrd + "', '";
                query += plan + "', '" + this.radButtonTextBoxCalendario.Text + "', 'N', ";
                query += ultAnoPrdReorgMvtos + ", '" + cif + "', " + saldosUltNivel + ", 0, 0, ";
                query += ultAnoPrdReorgSaldos + ", 0, 0)";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM01", this.codigo, null);

                //Dar de alta a la compañía en la tabla de compañías por grupo(GLM08)
                nombreTabla = GlobalVar.PrefijoTablaCG + "GLM08";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "GRUPAI, CCIAAI, ORIGAI) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "' ', '" + this.codigo + "', ' ')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Insertar al usuario como propietario del elemento
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + this.codigo + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
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
                string cif = this.txtCif.Text.Trim();
                cif = (cif == "") ? " " : cif;
                string importesDecimales = this.radToggleSwitchImportesDecimales.Value ? "S" : "N";
                string validacionFechaPrd = this.rbValFechaPerTer.IsChecked ? "T" : "W";
                string saldosUltNivel = this.radToggleSwitchSaldos.Value ? "1" : "0";

                string plan = this.radButtonTextBoxPlanCuentas.Text.Trim();
                int posSeparador = plan.IndexOf(separadorDesc);
                if (posSeparador != -1) plan = plan.Substring(0, posSeparador - 1).Trim();

                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM01 set ";
                query += "STATMG = '" + estado + "', NCIAMG = '" + this.txtNombre.Text + "', ";
                query += "LACIMG = " + this.txtPrdCierreEjerc.Text + ", MASCMG = '" + importesDecimales + "', ";
                query += "FELAMG = '" + validacionFechaPrd + "', TIPLMG = '" + plan + "', ";
                query += "TITAMG = '" + this.radButtonTextBoxCalendario.Text + "', NNITMG = '" + cif + "', ";
                query += "MSLDMG = " + saldosUltNivel + " ";
                query += "where CCIAMG = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM01", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La compañía está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNombre.IsReadOnly = true;
            this.txtCif.IsReadOnly = true;
            this.radToggleSwitchImportesDecimales.Enabled = false;
            this.txtPrdCierreEjerc.IsReadOnly = false;
            this.rbValFechaPerInf.Enabled = false;
            this.rbValFechaPerTer.Enabled = false;
            this.radToggleSwitchSaldos.Enabled = false;
        }

        /// <summary>
        /// Valida que no exista el código de la compañía
        /// </summary>
        /// <returns></returns>
        private bool CodigoCompaniaValido()
        {
            bool result = false;

            try
            {
                string codCompania = this.txtCodigo.Text.Trim();

                if (codCompania != "")
                {
                    string query = "select count(CCIAMG) from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                    query += "where CCIAMG = '" + codCompania + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Graba la info de la compañía actual
        /// </summary>
        private void Grabar()
        {
            bGrabar = false;
            Cursor.Current = Cursors.WaitCursor;
            
            if (this.FormValid())
            {
                Cursor.Current = Cursors.WaitCursor;

                string mensaje = this.LP.GetText("wrngrabarCodIVA", "Se va a actualizar la Compañia") + " " + this.codigo.Trim();
                mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                DialogResult yesno = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

                if (yesno == DialogResult.No)
                {
                    //Cerrar el formulario
                    bGrabar = true;
                    this.Close();
                }

                if (yesno == DialogResult.Yes)
                {
                    string result = "";
                    bGrabar = true;

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
                bGrabar = false;
            }
        }
            /// <summary>
            /// Actualiza el atributo TAG de los controles al valor actual de los controles
            /// </summary>
            private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtCif.Tag = this.txtCif.Text;

            if (this.radToggleSwitchImportesDecimales.Value) this.radToggleSwitchImportesDecimales.Tag = true;
            else this.radToggleSwitchImportesDecimales.Tag = false;

            this.radButtonTextBoxPlanCuentas.Tag = this.radButtonTextBoxPlanCuentas.Text;
            this.radButtonTextBoxCalendario.Tag = this.radButtonTextBoxCalendario.Text;

            this.txtPrdCierreEjerc.Tag = this.txtPrdCierreEjerc.Text;

            if (this.rbValFechaPerTer.IsChecked) this.rbValFechaPerTer.Tag = true;
            else this.rbValFechaPerTer.Tag = false;

            if (this.radToggleSwitchSaldos.Value) this.radToggleSwitchSaldos.Tag = true;
            else this.radToggleSwitchSaldos.Tag = false;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;
            this.txtNombre.Tag = "";
            this.txtCif.Tag = "";
            this.radToggleSwitchImportesDecimales.Tag = true;
            this.radButtonTextBoxPlanCuentas.Tag = "";
            this.radButtonTextBoxCalendario.Tag = "";
            this.txtPrdCierreEjerc.Tag = "";
            this.rbValFechaPerTer.Tag = true;
            this.radToggleSwitchSaldos.Tag = true;
        }

        private void BuildRadMultiColumnComboBoxPlanCuentas()
        {
            string query = "select TIPLMP, NOMBMP from ";
            query += GlobalVar.PrefijoTablaCG + "GLM02 ";
            query += "order by NOMBMP";

            DataTable dt = new DataTable();
            dt = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
        }
        #endregion

        private void frmMtoGLM01_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void txtCodigo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.txtNombre.Text.Trim() == "")
            {
                RadMessageBox.Show("- El nombre no puede estar en blanco");
                bTabulador = false;
                this.txtNombre.Focus();
            }
            bTabulador = false;
            Cursor.Current = Cursors.Default;
        }

        private void txtNombre_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxCalendario_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;
            if (!this.radButtonTextBoxCalendario.ReadOnly)
            {
                if (this.radButtonTextBoxCalendario.Text.Trim() == "")
                {
                    RadMessageBox.Show("- El calendario contable no puede estar en blanco \n\r");
                    bTabulador = false;
                    this.radButtonTextBoxCalendario.Focus();
                    bTabulador = false;
                }
                else
                {
                    string resultValidarCodCalendario = this.ValidarCodigoCalendario();
                    if (resultValidarCodCalendario != "")
                    {
                        RadMessageBox.Show("-"  + resultValidarCodCalendario + "\n\r");
                        this.radButtonTextBoxCalendario.Focus();
                        bTabulador = false;
                    }
                }
            }
            bTabulador = false;
            Cursor.Current = Cursors.Default;
        }

        private void radButtonTextBoxCalendario_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxPlanCuentas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bTabulador = false;
            if (e.KeyData == (Keys.Tab)) bTabulador = true;
            if (e.KeyData == (Keys.Tab | Keys.Shift)) bTabulador = true;
        }

        private void radButtonTextBoxPlanCuentas_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return; 
            if (!this.radButtonTextBoxPlanCuentas.ReadOnly)
            {
                if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
                {
                    RadMessageBox.Show("- El plan de cuentas no puede estar en blanco \n\r");
                    this.radButtonTextBoxPlanCuentas.Focus();
                    bTabulador = false;
                }
                else
                {
                    string resultValidarCodPlan = this.ValidarCodigoPlan();
                    if (resultValidarCodPlan != "")
                    {
                        RadMessageBox.Show("-" + resultValidarCodPlan + "\n\r");
                        this.radButtonTextBoxPlanCuentas.Focus();
                        bTabulador = false;
                    }
                    else
                    {
                        string codPlan = this.radButtonTextBoxPlanCuentas.Text.Trim();
                        if (codPlan.Length > 1) codPlan = codPlan.Substring(0, 1);
                        if (codPlan!="")
                        {
                            string planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", codPlan, false, "").Trim();
                            if (planDesc != "") this.radButtonTextBoxPlanCuentas.Text = codPlan + " " + separadorDesc + " " + planDesc;
                        }

                    }
                }
            }
            bTabulador = false;
            Cursor.Current = Cursors.Default;
        }
    }
}
