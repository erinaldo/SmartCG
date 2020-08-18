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
    public partial class frmMtoGLM03Titulo : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCTAMAT";

        private bool nuevo;
        private bool copiar;
        private string codigo;

        private string nombreCuenta;
        private string codigoPlan;
        private string nombrePlan;
        private string tipoCuenta;
        private string codigoCuentaCopiar;
        private string estadoCuenta;
        private bool planActivo;

        private string autClaseElemento = "003";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        //private string autOperAlta = "20";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        private DataTable dtEstructuraCuenta;
        
        private string[] estructuraPadreCuentas;
        private int[] datosPlan;

        //Campos de la tabla GLM03 que se insertará o se actualizará
        string TIPLMC = " ";
        string CUENMC = " ";
        string TCUEMC = " ";
        string NOABMC = " ";
        string NIVEMC = " ";
        string CIERMC = " ";
        string ADICMC = " ";
        string SASIMC = " ";
        string SCONMC = " ";
        string FEVEMC = " ";
        string NDDOMC = " ";
        string TERMMC = " ";
        string TIMOMC = " ";
        string TAU1MC = " ";
        string TAU2MC = " ";
        string TAU3MC = " ";
        string TDOCMC = " ";
        string GRUPMC = " ";
        string DEAUMC = " ";
        string NOLAAD = " ";
        string RNITMC = " ";
        string CNITMC = " ";
        string MASCMC = " ";
        string CEDTMC = " ";
        string FCRTMC = " ";
        
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

        public string NombreCuenta
        {
            get
            {
                return (this.nombreCuenta);
            }
            set
            {
                this.nombreCuenta = value;
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

        public string TipoCuenta
        {
            get
            {
                return (this.tipoCuenta);
            }
            set
            {
                this.tipoCuenta = value;
            }
        }

        public string CodigoCuentaCopiar
        {
            get
            {
                return (this.codigoCuentaCopiar);
            }
            set
            {
                this.codigoCuentaCopiar = value;
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

        public frmMtoGLM03Titulo()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM03Titulo_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Mayor de Titulo Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();
            
            //Ajustar el control de selección del plan
            this.radTextBoxControlPlan.Text = this.nombrePlan;

            this.txtTipo.Text = this.tipoCuenta;
            this.txtTipo.IsReadOnly = true;
            
            if (this.nuevo)
            {
                //------------ NUEVO --------------
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);

                utiles.ButtonEnabled(ref this.radButtonVerNivelesCta, false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);

                this.estadoCuenta = "V";

                //Cargar la estructura del plan de cuentas
                this.datosPlan = utilesCG.ObtenerNivelLongitudesDadoPlan(this.codigoPlan);

                this.ActiveControl = this.txtCuentaMayor;
                this.txtNombre.Select(0, 0);
                this.txtNombre.Focus();

                //FALTA al grabar llamar a cargar la estructura de la cuenta
            }
            else
            {
                if (this.copiar)
                {
                    //------------ COPIAR --------------
                    this.HabilitarDeshabilitarControles(false);
                    utiles.ButtonEnabled(ref this.radButtonSave, false);
                    utiles.ButtonEnabled(ref this.radButtonDelete, false);

                    //Cargar la estructura del plan de cuentas
                    this.datosPlan = utilesCG.ObtenerNivelLongitudesDadoPlan(this.codigoPlan);

                    //Mostrar la información de la cuenta en los controles
                    this.CargarInfoCuenta(this.codigoCuentaCopiar);

                    this.ActiveControl = this.txtCuentaMayor;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }
                else
                {
                    //------------ EDITAR --------------
                    this.txtCuentaMayor.Text = this.codigo.Trim();
                    this.txtCuentaMayor.IsReadOnly = true;
                    
                    //Mostrar la información de la cuenta en los controles
                    this.CargarInfoCuenta(this.codigo);

                    bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigoPlan, autOperModifica);
                    this.autEditar = operarModificar;
                    if (!operarModificar) this.NoEditarCampos();
                    else
                    {
                        this.ActiveControl = this.txtNombre;
                        this.txtNombre.Select(0, 0);
                        this.txtNombre.Focus();
                    }

                    bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                    if (!operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, false);
                    else
                    {
                        //Comprobar si la cuenta de mayor tiene hijos
                        bool cuentaTieneHijos = this.CuentaTieneHijos();
                        if (cuentaTieneHijos) utiles.ButtonEnabled(ref this.radButtonDelete, false);
                        else utiles.ButtonEnabled(ref this.radButtonDelete, true);
                    }
                }

                //Cargar la estructura de la cuenta
                this.CargarEstructuraCuenta(this.codigo.Trim(), this.codigoPlan);
            }
        }
        
        private void TxtCuentaMayor_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCuentaMayor_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar || this.copiar)
            {
                string codCuentaMayor = this.txtCuentaMayor.Text.Trim();

                if (codCuentaMayor == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCuentaMayor.Text = "";
                    this.txtCuentaMayor.Focus();

                    RadMessageBox.Show("Código de cuenta mayor obligatorio", this.LP.GetText("errValCodCtaMayor", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                if (!(System.Text.RegularExpressions.Regex.IsMatch(codCuentaMayor, "^[A-Z0-9]")))
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCuentaMayor.Focus();

                    RadMessageBox.Show("El rango de caracteres que puede contener la cuenta es: A-Z 0-9", this.LP.GetText("errValCodCtaMayorCaracteres", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                if(datosPlan[1] == this.txtCuentaMayor.Text.ToString().Length)
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCuentaMayor.Focus();

                    RadMessageBox.Show("Una cuenta de último nivel no puede ser de título", this.LP.GetText("errValCodCtaMayorUltNivelTitulo", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codCuentaMayorOk = true;
                string error = "";
                if (this.nuevo || this.copiar) codCuentaMayorOk = this.CodigoCuentaMayorValido(ref error);    //Verificar que el codigo no exista

                if (codCuentaMayorOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                    }
                    this.txtCuentaMayor.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);

                    this.codigo = this.txtCuentaMayor.Text;
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCuentaMayor.Focus();
                    RadMessageBox.Show(error, this.LP.GetText("errValCodCuentaAuxExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                    this.codigo = null;
                }
            }
            bTabulador = false;
        }
        
        private void FrmMtoGLM03Titulo_KeyDown(object sender, KeyEventArgs e)
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

        private void RadButtonVerNivelesCta_Click(object sender, EventArgs e)
        {
            this.VerNivelesCuenta();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void RadButtonVerNivelesCta_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonVerNivelesCta_MouseLeave(object sender, EventArgs e)
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

        private void FrmMtoGLM03Titulo_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtCuentaMayor.Text.Trim() != this.txtCuentaMayor.Tag.ToString() ||
                    this.txtCuentaMayor.Text.Trim() != this.txtCuentaMayor.Tag.ToString().Trim() ||
                    this.txtTipo.Text.Trim() != this.txtTipo.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtNombreExt.Text.Trim() != this.txtNombreExt.Tag.ToString().Trim()
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Cuentas de Mayor de Titulo Alta/Edit");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = "   " + this.LP.GetText("lblfrmMtoGLM03Titulo", "Mantenimiento de Cuentas de Mayor");   //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonVerNivelesCta.Text = this.LP.GetText("lblVerNivelesCta", "Ver niveles cuenta");
            this.radButtonExit.Text = this.LP.GetText("toolStripSalir", "Cancelar");
            
            //Traducir los campos del formulario
            this.lblPlan.Text = this.LP.GetText("lblGLM03Plan", "Plan");
        }
        
        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la zona (al dar de alta una zona)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.txtNombreExt.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La zona está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);
        
            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtCuentaMayor.Enabled = false;
            this.txtTipo.Enabled = false;
            this.txtNombre.Enabled = false;
            this.txtNombreExt.Enabled = false;
        }

        /// <summary>
        /// Rellena los controles con los datos de la cuenta de mayor (modo edición y modo copiar)
        /// </summary>
        /// <param name="codigoCuentaCargar"></param>
        private void CargarInfoCuenta(string codigoCuentaCargar)
        {
            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC='" + this.codigoPlan + "' and CUENMC = '" + codigoCuentaCargar + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();
                    this.txtNombreExt.Text = dr.GetValue(dr.GetOrdinal("NOLAAD")).ToString().Trim();

                    this.estadoCuenta = dr.GetValue(dr.GetOrdinal("STATMC")).ToString().Trim();
                    if (this.estadoCuenta == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;
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

            if (!this.planActivo) this.NoEditarCampos();
        }

        /// <summary>
        /// Valida que el código de la cuenta de mayor sea correcto
        /// </summary>
        /// <returns></returns>
        private bool CodigoCuentaMayorValido(ref string error)
        {
            bool result = false;
            IDataReader dr = null;

            try
            {
                string codCuentaMayor = this.txtCuentaMayor.Text.Trim();

                if (codCuentaMayor != "")
                {
                    //Validar que el código no exista
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + codCuentaMayor + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros != 0)
                    {
                        error = "Código de Cuenta de Mayor ya existe";  //Falta traducir
                        return (result);
                    }

                    //Validar que el código se ajuste al plan
                    bool ctaValidaEnPlan = utilesCG.CuentaValidaEnPlanCuentas(codCuentaMayor, this.codigoPlan, ref error, this.datosPlan, ref this.estructuraPadreCuentas);
                    if (!ctaValidaEnPlan)
                    {
                        error = "Cuenta inválida para la estructura contable";  //Falta traducir
                        return (result);
                    }

                    int nivelCuenta = 0;
                    try
                    {
                        nivelCuenta = Convert.ToInt16(this.estructuraPadreCuentas[0]);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    if (nivelCuenta > 0)
                    {
                        if (nivelCuenta > 1)
                        {
                            string codigoCuentaPadre = this.estructuraPadreCuentas[nivelCuenta-1].Trim();

                            if (codigoCuentaPadre != "")
                            {
                                //---- Validar que exista la cuenta padre -----
                                query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                                query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + codigoCuentaPadre + "'";

                                string tipoCuentaPadre = "";

                                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                                if (dr.Read())
                                {
                                    tipoCuentaPadre = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString();
                                    dr.Close();
                                }
                                else
                                {
                                    dr.Close();
                                    error = "Cuenta a crear no tiene nivel anterior";  //Falta traducir
                                    return (result);
                                }

                                if (tipoCuentaPadre == "D")
                                {
                                    error = "Imposible crear una cuenta nueva si el nivel anterior es de detalle";  //Falta traducir
                                    return (result);
                                }
                            }
                            else
                            {
                                error = "Error recuperando la cuenta del nivel anterior";  //Falta traducir
                                return (result);
                            }
                        }
                    }
                    else
                    {
                        error = "Error calculando el nivel de la cuenta";  //Falta traducir
                        return (result);
                    }

                    result = true;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                //string error = ex.Message;
                if (dr != null) dr.Close();
            }
             
            return (result);
        }

        /// <summary>
        /// Carga la estructura de la cuenta
        /// </summary>
        /// <param name="cuenta"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        private string CargarEstructuraCuenta(string cuenta, string plan)
        {
            string result = "";
            try
            {
                dtEstructuraCuenta = utilesCG.ObtenerEstructuraCuenta(cuenta, plan, ref result);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error al cargar la estructura de la cuenta (" + ex.Message + ")";     //Falta traducir
            }

            return (result);
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
                    errores += "- El nombre abreviado no puede estar en blanco \n\r";      //Falta traducir
                    this.txtNombre.Focus();
                }

                //El código de la cuenta ya fue validado al picarlo

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

        private string DatosFormToTabla()
        {
            string result = "";

            try
            {
                string plan = this.radTextBoxControlPlan.Text.Trim();
                int posSeparador = plan.IndexOf(separadorDesc);
                if (posSeparador != -1) plan = plan.Substring(0, posSeparador - 1).Trim();

                if (this.txtNombreExt.Text.Trim() == "") this.txtNombreExt.Text = this.txtNombre.Text;

                if (this.nuevo || this.copiar)
                {
                    int nivelCuenta = 0;
                    try
                    {
                        nivelCuenta = Convert.ToInt16(this.estructuraPadreCuentas[0]);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                     
                    NIVEMC = nivelCuenta.ToString();

                    //Cuenta Formateada
                    string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codigoPlan);

                    string error = "";
                    int nivel = 0;
                    //Formatear Cuenta
                    CUENMC = this.codigo.Trim();
                    string cuentaFormateada = utilesCG.CuentaFormatear(CUENMC, this.codigoPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);

                    CEDTMC = cuentaFormateada;
                    //NIVEMC = nivel.ToString();
                }

                //Dar de alta a la cuenta en la tabla del maestro de cuentas de mayor (GLM03)
                TIPLMC = plan;
                CUENMC = this.codigo;
                TCUEMC = this.tipoCuenta;
                NOABMC = this.txtNombre.Text.Trim();
                if (NOABMC == "") NOABMC = " ";
                
                SASIMC = "0";
                SCONMC = "0";

                NOLAAD = this.txtNombreExt.Text.Trim();
                if (NOLAAD == "") NOLAAD = NOABMC;             

                //Fecha del sistema en Formato CG
                string fecha = utiles.FechaToFormatoCG(System.DateTime.Now, true).ToString();
                FCRTMC = fecha;       //Fecha del alta o la modificacion
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error recuperando los datos del formulario (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a una cuenta de titulo
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                //Recuperar los valores del formulario
                string resultForm = DatosFormToTabla();

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM03";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATMC, TIPLMC, CUENMC, TCUEMC, NOABMC, NIVEMC, CIERMC, ADICMC, SASIMC, SCONMC, FEVEMC, NDDOMC, TERMMC, TIMOMC, ";
                query += "TAU1MC, TAU2MC, TAU3MC, TDOCMC, GRUPMC, DEAUMC, NOLAAD, RNITMC, CNITMC, MASCMC, CEDTMC, FCRTMC) ";
                query += "values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'V', '" + TIPLMC + "', '" + CUENMC + "', '" + TCUEMC + "', '" + NOABMC + "', " + NIVEMC + ", '";
                query += CIERMC + "', '" + ADICMC + "', " + SASIMC + ", " + SCONMC + ", '" + FEVEMC + "', '" + NDDOMC + "', '";
                query += TERMMC + "', '" + TIMOMC + "', '" + TAU1MC + "', '" + TAU2MC + "', '" + TAU3MC + "', '" + TDOCMC + "', ' ";
                query += GRUPMC + "', '" + DEAUMC + "', '" + NOLAAD + "', '" + RNITMC + "', '" + CNITMC + "', '" + MASCMC + "' , '";
                query += CEDTMC + "', " + FCRTMC + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM03", TIPLMC, CUENMC);

                //Falta insertarla en la tabla GLF03
                nombreTabla = GlobalVar.PrefijoTablaCG + "GLF03";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLBG, CUENBG, SAMDBG) ";
                query += "values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + TIPLMC + "', '" + CUENMC + "', " + FCRTMC + ")";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue); 
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
                //Recuperar los valores del formulario
                string resultForm = DatosFormToTabla();

                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 set ";
                query += "STATMC = '" + estado + "', NOABMC = '" + NOABMC + "', ";
                query += "NOLAAD = '" + NOLAAD + "' ";
                query += "where TIPLMC = '" + TIPLMC + "' and CUENMC = '" + this.codigo + "' and TCUEMC = '" + TCUEMC + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM03", TIPLMC, CUENMC);

                //Actualizar en la GLF03
                query = "update " + GlobalVar.PrefijoTablaCG + "GLF03 set ";
                query += "SAMDBG = " + FCRTMC + " ";
                query += "where TIPLBG = '" + TIPLMC + "' and CUENBG = '" + this.codigo + "'";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve true si la cuenta de mayor de titulo tiene hijos
        /// </summary>
        /// <returns></returns>
        private bool CuentaTieneHijos()
        {
            bool result = true;
            try
            {
                string query = "select count(*) ";
                query += "from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC like '" + this.codigo + "%' and ";
                query += "CUENMC <> '" + this.codigo + "' ";
                ///query += "group by STATMC, TCUEMC, CEDTMC ";
                ///query += "order by CEDTMC";

                ///int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Graba una cuenta de mayor
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo || this.copiar)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtCuentaMayor.Text.Trim();

                        bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                        else utiles.ButtonEnabled(ref this.radButtonDelete, false);
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
        /// Eliminar una cuenta de mayor
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la Cuenta de mayor de título " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Eliminar de la cuenta de mayor
                    string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC = '" + this.codigo + "' and TCUEMC = 'T'";

                    int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLM03", this.codigoPlan, this.codigo);

                    //Eliminar de GLF03
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLF03 ";
                    query += "where TIPLBG = '" + this.codigoPlan + "' and CUENBG = '" + this.codigo + "'";

                    cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    
                    //Cerrar el formulario
                    this.Close();
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("Error eliminando la cuenta (" + ex.Message + ")", "Error");  //Falta traducir
                    Log.Error(Utiles.CreateExceptionString(ex));
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Llama al formulario que muestra los niveles de la cuenta de mayor
        /// </summary>
        private void VerNivelesCuenta()
        {
            frmMtoGLM03VerNivelCuenta frmVisor = new frmMtoGLM03VerNivelCuenta
            {
                DTEstructuraCuenta = this.dtEstructuraCuenta,
                CodigoPlan = this.codigoPlan,
                NombrePlan = this.radTextBoxControlPlan.Text,
                Codigo = this.codigo,
                TipoCuenta = tipoCuenta,
                EstadoCuenta = this.estadoCuenta,
                FrmPadre = this
            };
            frmVisor.ShowDialog();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCuentaMayor.Tag = this.txtCuentaMayor.Text;
            this.txtCuentaMayor.Tag = this.txtCuentaMayor.Text;
            this.txtTipo.Tag = this.txtTipo.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtNombreExt.Tag = this.txtNombreExt.Text;
        }
        #endregion

        private void frmMtoGLM03Titulo_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
