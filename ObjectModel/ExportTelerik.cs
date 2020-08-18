using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.FileDialogs;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace ObjectModel
{
    /// <summary>
    /// Enumera los proveedores soportados
    /// </summary>
    public enum ExportFileType
    {
        EXCEL,
        HTML,
        PDF
    }

    public class ExportTelerik
    {
        private string titulo;
        private string subtitulo;
        private string nombreFichero;
        private string pathFichero;
        private string nombreHojaExcel_CaptionText;
        private bool exportToMemory;
        private ExportFileType exportFileType;
        private readonly RadGridView radGridView;
        private ArrayList gridColumnas;
        private bool selectAll;

        private string exportFileName = "";
        private const string nombreFicheroDefecto = "smartCGExportar";

        private string mensajeProceso;

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

        public string Subtitulo
        {
            get
            {
                return (this.subtitulo);
            }
            set
            {
                this.subtitulo = value;
            }
        }

        public string NombreHojaExcel_CaptionText
        {
            get
            {
                return (this.nombreHojaExcel_CaptionText);
            }
            set
            {
                this.nombreHojaExcel_CaptionText = value;
            }
        }

        public bool ExportToMemory
        {
            get
            {
                return (this.exportToMemory);
            }
            set
            {
                this.exportToMemory = value;
            }
        }

        public ExportFileType ExportType
        {
            get
            {
                return (this.exportFileType);
            }
            set
            {
                this.exportFileType = value;
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

        public bool SelectAll
        {
            get
            {
                return (this.selectAll);
            }
            set
            {
                this.selectAll = value;
            }
        }
        #endregion

        public ExportTelerik(ref RadGridView radGridViewExport)
        {
            this.radGridView = radGridViewExport;
            this.nombreHojaExcel_CaptionText = "";

            //Inicializarlos con los parámetros del usuario almacenados en el fichero Usuario_.xml y cargados en GlobalVar al hacer loginApp
            this.exportToMemory = GlobalVar.UsuarioEnv.ExportarVisualizarFicheroDefecto;
            this.exportFileType = GlobalVar.UsuarioEnv.ExportarTipoFicheroDefecto;

            this.pathFichero = "";
            this.nombreFichero = "";
            this.selectAll = false;
            this.subtitulo = "";

            this.gridColumnas = null;

            this.exportFileName = "";
            this.mensajeProceso = "";
        }

        #region Métodos Públicos
        /// <summary>
        /// Exportar 
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            string result = "";

            try
            {
                var selectedRows = this.radGridView.SelectedRows.ToArray();

                if (!this.selectAll)
                {
                    try
                    {
                        foreach (var row in this.radGridView.Rows.ToArray())
                        {
                            if (!row.IsSelected)
                            {
                                row.IsVisible = false;
                            }
                        }
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }

                if (this.exportToMemory)
                {
                    this.exportFileName = this.MemoryFileName(this.pathFichero + this.nombreFichero);
                }
                else
                {
                    this.exportFileName = this.FileDialogFileName(ref result);
                }

                if (result == "")
                {
                    switch (this.exportFileType)
                    {
                        case ExportFileType.EXCEL:
                            result = this.ExportToExcel();
                            break;
                        case ExportFileType.HTML:
                            result = this.ExportToHTML();
                            break;
                        case ExportFileType.PDF:
                            result = this.ExportToPDF();
                            break;
                    }
                }

                if (!this.selectAll)
                {
                    try
                    {
                        foreach (var row in this.radGridView.Rows.ToArray())
                        {
                            row.IsVisible = true;

                            if (selectedRows.Contains(row))
                            {
                                row.IsSelected = true;
                            }
                        }
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Exportar a Excel
        /// </summary>
        /// <returns></returns>
        public string ExportToExcel()
        {
            string result = "";

            try
            {
                GridViewSpreadExport spreadExporter = new GridViewSpreadExport(this.radGridView);
                spreadExporter.CellFormatting += SpreadExporter_CellFormatting;
                SpreadExportRenderer exportRenderer = new SpreadExportRenderer();
                exportRenderer.WorkbookCreated += ExportRenderer_WorkbookCreated;

                //The main cause for the slow performance is that you export the grid visual settings.When you have this option turned on all the grid elements have to be created in order to build the styles.After each element is created, its style is exported.If you can, I would recommend that you set styles of the cells in the ExcelCellFormatting event and turn off the export of styles settings.Also, it's always advisable to export data in a separate thread. 
                spreadExporter.ExportVisualSettings = true;
                if (this.nombreHojaExcel_CaptionText != "") spreadExporter.SheetName = this.nombreHojaExcel_CaptionText;
                spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
                spreadExporter.SummariesExportOption = SummariesOption.DoNotExport;
                spreadExporter.HiddenRowOption = HiddenOption.DoNotExport;
                spreadExporter.HiddenColumnOption = HiddenOption.DoNotExport;
                spreadExporter.ExportChildRowsGrouped = true;

                if (this.gridColumnas != null) this.ExcelColumnsProcesar();

                spreadExporter.AsyncExportCompleted += ExporterEXCEL_AsyncExportCompleted;

                if (this.exportToMemory)
                {
                    MemoryStream ms = new System.IO.MemoryStream();
                    spreadExporter.RunExportAsync(ms, exportRenderer);
                }
                else spreadExporter.RunExportAsync(this.exportFileName, exportRenderer);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);
                result = "Error exportando los datos a Excel. Para más información consulte el fichero de Log.";
                this.mensajeProceso = result;
            }

            return (result);
        }

        /// <summary>
        /// Exportar a HTML
        /// </summary>
        /// <returns></returns>
        public string ExportToHTML()
        {
            string result = "";

            try
            {
                ExportToHTML exporter = new ExportToHTML(this.radGridView)
                {
                    FileExtension = "html",
                    HiddenColumnOption = HiddenOption.DoNotExport,
                    HiddenRowOption = HiddenOption.DoNotExport,

                    TableBorderThickness = 1,
                    ExportVisualSettings = true,
                    ExportHierarchy = true,

                    FitWidthSize = 1000,
                    AutoSizeColumns = true,

                    TableCaption = "Table",
                };

                this.exportFileType = ExportFileType.HTML;

                exporter.HTMLTableCaptionFormatting += Exporter_HTMLTableCaptionFormatting;
                exporter.HTMLCellFormatting += Exporter_HTMLCellFormatting;

                exporter.RunExport(this.exportFileName);

                if (this.exportToMemory) this.HTMLViewFile();
                else
                {
                    DialogResult visualizarFichero = RadMessageBox.Show("Fichero exportado con éxito. ¿Desea visualizar el fichero (" + this.exportFileName + ")?", "Datos exportados", MessageBoxButtons.YesNo, RadMessageIcon.Question);

                    if (visualizarFichero == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.HTMLViewFile();
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Exportar a PDF
        /// </summary>
        /// <returns></returns>
        public string ExportToPDF()
        {
            string result = "";

            try
            {
                GridViewPdfExport pdfExporter = new GridViewPdfExport(this.radGridView)
                {
                    FileExtension = "pdf",
                    HiddenColumnOption = HiddenOption.DoNotExport,
                    HiddenRowOption = HiddenOption.DoNotExport,
                    ShowHeaderAndFooter = true,

                    FitToPageWidth = true,

                    Scale = 1.2
                };

                /*
                //Customizing headers and footers
                pdfExporter.HeaderHeight = 30;
                pdfExporter.HeaderFont = new Font("Arial", 22);
                //pdfExporter.Logo = System.Drawing.Image.FromFile(@"C:\MyLogo.png");
                pdfExporter.Logo = System.Drawing.Image.FromFile(@"C:\VS2017_Projects\FinanzasNet\FinanzasNet\bin\Debug\tmp\modSII.jpg");
                pdfExporter.LeftHeader = "[Logo]";
                pdfExporter.LogoAlignment = ContentAlignment.MiddleLeft;
                pdfExporter.LogoLayout = Telerik.WinControls.Export.LogoLayout.Fit;

                pdfExporter.MiddleHeader = "Middle header";
                pdfExporter.RightHeader = "Right header";
                pdfExporter.ReverseHeaderOnEvenPages = true;

                pdfExporter.FooterHeight = 30;
                pdfExporter.FooterFont = new Font("Arial", 22);
                pdfExporter.LeftFooter = "Left footer";
                pdfExporter.MiddleFooter = "Middle footer";
                pdfExporter.RightFooter = "Right footer";
                pdfExporter.ReverseFooterOnEvenPages = true;
                */
                pdfExporter.HeaderExported += PdfExporter_HeaderExported;

                this.exportFileType = ExportFileType.PDF;

                pdfExporter.AsyncExportCompleted += PdfExporter_AsyncExportCompleted;
                pdfExporter.RunExportAsync(this.exportFileName, new Telerik.WinControls.Export.PdfExportRenderer());
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        #endregion

        #region Métodos Privados

        /// <summary>
        /// Genera un nombre de fichero valido (si existe el fichero crea otro con el dia y la hora)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string MemoryFileName(string name)
        {
            string fileName = "";
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\app\\tmp\\";

            try
            {
                //Eliminar todos los ficheros que se han generado anteriormente
                string nombreFicheroEliminarPatron1 = "\\" + nombreFicheroDefecto + ".";
                string nombreFicheroEliminarPatron2 = "\\" + nombreFicheroDefecto + "_";
                string[] ficherosCarpeta = System.IO.Directory.GetFiles(@path);
                foreach (string ficheroActual in ficherosCarpeta)
                {
                    if (ficheroActual.Contains(nombreFicheroEliminarPatron1) ||
                        ficheroActual.Contains(nombreFicheroEliminarPatron2))
                    {

                        try
                        {
                            System.IO.File.Delete(ficheroActual);
                        }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }

            try
            {
                // Valida los caracteres del nombre del fichero. Si encuentra caracteres no validos los sustituye por '_'
                if (name != "") name = Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '_'));
                else name = nombreFicheroDefecto + "_" + System.Environment.UserName.ToUpper();

                string extension = "";
                switch (this.exportFileType)
                {
                    case ExportFileType.EXCEL:
                        extension = ".xlsx";
                        break;
                    case ExportFileType.HTML:
                        extension = ".html";
                        break;
                    case ExportFileType.PDF:
                        extension = ".pdf";
                        break;
                }

                if (FileInUse(path + name + extension))
                {
                    //Si el fichero está en uso, recalcular el nombre añadiendo día y hora
                    DateTime localDate = DateTime.Now;
                    string fecha = localDate.Year.ToString() + localDate.Month.ToString() + localDate.Day.ToString() + "_";
                    string hora = DateTime.Now.ToString("hh:mm:ss");
                    hora = hora.Replace(":", "");
                    fecha = fecha + hora;

                    fileName = name + "_" + fecha + extension;
                }
                else fileName += name + extension;

                fileName = path + fileName;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (fileName);
        }

        /// <summary>
        /// Devuelve si un fichero est
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool FileInUse(string path)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate))
                {
                    bool canW = fs.CanWrite;
                }
                return false;
            }
            catch { return true; }
        }

        private string FileDialogFileName(ref string error)
        {
            string fileName = "";
            error = "";

            try
            {
                //FileDialogsLocalizationProvider.CurrentProvider = new RadOpenFolderDialogLocalizationProviderES();
                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
                if (this.pathFichero != "") saveFileDialog.InitialDirectory = this.pathFichero;
                else
                {
                    string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\app\\tmp\\";
                    if (path == "" || path == null) saveFileDialog.RestoreDirectory = true;
                    else saveFileDialog.InitialDirectory = path;
                }
                
                if (this.nombreFichero != "") saveFileDialog.FileName = this.nombreFichero;
                else saveFileDialog.FileName = nombreFicheroDefecto;

                saveFileDialog.Filter = "Excel Worksheets|*.xlsx;" +
                                            "|HTML Files|*.html;" +
                                            "|PDF Files|*.pdf";

                switch (this.exportFileType)
                {
                    case ExportFileType.EXCEL:
                        saveFileDialog.FilterIndex = 1;
                        break;
                    case ExportFileType.HTML:
                        saveFileDialog.FilterIndex = 2;
                        break;
                    case ExportFileType.PDF:
                        saveFileDialog.FilterIndex = 3;
                        break;
                }

                //Traducir literales propios de los DataFilter
                System.Windows.Forms.DialogResult dr = saveFileDialog.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = saveFileDialog.FileName;

                    //Revisar la extensión del fichero para saber el tipo de fichero a exportar
                    if (fileName.Length > 4)
                    {
                        string lastCaracteres = fileName.Substring(fileName.Length - 4, 4);
                        switch (lastCaracteres)
                        {
                            case "xlsx":
                                this.exportFileType = ExportFileType.EXCEL;
                                break;
                            case "html":
                                this.exportFileType = ExportFileType.HTML;
                                break;
                            case ".pdf":
                                this.exportFileType = ExportFileType.PDF;
                                break;
                        }
                    }
                }
                else error = "Debe indicar un nombre de fichero para exportar la información";
            }
            catch (Exception ex)
            {
                error = ex.Message;
                GlobalVar.Log.Error(ex.Message);
            }

            return (fileName);
        }

        #region HTML
        private void Exporter_HTMLTableCaptionFormatting(object sender, Telerik.WinControls.UI.Export.HTML.HTMLTableCaptionFormattingEventArgs e)
        {
            if (this.titulo != "")
            {
                //e.TableCaptionElement.Styles.Add("background-color", ColorTranslator.ToHtml(Color.Red));
                e.TableCaptionElement.Styles.Add("background-color", "#C5D9F1");
                e.TableCaptionElement.Styles.Add("font-size", "150%");
                e.TableCaptionElement.Styles.Add("color", ColorTranslator.ToHtml(Color.White));
                e.TableCaptionElement.Styles.Add("font-weight", "bold");
                if (this.subtitulo != "") e.CaptionText = this.titulo + "<br>" + this.subtitulo;
                else e.CaptionText = this.titulo;
            }

            /*
            .TextoTITCab    {
                mso - number - format:\@; font - weight:700; background - color:#C5D9F1}"  */
        }

        private void Exporter_HTMLCellFormatting(object sender, Telerik.WinControls.UI.Export.HTML.HTMLCellFormattingEventArgs e)
        {
            /*if (e.GridColumnIndex == 1 && e.GridRowInfoType == typeof(GridViewTableHeaderRowInfo))
            {
                e.HTMLCellElement.Colspan = 10;
            }
            if (e.GridColumnIndex == 1 && e.GridRowInfoType == typeof(GridViewHierarchyRowInfo))
            {
                e.HTMLCellElement.Colspan = 10;
            }*/
            if (e.HTMLCellElement.Value == "")
            {
                e.HTMLCellElement.Value = " ";
            }
        }

        /// <summary>
        /// Carga el fichero HTML
        /// </summary>
        private void HTMLViewFile()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "\"" + this.exportFileName + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };

                //FileName = "CHROME",
                //Arguments = "\"" + this.exportFileName + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);
                RadMessageBox.Show("Error exportando el fichero. Para más información consulte el fichero de Log.");
            }
        }
        #endregion

        #region Excel
        private void ExportRenderer_WorkbookCreated(object sender, WorkbookCreatedEventArgs e)
        {
            if (this.titulo != "")
            {
                /*Worksheet worksheet = (Worksheet)e.Workbook.Sheets[0];
                worksheet.Columns[worksheet.UsedRange].AutoFitWidth();*/

                Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet = e.Workbook.Sheets[0] as Worksheet;
                CellRange range = new CellRange(0, 0, 1, this.radGridView.Columns.Count - 1);
                CellSelection header = worksheet.Cells[range];
                if (header.CanInsertOrRemove(range, ShiftType.Down))
                {
                    header.Insert(InsertShiftType.Down);
                }
                header.Merge();
                /*header.SetFormat(textFormat);
                header.SetHorizontalAlignment(RadHorizontalAlignment.Center);
                header.SetVerticalAlignment(RadVerticalAlignment.Center);
                header.SetFontFamily(new ThemableFontFamily("Rockwell"));
                header.SetFontSize(24);
                header.SetFill(solidPatternFill);*/
                header.SetIsBold(true);
                header.SetValue(this.titulo);

                if (this.subtitulo != "")
                {
                    CellRange range1 = new CellRange(2, 0, 2, this.radGridView.Columns.Count - 1);
                    CellSelection header1 = worksheet.Cells[range1];
                    //header1.SetIsBold(true);
                    if (header1.CanInsertOrRemove(range1, ShiftType.Down))
                    {
                        header1.Insert(InsertShiftType.Down);
                    }
                    header1.Merge();
                    header1.SetValue(this.subtitulo);
                }
            }
        }

        private void SpreadExporter_CellFormatting(object sender, Telerik.WinControls.Export.CellFormattingEventArgs e)
        {
            if (e.GridRowInfoType == typeof(GridViewTableHeaderRowInfo))
            {
                //e.CellStyleInfo.Underline = true;

                if (e.GridCellInfo.RowInfo.HierarchyLevel == 0)
                {
                    e.CellStyleInfo.BackColor = Color.DeepSkyBlue;
                }
                else if (e.GridCellInfo.RowInfo.HierarchyLevel == 1)
                {
                    e.CellStyleInfo.BackColor = Color.LightSkyBlue;
                }
            }
            /*
            if (e.GridRowInfoType == typeof(GridViewHierarchyRowInfo))
            {
                if (e.GridCellInfo.RowInfo.HierarchyLevel == 0)
                {
                    e.CellStyleInfo.IsItalic = true;
                    e.CellStyleInfo.FontSize = 12;
                    e.CellStyleInfo.BackColor = Color.GreenYellow;
                }
                else if (e.GridCellInfo.RowInfo.HierarchyLevel == 1)
                {
                    e.CellStyleInfo.ForeColor = Color.DarkGreen;
                    e.CellStyleInfo.BackColor = Color.LightGreen;
                }
            }*/
        }

        private void ExcelColumnsProcesar()
        {
            try
            {
                int columna = 0;
                string[] desColumna;
                // to do: format datetime values before printing 
                for (int j = 0; j < this.radGridView.Columns.Count; j++)
                {
                    //if (this.dataTable.Columns[j].GetType().Name == "Decimal") 

                    if (this.gridColumnas != null || j < this.gridColumnas.Count)
                    {
                        desColumna = (string[])this.gridColumnas[j];
                        if (this.radGridView.Columns[j].IsVisible)   //columna visible
                        {
                            columna++;

                            switch (desColumna[1])
                            {
                                case "decimal":
                                    this.radGridView.Columns[j].ExcelExportType = Telerik.WinControls.UI.Export.DisplayFormatType.Custom;
                                    this.radGridView.Columns[j].ExcelExportFormatString = "#.##0,00";
                                    break;
                                case "numero":
                                    var colNumero = this.radGridView.Columns[j] as GridViewDecimalColumn;
                                    colNumero.ExcelExportType = DisplayFormatType.Custom;
                                    colNumero.ExcelExportFormatString = "0";
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        private void ExporterEXCEL_AsyncExportCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (this.mensajeProceso != "") RadMessageBox.Show(this.mensajeProceso);
                else
                {
                    RunWorkerCompletedEventArgs args = e as RunWorkerCompletedEventArgs;
                    if (this.exportToMemory) this.ExcelViewFile(args);
                    else
                    {
                        DialogResult result = RadMessageBox.Show("Fichero exportado con éxito. ¿Desea visualizar el fichero (" + this.exportFileName + ")?", "Datos exportados", MessageBoxButtons.YesNo, RadMessageIcon.Question);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.ExcelViewFile(args);
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Carga el fichero Excel
        /// </summary>
        /// <param name="args"></param>
        private void ExcelViewFile(RunWorkerCompletedEventArgs args)
        {
            try
            {
                if (this.exportToMemory)
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(this.exportFileName, FileMode.Create, FileAccess.Write))
                    {
                        MemoryStream ms = args.Result as MemoryStream;
                        ms.WriteTo(fileStream);
                        ms.Close();
                    }
                }
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "EXCEL",
                    Arguments = "\"" + this.exportFileName + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);
                RadMessageBox.Show("Error exportando el fichero. Para más información consulte el fichero de Log.");
            }
        }
        #endregion

        #region PDF
        private void PdfExporter_HeaderExported(object sender, ExportEventArgs e)
        {
            var editor = e.Editor as Telerik.WinControls.Export.PdfEditor;
            editor.DrawText(this.titulo);
        }

        private void PdfExporter_AsyncExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (this.mensajeProceso != "") RadMessageBox.Show(this.mensajeProceso);
                else
                {
                    RunWorkerCompletedEventArgs args = e as RunWorkerCompletedEventArgs;
                    if (this.exportToMemory) this.PDFViewFile(args);
                    else
                    {
                        DialogResult result = RadMessageBox.Show("Fichero exportado con éxito. ¿Desea visualizar el fichero (" + this.exportFileName + ")?", "Datos exportados", MessageBoxButtons.YesNo, RadMessageIcon.Question);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.PDFViewFile(args);
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Carga el fichero Excel
        /// </summary>
        /// <param name="args"></param>
        private void PDFViewFile(RunWorkerCompletedEventArgs args)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "\"" + this.exportFileName + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                GlobalVar.Log.Error(ex.Message);
                RadMessageBox.Show("Error exportando el fichero. Para más información consulte el fichero de Log.");
            }
        }
        #endregion

        #endregion
    }
}