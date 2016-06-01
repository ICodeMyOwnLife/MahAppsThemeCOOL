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
using CB.IO.Common;
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
            ReloadFilesCommand = new DelegateCommand(ReloadFiles);
            SaveAsCommand = new DelegateCommand(SaveAs);
        }
        #endregion


        #region  Commands
        public ICommand EditMediaCommand { get; }
        public ICommand ReloadFilesCommand { get; }
        public ICommand SaveAsCommand { get; }
        #endregion


        #region  Properties & Indexers
        public ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel> BrushRequest { get; } =
            new ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel>();

        public ConfirmationInteractionRequest<ColorPickerViewModel> ColorRequest { get; } =
            new ConfirmationInteractionRequest<ColorPickerViewModel>();

        public IList<string> Files { get; } = new ObservableCollection<string>();

        public ResourceDictionary Resources
        {
            get { return _resources; }
            private set { SetProperty(ref _resources, value); }
        }

        public ConfirmationInteractionRequest<SaveFileDialogInfo> SaveFileRequest { get; } =
            new ConfirmationInteractionRequest<SaveFileDialogInfo>();

        public string SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (SetProperty(ref _selectedFile, value) && !string.IsNullOrEmpty(value))
                {
                    Resources = ResourceDictionaryHandler.Read(value);
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
            foreach (var file in Directory.EnumerateFiles(GetDefaultFolderPath()))
            {
                Files.Add(file);
            }
            SelectedFile = Files.FirstOrDefault();
        }

        public void SaveAs()
        {
            var saveInfo = new SaveFileDialogInfo
            {
                InitialDirectory = GetDefaultFolderPath(),
                Filter = "Resource Dictionary (*.xaml)|*.xaml"
            };

            SaveFileRequest.Raise(saveInfo, res =>
            {
                if (!res.Confirmed) return;

                var file = res.FileName;
                ResourceDictionaryHandler.Write(Resources, file);
                Files.Add(file);
                SelectedFile = file;
                IO.OpenExplorerToShow(file);
            });
        }
        #endregion


        #region Implementation
        private static string GetDefaultFolderPath()
            => Path.GetFullPath(ConfigurationManager.AppSettings["files"]);
        #endregion
    }
}


// TODO: Implement Color Edit & Brush Edit
// TODO: Implement Hue, Brighness, Opacity, Saturation adjust
// TODO: Why program not end - solved
// TODO: Resource values does not change or changes isn't shown