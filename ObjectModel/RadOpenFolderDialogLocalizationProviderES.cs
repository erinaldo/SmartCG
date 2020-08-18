using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.FileDialogs;

namespace ObjectModel
{
    public class RadOpenFolderDialogLocalizationProviderES : FileDialogsLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case FileDialogsStringId.OK:
                    return "Aceptar";
                case FileDialogsStringId.Yes:
                    return "Si";
                case FileDialogsStringId.No:
                    return "No";
                case FileDialogsStringId.Cancel:
                    return "Cancelar";

                case FileDialogsStringId.Back:
                    return "Retroceder";
                case FileDialogsStringId.Forward:
                    return "Avanzar";
                case FileDialogsStringId.Up:
                    return "Arriba";
                case FileDialogsStringId.NewFolder:
                    return "Nueva carpeta";
                case FileDialogsStringId.SearchIn:
                    return "Buscar en";
                case FileDialogsStringId.SearchResults:
                    return "Buscar resultados en";

                case FileDialogsStringId.ExtraLargeIcons:
                    return "Iconos muy grandes";
                case FileDialogsStringId.LargeIcons:
                    return "Iconos grandes";
                case FileDialogsStringId.MediumIcons:
                    return "Iconos medianos";
                case FileDialogsStringId.SmallIcons:
                    return "Iconos pequeños";
                case FileDialogsStringId.List:
                    return "Lista";
                case FileDialogsStringId.Tiles:
                    return "Mosaico";
                case FileDialogsStringId.Details:
                    return "Detalles";

                case FileDialogsStringId.NameHeader:
                    return "Nombre";
                case FileDialogsStringId.SizeHeader:
                    return "Tamaño";
                case FileDialogsStringId.TypeHeader:
                    return "Tipo";
                case FileDialogsStringId.DateHeader:
                    return "Fecha de modificación";

                case FileDialogsStringId.FileSizes_B:
                    return "bytes";
                case FileDialogsStringId.FileSizes_GB:
                    return "GB";
                case FileDialogsStringId.FileSizes_KB:
                    return "KB";
                case FileDialogsStringId.FileSizes_MB:
                    return "MB";
                case FileDialogsStringId.FileSizes_TB:
                    return "TB";

                case FileDialogsStringId.OpenFileDialogHeader:
                    return "Abrir fichero";
                case FileDialogsStringId.OpenFolderDialogHeader:
                    return "Abrir directorio";
                case FileDialogsStringId.SaveFileDialogHeader:
                    return "Salvar como";

                case FileDialogsStringId.FileName:
                    return "Nombre del fichero:";
                case FileDialogsStringId.Folder:
                    return "Directorio:";
                case FileDialogsStringId.SaveAsType:
                    return "Salvar como tipo:";

                case FileDialogsStringId.OpenFolder:
                    return "Seleccionar directorio";
                case FileDialogsStringId.FileFolderType:
                    return "Fichero directorio";

                case FileDialogsStringId.Cut:
                    return "Cortar";
                case FileDialogsStringId.Copy:
                    return "Copiar";
                case FileDialogsStringId.CopyTo:
                    return "Copiar a";
                case FileDialogsStringId.Delete:
                    return "Eliminar";
                case FileDialogsStringId.Edit:
                    return "Editar";
                case FileDialogsStringId.MoveTo:
                    return "Mover a";
                case FileDialogsStringId.Open:
                    return "Abrir";
                case FileDialogsStringId.Paste:
                    return "Pegar";
                case FileDialogsStringId.Properties:
                    return "Propiedades";
                case FileDialogsStringId.Rename:
                    return "Renombrar";
                case FileDialogsStringId.Save:
                    return "Grabar";
                case FileDialogsStringId.View:
                    return "Ver";

                case FileDialogsStringId.CheckThePath:
                    return "Revise el directorio y pruebe de nuevo.";
                case FileDialogsStringId.ConfirmSave:
                    return "Confirmar guardar como";
                case FileDialogsStringId.FileExists:
                    return "ya existe.";
                case FileDialogsStringId.FileNameWrongCharacters:
                    return "Un nombre de archivo no puede contener ninguno de los siguientes caracteres: \\ / : * ? \" < > |";
                case FileDialogsStringId.InvalidExtensionConfirmation:
                    return "¿Está seguro que desea cambiarlo?";
                case FileDialogsStringId.InvalidFileName:
                    return "El nombre del archivo no es válido.";
                case FileDialogsStringId.InvalidOrMissingExtension:
                    return "Si cambia el nombre de la extensión del archivo, este puede convertirse en no utilizable.";
                case FileDialogsStringId.InvalidPath:
                    return "El directorio no existe.";
                case FileDialogsStringId.OpenReadOnly:
                    return "Abrir como solo lectura";
                case FileDialogsStringId.ReplacementQuestion:
                    return "¿Desea reeplazarlo?";

                default:
                    return string.Empty;
            }
        }
    }
}
