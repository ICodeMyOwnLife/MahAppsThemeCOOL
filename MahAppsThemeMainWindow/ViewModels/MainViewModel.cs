using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BrushEditor;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using MahAppsThemeInfrastructure;
using Microsoft.Practices.Prism.Commands;


namespace MahAppsThemeMainWindow.ViewModels
{
    public class MainViewModel: PrismViewModelBase
    {
        #region Fields
        private ResourceDictionary _resources;
        private string _selectedFile;
        #endregion


        #region  Constructors & Destructor
        public MainViewModel()
        {
            ReloadFiles();
            EditMediaCommand = new DelegateCommand<object>(EditMedia);
        }
        #endregion


        #region  Properties & Indexers
        public ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel> BrushRequest { get; } =
            new ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel>();

        public ConfirmationInteractionRequest<ColorPickerViewModel> ColorRequest { get; } =
            new ConfirmationInteractionRequest<ColorPickerViewModel>();

        public ICommand EditMediaCommand { get; }

        public IList<string> Files { get; } = new ObservableCollection<string>();

        public ResourceDictionary Resources
        {
            get { return _resources; }
            private set { SetProperty(ref _resources, value); }
        }

        public string SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (SetProperty(ref _selectedFile, value) && !string.IsNullOrEmpty(value))
                {
                    Resources = ResourceReader.Read(value);
                }
            }
        }
        #endregion


        #region Methods
        public void EditMedia(object cmdParameter)
        {
            var entry = (DictionaryEntry)cmdParameter;
            var lgb = entry.Value as LinearGradientBrush;
            if (lgb != null)
            {
                BrushRequest.Raise(new LinearGradientBrushPickerViewModel { Brush = lgb }, res =>
                {
                    if (res.Confirmed)
                    {
                        Resources[entry.Key] = res.Brush;
                    }
                });
            }
            else
            {
                Color color;
                if (entry.Value is Color)
                {
                    color = (Color)entry.Value;
                }
                else if (entry.Value is SolidColorBrush)
                {
                    color = ((SolidColorBrush)entry.Value).Color;
                }
                else
                {
                    return;
                }
                ColorRequest.Raise(
                    new ColorPickerViewModel { ColorEditorViewModel = new ColorEditorViewModel { Color = color } },
                    res =>
                    {
                        if (res.Confirmed)
                        {
                            Resources[entry.Key] = res.ColorEditorViewModel.Color;
                        }
                    });
            }
        }

        public void ReloadFiles()
        {
            Files.Clear();
            var filesFolder = ConfigurationManager.AppSettings["files"];
            foreach (var file in Directory.EnumerateFiles(Path.GetFullPath(filesFolder)))
            {
                Files.Add(file);
            }
            SelectedFile = Files.FirstOrDefault();
        }
        #endregion
    }
}