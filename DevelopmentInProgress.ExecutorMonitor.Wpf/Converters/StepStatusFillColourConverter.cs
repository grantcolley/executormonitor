using DipRunner;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Converters
{
    public class StepStatusFillColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                      object parameter, CultureInfo culture)
        {
            var status = (StepStatus)value;

            switch (status)
            {
                case StepStatus.NotStarted:
                    return new SolidColorBrush(Colors.LightGray);
                case StepStatus.Initialise:
                    return new SolidColorBrush(Colors.Yellow);
                case StepStatus.InProgress:
                    return new SolidColorBrush(Colors.LightGreen);
                case StepStatus.Complete:
                    return new SolidColorBrush(Colors.LightSkyBlue);
                case StepStatus.Error:
                    return new SolidColorBrush(Colors.Red);
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
