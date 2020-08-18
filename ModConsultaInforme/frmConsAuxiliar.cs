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
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmConsAuxiliar : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCICONAUX";
        public string ficheroExtension = "cau";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCICONAUX
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string tipoAuxiliar;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string cuentaAuxiliar;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string posicionAuxiliar;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string compania;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string grupo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string plan;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string aappDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string aappHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string cuentaMayor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string mostrarCuentas;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string documentos;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string monedaExt;
        }

        private string codigoTipoAux = "";
        private string descTipoAux = "";
        private string codigoCompania = "";
        private string descCompania = "";
        private string codigoGrupo = "";
        private string descGrupo = "";
        private string codigoPlan = "";
        private string descPlan = "";
        private string codigoCtaAux = "";
        private string descCtaAux = "";
        private string codigoCtaMayor = "";
        private string descCtaMayor = "";

        private string saappDesde = "";
        private string saappHasta = "";

        private string ctaMayorTipo = "";
        private string ctaMayorDocumento = "";

        FormularioValoresCampos valoresFormulario;

        private System.Data.DataTable dtPosAux;
        private System.Data.DataTable dtMostrarCtas;
        private System.Data.DataTable dtDocumentos;

        private bool cargarPlanes = false;

        public frmConsAuxiliar()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsAuxiliar_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Auxiliar");

            this.TraducirLiterales();

            //Crear Tabla Posición del Auxiliar
            this.dtPosAux = new System.Data.DataTable();
            this.dtPosAux.Columns.Add("valor", typeof(string));
            this.dtPosAux.Columns.Add("desc", typeof(string));

            //Crear el desplegable con la posición del Aux
            CrearComboPosAux();

            //Crear Tabla Mostrar Cuentas
            this.dtMostrarCtas = new System.Data.DataTable();
            this.dtMostrarCtas.Columns.Add("valor", typeof(string));
            this.dtMostrarCtas.Columns.Add("desc", typeof(string));

            //Crear el desplegable con mostrar cuentas
            CrearComboMostrarCuentas();

            //Crear Tabla Documentos
            this.dtDocumentos = new System.Data.DataTable();
            this.dtDocumentos.Columns.Add("valor", typeof(string));
            this.dtDocumentos.Columns.Add("desc", typeof(string));

            //Crear el desplegable con documentos
            CrearComboDocumentos();

            //Cargar compañías
            this.FillCompanias();

            //Cargar grupo de compañías
            this.FillGruposCompanias();

            //Cargar planes
            this.FillPlanes("");

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                this.CargarValoresUltimaPeticion(valores);
            }

            this.radButtonTextBoxSelTipoAux.Select();
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

        private void RadButtonElementSelCuentaAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CAUXMA, NOMBMA, PCIFMA, NNITMA, ";
            query += "(CASE STATMA When 'V' Then 'Activa' Else 'Inactiva' End) STATMA from ";
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
            }
            this.radButtonTextBoxSelCuentaAux.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelCuentaAux;
            this.radButtonTextBoxSelCuentaAux.Select(0, 0);
            this.radButtonTextBoxSelCuentaAux.Focus();
        }

        private void RadDropDownListMostrarCtas_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (this.radDropDownListMostrarCtas.SelectedValue != null)
                {
                    switch (this.radDropDownListMostrarCtas.SelectedValue.ToString())
                    {
                        case "1":   //Sin documento
                            this.radDropDownListDocumentos.Enabled = false;
                            break;
                        case "2":   //Con documento
                            if (this.ctaMayorDocumento == "S") this.radDropDownListDocumentos.Enabled = false;
                            break;
                        case "3":   //Todas
                            if (this.ctaMayorDocumento == "S") this.radDropDownListDocumentos.Enabled = true;
                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonElementSelCuentasMayor_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select min(CUENMC) CUENMC, max(NOLAAD) NOLAAD, CEDTMC from ";
            query += GlobalVar.PrefijoTablaCG + "GLM03 ";
            query += "where TCUEMC = 'D' and TIPLMC = '" + codigoPlan + "' and STATMC = 'V' ";
            query += "group by CEDTMC order by CUENMC";

            //Definir la cabecera de las columnas
            ArrayList nombreColumnas = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
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

            }
            this.radButtonTextBoxSelCuentasMayor.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelCuentasMayor;
            this.radButtonTextBoxSelCuentasMayor.Select(0, 0);
            this.radButtonTextBoxSelCuentasMayor.Focus();
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

                    this.radButtonTextBoxSelCuentasMayor.Text = "";
                    this.radButtonTextBoxSelCuentasMayor.Enabled = false;
                }
                else
                {
                    this.radDropDownListGrupo.SelectedValue = "";
                    this.radDropDownListGrupo.Refresh();
                    this.radButtonTextBoxSelCuentasMayor.Enabled = true;

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
                this.codigoPlan = this.radDropDownListPlan.SelectedValue.ToString();

                string descPlan = "";
                string result = this.ValidarPlan(this.codigoPlan, ref descPlan);

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                    this.radDropDownListPlan.SelectedValue = "";
                    this.radDropDownListPlan.Focus();
                }
                else
                {
                    this.radButtonTextBoxSelCuentasMayor.Enabled = true;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonTextBoxSelTipoAux_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string codigo = this.radButtonTextBoxSelTipoAux.Text.Trim();
            if (codigo != "")
            {
                if (codigo != "" && codigo.Length >= 2)
                {
                    if (codigo.Length <= 1) this.codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text;
                    else this.codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text.Substring(0, 2);

                    string descTipoAux = "";
                    string result = this.ValidarTipoAuxiliar(this.codigoTipoAux, ref descTipoAux, false);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        this.radButtonTextBoxSelCuentaAux.Enabled = false;
                        this.radButtonTextBoxSelTipoAux.Focus();
                    }
                    else
                    {
                        string codTipoAux = this.codigoTipoAux;
                        if (descTipoAux != "") codTipoAux += " " + this.separadorDesc + " " + descTipoAux;

                        this.radButtonTextBoxSelTipoAux.Text = codTipoAux;

                        this.radButtonTextBoxSelCuentaAux.Enabled = true;
                        this.radButtonTextBoxSelCuentaAux.Focus();
                    }
                }
                else
                {
                    this.radButtonTextBoxSelCuentaAux.Enabled = false;
                    this.radButtonTextBoxSelTipoAux.Focus();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonTextBoxSelCuentaAux_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string codigo = this.radButtonTextBoxSelCuentaAux.Text.Trim();

            if (codigo != "")
            {
                int posSep = codigo.IndexOf('-');
                if (posSep != -1) codigo = codigo.Substring(0, posSep - 1);
                if (codigo != "")
                {
                    string descCtaAux = "";
                    string validarCuentaAux = this.ValidarCuentaAuxiliar(codigo, this.codigoTipoAux, ref descCtaAux);

                    if (validarCuentaAux != "")
                    {
                        RadMessageBox.Show(validarCuentaAux, this.LP.GetText("errValTitulo", "Error"));
                        this.radButtonTextBoxSelCuentaAux.Select();
                        return;
                    }
                    else
                    {
                        this.codigoCtaMayor = codigo;

                        if (descCtaAux != "") codigo = codigo + " " + this.separadorDesc + " " + descCtaAux;

                        this.radButtonTextBoxSelCuentaAux.Text = codigo;
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonTextBoxSelCuentasMayor_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string codigo = this.radButtonTextBoxSelCuentasMayor.Text.Trim();
            if (codigo != "")
            {
                int posSep = codigo.IndexOf('-');
                if (posSep != -1) codigo = codigo.Substring(0, posSep - 1);
                if (codigo != "")
                {
                    string descCtaMayor = "";
                    int posAux = this.ObtenerPosicionAux();

                    string validarCuentaMayor = this.ValidarCuentaMayorTipoDocumento(codigo, this.codigoPlan, ref descCtaMayor, this.codigoTipoAux, posAux, ref this.ctaMayorTipo, ref ctaMayorDocumento);

                    if (validarCuentaMayor != "")
                    {
                        RadMessageBox.Show(validarCuentaMayor, this.LP.GetText("errValTitulo", "Error"));
                        this.radButtonTextBoxSelCuentasMayor.Select();
                        return;
                    }
                    else
                    {
                        this.codigoCtaMayor = codigo;

                        if (descCtaMayor != "") codigo = codigo + " " + this.separadorDesc + " " + descCtaMayor;

                        this.radButtonTextBoxSelCuentasMayor.Text = codigo;

                        this.CuentaMayorChangeMostrarCtaDocumentosRefresh();
                    }
                }
            }
            else
            {
                this.radDropDownListMostrarCtas.Enabled = true;
                this.radDropDownListDocumentos.Enabled = true;
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

        private void FrmConsAuxiliar_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Auxiliar");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemMovimientosAux", "Movimientos de Auxiliar");
            this.Text = this.Text.Replace("&", "");
            this.lblTipoAux.Text = this.LP.GetText("lblTipoAux", "Tipo de Auxiliar");
            this.lblCtaAux.Text = this.LP.GetText("lblCtaAux", "Cuenta de Auxiliar");   //Falta traducir
            this.lblPosAux.Text = this.LP.GetText("lblPosAux", "Posición del auxiliar");   //Falta traducir
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblGrupo.Text = this.LP.GetText("lblGrupo", "Grupo");
            this.lblPlan.Text = this.LP.GetText("lblPlan", "Plan");
            this.lblDesdeAAPP.Text = this.LP.GetText("lblAAPeriodo", "Año-periodo");   //Falta traducir
            this.lblHastaAAPP.Text = this.LP.GetText("lblAAPeriodoHasta", "Hasta");   //Falta traducir
            this.lblCtaMayor.Text = this.LP.GetText("lblCtaMayor", "Cuenta de Mayor");   //Falta traducir
            this.chkMonedaExt.Text = this.LP.GetText("lblSinDoc", "Mostrar datos de moneda extranjera");   //Falta traducir
            this.lblDocumentos.Text = this.LP.GetText("lblSinDoc", "Documentos");   //Falta traducir

            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Crea el desplegable de posición del auxiliar
        /// </summary>
        private void CrearComboPosAux()
        {
            DataRow row;

            try
            {
                if (this.dtPosAux.Rows.Count > 0) this.dtPosAux.Rows.Clear();

                row = this.dtPosAux.NewRow();
                row["valor"] = "01";
                row["desc"] = this.LP.GetText("lblAux1", "Auxiliar 1");   //Falta traducir
                this.dtPosAux.Rows.Add(row);

                row = this.dtPosAux.NewRow();
                row["valor"] = "02";
                row["desc"] = this.LP.GetText("lblAux2", "Auxiliar 2");   //Falta traducir
                this.dtPosAux.Rows.Add(row);

                row = this.dtPosAux.NewRow();
                row["valor"] = "03";
                row["desc"] = this.LP.GetText("lblAux3", "Auxiliar 3");   //Falta traducir
                this.dtPosAux.Rows.Add(row);

                row = this.dtPosAux.NewRow();
                row["valor"] = "-1";
                row["desc"] = this.LP.GetText("lblIndiferente", "Indiferente");   //Falta traducir
                this.dtPosAux.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListPosAux.DataSource = this.dtPosAux;
            this.radDropDownListPosAux.ValueMember = "valor";
            this.radDropDownListPosAux.DisplayMember = "desc";
            this.radDropDownListPosAux.Refresh();
            this.radDropDownListPosAux.SelectedIndex = 3;
        }

        /// <summary>
        /// Crea el desplegable de crear cuentas
        /// </summary>
        private void CrearComboMostrarCuentas()
        {
            DataRow row;

            try
            {
                if (this.dtMostrarCtas.Rows.Count > 0) this.dtMostrarCtas.Rows.Clear();

                row = this.dtMostrarCtas.NewRow();
                row["valor"] = "1";
                row["desc"] = "Sin documento";   //Falta traducir
                this.dtMostrarCtas.Rows.Add(row);

                row = this.dtMostrarCtas.NewRow();
                row["valor"] = "2";
                row["desc"] = "Con documento";   //Falta traducir
                this.dtMostrarCtas.Rows.Add(row);

                row = this.dtMostrarCtas.NewRow();
                row["valor"] = "3";
                row["desc"] = "Todas";   //Falta traducir
                this.dtMostrarCtas.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListMostrarCtas.DataSource = this.dtMostrarCtas;
            this.radDropDownListMostrarCtas.ValueMember = "valor";
            this.radDropDownListMostrarCtas.DisplayMember = "desc";
            this.radDropDownListMostrarCtas.Refresh();
            this.radDropDownListMostrarCtas.SelectedIndex = 2;
        }

        /// <summary>
        /// Crea el desplegable de documentos
        /// </summary>
        private void CrearComboDocumentos()
        {
            DataRow row;

            try
            {
                if (this.dtDocumentos.Rows.Count > 0) this.dtDocumentos.Rows.Clear();

                row = this.dtDocumentos.NewRow();
                row["valor"] = "1";
                row["desc"] = "Cancelados";   //Falta traducir
                this.dtDocumentos.Rows.Add(row);

                row = this.dtDocumentos.NewRow();
                row["valor"] = "2";
                row["desc"] = "No Cancelados";   //Falta traducir
                this.dtDocumentos.Rows.Add(row);

                row = this.dtDocumentos.NewRow();
                row["valor"] = "3";
                row["desc"] = "Todos";   //Falta traducir
                this.dtDocumentos.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListDocumentos.DataSource = this.dtDocumentos;
            this.radDropDownListDocumentos.ValueMember = "valor";
            this.radDropDownListDocumentos.DisplayMember = "desc";
            this.radDropDownListDocumentos.Refresh();
            this.radDropDownListDocumentos.SelectedIndex = 2;
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

            string query = "";
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
                        this.codigoPlan = "";
                        //this.tgTexBoxSelCtaMayor.Textbox.Text = "";
                        //this.tgTexBoxSelCtaMayor.Enabled = false;
                        this.tgTexBoxSelPlan.Textbox.Text = "";
                        this.tgTexBoxSelPlan.Enabled = false;
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

                        this.codigoPlan = "";
                        //this.tgTexBoxSelCtaMayor.Textbox.Text = "";
                        //this.tgTexBoxSelCtaMayor.Enabled = false;

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
        }
        */

        /// <summary>
        /// Ejecutar la consulta
        /// </summary>
        private void Ejecutar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string posAux = "-1";
                if (this.radDropDownListPosAux.SelectedValue != null)
                {
                    switch (this.radDropDownListPosAux.SelectedValue.ToString())
                    {
                        case "01":
                            posAux = "1";
                            break;
                        case "02":
                            posAux = "2";
                            break;
                        case "03":
                            posAux = "3";
                            break;
                    }
                }

                string mostrarCuentas = "";
                string documentos = "";

                if (this.radDropDownListMostrarCtas.Enabled)
                {
                    if (this.radDropDownListMostrarCtas.SelectedValue != null) mostrarCuentas = this.radDropDownListMostrarCtas.SelectedValue.ToString();

                    if (this.radDropDownListDocumentos.Enabled)
                    {
                        if (this.radDropDownListDocumentos.SelectedValue != null) documentos = this.radDropDownListDocumentos.SelectedValue.ToString();
                    }
                    else documentos = "-1";
                }
                else
                {
                    mostrarCuentas = "-1";

                    switch (this.ctaMayorDocumento)
                    {
                        case "S":
                            if (this.radDropDownListDocumentos.SelectedValue != null) documentos = this.radDropDownListDocumentos.SelectedValue.ToString();
                            break;
                        case "N":
                            documentos = "-1";
                            break;
                    }
                }

                /*
                if (this.chkSinDocumento.Enabled)
                {
                    if (this.chkSinDocumento.Checked) documentos = "-1";
                    else if (this.rbDocCancelados.Checked) documentos = "1";
                    else if (this.rbDocNoCancelados.Checked) documentos = "2";
                    else if (this.rbDocTodos.Checked) documentos = "3";
                }
                else
                {
                    switch (this.ctaMayorDocumento)
                    {
                        case "S":
                            if (this.rbDocCancelados.Checked) documentos = "1";
                            else if (this.rbDocNoCancelados.Checked) documentos = "2";
                            else if (this.rbDocTodos.Checked) documentos = "3";
                            break;
                        case "N":
                            documentos = "-1";
                            break;
                    }
                }
                */


                string datosMonedaExt = "0";
                if (this.chkMonedaExt.Checked) datosMonedaExt = "1";

                if (this.codigoCtaMayor == "" || this.ctaMayorTipo == "T")
                {
                    //Consulta por saldos de cuentas de mayor
                    frmConsAuxViewSaldos frmViewConsSaldos = new frmConsAuxViewSaldos
                    {
                        TipoAuxCodigo = this.codigoTipoAux,
                        TipoAuxDesc = this.radButtonTextBoxSelTipoAux.Text,
                        CompaniaCodigo = this.codigoCompania,
                        CompaniaDesc = this.cmbCompania.Text,
                        GrupoCodigo = this.codigoGrupo,
                        AAPPDesde = this.saappDesde,
                        AAPPDesdeFormat = this.txtMaskAAPPDesde.Text,
                        AAPPHasta = this.saappHasta,
                        AAPPHastaFormat = this.txtMaskAAPPHasta.Text,
                        CtaAuxCodigo = this.codigoCtaAux,
                        CtaAuxDesc = this.radButtonTextBoxSelCuentaAux.Text,
                        CtaMayorCodigo = this.codigoCtaMayor,
                        CtaMayorDesc = this.radButtonTextBoxSelCuentasMayor.Text,
                        PosAux = posAux,
                        MostrarCuentas = mostrarCuentas,
                        Documentos = documentos,
                        DatosMonedaExt = datosMonedaExt,
                        PorCuentasAux = false
                    };
                    if (this.radDropDownListGrupo.SelectedValue != null) frmViewConsSaldos.GrupoDesc = this.radDropDownListGrupo.SelectedValue.ToString();
                    else frmViewConsSaldos.GrupoDesc = null;
                    frmViewConsSaldos.PlanCodigo = this.codigoPlan;
                    if (this.radDropDownListPlan.SelectedValue != null) frmViewConsSaldos.PlanDesc = this.radDropDownListPlan.SelectedValue.ToString();
                    else frmViewConsSaldos.PlanDesc = null;

                    frmViewConsSaldos.Show();
                }
                else
                {
                    //Cuenta de auxiliar a último nivel
                    string codCtaUltNivel = "";
                    string fevemc = "";
                    try
                    {
                        int fila = 0;
                        string errorMsg = "";

                        DataTable dtCtasUltimoNivel = utilesCG.ObtenerCuentaUltimoNivelValoresCampos(this.codigoCtaMayor, this.codigoPlan, ref errorMsg);

                        if (errorMsg != "")
                        {
                            RadMessageBox.Show(errorMsg, this.LP.GetText("errValTitulo", "Error"));
                            this.radButtonTextBoxSelCuentasMayor.Select();
                            return;
                        }
                        else if (dtCtasUltimoNivel != null && dtCtasUltimoNivel.Rows.Count > 0)
                        {
                            fila = dtCtasUltimoNivel.Rows.Count - 1;
                            codCtaUltNivel = dtCtasUltimoNivel.Rows[fila]["CUENMC"].ToString();
                            fevemc = dtCtasUltimoNivel.Rows[fila]["FEVEMC"].ToString();
                        }

                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    if (this.ctaMayorDocumento == "S")
                    {
                        //Cuenta de Mayor con documento
                        frmConsAuxViewDoc frmViewConsDoc = new frmConsAuxViewDoc
                        {
                            TipoAuxCodigo = this.codigoTipoAux,
                            TipoAuxDesc = this.radButtonTextBoxSelTipoAux.Text,
                            CompaniaCodigo = this.codigoCompania,
                            CompaniaDesc = this.cmbCompania.Text,
                            GrupoCodigo = this.codigoGrupo,
                            AAPPDesde = this.saappDesde,
                            AAPPDesdeFormat = this.txtMaskAAPPDesde.Text,
                            AAPPHasta = this.saappHasta,
                            AAPPHastaFormat = this.txtMaskAAPPHasta.Text,
                            CtaAuxCodigo = this.codigoCtaAux,
                            CtaAuxDesc = this.radButtonTextBoxSelCuentaAux.Text,
                            CtaMayorCodigo = codCtaUltNivel,
                            CtaMayorDesc = this.radButtonTextBoxSelCuentasMayor.Text,
                            PosAux = posAux,
                            MostrarCuentas = mostrarCuentas,
                            Documentos = documentos,
                            DatosMonedaExt = datosMonedaExt,
                            FEVEMC = fevemc,
                            CalcularPrdoMedioPago = true,
                        };
                        if (this.radDropDownListGrupo.SelectedValue != null) frmViewConsDoc.GrupoDesc = this.radDropDownListGrupo.Text;
                        else frmViewConsDoc.GrupoDesc = null;
                        frmViewConsDoc.PlanCodigo = this.codigoPlan;
                        if (this.radDropDownListPlan.SelectedValue != null) frmViewConsDoc.PlanDesc = this.radDropDownListPlan.SelectedValue.ToString();
                        else frmViewConsDoc.PlanDesc = null;
                        
                        frmViewConsDoc.Show();
                    }
                    else
                    {
                        //Cuenta de Mayor No tiene documentos
                        if (this.codigoCtaAux != "")
                        {
                            //Ver los movimientos por cuenta y auxiliar
                            frmConsAuxViewMov frmViewConsMov = new frmConsAuxViewMov
                            {
                                TipoAuxCodigo = this.codigoTipoAux,
                                TipoAuxDesc = this.radButtonTextBoxSelTipoAux.Text,
                                CompaniaCodigo = this.codigoCompania,
                                CompaniaDesc = this.cmbCompania.Text,
                                GrupoCodigo = this.codigoGrupo,
                                AAPPDesde = this.saappDesde,
                                AAPPDesdeFormat = this.txtMaskAAPPDesde.Text,
                                AAPPHasta = this.saappHasta,
                                AAPPHastaFormat = this.txtMaskAAPPHasta.Text,
                                CtaAuxCodigo = this.codigoCtaAux,
                                CtaAuxDesc = this.radButtonTextBoxSelCuentaAux.Text,
                                CtaMayorCodigo = codCtaUltNivel,
                                CtaMayorDesc = this.radButtonTextBoxSelCuentasMayor.Text,
                                PosAux = posAux,
                                MostrarCuentas = mostrarCuentas,
                                Documentos = documentos,
                                DatosMonedaExt = datosMonedaExt
                            };
                            if (this.radDropDownListGrupo.SelectedValue != null) frmViewConsMov.GrupoDesc = this.radDropDownListGrupo.Text;
                            else frmViewConsMov.GrupoDesc = null;
                            frmViewConsMov.PlanCodigo = this.codigoPlan;
                            if (this.radDropDownListPlan.SelectedValue != null) frmViewConsMov.PlanDesc = this.radDropDownListPlan.SelectedValue.ToString();
                            else frmViewConsMov.PlanDesc = null;
                            //frmViewConsMov.Clase = clase;
                            //frmViewConsMov.NoDocumento = doc;
                            //frmViewConsMov.SaldoInicialDocumento = saldoInicial;
                            frmViewConsMov.Show(this);
                        }
                        else
                        {
                            //Ver los saldos por cuentas de auxiliar de la cuenta de mayor
                            frmConsAuxViewSaldos frmViewConsSaldos = new frmConsAuxViewSaldos
                            {
                                TipoAuxCodigo = this.codigoTipoAux,
                                TipoAuxDesc = this.radButtonTextBoxSelTipoAux.Text,
                                CompaniaCodigo = this.codigoCompania,
                                CompaniaDesc = this.cmbCompania.Text,
                                GrupoCodigo = this.codigoGrupo,
                                AAPPDesde = this.saappDesde,
                                AAPPDesdeFormat = this.txtMaskAAPPDesde.Text,
                                AAPPHasta = this.saappHasta,
                                AAPPHastaFormat = this.txtMaskAAPPHasta.Text,
                                CtaAuxCodigo = this.codigoCtaAux,
                                CtaAuxDesc = this.radButtonTextBoxSelCuentaAux.Text,
                                CtaMayorCodigo = this.codigoCtaMayor,
                                CtaMayorDesc = this.radButtonTextBoxSelCuentasMayor.Text,
                                PosAux = posAux,
                                MostrarCuentas = mostrarCuentas,
                                Documentos = documentos,
                                DatosMonedaExt = datosMonedaExt,
                                PorCuentasAux = true,
                            };
                            if (this.radDropDownListGrupo.SelectedValue != null) frmViewConsSaldos.GrupoDesc = this.radDropDownListGrupo.Text;
                            else frmViewConsSaldos.GrupoDesc = null;
                            frmViewConsSaldos.PlanCodigo = this.codigoPlan;
                            if (this.radDropDownListPlan.SelectedValue != null) frmViewConsSaldos.PlanDesc = this.radDropDownListPlan.SelectedValue.ToString();
                            else frmViewConsSaldos.PlanDesc = null;

                            frmViewConsSaldos.Show();
                        }
                    }
                }
            }
            else
            {
                //Error
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
                    FrmPadre = this,
                    FicheroExtension = this.ficheroExtension
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
                        { "Cuenta Mayor", "radButtonTextBoxSelCuentasMayor" },
                        { "AAPP Hasta", "txtMaskAAPPHasta" },
                        { "AAPP Desde", "txtMaskAAPPDesde" },
                        { "Plan", "radDropDownListPlan" },
                        { "Grupo de Compañía", "radDropDownListGrupo" },
                        { "Compañía", "cmbCompania" },
                        { "Cuenta Auxiliar", "radButtonTextBoxSelCuentaAux" },
                        { "Tipo Auxiliar", "radButtonTextBoxSelTipoAux" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radDropDownListDocumentos",
                                                                                   "chkMonedaExt", "chkSinDocumento",
                                                                                   "radDropDownListPosAux", "radDropDownListMostrarCtas"});

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
        /// Devuelve la posición del auxiliar solicitada
        /// </summary>
        /// <returns></returns>
        private int ObtenerPosicionAux()
        {
            int posAux = -1;

            if (this.radDropDownListPosAux.SelectedValue != null)
            {
                switch (this.radDropDownListPosAux.SelectedValue.ToString())
                {
                    case "01":
                        posAux = 1;
                        break;
                    case "02":
                        posAux = 2;
                        break;
                    case "03":
                        posAux = 3;
                        break;
                }
            }

            return (posAux);
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

            codigoTipoAux = "";
            if (this.radButtonTextBoxSelTipoAux.Text.Length <= 2) codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text;
            else codigoTipoAux = this.radButtonTextBoxSelTipoAux.Text.Substring(0, 2);
            
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

            string saapp = "";
            string resultMsg = this.ValidarPeriodoFormato(ref this.txtMaskAAPPDesde, true, ref saapp);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskAAPPDesde.Focus();
                return (false);
            }
            else this.saappDesde = saapp;

            saapp = "";
            resultMsg = this.ValidarPeriodoFormato(ref this.txtMaskAAPPHasta, false, ref saapp);
            if (resultMsg != "")
            {
                RadMessageBox.Show(resultMsg, error);
                this.txtMaskAAPPHasta.Focus();
                return (false);
            }
            else this.saappHasta = saapp;

            //Falta validar que el periodo hasta sea mayor que el periodo desde !!!!!

            codigoCtaAux = this.radButtonTextBoxSelCuentaAux.Text.Trim();
            int posSep = codigoCtaAux.IndexOf('-');
            if (posSep != -1) codigoCtaAux = codigoCtaAux.Substring(0, posSep - 1).Trim();
         
            if (codigoCtaAux != "")
            {
                string descCtaAux = "";
                string validarCuentaAux = this.ValidarCuentaAuxiliar(codigoCtaAux, this.codigoTipoAux, ref descCtaAux);

                if (validarCuentaAux != "")
                {
                    RadMessageBox.Show(validarCuentaAux, error);
                    this.radButtonTextBoxSelCuentaAux.Select();
                    return (false);
                }
                else
                {
                    if (descCtaAux.Trim() != "") this.descCtaAux = this.codigoCtaAux + " " + this.separadorDesc + " " + descCtaAux.Trim();
                    else this.descCtaAux = this.codigoCtaAux;
                }
            }

            codigoCtaMayor = this.radButtonTextBoxSelCuentasMayor.Text.Trim();
            posSep = codigoCtaMayor.IndexOf('-');
            if (posSep != -1) codigoCtaMayor = codigoCtaMayor.Substring(0, posSep - 1).Trim();
            if (codigoCtaMayor != "")
            {
                string descCtaMayor = "";
                int posAux = this.ObtenerPosicionAux();
                
                string validarCuentaMayor = this.ValidarCuentaMayorTipoDocumento(codigoCtaMayor, this.codigoPlan, ref descCtaMayor, this.codigoTipoAux, posAux, ref this.ctaMayorTipo, ref ctaMayorDocumento);
                if (validarCuentaMayor != "")
                {
                    RadMessageBox.Show(validarCuentaMayor, error);
                    this.radButtonTextBoxSelCuentasMayor.Select();
                    return (false);
                }
                else
                {
                    if (descCtaMayor.Trim() != "") this.descCtaMayor = this.codigoCtaMayor + " " + this.separadorDesc + " " + descCtaMayor.Trim();
                    else this.descCtaMayor = this.codigoCtaMayor;

                    //Dependiendo del tipo de cuenta, revisar los campos Mostrar Cuentas y Documentos
                    if (this.ctaMayorTipo == "D")
                    {
                        this.radDropDownListMostrarCtas.SelectedIndex = 2;
                        this.radDropDownListMostrarCtas.Enabled = false;

                        if (this.ctaMayorDocumento != "S")
                        {
                            this.radDropDownListDocumentos.SelectedIndex = 2;
                            this.radDropDownListDocumentos.Enabled = false;
                        }
                    }
                    else
                    {
                        this.radDropDownListDocumentos.SelectedIndex = 2;
                        this.radDropDownListDocumentos.Enabled = false;
                    }
                }
            }

            //Grabar la petición
            string valores = this.ValoresPeticion();

            this.valoresFormulario.GrabarParametros(formCode, valores);

            return (result);
        }

        /// <summary>
        /// Carga los valores de la ultima petición del formulario
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCICONAUX myStruct = (StructGLL01_MCICONAUX)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCICONAUX));

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

                            this.radButtonTextBoxSelCuentaAux.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.cuentaAuxiliar.Trim() != "")
                    {
                        string codCtaAux = myStruct.cuentaAuxiliar.Trim();

                        string descCtaAuxiliar = "";

                        string resultValidarCtaAux = this.ValidarCuentaAuxiliar(codCtaAux, this.codigoTipoAux, ref descCtaAuxiliar);
                        if (resultValidarCtaAux != "")
                        {
                            this.codigoCtaAux = "";
                            this.radButtonTextBoxSelCuentaAux.Text = "";
                        }
                        else
                        {
                            this.codigoCtaAux = codCtaAux;
                            if (descCtaAuxiliar != "") codCtaAux += " " + this.separadorDesc + " " + descCtaAuxiliar;

                            this.radButtonTextBoxSelCuentaAux.Text = codCtaAux;

                            this.radButtonTextBoxSelCuentaAux.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                switch (myStruct.posicionAuxiliar.Trim())
                {
                    case "01":
                        this.radDropDownListPosAux.SelectedIndex = 0;
                        break;
                    case "02":
                        this.radDropDownListPosAux.SelectedIndex = 1;
                        break;
                    case "03":
                        this.radDropDownListPosAux.SelectedIndex = 2;
                        break;
                    default:
                        this.radDropDownListPosAux.SelectedIndex = 3;
                        break;
                }

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

                        this.radDropDownListGrupo.SelectedValue = "";
                        this.radDropDownListPlan.Enabled = false;

                        this.cmbCompania.Select();

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

                            this.radButtonTextBoxSelCuentasMayor.Enabled = true;

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

                        if (resultValidarPlan != "")
                        {
                            this.codigoPlan = "";
                            this.radDropDownListPlan.Text = "";
                        }
                        else
                        {
                            this.codigoPlan = codPlan;
                            if (descPlan != "") codPlan += " " + this.separadorDesc + " " + descPlan;

                            try
                            {
                                this.radDropDownListGrupo.SelectedValue = this.codigoPlan;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            this.radButtonTextBoxSelCuentasMayor.Enabled = true;
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.aappDesde.Trim() != "") this.txtMaskAAPPDesde.Text = myStruct.aappDesde.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    if (myStruct.aappHasta.Trim() != "") this.txtMaskAAPPHasta.Text = myStruct.aappHasta.Trim();
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try
                {
                    string documentosValor = myStruct.documentos.Trim();
                    if (myStruct.cuentaMayor.Trim() != "")
                    {
                        string codCtaMayor = myStruct.cuentaMayor.Trim();
                        string ctaMayorDesc = "";

                        int posAux = this.ObtenerPosicionAux();

                        string resultValidarCtaMayor = this.ValidarCuentaMayorTipoDocumento(codCtaMayor, this.codigoPlan, ref ctaMayorDesc, this.codigoTipoAux, posAux, ref this.ctaMayorTipo, ref ctaMayorDocumento);

                        if (resultValidarCtaMayor != "")
                        {
                            string error = this.LP.GetText("errValTitulo", "Error");
                            RadMessageBox.Show(resultValidarCtaMayor, error);
                            this.codigoCtaMayor = "";
                        }
                        else
                        {
                            this.codigoCtaMayor = codCtaMayor;
                            if (ctaMayorDesc != "") codCtaMayor += " " + this.separadorDesc + " " + ctaMayorDesc;

                            this.radButtonTextBoxSelCuentasMayor.Enabled = true;

                            this.CuentaMayorChangeMostrarCtaDocumentosRefresh();
                            
                            if (this.ctaMayorDocumento == "S")
                            {
                                this.radDropDownListDocumentos.Enabled = true;
                                if (documentosValor == "1" || documentosValor == "2" || documentosValor == "3")
                                    this.radDropDownListDocumentos.SelectedValue = documentosValor;
                                else this.radDropDownListDocumentos.SelectedValue = "3";
                            }
                        }

                        this.radButtonTextBoxSelCuentasMayor.Text = codCtaMayor;
                        this.radButtonTextBoxSelCuentasMayor.Select();
                    }
                    else
                    {
                        //Sin cuenta de mayor
                        string mostrarCtasValor = myStruct.mostrarCuentas.Trim();
                        if (mostrarCtasValor == "1" || mostrarCtasValor == "2" || mostrarCtasValor == "3")
                            this.radDropDownListMostrarCtas.SelectedValue = mostrarCtasValor;
                        else this.radDropDownListMostrarCtas.SelectedValue = "3";

                        if (documentosValor == "1" || documentosValor == "2" || documentosValor == "3")
                            this.radDropDownListDocumentos.SelectedValue = documentosValor;
                        else this.radDropDownListDocumentos.SelectedValue = "3";
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                if (myStruct.monedaExt == "1") this.chkMonedaExt.Checked = true;
                else this.chkMonedaExt.Checked = false;

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
                StructGLL01_MCICONAUX myStruct;

                myStruct.tipoAuxiliar = codigoTipoAux.PadRight(2, ' ');

                myStruct.cuentaAuxiliar = codigoCtaAux.PadRight(8, ' ');

                myStruct.posicionAuxiliar = "-1";
                if (this.radDropDownListPosAux.SelectedValue != null)
                {
                    myStruct.posicionAuxiliar = this.radDropDownListPosAux.SelectedValue.ToString();
                }

                myStruct.compania = codigoCompania.PadRight(2, ' ');

                myStruct.grupo = codigoGrupo.PadRight(2, ' ');

                myStruct.plan = codigoPlan.PadRight(2, ' ');

                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string aapp = this.txtMaskAAPPDesde.Value.ToString().Trim();
                this.txtMaskAAPPDesde.TextMaskFormat = MaskFormat.IncludeLiterals;
                myStruct.aappDesde = aapp.PadRight(4, ' ');

                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                aapp = this.txtMaskAAPPHasta.Value.ToString().Trim();
                this.txtMaskAAPPHasta.TextMaskFormat = MaskFormat.IncludeLiterals;
                myStruct.aappHasta = aapp.PadRight(4, ' ');

                myStruct.cuentaMayor = codigoCtaMayor.PadRight(15, ' ');

                myStruct.mostrarCuentas = "3";
                if (this.radDropDownListMostrarCtas.Enabled && this.radDropDownListMostrarCtas.SelectedValue != null)
                    myStruct.mostrarCuentas = this.radDropDownListMostrarCtas.SelectedValue.ToString();

                myStruct.documentos = "3";
                if (this.radDropDownListDocumentos.Enabled)
                {
                    if (this.radDropDownListDocumentos.SelectedValue != null)
                        myStruct.documentos = this.radDropDownListDocumentos.SelectedValue.ToString();
                }

                if (this.chkMonedaExt.Checked) myStruct.monedaExt = "1";
                else myStruct.monedaExt = "0";

                result = myStruct.tipoAuxiliar + myStruct.cuentaAuxiliar + myStruct.posicionAuxiliar + myStruct.compania;
                result += myStruct.grupo + myStruct.plan + myStruct.aappDesde + myStruct.aappHasta + myStruct.cuentaMayor;
                result += myStruct.mostrarCuentas + myStruct.documentos + myStruct.monedaExt;

                //int objsize = Marshal.SizeOf(typeof(StructGLL01_MCIMOVAUX));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Habilita o no los desplegables de mostrar cuentas y documentos en función del valor de la cuenta de mayor
        /// </summary>
        private void CuentaMayorChangeMostrarCtaDocumentosRefresh()
        {
            try
            {
                if (this.codigoCtaMayor != "" && this.ctaMayorTipo == "D")
                {
                    this.radDropDownListMostrarCtas.SelectedIndex = 2;
                    this.radDropDownListMostrarCtas.Enabled = false;

                    if (this.ctaMayorDocumento == "S")
                    {
                        this.radDropDownListDocumentos.Enabled = true;
                    }
                    else
                    {
                        this.radDropDownListDocumentos.SelectedIndex = 2;
                        this.radDropDownListDocumentos.Enabled = false;
                    }
                }
                else
                {
                    this.radDropDownListMostrarCtas.Enabled = true;
                    this.radDropDownListDocumentos.Enabled = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        #endregion
    }
}