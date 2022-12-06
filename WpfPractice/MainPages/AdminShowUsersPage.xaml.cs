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
    /// Логика взаимодействия для AdminShowUsersPage.xaml
    /// </summary>
    public partial class AdminShowUsersPage : Page
    {
        public AdminShowUsersPage()
        {
            InitializeComponent();
            try
            {
                LbUser.ItemsSource = FindUser();
            }
            catch
            {
                MessageBox.Show("Не удалось", "кринж", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        User[] FindUser()
        {
            List<User> users = AppConnect.ModelDB.User.ToList();

            var userAll = users;

            if (TbFinder.Text != null)
            {
                users = users.
                    Where(x => x.Login.ToLower().Contains(TbFinder.Text.ToLower())
                            ).ToList();
            }

            if (users.Count > 0)
            {
                TbFindeUsers.Text = "Найдено " + users.Count.ToString() + " из " + userAll.Count().ToString();
            }
            else
            {
                TbFindeUsers.Text = "Элементы не найдены";
            }

            return users.ToArray();
        }

        private void TbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbUser.ItemsSource = FindUser();
        }

        private void BtnChangeToUserTable_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new OrderPage());
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new AdminAddPage((sender as Button).DataContext as User));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            LbUser.ItemsSource = FindUser();
        }

        private void LbUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbUser.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.IDRole == 1)
            {
                User user = LbUser.SelectedItem as User;
                NavigationService.Navigate(new AdminAddPage(user));
            }
        }

        private void BtnChangeToWarehouse_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new WarehousePage());
        }
    }
}
