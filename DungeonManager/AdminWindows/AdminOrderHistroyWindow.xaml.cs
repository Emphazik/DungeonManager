using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DungeonManager.ApplicationData;

namespace DungeonManager.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для AdminOrderHistroyWindow.xaml
    /// </summary>
    public partial class AdminOrderHistroyWindow : Window
    {
        public ObservableCollection<OrderHistoryViewModel> OrderHistory { get; set; }

        private int AdminId { get; set; }
        private string LoginAdmin { get; set; }
        public AdminOrderHistroyWindow()
        {
            InitializeComponent();

            if (App.Current.Properties["LoginAdmin"] is string ManLogin)
            {
                LoginAdmin = ManLogin;
            }

            if (App.Current.Properties["idAdmin"] is int ManId1)
            {
                AdminId = ManId1;
            }

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
                    orderHistoryQuery = orderHistoryQuery.Where(o => o.idUser == AdminId);
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
            var selectedOrder = OrdersDataGrid.SelectedItem as OrderHistoryViewModel;

            if (selectedOrder != null)
            {
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

                var orderToUpdate = AppConnect.DarkAndDarkBD.Orders
                    .FirstOrDefault(o => o.idOrder == selectedOrder.idOrder);

                if (orderToUpdate != null)
                {
                    orderToUpdate.idStatus = selectedOrder.idStatus;
                    AppConnect.DarkAndDarkBD.SaveChanges();
                    MessageBox.Show("Статус заказа успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadOrderHistory();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as OrderHistoryViewModel;

            if (selectedOrder != null)
            {
                var confirmation = MessageBox.Show(
                    $"Вы уверены, что хотите удалить заказ ID {selectedOrder.idOrder}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmation == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var transaction = AppConnect.DarkAndDarkBD.Database.BeginTransaction())
                        {
                            var orderItemsToDelete = AppConnect.DarkAndDarkBD.OrderItems
                                .Where(oi => oi.idOrder == selectedOrder.idOrder)
                                .ToList();

                            AppConnect.DarkAndDarkBD.OrderItems.RemoveRange(orderItemsToDelete);

                            var orderToDelete = AppConnect.DarkAndDarkBD.Orders
                                .FirstOrDefault(o => o.idOrder == selectedOrder.idOrder);

                            if (orderToDelete != null)
                            {
                                AppConnect.DarkAndDarkBD.Orders.Remove(orderToDelete);
                            }

                            AppConnect.DarkAndDarkBD.SaveChanges();
                            transaction.Commit();

                            MessageBox.Show("Заказ успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadOrderHistory();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //Контекст юзера 
        private void AdminTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            new AdminWindow().Show();
            this.Close();
        }

        private void MenuItem_Orders_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Корзины.");
            new AdminCartWindow().Show();
            this.Close();
        }
        private void MenuItem_OrderHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Управление заказами.");
            new AdminOrderHistroyWindow().Show();
            this.Close();
        }
        private void MenuItem_Users_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Управление заказами.");
            new AdminUsersWindow().Show();
            this.Close();
        }
        private void MenuItem_Tables_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Управление заказами.");
            new AdminAllTables().Show();
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
