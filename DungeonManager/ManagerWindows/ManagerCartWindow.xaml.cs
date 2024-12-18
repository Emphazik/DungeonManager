﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DungeonManager.Model;
using static DungeonManager.AuthUsersWindows.CartViewWindow;

namespace DungeonManager.ManagerWindows
{
    /// <summary>
    /// Логика взаимодействия для ManagerCartWindow.xaml
    /// </summary>
    public partial class ManagerCartWindow : Window
    {
        private ObservableCollection<CartItemViewModel> CartItems;

        private int ManId { get; set; }
        private string Login { get; set; }
        public ManagerCartWindow()
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
            LoadCart();
        }

        private void LoadCart()
        {
            try
            {
                CartItems = new ObservableCollection<CartItemViewModel>(
                    AppConnect.DarkAndDarkBD.Cart
                        .Where(cart => cart.idUser == ManId)
                        .Join(AppConnect.DarkAndDarkBD.Characters,
                              cart => cart.idCharacter,
                              character => character.idCharacter,
                              (cart, character) => new CartItemViewModel
                              {
                                  idCart = cart.idCart,
                                  idUser = cart.idUser,
                                  idCharacter = cart.idCharacter,
                                  CharacterName = character.CharacterName,
                                  Quantity = (int)cart.Quantity,
                                  Price = character.Price * (int)cart.Quantity
                              }));

                CartListView.ItemsSource = CartItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки корзины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CartItemViewModel selectedCartItem)
            {
                try
                {
                    var cartItem = AppConnect.DarkAndDarkBD.Cart
                        .FirstOrDefault(c => c.idCart == selectedCartItem.idCart);

                    if (cartItem != null)
                    {
                        AppConnect.DarkAndDarkBD.Cart.Remove(cartItem);
                        AppConnect.DarkAndDarkBD.SaveChanges();
                        MessageBox.Show("Товар удален из корзины.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCart();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления из корзины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = AppConnect.DarkAndDarkBD.Cart.ToList();
            foreach (var item in itemsToRemove)
                AppConnect.DarkAndDarkBD.Cart.Remove(item);

            AppConnect.DarkAndDarkBD.SaveChanges();
            RefreshCart();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ManId == 0)
                {
                    MessageBox.Show("Пользователь не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var cartItems = AppConnect.DarkAndDarkBD.Cart
                    .Where(c => c.idUser == ManId)
                    .Join(AppConnect.DarkAndDarkBD.Characters,
                          cart => cart.idCharacter,
                          character => character.idCharacter,
                          (cart, character) => new
                          {
                              cart.idCharacter,
                              cart.Quantity,
                              character.Price
                          })
                    .ToList();

                if (!cartItems.Any())
                {
                    MessageBox.Show("Корзина пуста. Добавьте товары перед оформлением покупки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newOrder = new DungeonManager.Model.Orders
                {
                    idUser = ManId,
                    OrderDate = DateTime.Now,
                    idStatus = 1 // Статус "В ожидании" по умолчанию
                };

                AppConnect.DarkAndDarkBD.Orders.Add(newOrder);
                AppConnect.DarkAndDarkBD.SaveChanges();

                foreach (var item in cartItems)
                {
                    var orderItem = new DungeonManager.Model.OrderItems
                    {
                        idOrder = newOrder.idOrder,
                        idCharacter = item.idCharacter,
                        Quantity = (int)item.Quantity,
                        Price = item.Price * (int)item.Quantity
                    };

                    AppConnect.DarkAndDarkBD.OrderItems.Add(orderItem);
                }

                var userCart = AppConnect.DarkAndDarkBD.Cart.Where(c => c.idUser == ManId);
                AppConnect.DarkAndDarkBD.Cart.RemoveRange(userCart);

                AppConnect.DarkAndDarkBD.SaveChanges();

                MessageBox.Show("Покупка успешно оформлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении покупки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RefreshCart()
        {
            CartListView.ItemsSource = AppConnect.DarkAndDarkBD.Cart.ToList();
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
    }


}
