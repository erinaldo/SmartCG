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
using ObjectModel;
using Telerik.WinControls;

namespace SmartCG
{
    public partial class frmValidaClave : frmPlantilla, IReLocalizable
    {
        private string _titulo;
        private string tipoBaseDatosCG;

        string strCVISEC = "";
        string strCVIKEY = "";

        public Boolean claveOK;

        private bool gestionarClaveOpcion = false;

        public string Titulo
        {
            get
            {
                return (this._titulo);
            }
            set
            {
                this._titulo = value;
            }
        }

        public bool GestionarClaveOpcion
        {
            get
            {
                return (this.gestionarClaveOpcion);
            }
            set
            {
                this.gestionarClaveOpcion = value;
            }
        }

        public frmValidaClave()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
            resources.ApplyResources(this, "$this");
            foreach (Control c in this.Controls)
                resources.ApplyResources(c, c.Name);
        }
        
        private void FrmValidaClave_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Validar Clave");

            try
            {

                //Verificar de que exista una conexión a la base de datos
                if (GlobalVar.ConexionCG == null)
                {
                    RadMessageBox.Show("No existe entorno activo para conectar con la base de datos de contabilidad.");
                    claveOK = false;
                    this.Close();
                    return;
                }

                this.tipoBaseDatosCG = GlobalVar.ConexionCG.TipoBaseDatos.ToString();

                //Verificar que exista la tabla de claves
                UtilesCGConsultas utilesCG = new UtilesCGConsultas();
                if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, "CSCVI"))
                {
                    RadMessageBox.Show("No existe la tabla de claves.");
                    claveOK = false;
                    this.Close();
                    return;
                }

                ClaveAplicacion.fechaDesencriptada = "";

                this.Enabled = false;

                IDataReader dr = null;
                string query = "select CVIKEY, CVISEC from " + GlobalVar.PrefijoTablaCG + "CSCVI ";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    strCVIKEY = dr["CVIKEY"].ToString();
                    strCVISEC = dr["CVISEC"].ToString();
                }

                //si validacion no OK mostrar formulario
                ClaveAplicacion.ValidarClave(strCVIKEY, strCVISEC);
                if (ClaveAplicacion.claveCorrecta)
                {
                    //si validacion OK salir
                    Log.Info("Clave válida:" + ClaveAplicacion.fechaDesencriptada);

                    claveOK = true;

                    if (!this.gestionarClaveOpcion)
                    { 
                        this.Close();
                        return;
                    }
                    else
                    {
                        //Mostrar la fecha de caducidad
                        this.Enabled = true;
                        this.radLabelFechaCaducidadValor.Text = this.FechaCaducidadFormatShow(ClaveAplicacion.fechaDesencriptada);
                        this.radLabelFechaCaducidadValor.ForeColor = Color.Black;
                        this.radLlblCaducidad.Visible = true;
                    }
                }
                else
                {
                    if (ClaveAplicacion.fechaDesencriptada == "")
                    {
                        Log.Info("Clave no válida: no hay clave.");
                        this.radLabelFechaCaducidadValor.Text = "No hay clave definida";
                        this.radLabelFechaCaducidadValor.ForeColor = Color.Red;
                        this.radLlblCaducidad.Visible = false;
                    }
                    else
                    {
                        Log.Info("Clave no válida:" + ClaveAplicacion.fechaDesencriptada);
                        this.radLabelFechaCaducidadValor.Text = "Clave no válida: " + ClaveAplicacion.fechaDesencriptada;
                        this.radLabelFechaCaducidadValor.ForeColor = Color.Red;
                        this.radLlblCaducidad.Visible = false;
                    }
                    claveOK = false;
                    this.Enabled = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";

                if (radTextBoxNuevaClave.Text.Trim().Length != 24)
                {
                    MessageBox.Show(this.LP.GetText("errClave", "Formato de clave no correcto"));
                    this.radTextBoxNuevaClave.Focus();
                    return;
                }

                ClaveAplicacion.Encriptar(radTextBoxNuevaClave.Text.Trim());
                strCVISEC = ClaveAplicacion.claveEncriptada;

                if (strCVIKEY.Trim() == "") //si no hay registro en CSCVI se inserta uno asignando código de cliente
                {
                    ClaveAplicacion.Desencriptar(strCVISEC);
                    strCVIKEY = ClaveAplicacion.codigoClienteDesencriptado;

                    string nombreTabla = GlobalVar.PrefijoTablaCG + "CSCVI";
                    query = "insert into " + nombreTabla + " (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ", ";
                    query += "CVIKEY, CVIPRC, CVIUSR, CVISEC, CVIDAT) values (";
                    if (this.tipoBaseDatosCG == "Oracle") query += "ID_" + nombreTabla + ".nextval, ";
                    query += strCVIKEY + ", 0, 0, '" + strCVISEC + "', ' ')";

                    int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                else //se updatea, solo habrá un registro en la tabla.
                {
                    query = "update " + GlobalVar.PrefijoTablaCG + "CSCVI set ";
                    query += "CVISEC = '" + strCVISEC + "' ";

                    GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

                //si validacion no OK mostrar formulario
                ClaveAplicacion.ValidarClave(strCVIKEY, strCVISEC);
                if (ClaveAplicacion.claveCorrecta)
                {
                    //si validacion OK salir
                    this.radLabelFechaCaducidadValor.Text = this.FechaCaducidadFormatShow(ClaveAplicacion.fechaDesencriptada);
                    this.radLabelFechaCaducidadValor.ForeColor = Color.Black;
                    this.radLlblCaducidad.Visible = true;
                    Log.Info("Clave válida:" + ClaveAplicacion.fechaDesencriptada);
                    MessageBox.Show(this.LP.GetText("errClaveOK", "Clave actualizada correctamente."));
                    claveOK = true;
                    this.Close();
                    return;
                }
                else
                {
                    if (ClaveAplicacion.fechaDesencriptada == "")
                    {
                        this.radLabelFechaCaducidadValor.Text = "No hay clave definida";
                        this.radLabelFechaCaducidadValor.ForeColor = Color.Red;
                        this.radLlblCaducidad.Visible = false;
                        Log.Info("Clave no válida: no hay clave.");
                        MessageBox.Show(this.LP.GetText("errClaveKO", "Clave no actualizada correctamente."));
                    }
                    else
                    {
                        this.radLabelFechaCaducidadValor.Text = "Clave no actualizada correctamente: " + ClaveAplicacion.fechaDesencriptada;
                        this.radLabelFechaCaducidadValor.ForeColor = Color.Red;
                        this.radLlblCaducidad.Visible = false;
                        Log.Info("Clave no válida:" + ClaveAplicacion.fechaDesencriptada);
                        MessageBox.Show(this.LP.GetText("errClaveKO", "Clave no actualizada correctamente."));
                    }
                }

                //this.Close();
                //return;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonActualizar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonActualizar);
        }

        private void RadButtonActualizar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonActualizar);
        }

        private void RadButtonCancelar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCancelar);
        }

        private void RadButtonCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCancelar);
        }
        
        private void RadButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmValidaClave_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Validar Clave");

            if (this.gestionarClaveOpcion && claveOK) return;
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Devuelve la fecha de caducidad de la clave en formato dd/mm/aaaa
        /// </summary>
        /// <param name="fechaClave">Fecha caducidad de la clave en formato aaaammdd </param>
        /// <returns></returns>
        private string FechaCaducidadFormatShow(string fechaClave)
        {
            string fecha = fechaClave;
            try
            {
                if (fecha.Length == 8) fecha = fecha.Substring(6, 2) + "/" + fecha.Substring(4, 2) + "/" + fecha.Substring(0, 4);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (fecha);
        }
        #endregion
    }
}
