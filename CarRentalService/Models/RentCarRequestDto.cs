using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalService.Models
{
    public class RentCarRequestDto
    {
        public int CarId { get; set; }
        public string Employee { get; set; }

    }

}