using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ObjectModel
{
    public partial class TGTexBoxSel : UserControl
    {
        //handler y evento que permitirán que la información seleccionada en el ListView 
        //viajen desde el user control hacia el formulario
        public delegate void ValueChangedCommandEventHandler(ValueChangedCommandEventArgs e);
        public event ValueChangedCommandEventHandler ValueChanged;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class ValueChangedCommandEventArgs
        {
            public ArrayList Valor { get; protected set; }
            public ValueChangedCommandEventArgs(ArrayList valor)
            {
                this.Valor = valor;
            }
        }

        TGElementosSel frmElementosSel;
        private char caracterAjusteDefecto = 'W';

        #region Propiedades Para el Botón de Selección
        private string _tituloFormSel;
        /// <summary>
        /// Título del formulario de selección de elementos
        /// </summary>
        public string TituloFormSel
        {
            get
            {
                return (this._tituloFormSel);
            }
            set
            {
                this._tituloFormSel = value;
            }
        }

        private Point _locationFormSel = new Point(0, 0);
        /// <summary>
        /// Coordenadas donde se dibujará el formulario
        /// </summary>
        public Point LocationFormSel
        {
            get
            {
                return (this._locationFormSel);
            }
            set
            {
                this._locationFormSel = value;
            }
        }

        private bool _centrarFormSel = true;
        /// <summary>
        /// Centrar el formulario
        /// </summary>
        public bool CentrarFormSel
        {
            get
            {
                return (this._centrarFormSel);
            }
            set
            {
                this._centrarFormSel = value;
            }
        }

        private ProveedorDatos _proveedorDatosFormSel;
        /// <summary>
        /// Proveedor de Datos que ejecutará la consulta para buscar los datos
        /// La conexión deberá estar abierta y no se cerrará la conexión
        /// </summary>
        public ProveedorDatos ProveedorDatosFormSel
        {
            get
            {
                return (this._proveedorDatosFormSel);
            }
            set
            {
                this._proveedorDatosFormSel = value;
            }
        }

        private string _queryFormSel;
        /// <summary>
        /// Consulta que se ejecutará para buscar los elementos
        /// </summary>
        public string QueryFormSel
        {
            get
            {
                return (this._queryFormSel);
            }
            set
            {
                this._queryFormSel = value;
            }
        }

        private ArrayList _columnasCaptionFormSel;
        /// <summary>
        /// Encabezado de las columnas
        /// </summary>
        public ArrayList ColumnasCaptionFormSel
        {
            get
            {
                return (this._columnasCaptionFormSel);
            }
            set
            {
                this._columnasCaptionFormSel = value;
            }
        }

        private int _cantidadColumnasResult;
        /// <summary>
        /// Cantidad de columnas que serán mostradas en el TextBox
        /// </summary>
        public int CantidadColumnasResult
        {
            get
            {
                return (this._cantidadColumnasResult);
            }
            set
            {
                this._cantidadColumnasResult = value;
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

        #region Propiedades Para el TextBox
        public TextBox Textbox
        {
            get
            {
                return (this.txtElemento);
            }
            set
            {
                this.txtElemento = value;
            }
        }

        private int _numeroCaracteresView;
        /// <summary>
        /// Caracteres que se visualizan en el textbox
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public int NumeroCaracteresView
        {
            get
            {
                return (this._numeroCaracteresView);
            }
            set
            {
                this._numeroCaracteresView = value;
            }
        }

        private bool _todoMayuscula = false;
        /// <summary>
        /// Todos los caracteres del textbox en mayúsculas
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public bool TodoMayuscula
        {
            get
            {
                return (this._todoMayuscula);
            }
            set
            {
                this._todoMayuscula = value;
            }
        }

        private string _separadorCampos = "-";
        /// <summary>
        /// Caracter que se utilizará para separar los campos en el textbox
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public string SeparadorCampos
        {
            get
            {
                return (this._separadorCampos);
            }
            set
            {
                this._separadorCampos = value;
            }
        }

        public Button Boton
        {
            get
            {
                return (this.btnSel);
            }
            set
            {
                this.btnSel = value;
            }
        }
        #endregion

        #region Propiedades Para el Button
        public Button ButtonSel
        {
            get
            {
                return (this.btnSel);
            }
            set
            {
                this.btnSel = value;
            }
        }
        #endregion

        public TGTexBoxSel()
        {
            InitializeComponent();
        }

        #region Eventos
        private void btnSel_Click(object sender, EventArgs e)
        {
            //Crea el formulario de selección
            this.frmElementosSel = new TGElementosSel();

            //Título del Formulario de Selección de Elementos
            this.frmElementosSel.TituloForm = this._tituloFormSel;
            //Coordenadas donde se dibujará el Formulario de Selección de Elementos
            //this.frmElementosSel.LocationForm = new Point(this.btnSel.Location.X + 100, this.btnSel.Location.Y);
            this.frmElementosSel.LocationForm = this._locationFormSel;
            //Si se centrar el Formulario o no
            this.frmElementosSel.CentrarForm = this._centrarFormSel;
            //Pasar la conexión a la bbdd
            this.frmElementosSel.ProveedorDatosForm = this._proveedorDatosFormSel;
            //Consulta que se ejecutará para obtener los Elementos
            this.frmElementosSel.Query = this._queryFormSel;
            //Definir la cabecera de las columnas
            this.frmElementosSel.ColumnasCaption = this._columnasCaptionFormSel;
            //Definir Formulario Padre (formulario desde donde se invoca al buscador) 
            this.frmElementosSel.FrmPadre = this._frmPadre;

            this.frmElementosSel.ShowDialog();

            string result = "";
            if (GlobalVar.ElementosSel != null && GlobalVar.ElementosSel.Count > 0)
            {
                //Procesar el resultado y visualizarlo en el TextBox
                for (int i = 0; i < GlobalVar.ElementosSel.Count; i++)
                {
                    if (i + 1 > this._cantidadColumnasResult) break;

                    result += GlobalVar.ElementosSel[i].ToString().Trim();

                    if (this._cantidadColumnasResult <= 1)
                    {
                        break;
                    }
                    else
                    {
                        if (this._cantidadColumnasResult > i + 1 && this._cantidadColumnasResult <= GlobalVar.ElementosSel.Count)
                            result += " " + this._separadorCampos + " ";
                    }
                }
                this.txtElemento.Text = result;
                this.ActiveControl = this.txtElemento;
                this.txtElemento.Select(0, 0);
                this.txtElemento.Focus();

                //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar y cerrar el formulario .
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                if (ValueChanged != null)
                    ValueChanged(new ValueChangedCommandEventArgs(GlobalVar.ElementosSel));

            }
        }

        private void txtElemento_Enter(object sender, EventArgs e)
        {
            this.txtElemento.Modified = false;
        }

        private void txtElemento_TextChanged(object sender, EventArgs e)
        {
            //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar y cerrar el formulario .
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            if (ValueChanged != null)
            {
                this.txtElemento.Modified = true;
                ArrayList temp = new ArrayList();
                temp.Add(this.txtElemento.Text);
                ValueChanged(new ValueChangedCommandEventArgs(temp));
            }
        }

        private void txtElemento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this._todoMayuscula)
                e.KeyChar = Char.ToUpper(e.KeyChar);
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Por defecto se ajusta el TextBox al tamaño del caracter W
        /// </summary>
        public void AjustarTamanoTextBox()
        {
            //W - caracter por defecto
            this.AjustarTamanoTextBox(caracterAjusteDefecto); 
        }

        /// <summary>
        /// Ajustar el TextBox al tamaño del caracter indicado 
        /// (tantos caracteres como indique el atributo _numeroCaracteresView)
        /// </summary>
        /// <param name="caracter"></param>
        public void AjustarTamanoTextBox(char caracter)
        {
            if (this._numeroCaracteresView > 0)
            {
                string cadena = "";
                cadena = cadena.PadRight(this._numeroCaracteresView, caracter);

                Size size = TextRenderer.MeasureText(cadena, this.txtElemento.Font);

                int width = size.Width;
                this.txtElemento.Size = new Size(width, this.txtElemento.Size.Height);
                this.btnSel.Location = new Point(width + 5, this.btnSel.Location.Y);

                this.Size = new Size(width + 5 + this.btnSel.Size.Width + 5, this.Size.Height);
            }
        }
        #endregion
    }
}
