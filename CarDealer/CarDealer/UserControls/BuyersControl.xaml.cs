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
    /// Логика взаимодействия для BuyersControl.xaml
    /// </summary>
    public partial class BuyersControl : UserControl
    {
        public Buyers Buyers { get; set; }
        public BuyersControl(Buyers buyers)
        {
            InitializeComponent();
            Buyers = buyers;
            LastName_Label.Content = "Фамилия: " + buyers.LastName;
            FirstName_Label.Content = "Имя: " + buyers.FirstName;
            Patryonomic_Label.Content = "Отчество: " + buyers.Patronymic;
        }
    }
}
