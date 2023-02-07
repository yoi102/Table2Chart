using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace Table2Chart.Views.Dialogs
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string password = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            if (passwordBox.Password.Equals(password))
            {
                DialogResult = true;
            }
            else
            {
                HintAssist.SetHelperText(passwordBox, "密码错误!!");
                //passwordBox.
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string password = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
                if (passwordBox.Password.Equals(password))
                {
                    DialogResult = true;
                }
                else
                {
                    HintAssist.SetHelperText(passwordBox, "密码错误!!");
                    //passwordBox.
                }
            }
        }
    }
}