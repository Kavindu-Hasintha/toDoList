using AutoMapper;

namespace toDoAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
    }
}
