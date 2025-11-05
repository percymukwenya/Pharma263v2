using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pharma263.Application.Contracts.Email;
using Pharma263.Application.Models.Email;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Pharma263.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSmtpSettings _emailSettings;

        public EmailSender(IConfiguration configuration, IOptions<EmailSmtpSettings> emailSettings)
        {
            _configuration = configuration;
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmail(string email, string subject, string message)
        {
            try
            {
                using var client = new SmtpClient(_configuration["Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                    EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:From"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetUrl)
        {
            string subject = "Pharma263 - Password Reset";
            string body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .btn {{ display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; 
                               text-decoration: none; border-radius: 5px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Password Reset Request</h2>
                        <p>Hello,</p>
                        <p>We received a request to reset your password for your Pharma263 account.</p>
                        <p>To reset your password, please click the button below:</p>
                        <p><a href='{resetUrl}' class='btn'>Reset Password</a></p>
                        <p>Or copy and paste this URL into your browser:</p>
                        <p>{resetUrl}</p>
                        <p>This link will expire in 24 hours. If you did not request a password reset, please ignore this email.</p>
                        <p>Thank you,<br>Pharma263 Support Team</p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordChangedEmailAsync(string email)
        {
            string subject = "Pharma263 - Password Changed";
            string body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Password Changed</h2>
                        <p>Hello,</p>
                        <p>Your password for your Pharma263 account has been successfully changed.</p>
                        <p>If you did not make this change, please contact the administrator immediately.</p>
                        <p>Thank you,<br>Pharma263 Support Team</p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Host = _emailSettings.SmtpServer;
                    client.Port = _emailSettings.Port;
                    client.EnableSsl = _emailSettings.EnableSsl;

                    if (_emailSettings.RequiresAuthentication)
                    {
                        client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    }

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;
                        message.To.Add(new MailAddress(to));

                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}
