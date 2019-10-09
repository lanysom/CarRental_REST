using CarRental.DataAccessLayer.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CarRental.DataAccessLayer
{
    public class CarRepository
    {
        /// <summary>
        /// Gets a list of available cars
        /// </summary>
        public IEnumerable<Car> GetAvailable(string location = null)
        {
            const string SELECT_SQL = @"SELECT *
                                        FROM Cars
                                        WHERE Rented IS NULL";

            using (var conn = Database.Open())
            {
                var data = conn.Query<Car>(SELECT_SQL);
                if (location != null)
                {
                    return data.Where(c => c.Location == location);
                }
                return data;
            }
        }

        /// <summary>
        /// Gets a list of rented cars
        /// </summary>
        public IEnumerable<Car> GetRentedCars()
        {
            const string SELECT_SQL = @"SELECT *
                                        FROM Cars
                                        WHERE Rented IS NOT NULL";

            using (var conn = Database.Open())
            {
                return conn.Query<Car>(SELECT_SQL);
            }
        }

        /// <summary>
        /// Marks a car as rented in the database
        /// </summary>
        /// <param name="id">The identity of the car to be rented</param>
        /// <returns>A value indicating that the operation succeeded</returns>
        public bool RentCar(int id, string employee, string location)
        {
            const string UPDATE_SQL = @"UPDATE Cars 
                                        SET Rented = @rented, RentedBy = @employee, Returned = NULL, ReturnedBy = NULL, Location = NULL
                                        WHERE Id = @id AND Rented IS NULL AND Location = @location ";

            using (var conn = Database.Open())
            {
                var rows = conn.Execute(UPDATE_SQL, new { id, employee, location, rented = DateTime.Now });
                return rows == 1;
            }
        }

        /// <summary>
        /// Marks a car in the database as returned
        /// </summary>
        /// <param name="id">The identity of the car that is returned</param>
        /// <returns>A value indicating that the operation succeeded</returns>
        public bool ReturnCar(int id, string employee, string location)
        {
            const string UPDATE_SQL = @"UPDATE Cars 
                                        SET Rented = NULL, 
                                            RentedBy = NULL, 
                                            Returned = @returned, 
                                            ReturnedBy = @employee, 
                                            Location = @location 
                                        WHERE Id = @id AND Rented IS NOT NULL";

            using (var conn = Database.Open())
            {
                var rows = conn.Execute(UPDATE_SQL, new { id, employee, location, returned = DateTime.Now });
                return rows == 1;
            }
        }

        /// <summary>
        /// Adds a car to the database
        /// </summary>
        /// <param name="car">A car object that holds the data to be added</param>
        /// <returns>A value indicating that the operation succeeded</returns>
        public bool AddCar(Car car)
        {
            const string INSERT_SQL = "INSERT INTO Cars (Brand, Model) VALUES (@brand, @model);";

            using (var conn = Database.Open())
            {
                var rows = conn.Execute(INSERT_SQL, car);
                return rows == 1;
            }
        }

        /// <summary>
        /// Deletes a car from the database
        /// </summary>
        /// <param name="id">The identity of the car to delete</param>
        /// <returns>A value indicating that the operation succeeded</returns>
        public bool DeleteCar(int id)
        {
            const string DELETE_SQL = "DELETE FROM Cars WHERE Id = @id";

            using (var conn = Database.Open())
            {
                var rows = conn.Execute(DELETE_SQL, new { id });

                return rows == 1;
            }
        }
    }
}
