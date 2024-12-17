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
using DungeonManager.AuthUsersWindows;
using DungeonManager.AuthWindows;

namespace DungeonManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //new UserWindow().Show();
            //this.Close();
        }

        private void Login_To_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Переход в окно авторизации.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            new LoginWindow().Show();
            this.Close();
        }

        private void Register_To_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Переход в окно регистрации.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            new RegistrationWindow().Show();
            this.Close();
        }
    }
}
