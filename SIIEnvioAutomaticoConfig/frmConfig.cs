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

namespace SIIEnvioAutomaticoConfig
{
    public partial class frmConfig : Form
    {
        protected Utiles utiles;

        public Configuration configSIIAuto;

        public string ficheroConfiguracionEnvioAutoNombre = "";
        public const string nombreDefectoFiechoConfiguracionEnvioAutomatico = "SIIAuto.exe.config";

        public frmConfig()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmConfig_Load(object sender, EventArgs e)
        {
            //Centrar formulario respecto a la pantalla completa
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Top = (rect.Height / 2) - (this.Height / 2);
            this.Left = (rect.Width / 2) - (this.Width / 2);

            utiles = new Utiles();

            //Crear el desplegable de Libros
            this.CrearComboLibros();

            //Crear el desplegable de Operaciones
            this.CrearComboOperaciones();

            //Crear el desplegable de Periodos
            this.CrearComboPeriodos();

            this.CargarValoresConfig();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonGrabar_Click(object sender, EventArgs e)
        {
            this.Grabar();
        }

        private void btnAddEjercicio_Click(object sender, EventArgs e)
        {
            string ejercicio = this.txtEjercicio.Text.Trim();
            if (ejercicio != "" && ejercicio.Length == 2)
            {

                string result = this.AddToListBox(ejercicio, ref this.lbEjercicios);
                switch (result)
                {
                    case "":
                        this.txtEjercicio.Text = "";
                        this.txtEjercicio.Focus();
                        this.btnAddEjercicio.Enabled = false;
                        break;
                    case "1":
                        MessageBox.Show("El ejercicio ya está en la lista");    //Falta traducir
                        this.txtCompania.Focus();
                        break;
                    case "2":
                        MessageBox.Show("Debe seleccionar un ejercicio");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                }
            }
            else
            {
                MessageBox.Show("El ejercicio no tiene un formato correcto");    //Falta traducir
                this.txtCompania.Focus();
            }
        }

        private void btnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            this.lbEjercicios.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbEjercicios.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbEjercicios.Items.Remove(item);
            }
            this.lbEjercicios.EndUpdate();

            this.txtEjercicio.Select();

            this.btnQuitarEjercicio.Enabled = false;
        }

        private void btnSelRutaFicheroRespuestaEnvio_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath(this.txtRutaFicheroRespuestaEnvio, this.folderBrowserDialogRespuesta);
        }

        private void btnSelRutaFicheroLog_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath(this.txtRutaFicheroLog, this.folderBrowserDialogLog);
        }

        private void chkActivarRespuestaEnvio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkActivarLog_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtEjercicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Valida que sólo se introduzcan números
            utiles.ValidarNumeroKeyPress(ref this.txtEjercicio, ref sender, ref e, false);
            if (this.txtEjercicio.Text.Trim() != "") this.btnAddEjercicio.Enabled = true;
        }

        private void txtNumeroIntentos_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroKeyPress(ref this.txtNumeroIntentos, ref sender, ref e, false);
        }

        private void txtTiempoEspera_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroKeyPress(ref this.txtTiempoEspera, ref sender, ref e, false);
        }

        #region Compañías Fiscales
        private void txtCompania_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
            if (this.txtCompania.Text.Trim() != "") this.btnAddCiaFiscal.Enabled = true;
        }

        private void txtCompania_Leave(object sender, EventArgs e)
        {
            string cia = this.txtCompania.Text.Trim();
            if (cia != "" && cia.Length == 2) this.btnAddCiaFiscal.Enabled = true;
        }

        private void btnAddCiaFiscal_Click(object sender, EventArgs e)
        {
            string cia = this.txtCompania.Text.Trim();
            if (cia != "" && cia.Length == 2)
            {

                string result = this.AddToListBox(cia, ref this.lbCiasFiscales);
                switch (result)
                {
                    case "":
                        this.txtCompania.Text = "";
                        this.txtCompania.Focus();
                        this.btnAddCiaFiscal.Enabled = false;
                        break;
                    case "1":
                        MessageBox.Show("La compañía fiscal ya está en la lista");    //Falta traducir
                        this.txtCompania.Focus();
                        break;
                    case "2":
                        MessageBox.Show("Debe seleccionar una compañía fiscal");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                }
            }
            else
            {
                MessageBox.Show("El código de compañía fiscal no tiene un formato correcto");    //Falta traducir
                this.txtCompania.Focus();
            }
        }

        private void btnQuitarCiaFiscal_Click(object sender, EventArgs e)
        {
            this.lbCiasFiscales.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbCiasFiscales.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbCiasFiscales.Items.Remove(item);
            }
            this.lbCiasFiscales.EndUpdate();

            this.txtCompania.Select();

            this.btnQuitarCiaFiscal.Enabled = false;
        }

        private void lbCiasFiscales_Enter(object sender, EventArgs e)
        {
            if (this.lbCiasFiscales.Items.Count > 0) this.btnQuitarCiaFiscal.Enabled = true;
        }

        private void lbEjercicios_Enter(object sender, EventArgs e)
        {
            if (this.lbEjercicios.Items.Count > 0) this.btnQuitarEjercicio.Enabled = true;
        }

        private void txtEjercicio_Leave(object sender, EventArgs e)
        {
            string ejercicio = this.txtEjercicio.Text.Trim();
            if (ejercicio != "" && ejercicio.Length == 2) this.btnAddEjercicio.Enabled = true;
        }
        #endregion

        #region Libros
        private void btnAddLibro_Click(object sender, EventArgs e)
        {
            string value;
            string[] aValue;

            if (this.chkLibrosTodos.Checked)
            {
                //Insertar todos los libros
                for (int i = 0; i < this.cmbLibro.Items.Count; i++)
                {
                    value = this.cmbLibro.GetItemText(this.cmbLibro.Items[i]);
                    aValue = value.Split('-');
                    this.AddToListBox(aValue[0].Trim(), ref this.lbLibros);
                }
            }
            else
            {
                value = this.cmbLibro.SelectedValue.ToString();
                aValue = value.Split('-');

                string result = this.AddToListBox(aValue[0].Trim(), ref this.lbLibros);
                switch (result)
                {
                    case "":
                        this.cmbLibro.Focus();
                        break;
                    case "1":
                        MessageBox.Show("El libro ya está en la lista");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                    case "2":
                        MessageBox.Show("Debe seleccionar un libro");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                }
            }
        }

        private void lbLibros_Enter(object sender, EventArgs e)
        {
            if (this.lbLibros.Items.Count > 0) this.btnQuitarLibro.Enabled = true;
        }

        private void btnQuitarLibro_Click(object sender, EventArgs e)
        {
            this.lbLibros.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbLibros.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbLibros.Items.Remove(item);
            }
            this.lbLibros.EndUpdate();

            this.cmbLibro.Select();

            this.btnQuitarLibro.Enabled = false;
        }
        #endregion

        #region Operaciones
        private void btnAddOperacion_Click(object sender, EventArgs e)
        {
            string value;
            string[] aValue;

            if (this.chkOperacionesTodas.Checked)
            {
                //Insertar todas las operaciones 
                for (int i = 0; i < this.cmbOperacion.Items.Count; i++)
                {
                    value = this.cmbOperacion.GetItemText(this.cmbOperacion.Items[i]);
                    aValue = value.Split('-');
                    this.AddToListBox(aValue[0].Trim(), ref this.lbOperaciones);
                }
            }
            else
            {
                value = this.cmbOperacion.SelectedValue.ToString();
                aValue = value.Split('-');

                string result = this.AddToListBox(aValue[0].Trim(), ref this.lbOperaciones);
                switch (result)
                {
                    case "":
                        this.cmbOperacion.Focus();
                        break;
                    case "1":
                        MessageBox.Show("La operación ya está en la lista");    //Falta traducir
                        this.cmbOperacion.Focus();
                        break;
                    case "2":
                        MessageBox.Show("Debe seleccionar una operación");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                }
            }
        }

        private void lbOperaciones_Enter(object sender, EventArgs e)
        {
            if (this.lbOperaciones.Items.Count > 0) this.btnQuitarOperacion.Enabled = true;
        }

        private void btnQuitarOperacion_Click(object sender, EventArgs e)
        {
            this.lbOperaciones.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbOperaciones.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbOperaciones.Items.Remove(item);
            }
            this.lbOperaciones.EndUpdate();

            this.cmbOperacion.Select();

            this.btnQuitarOperacion.Enabled = false;
        }
        #endregion

        #region Periodos
        private void btnAddPeriodo_Click(object sender, EventArgs e)
        {
            string value;
            string[] aValue;

            if (this.chkPeriodosTodos.Checked)
            {
                //Insertar todos las periodos 
                for (int i = 0; i < this.cmbPeriodo.Items.Count; i++)
                {
                    value = this.cmbPeriodo.GetItemText(this.cmbPeriodo.Items[i]);
                    aValue = value.Split('-');
                    this.AddToListBox(aValue[0].Trim(), ref this.lbPeriodos);
                }
            }
            else
            {
                value = this.cmbPeriodo.SelectedValue.ToString();
                aValue = value.Split('-');

                string result = this.AddToListBox(aValue[0].Trim(), ref this.lbPeriodos);
                switch (result)
                {
                    case "":
                        this.cmbPeriodo.Focus();
                        break;
                    case "1":
                        MessageBox.Show("El periodo ya está en la lista");    //Falta traducir
                        this.cmbPeriodo.Focus();
                        break;
                    case "2":
                        MessageBox.Show("Debe seleccionar un periodo");    //Falta traducir
                        this.cmbLibro.Focus();
                        break;
                }
            }
        }

        private void lbPeriodos_Enter(object sender, EventArgs e)
        {
            if (this.lbPeriodos.Items.Count > 0) this.btnQuitarPeriodo.Enabled = true;
        }

        private void btnQuitarPeriodo_Click(object sender, EventArgs e)
        {
            this.lbPeriodos.BeginUpdate();
            ArrayList vSelectedItems = new ArrayList(this.lbPeriodos.SelectedItems);
            foreach (string item in vSelectedItems)
            {
                this.lbPeriodos.Items.Remove(item);
            }
            this.lbPeriodos.EndUpdate();

            this.cmbPeriodo.Select();

            this.btnQuitarPeriodo.Enabled = false;
        }
        #endregion
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Crea el desplegable de Libros
        /// </summary>
        private void CrearComboLibros()
        {
            try
            {
                string LibroID_FacturasEmitidas = "01";
                string LibroID_FacturasRecibidas = "03";
                string LibroID_BienesInversion = "05";
                string LibroID_CobrosMetalico = "07";
                string LibroID_OperacionesIntracomunitarias = "09";
                //string LibroID_CobrosEmitidas = "11";
                string LibroID_PagosRecibidas = "13";
                string LibroID_OperacionesSeguros = "15";
                string LibroID_AgenciasViajes = "17";

                ArrayList librosArray = new ArrayList();
                librosArray.Add(new AddValue("", ""));
                librosArray.Add(new AddValue("01 - Facturas Emitidas", LibroID_FacturasEmitidas));       //Facturas Emitidas
                librosArray.Add(new AddValue("03 - Facturas Recibidas", LibroID_FacturasRecibidas));      //Facturas Recibidas
                librosArray.Add(new AddValue("05 - Bienes de inversión", LibroID_BienesInversion));        //Bienes de inversión
                librosArray.Add(new AddValue("07 - Cobros en Metálico", LibroID_CobrosMetalico));         //Cobros en metálico
                librosArray.Add(new AddValue("09 - Operaciones Intracomunitarias", LibroID_OperacionesIntracomunitarias));   //Determinadas Operaciones Intracomunitarias
                librosArray.Add(new AddValue("13 - Pagos Recibidas RECC", LibroID_PagosRecibidas));         //Pagos Recibidas
                librosArray.Add(new AddValue("15 - Operaciones de Seguros", LibroID_OperacionesSeguros));     //Operaciones de seguros
                librosArray.Add(new AddValue("17 - Agencias de Viajes", LibroID_AgenciasViajes));         //Agencias de viajes

                this.cmbLibro.DataSource = librosArray;
                this.cmbLibro.DisplayMember = "Display";
                this.cmbLibro.ValueMember = "Value";

                this.cmbLibro.SelectedIndex = 0;
            }
            catch { }
        }

        /// <summary>
        /// Crea el desplegable de Operaciones
        /// </summary>
        private void CrearComboOperaciones()
        {
            try
            {
                ArrayList operacionesArray = new ArrayList();
                operacionesArray.Add(new AddValue("", ""));
                operacionesArray.Add(new AddValue("A0 - Alta", "A0"));
                operacionesArray.Add(new AddValue("A1 - Modificar", "A1"));
                operacionesArray.Add(new AddValue("A4 - Modificar Reg. Viajeros", "A4"));
                operacionesArray.Add(new AddValue("A5 - Alta devoluciones IVA de Viajeros", "A5"));
                operacionesArray.Add(new AddValue("A6 - Modificación devoluciones IVA de Viajeros", "A6"));
                operacionesArray.Add(new AddValue("B  - Anular", "B"));

                this.cmbOperacion.DataSource = operacionesArray;
                this.cmbOperacion.DisplayMember = "Display";
                this.cmbOperacion.ValueMember = "Value";

                this.cmbOperacion.SelectedIndex = 0;
            }
            catch { }
        }

        /// <summary>
        /// Crea el desplegable de Periodos
        /// </summary>
        private void CrearComboPeriodos()
        {
            try
            {
                ArrayList periodosArray = new ArrayList();
                periodosArray.Add(new AddValue("", ""));
                periodosArray.Add(new AddValue("01 - Enero", "01"));
                periodosArray.Add(new AddValue("02 - Febrero", "02"));
                periodosArray.Add(new AddValue("03 - Marzo", "03"));
                periodosArray.Add(new AddValue("04 - Abril", "04"));
                periodosArray.Add(new AddValue("05 - Mayo", "05"));
                periodosArray.Add(new AddValue("06 - Junio", "06"));
                periodosArray.Add(new AddValue("07 - Julio", "07"));
                periodosArray.Add(new AddValue("08 - Agosto", "08"));
                periodosArray.Add(new AddValue("09 - Setiembre", "09"));
                periodosArray.Add(new AddValue("10 - Octubre", "10"));
                periodosArray.Add(new AddValue("11 - Noviembre", "11"));
                periodosArray.Add(new AddValue("12 - Diciembre", "12"));
                periodosArray.Add(new AddValue("0A - Anual", "0A"));
                periodosArray.Add(new AddValue("1T - 1er Trimestre", "1T"));
                periodosArray.Add(new AddValue("2T - 2do Trimestre", "2T"));
                periodosArray.Add(new AddValue("3T - 3er Trimestre", "3T"));
                periodosArray.Add(new AddValue("4T - 4to Trimestre", "4T"));

                this.cmbPeriodo.DataSource = periodosArray;
                this.cmbPeriodo.DisplayMember = "Display";
                this.cmbPeriodo.ValueMember = "Value";

                this.cmbPeriodo.SelectedIndex = 0;
            }
            catch { }
        }

        /// <summary>
        /// Cargar los valores del config en los controles del formulario
        /// </summary>
        private void CargarValoresConfig()
        {
            try
            {
                //Inicializar con los valores por defecto   
                string urlWS = "";
                string compania = "";
                string logActivo = "0";
                string logPath = "";
                string libro = "";
                string operacion = "";
                string ejercicio = "";
                string periodo = "";
                string resultActivo = "0";
                string resultPath = "";

                string mailActivo = "0";
                string mailActivoAlerta = "0";
                string mailSMTPHost = "";
                string mailSMTPPuerto = "";
                string mailSMTPUsuario = "";
                string mailSMTPPwd = "";
                string mailSMTPSSL = "";
                string mailFrom = "";
                string mailTo = "";
                string mailSubject = "";
                string mailBody = "";

                string numeroIntentosSiError = "";
                string tiempoEsperaSiError = "";

                try
                {
                    //Leer nombre del fichero config que contiene la configuración para el envío automátio
                    this.ficheroConfiguracionEnvioAutoNombre = ConfigurationManager.AppSettings["configEnvioAutomaticoFichero"];
                    if (this.ficheroConfiguracionEnvioAutoNombre == null || this.ficheroConfiguracionEnvioAutoNombre.Trim() == "") this.ficheroConfiguracionEnvioAutoNombre = nombreDefectoFiechoConfiguracionEnvioAutomatico;
                    string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, this.ficheroConfiguracionEnvioAutoNombre);

                    // Leer parámetros de configuración
                    // Get the machine.config file.
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    // You may want to map to your own exe.comfig file here.
                    fileMap.ExeConfigFilename = exePath;
                    this.configSIIAuto = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    if (this.configSIIAuto.AppSettings.Settings["compania"] != null) compania = this.configSIIAuto.AppSettings.Settings["compania"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["logActivo"] != null) logActivo = this.configSIIAuto.AppSettings.Settings["logActivo"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["logPath"] != null) logPath = this.configSIIAuto.AppSettings.Settings["logPath"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["libro"] != null) libro = this.configSIIAuto.AppSettings.Settings["libro"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["operacion"] != null) operacion = this.configSIIAuto.AppSettings.Settings["operacion"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["ejercicio"] != null) ejercicio = this.configSIIAuto.AppSettings.Settings["ejercicio"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["periodo"] != null) periodo = this.configSIIAuto.AppSettings.Settings["periodo"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["resultActivo"] != null) resultActivo = this.configSIIAuto.AppSettings.Settings["resultActivo"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["resultPath"] != null) resultPath = this.configSIIAuto.AppSettings.Settings["resultPath"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailActivo"] != null) mailActivo = this.configSIIAuto.AppSettings.Settings["mailActivo"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailActivoSoloAlerta"] != null) mailActivoAlerta = this.configSIIAuto.AppSettings.Settings["mailActivoSoloAlerta"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["smtpHost"] != null) mailSMTPHost = this.configSIIAuto.AppSettings.Settings["smtpHost"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["smtpPort"] != null) mailSMTPPuerto = this.configSIIAuto.AppSettings.Settings["smtpPort"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["smtpUser"] != null) mailSMTPUsuario = this.configSIIAuto.AppSettings.Settings["smtpUser"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["smtpPwd"] != null) mailSMTPPwd = this.configSIIAuto.AppSettings.Settings["smtpPwd"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["smtpEnableSsl"] != null) mailSMTPSSL = this.configSIIAuto.AppSettings.Settings["smtpEnableSsl"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailFrom"] != null) mailFrom = this.configSIIAuto.AppSettings.Settings["mailFrom"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailTo"] != null) mailTo = this.configSIIAuto.AppSettings.Settings["mailTo"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailSubject"] != null) mailSubject = this.configSIIAuto.AppSettings.Settings["mailSubject"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["mailBody"] != null) mailBody = this.configSIIAuto.AppSettings.Settings["mailBody"].Value;

                    if (this.configSIIAuto.AppSettings.Settings["numeroIntentosSiError"] != null) numeroIntentosSiError = this.configSIIAuto.AppSettings.Settings["numeroIntentosSiError"].Value;
                    if (this.configSIIAuto.AppSettings.Settings["tiempoEsperaSiError"] != null) tiempoEsperaSiError = this.configSIIAuto.AppSettings.Settings["tiempoEsperaSiError"].Value;                    

                    ConfigurationSectionGroup configGroup = this.configSIIAuto.SectionGroups["applicationSettings"];
                    ConfigurationSection configSectionApplicationSettings = configGroup.Sections[0];
                    if (configSectionApplicationSettings != null) urlWS = ((ClientSettingsSection)configSectionApplicationSettings).Settings.Get("SIIEnvioAutomatico_tgSIIWebService_TGsiiService").Value.ValueXml.InnerText;

                }
                catch (Exception ex) { string error = ex.Message; }

                this.txtUrlWebService.Text = urlWS;

                if (resultActivo == "1")
                {
                    this.chkActivarRespuestaEnvio.Checked = true;
                    this.txtRutaFicheroRespuestaEnvio.Text = resultPath;
                }
                else
                {
                    this.chkActivarRespuestaEnvio.Checked = false;
                    this.txtRutaFicheroRespuestaEnvio.Text = "";
                }

                if (logActivo == "1")
                {
                    this.chkActivarLog.Checked = true;
                    this.txtRutaFicheroLog.Text = logPath;
                }
                else
                {
                    this.chkActivarLog.Checked = false;
                    this.txtRutaFicheroLog.Text = "";
                }

                this.txtCompania.Text = "";
                this.ListaElementos(ref this.lbCiasFiscales, compania);

                this.cmbLibro.Text = "";
                this.ListaElementos(ref this.lbLibros, libro);

                this.cmbOperacion.Text = "";
                this.ListaElementos(ref this.lbOperaciones, operacion);

                this.txtEjercicio.Text = "";
                this.ListaElementos(ref this.lbEjercicios, ejercicio);

                this.cmbPeriodo.Text = "";
                this.ListaElementos(ref this.lbPeriodos, periodo);

                if (mailActivo == "1") this.chkMailActivo.Checked = true;
                if (mailActivoAlerta == "1") this.chkMailActivoAlerta.Checked = true;

                this.txtSMTPServidor.Text = mailSMTPHost;
                this.txtSMTPPuerto.Text = mailSMTPPuerto;
                this.txtSMTPUsuario.Text = mailSMTPUsuario;
                this.txtSMTPContrasena.Text = mailSMTPPwd;
                if (mailSMTPSSL == "true") this.chkSMTPEnableSSL.Checked = true;
                this.txtMailFrom.Text = mailFrom;
                this.txtMailTo.Text = mailTo;
                this.txtMailSubject.Text = mailSubject;
                this.txtMailBody.Text = mailBody;

                this.txtNumeroIntentos.Text = numeroIntentosSiError;
                this.txtTiempoEspera.Text = tiempoEsperaSiError;
            }
            catch { }
        }

        /// <summary>
        /// Rellena los elementos de la lista con los valores que están separados po
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="valores"></param>
        private void ListaElementos(ref ListBox lista, string valores)
        {
            try
            {
                lista.Items.Clear();
                if (valores != "")
                {
                    string[] aValores = valores.Split(',');
                    for (int i = 0; i < aValores.Length; i++)
                    {
                        this.AddToListBox(aValores[i], ref lista);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Adiciona un elemento a una lista si no existe
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="controlListBox"></param>
        /// <returns></returns>
        public string AddToListBox(string texto, ref ListBox controlListBox)
        {
            string result = "";

            if (texto.Trim() != "")
            {
                for (int i = 0; i < controlListBox.Items.Count; i++)
                {
                    if (controlListBox.Items[i].ToString() == texto)
                    {
                        result = "1";   //El texto ya existe en la lista
                        break;
                    }
                }   
            }
            else result = "2";

            if (result == "")
            {
                controlListBox.Items.Add(texto);
            }

            return (result);
        }

        //Graba los parámetros en el fichero de configuración del Envio Automático de facturas al SII
        private void Grabar()
        {
            if (this.isFormValid())
            {
                string separador = ",";

                if (this.configSIIAuto.AppSettings.Settings["compania"] != null) this.configSIIAuto.AppSettings.Settings["compania"].Value = this.DevolverListaValores(this.lbCiasFiscales, separador);
                if (this.configSIIAuto.AppSettings.Settings["logActivo"] != null) this.configSIIAuto.AppSettings.Settings["logActivo"].Value = this.chkActivarLog.Checked ? "1" : "0";
                if (this.configSIIAuto.AppSettings.Settings["logPath"] != null) this.configSIIAuto.AppSettings.Settings["logPath"].Value = this.txtRutaFicheroLog.Text;
                if (this.configSIIAuto.AppSettings.Settings["libro"] != null) this.configSIIAuto.AppSettings.Settings["libro"].Value = this.DevolverListaValores(this.lbLibros, separador);
                if (this.configSIIAuto.AppSettings.Settings["operacion"] != null) this.configSIIAuto.AppSettings.Settings["operacion"].Value = this.DevolverListaValores(this.lbOperaciones, separador);
                if (this.configSIIAuto.AppSettings.Settings["ejercicio"] != null) this.configSIIAuto.AppSettings.Settings["ejercicio"].Value = this.DevolverListaValores(this.lbEjercicios, separador);
                if (this.configSIIAuto.AppSettings.Settings["periodo"] != null) this.configSIIAuto.AppSettings.Settings["periodo"].Value = this.DevolverListaValores(this.lbPeriodos, separador);
                if (this.configSIIAuto.AppSettings.Settings["resultActivo"] != null) this.configSIIAuto.AppSettings.Settings["resultActivo"].Value = this.chkActivarRespuestaEnvio.Checked ? "1" : "0";
                if (this.configSIIAuto.AppSettings.Settings["resultPath"] != null) this.configSIIAuto.AppSettings.Settings["resultPath"].Value = this.txtRutaFicheroRespuestaEnvio.Text;
                if (this.configSIIAuto.AppSettings.Settings["mailActivo"] != null) this.configSIIAuto.AppSettings.Settings["mailActivo"].Value = this.chkMailActivo.Checked ? "1" : "0";
                if (this.configSIIAuto.AppSettings.Settings["mailActivoSoloAlerta"] != null) this.configSIIAuto.AppSettings.Settings["mailActivoSoloAlerta"].Value = this.chkMailActivoAlerta.Checked ? "1" : "0";
                if (this.configSIIAuto.AppSettings.Settings["smtpHost"] != null) this.configSIIAuto.AppSettings.Settings["smtpHost"].Value = this.txtSMTPServidor.Text;
                if (this.configSIIAuto.AppSettings.Settings["smtpPort"] != null) this.configSIIAuto.AppSettings.Settings["smtpPort"].Value = this.txtSMTPPuerto.Text;
                if (this.configSIIAuto.AppSettings.Settings["smtpUser"] != null) this.configSIIAuto.AppSettings.Settings["smtpUser"].Value = this.txtSMTPUsuario.Text;
                if (this.configSIIAuto.AppSettings.Settings["smtpPwd"] != null) this.configSIIAuto.AppSettings.Settings["smtpPwd"].Value = this.txtSMTPContrasena.Text;
                if (this.configSIIAuto.AppSettings.Settings["smtpEnableSsl"] != null) this.configSIIAuto.AppSettings.Settings["smtpEnableSsl"].Value = this.chkSMTPEnableSSL.Checked ? "true" : "false";
                if (this.configSIIAuto.AppSettings.Settings["mailFrom"] != null) this.configSIIAuto.AppSettings.Settings["mailFrom"].Value = this.txtMailFrom.Text;
                if (this.configSIIAuto.AppSettings.Settings["mailTo"] != null) this.configSIIAuto.AppSettings.Settings["mailTo"].Value = this.txtMailTo.Text;
                if (this.configSIIAuto.AppSettings.Settings["mailSubject"] != null) this.configSIIAuto.AppSettings.Settings["mailSubject"].Value = this.txtMailSubject.Text;
                if (this.configSIIAuto.AppSettings.Settings["mailBody"] != null) this.configSIIAuto.AppSettings.Settings["mailBody"].Value = this.txtMailBody.Text;

                if (this.configSIIAuto.AppSettings.Settings["numeroIntentosSiError"] != null) this.configSIIAuto.AppSettings.Settings["numeroIntentosSiError"].Value = this.txtNumeroIntentos.Text;
                if (this.configSIIAuto.AppSettings.Settings["tiempoEsperaSiError"] != null) this.configSIIAuto.AppSettings.Settings["tiempoEsperaSiError"].Value = this.txtTiempoEspera.Text;

                this.configSIIAuto.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                ConfigurationSectionGroup applicationSectionGroup = this.configSIIAuto.GetSectionGroup("applicationSettings");

                ConfigurationSection applicationConfigSection = applicationSectionGroup.Sections["SIIEnvioAutomatico.Properties.Settings"];
                if (applicationConfigSection != null)
                {
                    ClientSettingsSection clientSection = (ClientSettingsSection)applicationConfigSection;

                    //WebService Configuration Setting
                    SettingElement applicationSetting = clientSection.Settings.Get("SIIEnvioAutomatico_tgSIIWebService_TGsiiService");
                    //Falta verificar la URL que sea válida
                    applicationSetting.Value.ValueXml.InnerXml = this.txtUrlWebService.Text;

                    applicationConfigSection.SectionInformation.ForceSave = true;
                }

                this.configSIIAuto.Save();
                Properties.Settings.Default.Reload();
                
                this.Close();
            }
        }

        /// <summary>
        /// Devuelve la lista de elementos
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="separador"></param>
        /// <returns></returns>
        private string DevolverListaValores(ListBox lb, string separador)
        {
            string result = "";
            try
            {
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    result += lb.Items[i] + separador;
                }

                if (result.Length > 1 && result.Substring(result.Length-1, 1) == separador) result = result.Substring(0, result.Length - 1);
            }
            catch{ }

            return (result);
        }

        //Valida el formulario
        private bool isFormValid()
        {
            bool result = true;
            string errores = "";

            //Url Servicio Web
            string urlServicioWebSII = this.txtUrlWebService.Text.Trim();
            if (!IsReachableUri(urlServicioWebSII))
            {
                errores += "La dirección del Servicio Web no es correcta.";
                this.txtUrlWebService.Focus();
            }

            if (this.chkActivarRespuestaEnvio.Checked && this.txtRutaFicheroRespuestaEnvio.Text == "")
            {
                if (errores != "") errores += "\n\r";
                errores += "Debe indicar la ruta donde se grabará el fichero con la respuesta del envío.";
                this.txtRutaFicheroLog.Focus();
            }

            if (this.chkActivarLog.Checked && this.txtRutaFicheroLog.Text == "")
            {
                if (errores != "") errores += "\n\r";
                errores += "Debe indicar la ruta donde se grabará el fichero de log.";
                this.txtRutaFicheroLog.Focus();
            }

            if (this.chkMailActivo.Checked)
            {
                if (this.txtSMTPServidor.Text == "")
                {
                    if (errores != "") errores += "\n\r";
                    errores += "Debe indicar el servidor de SMTP.";
                    this.txtSMTPServidor.Focus();
                }
                if (this.txtMailFrom.Text == "")
                {
                    if (errores != "") errores += "\n\r";
                    errores += "Debe indicar la dirección desde donde se enviará el correo.";
                    this.txtMailFrom.Focus();
                }
                if (this.txtMailTo.Text == "")
                {
                    if (errores != "") errores += "\n\r";
                    errores += "Debe indicar la dirección de la persona a la que se le enviará el correo.";
                    this.txtMailTo.Focus();
                }
                if (this.chkSMTPEnableSSL.Checked)
                {
                    if (this.txtSMTPUsuario.Text == "")
                    {
                        if (errores != "") errores += "\n\r";
                        errores += "Debe indicar el usuario de SMTP.";
                        this.txtSMTPUsuario.Focus();
                    }
                    if (this.txtSMTPContrasena.Text == "")
                    {
                        if (errores != "") errores += "\n\r";
                        errores += "Debe indicar la contraseña de SMTP.";
                        this.txtSMTPContrasena.Focus();
                    }
                }
            }

            if (errores != "")
            {
                result = false;
                MessageBox.Show(errores, "Error");
            }

            return (result);
        }

        /// <summary>
        /// Verifica que la url sel servicio web exista
        /// </summary>
        /// <param name="uriInput"></param>
        /// <returns></returns>
        private bool IsReachableUri(string uriInput)
        {
            // Variable to Return
            bool testStatus = false;
            try
            {
                // Create a request for the URL.
                System.Net.WebRequest request = System.Net.WebRequest.Create(uriInput);
                request.Timeout = 15000; // 15 Sec

                System.Net.WebResponse response;
                try
                {
                    response = request.GetResponse();
                    testStatus = true; // Uri does exist                 
                    response.Close();
                }
                catch
                {
                    testStatus = false; // Uri does not exist
                }

                return (testStatus);
            }
            catch { }

            return (testStatus);
        }

        /// <summary>
        /// Seleccionar la ruta del fichero que corresponda
        /// </summary>
        /// <param name="txtControl"></param>
        private void SeleccionarPath(TextBox txtControl, FolderBrowserDialog folderBrowser)
        {
            folderBrowser.Description = "Seleccionar una carpeta";
            folderBrowser.ShowNewFolderButton = false;

            if (txtControl.Text != "") folderBrowser.SelectedPath = txtControl.Text;

            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtControl.Text = folderBrowser.SelectedPath;
                //Environment.SpecialFolder root = folderBrowserDialogRespuesta.RootFolder;

            }
        }
        #endregion
    }
}