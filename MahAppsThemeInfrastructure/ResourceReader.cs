using System;
using System.Windows;


namespace MahAppsThemeInfrastructure
{
    public class ResourceEntry
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            return $"Name = {Name}, Type = {Type}";
        }
    }
    public class ResourceReader
    {
        #region Methods
        public static ResourceDictionary Read(string path)
        {
            return new ResourceDictionary { Source = new Uri(path, UriKind.RelativeOrAbsolute) };
        }
        #endregion
    }
}