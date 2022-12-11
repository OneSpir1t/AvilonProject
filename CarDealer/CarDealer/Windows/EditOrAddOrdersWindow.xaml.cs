using CarDealer.Entity;
using CarDealer.UserControls;
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
    /// Логика взаимодействия для EditOrAddOrdersWindow.xaml
    /// </summary>
    public partial class EditOrAddOrdersWindow : Window
    {
        Orders orders;
        public EditOrAddOrdersWindow(Orders orders, Equipments equipments, Buyers buyers)
        {
            InitializeComponent();
            foreach(Equipments equip in CarDealerEntities.DbContext.Equipments.ToList())
            {
                Equipment_Combobox.Items.Add(equip);
            }
            foreach (Buyers buyer1 in CarDealerEntities.DbContext.Buyers.ToList())
            {
                Buyer_Combobox.Items.Add(buyer1);
            }
            if(orders != null)
            {
                AddEditOrder_Button.Content = "Изменить";
                DeleteOrder_Button.Visibility = Visibility.Visible; 
                this.orders = orders;
                Equipment_Combobox.SelectedItem = CarDealerEntities.DbContext.Equipments.Where(e => e.ID == orders.EquipmentID).FirstOrDefault();
                Buyer_Combobox.SelectedItem = CarDealerEntities.DbContext.Buyers.Where(b => b.ID == orders.BuyerID).FirstOrDefault();
                OrderDate_Label.Content = Convert.ToDateTime(orders.Date).ToString("dd.MM.yyyy");
                
            }
            if (equipments != null)
            {
                DeleteOrder_Button.Visibility = Visibility.Hidden;
                Equipment_Combobox.SelectedItem = CarDealerEntities.DbContext.Equipments.Where(e => e.ID == equipments.ID).FirstOrDefault();
                
            }
            if(buyers != null)
            {
                DeleteOrder_Button.Visibility = Visibility.Hidden;
                Buyer_Combobox.SelectedItem = CarDealerEntities.DbContext.Buyers.Where(b => b.ID == buyers.ID).FirstOrDefault();
            }
            else
            {
               
            }
            DataContext = this;
        }


        private void AddEditOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            if(AddEditOrder_Button.Content.ToString() == "Добавить")
            {
                orders = new Orders();
                orders.Date = DateTime.Today;
                orders.ManagerID = MainWindow.managers.ID;
                orders.Equipments = (Equipments)Equipment_Combobox.SelectedItem;
                orders.Buyers = (Buyers)Buyer_Combobox.SelectedItem;
                CarDealerEntities.DbContext.Orders.Add(orders);
                CarDealerEntities.DbContext.SaveChanges();
                MessageBox.Show("Заказ успешно создан");
                Close();
            }
            else
            {
                var result = MessageBox.Show("Вы действительно хотите изменить?", "Уведомление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    orders.Date = DateTime.Today;
                {
                    orders.Equipments = (Equipments)Equipment_Combobox.SelectedItem;
                    orders.Buyers = (Buyers)Buyer_Combobox.SelectedItem;
                    CarDealerEntities.DbContext.SaveChanges();
                    MessageBox.Show("Заказ изменен");
                    Close();
                }
            }
        }

        private void DeleteOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить?", "Уведомление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (AddEditOrder_Button.Content.ToString() == "Изменить")
                {
                    CarDealerEntities.DbContext.Orders.Remove(orders);
                    CarDealerEntities.DbContext.SaveChanges();
                    MessageBox.Show("Заказ успешно удален");
                    Close();
                }
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
