using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Dto
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
