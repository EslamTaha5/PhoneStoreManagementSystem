using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Data;
namespace PhoneStoreManagementSystem {
    class Cashier {
        private static Cashier _instance = null;
        public static Cashier Instance {
            get {
                if (_instance == null)
                    _instance = new Cashier();
                return _instance;
            }
        }

        private Cashier() {

        }
        public void MakeTransaction(Transaction transaction, User user) {
            Common.InsertUser(user);
            transaction.InsertTransaction();
            // Insert Transaction Details
            
            EditPhonesStocks();
           
        }

        public void EditPhonesStocks() {
            foreach (DataRow row in ItemsCart.phonesInCartData.Rows) {
                Phone ph = Phone.GetPhoneFromRow(row);
                ph.Quantity = -(int.Parse((string)row["Quantity"]));
                Common.EditStock(ph);
            }
        }

        public DataTable GetCashierTransactions() {
            string Query = @"select * from Transactions 
                 where Cash_ID = @cashierID";
            return Common.GetCashierTransactions(Query);
        }
        public void AddParameters(SqlCommand cmd) {
            cmd.Parameters.AddWithValue("@cashierID", Home.ID);
        }

    }
}
