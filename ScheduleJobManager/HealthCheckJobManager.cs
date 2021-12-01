using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.NotificationService;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Contracts.Interfaces.Services;
using HealthLifeCheckService;

namespace ScheduleJobManager
{
    public class HealthCheckJobManager : IHealthCheckJobManager
    {
        private readonly INotificationService _notificationService;
        private readonly IHealthCheckService _healthCheckerService;
        private readonly IJobsRepository _jobRepository;

        public HealthCheckJobManager(INotificationService notificationService, IHealthCheckService healthCheckerService, IJobsRepository jobRepository)
        {
            _notificationService = notificationService;
            _healthCheckerService = healthCheckerService;
            _jobRepository = jobRepository;
        }


        public async Task Process(int jobId)
        {
         
            var job = await _jobRepository.GetJob(jobId);
            
            if(job == null)
            {
                return;
            }
           
            var healtResult = await _healthCheckerService.Check(job.TargetURL);

            if (!healtResult)
                await _notificationService.Notify();
            
        }
    }
}
