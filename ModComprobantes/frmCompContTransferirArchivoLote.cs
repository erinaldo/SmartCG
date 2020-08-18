using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using ObjectModel;

namespace ModComprobantes
{
    public partial class frmCompContTransferirArchivoLote : frmPlantilla, IReLocalizable
    {
        public string formCode = "MCCTRARLOT";
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StructGLL01_MCCTRARLOT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string formatoAmpliado;
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

        private string tipoBaseDatosCG = "";

        private string prefijoLote = "";

        //private string ficheroCompNombre = "";

        private string ficheroDetalle = "";

        ComprobanteContableTransferir compContTransf;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GLW00
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TTRAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CCIAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string ANOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string LAPSWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TICOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string NUCOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TVOUWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string DIAEWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string MESEWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string ANOEWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string TASCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TIMOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string STATWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string DOCRWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string DOCDWS;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GLW01
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TTRAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CCIAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string ANOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string LAPSWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TICOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string NUCOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string CUENWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string CAUXWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string DESCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string MONTWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TMOVWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string MOSMWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CLDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
            public string NDOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string FDOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string FEVEWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TEINWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string NNITWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string AUA1WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string AUA2WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CDDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string NDDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string TERCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CDIVWS;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GLW11
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TTRAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CCIAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string ANOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string LAPSWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TICOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string NUCOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string CUENWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string CAUXWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string DESCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string MONTWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string TMOVWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string MOSMWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CLDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
            public string NDOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string FDOCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string FEVEWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string TEINWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string NNITWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string AUA1WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string AUA2WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CDDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string NDDOWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string TERCWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string CDIVWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string PRFDWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string NFAAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string NFARWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string FIVAWS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string USA1WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string USA2WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string USA3WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string USA4WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string USA5WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string USA6WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string USA7WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
            public string USA8WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string USN1WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string USN2WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string USF1WS;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string USF2WS;
        }

        public frmCompContTransferirArchivoLote()
        {
            InitializeComponent();

            this.gbLote.ElementTree.EnableApplicationThemeName = false;
            this.gbLote.ThemeName = "ControlDefault";

            this.gbArchivo.ElementTree.EnableApplicationThemeName = false;
            this.gbArchivo.ThemeName = "ControlDefault";

            this.gbTransferencia.ElementTree.EnableApplicationThemeName = false;
            this.gbTransferencia.ThemeName = "ControlDefault";

            this.radToggleSwitchFormatoAmpliado.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchFormatoAmpliado.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContTransferirArchivoLote_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Transferir Archivos de Lote a Finanzas");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            //this.KeyPreview = true;

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Poner en el idioma correspondiente todos los literales
            this.TraducirLiterales();

            //Inicializar los desplegables
            string[] valoresCombo = new string[] { "No Aprobado/s", "Aprobado/s", "Contabilizado/s" };
            utiles.CreateRadDropDownListElement(ref this.cmbEstado, ref valoresCombo);

            //Inicializar los valores del formulario
            this.valoresFormulario = new FormularioValoresCampos();
            string valores = "";
            if (this.valoresFormulario.LeerParametros(formCode, ref valores))
            {
                if (!this.CargarValoresUltimaPeticion(valores))
                {
                    this.cmbEstado.SelectedIndex = 0;

                    this.radToggleSwitchFormatoAmpliado.Value = true;

                    this.rbGenerarLote.IsChecked = true;
                }
            }
            else
            {
                this.cmbEstado.SelectedIndex = 0;

                this.radToggleSwitchFormatoAmpliado.Value = true;

                this.rbGenerarLote.IsChecked = true;
            }

            //Crear e instanciar el objeto ComprobanteContableTransferir
            this.compContTransf = new ComprobanteContableTransferir();
            compContTransf.tipoBaseDatosCG = this.tipoBaseDatosCG;
            compContTransf.LP = this.LP;

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

        private void BtnSelCompContable_Click(object sender, EventArgs e)
        {
            this.SeleccionarFichero(this.txtPath);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RbGenerarLoteAdiciona_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLoteAdiciona.IsChecked)
            {
                this.cmbEstado.Enabled = true;
                this.rbGenerarLote.IsChecked = false;
            }
            else
            {
                this.cmbEstado.Enabled = false;
            }

        }

        private void RbGenerarLote_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLote.IsChecked)
            {
                this.rbGenerarLoteAdiciona.IsChecked = false;
                this.cmbEstado.Enabled = false;
            }
        }

        private void RadButtonTransferir_Click(object sender, EventArgs e)
        {
            this.Transferir();
        }

        private void RbGenerarLote_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLote.IsChecked)
            {
                this.rbGenerarLoteAdiciona.IsChecked = false;
                this.cmbEstado.Enabled = false;
            }
        }

        private void RbGenerarLoteAdiciona_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.rbGenerarLoteAdiciona.IsChecked)
            {
                this.cmbEstado.Enabled = true;
                this.rbGenerarLote.IsChecked = false;
            }
            else
            {
                this.cmbEstado.Enabled = false;
            }
        }

        private void RadButtonSelModelos_Click(object sender, EventArgs e)
        {
            this.SeleccionarFichero(this.txtPath);
        }

        private void RadButtonTransferir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonTransferir);
        }

        private void RadButtonTransferir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonTransferir);
        }

        private void RadButtonSelModelos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelModelos);
        }

        private void RadButtonSelModelos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelModelos);
        }

        private void FrmCompContTransferirArchivoLote_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Transferir Archivos de Lote a Finanzas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContTransferirArchivoLoteTitulo", "Transferir Archivos de Lote a Finanzas");
            this.gbArchivo.Text = "  " + this.LP.GetText("lblCompTransALArchivo", "Archivo") + "  ";
            this.gbLote.Text = "  " + this.LP.GetText("lblCompTransFinLoteBatch", "  Lote Batch  ") + "  ";
            this.lblPrefijo.Text = this.LP.GetText("lblCompTransFinPrefijo", "Prefijo");
            this.lblBibliotecaPrefijo.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinCola", "Cola de Salida");
            this.lblBiliotecaCola.Text = this.LP.GetText("lblCompTransFinBiblioteca", "Biblioteca");
            this.gbTransferencia.Text = "  " + this.LP.GetText("lblCompTransFinTipoTransf", "Tipo de Transferencia") + "  ";
            this.rbGenerarLote.Text = this.LP.GetText("lblCompTransFinGenerarLoteB", "Solo Generar Lote");
            this.rbGenerarLoteAdiciona.Text = this.LP.GetText("lblCompTransFinGenerarLoteBA", "Generar Lote y Adicionar");
            /*
            //Desplegable de Estados (en el idioma que corresponda)
            string[] literales = {this.LP.GetText("lblCompTransFinNoAprob", "No Aprobado/s"), 
                                  this.LP.GetText("lblCompTransFinAprob", "Aprobado/s"),
                                  this.LP.GetText("lblCompTransFinContab", "Contabilizado/s")};
            for (int i = 0; i < this.cmbEstado.Items.Count; i++)
            {
                //this.cmbEstado.Items[i] = (i + 1).ToString() + "-" + literales[i]; 
                this.cmbEstado.Items[i] = literales[i]; 
            }
            */
            this.chkVerNocomp.Text = this.LP.GetText("lblCompTransFinVerNoComp", "Ver los números de comprobante");
        }

        /// <summary>
        /// Seleccionar fichero
        /// </summary>
        /// <param name="txtControl"></param>
        private void SeleccionarFichero(Telerik.WinControls.UI.RadTextBoxControl txtControl)
        {
            if (this.radToggleSwitchFormatoAmpliado.Value)
            {
                this.openFileDialog1.Filter = "Archivos W10|*W10.txt";
                this.openFileDialog1.Title = this.LP.GetText("lblSelArchForAmp", "Seleccionar archivos en formato ampliado");
            }
            else
            {
                this.openFileDialog1.Filter = "Archivos W00|*W00.txt";
                this.openFileDialog1.Title = this.LP.GetText("lblSelArchForNoAmp", "Seleccionar archivos en formato no ampliado");
            }

            this.openFileDialog1.FileName = "";

            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtControl.Text = this.openFileDialog1.FileName;
                //Environment.SpecialFolder root = folder.RootFolder;
                // Assign the cursor in the Stream to the Form's Cursor property.
                //this.Cursor = new Cursor(openFileDialog1.OpenFile());
            }
        }

        private string ValidarForm()
        {
            string result = "";

            try
            {
                //Verificar que selecciono un fichero
                if (this.txtPath.Text == "")
                {
                    result += " - " + "Debe seleccionar un fichero " + "\n\r"; //Falta traducir
                    this.txtPath.Focus();
                }

                //Verificar que existe el fichero de cabecera
                if (!File.Exists(this.txtPath.Text))
                {
                    result += " - " + "El fichero no existe " + "\n\r"; //Falta traducir
                    this.txtPath.Focus();
                }
                else
                {
                    //Verificar que exista el detalle
                    string fichDetalle = this.txtPath.Text;
                    int pos = fichDetalle.IndexOf('.');

                    if (pos == -1)
                    {
                        result += " - " + "El fichero de detalle no existe " + "\n\r"; //Falta traducir
                        this.txtPath.Focus();
                        this.ficheroDetalle = "";
                    }
                    else
                    {
                        fichDetalle = fichDetalle.Substring(0, pos - 1) + "1" + fichDetalle.Substring(pos, this.txtPath.Text.Length - pos);

                        if (!File.Exists(fichDetalle))
                        {
                            result += " - " + "El fichero de detalle (" + ficheroDetalle + ") no existe " + "\n\r"; //Falta traducir
                            this.txtPath.Focus();
                            this.ficheroDetalle = "";
                        }
                        else this.ficheroDetalle = fichDetalle;
                    }
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
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error validando el formulario (" + ex.Message + ")";  //Falta traducir
            }
            return (result);
        }

        private string ProcesarComprobantes()
        {
            string result = "";
            StreamReader file = null;
            try
            {
                string linea;

                //Leer fichero cabecera
                file = new System.IO.StreamReader(this.txtPath.Text);
                while ((linea = file.ReadLine()) != null)
                {
                    if (linea != "")
                    {
                        IntPtr pBuf = Marshal.StringToBSTR(linea);
                        GLW00 myStruct = (GLW00)Marshal.PtrToStructure(pBuf, typeof(GLW00));

                        this.TransferirCabecera(myStruct);
                    }

                }

                file.Close();

                //Leer fichero detalle
                file = new System.IO.StreamReader(ficheroDetalle);
                while ((linea = file.ReadLine()) != null)
                {
                    if (linea != "")
                    {
                        IntPtr pBuf = Marshal.StringToBSTR(linea);

                        if (this.radToggleSwitchFormatoAmpliado.Value)
                        {
                            GLW01 myStruct = (GLW01)Marshal.PtrToStructure(pBuf, typeof(GLW01));

                            this.TransferirDetalle(myStruct);
                        }
                        else
                        {
                            GLW11 myStruct = (GLW11)Marshal.PtrToStructure(pBuf, typeof(GLW11));

                            this.TransferirDetalleAmpliado(myStruct);
                        }
                    }

                }

                file.Close();
                
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error procesando los comprobantes (" + ex.Message + ")";  //Falta traducir
                if (file != null) file.Close();
            }

            return (result);
        }

        private string TransferirCabecera(GLW00 glw00)
        {
            string result = "";
            
            compContTransf.TTRAWS = glw00.TTRAWS;
            compContTransf.CCIAWS = glw00.CCIAWS;
            compContTransf.ANOCWS = glw00.ANOCWS;
            compContTransf.LAPSWS = glw00.LAPSWS;
            compContTransf.TICOWS = glw00.TICOWS;
            compContTransf.NUCOWS = glw00.NUCOWS;
            compContTransf.TVOUWS = glw00.TVOUWS;
            compContTransf.DIAEWS = glw00.DIAEWS;
            compContTransf.MESEWS = glw00.MESEWS;
            compContTransf.ANOEWS = glw00.ANOEWS;
            compContTransf.TASCWS = glw00.TASCWS;
            compContTransf.TIMOWS = glw00.TIMOWS;
            compContTransf.STATWS = glw00.STATWS;
            compContTransf.DOCRWS = glw00.DOCRWS;
            compContTransf.DOCDWS = glw00.DOCDWS;

            compContTransf.prefijoLote = this.txtPrefijo.Text.Trim();
            compContTransf.bibliotecaPrefijo = this.txtBibliotecaPrefijo.Text.Trim().ToUpper();
            compContTransf.biliotecaCola = this.txtBibliotecaCola.Text.Trim().ToUpper();
            compContTransf.cola = this.txtCola.Text;
            compContTransf.descripcion = this.txtDescripcion.Text;

            compContTransf.tipoBaseDatosCG = this.tipoBaseDatosCG;
            
            if (this.radToggleSwitchFormatoAmpliado.Value) compContTransf.extendido = true;
            else compContTransf.extendido = false;

            result = compContTransf.TransferirCabecera();

            return (result);
        }

        private string TransferirDetalle(GLW01 glw01)
        {
            string result = "";

            compContTransf.TTRAWS = glw01.TTRAWS;
            compContTransf.CUENWS = glw01.CUENWS;
            compContTransf.CAUXWS = glw01.CAUXWS;
            compContTransf.DESCWS = glw01.DESCWS;
            compContTransf.MONTWS = glw01.MONTWS;
            compContTransf.TMOVWS = glw01.TMOVWS;
            compContTransf.MOSMWS = glw01.MOSMWS;
            compContTransf.CLDOWS = glw01.CLDOWS;
            compContTransf.NDOCWS = glw01.NDOCWS;
            compContTransf.FDOCWS = glw01.FDOCWS;
            compContTransf.FEVEWS = glw01.FEVEWS;
            compContTransf.TEINWS = glw01.TEINWS;
            compContTransf.NNITWS = glw01.NNITWS;
            compContTransf.AUA1WS = glw01.AUA1WS;
            compContTransf.AUA2WS = glw01.AUA2WS;
            compContTransf.CDDOWS = glw01.CDDOWS;
            compContTransf.NDDOWS = glw01.NDDOWS;
            compContTransf.TERCWS = glw01.TERCWS;
            compContTransf.CDIVWS = glw01.CDIVWS;

            result = compContTransf.TransferirDetalle();

            return (result);
        }

        private string TransferirDetalleAmpliado(GLW11 glw11)
        {
            string result = "";

            compContTransf.TTRAWS = glw11.TTRAWS;
            compContTransf.CUENWS = glw11.CUENWS;
            compContTransf.CAUXWS = glw11.CAUXWS;
            compContTransf.DESCWS = glw11.DESCWS;
            compContTransf.MONTWS = glw11.MONTWS;
            compContTransf.TMOVWS = glw11.TMOVWS;
            compContTransf.MOSMWS = glw11.MOSMWS;
            compContTransf.CLDOWS = glw11.CLDOWS;
            compContTransf.NDOCWS = glw11.NDOCWS;
            compContTransf.FDOCWS = glw11.FDOCWS;
            compContTransf.FEVEWS = glw11.FEVEWS;
            compContTransf.TEINWS = glw11.TEINWS;
            compContTransf.NNITWS = glw11.NNITWS;
            compContTransf.AUA1WS = glw11.AUA1WS;
            compContTransf.AUA2WS = glw11.AUA2WS;
            compContTransf.CDDOWS = glw11.CDDOWS;
            compContTransf.NDDOWS = glw11.NDDOWS;
            compContTransf.TERCWS = glw11.TERCWS;
            compContTransf.CDIVWS = glw11.CDIVWS;

            compContTransf.PRFDWS = glw11.PRFDWS;
            compContTransf.NFAAWS = glw11.NFAAWS;
            compContTransf.NFARWS = glw11.NFARWS;
            compContTransf.FIVAWS = glw11.FIVAWS;
            compContTransf.USA1WS = glw11.USA1WS;
            compContTransf.USA2WS = glw11.USA2WS;
            compContTransf.USA3WS = glw11.USA3WS;
            compContTransf.USA4WS = glw11.USA4WS;
            compContTransf.USA5WS = glw11.USA5WS;
            compContTransf.USA6WS = glw11.USA6WS;
            compContTransf.USA7WS = glw11.USA7WS;
            compContTransf.USA8WS = glw11.USA8WS;
            compContTransf.USN1WS = glw11.USN1WS;
            compContTransf.USN2WS = glw11.USN2WS;
            compContTransf.USF1WS = glw11.USF1WS;
            compContTransf.USF2WS = glw11.USF2WS;

            result = compContTransf.TransferirDetalle();

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
                StructGLL01_MCCTRARLOT myStruct = (StructGLL01_MCCTRARLOT)Marshal.PtrToStructure(pBuf, typeof(StructGLL01_MCCTRARLOT));

                if (myStruct.formatoAmpliado == "1") this.radToggleSwitchFormatoAmpliado.Value = true;
                else this.radToggleSwitchFormatoAmpliado.Value = false;

                this.txtDescripcion.Text = myStruct.descripcion.Trim();
                this.txtPrefijo.Text = myStruct.prefijo.Trim();
                this.txtBibliotecaPrefijo.Text = myStruct.bilbiotecaPrefijo.Trim();
                this.txtCola.Text = myStruct.colaSalida.Trim();
                this.txtBibliotecaCola.Text = myStruct.bibliotecaColaSalida.Trim();

                if (myStruct.generarLoteBatch == "1")
                {
                    this.rbGenerarLote.IsChecked = true;
                    this.rbGenerarLoteAdiciona.IsChecked = false;
                    this.cmbEstado.SelectedIndex = -1;
                    this.cmbEstado.Enabled = false;
                }

                if (myStruct.generarLoteAdiciona == "1")
                {
                    this.rbGenerarLote.IsChecked = false;
                    this.rbGenerarLoteAdiciona.IsChecked = true;
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
                StructGLL01_MCCTRARLOT myStruct;

                if (this.radToggleSwitchFormatoAmpliado.Value) myStruct.formatoAmpliado = "1";
                else myStruct.formatoAmpliado = "0";

                myStruct.descripcion = this.txtDescripcion.Text.PadRight(36, ' ');
                myStruct.prefijo = this.txtPrefijo.Text.PadRight(2, ' ');
                myStruct.bilbiotecaPrefijo = this.txtBibliotecaPrefijo.Text.PadRight(10, ' ');
                myStruct.colaSalida = this.txtCola.Text.PadRight(10, ' ');
                myStruct.bibliotecaColaSalida = this.txtBibliotecaCola.Text.PadRight(10, ' ');

                if (this.rbGenerarLote.IsChecked)
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

                result = myStruct.formatoAmpliado + myStruct.descripcion + myStruct.prefijo + myStruct.bilbiotecaPrefijo;
                result += myStruct.colaSalida + myStruct.bibliotecaColaSalida + myStruct.generarLoteBatch + myStruct.generarLoteAdiciona;
                result += myStruct.estado + myStruct.verNumerosComp;

                int objsize = Marshal.SizeOf(typeof(StructGLL01_MCCTRARLOT));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }  

        private void Transferir()
        {
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

            result = compContTransf.VerificarExistenDatosLote();
            bool procesarComp = true;
            if (result != "")
            {
                //Hay datos del lote, pedir confirmación para borrarlos
                string mensaje = result + "\n\r" + this.LP.GetText("errGrabarErroresPreg", "¿Desea eliminar los lotes?");
                DialogResult resultDialog = MessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
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
                    if (this.rbGenerarLoteAdiciona.IsChecked && compTransferidos == "")
                    {
                        //Insertar registro en la tabla GLC04
                        //this.InsertarRegistroGLC04(); 
                    }

                    if (this.chkVerNocomp.Checked)
                    {
                        if (compTransferidos != "-1")
                        {
                            if (compContTransf.numCompGenerados != null && compContTransf.numCompGenerados.Rows.Count > 0)
                            {
                                frmVisorNumComp frmVerNumComp = new frmVisorNumComp
                                {
                                    Datos = compContTransf.numCompGenerados,
                                    Extracontable = 1,
                                    FrmPadre = this
                                };
                                frmVerNumComp.ShowDialog();
                                compContTransf.numCompGenerados.Rows.Clear();
                            }
                            else
                            {
                                MessageBox.Show("El archivo se transfirió correctamente (" + this.txtPath.Text + ")", "");   //FALTA Traducir
                            }
                        }
                        else
                        {
                            //Mostrar mensaje de NO transferido
                            MessageBox.Show("El archivo no se transfirió correctamente (" + this.txtPath.Text + ")", "");   //FALTA Traducir
                        }
                    }
                    else
                    {
                        //Mostrar mensaje de transferido OK
                        MessageBox.Show("El archivo se transfirió correctamente (" + this.txtPath.Text + ")", "");   //FALTA Traducir
                    }
                }

                this.txtPath.Text = "";
            }

            //Grabar la petición
            string valores = this.ValoresPeticion();

            this.valoresFormulario.GrabarParametros(formCode, valores);
        }
        #endregion
    }
}
