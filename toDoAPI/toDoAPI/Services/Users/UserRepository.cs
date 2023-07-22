using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using toDoAPI.Data;
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

        public User GetUser(string email, string password)
        {
            return _context.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
        }

        public User GetUser(int userId)
        {
            return _context.Users.Where(x => x.Id == userId).FirstOrDefault();
        }

        public bool UserExist(int userId)
        {
            return _context.Users.Any(x => x.Id == userId);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool IsEmailValid(string email)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }
    }
}
