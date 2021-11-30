using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthChecker.Contracts.DTOs
{
    public class JobDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Target URL")]
        public string TargetURL { get; set; }

        [Required]
        public IntervalEnum TriggerType { get; set; }


        [Required]
        public int TriggerInterval { get; set; }
        public DateTime? LastRunTime { get; set; }
        public DateTime? Created { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? LastEdit { get; set; }
        public string? LastEditUser { get; set; }
    }
}
