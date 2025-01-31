using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStoreManagementSystem {
    public class Transaction {
        public int customerID { get; }
        public int cashierID { get; }
        public int Total { get; }
        public int transactionID { get; }
        public DateTime Date { get; }

        public Transaction(int customerID, int cashierID, int Total) {
            this.cashierID = cashierID;
            this.customerID = customerID;
            this.Total = Total;
            this.Date = DateTime.Now;
            this.transactionID = Common.CountTransactions() + 1;
        }

        public void InsertTransaction() {
            Common.InsertTransaction(this);
            InsertDetails();
        }
        public void InsertDetails() {
            DataTable dt = ItemsCart.phonesInCartData;
            string Query;
            foreach (DataRow row in dt.Rows) {
                TransactionDetail td = new TransactionDetail(
                    transactionID,
                   (string)row["Brand"],
                    (string)row["Model"],
                    (int)row["Ram"],
                    (int)row["Storage"],
                    (int)row["Price"],
                    int.Parse((string)row["Quantity"])
                    );
                Common.InsertTransactionDetails(td);
                Console.WriteLine("After Detail insertion!");
            }
        }
        
    }
}

