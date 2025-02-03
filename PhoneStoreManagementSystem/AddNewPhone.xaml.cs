using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneStoreManagementSystem
{
    /// <summary>
    /// Interaction logic for AddNewPhone.xaml
    /// </summary>
    /// 
    public partial class AddNewPhone : Page
    {
        public override string ToString() {
            return "Add New Phone";
        }
        public AddNewPhone()
        {
            InitializeComponent();
        }

        private void DevicePreviewTextInput(object sender, TextCompositionEventArgs e) {
            Common.PreviewTextInput(sender, e);
        }
        private void DevicePreviewKeyDown(object sender, KeyEventArgs e) {
            Common.PreviewKeyDown(sender, e);

        }
        private void DeviceBoxPasting(object sender, DataObjectPastingEventArgs e) {
            Common.BoxPasting(sender, e);
        }
        bool IsValid(string Brand, string Mode, string Name) {
            return Brand.Length > 0 && Mode.Length > 0 && Name.Length > 0;
        }
        private void AddDevice(object sender, RoutedEventArgs e) {
            string Brand = BrandBox.Text;
            Brand = Brand.Trim();

            string Model = ModelBox.Text;
            Model = Model.Trim();

            string Name = NameBox.Text;
            Name = Name.Trim();

            bool valid = int.TryParse(RamBox.Text, out int Ram);
            valid &= int.TryParse(StorageBox.Text, out int Storage);
            valid &= int.TryParse(PriceBox.Text, out int Price);

            valid &= IsValid(Brand, Model, Name);
            valid &= (Ram <= 32);
            if (!valid) {
                MessageBox.Show("Invalid Data!");
                return;
            }

            Phone ph = new Phone(Brand, Model, Name, Ram, Storage, Price);
            Common.AddPhone(ph);
            MessageBox.Show("added successfully!");
        }

    }
}
