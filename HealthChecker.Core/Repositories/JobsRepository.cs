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
using HealthCheckerWeb.Core.Exceptions;

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

        public async Task<JobDTO> AddJob(JobDTO job, string userName)
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
                return _mapper.Map<JobDTO>(jobEntity);
            }

            throw new CustomException("Job could not be saved to db ");

        }


        public async Task<JobDTO> UpdateJob(JobDTO job, string userName = null)
        {
            IdentityUser _user = null;

            var jobEntity = await _context.Jobs.AsNoTracking().FirstAsync(j=>j.Id ==job.Id);

            if (jobEntity == null)
            {
                throw new CustomException("Job could not be found to update ");
            }

            if (userName != null)
                _user =  await _context.Users.FirstAsync(u => u.UserName == userName);

            jobEntity.Name = job.Name;
            jobEntity.TargetURL = job.TargetURL;
            jobEntity.TriggerInterval = job.TriggerInterval;
            jobEntity.TriggerType = job.TriggerType;

            jobEntity.CreatedUser = _user !=null ?  _user.UserName : jobEntity.CreatedUser;
            jobEntity.LastEditUser = _user !=null ? _user.UserName : jobEntity.LastEditUser;
            jobEntity.LastEdit = DateTime.UtcNow;
            jobEntity.LastRunTime = job.LastRunTime;

            _context.Update(jobEntity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return _mapper.Map<JobDTO>(jobEntity);
            }

            throw new CustomException("Job could not be updated");
        }


        public async Task<JobDTO> GetJob(int id)
        {
            var job = await _context.Jobs.AsNoTracking()
                                             .Where(j => j.Id == id )
                                             .Select(j => _mapper.Map<JobDTO>(j))
                                             .FirstOrDefaultAsync();

            if(job == null)
            {
                throw new CustomException($"There is no job found with id - {id}");
            }

            return job;
        }


        public async Task<List<JobDTO>> GetJobs(string userName)
        {

            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            var jobs = await _context.Jobs.AsNoTracking()
                                         .Where(j=>j.UserId == user.Id && j.Tombstone == null)
                                         .Select(j => _mapper.Map<JobDTO>(j))
                                         .ToListAsync();
            return jobs;
        }

        public async Task<bool> DeleteJob(int id, string userName)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            var foundJobEntity = await _context.Jobs.FirstOrDefaultAsync(j=>j.Id == id);

            if(foundJobEntity == null)
                throw new CustomException($"Job was not found.");

            foundJobEntity.Tombstone = DateTime.UtcNow;
            foundJobEntity.LastEditUser = user.UserName;
            foundJobEntity.LastEdit = DateTime.UtcNow;

            _context.Update(foundJobEntity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }
    }
}

