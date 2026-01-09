using System.ComponentModel.DataAnnotations;

namespace Pitzam.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public string PizzaName { get; set; } = "";
        public string Size { get; set; } = "";
        
        public List<string> Extras { get; set; } = new();
        public List<string> RemovedIngredients { get; set; } = new();
        
        public decimal TotalPrice { get; set; }
        
        public int? CustomerId { get; set; }
        public Customer? CustomerInfo { get; set; }
        
        public string OrderNumber { get; set; } = "";
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
