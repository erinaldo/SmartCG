using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiSuministroInfoRespuesta :  frmPlantilla, IReLocalizable
    {
        private string titulo;

        private string compania;
        private string ejercicio;
        private string periodo;
        private string libroCodigo;
        private string operacionCodigo;
        private string libroDescripcion;
        private string operacionDescripcion;

        private DataTable dtRespuesta;

        private DataSet dsRespuesta;

        bool existeContraparte = false;
        bool existeClaveOperacion = false;

        #region Properties
        public string Titulo
        {
            get
            {
                return (this.titulo);
            }
            set
            {
                this.titulo = value;
            }
        }

        public string Compania
        {
            get
            {
                return (this.compania);
            }
            set
            {
                this.compania = value;
            }
        }

        public string Ejercicio
        {
            get
            {
                return (this.ejercicio);
            }
            set
            {
                this.ejercicio = value;
            }
        }

        public string Periodo
        {
            get
            {
                return (this.periodo);
            }
            set
            {
                this.periodo = value;
            }
        }

        public string LibroCodigo
        {
            get
            {
                return (this.libroCodigo);
            }
            set
            {
                this.libroCodigo = value;
            }
        }

        public string LibroDescripcion
        {
            get
            {
                return (this.libroDescripcion);
            }
            set
            {
                this.libroDescripcion = value;
            }
        }

        public string OperacionCodigo
        {
            get
            {
                return (this.operacionCodigo);
            }
            set
            {
                this.operacionCodigo = value;
            }
        }

        public string OperacionDescripcion
        {
            get
            {
                return (this.operacionDescripcion);
            }
            set
            {
                this.operacionDescripcion = value;
            }
        }

        public DataTable DTRespuesta
        {
            get
            {
                return (this.dtRespuesta);
            }
            set
            {
                this.dtRespuesta = value;
            }
        }

        public DataSet DSRespuesta
        {
            get
            {
                return (this.dsRespuesta);
            }
            set
            {
                this.dsRespuesta = value;
            }
        }

        public bool ExisteContraparte
        {
            get
            {
                return (this.existeContraparte);
            }
            set
            {
                this.existeContraparte = value;
            }
        }

        public bool ExisteClaveOperacion
        {
            get
            {
                return (this.existeClaveOperacion);
            }
            set
            {
                this.existeClaveOperacion = value;
            }
        }
        #endregion
        public frmSiiSuministroInfoRespuesta()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.titulo + " - " + this.LP.GetText("lblRespuesta", "Respuesta");       //Falta traducir
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Actualiza los valores de los controles de la cabecera
        /// </summary>
        private void ActualizarValoresCabecera()
        {
            this.lblCompaniaValor.Text = this.compania;

            if (this.ejercicio == "")
            {
                this.lblEjercicio.Visible = false;
                this.lblEjercicioValor.Visible = false;
            }
            else this.lblEjercicioValor.Text = this.ejercicio;

            if (this.periodo == "")
            {
                this.lblPeriodo.Visible = false;
                this.lblPeriodoValor.Visible = false;
            }
            else this.lblPeriodoValor.Text = this.periodo;

            this.lblLibroValor.Text = this.libroDescripcion;
            this.lblOperacionValor.Text = this.operacionDescripcion;
        }

        /// <summary>
        /// Construir el control de la Grid que contiene la información de las facturas enviadas
        /// </summary>
        private void BuiltgGridDatos()
        {
            if (this.dsRespuesta == null)
            {
                MessageBox.Show("No existe respuesta del envío", "Error");  //Falta traducir
                return;
            }

            //Crear el DataGrid
            this.tgGridRespuesta.dsDatos = new DataSet();
            this.tgGridRespuesta.dsDatos.DataSetName = "Respuesta";
            this.tgGridRespuesta.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridRespuesta.ReadOnly = true;
            this.tgGridRespuesta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridRespuesta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridRespuesta.AllowUserToAddRows = false;
            this.tgGridRespuesta.AllowUserToOrderColumns = false;
            this.tgGridRespuesta.AutoGenerateColumns = false;

            this.tgGridRespuesta.BackgroundColor = Color.White;
            this.tgGridRespuesta.RowHeadersVisible = true;      //Mostrar la columna de seleccion de la Grid

            //Crear la columnas del DataGrid
            this.tgGridRespuesta.AddTextBoxColumn(0, "Compania", this.LP.GetText("dgHeaderCompania", "Compañía"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            //this.tgGridRespuesta.AddTextBoxColumn(1, "Ejercicio", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 30, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            //this.tgGridRespuesta.AddTextBoxColumn(2, "Periodo", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 15, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(1, "Libro", this.LP.GetText("dgHeaderFechaLibro", "Libro"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(2, "Operacion", this.LP.GetText("dgHeaderOperacion", "Operación"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(3, "Estado", this.LP.GetText("dgHeaderEstado", "Estado"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(4, "NIFIdEmisor", this.LP.GetText("dgHeaderNoFactura", "NIF/Id Emisor"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(5, "NoFactura", this.LP.GetText("dgHeaderNoFactura", "No Factura"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(6, "FechaDoc", this.LP.GetText("dgHeaderFechaDoc", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(7, "NombreRazonSocial", this.LP.GetText("dgHeaderContraparteNombre", "Contraparte Nombre"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(8, "ClaveOperacion", this.LP.GetText("dgHeaderClaveOperacion", "Clave Operación"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(9, "NIF", "NIF", 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(10, "IdOtroCodPais", "IdOtroCodPais", 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(11, "IdOtroTipo", "IdOtroTipo", 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(12, "IdOtroId", "IdOtroId", 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);     //Falta traducir
            this.tgGridRespuesta.AddTextBoxColumn(13, "RowResumen", "RowResumen", 5, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, false);     //Falta traducir

            //Adicionar el DataTable al DataSet del DataGrid

            if (dsRespuesta != null && dsRespuesta.Tables != null && dsRespuesta.Tables.Count > 0) 
            {
               if (dsRespuesta.Tables["Resultado"] != null )
               {
                   this.tgGridRespuesta.dsDatos.Tables.Add(dsRespuesta.Tables["Resultado"].Copy());
               }
            }

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridRespuesta.DataSource = this.tgGridRespuesta.dsDatos.Tables["Resultado"];

            this.tgGridRespuesta.ClearSelection();
        }

        /// <summary>
        /// Construir el control de la Grid que contiene la información del resumen de envio
        /// </summary>
        private void BuiltgGridResumenEnvio()
        {
            if (this.dsRespuesta == null)
            {
                MessageBox.Show("No existe respuesta del envío", "Error");  //Falta traducir
                return;
            }

            //Crear el DataGrid
            this.tgGridResumen.dsDatos = new DataSet();
            this.tgGridResumen.dsDatos.DataSetName = "Resumen";
            this.tgGridResumen.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridResumen.ReadOnly = true;
            this.tgGridResumen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridResumen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridResumen.AllowUserToAddRows = false;
            this.tgGridResumen.AllowUserToOrderColumns = false;
            this.tgGridResumen.AutoGenerateColumns = false;

            this.tgGridResumen.BackgroundColor = Color.White;
            this.tgGridResumen.RowHeadersVisible = true;      //Mostrar la columna de seleccion de la Grid

            //Crear la columnas del DataGrid
            this.tgGridResumen.AddTextBoxColumn(0, "Compania", this.LP.GetText("dgHeaderCompania", "Compañía"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(1, "Libro", this.LP.GetText("dgHeaderFechaLibro", "Libro"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(2, "Operacion", this.LP.GetText("dgHeaderOperacion", "Operación"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(3, "TotalFactEnviadas", this.LP.GetText("dgHeaderTotalFactEnviadas", "Total Fact. Enviadas"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(4, "TotalFactCorrectas", this.LP.GetText("dgHeaderTotalFactCorrectas", "Total Fact. Correctas"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(5, "TotalFactAceptadasError", this.LP.GetText("dgHeaderTotalFactAceptadasError", "Total Fact. Aceptadas Con Errores"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(6, "TotalFactErrores", this.LP.GetText("dgHeaderTotalFactErrores", "Total Fact. Errores"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridResumen.AddTextBoxColumn(7, "TotalFactNoEnviadas", this.LP.GetText("dgHeaderTotalFactNoEnviadas", "Total Fact. No Enviadas"), 120, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir

            //Adicionar el DataTable al DataSet del DataGrid
            if (dsRespuesta != null && dsRespuesta.Tables != null && dsRespuesta.Tables.Count > 0)
            {
                if (dsRespuesta.Tables["Resumen"] != null)
                {
                    this.tgGridRespuesta.dsDatos.Tables.Add(dsRespuesta.Tables["Resumen"].Copy());
                }
            }

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridResumen.DataSource = this.tgGridRespuesta.dsDatos.Tables["Resumen"];

            this.tgGridResumen.ClearSelection();
        }
        #endregion

        private void frmSiiSuministroInfoRespuesta_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Suministro Información Respuesta");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Actualiza la cabecera del formulario con los valores de la búsqueda
            this.ActualizarValoresCabecera();

            //Crear el TGGrid con las facturas
            this.BuiltgGridDatos();

            //Crear el TGGrid con el resumen del envio
            this.BuiltgGridResumenEnvio();
        }

        private void frmSiiSuministroInfoRespuesta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiSuministroInfoRespuesta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Suministro Información Respuesta");
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();    //Exporta a un HTML temporal y despúes se muestra en un Excel
        }

        /// <summary>
        /// Exportar la Grid de la respuesta del envío del SII a un fichero HTML que se visualizará en Excel 
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string titulo = "Resultado del Suministro de Información";

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridRespuesta.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridRespuesta.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";
                    nombreTipoVisible[2] = this.tgGridRespuesta.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No 
                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridRespuesta);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIISumInfoRespuesta");

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "EXCEL";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
                catch // si no puede levantar excel, levantar html
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "IEXPLORE";
                    startInfo.Arguments = "\"" + ficheroHTML + "\""; //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    Process.Start(startInfo);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
            Cursor.Current = Cursors.Default;
        }

        private void GridExportar()
        {
            Cursor.Current = Cursors.WaitCursor;

            //Informar que se ha creado con exito o no
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                ExcelExportImport excelImport = new ExcelExportImport();
                excelImport.DateTableDatos = this.tgGridRespuesta.dsDatos.Tables["Resultado"];

                //Titulo
                excelImport.Titulo = this.Text;
                excelImport.Cabecera = true;

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridRespuesta.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridRespuesta.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";
                    nombreTipoVisible[2] = this.tgGridRespuesta.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.tgGridRespuesta.SelectedRows.Count > 0 && this.tgGridRespuesta.SelectedRows.Count < this.tgGridRespuesta.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.tgGridRespuesta.SelectedRows.Count; i++)
                    {
                        indice = this.tgGridRespuesta.SelectedRows[i].Index;

                        if (tgGridRespuesta.Rows.Count - 1 == indice)
                        {
                            //Linea Totales
                            if (aIndice.Count > 0)
                            {
                                aIndice.Add(indice);
                            }
                            else
                            {
                                //Solo linea de Totales, no se adiciona y se exportan todas las filas
                            }
                        }
                        else aIndice.Add(indice); 
                    }

                    excelImport.IndiceFilasSeleccionadas = aIndice;
                }

                string result = excelImport.ExportarMemoria();

                //this.progressBarEspera.Visible = false;
                //this.grBoxProgressBar.Visible = false;

                if (result != "" && result != "CANCELAR")
                {
                    MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + result + ")", error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tgGridRespuesta_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            string marcaResumenEnvio = this.tgGridRespuesta.Rows[e.RowIndex].Cells["RowResumen"].Value.ToString();
            if (marcaResumenEnvio == "1")
            {
                this.tgGridRespuesta.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
            }

            if (!existeContraparte)
                if (this.tgGridRespuesta.Columns.Contains("NombreRazonSocial")) this.tgGridRespuesta.Columns["NombreRazonSocial"].Visible = false;

            if (!existeClaveOperacion)
                if (this.tgGridRespuesta.Columns.Contains("ClaveOperacion")) this.tgGridRespuesta.Columns["ClaveOperacion"].Visible = false;
        }
    }
}
