using System.Collections.Generic;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace HealthChecker.Contracts.Interfaces.Repositories
{
    public interface IJobsRepository
    {
        Task<List<JobDTO>> GetJobs(string userName);
        Task<JobDTO> GetJob(int id);
        Task<JobDTO> AddJob(JobDTO job, string userName);
        Task<JobDTO> UpdateJob(JobDTO job, string userName);
        Task<bool> DeleteJob(int id, string userName);
    }
}
