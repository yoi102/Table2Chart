using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.Dialogs
{
    /// <summary>
    /// 如退出程序弹窗，消息弹窗
    /// </summary>
    public class MsgViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        private string _Content;

        private string _Title;

        public MsgViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public DelegateCommand CancelCommand { get; set; }
        /// <summary>
        /// 中间的文本
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; RaisePropertyChanged(); }
        }

        public string DialogHostName { get; set; }
        public bool KeepAlive => false;
        public DelegateCommand SaveCommand { get; set; }
        /// <summary>
        /// 左上角文字，如温馨提示
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 打开窗口时，获取文本
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Title"))
                Title = parameters.GetValue<string>("Title");
            if (parameters.ContainsKey("Content"))
                Content = parameters.GetValue<string>("Content");
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, new DialogParameters()));
            }

        }

        /// <summary>
        /// 确定
        /// </summary>
        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, new DialogParameters()));

            }
        }
    }
}