namespace toDoAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string EmailPassword { get; set; } = string.Empty;

        public bool IsVerified { get; set; }

        [Required]
        public Role UserRole { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpired { get; set; }

        public ICollection<Todo> Todos { get; set; }

        public ICollection<Email> Emails { get; set; }
    }
}
