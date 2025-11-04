using System.Net.Http.Json;
using Pitzam.Models;

namespace Pitzam.Services
{
    public class PizzaService
    {
        private readonly HttpClient _http;

        public PizzaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Pizza>> GetPizzasAsync()
        {
            var result = await _http.GetFromJsonAsync<List<Pizza>>("data/pizzas.json");
            return result ?? new List<Pizza>();
        }
    }
}
