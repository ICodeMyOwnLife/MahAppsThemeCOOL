using System;
using System.Windows;
using System.Windows.Markup;
using System.Xml;


namespace MahAppsThemeInfrastructure
{
    public class ResourceDictionaryHandler
    {
        #region Methods
        public static ResourceDictionary Read(string path)
            => new ResourceDictionary { Source = new Uri(path, UriKind.RelativeOrAbsolute) };

        public static void Write(ResourceDictionary resourceDictionary, string path)
        {
            using (var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true }))
            {
                XamlWriter.Save(resourceDictionary, writer);
            }
        }
        #endregion
    }
}