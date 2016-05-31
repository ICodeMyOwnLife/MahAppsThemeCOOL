using System.Windows.Media;
using BrushEditor;
using CB.Model.Prism;


namespace MahAppsThemeMainWindow.ViewModels
{
    public class ColorPickerViewModel: ConfirmationViewModelBase
    {
        #region Fields
        private ColorEditorViewModel _colorEditorViewModel;
        #endregion


        #region  Properties & Indexers
        public ColorEditorViewModel ColorEditorViewModel
        {
            get { return _colorEditorViewModel; }
            set { SetProperty(ref _colorEditorViewModel, value); }
        }
        #endregion
    }

    public class LinearGradientBrushPickerViewModel: ConfirmationViewModelBase
    {
        #region Fields
        private LinearGradientBrush _brush;
        #endregion


        #region  Properties & Indexers
        public LinearGradientBrush Brush
        {
            get { return _brush; }
            set { SetProperty(ref _brush, value); }
        }
        #endregion
    }
}