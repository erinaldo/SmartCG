using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using ObjectModel;

namespace ModComprobantes
{
    public partial class frmCompContTransferirFinanComprobantes : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCCTRFICOM";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCCTRFICOM
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string todos;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string descripcion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string prefijo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string bilbiotecaPrefijo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string colaSalida;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string bibliotecaColaSalida;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string generarLoteBatch;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string generarLoteAdiciona;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string estado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string verNumerosComp;
        }

        FormularioValoresCampos valoresFormulario;

        private DataTable dataTable = null; 
        private string pathFicherosCompContables = "";
        private string tipoBaseDatosCG = "";

        private string prefijoLote = "";

        ComprobanteContableTransferir compContTransf;

        public frmCompContTransferirFinanComprobantes()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmCompContTransferirFinanComprobantes_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Transferir Comprobantes a Finanzas");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            //this.KeyPreview = true;

            //ModComp_PathFicherosCompContables
            pathFicherosCompContables = ConfigurationManager.AppSettings["ModComp_PathFicherosCompContables"];
            if (pathFicherosCompContables != null) this.txtPath.Text = pathFicherosCompContables;

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    this.cmbEstado.SelectedIndex = 0;

                    this.rbEstadoNoTrans.Checked = true;

                    this.rbGenerarLote.Checked = true;
                }
            }
            else
            {
                this.cmbEstado.SelectedIndex = 0;

                this.rbEstadoNoTrans.Checked = true;

                this.rbGenerarLote.Checked = true;
            }

            compContTransf = new ComprobanteContableTransferir();
            compContTransf.tipoBaseDatosCG = this.tipoBaseDatosCG;
            compContTransf.LP = this.LP;

            //Crear el DataGrid y sus columnas
            this.BuildDataGrid();

            //Llenar el DataGrid
            this.FillDataGrid();

            //Poner en el idioma correspondiente todos los literales
            this.TraducirLiterales();

            if (tipoBaseDatosCG != "DB2")
            {
                this.txtBibliotecaPrefijo.Text = "";
                this.txtCola.Text = "";
                this.txtBibliotecaCola.Text = "";
                this.txtBibliotecaPrefijo.Enabled = false;
                this.txtCola.Enabled = false;
                this.txtBibliotecaCola.Enabled = false;
            }
        }

        private void btnSelCompContable_Click(object sender, EventArgs e)
        {
            this.SeleccionarPath(this.txtPath);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbGenerarLoteAdiciona_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLoteAdiciona.Checked)
            {
                this.cmbEstado.Enabled = true;
                this.rbGenerarLote.Checked = false;
            }
            else
            {
                this.cmbEstado.Enabled = false;
            }
        }

        private void rbGenerarLote_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLote.Checked)
            {
                this.rbGenerarLoteAdiciona.Checked = false;
                this.cmbEstado.Enabled = false;
            }
        }

        private void rbEstadoNoTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dataTable != null)
            {
                //Al cambiar la seleccion del estado de los comprobantes (transferidos o no transferidos) volver a buscar los comprobantes
                this.dataTable.Rows.Clear();
                this.FillDataGrid();
            }
        }

        private void txtPath_Leave(object sender, EventArgs e)
        {
            if (this.dataTable != null)
            {
                //Al cambiar la seleccion del estado de los comprobantes (transferidos o no transferidos) volver a buscar los comprobantes
                this.dataTable.Rows.Clear();
                this.FillDataGrid();
            }
        }

        private void frmCompContTransferirFinanComprobantes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null); 
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonTransferir_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            this.prefijoLote = this.txtPrefijo.Text.Trim().ToUpper();

            string result = this.ValidarForm();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                return;
            }

            compContTransf.prefijoLote = this.txtPrefijo.Text.Trim();
            compContTransf.bibliotecaPrefijo = this.txtBibliotecaPrefijo.Text.Trim().ToUpper();
            compContTransf.biliotecaCola = this.txtBibliotecaCola.Text.Trim().ToUpper();
            compContTransf.cola = this.txtCola.Text;
            compContTransf.descripcion = this.txtDescripcion.Text;
            compContTransf.extendido = Convert.ToBoolean(this.dgComprobantes.CurrentRow.Cells["extendido"].Value);
            
            result = compContTransf.VerificarExistenDatosLote();
            bool procesarComp = true;
            if (result != "")
            {
                //Hay datos del lote, pedir confirmación para borrarlos
                string mensaje = result + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿Desea eliminar los lotes?");
                DialogResult resultDialog = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                result = "";
                if (resultDialog == DialogResult.No) procesarComp = false;
                else
                {
                    //Eliminar los lotes de todas las tablas
                    try
                    {
                        string eliminar = "";
                        for (int i = 0; i < compContTransf.tablasLotes.Count; i++)
                        {
                            eliminar += compContTransf.EliminarDatosLoteTabla(compContTransf.tablasLotes[i].ToString());
                        }

                        if (eliminar != "")
                        {
                            result += eliminar;
                            procesarComp = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        result += "Error eliminando lotes de las tablas (" + ex.Message + ")" + "\n\r";
                        procesarComp = false;
                    }
                }
            }

            result += this.VerificarNumerosDocumentos();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(result, error);
                return;
            }

            if (procesarComp)
            {
                string compTransferidos = this.ProcesarComprobantes();
                if (compTransferidos != "" && compTransferidos != "-1")
                {
                    //Error transfiriendo los comprobantes
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(compTransferidos, error);
                }
                else
                {
                    if (this.rbGenerarLoteAdiciona.Checked && compTransferidos == "")
                    {
                        //Insertar registro en la tabla GLC04
                        compContTransf.tipoContabilizacion = this.cmbEstado.SelectedIndex + 1;
                        compContTransf.InsertarRegistroGLC04();
                    }

                    if (this.chkVerNocomp.Checked)
                    {
                        if (compTransferidos != "-1")
                        {
                            if (compContTransf.numCompGenerados != null && compContTransf.numCompGenerados.Rows.Count > 0)
                            {
                                //Mostrar los números de comprobantes generados
                                frmVisorNumComp frmVerNumComp = new frmVisorNumComp();
                                frmVerNumComp.Datos = compContTransf.numCompGenerados;
                                frmVerNumComp.Extracontable = 0;
                                frmVerNumComp.FrmPadre = this;
                                frmVerNumComp.ShowDialog();
                                compContTransf.numCompGenerados.Rows.Clear();
                                
                                /*//Mostrar los números de comprobantes generados
                                string noCompGenerados = "Números de comprobantes generados: \n\r";  //FALTA Traducir
                                string[] info;
                                for (int i = 0; i < this.compContTransf.noCompGenerados.Count; i++)
                                {
                                    info = (string[])this.compContTransf.noCompGenerados[i];
                                    noCompGenerados += "    " + info[0] + "   - " + info[1] + "\n\r";
                                }

                                MessageBox.Show(noCompGenerados, "");*/
                            }
                            else
                            {
                                //Mostrar mensaje de transferido OK
                                if (this.dgComprobantes.SelectedRows.Count == 1) MessageBox.Show("El comprobante se transfirió correctamente", ""); //FALTA Traducir
                                else if (this.dgComprobantes.SelectedRows.Count > 1) MessageBox.Show("Los comprobantes se transfirieron correctamente", ""); //FALTA Traducir
                            }
                        }
                        else
                        {
                            //Mostrar mensaje de NO transferido
                            if (this.dgComprobantes.SelectedRows.Count == 1) MessageBox.Show("El comprobante no se pudo transferir", this.LP.GetText("errValTitulo", "Error")); //FALTA Traducir
                            else if (this.dgComprobantes.SelectedRows.Count > 1) MessageBox.Show("Existen comprobantes que no se pudieron transferir", this.LP.GetText("errValTitulo", "Error")); //FALTA Traducir
                        }
                    }
                    else
                    {
                        //Mostrar mensaje de transferido OK
                        if (this.dgComprobantes.SelectedRows.Count == 1) MessageBox.Show("El comprobante se transfirió correctamente", ""); //FALTA Traducir
                        else if (this.dgComprobantes.SelectedRows.Count > 1) MessageBox.Show("Los comprobantes se transfirieron correctamente", ""); //FALTA Traducir
                    }

                    //Actualizar la Grid
                    if (this.dataTable != null)
                    {
                        if (this.dgComprobantes.SelectedRows.Count > 1 || compTransferidos != "-1")
                        {
                            //Al cambiar la seleccion del estado de los comprobantes (transferidos o no transferidos) volver a buscar los comprobantes
                            this.dataTable.Rows.Clear();
                            this.FillDataGrid();
                        }
                    }
                }
            }

            //Grabar la petición
            string valores = this.ValoresPeticion();

            this.valoresFormulario.GrabarParametros(formCode, valores);

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void frmCompContTransferirFinanComprobantes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Transferir Comprobantes a Finanzas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContTransferirFinanComprobantesTitulo", "Transferir Comprobantes a Finanzas");
            
            this.gbComprobante.Text = this.LP.GetText("lblCompTransFinComprobantes", "Comprobantes");
            this.folder.Description = this.LP.GetText("lblCompTransFinPath", "Ubicación");
            this.lblEstado.Text = this.LP.GetText("lblCompTransEstado", "Estado");
            this.rbEstadoNoTrans.Text = this.LP.GetText("lblCompTransFinNoTransf", "No Transferidos");
            this.rbEstadoTodos.Text = this.LP.GetText("lblCompTransFinTodos", "Todos");
            this.gbLote.Text = this.LP.GetText("lblCompTransFinLoteBatch", "Lote Batch"); 
            this.lblPrefijo.Text = this.LP.GetText("lblCompTransFinPrefijo", "Prefijo"); 
            this.lblBibliotecaPrefijo.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinCola", "Cola de Salida"); 
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.gbTransferencia.Text = this.LP.GetText("lblCompTransFinTipoTransf", "Tipo de Transferencia"); 
            this.rbGenerarLote.Text = this.LP.GetText("lblCompTransFinGenerarLoteB", "Solo Generar Lote");
            this.rbGenerarLoteAdiciona.Text = this.LP.GetText("lblCompTransFinGenerarLoteBA", "Generar Lote y Adicionar");

            //Desplegable de Estados (en el idioma que corresponda)
            string[] literales = {this.LP.GetText("lblCompTransFinNoAprob", "No Aprobado/s"), 
                                  this.LP.GetText("lblCompTransFinAprob", "Aprobado/s"),
                                  this.LP.GetText("lblCompTransFinContab", "Contabilizado/s")};
            for (int i = 0; i < this.cmbEstado.Items.Count; i++)
            {
                //this.cmbEstado.Items[i] = (i + 1).ToString() + "-" + literales[i];
                this.cmbEstado.Items[i] = literales[i];
            }

            this.chkVerNocomp.Text = this.LP.GetText("lblCompTransFinVerNoComp", "Ver los números de comprobante");

            //Literales del Encabezado del DataGrid
            this.dgComprobantes.Columns["archivo"].HeaderText = this.LP.GetText("CompContdgHeaderArchivo", "Archivo");
            this.dgComprobantes.Columns["compania"].HeaderText = this.LP.GetText("CompContdgHeaderCompania", "Compañía");
            this.dgComprobantes.Columns["aapp"].HeaderText = this.LP.GetText("CompContdgHeaderAAPP", "AA-PP");
            this.dgComprobantes.Columns["descripcion"].HeaderText = this.LP.GetText("CompContdgHeaderDescripcion", "Descripción");
            this.dgComprobantes.Columns["tipo"].HeaderText = this.LP.GetText("CompContdgHeaderTipo", "Tipo");
            this.dgComprobantes.Columns["noComp"].HeaderText = this.LP.GetText("CompContdgHeaderNoComp", "No Comp.");
            this.dgComprobantes.Columns["fecha"].HeaderText = this.LP.GetText("CompContdgHeaderFecha", "Fecha");
            this.dgComprobantes.Columns["clase"].HeaderText = this.LP.GetText("CompContdgHeaderClase", "Clase");
            this.dgComprobantes.Columns["tasa"].HeaderText = this.LP.GetText("CompContdgHeaderTasa", "Tasa");
            this.dgComprobantes.Columns["debeML"].HeaderText = this.LP.GetText("CompContdgHeaderDebeML", "Debe ML");
            this.dgComprobantes.Columns["haberML"].HeaderText = this.LP.GetText("CompContdgHeaderHaberML", "Haber ML");
            this.dgComprobantes.Columns["debeME"].HeaderText = this.LP.GetText("CompContdgHeaderDebeME", "Debe ME");
            this.dgComprobantes.Columns["haberME"].HeaderText = this.LP.GetText("CompContdgHeaderHaberME", "Haber ME");
            this.dgComprobantes.Columns["noMovimiento"].HeaderText = this.LP.GetText("CompContdgHeaderNoMovimiento", "No Movimiento");
            this.dgComprobantes.Columns["transferido"].HeaderText = this.LP.GetText("CompContdgHeaderTransferido", "Transferido");
        }

        /// <summary>
        /// Seleccionar la ruta del fichero que corresponda
        /// </summary>
        /// <param name="txtControl"></param>
        private void SeleccionarPath(TextBox txtControl)
        {
            this.folder.Description = this.LP.GetText("lblConfigSelPath", "Seleccionar una carpeta");
            this.folder.ShowNewFolderButton = false;

            if (this.txtPath.Text != "") folder.SelectedPath = this.txtPath.Text;

            DialogResult result = this.folder.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtControl.Text = this.folder.SelectedPath;
                Environment.SpecialFolder root = folder.RootFolder;

                if (this.dataTable != null)
                {
                    //Al cambiar la seleccion del estado de los comprobantes (transferidos o no transferidos) volver a buscar los comprobantes
                    this.dataTable.Rows.Clear();
                    this.FillDataGrid();
                }
            }
        }

        /// <summary>
        /// Construye el dataGrid
        /// </summary>
        private void BuildDataGrid()
        {
            this.dataTable = new DataTable();

            try
            {
                //Adicionar las columnas al DataTable
                this.compContTransf.CrearColumnasDataTableComprobantes(ref this.dataTable);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            this.dgComprobantes.DataSource = this.dataTable;
        }

        /// <summary>
        /// Rellena el DataGrid con la información correspondiente
        /// </summary>
        private void FillDataGrid()
        {
            //Leer todos los ficheros con extensión xml que existan dentro de la carpeta ModComp_PathFicherosCompContables
            try
            {
                this.lblNoHayComp.Text = "";
                DirectoryInfo dir = new DirectoryInfo(this.txtPath.Text);
                FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (FileInfo FI in fileList)
                {
                    try { compContTransf.ProcesarFichero(FI.FullName, FI.Name, ref this.dataTable, true, this.rbEstadoTodos.Checked); }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }

                if (this.dgComprobantes.Rows.Count > 0)
                {
                    this.dgComprobantes.Columns["extendido"].Visible = false;
                    this.dgComprobantes.Columns["revertir"].Visible = false;
                    this.dgComprobantes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                Console.WriteLine(ex.Message);
                this.lblNoHayComp.Text = "Error recuperando los comprobantes (" + ex.Message + ")";     //FALTA Traducir
            }

            //Chequear que existan comprobantes
            this.VerificarExistenciaComprobantes();
        }

        /// <summary>
        /// Verifica que existan comprobantes. Si no existen, lo informa en el formulario
        /// </summary>
        private void VerificarExistenciaComprobantes()
        {
            try
            {
                if (this.dgComprobantes.Rows.Count <= 0)
                {
                    this.dgComprobantes.Visible = false;

                    if (this.lblNoHayComp.Text == "") this.lblNoHayComp.Text = this.LP.GetText("errNoExistencomprobantes", "No existen comprobantes en la ruta") + " " + pathFicherosCompContables;
                    this.lblNoHayComp.Visible = true;
                    this.toolStripButtonTransferir.Enabled = false;
                }
                else
                {
                    this.dgComprobantes.Visible = true;

                    this.lblNoHayComp.Text = "";
                    this.lblNoHayComp.Visible = false;
                    this.toolStripButtonTransferir.Enabled = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private string ValidarForm()
        {
            string result = "";

            if (this.dgComprobantes.SelectedRows.Count <= 0)
            {
                result += " - " + "Debe seleccionar al menos un comprobante " + "\n\r"; //Falta traducir
                this.dgComprobantes.Focus();
            }

            if (this.prefijoLote == "")
            {
                result += " - " + "El prefijo no puede estar en blanco " + "\n\r"; //Falta traducir
                this.txtPrefijo.Focus();
            }

            if (tipoBaseDatosCG == "DB2")
            {
                if (this.txtBibliotecaPrefijo.Text == "")
                {
                    result += " - " + "La biblioteca no puede estar en blanco " + "\n\r"; //Falta traducir
                    this.txtBibliotecaPrefijo.Focus();
                }

                if (this.txtCola.Text != "" && this.txtBibliotecaCola.Text == "")
                {
                    result += " - " + "Si la Cola de salida está informada, también tiene que estar informada la biblioteca " + "\n\r"; //Falta traducir
                    this.txtBibliotecaCola.Focus();
                }
            }

            return (result);
        }

        /// <summary>
        /// Verifica que los números de documentos que han sido informados, sean correctos
        /// </summary>
        /// <returns></returns>
        private string VerificarNumerosDocumentos()
        {
            string result = "";

            IDataReader dr = null;

            int currentRow;
            string numComprobante;
            string ccia;
            string sapr;
            string tico;
            string query;
            try
            {
                for (int i = 0; i < this.dgComprobantes.SelectedRows.Count; i++)
                {
                    currentRow = this.dgComprobantes.SelectedRows[i].Index;
                    numComprobante = this.dgComprobantes.Rows[currentRow].Cells["noComp"].Value.ToString();

                    if (numComprobante != "")
                    {
                        //Verificar que el numero de comprobante no este en uso
                        ccia = this.dgComprobantes.Rows[currentRow].Cells["compania"].Value.ToString().ToUpper();
                        sapr = this.dgComprobantes.Rows[currentRow].Cells["aapp"].Value.ToString().Trim();
                        sapr = sapr.Replace("-", "");
                        sapr = utiles.SigloDadoAnno(sapr.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC) + sapr;
                        tico = this.dgComprobantes.Rows[currentRow].Cells["tipo"].Value.ToString();

                        query = "select NUCOIC from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                        query += "where CCIAIC = '" + ccia + "' and SAPRIC =" + sapr + " and TICOIC = " + tico;
                        query += " and NUCOIC = " + numComprobante;

                        dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                        if (dr.Read())
                        {
                            result += " - " + "El número de comprobante ya esta asignado " + "\n\r"; //Falta traducir
                        }

                        dr.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                result += " - " + "Error chequeando los números de comprobantes (" + ex.Message + ")" + "\n\r"; //Falta traducir   
            }

            return (result);
        }


        /// <summary>
        /// Transferir los comprobantes marcados
        /// </summary>
        private string ProcesarComprobantes()
        {
            string result = "";

            string compTranferidos = "-1";
            try
            {
                string archivo;
                string descCompArchivo = "";
                int currentRow;

                string revertir = "";
                bool transfComp = true;
                for (int i = 0; i < this.dgComprobantes.SelectedRows.Count; i++)
                {
                    currentRow = this.dgComprobantes.SelectedRows[i].Index;

                    revertir = this.dgComprobantes.Rows[currentRow].Cells["revertir"].Value.ToString();

                    if (revertir == "S" || revertir == "T")
                    {
                        //Pedir confirmacion para la reversión
                        string mensaje = this.LP.GetText("wrn1RervertirComp", "El comprobante ") + this.dgComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString() + " " + this.LP.GetText("wrn1RervertirComp", "es de reversión. ¿Desea transferirlo?");    //Falta traducir
                        DialogResult resultDialog = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (resultDialog == DialogResult.No) transfComp = false;
                        else transfComp = true;

                    }
                    else transfComp = true;

                    if (transfComp)
                    {
                        archivo = this.dgComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                        compContTransf.archivo = archivo;
                        descCompArchivo = this.dgComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString();
                        compContTransf.descCompArchivo = descCompArchivo;

                        result = this.TransferirCabecera(currentRow, false);

                        if (result == "")
                        {
                            //Transferir los detalles
                            compTranferidos = "";

                            //Leer el comprobante
                            archivo = this.dgComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                            DataSet ds = new DataSet();
                            ds.ReadXml(this.txtPath.Text + "\\" + archivo);
                            //ds.ReadXml(archivo);

                            //Verificar que exista la tabla de Detalle
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables["Detalle"].Rows.Count; j++)
                                {
                                    if (!TodaFilaEnBlanco(ds.Tables["Detalle"], j))
                                    {
                                        result = this.TransferirDetalle(ds.Tables["Detalle"].Rows[j], false);

                                        if (result != "")
                                        {
                                            compTranferidos = "-1";
                                            string error = this.LP.GetText("errValTitulo", "Error");
                                            MessageBox.Show(result, error);
                                            break;
                                        }
                                    }
                                }
                            }

                            //Actualizar el fichero xml para que el campo transferido tenga el valor 1
                            if (result == "" && ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                            {
                                ds.Tables["Cabecera"].Rows[0]["Transferido"] = "1";
                                ds.WriteXml(this.txtPath.Text + "\\" + archivo);
                            }
                        }
                        else
                        {
                            compTranferidos = "-1";
                            string error = this.LP.GetText("errValTitulo", "Error");
                            MessageBox.Show(result, error);
                        }

                        if (revertir == "S" || revertir == "T")
                        {
                            //SMR - Revertir
                            archivo = this.dgComprobantes.Rows[currentRow].Cells["archivo"].Value.ToString();
                            compContTransf.archivo = archivo;
                            descCompArchivo = this.dgComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString();
                            compContTransf.descCompArchivo = descCompArchivo;

                            result = this.TransferirCabecera(currentRow, true);

                            if (result == "")
                            {
                                //Transferir los detalles
                                compTranferidos = "";

                                //Leer el comprobante
                                DataSet ds = new DataSet();
                                ds.ReadXml(pathFicherosCompContables + "\\" + archivo);

                                //Verificar que exista la tabla de Detalle
                                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Detalle"].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds.Tables["Detalle"].Rows.Count; j++)
                                    {
                                        if (!TodaFilaEnBlanco(ds.Tables["Detalle"], j))
                                        {
                                            result = this.TransferirDetalle(ds.Tables["Detalle"].Rows[j], true);

                                            if (result != "")
                                            {
                                                string error = this.LP.GetText("errValTitulo", "Error");
                                                MessageBox.Show(result, error);
                                                break;
                                            }
                                        }
                                    }
                                }

                                //Actualizar el fichero xml para que el campo transferido tenga el valor 1
                                if (result == "" && ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["Cabecera"].Rows.Count > 0)
                                {
                                    ds.Tables["Cabecera"].Rows[0]["Transferido"] = "1";
                                    ds.WriteXml(pathFicherosCompContables + "\\" + archivo);
                                }
                            }
                            else
                            {
                                string error = this.LP.GetText("errValTitulo", "Error");
                                MessageBox.Show(result, error);
                            }
                        }
                        //END SMR - Revertir
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                compTranferidos = "-1";
                compTranferidos = "Error transfiriendo los comprobantes (" + ex.Message + ")";  //FALTA Traducir
            }

            return (compTranferidos);
        }

        /// <summary>
        /// Devuelve si todas las columnas están vacías dado una fila de un DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="row">fila</param>
        /// <returns></returns>
        private bool TodaFilaEnBlanco(DataTable table, int row)
        {
            bool todaFilaBlanco = true;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Rows[row][i].ToString() != "")
                {
                    todaFilaBlanco = false;
                    break;
                }
            }

            return (todaFilaBlanco);
        }

        /// <summary>
        /// Transfiere la cabecera del comprobante
        /// </summary>
        /// <param name="currentRow">fila de la grid de comprobantes</param>
        /// <returns></returns>
        private string TransferirCabecera(int currentRow, Boolean esReversion)
        {
            string result = "";
            try
            {
                string aapp = "";
                string fechaCad = "";
                DateTime fecha;

                compContTransf.TTRAWS = "1";

                compContTransf.revertir = this.dgComprobantes.Rows[currentRow].Cells["revertir"].Value.ToString();
                compContTransf.CCIAWS = this.dgComprobantes.Rows[currentRow].Cells["compania"].Value.ToString().ToUpper();
                aapp = this.dgComprobantes.Rows[currentRow].Cells["aapp"].Value.ToString();
                aapp = aapp.Replace("-", "");
                if (aapp.Length >= 2)
                {
                    compContTransf.ANOCWS = aapp.Substring(0, 2);
                    compContTransf.LAPSWS = aapp.Substring(aapp.Length - 2, 2);
                }

                compContTransf.TICOWS = this.dgComprobantes.Rows[currentRow].Cells["tipo"].Value.ToString();
                compContTransf.NUCOWS = this.dgComprobantes.Rows[currentRow].Cells["noComp"].Value.ToString();
                compContTransf.TVOUWS = this.dgComprobantes.Rows[currentRow].Cells["clase"].Value.ToString();

                fechaCad = this.dgComprobantes.Rows[currentRow].Cells["fecha"].Value.ToString();
                if (fechaCad != "")
                {
                    try
                    {
                        //fecha = Convert.ToDateTime(utiles.FormatoCGToFecha(fechaCad));
                        fecha = Convert.ToDateTime(fechaCad);
                        compContTransf.DIAEWS = fecha.Day.ToString();
                        compContTransf.MESEWS = fecha.Month.ToString();
                        compContTransf.ANOEWS = fecha.Year.ToString();

                        if (compContTransf.ANOEWS.Length > 2) compContTransf.ANOEWS = compContTransf.ANOEWS.Substring(compContTransf.ANOEWS.Length - 2, 2);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        compContTransf.DIAEWS = "";
                        compContTransf.MESEWS = "";
                        compContTransf.ANOEWS = "";
                    }
                }

                compContTransf.TASCWS = this.dgComprobantes.Rows[currentRow].Cells["tasa"].Value.ToString();
                compContTransf.DOCDWS = this.dgComprobantes.Rows[currentRow].Cells["descripcion"].Value.ToString().Replace("'", "''");
                /* compContTransf.DOCDWS = this.txtDescripcion.Text.Replace("'", "''"); */
                compContTransf.extendido = Convert.ToBoolean(this.dgComprobantes.Rows[currentRow].Cells["extendido"].Value);

                result = compContTransf.TransferirCabecera(esReversion);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error transfiriendo la cabecera del comprobante (" + ex.Message + ")";    //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Transfiere la cabecera del comprobante
        /// </summary>
        /// <param name="indexRow">fila de la grid de comprobantes</param>
        /// <returns></returns>
        private string TransferirDetalle(DataRow row, Boolean esReversion)
        {
            string result = "";
            try
            {
                string documento = "";
                DateTime fecha;
                compContTransf.TTRAWS = "2";

                try { compContTransf.CUENWS = row["Cuenta"].ToString().ToUpper(); }
                catch(Exception ex) 
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    //compContTransf.PRFDWS = "";     //¿?¿?PRFDWS o CUENWS ??
                    compContTransf.CUENWS = "";
                }
                try { compContTransf.CAUXWS = row["Auxiliar1"].ToString().ToUpper(); }
                catch(Exception ex) 
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.CAUXWS = ""; 
                }
                try { compContTransf.DESCWS = row["Descripcion"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.DESCWS = ""; 
                }
                compContTransf.DESCWS = compContTransf.DESCWS.Replace("'", "''");
                try { compContTransf.MONTWS = row["MonedaLocal"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.MONTWS = ""; 
                }
                try { compContTransf.TMOVWS = row["DH"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TMOVWS = ""; 
                }
                try { compContTransf.MOSMWS = row["MonedaExt"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.MOSMWS = ""; 
                }
                try { documento = row["Documento"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    documento = ""; 
                }
                
                if (documento.Length > 2)
                {
                    compContTransf.CLDOWS = documento.Substring(0, 2).ToUpper();
                    compContTransf.NDOCWS = documento.Substring(2, documento.Length - 3);
                    if (compContTransf.NDOCWS == "") compContTransf.NDOCWS = "0";
                }
                else
                {
                    compContTransf.CLDOWS = documento.ToUpper();
                    compContTransf.NDOCWS = "0";
                }

                try { compContTransf.FDOCWS = row["Fecha"].ToString(); }
                catch (Exception ex) 
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.FDOCWS = "0"; 
                }
                if (compContTransf.FDOCWS == "") compContTransf.FDOCWS = "0";
                try { compContTransf.FEVEWS = row["Vencimiento"].ToString(); }
                catch (Exception ex) 
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.FEVEWS = "0"; 
                }
                if (compContTransf.FEVEWS == "") compContTransf.FEVEWS = "0";

                //try
                //{
                //    if (compContTransf.FDOCWS != "")
                //    {
                //        fecha = Convert.ToDateTime(compContTransf.FDOCWS);
                //        compContTransf.FDOCWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                //    }
                //    else compContTransf.FDOCWS = "0";
                //    if (compContTransf.FEVEWS != "")
                //    {
                //        fecha = Convert.ToDateTime(compContTransf.FEVEWS);
                //        compContTransf.FEVEWS = utiles.FechaToFormatoCG(fecha, false).ToString();
                //    }
                //    else compContTransf.FEVEWS = "0";
                //}
                //catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                try { compContTransf.TEINWS = row["RU"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TEINWS = ""; 
                }
                try { compContTransf.NNITWS = row["CifDni"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.NNITWS = ""; 
                }
                try { compContTransf.AUA1WS = row["Auxiliar2"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.AUA1WS = ""; 
                }
                try { compContTransf.AUA2WS = row["Auxiliar3"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.AUA2WS = ""; 
                }
                try { documento = row["Documento2"].ToString(); documento = documento.Replace("-", ""); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    documento = ""; 
                }
                
                if (documento.Length > 2)
                {
                    compContTransf.CDDOWS = documento.Substring(0, 2).ToString();
                    compContTransf.NDDOWS = documento.Substring(2, documento.Length - 3);
                    if (compContTransf.NDDOWS == "") compContTransf.NDDOWS = "0";
                }
                else
                {
                    compContTransf.CDDOWS = documento.ToString();
                    compContTransf.NDDOWS = "0";
                }
                try { compContTransf.TERCWS = row["Importe3"].ToString(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.TERCWS = ""; 
                }
                try { compContTransf.CDIVWS = row["Iva"].ToString().ToUpper(); }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    compContTransf.CDIVWS = ""; 
                }

                //Insertar detalle del comprobante
                if (compContTransf.extendido)
                {
                    try { compContTransf.PRFDWS = row["PrefijoDoc"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.PRFDWS = ""; 
                    }
                    try { compContTransf.NFAAWS = row["NumFactAmp"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.NFAAWS = ""; 
                    }
                    try { compContTransf.NFARWS = row["NumFactRectif"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.NFARWS = ""; 
                    }
                    try { compContTransf.FIVAWS = row["FechaServIVA"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.FIVAWS = "0"; 
                    }
                    if (compContTransf.FIVAWS == "") compContTransf.FIVAWS = "0";
                    try { compContTransf.USA1WS = row["CampoUserAlfa1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA1WS = ""; 
                    }
                    try { compContTransf.USA2WS = row["CampoUserAlfa2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA2WS = ""; 
                    }
                    try { compContTransf.USA3WS = row["CampoUserAlfa3"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA3WS = ""; 
                    }
                    try { compContTransf.USA4WS = row["CampoUserAlfa4"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA4WS = ""; 
                    }
                    try { compContTransf.USA5WS = row["CampoUserAlfa5"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA5WS = ""; 
                    }
                    try { compContTransf.USA6WS = row["CampoUserAlfa6"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA6WS = ""; 
                    }
                    try { compContTransf.USA7WS = row["CampoUserAlfa7"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA7WS = ""; 
                    }
                    try { compContTransf.USA8WS = row["CampoUserAlfa8"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USA8WS = ""; 
                    }
                    try { compContTransf.USN1WS = row["CampoUserNum1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USN1WS = ""; 
                    }
                    try { compContTransf.USN2WS = row["CampoUserNum2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USN2WS = ""; 
                    }
                    try { compContTransf.USF1WS = row["CampoUserFecha1"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USF1WS = "0"; 
                    }
                    if (compContTransf.USF1WS == "") compContTransf.USF1WS = "0";
                    try { compContTransf.USF2WS = row["CampoUserFecha2"].ToString(); }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));
                        compContTransf.USF2WS = "0"; 
                    }
                    if (compContTransf.USF2WS == "") compContTransf.USF2WS = "0";
                }

                result = compContTransf.TransferirDetalle(esReversion);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error transfiriendo el detalle del comprobante (" + ex.Message + ")"; //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Actualiza los controles con los valores de la última petición
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MCCTRFICOM myStruct = (StructGLL01_MCCTRFICOM)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCCTRFICOM));

                if (myStruct.todos == "1")
                {
                    this.rbEstadoTodos.Checked = true;
                    this.rbEstadoNoTrans.Checked = false;
                }
                else
                {
                    this.rbEstadoNoTrans.Checked = true;
                    this.rbEstadoTodos.Checked = false;
                }

                this.txtDescripcion.Text = myStruct.descripcion.Trim();
                this.txtPrefijo.Text = myStruct.prefijo.Trim();
                this.txtBibliotecaPrefijo.Text = myStruct.bilbiotecaPrefijo.Trim();
                this.txtCola.Text = myStruct.colaSalida.Trim();
                this.txtBibliotecaCola.Text = myStruct.bibliotecaColaSalida.Trim();

                if (myStruct.generarLoteBatch == "1")
                {
                    this.rbGenerarLote.Checked = true;
                    this.rbGenerarLoteAdiciona.Checked = false;
                    this.cmbEstado.SelectedIndex = -1;
                    this.cmbEstado.Enabled = false;
                }

                if (myStruct.generarLoteAdiciona == "1")
                {
                    this.rbGenerarLote.Checked = false;
                    this.rbGenerarLoteAdiciona.Checked = true;
                    try
                    {
                        if (myStruct.estado.Trim() != "") this.cmbEstado.SelectedValue = myStruct.estado.Trim();
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    this.cmbEstado.Enabled = true;
                }

                if (myStruct.verNumerosComp == "1") this.chkVerNocomp.Checked = true;
                else this.chkVerNocomp.Checked = false;

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
                StructGLL01_MCCTRFICOM myStruct;

                if (this.rbEstadoTodos.Checked) myStruct.todos = "1";
                else myStruct.todos = "0";

                myStruct.descripcion = this.txtDescripcion.Text.PadRight(36, ' ');
                myStruct.prefijo = this.txtPrefijo.Text.PadRight(2, ' ');
                myStruct.bilbiotecaPrefijo = this.txtBibliotecaPrefijo.Text.PadRight(10, ' ');
                myStruct.colaSalida = this.txtCola.Text.PadRight(10, ' ');
                myStruct.bibliotecaColaSalida = this.txtBibliotecaCola.Text.PadRight(10, ' ');

                if (this.rbGenerarLote.Checked)
                {
                    myStruct.generarLoteBatch = "1";
                    myStruct.generarLoteAdiciona = "0";
                    myStruct.estado = " ";
                }
                else
                {
                    myStruct.generarLoteBatch = "0";
                    myStruct.generarLoteAdiciona = "1";
                    myStruct.estado = this.cmbEstado.SelectedValue.ToString();
                }

                if (this.chkVerNocomp.Checked) myStruct.verNumerosComp = "1";
                else myStruct.verNumerosComp = "0";

                result = myStruct.todos + myStruct.descripcion + myStruct.prefijo + myStruct.bilbiotecaPrefijo;
                result += myStruct.colaSalida + myStruct.bibliotecaColaSalida + myStruct.generarLoteBatch + myStruct.generarLoteAdiciona;
                result += myStruct.estado + myStruct.verNumerosComp;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCCTRFICOM));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //if (result.Length < 145) result.PadRight(145, ' ');

            return (result);
        }  
        #endregion       

    }
}
