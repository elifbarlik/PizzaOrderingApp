using System.ComponentModel.DataAnnotations;

namespace Pitzam.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ad soyad en az 3 karakter olmalıdır.")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Adres zorunludur.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Adres en az 5 karakter olmalıdır.")]
        public string Address { get; set; } = "";
    }
}
