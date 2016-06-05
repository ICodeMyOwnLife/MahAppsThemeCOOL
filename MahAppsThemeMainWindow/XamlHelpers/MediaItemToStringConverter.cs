using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace MahAppsThemeMainWindow.XamlHelpers
{
    public class MediaItemToStringConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lgb = value as LinearGradientBrush;
            if (lgb != null)
            {
                return
                    $"{value.GetType()} [{lgb.StartPoint} - {lgb.EndPoint}]:{Environment.NewLine}{GradientStopsToString(lgb.GradientStops)}";
            }
            var rgb = value as RadialGradientBrush;
            return rgb != null
                       ? $"{value.GetType()} [{rgb.GradientOrigin} x {rgb.Center}, {rgb.RadiusX} x {rgb.RadiusY}:{Environment.NewLine}{GradientStopsToString(rgb.GradientStops)}"
                       : value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
        #endregion


        #region Implementation
        private static string GradientStopsToString(GradientStopCollection gradientStops)
            => string.Join(Environment.NewLine, gradientStops.Select(s => $"{s.Color}: {s.Offset}"));
        #endregion
    }
}