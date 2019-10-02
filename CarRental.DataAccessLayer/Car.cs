using System;

namespace CarRental_v3
{
    /// <summary>
    /// A class representing a car
    /// </summary>
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime? Rented { get; set; }

        public override string ToString()
        {
            return $"[{Id}] - {Brand} {Model}";
        }
    }
}