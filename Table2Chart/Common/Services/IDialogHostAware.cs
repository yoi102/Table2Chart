using Prism.Commands;
using Prism.Services.Dialogs;

namespace Table2Chart.Common.Services
{
    /// <summary>
    /// 用于md的对话弹窗感知
    /// </summary>
    public interface IDialogHostAware
    {
        /// <summary>
        /// 所属DialogHost名称
        /// </summary>
        string DialogHostName { get; set; }

        /// <summary>
        /// 打开过程中执行
        /// </summary>
        /// <param name="parameters"></param>
        void OnDialogOpened(IDialogParameters parameters);

        DelegateCommand SaveCommand { get; set; }
        DelegateCommand CancelCommand { get; set; }
    }
}