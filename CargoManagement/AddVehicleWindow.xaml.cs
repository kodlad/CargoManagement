using System;
using System.Windows;
using CargoManagement.Models;
using CargoManagement.Data;

namespace CargoManagement
{
    public partial class AddVehicleWindow : Window
    {
        public AddVehicleWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PlateBox.Text) ||
                string.IsNullOrWhiteSpace(ModelBox.Text) ||
                !int.TryParse(CapacityBox.Text, out int cap))
            {
                MessageBox.Show("Заполните все поля и убедитесь, что грузоподъёмность — число.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var v = new Vehicle
            {
                PlateNumber = PlateBox.Text.Trim(),
                Model = ModelBox.Text.Trim(),
                Capacity = cap
            };
            DbHelper.InsertVehicle(v);

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
