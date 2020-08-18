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
    public partial class frmMtoGLT22 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOGRCTMA";

        private bool nuevo;
        private bool editarPlan;
        private string codigo;
        private string codigoPlan;

        private bool grabarClose;
        private string codigoDescGrupoMayor;

        private const string autClaseElemento = "008";
        private const string autGrupo = "01";
        private const string autOperConsulta = "";
        private const string autOperModifica = "30";
        private bool autEditar = false;

        public Boolean bCancelar = false;
        public Boolean bTabulador = false;

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

        public string CodigoPlan
        {
            get
            {
                return (this.codigoPlan);
            }
            set
            {
                this.codigoPlan = value;
            }
        }

        public bool EditarPlan
        {
            get
            {
                return (this.editarPlan);
            }
            set
            {
                this.editarPlan = value;
            }
        }

        public bool GrabarClose
        {
            get
            {
                return (this.grabarClose);
            }
            set
            {
                this.grabarClose = value;
            }
        }

        public string CodigoDesGrupoMayor
        {
            get
            {
                return (this.codigoDescGrupoMayor);
            }
            set
            {
                this.codigoDescGrupoMayor = value;
            }
        }

        public frmMtoGLT22()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            this.radButtonTextBoxPlanCuentas.AutoSize = false;
            this.radButtonTextBoxPlanCuentas.Size = new Size(this.radButtonTextBoxPlanCuentas.Width, 30);

            //------------ Inicializar atributos del formulario --------------

            //Si el campo Plan puede ser editado
            //Se utiliza para el alta. Cuando se llama dese el listado de grupos de cuentas de mayor es de edición, pero cuando 
            //se accede desde el mantenimiento de cuentas de mayor  para dar de alta a un grupo de cuentas de mayor es de sólo lectura
            this.editarPlan = true;

            //Si al darle de alta al grupo de cuentas de mayor se cierra el formulario (para cuando se llama desde
            //el formulario de mantenimiento de cuentas de mayor)
            this.grabarClose = false;

            //Devuelve el codigo - descripcion del grupo de cuentas de mayor creado (para cuando se llama desde
            //el formulario de mantenimiento de cuentas de mayor)
            this.codigoDescGrupoMayor = "";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLT22_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Grupos de Cuentas de Mayor Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            if (this.codigoPlan != "")
            {
                string planDesc = utilesCG.ObtenerDescDadoCodigo("GLM02", "TIPLMP", "NOMBMP", this.codigoPlan, false, "").Trim();
                if (planDesc != "")
                {
                    planDesc = this.codigoPlan + " " + separadorDesc + " " + planDesc;
                    this.radButtonTextBoxPlanCuentas.Text = planDesc;
                }
                else this.radButtonTextBoxPlanCuentas.Text = this.codigoPlan;
            }

            if (this.nuevo)
            {
                this.autEditar = true;
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.HabilitarDeshabilitarControles(false);
                utiles.ButtonEnabled(ref this.radButtonSave, false);
                utiles.ButtonEnabled(ref this.radButtonEliminar, false);

                // Actualiza el atributo TAG de los controles al valor inicial
                this.ActualizaValoresOrigenTAGControles();

                if (!this.editarPlan)
                {
                    this.radButtonTextBoxPlanCuentas.ReadOnly = true;
                    this.radButtonElementPlanCuentas.Enabled = false;
                    this.ActiveControl = this.txtCodigo;
                    this.txtCodigo.Select(0, 0);
                    this.txtCodigo.Focus();
                }
                else
                {
                    this.ActiveControl = this.radButtonTextBoxPlanCuentas;
                }
            }
            else
            {
                this.radButtonTextBoxPlanCuentas.ReadOnly = true;
                this.radButtonElementPlanCuentas.Enabled = false;
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información del grupo de cuentas de mayor
                this.CargarInfoGrupoCuentaMayor();

                bool operarConsulta = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperConsulta);
                bool operarModificar = aut.Validar(autClaseElemento, autGrupo, this.codigo, autOperModifica);
                this.autEditar = operarModificar;
                if (operarConsulta && !operarModificar) this.NoEditarCampos();
                else
                {
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select(0, 0);
                    this.txtNombre.Focus();
                }

                bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                else utiles.ButtonEnabled(ref this.radButtonEliminar, false);
           }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;

            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (this.autEditar) this.HabilitarDeshabilitarControles(true);
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            if (this.autEditar)
            {
                string codGrupoCtas = this.txtCodigo.Text.Trim();
                if (codGrupoCtas == "")
                {
                    RadMessageBox.Show("Debe indicar el código de grupo de cuentas", this.LP.GetText("errValCodGrupoCtas", "Error"));  //Falta traducir
                    bTabulador = false;

                    utiles.ButtonEnabled(ref this.radButtonSave, false);
                    this.txtCodigo.Focus();
                    return;
                }

                if (this.radButtonTextBoxPlanCuentas.Text.Trim() == "" && codGrupoCtas == "")
                {
                    RadMessageBox.Show("Debe indicar el plan y el código de grupo de cuentas de mayor", this.LP.GetText("errPlanGrupoCtasMayorObligatorio", "Error"));  //Falta traducir
                    this.radButtonTextBoxPlanCuentas.Select();
                    bTabulador = false;
                    return;
                }

                if (this.radButtonTextBoxPlanCuentas.Text.Trim() != "" && codGrupoCtas == "")
                {
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Código de grupo de cuentas no válido", this.LP.GetText("errValCodGrupoCtas", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                if (codGrupoCtas != "" && this.radButtonTextBoxPlanCuentas.Text.Trim() == "")
                {
                    RadMessageBox.Show("Debe indicar el código del plan", this.LP.GetText("errPlanObligatorio", "Error"));  //Falta traducir
                    this.radButtonTextBoxPlanCuentas.Select();
                    bTabulador = false;
                    return;
                }

                if (this.codigoPlan == "")
                {
                    this.codigoPlan = this.radButtonTextBoxPlanCuentas.Text.Trim();
                    int posSeparador = this.codigoPlan.IndexOf(separadorDesc);
                    if (posSeparador != -1) this.codigoPlan = this.codigoPlan.Substring(0, posSeparador - 1).Trim();
                }

                bool codGrupoCtasOk = true;
                if (this.nuevo) codGrupoCtasOk = this.CodigoGrupoCtasValido();    //Verificar que el codigo no exista

                if (codGrupoCtasOk)
                {
                    this.HabilitarDeshabilitarControles(true);

                    if (this.nuevo)
                    {
                        this.radToggleSwitchEstadoActiva.Value = true;
                        //this.radToggleSwitchEstadoActiva.Enabled = false;
                        this.radToggleSwitchEstadoActiva.Enabled = true;
                    }

                    this.radButtonTextBoxPlanCuentas.ReadOnly = true;
                    this.radButtonElementPlanCuentas.Enabled = false;
                    //this.txtCodigo.IsReadOnly = true;

                    this.codigo = codGrupoCtas;

                    utiles.ButtonEnabled(ref this.radButtonSave, true);
                }
                else
                {
                    this.HabilitarDeshabilitarControles(false);
                    this.txtCodigo.Focus();
                    RadMessageBox.Show("Código de grupo de cuentas ya existe", this.LP.GetText("errValCodGrupoCtasExiste", "Error"));  //Falta traducir
                    bTabulador = false;
                }
            }
            bTabulador = false;
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

        private void RadButtonEliminar_Click(object sender, EventArgs e)
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

        private void RadButtonElementPlanCuentas_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TIPLMP, NOMBMP from ";
            query += GlobalVar.PrefijoTablaCG + "GLM02 ";
            query += "order by NOMBMP";

            //Definir la cabecera de las columnas
            //Columnas de los campos de tipo TGTextBoxSel
            ArrayList nombreColumnasPlan = new ArrayList
            {
                this.LP.GetText("lblListaCampoCodigo", "Código"),
                this.LP.GetText("lblListaCampoDescripcion", "Descripción")
            };

            //Crea el formulario de selección
            TGElementosSel frmElementosSel = new TGElementosSel
            {
                //Título del Formulario de Selección de Elementos
                TituloForm = "Seleccionar planes de cuentas",
                //Coordenadas donde se dibujará el Formulario de Selección de Elementos
                //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
                LocationForm = new Point(0, 0),
                //Si se centrar el Formulario o no
                CentrarForm = true,
                //Pasar la conexión a la bbdd
                ProveedorDatosForm = GlobalVar.ConexionCG,
                Query = query,
                ColumnasCaption = nombreColumnasPlan,
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
                this.radButtonTextBoxPlanCuentas.Text = result;
                this.ActiveControl = this.radButtonTextBoxPlanCuentas;
                this.radButtonTextBoxPlanCuentas.Select(0, 0);
                this.radButtonTextBoxPlanCuentas.Focus();
            }
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void RadButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
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

        private void FrmMtoGLT22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLT22_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;
            if (radButtonSave.Enabled == true)
            {
                try
                {
                    if (this.txtCodigo.Text.Trim() != this.txtCodigo.Tag.ToString().Trim() ||
                        this.radToggleSwitchEstadoActiva.Value != (bool)(this.radToggleSwitchEstadoActiva.Tag) ||
                        this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim()
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

                if (cerrarForm) Log.Info("FIN Mantenimiento de Grupos de Cuentas de Mayor Alta/Edita");
            }

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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLT22TituloALta", "Mantenimiento de Grupos de Cuentas de Mayor - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT22TituloEdit", "Mantenimiento de Grupos de Cuentas  de Mayor - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
        }
        
        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo del grupo de cuentas de mayor (al dar de alta a un grupo)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El grupo de cuentas de mayor está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radButtonTextBoxPlanCuentas.Enabled = false;
            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNombre.IsReadOnly = true;
        }

        /// <summary>
        /// Rellena los controles con los datos del grupo de cuentas de mayor (modo edición)
        /// </summary>
        private void CargarInfoGrupoCuentaMayor()
        {
            IDataReader dr = null;
            try
            {
                string estado = "";
                this.txtCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT22 ";
                query += "where TIPLGC = '" + this.codigoPlan + "' and GRCTGC = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBGC")).ToString().Trim();

                    estado = dr.GetValue(dr.GetOrdinal("STATGC")).ToString().Trim();
                    if (estado == "V") this.radToggleSwitchEstadoActiva.Value = true;
                    else this.radToggleSwitchEstadoActiva.Value = false;
                }

                dr.Close();

                // Actualiza el atributo TAG de los controles al valor actual de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Valida que no exista el código de grupo de cuentas
        /// </summary>
        /// <returns></returns>
        private bool CodigoGrupoCtasValido()
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                string codGrupoCtas = this.txtCodigo.Text.Trim();

                if (codGrupoCtas != "")
                {
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT22 ";
                    query += "where TIPLGC = '" + this.codigoPlan + "' and GRCTGC = '" + codGrupoCtas + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        result = false;
                    }
                    else result = true;


                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
            
            return (result);
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
                //-------------- Validar el nombre del grupo de cuentas de mayor  ------------------
                if (this.txtNombre.Text.Trim() == "")
                {
                    errores += "El nombre no puede estar en blanco \n\r";      //Falta traducir
                    this.ActiveControl = this.txtNombre;
                    this.txtNombre.Select();
                    this.txtNombre.Focus();
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
        /// Dar de alta a un grupo de cuentas de mayor
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                //Dar de alta al grupo de cuentas (GLT22)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT22";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATGC, TIPLGC, GRCTGC, NOMBGC) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigoPlan + "', '" + this.codigo + "', '";
                query += this.txtNombre.Text + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLT22", this.codigoPlan, this.codigo);

                //Insertar al usuario como propietario del elemento
                string elemento = this.codigoPlan + this.codigo;
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + elemento + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar un grupo de cuentas de mayor
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT22 set ";
                query += "STATGC = '" + estado + "', NOMBGC = '" + this.txtNombre.Text + "' ";
                query += "where TIPLGC = '" + this.codigoPlan + "' and GRCTGC = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT22", this.codigoPlan, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
        
        /// <summary>
        /// Graba un grupo de cuentas de mayor
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = false;
                        this.codigo = this.txtCodigo.Text.Trim();

                        bool operarEliminar = aut.SuprimirElemento(autClaseElemento, this.codigo);
                        if (operarEliminar) utiles.ButtonEnabled(ref this.radButtonEliminar, true);
                        else utiles.ButtonEnabled(ref this.radButtonEliminar, false);
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
                    this.codigoDescGrupoMayor = this.txtCodigo.Text + " " + separadorDesc + " " + this.txtNombre.Text;

                    //Actualizar los valores originales de los controles
                    //if (!this.grabarClose) this.ActualizaValoresOrigenControles();
                    this.ActualizaValoresOrigenControles();

                    //if (this.grabarClose) this.Close();
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar un grupo de cuentas de mayor
        /// </summary>
        private void Eliminar()
        {
            string mensaje = "Se va a eliminar el Grupo de cuentas de mayor " + this.codigo.Trim() + " asociado al plan de cuentas " + this.codigoPlan.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en la tabla de cuentas de auxiliar
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLMX3 ";
                    query += "where TIPLMX = '" + this.codigoPlan + "' and GRCTMX = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso el ....
                        mensaje = "No es posible eliminar el Grupo de cuentas de mayor porque está en uso en algunas de las cuentas de mayor.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el grupo de cuenta de mayor
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT22 ";
                        query += "where TIPLGC = '" + this.codigoPlan + "' and GRCTGC = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT22", this.codigoPlan, this.codigo);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar el Grupo de cuentas de mayor.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Eliminarlo de las tablas de autorizaciones
                            try
                            {
                                string elemento = this.codigoPlan + this.codigo;
                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                                query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + elemento + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM07", autClaseElemento, elemento);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                            }

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
            this.radToggleSwitchEstadoActiva.Tag = this.radToggleSwitchEstadoActiva.Value;
            this.txtNombre.Tag = this.txtNombre.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;
            this.txtNombre.Tag = "";
        }
        #endregion

        private void frmMtoGLT22_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void radButtonTextBoxPlanCuentas_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            //validar que el plan sea correcto
            bool codPlanOk = false;
            if (this.nuevo)
            {
                codPlanOk = this.CodigoPlanExiste();
                if (codPlanOk == false)
                {
                    RadMessageBox.Show("Código de plan incorrecto.", this.LP.GetText("errValPlan", "Error"));  //Falta traducir
                    bTabulador = false;
                    radButtonTextBoxPlanCuentas.Focus();
                }
                else
                {
                    this.radButtonElementPlanCuentas.Enabled = false;
                }
            }
            bTabulador = false;
        }

        private bool CodigoPlanExiste()
        {
            bool result = false;
            IDataReader dr = null;

            try
            {
                string codPlan = this.radButtonTextBoxPlanCuentas.Text.Substring(0, 1).Trim();

                if (codPlan != "")
                {
                    string query = "select NOMBMP from " + GlobalVar.PrefijoTablaCG + "GLM02 ";
                    query += "where TIPLMP = '" + codPlan + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        this.radButtonTextBoxPlanCuentas.Text = codPlan + " - " + dr.GetValue(dr.GetOrdinal("NOMBMP")).ToString().Trim();
                        result = true;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void radButtonTextBoxPlanCuentas_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }
    }
}
