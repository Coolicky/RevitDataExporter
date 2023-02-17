using System;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.Applications;
using Revit.DependencyInjection.Unity.Base;
using Unity;
using Unity.Lifetime;

namespace Coolicky.Revit.DataExport
{
    [ContainerProvider("D4FDD867-5EA0-4906-ACD3-5F60D0F6F6A6")]
    public class App : RevitApp
    {
        private const string Tab = "Coolicky";

        public override Result OnShutdown(IUnityContainer container, UIControlledApplication uiApp)
        {
        }

        public override Result OnStartup(IUnityContainer container, UIControlledApplication uiApp)
        {
            container.RegisterInstance(new RevitTask(), new SingletonLifetimeManager());
            
            AddTab(uiApp);

            return Result.Succeeded;
        }

        private static void AddTab(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab(Tab);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}