using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ObjectModel
{
    public partial class TGGrid : DataGridView

    {
        public DataSet dsDatos = null;
        private string nombreTabla = "";
        private bool addUltimaFilaSiNoHayDisponile = false;
        private int rowHeaderInitWidth;
        private bool rowNumber;

        private string[,] comboValores;
        #region Properties
        public DataSet DSDatos
        {
            get
            {
                return (this.dsDatos);
            }
            set
            {
                this.dsDatos = value;
            }
        }

        public ContextMenuStrip ContextMenuStripGrid
        {
            get
            {
                return (this.ContextMenuStrip);
            }
            set
            {
                this.ContextMenuStrip = value;
            }
        }

        public string NombreTabla
        {
            get
            {
                return (this.nombreTabla);
            }
            set
            {
                this.nombreTabla = value;
            }
        }

        /// <summary>
        /// Configura si se añade una fila en blanco si se acaban (siempre una fila en blanco disponible)
        /// </summary>
        public bool AddUltimaFilaSiNoHayDisponile
        {
            get
            {
                return (this.addUltimaFilaSiNoHayDisponile);
            }
            set
            {
                this.addUltimaFilaSiNoHayDisponile = value;
            }
        }

        /// <summary>
        /// Configura los valores de los combos si existen dentro de la Grid. Se utiliza al pegar los datos
        /// </summary>
        public string[,] ComboValores
        {
            get
            {
                return (this.comboValores);
            }
            set
            {
                this.comboValores = value;
            }
        }

        /// <summary>
        /// Ancho de la columna header de las filas
        /// </summary>
        public int RowHeaderInitWidth
        {
            get
            {
                return (this.rowHeaderInitWidth);
            }
            set
            {
                this.rowHeaderInitWidth = value;
            }

        }

        public bool RowNumber
        {
            get
            {
                return (this.rowNumber);
            }
            set
            {
                this.rowNumber = value;
            }
        }
        #endregion

        public TGGrid()
        {
            InitializeComponent();

            this.AplicarEstilo();

            this.rowHeaderInitWidth = this.RowHeadersWidth;
        }

        #region Public Methods
        /// <summary>
        /// Copiar desde la Grid de detalles al Clipboard
        /// </summary>
        public void CopiarDetalles()
        {
            if (this.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                // Add the selection to the clipboard.
                Clipboard.SetDataObject(this.GetClipboardContent());
            }
        }

        /// <summary>
        /// Pega desde el Clipboard a la Grid de detalles
        /// </summary>
        /// <returns>Devuelve la fila a donde se va a copiar y la cantidad de filas a copiar</returns>
        public int[] PegarDetalles()
        {
            int[] result = new int[2]{0, 0};

            int lastRow = this.dsDatos.Tables[this.nombreTabla].Rows.Count;

            // Replace the text box contents with the clipboard text. 
            //string texto = Clipboard.GetText();

            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                int rowMenor = 0;
                int rowMayor = 0;
                int colMenor = 0;
                int colMayor = 0;
                int rowActual;
                int colActual;

                for (int i = 0; i < this.SelectedCells.Count; i++)
                {
                    rowActual = this.SelectedCells[i].RowIndex;
                    colActual = this.SelectedCells[i].ColumnIndex;

                    if (i == 0)
                    {
                        rowMenor = rowActual;
                        rowMayor = rowActual;

                        colMenor = colActual;
                        colMayor = colActual;
                    }
                    else
                    {
                        if (rowActual < rowMenor) rowMenor = rowActual;
                        if (rowActual > rowMayor) rowMayor = rowActual;

                        if (colActual < colMenor) colMenor = colActual;
                        if (colActual > colMayor) colMayor = colActual;
                    }
                }


                //int rowOfInterest = this.dgDetalles.CurrentCell.RowIndex;
                int rowOfInterest = rowMenor;
                result[0] = rowOfInterest;

                string[] selectedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

                if (selectedRows == null || selectedRows.Length == 0)
                    return(result);

                int colMenorAux;
                foreach (string row in selectedRows)
                {
                    colMenorAux = colMenor;

                    try
                    {
                        string[] data = Regex.Split(row, "\t");

                        //int col = this.dgDetalles.CurrentCell.ColumnIndex;

                        foreach (string ob in data)
                        {
                            if (colMenorAux >= this.Columns.Count)
                                break;
                            if (ob != null)
                            {
                                string valor = ob;

                                if (this.Columns[colMenorAux].CellType == typeof(CalendarColumn))
                                {
                                    //CalendarColumn
                                    if (valor == "") this[colMenorAux, rowOfInterest].Value = valor;
                                    else this[colMenorAux, rowOfInterest].Value = Convert.ChangeType(valor, this[colMenorAux, rowOfInterest].ValueType);
                                }
                                else
                                if (this.Columns[colMenorAux].CellType == typeof(DataGridViewComboBoxCell))
                                {
                                    //ComboBox
                                    string comboValue = valor;
                                    for (int i = 0 ; i < this.comboValores.Length / 2; i++)
                                    {
                                        if (this.comboValores[i, 0] == valor)
                                        {
                                            comboValue = this.comboValores[i, 1];
                                            break;
                                        }
                                    }

                                    this[colMenorAux, rowOfInterest].Value = Convert.ChangeType(comboValue, this[colMenorAux, rowOfInterest].ValueType);
                                }
                                else 
                                {
                                    if (this[colMenorAux, rowOfInterest].ValueType.Name == "DateTime" && valor == "")
                                    {
                                        this[colMenorAux, rowOfInterest].Value = valor;
                                    }
                                    else 
                                        this[colMenorAux, rowOfInterest].Value = Convert.ChangeType(valor, this[colMenorAux, rowOfInterest].ValueType);
                                }
                            }
                            colMenorAux++;
                        }

                        //Adicionar una fila en blanco si se ha utilizado la última
                        if (rowOfInterest == lastRow - 1)
                        {
                            this.dsDatos.Tables[this.nombreTabla].Rows.Add();
                            if (this.rowNumber) this.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
                            lastRow++;
                        }
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

                    rowOfInterest++;
                }
                
                result[1] = rowOfInterest - result[0];
            }
            return (result);
        }

        /// <summary>
        /// Elimina el contenido de las celdas seleccionadas
        /// </summary>
        public void BorrarDetalles()
        {
            for (int i = 0; i < this.SelectedCells.Count; i++)
            {
                this.SelectedCells[i].Value = "";
            }
        }

        /// <summary>
        /// Copia hacia el Clipboard el contenido de las celdas seleccionadas, después elimina el contenido de dichas celdas 
        /// </summary>
        public void CortarDetalles()
        {
            if (this.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                // Add the selection to the clipboard.
                Clipboard.SetDataObject(this.GetClipboardContent());

                for (int i = 0; i < this.SelectedCells.Count; i++)
                {
                    this.SelectedCells[i].Value = "";
                }
            }
        }

        /// <summary>
        /// Inserta una fila nueva arriba de la fila actual
        /// </summary>
        public void InsertarFila(int cantidad)
        {
            int newRow = 0;
            int currentRow = 0;
            try
            {
                for (int i = 0; i < this.SelectedCells.Count; i++)
                {
                    currentRow = this.SelectedCells[i].RowIndex;
                    break;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            if (currentRow > 0) newRow = currentRow--;
            for (int i = 0; i < cantidad; i++)
            {
                DataRow dr = dsDatos.Tables[this.nombreTabla].NewRow();
                dsDatos.Tables[this.nombreTabla].Rows.InsertAt(dr, newRow);
            }
        }

        /// <summary>
        /// Adiciona una fila nueva al final de la Grid
        /// </summary>
        public void AdicionarFila()
        {
            int lastRow = this.dsDatos.Tables[this.nombreTabla].Rows.Count;
            DataRow dr = this.dsDatos.Tables[this.nombreTabla].NewRow();
            this.dsDatos.Tables[this.nombreTabla].Rows.InsertAt(dr, lastRow);
            if (this.rowNumber) this.AddRowNumber(DataGridViewContentAlignment.MiddleRight);
        }

        /// <summary>
        /// Elimina las filas seleccionadas
        /// </summary>
        public void SuprimirFila()
        {
            try
            {
                foreach (DataGridViewRow row in this.SelectedRows)
                    if (!row.IsNewRow) this.Rows.Remove(row);

                if (this.dsDatos.Tables[this.nombreTabla].Rows.Count <= 1) this.dsDatos.Tables[this.nombreTabla].Rows.Add();

                this.Update();
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Cambia el texto del Encabezado de una columna
        /// </summary>
        /// <param name="nombreColumna"></param>
        /// <param name="encabezadoTexto"></param>
        public void CambiarColumnHeader(string nombreColumna, string encabezadoTexto)
        {
            try
            {
                if (this.Columns[nombreColumna] != null) this.Columns[nombreColumna].HeaderText = encabezadoTexto;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Inhabilita una columna del TGGrid
        /// </summary>
        /// <param name="nombreColumna">nombre de la columna</param>
        /// <param name="limpiarValores">indica si se blanquea la columna</param>
        /// <param name="valor">si limpiarValores es true -> a la columna se le pondrá el valor especificado</param>
        public void ColumnNotEnable(string nombreColumna, bool limpiarValores, string valor)
        {
            this.Columns[nombreColumna].ReadOnly = true;
            this.Columns[nombreColumna].DefaultCellStyle.BackColor = Color.LightGray;
            this.Columns[nombreColumna].DefaultCellStyle.ForeColor = Color.DarkGray;
            this.Columns[nombreColumna].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            this.Columns[nombreColumna].DefaultCellStyle.SelectionForeColor = Color.DarkGray;

            if (limpiarValores)
            {
                foreach (DataGridViewRow row in this.Rows)
                {
                    row.Cells[nombreColumna].Value = valor;  //Rellenar todas las filas de la columna con el valor especificado
                }
            }
        }

        /// <summary>
        /// Habilita una columna del TGGrid
        /// </summary>
        /// <param name="nombreColumna">nombre de la columna</param>
        public void ColumnEnable(string nombreColumna)
        {
            this.Columns[nombreColumna].ReadOnly = false;
            this.Columns[nombreColumna].DefaultCellStyle.BackColor = Color.White;
            this.Columns[nombreColumna].DefaultCellStyle.ForeColor = Color.Black;
            this.Columns[nombreColumna].DefaultCellStyle.SelectionBackColor = this.Columns[0].DefaultCellStyle.SelectionBackColor;  //Azul
            this.Columns[nombreColumna].DefaultCellStyle.SelectionForeColor = Color.White;
        }


        /// <summary>
        /// Inhabilita una celda del TGGrid
        /// </summary>
        /// <param name="filaIndex">índice de la fila</param>
        /// <param name="nombreColumna">nombre de la columna</param>
        /// <param name="limpiarValores">indica si se blanquea la columna</param>
        /// <param name="valor">si limpiarValores es true -> a la columna se le pondrá el valor especificado</param>
        public void CellNotEnable(int filaIndex, string nombreColumna, bool limpiarValores, string valor)
        {
            this.Rows[filaIndex].Cells[nombreColumna].ReadOnly = true;
            this.Rows[filaIndex].Cells[nombreColumna].Style.BackColor = Color.LightGray;

            if (limpiarValores)
            {
                this.Rows[filaIndex].Cells[nombreColumna].Value = valor;
            }
        }

        /// <summary>
        /// Habilita una celda del TGGrid
        /// </summary>
        /// <param name="filaIndex">índice de la fila</param>
        /// <param name="nombreColumna">nombre de la columna</param>
        public void CellEnable(int filaIndex, string nombreColumna)
        {
            this.Rows[filaIndex].Cells[nombreColumna].ReadOnly = false;
            this.Rows[filaIndex].Cells[nombreColumna].Style.BackColor = Color.White;
            this.Rows[filaIndex].Cells[nombreColumna].Style.ForeColor = Color.Black;
            this.Rows[filaIndex].Cells[nombreColumna].Style.SelectionBackColor = this.Columns[0].DefaultCellStyle.SelectionBackColor;  //Azul
            this.Rows[filaIndex].Cells[nombreColumna].Style.SelectionForeColor = Color.White;
        }

        /// <summary>
        /// Adicionar una columna de Tipo TextBox al TGGrid
        /// </summary>
        /// <param name="pos">Posición de la columna</param>
        /// <param name="nombre">Nombre de la columna</param>
        /// <param name="texto">Caption de la columna</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="maxImputLength">Máximo Número de Caracteres de entrada en la columna</param>
        /// <param name="tipo">Tipo de la columna</param>
        /// <param name="alinear">Alinear la columna</param>
        public void AddTextBoxColumn(int pos, string nombre, string texto, int width, int maxImputLength, Type tipo, DataGridViewContentAlignment alinear, bool visible)
        {
            try
            {
                DataGridViewTextBoxColumn columnTextBox = new DataGridViewTextBoxColumn
                {
                    Name = nombre,
                    DataPropertyName = nombre,
                    HeaderText = texto,
                    Width = width,
                    MaxInputLength = maxImputLength,
                    ValueType = tipo
                };
                columnTextBox.DefaultCellStyle.Alignment = alinear;
                columnTextBox.Visible = visible;
                this.Columns.Insert(pos, columnTextBox);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Adicionar una columna de Tipo MaskedTextBox al TGGrid
        /// </summary>
        /// <param name="pos">Posición de la columna</param>
        /// <param name="nombre">Nombre de la columna</param>
        /// <param name="texto">Caption de la columna</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="mascara">Máscara de la columna</param>
        public void AddMaskedTextBoxColumn(int pos, string nombre, string texto, int width, string mascara)
        {
            try
            {
                MaskedTextBoxColumn columnMaskedTextBox = new MaskedTextBoxColumn
                {
                    Name = nombre,
                    DataPropertyName = nombre,
                    HeaderText = texto,
                    Width = width,
                    Mask = mascara
                };
                this.Columns.Insert(pos, columnMaskedTextBox);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Adicionar una columna de Tipo Calendar al TGGrid
        /// </summary>
        /// <param name="pos">Posición de la columna</param>
        /// <param name="nombre">Nombre de la columna</param>
        /// <param name="texto">Caption de la columna</param>
        /// <param name="width">Ancho de la columna</param>
        public void AddCalendarColumn(int pos, string nombre, string texto, int width)
        {
            try
            {
                CalendarColumn columnCalendarFecha = new CalendarColumn
                {
                    Name = nombre,
                    DataPropertyName = nombre,
                    HeaderText = texto,
                    Width = width
                };
                this.Columns.Insert(pos, columnCalendarFecha);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Adicionar una columna de Tipo MaskedTextBox al TGGrid
        /// </summary>
        /// <param name="pos">Posición de la columna</param>
        /// <param name="nombre">Nombre de la columna</param>
        /// <param name="texto">Caption de la columna</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="mascara">Máscara de la columna</param>
        public void AddImageColumn(int pos, string nombre, string texto, int width, DataGridViewContentAlignment alinear)
        {
            try
            {
                DataGridViewImageColumn columnImage = new DataGridViewImageColumn
                {
                    Name = nombre,
                    DataPropertyName = nombre,
                    HeaderText = texto
                };
                columnImage.DefaultCellStyle.Alignment = alinear;
                columnImage.Width = width;
        
                this.Columns.Insert(pos, columnImage);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Adicionar una columna de Tipo CheckBox al TGGrid
        /// </summary>
        /// <param name="pos">Posición de la columna</param>
        /// <param name="nombre">Nombre de la columna</param>
        /// <param name="width">Ancho de la columna</param>
        /// <param name="tipo">Tipo de la columna</param>
        /// <param name="alinear">Alinear la columna</param>
        public void AddCheckBoxColumn(int pos, string nombre, int width, DataGridViewContentAlignment alinear, bool visible)
        {
            try
            {
                DataGridViewCheckBoxColumn columnCheckBox = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "",
                    Width = width,
                    Name = nombre
                };
                columnCheckBox.DefaultCellStyle.Alignment = alinear;
                columnCheckBox.Visible = visible;
                this.Columns.Insert(pos, columnCheckBox);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        public void AddRowNumber(DataGridViewContentAlignment align)
        {
            try
            {
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    this.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }

                string num = this.Rows.Count.ToString();
                this.RowHeadersWidth = this.rowHeaderInitWidth + (7 * num.Length);
                this.RowHeadersDefaultCellStyle.Alignment = align;
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Pone la fuente en negrita de una fila  -- No funciona !!
        /// </summary>
        /// <param name="indice"></param>
        public void RowBold(int indice)
        {
            try
            {
                /*
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Font = new Font(this.Font, FontStyle.Bold);

                this.Rows[indice].DefaultCellStyle.ApplyStyle(style);

                /*
                DataGridViewCellStyle fooCellStyle = new DataGridViewCellStyle();
                fooCellStyle.ForeColor = color;
                for (int i = 0; i < this.tgGridLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells.Count; i++)
                {
                    this.tgGridLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells[i].Style.ApplyStyle(fooCellStyle);
                }
                */
                

                this.Rows[indice].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        #endregion

        #region Private Methods
        private void AplicarEstilo()
        {
        }
        #endregion

        #region Eventos

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            //Chequear si es necesario adicionar una fila (en el caso que se esté introduciendo
            //información en la última fila, se habilitará una nueva fila)
            if (this.addUltimaFilaSiNoHayDisponile)
            {
                if (this.CurrentCell != null && this.CurrentCell.RowIndex == this.dsDatos.Tables[this.nombreTabla].Rows.Count - 1)
                {
                    if (this.CurrentCell.Value.ToString().Trim() != "") this.AdicionarFila();
                }
            }

            //Falta ... actualizar el campo No apuntes, para ellos se chequeara si se esta en la columna cuenta de mayor entonces se recorrera todas las filas
            //y se contara las q tengan valor en ese campo
        }
        /*
        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }*/

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e = new KeyEventArgs(Keys.Tab);
            base.OnKeyDown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.CurrentCell is DataGridViewTextBoxCell &&
                this.CurrentCell.IsInEditMode &&
                (keyData == Keys.Enter || keyData == Keys.Return))
            {
                //SendKeys.Send("+~");  //Enter
                SendKeys.Send("{TAB}"); //Tabulador
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

    }


    #region  CalendarColumn
    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn()
            : base(new CalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class CalendarCell : DataGridViewTextBoxCell
    {
        public CalendarCell()
            : base()
        {
            // Use the short date format. 
            if (GlobalVar.CGFormatoFecha == "") this.Style.Format = "d";
        }


        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value. 
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;

            //Formato de la fecha
            if (GlobalVar.CGFormatoFecha != "")
            {
                ctl.Format = DateTimePickerFormat.Custom;
                ctl.CustomFormat = GlobalVar.CGFormatoFecha;
            }
            
            // Use the default row value when Value property is null. 
            if (this.Value == null)
            {
                ctl.Value = (DateTime)this.DefaultNewRowValue;
            }
            else
            {
                string valor = "";

                if (this.Value != null)
                    try { valor = (String)this.Value; }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                if (valor == "")
                {
                    ctl.Value = (DateTime)this.DefaultNewRowValue;
                }
                else
                    try
                    {
                        DateTime fecha = DateTime.ParseExact(valor, GlobalVar.CGFormatoFecha, System.Globalization.CultureInfo.CurrentCulture);

                        //ctl.Value = (DateTime)this.Value;
                        ctl.Value = fecha;
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses. 
                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains. 

                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value. 
                return DateTime.Now;
            }
        }
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            this.Format = DateTimePickerFormat.Short;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue  
        // property. 
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is  
                        // null, empty, or not in the format of a date. 
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch (Exception ex)
                    {
                        GlobalVar.Log.Error(ex.Message);

                        // In the case of an exception, just use the  
                        // default value so we're not left with a null 
                        // value. 
                        this.Value = DateTime.Now;
                    }
                }
            }
        }

        // Implements the  
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method. 
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the  
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method. 
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex  
        // property. 
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey  
        // method. 
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed. 
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit  
        // method. 
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl 
        // .RepositionEditingControlOnValueChange property. 
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlDataGridView property. 
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlValueChanged property. 
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingPanelCursor property. 
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell 
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }

    ////EJEMPLO
    /*
    public class Form1 : Form
    {
        private DataGridView dataGridView1 = new DataGridView();

        [STAThreadAttribute()]
        public static void Main()
        {
            Application.Run(new Form1());
        }

        public Form1()
        {
            this.dataGridView1.Dock = DockStyle.Fill;
            this.Controls.Add(this.dataGridView1);
            this.Load += new EventHandler(Form1_Load);
            this.Text = "DataGridView calendar column demo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CalendarColumn col = new CalendarColumn();
            this.dataGridView1.Columns.Add(col);
            this.dataGridView1.RowCount = 5;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Cells[0].Value = DateTime.Now;
            }
        }
    }     
     */
    #endregion

    #region MaskedTextBoxColumn
    /// <summary>
    /// The base object for the custom column type.  Programmers manipulate
    /// the column types most often when working with the DataGridView, and
    /// this one sets the basics and Cell Template values controlling the
    /// default behaviour for cells of this column type.
    /// </summary>
    public class MaskedTextBoxColumn : DataGridViewColumn
    {
        private string mask;
        private char promptChar;
        private bool includePrompt;
        private bool includeLiterals;
        private Type validatingType;

        /// <summary>
        /// Initializes a new instance of this class, making sure to pass
        /// to its base constructor an instance of a MaskedTextBoxCell
        /// class to use as the basic template.
        /// </summary>
        public MaskedTextBoxColumn()
            : base(new MaskedTextBoxCell())
        {
        }

        /// <summary>
        /// Routine to convert from boolean to DataGridViewTriState.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static DataGridViewTriState TriBool(bool value)
        {
            return value ? DataGridViewTriState.True
                         : DataGridViewTriState.False;
        }


        /// <summary>
        /// The template cell that will be used for this column by default,
        /// unless a specific cell is set for a particular row.
        ///
        /// A MaskedTextBoxCell cell which will serve as the template cell
        /// for this column.
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }

            set
            {
                //  Only cell types that derive from MaskedTextBoxCell are supported
                // as the cell template.
                if (value != null && !value.GetType().IsAssignableFrom(
                    typeof(MaskedTextBoxCell)))
                {
                    string s = "Cell type is not based upon the MaskedTextBoxCell.";
                    //CustomColumnMain.GetResourceManager().GetString("excNotMaskedTextBox");
                    throw new InvalidCastException(s);
                }

                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// Indicates the Mask property that is used on the MaskedTextBox
        /// for entering new data into cells of this type.
        ///
        /// See the MaskedTextBox control documentation for more details.
        /// </summary>
        public virtual string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.mask != value)
                {
                    this.mask = value;

                    //
                    // First, update the value on the template cell.
                    //
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.Mask = value;

                    //
                    // Now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.Mask = value;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// By default, the MaskedTextBox uses the underscore (_) character
        /// to prompt for required characters.  This propertly lets you
        /// choose a different one.
        ///
        /// See the MaskedTextBox control documentation for more details.
        /// </summary>
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.promptChar != value)
                {
                    this.promptChar = value;

                    //
                    // First, update the value on the template cell.
                    //
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.PromptChar = value;

                    //
                    // Now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.PromptChar = value;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether any unfilled characters in the mask should be
        /// be included as prompt characters when somebody asks for the text
        /// of the MaskedTextBox for a particular cell programmatically.
        ///
        /// See the MaskedTextBox control documentation for more details.
        /// </summary>
        public virtual bool IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                MaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.includePrompt != value)
                {
                    this.includePrompt = value;

                    //
                    // First, update the value on the template cell.
                    //
                    mtbc = (MaskedTextBoxCell)this.CellTemplate;
                    mtbc.IncludePrompt = TriBool(value);

                    //
                    // Now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is MaskedTextBoxCell)
                            {
                                mtbc = (MaskedTextBoxCell)dgvc;
                                mtbc.IncludePrompt = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Controls whether or not literal (non-prompt) characters should
        /// be included in the output of the Text property for newly entered
        /// data in a cell of this type.
        ///
        /// See the MaskedTextBox control documentation for more details.
        /// </summary>
        public virtual bool IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.includeLiterals != value)
                {
                    this.includeLiterals = value;

                    //
                    // First, update the value on the template cell.
                    //
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.IncludeLiterals = TriBool(value);

                    //
                    // Now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {

                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.IncludeLiterals = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicates the type against any data entered in the MaskedTextBox
        /// should be validated.  The MaskedTextBox control will attempt to
        /// instantiate this type and assign the value from the contents of
        /// the text box.  An error will occur if it fails to assign to this
        /// type.
        ///
        /// See the MaskedTextBox control documentation for more details.
        /// </summary>
        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.validatingType != value)
                {
                    this.validatingType = value;

                    //
                    // First, update the value on the template cell.
                    //
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.ValidatingType = value;

                    //
                    // Now set it on all cells in other rows as well.
                    //
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.ValidatingType = value;
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region MaskedTextBoxCell
    class MaskedTextBoxCell : DataGridViewTextBoxCell
    {
        private string mask;
        private char promptChar;
        private DataGridViewTriState includePrompt;
        private DataGridViewTriState includeLiterals;
        private Type validatingType;

        /// <summary>
        /// Initializes a new instance of this class.  Fortunately, there's
        /// not much to do here except make sure that our base class is also
        /// initialized properly.
        /// </summary>
        public MaskedTextBoxCell()
            : base()
        {
            this.mask = "";
            this.promptChar = '_';
            this.includePrompt = DataGridViewTriState.NotSet;
            this.includeLiterals = DataGridViewTriState.NotSet;
            this.validatingType = typeof(string);
        }

        /// <summary>
        /// Whenever the user is to begin editing a cell of this type, the editing
        /// control must be created, which in this column type's case is a subclass
        /// of the MaskedTextBox control.
        ///
        /// This routine sets up all the properties and values on this control
        /// before the editing begins.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex,
            object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            try
            {
                MaskedTextBoxEditingControl mtbEditingCtrl;
                MaskedTextBoxColumn mtbColumn;
                DataGridViewColumn dgvColumn;

                base.InitializeEditingControl(rowIndex, initialFormattedValue,
                                              dataGridViewCellStyle);

                mtbEditingCtrl = DataGridView.EditingControl as MaskedTextBoxEditingControl;

                //
                // Set up props that are specific to the MaskedTextBox
                //

                dgvColumn = this.OwningColumn;   // this.DataGridView.Columns[this.ColumnIndex];
                if (dgvColumn is MaskedTextBoxColumn)
                {
                    mtbColumn = dgvColumn as MaskedTextBoxColumn;

                    //
                    // get the mask from this instance or the parent column.
                    //
                    if (string.IsNullOrEmpty(this.mask))
                    {
                        mtbEditingCtrl.Mask = mtbColumn.Mask;
                    }
                    else
                    {
                        mtbEditingCtrl.Mask = this.mask;
                    }

                    //
                    // Prompt char.
                    //
                    mtbEditingCtrl.PromptChar = this.PromptChar;

                    //
                    // IncludePrompt
                    //
                    if (this.includePrompt == DataGridViewTriState.NotSet)
                    {
                        //mtbEditingCtrl.IncludePrompt = mtbcol.IncludePrompt;
                    }
                    else
                    {
                        //mtbEditingCtrl.IncludePrompt = BoolFromTri(this.includePrompt);
                    }

                    //
                    // IncludeLiterals
                    //
                    if (this.includeLiterals == DataGridViewTriState.NotSet)
                    {
                        //mtbEditingCtrl.IncludeLiterals = mtbcol.IncludeLiterals;
                    }
                    else
                    {
                        //mtbEditingCtrl.IncludeLiterals = BoolFromTri(this.includeLiterals);
                    }

                    //
                    // Finally, the validating type ...
                    //
                    if (this.ValidatingType == null)
                    {
                        mtbEditingCtrl.ValidatingType = mtbColumn.ValidatingType;
                    }
                    else
                    {
                        mtbEditingCtrl.ValidatingType = this.ValidatingType;
                    }

                    if (this.Value == DBNull.Value) mtbEditingCtrl.Text = "";
                    else mtbEditingCtrl.Text = (string)this.Value;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
        }

        /// <summary>
        /// Returns the type of the control that will be used for editing
        /// cells of this type.  This control must be a valid Windows Forms
        /// control and must implement IDataGridViewEditingControl.
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(MaskedTextBoxEditingControl);
            }
        }

        /// <summary>
        /// A string value containing the Mask against input for cells of
        /// this type will be verified.
        /// </summary>
        public virtual string Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                this.mask = value;
            }
        }

        /// <summary>
        /// The character to use for prompting for new input.
        /// </summary>
        public virtual char PromptChar
        {
            get
            {
                return this.promptChar;
            }
            set
            {
                this.promptChar = value;
            }
        }


        /// <summary>
        /// A boolean indicating whether to include prompt characters in
        /// the Text property's value.
        /// </summary>
        public virtual DataGridViewTriState IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                this.includePrompt = value;
            }
        }

        /// <summary>
        /// A boolean value indicating whether to include literal characters
        /// in the Text property's output value.
        /// </summary>
        public virtual DataGridViewTriState IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                this.includeLiterals = value;
            }
        }

        /// <summary>
        /// A Type object for the validating type.
        /// </summary>
        public virtual Type ValidatingType
        {
            get
            {
                return this.validatingType;
            }
            set
            {
                this.validatingType = value;
            }
        }

        /// <summary>
        /// Quick routine to convert from DataGridViewTriState to boolean.
        /// True goes to true while False and NotSet go to false.
        /// </summary>
        /// <param name="tri"></param>
        /// <returns></returns>
        protected static bool BoolFromTri(DataGridViewTriState tri)
        {
            return (tri == DataGridViewTriState.True) ? true : false;
        }
    }
    #endregion

    #region MaskedTextBoxEditingControl
    public class MaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
    {
        protected int rowIndex;
        protected DataGridView dataGridView;
        protected bool valueChanged = false;

        public MaskedTextBoxEditingControl()
        {

        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // Let the DataGridView know about the value change
            NotifyDataGridViewOfValueChange();
        }

        /// <summary>
        /// Notify DataGridView that the value has changed.
        /// </summary>
        protected virtual void NotifyDataGridViewOfValueChange()
        {
            this.valueChanged = true;
            if (this.dataGridView != null)
            {
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }


        #region IDataGridViewEditingControl Members

        //  Indicates the cursor that should be shown when the user hovers their
        //  mouse over this cell when the editing control is shown.
        public Cursor EditingPanelCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }


        //  Returns or sets the parent DataGridView.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }

            set
            {
                this.dataGridView = value;
            }
        }


        //  Sets/Gets the formatted value contents of this cell.
        public object EditingControlFormattedValue
        {
            set
            {
                this.Text = value.ToString();
                NotifyDataGridViewOfValueChange();
            }
            get
            {
                return this.Text;
            }

        }

        //   Get the value of the editing control for formatting.
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        //  Process input key and determine if the key should be used for the editing control
        //  or allowed to be processed by the grid. Handle cursor movement keys for the MaskedTextBox
        //  control; otherwise if the DataGridView doesn't want the input key then let the editing control handle it.
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                    //
                    // If the end of the selection is at the end of the string
                    // let the DataGridView treat the key message
                    //
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == this.ToString().Length))
                    {
                        return true;
                    }
                    break;

                case Keys.Left:
                    //
                    // If the end of the selection is at the begining of the
                    // string or if the entire text is selected send this character
                    // to the dataGridView; else process the key event.
                    //
                    if (!(this.SelectionLength == 0
                          && this.SelectionStart == 0))
                    {
                        return true;
                    }
                    break;

                case Keys.Home:
                case Keys.End:
                    if (this.SelectionLength != this.ToString().Length)
                    {
                        return true;
                    }
                    break;

                case Keys.Prior:
                case Keys.Next:
                    if (this.valueChanged)
                    {
                        return true;
                    }
                    break;

                case Keys.Delete:
                    if (this.SelectionLength > 0 || this.SelectionStart < this.ToString().Length)
                    {
                        return true;
                    }
                    break;
            }

            //
            // defer to the DataGridView and see if it wants it.
            //
            return !dataGridViewWantsInputKey;
        }


        //  Prepare the editing control for edit.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                SelectAll();
            }
            else
            {
                //
                // Do not select all the text, but position the caret at the
                // end of the text.
                //
                this.SelectionStart = this.ToString().Length;
            }
        }

        //  Indicates whether or not the parent DataGridView control should
        //  reposition the editing control every time value change is indicated.
        //  There is no need to do this for the MaskedTextBox.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }


        //  Indicates the row index of this cell.  This is often -1 for the
        //  template cell, but for other cells, might actually have a value
        //  greater than or equal to zero.
        public int EditingControlRowIndex
        {
            get
            {
                return this.rowIndex;
            }

            set
            {
                this.rowIndex = value;
            }
        }



        //  Make the MaskedTextBox control match the style and colors of
        //  the host DataGridView control and other editing controls
        //  before showing the editing control.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.TextAlign = TranslateAlignment(dataGridViewCellStyle.Alignment);
        }


        //  Gets or sets our flag indicating whether the value has changed.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }

            set
            {
                this.valueChanged = value;
            }
        }

        #endregion // IDataGridViewEditingControl.


        /// <summary>
        /// Routine to translate between DataGridView content alignments and text
        /// box horizontal alignments.
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        private static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
        {
            switch (align)
            {
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.BottomLeft:
                    return HorizontalAlignment.Left;

                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return HorizontalAlignment.Center;

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return HorizontalAlignment.Right;
            }

            throw new ArgumentException("Error: Invalid Content Alignment!");
        }
    }
    #endregion
}
