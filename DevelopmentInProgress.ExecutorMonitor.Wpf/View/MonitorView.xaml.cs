using DevelopmentInProgress.ExecutorMonitor.Wpf.ViewModel;
using DevelopmentInProgress.Origin.Context;
using DevelopmentInProgress.Origin.View;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.View
{
    /// <summary>
    /// Interaction logic for MonitorView.xaml
    /// </summary>
    public partial class MonitorView : DocumentViewBase
    {
        public MonitorView(IViewContext viewContext, MonitorViewModel monitorViewModel)
            : base(viewContext, monitorViewModel, Module.ModuleName)
        {
            InitializeComponent();

            DataContext = monitorViewModel;
        }
    }
}