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

namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for EditPhone.xaml
    /// </summary>
    public partial class EditPhone : Page {
        public override string ToString() {
            return "Edit Phone";
        }
        public EditPhone(Phone phone) {
            InitializeComponent();
            SetData(phone);

        }
        public EditPhone() {
            InitializeComponent();
        }

        void SetData(Phone phone) {
            brandLabel.Content = phone.Brand;
            modelLabel.Content = phone.Model;
            ramLabel.Content = phone.Ram;
            storageLabel.Content = phone.Storage;
            OldPrice.Content = phone.Price;
        }
        private void NPricePreviewKeyDown(object sender, TextCompositionEventArgs e) {
            Common.PreviewTextInput(sender, e);
        }
        private void NPricePreviewKeyDown(object sender, KeyEventArgs e) {
            Common.PreviewKeyDown(sender, e);

        }
        private void NPricePasting(object sender, DataObjectPastingEventArgs e) {
            Common.BoxPasting(sender, e);
        }
        private bool ValidChoice() {

            if (OldPrice.Content == "0")
                return false;
            return true;
        }
        private void DeleteDeviceBtn(object sender, RoutedEventArgs e) {
            if (!ValidChoice()) {
                MessageBox.Show("Invalid Choice");
                return;
            }
            string Model = (string)modelLabel.Content;
            int Ram = int.Parse((string)ramLabel.Content);
            int Storage = int.Parse((string)storageLabel.Content);
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to continue?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes) {
                Common.DeleteDevice(Model, Ram, Storage);
                MessageBox.Show("Device deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void EditDeviceBtn(object sender, RoutedEventArgs e) {
            if (!ValidChoice()) {
                MessageBox.Show("Invalid Choice");
                return;
            }
            Console.WriteLine("Editing Device");
            string Model = (string)modelLabel.Content;
            int Ram = (int)ramLabel.Content;
            int Storage = (int)storageLabel.Content;
            if (!int.TryParse(NewPrice.Text, out int newPrice)) {
                MessageBox.Show("Invalid Price");

            }
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to continue?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                Common.UpdatePrice(Model, Ram, Storage, newPrice);
                MessageBox.Show("Price Changed!");

            }


        }
    }
}
