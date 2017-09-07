using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace OneCGame
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private bool isMyTurn;

        public bool IsMyTurn
        {
            get => isMyTurn;
            set
            {
                YouTurn.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                Wait.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
                isMyTurn = value;
            }
        }

        public GamePage(bool IsHost)
        {
            InitializeComponent();

            new Task(UpdateLoop).RunSynchronously();

            IsMyTurn = IsHost;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var cell = new Cell(i + j * 3);
                    cell.OnClick += Cell_OnClick;

                    GameMap.Children.Add(cell);
                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);
                }
            }
        }

        private async void UpdateLoop()
        {
            while (true)
            {
                var ev = await Api.ApiRequest("Game/WaitEvent", 2 * 60 * 1000);

                if (ev.StatusCode != System.Net.HttpStatusCode.OK)
                    continue;

                var events = JsonConvert.DeserializeObject<List<string>>(ev.Content);
                foreach (var eventName in events)
                {
                    var callParam = eventName.Split('?');

                    var method = GetType().GetMethods()
                        .Where(m => m.Name.Equals(callParam[0], StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    if (method == null || method.GetCustomAttributes<ExecuteFromServerAttribute>() == null)
                    {
                        continue;
                    }

                    if (callParam.Length > 1)
                        method.Invoke(this, new[] { callParam[1] });
                    else
                        method.Invoke(this, null);
                }
            }
        }

        private async void Cell_OnClick(object sender, int Index)
        {
            if (isMyTurn)
                await Api.ApiRequest($"Game/event/Insert/{Index}");
        }

        [ExecuteFromServer]
        public void NextTurn()
        {
            IsMyTurn = !IsMyTurn;
        }

        [ExecuteFromServer]
        public void SetX(string Index)
        {
            FindCell(int.Parse(Index)).State = CellState.X;
        }

        [ExecuteFromServer]
        public void SetO(string Index)
        {
            FindCell(int.Parse(Index)).State = CellState.O;
        }

        [ExecuteFromServer]
        public void EndGame(string Message)
        {
            NavigationService.Navigate(new EndGamePage(Message));
        }

        private Cell FindCell(int Index)
        {
            foreach (Cell child in GameMap.Children)
            {
                if (child.CellIndex == Index)
                    return child;
            }

            return null;
        }
    }
}