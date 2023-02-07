using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace Table2Chart.Common.Services
{
    /// <summary>
    /// 用于md 的对话主机服务
    /// </summary>
    public interface IDialogHostService : IDialogService
    {
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root");

        Task<IDialogResult> ShowDialog<T>(IDialogParameters parameters, string dialogHostName = "Root");
    }
}