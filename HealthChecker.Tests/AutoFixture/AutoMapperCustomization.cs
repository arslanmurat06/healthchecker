using AutoFixture;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HealthChecker.Core.MapperProfiles;
using Moq;
using System;
using System.Linq;

namespace HealthChecker.Tests.AutoFixture
{
    public class AutoMapperCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile(typeof(MapProfile));
            });

            fixture.Register<MapperConfiguration>(() => mapperConfiguration);
            fixture.Register<IMapper>(() => new Mapper(mapperConfiguration));
        }
    }
}
