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
using DungeonManager.AuthUsersWindows;
using DungeonManager.Model;

namespace DungeonManager.ManagerWindows
{
    /// <summary>
    /// Логика взаимодействия для ManagerOrderHistoryWindow.xaml
    /// </summary>
    public partial class ManagerOrderHistoryWindow : Window
    {
        public ObservableCollection<OrderHistoryViewModel> OrderHistory { get; set; }

        private int ManId { get; set; }
        private string Login { get; set; }

        public ManagerOrderHistoryWindow()
        {
            InitializeComponent();
            if (App.Current.Properties["LoginManager"] is string ManLogin)
            {
                Login = ManLogin;
            }

            if (App.Current.Properties["idManager"] is int ManId1)
            {
                ManId = ManId1;
            }
            //MessageBox.Show($"Ошибка загрузки данных: {Login + ManId}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            DataContext = this;
            LoadOrderHistory();
            OrderTypeComboBox.SelectedIndex = 0;
        }

        private void LoadOrderHistory()
        {
            try
            {
                var orderHistoryQuery = AppConnect.DarkAndDarkBD.Orders.Include(o => o.OrderItems)
                                                                         .Include(o => o.OrderStatus)
                                                                         .AsQueryable();

                var selectedOrderType = (OrderTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedOrderType == "Личные")
                {
                    orderHistoryQuery = orderHistoryQuery.Where(o => o.idUser == ManId);
                }

                var orderHistory = orderHistoryQuery.ToList()
                    .Select(order => new OrderHistoryViewModel
                    {
                        idOrder = order.idOrder,
                        idUser = order.idUser,
                        idStatus = (int)order.idStatus,
                        OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("g") : "Не указано",
                        StatusName = order.OrderStatus.StatusName,
                        TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity)
                    })
                    .ToList();

                OrdersDataGrid.ItemsSource = orderHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории заказов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OrderTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOrderHistory();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderHistory();
        }
        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную строку в DataGrid
            var selectedOrder = OrdersDataGrid.SelectedItem as OrderHistoryViewModel;

            if (selectedOrder != null)
            {
                // Меняем статус по циклу
                switch (selectedOrder.idStatus)
                {
                    case 1:
                        selectedOrder.idStatus = 2;
                        selectedOrder.StatusName = "Подтверждено";
                        break;
                    case 2:
                        selectedOrder.idStatus = 3;
                        selectedOrder.StatusName = "Отклонено";
                        break;
                    case 3:
                        selectedOrder.idStatus = 1;
                        selectedOrder.StatusName = "В ожидании";
                        break;
                }

                // Обновляем статус в базе данных
                var orderToUpdate = AppConnect.DarkAndDarkBD.Orders
                    .FirstOrDefault(o => o.idOrder == selectedOrder.idOrder);

                if (orderToUpdate != null)
                {
                    orderToUpdate.idStatus = selectedOrder.idStatus;
                    AppConnect.DarkAndDarkBD.SaveChanges();
                    MessageBox.Show("Статус заказа успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Обновляем данные на экране
                LoadOrderHistory();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Контекст юзера 
        private void ManagerTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            new ManagerWindow().Show();
            this.Close();
        }

        private void MenuItem_Orders_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Корзины.");
            new ManagerCartWindow().Show();
            this.Close();
        }
        private void MenuItem_OrderHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Управление заказами.");
            new ManagerOrderHistoryWindow().Show();
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

        public class OrderHistoryViewModel
        {
            public int idOrder { get; set; }
            public int idUser { get; set; }
            public int idStatus { get; set; }
            public string OrderDate { get; set; }
            public string StatusName { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public class Status
        {
            public int idStatus { get; set; }
            public string StatusName { get; set; }
        }

        
    }
}
