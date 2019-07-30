using System.ComponentModel.DataAnnotations;
using Windows_Backend.Enums;

namespace Windows_Backend.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
        
        [Required]
        public UserType UserType { get; set; }
        
        public BusinessDTO Business { get; set; }
    }
}