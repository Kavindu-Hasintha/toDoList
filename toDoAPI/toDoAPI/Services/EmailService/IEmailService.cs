namespace toDoAPI.Services.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string fromMail, string fromPassword, string toEmail, string subject, string body);
    }
}
