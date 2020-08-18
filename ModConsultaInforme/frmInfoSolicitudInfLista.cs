using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectModel;
using Telerik.WinControls;

namespace ModConsultaInforme
{
    public partial class frmInfoSolicitudInfLista : frmPlantilla, IReLocalizable
    {
        private string codGrupoInf = "";
        private string grupoInfDesc = "";

        private string aapp = "";

        Dictionary<string, string> displayNames;
        DataTable dtSolicitudInfoLista = new DataTable();

        public string CodGrupoInf
        {
            get
            {
                return (this.codGrupoInf);
            }
            set
            {
                this.codGrupoInf = value;
            }
        }

        public string GrupoInfDesc
        {
            get
            {
                return (this.grupoInfDesc);
            }
            set
            {
                this.grupoInfDesc = value;
            }
        }

        public frmInfoSolicitudInfLista()
        {
            InitializeComponent();

            this.FormElement.TitleBar.IconPrimitive.Alignment = ContentAlignment.MiddleCenter;
            this.FormElement.TitleBar.IconPrimitive.Margin = new Padding(3, 0, 0, 0);

            this.groupBox1.ElementTree.EnableApplicationThemeName = false;
            this.groupBox1.ThemeName = "ControlDefault";

            this.radGridViewInformeLista.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmInfoSolicitudInfLista_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Solicitar Informe Lista");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            this.BuildDisplayNames();

            //Crear el TGGrid
            this.BuildtgGridSolicitudInfLista();

            //this.show_chkBox();

            //Cargar los datos de la Grid
            this.FillDataGrid();
        }

        private void RadButtonEjecutarInforme_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEjecutarInforme);
        }

        private void RadButtonEjecutarInforme_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEjecutarInforme);
        }

        private void RadButtonExit_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonExit);
        }

        private void RadButtonExit_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonExit);
        }

        private void RadGridViewInformeLista_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            utiles.SelectUnselectAllRadGridViewRows(ref sender, ref this.radGridViewInformeLista, ref this.selectAll);
        }

        private void RadButtonEjecutarInforme_Click(object sender, EventArgs e)
        {
            this.Ejecutar();
        }

        private void RadButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmInfoSolicitudInfLista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) this.RadButtonExit_Click(sender, null);
        }

        private void TxtDiasPermanencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtDiasPermanencia, false, ref sender, ref e);
        }

        private void TxtNumPeriodosAno_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(0, ref this.txtNumPeriodosAno, false, ref sender, ref e);
        }

        /*
        void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerBox = ((CheckBox)tgGridInformeLista.Controls.Find("checkboxHeader", true)[0]);

            bool activo = true;

            for (int i = 0; i < tgGridInformeLista.RowCount; i++)
            {
                if (tgGridInformeLista.Rows[i].Cells[0].Value != null && (bool)tgGridInformeLista.Rows[i].Cells[0].Value == true)
                {
                    activo = false;
                    break;
                }
            }

            for (int i = 0; i < tgGridInformeLista.RowCount; i++)
            {
                tgGridInformeLista.Rows[i].Cells[0].Value = activo;
                //tgGridInformeLista.Rows[i].Cells[0].Value .Checked;
            }
        }
        */

        private void FrmInfoSolicitudInfLista_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Solicitar Informe Lista");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("subMenuItemSolicitudInfLista", "Lista de informes");
            this.Text = this.Text.Replace("&", "");

            if (this.codGrupoInf != "" || this.grupoInfDesc != "")
            {
                this.Text += " - " + this.codGrupoInf + " " + this.grupoInfDesc;
            }

            this.radButtonEjecutarInforme.Text = this.LP.GetText("lblEjecutarInforme", "Ejecutar informe");   //Falta traducir
            this.radButtonExit.Text = this.LP.GetText("lblSalir", "Salir");   //Falta traducir
        }

        /// <summary>
        /// Construir el control de la Grid que contiene las Solicitudes de grupo de informes
        /// </summary>
        private void BuildtgGridSolicitudInfLista()
        {
            //Adicionar las columnas para el DataTable para la Grid
            this.dtSolicitudInfoLista.Columns.Add("NRORP5", typeof(string));
            this.dtSolicitudInfoLista.Columns.Add("CODFP5", typeof(string));
            this.dtSolicitudInfoLista.Columns.Add("NOMBP5", typeof(string));
        }

        private void BuildDisplayNames()
        {
            try
            {
                this.displayNames = new Dictionary<string, string>
                {
                    { "NRORP5", this.LP.GetText("lblListaCampoSecuencia", "Secuencia") },
                    { "CODFP5", this.LP.GetText("lblListaCampoInforme", "Informe") },
                    { "NOMBP5", this.LP.GetText("lblListaCampoDesc", "Descripción") }
                };
            }
            catch { }
        }


        /// <summary>
        /// Carga los datos de los informes en la grid
        /// </summary>
        private void FillDataGrid()
        {
            try
            {
                string query = "select NRORP5, CODFP5, NOMBP5 from " + GlobalVar.PrefijoTablaCG + "PRT05 ";
                query += "where GRUPP5 = '" + this.codGrupoInf + "' ";
                query += "order by NRORP5";

                DataTable dtInformes = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                /*
                dtInformes.Columns.Add("activo", typeof(bool));

                for (int i = 0; i < dtInformes.Rows.Count; i++)
                {
                    dtInformes.Rows[i]["activo"] = false;
                }
                */
                string CODFP5 = "";

                DataRow rowDataTableGrid;

                foreach (DataRow dr in dtInformes.Rows)
                {
                    CODFP5 = dr["CODFP5"].ToString();

                    //if (this.UsuarioAutorizado(NRORP5))
                    if (this.UsuarioAutorizado(CODFP5))
                    {
                        rowDataTableGrid = this.dtSolicitudInfoLista.NewRow();
                        rowDataTableGrid["NRORP5"] = dr["NRORP5"].ToString().PadLeft(3, '0');
                        rowDataTableGrid["CODFP5"] = CODFP5.PadLeft(3, '0');
                        rowDataTableGrid["NOMBP5"] = dr["NOMBP5"].ToString();

                        this.dtSolicitudInfoLista.Rows.Add(rowDataTableGrid);
                    }
                }

                if (this.dtSolicitudInfoLista != null && this.dtSolicitudInfoLista.Rows != null && this.dtSolicitudInfoLista.Rows.Count > 0)
                {
                    this.radGridViewInformeLista.DataSource = this.dtSolicitudInfoLista;
                    this.radGridViewInformeLista.Visible = true;
                    this.RadGridViewHeader();
                    //this.radLabelNoHayInfo.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonEjecutarInforme, true);

                    for (int i = 0; i < this.radGridViewInformeLista.Columns.Count; i++)
                    {
                        this.radGridViewInformeLista.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    }

                    this.radGridViewInformeLista.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewInformeLista.MasterTemplate.BestFitColumns();
                }
                else
                {
                    this.radGridViewInformeLista.Visible = false;
                    //this.radLabelNoHayInfo.Visible = true;
                    utiles.ButtonEnabled(ref this.radButtonEjecutarInforme, false);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewHeader()
        {
            try
            {
                if (this.radGridViewInformeLista.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNames)
                    {
                        if (this.radGridViewInformeLista.Columns.Contains(item.Key)) this.radGridViewInformeLista.Columns[item.Key].HeaderText = item.Value;
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
        private bool UsuarioAutorizado(string CODFP5)
        {
            bool operarConsulta = false;

            try
            {
                //string autClaseElemento = "014";
                string autClaseElemento = "010";
                string autGrupo = "01";
                string autOperConsulta = "10";

                operarConsulta = aut.Validar(autClaseElemento, autGrupo, CODFP5, autOperConsulta);

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (operarConsulta);
        }

        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool ValidarForm()
        {
            bool result = true;
            string error = "";

            string aa = "";
            string pp = "";
            try
            {
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string aappAux = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                int aux;
                if (aappAux.Trim() == "")
                {
                    error = "El año-periodo no puede estar en blanco\n\r";
                    this.txtMaskAAPP.Focus();
                }
                else
                {
                    int pos = aappAux.IndexOf('-');
                    aa = aappAux.Substring(0, 2);
                    aa = aa.Trim();

                    if (aappAux.Length > 3)
                    {
                        pp = aappAux.Substring(3, aappAux.Length - 3);
                        pp = pp.Trim();
                    }

                    aux = Convert.ToInt16(pp);
                    if (aux <= 0)
                    {
                        error += "El periodo tiene que ser mayor que cero\n\r";   //Falta traducir
                        this.txtMaskAAPP.Focus();
                    }
                }

                string numPeriodos = this.txtNumPeriodosAno.Text.Trim();
                if (numPeriodos == "")
                {
                    error += "El número de periodos por año no puede estar en blanco\n\r";
                    this.txtNumPeriodosAno.Focus();
                }
                else
                {
                    aux = Convert.ToInt16(numPeriodos);
                    if (aux <= 0)
                    {
                        error += "El periodo tiene que ser mayor que cero\n\r";   //Falta traducir
                        this.txtNumPeriodosAno.Focus();
                    }
                }

                string diasPermanencia = this.txtDiasPermanencia.Text.Trim();
                if (diasPermanencia == "")
                {
                    error += "Los días de permanencia no pueden estar en blanco\n\r";
                    this.txtDiasPermanencia.Focus();
                }
                else
                {
                    aux = Convert.ToInt16(diasPermanencia);
                    if (aux <= 0)
                    {
                        error += "Los días de permanencia tienen que ser mayor que cero\n\r";   //Falta traducir
                        this.txtDiasPermanencia.Focus();
                    }
                }

                if (error != "")
                {
                    RadMessageBox.Show(error, this.LP.GetText("errValTitulo", "Error"));
                    result = false;
                }
                else
                {
                    this.aapp = aa.PadLeft(2, '0') + pp.PadLeft(2, '0');
                }

            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Obtiene la descripción para el programa solicitado que se busca en la tabla XPARA y se almacenará en la tabla XCONTRO
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerDescXContro(string programa)
        {
            string desc = "";
            IDataReader dr = null;
            try
            {
                string prefijoTabla = "";
                string tipoBaseDatosCG = System.Configuration.ConfigurationManager.AppSettings["tipoBaseDatosCG"];

                if (tipoBaseDatosCG == "DB2")
                {
                    prefijoTabla = System.Configuration.ConfigurationManager.AppSettings["bbddCGUF"];
                    if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                }

                string query = "select * from " + prefijoTabla + "XPROGRA ";
                query += "where PROGRAMA = '" + programa + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    desc = dr.GetValue(dr.GetOrdinal("DESCRI")).ToString().Trim();
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

        /// <summary>
        /// Obtiene la parte fija de la parte variable de la lda para el proceso de ejecutar informes e inserta en la XContro
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerLDAVariableFija(ref XContro xcontro)
        {
            string ldaVariable = "";
            try
            {
                ldaVariable = this.codGrupoInf.PadLeft(3, ' ');

                ldaVariable += aapp.PadLeft(4, '0');

                ldaVariable += this.txtNumPeriodosAno.Text.PadLeft(2, '0');
                ldaVariable += "1";  ///Solo Enfoc

                ldaVariable += this.txtDiasPermanencia.Text.PadLeft(3, '0');
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (ldaVariable);
        }

        /// <summary>
        /// Ejecutar el informe
        /// </summary>
        private void Ejecutar()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.ValidarForm())
                {
                    //Enviar a Ejecutar el informe
                    XContro xcontro = new XContro
                    {
                        ColaName = ColasName.CGJOBD,
                        Usuario = System.Environment.UserName.ToUpper(),
                        Prioridad = "20",
                        ProcTipo = "E",
                        ProcName = "CG012",

                        DescName = ObtenerDescXContro("EXP220")
                    };
                    if (xcontro.DescName == "") xcontro.DescName = this.LP.GetText("lblGenerarGrupoInfo", "Generación de un grupo de informes desde ENFOC");
                    xcontro.Status = "Pendiente";
                    xcontro.Pid = "0";
                    xcontro.PidMenu = "0";
                    xcontro.PidPanta = " ";

                    xcontro.LDA_BTCLDA = "EX220";

                    string ldaVariableFija = this.ObtenerLDAVariableFija(ref xcontro);

                    string ldaVariable = "";
                    string result = "";

                    string monedaAlternativaValor = "0";    //moneda local
                    if (this.chkMonedaAlternativa.Checked) monedaAlternativaValor = "1";    //moneda alternativa

                    //Chequear si se han seleccionado todos los informes o no
                    if (this.radGridViewInformeLista.Rows.Count == this.radGridViewInformeLista.SelectedRows.Count)          //Seleccionaron todos los informes
                    {
                        ldaVariable = ldaVariableFija;
                        ldaVariable += " ".PadLeft(6, ' ');
                        ldaVariable += " ";
                        ldaVariable += monedaAlternativaValor;
                        xcontro.LDA_Variable = ldaVariable;

                        xcontro.Parm = "@[PARA]";

                        result = xcontro.Insertar();
                    }
                    else
                    {
                        for (int i = 0; i < this.radGridViewInformeLista.SelectedRows.Count; i++)
                        {
                            ldaVariable = ldaVariableFija;

                            ldaVariable += this.radGridViewInformeLista.SelectedRows[i].Cells["CODFP5"].Value.ToString().PadLeft(3, ' ');
                            ldaVariable += this.radGridViewInformeLista.SelectedRows[i].Cells["NRORP5"].Value.ToString().PadLeft(3, '0');

                            ldaVariable += " ";
                            ldaVariable += monedaAlternativaValor;

                            xcontro.LDA_Variable = ldaVariable;

                            xcontro.Parm = "@[PARA]";

                            result = xcontro.Insertar();
                        }
                    }

                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /*
        private void show_chkBox()
        {
            Rectangle rect = this.tgGridInformeLista.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position 
            rect.Y = 3;
            rect.X = rect.Location.X + (rect.Width / 4);
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            //datagridview[0, 0].ToolTipText = "sdfsdf";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            this.tgGridInformeLista.Controls.Add(checkboxHeader);
        }
        */
        #endregion
    }
}
