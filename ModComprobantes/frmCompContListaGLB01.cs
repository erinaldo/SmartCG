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
    public partial class frmCompContListaGLB01 :  frmPlantilla, IReLocalizable
    {
        private string tipoBaseDatosCG = "";
        private string codCompania = "";
        private string codTipo = "";

        bool periodoCerrado = false;

        Dictionary<string, string> displayNamesComprobantes;
        private DataTable dtComprobantes;

        Dictionary<string, string> displayNamesVerHistorial;
        private DataTable dtVerHistorial;

        Dictionary<string, string> displayNamesAccionesActuales;
        private DataTable dtAccionesActuales;

        public string Saldo_local = "";
        public string Saldo_extran = "";

        public frmCompContListaGLB01()
        {
            InitializeComponent();

            this.gbCabecera.ElementTree.EnableApplicationThemeName = false;
            this.gbCabecera.ThemeName = "ControlDefault";

            this.radGridViewAccionesActuales.MasterView.TableSearchRow.IsVisible = false;
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContListaGLB01_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Lista de comprobantes");
            
            //Tipo de Base de Datos 
            tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];

            //Cargar compañías
            this.FillCompanias();

            //Cargar Tipos
            this.FillTiposComprobantes();

            //Cargar el desplegable de estados
            this.FillcmbEstado();

            //Cargar el desplegable de modos de trabajo
            this.FillcmbModoTrabajo();

            //Crear el DataGrid
            this.CrearDataGrid();

            //Crear el DataGrid para Visualizar el Historial del Comprobante
            this.CrearDataGridVisorHistorial();

            //Crear el DataGrid para las Acciones que va realizando el usuario
            this.CrearDataGridAccionesActuales();

            //Traducir los literales
            this.TraducirLiterales();
            
            //Inicializar la Barra de Progreso
            this.InitProgressBar();

            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
            utiles.ButtonEnabled(ref this.radButtonVerAccionesActuales, false);
            utiles.ButtonEnabled(ref this.radButtonVerHistorial, false);

            this.gbCabecera.Focus();
            this.cmbCompania.Select();
            this.btnSel.BringToFront();
            //Ocultar el botón de selección de la Grid
            this.btnSel.Visible = false;
        }

        private void TxtNoComprobante_KeyPress(object sender, KeyPressEventArgs e)
        {
            utiles.ValidarNumeroConDecimalesKeyPress(2, ref this.txtNoComprobante, false, ref sender, ref e);
        }

        private void DgComprobantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.EditarComprobante();

                /*
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                DataGridViewRow row = this.dgComprobantes.SelectedRows[0];

                this.CargarComprobanteSeleccionado(row);

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
                 */
            }
        }

        private void RadButtonNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;
                // lista de comprobantes?

                frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita
                {
                    NuevoComprobante = true,
                    NuevoComprobanteGLB01 = true,
                    EdicionComprobanteGLB01 = false,
                    Batch = false,
                    FrmPadre = this
                };
                frmCompCont.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
                frmCompCont.Show();

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            // lista de copmprobantes

            this.EditarComprobante();
        }

        private void RadButtonSuprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                string estado = "";
                string mensaje = "";
                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (estado != "R")
                    {
                        mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "D");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                        this.OcultarGridHistorial_Acciones();

                        //Cambia el estado del comprobante (lo pasa a Aprobado)
                        this.CambiarEstado(this.radGridViewComprobantes.SelectedRows[i], i, "", "1");

                        //Insertar la acción de Eliminar en la Grid de Acciones
                        this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "D", estado);
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonVerAccionesActuales_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewAccionesActuales.Visible)
            {
                //Oculta el Grid de Acciones Actuales
                this.OcultarGridHistorial_Acciones();
            }
            else
                try
                {
                    if (this.dtAccionesActuales.Rows.Count > 0)
                    {
                        //Ocultar la Grid del Visor del Historial
                        if (this.radGridViewVisorHistorial.Visible) this.OcultarGridHistorial_Acciones();

                        //Mostrar el Grid del Visor del Historial
                        this.ViewGridAccionesActuales();
                    }
                    else
                    {
                        RadMessageBox.Show(this.LP.GetText("errAccionesActuales", "No se han realizado acciones sobre los comprobantes"), "");
                    }
                }
                catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }
        private void BtnSel_Click(object sender, EventArgs e)
        {
        }
        private void BtnSelPosicion(TGGrid tgGridDetalles)
        {
        }
            private void RadButtonVerHistorial_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            if (this.radGridViewVisorHistorial.Visible)
            {
                //Oculta el Grid del Visor del Historial
                this.OcultarGridHistorial_Acciones();
            }
            else
            {
                if (this.radGridViewComprobantes.SelectedRows.Count == 1)
                {
                    try
                    {
                        //Ocultar la Grid de Acciones Actuales
                        if (this.radGridViewAccionesActuales.Visible) this.OcultarGridHistorial_Acciones();

                        GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];

                        //Carga la grid con la información del historial del comprobante
                        this.CargarInfoVisorHistorial(row);

                        if (this.radGridViewVisorHistorial.Rows.Count > 0)
                        {
                            //Mostrar el Grid del Visor del Historial
                            this.ViewGridVisorHistorial();
                        }
                        else
                        {
                            RadMessageBox.Show(this.LP.GetText("errVisorHistorial", "No se ha encontrado historial para el comprobante seleccionado"), "");
                        }

                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }


                }
                else
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrSelSoloUnComp", "Debe seleccionar un solo comprobante"), this.LP.GetText("errValTitulo", "Error"));
                }
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;
                
                //Chequear autorización sobre compañía
                bool autorizadoAprobar = aut.Validar("002", "04", this.codCompania, "10");
                if (!autorizadoAprobar)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobComp", "Usuario no autorizado a aprobar comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                    return;
                }

                if (this.codTipo != "")
                {
                    //Chequear autorización sobre tipo de comprobante
                    autorizadoAprobar = aut.Validar("004", "02", this.codTipo, "10");
                    if (!autorizadoAprobar)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobTipo", "Usuario no autorizado a aprobar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        return;
                    }
                }

                string estado = "";
                string mensaje = "";
                string tipoComp = "";
                string tipoCompAct = "";
                
                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (!(estado == "V" || estado == "R"))
                    {
                        mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "A");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Validar Cuadre
                        bool comprobanteCuadra = ValidarComprobanteCuadra(i,"A");
                        if (!comprobanteCuadra) return; // no cuadra
                        

                        //Chequear autorización sobre tipo de comprobante

                        tipoCompAct = this.radGridViewComprobantes.SelectedRows[i].Cells["TIPO"].Value.ToString();

                        if (this.codTipo != "" && tipoCompAct != tipoComp)
                        {
                            autorizadoAprobar = aut.Validar("004", "02", tipoCompAct, "10");
                            tipoComp = tipoCompAct;
                        }
                        else autorizadoAprobar = true;
                        
                        
                        //if (autorizadoAprobar)
                        if (autorizadoAprobar)
                        {
                            //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                            this.OcultarGridHistorial_Acciones();

                            //Cambia el estado del comprobante (lo pasa a Aprobado)
                            this.CambiarEstado(this.radGridViewComprobantes.SelectedRows[i], i, "A", "");

                            //Insertar la acción de Aprobar en la Grid de Acciones
                            this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "A", estado);
                        }
                        else
                        {
                            RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutTipoCompAprob", "Usuario no autorizado a aprobar comprobantes de tipo ") + tipoCompAct, this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                        }
                        
                        
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }
        private bool ValidarComprobanteCuadra(int i, string tipo)
        {
            
            // grid comprobantes

            bool result = true;
            string sapric = "";
            string cia = "";
            string noComp = "";
            string clase = "";
            string tipComp = "";
            string accion = "";
            if (tipo == "A") accion = "Aprobar";
            if (tipo == "AC") accion = "Aprobar y  Contabilizar";
            if (tipo == "C") accion = "Contabilizar";

            //monedaLocalDebe = this.radGridViewComprobantes.SelectedRows[i].Cells["DebeML"].Value.ToString();

            try
            {
                sapric = this.radGridViewComprobantes.SelectedRows[i].Cells["SAPRIC"].Value.ToString();
                cia = this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString();
                tipComp= this.radGridViewComprobantes.SelectedRows[i].Cells["TIPO"].Value.ToString();
                noComp = this.radGridViewComprobantes.SelectedRows[i].Cells["noComp"].Value.ToString();
                clase = this.radGridViewComprobantes.SelectedRows[i].Cells["clase"].Value.ToString();

                string mensaje = "";
                string[] saldos = this.CabeceraSumaImportes(sapric, cia, noComp, clase, tipComp);
                
                if (saldos[0] != "0,00")
                {
                    mensaje += "- Moneda Local no cuadra por:" + saldos[0] + " \n\r";      //Falta traducir
                    result = false;
                }
                
                if (saldos[1] != "0,00")
                {
                    mensaje += "- Moneda Extranjera no cuadra por:" + saldos[1] + " \n\r";      //Falta traducir
                    result = false;
                }
                if (!result)
                {
                    mensaje += "- No se puede " + accion + " un comprobante descuadrado \n\r";      //Falta traducir
                    RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = false;
            }
            
            return (result);
        }
        private string[] CabeceraSumaImportes(string sapric, string cia, string noComp, string clase, string tipComp)
        {
            string[] result = new string[2];
            //Documento Cuadrado
            //Total debe y total haber (sumando las 3 lineas) tiene q dar 0

            IDataReader dr = null;
            try
            {
                string nombreTabla = GlobalVar.PrefijoTablaCG + "GLI03";
                string query = "select sum( DEBEIC + HABEIC ) as local, sum( DEMEIC + HAMEIC ) as extran FROM " + nombreTabla;
                query += " where CCIAIC = '" + cia + "' and SAPRIC = " + sapric + " and TICOIC = '" + tipComp + "' and NUCOIC = " + noComp;

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    result[0] = dr["local"].ToString().Trim();
                    string kk1 = dr["local"].ToString().Trim(); // quitar jl
                    result[1] = dr["extran"].ToString().Trim();
                    string kk = dr["extran"].ToString().Trim(); // quitar jl
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        private void RadButtonRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                string estado = "";
                string mensaje = "";
                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (estado != "A" && estado != "V")
                    {
                        mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "R");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                        this.OcultarGridHistorial_Acciones();

                        //Cambia el estado del comprobante (lo pasa a Aprobado)
                        this.CambiarEstado(this.radGridViewComprobantes.SelectedRows[i], i, "R", "");

                        //Insertar la acción de Rechazar en la Grid de Acciones
                        this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "R", estado);
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonContabilizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                //Chequear autorización sobre contabilizar compañía
                bool autorizadoContabilizar = aut.Validar("002", "02", this.codCompania, "30");
                if (!autorizadoContabilizar)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobComp", "Usuario no autorizado a aprobar comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                    return;
                }

                if (this.codTipo != "")
                {
                    //Chequear autorización sobre contabilizar tipo de comprobante
                    autorizadoContabilizar = aut.Validar("004", "03", this.codTipo, "30");
                    if (!autorizadoContabilizar)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompContabTipo", "Usuario no autorizado a contabilizar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        return;
                    }
                }

                string estado = "";
                string mensaje = "";
                string plan = "";
                string tipoComp = "";
                string tipoCompAct = "";

                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (!(estado == "A"))
                    {
                        mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "AC");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Chequear autorización sobre tipo de comprobante
                        tipoCompAct = this.radGridViewComprobantes.SelectedRows[i].Cells["TIPO"].Value.ToString();

                        if (this.codTipo != "" && tipoCompAct != tipoComp)
                        {
                            autorizadoContabilizar = aut.Validar("004", "03", tipoCompAct, "30");

                            tipoComp = tipoCompAct;
                        }
                        else
                        {
                            autorizadoContabilizar = true;
                        }
                        //Validar Cuadre
                        bool comprobanteCuadra = ValidarComprobanteCuadra(i, "C");

                        if (autorizadoContabilizar && comprobanteCuadra)
                        {
                            if (estado != "A")
                            {
                                //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                                this.OcultarGridHistorial_Acciones();

                                
                                if (i == 0)
                                {
                                    //Buscar el plan de la compañía
                                    string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString());
                                    plan = datosCompAct[1];
                                }

                                //Enviarlo a Contabilizar
                                XContro xcontro = new XContro();
                                InicializaXContro(ref xcontro, "C", this.radGridViewComprobantes.SelectedRows[i], plan);
                                string result = xcontro.Insertar();

                                //Insertar la acción de Aprobar + Contabilizar en la Grid de Acciones
                                this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "C", estado);
                            }
                            else
                            {
                                //Chequear que no se haya enviado ya a contabilizar
                                if (this.AccionNoRealizada(this.radGridViewComprobantes.SelectedRows[i], "C"))
                                {
                                    //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                                    this.OcultarGridHistorial_Acciones();

                                    if (i == 0)
                                    {
                                        //Buscar el plan de la compañía
                                        string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString());
                                        plan = datosCompAct[1];
                                    }

                                    //Enviarlo a Contabilizar
                                    XContro xcontro = new XContro();
                                    InicializaXContro(ref xcontro, "C", this.radGridViewComprobantes.SelectedRows[i], plan);
                                    string result = xcontro.Insertar();

                                    //Insertar la acción de Aprobar + Contabilizar en la Grid de Acciones
                                    this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "C", estado);
                                }
                                else
                                {
                                    RadMessageBox.Show(this.LP.GetText("errAccionYaEjecutada", "Acción previamente ejecutada"), this.LP.GetText("errValTitulo", "Error"));
                                }
                            }
                        }
                        else
                        {
                            if (!autorizadoContabilizar) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutTipoCompCont", "Usuario no autorizado a contabilizar comprobantes de tipo ") + tipoCompAct, this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                        }
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonAprobarContabilizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                //Chequear autorización sobre aprobar compañía
                bool autorizadoAprobar = aut.Validar("002", "04", this.codCompania, "10");
                if (!autorizadoAprobar)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobComp", "Usuario no autorizado a aprobar comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                    return;
                }

                //Chequear autorización sobre contabilizar compañía
                bool autorizadoContabilizar = aut.Validar("002", "02", this.codCompania, "30");
                if (!autorizadoContabilizar)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobComp", "Usuario no autorizado a aprobar comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                    return;
                }

                if (this.codTipo != "")
                {
                    //Chequear autorización sobre aprobar tipo de comprobante
                    autorizadoAprobar = aut.Validar("004", "02", this.codTipo, "10");
                    if (!autorizadoAprobar)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobTipo", "Usuario no autorizado a aprobar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        return;
                    }

                    //Chequear autorización sobre contabilizar tipo de comprobante
                    autorizadoContabilizar = aut.Validar("004", "03", this.codTipo, "30");
                    if (!autorizadoContabilizar)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompContabTipo", "Usuario no autorizado a contabilizar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        return;
                    }
                }

                string estado = "";
                string mensaje = "";
                string plan = "";
                string tipoComp = "";
                string tipoCompAct = "";

                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (!(estado == "A" || estado == "R" || estado == "V"))
                    {
                        mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "AC");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Chequear autorización sobre tipo de comprobante
                        tipoCompAct = this.radGridViewComprobantes.SelectedRows[i].Cells["TIPO"].Value.ToString();

                        if (this.codTipo != "" && tipoCompAct != tipoComp)
                        {
                            autorizadoAprobar = aut.Validar("004", "02", tipoCompAct, "10");
                            autorizadoContabilizar = aut.Validar("004", "03", tipoCompAct, "30");

                            tipoComp = tipoCompAct;
                        }
                        else
                        {
                            autorizadoAprobar = true;
                            autorizadoContabilizar = true;
                        }
                        //Validar Cuadre
                        bool comprobanteCuadra = ValidarComprobanteCuadra(i, "AC");
                         
                        if (autorizadoAprobar && autorizadoContabilizar && comprobanteCuadra)
                        {
                            if (estado != "A")
                            {
                                //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                                this.OcultarGridHistorial_Acciones();

                                //Cambia el estado del comprobante (lo pasa a Aprobado)
                                this.CambiarEstado(this.radGridViewComprobantes.SelectedRows[i], i, "A", "");

                                if (i == 0)
                                {
                                    //Buscar el plan de la compañía
                                    string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString());
                                    plan = datosCompAct[1];
                                }

                                //Enviarlo a Contabilizar
                                XContro xcontro = new XContro();
                                InicializaXContro(ref xcontro, "C", this.radGridViewComprobantes.SelectedRows[i], plan);
                                string result = xcontro.Insertar();

                                //Insertar la acción de Aprobar + Contabilizar en la Grid de Acciones
                                this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "AC", estado);
                            }
                            else
                            {
                                //Chequear que no se haya enviado ya a contabilizar
                                if (this.AccionNoRealizada(this.radGridViewComprobantes.SelectedRows[i], "AC"))
                                {
                                    //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                                    this.OcultarGridHistorial_Acciones();

                                    if (i == 0)
                                    {
                                        //Buscar el plan de la compañía
                                        string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString());
                                        plan = datosCompAct[1];
                                    }

                                    //Enviarlo a Contabilizar
                                    XContro xcontro = new XContro();
                                    InicializaXContro(ref xcontro, "C", this.radGridViewComprobantes.SelectedRows[i], plan);
                                    string result = xcontro.Insertar();

                                    //Insertar la acción de Aprobar + Contabilizar en la Grid de Acciones
                                    this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "AC", estado);
                                }
                                else
                                {
                                    RadMessageBox.Show(this.LP.GetText("errAccionYaEjecutada", "Acción previamente ejecutada"), this.LP.GetText("errValTitulo", "Error"));
                                }
                            }
                        }
                        else
                        {
                            if (!autorizadoAprobar && !autorizadoContabilizar) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompAprobContTipo", "Usuario no autorizado a aprobar ni a contabilizar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                            else if (!autorizadoAprobar) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutTipoCompAprob", "Usuario no autorizado a aprobar comprobantes de tipo ") + tipoCompAct, this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                            else if (!autorizadoContabilizar) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutTipoCompCont", "Usuario no autorizado a contabilizar comprobantes de tipo ") + tipoCompAct, this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                        }
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonRevertir_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                //Chequear autorización sobre compañía
                bool autorizadoRevertir = aut.Validar("002", "02", this.codCompania, "60");
                if (!autorizadoRevertir)
                {
                    RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompRevComp", "Usuario no autorizado a revertir comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                    return;
                }

                if (this.codTipo != "")
                {
                    //Chequear autorización sobre tipo de comprobante
                    autorizadoRevertir = aut.Validar("004", "03", this.codTipo, "40");
                    if (!autorizadoRevertir)
                    {
                        RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutCompRevTipo", "Usuario no autorizado a revertir comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        return;
                    }
                }

                string estado = "";
                string mensaje = "";
                string plan = "";
                string tipoComp = "";
                string tipoCompAct = "";

                for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                {
                    estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();

                    if (estado != "E")
                    {
                        mensaje = mensaje = this.AccionNoPermitidaMsg(this.radGridViewComprobantes.SelectedRows[i], "RE");
                        RadMessageBox.Show(mensaje, this.LP.GetText("errValTitulo", "Error"));
                    }
                    else
                    {
                        //Chequear autorización sobre tipo de comprobante
                        tipoCompAct = this.radGridViewComprobantes.SelectedRows[i].Cells["TIPO"].Value.ToString();

                        if (this.codTipo != "" && tipoCompAct != tipoComp)
                        {
                            autorizadoRevertir = aut.Validar("004", "03", tipoCompAct, "40");
                            tipoComp = tipoCompAct;
                        }
                        else autorizadoRevertir = true;

                        if (autorizadoRevertir)
                        {
                            //Chequear que no se haya enviado ya a revertir
                            if (this.AccionNoRealizada(this.radGridViewComprobantes.SelectedRows[i], "RE"))
                            {
                                //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                                this.OcultarGridHistorial_Acciones();

                                if (i == 0)
                                {
                                    //Buscar el plan de la compañía
                                    string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(this.radGridViewComprobantes.SelectedRows[i].Cells["compania"].Value.ToString());
                                    plan = datosCompAct[1];
                                }

                                //Enviarlo a Revertir
                                XContro xcontro = new XContro();
                                InicializaXContro(ref xcontro, "RE", this.radGridViewComprobantes.SelectedRows[i], plan);
                                string result = xcontro.Insertar();

                                //Insertar la acción de Revertir en la Grid de Acciones
                                this.InsertarGridAcciones(this.radGridViewComprobantes.SelectedRows[i], "RE", estado);
                            }
                            else
                            {
                                RadMessageBox.Show(this.LP.GetText("errAccionYaEjecutada", "Acción previamente ejecutada"), this.LP.GetText("errValTitulo", "Error"));
                            }
                        }
                        else
                        {
                            RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutTipoCompRev", "Usuario no autorizado a revertir comprobantes de tipo ") + tipoCompAct, this.LP.GetText("errValTitulo", "Error"));   //Falta traducir
                        }
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                if (this.radGridViewComprobantes.Rows.Count > 0)
                {
                    //Ocultar los grids de Visor del Historial y de Acciones si están visibles y agrandar el grid de comprobantes
                    this.OcultarGridHistorial_Acciones();

                    for (int i = 0; i < this.radGridViewComprobantes.Rows.Count; i++)
                    {
                        //Actualiza las columnas no claves de los comprobantes que están cargados
                        this.ActualizarInfoGridComprobantes(this.radGridViewComprobantes.Rows[i], i);
                    }

                    this.radGridViewComprobantes.Refresh();

                    this.radGridViewComprobantes.BringToFront();

                    //Refrescar las acciones posibles
                    string estado = "";
                    if (this.radGridViewComprobantes.SelectedRows.Count == 1)
                    {
                        GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];
                        estado = row.Cells["STATIC"].Value.ToString();
                        this.AccionesPosibles(estado);
                    }
                    else
                    {
                        for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                        {
                            estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();
                            this.AccionesPosiblesActivar(estado);
                        }
                    }
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadButtonBuscar_Click(object sender, EventArgs e)
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
                this.radPanelAccionesComp.Visible = false;

                utiles.ButtonEnabled(ref this.radButtonEditar, false);
                utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
                utiles.ButtonEnabled(ref this.radButtonAprobar, false);
                utiles.ButtonEnabled(ref this.radButtonRechazar, false);
                utiles.ButtonEnabled(ref this.radButtonContabilizar, false);
                utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, false);
                utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                utiles.ButtonEnabled(ref this.radButtonVerAccionesActuales, false);
                utiles.ButtonEnabled(ref this.radButtonVerHistorial, false);
                utiles.ButtonEnabled(ref this.radButtonActualizar, false);

                //Oculta el Grid de Acciones Actuales o del Visor del Historial si estuvieran visibles
                this.OcultarGridHistorial_Acciones();

                string codigo = this.cmbCompania.Text.Trim();
                if (codigo.Length > 2) codigo = codigo.Substring(0, 2);

                string query = "select CCIAIC, SAPRIC, TICOIC, NUCOIC, FECOIC, TVOUIC, DEBEIC, HABEIC, DEMEIC, HAMEIC, TASCIC, SIMIIC, STATIC, ";

                switch (tipoBaseDatosCG)
                {
                    case "DB2":
                        query += "CASE WHEN (select COHEAD from GLAI3 A  where RRN(A) in (select MIN(RRN(B)) from GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD ";
                        query += "AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD AND B.CCIAAD = CCIAIC AND B.SAPRAD = SAPRIC AND B.TICOAD = TICOIC AND B.NUCOAD = NUCOIC group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD)) <> ' ' THEN(";
                        query += "select COHEAD FROM GLAI3 A where RRN(A) in (select MIN(RRN(B)) from GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD ";
                        query += "group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD) AND A.CCIAAD = CCIAIC AND A.SAPRAD = SAPRIC AND A.TICOAD = TICOIC AND A.NUCOAD = NUCOIC) ELSE ' ' END COHEAD ";
                        break;
                    case "SQLServer":
                        query += "CASE WHEN (select COHEAD from " + GlobalVar.PrefijoTablaCG + "GLAI3 A  where A.GERIDENTI in (select MIN(B.GERIDENTI) from " + GlobalVar.PrefijoTablaCG + "GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD ";
                        query += "AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD AND B.CCIAAD = CCIAIC AND B.SAPRAD = SAPRIC AND B.TICOAD = TICOIC AND B.NUCOAD = NUCOIC group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD)) <> ' ' THEN(";
                        query += "select COHEAD FROM " + GlobalVar.PrefijoTablaCG + "GLAI3 A where A.GERIDENTI in (select MIN(B.GERIDENTI) from " + GlobalVar.PrefijoTablaCG + "GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD ";
                        query += "group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD) AND A.CCIAAD = CCIAIC AND A.SAPRAD = SAPRIC AND A.TICOAD = TICOIC AND A.NUCOAD = NUCOIC) ELSE ' ' END COHEAD ";
                        break;
                    case "Oracle":
                        query += "CASE WHEN (select COHEAD from " + GlobalVar.PrefijoTablaCG + "GLAI3 A  where A.ID_TIGSA_GLAI3 in (select MIN(B.ID_TIGSA_GLAI3) from " + GlobalVar.PrefijoTablaCG + "GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD ";
                        query += "AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD AND B.CCIAAD = CCIAIC AND B.SAPRAD = SAPRIC AND B.TICOAD = TICOIC AND B.NUCOAD = NUCOIC group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD)) <> ' ' THEN(";
                        query += "select COHEAD FROM " + GlobalVar.PrefijoTablaCG + "GLAI3 A where A.ID_TIGSA_GLAI3 in (select MIN(B.ID_TIGSA_GLAI3) from " + GlobalVar.PrefijoTablaCG + "GLAI3 B WHERE B.CCIAAD = A.CCIAAD AND B.SAPRAD = A.SAPRAD AND B.TICOAD = A.TICOAD AND B.NUCOAD = A.NUCOAD ";
                        query += "group by B.CCIAAD, B.SAPRAD, B.TICOAD, B.NUCOAD) AND A.CCIAAD = CCIAIC AND A.SAPRAD = SAPRIC AND A.TICOAD = TICOIC AND A.NUCOAD = NUCOIC) ELSE ' ' END COHEAD ";
                        break;
                }

                query += "from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "inner join " + GlobalVar.PrefijoTablaCG + "GLT06 on TICOIC =TIVOTV ";
                query += "where CCIAIC ='" + codigo + "'";

                if (sap != "")
                {
                    //Coger el campo año período con siglo 
                    string aaCampoAP = sap.Substring(0, 2);
                    query += " and SAPRIC =" + utiles.SigloDadoAnno(aaCampoAP, CGParametrosGrles.GLC01_ALSIRC) + sap;
                }

                string tipo = this.cmbTipo.Text.Trim();
                if (tipo != "")
                {
                    if (tipo.Length > 2) tipo = tipo.Substring(0, 2);
                    query += " and TICOIC =" + tipo;
                }

                if (this.cmbEstado.SelectedValue.ToString() != " ")
                {
                    query += " and STATIC = '" + this.cmbEstado.SelectedValue.ToString() + "'";
                }

                //if (this.cmbModoTrabajo.SelectedValue.ToString() != "3")
                //{
                //  query += " and TVOUIC = '" + this.cmbModoTrabajo.SelectedValue.ToString() + "'";
                //}

                if (this.cmbModoTrabajo.SelectedValue.ToString() != "2")
                {
                    // query += " and TVOUIC = '" + this.cmbModoTrabajo.SelectedValue.ToString() + "'"; JL
                    query += " and CODITV = '" + this.cmbModoTrabajo.SelectedValue.ToString() + "'";
                }

                if (this.txtNoComprobante.Text.Trim() != "")
                {
                    query += " and NUCOIC =" + this.txtNoComprobante.Text;
                }

                query += " order by CCIAIC, SAPRIC, TICOIC, NUCOIC ";

                IDataReader dr = null;
                try
                {
                    dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                    if (this.radGridViewComprobantes.Rows.Count > 0) this.dtComprobantes.Rows.Clear();

                    string compania;
                    string sigloanoper;
                    string numero;
                    string aappAux;
                    //string aux;

                    int cantComp = 0;
                    DataRow rowComprobante;

                    while (dr.Read())
                    {
                        if (cantComp == 0)
                        {
                            this.progressBarEspera.Value = 0;
                            this.progressBarEspera.Visible = true;

                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.Refresh();
                        }
                        else
                        {
                            //Mover la barra de progreso
                            if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                            else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                            this.progressBarEspera.Refresh();
                        }

                        rowComprobante = this.dtComprobantes.NewRow();

                        compania = dr["CCIAIC"].ToString().Trim();
                        rowComprobante["compania"] = compania;

                        sigloanoper = dr["SAPRIC"].ToString().Trim();

                        tipo = dr["TICOIC"].ToString().Trim();
                        if (tipo.Length == 1) tipo = "0" + tipo;
                        rowComprobante["Tipo"] = tipo;

                        numero = dr["NUCOIC"].ToString().Trim();
                        rowComprobante["NoComp"] = numero;

                        aappAux = sigloanoper;
                        if (sigloanoper.Length == 5) aappAux = sigloanoper.Substring(1, 4);
                        aappAux = aappAux.Substring(0, 2) + "-" + aappAux.Substring(2, 2);
                        rowComprobante["AAPP"] = aappAux;

                        rowComprobante["Fecha"] = utiles.FechaToFormatoCG(dr["FECOIC"].ToString()).ToShortDateString();
                        rowComprobante["Clase"] = dr["TVOUIC"].ToString().Trim();
                        rowComprobante["DebeML"] = dr["DEBEIC"].ToString().Trim();
                        rowComprobante["HaberML"] = dr["HABEIC"].ToString().Trim();
                        rowComprobante["DebeME"] = dr["DEMEIC"].ToString().Trim();
                        rowComprobante["HaberME"] = dr["HAMEIC"].ToString().Trim();
                        rowComprobante["Estado"] = this.ObtenerEstadoComprobante(dr["STATIC"].ToString()).Trim();

                        //aux = this.ObtenerDescripcion(compania, sigloanoper, tipo, numero);
                        //aux = aux.Trim();
                        //rowComprobante["descripcion"] = aux;
                        rowComprobante["descripcion"] = dr["COHEAD"].ToString().Trim();
                        rowComprobante["Tasa"] = dr["TASCIC"].ToString().Trim();
                        rowComprobante["NoMovimiento"] = dr["SIMIIC"].ToString().Trim();
                        rowComprobante["FECOIC"] = dr["FECOIC"].ToString().Trim();
                        rowComprobante["SAPRIC"] = dr["SAPRIC"].ToString().Trim();
                        rowComprobante["STATIC"] = dr["STATIC"].ToString().Trim();

                        this.dtComprobantes.Rows.Add(rowComprobante);

                        cantComp++;
                    }

                    dr.Close();

                    if (cantComp > 0)
                    {
                        //Verificar si el AAPP para la companía esta cerrado o no
                        this.periodoCerrado = this.VerificarAAPPEstaCerrado();

                        for (int i = 0; i < this.radGridViewComprobantes.Columns.Count; i++)
                        {
                            this.radGridViewComprobantes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        }

                        this.radGridViewComprobantes.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);
                        //this.radGridViewComprobantes.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewComprobantes.Rows[0].IsCurrent = true;

                        this.radPanelAccionesComp.Visible = true;
                        this.radGridViewComprobantes.Visible = true;
                        this.radGridViewComprobantes.TableElement.ScrollToRow(0);
                        this.radGridViewComprobantes.Focus();


                        string estado = this.radGridViewComprobantes.Rows[0].Cells["STATIC"].Value.ToString();
                        this.AccionesPosibles(estado);
                    }
                    else
                    {
                        //No se encontraron comprobantes para la búsqueda solicitada
                        this.radGridViewComprobantes.Visible = false;
                        RadMessageBox.Show(this.LP.GetText("errBusqComp", "No se han encontrado comprobantes para el criterio de búsqueda seleccionado"), "");
                    }
                }
                catch (Exception ex)
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

        private void RadButtonSuprimir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSuprimir);
        }

        private void RadButtonSuprimir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSuprimir);
        }

        private void RadButtonVerAccionesActuales_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonVerAccionesActuales);
        }

        private void RadButtonVerAccionesActuales_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonVerAccionesActuales);
        }

        private void RadButtonVerHistorial_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonVerHistorial);
        }

        private void RadButtonVerHistorial_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonVerHistorial);
        }

        private void RadButtonBuscar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonBuscar);
        }

        private void RadButtonBuscar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonBuscar);
        }

        private void RadButtonAprobar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAprobar);
        }

        private void RadButtonAprobar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAprobar);
        }

        private void RadButtonRechazar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonRechazar);
        }

        private void RadButtonRechazar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonRechazar);
        }

        private void RadButtonContabilizar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonContabilizar);
        }

        private void RadButtonContabilizar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonContabilizar);
        }

        private void RadButtonAprobarContabilizar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonAprobarContabilizar);
        }

        private void RadButtonAprobarContabilizar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonAprobarContabilizar);
        }

        private void RadButtonRevertir_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonRevertir);
        }

        private void RadButtonRevertir_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonRevertir);
        }

        private void RadButtonActualizar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonActualizar);
        }

        private void RadButtonActualizar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonActualizar);
        }

        private void RadGridViewComprobantes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.EditarComprobante();

                /*
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                DataGridViewRow row = this.dgComprobantes.SelectedRows[0];

                this.CargarComprobanteSeleccionado(row);

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
                 */
            }
        }

        private void RadGridViewComprobantes_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string estado = "";
                if (this.radGridViewComprobantes.SelectedRows.Count == 1)
                {
                    int indice = this.radGridViewComprobantes.Rows.IndexOf(this.radGridViewComprobantes.CurrentRow);
                    estado = this.radGridViewComprobantes.Rows[indice].Cells["STATIC"].Value.ToString();
                    this.AccionesPosibles(estado);
                }
                else
                {
                    for (int i = 0; i < this.radGridViewComprobantes.SelectedRows.Count; i++)
                    {
                        estado = this.radGridViewComprobantes.SelectedRows[i].Cells["STATIC"].Value.ToString();
                        this.AccionesPosiblesActivar(estado);
                    }
                }

                if (this.radGridViewVisorHistorial.Visible)
                {
                    //Agrandar el tamaño de la grid de comprobantes
                    this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height + this.radGridViewVisorHistorial.Size.Height;

                    //Ocultar la Grid del Visor del Historial del comprobante porque se ha cambiado de comprobante
                    this.radGridViewVisorHistorial.Visible = false;
                }
                else
                if (this.radGridViewAccionesActuales.Visible)
                {
                    //Agrandar el tamaño de la grid de comprobantes
                    this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height + this.radGridViewAccionesActuales.Size.Height;

                    //Ocultar la Grid de Acciones actuales que se han realizado con los comprobantes
                    this.radGridViewAccionesActuales.Visible = false;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void RadGridViewComprobantes_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewComprobantes.Columns.Contains("NoComp")) this.radGridViewComprobantes.Columns["NoComp"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewComprobantes.Columns.Contains("DebeML")) this.radGridViewComprobantes.Columns["DebeML"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewComprobantes.Columns.Contains("HaberML")) this.radGridViewComprobantes.Columns["HaberML"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewComprobantes.Columns.Contains("DebeME")) this.radGridViewComprobantes.Columns["DebeME"].TextAlignment = ContentAlignment.MiddleRight;
            if (this.radGridViewComprobantes.Columns.Contains("HaberME")) this.radGridViewComprobantes.Columns["HaberME"].TextAlignment = ContentAlignment.MiddleRight;

            if (this.radGridViewComprobantes.Columns.Contains("Tasa")) this.radGridViewComprobantes.Columns["Tasa"].IsVisible = false;
            if (this.radGridViewComprobantes.Columns.Contains("NoMovimiento")) this.radGridViewComprobantes.Columns["NoMovimiento"].IsVisible = false;
            if (this.radGridViewComprobantes.Columns.Contains("FECOIC")) this.radGridViewComprobantes.Columns["FECOIC"].IsVisible = false;
            if (this.radGridViewComprobantes.Columns.Contains("SAPRIC")) this.radGridViewComprobantes.Columns["SAPRIC"].IsVisible = false;
            if (this.radGridViewComprobantes.Columns.Contains("STATIC")) this.radGridViewComprobantes.Columns["STATIC"].IsVisible = false;
        }

        private void RadGridViewVisorHistorial_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewVisorHistorial.Columns.Contains("Estado")) this.radGridViewVisorHistorial.Columns["Estado"].IsVisible = false;
        }

        private void RadGridViewAccionesActuales_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewAccionesActuales.Columns.Contains("CodAccion")) this.radGridViewAccionesActuales.Columns["CodAccion"].IsVisible = false;
        }

        private void frmCompContListaGLB01_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Lista de comprobantes");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContListaGLB01Titulo", "Lista de comprobantes");

            //Traducir los Literales de los botones
            this.radButtonNuevo.Text = this.LP.GetText("toolStripNuevo", "Nuevo");
            this.radButtonEditar.Text = this.LP.GetText("toolStripEditar", "Editar");
            this.radButtonSuprimir.Text = this.LP.GetText("toolStripSuprimir", "Suprimir");
            this.radButtonAprobar.Text = this.LP.GetText("toolStripAprobar", "Aprobar");
            this.radButtonRechazar.Text = this.LP.GetText("toolStripRechazar", "Rechazar");
            this.radButtonContabilizar.Text = this.LP.GetText("toolStripContabilizar", "Contabilizar");
            this.radButtonAprobarContabilizar.Text = this.LP.GetText("toolStripAprobarContab", "Aprobar+Contabilizar");
            this.radButtonRevertir.Text = this.LP.GetText("toolStripRevertir", "Revertir");
            this.radButtonActualizar.Text = this.LP.GetText("toolStripActualizar", "Actualizar");
            this.radButtonVerAccionesActuales.Text = this.LP.GetText("toolStripVerAccAct", "Ver Acciones Actuales");
            this.radButtonVerHistorial.Text = this.LP.GetText("toolStripVerHistorial", "Ver Historial");

            //this.gbCabecera.Text = this.LP.GetText("lblCabecera", "Cabecera");
            this.lblCompania.Text = this.LP.GetText("lblCompania", "Compañía");
            this.lblAAPP.Text = this.LP.GetText("lblAnoPeriodo", "Año-Período");
            // this.lblTipo.Text = this.LP.GetText("lblTipo", "Tipo");
            this.lblTipo.Text = this.LP.GetText("lblTipoComprobante", "Tipo Comprobante");
            this.lblNoComprobante.Text = this.LP.GetText("lblNoComprobante", "Nº Comprobante");
            this.lblEstado.Text = this.LP.GetText("lblEstado", "Estado");
            //this.lblModoTrabajo.Text = this.LP.GetText("lblModoTrabajo", "Modo de trabajo");
            this.lblModoTrabajo.Text = this.LP.GetText("lblSeleccionTipo", "Selección Tipo");
            this.radButtonBuscar.Text = this.LP.GetText("lblCompContImpDeFinBuscar", "Buscar");

            //Traducir los encabezados de las columnas de la Grid de Comprobantes
            if (this.radGridViewComprobantes != null) this.RadGridViewComprobantesHeader();
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
            this.dtComprobantes.Columns.Add("Clase", typeof(string));
            this.dtComprobantes.Columns.Add("DebeML", typeof(string));
            this.dtComprobantes.Columns.Add("HaberML", typeof(string));
            this.dtComprobantes.Columns.Add("DebeME", typeof(string));
            this.dtComprobantes.Columns.Add("HaberME", typeof(string));
            this.dtComprobantes.Columns.Add("Estado", typeof(string));
            this.dtComprobantes.Columns.Add("descripcion", typeof(string));
            this.dtComprobantes.Columns.Add("Tasa", typeof(string));
            this.dtComprobantes.Columns.Add("NoMovimiento", typeof(string));
            this.dtComprobantes.Columns.Add("FECOIC", typeof(string));
            this.dtComprobantes.Columns.Add("SAPRIC", typeof(string));
            this.dtComprobantes.Columns.Add("STATIC", typeof(string));

            this.radGridViewComprobantes.DataSource = this.dtComprobantes;
            //Escribe el encabezado de la Grid de EditarLotes
            this.BuildDisplayNamesComprobantes();
            this.RadGridViewComprobantesHeader();
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
                    { "compania", this.LP.GetText("CompContdgHeaderCompania", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "Tipo", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NoComp", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "Fecha", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "Clase", this.LP.GetText("CompContdgHeaderClase", "Clase") },
                    { "DebeML", this.LP.GetText("CompContdgHeaderDebeML", "Debe ML") },
                    { "HaberML", this.LP.GetText("CompContdgHeaderHaberML", "Haber ML") },
                    { "DebeME", this.LP.GetText("CompContdgHeaderDebeME", "Debe ME") },
                    { "HaberME", this.LP.GetText("CompContdgHeaderHaberME", "Haber ME") },
                    { "Estado", this.LP.GetText("lblCompTransEstado", "Estado") },
                    { "descripcion", this.LP.GetText("CompContdgHeaderDescripcion", "Descripción") },
                    { "Tasa", this.LP.GetText("CompContdgHeaderTasa", "Tasa") },
                    { "NoMovimiento", this.LP.GetText("CompContdgHeaderNoMovimiento", "No Mov") },
                    { "FECOIC", "FECOIC" },
                    { "SAPRIC", "SAPRIC" },
                    { "STATIC", "STATIC" }
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
        /// Crea la Grid para mostrar el historial del comprobante
        /// </summary>
        private void CrearDataGridVisorHistorial()
        {
            this.dtVerHistorial = new DataTable
            {
                TableName = "Tabla"
            };

            //Adicionar las columnas al DataTable
            this.dtVerHistorial.Columns.Add("Fecha", typeof(string));
            this.dtVerHistorial.Columns.Add("Hora", typeof(string));
            this.dtVerHistorial.Columns.Add("UsuarioApp", typeof(string));
            this.dtVerHistorial.Columns.Add("UsuarioRed", typeof(string));
            this.dtVerHistorial.Columns.Add("Estado", typeof(string));
            this.dtVerHistorial.Columns.Add("compania", typeof(string));
            this.dtVerHistorial.Columns.Add("AAPP", typeof(string));
            this.dtVerHistorial.Columns.Add("Tipo", typeof(string));
            this.dtVerHistorial.Columns.Add("NoComp", typeof(string));
            this.dtVerHistorial.Columns.Add("Accion", typeof(string));

            this.radGridViewVisorHistorial.DataSource = this.dtVerHistorial;
            //Escribe el encabezado de la Grid de EditarLotes
            this.BuildDisplayNamesVerHistorial();
            this.RadGridViewVerHistorialHeader();
        }

        /// <summary>
        /// Encabezados para la Grid del Visor del Historial
        /// </summary>
        private void BuildDisplayNamesVerHistorial()
        {
            try
            {
                this.displayNamesVerHistorial = new Dictionary<string, string>
                {
                    { "Fecha", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "Hora", this.LP.GetText("CompContdgHeaderHora", "Hora") },
                    { "UsuarioApp", this.LP.GetText("lblUsuarioApp", "Usuario App") },
                    { "UsuarioRed", this.LP.GetText("lblUsuarioRed", "Usuario Red") },
                    { "Estado", this.LP.GetText("lblEstado", "Estado") },
                    { "compania", this.LP.GetText("CompContdgHeaderCompania", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "Tipo", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NoComp", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "Accion", this.LP.GetText("lblAccion", "Acción") }
                };
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid del Visor del Historial
        /// </summary>
        private void RadGridViewVerHistorialHeader()
        {
            try
            {
                if (this.radGridViewVisorHistorial.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesVerHistorial)
                    {
                        if (this.radGridViewVisorHistorial.Columns.Contains(item.Key)) this.radGridViewVisorHistorial.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Crea la Grid para mostrar las acciones que está realizando el usuario sobre los comprobantes
        /// </summary>
        private void CrearDataGridAccionesActuales()
        {
            this.dtAccionesActuales = new DataTable
            {
                TableName = "Tabla"
            };

            //Adicionar las columnas al DataTable
            this.dtAccionesActuales.Columns.Add("Fecha", typeof(string));
            this.dtAccionesActuales.Columns.Add("Hora", typeof(string));
            this.dtAccionesActuales.Columns.Add("compania", typeof(string));
            this.dtAccionesActuales.Columns.Add("AAPP", typeof(string));
            this.dtAccionesActuales.Columns.Add("Tipo", typeof(string));
            this.dtAccionesActuales.Columns.Add("NoComp", typeof(string));
            this.dtAccionesActuales.Columns.Add("Estado", typeof(string));
            this.dtAccionesActuales.Columns.Add("Accion", typeof(string));
            
            this.radGridViewAccionesActuales.DataSource = this.dtAccionesActuales;
            //Escribe el encabezado de la Grid de EditarLotes
            this.BuildDisplayNamesAccionesActuales();
            this.RadGridViewAccionesActualesHeader();
        }

        /// <summary>
        /// Encabezados para la Grid del Visor del Historial
        /// </summary>
        private void BuildDisplayNamesAccionesActuales()
        {
            try
            {
                this.displayNamesAccionesActuales = new Dictionary<string, string>
                {
                    { "Fecha", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "Hora", this.LP.GetText("CompContdgHeaderHora", "Hora") },
                    { "compania", this.LP.GetText("CompContdgHeaderCompania", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "Tipo", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NoComp", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "Estado", this.LP.GetText("lblEstado", "Estado") },
                    { "Accion", this.LP.GetText("lblAccion", "Acción") },
                    { "CodAccion", "Código Acción" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid del Visor del Historial
        /// </summary>
        private void RadGridViewAccionesActualesHeader()
        {
            try
            {
                if (this.radGridViewAccionesActuales.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesAccionesActuales)
                    {
                        if (this.radGridViewAccionesActuales.Columns.Contains(item.Key)) this.radGridViewAccionesActuales.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Cargar las compañías
        /// </summary>
        private void FillCompanias()
        {
            //string query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 where STATMG='V' order by CCIAMG"; JL
            string query = "select CCIAMG, NCIAMG from " + GlobalVar.PrefijoTablaCG + "GLM01 order by CCIAMG";

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
            string query = "Select TIVOTV, NOMBTV From " + GlobalVar.PrefijoTablaCG + "GLT06 ";
            // query += "where CODITV='1' and STATTV='V' order by TIVOTV";  jl
            // query += "where STATTV='V' order by TIVOTV"; JL
            query += " order by TIVOTV";
            // string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, false); jl
            string result = this.FillComboBox(query, "TIVOTV", "NOMBTV", ref this.cmbTipo, true, -1, true);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                string mensaje = this.LP.GetText("errGetTiposComp", "Error obteniendo los tipos de comprobantes") + " (" + result + ")";
                RadMessageBox.Show(mensaje, error);
            }
        }
        
        /// <summary>
        /// Llena el desplegable de Estados
        /// </summary>
        private void FillcmbEstado()
        {
            ArrayList estadoValores = new ArrayList
            {
                new AddValue(this.LP.GetText("lblEstadoTodos", "Todos"), " "),
                new AddValue(this.LP.GetText("lblEstadoAprobados", "Aprobados"), "A"),
                new AddValue(this.LP.GetText("lblEstadoNoAprobados", "No Aprobados"), "V"),
                new AddValue(this.LP.GetText("lblEstadoContabilizados", "Contabilizados"), "E"),
                new AddValue(this.LP.GetText("lblEstadoRechazados", "Rechazados"), "R")
            };

            this.cmbEstado.DataSource = estadoValores;
            this.cmbEstado.DisplayMember = "Display";
            this.cmbEstado.ValueMember = "Value";
        }

        /// <summary>
        /// Llena el desplegable de Modos de Trabajo
        /// </summary>
        private void FillcmbModoTrabajo()
        {
            ArrayList modoValores = new ArrayList
            {
                new AddValue(this.LP.GetText("lblModoTodos", "Todos"), "2"),
                new AddValue(this.LP.GetText("lblModoInteractivo", "Interactivo"), "0"),
                new AddValue(this.LP.GetText("lblModoLotes", "Lotes"), "1")
            };

            this.cmbModoTrabajo.DataSource = modoValores;
            this.cmbModoTrabajo.DisplayMember = "Display";
            this.cmbModoTrabajo.ValueMember = "Value";
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
            
            //Chequear el AAPP
            sap = sap.Trim();
            if (sap == "")
            {
                RadMessageBox.Show(this.LP.GetText("errAAPPObl", "Es obligatorio informar el año-periodo"), error);
                this.txtMaskAAPP.Focus();
                return (false);
            }
            else
            {
                //Validar los periodos
                if (sap.Length != 4)
                {
                    RadMessageBox.Show(this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto"));
                    this.txtMaskAAPP.Focus();
                    return (false);
                }

                string periodo = sap.Substring(2, 2);
                bool errorPeriodo = false;
                try
                {
                    int periodoInt = Convert.ToInt16(periodo);
                    if (!(periodoInt >= 1 && periodoInt <= 99))
                    {
                        errorPeriodo = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));

                    errorPeriodo = true;
                }

                if (errorPeriodo)
                {
                    RadMessageBox.Show(this.LP.GetText("errPeriodoFormato", "El periodo no tiene un formato correcto"));
                    this.txtMaskAAPP.Focus();
                    return (false);
                }
            }

            //Chequear el tipo de comprobante
            if (this.cmbTipo.Text.Trim() == "")
            {
                this.codTipo = "";

                //Chequear si tiene permiso
                bool operarConsulta = aut.Validar("004", "03", " ", "10");   //DUDA blanco o vacío ???
                if (!operarConsulta)
                {
                    //Error usuario no autorizado
                    RadMessageBox.Show("Usuario no autorizado a ningún tipo de comprobante", error);
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
                case "0":
                    result = this.LP.GetText("lblEstadoCreado", "Creado");
                    break;
                case "1":
                    result = this.LP.GetText("lblEstadoSuprimido", "Suprimido");
                    break;
                case "2":
                    result = this.LP.GetText("lblEstadoTranfIVA", "Transferido IVA");
                    break;
                case "3":
                    result = this.LP.GetText("lblEstadoSupIVA", "Suprimido IVA");
                    break;
                case "A":
                    result = this.LP.GetText("lblEstadoAprobado", "Aprobado");
                    break;
                case "E" :
                    result = this.LP.GetText("lblEstadoContabilizado", "Contabilizado");
                    break;
                case "R" :
                    result = this.LP.GetText("lblEstadoRechazado", "Rechazado");
                    break;
                case "V" :
                    result = this.LP.GetText("lblEstadoNoAprobado", "No Aprobado");
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

            dtDetalle.Columns.Add("RowNumber", typeof(string));
            dtDetalle.Columns.Add("Pais", typeof(string));

            DataRow row;

            string nombreTabla = GlobalVar.PrefijoTablaCG + "GLB01 ";
            string query = "";

            /*string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
            query += "where CCIADT ='" + compania + "' and ";
            query += "SAPRDT =" + anoperiodo + " and ";
            query += "TICODT =" + tipo + " and ";
            query += "NUCODT =" + noComprobante;
            query += " order by SIMIDT";*/


            switch (this.tipoBaseDatosCG)
            {
                case "DB2":
                    query = "select RRN(" + nombreTabla + ") as id, " + nombreTabla + ".* from " + nombreTabla;
                    break;
                case "SQLServer":
                    query += "select GERIDENTI as id, " + nombreTabla + ".* from " + nombreTabla;
                    break;
                case "Oracle":
                    //id_prefijotabla_prefijolote + W01   o W11
                    string campoOracle = "ID_" + nombreTabla;
                    query += "select " + campoOracle + " as id, " + nombreTabla + ".* from " + nombreTabla;
                    break;
            }

            query += " where CCIADT ='" + compania + "' and ";
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
                    row["RowNumber"] = dr["id"].ToString().Trim();
                    row["Pais"] = dr["PCIFAD"].ToString().Trim();

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
        private void CargarComprobanteSeleccionado(GridViewRowInfo row)
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

            string estado = row.Cells["STATIC"].Value.ToString();
            bool soloConsulta = false;
            if (estado == "A" || estado == "E" || this.periodoCerrado) soloConsulta = true;

            this.CargarComprobanteEdicion(comprobante, soloConsulta);

            this.progressBarEspera.Visible = false;
        }

        /// <summary>
        /// Llamar al formulario de edición con los datos del comprobante
        /// </summary>
        /// <param name="comprobante"></param>
        private void CargarComprobanteEdicion(ComprobanteContable comprobante, bool soloConsulta)
        {
            //Cerrar el formulario actual ???
            // lista de comprobantes?
            // GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];
            frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita
            {
                ImportarComprobante = true,
                ComprobanteContableImportar = comprobante,
                NombreComprobante = comprobante.Cab_descripcion,
                EdicionComprobanteGLB01 = true,
                ComprobanteSoloConsulta = soloConsulta,
                Batch = false,
                FrmPadre = this
            };
            frmCompCont.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
            frmCompCont.Show();            
        }
        private void ActualizaListaComprobantes_ArgSel(frmCompContAltaEdita.ActualizaListaComprobantesArgs e)
        {
            try
            {
                string Cab_compania = e.Valor[0].ToString().Trim();
                string aapp = e.Valor[1].ToString().Trim();
                string Cab_anoperiodo = aapp;
                string Cab_tipo = e.Valor[2].ToString().Trim();
                string Cab_noComprobante = e.Valor[3].ToString().Trim();
                string Cab_fecha = e.Valor[4].ToString().Trim();
                string Cab_compania_ant = e.Valor[5].ToString().Trim();
                string Cab_anoperiodo_ant = e.Valor[6].ToString().Trim();
                string Cab_tipo_ant = e.Valor[7].ToString().Trim();
                string Cab_noComprobante_ant = e.Valor[8].ToString().Trim();
                string Cab_fecha_ant = e.Valor[9].ToString().Trim();
                string Cab_extendido = e.Valor[10].ToString().Trim();
                string lblTotalDebe = e.Valor[11].ToString().Trim();
                string lblTotalHaber = e.Valor[12].ToString().Trim();
                string lblExtDebe = e.Valor[13].ToString().Trim();
                string lblExtHaber = e.Valor[14].ToString().Trim();
                string lblImporte3Debe = e.Valor[15].ToString().Trim();
                string lblImporte3Haber = e.Valor[16].ToString().Trim();
                string numapuntes = e.Valor[17].ToString().Trim();

                // busco fila seleccionada.
                for (int i = 0; i < this.radGridViewComprobantes.Rows.Count; i++)
                {
                    if ((this.radGridViewComprobantes.Rows[i].Cells["compania"].Value.ToString().Trim() == Cab_compania)
                    && (this.radGridViewComprobantes.Rows[i].Cells["aapp"].Value.ToString().Trim() == Cab_anoperiodo)
                    && (this.radGridViewComprobantes.Rows[i].Cells["tipo"].Value.ToString().Trim() == Cab_tipo)
                    && (this.radGridViewComprobantes.Rows[i].Cells["noComp"].Value.ToString().Trim() == Cab_noComprobante)
                    && (this.radGridViewComprobantes.Rows[i].Cells["fecha"].Value.ToString().Trim() == Cab_fecha))
                    {
                        this.radGridViewComprobantes.Rows[i].Cells["DebeML"].Value = lblTotalDebe.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["HaberML"].Value = lblTotalHaber.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["DebeME"].Value = lblExtDebe.ToString().Trim();
                        this.radGridViewComprobantes.Rows[i].Cells["HaberME"].Value = lblExtHaber.ToString().Trim();

                    }
                }

                

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
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
                string[] datosCompAct = utilesCG.ObtenerTipoCalendarioCompania(codigo);
                string plan = datosCompAct[1];

                if (plan != "")
                {
                    //Buscar la información sobre los campos extendidos para el plan de la compañía
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLMX2 ";
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

                //Verificar autorizaciones
                bool operarConsulta = aut.Validar("002", "02", codigo, "10");
                if (!operarConsulta)
                {
                    //Error usuario no autorizado
                    result = this.LP.GetText("lblErrUserNoAutComp", "Usuario no autorizado a consultar los movimientos de la compañía seleccionada");   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmListaGLB01ValCiaExcep", "Error al validar la compañia") + " (" + ex.Message + ")";
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
                query += "where CODITV='1' and STATTV='V' and TIVOTV='" + codigo + "'";

                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros == 0) result = this.LP.GetText("errTipoNoExiste", "Tipo no existe");

                //Verificar autorizaciones
                bool operarConsulta = aut.Validar("004", "03", codigo, "10");
                if (!operarConsulta)
                {
                    //Error usuario no autorizado
                    result = this.LP.GetText("lblErrUserNoAutTipoComp", "Usuario no autorizado a consultar los movimientos del tipo de comprobante solicitado");   //Falta traducir
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("lblfrmListaGLB01ValTipoExcep", "Error al validar el tipo") + " (" + ex.Message + ")";
            }

            return (result);
        }

        /// <summary>
        /// Activa / Desactiva los botones de acciones de estado del comprobante
        /// </summary>
        /// <param name="estadoComprobante"></param>
        private void AccionesPosibles(string estadoComprobante)
        {
            if (periodoCerrado)
            {
                utiles.ButtonEnabled(ref this.radButtonAprobar, false);
                utiles.ButtonEnabled(ref this.radButtonRechazar, false);
                utiles.ButtonEnabled(ref this.radButtonContabilizar, false);
                utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, false);
                utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
            }
            else
                switch (estadoComprobante)
                {
                    case "A":  //Aprobado
                        utiles.ButtonEnabled(ref this.radButtonAprobar, false);
                        utiles.ButtonEnabled(ref this.radButtonRechazar, true);
                        utiles.ButtonEnabled(ref this.radButtonContabilizar, true);
                        utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, false);
                        utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                        utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
                        break;
                    case "E":  //Contabilizado
                        utiles.ButtonEnabled(ref this.radButtonAprobar, false);
                        utiles.ButtonEnabled(ref this.radButtonRechazar, false);
                        utiles.ButtonEnabled(ref this.radButtonContabilizar, false);
                        utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, false);
                        utiles.ButtonEnabled(ref this.radButtonRevertir, true);
                        utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
                        break;
                    case "R":  //Rechazado
                        utiles.ButtonEnabled(ref this.radButtonAprobar, true);
                        utiles.ButtonEnabled(ref this.radButtonRechazar, false);
                        utiles.ButtonEnabled(ref this.radButtonContabilizar, false);
                        utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, true);
                        utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                        utiles.ButtonEnabled(ref this.radButtonSuprimir, true);
                        break;
                    case "V":  //Pendientes de aprobar
                        utiles.ButtonEnabled(ref this.radButtonAprobar, true);
                        utiles.ButtonEnabled(ref this.radButtonRechazar, true);
                        utiles.ButtonEnabled(ref this.radButtonContabilizar, false);
                        utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, true);
                        utiles.ButtonEnabled(ref this.radButtonRevertir, false);
                        utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
                        break;
                }

            utiles.ButtonEnabled(ref this.radButtonEditar, true);
            utiles.ButtonEnabled(ref this.radButtonActualizar, true);
            utiles.ButtonEnabled(ref this.radButtonVerHistorial, true);
        }

        /// <summary>
        /// Activa los botones de acciones de estado del comprobante
        /// </summary>
        /// <param name="estadoComprobante"></param>
        private void AccionesPosiblesActivar(string estadoComprobante)
        {
            switch (estadoComprobante)
            {
                case "A":  //Aprobado
                    utiles.ButtonEnabled(ref this.radButtonRechazar, true);
                    utiles.ButtonEnabled(ref this.radButtonContabilizar, true);
                    break;
                case "E":  //Contabilizado
                    utiles.ButtonEnabled(ref this.radButtonRevertir, true);
                    break;
                case "R":  //Rechazado
                    utiles.ButtonEnabled(ref this.radButtonAprobar, true);
                    utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, true);
                    utiles.ButtonEnabled(ref this.radButtonSuprimir, true);
                    break;
                case "V":  //Pendientes de aprobar
                    utiles.ButtonEnabled(ref this.radButtonAprobar, true);
                    utiles.ButtonEnabled(ref this.radButtonAprobarContabilizar, true);
                    break;
            }

            utiles.ButtonEnabled(ref this.radButtonEditar, true);
            utiles.ButtonEnabled(ref this.radButtonActualizar, true);
            utiles.ButtonEnabled(ref this.radButtonVerHistorial, true);
        }

        /// <summary>
        /// Muestra el Grid con el Visor del Historial del Comprobante
        /// </summary>
        private void ViewGridVisorHistorial()
        {
            try
            {
                this.radGridViewVisorHistorial.DataSource = this.dtVerHistorial;

                /*
                //Ajustar las columnas
                this.dgErrores.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Linea"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Error"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                */

                this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height - this.radGridViewVisorHistorial.Size.Height;

                /*
                //Columnas
                this.dgErrores.Columns["Tipo"].HeaderText = this.LP.GetText("lblGridErrorCabTipo", "Tipo");
                this.dgErrores.Columns["Linea"].HeaderText = this.LP.GetText("lblGridErrorCabLinea", "Línea");
                this.dgErrores.Columns["Error"].HeaderText = this.LP.GetText("lblGridErrorCabError", "Error");
                */

                this.radGridViewVisorHistorial.SelectionMode = GridViewSelectionMode.FullRowSelect;
                this.radGridViewAccionesActuales.Visible = false;
                this.radGridViewVisorHistorial.Visible = true;

                for (int i = 0; i < this.radGridViewVisorHistorial.Columns.Count; i++)
                    this.radGridViewVisorHistorial.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                this.radGridViewVisorHistorial.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                this.radGridViewVisorHistorial.MasterTemplate.BestFitColumns();
                this.radGridViewVisorHistorial.Rows[0].IsCurrent = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Muestra el Grid de las acciones actuales
        /// </summary>
        private void ViewGridAccionesActuales()
        {
            try
            {
                this.radGridViewAccionesActuales.DataSource = this.dtAccionesActuales;

                /*
                //Ajustar las columnas
                this.dgErrores.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Linea"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgErrores.Columns["Error"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                */

                this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height - this.radGridViewAccionesActuales.Size.Height;

                /*
                //Columnas
                this.dgErrores.Columns["Tipo"].HeaderText = this.LP.GetText("lblGridErrorCabTipo", "Tipo");
                this.dgErrores.Columns["Linea"].HeaderText = this.LP.GetText("lblGridErrorCabLinea", "Línea");
                this.dgErrores.Columns["Error"].HeaderText = this.LP.GetText("lblGridErrorCabError", "Error");
                */

                this.radGridViewAccionesActuales.SelectionMode = GridViewSelectionMode.FullRowSelect;
                this.radGridViewVisorHistorial.Visible = false;
                this.radGridViewAccionesActuales.Visible = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Carga la Info del Historial del Comprobante en la Grid del Visor del Historial del comprobante
        /// </summary>
        /// <param name="row"></param>
        private void CargarInfoVisorHistorial(GridViewRowInfo row)
        {
            IDataReader dr = null;

            try
            {
                //Eliminar posible historial anterior
                if (this.radGridViewVisorHistorial.Rows.Count > 0) this.dtVerHistorial.Rows.Clear();

                string compania = row.Cells["compania"].Value.ToString();

                string aapp = row.Cells["SAPRIC"].Value.ToString().Trim();
                string aappFormat = aapp;
                if (aapp.Length == 5) aappFormat = aapp.Substring(1, 4);
                else if (aapp.Length < 4) aapp = aapp.PadRight(4, ' ');
                aappFormat = aappFormat.Substring(0, 2) + "-" + aappFormat.Substring(2, 2);

                string tipo = row.Cells["Tipo"].Value.ToString();
                string noComp = row.Cells["NoComp"].Value.ToString();

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLL02 ";
                query += "where CCIAL2 ='" + compania + "' and ";
                query += "SAPRL2 =" + aapp + " and ";
                query += "TICOL2 =" + tipo + " and ";
                query += "NUCOL2 =" + noComp;
                query += " order by DATEL2, TIMEL2";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                DataRow rowHistorial;
                string estado = "";
                string hora = "";

                string CRTDL2 = "";

                while (dr.Read())
                {
                    rowHistorial = this.dtVerHistorial.NewRow();

                    rowHistorial["Fecha"] = utiles.FechaToFormatoCG(dr["DATEL2"].ToString().Trim()).ToShortDateString();

                    hora = dr["TIMEL2"].ToString().Trim();
                    if (hora != "")
                    {
                        if (hora.Length != 6) hora = hora.PadRight(6, '0');
                        hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2) + ":" + hora.Substring(4, 2);
                    }
                    rowHistorial["Hora"] = hora;

                    rowHistorial["UsuarioApp"] = dr["IDUSL2"].ToString().Trim();
                    rowHistorial["UsuarioRed"] = dr["IBMUL2"].ToString().Trim();

                    estado = dr["STATL2"].ToString().Trim();
                    rowHistorial["Estado"] = this.ObtenerEstadoComprobante(estado);

                    rowHistorial["compania"] = compania;
                    rowHistorial["AAPP"] = aappFormat;
                    rowHistorial["Tipo"] = tipo;
                    rowHistorial["NoComp"] = noComp;

                    CRTDL2 = dr["CRTDL2"].ToString().Trim();

                    if (CRTDL2 != "")
                    {
                        rowHistorial["Accion"] = this.ObtenerEstadoComprobante(CRTDL2);
                    }
                    else
                    {
                        rowHistorial["Accion"] = this.ObtenerEstadoComprobante(estado);
                    }

                    this.dtVerHistorial.Rows.Add(rowHistorial);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Dado un código de acción, devuelve su nombre
        /// </summary>
        /// <param name="codAccion"></param>
        /// <returns></returns>
        private string AccionNombre(string codAccion)
        {
            string accionNombre = "";

            try
            {
                switch (codAccion)
                {
                    case "A":
                        accionNombre = this.LP.GetText("lblAccionAprobar", "Aprobar");
                        break;
                    case "R":
                        accionNombre = this.LP.GetText("lblAccionRechazar", "Rechazar");
                        break;
                    case "C":
                        accionNombre = this.LP.GetText("lblAccionContabilizar", "Contabilizar");
                        break;
                    case "AC":
                        accionNombre = this.LP.GetText("lblAccionAprobarContabilizar", "Aprobar+Contabilizar");
                        break;
                    case "RE":
                        accionNombre = this.LP.GetText("lblAccionRevertir", "Revertir");
                        break;
                    case "D":
                        accionNombre = this.LP.GetText("lblAccionEliminar", "Eliminar");
                        break;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (accionNombre);
        }

        /// <summary>
        /// Inserta una accion en la Grid de Acciones Actuales
        /// </summary>
        /// <param name="accion"></param>
        private void InsertarGridAcciones(GridViewRowInfo rowComp, string codAccion, string estadoAnterior)
        {
            try
            {
                //Nombre de la accion
                string accion = this.AccionNombre(codAccion);

                string compania = rowComp.Cells["compania"].Value.ToString();

                string aapp = rowComp.Cells["SAPRIC"].Value.ToString().Trim();
                string aappFormat = aapp;
                if (aapp.Length == 5) aappFormat = aapp.Substring(1, 4);
                else if (aapp.Length < 4) aapp = aapp.PadRight(4, ' ');
                aappFormat = aappFormat.Substring(0, 2) + "-" + aappFormat.Substring(2, 2);

                string tipo = rowComp.Cells["Tipo"].Value.ToString();
                string noComp = rowComp.Cells["NoComp"].Value.ToString();

                DataRow rowAccion = this.dtAccionesActuales.NewRow();

                rowAccion["Fecha"] = DateTime.Now.ToString("dd/MM/yyyy");
                rowAccion["Hora"] = DateTime.Now.ToString("hh:mm:ss");
                rowAccion["compania"] = compania;
                rowAccion["AAPP"] = aappFormat;
                rowAccion["Tipo"] = tipo;
                rowAccion["NoComp"] = noComp;
                rowAccion["Estado"] = rowComp.Cells["Estado"].Value.ToString();

                string estadoAnteriorDesc = this.ObtenerEstadoComprobante(estadoAnterior);
                if (accion != "") accion = estadoAnteriorDesc + " - " + accion;
                else accion = estadoAnteriorDesc;

                rowAccion["Accion"] = accion;
                rowAccion["CodAccion"] = codAccion;

                this.radGridViewAccionesActuales.Rows.Add(rowAccion);

                utiles.ButtonEnabled(ref this.radButtonVerAccionesActuales, true);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Ocultar el Grid de Visor del Historial o el de Acciones si están visibles y recalcula el tamaño del grid de comprobantes
        /// </summary>
        private void OcultarGridHistorial_Acciones()
        {
            try
            {
                if (this.radGridViewVisorHistorial.Visible)
                {
                    this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height + this.radGridViewVisorHistorial.Size.Height;
                    this.radGridViewVisorHistorial.Visible = false;
                }
                else
                    if (this.radGridViewAccionesActuales.Visible)
                {
                    this.radGridViewComprobantes.Height = this.radGridViewComprobantes.Size.Height + this.radGridViewAccionesActuales.Size.Height;
                    this.radGridViewAccionesActuales.Visible = false;
                }

                this.radGridViewComprobantes.Visible = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Actualiza la información de una fila de la Grid de comprobantes cargados
        /// </summary>
        /// <param name="row"></param>
        /// <param name="indice"></param>
        private void ActualizarInfoGridComprobantes(GridViewRowInfo row, int indice)
        {
            IDataReader dr = null;
            try
            {
                string compania = row.Cells["compania"].Value.ToString();
                string aapp = row.Cells["SAPRIC"].Value.ToString().Trim();
                string tipo = row.Cells["Tipo"].Value.ToString();
                string noComp = row.Cells["NoComp"].Value.ToString();

                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                query += "where CCIAIC ='" + compania;
                query += "' and SAPRIC =" + aapp;
                query += " and TICOIC =" + tipo;
                query += " and NUCOIC =" + noComp;

                string descripcion = "";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    this.radGridViewComprobantes.Rows[indice].Cells["Fecha"].Value = utiles.FechaToFormatoCG(dr["FECOIC"].ToString()).ToShortDateString();
                    this.radGridViewComprobantes.Rows[indice].Cells["Clase"].Value = dr["TVOUIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["DebeML"].Value = dr["DEBEIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["HaberML"].Value = dr["HABEIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["DebeME"].Value = dr["DEMEIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["HaberME"].Value = dr["HAMEIC"].ToString().Trim();

                    this.radGridViewComprobantes.Rows[indice].Cells["Estado"].Value = this.ObtenerEstadoComprobante(dr["STATIC"].ToString()).Trim();

                    descripcion = this.ObtenerDescripcion(compania, aapp, tipo, noComp).Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["descripcion"].Value = descripcion;

                    this.radGridViewComprobantes.Rows[indice].Cells["Tasa"].Value = dr["TASCIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["NoMovimiento"].Value = dr["SIMIIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["FECOIC"].Value = dr["FECOIC"].ToString().Trim();
                    this.radGridViewComprobantes.Rows[indice].Cells["STATIC"].Value = dr["STATIC"].ToString().Trim();
                }
                else
                {
                    this.radGridViewComprobantes.Rows[indice].Cells["Estado"].Value = "(ya no existe)";
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
            }
        }

        /// <summary>
        /// Cambia de estado un comprobante 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="indice"></param>
        /// <param name="STATL2"></param>
        /// <param name="CRTDL2"></param>
        private void CambiarEstado(GridViewRowInfo row, int indice, string STATL2, string CRTDL2)
        {
            try
            {
                string compania = row.Cells["compania"].Value.ToString();
                string aapp = row.Cells["SAPRIC"].Value.ToString().Trim();
                string tipo = row.Cells["Tipo"].Value.ToString();
                string noComp = row.Cells["NoComp"].Value.ToString();

                string query = "";
                int reg = 0;

                if (CRTDL2 == "1")
                {
                    //Eliminar el comprobante de la tabla GLAI3
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLAI3 ";
                    query += "where CCIAAD ='" + compania;
                    query += "' and SAPRAD =" + aapp;
                    query += " and TICOAD =" + tipo;
                    query += " and NUCOAD =" + noComp;
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Eliminar el comprobante de la tabla de encabezados de comrpobantes GLI03
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "where CCIAIC ='" + compania;
                    query += "' and SAPRIC =" + aapp;
                    query += " and TICOIC =" + tipo;
                    query += " and NUCOIC =" + noComp;
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Eliminar el comprobante de la tabla GLB01
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLB01 ";
                    query += "where CCIADT ='" + compania;
                    query += "' and SAPRDT =" + aapp;
                    query += " and TICODT =" + tipo;
                    query += " and NUCODT =" + noComp;
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                    //Eliminar el comprobante de la tabla GLBX1
                    query = "delete from " + GlobalVar.PrefijoTablaCG + "GLBX1 ";
                    query += "where CCIADX ='" + compania;
                    query += "' and SAPRDX =" + aapp;
                    query += " and TICODX =" + tipo;
                    query += " and NUCODX =" + noComp;
                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
                else
                {
                    //Actualizar la tabla que contiene la cabecera del comprobante
                    query = "update  " + GlobalVar.PrefijoTablaCG + "GLI03 ";
                    query += "set STATIC ='" + STATL2 + "' ";
                    query += "where CCIAIC ='" + compania;
                    query += "' and SAPRIC =" + aapp;
                    query += " and TICOIC =" + tipo;
                    query += " and NUCOIC =" + noComp;

                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }

                if (reg == 1)
                {
                    //Actualizar el estado de la fila del Grid de Comprobantes
                    if (CRTDL2 == "1")
                    {
                        this.radGridViewComprobantes.SelectedRows[indice].Cells["Estado"].Value = "(" + this.LP.GetText("errYaNoExiste", "ya no existe") + ")";
                    }
                    else
                    {
                        this.radGridViewComprobantes.SelectedRows[indice].Cells["STATIC"].Value = STATL2;
                        this.radGridViewComprobantes.SelectedRows[indice].Cells["Estado"].Value = this.ObtenerEstadoComprobante(STATL2).Trim();
                    }

                    this.radGridViewComprobantes.Refresh();

                    string fecha = DateTime.Now.ToString("yy/MM/dd");
                    string aa = "";
                    if (fecha != "" && fecha.Length >= 2) aa = fecha.Substring(0, 2);
                    string siglo = "1";
                    if (aa != "") siglo = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC);
                    fecha = siglo + fecha.Replace("/", "");

                    string hora = DateTime.Now.ToString("hh:mm:ss");
                    hora = hora.Replace(":", "");

                    string userWindowsLogado = System.Environment.UserName.ToUpper();

                    //Insertar la acción en la tabla GLL02
                    query = "insert into " + GlobalVar.PrefijoTablaCG + "GLL02 ";
                    query += "(DATEL2, TIMEL2, IDUSL2, IBMUL2, STATL2, CCIAL2, SAPRL2, TICOL2, NUCOL2, CRTDL2) values (";
                    query += fecha + ", " + hora + ", '" + GlobalVar.UsuarioLogadoCG + "', '" + userWindowsLogado + "', '";
                    query += STATL2 + "', '" + compania + "', " + aapp + ", " + tipo + ", " + noComp + ", '" + CRTDL2 + "')";

                    reg = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Devuelve la cadena con el mensaje de Acción no permitida para el comprobante y la clave del comprobante
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string AccionNoPermitidaMsg(GridViewRowInfo row, string codAccion)
        {
            string mensaje = this.LP.GetText("errAccionNoPermitida", "Acción no permitida para el Comprobante");
            try
            {
                string accionNombre = this.AccionNombre(codAccion);
                mensaje = this.LP.GetText("lblAccion", "Acción") + " " + accionNombre + " " + this.LP.GetText("lblAccionNoPermitida", "no permitida para el Comprobante");

                string compania = row.Cells["compania"].Value.ToString();
                string aapp = row.Cells["SAPRIC"].Value.ToString().Trim();
                if (aapp != "")
                {
                    if (aapp.Length == 5) aapp = aapp.Substring(1, 4);
                    else if (aapp.Length < 4) aapp = aapp.PadRight(4, ' ');
                    aapp = aapp.Substring(0, 2) + "-" + aapp.Substring(2, 2);

                }
                string tipo = row.Cells["Tipo"].Value.ToString();
                string noComp = row.Cells["NoComp"].Value.ToString();

                mensaje += "\n\r   " + this.LP.GetText("lblCompania", "Compañía") + ": " + compania + "\n\r   ";
                mensaje += this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") + ": " + aapp + "\n\r   ";
                mensaje += this.LP.GetText("lblTipo", "Tipo") + ": " + tipo + "\n\r   " + this.LP.GetText("lblNumero", "Número") + ": " + noComp;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (mensaje);
        }


        /// <summary>
        /// Inicializa los valores por defecto del objeto XContro (para contabilizar y revertir)
        /// </summary>
        /// <param name="xcontro">Objeto XContro</param>
        /// <param name="accion">codAccion (C->contabilizar, RE->revertir)</param>
        /// <param name="row">fila cabecera del comprobante</param>
        /// <param name="plan">plan de la compañía</param>
        private void InicializaXContro(ref XContro xcontro, string codAccion, GridViewRowInfo row, string plan)
        {
            try
            {
                xcontro.ColaName = ColasName.CGPOST;
                xcontro.Usuario = System.Environment.UserName.ToUpper();
                xcontro.Prioridad = "20";
                xcontro.ProcTipo = "E";
                xcontro.ProcName = "CG012";

                xcontro.DescName = ObtenerDescXContro(codAccion);   //(Contabilizar comprobante / Revertir comprobante)

                switch (codAccion)
                {
                    case "C":
                        if (xcontro.DescName == "") xcontro.DescName = this.LP.GetText("lblContabilizarComp", "Contabilizar comprobante");
                        xcontro.LDA_Variable = this.ObtenerLDAVariableContabilizar(ref xcontro, row, plan);
                        break;
                    case "AC":
                        if (xcontro.DescName == "") xcontro.DescName = this.LP.GetText("lblContabilizarComp", "Contabilizar comprobante");
                        xcontro.LDA_Variable = this.ObtenerLDAVariableContabilizar(ref xcontro, row, plan);
                        break;
                    case "RE":
                        if (xcontro.DescName == "") xcontro.DescName = this.LP.GetText("lblRevertirComp", "Revertir comprobante");
                        xcontro.LDA_Variable = this.ObtenerLDAVariableRevertir(ref xcontro, row, plan);
                        break;
                }

                xcontro.Status = "Pendiente";
                xcontro.Pid = "0";
                xcontro.PidMenu = "0";
                xcontro.PidPanta = " ";

                xcontro.Parm = "@[PARA]";
                xcontro.LDA_BTCLDA = "CG753";
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Obtiene la parte variable de la lda para el proceso de contabilizar
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerLDAVariableComunContabilizarRevertir(ref XContro xcontro, GridViewRowInfo row, string plan)
        {
            string ldaVariable = "";

            try
            {
                ldaVariable = row.Cells["compania"].Value.ToString().PadRight(3, ' ');
                ldaVariable += plan.PadRight(1, ' ');

                string saapp = row.Cells["AAPP"].Value.ToString();
                saapp = saapp.Replace("-", "");
                if (saapp.Length < 5 && saapp.Length >= 2)
                {
                    //Coger el siglo
                    string siglo = utiles.SigloDadoAnno(saapp.Substring(0, 2), CGParametrosGrles.GLC01_ALSIRC);
                    saapp = siglo + saapp;
                }
                saapp = saapp.PadRight(5, ' ');

                ldaVariable += saapp;
                ldaVariable += saapp;
                ldaVariable += row.Cells["Tipo"].Value.ToString().PadLeft(2, '0');
                ldaVariable += row.Cells["NoComp"].Value.ToString().PadLeft(5, '0');
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (ldaVariable);
        }

        /// <summary>
        /// Obtiene la parte variable de la lda para el proceso de contabilizar
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerLDAVariableContabilizar(ref XContro xcontro, GridViewRowInfo row, string plan)
        {
            string ldaVariable = ObtenerLDAVariableComunContabilizarRevertir(ref xcontro, row, plan);

            ldaVariable += " ".PadRight(10, ' ');
            ldaVariable += " ".PadRight(8, ' ');
            ldaVariable += " ".PadRight(826, ' ');
            ldaVariable += "GL220".PadRight(10, ' ');

            return (ldaVariable);
        }

        /// <summary>
        /// Obtiene la parte variable de la lda para el proceso de revertir
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerLDAVariableRevertir(ref XContro xcontro, GridViewRowInfo row, string plan)
        {
            string ldaVariable = ObtenerLDAVariableComunContabilizarRevertir(ref xcontro, row, plan);

            ldaVariable += "N";
            ldaVariable += "V";
            ldaVariable += "N";
            ldaVariable += " ".PadRight(841, ' ');
            ldaVariable += "AU221".PadRight(10, ' ');

            return (ldaVariable);
        }

        /// <summary>
        /// Obtiene la descripción para la acción solicitada que se busca en la tabla XPARA y se almacenará en la tabla XCONTRO
        /// </summary>
        /// <param name="xcontro"></param>
        /// <returns></returns>
        private string ObtenerDescXContro(string codAccion)
        {
            string desc = "";
            IDataReader dr = null;
            try
            {
                string programa = "";
                switch (codAccion)
                {
                    case "C":
                    case "AC":
                        programa = "GL220";
                        break;
                    case "RE":
                        programa = "AU221";
                        break;
                }

                string prefijoTabla = "";
                if (tipoBaseDatosCG == "DB2")
                {
                    prefijoTabla = ConfigurationManager.AppSettings["bbddCGUF"];
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
        /// Chequear que si la accion ya ha sido enviada a realizar (si es la última accion de la Grid de Acciones)
        /// </summary>
        /// <param name="codAccion"></param>
        /// <returns></returns>
        private bool AccionNoRealizada(GridViewRowInfo rowComp, string codAccion)
        {
            bool result = true;

            try
            {
                string ultAccion = "";

                string tipo = rowComp.Cells["Tipo"].Value.ToString();
                string noComp = rowComp.Cells["NoComp"].Value.ToString();

                string tipoActual = "";
                string noCompActual = "";

                for (int i = 0; i < this.radGridViewAccionesActuales.Rows.Count; i++)
                {
                    tipoActual = this.radGridViewAccionesActuales.Rows[i].Cells["Tipo"].Value.ToString();
                    noCompActual = this.radGridViewAccionesActuales.Rows[i].Cells["NoComp"].Value.ToString();

                    if (tipoActual == tipo && noCompActual == noComp)
                    {
                        ultAccion = this.radGridViewAccionesActuales.Rows[i].Cells["CodAccion"].Value.ToString();
                    }
                }

                if (ultAccion == codAccion) result = false;
                else if (ultAccion == "AC" && codAccion == "C") result = false;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Verifica si el AAPP solicitado para la compañía está cerrado o no
        /// </summary>
        /// <returns></returns>
        private bool VerificarAAPPEstaCerrado()
        {
            bool result = false;

            try
            {
                string sap = "";

                //coger el valor sin la máscara
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                sap = this.txtMaskAAPP.Value.ToString();
                this.txtMaskAAPP.TextMaskFormat = MaskFormat.IncludeLiterals;

                //Buscar el siglo dado el año

                string aa = sap.Substring(0, 2);
                sap = utiles.SigloDadoAnno(aa, CGParametrosGrles.GLC01_ALSIRC) + sap;

                return (utilesCG.SAAPPCerrado(this.codCompania, sap));
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Edita el comprobante seleccionado
        /// </summary>
        private void EditarComprobante()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.radGridViewComprobantes.CurrentRow is GridViewGroupRowInfo)
                {
                    if (this.radGridViewComprobantes.CurrentRow.IsExpanded) this.radGridViewComprobantes.CurrentRow.IsExpanded = false;
                    else this.radGridViewComprobantes.CurrentRow.IsExpanded = true;
                }
                else if (this.radGridViewComprobantes.CurrentRow is GridViewDataRowInfo)
                {
                    if (this.radGridViewComprobantes.SelectedRows.Count == 1)
                    {
                        GridViewRowInfo row = this.radGridViewComprobantes.SelectedRows[0];

                        //Chequear autorizaciones
                        string compania = row.Cells["compania"].Value.ToString();
                        string tipo = row.Cells["tipo"].Value.ToString();

                        //Chequear autorización sobre compañía
                        bool autorizadoEditarComp = aut.Validar("002", "02", compania, "20");
                        //Chequear autorización sobre tipo de comprobante
                        bool autorizadoEditarTipo = aut.Validar("004", "03", tipo, "20");

                        if (!autorizadoEditarComp && !autorizadoEditarTipo) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutEditarCompTipo", "Usuario no autorizado a editar comprobantes de esta compañía y tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        else if (!autorizadoEditarComp) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutEditarComp", "Usuario no autorizado a editar comprobantes de la compañía seleccionada"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        else if (!autorizadoEditarTipo) RadMessageBox.Show(this.LP.GetText("lblErrUserNoAutEditarTipo", "Usuario no autorizado a editar comprobantes del tipo seleccionado"), this.LP.GetText("errValTitulo", "Error")); //Falta traducir
                        else
                        {
                            this.CargarComprobanteSeleccionado(row);
                        }
                    }
                    else RadMessageBox.Show(this.LP.GetText("lblErrSelSoloUnComp", "Debe seleccionar un solo comprobante"), this.LP.GetText("errValTitulo", "Error"));
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

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

        
        private void radPanelApp_Resize(object sender, EventArgs e)
        {
            //this.radGridViewComprobantes.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 221);
            this.radGridViewComprobantes.Size = new Size(this.radPanelApp.Size.Width - 66, this.radPanelApp.Size.Height - 221);
        }
    }
}