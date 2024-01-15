namespace toDoAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> UserExist(int userId);
        Task<User> GetUserAsync(int userId);
    }
}
