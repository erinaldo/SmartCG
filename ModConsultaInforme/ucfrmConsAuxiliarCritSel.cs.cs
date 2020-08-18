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
    public partial class ucfrmConsAuxiliarCritSel : UserControl
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

        public Label Compania_GrupoLabel
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

        public Label PlanLabel
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

        public Label PlanDescLabel
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

        public Label DatosMonedaExt
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


        public ucfrmConsAuxiliarCritSel()
        {
            InitializeComponent();
        }

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

            switch (this.documentos)
            {
                case "-1":
                    this.documentosDesc = "Sin Documentos";
                    break;
                case "1":
                    this.documentosDesc = "Cancelados";
                    break;
                case "2":
                    this.documentosDesc = "No Cancelados";
                    break;
                case "3":
                    this.documentosDesc = "Todos";
                    break;
            }
            this.lblDocValor.Text = this.documentosDesc;

        }

    }
}
