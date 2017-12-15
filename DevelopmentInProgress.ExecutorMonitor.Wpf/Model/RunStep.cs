using DipRunner;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Model
{
    public class RunStep : Step, INotifyPropertyChanged
    {
        private string message;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<RunStep> Children { get; set; }

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

        public string ToolTip
        {
            get;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
            {
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}