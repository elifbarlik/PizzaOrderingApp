using System.Net.Http.Json;
using Pitzam.Models;

namespace Pitzam.Services
{
    public class PizzaService
    {
        private readonly HttpClient _http;

        // Pizzaları her seferinde JSON'dan çekmemek için bir önbellek (cache) oluşturalım.
        private List<Pizza>? _pizzasCache;

        public PizzaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Pizza>> GetPizzasAsync()
        {
            // Eğer önbellek boşsa, JSON'dan veriyi çek ve önbelleğe ata.
            if (_pizzasCache == null)
            {
                var result = await _http.GetFromJsonAsync<List<Pizza>>("data/pizzas.json");
                _pizzasCache = result ?? new List<Pizza>();
            }
            
            // Önbellekteki listeyi döndür.
            return _pizzasCache;
        }

        /// <summary>
        /// Verilen Id'ye göre bir pizzayı bulur.
        /// </summary>
        /// <param name="id">Aranan pizzanın Id'si</param>
        /// <returns>Bulunan Pizza nesnesi veya bulunamazsa null.</returns>
        public async Task<Pizza?> GetPizzaByIdAsync(int id)
        {
            // Tüm pizzaların önbelleğe yüklendiğinden emin ol.
            var allPizzas = await GetPizzasAsync();

            // Önbellekten ilgili pizzayı Id'ye göre bul.
            // FirstOrDefault, eşleşen ilk pizzayı, bulamazsa null (boş) döndürür.
            var pizza = allPizzas.FirstOrDefault(p => p.Id == id);
            
            return pizza;
        }
    }
}