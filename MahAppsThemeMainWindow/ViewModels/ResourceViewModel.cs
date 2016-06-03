using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CB.Media.Brushes;
using CB.Model.Prism;
using MahAppsThemeInfrastructure;
using MahAppsThemeMainWindow.Models;


namespace MahAppsThemeMainWindow.ViewModels
{
    public class ResourceViewModel: PrismViewModelBase
    {
        #region Fields
        private double _brighness;
        private readonly ResourceDictionary _source;
        #endregion


        #region  Constructors & Destructor
        public ResourceViewModel(ResourceDictionary source)
        {
            _source = source;
            ResourceValues = source.Keys.Cast<object>().Select(
                key => new ResourceValue { Key = key, Value = source[key] }).ToList();
        }

        public ResourceViewModel(string resourceFile): this(ResourceDictionaryHandler.Read(resourceFile)) { }
        #endregion


        #region  Properties & Indexers
        public double Brighness
        {
            get { return _brighness; }
            set
            {
                if (SetProperty(ref _brighness, value))
                {
                    foreach (var resource in ResourceValues)
                    {
                        if (resource.Value is Color || resource.Value is SolidColorBrush || resource.Value is LinearGradientBrush || resource.Value is RadialGradientBrush)
                        {
                            dynamic media = _source[resource.Key];
                            resource.Value = BrushHelper.SetBrightness(media, value);
                        }
                    }
                }
            }
        }

        public IEnumerable<ResourceValue> ResourceValues { get; }
        #endregion


        #region Methods
        public void WriteToFile(string filePath)
        {
            var resourceDict = new ResourceDictionary();
            foreach (var value in ResourceValues)
            {
                resourceDict.Add(value.Key, value.Value);
            }
            ResourceDictionaryHandler.Write(resourceDict, filePath);
        }
        #endregion
    }
}