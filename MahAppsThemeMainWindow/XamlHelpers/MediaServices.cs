using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace MahAppsThemeMainWindow.XamlHelpers
{
    public static class MediaServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty MediaItemProperty = DependencyProperty.RegisterAttached(
            "MediaItem", typeof(object), typeof(MediaServices),
            new PropertyMetadata(default(object), OnMediaItemChanged));

        [Category("MediaServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object GetMediaItem(DependencyObject d)
            => d.GetValue(MediaItemProperty);

        [Category("MediaServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetMediaItem(DependencyObject d, object value)
            => d.SetValue(MediaItemProperty, value);
        #endregion


        #region Implementation
        private static void OnMediaItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = e.NewValue as Brush;
            var brush = b ?? (e.NewValue is Color ? new SolidColorBrush((Color)e.NewValue) : null);

            var control = d as Control;
            if (control != null) control.Background = brush;
            else
            {
                var shape = d as Shape;
                if (shape != null) shape.Fill = brush;
            }
        }
        #endregion
    }
}