namespace toDoAPI.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(int userId);
    }
}
