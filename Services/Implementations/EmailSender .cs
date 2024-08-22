using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace PersonalWebApp.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailSender()
        {
            // Configuration for the SMTP client
            _smtpClient = new SmtpClient("smtp.gmail.com")  // Replace with your SMTP server
            {
                Port = 587,  // Typically, 587 for TLS
                Credentials = new NetworkCredential("jbrynnbacuta@gmail.com", ""),  // Replace with your email credentials
                EnableSsl = true
            };

            _fromEmail = "jbrynnbacuta@gmail.com";  // The email address you want to send from
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
