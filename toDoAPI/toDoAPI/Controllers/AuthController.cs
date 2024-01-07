using toDoAPI.Models;
using toDoAPI.Services.ForgetPasswordService;
using toDoAPI.Services.RefreshTokenService;

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string TaskManageEmail = "taskmanage535@gmail.com";
        private readonly string EmailPassword = "jucfmflgxtnaujjo";

        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;
        private readonly IForgetPasswordService _forgetPasswordService;

        public AuthController(IUserRepository userRepository, IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService, IEmailService emailService, IForgetPasswordService forgetPasswordService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
            _emailService = emailService;
            _forgetPasswordService = forgetPasswordService;
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
                IsVerified = false,
                UserRole = request.UserRole
            };

            var isUserSaved = await _userRepository.RegisterUser(user);

            if (!isUserSaved)
            {
                ModelState.AddModelError("UserError", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Account created. You need to verify your account to log in.");
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

            string token = await _jwtTokenService.CreateToken(user);

            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var isSetRefreshToken = await _refreshTokenService.SetRefreshTokenCookie(user.Id, refreshToken);

            if (!isSetRefreshToken)
            {
                return BadRequest("Server error!");
            }

            return Ok(token);

        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized("Refresh token is missing");
                }

                var user = await _userRepository.GetUserAsyncByRefreshToken(refreshToken);

                if (user == null || !user.RefreshToken.Equals(refreshToken))
                {
                    return Unauthorized("Invalid refresh token");
                }

                if (user.TokenExpired < DateTime.UtcNow)
                {
                    return Unauthorized("Token expired");
                }

                string newToken = await _jwtTokenService.CreateToken(user);

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email)
        {
            try
            {
                if (email == null || email.Length == 0)
                {
                    return BadRequest();
                }

                var IsUserExists = await _userRepository.UserExists(email);

                if (!IsUserExists)
                {
                    return BadRequest();
                }

                int otp = GetOTP();

                var isOTPSaved = await _forgetPasswordService.SaveOTP(email, otp.ToString());

                if (!isOTPSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                string subject = "OTP - Task Management System";
                string body = otp.ToString() + ", this is your OTP.";

                var isEmailSend = await _emailService.SendEmail(TaskManageEmail, EmailPassword, email, subject, body);

                if (!isEmailSend)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("OTP has sent to your email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("otpverification")]
        public async Task<IActionResult> OTPVerification([FromQuery] string email, [FromQuery] string otp)
        {
            try
            {
                if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }

                var isEmailValid = await _userRepository.IsEmailValid(email);

                if (!isEmailValid)
                {
                    return BadRequest();
                }

                var forgetPassword = await _forgetPasswordService.GetOTPByUsingEmailAsync(email);

                if (forgetPassword == null)
                {
                    return BadRequest();
                }

                if (!forgetPassword.OTP.Equals(otp) || forgetPassword.Expires < DateTime.UtcNow)
                {
                    return BadRequest("OTP expired");
                }

                forgetPassword.IsOTPVerified = true;

                var isSaved = await _forgetPasswordService.UpdateForgetPassword(forgetPassword);

                if (!isSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("OTP verified");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("addnewpassword")]
        public async Task<IActionResult> AddNewPassword([FromBody] NewPassword newPassword)
        {
            try
            {
                if (newPassword == null)
                {
                    return BadRequest();
                }

                var isVerified = await _forgetPasswordService.IsOTPVerified(newPassword.Email);

                if (!isVerified)
                {
                    return BadRequest("OTP is not verified");
                }

                var user = await _userRepository.GetUserAsync(newPassword.Email);

                if (user == null)
                {
                    return BadRequest();
                }

                CreatePasswordHash(newPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                var isUserSaved = await _userRepository.UpdateUser(user);

                if (!isUserSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                var isOTPDeleted = await _forgetPasswordService.DeleteOTP(newPassword.Email);

                if (!isOTPDeleted)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("New password saved");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        [Route("verifyaccount")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string email, [FromQuery] string verificationCode)
        {
            try
            {
                if (string.IsNullOrEmpty(verificationCode))
                {
                    return BadRequest("Verification code is null or empty");
                }

                var otp = await _forgetPasswordService.GetOTPByUsingEmailAsync(email);

                if (otp == null)
                {
                    return BadRequest("OTP object is null");
                }

                if (!otp.OTP.Equals(verificationCode) || otp.Expires < DateTime.UtcNow || !otp.IsOTPVerified)
                {
                    return BadRequest("Not equal verification code or expired or OTP is verified");
                }

                var isOTPDeleted = await _forgetPasswordService.DeleteOTP(email);

                if (!isOTPDeleted)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                var user = await _userRepository.GetUserAsync(email);

                if (user == null)
                {
                    return BadRequest();
                }

                user.IsVerified = true;

                var isUserSaved = await _userRepository.UpdateUser(user);

                if (!isUserSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("Registration is success. Now, your account is verified.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        private int GetOTP()
        {
            Random _random = new Random();
            int otp = 0;
            lock (_random) // Ensure thread safety
            {
                otp = _random.Next(100000, 999999 + 1);
            }
            return otp;
        }
    }
}
