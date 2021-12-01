using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.NotificationService;
using NotificationService.MailNotification.Model;

namespace NotificationService.MailNotification
{
    public class MailService : INotificationService
    {
        private readonly IEmailSender _emailSender;

        public MailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Notify(UserDTO user, Message message)
        {
            var mailContent = new Mail(new string[] { user.Email }, $"HealthCheck Report ", message.NotificationMessage);
            await _emailSender.SendEmail(mailContent);
        }
    }
}
