namespace toDoAPI.Services.JwtTokenService
{
    public interface IJwtTokenService
    {
        string CreateToken(User user);
    }
}
