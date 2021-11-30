using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HealthChecker.Contracts;
using Microsoft.AspNetCore.Identity;

namespace HealthChecker.DataContext.Entities
{

    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }

        public IntervalEnum TriggerType { get; set; }

        public int TriggerInterval { get; set; }
        public string TargetURL { get; set; }
        public DateTime? LastRunTime { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? LastEdit { get; set; }
        public string LastEditUser { get; set; }

        public DateTime? Tombstone { get; set; }

        public virtual IdentityUser User { get; set; }


    }
}
