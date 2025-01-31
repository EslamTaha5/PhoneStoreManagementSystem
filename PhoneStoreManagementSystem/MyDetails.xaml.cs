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

namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for MyDetails.xaml
    /// </summary>
    public partial class MyDetails : Page {
        public new string Title = "My Details";
        public override string ToString() {
            return Title;
        }
        public MyDetails() {
            InitializeComponent();
            Display();
            Console.WriteLine("Details Page loaded!");
        }
        DataTable dt = null;
        private void Display() {

            DataGrid dg = GetGrid();
            FillTotal();
            dg.MouseDoubleClick += Dg_MouseDoubleClick;
            Content_Box.Content = dg;
        }
        private DataGrid GetGrid() {
            dt = Cashier.Instance.GetCashierTransactions();

            return new DataGrid {
                IsReadOnly = true,
                ItemsSource = dt.DefaultView,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
        }

        private void FillTotal() {
            int total = 0;
            foreach (DataRow dr in dt.Rows) {
                total += (int)dr["Total"];
            }
            Total.Content = total;
        }

        private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            try {
                if (sender != null) {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null
                        && grid.SelectedItem != null
                        && grid.SelectedItems.Count == 1) {


                        DataRow row = (grid.SelectedItem as DataRowView).Row;
                        int transactioID = (int)row["Trans_ID"];
                        this.NavigationService.Navigate(new TransactionDetails(transactioID));


                    }
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
