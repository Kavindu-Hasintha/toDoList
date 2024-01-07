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
                Models.ForgetPassword data = new Models.ForgetPassword();
                data.Email = userEmail;
                data.OTP = otp;
                data.Expires = DateTime.UtcNow.AddMinutes(OTPExpireTime);

                _context.Add(data);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
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
