using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.Interfaces.NotificationService;

namespace NotificationService
{
    public class MailService : INotificationService
    {
        public Task<bool> Notify()
        {
            Console.WriteLine("***notify edildi***");

            return Task.FromResult(true);   
        }
    }
}
