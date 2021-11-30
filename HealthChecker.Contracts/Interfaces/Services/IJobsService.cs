using System.Collections.Generic;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace DevJobs.Core.Interfaces.Services
{
    public interface IJobsService
    {
        Task<ResponseDTO<List<JobDTO>>> GetJobs();
        Task<ResponseDTO<JobDTO>> GetJob(int id);
    }
}
