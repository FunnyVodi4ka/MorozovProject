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
    /// Логика взаимодействия для SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        private Sales currentSale = new Sales();
        private Products currentProduct = new Products();

        public SalesPage()
        {
            InitializeComponent();
            try
            {
                SetFilterProducts();
                ListViewSales.ItemsSource = SortFilterSales();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetFilterProducts()
        {
            ProductList.Items.Add("Выберите товар");
            foreach (var products in AppConnect.ModelDB.Products)
            {
                ProductList.Items.Add(products.Name);
            }
            ProductList.SelectedIndex = 0;
        }

        Sales[] SortFilterSales()
        {
            List<Sales> sales = AppConnect.ModelDB.Sales.ToList();
            var CounterALL = sales;
            sales = sales.OrderByDescending(x => x.DateTime).ToList();

            if (sales.Count != 0)
            {
                TbFinder.Text = "Показано: " + sales.Count + " из " + CounterALL.Count;
            }
            else
            {
                TbFinder.Text = "Не найдено";
            }

            return sales.ToArray();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListViewSales.ItemsSource = SortFilterSales();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentRow = ListViewSales.SelectedItems.Cast<Sales>().ToList().ElementAt(0);
                if (MessageBox.Show("Вы уверены, что хотите удалить продажу? Внимание! Удаление должно быть согласовано с директором!", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AppConnect.ModelDB.Sales.Remove(currentRow);
                    AppConnect.ModelDB.SaveChanges();

                    RefundProduct(currentRow.IDProduct, currentRow.Quantity);

                    ListViewSales.ItemsSource = SortFilterSales();
                }
            }
            catch
            {
                MessageBox.Show("Для удаления записи её необходимо выбрать!");
            }
        }

        private void SellProduct(int IDProduct, int quantity)
        {
            var product = AppConnect.ModelDB.Products.FirstOrDefault(x => x.ID == IDProduct);
            currentProduct = product;
            currentProduct.InStock = product.InStock - quantity;
            Entities.GetContext().SaveChanges();
            AppConnect.ModelDB.SaveChanges();
        }

        private void RefundProduct(int IDProduct, int quantity)
        {
            var product = AppConnect.ModelDB.Products.FirstOrDefault(x => x.ID == IDProduct);
            currentProduct = product;
            currentProduct.InStock = product.InStock + quantity;
            Entities.GetContext().SaveChanges();
            AppConnect.ModelDB.SaveChanges();
        }

        private void ButtonAddSale_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCorrectIntValue(TextBoxQuantity.Text))
            {
                var product = AppConnect.ModelDB.Products.FirstOrDefault(x => x.Name == ProductList.SelectedItem.ToString());
                if (product != null)
                {
                    if (product.InStock - Int32.Parse(TextBoxQuantity.Text) >= 0)
                    {
                        try
                        {

                            currentSale.IDUser = SelectedUser.user.ID;
                            currentSale.IDProduct = product.ID;
                            currentSale.Quantity = Int32.Parse(TextBoxQuantity.Text);
                            currentSale.DateTime = DateTime.Now;

                            Entities.GetContext().Sales.Add(currentSale);
                            Entities.GetContext().SaveChanges();
                            AppConnect.ModelDB.SaveChanges();

                            SellProduct(currentSale.IDProduct, currentSale.Quantity);

                            MessageBox.Show("Продажа успешно оформлена!", "Уведомление",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                            ListViewSales.ItemsSource = SortFilterSales();
                            currentSale = null;
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно товара на складе!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Некорректное количество!");
            }
        }

        private bool CheckCorrectIntValue(string value, int lowerBound = 0)
        {
            try
            {
                if (Int32.Parse(value) >= lowerBound)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        private void TextBoxQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }
    }
}
