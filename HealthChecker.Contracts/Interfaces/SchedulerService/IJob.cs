using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Contracts.Interfaces.SchedulerService
{
    public  interface IJob
    {
        Task Process(int jobId);
    }
}
