using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModConsultaInforme.DataClass
{
    class ComprobantesBindClass
    {
        public static List<ComprobantesBindClass> objComprobantesDGVBind = new List<ComprobantesBindClass>();
        public string AAPP { get; set; }
        public string CtaMayor { get; set; }
        public string CtaMayorDesc { get; set; }
        public string TipoAux1 { get; set; }
        public string CtaAux1 { get; set; }
        public string CtaAux1Desc { get; set; }

        public ComprobantesBindClass(string aapp, string ctamayor, string ctamayordesc, string tipoaux1, string ctaaux1, string ctaaux1desc)
        {
            this.AAPP = aapp;
            this.CtaMayor = ctamayor;
            this.CtaMayorDesc = ctamayordesc;
            this.TipoAux1 = tipoaux1;
            this.CtaAux1 = ctaaux1;
            this.CtaAux1Desc = ctaaux1desc;
        }
    }
}