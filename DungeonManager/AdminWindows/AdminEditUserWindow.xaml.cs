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
using System.Windows.Shapes;
using DungeonManager.ApplicationData;
using DungeonManager.Model;

namespace DungeonManager.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для AdminEditUserWindow.xaml
    /// </summary>
    public partial class AdminEditUserWindow : Window
    {
        private int userId; 

        public AdminEditUserWindow(int idUser)
        {
            InitializeComponent();
            userId = idUser;
            LoadRoles();
            LoadUserData();
        }

        private void LoadRoles()
        {
            try
            {
                var roles = AppConnect.DarkAndDarkBD.Roles.ToList();

                RoleComboBox.ItemsSource = roles;
                RoleComboBox.DisplayMemberPath = "RoleName";
                RoleComboBox.SelectedValuePath = "idRole";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке ролей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUserData()
        {
            try
            {
                var user = AppConnect.DarkAndDarkBD.Users.FirstOrDefault(u => u.idUser == userId);

                if (user != null)
                {
                    LoginTextBox.Text = user.Login;
                    PasswordTextBox.Text = user.Password;
                    EmailTextBox.Text = user.Email;

                    RoleComboBox.SelectedValue = user.idRole;
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = AppConnect.DarkAndDarkBD.Users.FirstOrDefault(u => u.idUser == userId);

                if (user != null)
                {
                    user.Login = LoginTextBox.Text;
                    user.Password = PasswordTextBox.Text;
                    user.Email = EmailTextBox.Text;

                    var selectedRole = (Roles)RoleComboBox.SelectedItem;

                    user.idRole = selectedRole.idRole;

                    AppConnect.DarkAndDarkBD.SaveChanges();
                    MessageBox.Show("Данные пользователя успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}