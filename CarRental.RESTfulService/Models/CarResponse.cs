using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.RESTfulService.Models
{
    public class CarResponse
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool Rented { get; set; }
    }
}