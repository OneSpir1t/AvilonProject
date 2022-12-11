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
using System.IO;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using System.Globalization;

namespace CarDealer.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {        
        public Orders currentOrder { get; set; }
        public static List<Orders> DisplayOrders = new List<Orders>();
        public OrdersWindow(Equipments equipments, Buyers buyers)
        {
            InitializeComponent();
            DataContext = this;
            if(equipments != null || buyers != null)
            {
                
                new EditOrAddOrdersWindow(null, equipments, buyers).ShowDialog();
            }
            UpdateOrdersList();
        }

        private void UpdateOrdersList()
        {
            OrderList_View.Items.Clear();
            DisplayOrders = CarDealerEntities.DbContext.Orders.Where(o => o.ManagerID == MainWindow.managers.ID).ToList();
            foreach(Orders orders in DisplayOrders)
            {
                OrderList_View.Items.Add(new OrdersControl(orders) { Width = GetNormalWidth() });
            }
            if(DisplayOrders.Count == 0)
            {
                EmptyOrders_Label.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyOrders_Label.Visibility = Visibility.Hidden;
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
            foreach (OrdersControl item in OrderList_View.Items)
            {
                item.Width = GetNormalWidth();
            }
        }

        private void AddEditOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            new EditOrAddOrdersWindow(null, null, null).ShowDialog();
            UpdateOrdersList();
        }

        private void OrderList_View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Orders orders = ((OrdersControl)OrderList_View.SelectedItem).Orders;
            new EditOrAddOrdersWindow(orders, null, null).ShowDialog();
            UpdateOrdersList();
        }      

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Word._Application aWord = new Word.Application();
        Word._Application aWord;
        
        private void PrintDoc_Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder != null)
            {
                aWord = new Word.Application();
                string ptc = Directory.GetCurrentDirectory() + "\\WordPattern\\PTS.docx";
                if (File.Exists(ptc))
                {
                    Word._Document aDoc = GetDoc(ptc);
                    string filename = System.IO.Path.GetRandomFileName() + ".docx";
                    Random rnd = new Random();
                    CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
                    var tags = new Dictionary<string, string>
                    {
                        {"<d>", Convert.ToDateTime(currentOrder.Date).ToString("dd") },
                        {"<month>", Convert.ToDateTime(currentOrder.Date).ToString("MMMM") },
                        {"<y", Convert.ToDateTime(currentOrder.Date).ToString("yy") },
                        {"<ManagerFIO>", string.Join(" ", currentOrder.Managers.LastName, currentOrder.Managers.FirstName, currentOrder.Managers.Patronymic) },
                        {"<Mpass,1>", currentOrder.Managers.Passport.ToString().Substring(0,4) },
                        {"<Mpass,2>", currentOrder.Managers.Passport.ToString().Substring(4) },
                        {"<Buyer>", string.Join(" ", currentOrder.Buyers.LastName, currentOrder.Buyers.FirstName, currentOrder.Buyers.Patronymic)},
                        {"<Bbirthday>", currentOrder.Buyers.Birthday.ToString() },
                        {"<address>", currentOrder.Buyers.Address},
                        {"<Bpass,1>", currentOrder.Buyers.Passport.ToString().Substring(0,4) },
                        {"<Bpass,2>", currentOrder.Buyers.Passport.ToString().Substring(4) },
                        {"<Brand>", currentOrder.Equipments.Models.Brands.Title},
                        {"<manufyear>", Convert.ToDateTime(currentOrder.Equipments.TechnicalInformation.Yearofmanufacture).ToString("yyyy") },
                        {"<engine>", currentOrder.Equipments.TechnicalInformation.EngineTypes.Title },
                        {"<Bodytype>", currentOrder.Equipments.TechnicalInformation.BodyTypes.Title },
                        {"<ss1>", rnd.Next(11,99).ToString() },
                        {"<ss2>", rnd.Next(100001, 999999).ToString() },
                        {"<Color>", currentOrder.Equipments.TechnicalInformation.Colors.Title},
                        {"<Cost>", currentOrder.Equipments.Cost.ToString("0,0", elGR) + " рублей"},
                        {"<Mphone>", currentOrder.Managers.Phone.ToString()},
                        {"<Bphone>", currentOrder.Buyers.Phone.ToString()},
                    };
                    foreach (var tag in tags)
                    {
                        aWord.Selection.Find.Text = tag.Key;
                        aWord.Selection.Find.Replacement.Text = tag.Value;
                        Object wrap = Word.WdFindWrap.wdFindContinue;
                        Object replace = Word.WdReplace.wdReplaceAll;
                        aWord.Selection.Find.Execute(FindText: Type.Missing, MatchCase: false, Forward: true, Wrap: wrap, Replace: replace);
                    }
                    string finalpath = "C:\\Users\\PC1\\Documents\\ " + filename;
                    aDoc.SaveAs2(finalpath);
                    MessageBox.Show("Договор купли-продажи успешно создан!", "Уведомление");
                    //aWord.Visible = true;
                    //aDoc.Close();
                    aWord.Quit();
                    if(File.Exists(finalpath))
                        System.Diagnostics.Process.Start(finalpath);
                }
                else
                {
                    MessageBox.Show("Шаблон не найден");
                }
            }
            else
            {
                MessageBox.Show("Заказ не выбран", "Уведомление");
            }
        }

        private Word._Document GetDoc(string path)
        {
            Word._Document aDoc = aWord.Documents.Add(path);
            return aDoc;
        }

        private void OrderList_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(OrderList_View.SelectedItem != null)
            {
                Orders orders = ((OrdersControl)OrderList_View.SelectedItem).Orders;
                currentOrder = orders;
            }
        }
    }
}
