using toDoAPI.Enums;

namespace toDoAPI.Services.Users
{
    public interface IUserService
    {
        ICollection<User> GetUsers();

        //User GetUser(string email, string password);

        Task<int> GetUserId();

        Task<UserDto> GetUserById(int userId);

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

        Task<UpdateUserResult> UpdateUserAsync(UserChangeDetailsDto userUpdate);

        Task<bool> UpdateUserRefreshToken(int userId, string newToken, DateTime created, DateTime expires);

        Task<bool> DeleteUser(User user);

        Task<bool> Save();

        Task<bool> IsEmailValid(string email);
    }
}
