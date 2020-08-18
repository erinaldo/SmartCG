using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmZonaJerarqSel : frmPlantilla
    {
        private ListViewColumnSorter lvwColumnSorter;

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

        private string _claseZona;
        /// <summary>
        /// Clase de la zona
        /// </summary>
        public string ClaseZona
        {
            get
            {
                return (this._claseZona);
            }
            set
            {
                this._claseZona = value;
            }
        }

        private string _zonaSel;
        /// <summary>
        /// Zona
        /// </summary>
        public string ZonaSel
        {
            get
            {
                return (this._zonaSel);
            }
            set
            {
                this._zonaSel = value;
            }
        }

        ArrayList _nombreColumnas;

        public frmZonaJerarqSel()
        {
            InitializeComponent();

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewElementos.ListViewItemSorter = lvwColumnSorter;
        }

        #region Eventos
        private void FrmZonaJerarqSel_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de Zonas Jerárquicas");

            //Actualizar Clase Zona
            this.ClaseZonaDatos();

            //Cargar el componente ListView con los elementos
            this.FillListView();

            this.ResizeColumnHeaders();
            this.Refresh();

            this._zonaSel = "";
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            //Grabar el valor en la variable global ElementosSel y cerrar el formulario
            this.DevolverValor();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            //GlobalVar.ElementosSel = null;

            //Define el evento local al user control que se ejecutará después de pulsar el botón cancelar y antes de cerrar el formulario .
            //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
            if (CancelForm != null)
            {
                CancelForm();
            }

            this.Close();
        }

        private void ListViewElementos_DoubleClick(object sender, EventArgs e)
        {
            //Grabar el valor en la variable global ElementosSel y cerrar el formulario
            this.DevolverValor();
        }

        private void FrmZonaJerarqSel_ResizeEnd(object sender, EventArgs e)
        {
            this.ResizeColumnHeaders();
        }

        private void ListViewElementos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listViewElementos.Sort();
        }

        private void TgBuscadorZona_BuscarFormResult(TGBuscador.BuscarFormResultCommandEventArgs e)
        {
            try
            {
                if (this.tgBuscadorZona.Datos != null)
                {
                    this.listViewElementos.Items.Clear();

                    //Añadir los registros de la tabla
                    for (int i = 0; i < this.tgBuscadorZona.Datos.Rows.Count; i++)
                    {
                        ListViewItem elemento = new ListViewItem(this.tgBuscadorZona.Datos.Rows[i].ItemArray[0].ToString());

                        for (int j = 1; j < this.tgBuscadorZona.Datos.Columns.Count; j++)
                        {
                            elemento.SubItems.Add(this.tgBuscadorZona.Datos.Rows[i].ItemArray[j].ToString());
                        }

                        this.listViewElementos.Items.Add(elemento);
                    }

                    //Seleccionar el primer elemento del ListView
                    if (this.listViewElementos.Items.Count > 0)
                    {
                        this.listViewElementos.Items[0].Selected = true;
                    }

                    this.ResizeColumnHeaders();
                    this.Refresh();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FrmZonaJerarqSel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de Zonas Jerárquicas");
        }
        #endregion

        #region Métodos Privados
        private void ClaseZonaDatos()
        {
            IDataReader dr = null;
            try
            {
                this.lblClaseZonaDesc.Text = this._claseZona;
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLM10 ";
                query += "where CLASZ0 = '" + this._claseZona + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                string desc = "";
                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("NOMBZ0")).ToString().Trim();
                }

                dr.Close();

                if (desc != "") this.lblClaseZonaDesc.Text += " - " + desc;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        private void FillListView()
        {
            if (GlobalVar.UsuarioLogadoCG == "")
            {
                RadMessageBox.Show("Error, debe estar logado");
                return;
            }

            try
            {
                this.listViewElementos.Items.Clear();
                this.listViewElementos.Columns.Clear();

                // Set the view to show details.
                this.listViewElementos.View = View.Details;
                // Not Allow the user to rearrange columns.
                this.listViewElementos.AllowColumnReorder = false;
                // Select the item and subitems when selection is made.
                this.listViewElementos.FullRowSelect = true;
                // Display grid lines.
                this.listViewElementos.GridLines = true;
                // Sort the items in the list in ascending order.
                this.listViewElementos.Sorting = SortOrder.Ascending;

                this.listViewElementos.HideSelection = true;

                //Ejecutar la Consulta
                string query = "select STATZ1, ZONAZ1, NOMBZ1 from ";
                query += GlobalVar.PrefijoTablaCG + "GLM11 ";
                query += " where CLASZ1 = '" + this._claseZona + "' and TIPOZ1 = 'D'";
                query += "order by ZONAZ1";

                string estadoLiteral = this.LP.GetText("lblListaCampoEstado", "Estado");
                string codigoLiteral = this.LP.GetText("lblListaCampoCodigo", "Código");
                string descripcionLiteral = this.LP.GetText("lblListaCampoDescripcion", "Descripción");

                //Inicializar el componente Buscador
                this.tgBuscadorZona.Query = query;
                this.tgBuscadorZona.ProveedorDatosForm = GlobalVar.ConexionCG;
                this.tgBuscadorZona.Datos = null;
                this.tgBuscadorZona.BuscarFormResult += new TGBuscador.BuscarFormResultCommandEventHandler(TgBuscadorZona_BuscarFormResult);
                this.tgBuscadorZona.FrmPadre = this;

                string nombreColumnas = "";
                ArrayList camposGridYDesc = new ArrayList();

                string[] campos1GridYDesc = new string[2];
                campos1GridYDesc[0] = estadoLiteral;
                nombreColumnas += campos1GridYDesc[0];
                campos1GridYDesc[1] = "STATZ1";
                camposGridYDesc.Add(campos1GridYDesc);

                string[] campos2GridYDesc = new string[2];
                campos2GridYDesc[0] = codigoLiteral;
                nombreColumnas += ", " + campos2GridYDesc[0];
                campos2GridYDesc[1] = "ZONAZ1";
                camposGridYDesc.Add(campos2GridYDesc);

                string[] campos3GridYDesc = new string[2];
                campos3GridYDesc[0] = descripcionLiteral;
                nombreColumnas += ", " + campos3GridYDesc[0];
                campos3GridYDesc[1] = "NOMBZ1";
                camposGridYDesc.Add(campos3GridYDesc);

                this.tgBuscadorZona.NombreColumnas = nombreColumnas;
                this.tgBuscadorZona.NombreColumnasCampos = camposGridYDesc;


                DataTable dtTable = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Columnas de los campos de tipo TGTextBoxSel
                this._nombreColumnas = new ArrayList();
                this._nombreColumnas.Add(estadoLiteral);
                this._nombreColumnas.Add(codigoLiteral);
                this._nombreColumnas.Add(descripcionLiteral);

                //Eliminar las columnas del DataTable que no se visualizarán en el ListView (son a las que no se han informado el caption)
                if (this._nombreColumnas != null && (this._nombreColumnas.Count < dtTable.Columns.Count))
                {
                    //Eliminar las columnas que no se visualizaran
                    int columnasDataTable = dtTable.Columns.Count;
                    for (int i = this._nombreColumnas.Count; i < columnasDataTable; i++)
                    {
                        dtTable.Columns.Remove(dtTable.Columns[i]);
                    }
                }

                //Los encabezados de las columnas son los indicados en la propiedad (ColumnasCaption)
                for (int i = 0; i < this._nombreColumnas.Count; i++)
                {
                    this.listViewElementos.Columns.Add(this._nombreColumnas[i].ToString());
                }
                //}

                //Añadir los registros de la tabla
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    ListViewItem elemento = new ListViewItem(dtTable.Rows[i].ItemArray[0].ToString());

                    if (elemento.Text.TrimEnd() == "V") elemento.Text = this.LP.GetText("lblEstadoActiva", "Activa");   //Falta traducir
                    else elemento.Text = this.LP.GetText("lblEstadoInactiva", "Inactiva");  //Falta traducir

                    for (int j = 1; j < dtTable.Columns.Count; j++)
                    {
                        elemento.SubItems.Add(dtTable.Rows[i].ItemArray[j].ToString());
                    }

                    this.listViewElementos.Items.Add(elemento);
                }

                //Seleccionar el primer elemento del ListView
                if (this.listViewElementos.Items.Count > 0)
                {
                    this.listViewElementos.Items[0].Selected = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Ajustar el tamaño de las columnas del ListView
        /// </summary>
        private void ResizeColumnHeaders()
        {
            //Ajustar sólo teniendo en cuenta las columnas visibles (serán las columnas con nombre indicado)
            for (int i = 0; i < this._nombreColumnas.Count - 1; i++)
                this.listViewElementos.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            this.listViewElementos.Columns[this.listViewElementos.Columns.Count - 1].Width = -2;
        }

        /// <summary>
        /// El valor seleccionado se almacena en la variable global ElementosSel
        /// y se cierra el formulario
        /// </summary>
        private void DevolverValor()
        {
            if (this.listViewElementos.SelectedItems.Count > 0)
            {
                ArrayList elementosSel = new System.Collections.ArrayList();

                this._zonaSel = this.listViewElementos.SelectedItems[0].SubItems[1].Text;

                for (int i = 0; i < this.listViewElementos.SelectedItems[0].SubItems.Count; i++)
                {
                    elementosSel.Add(this.listViewElementos.SelectedItems[0].SubItems[i].Text);
                }

                //GlobalVar.ElementosSel = elementosSel;

                //Define el evento local al user control que se ejecutará después de pulsar el botón aceptar o doble clik antes de cerrar el formulario .
                //Básicamente será una especie de conversión de eventos, en donde un evento atrapado localmente, es transformado en un evento exterior.
                if (OkForm != null)
                {
                    OkForm(new OkFormCommandEventArgs(elementosSel));
                }

                this.Close();
            }
        }

        #endregion
    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// Permite Ordenar un ListView por Columnas
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}
