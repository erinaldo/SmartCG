using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Diagnostics;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiAlerta : frmPlantilla, IReLocalizable
    {
        private DataSet dsAlertaEnvio;

        bool enviosExpanded = true;

        public frmSiiAlerta()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmSiiAlerta_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Alerta");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGGrid de Resumen
            this.BuiltgEnviosResumenInfo();

            //Crear el TGGrid de Facturas
            this.BuiltgEnviosFacturasInfo();

            //Ejecutar la consulta 
            this.Consultar();

            //this.tgGridResumen.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Consultar();
        }

        private void tgGridResumen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridResumen.Rows.Count > 0)
            {
                this.tgGridResumen.Rows[0].Selected = false;

                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("DATER1")) this.tgGridResumen.Columns["DATER1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TIMER1")) this.tgGridResumen.Columns["TIMER1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TDOCR1")) this.tgGridResumen.Columns["TDOCR1"].Visible = false;
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("COPSR1")) this.tgGridResumen.Columns["COPSR1"].Visible = false;
            }
        }

        private void tgGridResumen_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridResumen.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridResumen.dsDatos.Tables[0].Rows.Count)
                {
                    this.BuscarFacturasEnviadas();
                }
            }
        }

        private void tgGridResumen_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridResumen.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridResumen.dsDatos.Tables[0].Rows.Count)
                {
                    this.BuscarFacturasEnviadas();
                }
            }
        }

        private void tgGridResumen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.tgGridResumen.dsDatos.Tables.Count >= 1 && e.RowIndex <= this.tgGridResumen.dsDatos.Tables[0].Rows.Count)
                {
                    if (!this.enviosExpanded)
                    {
                        //Buscar las facturas del envio
                        this.BuscarFacturasEnviadas();
                    }
                }
            }
        }

        private void btnCerrarFacturas_Click(object sender, EventArgs e)
        {
            this.enviosExpanded = true;
            this.tgGridFacturas.Visible = false;
            this.toolStripButtonExportar.Enabled = false;
            //Grid Resumen modificar tamaño (más pequeño)
            this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 608);
        }

        private void tgGridResumen_SelectionChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!this.enviosExpanded)
            {
                //Buscar las facturas del envio
                this.BuscarFacturasEnviadas();
            }

            Cursor.Current = Cursors.Default;
        }
        
        private void tgGridFacturas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.tgGridFacturas.Rows.Count > 0)
            {
                this.tgGridFacturas.Rows[0].Selected = false;

                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdNif")) this.tgGridFacturas.Columns["IdNif"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroPais")) this.tgGridFacturas.Columns["IdOtroPais"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroTipo")) this.tgGridFacturas.Columns["IdOtroTipo"].Visible = false;
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IdOtroId")) this.tgGridFacturas.Columns["IdOtroId"].Visible = false;
            }
        }

        private void tgGridResumen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1) // 0 is the first column, specify the valid index of ur gridview
            {
                bool valueCurrent = (bool)this.tgGridResumen.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;

                int rowActual = e.RowIndex;

                if (valueCurrent)
                {
                    if (!this.btnConfirmarAlerta.Enabled) this.btnConfirmarAlerta.Enabled = true;
                }
                else
                {
                    if (this.tgGridResumen.Rows.Count == 1) this.btnConfirmarAlerta.Enabled = false;
                    else
                    {
                        int i;
                        bool valor;
                        foreach (DataGridViewRow row in this.tgGridResumen.Rows)
                        {
                            i = row.Index;

                            DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Selected"];

                            if (i == rowActual)
                            {
                                if (chk.Value.ToString() == "") valor = true;
                                else if (!((bool)chk.Value)) valor = true;
                                else valor = false;
                            }
                            else
                            {
                                if (chk.Value.ToString() == "") valor = false;
                                else if (!((bool)chk.Value)) valor = false;
                                else valor = true;
                            }

                            if (valor)
                            {
                                this.btnConfirmarAlerta.Enabled = true;
                                return;
                            };
                        }
                        this.btnConfirmarAlerta.Enabled = false;
                    }
                }
            }
        }

        private void chkMarcarDesmTodo_CheckedChanged(object sender, EventArgs e)
        {
            this.tgGridResumen.CurrentCell.Selected = false;
            this.tgGridResumen.ClearSelection();
            if (this.chkMarcarDesmTodo.Checked)
            {
                //Marcar
                foreach (DataGridViewRow row in this.tgGridResumen.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Selected"];
                    chk.Value = 1;
                }
                this.btnConfirmarAlerta.Enabled = true;
            }
            else
            {
                //Desmarcar
                foreach (DataGridViewRow row in this.tgGridResumen.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Selected"];
                    chk.Value = 0;
                }
                this.btnConfirmarAlerta.Enabled = false;
            }
            this.tgGridResumen.Refresh();
        }

        private void btnConfirmarAlerta_Click(object sender, EventArgs e)
        {
            try
            {
                IVRSIIKey clave;
                ArrayList aClaves = new ArrayList();
                string query = "";
                bool valor;
                foreach (DataGridViewRow row in this.tgGridResumen.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Selected"];

                    if (chk.Value.ToString() == "") valor = false;
                    else if (!((bool)chk.Value)) valor = false;
                    else valor = true;

                    if (valor)
                    {
                        clave = new IVRSIIKey();
                        clave.CSVER1 = row.Cells["CSV"].Value.ToString();
                        clave.NIFDR1 = row.Cells["NIF"].Value.ToString();
                        clave.DATER1 = row.Cells["DATER1"].Value.ToString();
                        clave.TIMER1 = row.Cells["TIMER1"].Value.ToString();
                        clave.TDOCR1 = row.Cells["TDOCR1"].Value.ToString();
                        clave.COPSR1 = row.Cells["COPSR1"].Value.ToString();
                        aClaves.Add(clave);
                    }
                }

                IVRSIIKey claveActual;
                if (aClaves.Count > 0)
                {
                    //Usuario Logado
                    string userlogado = System.Environment.UserName.ToUpper();
                    if (userlogado.Length > 10) userlogado = userlogado.Substring(0, 20);

                    //Fecha Actual
                    int fechaActual = utiles.FechaToFormatoCG(DateTime.Now, true);

                    //Crear la sentencia para actualizar
                    query = "update " + GlobalVar.PrefijoTablaCG + "IVRSII ";
                    query += "set AVISR1 = 'N', WUSER1 = '" + userlogado + "', DUSER1 = " + fechaActual;
                    query += " where AVISR1 = 'S' ";

                    string queryFiltro = "";
                    for (int i = 0; i < aClaves.Count; i++)
                    {
                        claveActual = (IVRSIIKey)aClaves[i];
                        if (i != 0) queryFiltro += " or ";
                        queryFiltro += " (CSVER1 = '" + claveActual.CSVER1 + "' and NIFDR1 = '" + claveActual.NIFDR1 + "' and ";
                        queryFiltro += "DATER1 = " + claveActual.DATER1 + " and TIMER1 = " + claveActual.TIMER1 + " and ";
                        queryFiltro += "TDOCR1 = '" + claveActual.TDOCR1 + "' and COPSR1 = '" + claveActual.COPSR1 + "') ";
                    }
                    if (queryFiltro != "") query += " and (" + queryFiltro + ") ";
                }

                //Ejecutar la query de actualización
                if (query != "")
                {
                    int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Deshabilitar el botón de confirmar alertas
                    this.btnConfirmarAlerta.Enabled = false;

                    //Refrescar la Grid
                    this.ConsultaInformacionEnvios();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void toolStripButtonExportar_Click(object sender, EventArgs e)
        {
            this.GridExportarHTML();
        }

        private void frmSiiAlerta_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSiiAlerta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Alerta");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmSiiAlertaTitulo", "Alerta");
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Llama a la consulta correspondiente según el libro solicitado
        /// </summary>
        private void Consultar()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.tgGridResumen.Visible = false;
                this.tgGridFacturas.Visible = false;
                this.lblNoInfo.Visible = false;
                this.btnCerrarFacturas.Visible = false;
                this.chkMarcarDesmTodo.Visible = false;
                this.btnConfirmarAlerta.Visible = false;

                this.lblInfo.Visible = true;
                this.lblInfo.Update();
                    
                if (this.tgGridResumen.dsDatos.Tables.Count > 0 && this.tgGridResumen.dsDatos.Tables.Contains("Resumen")) this.tgGridResumen.dsDatos.Tables.Remove("Resumen");
                    
                if (this.tgGridFacturas.dsDatos.Tables.Count > 0 && this.tgGridFacturas.dsDatos.Tables.Contains("Facturas")) this.tgGridFacturas.dsDatos.Tables.Remove("Facturas");

                //Eliminar todas las tablas del dataset
                if (this.dsAlertaEnvio != null && this.dsAlertaEnvio.Tables != null && this.dsAlertaEnvio.Tables.Count > 0)
                {
                    this.dsAlertaEnvio.Tables.Clear();
                    this.dsAlertaEnvio.Clear();
                }

                //Consultar la información de los envios
                this.ConsultaInformacionEnvios();
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                string msgError = ex.Message;
                if (msgError == "Error consultando la información. Para más detalle consulte el fichero de Log.")
                {
                    this.lblNoInfo.Text = ex.Message;
                    this.lblNoInfo.Visible = true;
                }
            }
            Cursor.Current = Cursors.Default;

            this.lblInfo.Visible = false;
            this.lblInfo.Update();
        }

        private void BuiltgEnviosResumenInfo()
        {
            //Crear el DataGrid
            this.tgGridResumen.dsDatos = new DataSet();
            this.tgGridResumen.dsDatos.DataSetName = "Resumen";
            this.tgGridResumen.AddUltimaFilaSiNoHayDisponile = false;
            //this.tgGridResumen.ReadOnly = true;
            this.tgGridResumen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridResumen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridResumen.AllowUserToAddRows = false;
            this.tgGridResumen.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridResumen.NombreTabla = "Resumen";
        }

        private void BuiltgEnviosFacturasInfo()
        {
            //Crear el DataGrid
            this.tgGridFacturas.dsDatos = new DataSet();
            this.tgGridFacturas.dsDatos.DataSetName = "Facturas";
            this.tgGridFacturas.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridFacturas.ReadOnly = true;
            this.tgGridFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridFacturas.AllowUserToAddRows = false;
            this.tgGridFacturas.AllowUserToOrderColumns = false;
            //this.tgGridConsulta.AutoGenerateColumns = false;
            this.tgGridFacturas.NombreTabla = "Facturas";
        }

        private void ConsultaInformacionEnvios()
        {
            IDataReader dr = null;
            try
            {
                //Inicializar el DataTable de Resultado
                if (this.dsAlertaEnvio == null || this.dsAlertaEnvio.Tables == null || this.dsAlertaEnvio.Tables.Count == 0) this.dsAlertaEnvio = this.CrearDataSetResultadoConsultaAlertaEnvio();
                else
                {
                    if (this.dsAlertaEnvio.Tables.Contains("Resumen")) this.dsAlertaEnvio.Tables["Resumen"].Rows.Clear();
                    if (this.dsAlertaEnvio.Tables.Contains("Facturas")) this.dsAlertaEnvio.Tables["Facturas"].Rows.Clear();

                    if (this.tgGridResumen.dsDatos != null && this.dsAlertaEnvio.Tables.Contains("Resumen") && this.tgGridResumen.dsDatos.Tables.Contains("Resumen"))
                    {
                        this.tgGridResumen.dsDatos.Tables["Resumen"].Rows.Clear();
                        this.tgGridResumen.dsDatos.Tables.Remove("Resumen");
                    }
                    if (this.tgGridResumen.dsDatos != null && this.dsAlertaEnvio.Tables.Contains("Facturas") && this.tgGridFacturas.dsDatos.Tables.Contains("Facturas"))
                    {
                        this.tgGridFacturas.dsDatos.Tables["Facturas"].Rows.Clear();
                        this.tgGridFacturas.dsDatos.Tables.Remove("Facturas");
                    }
                }

                string sqlResumen = "select * from " +  GlobalVar.PrefijoTablaCG + "IVRSII ";
                sqlResumen += "where AVISR1 = 'S' ";
                sqlResumen += "order by NIFDR1, DATER1 DESC, TIMER1 DESC";

                DataRow row;
                string fechaEnvio = "";
                string fechaEnvioFormatoSII = "";
                string horaEnvio = "";
                string horaEnvioFormatoSII = "";
                string libroCod = "";
                string libroDesc = "";
                string operacionCod = "";
                string operacionDesc = "";

                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(sqlResumen, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.dsAlertaEnvio.Tables["Resumen"].NewRow();

                    row["CSV"] = dr.GetValue(dr.GetOrdinal("CSVER1")).ToString().Trim();
                    row["NIF"] = dr.GetValue(dr.GetOrdinal("NIFDR1")).ToString().Trim();
                                        
                    fechaEnvio = dr.GetValue(dr.GetOrdinal("DATER1")).ToString().Trim();
                    fechaEnvioFormatoSII = utiles.FechaToFormatoCG(fechaEnvio).ToShortDateString();
                    row["Fecha"] = fechaEnvioFormatoSII;
                    row["DATER1"] = fechaEnvio;

                    horaEnvio = dr.GetValue(dr.GetOrdinal("TIMER1")).ToString().Trim();
                    row["TIMER1"] = horaEnvio;
                    if (horaEnvio != "0")
                    {
                        if (horaEnvio.Length < 6) horaEnvio = horaEnvio.PadLeft(6, '0'); 
                        horaEnvioFormatoSII = horaEnvio.Substring(0, 2) + ":" + horaEnvio.Substring(2, 2) + ":" + horaEnvio.Substring(4, 2);
                    }
                    else horaEnvioFormatoSII = "";
                    row["Hora"] = horaEnvioFormatoSII;
                    
                    libroCod = dr.GetValue(dr.GetOrdinal("TDOCR1")).ToString().Trim();
                    row["TDOCR1"] = libroCod;
                    libroDesc = this.ObtenerDescripcionLibro(libroCod);
                    row["Libro"] = libroDesc;
                    
                    operacionCod = dr.GetValue(dr.GetOrdinal("COPSR1")).ToString().Trim();
                    row["COPSR1"] = operacionCod; 
                    operacionDesc = this.ObtenerDescripcionOperacion(operacionCod);
                    row["Operacion"] = operacionDesc;

                    row["TotalFacturas"] = dr.GetValue(dr.GetOrdinal("TOTRR1")).ToString().Trim();
                    row["TotalFacturasCorrectas"] = dr.GetValue(dr.GetOrdinal("TOTVR1")).ToString().Trim();
                    row["TotalFacturasAcepErrores"] = dr.GetValue(dr.GetOrdinal("TOTWR1")).ToString().Trim();
                    row["TotalFacturasErroneas"] = dr.GetValue(dr.GetOrdinal("TOTER1")).ToString().Trim();
                    row["TotalFacturasNoEnviadas"] = dr.GetValue(dr.GetOrdinal("TOTNR1")).ToString().Trim();

                    //row["Aviso"] = dr.GetValue(dr.GetOrdinal("AVISR1")).ToString().Trim();

                    this.dsAlertaEnvio.Tables["Resumen"].Rows.Add(row);
                }

                dr.Close();

                if (this.dsAlertaEnvio.Tables.Count > 0)
                {
                    if (this.dsAlertaEnvio.Tables["Resumen"].Rows.Count > 0)
                    {
                        //Existen resumenes de envios
                        
                        //Adicionar el DataTable Resumen al DataSet del DataGrid
                        this.tgGridResumen.dsDatos.Tables.Add(this.dsAlertaEnvio.Tables["Resumen"].Copy());

                        this.tgGridResumen.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridResumen.DataSource = this.tgGridResumen.dsDatos.Tables["Resumen"];
                        
                        //Cambiar los headers de las columnas del DataGrid de Detalles de IVA
                        this.CambiarColumnasEncabezadosResumen();

                        this.tgGridResumen.Refresh();
                        this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 608);

                        this.tgGridResumen.Visible = true;
                        this.lblNoInfo.Visible = false;
                        //this.toolStripButtonExportar.Enabled = true;
                        this.enviosExpanded = true;

                        this.chkMarcarDesmTodo.Visible = true;
                        this.btnConfirmarAlerta.Visible = true;
                        this.btnConfirmarAlerta.Enabled = false;
                    }
                    else
                    {
                        this.tgGridResumen.Visible = false;
                        this.enviosExpanded = false;
                        this.lblNoInfo.Text = "No existen alertas pendientes"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.btnConfirmarAlerta.Visible = false;
                        this.chkMarcarDesmTodo.Visible = false;
                        this.tgGridFacturas.Visible = false;
                        this.btnCerrarFacturas.Visible = false;
                    }
                }
                else
                {
                    this.tgGridResumen.Visible = false;
                    this.lblNoInfo.Text = "No existen alertas pendientes"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                    this.tgGridFacturas.Visible = false;
                    this.btnCerrarFacturas.Visible = false;
                    this.btnConfirmarAlerta.Visible = false;
                    this.chkMarcarDesmTodo.Visible = false;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                throw new Exception("Error consultando la información. Para más detalle consulte el fichero de Log.");
            }
        }

        /// <summary>
        /// Crear los Datatables Resumen y Facturas que almacenan la información de los envios
        /// </summary>
        /// <returns></returns>
        private DataSet CrearDataSetResultadoConsultaAlertaEnvio()
        {
            this.dsAlertaEnvio = new DataSet();
            try
            {
                DataTable dtResumen = new DataTable();
                dtResumen.TableName = "Resumen";

                //Columna CheckBox
                dtResumen.Columns.Add("Selected", typeof(bool));
                dtResumen.Columns.Add("CSV", typeof(string));
                dtResumen.Columns.Add("NIF", typeof(string));
                dtResumen.Columns.Add("Fecha", typeof(string));
                dtResumen.Columns.Add("Hora", typeof(string));
                dtResumen.Columns.Add("Libro", typeof(string));
                dtResumen.Columns.Add("Operacion", typeof(string));
                dtResumen.Columns.Add("TotalFacturas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasCorrectas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasAcepErrores", typeof(string));
                dtResumen.Columns.Add("TotalFacturasErroneas", typeof(string));
                dtResumen.Columns.Add("TotalFacturasNoEnviadas", typeof(string));
                dtResumen.Columns.Add("DATER1", typeof(string));
                dtResumen.Columns.Add("TIMER1", typeof(string));
                dtResumen.Columns.Add("TDOCR1", typeof(string));
                dtResumen.Columns.Add("COPSR1", typeof(string));
                //dtResumen.Columns.Add("Aviso", typeof(string));
                
                this.dsAlertaEnvio.Tables.Add(dtResumen);

                DataTable dtFacturas = new DataTable();
                dtFacturas.TableName = "Facturas";
                dtFacturas.Columns.Add("NumSerieFactura", typeof(string));
                dtFacturas.Columns.Add("FechaExpedicionFactura", typeof(string));
                dtFacturas.Columns.Add("IDEmisorFactura", typeof(string));
                dtFacturas.Columns.Add("IdNif", typeof(string));
                dtFacturas.Columns.Add("IdOtroPais", typeof(string));
                dtFacturas.Columns.Add("IdOtroTipo", typeof(string));
                dtFacturas.Columns.Add("IdOtroId", typeof(string));
                dtFacturas.Columns.Add("ClaveOperacion", typeof(string));
                dtFacturas.Columns.Add("CargoAbono", typeof(string));
                dtFacturas.Columns.Add("EstadoFactura", typeof(string));
                dtFacturas.Columns.Add("CodigoErrorRegistro", typeof(string));
                dtFacturas.Columns.Add("DescripcionErrorRegistro", typeof(string));
                //dtFacturas.Columns.Add("HORA", typeof(string));
                this.dsAlertaEnvio.Tables.Add(dtFacturas);
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            return (this.dsAlertaEnvio);
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Resumen
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosResumen()
        {
            try
            {
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("Selected")) this.tgGridResumen.CambiarColumnHeader("Selected", "Seleccionar");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("Operacion")) this.tgGridResumen.CambiarColumnHeader("Operacion", "Operación");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturas", "Total Facturas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasCorrectas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasCorrectas", "Total Fact. Correctas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasAcepErrores")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasAcepErrores", "Total Fact. Aceptadas con errores");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasErroneas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasErroneas", "Total Fac. incorrectas");  //Falta traducir
                if (this.tgGridResumen.dsDatos.Tables[0].Columns.Contains("TotalFacturasNoEnviadas")) this.tgGridResumen.CambiarColumnHeader("TotalFacturasNoEnviadas", "Total Fact. no enviadas");  //Falta traducir

                //Poner las columnas ReadOnly, todas menos la primera
                for (int i = 0; i < this.tgGridResumen.Columns.Count; i++)
                {
                    if (this.tgGridResumen.Columns[i].Name == "Selected")
                    {
                        this.tgGridResumen.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        this.tgGridResumen.Columns[i].ReadOnly = true;
                    }
                }
               
                /*if (this.tgGridResumen.Columns.Contains("CSV")) this.tgGridResumen.Columns["CSV"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("NifPresentador")) this.tgGridResumen.Columns["NifPresentador"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("FechaPresentacion")) this.tgGridResumen.Columns["FechaPresentacion"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("HoraPresentacion")) this.tgGridResumen.Columns["HoraPresentacion"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("LibroCodigo")) this.tgGridResumen.Columns["LibroCodigo"].Visible = false;
                if (this.tgGridResumen.Columns.Contains("OperacionCodigo")) this.tgGridResumen.Columns["OperacionCodigo"].Visible = false;*/
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cambiar los headers de las columnas del DataGrid de Facturas
        /// </summary>
        /// <param name="grid"></param>
        private void CambiarColumnasEncabezadosFacturas()
        {
            try
            {
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("NumSerieFactura")) this.tgGridFacturas.CambiarColumnHeader("NumSerieFactura", "No. Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("FechaExpedicionFactura")) this.tgGridFacturas.CambiarColumnHeader("FechaExpedicionFactura", "Fecha Expedición Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("IDEmisorFactura")) this.tgGridFacturas.CambiarColumnHeader("IDEmisorFactura", "NIF Emisor Factura");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("ClaveOperacion")) this.tgGridFacturas.CambiarColumnHeader("ClaveOperacion", "Clave Operación");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("CargoAbono")) this.tgGridFacturas.CambiarColumnHeader("CargoAbono", "Cargo/Abono");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("EstadoFactura")) this.tgGridFacturas.CambiarColumnHeader("EstadoFactura", "Estado");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("CodigoErrorRegistro")) this.tgGridFacturas.CambiarColumnHeader("CodigoErrorRegistro", "Código Error");  //Falta traducir
                if (this.tgGridFacturas.dsDatos.Tables[0].Columns.Contains("DescripcionErrorRegistro")) this.tgGridFacturas.CambiarColumnHeader("DescripcionErrorRegistro", "Descripción Error");  //Falta traducir
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Obtiene la facturas del envio solicitado
        /// </summary>
        /// <param name="cvs"></param>
        /// <param name="nifCia"></param>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="libro"></param>
        /// <param name="operacion"></param>
        private void ConsultaInformacionFacturasEnvio(string cvs, string nifCia, string fecha, string hora, string libro, string operacion)
        {
            IDataReader dr = null;
            try
            {
                string sqlFacturas = "select * from " + GlobalVar.PrefijoTablaCG + "IVLSII ";
                sqlFacturas += "where CSVEL1 = '" + cvs + "' and ";
                sqlFacturas += "NIFDL1 = '" + nifCia + "' and ";
                sqlFacturas += "DATEL1 = '" + fecha + "' and ";
                sqlFacturas += "TIMEL1 = '" + hora + "' and ";
                sqlFacturas += "TDOCL1 = '" + libro + "' and ";
                sqlFacturas += "COPSL1 = '" + operacion + "' ";
                sqlFacturas += "order by NSFEL1";

                DataRow row;

                string fechaExpedicionFactura = "";
                string fechaExpedicionFacturaSII = "";

                string iDEmisorFacturaNIF = "";
                string idOtroCodPais = " ";
                string idOtroIdType = " ";
                string idOtroId = " ";
                
                //Ejecutar Consulta
                dr = GlobalVar.ConexionCG.ExecuteReader(sqlFacturas, GlobalVar.ConexionCG.GetConnectionValue);

                while (dr.Read())
                {
                    row = this.dsAlertaEnvio.Tables["Facturas"].NewRow();

                    row["NumSerieFactura"] = dr.GetValue(dr.GetOrdinal("NSFEL1")).ToString().Trim();

                    fechaExpedicionFactura = dr.GetValue(dr.GetOrdinal("FDOCL1")).ToString().Trim();
                    fechaExpedicionFacturaSII = utiles.FechaToFormatoCG(fechaExpedicionFactura).ToShortDateString();
                    row["FechaExpedicionFactura"] = fechaExpedicionFacturaSII;

                    iDEmisorFacturaNIF = dr.GetValue(dr.GetOrdinal("NIFEL1")).ToString().Trim();

                    if (iDEmisorFacturaNIF != "")
                    {
                        row["IdNif"] = iDEmisorFacturaNIF;
                        row["IdOtroPais"] = "";
                        row["IdOtroTipo"] = "";
                        row["IdOtroId"] = "";

                        idOtroCodPais = " ";
                        idOtroIdType = " ";
                        idOtroId = " ";
                    }
                    else
                    {
                        row["IdNif"] = "";

                        idOtroCodPais = dr.GetValue(dr.GetOrdinal("PAISL1")).ToString().Trim();
                        if (idOtroCodPais != "")
                        {
                            row["IdOtroPais"] = idOtroCodPais;
                            iDEmisorFacturaNIF += idOtroCodPais + "-";
                        }
                        row["IdOtroPais"] = idOtroCodPais;

                        idOtroIdType = dr.GetValue(dr.GetOrdinal("TIDEL1")).ToString().Trim();
                        row["IdOtroTipo"] = idOtroIdType;
                        idOtroId = dr.GetValue(dr.GetOrdinal("IDOEL1")).ToString().Trim();
                        row["IdOtroId"] = idOtroId;
                        iDEmisorFacturaNIF += idOtroIdType + "-" + idOtroId;
                    }

                    row["IDEmisorFactura"] = iDEmisorFacturaNIF;
                    row["ClaveOperacion"] = dr.GetValue(dr.GetOrdinal("CLOSL1")).ToString().Trim();
                    row["CargoAbono"] = dr.GetValue(dr.GetOrdinal("TPCGL1")).ToString().Trim();
                    row["EstadoFactura"] = dr.GetValue(dr.GetOrdinal("SFACL1")).ToString().Trim();
                    row["CodigoErrorRegistro"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString().Trim();
                    row["DescripcionErrorRegistro"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString().Trim();
                    //row["HORA"] = dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString().Trim();

                    this.dsAlertaEnvio.Tables["Facturas"].Rows.Add(row);
                }

                dr.Close();

                if (this.dsAlertaEnvio.Tables.Count > 0)
                {
                    if (this.dsAlertaEnvio.Tables["Facturas"].Rows.Count > 0)
                    {
                        //Existen resumenes de envios

                        //Adicionar el DataTable Resumen al DataSet del DataGrid
                        this.tgGridFacturas.dsDatos.Tables.Add(this.dsAlertaEnvio.Tables["Facturas"].Copy());

                        this.tgGridFacturas.AutoGenerateColumns = true;

                        //Poner como DataSource del DataGrid el DataTable creado

                        //Poner como DataSource del DataGrid el DataTable creado
                        this.tgGridFacturas.DataSource = this.tgGridFacturas.dsDatos.Tables["Facturas"];

                        //Cambiar los headers de las columnas del DataGrid de Facturas
                        this.CambiarColumnasEncabezadosFacturas();

                        this.tgGridFacturas.Refresh();
                        //this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 608);

                        this.btnCerrarFacturas.Visible = true;
                        this.tgGridFacturas.Visible = true;
                        this.lblNoInfo.Visible = false;
                        this.toolStripButtonExportar.Enabled = true;
                        this.enviosExpanded = false;
                    }
                    else
                    {
                        //No existen facturas que cumplen el criterio de seleccion
                        this.btnCerrarFacturas.Visible = false;
                        this.tgGridFacturas.Visible = false;
                        this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                        this.lblNoInfo.Visible = true;
                        this.toolStripButtonExportar.Enabled = false;
                        this.enviosExpanded = true;
                    }
                }
                else
                {
                    //El webservice no pudo consultar las facturas
                    this.tgGridResumen.Visible = false;
                    this.lblNoInfo.Text = "No existen envíos que cumplan el criterio seleccionado"; //Falta traducir
                    this.lblNoInfo.Visible = true;
                    this.toolStripButtonExportar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Buscar las facturas del envio seleccionado
        /// </summary>
        private void BuscarFacturasEnviadas()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Grid Resumen modificar tamaño (más pequeño)
                this.tgGridResumen.Size = new Size(this.tgGridResumen.Width, 265);

                //if (!this.btnCerrarFacturas.Visible)
                if (this.enviosExpanded)
                {
                    //Posicionar el envío seleccionado al principio de la Grid
                    int rowIndexResumen = this.tgGridResumen.CurrentRow.Index;
                    this.tgGridResumen.FirstDisplayedScrollingRowIndex = rowIndexResumen;
                }

                //Eliminar la tabla de Facturas del dataset de la Grid de Facturas
                if (this.tgGridFacturas.dsDatos.Tables.Count > 0 && this.tgGridFacturas.dsDatos.Tables.Contains("Facturas")) this.tgGridFacturas.dsDatos.Tables.Remove("Facturas");

                //Eliminar todas la tablas Facturas del dataset
                if (this.dsAlertaEnvio != null && this.dsAlertaEnvio.Tables != null && this.dsAlertaEnvio.Tables.Count > 0 && this.dsAlertaEnvio.Tables.Contains("Facturas")) this.dsAlertaEnvio.Tables["Facturas"].Rows.Clear();

                string csv = this.tgGridResumen.SelectedRows[0].Cells["CSV"].Value.ToString();
                string nifCia = this.tgGridResumen.SelectedRows[0].Cells["NIF"].Value.ToString();
                string fecha = this.tgGridResumen.SelectedRows[0].Cells["DATER1"].Value.ToString();
                string hora = this.tgGridResumen.SelectedRows[0].Cells["TIMER1"].Value.ToString();
                string libro = this.tgGridResumen.SelectedRows[0].Cells["TDOCR1"].Value.ToString();
                string operacion = this.tgGridResumen.SelectedRows[0].Cells["COPSR1"].Value.ToString();

                //Buscar las facturas del envío
                this.ConsultaInformacionFacturasEnvio(csv, nifCia, fecha, hora, libro, operacion);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Exporta la consulta de Datos en Local a Excel, pasando por un fichero HTML
        /// </summary>
        private void GridExportarHTML()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string titulo = "Lista de Facturas Enviadas";

                try
                {
                    if (this.tgGridResumen.SelectedRows != null)
                    {
                        //Mostrar los datos del envío en el títlulo de la exportación (libro operacion fechaEnvio horaEnvio CSV)
                        string csv = this.tgGridResumen.SelectedRows[0].Cells["CSV"].Value.ToString();
                        string nifCia = this.tgGridResumen.SelectedRows[0].Cells["NIF"].Value.ToString();
                        string fecha = this.tgGridResumen.SelectedRows[0].Cells["DATER1"].Value.ToString();
                        string fechaEnvioFormatoSII = utiles.FechaToFormatoCG(fecha).ToShortDateString();
                        string hora = this.tgGridResumen.SelectedRows[0].Cells["TIMER1"].Value.ToString();
                        string horaEnvioFormatoSII = hora.Trim();
                        if (horaEnvioFormatoSII != "0" && horaEnvioFormatoSII != "")
                        {
                            if (hora.Length < 6) hora = hora.PadLeft(6, '0');
                            horaEnvioFormatoSII = hora.Substring(0, 2) + ":" + hora.Substring(2, 2) + ":" + hora.Substring(4, 2);
                        }
                        else horaEnvioFormatoSII = "";
                        string libro = this.tgGridResumen.SelectedRows[0].Cells["TDOCR1"].Value.ToString();
                        string libroDesc = this.ObtenerDescripcionLibro(libro);
                        string operacion = this.tgGridResumen.SelectedRows[0].Cells["COPSR1"].Value.ToString();
                        string operacionDesc = this.ObtenerDescripcionOperacion(operacion);

                        titulo += " - " + libroDesc + " " + operacionDesc + " " + fechaEnvioFormatoSII + " " + horaEnvioFormatoSII + " " + csv;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Columnas
                ArrayList descColumnas = new ArrayList();
                string[] nombreTipoVisible;
                for (int i = 0; i < this.tgGridFacturas.ColumnCount; i++)
                {
                    nombreTipoVisible = new string[3];
                    nombreTipoVisible[0] = this.tgGridFacturas.Columns[i].HeaderText;  //Nombre de la columna

                    switch (this.tgGridFacturas.Columns[i].Name)
                    {
                        case "CodigoErrorRegistro":
                            nombreTipoVisible[1] = "numero";
                            nombreTipoVisible[2] = this.tgGridFacturas.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                        default:
                            nombreTipoVisible[1] = "string";
                            nombreTipoVisible[2] = this.tgGridFacturas.Columns[i].Visible ? "1" : "0";     //1-> Visible   0 -> No Visible
                            break;
                    }

                    descColumnas.Add(nombreTipoVisible);
                }

                StringBuilder documento_HTML = new StringBuilder();
                LibroUtiles.HTMLCrear(ref documento_HTML);

                LibroUtiles.HTMLEncabezado(ref documento_HTML, descColumnas, titulo);

                LibroUtiles.HTMLDatos(ref documento_HTML, descColumnas, ref this.tgGridFacturas);

                LibroUtiles.HTMLFin(ref documento_HTML);

                string ficheroHTML = LibroUtiles.ConsultaNombreFichero("SIIAlertaEnvio");

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
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(this.LP.GetText("errExportExcel", "Error exportando fichero excel") + " (" + ex.Message + ")", this.LP.GetText("errValTitulo", "Error"));
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion
    }

    /// <summary>
    /// Clase que describe la llave de un envío
    /// </summary>
    public class IVRSIIKey
    {
        public string CSVER1;
        public string NIFDR1;
        public string DATER1;
        public string TIMER1;
        public string TDOCR1;
        public string COPSR1;

        public IVRSIIKey()
        {
            this.CSVER1 = "";
            this.NIFDR1 = "";
            this.DATER1 = "";
            this.TIMER1 = "";
            this.TDOCR1 = "";
            this.COPSR1 = "";
        }
    }
}
