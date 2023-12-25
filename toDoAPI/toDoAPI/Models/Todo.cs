namespace toDoAPI.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string TaskName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
