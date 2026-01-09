namespace Pitzam.Server.Dtos
{
    public class PizzaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<PizzaSizeDto> Sizes { get; set; } = new();
    }

    public class PizzaSizeDto
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public string Size { get; set; } = "";
        public decimal Price { get; set; }
    }
}
