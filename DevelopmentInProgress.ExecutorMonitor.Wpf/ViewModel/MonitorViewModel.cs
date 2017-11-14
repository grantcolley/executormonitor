using DevelopmentInProgress.Origin.Context;
using DevelopmentInProgress.Origin.ViewModel;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.ViewModel
{
    public class MonitorViewModel : DocumentViewModel
    {
        public MonitorViewModel(ViewModelContext viewModelContext)
            : base(viewModelContext)
        {
        }

        protected async override void OnPublished(object data)
        {
            // Do stuff here...
        }

        protected async override void SaveDocument()
        {
            // Save stuff here...
        }
    }
}
