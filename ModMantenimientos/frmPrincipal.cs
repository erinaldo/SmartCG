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

namespace ModMantenimientos
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable, IPrincipalOpcionesMenu
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        #region Eventos
        void IReLocalizable.ReLocalize()
        {
            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        void IPrincipalOpcionesMenu.ActualizaOpcionesMenuPermisos()
        {
            //Activar o desactivar opciones del menú dependiendo del usuario logado
            //Se invoca desde la opcion de login CG de la Lanzadera
            this.AutorizacionesProcesos();
        }

        private void submenuItemCompanias_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoCompanias", "Mantenimiento de Compañías");
            frmMtoLista.Tabla = "GLM01";
            frmMtoLista.AutClaseElemento = "002";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show(this);

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemGruposCompanias_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoGruposCompanias", "Mantenimiento de Grupos de Compañías");
            frmMtoLista.Tabla = "GLM07";
            frmMtoLista.AutClaseElemento = "001";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemPlanesCtas_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoPlanesCtas", "Mantenimiento de Planes de Cuentas");
            frmMtoLista.Tabla = "GLM02";
            frmMtoLista.AutClaseElemento = "003";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemTiposAuxiliares_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoTiposAuxiliares", "Mantenimiento de Tipos de Auxiliares");
            frmMtoLista.Tabla = "GLM04";
            frmMtoLista.AutClaseElemento = "006";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemTablaMonedasExt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoTablasMonedasExt", "Mantenimiento de Tablas de Monedas Extranjeras");
            frmMtoLista.Tabla = "GLT03";
            frmMtoLista.AutClaseElemento = "826";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemTablaCalendarios_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoGLT04Sel frmMtoGLT04Buscar = new frmMtoGLT04Sel();
            frmMtoGLT04Buscar.FrmPadre = this;
            frmMtoGLT04Buscar.Show();
            
            Cursor.Current = Cursors.Default;
        }

        private void submenuItemClaseZona_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoClaseZona", "Mantenimiento de Clase de Zona");
            frmMtoLista.Tabla = "GLM10";
            frmMtoLista.AutClaseElemento = "024";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemZona_Click(object sender, EventArgs e)
        {
            //Autorizaciones ¿¿?¿?
            Cursor.Current = Cursors.WaitCursor;

            frmMtoGLM11Sel frmMtoZonas = new frmMtoGLM11Sel();
            frmMtoZonas.FrmPadre = this;
            frmMtoZonas.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemCuentasAux_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoGLM05Sel frmMtoCtasAux = new frmMtoGLM05Sel();
            frmMtoCtasAux.FrmPadre = this;
            frmMtoCtasAux.Show();

            Cursor.Current = Cursors.Default;
        }

        private void submenuItemCuentasMayor_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoGLM03Sel frmMtoCtasMayor = new frmMtoGLM03Sel();
            frmMtoCtasMayor.FrmPadre = this;
            frmMtoCtasMayor.Show();

            Cursor.Current = Cursors.Default;
        }

        private void gruposDeCuentasDeAuxiliarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoGrupoCuentasAux", "Mantenimiento de Grupos de Cuentas de Auxiliar");
            frmMtoLista.Tabla = "GLT08";
            frmMtoLista.AutClaseElemento = "007";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void gruposDeCuentasDeMayorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoGrupoCuentasMayor", "Mantenimiento de Grupos de Cuentas de Mayor");
            frmMtoLista.Tabla = "GLT22";
            frmMtoLista.AutClaseElemento = "008";
            frmMtoLista.AutGrupo = "01";
            frmMtoLista.AutOperConsulta = "10";
            frmMtoLista.AutOperModifica = "20";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();
            Cursor.Current = Cursors.Default;
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmAbout frmInfo = new frmAbout();
            frmInfo.FrmPadre = this;
            frmInfo.Show();

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo Mantenimientos");

            this.menuItemTablasMaestras.Text = this.LP.GetText("menuItemTablasMaestras", "Tablas &Maestras");
            this.menuItemTablasAux.Text = this.LP.GetText("menuItemTablasAux", "Tablas Au&xiliares");
            this.menuItemAbout.Text = this.LP.GetText("menuItemAbout", "&Acerca de");
            this.menuItemSalir.Text = this.LP.GetText("menuItemSalir", "&Salir");

            //Tablas Maestras
            this.submenuItemCompanias.Text = this.LP.GetText("submenuItemCompanias", "&Compañías");
            this.submenuItemGruposCompanias.Text = this.LP.GetText("submenuItemGruposCompanias", "&Grupos de compañías");
            this.submenuItemPlanesCtas.Text = this.LP.GetText("submenuItemPlanesCtas", "&Planes de cuentas");
            this.submenuItemTiposAuxiliares.Text = this.LP.GetText("submenuItemTiposAux", "&Tipos de auxiliares");
            this.submenuItemClaseZona.Text = this.LP.GetText("submenuItemClaseZona", "&Clase de zona");
            this.submenuItemZona.Text = this.LP.GetText("submenuItemZona", "&Zona");
            this.submenuItemCuentasAux.Text = this.LP.GetText("submenuItemCtasAux", "Cuentas de &auxiliar");
            this.submenuItemCuentasMayor.Text = this.LP.GetText("submenuItemCtasMayor", "Cuentas de &mayor");
            this.submenuItemMaestroCIF_DNI.Text = this.LP.GetText("submenuItemMaestroCIF_DNI", "Maestros de CIF / &DNI");
            this.submenuItemCompaniasFiscales.Text = this.LP.GetText("submenuItemCompaniasFiscales", "Compañías &fiscales");

            //Tablas Auxiliares
            this.submenuItemTablaMonedasExt.Text = this.LP.GetText("submenuItemTablaMonedaExt", "&Tabla de monedas extranjeras");
            this.submenuItemTablaCalendarios.Text = this.LP.GetText("submenuItemTablaCalendar", "Tabla de &calendarios");
            this.submenuItemGruposDeCuentasDeAuxiliar.Text = this.LP.GetText("submenuItemGrupoCtasAux", "&Grupos de cuentas de auxiliar");
            this.submenuItemGruposDeCuentasDeMayor.Text = this.LP.GetText("submenuItemGrupoCtaMayor", "Grupos de cuentas de &mayor");
            this.submenuItemTiposExtracontables.Text = this.LP.GetText("submenuItemTiposExt", "Tipos de &extracontables");
            this.submenuItemTiposComprobantes.Text = this.LP.GetText("submenuItemTiposComprobantes", "Tipos de &comprobantes");
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo de Mantenimientos");

            //Centrar formulario
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Top = (rect.Height / 2) - (this.Height / 2);
            this.Left = (rect.Width / 2) - (this.Width / 2);

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            this.TraducirLiterales();

            //Inhabilita opciones del menú, si el usuario no tiene permisos
            this.AutorizacionesProcesos();
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                menuItemSalir_Click(sender, null);
            }
        }
        #endregion     

        #region Métodos Privados

        /// <summary>
        /// Inhabilita o no las opciones de menú relacionadas con procesos dependiendo de si el usuario tiene acceso o no
        /// </summary>
        private void AutorizacionesProcesos()
        {
            string usuario = ConfigurationManager.AppSettings["USER_CGIFS"];
            if (GlobalVar.UsuarioLogadoCG.ToUpper() == usuario) return;

            const string autGrupo = "01";
            const string autOperConsulta = "10";
            string autClaseElemento = "";
            bool operarConsulta;

            //826  -- Tabla de monedas
            autClaseElemento = "826";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta) this.submenuItemTablaMonedasExt.Enabled = false;

            //825  -- Tabla de calendarios
            autClaseElemento = "825";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta) this.submenuItemTablaCalendarios.Enabled = false;

            //850  -- Compañías fiscales
            autClaseElemento = "850";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta) this.submenuItemCompaniasFiscales.Enabled = false;

            //854  -- Maestro CIF / DNI
            autClaseElemento = "854";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta) this.submenuItemMaestroCIF_DNI.Enabled = false;
        }
        #endregion

        private void submenuItemTiposExtracontables_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoPRT03Sel frmMtoTiposExt = new frmMtoPRT03Sel();
            frmMtoTiposExt.FrmPadre = this;
            frmMtoTiposExt.Show();

            Cursor.Current = Cursors.Default;

        }

        private void submenuItemTiposComprobantes_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoGLT06Sel frmMtoTiposComp = new frmMtoGLT06Sel();
            frmMtoTiposComp.FrmPadre = this;
            frmMtoTiposComp.Show();

            Cursor.Current = Cursors.Default;

        }

        private void submenuItemMaestroCIF_DNI_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmListaGenerica frmMtoLista = new frmListaGenerica();
            frmMtoLista.Titulo = this.LP.GetText("lblTituloMtoMaestroCIF_DNI", "Mantenimiento de Maestro de CIF / DNI");
            frmMtoLista.Tabla = "IVM05";
            frmMtoLista.AutClaseElemento = "";
            frmMtoLista.AutGrupo = "";
            frmMtoLista.AutOperConsulta = "";
            frmMtoLista.AutOperModifica = "";
            frmMtoLista.FrmPadre = this;
            frmMtoLista.Show();
            
            Cursor.Current = Cursors.Default;
        }

        private void submenuItemCompaniasFiscales_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            frmMtoIVT03Sel frmMtoCompFiscales = new frmMtoIVT03Sel();
            frmMtoCompFiscales.FrmPadre = this;
            frmMtoCompFiscales.Show();
            
            Cursor.Current = Cursors.Default;
        }

        private void submenuItemCodigosIVA_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmMtoIVT01Sel frmMtoCodIVA = new frmMtoIVT01Sel();
            frmMtoCodIVA.FrmPadre = this;
            frmMtoCodIVA.Show();

            Cursor.Current = Cursors.Default;
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo de Mantenimientos");
        }

    }
}
