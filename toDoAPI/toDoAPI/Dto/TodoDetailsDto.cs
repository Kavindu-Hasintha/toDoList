namespace toDoAPI.Dto
{
    public class TodoDetailsDto
    {
        public int Id { get; set; }

        public string TaskName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int UserId { get; set; }
    }
}
