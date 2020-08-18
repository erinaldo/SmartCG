using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using ObjectModel;
using Telerik.WinControls.UI;
using System.IO;
using System.Linq;
using Telerik.WinControls.Data;
using System.Diagnostics;

namespace ModConsultaInforme
{
    public partial class frmInfoConsFicheroGeneradoView : frmPlantilla, IReLocalizable
    {
        //public string formCode = "MCIFIGEVI";

        /// <summary>
        /// Enumera los proveedores soportados
        /// </summary>
        public enum FicherosGeneradosView
        {
            Informe,
            Consulta
        }

        private FicherosGeneradosView ficherosGeneradosViewTipo;

        Dictionary<string, string> displayNames;
        DataTable dtInfConFicheroGeneradoView = new DataTable();

        private string pathFicheros = "";

        public string FicherosGeneradosViewTipoStr
        {
            set
            {
                switch (value)
                {
                    case "I":
                        this.ficherosGeneradosViewTipo = FicherosGeneradosView.Informe;
                        break;
                    case "C":
                        this.ficherosGeneradosViewTipo = FicherosGeneradosView.Consulta;
                        break;
                }
            }
        }


        public frmInfoConsFicheroGeneradoView()
        {
            InitializeComponent();

            this.radGridViewInfConsGenerados.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        private void FrmInfoConsFicheroGeneradoView_Load(object sender, EventArgs e)
        {
            string formulario = "";
            if (this.ficherosGeneradosViewTipo == FicherosGeneradosView.Informe)
            {
                formulario = " - Informe";
                this.radLabelNoHayInfo.Text = "No existen informes generados";
            }
            else
            {
                formulario = " - Consulta";
                this.radLabelNoHayInfo.Text = "No existen consultas generadas";
            }

            Log.Info("INICIO Informe/Consulta Fichero Generado Visualizar" + formulario);

            Cursor.Current = Cursors.WaitCursor;

            this.BuildDisplayNames();

            //Crear la Grid
            this.BuilGridInfConFicheroGeneradoView();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            Cursor.Current = Cursors.Default;
        }
        
        private void RadButtonView_Click(object sender, EventArgs e)
        {
            this.FileView();
        }

        private void RadGridViewInfConsGenerados_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            this.FileView();
        }

        private void RadButtonEliminar_Click(object sender, EventArgs e)
        {
            this.FilesDelete();
        }

        private void RadButtonView_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonView);
        }

        private void RadButtonView_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonView);
        }

        private void RadButtonEliminar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminar);
        }

        private void RadButtonEliminar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminar);
        }

        private void RadGridViewInfConsGenerados_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewInfConsGenerados, ref this.selectAll);
        }

        private void FrmInfoConsFicheroGeneradoView_Shown(object sender, EventArgs e)
        {
            if (this.ficherosGeneradosViewTipo == FicherosGeneradosView.Consulta)
                if (this.radGridViewInfConsGenerados.Columns.Contains("Tipo")) this.radGridViewInfConsGenerados.Columns["Tipo"].IsVisible = false;

            if (this.radGridViewInfConsGenerados.Columns["Fecha"].GetType() == typeof(Telerik.WinControls.UI.GridViewDateTimeColumn))
            {
                ((GridViewDateTimeColumn)this.radGridViewInfConsGenerados.Columns["Fecha"]).FormatString = "{0:dd/MM/yyyy HH:mm:ss}";
                ((GridViewDateTimeColumn)this.radGridViewInfConsGenerados.Columns["Fecha"]).Format = DateTimePickerFormat.Custom;
                //((GridViewDateTimeColumn)this.radGridViewInfConsGenerados.Columns["Fecha"]).CustomFormat = "MM/dd/yyyy";
                ((GridViewDateTimeColumn)this.radGridViewInfConsGenerados.Columns["Fecha"]).CustomFormat = "dd/MM/yyyy HH:mm:ss";
            }
            
        }

        private void FrmInfoConsFicheroGeneradoView_FormClosing(object sender, FormClosingEventArgs e)
        {
            string formulario = "";
            if (this.ficherosGeneradosViewTipo == FicherosGeneradosView.Informe) formulario = " - Informe";
            else formulario = " - Consulta";
            Log.Info("FIN Diario Detallado" + formulario);
        }

        private void radButtonActualizarLista_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.FillDataGrid();
            Cursor.Current = Cursors.Default;
        }

        private void radButtonActualizarLista_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonActualizarLista);
        }

        private void radButtonActualizarLista_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonActualizarLista);
        }

        private void radGridViewInfConsGenerados_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir todos los literales del Formulario
            switch (this.ficherosGeneradosViewTipo)
            {
                case FicherosGeneradosView.Informe:
                    this.radLabelTitulo.Text = "Informes / Visualizar";
                    break;
                case FicherosGeneradosView.Consulta:
                    this.radLabelTitulo.Text = "Consultas / Visualizar";
                    break;
            }
        }

        /// <summary>
        /// Construir el control de la Grid que contiene los ficheros generados
        /// </summary>
        private void BuilGridInfConFicheroGeneradoView()
        {
            //Adicionar las columnas para el DataTable para la Grid
            this.radGridViewInfConsGenerados.Columns.Add("Archivo");

            GridViewDateTimeColumn dateTimeColumn = new GridViewDateTimeColumn()
            {
                Name = "Fecha",
                HeaderText = "Fecha",
                FieldName = "Fecha",
                FormatString = "{0:D}"
            };//FormatString = "{0:D}"  //FormatString = "{ 0:dd/MM/yyyy}"
            this.radGridViewInfConsGenerados.Columns.Add(dateTimeColumn);

            this.radGridViewInfConsGenerados.Columns.Add("Tipo");
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>
                {
                    { "Archivo", "Archivo" },
                    { "Fecha", "Fecha" },
                    { "Tipo", "Tipo" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Carga los datos de las Solicitudes en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                radGridViewInfConsGenerados.Rows.Clear();

                this.pathFicheros = "";
                string[] extensionFicheros = null;

                int[] array = new int[] { 3, 4 };

                switch (this.ficherosGeneradosViewTipo)
                {
                    case FicherosGeneradosView.Informe:
                        this.pathFicheros = GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosInformes;
                        extensionFicheros = new string[] { ".html" };
                        break;
                    case FicherosGeneradosView.Consulta:
                        this.pathFicheros = GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosConsultas;
                        extensionFicheros = new string[] { ".html", ".xlsx", ".pdf" };
                        break;
                    default:
                        this.pathFicheros = GlobalVar.UsuarioEnv.ModConsInfo_PathFicherosInformes;
                        extensionFicheros = new string[] { ".html" };
                        break;
                }

                //Leer todos los ficheros del directorio que corresponda
                DirectoryInfo dir = new DirectoryInfo(pathFicheros);
                FileInfo[] files = dir.EnumerateFiles()
                    .Where(f => extensionFicheros.Contains(f.Extension.ToLower()))
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToArray();

                GridViewRowInfo row;
                string nombreFichero = "";

                foreach (FileInfo FI in files)
                {
                    row = this.radGridViewInfConsGenerados.Rows.NewRow();
                    nombreFichero = FI.Name;
                    row.Cells["Archivo"].Value = nombreFichero;
                    //row.Cells["Fecha"].Value = FI.CreationTime;
                    row.Cells["Fecha"].Value = FI.LastWriteTime;
                    if (this.ficherosGeneradosViewTipo == FicherosGeneradosView.Consulta) row.Cells["Tipo"].Value = "";
                    else row.Cells["Tipo"].Value = this.InformeTipo(nombreFichero);
                    this.radGridViewInfConsGenerados.Rows.Add(row);
                }

                if (this.radGridViewInfConsGenerados.Rows.Count > 0)
                {
                    this.radGridViewInfConsGenerados.Visible = true;
                    this.RadGridViewHeader();
                    this.radLabelNoHayInfo.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonView, true);
                    utiles.ButtonEnabled(ref this.radButtonEliminar, true);

                    for (int i = 0; i < this.radGridViewInfConsGenerados.Columns.Count; i++)
                    {
                        this.radGridViewInfConsGenerados.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewInfConsGenerados.Columns[i].Width = 600;
                    }

                    this.radGridViewInfConsGenerados.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewInfConsGenerados.AllowSearchRow = true;
                    this.radGridViewInfConsGenerados.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewInfConsGenerados.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewInfConsGenerados.AllowEditRow = false;
                    this.radGridViewInfConsGenerados.EnableFiltering = true;

                    //SortDescriptor descriptorFecha = new SortDescriptor
                    //{
                    //    PropertyName = "Fecha",
                    //    Direction = ListSortDirection.Descending
                    //};
                    //this.radGridViewInfConsGenerados.MasterTemplate.SortDescriptors.Add(descriptorFecha);

                    this.radGridViewInfConsGenerados.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewInfConsGenerados.Rows[0].IsCurrent = true;
                    this.radGridViewInfConsGenerados.Focus();
                    this.radGridViewInfConsGenerados.Select();

                    this.radGridViewInfConsGenerados.Refresh();

                    /*
                    SortDescriptor descriptorArchivo = new SortDescriptor
                    {
                        PropertyName = "Archivo",
                        Direction = ListSortDirection.Ascending
                    };
                    this.radGridViewInfConsGenerados.MasterTemplate.SortDescriptors.Add(descriptorArchivo);
                    */

                    

                    /* Ordenar por 2 campos
                       SortDescriptor descriptorShipName = new SortDescriptor();
                       descriptorShipName.PropertyName = "ShipName";
                       descriptorShipName.Direction = ListSortDirection.Ascending;
                       SortDescriptor descriptorFreight = new SortDescriptor();
                       descriptorFreight.PropertyName = "Freight";
                       descriptorFreight.Direction = ListSortDirection.Descending;
                       this.radGridView1.SortDescriptors.Add(descriptorShipName);
                       this.radGridView1.SortDescriptors.Add(descriptorFreight);
                     */
                }
                else
                {
                    this.radGridViewInfConsGenerados.Visible = false;
                    this.radLabelNoHayInfo.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonView, false);
                    utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewInfConsGenerados.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewInfConsGenerados.Columns.Contains(item.Key)) this.radGridViewInfConsGenerados.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Visualizar el informe/consulta
        /// </summary>
        private void FileView()
        {
            if (this.radGridViewInfConsGenerados.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un archivo", "Error");  //Falta traducir
                return;
            }

            if (this.radGridViewInfConsGenerados.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewInfConsGenerados.CurrentRow.IsExpanded) this.radGridViewInfConsGenerados.CurrentRow.IsExpanded = false;
                else this.radGridViewInfConsGenerados.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewInfConsGenerados.CurrentRow is GridViewDataRowInfo)
            {
                if (this.radGridViewInfConsGenerados.CurrentRow == null)
                {
                    RadMessageBox.Show("Debe seleccionar un fichero");
                    return;
                }

                int indice = this.radGridViewInfConsGenerados.Rows.IndexOf(this.radGridViewInfConsGenerados.CurrentRow);
                string archivo = this.radGridViewInfConsGenerados.Rows[indice].Cells["Archivo"].Value.ToString();

                string fichero = this.pathFicheros;

                if (fichero.Length > 1 && fichero.Substring(fichero.Length - 1) != "\\") fichero += @"\";
                fichero += archivo;

                try
                {
                    if (this.ficherosGeneradosViewTipo == FicherosGeneradosView.Informe)
                    {
                        //Si es informe abrirlo con el tipo de fichero por defecto
                        if (GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes == "HTML") this.FileViewHTML(fichero);
                        else this.FileViewEXCEL(fichero);
                    }
                    else
                    {
                        //Abrirlrlo con el visor que corresponda
                        string extension = "";
                        int posPunto = fichero.IndexOf('.');

                        if (posPunto != -1)
                        {
                            extension = fichero.Substring(posPunto + 1, fichero.Length - posPunto-1);
                            switch (extension)
                            {
                                case "html":
                                    this.FileViewHTML(fichero); 
                                    break;
                                case "xlsx":
                                    this.FileViewEXCEL(fichero);
                                    break;
                                case "pdf":
                                    this.FileViewPDF(fichero);
                                    break;
                                default:
                                    //Abrirlo con el tipo de fichero por defecto
                                    if (GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosConsultas == "HTML") this.FileViewHTML(fichero);
                                    else this.FileViewEXCEL(fichero);
                                    break;
                            }
                        }
                        else
                        {
                            //Abrirlo con el tipo de fichero por defecto
                            if (GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosConsultas == "HTML") this.FileViewHTML(fichero);
                            else this.FileViewEXCEL(fichero);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    RadMessageBox.Show("El fichero no existe o no ha sido posible cargarlo. Para más información consulte el fichero de Log.");
                }
            }
        }

        /// <summary>
        /// Visualizar el informe/consulta en EXCEL
        /// </summary>
        private void FileViewEXCEL(string fichero)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "EXCEL",
                    Arguments = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };
                Process.Start(startInfo);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Visualizar el informe/consulta en HTML
        /// </summary>
        private void FileViewHTML(string fichero)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };

                Process.Start(startInfo);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Visualizar el informe/consulta en PDF
        /// </summary>
        private void FileViewPDF(string fichero)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                };
                Process.Start(startInfo);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void FilesDelete()
        {
            try
            {
                if (this.radGridViewInfConsGenerados.SelectedRows.Count > 0)
                {
                    //Pedir confirmación
                    DialogResult result = RadMessageBox.Show("Se van a eliminar los archivos seleccionados. ¿Desea continuar?", this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result != DialogResult.Yes) return;

                    string ficheroPath = this.pathFicheros;
                    if (ficheroPath.Length > 1 && ficheroPath.Substring(ficheroPath.Length - 1) != "\\") ficheroPath += @"\";

                    string archivo = "";

                    GridViewDataRowInfo[] rows = new GridViewDataRowInfo[this.radGridViewInfConsGenerados.SelectedRows.Count];
                    this.radGridViewInfConsGenerados.SelectedRows.CopyTo(rows, 0);

                    this.radGridViewInfConsGenerados.BeginUpdate();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        //Eliminar el fichero seleccionado
                        archivo = ficheroPath + rows[i].Cells["Archivo"].Value.ToString();

                        try
                        {
                            System.IO.File.Delete(archivo);
                            this.radGridViewInfConsGenerados.Rows.Remove(rows[i]);
                        }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    this.radGridViewInfConsGenerados.EndUpdate();

                    if (this.radGridViewInfConsGenerados.Rows.Count == 0)
                    {
                        this.radGridViewInfConsGenerados.Visible = false;
                        this.radLabelNoHayInfo.Visible = true;
                        utiles.ButtonEnabled(ref this.radButtonView, false);
                        utiles.ButtonEnabled(ref this.radButtonEliminar, false);
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve el nombre del tipo de informe a partir del codigo que se enuentra en el nombre del archivo
        /// </summary>
        /// <param name="nombreFichero"></param>
        /// <returns></returns>
        private string InformeTipo(string nombreFichero)
        {
            string tipo = "";

            try
            {
                string codigoNombreFichero = "";
                int posUnd = nombreFichero.IndexOf('_');
                if (posUnd > 0) codigoNombreFichero = nombreFichero.Substring(0, posUnd);

                if (codigoNombreFichero != "")
                {
                    switch (codigoNombreFichero)
                    {
                        case "DIARIOD":
                            tipo = "Diario Detallado";
                            break;
                        case "DIARIOP":
                            tipo = "Diario resumido por periodo";
                            break;
                        case "DIARIOF":
                            tipo = "Diario resumido por fecha";
                            break;
                        case "INFGENE":
                            tipo = "Informe generado";
                            break;
                        case "BASUMSA":
                            tipo = "Balance de sumas y saldos";
                            break;
                        case "MAYCONT":
                            tipo = "Mayor de contabilidad";
                            break;
                        case "MOVAUXI":
                            tipo = "Movimientos de auxiliar";
                            break;
                        case "MOVIIVA":
                            tipo = "Movimientos de IVA";
                            break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (tipo);
        }
        #endregion

    }
}
