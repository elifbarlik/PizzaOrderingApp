using System.ComponentModel.DataAnnotations;

namespace Pitzam.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ad soyad en az 3 karakter olmalıdır.")]
        public string FullName { get; set; } = "";
        
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = "";
        
        public string? Phone { get; set; }
        
        public string? Address { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Kaydedilmiş adresler (Customer yapısı tekrar kullanılabilir)
        public List<Customer> SavedAddresses { get; set; } = new();

        // Kaydedilmiş kartlar (yalnızca güvenli alanlar saklanır)
        public List<SavedCard> SavedCards { get; set; } = new();
    }
}

