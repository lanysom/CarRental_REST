using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_v3.RentalServiceReference
{
    partial class CarContract
    {
        public override string ToString()
        {
            return $"[{CarId}] - {Brand} {Model}";
        }
    }
}
