using DevelopmentInProgress.ExecutorMonitor.Wpf.View;
using DevelopmentInProgress.ExecutorMonitor.Wpf.ViewModel;
using DevelopmentInProgress.Origin.Module;
using DevelopmentInProgress.Origin.Navigation;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using System;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf
{
    public class Module : ModuleBase
    {
        public const string ModuleName = "Monitor";

        public Module(IUnityContainer container, ModuleNavigator moduleNavigator, ILoggerFacade logger)
            : base(container, moduleNavigator, logger)
        {
        }

        public override void Initialize()
        {
            Container.RegisterType<Object, MonitorView>(typeof(MonitorView).Name);
            Container.RegisterType<MonitorViewModel>(typeof(MonitorViewModel).Name);

            var moduleSettings = new ModuleSettings();
            moduleSettings.ModuleName = ModuleName;
            moduleSettings.ModuleImagePath = @"/DevelopmentInProgress.ExecutorMonitor.Wpf;component/Images/monitor.png";

            var moduleGroup = new ModuleGroup();
            moduleGroup.ModuleGroupName = "Distributor Monitor";

            var newDocument = new ModuleGroupItem();
            newDocument.ModuleGroupItemName = "Distributor";
            newDocument.TargetView = typeof(MonitorView).Name;
            newDocument.TargetViewTitle = "Distributor";
            newDocument.ModuleGroupItemImagePath = @"/DevelopmentInProgress.ExecutorMonitor.Wpf;component/Images/distributor.png";

            moduleGroup.ModuleGroupItems.Add(newDocument);
            moduleSettings.ModuleGroups.Add(moduleGroup);
            ModuleNavigator.AddModuleNavigation(moduleSettings);

            Logger.Log("Initialize DevelopmentInProgress.ExecutorMonitor.Wpf Complete", Category.Info, Priority.None);
        }
    }
}
