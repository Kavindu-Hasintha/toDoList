using toDoAPI.Models;

namespace toDoAPI.Repositories.UserRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public EmailRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<bool> AddEmail(string from, EmailDto request)
        {
            int userId = await _userRepository.GetUserIdByEmailAsync(from);

            Email email = new Email();
            email.From = from;
            email.To = request.ToEmail;
            email.CC = request.CC;
            email.BCC = request.BCC;
            email.Subject = request.Subject;
            email.Body = request.Body;
            email.SendAt = DateTime.Now;
            email.UserId = userId;

            _context.Add(email);
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
    }
}
