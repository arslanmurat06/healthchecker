using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthChecker.Contracts.DTOs;
using HealthChecker.DataContext.Entities;

namespace HealthChecker.Core.MapperProfiles
{
    public  class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<Job, JobDTO>().ReverseMap();
        }
    }
}
