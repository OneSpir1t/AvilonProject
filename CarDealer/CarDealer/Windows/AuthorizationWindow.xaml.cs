using CarDealer.Entity;
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

namespace CarDealer.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();  
        }

        public static Managers manager { get; set; }

        private void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            string pass;
            if(ShowHidePass_CheckBox.IsChecked == false)
            {
                pass = Password_PassBox.Password;
            }
            else
            {
                pass = Password_TextBox.Text;
            }
            manager = CarDealerEntities.DbContext.Managers.FirstOrDefault(m => m.Login == Login_TextBox.Text && m.Password == pass);
            if (manager != null)
            {
                Hide();
                MessageBox.Show($"Вы вошли как {string.Join(" ", manager.LastName, manager.FirstName, manager.Patronymic)}", "Успех");
                var m1 = new MainWindow(manager);
                m1.Owner = this;
                m1.Show();
                Password_TextBox.Text = default;
                Password_PassBox.Password = default;
            }
            else
            {
                MessageBox.Show("Пользователь не найден", "Уведомление");
            }
        }

        private void ShowHidePass_CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            Password_TextBox.Visibility = Visibility.Visible;
            Password_PassBox.Visibility = Visibility.Hidden;
            Password_TextBox.Text = Password_PassBox.Password;
        }

        private void ShowHidePass_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Password_TextBox.Visibility = Visibility.Hidden;
            Password_PassBox.Visibility = Visibility.Visible;
            Password_PassBox.Password = Password_TextBox.Text;
        }
    }
}
