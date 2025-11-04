namespace Pitzam.Models
{
    public class Pizza
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<PizzaSize> Sizes { get; set; } = new();
    }
}
