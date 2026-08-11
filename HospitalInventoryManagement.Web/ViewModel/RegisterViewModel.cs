using System.ComponentModel.DataAnnotations;

namespace HospitalInventoryManagement.Web.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int HospitalID { get; set; } // Hastane seçimi

        // Yeni kullanıcının rolü: "User" veya "Admin"
        public string Role { get; set; } = "User";
    }
}