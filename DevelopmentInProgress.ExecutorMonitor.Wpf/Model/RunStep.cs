using DevelopmentInProgress.DipCore;
using DipRunner;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Model
{
    public class RunStep : EntityBase
    {
        private string message;

        public RunStep(Step step)
        {
            Step = step;
        }

        public Step Step { get; private set; }

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged("Message");
                }
            }
        }
    }
}