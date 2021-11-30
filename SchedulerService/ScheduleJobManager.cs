using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HealthChecker.Contracts;
using HealthChecker.Contracts.Interfaces.SchedulerService;

namespace SchedulerService
{
    public class ScheduleJobManager<T> : IScheduleJobManager<T> where T : IJob
    {
        public void AddOrUpdateJob(string jobName, int jobId, int interval, IntervalEnum intervalType)
        {
            RecurringJob.AddOrUpdate<T>(jobName, j => j.Process(jobId), GetCronFromInterval(interval, intervalType));
        }

        public void DeleteJob(string jobName)
        {
            RecurringJob.RemoveIfExists(jobName);
        }

        private string GetCronFromInterval(int interval, IntervalEnum intervalType)
        {
            var result = intervalType switch
            {
                IntervalEnum.Minute => Cron.MinuteInterval(interval),
                IntervalEnum.Hour => Cron.HourInterval(interval),
                IntervalEnum.Day => Cron.DayInterval(interval),
                IntervalEnum.Week => Cron.Weekly(),
                IntervalEnum.Month => Cron.MonthInterval(interval),
                _ => throw new NotImplementedException()
            };
            return result;
        }
    }
}
