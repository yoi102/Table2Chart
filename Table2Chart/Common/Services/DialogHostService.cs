using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Table2Chart.Common.Services
{
    /// <summary>
    /// 用于对话主机服务（自定义）
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        /// <summary>
        /// MaterialDesignThemes的ShowDialog。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="dialogHostName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();

            //从容器当中取出弹出窗口的实例
            var content = containerExtension.Resolve<object>(name);
            //验证实例的有效性
            return await ShowDialogMethod(parameters, dialogHostName, content);
        }

        private static async Task<IDialogResult> ShowDialogMethod(IDialogParameters parameters, string dialogHostName, object content)
        {
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }

            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            viewModel.DialogHostName = dialogHostName;

            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpened(parameters);
                }

                eventArgs.Session.UpdateContent(content);
            };

            if (DialogHost.IsDialogOpen(viewModel.DialogHostName))
            {
                DialogHost.Close(viewModel.DialogHostName);
            }
            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="dialogHostName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<IDialogResult> ShowDialog<TView>(IDialogParameters parameters, string dialogHostName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();
            //从容器当中取出弹出窗口的实例
            var content = containerExtension.Resolve<object>(typeof(TView).Name);
            //验证实例的有效性
            return await ShowDialogMethod(parameters, dialogHostName, content);
        }
    }
}