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
using WpfPractice.MainPages;

namespace WpfPractice
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            try
            {
                AppConnect.ModelDB = new Entities();
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось создать экземляр базы данных", "Ошибка при авторизации!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRegist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.ModelDB.User.FirstOrDefault(x => x.Login == txtLogin.Text && x.Password == txtPassword.Password);

                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (userObj.Login == txtLogin.Text && txtPassword.Password == null)
                {
                    MessageBox.Show("Пользователь с таким логином уже есть!", "Ошибка при авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SelectedUser.user = userObj;

                    Orders orders = new Orders();
                    orders.Show();
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка.\nНе удалось определить пользователя" + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
