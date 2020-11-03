using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.BuildingNames.Models;

namespace Thornless.Data.GeneratorRepo
{
    public class BuildingNameRepository : IBuildingNameRepository
    {
        private readonly GeneratorContext _generatorContext;
        private readonly Mapper _mapper;

        public BuildingNameRepository(GeneratorContext generatorContext)
        {
            _generatorContext = generatorContext ?? throw new ArgumentNullException(nameof(generatorContext));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneratorMapperProfile>();
                
            });
            _mapper = new Mapper(mapperConfig);
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

        public async Task<BuildingTypeModel[]> ListBuildingTypes()
        {
            var buildingTypes = await _generatorContext.BuildingTypes.ToArrayAsync();
            var mapped = _mapper.Map<BuildingTypeModel[]>(buildingTypes);
            return mapped;
        }
    }
}
