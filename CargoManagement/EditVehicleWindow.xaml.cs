using System;
using System.Windows;
using CargoManagement.Models;
using CargoManagement.Data;

namespace CargoManagement
{
    public partial class EditVehicleWindow : Window
    {
        private Vehicle _veh;
        public EditVehicleWindow(Vehicle veh)
        {
            InitializeComponent();
            _veh = veh;
            PlateBox.Text = veh.PlateNumber;
            ModelBox.Text = veh.Model;
            CapacityBox.Text = veh.Capacity.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PlateBox.Text) ||
                string.IsNullOrWhiteSpace(ModelBox.Text) ||
                !int.TryParse(CapacityBox.Text, out int cap))
            {
                MessageBox.Show("Заполните все поля и проверьте число.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _veh.PlateNumber = PlateBox.Text.Trim();
            _veh.Model = ModelBox.Text.Trim();
            _veh.Capacity = cap;
            DbHelper.UpdateVehicle(_veh);

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
