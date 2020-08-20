using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiSuministroInfoView : frmPlantilla, IReLocalizable
    {
        private string titulo;

        private string compania;
        private string ejercicio;
        private string periodo;
        private string libroCodigo;
        private string operacionCodigo;
        private string libroDescripcion;
        private string operacionDescripcion;
        private string numeroFactura;
        private string fechaDocCGDesde;
        private string fechaDocCGHasta;

        private int totalRegistro = 0;
 
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

        public string NumeroFactura
        {
            get
            {
                return (this.numeroFactura);
            }
            set
            {
                this.numeroFactura = value;
            }
        }

        public string FechaDocCGDesde
        {
            get
            {
                return (this.fechaDocCGDesde);
            }
            set
            {
                this.fechaDocCGDesde = value;
            }
        }

        public string FechaDocCGHasta
        {
            get
            {
                return (this.fechaDocCGHasta);
            }
            set
            {
                this.fechaDocCGHasta = value;
            }
        }
        #endregion

        public frmSiiSuministroInfoView()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiSuministroInfoView_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Suministro Información Ver");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Actualiza la cabecera del formulario con los valores de la búsqueda
            this.ActualizarValoresCabecera();

            this.operacionCodigo = this.operacionCodigo.TrimEnd();

            //Crear el TGGrid
            this.BuiltgGridDatos();
        }

        private void frmSiiSuministroInfoView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiSuministroInfoView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Suministro Información Ver");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.titulo + " - " + this.LP.GetText("lblfrmSiiSuministroViewInfoTitulo", "Pendientes de enviar");       //Falta traducir
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
        /// Construir el control de la Grid que contiene la información pendiente de enviar selecionada
        /// </summary>
        private void BuiltgGridDatos()
        {
            //Crear el DataGrid
            this.tgGridPdteEnvio.dsDatos = new DataSet();
            this.tgGridPdteEnvio.dsDatos.DataSetName = "PdteEnviar";
            this.tgGridPdteEnvio.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridPdteEnvio.ReadOnly = true;
            this.tgGridPdteEnvio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridPdteEnvio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridPdteEnvio.AllowUserToAddRows = false;
            this.tgGridPdteEnvio.AllowUserToOrderColumns = false;
            this.tgGridPdteEnvio.AutoGenerateColumns = false;

            this.tgGridPdteEnvio.RowHeadersVisible = true;      //Mostrar la columna de seleccion de la Grid

            DataTable dt = new DataTable();
            dt.TableName = "Tabla";

            switch (this.libroCodigo)
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                    this.BuildtgGridDatosFacturasEmitidas(ref dt);
                    break;
                case LibroUtiles.LibroID_FacturasRecibidas:
                    this.BuildtgGridDatosFacturasRecibidas(ref dt);
                    break;
                case LibroUtiles.LibroID_BienesInversion:
                    this.BuildtgGridDatosBienesInversion(ref dt);
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                     this.BuildtgGridDatosOperacionesIntracomunitarias(ref dt);
                    break;
                case LibroUtiles.LibroID_CobrosMetalico:
                    this.BuildtgGridDatosCobrosMetalico(ref dt);
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    this.BuildtgGridDatosOperacionesSeguros(ref dt);
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    this.BuildtgGridDatosPagosRecibidas(ref dt);
                    break;
                case LibroUtiles.LibroID_CobrosEmitidas:
                    this.BuildtgGridDatosCobrosEmitidas(ref dt);
                    break;
                case LibroUtiles.LibroID_AgenciasViajes:
                    this.BuildtgGridDatosAgenciasViajes(ref dt);
                    break;
            }

            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridPdteEnvio.dsDatos.Tables.Add(dt);

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridPdteEnvio.DataSource = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"];

            //Cargar la Información
            switch (this.libroCodigo)
            {
                case LibroUtiles.LibroID_FacturasEmitidas:
                    this.FillDataGridDatosFacturasEmitidas();
                    break;
                case LibroUtiles.LibroID_FacturasRecibidas:
                    this.FillDataGridDatosFacturasRecibidas();
                    break;
                case LibroUtiles.LibroID_BienesInversion:
                    this.FillDataGridDatosBienesInversion();
                    break;
                case LibroUtiles.LibroID_OperacionesIntracomunitarias:
                    this.FillDataGridDatosOperacionesIntracomunitarias();
                    break;
                case LibroUtiles.LibroID_CobrosMetalico:
                    this.FillDataGridDatosCobrosMetalico();
                    break;
                case LibroUtiles.LibroID_OperacionesSeguros:
                    this.FillDataGridDatosOperacionesSeguros();
                    break;
                case LibroUtiles.LibroID_PagosRecibidas:
                    this.FillDataGridDatosPagosRecibidas();
                    break;
                case LibroUtiles.LibroID_CobrosEmitidas:
                    this.FillDataGridDatosCobroEmitidas();
                    break;
                case LibroUtiles.LibroID_AgenciasViajes:
                    this.FillDataGridDatosAgenciasViajes();
                    break;
            }

            if (this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Count == 0)
            {
                this.lblResult.Visible = true;
                this.tgGridPdteEnvio.Visible = false;
            }
            else
            {
                this.lblTotalReg.Visible = true;
                this.lblTotalRegValor.Text = this.totalRegistro.ToString();
                this.lblTotalRegValor.Visible = true;
            }
        }

        #region Facturas Emitidas
        private void BuildtgGridDatosFacturasEmitidas(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                dt.Columns.Add("COPS", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));
                dt.Columns.Add("NIFC", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "COPS", this.LP.GetText("dgHeaderCodOperacion", "Código Operación"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(6, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(7, "NIFC", this.LP.GetText("dgHeaderNIFContra", "NIF Contraparte"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(8, "NRAZ", this.LP.GetText("dgHeaderNombreContra", "Nombre Contraparte"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosFacturasEmitidas()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura ||
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII2J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS2 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS2 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS2 = '" + this.periodo + "' ";

                    query += "and TDOCS2 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS2 = '" + this.operacionCodigo + "' and BAJAS2 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS2 = '" + this.operacionCodigo + "' ";

                    if (existeNumeroFactura) query += "and NSFES2 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS2 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS2 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII2J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII2J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS2 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS2 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    //query += "and CIAFS2 = CIAFS1 and EJERS2 = EJERS1 and PERIS2 = PERIS1 and TDOCS2 = TDOCS1 and BAJAS2 = BAJAS1 ";
                    //query += "and NIFES2 = NIFES1 and NSFES2 = NSFES1 and FDOCS2 = FDOCS1 ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS2 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS2 <= " + this.fechaDocCGHasta + " ";
                }

                string FDOC = "";
                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS2")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS2")).ToString().Trim();

                    string nifEmisor = dr.GetValue(dr.GetOrdinal("NIFES2")).ToString().Trim();

                    row["NIFE"] = nifEmisor;

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES2")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS2")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    string codOper = dr.GetValue(dr.GetOrdinal("COPSS2")).ToString().Trim();
                    row["COPS"] = LibroUtiles.ListaSII_Descripcion("E", codOper, null);
                    
                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS2")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    row["NIFC"] = dr.GetValue(dr.GetOrdinal("NIFCS2")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS2")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region Facturas Recibidas
        private void BuildtgGridDatosFacturasRecibidas(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                dt.Columns.Add("COPS", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));
                dt.Columns.Add("NIFC", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "COPS", this.LP.GetText("dgHeaderCodOperacion", "Código Operación"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(6, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(7, "NIFC", this.LP.GetText("dgHeaderNIFContra", "NIF Contraparte"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(8, "NRAZ", this.LP.GetText("dgHeaderNombreContra", "Nombre Contraparte"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosFacturasRecibidas()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura ||
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII3J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS3 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS3 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS3 = '" + this.periodo + "' ";

                    query += "and TDOCS3 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS3 = '" + this.operacionCodigo + "' and BAJAS3 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS3 = '" + this.operacionCodigo + "' ";

                    if (this.numeroFactura != null && this.numeroFactura != "") query += "and NSFES3 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS3 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS3 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII3J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII3J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS3 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS3 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS3 = CIAFS1 and EJERS3 = EJERS1 and PERIS3 = PERIS1 and TDOCS3 = TDOCS1 and BAJAS3 = BAJAS1 ";
                    query += "and NIFES3 = NIFES1 and PAISS3 = PAISS1 and TIDES3 = TIDES1 and IDOES3 = IDOES1 ";
                    query += "and NSFES3 = NSFES1 and FDOCS3 = FDOCS1 ";*/

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS3 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS3 <= " + this.fechaDocCGHasta + " ";
                }
                
                string FDOC = "";
                string importeStr = "";
                decimal importe;
                string NIFES3 = "";
                string nifEmisor = "";
                string paisEmisor = "";
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS3")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS3")).ToString().Trim();

                    NIFES3 = dr.GetValue(dr.GetOrdinal("NIFES3")).ToString().Trim();
                    if (NIFES3 != "") nifEmisor = NIFES3;
                    else
                    {
                        paisEmisor = dr.GetValue(dr.GetOrdinal("PAISS3")).ToString().Trim();
                        if (paisEmisor == "" || paisEmisor == "ES") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDES3")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES3")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDES3")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES3")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES3")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS3")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    string codOper = dr.GetValue(dr.GetOrdinal("COPSS3")).ToString().Trim();
                    row["COPS"] = LibroUtiles.ListaSII_Descripcion("F", codOper, null);

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS3")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    row["NIFC"] = dr.GetValue(dr.GetOrdinal("NIFCS3")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS3")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region Bienes de Inversion
        private void BuildtgGridDatosBienesInversion(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                dt.Columns.Add("PRAD", typeof(string));
                dt.Columns.Add("REGA", typeof(string));
                dt.Columns.Add("RGDE", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "PRAD", "Prorrata Anual Definitiva", 70, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(6, "REGA", "Regulación Anual Deducción", 100, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(7, "RGDE", "Regularización Deducción Efectuada", 100, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(8, "NRAZ", this.LP.GetText("dgHeaderNombreContra", "Nombre Contraparte"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosBienesInversion()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura ||
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII4J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS4 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS4 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS4 = '" + this.periodo + "' ";

                    query += "and TDOCS4 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS4 = '" + this.operacionCodigo + "' and BAJAS4 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS4 = '" + this.operacionCodigo + "' ";

                    if (this.numeroFactura != null && this.numeroFactura != "") query += "and NSFES4 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS4 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS4 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII4J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII4J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS4 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS4 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS4 = CIAFS1 and EJERS4 = EJERS1 and PERIS4 = PERIS1 and TDOCS4 = TDOCS1 and BAJAS4 = BAJAS1 ";
                    query += "and NIFES4 = NIFES1 and PAISS4 = PAISS1 and TIDES4 = TIDES1 and IDOES4 = IDOES1 ";
                    query += "and NSFES4 = NSFES1 and FDOCS4 = FDOCS1 ";*/

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS4 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS4 <= " + this.fechaDocCGHasta + " ";
                }
                
                string FDOC = "";
                string NIFES4 = "";
                string nifEmisor = "";
                string paisEmisor = "";
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS4")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS4")).ToString().Trim();

                    NIFES4 = dr.GetValue(dr.GetOrdinal("NIFES4")).ToString().Trim();
                    if (NIFES4 != "") nifEmisor = NIFES4;
                    else
                    {
                        paisEmisor = dr.GetValue(dr.GetOrdinal("PAISS4")).ToString().Trim();
                        if (paisEmisor == "" || paisEmisor == "ES") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDES4")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES4")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDES4")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES4")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES4")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS4")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    row["PRAD"] = dr.GetValue(dr.GetOrdinal("PRADS4")).ToString().Trim();
                    row["REGA"] = dr.GetValue(dr.GetOrdinal("REGAS4")).ToString().Trim();
                    row["RGDE"] = dr.GetValue(dr.GetOrdinal("RGDES4")).ToString().Trim();

                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS4")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region CobrosMetalico
        private void BuildtgGridDatosCobrosMetalico(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));
                
                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "NRAZ", this.LP.GetText("dgHeaderNombreRazonSocial", "Nombre o Razón Social"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 90, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosCobrosMetalico()
        {
            IDataReader dr = null;
            try
            {
                string query = "";

                if (operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII6J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS6 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS6 = '" + this.ejercicio + "' ";
                    query += "and PERIS6 = '" + LibroUtiles.PeriodoAnual + "' ";

                    query += "and TDOCS6 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS6 = '" + this.operacionCodigo + "' and BAJAS6 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS6 = '" + this.operacionCodigo + "' ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII6J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII6J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS6 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS6 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS6 = CIAFS1 and EJERS6 = EJERS1 and PERIS6 = PERIS1 and TDOCS6 = TDOCS1 and BAJAS6 = BAJAS1 ";
                    query += "and NIFCS6 = NIFES1 and PAICS6 = PAISS1 and TIDCS6 = TIDES1 and IDOCS6 = IDOES1 ";*/
                    //query += "and NSFES6 = NSFES1 and FDOCS6 = FDOCS1";
                }

                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS6")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS6")).ToString().Trim();

                    string nifEmisor = dr.GetValue(dr.GetOrdinal("NIFCS6")).ToString().Trim();
                    if (nifEmisor == "")
                    {
                        string paisEmisor = dr.GetValue(dr.GetOrdinal("PAICS6")).ToString().Trim();
                        if (paisEmisor == "") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDCS6")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCS6")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDCS6")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCS6")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS6")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region Operaciones Intracomunitarias
        private void BuildtgGridDatosOperacionesIntracomunitarias(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                //dt.Columns.Add("COPS", typeof(string));
                //dt.Columns.Add("IMPT", typeof(string));
                //dt.Columns.Add("NIFC", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                //this.tgGridPdteEnvio.AddTextBoxColumn(5, "COPS", this.LP.GetText("dgHeaderCodOperacion", "Código Operación"), 70, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                //this.tgGridPdteEnvio.AddTextBoxColumn(6, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                //this.tgGridPdteEnvio.AddTextBoxColumn(7, "NIFC", this.LP.GetText("dgHeaderNIFContra", "NIF Contraparte"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "NRAZ", this.LP.GetText("dgHeaderNombreContra", "Nombre Contraparte"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosOperacionesIntracomunitarias()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura ||
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII5J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS5 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS5 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS5 = '" + this.periodo + "' ";

                    query += "and TDOCS5 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS5 = '" + this.operacionCodigo + "' and BAJAS5 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS5 = '" + this.operacionCodigo + "' ";

                    if (this.numeroFactura != null && this.numeroFactura != "") query += "and NSFES5 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS5 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS5 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII5J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII5J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS1 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS5 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS5 = CIAFS1 and EJERS5 = EJERS1 and PERIS5 = PERIS1 and TDOCS5 = TDOCS1 and BAJAS5 = BAJAS1 ";
                    query += "and NIFES5 = NIFES1 and PAISS5 = PAISS1 and TIDES5 = TIDES1 and IDOES5 = IDOES1 ";
                    query += "and NSFES5 = NSFES1 and FDOCS5 = FDOCS1 ";*/

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS5 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS5 <= " + this.fechaDocCGHasta + " ";
                }

                string FDOC = "";
                //string importeStr = "";
                //decimal importe;
                DataRow row;

                string nifEmisor = "";
                string NIFES5 = "";
                string paisEmisor = "";
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS5")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS5")).ToString().Trim();

                    NIFES5 = dr.GetValue(dr.GetOrdinal("NIFES5")).ToString().Trim();
                    if (NIFES5 != "") nifEmisor = NIFES5;
                    else
                    {
                        paisEmisor = dr.GetValue(dr.GetOrdinal("PAISS5")).ToString().Trim();
                        if (paisEmisor == "" || paisEmisor == "ES") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDES5")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES5")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDES5")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES5")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES5")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS5")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    /*row["COPS"] = dr.GetValue(dr.GetOrdinal("COPSS3")).ToString().Trim();

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS3")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    row["NIFC"] = dr.GetValue(dr.GetOrdinal("NIFCS3")).ToString().Trim();
                    */
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS5")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region OperacionesSeguros
        private void BuildtgGridDatosOperacionesSeguros(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("CLOS", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "NRAZ", this.LP.GetText("dgHeaderNombreRazonSocial", "Nombre o Razón Social"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "CLOS", this.LP.GetText("dgHeaderClaveOper", "Clave Operación"), 250, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 90, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosOperacionesSeguros()
        {
            IDataReader dr = null;
            try
            {
                string query = "";

                if (operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII7J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS7 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS7 = '" + this.ejercicio + "' ";
                    query += "and PERIS7 = '" + LibroUtiles.PeriodoAnual + "' ";

                    query += "and TDOCS7 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS7 = '" + this.operacionCodigo + "' and BAJAS7 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS7 = '" + this.operacionCodigo + "' ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII7J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII7J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS7 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS7 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS7 = CIAFS1 and EJERS7 = EJERS1 and PERIS7 = PERIS1 and TDOCS7 = TDOCS1 and BAJAS7 = BAJAS1 ";
                    query += "and NIFCS7 = NIFES1 and PAICS7 = PAISS1 and TIDCS7 = TIDES1 and IDOCS7 = IDOES1 ";*/
                    //query += "and NSFES7 = NSFES1 and FDOCS7 = FDOCS1";
                }

                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS7")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS7")).ToString().Trim();

                    string nifEmisor = dr.GetValue(dr.GetOrdinal("NIFCS7")).ToString().Trim();
                    if (nifEmisor == "")
                    {
                        string paisEmisor = dr.GetValue(dr.GetOrdinal("PAICS7")).ToString().Trim();
                        if (paisEmisor == "") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDCS7")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCS7")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDCS7")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCS7")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    string claveOper = dr.GetValue(dr.GetOrdinal("CLOSS7")).ToString().Trim();
                    row["CLOS"] = LibroUtiles.ListaSII_Descripcion("X", claveOper, null);

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS7")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region PagosRecibidas
        private void BuildtgGridDatosPagosRecibidas(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("NSFR", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                dt.Columns.Add("FPAG", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));
                dt.Columns.Add("MPAG", typeof(string));
                dt.Columns.Add("CTAP", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NRAZ", this.LP.GetText("dgHeaderNombreRazonSocial", "Nombre o Razón Social"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "NSFR", this.LP.GetText("dgHeaderNumFacturaEmisorResumen", "Número Serie Factura Emisor Resumen"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(6, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(7, "FPAG", this.LP.GetText("dgHeaderFechaPago", "Fecha Pago"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(8, "IMPT", this.LP.GetText("dgHeaderImportePago", "Importe Pago"), 90, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(9, "MPAG", this.LP.GetText("dgHeaderMedioPago", "Medio Pago"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(10, "CTAP", this.LP.GetText("dgHeaderCuentaOMedio", "Cuenta o Medio Pago"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosPagosRecibidas()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura ||
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII9J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS9 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS9 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS9 = '" + this.periodo + "' ";

                    query += "and TDOCS9 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS9 = '" + this.operacionCodigo + "' and BAJAS9 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS9 = '" + this.operacionCodigo + "' ";

                    if (existeNumeroFactura) query += "and NSFES9 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS9 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS9 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII9J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII9J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS9 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS9 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS9 = CIAFS1 and EJERS9 = EJERS1 and PERIS9 = PERIS1 and TDOCS9 = TDOCS1 and BAJAS9 = BAJAS1 ";
                    query += "and NIFES9 = NIFES1 and PAISS9 = PAISS1 and TIDES9 = TIDES1 and IDOES9 = IDOES1 ";
                    query += "and NSFES9 = NSFES1 and FDOCS9 = FDOCS1 ";*/

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS9 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS9 <= " + this.fechaDocCGHasta + " ";
                }

                string FDOC = "";
                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS9")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS9")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZS9")).ToString().Trim();

                    string nifEmisor = dr.GetValue(dr.GetOrdinal("NIFES9")).ToString().Trim();
                    if (nifEmisor == "")
                    {
                        string paisEmisor = dr.GetValue(dr.GetOrdinal("PAISS9")).ToString().Trim();
                        if (paisEmisor == "") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDES9")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES9")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDES9")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOES9")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES9")).ToString().Trim();
                    row["NSFR"] = dr.GetValue(dr.GetOrdinal("NSFRS9")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS9")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    FDOC = dr.GetValue(dr.GetOrdinal("FPAGS9")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FPAG"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FPAG"] = "";

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS9")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    row["MPAG"] = dr.GetValue(dr.GetOrdinal("MPAGS9")).ToString().Trim();
                    row["CTAP"] = dr.GetValue(dr.GetOrdinal("CTAPS9")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region CobrosEmitidas
        private void BuildtgGridDatosCobrosEmitidas(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("PERI", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("NSFE", typeof(string));
                dt.Columns.Add("NSFR", typeof(string));
                dt.Columns.Add("FDOC", typeof(string));
                dt.Columns.Add("FCOB", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));
                dt.Columns.Add("MCOB", typeof(string));
                dt.Columns.Add("CTAC", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "PERI", this.LP.GetText("dgHeaderPeriodo", "Periodo"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "NSFE", this.LP.GetText("dgHeaderNumFactura", "Número Factura"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(4, "NSFR", this.LP.GetText("dgHeaderNumFacturaEmisorResumen", "Número Serie Factura Emisor Resumen"), 140, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(5, "FDOC", this.LP.GetText("dgHeaderFechaDocumento", "Fecha Documento"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(6, "FCOB", this.LP.GetText("dgHeaderFechaPago", "Fecha Pago"), 100, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(7, "IMPT", this.LP.GetText("dgHeaderImportePago", "Importe Pago"), 90, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(8, "MCOB", this.LP.GetText("dgHeaderMedioPago", "Medio Pago"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(9, "CTAC", this.LP.GetText("dgHeaderCuentaOMedio", "Cuenta o Medio Pago"), 150, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosCobroEmitidas()
        {
            IDataReader dr = null;
            try
            {
                string query = "";
                bool existeNumeroFactura = false;
                if (this.numeroFactura != null && this.numeroFactura != "") existeNumeroFactura = true;

                if (existeNumeroFactura || 
                    operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII8J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS8 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERS8 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PERIS8 = '" + this.periodo + "' ";

                    query += "and TDOCS8 = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMS8 = '" + this.operacionCodigo + "' and BAJAS8 = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJAS8 = '" + this.operacionCodigo + "' ";

                    if (existeNumeroFactura) query += "and NSFES8 LIKE '%" + this.numeroFactura + "%' ";

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS8 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS8 <= " + this.fechaDocCGHasta + " ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSII8J ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII8J ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFS8 = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCS8 = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFS8 = CIAFS1 and EJERS8 = EJERS1 and PERIS8 = PERIS1 and TDOCS8 = TDOCS1 and BAJAS8 = BAJAS1 ";
                    query += "and NIFES8 = NIFES1 and NSFES8 = NSFES1 and FDOCS8 = FDOCS1 ";*/

                    if (this.fechaDocCGDesde != null && this.fechaDocCGDesde != "") query += "and FDOCS8 >= " + this.fechaDocCGDesde + " ";

                    if (this.fechaDocCGHasta != null && this.fechaDocCGHasta != "") query += "and FDOCS8 <= " + this.fechaDocCGHasta + " ";
                }

                string FDOC = "";
                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERS8")).ToString().Trim();
                    row["PERI"] = dr.GetValue(dr.GetOrdinal("PERIS8")).ToString().Trim();
                    row["NIFE"] = dr.GetValue(dr.GetOrdinal("NIFES8")).ToString().Trim();

                    row["NSFE"] = dr.GetValue(dr.GetOrdinal("NSFES8")).ToString().Trim();
                    row["NSFR"] = dr.GetValue(dr.GetOrdinal("NSFRS8")).ToString().Trim();

                    FDOC = dr.GetValue(dr.GetOrdinal("FDOCS8")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FDOC"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FDOC"] = "";

                    FDOC = dr.GetValue(dr.GetOrdinal("FCOBS8")).ToString();
                    if (FDOC != "" && FDOC != "0") row["FCOB"] = utiles.FormatoCGToFecha(FDOC).ToShortDateString();
                    else row["FCOB"] = "";

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTS8")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    row["MCOB"] = dr.GetValue(dr.GetOrdinal("MCOBS8")).ToString().Trim();
                    row["CTAC"] = dr.GetValue(dr.GetOrdinal("CTACS8")).ToString().Trim();

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        #region AgenciasViajes
        private void BuildtgGridDatosAgenciasViajes(ref DataTable dt)
        {
            try
            {
                //Adicionar las columnas al DataTable
                dt.Columns.Add("EJER", typeof(string));
                dt.Columns.Add("NRAZ", typeof(string));
                dt.Columns.Add("NIFE", typeof(string));
                dt.Columns.Add("IMPT", typeof(string));

                //Crear la columnas del DataGrid
                this.tgGridPdteEnvio.AddTextBoxColumn(0, "EJER", this.LP.GetText("dgHeaderEjercicio", "Ejercicio"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(1, "NRAZ", this.LP.GetText("dgHeaderNombreRazonSocial", "Nombre o Razón Social"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(2, "NIFE", this.LP.GetText("dgHeaderNIFEmisor", "NIF Emisor"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
                this.tgGridPdteEnvio.AddTextBoxColumn(3, "IMPT", this.LP.GetText("dgHeaderImporteTotal", "Importe Total"), 90, 50, typeof(string), DataGridViewContentAlignment.MiddleRight, true);     //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGridDatosAgenciasViajes()
        {
            IDataReader dr = null;
            try
            {
                string query = "";

                if (operacionCodigo != "B" ||
                    (operacionCodigo == "B" && this.ejercicio == "" && this.periodo == ""))
                {
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSIIAJ ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFSA = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJERSA = '" + this.ejercicio + "' ";
                    query += "and PERISA = '" + LibroUtiles.PeriodoAnual + "' ";

                    query += "and TDOCSA = '" + this.libroCodigo + "' ";

                    if (operacionCodigo != "B") query += "and TCOMSA = '" + this.operacionCodigo + "' and BAJASA = ' ' ";
                    else if (operacionCodigo == "B") query += "and BAJASA = '" + this.operacionCodigo + "' ";
                }
                else
                {
                    //Operación de Baja con ejercicio y/o periodo indicado
                    //query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSII1, " + GlobalVar.PrefijoTablaCG + "IVSIIAJ ";
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "IVSIIAJ ";
                    query += "where STATS1 = ' ' ";
                    query += "and CIAFSA = '" + this.compania + "' ";
                    query += "and DEDUS1 = '" + this.agencia + "' ";

                    if (this.ejercicio != "") query += "and EJBAS1 = '" + this.ejercicio + "' ";
                    if (this.periodo != "") query += "and PEBAS1 = '" + this.periodo + "' ";

                    query += "and TDOCSA = '" + this.libroCodigo + "' ";
                    query += "and BAJAS1 = '" + this.operacionCodigo + "' ";

                    /*query += "and CIAFSA = CIAFS1 and EJERSA = EJERS1 and PERISA = PERIS1 and TDOCSA = TDOCS1 and BAJASA = BAJAS1 ";
                    query += "and NIFCSA = NIFES1 and PAICSA = PAISS1 and TIDCSA = TIDES1 and IDOCSA = IDOES1 ";*/
                    //query += "and NSFESA = NSFES1 and FDOCSA = FDOCS1";
                }

                string importeStr = "";
                decimal importe;
                DataRow row;
                this.totalRegistro = 0;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].NewRow();

                    row["EJER"] = dr.GetValue(dr.GetOrdinal("EJERSA")).ToString().Trim();
                    row["NRAZ"] = dr.GetValue(dr.GetOrdinal("NRAZSA")).ToString().Trim();

                    string nifEmisor = dr.GetValue(dr.GetOrdinal("NIFCSA")).ToString().Trim();
                    if (nifEmisor == "")
                    {
                        string paisEmisor = dr.GetValue(dr.GetOrdinal("PAICSA")).ToString().Trim();
                        if (paisEmisor == "") nifEmisor = dr.GetValue(dr.GetOrdinal("TIDCSA")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCSA")).ToString().Trim();
                        else nifEmisor = paisEmisor + "-" + dr.GetValue(dr.GetOrdinal("TIDCSA")).ToString().Trim() + "-" + dr.GetValue(dr.GetOrdinal("IDOCSA")).ToString().Trim();
                    }
                    row["NIFE"] = nifEmisor;

                    importe = 0;
                    importeStr = dr.GetValue(dr.GetOrdinal("IMPTSA")).ToString().Trim();
                    if (importeStr != "")
                        try { importe = Convert.ToDecimal(importeStr); }
                        catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                    row["IMPT"] = importe.ToString("N2", this.LP.MyCultureInfo);

                    this.tgGridPdteEnvio.dsDatos.Tables["Tabla"].Rows.Add(row);

                    this.totalRegistro++;
                }

                //Ningún registro seleccionado
                this.tgGridPdteEnvio.ClearSelection();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }
        #endregion

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();
        }
        #endregion

        /// <summary>
        /// Exportar la Grid de la respuesta del envío del SII a un fichero HTML que se visualizará en Excel 
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string titulo = this.titulo + " - " + this.LP.GetText("lblfrmSiiSuministroViewInfoTitulo", "Pendientes de enviar");       //Falta traducir

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridPdteEnvio.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridPdteEnvio.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";
                    nombreTipoVisible[2] = this.tgGridPdteEnvio.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No 
                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridPdteEnvio);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIISumInfoPdteEnvio");

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
                excelImport.DateTableDatos = this.tgGridPdteEnvio.dsDatos.Tables["Tabla"];

                //Titulo
                excelImport.Titulo = this.Text;
                excelImport.Cabecera = true;

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridPdteEnvio.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridPdteEnvio.Columns[i].HeaderText;                   //Nombre de la columna
                    nombreTipoVisible[1] = "string";
                    nombreTipoVisible[2] = this.tgGridPdteEnvio.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                    descColumnas.Add(nombreTipoVisible);
                }
                excelImport.GridColumnas = descColumnas;

                //Filas seleccionadas
                if (this.tgGridPdteEnvio.SelectedRows.Count > 0 && this.tgGridPdteEnvio.SelectedRows.Count < this.tgGridPdteEnvio.Rows.Count)
                {
                    int indice = 0;
                    ArrayList aIndice = new ArrayList();
                    for (int i = 0; i < this.tgGridPdteEnvio.SelectedRows.Count; i++)
                    {
                        indice = this.tgGridPdteEnvio.SelectedRows[i].Index;

                        if (tgGridPdteEnvio.Rows.Count - 1 == indice)
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
    }
}
