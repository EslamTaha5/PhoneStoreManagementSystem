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
using System.Windows.Shapes;

namespace PhoneStoreManagementSystem
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public enum UserChoice { AddToCart, Edit, Cancel }
        public UserChoice SelectedOption { get; private set; } = UserChoice.Cancel;

        public CustomMessageBox() {
            InitializeComponent();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e) {
            SelectedOption = UserChoice.AddToCart;
            this.DialogResult = true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            SelectedOption = UserChoice.Edit;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            SelectedOption = UserChoice.Cancel;
            this.DialogResult = false;
        }
    }


}
