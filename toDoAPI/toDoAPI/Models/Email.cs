namespace toDoAPI.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }


        [Required, EmailAddress]
        public string From { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string CC { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string BCC { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string To { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        [Required]
        public DateTime SendAt { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
