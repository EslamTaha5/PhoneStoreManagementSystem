using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Data.SqlClient;
namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for SignIn_Page.xaml
    /// </summary>
    public partial class SignIn_Page : Window {

        public SignIn_Page() {
            InitializeComponent();
        }


        string Get_user_name() {
            return txtUsername.Text;
        }
    

        string get_password() {
            return txtPassword.Password;
        }
        private void SignIn(object sender, RoutedEventArgs e) {

            SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=PhoneStore;Integrated Security=True;Trust Server Certificate=True");

            string inputUserName = Get_user_name();
            string inputHashedPassword = PasswordHelper.HashPassword(get_password());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"select * from Cashier where ID = @UserName and Password = @Password";
            cmd.Connection = conn;

            try {

                cmd.Parameters.AddWithValue("@UserName", inputUserName);
                cmd.Parameters.AddWithValue("@Password", inputHashedPassword);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                    
                if (dt.Rows.Count == 0) {
                    MessageBox.Show("Wrong Username or Password!");
                    return;
                }

                MessageBox.Show("Signed in successfully!");
                Home.ID = int.Parse(inputUserName);
               
                Home.IsAdmin = ((bool)dt.Rows[0]["IsAdmin"] == true);
                Home home = new Home((string)dt.Rows[0]["fName"]+' ' + (string)dt.Rows[0]["sName"]);
                home.Show();
                this.Close();

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                conn.Close();
            }
            
        }
        //Hash Password
        public class PasswordHelper {
            public static string HashPassword(string password) {
                using (var sha256 = SHA256.Create()) {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                    return Convert.ToBase64String(hashBytes);
                }
            }

            public static bool VerifyPassword(string inputPassword, string storedHash) {
                string hash = HashPassword(inputPassword);
                Console.WriteLine(hash);
                return hash == storedHash;
            }
        }

    }
}