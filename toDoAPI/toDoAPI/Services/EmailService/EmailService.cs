namespace toDoAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(string fromMail, string fromPassword, string toEmail, string subject, string body)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(fromMail);
                msg.Subject = subject;
                msg.To.Add(new MailAddress(toEmail));
                msg.Body = body;
                //msg.Body = "<html><body> Test </body></html>";
                msg.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(msg);
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
