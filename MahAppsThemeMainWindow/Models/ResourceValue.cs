using CB.Model.Common;


namespace MahAppsThemeMainWindow.Models
{
    public class ResourceValue: BindableObject
    {
        #region Fields
        private object _key;
        private object _value;
        #endregion


        #region  Properties & Indexers
        public object Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        #endregion
    }
}