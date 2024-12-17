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
using DungeonManager.Model;

namespace DungeonManager.AuthUsersWindows
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private ObservableCollection<CharacterViewModel> Characters;
        private ObservableCollection<CharacterViewModel> FilteredCharacters;
        private int UserId { get; set; }
        public string UserLogin { get; set; }
        public UserWindow(/*int userId, string userLogin*/)
        {
            InitializeComponent();
            LoadCharacters();

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
        }

        //public UserWindow()
        //{
        //    InitializeComponent();
        //    LoadCharacters();
        //    DataContext = this;


        //}

        private void LoadCharacters()
        {
            try
            {
                Characters = new ObservableCollection<CharacterViewModel>(
                    AppConnect.DarkAndDarkBD.Characters
                    .Join(AppConnect.DarkAndDarkBD.Classes, c => c.idClass, cl => cl.idClass, (c, cl) => new { c, cl })
                    .Join(AppConnect.DarkAndDarkBD.Perks, temp => temp.c.idPerks, p => p.idPerk, (temp, p) => new { temp.c, temp.cl, p })
                    .Join(AppConnect.DarkAndDarkBD.Skills, temp => temp.c.idSkills, s => s.idSkill, (temp, s) => new { temp.c, temp.cl, temp.p, s })
                    .Join(AppConnect.DarkAndDarkBD.Stats, temp => temp.c.idStats, st => st.idStat, (temp, st) => new CharacterViewModel
                    {
                        CharacterName = temp.c.CharacterName,
                        ClassName = temp.cl.NameClass,
                        Price = temp.c.Price,
                        ImageURL = temp.c.ImageURL,
                        Perks = temp.p.NamePerk,
                        Skills = temp.s.NameSkill,
                        Health = (int)st.Health,
                        Mana = (int)st.Mana,
                        Strength = (int)st.Strength,
                        Intelligence = (int)st.Intelligence
                    }));

                FilteredCharacters = new ObservableCollection<CharacterViewModel>(Characters);
                CharactersListView.ItemsSource = FilteredCharacters;
                UpdateRecordsCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRecordsCount()
        {
            RecordsCountText.Text = $"Найдено записей: {FilteredCharacters.Count}";
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Введите имя...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Введите имя...";
                SearchBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySorting();
        }

        private void ApplyFilters()
        {
            if (Characters == null) return;

            string searchText = SearchBox.Text?.ToLower();
            if (searchText == null || searchText == "введите имя...") searchText = "";

            string selectedClass = null;
            if (FilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedClass = selectedItem.Content.ToString();
            }

            FilteredCharacters = new ObservableCollection<CharacterViewModel>(Characters.Where(c =>
                (string.IsNullOrEmpty(searchText) || c.CharacterName.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(selectedClass) || selectedClass == "Все классы" || c.ClassName == selectedClass)
            ));

            if (FilteredCharacters != null)
            {
                ApplySorting();
                CharactersListView.ItemsSource = FilteredCharacters;
                UpdateRecordsCount();
            }
            else
            {
                MessageBox.Show("Ошибка фильтрации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ApplySorting()
        {
            if (FilteredCharacters == null || !FilteredCharacters.Any()) return;

            string selectedSort = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            IEnumerable<CharacterViewModel> sorted;

            switch (selectedSort)
            {
                case "По имени":
                    sorted = FilteredCharacters.OrderBy(c => c.CharacterName);
                    break;
                case "По цене (возрастание)":
                    sorted = FilteredCharacters.OrderBy(c => c.Price);
                    break;
                case "По цене (убывание)":
                    sorted = FilteredCharacters.OrderByDescending(c => c.Price);
                    break;
                default:
                    sorted = FilteredCharacters;
                    break;
            }

            FilteredCharacters = new ObservableCollection<CharacterViewModel>(sorted);
            CharactersListView.ItemsSource = FilteredCharacters;
        }


        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SbrosButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Введите имя...";
            SearchBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);

            FilterComboBox.SelectedIndex = 0;

            SortComboBox.SelectedIndex = 0;

            ApplyFilters();
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
            new UserWindow().Show();
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

    public class CharacterViewModel
    {
        public string CharacterName { get; set; }
        public string ClassName { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public string Perks { get; set; }
        public string Skills { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
    }

    public class Cart
    {
        public int IdCart { get; set; }
        public int IdOrder { get; set; }
        public int IdCharacter { get; set; }
        public decimal Price { get; set; }
    }

}
