using System.Net.Mail;
using System.Net;

namespace Library_managment_system_API.Entities
{
    public class EmailService
    {

        public EmailService(IConfiguration configuration)
        {
            Configuration1 = configuration;
        }

        public IConfiguration Configuration1 { get; }

        public void SendingMail(string toMail, string subject, string body)
        {
            var fromMail = Configuration1.GetSection("Constants:FromEmail").Value ?? string.Empty;
            var fromEmailPassword = Configuration1.GetSection("Constants:FromEmailPass").Value ?? string.Empty;

            var message = new MailMessage()
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toMail);

            var smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromEmailPassword),
                EnableSsl = true
            };

            smtpServer.Send(message);

            
        }

    }
}
