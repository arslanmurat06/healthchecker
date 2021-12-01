using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace NotificationService.MailNotification.Model
{
    public class Mail
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }

        public Mail(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
        }
    }
}
