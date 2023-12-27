namespace toDoAPI.Services.Users
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        //User GetUser(string email, string password);

        User GetUser(int userId);

        Task<User> GetUserAsync(string email);

        bool UserExist(int userId);

        Task<bool> UserExists(string email);

        Task<bool> RegisterUser(User user);

        Task<bool> UpdateUser(User user);

        Task<bool> DeleteUser(User user);

        Task<bool> Save();

        bool IsEmailValid(string email);
    }
}
