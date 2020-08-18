using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;

namespace ModComprobantes
{
    public partial class frmCompContAltaEditaComentarios : frmPlantilla, IReLocalizable
    {
        private string codigoCompania;
        private string nombreCompania;
        private string aapp;
        private string aappFormateado;
        private string codigoTipo;
        private string nombreTipo;
        private string numeroComprobante;
        private bool actualizaDescFormEdicionComprobante;
        private string descFormEdicionComprobante;
        
        private string[] arrComentarios = new string[18];

        public string CodigoCompania
        {
            get
            {
                return (this.codigoCompania);
            }
            set
            {
                this.codigoCompania = value;
            }
        }

        public string NombreCompania
        {
            get
            {
                return (this.nombreCompania);
            }
            set
            {
                this.nombreCompania = value;
            }
        }

        public string AAPP
        {
            get
            {
                return (this.aapp);
            }
            set
            {
                this.aapp = value;
            }
        }

        public string AAPPFormato
        {
            get
            {
                return (this.aappFormateado);
            }
            set
            {
                this.aappFormateado = value;
            }
        }

        public string CodigoTipo
        {
            get
            {
                return (this.codigoTipo);
            }
            set
            {
                this.codigoTipo = value;
            }
        }

        public string NombreTipo
        {
            get
            {
                return (this.nombreTipo);
            }
            set
            {
                this.nombreTipo = value;
            }
        }

        public string NumeroComprobante
        {
            get
            {
                return (this.numeroComprobante);
            }
            set
            {
                this.numeroComprobante = value;
            }
        }

        public bool ActualizaDescripcion
        {
            get
            {
                return (this.actualizaDescFormEdicionComprobante);
            }
            set
            {
                this.actualizaDescFormEdicionComprobante = value;
            }
        }

        public string DescripcionComprobante
        {
            get
            {
                return (this.descFormEdicionComprobante);
            }
            set
            {
                this.descFormEdicionComprobante = value;
            }
        }


        public frmCompContAltaEditaComentarios()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbComentarios.ElementTree.EnableApplicationThemeName = false;
            this.gbComentarios.ThemeName = "ControlDefault";
        }


        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales(true);
        }
        
        private void FrmCompContAltaEditaComentarios_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Ver/Edita Comentarios");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Propiedades que utilizarán por el formulario de Alta/Edición de comprobantes
            this.actualizaDescFormEdicionComprobante = false;
            this.descFormEdicionComprobante = "";

            //Cargar Valores Fijos
            this.CargarCabeceraComprobante();

            //Inicializar el Array
            this.InicializarArrayComentarios();

            //Cargar los comentarios del comprobante
            this.CargarComentariosComprobante();

            this.ActiveControl = this.txtDesc1;
            this.txtDesc1.Select(0, 0);
            this.txtDesc1.Focus();
        }

        private void RadButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonGuardar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //Grabar los comentarios del comprobante
            this.GrabarComentarios();

            //Actualizar los valores originales de los controles
            this.ActualizaValoresOrigenControles();

            //Actualiza el listado de elementos del formulario frmListaGenerica
            //this.ActualizarFormularioListaElementos();

            //Cerrar el formulario
            this.Close();

            Cursor.Current = Cursors.Default;
        }

        private void FrmCompContAltaEditaComentarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonSalir_Click(sender, null); 
        }

        private void FrmCompContAltaEditaComentarios_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.txtDesc1.Text.Trim() != this.txtDesc1.Tag.ToString().Trim() ||
                    this.txtDesc2.Text.Trim() != this.txtDesc2.Tag.ToString().Trim() ||
                    this.txtDesc3.Text.Trim() != this.txtDesc3.Tag.ToString().Trim() ||
                    this.txtDesc4.Text.Trim() != this.txtDesc4.Tag.ToString().Trim() ||
                    this.txtDesc5.Text.Trim() != this.txtDesc5.Tag.ToString().Trim() ||
                    this.txtDesc6.Text.Trim() != this.txtDesc6.Tag.ToString().Trim() ||
                    this.txtDesc7.Text.Trim() != this.txtDesc7.Tag.ToString().Trim() ||
                    this.txtDesc8.Text.Trim() != this.txtDesc8.Tag.ToString().Trim() ||
                    this.txtDesc9.Text.Trim() != this.txtDesc9.Tag.ToString().Trim()
                )
                {
                    DialogResult result = MessageBox.Show(this.LP.GetText("lblfrmCompContAEComGuardarCambios", "¿Desea guardar los cambios efectuados?"), this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        this.radButtonGuardar.PerformClick();
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }

            Log.Info("FIN Ver/Edita Comentarios");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        /// <param name="traducirComboClase">Si se traducen o no los literales del Combo de Clase</param>
        private void TraducirLiterales(bool traducirComboClase)
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContAltaEditaComentariosTitulo", "Editar Comentarios");

            this.radButtonGuardar.Text = this.LP.GetText("lblfrmCompContBotGrabar", "Guardar");
            this.radButtonSalir.Text = this.LP.GetText("toolStripSalir", "Salir");

            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
        }

        /// <summary>
        /// Carga los valores fijos (sólo lectura) de la cabecera del comprobante
        /// </summary>
        private void CargarCabeceraComprobante()
        {
            try
            {
                this.txtCompania.Text = this.nombreCompania;
                this.txtTipo.Text = this.nombreTipo;
                this.txtMaskAAPP.Text = this.aappFormateado;
                this.txtNoComprobante.Text = this.numeroComprobante;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Inicializar el array de comentarios
        /// </summary>
        private void InicializarArrayComentarios()
        {
            try
            {
                string aux = "";
                aux = aux.PadRight(36, ' ');
                for (int i = 0; i < this.arrComentarios.Length; i++)
                {
                    this.arrComentarios[i] = aux;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Cargar los comentarios del comprobante
        /// </summary>
        private void CargarComentariosComprobante()
        {
            IDataReader dr = null;

            try
            {
                string query = "select COHEAD from " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                query += "where CCIAAD='" + this.codigoCompania + "' and SAPRAD= " + this.aapp + " and TICOAD=" + this.codigoTipo + " and NUCOAD=" + this.numeroComprobante;
                
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                int indice = 0;
                while (dr.Read())
                {
                    if (indice <= 17)
                    {
                        this.arrComentarios[indice] = dr["COHEAD"].ToString();
                        indice++;
                    }
                }

                dr.Close();

                this.txtDesc1.Text = (this.arrComentarios[0] + this.arrComentarios[1]).TrimEnd();
                this.txtDesc2.Text = (this.arrComentarios[2] + this.arrComentarios[3]).TrimEnd();
                this.txtDesc3.Text = (this.arrComentarios[4] + this.arrComentarios[5]).TrimEnd();
                this.txtDesc4.Text = (this.arrComentarios[6] + this.arrComentarios[7]).TrimEnd();
                this.txtDesc5.Text = (this.arrComentarios[8] + this.arrComentarios[9]).TrimEnd();
                this.txtDesc6.Text = (this.arrComentarios[10] + this.arrComentarios[11]).TrimEnd();
                this.txtDesc7.Text = (this.arrComentarios[12] + this.arrComentarios[13]).TrimEnd();
                this.txtDesc8.Text = (this.arrComentarios[14] + this.arrComentarios[15]).TrimEnd();
                this.txtDesc9.Text = (this.arrComentarios[16] + this.arrComentarios[17]).TrimEnd();

                //Actualizar los valores originales de los controles
                this.ActualizaValoresOrigenControles();
            }
            catch (Exception ex) 
            {
                Log.Error(Utiles.CreateExceptionString(ex)); 

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Graba los comentarios del comprobante
        /// </summary>
        private void GrabarComentarios()
        {
            try
            {
                //Eliminar los comentarios anteriores
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                query += "where CCIAAD='" + this.codigoCompania + "' and SAPRAD= " + this.aapp + " and TICOAD=" + this.codigoTipo + " and NUCOAD=" + this.numeroComprobante;

                int registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                string queryInicial = "insert into " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                queryInicial += "(CCIAAD, SAPRAD, TICOAD, NUCOAD, COHEAD) values ('";
                queryInicial += this.codigoCompania + "', " + this.aapp + ", " + this.codigoTipo + ", " + this.numeroComprobante + ", '";

                string queryFinal = "')";

                string desc1 = this.txtDesc1.Text.PadRight(72, ' ');
                string desc2 = this.txtDesc2.Text.PadRight(72, ' ');
                string desc3 = this.txtDesc3.Text.PadRight(72, ' ');
                string desc4 = this.txtDesc4.Text.PadRight(72, ' ');
                string desc5 = this.txtDesc5.Text.PadRight(72, ' ');
                string desc6 = this.txtDesc6.Text.PadRight(72, ' ');
                string desc7 = this.txtDesc7.Text.PadRight(72, ' ');
                string desc8 = this.txtDesc8.Text.PadRight(72, ' ');
                string desc9 = this.txtDesc9.Text.PadRight(72, ' ');

                string descripcionTotal = desc1 + desc2 + desc3 + desc4 + desc5 + desc6 + desc7 + desc8 + desc9;

                if (descripcionTotal.Trim() == "")
                {
                    query = queryInicial + desc1.Substring(0, 35) + queryFinal;

                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                else
                {
                    query = queryInicial + desc1.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc1.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc2.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc2.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc3.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc3.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc4.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc4.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc5.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc5.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc6.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc6.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc7.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc7.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc8.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc8.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    query = queryInicial + desc9.Substring(0, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    query = queryInicial + desc9.Substring(36, 36) + queryFinal;
                    registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

                //Actualizar las propiedades que serán consultadas por el Formulario de Alta/Edición de comprobantes
                this.actualizaDescFormEdicionComprobante = true;
                this.descFormEdicionComprobante = desc1.Substring(0, 36);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza el atributo TAG de los controles al valor actual de los controles
        /// </summary>
        private void ActualizaValoresOrigenControles()
        {
            this.txtDesc1.Tag = this.txtDesc1.Text;
            this.txtDesc2.Tag = this.txtDesc2.Text;
            this.txtDesc3.Tag = this.txtDesc3.Text;
            this.txtDesc4.Tag = this.txtDesc4.Text;
            this.txtDesc5.Tag = this.txtDesc5.Text;
            this.txtDesc6.Tag = this.txtDesc6.Text;
            this.txtDesc7.Tag = this.txtDesc7.Text;
            this.txtDesc8.Tag = this.txtDesc8.Text;
            this.txtDesc9.Tag = this.txtDesc9.Text;
        }
        #endregion
    }
}