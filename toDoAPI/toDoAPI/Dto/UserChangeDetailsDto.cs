using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Dto
{
    public class UserChangeDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
