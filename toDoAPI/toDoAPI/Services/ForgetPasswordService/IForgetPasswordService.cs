namespace toDoAPI.Services.ForgetPasswordService
{
    public interface IForgetPasswordService
    {
        Task<bool> SaveOTP(string userEmail, string otp);

        Task<bool> SaveChangesAsync();
    }
}
