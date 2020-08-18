using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

namespace ObjectModel
{
    public class ExcelExportImport
    {
        private string nombreFichero;
        private string pathFichero;
        private DataTable dataTable;
        private bool cabecera;
        private ArrayList gridColumnas = null;
        private string titulo;
        private ArrayList indiceFilasSeleccionadas = null;

        #region Propiedades
        public string NombreFichero
        {
            get
            {
                return (this.nombreFichero);
            }
            set
            {
                this.nombreFichero = value;
            }
        }

        public string PathFichero
        {
            get
            {
                return (this.pathFichero);
            }
            set
            {
                this.pathFichero = value;
            }
        }

        public DataTable DateTableDatos
        {
            get
            {
                return (this.dataTable);
            }
            set
            {
                this.dataTable = value;
            }
        }

        public bool Cabecera
        {
            get
            {
                return (this.cabecera);
            }
            set
            {
                this.cabecera = value;
            }
        }

        public ArrayList GridColumnas
        {
            get
            {
                return (this.gridColumnas);
            }
            set
            {
                this.gridColumnas = value;
            }
        }

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

        public ArrayList IndiceFilasSeleccionadas
        {
            get
            {
                return (this.indiceFilasSeleccionadas);
            }
            set
            {
                this.indiceFilasSeleccionadas = value;
            }
        }
        #endregion

        public ExcelExportImport()
        {
            //Por defecto se graba y se lee la cabecera en el fichero Excel
            this.cabecera = true;
        }

        #region Métodos Públicos
        /// <summary>
        /// Exporta losdatos de un DataTable a un fichero excel en memoria
        /// </summary>
        /// <returns></returns>
        public string ExportarMemoria()
        {
            string result = this.Exportar(false, false);
            return (result);
        }

        /// <summary>
        /// Exporta los datos de un DataTable a un fichero Excel o a memoria, dependiendo de los parámetros
        /// </summary>
        /// <param name="view">true -> se graba el fichero y después se muestra   false -> se muestra directamente el fichero</param>
        /// <param name="view">true -> visualizar el fichero (para el caso que grabarFichero=true)   false -> no se visualizar el fichero (para el caso que grabarFichero=true)</param>
        /// <returns></returns>
        public string Exportar(bool grabarFichero, bool view)
        {
            string result = "";

            try
            {
                if (this.dataTable == null || this.dataTable.Columns.Count == 0)
                {
                    //throw new Exception("Error exportando a excel: Tabla nula o vacía\n");
                    result = "Error exportando a excel: Tabla nula o vacía\n";
                    return (result);
                }

                // load excel, and create a new workbook 
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application(); 
                excelApp.Workbooks.Add(); 

                // single worksheet 
                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                int row = 1;
                int columna = 0;
                string[] desColumna;

                if (this.titulo != null && this.titulo.Trim() != "")
                {
                    //Escribir línea de título
                    workSheet.Cells[row, 1] = this.titulo;
                    row = 2;
                }

                if (this.cabecera)
                {
                    ColorConverter cc = new ColorConverter();
                    // column headings 
                    if (this.gridColumnas == null)
                    {
                        //Coger las cabeceras de las columnas del DataTable
                        for (int i = 0; i < this.dataTable.Columns.Count; i++)
                        {
                            workSheet.Cells[row, (i + 1)] = this.dataTable.Columns[i].ColumnName;
                            workSheet.Cells[row, (i + 1)].Interior.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#C5D9F1"));
                        }
                        
                    }
                    else
                    {
                        //Coger las cabeceras de las columnas de las columnas de la Grid
                        for (int i = 0; i < this.GridColumnas.Count; i++)
                        {
                            desColumna = (string[])this.gridColumnas[i];
                            if (desColumna[2] == "1")   //columna visible
                            {
                                columna++;
                                workSheet.Cells[row, columna] = ((string[])this.gridColumnas[i])[0];
                                workSheet.Cells[row, columna].Interior.Color = ColorTranslator.ToOle((Color)cc.ConvertFromString("#C5D9F1"));
                            }
                        }
                    }
                    row++;
                }

                // rows 
                int fila = 0;
                for (int i = 0; i < this.dataTable.Rows.Count; i++) 
                {
                    if ((this.indiceFilasSeleccionadas == null) ||
                        (this.indiceFilasSeleccionadas.Count == 0) ||
                        (this.indiceFilasSeleccionadas.Count > 0 && this.indiceFilasSeleccionadas.Contains(i)))
                    {
                        columna = 0;
                        // to do: format datetime values before printing 
                        for (int j = 0; j < this.dataTable.Columns.Count; j++)
                        {
                            //if (this.dataTable.Columns[j].GetType().Name == "Decimal") 

                            if (this.gridColumnas == null || j > this.gridColumnas.Count)
                            {
                                columna++;
                                workSheet.Cells[(fila + row), (columna)] = this.dataTable.Rows[i][j];
                            }
                            else
                            {
                                desColumna = (string[])this.gridColumnas[j];
                                if (desColumna[2] == "1")   //columna visible
                                {
                                    columna++;

                                    switch (desColumna[1])
                                    {
                                        case "decimal":
                                            //workSheet.Cells[(i + row), columna].NumberFormat = "0,00";
                                            workSheet.Cells[(fila + row), columna].NumberFormat = "#.##0,00";
                                            try
                                            {
                                                workSheet.Cells[(fila + row), columna] = Convert.ToDecimal(this.dataTable.Rows[i][j]);
                                            }
                                            catch (Exception ex)
                                            {
                                                workSheet.Cells[(fila + row), columna] = this.dataTable.Rows[i][j];
                                                GlobalVar.Log.Error(ex.Message);
                                            }
                                            break;
                                        case "numero":
                                            workSheet.Cells[(fila + row), columna].NumberFormat = "0";
                                            workSheet.Cells[(fila + row), columna] = this.dataTable.Rows[i][j];
                                            break;
                                        default:
                                            workSheet.Cells[(fila + row), columna].NumberFormat = "@";
                                            workSheet.Cells[(fila + row), columna] = this.dataTable.Rows[i][j];
                                            break;
                                    }
                                }
                            }
                        }
                        fila++;
                    }
                }

                if (!grabarFichero)
                {
                    excelApp.Visible = true;
                    return (result);
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    //saveFileDialog1.Filter = "Excel Files(.xls)|*.xls|Excel Files(.xlsx)|*.xlsx";
                    Filter = "Files(.xlsx)|*.xlsx",
                    //saveFileDialog1.FilterIndex = 2;
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //string filename = @"C:\VS2010_Projects\FinanzasNet\ModComprobantes\comprobantesContables\export.xlsx";
                    string filename = saveFileDialog1.FileName;
                    
                    // check fielpath 
                    if (filename != null && filename != "")
                    {
                        try
                        {
                            workSheet.SaveAs(filename);
                            excelApp.Quit();
                            MessageBox.Show("El fichero excel se grabó correctamente.");  //Falta traducir

                            if (view)
                            {
                                try
                                {
                                    //Cargar el fichero excel
                                    ProcessStartInfo startInfo = new ProcessStartInfo
                                    {
                                        FileName = "EXCEL.EXE",
                                        Arguments = filename
                                    };
                                    Process.Start(startInfo);
                                }
                                catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobalVar.Log.Error(ex.Message);

                            //throw new Exception("Error exportando a excel: El fichero excel no se pudo grabar (" + ex.Message + ")"); 
                            result = "Error exportando a excel: El fichero excel no se pudo grabar (" + ex.Message + ")";   //Falta traducir
                            return (result);
                        }
                    }
                    else // no filepath is given 
                    {
                        excelApp.Visible = true;
                    }
                }
                else
                {
                    result = "CANCELAR";
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Importa los datos de un fichero Excel a un DataTable
        /// </summary>
        /// <returns></returns>
        public string Importar()
        {
            string result = "";

            try
            {
                //string filename = @"C:\VS2010_Projects\FinanzasNet\ModComprobantes\comprobantesContables\CreadoPorExcel1.xlsx";
                OpenFileDialog openfile1 = new OpenFileDialog
                {
                    //openfile1.Filter = "Excel Files(.xls)|*.xls| Excel Files(.xlsx)|*.xlsx| Excel Files(*.xlsm)|*.xlsm";
                    //openfile1.Filter = "Excel Files(.xls)|*.xls|Excel Files(.xlsx)|*.xlsx";
                    Filter = "Excel Files(.xlsx)|*.xlsx"
                };
                if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filename = openfile1.FileName;
                    string excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    string excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                    //string extension = ".xlsx";
                    string extension = Path.GetExtension(filename);

                    string conStr = "";
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03
                            conStr = excel03ConString;
                            break;
                        case ".xlsx": //Excel 07
                            conStr = excel07ConString;
                            break;
                    }
                    //conStr = excel07ConString;
                    string isHDR = "Yes";
                    if (!this.cabecera) isHDR = "No";
                    conStr = String.Format(conStr, filename, isHDR);
                    //conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VS2010_Projects\FinanzasNet\ModComprobantes\comprobantesContables\Prueba (2).xls;Extended Properties='Excel 8.0;HDR=Yes'";
                    OleDbConnection connExcel = new OleDbConnection(conStr);
                    OleDbCommand cmdExcel = new OleDbCommand();
                    OleDbDataAdapter oda = new OleDbDataAdapter();
                    this.dataTable = new DataTable();
                    cmdExcel.Connection = connExcel;

                    //Get the name of First Sheet
                    connExcel.Open();
                    DataTable dtExcelSchema;
                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    connExcel.Close();

                    //Read Data from First Sheet
                    connExcel.Open();
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(this.dataTable);
                    connExcel.Close();
                }
                else
                {
                    result = "CANCELAR";
                }
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);

                result  = ex.Message;
            }

            return (result);
        }
        #endregion
    }
}
