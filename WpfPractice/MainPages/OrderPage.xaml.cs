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
using WpfPractice.MainPages.Warehouse;

namespace WpfPractice.MainPages
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        
        public OrderPage()
        {
            InitializeComponent();
            try
            {
                ShowAdminFunctions();
                SetSortItems();

                LbOrders.ItemsSource = FindOrders();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        Order[] FindOrders()
        {
            List<Order> order = AppConnect.ModelDB.Order.ToList();

            var orderAll = order;

            if (TbFinder.Text != null)
            {
                order = order.Where(x => x.Client.Surname.ToLower().Contains(TbFinder.Text.ToLower()) ||
                                        x.Client.Name.ToLower().Contains(TbFinder.Text.ToLower()) ||
                                        x.Client.Secondname.ToLower().Contains(TbFinder.Text.ToLower()) ||
                                        x.Technic.ToLower().Contains(TbFinder.Text.ToLower()) ||
                                        x.Description.ToLower().Contains(TbFinder.Text.ToLower())).ToList();
            }


            if (TbDatePick.Text != null)
            {
                order = order.Where(x => x.DateOfStart.Date.ToString().ToLower().Contains(TbDatePick.Text.ToString())).ToList();
            }

            if (CbSort.SelectedIndex > 0)
            {
                order = order.Where(x => x.TypeTechnic.Name == CbSort.SelectedItem.ToString()).ToList();
            }

            if (order.Count > 0)
            {
                TbFindeFurniture.Text = "Найдено " + order.Count.ToString() + " из " + orderAll.Count().ToString();
            }
            else
            {
                TbFindeFurniture.Text = "Элементы не найдены";
            }

            return order.ToArray();
        }

        private void SetSortItems()
        {
            CbSort.Items.Add("< Тип техники >");
            foreach (var type in AppConnect.ModelDB.TypeTechnic)
            {
                CbSort.Items.Add(type.Name);
            }
            CbSort.SelectedIndex = 0;
        }

        private void ShowAdminFunctions()
        {
            if (SelectedUser.user == null || SelectedUser.user.IDRole != 1)
            {
                BtnChangeToUserTable.Visibility = Visibility.Hidden;
            }
        }

        private void TbDatePick_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbOrders.ItemsSource = FindOrders();
        }

        private void BtnAddFurniture_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOrder((sender as Button).DataContext as Order)) ;
        }

        private void BtnChangeToUserTable_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new AdminShowUsersPage());
        }

        private void TbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbOrders.ItemsSource = FindOrders();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbOrders.ItemsSource = FindOrders();
        }

        private void LbOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbOrders.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Roles.ID == 1)
            {
                Order order = LbOrders.SelectedItem as Order;
                Client client = LbOrders.SelectedItem as Client;

                NavigationService.Navigate(new AddEditOrder(order));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            LbOrders.ItemsSource = FindOrders();
        }

        private void BtnChangeToWarehouse_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new WarehousePage());
        }
    }
}
