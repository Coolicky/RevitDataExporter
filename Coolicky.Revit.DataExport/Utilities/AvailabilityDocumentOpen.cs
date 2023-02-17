using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Coolicky.Revit.DataExport.Utilities
{
    public class AvailabilityDocumentOpen : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData?.ActiveUIDocument?.Document != null;
        }
    }
}