using System;
using System.Collections.Generic;
using System.Globalization;
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
using CarDealer.Entity;

namespace CarDealer.UserControls
{
    /// <summary>
    /// Логика взаимодействия для CarsControl.xaml
    /// </summary>
    public partial class CarsControl : UserControl
    {
        public Equipments Equipments { get; set; }
        public CarsControl(Equipments equipments)
        {            
            InitializeComponent();
            DataContext = this;
            Equipments = equipments;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Equipments != null)
            {
                Mark_Label.Content = Equipments.Models.Brands.Title;
                Country_Label.Content = Equipments.Models.Brands.Countries.Title;
                ModelName_Label.Content = Equipments.Models.Title + ", " + Convert.ToDateTime(Equipments.TechnicalInformation.Yearofmanufacture).ToString("yyyy");
                BodyType_Label.Content = "Тип кузова: " + Equipments.TechnicalInformation.BodyTypes.Title;
                Color_Label.Content = "Цвет: " + Equipments.TechnicalInformation.Colors.Title;
                EngineType_Label.Content = "Тип двигателя: " + Equipments.TechnicalInformation.EngineTypes.Title;
                CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
                Cost_Label.Content = Equipments.Cost.ToString("0,0", elGR) + " ₽";
                HorsePower_Label.Content = "Кол-во л.с. " + Equipments.TechnicalInformation.Horsepower;
                string mainPhotosPath = Environment.CurrentDirectory + "/CarImages/";
                if (Equipments.Image != null && Equipments.Image.Length > 3)
                {
                    if (System.IO.File.Exists(mainPhotosPath + Equipments.Image))
                        Car_Image.Source = new BitmapImage(new Uri(mainPhotosPath + Equipments.Image));
                    else
                    {
                        Car_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/defaultImage.png"));
                    }
                }
                else
                {
                    Car_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/defaultImage.png"));
                }
            }
        }
    }
}
