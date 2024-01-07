namespace toDoAPI.Services.ForgetPasswordService
{
    public interface IForgetPasswordService
    {
        Task<bool> SaveOTP(string userEmail, string otp);

        Task<ForgetPassword> GetOTPByUsingEmailAsync(string email);

        Task<bool> UpdateForgetPassword(ForgetPassword request);

        Task<bool> SaveChangesAsync();
    }
}
