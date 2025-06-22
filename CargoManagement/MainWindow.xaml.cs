using System.Collections.Generic;
using System.Windows;
using CargoManagement.Data;
using CargoManagement.Models;

namespace CargoManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // По умолчанию при старте показываем раздел «Поездки»
            ShowTrips();
        }

        #region Навигация между разделами

        private void ShowTrips()
        {
            TripsSection.Visibility = Visibility.Visible;
            DriversSection.Visibility = Visibility.Collapsed;
            VehiclesSection.Visibility = Visibility.Collapsed;
            SettingsSection.Visibility = Visibility.Collapsed;

            LoadTrips();
        }

        private void ShowDrivers()
        {
            TripsSection.Visibility = Visibility.Collapsed;
            DriversSection.Visibility = Visibility.Visible;
            VehiclesSection.Visibility = Visibility.Collapsed;
            SettingsSection.Visibility = Visibility.Collapsed;

            LoadDrivers();
        }

        private void ShowVehicles()
        {
            TripsSection.Visibility = Visibility.Collapsed;
            DriversSection.Visibility = Visibility.Collapsed;
            VehiclesSection.Visibility = Visibility.Visible;
            SettingsSection.Visibility = Visibility.Collapsed;

            LoadVehicles();
        }

        private void ShowSettings()
        {
            TripsSection.Visibility = Visibility.Collapsed;
            DriversSection.Visibility = Visibility.Collapsed;
            VehiclesSection.Visibility = Visibility.Collapsed;
            SettingsSection.Visibility = Visibility.Visible;
        }

        private void TripsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowTrips();
        }

        private void DriversButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDrivers();
        }

        private void VehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowVehicles();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        #endregion


        #region Загрузка данных

        private void LoadTrips()
        {
            List<Trip> trips = DbHelper.GetAllTrips();
            TripsDataGrid.ItemsSource = trips;
        }

        private void LoadDrivers()
        {
            List<Driver> drivers = DbHelper.GetAllDrivers();
            DriversDataGrid.ItemsSource = drivers;
        }

        private void LoadVehicles()
        {
            List<Vehicle> vehicles = DbHelper.GetAllVehicles();
            VehiclesDataGrid.ItemsSource = vehicles;
        }

        #endregion


        #region CRUD для «Поездки»

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddTripWindow { Owner = this };
            if (win.ShowDialog() == true)
                LoadTrips();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (TripsDataGrid.SelectedItem is not Trip trip) return;
            var win = new EditTripWindow(trip) { Owner = this };
            if (win.ShowDialog() == true)
                LoadTrips();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TripsDataGrid.SelectedItem is not Trip trip) return;
            var result = MessageBox.Show(
                $"Удалить поездку №{trip.Id}?\n{trip.Origin} → {trip.Destination}",
                "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DbHelper.DeleteTrip(trip.Id);
                LoadTrips();
            }
        }

        #endregion


        #region CRUD для «Водители»

        private void Drivers_Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddDriverWindow { Owner = this };
            if (win.ShowDialog() == true)
                LoadDrivers();
        }

        private void Drivers_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DriversDataGrid.SelectedItem is not Driver drv) return;
            var win = new EditDriverWindow(drv) { Owner = this };
            if (win.ShowDialog() == true)
                LoadDrivers();
        }

        private void Drivers_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DriversDataGrid.SelectedItem is not Driver drv) return;
            var result = MessageBox.Show(
                $"Удалить водителя {drv.Name}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DbHelper.DeleteDriver(drv.Id);
                LoadDrivers();
            }
        }

        #endregion


        #region CRUD для «Техника»

        private void Vehicles_Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddVehicleWindow { Owner = this };
            if (win.ShowDialog() == true)
                LoadVehicles();
        }

        private void Vehicles_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (VehiclesDataGrid.SelectedItem is not Vehicle veh) return;
            var win = new EditVehicleWindow(veh) { Owner = this };
            if (win.ShowDialog() == true)
                LoadVehicles();
        }

        private void Vehicles_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (VehiclesDataGrid.SelectedItem is not Vehicle veh) return;
            var result = MessageBox.Show(
                $"Удалить технику {veh.PlateNumber} ({veh.Model})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DbHelper.DeleteVehicle(veh.Id);
                LoadVehicles();
            }
        }

        #endregion
    }
}
