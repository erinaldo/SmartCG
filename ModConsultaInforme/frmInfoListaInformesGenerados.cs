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
using System.Diagnostics;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmInfoListaInformesGenerados :  frmPlantilla, IReLocalizable
    {
        public string formCode = "MCLIGEN";
        public string formCodeNombFichero = "INFGENE";

        StringBuilder documento_HTML;
        private string tipoFichero = "EXCEL";

        Dictionary<string, string> displayNames;
        DataTable dtInformesGenerados = new DataTable();

        private string nombreFicheroAGenerar = "";

        private string mensajeProceso = "";

        public frmInfoListaInformesGenerados()
        {
            InitializeComponent();

            this.radGridViewInformesGenerados.MasterView.TableSearchRow.IsVisible = false;
        }

        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            //this.TraducirLiterales();
        }

        #region Eventos
        private void FrmInfoListaInformesGenerados_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista Informes Generados");

            Cursor.Current = Cursors.WaitCursor;

            this.BuildDisplayNames();

            //Crear el DataTable para la Grid
            this.BuilDataTableInformesGenerados();

            //Cargar los datos de la Grid
            this.FillDataGrid();

            Cursor.Current = Cursors.Default;
        }

        private void RadButtonDescargarInforme_Click(object sender, EventArgs e)
        {
            this.DescargarInforme();
        }

        private void RadGridViewInformesGenerados_CellClick(object sender, GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewInformesGenerados, ref this.selectAll);
        }

        private void RadButtonActualizarLista_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.FillDataGrid();
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonEliminarInforme_Click(object sender, EventArgs e)
        {
            this.EliminarInformes();
        }

        private void RadDataFilterGridInfo_EditorInitialized(object sender, Telerik.WinControls.UI.TreeNodeEditorInitializedEventArgs e)
        {
            if (e.Editor is Telerik.WinControls.UI.TreeViewDropDownListEditor editor && e.NodeElement is Telerik.WinControls.UI.DataFilterCriteriaElement criteriaElement)
            {
                if (criteriaElement.EditingElement is Telerik.WinControls.UI.DataFilterFieldEditorElement)
                {
                    var element = editor.EditorElement as Telerik.WinControls.UI.BaseDropDownListEditorElement;
                    element.DataSource = displayNames;
                    element.ValueMember = "Key";
                    element.DisplayMember = "Value";
                }
            }
        }

        private void RadDataFilterGridInfo_NodeFormatting(object sender, Telerik.WinControls.UI.TreeNodeFormattingEventArgs e)
        {
            try
            {
                if (e.NodeElement is Telerik.WinControls.UI.DataFilterCriteriaElement dataExpressionFilterElement)
                {
                    var node = dataExpressionFilterElement.Data as Telerik.WinControls.UI.DataFilterCriteriaNode;
                    if (displayNames.ContainsKey(node.PropertyName))
                    {
                        dataExpressionFilterElement.FieldElement.Text = displayNames[node.PropertyName];
                    }
                }
            }
            catch { }
        }

        private void RadButtonListaInformes_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDescargarInforme);
        }

        private void RadButtonListaInformes_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDescargarInforme);
        }

        private void RadButtonEliminarInforme_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEliminarInforme);
        }

        private void RadButtonEliminarInforme_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEliminarInforme);
        }

        private void RadGridViewInformesGenerados_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            this.DescargarInforme();
        }

        private void RadButtonDescargarInforme_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonDescargarInforme);
        }

        private void RadButtonDescargarInforme_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonDescargarInforme);
        }

        private void RadButtonActualizarLista_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonActualizarLista);
        }

        private void RadButtonActualizarLista_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonActualizarLista);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.DescargarInformeGenerado();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.mensajeProceso != "") RadMessageBox.Show(this.mensajeProceso);
            else
            {
                try
                {
                    //Actualizar estado si procede
                    int indice = this.radGridViewInformesGenerados.Rows.IndexOf(this.radGridViewInformesGenerados.CurrentRow);
                    string sSTATW8 = this.radGridViewInformesGenerados.Rows[indice].Cells["STATW8"].Value.ToString();
                    if (sSTATW8 == "Terminada")
                    {
                        //Actualizar el estado a Importada
                        string sRUUNW8 = this.radGridViewInformesGenerados.Rows[indice].Cells["RUUNW8"].Value.ToString();
                        bool resultUpdate = this.ActualizarEstadoInforme(sRUUNW8, "I");
                        if (resultUpdate) this.radGridViewInformesGenerados.Rows[indice].Cells["STATW8"].Value = "Importada";
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                
                DialogResult result = RadMessageBox.Show(this, "Informe generado con éxito. ¿Desea visualizar el informe (" + this.nombreFicheroAGenerar + ")?", "Informe Generado", MessageBoxButtons.YesNo, RadMessageIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        //Abrir el fichero
                        string fichero = this.nombreFicheroAGenerar;
                        if (GlobalVar.UsuarioEnv.ModConsInfo_TipoFicherosInformes == "EXCEL")
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = tipoFichero, //coincide tipoFichero y nombre del .exe (excel)
                                                        //startInfo.FileName = "EXCEL.EXE";
                                Arguments = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                            };
                            Process.Start(startInfo);
                        }
                        else
                        {
                            //HTML
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                UseShellExecute = true,
                                FileName = "\"" + fichero + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                            };

                            Process.Start(startInfo);
                        }
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    //MessageBox.Show("No was pressed");
                }
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int opcion = e.ProgressPercentage;

            switch (opcion)
            {
                case 0:
                    this.IniciaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo, ProgressBarStyle.Marquee);
                    break;
                case 1:
                    this.FinalizaBarraProgreso(ref this.pBarProcesandoInfo, ref this.lblProcesandoInfo);
                    break;
                case 2:
                    if (this.pBarProcesandoInfo.Value + 10 > this.pBarProcesandoInfo.Maximum) this.pBarProcesandoInfo.Value = this.pBarProcesandoInfo.Minimum;
                    else this.pBarProcesandoInfo.Value += 10;
                    break;
            }
        }

        private void FrmInfoListaInformesGenerados_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista Informes Generados");
        }
        private void radGridViewInformesGenerados_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font newFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (e.CellElement is GridHeaderCellElement || e.CellElement is GridGroupContentCellElement)
            {
                e.CellElement.Font = newFont;
            }
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Construir el control de la Grid que contiene las Solicitudes de grupo de informes
        /// </summary>
        private void BuilDataTableInformesGenerados()
        {
            //Adicionar las columnas para el DataTable para la Grid
            this.dtInformesGenerados.Columns.Add("IDINW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("DESUW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("RUUNW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("STATW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("FECHW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("HORAW8", typeof(string));
            this.dtInformesGenerados.Columns.Add("NERRW8", typeof(string));

            this.radGridViewInformesGenerados.DataSource = this.dtInformesGenerados;
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>
                {
                    { "IDINW8", "Informe" },
                    { "DESUW8", "Descripción" },
                    { "RUUNW8", "Extracción" },
                    { "STATW8", "Estado" },
                    { "FECHW8", "Fecha" },
                    { "HORAW8", "Hora" },
                    { "NERRW8", "Errores" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Carga los datos de las Solicitudes en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                if (this.dtInformesGenerados != null && this.dtInformesGenerados.Rows != null && this.dtInformesGenerados.Rows.Count > 0) this.dtInformesGenerados.Clear();

                string query = "select IDINW8, DESUW8, RUUNW8, STATW8, FECHW8, HORAW8, NERRW8 from " + GlobalVar.PrefijoTablaCG + "EXW08 ";
                //query += "order by IDINW8, DESUW8, RUUNW8, STATW8, FECHW8, HORAW8, NERRW8";
                //string query = "select IDINW8, DESUW8, RUUNW8 from " + GlobalVar.PrefijoTablaCG + "EXW08 ";
                query += "order by IDINW8, RUUNW8";

                DataTable dtAux = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                string valorIDINW8 = "";
                string valorantIDINW8 = "";
                bool autorizado = false;

                DataRow rowDataTableGrid;
                
                foreach (DataRow dr in dtAux.Rows)
                {
                    valorIDINW8 = dr["IDINW8"].ToString();
                    if (valorantIDINW8 != valorIDINW8)
                    {
                        autorizado = this.UsuarioAutorizado(valorIDINW8);
                        valorantIDINW8 = valorIDINW8;
                    }
                    if (autorizado)
                    {
                        valorantIDINW8 = valorIDINW8;

                        rowDataTableGrid = this.dtInformesGenerados.NewRow();

                        rowDataTableGrid["IDINW8"] = valorantIDINW8;
                        rowDataTableGrid["DESUW8"] = dr["DESUW8"].ToString();
                        rowDataTableGrid["RUUNW8"] = dr["RUUNW8"].ToString();

                        switch (dr["STATW8"].ToString())
                        {
                            case "C":
                                rowDataTableGrid["STATW8"] = "En ejecución";
                                break;
                            case "T":
                                rowDataTableGrid["STATW8"] = "Terminada";
                                break;
                            case "I":
                                rowDataTableGrid["STATW8"] = "Importada";
                                break;
                            case "E":
                                rowDataTableGrid["STATW8"] = "Errores";
                                break;
                            default:
                                rowDataTableGrid["STATW8"] = " ";
                                break;
                        }

                        rowDataTableGrid["FECHW8"] = utiles.FechaToFormatoCG(dr["FECHW8"].ToString()).ToShortDateString();
                        rowDataTableGrid["HORAW8"] = ConvertirHora(dr["HORAW8"].ToString());
                        rowDataTableGrid["NERRW8"] = dr["NERRW8"].ToString().PadLeft(4, '0');

                        this.dtInformesGenerados.Rows.Add(rowDataTableGrid);
                    }
                }
                if (this.dtInformesGenerados != null && this.dtInformesGenerados.Rows != null && this.dtInformesGenerados.Rows.Count > 0)
                {
                    //this.radGridViewInformesGenerados.DataSource = this.dtInformesGenerados;
                    this.radGridViewInformesGenerados.Visible = true;
                    this.RadGridViewHeader();
                    this.radLabelNoHayInfo.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonDescargarInforme, true);
                    utiles.ButtonEnabled(ref this.radButtonEliminarInforme, true);

                    for (int i = 0; i < this.radGridViewInformesGenerados.Columns.Count; i++)
                    {
                        this.radGridViewInformesGenerados.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewInformesGenerados.Columns[i].Width = 600;
                    }

                    this.radGridViewInformesGenerados.TableElement.GridViewElement.GroupPanelElement.Text = "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                    this.radGridViewInformesGenerados.AllowSearchRow = true;
                    this.radGridViewInformesGenerados.MasterView.TableSearchRow.IsVisible = false;
                    this.radGridViewInformesGenerados.TableElement.SearchHighlightColor = Color.Aqua;
                    this.radGridViewInformesGenerados.AllowEditRow = false;
                    this.radGridViewInformesGenerados.EnableFiltering = true;
                    
                    this.radGridViewInformesGenerados.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);

                    this.radGridViewInformesGenerados.Rows[0].IsCurrent = true;
                    this.radGridViewInformesGenerados.Focus();
                    this.radGridViewInformesGenerados.Select();

                    this.radGridViewInformesGenerados.Refresh();
                }
                else
                {
                    this.radGridViewInformesGenerados.Visible = false;
                    this.radLabelNoHayInfo.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonDescargarInforme, false);
                    utiles.ButtonEnabled(ref this.radButtonEliminarInforme, false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewInformesGenerados.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewInformesGenerados.Columns.Contains(item.Key)) this.radGridViewInformesGenerados.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Verifica que el usuario logado tenga permisos para operar con el NRORP5 del informe
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        private bool UsuarioAutorizado(string IDINW8)
        {
            bool operarConsulta = false;

            try
            {
                string autClaseElemento = "010";
                string autGrupo = "01";
                string autOperConsulta = "10";

                operarConsulta = aut.Validar(autClaseElemento, autGrupo, IDINW8, autOperConsulta);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (operarConsulta);
        }

        private string ConvertirHora(string HORA)
        {
            string sHORA = "";
            HORA = HORA.PadLeft(6, '0');

            int iDig;
            for (iDig = 0; iDig < 6; iDig++)
            {
                sHORA = sHORA + HORA.Substring(iDig, 1);
                if ((iDig == 1) || (iDig == 3))
                {
                    sHORA += ":";
                }
            }
            return (sHORA);
        }

        /// <summary>
        /// Carga el formulario para solicitar la lista de informes
        /// </summary>
        //private void DescargarInformeGenerado(string sRUUN)
        private void DescargarInformeGenerado()
        {
            this.mensajeProceso = "";

            //Iniciar la barra de progreso
            this.backgroundWorker1.ReportProgress(0);

            //Mover la barra de progreso
            this.backgroundWorker1.ReportProgress(2);

            try
            {
                int indice = this.radGridViewInformesGenerados.Rows.IndexOf(this.radGridViewInformesGenerados.CurrentRow);
                string sRUUN = this.radGridViewInformesGenerados.Rows[indice].Cells["RUUNW8"].Value.ToString();

                string proveedorTipo = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

                IDataReader dr = null;
                string query;

                string sqlTmp = "";
                int espacioEXW09 = 21;
                int espacioEXW81 = 30;

                string sTIPDW8 = "";
                string sDESUW8 = "";
                string sTIPLW8 = "";
                string sFECHW8 = "";
                string sNCIAW8 = "";
                string sIDINW8 = "";
                string valor = "";
                string valorNum = "";

                int nColumnas = 0;

                //DateTime hoy = DateTime.Now;
                //string sFECHA = hoy.ToString("dd/MM/yyyy");

                //Cursor.Current = Cursors.WaitCursor;

                //Crear el fichero en memoria (Excel o HTML)
                this.InformeHTMLCrear(ref this.documento_HTML);

                //lectura de EXW08
                query = "SELECT * FROM " + GlobalVar.PrefijoTablaCG + "EXW08 WHERE RUUNW8 = '" + sRUUN + "'";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    sTIPDW8 = dr["TIPDW8"].ToString();
                    sDESUW8 = dr["DESUW8"].ToString();
                    sTIPLW8 = dr["TIPLW8"].ToString();
                    sFECHW8 = dr["FECHW8"].ToString();
                    sNCIAW8 = dr["NCIAW8"].ToString();
                    sIDINW8 = dr["IDINW8"].ToString();
                    nColumnas = sTIPDW8.Trim().Length;

                    if (dr["STATW8"].ToString().Trim() == "C")
                    {
                        dr.Close();
                        //Finaliza la barra de progreso
                        this.backgroundWorker1.ReportProgress(1);

                        this.mensajeProceso = "El informe está en curso. Espere a que esté terminado.";
                        return;
                    }
                }
                else
                {
                    dr.Close();
                    //Finaliza la barra de progreso
                    this.backgroundWorker1.ReportProgress(1);

                    this.mensajeProceso = "El informe no existe.";
                    return;
                }
                dr.Close();

                //Mover la barra de progreso
                this.backgroundWorker1.ReportProgress(2);

                //cabecera
                documento_HTML.Append("     <title>" + sDESUW8 + "</title>\n");
                documento_HTML.Append("     <style>\n");
                documento_HTML.Append("        .NumeroCG {mso-number-format:\\#\\,\\#\\#0\\.00;text-align=right;}\n");
                documento_HTML.Append("        .NumeroCGLeft {mso-number-format:\"0\";text-align:left; background-color:#D8D8D8}\n");
                documento_HTML.Append("        .NumeroCGSaldoIni {mso-number-format:\\#\\,\\#\\#0\\.00;text-align:right; background-color:#DBDBDB}\n");
                documento_HTML.Append(@"        .Texto    { mso-number-format:\@; }");
                documento_HTML.Append("\n");
                documento_HTML.Append(@"        .TextoTIT    { mso-number-format:\@;font-weight:700; background-color:#D8D8D8 }");
                documento_HTML.Append("\n");
                documento_HTML.Append(@"        .TextoTITCab    { mso-number-format:\@;font-weight:700; background-color:#C5D9F1}");
                documento_HTML.Append("\n");
                documento_HTML.Append(@"        .TextoTITSaldoIni { mso-number-format:\@;font-weight:700; background-color:#DBDBDB}");
                documento_HTML.Append("\n");
                documento_HTML.Append("     </style>\n");
                documento_HTML.Append(" </head>\n");

                //parte superior a cabecera
                //osubTit.Add 1, rbW8.Fields("NCIAW8"), "", "FECHA " & sFech      'rbW8.Fields("FECHW8")
                //osubTit.Add 2, "PLAN " & rbW8.Fields("TIPLW8"), rbW8.Fields("DESUW8"), "INFORME " & rbW8.Fields("IDINW8")
                documento_HTML.Append(" <body>\n");
                documento_HTML.Append("     <table width =\"100%\">\n");
                documento_HTML.Append("         <tr>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + sNCIAW8 + "</td>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + "&nbsp;" + " </td>\n");
                //documento_HTML.Append("             <td class=Texto width =\"40%\">" + "FECHA " + sFECHA + "</td>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + "FECHA " + utiles.FechaToFormatoCG(sFECHW8).ToShortDateString() + "</td>\n");
                documento_HTML.Append("         </tr>\n");
                documento_HTML.Append("         <tr>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + "PLAN " + sTIPLW8 + "</td>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + sDESUW8 + " </td>\n");
                documento_HTML.Append("             <td class=Texto width =\"40%\">" + "INFORME " + sIDINW8 + "</td>\n");
                documento_HTML.Append("         </tr>\n");
                documento_HTML.Append("     </table>\n");

                //lectura de EXW81
                sqlTmp = "SELECT RUUN81, NLIN81";
                for (int i = 0; i < nColumnas; i++)
                {
                    if (proveedorTipo == "Oracle")
                        sqlTmp = sqlTmp + ", SUBSTR (DESC81, 1+(" + espacioEXW81 + "*(" + i + ")), " + espacioEXW81 + ")";
                    else
                        sqlTmp = sqlTmp + ", SUBSTRING (DESC81, 1+(" + espacioEXW81 + "*(" + i + ")), " + espacioEXW81 + ")";
                }
                query = sqlTmp + " FROM " + GlobalVar.PrefijoTablaCG + "EXW81 WHERE RUUN81 = '" + sRUUN + "' ORDER BY NLIN81";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                while (dr.Read()) //siempre seran 4 filas
                {
                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);

                    //sDESC81 = dr["DESC81"].ToString();
                    //documento_HTML.Append(" <body>\n");
                    //documento_HTML.Append("     <b>" + fila1 + "</b><br>\n");

                    //documento_HTML.Append("     <b>" + descCompania + "</b>\n");

                    documento_HTML.Append("     <table width =\"100%\">\n");
                    documento_HTML.Append("         <tr>\n");
                    for (int i = 0; i < nColumnas; i++)
                    {
                        valor = (dr[i + 2].ToString() == "") ? "&nbsp;" : dr[i + 2].ToString();
                        documento_HTML.Append("             <td class=TextoTITCab width =\"40%\">" + valor + "</td>\n");
                    }
                    documento_HTML.Append("         </tr>\n");
                    documento_HTML.Append("     </table>\n");
                }
                dr.Close();

                //lectura de EXW09
                sqlTmp = "SELECT NLINW9, STATW9";
                for (int i = 0; i < nColumnas; i++)
                {
                    switch (sTIPDW8.Substring(i, 1).ToString())
                    {
                        case "M":
                            sqlTmp += ", CUENW9 ";
                            break;
                        case "A":
                            sqlTmp += ", CAUXW9 ";
                            break;
                        case "Z":
                            sqlTmp += ", ZONAW9 ";
                            break;
                        case "D":
                            sqlTmp += ", DESCW9 ";
                            break;
                        case "I":
                            if (proveedorTipo == "Oracle")
                                sqlTmp = sqlTmp + ", SUBSTR (IMPSW9, 1+(" + espacioEXW09 + "*(" + i + ")), " + espacioEXW09 + ")";
                            else
                                sqlTmp = sqlTmp + ", SUBSTRING (IMPSW9, 1+(" + espacioEXW09 + "*(" + i + ")), " + espacioEXW09 + ")";
                            break;
                        default:
                            break;
                    }
                }
                query = sqlTmp + " FROM " + GlobalVar.PrefijoTablaCG + "EXW09 WHERE RUUNW9 = '" + sRUUN + "' ORDER BY NLINW9";
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                while (dr.Read())
                {
                    //Mover la barra de progreso
                    this.backgroundWorker1.ReportProgress(2);

                    //sIMPSW9 = dr["IMPSW9"].ToString();
                    documento_HTML.Append("     <table width =\"100%\">\n");
                    documento_HTML.Append("         <tr>\n");
                    for (int i = 0; i < nColumnas; i++)
                    {
                        valor = (dr[i + 2].ToString() == "") ? "&nbsp;" : dr[i + 2].ToString();
                        if (sTIPDW8.Substring(i, 1).ToString() == "I")
                        {
                            if (valor == "&nbsp;")
                            {
                                if (dr["STATW9"].ToString() == "C")
                                {
                                    documento_HTML.Append("             <td class=TextoTIT width =\"40%\">" + valor + "</td>\n");
                                }
                                else
                                {
                                    documento_HTML.Append("             <td class=Texto width =\"40%\">" + valor + "</td>\n");
                                }
                            }
                            else
                            {
                                //para los negativos pasar el signo - del final al principio
                                if (valor.Trim().Contains("-"))
                                {
                                    valorNum = "-" + valor.Trim().Substring(0, valor.Trim().Length - 1);
                                }
                                else
                                {
                                    valorNum = valor;
                                }
                                documento_HTML.Append("             <td class=NumeroCG width =\"40%\">" + valorNum + "</td>\n");
                            }
                        }
                        else
                        {
                            if (dr["STATW9"].ToString() == "C")
                            {
                                documento_HTML.Append("             <td class=TextoTIT width =\"40%\">" + valor + "</td>\n");
                            }
                            else
                            {
                                documento_HTML.Append("             <td class=Texto width =\"40%\">" + valor + "</td>\n");
                            }
                        }
                    }
                    documento_HTML.Append("         </tr>\n");
                    documento_HTML.Append("     </table>\n");
                }
                dr.Close();

                this.InformeHTMLEscribirTagTable(ref this.documento_HTML, true);

                this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);

                string titulo = sRUUN + "_" + sIDINW8 + "_" + sDESUW8;
                //Generar el nombre de fichero
                this.nombreFicheroAGenerar = this.InformeNombreFichero(formCodeNombFichero, System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"], titulo);

                //Grabar el fichero de informe
                try
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(this.nombreFicheroAGenerar);
                    sw.WriteLine(this.documento_HTML.ToString());
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    this.mensajeProceso = "Error descargando el informe. Para más información consulte el fichero de Log.";
                }

                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);

                /*
                //levantar html con excel
                string REVISAR = "";
                string ficheroHTML = this.InformeNombreFichero(formCode, System.Configuration.ConfigurationManager.AppSettings["ModConsInfo_PathFicherosInformes"], REVISAR);

                try // tratar de levantar excel
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ficheroHTML);
                    sw.WriteLine(this.documento_HTML.ToString());
                    sw.Close();

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = tipoFichero, //coincide tipoFichero y nombre del .exe (excel)
                                                //startInfo.FileName = "EXCEL.EXE";
                        Arguments = "\"" + ficheroHTML + "\"" //entre barras y comillas para evitar problema de espacios en blanco en nombre fichero
                    };
                    Process.Start(startInfo);
                }
                catch // si no puede levantar excel, levantar html
                {
                    this.InformeHTMLEscribirAjustesGlobales(ref this.documento_HTML);
                }
                // fin levantar
                */
                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                this.mensajeProceso = "Error descargando el informe. Para más información consulte el fichero de Log.";

                //Finaliza la barra de progreso
                this.backgroundWorker1.ReportProgress(1);
            }
        }

        /*
        /// <summary>
        /// Verifica que el usuario logado tenga permisos para operar con el NRORP5 del informe
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        private bool UsuarioAutorizado(string IDINW8)
        {
            bool operarConsulta = false;

            try
            {
                string autClaseElemento = "010";
                string autGrupo = "01";
                string autOperConsulta = "10";

                operarConsulta = aut.Validar(autClaseElemento, autGrupo, IDINW8, autOperConsulta);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (operarConsulta);
        }

        private string ConvertirHora(string HORA)
        {
            string sHORA = "";
            HORA = HORA.PadLeft(6, '0');

            int iDig;
            for (iDig = 0; iDig < 6; iDig++)
            {
                sHORA += HORA.Substring(iDig, 1);
                if ((iDig == 1) || (iDig == 3))
                {
                    sHORA += ":";
                }
            }
            return (sHORA);
        }
        */

        /// <summary>
        /// Hace la llamada a descargar el informe solicitado o expande/collapsa la fila si es de tipo agrupacion
        /// </summary>
        private void DescargarInforme()
        {
            if (this.radGridViewInformesGenerados.SelectedRows.Count > 1)
            {
                RadMessageBox.Show("Debe seleccionar solo un informe", "Error");  //Falta traducir
                return;
            }

            if (this.radGridViewInformesGenerados.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewInformesGenerados.CurrentRow.IsExpanded) this.radGridViewInformesGenerados.CurrentRow.IsExpanded = false;
                else this.radGridViewInformesGenerados.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewInformesGenerados.CurrentRow is GridViewDataRowInfo)
            {
                backgroundWorker1.RunWorkerAsync();
                //int indice = this.radGridViewInformesGenerados.Rows.IndexOf(this.radGridViewInformesGenerados.CurrentRow);
                //string RUUNW8 = this.radGridViewInformesGenerados.Rows[indice].Cells["RUUNW8"].Value.ToString();
                //this.DescargarInformeGenerado(RUUNW8);
            }
        }

        /// <summary>
        /// Elimina los informes seleccionados
        /// </summary>
        private void EliminarInformes()
        {
            try
            {
                //Pedir confirmacion
                if (this.radGridViewInformesGenerados.SelectedRows.Count > 0)
                {
                    //Pedir confirmación
                    DialogResult result = RadMessageBox.Show("Se van a eliminar los informes seleccionados. ¿Desea continuar?", this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNoCancel);
                    if (result != DialogResult.Yes) return;


                    GridViewDataRowInfo[] rows = new GridViewDataRowInfo[this.radGridViewInformesGenerados.SelectedRows.Count];
                    this.radGridViewInformesGenerados.SelectedRows.CopyTo(rows, 0);

                    this.radGridViewInformesGenerados.BeginUpdate();

                    string RUUN;
                    bool deleteOK;
                    for (int i = 0; i < rows.Length; i++)
                    {
                        //Eliminar el informe seleccionado
                        RUUN = rows[i].Cells["RUUNW8"].Value.ToString();

                        try
                        {
                            deleteOK = this.EliminarInformeSel(RUUN);
                            if (deleteOK) this.radGridViewInformesGenerados.Rows.Remove(rows[i]);
                        }
                        catch (Exception ex) { GlobalVar.Log.Error(Utiles.CreateExceptionString(ex)); }
                    }

                    this.radGridViewInformesGenerados.EndUpdate();

                    if (this.radGridViewInformesGenerados.Rows.Count == 0)
                    {
                        this.radGridViewInformesGenerados.Visible = false;
                        this.radLabelNoHayInfo.Visible = true;
                        utiles.ButtonEnabled(ref this.radButtonDescargarInforme, false);
                        utiles.ButtonEnabled(ref this.radButtonEliminarInforme, false);
                    }

                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Elimina el informe indicado
        /// </summary>
        /// <param name="ruunInforme"></param>
        /// <returns></returns>
        private bool EliminarInformeSel(string ruunInforme)
        {
            bool result = false;

            try
            {
                //Eliminar de la tabla EXW08
                string query = "delete from " + GlobalVar.PrefijoTablaCG + "EXW08 ";
                query += "where RUUNW8 = '" + ruunInforme + "'";
                int reg;
                try
                {
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                    result = true;
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Eliminar de la tabla EXW09
                query = "delete from " + GlobalVar.PrefijoTablaCG + "EXW09 ";
                query += "where RUUNW9 = '" + ruunInforme + "'";
                try
                {
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Eliminar de la tabla EXW81
                query = "delete from " + GlobalVar.PrefijoTablaCG + "EXW81 ";
                query += "where RUUN81 = '" + ruunInforme + "'";
                try
                {
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                //Eliminar de la tabla EXW82
                query = "delete from " + GlobalVar.PrefijoTablaCG + "EXW82 ";
                query += "where RUUN82 = '" + ruunInforme + "'";
                try
                {
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
            
            return (result);
        }

        /// <summary>
        /// Actualiza el estado del identificador
        /// </summary>
        /// <param name="RUUNW8">identificador del informe</param>
        /// <param name="estado">estado nuevo del informe</param>
        /// <returns></returns>
        private bool ActualizarEstadoInforme(string RUUNW8, string estado)
        {
            bool result = false;

            try
            {
                //Actualizar el estado en la tabla  EXW08
                string query = "update " + GlobalVar.PrefijoTablaCG + "EXW08 set STATW8 = '" + estado + "' ";
                query += "where RUUNW8 = '" + RUUNW8 + "'";
                int reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (reg == 1) result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion
    }
}
