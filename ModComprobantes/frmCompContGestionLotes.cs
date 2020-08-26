using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ObjectModel;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ModComprobantes
{
    public enum GestionLotesUserInterfaces
    {
        EdicionLotes = 1,   //Edicion de lotes
        EdicionLotesProcesados = 2, //Edicion de lotes de errores
        EdicionLotesProcesadosComp = 3	//Edicion de comprobantes de lotes de errores
    }

    public partial class frmCompContGestionLotes : frmPlantilla, IReLocalizable
    {
        private string tipoBaseDatosCG;

        private bool bbddDB2 = false;
        private string bibliotecaTablasLoteAS = "";

        private GestionLotesUserInterfaces UI;
        private bool mostrarLotesTodos = false;
        private bool lotesErrorSizeGrande = true;

        private int tgGridLotesErroresRowSel;

        bool existeTablaW30;
        bool existeTablaW31;
        bool existeTablaW40;
        bool existeTablaW41;

        private static bool primeraLlamada = true;
        private static Point gridInfoLocation = new Point(19, 28);
        //private static Size gridInfoSize = new Size(833, 445);
        private static Size gridInfoSize = new Size(813, 529);
        private static int radCollapsiblePanelBuscadorExpandedHeight = 0;

        Dictionary<string, string> displayNamesEditarLotes;
        private DataTable dtEditarLote;

        Dictionary<string, string> displayNamesLotesError;
        private DataTable dtLotesError;

        Dictionary<string, string> displayNamesCompErrores;
        private DataTable dtCompErrores;

        public frmCompContGestionLotes()
        {
            InitializeComponent();

            radCollapsiblePanelBuscadorExpandedHeight = this.radCollapsiblePanelBuscador.Height;
            this.radCollapsiblePanelBuscador.IsExpanded = false;
            this.radCollapsiblePanelBuscador.EnableAnimation = false;

            this.gbEdicionLotes.ElementTree.EnableApplicationThemeName = false;
            this.gbEdicionLotes.ThemeName = "ControlDefault";

            this.gbGridEditarLotes.ElementTree.EnableApplicationThemeName = false;
            this.gbGridEditarLotes.ThemeName = "ControlDefault";

            this.gbGridLotesErrores.ElementTree.EnableApplicationThemeName = false;
            this.gbGridLotesErrores.ThemeName = "ControlDefault";

            this.gbGridCompErrores.ElementTree.EnableApplicationThemeName = false;
            this.gbGridCompErrores.ThemeName = "ControlDefault";

            this.gbSuprimirHco.ElementTree.EnableApplicationThemeName = false;
            this.gbSuprimirHco.ThemeName = "ControlDefault";

            this.gbBuscador.ElementTree.EnableApplicationThemeName = false;
            this.gbBuscador.ThemeName = "ControlDefault";

            this.radToggleSwitchFormatoAmpliado.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchFormatoAmpliado.ThemeName = "MaterialBlueGrey";

            this.radToggleSwitchBuscadorMostrarTodos.ElementTree.EnableApplicationThemeName = false;
            this.radToggleSwitchBuscadorMostrarTodos.ThemeName = "MaterialBlueGrey";
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void FrmCompContGestionLotes_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Gestión de Lotes");

            //Habilitar Edicion de Lotes
            this.gbEdicionLotes.Visible = true;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.gbGridEditarLotes.Visible = false;
            this.radGridViewEditarLotes.Visible = false;
            this.gbGridLotesErrores.Visible = false;
            this.radGridViewLotesErrores.Visible = false;
            this.gbGridCompErrores.Visible = false;
            this.radGridViewCompErrores.Visible = false;
            this.gbSuprimirHco.Visible = false;
            this.lblInfo.Visible = false;
            this.gbBuscador.Visible = false;
            this.radCollapsiblePanelBuscador.Visible = false;

            //Si no es AS inhabilitar bibilioteca
            this.tipoBaseDatosCG = ConfigurationManager.AppSettings["tipoBaseDatosCG"];
            if (tipoBaseDatosCG == "DB2") this.bbddDB2 = true;
            else this.bbddDB2 = false;

            if (this.bbddDB2) this.txtBiblioteca.Enabled = true;
            else this.txtBiblioteca.Enabled = false;

            //Crear el data grid para la edición de lotes
            this.BuildDataGridtgGridEditarLotes();

            //Crear el data grid para los lotes con errores
            this.BuildDataGridtgGridLotesErrores();

            //Crear el data grid para los comprobantes con errores
            this.BuildDataGridtgGridCompErrores();

            //Traducir los literales del formulario
            this.TraducirLiterales();

            this.UI = GestionLotesUserInterfaces.EdicionLotes;
            this.tgGridLotesErroresRowSel = -1;

            utiles.ButtonEnabled(ref this.radButtonEdicionLotes, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
            this.radButtonSuprimirHco.Visible = false;

            this.AcceptButton = this.btnAceptar;

            //this.txtBuscadorPrefijo.Select();
            this.txtPrefijo.Focus();
            this.txtPrefijo.Select();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            //Validar que existan las tablas de lotes
            string result = this.FormValid();
            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(result, error);
                return;
            }

            //Chequear si hay comprobantes en los lotes de errores
            bool extendido = false;
            if (this.radToggleSwitchFormatoAmpliado.Value) extendido = true;

            string prefijo = this.txtPrefijo.Text.Trim();
            string biblioteca = "";

            if (this.bbddDB2) biblioteca = this.txtBiblioteca.Text.Trim();

            //Verificar si existen las tablas de Lotes de errores, para traspasar los comprobantes si lo confirma
            this.VerificarExistenTablasLotesErroneos(prefijo, biblioteca, extendido);

            this.TraspasarComprobantesErroneosDeTabla(prefijo, biblioteca, extendido, true);

            //Cargar los datos de la edicion de lotes
            this.FillDataGridtgGridEditarLotes();

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void TxtPrefijo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtBiblioteca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void RadGridViewEditarLotes_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarLote();
        }

        private void BtnHcoMensajesAceptar_Click(object sender, EventArgs e)
        {
            int fechaHasta = 0;
            string result = this.FormValidHcoMensajes(ref fechaHasta);

            if (result != "")
            {
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(result, error);
                return;
            }

            //Pedir confirmacion
            string mensaje = this.LP.GetText("confEliminarMensajesError", "Se van a eliminar todos los mensajes de error hasta la fecha indicada. ¿Desea continuar?");
            DialogResult resultConf = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
            if (resultConf == DialogResult.Yes)
            {
                result = this.EliminarMensajesError(fechaHasta, "", "", "", "");

                //Volver a cargar la grid
                this.gbSuprimirHco.Visible = false;
                this.txtBuscadorBiblio.Text = "";
                this.txtBuscadorPrefijo.Text = "";
                this.txtBuscadorDescripcion.Text = "";
                this.txtBuscadorUsuario.Text = "";
                this.txtMaskBuscadorFechaDesde.Value = null;
                this.txtMaskBuscadorFechaHasta.Value = null;
                this.txtMaskHastaFecha.Value = null;
                this.gbBuscador.Visible = true;
                this.radCollapsiblePanelBuscador.Visible = true;
                utiles.ButtonEnabled(ref this.radButtonGestionLotesProc, false);
                utiles.ButtonEnabled(ref radButtonSuprimirHco, true);
                this.dtLotesError.Clear();
                this.FillDataGridtgGridLotesErrores();
            }
        }

        private void BtnBuscadorBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Filtrar la Grid
                string filtro = "";
                string prefijo = "";
                string biblioteca = "";
                string usuario = "";
                string fechaDesde = "";
                string fechaHasta = "";
                string descripcion = "";

                prefijo = this.txtBuscadorPrefijo.Text.Trim();

                if (this.txtBuscadorBiblio.Enabled) biblioteca = this.txtBuscadorBiblio.Text.Trim();

                usuario = this.txtBuscadorUsuario.Text.Trim();

                string errorTitulo = this.LP.GetText("errValTitulo", "Error");
                DateTime dt;
                int fechaDesdeFormatoCG = 0;
                int fechaHastaFormatoCG = 0;

                this.txtMaskBuscadorFechaDesde.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                fechaDesde = this.txtMaskBuscadorFechaDesde.Value.ToString();
                this.txtMaskBuscadorFechaDesde.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (fechaDesde != "")
                {
                    fechaDesde = this.txtMaskBuscadorFechaDesde.Text.Trim();
                    try
                    {
                        dt = Convert.ToDateTime(fechaDesde);
                        fechaDesdeFormatoCG = utiles.FechaToFormatoCG(dt, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        RadMessageBox.Show(this.LP.GetText("lblErrFechaDebeFormato", "La fecha desde no tiene un formato válido"), errorTitulo);
                        this.txtMaskBuscadorFechaDesde.Focus();
                        return;
                    }
                }

                this.txtMaskBuscadorFechaHasta.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                fechaHasta = this.txtMaskBuscadorFechaHasta.Value.ToString();
                this.txtMaskBuscadorFechaHasta.TextMaskFormat = MaskFormat.IncludeLiterals;
                if (fechaHasta != "")
                {
                    fechaHasta = this.txtMaskBuscadorFechaHasta.Text.Trim();
                    try
                    {
                        dt = Convert.ToDateTime(fechaHasta);
                        fechaHastaFormatoCG = utiles.FechaToFormatoCG(dt, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Utiles.CreateExceptionString(ex));

                        RadMessageBox.Show(this.LP.GetText("lblErrFechaHastaFormato", "La fecha hasta no tiene un formato válido"), errorTitulo);
                        this.txtMaskBuscadorFechaHasta.Focus();
                        return;
                    }
                }

                descripcion = this.txtBuscadorDescripcion.Text.Trim();

                if (prefijo != "")
                {
                    filtro += string.Format("PREF24 LIKE '%{0}%'", prefijo);
                }

                if (biblioteca != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("LIBL24 LIKE '%{0}%'", biblioteca);
                }

                if (descripcion != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("DESC24 LIKE '%{0}%'", descripcion);
                }

                if (usuario != "")
                {
                    if (filtro != "") filtro += " AND ";
                    filtro = string.Format("USER24 LIKE '%{0}%'", usuario);
                }

                if (fechaDesde != "")
                {
                    fechaDesdeFormatoCG = 9999999 - fechaDesdeFormatoCG;

                    if (fechaHasta != "")
                    {
                        fechaHastaFormatoCG = 9999999 - fechaHastaFormatoCG;
                        //fecha entre desde y hasta
                        if (filtro != "") filtro += " AND ";
                        //filtro += string.Format("DATE24Origen >= '{0}' AND DATE24Origen <= '{1}'", fechaDesdeFormatoCG.ToString(), fechaHastaFormatoCG.ToString());
                        filtro += string.Format("DATE24Origen >= '{0}' AND DATE24Origen <= '{1}'", fechaHastaFormatoCG.ToString(), fechaDesdeFormatoCG.ToString());
                    }
                    else
                    {
                        //fecha mayor o igual que desde
                        if (filtro != "") filtro += " AND ";
                        filtro += string.Format("DATE24Origen <= '{0}'", fechaDesdeFormatoCG.ToString());
                    }
                }
                else
                    if (fechaHasta != "")
                {
                    fechaHastaFormatoCG = 9999999 - fechaHastaFormatoCG;
                    //fecha menor o igual que hasta
                    if (filtro != "") filtro += " AND ";
                    filtro += string.Format("DATE24Origen >= '{0}'", fechaHastaFormatoCG.ToString());
                }


                if (!this.mostrarLotesTodos && this.radToggleSwitchBuscadorMostrarTodos.Value)
                {
                    this.mostrarLotesTodos = true;
                    //Buscar todos los lotes procesados (mostrar el histórico)
                    FillDataGridtgGridLotesErrores();
                }
                else
                {
                    if (this.mostrarLotesTodos && !this.radToggleSwitchBuscadorMostrarTodos.Value)
                    {
                        if (filtro != "") filtro += " AND ";
                        filtro += "NUEV24 = 1";
                        this.mostrarLotesTodos = false;
                    }
                }

                this.dtLotesError.DefaultView.RowFilter = filtro;
                this.radGridViewLotesErrores.Refresh();

                if (this.radGridViewLotesErrores!= null && this.radGridViewLotesErrores.Rows.Count == 0 && filtro != "")
                {
                    this.gbGridLotesErrores.Visible = false;
                    this.radGridViewLotesErrores.Visible = false;

                    string error = this.LP.GetText("errValTitulo", "Error");
                    RadMessageBox.Show(this.LP.GetText("lblErrNoLotes", "No existen lotes procesados para el criterio de búsqueda utilizado"), error);
                    this.gbBuscador.Select();
                }
                else
                {
                    this.gbGridLotesErrores.Visible = true;
                    this.radGridViewLotesErrores.Visible = true;
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        private void BtnBuscadorTodos_Click(object sender, EventArgs e)
        {
            this.txtBuscadorPrefijo.Text = "";
            this.txtBuscadorBiblio.Text = "";
            this.txtBuscadorDescripcion.Text = "";
            this.txtBuscadorUsuario.Text = "";
            this.txtMaskBuscadorFechaDesde.Value = null;
            this.txtMaskBuscadorFechaHasta.Value = null;
            this.radToggleSwitchBuscadorMostrarTodos.Value = false;

            //this.chkNoTransferidos.Checked = false;
            this.dtLotesError.DefaultView.RowFilter = "";

            this.gbGridLotesErrores.Visible = true;
            this.radGridViewLotesErrores.Visible = true;

            this.radCollapsiblePanelBuscador.Collapse();

            this.Refresh();
        }

        private void TxtBuscadorPrefijo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void TxtBuscadorBiblio_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void BtnHcoMensajesCancelar_Click(object sender, EventArgs e)
        {
            this.gbSuprimirHco.Visible = false;
            //this.gbSuprimirHco.SendToBack();

            //Llamar a la gestión de lotes procesados
            this.GestionLotesProcesados();
        }

        private void RadButtonEdicionLotes_Click(object sender, EventArgs e)
        {
            //Habilitar Interface de Edicion de Lotes
            this.UI = GestionLotesUserInterfaces.EdicionLotes;

            //Habilitar Edicion de Lotes
            this.gbEdicionLotes.Visible = true;
            this.gbEdicionLotes.BringToFront();
            utiles.ButtonEnabled(ref this.radButtonGestionLotesProc, true);
            utiles.ButtonEnabled(ref this.radButtonEdicionLotes, false);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
            this.radButtonSuprimirHco.Visible = false;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.radCollapsiblePanelBuscador.Visible = false;
            this.gbBuscador.Visible = false;
            this.radCollapsiblePanelBuscador.Visible = false;
            this.gbGridEditarLotes.Visible = false;
            this.radGridViewEditarLotes.Visible = false;
            this.gbGridLotesErrores.Visible = false;
            this.radGridViewLotesErrores.Visible = false;
            this.gbGridCompErrores.Visible = false;
            this.radGridViewCompErrores.Visible = false;
            this.lblInfo.Visible = false;

            //Ocultar Suprimir histórico de Mensajes
            this.gbSuprimirHco.Visible = false;

            if (this.bbddDB2) this.txtBiblioteca.Enabled = true;
            else this.txtBiblioteca.Enabled = false;

            this.AcceptButton = this.btnAceptar;

            this.txtPrefijo.Focus();
        }

        private void RadButtonGestionLotesProc_Click(object sender, EventArgs e)
        {
            this.GestionLotesProcesados();
        }

        private void RadButtonEditar_Click(object sender, EventArgs e)
        {
            this.EditarLote();
        }

        private void RadButtonSuprimir_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            string resultEliminar = "";
            string fecha = "";
            string hora = "";
            string biblioteca = "";
            string prefijo = "";

            string compania = "";
            string ano = "";
            string periodo = "";
            string tipo = "";
            string numeroComp = "";

            switch (this.UI)
            {
                case GestionLotesUserInterfaces.EdicionLotes:
                    //Suprimir Comprobante Lote
                    if (this.radGridViewEditarLotes.SelectedRows.Count > 0)
                    {
                        //Pedir confirmacion
                        string mensaje = this.LP.GetText("confEliminarApuntesComp", "Se van a eliminar todos los apuntes de los comprobantes seleccionados. ¿Desea continuar?");
                        DialogResult resultConf = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (resultConf == DialogResult.Yes)
                        {
                            using (this.radGridViewEditarLotes.DeferRefresh())
                            {
                                foreach (Telerik.WinControls.UI.GridViewRowInfo row in this.radGridViewEditarLotes.SelectedRows)
                                {
                                    compania = row.Cells["CCIAWS"].Value.ToString();
                                    if (tipoBaseDatosCG == "DB2")
                                        //ano = row.Cells["AÑOCWS"].Value.ToString();
                                        ano = row.Cells[2].Value.ToString();
                                    else
                                        ano = row.Cells["AVOCWS"].Value.ToString();
                                    //periodo = row.Cells["LAPSWS"].Value.ToString().Trim();
                                    periodo = row.Cells[1].Value.ToString().Substring(3, 2);
                                    tipo = row.Cells["TICOWS"].Value.ToString();
                                    numeroComp = row.Cells["NUCOWS"].Value.ToString();

                                    resultEliminar = this.EliminarApuntesComp(compania, ano, periodo, tipo, numeroComp);

                                    this.radGridViewEditarLotes.Rows.Remove(row);
                                }
                            }
                        }
                    }
                    else if (this.tgGridLotesErroresRowSel != -1)
                    {
                        fecha = this.radGridViewEditarLotes.Rows[this.tgGridLotesErroresRowSel].Cells["DATE24Origen"].Value.ToString();
                        hora = this.radGridViewEditarLotes.Rows[this.tgGridLotesErroresRowSel].Cells["TIME24Origen"].Value.ToString();
                        biblioteca = this.radGridViewEditarLotes.Rows[this.tgGridLotesErroresRowSel].Cells["LIBL24"].Value.ToString().Trim();
                        prefijo = this.radGridViewEditarLotes.Rows[this.tgGridLotesErroresRowSel].Cells["PREF24"].Value.ToString();

                        resultEliminar = this.EliminarMensajesError(0, prefijo, biblioteca, fecha, hora);

                        this.radGridViewEditarLotes.Rows.Remove(this.radGridViewEditarLotes.Rows[this.tgGridLotesErroresRowSel]);
                        this.radGridViewEditarLotes.Update();
                    }
                    else
                    {
                        //error
                    }
                    break;
                case GestionLotesUserInterfaces.EdicionLotesProcesados:
                    //Suprimir Histórico de Lotes
                    if (this.radGridViewLotesErrores.SelectedRows.Count > 0)
                    {
                        //Pedir confirmacion
                        string mensaje = this.LP.GetText("confEliminarMensajesErrorSel", "Se van a eliminar todos los mensajes de error seleccionados. ¿Desea continuar?");
                        DialogResult resultConf = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (resultConf == DialogResult.Yes)
                        {
                            using (this.radGridViewLotesErrores.DeferRefresh())
                            {
                                foreach (Telerik.WinControls.UI.GridViewRowInfo row in this.radGridViewLotesErrores.SelectedRows)
                                {
                                    fecha = row.Cells["DATE24Origen"].Value.ToString();
                                    hora = row.Cells["TIME24Origen"].Value.ToString();
                                    biblioteca = row.Cells["LIBL24"].Value.ToString().Trim();
                                    prefijo = row.Cells["PREF24"].Value.ToString();

                                    resultEliminar = this.EliminarMensajesError(0, prefijo, biblioteca, fecha, hora);

                                    this.radGridViewLotesErrores.Rows.Remove(row);   
                                }
                            }

                            this.radGridViewLotesErrores.Update();
                        }
                    }
                    else if (this.tgGridLotesErroresRowSel != -1)
                    {
                        fecha = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["DATE24Origen"].Value.ToString();
                        hora = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["TIME24Origen"].Value.ToString();
                        biblioteca = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["LIBL24"].Value.ToString().Trim();
                        prefijo = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["PREF24"].Value.ToString();

                        resultEliminar = this.EliminarMensajesError(0, prefijo, biblioteca, fecha, hora);

                        this.radGridViewLotesErrores.Rows.Remove(this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel]);
                        this.radGridViewLotesErrores.Update();
                    }
                    else
                    {
                        //error
                    }
                    break;
                case GestionLotesUserInterfaces.EdicionLotesProcesadosComp:
                    //Editar Comprobante Error
                    if (this.tgGridLotesErroresRowSel != -1)
                    {

                        fecha = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["DATE24Origen"].Value.ToString();
                        hora = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["TIME24Origen"].Value.ToString();
                        biblioteca = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["LIBL24"].Value.ToString().Trim();
                        prefijo = this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel].Cells["PREF24"].Value.ToString();

                        resultEliminar = this.EliminarMensajesError(0, prefijo, biblioteca, fecha, hora);

                        this.radGridViewLotesErrores.Rows.Remove(this.radGridViewLotesErrores.Rows[this.tgGridLotesErroresRowSel]);
                        this.radGridViewLotesErrores.Update();

                        if (this.radGridViewCompErrores.Visible)
                        {
                            this.gbGridCompErrores.Visible = false;
                            this.radGridViewCompErrores.Visible = false;
                            this.MostrarOcultarGridCompErrores();
                        }
                    }
                    else
                    {
                        //error
                    }
                    break;
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonSuprimirHco_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            this.gbBuscador.Visible = false;
            this.radCollapsiblePanelBuscador.Visible = false;
            this.gbGridLotesErrores.Visible = false;
            this.gbGridCompErrores.Visible = false;
            this.gbSuprimirHco.Visible = true;

            utiles.ButtonEnabled(ref this.radButtonEdicionLotes, true);
            utiles.ButtonEnabled(ref this.radButtonGestionLotesProc, true);
            utiles.ButtonEnabled(ref this.radButtonEditar, false);
            utiles.ButtonEnabled(ref this.radButtonSuprimir, false);
            utiles.ButtonEnabled(ref this.radButtonSuprimirHco, false);

            this.txtMaskHastaFecha.Focus();

            this.AcceptButton = this.btnHcoMensajesAceptar;

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void RadButtonEdicionLotes_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonEdicionLotes);
        }

        private void RadButtonEdicionLotes_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonEdicionLotes);
        }

        private void RadButtonGestionLotesProc_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonGestionLotesProc);
        }

        private void RadButtonGestionLotesProc_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonGestionLotesProc);
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

        private void RadButtonSuprimirHco_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.radButtonSuprimirHco);
        }

        private void RadButtonSuprimirHco_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.radButtonSuprimirHco);
        }

        private void BtnAceptar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnAceptar);
        }

        private void BtnAceptar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnAceptar);
        }

        private void BtnBuscadorTodos_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnBuscadorTodos);
        }

        private void BtnBuscadorTodos_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnBuscadorTodos);
        }

        private void BtnBuscadorBuscar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnBuscadorBuscar);
        }

        private void BtnBuscadorBuscar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnBuscadorBuscar);
        }

        private void RadCollapsiblePanelBuscador_Expanded(object sender, EventArgs e)
        {
            if (this.gbGridLotesErrores.Visible)
            {
                this.gbGridLotesErrores.Location = new Point(this.gbGridLotesErrores.Location.X, gridInfoLocation.Y + radCollapsiblePanelBuscadorExpandedHeight);
                this.gbGridLotesErrores.Size = new Size(this.gbGridLotesErrores.Size.Width, this.gbGridLotesErrores.Size.Height - radCollapsiblePanelBuscadorExpandedHeight);

                this.txtBuscadorPrefijo.Focus();
                this.txtBuscadorPrefijo.Select();
                this.gbGridLotesErrores.Refresh();
            }
        }

        private void RadCollapsiblePanelBuscador_Collapsed(object sender, EventArgs e)
        {
            if (primeraLlamada) primeraLlamada = false;
            else
            {
                this.gbGridLotesErrores.Location = gridInfoLocation;
                //Es necesario volver asignar el mismo valor porque en la primera ejecución no lo asigna bien (resta 54 al valor inicial)
                this.gbGridLotesErrores.Location = gridInfoLocation;

                this.gbGridLotesErrores.Size = gridInfoSize;
            }
        }

        private void RadGridViewEditarLotes_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.bbddDB2)
            {
                if (this.radGridViewEditarLotes.Columns.Contains("AÑOCWS")) this.radGridViewEditarLotes.Columns["AÑOCWS"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewEditarLotes.Columns.Contains("AVOCWS")) this.radGridViewEditarLotes.Columns["AVOCWS"].IsVisible = false;
            }
            if (this.radGridViewEditarLotes.Columns.Contains("LAPSWS")) this.radGridViewEditarLotes.Columns["LAPSWS"].IsVisible = false;
            if (this.radGridViewEditarLotes.Columns.Contains("DIAEWS")) this.radGridViewEditarLotes.Columns["DIAEWS"].IsVisible = false;
            if (this.radGridViewEditarLotes.Columns.Contains("MESEWS")) this.radGridViewEditarLotes.Columns["MESEWS"].IsVisible = false;
            if (this.bbddDB2)
            {
                if (this.radGridViewEditarLotes.Columns.Contains("AÑOEWS")) this.radGridViewEditarLotes.Columns["AÑOEWS"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewEditarLotes.Columns.Contains("AVOEWS")) this.radGridViewEditarLotes.Columns["AVOEWS"].IsVisible = false;
            }
        }

        private void RadGridViewLotesErrores_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.UI = GestionLotesUserInterfaces.EdicionLotesProcesados;

            this.dtCompErrores.Clear();
            this.gbGridCompErrores.Visible = false;
            this.radGridViewCompErrores.Visible = false;

            if (this.tgGridLotesErroresRowSel != -1)
            {
                this.tgGridLotesErroresRowSel = -1;
                this.radButtonEditar.Enabled = true;

                //La Grid de lotes de error tiene que estar a tamaño grande
                if (!this.lotesErrorSizeGrande)
                {
                    this.gbGridLotesErrores.Size = new Size(this.gbGridLotesErrores.Size.Width, this.gbGridLotesErrores.Size.Height + this.gbGridCompErrores.Size.Height);
                    this.gbGridLotesErrores.Refresh();
                    //this.gbGridLotesErrores.Size = gridInfoSize;
                    this.lotesErrorSizeGrande = true;
                }
            }
        }

        private void RadGridViewLotesErrores_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.UI = GestionLotesUserInterfaces.EdicionLotesProcesados;

            this.EditarLote();
        }

        private void RadGridViewLotesErrores_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewLotesErrores.Columns.Contains("APRO24Origen")) this.radGridViewLotesErrores.Columns["APRO24Origen"].IsVisible = false;
            if (this.radGridViewLotesErrores.Columns.Contains("DATE24Origen")) this.radGridViewLotesErrores.Columns["DATE24Origen"].IsVisible = false;
            if (this.radGridViewLotesErrores.Columns.Contains("TIME24Origen")) this.radGridViewLotesErrores.Columns["TIME24Origen"].IsVisible = false;
            if (this.radGridViewLotesErrores.Columns.Contains("NUEV24")) this.radGridViewLotesErrores.Columns["NUEV24"].IsVisible = false;
        }

        private void RadGridViewLotesErrores_RowFormatting(object sender, Telerik.WinControls.UI.RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.IsCurrent)
            {
                //e.RowElement.GradientStyle = GradientStyles.Solid;
                //e.RowElement.BackColor = Color.Green;
                e.RowElement.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                //e.RowElement.ResetValue(Telerik.WinControls.UI.LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                //e.RowElement.ResetValue(Telerik.WinControls.UI.LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(Telerik.WinControls.UI.LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }
        }

        private void RadGridViewCompErrores_DataBindingComplete(object sender, Telerik.WinControls.UI.GridViewBindingCompleteEventArgs e)
        {
            if (this.radGridViewCompErrores.Columns.Contains("PREF25")) this.radGridViewCompErrores.Columns["PREF25"].IsVisible = false;
            if (this.radGridViewCompErrores.Columns.Contains("LIBL25")) this.radGridViewCompErrores.Columns["LIBL25"].IsVisible = false;
            if (this.radGridViewCompErrores.Columns.Contains("DATE25")) this.radGridViewCompErrores.Columns["DATE25"].IsVisible = false;
            if (this.radGridViewCompErrores.Columns.Contains("TIME25")) this.radGridViewCompErrores.Columns["TIME25"].IsVisible = false;
            if (this.radGridViewCompErrores.Columns.Contains("APRO24")) this.radGridViewCompErrores.Columns["APRO24"].IsVisible = false;

            if (this.bbddDB2)
            {
                if (this.radGridViewCompErrores.Columns.Contains("AÑOC25")) this.radGridViewCompErrores.Columns["AÑOC25"].IsVisible = false;
            }
            else
            {
                if (this.radGridViewCompErrores.Columns.Contains("AVOC25")) this.radGridViewCompErrores.Columns["AVOC25"].IsVisible = false;
            }

            if (this.radGridViewCompErrores.Columns.Contains("LAPS25")) this.radGridViewCompErrores.Columns["LAPS25"].IsVisible = false;
        }

        private void RadGridViewCompErrores_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            this.EditarLote();
        }

        private void BtnHcoMensajesAceptar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnHcoMensajesAceptar);
        }

        private void BtnHcoMensajesAceptar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnHcoMensajesAceptar);
        }

        private void BtnHcoMensajesCancelar_MouseEnter(object sender, EventArgs e)
        {
            utiles.ButtonMouseEnter(ref this.btnHcoMensajesCancelar);
        }

        private void BtnHcoMensajesCancelar_MouseLeave(object sender, EventArgs e)
        {
            utiles.ButtonMouseLeave(ref this.btnHcoMensajesCancelar);
        }

        private void FrmCompContGestionLotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Gestión de Lotes");
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Traducir los literales del formulario
        /// </summary>
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmCompContGestionLotes", "Gestión de Lotes");

            this.radButtonEdicionLotes.Text = this.LP.GetText("lblEdicionLotes", "Edición de Lotes");
            this.radButtonGestionLotesProc.Text = this.LP.GetText("lblGestionLotProc", "Gestión de Lotes procesados");
            this.radButtonEditar.Text = this.LP.GetText("Editar", "Editar");
            this.radButtonSuprimir.Text = "Eliminar";
            this.radButtonSuprimirHco.Text = this.LP.GetText("toolStripSuprimirHcoMensajes", "Suprimir histórico de mensajes");
            //this.toolStripButtonAjustar.Text = this.LP.GetText("lblfrmCompContClickDerAjustCol", "Ajustar columnas");

            //Edición de Lotes
            this.lblPrefijo.Text = this.LP.GetText("lblPrefijo", "Prefijo");
            this.lblBilioteca.Text = this.LP.GetText("lblBiblioteca", "Biblioteca");
            this.lblFormatoAmpliado.Text = this.LP.GetText("lblFormatoAmpliado", "Formato ampliado");
            this.btnAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");

            //Gestión de Lotes procesados
            this.lblBuscadorPrefijo.Text = this.LP.GetText("lblPrefijo", "Prefijo");
            this.lblBuscadorBiblio.Text = this.LP.GetText("lblBiblioteca", "Biblioteca");
            this.lblBuscadorDescripcion.Text = this.LP.GetText("lblDescripcion", "Descripción");
            this.lblBuscadorUsuario.Text = this.LP.GetText("lblUsuario", "Usuario");
            this.lblBuscadorFecha.Text = this.LP.GetText("lblFecha", "Fecha");
            this.btnBuscadorBuscar.Text = this.LP.GetText("lblBuscar", "Buscar");
            this.btnBuscadorTodos.Text = this.LP.GetText("lblTodos", "Todos");

            //Suprimir histórico de mensajes de error
            this.gbSuprimirHco.Text = this.LP.GetText("lblSuprimirHcoMensError", "Suprimir histórico de mensajes de error");
            this.lblHastaFecha.Text = this.LP.GetText("lblHastaFecha", "Hasta fecha");
            this.btnHcoMensajesAceptar.Text = this.LP.GetText("lblAceptar", "Aceptar");
            this.btnHcoMensajesCancelar.Text = this.LP.GetText("lblCancelar", "Cancelar");

            //Literales cabeceras de los grupos que contienen las Grids
            this.gbGridEditarLotes.Text = this.LP.GetText("lblCabeceraComp", "Cabeceras de comprobantes");
            this.gbGridLotesErrores.Text = this.LP.GetText("lblLotesProc", "Lotes procesados");
            this.gbGridCompErrores.Text = this.LP.GetText("lblErrores", "Errores");
        }

        /// <summary>
        /// Crea la Grid para la edición de lotes
        /// </summary>
        private void BuildDataGridtgGridEditarLotes()
        {
            try
            {
                this.dtEditarLote = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                this.dtEditarLote.Columns.Add("CCIAWS", typeof(string));
                this.dtEditarLote.Columns.Add("AAPP", typeof(string));
                this.dtEditarLote.Columns.Add("DOCDWS", typeof(string));
                this.dtEditarLote.Columns.Add("TICOWS", typeof(string));
                this.dtEditarLote.Columns.Add("NUCOWS", typeof(string));
                this.dtEditarLote.Columns.Add("FECHA", typeof(string));
                this.dtEditarLote.Columns.Add("TVOUWS", typeof(string));
                this.dtEditarLote.Columns.Add("TASCWS", typeof(string));
                if (this.bbddDB2) this.dtEditarLote.Columns.Add("AÑOCWS", typeof(string));
                else this.dtEditarLote.Columns.Add("AVOCWS", typeof(string));
                this.dtEditarLote.Columns.Add("LAPSWS", typeof(string));
                this.dtEditarLote.Columns.Add("DIAEWS", typeof(string));
                this.dtEditarLote.Columns.Add("MESEWS", typeof(string));
                if (this.bbddDB2) this.dtEditarLote.Columns.Add("AÑOEWS", typeof(string));
                else this.dtEditarLote.Columns.Add("AVOEWS", typeof(string));

                this.radGridViewEditarLotes.DataSource = this.dtEditarLote;
                //Escribe el encabezado de la Grid de EditarLotes
                this.BuildDisplayNamesEditarLotes();
                this.RadGridViewEditarLotesHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea la Grid para los comprobantes con errores
        /// </summary>
        private void BuildDataGridtgGridLotesErrores()
        {
            try
            {
                this.dtLotesError = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                this.dtLotesError.Columns.Add("PREF24", typeof(string));
                this.dtLotesError.Columns.Add("LIBL24", typeof(string));
                this.dtLotesError.Columns.Add("APRO24", typeof(string));
                this.dtLotesError.Columns.Add("DESC24", typeof(string));
                this.dtLotesError.Columns.Add("USER24", typeof(string));
                this.dtLotesError.Columns.Add("WSGE24", typeof(string));
                this.dtLotesError.Columns.Add("DATE24", typeof(string));
                this.dtLotesError.Columns.Add("TIME24", typeof(string));
                this.dtLotesError.Columns.Add("ERRO24", typeof(string));
                this.dtLotesError.Columns.Add("APRO24Origen", typeof(string));
                this.dtLotesError.Columns.Add("DATE24Origen", typeof(string));
                this.dtLotesError.Columns.Add("TIME24Origen", typeof(string));
                this.dtLotesError.Columns.Add("NUEV24", typeof(string));

                this.radGridViewLotesErrores.DataSource = this.dtLotesError;
                //Escribe el encabezado de la Grid de EditarLotes
                this.BuildDisplayNamesLotesError();
                this.RadGridViewLotesErrorHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Crea la Grid para los comprobantes con errores
        /// </summary>
        private void BuildDataGridtgGridCompErrores()
        {
            try
            {
                this.dtCompErrores = new DataTable
                {
                    TableName = "Tabla"
                };

                //Adicionar las columnas al DataTable
                dtCompErrores.Columns.Add("CCIA25", typeof(string));
                dtCompErrores.Columns.Add("AAPP", typeof(string));
                dtCompErrores.Columns.Add("TICO25", typeof(string));
                dtCompErrores.Columns.Add("NUCO25", typeof(string));
                dtCompErrores.Columns.Add("SIMI25", typeof(string));
                dtCompErrores.Columns.Add("CODI25", typeof(string));
                dtCompErrores.Columns.Add("PREF25", typeof(string));
                dtCompErrores.Columns.Add("LIBL25", typeof(string));
                dtCompErrores.Columns.Add("DATE25", typeof(string));
                dtCompErrores.Columns.Add("TIME25", typeof(string));
                dtCompErrores.Columns.Add("APRO24", typeof(string));
                if (this.bbddDB2) dtCompErrores.Columns.Add("AÑOC25", typeof(string));
                else dtCompErrores.Columns.Add("AVOC25", typeof(string));
                dtCompErrores.Columns.Add("LAPS25", typeof(string));

                this.radGridViewCompErrores.DataSource = this.dtCompErrores;
                //Escribe el encabezado de la Grid de EditarLotes
                this.BuildDisplayNamesCompErrores();
                this.RadGridViewCompErroresHeader();
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }


        /// <summary>
        /// Valida que existan las tablas de lotes
        /// </summary>
        /// <returns></returns>
        private string FormValid()
        {
            string result = "";

            try
            {
                string prefijo = this.txtPrefijo.Text.Trim();
                string biblioteca = "";

                if (prefijo == "")
                {
                    result += "- " + this.LP.GetText("errPrefijoVacio", "El prefijo no puede estar en blanco");
                    this.txtPrefijo.Focus();
                }
                else prefijo = prefijo.ToUpper();

                if (this.bbddDB2)
                {
                    biblioteca = this.txtBiblioteca.Text.Trim();
                    if (biblioteca == "")
                    {
                        if (result != "") result += "\n\r";
                        result += "- " + this.LP.GetText("errBibliotecaVacia", "La biblioteca no puede estar en blanco");
                        this.txtBiblioteca.Focus();
                    }
                }

                if (result != "") return (result);

                if (this.bbddDB2)
                {
                    if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                    else this.bibliotecaTablasLoteAS = "";
                }

                try
                {
                    if (!this.radToggleSwitchFormatoAmpliado.Value)
                    {
                        //No extendido
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W00")) result += this.LP.GetText("errTablaCabLoteNoExiste", "No existe la tabla cabecera de lotes");
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W01")) result += this.LP.GetText("errTablaDetLoteNoExiste", "No existe la tabla detalles de lotes");
                    }
                    else
                    {
                        //Extendido
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W10")) result += this.LP.GetText("errTablaCabLoteNoExiste", "No existe la tabla cabecera de lotes");
                        if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W11")) result += this.LP.GetText("errTablaDetLoteNoExiste", "No existe la tabla detalles de lotes");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    result += " - " + this.LP.GetText("errVerificarTablaLote", "No existe la tabla de lote") + " (" + ex.Message + ")" + "\n\r";
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                result += " - " + this.LP.GetText("errVerificarDatosLote", "Error verificando si existen datos del lote") + " (" + ex.Message + ")" + "\n\r";
            }

            return (result);
        }

        private void FillDataGridtgGridEditarLotes()
        {
            try
            {
                this.dtEditarLote.Rows.Clear();
                string prefijo = this.txtPrefijo.Text.Trim().ToUpper();
                string biblioteca = "";

                if (this.bbddDB2)
                {
                    biblioteca = this.txtBiblioteca.Text.Trim();

                    if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                    else this.bibliotecaTablasLoteAS = "";
                }

                string tabla = "W00";
                if (this.radToggleSwitchFormatoAmpliado.Value) tabla = "W10";
                string query = "select * from " + GlobalVar.PrefijoTablaCG + this.bibliotecaTablasLoteAS + prefijo + tabla;

                DataTable dtLotes = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                DataRow rowLote;

                string sigloanoper = "";
                string anows = "";
                string dia = "";
                string mes = "";
                string ano = "";

                for (int i = 0; i < dtLotes.Rows.Count; i++)
                {
                    rowLote = this.dtEditarLote.NewRow();
                    rowLote["CCIAWS"] = dtLotes.Rows[i]["CCIAWS"].ToString();

                    //if (this.bbddDB2) anows = dtLotes.Rows[i]["AÑOCWS"].ToString().PadLeft(2, '0'); //Cambiado por problemas con la Ñ; Viene como #
                    if (this.bbddDB2) anows = dtLotes.Rows[i][2].ToString().PadLeft(2, '0');
                    else anows = dtLotes.Rows[i]["AVOCWS"].ToString().PadLeft(2, '0');

                    sigloanoper = anows + "-" + dtLotes.Rows[i]["LAPSWS"].ToString().PadLeft(2, '0');

                    rowLote["AAPP"] = sigloanoper;
                    rowLote["DOCDWS"] = dtLotes.Rows[i]["DOCDWS"].ToString();
                    rowLote["TICOWS"] = dtLotes.Rows[i]["TICOWS"].ToString();
                    rowLote["NUCOWS"] = dtLotes.Rows[i]["NUCOWS"].ToString();

                    dia = dtLotes.Rows[i]["DIAEWS"].ToString();
                    if (dia == "") dia = dia.PadLeft(2, ' ');
                    else if (dia.Length == 1) dia = dia.PadLeft(2, '0');
                    mes = dtLotes.Rows[i]["MESEWS"].ToString();
                    if (mes == "") mes = mes.PadLeft(2, ' ');
                    else if (mes.Length == 1) mes = mes.PadLeft(2, '0');
                    //if (this.bbddDB2) ano = dtLotes.Rows[i]["AÑOEWS"].ToString();
                    if (this.bbddDB2) ano = dtLotes.Rows[i][9].ToString();
                    else ano = dtLotes.Rows[i]["AVOEWS"].ToString();
                    if (ano == "") ano = ano.PadLeft(2, ' ');
                    else if (ano.Length == 1) ano = ano.PadLeft(2, '0');

                    rowLote["FECHA"] = dia + "/" + mes + "/" + ano;

                    rowLote["TVOUWS"] = dtLotes.Rows[i]["TVOUWS"].ToString();
                    rowLote["TASCWS"] = dtLotes.Rows[i]["TASCWS"].ToString();

                    //if (this.bbddDB2) rowLote["AÑOCWS"] = dtLotes.Rows[i]["AÑOCWS"].ToString(); jl
                    //if (this.bbddDB2) rowLote[2] = dtLotes.Rows[i]["AÑOCWS"].ToString();
                    //if (this.bbddDB2) rowLote[2] = dtLotes.Rows[i][2].ToString();
                    if (this.bbddDB2) rowLote[2] = dtLotes.Rows[i]["DOCDWS"].ToString();
                    else rowLote["AVOCWS"] = dtLotes.Rows[i]["AVOCWS"].ToString();

                    rowLote["LAPSWS"] = dtLotes.Rows[i]["LAPSWS"].ToString();
                    rowLote["DIAEWS"] = dtLotes.Rows[i]["DIAEWS"].ToString();
                    rowLote["MESEWS"] = dtLotes.Rows[i]["MESEWS"].ToString();

                    //if (this.bbddDB2) rowLote["AÑOEWS"] = dtLotes.Rows[i]["AÑOEWS"].ToString();
                    if (this.bbddDB2) rowLote[9] = dtLotes.Rows[i][9].ToString();
                    else rowLote["AVOEWS"] = dtLotes.Rows[i]["AVOEWS"].ToString();

                    this.dtEditarLote.Rows.Add(rowLote);
                }

                if (dtLotes.Rows.Count > 0)
                {
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref radButtonSuprimir, true);
                    this.gbGridEditarLotes.Visible = true;
                    this.radGridViewEditarLotes.Visible = true;
                    this.radGridViewEditarLotes.Focus();

                    for (int i = 0; i < this.radGridViewEditarLotes.Columns.Count; i++)
                        this.radGridViewEditarLotes.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    this.radGridViewEditarLotes.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewEditarLotes.MasterTemplate.BestFitColumns();
                    this.radGridViewEditarLotes.Rows[0].IsCurrent = true;
                    this.radGridViewEditarLotes.Visible = true;
                    this.radGridViewEditarLotes.Focus();
                }
                else
                {
                    this.gbGridEditarLotes.Visible = false;
                    this.radGridViewEditarLotes.Visible = false;
                    //this.toolStripButtonAjustar.Enabled = false;
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref radButtonSuprimir, false);
                    this.lblInfo.Text = this.LP.GetText("errNoExistenLotes", "No existen lotes");
                    this.lblInfo.Focus();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(ex.Message, error);
            }
        }

        /// <summary>
        /// Encabezados para la Grid de EditarLotes
        /// </summary>
        private void BuildDisplayNamesEditarLotes()
        {
            try
            {
                this.displayNamesEditarLotes = new Dictionary<string, string>
                {
                    { "CCIAWS", this.LP.GetText("CompContdgHeaderCompania", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "DOCDWS", this.LP.GetText("CompContdgHeaderDescripcion", "Descripción") },
                    { "TICOWS", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NUCOWS", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "FECHA", this.LP.GetText("CompContdgHeaderFecha", "Fecha") },
                    { "TVOUWS", this.LP.GetText("CompContdgHeaderClase", "Clase") },
                    { "TASCWS", this.LP.GetText("CompContdgHeaderTasa", "Tasa") }
                };
                if (this.bbddDB2)
                    this.displayNamesEditarLotes.Add("AÑOCWS", "PeriodoAA");
                else
                    this.displayNamesEditarLotes.Add("AVOCWS", "PeriodoAA");
                this.displayNamesEditarLotes.Add("LAPSWS", "PeriodoPP");
                this.displayNamesEditarLotes.Add("DIAEWS", "Dia");
                this.displayNamesEditarLotes.Add("MESEWS", "Mes");
                if (this.bbddDB2)
                    this.displayNamesEditarLotes.Add("AÑOEWS", "Ano");
                else
                    this.displayNamesEditarLotes.Add("AVOEWS", "Ano");
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de EditarLotes
        /// </summary>
        private void RadGridViewEditarLotesHeader()
        {
            try
            {
                if (this.radGridViewEditarLotes.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesEditarLotes)
                    {
                        if (this.radGridViewEditarLotes.Columns.Contains(item.Key)) this.radGridViewEditarLotes.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Carga la info en la Grid de Lotes Procesados
        /// </summary>
        private void FillDataGridtgGridLotesErrores()
        {
            try
            {
                string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLC24";
                if (!this.mostrarLotesTodos) query += " where NUEV24 = '1'";
                query += " order by DATE24, TIME24";

                DataTable dtErrores = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);

                this.dtLotesError.Clear();

                DataRow rowLotesErrores;
                string modoTransf = "";
                string fecha = "";
                int fechaInt;
                string hora = "";
                DateTime dtFecha;

                for (int i = 0; i < dtErrores.Rows.Count; i++)
                {
                    rowLotesErrores = this.dtLotesError.NewRow();

                    rowLotesErrores["PREF24"] = dtErrores.Rows[i]["PREF24"].ToString();
                    rowLotesErrores["LIBL24"] = dtErrores.Rows[i]["LIBL24"].ToString();

                    modoTransf = dtErrores.Rows[i]["APRO24"].ToString().Trim();

                    switch (modoTransf)
                    {
                        case "C":
                        case "c":
                            rowLotesErrores["APRO24"] = this.LP.GetText("lblTipoTransContab", "Contabilizados");
                            break;
                        case "S":
                        case "s":
                            rowLotesErrores["APRO24"] = this.LP.GetText("lblTipoTransAprob", "Aprobados");
                            break;
                        case "N":
                        case "n":
                        default:
                            rowLotesErrores["APRO24"] = this.LP.GetText("lblTipoTransNoAprob", "No Aprobados");
                            break;
                    }

                    rowLotesErrores["DESC24"] = dtErrores.Rows[i]["DESC24"].ToString();
                    rowLotesErrores["USER24"] = dtErrores.Rows[i]["USER24"].ToString();
                    rowLotesErrores["WSGE24"] = dtErrores.Rows[i]["WSGE24"].ToString();

                    fechaInt = 0;
                    fecha = dtErrores.Rows[i]["DATE24"].ToString();
                    try
                    {
                        fechaInt = Convert.ToInt32(fecha);

                        if (fechaInt < 0) fechaInt = fechaInt * -1;
                        else fechaInt = 9999999 - fechaInt;
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    rowLotesErrores["DATE24Origen"] = fecha;
                    dtFecha = utiles.FormatoCGToFecha(fechaInt.ToString());
                    rowLotesErrores["DATE24"] = dtFecha.Date.ToShortDateString();

                    hora = dtErrores.Rows[i]["TIME24"].ToString();
                    rowLotesErrores["TIME24Origen"] = hora;
                    hora = utiles.FormatoCGToTime(hora);
                    rowLotesErrores["TIME24"] = hora;

                    rowLotesErrores["ERRO24"] = dtErrores.Rows[i]["ERRO24"].ToString();

                    rowLotesErrores["NUEV24"] = dtErrores.Rows[i]["NUEV24"].ToString();

                    this.dtLotesError.Rows.Add(rowLotesErrores);
                }

                if (dtErrores.Rows.Count > 0)
                {
                    //this.toolStripButtonAjustar.Enabled = true;
                    utiles.ButtonEnabled(ref this.radButtonEditar, true);
                    utiles.ButtonEnabled(ref radButtonSuprimir, true);
                    this.radButtonSuprimirHco.Visible = true;
                    this.gbGridLotesErrores.Visible = true;
                    
                    for (int i = 0; i < this.radGridViewLotesErrores.Columns.Count; i++)
                        this.radGridViewLotesErrores.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                    this.radGridViewLotesErrores.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                    this.radGridViewLotesErrores.MasterTemplate.BestFitColumns();
                    this.radGridViewLotesErrores.Rows[0].IsCurrent = true;
                    this.radGridViewLotesErrores.Visible = true;
                    this.radGridViewLotesErrores.Focus();
                }
                else
                {
                    this.gbGridLotesErrores.Visible = false;
                    this.radGridViewLotesErrores.Visible = false;
                    utiles.ButtonEnabled(ref this.radButtonEditar, false);
                    utiles.ButtonEnabled(ref radButtonSuprimir, false);
                    this.radButtonSuprimirHco.Visible = false;
                    this.lblInfo.Text = this.LP.GetText("lblNoExistenCompConErrores", "No existen comprobantes con errores");
                    this.lblInfo.Focus();
                }   
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                string error = this.LP.GetText("errValTitulo", "Error");
                RadMessageBox.Show(ex.Message, error);
            }
        }

        /// <summary>
        /// Encabezados para la Grid de LotesError
        /// </summary>
        private void BuildDisplayNamesLotesError()
        {
            try
            {
                this.displayNamesLotesError = new Dictionary<string, string>
                {
                    { "PREF24", this.LP.GetText("lblPrefijo", "Prefijo") },
                    { "LIBL24", this.LP.GetText("lblBiblioteca", "Biblioteca") },
                    { "APRO24", this.LP.GetText("dgHeaderLotesErroresTipoTransf", "Tipo transferencia") },
                    { "DESC24", this.LP.GetText("lblListaCampoDescripcion", "Descripción") },
                    { "USER24", this.LP.GetText("lblUsuario", "Usuario") },
                    { "WSGE24", this.LP.GetText("dgHeaderLotesErroresOrigen", "Origen") },
                    { "DATE24", this.LP.GetText("lblFecha", "Fecha") },
                    { "TIME24", this.LP.GetText("dgHeaderLotesErroresHora", "Hora") },
                    { "ERRO24", this.LP.GetText("dgHeaderLotesErroresNumErrores", "Número de Errores") },
                    { "APRO24Origen", "Formato" },
                    { "DATE24Origen", "DATE24Origen" },
                    { "TIME24Origen", "TIME24Origen" },
                    { "NUEV24", "Nuevo" }
                };
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de LotesError
        /// </summary>
        private void RadGridViewLotesErrorHeader()
        {
            try
            {
                if (this.radGridViewLotesErrores.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesLotesError)
                    {
                        if (this.radGridViewLotesErrores.Columns.Contains(item.Key)) this.radGridViewLotesErrores.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Carga la info en la Grid de Comprobantes Errores
        /// </summary>
        private void FillDataGridtgGridCompErrores()
        {
            string error = this.LP.GetText("errValTitulo", "Error");
            try
            {
                //Seleccionar la fila activa
                if (this.radGridViewLotesErrores.SelectedRows.Count == 1)
                {
                    Telerik.WinControls.UI.GridViewRowInfo rowLotErr = this.radGridViewLotesErrores.SelectedRows[0];

                    string fecha = rowLotErr.Cells["DATE24Origen"].Value.ToString();
                    string hora = rowLotErr.Cells["TIME24Origen"].Value.ToString();
                    string biblioteca = rowLotErr.Cells["LIBL24"].Value.ToString().Trim();
                    string prefijo = rowLotErr.Cells["PREF24"].Value.ToString();
                    string formato = rowLotErr.Cells["APRO24Origen"].Value.ToString();

                    //Obtener los datos del comprobante
                    string query = "select * from " + GlobalVar.PrefijoTablaCG + "GLC25";
                    query += " where DATE25 = " + fecha + " and TIME25 = " + hora;
                    query += " and PREF25 = '" + prefijo + "' and LIBL25 = '" + biblioteca + "'";

                    DataTable dtErrores = GlobalVar.ConexionCG.FillDataTable(query, GlobalVar.ConexionCG.GetConnectionValue);
                    this.gbGridCompErrores.Visible = false;
                    this.radGridViewCompErrores.Visible = false;

                    this.dtCompErrores.Clear();

                    DataRow rowErrores;
                    string ano = "";
                    string anoper = "";

                    string codiError = "";

                    for (int i = 0; i < dtErrores.Rows.Count; i++)
                    {
                        rowErrores = this.dtCompErrores.NewRow();

                        rowErrores["CCIA25"] = dtErrores.Rows[i]["CCIA25"].ToString();

                        if (this.bbddDB2)
                        {
                            ano = dtErrores.Rows[i]["AÑOC25"].ToString().PadLeft(2, '0');
                            rowErrores["AÑOC25"] = ano;
                        }
                        else
                        {
                            ano = dtErrores.Rows[i]["AVOC25"].ToString().PadLeft(2, '0');
                            rowErrores["AVOC25"] = ano;
                        }
                        anoper = ano + "-" + dtErrores.Rows[i]["LAPS25"].ToString().PadLeft(2, '0');
                        rowErrores["AAPP"] = anoper;

                        rowErrores["TICO25"] = dtErrores.Rows[i]["TICO25"].ToString();
                        rowErrores["NUCO25"] = dtErrores.Rows[i]["NUCO25"].ToString();

                        rowErrores["SIMI25"] = dtErrores.Rows[i]["SIMI25"].ToString();

                        codiError = dtErrores.Rows[i]["CODI25"].ToString().Trim();
                        if (codiError != "") rowErrores["CODI25"] = codiError + this.DescripcionMensajeError(codiError);
                        else rowErrores["CODI25"] = codiError;

                        rowErrores["PREF25"] = dtErrores.Rows[i]["PREF25"].ToString();
                        rowErrores["LIBL25"] = dtErrores.Rows[i]["LIBL25"].ToString();
                        rowErrores["DATE25"] = dtErrores.Rows[i]["DATE25"].ToString();
                        rowErrores["TIME25"] = dtErrores.Rows[i]["TIME25"].ToString();
                        rowErrores["APRO24"] = formato;
                        rowErrores["LAPS25"] = dtErrores.Rows[i]["LAPS25"].ToString();

                        this.dtCompErrores.Rows.Add(rowErrores);
                    }

                    if (dtErrores.Rows.Count > 0)
                    {
                        this.gbGridCompErrores.BringToFront();
                        this.gbGridCompErrores.Visible = true;
                        
                        for (int i = 0; i < this.radGridViewCompErrores.Columns.Count; i++)
                            this.radGridViewCompErrores.Columns[i].HeaderTextAlignment = ContentAlignment.MiddleLeft;
                        this.radGridViewCompErrores.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                        this.radGridViewCompErrores.MasterTemplate.BestFitColumns();
                        this.radGridViewCompErrores.Rows[0].IsCurrent = true;
                        this.radGridViewCompErrores.Visible = true;
                        this.radGridViewCompErrores.BringToFront();
                        this.radGridViewCompErrores.Focus();
                        this.MostrarOcultarGridCompErrores();
                    }
                    else
                    {
                        this.gbGridCompErrores.Visible = false;
                        this.radGridViewCompErrores.Visible = false;
                        this.MostrarOcultarGridCompErrores();

                        RadMessageBox.Show(this.LP.GetText("lblNoExistenDatosCriterioSel", "No hay datos con el criterio de selección"), error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                RadMessageBox.Show(ex.Message, error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MostrarOcultarGridCompErrores()
        {
            try
            {
                if (this.radGridViewCompErrores.Visible)
                {
                    this.gbGridLotesErrores.Size = new Size(this.gbGridLotesErrores.Size.Width, this.gbGridLotesErrores.Size.Height - this.gbGridCompErrores.Size.Height);
                    this.gbGridCompErrores.Location = new Point(this.gbGridCompErrores.Location.X, this.gbGridLotesErrores.Location.Y + this.gbGridLotesErrores.Size.Height);
                    this.gbGridLotesErrores.Refresh();
                    this.gbGridCompErrores.Refresh();
                    this.lotesErrorSizeGrande = false;
                }
                else
                {
                    this.gbGridLotesErrores.Size = new Size(this.gbGridLotesErrores.Size.Width, this.gbGridLotesErrores.Size.Height + this.gbGridCompErrores.Size.Height);
                    this.gbGridLotesErrores.Refresh();
                    this.gbGridCompErrores.Refresh();
                    this.lotesErrorSizeGrande = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Encabezados para la Grid de Comprobantes Errores
        /// </summary>
        private void BuildDisplayNamesCompErrores()
        {
            try
            {
                this.displayNamesCompErrores = new Dictionary<string, string>
                {
                    { "CCIA25", this.LP.GetText("lblCompania", "Compañía") },
                    { "AAPP", this.LP.GetText("CompContdgHeaderAAPP", "AA-PP") },
                    { "TICO25", this.LP.GetText("CompContdgHeaderTipo", "Tipo") },
                    { "NUCO25", this.LP.GetText("CompContdgHeaderNoComp", "No Comp") },
                    { "SIMI25", this.LP.GetText("CompContdgHeaderLinea", "Línea") },
                    { "CODI25", this.LP.GetText("CompContdgHeaderMensaje", "Mensaje") },
                    { "PREF25", this.LP.GetText("lblPrefijo", "Prefijo") },
                    { "LIBL25", this.LP.GetText("lblBiblioteca", "Biblioteca") },
                    { "DATE25", this.LP.GetText("lblFecha", "Fecha") },
                    { "TIME25", this.LP.GetText("CompContdgHeaderHora", "Hora") },
                    { "APRO24", "Formato" }
                };
                if (this.bbddDB2)
                    this.displayNamesCompErrores.Add("AÑOCWS", "PeriodoAA");
                else
                    this.displayNamesCompErrores.Add("AVOCWS", "PeriodoAA");
                this.displayNamesCompErrores.Add("LAPSWS", "PeriodoPP");
                this.displayNamesCompErrores.Add("DIAEWS", "Dia");
                this.displayNamesCompErrores.Add("MESEWS", "Mes");
                if (this.bbddDB2)
                    this.displayNamesCompErrores.Add("AÑOC25", "Año");
                else
                    this.displayNamesCompErrores.Add("AVOC25", "Año");

                this.displayNamesCompErrores.Add("LAPS25", "Periodo");
            }
            catch { }
        }

        /// <summary>
        /// Escribe los encabezados de la Grid de Comprobantes Errores
        /// </summary>
        private void RadGridViewCompErroresHeader()
        {
            try
            {
                if (this.radGridViewCompErrores.Columns.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in this.displayNamesCompErrores)
                    {
                        if (this.radGridViewCompErrores.Columns.Contains(item.Key)) this.radGridViewCompErrores.Columns[item.Key].HeaderText = item.Value;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditarLote()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;
            switch (this.UI)
            {
                case GestionLotesUserInterfaces.EdicionLotes:
                    //Editar Comprobante Lote
                    this.EditarComprobanteLote();
                    break;
                case GestionLotesUserInterfaces.EdicionLotesProcesados:
                    //Editar Lotes Error
                    this.EditarLotesError();
                    break;
                case GestionLotesUserInterfaces.EdicionLotesProcesadosComp:
                    //Editar Comprobante Error
                    if (this.radButtonEditar.Enabled) this.EditarCompError();
                    break;
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Crea el comprobante para llamar al formulario de Edicion de comprobantes contables
        /// </summary>
        /// <returns></returns>
        private string EditarComprobanteLote()
        {
            string result = "";

            try
            {
                //Seleccionar la fila activa
                if (this.radGridViewEditarLotes.SelectedRows.Count == 1)
                {
                    if (this.radGridViewEditarLotes.CurrentRow is GridViewGroupRowInfo)
                    {
                        if (this.radGridViewEditarLotes.CurrentRow.IsExpanded) this.radGridViewEditarLotes.CurrentRow.IsExpanded = false;
                        else this.radGridViewEditarLotes.CurrentRow.IsExpanded = true;
                    }
                    else if (this.radGridViewEditarLotes.CurrentRow is GridViewDataRowInfo)
                    {
                        Telerik.WinControls.UI.GridViewRowInfo row = this.radGridViewEditarLotes.SelectedRows[0];

                        //Crear el comprobante contable
                        ComprobanteContable comprobante = new ComprobanteContable();

                        string codigo = row.Cells["CCIAWS"].Value.ToString();
                        comprobante.Cab_compania = codigo;

                        string aapp = row.Cells["AAPP"].Value.ToString();
                        int separador = aapp.IndexOf('-');
                        if (separador != -1) aapp = aapp.Remove(separador, 1);
                        comprobante.Cab_anoperiodo = aapp;

                        comprobante.Cab_descripcion = row.Cells["DOCDWS"].Value.ToString();

                        string tipo = row.Cells["TICOWS"].Value.ToString();
                        comprobante.Cab_tipo = tipo.PadLeft(2, '0');
                        comprobante.Cab_noComprobante = row.Cells["NUCOWS"].Value.ToString();

                        string fecha = "";
                        //if (this.bbddDB2) fecha += row.Cells["AÑOEWS"].Value.ToString().Trim();
                        //if (this.bbddDB2) fecha += row.Cells[9].Value.ToString().Trim();
                        if (this.bbddDB2) fecha += row.Cells[5].Value.ToString().Substring(6,2);
                        else fecha += row.Cells["AVOEWS"].Value.ToString().Trim();

                        if (fecha == "") fecha += "00";
                        else
                        {
                            fecha = fecha.PadLeft(2, '0');
                            int anoCorte = Convert.ToInt16(CGParametrosGrles.GLC01_ALSIRC);

                            int anoOrigenInt = Convert.ToInt32(fecha);
                            if (anoOrigenInt < anoCorte) anoOrigenInt = 2000 + anoOrigenInt;
                            else if (anoOrigenInt >= anoCorte) anoOrigenInt = 1900 + anoOrigenInt;

                            fecha = anoOrigenInt.ToString();
                        }

                        //fecha += row.Cells["MESEWS"].Value.ToString().PadLeft(2, '0');
                        fecha += row.Cells[5].Value.ToString().Substring(3,2);
                        //fecha += row.Cells["DIAEWS"].Value.ToString().PadLeft(2, '0');
                        fecha += row.Cells[5].Value.ToString().Substring(0, 2);

                        comprobante.Cab_fecha = fecha;
                        comprobante.Cab_clase = row.Cells["TVOUWS"].Value.ToString();
                        comprobante.Cab_tasa = row.Cells["TASCWS"].Value.ToString();

                        //Verificar si el comprobante tiene campos extendidos
                        bool extendido = false;
                        if (this.radToggleSwitchFormatoAmpliado.Value)
                        {
                            comprobante.Cab_extendido = "1";
                            extendido = true;
                        }
                        else comprobante.Cab_extendido = "0";

                        string ano = "";
                        //if (this.bbddDB2) ano = row.Cells["AÑOCWS"].Value.ToString();
                        //if (this.bbddDB2) ano = row.Cells[2].Value.ToString();
                        if (this.bbddDB2) ano = row.Cells[5].Value.ToString().Substring(6,2);
                        else ano = row.Cells["AVOCWS"].Value.ToString();

                        //string periodo = row.Cells["LAPSWS"].Value.ToString();
                        string periodo = row.Cells["AAPP"].Value.ToString().Substring(3, 2);

                        //Obtener los detalles del comprobante a importar
                        string prefijo = this.txtPrefijo.Text.Trim().ToUpper();
                        comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(this.bibliotecaTablasLoteAS + prefijo, comprobante.Cab_compania, ano, periodo,
                                                                                           tipo, comprobante.Cab_noComprobante,
                                                                                           extendido);

                        //Cerrar el formulario actual ???
                        frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita
                        {
                            ImportarComprobante = true,
                            Batch = true,                            
                            EdicionLote = true,
                            BatchLote = true,
                        };
                        comprobante.Biblioteca = this.txtBiblioteca.Text.Trim();
                        comprobante.Prefijo = this.txtPrefijo.Text.Trim();
                        frmCompCont.ComprobanteContableImportar = comprobante;
                        frmCompCont.NombreComprobante = comprobante.Cab_descripcion;
                        frmCompCont.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
                        frmCompCont.FrmPadre = this;
                        frmCompCont.Show();
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

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
        private DataTable ObtenerDetallesComprobanteImportar(string prefijo, string compania, string ano, string periodo, string tipo, string noComprobante, bool extendido)
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

            DataRow row;

            string tabla = "W01";
            if (extendido) tabla = "W11";

            string nombreTabla = GlobalVar.PrefijoTablaCG + prefijo + tabla;
            string query = "";


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

            query += " where CCIAWS ='" + compania + "' and ";
            if (this.bbddDB2) query += "AÑOCWS =" + ano + " and ";
            else query += "AVOCWS =" + ano + " and ";
            query += "LAPSWS =" + periodo + " and ";
            query += "TICOWS =" + tipo + " and ";
            query += "NUCOWS =" + noComprobante;

            IDataReader dr = null;

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                string CLDODT = "";
                string NDOCDT = "";
                string FDOCDT = "";
                string sFDOCDT = "";
                string FEVEDT = "";
                string sFEVEDT = "";
                string CDDOAD = "";
                string NDDOAD = "";
                string FIVAWS = "";
                string sFIVAWS = "";
                string USF1WS = "";
                string sUSF1WS = "";
                string USF2WS = "";
                string sUSF2WS = "";
                while (dr.Read())
                {
                    //Mover la barra de progreso
                    //if (this.progressBarEspera.Value + 10 > this.progressBarEspera.Maximum) this.progressBarEspera.Value = this.progressBarEspera.Minimum;
                    //else this.progressBarEspera.Value = this.progressBarEspera.Value + 10;
                    //this.progressBarEspera.Refresh();

                    row = dtDetalle.NewRow();

                    row["Cuenta"] = dr["CUENWS"].ToString().Trim();
                    row["Auxiliar1"] = dr["CAUXWS"].ToString().Trim();
                    row["Auxiliar2"] = dr["AUA1WS"].ToString().Trim();
                    row["Auxiliar3"] = dr["AUA2WS"].ToString().Trim();
                    row["DH"] = dr["TMOVWS"].ToString().Trim();
                    row["MonedaLocal"] = dr["MONTWS"].ToString().Trim();
                    row["MonedaExt"] = dr["MOSMWS"].ToString().Trim();
                    row["RU"] = dr["TEINWS"].ToString().Trim();
                    row["Descripcion"] = dr["DESCWS"].ToString().Trim();
                    CLDODT = dr["CLDOWS"].ToString().Trim();
                    NDOCDT = dr["NDOCWS"].ToString().Trim();
                    NDOCDT = NDOCDT.PadLeft(7, '0');
                    //if (CLDODT != "" && CLDODT != "0" && NDOCDT != "" && NDOCDT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    if (CLDODT != "" && CLDODT != "0") row["Documento"] = CLDODT + "-" + NDOCDT;
                    //FDOCDT a formato saammdd
                    sFDOCDT = dr["FDOCWS"].ToString().Trim();
                    row["Fecha"] = "";
                    if (sFDOCDT != "" && sFDOCDT != "0")
                    {
                        sFDOCDT = sFDOCDT.PadLeft(6, '0');
                        FDOCDT = sFDOCDT.Substring(4, 2) + sFDOCDT.Substring(2, 2) + sFDOCDT.Substring(0, 2);
                        if (string.Compare(FDOCDT.Substring(4, 2), CGParametrosGrles.GLC01_ALSIRC) < 0) FDOCDT = "1" + FDOCDT;
                        row["Fecha"] = FDOCDT;
                    }
                    //FEVEDT a formato saammdd
                    sFEVEDT = dr["FEVEWS"].ToString().Trim();
                    row["Vencimiento"] = "";
                    if (sFEVEDT != "" && sFEVEDT != "0")
                    {
                        sFEVEDT = sFEVEDT.PadLeft(6, '0');
                        FEVEDT = sFEVEDT.Substring(4, 2) + sFEVEDT.Substring(2, 2) + sFEVEDT.Substring(0, 2);
                        if (string.Compare(FEVEDT.Substring(4, 2), CGParametrosGrles.GLC01_ALSIRC) < 0) FEVEDT = "1" + FEVEDT;
                        row["Vencimiento"] = FEVEDT;
                    }
                    CDDOAD = dr["CDDOWS"].ToString().Trim();
                    NDDOAD = dr["NDDOWS"].ToString().Trim();
                    NDDOAD = NDDOAD.PadLeft(9, '0');
                    //if (NDDOAD != "" && NDDOAD != "0") row["Documento2"] = NDDOAD;
                    if (CDDOAD != "" && CDDOAD != "0") row["Documento2"] = CDDOAD + "-" + NDDOAD;
                    row["Importe3"] = dr["TERCWS"].ToString().Trim();
                    row["Iva"] = dr["CDIVWS"].ToString().Trim();
                    row["CifDni"] = dr["NNITWS"].ToString().Trim();
                    row["RowNumber"] = dr["id"].ToString().Trim();

                    if (extendido)
                    {
                        //Si el compobante tiene campos extendidos, leer los valores de los campos extendidos para la línea de detalle
                        //simidt = dr["SIMIDT"].ToString().Trim();
                        //this.ObtenerDetalleCamposExtendidos(ref row, compania, ano, periodo, tipo, noComprobante, simidt);


                        ////this.ObtenerDetalleCamposExtendidos(prefijo, ref row, compania, ano, periodo, tipo, noComprobante);

                        row["PrefijoDoc"] = dr["PRFDWS"].ToString().Trim();
                        row["NumFactAmp"] = dr["NFAAWS"].ToString().Trim();
                        row["NumFactRectif"] = dr["NFARWS"].ToString().Trim();
                        //FIVAWS a formato saammdd
                        sFIVAWS = dr["FIVAWS"].ToString().Trim();
                        row["FechaServIVA"] = "";
                        if (sFIVAWS != "" && sFIVAWS != "0")
                        {
                            sFIVAWS = sFIVAWS.PadLeft(6, '0');
                            FIVAWS = sFIVAWS.Substring(4, 2) + sFIVAWS.Substring(2, 2) + sFIVAWS.Substring(0, 2);
                            if (string.Compare(sFIVAWS.Substring(4, 2), CGParametrosGrles.GLC01_ALSIRC) < 0) FIVAWS = "1" + FIVAWS;
                            row["FechaServIVA"] = FIVAWS;
                        }
                        row["CampoUserAlfa1"] = dr["USA1WS"].ToString().Trim();
                        row["CampoUserAlfa2"] = dr["USA2WS"].ToString().Trim();
                        row["CampoUserAlfa3"] = dr["USA3WS"].ToString().Trim();
                        row["CampoUserAlfa4"] = dr["USA4WS"].ToString().Trim();
                        row["CampoUserAlfa5"] = dr["USA5WS"].ToString().Trim();
                        row["CampoUserAlfa6"] = dr["USA6WS"].ToString().Trim();
                        row["CampoUserAlfa7"] = dr["USA7WS"].ToString().Trim();
                        row["CampoUserAlfa8"] = dr["USA8WS"].ToString().Trim();
                        row["CampoUserNum1"] = dr["USN1WS"].ToString().Trim();
                        row["CampoUserNum2"] = dr["USN2WS"].ToString().Trim();
                        //USF1WS a formato saammdd
                        sUSF1WS = dr["USF1WS"].ToString().Trim();
                        row["CampoUserFecha1"] = "";
                        if (sUSF1WS != "" && sUSF1WS != "0")
                        {
                            sUSF1WS = sUSF1WS.PadLeft(6, '0');
                            USF1WS = sUSF1WS.Substring(4, 2) + sUSF1WS.Substring(2, 2) + sUSF1WS.Substring(0, 2);
                            if (string.Compare(sUSF1WS.Substring(4, 2), CGParametrosGrles.GLC01_ALSIRC) < 0) USF1WS = "1" + USF1WS;
                            row["CampoUserFecha1"] = USF1WS;
                        }
                        //USF2WS a formato saammdd
                        sUSF2WS = dr["USF2WS"].ToString().Trim();
                        row["CampoUserFecha2"] = "";
                        if (sUSF2WS != "" && sUSF2WS != "0")
                        {
                            sUSF2WS = sUSF2WS.PadLeft(6, '0');
                            USF2WS = sUSF2WS.Substring(4, 2) + sUSF2WS.Substring(2, 2) + sUSF2WS.Substring(0, 2);
                            if (string.Compare(sUSF2WS.Substring(4, 2), CGParametrosGrles.GLC01_ALSIRC) < 0) USF2WS = "1" + USF2WS;
                            row["CampoUserFecha2"] = USF2WS;
                        }
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
        //private void ObtenerDetalleCamposExtendidos(ref DataRow row, string compania, string anoperiodo, string tipo, string noComprobante, string simidt)
        private void ObtenerDetalleCamposExtendidos(string prefijo, ref DataRow row, string compania, string ano, string periodo, string tipo, string noComprobante)
        {
            string FIVAWS = "";
            string USF1WS = "";
            string USF2WS = "";
            //string prefijo = this.txtPrefijo.Text.Trim().ToUpper();

            string tabla = "W01";
            if (this.radToggleSwitchFormatoAmpliado.Value) tabla = "W11";

            string query = "select * from " + GlobalVar.PrefijoTablaCG + prefijo + tabla;
            query += " where CCIAWS ='" + compania + "' and ";
            if (this.bbddDB2) query += "AÑOCWS =" + ano + " and ";
            else query += "AVOCWS =" + ano + " and ";
            query += "LAPSWS =" + periodo + " and ";
            query += "TICOWS =" + tipo + " and ";
            query += "NUCOWS =" + noComprobante;
            //query += "NUCOWS =" + noComprobante + " and ";
            //query += "SIMIDX =" + simidt;

            IDataReader dr = null;

            try
            {
                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    row["PrefijoDoc"] = dr["PRFDWS"].ToString().Trim();
                    row["NumFactAmp"] = dr["NFAAWS"].ToString().Trim();
                    row["NumFactRectif"] = dr["NFARWS"].ToString().Trim();
                    //FIVA
                    FIVAWS = dr["FIVAWS"].ToString().Trim();
                    row["FechaServIVA"] = "";
                    if (FIVAWS != "" && FIVAWS != "0") row["FechaServIVA"] = FIVAWS;
                    //row["FechaServIVA"] = dr["FIVAWS"].ToString().Trim();

                    row["CampoUserAlfa1"] = dr["USA1WS"].ToString().Trim();
                    row["CampoUserAlfa2"] = dr["USA2WS"].ToString().Trim();
                    row["CampoUserAlfa3"] = dr["USA3WS"].ToString().Trim();
                    row["CampoUserAlfa4"] = dr["USA4WS"].ToString().Trim();
                    row["CampoUserAlfa5"] = dr["USA5WS"].ToString().Trim();
                    row["CampoUserAlfa6"] = dr["USA6WS"].ToString().Trim();
                    row["CampoUserAlfa7"] = dr["USA7WS"].ToString().Trim();
                    row["CampoUserAlfa8"] = dr["USA8WS"].ToString().Trim();
                    row["CampoUserNum1"] = dr["USN1WS"].ToString().Trim();
                    row["CampoUserNum2"] = dr["USN2WS"].ToString().Trim();
                    USF1WS = dr["USF1WS"].ToString().Trim();
                    row["CampoUserFecha1"] = "";
                    if (USF1WS != "" && USF1WS != "0") row["CampoUserFecha1"] = USF1WS;
                    //row["CampoUserFecha1"] = dr["USF1WS"].ToString().Trim();
                    USF1WS = dr["USF1WS"].ToString().Trim();
                    row["CampoUserFecha2"] = "";
                    if (USF2WS != "" && USF2WS != "0") row["CampoUserFecha2"] = USF2WS;
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
        /// Edita Lote de Error (Muestra la grid con los comprobantes de error que contiene)
        /// </summary>
        private void EditarLotesError()
        {
            string error = this.LP.GetText("errValTitulo", "Error");

            if (this.radGridViewLotesErrores.CurrentRow is GridViewGroupRowInfo)
            {
                if (this.radGridViewLotesErrores.CurrentRow.IsExpanded) this.radGridViewLotesErrores.CurrentRow.IsExpanded = false;
                else this.radGridViewLotesErrores.CurrentRow.IsExpanded = true;
            }
            else if (this.radGridViewLotesErrores.CurrentRow is GridViewDataRowInfo)
            {
                if (this.radGridViewLotesErrores.SelectedRows.Count > 1)
                {
                    RadMessageBox.Show(this.LP.GetText("lblSelSolo1Lote", "Debe seleccionar un solo lote"), error);
                }
                else
                {
                    string numErrores = this.radGridViewLotesErrores.SelectedRows[0].Cells["ERRO24"].Value.ToString();
                    int numErroresInt = 0;

                    try
                    {
                        numErroresInt = Convert.ToInt16(numErrores);
                    }
                    catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

                    if (numErroresInt == 0)
                        RadMessageBox.Show(this.LP.GetText("lblNoErrores", "No se han detectado errores"), error);
                    else
                    {
                        string nuevo = this.radGridViewLotesErrores.SelectedRows[0].Cells["NUEV24"].Value.ToString();

                        if (nuevo != "1") utiles.ButtonEnabled(ref this.radButtonEditar, false);

                        this.FillDataGridtgGridCompErrores();
                        this.UI = GestionLotesUserInterfaces.EdicionLotesProcesadosComp;
                    }
                }
            }
        }

        /// <summary>
        /// Edita un comprobante que tiene errores
        /// </summary>
        private void EditarCompError()
        {
            IDataReader dr = null;

            string error = this.LP.GetText("errValTitulo", "Error");

            try
            {
                if (this.radGridViewCompErrores.CurrentRow is GridViewGroupRowInfo)
                {
                    if (this.radGridViewCompErrores.CurrentRow.IsExpanded) this.radGridViewCompErrores.CurrentRow.IsExpanded = false;
                    else this.radGridViewCompErrores.CurrentRow.IsExpanded = true;
                }
                else if (this.radGridViewCompErrores.CurrentRow is GridViewDataRowInfo)
                {
                    //Seleccionar la fila activa
                    if (this.radGridViewCompErrores.SelectedRows.Count == 1)
                    {
                        Telerik.WinControls.UI.GridViewRowInfo row = this.radGridViewCompErrores.SelectedRows[0];

                        string fecha = row.Cells["DATE25"].Value.ToString();
                        string hora = row.Cells["TIME25"].Value.ToString();
                        string biblioteca = row.Cells["LIBL25"].Value.ToString().Trim();
                        string prefijo = row.Cells["PREF25"].Value.ToString();
                        string formato = row.Cells["APRO24"].Value.ToString();
                        string compania = row.Cells["CCIA25"].Value.ToString();

                        string ano = "";
                        if (this.bbddDB2) ano = row.Cells["AÑOC25"].Value.ToString();
                        else ano = row.Cells["AVOC25"].Value.ToString();

                        string periodo = row.Cells["LAPS25"].Value.ToString();
                        string tipo = row.Cells["TICO25"].Value.ToString();
                        string numComp = row.Cells["NUCO25"].Value.ToString();
                        bool extendido = false;
                        string tabla = "W00";

                        if (formato == "s" || formato == "c" || formato == "n")
                        {
                            extendido = true;
                            tabla = "W10";
                        }

                        if (this.bbddDB2)
                        {
                            if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                            else this.bibliotecaTablasLoteAS = "";
                        }

                        string errorCargarComp = "";
                        bool resultCargarComp = this.CargarComprobante(compania, prefijo, tabla, ano, periodo, tipo, numComp, extendido, ref error, biblioteca);

                        if (errorCargarComp != "") RadMessageBox.Show(errorCargarComp, error);
                        else
                        {
                            if (!resultCargarComp)
                            {
                                //Verificar si existen las tablas de Lotes de errores, para traspasar los comprobantes si lo confirma
                                this.VerificarExistenTablasLotesErroneos(prefijo, biblioteca, extendido);

                                this.TraspasarComprobantesErroneosDeTabla(prefijo, biblioteca, extendido, false);

                                errorCargarComp = "";
                                resultCargarComp = this.CargarComprobante(compania, prefijo, tabla, ano, periodo, tipo, numComp, extendido, ref error, biblioteca);

                                if (errorCargarComp != "") RadMessageBox.Show(errorCargarComp, error);
                                else if (!resultCargarComp) RadMessageBox.Show(this.LP.GetText("errNoExisteComp", "No existe el comprobante"), error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();

                RadMessageBox.Show(this.LP.GetText("errRecuperandoComp", "Error recuperando el comprobante") + " (" + ex.Message + ")", error);
            }
        }

        /// <summary>
        /// Busca la descripción del código de error en la tabla GLT05)
        /// </summary>
        /// <param name="codiError"></param>
        /// <returns></returns>
        private string DescripcionMensajeError(string codiError)
        {
            string result = "";

            IDataReader dr = null;
            try
            {
                string prefijoTabla = "";
                if (this.bbddDB2)
                {
                    prefijoTabla = ConfigurationManager.AppSettings["bbddCGAPP"];
                    if (prefijoTabla != null && prefijoTabla != "") prefijoTabla += ".";
                }
                else prefijoTabla = GlobalVar.PrefijoTablaCG;

                string query = "select * from " + prefijoTabla + "GLT05 ";
                query += "where CODITM = '" + codiError + "'";

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);
                if (dr.Read())
                {
                    result = dr["DEMETM"].ToString().Trim();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
                if (dr != null) dr.Close();
            }
            return (result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefijo"></param>
        /// <param name="bibliotecaTablasLoteAS"></param>
        /// <param name="extendido"></param>
        private void VerificarExistenTablasLotesErroneos(string prefijo, string biblioteca, bool extendido)
        {
            if (this.bbddDB2)
            {
                if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                else this.bibliotecaTablasLoteAS = "";
            }

            if (!extendido)
            {
                //Si existe la tabla W30
                if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W30")) this.existeTablaW30 = false;
                else this.existeTablaW30 = true;

                //Si existe la tabla W31
                if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W31")) this.existeTablaW31 = false;
                else this.existeTablaW31 = true;
            }
            else
            {
                //Si existe la tabla W40
                if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W40")) this.existeTablaW40 = false;
                else this.existeTablaW40 = true;

                //Si existe la tabla W41
                if (!utilesCG.ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W41")) this.existeTablaW41 = false;
                else this.existeTablaW41 = true;
            }
        }


        /// <summary>
        /// Traspasa automaticamente los comprobantes de errores de las tablas W30/W31 o W40/W41 a las tablas W00/W01 o W10/W11
        /// y vacias las tablas de comprobantes de errores de origen
        /// </summary>
        /// <param name="prefijo"></param>
        /// <param name="biblioteca"></param>
        /// <param name="extendido"></param>
        /// <param name="confirmar"></param>
        private void TraspasarComprobantesErroneosDeTabla(string prefijo, string biblioteca, bool extendido, bool confirmar)
        {
            try
            {
                string nombreTabla = "";

                if (this.bbddDB2)
                {
                    if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                    else this.bibliotecaTablasLoteAS = "";
                    nombreTabla = this.bibliotecaTablasLoteAS + prefijo;
                }
                else nombreTabla = GlobalVar.PrefijoTablaCG + prefijo;

                bool existenDatosLote = false;
                bool existenDatosLoteExt = false;

                if (extendido)
                {
                    if (!this.existeTablaW40)
                    {
                        RadMessageBox.Show("No existe la tabla de lotes de comprobantes extendidos (" + prefijo + "W40)");      //Falta traducir
                        return;
                    }
                    else
                    {
                        //Chequear si existen lotes de comprobantes extendidos
                        existenDatosLoteExt = this.VerificarExistenDatosLote(nombreTabla + "W40");
                    }
                }
                else
                {
                    if (!this.existeTablaW30)
                    {
                        RadMessageBox.Show("No existe la tabla de lotes de comprobantes (" + prefijo + "W30)");      //Falta traducir
                        return;
                    }
                    else
                    {
                        //Chequear si existen lotes de comprobantes
                        existenDatosLote = this.VerificarExistenDatosLote(nombreTabla + "W30");
                    }
                }

                string traspCompErroneos = "";
                if (existenDatosLote || existenDatosLoteExt)
                {
                    bool continuar = true;
                    //Pedir confirmacion traspasar comprobantes erróneos del lote
                    if (confirmar)
                    {
                        string mensaje = this.LP.GetText("confTraspasarCompErroresLote", "Hay comprobantes erróneos en el lote. ¿Quiere traspasarlos?");
                        DialogResult resultConf = RadMessageBox.Show(mensaje, this.LP.GetText("lblConfirm", "Confirmación"), MessageBoxButtons.YesNo);
                        if (resultConf == DialogResult.No)
                        {
                            continuar = false;
                        }
                    }

                    if (continuar)
                    {
                        if (existenDatosLote) traspCompErroneos = this.CopiarLoteErroneoVaciasTablas(nombreTabla + "W30", nombreTabla + "W31", nombreTabla + "W00", nombreTabla + "W01");
                        else if (existenDatosLoteExt) traspCompErroneos = this.CopiarLoteErroneoVaciasTablas(nombreTabla + "W40", nombreTabla + "W41", nombreTabla + "W10", nombreTabla + "W11");
                    }
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }
        }

        /// <summary>
        /// Verifica si existen datos del lote en las tablas de errores de lote (W30/31 o W40/41)
        /// </summary>
        /// <returns></returns>
        public bool VerificarExistenDatosLote(string tabla)
        {
            //ExisteTabla(this.tipoBaseDatosCG, this.bibliotecaTablasLoteAS + prefijo + "W10")
            bool result = false;
            try
            {
                string query = "select count(*) from " + tabla;
                int cantRegistros = Convert.ToInt16(GlobalVar.ConexionCG.ExecuteScalar(query, GlobalVar.ConexionCG.GetConnectionValue));

                if (cantRegistros > 0) result = true;
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Traspasa los comprobantes y vacia las tablas
        /// </summary>
        /// <param name="tablaOrigenCab">tabla cabecera de lotes erróneos (W30/W40)</param>
        /// <param name="tablaOrigenDet">tabla detalles de lotes erróneos (W31/W41)</param>
        /// <param name="tablaDestinoCab">tabla cabecera de lotes (W00/W01)</param>
        /// <param name="tablaDestinoDet">tabla detalles de lotes (W10/W11)</param>
        /// <returns></returns>
        private string CopiarLoteErroneoVaciasTablas(string tablaOrigenCab, string tablaOrigenDet, string tablaDestinoCab, string tablaDestinoDet)
        {
            string result = "";
            int registros;
            try
            {
                //Traspasar Cabeceras
                string query = "insert into " + tablaDestinoCab;
                query += " select * from " + tablaOrigenCab;

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Traspasar Detalles
                query = "insert into " + tablaDestinoDet;
                query += " select * from " + tablaOrigenDet;

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Vaciar Cabeceras
                query = "delete from " + tablaOrigenCab;

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

                //Vacias Detalles
                query = "delete from " + tablaOrigenDet;

                registros = GlobalVar.ConexionCG.ExecuteNonQuery(query, GlobalVar.ConexionCG.GetConnectionValue);

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                result = this.LP.GetText("errTraspasandoComp", "Error traspasando los lotes") + " (" + ex.Message + ")";
            }
            return (result);
        }

        /// <summary>
        /// Busca el comprobante y lo carga
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="prefijo"></param>
        /// <param name="tabla"></param>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="numComp"></param>
        /// <param name="extendido"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool CargarComprobante(string compania, string prefijo, string tabla, string ano, string periodo, string tipo, string numComp, bool extendido, ref string error, string biblioteca)
        {
            bool result = false;
            IDataReader dr = null;
            try
            {
                //Buscar el comprobante
                string query = "select * from " + GlobalVar.PrefijoTablaCG + this.bibliotecaTablasLoteAS + prefijo + tabla;
                query += " where CCIAWS = '" + compania + "' and ";
                if (this.bbddDB2) query += "AÑOCWS = " + ano + " and ";
                else query += "AVOCWS = " + ano + " and ";
                query += "LAPSWS = " + periodo + " and ";
                query += "TICOWS =" + tipo + " and ";
                query += "NUCOWS =" + numComp;

                ComprobanteContable comprobante = new ComprobanteContable();

                dr = GlobalVar.ConexionCG.ExecuteReader(query, GlobalVar.ConexionCG.GetConnectionValue);

                if (dr.Read())
                {
                    //Crear el comprobante contable

                    string codigo = dr["CCIAWS"].ToString().Trim();
                    comprobante.Cab_compania = codigo;

                    string aapp = "";
                    if (this.bbddDB2) aapp = dr[2].ToString().Trim().PadLeft(2, '0') + dr["LAPSWS"].ToString().Trim().PadLeft(2, '0');
                    else aapp = dr["AVOCWS"].ToString().Trim().PadLeft(2, '0') + dr["LAPSWS"].ToString().Trim().PadLeft(2, '0');
                    comprobante.Cab_anoperiodo = aapp;

                    comprobante.Cab_descripcion = dr["DOCDWS"].ToString().Trim();

                    tipo = dr["TICOWS"].ToString().Trim();
                    comprobante.Cab_tipo = tipo.PadLeft(2, '0');
                    comprobante.Cab_noComprobante = dr["NUCOWS"].ToString().Trim();

                    string fecha = "";
                    if (this.bbddDB2) fecha += dr["AÑOEWS"].ToString().Trim();
                    else fecha += dr["AVOEWS"].ToString().Trim();

                    if (fecha == "") fecha += "00";
                    else
                    {
                        fecha = fecha.PadLeft(2, '0');
                        int anoCorte = Convert.ToInt16(CGParametrosGrles.GLC01_ALSIRC);

                        int anoOrigenInt = Convert.ToInt32(fecha);
                        if (anoOrigenInt < anoCorte) anoOrigenInt = 2000 + anoOrigenInt;
                        else if (anoOrigenInt >= anoCorte) anoOrigenInt = 1900 + anoOrigenInt;

                        fecha = anoOrigenInt.ToString();
                    }

                    fecha += dr["MESEWS"].ToString().PadLeft(2, '0');
                    fecha += dr["DIAEWS"].ToString().PadLeft(2, '0');

                    comprobante.Cab_fecha = fecha;
                    comprobante.Cab_clase = dr["TVOUWS"].ToString().Trim();
                    comprobante.Cab_tasa = dr["TASCWS"].ToString().Trim();

                    if (extendido) comprobante.Cab_extendido = "1";
                    else comprobante.Cab_extendido = "0";

                    if (this.bbddDB2) ano = dr["AÑOCWS"].ToString().Trim();
                    else ano = dr["AVOCWS"].ToString().Trim();

                    periodo = dr["LAPSWS"].ToString().Trim();

                    //Obtener los detalles del comprobante a importar
                    comprobante.Det_detalles = this.ObtenerDetallesComprobanteImportar(this.bibliotecaTablasLoteAS + prefijo,
                                                                                        comprobante.Cab_compania, ano, periodo,
                                                                                        tipo, comprobante.Cab_noComprobante,
                                                                                        extendido);

                    result = true;
                }

                dr.Close();

                if (result)
                {
                    //Cerrar el formulario actual ???
                    frmCompContAltaEdita frmCompCont = new frmCompContAltaEdita
                    {
                        ImportarComprobante = true,
                        EdicionLoteError = true,
                        BatchLoteError = true,
                        Batch = true
                    };
                    comprobante.Biblioteca = biblioteca;
                    comprobante.Prefijo = prefijo;
                    frmCompCont.ComprobanteContableImportar = comprobante;
                    frmCompCont.NombreComprobante = comprobante.Cab_descripcion;
                    frmCompCont.FrmPadre = this;
                    frmCompCont.ArgSel += new frmCompContAltaEdita.ActualizaListaComprobantes(ActualizaListaComprobantes_ArgSel);
                    frmCompCont.Show();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));

                if (dr != null) dr.Close();
                error = this.LP.GetText("errCargandoComp", "Error cargando el comprobante") + " (" + ex.Message + ")";
            }
            return (result);
        }

        private void ActualizaListaComprobantes_ArgSel(frmCompContAltaEdita.ActualizaListaComprobantesArgs e)
        {
            try
            {
                string Cab_compania = e.Valor[0].ToString().Trim();
                string aapp = e.Valor[1].ToString().Trim();
                string Cab_anoperiodo = e.Valor[1].ToString().Trim(); 
                string Cab_tipo = e.Valor[2].ToString().Trim();
                string Cab_noComprobante = e.Valor[3].ToString().Trim();
                //string Cab_fecha = utiles.FormatoCGToFecha(e.Valor[4].ToString().Trim()).ToShortDateString();
                string Cab_fecha = e.Valor[4].ToString();
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
                string Descripcion = e.Valor[18].ToString().Trim();
                string Clase = e.Valor[19].ToString().Trim();
                string Tasa = e.Valor[20].ToString().Trim();
                if (Tasa == "") Tasa = "0,0000000";

                // busco fila seleccionada.
                for (int i = 0; i < this.radGridViewEditarLotes.Rows.Count; i++)
                {
                    
                    if ((this.radGridViewEditarLotes.Rows[i].Cells[0].Value.ToString().Trim() == Cab_compania_ant)
                    && (this.radGridViewEditarLotes.Rows[i].Cells[1].Value.ToString().Trim() == Cab_anoperiodo_ant)
                    && (this.radGridViewEditarLotes.Rows[i].Cells[3].Value.ToString().Trim().PadLeft(2, '0') == Cab_tipo_ant)
                    && (this.radGridViewEditarLotes.Rows[i].Cells[4].Value.ToString().Trim() == Cab_noComprobante_ant)
                    && (this.radGridViewEditarLotes.Rows[i].Cells[5].Value.ToString().Trim() == Cab_fecha_ant)
                    )
                    {
                        this.radGridViewEditarLotes.Rows[i].Cells[0].Value = Cab_compania;
                        this.radGridViewEditarLotes.Rows[i].Cells[1].Value = Cab_anoperiodo;
                        this.radGridViewEditarLotes.Rows[i].Cells[3].Value = Cab_tipo;
                        this.radGridViewEditarLotes.Rows[i].Cells[4].Value = Cab_noComprobante;
                        this.radGridViewEditarLotes.Rows[i].Cells[5].Value = Cab_fecha;
                        this.radGridViewEditarLotes.Rows[i].Cells["TVOUWS"].Value = Clase.ToString().Trim();
                        this.radGridViewEditarLotes.Rows[i].Cells["DOCDWS"].Value = Descripcion.ToString().Trim();
                        this.radGridViewEditarLotes.Rows[i].Cells["TASCWS"].Value = Tasa.ToString().Trim();

                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(Utiles.CreateExceptionString(ex));
            }
        }
        /// <summary>
        /// Carga los lotes procesados
        /// </summary>
        private void GestionLotesProcesados()
        {
            //Habilitar Interface de Edicion de Lotes Procesados
            this.UI = GestionLotesUserInterfaces.EdicionLotesProcesados;

            //La Grid de lotes de error tiene que estar a tamaño grande
            if (!this.lotesErrorSizeGrande)
            {
                this.gbGridLotesErrores.Height = this.gbGridLotesErrores.Size.Height + this.gbGridCompErrores.Size.Height;
                this.lotesErrorSizeGrande = true;
            }

            //Habilitar Edicion de listado de errores
            this.radCollapsiblePanelBuscador.Collapse();
            this.radCollapsiblePanelBuscador.BringToFront();
            this.radCollapsiblePanelBuscador.Visible = true;
            this.gbEdicionLotes.Visible = false;
            utiles.ButtonEnabled(ref this.radButtonGestionLotesProc, false);
            utiles.ButtonEnabled(ref this.radButtonEdicionLotes, true);
            utiles.ButtonEnabled(ref this.radButtonSuprimirHco, true);
            this.gbBuscador.Visible = true;
            this.radCollapsiblePanelBuscador.BringToFront();
            this.radCollapsiblePanelBuscador.Visible = true;

            //Ocultar las Grid y la etiquete de Info de Resultados
            this.gbGridEditarLotes.Visible = false;
            this.radGridViewEditarLotes.Visible = false;
            this.gbGridCompErrores.Visible = false;
            this.radGridViewCompErrores.Visible = false;
            this.gbGridLotesErrores.Visible = true;
            this.radGridViewLotesErrores.Visible = true;
            this.lblInfo.Visible = false;

            //Ocultar Suprimir histórico de Mensajes
            this.gbSuprimirHco.Visible = false;

            this.mostrarLotesTodos = false;

            this.AcceptButton = this.btnBuscadorBuscar;

            this.FillDataGridtgGridLotesErrores();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechadesde"></param>
        /// <returns></returns>
        private string FormValidHcoMensajes(ref int fechadesde)
        {
            string result = "";

            try
            {
                this.txtMaskHastaFecha.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string fecha = this.txtMaskHastaFecha.Value.ToString();
                this.txtMaskHastaFecha.TextMaskFormat = MaskFormat.IncludeLiterals;

                if (fecha == "")
                {
                    result = this.LP.GetText("errFechaHastaNoInf", "Debe indicar una fecha hasta la cual se va a suprimir el histórico de mensajes de error");
                    this.txtMaskHastaFecha.Focus();
                    return (result);
                }

                fecha = this.txtMaskHastaFecha.Text.Trim();
                try
                {
                    DateTime dt = Convert.ToDateTime(fecha);
                    fechadesde = utiles.FechaToFormatoCG(dt, true);
                    fechadesde = 9999999 - fechadesde;
                    //fecha menor o igual que hasta
                    //filtro += string.Format("DATE24Origen >= '{0}'", fechaHastaFormatoCG.ToString());
                }
                catch (Exception ex)
                {
                    Log.Error(Utiles.CreateExceptionString(ex));
                    result = this.LP.GetText("errFechaDesdeFormatoNoValido", "La fecha desde no tiene un formato válido");
                    this.txtMaskBuscadorFechaDesde.Focus();
                }
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Eliminar mensajes de error
        /// </summary>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        private string EliminarMensajesError(int fechaHasta, string prefijo, string biblioteca, string fecha, string hora)
        {
            string result = "";
            string filtro24 = "";
            string filtro25 = "";
            try
            {
                //Eliminar mensajes de la tabla GLC25
                string query24 = "delete from " + GlobalVar.PrefijoTablaCG + "GLC24 where ";
                string query25 = "delete from " + GlobalVar.PrefijoTablaCG + "GLC25 where ";

                if (fechaHasta != 0)
                {
                    filtro24 += "DATE24 >= " + fechaHasta.ToString();
                    filtro25 += "DATE25 >= " + fechaHasta.ToString();
                }

                if (prefijo != "")
                {
                    if (filtro24 != "") filtro24 += " AND ";
                    filtro24 += " PREF24 = '" + prefijo + "'";
                    if (filtro25 != "") filtro25 += " AND ";
                    filtro25 += " PREF25 = '" + prefijo + "'";
                }

                if (biblioteca != "")
                {
                    if (filtro24 != "") filtro24 += " AND ";
                    filtro24 += " LIBL24 = '" + biblioteca + "'";
                    if (filtro25 != "") filtro25 += " AND ";
                    filtro25 += " LIBL25 = '" + biblioteca + "'";
                }

                if (fecha != "")
                {
                    if (filtro24 != "") filtro24 += " AND ";
                    filtro24 += " DATE24 = " + fecha;
                    if (filtro25 != "") filtro25 += " AND ";
                    filtro25 += " DATE25 = " + fecha;
                }

                if (hora != "")
                {
                    if (filtro24 != "") filtro24 += " AND ";
                    filtro24 += " TIME24 = " + hora;
                    if (filtro25 != "") filtro25 += " AND ";
                    filtro25 += " TIME25 = " + hora;
                }

                query24 = query24 + filtro24;
                query25 = query25 + filtro25;

                int cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(query25, GlobalVar.ConexionCG.GetConnectionValue);

                cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(query24, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }

        /// <summary>
        /// Elimina los apuntes del comprobante
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="numeroComp"></param>
        private string EliminarApuntesComp(string compania, string ano, string periodo, string tipo, string numeroComp)
        {
            string result = "";
            try
            {
                string prefijo = this.txtPrefijo.Text.Trim().ToUpper();
                string biblioteca = "";

                if (this.bbddDB2)
                {
                    biblioteca = this.txtBiblioteca.Text.Trim();

                    if (biblioteca != "") this.bibliotecaTablasLoteAS = biblioteca + ".";
                    else this.bibliotecaTablasLoteAS = "";
                }

                //Eliminar la cabecera del comprobante
                string tabla = "W00";
                if (this.radToggleSwitchFormatoAmpliado.Value) tabla = "W10";

                string query = "delete from " + GlobalVar.PrefijoTablaCG + this.bibliotecaTablasLoteAS + prefijo + tabla;
                string queryWhere = " where CCIAWS = '" + compania + "' and ";

                if (this.bbddDB2) queryWhere += " AÑOCWS = " + ano + " and ";
                else queryWhere += " AVOCWS = " + ano + " and ";

                queryWhere += " LAPSWS = " + periodo + " and ";
                queryWhere += " TICOWS = " + tipo + " and ";
                queryWhere += " NUCOWS = " + numeroComp;

                string queryAux = query + queryWhere;
                int cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(queryAux, GlobalVar.ConexionCG.GetConnectionValue);

                //Eliminar todos los apuntes de detalle del comprobante
                tabla = "W01";
                if (this.radToggleSwitchFormatoAmpliado.Value) tabla = "W11";
                query = "delete from " + GlobalVar.PrefijoTablaCG + this.bibliotecaTablasLoteAS + prefijo + tabla;

                queryAux = query + queryWhere;
                cantReg = GlobalVar.ConexionCG.ExecuteNonQuery(queryAux, GlobalVar.ConexionCG.GetConnectionValue);
            }
            catch (Exception ex) { Log.Error(Utiles.CreateExceptionString(ex)); }

            return (result);
        }
        #endregion
    }
}