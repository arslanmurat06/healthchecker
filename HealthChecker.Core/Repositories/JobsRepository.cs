using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.DataContext;
using HealthChecker.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace HealthChecker.Core.Repositories
{
    public class JobsRepository : IJobsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobsRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<JobDTO>> AddJob(JobDTO job, string userName)
        {
            var jobEntity = _mapper.Map<Job>(job);

            var user = await _context.Users.FirstAsync(u=>u.UserName == userName);

            jobEntity.User = user;
            jobEntity.Created = DateTime.UtcNow;
            jobEntity.CreatedUser = user.UserName;
            jobEntity.LastEditUser = user.UserName;

            await _context.Jobs.AddAsync(jobEntity);

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return new ResponseDTO<JobDTO>(_mapper.Map<JobDTO>(jobEntity));
            }

            return new ResponseDTO<JobDTO>(new List<ErrorDTO>() { new ErrorDTO("Job could not be saved to db ")});
        }


        public async Task<ResponseDTO<JobDTO>> UpdateJob(JobDTO job, string userName)
        {

            var jobEntity = await _context.Jobs.AsNoTracking().FirstAsync(j=>j.Id ==job.Id);

            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            if (jobEntity == null)
            {
                return new ResponseDTO<JobDTO>(new List<ErrorDTO>() { new ErrorDTO("Job could not be found to update ") });
            }

            jobEntity.Name = job.Name;
            jobEntity.TargetURL = job.TargetURL;
            jobEntity.TriggerInterval = job.TriggerInterval;
            jobEntity.TriggerType = job.TriggerType;

            jobEntity.CreatedUser = user.UserName;
            jobEntity.LastEditUser = user.UserName;
            jobEntity.LastEdit = DateTime.UtcNow;

            _context.Update(jobEntity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseDTO<JobDTO>(_mapper.Map<JobDTO>(jobEntity));
            }

            return new ResponseDTO<JobDTO>(new List<ErrorDTO>() { new ErrorDTO("Job could not be updated") });
        }


        public async Task<ResponseDTO<JobDTO>> GetJob(int id)
        {
            var job = await _context.Jobs.AsNoTracking()
                                             .Where(j => j.Id == id )
                                             .Select(j => _mapper.Map<JobDTO>(j))
                                             .FirstOrDefaultAsync();

            if(job == null)
            {
                return new ResponseDTO<JobDTO>(new List<ErrorDTO>() { new ErrorDTO($"There is no job found with id - {id}") });
            }

            return  new ResponseDTO<JobDTO>(job);
        }


        public async Task<ResponseDTO<List<JobDTO>>> GetJobs(string userName)
        {

            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            var jobs = await _context.Jobs.AsNoTracking()
                                         .Where(j=>j.UserId == user.Id && j.Tombstone == null)
                                         .Select(j => _mapper.Map<JobDTO>(j))
                                         .ToListAsync();
            if (!jobs.Any())
            {
                return new ResponseDTO<List<JobDTO>>(new List<ErrorDTO>() { new ErrorDTO($"No job added yet") });
            }

            return new ResponseDTO<List<JobDTO>>(jobs);
        }

        public async Task<ResponseDTO<bool>> DeleteJob(int id, string userName)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            var foundJobEntity = await _context.Jobs.FirstOrDefaultAsync(j=>j.Id == id);

            if(foundJobEntity == null)
                return new ResponseDTO<bool>(new List<ErrorDTO>() { new ErrorDTO( $"Job was not found.") });

            foundJobEntity.Tombstone = DateTime.UtcNow;
            foundJobEntity.LastEditUser = user.UserName;
            foundJobEntity.LastEdit = DateTime.UtcNow;

            _context.Update(foundJobEntity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return new ResponseDTO<bool>(true);

            return new ResponseDTO<bool>(false);

        }
    }
}

