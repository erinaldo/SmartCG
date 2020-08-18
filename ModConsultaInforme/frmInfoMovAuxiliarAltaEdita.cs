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

namespace ModConsultaInforme
{
    public partial class frmInfoMovAuxiliarAltaEdita : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCIMOVAUX";
        public string formCodeNombFichero = "MOVAUXI";
        public string ficheroExtension = "mau";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCIMOVAUX
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string grupo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string plan;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string tipoAuxiliar;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string documentos;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string moneda;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string periodoHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string saldoInicial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string totales;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string cuentaEditada;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string titulo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
            public string cuentasAuxiliar;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
            public string cuentasMayor;
        }

        private string tipoFichero = "EXCEL";

        string[] columnas;

        ArrayList aEmpresas = null;
        string calendario = "";

        string nombreAuxiliar = "";
        string zonaAuxiliar = "";

        int saprDesde = -1;
        int saprHasta = -1;

        private decimal totalCtaMayorDebe = 0;
        private decimal totalCtaMayorHaber = 0;
        private decimal totalCtaMayorSaldo = 0;
        private decimal totalCompaniaDebe = 0;
        private decimal totalCompaniaHaber = 0;
        private decimal totalCompaniaSaldo = 0;
        private decimal totalCtaAuxDebe = 0;
        private decimal totalCtaAuxHaber = 0;
        private decimal totalCtaAuxSaldo = 0;
        private decimal totalFinalDebe = 0;
        private decimal totalFinalHaber = 0;
        private decimal totalFinalSaldo = 0;

        private string codigoCompania = "";
        private string descCompania = "";
        private string codigoGrupo = "";
        private string descGrupo = "";
        private string codigoPlan = "";
        private string descPlan = "";
        private string codigoTipoAux = "";
        private string descTipoAux = "";

        private StringBuilder documento_HTML;

        private FormularioValoresCampos valoresFormulario;

        private ArrayList avisosAutorizaciones;

        private Dictionary<string, string> dictAuxZonaNombre = new Dictionary<string, string>();

        private Dictionary<string, string> dictAutCtaAux = new Dictionary<string, string>();

        private Dictionary<string, string> dictAutCtaMayor = new Dictionary<string, string>();

        private bool cargarPlanes = false;

        private System.Data.DataTable dtDocumento;

        private string nombreFicheroAGenerar = "";

        private string mensajeProceso = "";

        public frmInfoMovAuxiliarAltaEdita()
        {
            InitializeComponent();

            this.gBoxListaCuentasAux.ElementTree.EnableApplicationThemeName = false;
            this.gBoxListaCuentasAux.ThemeName = "ControlDefault";

            this.gbListaCuentasMayor.ElementTree.EnableApplicationThemeName = false;
            this.gbListaCuentasMayor.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoMovAuxiliarAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Movimientos de Auxiliar");

            //Obtener el tipo de fichero
            string tipoFicherosInformes = System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_TipoFicherosInformes"];
            if (tipoFicherosInformes != null && tipoFicherosInformes != "") if (tipoFicherosInformes == "HTML") tipoFichero = tipoFicherosInformes;

            utiles.ButtonEnabled(ref this.btnAddCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);

            utiles.ButtonEnabled(ref this.btnAddCuentaAux, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, false);

            this.TraducirLiterales();

            //Cargar compañías
            this.FillCompanias();

            //Cargar grupo de compañías
            this.FillGruposCompanias();

            //Cargar planes
            this.FillPlanes("");

            //Crear Tabla Documento
            this.dtDocumento = new System.Data.DataTable();
            this.dtDocumento.Columns.Add("valor", typeof(string));
            this.dtDocumento.Columns.Add("desc", typeof(string));

            //Crear el desplegable de Documento
            this.CrearComboDocumento();

            //Cargar los tipos de moneda
            this.FillMonedas();

            //Iniciar el Campo titulo con el nombre del formulario
            this.txttitulo.Text = this.Text;

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

        private void BtnAddCuentaAux_Click(object sender, EventArgs e)
        {
            string codCuentaAuxActual = this.radButtonTextBoxSelCuentaAux.Text.Trim();
            if (codCuentaAuxActual != "")
            {
                string validarCuentaAux = this.ValidarCuentaAuxiliar(codCuentaAuxActual, this.codigoTipoAux);

                string error = this.LP.GetText("errValTitulo", "Error");
                if (validarCuentaAux != "")
                {
                    RadMessageBox.Show(validarCuentaAux, error);
                    this.radButtonTextBoxSelCuentaAux.Select();
                    return;
                }

                string result = this.AddToListBox(this.radButtonTextBoxSelCuentaAux.Text, ref this.lbCuentasAux);
                switch (result)
                {
                    case "":
                        this.radButtonTextBoxSelCuentaAux.Text = "";
                        this.radButtonTextBoxSelCuentaAux.Focus();
                        break;
                    case "1":
                        RadMessageBox.Show(this.LP.GetText("errCuentaAuxiliarExiste", "La cuenta de auxiliar ya está en la lista"), error);
                        this.radButtonTextBoxSelCuentaAux.Focus();
                        break;
                }
            }
        }

        private void BtnQuitarCuentaAux_Click(object sender, EventArgs e)
        {
            try
            {
                RadListDataItem item = this.lbCuentasAux.SelectedItem;
                this.lbCuentasAux.Items.Remove(item);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCuentasAux.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, false);
                this.radButtonTextBoxSelCuentaAux.Select();
            }
            else this.lbCuentasAux.Select();
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
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
                this.radButtonTextBoxSelCuentasMayor.Select();
            }
            else this.lbCuentasMayor.Select();
        }

        private void TgTexBoxSelTipoAux_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            /*
            if (this.tgTexBoxSelTipoAux.Textbox.Modified && this.tgTexBoxSelTipoAux.Textbox.Text.Trim() != "")
            {
                this.tgTexBoxSelTipoAux.Textbox.Modified = false;

                string codigo = this.tgTexBoxSelTipoAux.Textbox.Text.Trim();

                if (codigo != "" && codigo.Length >= 2)
                {
                    if (codigo.Length <= 1) this.codigoTipoAux = this.tgTexBoxSelTipoAux.Textbox.Text;
                    else this.codigoTipoAux = this.tgTexBoxSelTipoAux.Textbox.Text.Substring(0, 2);

                    string descTipoAux = "";
                    string result = this.ValidarTipoAuxiliar(this.codigoTipoAux, ref descTipoAux, false);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.lbCuentasAux.Items.Clear();
                        this.gBoxListaCuentasAux.Enabled = false;
                        this.tgTexBoxSelTipoAux.Textbox.Focus();
                    }
                    else
                    {
                        string codTipoAux = this.codigoTipoAux;
                        if (descTipoAux != "") codTipoAux += " " + this.separadorDesc + " " + descTipoAux;

                        this.tgTexBoxSelTipoAux.Textbox.Text = codTipoAux;

                        this.gBoxListaCuentasAux.Enabled = true;

                        this.radButtonTextBoxSelCuentasMayor.Focus();
                    }
                }
                else
                {
                    this.gBoxListaCuentasAux.Enabled = false;
                    this.tgTexBoxSelTipoAux.Textbox.Focus();
                }
            }*/
            Cursor.Current = Cursors.Default;
        }

        private void LbCuentasMayor_Enter(object sender, EventArgs e)
        {
            if (this.lbCuentasMayor.Items.Count > 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, true);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, true);
            }
        }

        private void LbCuentasAux_Enter(object sender, EventArgs e)
        {
            if (this.lbCuentasAux.Items.Count > 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, true);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, true);
            }
        }

        private void LbCuentasAux_Leave(object sender, EventArgs e)
        {
            //this.lbCuentasAux.SelectedIndex = -1;
        }

        private void LbCuentasMayor_Leave(object sender, EventArgs e)
        {
            //this.lbCuentasMayor.SelectedIndex = -1;
        }

        private void TgTexBoxSelCuentasAux_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, false);
            this.lbCuentasAux.SelectedIndex = -1;
        }

        private void TgTexBoxSelCuentasMayor_Enter(object sender, EventArgs e)
        {
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayor, false);
            utiles.ButtonEnabled(ref this.btnQuitarCuentaMayorTodas, false);
            this.lbCuentasMayor.SelectedIndex = -1;
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

                    this.radButtonTextBoxSelTipoAux.Focus();
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

                        this.radButtonTextBoxSelTipoAux.Focus();
                    }
                }
                else
                {
                    this.radDropDownListPlan.Focus();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonTextBoxSelCuentaAux_TextChanged(object sender, EventArgs e)
        {
            if (this.radButtonTextBoxSelCuentaAux.Text.Trim() == "") utiles.ButtonEnabled(ref this.btnAddCuentaAux, false);
            else utiles.ButtonEnabled(ref this.btnAddCuentaAux, true);
        }

        private void RadButtonElementSelCuentaAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CAUXMA, NOMBMA, PCIFMA, NNITMA, STATMA from ";
            query += GlobalVar.PrefijoTablaCG + "GLM05 ";

            if (this.codigoTipoAux != "")
            {
                query += "where TAUXMA = '" + codigoTipoAux + "' ";
            }
            query += "order by TAUXMA, CAUXMA";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                "Código",
                "Descripción",
                "País",
                "NIT",
                "Estado"
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar cuentas de auxiliar",
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
                utiles.ButtonEnabled(ref this.btnAddCuentaAux, true);
            }
            this.radButtonTextBoxSelCuentaAux.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelCuentaAux;
            this.radButtonTextBoxSelCuentaAux.Select(0, 0);
            this.radButtonTextBoxSelCuentaAux.Focus();
        }

        private void BtnQuitarCuentaAuxTodas_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lbCuentasAux.Items.Count;)
                {
                    RadListDataItem item = this.lbCuentasAux.Items[i];
                    this.lbCuentasAux.Items.Remove(item);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (this.lbCuentasAux.Items.Count == 0)
            {
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, false);
                utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, false);
                this.radButtonTextBoxSelCuentaAux.Select();
            }
            else this.lbCuentasAux.Select();
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

        private void RadButtonTextBoxSelTipoAux_TextChanged(object sender, EventArgs e)
        {
            string codigo = this.radButtonTextBoxSelTipoAux.Text.Trim();
            if (codigo != "")
            {
                if (codigo.Length >= 2)
                {
                    if (codigo.Length <= 1) this.codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text;
                    else this.codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text.Substring(0, 2);

                    string descTipoAux = "";
                    string result = this.ValidarTipoAuxiliar(this.codigoTipoAux, ref descTipoAux, false);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.lbCuentasAux.Items.Clear();
                        this.gBoxListaCuentasAux.Enabled = false;
                        this.radButtonTextBoxSelTipoAux.Focus();
                    }
                    else
                    {
                        string codTipoAux = this.codigoTipoAux;
                        if (descTipoAux != "") codTipoAux += " " + this.separadorDesc + " " + descTipoAux;

                        this.radButtonTextBoxSelTipoAux.Text = codTipoAux;

                        this.gBoxListaCuentasAux.Enabled = true;
                    }
                }
                else
                {
                    this.gBoxListaCuentasAux.Enabled = false;
                    this.radButtonTextBoxSelTipoAux.Focus();
                }
            }
            else this.gBoxListaCuentasAux.Enabled = false;
        }

        private void RadButtonElementSelTipoAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

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
                TituloForm = "Seleccionar tipo de auxiliar",
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
                this.radButtonTextBoxSelTipoAux.Text = result;
                this.ActiveControl = this.radButtonTextBoxSelTipoAux;
                this.radButtonTextBoxSelTipoAux.Select(0, 0);
                this.radButtonTextBoxSelTipoAux.Focus();
            }
        }

        private void BtnAddCuentaAux_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAddCuentaAux);
        }

        private void BtnAddCuentaAux_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAddCuentaAux);
        }

        private void BtnQuitarCuentaAux_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarCuentaAux);
        }

        private void BtnQuitarCuentaAux_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarCuentaAux);
        }

        private void BtnQuitarCuentaAuxTodas_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnQuitarCuentaAuxTodas);
        }

        private void BtnQuitarCuentaAuxTodas_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnQuitarCuentaAuxTodas);
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
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", "Movimientos de Auxiliar", MessageBoxButtons.YesNo, RadMessageIcon.Question);

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

        private void FrmInfoMovAuxiliarAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Movimientos de Auxiliar");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemMovimientosAux", "Movimientos de Auxiliar");
            this.Text = this.Text.Replace("&", "");
            this.radLabelCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.radLabelGrupo.Text = this.LP.GetText("lblGrupo", "Grupo");
            this.radLabelPlan.Text = this.LP.GetText("lblPlan", "Plan");
            this.lblTipoAux.Text = this.LP.GetText("lblTipoAux", "Tipo de Auxiliar");
            this.gBoxListaCuentasAux.Text = this.LP.GetText("lblCuentasAuxliar", "Lista de Cuentas de Auxiliar");
            this.btnAddCuentaAux.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnQuitarCuentaAux.Text = this.LP.GetText("lblQuitar", "Quitar");
            this.gbListaCuentasMayor.Text = this.LP.GetText("lblCuentasMayor", "Lista de Cuentas de Mayor");
            this.btnAddCuentaMayor.Text = this.LP.GetText("lblAnadir", "Añadir");
            this.btnQuitarCuentaMayor.Text = this.LP.GetText("lblQuitar", "Quitar");

            this.lblMoneda.Text = this.LP.GetText("lblMoneda", "Moneda");
            //this.lblDesdePeriodo.Text = this.LP.GetText("lblPeriodoDesde", "Período Desde");
            //this.lblDesdePeriodo.Text = this.LP.GetText("lblPeriodoHasta", "Período Hasta");
            this.chkSaldoInicial.Text = this.LP.GetText("lblCabSaldoIni", "Saldo inicial");
            this.chkTotales.Text = this.LP.GetText("lblTotales", "Totales");

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Cargar los tipos de moneda
        /// </summary>
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
        /// Crea el desplegable de Documento
        /// </summary>
        private void CrearComboDocumento()
        {
            DataRow row;

            try
            {
                if (this.dtDocumento.Rows.Count > 0) this.dtDocumento.Rows.Clear();

                row = this.dtDocumento.NewRow();
                row["valor"] = "1";
                row["desc"] = this.LP.GetText("lblCancelados", "Cancelados");
                this.dtDocumento.Rows.Add(row);

                row = this.dtDocumento.NewRow();
                row["valor"] = "2";
                row["desc"] = this.LP.GetText("lblNoCancelados", "No cancelados");
                this.dtDocumento.Rows.Add(row);

                row = this.dtDocumento.NewRow();
                row["valor"] = "0";
                row["desc"] = this.LP.GetText("lblTodos", "Todos");
                this.dtDocumento.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListDocumentos.DataSource = this.dtDocumento;
            this.radDropDownListDocumentos.ValueMember = "valor";
            this.radDropDownListDocumentos.DisplayMember = "desc";
            this.radDropDownListDocumentos.Refresh();
        }

        private bool FormValid()
        {
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

            if (codigoTipoAux == "")
            {
                RadMessageBox.Show(this.LP.GetText("errTipoauxObl", "Es obligatorio informar el tipo de auxiliar"), error);
                this.radButtonTextBoxSelTipoAux.Select();
                return (false);
            }
            else
            {
                string validarTipoAux = this.ValidarTipoAuxiliar(codigoTipoAux, ref descTipoAux, false);
                if (validarTipoAux != "")
                {
                    RadMessageBox.Show(validarTipoAux, error);
                    this.radButtonTextBoxSelTipoAux.Select();
                    return (false);
                }
            }

            this.txtMaskDesdePeriodos.Text = this.txtMaskDesdePeriodos.Text.PadRight(5, '0');
            this.txtDesdeFecha.Text = this.txtMaskDesdePeriodos.Text.Substring(0, 2) + this.txtMaskDesdePeriodos.Text.Substring(3, 2);
            string resultMsg = this.ValidarPeriodo(ref this.txtDesdeFecha);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskDesdePeriodos.Focus();
                return (false);
            }

            this.txtMaskHastaPeriodos.Text = this.txtMaskHastaPeriodos.Text.PadRight(5, '0');
            this.txtHastaFecha.Text = this.txtMaskHastaPeriodos.Text.Substring(0, 2) + this.txtMaskHastaPeriodos.Text.Substring(3, 2);
            resultMsg = this.ValidarPeriodo(ref this.txtHastaFecha);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskHastaPeriodos.Focus();
                return (false);
            }

            //Validar las cuentas de mayor si están informadas
            if (this.lbCuentasAux.Items.Count > 0)
            {
                string cuentaAux = "";
                int indiceCtaAux = 0;
                string resultMsgCtasMayor = this.ValidarCuentasAuxiliar(ref this.lbCuentasAux, ref cuentaAux, ref indiceCtaAux, this.codigoTipoAux);
                if (resultMsgCtasMayor != "")
                {
                    if (indiceCtaAux != -1)
                    {
                        this.lbCuentasAux.SelectedIndex = indiceCtaAux;
                        utiles.ButtonEnabled(ref this.btnQuitarCuentaAux, true);
                        utiles.ButtonEnabled(ref this.btnQuitarCuentaAuxTodas, true);
                    }
                    RadMessageBox.Show(resultMsgCtasMayor, error);
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

            return (true);
        }

        private void ProcesarInforme()
        {
            this.mensajeProceso = "";

            //Iniciar la barra de progreso
            this.backgroundWorker1.ReportProgress(0);

            //Mover la barra de progreso
            this.backgroundWorker1.ReportProgress(2);
            string[] codDes = new string[2];

            string desc;
            if (this.codigoGrupo != "")
            {
                desc = descGrupo;
                if (codigoGrupo != "")
                {
                    //Buscar las empresas del grupo
                    aEmpresas = utilesCG.ObtenerCodEmpresasDelGrupo(codigoGrupo, codigoPlan);
                    calendario = "";
                    descCompania = descGrupo;
                }
            }
            else
            {
                desc = descCompania;
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

            //Mover la barra de progreso
            this.backgroundWorker1.ReportProgress(2);

            //Crear el fichero en memoria HTML
            this.InformeHTMLCrear(ref this.documento_HTML);

            //Cabecera de las columnas del informe
            columnas = this.ObtenerCabecerasColumnasInforme();

            string fila1 = "";
            string fila2 = "";
            //Obtener la 1ra y 2da fila del encabezado del informe
            this.InformeTitulo(ref fila1, ref fila2, desc);

            //Eliminar los posibles permisos de no autorizado del usuario conectado
            GlobalVar.UsuarioEnv.ListaNoAutorizado.Clear();

            //Inicializar los avisos de autorizaciones
            this.avisosAutorizaciones = new ArrayList();

            this.ProcesarMovAuxiliar(fila1, fila2);
        }

        /// <summary>
        /// Devuelve la consulta para obtener los datos
        /// </summary>
        /// <returns></returns>
        private string ObtenerConsultaDatos()
        {
            string query = "";

            query += "Select * From " + GlobalVar.PrefijoTablaCG + "GLB01 A ";
            query += "Where STATDT='E' and TAUXDT = '" + codigoTipoAux + "' ";

            //Filtrar por Cuentas de Auxiliar
            if (this.lbCuentasAux.Items.Count > 0)
            {
                query += " and (";
                for (int i = 0; i < this.lbCuentasAux.Items.Count; i++)
                {
                    if (i != 0) query += " or ";
                    query += "CAUXDT like '" + this.lbCuentasAux.Items[i].ToString() + "%'";
                }
                query += ")";
            }

            string[] datosCompania;

            if (this.codigoCompania != "")
            {
                //Por compañía
                datosCompania = (string[])aEmpresas[0];
                query += " and CCIADT='" + datosCompania[0] + "' ";
            }
            else
            {
                //Por grupo de compañías
                query += " and (";
                for (int i = 0; i < aEmpresas.Count; i++)
                {
                    datosCompania = (string[])aEmpresas[i];
                    if (i != 0) query += " or ";
                    query += "CCIADT = '" + datosCompania[0] + "'";
                }
                query += ")";
            }

            //Filtrar por Cuentas de Mayor
            if (this.lbCuentasMayor.Items.Count > 0)
            {
                query += " and (";
                for (int i = 0; i < this.lbCuentasMayor.Items.Count; i++)
                {
                    if (i != 0) query += " or ";
                    query += "CUENDT like '" + this.lbCuentasMayor.Items[i].ToString() + "%'";
                }
                query += ")";
            }

            //Filtrar por Plan
            if (codigoPlan != "") query += " and TIPLDT = '" + codigoPlan + "' ";

            string aa = this.txtMaskDesdePeriodos.Text.Substring(0, 2);
            string siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
            saprDesde = Convert.ToInt32(siglo + this.txtMaskDesdePeriodos.Text.Substring(0, 2) + this.txtMaskDesdePeriodos.Text.Substring(3, 2));

            //Si no han seleccionado por Saldo Inicial, fijar el período de inicio de la búsqueda
            if (!this.chkSaldoInicial.Checked)
            {
                //query += " and SAPRDT >= " + siglo + this.txtDesdeFecha.Text;
                query += " and SAPRDT >= " + saprDesde;
            }

            aa = this.txtHastaFecha.Text.Substring(0, 2);
            siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
            saprHasta = Convert.ToInt32(siglo + this.txtMaskHastaPeriodos.Text.Substring(0, 2) + this.txtMaskHastaPeriodos.Text.Substring(3, 2));

            //Filtrar por el período hasta de la búsqueda
            //query += " and SAPRDT <= " + siglo + this.txtHastaFecha.Text;
            query += " and SAPRDT <= " + saprHasta;

            //saprHasta = Convert.ToInt32(siglo + this.txtHastaFecha.Text);

            query += " and exists ( ";
            //query += "select B.TAUXDT, B.CAUXDT, B.CCIADT, B.TIPLDT, B.CUENDT, B.CLDODT, B.NDOCDT From " + GlobalVar.PrefijoTablaCG + "GLB01A B ";
            query += "select B.TAUXDT, B.CAUXDT, B.CCIADT, B.TIPLDT, B.CUENDT, B.CLDODT, B.NDOCDT From " + GlobalVar.PrefijoTablaCG + "GLB01 B ";
            query += "where B.CCIADT=A.CCIADT and B.TIPLDT=A.TIPLDT and B.CUENDT=A.CUENDT and B.CLDODT=A.CLDODT and B.NDOCDT=A.NDOCDT AND ";
            query += "SAPRDT<=" + siglo + this.txtHastaFecha.Text + " and B.STATDT = 'E' ";
            query += "Group by B.TAUXDT, B.CAUXDT, B.CCIADT, B.TIPLDT, B.CUENDT, B.CLDODT, B.NDOCDT ";

            //Filtrar por documentos que están cancelados
            if (this.radDropDownListDocumentos.SelectedValue != null && this.radDropDownListDocumentos.SelectedValue.ToString() == "1") query += "Having SUM(MONTDT) = 0 ";

            //Filtrar por documentos no cancelados
            if (this.radDropDownListDocumentos.SelectedValue != null && this.radDropDownListDocumentos.SelectedValue.ToString() == "2") query += "Having SUM(MONTDT) <> 0 ";

            query += ") ";

            query += "Order by A.TAUXDT, A.CAUXDT, A.CCIADT, A.TIPLDT, A.CUENDT, A.CLDODT, A.NDOCDT, A.SAPRDT, A.FECODT, A.TICODT, A.NUCODT, A.SIMIDT";

            return (query);
        }

        /// <summary>
        /// Procesar el informe de los movimientos de Auxiliar
        /// </summary>
        /// <param name="fila1">1ra fila del informe</param>
        /// <param name="fila2">2da fila del informe</param>
        private void ProcesarMovAuxiliar(string fila1, string fila2)
        {
            IDataReader dr = null;

            try
            {
                bool hayReg = false;
                bool escribirComprobante = false;
                string query = this.ObtenerConsultaDatos();

                string[] datos = new string[18];

                string tauxdt = "";
                string cauxdt = "";
                string cciadt = "";
                string cuendt = "";
                string cldodt = "";
                string ndocdt = "";
                string saprdt = "";
                string cuendtEditada = "";

                string tauxdtActual = "";
                string cauxdtActual = "";
                string cciadtActual = "";
                string cuendtActual = "";
                string cldodtActual = "";
                string ndocdtActual = "";
                string saprdtActual = "";
                string cuendtActualEditada = "";
                string cuentaEscribirInforme = "";

                decimal saldoAcumulado = 0;

                string importe = "";
                decimal importeNum = 0;

                string comprobante = "";

                int registro = 0;
                bool escribirRegistro = false;
                int regXCta = 0;

                string ticodt = "";
                bool autTipoComp = false;
                string aviso = "";

                string ctaAux = "";
                string ctaMayor = "";
                string codPlan = "";

                bool chequearAutCtaMayor = (lbCuentasMayor.Items.Count <= 0);
                bool chequearAutCtaAux = (lbCuentasAux.Items.Count <= 0);

                bool autCuentaAux = !chequearAutCtaAux;
                bool autCuentaMayor = !chequearAutCtaMayor;
                string resultValCtaAux = "";
                string resultValCtaMayor = "";

                string auxTipoCuentaAux = "";
                string auxTipoCuentaAuxValor = "";
                string auxPlanCuentaMayor = "";
                string auxPlanCuentaMayorTipo = "";

                ArrayList cuentaDatos;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    escribirComprobante = false;

                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);
                    //if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    //else this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Value + 10;

                    ticodt = dr["TICODT"].ToString().PadLeft(2, '0');
                    ctaAux = dr["CAUXDT"].ToString().Trim();
                    codPlan = dr["TIPLDT"].ToString().Trim();
                    ctaMayor = dr["CUENDT"].ToString().Trim();

                    //Chequear autorización a los movimientos del tipo de comprobante
                    autTipoComp = GlobalVar.UsuarioEnv.UsuarioAutorizado(aut, "004", "03", "10", ticodt);

                    if (autTipoComp)
                    {
                        if (chequearAutCtaAux)
                        {
                            //Verificar la cuenta de auxiliar si no se ha realizado
                            auxTipoCuentaAux = this.codigoTipoAux.PadRight(2, ' ') + ctaAux.PadRight(8, ' ');
                            auxTipoCuentaAuxValor = utiles.FindFirstValueByKey(ref dictAutCtaAux, auxTipoCuentaAux);

                            if (auxTipoCuentaAuxValor != auxTipoCuentaAux) autCuentaAux = true;
                            else
                            {
                                resultValCtaAux = this.ValidarCuentaAuxiliar(ctaAux, this.codigoTipoAux);
                                if (resultValCtaAux == "")
                                {
                                    autCuentaAux = true;
                                    this.dictAutCtaAux.Add(auxTipoCuentaAux, "1");
                                }
                                else
                                {
                                    autCuentaAux = false;
                                    aviso = resultValCtaAux;
                                    if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                                }
                            }
                        }

                        if (chequearAutCtaMayor)
                        {
                            string tipoCta = "";

                            //Verificar la cuenta de mayor si no se ha realizado
                            auxPlanCuentaMayor = codPlan.PadRight(1, ' ') + ctaMayor.PadRight(15, ' ');
                            auxPlanCuentaMayorTipo = utiles.FindFirstValueByKey(ref dictAutCtaMayor, auxPlanCuentaMayor);

                            if (auxPlanCuentaMayorTipo != auxPlanCuentaMayor)
                            {
                                autCuentaMayor = true;
                                tipoCta = auxPlanCuentaMayorTipo;
                            }
                            else
                            {
                                resultValCtaMayor = this.ValidarCuentaMayor(ctaMayor, codPlan, ref tipoCta);
                                if (resultValCtaMayor == "")
                                {
                                    autCuentaMayor = true;
                                    this.dictAutCtaMayor.Add(auxPlanCuentaMayor, tipoCta);
                                }
                                else
                                {
                                    autCuentaMayor = false;
                                    aviso = resultValCtaMayor;
                                    if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                                }
                            }
                        }

                        if (autCuentaAux && autCuentaMayor && ctaAux != "99999999")
                        {
                            if (registro == 0)
                            {
                                hayReg = true;
                                this.InformeHTMLEscribirCabeceras(fila1, fila2, columnas);
                                this.InformeHTMLEscribirTagTable(ref this.documento_HTML, false);
                            }

                            tauxdtActual = dr.GetValue(dr.GetOrdinal("TAUXDT")).ToString();
                            cauxdtActual = dr.GetValue(dr.GetOrdinal("CAUXDT")).ToString();
                            cciadtActual = dr.GetValue(dr.GetOrdinal("CCIADT")).ToString();
                            cuendtActual = dr.GetValue(dr.GetOrdinal("CUENDT")).ToString();
                            cldodtActual = dr.GetValue(dr.GetOrdinal("CLDODT")).ToString();
                            ndocdtActual = dr.GetValue(dr.GetOrdinal("NDOCDT")).ToString();
                            saprdtActual = dr.GetValue(dr.GetOrdinal("SAPRDT")).ToString();

                            if (!(tauxdt == "" && cauxdt == "" && cciadt == "" && cuendt == "" && (cldodt == "" || ndocdt == "")))
                            {
                                if (tauxdt == tauxdtActual && cauxdt == cauxdtActual && cciadt == cciadtActual && cuendt == cuendtActual && cldodt == cldodtActual && ndocdt == ndocdtActual)
                                {
                                    //Mismo documento
                                    if (!this.chkSaldoInicial.Checked ||
                                        Convert.ToInt32(saprdt) >= saprDesde && Convert.ToInt32(saprdt) <= saprHasta)
                                    {
                                        datos[17] = "";

                                        escribirRegistro = true;
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(saprdt) < Convert.ToInt32(saprdtActual) &&
                                            Convert.ToInt32(saprdtActual) >= saprDesde && Convert.ToInt32(saprdtActual) <= saprHasta
                                            && saldoAcumulado != 0)
                                        {
                                            //Caso documento anterior y mismo documento actual en el periodo solicitado
                                            //Linea de Saldo Incial
                                            datos[6] = "";
                                            datos[7] = "";
                                            datos[9] = "";
                                            datos[10] = "";
                                            datos[11] = "";
                                            datos[12] = "";
                                            datos[13] = "";
                                            datos[14] = this.LP.GetText("lblSaldoIni", "Saldo inicial").ToUpper();
                                            datos[15] = "";
                                            datos[16] = "";
                                            datos[17] = saldoAcumulado.ToString();

                                            escribirRegistro = true;
                                            //saldoAcumulado = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    //Documento diferente
                                    if (this.chkSaldoInicial.Checked)
                                    {
                                        //Saldo inicial marcado
                                        if (Convert.ToInt32(saprdt) >= saprDesde && Convert.ToInt32(saprdt) <= saprHasta)
                                        {
                                            //Si el documento está dentro del período solicitado
                                            if (saldoAcumulado != 0) datos[17] = saldoAcumulado.ToString();
                                            else datos[17] = "";

                                            escribirRegistro = true;
                                            saldoAcumulado = 0;
                                        }
                                        else
                                        {
                                            //documento fuera del período solicitado
                                            if (saldoAcumulado != 0)
                                            {
                                                //Existe saldo acumulado
                                                //Linea de Saldo Incial
                                                datos[6] = "";
                                                datos[7] = "";
                                                datos[9] = "";
                                                datos[10] = "";
                                                datos[11] = "";
                                                datos[12] = "";
                                                datos[13] = "";
                                                datos[14] = this.LP.GetText("lblSaldoIni", "Saldo inicial").ToUpper();
                                                datos[15] = "";
                                                datos[16] = "";
                                                datos[17] = saldoAcumulado.ToString();

                                                escribirRegistro = true;
                                                saldoAcumulado = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Saldo inicial no solicitado
                                        datos[17] = saldoAcumulado.ToString();

                                        escribirRegistro = true;
                                        saldoAcumulado = 0;
                                    }
                                }

                                if (escribirRegistro)
                                {
                                    //Escribe la línea del detalle del comprobante en el fichero HTML
                                    this.InformeHTMLEscribirDetalleComprobante(datos);

                                    escribirRegistro = false;
                                    escribirComprobante = true;
                                    hayReg = true;
                                    if (this.chkTotales.Checked) this.ActualizarTotales(datos);

                                    regXCta++;
                                }
                            }

                            if (registro != 0 && this.chkTotales.Checked && escribirComprobante)
                            {
                                //Verificar si hay que escribir los totales
                                if (cauxdt != cauxdtActual)
                                {
                                    if (regXCta != 0) this.EscribirTotal("ctaMayor", cuendtEditada);
                                    if (this.codigoCompania == "") this.EscribirTotal("compania", cciadt);
                                    this.EscribirTotal("ctaAux", cauxdt);
                                }
                                else
                                    if (cciadt != cciadtActual)
                                {
                                    if (regXCta != 0) this.EscribirTotal("ctaMayor", cuendtEditada);
                                    this.EscribirTotal("compania", cciadt);
                                }
                                else
                                if (cuendt != cuendtActual && regXCta != 0) this.EscribirTotal("ctaMayor", cuendtEditada);
                            }

                            //falta si tiene saldo inicial
                            datos[0] = cciadtActual;
                            datos[1] = tauxdtActual;
                            datos[2] = cauxdtActual;

                            this.ObtenerNombreZonaAuxiliar(tauxdtActual, cauxdtActual);

                            datos[3] = nombreAuxiliar;
                            datos[4] = zonaAuxiliar;

                            if (this.chkCuentaEditada.Checked)
                            {
                                if (cuendtActualEditada == "" || cuendtActualEditada != cuendtActual)
                                {
                                    cuentaDatos = utilesCG.ObtenerDatosCuenta(cuendtActual, this.codigoPlan);
                                    cuentaEscribirInforme = cuentaDatos[1].ToString();
                                    cuendtActualEditada = cuendtActual;
                                }
                            }
                            else cuentaEscribirInforme = cuendtActual;
                            //datos[5] = cuendtActual;
                            datos[5] = cuentaEscribirInforme;

                            comprobante = "";
                            if (saprdtActual.Length == 5) //saapp
                            {
                                comprobante = saprdtActual.Substring(1, 2);
                                comprobante += "-" + saprdtActual.Substring(3, 2) + "-";
                            }
                            else  //aapp
                            {
                                comprobante = saprdtActual.Substring(0, 2);
                                comprobante += "-" + saprdtActual.Substring(2, 2) + "-";
                            }
                            comprobante += dr.GetValue(dr.GetOrdinal("TICODT")).ToString().PadLeft(2, '0') + "-" + dr.GetValue(dr.GetOrdinal("NUCODT")).ToString().PadLeft(5, '0');
                            datos[6] = comprobante;

                            datos[7] = utiles.FechaToFormatoCG(dr["FECODT"].ToString()).ToShortDateString();
                            datos[8] = "";
                            if (cldodtActual != "" || ndocdtActual != "0") datos[8] = cldodtActual.PadRight(2, ' ') + "-" + ndocdtActual.PadLeft(7, '0');
                            datos[9] = "";
                            if (dr["FDOCDT"].ToString() != "0") datos[9] = utiles.FechaToFormatoCG(dr["FDOCDT"].ToString()).ToShortDateString();
                            datos[10] = "";
                            if (dr["FEVEDT"].ToString() != "0") datos[10] = utiles.FechaToFormatoCG(dr["FEVEDT"].ToString()).ToShortDateString();
                            datos[11] = dr.GetValue(dr.GetOrdinal("TEINDT")).ToString();
                            datos[12] = dr.GetValue(dr.GetOrdinal("AUAD01")).ToString();
                            datos[13] = dr.GetValue(dr.GetOrdinal("AUAD02")).ToString();
                            datos[14] = dr.GetValue(dr.GetOrdinal("DESCAD")).ToString();

                            switch (this.cmbMoneda.SelectedValue.ToString())
                            {
                                case "ML":
                                    importe = dr["MONTDT"].ToString();
                                    break;
                                case "ME":
                                    importe = dr["MOSMAD"].ToString();
                                    break;
                                case "TI":
                                    importe = dr["TERCAD"].ToString();
                                    break;
                            }

                            importeNum = Convert.ToDecimal(importe);

                            if (dr.GetValue(dr.GetOrdinal("TMOVDT")).ToString() == "D")
                            {
                                datos[15] = importe;
                                datos[16] = "";
                            }
                            else
                            {
                                datos[15] = "";
                                datos[16] = importe;
                            }
                            datos[17] = "";

                            saldoAcumulado += importeNum;

                            registro++;

                            tauxdt = tauxdtActual;
                            cauxdt = cauxdtActual;
                            cciadt = cciadtActual;
                            if (cuendt != cuendtActual) regXCta = 0;
                            cuendt = cuendtActual;
                            cuendtEditada = cuentaEscribirInforme;

                            cldodt = cldodtActual;
                            ndocdt = ndocdtActual;
                            saprdt = saprdtActual;
                        }
                    }
                    else
                    {
                        aviso = "Usuario no autorizado para algunos tipos de comprobantes"; //Falta traducir
                        if (!this.avisosAutorizaciones.Contains(aviso)) this.avisosAutorizaciones.Add(aviso);
                    }
                }

                dr.Close();

                if (hayReg)
                {
                    //Escribir el último registro
                    if (this.chkSaldoInicial.Checked)
                    {
                        if (Convert.ToInt32(saprdt) >= saprDesde && Convert.ToInt32(saprdt) <= saprHasta)
                        {
                            if (saldoAcumulado != 0) datos[17] = saldoAcumulado.ToString();
                            else datos[17] = "";

                            escribirRegistro = true;
                        }
                        else
                        {
                            if (saldoAcumulado != 0)
                            {
                                //Linea de Saldo Incial
                                datos[6] = "";
                                datos[7] = "";
                                datos[9] = "";
                                datos[10] = "";
                                datos[11] = "";
                                datos[12] = "";
                                datos[13] = "";
                                datos[14] = this.LP.GetText("lblSaldoIni", "Saldo inicial").ToUpper();
                                datos[15] = "";
                                datos[16] = "";

                                if (saldoAcumulado != 0) datos[17] = saldoAcumulado.ToString();
                                else datos[17] = "";

                                escribirRegistro = true;
                            }
                        }

                        saldoAcumulado = 0;
                    }
                    else
                    {
                        if (saldoAcumulado != 0) datos[17] = saldoAcumulado.ToString();
                        else datos[17] = "";

                        escribirRegistro = true;
                        saldoAcumulado = 0;
                    }

                    if (escribirRegistro)
                    {
                        //Escribe la línea del detalle del comprobante en el fichero HTML
                        this.InformeHTMLEscribirDetalleComprobante(datos);

                        if (this.chkTotales.Checked) this.ActualizarTotales(datos);

                        regXCta++;
                    }

                    if (this.chkTotales.Checked && escribirComprobante)
                    {
                        //Verificar si hay que escribir los totales
                        if (regXCta != 0) this.EscribirTotal("ctaMayor", cuendtEditada);
                        if (this.codigoCompania == "") this.EscribirTotal("compania", cciadt);
                        this.EscribirTotal("ctaAux", cauxdt);

                        //Escribir el Total Final
                        this.EscribirTotal("final", "");
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

                    /*
                    //Escribir los ajustes globales en el fichero HTML / Excel
                    if (tipoFichero == "HTML")
                    {
                        this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);

                        //Finaliza la barra de progreso
                        this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

                        this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML,Informes);
                    }
                    else
                    {
                        //Finaliza la barra de progreso
                        this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);

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
                        // fin levantar
                    }
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

                if (dr != null) dr.Close();

                this.mensajeProceso = "Error generando el informe. Para más información consulte el fichero de Log.";

                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
            }
        }

        /// <summary>
        /// Actualiza las variables de totales
        /// </summary>
        /// <param name="datos"></param>
        private void ActualizarTotales(string[] datos)
        {
            decimal importe;

            //Debe
            if (datos[15] != "")
            {
                importe = Convert.ToDecimal(datos[15]);
                this.totalCompaniaDebe += importe;
                this.totalCtaAuxDebe += importe;
                this.totalCtaMayorDebe += importe;
                this.totalFinalDebe += importe;

                this.totalCompaniaSaldo += importe;
                this.totalCtaAuxSaldo += importe;
                this.totalCtaMayorSaldo += importe;
                this.totalFinalSaldo += importe;
            }

            //Haber
            if (datos[16] != "")
            {
                importe = Convert.ToDecimal(datos[16]);
                this.totalCompaniaHaber += importe;
                this.totalCtaAuxHaber += importe;
                this.totalCtaMayorHaber += importe;
                this.totalFinalHaber += importe;

                this.totalCompaniaSaldo += importe;
                this.totalCtaAuxSaldo += importe;
                this.totalCtaMayorSaldo += importe;
                this.totalFinalSaldo += importe;
            }

            //Saldo
            if (datos[17] != "" && datos[14] == this.LP.GetText("lblSaldoIni", "Saldo inicial").ToUpper())
            {
                importe = Convert.ToDecimal(datos[17]);
                this.totalCompaniaSaldo += importe;
                this.totalCtaAuxSaldo += importe;
                this.totalCtaMayorSaldo += importe;
                this.totalFinalSaldo += importe;
            }
        }

        /// <summary>
        /// Escribe una línea de total
        /// </summary>
        /// <param name="tipo">Tipo de total [cuenta de mayor (ctaMayor), compañía (compania), cuenta auxiliar (ctaAux) y total final (final)]</param>
        /// <param name="codigo">código de la cuenta o de la compañía o de la cuenta de auxiliar</param>
        private void EscribirTotal(string tipo, string codigo)
        {
            string[] datos = new string[18];

            for (int i = 0; i <= 13; i++)
                datos[i] = "";

            switch (tipo)
            {
                case "ctaMayor":
                    datos[14] = this.LP.GetText("lblTotalCtaMayor", "TOTAL CTA. MAYOR").ToUpper() + " " + codigo;
                    datos[15] = totalCtaMayorDebe.ToString();
                    datos[16] = totalCtaMayorHaber.ToString();
                    datos[17] = totalCtaMayorSaldo.ToString();
                    this.totalCtaMayorDebe = 0;
                    this.totalCtaMayorHaber = 0;
                    this.totalCtaMayorSaldo = 0;
                    break;
                case "compania":
                    datos[14] = this.LP.GetText("lblTotalCompania", "TOTAL COMPAÑÍA").ToUpper() + " " + codigo;
                    datos[15] = totalCompaniaDebe.ToString();
                    datos[16] = totalCompaniaHaber.ToString();
                    datos[17] = totalCompaniaSaldo.ToString();
                    this.totalCompaniaDebe = 0;
                    this.totalCompaniaHaber = 0;
                    this.totalCompaniaSaldo = 0;
                    break;
                case "ctaAux":
                    datos[14] = this.LP.GetText("lblTotalCtaAux", "TOTAL CTA.AUX.").ToUpper() + " " + codigo;
                    datos[15] = totalCtaAuxDebe.ToString();
                    datos[16] = totalCtaAuxHaber.ToString();
                    datos[17] = totalCtaAuxSaldo.ToString();
                    this.totalCtaAuxDebe = 0;
                    this.totalCtaAuxHaber = 0;
                    this.totalCtaAuxSaldo = 0;
                    break;
                case "final":
                    datos[14] = this.LP.GetText("lblCabTotalFinal", "TOTAL FINAL").ToUpper() + " " + codigo;
                    datos[15] = totalFinalDebe.ToString();
                    datos[16] = totalFinalHaber.ToString();
                    datos[17] = totalFinalSaldo.ToString();
                    this.totalFinalDebe = 0;
                    this.totalFinalHaber = 0;
                    this.totalFinalSaldo = 0;
                    break;
            }

            //Escribe la línea del detalle del comprobante en el fichero HTML
            this.InformeHTMLEscribirDetalleComprobante(datos);
        }

        /// <summary>
        /// Obtiene el nombre del auxiliar y la zona del auxiliar
        /// </summary>
        /// <param name="taux">tipo de auxiliar</param>
        /// <param name="caux">código del auxiliar</param>
        private void ObtenerNombreZonaAuxiliar(string taux, string caux)
        {
            nombreAuxiliar = "";
            zonaAuxiliar = "";

            IDataReader dr = null;
            try
            {
                //Verificar si ya existe su información
                string auxTipoCuenta = taux.PadRight(2, ' ') + caux.PadRight(8, ' ');
                string auxZonaNombre = utiles.FindFirstKeyByValue(ref this.dictAuxZonaNombre, auxTipoCuenta);

                if (auxZonaNombre != auxTipoCuenta)
                {
                    if (auxZonaNombre.Length != 48) auxZonaNombre = auxZonaNombre.PadRight(48, ' ');
                    zonaAuxiliar = auxZonaNombre.Substring(0, 8).Trim();
                    nombreAuxiliar = auxZonaNombre.Substring(7, 40).Trim();
                }
                else
                {
                    string query = "select NOMBMA, ZONAMA from " + GlobalVar.PrefijoTablaCG + "GLM05 where TAUXMA = '" + taux + "' and CAUXMA = '" + caux + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        nombreAuxiliar = dr.GetValue(dr.GetOrdinal("NOMBMA")).ToString();
                        zonaAuxiliar = dr.GetValue(dr.GetOrdinal("ZONAMA")).ToString();
                    }

                    dr.Close();

                    if (nombreAuxiliar != "" || zonaAuxiliar != "")
                    {
                        auxZonaNombre = zonaAuxiliar.PadRight(8, ' ') + nombreAuxiliar.PadRight(40, ' ');

                        this.dictAuxZonaNombre.Add(auxZonaNombre, auxTipoCuenta);
                    }
                }
            }
            catch (Exception ex)
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
            string[] columnas = new string[18];
            columnas[0] = this.LP.GetText("lblCabCodigoCompania", "Cia");
            columnas[1] = this.LP.GetText("lblCabTA", "TA");
            columnas[2] = this.LP.GetText("lblCabAuxiliar", "Auxiliar");
            columnas[3] = this.LP.GetText("lblCabNombre", "Nombre");
            columnas[4] = this.LP.GetText("lblCabZona", "Zona");
            columnas[5] = this.LP.GetText("lblCabCtaMayor", "Cta. Mayor");
            columnas[6] = this.LP.GetText("lblCabComprobante", "Comprobante");
            columnas[7] = this.LP.GetText("lblCabFContable", "F. Contable");
            columnas[8] = this.LP.GetText("lblCabNDocumento", "N. Documento");
            columnas[9] = this.LP.GetText("lblCabFDocum", "F. Docum.");
            columnas[10] = this.LP.GetText("lblCabFechaVencim", "F. Vencim.");
            columnas[11] = this.LP.GetText("lblCabRU", "RU");
            columnas[12] = this.LP.GetText("lblCabAux2", "Auxiliar2");
            columnas[13] = this.LP.GetText("lblCabAux3", "Auxiliar3");
            columnas[14] = this.LP.GetText("lblCabDescripcion", "Descripción");
            columnas[15] = this.LP.GetText("lblCabDebe", "DEBE");
            columnas[16] = this.LP.GetText("lblCabHaber", "HABER");
            columnas[17] = this.LP.GetText("lblCabSaldo", "Saldo");

            return (columnas);
        }

        /// <summary>
        /// Devuelve la 1ra línea (título) del Inform de Diario Detallado
        /// </summary>
        /// <returns></returns>
        private void InformeTitulo(ref string fila1, ref string fila2, string comp_grupo)
        {
            try
            {
                fila1 = comp_grupo + " " + this.LP.GetText("lblCabTituloInfoMovAuxiliar", "Listado de Movimientos de Auxiliar") + " ";
                fila1 += " " + this.LP.GetText("lblCabFecha", "Fecha") + " " + System.DateTime.Now.ToShortDateString();

                fila2 = this.LP.GetText("lblDelPeriodo", "Del periodo") + " ";
                if (this.txtMaskDesdePeriodos.Text.Length == 5)
                {
                    fila2 += this.txtMaskDesdePeriodos.Text.Substring(0, 2) + "-" + this.txtMaskDesdePeriodos.Text.Substring(3, 2) + " " + this.LP.GetText("lblAlPeriodo", "al periodo") + " ";
                }
                if (this.txtMaskHastaPeriodos.Text.Length == 5)
                {
                    fila2 += this.txtMaskHastaPeriodos.Text.Substring(0, 2) + "-" + this.txtMaskHastaPeriodos.Text.Substring(3, 2);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void InformeHTMLEscribirCabeceras(string fila1, string fila2, string[] cabecerasColumnas)
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
            documento_HTML.Append("     </table>\n");
        }

        private void InformeHTMLEscribirDetalleComprobante(string[] detalleComprobante)
        {
            bool lineaTotales = false;
            //Linea de Totales
            if (detalleComprobante[0] == "" && detalleComprobante[1] == "" && detalleComprobante[3] == "" &&
                detalleComprobante[4] == "" && detalleComprobante[5] == "")
            {
                lineaTotales = true;
            }

            //documento_HTML.Append("     <table>");
            documento_HTML.Append("         <tr>\n");
            string valor = (detalleComprobante[0] == "") ? "&nbsp;" : detalleComprobante[0];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[1] == "") ? "&nbsp;" : detalleComprobante[1];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[2] == "") ? "&nbsp;" : detalleComprobante[2];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[3] == "") ? "&nbsp;" : detalleComprobante[3];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[4] == "") ? "&nbsp;" : detalleComprobante[4];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[5] == "") ? "&nbsp;" : detalleComprobante[5];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[6] == "") ? "&nbsp;" : detalleComprobante[6];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[7] == "") ? "&nbsp;" : detalleComprobante[7];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[8] == "") ? "&nbsp;" : detalleComprobante[8];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[9] == "") ? "&nbsp;" : detalleComprobante[9];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[10] == "") ? "&nbsp;" : detalleComprobante[10];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[11] == "") ? "&nbsp;" : detalleComprobante[11];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[12] == "") ? "&nbsp;" : detalleComprobante[12];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[13] == "") ? "&nbsp;" : detalleComprobante[13];
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[14] == "") ? "&nbsp;" : detalleComprobante[14];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=Texto>" + valor + "</td>\n");
            valor = (detalleComprobante[15] == "") ? "&nbsp;" : detalleComprobante[15];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[16] == "") ? "&nbsp;" : detalleComprobante[16];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
            valor = (detalleComprobante[17] == "") ? "&nbsp;" : detalleComprobante[17];
            if (lineaTotales) valor = "<b>" + valor + "</b>";
            documento_HTML.Append("             <td class=NumeroCG align=\"right\">" + valor + "</td>\n");
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
                //Iniciar la barra de progreso
                //this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Marquee);

                if (this.chkTotales.Checked)
                {
                    this.totalCtaMayorDebe = 0;
                    this.totalCtaMayorHaber = 0;
                    this.totalCtaMayorSaldo = 0;
                    this.totalCompaniaDebe = 0;
                    this.totalCompaniaHaber = 0;
                    this.totalCompaniaSaldo = 0;
                    this.totalCtaAuxDebe = 0;
                    this.totalCtaAuxHaber = 0;
                    this.totalCtaAuxSaldo = 0;
                    this.totalFinalDebe = 0;
                    this.totalFinalHaber = 0;
                    this.totalFinalSaldo = 0;
                }

                //Array de Compañías a Procesar
                aEmpresas = new ArrayList();

                backgroundWorker1.RunWorkerAsync();
                //this.ProcesarInforme();

                //Grabar la petición
                string valores = this.ValoresPeticion();

                this.valoresFormulario.GrabarParametros(formCode, valores);

                //Finaliza la barra de progreso
                this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
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
                        { "Tipo Aux", "radButtonTextBoxSelTipoAux" },
                        { "Documentos", "radDropDownListDocumentos" },
                        { "Cuentas Aux", "lbCuentasAux" },
                        { "Cuentas Mayor", "lbCuentasMayor" },
                        /*dictControles.Add("Doc. Cancelados", "rbDocCancelados");
                        dictControles.Add("Doc. No Cancelados", "rbDocNoCancelados");
                        dictControles.Add("Doc. Todos", "rbTodos");*/
                        { "Moneda", "cmbMoneda" },
                        { "Periodo Desde", "txtMaskDesdePeriodos" },
                        { "Periodo Hasta", "txtMaskHastaPeriodos" },
                        /*dictControles.Add("Saldo Inicial", "chkSaldoInicial");
                        dctControles.Add("Totales", "chkTotales");*/
                        { "Titulo", "txttitulo" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radButtonTextBoxSelCuentaAux", "radButtonTextBoxSelCuentasMayor",
                                                                                   "chkSaldoInicial", "chkTotales", "txtDesdePeriodos", "chkCuentaEditada", "txtHastaPeriodos" });

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
                valores = valores.PadRight(724);
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCIMOVAUX myStruct = (StructGLL01_MCIMOVAUX)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCIMOVAUX));

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

                try
                {
                    if (myStruct.tipoAuxiliar.Trim() != "")
                    {
                        string codTipoAux = myStruct.tipoAuxiliar.Trim();

                        string tipoAuxDesc = "";
                        string resultValidarTipoAux = this.ValidarTipoAuxiliar(codTipoAux, ref tipoAuxDesc, false);

                        if (resultValidarTipoAux != "")
                        {
                            this.codigoTipoAux = "";
                            this.radButtonTextBoxSelTipoAux.Text = "";
                        }
                        else
                        {
                            this.codigoTipoAux = codTipoAux;
                            if (tipoAuxDesc != "") codTipoAux += " " + this.separadorDesc + " " + tipoAuxDesc;

                            this.radButtonTextBoxSelTipoAux.Text = codTipoAux;

                            this.gBoxListaCuentasAux.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.documentos.Trim() != "")
                {
                    try
                    {
                        this.radDropDownListDocumentos.SelectedValue = myStruct.documentos;
                    }
                    catch (Exception ex)
                    {
                        this.radDropDownListDocumentos.SelectedIndex = 2;
                        Log.Error(Utiles.CreateExceptionString(ex));
                    }
                }
                else this.radDropDownListDocumentos.SelectedIndex = 2;

                try
                {
                    if (myStruct.moneda.Trim() != "") this.cmbMoneda.SelectedValue = myStruct.moneda.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                this.txtMaskDesdePeriodos.Text = myStruct.periodoDesde.Trim();
                this.txtMaskHastaPeriodos.Text = myStruct.periodoHasta.Trim();

                if (myStruct.saldoInicial == "1") this.chkSaldoInicial.Checked = true;
                else this.chkSaldoInicial.Checked = false;

                if (myStruct.totales == "1") this.chkTotales.Checked = true;
                else this.chkTotales.Checked = false;

                if (myStruct.cuentaEditada == "1") this.chkCuentaEditada.Checked = true;
                else this.chkCuentaEditada.Checked = false;

                this.txttitulo.Text = myStruct.titulo.Trim();

                //Cuentas auxiliar
                if (myStruct.cuentasAuxiliar.Trim() != "")
                {
                    string cuentasAux = myStruct.cuentasAuxiliar.Trim();
                    string[] aCuentasAux = cuentasAux.Split(';');
                    this.lbCuentasAux.Items.Clear();
                    string cuentaAux = "";
                    for (int i = 0; i < aCuentasAux.Length; i++)
                    {
                        cuentaAux = aCuentasAux[i].Trim();
                        if (cuentaAux != "") this.lbCuentasAux.Items.Add(aCuentasAux[i]);
                    }
                }

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
                StructGLL01_MCIMOVAUX myStruct;

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

                myStruct.tipoAuxiliar = codigoTipoAux.PadRight(2, ' ');

                if (this.radDropDownListDocumentos.SelectedValue != null) myStruct.documentos = this.radDropDownListDocumentos.SelectedValue.ToString();
                else myStruct.documentos = "0";

                if (this.cmbMoneda.SelectedValue != null && this.cmbMoneda.SelectedValue.ToString().Trim() != "")
                {
                    myStruct.moneda = this.cmbMoneda.SelectedValue.ToString().PadRight(2, ' ');
                }
                else myStruct.moneda = codigo.PadRight(2, ' ');

                myStruct.periodoDesde = this.txtMaskDesdePeriodos.Text;
                myStruct.periodoHasta = this.txtMaskHastaPeriodos.Text;

                if (this.chkSaldoInicial.Checked) myStruct.saldoInicial = "1";
                else myStruct.saldoInicial = "0";

                if (this.chkTotales.Checked) myStruct.totales = "1";
                else myStruct.totales = "0";

                //Cuenta Editada
                if (this.chkCuentaEditada.Checked) myStruct.cuentaEditada = "1";
                else myStruct.cuentaEditada = "0";

                myStruct.titulo = this.txttitulo.Text.PadRight(100, ' ');
                myStruct.titulo = myStruct.titulo.Replace("'", "''");

                //cuentas de auxiliar
                myStruct.cuentasAuxiliar = "";
                for (int i = 0; i < this.lbCuentasAux.Items.Count; i++)
                {
                    myStruct.cuentasAuxiliar += this.lbCuentasAux.Items[i] + ";";
                }

                if (myStruct.cuentasAuxiliar.Length > 300) myStruct.cuentasAuxiliar = myStruct.cuentasAuxiliar.Substring(0, 299);
                else myStruct.cuentasAuxiliar = myStruct.cuentasAuxiliar.PadRight(300, ' ');

                //cuentas de mayor
                myStruct.cuentasMayor = "";
                for (int i = 0; i < this.lbCuentasMayor.Items.Count; i++)
                {
                    myStruct.cuentasMayor += this.lbCuentasMayor.Items[i] + ";";
                }

                if (myStruct.cuentasMayor.Length > 300) myStruct.cuentasMayor = myStruct.cuentasMayor.Substring(0, 299);

                result = myStruct.compania + myStruct.grupo + myStruct.plan + myStruct.tipoAuxiliar + myStruct.documentos + myStruct.moneda;
                result += myStruct.periodoDesde + myStruct.periodoHasta + myStruct.saldoInicial + myStruct.totales + myStruct.cuentaEditada + myStruct.titulo;
                result += myStruct.cuentasAuxiliar + myStruct.cuentasMayor;

                //int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIMOVAUX));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //if (result.Length < 145) result.PadRight(145, ' ');

            return (result);
        }

        #endregion
    }
}