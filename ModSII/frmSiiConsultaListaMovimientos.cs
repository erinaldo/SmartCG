using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModSII
{
    public partial class frmSiiConsultaListaMovimientos : frmPlantilla, IReLocalizable
    {
        private string libroID;
        private FacturaIdentificador facturaID;
        private string iDEmisorFactura;

        private string ejercicio;
        private string periodo;

        #region Properties
        public string LibroID
        {
            get
            {
                return (this.libroID);
            }
            set
            {
                this.libroID = value;
            }
        }

        public  FacturaIdentificador FacturaID
        {
            get
            {
                return (this.facturaID);
            }
            set
            {
                this.facturaID = value;
            }
        }

        public string IDEmisorFactura
        {
            get
            {
                return (this.iDEmisorFactura);
            }
            set
            {
                this.iDEmisorFactura = value;
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
        #endregion


        public frmSiiConsultaListaMovimientos()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmListaMovimientos_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Lista de Movimientos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Escribe la información de la cabecera de la factura
            this.EscribirDatosCabecera();

            //Crear el TGGrid
            this.BuiltgGridLOG();

            //Escribir los movimientos de log
            this.FillDataGrid();

            this.ActiveControl = this.lblIdEmisorFactura;
        }

        private void frmListaMovimientos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListaMovimientos_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Lista de Movimientos");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmListaMovimientosTitulo", "Lista de Movimientos - ") + this.ObtenerDescripcionLibro(this.libroID);
            this.Text += this.FormTituloAgenciaEntorno();
        }

        /// <summary>
        /// Escribe los datos de la cabecera de la factura
        /// </summary>
        private void EscribirDatosCabecera()
        {
            try
            {
                this.txtNIFEmisor.Text = this.iDEmisorFactura;
                this.txtNoFact.Text = this.facturaID.NumeroSerie;
                this.txtFechaDoc.Text = this.facturaID.FechaDocumento;

                /*this.txtEjercicio.Text = this.ejercicio;
                this.txtPeriodo.Text = this.periodo;
                */
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// //Escribe la información de los movimientos (tabla de logs) de la factura
        /// </summary>
        private void FillDataGrid()
        {
            IDataReader dr = null;
            try
            {
                //Obtener la consulta
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "IVLSII  ";
                query += "where TDOCL1 = '" + this.libroID + "' ";

                if (facturaID.EmisorFacturaNIF != "") query += "and NIFEL1 = '" + facturaID.EmisorFacturaNIF + "' ";
                else
                {
                    if (facturaID.EmisorFacturaIdOtroCodPais != "") query += "and PAISL1 = '" + facturaID.EmisorFacturaIdOtroCodPais + "' ";
                    string emisorFacturaIdOtroIdType = facturaID.EmisorFacturaIdOtroIdType;
                    if (emisorFacturaIdOtroIdType == "")  emisorFacturaIdOtroIdType = " ";
                    query += "and TIDEL1 = '" + facturaID.EmisorFacturaIdOtroIdType + "' ";
                    string emisorFacturaIdOtroId = facturaID.EmisorFacturaIdOtroId;
                    if (emisorFacturaIdOtroId == "") emisorFacturaIdOtroId = " ";
                    query += "and IDOEL1 = '" + emisorFacturaIdOtroId + "' ";
                }

                if (facturaID.NumeroSerie != "") query += "and NSFEL1 ='" + facturaID.NumeroSerie + "' ";
                if (facturaID.FechaDocumento != "")
                {
                    int fechaCG = utiles.FechaToFormatoCG(utiles.FechaCadenaToDateTime(facturaID.FechaDocumento), true);
                    if (fechaCG != -1) query += "and FDOCL1 = " + fechaCG + " ";
                }

                if (facturaID.CargoAbono != "") query += "and TPCGL1 ='" + facturaID.CargoAbono + "' ";
                
                //query += "and NIFDL1 = '" + this.nifComania + "' ";

                query += "order by DATEL1 DESC, TIMEL1 DESC";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                DataRow row;
                int cont = 0;

                string fecha = "";
                string hora = "";
                string operacionCodigo = "";
                string operacionDesc = "";

                //string desodLibro = "";
                //string codOperacion = "";
                while (dr.Read())
                {
                    row = this.tgGridMovimientos.dsDatos.Tables["Tabla"].NewRow();

                    row["CSVEL1"] = dr.GetValue(dr.GetOrdinal("CSVEL1")).ToString();
                    row["NIFDL1"] = dr.GetValue(dr.GetOrdinal("NIFDL1")).ToString();
                    
                    fecha = dr.GetValue(dr.GetOrdinal("DATEL1")).ToString();
                    if (fecha != "0") fecha = utiles.FormatoCGToFecha(fecha).ToShortDateString();
                    else fecha = "";
                    row["DATEL1"] = fecha;

                    hora = dr.GetValue(dr.GetOrdinal("TIMEL1")).ToString();
                    if (hora != "0")
                    {
                        if (hora.Length < 6) hora = hora.PadLeft(6, '0'); 
                        hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2) + ":" + hora.Substring(4, 2);
                    }
                    else hora = "";
                    row["TIMEL1"] = hora;

                    operacionCodigo = dr.GetValue(dr.GetOrdinal("COPSL1")).ToString();
                    operacionDesc = this.ObtenerDescripcionOperacion(operacionCodigo);
                    row["COPSL1"] = operacionDesc;
                    
                    row["SFACL1"] = dr.GetValue(dr.GetOrdinal("SFACL1")).ToString();
                    row["ERROL1"] = dr.GetValue(dr.GetOrdinal("ERROL1")).ToString();
                    row["DERRL1"] = dr.GetValue(dr.GetOrdinal("DERRL1")).ToString();

                    this.tgGridMovimientos.dsDatos.Tables["Tabla"].Rows.Add(row);

                    cont++;
                }

                //this.gbResultados.Visible = true;

                if (cont > 0)
                {
                    this.lblNoInfo.Visible = false;

                    //Ningún registro seleccionado
                    this.tgGridMovimientos.ClearSelection();
                    this.tgGridMovimientos.Refresh();
                    this.tgGridMovimientos.Visible = true;
                }
                else
                {
                    this.lblNoInfo.Text = "No existen movimientos";
                    this.lblNoInfo.Visible = true;
                    this.tgGridMovimientos.Visible = false;
                }
            }
            catch (Exception ex) 
            {
                if (dr != null) dr.Close();

                GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); 
            }
        }

        /// <summary>
        /// Construir el control de la Grid que contiene el log de los mivimientos de la factura
        /// </summary>
        private void BuiltgGridLOG()
        {
            //Crear el DataGrid
            this.tgGridMovimientos.dsDatos = new DataSet();
            this.tgGridMovimientos.dsDatos.DataSetName = "Log";
            this.tgGridMovimientos.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridMovimientos.ReadOnly = true;
            this.tgGridMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridMovimientos.AllowUserToAddRows = false;
            this.tgGridMovimientos.AllowUserToOrderColumns = false;
            this.tgGridMovimientos.AutoGenerateColumns = false;
            this.tgGridMovimientos.NombreTabla = "Log";

            DataTable dt = new DataTable();
            dt.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            //dt.Columns.Add("CIAFS1", typeof(string));
            //dt.Columns.Add("EJERS1", typeof(string));
            //dt.Columns.Add("PERIS1", typeof(string));
            dt.Columns.Add("CSVEL1", typeof(string));
            dt.Columns.Add("NIFDL1", typeof(string));
            dt.Columns.Add("DATEL1", typeof(string));
            dt.Columns.Add("TIMEL1", typeof(string));
            dt.Columns.Add("COPSL1", typeof(string));
            dt.Columns.Add("SFACL1", typeof(string));
            dt.Columns.Add("ERROL1", typeof(string));
            dt.Columns.Add("DERRL1", typeof(string));

            //Crear la columnas del DataGrid
            this.tgGridMovimientos.AddTextBoxColumn(0, "CSVEL1", this.LP.GetText("dgHeaderCSV", "CSV"), 40, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(1, "NIFDL1", this.LP.GetText("dgHeaderNIFDeclarante", "NIF Declarante"), 80, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(2, "DATEL1", this.LP.GetText("dgHeaderFecha", "Fecha"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(3, "TIMEL1", this.LP.GetText("dgHeaderHora", "Hora"), 50, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(4, "COPSL1", this.LP.GetText("dgHeaderOperacion", "Operación"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(5, "SFACL1", this.LP.GetText("dgHeaderEstado", "Estado"), 60, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(6, "ERROL1", this.LP.GetText("dgHeaderCodError", "Código Error"), 20, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir
            this.tgGridMovimientos.AddTextBoxColumn(7, "DERRL1", this.LP.GetText("dgHeaderDescError", "Descripción Error"), 200, 50, typeof(string), DataGridViewContentAlignment.MiddleLeft, true);     //Falta traducir

            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridMovimientos.dsDatos.Tables.Add(dt);

            //Poner como DataSource del DataGrid el DataTable tgGridMovimientos
            this.tgGridMovimientos.DataSource = this.tgGridMovimientos.dsDatos.Tables["Tabla"];
        }
        #endregion
    }
}
