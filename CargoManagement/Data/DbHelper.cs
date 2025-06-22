using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using CargoManagement.Models;

namespace CargoManagement.Data
{
    public static class DbHelper
    {
        private static string ConnString =>
            ConfigurationManager.ConnectionStrings["CargoDb"].ConnectionString;

        public static List<Trip> GetAllTrips()
        {
            var list = new List<Trip>();
            using var conn = new MySqlConnection(ConnString);
            conn.Open();

            string sql = @"
      SELECT
        t.id, t.origin, t.destination, t.trip_date, t.status,
        t.driver_id, t.vehicle_id,
        d.name AS driver_name,
        v.plate_number AS vehicle_plate
      FROM trips t
      LEFT JOIN drivers d ON t.driver_id = d.id
      LEFT JOIN vehicles v ON t.vehicle_id = v.id
      ORDER BY t.id;
    ";
            using var cmd = new MySqlCommand(sql, conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Trip
                {
                    Id = rdr.GetInt32("id"),
                    Origin = rdr.GetString("origin"),
                    Destination = rdr.GetString("destination"),
                    TripDate = rdr.GetDateTime("trip_date"),
                    Status = rdr.GetString("status"),

                    DriverId = rdr.IsDBNull(rdr.GetOrdinal("driver_id"))
                     ? (int?)null
                     : rdr.GetInt32("driver_id"),

                    VehicleId = rdr.IsDBNull(rdr.GetOrdinal("vehicle_id"))
                     ? (int?)null
                     : rdr.GetInt32("vehicle_id"),

                    DriverName = rdr.IsDBNull(rdr.GetOrdinal("driver_name"))
                     ? ""
                     : rdr.GetString("driver_name"),

                    VehiclePlate = rdr.IsDBNull(rdr.GetOrdinal("vehicle_plate"))
                     ? ""
                     : rdr.GetString("vehicle_plate")
                });
            }
            return list;
        }

        public static void InsertTrip(Trip t)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            string sql = @"
      INSERT INTO trips
        (origin, destination, trip_date, status, driver_id, vehicle_id)
      VALUES
        (@o, @d, @dt, @st, @drv, @veh);
    ";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@o", t.Origin);
            cmd.Parameters.AddWithValue("@d", t.Destination);
            cmd.Parameters.AddWithValue("@dt", t.TripDate);
            cmd.Parameters.AddWithValue("@st", t.Status);
            cmd.Parameters.AddWithValue("@drv", t.DriverId.HasValue ? (object)t.DriverId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@veh", t.VehicleId.HasValue ? (object)t.VehicleId.Value : DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Обновляет существующую поездку по её Id.
        /// </summary>
        public static void UpdateTrip(Trip t)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            string sql = @"
      UPDATE trips SET
        origin      = @o,
        destination = @d,
        trip_date   = @dt,
        status      = @st,
        driver_id   = @drv,
        vehicle_id  = @veh
      WHERE id = @id;
    ";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@o", t.Origin);
            cmd.Parameters.AddWithValue("@d", t.Destination);
            cmd.Parameters.AddWithValue("@dt", t.TripDate);
            cmd.Parameters.AddWithValue("@st", t.Status);
            cmd.Parameters.AddWithValue("@drv", t.DriverId.HasValue ? (object)t.DriverId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@veh", t.VehicleId.HasValue ? (object)t.VehicleId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@id", t.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteTrip(int id)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            string sql = "DELETE FROM trips WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // Получить всех водителей
        public static List<Driver> GetAllDrivers()
        {
            var list = new List<Driver>();
            using var conn = new MySqlConnection(ConnString);
            conn.Open();

            const string sql = @"
      SELECT d.id,
             d.name,
             d.license_number,
             d.phone,
             COUNT(t.id) AS trip_count
        FROM drivers d
        LEFT JOIN trips t ON t.driver_id = d.id
       GROUP BY d.id, d.name, d.license_number, d.phone
       ORDER BY d.id;
    ";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Driver
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    LicenseNumber = reader.GetString("license_number"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone"))
                                      ? null
                                      : reader.GetString("phone"),
                    TripCount = reader.GetInt32("trip_count")
                });
            }

            return list;
        }


        // Вставить нового водителя
        public static void InsertDriver(Driver d)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            const string sql = @"
        INSERT INTO drivers (name, license_number, phone)
        VALUES (@name, @lic, @phone)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", d.Name);
            cmd.Parameters.AddWithValue("@lic", d.LicenseNumber);
            cmd.Parameters.AddWithValue("@phone", d.Phone ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        // Обновить водителя
        public static void UpdateDriver(Driver d)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            const string sql = @"
        UPDATE drivers SET
          name = @name,
          license_number = @lic,
          phone = @phone
        WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", d.Name);
            cmd.Parameters.AddWithValue("@lic", d.LicenseNumber);
            cmd.Parameters.AddWithValue("@phone", d.Phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", d.Id);
            cmd.ExecuteNonQuery();
        }

        // Удалить водителя
        public static void DeleteDriver(int id)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            using var cmd = new MySqlCommand("DELETE FROM drivers WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // Получить все машины
        public static List<Vehicle> GetAllVehicles()
        {
            var list = new List<Vehicle>();
            using var conn = new MySqlConnection(ConnString);
            conn.Open();

            const string sql = @"
      SELECT v.id,
             v.plate_number,
             v.model,
             v.capacity,
             COUNT(t.id) AS trip_count
        FROM vehicles v
        LEFT JOIN trips t ON t.vehicle_id = v.id
       GROUP BY v.id, v.plate_number, v.model, v.capacity
       ORDER BY v.id;
    ";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Vehicle
                {
                    Id = reader.GetInt32("id"),
                    PlateNumber = reader.GetString("plate_number"),
                    Model = reader.GetString("model"),
                    Capacity = reader.GetInt32("capacity"),
                    TripCount = reader.GetInt32("trip_count")
                });
            }

            return list;
        }


        // Вставить новую машину
        public static void InsertVehicle(Vehicle v)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            const string sql = @"
      INSERT INTO vehicles (plate_number, model, capacity)
      VALUES (@plate, @model, @cap)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@plate", v.PlateNumber);
            cmd.Parameters.AddWithValue("@model", v.Model);
            cmd.Parameters.AddWithValue("@cap", v.Capacity);
            cmd.ExecuteNonQuery();
        }

        // Обновить машину
        public static void UpdateVehicle(Vehicle v)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            const string sql = @"
      UPDATE vehicles
        SET plate_number = @plate,
            model        = @model,
            capacity     = @cap
      WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@plate", v.PlateNumber);
            cmd.Parameters.AddWithValue("@model", v.Model);
            cmd.Parameters.AddWithValue("@cap", v.Capacity);
            cmd.Parameters.AddWithValue("@id", v.Id);
            cmd.ExecuteNonQuery();
        }

        // Удалить машину
        public static void DeleteVehicle(int id)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            using var cmd = new MySqlCommand(
              "DELETE FROM vehicles WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }


    }
}
