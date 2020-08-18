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
    public partial class frmMtoGLT08 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOGRCTAU";

        private bool nuevo;
        private bool editarTipoAux;
        private string codigo;
        private string codigoTipoAux;

        private bool grabarClose;
        private string codigoDescGrupoAux;

        private const string autClaseElemento = "007";
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

        public string CodigoTipoAux
        {
            get
            {
                return (this.codigoTipoAux);
            }
            set
            {
                this.codigoTipoAux = value;
            }
        }

        public bool EditarTipoAux
        {
            get
            {
                return (this.editarTipoAux);
            }
            set
            {
                this.editarTipoAux = value;
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

        public string CodigoDesGrupoAux
        {
            get
            {
                return (this.codigoDescGrupoAux);
            }
            set
            {
                this.codigoDescGrupoAux = value;
            }
        }

        public frmMtoGLT08()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radToggleSwitchEstadoActiva.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchEstadoActiva.ThemeName = "MaterialBlueGrey";

            //------------ Inicializar atributos del formulario --------------

            //Si el campo Tipo de Auxiliar puede ser editado
            //Se utiliza para el alta. Cuando se llama dese el listado de grupos de cuentas es de edición, pero cuando 
            //se accede desde el mantenimiento de cuentas de auxiliar para dar de alta a un grupo de cuentas es de sólo lectura
            this.editarTipoAux = true;

            //Si al darle de alta al grupo de cuentas de auxiliar se cierra el formulario (para cuando se llama desde
            //el formulario de mantenimiento de cuentas de auxiliar)
            this.grabarClose = false;

            //Devuelve el codigo - descripcion del grupo de cuentas de auxiliar creado (para cuando se llama desde
            //el formulario de mantenimiento de cuentas de auxiliar)
            this.codigoDescGrupoAux = "";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLT08_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Grupos de Cuentas de Auxiliar Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            if (this.codigoTipoAux != "")
            {
                string tipoAuxDesc = utilesCG.ObtenerDescDadoCodigo("GLM04", "TAUXMT", "NOMBMT", this.codigoTipoAux, false, "").Trim();
                if (tipoAuxDesc != "")
                {
                    tipoAuxDesc = this.codigoTipoAux + " " + separadorDesc + " " + tipoAuxDesc;
                    this.radButtonTextBoxTipoAux.Text = tipoAuxDesc;
                }
                else this.radButtonTextBoxTipoAux.Text = this.codigoTipoAux;
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

                if (!this.editarTipoAux)
                {
                    this.radButtonTextBoxTipoAux.ReadOnly = true;
                    this.ActiveControl = this.txtCodigo;
                    this.txtCodigo.Select(0, 0);
                    this.txtCodigo.Focus();
                }
                else
                {
                    this.ActiveControl = this.radButtonTextBoxTipoAux;
                }
            }
            else
            {
                this.radButtonTextBoxTipoAux.ReadOnly = true;
                this.txtCodigo.IsReadOnly = true;

                //Recuperar la información de la compañía y mostrarla en los controles
                this.CargarInfoGrupoCuentaAux();

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

                if (this.radButtonTextBoxTipoAux.Text.Trim() == "" && codGrupoCtas == "")
                {
                    RadMessageBox.Show("Debe indicar el tipo de auxiliar y el código de grupo de cuentas", this.LP.GetText("errTipoAuxGrupoCtasObligatorio", "Error"));  //Falta traducir
                    this.radButtonTextBoxTipoAux.Select();
                    bTabulador = false;
                    return;
                }

                if (this.radButtonTextBoxTipoAux.Text.Trim() != "" && codGrupoCtas == "")
                {
                    this.txtCodigo.Text = "";
                    this.txtCodigo.Focus();

                    RadMessageBox.Show("Código de grupo de cuentas no válido", this.LP.GetText("errValCodGrupoCtas", "Error"));  //Falta traducir
                    bTabulador = false;
                    return;
                }

                if (codGrupoCtas != "" && this.radButtonTextBoxTipoAux.Text.Trim() == "")
                {
                    RadMessageBox.Show("Debe indicar el código del tipo de auxiliar", this.LP.GetText("errTipoAuxObligatorio", "Error"));  //Falta traducir
                    this.radButtonTextBoxTipoAux.Select();
                    bTabulador = false;
                    return;
                }

                if (this.codigoTipoAux == "")
                {
                    this.codigoTipoAux = this.radButtonTextBoxTipoAux.Text.Trim();
                    int posSeparador = this.codigoTipoAux.IndexOf(separadorDesc);
                    if (posSeparador != -1) this.codigoTipoAux = this.codigoTipoAux.Substring(0, posSeparador - 1).Trim();

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

                    this.radButtonTextBoxTipoAux.ReadOnly = true;
                    this.txtCodigo.IsReadOnly = true;

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

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.Eliminar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = "",
                Operacion = OperacionMtoTipo.Alta
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();

            ModMantenimientos.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Codigo = this.codigo
            };
            if (this.nuevo) args.Operacion = OperacionMtoTipo.Alta;
            else args.Operacion = OperacionMtoTipo.Modificar;
            DoUpdateDataForm(args);
        }

        private void RadButtonElementTipoAux_Click(object sender, EventArgs e)
        {
            //Consulta que se ejecutará para obtener los Elementos
            string query = "select TAUXMT, NOMBMT from ";
            query += GlobalVar.PrefijoTablaCG + "GLM04 ";
            query += "order by TAUXMT";

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
                TituloForm = "Seleccionar tipo de auxiliar",
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
                this.radButtonTextBoxTipoAux.Text = result;
                this.ActiveControl = this.radButtonTextBoxTipoAux;
                this.radButtonTextBoxTipoAux.Select(0, 0);
                this.radButtonTextBoxTipoAux.Focus();
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

        private void FrmMtoGLT08_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmMtoGLT08_FormClosing(object sender, FormClosingEventArgs e)
        {

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
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLT08TituloALta", "Mantenimiento de Grupos de Cuentas de Auxiliar - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT08TituloEdit", "Mantenimiento de Grupos de Cuentas de Auxiliar - Edición");           //Falta traducir

            //Traducir los Literales de los ToolStrip
            this.radButtonSave.Text = this.LP.GetText("toolStripGrabar", "Grabar");
            this.radButtonEliminar.Text = this.LP.GetText("toolStripEliminar", "Eliminar");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
        }

        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo del grupo de cuentas de auxiliar (al dar de alta a un grupo)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.radToggleSwitchEstadoActiva.Enabled = valor;
            this.txtNombre.Enabled = valor;
        }

        /// <summary>
        /// Inactiva todos los campos del formulario. El grupo de cuentas de auxiliar está en modo consulta
        /// </summary>
        private void NoEditarCampos()
        {
            utiles.ButtonEnabled(ref this.radButtonSave, false);

            this.radButtonTextBoxTipoAux.Enabled = false;
            this.radToggleSwitchEstadoActiva.Enabled = false;
            this.txtNombre.IsReadOnly = true;
        }

        /// <summary>
        /// Rellena los controles con los datos del grupo de cuentas auxiliares (modo edición)
        /// </summary>
        private void CargarInfoGrupoCuentaAux()
        {
            IDataReader dr = null;
            try
            {
                string estado = "";
                this.txtCodigo.Text = this.codigo;

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT08 ";
                query += "where TAUXGA = '" + this.codigoTipoAux + "' and GRCTGA = '" + this.codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    this.txtNombre.Text = dr.GetValue(dr.GetOrdinal("NOMBGA")).ToString().Trim();

                    estado = dr.GetValue(dr.GetOrdinal("STATGA")).ToString().Trim();
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
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT08 ";
                    query += "where TAUXGA = '" + this.codigoTipoAux + "' and GRCTGA = '" + codGrupoCtas + "'";

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
                //-------------- Validar el nombre de la cuenta de auxiliar ------------------
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
        /// Dar de alta a una compañía
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                //Dar de alta al grupo de cuentas (GLT08)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT08";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "STATGA, TAUXGA, GRCTGA, NOMBGA) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estado + "', '" + this.codigoTipoAux + "', '" + this.codigo + "', '";
                query += this.txtNombre.Text + "')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLT08", this.codigoTipoAux, this.codigo);

                //Insertar al usuario como propietario del elemento
                string elemento = this.codigoTipoAux + this.codigo;
                nombreTabla = GlobalVar.PrefijoTablaCG + "ATM07";
                query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "CLELAF, ELEMAF, IDUSAF) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + autClaseElemento + "', '" + elemento + "', '" + GlobalVar.UsuarioLogadoCG.ToUpper() + "')";

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "ATM07", autClaseElemento, elemento);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualizar una compañía
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo()
        {
            string result = "";
            try
            {
                string estado = (this.radToggleSwitchEstadoActiva.Value) ? "V" : "*";

                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT08 set ";
                query += "STATGA = '" + estado + "', NOMBGA = '" + this.txtNombre.Text + "' ";
                query += "where TAUXGA = '" + this.codigoTipoAux + "' and GRCTGA = '" + this.codigo + "'";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT08", this.codigoTipoAux, this.codigo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Eliminar un Grupo de Cuentas de Auxiliar
        /// </summary>
        private void Eliminar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Pedir confirmación
            string mensaje = "Se va a eliminar el Grupo de cuentas de auxiliar " + this.codigo.Trim() + " asociado al Tipo de auxiliar " + this.codigoTipoAux.Trim();  //Falta traducir
            mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
            DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                //Eliminarlo
                try
                {
                    //Buscar si existen entradas en la tabla de cuentas de auxiliar
                    string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLM05 ";
                    query += "where TAUXMA = '" + this.codigoTipoAux + "' and GRCTMA = '" + this.codigo + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros > 0)
                    {
                        //Está en uso en cuentas de auxiliar
                        mensaje = "No es posible eliminar el Grupo de cuentas de auxiliar porque está en uso en cuentas de auxiliar.";
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                    }
                    else
                    {
                        //Eliminar el grupo de cuenta de auxiliar
                        query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT08 ";
                        query += "where TAUXGA = '" + this.codigoTipoAux + "' and GRCTGA = '" + this.codigo + "'";

                        cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT08", this.codigoTipoAux, this.codigo);

                        if (cantRegistros != 1)
                        {
                            mensaje = "No fue posible eliminar el Grupo de cuentas de auxiliar.";
                            RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                        }
                        else
                        {
                            //Eliminarlo de las tablas de autorizaciones
                            try
                            {
                                string elemento = this.codigoTipoAux + this.codigo;
                                query = "delete from " + GlobalVar.PrefijoTablaCG + "ATM07 ";
                                query += "where CLELAF = '" + autClaseElemento + "' and ELEMAF = '" + elemento + "'";

                                cantRegistros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "ATM07", autClaseElemento, elemento);
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

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Grabar un Grupo de Cuentas de Auxiliar
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
                    this.codigoDescGrupoAux = this.txtCodigo.Text + " " + separadorDesc + " " + this.txtNombre.Text;

                    //Actualizar los valores originales de los controles
                    if (!this.grabarClose) this.ActualizaValoresOrigenControles();

                    //if (this.grabarClose) this.Close();
                    this.Close();
                }
            }

            Cursor.Current = Cursors.Default;
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
            this.radToggleSwitchEstadoActiva.Tag = "V";
            this.txtNombre.Tag = "";
        }
        #endregion

        private void frmMtoGLT08_MouseEnter(object sender, EventArgs e)
        {
            bCancelar = false;
        }

        private void radButtonTextBoxTipoAux_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();

            if (bCancelar == true && bTabulador == false) return;

            //validar que el tipo de auxiliar
            bool codPlanOk = false;
            if (this.nuevo)
            {
                codPlanOk = this.TipoAuxiliarExiste();
                if (codPlanOk == false)
                {
                    RadMessageBox.Show("Tipo de Auxiliar incorrecto.", this.LP.GetText("errValTipo", "Error"));  //Falta traducir
                    bTabulador = false;
                    radButtonElementTipoAux.Focus();
                }
                else
                {
                    this.radButtonElementTipoAux.Enabled = false;
                }
            }
            bTabulador = false;
        }

        private bool TipoAuxiliarExiste()
        {
            bool result = false;
            IDataReader dr = null;

            try
            {
                string tipAux = this.radButtonTextBoxTipoAux.Text.Substring(0, 2).Trim();

                if (tipAux != "")
                {
                    string query = "select NOMBMT from " + GlobalVar.PrefijoTablaCG + "GLM04 ";
                    query += "where TAUXMT = '" + tipAux + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                    if (dr.Read())
                    {
                        this.radButtonTextBoxTipoAux.Text = tipAux + " - " + dr.GetValue(dr.GetOrdinal("NOMBMT")).ToString().Trim();
                        result = true;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void radButtonTextBoxTipoAux_KeyPress(object sender, KeyPressEventArgs e)
        {
            bTabulador = false;
            if (e.KeyChar == (char)Keys.Tab) bTabulador = true;
        }
    }
}
