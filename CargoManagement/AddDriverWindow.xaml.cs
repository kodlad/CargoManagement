using System;
using System.Windows;
using CargoManagement.Models;
using CargoManagement.Data;

namespace CargoManagement
{
    public partial class AddDriverWindow : Window
    {
        public AddDriverWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(LicenseBox.Text))
            {
                MessageBox.Show("Заполните ФИО и номер ВУ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var d = new Driver
            {
                Name = NameBox.Text,
                LicenseNumber = LicenseBox.Text,
                Phone = string.IsNullOrWhiteSpace(PhoneBox.Text) ? null : PhoneBox.Text
            };
            DbHelper.InsertDriver(d);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}