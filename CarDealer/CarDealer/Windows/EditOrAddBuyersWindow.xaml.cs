using CarDealer.Entity;
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
using System.Windows.Shapes;

namespace CarDealer.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrAddBuyersWindow.xaml
    /// </summary>
    public partial class EditOrAddBuyersWindow : Window
    {
        Buyers buyers { get; set; }
        public EditOrAddBuyersWindow(Buyers buyers)
        {
            InitializeComponent();
            if(buyers != null)
            {
                this.buyers = buyers;
                AddEditBuyer_Button.Content = "Изменить";
            }
            else
            {
                this.buyers = new Buyers();
                this.buyers.Birthday = DateTime.Today;
                this.buyers.Phone = "7";
                DeleteBuyer_Button.Visibility = Visibility.Hidden;
            }
            DataContext = this.buyers;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddEditBuyer_Button_Click(object sender, RoutedEventArgs e)
        {
            if(buyers.FirstName == null || buyers.LastName == null ||
                buyers.Address == null || buyers.Birthday == null || Passport_Textbox.Text.Length < 10 || Phone_Textbox.Text.Length < 11)
            {
                MessageBox.Show("Заполните поля");
            }
            else if(AddEditBuyer_Button.Content.ToString() == "Добавить")
            {
                if(Birtday_DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату");
                }
                else
                {
                    CarDealerEntities.DbContext.Buyers.Add(buyers);
                    CarDealerEntities.DbContext.SaveChanges();
                    MessageBox.Show("Покупатель успешно добавлен", "Уведомление");
                    Close();
                }
            }
            else
            {
                CarDealerEntities.DbContext.SaveChanges();
                MessageBox.Show("Данные успешно изменены", "Уведомление");
                Close();
            }
        }

        private void DeleteBuyer_Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить?", "Уведомление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CarDealerEntities.DbContext.Buyers.Remove(buyers);
                CarDealerEntities.DbContext.SaveChanges();
                MessageBox.Show("Вы успешно удалили покупателя");
                Close();
            }
        }

        private void Passport_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void Phone_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void LastName_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.Match(e.Text, @"[а-яА-Я]|[a-zA-Z]").Success;
        }

        private void FirstName_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.Match(e.Text, @"[а-яА-Я]|[a-zA-Z]").Success;
        }

        private void Patronymic_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.Match(e.Text, @"[а-яА-Я]|[a-zA-Z]").Success;
        }

        private void LastName_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void FirstName_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Patronymic_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Passport_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Phone_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
