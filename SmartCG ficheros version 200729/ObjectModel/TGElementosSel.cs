using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ObjectModel
{
    public partial class TGElementosSel : Telerik.WinControls.UI.RadForm
    {
        //handler y evento que se lanzarán cuando se ejecuta la acción Cancelar
        //viajen desde el user control hacia el formulario
        public delegate void CancelFormCommandEventHandler();
        public event CancelFormCommandEventHandler CancelForm;


        //handler y evento que se lanzarán cuando se ejecuta la acción Aceptar 
        //viajen desde el user control hacia el formulario
        public delegate void OkFormCommandEventHandler(OkFormCommandEventArgs e);
        public event OkFormCommandEventHandler OkForm;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class OkFormCommandEventArgs
        {
            public ArrayList Valor { get; protected set; }
            public OkFormCommandEventArgs(ArrayList valor)
            {
                this.Valor = valor;
            }
        }

        private string buscadorNombreColumnas = "";
        private ArrayList buscadorNombreColumnasCampos = null;

        private Utiles utiles;

        #region Properties
        private string _tituloForm;
        /// <summary>
        /// Título del formulario
        /// </summary>
        public string TituloForm
        {
            get
            {
                return (this._tituloForm);
            }
            set
            {
                this._tituloForm = value;
            }
        }

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
        
        private ProveedorDatos _proveedorDatos;
        /// <summary>
        /// Proveedor de Datos que ejecutará la consulta para buscar los datos
        /// La conexión deberá estar abierta y no se cerrará la conexión
        /// </summary>
        public ProveedorDatos ProveedorDatosForm
        {
            get
            {
                return (this._proveedorDatos);
            }
            set
            {
                this._proveedorDatos = value;
            }
        }

        private string _query;
        /// <summary>
        /// Consulta que se ejecutará para buscar los elementos
        /// </summary>
        public string Query
        {
            get
            {
                return (this._query);
            }
            set
            {
                this._query = value;
            }
        }

        private ArrayList _columnasCaption;
        /// <summary>
        /// Encabezado de las columnas
        /// </summary>
        public ArrayList ColumnasCaption
        {
            get
            {
                return (this._columnasCaption);
            }
            set
            {
                this._columnasCaption = value;
            }
        }

        private string _filtroCaracterComodin;
        /// <summary>
        /// Caracter comodín que se utiliza para los filtros (por defecto el signo de interrogación ?)
        /// </summary>
        public string FiltroCaracterComodin
        {
            get
            {
                return (this._filtroCaracterComodin);
            }
            set
            {
                this._filtroCaracterComodin = value;
            }
        }

        private string _columnasFilter;
        /// <summary>
        /// Columnas a las cuales se les aplique el filtro
        /// </summary>
        public string ColumnasFilter
        {
            get
            {
                return (this._columnasFilter);
            }
            set
            {
                this._columnasFilter = value;
            }
        }

        private string _filtro;
        /// <summary>
        /// Filtro que se ejecutará para buscar los elementos
        /// </summary>
        public string Filtro
        {
            get
            {
                return (this._filtro);
            }
            set
            {
                this._filtro = value;
            }
        }

        private string _todasEtiqueta = "Todas";
        /// <summary>
        /// Etiqueta de Todas las columnas
        /// </summary>
        public string TodasEtiqueta
        {
            get
            {
                return (this._todasEtiqueta);
            }
            set
            {
                this._todasEtiqueta = value;
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
        #endregion

        public TGElementosSel()
        {
            InitializeComponent();
            
            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            //this.radGridViewElementos.MasterView.TableSearchRow.IsVisible = false;

            this.radGridViewElementos.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
            this.radGridViewElementos.AllowSearchRow = true;
            this.radGridViewElementos.MasterView.TableSearchRow.IsVisible = false;
            this.radGridViewElementos.TableElement.SearchHighlightColor = Color.Aqua;
            this.radGridViewElementos.AllowEditRow = false;
            this.radGridViewElementos.EnableFiltering = true;
            this.radGridViewElementos.Focus();
            this.radGridViewElementos.Select();

            if (this.radGridViewElementos.Rows.Count > 0)
            {
                this.radGridViewElementos.Rows[0].IsCurrent = true;
                //this.radGridViewElementos.Focus();
                //this.radGridViewElementos.Select();

                //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, this.radPanelApp.Size.Height - this.radCollapsiblePanelDataFilter.Size.Height - 3);
                //this.radGridViewInfo.Size = new Size(this.radGridViewInfo.Size.Width, 609);
            }
        }

        #region Eventos
        private void TGElementosSel_Load(object sender, EventArgs e)
        {
            utiles = new Utiles();

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;


            //Título Form
            if (this._tituloForm != null && this._tituloForm != "") this.Text = this._tituloForm;
            else this.Text = "Seleccionar elemento";

            if (this._centrarForm)
            {
                if (this._frmPadre == null)
                {
                    //Centrar formulario respecto a la pantalla completa
                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Top = (rect.Height / 2) - (this.Height / 2);
                    this.Left = (rect.Width / 2) - (this.Width / 2);
                }
                else
                {
                    //Centrar el formulario respecto al formulario padre
                    Utiles utiles = new Utiles();

                    utiles.CentrarFormHijo(this._frmPadre, this);
                }
            }
            else
                //Coordenadas iniciales del formulario
                if (!(this._location.X == 0 && this._location.Y == 0))
                {
                    this.Location = this._location;
                }

            //Cargar el componente ListView con los elementos
            this.FillListView();

            this.Refresh();

            GlobalVar.ElementosSel = null;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            GlobalVar.ElementosSel = null;

            //Define el evento local al user control que se ejecutará después de pulsar el botón cancelar y antes de cerrar el formulario .
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            if (CancelForm != null)
            {
                CancelForm();
            }

            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Grabar el valor en la variable global ElementosSel y cerrar el formulario
            this.DevolverValor();
        }

        private void radGridViewElementos_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            //Grabar el valor en la variable global ElementosSel y cerrar el formulario
            this.DevolverValor();
        }

        private void btnAceptar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAceptar);
        }

        private void btnAceptar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAceptar);
        }

        private void btnSalir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnSalir);
        }

        private void btnSalir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnSalir);
        }

        private void TGElementosSel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.btnSalir_Click(sender, null);
        }

        private void radGridViewElementos_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
                //185; 219; 245
                //e.CellElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(219)))), ((int)(((byte)(245)))));
            }
        }
        #endregion

        #region Métodos Privados
        private void FillListView()
        {
            if (GlobalVar.UsuarioLogadoCG == "")
            {
                MessageBox.Show("Error, debe estar logado");
                return;
            }

            try
            {
                //Ejecutar la Consulta
                string queryFiltro = "";
                if (this._filtro != "" && this._filtro != null) queryFiltro = this.QueryConFiltro();

                DataTable dtTable = this._proveedorDatos.FillDataTable(this._query, _proveedorDatos.GetConnectionValue);

                //Eliminar las columnas del DataTable que no se visualizarán en el ListView (son a las que no se han informado el caption)
                if (this._columnasCaption != null && (this._columnasCaption.Count < dtTable.Columns.Count))
                {
                    //Eliminar las columnas que no se visualizaran
                    int columnasDataTable = dtTable.Columns.Count;
                    for (int i = this._columnasCaption.Count; i < columnasDataTable; i++)
                    {
                        dtTable.Columns.Remove(dtTable.Columns[i]);
                    }
                }

                this.radGridViewElementos.DataSource = dtTable;

                //Definir los encabezados de las columnas
                /*if (this._columnasCaption == null || (this._columnasCaption.Count != dtTable.Columns.Count))
                {
                    //Si no se ha indicado los encabezados en la propiedad (ColumnasCaption) 
                    //o no coinciden con la cantidad de campos de la query
                    //se mostrara como encabezado de las columnas, los campos de la consulta
                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        this.listViewElementos.Columns.Add(dtTable.Columns[i].Caption);
                    }
                }
                else
                {*/
                //Los encabezados de las columnas son los indicados en la propiedad (ColumnasCaption)
                string nombreColumna = "";
                    string descColumna = "";
                    this.buscadorNombreColumnasCampos = new ArrayList();
                    for (int i = 0; i < this._columnasCaption.Count; i++)
                    {
                        descColumna = this._columnasCaption[i].ToString();

                        if (this.buscadorNombreColumnas != "") this.buscadorNombreColumnas += ", " + descColumna;
                        else this.buscadorNombreColumnas += descColumna;

                        try
                        {
                            nombreColumna = dtTable.Columns[i].Caption;
                        }
                        catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    if (this.radGridViewElementos.Columns.Contains(nombreColumna))
                    {
                        this.radGridViewElementos.Columns[nombreColumna].HeaderText = descColumna;
                        this.radGridViewElementos.Columns[nombreColumna].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    }

                    string[] columnaCampo = new string[2];
                        columnaCampo[0] = descColumna;
                        columnaCampo[1] = nombreColumna;

                        this.buscadorNombreColumnasCampos.Add(columnaCampo);
                    }
                //}


                /*
                //Añadir los registros de la tabla
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    ListViewItem elemento = new ListViewItem(dtTable.Rows[i].ItemArray[0].ToString());

                    for (int j = 1; j < dtTable.Columns.Count; j++)
                    {
                        elemento.SubItems.Add(dtTable.Rows[i].ItemArray[j].ToString());
                    }

                    this.listViewElementos.Items.Add(elemento);
                }
                */

                //SMR añadir registros de la tabla
                var valores = new List<ListViewItem>();
                string[] strValores = new string[dtTable.Columns.Count];

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTable.Columns.Count; j++)
                    {
                        strValores[j] = dtTable.Rows[i].ItemArray[j].ToString();
                    }
                    valores.Add(new ListViewItem(strValores));
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            this.radGridViewElementos.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewElementos.MasterTemplate.BestFitColumns();
            if (this.radGridViewElementos.Rows.Count > 0) this.radGridViewElementos.Rows[0].IsCurrent = true;
        }

        private string QueryConFiltro()
        {
            string result = "";

            try
            {
                int posCaracterComodin = this._filtro.IndexOf(this._filtroCaracterComodin);
            }
            catch (Exception ex){ GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        
        /// <summary>
        /// El valor seleccionado se almacena en la variable global ElementosSel
        /// y se cierra el formulario
        /// </summary>
        private void DevolverValor()
        {
            if (this.radGridViewElementos.SelectedRows.Count == 1)
            {
                ArrayList elementosSel = new System.Collections.ArrayList();

                for (int i = 0; i < this.radGridViewElementos.SelectedRows[0].Cells.Count; i++)
                {
                    elementosSel.Add(this.radGridViewElementos.SelectedRows[0].Cells[i].Value.ToString());
                }

                GlobalVar.ElementosSel = elementosSel;

                //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar o doble clik antes de cerrar el formulario .
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                if (OkForm != null)
                {
                    OkForm(new OkFormCommandEventArgs(elementosSel));
                }
            }

            /*
            if (this.listViewElementos.SelectedItems.Count > 0)
            {
                ArrayList elementosSel = new System.Collections.ArrayList();

                for (int i = 0; i < this.listViewElementos.SelectedItems[0].SubItems.Count; i++)
                {
                    elementosSel.Add(this.listViewElementos.SelectedItems[0].SubItems[i].ToString());
                }

                GlobalVar.ElementosSel = elementosSel;

                //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar o doble clik antes de cerrar el formulario .
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                if (OkForm != null)
                {
                    OkForm(new OkFormCommandEventArgs(elementosSel));
                }
            }
            */
            this.Close();
        }
        #endregion
    }
}
