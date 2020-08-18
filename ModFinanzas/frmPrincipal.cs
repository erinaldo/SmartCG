using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Collections;
using ObjectModel;

namespace ModFinanzas
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable
    {
        private string menuXMLNombreFicheroGenerico;
        private string menuXMLNombreFichero;

        private string menuXMLPathFichero = Application.StartupPath + @"\app\menu";
        private XmlDocument menuXML;

        private ImageList menuImageList;
        private ArrayList menuImageArray;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo de Finanzas");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Recuperar nombre de fichero genérico para el menú
            menuXMLNombreFicheroGenerico = ConfigurationManager.AppSettings["ModFinan_FicheroMenu"];
            if (menuXMLNombreFicheroGenerico == null || menuXMLNombreFicheroGenerico == "") menuXMLNombreFicheroGenerico = "MenuFinanzas.xml";

            //Buscar Nombre Fichero para Usuario Logado
            menuXMLNombreFichero = menuXMLNombreFicheroGenerico;

            try
            {
                string userlogado = System.Environment.UserName.ToUpper();
                if (userlogado != "")
                {
                    string[] ficheroExtension = menuXMLNombreFichero.Split('.');
                    if (ficheroExtension.Length == 2)
                    {
                        menuXMLNombreFichero = ficheroExtension[0] + "_" + userlogado + "." + ficheroExtension[1];

                        //Chequea que exista el fichero menú para el usuario logado, si no lo encuentra lo crea partiendo del fichero genérico
                        this.ChequearExisteFicheroMenu();
                    }
                    else
                    {
                        //Error .... fichero no tiene el formato correcto
                        this.lblResult.Text = this.LP.GetText("errCargarOpciones", "No es posible cargar las opciones del Módulo de Finanzas");
                        this.lblResult.Text += " \n\r\n\r(" + menuXMLNombreFichero + ")";
                        this.lblResult.Visible = true;
                        this.gbFinanzas.Visible = false;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                this.lblResult.Text = this.LP.GetText("errCargarOpciones", "No es posible cargar las opciones del Módulo de Finanzas");
                this.lblResult.Text += " \n\r\n\r(" + ex.Message + ")";
                this.lblResult.Visible = true;
                this.gbFinanzas.Visible = false;
            }

            //Limpiar LDA
            this.LimpiarLDA();

            //Cargar el menú de finanzas
            this.CargarMenuFinanzas();
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeViewMenu_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                MenuItem item = ((MenuItem)e.Node);
                if (item.Programa != "")
                {
                    if (item.Proceso != "")
                    {
                        //Llamar al Programa con el proceso
                        this.LlamarPrograma(item.Programa, item.Proceso);
                        //MessageBox.Show(e.Node.FullPath);
                    }
                    else
                    {
                        //Llamar al Programa con el proceso
                        this.LlamarPrograma(item.Programa);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Click en algunas de las opciones del Menú Derecho del TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuDerecho_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
               
                Control ctrl = ((System.Windows.Forms.ContextMenuStrip)(sender)).SourceControl;
                int nodoSel = ((TreeView)ctrl).SelectedNode.Index;

                MenuItem nodo = ((MenuItem)((TreeView)ctrl).SelectedNode);

                string tag = e.ClickedItem.Tag.ToString();

                string advertenciaTitulo = this.LP.GetText("wrnTitulo", "Advertencia");
                string advertenciaTexto = "";
                string advertenciaPreg = this.LP.GetText("wrnPreguntaContinuar", "¿Desea continuar?");
                DialogResult res;
                MenuItem menuItem;

                XmlNodeList baseNodeList = this.menuXML.SelectNodes("MenuModuloFinanzas/nodo");
                bool saveXMLFile = false;
                bool addOpcionAllXMLFile = false;
                bool delOpcionAllXMLFile = false;
                menuItem = new MenuItem();
                string textoNodo = this.BuscarTextoNodo(nodo.Id, nodo.TextoDefecto);

                switch (tag)
                {
                    case "Desactivar":      //Desactivar el Nodo
                        //Pedir confirmación
                        advertenciaTexto = this.LP.GetText("wrnDesactivarOpcion", "Se va a desactivar la opción");
                        res = MessageBox.Show(advertenciaTexto + " \"" + textoNodo + " \". " + advertenciaPreg, advertenciaTitulo, MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            nodo.Activo = false;
                            this.treeViewMenu.Nodes.Remove(this.treeViewMenu.SelectedNode);
                            this.treeViewMenu.Refresh();

                            //Buscar el nodo dentro del fichero XML y desactivarlo (poner el atributo activo a 0)                          
                            this.ActivateDesactivateNodeToXML(false, nodo.Id, baseNodeList);
                            saveXMLFile = true;
                        }
                        break;
                    case "AddOpcion":       //Adicionar una opción al Menú de Opciones Especiales
                        frmAddOpcionEspecial frmAddOpcion = new frmAddOpcionEspecial();
                        frmAddOpcion.LPForm = this.LP;
                        frmAddOpcion.FrmPadre = this;
                        frmAddOpcion.ShowDialog();
                        
                        if (frmAddOpcion.Aceptar)
                        {
                            //Cantidad de hijos del nodo padre
                            //int cantHijosNodoPadre = this.treeViewMenu.SelectedNode.Nodes.Count;
                            int orden = this.BuscarOrdenNodo((MenuItem)this.treeViewMenu.SelectedNode);
                            //menuItem.Id = nodo.Id + "_" + (cantHijosNodoPadre + 1).ToString();
                            menuItem.Id = nodo.Id + "_" + orden.ToString();
                            menuItem.Activo = true;
                            menuItem.Orden = orden;
                            menuItem.Icono = nodo.Icono;
                            menuItem.Custom = false;    //??
                            menuItem.Programa = "wrun32";   //??
                            menuItem.Proceso = frmAddOpcion.Nombre;
                            menuItem.TextoDefecto = frmAddOpcion.Descripcion;
                            menuItem.Text = menuItem.TextoDefecto;
                            menuItem.IdPadre = "";      //No existe identificador del padre

                            //Adicionar las opciones del menú con click derecho
                            this.NodoAddMenuClickDerecho(ref menuItem);

                            this.treeViewMenu.SelectedNode.Nodes.Add(menuItem);
                            this.treeViewMenu.SelectedNode.Expand();
                            this.treeViewMenu.Refresh();

                            //Grabar en el XML el nuevo elemento dentro del nodo de Opciones Especiales
                            this.AddItemToXML(ref this.menuXML, nodo.Id, baseNodeList, menuItem.Id, menuItem.Orden.ToString(), menuItem.Programa, menuItem.Proceso, menuItem.Text, menuItem.IdPadre, menuItem.Icono);
                            saveXMLFile = true;
                            addOpcionAllXMLFile = true;
                        }
                        
                        break;
                    case "DelOpcion":       //Quitar una opción al Menú de Opciones Especiales
                        advertenciaTexto = this.LP.GetText("wrnEliminarOpcion", "Se va a eliminar la opción");
                        res = MessageBox.Show(advertenciaTexto + " \"" + textoNodo + " \". " + advertenciaPreg, advertenciaTitulo, MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            //Falta grabar en el XML que el nodo no está activo
                            nodo.Activo = false;
                            this.treeViewMenu.Nodes.Remove(this.treeViewMenu.SelectedNode);
                            this.treeViewMenu.Refresh();

                            //Grabar en el XML que no existe este elemento
                            this.DeleteNodeToXML(nodo.Id, baseNodeList);
                            saveXMLFile = true;
                            delOpcionAllXMLFile = true;
                        }
                        break;
                    case "AddGrupo":        //Adicionar un grupo al Menú Personalizado
                        frmAddMenuPersonalizado frmAddMenu = new frmAddMenuPersonalizado();
                        //frmAddOpcion.LPForm = this.LP;
                        frmAddMenu.FrmPadre = this;
                        frmAddMenu.ShowDialog();

                        if (frmAddMenu.Aceptar)
                        {
                            //Cantidad de hijos del nodo padre
                            int orden = this.BuscarOrdenNodo((MenuItem)this.treeViewMenu.SelectedNode);
                            //int cantHijosNodoPadre = this.treeViewMenu.SelectedNode.Nodes.Count;
                            menuItem.Id = nodo.Id + "_" + orden.ToString(); 
                            menuItem.Activo = true;
                            menuItem.Orden = orden;        
                            menuItem.Icono = "";        
                            menuItem.Custom = false;   //??
                            menuItem.Programa = "";
                            menuItem.Proceso = "";
                            menuItem.TextoDefecto = frmAddMenu.Descripcion;
                            menuItem.Text = menuItem.TextoDefecto;

                            //Adicionar las opciones del menú con click derecho
                            this.NodoAddMenuClickDerecho(ref menuItem);

                            this.treeViewMenu.SelectedNode.Nodes.Add(menuItem);
                            this.treeViewMenu.Refresh();

                            //Grabar en el XML el nuevo nodo dentro del nodo de Menú Personalizado
                            this.AddNodeToXML(nodo.Id, baseNodeList, menuItem.Id, menuItem.Orden.ToString(), menuItem.Text);
                            saveXMLFile = true;
                        }

                        break;
                    case "DelGrupoItem":    //Quitar un grupo o un elemento del Menú Personalizado
                        advertenciaTexto = this.LP.GetText("wrnEliminarOpcion", "Se va a eliminar la opción");
                        res = MessageBox.Show(advertenciaTexto + " \"" + textoNodo + " \". " + advertenciaPreg, advertenciaTitulo, MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            //Falta grabar en el XML que el nodo no está activo
                            nodo.Activo = false;
                            this.treeViewMenu.Nodes.Remove(this.treeViewMenu.SelectedNode);
                            this.treeViewMenu.Refresh();

                            //Grabar en el XML que no existe el nodo o el elemento
                            this.DeleteNodeToXML(nodo.Id, baseNodeList);
                            saveXMLFile = true;
                        }
                        break;
                }

                //Grabar los cambios en el fichero XML
                if (saveXMLFile)
                {
                    //string strXMLFile = @"C:\VS2010_Projects\FinanzasNet\ModFinanzas\" + menuXMLNombreFichero;
                    string strXMLFile = menuXMLPathFichero + "\\" + menuXMLNombreFichero;
                    
                    this.menuXML.Save(strXMLFile);

                    //Adicionar Opción Especial a Todos los Ficheros XMLs de opciones de menus
                    if (addOpcionAllXMLFile) this.AddOpcionAllXMLFile(nodo.Id, menuItem.Id, menuItem.Orden.ToString(), menuItem.Programa, menuItem.Proceso, menuItem.Text, menuItem.IdPadre, menuItem.Icono);

                    //Eliminar Opción Especial a Todos los Ficheros XMLs de opciones de menus
                    if (delOpcionAllXMLFile) this.DelOpcionAllXMLFile(nodo.Id);

                }
            }
            catch(Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //throw new NotImplementedException();
        }

        private void treeViewMenu_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void treeViewMenu_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeViewMenu_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point loc = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

                MenuItem nodoOrigen = (MenuItem)e.Data.GetData(typeof(MenuItem));
                MenuItem nodoDestino = ((MenuItem)((TreeView)sender).GetNodeAt(loc));

                bool saveXMLFile = false;
                XmlNodeList baseNodeList = this.menuXML.SelectNodes("MenuModuloFinanzas/nodo");

                //Sólo para Elementos y no Nodos del TreeView
                //if (nodoOrigen.Nodes.Count == 0)
                //{
                    string idNodoDestino = nodoDestino.Id;
                    if (idNodoDestino != "" && idNodoDestino.Length >= 2 && idNodoDestino.Substring(0, 2) == "MX")
                    {
                        //Menú Personalizado (Nodo destino es del Menú Personalizado)
                        
                        //Caso reordenar
                        if (nodoOrigen.Parent == nodoDestino.Parent)
                        {
                            this.OrdenarItemTreeView(nodoOrigen, nodoDestino, ref saveXMLFile);
                        }
                        else
                        {
                            //Caso insertar nuevo elemento al menú personalizado
                            //Verificar que no exista ya la opcion (sólo puede aparecer una vez dentro de la carpeta, solo uno por nodo padre directo)
                            if (!ExisteNodo(nodoDestino, nodoOrigen))
                            {

                                //Crear el nuevo nodo partiendo del nodo origen
                                MenuItem nodoNuevo = new MenuItem();
                                nodoNuevo.Activo = nodoOrigen.Activo;
                                nodoNuevo.Icono = nodoOrigen.Icono;
                                int indiceImagen = IndiceImagen(nodoOrigen.Icono);
                                nodoNuevo.ImageIndex = indiceImagen;
                                nodoNuevo.SelectedImageIndex = indiceImagen;
                                nodoNuevo.Custom = nodoOrigen.Custom;
                                nodoNuevo.Programa = nodoOrigen.Programa;
                                nodoNuevo.Proceso = nodoOrigen.Proceso;
                                nodoNuevo.TextoDefecto = nodoOrigen.TextoDefecto;
                                nodoNuevo.Text = nodoOrigen.Text;
                                nodoNuevo.IdPadre = nodoOrigen.Id;

                                if (nodoDestino.Programa != "")
                                {
                                    //El nodo destino es un nodo final, poner como nodo destino el nodo padre porque tiene que ser de tipo nodo y no de tipo item
                                    nodoDestino = ((MenuItem)nodoDestino.Parent);
                                }

                                //Buscar el Orden que le corresponde
                                int orden = BuscarOrdenNodo(nodoDestino);

                                //Buscar el id para el nodo nuevo
                                string id = nodoDestino.Id;
                                if (nodoDestino.Nodes.Count > 0)
                                {
                                    //Tiene hijos
                                    id += "_" + orden.ToString();
                                }
                                else
                                {
                                    //No tiene hijos
                                    string idAux = id;
                                    int pos = idAux.LastIndexOf('_');
                                    if (pos != -1)
                                        id = idAux.Substring(0, pos) + "_" + orden.ToString();
                                    else id += "_" + orden.ToString();
                                }

                                nodoNuevo.Id = id;
                                nodoNuevo.Orden = orden;

                                //Ponerle el menú derecho al nodo
                                NodoAddMenuClickDerecho(ref nodoNuevo);

                                nodoDestino.Nodes.Insert(nodoDestino.Index + 1, nodoNuevo);

                                /*if (nodoDestino.Parent == null)
                                    nodoDestino.TreeView.Nodes.Insert(nodoDestino.Index + 1, nodoNuevo);
                                else
                                    nodoDestino.Parent.Nodes.Insert(nodoDestino.Index + 1, nodoNuevo);
                                */


                                //Adicionar la nueva opción en el fichero XML
                                this.AddItemToXML(ref this.menuXML, nodoDestino.Id, baseNodeList, nodoNuevo.Id, nodoNuevo.Orden.ToString(), nodoNuevo.Programa, nodoNuevo.Proceso, nodoNuevo.Text, nodoOrigen.Id, nodoOrigen.Icono);
                                saveXMLFile = true;
                            }
                        }
                    }
                    else
                    {
                        //Caso Reordenar
                        //if (nodoDestino.Nodes.Count == 0 && nodoOrigen.Parent == nodoDestino.Parent)
                        if (nodoOrigen.Parent == nodoDestino.Parent)
                        {
                            this.OrdenarItemTreeView(nodoOrigen, nodoDestino, ref saveXMLFile);
                        }
                    }
                //}

                if (saveXMLFile)
                {
                    //string strXMLFile = @"C:\VS2010_Projects\FinanzasNet\ModFinanzas\" + menuXMLNombreFichero;
                    string strXMLFile = menuXMLPathFichero + "\\" + menuXMLNombreFichero;
                    this.menuXML.Save(strXMLFile);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void activarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmActivarOpciones frmActivar = new frmActivarOpciones();
            frmActivar.MenuXML = this.menuXML;
            frmActivar.MenuXMLNombreFichero = menuXMLNombreFichero;
            frmActivar.MenuXMLPathFichero = menuXMLPathFichero;
            frmActivar.FrmPadre = this;
            frmActivar.ShowDialog();
            if (frmActivar.ActivarOpciones)
            {
                //Cargar el árbol del formulario principal del módulo de finanzas (nuevas opciones activadas)
                this.CargarMenuFinanzas();
                this.treeViewMenu.Refresh();
            }
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            frmAbout frmabout = new frmAbout();
            frmabout.FrmPadre = this;
            frmabout.ShowDialog();
        }

        private void treeViewMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeViewHitTestInfo info = this.treeViewMenu.HitTest(e.X, e.Y);
                if (info != null)
                {
                    this.treeViewMenu.SelectedNode = info.Node;
                }
            }
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                menuItemSalir_Click(sender, null);
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo de Finanzas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo Finanzas");
            //Menú
            this.menuItemConfig.Text = this.LP.GetText("menuItemConfig", "Configuración");
            /*this.menuItemAbout.Text = this.LP.GetText("menuItemAbout", "Acerca de");
            this.menuItemSalir.Text = this.LP.GetText("menuItemSalir", "Salir");*/

            if (this.treeViewMenu.Nodes.Count > 0)
            {
                XmlNodeList baseNodeList = this.menuXML.SelectNodes("MenuModuloFinanzas/nodo");
                this.TraducirLiteralesTreeView();
            }
        }

        /// <summary>
        /// Traducir los literales del TreeView en el idioma que corresponda
        /// </summary>
        private void TraducirLiteralesTreeView()
        {
            //Traducir cada nodo recursivamente
            MenuItem item = null;

            string texto;
            
            foreach (TreeNode node in this.treeViewMenu.Nodes)
            {
                item = ((MenuItem)node);

                if (item.Id != "")
                {
                    texto = this.LP.GetText(item.Id);
                    if (texto != "")
                    {
                        item.Text = texto;
                    }
                }

                this.TraducirLiteralesNodos(node);
            }
        }

        /// <summary>
        /// Traducir los literales de los nodos en el idioma que corresponda
        /// </summary>
        private void TraducirLiteralesNodos(TreeNode treeNode)
        {
            MenuItem item = null;

            string texto;

            foreach (TreeNode node in treeNode.Nodes)
            {
                item = ((MenuItem)node);

                if (item.Id != "")
                {
                    texto = this.LP.GetText(item.Id);
                    if (texto != "")
                    {
                        item.Text = texto;
                    }
                }

                this.TraducirLiteralesNodos(node);
            }
        }

        /// <summary>
        /// Limpiar la LDA
        /// </summary>
        private void LimpiarLDA()
        {
            try
            {
                string pathLDA = Environment.GetEnvironmentVariable("BASETMP");
                pathLDA = pathLDA + @"\TMP\";
                pathLDA = pathLDA + Environment.UserName;

                if (Directory.Exists(pathLDA))
                {
                    string[] lda = Directory.GetFiles(pathLDA, "lda*.dta");
                    foreach (string f in lda)
                    {
                        File.Delete(f);
                    }
                }
                else
                {
                    Directory.CreateDirectory(pathLDA);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Chequea que exista el fichero menú para el usuario logado. De no existir lo crea igual que el fichero genérico de menú
        /// </summary>
        private void ChequearExisteFicheroMenu()
        {
            string strXMLFileUsuario = menuXMLPathFichero + "\\" + menuXMLNombreFichero;

            if (!File.Exists(strXMLFileUsuario))
            {
                //Crear el fichero de menú
                try
                {
                    string strXMLFileGenerico = menuXMLPathFichero + "\\" + menuXMLNombreFicheroGenerico;
                    File.Copy(strXMLFileGenerico, strXMLFileUsuario);
                }
                catch(Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    this.lblResult.Text = this.LP.GetText("errCargarOpciones", "No es posible cargar las opciones del Módulo de Finanzas");
                    this.lblResult.Text += " \n\r\n\r(" + ex.Message + ")";
                    this.lblResult.Visible = true;
                    this.gbFinanzas.Visible = false;
                }
            }
        }

        /// <summary>
        /// Cargar el TreeView partiendo del menú de Finanzas
        /// </summary>
        private void CargarMenuFinanzas()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //string strXPath = "MenuModuloFinanzas/nodo";
            //string strRootNode = "Finanzas";
            //string strXMLFile = @"C:\VS2010_Projects\FinanzasNet\ModFinanzas\" + menuXMLNombreFichero;
            string strXMLFile = menuXMLPathFichero + "\\" + menuXMLNombreFichero;

            try
            {
                this.menuXML = new XmlDocument();
                // Load the XML file.
                this.menuXML.Load(strXMLFile);

                //Cargar las imagenes del TreeView
                this.CargarImagenesMenu();

                this.PopulateBaseNodes(this.menuXML);

                //this.treeViewMenu.CollapseAll();
                //this.treeViewMenu.Nodes[0].Expand();

                this.treeViewMenu.CollapseAll();
                this.treeViewMenu.SelectedNode = this.treeViewMenu.Nodes[0];
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

        }

        /// <summary>
        /// Carga las imágenes que se utilizarán en el TreeView del menú de Finanzas
        /// </summary>
        private void CargarImagenesMenu()
        {
            //string pathApp = Application.StartupPath;
            //string path = @"C:\VS2010_Projects\FinanzasNet\ModFinanzas\imagenes\";
            string pathGenericoImagenes = ConfigurationManager.AppSettings["ModFinan_Imagenes"];
            if (pathGenericoImagenes == null || pathGenericoImagenes == "") pathGenericoImagenes = "\\images\\ModFinanzas\\";
            string path = Application.StartupPath + pathGenericoImagenes;
            //this.menuImageList = new ImageList();
            ImageList imageListAux = new ImageList();
            this.menuImageArray = new ArrayList();

            this.CargarImagen(path, "menuDefecto.png", ref imageListAux);
            this.CargarImagen(path, "menuModBasico.png", ref imageListAux);
            this.CargarImagen(path, "menuDefGenerales.png", ref imageListAux);
            this.CargarImagen(path, "menuParametros.png", ref imageListAux);
            this.CargarImagen(path, "menuAutorizaciones.png", ref imageListAux);
            this.CargarImagen(path, "menuListar.png", ref imageListAux);
            this.CargarImagen(path, "menuConexionesUser.png", ref imageListAux);
            this.CargarImagen(path, "menuCompaniaGrupo.png", ref imageListAux);
            this.CargarImagen(path, "menuActualizar.png", ref imageListAux);
            this.CargarImagen(path, "menuCalendario.png", ref imageListAux);
            this.CargarImagen(path, "menuTabla.png", ref imageListAux);
            this.CargarImagen(path, "menuVerOtraMoneda.png", ref imageListAux);
            this.CargarImagen(path, "menuCambiarUser.png", ref imageListAux);
            this.CargarImagen(path, "menuMantenimiento.png", ref imageListAux);
            this.CargarImagen(path, "menuReorganizar.png", ref imageListAux);
            this.CargarImagen(path, "menuGenArchEntBatch.png", ref imageListAux);
            this.CargarImagen(path, "menuAprobacion.png", ref imageListAux);
            this.CargarImagen(path, "menuContabilizar.png", ref imageListAux);
            this.CargarImagen(path, "menuCierre.png", ref imageListAux);
            this.CargarImagen(path, "menuContGeneral.png", ref imageListAux);

            this.CargarImagen(path, "menuLibroAuxCont.png", ref imageListAux);
            this.CargarImagen(path, "menuConsulta.png", ref imageListAux);
            this.CargarImagen(path, "menuGeneradorInfo.png", ref imageListAux);
            this.CargarImagen(path, "menuGestionTerceros.png", ref imageListAux);
            this.CargarImagen(path, "menuTesoreria.png", ref imageListAux);
            this.CargarImagen(path, "menuTransferir.png", ref imageListAux);
            this.CargarImagen(path, "menuEmpresa.png", ref imageListAux);
            this.CargarImagen(path, "menuEntradaDatos.png", ref imageListAux);            
            this.CargarImagen(path, "menuDatosExtraCont.png", ref imageListAux);
            this.CargarImagen(path, "menuGenerarInfo.png", ref imageListAux);
            this.CargarImagen(path, "menuInfo.png", ref imageListAux);
            this.CargarImagen(path, "menuIVA.png", ref imageListAux);
            this.CargarImagen(path, "menuAnalisisVto.png", ref imageListAux);
            this.CargarImagen(path, "menuBorrarDatos.png", ref imageListAux);
            this.CargarImagen(path, "menuCambiar.png", ref imageListAux);
            this.CargarImagen(path, "menuValidarListar.png", ref imageListAux);
            this.CargarImagen(path, "menuAnularComprob.png", ref imageListAux);
            this.CargarImagen(path, "menuNumerarMovIVA.png", ref imageListAux);
            this.CargarImagen(path, "menuDeclaracion.png", ref imageListAux);
            this.CargarImagen(path, "menuGenerarArchivo.png", ref imageListAux);
            this.CargarImagen(path, "menuEstructuras.png", ref imageListAux);
            this.CargarImagen(path, "menuCobrosTerceros.png", ref imageListAux);
            this.CargarImagen(path, "menuPagosTerceros.png", ref imageListAux);
            this.CargarImagen(path, "menuPagosParam.png", ref imageListAux);
            this.CargarImagen(path, "menuCambioPlan.png", ref imageListAux);
            this.CargarImagen(path, "menuCambioPlanComp.png", ref imageListAux);
            this.CargarImagen(path, "menuOpcionesEspeciales.png", ref imageListAux);
            this.CargarImagen(path, "menuPersonalizado.png", ref imageListAux);
            this.CargarImagen(path, "menuAccesoDirecto.png", ref imageListAux);
            this.CargarImagen(path, "menuGerSpool.png", ref imageListAux);
            this.CargarImagen(path, "menuGerProces.png", ref imageListAux);
           
            
            //Cargar las imagenes con el color de fondo del treeview
            this.menuImageList = new ImageList();

            foreach (Image image in imageListAux.Images)
            {
                Bitmap bitmap = new Bitmap(imageListAux.ImageSize.Width,
                  imageListAux.ImageSize.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(this.treeViewMenu.BackColor);
                    g.DrawImage(image, 0, 0);
                }
                this.menuImageList.Images.Add(bitmap);
            }

            this.treeViewMenu.ImageList = this.menuImageList;
        }

        /// <summary>
        /// Crea la lista auxiliar de imagenes para el treeview
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nombre"></param>
        /// <param name="imageListAux"></param>
        private void CargarImagen(string path, string nombre, ref ImageList imageListAux)
        {
            try
            {
                //this.menuImageList.Images.Add(Image.FromFile(path + nombre));
                imageListAux.Images.Add(Image.FromFile(path + nombre));
                this.menuImageArray.Add(nombre);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Llenar el TreeView con los nodos bases
        /// </summary>
        /// <param name="dom"></param>
        private void PopulateBaseNodes(XmlDocument dom)
        {
            this.treeViewMenu.Nodes.Clear(); // Clear any existing items
            this.treeViewMenu.BeginUpdate(); // Begin updating the treeview

            TreeNode treenode;
            //treenode = treeViewMenu.Nodes.Add("Finanzas");

            XmlNodeList baseNodeList = dom.SelectNodes("MenuModuloFinanzas/nodo");
            // Get all first level <folder> nodes

            MenuItem nodoTreeView;
            int ordenUlt = 0;
            int ordenAct = 0;
            foreach (XmlNode xmlnode in baseNodeList)
            // loop through all base <folder> nodes 
            {
                nodoTreeView = CrearNodeTreeView(xmlnode);
                if (nodoTreeView.Activo)
                {
                    ordenAct = nodoTreeView.Orden;
                    //string title = xmlnode.Attributes["id"].Value;
                    //treenode = treeViewMenu.Nodes.Add(title); // add it to the tree

                    if (this.treeViewMenu.Nodes.Count > 0)
                    {
                        //buscar el número de orden del último nodo insertado
                        ordenUlt = ((MenuItem)this.treeViewMenu.Nodes[this.treeViewMenu.Nodes.Count - 1]).Orden;
                    }

                    int i = 0;
                    if (ordenUlt == 0 || ordenAct > ordenUlt)
                    {
                        //insertarlo al final
                        i = this.treeViewMenu.Nodes.Add(nodoTreeView); // add it to the tree
                    }
                    else
                    {
                        //Buscar el índice donde se ha de insertar (teniendo en cuenta el orden del nodo que se va a adicionar)
                        i = BuscarIndexTreeViewInsertNodoOrden(this.treeViewMenu.Nodes, ordenAct);
                        this.treeViewMenu.Nodes.Insert(i, nodoTreeView);
                    }
                    
                    treenode = this.treeViewMenu.Nodes[i];

                    PopulateChildNodes(xmlnode, treenode); // Get the children
                }
            }

            treeViewMenu.EndUpdate(); // Stop updating the tree
            treeViewMenu.Refresh(); // refresh the treeview display
        }

        /// <summary>
        /// Llenar el TreeView con los nodos hijos
        /// </summary>
        /// <param name="oldXmlnode"></param>
        /// <param name="oldTreenode"></param>
        private void PopulateChildNodes(XmlNode oldXmlnode, TreeNode oldTreenode)
        {
            XmlNodeList childNodeList = oldXmlnode.ChildNodes;
            // Get all children for the past node (parent)

            MenuItem nodoTreeView = null;
            TreeNode treenode;
            int ordenUlt = 0;
            int ordenAct = 0;
            foreach (XmlNode xmlnode in childNodeList)
            // loop through all children
            {
                nodoTreeView = CrearNodeTreeView(xmlnode);
                if (nodoTreeView.Activo)
                {
                    ordenAct = nodoTreeView.Orden;
                    //string title = xmlnode.Attributes["id"].Value;
                    // add it to the parent node tree
                    //treenode = oldTreenode.Nodes.Add(title);

                    if (oldTreenode.Nodes.Count > 0)
                    {
                        //buscar el número de orden del último nodo insertado
                        ordenUlt = ((MenuItem)oldTreenode.Nodes[oldTreenode.Nodes.Count - 1]).Orden;
                    }

                    int i = 0;
                    if (ordenUlt == 0 || ordenAct > ordenUlt)
                    {
                        //insertarlo al final
                        i = oldTreenode.Nodes.Add(nodoTreeView); // add it to the tree
                    }
                    else
                    {
                        //Buscar el índice donde se ha de insertar (teniendo en cuenta el orden del nodo que se va a adicionar)
                        i = BuscarIndexTreeViewInsertNodoOrden(oldTreenode.Nodes, ordenAct);
                        oldTreenode.Nodes.Insert(i, nodoTreeView);
                    }

                    treenode = oldTreenode.Nodes[i];

                    PopulateChildNodes(xmlnode, treenode);
                }
            }
        }

        /// <summary>
        /// Verifica que no exista un nodo como hijo de otro nodo
        /// </summary>
        /// <returns></returns>
        private bool ExisteNodo(MenuItem nodoPadre, MenuItem nodoHijo)
        {
            bool result = false;
            MenuItem nodoAux;

            //Buscar si el nodo hijo no cuelga del nodo padre
            foreach (TreeNode node in nodoPadre.Nodes)
            {
                nodoAux = ((MenuItem)node);
                if ((nodoAux.IdPadre == nodoHijo.Id || nodoAux.IdPadre == nodoHijo.IdPadre) && nodoAux.IdPadre != "")
                {
                    result = true;
                    break;
                }
            }
            return (result);
        }

        /// <summary>
        /// Crea un nodo para el TreeView
        /// </summary>
        /// <param name="nodoXML">Nodo XML</param>
        /// <returns></returns>
        private MenuItem CrearNodeTreeView(XmlNode nodoXML)
        {
            MenuItem menuItem = new MenuItem();

            try
            {
                if (nodoXML.Attributes["id"] != null) menuItem.Id = nodoXML.Attributes["id"].Value;
                else menuItem.Id = "-1";
                if (nodoXML.Attributes["activo"] != null)
                {
                    menuItem.Activo = nodoXML.Attributes["activo"].Value == "1" ? true : false;
                }
                if (nodoXML.Attributes["orden"] != null) menuItem.Orden = Convert.ToInt16(nodoXML.Attributes["orden"].Value);
                if (nodoXML.Attributes["icono"] != null && nodoXML.Attributes["icono"].Value != "")
                {
                    menuItem.Icono = nodoXML.Attributes["icono"].Value;
                    int indiceImagen = IndiceImagen(menuItem.Icono);
                    menuItem.ImageIndex = indiceImagen;
                    menuItem.SelectedImageIndex = indiceImagen;
                }
                else
                {
                    menuItem.Icono = "";
                    menuItem.ImageIndex = -1;
                    menuItem.SelectedImageIndex = -1;
                }
                if (nodoXML.Attributes["custom"] != null)
                {
                    menuItem.Custom = nodoXML.Attributes["custom"].Value == "1" ? true : false;
                }
                if (nodoXML.Attributes["programa"] != null) menuItem.Programa = nodoXML.Attributes["programa"].Value;
                if (nodoXML.Attributes["proceso"] != null) menuItem.Proceso = nodoXML.Attributes["proceso"].Value;
                if (nodoXML.Attributes["textoDefecto"] != null) menuItem.TextoDefecto = nodoXML.Attributes["textoDefecto"].Value;

                //Texto que aparecerá en el TreeView
                string textoNodo = this.BuscarTextoNodo(menuItem.Id, menuItem.TextoDefecto);
                menuItem.Text = textoNodo;

                if (nodoXML.Attributes["idPadre"] != null) menuItem.IdPadre = nodoXML.Attributes["idPadre"].Value;
                
                //Adicionar las opciones del menú con click derecho
                this.NodoAddMenuClickDerecho(ref menuItem);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (menuItem);
        }

        /// <summary>
        /// Devuelve el texto del nodo
        /// </summary>
        /// <param name="id">identificador del nodo</param>
        /// <param name="textoDefecto">texto por defecto para el nodo</param>
        /// <returns></returns>
        private string BuscarTextoNodo(string id, string textoDefecto)
        {
            string result = "";

            if (id != "-1") result = this.LP.GetText(id, textoDefecto);
            else result = textoDefecto;

            if (result == "") result = id;
            return (result);
        }

        /// <summary>
        /// Dado una imagen, busca el índice que le corresponde dentro del array de imagenes
        /// </summary>
        /// <param name="imagen"></param>
        /// <returns></returns>
        private int IndiceImagen(string imagen)
        {
            int indice = 0;

            foreach (string imagenAct in this.menuImageArray)
            {
                if (imagenAct == imagen)
                {
                    return (indice);
                }
                else indice++;
            }
            return (-1);
        }

        /// <summary>
        /// Llama al Programa con un proceso
        /// </summary>
        /// <param name="programa"></param>
        /// <param name="proceso"></param>
        private void LlamarPrograma(string programa, string proceso)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(programa, proceso);
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        /// <summary>
        /// Llama al Programa
        /// </summary>
        /// <param name="programa"></param>
        private void LlamarPrograma(string programa)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(programa);
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }


        /// <summary>
        /// Adiciona el menú que aparece al hacer click derecho encima del nodo
        /// </summary>
        /// <param name="nodo"></param>
        private void NodoAddMenuClickDerecho(ref MenuItem nodo)
        {
            //Create the ContextMenuStrip.
            ContextMenuStrip menuDerecho = new ContextMenuStrip();

            //Opción Desactivar válida para todos los Nodos
            ToolStripMenuItem desactivarLabel = new ToolStripMenuItem();
            desactivarLabel.Text = this.LP.GetText("lblDesactivar", "Desactivar");
            desactivarLabel.Tag = "Desactivar";

            menuDerecho.Items.Add(desactivarLabel);

            //Opciones Menú Especiales
            if (nodo.Id == "MO")
            {
                //Opciones Menú Especiales
                ToolStripMenuItem opc1_MO_Label = new ToolStripMenuItem();
                opc1_MO_Label.Text = this.LP.GetText("lblAddOpcion", "Añadir Opción");
                opc1_MO_Label.Tag = "AddOpcion";

                menuDerecho.Items.Add(opc1_MO_Label);
            }
            else
            {
                //Buscar si el Id del nodo comienza por MO_
                if (nodo.Id.Length >= 3 && nodo.Id.Substring(0, 3) == "MO_")
                {
                    /*ToolStripMenuItem opc1_MO_HijoLabel = new ToolStripMenuItem();
                    opc1_MO_HijoLabel.Text = this.LP.GetText("lblAddOpcion", "Añadir Opción");
                    opc1_MO_HijoLabel.Tag = "AddOpcion";*/
                    ToolStripMenuItem opc2_MO_HijoLabel = new ToolStripMenuItem();
                    opc2_MO_HijoLabel.Text = this.LP.GetText("lblDelOpcion", "Quitar Opción");
                    opc2_MO_HijoLabel.Tag = "DelOpcion";

                    //menuDerecho.Items.Add(opc1_MO_HijoLabel);
                    menuDerecho.Items.Add(opc2_MO_HijoLabel);
                }
            }

            //Opciones Menú Personalizado
            if (nodo.Id == "MX")
            {
                //Opciones Menú Personalizado
                ToolStripMenuItem opc1_MX_Label = new ToolStripMenuItem();
                opc1_MX_Label.Text = this.LP.GetText("lblAddGrupo", "Añadir Grupo");
                opc1_MX_Label.Tag = "AddGrupo";

                menuDerecho.Items.Add(opc1_MX_Label);
            }
            else
            {
                //Buscar si el Id del nodo comienza por MX_
                if (nodo.Id.Length >= 3 && nodo.Id.Substring(0, 3) == "MX_")
                {
                    //Opciones Menú Personalizado
                    ToolStripMenuItem opc1_MX_HijoLabel = new ToolStripMenuItem();
                    opc1_MX_HijoLabel.Text = this.LP.GetText("lblAddGrupo", "Añadir Grupo");
                    opc1_MX_HijoLabel.Tag = "AddGrupo";
                    ToolStripMenuItem opc2_MX_HijoLabel = new ToolStripMenuItem();
                    opc2_MX_HijoLabel.Text = this.LP.GetText("lblDelGrupoItem", "Quitar Grupo/Elemento");
                    opc2_MX_HijoLabel.Tag = "DelGrupoItem";

                    menuDerecho.Items.Add(opc1_MX_HijoLabel);
                    menuDerecho.Items.Add(opc2_MX_HijoLabel);
                }
            }

            //Adicionar el Handler al menuDerecho para que capture el evento Click
            menuDerecho.ItemClicked += new ToolStripItemClickedEventHandler(menuDerecho_ItemClicked);

            // Set the ContextMenuStrip property to the ContextMenuStrip.
            nodo.ContextMenuStrip = menuDerecho; 
        }

        /// <summary>
        /// Adiciona un nodo dentro del fichero XML
        /// </summary>
        /// <param name="idNodo">identificador del Nodo Padre</param>
        /// <param name="baseNodeList">Nodo Base</param>
        /// <param name="id">valor del identificador del item</param>
        /// <param name="orden">valor del orden</param>
        /// <param name="texto">valor del texto</param>
        private void AddNodeToXML(string idNodo, XmlNodeList baseNodeList, string id, string orden, string texto)
        {
            string idNode;
            foreach (XmlNode xmlnode in baseNodeList)
            {
                idNode = "";
                if (xmlnode.Attributes["id"] != null)
                {
                    idNode = xmlnode.Attributes["id"].Value;
                }
                if (idNode == idNodo)
                {
                    //Crear un item              
                    XmlElement item = this.menuXML.CreateElement("nodo");
                    item.SetAttribute("id", id);
                    item.SetAttribute("activo", "1");
                    item.SetAttribute("orden", orden);
                    item.SetAttribute("icono", "");
                    item.SetAttribute("textoDefecto", texto);

                    xmlnode.AppendChild(item);
                    return;
                }
                else
                {
                    if (xmlnode.HasChildNodes)
                    {
                        this.AddNodeToXML(idNodo, xmlnode.ChildNodes, id, orden, texto);
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona un item dentro del fichero XML
        /// </summary>
        /// <param name="xmlMenu">fichero menu xml cargado</param>
        /// <param name="idNodo">identificador del Nodo Padre</param>
        /// <param name="baseNodeList">Nodo Base</param>
        /// <param name="id">valor del identificador del item</param>
        /// <param name="orden">valor del orden</param>
        /// <param name="programa">valor del programa</param>
        /// <param name="proceso">valor del proceso</param>
        /// <param name="texto">valor del texto</param>
        private void AddItemToXML(ref XmlDocument xmlMenu, string idNodo, XmlNodeList baseNodeList, string id, string orden, string programa, string proceso, string texto, string idPadre, string icono)
        {
            string idNode;
            foreach (XmlNode xmlnode in baseNodeList)
            {
                idNode = "";
                if (xmlnode.Attributes["id"] != null)
                {
                    idNode = xmlnode.Attributes["id"].Value;
                }
                if (idNode == idNodo)
                {
                    //Crear un item              
                    XmlElement item = xmlMenu.CreateElement("item");
                    item.SetAttribute("id", id);
                    item.SetAttribute("activo", "1");
                    item.SetAttribute("orden", orden);
                    item.SetAttribute("icono", icono);
                    item.SetAttribute("programa", programa);
                    item.SetAttribute("proceso", proceso);
                    item.SetAttribute("textoDefecto", texto);
                    if (idPadre != null && idPadre != "")
                        item.SetAttribute("idPadre", idPadre);

                    xmlnode.AppendChild(item);
                    return;
                }
                else
                {
                    if (xmlnode.HasChildNodes)
                    {
                        this.AddItemToXML(ref xmlMenu, idNodo, xmlnode.ChildNodes, id, orden, programa, proceso, texto, idPadre, icono);
                    }
                }
            }
        }

        /// <summary>
        /// Ordena dos nodos dentro del árbol
        /// </summary>
        /// <param name="nodoOrigen"></param>
        /// <param name="nodoDestino"></param>
        private void OrdenarItemTreeView(MenuItem nodoOrigen, MenuItem nodoDestino, ref bool saveXMLFile)
        {
            //Elementos finales del mismo padre
            int ordenOrigen = nodoOrigen.Orden;
            int ordenDestino = nodoDestino.Orden;

            nodoOrigen.Orden = ordenDestino;
            nodoDestino.Orden = ordenOrigen;

            //Eliminar el nodo origen
            this.treeViewMenu.Nodes.Remove(nodoOrigen);

            TreeNodeCollection nodos;

            //Insertarlo en la posicion correcta
            if (nodoDestino.Parent != null)
            {
                nodoDestino.Parent.Nodes.Insert(nodoDestino.Index, nodoOrigen);
                //Nodos que cuelgan del nodo origen (para actualizar después el fichero XML)
                nodos = nodoOrigen.Parent.Nodes;
            }
            else
            {
                //Nodo de la Raíz del árbol
                this.treeViewMenu.Nodes.Insert(nodoDestino.Index, nodoOrigen);
                //Nodos que cuelgan de la raíz del árbol (para actualizar después el fichero XML)
                nodos = this.treeViewMenu.Nodes;
            }

            this.treeViewMenu.Refresh();

            //Actualizar el orden en el fichero XML de todos los nodos que cuelgan del padre
            MenuItem nodoAux;
            int orden = 1;

            XmlNodeList baseNodeList = this.menuXML.SelectNodes("MenuModuloFinanzas/nodo");

            foreach (TreeNode node in nodos)
            {
                nodoAux = ((MenuItem)node);
                this.ActualizarOrdenNodeToXML(orden, nodoAux.Id, baseNodeList);
                orden++;
                saveXMLFile = true;
            }
        }

        /// <summary>
        /// Activa o desactiva un nodo
        /// </summary>
        /// <param name="orden">valor del atributo orden</param>
        /// <param name="idNodo">identificador del Nodo Padre</param>
        /// <param name="baseNodeList">Nodo Base</param>
        private void ActualizarOrdenNodeToXML(int orden, string idNodo, XmlNodeList baseNodeList)
        {
            string idNode;
            foreach (XmlNode xmlnode in baseNodeList)
            {
                idNode = "";
                if (xmlnode.Attributes["id"] != null)
                {
                    idNode = xmlnode.Attributes["id"].Value;
                }
                if (idNode == idNodo)
                {
                    xmlnode.Attributes["orden"].Value = orden.ToString();
                    return;
                }
                else
                {
                    if (xmlnode.HasChildNodes)
                    {
                        this.ActualizarOrdenNodeToXML(orden, idNodo, xmlnode.ChildNodes);
                    }
                }
            }
        }


        private void ModifyNodeToXML()
        {
        }

        /// <summary>
        /// Elimina un nodo dentro de un fichero xml
        /// </summary>
        /// <param name="idNodo"></param>
        /// <param name="baseNodeList"></param>
        private void DeleteNodeToXML(string idNodo, XmlNodeList baseNodeList)
        {
            string idNode;
            foreach (XmlNode xmlnode in baseNodeList)
            {
                idNode = "";
                if (xmlnode.Attributes["id"] != null)
                {
                    idNode = xmlnode.Attributes["id"].Value;
                }
                if (idNode == idNodo)
                {
                    xmlnode.RemoveAll();
                    xmlnode.ParentNode.RemoveChild(xmlnode);

                    return;
                }
                else
                {
                    if (xmlnode.HasChildNodes)
                    {
                        this.DeleteNodeToXML(idNodo, xmlnode.ChildNodes);
                    }
                }
            }
        }

        private void DeleteItemToXML()
        {
        }

        /// <summary>
        /// Buscar el orden que le corresponde a un nodo nuevo dentro del TreeView 
        /// </summary>
        /// <param name="nodoDestino"></param>
        /// <returns></returns>
        private int BuscarOrdenNodo(MenuItem nodoDestino)
        {
            int orden = 0;

            try
            {
                if (nodoDestino.Nodes.Count > 0)
                {
                    //Tiene hijos
                    foreach (MenuItem nodo in nodoDestino.Nodes)
                    {
                        if (nodo.Orden > orden) orden = nodo.Orden;
                    }


                    //orden = nodoDestino.Nodes.Count + 1;
                }
                else
                {
                    //No tiene hijos
                    if (nodoDestino.Parent != null)
                    {
                        foreach (MenuItem nodo in nodoDestino.Parent.Nodes)
                        {
                            if (nodo.Orden > orden) orden = nodo.Orden;
                        }
                        //orden = nodoDestino.Parent.Nodes.Count + 1;
                    }
                }

                orden = orden + 1;

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (orden);
        }

        /// <summary>
        /// Buscar el orden que le corresponde a un nodo nuevo dentro del TreeView 
        /// </summary>
        /// <param name="nodoDestino"></param>
        /// <returns></returns>
        private int BuscarIndexTreeViewInsertNodoOrden(TreeNodeCollection nodos, int ordenNodoAInsertar)
        {
            int pos = 0;
            if (nodos.Count > 0)
            {
                //Tiene hijos
                foreach (MenuItem nodo in nodos)
                {
                    if (nodo.Orden > ordenNodoAInsertar)
                        return (pos);
                    else
                        pos++;
                }
            }
            return (pos);
        }

        /// <summary>
        /// Obtener la lista de ficheros de opciones de menus (MenuFinanzas*.xml)
        /// </summary>
        /// <returns></returns>
        private FileInfo[] ObtenerFicheros()
        {
            FileInfo[] fileList = null;
            
            try
            {
                DirectoryInfo dir = new DirectoryInfo(menuXMLPathFichero);

                string[] ficheroExtension = menuXMLNombreFicheroGenerico.Split('.');
                string ficheros = "";
                if (ficheroExtension.Length == 2)
                {
                    ficheros = ficheroExtension[0] + "*" + "." + ficheroExtension[1];

                    fileList = dir.GetFiles(ficheros, SearchOption.AllDirectories);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (fileList);
        }

        /// <summary>
        /// Adiciona la opción especial a todos los ficheros de menus que existen en el PC (excepto al fichero menú del usuario logado)
        /// </summary>
        private void AddOpcionAllXMLFile(string idNodo, string id, string orden, string programa, string proceso, string texto, string idPadre, string icono)
        {
            try
            {
                FileInfo[] fileList = ObtenerFicheros();
                XmlDocument xmlActual;
                string strXMLFile;

                if (fileList != null)
                {
                    foreach (FileInfo FI in fileList)
                    {
                        if (FI.Name != menuXMLNombreFichero)
                        {
                            try
                            {
                                strXMLFile = menuXMLPathFichero + "\\" + FI.Name;

                                xmlActual = new XmlDocument();

                                //Load the XML file.
                                xmlActual.Load(strXMLFile);

                                XmlNodeList baseNodeList = xmlActual.SelectNodes("MenuModuloFinanzas/nodo");

                                //Buscar el orden que le corresponde al nodo a insertar en el fichero XML (último utilizado + 1)
                                bool result = false;
                                int ordenNodoXML = BuscarOrdenNodoXML(idNodo, baseNodeList, ref result);
                                if (ordenNodoXML != 0)
                                {
                                    orden = ordenNodoXML.ToString();
                                }
                                
                                //Insertar nodo en el fichero xml
                                this.AddItemToXML(ref xmlActual, idNodo, baseNodeList, id, orden, programa, proceso, texto, idPadre, icono);
                                //Salvar el fichero xml
                                xmlActual.Save(strXMLFile);
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Quitar la opción especial a todos los ficheros de menus que existen en el PC (excepto al fichero menú del usuario logado)
        /// </summary>
        private void DelOpcionAllXMLFile(string idNodo)
        {
            try
            {
                FileInfo[] fileList = ObtenerFicheros();
                XmlDocument xmlActual;
                string strXMLFile;

                if (fileList != null)
                {
                    foreach (FileInfo FI in fileList)
                    {
                        if (FI.Name != menuXMLNombreFichero)
                        {
                            try
                            {
                                strXMLFile = menuXMLPathFichero + "\\" + FI.Name;

                                xmlActual = new XmlDocument();

                                //Load the XML file.
                                xmlActual.Load(strXMLFile);

                                XmlNodeList baseNodeList = xmlActual.SelectNodes("MenuModuloFinanzas/nodo");
                                this.DeleteNodeToXML(idNodo, baseNodeList);

                                //Salvar el fichero xml
                                xmlActual.Save(strXMLFile);
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Buscar el orden que tendría un nuevo nodo que será insertado
        /// </summary>
        /// <param name="idNodo"></param>
        /// <param name="baseNodeList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private int BuscarOrdenNodoXML(string idNodo, XmlNodeList baseNodeList, ref bool result)
        {
            int orden = 0;
            
            if (!result)
            {
                string idNode;
                int ordenActual;

                foreach (XmlNode xmlnode in baseNodeList)
                {
                    ordenActual = 0;
                    idNode = "";
                    if (xmlnode.Attributes["id"] != null)
                    {
                        idNode = xmlnode.Attributes["id"].Value;
                    }
                    if (idNode == idNodo)
                    {
                        //Buscar el orden de todos los nodos hijos
                        foreach (XmlNode nodosHijos in xmlnode.ChildNodes)
                        {
                            if (nodosHijos.Attributes["orden"] != null)
                            {
                                try
                                {
                                    ordenActual = Convert.ToInt16(nodosHijos.Attributes["orden"].Value);
                                }
                                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                                if (ordenActual > orden) orden = ordenActual;
                            }
                        }
                        result = true;
                        return (orden+1);
                    }
                    else
                    {
                        if (xmlnode.HasChildNodes)
                        {
                            this.BuscarOrdenNodoXML(idNodo, xmlnode.ChildNodes, ref result);
                        }
                    }
                }
            }
            return (orden+1);
        }
        #endregion

    }

    /// <summary>
    /// Clase que describe a los elementos del TreeView
    /// </summary>
    public class MenuItem : TreeNode
    {
        /*
        TreeNode nodo = new TreeNode();
        nodo.Name = "";
        nodo.Tag = "";
        nodo.Text = "";
        nodo.ToolTipText = "";
        */


        #region Atributos y propiedades
        public string _id;
        public string Id
        {
            get
            {
                return (this._id);
            }
            set
            {
                this._id = value;
            }
        }

        public bool _activo;
        public bool Activo
        {
            get
            {
                return (this._activo);
            }
            set
            {
                this._activo = value;
            }
        }

        public int _orden;
        public int Orden
        {
            get
            {
                return (this._orden);
            }
            set
            {
                this._orden = value;
            }
        }

        public bool _custom;
        public bool Custom
        {
            get
            {
                return (this._custom);
            }
            set
            {
                this._custom = value;
            }
        }

        public string _icono;
        public string Icono
        {
            get
            {
                return (this._icono);
            }
            set
            {
                this._icono = value;
            }
        }

        public string _programa;
        public string Programa
        {
            get
            {
                return (this._programa);
            }
            set
            {
                this._programa = value;
            }
        }

        public string _proceso;
        public string Proceso
        {
            get
            {
                return (this._proceso);
            }
            set
            {
                this._proceso = value;
            }
        }

        public string _textoDefecto;
        public string TextoDefecto
        {
            get
            {
                return (this._textoDefecto);
            }
            set
            {
                this._textoDefecto = value;
            }
        }

        /// <summary>
        /// Id del Nodo Padre
        /// (Se utiliza sólo para el Menú Personalizado para el multidioma, con este Id se recupera la etiqueta en el idioma que corresponda)
        /// </summary>
        public string _idPadre;
        public string IdPadre
        {
            get
            {
                return (this._idPadre);
            }
            set
            {
                this._idPadre = value;
            }
        }
        #endregion

        public MenuItem()
        {
            this._id = "";
            this._activo = true;
            this._custom = false;
            this._icono = "";
            this._programa = "";
            this._proceso = "";
            this._textoDefecto = "";
            this._idPadre = "";
        }
    }
}