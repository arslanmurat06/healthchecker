using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Contracts.Interfaces.NotificationService
{
    public interface INotificationService
    {
        //Task<bool> Notify(User);
        Task<bool> Notify();
    }
}
