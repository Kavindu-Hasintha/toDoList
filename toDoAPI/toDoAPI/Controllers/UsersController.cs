using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using toDoAPI.Dto;
using toDoAPI.Models;
using toDoAPI.Services.Users;

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId) 
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (userCreate.Name.Length == 0 || userCreate.Email.Length == 0 || userCreate.Password.Length == 0)
            {
                ModelState.AddModelError("UserError", "Please fill all the fields.");
                return StatusCode(422, ModelState);
            }

            var emailValidation = new EmailAddressAttribute();

            if (!emailValidation.IsValid(userCreate.Email))
            {
                ModelState.AddModelError("UserError", "Invalid email address.");
                return StatusCode(422, ModelState);
            }

            var user = _userRepository.GetUsers()
                .Where(x => x.Email == userCreate.Email).FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("UserError", "User already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Account created successfully.");
        }
        
        [HttpPost("getUserId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult GetUserId([FromBody] UserSignInDto userSignIn)
        {
            if (userSignIn == null)
            {
                return BadRequest(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _userRepository.GetUser(userSignIn.Email, userSignIn.Password);

            if (user == null)
            {
                ModelState.AddModelError("LoginError", "Login Failed.");
                return StatusCode(422, ModelState);
            }

            return Ok(user.Id);
        }
        
    }
}
