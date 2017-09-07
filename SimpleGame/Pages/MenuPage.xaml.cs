using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace OneCGame
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            IsRemember.IsChecked = !string.IsNullOrEmpty(Properties.Settings.Default.Login);
            LoginBox.Text = Properties.Settings.Default.Login;

            IsRemember.Checked += IsRemember_Checked;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            if (IsRemember.IsChecked == true)
            {
                Properties.Settings.Default.Login = LoginBox.Text;
                Properties.Settings.Default.Save();
            }

            var result = await Api.ApiRequest($"User/Auth?login={LoginBox.Text}&password={PasswordBox.Password}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                NavigationService?.Navigate(new Uri("Pages/LobbyPage.xaml", UriKind.Relative));
            }
            else if (result.StatusCode == HttpStatusCode.Forbidden)
            {
                NavigationService?.Navigate(new ErrorPage("Invalid password"));
            }
            else
                NavigationService?.Navigate(new ErrorPage("Request failed"));

            IsEnabled = true;
        }

        private void LoginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login_Click(sender, null);
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/SettingPage.xaml", UriKind.Relative));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void IsRemember_Checked(object sender, RoutedEventArgs e)
        {
            if (IsRemember.IsChecked == true)
            {
                Properties.Settings.Default.Login = LoginBox.Text;
            }
            else
            {
                Properties.Settings.Default.Login = "";
            }
        }
    }
}