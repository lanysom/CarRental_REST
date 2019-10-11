using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalService.Models
{
    public class ReturnCarRequestDto
    {
        public int CarId { get; set; }
        public string Employee { get; set; }
        public string Location { get; set; }
        public Action Action { get; set; }
    }
}