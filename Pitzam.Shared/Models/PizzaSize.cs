using System.ComponentModel.DataAnnotations;

namespace Pitzam.Models
{
    public class PizzaSize
    {
        [Key]
        public int Id { get; set; }

        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }

        public string Size { get; set; } = "";
        public decimal Price { get; set; }
    }
}
