using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCG
{
    public interface IFormEntorno
    {
        //Actualiza los valores de la lista de entornos después de alta o edición
        void ActualizaListaElementos();

        //Desabilita los paneles de módulos al cambiar de módulos
        void ModulosDeshabilitar();
    }
}
