using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.IO;
using log4net;
using ObjectModel;

namespace FinanzasNet
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable, IFormEntorno
    {
        private const string modulosBotonesNombre = "btnModulo";
        private const string modulosPanelesNombre = "panelModulo";
        private const string modulosEtiquetasNombre = "labelModulo";

        private const string nombreFicheroAD = "AccesosDirectos.xml";

        private static int modulosActivosCant = 0;
        private string tipoBaseDatosCG;

        private DataSet dsAccesosDirectos;

        private int formWidth = 0;
        private int formHeight = 0;
        //private bool resizeEnd = false;

        private bool cargaInicial = true;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        #region Eventos
        //Deshabilita todos los controles de los módulos
        void IFormEntorno.ModulosDeshabilitar()
        {
            try
            {
                //Refrescar el valor del usuario logado en el panel
                this.RefrescarPanelInfoUsuario();

                //Habilitar los módulos
                this.ModulosHabilitarDeshabilitar(false);
                
                frmLogin frmLoginInicial = new frmLogin();
                frmLoginInicial.ShowDialog();

                //if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
                if (GlobalVar.ConexionCG != null && GlobalVar.ConexionCG.GetConnectionValue.State == ConnectionState.Open)
                {
                    //Refrescar el valor del usuario logado en el panel
                    this.RefrescarPanelInfoUsuario();
                   
                    //Habilitar los módulos
                    this.ModulosHabilitarDeshabilitar(true);
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        void IFormEntorno.ActualizaListaElementos()
        {
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo Principal (Lanzadera)");

            //Recuperar el tipo de bbdd de contabilidad
            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Método desarrollado más arriba, pasando como parámetro
            //el identificador de la ventana sobre la que vamos a actuar
            //utiles.DisableCloseButton(this.Handle.ToInt32());

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();

            //Inicial tamaño del formulario a las dimensiones de la pantalla
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            //this.Size = new Size(772, 491);

            this.formWidth = this.Size.Width;
            this.formHeight = this.Size.Height;

            //Mostrar el usuario que se ha logado si procede
            if (GlobalVar.UsuarioLogadoCG_BBDD != "")
            {
                this.RefrescarPanelInfoUsuario();
            }
            else
            {
                if (GlobalVar.EntornoActivo != null && GlobalVar.EntornoActivo.Nombre != "")
                {
                    this.lblEntorno.Text = this.LP.GetText("lblEntornoNombre", "Entorno") + ": " + GlobalVar.EntornoActivo.Nombre;      //Falta traducir
                    this.lblEntorno.Visible = true;
                }
                else
                {
                    this.lblEntorno.Text = "";
                    this.lblEntorno.Visible = false;
                }
            }

            //Bandera de idioma
            this.RefrescarPanelInfoBandera();
            
            //Construir DataSet con los Modulos
            this.FillDataSetModulos();

            //Cargar los módulos activos
            this.CargarModulosActivos();

            //Activar opción de menú que permite Transferir Archivos a PC y la de autentificar en CG, si el usuario está logado
            if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
            {
                this.subMenuItemTransferirArchivoPC.Enabled = true;
                this.subMenuItemLoginApp.Enabled = true;
            }

            //Cargar Accesos Directos
            this.CargarAccesosDirectos();           
        }

        private void frmPrincipal_Activated(object sender, EventArgs e)
        {
            //Si no existe entorno actual, no pedir las credenciales
            string entornoActual = System.Configuration.ConfigurationManager.AppSettings["entornoActual"];
            if (entornoActual == "")
            {
                this.cargaInicial = false;
                return;
            }

            if (this.cargaInicial)
            {
                this.cargaInicial = false;
                //Llamada al formulario de Login que se ejecuta siempre al inicio
                frmLogin frmLoginInicial = new frmLogin();
                frmLoginInicial.ShowDialog();

                if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
                {
                    //Refrescar el valor del usuario logado en el panel
                    this.RefrescarPanelInfoUsuario();

                    //Habilitar los módulos
                    this.ModulosHabilitarDeshabilitar(true);

                    this.btnModulo1.Focus();
                }
            }
        }

        private void MenuItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            string nombre = clickedItem.Name;
            string texto = clickedItem.Text;

            string nombreAD_XML = "";
            string programaAD_XML = "";
            if (this.dsAccesosDirectos.Tables["AD"] != null)
            {
                for (int i = 0; i < this.dsAccesosDirectos.Tables["AD"].Rows.Count; i++)
                {
                    nombreAD_XML = this.dsAccesosDirectos.Tables["AD"].Rows[i]["nombre"].ToString();
                    programaAD_XML = this.dsAccesosDirectos.Tables["AD"].Rows[i]["programa"].ToString();

                    //Buscar el acceso directo
                    if (nombreAD_XML == texto)
                    {
                        try
                        {
                            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            proc.EnableRaisingEvents = false;
                            //Invocar al programa
                            proc.StartInfo.FileName = @programaAD_XML;
                            proc.Start();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));

                            string msgError = this.LP.GetText("errValTitulo", "Error");
                            MessageBox.Show(this.LP.GetText("errAccesoDirectoLlamarApp", "Error al llamar al acceso directo") + " (" + ex.Message + ")", msgError);
                        }
                    }
                }
            }

            /*
            if (clickedItem.Text == "Calculadora")
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                //Llamar a calculadora 
                //proc.StartInfo.FileName = "calc";
                string app = @"C:\Software Install\Notepad++\notepad++.exe";
                proc.StartInfo.FileName = app;
                //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(app);
                //proc.StartInfo = psi;
                proc.Start(); 

                //Esto previene que traten de cerrar antes de que se haya abierto 
                //proc.WaitForInputIdle();
            }*/
            // Take some action based on the data in clickedItem
        }

        private void DropedDownItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            string text = e.ClickedItem.Text;
        }

        private void subMenuItemConectar_Click(object sender, EventArgs e)
        {
            frmLogin frmLoginBBDDCG = new frmLogin();
            frmLoginBBDDCG.ShowDialog();

            if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
            {
                //Refrescar el valor del usuario logado en el panel
                this.RefrescarPanelInfoUsuario();

                //Habilitar los módulos
                this.ModulosHabilitarDeshabilitar(true);
            }
        }

        private void subMenuItemLoginCG_Click(object sender, EventArgs e)
        {
            //Cargar formulario de login para CG
            frmLoginApp frmLoginAplicacionCG = new frmLoginApp();
            frmLoginAplicacionCG.ShowDialog(this);

            //Usuario no autentificado en CG
            frmLoginAplicacionCG.Dispose();
            this.RefrescarPanelInfoUsuario();
        }

        private void subMenuItemIdiomaCambiar_Click(object sender, EventArgs e)
        {
            frmIdioma frmLan = new frmIdioma();
            frmLan.ShowDialog();

            this.RefrescarPanelInfoBandera();
        }

        private void subMenuItemEntornos_Click(object sender, EventArgs e)
        {
            frmEntornoLista frmEntornos = new frmEntornoLista();
            frmEntornos.ShowDialog(this);

        }

        private void subMenuItemEntornoCargar_Click(object sender, EventArgs e)
        {
            frmEntornoCargar frmEntornos = new frmEntornoCargar();
            frmEntornos.ShowDialog(this);
        }

        private void pbIdioma_Click(object sender, EventArgs e)
        {
            frmIdioma frmLang = new frmIdioma();
            frmLang.ShowDialog();

            this.RefrescarPanelInfoBandera();
        }

        private void subMenuItemIdiomas_Click(object sender, EventArgs e)
        {
            frmIdiomaLista frmIdiomas = new frmIdiomaLista();
            frmIdiomas.ShowDialog();
        }

        private void subMenuItemCrearAccesoDirecto_Click(object sender, EventArgs e)
        {
            frmAccesoDirectoAlta frmAccesoDirectoNuevo = new frmAccesoDirectoAlta();
            frmAccesoDirectoNuevo.OkAccesoDirectoForm += new frmAccesoDirectoAlta.OkFormCommandEventHandler(frmAccesoDirectoNuevo_OkAccesoDirectoForm);
            frmAccesoDirectoNuevo.Show();
        }

        private void frmAccesoDirectoNuevo_OkAccesoDirectoForm(frmAccesoDirectoAlta.OkFormCommandEventArgs e)
        {
            try
            {  
                if (this.dsAccesosDirectos.Tables.Count == 0)
                {
                    //Crear la tabla
                    DataTable dtCabecera = new DataTable();
                    dtCabecera.TableName = "AD";

                    dtCabecera.Columns.Add("nombre", typeof(string));
                    dtCabecera.Columns.Add("programa", typeof(string));

                    this.dsAccesosDirectos.Tables.Add(dtCabecera);
                }
                //Crear Nueva entrada en el DataSet de Accesos Directos
                DataRow dr = this.dsAccesosDirectos.Tables["AD"].NewRow();
                dr["nombre"] = e.ADNombre;
                dr["programa"] = e.ADPrograma;
                this.dsAccesosDirectos.Tables[0].Rows.Add(dr);

                //Grabar el XML de accesos directos
                string fileAccesosDirecto = Application.StartupPath + "\\" + nombreFicheroAD;
                FileStream fs = new FileStream(fileAccesosDirecto, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                this.dsAccesosDirectos.WriteXml(writer, XmlWriteMode.IgnoreSchema);
                writer.Close();

                //Cargar Accesos Directos al menú de la lanzadera
                this.CrearSubmenuAccesoDirecto(e.ADNombre, e.ADPrograma);

                subMenuItemSeparadorAccesoDirecto.Visible = true;
                subMenuItemEliminarAccesoDirecto.Visible = true;

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errAccesoDirectoNuevo", "Error al crear el acceso directo") + " (" + ex.Message + ")", msgError);
            }
        }

        private void subMenuItemEliminarAccesoDirecto_Click(object sender, EventArgs e)
        {
            frmAccesoDirectoEliminar frmAccesoDirectoDel = new frmAccesoDirectoEliminar();
            frmAccesoDirectoDel.AccesosDirectos = this.dsAccesosDirectos;
            frmAccesoDirectoDel.OkAccesoDirectoEliminar += new frmAccesoDirectoEliminar.OkFormCommandEventHandler(frmAccesoDirectoEliminar_OkAccesoDirectoEliminarForm);
            frmAccesoDirectoDel.Show();
        }

        private void frmAccesoDirectoEliminar_OkAccesoDirectoEliminarForm(frmAccesoDirectoEliminar.OkFormCommandEventArgs e)
        {
            try
            {
                string nombre = e.ADNombre;
                int indice = e.ADIndice;

                //Eliminarlo del DataSet
                for (int i = 0; i < this.dsAccesosDirectos.Tables["AD"].Rows.Count; i++)
                {
                    if (i == indice && nombre == this.dsAccesosDirectos.Tables["AD"].Rows[i]["nombre"].ToString())
                    {
                        this.dsAccesosDirectos.Tables["AD"].Rows.Remove(this.dsAccesosDirectos.Tables["AD"].Rows[i]);
                        break;
                    }

                }
                
                //Eliminarlo del fichero XML
                //Grabar el XML de accesos directos
                string fileAccesosDirecto = Application.StartupPath + "\\" + nombreFicheroAD;
                FileStream fs = new FileStream(fileAccesosDirecto, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                this.dsAccesosDirectos.WriteXml(writer, XmlWriteMode.IgnoreSchema);
                writer.Close();

                //Eliminarlo del menú de la Lanzadera
                foreach (ToolStripItem item in menuItemAccesosDirectos.DropDownItems)
                {
                    if (item is ToolStripMenuItem)
                    {
                        if (item.Tag != null)
                        {
                            if (item.Tag.ToString() == nombre)
                            {
                                menuItemAccesosDirectos.DropDownItems.Remove(item);
                                break;
                            }
                        }
                    }
                }

                if (this.dsAccesosDirectos.Tables["AD"].Rows.Count == 0)
                {
                    //Ocultar la opcioón de eliminar y el separador
                    subMenuItemEliminarAccesoDirecto.Visible = false;
                    subMenuItemSeparadorAccesoDirecto.Visible = false;
                }

                this.menuStrip1.Refresh();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errAccesoDirectoEliminar", "Error al eliminar el acceso directo") + " (" + ex.Message + ")", msgError);
            }
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            frmAbout__old frmInfo = new frmAbout__old();
            frmInfo.ShowDialog();
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            //PDTE !! Cerrar todas las ventanas
            try
            {
                //Actualizar los datos de conexión del usuario
                GlobalVar.UsuarioEnv.GrabarUsuario();

                //Cerrar cadena de conexión a la bbdd de CG
                GlobalVar.ConexionCG.CloseConnection();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
            this.Close();
        }

        private void btnModulo1_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }

        private void btnModulo2_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }

        private void btnModulo3_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }

        private void btnModulo4_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }

        private void btnModulo5_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }

        private void btnModulo6_Click(object sender, EventArgs e)
        {
            string moduloFormulario = ((Button)sender).Tag.ToString();
            this.LlamarModulo(moduloFormulario);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Texto de los controles en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Uniclass");
            
            //Menú
            this.menuItemOpciones.Text = this.LP.GetText("menuItemOpciones", "Opciones");
            this.menuItemModulos.Text = this.LP.GetText("menuItemModulos", "Módulos");
            this.menuItemUtilidades.Text = this.LP.GetText("menuItemUtilidades", "Utilidades");
            this.menuItemParametrizacion.Text = this.LP.GetText("menuItemParametrizacion", "Configuración");
            this.menuItemAccesosDirectos.Text = this.LP.GetText("menuItemAccesoDirecto", "Accesos Directos");
            this.menuItemAbout.Text = this.LP.GetText("menuItemAbout", "Acerca de");
            this.menuItemSalir.Text = this.LP.GetText("menuItemSalir", "Salir");
            this.subMenuItemConectar.Text = this.LP.GetText("subMenuItemConectar", "Conectar al servidor");

            //if (this.tipoBaseDatosCG == "DB2") this.subMenuItemLoginApp.Text = this.LP.GetText("subMenuItemLoginCG", "Login en CG");
            //else this.subMenuItemLoginApp.Text = this.LP.GetText("subMenuItemLoginUniclass", "Login en Uniclass");
            this.subMenuItemLoginApp.Text = this.LP.GetText("subMenuItemLoginUniclass", "Conectar a la aplicación");

            this.subMenuItemIdiomaCambiar.Text = this.LP.GetText("subMenuItemIdiomaCambiar", "Cambiar Idioma");
            this.subMenuItemEntornoCargar.Text = this.LP.GetText("subMenuItemEntornoCargar", "Cargar Entorno");

            this.subMenuItemEntrarClave.Text = this.LP.GetText("subMenuItemEntrarClave", "Entrar clave");
            this.subMenuItemGestionModulos.Text = this.LP.GetText("subMenuItemGestionModulos", "Gestionar módulos");

            this.subMenuItemEntornos.Text = this.LP.GetText("subMenuItemEntornosGestionar", "Gestionar Entornos");
            this.subMenuItemIdiomas.Text = this.LP.GetText("subMenuItemIdiomasGestionar", "Gestionar Idiomas");

            this.subMenuItemTransferirArchivoPC.Text = this.LP.GetText("subMenuItemTransferirArchivoPC", "Transferir Archivo a PC");

            this.subMenuItemCrearAccesoDirecto.Text = this.LP.GetText("subMenuItemCrearAccesoDirecto", "Crear acceso directo");
            this.subMenuItemEliminarAccesoDirecto.Text = this.LP.GetText("subMenuItemEliminarAccesoDirecto", "Eliminar acceso directo");

            if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "") this.lblUsuarioBBDD.Text = this.LP.GetText("lblPrincipalUsuarioBBDD", "Usuario BBDD") + ": " + GlobalVar.UsuarioLogadoCG_BBDD.ToUpper();

            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
            {
                string usuarioAppTitulo = "";
                if (this.tipoBaseDatosCG == "DB2") usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioCG", "Usuario CG");
                else usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioUniclass", "Usuario Uniclass");
                this.lblUsuarioCG.Text = usuarioAppTitulo + ": " + GlobalVar.UsuarioLogadoCG.ToUpper();
                this.lblUsuarioCG.Visible = true;
            }

            //Traducir Nombres de los módulos
            string tag = "";
            //System.Windows.Forms.ToolTip tooltip;
            foreach (Panel p in this.Controls.OfType<Panel>())
            {
                if (p.Tag != null)
                {
                    tag = ((Panel)p).Tag.ToString();
                    if (((Panel)p).Name.IndexOf(modulosPanelesNombre) != -1)
                    {
                        p.Controls[0].Text = this.LP.GetText(tag, tag);

                        //Falta traducir los tooltips
                        //((Panel)p.Controls[0]).too
                        //tooltip = new System.Windows.Forms.ToolTip();
                        //tooltip.SetToolTip(p.Controls[0], this.LP.GetText(tag, tag));

                    }
                }
            }  
        }

        /// <summary>
        /// Define el método de la Interfaz para la relocalización al cambiar de idioma
        /// </summary>
        void IReLocalizable.ReLocalize()
        {
            /*
            System.Globalization.CultureInfo cultura = new System.Globalization.CultureInfo(GlobalVar.LanguageProvider);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this", cultura);
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name, cultura);

            resources.ApplyResources(this.menuStrip1, this.menuStrip1.Name, cultura);

            foreach (ToolStripItem item in this.menuStrip1.Items)
            {
                if (item is ToolStripDropDownItem)
                    foreach (ToolStripItem dropDownItem in ((ToolStripDropDownItem)item).DropDownItems)
                    {
                        resources.ApplyResources(dropDownItem, dropDownItem.Name, cultura);
                    }
            }
            */

            //Traducir Literales
            this.TraducirLiterales();

            //Bandera de idioma
            this.RefrescarPanelInfoBandera();
        }

        /// <summary>
        /// Actualiza el usuario de CG que se ha logado
        /// </summary>
        private void RefrescarPanelInfoUsuario()
        {
            if (GlobalVar.EntornoActivo != null && GlobalVar.EntornoActivo.Nombre != "")
            {
                this.lblEntorno.Text = this.LP.GetText("lblEntornoNombre", "Entorno") + ": " + GlobalVar.EntornoActivo.Nombre;      //Falta traducir
                this.lblEntorno.Visible = true;
            }
            else
            {
                this.lblEntorno.Text = "";
                this.lblEntorno.Visible = false;
            }
            if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
            {
                this.lblUsuarioBBDD.Text = this.LP.GetText("lblPrincipalUsuarioBBDD", "Usuario BBDD") + ": " + GlobalVar.UsuarioLogadoCG_BBDD.ToUpper();
                //this.lblUsuarioBBDD.Visible = true;
            }
            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
            {
                string usuarioAppTitulo = "";
                if (this.tipoBaseDatosCG == "DB2") usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioCG", "Usuario");
                else usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioUniclass", "Usuario");
                this.lblUsuarioCG.Text = usuarioAppTitulo + ": " + GlobalVar.UsuarioLogadoCG.ToUpper();
                this.lblUsuarioCG.Visible = true;
            }
            else
            {
                this.lblUsuarioCG.Text = "";
                this.lblUsuarioCG.Visible = false;
            }
            this.Refresh();
        }

        /// <summary>
        /// Actualiza la imagen de la bandera del idioma según el idioma seleccionado
        /// </summary>
        private void RefrescarPanelInfoBandera()
        {
            try
            {
                string idiomaActual = ConfigurationManager.AppSettings["idioma"];

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");
                string cultura = "";
                string descripcion = "";
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    cultura = idioma.Cultura;
                    if (idiomaActual == cultura)
                    {
                        descripcion = this.LP.GetText("lblIdioma" + cultura, idioma.Descripcion);
                        break;
                    }
                }

                this.lnkIdioma.Text = descripcion;

                /*this.pbIdioma.BackColor = Color.Transparent;

                IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

                string idiomaActual = ConfigurationManager.AppSettings["idioma"];
                string banderaImagen = "";
                foreach (IdiomaElement idioma in idiomaSection.Idiomas)
                {
                    if (idiomaActual == idioma.Cultura)
                    {
                        banderaImagen = idioma.Imagen;
                        break;
                    }
                }

                if (banderaImagen != "")
                {
                    string imagenPath = ConfigurationManager.AppSettings["pathImagenes"];
                    if (imagenPath != "") imagenPath = "\\" + imagenPath + "\\" + banderaImagen;
                    else imagenPath = "\\" + banderaImagen;
                    this.pbIdioma.Image = Image.FromFile(Application.StartupPath + imagenPath);
                    this.Refresh();
                }*/
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Activa los botones para acceder a los módulos activos que tiene el usuario contratados
        /// </summary>
        private void CargarModulosActivos()
        {
            try
            {
                //ModuloSection moduloSection = (ModuloSection)ConfigurationManager.GetSection("moduloSection");

                modulosActivosCant = 0;
                string controlButton = "";
                string imagenPath = ConfigurationManager.AppSettings["pathImagenes"];
                string moduloImagen = "";
                System.Windows.Forms.ToolTip toolTip;

                string controlPanel = "";
                string controlEtiqueta = "";
               
                List<Modulo> modulos = new List<Modulo>();
                if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0 && dsModulosApp.Tables["Modulo"].Rows.Count > 0)
                {
                    //Ordenar los módulos
                    this.dsModulosApp.Tables["Modulo"].DefaultView.Sort = "orden asc";
                    int basico;
                    int activo;
                    int orden;
                    
                    for (int i = 0; i < this.dsModulosApp.Tables["Modulo"].Rows.Count; i++ )
                    {
                        //tratar solo los activos
                        if ((bool)this.dsModulosApp.Tables["Modulo"].Rows[i]["activo"])
                        {
                            basico = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["basico"]);
                            activo = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["activo"]);
                            orden = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["orden"]);
                            modulos.Add(new Modulo(this.dsModulosApp.Tables["Modulo"].Rows[i]["id"].ToString(), 
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["nombre"].ToString(), 
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["nombreDll"].ToString(), 
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["formulario"].ToString(), 
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["imagen"].ToString(), 
                                        basico,activo, orden));
                        }
                    }
                }

                modulos.Sort(new GenericComparer<Modulo>("Orden", GenericComparer<Modulo>.SortOrder.Ascending));

                //Falta tratar el orden de los modulos !!
                int coordX = 33;
                int coordY = 109;
                foreach (Modulo modulo in modulos)
                {
                    if (modulo.Activo == 1)
                    {
                        modulosActivosCant++;

                        //Botones módulos
                        controlButton = modulosBotonesNombre + modulosActivosCant;
                        this.Controls[controlButton].Visible = true;

                        //Mostrar el usuario que se ha logado si procede
                        if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
                        {
                            this.Controls[controlButton].Enabled = true;
                        }
                        else
                        {
                            this.Controls[controlButton].Enabled = false;
                        }

                        this.Controls[controlButton].Tag = modulo.NombreDll + "." + modulo.Formulario;
                        this.Controls[controlButton].Location = new System.Drawing.Point(coordX, coordY);
                        //this.Controls[controlButton].Text = modulo.Descripcion;
                        //((Button)this.Controls[controlButton]).Image = global::FinanzasNet.Properties.Resources.modPagoProv;

                        if (imagenPath != "") moduloImagen = "\\" + imagenPath + "\\" + modulo.Imagen;
                        else moduloImagen = "\\" + modulo.Imagen;
                        ((Button)this.Controls[controlButton]).Image = Image.FromFile(Application.StartupPath + moduloImagen);

                        toolTip = new System.Windows.Forms.ToolTip();
                        toolTip.SetToolTip(this.Controls[controlButton], this.LP.GetText(modulo.NombreDll, modulo.Nombre));

                        //Paneles módulos
                        controlPanel = modulosPanelesNombre + modulosActivosCant;
                        this.Controls[controlPanel].Visible = true;
                        this.Controls[controlPanel].Tag = modulo.NombreDll;
                        this.Controls[controlPanel].Location = new System.Drawing.Point(coordX, coordY + 90);
                        controlEtiqueta = modulosEtiquetasNombre + modulosActivosCant;
                        ((Panel)this.Controls[controlPanel]).Controls[controlEtiqueta].Visible = true;
                        ((Panel)this.Controls[controlPanel]).Controls[controlEtiqueta].Text = this.LP.GetText(modulo.NombreDll, modulo.Nombre);

                        if (coordX + 244 + 211 > this.Size.Width)
                        {
                            coordX = 33;
                            coordY = coordY + 157;
                        }
                        else coordX = coordX + 244;
                    }
                }


                /*
                //Falta tratar el orden de los modulos !!
                foreach (ModuloElement modulo in moduloSection.Modulos)
                {
                    
                    if (modulo.Activo == 1)
                    {
                        modulosActivosCant++;

                        //Botones módulos
                        controlButton = modulosBotonesNombre + modulosActivosCant;
                        this.Controls[controlButton].Visible = true;
                        this.Controls[controlButton].Tag = modulo.Namespace + "." + modulo.Formulario;
                        //this.Controls[controlButton].Text = modulo.Descripcion;
                        //((Button)this.Controls[controlButton]).Image = global::FinanzasNet.Properties.Resources.modPagoProv;

                        if (imagenPath != "") moduloImagen = "\\" + imagenPath + "\\" + modulo.Imagen;
                        else moduloImagen = "\\" + modulo.Imagen;
                        ((Button)this.Controls[controlButton]).Image = Image.FromFile(Application.StartupPath + moduloImagen);

                        toolTip = new System.Windows.Forms.ToolTip();
                        toolTip.SetToolTip(this.Controls[controlButton], this.LP.GetText(modulo.Namespace, modulo.Nombre));

                        //Paneles módulos
                        controlPanel = modulosPanelesNombre + modulosActivosCant;
                        this.Controls[controlPanel].Visible = true;
                        this.Controls[controlPanel].Tag = modulo.Namespace;
                        controlEtiqueta = modulosEtiquetasNombre + modulosActivosCant;
                        ((Panel)this.Controls[controlPanel]).Controls[controlEtiqueta].Visible = true;
                        ((Panel)this.Controls[controlPanel]).Controls[controlEtiqueta].Text = this.LP.GetText(modulo.Namespace, modulo.Nombre);
                    }
                }
                 */ 
                /*
                if (modulosActivosCant > 0)
                {
                    controlButton = modulosBotonesNombre + 1;
                    this.Controls[controlButton].Focus();

                    if (GlobalVar.UsuarioLogadoCG != "")
                    {
                        this.ModulosHabilitarDesabilitar(true);
                    }
                    else
                    {
                        this.ModulosHabilitarDesabilitar(false);
                    }
                }
                 */ 
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errCargarModulosActivos", "Error al cargar los módulos activos") + " (" + ex.Message + ")", msgError);
            }

            /*
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            resources.ApplyResources(this.btnModulo1, "btnModulo1");
            this.btnModulo1.Image = global::FinanzasNet.Properties.Resources.modPagoProv;
            this.btnModulo1.Tag = "ModPagoProv";
            this.btnModulo1.UseVisualStyleBackColor = true;
            this.btnModulo2.Image = global::FinanzasNet.Properties.Resources.modImpuestos;
            */
        }

        /// <summary>
        /// Carga el formulario principal del módulo
        /// </summary>
        /// <param name="modulo">Nombre del módulo y del formulario (corresponde con el campo namespace y formulario de la seccion de modulos -namespace.formulario-)</param>
        private void LlamarModulo(string moduloFormulario)
        {
            try
            {
                //Verificar que el usuario esté logado a la aplicación CG
                if (GlobalVar.UsuarioLogadoCG == "")
                {
                    //Cargar formulario de login para CG
                    frmLoginApp frmLoginAplicacionCG = new frmLoginApp();
                    frmLoginAplicacionCG.ShowDialog(this);

                    //Usuario no autentificado en CG
                    frmLoginAplicacionCG.Dispose();
                    this.RefrescarPanelInfoUsuario();
                }

                if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
                {
                    string nombreFormulario = "";
                    Form formularioMostrar = null;
                    foreach (Form f in Application.OpenForms)
                    {
                        nombreFormulario = f.GetType().FullName;
                        if (nombreFormulario == moduloFormulario)
                        {
                            formularioMostrar = f;
                            break;
                        }
                    }

                    if (formularioMostrar != null)
                    {
                        string nombre = ((Form)formularioMostrar).GetType().FullName;
                        if (((Form)formularioMostrar).WindowState == FormWindowState.Minimized)
                        {
                            ((Form)formularioMostrar).WindowState = FormWindowState.Normal;
                            ((Form)formularioMostrar).BringToFront();
                        }
                        else
                        {
                            ((Form)formularioMostrar).BringToFront();
                        }
                    }
                    else
                    {
                        string ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\";
                        //Buscar el nombre de la dll (coger solo el namespace, desechando el nombre del formulario)
                        string[] moduloForm = moduloFormulario.Split('.');
                        if (moduloForm.Length == 2)
                        {
                            System.Reflection.Assembly extAssembly = System.Reflection.Assembly.LoadFrom(ruta + moduloForm[0] + ".dll");
                            Form extForm = (Form)extAssembly.CreateInstance(moduloFormulario);

                            //this.AddOwnedForm(extForm);
                            extForm.Show(this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = this.LP.GetText("errValTitulo", "Error");
                MessageBox.Show(this.LP.GetText("errLlamarModulos", "Error al cargar el módulo") + " " + moduloFormulario + " (" + ex.Message + ")", msgError);
            }
        }

        /// <summary>
        /// Al mover el tamaño de la ventana se dibujar de nuevo los botones de los módulos
        /// </summary>
        private void RecalcularPosicionModulosActivos()
        {
            try
            {
                modulosActivosCant = 0;
                string controlButton = "";
                string controlPanel = "";

                List<Modulo> modulos = new List<Modulo>();
                if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0 && dsModulosApp.Tables["Modulo"].Rows.Count > 0)
                {
                    //Ordenar los módulos
                    this.dsModulosApp.Tables["Modulo"].DefaultView.Sort = "orden asc";
                    int basico;
                    int activo;
                    int orden;

                    for (int i = 0; i < this.dsModulosApp.Tables["Modulo"].Rows.Count; i++)
                    {
                        //tratar solo los activos
                        if ((bool)this.dsModulosApp.Tables["Modulo"].Rows[i]["activo"])
                        {
                            basico = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["basico"]);
                            activo = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["activo"]);
                            orden = Convert.ToInt16(this.dsModulosApp.Tables["Modulo"].Rows[i]["orden"]);
                            modulos.Add(new Modulo(this.dsModulosApp.Tables["Modulo"].Rows[i]["id"].ToString(),
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["nombre"].ToString(),
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["nombreDll"].ToString(),
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["formulario"].ToString(),
                                        this.dsModulosApp.Tables["Modulo"].Rows[i]["imagen"].ToString(),
                                        basico, activo, orden));
                        }
                    }

                    modulos.Sort(new GenericComparer<Modulo>("Orden", GenericComparer<Modulo>.SortOrder.Ascending));

                    //Falta tratar el orden de los modulos !!
                    int coordX = 33;
                    int coordY = 109;
                    foreach (Modulo modulo in modulos)
                    {
                        if (modulo.Activo == 1)
                        {
                            modulosActivosCant++;

                            //Botones módulos
                            controlButton = modulosBotonesNombre + modulosActivosCant;
                            this.Controls[controlButton].Location = new System.Drawing.Point(coordX, coordY);

                            //Paneles módulos
                            controlPanel = modulosPanelesNombre + modulosActivosCant;
                            this.Controls[controlPanel].Location = new System.Drawing.Point(coordX, coordY + 90);

                            if (coordX + 244 + 211 > this.Size.Width)
                            {
                                coordX = 33;
                                coordY = coordY + 157;
                            }
                            else coordX = coordX + 244;
                        }
                    }

                    this.Refresh();
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                //string msgError = this.LP.GetText("errValTitulo", "Error");
                //MessageBox.Show(this.LP.GetText("errPosicionModulosActivos", "Error calcular la posición de los módulos activos") + " (" + ex.Message + ")", msgError); //Falta traducir
            }
        }

        /// <summary>
        /// Cambia la propiedad Enabled de los botones relacionados con los módulos
        /// </summary>
        /// <param name="valor">true / false</param>
        private void ModulosHabilitarDeshabilitar(bool valor)
        {
            string controlButton = "";
            for (int i = 0; i<modulosActivosCant; i++)
            {
                controlButton = modulosBotonesNombre + (i + 1);
                this.Controls[controlButton].Enabled = valor;
            }

            //Activar/Desactivar opción de menú que permite Transferir Archivos a PC
            this.subMenuItemTransferirArchivoPC.Enabled = valor;

            //Activar/Desactivar opción de menú que permite Autentificar en CG
            this.subMenuItemLoginApp.Enabled = valor;
        }

        private void FillDataSetModulos()
        {
            //Construir el DataSet de Módulos
            if (dsModulosApp == null)
            {
                this.tablaModulos = new DataTable("Modulo");
                this.tablaModulos.Columns.Add("id", typeof(string));
                this.tablaModulos.Columns.Add("nombre", typeof(String));
                this.tablaModulos.Columns.Add("nombreDll", typeof(String));
                this.tablaModulos.Columns.Add("formulario", typeof(String));
                this.tablaModulos.Columns.Add("imagen", typeof(String));
                this.tablaModulos.Columns.Add("basico", typeof(bool));
                this.tablaModulos.Columns.Add("activo", typeof(bool));
                this.tablaModulos.Columns.Add("orden", typeof(int));

                dsModulosApp = new DataSet();
                dsModulosApp.DataSetName = "Modulos_Tabla";
                dsModulosApp.Tables.Add(this.tablaModulos);

                string pathFile = ConfigurationManager.AppSettings["pathFicheros"];
                if (pathFile == null || pathFile == "") pathFile = Application.StartupPath + "\\";

                string nombreFichero = ConfigurationManager.AppSettings["ficheroModulosUsuario"];
                if (nombreFichero == null || nombreFichero == "") nombreFichero = "ModulosUsuario.xml";

                string ficheroXMLModulosCliente = pathFile + nombreFichero;

                string msgError = this.LP.GetText("errValTitulo", "Error");
                try
                {
                    //Chequear si existe el fichero
                    bool existe = System.IO.File.Exists(ficheroXMLModulosCliente) ? true : false;

                    if (existe)
                    {
                        //Leer el fichero XML del usuario
                        this.dsModulosApp.ReadXml(ficheroXMLModulosCliente);
                    }
                    else
                    {
                        MessageBox.Show(this.LP.GetText("errFicheroModulosNoExiste", "Fichero de módulos del usuario no encontrado"), msgError);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    MessageBox.Show(this.LP.GetText("errCargarModulosActivos", "Error al cargar los módulos activos") + " (" + ex.Message + ")", msgError);
                }
            }
        }
        #endregion

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            //Llamar a calculadora 
            proc.StartInfo.FileName = "calc"; 
            proc.Start(); 
        }

        private void CargarAccesosDirectos()
        {
            string pathFile = ConfigurationManager.AppSettings["pathFicheros"];
            if (pathFile == null || pathFile == "") pathFile = Application.StartupPath + "\\";

            string ficheroXMLAccesosDirectos = pathFile + nombreFicheroAD;

            string msgError = this.LP.GetText("errValTitulo", "Error");

            this.dsAccesosDirectos = new DataSet();
            this.dsAccesosDirectos.DataSetName = "Comprobante";
            try
            {
                //Chequear si existe el fichero
                bool existe = System.IO.File.Exists(ficheroXMLAccesosDirectos) ? true : false;

                if (existe)
                {
                    //Leer el fichero XML del usuario
                    this.dsAccesosDirectos.ReadXml(ficheroXMLAccesosDirectos);

                    string nombreAD = "";
                    string programaAD = "";
                    if (this.dsAccesosDirectos.Tables["AD"] != null)
                    {
                        for (int i = 0; i < this.dsAccesosDirectos.Tables["AD"].Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                subMenuItemSeparadorAccesoDirecto.Visible = true;   //Menú Separador
                                subMenuItemEliminarAccesoDirecto.Visible = true;    //Menú Eliminar Acceso Directo
                            }
                            nombreAD = this.dsAccesosDirectos.Tables["AD"].Rows[i]["nombre"].ToString();
                            programaAD = this.dsAccesosDirectos.Tables["AD"].Rows[i]["programa"].ToString();

                            //Cargar Accesos Directos al menú de la lanzadera
                            this.CrearSubmenuAccesoDirecto(nombreAD, programaAD);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Crea un acceso directo en el Menu Principal de la lanzadera
        /// </summary>
        /// <param name="nombre">Nombre del acceso directo</param>
        /// <param name="programa">Nombre del programa que se invoca</param>
        private void CrearSubmenuAccesoDirecto(string nombre, string programa)
        {
            ToolStripMenuItem menuItemOpciones = new ToolStripMenuItem(nombre);
            menuItemOpciones.Tag = nombre;
            Icon iconApp = Icon.ExtractAssociatedIcon(@programa);
            menuItemOpciones.Image = iconApp.ToBitmap();
            menuItemOpciones.Click += new EventHandler(MenuItemClickHandler);
            menuItemAccesosDirectos.DropDownItems.Add(menuItemOpciones);
        }

        private void lnkIdioma_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIdioma frmLang = new frmIdioma();
            frmLang.ShowDialog();

            this.RefrescarPanelInfoBandera();
        }

        private void frmPrincipal_ResizeEnd(object sender, EventArgs e)
        {
            //this.resizeEnd = true;
        }

        private void frmPrincipal_SizeChanged(object sender, EventArgs e)
        {
            //if (this.resizeEnd && (this.Size.Width != this.formWidth || this.Size.Height == this.formHeight))
            if (this.Size.Width != this.formWidth || this.Size.Height == this.formHeight)
            {
                //Recalcular la posicón de los controles de los módulos activos
                this.RecalcularPosicionModulosActivos();

                this.formWidth = this.Size.Width;
                this.formHeight = this.Size.Height;

                //this.resizeEnd = false;
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Grabar el fichero usuario
                GlobalVar.UsuarioEnv.GrabarUsuario();
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            Log.Info("Cerrar Módulo Principal (Lanzadera)");
        }

        private void subMenuItemTransferirArchivoPC_Click(object sender, EventArgs e)
        {
            //Verificar que el usuario esté logado a la aplicación CG
            if (GlobalVar.UsuarioLogadoCG == "")
            {
                //Cargar formulario de login para CG
                frmLoginApp frmLoginAplicacionCG = new frmLoginApp();
                frmLoginAplicacionCG.ShowDialog(this);

                //Usuario no autentificado en CG
                frmLoginAplicacionCG.Dispose();
                this.RefrescarPanelInfoUsuario();
            }

            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
            {
                frmTransferirArchivoPC frmTransferArchivoPC = new frmTransferirArchivoPC();
                frmTransferArchivoPC.ShowDialog();
            }
            
            
            /*
            try
            {
                //sCmd = "OVRDBF FILE(" & txtArchivo.Text & ") TOFILE(" & txtlib.Text & "/" & txtArchivo.Text & ") MBR(" & RTrim(txtMiembro.Text) & ") OVRSCOPE(*JOB)"
                string comando = "OVRDBF FILE(CSW10) TOFILE(CGDATALIS/CSW10) MBR(CSWS105556) OVRSCOPE(*JOB)";
                string sentencia = "CALL QSYS.QCMDEXC('" + comando + "' , ";

                string longitudComando = comando.Length.ToString();

                sentencia = sentencia + longitudComando.PadLeft(10, '0');
                sentencia = sentencia + ".00000)";

                GlobalVar.ConexionCG.ExecuteNonQuery(sentencia, GlobalVar.ConexionCG.GetConnectionValue);

                string query = "select * from CSW10";
                IDataReader dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    string HOLA = dr["NRECG2"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
             */ 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RadFrmMain nuevaLanzadera = new RadFrmMain();
            nuevaLanzadera.Show();
            //nuevaLanzadera.Activate();
            //nuevaLanzadera.Focus();
            //nuevaLanzadera.BringToFront();
        }
    }
}