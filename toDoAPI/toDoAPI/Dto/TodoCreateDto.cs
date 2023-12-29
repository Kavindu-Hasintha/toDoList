namespace toDoAPI.Dto
{
    public class TodoCreateDto
    {
        [Required]
        public string TaskName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
