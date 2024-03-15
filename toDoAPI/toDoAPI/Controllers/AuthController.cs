namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;
        private readonly IForgetPasswordService _forgetPasswordService;

        public AuthController(IUserService userService, IUserRepository userRepository, IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService, IEmailService emailService, IForgetPasswordService forgetPasswordService)
        {
            _userService = userService;
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

            bool isUserExists = await _userRepository.UserExistsByEmailAsync(request.Email);
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

            var isUserSaved = await _userService.RegisterUser(user);

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
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

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
                return StatusCode(500, "Internal Server Error!");
            }

            return Ok(new { token = token, refreshToken = refreshToken.Token, userRole = user.UserRole });
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

                var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

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
        [Route("getverificationcode")]
        public async Task<IActionResult> GetVerificationCode([FromQuery] string email)
        {
            try
            {
                if (email == null || email.Length == 0)
                {
                    return BadRequest();
                }

                var IsUserExists = await _userRepository.UserExistsByEmailAsync(email);

                if (!IsUserExists)
                {
                    return BadRequest();
                }

                int verificationCode = GetVerificationCode();

                var isOTPSaved = await _forgetPasswordService.SaveOTP(email, verificationCode.ToString());

                if (!isOTPSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                var taskManage = await _userRepository.GetTaskManageEmailPasswordAsync();

                string taskManageEmail = taskManage.Email;
                string taskManageEmailPassword = taskManage.EmailPassword;

                EmailDto emailRequest = new EmailDto();
                emailRequest.ToEmail = email;
                emailRequest.CC = string.Empty;
                emailRequest.BCC = string.Empty;
                emailRequest.Subject = "Verification Code - Task Management System";
                emailRequest.Body = verificationCode.ToString() + ", this is your Verification Code.";

                var isEmailSend = await _emailService.SendEmail(taskManageEmail, taskManageEmailPassword, emailRequest);

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
        [Route("forgetpasswordverification")]
        public async Task<IActionResult> ForgetPasswordVerification([FromQuery] string email, [FromQuery] string otp)
        {
            try
            {
                if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }

                var isEmailValid = await _userService.IsEmailValid(email);

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

                var user = await _userRepository.GetUserByEmailAsync(newPassword.Email);

                if (user == null)
                {
                    return BadRequest();
                }

                CreatePasswordHash(newPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                var isUserSaved = await _userRepository.UpdateUserAsync(user);

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

                if (otp.OTP != verificationCode)
                {
                    return BadRequest("Verification code is wrong");
                }

                if (otp.Expires < DateTime.UtcNow)
                {
                    return BadRequest("Verification token is expired");
                }

                if (otp.IsOTPVerified)
                {
                    return BadRequest("OTP is already verified");
                }
                /*
                if (otp.OTP != verificationCode || otp.Expires < DateTime.Now. || !otp.IsOTPVerified)
                {
                    return BadRequest("Not equal verification code or expired or OTP is verified");
                }
                */
                var isOTPDeleted = await _forgetPasswordService.DeleteOTP(email);

                if (!isOTPDeleted)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                var user = await _userRepository.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest();
                }

                user.IsVerified = true;

                var isUserSaved = await _userRepository.UpdateUserAsync(user);

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

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var result = await _refreshTokenService.DeleteRefreshTokenAsync();

                if (result == OperationResult.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { error = result });
                }
            } catch (Exception ex)
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

        private int GetVerificationCode()
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
