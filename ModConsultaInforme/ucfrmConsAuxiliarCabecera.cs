using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModConsultaInforme
{
    public partial class ucfrmConsAuxiliarCabecera : UserControl
    {

        private string tipoAuxCodigo;
        public string TipoAuxCodigo
        {
            get
            {
                return (this.tipoAuxCodigo);
            }
            set
            {
                this.tipoAuxCodigo = value;
            }
        }

        private string tipoAuxDesc;
        public string TipoAuxDesc
        {
            get
            {
                return (this.tipoAuxDesc);
            }
            set
            {
                this.tipoAuxDesc = value;
                this.lblTipoAuxValor.Text = value;
            }
        }

        private string ctaAuxCodigo;
        public string CtaAuxCodigo
        {
            get
            {
                return (this.ctaAuxCodigo);
            }
            set
            {
                this.ctaAuxCodigo = value;
            }
        }

        private string ctaAuxDesc;
        public string CtaAuxDesc
        {
            get
            {
                return (this.ctaAuxDesc);
            }
            set
            {
                this.ctaAuxDesc = value;
                this.lblCtaAuxValor.Text = value;
            }
        }


        private string posAux;
        public string PosAux
        {
            get
            {
                return (this.posAux);
            }
            set
            {
                this.posAux = value;
            }
        }

        private string posAuxDesc;
        public string PosAuxDesc
        {
            get
            {
                return (this.posAuxDesc);
            }
            set
            {
                this.posAuxDesc = value;
                this.lblPosAuxValor.Text = value;
            }
        }

        private string companiaCodigo;
        public string CompaniaCodigo
        {
            get
            {
                return (this.companiaCodigo);
            }
            set
            {
                this.companiaCodigo = value;
            }
        }

        private string companiaDesc;
        public string CompaniaDesc
        {
            get
            {
                return (this.companiaDesc);
            }
            set
            {
                this.companiaDesc = value;
                this.lblCia_GrupoValor.Text = value;
            }
        }

        private string grupoCodigo;
        public string GrupoCodigo
        {
            get
            {
                return (this.grupoCodigo);
            }
            set
            {
                this.grupoCodigo = value;
            }
        }

        private string grupoDesc;
        public string GrupoDesc
        {
            get
            {
                return (this.grupoDesc);
            }
            set
            {
                this.grupoDesc = value;
                this.lblCia_GrupoValor.Text = value;
            }
        }

        public Telerik.WinControls.UI.RadLabel Compania_GrupoLabel
        {
            get
            {
                return (this.lblCia_Grupo);
            }
            set
            {
                this.lblCia_Grupo = value;
            }
        }

        private string planCodigo;
        public string PlanCodigo
        {
            get
            {
                return (this.planCodigo);
            }
            set
            {
                this.planCodigo = value;
            }
        }

        private string planDesc;
        public string PlanDesc
        {
            get
            {
                return (this.planDesc);
            }
            set
            {
                this.planDesc = value;
                this.lblPlanValor.Text = value;
            }
        }

        public Telerik.WinControls.UI.RadLabel PlanLabel
        {
            get
            {
                return (this.lblPlan);
            }
            set
            {
                this.lblPlan = value;
            }
        }

        public Telerik.WinControls.UI.RadLabel PlanDescLabel
        {
            get
            {
                return (this.lblPlanValor);
            }
            set
            {
                this.lblPlanValor = value;
            }
        }

        private string aAPPDesde;
        public string AAPPDesde
        {
            get
            {
                return (this.aAPPDesde);
            }
            set
            {
                this.aAPPDesde = value;
                this.lblAnoPerDesdeValor.Text = value;
            }
        }

        private string aAPPHasta;
        public string AAPPHasta
        {
            get
            {
                return (this.aAPPHasta);
            }
            set
            {
                this.aAPPHasta = value;
                this.lblAnoPerHastaValor.Text = value;
            }
        }

        private string ctaMayorCodigo;
        public string CtaMayorCodigo
        {
            get
            {
                return (this.ctaMayorCodigo);
            }
            set
            {
                this.ctaMayorCodigo = value;
            }
        }

        private string ctaMayorDesc;
        public string CtaMayorDesc
        {
            get
            {
                return (this.ctaMayorDesc);
            }
            set
            {
                this.ctaMayorDesc = value;
                this.lblCtaMayorValor.Text = value;
            }
        }

        private string documentos;
        public string Documentos
        {
            get
            {
                return (this.documentos);
            }
            set
            {
                this.documentos = value;
            }
        }

        private string documentosDesc;
        public string DocumentosDesc
        {
            get
            {
                return (this.documentosDesc);
            }
            set
            {
                this.documentosDesc = value;
                this.lblDocValor.Text = value;
            }
        }

        private string mostrarCuentas;
        public string MostrarCuentas
        {
            get
            {
                return (this.mostrarCuentas);
            }
            set
            {
                this.mostrarCuentas = value;
            }
        }

        private string mostrarDocumentosDesc;
        public string MostrarDocumentosDesc
        {
            get
            {
                return (this.mostrarDocumentosDesc);
            }
            set
            {
                this.mostrarDocumentosDesc = value;
                this.lblDocValor.Text = value;
            }
        }

        public Telerik.WinControls.UI.RadLabel DatosMonedaExt
        {
            get
            {
                return (this.lblMostrarME);
            }
            set
            {
                this.lblMostrarME = value;
            }
        }

        private string saldoInicialDesc;
        public string SaldoInicialDesc
        {
            get
            {
                return (this.saldoInicialDesc);
            }
            set
            {
                this.saldoInicialDesc = value;
                this.lblSaldoInicialValor.Text = value;
            }
        }

        private string saldoFinalDesc;
        public string SaldoFinalDesc
        {
            get
            {
                return (this.saldoFinalDesc);
            }
            set
            {
                this.saldoFinalDesc = value;
                this.lblSaldoFinalValor.Text = value;
            }
        }

        private string totalDebeDesc;
        public string TotalDebeDesc
        {
            get
            {
                return (this.totalDebeDesc);
            }
            set
            {
                this.totalDebeDesc = value;
                this.lblTotalDebeValor.Text = value;
            }
        }

        private string totalHaberDesc;
        public string TotalHaberDesc
        {
            get
            {
                return (this.totalHaberDesc);
            }
            set
            {
                this.totalHaberDesc = value;
                this.lblTotalHaberValor.Text = value;
            }
        }

        private bool totalHaberVisible;
        public bool TotalHaberVisible
        {
            get
            {
                return (this.totalHaberVisible);
            }
            set
            {
                this.totalHaberVisible = value;
                this.lblTotalHaber.Visible = value;
                this.lblTotalHaberValor.Visible = value;
            }
        }

        private bool totalDebeVisible;
        public bool TotalDebeVisible
        {
            get
            {
                return (this.totalDebeVisible);
            }
            set
            {
                this.totalDebeVisible = value;
                this.lblTotalDebe.Visible = value;
                this.lblTotalDebeValor.Visible = value;
            }
        }


        private string saldoInicialMEDesc;
        public string SaldoInicialMEDesc
        {
            get
            {
                return (this.saldoInicialMEDesc);
            }
            set
            {
                this.saldoInicialMEDesc = value;
                this.lblSaldoInicialMEValor.Text = value;
            }
        }

        private string saldoFinalMEDesc;
        public string SaldoFinalMEDesc
        {
            get
            {
                return (this.saldoFinalMEDesc);
            }
            set
            {
                this.saldoFinalMEDesc = value;
                this.lblSaldoFinalMEValor.Text = value;
            }
        }

        private string totalDebeMEDesc;
        public string TotalDebeMEDesc
        {
            get
            {
                return (this.totalDebeMEDesc);
            }
            set
            {
                this.totalDebeMEDesc = value;
                this.lblTotalDebeMEValor.Text = value;
            }
        }

        private string totalHaberMEDesc;
        public string TotalHaberMEDesc
        {
            get
            {
                return (this.totalHaberMEDesc);
            }
            set
            {
                this.totalHaberMEDesc = value;
                this.lblTotalHaberMEValor.Text = value;
            }
        }

        private bool totalHaberMEVisible;
        public bool TotalHaberMEVisible
        {
            get
            {
                return (this.totalHaberMEVisible);
            }
            set
            {
                this.totalHaberMEVisible = value;
                this.lblTotalHaberME.Visible = value;
                this.lblTotalHaberMEValor.Visible = value;
            }
        }

        private bool totalDebeMEVisible;
        public bool TotalDebeMEVisible
        {
            get
            {
                return (this.totalDebeMEVisible);
            }
            set
            {
                this.totalDebeMEVisible = value;
                this.lblTotalDebeME.Visible = value;
                this.lblTotalDebeMEValor.Visible = value;
            }
        }

        private ObjectModel.LanguageProvider lp;
        public ObjectModel.LanguageProvider Lp
        {
            get
            {
                return (this.lp);
            }
            set
            {
                this.lp = value;
            }
        }

        private bool mostrarGrupoSaldosTotales = true;
        public bool MostrarGrupoSaldosTotales
        {
            get
            {
                return (this.mostrarGrupoSaldosTotales);
            }
            set
            {
                this.mostrarGrupoSaldosTotales = value;
                this.gbrSaldos.Visible = value;
            }
        }

        private bool mostrarGrupoSaldosTotalesME = false;
        public bool MostrarGrupoSaldosTotalesME
        {
            get
            {
                return (this.mostrarGrupoSaldosTotalesME);
            }
            set
            {
                this.mostrarGrupoSaldosTotalesME = value;
                this.gbrSaldosME.Visible = value;
            }
        }

        private bool mostrarDocumentos = true;
        public bool MostrarDocumentos
        {
            get
            {
                return (this.mostrarDocumentos);
            }
            set
            {
                this.mostrarDocumentos = value;
            }
        }

        public ucfrmConsAuxiliarCabecera()
        {
            InitializeComponent();

            this.grbSel.ElementTree.EnableApplicationThemeName = false;
            this.grbSel.ThemeName = "ControlDefault";

            this.gbrSaldos.ElementTree.EnableApplicationThemeName = false;
            this.gbrSaldos.ThemeName = "ControlDefault";

            this.gbrSaldosME.ElementTree.EnableApplicationThemeName = false;
            this.gbrSaldosME.ThemeName = "ControlDefault";
        }

        #region Métodos públicos
        //Actualiza los valores del formulario que se permitan
        public void ActualizarValores()
        {
            switch (this.PosAux)
            {
                case "-1":
                    this.posAuxDesc = "Indiferente";
                    break;
                case "1":
                    this.posAuxDesc = "Auxiliar 1";
                    break;
                case "2":
                    this.posAuxDesc = "Auxiliar 2";
                    break;
                case "3":
                    this.posAuxDesc = "Auxiliar 3";
                    break;
            }
            this.lblPosAuxValor.Text = this.posAuxDesc;

            if (this.ctaAuxCodigo == "")
            {
                this.ctaAuxDesc = "Todas";
                this.lblCtaAuxValor.Text = this.ctaAuxDesc;
            }

            if (this.ctaMayorCodigo == "")
            {
                this.ctaMayorDesc = "Todas";
                this.lblCtaMayorValor.Text = this.ctaMayorDesc;
            }

            if (this.mostrarDocumentos)
            {
                if (this.documentosDesc == "")
                {
                    if (this.mostrarCuentas == "1") this.documentosDesc = "Cuentas sin Documentos";
                    else
                    {
                        if (this.mostrarCuentas == "2") //Con documentos
                        {
                            switch (this.documentos)
                            {
                                case "1":
                                    this.documentosDesc = "Cuentas con Documentos Cancelados";
                                    break;
                                case "2":
                                    this.documentosDesc = "Cuentas con Documentos No Cancelados";
                                    break;
                                case "3":
                                    this.documentosDesc = "Cuentas con Todos lo Documentos";
                                    break;
                            }
                        }
                        else
                        {
                            if (this.documentos != null && this.documentos != "-1")
                            {
                                switch (this.documentos)
                                {
                                    case "1":
                                        this.documentosDesc = "Todas las Cuentas con Documentos Cancelados";
                                        break;
                                    case "2":
                                        this.documentosDesc = "Todas las Cuentas con Documentos No Cancelados";
                                        break;
                                    case "3":
                                        this.documentosDesc = "Todas las Cuentas con Todos lo Documentos";
                                        break;
                                }
                            }
                        }
                    }
                }

                this.lblDocValor.Text = this.documentosDesc;

                this.lblDoc.Visible = true;
                this.lblDocValor.Visible = true;
            }
            else
            {
                this.lblDoc.Visible = false;
                this.lblDocValor.Visible = false;
            }

            //this.gbrSaldos.Visible = this.mostrarGrupoSaldosTotales;
            //this.gbrSaldosME.Visible = (this.mostrarGrupoSaldosTotales && this.DatosMonedaExt.Visible);
            //this.gbrSaldosME.Visible = this.mostrarGrupoSaldosTotalesME;
        }

        /// <summary>
        /// Cambia la etiqueta Total Debe
        /// </summary>
        /// <param name="literal"></param>
        public void CambiarLiteralTotalDebe(string literal)
        {
            if (literal != "") this.lblTotalDebe.Text = literal;
        }

        /// <summary>
        /// Cambia la etiqueta Total Debe ME
        /// </summary>
        /// <param name="literal"></param>
        public void CambiarLiteralTotalDebeME(string literal)
        {
            if (literal != "") this.lblTotalDebeME.Text = literal;
        }
        #endregion

        #region Métodos Privados
        private void TraducirLiterales()
        {
            //Recuperar literales del formulario
            this.grbSel.Text = "Criterio de selección";   //Falta traducir
            this.lblTipoAux.Text = this.lp.GetText("lblTipoAux", "Tipo de auxiliar");   //Falta traducir
            this.lblCtaAux.Text = this.lp.GetText("lblCtaAux", "Cuenta de auxiliar");   //Falta traducir
            this.lblPosAux.Text = this.lp.GetText("lblPosAux", "Posición del auxiliar");   //Falta traducir
            this.lblCia_Grupo.Text = this.lp.GetText("lblCompGrupo", "Compañía/Grupo");   //Falta traducir
            this.lblPlan.Text = this.lp.GetText("lblPlan", "Plan");   //Falta traducir
            this.lblAnoPerDesde.Text = this.lp.GetText("lblAAPerDesde", "Año-periodo desde");   //Falta traducir
            this.lblAnoPerHasta.Text = this.lp.GetText("lblAAPerHasta", "hasta");   //Falta traducir
            this.lblCtaMayor.Text = this.lp.GetText("lblCtaMayor", "Cuenta de mayor");   //Falta traducir
            this.lblDoc.Text = this.lp.GetText("lblDoc", "Documentos");   //Falta traducir
            this.lblMostrarME.Text = this.lp.GetText("lblMostrarDatosMonExt", "Mostrar datos de moneda extranjera");   //Falta traducir
            this.lblSaldoInicial.Text = this.lp.GetText("lblSaldoInicial", "Saldo inicial");   //Falta traducir
            this.lblSaldoFinal.Text = this.lp.GetText("lblSaldoFinal", "Saldo final");   //Falta traducir
            this.lblTotalDebe.Text = this.lp.GetText("lblTotalDebe", "Total Debe");   //Falta traducir
            this.lblTotalHaber.Text = this.lp.GetText("lblTotalHaber", "Total Haber");   //Falta traducir
        }
        #endregion

    }
}
