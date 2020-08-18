using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace ObjectModel
{
    public class RadDataFilterLocalizationProviderES : Telerik.WinControls.UI.DataFilterLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case DataFilterStringId.LogicalOperatorAnd:
                    return "And";
                case DataFilterStringId.LogicalOperatorOr:
                    return "Or";
                case DataFilterStringId.LogicalOperatorDescription:
                    return " de los siguientes son verdaderos";

                case DataFilterStringId.FieldNullText:
                    return "Seleccione campo";
                case DataFilterStringId.ValueNullText:
                    return "Entre un valor";

                case DataFilterStringId.AddNewButtonText:
                    return "Agregar";
                case DataFilterStringId.AddNewButtonExpression:
                    return "Expresión";
                case DataFilterStringId.AddNewButtonGroup:
                    return "Grupo";

                case DataFilterStringId.DialogTitle:
                    return "Filtro de datos";
                case DataFilterStringId.DialogOKButton:
                    return "Aceptar";
                case DataFilterStringId.DialogCancelButton:
                    return "Cancelar";
                case DataFilterStringId.DialogApplyButton:
                    return "Aplicar";

                case DataFilterStringId.ErrorAddNodeDialogTitle:
                    return "RadDataFilter Error";
                case DataFilterStringId.ErrorAddNodeDialogText:
                    return "Cannot add entries to the control - missing property descriptors. \nDataSource is not set and/or DataFilterDescriptorItems are not added to the Descriptors collection of the control.";

                case DataFilterStringId.FilterFunctionBetween:
                    return "Entre";
                case DataFilterStringId.FilterFunctionContains:
                    return "Contiene";
                case DataFilterStringId.FilterFunctionDoesNotContain:
                    return "No contiene";
                case DataFilterStringId.FilterFunctionEndsWith:
                    return "Termina";
                case DataFilterStringId.FilterFunctionEqualTo:
                    return "Igual";
                case DataFilterStringId.FilterFunctionGreaterThan:
                    return "Mayor que";
                case DataFilterStringId.FilterFunctionGreaterThanOrEqualTo:
                    return "Mayor igual";
                case DataFilterStringId.FilterFunctionIsEmpty:
                    return "Vacios";
                case DataFilterStringId.FilterFunctionIsNull:
                    return "Nulos";
                case DataFilterStringId.FilterFunctionLessThan:
                    return "Menor que";
                case DataFilterStringId.FilterFunctionLessThanOrEqualTo:
                    return "Menor iggual";
                case DataFilterStringId.FilterFunctionNoFilter:
                    return "Sin filtrar";
                case DataFilterStringId.FilterFunctionIsContainedIn:
                    return "Está en la lista";
                case DataFilterStringId.FilterFunctionIsNotContainedIn:
                    return "No está en la ñista";
                case DataFilterStringId.FilterFunctionNotBetween:
                    return "No entre";
                case DataFilterStringId.FilterFunctionNotEqualTo:
                    return "No igual";
                case DataFilterStringId.FilterFunctionNotIsEmpty:
                    return "No vacio";
                case DataFilterStringId.FilterFunctionNotIsNull:
                    return "No nulo";
                case DataFilterStringId.FilterFunctionStartsWith:
                    return "Inicia";
                case DataFilterStringId.FilterFunctionCustom:
                    return "Personalizar";
            }
            return base.GetLocalizedString(id);
        }
    }
}
