using DipRunner;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Model
{
    public class Run
    {
        public int RunId { get; set; }
        public string RunName { get; set; }
        public string NotificationUrl{get;set;}
        public Step Step { get; set; }
    }
}