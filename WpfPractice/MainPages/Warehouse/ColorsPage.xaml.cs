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
using WpfPractice.AppConnection;
using Colors = WpfPractice.AppConnection.Colors;

namespace WpfPractice.MainPages.Warehouse
{
    /// <summary>
    /// Логика взаимодействия для ColorsPage.xaml
    /// </summary>
    public partial class ColorsPage : Page
    {
        private Colors currentColor = new Colors();

        public ColorsPage()
        {
            InitializeComponent();
            try
            {
                ListViewColors.ItemsSource = SortFilterColors();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        Colors[] SortFilterColors()
        {
            List<Colors> categories = AppConnect.ModelDB.Colors.ToList();
            var CounterALL = categories;
            if (SearchColor.Text != null)
            {
                categories = categories.Where(x => x.Color.ToLower().Contains(SearchColor.Text.ToLower())).ToList();
                categories = categories.OrderBy(x => x.Color).ToList();
            }

            if (categories.Count != 0)
            {
                TbFinder.Text = "Показано: " + categories.Count + " из " + CounterALL.Count;
            }
            else
            {
                TbFinder.Text = "Не найдено";
            }

            return categories.ToArray();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            ListViewColors.ItemsSource = SortFilterColors();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentRow = ListViewColors.SelectedItems.Cast<Colors>().ToList().ElementAt(0);
                if (MessageBox.Show("Вы уверены, что хотите удалить цвет?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AppConnect.ModelDB.Colors.Remove(currentRow);
                    AppConnect.ModelDB.SaveChanges();
                    ListViewColors.ItemsSource = SortFilterColors();
                }
            }
            catch
            {
                MessageBox.Show("Для удаления записи её необходимо выбрать!");
            }
        }

        private void ButtonAddColor_Click(object sender, RoutedEventArgs e)
        {
            var uniqueName = AppConnect.ModelDB.Colors.FirstOrDefault(x => x.Color == TextBoxAddColor.Text);
            if (TextBoxAddColor.Text.Length >= 2 && TextBoxAddColor.Text.Length <= 50)
            {
                if (uniqueName == null)
                {
                    try
                    {
                        currentColor.Color = TextBoxAddColor.Text;
                        if (currentColor.ID == 0)
                        {
                            Entities.GetContext().Colors.Add(currentColor);
                        }
                        Entities.GetContext().SaveChanges();
                        AppConnect.ModelDB.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Уведомление",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                        ListViewColors.ItemsSource = SortFilterColors();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    ResetEditRow();
                }
                else
                {
                    MessageBox.Show("Ошибка: Такой цвет уже существует");
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Название должно содержать от 2 до 50 символов!");
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetEditRow();
        }

        private void LbColors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewColors.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Roles.ID == 1)
            {
                currentColor = ListViewColors.SelectedItem as Colors;
                TextAddColor.Text = "Изменение цвета";
                TextBoxAddColor.Text = currentColor.Color;
                ButtonAddColor.Content = "Изменить";
                ButtonReset.Visibility = Visibility.Visible;
            }
        }

        private void SearchColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListViewColors.ItemsSource = SortFilterColors();
        }

        private void ResetEditRow()
        {
            currentColor = null;
            TextAddColor.Text = "Добавление нового цвета";
            TextBoxAddColor.Text = "";
            ButtonAddColor.Content = "Добавить";
            ButtonReset.Visibility = Visibility.Hidden;
        }
    }
}
