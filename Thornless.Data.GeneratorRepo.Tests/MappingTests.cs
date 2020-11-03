using AutoMapper;
using NUnit.Framework;

namespace Thornless.Data.GeneratorRepo.Tests
{
    public class MappingTests
    {
        [Test]
        public void VerifyMappingConfiguration()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneratorMapperProfile>();
            });
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}
