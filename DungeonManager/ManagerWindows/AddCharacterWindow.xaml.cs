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

namespace DungeonManager.ManagerWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCharacterWindow.xaml
    /// </summary>
    public partial class AddCharacterWindow : Window
    {
        public AddCharacterWindow()
        {
            InitializeComponent();

            ClassComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Classes.ToList();
            ClassComboBox.DisplayMemberPath = "NameClass";

            PerksComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Perks.ToList();
            PerksComboBox.DisplayMemberPath = "NamePerk";

            SkillComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Skills.ToList();
            SkillComboBox.DisplayMemberPath = "NameSkill";

            var statsList = AppConnect.DarkAndDarkBD.Stats.ToList()
        .Select(stat => new
        {
            Stat = stat,
            Display = $"{stat.Health} / {stat.Mana} / {stat.Strength} / {stat.Intelligence}"
        }).ToList();

            StatsComboBox.ItemsSource = statsList;
            StatsComboBox.DisplayMemberPath = "Display";
            StatsComboBox.SelectedValuePath = "Stat";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CharacterNameTextBox.Text) ||
                ClassComboBox.SelectedItem == null ||
                PerksComboBox.SelectedItem == null ||
                SkillComboBox.SelectedItem == null ||
                StatsComboBox.SelectedItem == null ||
                !decimal.TryParse(PriceTextBox.Text, out _))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //var selectedStat = (StatsComboBox.SelectedValue as Stats);

            //if (selectedStat != null)
            //{
            //    MessageBox.Show($"Вы выбрали характеристики: " +
            //                    $"Health: {selectedStat.Health}, " +
            //                    $"Mana: {selectedStat.Mana}, " +
            //                    $"Strength: {selectedStat.Strength}, " +
            //                    $"Intelligence: {selectedStat.Intelligence}");
            //}

            DialogResult = true;
            Close();
        }
        private void StatsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatsComboBox.SelectedItem != null)
            {
                var selectedStat = (dynamic)StatsComboBox.SelectedItem; // Используем dynamic для доступа к свойству "Stat"
                var statDetails = selectedStat.Stat;

                MessageBox.Show(
                    $"Выбрана стата:\n" +
                    $"- Health: {statDetails.Health}\n" +
                    $"- Mana: {statDetails.Mana}\n" +
                    $"- Strength: {statDetails.Strength}\n" +
                    $"- Intelligence: {statDetails.Intelligence}",
                    "Информация о статах",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}