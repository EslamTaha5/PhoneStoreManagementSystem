using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStoreManagementSystem {
    public class TransactionDetail {
        public int transactionID { get; }
        public int Ram { get; }
        public int Storage { get; }
        public int Price { get; }
        public int Quantity { get; }
        public string Brand { get; }
        public string Model;
        public string Query { get; }
        public TransactionDetail(int transactionID, string Brand, string Model, int Ram, int Storage, int Price, int Quantity) {
            this.Brand = Brand;
            this.Model = Model;
            this.Ram = Ram;
            this.Storage = Storage;
            this.Price = Price;
            this.Quantity = Quantity;
            this.transactionID = transactionID;
            Query = "insert into Transaction_Details(Trans_ID ,Brand, Model, Ram, Storage, Price, Quantity)" +
                   "Values(@transID, @Brand, @Model, @Ram, @Storage, @Price, @Quantity)";

        }
        public void addParametersValues(SqlCommand cmd) {
            cmd.Parameters.AddWithValue("@transID", this.transactionID);
            cmd.Parameters.AddWithValue("@Brand", this.Brand);
            cmd.Parameters.AddWithValue("@Model", this.Model);
            cmd.Parameters.AddWithValue("@Ram", this.Ram);
            cmd.Parameters.AddWithValue("@Storage", this.Storage);
            cmd.Parameters.AddWithValue("@Price", this.Price);
            cmd.Parameters.AddWithValue("@Quantity", this.Quantity);
        }

    }
}
