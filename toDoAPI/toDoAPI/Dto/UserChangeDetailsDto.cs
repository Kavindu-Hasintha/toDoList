using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Dto
{
    public class UserChangeDetailsDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } 
    }
}
