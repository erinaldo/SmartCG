using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Management;
using ObjectModel;

namespace AppAdminModulos
{
    public partial class frmModuloGenerarFichero : frmPlantilla
    {
        private DataSet dsModulosAGenerar = null;
        private DataTable tabla_Modulos;
        
        public frmModuloGenerarFichero()
        {
            InitializeComponent();
        }


        #region Eventos
        private void frmModuloGenerarFichero_Load(object sender, EventArgs e)
        {
            //string idDisk = serialIDfromDisk("C");
            //Construir DataSet partiendo del DataSet de módulos
            this.FilldsModulosAGenerar();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.FormValid())
                {
                    this.lblResult.Text = "Debe seleccionar al menos un módulo para generar el fichero.";
                    this.lblResult.Visible = true;
                    return;
                }
                else
                {
                    //Clonar el DataSet
                    DataSet dsParaCopiarFichero = this.dsModulosAGenerar.Copy();

                    //Eliminar la columna seleccionar del nuevo dataset
                    dsParaCopiarFichero.Tables["Modulo"].Columns.Remove("seleccionar");
                    //dt.Columns.RemoveAt(5);

                    //Generar fichero  xml
                    //dsParaCopiarFichero.WriteXml(@"C:\VS2010_Projects\FinanzasNet\AppAdminModulos\ModulosCliente.xml");
                    string nombreFichero = ConfigurationManager.AppSettings["FicheroModulosGenerado"];
                    if (nombreFichero == null || nombreFichero == "") nombreFichero = "Modulos.xml";
                    string path = Application.StartupPath + "\\" + nombreFichero;
                    //string path = @"C:\VS2010_Projects\Finanzas1Net\AppAdminModulos\ModulosCliente.xml";

                    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                    dsParaCopiarFichero.WriteXml(writer, XmlWriteMode.IgnoreSchema);

                    dsParaCopiarFichero.Clear();
                    dsParaCopiarFichero.Dispose();

                    writer.Close();

                    //this.lblResult.Text = "";
                    this.lblResult.Visible = true;
                    if (chkEncriptar.Checked)
                    {
                        //Encriptar el fichero
                        string key = ConfigurationManager.AppSettings["ClaveEncriptar"];
                        //if (key == "") key = "btgsa!admin";
                        string result = CryptoManager.Encrypt(path, key);
						
                        //Abrir el fichero y reemplazar el contenido por el encriptado
                        System.IO.File.WriteAllText(path, result);

                        this.btnViewFichero.Visible = false;
                    }
                    else
                    {
                        this.btnViewFichero.Visible = true;
                    }

                    //Guardar la última petición de módulos seleccionados para generar el fichero
                    string modulosSeleccionados = "";
                    for (int i = 0; i < this.dataGridViewModulos.Rows.Count; i++)
                    {
                        if ((bool)this.dataGridViewModulos.Rows[i].Cells["seleccionar"].Value)
                        {
                            modulosSeleccionados += this.dataGridViewModulos.Rows[i].Cells["id"].Value.ToString() + ",";
                        }
                    }
                    if (modulosSeleccionados.Length > 0)
                    {
                        //Eliminar la coma del final
                        if (modulosSeleccionados.Substring(modulosSeleccionados.Length - 1, 1) == ",") modulosSeleccionados = modulosSeleccionados.Substring(0, modulosSeleccionados.Length - 1);

                        //Actualizar la variable UltimoFicheroGeneradoModulosSel del fichero app.config
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["UltimoFicheroGeneradoModulosSel"].Value = modulosSeleccionados;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }

                    this.lblResult.Text = "El fichero se generó correctamente.";
                }

            }
            catch (Exception ex)
            {
                this.lblResult.Text = "El fichero no se pudo generar (" + ex.Message + ")";

                this.btnViewFichero.Visible = false;
            }

            this.lblResult.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.dsModulosAGenerar != null)
            {
                this.dsModulosAGenerar.Clear();
                this.dsModulosAGenerar.Dispose();
            }
            this.Close();
        }

        private void btnEncriptado_Click(object sender, EventArgs e)
        {
            string nombreFichero = ConfigurationManager.AppSettings["FicheroModulosGenerado"];
            if (nombreFichero == null || nombreFichero == "") nombreFichero = "ModulosCliente.xml";
            string pathFichero = Application.StartupPath + "\\" + nombreFichero;
            try
            {
                System.Diagnostics.Process.Start(pathFichero);
            }
            catch
            {
            }
        }

        private void btnNoEncriptado_Click(object sender, EventArgs e)
        {
            string nombreFichero = ConfigurationManager.AppSettings["FicheroModulosGenerado"];
            if (nombreFichero == null || nombreFichero == "") nombreFichero = "ModulosCliente.xml";
            string pathFichero = Application.StartupPath + "\\" + nombreFichero;
            try
            {
                System.Diagnostics.Process.Start(pathFichero);
            }
            catch
            {
            }
        }

        private void btnFicheroLog_Click(object sender, EventArgs e)
        {
            string nombreFichero = ConfigurationManager.AppSettings["FicheroLogs"];
            if (nombreFichero == null || nombreFichero == "") nombreFichero = "LogGenerarFicheroCliente.log";
            string pathFichero = Application.StartupPath + "\\" + nombreFichero;
            try
            {
                System.Diagnostics.Process.Start(pathFichero);
            }
            catch
            {
            }
        }
        #endregion

        #region Métodos Privados
        private void FilldsModulosAGenerar()
        {
            //Cargar los módulos si aún no se ha hecho
            string result = this.CargarModulos();
            if (result != "")
            {
                MessageBox.Show("Error cargando los módulos (" + result + ")", "Error");
                return;
            }

            if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0)
            {
                this.dsModulosAGenerar = null;

                //Buscar la última configuración utilizada para generar ficheros
                string ultimoFicheroGeneradoModulosSel = ConfigurationManager.AppSettings["UltimoFicheroGeneradoModulosSel"];
                if (ultimoFicheroGeneradoModulosSel != null && ultimoFicheroGeneradoModulosSel != "")
                {
                    ultimoFicheroGeneradoModulosSel = "," + ultimoFicheroGeneradoModulosSel + ",";
                }
                else ultimoFicheroGeneradoModulosSel = "";

                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];

                DataRow nuevoModulo;

                for (int i = 0; i < modulosTabla.Rows.Count; i++)
                {
                    //Chequer que el módulo esté activo
                    if ((bool)modulosTabla.Rows[i]["activo"])
                    {
                        if (this.dsModulosAGenerar == null)
                        {
                            //Crear el DataSet
                            this.BuildNewDataSet();
                        }
                       
                        nuevoModulo = this.dsModulosAGenerar.Tables["Modulo"].NewRow();   
                        nuevoModulo["id"] = modulosTabla.Rows[i]["id"].ToString();
                        nuevoModulo["nombre"] = modulosTabla.Rows[i]["nombre"].ToString();
                        nuevoModulo["nombreDll"] = modulosTabla.Rows[i]["nombreDll"].ToString();
                        nuevoModulo["formulario"] = modulosTabla.Rows[i]["formulario"].ToString();
                        nuevoModulo["imagen"] = modulosTabla.Rows[i]["imagen"].ToString();
                        if ((bool)modulosTabla.Rows[i]["basico"]) nuevoModulo["basico"] = "1";
                        else nuevoModulo["basico"] = "0";
                        if ((bool)modulosTabla.Rows[i]["activo"]) nuevoModulo["activo"] = "1";
                        else nuevoModulo["activo"] = "1";

                        //Activar o no la columna Seleccionar (va relacionada con la variable UltimoFicheroGeneradoModulosSel del fichero app.config
                        if (ultimoFicheroGeneradoModulosSel != "")
                        {
                            if (ultimoFicheroGeneradoModulosSel.Contains("," + modulosTabla.Rows[i]["id"].ToString() + ","))
                            {
                                nuevoModulo["seleccionar"] = true;
                            }
                            else
                                nuevoModulo["seleccionar"] = false;
                        }
                        else nuevoModulo["seleccionar"] = true;


                        this.dsModulosAGenerar.Tables["Modulo"].Rows.Add(nuevoModulo);
                    }
                }

                if (this.dsModulosAGenerar == null)
                {
                    this.NoExistenModulos("No existen módulos activos. Por favor, vaya a la opción \"Edición\" en el apartado Módulos.");
                }
                else
                {
                    this.FillDataGrid();
                }
            }
            else
            {
                this.NoExistenModulos("No se han definido módulos. Por favor, vaya a la opción \"Alta\" en el apartado Módulos.");
            }
        }

        /// <summary>
        /// Muestra el mensaje de que no existen módulos y pone no visibles los controles
        /// </summary>
        /// <param name="message"></param>
        private void NoExistenModulos(string message)
        {
            this.lblSeleccionar.Visible = false;
            this.btnAceptar.Visible = false;
            this.dataGridViewModulos.Visible = false;
            this.chkEncriptar.Visible = false;

            this.btnViewFichero.Visible = false;

            //this.lblResult.Text = "No se han definido módulos o no están activos. Por favor, vaya a las opciones de alta o de edición.";
            this.lblResult.Text = message;
            this.lblResult.Location = new Point(this.lblResult.Location.X, 21);
            this.btnCancel.Location = new Point(this.lblResult.Location.X, 51);
            this.Height = 200;
            this.lblResult.Visible = true;
        }

        /// <summary>
        /// Construye el Nuevo DataSet para copiar la información del Data Set dsModulosApp
        /// </summary>
        private void BuildNewDataSet()
        {
            this.dsModulosAGenerar = new DataSet();

            this.tabla_Modulos = new DataTable("Modulo");
            this.tabla_Modulos.Columns.Add("id", typeof(string));
            this.tabla_Modulos.Columns.Add("nombre", typeof(String));
            this.tabla_Modulos.Columns.Add("nombreDll", typeof(String));
            this.tabla_Modulos.Columns.Add("formulario", typeof(String));
            this.tabla_Modulos.Columns.Add("imagen", typeof(String));
            this.tabla_Modulos.Columns.Add("basico", typeof(int));
            this.tabla_Modulos.Columns.Add("activo", typeof(int));
            this.tabla_Modulos.Columns.Add("seleccionar", typeof(bool));

            this.dsModulosAGenerar = new DataSet();
            this.dsModulosAGenerar.DataSetName = "Modulos_Tabla";
            this.dsModulosAGenerar.Tables.Add(this.tabla_Modulos);

            this.dsModulosAGenerar.Tables["Modulo"].Columns["seleccionar"].DataType = typeof(bool);
        }

        /// <summary>
        /// Construye el DataGrid partiendo del DataSet dsModulosAGenerar
        /// </summary>
        private void FillDataGrid()
        {
            //Cargar la información del DataSet de Módulos al DataGrid
            this.dataGridViewModulos.DataSource = this.dsModulosAGenerar;
            this.dataGridViewModulos.DataMember = "Modulo";

            // Set a cell padding to provide space for the top of the focus  
            // rectangle and for the content that spans multiple columns
            int CUSTOM_CONTENT_HEIGHT = 15;
            Padding newPadding = new Padding(0, 1, 0, CUSTOM_CONTENT_HEIGHT);
            this.dataGridViewModulos.RowTemplate.DefaultCellStyle.Padding = newPadding;

            //Ajustar columnas al tamaño de la ventana
            this.dataGridViewModulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //this.dataGridIdiomas.Dock = DockStyle.Fill;

            this.dataGridViewModulos.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.dataGridViewModulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Adjust the row heights to accommodate the normal cell content. 
            this.dataGridViewModulos.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

            //Estilo para la fila header
            this.dataGridViewModulos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            DataGridViewCellStyle style = this.dataGridViewModulos.ColumnHeadersDefaultCellStyle;
            style.BackColor = Color.Navy;
            style.ForeColor = Color.White;
            style.Font = new Font(this.dataGridViewModulos.Font, FontStyle.Bold);

            //this.dataGridIdiomas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dataGridViewModulos.Columns["id"].SortMode = DataGridViewColumnSortMode.NotSortable;

            //Centrar los textos de la fila de Encabezados
            this.dataGridViewModulos.Columns["id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewModulos.Columns["nombre"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewModulos.Columns["nombreDll"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewModulos.Columns["seleccionar"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Textos de la fila del encabezado del DataGrid
            this.dataGridViewModulos.Columns["id"].HeaderCell.Value = "Identificador";
            this.dataGridViewModulos.Columns["nombre"].HeaderCell.Value = "Nombre";
            this.dataGridViewModulos.Columns["nombreDll"].HeaderCell.Value = "Dll";
            this.dataGridViewModulos.Columns["seleccionar"].HeaderCell.Value = "Seleccionar";

            //Columnas de solo lectura
            this.dataGridViewModulos.Columns["id"].ReadOnly = true;
            this.dataGridViewModulos.Columns["nombre"].ReadOnly = true;
            this.dataGridViewModulos.Columns["nombreDll"].ReadOnly = true;

            //Columnas no visibles
            this.dataGridViewModulos.Columns["formulario"].Visible = false;
            this.dataGridViewModulos.Columns["imagen"].Visible = false;
            this.dataGridViewModulos.Columns["basico"].Visible = false;
            this.dataGridViewModulos.Columns["activo"].Visible = false;

            //Centrar los valores de la columna de identificadores
            this.dataGridViewModulos.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Controles Visibles
            this.dataGridViewModulos.Visible = true;
            this.chkEncriptar.Visible = true;
            this.btnAceptar.Visible = true;
        }

        /*
        /// <summary>
        /// Escribe el error en el fichero de log
        /// </summary>
        /// <param name="error"></param>
        private void EscribirFicheroLog(string error)
        {
            string nombreFichero = ConfigurationManager.AppSettings["FicheroLogs"];
            if (nombreFichero == null || nombreFichero == "") nombreFichero = "LogGenerarFicheroCliente.log";
            string pathFile = Application.StartupPath + "\\" + nombreFichero;

            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                //sw.Write(DateTime.Now.ToString("yyyy-MM-dd") +  + "\n\r");
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd") + "\n\r");
                sw.Write("Error creando el fichero de módulos para clientes" + "\n\r");
                sw.Write(error);
                sw.Close();
            }
        }
        */

        /// <summary>
        /// Verifica que se haya seleccionado al menos un módulo
        /// </summary>
        private bool FormValid()
        {
            bool existenModulosSel = false;
            for (int i = 0; i < this.dataGridViewModulos.Rows.Count; i++)
            {
                //Chequer que el módulo esté activo
                if ((bool)this.dataGridViewModulos.Rows[i].Cells["seleccionar"].Value)
                {
                    existenModulosSel = true;
                    break;
                }
            }

            return existenModulosSel;
        }


        public string serialIDfromDisk(string letra)
        {
            string idDisk = "";
            try
            {
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + letra + ":\"");
                disk.Get();
                idDisk = disk["VolumeSerialNumber"].ToString();
            }
            catch
            {
            }
            return (idDisk);
           
                /*
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
             ManagementObjectSearcher searcher = new
    ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk"); 
    StringBuilder sb = new StringBuilder(); 
    foreach (ManagementObject wmi in searcher.Get())
    {
        try
        {
            sb.Append("Drive Device ID: " +
            wmi.GetPropertyValue("DeviceID").ToString() +Environment.NewLine);
            sb.Append("Caption: " + wmi.GetPropertyValue("Caption").ToString() + Environment.NewLine);
            sb.Append("Volume Serial Number: " + wmi.GetPropertyValue("VolumeSerialNumber").ToString()
            + Environment.NewLine);
            sb.Append("Free Space: " + wmi.GetPropertyValue("FreeSpace").ToString() + "
            bytes free" + Environment.NewLine + Environment.NewLine);
        }
        catch
        {
            return sb.ToString();
        }
    }
            */

            /*
            var enu = manageObjSearch.Get().GetEnumerator();
            if (!enu.MoveNext()) throw new Exception("Unexpected WMI query failure");
            long sizeInKilobytes = Convert.ToInt64(enu.Current["TotalVisibleMemorySize"]);

            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
             */ 
        }
        #endregion
    }
}
