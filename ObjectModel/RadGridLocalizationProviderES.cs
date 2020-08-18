using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI.Localization;

namespace ObjectModel
{
    public class RadGridLocalizationProviderES:RadGridLocalizationProvider
    {
        public override String GetLocalizedString(String id)
        {
            switch (id)
            {
                case RadGridStringId.AddNewRowString:
                    return "Clic aquí para agregar una fila";   //"Click aqui para añadir una nueva fila";
                case RadGridStringId.BestFitMenuItem:
                    return "Ajustar al contenido";      //"Mejor ajuste"
                case RadGridStringId.ClearSortingMenuItem:
                    return "Re-ordenar";    //"Limpiar clasificación"
                case RadGridStringId.ClearValueMenuItem:
                    return "Limpiar celda"; //"Limpoar valor"
                case RadGridStringId.ColumnChooserFormCaption:
                    return "Selector de columnas";
                case RadGridStringId.ColumnChooserFormMessage:
                    return "Para ocultar una columna," + Convert.ToChar(13).ToString() + "" + Convert.ToChar(10) + " arrastre hacia esta ventana.";
                case RadGridStringId.ColumnChooserMenuItem:
                    return "Selector de columnas";
                case RadGridStringId.ConditionalFormattingBtnAdd:
                    return "Agregar";
                case RadGridStringId.ConditionalFormattingBtnApply:
                    return "Aplicar";
                case RadGridStringId.ConditionalFormattingBtnCancel:
                    return "Cancelar";
                case RadGridStringId.ConditionalFormattingBtnOK:
                    return "Aceptar";
                case RadGridStringId.ConditionalFormattingBtnRemove:
                    return "Quitar";
                case RadGridStringId.ConditionalFormattingCaption:
                    return "Configurar formato condicional";
                case RadGridStringId.ConditionalFormattingChkApplyToRow:
                    return "Aplicar a toda la fila";
                case RadGridStringId.ConditionalFormattingChooseOne:
                    return "Seleccione una opción";
                case RadGridStringId.ConditionalFormattingContains:
                    return "Contiene";
                case RadGridStringId.ConditionalFormattingDoesNotContain:
                    return "No contiene";
                case RadGridStringId.ConditionalFormattingEndsWith:
                    return "Termina";
                case RadGridStringId.ConditionalFormattingEqualsTo:
                    return "Igual";
                case RadGridStringId.ConditionalFormattingGrpConditions:
                    return "Condiciones";
                case RadGridStringId.ConditionalFormattingGrpProperties:
                    return "Propiedades";
                case RadGridStringId.ConditionalFormattingIsBetween:
                    return "Entre";
                case RadGridStringId.ConditionalFormattingIsGreaterThan:
                    return "Mayor";
                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual:
                    return "Mayor igual";
                case RadGridStringId.ConditionalFormattingIsLessThan:
                    return "Menor";
                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual:
                    return "Menor igual";
                case RadGridStringId.ConditionalFormattingIsNotBetween:
                    return "No entre";
                case RadGridStringId.ConditionalFormattingIsNotEqualTo:
                    return "No igual";
                case RadGridStringId.ConditionalFormattingLblColumn:
                    return "Columna:";
                case RadGridStringId.ConditionalFormattingLblName:
                    return "Nombre:";
                case RadGridStringId.ConditionalFormattingLblType:
                    return "Tipo:";
                case RadGridStringId.ConditionalFormattingLblValue1:
                    return "Valor 1:";
                case RadGridStringId.ConditionalFormattingLblValue2:
                    return "Valor 2:";
                case RadGridStringId.ConditionalFormattingMenuItem:
                    return "Formato condicional";
                case RadGridStringId.ConditionalFormattingRuleAppliesOn:
                    return "Aplicar regla para";
                case RadGridStringId.ConditionalFormattingStartsWith:
                    return "Inicia";
                case RadGridStringId.CopyMenuItem:
                    return "Copiar";
                case RadGridStringId.CustomFilterDialogBtnCancel:
                    return "Cancelar";
                case RadGridStringId.CustomFilterDialogBtnOk:
                    return "Aceptar";
                case RadGridStringId.CustomFilterDialogCaption:
                    return "Personalizar filtro";       //"RadGridView Pantalla de Filtro [{0}]"
                case RadGridStringId.CustomFilterDialogCheckBoxNot:
                    return "No";
                case RadGridStringId.CustomFilterDialogLabel:
                    return "Criterios de filtrado";
                case RadGridStringId.CustomFilterDialogRbAnd:
                    return "Y";
                case RadGridStringId.CustomFilterDialogRbOr:
                    return "O";
                case RadGridStringId.CustomFilterDialogTrue:
                    return "Verdadero";
                case RadGridStringId.CustomFilterDialogFalse:
                    return "Falso";
                case RadGridStringId.CustomFilterMenuItem:
                    return "Personalizar filtro";       // "Personalizado";
                case RadGridStringId.DeleteRowMenuItem:
                    return "Eliminar fila";
                case RadGridStringId.EditMenuItem:
                    return "Editar celda";  //"Editar"
                case RadGridStringId.FilterFunctionBetween:
                    return "Entre";
                case RadGridStringId.FilterFunctionContains:
                    return "Contiene";
                case RadGridStringId.FilterFunctionCustom:
                    return "Personalizar";
                case RadGridStringId.FilterFunctionDoesNotContain:
                    return "No contiene";
                case RadGridStringId.FilterFunctionEndsWith:
                    return "Termina";       //"Finaliza con"
                case RadGridStringId.FilterFunctionEqualTo:
                    return "Igual";         //"Igual a"
                case RadGridStringId.FilterFunctionGreaterThan:
                    return "Mayor";         //"Mayor que"
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo:
                    return "Mayor igual";   //"Mayor o igual a"
                case RadGridStringId.FilterFunctionIsEmpty:
                    return "Vacios";        //"Esta vacío"
                case RadGridStringId.FilterFunctionIsNull:
                    return "Nulos";         //"Es nulo"
                case RadGridStringId.FilterFunctionLessThan:
                    return "Menor";         //"Menor que"
                case RadGridStringId.FilterFunctionLessThanOrEqualTo:
                    return "Menor igual";   //"Menor o igual que"
                case RadGridStringId.FilterFunctionNoFilter:
                    return "Sin filtrar";   //"Sin filtro"
                case RadGridStringId.FilterFunctionNotBetween:
                    return "No entre";
                case RadGridStringId.FilterFunctionNotEqualTo:
                    return "No igual";      //"Distinto de"
                case RadGridStringId.FilterFunctionNotIsEmpty:
                    return "No vacio";      //"No está vacío"
                case RadGridStringId.FilterFunctionNotIsNull:
                    return "No nulo";       //"No es nulo"
                case RadGridStringId.FilterFunctionStartsWith:
                    return "Inicia";        //"Comienza por"
                case RadGridStringId.FilterOperatorBetween:
                    return "Entre";
                case RadGridStringId.FilterOperatorContains:
                    return "Contiene";
                case RadGridStringId.FilterOperatorCustom:
                    return "Personalizado";
                case RadGridStringId.FilterOperatorDoesNotContain:
                    return "No contiene";
                case RadGridStringId.FilterOperatorEndsWith:
                    return "Termina";       //"Finaliza con"
                case RadGridStringId.FilterOperatorEqualTo:
                    return "Igual";
                case RadGridStringId.FilterOperatorGreaterThan:
                    return "Mayor";         //"Mayor que"
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo:
                    return "Mayor igual";   //"Mayor o igual que"
                case RadGridStringId.FilterOperatorIsContainedIn:
                    return "Contiene en";   //"Contenido en"
                case RadGridStringId.FilterOperatorIsEmpty:
                    return "Vacio";         //"Esta vacío"
                case RadGridStringId.FilterOperatorIsLike:
                    return "Es como";       //"Como"
                case RadGridStringId.FilterOperatorIsNull:
                    return "Nulo";          //"Es nulo"
                case RadGridStringId.FilterOperatorLessThan:
                    return "Menor";         //"Menor que"
                case RadGridStringId.FilterOperatorLessThanOrEqualTo:
                    return "Menor igual";   //"Menor o igual que"
                case RadGridStringId.FilterOperatorNoFilter:
                    return "Sin filtrar";   //"Sin filtro"
                case RadGridStringId.FilterOperatorNotBetween:
                    return "No entre";
                case RadGridStringId.FilterOperatorNotEqualTo:
                    return "No igual";      //"Distinto de"
                case RadGridStringId.FilterOperatorNotIsContainedIn:
                    return "No contiene en";    //"No contenido en"
                case RadGridStringId.FilterOperatorNotIsEmpty:
                    return "No vacio";
                case RadGridStringId.FilterOperatorNotIsLike:
                    return "No como";
                case RadGridStringId.FilterOperatorNotIsNull:
                    return "No nulo";
                case RadGridStringId.FilterOperatorStartsWith:
                    return "Inicia";        //"Comienza por"
                case RadGridStringId.GroupByThisColumnMenuItem:
                    return "Agrupar columna";   //"Agrupar por esta columna"
                case RadGridStringId.GroupingPanelDefaultMessage:
                    return "Arrastre una columna aquí para agrupar - Pulse ctrl+F para activar la búsqueda";
                case RadGridStringId.GroupingPanelHeader:
                    return "Columnas agrupadas";
                case RadGridStringId.HideMenuItem:
                    return "Ocular columna";
                case RadGridStringId.NoDataText:
                    return "No hay datos para mostrar";
                case RadGridStringId.PasteMenuItem:
                    return "Pegar";
                case RadGridStringId.PinAtLeftMenuItem:
                    return "Aplilar a la izquierda";    //"Anclar a la izquierda"
                case RadGridStringId.PinAtRightMenuItem:
                    return "Apilar a la derecha";       //"Anclar a la derecha"
                case RadGridStringId.PinMenuItem:
                    return "Apilar";    //"Estado de anclado"
                case RadGridStringId.SortAscendingMenuItem:
                    return "Ordenar Ascendente";
                case RadGridStringId.SortDescendingMenuItem:
                    return "Ordenar Descendente";
                case RadGridStringId.UngroupThisColumn:
                    return "Desagrupar columna";    //"Desagrupar esta columna"
                case RadGridStringId.UnpinMenuItem:
                    return "Desapilar";             //"Desanclar columna"
                case RadGridStringId.FilterMenuBlanks:
                    return "Vacío";
                case RadGridStringId.FilterMenuAvailableFilters:
                    return "Filtros disponibles";
                case RadGridStringId.FilterMenuSearchBoxText:
                    return "Buscar...";
                case RadGridStringId.FilterMenuClearFilters:
                    return "Limpiar filtro";
                case RadGridStringId.FilterMenuButtonOK:
                    return "OK";
                case RadGridStringId.FilterMenuButtonCancel:
                    return "Cancelar";
                case RadGridStringId.FilterMenuSelectionAll:
                    return "Todo";
                case RadGridStringId.FilterMenuSelectionAllSearched:
                    return "Todos los resultados";  
                case RadGridStringId.FilterMenuSelectionNull:
                    return "Nulo";
                case RadGridStringId.FilterMenuSelectionNotNull:
                    return "No nulo";
                case RadGridStringId.FilterFunctionSelectedDates:
                    return "Filtro para fecha especificas:";
                case RadGridStringId.FilterFunctionToday:
                    return "Hoy";
                case RadGridStringId.FilterFunctionYesterday:
                    return "Ayer";
                case RadGridStringId.FilterFunctionDuringLast7days:
                    return "En los ultimos 7 dias";
                case RadGridStringId.FilterLogicalOperatorAnd:
                    return "AND";
                case RadGridStringId.FilterLogicalOperatorOr:
                    return "OR";
                case RadGridStringId.FilterCompositeNotOperator:
                    return "NOT";                   
                case RadGridStringId.HideGroupMenuItem:
                    return "Ocultar grupo";
                case RadGridStringId.UnpinRowMenuItem:
                    return "Desanclar fila";
                case RadGridStringId.PinAtBottomMenuItem:
                    return "Anclar abajo";
                case RadGridStringId.PinAtTopMenuItem:
                    return "Anclar arriba";
                case RadGridStringId.CutMenuItem:
                    return "Cortar";
                case RadGridStringId.ConditionalFormattingSortAlphabetically:
                    return "Clasificar columnas alfabeticamente";
            /*case RadGridStringId.ConditionalFormattingCaption
                return "Gestor de reglas de formateo condicional"
            case RadGridStringId.ConditionalFormattingLblColumn
                return "Formatear solo celdas con"
            case RadGridStringId.ConditionalFormattingLblName
                return "Nombre Regla"
            case RadGridStringId.ConditionalFormattingLblType
                return "Valor de celda"
            case RadGridStringId.ConditionalFormattingLblValue1
                return "Valor 1"
            case RadGridStringId.ConditionalFormattingLblValue2
                return "Valor 2"
            case RadGridStringId.ConditionalFormattingGrpConditions
                return "Reglas"
            case RadGridStringId.ConditionalFormattingGrpProperties
                return "Propiedades regla"
            case RadGridStringId.ConditionalFormattingChkApplyToRow
                return "Aplicar este formateo a la fila completa"
            case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows
                return "Aplica este formateo a la fila si esta seleccionada"
            case RadGridStringId.ConditionalFormattingBtnAdd
                return "Añadir nueva regla"
            case RadGridStringId.ConditionalFormattingBtnRemove
                return "Eliminar"
            case RadGridStringId.ConditionalFormattingBtnOK
                return "OK"
            case RadGridStringId.ConditionalFormattingBtnCancel
                return "Cancelar"
            case RadGridStringId.ConditionalFormattingBtnApply
                return "Aplicar"
            case RadGridStringId.ConditionalFormattingRuleAppliesOn
                return "Regla aplicada a"
            case RadGridStringId.ConditionalFormattingCondition
                return "Condición"
            case RadGridStringId.ConditionalFormattingExpression
                return "Expresión"
            case RadGridStringId.ConditionalFormattingChooseOne
                return "[Seleccionar uno]"
            case RadGridStringId.ConditionalFormattingEqualsTo
                return "igual a [Valor1]"
            case RadGridStringId.ConditionalFormattingIsNotEqualTo
                return "distinto de [Valor1]"
            case RadGridStringId.ConditionalFormattingStartsWith
                return "comienza por [Valor1]"
            case RadGridStringId.ConditionalFormattingEndsWith
                return "finaliza con [Valor1]"
            case RadGridStringId.ConditionalFormattingContains
                return "contiene [Valor1]"
            case RadGridStringId.ConditionalFormattingDoesNotContain
                return "no contiene [Valor1]"
            case RadGridStringId.ConditionalFormattingIsGreaterThan
                return "es mayor que [Valor1]"
            case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual
                return "es mayor o igual que [Valor1]"
            case RadGridStringId.ConditionalFormattingIsLessThan
                return "es menor que [Valor1]"
            case RadGridStringId.ConditionalFormattingIsLessThanOrEqual
                return "es menor o igual que [Valor1]"
            case RadGridStringId.ConditionalFormattingIsBetween
                return "esta entre [Valor1] y [Valor2]"
            case RadGridStringId.ConditionalFormattingIsNotBetween
                return "no esta entre [Valor1] y [Valor2]"
            case RadGridStringId.ConditionalFormattingLblFormat
                return "Formato"
            case RadGridStringId.ConditionalFormattingBtnExpression
                return "Editor de expresiones"
            case RadGridStringId.ConditionalFormattingTextBoxExpression
                return "Expresión"
            case RadGridStringId.ConditionalFormattingPropertyGridcaseSensitive
                return "Sensible mayúsculas"
            case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor
                return "Color fondo celda"
            case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor
                return "Color frente celda"
            case RadGridStringId.ConditionalFormattingPropertyGridEnabled
                return "Activo"
            case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor
                return "Color fondo fila"
            case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor
                return "Color frente fila"
            case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment
                return "Alineamiento texto fila"
            case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment
                return "Alineamiento texto"
            case RadGridStringId.ConditionalFormattingPropertyGridcaseSensitiveDescription
                return "Determina si se tendrán en cuenta las comparaciones sensibles a mayusculas cuando se evaluen cadenas de caracteres"
            case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription
                return "Introducir color de fondo para usar con esta celda."
            case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription
                return "Introducir color frontal para usar con esta celda."
            case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription
                return "Determina si la condición esta activa (puede ser evaluada y aplicada)."
            case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription
                return "Introducir el color de fondo para usar en toda la fila."
            case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription
                return "Introducir el color frontal para usar en toda la fila."
            case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription
                return "Introduzca la alineación para usar con los valores de las celdas, cuando ApplyToRow sea verdadero."
            case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription
                return "Introduzca la alineación para usar con los valores de las celdas."
            case RadGridStringId.ColumnChooserFormCaption
                return "Selector de columnas"
            case RadGridStringId.ColumnChooserFormMessage
                return "Arrastrar un titulo de columna desde el" & vbLf & "grid aqui para eliminarla " & vbLf & "de la vista actual."
            case RadGridStringId.GroupingPanelDefaultMessage
                return "Arrastra una columna aqui para agrupar por esta columna."
            case RadGridStringId.GroupingPanelHeader
                return "Agrupar por:"
            case RadGridStringId.PagingPanelPagesLabel
                return "Página"
            case RadGridStringId.PagingPanelOfPagesLabel
                return "de"
            case RadGridStringId.NoDataText
                return "No hay información para mostrar"
            case RadGridStringId.CompositeFilterFormErrorCaption
                return "Error de filtro"
            case RadGridStringId.CompositeFilterFormInvalidFilter
                return "El descriptor de filtro compuesto no es válido."
            case RadGridStringId.ExpressionMenuItem
                return "Expresión"
            case RadGridStringId.ExpressionFormTitle
                return "Contructor de expresiones"
            case RadGridStringId.ExpressionFormFunctions
                return "Funciones"
            case RadGridStringId.ExpressionFormFunctionsText
                return "Texto"
            case RadGridStringId.ExpressionFormFunctionsAggregate
                return "Agregado"
            case RadGridStringId.ExpressionFormFunctionsDateTime
                return "Fecha-Hora"
            case RadGridStringId.ExpressionFormFunctionsLogical
                return "Logico"
            case RadGridStringId.ExpressionFormFunctionsMath
                return "Math"
            case RadGridStringId.ExpressionFormFunctionsOther
                return "Otros"
            case RadGridStringId.ExpressionFormOperators
                return "Operadores"
            case RadGridStringId.ExpressionFormConstants
                return "Constantes"
            case RadGridStringId.ExpressionFormFields
                return "Campos"
            case RadGridStringId.ExpressionFormDescription
                return "Descripcion"
            case RadGridStringId.ExpressionFormResultPreview
                return "Vista previa de resultados"
            case RadGridStringId.ExpressionFormTooltipPlus
                return "Sumar"
            case RadGridStringId.ExpressionFormTooltipMinus
                return "Restar"
            case RadGridStringId.ExpressionFormTooltipMultiply
                return "Multiplicar"
            case RadGridStringId.ExpressionFormTooltipDivide
                return "Dividir"
            case RadGridStringId.ExpressionFormTooltipModulo
                return "Módulo"
            case RadGridStringId.ExpressionFormTooltipEqual
                return "Igual"
            case RadGridStringId.ExpressionFormTooltipNotEqual
                return "Distinto"
            case RadGridStringId.ExpressionFormTooltipLess
                return "Menor"
            case RadGridStringId.ExpressionFormTooltipLessOrEqual
                return "Menor o igual"
            case RadGridStringId.ExpressionFormTooltipGreaterOrEqual
                return "Mayor o igual"
            case RadGridStringId.ExpressionFormTooltipGreater
                return "Mayor"
            case RadGridStringId.ExpressionFormTooltipAnd
                return """AND"" lógico"
            case RadGridStringId.ExpressionFormTooltipOr
                return """OR"" lógico"
            case RadGridStringId.ExpressionFormTooltipNot
                return """NOT"" lógico"
            case RadGridStringId.ExpressionFormAndButton
                return String.Empty
                'if empty, default button image is used
            case RadGridStringId.ExpressionFormOrButton
                return String.Empty
                'if empty, default button image is used
            case RadGridStringId.ExpressionFormNotButton
                return String.Empty
                'if empty, default button image is used
            case RadGridStringId.ExpressionFormOKButton
                return "OK"
            case RadGridStringId.ExpressionFormCancelButton
                return "Cancelar"
            case RadGridStringId.SearchRowChooseColumns
                return "SearchRowChooseColumns"
            case RadGridStringId.SearchRowSearchFromCurrentPosition
                return "SearchRowSearchFromCurrentPosition"
            case RadGridStringId.SearchRowMenuItemMasterTemplate
                return "SearchRowMenuItemMasterTemplate"
            case RadGridStringId.SearchRowMenuItemChildTemplates
                return "SearchRowMenuItemChildTemplates"
            case RadGridStringId.SearchRowMenuItemAllColumns
                return "SearchRowMenuItemAllColumns"
            case RadGridStringId.SearchRowTextBoxNullText
                return "SearchRowTextBoxNullText"
            case RadGridStringId.SearchRowResultsOfLabel
                return "SearchRowResultsOfLabel"
            case RadGridStringId.SearchRowMatchcase
                return "Match case"
*/
                case RadGridStringId.SearchRowMenuItemAllColumns:
                    return "Todas";
                case RadGridStringId.SearchRowMatchCase:
                    return "Coincidan";
                case RadGridStringId.SearchRowChooseColumns:
                    return "Buscar en columnas";
                case RadGridStringId.SearchRowSearchFromCurrentPosition:
                    return "Buscar desde la posición actual";
                case RadGridStringId.SearchRowTextBoxNullText:
                    return "Escriba aquí para buscar";
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}
