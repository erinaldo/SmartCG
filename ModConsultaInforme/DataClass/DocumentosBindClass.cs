using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModConsultaInforme.DataClass
{
    class DocumentosBindClass
    {
        public static List<DocumentosBindClass> objDocumentosDGVBind = new List<DocumentosBindClass>();
        public string ImgCol { get; set; }
        public string Clase { get; set; }
        public string Doc { get; set; }
        public string Auxiliar { get; set; }
        public string Fecha{ get; set; }
        public string Vencimiento { get; set; }

        public DocumentosBindClass(string imgcol, string clase, string doc, string auxiliar, string fecha, string vencimiento)
        {
            this.ImgCol = imgcol;
            this.Clase = clase;
            this.Doc = doc;
            this.Auxiliar = auxiliar;
            this.Fecha = fecha;
            this.Vencimiento = vencimiento;
        }   
    }
}
