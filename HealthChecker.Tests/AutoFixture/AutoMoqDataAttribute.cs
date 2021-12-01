using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Core.MapperProfiles;
using HealthChecker.Core.Repositories;
using HealthChecker.DataContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Tests.AutoFixture
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
          : base(() => GetFixture()) { }

        public AutoMoqDataAttribute(params ICustomization[] customization)
            : base(() => new Fixture().Customize(new CompositeCustomization(customization))) { }

        private static IFixture GetFixture()
        {
            var fixture = new Fixture()
               .Customize(new AutoMoqCustomization());

            DbContextOptionsBuilder<ApplicationDbContext> context = new DbContextOptionsBuilder<ApplicationDbContext>();
            context.UseInMemoryDatabase(Guid.NewGuid().ToString());

            fixture.Register(() => new ApplicationDbContext(context.Options));

            fixture.Register<IJobsRepository>(fixture.Create<JobsRepository>);
            fixture.Register<IUserRepository>(fixture.Create<UserRepository>);

            fixture.Customize(new AutoMapperCustomization());
            fixture.Customize(new HangFireCustomization());

            return fixture;
        }
    }
}
