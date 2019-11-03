using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.RESTfulService.Models
{
    public class RentCarRequest
    {
        public int CarId { get; set; }
        public string Employee { get; set; }
    }
}