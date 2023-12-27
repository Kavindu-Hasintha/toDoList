namespace toDoAPI.Services.JwtTokenService
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(User user);
    }
}
