using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneStoreManagementSystem {
    // just create one cart for cashier
    class ItemsCart {
        private HashSet<string> phonesInCart = null;//Phones added to cart
        public static DataTable phonesInCartData = null;
        public static ItemsCart _instance = null;
        public static int Total = 0;
        public static ItemsCart Instance {
            get {
                if (_instance == null)
                    _instance = new ItemsCart();
                return _instance;
            }
        }
        private ItemsCart() {
            phonesInCart = new HashSet<string>();
            phonesInCartData = ReqData.AllPhones.Clone();
            phonesInCartData.Columns.Add("Quantity");
        }

        public List<Phone> ItemCartToList() {
            List<Phone> ret = new List<Phone>();
            foreach(DataRow row in phonesInCartData.Rows) {
                Phone tmp = Phone.GetPhoneFromRow(row);
                tmp.Quantity = (int)row["Quantity"];
                ret.Add(tmp);
            }
            return ret;
        }

        public void AddPhoneToCart(DataRow row) {
            Phone ph = Phone.GetPhoneFromRow(row);
            if (!phonesInCart.Contains(ph.ToString())) {
                phonesInCart.Add(ph.ToString());
                phonesInCartData.ImportRow(row);
                phonesInCartData.Rows[phonesInCart.Count - 1]["Quantity"] = 1;
                
                
            }
            // Console.WriteLine(phonesInCartData.Rows.Count);
            MessageBox.Show($"{ph.Brand} {ph.Name} added to cart!");
            UpdateTotal();
        }

        public void RemovePhoneFromCart(DataRow row) {
            Phone ph = Phone.GetPhoneFromRow(row);
            Console.WriteLine(ph.ToString());
            phonesInCart.Remove(ph.ToString());
            phonesInCartData.Rows.Remove(row);
            UpdateTotal();
            MessageBox.Show($"{ph.Brand} {ph.Name} Removed from cart!");
           
        }

        public void UpdateTotal() {
            int tmpTotal = 0;
            Console.WriteLine("Update Total!");
            foreach (DataRow row in phonesInCartData.Rows) {
                int _price =(int)row["Price"];
                int _quantity = int.Parse((string)row["Quantity"]);
                tmpTotal += _price * _quantity;
            }
            Total = tmpTotal;
        }

        // We want to update restock in this page if we restocked an item
        public static void UpdateStockInCart(string phone, int stock) {
            foreach(DataRow row in phonesInCartData.Rows) {
                if(phone == Phone.GetPhoneFromRow(row).ToString()) {
                    row["Stock"] = (int)row["Stock"] + stock;
                    break;
                }
            }
        }
        public void Clear() {
            phonesInCart.Clear();
            phonesInCartData.Clear();
        }
    }
}
