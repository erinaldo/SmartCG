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
using Microsoft.Office.Interop.Excel;
using ObjectModel;
using System.Diagnostics;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmInfoMovIVAAltaEdita : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCIMOVIVA";
        public string formCodeNombFichero = "MOVIIVA";
        public string ficheroExtension = "miv";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCIMOVIVA
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string desdeAno;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string desdeMes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string hastaAno;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string hastaMes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string totalSerie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string titulo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string tipoTransa;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string soportado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string repercutido;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string series;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
            public string companias;
        } 

        private string tipoFichero = "EXCEL";

        string[] cabeceraComp = new string[8];

        ArrayList aCompFiscales;

        StringBuilder documento_HTML;

        FormularioValoresCampos valoresFormulario;

        private ArrayList avisosAutorizaciones;

        bool existenRegistros;

        private decimal totalSerieBaseImp;
        private decimal totalSerieCuotaIva;
        private decimal totalSerieCuotaIvaNoDed_RecargoEquiv;
        private decimal totalSerieTotalFact;

        private decimal totalLibroBaseImp;
        private decimal totalLibroCuotaIva;
        private decimal totalLibroCuotaIvaNoDed_RecargoEquiv;
        private decimal totalLibroTotalFact;

        private decimal totalCompBaseImp;
        private decimal totalCompCuotaIva;
        private decimal totalCompCuotaIvaNoDed_RecargoEquiv;
        private decimal totalCompTotalFact;

        Dictionary<string, string> dictRazonSocial = new Dictionary<string, string>();

        string anoDesde = "";
        string mesDesde = "";
        string anoHasta = "";
        string mesHasta = "";

        private string nombreFicheroAGenerar = "";
        private string tipoFicheroAGenerar = GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes;

        private string mensajeProceso = "";

        public frmInfoMovIVAAltaEdita()
        {
            InitializeComponent();

            this.gBoxListaCompFiscales.ElementTree.EnableApplicationThemeName = false;
            this.gBoxListaCompFiscales.ThemeName = "ControlDefault";

            this.gbLibroSerie.ElementTree.EnableApplicationThemeName = false;
            this.gbLibroSerie.ThemeName = "ControlDefault";

            this.gbTipoTrans.ElementTree.EnableApplicationThemeName = false;
            this.gbTipoTrans.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoMovIVAAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Movimientos de IVA");

            //Obtener el tipo de fichero
            string tipoFicherosInformes = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosInformes"];
            if (tipoFicherosInformes != null && tipoFicherosInformes != "") if (tipoFicherosInformes == "HTML") tipoFichero = tipoFicherosInformes;

            utiles.ButtonEnabled(ref this.btnAddCompFiscal, false);
            utiles.ButtonEnabled(ref this.btnQuitarCompFiscal, false);
            utiles.ButtonEnabled(ref this.btnQuitarTodasCompFiscal, false);

            utiles.ButtonEnabled(ref this.btnSerieDel, false);
            utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);

            //Traducir los literales
            this.TraducirLiterales();

            //Array de Compañías Fiscales a Procesar
            aCompFiscales = new ArrayList();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    //Iniciar el Campo titulo con el nombre del formulario
                    this.txttitulo.Text = this.Text;

                    this.radButtonTextBoxSelCompaniaFiscal.Select();
                }
            }
            else
            {
                //Iniciar el Campo titulo con el nombre del formulario
                this.txttitulo.Text = this.Text;

                this.radButtonTextBoxSelCompaniaFiscal.Select();
            }
        }

        private void RbLibroSerie_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void RbTipoTrans_CheckStateChanged(object sender, EventArgs e)
        {
            this.RadioButtonChange();
        }

        private void BtnAddCompFiscal_Click(object sender, EventArgs e)
        {
            string error = this.LP.GetText("errValTitulo", "Error");

            string codigo;
            if (this.radButtonTextBoxSelCompaniaFiscal.Text.Length <= 2) codigo = this.radButtonTextBoxSelCompaniaFiscal.Text;
            else codigo = this.radButtonTextBoxSelCompaniaFiscal.Text.Substring(0, 2);

            if (codigo == "")
            {
                RadMessageBox.Show("Debe introducir o seleccionar una compañía fiscal", error);    //Falta traducir
                this.radButtonTextBoxSelCompaniaFiscal.Select();
                return;
            }

            string desc = "";
            string validarCompFiscal = this.ValidarCompania(codigo, ref desc, false);

            if (validarCompFiscal != "")
            {
                RadMessageBox.Show(validarCompFiscal, error);
                this.radButtonTextBoxSelCompaniaFiscal.Select();
                return;
            }

            string result = this.AddToListBox(codigo, ref this.lbCompFiscal);
            switch (result)
            {
                case "":
                    //Adicionarla al array de compañias fiscales
                    string[] datosCompaniaFiscal = new string[2];
                    datosCompaniaFiscal[0] = codigo;
                    datosCompaniaFiscal[1] = desc;
                    this.aCompFiscales.Add(datosCompaniaFiscal);
                    this.radButtonTextBoxSelCompaniaFiscal.Text = "";
                    this.radButtonTextBoxSelCompaniaFiscal.Select();
                    break;
                case "1":
                    RadMessageBox.Show(this.LP.GetText("errCompFiscalExiste", "La compañía fiscal ya está en la lista"), error);
                    this.radButtonTextBoxSelCompaniaFiscal.Text = "";
                    this.radButtonTextBoxSelCompaniaFiscal.Select();
                    break;
            }
        }

        private void BtnQuitarCompFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                RadListDataItem item = this.lbCompFiscal.SelectedItem;
                this.lbCompFiscal.Items.Remove(item);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCompFiscal.Items.Count == 0)
            {
                this.radButtonTextBoxSelCompaniaFiscal.Select();
                utiles.ButtonEnabled(ref this.btnQuitarCompFiscal, false);
                utiles.ButtonEnabled(ref this.btnQuitarTodasCompFiscal, false);
            }
            else this.lbCompFiscal.Select();
        }

        private void BtnQuitarTodasCompFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lbCompFiscal.Items.Count;)
                {
                    RadListDataItem item = this.lbCompFiscal.Items[i];
                    this.lbCompFiscal.Items.Remove(item);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCompFiscal.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCompFiscal, false);
                utiles.ButtonEnabled(ref this.btnQuitarTodasCompFiscal, false);
                this.radButtonTextBoxSelCompaniaFiscal.Select();
            }
        }

        private void BtnSerieAdd_Click(object sender, EventArgs e)
        {
            if (this.txtSerie.Text.Trim() != "")
            {
                string result = this.AddToListBox(this.txtSerie.Text, ref this.lbSerie);
                switch (result)
                {
                    case "":
                        this.txtSerie.Text = "";
                        this.txtSerie.Focus();
                        break;
                    case "1":
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(this.LP.GetText("errSerieExiste", "La serie ya está en la lista"), error);
                        this.txtSerie.Focus();
                        break;
                }

                /*
                if (this.lbSerie.Items.Count > 0)
                {
                    utiles.ButtonEnabled(ref this.btnSerieDel, true);
                    utiles.ButtonEnabled(ref this.btnSerieDelTodas, true);
                }
                */
            }
        }

        private void BtnSerieDel_Click(object sender, EventArgs e)
        {
            try
            {
                RadListDataItem item = this.lbSerie.SelectedItem;
                this.lbSerie.Items.Remove(item);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbSerie.Items.Count == 0)
            {
                this.txtSerie.Select();
                utiles.ButtonEnabled(ref this.btnSerieDel, false);
                utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);
            }
            else this.lbSerie.Select();
        }

        private void BtnSerieDelTodas_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lbSerie.Items.Count;)
                {
                    RadListDataItem item = this.lbSerie.Items[i];
                    this.lbSerie.Items.Remove(item);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbSerie.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnSerieDel, false);
                utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);
                this.radButtonTextBoxSelCompaniaFiscal.Select();
            }
            else this.lbSerie.Select();
        }

        private void LbSerie_Enter(object sender, EventArgs e)
        {
            if (this.lbSerie.Items.Count > 0)
            {
                utiles.ButtonEnabled(ref this.btnSerieDel, true);
                utiles.ButtonEnabled(ref this.btnSerieDelTodas, true);
            }
        }

        private void TxtLibro_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnSerieDel, false);
            utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);

            this.lbSerie.SelectedIndex = -1;
        }

        private void LbCompFiscal_Enter(object sender, EventArgs e)
        {
            if (this.lbCompFiscal.Items.Count > 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCompFiscal, true);
                utiles.ButtonEnabled(ref this.btnQuitarTodasCompFiscal, true);
            }
        }

        private void TgTexBoxSelCompaniaFiscal_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnQuitarCompFiscal, false);
            utiles.ButtonEnabled(ref this.btnQuitarTodasCompFiscal, false);

            this.lbCompFiscal.SelectedIndex = -1;
        }

        private void TxtSerie_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnSerieDel, false);
            utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);

            this.lbSerie.SelectedIndex = -1;
        }

        private void FrmListarPeticiones_OkForm(TGPeticionesListar.OkFormCommandEventArgs e)
        {
            FormularioPeticion frmPeticion = new FormularioPeticion
            {
                FormCode = this.formCode,
                Formulario = this
            };
            string result = frmPeticion.CargarPeticionDataTable(((System.Data.DataTable)e.Valor));
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

        private void RadButtonElementSelCompaniaFiscal_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CIAFT3, NOMBT3, NIFDT3 from " + GlobalVar.PrefijoTablaCG + "IVT03 ";
            query += "order by CIAFT3";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción"),
                this.LP.GetText("lblListaCampoNIT", "NIT")     //Falta traducir ... y darlo de alta
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar compañía fiscal",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new System.Drawing.Point(0, 0),
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
                this.radButtonTextBoxSelCompaniaFiscal.Text = result;
                this.ActiveControl = this.radButtonTextBoxSelCompaniaFiscal;
                this.radButtonTextBoxSelCompaniaFiscal.Select(0, 0);
                this.radButtonTextBoxSelCompaniaFiscal.Focus();
            }
        }

        private void RadButtonTextBoxSelCompaniaFiscal_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxSelCompaniaFiscal.Text.Trim() == "") utiles.ButtonEnabled(ref this.btnAddCompFiscal, false);
            else utiles.ButtonEnabled(ref this.btnAddCompFiscal, true);
        }

        private void RadButtonTextBoxSelCompaniaFiscal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0")
            {
                string caracter = e.KeyChar.ToString().ToUpper();
                e.KeyChar = Convert.ToChar(caracter);
            }
        }

        private void TxtSerie_TextChanged(object sender, EventArgs e)
        {
            if (this.txtSerie.Text.Trim() == "") utiles.ButtonEnabled(ref this.btnSerieAdd, false);
            else utiles.ButtonEnabled(ref this.btnSerieAdd, true);
        }

        private void BtnAddCompFiscal_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAddCompFiscal);
        }

        private void BtnAddCompFiscal_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAddCompFiscal);
        }

        private void BtnQuitarCompFiscal_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarCompFiscal);
        }
        private void BtnQuitarCompFiscal_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarCompFiscal);
        }

        private void BtnQuitarTodasCompFiscal_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarTodasCompFiscal);
        }

        private void BtnQuitarTodasCompFiscal_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarTodasCompFiscal);
        }

        private void BtnSerieAdd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnSerieAdd);
        }

        private void BtnSerieAdd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnSerieAdd);
        }

        private void BtnSerieDel_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnSerieDel);
        }

        private void BtnSerieDel_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnSerieDel);
        }

        private void BtnSerieDelTodas_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnSerieDelTodas);
        }

        private void BtnSerieDelTodas_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnSerieDelTodas);
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
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", "Movimientos de IVA", MessageBoxButtons.YesNo, RadMessageIcon.Question);

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

        private void FrmInfoMovIVAAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Movimientos de IVA");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemMovIVA", "Movimientos de IVA");
            this.Text = this.Text.Replace("&", "");
            this.gBoxListaCompFiscales.Text = this.LP.GetText("lblListaCompFiscales", "Lista de Compañías Fiscales");
            this.btnAddCompFiscal.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnQuitarCompFiscal.Text = this.LP.GetText("lblQuitar", "Quitar");
            this.rbLibroSerie.Text = this.LP.GetText("lblLibroSerie", "Por Libro/Serie");
            this.lblLibro.Text = this.LP.GetText("lblLibro", "Libro");
            this.lblSerie.Text = this.LP.GetText("lblSerie", "Serie");
            this.btnSerieAdd.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnSerieDel.Text = this.LP.GetText("lblQuitar", "Quitar");
            this.rbTipoTrans.Text = this.LP.GetText("lblTipoTransac", "Por Tipo de Transacción");
            this.rbSoportado.Text = this.LP.GetText("lblSoportado", "Soportado");
            this.rbRepercutido.Text = this.LP.GetText("lblRepercutido", "Repercutido");
            this.lblAMDesde.Text = this.LP.GetText("lblAnoMesDesde", "Año-Mes Desde");
            this.lblAMHasta.Text = this.LP.GetText("lblAnoMesHasta", "Año-Mes Hasta");
            this.chkTotalSerie.Text = this.LP.GetText("lblTotalPorSerie", "Total por Serie");

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Habilita y Deshabilita los controles relacionados con los grupos de Libro/serie y de Tipo de Transaccion
        /// </summary>
        private void RadioButtonChange()
        {
            if (this.rbLibroSerie.IsChecked)
            {
                this.gbLibroSerie.Enabled = true;
                this.txtLibro.Enabled = true;
                this.txtSerie.Enabled = true;
                this.lbSerie.Enabled = true;
                //utiles.ButtonEnabled(ref this.btnSerieAdd, true);
                //utiles.ButtonEnabled(ref this.btnSerieDel, true);
                //utiles.ButtonEnabled(ref this.btnSerieDelTodas, true);
                this.rbTipoTrans.IsChecked = false;
                this.gbTipoTrans.Enabled = false;
                this.rbSoportado.IsChecked = false;
                this.rbRepercutido.IsChecked = false;
                this.txtLibro.Focus();
            }
            else
            {
                this.gbLibroSerie.Enabled = false;
                this.txtLibro.Enabled = false;
                this.txtLibro.Text = "";
                this.txtSerie.Enabled = false;
                this.txtSerie.Text = "";
                this.lbSerie.Enabled = false;
                utiles.ButtonEnabled(ref this.btnSerieAdd, false);
                utiles.ButtonEnabled(ref this.btnSerieDel, false);
                utiles.ButtonEnabled(ref this.btnSerieDelTodas, false);
                this.rbTipoTrans.IsChecked = true;
                this.gbTipoTrans.Enabled = true;
                this.rbSoportado.IsChecked = true;
                this.rbRepercutido.IsChecked = false;

                //Eliminar todos los elementos de la lista de las series
                this.lbSerie.Items.Clear();
            }
        }
        
        /// <summary>
        /// Validar el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            string error = this.LP.GetText("errValTitulo", "Error");

            if (this.lbCompFiscal.Items.Count <= 0)
            {
                if (this.radButtonTextBoxSelCompaniaFiscal.Text.Trim() != "")
                {
                    //Adicionar la compañia fiscal seleccionada a la lista de compañías fiscales

                    string codigo;
                    if (this.radButtonTextBoxSelCompaniaFiscal.Text.Length <= 2) codigo = this.radButtonTextBoxSelCompaniaFiscal.Text;
                    else codigo = this.radButtonTextBoxSelCompaniaFiscal.Text.Substring(0, 2);

                    string desc = "";
                    string validarCompFiscal = this.ValidarCompania(codigo, ref desc, false);

                    if (validarCompFiscal != "")
                    {
                        RadMessageBox.Show(validarCompFiscal, error);
                        this.radButtonTextBoxSelCompaniaFiscal.Select();
                        return (false);
                    }

                    this.lbCompFiscal.Items.Add(codigo);

                    //Adicionarla al array de compañias fiscales
                    string[] datosCompaniaFiscal = new string[2];
                    datosCompaniaFiscal[0] = codigo;
                    datosCompaniaFiscal[1] = desc;
                    this.aCompFiscales.Add(datosCompaniaFiscal);
                }
                else 
                {
                    RadMessageBox.Show(this.LP.GetText("errCompaniaFiscalObl", "Es obligatorio informar la compañía fiscal"), error);
                    this.radButtonTextBoxSelCompaniaFiscal.Select();
                    return (false);
                }
            }

            if (this.rbLibroSerie.IsChecked)
            {
                if (this.txtLibro.Text.Trim() == "")
                {
                    RadMessageBox.Show(this.LP.GetText("errLibroObl", "Es obligatorio informar el Libro"), error);
                    this.txtLibro.Focus();
                    return (false);
                }
            }

            this.txtMaskDesdeAnoMes.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            this.txtDesdeAnoMes.Text = this.txtMaskDesdeAnoMes.Value.ToString().Trim();
            this.txtMaskDesdeAnoMes.TextMaskFormat = MaskFormat.IncludeLiterals;
            string resultMsg = this.ValidarAnoMes(ref this.txtDesdeAnoMes, ref anoDesde, ref mesDesde);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskDesdeAnoMes.Focus();
                return (false);
            }

            this.txtMaskHastaAnoMes.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            this.txtHastaAnoMes.Text = this.txtMaskHastaAnoMes.Value.ToString().Trim();
            this.txtMaskHastaAnoMes.TextMaskFormat = MaskFormat.IncludeLiterals;
            resultMsg = this.ValidarAnoMes(ref this.txtHastaAnoMes, ref anoHasta, ref mesHasta);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskHastaAnoMes.Focus();
                return (false);
            }
            
            return (true);
        }

        /// <summary>
        /// Validar el año-mes
        /// </summary>
        /// <param name="txtAnoMes">año-mes</param>
        /// <param name="anoReturn">devuelve el año</param>
        /// <param name="mesReturn">devuelve el mes</param>
        /// <returns></returns>
        public string ValidarAnoMes(ref System.Windows.Forms.TextBox txtAnoMes, ref string anoReturn, ref string mesReturn)
        {
            string result = "";

            if (txtAnoMes.Text.Trim() == "")
            {
                result = "Es obligatorio informar el año-mes";
                txtAnoMes.Focus();
                return (result);
            }

            //Validar año-mes
            txtAnoMes.Text = txtAnoMes.Text.PadRight(4, '0');
            try
            {
                anoReturn = txtAnoMes.Text.Substring(0, 2);
                int ano = Convert.ToInt32(anoReturn);

                if (ano < 0)
                {
                    result = this.LP.GetText("errAnoFormatoIncorrecto", "Año incorrecto");
                    txtAnoMes.Focus();
                    return (result);
                }
            }
            catch
            {
                result = this.LP.GetText("errAnoFormatoIncorrecto", "Año incorrecto");
                txtAnoMes.Focus();
                return (result);
            }

            try
            {
                mesReturn = txtAnoMes.Text.Substring(2, 2);
                int mes = Convert.ToInt32(mesReturn);

                if (!(mes >= 1 && mes <= 12))
                {
                    result = this.LP.GetText("errMesFormatoIncorrecto", "Mes incorrecto");
                    txtAnoMes.Focus();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errMesFormatoIncorrecto", "Mes incorrecto");
                txtAnoMes.Focus();
                return (result);
            }

            return (result);
        }

        /// <summary>
        /// Crear el informe
        /// </summary>
        private void ProcesarInforme()
        {
            try
            {
                this.mensajeProceso = "";

                //Iniciar la barra de progreso
                this.backgroundWorker1.ReportProgress(0);

                //Mover la barra de progreso
                this.backgroundWorker1.ReportProgress(2);

                //Crear el fichero en memoria HTML
                this.InformeHTMLCrear(ref this.documento_HTML);

                //Iniciar la barra de progreso
                this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Marquee);

                //Eliminar los posibles permisos de no autorizado del usuario conectado
                GlobalVar.UsuarioEnv.ListaNoAutorizado.Clear();

                //Inicializar los avisos de autorizaciones
                this.avisosAutorizaciones = new ArrayList();

                string[] datosCompaniasFiscales;
                for (int i = 0; i < this.lbCompFiscal.Items.Count; i++)
                {
                    if (i == 0) this.InformeHTMLEscribirEncabezado(this.lbCompFiscal.Items[i].ToString());
                    datosCompaniasFiscales = (string[])aCompFiscales[i];
                    this.ProcesarCompaniaFiscal(this.lbCompFiscal.Items[i].ToString(), datosCompaniasFiscales[1].ToString());

                    if (i + 1 != this.lbCompFiscal.Items.Count && existenRegistros) this.EscribirLineaEnBlanco("#FFFFFF");

                    if (existenRegistros) this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);
                }

                if (existenRegistros)
                {
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

                    /*
                    //Escribir los ajustes globales en el fichero HTML / Excel

                    //SMR levantar html con excel
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

                    //Finaliza la barra de progreso
                    this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
                    */
                }
                else
                {
                    //Finaliza la barra de progreso
                    //this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                    //RadMessageBox.Show(this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado")); 

                    this.mensajeProceso = this.LP.GetText("errNoRegistros", "No existen datos para el informe solicitado");

                    //Finaliza la barra de progreso
                    this.backgroundWorker1.ReportProgress(1);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                this.mensajeProceso = "Error generando el informe. Para más información consulte el fichero de Log.";

                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
            }
        }

        /// <summary>
        /// Realiza el informe para cada compañía fiscal
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="nombre"></param>
        private void ProcesarCompaniaFiscal(string codigo, string nombre)
        {
            IDataReader dr = null;

            string librb3 = "";
            string serib3 = "";

            try
            {
                string query = this.ObtenerQuery(codigo);

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string[] datos = new string[18];
                int registros = 0;

                string resob3;
                decimal baimb3;
                decimal tpivb3;
                decimal cuotb3;
                decimal requb3;
                string dedub3;
                string pcifb3;
                string nitrb3;
                string tauxb3;

                string libroActual = "";
                string serieActual = "";

                decimal totalLinea = 0;
                decimal totalAcumuladoDocumento = 0;

                string claseActual = "";
                string documentoActual = "";
                string fechaDocumentoActual = "";
                string clase = "";
                string documento = "";
                string fechaDocumento = "";

                while (dr.Read())
                {
                    //Mover la barra de progreso
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;
                    this.backgroundWorker1.ReportProgress(2);

                    existenRegistros = true;

                    totalLinea = 0;
                    
                    resob3 = dr.GetValue(dr.GetOrdinal("RESOB3")).ToString();
                    librb3 = dr.GetValue(dr.GetOrdinal("LIBRB3")).ToString();
                    serib3 = dr.GetValue(dr.GetOrdinal("SERIB3")).ToString();

                    claseActual = dr.GetValue(dr.GetOrdinal("CLDOB3")).ToString();
                    documentoActual = dr.GetValue(dr.GetOrdinal("NDOCB3")).ToString();
                    fechaDocumentoActual = dr.GetValue(dr.GetOrdinal("FDOCB3")).ToString();

                    if (registros == 0)
                    {
                        //Cabecera de las columnas del informe
                        string[] columnas = this.ObtenerCabecerasColumnasInforme(resob3);

                        //Escribir el encabezado para la compañía
                        this.InformeHTMLEscribirCabeceras(nombre, columnas);

                        libroActual = librb3;
                        serieActual = serib3;

                        clase = claseActual;
                        documento = documentoActual;
                        fechaDocumento = fechaDocumentoActual;
                    }
                    else
                    {
                        if (clase != claseActual || documento != documentoActual || fechaDocumento != fechaDocumentoActual)
                        {
                            //Escribir el acumulado del documento
                            //Escribe la línea del detalle del comprobante en el fichero HTML
                            datos[15] = totalAcumuladoDocumento.ToString();
                            this.InformeHTMLEscribirDetalleComprobante(datos);

                            totalAcumuladoDocumento = 0;
                            clase = claseActual;
                            documento = documentoActual;
                            fechaDocumento = fechaDocumentoActual;
                        }
                        else
                        {
                            //Escribir el documento con Total Factura en blanco
                            //Escribe la línea del detalle del comprobante en el fichero HTML
                            datos[15] = "";
                            totalAcumuladoDocumento += totalLinea;
                            this.InformeHTMLEscribirDetalleComprobante(datos);
                        }
                    }

                    if (this.chkTotalSerie.Checked && (libroActual != librb3 || serieActual != serib3))
                    {
                        //Escribir totales
                        if (libroActual != librb3)
                        {
                            this.EscribirTotalSerie(serieActual);
                            this.EscribirLineaEnBlanco("#F2F2F2");

                            this.EscribirTotalLibro(libroActual);
                            this.EscribirLineaEnBlanco("#F2F2F2");
                            libroActual = librb3;
                        }
                        else
                        {
                            this.EscribirTotalSerie(serieActual);
                            this.EscribirLineaEnBlanco("#F2F2F2");
                            serieActual = serib3;
                        }
                    }

                    datos = new string[18];
                    datos[0] = dr.GetValue(dr.GetOrdinal("CIAFB3")).ToString();
                    datos[1] = librb3;
                    datos[2] = serib3;
                    datos[3] = dr.GetValue(dr.GetOrdinal("RECNB3")).ToString().PadLeft(6, '0');
                    datos[4] = utiles.FormatoCGToFecha(dr.GetValue(dr.GetOrdinal("FECOB3")).ToString()).ToShortDateString();
                    datos[5] = utiles.FormatoCGToFecha(fechaDocumentoActual).ToShortDateString();
                    datos[6] = claseActual.PadLeft(2, '0') + "-" + documentoActual.PadLeft(7, '0') ;
                    
                    pcifb3 = dr.GetValue(dr.GetOrdinal("PCIFB3")).ToString();
                    nitrb3 = dr.GetValue(dr.GetOrdinal("NITRB3")).ToString();
                    tauxb3 = dr.GetValue(dr.GetOrdinal("TAUXB3")).ToString();
                    datos[7] = pcifb3 + " " + nitrb3;
                    datos[8] = this.ObtenerNombreORazonSocial(pcifb3, nitrb3, tauxb3);
                    datos[9] = dr.GetValue(dr.GetOrdinal("DESCB3")).ToString();

                    baimb3 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("BAIMB3")));
                    tpivb3 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("TPIVB3")));
                    cuotb3 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("CUOTB3")));

                    switch (resob3)
                    {
                        case "S" :        
                            dedub3 = dr.GetValue(dr.GetOrdinal("DEDUB3")).ToString();

                            totalLinea = baimb3 + cuotb3;

                            datos[10] = baimb3.ToString();

                            switch (dedub3)
                            {
                                case "D":
                                    datos[11] = tpivb3.ToString();
                                    datos[12] = cuotb3.ToString();
                                    datos[13] = "";
                                    datos[14] = "";

                                    if (this.chkTotalSerie.Checked)
                                    {
                                        this.totalSerieCuotaIva += cuotb3;
                                        this.totalLibroCuotaIva += cuotb3;
                                        this.totalCompCuotaIva += cuotb3;
                                    }
                                    break;
                                case "N":
                                    datos[11] = "";
                                    datos[12] = "";
                                    datos[13] = tpivb3.ToString();
                                    datos[14] = cuotb3.ToString();

                                    if (this.chkTotalSerie.Checked)
                                    {
                                        this.totalSerieCuotaIvaNoDed_RecargoEquiv += cuotb3;
                                        this.totalLibroCuotaIvaNoDed_RecargoEquiv += cuotb3;
                                        this.totalCompCuotaIvaNoDed_RecargoEquiv += cuotb3;
                                    }
                                    break;
                            }

                            break;
                        case "R" :
                            requb3 = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("REQUB3")));
                            requb3 *= -1;
                            baimb3 *= -1;
                            cuotb3 *= -1;

                            totalLinea = baimb3 + cuotb3 + requb3;

                            datos[10] = baimb3.ToString();
                            datos[11] = tpivb3.ToString();
                            datos[12] = cuotb3.ToString();
                            datos[13] = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("RECGB3"))).ToString();
                            datos[14] = requb3.ToString();

                            if (this.chkTotalSerie.Checked)
                            {
                                this.totalSerieCuotaIva += cuotb3;
                                this.totalLibroCuotaIva += cuotb3;
                                this.totalCompCuotaIva += cuotb3;
                            }

                            break;
                    }

                    datos[15] = totalLinea.ToString();
                    totalAcumuladoDocumento += totalLinea;

                    datos[16] = dr.GetValue(dr.GetOrdinal("COIVB3")).ToString();
                    datos[17] = resob3;

                    if (this.chkTotalSerie.Checked)
                    {
                        this.totalSerieBaseImp += baimb3;
                        this.totalLibroBaseImp += baimb3;
                        this.totalCompBaseImp += baimb3;

                        this.totalSerieTotalFact += totalLinea;
                        this.totalLibroTotalFact += totalLinea;
                        this.totalCompTotalFact += totalLinea;
                    }

                    registros++;
                }

                if (registros > 0 && this.chkTotalSerie.Checked)
                {
                    //Escribir el acumulado del documento
                    //Escribe la línea del detalle del comprobante en el fichero HTML
                    datos[15] = totalAcumuladoDocumento.ToString();
                    this.InformeHTMLEscribirDetalleComprobante(datos);

                    this.EscribirTotalSerie(serib3);
                    //this.EscribirLineaEnBlanco("#F2F2F2");
                    this.EscribirTotalLibro(librb3);
                    //this.EscribirLineaEnBlanco("#F2F2F2");
                    this.EscribirTotalCompania(codigo);
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
        /// Construye la consulta para obtener los datos del informe
        /// </summary>
        /// <param name="codigo">Código de la compañía fiscal</param>
        /// <returns></returns>
        private string ObtenerQuery(string codigo)
        {
            string query = "";

            try
            {
                string fechaDesde = utiles.SigloDadoAnno(this.anoDesde, CGParametrosGrles.GLC01_ALSIRC) + this.anoDesde + this.mesDesde.PadLeft(2, '0') + "01";
                string fechaHasta = utiles.SigloDadoAnno(this.anoHasta, CGParametrosGrles.GLC01_ALSIRC) + this.anoHasta + this.mesHasta.PadLeft(2, '0') + "31";

                query = "select * from " + GlobalVar.PrefijoTablaCG + "IVB03 ";
                query += "where CIAFB3 = '" + codigo + "' and ";
                query += "FECOB3 >= " + fechaDesde + " and ";
                query += "FECOB3 <= " + fechaHasta;

                if (this.rbLibroSerie.IsChecked)
                {
                    //Por libro / Serie
                    query += " and LIBRB3 = '" + this.txtLibro.Text + "'";
                    if (lbSerie.Items.Count > 0)
                    {
                        query += " and (";
                        for (int i = 0; i < this.lbSerie.Items.Count; i++)
                        {
                            if (i != 0) query += " or ";
                            query += "SERIB3 = '" + this.lbSerie.Items[i].ToString() + "'";
                        }
                        query += ")";
                    }
                }
                else
                {
                    if (this.rbSoportado.IsChecked)
                    {
                        //Por soportado
                        query += " and RESOB3 = 'S'";
                    }
                    else if (this.rbRepercutido.IsChecked)
                    {
                        //Por repercutido
                        query += " and RESOB3 = 'R'";
                    }
                }

                query += " order by CIAFB3, LIBRB3, SERIB3, FECOB3, CLDOB3, NDOCB3, NITRB3";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }

        /// <summary>
        /// Devuelve las cabeceras de las columnas del informe en el idioma correspondiente
        /// </summary>
        /// <returns></returns>
        private string[] ObtenerCabecerasColumnasInforme(string resob3)
        {
            //Cabecera de las columnas del informe
            string[] columnas = new string[18];      //Falta traducir
            columnas[0] = this.LP.GetText("lblCabCodigoCompania", "Cia"); 
            columnas[1] = this.LP.GetText("lblLibro", "Libro");
            columnas[2] = this.LP.GetText("lblSerie", "Serie");
            columnas[3] = this.LP.GetText("lblCabNumMvto", "Núm. Mvto.");
            columnas[4] = this.LP.GetText("lblCabFechaCont", "Fecha Cont.");
            columnas[5] = this.LP.GetText("lblCabFechaDoc", "Fecha Docum.");
            columnas[6] = this.LP.GetText("lblCabNumDoc", "Núm. Docum.");
            columnas[7] = this.LP.GetText("lblCabNIdFiscal", "N. Ident. Fiscal");
            columnas[8] = this.LP.GetText("lblCabRazonSocial", "Nombre/Razón Social");
            columnas[9] = this.LP.GetText("lblCabDescripcion", "Descripción");
            columnas[10] = this.LP.GetText("lblCabBaseImponible", "Base Imponible");
            columnas[11] = this.LP.GetText("lblCabPorcIVA", "%IVA");
            if (resob3 == "S")
            {
                columnas[12] = this.LP.GetText("lblCuotaIVADeduc", "Cuota IVA Deducib.");
                columnas[13] = this.LP.GetText("lblCabPorcIVA", "%IVA");
                columnas[14] = this.LP.GetText("lblCuotaNoDeduc", "Cuota No Deduc.");
            }
            else
            {
                columnas[12] = this.LP.GetText("lblCuotaIVA", "Cuota IVA");
                columnas[13] = this.LP.GetText("lblPorcREquiv", "%R.Eqv.");
                columnas[14] = this.LP.GetText("lblRecargoEquiv", "Recargo Equiv.");
            }
            
            columnas[15] = this.LP.GetText("lblTotalFactura", "Total Factura");
            columnas[16] = this.LP.GetText("lblCodIVA", "Cód. IVA");
            columnas[17] = this.LP.GetText("lblSoportRepercut", "S/R");
            return (columnas);
        }

        /// <summary>
        /// Buscar el Nombre o la razoón social del proveedor
        /// </summary>
        /// <param name="pais">código del país</param>
        /// <param name="nif">nif</param>
        /// <param name="tauxb3">tipo de auxiliar</param>
        /// <returns></returns>
        private string ObtenerNombreORazonSocial(string pais, string nif, string tauxb3)
        {
            string result = "";
            IDataReader dr = null;

            string paisNifTaux = pais.PadRight(2, ' ') + nif.PadRight(13, ' ') + tauxb3.PadRight(2, ' ');

            try
            {
                string nombre = utiles.FindFirstValueByKey(ref dictRazonSocial, paisNifTaux);

                if (nombre != paisNifTaux)
                {
                    result = nombre;
                    return (result);
                }
                else
                {
                    string query = "select NOMBCF from " + GlobalVar.PrefijoTablaCG + "IVM05 ";
                    query += "where PCIFCF = '" + pais + "' and NITRCF = '" + nif + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        result = dr["NOMBCF"].ToString();
                    }

                    dr.Close();

                    if (result != "")
                    {
                        this.dictRazonSocial.Add(paisNifTaux, result);
                        return (result);
                    }

                    if (tauxb3 != "")
                    {
                        query = "select NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                        query += "where TAUXMA = '" + tauxb3 + "' and PCIFMA = '" + pais + "' and NNITMA = '" + nif + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result = dr["NOMBMA"].ToString();
                        }

                        dr.Close();

                        if (result != "")
                        {
                            this.dictRazonSocial.Add(paisNifTaux, result);
                            return (result);
                        }

                        query = "select NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                        query += "where TAUXMA = '" + tauxb3 + "' and NNITMA = '" + nif + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result = dr["NOMBMA"].ToString();
                        }

                        dr.Close();

                        if (result != "")
                        {
                            this.dictRazonSocial.Add(paisNifTaux, result);
                            return (result);
                        }

                        query = "select NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                        query += "where NNITMA = '" + nif + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result = dr["NOMBMA"].ToString();
                        }

                        dr.Close();
                    }
                    else
                    {
                        query = "select NOMBMA from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                        query += "where NNITMA = '" + nif + "'";

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result = dr["NOMBMA"].ToString();
                        }

                        dr.Close();
                    }
                }
            }
            catch(Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            this.dictRazonSocial.Add(paisNifTaux, result);
            
            return (result);
        }
        
        /// <summary>
        /// Escribe la línea de totales por Serie
        /// </summary>
        /// <param name="serie">nombre de la serie</param>
        private void EscribirTotalSerie(string serie)
        {
            string[] datos = new string[18];
            datos[0] = "";
            datos[1] = "";
            datos[2] = "";
            datos[3] = "";
            datos[4] = "";
            datos[5] = "";
            datos[6] = "";
            datos[7] = "";
            datos[8] = "";
            datos[9] = this.LP.GetText("lblTotalSerie", "TOTAL SERIE").ToUpper() + " " + serie;
            datos[10] = this.totalSerieBaseImp.ToString();
            datos[11] = "";
            datos[12] = this.totalSerieCuotaIva.ToString();
            datos[13] = "";
            datos[14] = this.totalSerieCuotaIvaNoDed_RecargoEquiv.ToString();
            datos[15] = this.totalSerieTotalFact.ToString();
            datos[16] = "";
            datos[17] = "";

            //Escribe la línea del detalle del comprobante en el fichero HTML
            this.InformeHTMLEscribirTotales(datos);

            //Inicializar los totales de la Serie
            this.totalSerieBaseImp = 0;
            this.totalSerieCuotaIva = 0;
            this.totalSerieCuotaIvaNoDed_RecargoEquiv = 0;
            this.totalSerieTotalFact = 0;
        }

        /// <summary>
        /// Escribe una línea de totales por Libro
        /// </summary>
        /// <param name="librb3">nombre del libro</param>
        private void EscribirTotalLibro(string librb3)
        {
            string[] datos = new string[18];
            datos[0] = "";
            datos[1] = "";
            datos[2] = "";
            datos[3] = "";
            datos[4] = "";
            datos[5] = "";
            datos[6] = "";
            datos[7] = "";
            datos[8] = "";
            datos[9] = this.LP.GetText("lblTotalLibro", "TOTAL LIBRO").ToUpper() + " " + librb3;
            datos[10] = this.totalLibroBaseImp.ToString();
            datos[11] = "";
            datos[12] = this.totalLibroCuotaIva.ToString();
            datos[13] = "";
            datos[14] = this.totalLibroCuotaIvaNoDed_RecargoEquiv.ToString();
            datos[15] = this.totalLibroTotalFact.ToString();
            datos[16] = "";
            datos[17] = "";

            //Escribe la línea del detalle del comprobante en el fichero HTML
            this.InformeHTMLEscribirTotales(datos);

            //Inicializar los totales del Libro
            this.totalLibroBaseImp = 0;
            this.totalLibroCuotaIva = 0;
            this.totalLibroCuotaIvaNoDed_RecargoEquiv = 0;
            this.totalLibroTotalFact = 0;
        }

        /// <summary>
        /// Escribe una línea de total por compañía
        /// </summary>
        /// <param name="codigo">código de la compañía</param>
        private void EscribirTotalCompania(string codigo)
        {
            string[] datos = new string[18];
            datos[0] = "";
            datos[1] = "";
            datos[2] = "";
            datos[3] = "";
            datos[4] = "";
            datos[5] = "";
            datos[6] = "";
            datos[7] = "";
            datos[8] = "";
            datos[9] = this.LP.GetText("lblTotalCompania", "TOTAL COMPAÑIA").ToUpper() + " " + codigo;
            datos[10] = this.totalCompBaseImp.ToString();
            datos[11] = "";
            datos[12] = this.totalCompCuotaIva.ToString();
            datos[13] = "";
            datos[14] = this.totalCompCuotaIvaNoDed_RecargoEquiv.ToString();
            datos[15] = this.totalCompTotalFact.ToString();
            datos[16] = "";
            datos[17] = "";

            //Escribe la línea del detalle del comprobante en el fichero HTML
            this.InformeHTMLEscribirTotales(datos);

            //Inicializar los totales de la Serie
            this.totalSerieBaseImp = 0;
            this.totalSerieCuotaIva = 0;
            this.totalSerieCuotaIvaNoDed_RecargoEquiv = 0;
            this.totalSerieTotalFact = 0;

            //Inicializar los totales del Libro
            this.totalLibroBaseImp = 0;
            this.totalLibroCuotaIva = 0;
            this.totalLibroCuotaIvaNoDed_RecargoEquiv = 0;
            this.totalLibroTotalFact = 0;

            //Inicializar los totales de la Compañía
            this.totalCompBaseImp = 0;
            this.totalCompCuotaIva = 0;
            this.totalCompCuotaIvaNoDed_RecargoEquiv = 0;
            this.totalCompTotalFact = 0;
        }

        /// <summary>
        /// Escribe una línea en blanco en el fichero
        /// </summary>
        private void EscribirLineaEnBlanco(string colorFondo)
        {
            string[] datos = new string[18];
            for (int i = 0; i <= 17; i++)
            {
                datos[i] = "";
            }
            this.InformeHTMLEscribirDetalleComprobante(datos);
        }

        /// <summary>
        /// Escribe en el fichero html la cabecera del mismo
        /// </summary>
        /// <param name="compania">nombre de la compañía</param>
        private void InformeHTMLEscribirEncabezado(string compania)
        {
            string fila1 = compania + "  " + this.txttitulo.Text;

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

        /// <summary>
        /// Escribe en el fichero excel las cabeceras de las tablas del informe
        /// </summary>
        /// <param name="compania">nombre de la compañía</param>
        /// <param name="cabecerasColumnas">nombre de las columnas del informe</param>
        private void InformeHTMLEscribirCabeceras(string compania, string[] cabecerasColumnas)
        {
            string fila1 = compania + "  " + this.txttitulo.Text;

            documento_HTML.Append("     <b>" + fila1 + "</b><br><br>\n");

            string fila2 = this.LP.GetText("lblAnoMes", "AÑO/MES").ToUpper() + " " + this.anoDesde.PadLeft(2, '0') + "-" +
                            this.mesDesde.PadLeft(2, '0') + " " + this.LP.GetText("lblHasta", "HASTA").ToUpper() + " " +
                            this.anoHasta.PadLeft(2, '0') + "-" + this.mesHasta.PadLeft(2, '0');
            if (this.rbLibroSerie.IsChecked) fila2 += " " + this.LP.GetText("lblLibro", "LIBRO").ToUpper() + " " + this.txtLibro.Text;
            else
            {
                if (this.rbSoportado.IsChecked) fila2 += "  " + this.LP.GetText("lblSoportado", "SOPORTADO").ToUpper();
                else fila2 += "  " + this.LP.GetText("lblRepercutido", "REPERCUTIDO").ToUpper();
            }

            documento_HTML.Append("     <b>" + fila2 + "</b>\n");

            documento_HTML.Append("     <table width =\"100%\">\n");
            documento_HTML.Append("         <tr>\n");
            string valor = (cabecerasColumnas[0] == "") ? "&nbsp;" : cabecerasColumnas[0];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[1] == "") ? "&nbsp;" : cabecerasColumnas[1];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[2] == "") ? "&nbsp;" : cabecerasColumnas[2];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[3] == "") ? "&nbsp;" : cabecerasColumnas[3];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[4] == "") ? "&nbsp;" : cabecerasColumnas[4];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[5] == "") ? "&nbsp;" : cabecerasColumnas[5];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[6] == "") ? "&nbsp;" : cabecerasColumnas[6];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[7] == "") ? "&nbsp;" : cabecerasColumnas[7];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[8] == "") ? "&nbsp;" : cabecerasColumnas[8];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[9] == "") ? "&nbsp;" : cabecerasColumnas[9];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[10] == "") ? "&nbsp;" : cabecerasColumnas[10];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[11] == "") ? "&nbsp;" : cabecerasColumnas[11];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[12] == "") ? "&nbsp;" : cabecerasColumnas[12];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[13] == "") ? "&nbsp;" : cabecerasColumnas[13];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[14] == "") ? "&nbsp;" : cabecerasColumnas[14];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[15] == "") ? "&nbsp;" : cabecerasColumnas[15];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[16] == "") ? "&nbsp;" : cabecerasColumnas[16];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            valor = (cabecerasColumnas[17] == "") ? "&nbsp;" : cabecerasColumnas[17];
            documento_HTML.Append("             <td class=TextoTITCab>" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            //documento_HTML.Append("     </table>\n");
        }

        /// <summary>
        /// Escribe en el fichero HTML una línea con los datos obtenidos de la consulta
        /// </summary>
        /// <param name="datos">datos a escribir</param>
        private void InformeHTMLEscribirDetalleComprobante(string[] datos)
        {
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (datos[0] == "") ? "&nbsp;" : datos[0];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[1] == "") ? "&nbsp;" : datos[1];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[2] == "") ? "&nbsp;" : datos[2];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[3] == "") ? "&nbsp;" : datos[3];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[4] == "") ? "&nbsp;" : datos[4];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[5] == "") ? "&nbsp;" : datos[5];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[6] == "") ? "&nbsp;" : datos[6];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[7] == "") ? "&nbsp;" : datos[7];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[8] == "") ? "&nbsp;" : datos[8];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[9] == "") ? "&nbsp;" : datos[9];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[10] == "") ? "&nbsp;" : datos[10];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[11] == "") ? "&nbsp;" : datos[11];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[12] == "") ? "&nbsp;" : datos[12];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[13] == "") ? "&nbsp;" : datos[13];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[14] == "") ? "&nbsp;" : datos[14];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[15] == "") ? "&nbsp;" : datos[15];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (datos[16] == "") ? "&nbsp;" : datos[16];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (datos[17] == "") ? "&nbsp;" : datos[17];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            documento_HTML.Append("         </tr>\n");
            //documento_HTML.Append("     </table>");
        }

        /// <summary>
        /// Escribe en el fichero HTML una línea de totales
        /// </summary>
        /// <param name="datos"></param>
        private void InformeHTMLEscribirTotales(string[] datos)
        {
            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            string valor = (datos[9] == "") ? "&nbsp;" : datos[9];
            documento_HTML.Append("             <td class=Texto><b>" + valor + "</b></td>\n");
            valor = (datos[10] == "") ? "&nbsp;" : datos[10];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\"><b>" + valor + "</b></td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp</td>\n");
            valor = (datos[12] == "") ? "&nbsp;" : datos[12];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\"><b>" + valor + "</b></td>\n");
            documento_HTML.Append("             <td class=NumeroCG>&nbsp;</td>\n");
            valor = (datos[14] == "") ? "&nbsp;" : datos[14];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\"><b>" + valor + "</b></td>\n");
            valor = (datos[15] == "") ? "&nbsp;" : datos[15];
            documento_HTML.Append("             <td class=NumeroCG align=\"right\"><b>" + valor + "</b></td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
            documento_HTML.Append("             <td class=Texto>&nbsp;</td>\n");
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
                this.existenRegistros = false;

                this.totalSerieBaseImp = 0;
                this.totalSerieCuotaIva = 0;
                this.totalSerieCuotaIvaNoDed_RecargoEquiv = 0;
                this.totalSerieTotalFact = 0;

                this.totalLibroBaseImp = 0;
                this.totalLibroCuotaIva = 0;
                this.totalLibroCuotaIvaNoDed_RecargoEquiv = 0;
                this.totalLibroTotalFact = 0;

                this.totalCompBaseImp = 0;
                this.totalCompCuotaIva = 0;
                this.totalCompCuotaIvaNoDed_RecargoEquiv = 0;
                this.totalCompTotalFact = 0;

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

                System.Data.DataTable dtPeticiones = frmPeticion.ListarPeticion();

                if (dtPeticiones.Rows.Count > 0)
                {
                    Dictionary<string, string> dictControles = new Dictionary<string, string>
                    {
                        //Falta traducir
                        { "Compañía Fiscal", "lbCompFiscal" },
                        //dictControles.Add("Por Libro/Serie", "rbLibroSerie");
                        { "Libro", "txtLibro" },
                        { "Series", "lbSerie" },
                        //dictControles.Add("Por Tipo Trans.", "rbTipoTrans");
                        { "Soportado", "rbSoportado" },
                        { "Repercutido", "rbRepercutido" },
                        { "AñoMes Desde", "txtMaskDesdeAnoMes" },
                        { "Año Hasta", "txtMaskHastaAnoMes" },
                        //dictControles.Add("Total por Serie", "chkTotalSerie");
                        { "Titulo", "txttitulo" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radButtonTextBoxSelCompaniaFiscal", "txtSerie", 
                                                                                   "rbLibroSerie", "rbTipoTrans", 
                                                                                   "chkTotalSerie", "txtDesdePeriodos", "txtHastaPeriodos" });

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
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCIMOVIVA myStruct = (StructGLL01_MCIMOVIVA)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCIMOVIVA));

                this.anoDesde = "";
                this.mesHasta = "";
                this.anoHasta = "";
                this.mesDesde = "";

                if (myStruct.desdeAno.Trim() != "") this.anoDesde = myStruct.desdeAno;
                if (myStruct.desdeMes.Trim() != "") this.mesDesde = myStruct.desdeMes;
                if (myStruct.hastaAno.Trim() != "") this.anoHasta = myStruct.hastaAno;
                if (myStruct.hastaMes.Trim() != "") this.mesHasta = myStruct.hastaMes;

                this.txtMaskDesdeAnoMes.Value = this.anoDesde + this.mesDesde;
                this.txtMaskHastaAnoMes.Value = this.anoHasta + this.mesHasta;

                if (myStruct.totalSerie == "1") this.chkTotalSerie.Checked = true;
                else this.chkTotalSerie.Checked = false;

                this.txttitulo.Text = myStruct.titulo.Trim();

                if (myStruct.tipoTransa == "1")
                {
                    this.rbTipoTrans.IsChecked = true;

                    if (myStruct.soportado == "1") this.rbSoportado.IsChecked = true;
                    else this.rbSoportado.IsChecked = false;

                    if (myStruct.repercutido == "1") this.rbRepercutido.IsChecked = true;
                    else this.rbRepercutido.IsChecked = false;

                    this.rbLibroSerie.IsChecked = false;
                    this.txtLibro.Text = "";
                    this.txtSerie.Text = "";
                    this.lbSerie.Items.Clear();
                }
                else
                {
                    this.rbLibroSerie.IsChecked = true;

                    if (myStruct.libro.Trim() != "") this.txtLibro.Text = myStruct.libro;
                    this.txtSerie.Text = "";
                    
                    if (myStruct.series.Trim() != "")
                    {
                        string series = myStruct.series.Trim();
                        string[] aSeries = series.Split(';');
                        this.lbSerie.Items.Clear();
                        string serie = "";
                        for (int i = 0; i < aSeries.Length; i++)
                        {
                            serie = aSeries[i].Trim();
                            if (serie != "") this.lbSerie.Items.Add(aSeries[i]);
                        }
                    }
                    
                    this.rbTipoTrans.IsChecked = false;
                    this.rbSoportado.IsChecked = false;
                    this.rbRepercutido.IsChecked = false;
                }
                 

                if (myStruct.companias.Trim() != "")
                {
                    string companias = myStruct.companias.Trim();
                    string[] aCompanias = companias.Split(';');
                    this.lbCompFiscal.Items.Clear();
                    string compania = "";

                    string desc = "";
                    string[] datosCompaniaFiscal;
                    for (int i = 0; i < aCompanias.Length; i++)
                    {
                        compania = aCompanias[i].Trim();
                        if (compania != "")
                        {
                            this.lbCompFiscal.Items.Add(aCompanias[i]);

                            //Adicionar en el array de compañias fiscales
                            datosCompaniaFiscal = new string[2];
                            datosCompaniaFiscal[0] = compania;
                            datosCompaniaFiscal[1] = desc;
                            this.aCompFiscales.Add(datosCompaniaFiscal);
                        }
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
                StructGLL01_MCIMOVIVA myStruct;

                myStruct.desdeAno = this.anoDesde.PadRight(2, ' ');
                myStruct.desdeMes = this.mesDesde.PadRight(2, ' ');
                myStruct.hastaAno = this.anoHasta.PadRight(2, ' ');
                myStruct.hastaMes = this.mesHasta.PadRight(2, ' ');

                if (this.chkTotalSerie.Checked) myStruct.totalSerie = "1";
                else myStruct.totalSerie = "0";

                myStruct.titulo = this.txttitulo.Text.PadRight(100, ' ');
                myStruct.titulo = myStruct.titulo.Replace("'", "''");

                if (this.rbTipoTrans.IsChecked)
                {
                    myStruct.tipoTransa = "1";

                    if (this.rbSoportado.IsChecked) myStruct.soportado = "1";
                    else myStruct.soportado = "0";

                    if (this.rbRepercutido.IsChecked) myStruct.repercutido = "1";
                    else myStruct.repercutido = "0";

                    myStruct.libro = " ";
                    myStruct.series = "";
                    myStruct.series = myStruct.series.PadRight(100, ' ');
                }
                else
                {
                    myStruct.tipoTransa = "0";
                    myStruct.soportado = "0";
                    myStruct.repercutido = "0";

                    if (this.txtLibro.Text.Trim() != "") myStruct.libro = this.txtLibro.Text;
                    else myStruct.libro = " ";

                    myStruct.series = "";
                    if (this.lbSerie.Items.Count > 0)
                    {
                        for (int i = 0; i < this.lbSerie.Items.Count; i++)
                        {
                            myStruct.series += this.lbSerie.Items[i] + ";";
                        }
                    }

                    if (myStruct.series.Length > 100) myStruct.series = myStruct.series.Substring(0, 99);
                    else myStruct.series = myStruct.series.PadRight(100, ' ');
                }

                //compañías fiscales
                myStruct.companias = "";
                for (int i = 0; i < this.lbCompFiscal.Items.Count; i++)
                {
                    myStruct.companias += this.lbCompFiscal.Items[i] + ";";
                }

                if (myStruct.companias.Length > 100) myStruct.companias = myStruct.companias.Substring(0, 99);

                result = myStruct.desdeAno + myStruct.desdeMes + myStruct.hastaAno + myStruct.hastaMes + myStruct.totalSerie + myStruct.titulo;
                result += myStruct.tipoTransa + myStruct.soportado + myStruct.repercutido + myStruct.libro + myStruct.series + myStruct.companias;

                //int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIMOVAUX));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //if (result.Length < 145) result.PadRight(145, ' ');

            return (result);
        }
        #endregion
    }
}