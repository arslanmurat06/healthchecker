using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace HealthChecker.Contracts.Interfaces.NotificationService
{
    public interface INotificationService
    {
        Task Notify(UserDTO user, Message message);
    }
}
