namespace Pitzam.Models
{
    public class Order
    {
        public string PizzaName { get; set; } = "";
        public string Size { get; set; } = "";
        public List<string> Extras { get; set; } = new();
        public List<string> RemovedIngredients { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public Customer? CustomerInfo { get; set; }
        public string OrderNumber { get; set; } = "";
    }
}
