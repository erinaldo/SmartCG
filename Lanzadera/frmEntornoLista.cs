using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using ObjectModel;

namespace SmartCG
{
    public partial class frmEntornoLista : frmPlantilla, IReLocalizable, IFormEntorno
    {
        private Entorno entorno;

        public frmEntornoLista()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmEntorno_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de Entornos");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Crear el DataGrid y sus columnas
            this.BuildDataGrid();

            //Llenar el DataGrid
            this.FillDataGrid();

            //Chequear que existan comprobantes
            //this.VerificarExistenciaComprobantes();

            this.TraducirLiterales();

            //Ajustar todas las columnas de la Grid
            //this.AjustarColumnas();
        }

        private void FrmEntornoLista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) RadButtonCancelar_Click(sender, null);
        }
        
        //Actualizar el listado de comprobantes desde el formulario frmCompContAltaEdita.cs después de un alta o una actualización de comprobante
        void IFormEntorno.ActualizaListaElementos()
        {
            //Volver a cargar la lista de comprobantes
            //this.dataTable.Rows.Clear();
            //this.dgComprobantes.Rows.Clear();
            this.FillDataGrid();
        }

        void IFormEntorno.ModulosDeshabilitar()
        {
        }

        private void RadGridViewEntornos_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarEntorno();
        }

        private void RadButtonCargar_Click(object sender, EventArgs e)
        {
            if (this.EntornoValid())
            {
                //int indice = this.dgEntornos.CurrentRow.Index;
                int indice = this.radGridViewEntornos.CurrentRow.Index;
                this.entorno.InstanciarEntorno(this.entorno, indice);

                //Actualizar las variables de configuración del appConfig
                this.entorno.CargarEntorno();

                //Blanquear las variables de memoria
                try
                {
                    if (GlobalVar.ConexionCG != null)
                    {
                        GlobalVar.ConexionCG.GetConnectionValue.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                }

                GlobalVar.CadenaConexionCG = "";

                GlobalVar.UsuarioLogadoCG_BBDD = "";
                GlobalVar.UsuarioLogadoCG = "";

                GlobalVar.ConexionCG = null;
                GlobalVar.PrefijoTablaCG = this.entorno.PrefijoTabla;

                this.Hide();
                this.Close();

                //Refrescar los paneles para que parezcan como no activos y llamar al formulario de login del servidor
                //if (Application.OpenForms["frmPrincipal"] != null)
                if (Application.OpenForms["RadFrmMain"] != null)
                {
                    int i = 0;
                    bool seguir = true;
                    Form lanzadera = null;
                    while (seguir)
                    {
                        if (i < Application.OpenForms.Count)
                        {
                            Form f = Application.OpenForms[i];

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
                            string nombre = ((Form)f).GetType().FullName;

                            //if (nombre != "SmartCG.frmPrincipal" && nombre != "SmartCG.frmEntornoLista")
                            //if (nombre != "SmartCG.frmPrincipal")

                            if (nombre != "SmartCG.RadFrmMain")
                            {
                                ((Form)f).Close();

                                if (Application.OpenForms.Count > 2) i = 0;
                                else i++;
                            }
                            else
                            {
                                lanzadera = f;
                                i++;
                            }

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

                    if (lanzadera != null)
                    {
                        if (lanzadera is IFormEntorno formInterface)
                            formInterface.ModulosDeshabilitar();
                        /*IFormEntorno formInterface = this.Owner as IFormEntorno;

                        if (formInterface != null)
                            formInterface.ModulosDeshabilitar();*/
                    }
                }
            }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            this.NuevoEntorno();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarEntorno();
        }

        private void RadButtonCopiarFormato_Click(object sender, EventArgs e)
        {
            this.CopiarEntorno();
        }

        private void RadButtonSuprimir_Click(object sender, EventArgs e)
        {
            this.EliminarEntorno();
        }

        private void RadButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadButtonCargar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCargar);
        }

        private void RadButtonCargar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCargar);
        }

        private void RadButtonNuevo_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonNuevo);
        }

        private void RadButtonNuevo_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonNuevo);
        }

        private void RadButtonEditar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEditar);
        }

        private void RadButtonEditar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEditar);
        }

        private void RadButtonCopiarFormato_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCopiarFormato);
        }

        private void RadButtonCopiarFormato_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCopiarFormato);
        }

        private void RadButtonSuprimir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSuprimir);
        }

        private void RadButtonSuprimir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSuprimir);
        }

        private void RadButtonCancelar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonCancelar);
        }

        private void RadButtonCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonCancelar);
        }

        private void FrmEntornoLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de Entornos");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Falta traducir literales!!

            //Recuperar literales del formulario
            this.Text = "   " + this.LP.GetText("lblfrmEntornoTitulo", "Lista de entornos");

            //DataGrid
            this.radGridViewEntornos.Columns["archivo"].HeaderText = this.LP.GetText("dgEntornoHeaderArchivo", "Archivo");
            this.radGridViewEntornos.Columns["nombre"].HeaderText = this.LP.GetText("dgEntornoHeaderNombre", "Nombre");
            this.radGridViewEntornos.Columns["proveedorDatos"].HeaderText = this.LP.GetText("dgEntornoHeaderProveedorDatos", "Proveedor Datos");
            this.radGridViewEntornos.Columns["tipoBaseDatos"].HeaderText = this.LP.GetText("dgEntornoHeaderTipoBaseDatos", "Tipo Base Datos");
            this.radGridViewEntornos.Columns["IPoNombreServidor"].HeaderText = this.LP.GetText("dgEntornoHeaderIPoNombreServer", "IP o nombre del servidor");
            this.radGridViewEntornos.Columns["nombrebbdd"].HeaderText = this.LP.GetText("dgEntornoHeaderNombreBBDD", "Nombre de la base de datos");
            this.radGridViewEntornos.Columns["cadenaConexion"].HeaderText = this.LP.GetText("dgEntornoHeaderCadenaConexion", "Cadena Conexión");
            this.radGridViewEntornos.Columns["lastDSNContab"].HeaderText = this.LP.GetText("dgEntornoHeaderLastDSNContab", "Ult. DSN");
            this.radGridViewEntornos.Columns["lastUserContab"].HeaderText = this.LP.GetText("dgEntornoHeaderLastUserContab", "Ult. Usuario Servidor");
            this.radGridViewEntornos.Columns["lastUserApp"].HeaderText = this.LP.GetText("dgEntornoHeaderLastUserApp", "Ult. Usuario App");
            this.radGridViewEntornos.Columns["prefijoTabla"].HeaderText = this.LP.GetText("dgEntornoHeaderPrefijoTabla", "Prefijo Tabla");
            this.radGridViewEntornos.Columns["bbddCGAPP"].HeaderText = this.LP.GetText("dgEntornoHeaderBbddCGAPP", "base datos de CGAPP");
            this.radGridViewEntornos.Columns["bbddCGUF"].HeaderText = "base datos de CGUF";
            this.radGridViewEntornos.Columns["USER_CGIFS"].HeaderText = this.LP.GetText("dgEntornoHeaderUSER_CGIFS", "Usuario CGIFS");
            this.radGridViewEntornos.Columns["siiAgencia"].HeaderText = "SII Agencia";
            this.radGridViewEntornos.Columns["siiEntorno"].HeaderText = "SII Entorno";
            this.radGridViewEntornos.Columns["Activo"].HeaderText = this.LP.GetText("dgEntornoHeaderActivo", "Activo");

            this.radGridViewEntornos.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar por dicha columna";

            this.lblNoHayEntorno.Text = this.LP.GetText("lblfrmEntornoNoExisten", "No se han definido entornos. Debe crear al menos un entorno.");
        }

        /// <summary>
        /// Construye el dataGrid
        /// </summary>
        private void BuildDataGrid()
        {
            try
            {
                this.entorno = new ObjectModel.Entorno();

                this.radGridViewEntornos.DataSource = this.entorno.DTEntorno;

                //Ocultar columnas
                this.radGridViewEntornos.Columns["cadenaConexion"].Width = 0;
                this.radGridViewEntornos.Columns["cadenaConexion"].IsVisible = false;
                this.radGridViewEntornos.Columns["lastDSNContab"].Width = 0;
                this.radGridViewEntornos.Columns["lastDSNContab"].IsVisible = false;
                this.radGridViewEntornos.Columns["lastUserContab"].Width = 0;
                this.radGridViewEntornos.Columns["lastUserContab"].IsVisible = false;
                this.radGridViewEntornos.Columns["lastUserApp"].Width = 0;
                this.radGridViewEntornos.Columns["lastUserApp"].IsVisible = false;
                this.radGridViewEntornos.Columns["prefijoTabla"].Width = 0;
                this.radGridViewEntornos.Columns["prefijoTabla"].IsVisible = false;
                this.radGridViewEntornos.Columns["bbddCGAPP"].Width = 0;
                this.radGridViewEntornos.Columns["bbddCGAPP"].IsVisible = false;
                this.radGridViewEntornos.Columns["bbddCGUF"].Width = 0;
                this.radGridViewEntornos.Columns["bbddCGUF"].IsVisible = false;
                this.radGridViewEntornos.Columns["USER_CGIFS"].Width = 0;
                this.radGridViewEntornos.Columns["USER_CGIFS"].IsVisible = false;
                this.radGridViewEntornos.Columns["siiAgencia"].Width = 0;
                this.radGridViewEntornos.Columns["siiAgencia"].IsVisible = false;
                this.radGridViewEntornos.Columns["siiEntorno"].Width = 0;
                this.radGridViewEntornos.Columns["siiEntorno"].IsVisible = false;
            }
            catch(Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }

        /// <summary>
        /// Rellena el DataGrid con la información correspondiente
        /// </summary>
        private void FillDataGrid()
        {
            //Leer todos los ficheros con extensión xml que existan dentro de la carpeta ModComp_PathFicherosCompContables
            try
            {
                
                string result = this.entorno.LeerTodosEntornos();

                if (this.entorno.DTEntorno.Rows.Count == 0)
                {
                    this.radGridViewEntornos.Visible = false;
                    this.lblNoHayEntorno.Visible = true;

                    utiles.ButtonEnabled(ref this.radButtonCargar, false);
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref this.radButtonCopiarFormato, false);
                    utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
                }
                else
                {
                    this.radGridViewEntornos.Rows[0].IsCurrent = true;
                    this.radGridViewEntornos.Visible = true;
                    this.lblNoHayEntorno.Visible = false;

                    utiles.ButtonEnabled(ref this.radButtonCargar, true);
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref this.radButtonCopiarFormato, true);
                    utiles.ButtonEnabled(ref this.radButtonSuprimir, true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Llamada a Editar el entorno
        /// </summary>
        private void EditarEntorno()
        {
            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.dgEntornos.CurrentRow.Index;
            int indice = this.radGridViewEntornos.CurrentRow.Index;

            Entorno entornoAux = new Entorno();
            entornoAux.InstanciarEntorno(this.entorno, indice);
            entornoAux.AdicionarEntornoLista(entornoAux);

            frmEntornoAltaEdita frmEntornoEdit = new frmEntornoAltaEdita
            {
                Nuevo = false,
                EntornoActual = entornoAux,
                Indice = indice,
                //frmEntornoEdit.Archivo = this.dgEntornos.Rows[indice].Cells["archivo"].Value.ToString();
                Archivo = this.radGridViewEntornos.Rows[indice].Cells["archivo"].Value.ToString()
            };
            //frmEntornoEdit.RowEntorno =  this.entorno.DTEntorno.Rows[indice];
            frmEntornoEdit.ShowDialog(this);
            
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Copiar el entorno
        /// </summary>
        private void CopiarEntorno()
        {
            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.dgEntornos.CurrentRow.Index;
            int indice = this.radGridViewEntornos.CurrentRow.Index;
            Entorno entornoAux = entorno;
            entornoAux.InstanciarEntorno(this.entorno, indice);

            frmEntornoAltaEdita frmEntornoEdit = new frmEntornoAltaEdita
            {
                Nuevo = false,
                Copiar = true,
                EntornoActual = entornoAux,
                //frmEntornoEdit.Archivo = this.dgEntornos.Rows[indice].Cells["archivo"].Value.ToString();
                Archivo = this.radGridViewEntornos.Rows[indice].Cells["archivo"].Value.ToString()
            };
            //frmEntornoEdit.RowEntorno = this.entorno.DTEntorno.Rows[indice];
            frmEntornoEdit.ShowDialog(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Nuevo entorno
        /// </summary>
        private void NuevoEntorno()
        {
            Cursor.Current = Cursors.WaitCursor;

            Entorno entornoAux = new Entorno();
            entornoAux.InicializarEntorno(true);

            frmEntornoAltaEdita frmEntornoNuevo = new frmEntornoAltaEdita
            {
                Nuevo = true,
                EntornoActual = entornoAux
            };
            //frmEntornoNuevo.RowEntorno = dr;
            frmEntornoNuevo.ShowDialog(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Llamada a Eliminar entorno
        /// </summary>
        private void EliminarEntorno()
        {
            Cursor.Current = Cursors.WaitCursor;

            //int indice = this.dgEntornos.CurrentRow.Index;
            int indice = this.radGridViewEntornos.CurrentRow.Index;

            string error = this.LP.GetText("errValTitulo", "Error");

            if (Convert.ToBoolean(this.radGridViewEntornos.Rows[indice].Cells["activo"].Value))
            {
                MessageBox.Show("No es posible eliminar el entorno porque es el activo", error);    //Falta traducir
            }
            else
            {
                if (this.radGridViewEntornos.Rows.Count == 1)
                {
                    MessageBox.Show("Al menos tiene que existir un entorno", error);    //Falta traducir
                }
                else
                {
                    string advertencia2 = this.LP.GetText("wrnSuprimirEntornoPregunta", "¿Desea continuar?");   //Falta traducir
                    string advertencia1 = this.LP.GetText("wrnSuprimirEntorno", "Se va a eliminar el entorno");
                    //archivo = this.dgEntornos.Rows[indice].Cells["archivo"].Value.ToString();
                    string archivo = this.radGridViewEntornos.Rows[indice].Cells["archivo"].Value.ToString();
                    //Pedir confirmación
                    string mensaje = advertencia1 + " \"" + archivo + " \"." + advertencia2;
                    string advertenciaTitulo = this.LP.GetText("wrnTitulo", "Advertencia");
                    DialogResult res = MessageBox.Show(mensaje, advertenciaTitulo, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        try
                        {
                            string fichero = this.entorno.EntornoXMLPathFichero + "\\" + archivo;
                            File.Delete(fichero);
                        }
                        catch(Exception ex)
                        {
                            Log.Error(Utiles.CreateExceptionString(ex));
                        }

                        this.entorno.DTEntorno.Rows[indice].Delete();   
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Valida que el entorno seleccionado sea correcto
        /// </summary>
        /// <returns></returns>
        private bool EntornoValid()
        {
            bool result = true;

            return (result);
        }
        #endregion
    }
}
