using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
namespace PhoneStoreManagementSystem {
    public class Common {
        private static string connectionString = "Data Source=.;Initial Catalog=PhoneStore;Integrated Security=True;Trust Server Certificate=True";
        private static SqlConnection createDataBaseConnection() {
            return new SqlConnection(connectionString);
        } 

        public static SqlCommand CreateCommand(string Query) {
            return new SqlCommand {
                CommandType = System.Data.CommandType.Text,
                CommandText = Query,
            };

        }

        private static void LinkCommandConnection(SqlCommand command, SqlConnection conn) {
            command.Connection = conn;
        }

        public static DataTable ExecQuery(string Query) {
            SqlConnection conn = createDataBaseConnection();
            SqlCommand cmd = CreateCommand(Query);

            LinkCommandConnection(cmd, conn);
            Console.WriteLine("Connection and Cmd Created");
            try {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            } finally {
                conn.Close();
            }
        }

        public static DataTable ExecQueryByCommand(SqlCommand cmd) {
            SqlConnection conn = createDataBaseConnection();
            LinkCommandConnection(cmd, conn);
            try {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            } finally {
                conn.Close();
            }
        }
        private static void ExecNonQuery(SqlCommand cmd) {
            SqlConnection conn = createDataBaseConnection();
            LinkCommandConnection(cmd, conn);

            try {
                conn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                conn.Close();
            }
        }
        public static void EditStock(Phone phone) {
            string Query = "update Phones " +
                "set Stock += @Quan " +
                "where Brand = @Brand and Model = @Model and Ram = @Ram and Storage = @Storage";

            SqlCommand cmd = CreateCommand(Query);

            cmd.Parameters.AddWithValue("@Quan", phone.Quantity);
            cmd.Parameters.AddWithValue("@Brand", phone.Brand);
            cmd.Parameters.AddWithValue("@Model", phone.Model);
            cmd.Parameters.AddWithValue("@Ram", phone.Ram);
            cmd.Parameters.AddWithValue("@Storage", phone.Storage);
            ExecNonQuery(cmd);

        }

        public static int CountTransactions() {
            string Query = "select count(*) as cnt from Transactions";
            var table = ExecQuery(Query);

            return (int)table.Rows[0]["cnt"];
        }

        public static void InsertTransaction(Transaction transaction) {
            string Query = "insert into Transactions(Trans_ID, Cus_ID, Cash_ID, Time, Total)" +
                "Values(@transactionID,@cusID, @cashierID, @Time, @Total)";
            SqlCommand cmd = CreateCommand(Query);

            cmd.Parameters.AddWithValue("@cusID", transaction.customerID);
            cmd.Parameters.AddWithValue("@cashierID", transaction.cashierID);
            cmd.Parameters.AddWithValue("@transactionID", transaction.transactionID);
            cmd.Parameters.AddWithValue("@Time", transaction.Date);
            cmd.Parameters.AddWithValue("@Total", transaction.Total);
            ExecNonQuery(cmd);
        }

        public static void InsertUser(User user) {
            string Query = @"
                            INSERT INTO Customer (ID, Name, Pnum)
                            SELECT @userID, @userName, @userPhone
                            WHERE NOT EXISTS (
                                SELECT 1 FROM Customer WHERE ID = @userID
                            )";
            SqlCommand cmd = CreateCommand(Query);
            

            cmd.Parameters.AddWithValue("@userID", user.userID);
            cmd.Parameters.AddWithValue("@userName", user.userName);
            cmd.Parameters.AddWithValue("@userPhone", user.userPhone);
            ExecNonQuery(cmd);

        }

        public static void InsertTransactionDetails(TransactionDetail transaction) {
            SqlCommand cmd = CreateCommand(transaction.Query);
            transaction.addParametersValues(cmd);
            ExecNonQuery(cmd);
        }

        public static DataTable GetCashierTransactions(string Query) {
            SqlCommand cmd = CreateCommand(Query);
            Cashier.Instance.AddParameters(cmd);
            return ExecQueryByCommand(cmd);
        }

        public static bool CountTransactionsForCashier(string Query, int transactionID) {
            SqlCommand cmd = CreateCommand(Query);
            cmd.Parameters.AddWithValue("@cashierID", Home.ID);
            cmd.Parameters.AddWithValue("@transactionID", transactionID);

            DataTable dt = ExecQueryByCommand(cmd);

            if (dt == null || dt.Rows.Count == 0) return true;

            return Convert.ToInt32(dt.Rows[0][0]) == 0;
        }

        public static DataTable FindTransactionDetails(string Query, int transactionID) {
            SqlCommand cmd = CreateCommand(Query);
            cmd.Parameters.AddWithValue("@transactionID", transactionID);
            return ExecQueryByCommand(cmd);
        }

        public static DataTable InStockPhones() {
            string Query = @"select * from Phones";
            DataTable dt = ExecQuery(Query);
            return dt;
        }
        //Make box for Numbers only

        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static bool IsTextNumeric(string text) {
            return !_regex.IsMatch(text);
        }
        public static void PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextNumeric(e.Text);

        }
        public static void BoxPasting(object sender, DataObjectPastingEventArgs e) {
            if (e.DataObject.GetDataPresent(typeof(string))) {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(text)) {
                    e.CancelCommand();
                }
            } else {
                e.CancelCommand();
            }
        }
        public static void PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                e.Handled = true;
            }
        }


        public static List<string> GetUniqueBrands() {
            var uniqueBrands = ReStock.dt.AsEnumerable()
                .Select(row => row.Field<string>("Brand"))
                .Distinct().ToList();
            return uniqueBrands;
        }

        public static List<string> GetUniqueModels(string brand) {
            var uniqueModels = ReStock.dt.AsEnumerable()
                .Where(row => row.Field<string>("Brand") == brand)
                .Select(row => row.Field<string>("Model"))
                .Distinct()
                .ToList();

            return uniqueModels;
        }

        public static List<int> GetRamFromModel(string model) {

            var ram = ReStock.dt.AsEnumerable().
                             Where(x => x.Field<string>("Model") == model).
                             Select(item => new {
                                 Ram = item.Field<int>("Ram"),
                             }).ToList();
            List<int> lst = ram.Select(item => item.Ram).Distinct().ToList();
            return lst;
        }

        public static List<int> GetStorageFromModelAndRam(string model, int ram) {
            var Storage = ReStock.dt.AsEnumerable().
                                Where(x => x.Field<string>("Model") == model
                                 && x.Field<int>("Ram") == ram).
                                Select(item => new {
                                    Storage = item.Field<int>("Storage")
                                });
            List<int> lst = Storage.Select(item => item.Storage).Distinct().ToList();
            return lst;
        }


    }
}


