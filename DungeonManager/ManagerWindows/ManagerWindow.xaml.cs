using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private ObservableCollection<CharacterViewModel> Characters;
        private ObservableCollection<CharacterViewModel> FilteredCharacters;
        private int ManId {  get; set; }
        private string Login { get; set; }

        public ManagerWindow()
        {
            InitializeComponent();
            LoadCharacters();

            if (App.Current.Properties["LoginManager"] is string ManLogin)
            {
                Login = ManLogin;
            }

            if (App.Current.Properties["idManager"] is int ManId1)
            {
                ManId = ManId1;
            }
            MessageBox.Show($"Ошибка загрузки данных: {Login + ManId}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            DataContext = this;
        }

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
                        idCharacter = temp.c.idCharacter,
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
            MessageBox.Show($"id - {ManId}", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);


            if (CharactersListView.SelectedItem is CharacterViewModel selectedCharacter)
            {
                MessageBox.Show($"id Character - {selectedCharacter.idCharacter}", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                try
                {
                    if (ManId == 0)
                    {
                        MessageBox.Show("Пользователь не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var existingCartItem = AppConnect.DarkAndDarkBD.Cart
                        .FirstOrDefault(c => c.idUser == ManId && c.idCharacter == selectedCharacter.idCharacter);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += 1;
                        MessageBox.Show("Вы увеличили кол-во данного персонажа.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var newCartItem = new DungeonManager.Model.Cart
                        {
                            idUser = ManId,
                            idCharacter = selectedCharacter.idCharacter,
                            Quantity = 1
                        };

                        AppConnect.DarkAndDarkBD.Cart.Add(newCartItem);
                    }

                    AppConnect.DarkAndDarkBD.SaveChanges();

                    MessageBox.Show($"{selectedCharacter.CharacterName} добавлен в корзину.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите персонажа для добавления в корзину.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CharactersListView.SelectedItem is CharacterViewModel selectedCharacter)
            {
                var characterToEdit = AppConnect.DarkAndDarkBD.Characters
                    .FirstOrDefault(c => c.idCharacter == selectedCharacter.idCharacter);

                if (characterToEdit != null)
                {
                    var editWindow = new EditCharacterWindow(characterToEdit);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadCharacters();
                    }
                }
                else
                {
                    MessageBox.Show("Персонаж не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите персонажа для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            var addWindow = new AddCharacterWindow();

            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    var characterName = addWindow.CharacterNameTextBox.Text;
                    var selectedClass = addWindow.ClassComboBox.SelectedItem as DungeonManager.Model.Classes;
                    var selectedPerks = addWindow.PerksComboBox.SelectedItem as DungeonManager.Model.Perks;
                    var selectedSkill = addWindow.SkillComboBox.SelectedItem as DungeonManager.Model.Skills;
                    var selectedStats = addWindow.StatsComboBox.SelectedValue as DungeonManager.Model.Stats;
                    var priceText = addWindow.PriceTextBox.Text;
                    var imageUrl = addWindow.ImageUrlTextBox.Text;

                    if (string.IsNullOrWhiteSpace(characterName) ||
                        selectedClass == null || selectedSkill == null || selectedStats == null ||
                        !decimal.TryParse(priceText, out var price))
                    {
                        MessageBox.Show("Некоторые поля заполнены некорректно. Проверьте введённые данные.",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var newEntity = new DungeonManager.Model.Characters
                    {
                        CharacterName = characterName,
                        idClass = selectedClass.idClass,
                        idPerks = selectedPerks.idPerk,
                        idSkills = selectedSkill.idSkill,
                        idStats = selectedStats.idStat,
                        Price = price,
                        ImageURL = string.IsNullOrWhiteSpace(imageUrl) ? @"\Images\default.jpg" : imageUrl
                    };

                    AppConnect.DarkAndDarkBD.Characters.Add(newEntity);
                    AppConnect.DarkAndDarkBD.SaveChanges();

                    LoadCharacters();
                    MessageBox.Show("Новый персонаж успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении нового персонажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void UpdateCharacterList()
        {
            using (var db = AppConnect.DarkAndDarkBD)
            {
                CharactersListView.ItemsSource = db.Characters.ToList();
            }
        }


    }

    public class CharacterViewModel
    {
        public int idCharacter { get; set; }
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
        public int idCart { get; set; }
        public int idUser { get; set; }
        public int idCharacter { get; set; }
        public int Quantity { get; set; }
    }

}

