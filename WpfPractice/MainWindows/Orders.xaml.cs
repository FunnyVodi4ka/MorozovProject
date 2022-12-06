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
using System.Windows.Shapes;
using WpfPractice.AppConnection;

namespace WpfPractice.MainPages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            InitializeComponent();

            try
            {
                AppConnect.ModelDB = new Entities();
                AppFrame.fraimMain = FrmMain;

                AppFrame.fraimMain.Navigate(new OrderPage());

                if (SelectedUser.user == null)
                {
                    UserName.Text += "Гость";
                }
                else
                {
                    UserName.Text += SelectedUser.user.Login.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось подключиться к базе данных", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Qiut_Btn_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser.user = null;

            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }
    }
}
