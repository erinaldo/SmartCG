using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using ObjectModel;
using System.Diagnostics;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.Svg;

namespace ModConsultaInforme
{
    public partial class frmInfoBalanceSumSaldosAltaEdita : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCIBASUMSA";
        public string formCodeNombFichero = "BASUMSA";
        public string ficheroExtension = "bss";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCIBASUMSA
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string grupo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string plan;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string monedaLocal;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string subtotalesJerarq;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string nivel;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string consolidado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string cuentaEditada;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string titulo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
            public string cuentasMayor;
        } 

        private string tipoFichero = "EXCEL";

        string[] comprobante = new string[6];
        string primeraFila = "";
        string[] columnas;

        int nivelPlanCuentas;
        int nivelInforme;
        string tipoDato = "";
        //string calendario = "";

        bool primeraFilaEscrita = false;

        private decimal totalSaldoInicial;
        private decimal totalSaldoDebe;
        private decimal totalSaldoHaber;
        private decimal totalSaldoFinal;

        ArrayList aEmpresas;
        ArrayList cuentasAProcesar;

        ArrayList matriz;

        private string codigoCompania = "";
        private string descCompania = "";
        private string codigoGrupo = "";
        private string descGrupo = "";
        private string codigoPlan = "";
        private string descPlan = "";

        StringBuilder documento_HTML;

        FormularioValoresCampos valoresFormulario;

        private ArrayList avisosAutorizaciones;

        private bool cargarPlanes = false;

        private string nombreFicheroAGenerar = "";

        private string mensajeProceso = "";

        public frmInfoBalanceSumSaldosAltaEdita()
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

        private void FrmInfoBalanceSumSaldosAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Balance Sumas y Saldos");

            //Obtener el tipo de fichero
            string tipoFicherosInformes = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosInformes"];
            if (tipoFicherosInformes != null && tipoFicherosInformes != "") if (tipoFicherosInformes == "HTML") tipoFichero = tipoFicherosInformes;

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
            
            //Llenar los niveles
            this.FillNiveles();

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
                this.radButtonTextBoxSelCuentasMayor.Select();
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
            }
            else this.lbCuentasMayor.Select();
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

                    this.gbListaCuentasMayor.Enabled = true;

                    this.chkConsolidado.Checked = false;
                    this.chkConsolidado.Enabled = false;
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

                    this.lbCuentasMayor.Items.Clear();
                    this.gbListaCuentasMayor.Enabled = false;
                    this.chkConsolidado.Checked = false;
                    this.chkConsolidado.Enabled = false;
                }
                else
                {
                    this.cmbCompania.SelectedValue = "";

                    this.FillPlanes(this.codigoGrupo);
                    this.cargarPlanes = true;
                    this.radDropDownListPlan.Enabled = true;

                    this.codigoPlan = "";
                    this.lbCuentasMayor.Items.Clear();
                    this.gbListaCuentasMayor.Enabled = false;
                    this.chkConsolidado.Enabled = true;

                    this.radDropDownListPlan.Select();
                }
            }
            Cursor.Current = Cursors.Default;
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

        private void RadDropDownListPlan_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.radDropDownListPlan.SelectedValue != null && this.radDropDownListPlan.SelectedValue.ToString() != "")
            {
                string codigo = this.radDropDownListPlan.SelectedValue.ToString();

                if (codigo != "" && codigo.Length >= 1)
                {
                    string descPlan = "";
                    string result = this.ValidarPlan(codigo, ref descPlan);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radDropDownListPlan.Focus();
                    }
                    else
                    {
                        this.codigoPlan = codigo;
                        //this.radDropDownListPlan.SelectedValue = this.codigoPlan;

                        this.gbListaCuentasMayor.Enabled = true;

                        this.txtMaskDesdePeriodos.Focus();
                    }
                }
                else
                {
                    this.radDropDownListPlan.Focus();
                }
            }
            
            Cursor.Current = Cursors.Default;
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

        private void FrmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion
            {
                FormCode = this.formCode,
                Formulario = this
            };
            _ = frmPeticion.CargarPeticionDataTable(((DataTable)e.Valor));

            if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString().Trim() != "") this.radDropDownListPlan.Enabled = false;
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
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", "Balance de Sumas y Saldos", MessageBoxButtons.YesNo, RadMessageIcon.Question);

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

        private void FrmInfoBalanceSumSaldosAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Balance Sumas y Saldos");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemBalanceSumSaldos", "Balance Sumas y Saldos");
            this.Text = this.Text.Replace("&", "");
            //Menú
            //this.lblDesdePeriodo.Text = this.LP.GetText("lblPeriodoDesde", "Período Desde");
            //this.lblHastaPeriodo.Text = this.LP.GetText("lblPeriodoHasta", "Período Hasta");
            this.chkMonedaLocal.Text = this.LP.GetText("lblMonedaLocal", "Moneda Local");
            this.chkSubtotales.Text = this.LP.GetText("lblSubtJerarq", "Subtotales Jerárquicos");
            //this.lblNivel.Text = this.LP.GetText("lblNivel", "Nivel de Cuenta");
            this.chkConsolidado.Text = this.LP.GetText("lblConsolidado", "Consolidado");
            this.gbListaCuentasMayor.Text = this.LP.GetText("lblCuentasMayor", "Lista de Cuentas de Mayor");
            this.btnAddCuentaMayor.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnQuitarCuentaMayor.Text = this.LP.GetText("lblQuitar", "Quitar");

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Llena el desplegable de niveles
        /// </summary>
        private void FillNiveles()
        {
            var items = new BindingList<KeyValuePair<string, string>>();

            for (int i = 1; i <= 9; i++)
            {
                items.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }

            this.cmbNivel.DataSource = items;
            this.cmbNivel.ValueMember = "Key";
            this.cmbNivel.DisplayMember = "Value";

            this.cmbNivel.SelectedIndex = 8;
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
                else
                {
                    RadMessageBox.Show("Debe serleccionar un plan", error);
                    this.radDropDownListGrupo.Select();
                    return (false);
                }
            }

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

            //string resultMsg = this.ValidarPeriodo(ref this.txtMaskHastaPeriodos);
            this.txtMaskHastaPeriodos.Value = this.txtMaskHastaPeriodos.Value.ToString().PadRight(5, '0');
            this.txtHastaPeriodos.Text = this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
            resultMsg = this.ValidarPeriodo(ref this.txtHastaPeriodos);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtHastaPeriodos.Focus();
                return (false);
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

        private void ProcesarInforme()
        {
            this.mensajeProceso = "";

            //Iniciar la barra de progreso
            this.backgroundWorker1.ReportProgress(0);

            //Mover la barra de progreso
            this.backgroundWorker1.ReportProgress(2);

            string[] codDes = new string[2];

            //Eliminar los posibles permisos de no autorizado del usuario conectado
            GlobalVar.UsuarioEnv.ListaNoAutorizado.Clear();

            //Inicializar los avisos de autorizaciones
            this.avisosAutorizaciones = new ArrayList();

            if (this.radDropDownListGrupo.SelectedValue != null && this.radDropDownListGrupo.SelectedValue.ToString().Trim() != "")
            {
                //if (codigoGrupo != "" && codigoPlan != "")
                if (codigoGrupo != "")
                {
                    //Buscar las empresas del grupo
                    aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(codigoGrupo, codigoPlan);
                    //calendario = "";
                    descCompania = descGrupo;
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
                //calendario = datosCompaniaAct[0];
            }

            //Crear el fichero en memoria (Excel o HTML)
            this.InformeHTMLCrear(ref this.documento_HTML);

            //Cabecera de las columnas del informe
            columnas = this.ObtenerCabecerasColumnasInforme();

            //Tipo de dato
            if (this.chkMonedaLocal.Checked) this.tipoDato = "R ";
            else this.tipoDato = "E ";

            if (this.codigoPlan != "")
            {
                //Buscar el nivel para el informe
                nivelInforme = this.ObtenerNivelInforme(codigoPlan);

                //Buscar las cuentas
                this.ObtenerCuentas(this.codigoPlan);

                if (this.cuentasAProcesar.Count <= 0) 
                {
                    //Finaliza la barra de progreso
                    //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
                    this.backgroundWorker1.ReportProgress(1);

                    //RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado"));
                    this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");
                    return;
                }
            }
            
            string[] datosCompania;

            if (!this.chkConsolidado.Checked)
            {
                string[] datosCompAct;

                //Escribir 1ra fila y las Cabeceras de las columnas
                primeraFila = this.InformeTitulo();
                this.EscribirTituloInforme();

                for (int i = 0; i < aEmpresas.Count; i++)
                {
                    matriz = new ArrayList();

                    datosCompania = (string[])aEmpresas[i];
                    descCompania = datosCompania[1];

                    this.totalSaldoInicial = 0;
                    this.totalSaldoDebe = 0;
                    this.totalSaldoHaber = 0;
                    this.totalSaldoFinal = 0;

                    string codPlan;
                    if (this.codigoPlan == "" && this.codigoGrupo != "")
                    {
                        //Buscar el plan y el calendario de la compañía
                        datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(datosCompania[0].ToString());
                        //calendario = datosCompAct[0];
                        codPlan = datosCompAct[1];

                        //Buscar el nivel para el informe
                        nivelInforme = this.ObtenerNivelInforme(codPlan);

                        if (i > 0) cuentasAProcesar.Clear();

                        //Buscar las cuentas
                        this.ObtenerCuentas(codPlan);
                    }
                    else codPlan = this.codigoPlan;

                    this.ProcesarBalanceSumasSaldosCompania(datosCompania[0].ToString(), codPlan);
                }
            }
            else
            {
                matriz = new ArrayList();
                //Escribir titulo del informe y las cabeceras
                primeraFila = this.InformeTitulo();
                descCompania = this.LP.GetText("lblTitConsolidadoCias", "CONSOLIDADO CIAS") + " " + descGrupo;
                this.ProcesarBalanceSumasSaldosCompania("", this.codigoPlan);
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
                //Finaliza la barra de progreso
                this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                //this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
            }
            else
            {
                //Finaliza la barra de progreso
                this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                //levantar html con excel
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
                    //this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
                }
                // fin levantar
            }*/
        }

        /// <summary>
        /// Obtener el nivel para el informe
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private int ObtenerNivelInforme(string plan)
        {
            //Obtener el nivel y la longitud del plan de cuentas
            int[] nivelLongitud = utilesCG.ObtenerNivelLongitudDadoPlan(plan);
            nivelPlanCuentas = nivelLongitud[0];

            int nivelFormulario = Convert.ToInt16(this.cmbNivel.SelectedValue);
            int niv;
            if (nivelFormulario > nivelPlanCuentas) niv = nivelPlanCuentas;
            else niv = nivelFormulario;

            return (niv);
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
                query += " where ";

                if (this.chkSubtotales.Checked) query += "NIVEMC <= " + this.nivelInforme;
                else query += "NIVEMC = " + this.nivelInforme;

                if (codPlan != "") query += " and TIPLMC='" + codPlan + "' ";

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
                string grctmx = "";

                string aviso = "";
                
                bool chequearAut = (this.lbCuentasMayor.Items.Count <= 0);
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

                            if (sconmc != "0" && (ultNivelConsultaCtaAut == -1 || nivelConsultaCtaActual > ultNivelConsultaCtaAut))
                            {
                                //Verificar autorizaciones
                                autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "003", "02", "0" + sconmc, codPlan);
                                if (autCuenta) ultNivelConsultaCtaAut = nivelConsultaCtaActual;
                            }
                            else autCuenta = true;
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
                        }
                        else
                        {
                            aviso = "Usuario no autorizado para algunas cuentas de mayor";  //Falta traducir
                            if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                        }
                    }

                    if (autCuenta)
                    {
                        datos = new string[3];
                        datos[0] = dr.GetValue(dr.GetOrdinal("CUENMC")).ToString().Trim();
                        datos[1] = dr.GetValue(dr.GetOrdinal("TCUEMC")).ToString().Trim();
                        datos[2] = dr.GetValue(dr.GetOrdinal("NIVEMC")).ToString().Trim();
                        cuentasAProcesar.Add(datos);
                    }

                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;
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
        /// Devuelve las cabeceras de las columnas del informe en el idioma correspondiente
        /// </summary>
        /// <returns></returns>
        private string[] ObtenerCabecerasColumnasInforme()
        {
            //Cabecera de las columnas del informe
            string[] columnas = new string[6];
            columnas[0] = this.LP.GetText("lblCabCuenta", "Cuenta");
            columnas[1] = this.LP.GetText("lblCabNombreCuenta", "Nombre de la cuenta");
            columnas[2] = this.LP.GetText("lblCabSaldoAnterior", "Saldo Anterior");
            columnas[3] = this.LP.GetText("lblCabDebe", "DEBE");
            columnas[4] = this.LP.GetText("lblCabHaber", "HABER");
            columnas[5] = this.LP.GetText("lblCabSaldoActual", "Saldo Actual");

            return (columnas);
        }

        /// <summary>
        /// Devuelve la 1ra línea (título) del Informe de Diario Detallado
        /// </summary>
        /// <returns></returns>
        private string InformeTitulo()
        {
            string result = "";

            try
            {
                result += this.LP.GetText("lblCabTituloInfoBalanceSumSaldo", "Balance de comprobación del periodo") + " ";

                if (this.txtMaskDesdePeriodos.Value.ToString().Length == 5)
                {
                    result += this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2) + " -  ";
                }
                if (this.txtMaskHastaPeriodos.Value.ToString().Length == 5)
                {
                    result += this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Porcesar el informe para una compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        private void ProcesarBalanceSumasSaldosCompania(string codigo, string codPlan)
        {
            try
            {
                int registros = 0;
                decimal[] saldos = { 0, 0, 0, 0 };
                string[] datos;

                int nivelActual = 0;
                int nivelAnterior = 0;

                DatosCuenta datosCuentaActual;
                bool acumular = false;

                string cuentaCaracteresInicio = "";

                for (int i = 0; i < cuentasAProcesar.Count; i++)
                {
                    //Mover la barra de progreso
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;
                    this.backgroundWorker1.ReportProgress(2);

                    if (registros == 0)
                    {
                        this.EscribirTituloInforme();
                        /*DEL if (tipoFichero == "HTML")*/
                        this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                    }

                    datos = (string[])cuentasAProcesar[i];
                    nivelActual = Convert.ToInt16(datos[2]);

                    if (registros == 0) cuentaCaracteresInicio = datos[0];
                    else
                    if (cuentaCaracteresInicio != "")
                    {
                        if (cuentaCaracteresInicio.Length <= datos[0].Length)
                            if (cuentaCaracteresInicio != datos[0].Substring(0, cuentaCaracteresInicio.Length))
                            {
                                //Escribir la matriz
                                //this.EscribirTodaMatriz();

                                //Escribir cuentas del nivel anterior
                                this.EscribirCuentas(nivelActual - 1);

                                if (this.matriz.Count > 0)
                                {
                                    //Escribir ultimas cuentas en el fichero
                                    this.EscribirUltimasCuentas();
                                }

                                cuentaCaracteresInicio = datos[0];
                            }
                    }

                    if (nivelAnterior != 0 && nivelActual < nivelAnterior)
                    {
                        //Escribir cuentas en el fichero
                        this.EscribirCuentas(nivelActual);
                        nivelAnterior = nivelActual;
                    }

                    if (nivelActual == nivelInforme)
                    {
                        //Buscar los saldos para la cuenta porque estamos a último nivel
                        if (!this.chkConsolidado.Checked)
                        {
                            saldos = this.ObtenerSaldosCuenta(codigo, datos[0], codPlan);
                        }
                        else
                        {
                            saldos = this.ObtenerSaldosCuentaTodasCompanias(datos[0], codPlan);
                        }
                        acumular = true;
                    }
                    else
                    {
                        saldos[0] = 0;
                        saldos[1] = 0;
                        saldos[2] = 0;
                        acumular = false;
                    }

                    ArrayList cuentaDatos;
                    cuentaDatos = utilesCG.ObtenerDatosCuenta(datos[0], codPlan);

                    datosCuentaActual = new DatosCuenta
                    {
                        Cuenta = datos[0],
                        CuentaEdit = cuentaDatos[1].ToString(),
                        Tipo = datos[1],
                        CuentaNombre = cuentaDatos[0].ToString(),
                        Nivel = nivelActual,
                        SaldoInicial = saldos[0],
                        SaldoDebe = saldos[1],
                        SaldoHaber = saldos[2]
                    };
                    matriz.Add(datosCuentaActual);

                    if (acumular) this.AcumularSaldosMatriz(nivelActual);

                    nivelAnterior = nivelActual;
                    registros++;
                }

                if (this.matriz.Count > 0)
                {
                    //Escribir cuentas del nivel anterior
                    this.EscribirCuentas(nivelActual-1);

                    if (this.matriz.Count > 0)
                    {
                        //Escribir ultimas cuentas en el fichero
                        this.EscribirUltimasCuentas();
                    }
                }

                //Escribir linea de totales
                this.EscribirLineaTotales();

                /*DEL if (tipoFichero == "HTML")*/
                this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void AcumularSaldosMatriz(int nivelActual)
        {
            DatosCuenta cuentaActual = (DatosCuenta)matriz[matriz.Count -1];
            decimal saldoInicial = cuentaActual.SaldoInicial;
            decimal saldoDebe = cuentaActual.SaldoDebe;
            decimal saldoHaber = cuentaActual.SaldoHaber;

            DatosCuenta cuentaAux;
            for (int i = matriz.Count - 2; i >= 0; i--)
            {
                cuentaAux = (DatosCuenta)matriz[i];
                if (cuentaAux.Nivel < nivelActual)
                {
                    cuentaAux.SaldoInicial += saldoInicial;
                    cuentaAux.SaldoDebe += saldoDebe;
                    cuentaAux.SaldoHaber += saldoHaber;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nivelActual"></param>
        private void EscribirCuentas(int nivelActual)
        {
            DatosCuenta cuentaActual;
            ArrayList comprob = new ArrayList();

            int i = matriz.Count;
            int nivelAux;
            string[] comprobanteAux;
            while (i > 0)
            {
                cuentaActual = (DatosCuenta)matriz[i-1];
                nivelAux = cuentaActual.Nivel;

                if (nivelAux >= nivelActual)
                {
                    if ((nivelAux == nivelInforme) || (nivelAux < nivelInforme && cuentaActual.Tipo != "D"))
                    {
                        if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                        {
                            //Crea el comprobante
                            comprobanteAux = this.CreaComprobanteDadoCuenta(cuentaActual);

                            if (cuentaActual.Tipo == "D") comprob.Add(comprobanteAux);
                            else comprob.Insert(0, comprobanteAux);
                        }
                    }

                    //Eliminar cuenta de la matriz
                    matriz.RemoveAt(i - 1);

                    i--;
                }
                else break;
            }

            if (comprob.Count > 0)
            {
                for (int j = comprob.Count-1; j >= 0; j--)
                {
                    comprobante = (string[])comprob[j];
                    //Escribe la línea del detalle del comprobante en el fichero HTML
                    this.InformeHTMLEscribirDetalleComprobante(comprobante);
                }
            }
        }

        /// <summary>
        /// Escribe las cuentas que están pendientes después del proceso EscribirCuentas
        /// </summary>
        private void EscribirUltimasCuentas()
        {
            DatosCuenta cuentaActual;

            if (this.chkSubtotales.Checked)
            {
                for (int i = matriz.Count; i > 0; i--)
                {
                    cuentaActual = (DatosCuenta)matriz[i - 1];
                    if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                    {
                        this.EscribirCuentaFichero(cuentaActual);
                    }
                }
            }
            else
            {
                for (int i = 0; i < matriz.Count; i++)
                {
                    cuentaActual = (DatosCuenta)matriz[i];
                    if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                    {
                        this.EscribirCuentaFichero(cuentaActual);
                    }
                }
            }

            matriz.Clear();
        }

        /// <summary>
        /// Escribe las cuentas que están pendientes después del proceso EscribirCuentas
        /// </summary>
        private void EscribirTodaMatriz()
        {
            DatosCuenta cuentaActual;

            if (matriz.Count == 1)
            {
                cuentaActual = (DatosCuenta)matriz[0];
                if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                {
                    this.EscribirCuentaFichero(cuentaActual);
                }
            }
            else
            {
                for (int i = 1; i < matriz.Count; i++)
                {
                    cuentaActual = (DatosCuenta)matriz[i];
                    if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                    {
                        this.EscribirCuentaFichero(cuentaActual);
                    }
                }
                cuentaActual = (DatosCuenta)matriz[0];
                if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                {
                    this.EscribirCuentaFichero(cuentaActual);
                }
            }

            matriz.Clear();

            /*
            if (this.chkSubtotales.Checked)
            {
                for (int i = matriz.Count; i > 0; i--)
                {
                    cuentaActual = (DatosCuenta)matriz[i - 1];
                    if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                    {
                        this.EscribirCuentaFichero(cuentaActual);
                    }
                }
            }
            else
            {
                for (int i = 0; i < matriz.Count; i++)
                {
                    cuentaActual = (DatosCuenta)matriz[i];
                    if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
                    {
                        this.EscribirCuentaFichero(cuentaActual);
                    }
                }
            }

            matriz.Clear();*/
        }

        /// <summary>
        /// Crea la estructura con los datos de la cuenta que se imprimirá
        /// </summary>
        /// <param name="cuentaActual"></param>
        /// <returns></returns>
        private string[] CreaComprobanteDadoCuenta(DatosCuenta cuentaActual)
        {
            string[] comprobanteAux = new string[6];
            try
            {
                if (chkCuentaEditada.Checked == true)
                    comprobanteAux[0] = cuentaActual.CuentaEdit;  //Cuenta Formateada
                else
                    comprobanteAux[0] = cuentaActual.Cuenta;  //Cuenta No Formateada
                comprobanteAux[1] = cuentaActual.CuentaNombre;  //nombre cuenta
                comprobanteAux[2] = cuentaActual.SaldoInicial.ToString();
                comprobanteAux[3] = cuentaActual.SaldoDebe.ToString();
                comprobanteAux[4] = cuentaActual.SaldoHaber.ToString();
                comprobanteAux[5] = (cuentaActual.SaldoInicial + cuentaActual.SaldoDebe + cuentaActual.SaldoHaber).ToString();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (comprobanteAux);
        }

        /// <summary>
        /// Escribe una línea con los datos de la cuenta en el fichero html
        /// </summary>
        /// <param name="cuentaActual"></param>
        private void EscribirCuentaFichero(DatosCuenta cuentaActual)
        {
            if (!(cuentaActual.SaldoInicial == 0 && cuentaActual.SaldoDebe == 0 && cuentaActual.SaldoHaber == 0))
            {
                //Imprimir cuenta, si al menos existe un saldo
                comprobante = this.CreaComprobanteDadoCuenta(cuentaActual);
            
                //Escribe la línea del detalle del comprobante en el fichero HTML
                this.InformeHTMLEscribirDetalleComprobante(comprobante);
            }
        }

        /// <summary>
        /// Escribe el encabezado del informe
        /// </summary>
        private void EscribirTituloInforme()
        {
            if (!this.primeraFilaEscrita)
            {
                //Escribir el encabezado para la compañía
                this.InformeHTMLEscribirCabeceras(primeraFila, columnas);
                this.primeraFilaEscrita = true;
            }
            else
            {
                if (this.radDropDownListGrupo.SelectedValue != null && this.radDropDownListGrupo.SelectedValue.ToString().Trim() != "")
                {
                    this.InformeHTMLEscribirCompania(descCompania);
                }
            }
        }

        /// <summary>
        /// Procesa todos los comprobantes de una cuenta de mayor
        /// </summary>
        /// <param name="codigoCompania">código de compañía</param>
        /// <param name="cuenta">código de la cuenta</param>
        /// <param name="plan">código del plan</param>
        private decimal[] ObtenerSaldosCuenta(string codigoCompania, string cuenta, string plan)
        {
            decimal[] result = { 0, 0, 0, 0 };
            try
            {
                //Mover la barra de progreso
                //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;
                this.backgroundWorker1.ReportProgress(2);

                string aa = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2);
                string siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                //string sigloanoPeriodoDesde = siglo + this.txtDesdePeriodos.Text;
                string sigloanoPeriodoDesde = siglo + this.txtMaskDesdePeriodos.Value.ToString().Substring(0,2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2);
                string sigloanoDesde = sigloanoPeriodoDesde.Substring(0, 3);

                aa = this.txtHastaPeriodos.Text.Substring(0, 2);
                siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                //string sigloanoPeriodoHasta = siglo + this.txtHastaPeriodos.Text;
                string sigloanoPeriodoHasta = siglo + this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                string sigloanoHasta = sigloanoPeriodoHasta.Substring(3, 2);

                //Buscar SaldoInicial
                decimal[] saldoInicial = { 0, 0, 0 };

                int sigloanoPeriodoDesdeAnterior = Convert.ToInt32(sigloanoPeriodoDesde) - 1;
                string sapr = sigloanoPeriodoDesdeAnterior.ToString();
                if (sapr.Length < 5) sapr = sapr.PadLeft(5, '0');
                saldoInicial = utilesCG.ObtenerSaldo(codigoCompania, plan, "00000", sapr, cuenta, this.tipoDato, "", "");

                decimal[] saldoPeriodo = { 0, 0, 0 };
                //Buscar Saldo periodo
                saldoPeriodo = utilesCG.ObtenerSaldo(codigoCompania, plan, sigloanoPeriodoDesde, sigloanoPeriodoHasta, cuenta, this.tipoDato, " ", " ");

                result[0] = saldoInicial[2];
                result[1] = saldoPeriodo[0];
                result[2] = saldoPeriodo[1];
                result[3] = saldoInicial[2] + saldoPeriodo[2];

                totalSaldoInicial += saldoInicial[2];
                totalSaldoDebe += saldoPeriodo[0];
                totalSaldoHaber += saldoPeriodo[1];
                totalSaldoFinal += saldoInicial[2] + saldoPeriodo[2];

            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private decimal[] ObtenerSaldosCuentaTodasCompanias(string cuenta, string plan)
        {
            decimal[] result = { 0, 0, 0, 0 };
            decimal[] saldos = { 0, 0, 0, 0 };
            try
            {
                string[] datosCompania;
                //Calcular el saldo de la cuenta para todas las compañías del grupo
                for (int i = 0; i < aEmpresas.Count; i++)
                {
                    datosCompania = (string[])aEmpresas[i];
                    saldos = this.ObtenerSaldosCuenta(datosCompania[0].ToString(), cuenta, plan);

                    result[0] += saldos[0];
                    result[1] += saldos[1];
                    result[2] += saldos[2];
                    result[3] += saldos[3];
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Escribe una línea de Totales
        /// </summary>
        private void EscribirLineaTotales()
        {
            comprobante[0] = this.LP.GetText("lblCabTotal", "TOTAL");
            comprobante[1] = "";
            comprobante[2] = totalSaldoInicial.ToString();
            comprobante[3] = totalSaldoDebe.ToString();
            comprobante[4] = totalSaldoHaber.ToString();
            comprobante[5] = totalSaldoFinal.ToString();

            //Escribe la línea del detalle del comprobante en el fichero HTML
            this.InformeHTMLEscribirDetalleComprobante(comprobante);
        }

        private void InformeHTMLEscribirCabeceras(string fila1, string[] cabecerasColumnas)
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
            documento_HTML.Append("     <b>" + fila1 + "</b><br>\n");

            documento_HTML.Append("     <b>" + descCompania + "</b>\n");

            documento_HTML.Append("     <table width =\"100%\">\n");
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
            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("     </table>\n");
        }

        private void InformeHTMLEscribirCompania(string desc)
        {
            documento_HTML.Append("     <b>" + desc + "</b>\n");
         }

        private void InformeHTMLEscribirDetalleComprobante(string[] detalleComprobante)
        {
            bool lineaTotales = false;
            if (detalleComprobante[0] == this.LP.GetText("lblCabTotal", "TOTAL") && detalleComprobante[1] == "") lineaTotales = true;
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (detalleComprobante[0].Trim() == "") ? "&nbsp;" : detalleComprobante[0];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[1].Trim() == "") ? "&nbsp;" : detalleComprobante[1];
            documento_HTML.Append("             <td class=Texto width =\"15%\">" + valor + "</td>\n");
            valor = (detalleComprobante[2] == "") ? "&nbsp;" : detalleComprobante[2];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[3] == "") ? "&nbsp;" : detalleComprobante[3];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"5%\">" + valor + "</td>\n");
            valor = (detalleComprobante[4] == "") ? "&nbsp;" : detalleComprobante[4];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"5%\">" + valor + "</td>\n");
            valor = (detalleComprobante[5] == "") ? "&nbsp;" : detalleComprobante[5];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"10%\">" + valor + "</td>\n");
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
                primeraFilaEscrita = false;
                cuentasAProcesar = new ArrayList();
                aEmpresas = new ArrayList();
                totalSaldoInicial = 0;
                totalSaldoDebe = 0;
                totalSaldoHaber = 0;
                totalSaldoFinal = 0;

                backgroundWorker1.RunWorkerAsync();
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
                        { "AAPP Desde", "txtMaskDesdePeriodos" },
                        { "AAPP Hasta", "txtMaskHastaPeriodos" },
                        { "Nivel", "cmbNivel" },
                        { "Cuentas Mayor", "lbCuentasMayor" },
                        { "Titulo", "txttitulo" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radButtonTextBoxSelCuentasMayor", "chkMonedaLocal", "chkSubtotales",  
                                                                                   "chkConsolidado", "chkCuentaEditada", "txtDesdePeriodos", "txtHastaPeriodos" });

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
        /// Actualiza los controles del formulario con los valores de la última petición
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                valores = valores.PadRight(419);
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCIBASUMSA myStruct = (StructGLL01_MCIBASUMSA)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCIBASUMSA));

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
                        string resultValCodCompania = this.ValidarCompaniaCodPlan(this.codigoCompania, ref companiaDesc, ref codPlan, true);

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

                            this.cmbCompania.Select();
                        }

                        this.chkConsolidado.Checked = false;
                        this.chkConsolidado.Enabled = false;
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

                        //this.chkConsolidado.Checked = false;
                        //this.chkConsolidado.Enabled = false;
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

                            this.chkConsolidado.Enabled = true;

                            this.gbListaCuentasMayor.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                this.txtMaskDesdePeriodos.Value = myStruct.periodoDesde.Trim();
                this.txtMaskHastaPeriodos.Value = myStruct.periodoHasta.Trim();

                if (myStruct.monedaLocal == "1") this.chkMonedaLocal.Checked = true;
                else this.chkMonedaLocal.Checked = false;

                if (myStruct.subtotalesJerarq == "1") this.chkSubtotales.Checked = true;
                else this.chkSubtotales.Checked = false;

                try
                {
                    if (myStruct.nivel.Trim() != "") this.cmbNivel.SelectedValue = myStruct.nivel.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.consolidado == "1" && this.chkConsolidado.Enabled) this.chkConsolidado.Checked = true;
                else this.chkConsolidado.Checked = false;

                this.txttitulo.Text = myStruct.titulo.Trim();

                if (myStruct.cuentaEditada == "1") this.chkCuentaEditada.Checked = true;
                else this.chkCuentaEditada.Checked = false;

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
                StructGLL01_MCIBASUMSA myStruct;

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

                myStruct.periodoDesde = this.txtMaskDesdePeriodos.Value.ToString().PadRight(5, ' ');
                myStruct.periodoHasta = this.txtMaskHastaPeriodos.Value.ToString().PadRight(5, ' ');

                if (this.chkMonedaLocal.Checked) myStruct.monedaLocal = "1";
                else myStruct.monedaLocal = "0";

                if (this.chkSubtotales.Checked) myStruct.subtotalesJerarq = "1";
                else myStruct.subtotalesJerarq = "0";

                //Niveles
                myStruct.nivel = this.cmbNivel.SelectedValue.ToString();

                if (this.chkConsolidado.Checked) myStruct.consolidado = "1";
                else myStruct.consolidado = "0";

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

                result = myStruct.compania + myStruct.grupo + myStruct.plan + myStruct.periodoDesde + myStruct.periodoHasta + myStruct.monedaLocal;
                result += myStruct.subtotalesJerarq + myStruct.nivel + myStruct.consolidado + myStruct.cuentaEditada + myStruct.titulo + myStruct.cuentasMayor;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIBASUMSA));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (result.Length < 421) result.PadRight(421, ' ');

            return (result);
        }
        #endregion
    }

    public class DatosCuenta
    {
        private string cuenta;
        private string cuentaEdit;
        private string tipo;  //T -> titulo     D -> detalle
        private string cuentaNombre;
        private int nivel;
        private decimal saldoInicial;
        private decimal saldoDebe;
        private decimal saldoHaber;

        public string Cuenta
        {
            get
            {
                return (this.cuenta);
            }
            set
            {
                this.cuenta = value;
            }
        }

        public string CuentaEdit
        {
            get
            {
                return (this.cuentaEdit);
            }
            set
            {
                this.cuentaEdit = value;
            }
        }

        public string Tipo
        {
            get
            {
                return (this.tipo);
            }
            set
            {
                this.tipo = value;
            }
        }

        public string CuentaNombre
        {
            get
            {
                return (this.cuentaNombre);
            }
            set
            {
                this.cuentaNombre = value;
            }
        }

        public int Nivel
        {
            get
            {
                return (this.nivel);
            }
            set
            {
                this.nivel = value;
            }
        }

        public decimal SaldoInicial
        {
            get
            {
                return (this.saldoInicial);
            }
            set
            {
                this.saldoInicial = value;
            }
        }

        public decimal SaldoDebe
        {
            get
            {
                return (this.saldoDebe);
            }
            set
            {
                this.saldoDebe = value;
            }
        }

        public decimal SaldoHaber
        {
            get
            {
                return (this.saldoHaber);
            }
            set
            {
                this.saldoHaber = value;
            }
        }

        public DatosCuenta()
        {
        }

        public DatosCuenta(string cuentaV, string cuentaEditV, string tipoV, string cuentaNombreV, int nivelV, 
                            decimal saldoInicialV, decimal saldoDebeV, decimal saldoHaberV)
        {
            this.cuenta = cuentaV;
            this.cuentaEdit = cuentaEditV;
            this.tipo = tipoV;
            this.cuentaNombre = cuentaNombreV;
            this.nivel = nivelV;
            this.saldoInicial = saldoInicialV;
            this.saldoDebe = saldoDebeV;
            this.saldoHaber = saldoHaberV;
        }
    }
}