using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pitzam.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; } = "";
        
        // Liste görünümünde göstermek için en düşük boyut fiyatını hesaplayan salt-okunur özellik
        [NotMapped]
        public decimal BasePrice => Sizes?.OrderBy(s => s.Price).FirstOrDefault()?.Price ?? 0m;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        
        [NotMapped]
        public decimal Price { get; set; } // Bu, seçilen boyutun fiyatını tutacak

        // Stored as JSON string via ValueConverter
        public List<string> Ingredients { get; set; } = new();

        public ICollection<PizzaSize> Sizes { get; set; } = new List<PizzaSize>();
    }
}