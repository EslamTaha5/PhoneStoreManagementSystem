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

namespace PhoneStoreManagementSystem
{
    /// <summary>
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Page {
        public override string ToString() {
            return "Cart";
        }

        DataGrid cartGrid;

        public Cart() {
            Console.WriteLine("Cart Initialization");
            InitializeComponent();
            Display();
            UpdateTotalCart();
        }

        private void Display() {
            foreach (DataRow row in ItemsCart.phonesInCartData.Rows) {
                foreach (DataColumn col in ItemsCart.phonesInCartData.Columns) {
                    Console.WriteLine(row[col]);
                }
            }

            cartGrid = new DataGrid {
                AutoGenerateColumns = false,
                ItemsSource = ItemsCart.phonesInCartData.DefaultView,
                Margin = new Thickness(10),
                CanUserAddRows = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            foreach (DataColumn column in ItemsCart.phonesInCartData.Columns) {
                var dataGridColumn = new DataGridTextColumn {
                    Header = column.ColumnName,
                    Binding = new Binding($"{column.ColumnName}")
                };

                if (column.ColumnName == "Quantity") {
                    dataGridColumn.IsReadOnly = false;
                } else {
                    dataGridColumn.IsReadOnly = true;
                }

                cartGrid.Columns.Add(dataGridColumn);
            }


            Console.WriteLine(ItemsCart.phonesInCartData.Rows.Count);
            Content_Box.Content = cartGrid;
            cartGrid.MouseDoubleClick += CartDoubleClick;
            cartGrid.CellEditEnding += CartGridQuanEdit;
        }

        private void CartDoubleClick(object sender, MouseButtonEventArgs e) {
            try {
                if (sender != null) {
                    DataGrid dg = sender as DataGrid;
                    if (dg == null || dg.SelectedItem == null
                        || dg.SelectedItems.Count != 1) return;
                    DataRow row = (dg.SelectedItem as DataRowView).Row ;

                    ItemsCart.Instance.RemovePhoneFromCart(row);

                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            UpdateTotalCart();
        }

        private void CartGridQuanEdit(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.Column.Header.ToString() == "Quantity") {
                var quanBox = e.EditingElement as TextBox;
                if (quanBox != null) {
                    if (e.Row.DataContext is DataRowView rowView) {
                        DataRow row = rowView.Row;
                        var stock = row["Stock"]; 
                        
                        if (!int.TryParse(quanBox.Text, out int quantity) || quantity <= 0) {
                            MessageBox.Show("Invalid quantity. Please enter a valid positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            row["Quantity"] = 1; // Or reset to a default value

                        } else if(quantity > (int)stock) {
                            MessageBox.Show("Invalid quantity. Quantity is greater than Stock!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            row["Quantity"] = 1; // Or reset to a default value

                        } else {
                            Console.WriteLine($"Row Updated: Quantity: {quantity}, Stock: {stock}");
                            row["Quantity"] = quantity;
                        }
                        UpdateTotalCart();
                    }
                }
            }
        }

        void UpdateTotalCart() {
            ItemsCart.Instance.UpdateTotal();
            totalValue.Content = ItemsCart.Total;

        }

        private void makeTransaction(object sender, RoutedEventArgs e) {
            if (!ValidTransactions()) {
                MessageBox.Show("Invalid Transaction!");
                return;
            };
            MessageBoxResult res =  MessageBox.Show("Are you sure that you want to continue", "", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No) {
                return;
            }
            // Create Transaction
            Transaction transaction = CreateTransaction();
            User user = CreateUser();
            Cashier.Instance.MakeTransaction(transaction, user);

            MessageBox.Show("Transaction Completed!");
            Clear(this);
        }
        
        bool ValidTransactions() {

            if (customerID.Text == null
                || customerName.Text == null
                || customerPhone == null
                ||ItemsCart.phonesInCartData.Rows.Count == 0
                ||customerID.Text.Length == 0
                ||customerName.Text.Length == 0
                ||customerPhone.Text.Length == 0) {
                return false;
            }
            return true;
        }

        private Transaction CreateTransaction() {

            int cusID = int.Parse(customerID.Text);
            int cusPhone = int.Parse(customerPhone.Text);
            int Total = (int)totalValue.Content;
            return new Transaction(cusID, Home.ID, Total);

        }

        private User CreateUser() {
            int userID = int.Parse(customerID.Text);
            int userPhone = int.Parse(customerPhone.Text);
            string userName = customerName.Text;
            return new User(userName, userPhone, userID);
        }
        public static void Clear(Cart cur) {
            ItemsCart.Instance.Clear();
            cur.UpdateTotalCart();
        }

        private void Customer_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Common.PreviewTextInput(sender, e);
        }

        private void Customer_PreviewKeyDown(object sender, KeyEventArgs e) {
            Common.PreviewKeyDown(sender, e);
        }

        private void Customer_Pasting(object sender, DataObjectPastingEventArgs e) {
            Common.BoxPasting(sender, e);
        }
    }
}
