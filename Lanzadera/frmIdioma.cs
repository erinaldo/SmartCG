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

namespace FinanzasNet
{
    public partial class frmIdioma: frmPlantilla
    {

        public frmIdioma()
        {
            InitializeComponent();
        }

        private void frmIdioma_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cambiar Idioma");

            //Poner los literales en el idioma que corresponda
            this.TraducirLiterales();

            //Leer todos los idiomas del fichero de configuración

            this.cmbIdioma.Items.Clear();
            IdiomaSection idiomaSection = (IdiomaSection)ConfigurationManager.GetSection("idiomaSection");

            ArrayList idiomasArray = new ArrayList();

            string cultura = "";
            string descripcion = "";
            string idiomaActual = ConfigurationManager.AppSettings["idioma"];
            int contador = 0;
            bool posicionIdiomaActualEncontrada = false;
            foreach(IdiomaElement idioma in idiomaSection.Idiomas)
            {
                if (idioma.Activo == 1)
                {
                    cultura = idioma.Cultura;
                    if (idiomaActual != cultura && !posicionIdiomaActualEncontrada) contador++;
                    if (!posicionIdiomaActualEncontrada)
                    {
                        if (idiomaActual == cultura) posicionIdiomaActualEncontrada = true;
                    }
                    descripcion = this.LP.GetText("lblIdioma" + cultura, idioma.Descripcion);
                    idiomasArray.Add(new AddValue(descripcion, cultura));
                }
            }

            if (idiomasArray.Count == 0) idiomasArray.Add(new AddValue("Español", "es-ES"));

            this.cmbIdioma.DataSource = idiomasArray;
            this.cmbIdioma.DisplayMember = "Display";
            this.cmbIdioma.ValueMember = "Value";

            if (cmbIdioma.Items.Count == contador) this.cmbIdioma.SelectedIndex = contador - 1;
            else this.cmbIdioma.SelectedIndex = contador;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string idiomaActual = ConfigurationManager.AppSettings["idioma"];
            if (idiomaActual != this.cmbIdioma.SelectedValue.ToString())
            {
                string idiomaSeleccionado = this.cmbIdioma.SelectedValue.ToString();

                //Actualizar la variable de configuración de idioma con el nuevo valor
                utiles.ModificarappSettings("idioma",idiomaSeleccionado);

                //Actualizar el idioma del usuario que está conectado
                GlobalVar.UsuarioEnv.IdiomaApp = idiomaSeleccionado;

                //Actualizar la variable global de idioma
                GlobalVar.LanguageProvider = idiomaSeleccionado;

                try
                {
                    //Recargar todos los formularios abiertos
                    CultureInfo nuevaCultura = new CultureInfo(idiomaSeleccionado);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = nuevaCultura;
                    foreach (Form f in Application.OpenForms)
                        if (f is IReLocalizable)
                            ((IReLocalizable)f).ReLocalize();
                }
                catch(Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                }
            }

            this.Close();
        }

        private void frmIdioma_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cambiar Idioma");
        }

        /// <summary>
        /// Busca los literales en el idioma que corresponda
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmIdiomaTitulo", "Cambiar idioma");
            this.lblIdioma.Text = this.LP.GetText("lblIdioma", "Idioma:");
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");
        }

    }
}
