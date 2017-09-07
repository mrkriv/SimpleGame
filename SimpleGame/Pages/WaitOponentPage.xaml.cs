using System;
using System.Collections.Generic;
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

namespace OneCGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для WaitOponentPage.xaml
    /// </summary>
    public partial class WaitOponentPage : Page
    {
        public WaitOponentPage()
        {
            InitializeComponent();

            Loaded += WaitOponentPage_Loaded;
        }

        private async void WaitOponentPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Api.ApiRequest("Game/StartFind");
            await Api.ApiRequest("Game/WaitEvent", 4 * 60 * 1000);
            NavigationService.Navigate(new GamePage(true));
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            await Api.ApiRequest("Game/EndFind");

            NavigationService.GoBack();
        }
    }
}