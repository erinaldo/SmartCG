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

namespace ModConsultaInforme
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable
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

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo de Consultas e Informes");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Inhabilita opciones del menú, si el usuario no tiene permisos
            this.AutorizacionesProcesos();
        }

        private void subMenuItemDiarioDetallado_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoDiarioAltaEdita frmInfoDiarioDetallado = new frmInfoDiarioAltaEdita();
            //frmInfoDiarioDetallado.InformeDiarioDetalladoTipo = frmInfoDiarioAltaEdita.DiarioDetalladoTipo.Detallado;
            frmInfoDiarioDetallado.InformeDiarioDetalladoTipoStr = "DE";
            frmInfoDiarioDetallado.FrmPadre = this;
            frmInfoDiarioDetallado.Show();
        }

        private void subMenuItemDiarioResFecha_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoDiarioAltaEdita frmInfoDiarioResFecha = new frmInfoDiarioAltaEdita();
            //frmInfoDiarioResFecha.InformeDiarioDetalladoTipo = frmInfoDiarioAltaEdita.DiarioDetalladoTipo.ResumidoFecha;
            frmInfoDiarioResFecha.InformeDiarioDetalladoTipoStr = "RF";
            frmInfoDiarioResFecha.FrmPadre = this;
            frmInfoDiarioResFecha.Show();
        }

        private void subMenuItemDiarioResPeriodo_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoDiarioAltaEdita frmInfoDiarioResPeriodo = new frmInfoDiarioAltaEdita();
            //frmInfoDiarioResPeriodo.InformeDiarioDetalladoTipo = frmInfoDiarioAltaEdita.DiarioDetalladoTipo.ResumidoPeriodo;
            frmInfoDiarioResPeriodo.InformeDiarioDetalladoTipoStr = "RP";
            frmInfoDiarioResPeriodo.FrmPadre = this;
            frmInfoDiarioResPeriodo.Show();
        }

        private void subMenuItemBalanceSumSaldos_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoBalanceSumSaldosAltaEdita frmInfoBalanceSumasSaldos = new frmInfoBalanceSumSaldosAltaEdita();
            frmInfoBalanceSumasSaldos.FrmPadre = this;
            frmInfoBalanceSumasSaldos.Show();
        }

        private void subMenuItemMayorContab_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoMayorContabAltaEdita frmInfoMayorcontab = new frmInfoMayorContabAltaEdita();
            frmInfoMayorcontab.FrmPadre = this;
            frmInfoMayorcontab.Show();
        }

        private void subMenuItemMovimientosAux_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoMovAuxiliarAltaEdita frmInfoMovAux = new frmInfoMovAuxiliarAltaEdita();
            frmInfoMovAux.FrmPadre = this;
            frmInfoMovAux.Show();
        }

        private void subMenuItemMovIVA_Click(object sender, EventArgs e)
        {
            //OJO !! habría que llamar al formulario con el resultado de informes anteriores
            frmInfoMovIVAAltaEdita frmInfoMovIVA = new frmInfoMovIVAAltaEdita();
            frmInfoMovIVA.FrmPadre = this;
            frmInfoMovIVA.Show();
        }

        private void menuItemConfig_Click(object sender, EventArgs e)
        {
            frmConfig frmConfiguracion = new frmConfig();
            frmConfiguracion.FrmPadre = this;
            frmConfiguracion.Show();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            frmAbout frmInfo = new frmAbout();
            frmInfo.FrmPadre = this;
            frmInfo.Show();
        }

        private void subMenuItemConsAuxiliar_Click(object sender, EventArgs e)
        {
            frmConsAuxiliar frmConsultaAux = new frmConsAuxiliar();
            frmConsultaAux.FrmPadre = this;
            frmConsultaAux.Show();
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                menuItemSalir_Click(sender, null);
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo de Consultas e Informes");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo de Consulta e Informes");
            //Menú
            this.menuItemInformes.Text = this.LP.GetText("menuItemInformes", "&Informes");
            this.menuItemConsultas.Text = this.LP.GetText("menuItemConsultas", "Consul&tas");
            this.menuItemConfig.Text = this.LP.GetText("menuItemConfig", "&Configuración");
            this.menuItemAbout.Text = this.LP.GetText("menuItemAbout", "&Acerca de");
            this.menuItemSalir.Text = this.LP.GetText("menuItemSalir", "&Salir");
            this.subMenuItemDiarioDetallado.Text = this.LP.GetText("subMenuItemDiarioDetallado", "Diario &Detallado");
            this.subMenuItemDiarioResFecha.Text = this.LP.GetText("subMenuItemDiarioResFecha", "Diario Resumido &Fecha");
            this.subMenuItemDiarioResPeriodo.Text = this.LP.GetText("subMenuItemDiarioResPeriodo", "Diario Resumido &Períodos");
            this.subMenuItemBalanceSumSaldos.Text = this.LP.GetText("subMenuItemBalanceSumSaldos", "&Balance Sumas y Saldos");
            this.subMenuItemMayorContab.Text = this.LP.GetText("subMenuItemMayorContab", "&Mayor de Contabilidad");
            this.subMenuItemMovimientosAux.Text = this.LP.GetText("subMenuItemMovimientosAux", "Movimientos de &Auxiliar");
            this.subMenuItemMovIVA.Text = this.LP.GetText("subMenuItemMovIVA", "Movimientos de &IVA");
        }

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

            //860  -- Listado de IVA
            autClaseElemento = "860";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta) this.subMenuItemMovIVA.Enabled = false;
        }
        #endregion

        private void solicitudDeInformesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInfoSolicitudInforme frmSolicitudInforme = new frmInfoSolicitudInforme();
            frmSolicitudInforme.FrmPadre = this;
            frmSolicitudInforme.Show();
        }

        private void listaDeInformesGeneradosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}