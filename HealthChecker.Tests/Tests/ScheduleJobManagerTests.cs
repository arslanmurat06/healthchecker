using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using HealthChecker.Contracts;
using HealthChecker.Tests.AutoFixture;
using ScheduleJobManager;
using SchedulerService;
using Xunit;

namespace HealthChecker.Tests.Tests
{
    public class ScheduleJobManagerTests
    {
        [Theory]
        [InlineAutoMoqData("TestJobName", 1,2, IntervalEnum.Minute)]
        public void Ensure_Job_Added_Into_Hangfire_Storage_Successfully(
            string jobName, int jobID, int interval, IntervalEnum intervalType, 
            ScheduleJobManager<IHealthCheckJobManager> sut)
        {

            sut.AddJob(jobName, jobID, interval, intervalType);

            using var connection = JobStorage.Current.GetConnection();
            var recurringJobs = connection.GetRecurringJobs();

            var hangfireTestJob = recurringJobs.First();

            Assert.Equal(jobName, hangfireTestJob.Id);
            
        }


        [Theory]
        [InlineAutoMoqData("TestJobName", 1, 2, IntervalEnum.Minute)]
        public void Ensure_Job_Updated_Into_Hangfire_Storage_Successfully(
            string jobName, int jobID, int interval, IntervalEnum intervalType,
            ScheduleJobManager<IHealthCheckJobManager> sut)
        {

            sut.AddJob(jobName, jobID, interval, intervalType);

            using var connection = JobStorage.Current.GetConnection();
            var recurringJobs = connection.GetRecurringJobs();

            var hangfireTestJob = recurringJobs.First();
            Assert.Equal(jobName, hangfireTestJob.Id);

            string updatedJobName = "TestJobName-Updated";

            sut.UpdateJob(jobName, updatedJobName, jobID, interval, intervalType);


            var updatedRecurringJobs = connection.GetRecurringJobs();

            Assert.True(updatedRecurringJobs.Count() == 1);
            Assert.DoesNotContain(updatedRecurringJobs, j => j.Id == jobName);
            Assert.Contains(updatedRecurringJobs, j => j.Id == updatedJobName);

        }


        [Theory]
        [InlineAutoMoqData("TestJobName", 1, 2, IntervalEnum.Minute)]
        public void Ensure_Job_Deleted_From_Hangfire_Storage_Successfully(
           string jobName, int jobID, int interval, IntervalEnum intervalType,
           ScheduleJobManager<IHealthCheckJobManager> sut)
        {

            sut.AddJob(jobName, jobID, interval, intervalType);

            using var connection = JobStorage.Current.GetConnection();
            var recurringJobs = connection.GetRecurringJobs();

            var hangfireTestJob = recurringJobs.First();
            Assert.Equal(jobName, hangfireTestJob.Id);

            sut.DeleteJob(jobName);

            var updatedRecurringJobs = connection.GetRecurringJobs();

            Assert.True(updatedRecurringJobs.Count() == 0);
        }


        [Theory]
        [InlineAutoMoqData("TestJobName", 1, 2, IntervalEnum.Minute)]
        public void Ensure_HangFire_Job_Will_Execute_Correct_Method(
        string jobName, int jobID, int interval, IntervalEnum intervalType,
        ScheduleJobManager<IHealthCheckJobManager> sut)
        {
            sut.AddJob(jobName, jobID, interval, intervalType);

            using var connection = JobStorage.Current.GetConnection();
            var recurringJobs = connection.GetRecurringJobs();

            var hangfireTestJob = recurringJobs.First();

            var hangfireJobMethodName = hangfireTestJob.Job.Method.Name;
            var interfaceMethodName = nameof(IHealthCheckJobManager.Process);

            Assert.Equal(hangfireJobMethodName, interfaceMethodName);
        }
    }
}

