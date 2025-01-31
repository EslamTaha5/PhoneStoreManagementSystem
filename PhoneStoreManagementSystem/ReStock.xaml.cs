using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Data.SqlClient;
namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for ReStock.xaml
    /// </summary>
    public partial class ReStock : Page {
        public new string Title = "ReStock";
        public override string ToString() {
            return Title;
        }
        public ReqData LoadedData;
        public static DataTable dt = ReqData.AllPhones;
        public ReStock() {

            InitializeComponent();
            LoadMenus(); // initiallized in home Page
            Console.WriteLine("ReStock Loaded!");
        }

        void LoadMenus() {
            LoadedData = ReqData.Instance;

            SetBrandBox();
            /*
            foreach (DataRow row in dt.Rows) {
                foreach (DataColumn col in dt.Columns) {
                    Console.WriteLine(row[col.ColumnName]);
                }
            }
            */

        }

        private void SetBrandBox() {
            var uniqueBrands = dt.AsEnumerable()
                .Select(row => row.Field<string>("Brand"))
                .Distinct().ToList();

            brandCombo.ItemsSource = Common.GetUniqueBrands();
            brandCombo.FontSize = 20;
        }

        private void BrandCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
            SetModelBox((string)brandCombo.SelectedItem);
        }

        private void SetModelBox(string brand) {
            List<string> ls = Common.GetUniqueModels(brand);

            modelCombo.ItemsSource = ls;
            modelCombo.FontSize = 20;
        }

        private void ModelCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SetRam((string)modelCombo.SelectedItem);
        }

        private void SetRam(string model) {
            var rams = GetRam(model);
            ramCombo.ItemsSource = rams;
            ramCombo.FontSize = 20;
        }
        private List<int> GetRam(string model) {
            return Common.GetRamFromModel(model);
        }

        private void ramCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (modelCombo.SelectedItem == null || ramCombo.SelectedItem == null) {
                storageCombo.SelectedItem = null;
                return;
            }
            SetStorage((string)modelCombo.SelectedItem, (int)ramCombo.SelectedItem);
        }

        private void SetStorage(string model, int ram) {
            var storage = GetStorage(model, ram);
            storageCombo.ItemsSource = storage;
            storageCombo.FontSize = 20;
        }

        private List<int> GetStorage(string model, int ram) {
            return Common.GetStorageFromModelAndRam(model, ram);
        }



        private void StorageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Btn_Change();
        }
        private void restockQuantBox_TextChanged(object sender, TextChangedEventArgs e) {
            Btn_Change();
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private bool IsTextNumeric(string text) {
            return !_regex.IsMatch(text);
        }
        private void RestockQuantBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Common.PreviewTextInput(sender, e);
        }

        private void RestockQuantBox_Pasting(object sender, DataObjectPastingEventArgs e) {
            Common.BoxPasting(sender, e);
        }
        private void RestockQuantBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            Common.PreviewKeyDown(sender, e);
        }

        
        private void Btn_Change() {
            bool IsQuanFilled = int.TryParse((string)restockQuantBox.Text, out _);
            bool IsStorageSelected = storageCombo.SelectedItem != null;
            reStockBtn.IsEnabled = IsQuanFilled && IsStorageSelected; 
        }

        // Restock Button
        private void ReStockBtnClicked(object sender, RoutedEventArgs e) {
            
            string selectedBrand = (string)brandCombo.SelectedItem;
            string selectedModel = (string)modelCombo.SelectedItem;
            int selectedRam = (int)ramCombo.SelectedItem;
            int selectedStorage = (int)storageCombo.SelectedItem;
            int selectedQuant;
            int.TryParse((string)restockQuantBox.Text,out selectedQuant);
            Phone phone = new Phone(selectedBrand, selectedModel, selectedRam, selectedStorage, selectedQuant);
            //Console.WriteLine(selectedBrand + " " + selectedModel +" "+selectedRam + ' '+ selectedStorage + " " + selectedQuant);
            Common.EditStock(phone);
            ItemsCart.UpdateStockInCart(phone.ToString(), selectedQuant);
            MessageBox.Show("Restocked Successfully!");

            ResetAll();
        }
        void ResetAll() {
            brandCombo.SelectedItem = null;
        }
    }
}
