namespace toDoAPI.Repositories.UserRepository
{
    public interface IEmailRepository
    {
        Task<bool> AddEmail(string from, EmailDto request);
        Task<bool> Save();
    }
}
