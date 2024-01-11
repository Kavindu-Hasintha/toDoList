namespace toDoAPI.Dto
{
    public class EmailDto
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
