using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Models
{
    public class UserRegisterRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be between 6 and 24 characters in length.")]
        [MaxLength(24, ErrorMessage = "Password must be between 6 and 24 characters in length.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be between 6 and 24 characters in length.")]
        [MaxLength(24, ErrorMessage = "Password must be between 6 and 24 characters in length.")]
        [Compare("Password")]
        public string confirmPassword { get; set; } = string.Empty;

        [Required]
        public Role UserRole { get; set; }
    }
}
