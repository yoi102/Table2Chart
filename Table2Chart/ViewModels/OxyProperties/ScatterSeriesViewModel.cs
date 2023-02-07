using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Table2Chart.Common.Models;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 好像未使用
    /// </summary>
    public class ScatterSeriesViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        public ScatterSeriesViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSave);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private MyPlotModel _Model;

        public bool KeepAlive => false;

        public MyPlotModel Model
        {
            get { return _Model; }
            set { SetProperty(ref _Model, value); }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Model = parameters.ContainsKey("Model") ? parameters.GetValue<MyPlotModel>("Model") : new MyPlotModel();
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
            {
                DialogParameters param = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, param));
            }
        }
    }
}