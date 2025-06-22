using CargoManagement.Data;
using CargoManagement.Models;
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

namespace CargoManagement
{
    public partial class EditDriverWindow : Window
    {
        private Driver _drv;

        public EditDriverWindow(Driver drv)
        {
            InitializeComponent();
            _drv = drv;

            // Заполняем поля
            NameBox.Text = drv.Name;
            LicenseBox.Text = drv.LicenseNumber;
            PhoneBox.Text = drv.Phone;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(LicenseBox.Text))
            {
                MessageBox.Show("Заполните ФИО и номер ВУ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Обновляем модель
            _drv.Name = NameBox.Text.Trim();
            _drv.LicenseNumber = LicenseBox.Text.Trim();
            _drv.Phone = string.IsNullOrWhiteSpace(PhoneBox.Text)
                                   ? null
                                   : PhoneBox.Text.Trim();

            // Сохраняем в БД
            DbHelper.UpdateDriver(_drv);

            // Закрываем окно с результатом OK
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Просто закрываем окно без сохранения
            DialogResult = false;
            Close();
        }
    }
}