using toDoAPI.Models;

namespace toDoAPI.Services.Users
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string email, string password);
        User GetUser(int userId);
        bool UserExist(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        bool IsEmailValid(string email);
    }
}
