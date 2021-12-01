using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using NotificationService.MailNotification.Model;

namespace NotificationService.MailNotification
{
    public class EmailSender : IEmailSender
    {

        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(EmailConfiguration emailConfig, ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }


        public async Task SendEmail(Mail mail)
        {
            var emailMessage = CreateEmailMessage(mail);

            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Mail mail)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(mail.To);
            emailMessage.Subject = mail.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = mail.Content };
            return emailMessage;
        }


        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error happened while sending mail to - {mailMessage.To}", ex);
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
