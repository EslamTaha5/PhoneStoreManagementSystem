using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Data.SqlClient;

namespace PhoneStoreManagementSystem
{
    /// <summary>
    /// Interaction logic for ShowPhones.xaml
    /// </summary>
    public partial class ShowPhones : Page
    {
        public new string Title = "Show Phones";
        public override string ToString() {
            return Title;
        }
        private DataTable dtToFill = null;
        private DataGrid dataGridToDisplay = null;

        public ShowPhones()
        {
            InitializeComponent();
            Console.WriteLine(Home.ID);
            Display();
        }

        private DataTable GetDataTable() {

            string Query = "select * from Phones";
            DataTable dt = Common.ExecQuery(Query);
            return dt;
        }

        private DataGrid GetDataGrid() {
            var dt = GetDataTable();
            DataGrid dg = new DataGrid {
                IsReadOnly = true,
                ItemsSource = dt.DefaultView,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            return dg;
        }


        private void Display() {
            dataGridToDisplay = GetDataGrid();
            FillCombo(dataGridToDisplay);
            dataGridToDisplay.MouseDoubleClick += Dg_MouseDoubleClick;
            Content_Box.Content = dataGridToDisplay;

            Console.WriteLine("Show Phones Loaded");
        }
        private void FillCombo(DataGrid dg) {
            HashSet<string> brandSet = new HashSet<string>(); // To store unique brand names

            foreach (DataRowView row in dg.ItemsSource) // Loop through DataGrid rows
            {
                string brand = row["Brand"].ToString();
                brandSet.Add(brand); // Add to the HashSet to prevent duplicates
            }

            List<string> brandList = brandSet.ToList();
            brandList.Insert(0, "All"); // Add "All" as the first item

            Brands.ItemsSource = brandList; // Set ComboBox items
            Brands.SelectedIndex = 0;
        }
        private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            try {
                if(sender != null) {
                    DataGrid grid = sender as DataGrid;
                    if(grid!= null 
                        && grid.SelectedItem != null 
                        && grid.SelectedItems.Count == 1) {


                        DataRow row = (grid.SelectedItem as DataRowView).Row;

                        if(Home.IsAdmin)HandleAdmin(row);
                        else HandleCashier(row);

                    }
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } 
        }
        private void HandleAdmin(DataRow row) {
            CustomMessageBox msgBox = new CustomMessageBox();
            if (msgBox.ShowDialog() == true) {
                var choice= msgBox.SelectedOption;
                if(choice == CustomMessageBox.UserChoice.AddToCart) {
                    HandleCashier(row);
                }else if(choice == CustomMessageBox.UserChoice.Edit) {
                    Phone phone = new Phone((string)row["Brand"], (string)row["Model"], (string)row["Name"], (int)row["Ram"], (int)row["Storage"], (int)row["Price"]);
                    Console.WriteLine($"{phone.Brand}, {phone.Model}, {phone.Ram}, {phone.Storage}");
                    this.NavigationService.Navigate(new EditPhone(phone));
                }
            }


        }
        private void HandleCashier(DataRow row) {
            int quan = (int)row["Stock"];

            if ((int)row["Stock"] == 0) {
                MessageBox.Show("This item is out of stock!");
                return;
            }
            ItemsCart.Instance.AddPhoneToCart(row);
        }

        
        private void InOutOfStock_Changed(object sender, RoutedEventArgs e) {
            FilterData();
            
        }

        private void FilterData() {
            if(dtToFill == null) dtToFill = GetDataTable();
            
            DataView dv = new DataView(dtToFill);
            List<string> filter = new List<string>();
            if (InStock.IsChecked == true)
                filter.Add("Stock > 0");
           

            if (OutOfStock.IsChecked == true)
                filter.Add("Stock = 0");

            string selection = Brands.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selection) && selection != "All")
                filter.Add($"Brand = '{selection}'");

            dv.RowFilter = string.Join(" AND ", filter);

            dataGridToDisplay.ItemsSource = dv;
            Content_Box.Content = dataGridToDisplay;

        }
        ~ShowPhones() {
            
        }

    }
}
