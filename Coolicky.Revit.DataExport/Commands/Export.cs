using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Coolicky.Revit.DataExport.Extensions;
using Coolicky.Revit.DataExport.Utilities;
using Revit.DependencyInjection.Unity.Commands;
using Revit.DependencyInjection.Unity.RibbonCommands.Attributes;
using Unity;

namespace Coolicky.Revit.DataExport.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    [RibbonPushButton(
        Availability = typeof(AvailabilityDocumentOpen),
        PanelName = "DataExport",
        FirstLine = "Export",
        SecondLine = "Data",
        Tooltip = "Revit Database to a json file",
        Image = "DataExport"
    )]
    public class Export : RevitAppCommand<App>
    {
        public override Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var allElements = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .Where(r => r.Id != ElementId.InvalidElementId)
                    .Where(r => r.Category != null)
                    .Select(r => new ElementData(r))
                    .ToList();

                var dialog = new SaveFileDialog();
                dialog.Title = "Save Revit Data";
                dialog.OverwritePrompt = true;
                dialog.Filter = "Json File | *.json";
                dialog.AddExtension = true;
                if (dialog.ShowDialog() != DialogResult.OK) return Result.Cancelled;

                var jsonString = JsonSerializer.Serialize(allElements);
                File.WriteAllText(dialog.FileName, jsonString);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", $"{e.Message}\n{e.StackTrace}");
            }

            return Result.Succeeded;
        }
    }

    public class ElementData
    {
        private readonly Element _element;

        private Type[] _basicTypes =
        {
            typeof(int),
            typeof(string),
            typeof(bool),
            typeof(double),
            typeof(Guid)
        };

        public ElementData(Element element)
        {
            _element = element;
            ModelName = element.GetModelName();
            Id = element.Id.IntegerValue;
            Guid = element.UniqueId;
            Category = element.Category.Name;
            Properties = new Dictionary<string, object>();
            GetClassProperties();
            GetParameters();
        }

        public void GetClassProperties()
        {
            try
            {
                var props = _element
                    .GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(r => _basicTypes.Contains(r.PropertyType));

                foreach (var prop in props)
                {
                    var name = prop.Name;
                    if (Properties.ContainsKey(name)) continue;
                    try
                    {
                        Properties.Add(name, prop.GetValue(_element));
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public void GetParameters()
        {
            try
            {
                var parameters = _element.Parameters;
                var iterator = parameters.ForwardIterator();
                while (iterator.MoveNext())
                {
                    try
                    {
                        if (!(iterator.Current is Parameter parameter)) continue;
                        Properties.AddFromRevitParameter(parameter);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public string ModelName { get; }
        public int Id { get; }
        public string Guid { get; }
        public string Category { get; }
        public Dictionary<string, object> Properties { get; }
    }
}