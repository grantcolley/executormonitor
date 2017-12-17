using DipRunner;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Converters
{
    public class StepStatusInactiveToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (StepStatus)value;

            switch (status)
            {
                case StepStatus.Initialise:
                case StepStatus.InProgress:
                    return Visibility.Collapsed;
                case StepStatus.NotStarted:
                case StepStatus.Complete:
                case StepStatus.Error:
                    return Visibility.Visible;
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
