using toDoAPI.Enums;
using toDoAPI.Repositories.UserRepository;

namespace toDoAPI.Services.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly int RefreshTokenExpirationTime = 25;

        public RefreshTokenService(IHttpContextAccessor httpContextAccessor, IUserService userService, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userRepository = userRepository;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRandomToken(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(RefreshTokenExpirationTime)
            };

            return refreshToken;
        }

        private string GenerateRandomToken()
        {
            byte[] randomNumber = new byte[64];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<bool> SetRefreshTokenCookie(int userId, RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                Secure = true, // Set to true if your application is served over HTTPS
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            var isUpdated = await _userRepository.UpdateUserRefreshTokenAsync(userId, newRefreshToken.Token, newRefreshToken.Created, newRefreshToken.Expires);
        
            return isUpdated;
        }

        public async Task<OperationResult> DeleteRefreshTokenAsync()
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync();
                
                if (user == null)
                {
                    return OperationResult.Error;
                }

                var isDeleted = await _userRepository.DeleteRefreshTokenByUserAsync(user);

                if (!isDeleted)
                {
                    return OperationResult.Error;
                }
                return OperationResult.Success;
            } 
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }
    }
}
