using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using ObjectModel;
using Telerik.WinControls.Primitives;
using System.Xml;
using System.Configuration;
using System.Collections.ObjectModel;

namespace SmartCG
{
    public partial class RadFrmMain : frmPlantilla, IReLocalizable, IFormEntorno, IOpcionesMenu
    {
        protected bool menuLateralExpanded = true;
        protected static int collapseWidth = 0;

        protected Color defaultBoderColor = Color.Empty;
        protected Color defaultAppBackColor = Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));

        private string tipoBaseDatosCG;

        private string menuXMLPathFichero = Application.StartupPath + @"\app\menu";

        private static Autorizaciones aut;

        public RadFrmMain()
        {
            /*
            ThemeResolutionService.ApplicationThemeName = "Office2010Blue";
            radGridView1.ElementTree.EnableApplicationThemeName = false;
            radGridView1.ThemeName = "Office2010Silver";
            */

            aut = new Autorizaciones();

            ThemeResolutionService.ApplicationThemeName = "Office2013Light";
            
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.InnerColor = Color.Transparent;

            this.radTreeViewMenu.Margin = new Padding(0, 20, 0, 0);
            this.radTreeViewMenu.TreeViewElement.TextAlignment = ContentAlignment.MiddleCenter;
            this.radTreeViewMenu.BackColor = Color.Transparent;
            this.radTreeViewMenu.TreeViewElement.BackColor = Color.Transparent;
            this.radTreeViewMenu.TreeViewElement.DrawBorder = false;

            this.radTreeViewMenu.TreeViewElement.ExpandImage = null;
            this.radTreeViewMenu.TreeViewElement.CollapseImage = null;

            this.radTreeViewMenu.BackgroundImage = null;

            this.radApplicationMenuLanzadera.ElementTree.EnableApplicationThemeName = false;
            this.radApplicationMenuLanzadera.ThemeName = "ControlDefault";

            ImagePrimitive searchIcon = new ImagePrimitive
            {
                Image = Properties.Resources.search
            };
            this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Add(searchIcon);

            this.radBtnMenuMostrarOcultar.ButtonElement.ToolTipText = "Menú mostrar/ocultar";
            this.radButtonExpandAll.ButtonElement.ToolTipText = "Expandir menú";
            this.radButtonCollapseAll.ButtonElement.ToolTipText = "Contraer menú";

            this.radPanelApp.BackColor = defaultAppBackColor;
            this.radPanelPrincipal.BackColor = defaultAppBackColor;
        }

        #region Eventos
        //Deshabilita todos los controles de los módulos
        void IFormEntorno.ModulosDeshabilitar()
        {
            try
            {
                if (GlobalVar.EntornoActivo != null)
                {
                    if (GlobalVar.EntornoActivo.Nombre != "")
                    {
                        this.radLblEntorno.Text = GlobalVar.EntornoActivo.Nombre;      //Falta traducir
                        this.radLblEntorno.Visible = true;
                    }
                    else
                    {
                        this.radLblEntorno.Text = "";
                        this.radLblEntorno.Visible = false;
                    }

                    //Cerrar las páginas cargadas
                    //Si hay páginas cargadas cerrarlas
                    if (this.radPageViewForm.Pages != null && this.radPageViewForm.Pages.Count > 0)
                    {
                        int cantPaginas = this.radPageViewForm.Pages.Count;
                        for (int i = 0; i < cantPaginas; i++)
                            this.radPageViewForm.Pages.Remove(this.radPageViewForm.Pages[0]);

                        this.radPageViewForm.Visible = false;
                        this.pictureBoxAggity.Visible = true;
                    }

                    GlobalVar.UsuarioLogadoCG = null;
                    GlobalVar.UsuarioLogadoCG_BBDD = null;
                    this.radLblUser.Text = "";

                    frmLogin frmLoginServidor = new frmLogin();
                    frmLoginServidor.ShowDialog();

                    frmLoginApp frmLoginAplicacion = new frmLoginApp
                    {
                        VerificarClave = true //Revisar que la clave sea válida
                    };
                    frmLoginAplicacion.ShowDialog();

                    if (GlobalVar.UsuarioLogadoCG == null || GlobalVar.UsuarioLogadoCG == "")
                    {
                        GlobalVar.UsuarioLogadoCG_Nombre = "";

                        //Eliminar todos los nodos del TreeView
                        this.radTreeViewMenu.Nodes.Clear();
                        this.radButtonExpandAll.Visible = false;
                        this.radButtonCollapseAll.Visible = false;
                    }

                    //if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG != null)
                    if (GlobalVar.UsuarioLogadoCG != null)
                    {
                        if (this.radLblUser.Text == "") this.radLblUser.Text = GlobalVar.UsuarioLogadoCG;
                        if (GlobalVar.UsuarioLogadoCG_Nombre != "") this.radLblUser.Text += " - " + GlobalVar.UsuarioLogadoCG_Nombre;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        void IFormEntorno.ActualizaListaElementos()
        {
        }

        public void ReLocalize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Se invoca al hacer login app para cerrar las páginas abiertas y 
        /// volver a cargar el menu con los permisos del usuario logado
        /// </summary>
        void IOpcionesMenu.ActualizaOpcionesMenuPermisos()
        {
            try
            {
                //Elimina todas las páginas cargadas
                if (this.radPageViewForm.Pages != null && this.radPageViewForm.Pages.Count > 0)
                {
                    for (int i = 0; i < this.radPageViewForm.Pages.Count; i++)
                        this.radPageViewForm.Pages.Remove(this.radPageViewForm.Pages[0]);

                    this.radPageViewForm.Visible = false;
                    this.pictureBoxAggity.Visible = true;
                }

                //Recarga el menú con los permisos necesarios según el usuario logado
                this.CargarMenu();

                //this.radMenuItemConfig.Enabled = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadFrmMain_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Smart CG");

            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;
            //this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            ////this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            //this.Visible = true;
            //this.Focus();

            this.radLblEntorno.Text = "";
            this.radLblUser.Text = "";

            //Recuperar el tipo de bbdd de contabilidad
            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Poner los literales en el idioma que corresponda
            //this.TraducirLiterales();

            //Mostrar el usuario que se ha logado si procede
            if (GlobalVar.UsuarioLogadoCG_BBDD != "" && GlobalVar.UsuarioLogadoCG_BBDD != null)
            {
                this.RefrescarPanelInfoUsuario();
            }
            else
            {
                if (GlobalVar.UsuarioEnv.CargarListaEntornosInicio)
                {
                    frmEntornoLista entornos = new frmEntornoLista();
                    entornos.ShowDialog();
                    return;
                }

                if (GlobalVar.EntornoActivo != null && GlobalVar.EntornoActivo.Nombre != "")
                {
                    this.radLblEntorno.Text = GlobalVar.EntornoActivo.Nombre;      //Falta traducir
                    this.radLblEntorno.Visible = true;
                }
                else
                {
                    this.radLblEntorno.Text = "";
                    this.radLblEntorno.Visible = false;

                    frmEntornoLista entornos = new frmEntornoLista();
                    entornos.ShowDialog();
                    return;
                }
            }

            //Cargar Menu
            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "") this.CargarMenu();

            if (GlobalVar.UsuarioLogadoCG_BBDD == null || GlobalVar.UsuarioLogadoCG_BBDD == "")
            {
                frmLogin frmLoginServidor = new frmLogin();
                frmLoginServidor.ShowDialog();
            }

            //validar clave
            frmValidaClave frmValidaClave = new frmValidaClave();
            frmValidaClave.ShowDialog();
            if (frmValidaClave.claveOK == true)
            {
                if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "" &&
                (GlobalVar.UsuarioLogadoCG == null || GlobalVar.UsuarioLogadoCG == ""))
                {
                    frmLoginApp frmLoginAplicacion = new frmLoginApp();
                    frmLoginAplicacion.ShowDialog();
                    if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
                    {
                        this.radLblUser.Text = GlobalVar.UsuarioLogadoCG;
                        if (GlobalVar.UsuarioLogadoCG_Nombre != "") this.radLblUser.Text += " - " + GlobalVar.UsuarioLogadoCG_Nombre;
                    }
                }
            }
            //fin valida clave

            this.radTreeViewMenu.Focus();
            if (this.radTreeViewMenu.Nodes.Count > 0)
                this.radTreeViewMenu.SelectedNode = this.radTreeViewMenu.Nodes[0];
        }

        private void RadBtnMenuMostrarOcultar_Click(object sender, EventArgs e)
        {
            if (this.menuLateralExpanded)
            {
                int sizePanel = this.radPanelMenu.Size.Width;
                int sizeButton = this.radBtnMenuMostrarOcultar.Width;
                collapseWidth = sizePanel - sizeButton - 4;

                this.menuLateralExpanded = false;
                this.radPanelMenu.Size = (Size)new Point(this.radPanelMenu.Size.Width - collapseWidth, this.radPanelMenu.Height);

                this.radPanelBarraSuperior.Size = (Size)new Point(this.radPanelBarraSuperior.Size.Width + collapseWidth, this.radPanelBarraSuperior.Height);
                this.radPanelBarraSuperior.Location = new Point(this.radPanelBarraSuperior.Location.X - collapseWidth, this.radPanelBarraSuperior.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width + collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X - collapseWidth, this.radPanelApp.Location.Y);
                this.radLlbUniclassFinanzas.Visible = true;
                this.radTreeViewMenu.Visible = false;
                this.radButtonExpandAll.Visible = false;
                this.radButtonCollapseAll.Visible = false;
            }
            else
            {
                this.menuLateralExpanded = true;
                this.radPanelMenu.Size = (Size)new Point(this.radPanelMenu.Size.Width + collapseWidth, this.radPanelMenu.Height);

                this.radPanelBarraSuperior.Size = (Size)new Point(this.radPanelBarraSuperior.Size.Width - collapseWidth, this.radPanelBarraSuperior.Height);
                this.radPanelBarraSuperior.Location = new Point(this.radPanelBarraSuperior.Location.X + collapseWidth, this.radPanelBarraSuperior.Location.Y);

                this.radPanelApp.Size = (Size)new Point(this.radPanelApp.Size.Width - collapseWidth, this.radPanelApp.Height);
                this.radPanelApp.Location = new Point(this.radPanelApp.Location.X + collapseWidth, this.radPanelApp.Location.Y);
                this.radLlbUniclassFinanzas.Visible = false;
                this.radTreeViewMenu.Visible = true;
                this.radButtonExpandAll.Visible = true;
                this.radButtonCollapseAll.Visible = true;
            }
        }

        private void RadTreeViewMenu_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            //e.NodeElement.ExpanderElement.Visibility = ElementVisibility.Hidden;
            if (e.Node.Expanded)
                //e.NodeElement.ExpanderElement.SignImage = Properties.Resources.treeview_expand;
                //e.NodeElement.ImageElement.Image = Properties.Resources.treeview_expand;
                e.NodeElement.ExpanderElement.Image = Properties.Resources.treeview_expand;
            else
                //e.NodeElement.ExpanderElement.SignImage = Properties.Resources.treeview_collapse;
                //e.NodeElement.ImageElement.Image = Properties.Resources.treeview_collapse;
                e.NodeElement.ExpanderElement.Image = Properties.Resources.treeview_collapse;
        }

        private void RadTreeViewMenu_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (GlobalVar.UsuarioLogadoCG_BBDD == null || GlobalVar.UsuarioLogadoCG_BBDD == "")
                {
                    frmLogin frmLoginServidor = new frmLogin();
                    frmLoginServidor.ShowDialog();
                }

                if (GlobalVar.UsuarioLogadoCG == null || GlobalVar.UsuarioLogadoCG == "")
                {
                    frmLoginApp frmLoginAplicacion = new frmLoginApp();
                    frmLoginAplicacion.ShowDialog();
                }

                //if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG != null)
                if (GlobalVar.UsuarioLogadoCG != null)
                {
                    if (this.radLblUser.Text == "")
                    {
                        this.radLblUser.Text = GlobalVar.UsuarioLogadoCG;
                        if (GlobalVar.UsuarioLogadoCG_Nombre != "") this.radLblUser.Text += " - " + GlobalVar.UsuarioLogadoCG_Nombre;
                    }

                    int cantNodos;
                    ///if (e is MouseEventArgs args)
                    ///{
                    ///    RadTreeNode clickedNode = this.radTreeViewMenu.GetNodeAt(args.X, args.Y);
                        RadTreeNode clickedNode = this.radTreeViewMenu.SelectedNode;
                        if (clickedNode != null)
                        {
                            cantNodos = clickedNode.GetNodeCount(true);
                            if (cantNodos == 0)
                            {
                                //MessageBox.Show("Node Text: " + clickedNode.Text + " Node Value: " + clickedNode.Tag);

                                RadPageViewPage pageOne = new RadPageViewPage
                                {
                                    Text = clickedNode.Text,
                                    AutoScroll = true
                                };

                                string ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\";

                                string parametros = clickedNode.Tag.ToString();
                                if (parametros != null)
                                {
                                    string[] dllForm = parametros.Split('_');
                                    if (dllForm.Length >= 2)
                                    {
                                        System.Reflection.Assembly extAssembly = System.Reflection.Assembly.LoadFrom(ruta + dllForm[0] + ".dll");
                                        string formularioCargar = dllForm[0] + "." + dllForm[1];
                                        Form extForm = (Form)extAssembly.CreateInstance(formularioCargar);
                                        extForm.FormBorderStyle = FormBorderStyle.None;
                                        extForm.Dock = DockStyle.Fill;
                                        extForm.TopLevel = false;

                                        //Parámetros llamada al formulario
                                        if (dllForm.Length >= 3)
                                        {
                                            Type t = extForm.GetType();
                                            System.Reflection.PropertyInfo pInfo;

                                            string[] paramForm = dllForm[2].Split(';');
                                            string paramNombre = "";
                                            char separadorParamValue = '=';
                                            for (int i = 0; i < paramForm.Length; i++)
                                            {
                                                string[] paramValor = paramForm[i].Split(separadorParamValue);

                                                paramNombre = paramValor[0].Trim();
                                                if (paramNombre != "AutUsuario" && paramNombre != "AutProceso")
                                                {
                                                    pInfo = t.GetProperty(paramValor[0].Trim());

                                                    pInfo.SetValue(extForm, Convert.ChangeType(paramValor[1], pInfo.PropertyType), null);
                                                }
                                                else
                                                {
                                                    if (!UsuarioAutorizado(paramNombre, paramValor[1])) return;


                                                    /*if (GlobalVar.UsuarioLogadoCG != "CGIFS")
                                                    if (GlobalVar.UsuarioLogadoCG_TipoSeguridad != "1")
                                                    {
                                                        //Si el usuario no tiene autorización, no cargar el formulario
                                                        if (paramValor[1].Trim() != GlobalVar.UsuarioLogadoCG_TipoSeguridad) return;
                                                    }*/
                                                }
                                            }
                                        }

                                        pageOne.Controls.Add(extForm);
                                        extForm.Show();
                                    }
                                }

                                if (radPageViewForm.Pages.Count == 0) this.radPageViewForm.PageBackColor = defaultAppBackColor;
                                else this.radPageViewForm.PageBackColor = System.Drawing.Color.White;

                                this.radPageViewForm.Pages.Add(pageOne);
                                this.radPageViewForm.SelectedPage = pageOne;

                                this.radTreeViewMenu.SelectedNode = null;

                                if (!this.radPageViewForm.Visible)
                                {
                                    this.radPageViewForm.Visible = true;
                                    this.pictureBoxAggity.Visible = false;
                                }
                            }
                        }
                    ///}
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Se ha producido un error y no es posible acceder a la opción solicitada.");
                Log.Error(Utiles.CreateExceptionString(ex));
                this.radTreeViewMenu.SelectedNode = null;
            }
        }

        private void RadPageViewForm_PageRemoved(object sender, RadPageViewEventArgs e)
        {
            if (this.radPageViewForm.Pages.Count > 0)
            {
                this.radPageViewForm.Visible = true;
                this.pictureBoxAggity.Visible = false;
            }
            else
            {
                this.radPageViewForm.Visible = false;
                this.pictureBoxAggity.Visible = true;
            }
        }

        private void RadMenuItemConectarApp_Click(object sender, EventArgs e)
        {
            frmLoginApp loginApp = new frmLoginApp
            {
                VerificarClave = true //Revisar que la clave sea válida
            };
            loginApp.Show();
        }

        private void RadMenuItemGestionEntornos_Click(object sender, EventArgs e)
        {
            frmEntornoLista entornosLista = new frmEntornoLista();
            entornosLista.Show();
        }

        private void RadMenuItemLogout_Click(object sender, EventArgs e)
        {
            try
            {
                //Cerrar la sesión
                //Si hay páginas cargadas cerrarlas
                if (this.radPageViewForm.Pages != null && this.radPageViewForm.Pages.Count > 0)
                {
                    for (int i = 0; i < this.radPageViewForm.Pages.Count; i++)
                        this.radPageViewForm.Pages.Remove(this.radPageViewForm.Pages[0]);

                    this.radPageViewForm.Visible = false; 
                    this.pictureBoxAggity.Visible = true;
                }

                GlobalVar.UsuarioLogadoCG = null;
                this.radLblUser.Text = "";

                //Eliminar todos los nodos del TreeView
                this.radTreeViewMenu.Nodes.Clear();
                this.radButtonExpandAll.Visible = false;
                this.radButtonCollapseAll.Visible = false;

                //Inhabilitar la opción del menú lateral de configuración (parámetros se almacenan por usuario)
                //this.radMenuItemConfig.Enabled = false;
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadMenuItemConfig_Click(object sender, EventArgs e)
        {
            RadFrmConfig config = new RadFrmConfig();
            config.Show();
        }

        private void RadMenuItemAbout_Click(object sender, EventArgs e)
        {
            try
            {
                RadFrmAbout about = new RadFrmAbout();
                about.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadMenuItemClave_Click(object sender, EventArgs e)
        {
            try
            {
                frmValidaClave clave = new frmValidaClave
                {
                    GestionarClaveOpcion = true
                };
                clave.Show();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        private void RadMenuItemConectarServidor_Click(object sender, EventArgs e)
        {
            frmLogin loginServidor = new frmLogin();
            loginServidor.Show();
        }

        private void RadTxtBoxMenuSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radTxtBoxMenuSearch.Text.Trim() != "")
                {
                    this.radTreeViewMenu.TreeViewElement.FilterPredicate = null;
                    this.radTreeViewMenu.Filter = this.radTxtBoxMenuSearch.Text;

                    if (this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Count == 2)
                    {
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);
                    }
                    else if (this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Count == 1) this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);

                    if (this.radTxtBoxMenuSearch.Text.Trim() != "")
                    {
                        RadButtonElement deleteIcon = new RadButtonElement
                        {
                            Image = Properties.Resources.delete,
                            ShowBorder = false
                        };
                        deleteIcon.Click += new System.EventHandler(DeleteIcon_Click);
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Add(deleteIcon);
                    }
                    else
                    {
                        ImagePrimitive searchIcon = new ImagePrimitive
                        {
                            Image = Properties.Resources.search
                        };
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Add(searchIcon);
                    }

                    this.radTreeViewMenu.ExpandAll();
                }
                else
                {
                    if (this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Count == 2)
                    {
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);
                        this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);
                    }
                    else if (this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Count == 1) this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.RemoveAt(0);

                    ImagePrimitive searchIcon = new ImagePrimitive
                    {
                        Image = Properties.Resources.search
                    };
                    this.radTxtBoxMenuSearch.TextBoxElement.ButtonsStack.Children.Add(searchIcon);
                    
                    this.radTreeViewMenu.TreeViewElement.FilterPredicate = FilterNodeTrue;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private bool FilterNodeTrue(RadTreeNode node)
        {
            return true;
        }

        private void DeleteIcon_Click(object sender, EventArgs e)
        {
            try
            {
                this.radTxtBoxMenuSearch.Text = "";
                this.radTreeViewMenu.TreeViewElement.FilterPredicate = FilterNodeTrue;
                this.radTreeViewMenu.CollapseAll();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void RadBtnMenuMostrarOcultar_GotFocus(object sender, System.EventArgs e)
        {
            //this.defaultBoderColor = this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.ForeColor;
            //this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.ForeColor = Color.FromArgb(255, 198, 114);
        }

        private void RadBtnMenuMostrarOcultar_LostFocus(object sender, System.EventArgs e)
        {
            //this.radBtnMenuMostrarOcultar.ButtonElement.BorderElement.ForeColor = defaultBoderColor;
        }

        private void RadButtonExpandAll_Click(object sender, EventArgs e)
        {
            this.radTreeViewMenu.ExpandAll();
        }

        private void RadButtonCollapseAll_Click(object sender, EventArgs e)
        {
            this.radTreeViewMenu.CollapseAll();
        }

        /// <summary>
        /// Actualiza el usuario de CG que se ha logado
        /// </summary>
        private void RefrescarPanelInfoUsuario()
        {
            if (GlobalVar.EntornoActivo != null && GlobalVar.EntornoActivo.Nombre != "")
            {
                this.radLblEntorno.Text = GlobalVar.EntornoActivo.Nombre;      //Falta traducir
                this.radLblEntorno.Visible = true;
            }
            else
            {
                this.radLblEntorno.Text = "";
                this.radLblEntorno.Visible = false;
            }

            /*if (GlobalVar.UsuarioLogadoCG_BBDD != null && GlobalVar.UsuarioLogadoCG_BBDD != "")
            {
                this.radLblUser.Text = GlobalVar.UsuarioLogadoCG_BBDD.ToUpper();
                //this.lblUsuarioBBDD.Visible = true;
            }*/
            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
            {
                /*string usuarioAppTitulo = "";
                if (this.tipoBaseDatosCG == "DB2") usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioCG", "Usuario");
                else usuarioAppTitulo = this.LP.GetText("lblPrincipalUsuarioUniclass", "Usuario");
                this.lblUsuarioCG.Text = usuarioAppTitulo + ": " + GlobalVar.UsuarioLogadoCG.ToUpper();*/
                this.radLblUser.Text = GlobalVar.UsuarioLogadoCG.ToUpper();
                if (GlobalVar.UsuarioLogadoCG_Nombre != "") this.radLblUser.Text += " - " + GlobalVar.UsuarioLogadoCG_Nombre;
                this.radLblUser.Visible = true;
            }
            else
            {
                this.radLblUser.Text = "";
                this.radLblUser.Visible = false;
            }
            this.Refresh();
        }

        private void PictureBoxUserLogado_Click(object sender, EventArgs e)
        {
            frmLoginApp frmLoginAplicacion = new frmLoginApp();
            frmLoginAplicacion.ShowDialog();
            if (GlobalVar.UsuarioLogadoCG != null && GlobalVar.UsuarioLogadoCG != "")
            {
                this.radLblUser.Text = GlobalVar.UsuarioLogadoCG;
                if (GlobalVar.UsuarioLogadoCG_Nombre != "") this.radLblUser.Text += " - " + GlobalVar.UsuarioLogadoCG_Nombre;
            }
        }

        private void PictureBoxEntorno_Click(object sender, EventArgs e)
        {
            frmEntornoLista entornosLista = new frmEntornoLista();
            entornosLista.Show();
        }

        private void PictureBoxUserLogado_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBoxUserLogado.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxUserLogado.Cursor = Cursors.Hand;
        }

        private void PictureBoxUserLogado_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBoxUserLogado.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxUserLogado.Cursor = Cursors.Default;
        }

        private void PictureBoxEntorno_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBoxEntorno.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxEntorno.Cursor = Cursors.Hand;
        }

        private void PictureBoxEntorno_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBoxEntorno.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxEntorno.Cursor = Cursors.Default;
        }

        private void RadPageViewForm_SelectedPageChanged(object sender, EventArgs e)
        {
            //this.radPageViewForm.SelectedPage.BackColor = System.Drawing.Color.Yellow;
        }

        private void RadPageViewForm_SelectedPageChanging(object sender, RadPageViewCancelEventArgs e)
        {
            //RadPageViewPage actual = this.radPageViewForm.SelectedPage;
        }

        private void RadFrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Smart CG");
        }

        private void radTreeViewMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.RadTreeViewMenu_DoubleClick(sender, e);
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Verifica si el usuario tiene autorización para la opción de menú indicada
        /// </summary>
        /// <param name="tipoAutorizacion"></param>
        /// <param name="valor"></param>
        private static bool UsuarioAutorizado(string tipoAutorizacion, string valor)
        {
            bool autorizacion = false;
            try
            {
                //DUDA, usuario de sistema siempre tiene autorizaciones sobre todos los procesos ?????
                if (GlobalVar.UsuarioLogadoCG == "CGIFS" || GlobalVar.UsuarioLogadoCG_TipoSeguridad == "1")
                    autorizacion = true;
                else
                {
                    switch (tipoAutorizacion)
                    {
                        case "AutUsuario":
                            //Autorizaciones para usuarios de tipo de seguridad igual que el valor indicado
                            if (valor == GlobalVar.UsuarioLogadoCG_TipoSeguridad) autorizacion = true;
                            break;
                        case "AutProceso":
                            //Autorizaciones sobre procesos
                            const string autGrupo = "01";
                            const string autOperConsulta = "10";

                            autorizacion = aut.Operar(valor, autGrupo, autOperConsulta);
                            break;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            return (autorizacion);
        }

        /// <summary>
        /// Deshabilita los nodos que el usuario no tenga autorizacion
        /// </summary>
        /// <param name="treeNodeCollection"></param>
        private static void CheckTreeViewNodesAutorizacion(IEnumerable<RadTreeNode> treeNodeCollection)
        {
            try
            {
                string parametros = "";
                foreach (var node in treeNodeCollection)
                {
                    if (node.Tag != null)
                    {
                        parametros = node.Tag.ToString();

                        if (parametros != null)
                        {
                            string[] param = parametros.Split('_');
                            if (param.Length >= 3)
                            {
                                string[] paramAll = param[2].Split(';');
                                string paramNombreActual = "";
                                string paramValorActual = "";
                                bool nodeEnabled = true;
                                for (int i = 0; i < paramAll.Length; i++)
                                {
                                    string[] paramValor = paramAll[i].Split('=');

                                    paramNombreActual = paramValor[0].Trim();

                                    if (paramNombreActual == "AutUsuario" || paramNombreActual == "AutProceso")
                                    {
                                        paramValorActual = paramValor[1].Trim();
                                        //if (paramValorActual != GlobalVar.UsuarioLogadoCG_TipoSeguridad)
                                        if (!UsuarioAutorizado(paramNombreActual, paramValorActual))
                                        {
                                            nodeEnabled = false;
                                        }
                                        break;
                                    }
                                }
                                node.Enabled = nodeEnabled;
                            }
                        }
                    }

                    CheckTreeViewNodesAutorizacion(node.Nodes);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga el menú partiendo del fichero xml que lo describe
        /// </summary>
        private void CargarMenu()
        {
            try
            {
                this.menuLateralExpanded = true;

                this.radTreeViewMenu.Nodes.Clear();

                string menuXMLNombreFichero = "MenuTreeView.xml";
                string strXMLFile = menuXMLPathFichero + "\\" + menuXMLNombreFichero;

                //this.radTreeViewMenu.LoadXML(@"C:\VS2017_Projects\SmartCG\SmartCG\MenuTreeView.xml");
                this.radTreeViewMenu.LoadXML(strXMLFile);

                //Si el usuario logado no es CGIFS y no es un usuario con tipo de seguridad de sistemas, chequear autorizaciones del menu
                if (GlobalVar.UsuarioLogadoCG != "CGIFS")
                    if (GlobalVar.UsuarioLogadoCG_TipoSeguridad != "1") CheckTreeViewNodesAutorizacion(this.radTreeViewMenu.Nodes);

                using (this.radTreeViewMenu.DeferRefresh())
                {
                    //this.radTreeViewMenu.Nodes.Clear();
                    this.radTreeViewMenu.CollapseAll();

                    this.radButtonExpandAll.Visible = true;
                    this.radButtonCollapseAll.Visible = true;
                }
            
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        #endregion

    }
}