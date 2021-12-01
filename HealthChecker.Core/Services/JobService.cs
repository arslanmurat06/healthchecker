using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Contracts.Interfaces.Services;
using ScheduleJobManager;
using SchedulerService;

namespace HealthChecker.Core.Services
{
    public class JobService : IJobService
    {
        private readonly IScheduleJobManager<IHealthCheckJobManager> _scheduleManager;
        private readonly IJobsRepository _jobRepository;
        private readonly IUserRepository _userRepository;

        public JobService(IScheduleJobManager<IHealthCheckJobManager> scheduleManager, IJobsRepository jobsRepository, IUserRepository userRepository)
        {
            _scheduleManager = scheduleManager;
            _jobRepository = jobsRepository;
            _userRepository = userRepository;
        }

        public async Task<ResponseDTO<JobDTO>> AddJob(JobDTO job, string userName)
        {
            var result = await _jobRepository.AddJob(job, userName);
             _scheduleManager.AddJob(await CreateUniqueJobName(result.Name, userName), result.Id, result.TriggerInterval, result.TriggerType);

            return new ResponseDTO<JobDTO>(result);

        }

        public async Task<ResponseDTO<bool>> DeleteJob(int id, string userName)
        {
            var job = await _jobRepository.GetJob(id);
            string jobName = job.Name;

            var result = await _jobRepository.DeleteJob(id, userName);
            _scheduleManager.DeleteJob(await CreateUniqueJobName(jobName, userName));

            return new ResponseDTO<bool>(result);
        }

        public async Task<ResponseDTO<JobDTO>> GetJob(int id)
        {
            var result = await _jobRepository.GetJob(id);

            return new ResponseDTO<JobDTO>(result);
        }

        public async Task<ResponseDTO<List<JobDTO>>> GetJobs(string userName)
        {
            var result = await _jobRepository.GetJobs(userName);

            return new ResponseDTO<List<JobDTO>>(result);
        }

        public async Task<ResponseDTO<JobDTO>> UpdateJob(JobDTO job, string userName)
        {

            var oldJob = await _jobRepository.GetJob(job.Id);
            string oldJobName = oldJob.Name;

            var result = await _jobRepository.UpdateJob(job, userName);

            _scheduleManager.UpdateJob(await CreateUniqueJobName(oldJobName, userName), result.Name, result.Id, result.TriggerInterval, result.TriggerType);

            return new ResponseDTO<JobDTO>(result);
        }

        private async Task<string> CreateUniqueJobName(string jobName, string userName)
        {
            var user = await _userRepository.GetUser(userName);

            return $"{jobName}-{user.Id}";
        }
    }
}
