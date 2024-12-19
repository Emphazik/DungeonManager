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

namespace DungeonManager.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для AdminAllTables.xaml
    /// </summary>
    public partial class AdminAllTables : Window
    {
        private int AdminId { get; set; }
        private string LoginAdmin { get; set; }
        public AdminAllTables()
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
            LoadTables();
        }

        private void LoadTables()
        {
            AddTabWithData("Users", AppConnect.DarkAndDarkBD.Users
                .Include("Roles")
                .Select(u => new
                {
                    u.idUser,
                    u.Login,
                    u.Password,
                    u.Email,
                    RoleName = u.Roles != null ? u.Roles.RoleName : "No Role"
                })
                .ToList());

            AddTabWithData("Roles", AppConnect.DarkAndDarkBD.Roles
                .Select(r => new
                {
                    r.idRole,
                    r.RoleName
                })
                .ToList());

            AddTabWithData("Stats", AppConnect.DarkAndDarkBD.Stats
                .Select(s => new
                {
                    s.idStat,
                    s.Health,
                    s.Mana,
                    s.Strength,
                    s.Agility,
                    s.Intelligence
                })
                .ToList());

            AddTabWithData("Skills", AppConnect.DarkAndDarkBD.Skills
                .Select(s => new
                {
                    s.idSkill,
                    s.NameSkill,
                    s.Description
                })
                .ToList());

            AddTabWithData("Characters", AppConnect.DarkAndDarkBD.Characters
            .Include("Stats")
            .Include("Skills")
            .Include("Classes")
            .AsEnumerable()
            .Select(c => new
            {
                c.idCharacter,
                c.CharacterName,
                ClassName = c.Classes != null ? c.Classes.NameClass : "No Class",
                Stats = c.Stats != null
                    ? $"{c.Stats.Health}/{c.Stats.Mana}/{c.Stats.Strength}/{c.Stats.Agility}/{c.Stats.Intelligence}"
                    : "No Stats",
                c.Price
            })
            .ToList());


            AddTabWithData("Perks", AppConnect.DarkAndDarkBD.Perks
                .Select(p => new
                {
                    p.idPerk,
                    p.NamePerk,
                    p.Description
                })
                .ToList());

            AddTabWithData("Orders", AppConnect.DarkAndDarkBD.Orders
                .Include("OrderStatus")
                .Include("Users")
                .Select(o => new
                {
                    o.idOrder,
                    UserName = o.Users != null ? o.Users.Login : "Unknown User",
                    o.OrderDate,
                    Status = o.OrderStatus != null ? o.OrderStatus.StatusName : "No Status"
                })
                .ToList());

            AddTabWithData("OrderItems", AppConnect.DarkAndDarkBD.OrderItems
                .Include("Characters")
                .Select(oi => new
                {
                    oi.idOrderItem,
                    OrderId = oi.idOrder,
                    CharacterName = oi.Characters != null ? oi.Characters.CharacterName : "No Character",
                    oi.Quantity,
                    oi.Price
                })
                .ToList());

            AddTabWithData("OrderStatus", AppConnect.DarkAndDarkBD.OrderStatus
                .Select(os => new
                {
                    os.idStatus,
                    os.StatusName
                })
                .ToList());

            AddTabWithData("Cart", AppConnect.DarkAndDarkBD.Cart
                .Include("Users")
                .Include("Characters")
                .Select(c => new
                {
                    c.idCart,
                    UserName = c.Users != null ? c.Users.Login : "No User",
                    CharacterName = c.Characters != null ? c.Characters.CharacterName : "No Character",
                    c.Quantity
                })
                .ToList());
        }

        private void AddTabWithData(string tableName, IEnumerable<object> data)
        {
            var tabItem = new TabItem
            {
                Header = tableName
            };

            var dataGrid = new DataGrid
            {
                ItemsSource = data,
                AutoGenerateColumns = true,
                IsReadOnly = true,
                Margin = new Thickness(5)
            };

            tabItem.Content = dataGrid;
            TablesTabControl.Items.Add(tabItem);
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
    }
}
