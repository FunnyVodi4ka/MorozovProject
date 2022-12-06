using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Page
    {
        private Products currentProduct = new Products();
        private string namePicture = String.Empty;
        bool flagError = false;

        public AddEditProduct(Products product)
        {
            InitializeComponent();

            try
            {
                SetFilterCategories();
                SetFilterManufacturers();
                SetFilterColors();
                if (product != null)
                {
                    currentProduct = product;

                    FindFilterCategoriesId();
                    FindFilterManufacturersId();
                    FindFilterColorsId();
                }
                else
                {
                    CategoryFilter.SelectedIndex = 0;
                    ManufaturerFilter.SelectedIndex = 0;
                    ColorFilter.SelectedIndex = 0;

                    DeleteData.Visibility = Visibility.Hidden;
                    AddData.Content = "Добавить товар";
                }

                DataContext = currentProduct;
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось обработать запуск формы", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetFilterCategories()
        {
            CategoryFilter.Items.Add("Выберите категорию");
            foreach (var category in AppConnect.ModelDB.Categories)
            {
                CategoryFilter.Items.Add(category.Category);
            }
            CategoryFilter.SelectedIndex = 0;
        }

        private void FindFilterCategoriesId()
        {
            var category = AppConnect.ModelDB.Categories.FirstOrDefault(x => x.ID == currentProduct.IDCategory);
            CategoryFilter.SelectedItem = category.Category;
        }

        private void FindFilterCategoriesItem()
        {
            var category = AppConnect.ModelDB.Categories.FirstOrDefault(x => x.Category == CategoryFilter.SelectedItem.ToString());
            if (category == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                currentProduct.IDCategory = category.ID;
            }
        }

        private void SetFilterManufacturers()
        {
            ManufaturerFilter.Items.Add("Выберите производителя");
            foreach (var manufacturer in AppConnect.ModelDB.Manufacturers)
            {
                ManufaturerFilter.Items.Add(manufacturer.Manufacturer);
            }
            ManufaturerFilter.SelectedIndex = 0;
        }

        private void FindFilterManufacturersId()
        {
            var manufacturer = AppConnect.ModelDB.Manufacturers.FirstOrDefault(x => x.ID == currentProduct.IDManufacturer);
            ManufaturerFilter.SelectedItem = manufacturer.Manufacturer;
        }

        private void FindFilterManufacturersItem()
        {
            var manufacturer = AppConnect.ModelDB.Manufacturers.FirstOrDefault(x => x.Manufacturer == ManufaturerFilter.SelectedItem.ToString());
            if (manufacturer == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                currentProduct.IDManufacturer = manufacturer.ID;
            }
        }

        private void SetFilterColors()
        {
            ColorFilter.Items.Add("Выберите цвет");
            foreach (var color in AppConnect.ModelDB.Colors)
            {
                ColorFilter.Items.Add(color.Color);
            }
            ColorFilter.SelectedIndex = 0;
        }

        private void FindFilterColorsId()
        {
            var color = AppConnect.ModelDB.Colors.FirstOrDefault(x => x.ID == currentProduct.IDColor);
            ColorFilter.SelectedItem = color.Color;
        }

        private void FindFilterColorsItem()
        {
            var color = AppConnect.ModelDB.Colors.FirstOrDefault(x => x.Color == ColorFilter.SelectedItem.ToString());
            if (color == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                currentProduct.IDColor = color.ID;
            }
        }

        private bool CheckData()
        {
            bool flagErrorMessage = false;

            if (currentProduct.ID == 0)
            {
                var uniqueName = AppConnect.ModelDB.Products.FirstOrDefault(x => x.Name == TextBoxNameProduct.Text);
                if (uniqueName != null)
                {
                    if (!flagErrorMessage)
                        MessageBox.Show("Ошибка: Такой товар уже существует");
                    flagErrorMessage = true;
                    flagError = true;
                }
            }

            if (TextBoxNameProduct.Text.Length < 4 || TextBoxNameProduct.Text.Length > 150)
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Название должно содержать от 4 до 150 символов!");
                flagErrorMessage = true;
                flagError = true;
            }
            if (CategoryFilter.Text.Length <= 0)
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Выберите категорию!");
                flagErrorMessage = true;
                flagError = true;
            }
            if (ManufaturerFilter.Text.Length <= 0)
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Выберите производителя!");
                flagErrorMessage = true;
                flagError = true;
            }
            if (ColorFilter.Text.Length <= 0)
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Выберите цвет!");
                flagErrorMessage = true;
                flagError = true;
            }
            if (!CheckCorrectIntValue(TextBoxCost.Text, 1))
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Некорректная цена!");
                flagErrorMessage = true;
                flagError = true;
            }
            if (!CheckCorrectIntValue(TextBoxInStock.Text))
            {
                if (!flagErrorMessage)
                    MessageBox.Show("Ошибка: Некорректное количество!");
                flagErrorMessage = true;
                flagError = true;
            }

            return flagError;
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

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                try
                {
                    currentProduct.Name = TextBoxNameProduct.Text;
                    FindFilterCategoriesItem();
                    FindFilterManufacturersItem();
                    FindFilterColorsItem();
                    currentProduct.Cost = Int32.Parse(TextBoxCost.Text);
                    currentProduct.InStock = Int32.Parse(TextBoxInStock.Text);
                    if (currentProduct.ID == 0)
                    {
                        Entities.GetContext().Products.Add(currentProduct);
                    }

                    Entities.GetContext().SaveChanges();

                    AppConnect.ModelDB.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                    AppFrame.fraimMain.GoBack();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить товар?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                AppConnect.ModelDB.Products.Remove(currentProduct);
                AppConnect.ModelDB.SaveChanges();
                NavigationService.GoBack();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        private void AddPicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.ShowDialog();

                string directory;
                directory = dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\'), dialog.FileName.Length - dialog.FileName.Substring(0, dialog.FileName.LastIndexOf('\\')).Length);

                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\ProductImages\\" + directory))
                {
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\ProductImages\\" + directory);
                }
                Random rnd = new Random();
                File.Copy(dialog.FileName, System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\ProductImages\\" + directory);
                currentProduct.ProductImage = dialog.SafeFileName;
                AppConnect.ModelDB.SaveChanges();
                if (currentProduct != null && currentProduct.ID != 0)
                {
                    DataContext = null;
                    DataContext = currentProduct;
                }
                namePicture = currentProduct.ProductImage;
                CurrentImage.Text = namePicture;
            }
            catch(Exception Ex)
            {
            }
        }

        private void TextBoxCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }

        private void TextBoxInStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }
    }
}

