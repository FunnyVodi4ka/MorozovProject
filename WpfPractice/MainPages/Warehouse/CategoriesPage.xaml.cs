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
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private Categories currentCategory = new Categories();

        public CategoriesPage()
        {
            InitializeComponent();
            try
            {
                ListViewCategories.ItemsSource = SortFilterCategories();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        Categories[] SortFilterCategories()
        {
            List<Categories> categories = AppConnect.ModelDB.Categories.ToList();
            var CounterALL = categories;
            if (SearchCategory.Text != null)
            {
                categories = categories.Where(x => x.Category.ToLower().Contains(SearchCategory.Text.ToLower())).ToList();
                categories = categories.OrderBy(x => x.Category).ToList();
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

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        private void SearchCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListViewCategories.ItemsSource = SortFilterCategories();
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var uniqueName = AppConnect.ModelDB.Categories.FirstOrDefault(x => x.Category == TextBoxAddCategory.Text);
            if (TextBoxAddCategory.Text.Length >= 2 && TextBoxAddCategory.Text.Length <= 100)
            {
                if (uniqueName == null)
                {
                    try
                    {
                        currentCategory.Category = TextBoxAddCategory.Text;
                        if (currentCategory.ID == 0)
                        {
                            Entities.GetContext().Categories.Add(currentCategory);
                        }
                        Entities.GetContext().SaveChanges();
                        AppConnect.ModelDB.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Уведомление",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                        ListViewCategories.ItemsSource = SortFilterCategories();
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
                    MessageBox.Show("Ошибка: Такая категория уже существует");
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Название должно содержать от 2 до 100 символов!");
            }
        }

        private void ResetEditRow()
        {
            currentCategory = null;
            TextAddCategory.Text = "Добавление новой категории";
            TextBoxAddCategory.Text = "";
            ButtonAddCategory.Content = "Добавить";
            ButtonReset.Visibility = Visibility.Hidden;
        }

        private void LbCategories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewCategories.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Roles.ID == 1)
            {
                currentCategory = ListViewCategories.SelectedItem as Categories;
                TextAddCategory.Text = "Изменение категории";
                TextBoxAddCategory.Text = currentCategory.Category;
                ButtonAddCategory.Content = "Изменить";
                ButtonReset.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            ListViewCategories.ItemsSource = SortFilterCategories();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try {
                var currentRow = ListViewCategories.SelectedItems.Cast<Categories>().ToList().ElementAt(0);
                if (MessageBox.Show("Вы уверены, что хотите удалить категорию?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AppConnect.ModelDB.Categories.Remove(currentRow);
                    AppConnect.ModelDB.SaveChanges();
                    ListViewCategories.ItemsSource = SortFilterCategories();
                }
            }
            catch
            {
                MessageBox.Show("Для удаления записи её необходимо выбрать!");
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetEditRow();
        }
    }
}
