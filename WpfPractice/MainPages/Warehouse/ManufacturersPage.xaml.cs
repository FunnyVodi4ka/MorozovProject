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

namespace WpfPractice.MainPages.Warehouse
{
    /// <summary>
    /// Логика взаимодействия для ManufacturersPage.xaml
    /// </summary>
    public partial class ManufacturersPage : Page
    {
        private Manufacturers currentManufacturer = new Manufacturers();

        public ManufacturersPage()
        {
            InitializeComponent();
            try
            {
                ListViewManufacturers.ItemsSource = SortFilterManufacturers();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        Manufacturers[] SortFilterManufacturers()
        {
            List<Manufacturers> categories = AppConnect.ModelDB.Manufacturers.ToList();
            var CounterALL = categories;
            if (SearchManufacturer.Text != null)
            {
                categories = categories.Where(x => x.Manufacturer.ToLower().Contains(SearchManufacturer.Text.ToLower())).ToList();
                categories = categories.OrderBy(x => x.Manufacturer).ToList();
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
            ListViewManufacturers.ItemsSource = SortFilterManufacturers();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentRow = ListViewManufacturers.SelectedItems.Cast<Manufacturers>().ToList().ElementAt(0);
                if (MessageBox.Show("Вы уверены, что хотите удалить производителя?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AppConnect.ModelDB.Manufacturers.Remove(currentRow);
                    AppConnect.ModelDB.SaveChanges();
                    ListViewManufacturers.ItemsSource = SortFilterManufacturers();
                }
            }
            catch
            {
                MessageBox.Show("Для удаления записи её необходимо выбрать!");
            }
        }

        private void ButtonAddManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var uniqueName = AppConnect.ModelDB.Manufacturers.FirstOrDefault(x => x.Manufacturer == TextBoxAddManufacturer.Text);
            if (TextBoxAddManufacturer.Text.Length >= 2 && TextBoxAddManufacturer.Text.Length <= 150)
            {
                if (uniqueName == null)
                {
                    try
                    {
                        currentManufacturer.Manufacturer = TextBoxAddManufacturer.Text;
                        if (currentManufacturer.ID == 0)
                        {
                            Entities.GetContext().Manufacturers.Add(currentManufacturer);
                        }
                        Entities.GetContext().SaveChanges();
                        AppConnect.ModelDB.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Уведомление",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                        ListViewManufacturers.ItemsSource = SortFilterManufacturers();
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
                    MessageBox.Show("Ошибка: Такой производитель уже существует");
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Название должно содержать от 2 до 150 символов!");
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

        private void SearchManufacturer_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListViewManufacturers.ItemsSource = SortFilterManufacturers();
        }

        private void LbManufacturers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewManufacturers.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Roles.ID == 1)
            {
                currentManufacturer = ListViewManufacturers.SelectedItem as Manufacturers;
                TextAddManufacturer.Text = "Изменение производителя";
                TextBoxAddManufacturer.Text = currentManufacturer.Manufacturer;
                ButtonAddManufacturer.Content = "Изменить";
                ButtonReset.Visibility = Visibility.Visible;
            }
        }

        private void ResetEditRow()
        {
            currentManufacturer = null;
            TextAddManufacturer.Text = "Добавление нового производителя";
            TextBoxAddManufacturer.Text = "";
            ButtonAddManufacturer.Content = "Добавить";
            ButtonReset.Visibility = Visibility.Hidden;
        }
    }
}
