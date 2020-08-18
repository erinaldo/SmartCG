using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using ObjectModel;

namespace ModSII
{
    public partial class frmConfig : frmPlantilla, IReLocalizable
    {
        public frmConfig()
        {
            InitializeComponent();
        }
        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO SII Config");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Crear el TGGrid
            this.BuiltgConfig();

            this.FillDataGrid();
        }

        private void toolStripButtonGrabar_Click(object sender, EventArgs e)
        {
            this.Grabar();
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConfig_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) toolStripButtonSalir_Click(sender, null);
        }

        private void frmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN SII Config");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmConfig", "Configuración");  //Falta traducir
        }

        /// <summary>
        /// Graba la nueva dirección del servicio web
        /// </summary>
        private void Grabar()
        {
            try
            {
                this.tgGridConfig.Refresh();

                int agenciaEntornoActivo = -1;
                int contadorActivo = 0;

                for (int i = 0; i < this.tgGridConfig.Rows.Count; i++ )
                {
                    if (Convert.ToBoolean(this.tgGridConfig.Rows[i].Cells["Activo"].Value) == true)
                    {
                        agenciaEntornoActivo = i;
                        contadorActivo++;
                    }
                }

                if (contadorActivo == 0)
                {
                    MessageBox.Show("Debe seleccionar un tipo de agencia y entorno activo."); //Falta traducir
                    return;
                }

                /*if (contadorActivo > 1)
                {
                    MessageBox.Show("Debe seleccionar un solo tipo de agencia y entorno activo."); //Falta traducir
                    return;
                }*/

                //Actualizar la agencia y el entorno en el fichero de config
                string agenciaSeleccionada = this.tgGridConfig.Rows[agenciaEntornoActivo].Cells["AGENAG"].Value.ToString();
                string entornoSeleccionado = this.tgGridConfig.Rows[agenciaEntornoActivo].Cells["TIPOAG"].Value.ToString();

                /*
                //Inicializar agencia y entorno, Si no existen las propiedades se crearán
                var agenciaValue = ConfigurationManager.AppSettings["tipoAgencia"];
                if (string.IsNullOrEmpty(agenciaValue)) utiles.CrearappSettings("tipoAgencia", agenciaSeleccionada);
                else utiles.ModificarappSettings("tipoAgencia", agenciaSeleccionada);

                var entornoValue = ConfigurationManager.AppSettings["entorno"];
                if (string.IsNullOrEmpty(entornoValue)) utiles.CrearappSettings("entorno", entornoSeleccionado);
                else utiles.ModificarappSettings("entorno", entornoSeleccionado);
                */

                this.agencia = agenciaSeleccionada;
                this.entorno = entornoSeleccionado;

                GlobalVar.EntornoActivo.SiiAgencia = this.agencia;
                GlobalVar.EntornoActivo.SiiEntorno = this.entorno;
                //Dudas - hay que grabar el cambio en el fichero de entornos ??? !!!!!!!!!! o 
                //para que se hagan efectivos para siempre hay que hacerlo desde la gesyion del entorno ????

                string urlServicioWebSIISeleccionado = this.tgGridConfig.Rows[agenciaEntornoActivo].Cells["URENAG"].Value.ToString();
                this.serviceSII.ServicioWebSIICambiarURL(this.agencia, this.entorno, urlServicioWebSIISeleccionado);

                MessageBox.Show("Se ha seleccionado la agencia y el entorno indicado");    //Falta traducir
                //this.Close();
            }
            catch (Exception ex) 
            { 
                Log.Error(Utiles.CreateExceptionString(ex));

                MessageBox.Show(ex.Message, "Error");   //Falta traducir
            }
        }

        /// <summary>
        /// Verifica que la url sel servicio web exista
        /// </summary>
        /// <param name="uriInput"></param>
        /// <returns></returns>
        private bool IsReachableUri(string uriInput)
        {
            // Variable to Return
            bool testStatus = false;
            try
            {
                // Create a request for the URL.
                System.Net.WebRequest request = WebRequest.Create(uriInput);
                request.Timeout = 15000; // 15 Sec

                WebResponse response;
                try
                {
                    response = request.GetResponse();
                    testStatus = true; // Uri does exist                 
                    response.Close();
                }
                catch
                {
                    testStatus = false; // Uri does not exist
                }

                return (testStatus);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (testStatus);
        }

        private void BuiltgConfig()
        {
            //Crear el DataGrid
            this.tgGridConfig.dsDatos = new DataSet();
            this.tgGridConfig.dsDatos.DataSetName = "Config";
            this.tgGridConfig.AddUltimaFilaSiNoHayDisponile = false;
            this.tgGridConfig.ReadOnly = false;
            this.tgGridConfig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //this.dgInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tgGridConfig.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.tgGridConfig.AllowUserToAddRows = false;
            this.tgGridConfig.AllowUserToOrderColumns = false;
            this.tgGridConfig.AutoGenerateColumns = true;
            this.tgGridConfig.NombreTabla = "Config";
            
            DataTable dt = new DataTable();
            dt.TableName = "Tabla";

            //Adicionar las columnas al DataTable
            dt.Columns.Add("Activo", typeof(bool));
            dt.Columns.Add("AGEDES", typeof(string));
            dt.Columns.Add("ENTDES", typeof(string));
            dt.Columns.Add("NOMBAG", typeof(string));
            dt.Columns.Add("URENAG", typeof(string));
            dt.Columns.Add("AGENAG", typeof(string));
            dt.Columns.Add("TIPOAG", typeof(string));
            
            //Adicionar el DataTable al DataSet del DataGrid
            this.tgGridConfig.dsDatos.Tables.Add(dt);

            //Poner como DataSource del DataGrid el DataTable creado
            this.tgGridConfig.DataSource = this.tgGridConfig.dsDatos.Tables["Tabla"];
        }

        /// <summary>
        /// Cargar los datos de la Grid
        /// </summary>
        private void FillDataGrid()
        {
            IDataReader dr = null;
            try
            {
                string query = "select AGENAG, TIPOAG, NOMBAG, URENAG from " + GlobalVar.PrefijoTablaCG + "IVSAGE ";
                query += "where STATAG='V'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                DataRow row;
                int cont = 0;
                string agenciaActual = "";
                string entornoActual = "";
                int indiceEntornoActual = 0;

                while (dr.Read())
                {
                    row = this.tgGridConfig.dsDatos.Tables["Tabla"].NewRow();
                    
                    agenciaActual = dr.GetValue(dr.GetOrdinal("AGENAG")).ToString();
                    entornoActual = dr.GetValue(dr.GetOrdinal("TIPOAG")).ToString();
                    row["AGEDES"] = this.ObtenerDescripcionAgencia(agenciaActual);
                    row["ENTDES"] = this.ObtenerDescripcionEntorno(entornoActual);
                    row["NOMBAG"] = dr.GetValue(dr.GetOrdinal("NOMBAG")).ToString();
                    row["URENAG"] = dr.GetValue(dr.GetOrdinal("URENAG")).ToString();
                    row["AGENAG"] = agenciaActual;
                    row["TIPOAG"] = entornoActual;

                    if (this.agencia == agenciaActual && this.entorno == entornoActual)
                    {
                        row["Activo"] = true;
                        indiceEntornoActual = cont;
                    }
                    else row["Activo"] = false;

                    this.tgGridConfig.dsDatos.Tables["Tabla"].Rows.Add(row);

                    cont++;
                }

                if (cont > 0)
                {
                    this.lblResult.Visible = false;

                    //Ningún registro seleccionado
                    //this.tgGridConfig.ClearSelection();

                    //Seleccionar la fila del entorno activo
                    this.tgGridConfig.ClearSelection();
                    this.tgGridConfig.CurrentCell = this.tgGridConfig.Rows[indiceEntornoActual].Cells[0];
                    this.tgGridConfig.Rows[indiceEntornoActual].Selected = true;

                    this.tgGridConfig.Refresh();
                    this.tgGridConfig.Visible = true;

                    //Ajustar todas las columnas de la Grid
                    this.AjustarColumnasGrid(ref this.tgGridConfig, -1);

                    //Cambiar el encabezado de las columnas y ocultar- las que no son necesarias
                    this.CambiarEncabezadoColumnas();
                }
                else
                {
                    this.lblResult.Text = "No existe agencia y tipo de entorno activo. Por favor contacte con el administrador del servicio web del SII.";
                    this.lblResult.Visible = true;
                    this.tgGridConfig.Visible = false;
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                this.lblResult.Text = "Se ha producido un error obteniendo las agencias y tipo de entornos activos. Por favor contacte con el administrador.";
                this.lblResult.Visible = true;
                this.tgGridConfig.Visible = false;
                this.toolStripButtonGrabar.Enabled = false;
            }
        }

        /// <summary>
        /// Cambiar el encabezado de las columnas y ocultar las que no son necesarias
        /// </summary>
        private void CambiarEncabezadoColumnas()
        {
            try
            {
                //Encabezado de las columnas
                if (this.tgGridConfig.dsDatos.Tables[0].Columns.Contains("Activo")) this.tgGridConfig.CambiarColumnHeader("Activo", "Activo");  //Falta traducir
                if (this.tgGridConfig.dsDatos.Tables[0].Columns.Contains("AGEDES")) this.tgGridConfig.CambiarColumnHeader("AGEDES", "Agencia");  //Falta traducir
                if (this.tgGridConfig.dsDatos.Tables[0].Columns.Contains("ENTDES")) this.tgGridConfig.CambiarColumnHeader("ENTDES", "Entorno");  //Falta traducir
                if (this.tgGridConfig.dsDatos.Tables[0].Columns.Contains("NOMBAG")) this.tgGridConfig.CambiarColumnHeader("NOMBAG", "Nombre");  //Falta traducir
                if (this.tgGridConfig.dsDatos.Tables[0].Columns.Contains("URENAG")) this.tgGridConfig.CambiarColumnHeader("URENAG", "URL Servicio Web");  //Falta traducir

                //Ocultar columnas
                if (this.tgGridConfig.dsDatos.Tables["Tabla"].Columns.Contains("AGENAG")) this.tgGridConfig.Columns["AGENAG"].Visible = false;
                if (this.tgGridConfig.dsDatos.Tables["Tabla"].Columns.Contains("TIPOAG")) this.tgGridConfig.Columns["TIPOAG"].Visible = false;

                //Columnas no editables, solo se permite editar la columna "Activo"
                foreach (DataGridViewColumn dc in this.tgGridConfig.Columns)
                {
                    if (dc.Index.Equals(0)) dc.ReadOnly = false;
                    else dc.ReadOnly = true;
                }

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Ajustar las columnas de un objeto DataGridView
        /// </summary>
        /// <param name="grid">Objeto DataGridView</param>
        /// <param name="indiceColumna">-1 todas las columnas; en caso contrario, la columna deseada</param>
        public void AjustarColumnasGrid(ref TGGrid grid, int indiceColumna)
        {
            if (indiceColumna != -1)
            {
                if (indiceColumna >= 0 && indiceColumna < grid.ColumnCount)
                {
                    //Ajustar la columna indicada (indiceColumna)
                    grid.AutoResizeColumn(indiceColumna);
                }
            }
            else
            {
                //Ajustar todas las columnas
                for (int i = 0; i < grid.ColumnCount; i++)
                {
                    grid.AutoResizeColumn(i);
                }
            }
        }
        #endregion

        private void tgGridConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex == this.tgGridConfig.Columns["Activo"].Index)
                {
                    //Parar de editar la celda
                    this.tgGridConfig.EndEdit();

                    //Permitir sólo una fila con la columna Defecto marcada
                    if ((bool)this.tgGridConfig.Rows[e.RowIndex].Cells["Activo"].Value)
                    {
                        for (int i = 0; i < this.tgGridConfig.Rows.Count; i++)
                        {
                            if (i != e.RowIndex)
                            {
                                if ((bool)this.tgGridConfig.Rows[i].Cells["Activo"].Value)
                                {
                                    this.tgGridConfig.Rows[i].Cells["Activo"].Value = false;
                                }
                            }
                        }
                    }

                    this.tgGridConfig.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
    }
}
