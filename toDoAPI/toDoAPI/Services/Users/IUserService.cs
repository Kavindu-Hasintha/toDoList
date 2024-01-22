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

        Task<User> GetUserAsync(int userId);

        Task<User> GetUserAsync(string email);

        Task<User> GetUserAsyncByRefreshToken(string refreshToken);

        Task<string> GetTaskManageEmail();

        Task<string> GetTaskManagePassword();

        Task<bool> UserExist(int userId);

        Task<bool> UserExists(string email);

        Task<bool> RegisterUser(User user);

        Task<OperationResult> UpdateUserAsync(UserChangeDetailsDto userUpdate);

        Task<OperationResult> DeleteUserByEmailAsync();

        Task<OperationResult> DeleteUserByIdAsync(int userId);

        Task<OperationResult> DeleteUserAsync(User deleteUser);

        Task<bool> UpdateUserRefreshToken(int userId, string newToken, DateTime created, DateTime expires);

        Task<bool> Save();

        Task<bool> IsEmailValid(string email);
    }
}
