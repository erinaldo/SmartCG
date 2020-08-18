using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ObjectModel;

namespace ModFinanzas
{
    public partial class frmActivarOpciones : frmPlantilla, IReLocalizable
    {
        /// <summary>
        /// Fichero XML Del Usuario
        /// </summary>
        private XmlDocument _menuXML;
        public XmlDocument MenuXML
        {
            get
            {
                return (this._menuXML);
            }
            set
            {
                this._menuXML = value;
            }
        }

        /// <summary>
        /// Nombre de los ficheros xml
        /// </summary>
        private string _menuXMLNombreFichero;
        public string MenuXMLNombreFichero
        {
            get
            {
                return (this._menuXMLNombreFichero);
            }
            set
            {
                this._menuXMLNombreFichero = value;
            }
        }

        /// <summary>
        /// Path donde se encuentran los ficheros xml
        /// </summary>
        private string _menuXMLPathFichero;
        public string MenuXMLPathFichero
        {
            get
            {
                return (this._menuXMLPathFichero);
            }
            set
            {
                this._menuXMLPathFichero = value;
            }
        }

        /// <summary>
        /// Devuelve true si se activó alguna opción
        /// </summary>
        private bool _activarOpciones;
        public bool ActivarOpciones
        {
            get
            {
                return (this._activarOpciones);
            }
            set
            {
                this._activarOpciones = value;
            }
        }

        public frmActivarOpciones()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmActivar_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Activar Opciones");

            this._activarOpciones = false;

            this.CargarOpcionesNoActivas();

            this.ChequearExistenOpcionesNoActivas();
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

                XmlNodeList baseNodeList = this._menuXML.SelectNodes("MenuModuloFinanzas/nodo");
                string textoNodo = this.BuscarTextoNodo(nodo.Id, nodo.TextoDefecto);
    
                //Pedir confirmación
                advertenciaTexto = this.LP.GetText("wrnActivarOpcion", "Se va a activar la opción");
                res = MessageBox.Show(advertenciaTexto + " \"" + textoNodo + " \"." + advertenciaPreg, advertenciaTitulo, MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    nodo.Activo = false;
                    TreeNode xmlnodoPadre = this.treeViewNoActiva.SelectedNode.Parent;
                    this.treeViewNoActiva.Nodes.Remove(this.treeViewNoActiva.SelectedNode);

                    if (xmlnodoPadre != null && xmlnodoPadre.Nodes.Count == 0)
                    {
                        //Si el nodo padre no tiene hijos, eliminar al nodo padre
                        this.treeViewNoActiva.Nodes.Remove(xmlnodoPadre);
                    }
                    this.treeViewNoActiva.Refresh();

                    //Buscar el nodo dentro del fichero XML y desactivarlo (poner el atributo activo a 0)                          
                    ActivateDesactivateNodeToXML(true, nodo.Id, baseNodeList);

                    //Grabar los cambios en el fichero XML
                    string strXMLFile = this._menuXMLPathFichero + "\\" + this._menuXMLNombreFichero;

                    this._menuXML.Save(strXMLFile);
                    this._activarOpciones = true;

                    //Chequea si existen opciones no activas
                    this.ChequearExistenOpcionesNoActivas();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            //throw new NotImplementedException();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeViewNoActiva_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeViewHitTestInfo info = this.treeViewNoActiva.HitTest(e.X, e.Y);
                if (info != null)
                {
                    this.treeViewNoActiva.SelectedNode = info.Node;
                }
            }
        }

        private void frmActivarOpciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Activar Opciones");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmActivarTitulo", "Activar Opciones");
            
            this.grBoxNoActiva.Text = this.LP.GetText("lblOpcionesNoActivas", "Opciones No Activas");
            this.btnSalir.Text = this.LP.GetText("menuItemSalir", "Salir");

            this.lblResult.Text = this.LP.GetText("wrnOpcionesActivas", "Todas las opciones están activas");
        }

        private void CargarOpcionesNoActivas()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                this.PopulateBaseNodes(this._menuXML);

                this.treeViewNoActiva.CollapseAll();
                if (this.treeViewNoActiva.Nodes.Count > 0) this.treeViewNoActiva.SelectedNode = this.treeViewNoActiva.Nodes[0];
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        /// <summary>
        /// Llenar el TreeView con los nodos bases
        /// </summary>
        /// <param name="dom"></param>
        private void PopulateBaseNodes(XmlDocument dom)
        {
            this.treeViewNoActiva.Nodes.Clear(); // Clear any existing items
            this.treeViewNoActiva.BeginUpdate(); // Begin updating the treeview

            TreeNode treenode;
            //treenode = treeViewMenu.Nodes.Add("Finanzas");

            XmlNodeList baseNodeList = dom.SelectNodes("MenuModuloFinanzas/nodo");
            // Get all first level <folder> nodes

            MenuItem nodoTreeView;
            int i;
            bool activo = true;
            foreach (XmlNode xmlnode in baseNodeList)
            // loop through all base <folder> nodes 
            {
                if (xmlnode.Attributes["activo"] != null)
                {
                    activo = xmlnode.Attributes["activo"].Value == "1" ? true : false;
                }
                
                if (!activo)
                {
                    nodoTreeView = CrearNodeTreeView(xmlnode, activo);
                    //string title = xmlnode.Attributes["id"].Value;
                    //treenode = treeViewMenu.Nodes.Add(title); // add it to the tree
                    nodoTreeView.ForeColor = Color.Gray;
                    i = this.treeViewNoActiva.Nodes.Add(nodoTreeView); // add it to the tree
                    treenode = this.treeViewNoActiva.Nodes[i];

                    PopulateChildNodes(xmlnode, treenode); // Get the children
                }
                else
                {
                    //Chequear si existen nodos inactivos que cuelguen de ese nodo
                    bool result = false;
                    if (xmlnode.HasChildNodes && HijosInactivos(xmlnode, ref result))
                    {
                        nodoTreeView = CrearNodeTreeView(xmlnode, activo);
                        nodoTreeView.ForeColor = Color.Black;
                        i = this.treeViewNoActiva.Nodes.Add(nodoTreeView); // add it to the tree
                        treenode = this.treeViewNoActiva.Nodes[i];

                        PopulateChildNodes(xmlnode, treenode); // Get the children

                    }
                }
            }

            this.treeViewNoActiva.EndUpdate(); // Stop updating the tree
            this.treeViewNoActiva.Refresh(); // refresh the treeview display
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
            int i;
            bool activo = true;

            foreach (XmlNode xmlnode in childNodeList)
            // loop through all children
            {
                if (xmlnode.Attributes["activo"] != null)
                {
                    activo = xmlnode.Attributes["activo"].Value == "1" ? true : false;
                }
                
                if (!activo)
                {
                    nodoTreeView = CrearNodeTreeView(xmlnode, activo);
                    //string title = xmlnode.Attributes["id"].Value;
                    // add it to the parent node tree
                    //treenode = oldTreenode.Nodes.Add(title);
                    nodoTreeView.ForeColor = Color.Gray;
                    i = oldTreenode.Nodes.Add(nodoTreeView);
                    treenode = oldTreenode.Nodes[i];

                    PopulateChildNodes(xmlnode, treenode);
                }
                else
                {
                    //Chequear si existen nodos inactivos que cuelguen de ese nodo
                    bool result = false;
                    if (xmlnode.HasChildNodes && HijosInactivos(xmlnode, ref result))
                    {
                        nodoTreeView = CrearNodeTreeView(xmlnode, activo);
                        nodoTreeView.ForeColor = Color.Black;
                        
                        i = oldTreenode.Nodes.Add(nodoTreeView);
                        treenode = oldTreenode.Nodes[i];

                        PopulateChildNodes(xmlnode, treenode);

                    }
                }
            }
        }

        /// <summary>
        /// Crea un nodo para el TreeView
        /// </summary>
        /// <param name="nodoXML">Nodo XML</param>
        /// <param name="activo">Si el nodo está activo</param>
        /// <returns></returns>
        private MenuItem CrearNodeTreeView(XmlNode nodoXML, bool activo)
        {
            MenuItem menuItem = new MenuItem();

            try
            {
                if (nodoXML.Attributes["id"] != null) menuItem.Id = nodoXML.Attributes["id"].Value;
                menuItem.Activo = activo;
                
                if (nodoXML.Attributes["orden"] != null) menuItem.Orden = Convert.ToInt16(nodoXML.Attributes["orden"].Value);
                if (nodoXML.Attributes["icono"] != null) menuItem.Icono = nodoXML.Attributes["icono"].Value;
                if (nodoXML.Attributes["custom"] != null)
                {
                    menuItem.Custom = nodoXML.Attributes["custom"].Value == "1" ? true : false;
                }
                if (nodoXML.Attributes["programa"] != null) menuItem.Programa = nodoXML.Attributes["programa"].Value;
                if (nodoXML.Attributes["proceso"] != null) menuItem.Proceso = nodoXML.Attributes["proceso"].Value;
                if (nodoXML.Attributes["textoDefecto"] != null) menuItem.TextoDefecto = nodoXML.Attributes["textoDefecto"].Value;

                //Texto que aparecerá en el TreeView
                if (menuItem.TextoDefecto != "") menuItem.Text = menuItem.TextoDefecto;
                else menuItem.Text = menuItem.Id;

                //Adicionar las opciones del menú con click derecho
                if (!activo) this.NodoAddMenuClickDerecho(ref menuItem);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (menuItem);
        }

        /// <summary>
        /// Verifican si existen opciones activas o no
        /// </summary>
        private void ChequearExistenOpcionesNoActivas()
        {
            if (this.treeViewNoActiva.Nodes.Count == 0)
            {
                this.lblResult.Visible = true;
                this.grBoxNoActiva.Visible = false;
                this.btnSalir.Top = this.btnSalir.Top - 400;
                this.Height = this.Height - 400;
            }
            else
            {
                this.lblResult.Visible = false;
                this.grBoxNoActiva.Visible = true;
            }
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
            ToolStripMenuItem activarLabel = new ToolStripMenuItem();
            activarLabel.Text = this.LP.GetText("lblActivar", "Activar");
            activarLabel.Tag = "Activar";

            menuDerecho.Items.Add(activarLabel);

            //Adicionar el Handler al menuDerecho para que capture el evento Click
            menuDerecho.ItemClicked += new ToolStripItemClickedEventHandler(menuDerecho_ItemClicked);

            // Set the ContextMenuStrip property to the ContextMenuStrip.
            nodo.ContextMenuStrip = menuDerecho;
        }

        /// <summary>
        /// Busca si existen nodos con el atributo activo=0, que cuelguen a partir del nodo actual
        /// </summary>
        /// <param name="xmlNode">Nodo del fichero XML</param>
        /// <returns></returns>
        private bool HijosInactivos(XmlNode xmlNode, ref bool result)
        {
            //bool result = false;
            if (!result)
            {
                foreach (XmlNode nodo in xmlNode.ChildNodes)
                // loop through all children
                {
                    if (nodo.Attributes["activo"] != null && nodo.Attributes["activo"].Value == "0")
                    {
                        result = true;
                        //return (result);
                        break;
                    }
                    else
                    {
                        result = HijosInactivos(nodo, ref result);
                    }
                }
            }
            return (result);
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
        #endregion
    }
}
