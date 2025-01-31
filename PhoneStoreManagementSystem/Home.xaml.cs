﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PhoneStoreManagementSystem {
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window {
        private int lastChoice;
        public static int ID { get; set; }
        private readonly DispatcherTimer timer;

        public Home(string userName) {
            InitializeComponent();
            LoadData();
            SetUserName(userName);
            

            timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += UpdateTime;
            timer.Start();

            Console.WriteLine("Home Page initialized and Data Loaded!");
        }
        private void UpdateTime(object sender, EventArgs e) {
            time.Content = DateTime.Now.ToString("HH:mm:ss tt");
        }

        private void LoadData() {
            var _ = ReqData.Instance;
            var __ = ItemsCart.Instance;
        }

        private void SetUserName(string userName) {
            userNameDashboard.Content = userName;
        }

        private void SetTitle(string title) {
            pTitle.Content = title;
        }

        private void LoadPage(Page newPage) {
            Content_Box.Content = newPage;
            SetTitle(newPage.ToString());
        }

        private void TryLoadPage(int choice, Page page) {
            if (lastChoice == choice) return;
            lastChoice = choice;
            LoadPage(page);
        }

        private void Phones(object sender, RoutedEventArgs e) => TryLoadPage(1, new ShowPhones());

        private void Restock(object sender, RoutedEventArgs e) => TryLoadPage(2, new ReStock());

        private void MyDetails(object sender, RoutedEventArgs e) => TryLoadPage(3, new MyDetails());

        private void Cart(object sender, RoutedEventArgs e) => TryLoadPage(4, new Cart());

        private void TransactionDetails(object sender, RoutedEventArgs e) => TryLoadPage(5, new TransactionDetails());
    }
}
