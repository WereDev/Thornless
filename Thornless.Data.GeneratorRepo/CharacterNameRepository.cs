using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thornless.Domain.CharacterNames;
using Thornless.Domain.CharacterNames.Models;

namespace Thornless.Data.GeneratorRepo
{
    public class CharacterNameRepository : ICharacterNameRepository
    {
        private readonly GeneratorContext _generatorContext;
        private readonly Mapper _mapper;

        public CharacterNameRepository(GeneratorContext generatorContext)
        {
            _generatorContext = generatorContext ?? throw new ArgumentNullException(nameof(generatorContext));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneratorMapperProfile>();
                
            });
            _mapper = new Mapper(mapperConfig);
        }

        public async Task<AncestryDetailsModel?> GetAncestry(string ancestryCode)
        {
            var ancestry = await _generatorContext.CharacterAncestries
                                            .Include(x => x.Options)
                                                .ThenInclude(x => x.SegmentGroups)
                                            .Include(x => x.NameParts)
                                            .FirstOrDefaultAsync(x => x.Code == ancestryCode);

            var mapped = _mapper.Map<AncestryDetailsModel>(ancestry);
            return mapped;    
        }

        public async Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = await _generatorContext.CharacterAncestries.ToArrayAsync();
            var mapped = _mapper.Map<AncestryModel[]>(ancestries);
            return mapped ?? Array.Empty<AncestryModel>();
        }
    }
}
