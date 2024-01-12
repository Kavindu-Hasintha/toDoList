namespace toDoAPI.Models
{
    public class ForgetPassword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string OTP { get; set; } = string.Empty;

        [Required]
        public bool IsOTPVerified { get; set; }

        [Required]
        public DateTime Expires { get; set; }
    }
}
