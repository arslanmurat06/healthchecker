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
        private readonly IUserRepository _userRepository;

        public HealthCheckJobManager(INotificationService notificationService, 
            IHealthCheckService healthCheckerService, 
            IJobsRepository jobRepository, IUserRepository userRepository)
        {
            _notificationService = notificationService;
            _healthCheckerService = healthCheckerService;
            _jobRepository = jobRepository;
            _userRepository = userRepository;
        }

        public async Task Process(int jobId)
        {
         
            var job = await _jobRepository.GetJob(jobId);
            var user = await _userRepository.GetUserById(job.UserId);
            
            if(job == null)
            {
                return;
            }
           
            var healtResult = await _healthCheckerService.Check(job.TargetURL);

            if (!healtResult)
            {
                var message = $"Website with url: {job.TargetURL} is down";
                await _notificationService.Notify(user, new Message(message));
            }
               

            job.LastRunTime = DateTime.UtcNow;

            await _jobRepository.UpdateJob(job);
        }
    }
}
