using MahApps.Metro.Controls;
using Prism.Services.Dialogs;
using System.Windows;

namespace Table2Chart.Views.Dialogs
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : MetroWindow, IDialogWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
            Closing += DialogWindow_Closing;
        }

        private void DialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((UIElement)Content).Focus();//为提交数据
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            Title = ((IDialogAware)DataContext).Title;
        }

        public IDialogResult Result { get; set; }
    }
}