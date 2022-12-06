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
    /// Логика взаимодействия для WarehousePage.xaml
    /// </summary>
    public partial class WarehousePage : Page
    {
        public WarehousePage()
        {
            InitializeComponent();
            try
            {
                SetSortByProducts();
                SetFilterByProducts();

                ListViewProducts.ItemsSource = SortFilterProducts();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
                bt3.Visibility = Visibility.Hidden;
            }
        }*/

        private void SetSortByProducts()
        {
            ProductSort.Items.Add("Без сортировки");
            ProductSort.Items.Add("Стоимость по возрастанию");
            ProductSort.Items.Add("Стоимость по убыванию");

            ProductSort.SelectedIndex = 0;
        }

        private void SetFilterByProducts()
        {
            ProductFilter.Items.Add("Все категории");
            foreach (var category in AppConnect.ModelDB.Categories)
            {
                ProductFilter.Items.Add(category.Category);
            }

            ProductFilter.SelectedIndex = 0;
        }

        Products[] SortFilterProducts()
        {
            List<Products> products = AppConnect.ModelDB.Products.ToList();
            var CounterALL = products;
            if (SearchProduct.Text != null)
            {
                products = products.Where(x => x.Name.ToLower().Contains(SearchProduct.Text.ToLower())).ToList();

                switch (ProductSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.Cost).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }

            if (ProductFilter.SelectedIndex > 0)
            {
                products = products.Where(x => x.Categories.Category == ProductFilter.SelectedItem.ToString()).ToList();
            }

            if (products.Count != 0)
            {
                TbFindeProduct.Text = "Показано товаров: " + products.Count + " из " + CounterALL.Count;
            }
            else
            {
                TbFindeProduct.Text = "Не найдено";
            }

            return products.ToArray();
        }

        private void SearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListViewProducts.ItemsSource = SortFilterProducts();
        }

        private void ProductSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewProducts.ItemsSource = SortFilterProducts();
        }

        private void ProductFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewProducts.ItemsSource = SortFilterProducts();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            //AppFrame.fraimMain.Navigate(new OrderPage());
            AppFrame.fraimMain.GoBack();
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProduct((sender as Button).DataContext as Products));
        }

        private void LbProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewProducts.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Roles.ID == 1)
            {
                Products product = ListViewProducts.SelectedItem as Products;

                NavigationService.Navigate(new AddEditProduct(product));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            ListViewProducts.ItemsSource = SortFilterProducts();
        }

        private void ButtonToColors_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new ColorsPage());
        }

        private void ButtonToManufacturers_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new ManufacturersPage());
        }

        private void ButtonToCategories_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new CategoriesPage());
        }

        private void ButtonToSales_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new SalesPage());
        }
    }
}
