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
using KPO.MainControl;
using KPO.VM;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace KPO.MainControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartVm(null, null);

        }

        private void StartVm(object sender, EventArgs e)
        {

            var tmp = new ViewModel
            {
                GridCartoonsList = GridCartoonsList,
                CalendarFrom = CalendarFrom,
                CalendarTo = CalendarTo 
            };

            Base.DataContext = tmp;
            Base.Visibility = Visibility.Visible;
        }

        private void RadContextMenu_OnOpening(object sender, RadRoutedEventArgs e) {
            var tmp = Base.DataContext as ViewModel;
            var contextMenu = sender as RadContextMenu;
            var row = contextMenu?.GetClickedElement<GridViewRow>();
            GridCartoonsList.SelectedItem = row?.DataContext;
            if (row != null) {
                contextMenu.ItemsSource = tmp?.GetContextMenu(row?.DataContext as TheaterPerformanceVM);

            }

        }

        private void ButtonMin_OnClick(object sender, RoutedEventArgs e)
        {
            MWindow.WindowState = WindowState.Minimized;
        }
        private WindowState oldstate { get; set; }
        private void ButtonMax_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MWindow.Left = 2D;
                MWindow.Top = 2D;
            }
            DragMove();
        }

        private void PART_RestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            Width = 1400;
            Height = 1024;

        }

        private void SecondListBoxRamps_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!(Base.DataContext is ViewModel vm))return;
            var item = ((KeyValuePair<int, SlotsVM>) e.AddedItems[0]).Value;

            if (item.IsFree)
                vm.SelectedSlots = ((KeyValuePair<int, SlotsVM>) e.AddedItems[0]).Value;
        }
    }
}
