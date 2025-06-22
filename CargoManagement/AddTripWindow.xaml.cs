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
using System.Collections.Generic;
using System.Windows;


namespace CargoManagement
{
    /// <summary>
    /// Логика взаимодействия для AddTripWindow.xaml
    /// </summary>
    public partial class AddTripWindow : Window
    {
        public AddTripWindow()
        {
            InitializeComponent();
            DriverCombo.ItemsSource = DbHelper.GetAllDrivers();
            VehicleCombo.ItemsSource = DbHelper.GetAllVehicles();
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

            var selectedItem = (ComboBoxItem)StatusComboBox.SelectedItem;

            if (DriverCombo.SelectedItem is null || VehicleCombo.SelectedItem is null)
            {
                MessageBox.Show("Выберите водителя и технику.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Создаём модель и сохраняем
            var trip = new Trip
            {
                Origin = OriginTextBox.Text,
                Destination = DestinationTextBox.Text,
                TripDate = DatePicker.SelectedDate.Value,
                DriverId = (int)DriverCombo.SelectedValue,
                VehicleId = (int)VehicleCombo.SelectedValue,


                // Берём код из Tag, а не из Content
                Status = selectedItem.Tag.ToString()
            };

            // Вставляем в БД
            DbHelper.InsertTrip(trip);

            // Сигнализируем успех и закрываем окно
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
