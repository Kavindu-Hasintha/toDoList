namespace toDoAPI.Dto
{
    public class EmailDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string ToEmail { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string CC { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string BCC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
