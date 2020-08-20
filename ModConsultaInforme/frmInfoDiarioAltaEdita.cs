using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ObjectModel;
using System.Diagnostics;
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmInfoDiarioAltaEdita : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCIDIARIO";
        public string formCodeNombFichero = "DIARIO";
        public string ficheroExtension = "ddt";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCIDIARIO
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
            public string tipoInforme;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string cuentaEditada;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string importarTotales;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string noPrimerComp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string titulo;
        }

        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum DiarioDetalladoTipo
        {
            Detallado,
            ResumidoFecha,
            ResumidoPeriodo
        }

        private string tipoFichero = "EXCEL";

        private DiarioDetalladoTipo informeDiaroDetalladoTipo;

        //private Microsoft.Office.Interop.Excel.Application excelApp;
        //private Microsoft.Office.Interop.Excel.Workbook objLibroExcel;
        //private Microsoft.Office.Interop.Excel.Worksheet objHojaExcel;

        //private int filaExcel;
        string[] cabeceraComp = new string[8];

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

        FormularioValoresCampos valoresFormulario;

        private ArrayList avisosAutorizaciones;

        private bool cargarPlanes = false;

        private System.Data.DataTable dtTipoInforme;

        public string InformeDiarioDetalladoTipoStr
        {
            set
            {
                switch (value)
                {
                    case "DE":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.Detallado;
                        break;
                    case "RF":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.ResumidoFecha;
                        break;
                    case "RP":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.ResumidoPeriodo;
                        break;
                }
            }
        }

        int auxIncremento = 0;

        private string nombreFicheroAGenerar = "";

        private string mensajeProceso = "";

        public frmInfoDiarioAltaEdita()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoDiarioAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Diario");

            //Obtener el tipo de fichero
            string tipoFicherosInformes = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosInformes"];
            if (tipoFicherosInformes != null && tipoFicherosInformes != "") if (tipoFicherosInformes == "HTML") tipoFichero = tipoFicherosInformes;

            //Traducir los literales
            this.TraducirLiterales();

            //Cargar compañías
            this.FillCompanias();

            //Cargar grupo de compañías
            this.FillGruposCompanias();

            //Cargar planes
            this.FillPlanes("");

            //Crear Tabla Tipo de informe
            this.dtTipoInforme = new System.Data.DataTable();
            this.dtTipoInforme.Columns.Add("valor", typeof(string));
            this.dtTipoInforme.Columns.Add("desc", typeof(string));

            //Crear el desplegable de Tipo de informe
            this.CrearComboTipoInforme();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            switch (this.informeDiaroDetalladoTipo)
            {
                case DiarioDetalladoTipo.Detallado:
                    formCode += "D";
                    this.ficheroExtension = "ddt";
                    this.formCodeNombFichero = "DIARIOD";
                    break;
                case DiarioDetalladoTipo.ResumidoFecha:
                    formCode += "F";
                    this.ficheroExtension = "drf";
                    this.formCodeNombFichero = "DIARIOF";
                    break;
                case DiarioDetalladoTipo.ResumidoPeriodo:
                    formCode += "P";
                    this.ficheroExtension = "drp";
                    this.formCodeNombFichero = "DIARIOP";
                    break;
            }

            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    //Iniciar el Campo titulo con el nombre del formulario
                    this.txttitulo.Text = this.Text;

                    //FALTA !!! doblar las comillas simples '

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
        
        private void FrmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion
            {
                FormCode = this.formCode,
                Formulario = this
            };
            _ = frmPeticion.CargarPeticionDataTable(((System.Data.DataTable)e.Valor));

            if (this.cmbCompania.SelectedValue != null && this.cmbCompania.SelectedValue.ToString().Trim() != "") this.radDropDownListPlan.Enabled = false;
        }

        private void TxtNoPrimerComp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                if (e.KeyChar != 8)
                    e.Handled = true;
            }
        }

        private void RadButtonEjecutar_Click(object sender, EventArgs e)
        {
            this.Ejecutar();
        }

        private void RadButtonCargarPeticion_Click(object sender, EventArgs e)
        {
            this.CargarPeticiones();
        }

        private void RadButtonGrabarPeticion_Click(object sender, EventArgs e)
        {
            this.GrabarPeticion();
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

        private void RbPeriodos_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void RbFecha_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
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
                string tituloDialogo = "Diario ";
                switch(this.informeDiaroDetalladoTipo)
                {
                    case DiarioDetalladoTipo.Detallado:
                        tituloDialogo += "Detallado";
                        break;
                    case DiarioDetalladoTipo.ResumidoFecha:
                        tituloDialogo += "Resumido por Fecha";
                        break;
                    case DiarioDetalladoTipo.ResumidoPeriodo:
                        tituloDialogo += "Resumido por Periodo";
                        break;
                }
                    
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", tituloDialogo, MessageBoxButtons.YesNo, RadMessageIcon.Question);

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
                    if (this.pBarProcesandoInfo.Value + this.auxIncremento > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Maximum;
                    else this.pBarProcesandoInfo.Value += this.auxIncremento;
                    break;
            }
        }

        private void FrmInfoDiarioAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Diario Detallado");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            switch (this.informeDiaroDetalladoTipo)
            {
                case DiarioDetalladoTipo.Detallado:
                    this.Text = this.LP.GetText("subMenuItemDiarioDetallado", "Diario Detallado");
                    break;
                case DiarioDetalladoTipo.ResumidoFecha:
                    this.Text = this.LP.GetText("subMenuItemDiarioResFecha", "Diario Resumido Fecha");
                    break;
                case DiarioDetalladoTipo.ResumidoPeriodo:
                    this.Text = this.LP.GetText("subMenuItemDiarioResPeriodo", "Diario Resumido Periodo");
                    break;
            }
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
            this.chkImportarTotales.Text = this.LP.GetText("lblImportarTotales", "Importar Totales");
            //this.lblNoPrimerComp.Text = this.LP.GetText("lblNoPrimerComp", "No. Primer Comp.");
            this.lblProcesandoInfo.Text = this.LP.GetText("lblProcesandoInfo", "Procesando informe");

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Guardar Petición");   //Falta traducir
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
        /// Crea el desplegable de Tipo de informe
        /// </summary>
        private void CrearComboTipoInforme()
        {
            DataRow row;
            
            try
            {
                if (this.dtTipoInforme.Rows.Count > 0) this.dtTipoInforme.Rows.Clear();
                
                row = this.dtTipoInforme.NewRow();
                row["valor"] = "DE";
                row["desc"] = "Detallado";
                this.dtTipoInforme.Rows.Add(row);
                
                row = this.dtTipoInforme.NewRow();
                row["valor"] = "RF";
                row["desc"] = "Resumido por fecha";
                this.dtTipoInforme.Rows.Add(row);

                row = this.dtTipoInforme.NewRow();
                row["valor"] = "RP";
                row["desc"] = "Resumido por periodo";
                this.dtTipoInforme.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListTipoInforme.DataSource = this.dtTipoInforme;
            this.radDropDownListTipoInforme.ValueMember = "valor";
            this.radDropDownListTipoInforme.DisplayMember = "desc";
            this.radDropDownListTipoInforme.Refresh();
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
            else if (this.rbPeriodos.IsChecked)
            {
                //string resultMsg = this.ValidarPeriodo(ref this.txtMaskDesdePeriodos);
                //this.txtMaskDesdePeriodos.Text = this.txtMaskDesdePeriodos.Text.PadRight(5, '0');
                this.txtMaskDesdePeriodos.Text = this.txtMaskDesdePeriodos.Value.ToString().PadRight(5, '0');
                this.txtDesdePeriodos.Text  = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2);
                string resultMsg = this.ValidarPeriodo(ref this.txtDesdePeriodos);
                if (resultMsg != "")
                {
                    RadMessageBox.Show(resultMsg, error);
                    this.txtMaskDesdePeriodos.Focus();
                    return (false);
                }

                //resultMsg = this.ValidarPeriodo(ref this.txtMaskHastaPeriodos);
                this.txtMaskHastaPeriodos.Text = this.txtMaskHastaPeriodos.Value.ToString().PadRight(5, '0');
                this.txtHastaPeriodos.Text = this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                resultMsg = this.ValidarPeriodo(ref this.txtHastaPeriodos);
                if (resultMsg != "")
                {
                    RadMessageBox.Show(resultMsg, error);
                    this.txtMaskHastaPeriodos.Focus();
                    return (false);
                }
            }
                
            //Validar que el numero de comprobante sea numerico o vacio

            return (result);
        }

        /// <summary>
        /// Genera el Informe
        /// </summary>
        private void ProcesarInforme()
        {
            this.mensajeProceso = "";

            //Iniciar la barra de progreso
            this.backgroundWorker1.ReportProgress(0);

            //Cabecera de las columnas del informe
            string[] columnas = this.ObtenerCabecerasColumnasInforme();
            decimal registros = 0;
            totalFinalImporteDebe = 0;
            totalFinalImporteHaber = 0;    

            //Condicionar segun tipo de informe
            if (this.radDropDownListTipoInforme.SelectedValue != null)
            {
                switch (this.radDropDownListTipoInforme.SelectedValue.ToString())
                {
                    case "DE":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.Detallado;
                        formCode += "D";
                        this.ficheroExtension = "ddt";
                        this.formCodeNombFichero = "DIARIOD";
                        break;
                    case "RF":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.ResumidoFecha;
                        formCode += "F";
                        this.ficheroExtension = "drf";
                        this.formCodeNombFichero = "DIARIOF";
                        break;
                    case "RP":
                        this.informeDiaroDetalladoTipo = DiarioDetalladoTipo.ResumidoPeriodo;
                        formCode += "P";
                        this.ficheroExtension = "drp";
                        this.formCodeNombFichero = "DIARIOP";
                        break;
                }
            }

            //Eliminar los posibles permisos de no autorizado del usuario conectado
            GlobalVar.UsuarioEnv.ListaNoAutorizado.Clear();

            //Inicializar los avisos de autorizaciones
            this.avisosAutorizaciones = new ArrayList();

            string fila1;
            decimal incremento;
            if (codigoCompania != "")
            {
                //Informe Detallado Por Compania

                //Escribir 1ra fila y las Cabeceras de las columnas
                fila1 = this.InformeTitulo(this.descCompania);

                //Obtener el Informe Por Compañía
                switch (this.informeDiaroDetalladoTipo)
                {
                    case DiarioDetalladoTipo.Detallado:
                        //Obtener cantidad de registros de comprobantes que serán procesados
                        registros = this.InformeDetalladoCalcularRegistrosInitBarraProgreso(this.codigoCompania);

                        if (registros == 0)
                        {
                            //RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado")); 
                            //Finaliza la barra de progreso
                            this.backgroundWorker1.ReportProgress(1);
                            this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");
                            return;
                        }
                        incremento = Convert.ToDecimal(this.pBarProcesandoInfo.Maximum) / registros;
                        //Iniciar la barra de progreso
                        //this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Blocks);

                        //Crear el fichero en memoria (Excel o HTML)
                        /*SMR if (tipoFichero == "HTML")*/
                        this.InformeHTMLCrear(ref this.documento_HTML);
                        /*SMR else this.InformeExcelCrear(ref excelApp, ref objLibroExcel, ref objHojaExcel, "Informe");*/

                        /*SMR if (tipoFichero == "HTML")*/
                        this.InformeHTMLEscribirCabeceras(fila1, columnas);
                        /*SMR else this.InformeExcelEscribirCabeceras(fila1, columnas);*/

                        this.InformeDetalladoPorCompania(this.codigoCompania, incremento);
                        break;
                    case DiarioDetalladoTipo.ResumidoFecha:
                    case DiarioDetalladoTipo.ResumidoPeriodo:
                        //Obtener cantidad de registros de comprobantes que serán procesados
                        registros = this.InformeResumidoCalcularRegistrosInitBarraProgreso("'" + this.codigoCompania + "'");

                        if (registros == 0)
                        {
                            //Finaliza la barra de progreso
                            this.backgroundWorker1.ReportProgress(1);
                            //RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado"));
                            this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");
                            return;
                        }
                        incremento = Convert.ToDecimal(this.pBarProcesandoInfo.Maximum) / registros;
                        //Iniciar la barra de progreso
                        //this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Blocks);

                        //Crear el fichero en memoria (Excel o HTML)
                        /*SMR if (tipoFichero == "HTML")*/
                        this.InformeHTMLCrear(ref this.documento_HTML);
                        /*SMR else this.InformeExcelCrear(ref excelApp, ref objLibroExcel, ref objHojaExcel, "Informe");*/

                        /*SMR if (tipoFichero == "HTML")*/
                        this.InformeHTMLEscribirCabeceras(fila1, columnas);
                        /*SMR else this.InformeExcelEscribirCabeceras(fila1, columnas);*/

                        this.InformeResumidoPorCompania("'" + this.codigoCompania + "'", incremento);
                        break;
                }
            }
            else
            {
                //if (codigoGrupo != "" && codigoPlan != "")
                if (codigoGrupo != "")
                {
                    //Informe Detallado por Grupo de Compañías

                    //Buscar las empresas del grupo
                    ArrayList aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(codigoGrupo, codigoPlan);

                    string[] datosCompania;
                    //Obtener cantidad de registros de comprobantes que serán procesados para cada compañia
                    for (int i = 0; i < aEmpresas.Count; i++)
                    {
                        datosCompania = (string[])aEmpresas[i];
                        registros += this.InformeDetalladoCalcularRegistrosInitBarraProgreso(datosCompania[0].ToString());
                    }

                    if (registros == 0)
                    {
                        //Finaliza la barra de progreso
                        this.backgroundWorker1.ReportProgress(1);
                        //RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado"));
                        this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");
                        return;
                    }
                    incremento = Convert.ToDecimal(this.pBarProcesandoInfo.Maximum) / registros;
                    //Iniciar la barra de progreso
                    //this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Blocks);

                    //Crear el fichero en memoria (Excel o HTML)
                    /*SMR if (tipoFichero == "HTML")*/
                    this.InformeHTMLCrear(ref this.documento_HTML);
                    /*SMR else this.InformeExcelCrear(ref excelApp, ref objLibroExcel, ref objHojaExcel, "Informe");*/

                    //Escribir 1ra fila y las Cabeceras de las columnas
                    fila1 = this.InformeTitulo(descGrupo);
                    /*SMR if (tipoFichero == "HTML")*/
                    this.InformeHTMLEscribirCabeceras(fila1, columnas);
                    /*SMR else this.InformeExcelEscribirCabeceras(fila1, columnas);*/

                    //Informe Detallado Por Compania
                    if (this.informeDiaroDetalladoTipo == DiarioDetalladoTipo.Detallado)
                    {
                        for (int i = 0; i < aEmpresas.Count; i++)
                        {
                            datosCompania = (string[])aEmpresas[i];
                            //Informe Detallado Por Compania
                            this.InformeDetalladoPorCompania(datosCompania[0].ToString(), incremento);
                        }
                    }
                    else
                    {
                        string codigoEmpresas = "";
                        for (int i = 0; i < aEmpresas.Count; i++)
                        {
                            datosCompania = (string[])aEmpresas[i];
                            if (i != 0) codigoEmpresas += ",";
                            codigoEmpresas += "'" + datosCompania[0].ToString() + "'";
                        }

                        this.InformeResumidoPorCompania(codigoEmpresas, incremento);
                    }
                }
            }

            if (this.chkImportarTotales.Checked)
            {
                //Escribir linea de Total Final del informe el comprobante (si esta activa la opcion)
                cabeceraComp[0] = "";
                cabeceraComp[1] = "";
                cabeceraComp[2] = "";
                cabeceraComp[3] = "";
                cabeceraComp[4] = "";
                cabeceraComp[5] = this.LP.GetText("lblCabTotalFinal", "Total final");
                cabeceraComp[6] = totalFinalImporteDebe.ToString();
                cabeceraComp[7] = totalFinalImporteHaber.ToString();

                /*SMR if (tipoFichero == "HTML")
                {*/
                    this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);

                    this.InformeHTMLEscribirDetalleComprobante(cabeceraComp);

                    this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);
                /*SMR }
                else
                {
                    this.InformeExcelEscribirDetalleComprobante(cabeceraComp);
                }*/
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
                this.backgroundWorker1.ReportProgress(1);
                //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
                this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
            }
            else
            {
                /*SMR this.InformeExcelEscribirAjustesGlobales();*/
                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
                //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                //SMR levantar html con excel
                /*string ficheroHTML = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"];
                ficheroHTML = ficheroHTML + txttitulo.Text  + ".html";
                */

            /*
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
            */
        }

        /// <summary>
        /// Devuelve las cabeceras de las columnas del informe en el idioma correspondiente
        /// </summary>
        /// <returns></returns>
        private string[] ObtenerCabecerasColumnasInforme()
        {
            //Cabecera de las columnas del informe
            string[] columnas = new string[8];
            columnas[0] = this.LP.GetText("lblCabNumero", "Número");
            columnas[1] = this.LP.GetText("lblCabFecha", "Fecha");
            columnas[2] = this.LP.GetText("lblCabIdInterno", "Id. interno");
            columnas[3] = this.LP.GetText("lblCabDesc", "Concepto");
            columnas[4] = this.LP.GetText("lblCabCuenta", "Cuenta");
            columnas[5] = this.LP.GetText("lblCabNombreCuenta", "Nombre de la cuenta");
            columnas[6] = this.LP.GetText("lblCabDebe", "DEBE");
            columnas[7] = this.LP.GetText("lblCabHaber", "HABER");
            return (columnas);
        }


        /// <summary>
        /// Calcula los registros de comprobantes que se procesarán para la compañía solicitada
        /// </summary>
        /// <param name="codigo">Código de la compañía</param>
        /// <returns></returns>
        private decimal InformeDetalladoCalcularRegistrosInitBarraProgreso(string codigo)
        {
            string queryCount = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLI03 ";

            string query = this.InformeDetalladoObtenerWhereQueryComprobantes(codigo);

            queryCount += query;

            decimal registros = 0;
            
            try
            {
                //Obtener cantidad de comprobantes para la compañía
                object totalregistros = GlobalVar.ConexionCG.ExecuteScalar(queryCount, GlobalVar.ConexionCG.GetConnectionValue);
                registros = Convert.ToInt32(totalregistros);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (registros);
        }

        /// <summary>
        /// Calcula los registros que se procesarán para el informe resumido por fecha y por periodo para la compañía o grupo de compañías solicitada
        /// </summary>
        /// <param name="codigo">Código de la compañía o grupo de compañías</param>
        /// <returns></returns>
        private decimal InformeResumidoCalcularRegistrosInitBarraProgreso(string codigo)
        {
            string queryCount = " select count(*) from( ";
            string query;
            if (this.informeDiaroDetalladoTipo == DiarioDetalladoTipo.ResumidoFecha)
            {
                //SMR - join GLM03
                queryCount += "select FECODT, CCIADT, CUENDT, TMOVDT, SUM(MONTDT) IMPORTE, NOLAAD, CEDTMC FROM " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                queryCount += "LEFT JOIN " + GlobalVar.PrefijoTablaCG + "GLM03 ON CUENDT = CUENMC and TIPLDT = TIPLMC ";
                query = this.InformeResumidoObtenerWhereQueryComprobantes(codigo);
            }
            else
            {
                //SMR - join GLM03
                queryCount += "select SAPRDT, CCIADT, CUENDT, TMOVDT, SUM(MONTDT) IMPORTE, NOLAAD, CEDTMC FROM " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                queryCount += "LEFT JOIN " + GlobalVar.PrefijoTablaCG + "GLM03 ON CUENDT = CUENMC and TIPLDT = TIPLMC ";
                query = this.InformeResumidoObtenerWhereQueryComprobantes(codigo);
            }

            queryCount += query;
            queryCount += ") as total ";

            decimal registros = 0;

            IDataReader dr = null;
            try
            {
                //Obtener cantidad de comprobantes para la compañía
                dr = GlobalVar.ConexionCG.ExecuteReader(queryCount, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    registros = Convert.ToInt32(dr.GetValue(0));
                }
            }
            catch(Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (registros);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string InformeDetalladoObtenerWhereQueryComprobantes(string codigo)
        {
            string query = "where STATIC = 'E' and CCIAIC ='" + codigo + "'";

            if (this.rbPeriodos.IsChecked)
            {
                string aa = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2);
                query += " and SAPRIC >=" + utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this.txtMaskDesdePeriodos.Value.ToString().Substring(0,2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2);
                aa = this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2);
                query += " and SAPRIC <=" + utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this.txtMaskHastaPeriodos.Value.ToString().Substring(0,2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
            }
            else
            {
                int fechaDesdeFormatoCG = utiles.FechaToFormatoCG(this.dateTimeDesdeFecha.Value, true);
                int fechaHastaFormatoCG = utiles.FechaToFormatoCG(this.dateTimeHastaFecha.Value, true);
                query += " and FECOIC >=" + fechaDesdeFormatoCG.ToString() + " and FECOIC <=" + fechaHastaFormatoCG.ToString();
            }
            return (query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string InformeResumidoObtenerWhereQueryComprobantes(string codigo)
        {
            string query = "where STATDT = 'E' and CCIADT in (" + codigo + ")";

            if (this.rbPeriodos.IsChecked)
            {
                string aa = this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2);
                query += " and SAPRDT >=" + utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + this.txtMaskDesdePeriodos.Value.ToString().Substring(3, 2); ;
                aa = this.txtMaskHastaPeriodos.Text.Substring(0, 2);
                query += " and SAPRDT <=" + utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2); ;
            }
            else
            {
                int fechaDesdeFormatoCG = utiles.FechaToFormatoCG(this.dateTimeDesdeFecha.Value, true);
                int fechaHastaFormatoCG = utiles.FechaToFormatoCG(this.dateTimeHastaFecha.Value, true);
                query += " and FECODT >=" + fechaDesdeFormatoCG.ToString() + " and FECODT <=" + fechaHastaFormatoCG.ToString();
            }

            if (this.informeDiaroDetalladoTipo == DiarioDetalladoTipo.ResumidoFecha)
            {
                query += " GROUP BY FECODT, CCIADT, CUENDT, TMOVDT, SAPRDT, NOLAAD, CEDTMC "; //SMR having + order by
                query += "HAVING SUM(MONTDT) <> 0 ORDER BY FECODT, CCIADT, CUENDT, TMOVDT, SAPRDT, NOLAAD, CEDTMC"; 
            }
            else
            {
                query += " GROUP BY SAPRDT, CCIADT, CUENDT, TMOVDT, NOLAAD, CEDTMC "; //SMR having + order by
                query += "HAVING SUM(MONTDT) <> 0 ORDER BY SAPRDT, CCIADT, CUENDT, TMOVDT, NOLAAD, CEDTMC";
            }
            return (query);
        }


        /// <summary>
        /// Genera el Informe Detallado por la compañia seleccionada
        /// </summary>
        /// <param name="codigo">código de la compañia seleccionada</param>
        /// <param name="incremento">incremento para avanzar la barra de progreso</param>
        private void InformeDetalladoPorCompania(string codigo, decimal incremento)
        {
            string queryTabla = "select * from " + GlobalVar.PrefijoTablaCG + "GLI03 ";

            string query = this.InformeDetalladoObtenerWhereQueryComprobantes(codigo);

            query += " order by FECOIC, CCIAIC, TICOIC, NUCOIC";

            query = queryTabla + query;

            IDataReader dr = null;
            try
            {
                int noComp;
                if (this.txtNoPrimerComp.Text.Trim() == "") noComp = 1;
                else noComp = Convert.ToInt32(this.txtNoPrimerComp.Text);

                string idInterno = "";
                string cciaic = "";
                string sapric = "";
                string anoSAPRIC = "";
                string perSAPRIC = "";
                string ticoic = "";
                string ticoic2Pos = "";
                string nucoic = "";

                bool autTipoComp = false;
                string autClaseElemento = "004";
                string autGrupo = "03";
                string autOperConsulta = "10";

                string aviso = "";

                bool compConMov = false;

                //Obtener las cabeceras de los comprobantes
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    ticoic = dr["TICOIC"].ToString();
                    ticoic2Pos = ticoic.PadLeft(2, '0');

                    //Chequear autorización a los movimientos del tipo de comprobante
                    autTipoComp = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, autClaseElemento, autGrupo, autOperConsulta, ticoic2Pos);

                    if (autTipoComp)
                    {
                        //Cabecera
                        cciaic = dr["CCIAIC"].ToString();
                        sapric = dr["SAPRIC"].ToString();
                        if (sapric.Length == 5) //saapp
                        {
                            anoSAPRIC = sapric.Substring(1, 2);
                            perSAPRIC = sapric.Substring(3, 2);
                        }
                        else  //aapp
                        {
                            anoSAPRIC = sapric.Substring(0, 2);
                            perSAPRIC = sapric.Substring(2, 2);
                        }

                        nucoic = dr["NUCOIC"].ToString();
                        nucoic = nucoic.PadLeft(5, '0');
                        idInterno = cciaic + " " + anoSAPRIC + " " + perSAPRIC + " " + ticoic2Pos + " " + nucoic;

                        cabeceraComp[0] = noComp.ToString();
                        cabeceraComp[1] = utiles.FormatoCGToFecha(dr["FECOIC"].ToString()).ToShortDateString();
                        cabeceraComp[2] = idInterno;
                        cabeceraComp[3] = utilesCG.ObtenerDescripcionComprobante(cciaic, sapric, ticoic, nucoic);   //GLAI3.Desc
                        cabeceraComp[4] = "";
                        cabeceraComp[5] = "";
                        cabeceraComp[6] = "";
                        cabeceraComp[7] = "";
                        /*
                        //Escribe la cabecera del comprobante en el fichero HTML / Excel
                        this.InformeHTMLEscribirCabeceraComprobante(cabeceraComp);

                        this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                        */

                        //Procesar el Detalle del comprobante
                        compConMov = false;
                        this.InformeDetalladoPorCompaniaDetallesComp(cciaic, sapric, ticoic, nucoic, ref compConMov);

                        noComp++;

                        if (this.chkImportarTotales.Checked && compConMov)
                        {
                            //Escribe el total del comprobante en el fichero Excel
                            cabeceraComp[0] = "";
                            cabeceraComp[1] = "";
                            cabeceraComp[2] = "";
                            cabeceraComp[3] = "";
                            cabeceraComp[4] = "";
                            cabeceraComp[5] = this.LP.GetText("lblCabTotal", "TOTAL");
                            cabeceraComp[6] = totalImporteDebe.ToString();
                            cabeceraComp[7] = totalImporteHaber.ToString();
                            this.EscribirDetalleComprobante(cabeceraComp);
                        }

                        //Mover la barra de progreso
                        this.auxIncremento = Convert.ToInt32(incremento);

                        this.backgroundWorker1.ReportProgress(2);

                        //if (this.pBarProcesandoInfo.Value + this.auxIncremento > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Maximum;
                        //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + this.auxIncremento;
                    }
                    else
                    {
                        aviso = "Usuario no autorizado para algunos tipos de comprobantes"; //Falta traducir
                        if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                    }
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(this.LP.GetText("errGetInforme", "Error obteniendo el informe") + " (" + ex.Message + ")", error);
            }
        }

        /// <summary>
        /// Genera el Informe Resumido por la compañia seleccionada
        /// </summary>
        /// <param name="codigo">código de la compañia seleccionada</param>
        /// <param name="incremento">incremento para avanzar la barra de progreso</param>
        private void InformeResumidoPorCompania(string codigo, decimal incremento)
        {
            string query = "";

            switch (this.informeDiaroDetalladoTipo)
            {
                case DiarioDetalladoTipo.ResumidoFecha:
                    query = "SELECT FECODT, CCIADT, CUENDT, TMOVDT, SUM(MONTDT) IMPORTE, NOLAAD, CEDTMC, SAPRDT FROM " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                    query += "LEFT JOIN " + GlobalVar.PrefijoTablaCG + "GLM03 ON CUENDT = CUENMC and TIPLDT = TIPLMC ";
                    break;
                case DiarioDetalladoTipo.ResumidoPeriodo:
                    query = "SELECT SAPRDT, CCIADT, CUENDT, TMOVDT, SUM(MONTDT) IMPORTE, NOLAAD, CEDTMC FROM " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                    query += "LEFT JOIN " + GlobalVar.PrefijoTablaCG + "GLM03 ON CUENDT = CUENMC and TIPLDT = TIPLMC ";
                    break;
            }

            query += this.InformeResumidoObtenerWhereQueryComprobantes(codigo);

            IDataReader dr = null;
            try
            {
                int noComp;
                if (this.txtNoPrimerComp.Text.Trim() == "") noComp = 1;
                else noComp = Convert.ToInt32(this.txtNoPrimerComp.Text);

                string[] datosCompania = new string[2];

                string cciadt = "";
                string cciadtAux = "";
                string cedtmc = "";
                string cuendt = "";
                string nolaad = "";
                string descad = "";
                string tmovdt = "";
                decimal importeDebe = 0;
                decimal importeHaber = 0;
                decimal totalPeriodoImporteDebe = 0;
                decimal totalPeriodoImporteHaber = 0;
                totalImporteDebe = 0;
                totalImporteHaber = 0;
                totalFinalImporteDebe = 0;
                totalFinalImporteHaber = 0;

                string[] detalleComp = new string[8];
                //ArrayList cuentaDatos;
                string fechaAux = "";
                string fecha = "";
                string saprdt = "";
                string saprdtAux = "";
                string anoSAPRDT = "";
                string perSAPRDT = "";
                string idInterno = "";
                string[] fechasCalendario;
                bool hayReg = false;

                bool importe0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    hayReg = true;
                    importe0 = false;

                    cciadt = dr["CCIADT"].ToString();
                    if (cciadt != cciadtAux)
                    {
                        datosCompania = utilesCG.ObtenerTipoCalendarioCompania(cciadt);
                        cciadtAux = cciadt;
                    }

                    saprdt = dr["SAPRDT"].ToString();

                    switch (this.informeDiaroDetalladoTipo)
                    {
                        case DiarioDetalladoTipo.ResumidoFecha:
                            fecha = dr["FECODT"].ToString();
                            if (fecha != fechaAux)
                            {
                                if (this.chkImportarTotales.Checked)
                                {
                                    //Escribir totales del dia
                                    if (fechaAux != "")
                                    {
                                        //Escribe el total del comprobante en el fichero Excel
                                        cabeceraComp[0] = "";
                                        cabeceraComp[1] = "";
                                        cabeceraComp[2] = "";
                                        cabeceraComp[3] = "";
                                        cabeceraComp[4] = "";
                                        cabeceraComp[5] = this.LP.GetText("lblCabTotalDia", "Total dia");
                                        cabeceraComp[6] = totalImporteDebe.ToString();
                                        cabeceraComp[7] = totalImporteHaber.ToString();
                                        this.EscribirDetalleComprobante(cabeceraComp);

                                        totalImporteDebe = 0;
                                        totalImporteHaber = 0;
                                        if (saprdt != saprdtAux)
                                        {
                                            //Escribir totales del periodo
                                            cabeceraComp[0] = "";
                                            cabeceraComp[1] = "";
                                            cabeceraComp[2] = "";
                                            cabeceraComp[3] = "";
                                            cabeceraComp[4] = "";
                                            cabeceraComp[5] = this.LP.GetText("lblCabTotalPeriodo", "Total periodo");
                                            cabeceraComp[6] = totalPeriodoImporteDebe.ToString();
                                            cabeceraComp[7] = totalPeriodoImporteHaber.ToString();

                                            /*SMR if (this.tipoFichero == "HTML")*/ this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);

                                            this.EscribirDetalleComprobante(cabeceraComp);

                                            totalPeriodoImporteDebe = 0;
                                            totalPeriodoImporteHaber = 0;
                                        }
                                    }
                                }

                                fechaAux = fecha;
                                saprdtAux = saprdt;

                                //Escribir cabecera de fecha
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

                                idInterno = cciadt + " " + anoSAPRDT + " " + perSAPRDT;

                                cabeceraComp[0] = noComp.ToString();
                                cabeceraComp[1] = utiles.FormatoCGToFecha(dr["FECODT"].ToString()).ToShortDateString();
                                cabeceraComp[2] = idInterno;
                                cabeceraComp[3] = this.LP.GetText("lblResumenMovDia", "Resumen movimientos del día");
                                cabeceraComp[4] = "";
                                cabeceraComp[5] = "";
                                cabeceraComp[6] = "";
                                cabeceraComp[7] = "";
                                //Escribe la cabecera del comprobante en el fichero HTML / Excel
                                /*SMR if (tipoFichero == "HTML")*/ this.InformeHTMLEscribirCabeceraComprobante(cabeceraComp);
                                /*SMR else this.InformeExcelEscribirCabeceraComprobante(cabeceraComp);*/

                                /*SMR if (this.tipoFichero == "HTML")*/ this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                                noComp++;
                            }
                            break;
                        case DiarioDetalladoTipo.ResumidoPeriodo:
                            if (saprdt != saprdtAux)
                            {
                                if (this.chkImportarTotales.Checked)
                                {
                                    //Escribir totales del dia
                                    if (saprdtAux != "")
                                    {
                                        //Escribe el total del comprobante en el fichero Excel
                                        cabeceraComp[0] = "";
                                        cabeceraComp[1] = "";
                                        cabeceraComp[2] = "";
                                        cabeceraComp[3] = "";
                                        cabeceraComp[4] = "";
                                        cabeceraComp[5] = this.LP.GetText("lblCabTotalPeriodo", "Total periodo");
                                        cabeceraComp[6] = totalImporteDebe.ToString();
                                        cabeceraComp[7] = totalImporteHaber.ToString();
                                        this.EscribirDetalleComprobante(cabeceraComp);

                                        totalImporteDebe = 0;
                                        totalImporteHaber = 0;
                                    }
                                }

                                saprdtAux = saprdt;

                                //Escribir cabecera de fecha
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

                                idInterno = cciadt + " " + anoSAPRDT + " " + perSAPRDT;

                                fechasCalendario = utilesCG.ObtenerFechasCalendarioDadoSAPR(datosCompania[0], saprdt);


                                cabeceraComp[0] = noComp.ToString();
                                cabeceraComp[1] = utiles.FormatoCGToFecha(fechasCalendario[1]).ToShortDateString();
                                cabeceraComp[2] = idInterno;
                                cabeceraComp[3] = this.LP.GetText("lblResumenMovPeriodo", "Resumen movimientos del periodo");
                                cabeceraComp[4] = "";
                                cabeceraComp[5] = "";
                                cabeceraComp[6] = "";
                                cabeceraComp[7] = "";
                                //Escribe la cabecera del comprobante en el fichero HTML / Excel
                                /*SMR if (tipoFichero == "HTML")*/ this.InformeHTMLEscribirCabeceraComprobante(cabeceraComp);
                                /*SMR else this.InformeExcelEscribirCabeceraComprobante(cabeceraComp);*/

                                /*SMR if (this.tipoFichero == "HTML")*/ this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                                noComp++;
                            }
                            break;
                    }

                    //cuentaDatos = utilesCG.ObtenerDatosCuenta(dr["CUENDT"].ToString(), datosCompania[1]); //SMR - obtener cedtmc y nolaad de join
                    //cedtmc = cuentaDatos[1].ToString();
                    ////noabmc = dr["NOABMC"].ToString();
                    //nolaad = cuentaDatos[0].ToString();
                    cedtmc = dr["CEDTMC"].ToString();
                    cuendt = dr["CUENDT"].ToString();
                    nolaad = dr["NOLAAD"].ToString();

                    switch (this.informeDiaroDetalladoTipo)
                    {
                        case DiarioDetalladoTipo.ResumidoFecha:
                            descad = this.LP.GetText("lblResumenMovDia", "Resumen movimientos del día");
                            break;
                        case DiarioDetalladoTipo.ResumidoPeriodo:
                            descad = this.LP.GetText("lblResumenMovPeriodo", "Resumen movimientos del periodo");
                            break;
                    }
                    
                    tmovdt = dr["TMOVDT"].ToString();

                    detalleComp[0] = "";
                    detalleComp[1] = "";
                    detalleComp[2] = "";
                    detalleComp[3] = descad;
                    if (chkCuentaEditada.Checked == true) detalleComp[4] = cedtmc;
                    else detalleComp[4] = cuendt;
                    detalleComp[5] = nolaad;

                    switch (tmovdt)
                    {
                        case "D":
                            try
                            {
                                importeDebe = Convert.ToDecimal(dr["IMPORTE"].ToString());
                                if (importeDebe == 0) importe0 = true;
                                else
                                {
                                    totalImporteDebe += importeDebe;
                                    totalFinalImporteDebe += importeDebe;

                                    totalPeriodoImporteDebe += importeDebe;

                                    detalleComp[6] = importeDebe.ToString();
                                    detalleComp[7] = "";
                                }
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            break;
                        case "H":
                            try
                            {
                                importeHaber = Convert.ToDecimal(dr["IMPORTE"].ToString());
                                if (importeHaber == 0) importe0 = true;
                                {
                                    totalImporteHaber += importeHaber;
                                    totalFinalImporteHaber += importeHaber;

                                    totalPeriodoImporteHaber += importeHaber;

                                    detalleComp[6] = "";
                                    detalleComp[7] = importeHaber.ToString();
                                }
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                            break;
                    }

                    //Escribe la línea del detalle del comprobante en el fichero HTML / Excel
                    if (!importe0) this.InformeHTMLEscribirDetalleComprobante(detalleComp);

                    //Mover la barra de progreso
                    this.auxIncremento = Convert.ToInt32(incremento);
    
                    this.backgroundWorker1.ReportProgress(2);
                    //if (this.pBarProcesandoInfo.Value + this.auxIncremento > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Maximum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + this.auxIncremento;
                }
                dr.Close();

                if (this.chkImportarTotales.Checked && hayReg)
                {
                        //Escribe el total del dia
                        cabeceraComp[0] = "";
                        cabeceraComp[1] = "";
                        cabeceraComp[2] = "";
                        cabeceraComp[3] = "";
                        cabeceraComp[4] = "";
                        if (this.informeDiaroDetalladoTipo == DiarioDetalladoTipo.ResumidoFecha)
                            cabeceraComp[5] = this.LP.GetText("lblCabTotalDia", "Total día");
                        else cabeceraComp[5] = this.LP.GetText("lblCabTotalPeriodo", "Total periodo");
                        cabeceraComp[6] = totalImporteDebe.ToString();
                        cabeceraComp[7] = totalImporteHaber.ToString();
                        this.EscribirDetalleComprobante(cabeceraComp);

                    if (this.informeDiaroDetalladoTipo == DiarioDetalladoTipo.ResumidoFecha)
                    {
                        //Escribir el totales del periodo
                        cabeceraComp[0] = "";
                        cabeceraComp[1] = "";
                        cabeceraComp[2] = "";
                        cabeceraComp[3] = "";
                        cabeceraComp[4] = "";
                        cabeceraComp[5] = this.LP.GetText("lblCabTotalPeriodo", "Total periodo");
                        cabeceraComp[6] = totalPeriodoImporteDebe.ToString();
                        cabeceraComp[7] = totalPeriodoImporteHaber.ToString();
                        /*SMR if (this.tipoFichero == "HTML")*/ this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                        this.EscribirDetalleComprobante(cabeceraComp);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comprobante"></param>
        private void EscribirDetalleComprobante(string[] comprobante)
        {
            try
            {
                /*SMR if (tipoFichero == "HTML")
                {*/
                    this.InformeHTMLEscribirDetalleComprobante(comprobante);

                    this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);
                /*SMR }
                else
                {
                    this.InformeExcelEscribirDetalleComprobante(comprobante);
                }*/
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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
                result += comp_grupo + " " + this.LP.GetText("lblCabTituloInfo", "Diario de comprobantes") + " ";

                switch (this.informeDiaroDetalladoTipo)
                {
                    case DiarioDetalladoTipo.ResumidoFecha:
                        result += this.LP.GetText("lblCabTituloInfoPorFecha", "por fecha") + " ";
                        break;
                    case DiarioDetalladoTipo.ResumidoPeriodo:
                        result += this.LP.GetText("lblCabTituloInfoPorPer", "por periodo") + " ";
                        break;
                }

                if (this.rbPeriodos.IsChecked)
                {
                    result += this.LP.GetText("lblCabTituloInfoDePer", "de periodo") + " ";
                    if (this.txtMaskDesdePeriodos.Value.ToString().Length == 5)
                    {
                        result += this.txtMaskDesdePeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskDesdePeriodos.Text.Substring(3, 2) + " -  ";
                    }
                    if (this.txtMaskHastaPeriodos.Text.Length == 5)
                    {
                        result += this.txtMaskHastaPeriodos.Value.ToString().Substring(0, 2) + " " + this.txtMaskHastaPeriodos.Value.ToString().Substring(3, 2);
                    }
                }
                else
                    if (this.rbFecha.IsChecked)
                    {
                        result += this.LP.GetText("lblCabTituloInfoDeFecha", "de fecha") + " ";
                        result += this.dateTimeDesdeFecha.Value.ToShortDateString() + " -  ";
                        result += this.dateTimeHastaFecha.Value.ToShortDateString();
                    }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Busca los detalles de los comprobantes para el Informe Detallado
        /// </summary>
        /// <param name="cciaic">código de la compañía</param>
        /// <param name="sapric">siglo año periodo</param>
        /// <param name="ticoic">tipo de comprobante</param>
        /// <param name="nucoic">numero de comprobante</param>
        /// <param name="compConMov">true -> comprobante con movimientos  false -> commprobante sin movimientos</param>
        private void InformeDetalladoPorCompaniaDetallesComp(string cciaic, string sapric, string ticoic, string nucoic, ref bool compConMov)
        {
            string query = "select GLM03.CUENMC, GLM03.CEDTMC, GLM03.NOLAAD, GLB01.DESCAD, GLB01.TMOVDT, GLB01.MONTDT, GLM03.SASIMC, GLM03.TIPLMC, ";
            query += "GLM03.RNITMC, GLM03.SCONMC, GLB01.TAUXDT, GLM03.TAU1MC, GLM03.TAU2MC, GLM03.TAU3MC ";
            query += "from " + GlobalVar.PrefijoTablaCG + "GLB01 as GLB01, " + GlobalVar.PrefijoTablaCG + "GLM03 as GLM03 ";
            query += "where GLB01.CCIADT ='" + cciaic + "' and ";
            query += "GLB01.SAPRDT = " + sapric + " and ";
            query += "GLB01.TICODT = " + ticoic + " and ";
            query += "GLB01.NUCODT = '" + nucoic + "' and ";
            query += "GLM03.CUENMC=GLB01.CUENDT and ";
            query += "GLM03.TIPLMC=GLB01.TIPLDT ";
            query += "order by GLB01.SIMIDT";

            IDataReader dr = null;
            
            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string cedtmc = "";
                string cuenmc = "";
                string nolaad = "";
                string descad = "";
                string tmovdt = "";
                decimal importeDebe = 0;
                decimal importeHaber = 0;
                totalImporteDebe = 0;
                totalImporteHaber = 0;

                string[] detalleComp = new string[8];

                string sconmc = "";
                string tiplmc = "";
                bool autCuenta = false;
                int ultNivelConsultaCtaAut = -1;
                int nivelConsultaCtaActual = -1;

                string taux1 = "";
                string taux2 = "";
                string taux3 = "";
                bool autTaux1 = false;
                bool autTaux2 = false;
                bool autTaux3 = false;

                string aviso = "";

                int cont = 0;

                while (dr.Read())
                {
                    tiplmc = dr["TIPLMC"].ToString();
                    sconmc = dr["SCONMC"].ToString();

                    //Chequear autorización a la cuenta de mayor
                    try 
                    {
                        nivelConsultaCtaActual = Convert.ToInt16(sconmc);

                        if (ultNivelConsultaCtaAut == -1 || nivelConsultaCtaActual > ultNivelConsultaCtaAut)
                        {
                            //Verificar autorizaciones
                            autCuenta = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "003", "02", "0" + sconmc, tiplmc);
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
                        //Chequear autorización sobre el tipo de auxiliar
                        taux1 = dr["TAU1MC"].ToString().Trim();
                        taux2 = dr["TAU2MC"].ToString();
                        taux3 = dr["TAU3MC"].ToString();
                        
                        if (taux1 != "") autTaux1 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux1);
                        else autTaux1 = true;
                        if (taux2 != "") autTaux2 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux2);
                        else autTaux2 = true;
                        if (taux3 != "") autTaux3 = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "006", "02", "10", taux3);
                        else autTaux3 = true;

                        if (autTaux1 && autTaux2 && autTaux3)
                        {
                            cedtmc = dr["CEDTMC"].ToString();
                            cuenmc = dr["CUENMC"].ToString();
                            //noabmc = dr["NOABMC"].ToString();
                            nolaad = dr["NOLAAD"].ToString();
                            descad = dr["DESCAD"].ToString();
                            tmovdt = dr["TMOVDT"].ToString();

                            detalleComp[0] = "";
                            detalleComp[1] = "";
                            detalleComp[2] = "";
                            detalleComp[3] = descad;
                            if (chkCuentaEditada.Checked == true) detalleComp[4] = cedtmc;
                            else detalleComp[4] = cuenmc;
                            detalleComp[5] = nolaad;

                            switch (tmovdt)
                            {
                                case "D":
                                    try
                                    {
                                        importeDebe = Convert.ToDecimal(dr["MONTDT"].ToString());
                                        totalImporteDebe += importeDebe;
                                        totalFinalImporteDebe += importeDebe;

                                        detalleComp[6] = importeDebe.ToString();
                                        detalleComp[7] = "";
                                    }
                                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                    break;
                                case "H":
                                    try
                                    {
                                        importeHaber = Convert.ToDecimal(dr["MONTDT"].ToString());
                                        totalImporteHaber += importeHaber;
                                        totalFinalImporteHaber += importeHaber;

                                        detalleComp[6] = "";
                                        detalleComp[7] = importeHaber.ToString();
                                    }
                                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                                    break;
                            }

                            if (cont == 0)
                            {
                                //Escribe la cabecera del comprobante en el fichero HTML / Excel
                                /*SMR if (tipoFichero == "HTML")*/
                                compConMov = true;
                                this.InformeHTMLEscribirCabeceraComprobante(cabeceraComp);
                                /*SMR else this.InformeExcelEscribirCabeceraComprobante(cabeceraComp);*/

                                /*SMR if (this.tipoFichero == "HTML")*/
                                this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                            }

                            //Escribe la línea del detalle del comprobante en el fichero HTML / Excel
                            /*SMR if (tipoFichero == "HTML")*/
                            this.InformeHTMLEscribirDetalleComprobante(detalleComp);
                            /*SMR else this.InformeExcelEscribirDetalleComprobante(detalleComp);*/

                            cont++;
                        }
                        else
                        {
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
                dr.Close();

            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        #region InformeDetalladoPorCompaniaExcel    EXCEL
        /*
        private void InformeExcelEscribirCabeceras(string fila1, string[] cabecerasColumnas)
        {
            filaExcel++;
            ColorConverter cc = new ColorConverter();
            //Crear el encabezado del informe
            string rango = "A" + filaExcel + ":" + "H" + filaExcel;
            objHojaExcel.Range[rango].Merge();
            objHojaExcel.Range[rango].Value = fila1;
            //objHojaExcel.Range[rango].Font.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#595959"));
            objHojaExcel.Range[rango].Font.Bold = true;
            //objHojaExcel.Range[rango].Font.Size = 12;

            filaExcel++;
            rango = "A" + filaExcel + ":" + "H" + filaExcel;
            //Crear el encabezado de las columnas
            objHojaExcel.Range[rango].Font.Bold = true;

            Range range2 = objHojaExcel.get_Range(rango);
            Object[] args2 = new Object[1];
            args2[0] = cabecerasColumnas;
            range2.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range2, args2);
            //range2.Cells.Interior.Color = System.Drawing.Color.LightBlue;

            range2.Cells.Interior.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#C5D9F1"));
        }
        
        private void InformeExcelEscribirCabeceraComprobante(string[] cabeceraComprobante)
        {
            //return;

            filaExcel++;
            //Crear el encabezado del informe
            string rango = "A" + filaExcel + ":" + "H" + filaExcel;
            //objHojaExcel.Range[rango].Font.Bold = true;

            Range range2 = objHojaExcel.get_Range(rango);
            //Object[] args2 = new Object[1];
            //args2[0] = cabeceraComprobante;
            //range2.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range2, args2);

            string celda;
            Microsoft.Office.Interop.Excel.Range oCelda;
            //Número del comprobante, escribirlo como numérico para que aparezca con este formato en el fichero excel
            if (cabeceraComprobante[0] == "") objHojaExcel.Cells[filaExcel, "A"] = cabeceraComprobante[0];
            else
            {
                celda = "A" + filaExcel;
                oCelda = objHojaExcel.get_Range(celda, celda);
                //SMR oCelda.EntireColumn.NumberFormat = "@";
                oCelda.NumberFormat = "@";
                //oCelda.EntireColumn.NumberFormat = "0";
                //objHojaExcel.Range[oCelda].Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                objHojaExcel.Cells[filaExcel, "A"] = Convert.ToInt32(cabeceraComprobante[0]);
            }

            celda = "B" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "B"] = cabeceraComprobante[1].ToString();

            celda = "C" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "C"] = cabeceraComprobante[2].ToString();

            celda = "D" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "D"] = cabeceraComprobante[3].ToString();

            objHojaExcel.Cells[filaExcel, "E"] = cabeceraComprobante[4].ToString();
            objHojaExcel.Cells[filaExcel, "F"] = cabeceraComprobante[5].ToString();
            objHojaExcel.Cells[filaExcel, "G"] = cabeceraComprobante[6].ToString();
            objHojaExcel.Cells[filaExcel, "H"] = cabeceraComprobante[7].ToString();

            ColorConverter cc = new ColorConverter();
            range2.Cells.Interior.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#D8D8D8"));
        }

        private void InformeExcelEscribirDetalleComprobante(string[] detalleComprobante)
        {
            //return;

            filaExcel++;

            string celda = "A" + filaExcel;
            Microsoft.Office.Interop.Excel.Range oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            //oCelda.EntireColumn.NumberFormat = "0"; 
            //objHojaExcel.Range[oCelda].Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            objHojaExcel.Cells[filaExcel, "A"] = detalleComprobante[0].ToString();

            celda = "B" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@"; 
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "B"] = detalleComprobante[1].ToString();

            celda = "C" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@"; 
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "C"] = detalleComprobante[2].ToString();

            celda = "D" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@"; 
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "D"] = detalleComprobante[3].ToString();

            celda = "E" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "E"] = detalleComprobante[4].ToString();
            
            celda = "F" + filaExcel;
            oCelda = objHojaExcel.get_Range(celda, celda);
            //SMR oCelda.EntireColumn.NumberFormat = "@";
            oCelda.NumberFormat = "@";
            objHojaExcel.Cells[filaExcel, "F"] = detalleComprobante[5].ToString();

            if (detalleComprobante[6] == "") objHojaExcel.Cells[filaExcel, "G"] = detalleComprobante[6];
            else
            {
                celda = "G" + filaExcel;
                oCelda = objHojaExcel.get_Range(celda, celda);
                //SMR oCelda.EntireColumn.NumberFormat = "#,##0.00";
                oCelda.NumberFormat = "#,##0.00";
                objHojaExcel.Cells[filaExcel, "G"] = Convert.ToDecimal(detalleComprobante[6]);
            }

            if (detalleComprobante[7] == "") objHojaExcel.Cells[filaExcel, "H"] = detalleComprobante[7];
            else
            {
                celda = "H" + filaExcel;
                oCelda = objHojaExcel.get_Range(celda, celda);
                //SMR oCelda.EntireColumn.NumberFormat = "#,##0.00";
                oCelda.NumberFormat = "#,##0.00";
                objHojaExcel.Cells[filaExcel, "H"] = Convert.ToDecimal(detalleComprobante[7]);
            }

            string rango;

            //Linea de Totales
            if (detalleComprobante[0] == "" && detalleComprobante[1] == "" && detalleComprobante[2] == "" && 
                detalleComprobante[3] == "" && detalleComprobante[4] == "")
            {
                rango = "A" + filaExcel + ":" + "H" + filaExcel;
                objHojaExcel.Range[rango].Font.Bold = true;
            }

            ColorConverter cc = new ColorConverter();
            rango = "A" + filaExcel + ":" + "H" + filaExcel;
            Range range2 = objHojaExcel.get_Range(rango);
            range2.Cells.Interior.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#F2F2F2"));
        }

        private void InformeExcelEscribirAjustesGlobales()
        {
            string rango = "A1:" + "H" + filaExcel;
            Microsoft.Office.Interop.Excel.Range objRango = objHojaExcel.Range[rango];
            //Ajustamos el ancho de las columnas al ancho máximo del contenido de sus celdas
            objRango.Columns.AutoFit();
           
            //objLibroExcel.PrintPreview();
            excelApp.Visible = true;
        }
        */
        #endregion

        #region InformeDetalladoHTML     HTML
        private void InformeHTMLEscribirCabeceras(string fila1, string[] cabecerasColumnas)
        {
            documento_HTML.Append("     <title>" + fila1 + "</title>\n");
            documento_HTML.Append("     <style>\n");
            documento_HTML.Append("        .NumeroCG {mso-number-format:\\#\\,\\#\\#0\\.00;text-align=right;}\n");
            documento_HTML.Append("        .NumeroCGLeft {mso-number-format:\"0\";text-align:left; background-color:#D8D8D8}\n");
            documento_HTML.Append(@"        .Texto    { mso-number-format:\@; }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTIT    { mso-number-format:\@;font-weight:700; background-color:#D8D8D8 }");
            documento_HTML.Append("\n");
            documento_HTML.Append(@"        .TextoTITCab    { mso-number-format:\@;font-weight:700; background-color:#C5D9F1}");
            documento_HTML.Append("\n");
            documento_HTML.Append("     </style>\n");
            documento_HTML.Append(" </head>\n");
            documento_HTML.Append(" <body>\n");
            documento_HTML.Append("     <b>" + fila1 + "</b>\n");

            documento_HTML.Append("     <table width =\"100%\">\n");
            documento_HTML.Append("         <tr>\n");
            string valor = (cabecerasColumnas[0] == "") ? "&nbsp;" : cabecerasColumnas[0];
            documento_HTML.Append("             <td class=TextoTITCab width =\"8%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[1] == "") ? "&nbsp;" : cabecerasColumnas[1];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[2] == "") ? "&nbsp;" : cabecerasColumnas[2];
            documento_HTML.Append("             <td class=TextoTITCab width =\"12%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[3] == "") ? "&nbsp;" : cabecerasColumnas[3];
            documento_HTML.Append("             <td class=TextoTITCab width =\"20%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[4] == "") ? "&nbsp;" : cabecerasColumnas[4];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[5] == "") ? "&nbsp;" : cabecerasColumnas[5];
            documento_HTML.Append("             <td class=TextoTITCab width =\"20%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[6] == "") ? "&nbsp;" : cabecerasColumnas[6];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            valor = (cabecerasColumnas[7] == "") ? "&nbsp;" : cabecerasColumnas[7];
            documento_HTML.Append("             <td class=TextoTITCab width =\"10%\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("     </table>\n");
        }

        private void InformeHTMLEscribirCabeceraComprobante(string[] cabeceraComprobante)
        {
            documento_HTML.Append("     <table width =\"100%\">\n");
            documento_HTML.Append("         <tr>\n");
            string valor = (cabeceraComprobante[0] == "") ? "&nbsp;" : cabeceraComprobante[0];
            documento_HTML.Append("             <td class=NumeroCGLeft width =\"8%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[1] == "") ? "&nbsp;" : cabeceraComprobante[1];
            documento_HTML.Append("             <td class=TextoTIT width =\"10%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[2] == "") ? "&nbsp;" : cabeceraComprobante[2];
            documento_HTML.Append("             <td class=TextoTIT width =\"12%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[3] == "") ? "&nbsp;" : cabeceraComprobante[3];
            documento_HTML.Append("             <td class=TextoTIT width =\"20%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[4] == "") ? "&nbsp;" : cabeceraComprobante[4];
            documento_HTML.Append("             <td class=TextoTIT width =\"10%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[5] == "") ? "&nbsp;" : cabeceraComprobante[5];
            documento_HTML.Append("             <td class=TextoTIT width =\"20%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[6] == "") ? "&nbsp;" : cabeceraComprobante[6];
            documento_HTML.Append("             <td class=TextoTIT width =\"10%\">" + valor + "</td>\n");
            valor = (cabeceraComprobante[7] == "") ? "&nbsp;" : cabeceraComprobante[7];
            documento_HTML.Append("             <td class=TextoTIT width =\"10%\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            documento_HTML.Append("     </table>\n");
        }

        private void InformeHTMLEscribirDetalleComprobante(string[] detalleComprobante)
        {
            bool lineaTotales = false;
            if (detalleComprobante[0] == "" && detalleComprobante[1] == "" && detalleComprobante[2] == "" &&
                detalleComprobante[3] == "" && detalleComprobante[4] == "")
            {
                lineaTotales = true;
            }
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (detalleComprobante[0] == "") ? "&nbsp;" : detalleComprobante[0];
            documento_HTML.Append("             <td class=Texto width =\"8%\">" + valor + "</td>\n");
            valor = (detalleComprobante[1] == "") ? "&nbsp;" : detalleComprobante[1];
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[2] == "") ? "&nbsp;" : detalleComprobante[2];
            documento_HTML.Append("             <td class=Texto width =\"12%\">" + valor + "</td>\n");
            valor = (detalleComprobante[3] == "") ? "&nbsp;" : detalleComprobante[3];
            documento_HTML.Append("             <td class=Texto width =\"20%\">" + valor + "</td>\n");
            valor = (detalleComprobante[4] == "") ? "&nbsp;" : detalleComprobante[4];
            documento_HTML.Append("             <td class=Texto width =\"10%\">" + valor + "</td>\n");
            valor = (detalleComprobante[5] == "") ? "&nbsp;" : detalleComprobante[5];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=Texto width =\"20%\">" + valor + "</td>\n");
            valor = (detalleComprobante[6] == "") ? "&nbsp;" : detalleComprobante[6];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"10%\" align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[7] == "") ? "&nbsp;" : detalleComprobante[7];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG width =\"10%\" align=\"right\">" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            //documento_HTML.Append("     </table>");
        }
        #endregion


        /// <summary>
        /// Ejecutar el informe
        /// </summary>
        private void Ejecutar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                //filaExcel = 0;
                //this.ProcesarInforme();
                backgroundWorker1.RunWorkerAsync();

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

                System.Data.DataTable dtPeticiones = frmPeticion.ListarPeticion();

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
                        { "Fecha Desde", "dateTimeDesdeFecha" },
                        { "Fecha Hasta", "dateTimeHastaFecha" },
                        { "Tipo de informe", "radDropDownListTipoInforme" },
                        { "Titulo", "txttitulo" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "rbPeriodos", "rbFecha", "chkImportarTotales", "chkCuentaEditada", "txtNoPrimerComp", "txtDesdePeriodos", "txtHastaPeriodos" });

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
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCIDIARIO myStruct = (StructGLL01_MCIDIARIO)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCIDIARIO));

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

                            this.cmbCompania.Select();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.compania.Trim() == "" && myStruct.grupo.Trim() != "")
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
                        this.codigoPlan = myStruct.plan.Trim();
                    
                        string descPlan = "";
                        string resultValidarPlan = this.ValidarPlan(this.codigoPlan, ref descPlan);

                        if (resultValidarPlan == "")
                        {
                            try
                            {
                                this.radDropDownListPlan.SelectedValue = this.codigoPlan;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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
                    catch (Exception ex)
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
                    switch (myStruct.tipoInforme)
                    {
                        case "RF":
                            this.radDropDownListTipoInforme.SelectedIndex = 1;
                            break;
                        case "RP":
                            this.radDropDownListTipoInforme.SelectedIndex = 2;
                            break;
                        case "DE":
                        default:
                            this.radDropDownListTipoInforme.SelectedIndex = 0;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    this.radDropDownListTipoInforme.SelectedIndex = 0;
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                if (myStruct.cuentaEditada == "1") this.chkCuentaEditada.Checked = true;
                else this.chkCuentaEditada.Checked = false;

                if (myStruct.importarTotales == "1") this.chkImportarTotales.Checked = true;
                else this.chkImportarTotales.Checked = false;

                this.txtNoPrimerComp.Text = myStruct.noPrimerComp.Trim();

                this.txttitulo.Text = myStruct.titulo.Trim();

                result = true;

                Marshal.FreeBSTR(pBuf); 
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.cmbCompania.Focus();

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
                StructGLL01_MCIDIARIO myStruct;
                
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

                string tipoInforme = "DE";
                if (this.radDropDownListTipoInforme.SelectedValue != null) tipoInforme = this.radDropDownListTipoInforme.SelectedValue.ToString();
                myStruct.tipoInforme = tipoInforme;
               
                if (this.chkCuentaEditada.Checked) myStruct.cuentaEditada = "1";
                else myStruct.cuentaEditada = "0";

                if (this.chkImportarTotales.Checked) myStruct.importarTotales = "1";
                else myStruct.importarTotales = "0";

                myStruct.noPrimerComp = this.txtNoPrimerComp.Text.PadRight(9, ' ');

                myStruct.titulo = this.txttitulo.Text.PadRight(100, ' ');
                myStruct.titulo = myStruct.titulo.Replace("'", "''" );

                result = myStruct.compania + myStruct.grupo + myStruct.plan + myStruct.porFechaporPeriodo + myStruct.fechaDesde + myStruct.fechaHasta;
                result += myStruct.periodoDesde + myStruct.periodoHasta + myStruct.tipoInforme + myStruct.cuentaEditada + myStruct.importarTotales + myStruct.noPrimerComp + myStruct.titulo;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIDIARIO));

                /*int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIDIARIO));
                Byte[] ret = new Byte[objsize];
                IntPtr buff = Marshal.AllocHGlobal(objsize);
                Marshal.StructureToPtr(myStruct, buff, true);
                Marshal.Copy(buff, ret, 0, objsize);
                Marshal.FreeHGlobal(buff);

                result = System.Text.Encoding.UTF8.GetString(ret);*/
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (result.Length < 150) result.PadRight(150, ' ');

            return (result);
        }

        /*
        /// <summary>
        /// Se invoca al cambiar de valor el control del grupo
        /// (Valida que el código del grupo sea correcto. Escribe la descripción del grupo. Pone el focus en el control del plan si
        /// el parámetro de entrada es True, si es Falso deja el focus en el control de Grupo)
        /// </summary>
        /// <param name="planFocus"></param>
        /// <returns></returns>
        private bool GrupoValorRefresh(bool planFocus)
        {
            bool result = true;

            Cursor.Current = Cursors.WaitCursor;

            if (this.tgTexBoxSelGrupo.Textbox.Text.Trim() != "")
            {
                //this.tgTexBoxSelGrupo.Textbox.Modified = false;

                string codigo = this.tgTexBoxSelGrupo.Textbox.Text.Trim();

                if (codigo != "" && codigo.Length >= 2)
                {
                    if (codigo.Length <= 2) this.codigoGrupo = this.tgTexBoxSelGrupo.Textbox.Text;
                    else this.codigoGrupo = this.tgTexBoxSelGrupo.Textbox.Text.Substring(0, 2);

                    string grupoDesc = "";
                    string resultValGrupo = this.ValidarGrupo(this.codigoGrupo, ref grupoDesc, false);

                    if (resultValGrupo != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(resultValGrupo, error);
                        this.tgTexBoxSelCompania.Textbox.Text = "";
                        this.tgTexBoxSelGrupo.Textbox.Focus();
                        return (false);
                    }
                    else
                    {
                        string codGrupo = this.codigoGrupo;
                        if (grupoDesc != "") codGrupo += " " + this.separadorDesc + " " + grupoDesc;

                        this.tgTexBoxSelGrupo.Textbox.Text = codGrupo;
                        this.tgTexBoxSelCompania.Textbox.Text = "";
                        this.tgTexBoxSelPlan.Enabled = true;

                        //Actualiza la consulta para la selección del plan (dependiendo del código de grupo de compañías seleccionado)
                        this.ActualizarQueryTGTextBoxSelPlan();

                        if (planFocus) this.tgTexBoxSelPlan.Textbox.Focus();
                    }
                }
                else
                {
                    this.tgTexBoxSelGrupo.Textbox.Focus();
                }
            }
            Cursor.Current = Cursors.Default;

            return (result);
        }*/
        #endregion
    }
}