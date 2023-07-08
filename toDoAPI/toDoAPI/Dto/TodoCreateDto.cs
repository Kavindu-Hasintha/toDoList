namespace toDoAPI.Dto
{
    public class TodoCreateDto
    {
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
