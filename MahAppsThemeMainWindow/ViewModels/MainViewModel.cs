using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CB.IO.Common;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using Microsoft.Practices.Prism.Commands;


namespace MahAppsThemeMainWindow.ViewModels
{
    public class MainViewModel: PrismViewModelBase
    {
        #region Fields
        /*public ResourceDictionary Resources
        {
            get { return _resources; }
            private set { SetProperty(ref _resources, value); }
        }*/

        private ResourceViewModel _resources;

        private readonly IDictionary<string, ResourceViewModel> _resourcesMap =
            new Dictionary<string, ResourceViewModel>();

        //private ResourceDictionary _resources;
        private string _selectedFile;
        #endregion


        #region  Constructors & Destructor
        public MainViewModel()
        {
            ReloadFiles();
            ReloadFilesCommand = new DelegateCommand(ReloadFiles);
            SaveAsCommand = new DelegateCommand(SaveAs);
        }
        #endregion


        #region  Properties & Indexers
        public ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel> BrushRequest { get; } =
            new ConfirmationInteractionRequest<LinearGradientBrushPickerViewModel>();

        public ConfirmationInteractionRequest<ColorPickerViewModel> ColorRequest { get; } =
            new ConfirmationInteractionRequest<ColorPickerViewModel>();

        public IList<string> Files { get; } = new ObservableCollection<string>();

        public ResourceViewModel Resources
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
                    //Resources = ResourceDictionaryHandler.Read(value);
                    Resources = GetResources(value);
                }
            }
        }
        #endregion


        #region Methods
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

                //ResourceDictionaryHandler.Write(Resources, file);
                Resources.WriteToFile(file);
                Files.Add(file);
                SelectedFile = file;
                IO.OpenExplorerToShow(file);
            });
        }
        #endregion


        #region Implementation
        private static string GetDefaultFolderPath()
            => Path.GetFullPath(ConfigurationManager.AppSettings["files"]);

        private ResourceViewModel GetResources(string path)
        {
            ResourceViewModel result;
            if (!_resourcesMap.TryGetValue(path, out result))
            {
                result = new ResourceViewModel(path);
                _resourcesMap[path] = result;
            }
            return result;
        }
        #endregion


        #region  Commands
        public ICommand ReloadFilesCommand { get; }
        public ICommand SaveAsCommand { get; }
        #endregion
    }
}


// TODO: Implement Color Edit & Brush Edit
// TODO: Implement Hue, Brighness, Opacity, Saturation adjust
// TODO: Why program not end - solved
// TODO: Resource values does not change or changes isn't shown