using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM03Detalle : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCTAMAD";

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

        private string codigoGrupoCta;

        private string autClaseElemento = "003";
        private string autGrupo = "01";
        private string autOperModifica = "20";
        //private string autOperAlta = "20";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        //Grupo de cuentas de mayor
        private const string autClaseElementoGLT22 = "008";
        private const string autGrupoGLT22 = "01";
        private const string autOperModificaGLT22 = "20";
        private const string autOperAltaGLT22 = "10";

        private DataTable dtEstructuraCuenta;

        private string[] estructuraPadreCuentas;
        private int[] datosPlan;

        private DataTable dtGLMX2 = null;
        private DataTable dtFormExt = null;

        private bool planTieneCamposExt;
        private bool cuentaMayorEnUso;

        //Campos de la tabla GLM03 que se insertará o se actualizará
        string STATMC = " ";
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

        //string GRCTMX = "";

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

        public frmMtoGLM03Detalle()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchNumDoc.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchNumDoc.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchDetalleAuxiliar.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchDetalleAuxiliar.ThemeName = "MaterialBlueGrey";

            this.gbFechaVto.ElementTree.EnableApplicationThemeName = false;
            this.gbFechaVto.ThemeName = "ControlDefault";

            this.gbNumIdTrib.ElementTree.EnableApplicationThemeName = false;
            this.gbNumIdTrib.ThemeName = "ControlDefault";

            this.gbSegundoDoc.ElementTree.EnableApplicationThemeName = false;
            this.gbSegundoDoc.ThemeName = "ControlDefault";

            this.gbTercerImporte.ElementTree.EnableApplicationThemeName = false;
            this.gbTercerImporte.ThemeName = "ControlDefault";

            this.gbDecTercerImporte.ElementTree.EnableApplicationThemeName = false;
            this.gbDecTercerImporte .ThemeName = "ControlDefault";

            this.gbDigReservDoc.ElementTree.EnableApplicationThemeName = false;
            this.gbDigReservDoc.ThemeName = "ControlDefault";

            this.gbCampo1.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo1.ThemeName = "ControlDefault";
            this.gbCampo2.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo2.ThemeName = "ControlDefault";
            this.gbCampo3.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo3.ThemeName = "ControlDefault";
            this.gbCampo4.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo4.ThemeName = "ControlDefault";
            this.gbCampo5.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo5.ThemeName = "ControlDefault";
            this.gbCampo6.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo6.ThemeName = "ControlDefault";
            this.gbCampo7.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo7.ThemeName = "ControlDefault";
            this.gbCampo8.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo8.ThemeName = "ControlDefault";
            this.gbCampo9.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo9.ThemeName = "ControlDefault";
            this.gbCampo10.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo10.ThemeName = "ControlDefault";
            this.gbCampo11.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo11.ThemeName = "ControlDefault";
            this.gbCampo12.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo12.ThemeName = "ControlDefault";
            this.gbCampo13.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo13.ThemeName = "ControlDefault";
            this.gbCampo14.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo14.ThemeName = "ControlDefault";
            this.gbCampo15.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo15.ThemeName = "ControlDefault";
            this.gbCampo16.ElementTree.EnableApplicationThemeName = false;
            this.gbCampo16.ThemeName = "ControlDefault";

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

        private void FrmMtoGLM03Detalle_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Cuentas de Mayor de Detalle Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Traducir los literales del formulario
            this.TraducirLiterales();

            this.radTextBoxControlPlan.Text = this.nombrePlan;
            this.txtTipo.Text = this.tipoCuenta;
            this.txtTipo.IsReadOnly = true;

            //Verifica si el plan tiene campos extendidos
            this.planTieneCamposExt = this.CamposExtendidos();

            //Verificar autorización sobre crear grupo de cuentas
            utiles.ButtonEnabled(ref this.radButtonCrearGruposCtas, aut.CrearElemento(autClaseElementoGLT22));

            string[] valores = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            utiles.CreateRadDropDownListElement(ref this.cmbNivelSegAsiento, ref valores);
            utiles.CreateRadDropDownListElement(ref this.cmbNivelSegConsultaList, ref valores);

            if (this.nuevo)
            {
                //------------ NUEVO --------------
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);

                utiles.ButtonEnabled(ref this.radButtonCrearGruposCtas, false);
                utiles.ButtonEnabled(ref this.radButtonVerNivelesCta, false);
                utiles.ButtonEnabled(ref this.radButtonSave,false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);
                
                this.estadoCuenta = "V";

                //Cargar la estructura del plan de cuentas
                this.datosPlan = utilesCG.ObtenerNivelLongitudesDadoPlan(this.codigoPlan);

                //Extendidos
                this.CargarInfoCamposExtendidos("");

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCuentaMayor;
                this.txtNombre.Select(0, 0);
                this.txtNombre.Focus();

                //FALTA al grabar llamar a cargar la estructura de la cuenta
            }
            else
            {
                //Cargar la estructura de la cuenta
                this.CargarEstructuraCuenta(this.codigo.Trim(), this.codigoPlan);

                if (this.copiar)
                {
                    //------------ COPIAR --------------
                    this.autEditar = true;
                    //this.HabilitarDeshabilitarControles(false);
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

                    this.cuentaMayorEnUso = this.CuentaMayorEnUso();

                    //Mostrar la información de la cuenta en los controles
                    this.CargarInfoCuenta(this.codigo);

                    bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigoPlan, autOperModifica);
                    this.autEditar = operarModificar;
                    if (!operarModificar)
                    {
                        this.NoEditarCampos();
                        utiles.ButtonEnabled(ref this.radButtonDelete, false);
                    }
                    else
                    {
                        this.ActiveControl = this.txtNombre;
                        this.txtNombre.Select(0, 0);
                        this.txtNombre.Focus();

                        if (this.cuentaMayorEnUso) utiles.ButtonEnabled(ref this.radButtonDelete, false);
                        else utiles.ButtonEnabled(ref this.radButtonDelete, true);
                    }                    
                }
            }
        }

        private void TxtCuentaMayor_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            if (!txtCuentaMayor.IsReadOnly)
            {
                e.KeyChar = Char.ToUpper(e.KeyChar);

                if (this.autEditar) this.HabilitarDeshabilitarControles(true);
            }
        }

        private void TxtCuentaMayor_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                if (this.nuevo || this.copiar)
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
            }
            bTabulador = false;
        }

        private void TxtReservada1Letra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtCodigoConcepto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
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

        private void RadButtonCrearGruposCtas_Click(object sender, EventArgs e)
        {
            this.CrearGruposCuentas();
        }

        private void RadButtonVerNivelesCta_Click(object sender, EventArgs e)
        {
            this.VerNivelesCuenta();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonElementTipoMonedaExt_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIMOME, DESCME from ";
            query += GlobalVar.PrefijoTablaCG + "GLT03 ";
            query += "order by DESCME";

            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

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

        private void RadButtonElementTipoAux1_Click(object sender, EventArgs e)
        {
            this.TipoAuxiliarSeleccionar(ref this.radButtonTextBoxTipoAux1);
        }

        private void RadButtonElementTipoAux2_Click(object sender, EventArgs e)
        {
            this.TipoAuxiliarSeleccionar(ref this.radButtonTextBoxTipoAux2);
        }

        private void RadButtonElementTipoAux3_Click(object sender, EventArgs e)
        {
            this.TipoAuxiliarSeleccionar(ref this.radButtonTextBoxTipoAux3);
        }

        private void RadButtonElementCtaCierre_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select min(CUENMC) CUENMC, max(NOLAAD) NOLAAD, min(CEDTMC) CEDTMC from ";
            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
            query += "where TIPLMC = '" + this.codigoPlan + "' and TCUEMC = 'D' and CUENMC not like '" + this.codigo.Trim() + "%'";
            query += "group by CEDTMC ";
            query += "order by CUENMC";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasCtaCierre = new ArrayList
            {
                //nombreColumnasCtaCierre.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
                "Cuenta",
                //nombreColumnasCtaCierre.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));
                "Nombre",
                //nombreColumnasCtaCierre.Add(this.LP.GetText("lblListaCampoDescFormato", "Código Formato"));
                "Cuenta editada"
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar cuenta cierre",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasCtaCierre,
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
                this.radButtonTextBoxCtaCierre.Text = result;
                this.ActiveControl = this.radButtonTextBoxCtaCierre;
                this.radButtonTextBoxCtaCierre.Select(0, 0);
                this.radButtonTextBoxCtaCierre.Focus();
            }
        }

        private void RadButtonElementGrupoCtas_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select GRCTGC, NOMBGC from ";
            query += GlobalVar.PrefijoTablaCG + "GLT22 ";
            query += "where TIPLGC = '" + this.codigoPlan;
            query += "' order by TIPLGC, GRCTGC";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasGrupoCtas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar grupo de cuentas",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasGrupoCtas,
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
                this.radButtonTextBoxGrupoCtas.Text = result;
                this.ActiveControl = this.radButtonTextBoxGrupoCtas;
                this.radButtonTextBoxGrupoCtas.Select(0, 0);
                this.radButtonTextBoxGrupoCtas.Focus();
            }
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

        private void RadButtonCrearGruposCtas_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCrearGruposCtas);
        }

        private void RadButtonCrearGruposCtas_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCrearGruposCtas);
        }

        private void RadButtonVerNivelesCta_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonVerNivelesCta);
        }

        private void RadButtonVerNivelesCta_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonVerNivelesCta);
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

        private void FrmMtoGLM03Detalle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLM03Detalle_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            try
            {
                if (this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.txtNombreExt.Text.Trim() != this.txtNombreExt.Tag.ToString().Trim() ||
                    this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                    this.radButtonTextBoxTipoAux1.Text.Trim() != this.radButtonTextBoxTipoAux1.Tag.ToString().Trim() ||
                    this.radButtonTextBoxTipoAux2.Text.Trim() != this.radButtonTextBoxTipoAux2.Tag.ToString().Trim() ||
                    this.radButtonTextBoxTipoAux3.Text.Trim() != this.radButtonTextBoxTipoAux3.Tag.ToString().Trim() ||
                    this.radToggleSwitchNumDoc.Value != (bool)(this.radToggleSwitchNumDoc.Tag) ||
                    (this.rbFechaVtoNo.IsChecked && !(Convert.ToBoolean(this.rbFechaVtoNo.Tag))) ||
                    (this.rbFechaVtoDebe.IsChecked && !(Convert.ToBoolean(this.rbFechaVtoDebe.Tag))) ||
                    (this.rbFechaVtoHaber.IsChecked && !(Convert.ToBoolean(this.rbFechaVtoHaber.Tag))) ||
                    this.radButtonTextBoxTipoMonedaExt.Text.Trim() != this.radButtonTextBoxTipoMonedaExt.Tag.ToString().Trim() ||
                    this.cmbNivelSegAsiento.Text.Trim() != this.cmbNivelSegAsiento.Tag.ToString().Trim() ||
                    this.cmbNivelSegConsultaList.Text.Trim() != this.cmbNivelSegConsultaList.Tag.ToString().Trim() ||
                    this.radToggleSwitchDetalleAuxiliar.Value != (bool)(this.radToggleSwitchDetalleAuxiliar.Tag) ||
                    this.txtReservada1Letra.Text.Trim() != this.txtReservada1Letra.Tag.ToString().Trim() ||
                    (this.rbNumIdTribNo.IsChecked && !(Convert.ToBoolean(this.rbNumIdTribNo.Tag))) ||
                    (this.rbNumIdTribIVA.IsChecked && !(Convert.ToBoolean(this.rbNumIdTribIVA.Tag))) ||
                    (this.rbNumIdTribNITD.IsChecked && !(Convert.ToBoolean(this.rbNumIdTribNITD.Tag))) ||
                    (this.rbNumIdTribNITH.IsChecked && !(Convert.ToBoolean(this.rbNumIdTribNITH.Tag))) ||
                    (this.rbNumIdTribNITT.IsChecked && !(Convert.ToBoolean(this.rbNumIdTribNITT.Tag))) ||
                    this.txtCodigoConcepto.Text.Trim() != this.txtCodigoConcepto.Tag.ToString().Trim() ||
                    (this.rbSegundoDocSi.IsChecked && !(Convert.ToBoolean(this.rbSegundoDocSi.Tag))) ||
                    (this.rbSegundoDocIVA.IsChecked && !(Convert.ToBoolean(this.rbSegundoDocIVA.Tag))) ||
                    (this.rbSegundoDocNo.IsChecked && !(Convert.ToBoolean(this.rbSegundoDocNo.Tag))) ||
                    (this.rbTercerImporteSi.IsChecked && !(Convert.ToBoolean(this.rbTercerImporteSi.Tag))) ||
                    (this.rbTercerImporteIVA.IsChecked && !(Convert.ToBoolean(this.rbTercerImporteIVA.Tag))) ||
                    (this.rbTercerImporteNo.IsChecked && !(Convert.ToBoolean(this.rbTercerImporteNo.Tag))) ||
                    (this.rbDecTercerImporteSi.IsChecked && !(Convert.ToBoolean(this.rbDecTercerImporteSi.Tag))) ||
                    (this.rbDecTercerImporteSegunCia.IsChecked && !(Convert.ToBoolean(this.rbDecTercerImporteSegunCia.Tag))) ||
                    (this.rbDecTercerImporteNo.IsChecked && !(Convert.ToBoolean(this.rbDecTercerImporteNo.Tag))) ||
                    (this.rbDigReservDoc0.IsChecked && !(Convert.ToBoolean(this.rbDigReservDoc0.Tag))) ||
                    (this.rbDigReservDoc1.IsChecked && !(Convert.ToBoolean(this.rbDigReservDoc1.Tag))) ||
                    (this.rbDigReservDoc2.IsChecked && !(Convert.ToBoolean(this.rbDigReservDoc2.Tag))) ||
                    (this.rbDigReservDocNo.IsChecked && !(Convert.ToBoolean(this.rbDigReservDocNo.Tag))) ||
                    this.radButtonTextBoxCtaCierre.Text.Trim() != this.radButtonTextBoxCtaCierre.Tag.ToString().Trim() ||
                    this.radButtonTextBoxGrupoCtas.Text.Trim() != this.radButtonTextBoxGrupoCtas.Tag.ToString().Trim() ||
                    this.CambioValoresCamposExtendidos()
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

            if (cerrarForm) Log.Info("FIN Mantenimiento de Cuentas de Mayor de Detalle Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmMtoGLM03Titulo", "Mantenimiento de Cuentas de Mayor");   //Falta traducir

            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar"); 
            this.radButtonCrearGruposCtas.Text = this.LP.GetText("lblCrearGruposCtas", "Crear Grupos de Cuentas"); 
            this.radButtonVerNivelesCta.Text = this.LP.GetText("toolStripVerNivelesCta", "Ver niveles cuenta");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
            
            //Traducir los campos del formulario
            this.lblPlan.Text = this.LP.GetText("lblGLM03Plan", "Plan");
        }
        
        /// <summary>
        /// Construir el control de seleccion de clases de zona
        /// </summary>
        private void BuildtgTexBoxSelTipoAux(ref TGTexBoxSel tipoAux)
        {
            tipoAux.NumeroCaracteresView = 10;
            tipoAux.AjustarTamanoTextBox();

            tipoAux.CantidadColumnasResult = 2;
            tipoAux.Textbox.MaxLength = 2;

            tipoAux.TodoMayuscula = true;

            //Consulta para obtener los planes
            tipoAux.ProveedorDatosFormSel = GlobalVar.ConexionCG;
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";
            tipoAux.QueryFormSel = query;

            tipoAux.FrmPadre = this;
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
                this.dtEstructuraCuenta = utilesCG.ObtenerEstructuraCuenta(cuenta, plan, ref result);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error al cargar la estructura de la cuenta (" + ex.Message + ")";     //Falta traducir
            }

            return (result);
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
                        if (nivelCuenta == 1)
                        {
                            error = "Una cuenta de primer nivel no puede ser de detalle";  //Falta traducir
                            return (result);
                        }
                        
                        string codigoCuentaPadre = this.estructuraPadreCuentas[nivelCuenta - 1].Trim();

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

                string desc = "";
                string tipoAux1 = "";
                string tipoAux2 = "";
                string tipoAux3 = "";
                string numeroDoc = "";
                string fechaVto = "";
                string monedaExt = "";
                string nivelSegAsiento = "";
                string nivelSegConsulta = "";
                string detalleAux = "";
                string noIdTrib = "";
                string segundoDoc = "";
                string tercerImp = "";
                string decimalesTercerImp = "";
                string digitoReservDoc = "";
                string cuentaCierre = "";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOABMC")).ToString().Trim();
                    this.txtNombreExt.Text = dr.GetValue(dr.GetOrdinal("NOLAAD")).ToString().Trim();

                    this.estadoCuenta = dr.GetValue(dr.GetOrdinal("STATMC")).ToString().Trim();
                    if (this.estadoCuenta == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;

                    //Tipos de auxiliar
                    tipoAux1 = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString().Trim();
                    if (tipoAux1 != "")
                    {
                        desc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", tipoAux1, false, "").Trim();
                        if (desc != "") tipoAux1 = tipoAux1 + " " + separadorDesc + " " + desc;
                    }
                    this.radButtonTextBoxTipoAux1.Text = tipoAux1;

                    tipoAux2 = dr.GetValue(dr.GetOrdinal("TAU2MC")).ToString().Trim();
                    if (tipoAux2 != "")
                    {
                        desc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", tipoAux2, false, "").Trim();
                        if (desc != "") tipoAux2 = tipoAux2 + " " + separadorDesc + " " + desc;
                    }
                    this.radButtonTextBoxTipoAux2.Text = tipoAux2;

                    tipoAux3 = dr.GetValue(dr.GetOrdinal("TAU3MC")).ToString().Trim();
                    if (tipoAux3 != "")
                    {
                        desc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", tipoAux3, false, "").Trim();
                        if (desc != "") tipoAux3 = tipoAux3 + " " + separadorDesc + " " + desc;
                    }
                    this.radButtonTextBoxTipoAux3.Text = tipoAux3;
                    
                    numeroDoc = dr.GetValue(dr.GetOrdinal("TDOCMC")).ToString().Trim();
                    if (numeroDoc == "S") this.radToggleSwitchNumDoc.Value = true;
                    else radToggleSwitchNumDoc.Value = false;

                    fechaVto = dr.GetValue(dr.GetOrdinal("FEVEMC")).ToString().Trim();
                    switch (fechaVto)
                    {
                        case "D":
                            this.rbFechaVtoNo.IsChecked = false;
                            this.rbFechaVtoDebe.IsChecked = true;
                            this.rbFechaVtoHaber.IsChecked = false;
                            break;
                        case "H":
                            this.rbFechaVtoNo.IsChecked = false;
                            this.rbFechaVtoDebe.IsChecked = false;
                            this.rbFechaVtoHaber.IsChecked = true;
                            break;
                        case "N":
                        default:
                            this.rbFechaVtoNo.IsChecked = true;
                            this.rbFechaVtoDebe.IsChecked = false;
                            this.rbFechaVtoHaber.IsChecked = false;
                            break;
                    }

                    monedaExt = dr.GetValue(dr.GetOrdinal("TIMOMC")).ToString().Trim();
                    if (monedaExt != "")
                    {
                        desc = utilesCG.ObtenerDescDadoCodigo("GLT03", "TIMOME", "DESCME", monedaExt, false, "").Trim();
                        if (desc != "") monedaExt = monedaExt + " " + separadorDesc + " " + desc;
                    }
                    this.radButtonTextBoxTipoMonedaExt.Text = monedaExt;
                    
                    nivelSegAsiento = dr.GetValue(dr.GetOrdinal("SASIMC")).ToString().Trim();
                    try
                    {
                        if (nivelSegAsiento == " ") nivelSegAsiento = "0";
                        this.cmbNivelSegAsiento.SelectedIndex = this.cmbNivelSegAsiento.Items.IndexOf(nivelSegAsiento);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.cmbNivelSegAsiento.SelectedIndex = 0; 
                    }

                    nivelSegConsulta = dr.GetValue(dr.GetOrdinal("SCONMC")).ToString().Trim();
                    try
                    {
                        if (nivelSegConsulta == " ") nivelSegConsulta = "0";
                        this.cmbNivelSegConsultaList.SelectedIndex = this.cmbNivelSegConsultaList.Items.IndexOf(nivelSegConsulta);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        this.cmbNivelSegAsiento.SelectedIndex = 0; 
                    }

                    detalleAux = dr.GetValue(dr.GetOrdinal("DEAUMC")).ToString().Trim();
                    if (detalleAux == "S") this.radToggleSwitchDetalleAuxiliar.Value = true;
                    else this.radToggleSwitchDetalleAuxiliar.Value = false;

                    this.txtReservada1Letra.Text = dr.GetValue(dr.GetOrdinal("GRUPMC")).ToString().Trim();

                    noIdTrib = dr.GetValue(dr.GetOrdinal("RNITMC")).ToString().Trim();
                    switch (noIdTrib)
                    {
                        case "R":
                            this.rbNumIdTribNo.IsChecked = false;
                            this.rbNumIdTribIVA.IsChecked = true;
                            this.rbNumIdTribNITD.IsChecked = false;
                            this.rbNumIdTribNITH.IsChecked = false;
                            this.rbNumIdTribNITT.IsChecked = false;
                            break;
                        case "D":
                            this.rbNumIdTribNo.IsChecked = false;
                            this.rbNumIdTribIVA.IsChecked = false;
                            this.rbNumIdTribNITD.IsChecked = true;
                            this.rbNumIdTribNITH.IsChecked = false;
                            this.rbNumIdTribNITT.IsChecked = false;
                            break;
                        case "H":
                            this.rbNumIdTribNo.IsChecked = false;
                            this.rbNumIdTribIVA.IsChecked = false;
                            this.rbNumIdTribNITD.IsChecked = false;
                            this.rbNumIdTribNITH.IsChecked = true;
                            this.rbNumIdTribNITT.IsChecked = false;
                            break;
                        case "T":
                            this.rbNumIdTribNo.IsChecked = false;
                            this.rbNumIdTribIVA.IsChecked = false;
                            this.rbNumIdTribNITD.IsChecked = false;
                            this.rbNumIdTribNITH.IsChecked = false;
                            this.rbNumIdTribNITT.IsChecked = true;
                            break;
                        case "N":
                        default:
                            this.rbNumIdTribNo.IsChecked = true;
                            this.rbNumIdTribIVA.IsChecked = false;
                            this.rbNumIdTribNITD.IsChecked = false;
                            this.rbNumIdTribNITH.IsChecked = false;
                            this.rbNumIdTribNITT.IsChecked = false;
                            break;
                    }

                    this.txtCodigoConcepto.Text = dr.GetValue(dr.GetOrdinal("CNITMC")).ToString().Trim();

                    segundoDoc = dr.GetValue(dr.GetOrdinal("NDDOMC")).ToString().Trim();
                    switch (segundoDoc)
                    {
                        case "S":
                            this.rbSegundoDocSi.IsChecked = true;
                            this.rbSegundoDocIVA.IsChecked = false;
                            this.rbSegundoDocNo.IsChecked = false;
                            break;
                        case "R":
                            this.rbSegundoDocSi.IsChecked = false;
                            this.rbSegundoDocIVA.IsChecked = true;
                            this.rbSegundoDocNo.IsChecked = false;
                            break;
                        case "N":
                        default:
                            this.rbSegundoDocSi.IsChecked = false;
                            this.rbSegundoDocIVA.IsChecked = false;
                            this.rbSegundoDocNo.IsChecked = true;
                            break;
                    }

                    tercerImp = dr.GetValue(dr.GetOrdinal("TERMMC")).ToString().Trim();
                    switch (tercerImp)
                    {
                        case "S":
                            this.rbTercerImporteSi.IsChecked = true;
                            this.rbTercerImporteIVA.IsChecked = false;
                            this.rbTercerImporteNo.IsChecked = false;
                            break;
                        case "R":
                            this.rbTercerImporteSi.IsChecked = false;
                            this.rbTercerImporteIVA.IsChecked = true;
                            this.rbTercerImporteNo.IsChecked = false;
                            break;
                        case "N":
                        default:
                            this.rbTercerImporteSi.IsChecked = false;
                            this.rbTercerImporteIVA.IsChecked = false;
                            this.rbTercerImporteNo.IsChecked = true;
                            break;
                    }

                    decimalesTercerImp = dr.GetValue(dr.GetOrdinal("MASCMC")).ToString().Trim();
                    switch (decimalesTercerImp)
                    {
                        case "S":
                            this.rbDecTercerImporteSi.IsChecked = true;
                            this.rbDecTercerImporteSegunCia.IsChecked = false;
                            this.rbDecTercerImporteNo.IsChecked = false;
                            break;
                        case "C":
                            this.rbDecTercerImporteSi.IsChecked = false;
                            this.rbDecTercerImporteSegunCia.IsChecked = true;
                            this.rbDecTercerImporteNo.IsChecked = false;
                            break;
                        case "N":
                        default:
                            this.rbDecTercerImporteSi.IsChecked = false;
                            this.rbDecTercerImporteSegunCia.IsChecked = false;
                            this.rbDecTercerImporteNo.IsChecked = true;
                            break;
                    }

                    digitoReservDoc = dr.GetValue(dr.GetOrdinal("ADICMC")).ToString().Trim();
                    switch (digitoReservDoc)
                    {
                        case "0":
                            this.rbDigReservDoc0.IsChecked = true;
                            this.rbDigReservDoc1.IsChecked = false;
                            this.rbDigReservDoc2.IsChecked = false;
                            this.rbDigReservDocNo.IsChecked = false;
                            break;
                        case "1":
                            this.rbDigReservDoc0.IsChecked = false;
                            this.rbDigReservDoc1.IsChecked = true;
                            this.rbDigReservDoc2.IsChecked = false;
                            this.rbDigReservDocNo.IsChecked = false;
                            break;
                        case "2":
                            this.rbDigReservDoc0.IsChecked = false;
                            this.rbDigReservDoc1.IsChecked = false;
                            this.rbDigReservDoc2.IsChecked = true;
                            this.rbDigReservDocNo.IsChecked = false;
                            break;
                        case "N":
                        default:
                            this.rbDigReservDoc0.IsChecked = false;
                            this.rbDigReservDoc1.IsChecked = false;
                            this.rbDigReservDoc2.IsChecked = false;
                            this.rbDigReservDocNo.IsChecked = true;
                            break;
                    }

                    cuentaCierre = dr.GetValue(dr.GetOrdinal("CIERMC")).ToString().Trim();
                    if (cuentaCierre != "")
                    {
                        string filtro = " and TIPLMC = '" + this.codigoPlan + "' ";
                        desc = utilesCG.ObtenerDescDadoCodigo("GLM03", "CUENMC", "NOLAAD", cuentaCierre, false, filtro).Trim();
                        if (desc != "") cuentaCierre = cuentaCierre + " " + separadorDesc + " " + desc;
                    }
                    this.radButtonTextBoxCtaCierre.Text = cuentaCierre;
                }

                dr.Close();

                if (this.cuentaMayorEnUso)
                {
                    //La cuenta de mayor está en uso en saldos o en asientos contables y no es posibles editar los campos
                    this.radButtonTextBoxTipoAux1.Enabled = false;
                    this.radButtonElementTipoAux1.Enabled = false;
                    this.radButtonTextBoxTipoAux2.Enabled = false;
                    this.radButtonElementTipoAux2.Enabled = false;
                    this.radButtonTextBoxTipoAux3.Enabled = false;
                    this.radButtonElementTipoAux3.Enabled = false;
                    this.radToggleSwitchNumDoc.Enabled = false;
                    this.gbFechaVto.Enabled = false;
                    this.radButtonTextBoxTipoMonedaExt.Enabled = false;
                    this.radButtonElementTipoMonedaExt.Enabled = false;
                    this.gbDigReservDoc.Enabled = false;
                }

                //Extendidos
                this.CargarInfoCamposExtendidos(codigoCuentaCargar);

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
        /// Rellena los controles de los campos extendidos con los datos de la cuenta de mayor (modo edición y modo copiar)
        /// </summary>
        /// <param name="codigoCuentaCargar"></param>
        private void CargarInfoCamposExtendidos(string codigoCuentaCargar)
        {
            IDataReader dr = null;

            try
            {
                string FGPRMX = "0";
                string FGFAMX = "0";
                string FGFRMX = "0";
                string FGDVMX = "0";
                string FG01MX = "0";
                string FG02MX = "0";
                string FG03MX = "0";
                string FG04MX = "0";
                string FG05MX = "0";
                string FG06MX = "0";
                string FG07MX = "0";
                string FG08MX = "0";
                string FG09MX = "0";
                string FG10MX = "0";
                string FG11MX = "0";
                string FG12MX = "0";

                if (!this.nuevo)
                {
                    string cuentaDetalleUltimoNivel = "";

                    string errorMsg = "";
                    DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivel(this.codigo, this.codigoPlan, ref errorMsg);

                    if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0)
                    {
                        cuentaDetalleUltimoNivel = dtCtasUltimoNivel.Rows[dtCtasUltimoNivel.Rows.Count - 1]["CUENMC"].ToString().Trim();
                    }

                    if (cuentaDetalleUltimoNivel != "")
                    {
                        //Verificar que exista la tabla GLMX3
                        bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX3");

                        if (!existeTabla) return;

                        string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                        query += "where TIPLMX = '" + this.codigoPlan + "' and ";
                        query += "CUENMX = '" + cuentaDetalleUltimoNivel + "' ";

                        string grupoCtas = "";
                        string desc = "";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        if (dr.Read())
                        {
                            grupoCtas = dr.GetValue(dr.GetOrdinal("GRCTMX")).ToString().Trim();
                            if (grupoCtas != "")
                            {
                                string filtro = " and TIPLGC = '" + this.codigoPlan + "' ";
                                desc = utilesCG.ObtenerDescDadoCodigo("GLT22", "GRCTGC", "NOMBGC", grupoCtas, false, filtro).Trim();
                                if (desc != "") grupoCtas = grupoCtas + " " + separadorDesc + " " + desc;
                            }
                            this.radButtonTextBoxGrupoCtas.Text = grupoCtas;

                            if (this.planTieneCamposExt)
                            {
                                //Recuperar los valores para los campos extendidos
                                FGPRMX = dr.GetValue(dr.GetOrdinal("FGPRMX")).ToString();
                                FGFAMX = dr.GetValue(dr.GetOrdinal("FGFAMX")).ToString();
                                FGFRMX = dr.GetValue(dr.GetOrdinal("FGFRMX")).ToString();
                                FGDVMX = dr.GetValue(dr.GetOrdinal("FGDVMX")).ToString();
                                FG01MX = dr.GetValue(dr.GetOrdinal("FG01MX")).ToString();
                                FG02MX = dr.GetValue(dr.GetOrdinal("FG02MX")).ToString();
                                FG03MX = dr.GetValue(dr.GetOrdinal("FG03MX")).ToString();
                                FG04MX = dr.GetValue(dr.GetOrdinal("FG04MX")).ToString();
                                FG05MX = dr.GetValue(dr.GetOrdinal("FG05MX")).ToString();
                                FG06MX = dr.GetValue(dr.GetOrdinal("FG06MX")).ToString();
                                FG07MX = dr.GetValue(dr.GetOrdinal("FG07MX")).ToString();
                                FG08MX = dr.GetValue(dr.GetOrdinal("FG08MX")).ToString();
                                FG09MX = dr.GetValue(dr.GetOrdinal("FG09MX")).ToString();
                                FG10MX = dr.GetValue(dr.GetOrdinal("FG10MX")).ToString();
                                FG11MX = dr.GetValue(dr.GetOrdinal("FG11MX")).ToString();
                                FG12MX = dr.GetValue(dr.GetOrdinal("FG12MX")).ToString();
                            }
                        }

                        dr.Close();
                    }
                }

                //Mostrar los controles que corresponda
                if (this.planTieneCamposExt && this.dtGLMX2 != null)
                {
                    //Construir el DataTable para los campos extendidos del Formulario
                    this.BuilddtFormExt();

                    int contador = 0;
                    if (this.dtGLMX2.Rows[0]["FGPRPX"].ToString() == "1") this.ActivarEtiqueta(ref contador, "Prefijo de documento", "05", FGPRMX);

                    if (this.dtGLMX2.Rows[0]["FGFAPX"].ToString() == "1") this.ActivarEtiqueta(ref contador, "No. Factura ampliado", "25", FGFAMX);

                    if (this.dtGLMX2.Rows[0]["FGFRPX"].ToString() == "1") this.ActivarEtiqueta(ref contador, "No. Factura rectificativa", "25", FGFRMX);

                    if (this.dtGLMX2.Rows[0]["FGDVPX"].ToString() == "1") this.ActivarEtiqueta(ref contador, "Fecha Servicio", "06", FGDVMX);

                    if (this.dtGLMX2.Rows[0]["FG01PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM01PX"].ToString(), this.dtGLMX2.Rows[0]["MX01PX"].ToString(), FG01MX);

                    if (this.dtGLMX2.Rows[0]["FG02PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM02PX"].ToString(), this.dtGLMX2.Rows[0]["MX02PX"].ToString(), FG02MX);

                    if (this.dtGLMX2.Rows[0]["FG03PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM03PX"].ToString(), this.dtGLMX2.Rows[0]["MX03PX"].ToString(), FG03MX);

                    if (this.dtGLMX2.Rows[0]["FG04PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM04PX"].ToString(), this.dtGLMX2.Rows[0]["MX04PX"].ToString(), FG04MX);

                    if (this.dtGLMX2.Rows[0]["FG05PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM05PX"].ToString(), this.dtGLMX2.Rows[0]["MX05PX"].ToString(), FG05MX);

                    if (this.dtGLMX2.Rows[0]["FG06PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM06PX"].ToString(), this.dtGLMX2.Rows[0]["MX06PX"].ToString(), FG06MX);

                    if (this.dtGLMX2.Rows[0]["FG07PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM07PX"].ToString(), this.dtGLMX2.Rows[0]["MX07PX"].ToString(), FG07MX);

                    if (this.dtGLMX2.Rows[0]["FG08PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM08PX"].ToString(), this.dtGLMX2.Rows[0]["MX08PX"].ToString(), FG08MX);

                    if (this.dtGLMX2.Rows[0]["FG09PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM09PX"].ToString(), "15,2", FG09MX);

                    if (this.dtGLMX2.Rows[0]["FG10PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM10PX"].ToString(), "15,2", FG10MX);

                    if (this.dtGLMX2.Rows[0]["FG11PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM11PX"].ToString(), "06", FG11MX);

                    if (this.dtGLMX2.Rows[0]["FG12PX"].ToString() == "1") this.ActivarEtiqueta(ref contador, this.dtGLMX2.Rows[0]["NM12PX"].ToString(), "06", FG12MX);

                }
                else this.radPageViewDatos.Pages.Remove(this.radPageViewPageCamposExt);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        private void BuilddtFormExt()
        {
            try
            {
                this.dtFormExt = new DataTable
                {
                    TableName = "FormExt"
                };

                this.dtFormExt.Columns.Add("FGPRMX", typeof(string));
                this.dtFormExt.Columns.Add("FGFAMX", typeof(string));
                this.dtFormExt.Columns.Add("FGFRMX", typeof(string));
                this.dtFormExt.Columns.Add("FGDVMX", typeof(string));
                this.dtFormExt.Columns.Add("FG01MX", typeof(string));
                this.dtFormExt.Columns.Add("FG02MX", typeof(string));
                this.dtFormExt.Columns.Add("FG03MX", typeof(string));
                this.dtFormExt.Columns.Add("FG04MX", typeof(string));
                this.dtFormExt.Columns.Add("FG05MX", typeof(string));
                this.dtFormExt.Columns.Add("FG06MX", typeof(string));
                this.dtFormExt.Columns.Add("FG07MX", typeof(string));
                this.dtFormExt.Columns.Add("FG08MX", typeof(string));
                this.dtFormExt.Columns.Add("FG09MX", typeof(string));
                this.dtFormExt.Columns.Add("FG10MX", typeof(string));
                this.dtFormExt.Columns.Add("FG11MX", typeof(string));
                this.dtFormExt.Columns.Add("FG12MX", typeof(string));

                DataRow row = this.dtFormExt.NewRow();

                row["FGPRMX"] = "";
                row["FGFAMX"] = "";
                row["FGFRMX"] = "";
                row["FGDVMX"] = "";
                row["FG01MX"] = "";
                row["FG02MX"] = "";
                row["FG03MX"] = "";
                row["FG04MX"] = "";
                row["FG05MX"] = "";
                row["FG06MX"] = "";
                row["FG07MX"] = "";
                row["FG08MX"] = "";
                row["FG09MX"] = "";
                row["FG10MX"] = "";
                row["FG11MX"] = "";
                row["FG12MX"] = "";

                this.dtFormExt.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Escribe una línea de los campos extendidos
        /// </summary>
        /// <param name="contador">contador de la linea</param>
        /// <param name="labelCampo"></param>
        /// <param name="longCampo"></param>
        /// <param name="valorCampo"></param>
        private void ActivarEtiqueta(ref int contador, string labelCampo, string longCampo, string valorCampo)
        {
            try
            {
                contador++;
                switch (contador)
                {
                    case 1:
                        this.lblCampo1.Text = labelCampo;
                        this.lblCampo1.Visible = true;
                        this.lblLongCampo1.Text = longCampo;
                        this.lblLongCampo1.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo1Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo1NoObligatorio.IsChecked = true;
                        else this.rbReqCampo1No.IsChecked = true;
                        this.gbCampo1.Visible = true;
                        break;
                    case 2:
                        this.lblCampo2.Text = labelCampo;
                        this.lblCampo2.Visible = true;
                        this.lblLongCampo2.Text = longCampo;
                        this.lblLongCampo2.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo2Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo2NoObligatorio.IsChecked = true;
                        else this.rbReqCampo2No.IsChecked = true;
                        this.gbCampo2.Visible = true;
                        break;
                    case 3:
                        this.lblCampo3.Text = labelCampo;
                        this.lblCampo3.Visible = true;
                        this.lblLongCampo3.Text = longCampo;
                        this.lblLongCampo3.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo3Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo3NoObligatorio.IsChecked = true;
                        else this.rbReqCampo3No.IsChecked = true;
                        this.gbCampo3.Visible = true;
                        break;
                    case 4:
                        this.lblCampo4.Text = labelCampo;
                        this.lblCampo4.Visible = true;
                        this.lblLongCampo4.Text = longCampo;
                        this.lblLongCampo4.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo4Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo4NoObligatorio.IsChecked = true;
                        else this.rbReqCampo4No.IsChecked = true;
                        this.gbCampo4.Visible = true;
                        break;
                    case 5:
                        this.lblCampo5.Text = labelCampo;
                        this.lblCampo5.Visible = true;
                        this.lblLongCampo5.Text = longCampo;
                        this.lblLongCampo5.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo5Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo5NoObligatorio.IsChecked = true;
                        else this.rbReqCampo5No.IsChecked = true;
                        this.gbCampo5.Visible = true;
                        break;
                    case 6:
                        this.lblCampo6.Text = labelCampo;
                        this.lblCampo6.Visible = true;
                        this.lblLongCampo6.Text = longCampo;
                        this.lblLongCampo6.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo6Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo6NoObligatorio.IsChecked = true;
                        else this.rbReqCampo6No.IsChecked = true;
                        this.gbCampo6.Visible = true;
                        break;
                    case 7:
                        this.lblCampo7.Text = labelCampo;
                        this.lblCampo7.Visible = true;
                        this.lblLongCampo7.Text = longCampo;
                        this.lblLongCampo7.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo7Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo7NoObligatorio.IsChecked = true;
                        else this.rbReqCampo7No.IsChecked = true;
                        this.gbCampo7.Visible = true;
                        break;
                    case 8:
                        this.lblCampo8.Text = labelCampo;
                        this.lblCampo8.Visible = true;
                        this.lblLongCampo8.Text = longCampo;
                        this.lblLongCampo8.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo8Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo8NoObligatorio.IsChecked = true;
                        else this.rbReqCampo8No.IsChecked = true;
                        this.gbCampo8.Visible = true;
                        break;
                    case 9:
                        this.lblCampo9.Text = labelCampo;
                        this.lblCampo9.Visible = true;
                        this.lblLongCampo9.Text = longCampo;
                        this.lblLongCampo9.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo9Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo9NoObligatorio.IsChecked = true;
                        else this.rbReqCampo9No.IsChecked = true;
                        this.gbCampo9.Visible = true;
                        break;
                    case 10:
                        this.lblCampo10.Text = labelCampo;
                        this.lblCampo10.Visible = true;
                        this.lblLongCampo10.Text = longCampo;
                        this.lblLongCampo10.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo10Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo10NoObligatorio.IsChecked = true;
                        else this.rbReqCampo10No.IsChecked = true;
                        this.gbCampo10.Visible = true;
                        break;
                    case 11:
                        this.lblCampo11.Text = labelCampo;
                        this.lblCampo11.Visible = true;
                        this.lblLongCampo11.Text = longCampo;
                        this.lblLongCampo11.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo11Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo11NoObligatorio.IsChecked = true;
                        else this.rbReqCampo11No.IsChecked = true;
                        this.gbCampo11.Visible = true;
                        break;
                    case 12:
                        this.lblCampo12.Text = labelCampo;
                        this.lblCampo12.Visible = true;
                        this.lblLongCampo12.Text = longCampo;
                        this.lblLongCampo12.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo12Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo12NoObligatorio.IsChecked = true;
                        else this.rbReqCampo12No.IsChecked = true;
                        this.gbCampo12.Visible = true;
                        break;
                    case 13:
                        this.lblCampo13.Text = labelCampo;
                        this.lblCampo13.Visible = true;
                        this.lblLongCampo13.Text = longCampo;
                        this.lblLongCampo13.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo13Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo13NoObligatorio.IsChecked = true;
                        else this.rbReqCampo13No.IsChecked = true;
                        this.gbCampo13.Visible = true;
                        break;
                    case 14:
                        this.lblCampo14.Text = labelCampo;
                        this.lblCampo14.Visible = true;
                        this.lblLongCampo14.Text = longCampo;
                        this.lblLongCampo14.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo14Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo14NoObligatorio.IsChecked = true;
                        else this.rbReqCampo14No.IsChecked = true;
                        this.gbCampo14.Visible = true;
                        break;
                    case 15:
                        this.lblCampo15.Text = labelCampo;
                        this.lblCampo15.Visible = true;
                        this.lblLongCampo15.Text = longCampo;
                        this.lblLongCampo15.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo15Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo15NoObligatorio.IsChecked = true;
                        else this.rbReqCampo15No.IsChecked = true;
                        this.gbCampo15.Visible = true;
                        break;
                    case 16:
                        this.lblCampo16.Text = labelCampo;
                        this.lblCampo16.Visible = true;
                        this.lblLongCampo16.Text = longCampo;
                        this.lblLongCampo16.Visible = true;
                        if (valorCampo == "1") this.rbReqCampo16Obligatorio.IsChecked = true;
                        else if (valorCampo == "2") this.rbReqCampo16NoObligatorio.IsChecked = true;
                        else this.rbReqCampo16No.IsChecked = true;
                        this.gbCampo16.Visible = true;
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Verifica si el plan utiliza los campos extendidos
        /// </summary>
        /// <returns></returns>
        private bool CamposExtendidos()
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Verificar que exista la tabla GLMX2
                bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX2");

                if (!existeTabla) return (result);

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                query += "where TIPLPX = '" + this.codigoPlan + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    string FGPRPX = dr.GetValue(dr.GetOrdinal("FGPRPX")).ToString();
                    string FGFAPX = dr.GetValue(dr.GetOrdinal("FGFAPX")).ToString();
                    string FGFRPX = dr.GetValue(dr.GetOrdinal("FGFRPX")).ToString();
                    string FGDVPX = dr.GetValue(dr.GetOrdinal("FGDVPX")).ToString();
                    string FG01PX = dr.GetValue(dr.GetOrdinal("FG01PX")).ToString();
                    string FG02PX = dr.GetValue(dr.GetOrdinal("FG02PX")).ToString();
                    string FG03PX = dr.GetValue(dr.GetOrdinal("FG03PX")).ToString();
                    string FG04PX = dr.GetValue(dr.GetOrdinal("FG04PX")).ToString();
                    string FG05PX = dr.GetValue(dr.GetOrdinal("FG05PX")).ToString();
                    string FG06PX = dr.GetValue(dr.GetOrdinal("FG06PX")).ToString();
                    string FG07PX = dr.GetValue(dr.GetOrdinal("FG07PX")).ToString();
                    string FG08PX = dr.GetValue(dr.GetOrdinal("FG08PX")).ToString();
                    string FG09PX = dr.GetValue(dr.GetOrdinal("FG09PX")).ToString();
                    string FG10PX = dr.GetValue(dr.GetOrdinal("FG10PX")).ToString();
                    string FG11PX = dr.GetValue(dr.GetOrdinal("FG11PX")).ToString();
                    string FG12PX = dr.GetValue(dr.GetOrdinal("FG12PX")).ToString();

                    //Chequear que al menos exista una columna visible
                    if (FGPRPX == "0" && FGFAPX == "0" && FGFRPX == "0" && FGDVPX == "0" && FG01PX == "0" && FG01PX == "0" &&
                        FG02PX == "0" && FG03PX == "0" && FG04PX == "0" && FG05PX == "0" && FG06PX == "0" && FG06PX == "0" &&
                        FG07PX == "0" && FG08PX == "0" && FG08PX == "0" && FG09PX == "0" && FG10PX == "0" && FG11PX == "0" &&
                        FG12PX == "0")
                    {
                        dr.Close();
                        return (result);
                    }

                    if (this.dtGLMX2 != null && this.dtGLMX2.Rows.Count > 0) this.dtGLMX2.Clear();
                    this.dtGLMX2 = new DataTable
                    {
                        TableName = "GLMX2"
                    };

                    this.dtGLMX2.Columns.Add("FGPRPX", typeof(string));
                    this.dtGLMX2.Columns.Add("PGPRPX", typeof(string));
                    this.dtGLMX2.Columns.Add("FGFAPX", typeof(string));
                    this.dtGLMX2.Columns.Add("PGFAPX", typeof(string));
                    this.dtGLMX2.Columns.Add("FGFRPX", typeof(string));
                    this.dtGLMX2.Columns.Add("PGFRPX", typeof(string));
                    this.dtGLMX2.Columns.Add("FGDVPX", typeof(string));
                    this.dtGLMX2.Columns.Add("PGDVPX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM01PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX01PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG01PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA01PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG01PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM02PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX02PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG02PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA02PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG02PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM03PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX03PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG03PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA03PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG03PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM04PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX04PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG04PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA04PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG04PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM05PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX05PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG05PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA05PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG05PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM06PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX06PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG06PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA06PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG06PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM07PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX07PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG07PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA07PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG07PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM08PX", typeof(string));
                    this.dtGLMX2.Columns.Add("MX08PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG08PX", typeof(string));
                    this.dtGLMX2.Columns.Add("TA08PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG08PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM09PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG09PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG09PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM10PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG10PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG10PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM11PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG11PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG11PX", typeof(string));
                    this.dtGLMX2.Columns.Add("NM12PX", typeof(string));
                    this.dtGLMX2.Columns.Add("PG12PX", typeof(string));
                    this.dtGLMX2.Columns.Add("FG12PX", typeof(string));

                    DataRow row = this.dtGLMX2.NewRow();

                    row["FGPRPX"] = FGPRPX;
                    row["PGPRPX"] = dr.GetValue(dr.GetOrdinal("PGPRPX")).ToString();
                    row["FGFAPX"] = FGFAPX;
                    row["PGFAPX"] = dr.GetValue(dr.GetOrdinal("PGFAPX")).ToString();
                    row["FGFRPX"] = FGFRPX;
                    row["PGFRPX"] = dr.GetValue(dr.GetOrdinal("PGFRPX")).ToString();
                    row["FGDVPX"] = FGDVPX;
                    row["PGDVPX"] = dr.GetValue(dr.GetOrdinal("PGDVPX")).ToString();
                    row["NM01PX"] = dr.GetValue(dr.GetOrdinal("NM01PX")).ToString();
                    row["MX01PX"] = dr.GetValue(dr.GetOrdinal("MX01PX")).ToString();
                    row["PG01PX"] = dr.GetValue(dr.GetOrdinal("PG01PX")).ToString();
                    row["TA01PX"] = dr.GetValue(dr.GetOrdinal("TA01PX")).ToString();
                    row["FG01PX"] = FG01PX;
                    row["NM02PX"] = dr.GetValue(dr.GetOrdinal("NM02PX")).ToString();
                    row["MX02PX"] = dr.GetValue(dr.GetOrdinal("MX02PX")).ToString();
                    row["PG02PX"] = dr.GetValue(dr.GetOrdinal("PG02PX")).ToString();
                    row["TA02PX"] = dr.GetValue(dr.GetOrdinal("TA02PX")).ToString();
                    row["FG02PX"] = FG02PX;
                    row["NM03PX"] = dr.GetValue(dr.GetOrdinal("NM03PX")).ToString();
                    row["MX03PX"] = dr.GetValue(dr.GetOrdinal("MX03PX")).ToString();
                    row["PG03PX"] = dr.GetValue(dr.GetOrdinal("PG03PX")).ToString();
                    row["TA03PX"] = dr.GetValue(dr.GetOrdinal("TA03PX")).ToString();
                    row["FG03PX"] = FG03PX;
                    row["NM04PX"] = dr.GetValue(dr.GetOrdinal("NM04PX")).ToString();
                    row["MX04PX"] = dr.GetValue(dr.GetOrdinal("MX04PX")).ToString();
                    row["PG04PX"] = dr.GetValue(dr.GetOrdinal("PG04PX")).ToString();
                    row["TA04PX"] = dr.GetValue(dr.GetOrdinal("TA04PX")).ToString();
                    row["FG04PX"] = FG04PX;
                    row["NM05PX"] = dr.GetValue(dr.GetOrdinal("NM05PX")).ToString();
                    row["MX05PX"] = dr.GetValue(dr.GetOrdinal("MX05PX")).ToString();
                    row["PG05PX"] = dr.GetValue(dr.GetOrdinal("PG05PX")).ToString();
                    row["TA05PX"] = dr.GetValue(dr.GetOrdinal("TA05PX")).ToString();
                    row["FG05PX"] = FG05PX;
                    row["NM06PX"] = dr.GetValue(dr.GetOrdinal("NM06PX")).ToString();
                    row["MX06PX"] = dr.GetValue(dr.GetOrdinal("MX06PX")).ToString();
                    row["PG06PX"] = dr.GetValue(dr.GetOrdinal("PG06PX")).ToString();
                    row["TA06PX"] = dr.GetValue(dr.GetOrdinal("TA06PX")).ToString();
                    row["FG06PX"] = FG06PX;
                    row["NM07PX"] = dr.GetValue(dr.GetOrdinal("NM07PX")).ToString();
                    row["MX07PX"] = dr.GetValue(dr.GetOrdinal("MX07PX")).ToString();
                    row["PG07PX"] = dr.GetValue(dr.GetOrdinal("PG07PX")).ToString();
                    row["TA07PX"] = dr.GetValue(dr.GetOrdinal("TA07PX")).ToString();
                    row["FG07PX"] = FG07PX;
                    row["NM08PX"] = dr.GetValue(dr.GetOrdinal("NM08PX")).ToString();
                    row["MX08PX"] = dr.GetValue(dr.GetOrdinal("MX08PX")).ToString();
                    row["PG08PX"] = dr.GetValue(dr.GetOrdinal("PG08PX")).ToString();
                    row["TA08PX"] = dr.GetValue(dr.GetOrdinal("TA08PX")).ToString();
                    row["FG08PX"] = FG08PX;
                    row["NM09PX"] = dr.GetValue(dr.GetOrdinal("NM09PX")).ToString();
                    row["PG09PX"] = dr.GetValue(dr.GetOrdinal("PG09PX")).ToString();
                    row["FG09PX"] = FG09PX;
                    row["NM10PX"] = dr.GetValue(dr.GetOrdinal("NM10PX")).ToString();
                    row["PG10PX"] = dr.GetValue(dr.GetOrdinal("PG10PX")).ToString();
                    row["FG10PX"] = FG10PX;
                    row["NM11PX"] = dr.GetValue(dr.GetOrdinal("NM11PX")).ToString();
                    row["PG11PX"] = dr.GetValue(dr.GetOrdinal("PG11PX")).ToString();
                    row["FG11PX"] = FG11PX;
                    row["NM12PX"] = dr.GetValue(dr.GetOrdinal("NM12PX")).ToString();
                    row["PG12PX"] = dr.GetValue(dr.GetOrdinal("PG12PX")).ToString();
                    row["FG12PX"] = FG12PX;

                    this.dtGLMX2.Rows.Add(row);

                    result = true;
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            return (result);
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la zona (al dar de alta una zona)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            //this.txtNombre.Enabled = valor;
            //this.txtNombreExt.Enabled = valor;

            for (int i = 0; i < this.radPageViewPageCampos.Controls.Count; i++)
            {
                this.radPageViewPageCampos.Controls[i].Enabled = valor;
            }

            for (int i = 0; i < this.radPageViewPageCamposExt.Controls.Count; i++)
            {
                this.radPageViewPageCamposExt.Controls[i].Enabled = valor;
            }
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. La compañía está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            this.radButtonSave.Enabled = false;
            this.radButtonCrearGruposCtas.Enabled = false;

            this.radToggleSwitchEstadoActiva.Enabled = false;

            for (int i = 0; i < this.radPageViewPageCampos.Controls.Count; i++)
            {
                this.radPageViewPageCampos.Controls[i].Enabled = false;
            }

            for (int i = 0; i < this.radPageViewPageCamposExt.Controls.Count; i++)
            {
                this.radPageViewPageCamposExt.Controls[i].Enabled = false;
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
                //El código de la cuenta ya fue validado al picarlo

                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "- El nombre abreviado no puede estar en blanco \n\r";      //Falta traducir
                    this.txtNombre.Focus();
                }

                int posSeparador;

                //------------- Validar auxiliares  ---------------------
                //string codigoAux1 = this.radButtonElementTipoAux1.Text.Trim();
                //string codigoAux2 = this.radButtonElementTipoAux2.Text.Trim();
                //string codigoAux3 = this.radButtonElementTipoAux3.Text.Trim();
                string codigoAux1 = this.radButtonTextBoxTipoAux1.Text.Trim();
                string codigoAux2 = this.radButtonTextBoxTipoAux2.Text.Trim();
                string codigoAux3 = this.radButtonTextBoxTipoAux3.Text.Trim();

                //Al menos un codigo de auxiliar informado para hacer las validaciones
                if (codigoAux1 != "" || codigoAux2 != "" || codigoAux3 != "")
                {
                    if (codigoAux1 != "")
                    {
                        posSeparador = codigoAux1.IndexOf('-');
                        if (posSeparador != -1) codigoAux1 = codigoAux1.Substring(0, posSeparador - 1).Trim();
                    }

                    if (codigoAux2 != "")
                    {
                        posSeparador = codigoAux2.IndexOf('-');
                        if (posSeparador != -1) codigoAux2= codigoAux2.Substring(0, posSeparador - 1).Trim();
                    }

                    if (codigoAux3 != "")
                    {
                        posSeparador = codigoAux3.IndexOf('-');
                        if (posSeparador != -1) codigoAux3 = codigoAux3.Substring(0, posSeparador - 1).Trim();
                    }

                    if ((codigoAux1 == codigoAux2 && codigoAux1 != "") || (codigoAux1 == codigoAux3 && codigoAux1 != "") || (codigoAux2 == codigoAux3 && codigoAux2 != ""))
                    {
                        errores += "- Código de auxiliar repetido \n\r";      //Falta traducir
                        this.radButtonElementTipoAux1.Focus();
                    }
                    else
                    {
                        if (codigoAux1 != "")
                        {
                            string valCodAux1 = this.ValidarTipoAuxiliar(codigoAux1);
                            if (valCodAux1 != "")
                            {
                                errores += "- " + valCodAux1 + " \n\r";      //Falta traducir
                                this.radButtonElementTipoAux1.Focus();
                            }
                            else this.TAU1MC = codigoAux1;
                        }
                        if (codigoAux2 != "")
                        {
                            string valCodAux2 = this.ValidarTipoAuxiliar(codigoAux2);
                            if (valCodAux2 != "")
                            {
                                errores += "- " + valCodAux2 + " \n\r";      //Falta traducir
                                this.radButtonElementTipoAux2.Focus();
                            }
                            else this.TAU2MC = codigoAux2;
                        }
                        if (codigoAux3 != "")
                        {
                            string valCodAux3 = this.ValidarTipoAuxiliar(codigoAux3);
                            if (valCodAux3 != "")
                            {
                                errores += "- " + valCodAux3 + " \n\r";      //Falta traducir
                                this.radButtonElementTipoAux3.Focus();
                            }
                            else this.TAU3MC = codigoAux3;
                        }
                    }
                }

                //------------- Validar fecha vencimiento ---------------------
                if (this.rbFechaVtoDebe.IsChecked || this.rbFechaVtoHaber.IsChecked)
                {
                    if (this.radToggleSwitchNumDoc.Value == false)
                    {
                        errores += "- Cuenta no usa documento o no usa auxiliar \n\r";      //Falta traducir
                        this.radToggleSwitchNumDoc.Focus();
                    }
                }

                //------------- Validar moneda extranjera ---------------------
                string monedaExt = this.radButtonTextBoxTipoMonedaExt.Text.Trim();
                if (this.radButtonElementTipoMonedaExt.Enabled && monedaExt != "")
                {
                    posSeparador = monedaExt.IndexOf('-');
                    if (posSeparador != -1) monedaExt = monedaExt.Substring(0, posSeparador - 1).Trim();

                    string valMonedaExt = this.ValidarMonedaExt(monedaExt);
                    if (valMonedaExt != "")
                    {
                        errores += "- " + valMonedaExt + " \n\r";      //Falta traducir
                        this.radButtonTextBoxTipoMonedaExt.Focus();
                    }
                    else this.TIMOMC = monedaExt;
                }

                //------------- Validar cuenta de cierre ---------------------
                string cuentaCierre = this.radButtonTextBoxCtaCierre.Text.Trim();
                if (cuentaCierre != "")
                {
                    posSeparador = cuentaCierre.IndexOf('-');
                    if (posSeparador != -1) cuentaCierre = cuentaCierre.Substring(0, posSeparador - 1).Trim();

                    if (cuentaCierre == this.codigo.Trim())
                    {
                        //La cuenta mayor y la cuenta de cierre no pueden ser iguales
                        errores += "Código de cuenta de cierre no válido\n\r";      //Falta traducir
                        this.radButtonTextBoxCtaCierre.Focus();
                    }
                    else
                    {
                        string valCuentaCierre = this.ValidarCuentaCierre(cuentaCierre);
                        if (valCuentaCierre != "")
                        {
                            errores += "- " + valCuentaCierre + " \n\r";      //Falta traducir
                            this.radButtonTextBoxCtaCierre.Focus();
                        }
                        else this.CIERMC = cuentaCierre;
                    }
                }

                //------------- Validar grupo de cuentas ---------------------
                string grupoCtas = this.radButtonTextBoxGrupoCtas.Text.Trim();
                this.codigoGrupoCta = "";

                if (grupoCtas != "")
                {
                    posSeparador = grupoCtas.IndexOf('-');
                    if (posSeparador != -1) grupoCtas = grupoCtas.Substring(0, posSeparador - 1).Trim();

                    string valGrupoCtas = this.ValidarGrupoCuentas(grupoCtas);
                    if (valGrupoCtas != "")
                    {
                        errores += "- " + valGrupoCtas + " \n\r";      //Falta traducir
                        this.radButtonTextBoxGrupoCtas.Focus();
                    }
                    else
                    {
                        this.codigoGrupoCta = grupoCtas;

                        this.TIMOMC = monedaExt;
                    }

                    //Validar autorización sobre grupos de cuentas
                    string elemento = this.codigoPlan + grupoCtas;
                    bool autorizado;
                    if (this.nuevo)
                    {
                        autorizado = aut.Validar(autClaseElementoGLT22, autGrupoGLT22, elemento, autOperAltaGLT22);
                        if (!autorizado)
                        {
                            errores += "- " + "Usuario no autorizado a dar alta a Cuentas de Mayor con este Grupo" + " \n\r";      //Falta traducir
                            this.radButtonTextBoxGrupoCtas.Focus();
                        }
                    }
                    else
                    {
                        autorizado = aut.Validar(autClaseElementoGLT22, autGrupoGLT22, elemento, autOperModificaGLT22);
                        if (!autorizado)
                        {
                            errores += "- " + "Usuario no autorizado a modificar Cuentas de Mayor con este grupo" + " \n\r";      //Falta traducir
                            this.radButtonTextBoxGrupoCtas.Focus();
                        }
                    }
                }

                /*else
                {
                    //Validar autorización sobre grupos de cuentas
                    if (this.tgTexBoxSelGrupoCtas.Tag.ToString().Trim() != "")
                    {
                        string grupoCtasTag = this.tgTexBoxSelGrupoCtas.Tag.ToString();
                        int posSeparadorTag = grupoCtasTag.IndexOf('-');
                        if (posSeparadorTag != -1) grupoCtasTag = grupoCtasTag.Substring(0, posSeparadorTag - 1).Trim();

                        string elemento = this.codigoPlan + grupoCtasTag;
                        bool autorizadoDel = aut.Validar(autClaseElementoGLT22, autGrupoGLT22, elemento, autOperModificaGLT22);
                        if (!autorizadoDel)
                        {
                            errores += "- " + "Usuario no autorizado a modificar Grupos de Cuentas de Mayor" + " \n\r";      //Falta traducir
                            this.tgTexBoxSelGrupoCtas.Textbox.Text = this.tgTexBoxSelGrupoCtas.Tag.ToString();
                            this.codigoGrupoCta = grupoCtasTag;
                            this.tgTexBoxSelGrupoCtas.Textbox.Focus();
                        }
                    }
                }*/

                //------------- Validar las marcas de IVA --------------
                //Si el Número de Id. Tributaria es IVA, es obligatorio que 
                //el Segundo documento y el Tercer importe sean de IVA tambien
                if (this.rbNumIdTribIVA.IsChecked)
                {
                    if (!this.rbSegundoDocIVA.IsChecked || !this.rbTercerImporteIVA.IsChecked)
                    {
                        errores += "- " + "Si el Número Id. Tributaria es IVA, es obligatorio que el Segundo documento y el Tercer importe sean de IVA también" + " \n\r";      //Falta traducir
                    }
                }
                else
                {
                    //Si el Segundo documento o el Tercer importe son de IVA, es obligatorio que
                    //el Número de Id. Tributaria sea de IVA tambien
                    if (this.rbSegundoDocIVA.IsChecked || this.rbTercerImporteIVA.IsChecked)
                    {
                        errores += "- " + "Si el Segundo documento es IVA o Tercer importe es IVA, es obligatorio que el Número Id. Tributaria sea de IVA también" + " \n\r";      //Falta traducir
                    }
                }

                //------------- Validar Decimales en tercer importe --------------
                //Decimales en tercer importe debe ser N si Tercer importe no es S
                if (!this.rbTercerImporteSi.IsChecked)
                {
                    if (!this.rbDecTercerImporteNo.IsChecked)
                    {
                        errores += "- " + "Decimales en tercer importe debe ser N si tercer importe no es S" + " \n\r";      //Falta traducir
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
        /// Valida el código del tipo de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ValidarTipoAuxiliar(string codigoTipoAux)
        {
            string result = "";

            try
            {
                string query = "select count(TAUXMT) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + codigoTipoAux + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = codigoTipoAux + ": Código de tipo de auxiliar no válido\n\r";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = codigoTipoAux + ": Error verificando el código de tipo de auxiliar (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida el código de la moneda extranjera
        /// </summary>
        /// <returns></returns>
        private string ValidarMonedaExt(string codigoMonedaExt)
        {
            string result = "";

            try
            {
                string query = "select count(TIMOME) from " + GlobalVar.PrefijoTablaCG + "GLT03 ";
                query += "where TIMOME = '" + codigoMonedaExt + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Código de moneda extranjera no válido\n\r";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de tipo de auxiliar (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida el código de la cuenta de cierre. Tiene que ser del mismo plan, de tipo 'D' y no puede ser la cuenta actual
        /// </summary>
        /// <returns></returns>
        private string ValidarCuentaCierre(string codigoCuentaCierre)
        {
            string result = "";

            try
            {
                string query = "select count(CUENMC) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                query += "where TIPLMC = '" + this.codigoPlan + "' and TCUEMC = 'D' and ";
                query += "CUENMC = '" + codigoCuentaCierre + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Código de cuenta de cierre no válido\n\r";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de cuenta de cierre (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida el código del grupo de cuentas
        /// </summary>
        /// <returns></returns>
        private string ValidarGrupoCuentas(string codigoGrupoCuentas)
        {
            string result = "";

            try
            {
                string query = "select count(GRCTGC) from " + GlobalVar.PrefijoTablaCG + "GLT22 ";
                query += "where TIPLGC = '" + this.codigoPlan + "' and GRCTGC = '" + codigoGrupoCuentas + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = "Código de grupo de cuentas no válido\n\r";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error verificando el código de grupo de cuentas (" + ex.Message + ")";
            }

            return (result);
        }


        private string DatosFormToTabla()
        {
            string result = "";

            try
            {
                
                string plan = this.codigoPlan;
                
                int posSeparador = plan.IndexOf(separadorDesc);
                if (posSeparador != -1) plan = plan.Substring(0, posSeparador - 1).Trim();

                if (this.txtNombreExt.Text.Trim() == "") this.txtNombreExt.Text = this.txtNombre.Text;

                int nivelCuenta = 0;
                try
                {
                    if (this.estructuraPadreCuentas != null)
                    {
                        nivelCuenta = Convert.ToInt16(this.estructuraPadreCuentas[0]);
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Dar de alta a la cuenta en la tabla del maestro de cuentas de mayor (GLM03)
                STATMC = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";
                TIPLMC = plan;
                CUENMC = this.codigo;
                TCUEMC = this.tipoCuenta;
                NOABMC = this.txtNombre.Text.Trim();
                NIVEMC = nivelCuenta.ToString();        //Nivel de la cuenta  ¿?¿?¿?

                if (this.rbDigReservDocNo.IsChecked) ADICMC = "N";
                else if (this.rbDigReservDoc0.IsChecked) ADICMC = "0";
                else if (this.rbDigReservDoc1.IsChecked) ADICMC = "1";
                else if (this.rbDigReservDoc2.IsChecked) ADICMC = "2";

                SASIMC = this.cmbNivelSegAsiento.Text.Trim();
                if (SASIMC == "") SASIMC = "0";

                SCONMC = this.cmbNivelSegConsultaList.Text.Trim();
                if (SCONMC == "") SCONMC = "0";

                if (this.rbFechaVtoDebe.IsChecked) FEVEMC = "D";
                else if (this.rbFechaVtoHaber.IsChecked) FEVEMC = "H";
                else FEVEMC = "N";

                if (this.rbSegundoDocSi.IsChecked) NDDOMC = "S";
                else if (this.rbSegundoDocIVA.IsChecked) NDDOMC = "R";
                else NDDOMC = "N";

                if (this.rbTercerImporteSi.IsChecked) TERMMC = "S";
                else if (this.rbTercerImporteIVA.IsChecked) TERMMC = "R";
                else TERMMC = "N";

                /*   Ya tienen el valor correcto por la validación
                CIERMC = " ";
                TIMOMC = " ";
                TAU1MC = " ";
                TAU2MC = " ";
                TAU3MC = " ";    */

                TDOCMC = this.radToggleSwitchNumDoc.Value ? "S" : "N";
                GRUPMC = this.txtReservada1Letra.Text;
                DEAUMC = this.radToggleSwitchDetalleAuxiliar.Value ? "S" : "N";

                NOLAAD = this.txtNombreExt.Text.Trim();
                if (NOLAAD == "") NOLAAD = NOABMC;

                if (this.rbNumIdTribNo.IsChecked) RNITMC = "N";
                else if (this.rbNumIdTribIVA.IsChecked) RNITMC = "R";
                else if (this.rbNumIdTribNITD.IsChecked) RNITMC = "D";
                else if (this.rbNumIdTribNITH.IsChecked) RNITMC = "H";
                else if (this.rbNumIdTribNITT.IsChecked) RNITMC = "T";

                CNITMC = this.txtCodigoConcepto.Text.Trim();
                if (CNITMC == "") CNITMC = " ";

                if (this.rbDecTercerImporteSi.IsChecked) MASCMC = "S";
                else if (this.rbDecTercerImporteSegunCia.IsChecked) MASCMC = "C";
                else MASCMC = "N";

                CEDTMC = " ";       //Formatear la cuenta  (se hace en el momento de insertar)

                //Fecha del sistema en Formato CG
                string fecha = utiles.FechaToFormatoCG(System.DateTime.Now, true).ToString();
                FCRTMC = fecha;       //Fecha del alta o la modificacion
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error recuperando los datos del formulario (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Dar de alta a una cuenta de mayor de detalle
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                //Recuperar los valores del formulario
                string resultForm = DatosFormToTabla();

                string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codigoPlan);

                string error = "";
                int nivel = 0;
                //Formatear Cuenta
                string cuentaFormateada = utilesCG.CuentaFormatear(CUENMC, this.codigoPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);

                CEDTMC = cuentaFormateada;
                NIVEMC = nivel.ToString();

                //Insertar en la GLM03
                result = this.InsertarGLM03(CUENMC, cuentaFormateada, nivel);

                if (result == "")
                {
                    //Completar con las cuentas a ultimo nivel
                    string errores = "";
                    //string[] cuentas = utilesCG.CuentaCompletarNiveles("A", "Z", estructuraMascaraPlan[0], ref errores);
                    string[] cuentas = utilesCG.CuentaCompletarNiveles(CUENMC, this.codigoPlan, estructuraMascaraPlan[0], ref errores);
                    int cantCuentas = 0;
                    
                    if (cuentas[0] != "") 
                    {
                        try 
                        {
                            cantCuentas = Convert.ToInt16(cuentas[0]);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    string cuentaAux = "";
                    string cuentaUltimoNivel = "";
                    for (int i = 0; i < cantCuentas; i++)
                    {
                        cuentaAux = cuentas[i + 2];
                        //Insertar en la GLM03
                        if (cuentaAux != "")
                        {
                            nivel++;
                            result = this.InsertarGLM03(cuentaAux, cuentaFormateada, nivel);
                        }
                        cuentaUltimoNivel = cuentaAux;
                    }

                    if ( (this.planTieneCamposExt  && cuentaUltimoNivel != "") || (cantCuentas == 0) )
                    {
                        //Insertar en la tabla de campos extendidos
                        this.InsertarActualizarGLMX3();
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
        /// Inserta un registro en la tabla GLM03 y en la GLF03
        /// </summary>
        /// <param name="cuenta">cuenta a insertar en la GLM03</param>
        /// <param name="estructuraMascaraPlan">estructura y máscara de edición del plan</param>
        /// <returns></returns>
        ///private string InsertarGLM03(string cuenta, string[] estructuraMascaraPlan)
        private string InsertarGLM03(string cuenta, string cuentaFormateada, int nivel)
        {
            string result = "";

            try
            {
                ///string error = "";
                ///int nivel = 0;
                //Formatear Cuenta
                ///string cuentaFormateada = utilesCG.CuentaFormatear(cuenta, this.codigoPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);

                CEDTMC = cuentaFormateada;   
                NIVEMC = nivel.ToString();

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM03";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", "; 
                query += "STATMC, TIPLMC, CUENMC, TCUEMC, NOABMC, NIVEMC, CIERMC, ADICMC, SASIMC, SCONMC, FEVEMC, NDDOMC, TERMMC, TIMOMC, ";
                query += "TAU1MC, TAU2MC, TAU3MC, TDOCMC, GRUPMC, DEAUMC, NOLAAD, RNITMC, CNITMC, MASCMC, CEDTMC, FCRTMC) ";
                query += "values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, "; 
                query += "'" + STATMC + "', '" + TIPLMC + "', '" + cuenta + "', '" + TCUEMC + "', '" + NOABMC + "', " + NIVEMC + ", '";
                query += CIERMC + "', '" + ADICMC + "', " + SASIMC + ", " + SCONMC + ", '" + FEVEMC + "', '" + NDDOMC + "', '";
                query += TERMMC + "', '" + TIMOMC + "', '" + TAU1MC + "', '" + TAU2MC + "', '" + TAU3MC + "', '" + TDOCMC + "', '";
                query += GRUPMC + "', '" + DEAUMC + "', '" + NOLAAD + "', '" + RNITMC + "', '" + CNITMC + "', '" + MASCMC + "', '";
                query += CEDTMC + "', " + FCRTMC + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Insertar en la GLF03
                nombreTabla = GlobalVar.PrefijoTablaCG + "GLF03";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", "; 
                query += "TIPLBG, CUENBG, SAMDBG) ";
                query += "values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, "; 
                query += "'" + TIPLMC + "', '" + cuenta + "', " + FCRTMC + ")";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Insertar en la GLMX3
                //this.InsertarGLMX3(TIPLMC, cuenta);
                //this.InsertarActualizarGLMX3();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Inserta un registro en la tabla GLMX3
        /// </summary>
        /// <param name="plan">plan a insertar en la GLMX3</param>
        /// <param name="cuenta">cuenta a insertar en la GLMX3</param>
        /// <returns></returns>
        private string InsertarGLMX3(string plan, string cuenta)
        {
            string FGPRMX = "0";
            if ((this.rbReqCampo1Obligatorio.IsChecked) && (this.rbReqCampo1Obligatorio.Visible)) FGPRMX = "1";
            if ((this.rbReqCampo1NoObligatorio.IsChecked) && (this.rbReqCampo1NoObligatorio.Visible)) FGPRMX = "2";
            string FGFAMX = "0";
            if ((this.rbReqCampo2Obligatorio.IsChecked) && (this.rbReqCampo2Obligatorio.Visible)) FGFAMX = "1";
            if ((this.rbReqCampo2NoObligatorio.IsChecked) && (this.rbReqCampo2NoObligatorio.Visible)) FGFAMX = "2";
            string FGFRMX = "0";
            if ((this.rbReqCampo3Obligatorio.IsChecked) && (this.rbReqCampo3Obligatorio.Visible)) FGFRMX = "1";
            if ((this.rbReqCampo3NoObligatorio.IsChecked) && (this.rbReqCampo3NoObligatorio.Visible)) FGFRMX = "2";
            string FGDVMX = "0";
            if ((this.rbReqCampo4Obligatorio.IsChecked) && (this.rbReqCampo4Obligatorio.Visible)) FGDVMX = "1";
            if ((this.rbReqCampo4NoObligatorio.IsChecked) && (this.rbReqCampo4NoObligatorio.Visible)) FGDVMX = "2";
            string FG01MX = "0";
            if ((this.rbReqCampo5Obligatorio.IsChecked) && (this.rbReqCampo5Obligatorio.Visible)) FG01MX = "1";
            if ((this.rbReqCampo5NoObligatorio.IsChecked) && (this.rbReqCampo5NoObligatorio.Visible)) FG01MX = "2";
            string FG02MX = "0";
            if ((this.rbReqCampo6Obligatorio.IsChecked) && (this.rbReqCampo6Obligatorio.Visible)) FG02MX = "1";
            if ((this.rbReqCampo6NoObligatorio.IsChecked) && (this.rbReqCampo6NoObligatorio.Visible)) FG02MX = "2";
            string FG03MX = "0";
            if ((this.rbReqCampo7Obligatorio.IsChecked) && (this.rbReqCampo7Obligatorio.Visible)) FG03MX = "1";
            if ((this.rbReqCampo7NoObligatorio.IsChecked) && (this.rbReqCampo7NoObligatorio.Visible)) FG03MX = "2";
            string FG04MX = "0";
            if ((this.rbReqCampo8Obligatorio.IsChecked) && (this.rbReqCampo8Obligatorio.Visible)) FG04MX = "1";
            if ((this.rbReqCampo8NoObligatorio.IsChecked) && (this.rbReqCampo8NoObligatorio.Visible)) FG04MX = "2";
            string FG05MX = "0";
            if ((this.rbReqCampo9Obligatorio.IsChecked) && (this.rbReqCampo9Obligatorio.Visible)) FG05MX = "1";
            if ((this.rbReqCampo9NoObligatorio.IsChecked) && (this.rbReqCampo9NoObligatorio.Visible)) FG05MX = "2";
            string FG06MX = "0";
            if ((this.rbReqCampo10Obligatorio.IsChecked) && (this.rbReqCampo10Obligatorio.Visible)) FG06MX = "1";
            if ((this.rbReqCampo10NoObligatorio.IsChecked) && (this.rbReqCampo10NoObligatorio.Visible)) FG06MX = "2";
            string FG07MX = "0";
            if ((this.rbReqCampo11Obligatorio.IsChecked) && (this.rbReqCampo11Obligatorio.Visible)) FG07MX = "1";
            if ((this.rbReqCampo11NoObligatorio.IsChecked) && (this.rbReqCampo11NoObligatorio.Visible)) FG07MX = "2";
            string FG08MX = "0";
            if ((this.rbReqCampo12Obligatorio.IsChecked) && (this.rbReqCampo12Obligatorio.Visible)) FG08MX = "1";
            if ((this.rbReqCampo12NoObligatorio.IsChecked) && (this.rbReqCampo12NoObligatorio.Visible)) FG08MX = "2";
            string FG09MX = "0";
            if ((this.rbReqCampo13Obligatorio.IsChecked) && (this.rbReqCampo13Obligatorio.Visible)) FG09MX = "1";
            if ((this.rbReqCampo13NoObligatorio.IsChecked) && (this.rbReqCampo13NoObligatorio.Visible)) FG09MX = "2";
            string FG10MX = "0";
            if ((this.rbReqCampo14Obligatorio.IsChecked) && (this.rbReqCampo14Obligatorio.Visible)) FG10MX = "1";
            if ((this.rbReqCampo14NoObligatorio.IsChecked) && (this.rbReqCampo14NoObligatorio.Visible)) FG10MX = "2";
            string FG11MX = "0";
            if ((this.rbReqCampo15Obligatorio.IsChecked) && (this.rbReqCampo15Obligatorio.Visible)) FG11MX = "1";
            if ((this.rbReqCampo15NoObligatorio.IsChecked) && (this.rbReqCampo15NoObligatorio.Visible)) FG11MX = "2";
            string FG12MX = "0";
            if ((this.rbReqCampo16Obligatorio.IsChecked) && (this.rbReqCampo16Obligatorio.Visible)) FG12MX = "1";
            if ((this.rbReqCampo16NoObligatorio.IsChecked) && (this.rbReqCampo16NoObligatorio.Visible)) FG12MX = "2";

            string result = "";

            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLMX3";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TIPLMX, CUENMX, GRCTMX, FGPRMX, FGFAMX, FGFRMX, FGDVMX, FG01MX, FG02MX, FG03MX, FG04MX, FG05MX, FG06MX, ";
                query += "FG07MX, FG08MX, FG09MX, FG10MX, FG11MX, FG12MX) ";
                query += "values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + plan + "', '" + cuenta + "', '" + this.codigoGrupoCta + "', '";
                query += FGPRMX + "', '" + FGFAMX + "', '" + FGFRMX + "', '" + FGDVMX + "', '"+ FG01MX + "', '" + FG02MX;
                query += "', '" + FG03MX + "', '" + FG04MX + "', '" + FG05MX + "', '" + FG06MX + "', '" + FG07MX + "', '" + FG08MX;
                query += "', '" + FG09MX + "', '" + FG10MX + "', '" + FG11MX + "', '" + FG12MX + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualiza un registro en la tabla GLMX3
        /// </summary>
        /// <param name="plan">plan a insertar en la GLMX3</param>
        /// <param name="cuenta">cuenta a insertar en la GLMX3</param>
        /// <returns></returns>
        private string ActualizaGLMX3(string plan, string cuenta)
        {
            string FGPRMX = "0";
            if ((this.rbReqCampo1Obligatorio.IsChecked) && (this.rbReqCampo1Obligatorio.Visible)) FGPRMX = "1";
            if ((this.rbReqCampo1NoObligatorio.IsChecked) && (this.rbReqCampo1NoObligatorio.Visible)) FGPRMX = "2";
            string FGFAMX = "0";
            if ((this.rbReqCampo2Obligatorio.IsChecked) && (this.rbReqCampo2Obligatorio.Visible)) FGFAMX = "1";
            if ((this.rbReqCampo2NoObligatorio.IsChecked) && (this.rbReqCampo2NoObligatorio.Visible)) FGFAMX = "2";
            string FGFRMX = "0";
            if ((this.rbReqCampo3Obligatorio.IsChecked) && (this.rbReqCampo3Obligatorio.Visible)) FGFRMX = "1";
            if ((this.rbReqCampo3NoObligatorio.IsChecked) && (this.rbReqCampo3NoObligatorio.Visible)) FGFRMX = "2";
            string FGDVMX = "0";
            if ((this.rbReqCampo4Obligatorio.IsChecked) && (this.rbReqCampo4Obligatorio.Visible)) FGDVMX = "1";
            if ((this.rbReqCampo4NoObligatorio.IsChecked) && (this.rbReqCampo4NoObligatorio.Visible)) FGDVMX = "2";
            string FG01MX = "0";
            if ((this.rbReqCampo5Obligatorio.IsChecked) && (this.rbReqCampo5Obligatorio.Visible)) FG01MX = "1";
            if ((this.rbReqCampo5NoObligatorio.IsChecked) && (this.rbReqCampo5NoObligatorio.Visible)) FG01MX = "2";
            string FG02MX = "0";
            if ((this.rbReqCampo6Obligatorio.IsChecked) && (this.rbReqCampo6Obligatorio.Visible)) FG02MX = "1";
            if ((this.rbReqCampo6NoObligatorio.IsChecked) && (this.rbReqCampo6NoObligatorio.Visible)) FG02MX = "2";
            string FG03MX = "0";
            if ((this.rbReqCampo7Obligatorio.IsChecked) && (this.rbReqCampo7Obligatorio.Visible)) FG03MX = "1";
            if ((this.rbReqCampo7NoObligatorio.IsChecked) && (this.rbReqCampo7NoObligatorio.Visible)) FG03MX = "2";
            string FG04MX = "0";
            if ((this.rbReqCampo8Obligatorio.IsChecked) && (this.rbReqCampo8Obligatorio.Visible)) FG04MX = "1";
            if ((this.rbReqCampo8NoObligatorio.IsChecked) && (this.rbReqCampo8NoObligatorio.Visible)) FG04MX = "2";
            string FG05MX = "0";
            if ((this.rbReqCampo9Obligatorio.IsChecked) && (this.rbReqCampo9Obligatorio.Visible)) FG05MX = "1";
            if ((this.rbReqCampo9NoObligatorio.IsChecked) && (this.rbReqCampo9NoObligatorio.Visible)) FG05MX = "2";
            string FG06MX = "0";
            if ((this.rbReqCampo10Obligatorio.IsChecked) && (this.rbReqCampo10Obligatorio.Visible)) FG06MX = "1";
            if ((this.rbReqCampo10NoObligatorio.IsChecked) && (this.rbReqCampo10NoObligatorio.Visible)) FG06MX = "2";
            string FG07MX = "0";
            if ((this.rbReqCampo11Obligatorio.IsChecked) && (this.rbReqCampo11Obligatorio.Visible)) FG07MX = "1";
            if ((this.rbReqCampo11NoObligatorio.IsChecked) && (this.rbReqCampo11NoObligatorio.Visible)) FG07MX = "2";
            string FG08MX = "0";
            if ((this.rbReqCampo12Obligatorio.IsChecked) && (this.rbReqCampo12Obligatorio.Visible)) FG08MX = "1";
            if ((this.rbReqCampo12NoObligatorio.IsChecked) && (this.rbReqCampo12NoObligatorio.Visible)) FG08MX = "2";
            string FG09MX = "0";
            if ((this.rbReqCampo13Obligatorio.IsChecked) && (this.rbReqCampo13Obligatorio.Visible)) FG09MX = "1";
            if ((this.rbReqCampo13NoObligatorio.IsChecked) && (this.rbReqCampo13NoObligatorio.Visible)) FG09MX = "2";
            string FG10MX = "0";
            if ((this.rbReqCampo14Obligatorio.IsChecked) && (this.rbReqCampo14Obligatorio.Visible)) FG10MX = "1";
            if ((this.rbReqCampo14NoObligatorio.IsChecked) && (this.rbReqCampo14NoObligatorio.Visible)) FG10MX = "2";
            string FG11MX = "0";
            if ((this.rbReqCampo15Obligatorio.IsChecked) && (this.rbReqCampo15Obligatorio.Visible)) FG11MX = "1";
            if ((this.rbReqCampo15NoObligatorio.IsChecked) && (this.rbReqCampo15NoObligatorio.Visible)) FG11MX = "2";
            string FG12MX = "0";
            if ((this.rbReqCampo16Obligatorio.IsChecked) && (this.rbReqCampo16Obligatorio.Visible)) FG12MX = "1";
            if ((this.rbReqCampo16NoObligatorio.IsChecked) && (this.rbReqCampo16NoObligatorio.Visible)) FG12MX = "2";
            string result = "";

            try
            {
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                query += "set GRCTMX = '" + this.codigoGrupoCta;
                query += "', FGPRMX = '" + FGPRMX + "', FGFAMX = '" + FGFAMX + "', FGFRMX = '" + FGFRMX + "', FGDVMX = '" + FGDVMX;
                query += "', FG01MX = '" + FG01MX + "', FG02MX = '" + FG02MX + "', FG03MX = '" + FG03MX + "', FG04MX = '" + FG04MX;
                query += "', FG05MX = '" + FG05MX + "', FG06MX = '" + FG06MX + "', FG07MX = '" + FG07MX + "', FG08MX = '" + FG08MX;
                query += "', FG09MX = '" + FG09MX + "', FG10MX = '" + FG10MX + "', FG11MX = '" + FG11MX + "', FG12MX = '" + FG12MX;
                query += "' where TIPLMX = '" + plan + "' and CUENMX = '" + cuenta + "'";
                //and GRCTMX = '" + grupoInicial + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una cuenta de mayor de detalle
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                //Recuperar los valores del formulario
                string resultForm = DatosFormToTabla();

                string[] estructuraMascaraPlan = utilesCG.ObtenerEstructuraMascaraDadoPlan(this.codigoPlan);

                //Fecha del sistema en Formato CG
                string fecha = utiles.FechaToFormatoCG(System.DateTime.Now, true).ToString();

                int nivel = 0;
                string error = "";
                string cuentaFormateada = utilesCG.CuentaFormatear(CUENMC, this.codigoPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);

                //Actualizar un registro en la GLM03
                result = this.ActualizarGLM03(CUENMC, cuentaFormateada, nivel);

                if (result == "")
                {
                    //Completar con las cuentas a ultimo nivel
                    string errores = "";
                    //string[] cuentas = utilesCG.CuentaCompletarNiveles("A", "Z", estructuraMascaraPlan[0], ref errores);
                    string[] cuentas = utilesCG.CuentaCompletarNiveles(CUENMC, this.codigoPlan, estructuraMascaraPlan[0], ref errores);
                    int cantCuentas = 0;

                    if (cuentas[0] != "")
                    {
                        try
                        {
                            cantCuentas = Convert.ToInt16(cuentas[0]);
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    string cuentaAux = "";
                    for (int i = 0; i < cantCuentas; i++)
                    {
                        cuentaAux = cuentas[i + 2];
                        //Actualizar un registro en la GLM03
                        if (cuentaAux != "") result = this.ActualizarGLM03(cuentaAux, cuentaFormateada, nivel);
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
        /// Actualizar un registro en la tabla GLM03 y en la GLF03
        /// </summary>
        /// <param name="cuenta">cuenta a insertar en la GLM03</param>
        /// <param name="estructuraMascaraPlan">estructura y máscara de edición del plan</param>
        /// <returns></returns>
        ///SMR private string ActualizarGLM03(string cuenta, string[] estructuraMascaraPlan)
        private string ActualizarGLM03(string cuenta, string cuentaFormateada, int nivel)
        {
            string result = "";

            try
            {
                //string error = "";
                ///int nivel = 0;
                //Formatear Cuenta
                ///string cuentaFormateada = utilesCG.CuentaFormatear(cuenta, this.codigoPlan, estructuraMascaraPlan[0].ToString(), estructuraMascaraPlan[1].ToString(), ref error, ref nivel);
                CEDTMC = cuentaFormateada;
                //NIVEMC = nivel.ToString();
                //FCRTMC = fecha;

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM03 set ";
                query += "STATMC = '" + STATMC + "', TIPLMC = '" + TIPLMC + "', CUENMC = '" + cuenta + "', ";
                query += "TCUEMC = '" + TCUEMC + "', NOABMC = '" + NOABMC + "', ";
                query += "CIERMC = '" + CIERMC + "', ";
                query += "ADICMC = '" + ADICMC + "', SASIMC = " + SASIMC + ", ";
                query += "SCONMC = " + SCONMC + ", FEVEMC = '" + FEVEMC + "', ";
                query += "NDDOMC = '" + NDDOMC + "', TERMMC = '" + TERMMC + "', ";
                query += "TIMOMC = '" + TIMOMC + "', TAU1MC = '" + TAU1MC + "', ";
                query += "TAU2MC = '" + TAU2MC + "', TAU3MC = '" + TAU3MC + "', ";
                query += "TDOCMC = '" + TDOCMC + "', GRUPMC = '" + GRUPMC + "', ";
                query += "DEAUMC = '" + DEAUMC + "', NOLAAD = '" + NOLAAD + "', ";
                query += "RNITMC = '" + RNITMC + "', CNITMC = '" + CNITMC + "', ";
                query += "MASCMC = '" + MASCMC + "', CEDTMC = '" + CEDTMC + "' ";
                query += "where TIPLMC = '" + TIPLMC + "' and CUENMC = '" + cuenta + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Actualizar en la GLF03
                query = "update " + GlobalVar.PrefijoTablaCG + "GLF03 set ";
                query += "SAMDBG = " + FCRTMC + " ";
                query += "where TIPLBG = '" + this.codigo.Trim() + "' and CUENBG = '" + cuenta + "'";
                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);


                //Actualizar en la GLMX3
                this.InsertarActualizarGLMX3();
                                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (result);
        }

        private void InsertarActualizarGLMX3()
        {
            IDataReader dr = null;
            try
            {
                //Por el plan y la cuenta a último nivel. Si existe grupo asociado actualizarlo, sino insertarlo.
                string cuentaDetalleUltimoNivel = "";

                string errorMsg = "";
                DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivel(this.codigo, this.codigoPlan, ref errorMsg);

                if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0)
                {
                    cuentaDetalleUltimoNivel = dtCtasUltimoNivel.Rows[dtCtasUltimoNivel.Rows.Count - 1]["CUENMC"].ToString().Trim();
                }

                if (cuentaDetalleUltimoNivel != "")
                {
                    //Verificar que exista la tabla GLMX3
                    bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX3");

                    if (!existeTabla) return;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX = '" + this.codigoPlan + "' and ";
                    query += "CUENMX = '" + cuentaDetalleUltimoNivel + "' ";

                    bool existeReg = false;

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read()) existeReg = true;

                    dr.Close();

                    if (existeReg) this.ActualizaGLMX3(this.codigoPlan, cuentaDetalleUltimoNivel);
                    else this.InsertarGLMX3(this.codigoPlan, cuentaDetalleUltimoNivel);    
                 }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verifica si la cuenta de mayor está siendo uitlizada en saldos o en asientos contables
        /// </summary>
        /// <returns></returns>
        private bool CuentaMayorEnUso()
        {
            bool result = true;
            try
            {
                //Buscar si la cuenta tiene saldos
                string query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "PRH01 ";
                query += "where TIPLH1 = '" + this.codigoPlan + "' and CUENH1 like '" + this.codigo.Trim() + "%'";

                int registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (registros > 0) return (result);

                //Buscar si la cuenta tiene asientos contables
                query = "select count (*) from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where TIPLDT = '" + this.codigoPlan + "' and CUENDT like '" + this.codigo.Trim() + "%'";

                registros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (registros <= 0) result = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCuentaMayor.Tag = this.txtCuentaMayor.Text;
            this.txtNombre.Tag = this.txtNombre.Text;
            this.txtNombreExt.Tag = this.txtNombreExt.Text;
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;

            this.radButtonTextBoxTipoAux1.Tag = this.radButtonTextBoxTipoAux1.Text;
            this.radButtonTextBoxTipoAux2.Tag = this.radButtonTextBoxTipoAux2.Text;
            this.radButtonTextBoxTipoAux3.Tag = this.radButtonTextBoxTipoAux3.Text;

            this.radToggleSwitchNumDoc.Tag = this.radToggleSwitchNumDoc.Value;

            if (this.rbFechaVtoNo.IsChecked) this.rbFechaVtoNo.Tag = true;
            else this.rbFechaVtoNo.Tag = false;

            if (this.rbFechaVtoDebe.IsChecked) this.rbFechaVtoDebe.Tag = true;
            else this.rbFechaVtoDebe.Tag = false;

            if (this.rbFechaVtoHaber.IsChecked) this.rbFechaVtoHaber.Tag = true;
            else this.rbFechaVtoHaber.Tag = false;

            this.radButtonTextBoxTipoMonedaExt.Tag = this.radButtonTextBoxTipoMonedaExt.Text;

            this.cmbNivelSegAsiento.Tag = this.cmbNivelSegAsiento.Text;

            this.cmbNivelSegConsultaList.Tag = this.cmbNivelSegConsultaList.Text;

            this.radToggleSwitchDetalleAuxiliar.Tag = this.radToggleSwitchDetalleAuxiliar.Value;

            this.txtReservada1Letra.Tag = this.txtReservada1Letra.Text;

            if (this.rbNumIdTribNo.IsChecked) this.rbNumIdTribNo.Tag = true;
            else this.rbNumIdTribNo.Tag = false;

            if (this.rbNumIdTribIVA.IsChecked) this.rbNumIdTribIVA.Tag = true;
            else this.rbNumIdTribIVA.Tag = false;

            if (this.rbNumIdTribNITD.IsChecked) this.rbNumIdTribNITD.Tag = true;
            else this.rbNumIdTribNITD.Tag = false;

            if (this.rbNumIdTribNITH.IsChecked) this.rbNumIdTribNITH.Tag = true;
            else this.rbNumIdTribNITH.Tag = false;

            if (this.rbNumIdTribNITT.IsChecked) this.rbNumIdTribNITT.Tag = true;
            else this.rbNumIdTribNITT.Tag = false;

            this.txtCodigoConcepto.Tag = this.txtCodigoConcepto.Text;

            if (this.rbSegundoDocSi.IsChecked) this.rbSegundoDocSi.Tag = true;
            else this.rbSegundoDocSi.Tag = false;

            if (this.rbSegundoDocIVA.IsChecked) this.rbSegundoDocIVA.Tag = true;
            else this.rbSegundoDocIVA.Tag = false;

            if (this.rbSegundoDocNo.IsChecked) this.rbSegundoDocNo.Tag = true;
            else this.rbSegundoDocNo.Tag = false;

            if (this.rbTercerImporteSi.IsChecked) this.rbTercerImporteSi.Tag = true;
            else this.rbTercerImporteSi.Tag = false;

            if (this.rbTercerImporteIVA.IsChecked) this.rbTercerImporteIVA.Tag = true;
            else this.rbTercerImporteIVA.Tag = false;

            if (this.rbTercerImporteNo.IsChecked) this.rbTercerImporteNo.Tag = true;
            else this.rbTercerImporteNo.Tag = false;

            if (this.rbDecTercerImporteSi.IsChecked) this.rbDecTercerImporteSi.Tag = true;
            else this.rbDecTercerImporteSi.Tag = false;

            if (this.rbDecTercerImporteSegunCia.IsChecked) this.rbDecTercerImporteSegunCia.Tag = true;
            else this.rbDecTercerImporteSegunCia.Tag = false;

            if (this.rbDecTercerImporteNo.IsChecked) this.rbDecTercerImporteNo.Tag = true;
            else this.rbDecTercerImporteNo.Tag = false;

            if (this.rbDigReservDoc0.IsChecked) this.rbDigReservDoc0.Tag = true;
            else this.rbDigReservDoc0.Tag = false;

            if (this.rbDigReservDoc1.IsChecked) this.rbDigReservDoc1.Tag = true;
            else this.rbDigReservDoc1.Tag = false;

            if (this.rbDigReservDoc2.IsChecked) this.rbDigReservDoc2.Tag = true;
            else this.rbDigReservDoc2.Tag = false;

            if (this.rbDigReservDocNo.IsChecked) this.rbDigReservDocNo.Tag = true;
            else this.rbDigReservDocNo.Tag = false;

            this.radButtonTextBoxCtaCierre.Tag = this.radButtonTextBoxCtaCierre.Text;

            this.radButtonTextBoxGrupoCtas.Tag = this.radButtonTextBoxGrupoCtas.Text;

            //Campos Extendidos
            if (this.planTieneCamposExt && this.dtGLMX2 != null)
            {
                if (this.rbReqCampo1No.IsChecked) this.rbReqCampo1No.Tag = true;
                else this.rbReqCampo1No.Tag = false;
                if (this.rbReqCampo1Obligatorio.IsChecked) this.rbReqCampo1Obligatorio.Tag = true;
                else this.rbReqCampo1Obligatorio.Tag = false;
                if (this.rbReqCampo1NoObligatorio.IsChecked) this.rbReqCampo1NoObligatorio.Tag = true;
                else this.rbReqCampo1NoObligatorio.Tag = false;

                if (this.rbReqCampo2No.IsChecked) this.rbReqCampo2No.Tag = true;
                else this.rbReqCampo2No.Tag = false;
                if (this.rbReqCampo2Obligatorio.IsChecked) this.rbReqCampo2Obligatorio.Tag = true;
                else this.rbReqCampo2Obligatorio.Tag = false;
                if (this.rbReqCampo2NoObligatorio.IsChecked) this.rbReqCampo2NoObligatorio.Tag = true;
                else this.rbReqCampo2NoObligatorio.Tag = false;

                if (this.rbReqCampo3No.IsChecked) this.rbReqCampo3No.Tag = true;
                else this.rbReqCampo3No.Tag = false;
                if (this.rbReqCampo3Obligatorio.IsChecked) this.rbReqCampo3Obligatorio.Tag = true;
                else this.rbReqCampo3Obligatorio.Tag = false;
                if (this.rbReqCampo3NoObligatorio.IsChecked) this.rbReqCampo3NoObligatorio.Tag = true;
                else this.rbReqCampo3NoObligatorio.Tag = false;

                if (this.rbReqCampo4No.IsChecked) this.rbReqCampo4No.Tag = true;
                else this.rbReqCampo4No.Tag = false;
                if (this.rbReqCampo4Obligatorio.IsChecked) this.rbReqCampo4Obligatorio.Tag = true;
                else this.rbReqCampo4Obligatorio.Tag = false;
                if (this.rbReqCampo4NoObligatorio.IsChecked) this.rbReqCampo4NoObligatorio.Tag = true;
                else this.rbReqCampo4NoObligatorio.Tag = false;

                if (this.rbReqCampo5No.IsChecked) this.rbReqCampo5No.Tag = true;
                else this.rbReqCampo5No.Tag = false;
                if (this.rbReqCampo5Obligatorio.IsChecked) this.rbReqCampo5Obligatorio.Tag = true;
                else this.rbReqCampo5Obligatorio.Tag = false;
                if (this.rbReqCampo5NoObligatorio.IsChecked) this.rbReqCampo5NoObligatorio.Tag = true;
                else this.rbReqCampo5NoObligatorio.Tag = false;

                this.rbReqCampo6No.Tag = this.rbReqCampo6No.IsChecked;
                this.rbReqCampo6Obligatorio.Tag = this.rbReqCampo6Obligatorio.IsChecked;
                this.rbReqCampo6NoObligatorio.Tag = this.rbReqCampo6NoObligatorio.IsChecked;

                this.rbReqCampo7No.Tag = this.rbReqCampo7No.IsChecked;
                this.rbReqCampo7Obligatorio.Tag = this.rbReqCampo7Obligatorio.IsChecked;
                this.rbReqCampo7NoObligatorio.Tag = this.rbReqCampo7NoObligatorio.IsChecked;

                this.rbReqCampo8No.Tag = this.rbReqCampo8No.IsChecked;
                this.rbReqCampo8Obligatorio.Tag = this.rbReqCampo8Obligatorio.IsChecked;
                this.rbReqCampo8NoObligatorio.Tag = this.rbReqCampo8NoObligatorio.IsChecked;

                this.rbReqCampo9No.Tag = this.rbReqCampo9No.IsChecked;
                this.rbReqCampo9Obligatorio.Tag = this.rbReqCampo9Obligatorio.IsChecked;
                this.rbReqCampo9NoObligatorio.Tag = this.rbReqCampo9NoObligatorio.IsChecked;

                this.rbReqCampo10No.Tag = this.rbReqCampo10No.IsChecked;
                this.rbReqCampo10Obligatorio.Tag = this.rbReqCampo10Obligatorio.IsChecked;
                this.rbReqCampo10NoObligatorio.Tag = this.rbReqCampo10NoObligatorio.IsChecked;

                this.rbReqCampo11No.Tag = this.rbReqCampo11No.IsChecked;
                this.rbReqCampo11Obligatorio.Tag = this.rbReqCampo11Obligatorio.IsChecked;
                this.rbReqCampo11NoObligatorio.Tag = this.rbReqCampo11NoObligatorio.IsChecked;

                this.rbReqCampo12No.Tag = this.rbReqCampo12No.IsChecked;
                this.rbReqCampo12Obligatorio.Tag = this.rbReqCampo12Obligatorio.IsChecked;
                this.rbReqCampo12NoObligatorio.Tag = this.rbReqCampo12NoObligatorio.IsChecked;

                this.rbReqCampo13No.Tag = this.rbReqCampo13No.IsChecked;
                this.rbReqCampo13Obligatorio.Tag = this.rbReqCampo13Obligatorio.IsChecked;
                this.rbReqCampo13NoObligatorio.Tag = this.rbReqCampo13NoObligatorio.IsChecked;

                this.rbReqCampo14No.Tag = this.rbReqCampo14No.IsChecked;
                this.rbReqCampo14Obligatorio.Tag = this.rbReqCampo14Obligatorio.IsChecked;
                this.rbReqCampo14NoObligatorio.Tag = this.rbReqCampo14NoObligatorio.IsChecked;

                this.rbReqCampo15No.Tag = this.rbReqCampo15No.IsChecked;
                this.rbReqCampo15Obligatorio.Tag = this.rbReqCampo15Obligatorio.IsChecked;
                this.rbReqCampo15NoObligatorio.Tag = this.rbReqCampo15NoObligatorio.IsChecked;

                this.rbReqCampo16No.Tag = this.rbReqCampo6No.IsChecked;
                this.rbReqCampo16Obligatorio.Tag = this.rbReqCampo16Obligatorio.IsChecked;
                this.rbReqCampo16NoObligatorio.Tag = this.rbReqCampo16NoObligatorio.IsChecked;
            }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCuentaMayor.Tag = "";
            this.txtNombre.Tag = "";
            this.txtNombreExt.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;

            this.radButtonTextBoxTipoAux1.Tag = "";
            this.radButtonTextBoxTipoAux2.Tag = "";
            this.radButtonTextBoxTipoAux3.Tag = "";

            this.radToggleSwitchNumDoc.Value = false;

            this.rbFechaVtoNo.Tag = true;
            this.rbFechaVtoDebe.Tag = false;
            this.rbFechaVtoHaber.Tag = false;

            this.radButtonTextBoxTipoMonedaExt.Tag = "";

            this.cmbNivelSegAsiento.Tag = "";
            this.cmbNivelSegConsultaList.Tag = "";

            this.radToggleSwitchDetalleAuxiliar.Value = false;

            this.txtReservada1Letra.Tag = "";

            this.rbNumIdTribNo.Tag = true;
            this.rbNumIdTribIVA.Tag = false;
            this.rbNumIdTribNITD.Tag = false;
            this.rbNumIdTribNITH.Tag = false;
            this.rbNumIdTribNITT.Tag = false;

            this.txtCodigoConcepto.Tag = "";

            this.rbSegundoDocSi.Tag = false;
            this.rbSegundoDocIVA.Tag = false;
            this.rbSegundoDocNo.Tag = true;

            this.rbTercerImporteSi.Tag = false;
            this.rbTercerImporteIVA.Tag = false;
            this.rbTercerImporteNo.Tag = true;
            
            this.rbDecTercerImporteSi.Tag = false;
            this.rbDecTercerImporteSegunCia.Tag = false;
            this.rbDecTercerImporteNo.Tag = true;
            
            this.rbDigReservDoc0.Tag = false;
            this.rbDigReservDoc1.Tag = false;
            this.rbDigReservDoc2.Tag = false;
            this.rbDigReservDocNo.Tag = true;
            
            this.radButtonTextBoxCtaCierre.Tag = "";

            this.radButtonTextBoxGrupoCtas.Tag = "";

            //Campos Extendidos
            if (this.planTieneCamposExt && this.dtGLMX2 != null)
            {
                this.rbReqCampo1No.Tag = true;
                this.rbReqCampo1Obligatorio.Tag = false;
                this.rbReqCampo1NoObligatorio.Tag = false;

                this.rbReqCampo2No.Tag = true;
                this.rbReqCampo2Obligatorio.Tag = false;
                this.rbReqCampo2NoObligatorio.Tag = false;

                this.rbReqCampo3No.Tag = true;
                this.rbReqCampo3Obligatorio.Tag = false;
                this.rbReqCampo3NoObligatorio.Tag = false;

                this.rbReqCampo4No.Tag = true;
                this.rbReqCampo4Obligatorio.Tag = false;
                this.rbReqCampo4NoObligatorio.Tag = false;

                this.rbReqCampo5No.Tag = true;
                this.rbReqCampo5Obligatorio.Tag = false;
                this.rbReqCampo5NoObligatorio.Tag = false;

                this.rbReqCampo6No.Tag = true;
                this.rbReqCampo6Obligatorio.Tag = false;
                this.rbReqCampo6NoObligatorio.Tag = false;

                this.rbReqCampo7No.Tag = true;
                this.rbReqCampo7Obligatorio.Tag = false;
                this.rbReqCampo7NoObligatorio.Tag = false;

                this.rbReqCampo8No.Tag = true;
                this.rbReqCampo8Obligatorio.Tag = false;
                this.rbReqCampo8NoObligatorio.Tag = false;

                this.rbReqCampo9No.Tag = true;
                this.rbReqCampo9Obligatorio.Tag = false;
                this.rbReqCampo9NoObligatorio.Tag = false;

                this.rbReqCampo10No.Tag = true;
                this.rbReqCampo10Obligatorio.Tag = false;
                this.rbReqCampo10NoObligatorio.Tag = false;

                this.rbReqCampo11No.Tag = true;
                this.rbReqCampo11Obligatorio.Tag = false;
                this.rbReqCampo11NoObligatorio.Tag = false;

                this.rbReqCampo12No.Tag = true;
                this.rbReqCampo12Obligatorio.Tag = false;
                this.rbReqCampo12NoObligatorio.Tag = false;

                this.rbReqCampo13No.Tag = true;
                this.rbReqCampo13Obligatorio.Tag = false;
                this.rbReqCampo13NoObligatorio.Tag = false;

                this.rbReqCampo14No.Tag = true;
                this.rbReqCampo14Obligatorio.Tag = false;
                this.rbReqCampo14NoObligatorio.Tag = false;

                this.rbReqCampo15No.Tag = true;
                this.rbReqCampo15Obligatorio.Tag = false;
                this.rbReqCampo15NoObligatorio.Tag = false;

                this.rbReqCampo16No.Tag = true;
                this.rbReqCampo16Obligatorio.Tag = false;
                this.rbReqCampo16NoObligatorio.Tag = false;
            }
        }

        /// <summary>
        /// Verifica si existen cambios en los valores iniciales y los valores actuales de los campos extendidos
        /// </summary>
        /// <returns></returns>
        private bool CambioValoresCamposExtendidos()
        {
            bool cambios = false;
            try
            {
                if (this.planTieneCamposExt && this.dtGLMX2 != null)
                {
                    if ((this.rbReqCampo1No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo1No.Tag))) ||
                        (this.rbReqCampo1Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo1Obligatorio.Tag))) ||
                        (this.rbReqCampo1NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo1NoObligatorio.Tag))) ||
                        (this.rbReqCampo2No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo2No.Tag))) ||
                        (this.rbReqCampo2Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo2Obligatorio.Tag))) ||
                        (this.rbReqCampo2NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo2NoObligatorio.Tag))) ||
                        (this.rbReqCampo3No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo3No.Tag))) ||
                        (this.rbReqCampo3Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo3Obligatorio.Tag))) ||
                        (this.rbReqCampo3NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo3NoObligatorio.Tag))) ||
                        (this.rbReqCampo4No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo4No.Tag))) ||
                        (this.rbReqCampo4Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo4Obligatorio.Tag))) ||
                        (this.rbReqCampo4NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo4NoObligatorio.Tag))) ||
                        (this.rbReqCampo5No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo5No.Tag))) ||
                        (this.rbReqCampo5Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo5Obligatorio.Tag))) ||
                        (this.rbReqCampo5NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo5NoObligatorio.Tag))) ||
                        (this.rbReqCampo6No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo6No.Tag))) ||
                        (this.rbReqCampo6Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo6Obligatorio.Tag))) ||
                        (this.rbReqCampo6NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo6NoObligatorio.Tag))) ||
                        (this.rbReqCampo7No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo7No.Tag))) ||
                        (this.rbReqCampo7Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo7Obligatorio.Tag))) ||
                        (this.rbReqCampo7NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo7NoObligatorio.Tag))) ||
                        (this.rbReqCampo8No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo8No.Tag))) ||
                        (this.rbReqCampo8Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo8Obligatorio.Tag))) ||
                        (this.rbReqCampo8NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo8NoObligatorio.Tag))) ||
                        (this.rbReqCampo9No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo9No.Tag))) ||
                        (this.rbReqCampo9Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo9Obligatorio.Tag))) ||
                        (this.rbReqCampo9NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo9NoObligatorio.Tag))) ||
                        (this.rbReqCampo10No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo10No.Tag))) ||
                        (this.rbReqCampo10Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo10Obligatorio.Tag))) ||
                        (this.rbReqCampo10NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo10NoObligatorio.Tag))) ||
                        (this.rbReqCampo11No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo11No.Tag))) ||
                        (this.rbReqCampo11Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo11Obligatorio.Tag))) ||
                        (this.rbReqCampo11NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo11NoObligatorio.Tag))) ||
                        (this.rbReqCampo12No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo12No.Tag))) ||
                        (this.rbReqCampo12Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo12Obligatorio.Tag))) ||
                        (this.rbReqCampo12NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo12NoObligatorio.Tag))) ||
                        (this.rbReqCampo13No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo13No.Tag))) ||
                        (this.rbReqCampo13Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo13Obligatorio.Tag))) ||
                        (this.rbReqCampo13NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo13NoObligatorio.Tag))) ||
                        (this.rbReqCampo14No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo14No.Tag))) ||
                        (this.rbReqCampo14Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo14Obligatorio.Tag))) ||
                        (this.rbReqCampo14NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo14NoObligatorio.Tag))) ||
                        (this.rbReqCampo15No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo15No.Tag))) ||
                        (this.rbReqCampo15Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo15Obligatorio.Tag))) ||
                        (this.rbReqCampo15NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo15NoObligatorio.Tag))) ||
                        (this.rbReqCampo16No.IsChecked && !(Convert.ToBoolean(this.rbReqCampo16No.Tag))) ||
                        (this.rbReqCampo16Obligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo16Obligatorio.Tag))) ||
                        (this.rbReqCampo16NoObligatorio.IsChecked && !(Convert.ToBoolean(this.rbReqCampo16NoObligatorio.Tag)))
                        )
                        cambios = true;

                    this.rbReqCampo7No.Tag = this.rbReqCampo7No.IsChecked;
                    this.rbReqCampo7Obligatorio.Tag = this.rbReqCampo7Obligatorio.IsChecked;
                    this.rbReqCampo7NoObligatorio.Tag = this.rbReqCampo7NoObligatorio.IsChecked;

                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (cambios);
        }

        /// <summary>
        /// Graba una cuenta de mayor de detalle
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
        /// Eliminar una cuenta de mayor de detalle
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar la Cuenta de Mayor " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");

            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Eliminar de la cuenta de mayor
                    string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TIPLMC = '" + this.codigoPlan + "' and CUENMC like '" + this.codigo.Trim() + "%' and TCUEMC = 'D'";

                    int cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Eliminar la cuenta de mayor en la tabla de campos extendidos
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX= '" + this.codigoPlan + "' and CUENMX like '" + this.codigo.Trim() + "%' ";

                    cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLM03", this.codigoPlan, this.codigo);

                    //Eliminar de GLF03
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLF03 ";
                    query += "where TIPLBG = '" + this.codigoPlan + "' and CUENBG like '" + this.codigo.Trim() + "%'";

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
        /// Carga el formulario que permite crear grupos de cuentas de mayor
        /// </summary>
        private void CrearGruposCuentas()
        {
            frmMtoGLT22 frmEditGLT22 = new frmMtoGLT22
            {
                Nuevo = true,
                EditarPlan = false,
                GrabarClose = true,
                Codigo = "",
                CodigoPlan = this.codigoPlan,
                FrmPadre = this
            };
            frmEditGLT22.ShowDialog(this);

            if (frmEditGLT22.CodigoDesGrupoMayor != "") this.radButtonTextBoxGrupoCtas.Text = frmEditGLT22.CodigoDesGrupoMayor;
        }

        /// <summary>
        /// Carga el formulario que visualiza los niveles de la cuenta de mayor
        ///
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

        private void TipoAuxiliarSeleccionar(ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxControl)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasTipoAux = new ArrayList
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
                ColumnasCaption = nombreColumnasTipoAux,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this,
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
                radButtonTextBoxControl.Text = result;
                this.ActiveControl = radButtonTextBoxControl;
                radButtonTextBoxControl.Select(0, 0);
                radButtonTextBoxControl.Focus();
            }
        }
        #endregion

        private void frmMtoGLM03Detalle_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
