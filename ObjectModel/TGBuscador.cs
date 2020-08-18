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
    public partial class TGBuscador : UserControl
    {
        //handler y evento que se lanzarán cuando se ejecuta la acción Buscar 
        //viajen desde el user control hacia el formulario
        public delegate void BuscarFormResultCommandEventHandler(BuscarFormResultCommandEventArgs e);
        public event BuscarFormResultCommandEventHandler BuscarFormResult;

        /// <summary>
        /// Definir el argumento del evento que será usado para realizar el pasaje de los datos seleccionados en el ListView. 
        /// Es por medio de este argumento que se podrá recuperar los valores elegidos, ya que desde fuera del user control 
        /// no se tendra acceso directo al ListView.
        /// </summary>
        public class BuscarFormResultCommandEventArgs
        {
            public DataTable Valor { get; protected set; }
            public BuscarFormResultCommandEventArgs(DataTable valor)
            {
                this.Valor = valor;
            }
        }

        private string _queryEjecutar;

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

        private string _tituloGrupo;
        /// <summary>
        /// Titulo que aparecerá en la cabecera del Grupo
        /// </summary>
        public string TituloGrupo
        {
            get
            {
                return (this._tituloGrupo);
            }
            set
            {
                this._tituloGrupo = value;
            }
        }

        private string _query;
        /// <summary>
        /// Sentencia SQL por defecto
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

        private DataTable _datos;
        /// <summary>
        /// DataTable que contiene el resultado de la búsqueda
        /// </summary>
        public DataTable Datos
        {
            get
            {
                return (this._datos);
            }
            set
            {
                this._datos = value;
            }
        }

        /// <summary>
        /// Control del TextBox del filtro
        /// </summary>
        public Telerik.WinControls.UI.RadTextBox ValorFiltro
        {
            get
            {
                return (this.txtFiltro);
            }
            set
            {
                this.txtFiltro = value;
            }
        }

        private string _camposBusqueda = "";
        /// <summary>
        /// Campos sobre los que se hará la búsqueda
        /// (si no se informan se cogerán automáticamente de la select. De forma automática funciona correctamente para 
        /// sentencias de tipo SELECT FROM WHERE ORDER BY. Para el resto es necesario informar cuáles son los campos de búsqueda)
        /// </summary>
        public string CamposBusqueda
        {
            get
            {
                return (this._camposBusqueda);
            }
            set
            {
                this._camposBusqueda = value;
            }
        }

        private bool _todasColumnas = true;
        /// <summary>
        /// Si la búsqueda ser realiza en todas las columnas
        /// </summary>
        public bool TodasColumnas
        {
            get
            {
                return (this._todasColumnas);
            }
            set
            {
                this._todasColumnas = value;
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

        private string _nombreColumnas = "";
        /// <summary>
        /// Nombre de las columnas sobre las posibles a realizar la búsqueda
        /// </summary>
        public string NombreColumnas
        {
            get
            {
                return (this._nombreColumnas);
            }
            set
            {
                this._nombreColumnas = value;
            }
        }

        private ArrayList _nombreColumnasCampos = null;
        /// <summary>
        /// Nombre de todas las columnas de la Grid con su correspondencia con el campo de la tabla
        /// </summary>
        public ArrayList NombreColumnasCampos
        {
            get
            {
                return (this._nombreColumnasCampos);
            }
            set
            {
                this._nombreColumnasCampos = value;
            }
        }

        private string _nombreColumnasSel = "";
        /// <summary>
        /// Nombre de las columnas sobre las que se realizará la búsqueda
        /// </summary>
        public string NombreColumnasSel
        {
            get
            {
                return (this._nombreColumnasSel);
            }
            set
            {
                this._nombreColumnasSel = value;
            }
        }

        private System.Windows.Forms.Form _frmPadre = null;
        /// <summary>
        /// Formulario Padre (formulario desde donde se invoca al buscador)
        /// </summary>
        public System.Windows.Forms.Form FrmPadre
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
        
        public TGBuscador()
        {
            InitializeComponent();

            this._datos = new DataTable();
            this._tituloGrupo = "Buscador";     //Falta traducir
        }

        #region Eventos
        private void TGBuscador_Load(object sender, EventArgs e)
        {
            this.btnBuscar.Enabled = false;
            this.gbModo.Enabled = false;

            if (this._tituloGrupo != "") this.gbBuscador.Text = this._tituloGrupo;

            this.lblColumnasNombres.Text = this._todasEtiqueta;

            this.txtFiltro.Select();
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            this.txtFiltro.Text = "";
            this._queryEjecutar = this._query;

            try
            {
                this._datos = this._proveedorDatos.FillDataTable(this._queryEjecutar, _proveedorDatos.GetConnectionValue);

                //Define el evento local al user control que se ejecutará después de pulsar el botón buscar
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                BuscarFormResult(new BuscarFormResultCommandEventArgs(this._datos));
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtener la consulta filtrada
                this.ObtenerConsulta();

                this._datos = this._proveedorDatos.FillDataTable(this._queryEjecutar, _proveedorDatos.GetConnectionValue);

                //Define el evento local al user control que se ejecutará después de pulsar el botón buscar
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                BuscarFormResult(new BuscarFormResultCommandEventArgs(this._datos));
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void txtFiltro_Leave(object sender, EventArgs e)
        {
            if (this.txtFiltro.Text.Trim() != "")
            {
                this.btnBuscar.Enabled = true;
                this.gbModo.Enabled = true;
            }
            else
            {
                this.btnBuscar.Enabled = false;
                this.gbModo.Enabled = false;
            }
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);

            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete) //Retroceso y delete
            {
                return;
            }

            if (!this.btnBuscar.Enabled)
            {
                this.btnBuscar.Enabled = true;
                this.gbModo.Enabled = true;
                this.lnkColumns.Enabled = true;
            }
        }

        private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.txtFiltro.Text.Trim() != "")
            {
                this.btnBuscar.Enabled = true;
                this.gbModo.Enabled = true;
                this.lnkColumns.Enabled = true;
                this.btnBuscar.PerformClick();
            }
            else 
                if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && this.txtFiltro.Text.Length == 1)
                {
                    this.btnBuscar.Enabled = false;
                    this.gbModo.Enabled = false;
                    this.lnkColumns.Enabled = false;
                }
        }

        private void lnkColumns_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TGBuscadorSelColumnas formSelCol = Application.OpenForms.OfType<TGBuscadorSelColumnas>().FirstOrDefault();

            bool todas = false;
            if (this.lblColumnasNombres.Text == this._todasEtiqueta) todas = true;

            if (formSelCol == null)
            {
                formSelCol = new TGBuscadorSelColumnas();
                formSelCol.ColumnasSelFormResult += new TGBuscadorSelColumnas.ColumnasSelFormResultCommandEventHandler(formSelCol_ColumnasSelFormResult);
                formSelCol.Todas = todas;
                formSelCol.NombreColumnas = this._nombreColumnas;
                formSelCol.NombreColumnasSel = this.lblColumnasNombres.Text;
                formSelCol.FrmPadre = this._frmPadre;
                //formSelCol.Show();
                formSelCol.ShowDialog();
            }
            /*else
            {
                formSelCol.Todas = todas;
                formSelCol.NombreColumnas = this._nombreColumnas;
                formSelCol.NombreColumnasSel = this.lblColumnasNombres.Text;
                formSelCol.BringToFront();
            }*/

        }

        private void formSelCol_ColumnasSelFormResult(TGBuscadorSelColumnas.ColumnasSelFormResultCommandEventArgs e)
        {
            this.lblColumnasNombres.Text = e.Valor;
            this._nombreColumnasSel = e.Valor;
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Obtiene los campos que han de ser filtrados
        /// Son todos los que aparecen en la consulta (despues de la select y antes del from)
        /// Devuelve los campos separados por coma
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string ObtenerCampos(string query)
        {
            string campos = "";
            try
            {
                int posFrom = query.IndexOf("FROM");
                if (posFrom > 0)
                {
                    campos = query.Substring(0, posFrom);
                    campos = campos.Replace("SELECT", "");

                    int posMin = campos.IndexOf("MIN(");
                    int posMinParentesis = 0;
                    int posMax = campos.IndexOf("MAX(");
                    int posMaxParentesis = 0;

                    while (posMin > 0 || posMax > 0)
                    {
                        if (posMin > 0)
                        {
                            posMinParentesis = campos.IndexOf(")", posMin);
                            if (posMinParentesis > 0)
                            {
                                campos = campos.Substring(0, posMin) + campos.Substring(posMinParentesis + 1, campos.Length - posMinParentesis - 1);
                            }
                        }

                        posMax = campos.IndexOf("MAX(");
                        if (posMax > 0)
                        {
                            posMaxParentesis = campos.IndexOf(")", posMax);
                            if (posMaxParentesis > 0)
                            {
                                campos = campos.Substring(0, posMax) + campos.Substring(posMaxParentesis + 1, campos.Length - posMaxParentesis - 1);
                            }
                        }

                        posMin = campos.IndexOf("MIN(");
                        posMax = campos.IndexOf("MAX(");
                    }

                    campos = campos.Trim();
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (campos);
        }

        /// <summary>
        /// A la consulta inicial le agrega el filtro
        /// </summary>
        private void ObtenerConsulta()
        {
            try
            {
                string queryAux = this._query.ToUpper();

                string filtro = this.txtFiltro.Text.Trim();

                string campos = this._camposBusqueda;
                if (campos == "") campos = this.ObtenerCampos(queryAux);

                if (campos != "" && this._nombreColumnasCampos != null && (this._nombreColumnasSel != "" || this._nombreColumnasSel != this._todasEtiqueta))
                {
                   campos = this.ObtenerCamposFiltroSelCol(campos);
                }

                if (filtro != "" && this._query != "" && campos != "")
                {
                    string query = this._query;

                    string[] camposArr = campos.Split(',');
                    string filtroQuery = "";
                    string filtroMayusculas = "";
                    for (int i = 0; i < camposArr.Length; i++)
                    {
                        if (i > 0) filtroQuery += " or ";
                        //----- Filtro tal cual se ha introducido -----
                        filtroQuery += camposArr[i].Trim() + " LIKE '";
                        if (this.rbModoContiene.Checked) filtroQuery += "%";
                        filtroQuery += filtro + "%'";
                        //----- Filtro llevado a mayusculas -----
                        filtroMayusculas = filtro.ToUpper();
                        if (filtro != filtroMayusculas)
                        {
                            filtroQuery += " or " + camposArr[i].Trim() + " LIKE '";
                            if (this.rbModoContiene.Checked) filtroQuery += "%";
                            filtroQuery += filtroMayusculas + "%'";
                        }
                    }

                    if (filtroQuery != "")
                    {
                        filtroQuery = "(" + filtroQuery + ") ";
                        int posWhere;
                        int posGroupBy = queryAux.IndexOf("GROUP BY");

                        if (posGroupBy != -1)
                        {
                            //Filtro + Order By
                            filtroQuery += query.Substring(posGroupBy, query.Length - posGroupBy);
                            posWhere = queryAux.IndexOf("WHERE");
                            if (posWhere != -1)
                            {
                                query = query.Substring(0, posGroupBy) + " and " + filtroQuery;
                            }
                            else
                            {
                                query = query.Substring(0, posGroupBy) + " where " + filtroQuery;
                            }
                        }
                        else
                        {
                            int posOrderBy = queryAux.IndexOf("ORDER BY");
                            if (posOrderBy != -1)
                            {
                                //Filtro + Order By
                                filtroQuery += query.Substring(posOrderBy, query.Length - posOrderBy);
                                posWhere = queryAux.IndexOf("WHERE");
                                if (posWhere != -1)
                                {
                                    query = query.Substring(0, posOrderBy) + " and " + filtroQuery;
                                }
                                else
                                {
                                    query = query.Substring(0, posOrderBy) + " where " + filtroQuery;
                                }
                            }
                            else
                            {
                                posWhere = queryAux.IndexOf("WHERE");
                                if (posWhere != -1)
                                {
                                    query = query.Substring(0, query.Length) + " and " + filtroQuery;
                                }
                                else
                                {
                                    query = query.Substring(0, query.Length) + " where " + filtroQuery;
                                }
                            }

                        }
                        
                        this._queryEjecutar = query;
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Deja los campos de búsqueda según las columnas seleccionadas
        /// </summary>
        /// <param name="campos"></param>
        /// <returns></returns>
        private string ObtenerCamposFiltroSelCol(string campos)
        {
            string result = "";

            try
            {
                ArrayList camposColumnasSel = new ArrayList();
                string[] aColumnasSel = this._nombreColumnasSel.Split('|');

                string nombreColumna = "";
                string[] nombreColumnaCampo;
                for (int i = 0; i < aColumnasSel.Length; i++)
                {
                    nombreColumna = aColumnasSel[i].Trim();

                    for (int j = 0; j < this._nombreColumnasCampos.Count; j++)
                    {
                        nombreColumnaCampo = (string[])this._nombreColumnasCampos[j];

                        if (nombreColumnaCampo[0].ToString() == nombreColumna)
                        {
                            camposColumnasSel.Add(nombreColumnaCampo[1]);
                            break;
                        }
                    }
                }

                if (camposColumnasSel.Count > 0)
                {
                    result = string.Join(",", camposColumnasSel.ToArray().Select(o => o.ToString()).ToArray());
                }
                else result = campos;

            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        #endregion

    }
}
