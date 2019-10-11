using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalService.Models
{
    public class CarResponseDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
    }
}