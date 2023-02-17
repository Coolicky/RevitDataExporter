using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.DB;

namespace Coolicky.Revit.DataExport.Extensions
{
    public static class RevitExtensions
    {
        public static string GetModelName(this Element element)
        {
            return element.Document.IsDetached
                ? element.Document.Title
                : Path.GetFileNameWithoutExtension(element.Document.PathName);
        }

        public static void AddFromRevitParameter(this Dictionary<string, object> dict, Parameter parameter)
        {
            var name = parameter.Definition.Name;
            if (dict.ContainsKey(name)) return;

            switch (parameter.StorageType)
            {
                case StorageType.None:
                    return;
                case StorageType.Integer:
                    dict.Add(name, parameter.AsInteger());
                    break;
                case StorageType.Double:
                    dict.Add(name, parameter.AsDouble());
                    break;
                case StorageType.String:
                    dict.Add(name, parameter.AsString());
                    break;
                case StorageType.ElementId:
                    var value = parameter.AsValueString();
                    dict.Add(name, value == "-1" ? null : value);
                    break;
                default:
                    return;
            }
        }
    }
}