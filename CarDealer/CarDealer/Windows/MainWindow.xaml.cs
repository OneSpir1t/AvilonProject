using CarDealer.Entity;
using CarDealer.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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


namespace CarDealer.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Managers managers { get; set; } 
        public static Buyers currentBuyer { get; set; }
        bool flag;
        public static List<Equipments> DisplayEquipmets = new List<Equipments>();
        int countPages = 1;
        int currentPage = 1;
        Equipments currentEquipment { get; set; }

        public MainWindow(Managers manager)
        {
            managers = manager;
            InitializeComponent();
            
            DataContext = this;
            FioManager_Label.Content = string.Join(" ", manager.LastName, manager.FirstName, manager.Patronymic);
            UpdateCarList();
            MarkSort_Combobox.Items.Add("Все марки");
            foreach(Brands brands in CarDealerEntities.DbContext.Brands.ToList())
            {
                MarkSort_Combobox.Items.Add(brands.Title);
            }
            BodyType_Combobox.Items.Add("Все типы");
            foreach(BodyTypes bodytype in CarDealerEntities.DbContext.BodyTypes)
            {
                BodyType_Combobox.Items.Add(bodytype.Title);
            }
            Color_Combobox.Items.Add("Все цвета");
            foreach(Entity.Colors colors in CarDealerEntities.DbContext.Colors.ToList())
            {
                Color_Combobox.Items.Add(colors.Title);
            }
            TypeEngine_Combobox.Items.Add("Все типы");
            foreach(EngineTypes enginetype in CarDealerEntities.DbContext.EngineTypes.ToList())
            {
                TypeEngine_Combobox.Items.Add(enginetype.Title);
            }
            Sort_Combobox.Items.Add("Умолчанию");
            Sort_Combobox.Items.Add("Подешевле");
            Sort_Combobox.Items.Add("Подороже");    

        }

        private void LogOut_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }

        private void UpdateCarList()
        {
            DisplayEquipmets = CarDealerEntities.DbContext.Equipments.ToList();
            if (MarkSort_Combobox.SelectedIndex > 0)
            {
                DisplayEquipmets = DisplayEquipmets.Where(m => m.Models.Brands.Title == MarkSort_Combobox.SelectedItem.ToString()).ToList();
            }
            if(BodyType_Combobox.SelectedIndex > 0)
            {
                DisplayEquipmets = DisplayEquipmets.Where(b => b.TechnicalInformation.BodyTypes.Title == BodyType_Combobox.SelectedItem.ToString()).ToList();
            }
            if(!string.IsNullOrEmpty(MinCost_TextBox.Text))
            {
                DisplayEquipmets = DisplayEquipmets.Where(c => c.Cost >= float.Parse(MinCost_TextBox.Text)).ToList();    
            }
            if (!string.IsNullOrEmpty(MaxCost_TextBox.Text))
            {
                DisplayEquipmets = DisplayEquipmets.Where(c => c.Cost <= float.Parse(MaxCost_TextBox.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(MinHorsePower_TextBox.Text))
            {
                DisplayEquipmets = DisplayEquipmets.Where(c => c.TechnicalInformation.Horsepower >= float.Parse(MinHorsePower_TextBox.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(MaxHorsePower_TextBox.Text))
            {
                DisplayEquipmets = DisplayEquipmets.Where(c => c.TechnicalInformation.Horsepower <= float.Parse(MaxHorsePower_TextBox.Text)).ToList();
            }
            if (Sort_Combobox.SelectedIndex > 0)
            {
                switch (Sort_Combobox.SelectedIndex)
                {
                    case 1:
                        DisplayEquipmets = DisplayEquipmets.OrderBy(p => p.Cost).ToList();
                        break;
                    case 2:
                        DisplayEquipmets = DisplayEquipmets.OrderByDescending(p => p.Cost).ToList();
                        break;
                }
            }
            if(Color_Combobox.SelectedIndex > 0)
            {
                DisplayEquipmets = DisplayEquipmets.Where(c => c.TechnicalInformation.Colors.Title == Color_Combobox.SelectedItem.ToString()).ToList();
            }
            if(TypeEngine_Combobox.SelectedIndex > 0)
            {
                DisplayEquipmets = DisplayEquipmets.Where(e => e.TechnicalInformation.EngineTypes.Title == TypeEngine_Combobox.SelectedItem.ToString()).ToList();
            }
            if(!string.IsNullOrEmpty(Search_TextBox.Text))
            {
                DisplayEquipmets = DisplayEquipmets.Where(n => n.Models.Brands.Title.ToLower().Contains(Search_TextBox.Text.ToLower()) || n.Models.Title.ToLower().Contains(Search_TextBox.Text.ToLower())).ToList();
            }
            if (MarkSort_Combobox.SelectedIndex == 0 && BodyType_Combobox.SelectedIndex == 0 && Sort_Combobox.SelectedIndex ==0 &&
                Color_Combobox.SelectedIndex ==0 && TypeEngine_Combobox.SelectedIndex == 0 && string.IsNullOrEmpty(Search_TextBox.Text) &&
                string.IsNullOrEmpty(MinCost_TextBox.Text) && string.IsNullOrEmpty(MaxCost_TextBox.Text) && string.IsNullOrEmpty(MinHorsePower_TextBox.Text) &&
                string.IsNullOrEmpty(MaxHorsePower_TextBox.Text))
            {
                DisplayEquipmets = CarDealerEntities.DbContext.Equipments.ToList();
            }
            if (flag)
            {
                Pages_Combobox.Items.Clear();
                countPages = DisplayEquipmets.Count / 10;
                if (DisplayEquipmets.Count % 10 > 0)
                {
                    countPages++;
                }
                for (int i = 1; i <= countPages; i++)
                {
                    Pages_Combobox.Items.Add(i);
                }
                flag = false;
                Pages_Combobox.SelectedIndex = 0;
            }
            Cars_ViewList.Items.Clear();
            foreach(Equipments equipments in DisplayEquipmets.Skip((currentPage - 1) * 10).Take(10))
            {
                Cars_ViewList.Items.Add(new CarsControl(equipments) { Width = GetNormalWidth() });
            }
            if (DisplayEquipmets.Count == 0)
            {

                EmptySearch_Label.Visibility = Visibility.Visible;
            }
            else
            {
                EmptySearch_Label.Visibility = Visibility.Hidden;
            }
        }

        private Double GetNormalWidth()
        {
            if (WindowState == WindowState.Maximized)
                return RenderSize.Width - 50;

            else
                return Width - 50;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (CarsControl item in Cars_ViewList.Items)
            {
                item.Width = GetNormalWidth();
            }
        }       

        private void MarkSort_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void BodyType_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }


        private void Sort_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void Color_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void TypeEngine_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void MinCost_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void MaxCost_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void MinHorsePower_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void MaxHorsePower_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
            UpdateCarList();
        }

        private void MinCost_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void MaxCost_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void MinHorsePower_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void MaxHorsePower_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void DropSettings_Button_Click(object sender, RoutedEventArgs e)
        {
            Pages_Combobox.SelectedIndex = 0;
            Search_TextBox.Text = default;
            BodyType_Combobox.SelectedIndex = 0;
            Color_Combobox.SelectedIndex = 0;
            MarkSort_Combobox.SelectedIndex = 0;
            Sort_Combobox.SelectedIndex = 0;
            TypeEngine_Combobox.SelectedIndex = 0;
            MinCost_TextBox.Text = default;
            MaxCost_TextBox.Text = default;
            MinHorsePower_TextBox.Text = default;
            MaxHorsePower_TextBox.Text = default;
        }

        private void EditOrAddCar_Button_Click(object sender, RoutedEventArgs e)
        {
            new EditOrAddCarsWindow(null).ShowDialog();
            if (EditOrAddCarsWindow.isedited)
            {
                flag = true;
                UpdateCarList();
            }
        }

        private void Cars_ViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Cars_ViewList.SelectedItem != null)
            {
                Equipments equipment = ((CarsControl)Cars_ViewList.SelectedItem).Equipments;
                new EditOrAddCarsWindow(equipment).ShowDialog();
                if (EditOrAddCarsWindow.isedited)
                {
                    flag = true;
                    UpdateCarList();
                }
            }
        }

        private void Pages_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Pages_Combobox.SelectedItem != null && !flag)
            {
                currentPage = (int)Pages_Combobox.SelectedItem;
                UpdateCarList();
            }
            int PageCount = 10;
            if(Pages_Combobox.SelectedIndex + 1 < countPages && Pages_Combobox.SelectedItem != null)
            {
                for(int i = 0; i < Pages_Combobox.SelectedIndex; i++)
                {
                    PageCount += 10;
                }
                CountItems_Label.Content = ($"{PageCount} из {DisplayEquipmets.Count}");
            }
            else
            {
                CountItems_Label.Content = ($"{DisplayEquipmets.Count} из {DisplayEquipmets.Count}");
            }
        }

        private void BackPage_Buttom_Click(object sender, RoutedEventArgs e)
        {
            if(currentPage > 1)
            {
                Pages_Combobox.SelectedItem = currentPage - 1;
            }
        }

        private void NextPage_Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < countPages)
            {
                Pages_Combobox.SelectedItem = currentPage + 1;
            }
        }

        private void Clients_Button_Click(object sender, RoutedEventArgs e)
        {
            new BuyersWindow().Show();
        }

        private void Cars_ViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cars_ViewList.SelectedItem != null)
            {
                currentEquipment = ((CarsControl)Cars_ViewList.SelectedItem).Equipments;
            }
        }

        private void AddOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            if(currentEquipment != null)
            {
                new OrdersWindow(currentEquipment, currentBuyer).ShowDialog();
            }
        }

        private void Orders_Button_Click(object sender, RoutedEventArgs e)
        {
            new OrdersWindow(null, null).Show();
        }

        private void ShowParam_Button_Click(object sender, RoutedEventArgs e)
        {
            SortParam_Grid.Visibility = Visibility.Visible;
            SortParam_Grid.Height = 200;
            ShowParam_Button.Visibility = Visibility.Hidden;
        }

        private void HideParam_Button_Click(object sender, RoutedEventArgs e)
        {
            SortParam_Grid.Visibility = Visibility.Hidden;
            SortParam_Grid.Height = 0;
            ShowParam_Button.Visibility = Visibility.Visible;
        }
    }
}
