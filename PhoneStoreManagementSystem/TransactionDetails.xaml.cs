using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for TransactionDetails.xaml
    /// </summary>
    public partial class TransactionDetails : Page {
        public override string ToString() {
            return "Transaction Details";
        }

        public TransactionDetails() {
            InitializeComponent();
        }

        public TransactionDetails(int transactionID) {
            InitializeComponent();
            SetTransactionID(transactionID);
        }

        private void SetTransactionID(int transactionID) {
            transactionIDBox.Text = transactionID.ToString();
            FindTransaction();
        }

        private void PreviewText(object sender, TextCompositionEventArgs e) {
            Common.PreviewTextInput(sender, e);
        }
        
        private void PasteText(object sender, DataObjectPastingEventArgs e) {
            Common.BoxPasting(sender, e);
        }

        private void FindKeyDown(object sender, KeyEventArgs e) {
            Common.PreviewKeyDown(sender, e);
        }

        private void FindTransaction(object sender, RoutedEventArgs e) {
            FindTransaction();
        }

        private void FindTransaction() {
            if (!int.TryParse(transactionIDBox.Text, out int transactionID)) {
                MessageBox.Show("Invalid Transaction ID!");
                return;
            }

            if (!IsTransactionValid(transactionID)) {
                MessageBox.Show("This transaction doesn't exist!");
                return;
            }

            DisplayTransactionDetails(transactionID);
        }

        private bool IsTransactionValid(int transactionID) {
            string Query = @"SELECT COUNT(*) FROM Transactions
                             WHERE Cash_ID = @cashierID AND Trans_ID = @transactionID";
            return !Common.CountTransactionsForCashier(Query, transactionID);
        }

        private void DisplayTransactionDetails(int transactionID) {
            try {
                string Query = @"SELECT * FROM Transaction_Details WHERE Trans_ID = @transactionID";
                DataTable dt = Common.FindTransactionDetails(Query, transactionID);

                int total = CalculateTotal(dt);
                totalCost.Content = total;

                // Update existing DataGrid instead of creating a new one every time
                if (Content_Box.Content is DataGrid dg) {
                    dg.ItemsSource = dt.DefaultView;
                } else {
                    dg = new DataGrid {
                        IsReadOnly = true,
                        AutoGenerateColumns = true,
                        ItemsSource = dt.DefaultView,
                        Margin = new Thickness(10),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star)
                    };

                    Content_Box.Content = dg;
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error retrieving transaction details: {ex.Message}");
            }
        }

        private int CalculateTotal(DataTable dt) {
            int total = 0;
            foreach (DataRow row in dt.Rows) {
                total += (int)row["Quantity"] * (int)row["Price"];
            }
            return total;
        }
    }
}
