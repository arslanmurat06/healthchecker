using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationService.MailNotification.Model;

namespace NotificationService.MailNotification
{
    public interface IEmailSender
    {
        Task SendEmail(Mail mail);
    }
}
