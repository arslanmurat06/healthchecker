using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Contracts.DTOs
{
    public  class Message
    {
        public string NotificationMessage { get; set; }

        public Message(string message)
        {
            NotificationMessage = message;
        }
    }
}
