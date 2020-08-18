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
using System.Globalization;
using ObjectModel;

namespace SmartCG
{
    public partial class frmEntornoCargar : frmPlantilla
    {
        private Entorno entorno;

        public frmEntornoCargar()
        {
            InitializeComponent();
        }

        private void FrmEntornoCargar_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Entorno");

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();

            //Leer todos los entornos de la carpeta entornos
            this.FillcmbEntorno();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                int indice = Convert.ToInt16(this.cmbEntorno.SelectedValue.ToString());
                string result = this.entorno.InstanciarEntorno(this.entorno, indice);

                if (result != "")
                {
                    //Error
                    return;
                }

                result = this.entorno.CargarEntorno();

                if (result == "")
                {

                    this.Hide();
                    this.Close();

                    //Refrescar los paneles para que parezcan como no activos y llamar al formulario de login del servidor
                    if (Application.OpenForms["frmPrincipal"] != null)
                    {
                        /*
                        //Application.OpenForms["ModMantenimientos.frmPrincipal"].Close();
                        for (int i = 0; i < Application.OpenForms.Count; i++)
                        {
                            if (Application.OpenForms[i].Owner == this.Owner)
                            {
                                Type aplicacion = Application.OpenForms[i].GetType();
                                //string aplicacion = Application.OpenForms[i].Name;
                            }
                        }
                        */


                        //Cerrar los formmularios abiertos desde la Lanzadera
                        string nombre = "";
                        int i = 0;
                        bool seguir = true;
                        while (seguir)
                        {
                            if (i < Application.OpenForms.Count)
                            {
                                Form f = Application.OpenForms[i];

                                nombre = ((Form)f).GetType().FullName;

                                //if (nombre != "SmartCG.frmPrincipal" && nombre != "SmartCG.frmEntornoLista")
                                if (nombre != "SmartCG.frmPrincipal")
                                {
                                    ((Form)f).Close();

                                    if (Application.OpenForms.Count > 2) i = 0;
                                    else i++;
                                }
                                else i++;

                                /*
                                if (f.Owner == this.Owner)
                                {
                                    nombre = ((Form)f).GetType().FullName;

                                    ((Form)f).Close();

                                    i = 0;
                                }
                                else i++;*/
                            }
                            else seguir = false;
                        }

                        if (this.Owner is IFormEntorno formInterface)
                            formInterface.ModulosDeshabilitar();
                    }
                }

            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        private void FrmEntornoCargar_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Entorno");
        }

        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Falta traducir
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmEntornoCargar", "Cargar entorno");
            this.lblEntorno.Text = this.LP.GetText("lblEntorno", "Entorno");
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }

        /// <summary>
        /// Cargar el desplegable de entornos
        /// </summary>
        private void FillcmbEntorno()
        {
            string result = "";

            try
            {
                string varPathFicherosEntornos = ConfigurationManager.AppSettings["pathFicherosEntornos"];
                string pathFicheros = Application.StartupPath + "\\" + varPathFicherosEntornos;

                this.entorno = new Entorno();
                result = this.entorno.LeerTodosEntornos();

                if (result == "")
                {
                    this.cmbEntorno.Items.Clear();
                    ArrayList entornosArray = new ArrayList();

                    if (this.entorno.DTEntorno != null && this.entorno.DTEntorno.Rows.Count > 0)
                    {
                        int indiceEntornoActivo = 0;
                        for (int i = 0; i < this.entorno.DTEntorno.Rows.Count; i++)
                        {
                            entornosArray.Add(new AddValue(this.entorno.DTEntorno.Rows[i]["nombre"].ToString(), i.ToString()));

                            try
                            {
                                if (Convert.ToBoolean(this.entorno.DTEntorno.Rows[i]["activo"])) indiceEntornoActivo = i;
                            }
                            catch(Exception ex) 
                            {
                                Log.Error(Utiles.CreateExceptionString(ex));
                            }
                        }

                        this.cmbEntorno.DataSource = entornosArray;
                        this.cmbEntorno.DisplayMember = "Display";
                        this.cmbEntorno.ValueMember = "Value";
                        this.cmbEntorno.SelectedIndex = indiceEntornoActivo;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
    }
}
