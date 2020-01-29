using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thornless.Domain.Interfaces;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Data.GeneratorRepo
{
    public class GeneratorRepo : IGeneratorRepo
    {
        private readonly GeneratorContext _generatorContext;
        private readonly Mapper _mapper;

        public GeneratorRepo(GeneratorContext generatorContext)
        {
            _generatorContext = generatorContext ?? throw new ArgumentNullException(nameof(generatorContext));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneratorMapperProfile>();
                
            });
            mapperConfig.AssertConfigurationIsValid();
            _mapper = new Mapper(mapperConfig);
        }

        public async Task<AncestryDetailsModel> GetAncestry(string ancestryCode)
        {
            var ancestry = await _generatorContext.CharacterAncestries
                                            .Include(x => x.Options)
                                            .FirstOrDefaultAsync(x => x.Code == ancestryCode);

            var mapped = _mapper.Map<AncestryDetailsModel>(ancestry);
            return mapped;    
        }

        public async Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = await _generatorContext.CharacterAncestries.ToArrayAsync();
            var mapped = _mapper.Map<AncestryModel[]>(ancestries);
            return mapped;
        }
    }
}
