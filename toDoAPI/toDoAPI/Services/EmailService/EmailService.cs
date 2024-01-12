using toDoAPI.Repositories.UserRepository;

namespace toDoAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        public EmailService(IEmailRepository emailRepository) 
        {
            _emailRepository = emailRepository;
        }

        public async Task<bool> SendEmail(string fromMail, string fromPassword, EmailDto request)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromMail);
                msg.To.Add(new MailAddress(request.ToEmail));
                if (!string.IsNullOrEmpty(request.CC))
                {
                    msg.CC.Add(new MailAddress(request.CC));
                }
                if (!string.IsNullOrEmpty(request.BCC))
                {
                    msg.Bcc.Add(new MailAddress(request.BCC));
                }
                msg.Subject = request.Subject;
                msg.Body = request.Body;
                //msg.Body = "<html><body> Test </body></html>";
                msg.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };
                smtpClient.Send(msg);

                var isEmailSaved = await _emailRepository.AddEmail(fromMail, request);

                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
