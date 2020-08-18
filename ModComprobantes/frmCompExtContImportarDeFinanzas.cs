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
    public partial class frmCompExtContImportarDeFinanzas :  frmPlantilla, IReLocalizable
    {
        private string tipoBaseDatosCG = "";
        private const string prefijoColumnaPeriodo = "PRD";

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

        public frmCompExtContImportarDeFinanzas()
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

            this.radGridViewComprobantes.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompExtContImportarDeFinanzas_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Importar Comprobantes Extracontables de Finanzas");

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
                this.radButtonImportar.Location = new Point(this.radButtonImportar.Location.X, this.radButtonImportar.Location.Y + (this.radButtonImportar.Size.Height / 2) + 4);
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

            utiles.ButtonEnabled(ref this.radButtonImportar, false);

            this.cmbCompania.Select();
        }

        private void TxtNoComprobante_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtNoComprobante, false, ref sender, ref e);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
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

                string query = "select CCIAP3, SAPCP3, TICOP3, NUCOP3, FECOP3, STATP3, count(SIMIP3) as NOMOV from " + GlobalVar.PrefijoTablaCG + "PRB01 ";
                query += "where CCIAP3 ='" + codigo + "'";

                if (sap != "")
                {
                    //Coger el campo año período con siglo 
                    string aaCampoAP = sap.Substring(0, 2);
                    query += " and SAPCP3 =" + utiles.SigloDadoAnno(aaCampoAP, CGParametrosGrles.GLC01_ALSIRC) + sap;
                }
                else todos = false;

                if (this.cmbTipo.Text.Trim() != "")
                {
                    string tipo = "";
                    if (this.cmbTipo.Text.Length <= 2) tipo = this.cmbTipo.Text;
                    else tipo = this.cmbTipo.Text.Substring(0, 2);
                    query += " and TICOP3 =" + tipo;
                }
                else todos = false;

                if (this.txtNoComprobante.Text.Trim() != "")
                {
                    query += " and NUCOP3 =" + this.txtNoComprobante.Text;
                }
                else todos = false;

                query += " GROUP BY CCIAP3, SAPCP3, TICOP3, NUCOP3, FECOP3, STATP3";

                IDataReader dr = null;
                try
                {
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (todos)
                    {
                        this.radGridViewComprobantes.Visible = false;
                        utiles.ButtonEnabled(ref this.radButtonImportar, false);

                        codigo = "";
                        if (dr.Read())
                        {
                            this.progressBarEspera.Value = 0;
                            this.progressBarEspera.Visible = true;

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.Refresh();

                            //Crear el comprobante extracontable
                            ComprobanteExtContable comprobante = new ComprobanteExtContable();

                            codigo = dr["CCIAP3"].ToString().Trim();
                            comprobante.Cab_compania = codigo;
                            comprobante.Cab_anoperiodo = dr["SAPCP3"].ToString().Trim();
                            comprobante.Cab_tipo = dr["TICOP3"].ToString().Trim();
                            comprobante.Cab_noComprobante = dr["NUCOP3"].ToString().Trim();
                            comprobante.Cab_fecha = dr["FECOP3"].ToString().Trim();
                            comprobante.Cab_noMov = dr["NOMOV"].ToString().Trim();

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();

                            //Obtener los detalles del comprobante a importar
                            comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(ref comprobante);

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
                        if (this.radGridViewComprobantes.Rows.Count > 0) this.dtComprobantes.Rows.Clear();

                        string compania;
                        string sigloanoper;
                        string tipo;
                        string numero;
                        string aappAux;

                        int cantComp = 0;

                        DataRow rowComp;

                        while (dr.Read())
                        {
                            if (cantComp == 0)
                            {
                                this.progressBarEspera.Value = 0;
                                this.progressBarEspera.Visible = true;
                            }

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();

                            rowComp = this.dtComprobantes.NewRow();

                            compania = dr["CCIAP3"].ToString().Trim();
                            rowComp["compania"] = compania;

                            sigloanoper = dr["SAPCP3"].ToString().Trim();
                            aappAux = sigloanoper;
                            if (sigloanoper.Length == 5) aappAux = sigloanoper.Substring(1, 4);
                            aappAux = aappAux.Substring(0, 2) + "-" + aappAux.Substring(2, 2);
                            rowComp["AAPP"] = aappAux;

                            tipo = dr["TICOP3"].ToString().Trim();
                            if (tipo.Length == 1) tipo = "0" + tipo;
                            rowComp["Tipo"] = tipo;

                            numero = dr["NUCOP3"].ToString().Trim();
                            rowComp["NoComp"] = numero;

                            rowComp["Fecha"] = utiles.FechaToFormatoCG(dr["FECOP3"].ToString()).ToShortDateString();
                            rowComp["NoMovimiento"] = dr["NOMOV"].ToString().Trim();
                            rowComp["Estado"] = this.ObtenerEstadoComprobante(dr["STATP3"].ToString()).Trim();
                            
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

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
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

        private void RadGridViewComprobantes_CellDoubleClick(object sender, GridViewCellEventArgs e)
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

        private void RadButtonImportar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonImportar);
        }

        private void RadButtonImportar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonImportar);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void FrmCompExtContImportarDeFinanzas_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.radButtonExit.Visible && e.KeyValue == 27) this.RadButtonExit_Click(sender, null);
        }
        
        private void FrmCompExtContImportarDeFinanzas_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Importar Comprobantes Extracontables de Finanzas");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompExtContImportarDeFinanzasTitulo", "Importar Comprobantes Extracontables de Finanzas");	//Falta darle de alta y traducir
            //Traducir las etiquetas de la Cabecera
            //this.gbCabecera.Text = this.LP.GetText("lblCabecera", "Cabecera");
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
            this.btnBuscar.Text = "   " + this.LP.GetText("lblCompContImpDeFinBuscar", "Buscar");
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
            this.dtComprobantes.Columns.Add("Tipo", typeof(string));
            this.dtComprobantes.Columns.Add("NoComp", typeof(string));
            this.dtComprobantes.Columns.Add("Fecha", typeof(string));         
            this.dtComprobantes.Columns.Add("NoMovimiento", typeof(string));
            this.dtComprobantes.Columns.Add("Estado", typeof(string));

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
            string query = "Select CCIAMG, NCIAMG From " + GlobalVar.PrefijoTablaCG + "GLM01 where STATMG='V' Order by CCIAMG";
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
            string query = "Select TIVOTV, NOMBTV From " + GlobalVar.PrefijoTablaCG + "GLT06 Order by TIVOTV";
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
                    { "Tipo", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NoComp", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "Fecha", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "NoMovimiento", this.LP.GetText("CompContdgHeaderNoMovimiento", "No Mov") },
                    { "Estado", this.LP.GetText("lblCompTransEstado", "Estado") }
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

            if (this.cmbCompania.Text.Trim() == "")
            {
                RadMessageBox.Show(this.LP.GetText("errCompaniaObl", "Es obligatorio informar la compañía"), error);
                this.cmbCompania.Focus();
                return (false);
            }

            if (sap.Trim() == "" && this.cmbTipo.Text.Trim() == "")
            {
                RadMessageBox.Show(this.LP.GetText("errAAPPoTipoObl", "Es obligatorio informar el año-periodo o el tipo"), error);
                this.txtMaskAAPP.Focus();
                return (false);
            }

            if (this.txtNoComprobante.Text.Trim() != "")
            {
                if (sap.Trim() == "" && this.cmbTipo.Text.Trim() == "")
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
        private DataTable ObtenerDetallesComprobanteImportar(ref ComprobanteExtContable comprobante)
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
            dtDetalle.Columns.Add("TipoExt", typeof(string));
            dtDetalle.Columns.Add("Descripcion", typeof(string));
            DataRow row = null;

            string query = "select * from " + GlobalVar.PrefijoTablaCG + "PRB01 ";
            query += "where CCIAP3 ='" + comprobante.Cab_compania + "' and ";
            query += "SAPCP3 =" + comprobante.Cab_anoperiodo + " and ";
            query += "TICOP3 =" + comprobante.Cab_tipo + " and ";
            query += "NUCOP3 =" + comprobante.Cab_noComprobante;
            query += " order by CUENP3, CAX1P3, CAX2P3, CAX3P3, TMOVP3, TIDAP3, DESCP3, SAPRP3";        //Buscar el indice apropiado

            IDataReader dr = null;
            string periodoDesde = "";
            string periodoHasta = "";
            try
            {
                string periodoActual = "";
                string nombreColumnaPeriodo = "";

                string cuenta = "";
                string cuentaActual = "";
                string auxiliar1 = "";
                string auxiliar1Actual = "";
                string auxiliar2 = "";
                string auxiliar2Actual = "";
                string auxiliar3 = "";
                string auxiliar3Actual = "";
                string dh = "";
                string dhActual = "";
                string tipoExt = "";
                string tipoExtActual = "";
                string descripcion = "";
                string descripcionActual = "";

                bool insertarRow = false;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                while (dr.Read())
                {
                    //Mover la barra de progreso
                    if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                    else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                    this.progressBarEspera.Refresh();

                    cuentaActual = dr["CUENP3"].ToString().Trim();
                    auxiliar1Actual = dr["CAX1P3"].ToString().Trim();
                    auxiliar2Actual = dr["CAX2P3"].ToString().Trim();
                    auxiliar3Actual = dr["CAX3P3"].ToString().Trim();
                    dhActual = dr["TMOVP3"].ToString().Trim();
                    tipoExtActual = dr["TIDAP3"].ToString().Trim();
                    descripcionActual = dr["DESCP3"].ToString().Trim();

                    //Verificar si ha cambiado las filas "fijas" para grabar en otra fila
                    if (cuentaActual != cuenta || auxiliar1Actual != auxiliar1 || auxiliar2Actual != auxiliar2 || auxiliar3Actual != auxiliar3 ||
                        dhActual != dh || tipoExtActual != tipoExt || descripcionActual != descripcion)
                    {
                        if (insertarRow) dtDetalle.Rows.Add(row);

                        row = dtDetalle.NewRow();

                        row["Cuenta"] = cuentaActual;
                        cuenta = cuentaActual;
                        row["Auxiliar1"] = auxiliar1Actual;
                        auxiliar1 = auxiliar1Actual;
                        row["Auxiliar2"] = auxiliar2Actual;
                        auxiliar2 = auxiliar2Actual;
                        row["Auxiliar3"] = auxiliar3Actual;
                        auxiliar3 = auxiliar3Actual;
                        row["DH"] = dhActual;
                        dh = dhActual;
                        row["TipoExt"] = tipoExtActual;
                        tipoExt = tipoExtActual;
                        row["Descripcion"] = descripcionActual;
                        descripcion = descripcionActual;

                        insertarRow = true;
                    }

                    periodoActual = dr["SAPRP3"].ToString().Trim();
                    if (periodoActual.Length > 4) periodoActual = periodoActual.Substring(1, periodoActual.Length - 1);
                    nombreColumnaPeriodo = prefijoColumnaPeriodo + periodoActual;
                    //Chequear si ya existe el período como columna
                    if (this.ExistePeriodo(nombreColumnaPeriodo, dtDetalle))
                    {
                        row[nombreColumnaPeriodo] = dr["MONTP3"].ToString().Trim();
                    }
                    else
                    {
                        //Crea la columna y rellena las filas anteriores con vacio
                        if (this.CrearColumna(nombreColumnaPeriodo, ref dtDetalle))
                            row[nombreColumnaPeriodo] = dr["MONTP3"].ToString().Trim();

                        if (periodoDesde == "")
                        {
                            periodoDesde = periodoActual;
                            periodoHasta = periodoActual;
                        }
                        else periodoHasta = periodoActual;
                    }
                }

                dr.Close();

                if (insertarRow) dtDetalle.Rows.Add(row);

                //Actualizar el periodo inicial y periodo final del comprobante
                comprobante.Cab_periodoDesde = periodoDesde;
                comprobante.Cab_periodoHasta = periodoHasta;

                //Actualizar el tipo de extracontable por defecto
                comprobante.Cab_TipoExtDefecto = tipoExtActual;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }

            return (dtDetalle);
        }

        /// <summary>
        /// Chequea si existe la columna en el DataTable
        /// </summary>
        /// <param name="periodoActual">nombre del periodo</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ExistePeriodo(string periodoActual, DataTable dt)
        {
            bool result = false;
            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == periodoActual)
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Crea la columna de periodo en el datatable y la inicializa en vacía para todas las filas del datatable
        /// </summary>
        /// <param name="nombreColumnaPeriodo"></param>
        /// <param name="dtDetalle"></param>
        /// <returns></returns>
        private bool CrearColumna(string nombreColumnaPeriodo, ref DataTable dtDetalle)
        {
            bool result = false;

            try
            {
                //if (nombreColumnaPeriodo.Length > 4) nombreColumnaPeriodo = nombreColumnaPeriodo.Substring(1, nombreColumnaPeriodo.Length-1);

                dtDetalle.Columns.Add(nombreColumnaPeriodo, typeof(string));

                for (int i = 0; i < dtDetalle.Rows.Count; i++)
                {
                    dtDetalle.Rows[i][nombreColumnaPeriodo] = "";
                }

                result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Carga para edición el comprobante seleccionado del Grid
        /// </summary>
        /// <param name="row"></param>
        private void CargarComprobanteSeleccionado(GridViewRowInfo row)
        {
            this.progressBarEspera.Value = 0;
            this.progressBarEspera.Visible = true;

            //Mover la barra de progreso
            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
            this.progressBarEspera.Refresh();

            //Crear el comprobante contable
            ComprobanteExtContable comprobante = new ComprobanteExtContable();

            string codigo = row.Cells["compania"].Value.ToString();
            comprobante.Cab_compania = codigo;
            comprobante.Cab_anoperiodo = row.Cells["AAPP"].Value.ToString();

            //Coger el campo año período con siglo 
            string aa = comprobante.Cab_anoperiodo.Substring(0, 2);
            string saapp = comprobante.Cab_anoperiodo.Replace("-", "");
            saapp = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + saapp;

            comprobante.Cab_tipo = row.Cells["tipo"].Value.ToString();
            comprobante.Cab_noComprobante = row.Cells["noComp"].Value.ToString();
            comprobante.Cab_fecha = row.Cells["Fecha"].Value.ToString();
            comprobante.Cab_noMov = row.Cells["NoMovimiento"].Value.ToString();
            comprobante.Cab_anoperiodo = saapp;

            //Obtener los detalles del comprobante a importar
            comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(ref comprobante);

            this.CargarComprobanteEdicion(comprobante);

            this.progressBarEspera.Visible = false;
        }

        /// <summary>
        /// Llamar al formulario de edición con los datos del comprobante
        /// </summary>
        /// <param name="comprobante"></param>
        private void CargarComprobanteEdicion(ComprobanteExtContable comprobante)
        {
            //Cerrar el formulario actual ???
            frmCompExtContAltaEdita frmCompExtCont = new frmCompExtContAltaEdita
            {
                ImportarComprobante = true,
                ComprobanteExtContableImportar = comprobante,
                FrmPadre = this
            };
            frmCompExtCont.Show();
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

                if (cantRegistros == 0) result = "Compañía no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmImportarDeFinanzasValCiaExcep", "Error al validar la compañia") + " (" + ex.Message + ")";
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

                if (cantRegistros == 0) result = "Tipo no existe";   //Falta traducir
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmImportarDeFinanzasValTipoExcep", "Error al validar el tipo") + " (" + ex.Message + ")";
            }

            return (result);
        }
        #endregion
    }
}