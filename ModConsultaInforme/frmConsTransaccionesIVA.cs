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
    public partial class frmConsTransaccionesIVA : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCICONIVA";
        public string ficheroExtension = "civ";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCICONIVA
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string numIdTributaria;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string companiaFiscal;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string libro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string serie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string codIVA;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaContableDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaContableHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaDocumentoDesde;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string fechaDocumentoHasta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string numeroDoc;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string numeroFactAmpliado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string tipoTransaccion;
        }

        private string codigoCompaniaFiscal = "";
        private string descCiaFiscal = "";

        FormularioValoresCampos valoresFormulario;

        private System.Data.DataTable dtTipoTransaccion;

        public frmConsTransaccionesIVA()
        {
            InitializeComponent();

            this.dateTimeFechaContDesde.Format = DateTimePickerFormat.Custom;
            this.dateTimeFechaContDesde.CustomFormat = "dd/MM/yyyy";

            this.dateTimeFechaContHasta.Format = DateTimePickerFormat.Custom;
            this.dateTimeFechaContHasta.CustomFormat = "dd/MM/yyyy";

            this.dateTimeFechaDocDesde.Format = DateTimePickerFormat.Custom;
            this.dateTimeFechaDocDesde.CustomFormat = "dd/MM/yyyy";

            this.dateTimeFechaDocHasta.Format = DateTimePickerFormat.Custom;
            this.dateTimeFechaDocHasta.CustomFormat = "dd/MM/yyyy";

            this.dateTimeFechaContDesde.NullableValue = null;
            this.dateTimeFechaContDesde.SetToNullValue();

            this.dateTimeFechaContHasta.NullableValue = null;
            this.dateTimeFechaContHasta.SetToNullValue();

            this.dateTimeFechaDocDesde.NullableValue = null;
            this.dateTimeFechaDocDesde.SetToNullValue();

            this.dateTimeFechaDocHasta.NullableValue = null;
            this.dateTimeFechaDocHasta.SetToNullValue();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmConsTransaccionesIVA_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Consultas de Transacciones de IVA");

            this.TraducirLiterales();

            //Crear Tabla Tipo de Transaccion
            this.dtTipoTransaccion = new System.Data.DataTable();
            this.dtTipoTransaccion.Columns.Add("valor", typeof(string));
            this.dtTipoTransaccion.Columns.Add("desc", typeof(string));

            //Crear el desplegable con tipo de transaccion
            this.CrearComboTipoTransaccion();

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                this.CargarValoresUltimaPeticion(valores);
            }

            this.radButtonTextBoxSelNIT.Select();
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
        }

        private void RadButtonElementSelCiaFiscal_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CIAFT3, NOMBT3 from ";
            query += GlobalVar.PrefijoTablaCG + "IVT03 ";
            query += "order by CIAFT3, NOMBT3";
            
            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                "Código",
                "Descripción"
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar compañía fiscal",
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

            this.radButtonTextBoxSelCiaFiscal.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelCiaFiscal;
            this.radButtonTextBoxSelCiaFiscal.Select(0, 0);
            this.radButtonTextBoxSelCiaFiscal.Focus();
        }

        private void RadButtonTextBoxSelCiaFiscal_Leave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string codigo = this.radButtonTextBoxSelCiaFiscal.Text.Trim();
            if (codigo != "")
            {
                if (codigo != "" && codigo.Length >= 2)
                {
                    if (codigo.Length <= 1) this.codigoCompaniaFiscal = this.radButtonTextBoxSelCiaFiscal.Text;
                    else this.codigoCompaniaFiscal = this.radButtonTextBoxSelCiaFiscal.Text.Substring(0, 2);

                    string descCompFiscal = "";
                    string result = this.ValidarCompaniaFiscal(this.codigoCompaniaFiscal, ref descCompFiscal);

                    if (result != "")
                    {
                        string error = this.LP.GetText("errValTitulo", "Error");
                        RadMessageBox.Show(result, error);
                        //this.radButtonTextBoxSelCiaFiscal.Enabled = false;
                        this.radButtonTextBoxSelCiaFiscal.Focus();
                    }
                    else
                    {
                        string codCiaFiscal = this.codigoCompaniaFiscal;
                        if (descCompFiscal != "") codCiaFiscal += " " + this.separadorDesc + " " + descCompFiscal;

                        this.radButtonTextBoxSelCiaFiscal.Text = codCiaFiscal;
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonElementSelNIT_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select NNITMA, PCIFMA, NOMBMA, TAUXMA, CAUXMA from ";
            query += GlobalVar.PrefijoTablaCG + "GLM05 ";
            query += "where NNITMA <> ' ' ";
            string nitActual = this.radButtonTextBoxSelNIT.Text.Trim();
            if (nitActual != "") query += "and NNITMA LIKE '%" + nitActual + "%' ";
            query += "order by NNITMA, PCIFMA, TAUXMA, CAUXMA";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnas = new ArrayList
            {
                "Código",
                "País",
                "Descripción",
                "Tipo de Auxiliar",
                "Cuenta de Auxiliar"
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar NIT",
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
            string separadorCampos = "-";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                if (GlobalVar.ElementosSel.Count >= 3)
                {
                    result = GlobalVar.ElementosSel[0].ToString().Trim() + " " + separadorCampos + " " + GlobalVar.ElementosSel[2].ToString().Trim();
                }
            }

            this.radButtonTextBoxSelNIT.Text = result;
            this.ActiveControl = this.radButtonTextBoxSelNIT;
            this.radButtonTextBoxSelNIT.Select(0, 0);
            this.radButtonTextBoxSelNIT.Focus();
        }

        private void RadButtonTextBoxSelCiaFiscal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void RadTextBoxControlLibro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void RadTextBoxControlSerie_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void RadTextBoxControlCodIVA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void RadButtonTextBoxSelNIT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CGParametrosGrles.GLC01_MCIARC == "0") e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
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

        private void FrmConsTransaccionesIVA_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Consultas de Transacciones de IVA");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            this.radButtonEjecutar.Text = this.LP.GetText("lblEjecutar", "Ejecutar");   //Falta traducir
            this.radButtonGrabarPeticion.Text = this.LP.GetText("lblGrabarPeticion", "Grabar Petición");   //Falta traducir
            this.radButtonCargarPeticion.Text = this.LP.GetText("lblCargarPeticion", "Cargar Petición");   //Falta traducir
        }

        /// <summary>
        /// Crea el desplegable de tipo de transaccion
        /// </summary>
        private void CrearComboTipoTransaccion()
        {
            DataRow row;

            try
            {
                if (this.dtTipoTransaccion.Rows.Count > 0) this.dtTipoTransaccion.Rows.Clear();

                row = this.dtTipoTransaccion.NewRow();
                row["valor"] = " ";
                row["desc"] = "Todos";   //Falta traducir
                this.dtTipoTransaccion.Rows.Add(row);

                row = this.dtTipoTransaccion.NewRow();
                row["valor"] = "R";
                row["desc"] = "Repercutido";   //Falta traducir
                this.dtTipoTransaccion.Rows.Add(row);

                row = this.dtTipoTransaccion.NewRow();
                row["valor"] = "S";
                row["desc"] = "Soportado";   //Falta traducir
                this.dtTipoTransaccion.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListTipoTransaccion.DataSource = this.dtTipoTransaccion;
            this.radDropDownListTipoTransaccion.ValueMember = "valor";
            this.radDropDownListTipoTransaccion.DisplayMember = "desc";
            this.radDropDownListTipoTransaccion.Refresh();
            this.radDropDownListTipoTransaccion.SelectedIndex = 0;
        }
        
        /// <summary>
        /// Ejecutar la consulta
        /// </summary>
        private void Ejecutar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string nitSel = radButtonTextBoxSelNIT.Text.Trim();
                int posSeparador = nitSel.IndexOf('-');
                if (posSeparador != -1) nitSel = nitSel.Substring(0, posSeparador - 1);

                string nDoc = "";
                this.txtMaskNoDoc.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                nDoc = this.txtMaskNoDoc.Value.ToString().Trim();
                this.txtMaskNoDoc.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Llamar al formulario de visuallización
                frmConsTransIVAAuxView frmView = new frmConsTransIVAAuxView
                {
                    NumeroIdTributaria = nitSel,
                    CiaFiscalCodigo = this.codigoCompaniaFiscal,
                    CiaFiscalDesc = this.descCiaFiscal,
                    Libro = this.radTextBoxControlLibro.Text,
                    Serie = this.radTextBoxControlSerie.Text,
                    CodigoIVA = this.radTextBoxControlCodIVA.Text,
                    FechaContableDesde = this.dateTimeFechaContDesde.Text,
                    FechaContableHasta = this.dateTimeFechaContHasta.Text,
                    FechaDocumentoDesde = this.dateTimeFechaDocDesde.Text,
                    FechaDocumentoHasta = this.dateTimeFechaDocHasta.Text,
                    NumDocumento = nDoc,
                    NumFactAmpliada = this.radTextBoxControlNumFactAmpliada.Text,
                    TipoTransaccion = this.radDropDownListTipoTransaccion.SelectedValue.ToString(),
                    TipoTransaccionDesc = this.radDropDownListTipoTransaccion.Text
                };
                frmView.Show(this);

                //Grabar la petición
                string valores = this.ValoresPeticion();

                this.valoresFormulario.GrabarParametros(formCode, valores);
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
                        { "Número Id Tributaria", "radButtonTextBoxSelNIT" },
                        { "Compañia Fiscal", "radButtonTextBoxSelCiaFiscal" },
                        { "Libro", "radTextBoxControlLibro" },
                        { "Serie", "radTextBoxControlSerie" },
                        { "Código de IVA", "radTextBoxControlCodIVA" },
                        { "Fecha Contable Desde", "dateTimeFechaContDesde" },
                        { "Fecha Contable Hasta", "dateTimeFechaContHasta" },
                        { "Fecha Documento Desde", "dateTimeFechaDocDesde" },
                        { "Fecha Documento Hasta", "dateTimeFechaDocHasta" },
                        { "Número documento", "txtMaskNoDoc" }
                    };

                    List<string> columnNoVisible = new List<string>(new string[] { "radTextBoxControlNumFactAmpliada",
                                                                                   "radDropDownListTipoTransaccion" });

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
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;

            string error = this.LP.GetText("errValTitulo", "Error");

            if (this.radButtonTextBoxSelNIT.Text.Trim() == "")
            {
                RadMessageBox.Show("Es obligatorio informar número id. tributaria", error);
                this.radButtonTextBoxSelNIT.Select();
                return (false);
            }

            if (this.dateTimeFechaContDesde.Text == "" && this.dateTimeFechaContHasta.Text == "" &&
                this.dateTimeFechaDocDesde.Text == "" && this.dateTimeFechaDocHasta.Text == "")
            {
                RadMessageBox.Show("Es obligatorio informar la fecha contable o la fecha documento", error);
                if (this.dateTimeFechaContDesde.Text == "")
                {
                    this.dateTimeFechaContDesde.Select();
                    return (false);
                }
            }
            else
            {
                //Validar fecha contable (si están informadas fecha desde no puede ser mayor que fecha hasta)
                if (this.dateTimeFechaContDesde.Text != "" && this.dateTimeFechaContHasta.Text != "")
                {
                    try
                    {
                        if (dateTimeFechaContDesde.Value > dateTimeFechaContHasta.Value)
                        {
                            RadMessageBox.Show("La fecha contable desde no puede ser mayor que la fecha contable hasta", error);
                            this.dateTimeFechaContDesde.Select();
                            return (false);
                        }
                    }
                    catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                //Validar fecha documento (si están informadas fecha desde no puede ser mayor que fecha hasta)
                if (this.dateTimeFechaDocDesde.Text != "" && this.dateTimeFechaDocHasta.Text != "")
                {
                    try
                    {
                        if (dateTimeFechaDocDesde.Value > dateTimeFechaDocHasta.Value)
                        {
                            RadMessageBox.Show("La fecha documento desde no puede ser mayor que la fecha contable hasta", error);
                            this.dateTimeFechaDocDesde.Select();
                            return (false);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }

            this.codigoCompaniaFiscal = "";
            if (this.radButtonTextBoxSelCiaFiscal.Text.Length <= 2) this.codigoCompaniaFiscal = this.radButtonTextBoxSelCiaFiscal.Text;
            else this.codigoCompaniaFiscal = this.radButtonTextBoxSelCiaFiscal.Text.Substring(0, 2);

            if (this.codigoCompaniaFiscal != "")
            {
                string validarCiaFiscal = this.ValidarCompaniaFiscal(this.codigoCompaniaFiscal, ref this.descCiaFiscal);
                if (validarCiaFiscal != "")
                {
                    RadMessageBox.Show(validarCiaFiscal, error);
                    this.radButtonTextBoxSelCiaFiscal.Select();
                    return (false);
                }
            }

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
                StructGLL01_MCICONIVA myStruct = (StructGLL01_MCICONIVA)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCICONIVA));

                try
                {
                    if (myStruct.numIdTributaria.Trim() != "") this.radButtonTextBoxSelNIT.Text = myStruct.numIdTributaria.Trim();

                    if (myStruct.companiaFiscal.Trim() != "")
                    {
                        radButtonTextBoxSelCiaFiscal.Text = myStruct.companiaFiscal.Trim();
                    }

                    if (myStruct.libro.Trim() != "") this.radTextBoxControlLibro.Text = myStruct.libro;

                    if (myStruct.serie.Trim() != "") this.radTextBoxControlSerie.Text = myStruct.serie;

                    if (myStruct.codIVA.Trim() != "") this.radTextBoxControlCodIVA.Text = myStruct.codIVA;

                    if (myStruct.fechaContableDesde.Trim() != "")
                    {
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = Convert.ToDateTime(myStruct.fechaContableDesde);
                            this.dateTimeFechaContDesde.Value = fecha;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }

                    if (myStruct.fechaContableHasta.Trim() != "")
                    {
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = Convert.ToDateTime(myStruct.fechaContableHasta);
                            this.dateTimeFechaContHasta.Value = fecha;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }

                    if (myStruct.fechaDocumentoDesde.Trim() != "")
                    {
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = Convert.ToDateTime(myStruct.fechaDocumentoDesde);
                            this.dateTimeFechaDocDesde.Value = fecha;
                        }
                        catch (Exception ex) 
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }

                    if (myStruct.fechaDocumentoHasta.Trim() != "")
                    {
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = Convert.ToDateTime(myStruct.fechaDocumentoHasta);
                            this.dateTimeFechaDocHasta.Value = fecha;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }

                    if (myStruct.numeroDoc.Trim() != "") this.txtMaskNoDoc.Text = myStruct.numeroDoc;

                    if (myStruct.numeroFactAmpliado.Trim() != "") this.radTextBoxControlNumFactAmpliada.Text = myStruct.numeroFactAmpliado.Trim();

                    string tipoTransac = myStruct.tipoTransaccion.Trim();
                    if (tipoTransac == "R" || tipoTransac == "S") this.radDropDownListTipoTransaccion.SelectedValue = tipoTransac;
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
                StructGLL01_MCICONIVA myStruct;

                string nitSel = radButtonTextBoxSelNIT.Text.Trim();
                int posSeparador = nitSel.IndexOf('-');
                if (posSeparador != -1) nitSel = nitSel.Substring(0, posSeparador - 1);
                myStruct.numIdTributaria = nitSel.PadRight(13, ' ');

                myStruct.companiaFiscal = this.codigoCompaniaFiscal.PadRight(2, ' ');

                myStruct.libro = this.radTextBoxControlLibro.Text.PadRight(1, ' ');

                myStruct.serie = this.radTextBoxControlSerie.Text.PadRight(1, ' ');

                myStruct.codIVA = this.radTextBoxControlCodIVA.Text.PadRight(2, ' ');

                string aux = "";
                if (this.dateTimeFechaContDesde.Value == null) myStruct.fechaContableDesde = aux.PadRight(10, ' ');
                else myStruct.fechaContableDesde = this.dateTimeFechaContDesde.Value.ToShortDateString().PadRight(10, ' ');

                if (this.dateTimeFechaContHasta.Value == null) myStruct.fechaContableHasta = aux.PadRight(10, ' ');
                else myStruct.fechaContableHasta = this.dateTimeFechaContHasta.Value.ToShortDateString().PadRight(10, ' ');

                if (this.dateTimeFechaDocDesde.Value == null) myStruct.fechaDocumentoDesde = aux.PadRight(10, ' ');
                else myStruct.fechaDocumentoDesde = this.dateTimeFechaDocDesde.Value.ToShortDateString().PadRight(10, ' ');

                if (this.dateTimeFechaDocHasta.Value == null) myStruct.fechaDocumentoHasta = aux.PadRight(10, ' ');
                else myStruct.fechaDocumentoHasta = this.dateTimeFechaDocHasta.Value.ToShortDateString().PadRight(10, ' ');

                myStruct.numeroDoc = this.txtMaskNoDoc.Value.ToString().PadRight(10, ' ');

                myStruct.numeroFactAmpliado = this.radTextBoxControlNumFactAmpliada.Text.PadRight(25, ' ');

                myStruct.tipoTransaccion = this.radDropDownListTipoTransaccion.SelectedValue.ToString().PadRight(1, ' ');

                result = myStruct.numIdTributaria + myStruct.companiaFiscal + myStruct.libro + myStruct.serie;
                result += myStruct.codIVA + myStruct.fechaContableDesde + myStruct.fechaContableHasta;
                result += myStruct.fechaDocumentoDesde + myStruct.fechaDocumentoHasta;
                result += myStruct.numeroDoc + myStruct.numeroFactAmpliado + myStruct.tipoTransaccion;
                
                //int objsize = Marshal.SizeOf(typeof(StructGLL01_MCICONIVA));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion
    }
}