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
using log4net;

namespace ModSII
{
    public partial class frmPrincipal : frmPlantilla, IReLocalizable, IOpcionesMenu
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

        void IOpcionesMenu.ActualizaOpcionesMenuPermisos()
        {
            //Activar o desactivar opciones del menú dependiendo del usuario logado
            //Se invoca desde la opcion de login CG de la Lanzadera
            this.AutorizacionesProcesos();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Log.Info("INICIO Cargar Módulo SII");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();

            //Inhabilita opciones del menú, si el usuario no tiene permisos
            this.AutorizacionesProcesos();

            //Menu Soap - Visualiza la respuesta - Visible o no
            var siiMenuSoapViewValue = ConfigurationManager.AppSettings["SIIMenuSoapView"];
            if (!string.IsNullOrEmpty(siiMenuSoapViewValue))
            {
                if (siiMenuSoapViewValue.ToString() == "true") this.menuItemSoap.Visible = true;
                else this.menuItemSoap.Visible = false;
            }
            else this.menuItemSoap.Visible = false;
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmAbout frmInfo = new frmAbout();
            frmInfo.FrmPadre = this;
            frmInfo.Show();

            Cursor.Current = Cursors.Default;
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) menuItemSalir_Click(sender, null);
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemConfig_Click(object sender, EventArgs e)
        {
            frmConfig frmConf = new frmConfig();
            frmConf.FrmPadre = this;
            frmConf.ShowDialog();
        }

        private void subMenuItemSoapRespuestaVisualizar_Click(object sender, EventArgs e)
        {
            frmSiiSoapResponseView frmSiiSoapRespView = new frmSiiSoapResponseView();
            frmSiiSoapRespView.FrmPadre = this;
            frmSiiSoapRespView.Show();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo SII");
        }

        private void subMenuItemEnviarInfoSII_Click(object sender, EventArgs e)
        {
            this.EnviarSII();
        }

        private void subMenuItemConsultarInfoSII_Click(object sender, EventArgs e)
        {
            this.ConsultarSII();
        }

        private void subMenuItemHcoEnviosSII_Click(object sender, EventArgs e)
        {
            this.ConsultarLocal();
        }

        private void subMenuItemConsultarFacturasPorCliente_Click(object sender, EventArgs e)
        {
            this.ConsultarSIIFacturasExpedidasPorCliente();
        }

        private void subMenuItemConsultarFacturasAgrupadasPorCliente_Click(object sender, EventArgs e)
        {
            this.ConsultarSIIFacturasExpedidasAgrupadasPorCliente();
        }

        private void subMenuItemConsultarFacturasPorProveedor_Click(object sender, EventArgs e)
        {
            this.ConsultarSIIFacturasExpedidasPorProveedor();
        }

        private void subMenuItemConsultarFacturasAgrupadasPorProveedor_Click(object sender, EventArgs e)
        {
            this.ConsultarSIIFacturasExpedidasAgrupadasPorProveedor();
        }

        private void subMenuComprobarCuadre_Click(object sender, EventArgs e)
        {
            this.ComprobarCuadre();
        }

        private void subMenuItemSituacionGralSII_Click(object sender, EventArgs e)
        {
            this.SituacionGralSII();
        }

        private void subMenuItemConsultarEnviosSII_Click(object sender, EventArgs e)
        {
            this.ConsultarEnviosSII();
        }

        private void subMenuItemAlertasEnvios_Click(object sender, EventArgs e)
        {
            frmSiiAlerta frmSiiAlertaEnvios = new frmSiiAlerta();
            frmSiiAlertaEnvios.FrmPadre = this;
            frmSiiAlertaEnvios.Show();
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

            //860  -- Listados y Consultas de IVA
            autClaseElemento = "860";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);
            if (!operarConsulta)
            {
                this.subMenuItemConsultarInfoSII.Enabled = false;
                this.subMenuItemHcoEnviosSII.Enabled = false;
                this.subMenuComprobarCuadre.Enabled = false;
                this.subMenuItemSituacionGralSII.Enabled = false;
                this.subMenuItemConsultarEnviosSII.Enabled = false;
                this.subMenuItemAlertasEnvios.Enabled = false;
                this.subMenuItemConsultarFacturasPorCliente.Enabled = false;
                this.subMenuItemConsultarFacturasAgrupadasPorCliente.Enabled = false;
                this.subMenuItemConsultarFacturasPorProveedor.Enabled = false;
                this.subMenuItemConsultarFacturasAgrupadasPorProveedor.Enabled = false;
            }

            //866  -- Procesos anuales de IVA
            autClaseElemento = "866";
            operarConsulta = aut.Operar(autClaseElemento, autGrupo, autOperConsulta);

            if (!operarConsulta) this.subMenuItemEnviarInfoSII.Enabled = false;
        }

        private void TraducirLiterales()
        {
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo SII (Suministro Inmediato de la Información)");
            //this.Text += this.FormTituloAgenciaEntorno();

            this.subMenuItemSIIGestion.Text = "&" + this.LP.GetText("subMenuItemSIIGestion", "SII Gestión");
            this.subMenuItemEnviarInfoSII.Text = "&" + this.LP.GetText("subMenuItemEnviarInfoSII", "Enviar Información al SII");
            this.subMenuItemConsultarInfoSII.Text = "&" + this.LP.GetText("subMenuItemConsultarInfoSII", "Consultar Datos en el SII");
            string item = this.LP.GetText("subMenuItemHcoEnviosSII1raparte", "Consultar Datos en") + " &" +
                          this.LP.GetText("subMenuItemHcoEnviosSII2daparte", "Local");
            this.subMenuItemHcoEnviosSII.Text = item;
            item = this.LP.GetText("subMenuItemConsultarEnviosSII1raparte", "Consultar") + " &" +
                          this.LP.GetText("subMenuItemConsultarEnviosSII2daparte", "Envíos al SII");
            this.subMenuItemConsultarEnviosSII.Text = item;
            item = this.LP.GetText("subMenuComprobarCuadre1raparte", "Comprobar Datos") + " &" +
                          this.LP.GetText("subMenuComprobarCuadre2daparte", "Presentados y Datos Local");
            this.subMenuComprobarCuadre.Text = item;
            this.subMenuItemSituacionGralSII.Text = "&" + this.LP.GetText("subMenuItemSituacionGralSII", "Situación General SII");

            this.menuItemConfig.Text = "&" + this.LP.GetText("menuItemConfig", "Configuración");

            this.menuItemAbout.Text = "&" + this.LP.GetText("menuItemAbout", "Acerca de");

            this.menuItemSalir.Text = "&" + this.LP.GetText("toolStripButtonSalir", "Salir");
        }

        /// <summary>
        /// Enviar la información al SII
        /// </summary>
        private void EnviarSII()
        {
            frmSiiSuministroInfo frmSiiSumInfo = new frmSiiSuministroInfo();
            frmSiiSumInfo.FrmPadre = this;
            frmSiiSumInfo.Show();
        }

        /// <summary>
        /// Consultar la información al SII
        /// </summary>
        private void ConsultarSII()
        {
            frmSiiConsultaInfo frmSiiConInfo = new frmSiiConsultaInfo();
            frmSiiConInfo.FrmPadre = this;
            frmSiiConInfo.Show();
        }

        /// <summary>
        /// Consultar la información en local
        /// </summary>
        private void ConsultarLocal()
        {
            frmSiiDatosLocal frmSiiConDatosLocal = new frmSiiDatosLocal();
            frmSiiConDatosLocal.FrmPadre = this;
            frmSiiConDatosLocal.Show();
        }

        /// <summary>
        /// Comprueba si hay descuadre entre la información enviada al SII y los Datos en Local
        /// </summary>
        private void ComprobarCuadre()
        {
            frmSiiLocalDescuadre frmSiiLocalDescuadreView = new frmSiiLocalDescuadre();
            frmSiiLocalDescuadreView.FrmPadre = this;
            frmSiiLocalDescuadreView.Show();
        }

        /// <summary>
        /// Visualizar la Situación General del SII
        /// </summary>
        private void SituacionGralSII()
        {
            frmSiiSituacionGral frmSiiSituacion = new frmSiiSituacionGral();
            frmSiiSituacion.FrmPadre = this;
            frmSiiSituacion.Show();
        }

        /// <summary>
        /// Consultar los envíos realizados al SII
        /// </summary>
        private void ConsultarEnviosSII()
        {
            frmSiiEnviosConsulta frmSiiConsultaEnvios = new frmSiiEnviosConsulta();
            frmSiiConsultaEnvios.FrmPadre = this;
            frmSiiConsultaEnvios.Show();
        }

        /// <summary>
        /// Consultar las facturas expedidas informadas por cliente realizadas al SII
        /// </summary>
        private void ConsultarSIIFacturasExpedidasPorCliente()
        {
            frmSiiConsultaCliente frmSiiConsultaFacturasExpedidasPorCliente = new frmSiiConsultaCliente();
            frmSiiConsultaFacturasExpedidasPorCliente.FrmPadre = this;
            frmSiiConsultaFacturasExpedidasPorCliente.Show();
        }

        /// <summary>
        /// Consultar las facturas expedidas informadas por proveedor realizadas al SII
        /// </summary>
        private void ConsultarSIIFacturasExpedidasPorProveedor()
        {
            frmSiiConsultaProveedor frmSiiConsultaFacturasExpedidasPorProveedor = new frmSiiConsultaProveedor();
            frmSiiConsultaFacturasExpedidasPorProveedor.FrmPadre = this;
            frmSiiConsultaFacturasExpedidasPorProveedor.Show();
        }

        /// <summary>
        /// Consultar las facturas expedidas informadas agrupadas por cliente realizadas al SII
        /// </summary>
        private void ConsultarSIIFacturasExpedidasAgrupadasPorCliente()
        {
            frmSiiConsultaAgrupadaCliente frmSiiConsultaFacturasExpedidasAgrupadasPorCliente = new frmSiiConsultaAgrupadaCliente();
            frmSiiConsultaFacturasExpedidasAgrupadasPorCliente.FrmPadre = this;
            frmSiiConsultaFacturasExpedidasAgrupadasPorCliente.Show();
        }

        /// <summary>
        /// Consultar las facturas expedidas informadas agrupadas por proveedor realizadas al SII
        /// </summary>
        private void ConsultarSIIFacturasExpedidasAgrupadasPorProveedor()
        {
            frmSiiConsultaAgrupadaProveedor frmSiiConsultaFacturasExpedidasAgrupadasPorProveedor = new frmSiiConsultaAgrupadaProveedor();
            frmSiiConsultaFacturasExpedidasAgrupadasPorProveedor.FrmPadre = this;
            frmSiiConsultaFacturasExpedidasAgrupadasPorProveedor.Show();
        }
        #endregion
    }
}
