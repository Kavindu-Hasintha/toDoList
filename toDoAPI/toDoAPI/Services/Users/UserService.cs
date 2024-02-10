using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using toDoAPI.Enums;
using toDoAPI.Models;
using toDoAPI.Repositories.TodoRepository;
using toDoAPI.Repositories.UserRepository;
using toDoAPI.Services.Todos;

namespace toDoAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository, ITodoRepository todoRepository, DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _todoRepository = todoRepository;
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto> GetUserById(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid userId");
            }

            var isUserExist = await _userRepository.UserExist(userId);

            if (!isUserExist)
            {
                return null;
            }

            var user = await _userRepository.GetUserAsync(userId);
            var userMap = _mapper.Map<UserDto>(user);
            return userMap;
        }

        public async Task<int> GetUserId()
        {
            var email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            var user = await GetUserAsync(email);
            if (user == null)
            {
                return 0;
            }
            return user.Id;
        }

        public async Task<User> GetUserByEmailAsync()
        {
                var email = string.Empty;
                if (_httpContextAccessor.HttpContext != null)
                {
                    email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                }

                var user = await GetUserAsync(email);
                return user;
        }

        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            var user = await GetUserAsync(email);
            if (user == null)
            {
                return 0;
            }
            return user.Id;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(x => x.Id).ToList();
        }
        /*
        public User GetUser(string email, string password)
        {
            return _context.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
        }
        */
        public async Task<User> GetUserAsync(int userId)
        {
            // return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserAsyncByRefreshToken(string refreshToken)
        {
            return await _context.Users.Where(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
        }

        public async Task<string> GetTaskManageEmail()
        {
            var userAdmin = await _context.Users.Where(u => u.Id == 5).FirstOrDefaultAsync();
            return userAdmin.Email;
        }

        public async Task<string> GetTaskManagePassword()
        {
            var userAdmin = await _context.Users.Where(u => u.Id == 5).FirstOrDefaultAsync();
            return userAdmin.EmailPassword;
        }

        public async Task<bool> UserExist(int userId)
        {
            return await _context.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> RegisterUser(User user)
        {
            _context.Add(user);
            return await Save();
        }

        public async Task<OperationResult> UpdateUserAsync(UserChangeDetailsDto userUpdate)
        {
            try
            {
                if (userUpdate == null)
                {
                    throw new ArgumentNullException("Request body is null.");
                }

                var isUserExists = await _userRepository.UserExist(userUpdate.Id);
                if (!isUserExists)
                {
                    return OperationResult.NotFound;
                }

                if (userUpdate.Name.Length == 0 || userUpdate.Email.Length == 0)
                {
                    return OperationResult.InvalidInput;
                }

                var isEmailValid = await IsEmailValid(userUpdate.Email);
                if (!isEmailValid)
                {
                    return OperationResult.InvalidEmail;
                }

                var userMap = _mapper.Map<User>(userUpdate);

                var isUpdated = await _userRepository.UpdateUserAsync(userMap);

                if (!isUpdated)
                {
                    return OperationResult.Error;
                }

                return OperationResult.Success;
            }
            catch
            {
                return OperationResult.Error;
            }
        }

        public async Task<OperationResult> DeleteUserByEmailAsync()
        {
            try
            {
                var deleteUser = await GetUserByEmailAsync();
                return await DeleteUserAsync(deleteUser);
            }
            catch
            {
                return OperationResult.Error;
            }
        }

        public async Task<OperationResult> DeleteUserByIdAsync(int userId)
        {
            try
            {
                User deleteUser = await GetUserAsync(userId);
                return await DeleteUserAsync(deleteUser);
            }
            catch
            {
                return OperationResult.Error;
            }
        }

        public async Task<OperationResult> DeleteUserAsync(User deleteUser)
        {
            try
            {
                if (deleteUser == null)
                {
                    return OperationResult.NotFound;
                }

                var todos = await _todoRepository.GetTodosByUserIdAsync(deleteUser.Id);

                foreach (var t in todos)
                {
                    var isTaskDeleted = await _todoRepository.DeleteTodo(t);
                    if (!isTaskDeleted)
                    {
                        return OperationResult.Error;
                    }
                }

                var isUserDeleted = await _userRepository.DeleteUserAsync(deleteUser);
                if (!isUserDeleted)
                {
                    return OperationResult.Error;
                }

                return OperationResult.Success;
            }
            catch
            {
                return OperationResult.Error;
            }
        }

        public async Task<bool> UpdateUserRefreshToken(int userId, string newToken, DateTime created, DateTime expires)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            user.RefreshToken = newToken;
            user.TokenCreated = created;
            user.TokenExpired = expires;

            return await Save();
        }



        public async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        public async Task<bool> IsEmailValid(string email)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }

    }
}
