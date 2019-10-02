using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_v3.DataAccessLayer
{
    public class CarRepository
    {
        /// <summary>
        /// Gets a list of available cars
        /// </summary>
        public IEnumerable<Car> GetAvailableCars(string location = null)
        {
            const string SELECT_SQL = @"SELECT *
                                        FROM Cars
                                        WHERE Rented IS NULL";

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                if (location == null)
                {
                    cmd.CommandText = SELECT_SQL;
                }
                else
                {
                    cmd.CommandText = $"{SELECT_SQL} AND Location = @location";
                    cmd.Parameters.AddWithValue("location", location);
                }

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    object[] values = new object[5];
                    var columns = reader.GetValues(values);

                    yield return MapCar(reader);
                }
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

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = SELECT_SQL;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    object[] values = new object[5];
                    var columns = reader.GetValues(values);

                    yield return MapCar(reader);
                }
            }
        }

        /// <summary>
        /// Marks a car as rented in the database
        /// </summary>
        /// <param name="id">The identity of the car to be rented</param>
        /// <returns>A value indicating that the operation succeeded</returns>
        public bool RentCar(int id, string employee, string location)
        {
            // NOTE: The only thing that is changed regarding to concurrency control is the condition of when to update. By adding 
            //       the extra check where the Rented column has to be null, you make sure that the operation not will alter data 
            //       if Fritz updated the table before you.

            const string UPDATE_SQL = @"UPDATE Cars 
                                        SET Rented = @rented, RentedBy = @employee, Returned = NULL, ReturnedBy = NULL, Location = NULL
                                        WHERE Id = @id AND Rented IS NULL AND Location = @location ";

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = UPDATE_SQL;

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("rented", DateTime.Now);
                cmd.Parameters.AddWithValue("employee", employee);
                cmd.Parameters.AddWithValue("location", location);

                var rows = cmd.ExecuteNonQuery();

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

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = UPDATE_SQL;

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("returned", DateTime.Now);
                cmd.Parameters.AddWithValue("employee", employee);
                cmd.Parameters.AddWithValue("location", location);

                var rows = cmd.ExecuteNonQuery();

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

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = INSERT_SQL;

                var brandParam = cmd.CreateParameter();
                brandParam.Value = car.Brand;
                brandParam.ParameterName = "brand";
                cmd.Parameters.Add(brandParam);

                var modelParam = cmd.CreateParameter();
                modelParam.Value = car.Model;
                modelParam.ParameterName = "model";
                cmd.Parameters.Add(modelParam);

                var rows = cmd.ExecuteNonQuery();

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

            using (var conn = OpenDbConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = DELETE_SQL;

                var idParam = cmd.CreateParameter();
                idParam.Value = id;
                idParam.ParameterName = "id";
                cmd.Parameters.Add(idParam);

                var rows = cmd.ExecuteNonQuery();

                return rows == 1;
            }
        }

        #region Helper methods

        /// <summary>
        /// Creates and returns an open IDbConnection object
        /// </summary>
        private static SqlConnection OpenDbConnection()
        {
            var conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CarRental_v3;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Maps the data from the reader into a car object
        /// </summary>
        /// <param name="reader">The reader containing the data</param>
        /// <returns>A new Car object with data from the reader</returns>
        private static Car MapCar(IDataReader reader)
        {
            return new Car
            {
                Id = Convert.ToInt32(reader["Id"]),
                Brand = reader["Brand"] as string,
                Model = reader["Model"] as string,
                Rented = reader["Rented"] as DateTime?
            };
        }
        #endregion
    }
}
