using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace PersonalWebApp.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _fromEmail = _configuration["EmailSender:FromEmail"];
            _smtpClient = new SmtpClient(_configuration["EmailSender:STMPClient"])
            {
                Port = 587,
                Credentials = new NetworkCredential(_fromEmail, _configuration["EmailSender:Password"]),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }

}
