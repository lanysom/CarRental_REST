using CarRental_v3.RentalServiceReference;
using System;

namespace CarRental_v3
{
    class Program
    {
        static void Main(string[] args)
        {
            string msg = string.Empty;

            ClearScreen();

            Console.Write("Write your name >");
            string employee = Console.ReadLine();
            Console.Write("Write your location >");
            string location = Console.ReadLine();

            Menu();

            while (true)
            {
                Console.Write(">");

                if (Int32.TryParse(Console.ReadLine(), out int input))
                {
                    switch (input)
                    {
                        case 0:
                            // Exit program
                            return;
                        case 1:
                            // Show list of available cars
                            using (var client = new RentalServiceClient())
                            {
                                foreach (var car in client.GetAvailableCars(location))
                                {
                                    Console.WriteLine(car);
                                }
                            }
                            break;
                        case 2:
                            // Show list of rented cars
                            using (var client = new RentalServiceClient())
                            {
                                foreach (var car in client.GetUnavailableCars())
                                {
                                    Console.WriteLine(car);
                                }
                            }
                            break;
                        case 3:
                            // Rent car
                            Console.Write("Write id of car>");

                            if (Int32.TryParse(Console.ReadLine(), out int rentedCarId))
                            {
                                using (var client = new RentalServiceClient())
                                {
                                    msg = client.RentCar(rentedCarId, employee) ? "Car returned successfully" : "Something went wrong!";
                                }
                            }
                            else
                            {
                                msg = "Not a valid car id!";
                            }

                            ClearScreen(msg);
                            Menu();
                            break;
                        case 4:
                            // Return car
                            Console.Write("Write id of car>");

                            if (Int32.TryParse(Console.ReadLine(), out int returnCarId))
                            {
                                using (var client = new RentalServiceClient())
                                {
                                    msg = client.ReturnCar(returnCarId, employee, location) ? "Car returned successfully" : "Something went wrong!";
                                }
                            }
                            else
                            {
                                msg = "Not a valid car id!";
                            }

                            ClearScreen(msg);
                            Menu();
                            break;
                        default:
                            Console.WriteLine("Command not found!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command!");
                }
            }
        }

        static void Menu()
        {
            Console.WriteLine("Select a command:");
            Console.WriteLine(" [1] - Show available cars");
            Console.WriteLine(" [2] - Show rented cars");
            Console.WriteLine(" [3] - Rent a car");
            Console.WriteLine(" [4] - Return a car");
            Console.WriteLine();
        }

        private static void ClearScreen(string msg = null)
        {
            Console.Clear();

            Console.WriteLine("------------------------------------");
            Console.WriteLine(" Car Rental Application v2");
            Console.WriteLine("------------------------------------");
            Console.WriteLine();
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine(msg);
                Console.WriteLine();
            }
        }
    }
}
