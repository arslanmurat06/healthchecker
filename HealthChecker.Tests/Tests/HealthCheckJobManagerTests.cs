using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using HealthChecker.Contracts.Interfaces.NotificationService;
using HealthChecker.Core.Services;
using HealthChecker.DataContext;
using HealthChecker.DataContext.Entities;
using HealthChecker.Tests.AutoFixture;
using HealthLifeCheckService;
using Microsoft.AspNetCore.Identity;
using Moq;
using ScheduleJobManager;
using Xunit;

namespace HealthChecker.Tests.Tests
{
    public class HealthCheckJobManagerTests
    {
        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_When_HealthCheck_False_Send_Notify(string userID, string userName, ApplicationDbContext context,
            JobService service,  [Frozen]Mock<INotificationService> moqNotificationService, [Frozen] Mock<IHealthCheckService> moqHealthCheckService,
            HealthCheckJobManager sut)
        {
            await SeedDataContext(context, userID);

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();


            var addedJobResult = await service.GetJobs(userName);

            var addedJob = addedJobResult.Data.First();

            moqHealthCheckService.Setup(s => s.Check(It.IsAny<string>())).ReturnsAsync(false);

            moqNotificationService.Setup(s => s.Notify()).ReturnsAsync(true);

            await sut.Process(addedJob.Id);

            moqNotificationService.Verify(m => m.Notify(), Times.Once());
        }



        [Theory]
        [InlineAutoMoqData("1a2b3c", "Test User")]
        public async Task Ensure_When_HealthCheck_True_Never_Send_Notify(string userID, string userName, ApplicationDbContext context,
            JobService service, [Frozen] Mock<INotificationService> moqNotificationService, [Frozen] Mock<IHealthCheckService> moqHealthCheckService,
            HealthCheckJobManager sut)
        {
            await SeedDataContext(context, userID);

            var user = new IdentityUser { Id = userID, UserName = userName };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();


            var addedJobResult = await service.GetJobs(userName);

            var addedJob = addedJobResult.Data.First();

            moqHealthCheckService.Setup(s => s.Check(It.IsAny<string>())).ReturnsAsync(true);

            moqNotificationService.Setup(s => s.Notify()).ReturnsAsync(true);

            await sut.Process(addedJob.Id);

            moqNotificationService.Verify(m => m.Notify(), Times.Never());
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
