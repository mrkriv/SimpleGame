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

namespace OneCGame
{
    public enum CellState
    {
        None, X, O
    }

    /// <summary>
    /// Логика взаимодействия для Cell.xaml
    /// </summary>
    public partial class Cell : UserControl
    {
        private CellState state;
        public readonly int CellIndex;

        public CellState State
        {
            get => state;
            set
            {
                IconO.Visibility = value == CellState.O ? Visibility.Visible : Visibility.Collapsed;
                IconX.Visibility = value == CellState.X ? Visibility.Visible : Visibility.Collapsed;
                state = value;
            }
        }

        public event EventHandler<int> OnClick = new EventHandler<int>((_, __) => { });

        public Cell(int index)
        {
            CellIndex = index;

            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnClick(this, CellIndex);
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new SolidColorBrush(Color.FromRgb(214, 197, 172));
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new SolidColorBrush(Color.FromRgb(214, 214, 214));
        }
    }
}