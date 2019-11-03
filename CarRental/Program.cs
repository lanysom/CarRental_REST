using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarRental
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = GetCarAsync("https://localhost:44336/api/cars/").GetAwaiter().GetResult();

            Console.WriteLine(result);
            
            var cars = JsonConvert.DeserializeObject<IEnumerable<Car>>(result);
            
            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }

            Console.ReadLine();
        }

        static async Task<string> GetCarAsync(string path)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
