using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLM04 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOTIPAUX";

        private bool nuevo;
        private string codigo;

        private const string autClaseElemento = "006";
        private const string autGrupo = "01";
        private const string autOperConsulta = "";
        private const string autOperModifica = "30";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

        private string glm04_azmt = "";

        ArrayList nombreColumnasClaseZonas = new ArrayList();

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public frmMtoGLM04()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActivo.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActivo.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchDigitoAutoverif.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchDigitoAutoverif.ThemeName = "MaterialBlueGrey";

            this.radGroupBoxDirRequerida.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxDirRequerida.ThemeName = "ControlDefault";

            this.radGroupBoxFormatoMemo.ElementTree.EnableApplicationThemeName = false;
            this.radGroupBoxFormatoMemo.ThemeName = "ControlDefault";

            //((Telerik.WinControls.Primitives.FillPrimitive)this.radGroupBoxDirRequerida.GroupBoxElement.Content.Children[0]).BackColor = Color.Transparent;
            //this.radGroupBoxDirRequeridaOLD.GroupBoxElement.Header.ForeColor = Color.Transparent;
            this.radGroupBoxFormatoMemo.GroupBoxElement.Header.ForeColor = Color.Transparent;

            this.radButtonTextBoxClaseZona1.AutoSize = false;
            this.radButtonTextBoxClaseZona1.Size = new Size(this.radButtonTextBoxClaseZona1.Width, 30);

            Log = log4net.LogManager.GetLogger(this.GetType());
        }

        #region Eventosm
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLM04_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Tipos de Auxiliar Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();
            
            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);

                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonDelete, false);
                
                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información del tipos de auxiliar y mostrarla en los controles
                this.CargarInfoTipoAuxiliar();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtDesc;
                    this.txtDesc.Select(0, 0);
                    this.txtDesc.Focus();
                }

                bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                else utiles.ButtonEnabled(ref this.radButtonDelete, false);
            }
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codTipAux = this.txtCodigo.Text;

                if (codTipAux == "")
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Tipo de auxiliar obligatorio", this.LP.GetText("errValCodTipoAux", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                bool codTipoAuxOk = true;
                if (this.nuevo) codTipoAuxOk = this.CodigoTipoAuxValido();    //Verificar que el codigo no exista

                if (codTipoAuxOk)
                {
                    this.HabilitarDeshabilitarControles(true);
                    this.txtCodigo.IsReadOnly = true;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Tipo de auxiliar ya existe", this.LP.GetText("errValCodTipoAuxExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            //DoUpdateDataForm();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonDelete_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Eliminar
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonElementClaseZona1_Click(object sender, EventArgs e)
        {
            this.SeleccionarClaseZona(ref this.radButtonTextBoxClaseZona1, ref this.radButtonTextBoxClaseZona2);
        }

        private void RadButtonElementClaseZona2_Click(object sender, EventArgs e)
        {
            this.SeleccionarClaseZona(ref this.radButtonTextBoxClaseZona2, ref this.radButtonTextBoxClaseZona3);
        }

        private void RadButtonElementClaseZona3_Click(object sender, EventArgs e)
        {
            this.SeleccionarClaseZona(ref this.radButtonTextBoxClaseZona3, ref this.radButtonTextBoxClaseZona4);
        }

        private void RadButtonElementClaseZona4_Click(object sender, EventArgs e)
        {
            string result = "";
            this.SeleccionarClaseZona(ref this.radButtonTextBoxClaseZona4, ref result);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonDelete_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDelete);
        }

        private void RadButtonDelete_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDelete);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = true;
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmMtoGLM04_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLM04_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            
            try
            {
                if (this.txtCodigo.Text != this.txtCodigo.Tag.ToString() ||
                    this.radToggleSwitchEstadoActivo.Value != (bool)(this.radToggleSwitchEstadoActivo.Tag) ||
                    this.txtDesc.Text.Trim() != this.txtDesc.Tag.ToString().Trim() ||
                    (this.rbDirReqSi.IsChecked && !(Convert.ToBoolean(this.rbDirReqSi.Tag))) ||
                    (!this.rbDirReqSi.IsChecked && (Convert.ToBoolean(this.rbDirReqSi.Tag))) ||
                    (this.rbDirReqFormat.IsChecked && !(Convert.ToBoolean(this.rbDirReqFormat.Tag))) ||
                    (!this.rbDirReqFormat.IsChecked && (Convert.ToBoolean(this.rbDirReqFormat.Tag))) ||
                    (this.rbDirReqNo.IsChecked && !(Convert.ToBoolean(this.rbDirReqNo.Tag))) ||
                    (!this.rbDirReqNo.IsChecked && (Convert.ToBoolean(this.rbDirReqNo.Tag))) ||
                    this.radButtonTextBoxClaseZona1.Text != this.radButtonTextBoxClaseZona1.Tag.ToString() ||
                    this.radButtonTextBoxClaseZona2.Text != this.radButtonTextBoxClaseZona2.Tag.ToString() ||
                    this.radButtonTextBoxClaseZona3.Text != this.radButtonTextBoxClaseZona3.Tag.ToString() ||
                    this.radButtonTextBoxClaseZona4.Text != this.radButtonTextBoxClaseZona4.Tag.ToString() ||
                    (this.rbFormatoMemoNinguno.IsChecked && !(Convert.ToBoolean(this.rbFormatoMemoNinguno.Tag))) ||
                    (!this.rbFormatoMemoNinguno.IsChecked && (Convert.ToBoolean(this.rbFormatoMemoNinguno.Tag))) ||
                    (this.rbFormatoMemoTerceros.IsChecked && !(Convert.ToBoolean(this.rbFormatoMemoTerceros.Tag))) ||
                    (!this.rbFormatoMemoTerceros.IsChecked && (Convert.ToBoolean(this.rbFormatoMemoTerceros.Tag))) ||
                    (this.rbFormatoMemoBancos.IsChecked && !(Convert.ToBoolean(this.rbFormatoMemoBancos.Tag))) ||
                    (!this.rbFormatoMemoBancos.IsChecked && (Convert.ToBoolean(this.rbFormatoMemoBancos.Tag))) ||
                    this.radToggleSwitchDigitoAutoverif.Value != (bool)(this.radToggleSwitchDigitoAutoverif.Tag)
                )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonSave.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        cerrarForm = false;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            if (cerrarForm) Log.Info("FIN Mantenimiento de Tipos de Auxiliar Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir todos los campos !!!
            //Recuperar literales del formulario
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLM04TituloALta", "Mantenimiento de Tipos de Auxiliar - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLM04TituloEdit", "Mantenimiento de Tipos de Auxiliar - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonDelete.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");

            //Traducir los campos del formulario
            this.lblTipoAux.Text = this.LP.GetText("lblGLM04CodTipoAux", "Tipo de Auxiliar");
            //this.lblEstado.Text = this.LP.GetText("lblGLM04Estado", "Estado");
            this.lblEstado.Text = "Activo"; //Falta traducir
            this.lblDesc.Text = this.LP.GetText("lblGLM04Desc", "Descripción");
            this.lblDireccionRequerida.Text = this.LP.GetText("lblGLM04Dir", "Dirección requerida");
            this.rbDirReqSi.Text = this.LP.GetText("lblSi", "Sí");
            this.rbDirReqFormat.Text = this.LP.GetText("lblGLM04DirFormateada", "Formateada");
            this.rbDirReqNo.Text = this.LP.GetText("lblNo", "No");
            this.lblClasesZona.Text = this.LP.GetText("lblGLM04ClasesZona", "Clases de zona");
            this.lblFormatoMemo.Text = this.LP.GetText("lblGLM04ClasesZona", "Formato memo");
            this.rbFormatoMemoNinguno.Text = this.LP.GetText("lblGLM04MemoNinguno", "Ninguno");
            this.rbFormatoMemoTerceros.Text = this.LP.GetText("lblGLM04MemoTerceros", "Terceros");
            this.rbFormatoMemoBancos.Text = this.LP.GetText("lblGLM04MemoBancos", "Bancos");
            this.lblDigitoAutoVerif.Text = this.LP.GetText("lblGLM04ClasesZona", "Dígito autoverificación");

            //Columnas de los campos de tipo TGTextBoxSel
            nombreColumnasClaseZonas.Add(this.LP.GetText("lblListaCampoCodigo", "Código"));
            nombreColumnasClaseZonas.Add(this.LP.GetText("lblListaCampoDescripcion", "Descripción"));
            nombreColumnasClaseZonas.Add(this.LP.GetText("lblListaCampoEstructura", "Estructura"));     //Falta traducir
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo de la compahia (al dar de alta una compañía)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActivo.Enabled = valor;
            this.txtDesc.Enabled = valor;
            this.rbDirReqFormat.Enabled = valor;
            this.rbDirReqNo.Enabled = valor;
            this.rbDirReqSi.Enabled = valor;
            this.radButtonTextBoxClaseZona1.Enabled = valor;
            this.radButtonTextBoxClaseZona2.Enabled = valor;
            this.radButtonTextBoxClaseZona3.Enabled = valor;
            this.radButtonTextBoxClaseZona4.Enabled = valor;
            this.rbFormatoMemoBancos.Enabled = valor;
            this.rbFormatoMemoNinguno.Enabled = valor;
            this.rbFormatoMemoTerceros.Enabled = valor;
            this.radToggleSwitchDigitoAutoverif.Enabled = valor;
        }

        /// <summary>
        /// Rellena los controles con los datos del tipo de auxiliar (modo edición)
        /// </summary>
        private void CargarInfoTipoAuxiliar()
        {
            IDataReader dr = null;
            try
            {
                this.txtCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                query += "where TAUXMT = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    string estado = dr.GetValue(dr.GetOrdinal("STATMT")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActivo.Value = true;
                    else this.radToggleSwitchEstadoActivo.Value = false;

                    this.txtDesc.Text = dr.GetValue(dr.GetOrdinal("NOMBMT")).ToString().Trim();

                    string dirRequerida = dr.GetValue(dr.GetOrdinal("RQDAMT")).ToString().Trim();
                    switch (dirRequerida)
                    {
                        case "S":
                            this.rbDirReqSi.IsChecked = true;
                            this.rbDirReqFormat.IsChecked = false;
                            this.rbDirReqNo.IsChecked = false;
                            break;
                        case "F":
                            this.rbDirReqSi.IsChecked = false;
                            this.rbDirReqFormat.IsChecked = true;
                            this.rbDirReqNo.IsChecked = false;
                            break;
                        case "N":
                        default:
                            this.rbDirReqSi.IsChecked = false;
                            this.rbDirReqFormat.IsChecked = false;
                            this.rbDirReqNo.IsChecked = true;
                            break;
                    }

                    this.radButtonTextBoxClaseZona1.Text = dr.GetValue(dr.GetOrdinal("CLS1MT")).ToString().Trim();
                    this.radButtonTextBoxClaseZona2.Text = dr.GetValue(dr.GetOrdinal("CLS2MT")).ToString().Trim();
                    this.radButtonTextBoxClaseZona3.Text = dr.GetValue(dr.GetOrdinal("CLS3MT")).ToString().Trim();
                    this.radButtonTextBoxClaseZona4.Text = dr.GetValue(dr.GetOrdinal("CLS4MT")).ToString().Trim();

                    string formatoMemo = dr.GetValue(dr.GetOrdinal("FMEMMT")).ToString().Trim();
                    switch (formatoMemo)
                    {
                        case "1":
                            this.rbFormatoMemoNinguno.IsChecked = false;
                            this.rbFormatoMemoTerceros.IsChecked = true;
                            this.rbFormatoMemoBancos.IsChecked = false;
                            break;
                        case "2":
                            this.rbFormatoMemoNinguno.IsChecked = false;
                            this.rbFormatoMemoTerceros.IsChecked = false;
                            this.rbFormatoMemoBancos.IsChecked = true;
                            break;
                        case "0":
                        default:
                            this.rbFormatoMemoNinguno.IsChecked = true;
                            this.rbFormatoMemoTerceros.IsChecked = false;
                            this.rbFormatoMemoBancos.IsChecked = false;
                            break;
                    }

                    string digitoAutov = dr.GetValue(dr.GetOrdinal("DIGIMT")).ToString().Trim();
                    if (digitoAutov == "S") this.radToggleSwitchDigitoAutoverif.Value = true;
                    else this.radToggleSwitchDigitoAutoverif.Value = false;
                }

                dr.Close();

                //Actualizar los valores originales de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Valida que no exista el código del Tipo de Auxiliar
        /// </summary>
        /// <returns></returns>
        private bool CodigoTipoAuxValido()
        {
            bool result = false;

            try
            {
                string codTipoAux = this.txtCodigo.Text.Trim();

                if (codTipoAux != "")
                {
                    string query = "select count(TAUXMT) from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                    query += "where TAUXMT = '" + codTipoAux + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El tipo de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radToggleSwitchEstadoActivo.Enabled = false;
            this.txtDesc.Enabled = false;
            this.rbDirReqFormat.Enabled = false;
            this.rbDirReqNo.Enabled = false;
            this.rbDirReqSi.Enabled = false;
            this.radButtonTextBoxClaseZona1.Enabled = false;
            this.radButtonTextBoxClaseZona2.Enabled = false;
            this.radButtonTextBoxClaseZona3.Enabled = false;
            this.radButtonTextBoxClaseZona4.Enabled = false;
            this.rbFormatoMemoBancos.Enabled = false;
            this.rbFormatoMemoNinguno.Enabled = false;
            this.rbFormatoMemoTerceros.Enabled = false;
            this.radToggleSwitchDigitoAutoverif.Enabled = false;
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            try
            {
                if (this.txtDesc.Text.Trim() == "")
                {
                    errores += "- La descripción del tipo de auxiliar no puede estar en blanco \n\r";      //Falta traducir
                    this.txtDesc.Focus();
                }

                string resultValidarClasesZona = this.ValidarClasesZona();
                if (resultValidarClasesZona != "")
                {
                    errores += "- " + resultValidarClasesZona + "\n\r";
                }
         
                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Dar de alta a un tipo de auxiliar
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string desc = this.txtDesc.Text.Trim();

                string dirRequerida = "N";
                if (this.rbDirReqSi.IsChecked) dirRequerida = "S";
                else if (this.rbDirReqFormat.IsChecked) dirRequerida = "F";

                string claseZona1 = this.radButtonTextBoxClaseZona1.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona1.Text;
                string claseZona2 = this.radButtonTextBoxClaseZona2.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona2.Text;
                string claseZona3 = this.radButtonTextBoxClaseZona3.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona3.Text;
                string claseZona4 = this.radButtonTextBoxClaseZona4.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona4.Text;

                string formatoMemo = "0";
                if (this.rbFormatoMemoTerceros.IsChecked) formatoMemo = "1";
                else if (this.rbFormatoMemoBancos.IsChecked) formatoMemo = "2";

                string digitoAutoVerif = this.radToggleSwitchDigitoAutoverif.Value == true ? "S" : "N";

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLM04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATMT, TAUXMT, NOMBMT, DIGIMT, AZMT, RQDAMT, FMEMMT, CLS1MT, CLS2MT, CLS3MT, CLS4MT) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigo + "', '" + this.txtDesc.Text + "', '" + digitoAutoVerif;
                query += "', " + this.glm04_azmt + ", '" + dirRequerida + "', " + formatoMemo + ", '";
                query += claseZona1 + "', '" + claseZona2 + "', '" + claseZona3 + "', '";
                query += claseZona4 + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLM04", this.codigo, null);

                //Insertarlo como propietario del elemento
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + this.codigo + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar un tipo de auxiliar
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string desc = this.txtDesc.Text.Trim();

                string dirRequerida = "N";
                if (this.rbDirReqSi.IsChecked) dirRequerida = "S";
                else if (this.rbDirReqFormat.IsChecked) dirRequerida = "F";

                string claseZona1 = this.radButtonTextBoxClaseZona1.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona1.Text;
                string claseZona2 = this.radButtonTextBoxClaseZona2.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona2.Text;
                string claseZona3 = this.radButtonTextBoxClaseZona3.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona3.Text;
                string claseZona4 = this.radButtonTextBoxClaseZona4.Text.Trim() == "" ? " " : this.radButtonTextBoxClaseZona4.Text;

                string formatoMemo = "0";
                if (this.rbFormatoMemoTerceros.IsChecked) formatoMemo = "1";
                else if (this.rbFormatoMemoBancos.IsChecked) formatoMemo = "2";

                string digitoAutoVerif = this.radToggleSwitchDigitoAutoverif.Value == true ? "S" : "N";

                string estado = (this.radToggleSwitchEstadoActivo.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLM04 set ";
                query += "STATMT = '" + estado + "', NOMBMT = '" + this.txtDesc.Text + "', ";
                query += "DIGIMT = '" + digitoAutoVerif + "', AZMT = " + this.glm04_azmt + ", RQDAMT = '" + dirRequerida + "', ";
                query += "FMEMMT = " + formatoMemo + ", CLS1MT = '" + claseZona1 + "', CLS2MT = '" + claseZona2 + "', ";
                query += "CLS3MT = '" + claseZona3 + "', CLS4MT = '" + claseZona4 + "' ";
                query += "where TAUXMT = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLM04", this.codigo, null);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Valida que las clases de zonas sean permitidas
        /// </summary>
        private string ValidarClasesZona()
        {
            string result = "";
            try
            {
                string claseZona1 = this.radButtonTextBoxClaseZona1.Text.Trim();
                string claseZona2 = this.radButtonTextBoxClaseZona2.Text.Trim();
                string claseZona3 = this.radButtonTextBoxClaseZona3.Text.Trim();
                string claseZona4 = this.radButtonTextBoxClaseZona4.Text.Trim();

                //Al menos una zona informada para hacer las validaciones
                if (claseZona1 != "" || claseZona2 != "" || claseZona3 != "" || claseZona4 != "")
                {
                    //Verificar que no existan huecos, zonas informadas no consecutivas
                    if ((claseZona1 == "" && (claseZona2 != "" || claseZona3 != "" || claseZona4 != "")) ||
                        (claseZona2 == "" && (claseZona3 != "" || claseZona4 != "")) ||
                        (claseZona3 == "" && (claseZona4 != ""))
                        )
                    {
                        result = "El valor de la clase de zona no puede estar en blanco";   //Falta traducir
                        if (claseZona1 == "") this.radButtonTextBoxClaseZona1.Focus();
                        if (claseZona2 == "") this.radButtonTextBoxClaseZona2.Focus();
                        if (claseZona3 == "") this.radButtonTextBoxClaseZona3.Focus();
                        return (result);
                    }

                    //Verificar que los valores de las clases de zonas existan
                    //----------- Clase Zona 1 --------------
                    bool validaClaseZona1 = (claseZona1 == "") ? true : ValidarClaseZona(claseZona1);
                    if (!validaClaseZona1)
                    {
                        result = "El valor de la clase de zona no es válido";   //Falta traducir
                        this.radButtonTextBoxClaseZona1.Focus();
                        return (result);
                    }
                    //----------- Clase Zona 2 --------------
                    bool validaClaseZona2 = (claseZona2 == "") ? true : ValidarClaseZona(claseZona2);
                    if (!validaClaseZona2)
                    {
                        result = "El valor de la clase de zona no es válido";   //Falta traducir
                        this.radButtonTextBoxClaseZona2.Focus();
                        return (result);
                    }
                    //----------- Clase Zona 3 --------------
                    bool validaClaseZona3 = (claseZona3 == "") ? true : ValidarClaseZona(claseZona3);
                    if (!validaClaseZona3)
                    {
                        result = "El valor de la clase de zona no es válido";   //Falta traducir
                        this.radButtonTextBoxClaseZona3.Focus();
                        return (result);
                    }
                    //----------- Clase Zona 4 --------------
                    bool validaClaseZona4 = (claseZona4 == "") ? true : ValidarClaseZona(claseZona4);
                    if (!validaClaseZona4)
                    {
                        result = "El valor de la clase de zona no es válido";   //Falta traducir
                        this.radButtonTextBoxClaseZona4.Focus();
                        return (result);
                    }

                    //Verificar si alguna de las clases de zonas se repite
                    if ((claseZona1 == claseZona2 && claseZona2 != "") || (claseZona1 == claseZona3 && claseZona3 != "") ||
                        (claseZona1 == claseZona4 && claseZona4 != "") ||
                        (claseZona2 == claseZona3 && claseZona3 != "") || (claseZona2 == claseZona4 && claseZona4 != "") ||
                        (claseZona3 == claseZona4 && claseZona4 != ""))
                    {
                        result = "Código de clase de zona duplicado";   //Falta traducir
                        this.radButtonTextBoxClaseZona1.Focus();
                        return (result);
                    }

                    if (claseZona2 != "")
                    {
                        //Verificar si la primera clase de zona es jerárquica, no pueden haber más informadas
                        bool zonaJerarquica = utilesCG.isClaseZonaJerarquica(claseZona1);
                        if (zonaJerarquica)
                        {
                            if (claseZona2 != "" || claseZona3 != "" || claseZona4 != "")
                            {
                                result = "Si la primera clase de zona es jerárquica, no se admiten más";   //Falta traducir
                                if (claseZona2 != "") this.radButtonTextBoxClaseZona2.Focus();
                                if (claseZona3 != "") this.radButtonTextBoxClaseZona3.Focus();
                                if (claseZona4 != "") this.radButtonTextBoxClaseZona4.Focus();
                                return (result);
                            }
                        }

                        //Si alguna clase de zona es jerarquicas, sólo puede estar informada en la 1ra posición
                        zonaJerarquica = utilesCG.isClaseZonaJerarquica(claseZona2);
                        if (zonaJerarquica)
                        {
                            result = "Clase de zona no puede ser jerárquica";   //Falta traducir
                            this.radButtonTextBoxClaseZona2.Focus();
                            return (result);
                        }

                        if (claseZona3 != "")
                        {
                            //Si alguna clase de zona es jerarquicas, sólo puede estar informada en la 1ra posición
                            zonaJerarquica = utilesCG.isClaseZonaJerarquica(claseZona3);
                            if (zonaJerarquica)
                            {
                                result = "Clase de zona no puede ser jerárquica";   //Falta traducir
                                this.radButtonTextBoxClaseZona3.Focus();
                                return (result);
                            }

                            if (claseZona4 != "")
                            {
                                //Si alguna clase de zona es jerarquicas, sólo puede estar informada en la 1ra posición
                                zonaJerarquica = utilesCG.isClaseZonaJerarquica(claseZona4);
                                if (zonaJerarquica)
                                {
                                    result = "Clase de zona no puede ser jerárquica";   //Falta traducir
                                    this.radButtonTextBoxClaseZona4.Focus();
                                    return (result);
                                }
                            }
                        }
                    }

                    //Verificar que la estructura no sume más de 8
                    int sumaEstructura = this.SumaEstructura();
                    if (sumaEstructura > 8)
                    {
                        result = "La estructura no puede sumar más de 8";   //Falta traducir
                        this.radButtonTextBoxClaseZona1.Focus();
                        return (result);
                    }
                }
                else this.glm04_azmt = "0";     //Si no hay clases de zona informada, el campo AZMT de la tabla GLM04 se grabará con valor 0
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error validando las clases de zona (" + ex.Message + ")";     //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Valida si la clase de zona existe en la tabla GLM10
        /// </summary>
        /// <param name="claseZona"></param>
        /// <returns></returns>
        private bool ValidarClaseZona(string claseZona)
        {
            bool claseZonaValida = false;

            IDataReader dr = null;
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + claseZona + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    claseZonaValida = true;
                }

                dr.Close();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (claseZonaValida);
        }

        /// <summary>
        /// Devuelve la suma de la estructura de las clases de zona que seleccionó el usuario
        /// </summary>
        /// <returns></returns>
        private int SumaEstructura()
        {
            int resSuma = 0;

            this.glm04_azmt = "";

            int[] azmt = new int[4] { 0, 0, 0, 0 };

            IDataReader dr = null;
            try
            {
                string valorClaseZona1 = this.radButtonTextBoxClaseZona1.Text.Trim();
                string valorClaseZona2 = this.radButtonTextBoxClaseZona2.Text.Trim();
                string valorClaseZona3 = this.radButtonTextBoxClaseZona3.Text.Trim();
                string valorClaseZona4 = this.radButtonTextBoxClaseZona4.Text.Trim();

                string query = "select CLASZ0, ESTRZ0 from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + valorClaseZona1 + "'";

                string queryFiltro = "";

                if (valorClaseZona2 != "") queryFiltro += " or CLASZ0 = '" + valorClaseZona2 + "'";
                if (valorClaseZona3 != "") queryFiltro += " or CLASZ0 = '" + valorClaseZona3 + "'";
                if (valorClaseZona4 != "") queryFiltro += " or CLASZ0 = '" + valorClaseZona4 + "'";

                query += queryFiltro;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string clasz0 = "";
                string estrz0 = "";
                int valorActual = 0;
                while (dr.Read())
                {
                    clasz0 = dr.GetValue(dr.GetOrdinal("CLASZ0")).ToString().Trim();
                    estrz0 = dr.GetValue(dr.GetOrdinal("ESTRZ0")).ToString().Trim();

                    if (estrz0 != "")
                    {
                        if (queryFiltro == "") this.glm04_azmt = estrz0;
                        else
                        {
                            try
                            {
                                valorActual = Convert.ToInt16(estrz0.Substring(0, 1));

                                resSuma += valorActual;

                                if (clasz0 == valorClaseZona1) azmt[0] = valorActual;
                                else if (clasz0 == valorClaseZona2) azmt[1] = valorActual;
                                else if (clasz0 == valorClaseZona3) azmt[2] = valorActual;
                                else if (clasz0 == valorClaseZona4) azmt[3] = valorActual;
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }
                }

                dr.Close();

                if (this.glm04_azmt == "") this.glm04_azmt = azmt[0].ToString() + azmt[1].ToString() + azmt[2].ToString() + azmt[3].ToString();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (resSuma);
        }

        /// <summary>
        /// Grabar un tipo de auxiliar
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    this.codigo = this.txtCodigo.Text.Trim();
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;

                        bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonDelete, true);
                        else utiles.ButtonEnabled(ref this.radButtonDelete, false);
                    }
                }
                else result = this.ActualizarInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Cerrar el formulario
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar un tipo de auxiliar
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar el Tipo de auxiliar " + this.codigo.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en las cuentas de auxiliar
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en las cuentas de auxiliar
                        mensaje = "No es posible eliminar el Tipo de auxiliar porque está en uso en cuentas de auxiliar.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        return;
                    }

                    //Buscar si existen entradas en el maestro de cuentas de mayor
                    query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM03 ";
                    query += "where TAU1MC = '" + this.codigo + "' or TAU2MC = '" + this.codigo + "' or TAU3MC = '" + this.codigo + "'";

                    cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en las cuentas de mayor
                        mensaje = "No es posible eliminar el Tipo de auxiliar porque está en uso en cuentas de mayor.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el tipo de auxiliar
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                        query += "where TAUXMT = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar el Tipo de auxiliar.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Eliminarlo de las tablas de autorizaciones
                            try
                            {
                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                                query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + this.codigo + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM08 ";
                                query += "where CLELAG = '" + autClaseElemento + "' and ELEMAG = '" + this.codigo + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                            //Cerrar el formulario
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else return;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtCodigo.Tag = this.txtCodigo.Text;
            this.radToggleSwitchEstadoActivo.Tag = this.radToggleSwitchEstadoActivo.Value;
            this.txtDesc.Tag = this.txtDesc.Text;

            if (this.rbDirReqSi.IsChecked) this.rbDirReqSi.Tag = true;
            else this.rbDirReqSi.Tag = false;
            if (this.rbDirReqFormat.IsChecked) this.rbDirReqFormat.Tag = true;
            else this.rbDirReqFormat.Tag = false;
            if (this.rbDirReqNo.IsChecked) this.rbDirReqNo.Tag = true;
            else this.rbDirReqNo.Tag = false;

            this.radButtonTextBoxClaseZona1.Tag = this.radButtonTextBoxClaseZona1.Text;
            this.radButtonTextBoxClaseZona2.Tag = this.radButtonTextBoxClaseZona2.Text;
            this.radButtonTextBoxClaseZona3.Tag = this.radButtonTextBoxClaseZona3.Text;
            this.radButtonTextBoxClaseZona4.Tag = this.radButtonTextBoxClaseZona4.Text;

            if (this.rbFormatoMemoNinguno.IsChecked) this.rbFormatoMemoNinguno.Tag = true;
            else this.rbFormatoMemoNinguno.Tag = false;
            if (this.rbFormatoMemoTerceros.IsChecked) this.rbFormatoMemoTerceros.Tag = true;
            else this.rbFormatoMemoTerceros.Tag = false;
            if (this.rbFormatoMemoBancos.IsChecked) this.rbFormatoMemoBancos.Tag = true;
            else this.rbFormatoMemoBancos.Tag = false;

            if (this.radToggleSwitchDigitoAutoverif.Value) this.radToggleSwitchDigitoAutoverif.Tag = true;
            else this.radToggleSwitchDigitoAutoverif.Tag = false;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActivo.Tag = true;
            this.txtDesc.Tag = "";

            this.rbDirReqSi.Tag = false;
            this.rbDirReqFormat.Tag = false;
            this.rbDirReqNo.Tag = true;
            
            this.radButtonTextBoxClaseZona1.Tag = "";
            this.radButtonTextBoxClaseZona2.Tag = "";
            this.radButtonTextBoxClaseZona3.Tag = "";
            this.radButtonTextBoxClaseZona4.Tag = "";

            this.rbFormatoMemoNinguno.Tag = true;
            this.rbFormatoMemoTerceros.Tag = false;
            this.rbFormatoMemoBancos.Tag = false;

            //this.radToggleSwitchDigitoAutoverif.Tag = false;
            this.radToggleSwitchDigitoAutoverif.Tag = true;
        }

        /// <summary>
        /// Seleccionar clase de zona y avanzar hacia la siguiente
        /// </summary>
        /// <param name="radButtonTextBoxClaseZona"></param>
        /// <param name="radButtonTextBoxClaseZonaSiguiente"></param>
        private void SeleccionarClaseZona(ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxClaseZona,
                                          ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxClaseZonaSiguiente)
        {
            string result = "";
            this.SeleccionarClaseZona(ref radButtonTextBoxClaseZona, ref result);

            //Ir hacia la clase de zona siguiente
            if (result != "" && radButtonTextBoxClaseZonaSiguiente != null)
            {
                this.ActiveControl = radButtonTextBoxClaseZonaSiguiente;
                radButtonTextBoxClaseZonaSiguiente.Select();
                radButtonTextBoxClaseZonaSiguiente.Focus();
            }
        }

        /// <summary>
        /// Seleccionar clase de zona
        /// </summary>
        /// <param name="radButtonTextBoxClaseZona"></param>
        /// <param name="result"></param>
        private void SeleccionarClaseZona(ref Telerik.WinControls.UI.RadButtonTextBox radButtonTextBoxClaseZona, ref string result)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select CLASZ0, NOMBZ0, ESTRZ0 from ";
            query += GlobalVar.PrefijoTablaCG + "GLM10 ";
            query += "where STATZ0='V' ";
            query += "order by CLASZ0";

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar clase de zona",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasClaseZonas,
                //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
                FrmPadre = this
            };

            frmElementosSel.ShowDialog();

            result = "";
            Telerik.WinControls.UI.RadButtonTextBox claseZonaSiguiente = new Telerik.WinControls.UI.RadButtonTextBox();
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                result = GlobalVar.ElementosSel[0].ToString().Trim();
                radButtonTextBoxClaseZona.Text = result;
            }
        }
        #endregion

        private void frmMtoGLM04_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }
    }
}
