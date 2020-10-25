using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Thornless.Data.GeneratorRepo;
using Thornless.Domain;
using Thornless.Domain.CharacterNames;
using Thornless.Domain.Randomization;
using Thornless.UI.Web.ViewModels.CharacterName;

namespace Thornless.UI.Web.AppStart
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<GeneratorContext>();
            services.AddScoped<IGeneratorRepo, GeneratorRepo>();
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<IRandomItemSelector, RandomItemSelector>();
            services.AddScoped<ICharacterNameGenerator, CharacterNameGenerator>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CharacterNameMapperProfile>();    
            });

            mapperConfig.AssertConfigurationIsValid();
            services.AddSingleton<IMapper>(new Mapper(mapperConfig));
        }
    }
}
