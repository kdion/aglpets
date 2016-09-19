using System;
using System.Linq;
using System.Net.Http;

namespace AglPetsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string url = "http://agl-developer-test.azurewebsites.net/people.json";
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var people = response.Content.ReadAsAsync<Person[]>().Result;

            var petsByOwnerGender =
                    from person in people
                    where person.Pets != null
                    from pet in person.Pets
                    where pet.Type == "Cat"
                    group pet by person.Gender into g
                    select new { OwnerGender = g.Key, PetNames = g.Select(p => p.Name).OrderBy(n => n) };

            foreach (var genderGroup in petsByOwnerGender)
            {
                Console.WriteLine(genderGroup.OwnerGender);
                foreach (var petName in genderGroup.PetNames)
                {
                    Console.WriteLine($" - {petName}");
                }
            }

            Console.ReadLine();
        }
    }
}
