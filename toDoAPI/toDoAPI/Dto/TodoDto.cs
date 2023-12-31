using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Dto
{
    public class TodoDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string TaskName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
    }
}
