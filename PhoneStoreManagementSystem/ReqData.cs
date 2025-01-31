using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStoreManagementSystem
/// <summary>
/// This class loads all mobile data from the database and organizes it. 
/// It includes:
/// - A list of all mobiles.
/// - A hash set containing all distinct brand names.
/// - A hash set containing all distinct model names.
/// - A dictionary mapping each model to its available memory and storage options.
/// - We do not need every time we go to Restock page 
/// to get data from database so we will hold all information here.
/// </summary>
{
    public class ReqData {


        public static DataTable AllPhones = null;
        Dictionary<string, Phone> nameToPhone = null;
        private static ReqData _instance = null;
        public static ReqData Instance {
            get {
                if (_instance == null) {
                    _instance = new ReqData();
                }
                return _instance;
            }
        }

        private ReqData() {
            LoadPhones();
            SetNameToPhone();
        }
        void SetNameToPhone() {
            nameToPhone = new Dictionary<string, Phone>();
            Phone tmpPhone = null;
            foreach(DataRow row in AllPhones.Rows) {
                tmpPhone = Phone.GetPhoneFromRow(row);
                nameToPhone.TryAdd(tmpPhone.ToString(), tmpPhone);
                Console.WriteLine(tmpPhone.ToString());

            }
        }

        private void LoadPhones() {
            string Query = "Select * from Phones";
            AllPhones = Common.ExecQuery(Query);
        }

    }
}
