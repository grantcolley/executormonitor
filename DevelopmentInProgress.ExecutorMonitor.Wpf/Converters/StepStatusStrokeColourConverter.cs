using DipRunner;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DevelopmentInProgress.ExecutorMonitor.Wpf.Converters
{
    public class StepStatusStrokeColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
              object parameter, CultureInfo culture)
        {
            var status = (StepStatus)value;

            switch (status)
            {
                case StepStatus.NotStarted:
                    return new SolidColorBrush(Colors.Gray);
                case StepStatus.Initialise:
                    return new SolidColorBrush(Colors.Gold);
                case StepStatus.InProgress:
                    return new SolidColorBrush(Colors.MediumSeaGreen);
                case StepStatus.Complete:
                    return new SolidColorBrush(Colors.Navy);
                case StepStatus.Error:
                    return new SolidColorBrush(Colors.DarkRed);
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
