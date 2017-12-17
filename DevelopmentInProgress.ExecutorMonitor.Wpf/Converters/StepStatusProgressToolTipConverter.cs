using DipRunner;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Converters
{
    public class StepStatusProgressToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (StepStatus)value;

            switch (status)
            {
                case StepStatus.Initialise:
                    return "Initialising...";
                case StepStatus.InProgress:
                    return "In progress...";
                case StepStatus.NotStarted:
                    return "Not started";
                case StepStatus.Complete:
                    return "Complete";
                case StepStatus.Error:
                    return "Error";
                default:
                    throw new NotImplementedException($"Status {status}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
