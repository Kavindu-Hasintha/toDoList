namespace toDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ITodoService _todoRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, ITodoService todoRepository, IMapper mapper, IUserService userService)
        {
            _userRepository = userRepository;
            _todoRepository = todoRepository;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserChangeDetailsDto>>(_userService.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpPut]
        [Route("setemailpassword")]
        [Authorize]
        public async Task<IActionResult> SetEmailPassword([FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return BadRequest();
                }

                var user = await _userService.GetUserByEmailAsync();

                if (user == null)
                {
                    return BadRequest();
                }

                user.EmailPassword = password;

                var isSaved = await _userRepository.UpdateUserAsync(user);

                if (!isSaved)
                {
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpGet]
        [Route("getuserbyid/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> GetUserById(int userId) 
        {
            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);

                /*
                if (userId == null)
                {
                    return BadRequest();
                }

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
                */
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*
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

        
        [HttpPut]
        [Route("updateuser")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser([FromBody] UserChangeDetailsDto userUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _userService.UpdateUserAsync(userUpdate);

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.InvalidInput => StatusCode(422, ModelState),
                    OperationResult.InvalidEmail => StatusCode(422, ModelState),
                    OperationResult.Error => StatusCode(500, "Something went wrong while updating user"),
                    _ => BadRequest(),
                };

            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        
        [HttpDelete]
        [Route("deleteuser")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var result = await _userService.DeleteUserByEmailAsync();

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.Error => StatusCode(500, "Something went wrong while deleting user"),
                    _ => BadRequest(),
                };
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteuserbyid/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUserById([FromQuery] int userId)
        {
            try
            {
                var result = await _userService.DeleteUserByIdAsync(userId);

                return result switch
                {
                    OperationResult.Success => NoContent(),
                    OperationResult.NotFound => NotFound(),
                    OperationResult.Error => StatusCode(500, "Something went wrong while deleting user"),
                    _ => BadRequest(),
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
