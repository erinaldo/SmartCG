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

namespace ModComprobantes
{
    public partial class frmCompContRecepcionLotesNuevo : frmPlantilla, IReLocalizable
    {
        private DataTable dtEstado;
        private DataTable dtFormatoAmpliado;

        private Dictionary<string, string> literalesDict;

        public Dictionary<string, string> LiteralesDict
        {
            get
            {
                return (this.literalesDict);
            }
            set
            {
                this.literalesDict = value;
            }
        }

        public frmCompContRecepcionLotesNuevo()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContRecepcionLotesNuevo_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Recepción de Lotes Nuevo");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Crear Tabla Estado del comprobante
            this.dtEstado = new System.Data.DataTable();
            this.dtEstado.Columns.Add("valor", typeof(string));
            this.dtEstado.Columns.Add("desc", typeof(string));
            this.CrearComboEstado();

            //Crear Tabla Formato Ampliado
            this.dtFormatoAmpliado = new System.Data.DataTable();
            this.dtFormatoAmpliado.Columns.Add("valor", typeof(string));
            this.dtFormatoAmpliado.Columns.Add("desc", typeof(string));
            this.CrearComboFormatoAmpliado();

            this.TraducirLiterales();

            if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2)
            {
                this.radTextBoxControlBiblioteca.Enabled = true;
            }
            else
            {
                string biblioteca = GlobalVar.PrefijoTablaCG;
                if (biblioteca != "" && biblioteca.Substring(biblioteca.Length - 1, 1) == "_")
                {
                    biblioteca = biblioteca.Substring(0, biblioteca.Length - 1);
                    this.radTextBoxControlBiblioteca.Text = biblioteca;
                }
            }

            this.ActualizaValoresOrigenControles();

            this.ActiveControl = this.txtLote;
            this.txtLote.Select(0, 0);
            this.txtLote.Focus();
        }

        private void TxtCif_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
            ModComprobantes.UpdateDataFormEventArgs args = new UpdateDataFormEventArgs
            {
                Operacion = OperacionCompTipo.Alta
            };
            DoUpdateDataForm(args);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadTextBoxControlBiblioteca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void FrmCompContRecepcionLotesNuevo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmCompContRecepcionLotesNuevo_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cerrarForm = true;

            try
            {
                if (this.txtLote.Text.Trim() != this.txtLote.Tag.ToString() ||
                    this.radTextBoxControlBiblioteca.Text.Trim() != this.radTextBoxControlBiblioteca.Tag.ToString() ||
                    this.radDropDownListEstado.SelectedIndex.ToString() != this.radDropDownListEstado.Tag.ToString() ||
                    this.radDropDownListEstado.SelectedIndex.ToString() != this.radDropDownListEstado.Tag.ToString() ||
                    this.txtDescripcion.Text.Trim() != this.txtDescripcion.Tag.ToString()
                    )
                {
                    string mensaje = "¿Desea guardar los cambios efectuados?";  //Falta traducir

                    DialogResult result = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            if (cerrarForm) Log.Info("FIN Recepción de Lotes Nuevo");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            try
            {
                this.Text = "Recepción de Lotes - Nuevo";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
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
                if (this.txtLote.Text.Trim() == "")
                {
                    errores += "- El lote no puede estar en blanco \n\r";      //Falta traducir
                    this.txtLote.Focus();
                }

                if (this.radTextBoxControlBiblioteca.Enabled && this.radTextBoxControlBiblioteca.Text.Trim() == "")
                {
                    errores += "- La biblioteca no puede estar en blanco \n\r";      //Falta traducir
                    this.radTextBoxControlBiblioteca.Focus();
                }

                //----- Verificar Tablas de Lotes y existencia de los mismos
                string tipoBBDD = GlobalVar.ConexionCG.TipoBaseDatos.ToString();
                string bibliotecaTablasLoteAS = "";
                if (tipoBBDD == "DB2") bibliotecaTablasLoteAS = this.radTextBoxControlBiblioteca.Text + ".";

                bool resultAux = false;
                if (this.radDropDownListFornatoAmpliado.SelectedValue.ToString() == "S")
                {
                    //Si existe la tabla W10
                    if (!utilesCG.ExisteTabla(tipoBBDD, bibliotecaTablasLoteAS + this.txtLote.Text + "W10"))
                    {
                        errores += "- El archivo o la biblioteca indicada no existe \n\r";      //Falta traducir
                    }
                    else
                    {
                        //Comprobar que hay datos W10
                        resultAux = this.VerificarExistenDatosLoteTabla(bibliotecaTablasLoteAS, this.txtLote.Text + "W10");
                        if (!resultAux) errores += "- No hay comprobantes nuevos en el lote \n\r";
                        else
                        {
                            //Comprobar que NO hay datos en las tablas de errores W40
                            if (utilesCG.ExisteTabla(tipoBBDD, bibliotecaTablasLoteAS + this.txtLote.Text + "W40"))
                            {
                                resultAux = this.VerificarExistenDatosLoteTabla(bibliotecaTablasLoteAS, this.txtLote.Text + "W40");
                                if (resultAux) errores += "- Existen comprobantes erróneos en el lote \n\r";
                            }
                        }
                    }
                }
                else
                {
                    //Si existe la tabla W00
                    if (!utilesCG.ExisteTabla(tipoBBDD, bibliotecaTablasLoteAS + this.txtLote.Text + "W00"))
                    {
                        errores += "- El archivo o la biblioteca indicada no existe \n\r";      //Falta traducir
                    }
                    else
                    {
                        //Comprobar que hay datos W00
                        resultAux = this.VerificarExistenDatosLoteTabla(bibliotecaTablasLoteAS, this.txtLote.Text + "W00");
                        if (!resultAux) errores += "- No hay comprobantes nuevos en el lote \n\r";
                        else
                        {
                            //Comprobar que NO hay datos en las tablas de errores W30
                            if (utilesCG.ExisteTabla(tipoBBDD, bibliotecaTablasLoteAS + this.txtLote.Text + "W30"))
                            {
                                resultAux = this.VerificarExistenDatosLoteTabla(bibliotecaTablasLoteAS, this.txtLote.Text + "W30");
                                if (resultAux) errores += "- Existen comprobantes erróneos en el lote \n\r";
                            }
                        }
                    }                       
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") MessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }

        /// <summary>
        /// Verifica si existen datos del lote en la tabla
        /// </summary>
        /// <param name="bibliotecaTablasLoteAS"></param>
        /// <param name="nombreTabla"></param>
        /// <returns></returns>
        public bool VerificarExistenDatosLoteTabla(string bibliotecaTablasLoteAS, string nombreTabla)
        {
            bool result = false;
            IDataReader dr = null;

            try
            {
                string query = "select * from " + bibliotecaTablasLoteAS + GlobalVar.PrefijoTablaCG + nombreTabla;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                
                if (dr.Read())
                {
                    result = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

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
                
                string lote = this.txtLote.Text;
                string biblioteca = this.radTextBoxControlBiblioteca.Text.Trim();
                string estadoComp = " ";
                string formatoAmpliado = this.radDropDownListFornatoAmpliado.SelectedValue.ToString();
                string descripcion = this.txtDescripcion.Text;
                string adicionarComp = this.ObtenerCodigoAdicionarTabla();

                //Dar de alta al lote en la tabla (GLC04)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLC04";
                string query = "insert into " + nombreTabla + " (";
                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.Oracle) query += "ID_" + nombreTabla + ", ";
                query += "STATC4, PREFC4, APROC4, USERC4, WSGEC4, DESCC4, LIBLC4, LIBEC4, OUTQC4, LIBQC4) values (";
                if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.Oracle) query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + estadoComp + "', '" + lote + "', '" + adicionarComp + "', '" + GlobalVar.UsuarioLogadoCG;
                query += "','CGCS', '" + descripcion + "', '" + biblioteca + "', ' ', ' ', ' ')";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (registros == 1)
                {
                    if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.DB2)
                    {
                        //ejecutar CALL QCMDEXC('CALL GL21G', 20) en As400

                        /* llamar a GL21G */
                        string comando = "CALL PGM(GL21G)";
                        string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";
                        string longitudComando = comando.Length.ToString();

                        sentencia = sentencia + longitudComando.PadLeft(10, '0');
                        sentencia = sentencia + ".00000)";

                        GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                    else
                    {
                        //INSERT INTO XDTAQ('GO-A')    en Oracle/ SQlserver
                        query = "insert into XDTAQ (";
                        if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.Oracle) query += "ID_XDTAQ, ";
                        query += "NOMBRE, LIBRERIA, LONGITUD, DATOS) values (";
                        if (GlobalVar.ConexionCG.TipoBaseDatos == ProveedorDatos.DBTipos.Oracle) query += "ID_XDTAQ.nextval, ";
                        query += "'CGADTQ', ' ', 10, 'GO-A')";
                        registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }
       
        /// <summary>
        /// Devuelve el valor para el campo APROC4 al insertar el registro en la tabla GLC04
        /// </summary>
        /// <returns></returns>
        private string ObtenerCodigoAdicionarTabla()
        {
            string codAdicionar = "";
            bool formatoAmpliado = this.radDropDownListFornatoAmpliado.SelectedValue.ToString() == "S" ? true : false;
            try
            {
                switch (radDropDownListEstado.SelectedValue.ToString())
                {
                    case "Aprobado":
                        if (formatoAmpliado) codAdicionar = "s";
                        else codAdicionar = "S";
                        break;
                    case "NoAprobado":
                        if (formatoAmpliado) codAdicionar = "n";
                        else codAdicionar = "N";
                        break;
                    case "Contabilizado":
                        if (formatoAmpliado) codAdicionar = "c";
                        else codAdicionar = "C";
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (codAdicionar);
        }

        /// <summary>
        /// Graba la info de la compañía actual
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";

                result = this.AltaInfo();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
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
        /// Crea el desplegable de Estado del comprobante
        /// </summary>
        private void CrearComboEstado()
        {
            DataRow row;

            try
            {
                string literalAprobado = "Aprobado";
                string literalNoAprobado = "No Aprobado";
                string literalContabilizado = "Contabilizado";

                if (this.literalesDict.Count > 0)
                {
                    string aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0751").Trim();
                    if (aux != "") literalAprobado = aux;
                    aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0752").Trim();
                    if (aux != "") literalNoAprobado = aux;
                    aux = utiles.FindFirstValueByKey(ref this.literalesDict, "TÑ0753").Trim();
                    if (aux != "") literalContabilizado = aux;
                }

                row = this.dtEstado.NewRow();
                row["valor"] = "Aprobado";
                row["desc"] = literalAprobado;
                this.dtEstado.Rows.Add(row);

                row = this.dtEstado.NewRow();
                row["valor"] = "NoAprobado";
                row["desc"] = literalNoAprobado;
                this.dtEstado.Rows.Add(row);

                row = this.dtEstado.NewRow();
                row["valor"] = "Contabilizado";
                row["desc"] = literalContabilizado;
                this.dtEstado.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListEstado.DataSource = this.dtEstado;
            this.radDropDownListEstado.ValueMember = "valor";
            this.radDropDownListEstado.DisplayMember = "desc";
            this.radDropDownListEstado.SelectedIndex = 1;
            this.radDropDownListEstado.Refresh();
        }

        /// <summary>
        /// Crea el desplegable de Formato Ampliado
        /// </summary>
        private void CrearComboFormatoAmpliado()
        {
            DataRow row;

            try
            {
                row = this.dtFormatoAmpliado.NewRow();
                row["valor"] = "S";
                row["desc"] = "Sí";
                this.dtFormatoAmpliado.Rows.Add(row);

                row = this.dtFormatoAmpliado.NewRow();
                row["valor"] = "N";
                row["desc"] = "No";              
                this.dtFormatoAmpliado.Rows.Add(row);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.radDropDownListFornatoAmpliado.DataSource = this.dtFormatoAmpliado;
            this.radDropDownListFornatoAmpliado.ValueMember = "valor";
            this.radDropDownListFornatoAmpliado.DisplayMember = "desc";
            this.radDropDownListFornatoAmpliado.SelectedIndex = 1;
            this.radDropDownListFornatoAmpliado.Refresh();
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtLote.Tag = this.txtLote.Text;
            this.radTextBoxControlBiblioteca.Tag = this.radTextBoxControlBiblioteca.Text;
            this.radDropDownListEstado.Tag = this.radDropDownListEstado.SelectedIndex.ToString();
            this.radDropDownListFornatoAmpliado.Tag = this.radDropDownListFornatoAmpliado.SelectedIndex.ToString();
            this.txtDescripcion.Tag = this.txtDescripcion.Text;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor inicial (opción nuevo elemento)
        /// </summary>
        private void ActualizaValoresOrigenTAGControles()
        {
            /*this.txtCodigo.Tag = "";
            this.radToggleSwitchEstadoActiva.Tag = true;
            this.txtNombre.Tag = "";
            this.txtCif.Tag = "";
            this.radToggleSwitchImportesDecimales.Tag = true;
            this.radButtonTextBoxPlanCuentas.Tag = "";
            this.radButtonTextBoxCalendario.Tag = "";
            this.txtPrdCierreEjerc.Tag = "";
            this.rbValFechaPerTer.Tag = true;
            this.radToggleSwitchSaldos.Tag = true;  */
        }
        #endregion
    }
}
