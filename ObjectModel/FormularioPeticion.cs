using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Telerik.WinControls.UI;

namespace ObjectModel
{
    public class FormularioPeticion
    {
        private DataSet dsFormulario;

        private string formCode;
        public string FormCode
        {
            get
            {
                return (this.formCode);
            }
            set
            {
                this.formCode = value;
            }
        }

        private string ficheroExtension;
        public string FicheroExtension
        {
            get
            {
                return (this.ficheroExtension);
            }
            set
            {
                this.ficheroExtension = value;
            }
        }

        private Form formulario;
        public Form Formulario
        {
            get
            {
                return (this.formulario);
            }
            set
            {
                this.formulario = value;
            }
        }

        private string path;
        public string Path
        {
            get
            {
                return (this.path);
            }
            set
            {
                this.path= value;
            }
        }

        private string fichero;
        public string Fichero
        {
            get
            {
                return (this.fichero);
            }
            set
            {
                this.fichero = value;
            }
        }

        private string descripcion = "";
        public string Descripcion
        {
            get
            {
                return (this.descripcion);
            }
            set
            {
                this.descripcion = value;
            }
        }

        public FormularioPeticion()
        {
        }

        #region Métodos públicos
        /// <summary>
        /// Graba un fichero con los datos de la petición actual
        /// </summary>
        /// <returns></returns>
        public string GrabarPeticion()
        {
            string result = "";

            try
            {
                //Crear el DataSet con las tablas Cabecera y Control
                this.CrearDataSet();

                //Obtener del formulario los controles, tipos y valores para grabar el fichero
                this.ObtenerFormControlesValores(this.formulario.Controls);

                //Chequear que exista la extensión en el nombre del fichero, sino añadirla
                if (this.ficheroExtension != "")
                {
                    if (this.fichero.IndexOf("." + this.ficheroExtension) == -1) this.fichero += "." + this.ficheroExtension;
                }

                //Grabar el nuevo fichero de petición
                string pathNombreFichero = this.path;
                if (pathNombreFichero.Substring(pathNombreFichero.Length - 1, 1) != "\\") pathNombreFichero += "\\";
                pathNombreFichero += this.fichero;
                this.dsFormulario.WriteXml(pathNombreFichero);
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /*
        /// <summary>
        /// Carga una petición anterior (un fichero xml) en el formulario actual
        /// </summary>
        /// <returns></returns>
        public string CargarPeticion()
        {
            string result = "";

            try
            {
                this.dsFormulario = new DataSet();
                this.dsFormulario.ReadXml(@"C:\VS2010_Projects\FinanzasNet\ModConsultaInforme\informes\peticion.xml");
                
                if (this.dsFormulario != null && this.dsFormulario.Tables != null && this.dsFormulario.Tables.Count > 0 && 
                    this.dsFormulario.Tables["Cabecera"].Rows.Count > 0)
                {
                    if (this.dsFormulario.Tables["Cabecera"].Rows[0]["Codigo"].ToString() == this.formCode)
                    {
                        if (this.dsFormulario.Tables["Control"].Rows.Count > 0)
                        {
                            string controlName = "";
                            string controlType = "";
                            string controlValue = "";

                            string[] valores;

                            TextBox textBox;
                            CheckBox checkBox;
                            RadioButton radioButton;
                            MaskedTextBox maskedTextBox;
                            ListBox listBox;
                            TGTexBoxSel tgTexBoxSel;
                            ComboBox comboBox;
                            DateTimePicker dateTimePicker;

                            for (int i = 0; i < this.dsFormulario.Tables["Control"].Rows.Count; i++)
                            {
                                controlName = this.dsFormulario.Tables["Control"].Rows[i]["Nombre"].ToString();
                                controlType = this.dsFormulario.Tables["Control"].Rows[i]["Tipo"].ToString();
                                controlValue = this.dsFormulario.Tables["Control"].Rows[i]["Valor"].ToString();

                                try
                                {
                                    switch (controlType)
                                    {
                                        case "TextBox":
                                            textBox = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as TextBox;
                                            textBox.Text = controlValue;
                                            break;
                                        case "CheckBox":
                                            checkBox = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as CheckBox;
                                            if (controlValue == "1") checkBox.Checked = true;
                                            else checkBox.Checked = false;
                                            break;
                                        case "RadioButton":
                                            radioButton = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as RadioButton;
                                            if (controlValue == "1") radioButton.Checked = true;
                                            else radioButton.Checked = false;
                                            break;
                                        case "MaskedTextBox":
                                            maskedTextBox = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as MaskedTextBox;
                                            maskedTextBox.Text = controlValue;
                                            break;
                                        case "ListBox":
                                            listBox = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as ListBox;
                                            listBox.Items.Clear();

                                            valores = controlValue.Split('|');
                                            for (int j = 0; j < valores.Length; j++)
                                            {
                                                listBox.Items.Add(valores[j]);
                                            }
                                            break;
                                        case "TGTexBoxSel":
                                            tgTexBoxSel = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as TGTexBoxSel;
                                            tgTexBoxSel.Text = controlValue;
                                            break;
                                        case "ComboBox":
                                            comboBox = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as ComboBox;
                                            comboBox.SelectedValue = controlValue;
                                            break;
                                        case "DateTimePicker":
                                            if (controlValue != "")
                                            {
                                                dateTimePicker = this.formulario.Controls.Find(controlName, true).FirstOrDefault() as DateTimePicker;
                                                dateTimePicker.Value = Convert.ToDateTime(controlValue);
                                            }
                                            break;
                                    }

                                }
                                catch  (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                            }
                        }
                
                    }
                }

            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }
        */

        /// <summary>
        /// Carga una petición anterior (un fichero xml) en el formulario actual
        /// </summary>
        /// <returns></returns>
        public string CargarPeticionDataTable(DataTable dtPeticion)
        {
            string result = "";

            try
            {
                if (dtPeticion.Rows.Count > 0)
                {
                    string[] valores;

                    string columnName = "";
                    string valor = "";
                    Control ctrl;

                    for (int i = 0; i < dtPeticion.Columns.Count; i++)
                    {
                        columnName = dtPeticion.Columns[i].ColumnName;
                        if (columnName != "Archivo" && columnName != "Descripcion")
                        {
                            ctrl = this.formulario.Controls.Find(columnName, true).FirstOrDefault();
                            valor = dtPeticion.Rows[0][columnName].ToString();

                            try
                            {
                                switch (ctrl.GetType().Name)
                                {
                                    case "RadTextBoxControl":
                                        ((RadTextBoxControl)ctrl).Text = valor;
                                        break;
                                    case "RadCheckBox":
                                        if (valor == "1") ((RadCheckBox)ctrl).Checked = true;
                                        else ((RadCheckBox)ctrl).Checked = false;
                                        break;
                                    case "RadRadioButton":
                                        if (valor == "1") ((RadRadioButton)ctrl).IsChecked = true;
                                        else ((RadRadioButton)ctrl).IsChecked = false;
                                        break;
                                    case "RadMaskedEditBox":
                                        ((RadMaskedEditBox)ctrl).Text = valor;
                                        break;
                                    case "RadListControl":
                                        ((RadListControl)ctrl).Items.Clear();
                                        valores = valor.Split('|');
                                        for (int j = 0; j < valores.Length; j++)
                                        {
                                            ((RadListControl)ctrl).Items.Add(valores[j]);
                                        }
                                        break;
                                    case "RadButtonTextBox":
                                        ((RadButtonTextBox)ctrl).Text = valor;
                                        break;
                                    case "RadDropDownList":
                                        ((RadDropDownList)ctrl).SelectedValue = valor;
                                        break;
                                    case "RadDateTimePicker":
                                        if (valor != "") ((RadDateTimePicker)ctrl).Value = Convert.ToDateTime(valor);
                                        break;
                                }
                            }
                            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }
                        }
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (result);
        }

        /// <summary>
        /// Devuelve un DataTable con todos los ficheros de peticiones generados para un formulario
        /// </summary>
        /// <returns></returns>
        public DataTable ListarPeticion()
        {
            DataTable dtPeticiones = new DataTable
            {
                TableName = "Peticiones"
            };

            try
            {
                DataSet dsAux = new DataSet();
                int contFichero = 0;

                DirectoryInfo dir = new DirectoryInfo(this.path);
                //FileInfo[] fileList = dir.GetFiles("*.xml", SearchOption.AllDirectories);
                FileInfo[] fileList = dir.GetFiles("*." + this.ficheroExtension, SearchOption.AllDirectories);

                string controlName = "";
                string controlValue = "";

                foreach (FileInfo FI in fileList)
                {
                    try 
                    {
                        dsAux.ReadXml(FI.FullName);

                        if (dsAux != null && dsAux.Tables != null && dsAux.Tables.Count > 0 && dsAux.Tables["Cabecera"].Rows.Count > 0)
                        {
                            //Verificar que sea un fichero de petición del tipo de formulario solicitado
                            if (dsAux.Tables["Cabecera"].Rows[0]["Codigo"].ToString() == this.formCode)
                            {
                                if (dsAux.Tables["Control"].Rows.Count > 0)
                                {
                                    controlName = "";
                                    controlValue = "";

                                    DataRow row = dtPeticiones.NewRow();
                                    
                                    if (!dtPeticiones.Columns.Contains("Archivo")) dtPeticiones.Columns.Add("Archivo", typeof(String));
                                    if (!dtPeticiones.Columns.Contains("Descripcion")) dtPeticiones.Columns.Add("Descripcion", typeof(String));

                                    //row["Archivo"] = FI.FullName;
                                    row["Archivo"] = FI.Name;
                                    
                                    row["Descripcion"] = dsAux.Tables["Cabecera"].Rows[0]["Descripcion"].ToString();

                                    for (int i = 0; i < dsAux.Tables["Control"].Rows.Count; i++)
                                    {
                                        controlName = dsAux.Tables["Control"].Rows[i]["Nombre"].ToString();
                                        controlValue = dsAux.Tables["Control"].Rows[i]["Valor"].ToString();

                                        if (!dtPeticiones.Columns.Contains(controlName)) dtPeticiones.Columns.Add(controlName, typeof(String));

                                        row[controlName] = controlValue;

                                    }
                                    dtPeticiones.Rows.Add(row);
                                }
                            }
                        }

                        contFichero++;
                        dsAux.Clear();
                    }
                    catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(ex.Message); }

            return (dtPeticiones);
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Crea el dataset que contendrá la info del xml de peticion
        /// </summary>
        private void CrearDataSet()
        {
            this.dsFormulario = new DataSet
            {
                DataSetName = "Formulario"
            };

            //Tabla Cabecera
            DataTable dtCabecera = new DataTable
            {
                TableName = "Cabecera"
            };
            dtCabecera.Columns.Add("Codigo", typeof(string));
            dtCabecera.Columns.Add("Descripcion", typeof(string));
            this.dsFormulario.Tables.Add(dtCabecera);

            DataRow row = this.dsFormulario.Tables["Cabecera"].NewRow();
            row["Codigo"] = this.formCode;
            row["Descripcion"] = this.descripcion;
            this.dsFormulario.Tables["Cabecera"].Rows.Add(row);

            //Tabla Control
            DataTable dtControles = new DataTable
            {
                TableName = "Control"
            };
            dtControles.Columns.Add("Nombre", typeof(string));
            dtControles.Columns.Add("Tipo", typeof(string));
            dtControles.Columns.Add("Valor", typeof(string));
            this.dsFormulario.Tables.Add(dtControles);
        }

        /// <summary>
        /// Dado un control de un formulario devuelve el nombre, el tipo y su valor
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="nombre"></param>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        private void ObtenerControlValor(Control ctrl, ref string nombre, ref string tipo, ref string valor)
        {
            try
            {
                nombre = ctrl.Name;
                valor = "";

                tipo = ctrl.GetType().Name;

                switch (ctrl.GetType().Name)
                {
                    case "RadTextBoxControl":
                        valor = ((RadTextBoxControl)ctrl).Text;
                        break;
                    case "RadCheckBox":
                        valor = ((RadCheckBox)ctrl).Checked ? "1" : "0";
                        break;
                    case "RadRadioButton":
                        valor = ((RadRadioButton)ctrl).IsChecked ? "1" : "0";
                        break;
                    case "RadMaskedEditBox":
                        string valorCampoMascara = "";
                        try
                        {
                            ((RadMaskedEditBox)ctrl).TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                            valorCampoMascara = ((RadMaskedEditBox)ctrl).Value.ToString().Trim();
                            ((RadMaskedEditBox)ctrl).TextMaskFormat = MaskFormat.IncludeLiterals;
                            if (valorCampoMascara != "") valorCampoMascara = ((RadMaskedEditBox)ctrl).Text;
                        }
                        catch { }
                        valor = valorCampoMascara;
                        break;
                    case "RadListControl":
                        RadListControl lista = ((RadListControl)ctrl);
                        for (int i = 0; i < lista.Items.Count; i++)
                        {
                            if (i != 0) valor += "|";
                            valor += lista.Items[i].ToString();
                        }
                        break;
                    case "RadButtonTextBox":
                        valor = ((RadButtonTextBox)ctrl).Text;
                        break;
                    case "RadDropDownList":
                        valor = ((RadDropDownList)ctrl).SelectedValue.ToString();
                        break;
                    case "RadDateTimePicker":
                        if (((RadDateTimePicker)ctrl).Value != null && ((RadDateTimePicker)ctrl).Text != "") valor = ((RadDateTimePicker)ctrl).Value.ToShortDateString();
                        else valor = "";
                        break;
                    default:
                        nombre = "";
                        tipo = "";
                        valor = "";
                        break;
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Obtiene todos los controles de un formulario y de ellos busca su nombre, tipo y valor
        /// </summary>
        private void ObtenerFormControlesValores(System.Windows.Forms.Control.ControlCollection collection)
        {
            string nombre = "";
            string valor = "";
            string tipo = "";

            foreach (Control ctrl in collection)
            {
                if (ctrl.GetType().FullName != "Telerik.WinControls.UI.RadPanel" &&
                    ctrl.GetType().FullName != "Telerik.WinControls.UI.RadGroupBox")
                {
                    this.ObtenerControlValor(ctrl, ref nombre, ref tipo, ref valor);
                    this.TablaControlesAdd(nombre, tipo, valor);

                    //Console.WriteLine(ctrl);
                }

                //do smth with ctrl
                ObtenerFormControlesValores(ctrl.Controls);
            }
        }

        /*
        /// <summary>
        /// Obtiene todos los controles de un formulario y de ellos busca su nombre, tipo y valor
        /// </summary>
        private void ObtenerFormControlesValores()
        {
            try
            {
                string nombre = "";
                string valor = "";
                string tipo = "";

                foreach (Control ctrl in this.formulario.Controls)
                {
                    if (ctrl.HasChildren && ctrl.GetType().Name != "TGTexBoxSel")
                    {
                        foreach (Control controlChild in ctrl.Controls)
                        {
                            this.ObtenerControlValor(controlChild, ref nombre, ref tipo, ref valor);
                            this.TablaControlesAdd(nombre, tipo, valor);
                        }
                    }
                    else
                    {
                        this.ObtenerControlValor(ctrl, ref nombre, ref tipo, ref valor);
                        this.TablaControlesAdd(nombre, tipo, valor);
                    }
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        */
        /// <summary>
        /// Adiciona una entrada en la tabla de controles del dataset
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        private void TablaControlesAdd(string nombre, string tipo, string valor)
        {
            try
            {
                if (nombre != "" || tipo != "" || valor != "")
                {
                    DataRow row = this.dsFormulario.Tables["Control"].NewRow();
                    row["Nombre"] = nombre;
                    row["Tipo"] = tipo;
                    row["Valor"] = valor;
                    this.dsFormulario.Tables["Control"].Rows.Add(row);
                }
            }
            catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        #endregion
    }
}
