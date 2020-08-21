using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ModComprobantes
{
    public partial class frmCompContImportarDeFinanzas :  frmPlantilla, IReLocalizable
    {
        private string tipoBaseDatosCG = "";

        Dictionary<string, string> displayNamesComprobantes;
        private DataTable dtComprobantes;

        private bool ventanaFlotante = false;

        public bool VentanaFlotante
        {
            get
            {
                return (this.ventanaFlotante);
            }
            set
            {
                this.ventanaFlotante = value;
            }
        }

        public frmCompContImportarDeFinanzas()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.gbCabecera.ElementTree.EnableApplicationThemeName = false;
            this.gbCabecera.ThemeName = "ControlDefault";

            this.txtMaskAAPP.AutoSize = false;
            this.txtMaskAAPP.Size = new Size(this.txtMaskAAPP.Width, 30);

            this.cmbCompania.AutoSize = false;
            this.cmbCompania.Size = new Size(this.cmbCompania.Width, 30);

            this.cmbTipo.AutoSize = false;
            this.cmbTipo.Size = new Size(this.cmbTipo.Width, 30);
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContImportarDeFinanzas_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Importar Comprobantes Contables de Finanzas");

            if (this.ventanaFlotante)
            {
                //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
                this.KeyPreview = true;

                this.radPanelMenuPath.Visible = false;
                utiles.ButtonEnabled(ref this.radButtonImportar, false);
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.BackColor = Color.White;
                this.radButtonExit.Visible = false;
                this.radButtonImportar.Visible = false;
                this.radButtonImportar.Location = new Point(this.radButtonImportar.Location.X, this.radButtonImportar.Location.Y + (this.radButtonImportar.Size.Height/2) + 4);
            }

            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Cargar compañías
            this.FillCompanias();

            //Cargar Tipos
            this.FillTiposComprobantes();

            //Crear el DataGrid
            this.CrearDataGrid();

            //Inicializar la Barra de Progreso
            this.InitProgressBar();

            this.cmbCompania.Select();
        }

        private void TxtNoComprobante_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtNoComprobante, false, ref sender, ref e);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            //coger el valor sin la máscara
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string sap = this.txtMaskAAPP.Value.ToString();
            this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

            if (this.FormValid(sap))
            {
                this.radGridViewComprobantes.Visible = false;
                utiles.ButtonEnabled(ref this.radButtonImportar, false);

                bool todos = true;
                string codigo = "";
                if (this.cmbCompania.Text.Length <= 2) codigo = this.cmbCompania.Text;
                else codigo = this.cmbCompania.Text.Substring(0,2);
                
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "where CCIAIC ='" + codigo + "'";

                if (sap != "")
                {
                    //Coger el campo año período con siglo 
                    string aaCampoAP = sap.Substring(0, 2);
                    query += " and SAPRIC =" + utiles.SigloDadoAnno(aaCampoAP, CGParametrosGrles.GLC01_ALSIRC) + sap;
                }
                else todos = false;

                if (this.cmbTipo.Text.Trim() != "")
                {
                    string tipo = "";
                    if (this.cmbTipo.Text.Length <= 2) tipo = this.cmbTipo.Text;
                    else tipo = this.cmbTipo.Text.Substring(0, 2);
                    query += " and TICOIC =" + tipo;
                }
                else todos = false;

                if (this.txtNoComprobante.Text.Trim() != "")
                {
                    query += " and NUCOIC =" + this.txtNoComprobante.Text;
                }
                else todos = false;

                this.progressBarEspera.Value = 0;

                //Mover la barra de progreso
                if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                this.Refresh();

                IDataReader dr = null;
                try
                {
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (todos)
                    {
                        codigo = "";
                        bool extendido = false;
                        if (dr.Read())
                        {
                            this.progressBarEspera.Visible = true;

                            //Crear el comprobante contable
                            ComprobanteContable comprobante = new ComprobanteContable();

                            codigo = dr["CCIAIC"].ToString().Trim();
                            comprobante.Cab_compania = codigo;
                            comprobante.Cab_anoperiodo = dr["SAPRIC"].ToString().Trim();
                            comprobante.Cab_tipo = dr["TICOIC"].ToString().Trim();
                            comprobante.Cab_noComprobante = dr["NUCOIC"].ToString().Trim();
                            comprobante.Cab_fecha = dr["FECOIC"].ToString().Trim();
                            comprobante.Cab_clase = dr["TVOUIC"].ToString().Trim();
                            comprobante.Cab_tasa = dr["TASCIC"].ToString().Trim();

                            //Verificar si el comprobante tiene campos extendidos
                            extendido = this.CamposExtendidos(codigo);
                            if (extendido) comprobante.Cab_extendido = "1";
                            else comprobante.Cab_extendido = "0";

                            comprobante.Cab_descripcion = this.ObtenerDescripcion(comprobante.Cab_compania, comprobante.Cab_anoperiodo,
                                                                                comprobante.Cab_tipo, comprobante.Cab_noComprobante);
                            comprobante.Cab_descripcion.Trim();

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();

                            //Obtener los detalles del comprobante a importar
                            comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(comprobante.Cab_compania, comprobante.Cab_anoperiodo,
                                                                                               comprobante.Cab_tipo, comprobante.Cab_noComprobante,
                                                                                               extendido);

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();

                            this.CargarComprobanteEdicion(comprobante);
                        }

                        dr.Close();
                        
                        //Cerrar esta ventana
                    }
                    else
                    {
                        if (this.radGridViewComprobantes.Rows.Count > 0) this.dtComprobantes.Clear();

                        string compania;
                        string sigloanoper;
                        string tipo;
                        string numero;
                        string aappAux;
                        string aux;

                        int cantComp = 0;

                        DataRow rowComp;

                        while (dr.Read())
                        {
                            this.progressBarEspera.Visible = true;

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();

                            rowComp = this.dtComprobantes.NewRow();

                            compania = dr["CCIAIC"].ToString().Trim();
                            rowComp["compania"] = compania;

                            sigloanoper = dr["SAPRIC"].ToString().Trim();
                            aappAux = sigloanoper;
                            if (sigloanoper.Length == 5) aappAux = sigloanoper.Substring(1, 4);
                            aappAux = aappAux.Substring(0, 2) + "-" + aappAux.Substring(2, 2);
                            rowComp["AAPP"] = aappAux;

                            tipo = dr["TICOIC"].ToString().Trim();
                            if (tipo.Length == 1) tipo = "0" + tipo;
                            rowComp["Tipo"] = tipo;

                            numero = dr["NUCOIC"].ToString().Trim();
                            rowComp["NoComp"] = numero;
                            
                            aux = this.ObtenerDescripcion(compania, sigloanoper, tipo, numero);
                            aux = aux.Trim();
                            rowComp["descripcion"] = aux;

                            rowComp["Fecha"] = utiles.FormatoCGToFecha(dr["FECOIC"].ToString()).ToShortDateString();
                            rowComp["Clase"] = dr["TVOUIC"].ToString().Trim();
                            rowComp["Tasa"] = dr["TASCIC"].ToString().Trim();
                            rowComp["DebeML"] = dr["DEBEIC"].ToString().Trim();
                            rowComp["HaberML"] = dr["HABEIC"].ToString().Trim();
                            rowComp["DebeME"] = dr["DEMEIC"].ToString().Trim();
                            rowComp["HaberME"] = dr["HAMEIC"].ToString().Trim();
                            rowComp["NoMovimiento"] = dr["SIMIIC"].ToString().Trim();
                            rowComp["Estado"] = this.ObtenerEstadoComprobante(dr["STATIC"].ToString()).Trim();
                            rowComp["FECOIC"] = dr["FECOIC"].ToString().Trim();
                            rowComp["SAPRIC"] = dr["SAPRIC"].ToString().Trim();

                            this.dtComprobantes.Rows.Add(rowComp);

                            cantComp++;
                        }

                        dr.Close();

                        if (cantComp > 0)
                        {
                            utiles.ButtonEnabled(ref this.radButtonImportar, true);

                            this.radGridViewComprobantes.Visible = true;

                            for (int i = 0; i < this.radGridViewComprobantes.Columns.Count; i++)
                                this.radGridViewComprobantes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                            this.radGridViewComprobantes.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                            this.radGridViewComprobantes.MasterTemplate.BestFitColumns();
                            this.radGridViewComprobantes.Rows[0].IsCurrent = true;
                        }
                        else
                        {
                            //No se encontraron comprobantes para la búsqueda solicitada
                            this.radGridViewComprobantes.Visible = false;
                            utiles.ButtonEnabled(ref this.radButtonImportar, false);
                            RadMessageBox.Show(this.LP.GetText("errBusqComp", "No se han encontrado comprobantes para el criterio de búsqueda seleccionado"), "");  //FALTA traducir
                        }
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    if (dr != null) dr.Close();
                    string error = ex.Message;
                }

                this.progressBarEspera.Visible = false;
            }

            if (this.radGridViewComprobantes.Visible) this.radButtonImportar.Visible = true;

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonImportar_Click(object sender, EventArgs e)
        {
            if (this.radGridViewComprobantes.SelectedRows.Count == 1)
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    Telerik.WinControls.UI.GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];

                    this.CargarComprobanteSeleccionado(row);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBuscar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnBuscar);
        }

        private void BtnBuscar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnBuscar);
        }

        private void RadButtonImportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonImportar);
        }

        private void RadButtonImportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonImportar);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadGridViewComprobantes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (this.radGridViewComprobantes.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewComprobantes.CurrentRow.IsExpanded) this.radGridViewComprobantes.CurrentRow.IsExpanded = false;
                else this.radGridViewComprobantes.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewComprobantes.CurrentRow is GridViewDataRowInfo)
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                Telerik.WinControls.UI.GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];

                this.CargarComprobanteSeleccionado(row);

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadGridViewComprobantes_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewComprobantes.Columns.Contains("FECOIC")) this.radGridViewComprobantes.Columns["FECOIC"].IsVisible = false;
            if (this.radGridViewComprobantes.Columns.Contains("SAPRIC")) this.radGridViewComprobantes.Columns["SAPRIC"].IsVisible = false;
        }

        private void FrmCompContImportarDeFinanzas_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.radButtonExit.Visible && e.KeyValue == 27) this.RadButtonExit_Click(sender, null);
        }

        private void FrmCompContImportarDeFinanzas_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Importar Comprobantes Contables de Finanzas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContImportarDeFinanzasTitulo", "Importar Comprobantes de Finanzas");
            //Traducir las etiquetas de la Cabecera
            //this.gbCabecera.Text = this.LP.GetText("lblCabecera", "Cabecera");
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
            this.btnBuscar.Text = this.LP.GetText("lblCompContImpDeFinBuscar", "Buscar");
        }

        /// <summary>
        /// Crea la Grid para mostrar los comprobantes
        /// </summary>
        private void CrearDataGrid()
        {
            this.dtComprobantes = new DataTable
            {
                TableName = "Tabla"
            };

            //Adicionar las columnas al DataTable
            this.dtComprobantes.Columns.Add("compania", typeof(string));
            this.dtComprobantes.Columns.Add("AAPP", typeof(string));
            this.dtComprobantes.Columns.Add("descripcion", typeof(string));
            this.dtComprobantes.Columns.Add("Tipo", typeof(string));
            this.dtComprobantes.Columns.Add("NoComp", typeof(string));
            this.dtComprobantes.Columns.Add("Fecha", typeof(string));
            this.dtComprobantes.Columns.Add("Clase", typeof(string));
            this.dtComprobantes.Columns.Add("Tasa", typeof(string));
            this.dtComprobantes.Columns.Add("DebeML", typeof(string));
            this.dtComprobantes.Columns.Add("HaberML", typeof(string));
            this.dtComprobantes.Columns.Add("DebeME", typeof(string));
            this.dtComprobantes.Columns.Add("HaberME", typeof(string));
            this.dtComprobantes.Columns.Add("NoMovimiento", typeof(string));
            this.dtComprobantes.Columns.Add("Estado", typeof(string));
            this.dtComprobantes.Columns.Add("FECOIC", typeof(string));
            this.dtComprobantes.Columns.Add("SAPRIC", typeof(string));

            this.radGridViewComprobantes.DataSource = this.dtComprobantes;
            //Escribe el encabezado de la Grid de Comprobantes
            this.BuildDisplayNamesComprobantes();
            this.RadGridViewComprobantesHeader();
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
            string query = "Select CCIAMG, NCIAMG From " + GlobalVar.PrefijoTablaCG + "GLM01 Order by CCIAMG";
            string result = this.FillComboBox(query, "CCIAMG", "NCIAMG", ref this.cmbCompania, true, -1, false);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetCompanias", "Error obteniendo las compañías") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Cargar los tipos de comprobantes
        /// </summary>
        private void FillTiposComprobantes()
        {
            //string query = "Select TIVOTV, NOMBTV From " + GlobalVar.PrefijoTablaCG + "GLT06 Order by TIVOTV";
            string query = "Select TIVOTV, NOMBTV From " + GlobalVar.PrefijoTablaCG + "GLT06";
            query += " where CODITV = '1' order by TIVOTV"; //jl solo presenta batch
            string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de comprobantes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }

        /// <summary>
        /// Encabezados para la Grid de Comprobantes
        /// </summary>
        private void BuildDisplayNamesComprobantes()
        {
            try
            {
                this.displayNamesComprobantes = new Dictionary<string, string>
                {
                    { "compania", this.LP.GetText("Compañía", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "descripcion", this.LP.GetText("CompContdgHeaderDescripcion", "Descripción") },
                    { "Tipo", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NoComp", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "Fecha", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "Clase", this.LP.GetText("CompContdgHeaderClase", "Clase") },
                    { "Tasa", this.LP.GetText("CompContdgHeaderTasa", "Tasa") },
                    { "DebeML", this.LP.GetText("CompContdgHeaderDebeML", "Debe ML") },
                    { "HaberML", this.LP.GetText("CompContdgHeaderHaberML", "Haber ML") },
                    { "DebeME", this.LP.GetText("CompContdgHeaderDebeME", "Debe ME") },
                    { "HaberME", this.LP.GetText("CompContdgHeaderHaberME", "Haber ME") },
                    { "NoMovimiento", this.LP.GetText("CompContdgHeaderNoMovimiento", "No Mov") },
                    { "Estado", this.LP.GetText("lblCompTransEstado", "Estado") },
                    { "FECOIC", "FECOIC" },
                    { "SAPRIC", "SAPRIC" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Comprobantes
        /// </summary>
        private void RadGridViewComprobantesHeader()
        {
            try
            {
                if (this.radGridViewComprobantes.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesComprobantes)
                    {
                        if (this.radGridViewComprobantes.Columns.Contains(item.Key)) this.radGridViewComprobantes.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }


        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <param name="sap"></param>
        /// <returns></returns>
        private bool FormValid(string sap)
        {
            string error = this.LP.GetText("errValTitulo", "Error");

            string compania = this.cmbCompania.Text.Trim();
            if (compania == "")
            {
                RadMessageBox.Show(this.LP.GetText("errCompaniaObl", "Es obligatorio informar la compañía"), error);
                this.cmbCompania.Focus();
                return (false);
            }

            string tipo = this.cmbTipo.Text.Trim();
            if (sap.Trim() == "" && tipo == "")
            {
                RadMessageBox.Show(this.LP.GetText("errAAPPoTipoObl", "Es obligatorio informar el año-periodo o el tipo"), error);
                this.txtMaskAAPP.Focus();
                return (false);
            }

            if (this.txtNoComprobante.Text.Trim() != "")
            {
                if (sap.Trim() == "" && tipo == "")
                {
                    RadMessageBox.Show(this.LP.GetText("errAAPPoTipoObl", "Es obligatorio informar el año-periodo o el tipo"), error);
                    this.txtMaskAAPP.Focus();
                    return (false);
                }
            }
            return (true);
        }

        /// <summary>
        /// Devuelve el estado del comprobante
        /// </summary>
        /// <param name="estado"></param>
        private string ObtenerEstadoComprobante(string estado)
        {
            string result = "";

            switch (estado)
            {
                case "A" :
                    result = this.LP.GetText("lblEstadoAprobado", "Aprobado");
                    break;
                case "E" :
                    result = this.LP.GetText("lblEstadoContabilizado", "Contabilizado");
                    break;
                case "R" :
                    result = this.LP.GetText("lblEstadoRechazado", "Rechazado");
                    break;
                case "V" :
                    result = this.LP.GetText("lblEstadoNoAprobado", "No aprobado");
                    break;
            }

            return (result);
        }

        /// <summary>
        /// Busca los detalles del comrpobante a editar
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="anoperiodo"></param>
        /// <param name="tipo"></param>
        /// <param name="noComprobante"></param>
        /// <returns></returns>
        private DataTable ObtenerDetallesComprobanteImportar(string compania, string anoperiodo, string tipo, string noComprobante, bool extendido)
        {
            DataTable dtDetalle = new DataTable
            {
                TableName = "Detalle"
            };

            dtDetalle.Columns.Add("Cuenta", typeof(string));
            dtDetalle.Columns.Add("Auxiliar1", typeof(string));
            dtDetalle.Columns.Add("Auxiliar2", typeof(string));
            dtDetalle.Columns.Add("Auxiliar3", typeof(string));
            dtDetalle.Columns.Add("DH", typeof(string));
            dtDetalle.Columns.Add("MonedaLocal", typeof(string));
            dtDetalle.Columns.Add("MonedaExt", typeof(string));
            dtDetalle.Columns.Add("RU", typeof(string));
            dtDetalle.Columns.Add("Descripcion", typeof(string));
            dtDetalle.Columns.Add("Documento", typeof(string));
            dtDetalle.Columns.Add("Fecha", typeof(string));
            dtDetalle.Columns.Add("Vencimiento", typeof(string));
            dtDetalle.Columns.Add("Documento2", typeof(string));
            dtDetalle.Columns.Add("Importe3", typeof(string));
            dtDetalle.Columns.Add("Iva", typeof(string));
            dtDetalle.Columns.Add("CifDni", typeof(string));

            if (extendido)
            {
                dtDetalle.Columns.Add("PrefijoDoc", typeof(string));
                dtDetalle.Columns.Add("NumFactAmp", typeof(string));
                dtDetalle.Columns.Add("NumFactRectif", typeof(string));
                dtDetalle.Columns.Add("FechaServIVA", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa1", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa2", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa3", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa4", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa5", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa6", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa7", typeof(string));
                dtDetalle.Columns.Add("CampoUserAlfa8", typeof(string));
                dtDetalle.Columns.Add("CampoUserNum1", typeof(string));
                dtDetalle.Columns.Add("CampoUserNum2", typeof(string));
                dtDetalle.Columns.Add("CampoUserFecha1", typeof(string));
                dtDetalle.Columns.Add("CampoUserFecha2", typeof(string));
            }

            DataRow row;

            string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
            query += "where CCIADT ='" + compania + "' and ";
            query += "SAPRDT =" + anoperiodo + " and ";
            query += "TICODT =" + tipo + " and ";
            query += "NUCODT =" + noComprobante;
            query += " order by SIMIDT";

            IDataReader dr = null;

            string simidt = "";

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string CLDODT = "";
                string NDOCDT = "";
                string FDOCDT = "";
                string FEVEDT = "";
                string CDDOAD = "";
                string NDDOAD = "";
                while (dr.Read())
                {
                    //Mover la barra de progreso
                    if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                    else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                    this.progressBarEspera.Refresh();

                    row = dtDetalle.NewRow();

                    row["Cuenta"] = dr["CUENDT"].ToString().Trim();
                    row["Auxiliar1"] = dr["CAUXDT"].ToString().Trim();
                    row["Auxiliar2"] = dr["AUAD01"].ToString().Trim();
                    row["Auxiliar3"] = dr["AUAD02"].ToString().Trim();
                    row["DH"] = dr["TMOVDT"].ToString().Trim();
                    row["MonedaLocal"] = dr["MONTDT"].ToString().Trim();
                    row["MonedaExt"] = dr["MOSMAD"].ToString().Trim();
                    row["RU"] = dr["TEINDT"].ToString().Trim();
                    row["Descripcion"] = dr["DESCAD"].ToString().Trim();
                    CLDODT = dr["CLDODT"].ToString().Trim();
                    NDOCDT = dr["NDOCDT"].ToString().Trim();
                    NDOCDT = NDOCDT.PadLeft(7, '0');
                    //if (CLDODT != "" && CLDODT != "0" && NDOCDT != "" && NDOCDT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    if (CLDODT != "" && CLDODT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    FDOCDT = dr["FDOCDT"].ToString().Trim();
                    if (FDOCDT != "" && FDOCDT != "0") row["Fecha"] = FDOCDT;
                    FEVEDT = dr["FEVEDT"].ToString().Trim();
                    if (FEVEDT != "" && FEVEDT != "0") row["Vencimiento"] = FEVEDT;
                    CDDOAD = dr["CDDOAD"].ToString().Trim();
                    NDDOAD = dr["NDDOAD"].ToString().Trim();
                    NDDOAD = NDDOAD.PadLeft(9, '0');
                    //if (NDDOAD != "" && NDDOAD != "0") row["Documento2"] = NDDOAD;
                    if (CDDOAD != "" && CDDOAD != "0") row["Documento2"] = CDDOAD + "-" + NDDOAD;
                    row["Importe3"] = dr["TERCAD"].ToString().Trim();
                    row["Iva"] = dr["CDIVDT"].ToString().Trim();
                    row["CifDni"] = dr["NNITAD"].ToString().Trim();

                    if (extendido)
                    {
                        //Si el compobante tiene campos extendidos, leer los valores de los campos extendidos para la línea de detalle
                        simidt = dr["SIMIDT"].ToString().Trim();
                        this.ObtenerDetalleCamposExtendidos(ref row, compania, anoperiodo, tipo, noComprobante, simidt);
                    }

                    dtDetalle.Rows.Add(row);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                string error = ex.Message;
            }

            return (dtDetalle);
        }

        /// <summary>
        /// Obtiene los campos extendidos para una línea de detalle
        /// </summary>
        /// <param name="row">fila de la línea de detalle del DataRow</param>
        /// <param name="compania">código de la compañía</param>
        /// <param name="anoperiodo">sigloanoperiodo</param>
        /// <param name="tipo">tipo de comprobante</param>
        /// <param name="noComprobante">número de comprobante</param>
        /// <param name="simidt">línea del comprobante</param>
        /// <returns></returns>
        private void ObtenerDetalleCamposExtendidos(ref DataRow row, string compania, string anoperiodo, string tipo, string noComprobante, string simidt)
        {
            string FIVADX = "";
            string USF1DX = "";
            string USF2DX = "";

            string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLBX1 ";
            query += "where CCIADX ='" + compania + "' and ";
            query += "SAPRDX =" + anoperiodo + " and ";
            query += "TICODX =" + tipo + " and ";
            query += "NUCODX =" + noComprobante + " and ";
            query += "SIMIDX =" + simidt;

            IDataReader dr = null;

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    row["PrefijoDoc"] = dr["PRFDDX"].ToString().Trim();
                    row["NumFactAmp"] = dr["NFAADX"].ToString().Trim();
                    row["NumFactRectif"] = dr["NFARDX"].ToString().Trim();
                    FIVADX = dr["FIVADX"].ToString().Trim();
                    row["FechaServIVA"] = "";
                    if (FIVADX != "" && FIVADX != "0") row["FechaServIVA"] = FIVADX;
                    //row["FechaServIVA"] = dr["FIVAWS"].ToString().Trim();
                    row["CampoUserAlfa1"] = dr["USA1DX"].ToString().Trim();
                    row["CampoUserAlfa2"] = dr["USA2DX"].ToString().Trim();
                    row["CampoUserAlfa3"] = dr["USA3DX"].ToString().Trim();
                    row["CampoUserAlfa4"] = dr["USA4DX"].ToString().Trim();
                    row["CampoUserAlfa5"] = dr["USA5DX"].ToString().Trim();
                    row["CampoUserAlfa6"] = dr["USA6DX"].ToString().Trim();
                    row["CampoUserAlfa7"] = dr["USA7DX"].ToString().Trim();
                    row["CampoUserAlfa8"] = dr["USA8DX"].ToString().Trim();
                    row["CampoUserNum1"] = dr["USN1DX"].ToString().Trim();
                    row["CampoUserNum2"] = dr["USN2DX"].ToString().Trim();
                    USF1DX = dr["USF1DX"].ToString().Trim();
                    row["CampoUserFecha1"] = "";
                    if (USF1DX != "" && USF1DX != "0") row["CampoUserFecha1"] = USF1DX;
                    //row["CampoUserFecha1"] = dr["USF1WS"].ToString().Trim();
                    USF2DX = dr["USF2DX"].ToString().Trim();
                    row["CampoUserFecha2"] = "";
                    if (USF2DX != "" && USF2DX != "0") row["CampoUserFecha2"] = USF2DX;
                    //row["CampoUserFecha2"] = dr["USF2WS"].ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                string error = ex.Message;
            }
        }

        /// <summary>
        /// Carga para edición el comprobante seleccionado del Grid
        /// </summary>
        /// <param name="row"></param>
        private void CargarComprobanteSeleccionado(Telerik.WinControls.UI.GridViewRowInfo row)
        {
            this.progressBarEspera.Value = 0;
            this.progressBarEspera.Visible = true;

            //Mover la barra de progreso
            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
            this.progressBarEspera.Refresh();

            //Crear el comprobante contable
            ComprobanteContable comprobante = new ComprobanteContable();

            string codigo = row.Cells["compania"].Value.ToString();
            comprobante.Cab_compania = codigo;
            comprobante.Cab_anoperiodo = row.Cells["SAPRIC"].Value.ToString();
            comprobante.Cab_descripcion = row.Cells["descripcion"].Value.ToString();
            comprobante.Cab_tipo = row.Cells["tipo"].Value.ToString();
            comprobante.Cab_noComprobante = row.Cells["noComp"].Value.ToString();
            comprobante.Cab_fecha = row.Cells["FECOIC"].Value.ToString();
            comprobante.Cab_clase = row.Cells["clase"].Value.ToString();
            comprobante.Cab_tasa = row.Cells["tasa"].Value.ToString();

            //Verificar si el comprobante tiene campos extendidos
            bool extendido = this.CamposExtendidos(codigo);
            if (extendido) comprobante.Cab_extendido = "1";
            else comprobante.Cab_extendido = "0";

            //Obtener los detalles del comprobante a importar
            comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(comprobante.Cab_compania, comprobante.Cab_anoperiodo,
                                                                               comprobante.Cab_tipo, comprobante.Cab_noComprobante,
                                                                               extendido);

            this.CargarComprobanteEdicion(comprobante);

            this.progressBarEspera.Visible = false;
        }

        /// <summary>
        /// Llamar al formulario de edición con los datos del comprobante
        /// </summary>
        /// <param name="comprobante"></param>
        private void CargarComprobanteEdicion(ComprobanteContable comprobante)
        {
            //Cerrar el formulario actual ???
            frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita
            {
                ImportarComprobante = true,
                ComprobanteContableImportar = comprobante,
                NombreComprobante = comprobante.Cab_descripcion,
                FrmPadre = this
            };
            frmCompCont.Show();
        }

        /// <summary>
        /// Verifica si la compañía utiliza los campos extendidos
        /// </summary>
        /// <returns></returns>
        private bool CamposExtendidos(string codigo)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Verificar que exista la tabla GLMX2
                bool existeTabla = utilesCG.ExisteTabla(tipoBaseDatosCG, "GLMX2");

                if (!existeTabla) return (result);

                //Buscar el plan de la compañía
                string query = "select NCIAMG, TITAMG, FELAMG, TIPLMG from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where STATMG = 'V' and CCIAMG = '" + codigo + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                string plan = "";

                if (dr.Read())
                {
                    plan = dr["TIPLMG"].ToString().Trim();
                }
                dr.Close();


                if (plan != "")
                {
                    //Buscar la información sobre los campos extendidos para el plan de la compañía
                    query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
                    query += "where TIPLPX = '" + plan + "'";

                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (dr.Read())
                    {
                        string FGPRPX = dr.GetValue(dr.GetOrdinal("FGPRPX")).ToString();
                        string FGFAPX = dr.GetValue(dr.GetOrdinal("FGFAPX")).ToString();
                        string FGFRPX = dr.GetValue(dr.GetOrdinal("FGFRPX")).ToString();
                        string FGDVPX = dr.GetValue(dr.GetOrdinal("FGDVPX")).ToString();
                        string FG01PX = dr.GetValue(dr.GetOrdinal("FG01PX")).ToString();
                        string FG02PX = dr.GetValue(dr.GetOrdinal("FG02PX")).ToString();
                        string FG03PX = dr.GetValue(dr.GetOrdinal("FG03PX")).ToString();
                        string FG04PX = dr.GetValue(dr.GetOrdinal("FG04PX")).ToString();
                        string FG05PX = dr.GetValue(dr.GetOrdinal("FG05PX")).ToString();
                        string FG06PX = dr.GetValue(dr.GetOrdinal("FG06PX")).ToString();
                        string FG07PX = dr.GetValue(dr.GetOrdinal("FG07PX")).ToString();
                        string FG08PX = dr.GetValue(dr.GetOrdinal("FG08PX")).ToString();
                        string FG09PX = dr.GetValue(dr.GetOrdinal("FG09PX")).ToString();
                        string FG10PX = dr.GetValue(dr.GetOrdinal("FG10PX")).ToString();
                        string FG11PX = dr.GetValue(dr.GetOrdinal("FG11PX")).ToString();
                        string FG12PX = dr.GetValue(dr.GetOrdinal("FG12PX")).ToString();

                        //Chequear que al menos exista una columna visible
                        if (FGPRPX == "0" && FGFAPX == "0" && FGFRPX == "0" && FGDVPX == "0" && FG01PX == "0" && FG01PX == "0" &&
                            FG02PX == "0" && FG03PX == "0" && FG04PX == "0" && FG05PX == "0" && FG06PX == "0" && FG06PX == "0" &&
                            FG07PX == "0" && FG08PX == "0" && FG08PX == "0" && FG09PX == "0" && FG10PX == "0" && FG11PX == "0" &&
                            FG12PX == "0")
                        {
                            dr.Close();
                            return (result);
                        }

                        result = true;
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (result);
        }

        private void InitProgressBar()
        {
            this.progressBarEspera.Value = 0;
            this.progressBarEspera.MarqueeAnimationSpeed = 30;
            this.progressBarEspera.Style = ProgressBarStyle.Marquee;
            this.progressBarEspera.Visible = false;
            this.progressBarEspera.Value = 0;
            this.progressBarEspera.Maximum = 100;
        }

        /// <summary>
        /// Valida la existencia o no de la compañia
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarCompania(string codigo)
        {
            string result = "";
            try
            {
                string query = "select count(CCIAMG) from " + GlobalVar.PrefijoTablaCG + "GLM01 ";
                query += "where CCIAMG = '" + codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("errCompaniaNoExiste", "Compañía no existe");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmImportarDeFinanzasValCiaExcep", "Error al validar la compañía") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Valida la existencia o no del tipo
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string ValidarTipo(string codigo)
        {
            string result = "";
            try
            {
                string query = "select count(TIVOTV) from " + GlobalVar.PrefijoTablaCG + "GLT06 ";
                query += "where TIVOTV='" + codigo + "'";
                //query += "where CODITV='1' and STATTV='V' and TIVOTV='" + codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("errTipoNoExiste", "Tipo no existe");
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmImportarDeFinanzasValTipoExcep", "Error al validar el tipo") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Añadir una celda a una fila del DataGrid
        /// </summary>
        /// <param name="row">Fila</param>
        /// <param name="valor">Valor de la celda</param>
        private void AddDataGridViewTextBoxCell(ref DataGridViewRow row, string valor)
        {
            DataGridViewCell cell = new DataGridViewTextBoxCell
            {
                Value = valor
            };
            row.Cells.Add(cell);
        }

        /// <summary>
        /// Obtiene la descripción del comprobante
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="sigloanoper"></param>
        /// <param name="tipo"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        private string ObtenerDescripcion(string compania, string sigloanoper, string tipo, string numero)
        {
            string desc = "";
            string query = "select COHEAD from " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
            query += "where CCIAAD='" + compania + "' and SAPRAD=" + sigloanoper + " and TICOAD=" + tipo + " and NUCOAD=" + numero;

            IDataReader dr = null;

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr["COHEAD"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (desc);
        }
        #endregion
    }
}