using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HealthChecker.Contracts;
using HealthChecker.Contracts.Interfaces.SchedulerService;

namespace SchedulerService
{

    public  interface IScheduleJobManager<T> where T : IJob
    {
        void AddOrUpdateJob(string jobName,int jobId, int  interval, IntervalEnum intervalType);
        void DeleteJob(string jobName);

    }
}
