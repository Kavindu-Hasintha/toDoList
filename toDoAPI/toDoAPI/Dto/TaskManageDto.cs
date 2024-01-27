namespace toDoAPI.Dto
{
    public class TaskManageDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string EmailPassword { get; set; } = string.Empty;
    }
}
