using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoManagement.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime TripDate { get; set; }
        public string Status { get; set; }


        public string RussianStatus
        {
            get
            {
                // Приводим англ. статус к русскому
                return Status switch
                {
                    "Planned" => "Запланировано",
                    "On Route" => "В пути",
                    "Completed" => "Завершено",
                    _ => Status
                };
            }
        }

        // Новые поля
        public int? DriverId { get; set; }
        public int? VehicleId { get; set; }

        // Для отображения в гриде
        public string DriverName { get; set; }
        public string VehiclePlate { get; set; }
    }
}
