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
using CargoManagement.Data;
using CargoManagement.Models;
using System.Windows;
using System.Windows.Controls;


namespace CargoManagement
{
    /// <summary>
    /// Логика взаимодействия для EditTripWindow.xaml
    /// </summary>
    public partial class EditTripWindow : Window
    {
        private Trip _trip;

        public EditTripWindow(Trip trip)
        {
            InitializeComponent();

            _trip = trip;

            var drivers = DbHelper.GetAllDrivers();
            var vehicles = DbHelper.GetAllVehicles();
            DriverCombo.ItemsSource = drivers;
            VehicleCombo.ItemsSource = vehicles;
            if (trip == null)
                throw new ArgumentException(nameof(trip), "Поездки нету");

            // Установить текущее
            DriverCombo.SelectedValue = _trip.DriverId;
            VehicleCombo.SelectedValue = _trip.VehicleId;
            

          
            // Заполняем поля значениями из переданного trip
            OriginTextBox.Text = trip.Origin;
            DestinationTextBox.Text = trip.Destination;
            DatePicker.SelectedDate = trip.TripDate;

            // Устанавливаем выбранный статус по tag
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if ((string)item.Tag == trip.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(OriginTextBox.Text) ||
                string.IsNullOrWhiteSpace(DestinationTextBox.Text) ||
                !DatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Обновляем поля в _trip
            _trip.Origin = OriginTextBox.Text;
            _trip.Destination = DestinationTextBox.Text;
            _trip.TripDate = DatePicker.SelectedDate.Value;
            _trip.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString();
            _trip.DriverId = (int)DriverCombo.SelectedValue;
            _trip.VehicleId = (int)VehicleCombo.SelectedValue;

            // Обновляем в БД
            DbHelper.UpdateTrip(_trip);

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
