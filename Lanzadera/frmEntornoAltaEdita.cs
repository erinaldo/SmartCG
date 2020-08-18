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
using ObjectModel;

namespace SmartCG
{
    public partial class frmEntornoAltaEdita : frmPlantilla, IReLocalizable
    {
        private bool nuevo;
        private bool copiar;
        private int indice;

        private string archivo;

        //private DataRow rowEntorno;

        private Entorno entornoActual;

        #region Properties
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

        public bool Copiar
        {
            get
            {
                return (this.copiar);
            }
            set
            {
                this.copiar = value;
            }
        }

        public int Indice
        {
            get
            {
                return (this.indice);
            }
            set
            {
                this.indice = value;
            }
        }

        public string Archivo
        {
            get
            {
                return (this.archivo);
            }
            set
            {
                this.archivo = value;
            }
        }

        /*public DataRow RowEntorno
        {
            get
            {
                return (this.rowEntorno);
            }
            set
            {
                this.rowEntorno = value;
            }
        }*/

        public Entorno EntornoActual
        {
            get
            {
                return (this.entornoActual);
            }
            set
            {
                this.entornoActual = value;
            }
        }
        #endregion

        public frmEntornoAltaEdita()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.cmbProveedorDatos.DropDownListElement.AutoSize = false;
            this.cmbProveedorDatos.DropDownListElement.Size = new Size(this.cmbProveedorDatos.Width, 30);
            this.cmbProveedorDatos.ListElement.ItemHeight = 30;

            this.cmbTipoBbdd.DropDownListElement.AutoSize = false;
            this.cmbTipoBbdd.DropDownListElement.Size = new Size(this.cmbProveedorDatos.Width, 30);
            this.cmbTipoBbdd.ListElement.ItemHeight = 30;

            this.radDropDownListSiiAgencia.DropDownListElement.AutoSize = false;
            this.radDropDownListSiiAgencia.DropDownListElement.Size = new Size(this.radDropDownListSiiAgencia.Width, 30);
            this.radDropDownListSiiAgencia.ListElement.ItemHeight = 30;

            this.radDropDownListSiiEntorno.DropDownListElement.AutoSize = false;
            this.radDropDownListSiiEntorno.DropDownListElement.Size = new Size(this.radDropDownListSiiEntorno.Width, 30);
            this.radDropDownListSiiEntorno.ListElement.ItemHeight = 30;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmEntornoAltaEdita_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Alta / Editar Entorno");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Cargar el desplegable de Tipos de Proveedores
            this.FillTipoAcceso();

            //Cargar el desplegable de Tipos de Accesos
            this.FillTipo();

            //Cargar el desplegable de Sii Agencias
            this.FillSiiAgencia();

            //Cargar el desplegable de Sii Entornos
            this.FillSiiEntorno();

            if (this.nuevo)
            {
                this.entornoActual = new Entorno();
                this.entornoActual.InicializarEntorno(false);
                this.entornoActual.AdicionarEntornoLista(this.entornoActual);
            }
            else
            {
                if (this.copiar) this.txtNombre.Text = "";
                else this.txtNombre.Text = this.entornoActual.Nombre;

                //this.entornoActual.ProveedorDatos = this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString();
                //this.entornoActual.TipoBaseDatos = this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString();
                this.txtIPNombreServer.Text = this.entornoActual.IPoNombreServidor;
                this.txtNombreBBDD.Text = this.entornoActual.NombreBBDD;
                this.txtCadenaConexion.Text = this.entornoActual.CadenaConexion;
                this.txtDSNDefault.Text = this.entornoActual.UserDSN;
                this.txtUserServidor.Text = this.entornoActual.UserNameServidor;
                this.txtUserApp.Text = this.entornoActual.UserNameApp;
                this.txtPrefijoTabla.Text = this.entornoActual.PrefijoTabla;
                this.txtBibliotecaCGAPP.Text = this.entornoActual.BbddCGAPP;
                this.txtBibliotecaCGUF.Text = this.entornoActual.BbddCGUF;
                this.txtUserCGIFS.Text = this.entornoActual.UserCGIFS;
                try
                {
                    this.radDropDownListSiiAgencia.SelectedValue = this.entornoActual.SiiAgencia;
                    this.radDropDownListSiiEntorno.SelectedValue = this.entornoActual.SiiEntorno;
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex));  }

                //this.entornoActual.DTEntorno.Rows.Clear();   
            }

            this.TraducirLiterales();

            // Actualiza el atributo TAG de los controles al valor actual de los controles
            this.ActualizaValoresOrigenControles();
        }
        
        private void CmbProveedorDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDSNDefault.Text = "";
            if (this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.txtDSNDefault.Enabled = true;
            }
            else
            {
                this.txtDSNDefault.Enabled = false;
            }
        }

        private void CmbTipoBbdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void CmbTipoBbdd_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString() == ProveedorDatos.DBTipos.DB2.ToString())
            {
                if (this.txtBibliotecaCGAPP.Text == "") this.txtBibliotecaCGAPP.Text = "CGAPP";
                if (this.txtBibliotecaCGUF.Text == "") this.txtBibliotecaCGUF.Text = "CGUF";

                this.txtBibliotecaCGAPP.Enabled = true;
                this.txtBibliotecaCGUF.Enabled = true;
                this.txtPrefijoTabla.Text = "";
            }
            else
            {
                this.txtBibliotecaCGAPP.Text = "";
                this.txtBibliotecaCGUF.Text = "";
                this.txtBibliotecaCGAPP.Tag = "";
                this.txtBibliotecaCGUF.Tag = "";

                this.txtBibliotecaCGAPP.Enabled = false;
                this.txtBibliotecaCGUF.Enabled = false;

                if (this.txtPrefijoTabla.Text == "") this.txtPrefijoTabla.Text = "TIGSA_";
            }
        }

        private void RadButtonSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                //Actualiza el DataRow (this.rowEntorno) con los valores del formulario
                this.ActualizarRowEntorno();

                string result;
                if (this.nuevo || this.copiar)
                {
                    result = this.AltaEntorno();
                    if (result == "")
                    {
                        this.nuevo = false;
                        this.copiar = false;
                    }
                }
                else result = this.ActualizarEntorno();

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Actualiza la lista de Entornos
                    this.ActualizarFormularioListaElementos();

                    //Cerrar el formulario
                    this.Close();
                }

                this.Close();
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonSaveAs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                this.txtNombre.Text += "COPIA";
                this.entornoActual.DTEntorno.Rows.Clear();

                //Actualiza el DataRow (this.rowEntorno) con los valores del formulario
                this.ActualizarRowEntorno();

                string result = this.AltaEntorno();
                if (result == "")
                {
                    this.nuevo = false;
                    this.copiar = false;
                }

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    MessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    this.ActualizaValoresOrigenControles();

                    //Actualiza la lista de Entornos
                    this.ActualizarFormularioListaElementos();

                    //Cerrar el formulario
                    this.Close();
                }

                this.Close();
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonCrear_Click(object sender, EventArgs e)
        {
            this.CrearCadenaConexion();
        }

        private void CmbProveedorDatos_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            this.txtDSNDefault.Text = "";
            if (this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.txtDSNDefault.Enabled = true;
            }
            else
            {
                this.txtDSNDefault.Enabled = false;
            }
        }

        private void RadButtonCrear_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCrear);
        }

        private void RadButtonCrear_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCrear);
        }

        private void RadButtonSave_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSave);
        }

        private void RadButtonSave_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSave);
        }

        private void RadButtonSaveAs_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSaveAs);
        }

        private void RadButtonSaveAs_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSaveAs);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void BtnCrear_Click(object sender, EventArgs e)
        {
            this.CrearCadenaConexion();
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmEntornoAltaEdita_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null);
        }

        private void FrmEntornoAltaEdita_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Log.Info("FIN Alta / Editar Entorno");

                if (this.txtNombre.Text.Trim() != this.txtNombre.Tag.ToString().Trim() ||
                    this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString() != this.cmbProveedorDatos.Tag.ToString() ||
                    this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString() != this.cmbTipoBbdd.Tag.ToString() ||
                    this.txtCadenaConexion.Text != this.txtCadenaConexion.Tag.ToString() ||
                    this.txtIPNombreServer.Text != this.txtIPNombreServer.Tag.ToString() ||
                    this.txtNombreBBDD.Text != this.txtNombreBBDD.Tag.ToString() ||
                    this.txtPrefijoTabla.Text != this.txtPrefijoTabla.Tag.ToString() ||
                    this.txtBibliotecaCGAPP.Text != this.txtBibliotecaCGAPP.Tag.ToString() ||
                    this.txtBibliotecaCGUF.Text != this.txtBibliotecaCGUF.Tag.ToString() ||
                    this.txtUserCGIFS.Text != this.txtUserCGIFS.Tag.ToString() ||
                    this.txtDSNDefault.Text != this.txtDSNDefault.Tag.ToString() ||
                    this.txtUserServidor.Text != this.txtUserServidor.Tag.ToString() ||
                    this.txtUserApp.Text != this.txtUserApp.Tag.ToString() ||
                    this.radDropDownListSiiAgencia.SelectedValue.ToString() != this.radDropDownListSiiAgencia.Tag.ToString() ||
                    this.radDropDownListSiiEntorno.SelectedValue.ToString() != this.radDropDownListSiiEntorno.Tag.ToString()
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
                    }
                    else e.Cancel = false;

                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            string titulo = "   " +this.LP.GetText("lblfrmEntornoAltaEditaTituloGen", "Entorno");
            titulo += " - ";
            if (this.nuevo) titulo += this.LP.GetText("lblfrmEntornoAltaEditaTituloAlta", "Alta");
            else titulo += this.LP.GetText("lblfrmEntornoAltaEditaTituloEditar", "Editar");
            this.Text = titulo;
        }

        /// <summary>
        /// Llena el combo con los Tipos de accesos a las bbdd posibles
        /// </summary>
        private void FillTipoAcceso()
        {
            int actual = 0;
            bool existe = false;

            string proveedorDatosActual = this.entornoActual.ProveedorDatos;

            foreach (ProveedorDatos.DBProveedores tipoAcceso in Enum.GetValues(typeof(ProveedorDatos.DBProveedores)))
            {
                if (!existe)
                {
                    if (tipoAcceso.ToString() != proveedorDatosActual) actual++;
                    else existe = true;
                }
                this.cmbProveedorDatos.Items.Add(tipoAcceso.ToString());
            }

            if (this.cmbProveedorDatos.Items.Count == actual) this.cmbProveedorDatos.SelectedIndex = 0;
            else this.cmbProveedorDatos.SelectedIndex = actual;

            if (this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.txtDSNDefault.Enabled = true;
            }
            else
            {
                this.txtDSNDefault.Text = "";
                this.txtDSNDefault.Enabled = false;
            }
        }

        /// <summary>
        /// Llena el combo con los Tipos de bbdd posibles
        /// </summary>
        private void FillTipo()
        {
            int actual = 0;
            bool existe = false;

            string tipoBBDDActual = this.entornoActual.TipoBaseDatos;

            foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
            {
                if (!existe)
                {
                    if (tipo.ToString() != tipoBBDDActual) actual++;
                    else existe = true;
                }
                this.cmbTipoBbdd.Items.Add(tipo.ToString());
            }

            if (this.cmbTipoBbdd.Items.Count == actual) this.cmbTipoBbdd.SelectedIndex = 0;
            else this.cmbTipoBbdd.SelectedIndex = actual;
        }

        /// <summary>
        /// Llena el combo con las Agencias del SII
        /// </summary>
        private void FillSiiAgencia()
        {
            string agenciaActual = this.entornoActual.SiiAgencia;

            try
            {
                List<string[]> l = new List<string[]> { new string[] { "A", "AEAT" },
                                                        new string[] { "C", "Canaria" },
                                                        new string[] { "V", "Foral Viscaya" } };

                this.radDropDownListSiiAgencia.DataSource = from obj in l
                                                            select new
                                                            {
                                                                Id = obj[0],
                                                                Name = obj[1]
                                                            };

                this.radDropDownListSiiAgencia.DisplayMember = "Name";
                this.radDropDownListSiiAgencia.ValueMember = "Id";


                this.radDropDownListSiiAgencia.SelectedValue = agenciaActual;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llena el combo con los Entornos del SII
        /// </summary>
        private void FillSiiEntorno()
        {
            string entornoActual = this.entornoActual.SiiEntorno;

            try
            {
                List<string[]> l = new List<string[]> { new string[] { "P", "Producción" },
                                                        new string[] { "T", "Test" } };

                this.radDropDownListSiiEntorno.DataSource = from obj in l
                                                            select new
                                                            {
                                                                Id = obj[0],
                                                                Name = obj[1]
                                                            };

                this.radDropDownListSiiEntorno.DisplayMember = "Name";
                this.radDropDownListSiiEntorno.ValueMember = "Id";


                this.radDropDownListSiiEntorno.SelectedValue = entornoActual;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = true;
          
            string error = this.LP.GetText("errValTitulo", "Error");
            string mensaje = "";

            if (this.txtNombre.Text == "")
            {
                mensaje += "- Debe indicar el nombre del entorno \n\r";   //Falta traducir   
                this.txtNombre.Focus();
            }

            string tipoAcceso = this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString();
            if (tipoAcceso != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                if (this.txtIPNombreServer.Text == "" && this.txtNombreBBDD.Text == "")
                {
                    mensaje = "- Debe indicar la IP o el nombre del servidor de base de datos y el nombre de la base de datos \n\r";
                    this.txtIPNombreServer.Focus();
                }
                else if (this.txtIPNombreServer.Text == "")
                {
                    mensaje = "- Debe indicar la IP o el nombre del servidor de base de datos \n\r";
                    this.txtIPNombreServer.Focus();
                }
                else
                    if (this.txtNombreBBDD.Text == "")
                    {
                        mensaje = "- Debe indicar el nombre de la base de datos  \n\r";
                        this.txtNombreBBDD.Focus();
                    }
            }

            if (mensaje != "")
            {
                result = false;
                MessageBox.Show(mensaje, error);
            }

            return (result);
        }

        /// <summary>
        /// Crear cadena de conexión
        /// </summary>
        private void CrearCadenaConexion()
        {
            string cadenaConexion = "";
            if (this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                cadenaConexion = "DSN=;UID=@USER;PWD=@PWD";
            }
            else
            {
                switch ((ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString()))
                {
                    case ProveedorDatos.DBProveedores.OleDb:

                        switch ((ProveedorDatos.DBTipos)Enum.Parse(typeof(ProveedorDatos.DBTipos), this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString()))
                        {
                            case ProveedorDatos.DBTipos.DB2:
                                cadenaConexion = "Provider=IBMDA400.DataSource.1;User ID=@USER;Password=@PWD;Persist Security Info=True;Transport Product=Client Access;SSL=DEFAULT;Data Source=" + this.txtIPNombreServer.Text + ";Default Collection=" + this.txtNombreBBDD.Text;
                                break;
                            case ProveedorDatos.DBTipos.SQLServer:
                                //cadenaConexion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=@USER;Password=@PWD;Data Source=" + this.txtIPNombreServer.Text + ";Initial Catalog=" + this.txtNombreBBDD.Text;
                                cadenaConexion = "Provider=SQLOLEDB.1;Persist Security Info=False;Data Source=" + this.txtIPNombreServer.Text + ";Initial Catalog=" + this.txtNombreBBDD.Text + ";Integrated Security=SSPI;User ID=@USER;Password=@PWD";
                                break;
                        }
                        break;

                    case ProveedorDatos.DBProveedores.SqlClient:
                        cadenaConexion = "User ID=@USER;Password=@PWD;Data Source=" + this.txtIPNombreServer.Text + ";Initial Catalog=" + this.txtNombreBBDD.Text;
                        break;
                }
            }


            /*
            string cadenaConexionActual = ConfigurationManager.AppSettings["cadenaConexionCG"];
            string cadenaConexionResult = "";


            switch ((ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString()))
            {
                case ProveedorDatos.DBProveedores.OleDb:
                case ProveedorDatos.DBProveedores.SqlClient:

                    try
                    {
                        string[] cadenaConexionElementos = cadenaConexionActual.Split(';');
                        string[] elementoNombreValor;
                        string elementoNombreActual = "";

                        //Ejemplos cadena conexión para el proveedor OleDb
                        //Provider=IBMDA400.DataSource.1;Password=@PWD;Persist Security Info=True;User ID=@USER;Data Source=192.100.100.110;Transport Product=Client Access;SSL=DEFAULT;Default Collection=CPNETV2"
                        //Provider=SQLOLEDB.1;Persist Security Info=False;User ID=CPNET;password=CPNET;Initial Catalog=CPNET;Data Source=BTG-ANNIA
                        for (int i = 0; i < cadenaConexionElementos.Length; i++)
                        {
                            elementoNombreValor = cadenaConexionElementos[i].Split('=');
                            if (elementoNombreValor.Length == 2)
                            {
                                elementoNombreActual = elementoNombreValor[0].Trim().ToUpper();

                                switch (elementoNombreActual)
                                {
                                    case "DATA SOURCE":
                                        cadenaConexionResult += elementoNombreValor[0] + " = " + this.txtIpNombreServidor.Text + ";";
                                        break;
                                    case "DEFAULT COLLECTION":
                                    case "INITIAL CATALOG":
                                        cadenaConexionResult += elementoNombreValor[0] + " = " + this.txtNombreBbdd.Text + ";";
                                        break;
                                    default:
                                        cadenaConexionResult += elementoNombreValor[0] + " = " + elementoNombreValor[1] + ";";
                                        break;
                                }
                            }
                        }

                        if (cadenaConexionResult != "")
                        {
                            //Eliminar la coma final
                            if (cadenaConexionResult.Substring(cadenaConexionResult.Length - 1, 1) == ";") cadenaConexionResult = cadenaConexionResult.Substring(0, cadenaConexionResult.Length - 1);
                            this.ModificarappSettings("cadenaConexionCG", cadenaConexionResult);
                        }
                    }
                    catch
                    {
                    }

                    break;
            }
             */

            this.txtCadenaConexion.Text = cadenaConexion;
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtNombre.Tag = this.txtNombre.Text;
            this.cmbProveedorDatos.Tag = this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString();
            this.cmbTipoBbdd.Tag = this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString();
            this.txtCadenaConexion.Tag = this.txtCadenaConexion.Text;
            this.txtIPNombreServer.Tag = this.txtIPNombreServer.Text;
            this.txtNombreBBDD.Tag = this.txtNombreBBDD.Text;
            this.txtPrefijoTabla.Tag = this.txtPrefijoTabla.Text;
            this.txtBibliotecaCGAPP.Tag = this.txtBibliotecaCGAPP.Text;
            this.txtBibliotecaCGUF.Tag = this.txtBibliotecaCGUF.Text;
            this.txtUserCGIFS.Tag = this.txtUserCGIFS.Text;
            this.txtDSNDefault.Tag = this.txtDSNDefault.Text;
            this.txtUserServidor.Tag = this.txtUserServidor.Text;
            this.txtUserApp.Tag = this.txtUserApp.Text;
            this.radDropDownListSiiAgencia.Tag = this.radDropDownListSiiAgencia.SelectedValue.ToString();
            this.radDropDownListSiiEntorno.Tag = this.radDropDownListSiiEntorno.SelectedValue.ToString();
        }

        /// <summary>
        /// Actualiza la lista de Entornos        /// </summary>
        private void ActualizarFormularioListaElementos()
        {
            if (Application.OpenForms["frmEntornoLista"] != null)
            {
                if (this.Owner is IFormEntorno formInterface)
                    formInterface.ActualizaListaElementos();
            }
        }

        /// <summary>
        /// Dar de alta a un nuevo entorno
        /// </summary>
        /// <returns></returns>
        private string AltaEntorno()
        {
            string result = "";
            try
            {
                this.saveFileDialogGrabar = new SaveFileDialog
                {
                    //Recuperar el directorio por defecto que está en la configuarción
                    InitialDirectory = this.entornoActual.EntornoXMLPathFichero,
                    DefaultExt = "xml",
                    //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    Filter = "ficheros xml (*.xml)|*.xml",
                    FileName = this.txtNombre.Text
                };

                //openFileDialog1.FilterIndex = 2;
                //openFileDialog1.RestoreDirectory = true;
                if (DialogResult.OK == this.saveFileDialogGrabar.ShowDialog())
                {
                    string nombre = System.IO.Path.GetFileName(this.saveFileDialogGrabar.FileName);
                    this.entornoActual.FicheroEntorno = nombre;
                    result = this.entornoActual.AdicionarEntornoLista(this.entornoActual);
                    //result = this.EscribirEntorno(this.saveFileDialogGrabar.FileName);
                    result = this.entornoActual.GrabarEntorno();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error dando de alta al nuevo entorno (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Actualizar un nuevo entorno
        /// </summary>
        /// <returns></returns>
        private string ActualizarEntorno()
        {
            string result = "";
            try
            {
                this.entornoActual.GrabarEntorno();
                //result = this.EscribirEntorno(this.pathFicherosEntornos + this.archivo);
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos del entorno (" + ex.Message + ")";   //Falta traducir
            }
            return (result);
        }

        /// <summary>
        /// Actualizar los valores del DataRow (this.rowEntorno) con los valores de los campos del formulario
        /// </summary>
        private void ActualizarRowEntorno()
        {
            try
            {
                this.entornoActual.FicheroEntorno = this.archivo;
                this.entornoActual.Nombre = this.txtNombre.Text;
                this.entornoActual.ProveedorDatos = this.cmbProveedorDatos.Items[this.cmbProveedorDatos.SelectedIndex].ToString();
                this.entornoActual.TipoBaseDatos = this.cmbTipoBbdd.Items[this.cmbTipoBbdd.SelectedIndex].ToString();
                this.entornoActual.IPoNombreServidor = this.txtIPNombreServer.Text;
                this.entornoActual.NombreBBDD = this.txtNombreBBDD.Text;
                this.entornoActual.CadenaConexion = this.txtCadenaConexion.Text;
                this.entornoActual.UserDSN = this.txtDSNDefault.Text;
                this.entornoActual.UserNameServidor = this.txtUserServidor.Text;
                this.entornoActual.UserNameApp = this.txtUserApp.Text;
                this.entornoActual.PrefijoTabla = this.txtPrefijoTabla.Text;
                this.entornoActual.BbddCGAPP = this.txtBibliotecaCGAPP.Text;
                this.entornoActual.BbddCGUF = this.txtBibliotecaCGUF.Text;
                this.entornoActual.UserCGIFS = this.txtUserCGIFS.Text;
                this.entornoActual.SiiAgencia = this.radDropDownListSiiAgencia.SelectedValue.ToString();
                this.entornoActual.SiiEntorno= this.radDropDownListSiiEntorno.SelectedValue.ToString();

                this.entornoActual.DTEntorno.Rows[0]["archivo"] = this.entornoActual.FicheroEntorno;
                this.entornoActual.DTEntorno.Rows[0]["nombre"] = this.entornoActual.Nombre;
                this.entornoActual.DTEntorno.Rows[0]["proveedorDatos"] = this.entornoActual.ProveedorDatos;
                this.entornoActual.DTEntorno.Rows[0]["tipoBaseDatos"] = this.entornoActual.TipoBaseDatos;
                this.entornoActual.DTEntorno.Rows[0]["IPoNombreServidor"] = this.entornoActual.IPoNombreServidor;
                this.entornoActual.DTEntorno.Rows[0]["nombrebbdd"] = this.entornoActual.NombreBBDD;
                this.entornoActual.DTEntorno.Rows[0]["cadenaConexion"] = this.entornoActual.CadenaConexion;
                this.entornoActual.DTEntorno.Rows[0]["lastDSNContab"] = this.entornoActual.UserDSN;
                this.entornoActual.DTEntorno.Rows[0]["lastUserContab"] = this.entornoActual.UserNameServidor;
                this.entornoActual.DTEntorno.Rows[0]["lastUserApp"] = this.entornoActual.UserNameApp;
                this.entornoActual.DTEntorno.Rows[0]["prefijoTabla"] = this.entornoActual.PrefijoTabla;
                this.entornoActual.DTEntorno.Rows[0]["bbddCGAPP"] = this.entornoActual.BbddCGAPP;
                this.entornoActual.DTEntorno.Rows[0]["bbddCGUF"] = this.entornoActual.BbddCGUF;
                this.entornoActual.DTEntorno.Rows[0]["USER_CGIFS"] = this.entornoActual.UserCGIFS;
                this.entornoActual.DTEntorno.Rows[0]["siiAgencia"] = this.entornoActual.SiiAgencia;
                this.entornoActual.DTEntorno.Rows[0]["siiEntorno"] = this.entornoActual.SiiEntorno;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
        #endregion
    }
}
