using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace HealthChecker.Contracts.Interfaces.Services
{
    public interface IJobService
    {
        Task<ResponseDTO<List<JobDTO>>> GetJobs(string userName);
        Task<ResponseDTO<JobDTO>> GetJob(int id);
        Task<ResponseDTO<JobDTO>> AddJob(JobDTO job, string userName);
        Task<ResponseDTO<JobDTO>> UpdateJob(JobDTO job, string userName);
        Task<ResponseDTO<bool>> DeleteJob(int id, string userName);
    }
}
