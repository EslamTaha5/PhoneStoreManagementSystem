using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStoreManagementSystem
{
    public class User
    {
        public string userName { get; }
        public int userPhone { get; }
        public int userID { get; }
        public User(string userName, int userPhone, int userID) {
            this.userName = userName;
            this.userPhone = userPhone;
            this.userID = userID;
        }
    }
}
