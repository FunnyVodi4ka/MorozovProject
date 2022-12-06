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
    /// Логика взаимодействия для AdminAddPage.xaml
    /// </summary>
    public partial class AdminAddPage : Page
    {
        private User user1 = new User();

        public AdminAddPage(User user)
        {
            InitializeComponent();

            try
            {
                SetRoleItems();

                if(user != null)
                {
                    user1 = user;
                    FindRoleItem();
                }
                else
                {
                    DeleteData.Visibility = Visibility.Hidden;
                    AddData.Content = "Добавить пользователя";
                }
            }
            catch
            {
                MessageBox.Show("Не удалось иницализировать форму и данные для нее.\nПопробуйте позже.", "Внимание!", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            DataContext = user1;
        }

        private void FindRoleItem()
        {
            var role = AppConnect.ModelDB.Roles.FirstOrDefault(x => x.ID == user1.IDRole);

            RoleFilter.SelectedItem = role.Name;
        }

        private void SetRoleItems()
        {
            foreach (var roles in AppConnect.ModelDB.Roles)
            {
                RoleFilter.Items.Add(roles.Name);
            }
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            TextBox[] textBox = { ClientName, Surname, ClientSecondname, MailTb, Phone, LoginTb };

            foreach (TextBox txt in textBox)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Не все поля заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(PasswordTb.Password))
                errors.AppendLine("Укажите пароль");
            if (!Regex.IsMatch(MailTb.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") && !string.IsNullOrEmpty(MailTb.Text))
                errors.AppendLine("Неправильно указана почта");
            if (!Regex.IsMatch(PasswordTb.Password, @"((?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9!@#$%^&*a-zA-Z]{6,})")
                && !string.IsNullOrEmpty(PasswordTb.Password))
                errors.AppendLine("Пароль должен содержать прописные и заглавные буквы, цифры и специальные символы");
            if (Phone.Text.Length > 12 || Phone.Text.Length < 11 || !Phone.Text.StartsWith("7")
                && !string.IsNullOrEmpty(Phone.Text))
                errors.AppendLine("Номер телефона начинается с 7 и больше 11 символов");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            FindFilterRoleItem();

            try
            {
                user1.Password = PasswordTb.Password.ToString();

                if (user1.ID == 0)
                {
                    Entities.GetContext().User.Add(user1);
                }
                
                Entities.GetContext().SaveChanges();

                AppConnect.ModelDB.SaveChanges();
                MessageBox.Show("Данные сохранены!");

                AppFrame.fraimMain.GoBack();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindFilterRoleItem()
        {
            var role= AppConnect.ModelDB.Roles.FirstOrDefault(x => x.Name == RoleFilter.SelectedItem.ToString());

            if (role == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                user1.IDRole = role.ID;
            }
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelDB.User.Remove(user1);
            AppConnect.ModelDB.SaveChanges();
            NavigationService.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }
    }
}
