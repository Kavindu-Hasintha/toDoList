using toDoAPI.Enums;

namespace toDoAPI.Services.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        RefreshToken GenerateRefreshToken();

        Task<bool> SetRefreshTokenCookie(int userId, RefreshToken newRefreshToken);

        Task<OperationResult> DeleteRefreshTokenAsync();
    }
}
