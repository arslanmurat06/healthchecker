using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SchedulerService
{
    public static class HangFireConfigurationExtension
    {
        public static void UseHangFire(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHangfire(config =>
            {
                var options = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,

                    // Hangfire'ın ne kadar süre aralıkta kontrol edeceği bilgisi
                    // Default değeri 15 saniye
                    //QueuePollInterval = TimeSpan.FromMinutes(2),

                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                };

                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), options).WithJobExpirationTimeout(TimeSpan.FromHours(6));
            });
        }


        public static void UseHangFireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {

            var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();

            app.UseHangfireDashboard(
                "/hangfire", new DashboardOptions
                {
                    DashboardTitle = "Health Checker Hangfire Dashboard",
                    AppPath = "https://localhost:44338/jobs", 
                    Authorization = new[] { new HangfireCustomBasicAuthenticationFilter{
            User = hangfireSettings.Username,
            Pass = hangfireSettings.Password
        } } 
                }
                );

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMinutes(1), 
                WorkerCount = Environment.ProcessorCount * 5
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 7 });

        }
    }
}
