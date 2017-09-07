using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using System.Threading;

namespace OneCGame
{
    /// <summary>
    /// Логика взаимодействия для SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void SetLangEN_Click(object sender, RoutedEventArgs e)
        {
            App.SetLang("en-US");
        }

        private void SetLangRU_Click(object sender, RoutedEventArgs e)
        {
            App.SetLang("ru-RU");
        }

        private void ServerAdress_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}