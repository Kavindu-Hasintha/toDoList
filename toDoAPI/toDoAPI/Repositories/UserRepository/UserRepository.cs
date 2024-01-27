using AutoMapper;
using toDoAPI.Dto;

namespace toDoAPI.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<TaskManageDto> GetTaskManageEmailPasswordAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 5);

            if (user != null)
            {
                TaskManageDto taskManageDto = new TaskManageDto();
                taskManageDto.Email = user.Email;
                taskManageDto.EmailPassword = user.EmailPassword;

                return taskManageDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UserExist(int userId)
        {
            return await _context.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Implement the logic to update the user in the database
            // Example using Entity Framework:
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;

                return await SaveChangesAsync();
            }

            return false;
        }

        public async Task<bool> UpdateUserRefreshTokenAsync(int userId, string token, DateTime created, DateTime expire)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (existingUser != null) 
            {
                existingUser.RefreshToken = token;
                existingUser.TokenCreated = created;
                existingUser.TokenExpired = expire;

                return await SaveChangesAsync();
            }

            return false;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _context.Remove(user);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteRefreshTokenByUserAsync(User user)
        {
            if (user != null)
            {
                user.RefreshToken = string.Empty;
                user.TokenCreated = new DateTime();
                user.TokenExpired = new DateTime();

                return await SaveChangesAsync();
            }
            return false;
        }

        public async Task<bool> SaveChangesAsync()
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
    }
}
