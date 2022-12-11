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
    /// Логика взаимодействия для BuyersWindow.xaml
    /// </summary>
    public partial class BuyersWindow : Window
    {
        public static List<Buyers> DisplayBuyers = new List<Buyers>();
        Buyers currentBuyer { get; set; }
        public BuyersWindow()
        {
            InitializeComponent();
            ManagerFio_Label.Content = string.Join(" ", MainWindow.managers.LastName, MainWindow.managers.FirstName, MainWindow.managers.Patronymic);   
            UpdateBuyers();
        }
        private Double GetNormalWidth()
        {
            if (WindowState == WindowState.Maximized)
                return RenderSize.Width - 50;

            else
                return Width - 50;
        }
        
        private void UpdateBuyers()
        {
            DisplayBuyers = CarDealerEntities.DbContext.Buyers.ToList();
            if (!string.IsNullOrEmpty(Search_TextBox.Text))
            {
                DisplayBuyers = DisplayBuyers.Where(b => b.FirstName.ToLower().Contains(Search_TextBox.Text.ToLower()) || b.LastName.ToLower().Contains(Search_TextBox.Text.ToLower())
                || b.Patronymic.ToLower().Contains(Search_TextBox.Text.ToLower())).ToList();
            }
            Buyers_ViewList.Items.Clear();
            foreach (Buyers buyers in DisplayBuyers)
            {
                Buyers_ViewList.Items.Add(new BuyersControl(buyers) { Width = GetNormalWidth() });
            }
            if(Buyers_ViewList.Items.Count == 0)
            {
                EmptySearch_Label.Visibility = Visibility.Visible;
            }
            else
            {
                EmptySearch_Label.Visibility = Visibility.Hidden;
            }
        }

        private void Buyers_ViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Buyers buyers = ((BuyersControl)Buyers_ViewList.SelectedItem).Buyers;
            new EditOrAddBuyersWindow(buyers).ShowDialog();
            UpdateBuyers();

        }

        private void AddEditBuyer_Button_Click(object sender, RoutedEventArgs e)
        {
            new EditOrAddBuyersWindow(null).ShowDialog();
            UpdateBuyers();
        }

        private void Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBuyers();
        }

        private void ChooseBuyer_Button_Click(object sender, RoutedEventArgs e)
        {
            if(currentBuyer != null)
            {
                MainWindow.currentBuyer = currentBuyer;
                Close();
            }
        }

        private void Buyers_ViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Buyers_ViewList.SelectedItem != null)
            {
                currentBuyer = ((BuyersControl)Buyers_ViewList.SelectedItem).Buyers;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
