using Prism.Mvvm;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    public class SeriesPropertyBase : BindableBase
    {
        private string _Title = string.Empty;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
    }
}