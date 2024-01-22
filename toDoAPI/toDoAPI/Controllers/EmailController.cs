namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userRepository;

        public EmailController(IEmailService emailService, IUserService userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("sendemail")]
        [Authorize]
        public async Task<IActionResult> SendEmails([FromBody] EmailDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest();
                }

                var user = await _userRepository.GetUserByEmailAsync();

                var isSent = await _emailService.SendEmail(user.Email , user.EmailPassword, request);

                if (!isSent)
                {
                    return StatusCode(500, "Internal Server Error");
                }
                return Ok("Email sent");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
