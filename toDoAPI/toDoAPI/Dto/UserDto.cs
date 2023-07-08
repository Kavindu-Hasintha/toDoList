using System.ComponentModel.DataAnnotations;

namespace toDoAPI.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
