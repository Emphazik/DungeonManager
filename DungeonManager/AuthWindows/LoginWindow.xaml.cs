using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using DungeonManager.ApplicationData;
using DungeonManager.AuthUsersWindows;
using DungeonManager.Model;

namespace DungeonManager.AuthWindows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            AppConnect.DarkAndDarkBD = new DungeonManagerEntities();
            LoginTextBox.Text = "Vladusha";
            PasswordBox.Password = "Vladusha123";

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.DarkAndDarkBD == null)
            {
                MessageBox.Show("Ошибка подключения к базе данных. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var user = AppConnect.DarkAndDarkBD.Users
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Roles == null)
                {
                    MessageBox.Show("Роль пользователя не определена. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Console.WriteLine($"Успешный вход: Пользователь {user.Login}, Роль: {user.Roles.RoleName}.");

                switch (user.idRole)
                {
                    case 1:
                        MessageBox.Show($"Добро пожаловать, {user.Login}! Ваша роль: Пользователь.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.Current.Properties["idUser"] = user.idUser;
                        App.Current.Properties["LoginUser"] = user.Login;
                        new UserWindow().Show();
                        this.Close();
                        break;

                    case 2:
                        MessageBox.Show($"Добро пожаловать, {user.Login}! Ваша роль: Менеджер.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        break;

                    case 3:
                        MessageBox.Show($"Добро пожаловать, {user.Login}! Ваша роль: Администратор.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        break;

                    default:
                        MessageBox.Show("Роль пользователя не определена. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
                MessageBox.Show("Произошла ошибка при выполнении авторизации. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Возвращение к главному окну.","Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            new MainWindow().Show();
            this.Close();
        }

    }
}
