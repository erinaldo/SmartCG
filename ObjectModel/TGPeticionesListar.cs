using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ObjectModel
{
    public partial class TGPeticionesListar : Telerik.WinControls.UI.RadForm
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
            public DataTable Valor { get; protected set; }
            public OkFormCommandEventArgs(DataTable valor)
            {
                this.Valor = valor;
            }
        }

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

        private DataTable _dtPeticiones = null;
        /// <summary>
        /// Tabla con el listado de las peticiones
        /// </summary>
        public DataTable DtPeticiones
        {
            get
            {
                return (this._dtPeticiones);
            }
            set
            {
                this._dtPeticiones = value;
            }
        }

        private Dictionary<string, string> headers;
        /// <summary>
        /// Diccionario con el nombre de las columnas dado el campo del formulario
        /// </summary>
        public Dictionary<string, string> Headers
        {
            get
            {
                return (this.headers);
            }
            set
            {
                this.headers = value;
            }
        }

        private List<string> _columnNoVisible = null;
        /// <summary>
        /// Nombre de columnas no visibles
        /// </summary>
        public List<string> ColumnNoVisible
        {
            get
            {
                return (this._columnNoVisible);
            }
            set
            {
                this._columnNoVisible = value;
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

        public TGPeticionesListar()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.radGridViewPeticiones.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        private void TGListarPeticiones_Load(object sender, EventArgs e)
        {
            utiles = new Utiles();

            //Título Form
            if (this._tituloForm != null && this._tituloForm != "") this.Text = this._tituloForm;
            else this.Text = "Lista de peticiones";

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Construir la Grid partiendo del DataTable
            this.DataGridFill();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DevolverValor();
        }
        
        private void TGPeticionesListar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) btnSalir_Click(sender, null);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //Define el evento local al user control que se ejecutará después de pulsar el botón cancelar y antes de cerrar el formulario .
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            if (CancelForm != null)
            {
                CancelForm();
            }

            this.Close();
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Carga las peticiones en la Grid
        /// </summary>
        private void DataGridFill()
        {
            try
            {
                this.radGridViewPeticiones.DataSource = this.DtPeticiones;
                //Poner los headers a las columnas
                string nombreColumna = "";
                string headerColumna = "";

                bool ocultarColumnas = (this._columnNoVisible != null && this._columnNoVisible.Count > 0);

                for (int i = 0; i < this.radGridViewPeticiones.ColumnCount; i++)
                {
                    nombreColumna = this.radGridViewPeticiones.Columns[i].Name;
                    headerColumna = this.FindFirstKeyByValue(nombreColumna);

                    this.radGridViewPeticiones.Columns[i].HeaderText = headerColumna;

                    if (ocultarColumnas && this._columnNoVisible.Contains(nombreColumna)) this.radGridViewPeticiones.Columns[i].IsVisible = false;
                    else this.radGridViewPeticiones.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                }

                if (this.radGridViewPeticiones.RowCount > 0)
                {
                    this.radGridViewPeticiones.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    //this.radGridViewPeticiones.MasterTemplate.BestFitColumns();
                    this.radGridViewPeticiones.Rows[0].IsCurrent = true;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Devuelve la etiqueta para el control que será la cabecera de la Grid de la lista de peticiones
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FindFirstKeyByValue(string value)
        {
            try
            {
                if (this.headers.ContainsValue(value))
                {
                    foreach (string key in headers.Keys)
                    {
                        if (headers[key].Equals(value))
                            return key;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return value;
        }

        /// <summary>
        /// El valor seleccionado se almacena en la variable global ElementosSel
        /// y se cierra el formulario
        /// </summary>
        private void DevolverValor()
        {
            if (this.radGridViewPeticiones.SelectedRows.Count == 1)
            {
                //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar o doble clik antes de cerrar el formulario .
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                if (OkForm != null)
                {
                    DataTable dtAux = DtPeticiones.Clone();
                    int indice = this.radGridViewPeticiones.Rows.IndexOf(this.radGridViewPeticiones.CurrentRow);

                    dtAux.ImportRow(this.DtPeticiones.Rows[indice]);
                    
                    OkForm(new OkFormCommandEventArgs(dtAux));
                }

                this.Close();
            }
        }
        #endregion

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

        private void radGridViewPeticiones_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.DevolverValor();
        }
    }
}
