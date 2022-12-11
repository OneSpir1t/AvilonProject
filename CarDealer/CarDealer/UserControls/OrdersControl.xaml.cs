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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarDealer.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrdersControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        public Orders Orders { get; set; }
        public OrdersControl(Orders orders)
        {
            InitializeComponent();
            Orders = orders;
            Buyer_Label.Content = "Покупатель: " + string.Join(" ", orders.Buyers.LastName, orders.Buyers.FirstName);
            EquipmentName_Label.Content = "Комплектация: " + orders.Equipments.Title;
            Mark_Label.Content = "Марка: " + orders.Equipments.Models.Brands.Title;
            Model_Label.Content = "Модель: " + orders.Equipments.Models.Title;
        }
    }
}
