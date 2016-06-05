using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahAppsThemeMainWindow.Models;


namespace MahAppsThemeMainWindow.XamlHelpers
{
    public class ResourceValueEditTemplateSelector: DataTemplateSelector
    {
        #region Override
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;
            var resourceValue = (ResourceValue)item;

            return resourceValue?.Value is Color
                       ? element.FindResource("ColorEditColumn") as DataTemplate
                       : resourceValue?.Value is Brush
                             ? element.FindResource("BrushEditColumn") as DataTemplate
                             : null;
        }
        #endregion
    }
}