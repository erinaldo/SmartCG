using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModComprobantes
{
    /// <summary>
    /// Interfaz que permite desde la edición o nuevo comprobante (frmCompContAltaEdita.cs), actualizar la lista de comprobantes (frmCompContLista.cs)
    /// </summary>
    public interface IForm
    {
        void ActualizaListaComprobantes();  
    }

    /// <summary>
    /// Interfaz que permite desde la edición o nuevo modelo de comprobante (frmModeloCompContAltaEdita.cs), actualizar la lista de modelos de comprobante (frmModeloCompContLista.cs)
    /// </summary>
    public interface IFormModelo
    {
        void ActualizaListaModelosComprobante();
    }
}
