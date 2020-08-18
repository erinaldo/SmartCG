using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace AppAdminModulos
{
    public partial class frmModuloAltaEdita : frmPlantilla
    {
        private bool nuevoModulo;

        public bool NuevoModulo
        {
            get
            {
                return (this.nuevoModulo);
            }
            set
            {
                this.nuevoModulo = value;
            }
        }


        public frmModuloAltaEdita()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmModuloAltaEdita_Load(object sender, EventArgs e)
        {
            switch (this.nuevoModulo)
            {
                case false:
                    this.Text = "Módulo - Edición";
                    this.lblModulos.Visible = true;
                    this.cmbModulos.Visible = true;
                    this.FillModulos();
                    break;
                default:
                    this.Text = "Módulo - Alta";
                    this.lblModulos.Visible = false;
                    this.cmbModulos.Visible = false;
                    this.rbBasico.Checked = false;
                    this.rbBasicoNo.Checked = true;
                    this.rbActivo.Checked = true;
                    this.rbActivoNo.Checked = false;
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Verificar el formulario

            if (!this.isFormValid()) return;

            if (this.nuevoModulo)
            {
                this.GrabarNuevoModulo();
            }
            else
            {
                this.ActualizarModulo();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbModulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable modulosTabla = dsModulosApp.Tables["Modulo"];
            for (int i = 0; i < modulosTabla.Rows.Count; i++)
            {
                if (this.cmbModulos.SelectedValue.ToString() == modulosTabla.Rows[i]["id"].ToString())
                {
                    this.txtNombreModulo.Text = modulosTabla.Rows[i]["nombre"].ToString();
                    this.txtNombreDll.Text = modulosTabla.Rows[i]["nombreDll"].ToString();
                    this.txtFormInicio.Text = modulosTabla.Rows[i]["formulario"].ToString();
                    this.txtImagen.Text = modulosTabla.Rows[i]["imagen"].ToString();
                    if ((bool)modulosTabla.Rows[i]["basico"])
                    {
                        this.rbBasico.Checked = true;
                        this.rbBasicoNo.Checked = false;
                    }
                    else
                    {
                        this.rbBasico.Checked = false;
                        this.rbBasicoNo.Checked = true;
                    }
                    if ((bool)modulosTabla.Rows[i]["activo"])
                    {
                        this.rbActivo.Checked = true;
                        this.rbActivoNo.Checked = false;
                    }
                    else
                    {
                        this.rbActivo.Checked = false;
                        this.rbActivoNo.Checked = true;
                    }
                }
            }            
        }
        #endregion

        #region Private Methods
        private bool isFormValid()
        {
            if (this.txtNombreModulo.Text == "")
            {
                MessageBox.Show("Debe indicar el nombre del módulo", "Error");
                this.txtNombreModulo.Focus();
                return (false);
            }

            if (this.txtNombreDll.Text == "")
            {
                MessageBox.Show("Debe indicar el nombre de la dll", "Error");
                this.txtNombreDll.Focus();
                return (false);
            }

            if (this.txtFormInicio.Text == "")
            {
                MessageBox.Show("Debe indicar el nombre del formulario de inicio", "Error");
                this.txtFormInicio.Focus();
                return (false);
            }

            if (this.txtImagen.Text == "")
            {
                MessageBox.Show("Debe indicar el nombre del fichero imagen del módulo", "Error");
                this.txtImagen.Focus();
                return (false);
            }

            //Verificar que los datos (nombre y fichero dll) no se repitan (tienen que ser datos únicos por módulos)
            bool NOduplicado = true;

            if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0)
            {
                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];
                for (int i = 0; i < modulosTabla.Rows.Count; i++)
                {
                    if (this.txtNombreModulo.Text.Trim().ToUpper() == modulosTabla.Rows[i]["nombre"].ToString().ToUpper())
                    {
                        //Modo edición (no nueva alta)
                        if (!nuevoModulo)
                        {
                            if (this.cmbModulos.SelectedValue.ToString() != modulosTabla.Rows[i]["id"].ToString())
                            {
                                NOduplicado = false;
                            }
                        }
                        else
                        {
                            NOduplicado = false;
                        }

                        if (!NOduplicado)
                        {
                            MessageBox.Show("El nombre del módulo tiene que ser único y ya existe un módulo registrado con este nombre. Por favor, introduzca otro nombre.", "Error");
                            this.txtNombreModulo.Focus();
                            break;
                        }
                    }

                    if (this.txtNombreDll.Text.Trim().ToUpper() == modulosTabla.Rows[i]["nombreDll"].ToString().ToUpper())
                    {
                        //Modo edición (no nueva alta)
                        if (!nuevoModulo)
                        {
                            if (this.cmbModulos.SelectedValue.ToString() != modulosTabla.Rows[i]["id"].ToString())
                            {
                                NOduplicado = false;
                            }
                        }
                        else
                        {
                            NOduplicado = false;
                        }

                        if (!NOduplicado)
                        {
                            MessageBox.Show("El nombre de la dll tiene que ser único y ya existe un módulo registrado con esta dll. Por favor, introduzca otra dll.", "Error");
                            this.txtNombreDll.Focus();
                            break;
                        }
                    }

                }
            }

            return (NOduplicado);
        }


        /// <summary>
        /// Crea una pareja nombre, valor
        /// que después se puede adicionar a un ArrayList
        /// Es útil para llenar ComboBox
        /// </summary>
        protected class AddValue
        {
            private string m_Display;
            private string m_Value;
            public AddValue(string Display, string Value)
            {
                m_Display = Display;
                m_Value = Value;
            }
            public string Display
            {
                get { return m_Display; }
            }
            public string Value
            {
                get { return m_Value; }
            }
        }


        /// <summary>
        /// Llena el desplegable de módulos
        /// </summary>
        private void FillModulos()
        {
            this.cmbModulos.Items.Clear();

            //Cargar los módulos si aún no se ha hecho
            string result = this.CargarModulos();
            if (result != "")
            {
                MessageBox.Show("Error cargando los módulos (" + result + ")", "Error");
                return;
            }

            if (dsModulosApp != null && dsModulosApp.Tables != null && dsModulosApp.Tables.Count > 0 && dsModulosApp.Tables[0].Rows.Count > 0)
            {
                ArrayList modulos = new ArrayList();

                int cont = 0;
                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];
                try
                {
                    for (int i = 0; i < modulosTabla.Rows.Count; i++)
                    {
                        modulos.Add(new AddValue(modulosTabla.Rows[i]["id"].ToString() + "  -  " + modulosTabla.Rows[i]["nombre"].ToString(), modulosTabla.Rows[i]["id"].ToString()));

                        if (cont == 0)
                        {
                            this.txtNombreModulo.Text = modulosTabla.Rows[i]["nombre"].ToString();
                            this.txtNombreDll.Text = modulosTabla.Rows[i]["nombreDll"].ToString();
                            this.txtFormInicio.Text = modulosTabla.Rows[i]["formulario"].ToString();
                            this.txtImagen.Text = modulosTabla.Rows[i]["imagen"].ToString();
                            if ((bool)modulosTabla.Rows[i]["basico"])
                            {
                                this.rbBasico.Checked = true;
                                this.rbBasicoNo.Checked = false;
                            }
                            else
                            {
                                this.rbBasico.Checked = false;
                                this.rbBasicoNo.Checked = true;
                            }
                            if ((bool)modulosTabla.Rows[i]["activo"])
                            {
                                this.rbActivo.Checked = true;
                                this.rbActivoNo.Checked = false;
                            }
                            else
                            {
                                this.rbActivo.Checked = false;
                                this.rbActivoNo.Checked = true;
                            }
                            cont++;
                        }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                this.cmbModulos.DataSource = modulos;
                this.cmbModulos.DisplayMember = "Display";
                this.cmbModulos.ValueMember = "Value";

                this.cmbModulos.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No existen módulos definidos. Debe ir a la opción de dar de Alta a los módulos.", "Error");
                dsModulosApp.Clear();
                dsModulosApp.Dispose();
                dsModulosApp = null;
                this.Close();
            }
        }

        /// <summary>
        /// Da de alta a un nuevo módulo en el fichero de configuración
        /// </summary>
        private void GrabarNuevoModulo()
        {
            try
            {
                int idModulo = 0;
                //Buscar el identificador del último módulo
                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];

                if (modulosTabla.Rows.Count > 0)
                {
                    idModulo = Convert.ToInt32(modulosTabla.Rows[modulosTabla.Rows.Count - 1]["id"]);
                }

                idModulo++;

                //insertarlo en la bbdd 
                string query = "insert into Modulos(id, nombre, nombreDll, formulario,imagen,basico,activo) values(";
                query += idModulo.ToString() + ",";
                query += "'" + this.txtNombreModulo.Text + "',";
                query += "'" + this.txtNombreDll.Text + "',";
                query += "'" + this.txtFormInicio.Text + "',";
                query += "'" + this.txtImagen.Text + "',";
                if (this.rbBasico.Checked) query += "1,";
                else query += "0,";
                if (this.rbActivo.Checked) query += "1";
                else query += "0";
                query += ")";

                int cont = proveedorDatos.ExecuteNonQuery(query, proveedorDatos.GetConnectionValue);

                //insertarlo en la tabla Modulo del DataSet de Modulos
                DataRow nuevoModulo = dsModulosApp.Tables["Modulo"].NewRow();
                nuevoModulo["id"] = idModulo.ToString();
                nuevoModulo["nombre"] = this.txtNombreModulo.Text;
                nuevoModulo["nombreDll"] = this.txtNombreDll.Text;
                nuevoModulo["formulario"] = this.txtFormInicio.Text;
                nuevoModulo["imagen"] = this.txtImagen.Text;
                if (this.rbBasico.Checked) nuevoModulo["basico"] = true;
                else nuevoModulo["basico"] = false;
                if (this.rbActivo.Checked) nuevoModulo["activo"] = true;
                else nuevoModulo["activo"] = false;

                dsModulosApp.Tables["Modulo"].Rows.Add(nuevoModulo);

                //dsModulosApp.WriteXml(filePath, XmlWriteMode.IgnoreSchema);

                MessageBox.Show("El módulo ha sido dado de alta.", "");

                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error dando de alta al módulo (" + ex.Message + ")", "Error");
            }
        }

        /// <summary>
        /// Actualizar los datos del módulo seleccionado
        /// </summary>
        private void ActualizarModulo()
        {
            try
            {
                DataTable modulosTabla = dsModulosApp.Tables["Modulo"];
                for (int i = 0; i < modulosTabla.Rows.Count; i++)
                {
                    if (this.cmbModulos.SelectedValue.ToString() == modulosTabla.Rows[i]["id"].ToString())
                    {
                        //Actualizarlo en la tabla módulos de la bbdd
                        string query = "update Modulos set ";
                        query += "nombre = '" + this.txtNombreModulo.Text + "',";
                        query += "nombreDll = '" + this.txtNombreDll.Text + "',";
                        query += "formulario = '" + this.txtFormInicio.Text + "',";
                        query += "imagen = '" + this.txtImagen.Text + "',";
                        if (this.rbBasico.Checked) query += "basico = 1,";
                        else query += "basico = 0,";
                        if (this.rbActivo.Checked) query += "activo = 1";
                        else query += "activo = 0";
                        query += " where id = " + this.cmbModulos.SelectedValue.ToString();

                        int cont = proveedorDatos.ExecuteNonQuery(query, proveedorDatos.GetConnectionValue);

                        //Actualizarlo en el DataSet
                        modulosTabla.Rows[i]["nombre"] = this.txtNombreModulo.Text;
                        modulosTabla.Rows[i]["nombreDll"] = this.txtNombreDll.Text;
                        modulosTabla.Rows[i]["formulario"] = this.txtFormInicio.Text;
                        modulosTabla.Rows[i]["imagen"] = this.txtImagen.Text;
                        if (this.rbBasico.Checked)
                        {
                            modulosTabla.Rows[i]["basico"] = true;
                        }
                        else
                        {
                            modulosTabla.Rows[i]["basico"] = false;
                        }
                        if (this.rbActivo.Checked)
                        {
                            modulosTabla.Rows[i]["activo"] = true;
                        }
                        else
                        {
                            modulosTabla.Rows[i]["activo"] = false;
                        }
                        break;
                    }
                }            

                //dsModulosApp.WriteXml(filePath, XmlWriteMode.IgnoreSchema);

                MessageBox.Show("El módulo ha sido actualizado correctamente.", "");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error actualizando el módulo (" + ex.Message + ")", "Error");
            }
        }
        #endregion
    }
}
