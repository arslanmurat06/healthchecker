using System.Collections.Generic;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace HealthChecker.Contracts.Interfaces.Repositories
{
    public interface IJobsRepository
    {
        Task<ResponseDTO<List<JobDTO>>> GetJobs(string userName);
        Task<ResponseDTO<JobDTO>> GetJob(int id);
        Task<ResponseDTO<JobDTO>> AddJob(JobDTO job, string userName);
        Task<ResponseDTO<JobDTO>> UpdateJob(JobDTO job, string userName);
        Task<ResponseDTO<bool>> DeleteJob(int id, string userName);
    }
}
