using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Core.Services;
using HealthChecker.DataContext;
using HealthChecker.DataContext.Entities;
using HealthChecker.Tests.AutoFixture;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace HealthChecker.Tests.Tests
{
    public class JobServiceTests
    {
        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_Gathered_Jobs_For_Specific_User(string userID, string userName, ApplicationDbContext context, JobService sut)
        {
            await SeedDataContext(context, userID);

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var result = await sut.GetJobs(userName);

            var fetchedJobs = result.Data;

            Assert.True(fetchedJobs.All(j => j.UserId == userID));
        }


        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_New_Added_Job_Saved_Successully(string userID, string userName, ApplicationDbContext context, JobService sut)
        {

            var newJobDTO = new JobDTO
            {
                Name = "Test Job",
                TargetURL = "http://www.test.com",
                UserId = userID,
                TriggerInterval = 1,
                TriggerType = Contracts.IntervalEnum.Minute,
                CreatedUser = "Tester Man",
                LastEditUser = "Tester Man"
            };

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = await sut.AddJob(newJobDTO, userName);

            var newlyAddedJob = result.Data;

            Assert.Equal(newJobDTO.UserId, newlyAddedJob.UserId);
            Assert.Equal(newJobDTO.Name, newlyAddedJob.Name);
        }


        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_Job_Updated_Successully(string userID, string userName, ApplicationDbContext context, JobService sut)
        {

            await SeedDataContext(context, userID); 

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();


            var result = await sut.GetJobs(userName);

            var firstJob = result.Data.First();

            firstJob.Name = "Updated Name";

            var response =  await sut.UpdateJob(firstJob, userName);

            var updatedJob = response.Data;

           Assert.Equal(firstJob.Name, updatedJob.Name);
           
        }


        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_Job_Deleted_Successully(string userID, string userName, ApplicationDbContext context, JobService sut)
        {

            await SeedDataContext(context, userID);

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();


            var jobsBeforeDelte = await sut.GetJobs(userName);

            var willBeDeletedJob = jobsBeforeDelte.Data.First();

            var response = await sut.DeleteJob(willBeDeletedJob.Id, userName);

            var jobsAfterDelete = (await sut.GetJobs(userName)).Data;

            Assert.DoesNotContain(jobsAfterDelete, j =>j.Id == willBeDeletedJob.Id);
        }


        private async Task SeedDataContext(ApplicationDbContext context, string userID)
        {
            var jobs = new List<Job>
            {
                new Job { Name = "Test Job" , TargetURL="http://www.test.com", UserId = userID, TriggerInterval = 1,
                    TriggerType = Contracts.IntervalEnum.Minute,CreatedUser="Tester Man", LastEditUser="Tester Man"} ,
                new Job { Name = "Test Job 2" , TargetURL="http://www.test.com", UserId = "2c3d4e", TriggerInterval = 1,
                    TriggerType = Contracts.IntervalEnum.Minute,CreatedUser="Tester", LastEditUser="Tester"} ,
            };

            await context.Jobs.AddRangeAsync(jobs);
        }
    }
}
