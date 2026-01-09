namespace Pitzam.Server.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string PizzaName { get; set; } = "";
        public string Size { get; set; } = "";
        public List<string> Extras { get; set; } = new();
        public List<string> RemovedIngredients { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public int? CustomerId { get; set; }
        public CustomerDto? CustomerInfo { get; set; }
        public string OrderNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
    }

    public class UserDto
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        // Password is never sent
        public List<CustomerDto> SavedAddresses { get; set; } = new();
        // SavedCards if needed, keeping simple for now or mapping if present
        // Model User.cs had SavedCards. Let's include if serialized previously.
        // User.cs: public List<SavedCard> SavedCards { get; set; } = new();
        // Need SavedCardDto
        public List<SavedCardDto> SavedCards { get; set; } = new();
    }

    public class SavedCardDto
    {
        public string Id { get; set; } = "";
        public string Cardholder { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Last4 { get; set; } = "";
        public string Expiry { get; set; } = "";
    }
}
