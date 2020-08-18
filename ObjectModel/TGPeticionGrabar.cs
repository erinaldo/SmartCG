using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Telerik.WinControls;

namespace ObjectModel
{
    public partial class TGPeticionGrabar : Telerik.WinControls.UI.RadForm
    {
        private string archivo = "";
        private string pathPeticiones = "";

        private Utiles utiles;

        private Point _location = new Point(0, 0);
        /// <summary>
        /// Coordenadas donde se dibujará el formulario
        /// </summary>
        public Point LocationForm
        {
            get
            {
                return (this._location);
            }
            set
            {
                this._location = value;
            }
        }

        private Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
        /// </summary>
        public Form FrmPadre
        {
            get
            {
                return (this._frmPadre);
            }
            set
            {
                this._frmPadre = value;
            }
        }

        private bool _centrarForm = true;
        /// <summary>
        /// Centrar el formulario
        /// </summary>
        public bool CentrarForm
        {
            get
            {
                return (this._centrarForm);
            }
            set
            {
                this._centrarForm = value;
            }
        }

        private string formCode;
        public string FormCode
        {
            get
            {
                return (this.formCode);
            }
            set
            {
                this.formCode = value;
            }
        }
        
        private string ficheroExtension;
        public string FicheroExtension
        {
            get
            {
                return (this.ficheroExtension);
            }
            set
            {
                this.ficheroExtension = value;
            }
        }

        public TGPeticionGrabar()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        private void TGPeticionGrabar_Load(object sender, EventArgs e)
        {
            utiles = new Utiles();
            
            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales
            //this.TraducirLiterales();

            //Directorio donde se almacenan las peticiones por usuarios
            this.pathPeticiones = ConfigurationManager.AppSettings["PathFicherosPeticiones"];
            this.radTextBoxControlDirectorio.Text = this.pathPeticiones;
            this.radTextBoxControlDirectorio.Tag = this.pathPeticiones;
            if (this.ficheroExtension != "") this.radTextBoxControlExtension.Text = "." + this.ficheroExtension;
            
            utiles.ButtonEnabled(ref this.radButtonSave, false);
        }

        private void btnSelFichero_Click(object sender, EventArgs e)
        {
            try
            {
                this.saveFileDialogGrabar = new SaveFileDialog();
                //Recuperar el directorio por defecto que está en la configuarción
                this.saveFileDialogGrabar.InitialDirectory = Path.GetFullPath(this.pathPeticiones);
                this.saveFileDialogGrabar.RestoreDirectory = true;
                this.saveFileDialogGrabar.DefaultExt = "xml";
                //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                this.saveFileDialogGrabar.Filter = this.ObtenerFiltroExtensiones();
                //this.saveFileDialogGrabar.Filter = "ficheros xml (*.xml)|*.xml";
                this.saveFileDialogGrabar.FileName = "Peticion";
            
                //Si el fichero existe pedir confirmacion para reemplazarlo
                //openFileDialog1.FilterIndex = 2;
                //openFileDialog1.RestoreDirectory = true;

                this.saveFileDialogGrabar.FileOk += saveFileDialogGrabar_FileOk;

                if (DialogResult.OK == this.saveFileDialogGrabar.ShowDialog())
                {
                    string fichero = this.saveFileDialogGrabar.FileName;
                    this.archivo = this.pathPeticiones + fichero;
                    //Actualizar el nombre del archivo para la opción Grabar
                    this.txtFichero.Text = Path.GetFileName(fichero);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void saveFileDialogGrabar_FileOk(object sender, CancelEventArgs e)
        {
            if (Path.GetDirectoryName(this.saveFileDialogGrabar.FileName) != Path.GetDirectoryName(this.pathPeticiones))
            {
                RadMessageBox.Show("Los ficheros de peticiones se guardan en la carpeta: " + this.pathPeticiones);  //Falta traducir
                e.Cancel = true;
            }
        }

        private void txtFichero_TextChanged(object sender, EventArgs e)
        {
            if (this.txtFichero.Text.Trim().Length > 0) utiles.ButtonEnabled(ref this.radButtonSave, true);
            else utiles.ButtonEnabled(ref this.radButtonSave, false);
        }

        private void radButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            this.Grabar();
        }

        private void radButtonSelNombreArchivo_Click(object sender, EventArgs e)
        {
            this.radSaveFileDialogGuardar = new Telerik.WinControls.UI.RadSaveFileDialog();
            //Recuperar el directorio por defecto que está en la configuarción
            //this.radSaveFileDialogGuardar.InitialDirectory = Path.GetFullPath(this.pathPeticiones);
            this.radSaveFileDialogGuardar.RestoreDirectory = true;
            this.radSaveFileDialogGuardar.DefaultExt = "xml";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            this.radSaveFileDialogGuardar.Filter = this.ObtenerFiltroExtensiones();
            //this.saveFileDialogGrabar.Filter = "ficheros xml (*.xml)|*.xml";
            this.radSaveFileDialogGuardar.FileName = "Peticion";

            //Si el fichero existe pedir confirmacion para reemplazarlo
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;

            //this.radSaveFileDialogGuardar.FileOk += saveFileDialogGrabar_FileOk;

            string pathDirectorioAct = this.radTextBoxControlDirectorio.Text.Trim();
            if (pathDirectorioAct != "") this.radSaveFileDialogGuardar.InitialDirectory = Path.GetFullPath(pathDirectorioAct); 

            if (DialogResult.OK == this.radSaveFileDialogGuardar.ShowDialog())
            {
                string pathDirectorio = Path.GetDirectoryName(this.radSaveFileDialogGuardar.FileName);

                if (pathDirectorioAct == "")
                {
                    this.radTextBoxControlDirectorio.Text = pathDirectorio;
                }
                else
                {
                    if (pathDirectorio != pathDirectorioAct)
                    {
                        RadMessageBox.Show("Los ficheros de peticiones se guardan en la carpeta: " + pathDirectorioAct);  //Falta traducir
                        return;
                    }
                }

                string fichero = this.radSaveFileDialogGuardar.FileName;
                this.archivo = this.pathPeticiones + fichero;
                //Actualizar el nombre del archivo para la opción Grabar
                this.txtFichero.Text = Path.GetFileName(fichero);
                this.txtDescripcion.Focus();
            }
        }

        private void radButtonSelDirectorio_Click(object sender, EventArgs e)
        {
            if (this.radTextBoxControlDirectorio.Text.Trim() != "") this.radOpenFolderDialog1.InitialDirectory = this.radTextBoxControlDirectorio.Text.Trim();

            if (this.radOpenFolderDialog1.ShowDialog() == DialogResult.OK)
            {
                this.radTextBoxControlDirectorio.Text = this.radOpenFolderDialog1.FileName;
            }
        }

        private void radButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void radButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void radButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void radButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void radButtonSelNombreArchivo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelNombreArchivo);
        }

        private void radButtonSelNombreArchivo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelNombreArchivo);
        }

        private void radButtonSelDirectorio_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSelDirectorio);
        }

        private void radButtonSelDirectorio_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSelDirectorio);
        }

        private void TGPeticionGrabar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.radButtonExit_Click(sender, null);
        }
        #endregion

        #region Métodos privados
        private void TraducirLiterales()
        {
            //Falta traducir los literales
        }

        private bool FormValid()
        {
            bool result = true;

            try
            {
                if (this.radTextBoxControlDirectorio.Text.Trim() == "")
                {
                    RadMessageBox.Show("El directorio no puede estar en blanco", "Error");
                    this.radButtonSelDirectorio.Focus();
                    return (false);
                }

                if (this.txtFichero.Text.Trim() == "")
                {
                    RadMessageBox.Show("El nombre del archivo no puede estar en blanco", "Error");
                    this.txtFichero.Focus();
                    return (false);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Grabar la petición
        /// </summary>
        private void Grabar()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.FormValid())
                {
                    string pathPeticionesAct = this.radTextBoxControlDirectorio.Text.Trim();

                    FormularioPeticion frmPeticion = new FormularioPeticion();
                    frmPeticion.Path = pathPeticionesAct;
                    frmPeticion.Fichero = this.txtFichero.Text;
                    frmPeticion.FicheroExtension = this.ficheroExtension;
                    frmPeticion.FormCode = this.formCode;
                    frmPeticion.Descripcion = this.txtDescripcion.Text;
                    frmPeticion.Formulario = this.FrmPadre;

                    string result = frmPeticion.GrabarPeticion();

                    if (result == "")
                    {
                        RadMessageBox.Show("La petición se guardó correctamente");  //Falta traducir

                        //Actualizar variable directorio de peticiones si es necesario
                        if (pathPeticionesAct.ToUpper() != this.radTextBoxControlDirectorio.Tag.ToString().Trim().ToUpper())
                        {
                            utiles.ModificarappSettings("PathFicherosPeticiones", pathPeticionesAct);
                            GlobalVar.UsuarioEnv.GrabarUsuario("PathFicherosPeticiones", pathPeticionesAct);
                        }

                        this.Close();
                    }
                    else
                    {
                        RadMessageBox.Show("Error: " + result);  //Falta traducir
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Devuelve el filtro según la extensión para el formulario de dialogo de grabar
        /// </summary>
        /// <returns></returns>
        private string ObtenerFiltroExtensiones()
        {

            //string result = "Todos los ficheros (*.*)|*.*";
            string extFichero = "";

            switch (this.ficheroExtension)
            {
                case "ddt":
                    extFichero = "Informe Diario Detallado (*.dtt)|*.dtt";
                    break;
                case "drf":
                    extFichero = "Informe Diario Resumido Fecha (*.drf)|*.drf";
                    break;
                case "drp":
                    extFichero = "Informe Diario Resumido Periodo (*.drp)|*.drp";
                    break;
                case "bss":
                    extFichero = "Informe Balance Sumas y Saldos (*.bss)|*.bss";
                    break;
                case "mct":
                    extFichero = "Informe Mayor de Contabilidad (*.mct)|*.mct";
                    break;
                case "mau":
                    extFichero = "Informe Mayor de Auxiliar (*.mau)|*.mau";
                    break;
                case "miv":
                    extFichero = "Informe Movimientos de IVA (*.miv)|*.miv";
                    break;
                case "cau":
                    extFichero = "Consulta de Cuentas de Auxiliar (*.cau)|*.cau";
                    break;
                case "sis":
                    extFichero = "Suministro de Información del SII (*.sis)|*.sis";
                    break;
                case "sic":
                    extFichero = "Consulta de Datos del SII (*.sic)|*.sic";
                    break;
            }

            //if (extFichero != "") result = extFichero + "|" + result;

            return (extFichero);
        }
        #endregion
    }
}
