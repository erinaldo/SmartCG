using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections;
using Telerik.WinControls.UI;
using ObjectModel;
using log4net;
using Telerik.WinControls;

namespace ModComprobantes
{
    public partial class frmCompContAltaEdita : frmPlantilla, IReLocalizable
    {
        //viajen desde el user control hacia el formulario
        public delegate void ActualizaListaComprobantes(ActualizaListaComprobantesArgs e);
        public event ActualizaListaComprobantes ArgSel;

        public class ActualizaListaComprobantesArgs
        {

            public ArrayList Valor { get; protected set; }
            public ActualizaListaComprobantesArgs(ArrayList valor)
            {
                this.Valor = valor;
            }
        }
        protected bool menuLateralExpanded = true;
        protected static int collapseWidth = 0;

        private bool nuevoComprobante;
        private bool batch;
        private string tipomoneda;
        private bool EsNuevo;
        private int row_index;
        private bool importarComprobante;
        private bool edicionLote;
        private bool batchlote;
        private bool batchloteerror;
        private bool edicionLoteError;
        private bool edicionComprobanteGLB01;
        private bool comprobanteSoloConsulta;       //Se utiliza para el caso edicionComprobanteGLB01, para comprobantes contabilizados o aprobados
        private bool nuevoComprobanteGLB01;
        private bool nuevoComprobanteGLB01CabeceraGrabada;  //Se utiliza para indicar si se está trabajando con la opción nuevoComprobanteGLB01 y se ha grabado ya la cabecera
        private string nombreComprobante;
        private string compania;
        private string archivoComprobante;
        private ComprobanteContable comprobanteContableImportar;
        private string[] cmpextendidos;


        private DataSet ds;

        private Telerik.WinControls.UI.RadLabel lblTotalDebe;
        private Telerik.WinControls.UI.RadLabel lblTotalHaber;
        private Telerik.WinControls.UI.RadLabel lblMonedaLocal;
        private Telerik.WinControls.UI.RadLabel lblMonedaExtranjera;
        private Telerik.WinControls.UI.RadLabel lblImporte3;

        private Telerik.WinControls.UI.RadLabel lblMonedaLocal_Debe;
        private Telerik.WinControls.UI.RadLabel lblMonedaLocal_Haber;
        private Telerik.WinControls.UI.RadLabel lblMonedaExtranjera_Debe;
        private Telerik.WinControls.UI.RadLabel lblMonedaExtranjera_Haber;
        private Telerik.WinControls.UI.RadLabel lblImporte3_Debe;
        private Telerik.WinControls.UI.RadLabel lblImporte3_Haber;

        private Telerik.WinControls.UI.RadLabel lblNoApuntes;
        private Telerik.WinControls.UI.RadLabel lblNoApuntes_Valor;

        private const string REVERTIRNo = "N";
        private const string REVERTIRImportes = "S";
        private const string REVERTIRImportesDH = "T";

        //Valores necesarios de la compañía
        private string GLM01_NCIAMG;
        private string GLM01_TITAMG;
        private string GLM01_FELAMG;
        private string GLM01_TIPLMG;

        //Descripción del tipo
        private string GLT06_NOMBTV;

        private const string separadorDesc = "-";

        private string compania_ant;
        private string aapp_ant;
        private string tipo_ant;
        private string nocomp_ant;
        private string fecha_ant;
        

        //TextBox que se utiliza en el DataGridView de Detalles para validar que las monedas y el importe sean sólo numéricos
        TextBox tb;

        private ComprobanteContable comp = null;

        //Comprobantes con campos extendidos
        // private bool extendido = true; //Variable que indica si se trabaja en modo extendido o no
        private bool extendido;
        private string tipoBaseDatosCG = "";
        private DataTable dtGLMX2 = null;
        //private DataTable dtGLMX3 = null;

        private bool showMsgValidacionOk = true;    //Muestra el mensaje de validación Ok cuando no existen errores de validación al acceder a la opción de Validar Comprobante

        private bool gridChange = false;
        private bool dGrabar = false;
        private bool dNoPreguntar = false;
        private bool gridCambiada = false;
        private DataTable dtClase;

        private DataTable dtRevertir;

        public bool nglm01;
        public bool nglm02;
        public bool nglmx2;
        public string compania_cmb;

        public bool NuevoComprobante
        {
            get
            { 
                return (this.nuevoComprobante);
            }
            set
            {
                this.nuevoComprobante = value;
            }
        }
        public bool nGlm02
        {
            get
            {
                return (this.nglm02);
            }
            set
            {
                this.nglm02 = value;
            }
        }
        public bool nGlmx2
        {
            get
            {
                return (this.nglmx2);
            }
            set
            {
                this.nglmx2 = value;
            }
        }
        public bool Batch
        {
            get
            {
                return (this.batch);
            }
            set
            {
                this.batch = value;
            }
        }
        public string[] CmpExtendidos
        {
            get
            {
                return (this.cmpextendidos);
            }
            set
            {
                this.cmpextendidos = value;
            }
        }
        public string TipoMoneda
        {
            get
            {
                return (this.tipomoneda);
            }
            set
            {
                this.tipomoneda = value;
            }
        }
        public int Row_Index
        {
            get
            {
                return (this.row_index);
            }
            set
            {
                this.row_index = value;
            }
        }
        public bool ImportarComprobante
        {
            get
            {
                return (this.importarComprobante);
            }
            set
            {
                this.importarComprobante = value;
            }
        }

        public bool EdicionLote
        {
            get
            {
                return (this.edicionLote);
            }
            set
            {
                this.edicionLote = value;
            }
        }
        public bool BatchLote
        {
            get
            {
                return (this.batchlote);
            }
            set
            {
                this.batchlote = value;
            }
        }
        public bool BatchLoteError
        {
            get
            {
                return (this.batchloteerror);
            }
            set
            {
                this.batchloteerror = value;
            }
        }
        public bool EdicionLoteError
        {
            get
            {
                return (this.edicionLoteError);
            }
            set
            {
                this.edicionLoteError = value;
            }
        }

        public bool EdicionComprobanteGLB01
        {
            get
            {
                return (this.edicionComprobanteGLB01);
            }
            set
            {
                this.edicionComprobanteGLB01 = value;
            }
        }
        
        public bool ComprobanteSoloConsulta
        {
            get
            {
                return (this.comprobanteSoloConsulta);
            }
            set
            {
                this.comprobanteSoloConsulta = value;
            }
        }

        public bool NuevoComprobanteGLB01
        {
            get
            {
                return (this.nuevoComprobanteGLB01);
            }
            set
            {
                this.nuevoComprobanteGLB01 = value;
            }
        }

        public bool NuevoComprobanteGLB01CabeceraGrabada
        {
            get
            {
                return (this.nuevoComprobanteGLB01CabeceraGrabada);
            }
            set
            {
                this.nuevoComprobanteGLB01CabeceraGrabada = value;
            }
        }
               
        public string NombreComprobante
        {
            get
            {
                return (this.nombreComprobante);
            }
            set
            {
                this.nombreComprobante = value;
            }
        }

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
        public string ArchivoComprobante
        {
            get
            {
                return (this.archivoComprobante);
            }
            set
            {
                this.archivoComprobante = value;
            }
        }

        public ComprobanteContable ComprobanteContableImportar
        {
            get
            {
                return (this.comprobanteContableImportar);
            }
            set
            {
                this.comprobanteContableImportar = value;
            }
        }

        public frmCompContAltaEdita()
        {
            InitializeComponent();
            
            //Sólo se utiliza para editar lotes
            this.edicionLote = false;
            //Sólo se utiliza para editar lotes con errores
            this.edicionLoteError = false;

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbCabecera.ElementTree.EnableApplicationThemeName = false;
            this.gbCabecera.ThemeName = "ControlDefault";

            this.gbTotales.ElementTree.EnableApplicationThemeName = false;
            this.gbTotales.ThemeName = "ControlDefault";

            this.grBoxProgressBar.ElementTree.EnableApplicationThemeName = false;
            this.grBoxProgressBar.ThemeName = "ControlDefault";

            this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.InnerColor = Color.Transparent;
            this.radBtnMenuMostrarOcultar.ElementTree.EnableApplicationThemeName = false;
            this.radBtnMenuMostrarOcultar.ThemeName = "Office2013Light";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales(true);
        }

        private void FrmCompContAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Alta / Edita comprobantes contables ");

            EsNuevo = this.nuevoComprobante; // guardo boot para devolución al programa llamada

            if (this.nuevoComprobante) this.gridChange = true;

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Crear Tabla de clases
            this.dtClase = new DataTable();
            dtClase.Columns.Add("valor", typeof(string));
            dtClase.Columns.Add("desc", typeof(string));

            //Crear Tabla de Revertir
            this.dtRevertir = new DataTable();
            dtRevertir.Columns.Add("valor", typeof(string));
            dtRevertir.Columns.Add("desc", typeof(string));

            //Crear el Data Grid
            this.CrearDataGrid();

            //Cargar compañías
            this.FillCompanias();

            //Cargar Tipos
            this.FillTiposComprobantes();

            //Crear el datatimepicker con el formato para las fechas que está parametrizado en el CG
            this.CrearFechaConFormatoCG();
            
            //Crear el desplegable de clase
            this.CrearComboClase();

            //Crear el desplegable de revertir
            this.CrearComboRevertir();

            //Crear las etiquetas para la tabla de totales
            this.CrearTablaTotales();

            if (this.importarComprobante)
            {
                this.CrearTablasDataSetVacias();
                //Inicializar los valores
                this.ImportarDatosComprobante();
                
                // guarda datos cabecera iniciales
                compania_ant = this.comprobanteContableImportar.Cab_compania;
                compania_cmb = this.comprobanteContableImportar.Cab_compania;
                tipo_ant = this.comprobanteContableImportar.Cab_tipo;
                aapp_ant = this.txtMaskAAPP.Value.ToString();
                nocomp_ant = this.comprobanteContableImportar.Cab_noComprobante;
                DateTime fecha_int = Convert.ToDateTime(this.dateTimePickerFecha.Text);
                string fecha_Seditar = utiles.FechaToFormatoCG(fecha_int, false, 6).ToString().Trim();
                fecha_ant = fecha_Seditar.Substring(4, 2) + "/" + fecha_Seditar.Substring(2, 2) +
                            "/" + fecha_Seditar.Substring(0, 2);
                //Calcular los totales
                this.CalcularTotales();

                //Actualizar los atributos TAG de los controles de la cabecera
                this.ActualizaValoresOrigenControles();

                if (this.edicionLote || this.edicionLoteError)
                {
                    utiles.ButtonEnabled(ref this.radButtonRevertir, false);

                    this.radDropDownListRevertir.Enabled = false;

                    if (this.edicionLoteError)
                    {
                        //Validar el comprobante
                        this.radButtonValidar.PerformClick();
                    }
                }
                else
                {
                    if (this.edicionComprobanteGLB01)
                    {
                        //Desactivar el campo compañia
                        //this.cmbCompania.ReadOnly = true;
                        this.cmbCompania.Enabled = false;
                        this.txtMaskAAPP.Enabled = false;
                        this.cmbTipo.Enabled = false;
                        this.dateTimePickerFecha.Enabled = false;
                        this.txtNoComprobante.Enabled = true;
                        this.cmbClase.Enabled = false;
                        this.txtTasa.Enabled = false;
                        this.txtNoComprobante.Enabled = false;
                        this.txtDescripcion.Enabled = false;
                        this.radDropDownListRevertir.Enabled = false;

                        if (this.comprobanteSoloConsulta)
                        {
                            //this.toolStripButtonSelecModelo.Enabled = false;
                            utiles.ButtonEnabled(ref this.radButtonGrabar, false);
                            utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                            utiles.ButtonEnabled(ref this.radButtonValidar, false);
                            utiles.ButtonEnabled(ref this.radButtonValidarErrores, false);
                            utiles.ButtonEnabled(ref this.radButtonGrabarComo, false);
                            utiles.ButtonEnabled(ref this.radButtonImportar, false);

                            this.menuGridButtonAdicionarFila.Enabled = false;
                            this.menuGridButtonBorrar.Enabled = false;
                            this.menuGridButtonCortar.Enabled = false;
                            this.menuGridButtonInsertarFila.Enabled = false;
                            this.menuGridButtonPegar.Enabled = false;
                            this.menuGridButtonSuprimirFila.Enabled = false;
                            this.menuGridButtonReemplazar.Enabled = false;
                            

                            /*this.txtMaskAAPP.ReadOnly = true;
                            this.cmbTipo.ReadOnly = true;
                            this.txtNoComprobante.IsReadOnly = true;
                            this.cmbClase.Enabled = false;
                            this.txtTasa.IsReadOnly = true;*/

                            this.radDropDownListRevertir.Enabled = false;

                            this.dgDetalle.ReadOnly = true;

                            //Guardar los valores iniciales (que son posibles a actualizar)
                            this.dateTimePickerFecha.Tag = this.comprobanteContableImportar.Cab_fecha;
                            this.txtDescripcion.Tag = this.comprobanteContableImportar.Cab_descripcion;

                        }
                        else
                        {
                            if (!this.nuevoComprobante)
                            {
                                utiles.ButtonEnabled(ref this.radButtonImportar, false);
                            }
                                this.txtMaskAAPP.Tag = this.comprobanteContableImportar.Cab_anoperiodo;
                            this.cmbTipo.Tag = this.comprobanteContableImportar.Cab_tipo;
                            this.txtNoComprobante.Tag = this.comprobanteContableImportar.Cab_noComprobante;
                        }

                        //Botón que permite Ver/Editar Comentarios
                        this.btnVerComentarios.Visible = true;
                    }
                }
            }
            else
            if (!this.nuevoComprobante)
            {
                this.CargarDatosComprobante();

                //Actualizar los atributos TAG de los controles de la cabecera
                ActualizaValoresOrigenControles();

                this.ControlesHabilitarDeshabilitar(true);
            }
            else
            {
                this.CrearTablasDataSetVacias();
                this.ControlesHabilitarDeshabilitar(false);
            }

            //XX-9999999

            //El botón de selección tiene que estar por encima de la Grid
            this.btnSel.BringToFront();

            //Poner en el idioma correspondiente todos los literales
            this.TraducirLiterales(false);

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
//DUDA            this.tgTexBoxSelCompania.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelCompania_ValueChangedFormClose);

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
//DUDA            this.tgTexBoxSelTipo.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelTipo_ValueChanged);

            //this.dgDetalle.CurrentCell = this.dgDetalle[4, 0];
            this.dgDetalle.ClearSelection();

            //Ocultar el botón de selección de la Grid
            this.btnSel.Visible = false;

            //Ajustar las columnas de la Grid de Detalles
            //utiles.AjustarColumnasGrid(ref this.dgDetalle, -1);
            //this.dgDetalle.Refresh();

            //En el menú derecho en el elemento de insertar filas, poner por defecto 1 fila (1er número del desplegable)
            this.toolStripInsertarFilacmbFilas.SelectedIndex = 0;
            
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;
        }
        
        private void MenuGridButtonInsertarFila_Click(object sender, EventArgs e)
        {
            if (this.toolStripInsertarFilacmbFilas.SelectedItem != null)
            {
                this.dgDetalle.InsertarFila(Convert.ToInt16(this.toolStripInsertarFilacmbFilas.SelectedItem));
                this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
            }            
        }

        private void MenuGridButtonSuprimirFila_Click(object sender, EventArgs e)
        {
            this.SuprimirFilas();
        }

        private void MenuGridButtonBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void MenuGridButtonReemplazar_Click(object sender, EventArgs e)
        {
            this.Reemplazar();
        }

        private void MenuGridButtonAdicionarFila_Click(object sender, EventArgs e)
        {
            this.dgDetalle.AdicionarFila();
            this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
        }

        private void MenuGridButtonCopiar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.CopiarDetalles();
            this.dgDetalle.Refresh();
        }

        private void MenuGridButtonPegar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.PegarDetalles();
            this.dgDetalle.Refresh();
            this.CalcularTotales();
            this.gridChange = true;

            /*
            this.dgDetalle.PegarDetalles();

            int[] posicionInicial = this.dgDetalle.PegarDetalles();
            //Actualizar columnas
            //Habilitar / Deshabilitar Columnas
            /*for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
            {
                this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
            }*/
            /*
            for (int i = 0; i < posicionInicial[1]; i++)
            {
                this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[posicionInicial[0] + i]["Cuenta"].ToString(), posicionInicial[0] + i);
            }

            this.gridChange = true;
            this.CalcularTotales();*/
        }

        private void MenuGridButtonCortar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.CortarDetalles();
            this.dgDetalle.Refresh();
        }

        private void MenuGridButtonBorrar_Click(object sender, EventArgs e)
        {
            this.dgDetalle.BorrarDetalles();
            this.dgDetalle.Refresh();
        }

   
        private void FrmCompContAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            Boolean preguntar = false;
            string cmbCompania = "";
            if (this.cmbCompania.Text!="") cmbCompania = this.cmbCompania.Text.Substring(0, 2).ToString();
            string cmbTipo = "";
            if (this.cmbTipo.Text.ToString()!="") cmbTipo = this.cmbTipo.Text.Substring(0, 2).ToString();
            string cmbClase = "";
            if (this.cmbClase.Text.ToString()!="") cmbClase = this.cmbClase.Text.Substring(0, 1).ToString();
            int lng = Convert.ToString(this.dateTimePickerFecha.Tag).Length;
            if (lng > 10) lng = 10;
            string fechatag = Convert.ToString(this.dateTimePickerFecha.Tag).Substring(0, lng);
            lng = Convert.ToString(this.dateTimePickerFecha.Text).Length;
            if (lng > 10) lng = 10;
            string fechatext = Convert.ToString(this.dateTimePickerFecha.Text).Substring(0, lng);
            try
            {
                if (this.nuevoComprobante)
                {
                    if (!dNoPreguntar) preguntar = true;
                }
                else
                {
                    if (cmbCompania != compania_ant ||
                        this.txtMaskAAPP.Text != aapp_ant ||
                        //this.dateTimePickerFecha.Value != Convert.ToDateTime(this.dateTimePickerFecha.Tag) ||
                        fechatext != fechatag ||
                        cmbTipo != tipo_ant ||
                        this.txtNoComprobante.Text != nocomp_ant ||
                        cmbClase != this.cmbClase.Tag.ToString() ||
                        this.txtTasa.Text != this.txtTasa.Tag.ToString() ||
                        this.txtDescripcion.Text != this.txtDescripcion.Tag.ToString() ||
                        this.gridCambiada)
                    {
                        preguntar = true;
                    } 
                } 
                if (!dNoPreguntar && preguntar == true && !this.comprobanteSoloConsulta)
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes && dGrabar==false)
                    {
                        this.radButtonGrabar.PerformClick();
                        e.Cancel = false;
                        this.gridCambiada = false;
                        dNoPreguntar = true;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Alta / Edita comprobantes contables ");

            e.Cancel = false;
        }

        private static int DataGridViewRowIndexCompare(DataGridViewRow x, DataGridViewRow y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Index.CompareTo(y.Index);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.Index.CompareTo(y.Index);
                    }
                }
            }
        }

        private void BtnSel_Click(object sender, EventArgs e)
        {
            DataGridViewCell celdaActiva = this.dgDetalle.CurrentCell;
            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;

            string columnName = this.dgDetalle.Columns[celdaActiva.ColumnIndex].Name;

            string titulo = "";
            string error = "";
            string query = QuerySelElementos(celdaActiva.ColumnIndex, celdaActiva.RowIndex, ref titulo, ref error, columnName);

            if (error == "")
            {
                TGElementosSel frmElementosSel = new TGElementosSel
                {
                    TituloForm = titulo,

                    //Coordenadas donde aparecerá el formulario para seleccionar los elementos
                    /*int coordX = 0;
                    int coordY = 0;
                    if (this.btnSel.Location.X + this.btnSel.Width + frmElementosSel.Size.Width <= this.Size.Width) coordX = this.btnSel.Location.X + this.btnSel.Width;
                    else coordX = this.btnSel.Location.X - frmElementosSel.Size.Width;

                    if (this.btnSel.Location.Y + frmElementosSel.Size.Height <= this.Size.Height) coordY = this.btnSel.Location.Y;
                    else coordY = this.btnSel.Location.Y - frmElementosSel.Size.Height;

                    //frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 95, this.btnSel.Location.Y + 98);
                    //frmElementosSel.LocationForm = new Point(coordX, coordY);
                    */
                    //Centrar el formulario
                    CentrarForm = true,
                    FrmPadre = this,

                    //Pasar la conexión a la bbdd
                    ProveedorDatosForm = GlobalVar.ConexionCG,
                    //Consulta que se ejecutará para obtener los elementos
                    Query = query
                };
                //Definir la cabecera de las columnas
                ArrayList nombreColumnas = new ArrayList
                {
                    this.LP.GetText("lblListaCampoCodigo", "Código"),
                    this.LP.GetText("lblListaCampoDescripcion", "Descripción")
                };
                frmElementosSel.ColumnasCaption = nombreColumnas;

                //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
                //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
                frmElementosSel.OkForm += new TGElementosSel.OkFormCommandEventHandler(FrmElementosSel_OkForm);
                frmElementosSel.ShowDialog();
                
            }

            this.dgDetalle.Focus();
        }

        private void FrmElementosSel_OkForm(TGElementosSel.OkFormCommandEventArgs e)
        {
            DataGridViewCell celdaActiva = this.dgDetalle.CurrentCell;
            this.dgDetalle.CurrentCell.Value = e.Valor[0].ToString().Trim();

            DataGridViewCellEventArgs arg = new DataGridViewCellEventArgs(celdaActiva.ColumnIndex, celdaActiva.RowIndex);
            //dgDetalle_CellLeave(this.dgDetalle, arg);
            try
            {
                Boolean salir = false;
                int i = 0;
                while (salir == false)
                {
                    i++;
                    if (this.dgDetalle[celdaActiva.ColumnIndex + i, celdaActiva.RowIndex].Visible == true)
                    {
                        salir = true;
                        this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex + i, celdaActiva.RowIndex];
                    }
                }
            }
            catch
            {

            }
        }
        
        private void TxtMaskAAPP_TextChanged(object sender, EventArgs e)
        {
            txtMaskAAPP.Modified = true;
        }

        private void TxtMaskAAPP_Enter(object sender, EventArgs e)
        {
            txtMaskAAPP.Modified = false;
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtNoComprobante_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtTasa_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void TxtDescripcion_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void DateTimePickerFecha_Enter(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
            this.dgDetalle.ClearSelection();
        }

        private void DgDetalle_KeyDown(object sender, KeyEventArgs e)
        {
            //tecla suprimir
            if (e.KeyCode == Keys.Delete)
            {
                //no en edicion 
                if (dgDetalle.IsCurrentCellInEditMode == false && this.dgDetalle.SelectedRows.Count > 0)
                {
                    //this.dgDetalle.SuprimirFila();
                    this.SuprimirFilas();

                    /*
                    //borrar toda la seleccion
                    for (int i = 0; i < this.dgDetalle.SelectedCells.Count; i++)
                    {
                        int row = this.dgDetalle.SelectedCells[i].RowIndex;
                        int col = this.dgDetalle.SelectedCells[i].ColumnIndex;
                        this.dgDetalle.Rows[row].Cells[col].Value = "";
                    }
                     */
                }
                else
                {
                    for (int i = 0; i < this.dgDetalle.SelectedCells.Count; i++)
                    {
                        this.dgDetalle.SelectedCells[i].Value = "";
                    }
                }
                this.dgDetalle.Refresh();
            }
            else
                if (e.Control && e.KeyCode == Keys.C)
                {
                    this.dgDetalle.CopiarDetalles();
                    this.dgDetalle.Refresh();
                }
                else
                    if (e.Control && e.KeyCode == Keys.V)
                    {
                        int[] posicionInicial = this.dgDetalle.PegarDetalles();

                        //Actualizar columnas
                        //Habilitar / Deshabilitar Columnas   (estaría bien poderlo hacer solo de las columnas que se van a pegar)
                        /*for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                        {
                            this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                        }*/
                        for (int i = 0; i < posicionInicial[1]; i++)
                        {
                            this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[posicionInicial[0]+i]["Cuenta"].ToString(), posicionInicial[0]+i);
                        }
                        this.dgDetalle.Refresh();
                    }
                    else
                        if (e.Control && e.KeyCode == Keys.X)
                        {
                            this.dgDetalle.CortarDetalles();
                            this.dgDetalle.Refresh();
                        }

            this.gridChange = true;
            this.CalcularTotales();
        }

        private void DgDetalle_CurrentCellChanged(object sender, EventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    switch (columnName)
                    {
                        case "Cuenta":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            //this.btnSel.Select();
                            break;
                        case "Auxiliar1":
                        case "Auxiliar2":
                        case "Auxiliar3":
                        case "Iva":
                            //Verificar que la cuenta de mayor se ha introducido
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Cuenta"].Value.ToString() != "")
                            {
                                //Mostrar el botón de Selección en las coordenadas que le corresponde
                                this.BtnSelPosicion(tgGridDetalles);
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = false;
                                //this.btnSel.Select();
                            }
                            else
                            {
                                this.btnSel.Visible = false;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = true;
                            }
                            break;
                        
                        //extendidos
                        case "CampoUserAlfa1":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa1", "TA01PX", true);
                            break;
                        case "CampoUserAlfa2":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa2", "TA02PX", true);
                            break;
                        case "CampoUserAlfa3":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa3", "TA03PX", true);
                            break;
                        case "CampoUserAlfa4":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa4", "TA04PX", true);
                            break;
                        case "CampoUserAlfa5":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa5", "TA05PX", true);
                            break;
                        case "CampoUserAlfa6":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa6", "TA06PX", true);
                            break;
                        case "CampoUserAlfa7":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa7", "TA07PX", true);
                            break;
                        case "CampoUserAlfa8":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa8", "TA08PX", true);
                            break;

                        default:
                            this.btnSel.Visible = false;
                            break;
                    }
                }
            } 
        }

        private void DgDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                this.gridChange = true;
                this.gridCambiada = true;


                string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                switch (columnName)
                {
                    case "DH":
                        this.CalcularTotales();
                        break;
                    case "MonedaLocal":
                        string importeMonedaLocal = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["MonedaLocal"].Value.ToString().Trim();
                        this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["MonedaLocal"].Value =  utiles.ImporteFormato(importeMonedaLocal, this.LP.MyCultureInfo);

                        this.CalcularTotales();
                        break;
                    case "MonedaExt":
                        string importeMonedaExt = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["MonedaExt"].Value.ToString().Trim();
                        this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["MonedaExt"].Value = utiles.ImporteFormato(importeMonedaExt, this.LP.MyCultureInfo);
                        
                        this.CalcularTotales();
                        break;
                    case "Importe3":
                        string importe3 = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Importe3"].Value.ToString().Trim();
                        this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Importe3"].Value = utiles.ImporteFormato(importe3, this.LP.MyCultureInfo);

                        this.CalcularTotales();
                        break;
                    case "CampoUserNum1":
                        string campoUserNum1 = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["CampoUserNum1"].Value.ToString().Trim();
                        this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["CampoUserNum1"].Value = utiles.ImporteFormato(campoUserNum1, this.LP.MyCultureInfo);

                        break;
                    case "CampoUserNum2":
                        string campoUserNum2 = this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["CampoUserNum2"].Value.ToString().Trim();
                        this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["CampoUserNum2"].Value = utiles.ImporteFormato(campoUserNum2, this.LP.MyCultureInfo);

                        break;
                }
            }
            //SMR - necesario para que al validar tome el ultimo valor de la celda, no el anterior
            this.dgDetalle.BindingContext[this.dgDetalle.DataSource].EndCurrentEdit();
        }

        private void DgDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (e.ColumnIndex == -1)
                {
                    this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (e.RowIndex != -1)
                    {
                        this.dgDetalle.Rows[e.RowIndex].Selected = true;
                        this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex, e.RowIndex];
                    }
                    return;
                }

                if (e.RowIndex == -1)
                {
                    //this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                    this.dgDetalle.Columns[e.ColumnIndex].Selected = true;
                    this.dgDetalle.CurrentCell = this.dgDetalle[e.ColumnIndex, celdaActiva.RowIndex];
                    return;
                }

                this.dgDetalle.SelectionMode = DataGridViewSelectionMode.CellSelect;
                this.dgDetalle.ClearSelection();
                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                this.dgDetalle.CurrentCell = this.dgDetalle[celdaActiva.ColumnIndex, celdaActiva.RowIndex];

                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    switch (columnName)
                    {
                        case "Cuenta":
                            //Mostrar el botón de Selección en las coordenadas que le corresponde
                            this.BtnSelPosicion(tgGridDetalles);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            //this.btnSel.Select();
                            break;
                        case "Auxiliar1":
                        case "Auxiliar2":
                        case "Auxiliar3":
                        case "Iva":
                            //Verificar que la cuenta de mayor se ha introducido
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Cuenta"].Value.ToString() != "")
                            {
                                //Mostrar el botón de Selección en las coordenadas que le corresponde
                                this.BtnSelPosicion(tgGridDetalles);
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = false;
                                //this.btnSel.Select();
                            }
                            else
                            {
                                this.btnSel.Visible = false;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = true;
                            }
                            break;

                        //extendidos
                        case "CampoUserAlfa1":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa1", "TA01PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa2":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa2", "TA02PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa3":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa3", "TA03PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa4":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa4", "TA04PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa5":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa5", "TA05PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa6":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa6", "TA06PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa7":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa7", "TA07PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa8":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa8", "TA08PX", true);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        default:
                            this.btnSel.Visible = false;
                            break;
                    }
                }
            }
        }

        private void DgDetalle_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (this.btnSel.Visible)
            {
                this.BtnSelPosicion((TGGrid)sender);
            }
        }

        private void DgDetalle_Sorted(object sender, EventArgs e)
        {
            //Ocultar el botón ??? OJO ???
            //this.btnSel.Visible = false;

            //O posicionarlo en la celda [0,0]   ??? OJO ???
            this.dgDetalle.CurrentCell = this.dgDetalle[0, 0];
            this.dgDetalle.Rows[0].Cells[0].Selected = true;
            this.dgDetalle.Focus();
            this.BtnSelPosicion(this.dgDetalle);
        }

        private void DgDetalle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = this.dgDetalle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)this.dgDetalle.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    //((DataGridViewComboBoxColumn)this.dgDetalle.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }

        private void TgTexBoxSelCompania_Leave(object sender, EventArgs e)
        {
            /*
            if (this.tgTexBoxSelCompania.Textbox.Modified)
            {
                string codigo = "";
                if (this.tgTexBoxSelCompania.Textbox.Text.Length <= 2) codigo = this.tgTexBoxSelCompania.Textbox.Text;
                else codigo = this.tgTexBoxSelCompania.Textbox.Text.Substring(0, 2);

                string result = ValidarCompania(codigo);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                    this.tgTexBoxSelCompania.Textbox.Focus();
                }
                else
                {
                    if (this.tgTexBoxSelCompania.Textbox.Text.Length <= 2)
                    {
                        if (this.GLM01_NCIAMG != "") this.tgTexBoxSelCompania.Textbox.Text += " " + separadorDesc + " " + this.GLM01_NCIAMG;
                    }
                    
                    this.CrearComboClase();
                }
            }

            //this.tgTexBoxSelCompania.Textbox.Select(0, 0);
            */
        }

        private void TxtMaskAAPP_Leave(object sender, EventArgs e)
        {
            //DUDA if (this.txtMaskAAPP.Modified == true || this.tgTexBoxSelCompania.Textbox.Modified == true)
            if (this.txtMaskAAPP.Modified == true)
            {
                string result = this.ValidarAAPP();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                    //this.txtMaskAAPP.Focus();
                }
            }
        }
       
        private void DateTimePickerFecha_Leave(object sender, EventArgs e)
        {
            string result = this.ValidarFecha();

            if (result != "")
            {
                string titulo = "";
                if (this.GLM01_FELAMG != null && this.GLM01_FELAMG != "")
                {
                    //Ver el valor del campo
                    switch (this.GLM01_FELAMG)
                    {
                        case "W":   //ADVERTENCIA
                            titulo = this.LP.GetText("wrnTitulo", "Advertencia");
                            break;
                        case "T":   //ERROR
                        default:
                            titulo = this.LP.GetText("errValTitulo", "Error");
                            break;
                    }
                }

                MessageBox.Show(result, titulo);
            }
        }

        private void TxtTasa_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números con 7 posiciones decimales
            utiles.ValidarNumeroConDecimalesKeyPress(7, ref this.txtTasa, false, ref sender, ref e);
        }

        private void DgDetalle_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            /*
                    // Validate the CompanyName entry by disallowing empty strings.
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "CompanyName")
                    {
                        if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText =
                                "Company Name must not be empty";
                            e.Cancel = true;
                        }
                    }
             
                    void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
                    {
                        // Clear the row error in case the user presses ESC.   
                        dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
                    }             
             */
            if (this.dgDetalle.IsCurrentCellDirty)
            {
                string result = "";
                string error = this.LP.GetText("errValTitulo", "Error");

                string valor = e.FormattedValue.ToString();
                string cuentaMayor = this.dgDetalle.Rows[e.RowIndex].Cells["Cuenta"].Value.ToString();
                switch (this.dgDetalle.Columns[e.ColumnIndex].Name)
                {
                    case "Cuenta":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaMayor(valor);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;

                                //this.dgDetalle.Rows[e.RowIndex].ErrorText = String.Empty;
                            }
                        }
                        break;
                    case "Auxiliar1":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 1);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "Auxiliar2":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 2);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "Auxiliar3":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 8 caracteres
                            result = ValidarCuentaAuxiliar(valor, cuentaMayor, 3);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "DH":
                        //FALTA !!!! SOlo leer 1 caracter que tiene que ser D / H
                        break;
                    case "MonedaLocal":
                        //FALTA !!!! Numerico de 14 posiciones (12 lugares enteros y 2 decimales)
                        break;
                    case "MonedaExt":
                        //FALTA !!!! Numerico de 14 posiciones (12 lugares enteros y 2 decimales)
                        break;
                    case "RU":
                        //FALTA !!!! SOlo leer 2 caracteres
                        break;
                    case "Descripcion":
                        //FALTA !!!! SOlo leer 36 caracteres
                        break;
                    case "Documento":
                        //Falta !!!! Máscara XX-9999999
                        break;
                    case "Fecha":
                        //DataTimePicker de ser posible
                        break;
                    case "Vencimiento":
                        //DataTimePicker de ser posible
                        break;
                    case "Documento2":
                        //Máscara XX-999999999
                        break;
                    case "Importe3":
                        //FALTA !!!! Numerico de 14 posiciones (12 lugares enteros y 2 decimales)
                        break;
                    case "Iva":
                        if (valor != "")
                        {
                            //FALTA !!!! SOlo leer 2 caracteres
                            result = ValidarIVA(valor);
                            if (result != "")
                            {
                                MessageBox.Show(result, error);
                                //e.Cancel = true;
                            }
                        }
                        break;
                    case "CifDNi":
                        //FALTA !!!! SOlo leer 13 caracteres
                        break;
                    case "PrefijoDoc":
                        break;
                    case "NumFactAmp":
                        break;
                    case "NumFactRectif":
                        break;
                    case "FechaServIVA":
                        break;
                    case "CampoUserAlfa1":
                        break;
                    case "CampoUserAlfa2":
                        break;
                    case "CampoUserAlfa3":
                        break;
                    case "CampoUserAlfa4":
                        break;
                    case "CampoUserAlfa5":
                        break;
                    case "CampoUserAlfa6":
                        break;
                    case "CampoUserAlfa7":
                        break;
                    case "CampoUserAlfa8":
                        break;
                    case "CampoUserNum1":
                        break;
                    case "CampoUserNum2":
                        break;
                    case "CampoUserFecha1":
                        break;
                    case "CampoUserFecha2":
                        break;
                }
            }
        }

        private void DgDetalle_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //string columnName = this.dgDetalle.Columns[this.dgDetalle.CurrentCell.ColumnIndex].Name;
            
            switch (this.dgDetalle.Columns[e.ColumnIndex].Name)
            {
                case "Cuenta":
                    //Comprobar si la cuenta de mayor tiene auxiliares
                    string valor = this.dgDetalle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    //QUITAR COMENTARIO
                    //if (!(this.edicionLote || this.edicionLoteError))
                    
                    //{
                        if (cmbCompania.Text.Substring(0, 2) != "" && nuevoComprobante && !nglm01)
                        {
                            string result1 = this.QueryGLM01(cmbCompania.Text.Substring(0, 2));
                            nglm01 = true;
                        }
                            string result = UpdateEstadoColumnasDadoCuentaMayor(valor, e.RowIndex);
                    //}

                    //poner la celda activa la que toque
                    break;
                case "Auxiliar1":
                    break;
                case "Auxiliar2":
                    break;
                case "Auxiliar3":
                    break;
                case "DH":
                    break;
                case "MonedaLocal":
                    break;
                case "MonedaExt":
                    break;
                case "RU":
                    break;
                case "Descripcion":
                    break;
                case "Documento":
                    break;
                case "Fecha":
                    break;
                case "Vencimiento":
                    break;
                case "Documento2":
                    break;
                case "Importe3":
                    break;
                case "Iva":
                    break;
                case "CifDNi":
                    break;
                case "PrefijoDoc":
                    break;
                case "NumFactAmp":
                    break;
                case "NumFactRectif":
                    break;
                case "FechaServIVA":
                    break;
                case "CampoUserAlfa1":
                    break;
                case "CampoUserAlfa2":
                    break;
                case "CampoUserAlfa3":
                    break;
                case "CampoUserAlfa4":
                    break;
                case "CampoUserAlfa5":
                    break;
                case "CampoUserAlfa6":
                    break;
                case "CampoUserAlfa7":
                    break;
                case "CampoUserAlfa8":
                    break;
                case "CampoUserNum1":
                    break;
                case "CampoUserNum2":
                    break;
                case "CampoUserFecha1":
                    break;
                case "CampoUserFecha2":
                    break;
            }
        }

        private void DgDetalle_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;

            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                if (celdaActiva.ReadOnly) this.btnSel.Visible = false;
                else
                {
                    string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;
                    switch (columnName)
                    {
                        case "Cuenta":     //Cta Mayor
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            this.btnSel.Visible = true;
                            //this.btnSel.Select();
                            break;
                        case "Auxiliar1":     //Auxiliar 1 
                        case "Auxiliar2":     //Auxiliar 2
                        case "Auxiliar3":     //Auxiliar 3
                        case "Iva":    //IVA
                            //Verificar que la cuenta de mayor se ha introducido
                            if (this.dgDetalle.Rows[celdaActiva.RowIndex].Cells["Cuenta"].Value.ToString() != "")
                            {
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = false;
                                this.btnSel.Visible = true;
                                //this.btnSel.Select();
                            }
                            else
                            {
                                this.btnSel.Visible = false;
                                this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].ReadOnly = true;
                            }
                            break;

                        //extendidos
                        case "CampoUserAlfa1":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa1", "TA01PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa2":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa2", "TA02PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa3":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa3", "TA03PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa4":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa4", "TA04PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa5":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa5", "TA05PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa6":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa6", "TA06PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa7":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa7", "TA07PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        case "CampoUserAlfa8":
                            this.ActivaroNOBotonSelCamposExt("CampoUserAlfa8", "TA08PX", false);
                            this.dgDetalle.Rows[celdaActiva.RowIndex].Cells[celdaActiva.ColumnIndex].Selected = true;
                            break;
                        default:
                            this.btnSel.Visible = false;
                            break;
                    }
                }
            }
        }

        private void DgDetalle_Enter(object sender, EventArgs e)
        {
            this.BtnSelPosicion((TGGrid)sender);
        }

        private void DgDetalle_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TGGrid tgGridDetalles = ((TGGrid)sender);
            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            string columnName = tgGridDetalles.Columns[celdaActiva.ColumnIndex].Name;

            e.Control.KeyPress -= Tb_KeyPress;
            e.Control.KeyPress -= Tb_May_KeyPress;
            e.Control.KeyPress -= Tb_NADA_KeyPress;

            switch (columnName)
            {                
                //Todo caracter a mayúsculas
                case "Cuenta":
                case "Auxiliar1":
                case "Auxiliar2":
                case "Auxiliar3":
                case "DH":
                case "RU":
                case "Documento":
                case "Documento2":
                case "Iva":
                case "CifDni":
                case "Pais":
                    e.Control.KeyPress += new KeyPressEventHandler(Tb_May_KeyPress);
                    break;          
                //Solo se permiten decimales
                case "MonedaLocal":
                case "MonedaExt":
                case "Importe3":
                case "CampoUserNum1":
                case "CampoUserNum2": 
                    e.Control.KeyPress += new KeyPressEventHandler(Tb_KeyPress);
                    break;
                //No se hace nada, se deja el caracter como se escribe
                default:
                    e.Control.KeyPress += new KeyPressEventHandler(Tb_NADA_KeyPress);
                    break;
            }
        }

        void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb = sender as TextBox;
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.tb, true, ref sender, ref e);
        }

        void Tb_May_KeyPress(object sender, KeyPressEventArgs e)
        {
            //string caracter = e.KeyChar.ToString().ToUpper();
            //e.KeyChar = Convert.ToChar(caracter);
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        void Tb_NADA_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = e.KeyChar;
        }

        private void TxtNoComprobante_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtNoComprobante, false, ref sender, ref e);
        }

        private void DgDetalle_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //this.dgDetalle.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
        }
        
        private void DgErrores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                string tipo = this.dgErrores.Rows[row].Cells["CodiTipo"].Value.ToString();
                string control_celda = this.dgErrores.Rows[row].Cells["CtrlCelda"].Value.ToString();
                string rowDetalle = "";
                int rowDet;
                switch (tipo)
                {
                    case "C":       //CABECERA
                        if (control_celda != "")
                        {
                            //Control[] ctrl = this.Controls.Find(control_celda, true);
                            Control[] ctrl = this.Controls["radPanelApp"].Controls["gbCabecera"].Controls.Find(control_celda, true);

                            if (ctrl.Length > 0)
                            {
                                /*if (ctrl[0].GetType().Name == "TGTexBoxSel")
                                {
                                    TGTexBoxSel tt = ((TGTexBoxSel)ctrl[0]);
                                    tt.Textbox.Select();
                                }
                                else*/
                                ctrl[0].Select();
                            }
                        }
                        break;
                    case "D":       //DETALLE
                        rowDetalle = this.dgErrores.Rows[row].Cells["Linea"].Value.ToString();
                        if (rowDetalle != "")
                        {
                            rowDet = Convert.ToInt32(rowDetalle) - 1;

                            if (control_celda != "")
                            {
                                this.dgDetalle.CurrentCell = this.dgDetalle.Rows[rowDet].Cells[control_celda];
                                this.dgDetalle.BeginEdit(true);
                            }
                            else
                            {
                                this.dgDetalle.CurrentCell = this.dgDetalle.Rows[rowDet].Cells[0];
                                this.dgDetalle.Rows[0].Selected = true;
                            }
                        }
                        break;
                }

                this.dgErrores.ClearSelection();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void DgDetalle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void FrmCompContAltaEdita_SizeChanged(object sender, EventArgs e)
        {
            this.btnSel.Visible = false;
        }

        private void FrmCompContAltaEdita_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonSalir_Click(sender, null);
        }

        private void BtnVerComentarios_Click(object sender, EventArgs e)
        {
            frmCompContAltaEditaComentarios frmAltaEditaComentarios = new frmCompContAltaEditaComentarios
            {
                CodigoCompania = this.comprobanteContableImportar.Cab_compania,
                NombreCompania = this.cmbCompania.Text,
                AAPP = this.comprobanteContableImportar.Cab_anoperiodo,
                AAPPFormato = this.txtMaskAAPP.Text,
                CodigoTipo = this.comprobanteContableImportar.Cab_tipo,
                NombreTipo = this.cmbTipo.Text,
                NumeroComprobante = this.txtNoComprobante.Text,
                FrmPadre = this
            };
            frmAltaEditaComentarios.ShowDialog();

            if (frmAltaEditaComentarios.ActualizaDescripcion)
            {
                this.txtDescripcion.Text = frmAltaEditaComentarios.DescripcionComprobante;
                this.txtDescripcion.Tag = this.txtDescripcion.Text;
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        /// <param name="traducirComboClase">Si se traducen o no los literales del Combo de Clase</param>
        private void TraducirLiterales(bool traducirComboClase)
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContAltaEditaTitulo", "Comprobante Contable");
            if (this.nuevoComprobante)
            {
                this.radLabelTitulo.Text = this.Text + " / " + this.LP.GetText("lblfrmCompContAltaEditaTituloNuevo", "Nuevo");
                //utiles.ButtonEnabled(ref this.radButtonNuevo, false);
            }
            else
            {
                this.radLabelTitulo.Text = this.Text + " / " + this.nombreComprobante;

                if (this.edicionLote || this.edicionLoteError)
                {
                    string prefijo = "";
                    if (comprobanteContableImportar.Prefijo != null) prefijo = comprobanteContableImportar.Prefijo;
                    string biblioteca = "";
                    if (comprobanteContableImportar.Biblioteca != null) biblioteca = comprobanteContableImportar.Biblioteca;

                    if (prefijo != "") this.radLabelTitulo.Text = this.Text.Trim() + " - " + this.LP.GetText("lblPrefijo", "Prefijo") + ": " + prefijo;

                    if (biblioteca != "") this.radLabelTitulo.Text = this.Text.Trim() + " - " + this.LP.GetText("lblBiblioteca", "Biblioteca") + ": " + biblioteca;
                }
                else if (this.edicionComprobanteGLB01) this.radLabelTitulo.Text += "Registro tabla de contabilidad";
                
                //utiles.ButtonEnabled(ref this.radButtonNuevo, true);
            }

            //Traducir las etiquetas de la Cabecera
            this.gbCabecera.Text = this.LP.GetText("lblCabecera", "Cabecera");
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            this.lblFecha.Text = "Fecha contable";
                //this.LP.GetText("lblfrmCompContAECabeceraFecha", "Fecha contable");
            this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
            this.lblClase.Text = this.LP.GetText("lblfrmCompContAECabeceraClase", "Clase");
            this.lblTasa.Text = this.LP.GetText("lblfrmCompContAECabeceraTasa", "Tasa de cambio");
            this.lblDescripcion.Text = this.LP.GetText("lblfrmCompContAECabeceraDescripcion", "Descripción");

            //Traducir las etiquetas de la tabla de totales
            this.gbTotales.Text = this.LP.GetText("lblfrmCompContAETablaTotalesGroup", "Totales");
            this.lblTotalDebe.Text = this.LP.GetText("lblfrmCompContAETablaTotalesTotalDebe", "Total Debe");
            this.lblTotalHaber.Text = this.LP.GetText("lblfrmCompContAETablaTotalesTotalHaber", "Total Haber");
            this.lblMonedaLocal.Text = this.LP.GetText("lblfrmCompContAETablaTotalesMonedaLocal", "Moneda Local");
            this.lblMonedaExtranjera.Text = this.LP.GetText("lblfrmCompContAETablaTotalesMonedaExt", "Moneda Extranjera");
            this.lblImporte3.Text = this.LP.GetText("lblfrmCompContAETablaTotalesImporte3", "Importe3");
            this.lblNoApuntes.Text = this.LP.GetText("lblfrmCompContAETablaTotalesNoApuntes", "No. Apuntes");

            //Traducir los Literales de los Botones
            //this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonGrabar.Text = this.LP.GetText("lblfrmCompContBotGrabar", "Guardar");
            this.radButtonGrabarComo.Text = this.LP.GetText("lblfrmCompContBotGrabarComo", "Guardar Como");
            //this.toolStripButtonSelecModelo.Text = this.LP.GetText("lblfrmCompContBotSelModelo", "Seleccionar Modelo");
            this.radButtonValidar.Text = this.LP.GetText("lblfrmCompContBotValidar", "Validar");
            this.radButtonRevertir.Text = this.LP.GetText("lblfrmCompContBotRevertir", "Revertir importes");
            this.radButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");
            
            //Traducir los Literales del menuGridClickDerecho
            this.menuGridButtonAdicionarFila.Text = this.LP.GetText("lblfrmCompContClickDerAdicionarFila", "Añadir fila");
            this.menuGridButtonInsertarFila.Text = this.LP.GetText("lblfrmCompContClickDerInsertarFila", "Insertar fila");
            this.menuGridButtonSuprimirFila.Text = this.LP.GetText("lblfrmCompContClickDerSuprimirFila", "Suprimir fila");
            this.menuGridButtonBuscar.Text = this.LP.GetText("lblfrmCompContClickDerBuscar", "Buscar");
            this.menuGridButtonCortar.Text = this.LP.GetText("lblfrmCompContClickDerCortar", "Cortar");
            this.menuGridButtonCopiar.Text = this.LP.GetText("lblfrmCoContClickDerCopiar", "Copiar");
            this.menuGridButtonPegar.Text = this.LP.GetText("lblfrmCompContClickDerPegar", "Pegar");
            this.menuGridButtonBorrar.Text = this.LP.GetText("lblfrmCompContClickDerBorrar", "Borrar");

            this.btnVerComentarios.Text = this.LP.GetText("lblfrmCompContVerComent", "Ver Comentarios");

            //Traducir los encabezados de las columnas
            if (this.dgDetalle != null)
            {
                this.dgDetalle.CambiarColumnHeader("Cuenta", this.LP.GetText("lblfrmCompContdgCuenta", "Cuenta"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar1", this.LP.GetText("lblfrmCompContdgAux1", "Auxiliar-1"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar2", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-2"));
                this.dgDetalle.CambiarColumnHeader("Auxiliar3", this.LP.GetText("lblfrmCompContdgAux3", "Auxiliar-3"));
                this.dgDetalle.CambiarColumnHeader("DH", this.LP.GetText("lblfrmCompContdgDH", "D/H"));
                this.dgDetalle.CambiarColumnHeader("MonedaLocal", this.LP.GetText("lblfrmCompContdgMonedaLocal", "Moneda Local"));
                this.dgDetalle.CambiarColumnHeader("MonedaExt", this.LP.GetText("lblfrmCompContdgMonedaExt", "Moneda Extranjera"));
                this.dgDetalle.CambiarColumnHeader("RU", this.LP.GetText("lblfrmCompContdgRU", "RU"));
                this.dgDetalle.CambiarColumnHeader("Descripcion", this.LP.GetText("lblfrmCompContdgDesc", "Descripción"));
                this.dgDetalle.CambiarColumnHeader("Documento", this.LP.GetText("lblfrmCompContdgDocumento", "Documento"));
                this.dgDetalle.CambiarColumnHeader("Fecha", this.LP.GetText("lblfrmCompContdgFecha", "Fecha"));
                this.dgDetalle.CambiarColumnHeader("Vencimiento", this.LP.GetText("lblfrmCompContdgVencimiento", "Vencimiento"));
                this.dgDetalle.CambiarColumnHeader("Documento2", this.LP.GetText("lblfrmCompContdgDocumento2", "Documento-2"));
                this.dgDetalle.CambiarColumnHeader("Importe3", this.LP.GetText("lblfrmCompContdgImporte3", "Importe-3"));
                this.dgDetalle.CambiarColumnHeader("Iva", this.LP.GetText("lblfrmCompContdgIva", "Iva"));
                this.dgDetalle.CambiarColumnHeader("CifDNi", this.LP.GetText("lblfrmCompContdgCifDni", "Cif/Dni"));

                if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) this.dgDetalle.CambiarColumnHeader("Pais", this.LP.GetText("lblfrmCompContdgPais", "Pais")); 
            }
        }

        /// <summary>
        /// Crea el desplegable de Clase
        /// </summary>
        private void CrearComboClase()
        {         
            DataRow row;
            bool todos = false;
            
            if (TipoMoneda is null) TipoMoneda = "";

            if (Batch && !BatchLote)
            { 
                if (this.cmbCompania.Tag is null && 
                    this.cmbCompania.Text != "" && 
                    this.cmbCompania.Text.Substring(0, 2) != Compania)
                {
                    nGlm02 = true;
                }
                else
                {
                if (this.cmbCompania.Tag is null && 
                        this.cmbCompania.Text != "" && 
                        this.cmbCompania.Text.Substring(0, 2) == Compania)
                    nGlm02 = false;
                }
            }
            

            if ((this.cmbCompania.Tag != null && this.cmbCompania.Tag != "") &&
                (this.cmbCompania.Tag != this.cmbCompania.Text.Substring(0, 2))
                || BatchLote)
            {
                nGlm02 = false;
            }

            IDataReader dr = null;
            string texto0 = "0 - " + this.LP.GetText("lblClaseValor0", "Solo ML");
            string texto1 = "1 - " + this.LP.GetText("lblClaseValor1", "ML calcula ME");
            string texto2 = "2 - " + this.LP.GetText("lblClaseValor2", "ME calcula ML");
            string texto3 = "3 - " + this.LP.GetText("lblClaseValor3", "ML y ME");
            string TIMOMP = "";

            try
            {
                if (this.dtClase.Rows.Count > 0) this.dtClase.Rows.Clear();

                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    //Leer el tipo de moneda
                    if (!nGlm02)
                    {

                        string query = "select TIMOMP from ";
                        query += GlobalVar.PrefijoTablaCG + "GLM02 ";
                        query += "where STATMP='V' and TIPLMP = '" + this.GLM01_TIPLMG + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                        
                        if (dr.Read())
                        {
                            TIMOMP = dr["TIMOMP"].ToString().Trim();
                            TipoMoneda = dr["TIMOMP"].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (nGlm02) TIMOMP = TipoMoneda;
                    }

                    if (TIMOMP == "")
                        {
                            //si TIMOMP = "" la clase de comprobante solo puede ser 0 ó 3. 
                            row = dtClase.NewRow();
                            row["valor"] = "0";
                            row["desc"] = texto0;
                            dtClase.Rows.Add(row);

                            // añadido por jl el 13/7/20
                            row = dtClase.NewRow();
                            row["valor"] = "1";
                            row["desc"] = texto1;
                            dtClase.Rows.Add(row);

                            row = dtClase.NewRow();
                            row["valor"] = "2";
                            row["desc"] = texto2;
                            dtClase.Rows.Add(row);

                            row = dtClase.NewRow();
                            row["valor"] = "3";
                            row["desc"] = texto3;
                            dtClase.Rows.Add(row);

                            this.txtTasa.Text = "";
                            this.txtTasa.Enabled = false;
                        }
                        else
                        {
                            //Si TIMOMP <> "" la clase de comprobante solo puede ser 1, 2 ó 3.
                            row = dtClase.NewRow();
                            row["valor"] = "1";
                            row["desc"] = texto1;
                            dtClase.Rows.Add(row);

                            row = dtClase.NewRow();
                            row["valor"] = "2";
                            row["desc"] = texto2;
                            dtClase.Rows.Add(row);

                            row = dtClase.NewRow();
                            row["valor"] = "3";
                            row["desc"] = texto3;
                            dtClase.Rows.Add(row);

                            this.txtTasa.Enabled = true;
                        }
                    if (!nGlm02)
                    {
                        nGlm02 = true;
                        dr.Close();
                    }
                }
                    else
                {
                    todos = true;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                todos = true;

                if (!nGlm02 && dr != null)
                {
                    nGlm02 = true;
                    dr.Close();
                }
            }

            if (todos)
            {
                row = dtClase.NewRow();
                row["valor"] = "0";
                row["desc"] = texto0;
                dtClase.Rows.Add(row);

                row = dtClase.NewRow();
                row["valor"] = "1";
                row["desc"] = texto1;
                dtClase.Rows.Add(row);

                row = dtClase.NewRow();
                row["valor"] = "2";
                row["desc"] = texto2;
                dtClase.Rows.Add(row);

                row = dtClase.NewRow();
                row["valor"] = "3";
                row["desc"] = texto3;
                dtClase.Rows.Add(row);

                this.txtTasa.Enabled = true;
            }

            this.cmbClase.DataSource = dtClase;
            this.cmbClase.ValueMember = "valor";
            this.cmbClase.DisplayMember = "desc";
            this.cmbClase.Refresh();

            if (this.cmbClase.Items.Count > 0) this.cmbClase.SelectedIndex = 0;
        }

        /// <summary>
        /// Crea el desplegable de Revertir
        /// </summary>
        private void CrearComboRevertir()
        {
            DataRow row;

            try
            {
                if (this.dtRevertir.Rows.Count > 0) this.dtRevertir.Rows.Clear();

                row = dtRevertir.NewRow();
                row["valor"] = REVERTIRNo;
                row["desc"] = "No";
                dtRevertir.Rows.Add(row);

                row = dtRevertir.NewRow();
                row["valor"] = REVERTIRImportes;
                row["desc"] = "Revertir importes";
                dtRevertir.Rows.Add(row);

                row = dtRevertir.NewRow();
                row["valor"] = REVERTIRImportesDH;
                row["desc"] = "Revertir importes y D/H";
                dtRevertir.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListRevertir.DataSource = dtRevertir;
            this.radDropDownListRevertir.ValueMember = "valor";
            this.radDropDownListRevertir.DisplayMember = "desc";
            this.radDropDownListRevertir.Refresh();
        }

        /// <summary>
        /// Crear las etiquetas para la tabla de totales
        /// </summary>
        private void CrearTablaTotales()
        {
            Font fontTabla = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);

            //this.lblTotalDebe.Text = "Total Debe";
            this.lblTotalDebe = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleCenter,
                Font = fontTabla
            };

            //this.lblTotalHaber.Text = "Total Haber";
            this.lblTotalHaber = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleCenter,
                Font = fontTabla
            };

            //this.lblMonedaLocal.Text = "Moneda Local";
            this.lblMonedaLocal = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                Font = fontTabla
            };

            //this.lblMonedaExtranjera.Text = "Moneda Extranjera";
            this.lblMonedaExtranjera = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                Font = fontTabla
            };

            //this.lblImporte3.Text = "Importe3";
            this.lblImporte3 = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                Font = fontTabla
            };

            //this.lblMonedaLocal_Debe.Text = "300.00";
            this.lblMonedaLocal_Debe = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblMonedaLocal_Haber.Text = "20.00";
            this.lblMonedaLocal_Haber = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblMonedaExtranjera_Debe.Text = "5000.00";
            this.lblMonedaExtranjera_Debe = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblMonedaExtranjera_Haber.Text = "10.00";
            this.lblMonedaExtranjera_Haber = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblImporte3_Debe.Text = "56.00";
            this.lblImporte3_Debe = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblImporte3_Haber.Text = "23.99";
            this.lblImporte3_Haber = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                Font = fontTabla
            };

            this.tablaTotales.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            this.tablaTotales.Controls.Add(this.lblTotalDebe, 1, 0);
            this.tablaTotales.Controls.Add(this.lblTotalHaber, 2, 0);
            this.tablaTotales.Controls.Add(this.lblMonedaLocal, 0, 1);
            this.tablaTotales.Controls.Add(this.lblMonedaExtranjera, 0, 2);
            this.tablaTotales.Controls.Add(this.lblImporte3, 0, 3);

            this.tablaTotales.Controls.Add(this.lblMonedaLocal_Debe, 1, 1);
            this.tablaTotales.Controls.Add(this.lblMonedaLocal_Haber, 2, 1);
            this.tablaTotales.Controls.Add(this.lblMonedaExtranjera_Debe, 1, 2);
            this.tablaTotales.Controls.Add(this.lblMonedaExtranjera_Haber, 2, 2);
            this.tablaTotales.Controls.Add(this.lblImporte3_Debe, 1, 3);
            this.tablaTotales.Controls.Add(this.lblImporte3_Haber, 2, 3);

            //this.lblNoApuntes.Text = "No. Apuntes";
            this.lblNoApuntes = new Telerik.WinControls.UI.RadLabel
            {
                TextAlignment = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Font = fontTabla
            };

            //this.lblNoApuntes_Valor.Text = "5";
            this.lblNoApuntes_Valor = new Telerik.WinControls.UI.RadLabel
            {
                Anchor = AnchorStyles.None,
                TextAlignment = ContentAlignment.TopCenter,
                AutoSize = true,
                Font = fontTabla
            };

            this.tablaNoApuntes.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            this.tablaNoApuntes.Controls.Add(this.lblNoApuntes, 0, 0);
            this.tablaNoApuntes.Controls.Add(this.lblNoApuntes_Valor, 1, 0);
        }


        /// <summary>
        /// Construye el DataGrid
        /// </summary>
        private void CrearDataGrid()
        {
            this.dgDetalle.dsDatos = new DataSet
            {
                DataSetName = "Comprobante"
            };
            this.dgDetalle.NombreTabla = "Detalle";
            this.dgDetalle.AddUltimaFilaSiNoHayDisponile = true;

            this.dgDetalle.Name = "dgDetalle";
            this.dgDetalle.AllowUserToAddRows = false;
            this.dgDetalle.AllowUserToOrderColumns = false;
            this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            //this.dgDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetalle.ContextMenuStrip = this.menuGridClickDerecho;

            this.dgDetalle.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            this.dgDetalle.AutoGenerateColumns = false;

            this.dgDetalle.RowNumber = true;

            try
            {
                this.dgDetalle.AddTextBoxColumn(0, "Cuenta", this.LP.GetText("lblfrmCompContdgCuenta", "Cuenta"), 90, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(1, "Auxiliar1", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-1"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(2, "Auxiliar2", this.LP.GetText("lblfrmCompContdgAux2", "Auxiliar-2"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(3, "Auxiliar3", this.LP.GetText("lblfrmCompContdgAux3", "Auxiliar-3"), 90, 8, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);

                //Valores del ComboBox para el Debe / Habe
                DataTable tableSource = new DataTable("tableSource");
                tableSource.Columns.AddRange(new DataColumn[] {
                new DataColumn("id"),
                new DataColumn("desc") });
                tableSource.Rows.Add("", "");
                tableSource.Rows.Add("D", this.LP.GetText("lblCompContDebe", "Debe"));
                tableSource.Rows.Add("H", this.LP.GetText("lblCompContHaber", "Haber"));

                DataGridViewComboBoxColumn columnComboBoxDH = new DataGridViewComboBoxColumn
                {
                    Name = "DH",
                    DataPropertyName = "DH",
                    Width = 60,
                    DropDownWidth = 160,
                    MaxDropDownItems = 3,
                    FlatStyle = FlatStyle.Flat,

                    DataSource = tableSource,
                    DisplayMember = "desc",
                    ValueMember = "id",
                    ValueType = typeof(string)
                };
                this.dgDetalle.Columns.Insert(4, columnComboBoxDH);

                this.dgDetalle.AddTextBoxColumn(5, "MonedaLocal", this.LP.GetText("lblfrmCompContdgMonedaLocal", "Moneda Local"), 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(6, "MonedaExt", this.LP.GetText("lblfrmCompContdgMonedaExt", "Moneda Ext"), 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(7, "RU", this.LP.GetText("lblfrmCompContdgRU", "RU"), 30, 2, typeof(String), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(8, "Descripcion", this.LP.GetText("lblfrmCompContdgDesc", "Descripción"), 100, 36, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddMaskedTextBoxColumn(9, "Documento", this.LP.GetText("lblfrmCompContdgDocumento", "Documento"), 100, "aa-9999999");
                this.dgDetalle.AddCalendarColumn(10, "Fecha", this.LP.GetText("lblfrmCompContdgFecha", "Fecha"), 100);
                this.dgDetalle.AddCalendarColumn(11, "Vencimiento", this.LP.GetText("lblfrmCompContdgVencimiento", "Vencimiento"), 100);
                this.dgDetalle.AddMaskedTextBoxColumn(12, "Documento2", this.LP.GetText("lblfrmCompContdgDocumento2", "Documento-2"), 100, "aa-999999999");
                this.dgDetalle.AddTextBoxColumn(13, "Importe3", this.LP.GetText("lblfrmCompContdgImporte3", "Importe-3"), 100, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(14, "Iva", this.LP.GetText("lblfrmCompContdgIva", "IVA"), 100, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(15, "CifDni", this.LP.GetText("lblfrmCompContdgCifDni", "Cif/Dni"), 100, 13, typeof(String), DataGridViewContentAlignment.MiddleRight, true);

                //Campos Extendidos
                this.dgDetalle.AddTextBoxColumn(16, "PrefijoDoc", this.LP.GetText("lblfrmCompContdgPrefijoDoc", "Prefijo documento"), 100, 5, typeof(String), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(17, "NumFactAmp", this.LP.GetText("lblfrmCompContdgNumFactAmp", "Número Factura Ampliado"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(18, "NumFactRectif", this.LP.GetText("lblfrmCompContdgNumFactRectif", "Número Factura Rectificada"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddCalendarColumn(19, "FechaServIVA", this.LP.GetText("lblfrmCompContdgFechaServIVA", "Fecha de Servicio"), 100);
                this.dgDetalle.AddTextBoxColumn(20, "CampoUserAlfa1", this.LP.GetText("lblfrmCompContdgCampoUserAlfa1", "Campo de usuario alfa 1"), 140, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(21, "CampoUserAlfa2", this.LP.GetText("lblfrmCompContdgCampoUserAlfa2", "Campo de usuario alfa 2"), 140, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(22, "CampoUserAlfa3", this.LP.GetText("lblfrmCompContdgCampoUserAlfa3", "Campo de usuario alfa 3"), 140, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(23, "CampoUserAlfa4", this.LP.GetText("lblfrmCompContdgCampoUserAlfa4", "Campo de usuario alfa 4"), 140, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(24, "CampoUserAlfa5", this.LP.GetText("lblfrmCompContdgCampoUserAlfa5", "Campo de usuario alfa 5"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(25, "CampoUserAlfa6", this.LP.GetText("lblfrmCompContdgCampoUserAlfa6", "Campo de usuario alfa 6"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(26, "CampoUserAlfa7", this.LP.GetText("lblfrmCompContdgCampoUserAlfa7", "Campo de usuario alfa 7"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(27, "CampoUserAlfa8", this.LP.GetText("lblfrmCompContdgCampoUserAlfa8", "Campo de usuario alfa 8"), 140, 25, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                this.dgDetalle.AddTextBoxColumn(28, "CampoUserNum1", this.LP.GetText("lblfrmCompContdgCampoUserNum1", "Campo usuario numérico 1"), 140, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddTextBoxColumn(29, "CampoUserNum2", this.LP.GetText("lblfrmCompContdgCampoUserNum2", "Campo usuario numérico 2"), 140, 15, typeof(Decimal), DataGridViewContentAlignment.MiddleRight, true);
                this.dgDetalle.AddCalendarColumn(30, "CampoUserFecha1", this.LP.GetText("lblfrmCompContdgCampoUserFecha1", "Campo usuario fecha 1"), 140);
                this.dgDetalle.AddCalendarColumn(31, "CampoUserFecha2", this.LP.GetText("lblfrmCompContdgCampoUserFecha2", "Campo usuario fecha 2"), 140);

                if (this.edicionLote || this.edicionLoteError || this.edicionComprobanteGLB01 || this.nuevoComprobanteGLB01) 
                    this.dgDetalle.AddTextBoxColumn(32, "RowNumber", "RowNumber", 20, 15, typeof(String), DataGridViewContentAlignment.MiddleLeft, false);

                if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01)
                {
                    //this.dgDetalle.AddTextBoxColumn(33, "Pais", this.LP.GetText("lblfrmCompContdgCampoPais", "País"), 30, 2, typeof(String), DataGridViewContentAlignment.MiddleLeft, true);
                    if (this.NuevoComprobanteGLB01) EsNuevo = true;
                    else EsNuevo = false;
                    //ComboBox para los paises
                    DataTable tableSourcePaises = new DataTable("tableSourcePaises");
                    tableSourcePaises.Columns.AddRange(new DataColumn[] {
                    new DataColumn("id"),
                    new DataColumn("desc") });
                    tableSourcePaises.Rows.Add("", "");
                    
                    //Falta Cargar los paises
                    IDataReader dr = null;
                    try
                    {
                        string prefijoTabla = "";
                        string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
                        //string proveedorDatosCG = ConfigurationManager.AppSettings["proveedorDatosCG"];
                        if (proveedorTipo == "DB2")
                        {
                            prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                            if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                        }
                        else prefijoTabla = GlobalVar.PrefijoTablaCG;

                        string query = "select * from " + prefijoTabla + "GLT30 ";
                        query += "order by NOMB30";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        while (dr.Read())
                        {
                            //tableSourcePaises.Rows.Add("ES", "España");
                            tableSourcePaises.Rows.Add(dr["PCIF30"].ToString().Trim(), dr["NOMB30"].ToString().Trim());     //Traducir ¿?¿?
                        }

                        dr.Close();
                    }
                    catch (Exception ex) 
                    { 
                        Log.Error(Utiles.CreateExceptionString(ex));

                        if (dr != null) dr.Close();
                    }

                    DataGridViewComboBoxColumn columnComboBoxPaises = new DataGridViewComboBoxColumn
                    {
                        Name = "Pais",
                        DataPropertyName = "Pais",
                        Width = 60,
                        DropDownWidth = 160,
                        MaxDropDownItems = 15,
                        FlatStyle = FlatStyle.Flat,

                        DataSource = tableSourcePaises,
                        DisplayMember = "desc",
                        ValueMember = "id",
                        ValueType = typeof(string)
                    };
                    this.dgDetalle.Columns.Insert(33, columnComboBoxPaises);
                }
                
                //Fijar las columnas hasta la columna de Moneda Local
                //this.dgDetalle.Columns["MonedaLocal"].Frozen = true;

                for (int i = 0; i < this.dgDetalle.ColumnCount; i++)
                {
                    this.dgDetalle.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                //QUITAR !!!! this.dgDetalle.dsDatos.Tables["Detalle"].ColumnChanged += new DataColumnChangeEventHandler(frmCompContAltaEdita_ColumnChanged);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            //Crear la propiedad ComboValores para las columnas de tipo DataGridViewComboBoxColumn
             //               tableSource.Rows.Add("D", "Debe");
              //  tableSource.Rows.Add("H", "Haber")
            
            string[,] valoresCombo = new string[,]
	        {
	            {this.LP.GetText("lblCompContDebe", "Debe"), "D"},
	            {this.LP.GetText("lblCompContHaber", "Haber"), "H"}
	        
	        };
            this.dgDetalle.ComboValores = valoresCombo;

            /*
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetalle.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDetalle.DefaultCellStyle = dataGridViewCellStyle2;
            
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetalle.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            */
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
            try
            {
                string query = "";
                if (!this.edicionComprobanteGLB01)
                {
                    query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 where STATMG='V' order by CCIAMG";
                }
                else
                {
                    query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 where CCIAMG = '" + this.comprobanteContableImportar.Cab_compania + "'";
                }

                string result = this.FillComboBox(query, "CCIAMG", "NCIAMG", ref this.cmbCompania, true, -1, false);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    string mensaje = this.LP.GetText("errGetCompanias", "Error obteniendo las compañías") + " (" + result + ")";
                    RadMessageBox.Show(mensaje, error);
                }
                else
                {
                    if (this.edicionComprobanteGLB01 && this.cmbCompania.Items.Count > 0) this.cmbCompania.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los tipos de comprobantes
        /// </summary>
        private void FillTiposComprobantes()
        {
            string query = "select TIVOTV, NOMBTV  from ";
            query += GlobalVar.PrefijoTablaCG + "GLT06 ";

            //if (this.edicionComprobanteGLB01) query += "where TIVOTV = '" + this.comprobanteContableImportar.Cab_tipo + "' ";
            //else query += "where CODITV='0' "; //jl
            if (batch) query += "where CODITV='1' ";
            //else query += "where CODITV='0' ";

            // if (this.edicionComprobanteGLB01) query += "where CODITV='1' and TIVOTV = '" + this.comprobanteContableImportar.Cab_tipo + "' ";
            // else if (this.nuevoComprobanteGLB01) query += "where STATTV='V' and CODITV='0' ";
            //else query += "where STATTV='V' and CODITV='1' ";

            query += "order by TIVOTV";

            string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, false);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de comprobantes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
            else
            {
                if (this.edicionComprobanteGLB01 && this.cmbTipo.Items.Count > 0) this.cmbTipo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Formato para las fechas (dado parámetro de CG)
        /// </summary>
        private void CrearFechaConFormatoCG()
        {
            this.dateTimePickerFecha.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerFecha.CustomFormat = GlobalVar.CGFormatoFecha;
            DateTime localDate = DateTime.Now;
            this.dateTimePickerFecha.Value = localDate;
        }

        /// <summary>
        /// Recupera el comprobante y llena los controles del formulario con los datos del comprobante
        /// </summary>
        private void CargarDatosComprobante()
        {
            try
            {
                //Leer el comprobante
                string ficheroComp = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];
                ficheroComp = ficheroComp + "\\" + this.archivoComprobante;

                ds = new DataSet();
                ds.ReadXml(ficheroComp);

                //Verificar que exista la tabla de Cabecera
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                {
                    //Verificar la marca contable para asegurar que sea un comprobante contable
                    if (ds.Tables["Cabecera"].Rows[0]["Contable"].ToString() == "1")
                    {
                        //Validar la compañia
                        string codigo = ds.Tables["Cabecera"].Rows[0]["Compania"].ToString();
                        string validarCompania = this.ValidarCompania(codigo);
                        if (validarCompania == "")
                        {
                            compania_ant = codigo;
                            this.cmbCompania.SelectedValue = codigo;
                        }

                        this.txtMaskAAPP.Text = ds.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                        aapp_ant = this.txtMaskAAPP.Text;
                        //if (this.cmbTipo.SelectedValue != null)
                        //{
                        codigo = ds.Tables["Cabecera"].Rows[0]["Tipo"].ToString();
                        tipo_ant = codigo;
                        //}
                        //else
                        //{
                            //codigo = "";
                        //}
                        
                        string validarTipo = this.ValidarTipo(codigo);
                        if (validarTipo == "") this.cmbTipo.SelectedValue = codigo;

                        this.txtNoComprobante.Text = ds.Tables["Cabecera"].Rows[0]["Numero"].ToString();
                        nocomp_ant = this.txtNoComprobante.Text;
                        string fecha = ds.Tables["Cabecera"].Rows[0]["Fecha"].ToString();
                        this.dateTimePickerFecha.Text = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                        fecha_ant = this.dateTimePickerFecha.Text;

                        string clase = ds.Tables["Cabecera"].Rows[0]["Clase"].ToString();
                        try
                        {
                            if (Convert.ToInt16(clase) < this.cmbClase.Items.Count)
                            {
                                //this.cmbClase.SelectedItem = this.cmbClase.Items.Equals(clase);
                                this.cmbClase.SelectedIndex = this.cmbClase.FindString(clase);                               //this.cmbClase.SelectedIndex = this.cmbClase.Items.IndexOf("1 - ML calcula ME");
                                //this.cmbClase.SelectedItem = Convert.ToInt16(clase) + 1;
                                //this.cmbClase.Refresh();
                            }
                            //else   (antes era un Combo ahora es un Desplegable (si no es un valor correcto no se inicializa)
                            //    this.cmbClase.Text = clase;
                        }
                        catch(Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            this.cmbClase.Text = clase;
                        }
                       
                        this.txtTasa.Text = ds.Tables["Cabecera"].Rows[0]["Tasa"].ToString();
                        this.txtDescripcion.Text = ds.Tables["Cabecera"].Rows[0]["Descripcion"].ToString();
                        string revertirValor = ds.Tables["Cabecera"].Rows[0]["Revertir"].ToString();
                        switch (revertirValor)
                        {
                            case REVERTIRNo:            //N
                                this.radDropDownListRevertir.SelectedValue = REVERTIRNo;
                                break;
                            case REVERTIRImportes:      //S
                                this.radDropDownListRevertir.SelectedValue = REVERTIRImportes;
                                break;
                            case REVERTIRImportesDH:    //T
                                this.radDropDownListRevertir.SelectedValue = REVERTIRImportesDH;
                                break;
                            default:
                                this.radDropDownListRevertir.SelectedValue = REVERTIRNo;
                                break;
                        }
                        
                        //Verificar que exista la tabla de Totales
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Totales"].Rows.Count > 0)
                        {

                            this.lblMonedaLocal_Debe.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString(), this.LP.MyCultureInfo);
                            this.lblMonedaLocal_Haber.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString(), this.LP.MyCultureInfo);
                            this.lblMonedaExtranjera_Debe.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaExtDebe"].ToString(), this.LP.MyCultureInfo);
                            this.lblMonedaExtranjera_Haber.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["MonedaExtHaber"].ToString(), this.LP.MyCultureInfo);
                            this.lblImporte3_Debe.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["Importe3Debe"].ToString(), this.LP.MyCultureInfo);
                            this.lblImporte3_Haber.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["Importe3Haber"].ToString(), this.LP.MyCultureInfo);
                            //this.lblNoApuntes_Valor.Text = utiles.ImporteFormato(ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString(), this.LP.MyCultureInfo);
                            this.lblNoApuntes_Valor.Text = ds.Tables["Totales"].Rows[0]["NumeroApuntes"].ToString();
                        }

                        //Verificar que exista la tabla de Detalle
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                        {
                            this.dgDetalle.dsDatos = ds;
                            this.dgDetalle.DataSource = ds.Tables["Detalle"];

                            bool existeDetalle = false;
                            if (this.ds.Tables["Detalle"].Rows.Count == 1)
                            {
                                if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], 0))
                                {
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    existeDetalle = true;
                                }
                                else
                                {
                                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                                }
                            }
                            else
                            {
                                if (this.ds.Tables["Detalle"].Rows.Count > 1)
                                {
                                    existeDetalle = true;
                                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], this.ds.Tables["Detalle"].Rows.Count-1))
                                    {
                                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    }
                                }
                                else
                                {
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                                    existeDetalle = true;
                                }
                            }

                            if (existeDetalle)
                            {
                                //Habilitar / Deshabilitar Columnas y Dar Formato a las Fechas
                                bool actualizarColumnasDadoCtaMayor = (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count <= 500);

                                //SMR// for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                                for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count - 1; i++)
                                {
                                    if (actualizarColumnasDadoCtaMayor) this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);

                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"] = utiles.ImporteFormato(ds.Tables["Detalle"].Rows[i]["MonedaLocal"].ToString(), this.LP.MyCultureInfo);
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"] = utiles.ImporteFormato(ds.Tables["Detalle"].Rows[i]["MonedaExt"].ToString(), this.LP.MyCultureInfo);
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"] = utiles.ImporteFormato(ds.Tables["Detalle"].Rows[i]["Importe3"].ToString(), this.LP.MyCultureInfo);

                                    //Fechas
                                    try
                                    {
                                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"].ToString(), false);
                                    }
                                    catch { /*Log.Error(Utiles.CreateExceptionString(ex)); */}
                                    try
                                    {
                                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"].ToString(), false);
                                    }
                                    catch { /*Log.Error(Utiles.CreateExceptionString(ex));*/ }
                                    
                                    if (this.extendido)
                                    {
                                        try
                                        {
                                             this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"].ToString(), false);
                                        }
                                        catch { /*Log.Error(Utiles.CreateExceptionString(ex));*/ }
                                        try
                                        {
                                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"].ToString(), false);
                                        }
                                        catch { /*Log.Error(Utiles.CreateExceptionString(ex));*/ }
                                        try
                                        {
                                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"].ToString(), false);
                                        }
                                        catch { /*Log.Error(Utiles.CreateExceptionString(ex)); */}
                                    }
                                }

                                this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void ImportarDatosComprobante()
        {
            bool errorTipo = false;
            try
            {
                string compania = this.comprobanteContableImportar.Cab_compania.Trim();
                if (compania != "")
                {
                    if (!this.edicionComprobanteGLB01)
                    try
                    {
                        this.cmbCompania.SelectedValue = compania;
                        if (this.cmbCompania.SelectedValue == null)
                        {
                            RadMessageBox.Show("La compañía " + compania + "no es válida. Debe seleccionar una.");
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("La compañía " + compania + "no es válida. Debe seleccionar una.");
                    }
                }
                else RadMessageBox.Show("Debe seleccionar la compañía.");

                string sigloanoper = this.comprobanteContableImportar.Cab_anoperiodo;
                if (sigloanoper.Length == 5) sigloanoper = sigloanoper.Substring(1, 4);
                this.txtMaskAAPP.Text = sigloanoper;

                this.dateTimePickerFecha.Value = utiles.FormatoCGToFecha(this.comprobanteContableImportar.Cab_fecha);

                String tipo = this.comprobanteContableImportar.Cab_tipo.Trim();
                if (tipo != "")
                {
                    //if (!this.edicionComprobanteGLB01)
                    if (this.edicionComprobanteGLB01 || (Batch==true && BatchLote==true))
                        try
                    {
                            this.cmbTipo.SelectedValue = tipo;
                            if (this.cmbTipo.SelectedValue == null)
                        {
                            RadMessageBox.Show("El tipo " + tipo + " no es válido. Debe seleccionar uno.");
                            errorTipo = true;
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show("El tipo " + tipo + " no es válido. Debe seleccionar uno.");
                        errorTipo = true;
                    }
                }
                else
                {
                    RadMessageBox.Show(" Debe seleccionar el tipo.");
                    errorTipo = true;
                }
                
                if (this.comprobanteContableImportar.Cab_clase.Trim() != "")
                {
                    try { this.cmbClase.SelectedValue = this.comprobanteContableImportar.Cab_clase; }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                this.txtTasa.Text = this.comprobanteContableImportar.Cab_tasa;
                this.txtNoComprobante.Text = this.comprobanteContableImportar.Cab_noComprobante;
                this.txtDescripcion.Text = this.comprobanteContableImportar.Cab_descripcion;

                if (this.comprobanteContableImportar.Cab_extendido != "") 
                {
                    if (this.comprobanteContableImportar.Cab_extendido == "1") this.extendido = true;
                    else this.extendido = false;
                }
                else this.extendido = false;

                //Detalles
                this.dgDetalle.dsDatos.Tables.Add(this.comprobanteContableImportar.Det_detalles);
                this.dgDetalle.DataSource = this.dgDetalle.dsDatos.Tables["Detalle"];

                bool existeDetalle = false;

                if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count == 1)
                {
                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], 1))
                    {
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                        existeDetalle = true;
                    }
                    else
                    {
                        //Se podría dejar sin seleccionar la fila pero no lo consigo
                        //this.dgDetalle.CurrentRow.Selected = false;
                        //this.dgDetalle.Rows[0].Selected = false;
                    }
                }
                else
                {
                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add();
                    existeDetalle = true;
                }

                if (existeDetalle)
                {
                    //Validar compañia para recuperar el campo GLM01.TIPLMG
                    // string result = this.ValidarCompania(this.comprobanteContableImportar.Cab_compania);
                    string result = this.QueryGLM01(this.comprobanteContableImportar.Cab_compania);
                    //Habilitar / Deshabilitar Columnas y Dar Formato a las Fechas
                    string fecha;
                    string vencimiento;
                    string fechaServ;
                    string USF1;
                    string USF2;
                    
                    bool actualizarColumnasDadoCtaMayor = (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count <= 500);

                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count - 1; i++)
                    {
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"] = utiles.ImporteFormato(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"].ToString(), this.LP.MyCultureInfo);
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"] = utiles.ImporteFormato(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"].ToString(), this.LP.MyCultureInfo);
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"] = utiles.ImporteFormato(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"].ToString(), this.LP.MyCultureInfo);

                        if (this.extendido)
                        {
                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum1"] = utiles.ImporteFormato(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum1"].ToString(), this.LP.MyCultureInfo);
                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum2"] = utiles.ImporteFormato(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum2"].ToString(), this.LP.MyCultureInfo);
                        }

                        fecha = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"].ToString().Trim();
                        vencimiento = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"].ToString().Trim();

                        if (fecha != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"] = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                        if (vencimiento != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"] = utiles.FormatoCGToFecha(vencimiento).ToShortDateString();

                        if (this.extendido)
                        {
                            fechaServ = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"].ToString().Trim();
                            USF1 = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"].ToString().Trim();
                            USF2 = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"].ToString().Trim();

                            if (fechaServ != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"] = utiles.FormatoCGToFecha(fechaServ).ToShortDateString();
                            if (USF1 != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"] = utiles.FormatoCGToFecha(USF1).ToShortDateString();
                            if (USF2 != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"] = utiles.FormatoCGToFecha(USF2).ToShortDateString();
                        }

                        //QUITAR COMENTARIO
                        if (actualizarColumnasDadoCtaMayor) this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);

                    }

                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                }

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (errorTipo) this.cmbTipo.Focus();
            else this.cmbCompania.Focus();
        }

        /// <summary>
        /// Crea la tabla Cabecera del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaCabeceraVacia()
        {
            DataTable dtCabecera = new DataTable
            {
                TableName = "Cabecera"
            };

            dtCabecera.Columns.Add("Contable", typeof(string));
            dtCabecera.Columns.Add("Transferido", typeof(string));
            dtCabecera.Columns.Add("Extendido", typeof(string));
            dtCabecera.Columns.Add("Compania", typeof(string));
            dtCabecera.Columns.Add("AnoPeriodo", typeof(string));
            dtCabecera.Columns.Add("Tipo", typeof(string));
            dtCabecera.Columns.Add("Numero", typeof(string));
            dtCabecera.Columns.Add("Fecha", typeof(string));
            dtCabecera.Columns.Add("Clase", typeof(string));
            dtCabecera.Columns.Add("Tasa", typeof(string));
            dtCabecera.Columns.Add("Descripcion", typeof(string));
            dtCabecera.Columns.Add("Revertir", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtCabecera);

            DataRow row = this.dgDetalle.dsDatos.Tables["Cabecera"].NewRow();
            row["Contable"] = "1";
            row["Transferido"] = "0";
            row["Extendido"] = "0";
            row["Compania"] = "";
            row["AnoPeriodo"] = "";
            row["Tipo"] = "";
            row["Numero"] = "";
            row["Fecha"] = "";
            row["Clase"] = "";
            row["Tasa"] = "";
            row["Descripcion"] = "";
            row["Revertir"] = "";
            
            this.dgDetalle.dsDatos.Tables["Cabecera"].Rows.Add(row);

        }

        /// <summary>
        /// Crea la tabla Totales del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaTotalesVacia()
        {
            DataTable dtTotales = new DataTable
            {
                TableName = "Totales"
            };

            dtTotales.Columns.Add("MonedaLocalDebe", typeof(string));
            dtTotales.Columns.Add("MonedaLocalHaber", typeof(string));
            dtTotales.Columns.Add("MonedaExtDebe", typeof(string));
            dtTotales.Columns.Add("MonedaExtHaber", typeof(string));
            dtTotales.Columns.Add("Importe3Debe", typeof(string));
            dtTotales.Columns.Add("Importe3Haber", typeof(string));
            dtTotales.Columns.Add("NumeroApuntes", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtTotales);

            DataRow row = this.dgDetalle.dsDatos.Tables["Totales"].NewRow();
            row["MonedaLocalDebe"] = "";
            row["MonedaLocalHaber"] = "";
            row["MonedaExtDebe"] = "";
            row["MonedaExtHaber"] = "";
            row["Importe3Debe"] = "";
            row["Importe3Haber"] = "";
            row["NumeroApuntes"] = "";

            this.dgDetalle.dsDatos.Tables["Totales"].Rows.Add(row);
        }

        /// <summary>
        /// Crea la tabla Detalle del DataSet para el Comprobante Contable
        /// </summary>
        private void DataSetCrearTablaDetalleVacia()
        {
            DataTable dtDetalle = new DataTable
            {
                TableName = "Detalle"
            };

            dtDetalle.Columns.Add("Cuenta", typeof(string));
            //dtDetalle.Columns.Add("Cuenta", typeof(TGTexBoxSel));
            dtDetalle.Columns.Add("Auxiliar1", typeof(string));
            dtDetalle.Columns.Add("Auxiliar2", typeof(string));
            dtDetalle.Columns.Add("Auxiliar3", typeof(string));
            dtDetalle.Columns.Add("DH", typeof(string));
            dtDetalle.Columns.Add("MonedaLocal", typeof(string));
            dtDetalle.Columns.Add("MonedaExt", typeof(string));
            dtDetalle.Columns.Add("RU", typeof(string));
            dtDetalle.Columns.Add("Descripcion", typeof(string));
            dtDetalle.Columns.Add("Documento", typeof(string));
            dtDetalle.Columns.Add("Fecha", typeof(string));
            dtDetalle.Columns.Add("Vencimiento", typeof(string));
            dtDetalle.Columns.Add("Documento2", typeof(string));
            dtDetalle.Columns.Add("Importe3", typeof(string));
            dtDetalle.Columns.Add("Iva", typeof(string));
            dtDetalle.Columns.Add("CifDni", typeof(string));

            //Extendidos
            dtDetalle.Columns.Add("PrefijoDoc", typeof(string));
            dtDetalle.Columns.Add("NumFactAmp", typeof(string));
            dtDetalle.Columns.Add("NumFactRectif", typeof(string));
            dtDetalle.Columns.Add("FechaServIVA", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa1", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa2", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa3", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa4", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa5", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa6", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa7", typeof(string));
            dtDetalle.Columns.Add("CampoUserAlfa8", typeof(string));
            dtDetalle.Columns.Add("CampoUserNum1", typeof(string));
            dtDetalle.Columns.Add("CampoUserNum2", typeof(string));
            dtDetalle.Columns.Add("CampoUserFecha1", typeof(string));
            dtDetalle.Columns.Add("CampoUserFecha2", typeof(string));

            //if (this.edicionLote || this.edicionLoteError || this.edicionComprobanteGLB01) dtDetalle.Columns.Add("RowNumber", typeof(string));
            dtDetalle.Columns.Add("RowNumber", typeof(string)); //jl
            if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) dtDetalle.Columns.Add("Pais", typeof(string));

            this.dgDetalle.dsDatos.Tables.Add(dtDetalle);

            DataRow row = this.dgDetalle.dsDatos.Tables["Detalle"].NewRow();
            
            row["Cuenta"] = "";
            row["Auxiliar1"] = "";
            row["Auxiliar2"] = "";
            row["Auxiliar3"] = "";
            row["DH"] = "";
            row["MonedaLocal"] = "";
            row["MonedaExt"] = "";
            row["RU"] = "";
            row["Descripcion"] = "";
            row["Documento"] = "";
            row["Fecha"] = "";
            row["Vencimiento"] = "";
            row["Documento2"] = "";
            row["Importe3"] = "";
            row["Iva"] = "";
            row["CifDni"] = "";

            //Extendidos
            row["PrefijoDoc"] = "";
            row["NumFactAmp"] = "";
            row["NumFactRectif"] = "";
            row["FechaServIVA"] = "";
            row["CampoUserAlfa1"] = "";
            row["CampoUserAlfa2"] = "";
            row["CampoUserAlfa3"] = "";
            row["CampoUserAlfa4"] = "";
            row["CampoUserAlfa5"] = "";
            row["CampoUserAlfa6"] = "";
            row["CampoUserAlfa7"] = "";
            row["CampoUserAlfa8"] = "";
            row["CampoUserNum1"] = "";
            row["CampoUserNum2"] = "";
            row["CampoUserFecha1"] = "";
            row["CampoUserFecha2"] = "";

            if (this.edicionLote || this.edicionLoteError || this.edicionComprobanteGLB01) row["RowNumber"] = "";
            
            if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) row["Pais"] = "";
            
            this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add(row);

            //Poner el data source del Grid a la tabla detalle del DataSet
            this.dgDetalle.DataSource = this.dgDetalle.dsDatos.Tables["Detalle"];

            this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
        } 

        /// <summary>
        /// Crea las tablas del dataset vacías
        /// </summary>
        private void CrearTablasDataSetVacias()
        {
            try
            {
                //Crea la tabla Cabecera
                this.DataSetCrearTablaCabeceraVacia();

                //Crea la tabla Totales
                this.DataSetCrearTablaTotalesVacia();

                if (!this.importarComprobante)
                {
                    //Crea la tabla Detalle
                    this.DataSetCrearTablaDetalleVacia();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve si todas las columnas están vacías dado una fila de un DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="row">fila</param>
        /// <returns></returns>
        private bool TodaFilaEnBlanco(DataTable table, int row)
        {
            bool todaFilaBlanco = true;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Rows[row][i].ToString() != "")
                {
                    todaFilaBlanco = false;
                    break;
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarTablaCabeceraDesdeForm()
        {
            try
            {
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Contable"] = "1";
                //Siempre se guardará como No Transferido
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Transferido"] = "0";
                if (this.extendido) this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Extendido"] = "1";
                else this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Extendido"] = "0";

                string codigo = this.cmbCompania.SelectedValue.ToString();
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Compania"] = codigo;
                
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Text;

                codigo = this.cmbTipo.SelectedValue.ToString();
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tipo"] = codigo;
                
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Numero"] = this.txtNoComprobante.Text;
                string fecha = this.dateTimePickerFecha.Value.Year.ToString() + this.dateTimePickerFecha.Value.Month.ToString().PadLeft(2, '0') +
                               this.dateTimePickerFecha.Value.Day.ToString().PadLeft(2, '0');
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Fecha"] = fecha;
                string clase = this.cmbClase.Text;
                if (clase != null && clase != "")
                {
                    string[] aux = clase.Split('-');
                    if (aux.Length > 0) clase = aux[0].Trim();
                    else clase = "";
                }
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Clase"] = clase;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tasa"] = this.txtTasa.Text;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Descripcion"] = this.txtDescripcion.Text;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Revertir"] = this.radDropDownListRevertir.SelectedValue.ToString();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarTablaTotalesDesdeForm()
        {
            try
            {
                //Recalcular Totales
                this.CalcularTotales();

                /*this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"] = this.lblMonedaLocal_Debe.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"] = this.lblMonedaLocal_Haber.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtDebe"] = this.lblMonedaExtranjera_Debe.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtHaber"] = this.lblMonedaExtranjera_Haber.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Debe"] = this.lblImporte3_Debe.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Haber"] = this.lblImporte3_Haber.Text;
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["NumeroApuntes"] = this.lblNoApuntes_Valor.Text;*/
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza los campos fechas del detalle del comprobante para que sean grabados correctamente (formato aaaammdd) o
        /// dado si estan en el formato anterior los convierte en dd/mm/yyyy
        /// </summary>
        /// <param name="grabar">si la fecha es para grabar el comprobante en el fichero o no</param>
        private void ActualizarTablaDetallesFechas(bool fechaGrabar)
        {
            try
            {
                if (this.dgDetalle.dsDatos != null && this.dgDetalle.dsDatos.Tables != null &&
                    this.dgDetalle.dsDatos.Tables.Count > 0 && this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
                {
                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                    {
                        if (fechaGrabar == true)
                        {
                            if ((this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"]).ToString() != "")
                            {
                                DateTime Fecha = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"]);
                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"] = Fecha.Year.ToString() + Fecha.Month.ToString().PadLeft(2, '0') + Fecha.Day.ToString().PadLeft(2, '0');
                            }
                            if ((this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"]).ToString() != "")
                            {
                                DateTime Fecha = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"]);
                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"] = Fecha.Year.ToString() + Fecha.Month.ToString().PadLeft(2, '0') + Fecha.Day.ToString().PadLeft(2, '0');
                            }
                            if (this.extendido)
                            {
                                if ((this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"]).ToString() != "")
                                {
                                    DateTime Fecha = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"]);
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"] = Fecha.Year.ToString() + Fecha.Month.ToString().PadLeft(2, '0') + Fecha.Day.ToString().PadLeft(2, '0');
                                }
                                if ((this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"]).ToString() != "")
                                {
                                    DateTime Fecha = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"]);
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"] = Fecha.Year.ToString() + Fecha.Month.ToString().PadLeft(2, '0') + Fecha.Day.ToString().PadLeft(2, '0');
                                }
                                if ((this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"]).ToString() != "")
                                {
                                    DateTime Fecha = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"]);
                                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"] = Fecha.Year.ToString() + Fecha.Month.ToString().PadLeft(2, '0') + Fecha.Day.ToString().PadLeft(2, '0');
                                }
                            }
                        }
                        else
                        {
                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"].ToString(), fechaGrabar);
                            this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"].ToString(), fechaGrabar);
                            if (this.extendido)
                            {
                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"].ToString(), fechaGrabar);
                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"].ToString(), fechaGrabar);
                                this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"].ToString(), fechaGrabar);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Dado una fecha la convierte en aaaammdd si es para grabar en el fichero o en dd/mm/aaaa si es para seguir en edición del comprobante
        /// </summary>
        /// <param name="fecha">fecha</param>
        /// <param name="fechaGrabar">si la fecha es para grabar el comprobante en el fichero o no</param>
        /// <returns></returns>
        private string FechaDetalle(string fecha, bool fechaGrabar)
        {
            string result = "";
            try
            {
                if (fecha != "")
                {
                    int fechaInt;
                    DateTime dt;

                    if (fechaGrabar)
                    {
                        dt = Convert.ToDateTime(fecha);
                        if (fecha.Length == 10) fechaInt = utiles.FechaToFormatoCG(dt, false, 8);
                        else fechaInt = utiles.FechaToFormatoCG(dt, false);
                        result = fechaInt.ToString();
                    }
                    else
                    {
                        result = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// 
        /// </summary>
        private void GrabarNuevoComprobante(bool duplicar)
        {
            //Directorio donde se almacenan los comprobantes contables
            string pathFicherosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];

            this.saveFileDialogGrabar = new SaveFileDialog
            {
                //Recuperar el directorio por defecto que está en la configuarción
                InitialDirectory = pathFicherosCompContables,
                DefaultExt = "xml",
                //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                Filter = "ficheros xml (*.xml)|*.xml"
            };

            if (duplicar) this.saveFileDialogGrabar.FileName = this.txtDescripcion.Text + " " + this.LP.GetText("lblfrmCompContCopia", "copia");
            else this.saveFileDialogGrabar.FileName = this.txtDescripcion.Text;
            
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            if (DialogResult.OK == this.saveFileDialogGrabar.ShowDialog())
            {
                //Actualizar el nombre del archivo para la opción Grabar
                this.archivoComprobante = System.IO.Path.GetFileName(this.saveFileDialogGrabar.FileName);

                //Eliminar columnas de campos extendidos del detalle del comprobante si procede
                if (!this.extendido) this.EliminarColumnasCamposExtendidos();

                //Eliminar columna RowNumber y Pais si procede
                if (this.dgDetalle.dsDatos != null && this.dgDetalle.dsDatos.Tables != null && this.dgDetalle.dsDatos.Tables.Count > 0 &&
                    this.dgDetalle.dsDatos.Tables["Detalle"] != null)
                {
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("RowNumber")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("RowNumber");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("Pais")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("Pais");
                }

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Grabar el nuevo comprobante
                this.dgDetalle.dsDatos.WriteXml(this.saveFileDialogGrabar.FileName);

                //Actualizar el titulo del comprobante con la descripcion del comprobante
                if (!duplicar) this.Text += this.txtDescripcion.Text;

                //Actualizar el listado de comprobantes del formulario frmCompContLista
                this.ActualizarFormularioListaComprobantes();

                //Ocultar botón de ver comentarios
                this.btnVerComentarios.Visible = false;

                //Pasar a la interface de edicion de comprobante desde xml
                this.nuevoComprobante = false;
                this.importarComprobante = false;
                this.edicionLote = false;
                this.edicionLoteError = false;
                this.nuevoComprobanteGLB01 = false;
                this.edicionComprobanteGLB01 = false;
            }
            else
            {
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActualizarComprobante()
        {
            try
            {
                //Leer el comprobante
                string ficheroComp = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];
                ficheroComp = ficheroComp + "\\" + this.archivoComprobante;

                //Quitar la máscara al campo Año período (grabar la info sin el separador)
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"] = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Grabar el nuevo comprobante
                this.dgDetalle.dsDatos.WriteXml(ficheroComp);

                //Actualizar el listado de comprobantes del formulario frmCompContLista
                this.ActualizarFormularioListaComprobantes();
                gridCambiada = false;
                dNoPreguntar = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el comprobante en las tablas (W00, W01, W10, W11)
        /// </summary>
        private void ActualizarComprobanteTablas()
        {
            try
            {
                string prefijo = "";
                if (comprobanteContableImportar.Prefijo != null) prefijo = comprobanteContableImportar.Prefijo;

                string tablaCabecera = prefijo + "W00";
                string tablaDetalle = prefijo + "W01";

                if (this.extendido)
                {
                    tablaCabecera = prefijo + "W10";
                    tablaDetalle = prefijo + "W11";
                }

                string bibliotecaTablasLoteAS = "";
                if (tipoBaseDatosCG == "DB2")
                {
                    if (comprobanteContableImportar.Biblioteca != null && comprobanteContableImportar.Biblioteca != "") bibliotecaTablasLoteAS = comprobanteContableImportar.Biblioteca + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                string aapp = this.comprobanteContableImportar.Cab_anoperiodo.PadRight(4, ' ');
                string anno = aapp.Substring(0, 2).Trim();
                string periodo = aapp.Substring(2, 2).Trim();

                //Actualizar la tabla cabecera del comprobante
                string result = this.ActualizarComprobanteTablaCabecera(tablaCabecera, bibliotecaTablasLoteAS, anno, periodo);

                //Actualizar detalle del comprobante
                result = this.ActualizarComprobanteTablaDetalle(tablaDetalle, bibliotecaTablasLoteAS, anno, periodo);
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Actualiza la tabla cabecera del comprobante (W00 o W10)
        /// </summary>
        /// <param name="tablaCabecera"></param>
        /// <param name="bibliotecaTablasLoteAS"></param>
        /// <param name="anno"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private string ActualizarComprobanteTablaCabecera(string tablaCabecera, string bibliotecaTablasLoteAS, string anno, string periodo)
        {
            string result = "";

            try
            {
                //string TTRAWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Transferido"].ToString();
                string TTRAWS = "1";
                string CCIAWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Compania"].ToString();
                string ANOCWS = "";
                string LAPSWS = "";
                string aapp = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                int posSep = aapp.IndexOf('-');
                if (posSep != -1)
                {
                    ANOCWS = aapp.Substring(0, posSep);
                    LAPSWS = aapp.Substring(posSep + 1, aapp.Length - ANOCWS.Length - 1);
                }
                string TICOWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tipo"].ToString();
                string NUCOWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Numero"].ToString();
                string TVOUWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Clase"].ToString();
                string DIAEWS = this.dateTimePickerFecha.Value.Day.ToString().PadLeft(2, '0');
                string MESEWS = this.dateTimePickerFecha.Value.Month.ToString().PadLeft(2, '0');
                string ANOEWS = this.dateTimePickerFecha.Value.Year.ToString().PadLeft(4, '0').Substring(2, 2);
                string TASCWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tasa"].ToString();
                if (TASCWS.Trim() == "") TASCWS = "0";
                TASCWS = TASCWS.Replace(',', '.');
                string TIMOWS = "";
                string STATWS = "";
                string DOCRWS = "";
                string DOCDWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Descripcion"].ToString();

                //Actualizar cabecera del comprobante
                
                string query = "update " + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + tablaCabecera + " set ";
                query += "TTRAWS = '" + TTRAWS + "', CCIAWS = '" + CCIAWS + "', ";

                if (tipoBaseDatosCG == "DB2") query += "AÑOCWS = " + ANOCWS + ", ";
                else query += "AVOCWS = " + ANOCWS + ", ";

                query += "LAPSWS = " + LAPSWS + ", TICOWS = " + TICOWS + ", NUCOWS = " + NUCOWS + ", TVOUWS = ";
                query += TVOUWS + ", DIAEWS = " + DIAEWS + ", MESEWS = " + MESEWS + ", ";

                if (tipoBaseDatosCG == "DB2") query += "AÑOEWS = " + ANOEWS + ", ";
                else query += "AVOEWS = " + ANOEWS + ", ";

                query += "TASCWS = " + TASCWS + ", TIMOWS = '" + TIMOWS + "', STATWS = '" + STATWS + "', DOCRWS = '" + DOCRWS + "', DOCDWS = '" + DOCDWS + "' ";
               
                // wheres
                query += "where ";
                query += "CCIAWS = '" + this.comprobanteContableImportar.Cab_compania + "' and ";

                aapp = this.comprobanteContableImportar.Cab_anoperiodo.PadRight(4, ' ');
                if (tipoBaseDatosCG == "DB2") query += "AÑOCWS = " + anno + " and ";
                else query += "AVOCWS = " + anno + " and ";

                query += "LAPSWS = " + periodo + " and ";
                query += "TICOWS = " + this.comprobanteContableImportar.Cab_tipo + " and ";
                query += "NUCOWS = " + this.comprobanteContableImportar.Cab_noComprobante;

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errActualizarCabCompCont", "Error actualizando la cabecera del comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }


        /// <summary>
        /// Actualiza la tabla detalle del comprobante (W01 o W11)
        /// </summary>
        /// <param name="tablaDetalle"></param>
        /// <param name="bibliotecaTablasLoteAS"></param>
        /// <param name="anno"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private string ActualizarComprobanteTablaDetalle(string tablaDetalle, string bibliotecaTablasLoteAS, string anno, string periodo)
        {
            string result = "";

            try
            {
                if (!this.extendido)
                {
                    //Eliminar columnas de campos extendidos del detalle del comprobante
                    this.EliminarColumnasCamposExtendidos();
                }

                if (this.dgDetalle.dsDatos != null && this.dgDetalle.dsDatos.Tables != null &&
                    this.dgDetalle.dsDatos.Tables.Count > 0 && this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
                {
                    string camposDet = "";
                    string camposDetExt = "";

                    switch (tipoBaseDatosCG)
                    {
                        case "DB2":
                            camposDet = "TTRAWS,CCIAWS,AÑOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                            camposDetExt = "TTRAWS,CCIAWS,AÑOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                            break;
                        case "SQLServer":
                            camposDet = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                            camposDetExt = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                            break;
                        case "Oracle":
                            camposDet = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS";
                            camposDetExt = "TTRAWS,CCIAWS,AVOCWS,LAPSWS,TICOWS,NUCOWS,CUENWS,CAUXWS,DESCWS,MONTWS,TMOVWS,MOSMWS,CLDOWS,NDOCWS,FDOCWS,FEVEWS,TEINWS,NNITWS,AUA1WS,AUA2WS,CDDOWS,NDDOWS,TERCWS,CDIVWS,PRFDWS,NFAAWS,NFARWS,FIVAWS,USA1WS,USA2WS,USA3WS,USA4WS,USA5WS,USA6WS,USA7WS,USA8WS,USN1WS,USN2WS,USF1WS,USF2WS";
                            break;
                    }

                    string TTRAWS = "2";
                    string CCIAWS = "";
                    string ANOCWS = "";
                    string LAPSWS = "";
                    string TICOWS = "";
                    string NUCOWS = "";
                    string CUENWS = "";
                    string CAUXWS = "";
                    string DESCWS = "";
                    string MONTWS = "";
                    decimal montws = 0;
                    string TMOVWS = "";
                    string MOSMWS = "";
                    string CLDOWS = "";
                    string NDOCWS = "";
                    string documento = "";
                    string FDOCWS = "";
                    string FEVEWS = "";
                    string TEINWS = "";
                    string NNITWS = "";
                    string AUA1WS = "";
                    string AUA2WS = "";
                    string CDDOWS = "";
                    string NDDOWS = "";
                    string TERCWS = "";
                    decimal tercws = 0;
                    string CDIVWS = "";
                    decimal mosmws = 0;

                    //Campos extendidos
                    string PRFDWS = "";
                    string NFAAWS = "";
                    string NFARWS = "";
                    string FIVAWS = "";
                    string USA1WS = "";
                    string USA2WS = "";
                    string USA3WS = "";
                    string USA4WS = "";
                    string USA5WS = "";
                    string USA6WS = "";
                    string USA7WS = "";
                    string USA8WS = "";
                    string USN1WS = "";
                    decimal usn1ws = 0;
                    string USN2WS = "";
                    decimal usn2ws = 0;
                    string USF1WS = "";
                    string USF2WS = "";

                    string rowNumber = "";
                    string rowNumberGLBX1 = "";

                    ComprobanteContableTransferir compContTransf = new ComprobanteContableTransferir
                    {
                        bibliotecaTablasLoteAS = bibliotecaTablasLoteAS
                    };
                    //prefijo ¿?¿?

                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                    {
                        if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i))
                        {
                            try
                            {
                                MOSMWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"].ToString();
                                if (MOSMWS != "")
                                {
                                    try { mosmws = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"]); }
                                    catch(Exception ex) 
                                    {
                                        Log.Error(Utiles.CreateExceptionString(ex));

                                        mosmws = 0; 
                                    }
                                }

                                //TTRAWS = "2";
                                //CCIAWS = "";
                                //ANOCWS = "";
                                //LAPSWS = "";
                                //TICOWS = "";
                                //NUCOWS = "";

                                //string TTRAWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Transferido"].ToString();
                                TTRAWS = "2";
                                CCIAWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Compania"].ToString();
                                ANOCWS = "";
                                LAPSWS = "";
                                string aapp = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"].ToString();
                                int posSep = aapp.IndexOf('-');
                                if (posSep != -1)
                                {
                                    ANOCWS = aapp.Substring(0, posSep);
                                    LAPSWS = aapp.Substring(posSep + 1, aapp.Length - ANOCWS.Length - 1);
                                }
                                TICOWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tipo"].ToString();
                                NUCOWS = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Numero"].ToString();

                                CUENWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString().ToUpper();
                                CAUXWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Auxiliar1"].ToString().ToUpper();
                                DESCWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Descripcion"].ToString();
                                DESCWS = DESCWS.Replace("'", "''");
                                
                                MONTWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"].ToString();
                                if (MONTWS != "")
                                {
                                    try { montws = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"]); }
                                    catch (Exception ex)
                                    {
                                        Log.Error(Utiles.CreateExceptionString(ex));

                                        montws = 0; 
                                    }
                                }

                                
                                TMOVWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"].ToString().ToUpper();

                                CLDOWS = "";
                                NDOCWS = "";
                                documento = "";
                                try { documento = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Documento"].ToString(); documento = documento.Replace("-", ""); }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                                if (documento.Length > 2)
                                {
                                    CLDOWS = documento.Substring(0, 2).ToUpper();
                                    NDOCWS = documento.Substring(2, documento.Length - 3);
                                    if (NDOCWS == "") NDOCWS = "0";
                                }
                                else
                                {
                                    CLDOWS = documento.ToUpper();
                                    NDOCWS = "0";
                                }

                                FDOCWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"].ToString();
                                FEVEWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Vencimiento"].ToString();

                                try
                                {
                                    if (FDOCWS != "")
                                    {
                                        if ((FDOCWS.ToString() != "0") && (FDOCWS.Length == 8)) FDOCWS = FDOCWS.Substring(6, 2) + FDOCWS.Substring(4, 2) + FDOCWS.Substring(2, 2);
                                        //fecha = Convert.ToDateTime(FDOCWS);
                                        //FDOCWS = utiles.FechaToFormatoCG(fecha, false).ToString();

                                    }
                                    else FDOCWS = "0";
                                    if (FEVEWS != "")
                                    {
                                        if ((FEVEWS.ToString() != "0") && (FEVEWS.Length == 8)) FEVEWS = FEVEWS.Substring(6, 2) + FEVEWS.Substring(4, 2) + FEVEWS.Substring(2, 2);
                                        //fecha = Convert.ToDateTime(FEVEWS);
                                        //FEVEWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                                    }
                                    else FEVEWS = "0";
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                                TEINWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["RU"].ToString().ToUpper();
                                NNITWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CifDni"].ToString().ToUpper();
                                AUA1WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Auxiliar2"].ToString().ToUpper();
                                AUA2WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Auxiliar3"].ToString().ToUpper();

                                CDDOWS = "";
                                NDDOWS = "";

                                try { documento = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Documento2"].ToString(); documento = documento.Replace("-", ""); }
                                catch(Exception ex) 
                                {
                                    Log.Error(Utiles.CreateExceptionString(ex));

                                    documento = ""; 
                                }

                                if (documento.Length > 2)
                                {
                                    CDDOWS = documento.Substring(0, 2).ToString();
                                    NDDOWS = documento.Substring(2, documento.Length - 3);
                                    if (NDDOWS == "") NDDOWS = "0";
                                }
                                else
                                {
                                    CDDOWS = documento.ToString();
                                    NDDOWS = "0";
                                }

                                TERCWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"].ToString();
                                if (TERCWS != "") 
                                {
                                    try { tercws = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"]); }
                                    catch(Exception ex) 
                                    {
                                        Log.Error(Utiles.CreateExceptionString(ex));

                                        tercws = 0; 
                                    }
                                }

                                CDIVWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Iva"].ToString().ToUpper();

                                if (extendido)
                                {
                                    PRFDWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["PrefijoDoc"].ToString();
                                    NFAAWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["NumFactAmp"].ToString();
                                    NFARWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["NumFactRectif"].ToString();
                                    FIVAWS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["FechaServIVA"].ToString();
                                    USA1WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa1"].ToString();
                                    USA2WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa2"].ToString();
                                    USA3WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa3"].ToString();
                                    USA4WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa4"].ToString();
                                    USA5WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa5"].ToString();
                                    USA6WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa6"].ToString();
                                    USA7WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa7"].ToString();
                                    USA8WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserAlfa8"].ToString();
                                    USN1WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum1"].ToString();
                                    if (USN1WS != "")
                                    {
                                        try { usn1ws = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum1"]); }
                                        catch (Exception ex)
                                        {
                                            Log.Error(Utiles.CreateExceptionString(ex));

                                            usn1ws = 0;
                                        }
                                    }
                                    USN2WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum2"].ToString();
                                    if (USN2WS != "")
                                    {
                                        try { usn2ws = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserNum2"]); }
                                        catch (Exception ex)
                                        {
                                            Log.Error(Utiles.CreateExceptionString(ex));

                                            usn2ws = 0;
                                        }
                                    }
                                    USF1WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha1"].ToString();
                                    USF2WS = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["CampoUserFecha2"].ToString();

                                    try
                                    {
                                        if (FIVAWS != "")
                                        {
                                            if ((FIVAWS.ToString() != "0") && (FIVAWS.Length == 8)) FIVAWS = FIVAWS.Substring(6, 2) + FIVAWS.Substring(4, 2) + FIVAWS.Substring(2, 2);
                                            //fecha = Convert.ToDateTime(FIVAWS);
                                            //FIVAWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                                        }
                                        else FIVAWS = "0";
                                        if (USF1WS != "")
                                        {
                                            if ((USF1WS.ToString() != "0") && (USF1WS.Length == 8)) USF1WS = USF1WS.Substring(6, 2) + USF1WS.Substring(4, 2) + USF1WS.Substring(2, 2);
                                            //fecha = Convert.ToDateTime(USF1WS);
                                            //USF1WS = utiles.FechaToFormatoCG(fecha, false).ToString();
                                        }
                                        else USF1WS = "0";
                                        if (USF2WS != "")
                                        {
                                            if ((USF2WS.ToString() != "0") && (USF2WS.Length == 8)) USF2WS = USF2WS.Substring(6, 2) + USF2WS.Substring(4, 2) + USF2WS.Substring(2, 2);
                                            //fecha = Convert.ToDateTime(USF2WS);
                                            //USF2WS = utiles.FechaToFormatoCG(fecha, false).ToString();
                                        }
                                        else USF2WS = "0";
                                    }
                                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                }

                                //Insertar o Actualizar
                                rowNumber = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["RowNumber"].ToString();
                                if (rowNumber == "")
                                {
                                    //Insertar detalle del comprobante
                                    if (!extendido)
                                    {
                                        result = compContTransf.InsertarDetalleComprobante(tablaDetalle, camposDet, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                                        CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                                        TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS);
                                    }
                                    else
                                    {
                                        result = compContTransf.InsertarDetalleComprobanteExt(tablaDetalle, camposDetExt, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                                            CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                                            TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS,
                                                                            PRFDWS, NFAAWS, NFARWS, FIVAWS, USA1WS, USA2WS, USA3WS, USA4WS, USA5WS, USA6WS,
                                                                            USA7WS, USA8WS, usn1ws, usn2ws, USF1WS, USF2WS);
                                    }
                                }
                                else
                                {
                                    //Actualizar detalle del comprobante
                                    if (!extendido)
                                    {
                                        result = ActualizarDetalleComprobanteTabla(bibliotecaTablasLoteAS + tablaDetalle, rowNumber, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                                        CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                                        TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS);
                                    }
                                    else
                                    {
                                        result = ActualizarDetalleComprobanteExtTabla(bibliotecaTablasLoteAS + tablaDetalle, rowNumber, TTRAWS, CCIAWS, ANOCWS, LAPSWS, TICOWS, NUCOWS,
                                                                            CUENWS, CAUXWS, DESCWS, montws, TMOVWS, mosmws, CLDOWS, NDOCWS, FDOCWS, FEVEWS,
                                                                            TEINWS, NNITWS, AUA1WS, AUA2WS, CDDOWS, NDDOWS, tercws, CDIVWS,
                                                                            PRFDWS, NFAAWS, NFARWS, FIVAWS, USA1WS, USA2WS, USA3WS, USA4WS, USA5WS, USA6WS,
                                                                            USA7WS, USA8WS, usn1ws, usn2ws, USF1WS, USF2WS);
                                    }                                  
                                }


                                //this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"] = this.FechaDetalle(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Fecha"].ToString(), fechaGrabar);
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errActualizarDetCompCont", "Error actualizando el detalle del comprobante") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Actualiza un registro en la tabla detalle del comprobante
        /// </summary>
        /// <param name="nombreTablaDet"></param>
        /// <param name="rowNumber"></param>
        /// <param name="TTRAWS"></param>
        /// <param name="CCIAWS"></param>
        /// <param name="ANOCWS"></param>
        /// <param name="LAPSWS"></param>
        /// <param name="TICOWS"></param>
        /// <param name="NUCOWS"></param>
        /// <param name="CUENWS"></param>
        /// <param name="CAUXWS"></param>
        /// <param name="DESCWS"></param>
        /// <param name="MONTWS"></param>
        /// <param name="TMOVWS"></param>
        /// <param name="MOSMWS"></param>
        /// <param name="CLDOWS"></param>
        /// <param name="NDOCWS"></param>
        /// <param name="FDOCWS"></param>
        /// <param name="FEVEWS"></param>
        /// <param name="TEINWS"></param>
        /// <param name="NNITWS"></param>
        /// <param name="AUA1WS"></param>
        /// <param name="AUA2WS"></param>
        /// <param name="CDDOWS"></param>
        /// <param name="NDDOWS"></param>
        /// <param name="TERCWS"></param>
        /// <param name="CDIVWS"></param>
        public string ActualizarDetalleComprobanteTabla(string nombreTablaDet, string rowNumber, string TTRAWS, string CCIAWS, string ANOCWS,
                                                string LAPSWS, string TICOWS, string NUCOWS, string CUENWS, string CAUXWS, string DESCWS,
                                                decimal MONTWS, string TMOVWS, decimal MOSMWS, string CLDOWS, string NDOCWS, string FDOCWS,
                                                string FEVEWS, string TEINWS, string NNITWS, string AUA1WS, string AUA2WS, string CDDOWS,
                                                string NDDOWS, decimal TERCWS, string CDIVWS)
        {
            string result = "";
            try
            {
                string MONTWS_Cad = MONTWS.ToString().Replace(",", ".");
                string MOSMWS_Cad = MOSMWS.ToString().Replace(",", ".");
                string TERCWS_Cad = TERCWS.ToString().Replace(",", ".");

                //Actualizar el detalle del comprobante
                string query = "update " + nombreTablaDet + " set TTRAWS = '" + TTRAWS + "', CCIAWS  = '" + CCIAWS;
                if (tipoBaseDatosCG == "DB2")
                    query += "', AÑOCWS = " + ANOCWS + ", LAPSWS = " + LAPSWS + ", TICOWS = " + TICOWS + ", NUCOWS = " + NUCOWS + ", CUENWS = '" + CUENWS;
                else
                    query += "', AVOCWS = " + ANOCWS + ", LAPSWS = " + LAPSWS + ", TICOWS = " + TICOWS + ", NUCOWS = " + NUCOWS + ", CUENWS = '" + CUENWS;
                query += "', CAUXWS = '" + CAUXWS + "', DESCWS = '" + DESCWS + "', MONTWS = " + MONTWS_Cad + ", TMOVWS = '" + TMOVWS;
                query += "', MOSMWS = " + MOSMWS_Cad + ", CLDOWS = '" + CLDOWS + "', NDOCWS = " + NDOCWS + ", FDOCWS = " + FDOCWS;
                query += ", FEVEWS = " + FEVEWS + ", TEINWS  = '" + TEINWS + "', NNITWS  = '" + NNITWS + "', AUA1WS  = '" + AUA1WS;
                query += "', AUA2WS = '" + AUA2WS + "', CDDOWS = '" + CDDOWS + "', NDDOWS  = " + NDDOWS + ", TERCWS = " + TERCWS_Cad;
                query += ", CDIVWS = '" + CDIVWS + "' ";
                query += " Where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + nombreTablaDet + ") = " + rowNumber;
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = " + rowNumber;
                        break;
                    case "Oracle":
                        //id_prefijotabla_prefijolote + W01   o W11
                        string campoOracle = "ID_" + nombreTablaDet;
                        query += campoOracle + " = " + rowNumber;
                        break;
                }

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                gridCambiada = false;
                dNoPreguntar = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errUpdateDetComp", "Error actualizando el detalle del comprobante") + " (CCIA: " + CCIAWS + " AAPP: " + utiles.AAPPConFormato(ANOCWS + LAPSWS) + " Tipo: " + TICOWS;      //Falta traducir
                result += " NUCO: " + NUCOWS + ") (" + ex.Message + ")";
            }

            return (result);
        }


        /// <summary>
        /// Actualiza el detalle en la tabla de detalles de campos extendidos
        /// </summary>
        /// <param name="nombreTablaDet"></param>
        /// <param name="rowNumber"></param>
        /// <param name="TTRAWS"></param>
        /// <param name="CCIAWS"></param>
        /// <param name="ANOCWS"></param>
        /// <param name="LAPSWS"></param>
        /// <param name="TICOWS"></param>
        /// <param name="NUCOWS"></param>
        /// <param name="CUENWS"></param>
        /// <param name="CAUXWS"></param>
        /// <param name="DESCWS"></param>
        /// <param name="MONTWS"></param>
        /// <param name="TMOVWS"></param>
        /// <param name="MOSMWS"></param>
        /// <param name="CLDOWS"></param>
        /// <param name="NDOCWS"></param>
        /// <param name="FDOCWS"></param>
        /// <param name="FEVEWS"></param>
        /// <param name="TEINWS"></param>
        /// <param name="NNITWS"></param>
        /// <param name="AUA1WS"></param>
        /// <param name="AUA2WS"></param>
        /// <param name="CDDOWS"></param>
        /// <param name="NDDOWS"></param>
        /// <param name="TERCWS"></param>
        /// <param name="CDIVWS"></param>
        /// <param name="PRFDWS"></param>
        /// <param name="NFAAWS"></param>
        /// <param name="NFARWS"></param>
        /// <param name="FIVAWS"></param>
        /// <param name="USA1WS"></param>
        /// <param name="USA2WS"></param>
        /// <param name="USA3WS"></param>
        /// <param name="USA4WS"></param>
        /// <param name="USA5WS"></param>
        /// <param name="USA6WS"></param>
        /// <param name="USA7WS"></param>
        /// <param name="USA8WS"></param>
        /// <param name="USN1WS"></param>
        /// <param name="USN2WS"></param>
        /// <param name="USF1WS"></param>
        /// <param name="USF2WS"></param>
        public string ActualizarDetalleComprobanteExtTabla(string nombreTablaDet, string rowNumber, string TTRAWS, string CCIAWS, string ANOCWS,
                                        string LAPSWS, string TICOWS, string NUCOWS, string CUENWS, string CAUXWS, string DESCWS,
                                        decimal MONTWS, string TMOVWS, decimal MOSMWS, string CLDOWS, string NDOCWS, string FDOCWS,
                                        string FEVEWS, string TEINWS, string NNITWS, string AUA1WS, string AUA2WS, string CDDOWS,
                                        string NDDOWS, decimal TERCWS, string CDIVWS,
                                        string PRFDWS, string NFAAWS, string NFARWS, string FIVAWS, string USA1WS, string USA2WS, string USA3WS,
                                        string USA4WS, string USA5WS, string USA6WS, string USA7WS, string USA8WS, decimal USN1WS, decimal USN2WS,
                                        string USF1WS, string USF2WS)
        {
            string result = "";
            try
            {
                string MONTWS_Cad = MONTWS.ToString().Replace(",", ".");
                string MOSMWS_Cad = MOSMWS.ToString().Replace(",", ".");
                string TERCWS_Cad = TERCWS.ToString().Replace(",", ".");
                string USN1WS_Cad = USN1WS.ToString().Replace(",", ".");
                string USN2WS_Cad = USN2WS.ToString().Replace(",", ".");

                //Actualizar el detalle del comprobante
                string query = "update " + nombreTablaDet + " set TTRAWS = '" + TTRAWS + "', CCIAWS  = '" + CCIAWS;
                if (tipoBaseDatosCG == "DB2")
                    query += "', AÑOCWS = " + ANOCWS + ", LAPSWS = " + LAPSWS + ", TICOWS = " + TICOWS + ", NUCOWS = " + NUCOWS + ", CUENWS = '" + CUENWS;
                else
                    query += "', AVOCWS = " + ANOCWS + ", LAPSWS = " + LAPSWS + ", TICOWS = " + TICOWS + ", NUCOWS = " + NUCOWS + ", CUENWS = '" + CUENWS;
                query += "', CAUXWS = '" + CAUXWS + "', DESCWS = '" + DESCWS + "', MONTWS = " + MONTWS_Cad + ", TMOVWS = '" + TMOVWS;
                query += "', MOSMWS = " + MOSMWS_Cad + ", CLDOWS = '" + CLDOWS + "', NDOCWS = " + NDOCWS + ", FDOCWS = " + FDOCWS;
                query += ", FEVEWS = " + FEVEWS + ", TEINWS  = '" + TEINWS + "', NNITWS  = '" + NNITWS + "', AUA1WS  = '" + AUA1WS;
                query += "', AUA2WS = '" + AUA2WS + "', CDDOWS = '" + CDDOWS + "', NDDOWS  = " + NDDOWS + ", TERCWS = " + TERCWS_Cad;
                query += ", CDIVWS = '" + CDIVWS;
                query += "', PRFDWS = '" + PRFDWS + "', NFAAWS = '" + NFAAWS + "', NFARWS  = '" + NFARWS + "', FIVAWS  = " + FIVAWS;
                query += ", USA1WS = '" + USA1WS + "', USA2WS = '" + USA2WS + "', USA3WS = '" + USA3WS + "', USA4WS = '" + USA4WS;
                query += "', USA5WS = '" + USA5WS + "', USA6WS = '" + USA6WS + "', USA7WS = '" + USA7WS + "', USA8WS = '" + USA8WS;
                query += "', USN1WS = " + USN1WS_Cad + ", USN2WS = " + USN2WS_Cad + ", USF1WS = " + USF1WS + ", USF2WS = " + USF2WS;
                query += " Where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + nombreTablaDet + ") = " + rowNumber;
                        break;
                    case "SQLServer":
                        query += "GERIDENTI = " + rowNumber;
                        break;
                    case "Oracle":
                        //id_prefijotabla_prefijolote + W01   o W11
                        string campoOracle = "ID_" + nombreTablaDet;
                        query += campoOracle + " = " + rowNumber;
                        break;
                }

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errInsertDetComp", "Error actualizando el detalle del comprobante") + " (CCIA: " + CCIAWS + " AAPP: " + utiles.AAPPConFormato(ANOCWS + LAPSWS) + " Tipo: " + TICOWS;
                result += " NUCO: " + NUCOWS + ") (" + ex.Message + ")";
            }

            return (result);
        }   

        /// <summary>
        /// Actualiza el listado de comprobantes del formulario frmCompContLista
        /// </summary>
        private void ActualizarFormularioListaComprobantes()
        {
            if (Application.OpenForms["frmCompContLista"] != null)
            {
                if (this.Owner is IForm formInterface)
                    formInterface.ActualizaListaComprobantes();
            }
        }

        /// <summary>
        /// Habilitar / Deshabilitar los controles del formulario
        /// </summary>
        /// <param name="valor"></param>
        private void ControlesHabilitarDeshabilitar(bool valor)
        {
            this.txtMaskAAPP.Enabled = valor;
            this.dateTimePickerFecha.Enabled = valor;
            this.cmbTipo.Enabled = valor;
            this.txtNoComprobante.Enabled = valor;
            this.cmbClase.Enabled = valor;
            this.txtTasa.Enabled = valor;
            this.txtDescripcion.Enabled = valor;
            this.dgDetalle.Enabled = valor;
            this.radDropDownListRevertir.Enabled = valor;

            utiles.ButtonEnabled(ref this.radButtonGrabar, valor);
            utiles.ButtonEnabled(ref this.radButtonGrabarComo, valor);
            utiles.ButtonEnabled(ref this.radButtonRevertir, valor);
            utiles.ButtonEnabled(ref this.radButtonValidar, valor);

            if (valor == false && this.dgErrores.Visible)
            {
                this.dgErrores.Visible = false;
                this.dgDetalle.Height = this.dgDetalle.Height + this.dgErrores.Height;
                utiles.ButtonEnabled(ref this.radButtonValidarErrores, valor);
            }

            this.menuGridButtonBuscar.Enabled = valor;
            this.menuGridButtonAdicionarFila.Enabled = valor;
            this.menuGridButtonBorrar.Enabled = valor;
            this.menuGridButtonCopiar.Enabled = valor;
            this.menuGridButtonCortar.Enabled = valor;
            this.menuGridButtonInsertarFila.Enabled = valor;
            this.menuGridButtonPegar.Enabled = valor;
            this.menuGridButtonSuprimirFila.Enabled = valor;
            this.menuGridButtonReemplazar.Enabled = valor;

            if (!valor) this.btnSel.Visible = false;
        }

        /// <summary>
        /// Mostrar el botón de Selección en las coordenadas que le corresponde
        /// </summary>
        /// <param name="tgGridDetalles">Grid de detalles</param>
        private void BtnSelPosicion(TGGrid tgGridDetalles)
        {
            this.btnSel.Visible = false;
            //Application.DoEvents();

            DataGridViewCell cellPrimera = tgGridDetalles.FirstDisplayedCell;
            int widthOutScroll = tgGridDetalles.FirstDisplayedScrollingColumnHiddenWidth;
            int firstColumn = tgGridDetalles.FirstDisplayedScrollingColumnIndex;
            int firstRow = tgGridDetalles.FirstDisplayedScrollingRowIndex;

            DataGridViewCell celdaActiva = tgGridDetalles.CurrentCell;

            if (celdaActiva != null)
            {
                //Calcular número de Fila a posicionar el botón
                this.btnSel.Top = this.dgDetalle.Top + tgGridDetalles.Rows[0].Height;
                long posLong = this.btnSel.Top;

                for (int i = 1; i < tgGridDetalles.Rows.Count; i++)
                {
                    if (celdaActiva.RowIndex == 0) break;
                    else
                    {
                        posLong = posLong + tgGridDetalles.Rows[i].Height;
                        if (i == celdaActiva.RowIndex) break;
                    }
                }

                if (firstRow != 0)
                {
                    for (int i = 0; i < firstRow; i++)
                    {
                        posLong = posLong - tgGridDetalles.Rows[i].Height;
                    }
                }

                this.btnSel.Top = Convert.ToInt32(posLong) + 2;

                //------ SCROLL HORIZONTAL ------

                //Falta tener en cuenta la 1ra columna
                //Calcular número de Columna a posicionar el botón
                this.btnSel.Left = tgGridDetalles.Left + tgGridDetalles.Columns[0].Width - widthOutScroll;
                for (int i = 1; i < tgGridDetalles.Columns.Count; i++)
                {
                    if (celdaActiva.ColumnIndex == 0) 
                    {
                        this.btnSel.Left = this.btnSel.Left + 15;
                        break;
                    }
                    else
                    {
                        if (tgGridDetalles.Columns[i].Visible == true) this.btnSel.Left = this.btnSel.Left + tgGridDetalles.Columns[i].Width;
                        if (i == celdaActiva.ColumnIndex)
                        {
                            this.btnSel.Left = this.btnSel.Left + 15;
                            break;
                        }
                    }
                }

                if (firstColumn != 0)
                {
                    for (int i = 0; i < firstColumn; i++)
                    {
                        this.btnSel.Left = this.btnSel.Left - tgGridDetalles.Columns[i].Width;
                    }
                }

                //this.btnSel.Left = this.btnSel.Left + 5
                this.btnSel.Left = this.btnSel.Left + (tgGridDetalles.RowHeadersWidth - 35);

                this.btnSel.Visible = true;

                if (firstColumn > celdaActiva.ColumnIndex) this.btnSel.Visible = false;
                else
                {
                    //Analizar el scroll por la derecha
                    int widthVisible = 0;
                    if (firstColumn != -1)
                    {
                        for (int i = firstColumn; i < celdaActiva.ColumnIndex; i++)
                        {
                            widthVisible = widthVisible + this.dgDetalle.Columns[i].Width;
                        }
                    }

                    if (widthVisible > this.dgDetalle.Width) this.btnSel.Visible = false;
                }

                //------ SCROLL VERTICAL ------
                if (firstRow > celdaActiva.RowIndex) this.btnSel.Visible = false;
                else
                {
                    //Analizar el scroll por abajo
                    int altoVisible = 0;
                    for (int i = firstRow; i < celdaActiva.RowIndex; i++)
                    {
                        altoVisible = altoVisible + this.dgDetalle.Rows[i].Height;
                    }
                    //int altoHeader = this.dgDetalle.ColumnHeadersHeight;
                    if (altoVisible > this.dgDetalle.Height) this.btnSel.Visible = false;
                }
            }
        }

        private string QueryGLM01(string codigo)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (nglm01) return (result);

                //Comprobar que la compañía es válida
                string query = "select NCIAMG, TITAMG, FELAMG, TIPLMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where STATMG = 'V' and CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    //Inicializar los valores que se necesitan en función de la compañía
                    this.GLM01_NCIAMG = dr["NCIAMG"].ToString().Trim();
                    this.GLM01_TITAMG = dr["TITAMG"].ToString().Trim();
                    this.GLM01_FELAMG = dr["FELAMG"].ToString().Trim();
                    this.GLM01_TIPLMG = dr["TIPLMG"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }
    
                /// <summary>
                /// Valida la existencia o no de la compañía. Si la encuentra recupera los valores NCIAMG, TITAMG, FELAMG, TIPLMG
                /// </summary>
                /// <param name="codigo"></param>
                /// <returns>Valida la existencia o no de la compañía. Si la encuentra recupera los valores NCIAMG, TITAMG, FELAMG, TIPLMG</returns>
        private string ValidarCompania(string codigo)
        {
            string result = "";
            IDataReader dr = null;
           
            try
            {
                if ((!Batch && nglm01) || (Batch && nglm01 && codigo== compania_cmb) || compania_ant is null) return ("");

                if (codigo != compania_cmb) compania_cmb = codigo;

                //Comprobar que la compañía es válida
                string query = "select NCIAMG, TITAMG, FELAMG, TIPLMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where STATMG = 'V' and CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue); 

                if (dr.Read())
                {
                    //Inicializar los valores que se necesitan en función de la compañía
                    this.GLM01_NCIAMG = dr["NCIAMG"].ToString().Trim();
                    this.GLM01_TITAMG = dr["TITAMG"].ToString().Trim();
                    this.GLM01_FELAMG = dr["FELAMG"].ToString().Trim();
                    this.GLM01_TIPLMG = dr["TIPLMG"].ToString().Trim();
                    
                    //Verificar si tiene campos extendidos (sólo para el caso de los xmls, no para la gestión de lotes que ya viene informado)
                    if (!nGlmx2)
                    {
                        if (!(this.edicionLote || this.edicionLoteError))
                            this.extendido = this.CamposExtendidos();
                        else this.extendido = false;
                    }

                    //Actualizar las columnas de campos extendidos de la Grid de detalles
                    this.ActualizarDetallesCamposExtendidos();

                    result = "";
                }
                else
                {
                    //Error la compañía no es válida
                    result = this.LP.GetText("lblfrmCompContErrComp", "La compañía no es válida");
                }
                nglm01 = true;
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCompExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que el año/período esté definido para la compañía
        /// </summary>
        /// <returns></returns>
        private string ValidarAAPP()
        {
            string result = "";

            //coger el valor sin la máscara
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string sap = this.txtMaskAAPP.Value.ToString();
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

            IDataReader dr = null;

            //Validar que el año/período esté definido para la compañía
            try
            {
                if (sap.Length == 0)
                {
                    result = this.LP.GetText("lblfrmCompContErrAAPPVacio", "Debe introducir el Año-Período");
                }
                else
                if (this.GLM01_TITAMG != null && this.GLM01_TITAMG != "")
                {
                    //Si la compañía es válida y está definido el TITAMG
                    //Buscar el siglo dado el año
                    string aa = sap.Substring(0, 2);
                    sap = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + sap;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where SAPRFL =" + sap + " and TITAFL = '" + this.GLM01_TITAMG + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue); 

                    if (!dr.Read())
                    {
                        result = this.LP.GetText("lblfrmCompContErrAAPP", "El Año-Período no es válido para la compañía seleccionada");
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrAAPPExcep", "Error al validar el Año-Período") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que el tipo sea correcto
        /// </summary>
        /// <returns></returns>
        private string ValidarTipo(string codigo)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select NOMBTV from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where STATTV = 'V' and TIVOTV = '" + codigo + "'";

                if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) query += " and CODITV='0' ";
                else query += " and CODITV='1' ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.GLT06_NOMBTV = dr["NOMBTV"].ToString().Trim();
                }
                else
                {
                    result = this.LP.GetText("lblfrmCompContErrTipo", "El tipo no es válido");
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrTipoExcep", "Error al validar el Tipo") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }
        private string ValidarTipoCmp(string codigo)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select NOMBTV from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where STATTV = 'V' and TIVOTV = '" + codigo + "'";

                if (!Batch && this.nuevoComprobanteGLB01) query += " and CODITV='0' ";
                else
                {
                    if (Batch) query += " and CODITV='1' ";
                }

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (!dr.Read())
                
                {
                    result = this.LP.GetText("lblfrmCompContErrTipo", "- El tipo no es válido");
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrTipoExcep", "Error al validar el Tipo") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar que la fecha sea correcta
        /// </summary>
        /// <returns></returns>
        private string ValidarFecha()
        {
            string result = "";
            IDataReader dr = null;

            //coger el valor sin la máscara
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string sap = this.txtMaskAAPP.Value.ToString();
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

            //Para esta validacion importa el formato de la fecha ??? o de la misma forma que se entra es en la que se almacena ???
            try
            {
                //Si hay SAAP definido y la compañía es válida y está definido el TITAMG
                if (sap.Length != 0 && this.GLM01_TITAMG != null && this.GLM01_TITAMG != "")
                {
                    //Coger el campo año período con siglo 
                    string aaCampoAP = sap.Substring(0, 2);
                    sap = utiles.SigloDadoAnno(aaCampoAP, CGParametrosGrles.GLC01_ALSIRC) + sap;

                    //Coger el dateTimePickerFecha y convertirlo a syymmdd para la select
                    int year = this.dateTimePickerFecha.Value.Year;
                    string aa = year.ToString();
                    if (aa.Length > 2) aa = aa.Substring(aa.Length - 2, 2);

                    string mes = this.dateTimePickerFecha.Value.Month.ToString();
                    mes = mes.PadLeft(2, '0');

                    string dia = this.dateTimePickerFecha.Value.Day.ToString();
                    dia = dia.PadLeft(2, '0');

                    string samd = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa + mes + dia;

                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + this.GLM01_TITAMG + "' and SAPRFL = " + sap + " and ";
                    query += "INLAFL <= " + samd + " and ";
                    query += "FINLFL >= " + samd;

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (!dr.Read())
                    {
                        result = this.LP.GetText("lblfrmCompContErrFecha", "La Fecha no está dentro del período");
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrFechaExcep", "Error al validar la Fecha") + " (" + ex.Message + ")";
                if (dr != null) dr.Close();
            }

            return (result);
        }

        /// <summary>
        /// Validar cuenta de mayor
        /// </summary>
        /// <returns></returns>
        private string ValidarCuentaMayor(string codigo)
        {
            string result = "";
            try
            {
                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where CUENMC = '" + codigo + "' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC = 'V'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        result = this.LP.GetText("lblfrmCompContErrCtaMayor", "La cuenta de mayor no es válida");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCtaMayorExcep", "Error al validar la cuenta de mayor") + " (" + ex.Message + ")";
            }

            return (result);
        }


        /// <summary>
        /// Validar cuenta de auxiliar
        /// </summary>
        /// <param name="codigo">Cuenta de auxiliar</param>
        /// <param name="cuentaMayor">Cuenta de Mayor</param>
        /// <param name="ctaAuxiliar">Indica cual es la cuenta de auxiliar (1, 2 ó 3)</param>
        /// <returns></returns>
        private string ValidarCuentaAuxiliar(string codigo, string cuentaMayor, int ctaAuxiliar)
        {
            string result = "";
            codigo = codigo.ToUpper();
            try
            {
                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string queryTAUXMA = "";
                    switch (ctaAuxiliar)
                    {
                        case 1:
                            queryTAUXMA = "TAUXMA = TAU1MC";
                            break;
                        case 2:
                            queryTAUXMA = "TAUXMA = TAU2MC";
                            break;
                        case 3:
                            queryTAUXMA = "TAUXMA = TAU3MC";
                            break;
                    }

                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05, ";
                    query += GlobalVar.PrefijoTablaCG + "GLM03 " ;
                    query += "where CAUXMA = '" + codigo + "' and " + queryTAUXMA + " and " ;
                    query += "TIPLMC = '" + this.GLM01_TIPLMG + "' and CUENMC = '" + cuentaMayor + "' and ";
                    query += "STATMC = 'V'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        result = this.LP.GetText("lblfrmCompContErrCtaAuxiliar", "La cuenta de auxiliar no es válida");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrCtaAuxiliarExcep", "Error al validar la cuenta de auxiliar") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Validar IVA
        /// </summary>
        /// <returns></returns>
        private string ValidarIVA(string codigo)
        {
            string result = "";
            try
            {
                if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "IVT01 ";
                    query += "where TIPLCI = '" + this.GLM01_TIPLMG + "' and STATCI = 'V' and COIVCI = '" + codigo.ToUpper() + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (!(cantRegistros > 0))
                    {
                        result = this.LP.GetText("lblfrmCompContErrIVA", "El código de IVA no es válido");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmCompContErrIVAExcep", "Error al validar el código de IVA") + " (" + ex.Message + ")";
            }

            return (result);
        }

        
        /// <summary>
        /// Valida si es posible grabar el formulario
        /// </summary>
        /// <returns></returns>
        private string ValidarGrabarFormulario()
        {
            string result = "";
            string gravedad = "0";
            string codigo = "";

            if (Batch && !BatchLote)
            {
                codigo = compania;
            }
            else
            {
                codigo = this.cmbCompania.SelectedValue.ToString();
            }
            
            string validarCompania = this.ValidarCompania(codigo);
            if (validarCompania != "")
            {
                validarCompania = "  - " + validarCompania + "\n\r";
                gravedad = "1";
            }

            string validarAAPP = this.ValidarAAPP();
            if (validarAAPP != "") validarAAPP = "  - " + validarAAPP + "\n\r";

            if (this.cmbTipo.SelectedValue != null)
            {
                codigo = this.cmbTipo.SelectedValue.ToString();
            }
            else
            {
                codigo = "";
            }
            string validarTipo = this.ValidarTipoCmp(codigo);
            if (validarTipo != "")
            {
                validarTipo = "  - " + validarTipo + "\n\r"; 
                gravedad = "1";
            }

            string validarFecha = this.ValidarFecha();
            if (validarFecha != "")
            {
                validarFecha = "  - " + validarFecha + "\n\r";
                gravedad = "1";
            }

            bool documentoCuadra = this.ValidarComprobanteCuadra();
            string validarCuadre = "";
            if (!documentoCuadra) validarCuadre = "  - " + this.LP.GetText("lblfrmCompContErrSinCuadrar", "Moneda Local Comprobante sin cuadrar") + "\n\r";

            //Validar Importe
            bool documentoSinImporte = this.ValidarComprobanteSinImporte();
            string validarSinImporte = "";
            if (!documentoSinImporte)
            { 
                validarSinImporte = "  - " + this.LP.GetText("lblfrmCompContErrSinImporte", "Comprobante sin Importes") + "\n\r";
                gravedad = "1";
            }

            result = gravedad + validarCompania + validarAAPP + validarTipo + validarFecha + validarCuadre + validarSinImporte;

            return (result);
        }

        /// <summary>
        /// Valida si el comprobante cuadra en moneda local
        /// </summary>
        /// <returns></returns>
        private bool ValidarComprobanteCuadra_ant()
        {
            bool result = true;

            //Documento Cuadrado
            //Total debe y total haber (sumando las 3 lineas) tiene q dar 0

            decimal monedaLocalDebe = 0;
            decimal monedaExtDebe = 0;
            decimal importe3Debe = 0;
            decimal monedaLocalHaber = 0;
            decimal monedaExtHaber = 0;
            decimal importe3Haber = 0;

            try
            {
                string monedaLocalDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString();
                string importe3DebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Debe"].ToString();
                string monedaLocalHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString();
                string monedaExtHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtHaber"].ToString();
                string monedaExtDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtDebe"].ToString();
                string importe3HaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Haber"].ToString();

                if (monedaLocalDebeStr != "") monedaLocalDebe = Convert.ToDecimal(monedaLocalDebeStr);
                if (monedaExtDebeStr != "") monedaExtDebe = Convert.ToDecimal(monedaExtDebeStr);
                if (importe3DebeStr != "") importe3Debe = Convert.ToDecimal(importe3DebeStr);
                if (monedaLocalHaberStr != "")
                {
                    monedaLocalHaber = Convert.ToDecimal(monedaLocalHaberStr);
                    //monedaLocalHaber = monedaLocalHaber * -1;
                }
                if (monedaExtHaberStr != "")
                {
                    monedaExtHaber = Convert.ToDecimal(monedaExtHaberStr);
                    //monedaExtHaber = monedaExtHaber * -1;
                }
                if (importe3HaberStr != "")
                {
                    importe3Haber = Convert.ToDecimal(importe3HaberStr);
                    //importe3Haber = importe3Haber * -1;
                }

                //decimal total = monedaLocalDebe + monedaExtDebe + importe3Debe + monedaLocalHaber + monedaExtHaber + importe3Haber;
                decimal total = monedaLocalDebe + monedaExtDebe + monedaLocalHaber + monedaExtHaber;

                if (total != 0) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = false;
            }

            return (result);
        }
        private bool ValidarComprobanteCuadra()
        {
            // cuadre grid ,movimientos de un comprobante

            bool result = true;
            try
            {
                bool documentocuadra = ValidarComprobanteCuadre_moneda_local();
                string validarCuadre = "";
                if (!documentocuadra)
                {
                    validarCuadre = "  - " + this.LP.GetText("lblfrmCompContErrMLSinCuadrar", "Moneda Local Comprobante sin cuadrar") + "\n\r";
                    result = false;
                }
                                
                string clase = this.cmbClase.Text;
                if (clase.Substring(0, 1) != "0")
                {
                    documentocuadra = ValidarComprobanteCuadre_moneda_extranjera();
                    if (!documentocuadra)
                    {
                        validarCuadre += "  - " + this.LP.GetText("lblfrmCompContEXErrSinCuadrar", "Moneda Extranjera Comprobante sin cuadrar") + "\n\r";
                        result = false;
                    }
                }
                
                if (!documentocuadra)
                {
                    validarCuadre += "  - " + this.LP.GetText("lblfrmCompContErrnoCuadra", "ESte Comprobante no está cuadrado") + "\n\r";
                    RadMessageBox.Show(validarCuadre, this.LP.GetText("errValTitulo", "Error")); result = false;
                }
            }
                        catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = false;
            }
            
            return (result);
        }


        /// <summary>
        /// Valida si el comprobante cuadra en moneda local
        /// </summary>
        /// <returns></returns>
        private bool ValidarComprobanteCuadre_moneda_local()
        {
            bool result = true;

            //Documento Cuadrado
            //Total debe y total haber (sumando las 3 lineas) tiene q dar 0

            decimal monedaLocalDebe = 0;
            
            decimal monedaLocalHaber = 0;

            try
            {
                string monedaLocalDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString();
                string monedaLocalHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString();
                
                if (monedaLocalDebeStr != "") monedaLocalDebe = Convert.ToDecimal(monedaLocalDebeStr);
                
                
                if (monedaLocalHaberStr != "")
                {
                    monedaLocalHaber = Convert.ToDecimal(monedaLocalHaberStr);
                    //monedaLocalHaber = monedaLocalHaber * -1;
                }
                
                
                //decimal total = monedaLocalDebe + monedaExtDebe + importe3Debe + monedaLocalHaber + monedaExtHaber + importe3Haber;
                decimal total = monedaLocalDebe +  monedaLocalHaber;

                if (total != 0) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = true;
            }

            return (result);
        }
        private bool ValidarComprobanteSinImporte()
        {
            bool result = true;

            decimal monedaLocalDebe = 0;

            decimal monedaLocalHaber = 0;

            try
            {
                string monedaLocalDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"].ToString();
                string monedaLocalHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"].ToString();

                if (monedaLocalDebeStr != "") monedaLocalDebe = Convert.ToDecimal(monedaLocalDebeStr);


                if (monedaLocalHaberStr != "")
                {
                    monedaLocalHaber = Convert.ToDecimal(monedaLocalHaberStr);
                    
                }

                decimal monedaExtDebe = 0;

                decimal monedaExtHaber = 0;

                string monedaExtHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtHaber"].ToString();
                string monedaExtDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtDebe"].ToString();

                if (monedaExtDebeStr != "") monedaExtDebe = Convert.ToDecimal(monedaExtDebeStr);


                if (monedaExtHaberStr != "")
                {
                    monedaExtHaber = Convert.ToDecimal(monedaExtHaberStr);
                }

                /* if (monedaLocalDebe != 0 || monedaLocalHaber !=0 ||
                    monedaExtDebe != 0 || monedaExtHaber != 0
                    ) result = false; */
                if (monedaLocalDebe == 0 && monedaLocalHaber  == 0 &&
                    monedaExtDebe == 0 && monedaExtHaber == 0
                    ) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }
        /// <summary>
        /// Valida si el comprobante cuadra en moneda extranjera
        /// </summary>
        /// <returns></returns>
        private bool ValidarComprobanteCuadre_moneda_extranjera()
        {
            bool result = true;

            //Documento Cuadrado
            //Total debe y total haber (sumando las 3 lineas) tiene q dar 0

            decimal monedaExtDebe = 0;

            decimal monedaExtHaber = 0;

            try
            {
                string monedaExtHaberStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtHaber"].ToString();
                string monedaExtDebeStr = this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtDebe"].ToString();

                if (monedaExtDebeStr != "") monedaExtDebe = Convert.ToDecimal(monedaExtDebeStr);


                if (monedaExtHaberStr != "")
                {
                    monedaExtHaber = Convert.ToDecimal(monedaExtHaberStr);
                    //monedaLocalHaber = monedaLocalHaber * -1;
                }


                //decimal total = monedaLocalDebe + monedaExtDebe + importe3Debe + monedaLocalHaber + monedaExtHaber + importe3Haber;
                decimal total = monedaExtDebe + monedaExtHaber;

                if (total != 0) result = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = false;
            }

            return (result);
        }

        /// <summary>
        /// Devuelve la query para el objeto TGElementosSel que se dibujará en la celda correspondiente 
        /// (dependiendo de la celda donde se hace el click)
        /// </summary>
        /// <param name="columna">Columna activa del Grid</param>
        /// <param name="fila">Fila activa del Grid</param>
        /// <param name="titulo">Devuelve el título para el formulario de selección</param>
        /// <param name="error">Devuelve si hubo error</param>
        /// <returns></returns>
        private string QuerySelElementos(int columna, int fila, ref string titulo, ref string error, string columnName)
        {
            string query = "";
            string queryTodo = "";

            IDataReader dr = null;

            string vCia = this.cmbCompania.SelectedValue.ToString();
            
            string cuentaMayor = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[fila][0].ToString();

            try
            {
                switch (columnName)
                {
                    case "Cuenta":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaMayor", "Seleccionar Cuenta de Mayor");

                            query = "select min(CUENMC) CUENMC, max(NOLAAD) NOLAAD, CEDTMC from ";
                            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
                            query += "where TCUEMC = 'D' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC != '*' ";
                            query += "group by CEDTMC order by CUENMC";
                            error = "";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoPlanCompania", "No está definido el tipo de plan para la compañía seleccionada");
                        }
                        break;
                    case "Auxiliar1":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux1", "Seleccionar Cuenta Auxiliar1");

                            queryTodo = "select CAUXMA, NOMBMA from ";
                            queryTodo += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            queryTodo += "where TAUXMA = TAU1MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            queryTodo += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            queryTodo += "order by NOMBMA";
                            error = "";

                            //buscar en GLT21, si existe la restriccion y 1er Aux en blanco mostrar todos.
                            query = "select count(*) from ";
                            query += GlobalVar.PrefijoTablaCG + "GLT21 ";
                            query += " where TIPL21 = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CULT21 ='"+ CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                            query += "(CCIA21 = '*' or CCIA21 = '" + vCia + "') and AUX121 = ''";
                            int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));
                            
                            if (cantRegistros == 0)
                            {
                                //buscar restriccion de 1er auxiliar en GLT21
                                query = "select A.CAUXMA, A.NOMBMA from ";
                                query += GlobalVar.PrefijoTablaCG + "GLM05 A JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLM03 B ON A.TAUXMA = B.TAU1MC JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLT21 C ON C.TIPL21 = B.TIPLMC and ";
                                query += "C.CULT21 = B.CUENMC and C.AUX121 = A.CAUXMA where ";
                                query += "B.CUENMC='" + CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                                query += "B.TIPLMC='" + this.GLM01_TIPLMG + "' and A.STATMA = 'V' and ";
                                query += "(C.CCIA21 = '*' or C.CCIA21 = '" + vCia+ "') ";
                                query += "group by A.CAUXMA, A.NOMBMA order by A.NOMBMA";

                                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                                if (!dr.Read())
                                {
                                    //no hay restriccion, mostrar todos.
                                    query = queryTodo;
                                }
                            }
                            //no hay restriccion, mostrar todos.
                            else
                            {
                                query = queryTodo;
                            }
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux1", "No está definido la cuenta de auxiliar1 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "Auxiliar2":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux2", "Seleccionar Cuenta Auxiliar2");

                            queryTodo = "select CAUXMA, NOMBMA from ";
                            queryTodo += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            queryTodo += "where TAUXMA = TAU2MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            queryTodo += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            queryTodo += "order by NOMBMA";
                            error = "";

                            //buscar en GLT21, si existe la restriccion y 2do Aux en blanco mostrar todos.
                            query = "select count(*) from ";
                            query += GlobalVar.PrefijoTablaCG + "GLT21 ";
                            query += " where TIPL21 = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CULT21 ='" + CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                            query += "(CCIA21 = '*' or CCIA21 = '" + vCia + "') and AUX221 = ''";
                            int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                            if (cantRegistros == 0)
                            {
                                //buscar restriccion de 1er auxiliar en GLT21
                                query = "select A.CAUXMA, A.NOMBMA from ";
                                query += GlobalVar.PrefijoTablaCG + "GLM05 A JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLM03 B ON A.TAUXMA = B.TAU2MC JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLT21 C ON C.TIPL21 = B.TIPLMC and ";
                                query += "C.CULT21 = B.CUENMC and C.AUX221 = A.CAUXMA where ";
                                query += "B.CUENMC='" + CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                                query += "B.TIPLMC='" + this.GLM01_TIPLMG + "' and A.STATMA = 'V' and ";
                                query += "(C.CCIA21 = '*' or C.CCIA21 = '" + vCia + "') ";
                                query += "group by A.CAUXMA, A.NOMBMA order by A.NOMBMA";

                                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                                if (!dr.Read())
                                {
                                    //no hay restriccion, mostrar todos.
                                    query = queryTodo;
                                }
                            }
                            //no hay restriccion, mostrar todos.
                            else
                            {
                                query = queryTodo;
                            }
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux2", "No está definido la cuenta de auxiliar2 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "Auxiliar3":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaAux3", "Seleccionar Cuenta Auxiliar3");

                            queryTodo = "select CAUXMA, NOMBMA from ";
                            queryTodo += GlobalVar.PrefijoTablaCG + "GLM05, " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                            queryTodo += "where TAUXMA = TAU3MC and TIPLMC = '" + this.GLM01_TIPLMG + "' and ";
                            queryTodo += "CUENMC = '" + cuentaMayor + "' and STATMA != '*' ";
                            queryTodo += "order by NOMBMA";
                            error = "";

                            //buscar en GLT21, si existe la restriccion y 3er Aux en blanco mostrar todos.
                            query = "select count(*) from ";
                            query += GlobalVar.PrefijoTablaCG + "GLT21 ";
                            query += " where TIPL21 = '" + this.GLM01_TIPLMG + "' and ";
                            query += "CULT21 ='" + CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                            query += "(CCIA21 = '*' or CCIA21 = '" + vCia + "') and AUX321 = ''";
                            int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                            if (cantRegistros == 0)
                            {
                                //buscar restriccion de 1er auxiliar en GLT21
                                query = "select A.CAUXMA, A.NOMBMA from ";
                                query += GlobalVar.PrefijoTablaCG + "GLM05 A JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLM03 B ON A.TAUXMA = B.TAU3MC JOIN ";
                                query += GlobalVar.PrefijoTablaCG + "GLT21 C ON C.TIPL21 = B.TIPLMC and ";
                                query += "C.CULT21 = B.CUENMC and C.AUX321 = A.CAUXMA where ";
                                query += "B.CUENMC='" + CtaMayorUltimoNivel(this.GLM01_TIPLMG, cuentaMayor) + "' and ";
                                query += "B.TIPLMC='" + this.GLM01_TIPLMG + "' and A.STATMA = 'V' and ";
                                query += "(C.CCIA21 = '*' or C.CCIA21 = '" + vCia + "') ";
                                query += "group by A.CAUXMA, A.NOMBMA order by A.NOMBMA";

                                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                                if (!dr.Read())
                                {
                                    //no hay restriccion, mostrar todos.
                                    query = queryTodo;
                                }
                            }
                            //no hay restriccion, mostrar todos.
                            else
                            {
                                query = queryTodo;
                            }
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaAux3", "No está definido la cuenta de auxiliar3 para la cuenta de mayor seleccionada");
                        }
                        break;
                    case "Iva":
                        if (this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                        {
                            titulo = this.LP.GetText("lblSelCuentaIVA", "Seleccionar IVA");

                            query = "select COIVCI, NOMBCI from ";
                            query += GlobalVar.PrefijoTablaCG + "IVT01 ";
                            query += "where TIPLCI = '" + this.GLM01_TIPLMG + "' and STATCI != '*' ";
                            query += "order by COIVCI";
                        }
                        else
                        {
                            error = this.LP.GetText("errNoCtaIVA", "No está definido la cuenta de IVA para el de plan de la compañía seleccionada");
                        }
                        break;
                    case "CampoUserAlfa1":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA01PX");
                        break;
                    case "CampoUserAlfa2":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA02PX");
                        break;
                    case "CampoUserAlfa3":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA03PX");
                        break;
                    case "CampoUserAlfa4":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA04PX");
                        break;
                    case "CampoUserAlfa5":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA05PX");
                        break;
                    case "CampoUserAlfa6":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA06PX");
                        break;
                    case "CampoUserAlfa7":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA07PX");
                        break;
                    case "CampoUserAlfa8":
                        this.QuerySelElementosCampoUserAlfa(ref titulo, ref query, ref error, "TA08PX");
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }

        /// <summary>
        ///  Devuelve la query para el objeto TGElementosSel que se dibujará en la celda correspondiente para los campos extendidos CampoUserAlfa
        /// (dependiendo de la celda donde se hace el click)
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="query"></param>
        /// <param name="error"></param>
        /// <param name="campoTabla"></param>
        private void QuerySelElementosCampoUserAlfa(ref string titulo, ref string query, ref string error, string campoTabla)
        {
            if (this.dtGLMX2 != null && this.dtGLMX2.Rows.Count == 1)
            {
                string tipoAux = this.dtGLMX2.Rows[0][campoTabla].ToString();
                if (tipoAux != "")
                {
                    titulo = this.LP.GetText("lblSelCuenta", "Seleccionar Cuenta");

                    query = "select CAUXMA, NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + tipoAux + "' and STATMA != '*' ";
                    query += "order by NOMBMA";
                }
                else
                {
                    error = this.LP.GetText("errNoCta", "No está definido el auxiliar para la cuenta");
                }
            }
            else
            {
                error = this.LP.GetText("errNoCta", "No está definido el auxiliar para la cuenta");
            }
        }

        /// <summary>
        /// Dado una cuenta y una fila, procesa las columnas (activas o no y valores por defecto)
        /// </summary>
        /// <param name="cuentaMayor">Código de la cuenta de mayor</param>
        /// <param name="filaIndex">Índice de la fila</param>
        /// <returns></returns>
        private string UpdateEstadoColumnasDadoCuentaMayor(string cuentaMayor, int filaIndex)
        {
            string result = "";
            IDataReader dr = null;

            try
            {
                if (cuentaMayor != "" && this.GLM01_TIPLMG != null && this.GLM01_TIPLMG != "")
                {
                    string query = "select TAU1MC, TAU2MC, TAU3MC, TDOCMC, NDDOMC, FEVEMC, ADICMC, TERMMC, RNITMC ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TCUEMC = 'D' and CUENMC = '" + cuentaMayor + "' and TIPLMC = '" + this.GLM01_TIPLMG + "' and STATMC = 'V'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    string TAU1MC = "";
                    string TAU2MC = "";
                    string TAU3MC = "";
                    string TDOCMC = "";
                    string NDDOMC = "";
                    string FEVEMC = "";
                    string ADICMC = "";
                    string TERMMC = "";
                    string RNITMC = "";
                    if (dr.Read())
                    {
                        TAU1MC = dr["TAU1MC"].ToString().Trim();
                        TAU2MC = dr["TAU2MC"].ToString().Trim();
                        TAU3MC = dr["TAU3MC"].ToString().Trim();
                        TDOCMC = dr["TDOCMC"].ToString().Trim();
                        NDDOMC = dr["NDDOMC"].ToString().Trim();
                        FEVEMC = dr["FEVEMC"].ToString().Trim();
                        ADICMC = dr["ADICMC"].ToString().Trim();
                        TERMMC = dr["TERMMC"].ToString().Trim();
                        RNITMC = dr["RNITMC"].ToString().Trim();
                    }
                    dr.Close();

                    if (TAU1MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar1", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar1");

                    if (TAU2MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar2", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar2");

                    if (TAU3MC == "") this.dgDetalle.CellNotEnable(filaIndex, "Auxiliar3", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Auxiliar3");

                    if (TDOCMC == "N") this.dgDetalle.CellNotEnable(filaIndex, "Documento", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Documento");

                    if (TDOCMC == "N" && NDDOMC == "N") this.dgDetalle.CellNotEnable(filaIndex, "Fecha", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Fecha");

                    if (FEVEMC == "N") this.dgDetalle.CellNotEnable(filaIndex, "Vencimiento", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Vencimiento");

                    if (NDDOMC == "N") this.dgDetalle.CellNotEnable(filaIndex, "Documento2", true, "");
                    else
                    {
                        this.dgDetalle.CellEnable(filaIndex, "Documento2");

                        if (ADICMC != "N") this.dgDetalle.CellNotEnable(filaIndex, "Documento2", true, "* -000000000");
                        else this.dgDetalle.CellEnable(filaIndex, "Documento2");
                    }

                    if (TERMMC == "N") this.dgDetalle.CellNotEnable(filaIndex, "Importe3", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Importe3");

                    if (RNITMC != "R") this.dgDetalle.CellNotEnable(filaIndex, "Iva", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "Iva");

                    if (RNITMC == "N")
                    {
                        this.dgDetalle.CellNotEnable(filaIndex, "CifDNi", true, "");
                        if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) this.dgDetalle.CellNotEnable(filaIndex, "Pais", true, "");
                    }
                    else
                    {
                        this.dgDetalle.CellEnable(filaIndex, "CifDNi");
                        if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) this.dgDetalle.CellEnable(filaIndex, "Pais");
                    }

                    //Validar Columnas de campos extendidos - GLMX3
                    query = "select * ";
                    query += "from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX = '" + this.GLM01_TIPLMG + "' and CUENMX like '" + cuentaMayor + "%'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

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
                    if (dr.Read())
                    {
                        FGPRMX = dr["FGPRMX"].ToString().Trim();
                        FGFAMX = dr["FGFAMX"].ToString().Trim();
                        FGFRMX = dr["FGFRMX"].ToString().Trim();
                        FGDVMX = dr["FGDVMX"].ToString().Trim();
                        FG01MX = dr["FG01MX"].ToString().Trim();
                        FG02MX = dr["FG02MX"].ToString().Trim();
                        FG03MX = dr["FG03MX"].ToString().Trim();
                        FG04MX = dr["FG04MX"].ToString().Trim();
                        FG05MX = dr["FG05MX"].ToString().Trim();
                        FG06MX = dr["FG06MX"].ToString().Trim();
                        FG07MX = dr["FG07MX"].ToString().Trim();
                        FG08MX = dr["FG08MX"].ToString().Trim();
                        FG09MX = dr["FG09MX"].ToString().Trim();
                        FG10MX = dr["FG10MX"].ToString().Trim();
                        FG11MX = dr["FG11MX"].ToString().Trim();
                        FG12MX = dr["FG12MX"].ToString().Trim();
                    }
                    dr.Close();

                    if (FGPRMX == "0") this.dgDetalle.CellNotEnable(filaIndex, "PrefijoDoc", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "PrefijoDoc");

                    if (FGFAMX == "0") this.dgDetalle.CellNotEnable(filaIndex, "NumFactAmp", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "NumFactAmp");

                    if (FGFRMX == "0") this.dgDetalle.CellNotEnable(filaIndex, "NumFactRectif", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "NumFactRectif");

                    if (FGDVMX == "0") this.dgDetalle.CellNotEnable(filaIndex, "FechaServIVA", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "FechaServIVA");

                    if (FG01MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa1", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa1");

                    if (FG02MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa2", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa2");

                    if (FG03MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa3", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa3");

                    if (FG04MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa4", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa4");

                    if (FG05MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa5", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa5");

                    if (FG06MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa6", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa6");

                    if (FG07MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa7", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa7");

                    if (FG08MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserAlfa8", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserAlfa8");

                    if (FG09MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserNum1", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserNum1");

                    if (FG10MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserNum2", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserNum2");

                    if (FG11MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserFecha1", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserFecha1");

                    if (FG12MX == "0") this.dgDetalle.CellNotEnable(filaIndex, "CampoUserFecha2", true, "");
                    else this.dgDetalle.CellEnable(filaIndex, "CampoUserFecha2");
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = this.LP.GetText("lblfrmCompContErrCtaMayorExcep", "Error al procesar la cuenta de mayor") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Calcula todos los totales partiendo de la Tabla de Detalle
        /// </summary>
        private void CalcularTotales()
        {
            try
            {
                string monedaLocal = "";
                string monedaExtranjera = "";
                string importe3 = "";
                decimal monedaLocalDec = 0;
                decimal monedaExtranjeraDec = 0;
                decimal importe3Dec = 0;

                decimal monedaLocal_Debe = 0;
                decimal monedaLocal_Haber = 0;
                decimal monedaExtranjera_Debe = 0;
                decimal monedaExtranjera_Haber = 0;
                decimal importe3_Debe = 0;
                decimal importe3_Haber = 0;
                int noApuntes = 0;

                if (this.dgDetalle.dsDatos.Tables["Detalle"] != null && this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
                {

                    for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                    {
                        monedaLocal = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"].ToString().Trim();
                        monedaExtranjera = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"].ToString().Trim();
                        importe3 = this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"].ToString().Trim();

                        if (monedaLocal != "")
                            try { monedaLocalDec = Convert.ToDecimal(monedaLocal); }
                            catch(Exception ex) 
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                                monedaLocalDec = 0; 
                            }
                        else monedaLocalDec = 0;
                        if (monedaExtranjera != "")
                            try { monedaExtranjeraDec = Convert.ToDecimal(monedaExtranjera); }
                            catch (Exception ex)
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                                monedaExtranjeraDec = 0; 
                            }
                        else monedaExtranjeraDec = 0;
                        if (importe3 != "")
                            try { importe3Dec = Convert.ToDecimal(importe3); }
                            catch(Exception ex) 
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                                importe3Dec = 0; 
                            }
                        else importe3Dec = 0;

                        if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"] != null && this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"].ToString().Trim() != "")
                        {
                            switch (this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["DH"].ToString())
                            {
                                case "D":
                                    monedaLocal_Debe += monedaLocalDec;
                                    monedaExtranjera_Debe += monedaExtranjeraDec;
                                    importe3_Debe += importe3Dec;
                                    break;
                                case "H":
                                    monedaLocal_Haber += monedaLocalDec;
                                    monedaExtranjera_Haber += monedaExtranjeraDec;
                                    importe3_Haber += importe3Dec;
                                    break;
                            }
                        }
                        else
                        {
                            //No se especifica si el detalle va al Debe o al Haber. Se suma de acuerdo al signo del importe
                            if (monedaLocalDec > 0) monedaLocal_Debe += monedaLocalDec;
                            else if (monedaLocalDec < 0) monedaLocal_Haber += monedaLocalDec;
                            if (monedaExtranjeraDec > 0) monedaExtranjera_Debe += monedaExtranjeraDec;
                            else if (monedaExtranjeraDec < 0) monedaExtranjera_Haber += monedaExtranjeraDec;
                            if (importe3Dec > 0) importe3_Debe += importe3Dec;
                            else if (importe3Dec < 0) importe3_Haber += importe3Dec;
                        }

                        if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i)) noApuntes++;
                    }

                    //noApuntes = this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count - 1;
                }

                //Actualizar la tabla de Totales
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalDebe"] = monedaLocal_Debe.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaLocalHaber"] = monedaLocal_Haber.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtDebe"] = monedaExtranjera_Debe.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["MonedaExtHaber"] = monedaExtranjera_Haber.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Debe"] = importe3_Debe.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["Importe3Haber"] = importe3_Haber.ToString();
                this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["NumeroApuntes"] = noApuntes.ToString();

                //Actualizar las etiquetas que almacenan los valores de los totales
                //FALTA !!! Coger la cultura de 
                System.Globalization.CultureInfo ci;
                try
                {
                    ci = new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["idioma"]);
                }
                catch(Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    ci = new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["es-ES"]);
                }

                this.lblMonedaLocal_Debe.Text = monedaLocal_Debe.ToString("N2", ci);
                this.lblMonedaLocal_Haber.Text = monedaLocal_Haber.ToString("N2", ci);
                this.lblMonedaExtranjera_Debe.Text = monedaExtranjera_Debe.ToString("N2", ci);
                this.lblMonedaExtranjera_Haber.Text = monedaExtranjera_Haber.ToString("N2", ci);
                this.lblImporte3_Debe.Text = importe3_Debe.ToString("N2", ci);
                this.lblImporte3_Haber.Text = importe3_Haber.ToString("N2", ci);

                this.lblNoApuntes_Valor.Text = noApuntes.ToString();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Muestra el Grid con los Errores de la Validación
        /// </summary>
        private void ViewGridErrores()
        {
            try
            {
                this.dgErrores.DataSource = this.comp.DSErrores.Tables["Errores"];
                this.dgErrores.Columns["CodiTipo"].Visible = false;
                this.dgErrores.Columns["CtrlCelda"].Visible = false;

                //Ajustar las columnas
                this.dgErrores.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Linea"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Error"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                this.dgDetalle.Height = this.dgDetalle.Size.Height - this.dgErrores.Size.Height;

                //Columnas
                this.dgErrores.Columns["Tipo"].HeaderText = this.LP.GetText("lblGridErrorCabTipo", "Tipo");
                this.dgErrores.Columns["Linea"].HeaderText = this.LP.GetText("lblGridErrorCabLinea", "Línea");
                this.dgErrores.Columns["Error"].HeaderText = this.LP.GetText("lblGridErrorCabError", "Error");

                this.dgErrores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                this.dgErrores.Visible = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        #region Métodos privados
        /// <summary>
        /// Verifica si la compañía utiliza los campos extendidos
        /// </summary>
        /// <returns></returns>
        private bool CamposExtendidos()
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Verificar que exista la tabla GLMX2
                bool existeTabla = false;

                if (!nGlmx2)
                {
                    existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX2");
                }

                if (!existeTabla || nGlmx2) return (result);

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                query += "where TIPLPX = '" + this.GLM01_TIPLMG + "'";

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
                        nGlmx2 = true;
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
                    nGlmx2 = true;
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

        private string CtaMayorUltimoNivel(string plan, string cuenta)
        {
            string result = cuenta;

            int[] nivelLongitud = this.utilesCG.ObtenerNivelLongitudDadoPlan(plan);

            int longitudPlanCuentas = nivelLongitud[1];

            if (longitudPlanCuentas > cuenta.Length) result = cuenta.PadRight(longitudPlanCuentas, '0');

            return (result);
        }

        /// <summary>
        /// Actualiza las columnas de la grid de Detalles con la información de los campos extendidos para la compañía seleccionada
        /// </summary>
        private void ActualizarDetallesCamposExtendidos()
        {
            //Chequear si ya existen las columnas en el DataSet, sino crearlas
            if (this.dgDetalle.dsDatos != null && this.dgDetalle.dsDatos.Tables != null && this.dgDetalle.dsDatos.Tables.Count > 0 &&
                this.dgDetalle.dsDatos.Tables.Contains("Detalle") && this.dgDetalle.dsDatos.Tables["Detalle"].Rows != null && this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
            {
                try
                {
                    bool crear = false;
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("PrefijoDoc")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("PrefijoDoc", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("NumFactAmp")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("NumFactAmp", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("NumFactRectif")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("NumFactRectif", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("FechaServIVA")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("FechaServIVA", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa1")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa1", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa2")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa2", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa3")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa3", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa4")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa4", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa5")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa5", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa6")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa6", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa7")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa7", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa8")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserAlfa8", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserNum1")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserNum1", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserNum2")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserNum2", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserFecha1")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserFecha1", typeof(string)); }
                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserFecha2")) { crear = true; this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Add("CampoUserFecha2", typeof(string)); }

                    if (crear)
                    {
                        foreach (DataRow row in this.dgDetalle.dsDatos.Tables["Detalle"].Rows)
                        {
                            //Rellenar todas las filas de la columna con el valor vacío  
                            row["PrefijoDoc"] = "";
                            row["NumFactAmp"] = "";
                            row["NumFactRectif"] = "";
                            row["FechaServIVA"] = "";
                            row["CampoUserAlfa1"] = "";
                            row["CampoUserAlfa2"] = "";
                            row["CampoUserAlfa3"] = "";
                            row["CampoUserAlfa4"] = "";
                            row["CampoUserAlfa5"] = "";
                            row["CampoUserAlfa6"] = "";
                            row["CampoUserAlfa7"] = "";
                            row["CampoUserAlfa8"] = "";
                            row["CampoUserNum1"] = "";
                            row["CampoUserNum2"] = "";
                            row["CampoUserFecha1"] = "";
                            row["CampoUserFecha2"] = "";
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }

            if (!this.extendido)
            {
                //Blanquear las columnas de los campos extendidos
                for (int i = 0; i < this.dgDetalle.Rows.Count; i++)
                {
                    this.dgDetalle.Rows[i].Cells["PrefijoDoc"].Value = "";
                    this.dgDetalle.Rows[i].Cells["NumFactAmp"].Value = "";
                    this.dgDetalle.Rows[i].Cells["NumFactRectif"].Value = "";
                    this.dgDetalle.Rows[i].Cells["FechaServIVA"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa1"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa2"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa3"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa4"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa5"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa6"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa7"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserAlfa8"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserNum1"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserNum2"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserFecha1"].Value = "";
                    this.dgDetalle.Rows[i].Cells["CampoUserFecha2"].Value = "";
                }

                //Poner como no visibles todas las columnas de los campos extendidos
                this.dgDetalle.Columns["PrefijoDoc"].Visible = false;
                this.dgDetalle.Columns["NumFactAmp"].Visible = false;
                this.dgDetalle.Columns["NumFactRectif"].Visible = false;
                this.dgDetalle.Columns["FechaServIVA"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa1"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa2"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa3"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa4"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa5"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa6"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa7"].Visible = false;
                this.dgDetalle.Columns["CampoUserAlfa8"].Visible = false;
                this.dgDetalle.Columns["CampoUserNum1"].Visible = false;
                this.dgDetalle.Columns["CampoUserNum2"].Visible = false;
                this.dgDetalle.Columns["CampoUserFecha1"].Visible = false;
                this.dgDetalle.Columns["CampoUserFecha2"].Visible = false;

                return;
            }
            
            //Campos extendidos
            if (this.dtGLMX2 != null && this.dtGLMX2.Rows.Count == 1)
            {
                string nombreColumna = "";

                if (this.dtGLMX2.Rows[0]["FGPRPX"].ToString() == "0") this.dgDetalle.Columns["PrefijoDoc"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["PrefijoDoc"].Visible = true;
                    this.dgDetalle.Columns["PrefijoDoc"].Width = 100;
                }

                if (this.dtGLMX2.Rows[0]["FGFAPX"].ToString() == "0") this.dgDetalle.Columns["NumFactAmp"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["NumFactAmp"].Visible = true;
                    this.dgDetalle.Columns["NumFactAmp"].Width = 140;
                }

                if (this.dtGLMX2.Rows[0]["FGFRPX"].ToString() == "0") this.dgDetalle.Columns["NumFactRectif"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["NumFactRectif"].Visible = true;
                    this.dgDetalle.Columns["NumFactRectif"].Width = 140;
                }

                if (this.dtGLMX2.Rows[0]["FGDVPX"].ToString() == "0") this.dgDetalle.Columns["FechaServIVA"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["FechaServIVA"].Visible = true;
                    this.dgDetalle.Columns["FechaServIVA"].Width = 140;
                }

                if (this.dtGLMX2.Rows[0]["FG01PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa1"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa1"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa1"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM01PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa1", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG02PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa2"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa2"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa2"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM02PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa2", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG03PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa3"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa3"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa3"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM03PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa3", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG04PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa4"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa4"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa4"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM04PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa4", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG05PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa5"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa5"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa5"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM05PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa5", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG06PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa6"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa6"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa6"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM06PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa6", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG07PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa7"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa7"].Visible = true;
                    this.dgDetalle.Columns["CampoUserAlfa7"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM07PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa7", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG08PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserAlfa8"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserAlfa8"].Visible = true;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM08PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserAlfa8", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG09PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserNum1"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserNum1"].Visible = true;
                    this.dgDetalle.Columns["CampoUserNum1"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM09PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserNum1", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG10PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserNum2"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserNum2"].Visible = true;
                    this.dgDetalle.Columns["CampoUserNum2"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM10PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserNum2", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG11PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserFecha1"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserFecha1"].Visible = true;
                    this.dgDetalle.Columns["CampoUserFecha1"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM11PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserFecha1", nombreColumna);
                }

                if (this.dtGLMX2.Rows[0]["FG12PX"].ToString() == "0") this.dgDetalle.Columns["CampoUserFecha2"].Visible = false;
                else
                {
                    this.dgDetalle.Columns["CampoUserFecha2"].Visible = true;
                    this.dgDetalle.Columns["CampoUserFecha2"].Width = 140;
                    //Poner el título a la columna
                    nombreColumna = this.dtGLMX2.Rows[0]["NM12PX"].ToString();
                    if (nombreColumna != "") this.dgDetalle.CambiarColumnHeader("CampoUserFecha2", nombreColumna);
                }
            }

            //Mover el índice de las columnas para que la columna país aparezca al lado de la del DNI para los comprobantes nuevos/editados de la tabla GLB01
            if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01) this.MoverIndiceColumnas();
        }

        /// <summary>
        /// Chequear si la columna CampoUserAlfa que corresponda tiene informado tipo de auxiliar o no. 
        /// Si tiene se activa el botón para la selección de la cuenta
        /// </summary>
        /// <param name="nombreColumna">Nombre de la columna de la Grid</param>
        /// <param name="campoTabla">Nombre del campo de la tabla</param>
        /// <param name="calculaPosicionBtnSel">true-> se calcula las coordenados del botón false-> no se calcula</param>
        private void ActivaroNOBotonSelCamposExt(string nombreColumna, string campoTabla, bool calculaPosicionBtnSel)
        {
            try
            {
                string tipoAux = "";

                if (this.dtGLMX2 != null && this.dtGLMX2.Rows.Count == 1)
                {
                    tipoAux = this.dtGLMX2.Rows[0][campoTabla].ToString().Trim();
                    if (tipoAux == "") this.btnSel.Visible = false;
                    else
                    {
                        if (calculaPosicionBtnSel) this.BtnSelPosicion(this.dgDetalle);
                        this.btnSel.Visible = true;
                    }
                }
                else
                {
                    this.btnSel.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina las columnas que corresponden a campos extendidos de la tabla Detalles del comprobante
        /// </summary>
        private void EliminarColumnasCamposExtendidos()
        {
            try
            {
                if (this.dgDetalle.dsDatos != null && this.dgDetalle.dsDatos.Tables != null && this.dgDetalle.dsDatos.Tables.Count > 0 &&
                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count > 0)
                {
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("PrefijoDoc")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("PrefijoDoc");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("NumFactAmp")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("NumFactAmp");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("NumFactRectif")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("NumFactRectif");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("FechaServIVA")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("FechaServIVA");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa1")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa1");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa2")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa2");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa3")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa3");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa4")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa4");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa5")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa5");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa6")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa6");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa7")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa7");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserAlfa8")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserAlfa8");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserNum1")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserNum1");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserNum2")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserNum2");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserFecha1")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserFecha1");
                    if (this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Contains("CampoUserFecha2")) this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Remove("CampoUserFecha2");
                }
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Eliminar filas de Detalle (en la grid y en la tabla)
        /// </summary>
        private void SuprimirFilas()
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                bool suprimirRegistrosTabla = false;
                string rowNumber = "";

                //ArrayList rowNumbers = new ArrayList();
                List<string> rowNumbers = new List<string>();
                
                //foreach (DataGridViewRow row in this.SelectedRows)
                //    if (!row.IsNewRow) this.Rows.Remove(row);
                if (dgDetalle.IsCurrentCellInEditMode == false)
                {
                    if (this.edicionLote || this.edicionLoteError || this.edicionComprobanteGLB01)
                    {
                        if (this.dgDetalle.SelectedRows.Count == 1 && TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], 0))
                        {
                            //Sólo una fila y está en blanco
                            this.dgDetalle.ClearSelection();
                            this.btnSel.Visible = false;
                            return;
                        }

                        //Pedir confirmacion
                        string mensaje = this.LP.GetText("lblConfDelLineasDetalle", "Se van a eliminar las líneas de detalle. ¿Desea continuar?");
                        DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            this.dgDetalle.ClearSelection();
                            this.btnSel.Visible = false;
                            return;
                        }

                        suprimirRegistrosTabla = true;

                        foreach (DataGridViewRow row in this.dgDetalle.SelectedRows)
                            if (!row.IsNewRow)
                            {
                                rowNumber = row.Cells["RowNumber"].Value.ToString();
                                if (rowNumber != "") rowNumbers.Add(rowNumber);
                            }
                    }

                    this.dgDetalle.SuprimirFila();
                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                    this.dgDetalle.ClearSelection();
                    this.dgDetalle.Refresh();

                    //Recalcular Totales
                    this.gridChange = true;
                    this.CalcularTotales();

                    this.btnSel.Visible = false;

                    if (suprimirRegistrosTabla && rowNumbers != null && rowNumbers.Count > 0)
                    {
                        if (this.edicionLote || this.edicionLoteError) this.SuprimirRegistrosTabla(rowNumbers);
                        else
                        {
                            ComprobanteContCabeceraDetalle1FilaGLB01 comp = new ComprobanteContCabeceraDetalle1FilaGLB01
                            {
                                LPValor = this.LP
                            };
                            for (int i = 0; i < rowNumbers.Count; i++)
                            {
                                //Eliminar las líneas de detalles de las tablas GLB01, GLBX1 y actualizar el número de apuntes de la tabla GLI03
                                comp.SuprimirDetalleComprobanteTablaGLB01(rowNumbers[i].ToString());
                            }
                        }
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Eliminar registros de las tablas de detalle
        /// </summary>
        /// <param name="rowNumbers"></param>
        private void SuprimirRegistrosTabla(List<string> rowNumbers)
        {
            try
            {
                string prefijo = "";
                if (comprobanteContableImportar.Prefijo != null) prefijo = comprobanteContableImportar.Prefijo;

                string tablaDetalle = prefijo + "W01";
                if (this.extendido) tablaDetalle = prefijo + "W11";

                string bibliotecaTablasLoteAS = "";
                if (tipoBaseDatosCG == "DB2")
                {
                    if (comprobanteContableImportar.Biblioteca != null && comprobanteContableImportar.Biblioteca != "") bibliotecaTablasLoteAS = comprobanteContableImportar.Biblioteca + ".";
                    else bibliotecaTablasLoteAS = "";
                }

                string query = "delete from " + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + tablaDetalle;
                query += " where ";

                switch (this.tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "RRN(" + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + tablaDetalle + ") ";
                        break;
                    case "SQLServer":
                        query += "GERIDENTI ";
                        break;
                    case "Oracle":
                        //id_prefijotabla_prefijolote + W01  o W11
                        string campoOracle = "ID_" + GlobalVar.PrefijoTablaCG + tablaDetalle;
                        query += campoOracle + " ";
                        break;
                }

                if (rowNumbers.Count > 0)
                {
                    string rowNumbersSel = String.Join(",", rowNumbers.ToArray()[0]);
                    query += "in ('" + rowNumbersSel + "'";
                    for (int i = 1; i < rowNumbers.Count(); i++)
                    {
                        rowNumbersSel = String.Join(",", rowNumbers.ToArray()[i]);
                        query += ", '" + rowNumbersSel + "'";
                    }
                    query += ")";
                }

                //string rowNumbersSel = String.Join(",", rowNumbers.ToArray()[0]);
                //query += "in (" + rowNumbersSel + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Buscar dentro de la Grid de detalles del comprobantes
        /// </summary>
        private void Buscar()
        {
            TGGridBuscar tgBuscar = new TGGridBuscar
            {
                Opcion = TGGridBuscar.OpcionBuscarReemp.Buscar
            };
            if (!this.menuGridButtonReemplazar.Enabled) tgBuscar.OpcionReemplazarNO = true;
            tgBuscar.Grid = this.dgDetalle;
            tgBuscar.FormPadre = this;
            tgBuscar.BuscarTextoResult += new TGGridBuscar.BuscarTextoResultCommandEventHandler(TgBuscar_BuscarTextoResult);
            tgBuscar.ReemplazarTextoResult += new TGGridBuscar.ReemplazarTextoResultCommandEventHandler(TgBuscar_ReemplazarTextoResult);
            tgBuscar.ShowDialog(this);
        }

        private void TgBuscar_BuscarTextoResult(TGGridBuscar.BuscarTextoResultCommandEventArgs e)
        {
            this.dgDetalle.ClearSelection();

            int fila = e.Fila;
            int columna = e.Columna;
            
            this.dgDetalle.CurrentCell = this.dgDetalle[columna, fila];
            this.dgDetalle[columna, fila].Selected = true;


            /*if (this.dgDetalle.Columns[columna].GetType().Name == "DataGridViewTextBoxColumn")
            {
                DataGridViewCell cell = this.dgDetalle.Rows[fila].Cells[columna];
                if (!cell.IsInEditMode)
                {
                    this.dgDetalle.CurrentCell = cell;
                    this.dgDetalle.BeginEdit(false);
                }

                TextBox control = (TextBox)this.dgDetalle.EditingControl as TextBox;
                control.SelectionStart = 0;
                control.SelectionLength = 3;
            }
            */

            //this.dgDetalle[columna, fila].Style.BackColor = Color.Blue;

            
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Reemplazar dentro de la grid de detalles del comprobante
        /// </summary>
        private void Reemplazar()
        {
            TGGridBuscar tgBuscar = new TGGridBuscar
            {
                Opcion = TGGridBuscar.OpcionBuscarReemp.Reemplazar,
                Grid = this.dgDetalle,  
                FormPadre = this
            };
            if (!this.menuGridButtonReemplazar.Enabled) tgBuscar.OpcionReemplazarNO = true;
            tgBuscar.ReemplazarTextoResult +=new TGGridBuscar.ReemplazarTextoResultCommandEventHandler(TgBuscar_ReemplazarTextoResult);
            tgBuscar.BuscarTextoResult += new TGGridBuscar.BuscarTextoResultCommandEventHandler(TgBuscar_BuscarTextoResult);
            tgBuscar.ShowDialog(this);
        }

        private void TgBuscar_ReemplazarTextoResult(TGGridBuscar.ReemplazarTextoResultCommandEventArgs e)
        {
            this.dgDetalle.ClearSelection();

            int fila = e.Fila;
            int columna = e.Columna;

            this.dgDetalle.CurrentCell = this.dgDetalle[columna, fila];

            if (this.dgDetalle.CurrentCell.ReadOnly)
            {
                if (e.ReemplazarMostrarError) MessageBox.Show("Celda de sólo lectura", "Error");  //Falta traducir
                return;
            }

            if (e.ReemplazarCeldaCompleta) this.dgDetalle[columna, fila].Value = e.ReemplazarCadena;
            else
            {
                try
                {
                    int i = e.PosInicial;
                    string reemplazarCadena = e.ReemplazarCadena;
                    string buscarCadena = e.BuscarCadena;
                    if (i >= 0 && reemplazarCadena != "")
                    {
                        string celdaValor = this.dgDetalle[columna, fila].Value.ToString();
                        string celdaValorNuevo = celdaValor;
                        celdaValorNuevo = celdaValorNuevo.Remove(i, buscarCadena.Length).Insert(i, reemplazarCadena);
                        this.dgDetalle[columna, fila].Value = celdaValorNuevo;
                        this.gridChange = true;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }

            this.dgDetalle[columna, fila].Selected = true;
        }
 
        #region  Ver/Editar comprobante desde la opción edicionComprobanteGLB01  (Tabla GLB01)
       
        /// <summary>
        /// Actualiza los campos (fecha, descripcion) cuando el comprobante es de sólo consulta
        /// </summary>
        private void ActualizarComprobanteGLB01SoloConsulta()
        {
            IDataReader dr = null;
            try
            {
                //Falta Verificar Autorizaciones

                //Falta verificar si el periodo esta cerrado

                string aa = this.dateTimePickerFecha.Value.Year.ToString();

                if (aa.Length >= 2) aa = aa.Substring(aa.Length - 2, 2);
                //Coger el siglo
                aa = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + aa;

                string fecha = aa + this.dateTimePickerFecha.Value.Month.ToString().PadLeft(2, '0') +
                               this.dateTimePickerFecha.Value.Day.ToString().PadLeft(2, '0');

                //Verificar si se han producidos cambios
                if (this.dateTimePickerFecha.Tag.ToString().Trim() == fecha.Trim() &&
                    this.txtDescripcion.Tag.ToString().Trim() == this.txtDescripcion.Text.Trim())
                {
                    //No existen cambios
                    return;
                }

                string query = "";

                if (this.dateTimePickerFecha.Tag.ToString().Trim() != fecha.Trim())
                {
                    //Actualizar la Tabla GLI03 (campo fecha del comprobante)
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "set FECOIC = " + fecha;
                    query += " where ";
                    query += "CCIAIC='" + this.comprobanteContableImportar.Cab_compania + "' and ";
                    query += "SAPRIC= " + this.comprobanteContableImportar.Cab_anoperiodo + " and ";
                    query += "TICOIC= " + this.comprobanteContableImportar.Cab_tipo + " and ";
                    query += "NUCOIC= " + this.comprobanteContableImportar.Cab_noComprobante;

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                  
                    if (registros == 1) this.dateTimePickerFecha.Tag = fecha;
                }

                if (this.txtDescripcion.Tag.ToString().Trim() != this.txtDescripcion.Text.Trim())
                {
                    //Actualizar la Tabla GLAI3 (comentarios)

                    //Coger la 1ra línea de comentarios
                    string nombreTabla = GlobalVar.PrefijoTablaCG + "GLAI3";
                    string whereId = "";
                    switch (this.tipoBaseDatosCG)
                    {
                        case "DB2":
                            query = "select RRN(" + nombreTabla + ") as id, " + nombreTabla + ".* from " + nombreTabla;
                            whereId = "RRN(" + nombreTabla + ")";
                            break;
                        case "SQLServer":
                            query += "select GERIDENTI as id, " + nombreTabla + ".* from " + nombreTabla;
                            whereId = "GERIDENTI";
                            break;
                        case "Oracle":
                            //id_prefijotabla + GLAI3
                            string campoOracle = "ID_" + nombreTabla;
                            query += "select " + campoOracle + " as id, " + nombreTabla + ".* from " + nombreTabla;
                            whereId = campoOracle;
                            break;
                    }

                    query += " where CCIAAD='" + this.comprobanteContableImportar.Cab_compania + "' and ";
                    query += "SAPRAD= " + this.comprobanteContableImportar.Cab_anoperiodo + " and ";
                    query += "TICOAD= " + this.comprobanteContableImportar.Cab_tipo + " and ";
                    query += "NUCOAD= " + this.comprobanteContableImportar.Cab_noComprobante;
                    query += " order by id ";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    string idPrimerComentario = "";
                    if (dr.Read())
                    {
                        idPrimerComentario = dr["id"].ToString().Trim();
                    }

                    dr.Close();

                    string comentario = this.txtDescripcion.Text.PadRight(36, ' ');

                    if (idPrimerComentario == "")
                    {
                        query = "insert into " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                        query += "(CCIAAD, SAPRAD, TICOAD, NUCOAD, COHEAD) values ('";
                        query += this.comprobanteContableImportar.Cab_compania + "', " + this.comprobanteContableImportar.Cab_anoperiodo + ", ";
                        query += this.comprobanteContableImportar.Cab_tipo + ", " + this.comprobanteContableImportar.Cab_noComprobante + ", '";
                        query += this.txtDescripcion.Text.PadRight(36, ' ');
                        query += "')";
                    }
                    else
                    {
                        query = "update " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                        query += "set COHEAD = '" + this.txtDescripcion.Text.PadRight(36, ' ') + "' ";
                        query += "where CCIAAD='" + this.comprobanteContableImportar.Cab_compania + "' and ";
                        query += "SAPRAD= " + this.comprobanteContableImportar.Cab_anoperiodo + " and ";
                        query += "TICOAD= " + this.comprobanteContableImportar.Cab_tipo + " and ";
                        query += "NUCOAD= " + this.comprobanteContableImportar.Cab_noComprobante + " and ";
                        query += whereId + "= " + idPrimerComentario;
                    }

                    int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (reg == 1) this.txtDescripcion.Tag = this.txtDescripcion.Text;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        /// <summary>
        /// Actualiza el comprobante en las tablas (GLI03, GLB01, GLBX1, GLAI3)
        /// </summary>
        private void ActualizarComprobanteTablaGLB01()
        {
            try
            {
                ComprobanteContCabeceraDetalle1FilaGLB01 comp = new ComprobanteContCabeceraDetalle1FilaGLB01
                {
                    LPValor = this.LP
                };

                //Actualizar los datos de la cabecera del comprobante
                comp.ActualizarCompCabeceraDesdeFormAltaEdita(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0], this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]);

                comp.cab_anoperiodoInicial = this.txtMaskAAPP.Tag.ToString();
                comp.cab_tipoInicial = this.cmbTipo.Tag.ToString();
                comp.cab_noComprobanteInicial = this.txtNoComprobante.Tag.ToString();

                //Actualizar la tabla GLI03 (cabecera del comprobante);
                //string result = this.ActualizarComprobanteTablaCabeceraGLI03(codCompania, saappInicial, codTipoInicial, noCompInicial);
                string result = comp.ActualizarComprobanteTablaCabeceraGLI03(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0], this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]);

                //Actualiza la tabla comentario del comprobante
                comp.ActualizaPrimerComentarioCompTablaGLAI3(this.txtDescripcion.Text);

                //Actualizar la tabla GLB01 (detalle del comprobante)
                comp.det_SIMI = "0";

                for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                {
                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i))
                    {
                        //Actualizar la línea de detalle para el comprobante
                        result = comp.ActualizarCompDetalleDesdeFilaForm(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]);

                        if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["RowNumber"].ToString() != "")
                        {
                            result = comp.ActualizarDetalleComprobanteTablaGLB01();
                            if (this.extendido == true) result = comp.ActualizarDetalleComprobanteExtTablaGLBX1();
                        }
                        else
                        {
                            //Calcular el SIMI si fuera necesario
                            if (comp.det_SIMI == "0") comp.det_SIMI = comp.CalcularSIMIDetalleComprobanteTablaGLM01().ToString();
                            result = comp.InsertarDetalleComprobanteGLB01();
                            if (this.extendido == true) result = comp.InsertarDetalleComprobanteExtGLBX1();
                        }
                    }
                    gridCambiada = false;
                    dNoPreguntar = true;
                }
                
                //result = this.ActualizarComprobanteTablaDetalleGLB01(codCompania, saappInicial, codTipoInicial, noCompInicial);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Graba un nuevo comprobante en la tabla GLB01
        /// </summary>
        private void GrabarNuevoComprobanteTablaGLB01()
        {
            string resultCab = "";
            try
            {
                ComprobanteContCabeceraDetalle1FilaGLB01 comp = new ComprobanteContCabeceraDetalle1FilaGLB01
                {
                    LPValor = this.LP
                };

                //Actualizar los datos de la cabecera del comprobante
                comp.ActualizarCompCabeceraDesdeFormAltaEdita(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0], this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]);

                //Chequear si existe el comprobante
                bool existeComp = comp.ExisteComprobanteTablaGLI03();
                if (existeComp)
                {
                    MessageBox.Show("Ya existe un comprobante con la misma cabecera", this.LP.GetText("errValTitulo", "Error"));
                    return;
                }

                //Insertar la cabecera del comprobante en la tabla GLI03
                resultCab = comp.InsertarComprobanteTablaCabeceraGLI03(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]);

                //Insertar la descripción del comprobante tabla GLAI3 (cabecera del comprobante)
                string result = comp.InsertarComentarioCompTablaGLAI3(this.txtDescripcion.Text);

                //Insertar en la tabla GLB01 (detalle del comprobante)
                
                //comp.det_SIMI = "1"; jl
                string rowNumber = "";
                for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                {
                    if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i))
                    {
                        //Actualizar la línea de detalle para el comprobante
                        result = comp.ActualizarCompDetalleDesdeFilaForm(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]);

                        //Insertar la línea de detalle

                        //Calcular el SIMI si fuera necesario
                        comp.det_SIMI = comp.CalcularSIMIDetalleComprobanteTablaGLM01().ToString();

                        result = comp.InsertarComprobanteTablaDetalleGLB01();

                        if (result == "")
                        {
                            //Buscar el identificador del RowNumber de la linea insertada
                            rowNumber = comp.ObtenerIdLastRowNumberDetalleGLB01();
                            if (rowNumber != "") this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["RowNumber"] = rowNumber.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (resultCab == "") 
            {
                //Pasar el comprobante a edición y quitarle la marca de nuevo
                this.edicionComprobanteGLB01 = true;
                this.nuevoComprobanteGLB01 = false;
                this.nuevoComprobante = false;

                //Actualizar los atributos TAG de los controles de la cabecera
                this.ActualizaValoresOrigenControles();
            }
        }
        
        /// <summary>
        /// Mueve la columna país de posición. La sitúa detrás de la del Nit/Cif
        /// </summary>
        private void MoverIndiceColumnas()
        {
            try
            {
                int indice = this.dgDetalle.Columns["CifDni"].DisplayIndex;
                indice++;
                this.dgDetalle.Columns["Pais"].DisplayIndex = indice;
                indice++;
                if (this.extendido)
                {
                    this.dgDetalle.Columns["PrefijoDoc"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["NumFactAmp"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["NumFactRectif"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["FechaServIVA"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa1"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa2"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa3"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa4"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa5"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa6"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa7"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserAlfa8"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserNum1"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserNum2"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserFecha1"].DisplayIndex = indice++;
                    this.dgDetalle.Columns["CampoUserFecha2"].DisplayIndex = indice++;
                }

                if (this.edicionLote || this.edicionLoteError || this.edicionComprobanteGLB01 || this.nuevoComprobanteGLB01) this.dgDetalle.Columns["RowNumber"].DisplayIndex = indice++;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            try
            {
                if (this.cmbCompania.SelectedValue != null) this.cmbCompania.Tag = this.cmbCompania.SelectedValue.ToString();
                this.txtMaskAAPP.Tag = this.txtMaskAAPP.Text;
                
                this.dateTimePickerFecha.Tag = this.dateTimePickerFecha.Value;
                if (this.cmbTipo.SelectedValue != null) this.cmbTipo.Tag = this.cmbTipo.SelectedValue.ToString();
                this.txtNoComprobante.Tag = this.txtNoComprobante.Text;
                if (this.cmbClase.SelectedValue != null) this.cmbClase.Tag = this.cmbClase.SelectedValue;
                this.txtTasa.Tag = this.txtTasa.Text;
                this.txtDescripcion.Tag = this.txtDescripcion.Text;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        
        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            frmCompContAltaEdita frmAltaEdita = new frmCompContAltaEdita
            {
                NuevoComprobante = true,
                FrmPadre = this
            };
            frmAltaEdita.Show();
        }

        private void RadButtonGrabar_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;
       
            string gravedad = "";
            string validarGrabarFormulario = ValidarGrabarFormulario();
            gravedad = validarGrabarFormulario.Substring(0, 1);

            int length = validarGrabarFormulario.Length -1;
            if (length > 0) validarGrabarFormulario = validarGrabarFormulario.Substring(1, length);
            else validarGrabarFormulario = "";
            bool grabar = true;
            if (validarGrabarFormulario != "" && gravedad == "0")
            {
                //Hay errores, pedir confirmación para grabar
                string mensaje = this.LP.GetText("errGrabarErrores", "Se han encontrado los siguientes errores") + ": \n\r" + validarGrabarFormulario + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿De todas formas desea grabar el fichero?");
                DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.No) grabar = false;
            }
            else if (validarGrabarFormulario != "" && gravedad == "1")
            {
                //Hay errores, pedir confirmación para grabar
                string mensaje = this.LP.GetText("errGrabarErroresG", "Se han encontrado los siguientes errores Graves, no se puede continuar con la operación pedida") + ": \n\r" + validarGrabarFormulario + "\n\r" + this.LP.GetText("errGrabarErroresPreg2", "¿Desea Salir sin Grabar?");
                DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm2", "Confirmación"), MessageBoxButtons.YesNo);
                grabar = false;
                if (result == DialogResult.Yes)
                {
                    dNoPreguntar = true;
                    Close();
                    return;
                }
            }
            
            if (grabar)
            {
                // Actualizar valores de tag para que no pregunte por cambios al salir
                if (!gridCambiada)
                {
                    this.cmbCompania.Tag = this.cmbCompania.SelectedValue.ToString();
                    this.txtMaskAAPP.Tag = this.txtMaskAAPP.Text.ToString();
                    this.dateTimePickerFecha.Tag = Convert.ToDateTime(this.dateTimePickerFecha.Value);
                    if (this.cmbTipo.SelectedValue == null) this.cmbTipo.Tag = "";
                    else this.cmbTipo.Tag = this.cmbTipo.SelectedValue.ToString();
                    this.txtNoComprobante.Tag = this.txtNoComprobante.Text.ToString();
                    this.cmbClase.Tag = this.cmbClase.SelectedValue.ToString();
                    this.txtTasa.Tag = this.txtTasa.Text.ToString();
                    this.txtDescripcion.Tag = this.txtDescripcion.Text.ToString();
                    this.gridChange = false;
                }

                if (this.edicionComprobanteGLB01 && this.comprobanteSoloConsulta)
                {
                    //Comprobante en modo consulta. Actualizar los campos que son posibles modificar (fecha, descripcion)
                    this.ActualizarComprobanteGLB01SoloConsulta();
                }
                else
                {
                    if (this.nuevoComprobanteGLB01 || this.edicionComprobanteGLB01)
                    {
                        //Ocultar el mensaje de Validación correcta en caso de que el comprobante no tenga errores
                        this.showMsgValidacionOk = false;
                        //Verificar que el comprobante este correctamente validado (todas sus lineas de detalle)
                        this.radButtonValidar.PerformClick();
                        this.showMsgValidacionOk = true;

                        if (this.comp.DSErrores.Tables["Errores"].Rows.Count > 0)
                        {
                            //Chequear si es el de comprobante no cuadra, único caso en el que se permite grabar
                            if (!(this.comp.DSErrores.Tables["Errores"].Rows.Count == 1 &&
                                this.comp.DSErrores.Tables["Errores"].Rows[0]["Error"].ToString() == this.LP.GetText("lblfrmCompContErrSinCuadrar", "Comprobante sin cuadrar")))
                            {
                                //comp.DSErroresAdd(-1, this.LP.GetText("lblfrmCompContErrSinCuadrar", "Comprobante sin cuadrar"), "T", "");
                                MessageBox.Show(this.LP.GetText("lblCompContConErrores", "Comprobante con errores"), this.LP.GetText("errValTitulo", "Error"));
                                return;
                            }
                        }
                    }

                    //Actualizar Tabla Cabecera
                    this.ActualizarTablaCabeceraDesdeForm();

                    //Actualizar Tabla Totales
                    this.ActualizarTablaTotalesDesdeForm();

                    //Actualizar Tabla Detalles Fechas para grabar
                    this.ActualizarTablaDetallesFechas(true);

                    if (this.edicionComprobanteGLB01)
                    {
                        //Actualiza el comprobante en las tablas (GLI03, GLB01, GLBX1, GLAI3)
                        this.ActualizarComprobanteTablaGLB01();
                    }
                    else
                    if (this.edicionLote || this.edicionLoteError)
                    {
                        //Actualiza el comprobante en las tablas
                        this.ActualizarComprobanteTablas();
                        //this.Close(); //jl
                    }
                    else
                        if (this.nuevoComprobante || this.importarComprobante)
                    {
                        if (this.nuevoComprobante && this.nuevoComprobanteGLB01)
                        {
                            this.GrabarNuevoComprobanteTablaGLB01();
                        }
                        else
                        {
                            //Graba el comprobante en formato xml
                            this.GrabarNuevoComprobante(false);
                        }
                    }
                    else
                    {
                        this.ActualizarComprobante();
                    }

                    //Actualizar Tabla Detalles Fechas para edición
                    this.ActualizarTablaDetallesFechas(false);
                }

                //Actualizar los atributos TAG de los controles de la cabecera
                ActualizaValoresOrigenControles(); 
                dGrabar = true;
                DevolverValor(); //jl
                this.Close(); //jl
                dGrabar = false;
            }

            // Set cursor as default arrow
            
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonGrabarComo_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            string validarGrabarFormulario = ValidarGrabarFormulario();

            bool grabar = true;
            if (validarGrabarFormulario != "")
            {
                //Hay errores, pedir confirmación para grabar
                string mensaje = this.LP.GetText("errGrabarErrores", "Se han encontrado los siguientes errores") + ": \n\r" + validarGrabarFormulario + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿De todas formas desea grabar el fichero?");
                DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.No) grabar = false;
            }

            if (grabar)
            {
                //Actualizar Tabla Cabecera
                this.ActualizarTablaCabeceraDesdeForm();

                //Actualizar Tabla Totales
                this.ActualizarTablaTotalesDesdeForm();

                //Actualizar Tabla Detalles Fechas para grabar
                this.ActualizarTablaDetallesFechas(true);

                //Graba el comprobante en formato xml
                this.GrabarNuevoComprobante(true);

                //Actualizar Tabla Detalles Fechas para edición
                this.ActualizarTablaDetallesFechas(false);

            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonValidar_Click(object sender, EventArgs e)
        {
            //SMR - necesario para que al validar tome el ultimo valor de la celda, no el anterior
            this.dgDetalle.BindingContext[this.dgDetalle.DataSource].EndCurrentEdit();

            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.btnSel.Visible = false;

                if (this.dgErrores.Visible == true) this.dgDetalle.Height = this.dgDetalle.Height + this.dgErrores.Height;
                utiles.ButtonEnabled(ref this.radButtonValidarErrores, false);
                this.dgErrores.Visible = false;

                //Crear el comprobante y llamar a las funciones de validacion
                comp = new ComprobanteContable
                {
                    Cab_compania = this.cmbCompania.SelectedValue.ToString()
                    
                };

                //coger el valor sin la máscara
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                comp.Cab_anoperiodo = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;
                comp.EsNuevo = NuevoComprobanteGLB01;
                comp.Cab_fecha = this.dateTimePickerFecha.Value.ToShortDateString();

                if (this.cmbTipo.SelectedValue != null)
                {
                    comp.Cab_tipo = this.cmbTipo.SelectedValue.ToString();
                }
                else
                {
                    comp.Cab_tipo = "";
                }

                //Se utiliza para validar el tipo de comprobante (batch o interactivo)
                if (this.edicionComprobanteGLB01 || this.nuevoComprobanteGLB01) comp.CompGLB01 = true;
                else comp.CompGLB01 = false;
                
                comp.Cab_noComprobante = this.txtNoComprobante.Text;
                comp.Cab_descripcion = this.txtDescripcion.Text;
                if (this.cmbClase.SelectedValue != null) comp.Cab_clase = this.cmbClase.SelectedValue.ToString();
                else comp.Cab_clase = "";

                if (extendido) comp.Cab_extendido = "1";
                else comp.Cab_extendido = "0";

                comp.Det_detalles = this.dgDetalle.dsDatos.Tables["Detalle"].Clone();

                string debe_haber = "";

                ArrayList filas = new ArrayList();
                if (this.dgDetalle.SelectedRows.Count > 0)
                {
                    //    Create a generics list to hold selected rows so it can be sorted later
                    List<DataGridViewRow> dgSelectedRows = new List<DataGridViewRow>();

                    foreach (DataGridViewRow dgvr in this.dgDetalle.SelectedRows)
                        dgSelectedRows.Add(dgvr);

                    //    Sort list based on DataGridViewRow.Index    
                    dgSelectedRows.Sort(DataGridViewRowIndexCompare);

                    int indiceDetComp = 0;
                    foreach (DataGridViewRow row in dgSelectedRows)
                    {
                        int selectedIndex = row.Index;
                        filas.Add(selectedIndex);
                        //Copiar solo las filas seleccionadas
                        comp.Det_detalles.ImportRow(((DataRowView)(row.DataBoundItem)).Row);
                        debe_haber = row.Cells["DH"].Value.ToString();
                        comp.Det_detalles.Rows[indiceDetComp]["DH"] = debe_haber;
                        indiceDetComp++;
                    }
                }
                else
                {
                    int i = 0;
                    foreach (DataRow row in this.dgDetalle.dsDatos.Tables["Detalle"].Rows)
                    {
                        filas.Add(i);
                        debe_haber = row["DH"].ToString();
                        comp.Det_detalles.ImportRow(row);
                        comp.Det_detalles.Rows[i]["DH"] = debe_haber;
                        i++;
                    }
                }

                comp.LPValor = this.LP;

                this.grBoxProgressBar.Text = this.LP.GetText("lblCompContValidando", "Validando");
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Blocks;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = comp.Det_detalles.Rows.Count;
                this.progressBarEspera.Visible = true;
                this.Refresh();

                bool resultCabecera = comp.ValidarCabecera();

                //Validar Cuadre
                bool comprobanteCuadra = this.ValidarComprobanteCuadra();
                if (!comprobanteCuadra) comp.DSErroresAdd(-1, this.LP.GetText("lblfrmCompContErrSinCuadrar", "Comprobante sin cuadrar"), "T", "");

                //Validar Importe
                bool comprobanteSinImporte = this.ValidarComprobanteSinImporte();
                if (!comprobanteSinImporte) comp.DSErroresAdd(-1, this.LP.GetText("lblfrmCompContErrSinImportes", "Comprobante sin Importes"), "T", "");

                bool resultAux = true;
                bool resultDetalle = true;

                //if (!TodaFilaEnBlanco(this.dgDetalle.dsDatos.Tables["Detalle"], i)) noApuntes++;
                if (comp.Det_detalles.Rows.Count == 0)
                {
                    //No existen lineas de detalle 
                }
                else
                    for (int i = 0; i < comp.Det_detalles.Rows.Count; i++)
                    {
                        resultAux = comp.ValidarDetalle(i, Convert.ToInt16(filas[i]) + 1);
                        if (!resultAux) resultDetalle = false;

                        //Mover la barra de progreso
                        this.progressBarEspera.Value = this.progressBarEspera.Value + 1;
                        this.progressBarEspera.Refresh();
                    }

                this.progressBarEspera.Visible = false;
                this.grBoxProgressBar.Visible = false;

                if (!resultCabecera || !resultDetalle || !comprobanteCuadra || !comprobanteSinImporte)
                {
                    utiles.ButtonEnabled(ref this.radButtonValidarErrores, true);

                    //Mostrar el Grid de Errores
                    this.ViewGridErrores();

                    /*ScrollableMessageBox scrollMsgBox = new ScrollableMessageBox();
                    scrollMsgBox.Titulo = "Validación comprobante";
                    scrollMsgBox.TextoBotonAceptar = "Aceptar";
                    scrollMsgBox.TextoMensaje = this.comp.Errores;
                    scrollMsgBox.ShowDialog();*/
                }
                else
                {
                    utiles.ButtonEnabled(ref this.radButtonValidarErrores, false);
                    if (!this.edicionLoteError && this.showMsgValidacionOk) MessageBox.Show(this.LP.GetText("lblValidarOk", "Validación correcta"), this.LP.GetText("lblCompContComprobante", "Comprobante"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonErroresValidar_Click(object sender, EventArgs e)
        {
            if (this.comp != null)
            {
                if (this.dgErrores.Visible)
                {
                    this.dgDetalle.Height = this.dgDetalle.Size.Height + this.dgErrores.Size.Height;
                    this.dgErrores.Visible = false;
                }
                else
                {
                    if (this.comp.DSErrores.Tables["Errores"].Rows.Count > 0)
                    {
                        //Mostrar el Grid de Errores
                        this.ViewGridErrores();
                    }
                }
            }
        }

        private void RadButtonRevertir_Click(object sender, EventArgs e)
        {
            //Multiplicar por -1 las columnas (MonedaLocal, Moneda Extranjera e Importes)
            decimal monedaLocal;
            decimal monedaExtranjera;
            decimal importe3;

            bool change = false;

            try
            {
                for (int i = 0; i < this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count; i++)
                {

                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns["MonedaLocal"].ReadOnly &&
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"].ToString() != "")
                    {
                        monedaLocal = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"]);
                        monedaLocal = monedaLocal * -1;
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaLocal"] = monedaLocal;
                        change = true;
                    }

                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns["MonedaExt"].ReadOnly &&
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"].ToString() != "")
                    {
                        monedaExtranjera = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"]);
                        monedaExtranjera = monedaExtranjera * -1;
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["MonedaExt"] = monedaExtranjera;
                        change = true;
                    }

                    if (!this.dgDetalle.dsDatos.Tables["Detalle"].Columns["Importe3"].ReadOnly &&
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"].ToString() != "")
                    {
                        importe3 = Convert.ToDecimal(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"]);
                        importe3 = importe3 * -1;
                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Importe3"] = importe3;
                        change = true;
                    }
                }

                //Recalcular la tabla de totales
                this.gridChange = true;
                if (change) this.CalcularTotales();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BuildMenuClickDerecho()
        {
            this.radContextMenuClickDerecho = new RadContextMenu();
            RadMenuItem menuItemNuevo = new RadMenuItem("Nuevo");
            menuItemNuevo.Click += new EventHandler(RadButtonNuevo_Click);
            RadMenuItem menuItemEditar = new RadMenuItem("Editar");
            //menuItemEditar.Click += new EventHandler(radButtonEditar_Click);
            this.radContextMenuClickDerecho.Items.Add(menuItemNuevo);
            this.radContextMenuClickDerecho.Items.Add(menuItemEditar);

            //e.ContextMenu = this.radContextMenuClickDerecho.DropDown;
        }

        private void RadBtnMenuMostrarOcultar_Click(object sender, EventArgs e)
        {
            if (this.menuLateralExpanded)
            {
                int sizePanel = this.radPanelAcciones.Size.Width;
                int sizeButton = this.radBtnMenuMostrarOcultar.Width;
                collapseWidth = sizePanel - sizeButton - 4;

                this.menuLateralExpanded = false;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width - collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = true;

                this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width + collapseWidth, this.radPanelMenuPath.Height);
                this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X - collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width + collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X - collapseWidth, this.radPanelApp.Location.Y);
            }
            else
            {
                this.menuLateralExpanded = true;
                this.radPanelAcciones.Size = (Size)new Point(this.radPanelAcciones.Size.Width + collapseWidth, this.radPanelAcciones.Height);
                this.radLabelOpciones.Visible = false;

                this.radPanelMenuPath.Size = (Size)new Point(this.radPanelMenuPath.Size.Width - collapseWidth, this.radPanelMenuPath.Height);
                this.radPanelMenuPath.Location = new Point(this.radPanelMenuPath.Location.X + collapseWidth, this.radPanelMenuPath.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width - collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X + collapseWidth, this.radPanelApp.Location.Y);
            }
        }
        
        private void CmbCompania_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.cmbCompania.SelectedValue != null)
            {
                string codigo = this.cmbCompania.SelectedValue.ToString();

                string result = this.ValidarCompania(codigo);
                
                this.CrearComboClase();

                if (this.nuevoComprobante)
                {
                    this.ControlesHabilitarDeshabilitar(true);
                }
            }

            /*
            string result = ValidarCompania(codigo);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                this.cmbCompania.Focus();
            }
            else
            {
                if (this.cmbCompania.Text.Length <= 2)
                {
                    if (this.GLM01_NCIAMG != "") this.tgTexBoxSelCompania.Textbox.Text += " " + separadorDesc + " " + this.GLM01_NCIAMG;
                }

                this.CrearComboClase();
            }
            

            //this.tgTexBoxSelCompania.Textbox.Select(0, 0);
            */
        }

        private void CmbClase_SelectedValueChanged(object sender, EventArgs e)
        {
            //Al seleccionar la clase habilitar o no las columnas de Moneda Local y de Moneda Extranjera de los detalles del comprobante
            if (this.cmbClase.SelectedValue != null && this.dgDetalle.Columns.Count > 6)
            {
                switch (this.cmbClase.SelectedValue.ToString())
                {
                    case "2":
                        this.dgDetalle.ColumnNotEnable("MonedaLocal", true, "");
                        this.dgDetalle.ColumnEnable("MonedaExt");
                        break;
                    case "0":
                    case "1":
                        this.dgDetalle.ColumnEnable("MonedaLocal");
                        this.dgDetalle.ColumnNotEnable("MonedaExt", true, "");
                        break;
                    default:
                        this.dgDetalle.ColumnEnable("MonedaLocal");
                        this.dgDetalle.ColumnEnable("MonedaExt");
                        break;
                }
            }
        }

        //private void RadButtonNuevo_MouseEnter(object sender, EventArgs e)
        //{
            //utiles.ButtonMouseEnter(ref this.radButtonNuevo);
        //}

        //private void RadButtonNuevo_MouseLeave(object sender, EventArgs e)
        //{
            //utiles.ButtonMouseLeave(ref this.radButtonNuevo);
        //}

        private void RadButtonGrabar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabar);
        }

        private void RadButtonGrabar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabar);
        }

        private void RadButtonGrabarComo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabarComo);
        }

        private void RadButtonGrabarComo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabarComo);
        }

        private void RadButtonValidar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonValidar);
        }

        private void RadButtonValidar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonValidar);
        }

        private void RadButtonValidarErrores_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonValidarErrores);
        }

        private void RadButtonValidarErrores_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonValidarErrores);
        }

        private void RadButtonRevertir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonRevertir);
        }

        private void RadButtonRevertir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonRevertir);
        }

        private void RadButtonImportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonImportar);
        }

        private void RadButtonImportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonImportar);
        }

        private void RadButtonExportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExportar);
        }

        private void RadButtonExportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExportar);
        }

        private void RadButtonSalir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSalir);
        }

        private void RadButtonSalir_MouseLeave(object sender, EventArgs e)
        {
                utiles.ButtonMouseLeave(ref this.radButtonSalir);
        }

        private void BtnVerComentarios_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnVerComentarios);
        }

        private void BtnVerComentarios_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnVerComentarios);
        }

        private void RadButtonImportar_Click(object sender, EventArgs e)
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                this.grBoxProgressBar.Text = this.LP.GetText("lblCompContImportando", "Importando");
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                ExcelExportImport excelImport = new ExcelExportImport();
                string cabecera = ConfigurationManager.AppSettings["ModComp_Excel_Cabecera"];
                if (cabecera != null)
                {
                    excelImport.Cabecera = (cabecera == "Yes" ? true : false);
                }
                string result = excelImport.Importar();

                if (result == "")
                {
                    this.dgDetalle.AddUltimaFilaSiNoHayDisponile = false;

                    //Eliminar Todas las filas del DataGrid
                    this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Clear();

                    for (int i = 0; i <= excelImport.DateTableDatos.Rows.Count; i++)
                    {
                        //Mover la barra de progreso
                        if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                        else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                        this.progressBarEspera.Refresh();

                        DataRow row = this.dgDetalle.dsDatos.Tables["Detalle"].NewRow();

                        //Insertar filas con el valor de la fila del excel
                        if (i < excelImport.DateTableDatos.Rows.Count)
                        {
                            for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                            {
                                if (j < excelImport.DateTableDatos.Columns.Count)
                                {
                                    row[j] = excelImport.DateTableDatos.Rows[i][j].ToString();
                                }
                                else break;
                            }
                        }
                        else
                        {
                            //Insertar fila en blanco
                            for (int j = 0; j < this.dgDetalle.dsDatos.Tables["Detalle"].Columns.Count; j++)
                            {
                                if (j < excelImport.DateTableDatos.Columns.Count)
                                {
                                    row[j] = "";
                                }
                                else break;
                            }
                        }

                        this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Add(row);

                        //Habilitar / Deshabilitar Columnas
                        this.UpdateEstadoColumnasDadoCuentaMayor(this.dgDetalle.dsDatos.Tables["Detalle"].Rows[i]["Cuenta"].ToString(), i);
                    }

                    this.dgDetalle.AddRowNumber(DataGridViewContentAlignment.MiddleRight);

                    //Recalcular Totales
                    this.CalcularTotales();
                }
                else
                {
                    if (result != "CANCELAR")
                    {
                        MessageBox.Show(this.LP.GetText("errImportExcel", "Error importando fichero excel") + " (" + result + ")", error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errImportExcel", "Error importando fichero excel") + " (" + ex.Message + ")", error);
            }

            this.progressBarEspera.Visible = false;
            this.grBoxProgressBar.Visible = false;

            this.dgDetalle.AddUltimaFilaSiNoHayDisponile = true;

            if (this.dgDetalle.dsDatos.Tables["Detalle"].Rows.Count == 0) this.dgDetalle.AdicionarFila();
        }
 
private void RadButtonExportar_Click(object sender, EventArgs e)
        {
            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                this.grBoxProgressBar.Text = this.LP.GetText("lblCompContExportando", "Exportando");
                this.grBoxProgressBar.Visible = true;
                this.grBoxProgressBar.Top = this.Size.Height / 2;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.MarqueeAnimationSpeed = 30;
                this.progressBarEspera.Style = ProgressBarStyle.Marquee;
                this.progressBarEspera.Visible = true;
                this.progressBarEspera.Value = 0;
                this.progressBarEspera.Maximum = 1000;
                this.progressBarEspera.Visible = true;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                ExcelExportImport excelImport = new ExcelExportImport
                {
                    DateTableDatos = this.dgDetalle.dsDatos.Tables["Detalle"]
                };
                string cabecera = ConfigurationManager.AppSettings["ModComp_Excel_Cabecera"];
                if (cabecera != null)
                {
                    excelImport.Cabecera = (cabecera == "Yes" ? true : false);
                    //if (excelImport.Cabecera) excelImport.GridColumnas = this.dgDetalle.Columns;
                }

                if (!this.extendido)
                {
                    //Eliminar columnas de campos extendidos del detalle del comprobante
                    this.EliminarColumnasCamposExtendidos();
                }

                string result = excelImport.ExportarMemoria();

                this.progressBarEspera.Visible = false;
                this.grBoxProgressBar.Visible = false;

                if (result != "" && result != "CANCELAR")
                {
                    MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + result + ")", error);
                }

                /*
                //Exportar a Excel 
                string stOutput = "";
                // Export titles:
                string sHeaders = "";

                for (int j = 0; j < this.dgDetalle.Columns.Count; j++)
                    sHeaders = sHeaders.ToString() + Convert.ToString(this.dgDetalle.Columns[j].HeaderText) + "\t";
                stOutput += sHeaders + "\r\n";
                // Export data.
                for (int i = 0; i < this.dgDetalle.RowCount - 1; i++)
                {
                    string stLine = "";
                    for (int j = 0; j < this.dgDetalle.Rows[i].Cells.Count; j++)
                        stLine = stLine.ToString() + Convert.ToString(this.dgDetalle.Rows[i].Cells[j].Value) + "\t";
                    stOutput += stLine + "\r\n";
                }
                Encoding utf16 = Encoding.GetEncoding(1254);
                byte[] output = utf16.GetBytes(stOutput);
                string filename = @"C:\VS2010_Projects\FinanzasNet\ModComprobantes\comprobantesContables\export.xls";
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(output, 0, output.Length); //write the encoded file
                bw.Flush();
                bw.Close();
                fs.Close();
                 */
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", error);
            }
        }

        /// devuelve valor a frmCompContListaGLB01
        private void DevolverValor()
        {
            ComprobanteContCabeceraDetalle1FilaGLB01 compCont = new ComprobanteContCabeceraDetalle1FilaGLB01();
            ArrayList elementosSel = new System.Collections.ArrayList();
            
            
            if (this.EsNuevo)
            {
                
                DataRow rowCabecera = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0];
                elementosSel.Add(rowCabecera["Compania"].ToString());  // cia
                elementosSel.Add(rowCabecera["AnoPeriodo"].ToString()); // AA-PP
                elementosSel.Add(rowCabecera["Tipo"].ToString());  // Tipo
                elementosSel.Add(compCont.cab_noComprobante);
                elementosSel.Add(rowCabecera["Fecha"].ToString());  // FECHA dd/mm/aa
                elementosSel.Add(rowCabecera["Extendido"].ToString());
                if (extendido) elementosSel.Add("1");
                else elementosSel.Add("0");
            }
            else   // no es nuevo
            {
            if (Batch == false || BatchLote==true)   // lista comprobantes o gestión lotes
                {
                    elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Compania"]);
                    elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["AnoPeriodo"]);
                    elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Tipo"]);
                    elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Numero"]);
                    //elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Fecha"]);
                    //DateTime fecha_int = Convert.ToDateTime(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Fecha"].ToString());
                    string fecha_Seditar = this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["Fecha"].ToString();
                    if (Batch == false)
                    {
                        elementosSel.Add(fecha_Seditar.Substring(6, 2) + "/" + fecha_Seditar.Substring(4, 2) +
                                    "/" + fecha_Seditar.Substring(0, 4));
                    }
                    else
                    {
                        elementosSel.Add(fecha_Seditar.Substring(6, 2) + "/" + fecha_Seditar.Substring(4, 2) +
                                    "/" + fecha_Seditar.Substring(2, 2));
                    }
                    elementosSel.Add(compania_ant);
                    elementosSel.Add(aapp_ant);
                    elementosSel.Add(tipo_ant);
                    elementosSel.Add(nocomp_ant);
                    elementosSel.Add(fecha_ant);
                    //elementosSel.Add(this.comprobanteContableImportar.Cab_extendido);
                }
                else
                {
                    //elementosSel.Add(this.cmbCompania.Text.Substring(0,2).ToString());
                    elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["compania"]);
                    //elementosSel.Add(this.txtMaskAAPP.Text.ToString());
                    elementosSel.Add(this.txtMaskAAPP.Text);
                    elementosSel.Add(this.cmbTipo.Text.Substring(0, 2).ToString());
                    elementosSel.Add(this.txtNoComprobante.Text.ToString());
                    elementosSel.Add(utiles.FechaAno4DigitosToFormatoCG(utiles.FechaCadenaToDateTime(this.dateTimePickerFecha.Text)));
                    elementosSel.Add(compania_ant);
                    elementosSel.Add(aapp_ant);
                    elementosSel.Add(tipo_ant);
                    elementosSel.Add(nocomp_ant);
                    elementosSel.Add(fecha_ant);

                }
            }
            if (extendido) elementosSel.Add("1");
            else elementosSel.Add("0");
            if (Batch == false && BatchLote == false && BatchLoteError == false)
            {
                elementosSel.Add(this.tablaTotales.Controls[5].Text);  //Local Debe
                elementosSel.Add(this.tablaTotales.Controls[6].Text); //Local Haber
                elementosSel.Add(this.tablaTotales.Controls[7].Text); // Ext. Debe
                elementosSel.Add(this.tablaTotales.Controls[8].Text); // Ext. Haber
                elementosSel.Add(this.tablaTotales.Controls[9].Text); // Importe 3 Debe
                elementosSel.Add(this.tablaTotales.Controls[10].Text); // Importe 3 Haber
                elementosSel.Add(this.lblNoApuntes_Valor.Text); // N.Apuntes
            }
            else
            {
                elementosSel.Add(lblMonedaLocal_Debe.Text);  //Local Debe
                elementosSel.Add(lblMonedaLocal_Haber.Text); //Local Haber
                elementosSel.Add(this.lblMonedaExtranjera_Debe.Text); // Ext. Debe
                elementosSel.Add(this.lblMonedaExtranjera_Haber.Text); // Ext. Haber
                elementosSel.Add(this.lblImporte3_Debe.Text); // Importe 3 Debe
                elementosSel.Add(this.lblImporte3_Haber.Text); // Importe 3 Haber
                //elementosSel.Add(this.lblNoApuntes_Valor.Text); // N.Apuntes
                elementosSel.Add(this.dgDetalle.dsDatos.Tables["Totales"].Rows[0]["NumeroApuntes"]); // N.Apuntes
            }
            if (Batch == true)
            {
                elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["descripcion"]);
                elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["clase"]);
                elementosSel.Add(this.dgDetalle.dsDatos.Tables["Cabecera"].Rows[0]["tasa"]);
                if (BatchLote == false)
                    elementosSel.Add(this.ArchivoComprobante);
            }


                GlobalVar.ElementosSel = elementosSel;

            //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar o doble clik antes de cerrar el formulario .
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            if (ArgSel != null)
            {
                ArgSel(new ActualizaListaComprobantesArgs(elementosSel));
            }
        }

        private void dgDetalle_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format(
        "Cell at row {0}, column {1} value changed",
        e.RowIndex, e.ColumnIndex);
            MessageBox.Show(msg, "Cell Value Changed");
        }

        #endregion

        #endregion

        /*
        /// <summary>
        /// Copiar desde la Grid de detalles al Clipboard
        /// </summary>
        private void CopiarDetalles()
        {
            if (this.dgDetalles.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                // Add the selection to the clipboard.
                Clipboard.SetDataObject(this.dgDetalles.GetClipboardContent());
            }
        }

        /// <summary>
        /// Pega desde el Clipboard a la Grid de detalles
        /// </summary>
        private void PegarDetalles()
        {
            int lastRow = this.ds.Tables["Detalle"].Rows.Count;

            // Replace the text box contents with the clipboard text. 
            //string texto = Clipboard.GetText();

            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                int rowMenor = 0;
                int rowMayor = 0;
                int colMenor = 0;
                int colMayor = 0;
                int rowActual;
                int colActual;

                for (int i = 0; i < this.dgDetalles.SelectedCells.Count; i++)
                {
                    rowActual = this.dgDetalles.SelectedCells[i].RowIndex;
                    colActual = this.dgDetalles.SelectedCells[i].ColumnIndex;

                    if (i == 0)
                    {
                        rowMenor = rowActual;
                        rowMayor = rowActual;

                        colMenor = colActual;
                        colMayor = colActual;
                    }
                    else
                    {
                        if (rowActual < rowMenor) rowMenor = rowActual;
                        if (rowActual > rowMayor) rowMayor = rowActual;

                        if (colActual < colMenor) colMenor = colActual;
                        if (colActual > colMayor) colMayor = colActual;
                    }
                }


                //int rowOfInterest = this.dgDetalles.CurrentCell.RowIndex;
                int rowOfInterest = rowMenor;

                string[] selectedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

                if (selectedRows == null || selectedRows.Length == 0)
                    return;

                int colMenorAux;
                foreach (string row in selectedRows)
                {
                    colMenorAux = colMenor;

                    try
                    {
                        string[] data = Regex.Split(row, "\t");

                        //int col = this.dgDetalles.CurrentCell.ColumnIndex;

                        foreach (string ob in data)
                        {
                            if (colMenorAux >= this.dgDetalles.Columns.Count)
                                break;
                            if (ob != null)
                                this.dgDetalles[colMenorAux, rowOfInterest].Value = Convert.ChangeType(ob, this.dgDetalles[colMenorAux, rowOfInterest].ValueType);
                            colMenorAux++;
                        }

                        //Adicionar una fila en blanco si se ha utilizado la última
                        if (rowOfInterest == lastRow - 1)
                        {
                            this.ds.Tables["Detalle"].Rows.Add();
                            lastRow++;
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }

                    rowOfInterest++;
                }
            }
        }

        /// <summary>
        /// Elimina el contenido de las celdas seleccionadas
        /// </summary>
        private void BorrarDetalles()
        {
            for (int i = 0; i < this.dgDetalles.SelectedCells.Count; i++)
            {
                this.dgDetalles.SelectedCells[i].Value = "";
            }
        }

        /// <summary>
        /// Copia hacia el Clipboard el contenido de las celdas seleccionadas, después elimina el contenido de dichas celdas 
        /// </summary>
        private void CortarDetalles()
        {
            if (this.dgDetalles.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                // Add the selection to the clipboard.
                Clipboard.SetDataObject(this.dgDetalles.GetClipboardContent());

                for (int i = 0; i < this.dgDetalles.SelectedCells.Count; i++)
                {
                    this.dgDetalles.SelectedCells[i].Value = "";
                }
            }
        }

        /// <summary>
        /// Inserta una fila nueva arriba de la fila actual
        /// </summary>
        private void InsertarFila(int cantidad)
        {
            int newRow = 0;
            int currentRow = 0;
            try
            {
                for (int i = 0; i < this.dgDetalles.SelectedCells.Count; i++)
                {
                    currentRow = this.dgDetalles.SelectedCells[i].RowIndex;
                    break;
                }
            }
            catch
            {
            }

            if (currentRow > 0) newRow = currentRow--;
            DataRow dr = this.ds.Tables["Detalle"].NewRow();
            this.ds.Tables["Detalle"].Rows.InsertAt(dr, newRow);
        }

        /// <summary>
        /// Adiciona una fila nueva al final de la Grid
        /// </summary>
        private void c()
        {
            int lastRow = this.ds.Tables["Detalle"].Rows.Count;
            DataRow dr = this.ds.Tables["Detalle"].NewRow();
            this.ds.Tables["Detalle"].Rows.InsertAt(dr, lastRow);
        }

        /// <summary>
        /// Elimina las filas seleccionadas
        /// </summary>
        private void SuprimirFila()
        {
            try
            {
                int currentRow;
                for (int i = 0; i < this.dgDetalles.SelectedCells.Count; i++)
                {
                    currentRow = this.dgDetalles.SelectedCells[i].RowIndex;
                    this.ds.Tables["Detalle"].Rows.RemoveAt(currentRow);
                }

                if (this.ds.Tables["Detalle"].Rows.Count == 0) this.ds.Tables["Detalle"].Rows.Add();
            }
            catch
            {
            }
        }
        */
        #endregion

        /*
        private void dgDetalles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Chequear si es necesario adicionar una fila (en el caso que se esté introduciendo
            //información en la última fila, se habilitará una nueva fila)
            if (this.dgDetalles.CurrentCell.RowIndex == this.ds.Tables["Detalle"].Rows.Count - 1)
            {
                if (this.dgDetalles.CurrentCell.Value.ToString().Trim() != "") this.AdicionarFila();
            }

            //Falta ... actualizar el campo No apuntes, para ellos se chequeara si se esta en la columna cuenta de mayor entonces se recorrera todas las filas
            //y se contara las q tengan valor en ese campo
        }
         */
    }
}