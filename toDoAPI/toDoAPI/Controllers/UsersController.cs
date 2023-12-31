using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using toDoAPI.Dto;
using toDoAPI.Models;
using toDoAPI.Services.Todos;
using toDoAPI.Services.Users;

namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, ITodoRepository todoRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserChangeDetailsDto>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }
        /*

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

        [HttpGet("getuserdetails{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDetails(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserChangeDetailsDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }
        */
        /*
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

            if (!_userRepository.IsEmailValid(userCreate.Email))
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
        */

        /*
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
        */

        /*
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserDetails([FromBody] UserChangeDetailsDto userUpdate)
        {
            if (userUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.UserExist(userUpdate.Id))
            {
                return NotFound();
            }

            if (userUpdate.Name.Length == 0 || userUpdate.Email.Length == 0 || userUpdate.Password.Length == 0)
            {
                ModelState.AddModelError("UserError", "Please fill all the fields.");
                return StatusCode(422, ModelState);
            }

            if (!_userRepository.IsEmailValid(userUpdate.Email))
            {
                ModelState.AddModelError("UserError", "Invalid email address.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userMap = _mapper.Map<User>(userUpdate);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("UserError", "Something went wrong while updating user");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated.");
        }
        */

        /*
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            var userToDelete = _userRepository.GetUser(userId);

            var todos = _todoRepository.GetTodos(userId);

            foreach (var t in todos)
            {
                if (!_todoRepository.DeleteTodo(t))
                {
                    ModelState.AddModelError("UserError", "Something went wrong while deleting.");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("UserError", "Something went wrong while deleting.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted.");
        }
        */
    }
}
