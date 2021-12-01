using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.MailNotification;
using NotificationService.MailNotification.Model;

namespace MailNotification.Extension
{
    public static class MailServiceExtensions
    {

        public static void UseMailService(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfiguration);
            services.AddScoped<IEmailSender, EmailSender>();
        
        }
    }
}
