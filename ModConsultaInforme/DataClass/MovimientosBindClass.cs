using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModConsultaInforme.DataClass
{
    class MovimientosBindClass
    {
        public static List<MovimientosBindClass> objMovimientosDGVBind = new List<MovimientosBindClass>();
        public string Auxiliar { get; set; }
        public string Fecha { get; set; }
        public string Vencimiento { get; set; }
        public string DH { get; set; }
        public string MonedaLocal { get; set; }
        public string MonedaExt { get; set; }

        public MovimientosBindClass(string auxiliar, string fecha, string vencimiento, string dh, string monedalocal, string monedaext)
        {
            this.Auxiliar = auxiliar;
            this.Fecha = fecha;
            this.Vencimiento = vencimiento;
            this.DH = dh;
            this.MonedaLocal = monedalocal;
            this.MonedaExt = monedaext;      
        }
    }
}
