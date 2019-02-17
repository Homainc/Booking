using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace Booking.Services
{
    public class EmailService
    {
        private IConfiguration _config;

        public EmailService(IConfiguration configuration) => _config = configuration;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            var adminEmailConfig = _config.GetSection("AdminEmail");
            var adminEmail = adminEmailConfig.GetValue<string>("Email");
            var adminPassword = adminEmailConfig.GetValue<string>("Password");
            var smtpHost = adminEmailConfig.GetValue<string>("SmtpHost");
            var smtpPort = adminEmailConfig.GetValue<int>("SmtpPort");

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", adminEmail));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using(var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpHost, smtpPort, false);
                await client.AuthenticateAsync(adminEmail, adminPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
