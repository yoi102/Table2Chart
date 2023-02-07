using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 好像未使用
    /// </summary>
    public class LineSeriesDetailPointsViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        public LineSeriesDetailPointsViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSave);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        public string DialogHostName { get; set; }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private List<TDataPoint> _Points;
        public bool KeepAlive => false;

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Points = parameters.ContainsKey("Points") ? parameters.GetValue<List<TDataPoint>>("Points") : new List<TDataPoint>();
        }

        public List<TDataPoint> Points
        {
            get { return _Points; }
            set { SetProperty(ref _Points, value); }
        }

        private void ExecuteSave()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        private void ExecuteCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, new DialogParameters()));
        }
    }
}