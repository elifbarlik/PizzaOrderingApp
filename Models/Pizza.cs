// Models/Pizza.cs
using System.Linq;
namespace Pitzam.Models
{
    public class Pizza
    {
        public int Id { get; set; } 

        public string Name { get; set; } = "";
        
        // Liste görünümünde göstermek için en düşük boyut fiyatını hesaplayan salt-okunur özellik
        public decimal BasePrice => Sizes?.OrderBy(s => s.Price).FirstOrDefault()?.Price ?? 0m;

        // AddToOrder metodunda bu alan kullanılıyor
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        
        // AddToOrder metodunda bu alan kullanılıyor
        public decimal Price { get; set; } // Bu, seçilen boyutun fiyatını tutacak

        public List<string> Ingredients { get; set; } = new();
        public List<PizzaSize> Sizes { get; set; } = new();
    }
}