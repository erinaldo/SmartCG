using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using ObjectModel;
using System.Diagnostics;
using Telerik.WinControls; 
using Telerik.WinControls.UI;

namespace ModConsultaInforme
{
    public partial class frmInfoMayorContabAltaEdita : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCIMAYCONT";
        public string formCodeNombFichero = "MAYCONT";
        public string ficheroExtension = "mct";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCIMAYCONT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string grupo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string plan;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string porFechaporPeriodo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string moneda;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string ordenComp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string saldoInicial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string totalCuenta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string cuentaEditada;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string titulo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
            public string cuentasMayor;
        }

        private string tipoFichero = "EXCEL";

        string[] comprobante = new string[11];
        string primeraFila = "";
        string[] columnas;

        ArrayList cuentasAProcesar;
        int nivelPlan;
        string calendario = "";

        private decimal totalImporteDebe = 0;
        private decimal totalImporteHaber = 0;
        private decimal totalFinalImporteDebe = 0;
        private decimal totalFinalImporteHaber = 0;

        private string codigoCompania = "";
        private string descCompania = "";
        private string codigoGrupo = "";
        private string descGrupo = "";
        private string codigoPlan = "";
        private string descPlan = "";

        StringBuilder documento_HTML;

        private string tipoDato = "";

        FormularioValoresCampos valoresFormulario;

        private ArrayList avisosAutorizaciones;

        private bool cargarPlanes = false;

        //Procesar Informe
        private ArrayList aEmpresas = null;
        private string[] codDes = new string[2];
        private int[] nivelLongitud;

        private string nombreFicheroAGenerar = "";

        private string mensajeProceso = "";

        public frmInfoMayorContabAltaEdita()
        {
            InitializeComponent();

            this.gbListaCuentasMayor.ElementTree.EnableApplicationThemeName = false;
            this.gbListaCuentasMayor.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoMayorContabAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mayor de Contabilidad");
            
            //Obtener el tipo de fichero
            tipoFichero = GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes;

            utiles.ButtonEnabled(ref this.btnAddCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);

            //Traducir los literales
            this.TraducirLiterales();

            //Cargar compañías
            this.FillCompanias();

            //Cargar grupo de compañías
            this.FillGruposCompanias();

            //Cargar planes
            this.FillPlanes("");

            //Llenar el desplegable de Moneda
            this.FillMonedas();

            //Llenar el desplegable de Orden de Comprobante
            this.FillOrdenComprobantes();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";

            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    //Iniciar el Campo titulo con el nombre del formulario
                    this.txttitulo.Text = this.Text;

                    this.cmbCompania.Select();
                }
            }
            else
            {
                //Iniciar el Campo titulo con el nombre del formulario
                this.txttitulo.Text = this.Text;

                this.cmbCompania.Select();
            }
        }

        private void RbFecha_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void RbPeriodos_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void BtnAddCuentaMayor_Click(object sender, EventArgs e)
        {
            string codCuentaActual = this.radButtonTextBoxSelCuentasMayor.Text.Trim();
            if (codCuentaActual != "")
            {
                string tipoCta = "";
                string validarCuentaMayor = this.ValidarCuentaMayor(codCuentaActual, this.codigoPlan, ref tipoCta);

                string error = this.LP.GetText("errValTitulo", "Error");
                if (validarCuentaMayor != "")
                {
                    RadMessageBox.Show(validarCuentaMayor, error);
                    this.radButtonTextBoxSelCuentasMayor.Select();
                    return;
                }

                string result = this.AddToListBox(this.radButtonTextBoxSelCuentasMayor.Text, ref this.lbCuentasMayor);
                switch (result)
                {
                    case "":
                        this.radButtonTextBoxSelCuentasMayor.Text = "";
                        this.radButtonTextBoxSelCuentasMayor.Focus();
                        break;
                    case "1":
                        RadMessageBox.Show(this.LP.GetText("errCuentaMayorExiste", "La cuenta de mayor ya está en la lista"), error);
                        this.radButtonTextBoxSelCuentasMayor.Focus();
                        break;
                }
            }
        }

        private void BtnQuitarCuentaMayor_Click(object sender, EventArgs e)
        {
            try
            {
                RadListDataItem item = this.lbCuentasMayor.SelectedItem;
                this.lbCuentasMayor.Items.Remove(item);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCuentasMayor.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnAddCuentaMayor, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
                this.radButtonTextBoxSelCuentasMayor.Select();
            }
            else this.lbCuentasMayor.Select();
        }
        
        private void LbCuentasMayor_Enter(object sender, EventArgs e)
        {
            if (this.lbCuentasMayor.Items.Count > 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, true);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, true);
            }
        }

        private void TgTexBoxSelCuentasMayor_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
            this.lbCuentasMayor.SelectedIndex = -1;
        }
        

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void FrmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion
            {
                FormCode = this.formCode,
                FicheroExtension = this.ficheroExtension,
                Formulario = this
            };
            string result = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));

            if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString().Trim() != "") this.cmbCompania.Enabled = false;
        }

        private void RadButtonEjecutar_Click(object sender, EventArgs e)
        {
            this.Ejecutar();
        }

        private void RadButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
        }

        private void RadButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void CmbCompania_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString() != "")
            {
                this.codigoCompania = this.cmbCompania.SelectedValue.ToString();

                string companiaDesc = "";
                string codPlan = "";
                string result = this.ValidarCompaniaCodPlan(this.codigoCompania, ref companiaDesc, ref codPlan, false);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    this.cmbCompania.SelectedValue = "";
                    this.cmbCompania.Select();
                }
                else
                {
                    this.radDropDownListGrupo.SelectedValue = "";
                    this.radDropDownListGrupo.Refresh();

                    if (this.cargarPlanes)
                    {
                        this.FillPlanes("");
                        this.cargarPlanes = false;
                    }

                    this.codigoPlan = codPlan;
                    try
                    {
                        this.radDropDownListPlan.SelectedValue = "";
                        this.radDropDownListPlan.Enabled = false;
                        this.radDropDownListPlan.SelectedValue = codPlan;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    if (this.rbFecha.IsChecked) this.dateTimeDesdeFecha.Focus();
                    else this.txtMaskDesdePeriodos.Focus();
                }
            }
            else
            {
                if (!this.radDropDownListPlan.Enabled)
                {
                    this.radDropDownListPlan.SelectedValue = "";
                    this.radDropDownListPlan.Enabled = true;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadDropDownListGrupo_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.radDropDownListGrupo.SelectedValue != null && this.radDropDownListGrupo.SelectedValue.ToString() != "")
            {
                this.codigoGrupo = this.radDropDownListGrupo.SelectedValue.ToString();

                string grupoDesc = "";
                string resultValGrupo = this.ValidarGrupo(this.codigoGrupo, ref grupoDesc, false);

                if (resultValGrupo != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(resultValGrupo, error);
                    this.radDropDownListGrupo.SelectedValue = "";
                    this.radDropDownListGrupo.Focus();
                }
                else
                {
                    this.cmbCompania.SelectedValue = "";

                    this.FillPlanes(this.codigoGrupo);
                    this.cargarPlanes = true;
                    this.radDropDownListPlan.Enabled = true;

                    this.radDropDownListPlan.Select();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void RadDropDownListPlan_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.radDropDownListPlan.SelectedValue != null && this.radDropDownListPlan.SelectedValue.ToString() != "")
            {
                string codigo = this.radDropDownListGrupo.SelectedValue.ToString();

                if (codigo != "" && codigo.Length >= 1)
                {
                    string descPlan = "";
                    string result = this.ValidarPlan(this.codigoPlan, ref descPlan);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radDropDownListPlan.Focus();
                    }
                    else
                    {
                        string codPlan = this.codigoPlan;

                        try
                        {
                            this.radDropDownListPlan.SelectedValue = "";
                            this.radDropDownListPlan.Enabled = false;
                            this.radDropDownListPlan.SelectedValue = codPlan;
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        this.gbListaCuentasMayor.Enabled = true;

                        if (this.rbFecha.IsChecked) this.dateTimeDesdeFecha.Focus();
                        else this.txtMaskDesdePeriodos.Focus();
                    }
                }
                else
                {
                    this.radDropDownListPlan.Focus();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void RbFecha_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void RbPeriodos_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void RadDropDownListGrupo_MouseEnter(object sender, EventArgs e)
        {

        }

        private void RadButtonEjecutar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEjecutar);
        }

        private void RadButtonEjecutar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEjecutar);
        }

        private void RadButtonGrabarPeticion_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabarPeticion);
        }

        private void RadButtonGrabarPeticion_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabarPeticion);
        }

        private void RadButtonCargarPeticion_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCargarPeticion);
        }

        private void RadButtonCargarPeticion_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCargarPeticion);
        }

        private void RadButtonTextBoxSelCuentasMayor_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxSelCuentasMayor.Text.Trim() == "") utiles.ButtonEnabled(ref this.btnAddCuentaMayor, false);
            else utiles.ButtonEnabled(ref this.btnAddCuentaMayor, true);
        }

        private void RadButtonElementSelCuentasMayor_Click(object sender, EventArgs e)
        {
            if (codigoPlan == "") return;

            //Consulta que se ejecutará para obtener los Elementos
            string query = "select min(CUENMC) CUENMC, TCUEMC, max(NOLAAD) NOLAAD, STATMC, CEDTMC from ";
            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
            query += "where TIPLMC = '" + codigoPlan + "' ";
            query += "group by CEDTMC, STATMC, TCUEMC order by CUENMC";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                "Tipo", //falta traducir
                this.LP.GetText("lblListaCampoDescripcion", "Descripción"),
                "Estado"   //falta traducir
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar cuentas de mayor",
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
                utiles.ButtonEnabled(ref this.btnAddCuentaMayor, true);
            }
            this.radButtonTextBoxSelCuentasMayor.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelCuentasMayor;
            this.radButtonTextBoxSelCuentasMayor.Select(0, 0);
            this.radButtonTextBoxSelCuentasMayor.Focus();
        }

        private void BtnQuitarCuentaMayorTodas_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lbCuentasMayor.Items.Count;)
                {
                    RadListDataItem item = this.lbCuentasMayor.Items[i];
                    this.lbCuentasMayor.Items.Remove(item);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCuentasMayor.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
                this.radButtonTextBoxSelCuentasMayor.Select();
            }
            else this.lbCuentasMayor.Select();
        }

        private void BtnAddCuentaMayor_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAddCuentaMayor);
        }

        private void BtnAddCuentaMayor_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAddCuentaMayor);
        }

        private void BtnQuitarCuentaMayor_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarCuentaMayor);
        }

        private void BtnQuitarCuentaMayor_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarCuentaMayor);
        }

        private void BtnQuitarCuentaMayorTodas_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarCuentaMayorTodas);
        }

        private void BtnQuitarCuentaMayorTodas_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarCuentaMayorTodas);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.ProcesarInforme();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.mensajeProceso != "") RadMessageBox.Show(this.mensajeProceso);
            else
            {
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", "Mayor de Contabilidad", MessageBoxButtons.YesNo, RadMessageIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        //Abrir el fichero
                        string fichero = this.nombreFicheroAGenerar;
                        if (GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes == "EXCEL")
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = tipoFichero, //coincide tipoFichero y nombre del .exe (excel)
                                                        //startInfo.FileName = "EXCEL.EXE";
                                Arguments = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                            };
                            Process.Start(startInfo);
                        }
                        else
                        {
                            //HTML
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                UseShellExecute = true,
                                FileName = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                            };

                            Process.Start(startInfo);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    //MessageBox.Show("No was pressed");
                }
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int opcion = e.ProgressPercentage;

            switch (opcion)
            {
                case 0:
                    this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Marquee);
                    break;
                case 1:
                    this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
                    break;
                case 2:
                    if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    else this.pBarProcesandoInfo.Value += 10;
                    break;
            }
        }

        private void FrmInfoMayorContabAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mayor de Contabilidad");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemMayorContab", "Mayor de Contabilidad");
            this.Text = this.Text.Replace("&", "");
            this.radLabelCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.radLabelGrupo.Text = this.LP.GetText("lblGrupo", "Grupo");
            this.radLabelPlan.Text = this.LP.GetText("lblPlan", "Plan");
            //this.rbFecha.Text = this.LP.GetText("lblPorFecha", "Por fecha");
            //this.rbPeriodos.Text = this.LP.GetText("lblPorPeriodos", "Por Periodos");
            this.lblDesdeFecha.Text = this.LP.GetText("lblDesde", "Desde");
            this.lblDesdePeriodos.Text = this.LP.GetText("lblDesde", "Desde");
            this.lblHastaFecha.Text = this.LP.GetText("lblHasta", "Hasta");
            this.lblHastaPeriodos.Text = this.LP.GetText("lblHasta", "Hasta");
            this.lblMoneda.Text = this.LP.GetText("lblMoneda", "Moneda");
            //this.lblOrdenComp.Text = this.LP.GetText("lblHasta", "Orden comprobantes");
            this.chkSaldoInicial.Text = this.LP.GetText("lblSaldoIni", "Saldo inicial");
            this.chkTotalCuenta.Text = this.LP.GetText("lblTotalCuenta", "Total por cuenta");
            this.gbListaCuentasMayor.Text = this.LP.GetText("lblCuentasMayor", "Lista de Cuentas de Mayor");
            this.btnAddCuentaMayor.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnQuitarCuentaMayor.Text = this.LP.GetText("lblQuitar", "Quitar");
            this.lblProcesandoInfo.Text = this.LP.GetText("lblProcesandoInfo", "Procesando informe");

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Habilita y Deshabilita los controles relacionados con los radiobutton de Fechas y Periodos
        /// </summary>
        private void RadioButtonChange()
        {
            if (this.rbFecha.IsChecked)
            {
                this.dateTimeDesdeFecha.Enabled = true;
                this.dateTimeHastaFecha.Enabled = true;
                this.rbPeriodos.IsChecked = false;
                this.txtMaskDesdePeriodos.Enabled = false;
                this.txtMaskDesdePeriodos.Value = "";
                this.txtMaskHastaPeriodos.Enabled = false;
                this.txtMaskHastaPeriodos.Value = "";
                this.dateTimeDesdeFecha.Focus();
            }
            else
            {
                this.dateTimeDesdeFecha.Enabled = false;
                this.dateTimeDesdeFecha.Text = "";
                this.dateTimeHastaFecha.Enabled = false;
                this.dateTimeHastaFecha.Text = "";
                this.rbFecha.IsChecked = false;
                this.txtMaskDesdePeriodos.Enabled = true;
                this.txtMaskHastaPeriodos.Enabled = true;
                this.txtMaskDesdePeriodos.Focus();
            }
        }

        private void FillMonedas()
        {
            var items = new BindingList<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ML", this.LP.GetText("lblTipoMonedaLocal", "Local")),
                new KeyValuePair<string, string>("ME", this.LP.GetText("lblTipoMonedaExt", "Extranjera")),
                new KeyValuePair<string, string>("TI", this.LP.GetText("lblTipoMonedaTercerImporte", "Tercer importe"))
            };

            this.cmbMoneda.DataSource = items;
            this.cmbMoneda.ValueMember = "Key";
            this.cmbMoneda.DisplayMember = "Value";
        }


        private void FillOrdenComprobantes()
        {
            var items = new BindingList<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("TN", this.LP.GetText("lblOrdenCompTipoNum", "Tipo-Número")),
                new KeyValuePair<string, string>("FE", this.LP.GetText("lblOrdenCompFecha", "Fecha"))
            };

            this.cmbOrdenComp.DataSource = items;
            this.cmbOrdenComp.ValueMember = "Key";
            this.cmbOrdenComp.DisplayMember = "Value";
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
            string query = "Select CCIAMG, NCIAMG From " + GlobalVar.PrefijoTablaCG + "GLM01 Order by CCIAMG";
            string result = utiles.FillComboBox(query, "CCIAMG", "NCIAMG", ref this.cmbCompania, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetCompanias", "Error obteniendo las compañías") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Cargar grupos de compañías
        /// </summary>
        private void FillGruposCompanias()
        {
            string query = "select GRUPGR, NOMBGR from " + GlobalVar.PrefijoTablaCG + "GLM07 order by GRUPGR";
            string result = utiles.FillComboBox(query, "GRUPGR", "NOMBGR", ref this.radDropDownListGrupo, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetGrupo", "Error obteniendo los grupos de compañías") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Cargar planes
        /// </summary>
        private void FillPlanes(string codGrupo)
        {
            if (this.radDropDownListPlan.Items.Count > 0) this.radDropDownListPlan.Items.Clear();

            string query;
            if (codGrupo == "") query = "select TIPLMP, NOMBMP from " + GlobalVar.PrefijoTablaCG + "GLM02 order by TIPLMP";
            else
            {
                query = "select distinct TIPLMP, NOMBMP from " + GlobalVar.PrefijoTablaCG + "GLM01J, " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                query += "where GRUPAI = '" + this.codigoGrupo + "' and TIPLMP = TIPLMG ";
                query += "order by TIPLMP";
            }

            string result = utiles.FillComboBox(query, "TIPLMP", "NOMBMP", ref this.radDropDownListPlan, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetGrupo", "Error obteniendo los planes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }
       
        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;

            string error = this.LP.GetText("errValTitulo", "Error");

            codigoCompania = "";
            if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString().Trim() != "") codigoCompania = this.cmbCompania.SelectedValue.ToString();

            codigoGrupo = "";
            if (this.radDropDownListGrupo.SelectedValue != null && this.radDropDownListGrupo.SelectedValue.ToString().Trim() != "") codigoGrupo = this.radDropDownListGrupo.SelectedValue.ToString();

            if (codigoCompania.Trim() == "" && codigoGrupo.Trim() == "")
            {
                RadMessageBox.Show(this.LP.GetText("errCompaniaGrupoObl", "Es obligatorio informar la compañía o el grupo de compañías"), error);
                this.cmbCompania.Select();
                return (false);
            }

            if (codigoCompania != "")
            {
                string validarCompania = this.ValidarCompania(codigoCompania, ref descCompania, false);
                if (validarCompania != "")
                {
                    RadMessageBox.Show(validarCompania, error);
                    this.cmbCompania.Select();
                    return (false);
                }
            }
            else
            if (codigoGrupo != "")
            {
                string validarGrupo = this.ValidarGrupo(codigoGrupo, ref descGrupo, false);
                if (validarGrupo != "")
                {
                    RadMessageBox.Show(validarGrupo, error);
                    this.radDropDownListGrupo.Select();
                    return (false);
                }
            }

            /*if (codigoGrupo.Trim() != "" && codigoPlan.Trim() == "")
            {
                RadMessageBox.Show(this.LP.GetText("errPlanObl", "Es obligatorio informar el plan de cuentas"), error);
                this.tgTexBoxSelPlan.Textbox.Select();
                return (false);
            }*/

            if (codigoCompania == "" && codigoGrupo != "")
            {
                codigoPlan = "";
                if (this.radDropDownListPlan.SelectedValue != null && this.radDropDownListPlan.SelectedValue.ToString().Trim() != "") codigoPlan = this.radDropDownListPlan.SelectedValue.ToString();

                if (codigoPlan.Trim() != "")
                {
                    string validarPlan = this.ValidarPlan(codigoPlan, ref descPlan);
                    if (validarPlan != "")
                    {
                        RadMessageBox.Show(validarPlan, error);
                        this.radDropDownListPlan.Select();
                        return (false);
                    }
                }
            }

            if (this.rbFecha.IsChecked)
            {
                if (this.dateTimeDesdeFecha.Text.Trim() == "")
                {
                    RadMessageBox.Show(this.LP.GetText("errFechaDesdeObl", "Es obligatorio informar la fecha Desde"), error);
                    this.dateTimeDesdeFecha.Focus();
                    return (false);
                }
                if (this.dateTimeHastaFecha.Text.Trim() == "")
                {
                    RadMessageBox.Show(this.LP.GetText("errFechaHastaObl", "Es obligatorio informar la fecha Hasta"), error);
                    this.dateTimeHastaFecha.Focus();
                    return (false);
                }
                //Validar las fechas
            }
            else 
                if (this.rbPeriodos.IsChecked)
                {
                    //string resultMsg = this.ValidarPeriodo(ref this.txtMaskDesdePeriodos);
                    this.txtMaskDesdePeriodos.Value = this.txtMaskDesdePeriodos.Value.ToString().PadRight(5, '0');
                    this.txtDesdePeriodos.Text = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2);
                    string resultMsg = this.ValidarPeriodo(ref this.txtDesdePeriodos);
                    if (resultMsg != "")
                    {
                        RadMessageBox.Show(resultMsg, error);
                        this.txtMaskDesdePeriodos.Focus();
                        return (false);
                    }

                    //resultMsg = this.ValidarPeriodo(ref this.txtMaskHastaPeriodos);
                    this.txtMaskHastaPeriodos.Value = this.txtMaskHastaPeriodos.Value.ToString().PadRight(5, '0');
                    this.txtHastaPeriodos.Text = this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                    if (resultMsg != "")
                    {
                        RadMessageBox.Show(resultMsg, error);
                        this.txtMaskHastaPeriodos.Focus();
                        return (false);
                    }
                }

            //Validar las cuentas de mayor si están informadas
            if (this.lbCuentasMayor.Items.Count > 0)
            {
                string cuentaMayor = "";
                int indice = 0;
                string resultMsgCtasMayor = this.ValidarCuentasMayor(ref this.lbCuentasMayor, ref cuentaMayor, ref indice, this.codigoPlan);
                if (resultMsgCtasMayor != "")
                {
                    if (indice != -1)
                    {
                        this.lbCuentasMayor.SelectedIndex = indice;
                        utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, true);
                        utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, true);
                    }
                    RadMessageBox.Show(resultMsgCtasMayor, error);
                    return (false);
                }
            }

            return (result);
        }
        
        /// <summary>
        /// Verifica si existen datos para el informe
        /// </summary>
        /// <returns></returns>
        private bool HayDatosInforme()
        {
            bool existenDatos = false;
            try
            {
                this.backgroundWorker1.ReportProgress(2);

                aEmpresas = null;
                codDes = new string[2];

                cuentasAProcesar = new ArrayList();

                if (codigoGrupo != "")
                {
                    //if (codigoGrupo != "" && codigoPlan != "")
                    if (codigoGrupo != "")
                    {
                        //Buscar las empresas del grupo
                        aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(codigoGrupo, codigoPlan);
                        calendario = "";
                    }
                }
                else
                {
                    codDes[0] = codigoCompania;
                    codDes[1] = descCompania;
                    aEmpresas = new ArrayList
                    {
                        codDes
                    };

                    //Buscar el plan de la compañía (el método devuelve además el calendario)
                    string[] datosCompaniaAct = utilesCG.ObtenerTipoCalendarioCompania(codigoCompania);
                    codigoPlan = datosCompaniaAct[1];
                    calendario = datosCompaniaAct[0];
                }

                if (this.codigoPlan != "")
                {
                    this.backgroundWorker1.ReportProgress(2);

                    nivelLongitud = utilesCG.ObtenerNivelLongitudDadoPlan(codigoPlan);
                    nivelPlan = nivelLongitud[0];

                    //Buscar las cuentas
                    this.ObtenerCuentas(this.codigoPlan);

                    if (this.cuentasAProcesar.Count <= 0)
                    {
                        this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");
                        existenDatos = false;
                    }
                }

                existenDatos = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (existenDatos);
        }

        /// <summary>
        /// Genera el Informe
        /// </summary>
        private void ProcesarInforme()
        {
            this.mensajeProceso = "";

            //Iniciar la barra de progreso
            this.backgroundWorker1.ReportProgress(0);

            //Verificar que existan datos para el informe
            if (!this.HayDatosInforme())
            {
                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
                //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                return;
            }

            //Crear el fichero en memoria (Excel o HTML)
            this.InformeHTMLCrear(ref this.documento_HTML);

            //Cabecera de las columnas del informe
            columnas = this.ObtenerCabecerasColumnasInforme();
            
            tipoDato = this.TipoDato();
            string[] datosCompania;

            //Iniciar la barra de progreso
            //this.backgroundWorker1.ReportProgress(0);
            //this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Marquee);

            //Eliminar los posibles permisos de no autorizado del usuario conectado
            GlobalVar.UsuarioEnv.ListaNoAutorizado.Clear();

            //Inicializar los avisos de autorizaciones
            this.avisosAutorizaciones = new ArrayList();

            bool hayReg = false;
            string[] datosCompAct;

            for (int i = 0; i < aEmpresas.Count; i++)
            {
                totalFinalImporteDebe = 0;
                totalFinalImporteHaber = 0;

                datosCompania = (string[])aEmpresas[i];
                string desc = datosCompania[1];

                if (i == 0) this.InformeHTMLEscribirEncabezado(desc);
                /*DEL if (i == 0 && tipoFichero == "HTML") this.InformeHTMLEscribirEncabezado(desc);*/

                //Escribir 1ra fila y las Cabeceras de las columnas
                primeraFila = this.InformeTitulo(desc);

                string codPlan;
                if (this.codigoPlan == "" && this.codigoGrupo != "")
                {
                    //Buscar el plan y el calendario de la compañía
                    datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(datosCompania[0].ToString());
                    calendario = datosCompAct[0];
                    codPlan = datosCompAct[1];

                    nivelLongitud = utilesCG.ObtenerNivelLongitudDadoPlan(codPlan);
                    nivelPlan = nivelLongitud[0];

                    if (i > 0) cuentasAProcesar.Clear();

                    //Buscar las cuentas
                    this.ObtenerCuentas(codPlan);
                }
                else codPlan = this.codigoPlan;

                bool hayRegAux = this.ProcesarMayorContabilidadCompania(datosCompania[0].ToString(), codPlan);

                if (hayRegAux)
                {
                    hayReg = true;

                    //Escribir linea total del periodo
                    comprobante[0] = "";
                    comprobante[1] = "";
                    comprobante[2] = "";
                    comprobante[3] = "";
                    comprobante[4] = "";
                    comprobante[5] = "";
                    comprobante[6] = this.LP.GetText("lblCabTotalPeriodo", "TOTAL PERIODO");   //o fecha TAL FALTA !!!

                    comprobante[7] = "";
                    comprobante[10] = "";

                    comprobante[8] = totalFinalImporteDebe.ToString();
                    comprobante[9] = totalFinalImporteHaber.ToString();

                    this.InformeHTMLEscribirDetalleComprobante(comprobante);
                    
                    //Escribir línea en blanco
                    this.EscribirLineaEnBlanco();

                    /*DEL if (tipoFichero == "HTML")*/ this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);
                }
            }
           
            if (!hayReg) 
            {
                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
                //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");               
                return; 
            }

            //Escribir los avisos de no autorizaciones si existen
            if (this.avisosAutorizaciones.Count > 0)
            {
                //Crear tabla
                documento_HTML.Append("     <table>");
                for (int i = 0; i < this.avisosAutorizaciones.Count; i++)
                {
                    documento_HTML.Append("         <tr>\n");
                    documento_HTML.Append("             <td class=Texto width =\"8%\">" + this.avisosAutorizaciones[i].ToString() + "</td>\n");
                    documento_HTML.Append("         </tr>\n");
                }
                documento_HTML.Append("     <table>");
                //Fin tabla
            }

            this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);

            this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);

            //Generar el nombre de fichero
            this.nombreFicheroAGenerar = this.InformeNombreFichero(formCodeNombFichero, System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"], this.txttitulo.Text);

            //Grabar el fichero de informe
            try 
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.nombreFicheroAGenerar);
                sw.WriteLine(this.documento_HTML.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                this.mensajeProceso = "Error generando el informe. Para más información consulte el fichero de Log.";
            }

            //Finaliza la barra de progreso
            this.backgroundWorker1.ReportProgress(1);
            //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

            /*
                        //Escribir los ajustes globales en el fichero HTML / Excel
                        if (tipoFichero == "HTML")
                        {
                            //Escribir línea en blanco
                            this.EscribirLineaEnBlanco();

                            this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);

                            //Finaliza la barra de progreso
                            this.backgroundWorker1.ReportProgress(1);
                            //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                            this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
                        }
                        else 
                        {
                            //Finaliza la barra de progreso
                            this.backgroundWorker1.ReportProgress(1);
                            //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                            if (GlobalVar.UsuarioEnv.ModConsInfo_VisualizarFicheroInformes)
                            {
                                string ficheroHTML = this.InformeNombreFichero(formCode, System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"], this.txttitulo.Text);

                                try // tratar de levantar excel
                                {
                                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                                    sw.WriteLine(this.documento_HTML.ToString());
                                    sw.Close();

                                    ProcessStartInfo startInfo = new ProcessStartInfo
                                    {
                                        FileName = tipoFichero, //coincide tipoFichero y nombre del .exe (excel)
                                                                //startInfo.FileName = "EXCEL.EXE";
                                        Arguments = "\"" + ficheroHTML + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                                    };
                                    Process.Start(startInfo);
                                }
                                catch // si no puede levantar excel, levantar html
                                {
                                    this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
                                }
                                // fin levantar
                            }
                        }

                        if (!GlobalVar.UsuarioEnv.ModConsInfo_VisualizarFicheroInformes)
                        {
                            try // tratar de levantar excel
                            {
                                //Escribir el fichero
                                nombreFicheroAGenerar += ".html";
                                System.IO.StreamWriter sw = new System.IO.StreamWriter(nombreFicheroAGenerar);
                                sw.WriteLine(this.documento_HTML.ToString());
                                sw.Close();
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                        else
                        {

                        }*/
        }

        /// <summary>
        /// Buscar las cuentas que sean del plan de la compañía y del nivel indicado para ese plan
        /// </summary>
        private void ObtenerCuentas(string codPlan)
        {
            IDataReader dr = null;

            try
            {
                //Buscar las cuentas que sean del plan de la compañía y del nivel indicado para ese plan
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM03 left join " + GlobalVar.PrefijoTablaCG + "GLMX3 on ";
                query += " TIPLMC = TIPLMX and CUENMC = CUENMX ";
                query += " where TCUEMC = 'D' and TIPLMC='" + codPlan + "' and NIVEMC = " + nivelPlan;
                    
                //Condicionar a las cuentas indicadas
                if (this.lbCuentasMayor.Items.Count > 0)
                {
                    query += " and (";
                    for (int i = 0; i < this.lbCuentasMayor.Items.Count; i++)
                    {
                        if (i != 0) query += " or ";
                        query += "CUENMC like '" + this.lbCuentasMayor.Items[i].ToString() + "%'";
                    }
                    query += ")";
                }

                query += " order by CUENMC";

                string sconmc = "";
                int ultNivelConsultaCtaAut = -1;
                int nivelConsultaCtaActual = -1;

                string taux1 = "";
                string taux2 = "";
                string taux3 = "";
                bool autTaux1 = false;
                bool autTaux2 = false;
                bool autTaux3 = false;

                string grctmx = "";

                string aviso = "";

                bool chequearAut = (this.lbCuentasMayor.Items.Count <= 0) ? true : false;
                bool autCuenta;
                if (chequearAut) autCuenta = false;
                else autCuenta = true;

                string[] datos;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    if (chequearAut)
                    {
                        //Chequear autorizaciones
                        sconmc = dr["SCONMC"].ToString();
                        grctmx = dr["GRCTMX"].ToString();

                        //Chequear autorización a la cuenta de mayor
                        try
                        {
                            nivelConsultaCtaActual = Convert.ToInt16(sconmc);

                            if (ultNivelConsultaCtaAut == -1 || nivelConsultaCtaActual > ultNivelConsultaCtaAut)
                            {
                                //Verificar autorizaciones
                                autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "003", "02", "0" + sconmc, codPlan);
                                if (autCuenta)
                                {
                                    ultNivelConsultaCtaAut = nivelConsultaCtaActual;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            autCuenta = false;
                        }

                        if (autCuenta)
                        {
                            //Chequear autorización a grupos de cuentas de mayor
                            if (grctmx != "")
                            {
                                autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "008", "02", "10", codPlan + grctmx);
                                if (!autCuenta)
                                {
                                    aviso = "Usuario no autorizado para algunos grupos de cuentas de mayor";  //Falta traducir
                                    if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                                }
                            }

                            //Chequear autorización sobre el tipo de auxiliar (a nivel de saldos)
                            taux1 = dr["TAU1MC"].ToString().Trim();
                            taux2 = dr["TAU2MC"].ToString();
                            taux3 = dr["TAU3MC"].ToString();

                            if (taux1 != "") autTaux1 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux1);  //permiso sobre saldo?? o sobre mov??
                            else autTaux1 = true;
                            if (taux2 != "") autTaux2 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux2);
                            else autTaux2 = true;
                            if (taux3 != "") autTaux3 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux3);
                            else autTaux3 = true;

                            if (autTaux1 && autTaux2 && autTaux3) autCuenta = true;
                            else
                            {
                                autCuenta = false;
                                aviso = "Usuario no autorizado para algunos tipos de auxiliar";  //Falta traducir
                                if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                            }
                        }
                        else
                        {
                            aviso = "Usuario no autorizado para algunas cuentas de mayor";  //Falta traducir
                            if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                        }
                    }

                    if (autCuenta)
                    {
                        datos = new string[5];
                        datos[0] = dr.GetValue(dr.GetOrdinal("CUENMC")).ToString().Trim();
                        datos[1] = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString().Trim();
                        datos[2] = dr.GetValue(dr.GetOrdinal("NIVEMC")).ToString().Trim();
                        datos[3] = dr.GetValue(dr.GetOrdinal("DEAUMC")).ToString().Trim();
                        datos[4] = dr.GetValue(dr.GetOrdinal("TAU1MC")).ToString().Trim();
                        cuentasAProcesar.Add(datos);
                    }
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
        /// Porcesar el informe para una compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        private bool ProcesarMayorContabilidadCompania(string codigo, string codPlan)
        {
            bool hayReg = false;
            try
            {
                string[] datos;
                
                bool hayRegAux = false;

                string siglo = "";
                string sigloanoPeriodoDesde = "";
                int fechaDesdeFormatoCG = -1;
                int fechaHastaFormatoCG = -1;

                if (cuentasAProcesar.Count > 0)
                {
                    if (this.rbPeriodos.IsChecked)
                    {
                        string aa = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2);
                        siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                        //sigloanoPeriodoDesde = siglo + this.txtDesdePeriodos.Text;
                        sigloanoPeriodoDesde = siglo + this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2);
                        //query += " and SAPRDT >=" + sigloanoPeriodoDesde;
                        aa = this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2);
                        siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                        //query += " and SAPRDT <=" + siglo + this.txtHastaPeriodos.Text;
                    }
                    else
                    {                
                        fechaDesdeFormatoCG = utiles.FechaToFormatoCG(this.dateTimeDesdeFecha.Value, true);
                        fechaHastaFormatoCG = utiles.FechaToFormatoCG(this.dateTimeHastaFecha.Value, true);
                        //query += " and FECODT >=" + fechaDesdeFormatoCG.ToString() + " and FECODT <=" + fechaHastaFormatoCG.ToString();

                        if (this.chkSaldoInicial.Checked)
                        {
                            sigloanoPeriodoDesde = utilesCG.ObtenerSAPRCalendarioDadoFecha(calendario, fechaDesdeFormatoCG.ToString());
                            if (sigloanoPeriodoDesde == "") sigloanoPeriodoDesde = "00000";
                        }
                    }
                }

                for (int i = 0; i < cuentasAProcesar.Count; i++)
                {
                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;

                    datos = (string[])cuentasAProcesar[i];

                    if (i == 0)
                    {
                        this.InformeHTMLEscribirCabeceras(primeraFila, columnas);
                        this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                    }

                    //Buscar los movimientos contables para la cuenta
                    hayRegAux = this.ProcesarMovimientosCuenta(codigo, datos[0], codPlan, calendario, siglo, sigloanoPeriodoDesde, fechaDesdeFormatoCG, fechaHastaFormatoCG, datos[3], datos[4]);

                    if (hayRegAux) hayReg = true;
                }

                //if (!hayReg) { RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado")); return; }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (hayReg);
        }

        /// <summary>
        /// Procesa todos los comprobantes de una cuenta de mayor
        /// </summary>
        /// <param name="codigoCompania">código de compañía</param>
        /// <param name="cuenta">código de la cuenta</param>
        /// <param name="plan">código del plan</param>
        /// <param name="calendario">código del calendario de la compañía</param>
        /// <param name="siglo"></param>
        /// <param name="sigloanoPeriodoDesde"></param>
        /// <param name="fechaDesdeFormatoCG"></param>
        /// <param name="fechaHastaFormatoCG"></param>
        /// <param name="cuentaDesglose">S -> cuenta aparece desglozada    N -> cuenta aparece resumida </param>
        /// <returns></returns>
        private bool ProcesarMovimientosCuenta(string codigoCompania, string cuenta, string plan, string calendario, string siglo, string sigloanoPeriodoDesde, int fechaDesdeFormatoCG, int fechaHastaFormatoCG, string cuentaDesglose, string taux1)
        {
            IDataReader dr = null;
            bool hayReg = false;

            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where STATDT = 'E' and CCIADT='" + codigoCompania + "' and ";
                query += "CUENDT = '" + cuenta + "' " ;
                
                if (this.rbPeriodos.IsChecked)
                {
                    query += " and SAPRDT >=" + sigloanoPeriodoDesde;
                    query += " and SAPRDT <=" + siglo + this.txtHastaPeriodos.Text;
                }
                else
                {
                    fechaDesdeFormatoCG = utiles.FechaToFormatoCG(this.dateTimeDesdeFecha.Value, true);
                    query += " and FECODT >=" + fechaDesdeFormatoCG.ToString() + " and FECODT <=" + fechaHastaFormatoCG.ToString();
                }

                switch (this.cmbOrdenComp.SelectedValue.ToString())
                {
                    case "TN":  //Tipo-número
                        query += " order by CCIADT, CUENDT, SAPRDT, TICODT, NUCODT, SIMIDT";
                        break;
                    case "FE":  //Fecha
                        query += " order by CCIADT, CUENDT, FECODT, SAPRDT, TICODT, NUCODT, SIMIDT";
                        break;
                }

                string saprdt = "";
                string anoSAPRDT = "";
                string perSAPRDT = "";
                string ticodt = "";
                string nucodt = "";

                string saprdtAux = "";

                ArrayList cuentaDatos;
                string cedtmc = "";
                string nolaad = "";

                string tmovdt = "";
                string importe = "";

                decimal importeDebe = 0;
                decimal importeHaber = 0;
                totalImporteDebe = 0;
                totalImporteHaber = 0;
                decimal importeDebeAcumulado = 0;
                decimal importeHaberAcumulado = 0;

                decimal[] saldoInicial = { 0, 0, 0 };
                decimal saldoFinal = 0;

                int registro = 0;

                //Saldo Inicial
                cuentaDatos = utilesCG.ObtenerDatosCuenta(cuenta, plan);
                cedtmc = cuentaDatos[1].ToString();
                nolaad = cuentaDatos[0].ToString();

                if (this.chkSaldoInicial.Checked)
                {
                    //Buscar SaldoInicial
                    string sapr = "";
                    if (sigloanoPeriodoDesde != "00000")
                    {
                        int sigloanoPeriodoDesdeAnterior = Convert.ToInt32(sigloanoPeriodoDesde) - 1;
                        sapr = sigloanoPeriodoDesdeAnterior.ToString();
                        if (sapr.Length < 5) sapr = sapr.PadLeft(5, '0');
                    }
                    else sapr = sigloanoPeriodoDesde;

                    saldoInicial = utilesCG.ObtenerSaldo(codigoCompania, plan, "00000", sapr, cuenta, this.tipoDato, " ", " ");

                    if (this.rbFecha.IsChecked)
                    {
                        //Sumar los importes de los movimientos de GLB01 anteriores a la fecha de inicio y que estén dentro del período
                        decimal[] saldoAnteriorFechaInicio = { 0, 0, 0 };
                        saldoAnteriorFechaInicio = utilesCG.ObtenerSaldoFechaAnteriorFechaInicioPeriodo(codigoCompania, sigloanoPeriodoDesde,
                                                            cuenta, plan, fechaDesdeFormatoCG.ToString());

                        saldoInicial[0] += saldoAnteriorFechaInicio[0];
                        saldoInicial[1] += saldoAnteriorFechaInicio[1];
                        saldoInicial[2] += saldoAnteriorFechaInicio[2];
                    }

                    saldoFinal = saldoInicial[2];
                }

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;

                    importe = "";
                    bool monedaExt_TercImp = false;
                    switch (this.cmbMoneda.SelectedValue.ToString())
                    {
                        case "ML":
                            importe = dr["MONTDT"].ToString();
                            break;
                        case "ME":
                            importe = dr["MOSMAD"].ToString();
                            monedaExt_TercImp = true;
                            break;
                        case "TI":
                            importe = dr["TERCAD"].ToString();
                            monedaExt_TercImp = true;
                            break;
                    }

                    bool procesarCuenta = true;
                    if (monedaExt_TercImp && importe != "")
                    {
                        //Comprobar que el importe sea diferente de 0
                        try
                        {
                            decimal importeDec = Convert.ToDecimal(importe);
                            if (importeDec == 0) procesarCuenta = false;
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    if (procesarCuenta)
                    {
                        hayReg = true;

                        if (registro == 0)
                        {
                            if (chkCuentaEditada.Checked == true)
                            {
                                if (this.chkSaldoInicial.Checked) this.EscribirSaldoInicial(cedtmc, nolaad, saldoInicial);
                            }
                            else
                            {
                                if (this.chkSaldoInicial.Checked) this.EscribirSaldoInicial(cuenta, nolaad, saldoInicial);
                            }
                        }

                        saprdt = dr["SAPRDT"].ToString();
                        if (cuentaDesglose == "N" && taux1 != "" && saprdtAux != "" && saprdt != saprdtAux)
                        {
                            //Escribe las líneas del detalle del comprobante en el fichero HTML
                            if (importeDebeAcumulado != 0)
                            {
                                comprobante[8] = importeDebeAcumulado.ToString();
                                comprobante[9] = "";
                                this.InformeHTMLEscribirDetalleComprobante(comprobante);
                            }

                            if (importeHaberAcumulado != 0)
                            {
                                comprobante[8] = "";
                                comprobante[9] = importeHaberAcumulado.ToString();
                                this.InformeHTMLEscribirDetalleComprobante(comprobante);
                            }

                            importeDebeAcumulado = 0;
                            importeHaberAcumulado = 0;
                        }
                        saprdtAux = saprdt;

                        if (saprdt.Length == 5) //saapp
                        {
                            anoSAPRDT = saprdt.Substring(1, 2);
                            perSAPRDT = saprdt.Substring(3, 2);
                        }
                        else  //aapp
                        {
                            anoSAPRDT = saprdt.Substring(0, 2);
                            perSAPRDT = saprdt.Substring(2, 2);
                        }

                        ticodt = dr["TICODT"].ToString();
                        nucodt = dr["NUCODT"].ToString();
                        nucodt = nucodt.PadLeft(5, '0');

                        if (chkCuentaEditada.Checked == true)
                            comprobante[0] = cedtmc;  //Cuenta Formateada
                        else
                            comprobante[0] = cuenta;  //Cuenta No Formateada

                        comprobante[1] = nolaad;  //nombre cuenta

                        if (cuentaDesglose == "S" || taux1 == "")
                        {
                            //Cuenta con desgloce de movimientos
                            comprobante[2] = anoSAPRDT + "-" + perSAPRDT + "-" + ticodt + "-" + nucodt;
                            comprobante[3] = utiles.FechaToFormatoCG(dr["FECODT"].ToString()).ToShortDateString();
                            comprobante[4] = dr["CAUXDT"].ToString();

                            string clase = dr["CLDODT"].ToString().Trim();
                            string nDoc = dr["NDOCDT"].ToString().Trim();
                            string claseNDoc = "";

                            if (clase != "")
                            {
                                claseNDoc = clase.PadRight(2, ' ') + "-" + nDoc.PadLeft(7, '0');
                            }
                            comprobante[5] = claseNDoc;
                            comprobante[6] = dr["DESCAD"].ToString();
                            comprobante[7] = "";// "Saldo inicial"
                        }
                        else
                        {
                            //Cuenta agrupada, sin desgloce de movimientos
                            comprobante[2] = anoSAPRDT + "-" + perSAPRDT;
                            comprobante[3] = "";
                            comprobante[4] = "";
                            comprobante[5] = "";
                            comprobante[6] = "MOVIMIENTOS DE AUXILIAR"; //Falta traducir
                            comprobante[7] = "";// "Saldo inicial"
                        }
                        /*
                        importe = "";
                        switch (this.cmbMoneda.SelectedValue.ToString())
                        {
                            case "ML" :
                                importe = dr["MONTDT"].ToString();
                                break;
                            case "ME" :
                                importe = dr["MOSMAD"].ToString();
                                break;
                            case "TI" :
                                importe = dr["TERCAD"].ToString();
                                break;
                        }
                        */
                        tmovdt = dr["TMOVDT"].ToString();
                        switch (tmovdt)
                        {
                            case "D":
                                try
                                {
                                    if (importe != "") importeDebe = Convert.ToDecimal(importe);
                                    else importeDebe = 0;

                                    totalImporteDebe += importeDebe;
                                    totalFinalImporteDebe += importeDebe;

                                    if (cuentaDesglose == "S" || taux1 == "") comprobante[8] = importeDebe.ToString();
                                    else
                                    {
                                        importeDebeAcumulado += importeDebe;
                                        comprobante[8] = importeDebeAcumulado.ToString();
                                    }

                                    comprobante[9] = "";

                                    saldoFinal += importeDebe;
                                    comprobante[10] = saldoFinal.ToString();
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                break;
                            case "H":
                                try
                                {
                                    if (importe != "") importeHaber = Convert.ToDecimal(importe);
                                    else importeHaber = 0;

                                    totalImporteHaber += importeHaber;
                                    totalFinalImporteHaber += importeHaber;

                                    comprobante[8] = "";

                                    if (cuentaDesglose == "S" || taux1 == "") comprobante[9] = importeHaber.ToString();
                                    else
                                    {
                                        importeHaberAcumulado += importeHaber;
                                        comprobante[9] = importeHaberAcumulado.ToString();
                                    }

                                    saldoFinal += importeHaber;
                                    comprobante[10] = saldoFinal.ToString();
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                break;
                        }

                        if (cuentaDesglose == "S" || taux1 == "")
                        {
                            //Escribe la línea del detalle del comprobante en el fichero HTML
                            this.InformeHTMLEscribirDetalleComprobante(comprobante);
                        }
                        registro++;
                    }
                }

                dr.Close();

                if (hayReg)
                {
                    if (cuentaDesglose == "N" && taux1 != "")
                    {
                        //Escribe la línea del detalle del comprobante en el fichero HTML
                        if (importeDebeAcumulado != 0)
                        {
                            comprobante[8] = importeDebeAcumulado.ToString();
                            comprobante[9] = "";
                            this.InformeHTMLEscribirDetalleComprobante(comprobante);
                        }

                        if (importeHaberAcumulado != 0)
                        {
                            comprobante[8] = "";
                            comprobante[9] = importeHaberAcumulado.ToString();
                            this.InformeHTMLEscribirDetalleComprobante(comprobante);
                        }
                    }
                }
                else
                {
                    if (chkCuentaEditada.Checked == true)
                    {
                        if (this.chkSaldoInicial.Checked && saldoInicial[2] != 0) this.EscribirSaldoInicial(cedtmc, nolaad, saldoInicial);
                    }
                    else
                    {
                        if (this.chkSaldoInicial.Checked && saldoInicial[2] != 0) this.EscribirSaldoInicial(cuenta, nolaad, saldoInicial);
                    }
                }

                if (chkTotalCuenta.Checked)
                {
                    if (hayReg || saldoInicial[2] != 0)
                    {
                        //Escribir línea de totales
                        comprobante[0] = "";
                        comprobante[1] = "";
                        comprobante[2] = "";
                        comprobante[3] = "";
                        comprobante[4] = "";
                        comprobante[5] = "";
                        if (chkCuentaEditada.Checked == true)
                            comprobante[6] = this.LP.GetText("lblCabTotalCuenta", "TOTAL CUENTA") + " " + cedtmc;
                        else
                            comprobante[6] = this.LP.GetText("lblCabTotalCuenta", "TOTAL CUENTA") + " " + cuenta;

                        if (this.chkSaldoInicial.Checked)
                        {
                            comprobante[7] = saldoInicial[2].ToString();
                            comprobante[10] = (saldoInicial[2] + totalImporteDebe + totalImporteHaber).ToString();
                        }
                        else
                        {
                            comprobante[7] = "";
                            //comprobante[10] = "";
                            comprobante[10] = (saldoInicial[2] + totalImporteDebe + totalImporteHaber).ToString();
                        }

                        if (totalImporteDebe != 0) comprobante[8] = totalImporteDebe.ToString();
                        else comprobante[8] = "";
                        if (totalImporteHaber != 0) comprobante[9] = totalImporteHaber.ToString();
                        else comprobante[9] = "";

                        this.InformeHTMLEscribirDetalleComprobante(comprobante);
                        
                        //Escribir una línea en blanco
                        this.EscribirLineaEnBlanco();
                    }
                }
                else if (hayReg) this.EscribirLineaEnBlanco();
            }
            catch(Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (hayReg);
        }

        /// <summary>
        /// Devuelve las cabeceras de las columnas del informe en el idioma correspondiente
        /// </summary>
        /// <returns></returns>
        private string[] ObtenerCabecerasColumnasInforme()
        {
            //Cabecera de las columnas del informe
            string[] columnas = new string[11];
            columnas[0] = this.LP.GetText("lblCabCuenta", "Cuenta");
            columnas[1] = this.LP.GetText("lblCabNombreCuenta", "Nombre de la cuenta");
            columnas[2] = this.LP.GetText("lblCabNumeroCompAbrev", "No. Comp.");
            columnas[3] = this.LP.GetText("lblCabFecha", "Fecha");
            columnas[4] = this.LP.GetText("lblCabCuentaAux", "Auxiliar");
            columnas[5] = this.LP.GetText("lblCabDocumento", "No. Documento");
            columnas[6] = this.LP.GetText("lblCabDesc", "Concepto");
            columnas[7] = this.LP.GetText("lblCabSaldoIni", "Saldo inicial");
            columnas[8] = this.LP.GetText("lblCabDebe", "DEBE");
            columnas[9] = this.LP.GetText("lblCabHaber", "HABER");
            columnas[10] = this.LP.GetText("lblCabSaldoFin", "Saldo final");

            return (columnas);
        }

        /// <summary>
        /// Devuelve la 1ra línea (título) del Inform de Diario Detallado
        /// </summary>
        /// <returns></returns>
        private string InformeTitulo(string comp_grupo)
        {
            string result = "";

            try
            {
                result += comp_grupo + " " + this.LP.GetText("lblCabTituloInfoMayorCont", "Mayor detallado") + " ";

                if (this.rbPeriodos.IsChecked)
                {
                    result += " " + this.LP.GetText("lblDelPeriodoMin", "del periodo") + " ";
                    if (this.txtMaskDesdePeriodos.Value.ToString().Length == 5)
                    {
                        result += this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2) + " -  ";
                    }
                    if (this.txtMaskHastaPeriodos.Value.ToString().Length == 5)
                    {
                        result += this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                    }
                }
                else
                    if (this.rbFecha.IsChecked)
                    {
                        result += " " + this.LP.GetText("lblDeFecha", "de fecha") + " ";
                        result += this.dateTimeDesdeFecha.Value.ToShortDateString() + " " + this.LP.GetText("lblAFecha", "a fecha") + " ";
                        result += this.dateTimeHastaFecha.Value.ToShortDateString();
                    }

                result += " " + this.LP.GetText("lblfechaMin", "fecha") + " " + System.DateTime.Now.ToShortDateString();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el Tipo de Dato para la consulta de Saldo
        /// </summary>
        /// <returns></returns>
        private string TipoDato()
        {
            string tipo = "";

            if (this.cmbMoneda.SelectedValue != null)
            {
                switch (this.cmbMoneda.SelectedValue.ToString())
                {
                    case "ML":
                        tipo = "R ";
                        break;
                    case "ME":
                        tipo = "E ";
                        break;
                    case "TI":
                        tipo = "U ";
                        break;
                }
            }

            return (tipo);
        }

        private void EscribirSaldoInicial(string cuenta, string desc, decimal[] saldoInicial)
        {
            try
            {
                comprobante[0] = cuenta;
                comprobante[1] = desc;
                comprobante[2] = "";
                comprobante[3] = "";
                comprobante[4] = "";
                comprobante[5] = "";
                comprobante[6] = this.LP.GetText("lblSaldoIni", "Saldo inicial").ToUpper();
                comprobante[7] = saldoInicial[2].ToString();
                comprobante[8] = "";
                comprobante[9] = "";
                comprobante[10] = "";

                this.InformeHTMLEscribirSaldoInicial(comprobante);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Escribe una línea en blanco en el fichero
        /// </summary>
        private void EscribirLineaEnBlanco()
        {
            for (int i = 0; i <= 10; i++)
            {
                comprobante[i] = "";
            }

            this.InformeHTMLEscribirDetalleComprobante(comprobante);
        }                    

        /// <summary>
        /// Escribe en el fichero html la cabecera del mismo
        /// </summary>
        /// <param name="fila1"></param>
        private void InformeHTMLEscribirEncabezado(string fila1)
        {
            
            documento_HTML.Append("     <title>" + fila1 + "</title>\n");
            documento_HTML.Append("     <style>\n");
            documento_HTML.Append("        .NumeroCG {mso-number-format:\\#\\,\\#\\#0\\.00;text-align=right;}\n");
            documento_HTML.Append("        .NumeroCGLeft {mso-number-format:\"0\";text-align:left; background-color:#D8D8D8}\n");
            documento_HTML.Append("        .NumeroCGSaldoIni {mso-number-format:\\#\\,\\#\\#0\\.00;text-align:right; background-color:#DBDBDB}\n");
            documento_HTML.Append(@"        .Texto    { mso-number-format:\@; }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTIT    { mso-number-format:\@;font-weight:700; background-color:#D8D8D8 }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTITCab    { mso-number-format:\@;font-weight:700; background-color:#C5D9F1}");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTITSaldoIni { mso-number-format:\@;font-weight:700; background-color:#DBDBDB}");
            documento_HTML.Append("\n");

            documento_HTML.Append("     </style>\n");
            documento_HTML.Append(" </head>\n");
            documento_HTML.Append(" <body>\n");
        }

        private void InformeHTMLEscribirCabeceras(string fila1, string[] cabecerasColumnas)
        {
            //documento_HTML.Append("     <b>" + fila1 + "</b><br>\n");

            documento_HTML.Append("     <table width =\"100%\">\n");
            documento_HTML.Append("         <tr>\n");
            documento_HTML.Append("             <td width =\"10%\" colspan=\"11\"><b>" + fila1 + "</b></td>\n");
            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("         <tr>\n");
            string valor = (cabecerasColumnas[0] == "") ? "&nbsp;" : cabecerasColumnas[0];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[1] == "") ? "&nbsp;" : cabecerasColumnas[1];
            documento_HTML.Append("             <td class=TextoTITCab width =\"15%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[2] == "") ? "&nbsp;" : cabecerasColumnas[2];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[3] == "") ? "&nbsp;" : cabecerasColumnas[3];
            documento_HTML.Append("             <td class=TextoTITCab width =\"5%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[4] == "") ? "&nbsp;" : cabecerasColumnas[4];
            documento_HTML.Append("             <td class=TextoTITCab width =\"5%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[5] == "") ? "&nbsp;" : cabecerasColumnas[5];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[6] == "") ? "&nbsp;" : cabecerasColumnas[6];
            documento_HTML.Append("             <td class=TextoTITCab width =\"15%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[7] == "") ? "&nbsp;" : cabecerasColumnas[7];
            documento_HTML.Append("             <td class=TextoTITCab width =\"8%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[8] == "") ? "&nbsp;" : cabecerasColumnas[8];
            documento_HTML.Append("             <td class=TextoTITCab width =\"8%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[9] == "") ? "&nbsp;" : cabecerasColumnas[9];
            documento_HTML.Append("             <td class=TextoTITCab width =\"8%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[10] == "") ? "&nbsp;" : cabecerasColumnas[10];
            documento_HTML.Append("             <td class=TextoTITCab width =\"8%\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("     </table>\n");
        }

        private void InformeHTMLEscribirDetalleComprobante(string[] detalleComprobante)
        {
            bool lineaTotales = false;
            if (detalleComprobante[0] == "" && detalleComprobante[1] == "" && detalleComprobante[3] == "" &&
                detalleComprobante[4] == "" && detalleComprobante[5] == "")
            {
                lineaTotales = true;
            }
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (detalleComprobante[0] == "") ? "&nbsp;" : detalleComprobante[0];
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[1] == "") ? "&nbsp;" : detalleComprobante[1];
            documento_HTML.Append("             <td class=Texto width =\"15%\">" + valor + "</td>\n");
            valor = (detalleComprobante[2] == "") ? "&nbsp;" : detalleComprobante[2];
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[3] == "") ? "&nbsp;" : detalleComprobante[3];
            documento_HTML.Append("             <td class=Texto width =\"5%\">" + valor + "</td>\n");
            valor = (detalleComprobante[4] == "") ? "&nbsp;" : detalleComprobante[4];
            documento_HTML.Append("             <td class=Texto width =\"5%\">" + valor + "</td>\n");
            valor = (detalleComprobante[5] == "") ? "&nbsp;" : detalleComprobante[5];
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[6] == "") ? "&nbsp;" : detalleComprobante[6];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=Texto width =\"15%\">" + valor + "</td>\n");
            valor = (detalleComprobante[7] == "") ? "&nbsp;" : detalleComprobante[7];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"8%\" align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[8] == "") ? "&nbsp;" : detalleComprobante[8];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"8%\" align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[9] == "") ? "&nbsp;" : detalleComprobante[9];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"8%\" align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[10] == "") ? "&nbsp;" : detalleComprobante[10];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"8%\" align=\"right\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            //documento_HTML.Append("     </table>");
        }

        private void InformeHTMLEscribirSaldoInicial(string[] comprobante)
        {
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (comprobante[0] == "") ? "&nbsp;" : comprobante[0];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"10%\">" + valor + "</td>\n");
            valor = (comprobante[1] == "") ? "&nbsp;" : comprobante[1];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"15%\">" + valor + "</td>\n");
            valor = (comprobante[2] == "") ? "&nbsp;" : comprobante[2];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"10%\">" + valor + "</td>\n");
            valor = (comprobante[3] == "") ? "&nbsp;" : comprobante[3];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"5%\">" + valor + "</td>\n");
            valor = (comprobante[4] == "") ? "&nbsp;" : comprobante[4];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"5%\">" + valor + "</td>\n");
            valor = (comprobante[5] == "") ? "&nbsp;" : comprobante[5];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"10%\">" + valor + "</td>\n");
            valor = (comprobante[6] == "") ? "&nbsp;" : comprobante[6];
            documento_HTML.Append("             <td class=TextoTITSaldoIni width =\"15%\">" + valor + "</td>\n");
            valor = (comprobante[7] == "") ? "&nbsp;" : comprobante[7];
            documento_HTML.Append("             <td class=NumeroCGSaldoIni width =\"8%\">" + valor + "</td>\n");
            valor = (comprobante[8] == "") ? "&nbsp;" : comprobante[8];
            documento_HTML.Append("             <td class=NumeroCGSaldoIni width =\"8%\" align=\"right\">" + valor + "</td>\n");
            valor = (comprobante[9] == "") ? "&nbsp;" : comprobante[9];
            documento_HTML.Append("             <td class=NumeroCGSaldoIni width =\"8%\" align=\"right\">" + valor + "</td>\n");
            valor = (comprobante[10] == "") ? "&nbsp;" : comprobante[10];
            documento_HTML.Append("             <td class=NumeroCGSaldoIni width =\"8%\" align=\"right\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            //documento_HTML.Append("     </table>");
        }

        /// <summary>
        /// Ejecutar el informe
        /// </summary>
        private void Ejecutar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                //Verificar que existan datos para el informe
                /*if (this.HayDatosInforme())
                {
                    if (!GlobalVar.UsuarioEnv.ModConsInfo_VisualizarFicheroInformes)
                    {
                        string error = "";
                        string nombreBaseFichero = this.InformeSoloNombreFichero(formCode, GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosInformes, this.txttitulo.Text);
                        //Pedir el nombre del fichero y la ubicación
                        nombreFicheroAGenerar = this.FileDialogFileName(ref error, true, nombreBaseFichero, ref tipoFichero);
                        if (error != "")
                        {
                            RadMessageBox.Show(error);
                            return;
                        }
                    }
                    */
                    backgroundWorker1.RunWorkerAsync();
                //}

                //this.ProcesarInforme();

                //Grabar la petición
                string valores = this.ValoresPeticion();

                this.valoresFormulario.GrabarParametros(formCode, valores);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Grabar la petición
        /// </summary>
        private void GrabarPeticion()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                TGPeticionGrabar frmGrabarPeticion = new TGPeticionGrabar
                {
                    FormCode = this.formCode,
                    FicheroExtension = this.ficheroExtension,
                    FrmPadre = this
                };
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
                FormularioPeticion frmPeticion = new FormularioPeticion
                {
                    Path = System.Configuration.ConfigurationManager.AppSettings["PathFicherosPeticiones"],
                    FormCode = this.formCode,
                    FicheroExtension = this.ficheroExtension,
                    Formulario = this
                };

                DataTable dtPeticiones = frmPeticion.ListarPeticion();

                if (dtPeticiones.Rows.Count > 0)
                {
                    Dictionary<string, string> dictControles = new Dictionary<string, string>
                    {
                        //Falta traducir
                        { "Compañía", "cmbCompania" },
                        { "Grupo de Compañía", "radDropDownListGrupo" },
                        { "Plan", "radDropDownListPlan" },
                        //dictControles.Add("Por Periodos", "rbPeriodos");
                        { "AAPP Desde", "txtMaskDesdePeriodos" },
                        { "AAPP Hasta", "txtMaskHastaPeriodos" },
                        //dictControles.Add("Por Fechas", "rbFecha");
                        { "Fecha Desde", "dateTimeDesdeFecha" },
                        { "Fecha Hasta", "dateTimeHastaFecha" },
                        { "Moneda", "cmbMoneda" },
                        { "Orden Comp.", "cmbOrdenComp" },
                        /*dictControles.Add("Saldo inicial", "chkSaldoInicial");
                        dictControles.Add("Total por cuenta", "chkTotalCuenta");*/
            { "Cuentas Mayor", "lbCuentasMayor" },
                        { "Titulo", "txttitulo" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radButtonTextBoxSelCuentasMayor", "rbPeriodos", "rbFecha", 
                                                                                   "chkSaldoInicial", "chkTotalCuenta", "chkCuentaEditada", "txtDesdePeriodos", "txtHastaPeriodos" });

                    TGPeticionesListar frmListarPeticiones = new TGPeticionesListar
                    {
                        DtPeticiones = dtPeticiones,
                        CentrarForm = true,
                        Headers = dictControles,
                        ColumnNoVisible = columnNoVisible,
                        FrmPadre = this
                    };
                    frmListarPeticiones.OkForm += new TGPeticionesListar.OkFormCommandEventHandler(FrmListarPeticiones_OkForm);

                    frmListarPeticiones.Show();
                }
                else
                {
                    RadMessageBox.Show("No existen peticiones guardadas");    //Falta traducir
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Carga los valores de la última petición del formulario
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                valores = valores.PadRight(444);
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCIMAYCONT myStruct = (StructGLL01_MCIMAYCONT)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCIMAYCONT));

                try
                {
                    if (myStruct.compania.Trim() != "")
                    {
                        this.codigoCompania = myStruct.compania.Trim();
                        try
                        {
                            this.cmbCompania.SelectedValue = this.codigoCompania;
                        }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                        string codPlan = "";
                        string companiaDesc = "";
                        string resultValCodCompania = this.ValidarCompaniaCodPlan(this.codigoCompania, ref companiaDesc, ref codPlan, false);

                        if (resultValCodCompania != "")
                        {
                            string error = this.LP.GetText("errValTitulo", "Error");
                            RadMessageBox.Show(resultValCodCompania, error);
                        }
                        else
                        {
                            this.codigoPlan = codPlan;

                            try
                            {
                                this.radDropDownListPlan.SelectedValue = this.codigoPlan;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.radDropDownListGrupo.SelectedValue = "";
                            this.radDropDownListPlan.Enabled = false;

                            this.gbListaCuentasMayor.Enabled = true;

                            this.cmbCompania.Select();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.grupo.Trim() != "")
                    {
                        string codGrupo = myStruct.grupo.Trim();

                        string grupoDesc = "";
                        string resultValidarGrupo = this.ValidarGrupo(codGrupo, ref grupoDesc, false);

                        if (resultValidarGrupo != "")
                        {
                            string error = this.LP.GetText("errValTitulo", "Error");
                            RadMessageBox.Show(resultValidarGrupo, error);
                            this.codigoGrupo = "";
                        }
                        else
                        {
                            this.codigoGrupo = codGrupo;
                            try
                            {
                                this.radDropDownListGrupo.SelectedValue = this.codigoGrupo;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.FillPlanes(this.codigoGrupo);
                        }

                        this.radDropDownListGrupo.Select();
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.compania.Trim() == "" && myStruct.plan.Trim() != "")
                    {
                        string codPlan = myStruct.plan.Trim();

                        string descPlan = "";
                        string resultValidarPlan = this.ValidarPlan(codPlan, ref descPlan);

                        if (resultValidarPlan == "")
                        {
                            try
                            {
                                this.radDropDownListPlan.SelectedValue = this.codigoPlan;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.gbListaCuentasMayor.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.porFechaporPeriodo == "1")
                {
                    this.rbFecha.IsChecked = true;
                    DateTime fecha = new DateTime();

                    try
                    {
                        fecha = Convert.ToDateTime(myStruct.fechaDesde);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        fecha = DateTime.Today; 
                    }
                    this.dateTimeDesdeFecha.Value = fecha;

                    try
                    {
                        fecha = Convert.ToDateTime(myStruct.fechaHasta);
                    }
                    catch(Exception ex) 
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        fecha = DateTime.Today; 
                    }
                    this.dateTimeHastaFecha.Value = fecha;

                    this.rbPeriodos.IsChecked = false;
                    this.txtMaskDesdePeriodos.Value = "";
                    this.txtMaskHastaPeriodos.Value = "";
                }
                else
                {
                    this.rbFecha.IsChecked = false;
                    DateTime fecha = new DateTime();

                    try
                    {
                        fecha = DateTime.Today;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.dateTimeDesdeFecha.Value = fecha;
                    this.dateTimeHastaFecha.Value = fecha;

                    this.rbPeriodos.IsChecked = true;
                    this.txtMaskDesdePeriodos.Value = myStruct.periodoDesde.Trim();
                    this.txtMaskHastaPeriodos.Value = myStruct.periodoHasta.Trim();
                }

                try
                {
                    if (myStruct.moneda.Trim() != "") this.cmbMoneda.SelectedValue = myStruct.moneda.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.ordenComp.Trim() != "") this.cmbOrdenComp.SelectedValue = myStruct.ordenComp.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.saldoInicial == "1") this.chkSaldoInicial.Checked = true;
                else this.chkSaldoInicial.Checked = false;

                if (myStruct.totalCuenta == "1") this.chkTotalCuenta.Checked = true;
                else this.chkTotalCuenta.Checked = false;

                if (myStruct.cuentaEditada == "1") this.chkCuentaEditada.Checked = true;
                else this.chkCuentaEditada.Checked = false;

                this.txttitulo.Text = myStruct.titulo.Trim();

                //Cuentas de mayor 
                if (myStruct.cuentasMayor.Trim() != "")
                {
                    string cuentasMayor = myStruct.cuentasMayor.Trim();
                    string[] aCuentasMayor = cuentasMayor.Split(';');
                    this.lbCuentasMayor.Items.Clear();
                    string cuenta = "";
                    for (int i = 0; i < aCuentasMayor.Length; i++)
                    {
                        cuenta = aCuentasMayor[i].Trim();
                        if (cuenta != "") this.lbCuentasMayor.Items.Add(aCuentasMayor[i]);
                    }
                }

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
                StructGLL01_MCIMAYCONT myStruct;

                string codigo = "";
                if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString().Trim() != "")
                {
                    myStruct.compania = codigoCompania.PadRight(2, ' ');
                }
                else myStruct.compania = codigo.PadRight(2, ' ');

                if (this.radDropDownListGrupo.SelectedValue != null && this.radDropDownListGrupo.SelectedValue.ToString().Trim() != "")
                {
                    myStruct.grupo = codigoGrupo.PadRight(2, ' ');
                }
                else myStruct.grupo = codigo.PadRight(2, ' ');

                myStruct.plan = codigoPlan.PadRight(2, ' ');

                if (this.rbFecha.IsChecked) myStruct.porFechaporPeriodo = "1";
                else myStruct.porFechaporPeriodo = "0";

                if (this.rbFecha.IsChecked)
                {
                    myStruct.fechaDesde = this.dateTimeDesdeFecha.Value.ToShortDateString().PadRight(10, ' ');
                    myStruct.fechaHasta = this.dateTimeHastaFecha.Value.ToShortDateString().PadRight(10, ' ');
                    codigo = "";
                    myStruct.periodoDesde = codigo.PadRight(5, ' ');
                    myStruct.periodoHasta = codigo.PadRight(5, ' ');

                }
                else
                {
                    myStruct.periodoDesde = this.txtMaskDesdePeriodos.Value.ToString().PadRight(5, ' ');
                    myStruct.periodoHasta = this.txtMaskHastaPeriodos.Value.ToString().PadRight(5, ' ');
                    codigo = "";
                    myStruct.fechaDesde = codigo.PadRight(10, ' ');
                    myStruct.fechaHasta = codigo.PadRight(10, ' ');
                }

                myStruct.moneda = this.cmbMoneda.SelectedValue.ToString();

                myStruct.ordenComp = this.cmbOrdenComp.SelectedValue.ToString();

                if (this.chkSaldoInicial.Checked) myStruct.saldoInicial= "1";
                else myStruct.saldoInicial = "0";

                if (this.chkTotalCuenta.Checked) myStruct.totalCuenta = "1";
                else myStruct.totalCuenta = "0";

                myStruct.titulo = this.txttitulo.Text.PadRight(100, ' ');
                myStruct.titulo = myStruct.titulo.Replace("'", "''");

                //Cuenta Editada
                if (this.chkCuentaEditada.Checked) myStruct.cuentaEditada = "1";
                else myStruct.cuentaEditada = "0";

                //cuentas de mayor
                myStruct.cuentasMayor = "";
                for (int i = 0; i < this.lbCuentasMayor.Items.Count; i++)
                {
                    myStruct.cuentasMayor += this.lbCuentasMayor.Items[i] + ";";
                }

                if (myStruct.cuentasMayor.Length > 300) myStruct.cuentasMayor = myStruct.cuentasMayor.Substring(0, 299);

                result = myStruct.compania + myStruct.grupo + myStruct.plan + myStruct.porFechaporPeriodo + myStruct.fechaDesde + myStruct.fechaHasta;
                result += myStruct.periodoDesde + myStruct.periodoHasta + myStruct.moneda + myStruct.ordenComp + myStruct.saldoInicial;
                result += myStruct.totalCuenta + myStruct.cuentaEditada + myStruct.titulo + myStruct.cuentasMayor;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIMAYCONT));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //if (result.Length < 145) result.PadRight(145, ' ');

            return (result);
        }
        #endregion
    }
}