using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace ObjectModel
{
    public partial class TGGrid3Niveles :  UserControl
    {
        #region Variables
        public TGGrid MasterGrid = new TGGrid();
        public TGGrid DetailGrid = new TGGrid();
        public TGGrid TercerGrid = new TGGrid();

        public bool existeTercerGrid = false;

        //List<int> listcolumnIndex;
        
        static String ImageName = "toggle.png";
        String FilterColumnName = "";
        DataTable DetailgridDT;
        DataTable TercergridDT;
        int gridColumnIndex = 0;

        static String[] ImageNameArray = {"expand.png", "expand.png"};

        DataGridView shanuNestedDGV = new DataGridView();
        //String EventFucntions;
        # endregion

        //Set all the telerik Grid layout
        #region Layout

        public static void Layouts(DataGridView ShanuDGV, Color BackgroundColor, Color RowsBackColor, Color AlternatebackColor, Boolean AutoGenerateColumns, Color HeaderColor, Boolean HeaderVisual, Boolean RowHeadersVisible, Boolean AllowUserToAddRows)
        {
            //Grid Back ground Color
            ShanuDGV.BackgroundColor = BackgroundColor;

            //Grid Back Color
            ShanuDGV.RowsDefaultCellStyle.BackColor = RowsBackColor;

            //GridColumnStylesCollection Alternate Rows Backcolr
            ShanuDGV.AlternatingRowsDefaultCellStyle.BackColor = AlternatebackColor;

            // Auto generated here set to tru or false.
            ShanuDGV.AutoGenerateColumns = AutoGenerateColumns;
            //  ShanuDGV.DefaultCellStyle.Font = new Font("Verdana", 10.25f, FontStyle.Regular);
            // ShanuDGV.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 11, FontStyle.Regular);

            //Column Header back Color
            ShanuDGV.ColumnHeadersDefaultCellStyle.BackColor = HeaderColor;
            //
            ShanuDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //header Visisble
            ShanuDGV.EnableHeadersVisualStyles = HeaderVisual;

            // Enable the row header
            ShanuDGV.RowHeadersVisible = RowHeadersVisible;

            // to Hide the Last Empty row here we use false.
            ShanuDGV.AllowUserToAddRows = AllowUserToAddRows;
        }
        #endregion

        //Add your grid to your selected Control and set height,width,position of your grid.
        #region Variables
        public static void Generategrid(TGGrid DGV, Control cntrlName, int width, int height, int xval, int yval)
        {
            DGV.Location = new Point(xval, yval);
            DGV.Size = new Size(width, height);
            //ShanuDGV.Dock = docktyope.
            cntrlName.Controls.Add(DGV);
        }
        #endregion

        //List to Data Table Convert
        private static DataTable ListtoDataTable<T>(IEnumerable<T> DetailList)
        {
            Type type = typeof(T);
            var typeproperties = type.GetProperties();

            DataTable listToDT = new DataTable();
            foreach (PropertyInfo propInfo in typeproperties)
            {
                listToDT.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
            }

            foreach (T ListItem in DetailList)
            {
                object[] values = new object[typeproperties.Length];
                for (int i = 0; i < typeproperties.Length; i++)
                {
                    values[i] = typeproperties[i].GetValue(ListItem, null);
                }

                listToDT.Rows.Add(values);
            }

            return listToDT;
        }

        //Template Column In this column we can add Textbox,Lable,Check Box,Dropdown box and etc
        #region Templatecolumn
        public static void Templatecolumn(TGGrid ShanuDGV, ShanuControlTypes ShanuControlTypes, String cntrlnames, String Headertext, String ToolTipText, Boolean Visible, int width, DataGridViewTriState Resizable, DataGridViewContentAlignment cellAlignment, DataGridViewContentAlignment headerAlignment, Color CellTemplateBackColor, DataTable dtsource, String DisplayMember, String ValueMember, Color CellTemplateforeColor)
        {
            switch (ShanuControlTypes)
            {
                case ShanuControlTypes.CheckBox:
                    DataGridViewCheckBoxColumn dgvChk = new DataGridViewCheckBoxColumn();
                    dgvChk.ValueType = typeof(bool);
                    dgvChk.Name = cntrlnames;

                    dgvChk.HeaderText = Headertext;
                    dgvChk.ToolTipText = ToolTipText;
                    dgvChk.Visible = Visible;
                    dgvChk.Width = width;
                    dgvChk.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvChk.Resizable = Resizable;
                    dgvChk.DefaultCellStyle.Alignment = cellAlignment;
                    dgvChk.HeaderCell.Style.Alignment = headerAlignment;
                    if (CellTemplateBackColor.Name.ToString() != "Transparent")
                    {
                        dgvChk.CellTemplate.Style.BackColor = CellTemplateBackColor;
                    }
                    dgvChk.DefaultCellStyle.ForeColor = CellTemplateforeColor;
                    ShanuDGV.Columns.Add(dgvChk);
                    break;
                case ShanuControlTypes.BoundColumn:
                    DataGridViewColumn dgvbound = new DataGridViewTextBoxColumn();
                    dgvbound.DataPropertyName = cntrlnames;
                    dgvbound.Name = cntrlnames;
                    dgvbound.HeaderText = Headertext;
                    dgvbound.ToolTipText = ToolTipText;
                    dgvbound.Visible = Visible;
                    dgvbound.Width = width;
                    dgvbound.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvbound.Resizable = Resizable;
                    dgvbound.DefaultCellStyle.Alignment = cellAlignment;
                    dgvbound.HeaderCell.Style.Alignment = headerAlignment;
                    dgvbound.ReadOnly = true;
                    if (CellTemplateBackColor.Name.ToString() != "Transparent")
                    {
                        dgvbound.CellTemplate.Style.BackColor = CellTemplateBackColor;
                    }
                    dgvbound.DefaultCellStyle.ForeColor = CellTemplateforeColor;
                    ShanuDGV.Columns.Add(dgvbound);
                    break;
                case ShanuControlTypes.TextBox:
                    DataGridViewTextBoxColumn dgvText = new DataGridViewTextBoxColumn();
                    dgvText.ValueType = typeof(decimal);
                    dgvText.DataPropertyName = cntrlnames;
                    dgvText.Name = cntrlnames;
                    dgvText.HeaderText = Headertext;
                    dgvText.ToolTipText = ToolTipText;
                    dgvText.Visible = Visible;
                    dgvText.Width = width;
                    dgvText.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvText.Resizable = Resizable;
                    dgvText.DefaultCellStyle.Alignment = cellAlignment;
                    dgvText.HeaderCell.Style.Alignment = headerAlignment;
                    if (CellTemplateBackColor.Name.ToString() != "Transparent")
                    {
                        dgvText.CellTemplate.Style.BackColor = CellTemplateBackColor;
                    }
                    dgvText.DefaultCellStyle.ForeColor = CellTemplateforeColor;
                    ShanuDGV.Columns.Add(dgvText);
                    break;
                case ShanuControlTypes.ComboBox:
                    DataGridViewComboBoxColumn dgvcombo = new DataGridViewComboBoxColumn();
                    dgvcombo.ValueType = typeof(decimal);
                    dgvcombo.Name = cntrlnames;
                    dgvcombo.DataSource = dtsource;
                    dgvcombo.DisplayMember = DisplayMember;
                    dgvcombo.ValueMember = ValueMember;
                    dgvcombo.Visible = Visible;
                    dgvcombo.Width = width;
                    dgvcombo.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvcombo.Resizable = Resizable;
                    dgvcombo.DefaultCellStyle.Alignment = cellAlignment;
                    dgvcombo.HeaderCell.Style.Alignment = headerAlignment;
                    if (CellTemplateBackColor.Name.ToString() != "Transparent")
                    {
                        dgvcombo.CellTemplate.Style.BackColor = CellTemplateBackColor;

                    }
                    dgvcombo.DefaultCellStyle.ForeColor = CellTemplateforeColor;
                    ShanuDGV.Columns.Add(dgvcombo);
                    break;

                case ShanuControlTypes.Button:
                    DataGridViewButtonColumn dgvButtons = new DataGridViewButtonColumn();
                    dgvButtons.Name = cntrlnames;
                    dgvButtons.FlatStyle = FlatStyle.Popup;
                    dgvButtons.DataPropertyName = cntrlnames;
                    dgvButtons.Visible = Visible;
                    dgvButtons.Width = width;
                    dgvButtons.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvButtons.Resizable = Resizable;
                    dgvButtons.DefaultCellStyle.Alignment = cellAlignment;
                    dgvButtons.HeaderCell.Style.Alignment = headerAlignment;
                    if (CellTemplateBackColor.Name.ToString() != "Transparent")
                    {
                        dgvButtons.CellTemplate.Style.BackColor = CellTemplateBackColor;
                    }
                    dgvButtons.DefaultCellStyle.ForeColor = CellTemplateforeColor;
                    ShanuDGV.Columns.Add(dgvButtons);
                    break;
                case ShanuControlTypes.ImageColumn:
                    DataGridViewImageColumn dgvnestedBtn = new DataGridViewImageColumn();
                    dgvnestedBtn.Name = cntrlnames;
                    ImageName = "expand.png";
                  
                    dgvnestedBtn.Image = Image.FromFile(ImageName);//global::ShanuDGVHelper_Demo.Properties.Resources.toggle;
                    // dgvnestedBtn.DataPropertyName = cntrlnames;
                    dgvnestedBtn.Visible = Visible;
                    dgvnestedBtn.Width = width;
                    dgvnestedBtn.SortMode = DataGridViewColumnSortMode.Automatic;
                    dgvnestedBtn.Resizable = Resizable;
                    dgvnestedBtn.DefaultCellStyle.Alignment = cellAlignment;
                    dgvnestedBtn.HeaderCell.Style.Alignment = headerAlignment;
                    ShanuDGV.Columns.Add(dgvnestedBtn);
                    break;
            }

        }

        #endregion


        // Image Colukmn Click evnet
        #region Image Colukmn Click Event
        public void DGVMasterGridClickEvents(TGGrid MasterDGV, TGGrid DetailDGV, int columnIndexs, EventTypes eventtype, ShanuControlTypes types, DataTable DetailTable, String FilterColumn)
        {
            this.MasterGrid = MasterDGV;
            this.DetailGrid = DetailDGV;
            gridColumnIndex = columnIndexs;
            DetailgridDT = DetailTable;
            FilterColumnName = FilterColumn;

            this.MasterGrid.CellContentClick += new DataGridViewCellEventHandler(masterDGVs_CellContentClick_Event);
        }

        private void masterDGVs_CellContentClick_Event(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewImageColumn cols = (DataGridViewImageColumn)this.MasterGrid.Columns[0];
         
            // cols.Image = Image.FromFile(ImageName);
            this.MasterGrid.Rows[e.RowIndex].Cells[0].Value = Image.FromFile("expand.png");

             if (e.ColumnIndex == gridColumnIndex)
             {
                 //if (ImageName == "expand.png")
                 if (ImageNameArray[0] == "expand.png")
                 {
                     this.DetailGrid.Visible = true;
                     //ImageName = "toggle.png";
                     ImageNameArray[0] = "toggle.png";
                     // cols.Image = Image.FromFile(ImageName);
                     this.MasterGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Image.FromFile(ImageNameArray[0]);


                     String Filterexpression = this.MasterGrid.Rows[e.RowIndex].Cells[FilterColumnName].Value.ToString();

                     this.MasterGrid.Controls.Add(this.DetailGrid);

                     Rectangle dgvRectangle = this.MasterGrid.GetCellDisplayRectangle(1, e.RowIndex, true);
                     this.DetailGrid.Size = new Size(this.MasterGrid.Width - 200, 200);
                     this.DetailGrid.Location = new Point(dgvRectangle.X, dgvRectangle.Y + 20);


                     DataView detailView = new DataView(DetailgridDT);
                     detailView.RowFilter = FilterColumnName + " = '" + Filterexpression + "'";
                     if (detailView.Count <= 0)
                     {
                         MessageBox.Show("No Details Found");
                     }
                     this.DetailGrid.DataSource = detailView;
                 }
                 else
                 {
                     //ImageName = "expand.png";
                     ImageNameArray[0] = "expand.png";
                     //  cols.Image = Image.FromFile(ImageName);
                     this.MasterGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Image.FromFile(ImageNameArray[0]);
                     this.DetailGrid.Visible = false;
                 }                 
             }
             else
             {
                 this.DetailGrid.Visible = false;
                 
             }
        }
        #endregion

        public void DGVDetailGridClickEvents(TGGrid DetailDGV, TGGrid TercerGridDGV, int columnIndexs, EventTypes eventtype, ShanuControlTypes types, DataTable TercerTable, String FilterColumn)
        {

            this.DetailGrid = DetailDGV;

            if (TercerGridDGV != null)
            {
                this.TercerGrid = TercerGridDGV;
                gridColumnIndex = columnIndexs;
                TercergridDT = TercerTable;
                FilterColumnName = FilterColumn;  
            }

            this.DetailGrid.CellContentClick += new DataGridViewCellEventHandler(detailDGVs_CellContentClick_Event);

            /*DetailDGVs = ShanuDetailDGV;

            DetailDGVs.CellContentClick += new DataGridViewCellEventHandler(detailDGVs_CellContentClick_Event);
            */

        }

         /* private void detailDGVs_CellContentClick_Event(object sender, DataGridViewCellEventArgs e)
          {
              MessageBox.Show("Detail grid Clicked : You clicked on " + DetailDGVs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
          }
        */

        private void detailDGVs_CellContentClick_Event(object sender, DataGridViewCellEventArgs e)
        {
            if (existeTercerGrid)
            {
                DataGridViewImageColumn cols = (DataGridViewImageColumn)this.DetailGrid.Columns[0];

                // cols.Image = Image.FromFile(ImageName);
                this.DetailGrid.Rows[e.RowIndex].Cells[0].Value = Image.FromFile("expand.png");

                if (e.ColumnIndex == gridColumnIndex)
                {
                    //if (ImageName == "expand.png")
                    if (ImageNameArray[1] == "expand.png")
                    {
                        this.TercerGrid.Visible = true;
                        //ImageName = "toggle.png";
                        ImageNameArray[1] = "toggle.png";
                        // cols.Image = Image.FromFile(ImageName);
                        this.DetailGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Image.FromFile(ImageNameArray[1]);


                        String Filterexpression = this.DetailGrid.Rows[e.RowIndex].Cells[FilterColumnName].Value.ToString();

                        this.DetailGrid.Controls.Add(this.TercerGrid);

                        Rectangle dgvRectangle = this.DetailGrid.GetCellDisplayRectangle(1, e.RowIndex, true);
                        this.TercerGrid.Size = new Size(this.DetailGrid.Width - 200, 200);
                        this.TercerGrid.Location = new Point(dgvRectangle.X, dgvRectangle.Y + 20);

                        DataView tercerView = new DataView(TercergridDT);
                        tercerView.RowFilter = FilterColumnName + " = '" + Filterexpression + "'";
                        if (tercerView.Count <= 0)
                        {
                            MessageBox.Show("No Details Found");
                        }
                        this.TercerGrid.DataSource = tercerView;
                    }
                    else
                    {
                        //ImageName = "expand.png";
                        ImageNameArray[1] = "expand.png";
                        //  cols.Image = Image.FromFile(ImageName);
                        this.DetailGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Image.FromFile(ImageNameArray[1]);
                        this.TercerGrid.Visible = false;
                    }
                }
                else
                {
                    this.TercerGrid.Visible = false;
                }
            }
        }


          public void DGVTercerGridClickEvents(TGGrid DetailDGV)
          {

              this.TercerGrid = DetailDGV;

              this.DetailGrid.CellContentClick += new DataGridViewCellEventHandler(tercerGridDGVs_CellContentClick_Event);


          }
          private void tercerGridDGVs_CellContentClick_Event(object sender, DataGridViewCellEventArgs e)
          {
              if (this.TercerGrid.Rows.Count > 0) MessageBox.Show("Detail grid Clicked : You clicked on " + this.TercerGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
          }

    }
}
//Enam decalaration for DataGridView Column Type ex like Textbox Column ,Button Column
public enum ShanuControlTypes { BoundColumn, TextBox, ComboBox, CheckBox, DateTimepicker, Button, NumericTextBox, ColorDialog, ImageColumn }
public enum EventTypes { CellClick, cellContentClick, EditingControlShowing }

