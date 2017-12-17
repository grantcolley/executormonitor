using DipRunner;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Converters
{
    public class StepStatusProgressColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (StepStatus)value;

            switch (status)
            {
                case StepStatus.Initialise:
                    return new SolidColorBrush(Colors.Gold);
                case StepStatus.InProgress:
                    return new SolidColorBrush(Colors.Green);
                case StepStatus.NotStarted:
                    return new SolidColorBrush(Colors.Gray);
                case StepStatus.Complete:
                    return new SolidColorBrush(Colors.RoyalBlue);
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
