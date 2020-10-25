using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thornless.Domain;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.CharacterNames.Models;

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
            _mapper = new Mapper(mapperConfig);
        }

        public async Task<AncestryDetailsModel> GetAncestry(string ancestryCode)
        {
            var ancestry = await _generatorContext.CharacterAncestries
                                            .Include(x => x.Options)
                                                .ThenInclude(x => x.SegmentGroups)
                                            .Include(x => x.NameParts)
                                            .FirstOrDefaultAsync(x => x.Code == ancestryCode);

            var mapped = _mapper.Map<AncestryDetailsModel>(ancestry);
            return mapped;    
        }

        public async Task<BuildingNameGroups> GetBuildingNameGroups()
        {
            var nameParts = await _generatorContext.BuildingNameParts.ToArrayAsync();
            var mapped = _mapper.Map<BuildingNameGroups>(nameParts);
            return mapped;
        }

        public async Task<BuildingTypeDetailsModel> GetBuildingType(string buildingTypeCode)
        {
            var buildingType = await _generatorContext.BuildingTypes
                                                    .Include(x => x.NameFormats)
                                                    .FirstOrDefaultAsync(x => x.Code == buildingTypeCode);

            var mapped = _mapper.Map<BuildingTypeDetailsModel>(buildingType);
            return mapped;
        }

        public async Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = await _generatorContext.CharacterAncestries.ToArrayAsync();
            var mapped = _mapper.Map<AncestryModel[]>(ancestries);
            return mapped;
        }

        public async Task<BuildingTypeModel[]> ListBuildingTypes()
        {
            var buildingTypes = await _generatorContext.BuildingTypes.ToArrayAsync();
            var mapped = _mapper.Map<BuildingTypeModel[]>(buildingTypes);
            return mapped;
        }
    }
}
