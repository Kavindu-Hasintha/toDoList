namespace toDoAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> UserExist(int userId);
        Task<User> GetUserAsync(int userId);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}
