using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using OutlookStyleControls;
using ObjectModel;
using Telerik.WinControls;

namespace ModMantenimientos
{
    public partial class frmMtoGLT04 : frmPlantilla, IReLocalizable
    {
        public const string formCode = "MMTOCALEND";

        private bool nuevo;
        private string codigo;
        private int situarseAno;

        private DataSet dsDatos;

        private DataTable dtCalendario;

        // remember the column index that was last sorted on
        private int prevColIndex = -1;
        private int posRegistro = 0;

        // remember the direction the rows were last sorted on (ascending/descending)
        private ListSortDirection prevSortDirection = ListSortDirection.Ascending;

        //se utiliza para actualizar la fecha del último periodo
        private string saappOrigenUltPrd;
        private string fechaIniOrigenUltPrd;
        private string fechaFinOrigenUltPrd;

        public bool Nuevo
        {
            get
            {
                return (this.nuevo);
            }
            set
            {
                this.nuevo = value;
            }
        }

        public string Codigo
        {
            get
            {
                return (this.codigo);
            }
            set
            {
                this.codigo = value;
            }
        }

        public int SituarseAno
        {
            get
            {
                return (this.situarseAno);
            }
            set
            {
                this.situarseAno= value;
            }
        }

        public frmMtoGLT04()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbPeriodoCopiar.ElementTree.EnableApplicationThemeName = false;
            this.gbPeriodoCopiar.ThemeName = "ControlDefault";

            this.gbPeriodoEdicion.ElementTree.EnableApplicationThemeName = false;
            this.gbPeriodoEdicion.ThemeName = "ControlDefault";

            this.gbPeriodoFechaFinUltPrd.ElementTree.EnableApplicationThemeName = false;
            this.gbPeriodoFechaFinUltPrd.ThemeName = "ControlDefault";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmMtoGLT04_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Mantenimiento de Calendarios Contables Alta/Edita");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.dtCalendario = new DataTable();

            //OutlookGrid
            this.BuildOutlookGrid();
            // invoke the outlook style
            MenuSkinOutlook_Click(sender, e);
            
            if (this.nuevo)
            {
                //Inactivar los controles hasta que introduzcan un codigo valido
                this.oGridPeriodos.Enabled = false;
                utiles.ButtonEnabled(ref this.radButtonAdicionarPrd, false);
                utiles.ButtonEnabled(ref this.radButtonCopiarPrd, false);
                utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);
                utiles.ButtonEnabled(ref this.radButtonFechaFinUltPrd, false);

                this.gbPeriodoEdicion.Enabled = false;
                utiles.ButtonEnabled( ref this.radButtonGrabarPeriodo, false);

                this.oGridPeriodos.Visible = false;

                this.gbPeriodoEdicion.Location = new Point(135, 92);
                this.Size = new System.Drawing.Size(this.Size.Width - 100, this.Size.Height - 300);

                this.ActiveControl = this.txtCodigo;
                this.txtCodigo.Select(0, 0);
                this.txtCodigo.Focus();
            }
            else
            {
                this.txtCodigo.Text = codigo;
                this.txtCodigo.IsReadOnly = true;
                this.oGridPeriodos.Enabled = true;

                utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);

                //Recuperar la información del calendario y lo muestra en los controles
                this.FillOutlookGrid();

                ListSortDirection direction = ListSortDirection.Ascending;

                // remember the column that was clicked and in which direction is ordered
                prevColIndex = 0;
                prevSortDirection = direction;

                // set the column to be grouped
                this.oGridPeriodos.GroupTemplate.Column = this.oGridPeriodos.Columns[0];

                this.oGridPeriodos.GroupTemplate.ItemName = "periodo";      //Falta traducir
                this.oGridPeriodos.GroupTemplate.ItemsName = "periodos";    //Falta traducir

                //sort the grid
                //this.oGridPeriodos.Sort(new DataRowComparer(0, direction));
                this.oGridPeriodos.Sort(new PeriodoInfoComparer(0, direction));

                this.oGridPeriodos.CollapseAll();

                //this.dgPeriodos.ClearSelection();

                this.gbPeriodoEdicion.Visible = false;
            }

            //Traducir los literales
            this.TraducirLiterales();

            this.gbPeriodoCopiar.Visible = false;
            this.gbPeriodoFechaFinUltPrd.Visible = false;
        }
        
        private void FrmMtoGLT04_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonExit_Click(sender, null); 
        }
        
        private void MenuSkinOutlook_Click(object sender, EventArgs e)
        {
            this.oGridPeriodos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.SystemColors.Window,
                //dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ForeColor = System.Drawing.SystemColors.ControlText,
                SelectionBackColor = System.Drawing.SystemColors.Highlight,
                SelectionForeColor = System.Drawing.SystemColors.HighlightText
            };
            this.oGridPeriodos.DefaultCellStyle = dataGridViewCellStyle2;
            this.oGridPeriodos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle
            {
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
                BackColor = System.Drawing.SystemColors.Desktop,
                //dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ForeColor = System.Drawing.SystemColors.WindowText,
                SelectionBackColor = System.Drawing.SystemColors.Highlight,
                SelectionForeColor = System.Drawing.SystemColors.HighlightText
            };
            this.oGridPeriodos.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            this.oGridPeriodos.GridColor = System.Drawing.SystemColors.Control;
            this.oGridPeriodos.RowTemplate.Height = 19;
            this.oGridPeriodos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.oGridPeriodos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.oGridPeriodos.RowHeadersVisible = false;
            this.oGridPeriodos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.oGridPeriodos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.oGridPeriodos.AllowUserToAddRows = false;
            this.oGridPeriodos.AllowUserToDeleteRows = false;
            this.oGridPeriodos.AllowUserToResizeRows = false;
            this.oGridPeriodos.EditMode = DataGridViewEditMode.EditProgrammatically;
            //this.oGridPeriodos.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.oGridPeriodos.ClearGroups(); // reset
        }

        // this event is called when the user clicks on a cell
        // in this particular case we check to see if one of the column headers
        // was clicked. If so, the grid will be sorted based on the clicked column.
        // Note: this handler is not implemented optimally. It is merely used for demonstration purposes
        private void OGridPeriodos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.gbPeriodoEdicion.Visible = false;
            this.gbPeriodoCopiar.Visible = false;
            this.gbPeriodoFechaFinUltPrd.Visible = false;

            if (e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    ListSortDirection direction = ListSortDirection.Ascending;
                    if (e.ColumnIndex == prevColIndex) // reverse sort order
                        direction = prevSortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                    // remember the column that was clicked and in which direction is ordered
                    prevColIndex = e.ColumnIndex;
                    prevSortDirection = direction;

                    switch (e.ColumnIndex)
                    {
                        case 0:
                            // set the column to be grouped
                            this.oGridPeriodos.GroupTemplate.Column = this.oGridPeriodos.Columns[e.ColumnIndex];

                            this.oGridPeriodos.GroupTemplate.ItemName = "periodo";      //Falta traducir
                            this.oGridPeriodos.GroupTemplate.ItemsName = "periodos";    //Falta traducir

                            //sort the grid
                            //this.oGridPeriodos.Sort(new DataRowComparer(e.ColumnIndex, direction));
                            this.oGridPeriodos.Sort(new PeriodoInfoComparer(e.ColumnIndex, direction));

                            this.oGridPeriodos.CollapseAll();
                            break;
                        case 1:
                            // set the column to be grouped
                            this.oGridPeriodos.GroupTemplate.Column = this.oGridPeriodos.Columns[e.ColumnIndex];

                            this.oGridPeriodos.GroupTemplate.ItemName = "periodo";      //Falta traducir
                            this.oGridPeriodos.GroupTemplate.ItemsName = "periodos";    //Falta traducir

                            //sort the grid
                            //this.oGridPeriodos.Sort(new DataRowComparer(e.ColumnIndex, direction));
                            this.oGridPeriodos.Sort(new PeriodoInfoComparer(e.ColumnIndex, direction));
                            this.oGridPeriodos.CollapseAll();
                            break;
                        case 2:
                            IOutlookGridGroup prevGroup = this.oGridPeriodos.GroupTemplate;

                            this.oGridPeriodos.GroupTemplate = null;
                            this.oGridPeriodos.Sort(new PeriodoInfoComparer(e.ColumnIndex, direction));

                            //after sorting, reset the GroupTemplate back to its default (if it was changed)
                            // this is needed just for this demo. We do not want the other
                            // columns to be grouped alphabetically.
                            this.oGridPeriodos.GroupTemplate = prevGroup;
                            break;
                        case 3:
                            IOutlookGridGroup prevGroup1 = this.oGridPeriodos.GroupTemplate;

                            this.oGridPeriodos.GroupTemplate = null;
                            this.oGridPeriodos.Sort(new PeriodoInfoComparer(e.ColumnIndex, direction));

                            //after sorting, reset the GroupTemplate back to its default (if it was changed)
                            // this is needed just for this demo. We do not want the other
                            // columns to be grouped alphabetically.
                            this.oGridPeriodos.GroupTemplate = prevGroup1;
                            break;
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            else
            {
                this.nuevo = false;
                this.txtAno.IsReadOnly = true;
                this.txtPeriodo.IsReadOnly = true;

                try
                {
                    DataGridViewCell celdaActiva = this.oGridPeriodos.CurrentCell;
                    this.oGridPeriodos.Rows[celdaActiva.RowIndex].Selected = true;

                    this.gbPeriodoEdicion.Visible = true;
                    this.gbPeriodoEdicion.Text = "Periodo";
                    this.gbPeriodoCopiar.Visible = false;
                    this.gbPeriodoFechaFinUltPrd.Visible = false;

                    this.txtAno.Text = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["AA"].Value.ToString();
                    this.txtAno.Tag = this.txtAno.Text;
                    this.txtPeriodo.Text = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["PP"].Value.ToString();
                    this.txtPeriodo.Tag = this.txtPeriodo.Text;
                    this.txtMaskFechaIni.Text = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["FechaIni"].Value.ToString();
                    this.txtMaskFechaIni.Tag = this.txtMaskFechaIni.Text;

                    utiles.ButtonEnabled(ref this.radButtonEliminarPrd, true);
                    this.nuevo = false;
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    /*string error = ex.Message;
                    this.txtAno.Text = "";
                    this.txtPeriodo.Text = "";
                    this.txtMaskFechaIni.Text = "";
                    this.txtAno.Tag = "";
                    this.txtPeriodo.Tag = "";
                    this.txtMaskFechaIni.Tag = "";*/

                    this.gbPeriodoEdicion.Visible = false;
                }
            }
        }

        private void TxtAno_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtAno, false, ref sender, ref e);
        }

        private void TxtPeriodo_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtPeriodo, false, ref sender, ref e);
        }

        private void TxtAnoOrigen_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtAnoOrigen, false, ref sender, ref e);

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.radButtonCopiarPrd.PerformClick();
            }
        }

        private void TxtAnoDestino_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtAnoDestino, false, ref sender, ref e);

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.radButtonCopiarPrd.PerformClick();
            }
        }

        private void TxtAnoUltPrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtAnoUltPrd, false, ref sender, ref e);
        }

        private void TxtPeriodoUltPrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtPeriodoUltPrd, false, ref sender, ref e);
        }

        private void TxtMaskFechaIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.radButtonGrabarPeriodo.PerformClick();
            }
        }

        private void TxtMaskFechaFinUltPrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                this.radButtonFechaFinUltPrd.PerformClick();
            }
        }

        private void RadButtonFechaFinUltPrd_Click(object sender, EventArgs e)
        {
            this.FechaFinUltPrd();
        }

        private void RadButtonAdicionarPrd_Click(object sender, EventArgs e)
        {
            this.AdicionarPrd();
        }

        private void RadButtonCopiarPrd_Click(object sender, EventArgs e)
        {
            this.CopiarPrd();
        }

        private void RadButtonEliminarPrd_Click(object sender, EventArgs e)
        {
            this.EliminarPrd();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonGrabarPeriodo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.FormValid())
            {
                string result = "";
                this.posRegistro = 0;

                if (this.nuevo)
                {
                    result = this.AltaInfo();
                    if (result == "")
                    {
                        //this.nuevo = true;
                        this.codigo = this.txtCodigo.Text.Trim();
                        this.posRegistro = this.oGridPeriodos.Rows.Count - 1;    //FaltaBuscarlo

                        //Habilitar Controles
                        utiles.ButtonEnabled(ref this.radButtonAdicionarPrd, true);
                        utiles.ButtonEnabled(ref this.radButtonCopiarPrd, true);
                        utiles.ButtonEnabled(ref this.radButtonEliminarPrd, true);
                        utiles.ButtonEnabled(ref this.radButtonFechaFinUltPrd, true);

                        if (!this.oGridPeriodos.Visible)
                        {
                            this.gbPeriodoEdicion.Location = new Point(this.oGridPeriodos.Location.X + this.oGridPeriodos.Size.Width + 30, 85);
                            this.Size = new System.Drawing.Size(this.Size.Width + 100, this.Size.Height + 300);
                        }
                    }
                }
                else
                {
                    DataGridViewCell celdaActiva = this.oGridPeriodos.CurrentCell;
                    result = this.ActualizarInfo(celdaActiva);
                    this.posRegistro = celdaActiva.RowIndex;

                    if (result == "") this.gbPeriodoEdicion.Visible = false;
                }

                if (result != "")
                {
                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(result, error);
                }
                else
                {
                    //Actualizar los valores originales de los controles
                    //this.ActualizaValoresOrigenControles();

                    //Actualiza el listado de periodos
                    this.ActualizarListaPeriodos();

                    this.txtAno.Text = "";
                    this.txtPeriodo.Text = "";
                    this.txtMaskFechaIni.Text = "";

                    if (this.nuevo) this.txtAno.Focus();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonCopiarPeriodo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            IDataReader dr = null;

            try
            {
                if (this.FormValidCopiarPeriodos())
                {
                    string aaOrigen = this.txtAnoOrigen.Text.Substring(2, 2);
                    string sigloOrigen = utiles.SigloDadoAnno(aaOrigen, CGParametrosGrles.GLC01_ALSIRC);
                    string saaOrigen = sigloOrigen + aaOrigen;

                    string aaDestino = this.txtAnoDestino.Text.Substring(2, 2);
                    int aDestinoInt = Convert.ToInt16(this.txtAnoDestino.Text);

                    string valorI = saaOrigen + "00";
                    string valorF = saaOrigen + "99";
                    //Obtener los periodos del origen
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + this.codigo + "' and SAPRFL > " + valorI + " and SAPRFL <= " + valorF;
                    query += " order by SAPRFL";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    string saprfl = "";
                    string inlafl = "";
                    string finlfl = "";

                    string siglo = "0";
                    if (aDestinoInt >= 2000) siglo = "1";

                    string saprflDestino = "";
                    string inlaflDestino = "";
                    string finlflDestino = "";

                    string saaDestino = siglo + aaDestino;
                    string periodo = "";
                    DateTime dtFechaIniOriginal;
                    DateTime dtFechaFinOriginal;
                    string resultAux = "";

                    bool primerReg = true;
                    bool ejercicioPosterior = false;

                    int anoDiff = Convert.ToInt32(this.txtAnoDestino.Text) - Convert.ToInt32(this.txtAnoOrigen.Text);
                    string saaDestinoFechaIni = "";
                    string saaDestinoFechaFin = "";
                    string saaDestinoFechaAux = "";

                    string aaDestinoFechaIni4Digitos = "";
                    string aaDestinoFechaFin4Digitos = "";
                    string aaDestinoFecha4DigitosResult = "";

                    bool anoOrigenBisiesto = false;
                    bool anoDestinoBisiesto = false;

                    while (dr.Read())
                    {
                        saprfl = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString();
                        inlafl = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                        finlfl = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();

                        if (saprfl.Length >= 2) periodo = saprfl.Substring(saprfl.Length - 2, 2);
                        else periodo = saprfl;

                        dtFechaIniOriginal = utiles.FormatoCGToFecha(inlafl);
                        dtFechaFinOriginal = utiles.FormatoCGToFecha(finlfl);

                        saprflDestino = saaDestino + periodo;   //Esto es para el periodo
                        //calcular el saaDestinoFechaIni y ssaaDestinoFechaFinal
                        saaDestinoFechaAux = inlafl.PadLeft(7, '0');
                        saaDestinoFechaIni = (Convert.ToInt32(saaDestinoFechaAux.Substring(0, 3)) + anoDiff).ToString();
                        saaDestinoFechaAux = finlfl.PadLeft(7, '0');
                        saaDestinoFechaFin = (Convert.ToInt32(saaDestinoFechaAux.Substring(0, 3)) + anoDiff).ToString();


                        //Comprobar si la fecha de inicio tiene como mes 02, si es un año bisiesto
                        if (dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') == "02")
                        {
                            //Comprobar si el año origen es bisiesto
                            anoOrigenBisiesto = DateTime.IsLeapYear(dtFechaIniOriginal.Date.Year);
                            if (anoOrigenBisiesto)
                            {
                                aaDestinoFechaIni4Digitos = saaDestinoFechaIni.Substring(1, 2);
                                aaDestinoFecha4DigitosResult = this.CompletarAno4Posiciones(ref aaDestinoFechaIni4Digitos);

                                //Comprobar si el año origen es bisiesto
                                anoDestinoBisiesto = DateTime.IsLeapYear(Convert.ToInt32(aaDestinoFechaIni4Digitos));

                                switch (dtFechaIniOriginal.Day.ToString())
                                {
                                    case "29":
                                        if (!anoDestinoBisiesto) inlaflDestino = saaDestinoFechaIni + dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') + (dtFechaIniOriginal.Day - 1);
                                        else inlaflDestino = saaDestinoFechaIni + dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaIniOriginal.Day.ToString().PadLeft(2, '0');
                                        break;
                                    default:
                                        inlaflDestino = saaDestinoFechaIni + dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaIniOriginal.Day.ToString().PadLeft(2, '0');
                                        break;
                                }
                            }
                            else inlaflDestino = saaDestinoFechaIni + dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaIniOriginal.Day.ToString().PadLeft(2, '0');
                        }
                        else inlaflDestino = saaDestinoFechaIni + dtFechaIniOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaIniOriginal.Day.ToString().PadLeft(2, '0');


                        //Comprobar si la fecha de fin tiene como mes 02, si es un año bisiesto
                        if (dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') == "02")
                        {
                            //Comprobar si el año origen es bisiesto
                            anoOrigenBisiesto = DateTime.IsLeapYear(dtFechaFinOriginal.Date.Year);

                            aaDestinoFechaFin4Digitos = saaDestinoFechaFin.Substring(1, 2);
                            aaDestinoFecha4DigitosResult = this.CompletarAno4Posiciones(ref aaDestinoFechaFin4Digitos);

                            //Comprobar si el año origen es bisiesto
                            anoDestinoBisiesto = DateTime.IsLeapYear(Convert.ToInt32(aaDestinoFechaFin4Digitos));

                            if (anoOrigenBisiesto)
                            {
                                switch (dtFechaFinOriginal.Day.ToString())
                                {
                                    case "29":
                                        if (!anoDestinoBisiesto) finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + (dtFechaFinOriginal.Day - 1);
                                        else finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaFinOriginal.Day.ToString().PadLeft(2, '0');
                                        break;
                                    default:
                                        finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaFinOriginal.Day.ToString().PadLeft(2, '0');
                                        break;
                                }
                            }
                            else
                            {
                                if (anoDestinoBisiesto)
                                {
                                    switch (dtFechaFinOriginal.Day.ToString())
                                    {
                                        case "28":
                                            finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + (dtFechaFinOriginal.Day + 1);
                                            break;
                                        default:
                                            finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaFinOriginal.Day.ToString().PadLeft(2, '0');
                                            break;
                                    }
                                }
                                else finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaFinOriginal.Day.ToString().PadLeft(2, '0');
                            }
                        }
                        else finlflDestino = saaDestinoFechaFin + dtFechaFinOriginal.Date.Month.ToString().PadLeft(2, '0') + dtFechaFinOriginal.Day.ToString().PadLeft(2, '0');

                        //Validar si existe un ejercicio posterior al que intentamos crear o 
                        //Fecha destino esta incluida en un periodo existene
                        if (primerReg)
                        {
                            primerReg = false;
                            ejercicioPosterior = ExisteEjercicioPosteriorDestino(inlaflDestino);
                            if (ejercicioPosterior)
                            {
                                RadMessageBox.Show("Ya existe una fecha posterior al destino", this.LP.GetText("errValCodCalendario", "Error"));  //Falta traducir
                                break;
                            }
                        }

                        //Insertar el Periodo
                        resultAux = this.InsertarPeriodoGLT04(this.codigo, saprflDestino, inlaflDestino, finlflDestino);
                        //resultAux = this.InsertarPeriodo(this.codigo, saprflDestino, Convert.ToInt32(inlaflDestino), false);
                    }

                    dr.Close();

                    //Actualiza el listado de periodos
                    this.ActualizarListaPeriodos();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                RadMessageBox.Show("Error copiando periodos (" + ex.Message + ")", this.LP.GetText("errValCodCalendario", "Error"));  //Falta traducir
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonGrabarFechaFinUltPrd_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string error = this.LP.GetText("errValTitulo", "Error");

                int fechaFinalUltPerInt;

                if (this.txtMaskFechaFinUltPrd.Text != "99/99/9999")
                {
                    DateTime dtFechaFinalUltPer = new DateTime();
                    string result = FormValidFechaUltPrd(ref dtFechaFinalUltPer);
                    if (result == "")
                    {
                        fechaFinalUltPerInt = utiles.FechaToFormatoCG(dtFechaFinalUltPer, true);
                    }
                    else
                    {
                        RadMessageBox.Show(result, error);
                        return;
                    }

                }
                else fechaFinalUltPerInt = 9999999;

                //Actualizar la fecha final del registro del periodo anterior
                string query = this.ObtenerUpdateFechaFin(this.codigo, this.saappOrigenUltPrd, this.fechaIniOrigenUltPrd, this.fechaFinOrigenUltPrd, fechaFinalUltPerInt.ToString());

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Actualiza el listado de periodos
                this.ActualizarListaPeriodos();

                this.gbPeriodoFechaFinUltPrd.Visible = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show("Error actualizando la fecha final del último periodo (" + ex.Message + ")", error); //Falta traducir
            }

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonFechaFinUltPrd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonFechaFinUltPrd);
        }

        private void RadButtonFechaFinUltPrd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonFechaFinUltPrd);
        }

        private void RadButtonAdicionarPrd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAdicionarPrd);
        }

        private void RadButtonAdicionarPrd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAdicionarPrd);
        }

        private void RadButtonCopiarPrd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCopiarPrd);
        }

        private void RadButtonCopiarPrd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCopiarPrd);
        }

        private void RadButtonEliminarPrd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminarPrd);
        }

        private void RadButtonEliminarPrd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminarPrd);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadButtonGrabarPeriodo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabarPeriodo);
        }

        private void RadButtonGrabarPeriodo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabarPeriodo);
        }

        private void RadButtonCopiarPeriodo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCopiarPeriodo);
        }

        private void RadButtonCopiarPeriodo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCopiarPeriodo);
        }

        private void RadButtonGrabarFechaFinUltPrd_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGrabarFechaFinUltPrd);
        }

        private void RadButtonGrabarFechaFinUltPrd_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGrabarFechaFinUltPrd);
        }

        private void FrmMtoGLT04_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Mantenimiento de Calendarios Contables Alta/Edita");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Escribe los literales del formulario en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            if (this.nuevo) this.Text = "   " + this.LP.GetText("lblfrmMtoGLT04TituloALta", "Mantenimiento de Calendarios Contables - Alta");   //Falta traducir
            else this.Text = "   " + this.LP.GetText("lblfrmMtoGLT04TituloEdit", "Mantenimiento de Calendarios Contables - Edición");           //Falta traducir

            this.radButtonFechaFinUltPrd.Text = "Fecha Final \n Último Periodo";
            this.radButtonAdicionarPrd.Text = this.LP.GetText("toolStripAdicionarPrd", "Adicionar Periodo");
            this.radButtonCopiarPrd.Text = this.LP.GetText("toolStripCopiarPrd", "Copiar Periodos");
            this.radButtonEliminarPrd.Text = this.LP.GetText("toolStripEliminarPrd", "Eliminar Periodos");
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Cancelar");
        }

        /// <summary>
        /// Construir el DataGrid de periodos
        /// </summary>
        private void BuildOutlookGrid()
        {
            this.dsDatos = new DataSet();
            dsDatos.DataSetName = "InfoCalendario";

            DataTable dt = new DataTable
            {
                TableName = "Periodo"
            };

            //Adicionar la columna al DataTable
            dt.Columns.Add("AA", typeof(string));
            dt.Columns.Add("PP", typeof(string));
            dt.Columns.Add("FechaIni", typeof(string));
            dt.Columns.Add("FechaFin", typeof(string));
            //dt.Columns.Add("Eliminar", typeof(Image));
            //Columnas no visibles
            dt.Columns.Add("AAPPOrigen", typeof(string));
            dt.Columns.Add("FechaIniOrigen", typeof(string));
            dt.Columns.Add("FechaFinOrigen", typeof(string));

            //Adicionar el DataTable al DataSet del DataGrid
            dsDatos.Tables.Add(dt);
        }

        /// <summary>
        /// Llenar el DataGrid con los periodos
        /// </summary>
        private void FillOutlookGrid()
        {
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + this.codigo + "' ";

                if (this.situarseAno != -1)
                {
                    //DUDA como situarse en ano ¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?
                    //query += " and ";
                }

                //query += "order by SAPRFL, INLAFL";
                query += "order by SAPRFL";

                this.dtCalendario = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (this.dtCalendario != null && this.dtCalendario.Rows.Count > 0)
                {
                    DataRow row;
                    
                    for (int i = 0; i < this.dtCalendario.Rows.Count; i++)
                    {
                        //Fila nueva de la Grid
                        row = this.dsDatos.Tables["Periodo"].NewRow();

                        //Dado una fila en la tabla de periodos, devuelve una fila para la Grid (con las celdas que espera la Grid)
                        this.ObtenerFilaGrid(this.dtCalendario.Rows[i], ref row);

                        //Adicionar la fila a la Grid
                        this.dsDatos.Tables["Periodo"].Rows.Add(row);
                    }

                    try
                    {
                        this.oGridPeriodos.BindData(dsDatos, "Periodo");

                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    //example of overriding the databound column header texts
                    this.oGridPeriodos.Columns["AA"].HeaderText = "Año";
                    this.oGridPeriodos.Columns["PP"].HeaderText = "Periodo";
                    this.oGridPeriodos.Columns["FechaIni"].HeaderText = "Fecha inicio";
                    this.oGridPeriodos.Columns["FechaFin"].HeaderText = "Fecha fin";

                    // example of hiding columns
                    this.oGridPeriodos.Columns["AAPPOrigen"].Visible = false;
                    this.oGridPeriodos.Columns["FechaIniOrigen"].Visible = false;
                    this.oGridPeriodos.Columns["FechaFinOrigen"].Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Dado un registro de la tabla de periodos devuelve una fila de la Grid
        /// </summary>
        /// <param name="rowBBDD"></param>
        /// <param name="rowGrid"></param>
        private void ObtenerFilaGrid(DataRow rowBBDD, ref DataRow rowGrid)
        {
            try
            {
                rowGrid = this.dsDatos.Tables["Periodo"].NewRow();

                string saapp = rowBBDD["SAPRFL"].ToString();
                saapp = saapp.PadLeft(5, '0');

                int siglo = 1;
                if (saapp.Length == 4) siglo = 0;
                else if (saapp.Length == 5)
                {
                    string valorSiglo = saapp.Substring(0, 1);
                    if (valorSiglo == "0") siglo = 0;
                }
                string saappOrigen = saapp;
                saapp = utiles.AAPPConFormato(saapp);

                string ano = "";
                int anoI = 1900;
                if (siglo == 1) anoI = 2000;
                try
                {
                    ano = saapp.Substring(0, 2);
                    anoI = Convert.ToInt16(ano) + anoI;
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                rowGrid["AA"] = anoI.ToString();

                string periodo = "";
                try
                {
                    periodo = saapp.Substring(3, 2);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                rowGrid["PP"] = periodo;

                string fechaIni = rowBBDD["INLAFL"].ToString();
                fechaIni = fechaIni.PadLeft(7, '0');
                DateTime fechaI = utiles.FormatoCGToFecha(fechaIni);
                rowGrid["FechaIni"] = fechaI.Date.ToShortDateString();

                string fechaFin = rowBBDD["FINLFL"].ToString();
                fechaFin = fechaFin.PadLeft(7, '0');
                if (fechaFin == "9999999")
                {
                    rowGrid["FechaFin"] = "99/99/9999";
                }
                else
                {
                    DateTime fechaF = utiles.FormatoCGToFecha(fechaFin);
                    rowGrid["FechaFin"] = fechaF.Date.ToShortDateString();
                }

                rowGrid["AAPPOrigen"] = saappOrigen;
                rowGrid["FechaIniOrigen"] = fechaIni;
                rowGrid["FechaFinOrigen"] = fechaFin;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Habilitar/Deshabilitar los controles relacionados con el campo del código del calendario (al dar de alta un calendario)
        /// </summary>
        /// <param name="valor"></param>
        private void HabilitarDeshabilitarControles(bool valor)
        {
            this.gbPeriodoEdicion.Enabled = valor;
            this.txtAno.Enabled = valor;
            this.txtPeriodo.Enabled = valor;
            this.txtMaskFechaIni.Enabled = valor;
        }

        /// <summary>
        /// Valida que no exista el código del calendario
        /// </summary>
        /// <returns></returns>
        private bool CodigoCalendarioValido()
        {
            bool result = false;

            try
            {
                string codCalendario = this.txtCodigo.Text.Trim();

                if (codCalendario != "")
                {
                    string query = "select count(TITAFL) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                    query += "where TITAFL = '" + codCalendario + "'";

                    int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                    if (cantRegistros == 0) result = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Completar el año a 4 posiciones
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        private string CompletarAno4Posiciones(ref string ano)
        {
            string result = "";

            try
            {
                if (ano == "") return(result);

                if (ano.Length == 1 || ano.Length == 3)
                {
                    result = "El año destino no tiene un formato correcto.";
                    return (result);
                }

                int anoCorte = Convert.ToInt16(CGParametrosGrles.GLC01_ALSIRC);

                int anoInt = Convert.ToInt32(ano);
                if (anoInt < anoCorte) anoInt = 2000 + anoInt;
                else if (anoInt >= anoCorte) anoInt = 1900 + anoInt;

                ano = anoInt.ToString();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool FormValid()
        {
            bool result = false;
            string errores = "";

            try
            {
                string ano = this.txtAno.Text.Trim() ;
                if (ano == "")
                {
                    errores += "- El año no puede estar en blanco \n\r";      //Falta traducir
                    this.txtAno.Focus();
                }

                if (this.txtPeriodo.Text.Trim() == "")
                {
                    errores += "- El periodo no puede estar en blanco \n\r";      //Falta traducir
                    this.txtPeriodo.Focus();
                }

                this.txtMaskFechaIni.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string fechaIni = this.txtMaskFechaIni.Text.Trim();
                this.txtMaskFechaIni.TextMaskFormat = MaskFormat.IncludeLiterals;

                DateTime dt = new DateTime();

                if (fechaIni == "")
                {
                    errores += "- La fecha de inicio no puede estar en blanco \n\r";      //Falta traducir
                    this.txtMaskFechaIni.Focus();
                }
                else
                {
                    try
                    {
                        dt = Convert.ToDateTime(this.txtMaskFechaIni.Text);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        errores += "- La fecha de inicio no tiene un formato válido";
                        this.txtMaskFechaIni.Focus();
                    }
                }

                //Si el año tiene menos de 4 digitos (completar el año a 4 digitos)
                if (ano.Length < 4)
                {
                    string resultCompletar = this.CompletarAno4Posiciones(ref ano);
                    if (resultCompletar == "") this.txtAno.Text = ano;
                }

                if (errores == "")
                {
                    if (!utiles.EjercicioValido(ano))
                    {
                        errores += "- Ejercicio fuera de años permitidos";  //Falta traducir
                        this.txtAno.Focus();
                    }
                    else
                    {
                        if (!utiles.EjercicioValido(dt.Year.ToString()))
                        {
                            errores += "- Ejercicio fuera de años permitidos";  //Falta traducir
                            this.txtMaskFechaIni.Focus();
                        }
                        else
                        {
                        }

                        if (this.txtAno.Text.Length < 4)
                        {
                            int anoI = 2000;
                            try
                            {
                                anoI = Convert.ToInt16(this.txtAno.Text.Length) + anoI;
                                this.txtAno.Text = anoI.ToString();
                            }
                            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                        }
                    }                    
                }

                if (errores == "") result = true;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                errores += "- Error validando el formulario (" + ex.Message + ") \n\r";   //Falta traducir
            }

            if (errores != "") RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));

            return (result);
        }


        /// <summary>
        /// Dar de alta a un calendario
        /// </summary>
        /// <returns></returns>
        private string AltaInfo()
        {
            string result = "";
            try
            {
                string aa = this.txtAno.Text;
                if (aa.Length < 4) aa.PadLeft(4, '0');
                aa = aa.Substring(2, 2);

                //poner el siglo 1 si es necesario
                string siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                string saprfl = siglo + aa + this.txtPeriodo.Text.PadLeft(2, '0');

                //Chequear que no exista un registro para el mismo año periodo
                if (this.dsDatos != null && this.dsDatos.Tables.Count > 0 && this.dsDatos.Tables["Periodo"].Rows.Count > 0)
                {
                    string periodo = this.txtPeriodo.Text.Trim().PadLeft(2, '0');
                    for (int i = 0; i < this.dsDatos.Tables["Periodo"].Rows.Count; i++)
                    {
                        if (this.dsDatos.Tables["Periodo"].Rows[i]["AA"].ToString() == this.txtAno.Text)
                        {
                            if (this.dsDatos.Tables["Periodo"].Rows[i]["PP"].ToString().PadLeft(2, '0') == periodo)
                            {
                                result = "El periodo ya se encuentra definido en el calendario para el mismo ejercicio";   //Falta traducir
                                this.txtPeriodo.Focus();
                                return (result);
                            }
                        }
                    }
                }

                //fecha con formato CG
                int fechaIniInt = 0;
                DateTime dt;

                dt = Convert.ToDateTime(this.txtMaskFechaIni.Text);
                fechaIniInt = utiles.FechaToFormatoCG(dt, true);

                /*
                bool ejercicioPosterior = ExisteEjercicioPosteriorDestino(fechaIniInt.ToString());
                if (ejercicioPosterior)
                {
                    result = "Ya existe una fecha posterior al destino";  //Falta traducir
                    return (result);
                }
                */

                result = this.InsertarPeriodo(this.txtCodigo.Text, saprfl, fechaIniInt, true);

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Inserta un periodo en la Tabla GLT04
        /// </summary>
        /// <param name="titafl">código del calendario</param>
        /// <param name="saprfl">siglo año periodo del periodo a insertar</param>
        /// <param name="fechaIni">fecha de inicio del periodo a insertar</param>
        /// <param name="actualizarListaPeriodos">true -> actualiza los periodos de la grid con la info de los periodos   false -> no actualiza info</param>
        /// <returns></returns>
        private string InsertarPeriodo(string titafl, string saprfl, int fechaIni, bool actualizarListaPeriodos)
        {
            string result = "";

            try
            {
                //Obtener el periodo posterior para calcular la fecha fin
                int fechaFinInt = 0;     //Calcular la fecha fin
                string saapp = "";
                string fechaIniPeriodoPosterior = "";
                string fechaFinPeriodoPosterior = "";
                result = this.ObtenerPeriodoPosterior(titafl, saprfl, ref saapp, ref fechaIniPeriodoPosterior, ref fechaFinPeriodoPosterior);
                DateTime dt = new DateTime();

                if (result != "")
                {
                    result = "Error obteniendo el periodo posterior (" + result + ")";   //Falta traducir
                    return (result);
                }
                else
                {
                    if (saapp == "-1" && fechaIniPeriodoPosterior == "-1" && fechaFinPeriodoPosterior == "-1")
                    {
                        fechaFinInt = 9999999;
                    }
                    else
                    {
                        //Calcular la fecha final (fecha inicial del periodo posterior menos 1)
                        dt = utiles.FormatoCGToFecha(fechaIniPeriodoPosterior);
                        dt = dt.AddDays(-1);
                        fechaFinInt = utiles.FechaToFormatoCG(dt, true);
                    }
                }

                //Dar de alta al calendario en la tabla del maestro de calendarios (GLT04)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TITAFL, SAPRFL, INLAFL, FINLFL) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + this.txtCodigo.Text + "', " + saprfl + ", " + fechaIni + ", " + fechaFinInt + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Alta, "GLT04", this.txtCodigo.Text, saprfl);

                //--------------- Ajustar fecha fin del registro anterior si es necesario ----------------
                fechaFinInt = 0;     //Calcular la fecha fin
                saapp = "";
                string fechaIniPeriodoAnterior = "";
                string fechaFinPeriodoAnterior = "";
                string resultPeriodoAnterior = this.ObtenerPeriodoAnterior(titafl, saprfl, ref saapp, ref fechaIniPeriodoAnterior, ref fechaFinPeriodoAnterior);

                if (resultPeriodoAnterior == "" && saapp != "-1" && fechaIniPeriodoAnterior != "-1" && fechaIniPeriodoAnterior != "-1")
                {
                    //Calcular la fecha final (fecha inicial del periodo posterior menos 1)
                    dt = utiles.FormatoCGToFecha(fechaIni.ToString());
                    dt = dt.AddDays(-1);
                    fechaFinInt = utiles.FechaToFormatoCG(dt, true);

                    //Actualizar la fecha final del registro del periodo anterior
                    query = this.ObtenerUpdateFechaFin(this.codigo, saapp, fechaIniPeriodoAnterior, fechaFinPeriodoAnterior, fechaFinInt.ToString());

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

                if (actualizarListaPeriodos)
                {
                    //Actualiza el listado de elementos del formulario frmGLT04Sel
                    //this.ActualizarFormularioListaElementos();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }


        /// <summary>
        /// Actualizar un calendario
        /// </summary>
        /// <returns></returns>
        private string ActualizarInfo(DataGridViewCell celdaActiva)
        {
            string result = "";
            try
            {
                string saprfl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["AAPPOrigen"].Value.ToString();
                string inlafl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["FechaIniOrigen"].Value.ToString();
                string finlfl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["FechaFinOrigen"].Value.ToString();


                //Comprobar si la fecha no está incluida en otros periodos


                //--------------- Obtener el periodo anterior ----------------
                string saappAnterior = "";
                string fechaIniPeriodoAnterior = "";
                string fechaFinPeriodoAnterior = "";
                string resultPeriodoAnterior = this.ObtenerPeriodoAnterior(this.codigo, saprfl, ref saappAnterior, ref fechaIniPeriodoAnterior, ref fechaFinPeriodoAnterior);

                DateTime dtPeriodoModificar;
                DateTime dtPeriodoAnterior;
                DateTime dtPeriodoPosterior = new DateTime();

                dtPeriodoModificar = Convert.ToDateTime(this.txtMaskFechaIni.Text);

                bool existePeriodoAnterior = false;
                bool existePeriodoPosterior = false;

                if (resultPeriodoAnterior == "" && saappAnterior != "-1" && fechaIniPeriodoAnterior != "-1" && fechaIniPeriodoAnterior != "-1")
                {
                    existePeriodoAnterior = true;
                    //Comprobar que la nueva fecha de inicio del periodo actual tiene que ser mayor que la fecha de inicio del periodo anterior
                    dtPeriodoAnterior = utiles.FormatoCGToFecha(fechaIniPeriodoAnterior);
                    if (dtPeriodoModificar < dtPeriodoAnterior)
                    {
                        result = "La fecha de inicio ya se encuentra definida en el periodo anterior";
                        return (result);
                    }
                }

                //Obtener el periodo posterior
                string saappPosterior = "";
                string fechaIniPeriodoPosterior = "";
                string fechaFinPeriodoPosterior = "";
                string resultPeriodoPosterior = this.ObtenerPeriodoPosterior(this.codigo, saprfl, ref saappPosterior, ref fechaIniPeriodoPosterior, ref fechaFinPeriodoPosterior);

                if (resultPeriodoPosterior == "" && saappPosterior != "-1" && fechaIniPeriodoPosterior != "-1" && fechaFinPeriodoPosterior != "-1")
                {
                    existePeriodoPosterior = true;
                    //Comprobar que la nueva fecha de inicio del periodo actual tiene que ser menos que la fecha de inicio del periodo posterior
                    dtPeriodoPosterior = utiles.FormatoCGToFecha(fechaIniPeriodoPosterior);
                    if (dtPeriodoModificar > dtPeriodoPosterior)
                    {
                        result = "La fecha de inicio ya se encuentra definida en el periodo posterior";
                        return (result);
                    }
                }

                //Calcular la fecha final (fecha inicial del periodo posterior menos 1)
                int fechaIniInt = utiles.FechaToFormatoCG(dtPeriodoModificar, true);
                string fechaFinInt = finlfl;
                int fechaFinAux;

                if (existePeriodoPosterior)
                {
                    DateTime dt = dtPeriodoPosterior;
                    dt = dt.AddDays(-1);
                    fechaFinAux = utiles.FechaToFormatoCG(dt, true);
                    fechaFinInt = fechaFinAux.ToString();

                }

                //Actualizar el calendario en la tabla del maestro de calendarios (GLT04)
                string query = "update " + GlobalVar.PrefijoTablaCG + "GLT04 set ";
                query += "INLAFL = " + fechaIniInt + ", ";
                query += "FINLFL = " + fechaFinInt + " ";
                query += "where TITAFL = '" + this.codigo + "' and ";
                query += "SAPRFL = " + saprfl + " and ";
                query += "INLAFL = " + inlafl + " and ";
                query += "FINLFL = " + finlfl; 

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Modificar, "GLT04", this.codigo, saprfl);

                if (existePeriodoAnterior)
                {
                    DateTime dt = Convert.ToDateTime(this.txtMaskFechaIni.Text);
                    dt = dt.AddDays(-1);
                    fechaFinAux = utiles.FechaToFormatoCG(dt, true);
                    fechaFinInt = fechaFinAux.ToString();

                    //Actualizar la fecha fin del registro anterior si existe
                    query = "update " + GlobalVar.PrefijoTablaCG + "GLT04 set ";
                    query += "FINLFL = " + fechaFinInt + " ";
                    query += "where TITAFL = '" + this.codigo + "' and ";
                    query += "SAPRFL = " + saappAnterior + " and ";
                    query += "INLAFL = " + fechaIniPeriodoAnterior + " and ";
                    query += "FINLFL = " + fechaFinPeriodoAnterior;

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error actualizando los datos (" + ex.Message + ")";   //Falta traducir
            }

            return (result);
        }


        /// <summary>
        /// Actualiza el listado de periodos
        /// </summary>
        private void ActualizarListaPeriodos()
        {
            //Eliminar las filas de la Grid de Periodos
            if (this.dsDatos.Tables["Periodo"].Rows.Count > 0) this.dsDatos.Tables["Periodo"].Rows.Clear();
            if (this.oGridPeriodos.Rows.Count > 0) this.oGridPeriodos.Rows.Clear();

            //Volver a cargar los valores de la tabla de calendarios
            this.FillOutlookGrid();

            this.oGridPeriodos.Visible = true;
            this.oGridPeriodos.Enabled = true;

            ListSortDirection direction = ListSortDirection.Ascending;

            // remember the column that was clicked and in which direction is ordered
            prevColIndex = 0;
            prevSortDirection = direction;

            // set the column to be grouped
            this.oGridPeriodos.GroupTemplate.Column = this.oGridPeriodos.Columns[0];

            this.oGridPeriodos.GroupTemplate.ItemName = "periodo";      //Falta traducir
            this.oGridPeriodos.GroupTemplate.ItemsName = "periodos";    //Falta traducir

            //sort the grid
            this.oGridPeriodos.Sort(new PeriodoInfoComparer(0, direction));

            this.oGridPeriodos.ExpandAll();

            //int fila = this.oGridPeriodos.Rows.Count-1;
            //this.oGridPeriodos.Rows[fila].Selected = true;
            //this.oGridPeriodos.CurrentCell = this.oGridPeriodos[0, fila];

            //this.oGridPeriodos.Rows[posRegistro].Selected = true;
            this.oGridPeriodos.CurrentCell = this.oGridPeriodos[0, posRegistro];
        }


        /// <summary>
        /// Validar si el año tiene periodos
        /// </summary>
        /// <param name="aa"></param>
        /// <returns></returns>
        private bool ValidarAnoTienePeriodos(string aa)
        {
            bool result = false;
            try
            {
                string valorI = aa + "00";
                string valorF = aa + "99";
                string query = "select count(*) from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + this.codigo + "' and SAPRFL > " + valorI + " and SAPRFL <= " + valorF;

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) result = true;

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verificar que el SAPRFL no se este utilizando en ningún apunte contable introducido
        /// </summary>
        /// <returns></returns>
        private bool ExisteApunteContable(string saprfl)
        {
            bool result = true;

            try
            {
                //Busca todas las compañías que tengan el calendario seleccionado en la tabla (GLM01) y 
                //Para esas compañías, buscar en la tabla de comprobantes contables (GLB01) que no exista el saprfl
                string query = "select count(*) SAPRDT from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                query += "where CCIADT in (";
                query += "select distinct CCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where TITAMG = '" + this.codigo + "') ";
                query += "and SAPRDT = " + saprfl + "";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros <= 0) result = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Devuelve el periodo anterior al indicado por saprfl
        /// </summary>
        /// <param name="titafl">código del calendario</param>
        /// <param name="saprfl">siglo año periodo actual</param>
        /// <param name="saapp">siglo año perido del periodo anterior</param>
        /// <param name="fechaIni">fecha inicio del periodo anterior</param>
        /// <param name="fechaFin">fecha fin del periodo anterior</param>
        /// <returns></returns>
        private string ObtenerPeriodoAnterior(string titafl, string saprfl, ref string saapp, ref string fechaIni, ref string fechaFin)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select MAX(SAPRFL) SAPRFL,  INLAFL, FINLFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titafl + "' and SAPRFL < " + saprfl;
                query += " group by SAPRFL, INLAFL, FINLFL";
                query += " order by SAPRFL desc ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    saapp = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString();
                    fechaIni = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    fechaFin = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();
                }
                else
                {
                    saapp = "-1";
                    fechaIni = "-1";
                    fechaFin = "-1";
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = "Error obteniendo el periodo anterior (" + ex.Message + ")";     //Falta traducir
            }

            return (result);
        }


        /// <summary>
        /// Devuelve el periodo posterior al indicado por saprfl
        /// </summary>
        /// <param name="titafl">código del calendario</param>
        /// <param name="saprfl">siglo año periodo actual</param>
        /// <param name="saapp">siglo año perido del periodo posterior</param>
        /// <param name="fechaIni">fecha inicio del periodo posterior</param>
        /// <param name="fechaFin">fecha fin del periodo posterior</param>
        /// <returns></returns>
        private string ObtenerPeriodoPosterior(string titafl, string saprfl, ref string saapp, ref string fechaIni, ref string fechaFin)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                string query = "select MIN(SAPRFL) SAPRFL,  INLAFL, FINLFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titafl + "' and SAPRFL > " + saprfl;
                query += " group by SAPRFL, INLAFL, FINLFL";
                query += " order by SAPRFL";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    saapp = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString();
                    fechaIni = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    fechaFin = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();
                }
                else
                {
                    saapp = "-1";
                    fechaIni = "-1";
                    fechaFin = "-1";
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = "Error obteniendo el periodo posterior (" + ex.Message + ")";    //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve el último periodo del calendario
        /// </summary>
        /// <param name="titafl">código del calendario</param>
        /// <param name="saapp">siglo año perido del ultimo periodo</param>
        /// <param name="fechaIni">fecha inicio del ultimo periodo</param>
        /// <param name="fechaFin">fecha fin del ultimo periodo</param>
        /// <returns></returns>
        private string ObtenerUltimoPeriodo(string titafl, ref string saapp, ref string fechaIni, ref string fechaFin)
        {
            string result = "";
            IDataReader dr = null;
            try
            {
                //Buscar los datos del ultimo periodo
                string query = "select MAX(SAPRFL) SAPRFL, INLAFL, FINLFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + titafl + "' ";
                query += "group by SAPRFL, INLAFL, FINLFL ";
                query += "order by SAPRFL desc";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    saapp = dr.GetValue(dr.GetOrdinal("SAPRFL")).ToString();
                    fechaIni = dr.GetValue(dr.GetOrdinal("INLAFL")).ToString();
                    fechaFin = dr.GetValue(dr.GetOrdinal("FINLFL")).ToString();
                }
                else
                {
                    saapp = "-1";
                    fechaIni = "-1";
                    fechaFin = "-1";
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                result = "Error obteniendo el último periodo del calendario (" + ex.Message + ")";  //Falta traducir
            }

            return (result);
        }

        /// <summary>
        /// Devuelve la sentencia sql del update del campo fecha final del periodo
        /// </summary>
        /// <param name="titafl">código del calendario</param>
        /// <param name="saprfl">siglo año periodo del periodo</param>
        /// <param name="inlafl">fecha inicio del periodo</param>
        /// <param name="finlfl">fecha final del periodo</param>
        /// <returns></returns>
        private string ObtenerUpdateFechaFin(string titafl, string saprfl, string inlafl, string finlfl, string finlflNueva)
        {
            string query = "";
            try
            {
                query = "update " + GlobalVar.PrefijoTablaCG + "GLT04 set ";
                query += "FINLFL = " + finlflNueva + " ";
                query += "where TITAFL = '" + titafl + "' and ";
                query += "SAPRFL = " + saprfl + " and ";
                query += "INLAFL = " + inlafl + " and ";
                query += "FINLFL = " + finlfl ;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (query);
        }


        /// <summary>
        /// Valida si la fecha fin del último período es correcta
        /// </summary>
        /// <param name="dtFechaFin">Devuelve la fecha final en formato DateTime</param>
        /// <returns></returns>
        private string FormValidFechaUltPrd(ref DateTime dtFechaFin)
        {
            string result = "";

            try
            {
                this.txtMaskFechaFinUltPrd.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string fechaFin = this.txtMaskFechaFinUltPrd.Text.Trim();
                this.txtMaskFechaFinUltPrd.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (fechaFin == "")
                {
                    result = "La fecha final no puede estar en blanco \n\r";      //Falta traducir
                    this.txtMaskFechaFinUltPrd.Focus();
                    return (result);
                }
                else
                {
                    if (this.txtMaskFechaFinUltPrd.Text == "99/99/9999") return (result);

                    DateTime dtFechaIni;
                    try
                    {
                        dtFechaFin = Convert.ToDateTime(this.txtMaskFechaFinUltPrd.Text);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        result = "La fecha final no tiene un formato válido";
                        this.txtMaskFechaFinUltPrd.Focus();
                        return (result);
                    }

                    try
                    {
                        dtFechaIni = Convert.ToDateTime(this.txtMaskFechaIniUltPrd.Text);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        result = "La fecha inicial no tiene un formato válido";
                        this.txtMaskFechaFinUltPrd.Focus();
                        return (result);
                    }

                    if (dtFechaIni > dtFechaFin)
                    {
                        result = "La fecha final tiene que ser mayor o igual que la fecha inicial";
                        this.txtMaskFechaFinUltPrd.Focus();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
       
        /// <summary>
        /// Inserta un periodo en la tabla GLT04
        /// </summary>
        /// <param name="titafl"></param>
        /// <param name="saapp"></param>
        /// <param name="inlafl"></param>
        /// <param name="finlfl"></param>
        /// <returns></returns>
        private string InsertarPeriodoGLT04(string titafl, string saapp, string inlafl, string finlfl)
        {
            string result = "";

            try
            {
                //--------------- Obtener el periodo anterior ----------------
                string saappAux = "";
                string fechaIniPeriodoAnterior = "";
                string fechaFinPeriodoAnterior = "";
                string resultPeriodoAnterior = this.ObtenerPeriodoAnterior(titafl, saapp, ref saappAux, ref fechaIniPeriodoAnterior, ref fechaFinPeriodoAnterior);

                DateTime dtPeriodoNuevo;
                DateTime dtPeriodoExiste;

                dtPeriodoNuevo = utiles.FormatoCGToFecha(inlafl);

                if (resultPeriodoAnterior == "" && saappAux != "-1" && fechaIniPeriodoAnterior != "-1" && fechaIniPeriodoAnterior != "-1")
                {
                    //Comprobar que la fecha de inicio del periodo nuevo ha de ser mayor que la fecha de inicio del periodo anterior

                    dtPeriodoExiste = utiles.FormatoCGToFecha(fechaIniPeriodoAnterior);
                    if (dtPeriodoNuevo <= dtPeriodoExiste)
                    {
                        result = "La fecha ya se encuentra definida en este calendario";
                        return (result);
                    }
                }

                //Obtener el periodo posterior
                saappAux = "";
                string fechaIniPeriodoPosterior = "";
                string fechaFinPeriodoPosterior = "";
                result = this.ObtenerPeriodoPosterior(titafl, saapp, ref saappAux, ref fechaIniPeriodoPosterior, ref fechaFinPeriodoPosterior);

                if (fechaIniPeriodoPosterior != "-1")
                {
                    //Comprobar que la fecha de inicio del periodo nuevo ha de ser mayor que la fecha de inicio del periodo posterior
                    dtPeriodoExiste = utiles.FormatoCGToFecha(fechaIniPeriodoPosterior);

                    if (dtPeriodoNuevo >= dtPeriodoExiste)
                    {
                        result = "La fecha ya se encuentra definida en este calendario";
                        return (result);
                    }
                }

                //Dar de alta al calendario en la tabla del maestro de calendarios (GLT04)
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLT04";
                string query = "insert into " + nombreTabla + " (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                query += "TITAFL, SAPRFL, INLAFL, FINLFL) values (";
                if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                query += "'" + titafl + "', " + saapp + ", " + inlafl + ", " + finlfl + ")";

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                this.posRegistro = this.oGridPeriodos.Rows.Count - 1;   //Falta buscar la posicion del registro dentro de la grid

                //Actualiza el listado de periodos
                this.ActualizarListaPeriodos();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = "Error insertando el periodo (" + ex.Message + ")";   //Falta traducir
                return (result);
            }

            return (result);
        }

        /// <summary>
        /// Valida si existe un ejercicio posterior al año destino indicado al copiar periodos
        /// </summary>
        /// <param name="inlaflDestino">fecha inicio del periodo para la copia</param>
        /// <returns></returns>
        private bool ExisteEjercicioPosteriorDestino(string inlaflDestino)
        {
            bool result = false;

            try
            {
                //Registro último periodo del calendario
                string saapp = "";
                string fechaIniUltPeriodo = "";
                string fechaFinUltPeriodo = "";
                string resultUltPeriodo = this.ObtenerUltimoPeriodo(this.txtCodigo.Text, ref saapp, ref fechaIniUltPeriodo, ref fechaFinUltPeriodo);

                if (resultUltPeriodo == "" && fechaFinUltPeriodo != "-1")
                {
                    if (fechaFinUltPeriodo != "9999999")
                    {
                        DateTime dtFechaIniNuevoPeriodo = utiles.FormatoCGToFecha(inlaflDestino);
                        DateTime dtFechaFinUltPeriodo = utiles.FormatoCGToFecha(fechaFinUltPeriodo);

                        if (dtFechaFinUltPeriodo > dtFechaIniNuevoPeriodo)
                        {
                            result = true;
                        }
                    }
                    else result = true;
                }

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Adicionar un Periodo
        /// </summary>
        private void AdicionarPrd()
        {
            this.txtAno.Text = "";
            this.txtAno.Tag = "";
            this.txtPeriodo.Text = "";
            this.txtPeriodo.Tag = "";
            this.txtMaskFechaIni.Text = "";
            this.txtMaskFechaIni.Tag = "";

            this.txtAno.IsReadOnly = false;
            this.txtPeriodo.IsReadOnly = false;
            this.txtMaskFechaIni.ReadOnly = false;

            this.oGridPeriodos.ClearSelection();

            this.gbPeriodoCopiar.Visible = false;
            this.gbPeriodoEdicion.Visible = true;
            this.gbPeriodoEdicion.Text = "Adicionar Periodo";
            this.gbPeriodoFechaFinUltPrd.Visible = false;

            utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);

            this.txtAno.Focus();
            this.nuevo = true;
        }

        /// <summary>
        /// Copiar un Periodo
        /// </summary>
        private void CopiarPrd()
        {
            this.gbPeriodoEdicion.Visible = false;
            this.txtAnoOrigen.Text = "";
            this.txtAnoDestino.Text = "";
            this.gbPeriodoCopiar.Visible = true;
            this.gbPeriodoFechaFinUltPrd.Visible = false;

            this.gbPeriodoCopiar.Location = new Point(this.gbPeriodoEdicion.Location.X, this.gbPeriodoEdicion.Location.Y);

            this.oGridPeriodos.ClearSelection();

            utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);

            this.txtAnoOrigen.Focus();
        }

        /// <summary>
        /// Fecha Final del Último Periodo
        /// </summary>
        private void FechaFinUltPrd()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.gbPeriodoEdicion.Visible = false;
                this.gbPeriodoCopiar.Visible = false;
                this.gbPeriodoFechaFinUltPrd.Visible = true;
                this.gbPeriodoFechaFinUltPrd.Location = new Point(this.gbPeriodoEdicion.Location.X, this.gbPeriodoEdicion.Location.Y);

                this.oGridPeriodos.ClearSelection();

                utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);

                //Buscar los datos del ultimo periodo
                string query = "select MAX(SAPRFL) SAPRFL, INLAFL, FINLFL from " + GlobalVar.PrefijoTablaCG + "GLT04 ";
                query += "where TITAFL = '" + this.codigo + "' ";
                query += "group by SAPRFL, INLAFL, FINLFL ";
                query += "order by SAPRFL desc";

                DataTable dtAux = new DataTable();
                dtAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dtAux != null && dtAux.Rows.Count > 0)
                {
                    //dtAux.DefaultView.Sort = "SAPRFL desc";

                    //Fila nueva de la Grid
                    DataRow row = this.dsDatos.Tables["Periodo"].NewRow();

                    //Dado una fila en la tabla de periodos, devuelve una fila para la Grid (con las celdas que espera la Grid)
                    this.ObtenerFilaGrid(dtAux.Rows[0], ref row);

                    //Inicializar las variables del formulario de Fecha Final Ultimo Periodo
                    this.txtAnoUltPrd.Text = row["AA"].ToString();
                    this.txtPeriodoUltPrd.Text = row["PP"].ToString();
                    this.txtMaskFechaIniUltPrd.Text = row["FechaIni"].ToString();
                    this.txtMaskFechaFinUltPrd.Text = row["FechaFin"].ToString();
                    this.saappOrigenUltPrd = row["AAPPOrigen"].ToString();
                    this.fechaIniOrigenUltPrd = row["FechaIniOrigen"].ToString();
                    this.fechaFinOrigenUltPrd = row["FechaFinOrigen"].ToString();
                }

                this.txtMaskFechaFinUltPrd.Focus();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eliminar un periodo
        /// </summary>
        private void EliminarPrd()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Pedir confirmación y eliminar el cambio seleccionado
                string mensaje = "Se va a eliminar el período asociado. ";  //Falta traducir
                mensaje += " " + this.LP.GetText("confDeseaCont", "¿Desea continuar?");
                DialogResult result = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DataGridViewCell celdaActiva = this.oGridPeriodos.CurrentCell;
                    string saprfl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["AAPPOrigen"].Value.ToString();
                    string inlafl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["FechaIniOrigen"].Value.ToString();
                    string finlfl = this.oGridPeriodos.Rows[celdaActiva.RowIndex].Cells["FechaFinOrigen"].Value.ToString();

                    //Verificar que el SAPRFL no se este utilizando en ningún apunte contable contable introducido
                    bool existeApunteContable = this.ExisteApunteContable(saprfl);

                    if (existeApunteContable)
                    {
                        RadMessageBox.Show("Existe un apunte contable para el periodo indicado. No es posible eliminar el periodo.", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
                    }
                    else
                    {
                        this.gbPeriodoEdicion.Visible = false;

                        //------- Buscar el registro anterior y el posterior para actualizar las fechas y que no hayan saltos en el tiempo ------
                        //Registro Periodo Anterior
                        string saapp = "";
                        string fechaIniPeriodoAnterior = "";
                        string fechaFinPeriodoAnterior = "";
                        string resultPeriodoAnterior = this.ObtenerPeriodoAnterior(this.txtCodigo.Text, saprfl, ref saapp, ref fechaIniPeriodoAnterior, ref fechaFinPeriodoAnterior);

                        //Registro Periodo Posterior
                        string saappPosterior = "";
                        string fechaIniPeriodoPosterior = "";
                        string fechaFinPeriodoPosterior = "";
                        string resultPeriodoPosterior = this.ObtenerPeriodoPosterior(this.txtCodigo.Text, saprfl, ref saappPosterior, ref fechaIniPeriodoPosterior, ref fechaFinPeriodoPosterior);

                        //Eliminar registro
                        string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLT04 where ";
                        query += " TITAFL = '" + this.codigo + "' and SAPRFL = " + saprfl + " and INLAFL = " + inlafl + " and FINLFL = " + finlfl;

                        int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                        utiles.Auditoria(formCode, AuditoriaGLL04.OperacionTipo.Eliminar, "GLT04", this.codigo, saprfl);

                        //--------- Actualizar las fechas del registro anterior si existe registro posterior -------
                        if (resultPeriodoAnterior == "" && saapp != "-1" && fechaIniPeriodoAnterior != "-1" && fechaFinPeriodoAnterior != "-1")
                        {
                            //Existe periodo anterior
                            if (resultPeriodoPosterior == "" && saappPosterior != "-1" && fechaIniPeriodoPosterior != "-1" && fechaFinPeriodoPosterior != "-1")
                            {
                                //Existe periodo posterior
                                //Actualizar la fecha final del registro del periodo anterior
                                query = this.ObtenerUpdateFechaFin(this.codigo, saapp, fechaIniPeriodoAnterior, fechaFinPeriodoAnterior, finlfl);

                                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                            }
                        }

                        //Actualiza el listado de periodos
                        this.ActualizarListaPeriodos();

                        if (this.dtCalendario.Rows.Count == 0)
                        {
                            //Actualiza el listado de elementos del formulario frmGLT04Sel
                            //this.ActualizarFormularioListaElementos();

                            //No existen Periodos
                            this.oGridPeriodos.Visible = false;
                            utiles.ButtonEnabled(ref this.radButtonAdicionarPrd, false);
                            utiles.ButtonEnabled(ref this.radButtonCopiarPrd, false);
                            utiles.ButtonEnabled(ref this.radButtonEliminarPrd, false);
                            utiles.ButtonEnabled(ref this.radButtonFechaFinUltPrd, false);

                            //Mostrar el formulario como cuando nuevo
                            this.nuevo = true;
                            this.txtAno.Text = "";
                            this.txtAno.IsReadOnly = false;
                            this.txtPeriodo.Text = "";
                            this.txtPeriodo.IsReadOnly = false;
                            this.txtMaskFechaIni.Text = "";

                            this.gbPeriodoEdicion.Location = new Point(135, 92);
                            this.Size = new System.Drawing.Size(this.Size.Width - 100, this.Size.Height - 300);

                            this.gbPeriodoEdicion.Visible = true;
                            this.gbPeriodoEdicion.Text = "Periodo";
                            this.txtAno.Focus();
                        }

                        this.oGridPeriodos.ClearSelection();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            Cursor.Current = Cursors.Default;
        }

        private bool FormValidCopiarPeriodos()
        {
            bool result = false;

            try
            {
                string errores = "";
                string anoOrigen = this.txtAnoOrigen.Text.Trim();
                string anoDestino = this.txtAnoDestino.Text.Trim();
                if (anoOrigen == "")
                {
                    errores += "- El año de origen no puede estar en blanco.";
                    this.txtAnoOrigen.Focus();
                }

                if (anoDestino == "")
                {
                    if (errores != "") errores += "\n\r";
                    errores += "- El año final no puede estar en blanco.";
                    this.txtAnoDestino.Focus();
                }

                if (anoOrigen == anoDestino)
                {
                    if (errores != "") errores += "\n\r";
                    errores += "- El año origen y destino no pueden ser iguales.";
                    this.txtAnoDestino.Focus();
                }

                if (anoOrigen.Length == 1 || anoOrigen.Length == 3)
                {
                    if (errores != "") errores += "\n\r";
                    errores += "- El año origen no tiene un formato correcto.";
                    this.txtAnoOrigen.Focus();
                }

                if (anoDestino.Length == 1 || anoDestino.Length == 3)
                {
                    if (errores != "") errores += "\n\r";
                    errores += "- El año destino no tiene un formato correcto.";
                    this.txtAnoDestino.Focus();
                }

                int anoCorte = Convert.ToInt16(CGParametrosGrles.GLC01_ALSIRC);

                int anoOrigenInt = Convert.ToInt32(this.txtAnoOrigen.Text);
                if (anoOrigen.Length < 4)
                    if (anoOrigenInt < anoCorte) anoOrigenInt = 2000 + anoOrigenInt;
                    else if (anoOrigenInt >= anoCorte) anoOrigenInt = 1900 + anoOrigenInt;

                int anoDestinoInt = Convert.ToInt32(this.txtAnoDestino.Text);
                if (anoDestino.Length < 4)
                    if (anoDestinoInt < anoCorte) anoDestinoInt = 2000 + anoDestinoInt;
                    else if (anoDestinoInt >= anoCorte) anoDestinoInt = 1900 + anoDestinoInt;

                if (anoDestinoInt < anoOrigenInt)
                {
                    if (errores != "") errores += "\n\r";
                    errores += "- Existe un ejercicio posterior al ejercicio destino.";
                    this.txtAnoDestino.Focus();
                }

                if (errores != "")
                {
                    RadMessageBox.Show(errores, this.LP.GetText("errValTitulo", "Error"));
                    return (false);
                }

                //Validar si el año está dentro de los ejercicios permitidos (>= 1940 y <= 2039)
                if (!utiles.EjercicioValido(anoDestinoInt.ToString()))
                {
                    RadMessageBox.Show("Ejercicio fuera de años permitidos", this.LP.GetText("errValTitulo", "Error"));    //Falta traducir
                    return (false);
                }

                //Completar a 4 digitos el año
                if (anoOrigen.Length < 4) this.txtAnoOrigen.Text = anoOrigenInt.ToString();
                if (anoDestino.Length < 4) this.txtAnoDestino.Text = anoDestinoInt.ToString();

                string aaOrigen = this.txtAnoOrigen.Text.Substring(2, 2);
                string siglo = utiles.SigloDadoAnno(aaOrigen, CGParametrosGrles.GLC01_ALSIRC);
                aaOrigen = siglo + aaOrigen;
                bool validarAnoOrigen = this.ValidarAnoTienePeriodos(aaOrigen);
                if (!validarAnoOrigen)
                {
                    RadMessageBox.Show("- El año origen no tiene periodos definidos", this.LP.GetText("errValTitulo", "Error"));
                    this.txtAnoOrigen.Focus();
                    return (false);
                }

                string aaDestino = this.txtAnoDestino.Text.Substring(2, 2);
                siglo = utiles.SigloDadoAnno(aaDestino, CGParametrosGrles.GLC01_ALSIRC);
                aaDestino = siglo + aaDestino;
                bool validarAnoDestino = this.ValidarAnoTienePeriodos(aaDestino);
                if (validarAnoDestino)
                {
                    RadMessageBox.Show("- El año destino ya tiene periodos definidos", this.LP.GetText("errValTitulo", "Error"));
                    this.txtAnoDestino.Focus();
                    return (false);
                }

                result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion

        #region Comparers - used to sort CustomerInfo objects and DataRows of a DataTable

        // custom object comparer implementation
        public class PeriodoInfoComparer : IComparer
        {
            private readonly int propertyIndex;
            readonly ListSortDirection direction;

            public PeriodoInfoComparer(int propertyIndex, ListSortDirection direction)
            {
                this.propertyIndex = propertyIndex;
                this.direction = direction;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                DataRow obj1 = (DataRow)x;
                DataRow obj2 = (DataRow)y;

                switch (propertyIndex)
                {
                    case 0:
                        return CompareFechaPeriodo(obj1, obj2);
                    case 1:
                        return ComparePeriodoFecha(obj1, obj2);
                    case 2:
                        return CompareNumbers(obj1, obj2, 5);
                    case 3:
                        return CompareNumbers(obj1, obj2, 6);
                    default:
                        return CompareNumbers(obj1, obj2, 5);
                }
            }
            #endregion

            private int CompareFechaPeriodo(DataRow obj1, DataRow obj2)
            {
                int result;
                result = string.Compare(obj1[0].ToString(), obj2[0].ToString()) * (direction == ListSortDirection.Ascending ? 1 : -1);
                if (result != 0)
                    return result;
                return string.Compare(obj1[1].ToString(), obj2[1].ToString());
            }

            public int ComparePeriodoFecha(DataRow obj1, DataRow obj2)
            {
                int result;
                result = string.Compare(obj1[1].ToString(), obj2[1].ToString()) * (direction == ListSortDirection.Ascending ? 1 : -1);
                if (result != 0)
                    return result;
                return string.Compare(obj1[0].ToString(), obj2[0].ToString());
            }


            public int CompareNumbers(DataRow obj1, DataRow obj2, int indiceCol)
            {
                try
                {
                    double val1 = Convert.ToDouble(obj1[indiceCol]);
                    double val2 = Convert.ToDouble(obj2[indiceCol]);

                    if (val1 > val2) return (direction == ListSortDirection.Ascending ? 1 : -1);
                    if (val1 < val2) return (direction == ListSortDirection.Ascending ? -1 : 1);
                }
                catch(Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); 
                }

                return 0;
            }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);

            this.HabilitarDeshabilitarControles(true);
            this.txtAno.IsReadOnly = false;
            this.txtPeriodo.IsReadOnly = false;
        }

        private void TxtCodigo_Leave(object sender, EventArgs e)
        {

            string codCalendario = this.txtCodigo.Text.Trim();

            if (codCalendario == "")
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCodigo.Text = "";
                this.txtCodigo.Focus();

                RadMessageBox.Show("Código de calendario obligatorio", this.LP.GetText("errValCodCalendario", "Error"));  //Falta traducir
                return;
            }

            bool codCalendarioOk = true;
            if (this.nuevo) codCalendarioOk = this.CodigoCalendarioValido();    //Verificar que el codigo no exista

            if (codCalendarioOk)
            {
                this.HabilitarDeshabilitarControles(true);

                this.txtCodigo.IsReadOnly = true;
                this.txtAno.IsReadOnly = false;
                this.txtPeriodo.IsReadOnly = false;

                utiles.ButtonEnabled(ref this.radButtonGrabarPeriodo, true);

                this.txtAno.Focus();
            }
            else
            {
                this.HabilitarDeshabilitarControles(false);
                this.txtCodigo.Focus();
                RadMessageBox.Show("Código de calendario ya existe", this.LP.GetText("errValCodCalendarioExiste", "Error"));  //Falta traducir
            }
        }
        // custom object comparer implementation
        #endregion Comparers        
    }
}
