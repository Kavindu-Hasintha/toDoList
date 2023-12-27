using toDoAPI.Models;

namespace toDoAPI.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
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
        public async Task<Object> GetUserAsync(int userId)
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

        public bool UserExist(int userId)
        {
            return _context.Users.Any(x => x.Id == userId);
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

        public async Task<bool> UpdateUser(User user)
        {
            _context.Update(user);
            return await Save();
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

        public async Task<bool> DeleteUser(User user)
        {
            _context.Remove(user);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public bool IsEmailValid(string email)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }

    }
}
