using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Coolicky.Revit.DataExport.Utilities;
using Revit.DependencyInjection.Unity.Commands;
using Revit.DependencyInjection.Unity.RibbonCommands.Attributes;
using Unity;

namespace Coolicky.Revit.DataExport.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.ReadOnly)]
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
        public override Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var everything = new FilteredElementCollector(doc)
                .OfClass(typeof(Element))
                .ToList();

            return Result.Succeeded;
        }
    }
}