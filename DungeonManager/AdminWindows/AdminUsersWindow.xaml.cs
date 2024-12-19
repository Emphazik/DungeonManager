using System;
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

namespace DungeonManager.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для AdminUsersWindow.xaml
    /// </summary>
    public partial class AdminUsersWindow : Window
    {
        public ObservableCollection<UserViewModel> UsersList { get; set; }
        private int AdminId { get; set; }
        private string LoginAdmin { get; set; }
        public AdminUsersWindow()
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
            LoadUsers();
        }
         private void LoadUsers()
         {
            try
            {
                var users = AppConnect.DarkAndDarkBD.Users
                    .Select(user => new UserViewModel
                    {
                        idUser = user.idUser,
                        Login = user.Login,
                        Password = user.Password,
                        Email = user.Email,
                        RoleName = user.Roles != null ? user.Roles.RoleName : "Не назначена"
                    })
                    .ToList();

                UsersList = new ObservableCollection<UserViewModel>(users);
                UsersDataGrid.ItemsSource = UsersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AdminAddUserWindow();
            addUserWindow.ShowDialog();

            LoadUsers();
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as UserViewModel;

            if (selectedUser != null)
            {
                var editUserWindow = new AdminEditUserWindow(selectedUser.idUser);
                editUserWindow.ShowDialog();
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as UserViewModel;
            if (selectedUser != null)
            {
                var confirmation = MessageBox.Show(
                    $"Вы уверены, что хотите удалить пользователя ID {selectedUser.idUser}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmation == MessageBoxResult.Yes)
                {
                    try
                    {
                        var userToDelete = AppConnect.DarkAndDarkBD.Users
                            .FirstOrDefault(u => u.idUser == selectedUser.idUser);

                        if (userToDelete != null)
                        {
                            AppConnect.DarkAndDarkBD.Users.Remove(userToDelete);
                            AppConnect.DarkAndDarkBD.SaveChanges();

                            MessageBox.Show("Пользователь успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadUsers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public class UserViewModel
        {
            public int idUser { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }

            public string Email { get; set; }
            public string RoleName { get; set; }
        }
    }
}
