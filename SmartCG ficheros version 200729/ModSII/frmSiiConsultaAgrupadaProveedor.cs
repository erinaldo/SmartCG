using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiConsultaAgrupadaProveedor : frmPlantilla, IReLocalizable
    {
        public string formCode = "MISIICARO";
        public string ficheroExtension = "aro";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MISIICARO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string ejercicio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string periodo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string nif;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string nombreRazonSocial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estadoCuadre;
        }

        FormularioValoresCampos valoresFormulario;

        private ArrayList periodoArray;
        private ArrayList estadoCuadreArray;

        private DataSet dsConsultaRespuesta;

        private string codigoCompania = "";
        private string ejercicioCG = "";
        private string tipoPeriodoImpositivo = "";

        public frmSiiConsultaAgrupadaProveedor()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }
        private void frmSiiConsultaAgrupadaProveedor_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Consulta de Facturas Informadas Agrupadas por Proveedor");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGTextBoxSel para las compañías fiscales
            this.BuildtgTexBoxSelCiaFiscal();

            //Definir la utilización del evento expuesto por el user control (TGTextBoxSel) que contiene el ListView, 
            //la asignación del handler requiere de un método declarado (tgTexBoxSelCompania_AceptarFormClose)
            this.tgTexBoxSelCiaFiscal.ValueChanged += new TGTexBoxSel.ValueChangedCommandEventHandler(tgTexBoxSelCiaFiscal_ValueChanged);

            //Crear el desplegable de Estado de Cuadre (No Contrastable/En proceso de contraste/...)
            this.CrearComboEstadoCuadre();

            //Crear el TGGrid
            this.BuiltgConsultaInfo();

            //Construir el DataSet con el resultado del envio
            this.dsConsultaRespuesta = new DataSet();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (this.CargarValoresUltimaPeticion(valores)) { }
            }

            this.tgTexBoxSelCiaFiscal.Textbox.Select();
        }

        /// <summary>
        /// Método donde se hará uso de los parámetro del argumento del evento ValueChangedCommandEventArgs
        /// </summary>
        /// <param name="e"></param>
        private void tgTexBoxSelCiaFiscal_ValueChanged(TGTexBoxSel.ValueChangedCommandEventArgs e)
        {
            if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "")
            {
                //Deshabilitar los controles del formulario, al haber introducido la compañía
                //this.ControlesHabilitarDeshabilitar(false);

                //No existe valor para la compañía
                //this.codigoCompania = "";
            }
            else
            {
                //Habilitar los controles del formulario, al haber introducido la compañía
                //this.ControlesHabilitarDeshabilitar(true);

                this.tgTexBoxSelCiaFiscal.Textbox.Focus();

                this.tgTexBoxSelCiaFiscal.Textbox.Modified = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Consultar();
        }

        private void tgTexBoxSelCiaFiscal_Enter(object sender, EventArgs e)
        {
            this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;
        }

        private void tgTexBoxSelCiaFiscal_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.tgTexBoxSelCiaFiscal.Textbox.Modified && this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
            {
                this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;

                this.codigoCompania = this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim();
                string result = this.ValidarCompania();
                if (result != "") MessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
                else
                {
                    //Crear el desplegable de Periodos
                    this.CrearComboPeriodos();
                }
            }
            else
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "" && periodoArray != null && periodoArray.Count > 1)
                {
                    //Eliminar periodos, falta seleccionar la compañía
                    this.periodoArray.Clear();

                    this.cmbPeriodo.DataSource = null;
                    this.cmbPeriodo.DataBindings.Clear();
                    this.cmbPeriodo.Refresh();
                }
            Cursor.Current = Cursors.Default;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtEjercicio.Text = "";
            if (this.cmbPeriodo.Enabled) this.cmbPeriodo.Text = "";
            this.txtNIF.Text = "";
            this.txtNombreRazon.Text = "";
            this.cmbEstadoCuadre.Text = "";
            this.gbResultado.Visible = false;
            this.tgGridConsulta.Visible = false;
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();    //Exporta a un HTML temporal y despúes se muestra en un Excel
        }

        private void toolStripButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void toolStripButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void cmbPeriodo_Leave(object sender, EventArgs e)
        {
            this.cmbPeriodo.Text = this.cmbPeriodo.Text.ToUpper();

            if (!LibroUtiles.ValidarPeriodo(this.cmbPeriodo.Text))
            {
                MessageBox.Show("Por favor, entre un periodo válido (01-12 o " + LibroUtiles.PeriodoAnual + "o 1T-4T)");     //Falta traducir
                this.cmbPeriodo.Focus();
                return;
            }
            else if (this.cmbPeriodo.Text.Length == 1) this.cmbPeriodo.Text = "0" + this.cmbPeriodo.Text;
        }

        private void frmSiiConsultaAgrupadaProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiConsultaAgrupadaProveedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Consulta de Facturas Informadas Agrupadas por Proveedor");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiConsultaAgrupadaProveedor Titulo", "Consulta de Facturas Informadas Agrupadas por Proveedor");
            this.Text += this.FormTituloAgenciaEntorno();

            this.gbBuscador.Text = " " + this.LP.GetText("lblBuscador", "Buscador") + " ";
            this.lblCompania.Text = this.LP.GetText("lblCompaniaFiscal", "Compañía Fiscal");
            this.lblEjercicio.Text = this.LP.GetText("lblEjercicio", "Ejercicio");
            this.lblPeriodo.Text = this.LP.GetText("lblPeriodo", "Periodo");

            this.lblNIF.Text = this.LP.GetText("lblNIFProveedor", "NIF Proveedor");     //Falta traducir
            this.lblNombreRazon.Text = this.LP.GetText("lblNombreRazonProveedor", "Nombre o Razón Social Proveedor");  //Falta traducir
            this.lblEstadoCuadre.Text = this.LP.GetText("lblEstadoCuadre", "Estado Cuadre");
            
            this.gbResultado.Text = " " + this.LP.GetText("gbResultado", "Resultado") + " ";
            this.lblNoInfo.Text = this.LP.GetText("lblNoExistenFacturas", "No existen facturas que cumplan el criterio seleccionado");

            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList();
            nombreColumnas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoNombreRazonSocial", "Nombre o razón social"));
            nombreColumnas.Add(this.LP.GetText("lblListaCampoNIF", "NIF"));
            nombreColumnas.Add("Año-Mes Cerrado IVA");
            nombreColumnas.Add("Agencia");

            if (this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel != null) this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel.Clear();
            this.tgTexBoxSelCiaFiscal.ColumnasCaptionFormSel = nombreColumnas;
        }

        /// <summary>
        /// Construir el control de seleccion de compañías fiscales
        /// </summary>
        private void BuildtgTexBoxSelCiaFiscal()
        {
            this.tgTexBoxSelCiaFiscal.NumeroCaracteresView = 25;
            this.tgTexBoxSelCiaFiscal.AjustarTamanoTextBox();

            this.tgTexBoxSelCiaFiscal.CantidadColumnasResult = 4;
            this.tgTexBoxSelCiaFiscal.Textbox.MaxLength = 2;

            this.tgTexBoxSelCiaFiscal.ProveedorDatosFormSel = GlobalVar.ConexionCG;

            this.tgTexBoxSelCiaFiscal.QueryFormSel = this.ObtenerQueryListaCompaniasFiscales();

            this.tgTexBoxSelCiaFiscal.FrmPadre = this;
        }

        /// <summary>
        /// Crea el desplegable de Periodos
        /// </summary>
        private void CrearComboPeriodos()
        {
            try
            {
                periodoArray = new ArrayList();
                periodoArray.Add(new AddValue("", ""));

                if (this.tipoPeriodoImpositivo == "T")
                {
                    periodoArray.Add(new AddValue("1T", "1T"));
                    periodoArray.Add(new AddValue("2T", "2T"));
                    periodoArray.Add(new AddValue("3T", "3T"));
                    periodoArray.Add(new AddValue("4T", "4T"));
                }
                else
                {
                    periodoArray.Add(new AddValue("01", "01"));
                    periodoArray.Add(new AddValue("02", "02"));
                    periodoArray.Add(new AddValue("03", "03"));
                    periodoArray.Add(new AddValue("04", "04"));
                    periodoArray.Add(new AddValue("05", "05"));
                    periodoArray.Add(new AddValue("06", "06"));
                    periodoArray.Add(new AddValue("07", "07"));
                    periodoArray.Add(new AddValue("08", "08"));
                    periodoArray.Add(new AddValue("09", "09"));
                    periodoArray.Add(new AddValue("10", "10"));
                    periodoArray.Add(new AddValue("11", "11"));
                    periodoArray.Add(new AddValue("12", "12"));
                }

                this.cmbPeriodo.DataSource = periodoArray;
                this.cmbPeriodo.DisplayMember = "Display";
                this.cmbPeriodo.ValueMember = "Value";

                this.cmbPeriodo.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea el desplegable de Factura Modifica
        /// </summary>
        private void CrearComboEstadoCuadre()
        {
            try
            {
                estadoCuadreArray = new ArrayList();
                estadoCuadreArray.Add(new AddValue("", ""));
                estadoCuadreArray.Add(new AddValue("No contrastable", "1"));
                estadoCuadreArray.Add(new AddValue("En proceso de contraste", "2"));
                estadoCuadreArray.Add(new AddValue("No contrastada", "3"));
                estadoCuadreArray.Add(new AddValue("Parcialmente contrastada", "4"));
                estadoCuadreArray.Add(new AddValue("Contrastada", "5"));

                this.cmbEstadoCuadre.DataSource = estadoCuadreArray;
                this.cmbEstadoCuadre.DisplayMember = "Display";
                this.cmbEstadoCuadre.ValueMember = "Value";

                this.cmbEstadoCuadre.SelectedIndex = 0;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea la Grid con el resultado de la consulta
        /// </summary>
        private void BuiltgConsultaInfo()
        {
            //Crear el DataGrid
            this.tgGridConsulta.dsDatos = new DataSet();
            this.tgGridConsulta.dsDatos.DataSetName = "Consulta";
            this.tgGridConsulta.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridConsulta.ReadOnly = true;
            this.tgGridConsulta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridConsulta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridConsulta.AllowUserToAddRows = false;
            this.tgGridConsulta.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridConsulta.NombreTabla = "Consulta";
        }

        /// <summary>
        /// Validar el formulario 
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            try
            {
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() == "")
                {
                    MessageBox.Show("Es obligatorio informar la compañía", "Error");   //Falta traducir
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    return (false);
                }
                else
                {
                    this.codigoCompania = this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim();
                    string result = this.ValidarCompania();
                    if (result != "")
                    {
                        MessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
                        return (false);
                    }
                }

                if (this.txtEjercicio.Text == "")
                {
                    MessageBox.Show("Es obligatorio informar el ejercicio", "Error");   //Falta traducir
                    this.txtEjercicio.Focus();
                    return (false);
                }

                if (this.txtEjercicio.Text.Length == 1 || this.txtEjercicio.Text.Length == 3)
                {
                    MessageBox.Show("El ejercicio no tiene un formato válido (aa o aaaa)", "Error");   //Falta traducir
                    this.txtEjercicio.Focus();
                    return (false);
                }

                string ejercicio = this.txtEjercicio.Text;
                if (this.txtEjercicio.Text.Length == 2)
                {
                    //Completar el ejercicio a formato aaaa
                    ejercicio = "20" + ejercicio;
                }

                //El ejercicio no puede ser mayor que el año actual
                DateTime fechaActual = DateTime.Now;
                int ejercicioInt = Convert.ToInt16(ejercicio);
                if (ejercicioInt > fechaActual.Date.Year)
                {
                    MessageBox.Show("El ejercicio no puede ser mayor que el año en curso", "Error");   //Falta traducir
                    this.txtEjercicio.Focus();
                    return (false);
                }
                else
                {
                    this.txtEjercicio.Text = ejercicio;
                    if (ejercicio.Length == 4) this.ejercicioCG = ejercicio.Substring(2, 2);
                }

                //Periodo
                if (this.cmbPeriodo.SelectedIndex == 0)
                {
                    MessageBox.Show("Debe indicar el periodo", "Error");   //Falta traducir
                    this.cmbPeriodo.Focus();
                    return (false);
                }

                if (this.txtNIF.Text == "" && this.txtNombreRazon.Text != "")
                {
                    MessageBox.Show("Es obligatorio informar el NIF", "Error");   //Falta traducir
                    this.txtNIF.Focus();
                    return (false);
                }
                else
                {
                    if (this.txtNombreRazon.Text == "" && this.txtNIF.Text != "")
                    {
                        MessageBox.Show("Es obligatorio informar el Nombre o Razón Social", "Error");   //Falta traducir
                        this.txtNombreRazon.Focus();
                        return (false);
                    }
                }

                //Verificar que esté informada y sea correcta la url del servicio web del sii
                if (this.serviceSII.URL == null || this.serviceSII.URL == "")
                {
                    MessageBox.Show("La dirección del servicio web que comunica con el SII no está informada. Por favor contacte con el administrador del sistema", "Error");   //Falta traducir
                    return (false);
                }
                if (!LibroUtiles.IsReachableUri(this.serviceSII.URL))
                {
                    MessageBox.Show("La dirección del servicio web que comunica con el SII no es correcta. Por favor contacte con el administrador del sistema", "Error");   //Falta traducir
                    return (false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (true);
        }

        /// <summary>
        /// Valida la compañia del formulario (valor compañía que exista en la variable this.codigoCompania)
        /// </summary>
        /// <returns></returns>
        private string ValidarCompania()
        {
            string result = "";
            try
            {
                if (this.codigoCompania == "")
                {
                    result = "La compañia no puede estar en blanco";
                    return (result);
                }

                if (this.codigoCompania.Length >= 2)
                {
                    if (this.codigoCompania.Length > 2) this.codigoCompania = this.codigoCompania.Substring(0, 2);

                    string companiaDesc = "";
                    string companiaNIF = "";
                    result = this.ValidarCompaniaFiscal(this.codigoCompania, ref companiaDesc, ref companiaNIF, ref this.tipoPeriodoImpositivo);

                    if (result != "")
                    {
                        //string error = this.LP.GetText("errValTitulo", "Error");
                        //MessageBox.Show(result, error);
                        this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
                        this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    }
                    else
                    {
                        string codigoComp = this.codigoCompania;
                        if (companiaDesc != "") codigoComp += " - " + companiaDesc;
                        if (companiaNIF != "") codigoComp += " - " + companiaNIF;

                        this.tgTexBoxSelCiaFiscal.Textbox.Text = codigoComp;
                    }
                }
                else
                {
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                    result = "La compañía no es válida";
                    //MessageBox.Show("La compañía no es válida", "Error");   //Falta traducir
                    //this.tgTexBoxSelCiaFiscal.Textbox.Text = "";
                    this.tgTexBoxSelCiaFiscal.Textbox.Focus();
                }
            }
            catch (Exception ex)
            {
                result = "Error validando la compañía";
                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (result);
        }

        /// <summary>
        /// Llama a la consulta de facturas por proveedor solicitada
        /// </summary>
        private void Consultar()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.tgGridConsulta.Visible = false;
                this.gbResultado.Visible = false;
                this.lblNoInfo.Visible = false;

                if (this.FormValid())
                {
                    this.lblInfo.Visible = true;
                    this.lblInfo.Update();

                    //Eliminar los resultados de la búsqueda anteriorde la grid
                    if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                    {
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("NumeroFacturas")) this.tgGridConsulta.dsDatos.Tables.Remove("DatosGenerales");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resultado")) this.tgGridConsulta.dsDatos.Tables.Remove("Resultado");
                        if (this.tgGridConsulta.dsDatos.Tables.Contains("Resumen")) this.tgGridConsulta.dsDatos.Tables.Remove("Resumen");
                    }

                    //Eliminar todas las tablas del dataset
                    if (this.dsConsultaRespuesta != null && this.dsConsultaRespuesta.Tables != null && this.dsConsultaRespuesta.Tables.Count > 0)
                    {
                        this.dsConsultaRespuesta.Tables.Clear();
                        this.dsConsultaRespuesta.Clear();
                    }

                    string periodo = this.cmbPeriodo.SelectedValue.ToString();

                    this.dsConsultaRespuesta = this.ConsultaInformacionFacturasAgrupadaProveedor(this.codigoCompania, this.ejercicioCG, periodo);
                    
                    //Grabar la petición
                    string valores = this.ValoresPeticion();

                    this.valoresFormulario.GrabarParametros(formCode, valores);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;

            this.lblInfo.Visible = false;
            this.lblInfo.Update();
        }

        /// <summary>
        /// Invoca la consulta de las facturas emitidas enviadas a la AEAT agrupadas por Proveedor
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ejercicio"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        private DataSet ConsultaInformacionFacturasAgrupadaProveedor(string compania, string ejercicio, string periodo)
        {
            this.dsConsultaRespuesta = null;
            IDataReader dr = null;
            try
            {
                string estadoCuadre = "";
                if (this.cmbEstadoCuadre != null && this.cmbEstadoCuadre.SelectedValue != null) estadoCuadre = this.cmbEstadoCuadre.SelectedValue.ToString();

                this.dsConsultaRespuesta = this.serviceSII.WSTGsii.ConsultaLRFactInformadasAgrupadasProveedor(this.codigoCompania, this.ejercicioCG, periodo,
                                                                              this.txtNIF.Text, this.txtNombreRazon.Text, 
                                                                              estadoCuadre);
                
                if (this.dsConsultaRespuesta.Tables.Count > 0)
                {
                    //Si sólo existe la tabla de Errores, mostrarla
                    if (this.SoloExisteTablaErrores())
                    {
                        //Visualizar el error
                        this.ConsultaVerError();
                        return (this.dsConsultaRespuesta);
                    }

                    if (this.dsConsultaRespuesta.Tables["NumeroFacturas"].Rows.Count > 0)
                    {
                        //Existen facturas que cumplen el criterio de seleccion

                        //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                        this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["NumeroFacturas"].Copy());

                        //BindingSource SBind = new BindingSource();
                        //SBind.DataSource = this.dsConsultaRespuesta.Tables["DatosGenerales"];
                        this.tgGridConsulta.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["NumeroFacturas"];
                        //this.tgGridConsulta.dsDatos.DataSource = SBind;

                        /*
                        foreach (var column in this.tgGridConsulta.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                                (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                        }
                        */

                        this.tgGridConsulta.Refresh();

                        this.tgGridConsulta.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;

                        //Visualizar resumen
                        if (this.dsConsultaRespuesta.Tables["Resumen"].Rows.Count > 0)
                        {
                            this.lblTotalRegValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["NoReg"].ToString();
                            this.lblTotalNumFacturasValor.Text = this.dsConsultaRespuesta.Tables["Resumen"].Rows[0]["TotalFactura"].ToString();
                            this.gbResultado.Visible = true;
                        }
                        else this.gbResultado.Visible = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.tgGridConsulta.Visible = false;
                        this.lblNoInfo.Text = "No existen facturas agrupadas por proveedor que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.gbResultado.Visible = false;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridConsulta.Visible = false;
                    this.lblNoInfo.Text = "No existen facturas agrupadas por proveedor que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.gbResultado.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                MessageBox.Show(ex.Message, "Error");
            }

            return (this.dsConsultaRespuesta);
        }

        /// <summary>
        /// Grabar la petición
        /// </summary>
        private void GrabarPeticion()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                TGPeticionGrabar frmGrabarPeticion = new TGPeticionGrabar();
                frmGrabarPeticion.FormCode = this.formCode;
                frmGrabarPeticion.FrmPadre = this;
                frmGrabarPeticion.FicheroExtension = this.ficheroExtension;
                frmGrabarPeticion.ShowDialog();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Cargar el listado de las peticiones
        /// </summary>
        private void CargarPeticiones()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                FormularioPeticion frmPeticion = new FormularioPeticion();
                frmPeticion.Path = ConfigurationManager.AppSettings["PathFicherosPeticiones"];
                frmPeticion.FormCode = this.formCode;
                frmPeticion.FicheroExtension = this.ficheroExtension;
                frmPeticion.Formulario = this;

                DataTable dtPeticiones = frmPeticion.ListarPeticion();

                if (dtPeticiones.Rows.Count > 0)
                {
                    Dictionary<string, string> dictControles = new Dictionary<string, string>();
                    dictControles.Add("Compañía", "tgTexBoxSelCiaFiscal");
                    dictControles.Add("Ejercicio", "txtEjercicio");
                    dictControles.Add("Periodo", "cmbPeriodo");
                    dictControles.Add("Número Serie Factura", "txtNumSerieFactura");
                    dictControles.Add("NIF", "txtNIF");
                    dictControles.Add("Nombre o Razón Social", "txtNombreRazon");
                    dictControles.Add("Fecha Expedición Desde", "txtMaskFechaExpedicionDesde");
                    dictControles.Add("Fecha Expedición Hasta", "txtMaskFechaExpedicionHasta");
                    dictControles.Add("Fecha Operación Desde", "txtMaskFechaOperacionDesde");
                    dictControles.Add("Fecha Operación Hasta", "txtMaskFechaOperacionHasta");
                    
                    List<string> columnNoVisible = new List<string>(new string[] { "txtElemento"});

                    TGPeticionesListar frmListarPeticiones = new TGPeticionesListar();
                    frmListarPeticiones.DtPeticiones = dtPeticiones;
                    frmListarPeticiones.CentrarForm = true;
                    frmListarPeticiones.Headers = dictControles;
                    frmListarPeticiones.ColumnNoVisible = columnNoVisible;
                    frmListarPeticiones.FrmPadre = this;
                    frmListarPeticiones.OkForm += new TGPeticionesListar.OkFormCommandEventHandler(frmListarPeticiones_OkForm);
                    frmListarPeticiones.Show();
                }
                else
                {
                    MessageBox.Show("No existen peticiones guardadas");    //Falta traducir
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        private void frmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion();
            frmPeticion.FormCode = this.formCode;
            frmPeticion.FicheroExtension = this.ficheroExtension;
            frmPeticion.Formulario = this;
            string result = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));

            this.tgGridConsulta.Visible = false;
        }

        #region Valores Petición Formulario
        /// <summary>
        /// Carga los valores de la última petición del formulario
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MISIICARO myStruct = (StructGLL01_MISIICARO)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MISIICARO));

                if (myStruct.compania.Trim() != "")
                {
                    this.codigoCompania = myStruct.compania.Trim();
                    this.tgTexBoxSelCiaFiscal.Textbox.Modified = false;

                    string resultValComp = this.ValidarCompania();
                    if (resultValComp != "") MessageBox.Show(resultValComp, this.LP.GetText("errValTitulo", "Error"));
                    else
                    {
                        //Crear el desplegable de Periodos
                        this.CrearComboPeriodos();
                    }
                }
                else this.tgTexBoxSelCiaFiscal.Textbox.Text = "";

                if (myStruct.ejercicio.Trim() != "") this.txtEjercicio.Text = myStruct.ejercicio.Trim();
                else this.txtEjercicio.Text = "";

                try
                {
                    if (myStruct.periodo.Trim() != "") this.cmbPeriodo.SelectedValue = myStruct.periodo.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.nif.Trim() != "") this.txtNIF.Text = myStruct.nif.Trim();
                else this.txtNIF.Text = "";

                if (myStruct.nombreRazonSocial.Trim() != "") this.txtNombreRazon.Text = myStruct.nombreRazonSocial.Trim();
                else this.txtNombreRazon.Text = "";

                try
                {
                    if (myStruct.estadoCuadre.Trim() != "") this.cmbEstadoCuadre.SelectedValue = myStruct.estadoCuadre.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                result = true;

                Marshal.FreeBSTR(pBuf);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve una  cadena con todos los valores del formulario para grabar en la tabla de peticiones GLL01
        /// </summary>
        /// <returns></returns>
        private string ValoresPeticion()
        {
            string result = "";
            try
            {
                StructGLL01_MISIICARO myStruct;

                string codigo = "";
                if (this.tgTexBoxSelCiaFiscal.Textbox.Text.Trim() != "")
                {
                    myStruct.compania = codigoCompania.PadRight(2, ' ');
                }
                else myStruct.compania = codigo.PadRight(2, ' ');

                myStruct.ejercicio = this.txtEjercicio.Text.PadRight(4, ' ');

                string cadenaVacia = "";
                if (this.cmbPeriodo.SelectedValue != null) myStruct.periodo = this.cmbPeriodo.SelectedValue.ToString().PadRight(2, ' ');
                else myStruct.periodo = cadenaVacia.PadRight(2, ' ');

                myStruct.nif = this.txtNIF.Text.PadRight(20, ' ');

                myStruct.nombreRazonSocial = this.txtNombreRazon.Text.PadRight(40, ' ');

                if (this.cmbEstadoCuadre.Visible)
                {
                    if (this.cmbEstadoCuadre.SelectedValue != null) myStruct.estadoCuadre = this.cmbEstadoCuadre.SelectedValue.ToString().PadRight(1, ' ');
                    else myStruct.estadoCuadre = cadenaVacia.PadRight(1, ' ');
                }
                else myStruct.estadoCuadre = cadenaVacia.PadRight(1, ' ');

                result = myStruct.compania + myStruct.ejercicio + myStruct.periodo + myStruct.nif;
                result += myStruct.nombreRazonSocial + myStruct.estadoCuadre;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

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
                if (this.tgGridConsulta.dsDatos.Tables.Count > 0)
                {
                    //Eliminar los resultados de la búsqueda anterior o los datos generales
                    this.tgGridConsulta.Visible = false;

                    if (this.tgGridConsulta.dsDatos.Tables.Contains("NumeroFacturas")) this.tgGridConsulta.dsDatos.Tables.Remove("NumeroFacturas");
                    if (this.tgGridConsulta.dsDatos.Tables.Contains("Resultado")) this.tgGridConsulta.dsDatos.Tables.Remove("Resultado");
                }

                //Adicionar el DataTable DatosGenerales al DataSet del DataGrid
                this.tgGridConsulta.dsDatos.Tables.Add(this.dsConsultaRespuesta.Tables["Resultado"].Copy());

                //Eliminar columnas
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Compania")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Compania");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Libro")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Libro");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Operacion")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("Operacion");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NIFIdEmisor")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NIFIdEmisor");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NoFactura")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NoFactura");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("FechaDoc")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("FechaDoc");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("RowResumen")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("RowResumen");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NombreRazonSocial")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NombreRazonSocial");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("ClaveOperacion")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("ClaveOperacion");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("NIF")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("NIF");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroCodPais")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroCodPais");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroTipo")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroTipo");
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("IdOtroId")) this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Remove("IdOtroId");

                this.tgGridConsulta.AutoGenerateColumns = true;

                //Poner como DataSource del DataGrid el DataTable creado
                this.tgGridConsulta.DataSource = this.tgGridConsulta.dsDatos.Tables["Resultado"];

                //Cambiar los headers de las columnas del DataGrid de Error
                if (this.tgGridConsulta.dsDatos.Tables["Resultado"].Columns.Contains("Estado")) this.tgGridConsulta.CambiarColumnHeader("Estado", "Error");  //Falta traducir

                this.tgGridConsulta.Refresh();

                this.tgGridConsulta.Visible = true;
                this.lblNoInfo.Visible = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion            

        private void tgGridConsulta_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridConsulta.Rows.Count > 0)
            {
                //Ocultar las columnas necesarias

                this.tgGridConsulta.Rows[0].Selected = false;

                if (this.tgGridConsulta.dsDatos.Tables.Contains("NumeroFacturas"))
                {
                    if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ProveedorNIF")) this.tgGridConsulta.CambiarColumnHeader("ProveedorNIF", "NIF Proveedor");  //Falta traducir
                    if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("ProveedorNombreRazonSocial")) this.tgGridConsulta.CambiarColumnHeader("ProveedorNombreRazonSocial", "Nombre o Razón Social Proveedor");  //Falta traducir
                    if (this.tgGridConsulta.dsDatos.Tables[0].Columns.Contains("NumeroFacturas")) this.tgGridConsulta.CambiarColumnHeader("NumeroFacturas", "Número de Facturas");  //Falta traducir   
                }
            }
        }

        /// <summary>
        /// Exporta la consulta de Datos en Presentados a la AEAT a Excel, pasando por un fichero HTML
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string titulo = "Consulta de Facturas Informadas Agrupadas por Proveedor";
                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridConsulta.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridConsulta.Columns[i].HeaderText;  //Nombre de la columna

                    switch (this.tgGridConsulta.Columns[i].Name)
                    {
                        case "NumeroFacturas":
                            nombreTipoVisible[1] = "numero";
                            nombreTipoVisible[2] = this.tgGridConsulta.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = this.tgGridConsulta.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridConsulta);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIIConsultaAgrupadaProveedor");

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "EXCEL";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
                catch // si no puede levantar excel, levantar html
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "IEXPLORE";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));
            }

            Cursor.Current = Cursors.Default;
        }
    }
}