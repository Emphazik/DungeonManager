using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DungeonManager.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для AdminAddUserWindow.xaml
    /// </summary>
    public partial class AdminAddUserWindow : Window
    {
        public AdminAddUserWindow()
        {
            InitializeComponent();
            LoadRoles();
        }
        private void LoadRoles()
        {
            var roles = AppConnect.DarkAndDarkBD.Roles.ToList();
            RoleComboBox.ItemsSource = roles;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text;
                var password = PasswordTextBox.Text;
                var email = EmailTextBox.Text;
                var selectedRole = RoleComboBox.SelectedItem as Roles;
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || selectedRole == null)
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (password.Length < 6 || !password.Any(char.IsDigit))
                {
                    MessageBox.Show("Пароль должен содержать минимум 6 символов, включая хотя бы одну цифру.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Введите корректный Email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new Users
                {
                    Login = login,
                    Password = password,
                    Email = email,
                    idRole = selectedRole.idRole
                };

                AppConnect.DarkAndDarkBD.Users.Add(newUser);
                AppConnect.DarkAndDarkBD.SaveChanges();

                MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
