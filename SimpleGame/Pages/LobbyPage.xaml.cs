using Newtonsoft.Json;
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
    /// Логика взаимодействия для LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : Page
    {
        public LobbyPage()
        {
            InitializeComponent();

            Loaded += LobbyPage_Loaded;
        }

        private void LobbyPage_Loaded(object sender, RoutedEventArgs e)
        {
            new Task(UpdateItems).RunSynchronously();
        }

        private async void UpdateItems()
        {
            Update.IsEnabled = false;

            var result = await Api.ApiRequest("Game/GetFreeGame");
            var games = JsonConvert.DeserializeObject<List<string>>(result.Content);

            GameList.Items.Clear();

            if (games != null)
            {
                foreach (var game in games)
                {
                    GameList.Items.Add(game);
                }
            }

            Update.IsEnabled = true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            new Task(UpdateItems).RunSynchronously();
        }

        private async void GameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            IsEnabled = false;

            var result = await Api.ApiRequest($"Game/JoinToGame?UserALogin={e.AddedItems[0]}");
            NavigationService?.Navigate(new GamePage(false));

            IsEnabled = true;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/WaitOponentPage.xaml", UriKind.Relative));
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/SettingPage.xaml", UriKind.Relative));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}