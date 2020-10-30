using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Thornless.Data.GeneratorRepo;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.CharacterNames;
using Thornless.Domain.Randomization;
using Thornless.UI.Web.ViewModels.BuildingName;
using Thornless.UI.Web.ViewModels.CharacterName;

namespace Thornless.UI.Web.AppStart
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<GeneratorContext>();
            services.AddScoped<ICharacterNameRepository, CharacterNameRepository>();
            services.AddScoped<IBuildingNameRepository, BuildingNameRepository>();
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<IRandomItemSelector, RandomItemSelector>();
            services.AddScoped<ICharacterNameGenerator, CharacterNameGenerator>();
            services.AddScoped<IBuildingNameGenerator, BuildingNameGenerator>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CharacterNameMapperProfile>();
                cfg.AddProfile<BuildingNameMapperProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
            services.AddSingleton<IMapper>(new Mapper(mapperConfig));
        }
    }
}
