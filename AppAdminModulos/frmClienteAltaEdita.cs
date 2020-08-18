using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ObjectModel;

namespace AppAdminModulos
{
    public partial class frmClienteAltaEdita : frmPlantilla
    {
        private bool nuevoCliente;
        
        private DataSet dsClientes;
        private DataTable tablaCliente;

        public bool NuevoCliente
        {
            get
            {
                return (this.nuevoCliente);
            }
            set
            {
                this.nuevoCliente = value;
            }
        }

        public frmClienteAltaEdita()
        {
            InitializeComponent();
        }

        #region Eventos
        private void frmClienteAltaEdita_Load(object sender, EventArgs e)
        {
            switch (this.nuevoCliente)
            {
                case false:
                    this.Text = "Cliente - Edición";
                    this.lblClientes.Visible = true;
                    this.cmbClientes.Visible = true;
                    this.txtIdDisco.ReadOnly = false;
                    this.btnAdicionar.Enabled = true;
                    this.listBoxIdDisco.Enabled = true;
                    this.btnEliminar.Enabled = true;
                    this.FillClientes();
                    break;
                default:
                    this.Text = "Cliente - Alta";
                    this.lblClientes.Visible = false;
                    this.cmbClientes.Visible = false;
                    this.rbActivo.Checked = true;
                    this.rbActivoNo.Checked = false;
                    this.txtIdDisco.ReadOnly = true;
                    this.btnAdicionar.Enabled = false;
                    this.listBoxIdDisco.Enabled = false;
                    this.btnEliminar.Enabled = false;

                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validar el formulario
            //if (!this.isFormValid()) return;

            if (this.nuevoCliente)
            {
                this.GrabarNuevoCliente();
            }
            else
            {
                this.ActualizarCliente();
            }

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (this.txtIdDisco.Text != "")
            {
                //Si no existe en la lista insertarlo
                this.listBoxIdDisco.Items.Add(this.txtIdDisco.Text);
                this.txtIdDisco.Text = "";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (this.listBoxIdDisco.SelectedItem != null)
            {
                int i = 0;
                i = this.listBoxIdDisco.SelectedIndex;
                this.listBoxIdDisco.Items.RemoveAt(i);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion


        #region Métodos Privados
        /// <summary>
        /// Llena el desplegable de clientes
        /// </summary>
        private void FillClientes()
        {
            //Crear el DataSet de Clientes
            this.tablaCliente = new DataTable("Cliente");
            this.tablaCliente.Columns.Add("id", typeof(string));
            this.tablaCliente.Columns.Add("codigo", typeof(String));
            this.tablaCliente.Columns.Add("nombre", typeof(String));
            this.tablaCliente.Columns.Add("activo", typeof(bool));

            this.dsClientes = new DataSet();
            this.dsClientes.DataSetName = "Cliente_Tabla";
            this.dsClientes.Tables.Add(this.tablaCliente);

            this.cmbClientes.Items.Clear();

            string query = "select * from Cliente order by id";

            try
            {
                proveedorDatos.FillDataTable(query, proveedorDatos.GetConnectionValue, "Cliente", ref this.dsClientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando los clientes (" + ex.Message + ")", "Error");
                return;
            }

            if (this.dsClientes != null && this.dsClientes.Tables != null && this.dsClientes.Tables.Count > 0 && this.dsClientes.Tables[0].Rows.Count > 0)
            {
                ArrayList clientes = new ArrayList();

                int cont = 0;
                DataTable clientesTabla = this.dsClientes.Tables["Cliente"];
                try
                {
                    for (int i = 0; i < clientesTabla.Rows.Count; i++)
                    {
                        clientes.Add(new AddValue(clientesTabla.Rows[i]["codigo"].ToString() + "  -  " + clientesTabla.Rows[i]["nombre"].ToString(), clientesTabla.Rows[i]["id"].ToString()));

                        if (cont == 0)
                        {
                            this.txtCodigo.Text = clientesTabla.Rows[i]["codigo"].ToString();
                            this.txtNombre.Text = clientesTabla.Rows[i]["nombre"].ToString();
                            if ((bool)clientesTabla.Rows[i]["activo"])
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
                this.cmbClientes.DataSource = clientes;
                this.cmbClientes.DisplayMember = "Display";
                this.cmbClientes.ValueMember = "Value";

                this.cmbClientes.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No existen clientes definidos. Debe ir a la opción de dar de Alta a los clientes.", "Error");
                this.dsClientes.Clear();
                this.dsClientes.Dispose();
                this.dsClientes= null;
                this.Close();
            }
        }

        /// <summary>
        /// Da de alta a un nuevo módulo en el fichero de configuración
        /// </summary>
        private void GrabarNuevoCliente()
        {
            try
            {
                int idCliente = 0;
                //Buscar el identificador del último cliente
                string query = "select MAX(id) as id from Cliente";

                int idUltimoCliente = proveedorDatos.ExecuteNonQuery(query, proveedorDatos.GetConnectionValue);

                idCliente++;

                //insertarlo en la bbdd 
                query = "insert into Cliente(id, codigo, nombre,activo) values(";
                query += idCliente.ToString() + ",";
                query += "'" + this.txtCodigo.Text + "',";
                query += "'" + this.txtNombre.Text + "',";
                if (this.rbActivo.Checked) query += "1";
                else query += "0";
                query += ")";

                int cont = proveedorDatos.ExecuteNonQuery(query, proveedorDatos.GetConnectionValue);

                MessageBox.Show("El cliente ha sido dado de alta.", "");

                //this.Close();
                this.nuevoCliente = false;
                this.Text = "Cliente - Edición";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error dando de alta al cliente (" + ex.Message + ")", "Error");
            }
                
        }

        /// <summary>
        /// Actualizar los datos del clienteseleccionado
        /// </summary>
        private void ActualizarCliente()
        {
            /*
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
            */ 
        }

        #endregion
    }
}
