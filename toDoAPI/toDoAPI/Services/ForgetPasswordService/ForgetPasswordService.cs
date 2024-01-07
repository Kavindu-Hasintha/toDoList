using toDoAPI.Models;

namespace toDoAPI.Services.ForgetPasswordService
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly DataContext _context;
        private readonly int OTPExpireTime = 30;

        public ForgetPasswordService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveOTP(string userEmail, string otp) 
        {
            try
            {
                ForgetPassword data = new ForgetPassword();
                data.Email = userEmail;
                data.OTP = otp;
                data.IsOTPVerified = false;
                data.Expires = DateTime.UtcNow.AddMinutes(OTPExpireTime);

                _context.Add(data);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ForgetPassword> GetOTPByUsingEmailAsync(string email)
        {
            return await _context.ForgetPasswords.FirstOrDefaultAsync(f => f.Email == email);
        }

        public async Task<bool> UpdateForgetPassword(ForgetPassword request)
        {
            _context.Update(request);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
