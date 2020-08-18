using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using ObjectModel;

namespace ModComprobantes
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
            Log.Info("INICIO Cargar Módulo de Comprobantes");

            //Necesario para el KeyDown (cerrar el formulario al pulsar la tecla 'ESC')
            this.KeyPreview = true;

            //Traducir los literales del formulario
            this.TraducirLiterales();
        }

        private void subMenuItemGestionCompCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompContLista frmLista = new frmCompContLista();
            frmLista.FrmPadre = this;
            frmLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemModelosCompCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmModeloCompContLista frmLista = new frmModeloCompContLista();
            frmLista.FrmPadre = this;
            frmLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemTransferirCompExtCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompExtContTransferirFinanComprobantes frmTransferirComprobante = new frmCompExtContTransferirFinanComprobantes();
            frmTransferirComprobante.FrmPadre = this;
            frmTransferirComprobante.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemImportarCompCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompContImportarDeFinanzas frmImportarDeFinanzas = new frmCompContImportarDeFinanzas();
            frmImportarDeFinanzas.FrmPadre = this;
            frmImportarDeFinanzas.Show();
            
            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemTransferirCompEditadosCompCont_Click(object sender, EventArgs e)
        {
            /*
            Cursor.Current = Cursors.WaitCursor;

            frmCompContTransferirFinanComprobantes frmTransferirComprobante = new frmCompContTransferirFinanComprobantes();
            frmTransferirComprobante.FrmPadre = this;
            frmTransferirComprobante.Show();

            Cursor.Current = Cursors.Default;
            */
        }

        private void subMenuItemTransferirArchivoCompCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompContTransferirArchivoLote frmTransferirArchivo = new frmCompContTransferirArchivoLote();
            frmTransferirArchivo.FrmPadre = this;
            frmTransferirArchivo.Show();

            Cursor.Current = Cursors.Default;
        }

        private void menuItemConfig_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmConfig frmConfiguracion = new frmConfig();
            frmConfiguracion.FrmPadre = this;
            frmConfiguracion.Show();

            Cursor.Current = Cursors.Default;
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmAbout frmInfo = new frmAbout();
            frmInfo.FrmPadre = this;
            frmInfo.Show();

            Cursor.Current = Cursors.Default;
        }

        private void menuItemSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void subMenuItemImportarCompExtCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompExtContImportarDeFinanzas frmImportarDeFinanzas = new frmCompExtContImportarDeFinanzas();
            frmImportarDeFinanzas.FrmPadre = this;
            frmImportarDeFinanzas.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemGestionCompExtCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompExtContLista frmLista = new frmCompExtContLista();
            frmLista.FrmPadre = this;
            frmLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemEditLotesCompCont_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompContGestionLotes frmLista = new frmCompContGestionLotes();
            frmLista.FrmPadre = this;
            frmLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void subMenuItemListaComp_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            frmCompContListaGLB01 frmLista = new frmCompContListaGLB01();
            frmLista.FrmPadre = this;
            frmLista.Show();

            Cursor.Current = Cursors.Default;
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27) menuItemSalir_Click(sender, null);
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Info("FIN Cargar Módulo de Comprobantes");
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.Text = this.LP.GetText("lblfrmPrincipalTitulo", "Módulo Entrada de Comprobantes");

            this.menuItemConfig.Text = this.LP.GetText("menuItemConfig", "&Configuración");
            this.menuItemAbout.Text = this.LP.GetText("menuItemAbout", "&Acerca de");
            this.menuItemSalir.Text = this.LP.GetText("menuItemSalir", "&Salir");
            
            //Menú Comprobantes Contables
            this.menuItemCompContables.Text = this.LP.GetText("menuItemCompContables", "Comprobantes Con&tables");
            this.subMenuItemGestionCompCont.Text = this.LP.GetText("subMenuItemGestionCompCont", "&Gestión");
            this.subMenuItemModelosCompCont.Text = this.LP.GetText("subMenuItemModelosCompCont", "&Modelos");
            this.subMenuItemTransferirCompCont.Text = this.LP.GetText("subMenuItemTransferirCompCont", "&Transferir a finanzas");
            this.subMenuItemTransferirCompEditadosCompCont.Text = this.LP.GetText("subMenuItemTransferirCompEditadosCompCont", "&Comprobantes editados");
            this.subMenuItemTransferirArchivoCompCont.Text = this.LP.GetText("subMenuItemTransferirArchivoCompCont", "Desde &archivo externo");
            this.subMenuItemImportarCompCont.Text = this.LP.GetText("subMenuItemImportarCompCont", "&Importar de finanzas");

            //Menú Comprobantes Contables
            this.menuItemCompExtraContables.Text = this.LP.GetText("menuItemCompExtraContables", "Comprobantes &Extracontables");
            this.subMenuItemGestionCompExtCont.Text = this.LP.GetText("subMenuItemGestionCompCont", "&Gestión");
            this.subMenuItemTransferirCompExtCont.Text = this.LP.GetText("subMenuItemTransferirCompCont", "&Transferir a finanzas");
            this.subMenuItemImportarCompExtCont.Text = this.LP.GetText("subMenuItemImportarCompCont", "&Importar de finanzas");
        }
        #endregion
    }
}
