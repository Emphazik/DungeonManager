using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;
using DungeonManager.ApplicationData;

namespace DungeonManager.AuthUsersWindows
{
    /// <summary>
    /// Логика взаимодействия для UserOrderHistoryWindow.xaml
    /// </summary>
    public partial class UserOrderHistoryWindow : Window
    {
        private int UserId { get; set; }
        public string UserLogin { get; set; }
        public ObservableCollection<OrderHistoryViewModel> OrderHistory { get; set; }

        public UserOrderHistoryWindow()
        {
            InitializeComponent();

            if (App.Current.Properties["LoginUser"] is string UserLogin1)
            {
                UserLogin = UserLogin1;
            }

            if (App.Current.Properties["idUser"] is int UserId1)
            {
                UserId = UserId1;
            }
            //UserId = userId;
            //UserLogin = userLogin;

            DataContext = this;
            LoadOrderHistory();

        }

        private void LoadOrderHistory()
        {
            try
            {
                var orderHistory = AppConnect.DarkAndDarkBD.Orders
                 .Where(o => o.idUser == UserId)
                 .Include(o => o.OrderItems)
                 .Include(o => o.OrderStatus)
                 .ToList()
                 .Select(order => new
                 {
                     idOrder = order.idOrder,
                     OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("g") : "Не указано",
                     StatusName = order.OrderStatus.StatusName,
                     TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity)
                 })
                 .ToList();

                OrdersListView.ItemsSource = orderHistory;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории заказов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write($"Ошибка загрузки истории заказов: {ex.Message}");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderHistory();
        }

        //Контекст юзера 
        private void ClientTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (textBlock != null && textBlock.ContextMenu != null)
            {
                textBlock.ContextMenu.PlacementTarget = textBlock;
                textBlock.ContextMenu.IsOpen = true;
            }
        }

        private void MenuItem_Events_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на главную страницу.");
            new UserWindow().Show();
            this.Close();
        }

        private void MenuItem_Orders_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу заказов.");
            new CartViewWindow().Show();
            this.Close();
        }
        private void MenuItem_OrderHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Истории заказов.");
            new UserOrderHistoryWindow().Show();
            this.Close();
        }
        private void MenuItem_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выход из профиля.");
            new MainWindow().Show();
            this.Close();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выход из приложения.");
            Application.Current.Shutdown();
        }
    }

    public class OrderHistoryViewModel
    {
        public int idOrder { get; set; }
        public string OrderDate { get; set; }
        public string StatusName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
