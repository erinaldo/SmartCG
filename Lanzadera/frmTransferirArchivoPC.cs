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
using System.Runtime.InteropServices;
using ObjectModel;

namespace FinanzasNet
{
    public partial class frmTransferirArchivoPC : frmPlantilla, IReLocalizable
    {
        public string formCode = "MLTRAARPC";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MLTRAARPC
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string archivo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string miembro;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
            public string biblioteca;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string pathFile;
        }

        FormularioValoresCampos valoresFormulario;

        public frmTransferirArchivoPC()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmTransferirArchivoPC_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Transferir archivo a PC");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                bool result = this.CargarValoresUltimaPeticion(valores);
            }

            this.txtArchivo.Select();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransferirArchivoPC_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Transferir archivo a PC");
        }

        private void frmTransferirArchivoPC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                toolStripButtonSalir_Click(sender, null);
            }
        }

        private void btnSelDestino_Click(object sender, EventArgs e)
        {

            this.saveFileDialogTransferir = new SaveFileDialog();
            this.saveFileDialogTransferir.Title = "Archivo destino";      //Falta traducir
            this.saveFileDialogTransferir.OverwritePrompt = false;

            //Recuperar el directorio por defecto que está en la configuarción
            this.saveFileDialogTransferir.DefaultExt = "xml";
            this.saveFileDialogTransferir.Filter = "ficheros txt (*.txt)|*.txt";    //Falta traducir

            if (DialogResult.OK == this.saveFileDialogTransferir.ShowDialog())
            {
                this.txtPathFile.Text = this.saveFileDialogTransferir.FileName;

                this.lblResult.Visible = false;
                this.lnkFile.Visible = false;
            }
        }

        private void toolStripButtonTransferir_Click(object sender, EventArgs e)
        {
            IDataReader dr = null;

            try
            {
                this.lblResult.Visible = false;
                this.lnkFile.Visible = false;

                if (this.FormValid())
                {
                    string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

                    string error = "";
                    if (tipoBaseDatosCG == "DB2") dr = this.ObtenerDatosDB2(ref error);
                    else dr = this.ObtenerDatos(ref error);

                    if (error != "")
                    {
                        MessageBox.Show(error, this.LP.GetText("errValTitulo", "Error"));
                        return;
                    }

                    if (dr != null)
                    {
                        //Chquear que el archivo sólo puede tener un campo
                        int posicionCampo = 0;
                        int cantidadCampos = dr.FieldCount;
                        if (tipoBaseDatosCG != "DB2")
                        {
                            if (cantidadCampos > 0) cantidadCampos = cantidadCampos - 1;
                            posicionCampo = 1;
                        }

                        if (cantidadCampos != 1)
                        {
                            MessageBox.Show("El archivo sólo puede tener un campo", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
                            return;
                        }
                        
                        //Copiar las líneas al fichero
                        using (StreamWriter sw = new StreamWriter(this.txtPathFile.Text))
                        {
                            int contador = 0;
                            while (dr.Read())
                            {
                                //Escribir las filas en el fichero
                                sw.WriteLine(dr.GetValue(posicionCampo));
                                contador++;
                            }

                            dr.Close();

                            if (contador == 1) this.lblResult.Text = "Se ha transferido 1 registro "; //Falta traducir
                            else this.lblResult.Text = "Se han transferido " + contador + " registros"; //Falta traducir
                             
                            this.lblResult.Visible = true;
                            this.lnkFile.Visible = true;
                        }

                        //Grabar la petición
                        string valores = this.ValoresPeticion();

                        this.valoresFormulario.GrabarParametros(formCode, valores);
                    }
                    else
                    {
                        MessageBox.Show("El archivo no tiene datos", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (dr != null) dr.Close();

                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show("Error transfiriendo archivo (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
            }
        }

        private void lnkFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(this.txtPathFile.Text);

                System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void toolStripButtonGrabar_Click(object sender, EventArgs e)
        {
            if (this.txtArchivo.Text.Trim() == "" && this.txtMiembro.Text.Trim() == "" &&
                this.txtBiblioteca.Text.Trim() == "" && this.txtPathFile.Text.Trim() == "")
            {
                MessageBox.Show("No existe información para ser grabada", this.LP.GetText("errValTitulo", "Error"));  //Falta traducir
                return;
            }

            this.saveFileDialogTransferir = new SaveFileDialog();
            this.saveFileDialogTransferir.Title = "Grabar";      //Falta traducir
            this.saveFileDialogTransferir.DefaultExt = "xmlt";
            this.saveFileDialogTransferir.FileName = "archivo";
            this.saveFileDialogTransferir.DefaultExt = "xmlt";
            this.saveFileDialogTransferir.Filter = "ficheros xmlt (*.xml)|*.xmlt";

            if (DialogResult.OK == this.saveFileDialogTransferir.ShowDialog())
            {
                string result = this.GrabarTransferirArchivoPC(this.saveFileDialogTransferir.FileName);

                if (result == "") MessageBox.Show("El fichero se grabó correctamente");    //Falta traducir
                else MessageBox.Show(result, this.LP.GetText("errValTitulo", "Error"));
            }
        }

        private void toolStripButtonCargar_Click(object sender, EventArgs e)
        {
            this.openFileDialogTransferirConfig.Title = this.LP.GetText("lblConfigSelPath", "Seleccionar un archivo");    //Falta traducir
            this.openFileDialogTransferirConfig.DefaultExt = "xmlt";
            this.openFileDialogTransferirConfig.Filter = "ficheros xmlt (*.xml)|*.xmlt";
            this.openFileDialogTransferirConfig.CheckFileExists = true;
            this.openFileDialogTransferirConfig.CheckPathExists = true;

            if (DialogResult.OK == this.openFileDialogTransferirConfig.ShowDialog())
            {
                string fichero = this.openFileDialogTransferirConfig.FileName;
                //Cargar el fichero
                string result = LeerTransferirArchivoPC(fichero);

                if (result != "") MessageBox.Show("El fichero no se cargó correctamente", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
            }
        }

        private void txtArchivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtMiembro_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtBiblioteca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            this.Text = this.LP.GetText("lblfrmTransferirArchivoPC", "Transferir Archivo a PC");

            //Falta traducir la mayoria de los literales

            //Traducir los Literales de los ToolStrip
            this.toolStripButtonTransferir.Text = this.LP.GetText("lblfrmTransfArcPCTransferir", "Transferir");
            this.toolStripButtonGrabar.Text = this.LP.GetText("lblfrmCompContBotGrabar", "Grabar");
            this.toolStripButtonCargar.Text = this.LP.GetText("lblfrmTransfArcPCCargar", "Cargar");
            this.toolStripButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");

            //Falta traducir
            this.lblArchivo.Text = this.LP.GetText("lblfrmTransfArcPCArchivo", "Archivo");
            this.lblArchivo.Text = this.LP.GetText("lblfrmTransfArcPCMiembro", "Miembro");
            this.lblBiblioteca.Text = this.LP.GetText("lblBiblioteca", "Biblioteca");

            this.lblPathFile.Text = this.LP.GetText("lblfrmTransfArcPCRutaArc", "Ruta y Archivo");

            this.lnkFile.Text = this.LP.GetText("lblfrmTransfArcPCVisualizar", "Para visualizarlo pinche aquí");
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;

            string mensaje = "";

            if (this.txtArchivo.Text.Trim() == "")
            {
                mensaje = "- " + "El archivo no puede estar en blanco";
                this.txtArchivo.Focus();
            }

            if (this.txtBiblioteca.Text.Trim() == "")
            {
                if (mensaje != "") mensaje += "\n\r";
                mensaje = "- " + "La biblioteca no puede estar en blanco";
                this.txtBiblioteca.Focus();
            }

            if (this.txtPathFile.Text.Trim() == "")
            {
                if (mensaje != "") mensaje += "\n\r";
                mensaje += "- " + "La ruta y archivo no puede estar en blanco";
                this.txtPathFile.Focus();
            }

            if (mensaje != "")
            {
                result = false;
                MessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
            }
            else
            {
                //Validar si existe el fichero
                if (File.Exists(this.txtPathFile.Text))
                {
                    string conf = this.LP.GetText("lblConfFileOvr", "El archivo ya existe. ¿Desea reemplazarlo?");
                    DialogResult resultConf = MessageBox.Show(conf, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                    if (resultConf == DialogResult.No)
                    {
                        return (false);
                    }
                }
            }

            return (result);
        }

        /// <summary>
        /// Lee los datos de la tabla indicada (bbdd db2)
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private IDataReader ObtenerDatosDB2(ref string error)
        {
            IDataReader dr = null;

            try
            {
                //sCmd = "OVRDBF FILE(" & txtArchivo.Text & ") TOFILE(" & txtlib.Text & "/" & txtArchivo.Text & ") MBR(" & RTrim(txtMiembro.Text) & ") OVRSCOPE(*JOB)"
                //string comando = "OVRDBF FILE(CSW10) TOFILE(CGDATALIS/CSW10) MBR(CSWS105556) OVRSCOPE(*JOB)";
                
                string archivo = this.txtArchivo.Text.Trim();
                string miembro = this.txtMiembro.Text.Trim();
                string biblioteca = this.txtBiblioteca.Text.Trim();

                //?? Duda puede no tener miembro ===
                string comando = "OVRDBF FILE(" + archivo + ") TOFILE(" + biblioteca + "/" + archivo + ") MBR(" + miembro + ") OVRSCOPE(*JOB)";
                string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";

                string longitudComando = comando.Length.ToString();

                sentencia = sentencia + longitudComando.PadLeft(10, '0');
                sentencia = sentencia + ".00000)";

                GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);

                string query = "select * from " + archivo;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                error = "Error obteniendo los datos (" + ex.Message + ")";    //Falta traducir
            }

            return (dr);
        }

        /// <summary>
        /// Lee los datos de la tabla indicada (bbdd SQL Server, Oracle)
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private IDataReader ObtenerDatos(ref string error)
        {
            IDataReader dr = null;

            try
            {
                string archivo = this.txtArchivo.Text.Trim();
                string miembro = this.txtMiembro.Text.Trim();
                string biblioteca = this.txtBiblioteca.Text.Trim();

                string tabla = biblioteca + "_" + archivo + "_" + miembro;

                string query = "select * from " + tabla;
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                error = "Error obteniendo los datos (" + ex.Message + ")";    //Falta traducir
            }

            return (dr);
        }

        /// <summary>
        /// Lee un un fichero que contiene la información de la búsqueda para la transferencia
        /// </summary>
        /// <param name="ficheroEntorno">Nombre del fichero que contiene la información de la búsqueda para la transferencia</param>
        /// <returns></returns>
        public string LeerTransferirArchivoPC(string ficheroTransferirArchivoPC)
        {
            string result = "";

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(ficheroTransferirArchivoPC);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables["config"].Rows.Count > 0)
                {
                    //Comprobar que tenga la estructura deseada   FALTA !!

                    try { this.txtArchivo.Text = ds.Tables["config"].Rows[0]["tabla"].ToString().Trim(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.txtMiembro.Text = ds.Tables["config"].Rows[0]["miembro"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.txtBiblioteca.Text = ds.Tables["config"].Rows[0]["bbdd"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                    try { this.txtPathFile.Text = ds.Tables["config"].Rows[0]["pathFile"].ToString(); }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                result = "Error cargando el fichero (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Graba un fichero que contiene la información de la búsqueda para la transferencia
        /// </summary>
        /// <returns></returns>
        public string GrabarTransferirArchivoPC(string ficheroTransferirArchivoPC)
        {
            string result = "";

            try
            {
                //Construir el DataSet
                DataSet ds = new DataSet();
                ds.DataSetName = "TransferirArchivoPC";

                DataTable dt = new DataTable();
                dt.TableName = "config";

                //Adicionar las columnas al DataTable               
                DataColumn col = new DataColumn("tabla", typeof(System.String));
                dt.Columns.Add(col);
                col = new DataColumn("miembro", typeof(System.String));
                dt.Columns.Add(col);
                col = new DataColumn("bbdd", typeof(System.String));
                dt.Columns.Add(col);
                col = new DataColumn("pathFile", typeof(System.String));
                dt.Columns.Add(col);

                ds.Tables.Add(dt);

                //Inicializar el DataSet con los valores del formulario
                DataRow dr = dt.NewRow();
                dr["tabla"] = this.txtArchivo.Text;
                dr["miembro"] = this.txtMiembro.Text;
                dr["bbdd"] = this.txtBiblioteca.Text;
                dr["pathFile"] = this.txtPathFile.Text;

                dt.Rows.Add(dr);

                dt.WriteXml(ficheroTransferirArchivoPC);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                result = "Error grabando el fichero (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Carga los valores de la última petición
        /// </summary>
        /// <returns></returns>
        private bool CargarValoresUltimaPeticion(string valores)
        {
            bool result = false;

            try
            {
                IntPtr pBuf = Marshal.StringToBSTR(valores);
                StructGLL01_MLTRAARPC myStruct = (StructGLL01_MLTRAARPC)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MLTRAARPC));

                if (myStruct.archivo.Trim() != "") this.txtArchivo.Text = myStruct.archivo.Trim();
                if (myStruct.miembro.Trim() != "") this.txtMiembro.Text = myStruct.miembro.Trim();
                if (myStruct.biblioteca.Trim() != "") this.txtBiblioteca.Text = myStruct.biblioteca.Trim();
                if (myStruct.pathFile.Trim() != "") this.txtPathFile.Text = myStruct.pathFile.Trim();

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
                StructGLL01_MLTRAARPC myStruct;

                myStruct.archivo = this.txtArchivo.Text.PadRight(10, ' ');
                myStruct.miembro = this.txtMiembro.Text.PadRight(10, ' ');
                myStruct.biblioteca = this.txtBiblioteca.Text.PadRight(10, ' ');
                myStruct.pathFile = this.txtPathFile.Text.PadRight(260, ' ');

                result = myStruct.archivo + myStruct.miembro + myStruct.biblioteca + myStruct.pathFile;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion        
    }
}