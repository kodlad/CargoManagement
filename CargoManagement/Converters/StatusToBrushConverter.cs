using System;
using System.Globalization;
using System.Windows;          // <-- сюда
using System.Windows.Data;
using System.Windows.Media;

namespace CargoManagement.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            return status switch
            {
                "Planned" => Application.Current.FindResource("PlannedBrush") as SolidColorBrush,
                "On Route" => Application.Current.FindResource("OnRouteBrush") as SolidColorBrush,
                "Completed" => Application.Current.FindResource("CompletedBrush") as SolidColorBrush,
                _ => Brushes.Gray,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
