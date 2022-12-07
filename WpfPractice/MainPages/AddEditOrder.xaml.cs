using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfPractice.MainPages
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrder.xaml
    /// </summary>
    public partial class AddEditOrder : Page
    {
        private Order order1 = new Order();
        private Client client1 = new Client();

        public AddEditOrder(Order order)
        {
            InitializeComponent();

            try
            {
                SetFilterTypeItems();
                SetFilterStatusItems();
                if (order != null)
                {
                    order1 = order;

                    FindFilterTypeId();
                    FindFilterStatusId();

                    if(order1.IsPayed == "Оплачен")
                    {
                        checkB.IsChecked = true;
                    }
                    //Auth
                }
                else
                {
                    TechFilter.SelectedIndex = 0;
                    StatusCb.SelectedIndex = 0;

                    DeleteData.Visibility = Visibility.Hidden;
                    AddData.Content = "Добавить заказ";
                }

                DataContext = order1;
            }
            catch
            {
                MessageBox.Show("Критическая ошибка.\nНе удалось обработать запуск формы", "Ошибка при авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FindFilterStatusId()
        {
            var type = AppConnect.ModelDB.TypeTechnic.FirstOrDefault(x => x.ID == order1.IDTypeTechnic);

            TechFilter.SelectedItem = type.Name;
        }

        private void FindFilterTypeId()
        {
            var status = AppConnect.ModelDB.Status.FirstOrDefault(x => x.ID == order1.IDStatus);

            StatusCb.SelectedItem = status.Name;
        }

        private void SetFilterStatusItems()
        {
            foreach (var status in AppConnect.ModelDB.Status)
            {
                StatusCb.Items.Add(status.Name);
            }
        }

        private void SetFilterTypeItems()
        {
            foreach (var type in AppConnect.ModelDB.TypeTechnic)
            {
                TechFilter.Items.Add(type.Name);
            }
        }

        /// <summary>
        /// Save new client, if it new and save order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddData_Click(object sender, RoutedEventArgs e)
        {

            TextBox[] textBox = { ClientSurname, ClientName, ClientSurname, DescriptionTb };

            foreach (TextBox txt in textBox)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Не все поля заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            if (Phone.Text.Length > 12 || Phone.Text.Length < 11 || !Phone.Text.StartsWith("7")
                && !string.IsNullOrEmpty(Phone.Text))
            {
                MessageBox.Show("Номер телефона начинается с 7 и больше 11 символов", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            try
            {
                FindFilterTypeItem();
                FindFilterStatusItem();
                Client client1 = new Client
                {
                    Surname = ClientSurname.Text,
                    Name = ClientName.Text,
                    Secondname = ClientSecondname.Text,
                    DateBirth = DateTime.Now,
                    Phone = Phone.Text
                };

                if (order1.ID == 0)
                {
                    Entities.GetContext().Client.Add(client1);
                    order1.IDClient = 1 + (from m in Entities.GetContext().Client select m.ID).ToList().Last();
                }

                Entities.GetContext().SaveChanges();
                AppConnect.ModelDB.SaveChanges();

                if (checkB.IsChecked == true)
                    order1.IsPayed = "Оплачено";
                else
                    order1.IsPayed = "Не оплачено";

                order1.DateOfStart = DateTime.Parse(datePickerOn.SelectedDate.ToString());
                order1.DateOfEnd = DateTime.Parse(datePickerOff.SelectedDate.ToString());
                order1.IDUser = SelectedUser.user.ID;
                order1.Technic = "Aasd";

                if (order1.ID == 0)
                {
                    Entities.GetContext().Order.Add(order1);
                }
                
                Entities.GetContext().SaveChanges();

                AppConnect.ModelDB.SaveChanges();
                MessageBox.Show("Сохранил!");

                AppFrame.fraimMain.GoBack();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindFilterStatusItem()
        {
            var status = AppConnect.ModelDB.Status.FirstOrDefault(x => x.Name == StatusCb.SelectedItem.ToString());
            if (status == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                order1.IDStatus = status.ID;
            }
        }

        private void FindFilterTypeItem()
        {
            var type = AppConnect.ModelDB.TypeTechnic.FirstOrDefault(x => x.Name == TechFilter.SelectedItem.ToString());
            if (type == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                order1.IDTypeTechnic = type.ID;
            }
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.Order.Remove(order1);
            AppConnect.ModelDB.SaveChanges();
            NavigationService.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }
    }
}
