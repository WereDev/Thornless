using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thornless.Domain.Settlements;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Data.GeneratorRepo
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly GeneratorContext _generatorContext;
        private readonly Mapper _mapper;

        public SettlementRepository(GeneratorContext generatorContext)
        {
            _generatorContext = generatorContext ?? throw new ArgumentNullException(nameof(generatorContext));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneratorMapperProfile>();
                
            });
            _mapper = new Mapper(mapperConfig);
        }

        public async Task<SettlementBuildingModel[]> GetSettlement(string settlementCode)
        {
            var data = await _generatorContext.SettlementTypes
                                            .Where(x => x.Code == settlementCode)
                                            .Include(x => x.Buildings)
                                            .FirstOrDefaultAsync();
            var mapped = _mapper.Map<SettlementBuildingModel[]>(data.Buildings);
            return mapped;
        }

        public async Task<SettlementTypeModel[]> ListSettlementTypes()
        {
            var data = await _generatorContext.SettlementTypes.ToArrayAsync();
            var mapped = _mapper.Map<SettlementTypeModel[]>(data);
            return mapped;
        }
    }
}
