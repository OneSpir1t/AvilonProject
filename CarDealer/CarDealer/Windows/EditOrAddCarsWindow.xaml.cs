using CarDealer.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarDealer.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrAddCarsWindow.xaml
    /// </summary>
    public partial class EditOrAddCarsWindow : Window
    {
        public static bool isedited;
        Equipments equipments;
        static Equipments equipments1 { get; set; }
        OpenFileDialog ofd = new OpenFileDialog();
        string photoCar;
        string mainPhotosPath = Environment.CurrentDirectory + "/CarImages/";
        bool flag, flag2 = true;
        public EditOrAddCarsWindow(Equipments equipments)
        {
            InitializeComponent();
            isedited = false;
            ofd.Filter = "All files (*.*)|*.*|Png files(*.png)|*.png|Jpg files(.*jpg)|*.jpg";
            Mark_Combobox.ItemsSource = CarDealerEntities.DbContext.Brands.ToList();
            BodyType_Combobox.ItemsSource = CarDealerEntities.DbContext.BodyTypes.ToList();
            TypeEngine_Combobox.ItemsSource = CarDealerEntities.DbContext.EngineTypes.ToList();
            Color_Combobox.ItemsSource = CarDealerEntities.DbContext.Colors.ToList();
            for (int i = 1; i < 6; i++)
            {
                CountSeats_Combobox.Items.Add(i);
            }
            if (equipments != null)
            {
                this.equipments = equipments;
                if(equipments.Image != null && equipments.Image.Length > 3)
                {
                    if (File.Exists(mainPhotosPath + equipments.Image))
                        Car_Image.Source = new BitmapImage(new Uri(mainPhotosPath + equipments.Image));
                    else
                    {
                        Car_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/defaultImage.png"));
                    }
                }
                else
                {
                    Car_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/defaultImage.png"));
                }
                AddChange_Button.Content = "Изменить";
                equipments1 = equipments;
                Mark_Combobox.SelectedItem = equipments.Models.Brands;
                ModelName_Textbox.Text = equipments.Models.Title;
                BodyType_Combobox.SelectedItem = equipments.TechnicalInformation.BodyTypes;
                TypeEngine_Combobox.SelectedItem = equipments.TechnicalInformation.EngineTypes;
                Color_Combobox.SelectedItem = equipments.TechnicalInformation.Colors;
                HorsePower_Textbox.Text = equipments.TechnicalInformation.Horsepower.ToString();
                YearManuf_DatePicker.SelectedDate = equipments.TechnicalInformation.Yearofmanufacture;
                CountSeats_Combobox.SelectedItem = equipments.TechnicalInformation.CountSeats;
            }
            else
            {
                DeleteEq_Button.Visibility = Visibility.Hidden;
                this.equipments = new Equipments();
                Car_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/defaultImage.png"));
            }
            DataContext = this.equipments;           
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            isedited = false;
            Close();
        }

        private void AddChange_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Cost_Textbox.Text) || string.IsNullOrEmpty(HorsePower_Textbox.Text) ||
                string.IsNullOrEmpty(ModelName_Textbox.Text) || string.IsNullOrEmpty(NameEq_Textbox.Text))
            {
                MessageBox.Show("Заполните поля");
            }
            else if (AddChange_Button.Content.ToString() == "Добавить")
            {
                if (YearManuf_DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату");
                }
                else
                {
                    try
                    {
                        Models models = new Models
                        {
                            Title = ModelName_Textbox.Text,
                            Brands = (Brands)Mark_Combobox.SelectedItem
                        };
                        TechnicalInformation technicalInformation = new TechnicalInformation
                        {
                            BodyTypes = (BodyTypes)BodyType_Combobox.SelectedItem,
                            EngineTypes = (EngineTypes)TypeEngine_Combobox.SelectedItem,
                            Colors = (Entity.Colors)Color_Combobox.SelectedItem,
                            Horsepower = Convert.ToInt32(HorsePower_Textbox.Text),
                            CountSeats = Convert.ToInt32(CountSeats_Combobox.SelectedItem),
                            Yearofmanufacture = (DateTime)YearManuf_DatePicker.SelectedDate
                        };
                        equipments.Models = models;
                        equipments.TechnicalInformation = technicalInformation;
                        if (photoCar != null)
                        {
                            string carImage = System.IO.Path.GetFileName(photoCar);
                            File.Copy(photoCar, Environment.CurrentDirectory + "/CarImages/" + carImage, true);
                            equipments.Image = carImage;
                        }
                        CarDealerEntities.DbContext.Equipments.Add(equipments);
                        CarDealerEntities.DbContext.SaveChanges();
                        MessageBox.Show("Комплектиция добавлена");
                        isedited = true;
                        Close();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                        {
                            MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());

                            foreach (DbValidationError err in validationError.ValidationErrors)
                            {
                                MessageBox.Show(err.ErrorMessage + "");
                            }
                        }
                    }
                }
            }
            else
            {
                if (YearManuf_DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату");
                }
                else
                {
                    if (photoCar != null)
                    {
                        string carImage = System.IO.Path.GetFileName(photoCar);
                        carImage = System.IO.Path.GetRandomFileName() + ".png";
                        File.Copy(photoCar, Environment.CurrentDirectory + "/CarImages/" + carImage, true);
                        equipments1.Image = carImage;
                    }
                    equipments1.Title = NameEq_Textbox.Text;
                    equipments1.Models.Brands = (Brands)Mark_Combobox.SelectedItem;
                    equipments1.Models.Title = ModelName_Textbox.Text;
                    equipments1.TechnicalInformation.BodyTypes = (BodyTypes)BodyType_Combobox.SelectedItem;
                    equipments1.TechnicalInformation.EngineTypes = (EngineTypes)TypeEngine_Combobox.SelectedItem;
                    equipments1.Cost = float.Parse(Cost_Textbox.Text);
                    equipments1.TechnicalInformation.Colors = (Entity.Colors)Color_Combobox.SelectedItem;
                    equipments1.TechnicalInformation.Horsepower = Convert.ToInt32(HorsePower_Textbox.Text);
                    equipments1.TechnicalInformation.Yearofmanufacture = (DateTime)YearManuf_DatePicker.SelectedDate;
                    equipments1.TechnicalInformation.CountSeats = Convert.ToInt32(CountSeats_Combobox.SelectedItem);
                    CarDealerEntities.DbContext.SaveChanges();
                    MessageBox.Show("Комплектация успешно изменена!");
                    isedited = true;
                    Close();
                }
            }

        }

        private void DeleteEq_Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить?", "Уведомление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CarDealerEntities.DbContext.TechnicalInformation.Remove(equipments1.TechnicalInformation);
                CarDealerEntities.DbContext.Models.Remove(CarDealerEntities.DbContext.Models.Where(m => m.ID == equipments1.ID).FirstOrDefault());
                //CarDealerEntities.DbContext.Equipments.Remove(equipments1);
                CarDealerEntities.DbContext.SaveChanges();
                MessageBox.Show("Вы успешно удалили комплектацию");
                isedited = true;
                Close();
            }
            
        }


        private void Cost_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void HorsePower_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;          
            
        }

        private void ChangeImage_Button_Click(object sender, RoutedEventArgs e)
        {
            if(ofd.ShowDialog() == true)
            {
                photoCar = ofd.FileName;
                Car_Image.Source = new BitmapImage(new Uri(photoCar));
            }
        }

        private void HorsePower_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Cost_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if(Cost_Textbox.Text.Length == 0)
            {
                
            }
        }

        private void NameEq_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (NameEq_Textbox.Text.Length == 0)
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                }
                flag = true;
            }
            else if (e.Key == Key.Space)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            
        }

        private void ModelName_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ModelName_Textbox.Text.Length == 0)
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                }
                flag2 = true;
            }
            else if (e.Key == Key.Space)
            {
                if (flag2)
                {
                    flag2 = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
    }
}
