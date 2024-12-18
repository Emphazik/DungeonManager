using System;
using System.Linq;
using System.Windows;
using DungeonManager.ApplicationData;

namespace DungeonManager.ManagerWindows
{
    public partial class EditCharacterWindow : Window
    {
        private readonly DungeonManager.Model.Characters _characterToEdit;

        public EditCharacterWindow(DungeonManager.Model.Characters character)
        {
            InitializeComponent();
            _characterToEdit = character;
            LoadData();
        }

        private void LoadData()
        {
            CharacterNameTextBox.Text = _characterToEdit.CharacterName;
            PriceTextBox.Text = _characterToEdit.Price.ToString();
            ImageUrlTextBox.Text = _characterToEdit.ImageURL;

            ClassComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Classes.ToList();
            ClassComboBox.DisplayMemberPath = "NameClass";
            ClassComboBox.SelectedValuePath = "idClass";
            ClassComboBox.SelectedValue = _characterToEdit.idClass;

            PerksComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Perks.ToList();
            PerksComboBox.DisplayMemberPath = "NamePerk";
            PerksComboBox.SelectedValuePath = "idPerk";
            PerksComboBox.SelectedValue = _characterToEdit.idPerks;

            SkillComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Skills.ToList();
            SkillComboBox.DisplayMemberPath = "NameSkill";
            SkillComboBox.SelectedValuePath = "idSkill";
            SkillComboBox.SelectedValue = _characterToEdit.idSkills;

            StatsComboBox.ItemsSource = AppConnect.DarkAndDarkBD.Stats.ToList();
            StatsComboBox.SelectedValuePath = "idStat";
            StatsComboBox.SelectedValue = _characterToEdit.idStats;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _characterToEdit.CharacterName = CharacterNameTextBox.Text;
                _characterToEdit.Price = decimal.Parse(PriceTextBox.Text);
                _characterToEdit.ImageURL = ImageUrlTextBox.Text;

                _characterToEdit.idClass = (int)ClassComboBox.SelectedValue;
                _characterToEdit.idPerks = (int)PerksComboBox.SelectedValue;
                _characterToEdit.idSkills = (int)SkillComboBox.SelectedValue;
                _characterToEdit.idStats = (int)StatsComboBox.SelectedValue;

                AppConnect.DarkAndDarkBD.SaveChanges();
                MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
