using System;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.Applications;
using Revit.DependencyInjection.Unity.Base;
using Revit.DependencyInjection.Unity.RibbonCommands;
using Revit.DependencyInjection.Unity.UI;
using Unity;
using Unity.Lifetime;

namespace Coolicky.Revit.DataExport
{
    [ContainerProvider("D4FDD867-5EA0-4906-ACD3-5F60D0F6F6A6")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            ribbonManager.AddRibbonCommands(config =>
            {
                config.TabName = "Coolicky";
                config.DefaultPanelName = "Data Export";
            });
        }

        public override Result OnShutdown(IUnityContainer container, UIControlledApplication uiApp)
        {
            return Result.Succeeded;
        }

        public override Result OnStartup(IUnityContainer container, UIControlledApplication uiApp)
        {
            container.RegisterInstance(new RevitTask(), new SingletonLifetimeManager());
            

            return Result.Succeeded;
        }
    }
}