using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public ICollection<Todo> Todos { get; set; }
    }
}
