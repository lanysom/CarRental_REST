using CarRental.DataAccessLayer;
using CarRentalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRentalService.Controllers
{
    public class CarsController : ApiController
    {
        private readonly ICarRepository _carRepository;

        public CarsController()
        {
            _carRepository = new CarRepository();
        }

        [HttpGet]
        public IEnumerable<CarResponseDto> GetRentedCars()
        {
            return _carRepository.GetRentedCars().Select(c => new CarResponseDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model
            });
        }

        [HttpGet]
        public IEnumerable<CarResponseDto> GetAvailableCars(string location)
        {
            return _carRepository.GetAvailable(location).Select(c => new CarResponseDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model
            });
        }

        [HttpPut]
        [Route("api/cars/rent")]
        public void RentCar(RentCarRequestDto request)
        {
            _carRepository.RentCar(request.CarId, request.Employee);
        }

        [HttpPut]
        [Route("api/cars/return")]
        public void ReturnCar(ReturnCarRequestDto request)
        {
            _carRepository.ReturnCar(request.CarId, request.Employee, request.Location);
        }
    }
}
