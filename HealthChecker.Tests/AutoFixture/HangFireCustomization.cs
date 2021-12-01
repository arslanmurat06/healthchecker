using AutoFixture;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Hangfire;
using Hangfire.MemoryStorage;
using HealthChecker.Core.MapperProfiles;
using Moq;
using System;
using System.Linq;

namespace HealthChecker.Tests.AutoFixture
{
    public class HangFireCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var jobStorage = new MemoryStorage();
            GlobalConfiguration.Configuration.UseStorage(jobStorage);


            fixture.Register<JobStorage>(() => jobStorage);
        }
    }
}
