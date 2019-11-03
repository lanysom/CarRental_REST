using CarRental.DataAccessLayer;
using CarRental.RESTfulService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.RESTfulService.Controllers
{
    public class CarsController : ApiController
    {
        private readonly CarRepository _carRepository;

        public CarsController()
        {
            _carRepository = new CarRepository(() =>
            {
                var connStrBldr = new SqlConnectionStringBuilder
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "CarRental",
                    IntegratedSecurity = true
                };
                var conn = new SqlConnection(connStrBldr.ConnectionString);
                conn.Open();
                return conn;
            });
        }


        public IEnumerable<CarResponse> GetRented()
        {
            return _carRepository.GetAll().Where(c=>c.Rented).Select(car => new CarResponse
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model
            });
        }

        public IEnumerable<CarResponse> GetAvailable(string location)
        {
            return _carRepository.GetAll().Where(c => c.Location == location).Select(car => new CarResponse
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model
            });
        }

        [HttpPut]
        public void Rent(RentCarRequest request)
        {
            _carRepository.RentCar(request.CarId, request.Employee);
        }

        [HttpPut]
        public void Return(ReturnCarRequest request)
        {
            _carRepository.ReturnCar(request.CarId, request.Employee, request.Location);
        }
    }
}
