using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStoreManagementSystem {
    public class Phone {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }

        public int Ram { get; set; }
        public int Storage { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Phone(string Brand, string Model, string Name, int Ram, int Storage, int Price) {
            this.Brand = Brand;
            this.Model = Model;
            this.Ram = Ram;
            this.Name = Name;
            this.Storage = Storage;
            this.Price = Price;
            this.Quantity = 1;
        }
        public Phone(string Brand, string Model, int Ram, int Storage, int Quantity) {
            this.Brand=Brand;
            this.Model = Model; 
            this.Ram = Ram;
            this.Storage = Storage;
            this.Quantity = Quantity;

            this.Price = 0;
            this.Name = "";
        }
        public override string ToString() {
            return Brand + "-"
                + Model + "-"
                + Ram.ToString() + "-"
                + Storage.ToString();
        }

        public static Phone GetPhoneFromRow(DataRow row) {
            return new Phone(
                    (string)row["Brand"],
                    (string)row["Model"],
                    (string)row["Name"],
                    (int)row["Ram"],
                    (int)row["Storage"],
                    (int)row["Price"]
                );
        }
        public string GetPhoneQuery() {
            return $"select Model, Ram, Stoarge from Phones " +
                $"where Model = '{this.Model}' and Ram = '{this.Ram}' and Storage = '{this.Storage}'";
        }


    }
}
