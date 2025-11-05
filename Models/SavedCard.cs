namespace Pitzam.Models
{
    public class SavedCard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Cardholder { get; set; } = string.Empty;
        // Sadece son 4 rakam saklanır
        public string Last4 { get; set; } = string.Empty;
        // AA/YY
        public string Expiry { get; set; } = string.Empty;
        public string Brand { get; set; } = "Card";
    }
}


