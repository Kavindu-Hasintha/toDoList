namespace toDoAPI.Services.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string fromMail, string fromPassword, EmailDto request);
    }
}
