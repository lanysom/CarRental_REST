using System;

namespace CarRental.DataAccessLayer.Model
{
    /// <summary>
    /// A class representing a car
    /// </summary>
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool Rented { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"[{Id}] - {Brand} {Model}";
        }
    }
}