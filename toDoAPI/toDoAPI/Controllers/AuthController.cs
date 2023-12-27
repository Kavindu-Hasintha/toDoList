namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] UserRegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            bool isUserExists = await _userRepository.UserExists(request.Email);
            if (isUserExists)
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserRole = request.UserRole
            };

            var isUserSaved = await _userRepository.RegisterUser(user);

            if (!isUserSaved)
            {
                ModelState.AddModelError("UserError", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Registered!");
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDto request)
        {
            var user = await _userRepository.GetUserAsync(request.Email);

            if (user == null)
            {
                return BadRequest("Login failed!");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Login failed!");
            }

            string token = _jwtTokenService.CreateToken(user);

            var refreshToken = _refreshTokenService.GenerateRefreshToken();
            SetRefreshTokenCookie(refreshToken);

            return Ok(token);

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
