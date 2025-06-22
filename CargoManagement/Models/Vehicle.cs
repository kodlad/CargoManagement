using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoManagement.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }

        public int TripCount { get; set; }

    }
}

