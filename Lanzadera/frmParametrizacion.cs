using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;
using System.Collections;

namespace SmartCG
{
    public partial class frmParametrizacion : frmPlantilla, IReLocalizable
    {
        private DataTable tablaIdiomas;

        public frmParametrizacion()
        {
            InitializeComponent();
        }

        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Traducir los nombres de los idiomas que aparecen en la Grid
            this.TraducirIdiomasGrid();
        }

        #region Eventos
        private void frmParametrizacion_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Configuración");

            //Llenar el combo de Tipo
            this.FillTipo();

            //Llenar el combo de Tipos de acceso
            this.FillTipoAcceso();

            //Llenar los campos de texto bases de datos si la conexión es diferente a Odbc
            this.FillBbddCamposTexto();

            //Prefijo Tabla
            this.txtPrefijo.Text = ConfigurationManager.AppSettings["prefijoTablaCG"];

            //CadenaConexion
            this.txtCadenaConexion.Text = ConfigurationManager.AppSettings["cadenaConexionCG"];

            //LLenar el Grid de idiomas
            this.FillDataGridIdiomas();

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTipoAcceso.SelectedIndex != -1)
            {
                if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.SqlClient.ToString())
                {
                    //Si la via de acceso a los datos es SqlClient, el tipo de bbdd tiene que ser SQLServer
                    int actual = 0;
                    bool existe = false;
                    foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
                    {
                        if (tipo == ProveedorDatos.DBTipos.SQLServer)
                        {
                            existe = true;
                            break;
                        }
                        else actual++;
                    }

                    if (existe) this.cmbTipo.SelectedIndex = actual;
                }
            }
        }

        private void cmbTipoAcceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.lblIpNombreServidor.Enabled = true;
                this.txtIpNombreServidor.Enabled = true;
                this.lblNombreBbdd.Enabled = true;
                this.txtNombreBbdd.Enabled = true;

                if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.SqlClient.ToString())
                {
                    //Si la via de acceso a los datos es SqlClient, el tipo de bbdd tiene que ser SQLServer
                    int actual = 0;
                    bool existe = false;
                    foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
                    {
                        if (tipo == ProveedorDatos.DBTipos.SQLServer)
                        {
                            existe = true;
                            break;
                        }
                        else actual++;
                    }

                    if (existe) this.cmbTipo.SelectedIndex = actual;
                }
            }
            else
            {
                this.lblIpNombreServidor.Enabled = false;
                this.txtIpNombreServidor.Enabled = false;
                this.lblNombreBbdd.Enabled = false;
                this.txtNombreBbdd.Enabled = false;
            }
        }

        private void btnGeneralAceptar_Click(object sender, EventArgs e)
        {
            bool error = false;
            string msgError = this.LP.GetText("errValTitulo", "Error");
            string tipoAcceso = this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString();
            if (tipoAcceso != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                if (this.txtIpNombreServidor.Text == "" && this.txtNombreBbdd.Text == "")
                {
                    MessageBox.Show(this.LP.GetText("errValIpServidorNombreBbdd", "Debe indicar la IP o el nombre del servidor de base de datos y el nombre de la base de datos"), msgError);
                    this.txtIpNombreServidor.Focus();
                    error = true;
                }
                else if (this.txtIpNombreServidor.Text == "")
                {
                    MessageBox.Show(this.LP.GetText("errValIpServidor", "Debe indicar la IP o el nombre del servidor de base de datos"), msgError);
                    this.txtIpNombreServidor.Focus();
                    error = true;
                }
                else
                    if (this.txtNombreBbdd.Text == "")
                    {
                        MessageBox.Show(this.LP.GetText("errValNombreBbdd", "Debe indicar el nombre de la base de datos"), msgError);
                        this.txtNombreBbdd.Focus();
                        error = true;
                    }
            }

            if (!error)
            {
                //Actualizar las variables de configuración del appConfig
                utiles.ModificarappSettings("proveedorDatosCG", tipoAcceso);
                utiles.ModificarappSettings("tipoBaseDatosCG", this.cmbTipo.Items[this.cmbTipo.SelectedIndex].ToString());
                utiles.ModificarappSettings("prefijoTablaCG", this.txtPrefijo.Text);

                //Cadena de conexión
                this.CrearCadenaConexion();

                //Refrescar el recuadro que muestra la cadena de conexión
                this.txtCadenaConexion.Text = ConfigurationManager.AppSettings["cadenaConexionCG"];
            }
        }

        private void btnAceptarCadenaConexion_Click(object sender, EventArgs e)
        {
            if (this.txtCadenaConexion.Text == "")
            {
                MessageBox.Show(this.LP.GetText("errValCadenaConexion", "Debe indicar la cadena de conexión"), this.LP.GetText("errValTitulo", "Error"));
                this.txtCadenaConexion.Focus();
            }
            else
            {
                utiles.ModificarappSettings("cadenaConexionCG", this.txtCadenaConexion.Text);
            }
        }

        private void dataGridIdiomas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Ocultar la columna Id del idioma
            this.dataGridIdiomas.Columns[0].Visible = false;
        }

        private void dataGridIdiomas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex == this.dataGridIdiomas.Columns["Defecto"].Index)
                {
                    //Parar de editar la celda
                    this.dataGridIdiomas.EndEdit();

                    //Permitir sólo una fila con la columna Defecto marcada
                    if ((bool)this.dataGridIdiomas.Rows[e.RowIndex].Cells["Defecto"].Value)
                    {
                        for (int i = 0; i < this.dataGridIdiomas.Rows.Count; i++)
                        {
                            if (i != e.RowIndex)
                            {
                                if ((bool)this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value)
                                {
                                    this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value = false;
                                }
                            }
                        }

                        //Marcar el idioma por defecto como activo
                        this.dataGridIdiomas.Rows[e.RowIndex].Cells["Activo"].Value = true;
                    }
                }
                else
                    if (e.ColumnIndex == this.dataGridIdiomas.Columns["Activo"].Index)
                    {
                        this.dataGridIdiomas.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void dataGridIdiomas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Columna Activo seleccionada y no es la fila Header
                if (e.RowIndex != -1 && e.ColumnIndex == 2)
                {
                    //Si columna Activo no está marcada, la columna Defecto tampoco lo podrá estar
                    if (!((bool)this.dataGridIdiomas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        this.dataGridIdiomas.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = false;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void btnIdiomaAceptar_Click(object sender, EventArgs e)
        {
            //Validar el formulario
            if (this.ValidarFormulario())
            {

                try
                {
                    string culturaIdiomaDefectoActual = ConfigurationManager.AppSettings["idioma"]; 
                    string culturaIdiomaDefecto = "";

                    IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

                    Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    IdiomaSection section = (IdiomaSection)cfg.Sections["idiomaSection"];

                    if (section != null)
                    {

                        // You need to remove the old settings object before you can replace it
                        //cfg.Sections.Remove("idiomaSection");

                        int fila = 0;

                        foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                        {
                            section.Idiomas[fila].Id = idioma.Id;
                            section.Idiomas[fila].Descripcion = idioma.Descripcion;
                            section.Idiomas[fila].Cultura = idioma.Cultura;
                            //section.Idiomas[fila].Imagen = idioma.Imagen;

                            if ((bool)this.dataGridIdiomas.Rows[fila].Cells["Activo"].Value)
                            {
                                section.Idiomas[fila].Activo = 1;
                            }
                            else section.Idiomas[fila].Activo = 0;

                            if ((bool)this.dataGridIdiomas.Rows[fila].Cells["Defecto"].Value) culturaIdiomaDefecto = idioma.Cultura;

                            fila++;
                            cfg.Save();
                        }

                    }

                    //Actualizar los valores de la sección de idiomas del app.config
                    utiles.ModificarappSettings("idioma", culturaIdiomaDefecto);

                    if (culturaIdiomaDefectoActual != culturaIdiomaDefecto)
                    {
                        //cambiar el idioma de los formularios
                        //Actualizar la variable global de idioma
                        GlobalVar.LanguageProvider = culturaIdiomaDefecto;

                        try
                        {
                            //Recargar todos los formularios abiertos
                            System.Globalization.CultureInfo nuevaCultura = new System.Globalization.CultureInfo(culturaIdiomaDefecto);
                            System.Threading.Thread.CurrentThread.CurrentUICulture = nuevaCultura;
                            foreach (Form f in Application.OpenForms)
                                if (f is IReLocalizable)
                                    ((IReLocalizable)f).ReLocalize();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    string msgError = this.LP.GetText("errValTitulo", "Error");
                    string msg = this.LP.GetText("errGenActualizandoValores", "Se ha producido un error actualizando los valores") + " (" + ex.Message + ")";
                    MessageBox.Show(msg, msgError);
                }
            }
        }

        private void frmParametrizacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Configuración");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Texto de los controles en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmParametrizacionTitulo", "Configuración");
            this.btnSalir.Text = this.LP.GetText("lblSalir", "Salir");
            this.tabPageBBDD.Text = this.LP.GetText("tabBBDD", "BBDD");
            this.tabPageIdioma.Text = this.LP.GetText("tabIdiomas", "Idiomas");
            this.groupBoxBBDD.Text = this.LP.GetText("groupBoxGeneralBbdd", "Bbdd Contabilidad");
            this.lblTipoBbdd.Text = this.LP.GetText("lblGeneralTipo", "Tipo") + ":";
            this.lblTipoAcceso.Text = this.LP.GetText("lblGeneralTipoAcceso", "Tipo de acceso") + ":";
            this.lblIpNombreServidor.Text = this.LP.GetText("lblGeneralIPNombreServidor", "IP o nombre del servidor") + ":";
            this.lblNombreBbdd.Text = this.LP.GetText("lblGeneralNombreBbdd", "Nombre bbdd") + ":";
            this.lblPrefijo.Text = this.LP.GetText("lblGeneralPrefijoTablas", "Prefijo tablas bbdd") + ":";
            this.btnGeneralAceptar.Text = this.LP.GetText("lblGeneralAceptar", "Aceptar");
            this.groupBoxBBDD.Text = this.LP.GetText("groupBoxCadenaConexion", "Cadena Conexión Bbdd Contabilidad");
            this.btnGeneralAceptar.Text = this.LP.GetText("btnAceptarCadenaConexion", "Aceptar");

            this.dataGridIdiomas.Columns["Descripcion"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderIdioma", "Idioma");
            this.dataGridIdiomas.Columns["Activo"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderActivo", "Activo");
            this.dataGridIdiomas.Columns["Defecto"].HeaderText = this.LP.GetText("lblIdiomaGridHeaderDefecto", "Defecto");
            this.lblNoExistenIdiomas.Text = this.LP.GetText("lblIdiomaNoDefinidos", "No se han definido idiomas. Se trabajará con el idioma por defecto (español)");
            this.btnIdiomaAceptar.Text = this.LP.GetText("btnAceptarIdiomas", "Aceptar");
        }

        /// <summary>
        /// Traducir los nombres de los idiomas que aparecen en la Grid
        /// </summary>
        private void TraducirIdiomasGrid()
        {
            try
            {
                int fila = 0;
                string descripcion;

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    descripcion = this.LP.GetText("lblIdioma" + idioma.Cultura, idioma.Descripcion); ;
                    this.dataGridIdiomas.Rows[fila].Cells["Descripcion"].Value = descripcion;
                    
                    fila++;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Llena el combo con los Tipos de bbdd posibles
        /// </summary>
        private void FillTipo()
        {
            int actual = 0;
            string tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            bool existe = false;

            this.cmbTipo.Items.Clear();

            foreach (ProveedorDatos.DBTipos tipo in Enum.GetValues(typeof(ProveedorDatos.DBTipos)))
            {
                if (!existe)
                {
                    if (tipo.ToString() != tipoBaseDatosCG) actual++;
                    else existe = true;
                }
                this.cmbTipo.Items.Add(tipo);
            }

            this.cmbTipo.SelectedIndex = actual;
        }

        /// <summary>
        /// Llena el combo con los Tipos de accesos a las bbdd posibles
        /// </summary>
        private void FillTipoAcceso()
        {
            int actual = 0;
            string proveedorDatosCG = ConfigurationManager.AppSettings["proveedorDatosCG"];
            bool existe = false;

            this.cmbTipoAcceso.Items.Clear();

            foreach (ProveedorDatos.DBProveedores tipoAcceso in Enum.GetValues(typeof(ProveedorDatos.DBProveedores)))
            {
                if (!existe)
                {
                    if (tipoAcceso.ToString() != proveedorDatosCG) actual++;
                    else existe = true;
                }
                this.cmbTipoAcceso.Items.Add(tipoAcceso);
            }

            this.cmbTipoAcceso.SelectedIndex = actual;

            if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() != ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                this.lblIpNombreServidor.Enabled = true;
                this.txtIpNombreServidor.Enabled = true;
                this.lblNombreBbdd.Enabled = true;
                this.txtNombreBbdd.Enabled = true;
            }
            else
            {
                this.lblIpNombreServidor.Enabled = false;
                this.txtIpNombreServidor.Enabled = false;
                this.lblNombreBbdd.Enabled = false;
                this.txtNombreBbdd.Enabled = false;

                //Recuperar los valores de la cadena de conexión
            }
        }

        /// <summary>
        /// Llena los campos textos del apartado de base de datos de contabilidad
        /// </summary>
        private void FillBbddCamposTexto()
        {

            string tipoAccesoParam = ConfigurationManager.AppSettings["proveedorDatosCG"];
            string tipoAccesoFormulario = this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString();
            string odbc = ProveedorDatos.DBProveedores.Odbc.ToString();

            if (!(tipoAccesoParam == odbc || tipoAccesoFormulario == odbc))
            {
                if (this.txtIpNombreServidor.Text == "" && this.txtNombreBbdd.Text == "")
                {
                    //Leer la información de la cadena de conexión del fichero de configuración
                    string cadenaConexion = ConfigurationManager.AppSettings["cadenaConexionCG"].Trim();
                    if (cadenaConexion != "")
                    {
                        string valorDataSource = this.ObtenerValorParamCadenaConexion(cadenaConexion, "DATA SOURCE");

                        this.txtIpNombreServidor.Text = valorDataSource;

                        string valorNombreBBDD = this.ObtenerValorParamCadenaConexion(cadenaConexion, "DEFAULT COLLECTION");
                        if (valorNombreBBDD == "")
                            valorNombreBBDD = this.ObtenerValorParamCadenaConexion(cadenaConexion, "INITIAL CATALOG");

                        this.txtNombreBbdd.Text = valorNombreBBDD;
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el valor de un parámetro de la cadena de conexión
        /// </summary>
        /// <param name="cadenaConexion">Cadena de conexión</param>
        /// <param name="param">Parámetro</param>
        /// <returns></returns>
        private string ObtenerValorParamCadenaConexion(string cadenaConexion, string param)
        {
            string valorParam = "";
            try
            {
                string cadenaConexionMay = cadenaConexion.ToUpper();
                int posDataSource = cadenaConexionMay.IndexOf(param);
                if (posDataSource != -1)
                {
                    int posIgual = cadenaConexionMay.IndexOf("=", posDataSource);
                    if (posIgual != -1)
                    {
                        int posDosPuntos = cadenaConexionMay.IndexOf(";", posIgual);
                        if (posDosPuntos != -1)
                        {
                            valorParam = cadenaConexion.Substring(posIgual + 1, posDosPuntos - posIgual - 1);
                        }
                        else
                        {
                            valorParam = cadenaConexion.Substring(posIgual + 1, cadenaConexion.Length - posIgual - 1);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            return (valorParam);
        }

        private void CrearCadenaConexion()
        {
            string cadenaConexion = "";
            if (this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString() == ProveedorDatos.DBProveedores.Odbc.ToString())
            {
                cadenaConexion = "DSN=;UID=@USER;PWD=@PWD";
            }
            else
            {
                switch ((ProveedorDatos.DBProveedores)Enum.Parse(typeof(ProveedorDatos.DBProveedores), this.cmbTipoAcceso.Items[this.cmbTipoAcceso.SelectedIndex].ToString()))
                {
                    case ProveedorDatos.DBProveedores.OleDb:

                        switch ((ProveedorDatos.DBTipos)Enum.Parse(typeof(ProveedorDatos.DBTipos), this.cmbTipo.Items[this.cmbTipo.SelectedIndex].ToString()))
                        {
                            case ProveedorDatos.DBTipos.DB2:
                                cadenaConexion = "Provider=IBMDA400.DataSource.1;Password=@PWD;Persist Security Info=True;User ID=@USER;Transport Product=Client Access;SSL=DEFAULT;Data Source=" + this.txtIpNombreServidor.Text + ";Default Collection=" + this.txtNombreBbdd.Text;
                                break;
                            case ProveedorDatos.DBTipos.SQLServer:
                                cadenaConexion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=@USER;Password=@PWD;Data Source=" + this.txtIpNombreServidor.Text + ";Initial Catalog=" + this.txtNombreBbdd.Text;
                                break;
                        }
                        break;

                    case ProveedorDatos.DBProveedores.SqlClient:
                        cadenaConexion = "User ID=@USER;Password=@PWD;Data Source=" + this.txtIpNombreServidor.Text + ";Initial Catalog=" + this.txtNombreBbdd.Text;
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

            utiles.ModificarappSettings("cadenaConexionCG", cadenaConexion);
        }

        /// <summary>
        /// Llena el DataGrid de idiomas a partir de la sección de idiomas del app.config
        /// </summary>
        private void FillDataGridIdiomas()
        {
            try
            {
                string idiomaDefecto = ConfigurationManager.AppSettings["idioma"];

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

                this.tablaIdiomas = new DataTable("Idiomas");
                this.tablaIdiomas.Columns.Add("Id", typeof(Int16));
                this.tablaIdiomas.Columns.Add("Descripcion", typeof(String));
                this.tablaIdiomas.Columns.Add("Activo", typeof(bool));
                this.tablaIdiomas.Columns.Add("Defecto", typeof(bool));
                string cultura = "";
                DataRow row;

                CheckBox colCheckboxActivo = new CheckBox();
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    row = this.tablaIdiomas.NewRow();
                    cultura = idioma.Cultura;
                    row["Id"] = idioma.Id;
                    row["Descripcion"] = this.LP.GetText("lblIdioma" + cultura, idioma.Descripcion);
                    if (idioma.Activo == 1) row["Activo"] = true;
                    else row["Activo"] = false;

                    if (idiomaDefecto != null && idiomaDefecto != "")
                    {
                        if (cultura == idiomaDefecto)
                        {
                            row["Defecto"] = true;
                        }
                        else row["Defecto"] = false;
                    }
                    else row["Defecto"] = false;

                    this.tablaIdiomas.Rows.Add(row);
                }

                this.dataGridIdiomas.DataSource = this.tablaIdiomas;

                if (idiomaSection.Idiomas.Count > 0)
                {
                    // Set a cell padding to provide space for the top of the focus  
                    // rectangle and for the content that spans multiple columns
                    int CUSTOM_CONTENT_HEIGHT = 15;
                    Padding newPadding = new Padding(0, 1, 0, CUSTOM_CONTENT_HEIGHT);
                    this.dataGridIdiomas.RowTemplate.DefaultCellStyle.Padding = newPadding;

                    //Ajustar columnas al tamaño de la ventana
                    this.dataGridIdiomas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    //this.dataGridIdiomas.Dock = DockStyle.Fill;

                    this.dataGridIdiomas.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    this.dataGridIdiomas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Adjust the row heights to accommodate the normal cell content. 
                    this.dataGridIdiomas.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

                    //Estilo para la fila header
                    this.dataGridIdiomas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                    DataGridViewCellStyle style = this.dataGridIdiomas.ColumnHeadersDefaultCellStyle;
                    style.BackColor = Color.Navy;
                    style.ForeColor = Color.White;
                    style.Font = new Font(this.dataGridIdiomas.Font, FontStyle.Bold);

                    //this.dataGridIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    this.dataGridIdiomas.Columns["Descripcion"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //De la fila Header, centrar el texto de las columnas de los checkbox
                    this.dataGridIdiomas.Columns["Activo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridIdiomas.Columns["Defecto"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //La columna descripción es de solo lectura
                    this.dataGridIdiomas.Columns["Descripcion"].ReadOnly = true;

                    this.dataGridIdiomas.Visible = true;
                    this.btnIdiomaAceptar.Visible = true;
                    this.lblNoExistenIdiomas.Visible = false;
                }
                else
                {
                    this.dataGridIdiomas.Visible = false;
                    this.btnIdiomaAceptar.Visible = false;
                    this.lblNoExistenIdiomas.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Validar si el formualario es correcto para grabar los valores
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormulario()
        {
            bool result = true;

            bool existeActivo = false;
            bool existeDefecto = false;

            string msgError = this.LP.GetText("errValTitulo", "Error");
            try
            {
                //Verificar que exista al menos un idioma activo y uno por defecto
                for (int i = 0; i < this.dataGridIdiomas.Rows.Count; i++)
                {
                    if (existeActivo && existeDefecto) break;

                    if ((bool)this.dataGridIdiomas.Rows[i].Cells["Activo"].Value) existeActivo = true;
                    if ((bool)this.dataGridIdiomas.Rows[i].Cells["Defecto"].Value) existeDefecto = true;
                }

                if (!existeActivo)
                {
                    MessageBox.Show(this.LP.GetText("errValIdiomasActivos", "Debe indicar al menos un idioma activo"), msgError);
                    result = false;
                }
                else
                {
                    if (!existeDefecto)
                    {
                        MessageBox.Show(this.LP.GetText("errValIdiomaDefecto", "Debe indicar el idioma por defecto"), msgError);
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msg = this.LP.GetText("errValIdiomaDefecto", "Se ha producido un error validando el formulario") + " (" + ex.Message + ")";
                MessageBox.Show(msg, msgError);
                result = false;
            }

            if (!result) this.dataGridIdiomas.Focus();

            return (result);
        }
        #endregion
    }
}
