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
            return await _http.GetFromJsonAsync<List<Pizza>>("api/pizzas") ?? new List<Pizza>();
        }

        public async Task<Pizza?> GetPizzaByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Pizza>($"api/pizzas/{id}");
        }
    }
}