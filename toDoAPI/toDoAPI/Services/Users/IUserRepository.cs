namespace toDoAPI.Services.Users
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        //User GetUser(string email, string password);

        Task<int> GetUserId();

        Task<int> GetUserIdByEmailAsync(string email);

        Task<User> GetUserByEmailAsync();

        Task<Object> GetUserAsync(int userId);

        Task<User> GetUserAsync(string email);

        Task<User> GetUserAsyncByRefreshToken(string refreshToken);

        Task<string> GetTaskManageEmail();

        Task<string> GetTaskManagePassword();

        Task<bool> UserExist(int userId);

        Task<bool> UserExists(string email);

        Task<bool> RegisterUser(User user);

        Task<bool> UpdateUser(User user);

        Task<bool> UpdateUserRefreshToken(int userId, string newToken, DateTime created, DateTime expires);

        Task<bool> DeleteUser(User user);

        Task<bool> Save();

        Task<bool> IsEmailValid(string email);
    }
}
