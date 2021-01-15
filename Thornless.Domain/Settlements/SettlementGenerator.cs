using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.Randomization;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Domain.Settlements
{
    public class SettlementGenerator : ISettlementGenerator
    {
        private readonly ISettlementRepository _repo;
        private readonly IRandomNumberGenerator _randomNumber;

        public SettlementGenerator(ISettlementRepository repo, IRandomNumberGenerator randomNumber)
        {
            _repo = repo;
            _randomNumber = randomNumber;
        }

        public async Task<SettlementTypeModel[]> ListSettlementTypes()
        {
            return await _repo.ListSettlementTypes();
        }

        public async Task<SettlementResultsModel> GenerateSettlement(string settlementCode, IBuildingNameGenerator buildingNameGenerator)
        {
            var settlement = await _repo.GetSettlement(settlementCode);
            if (settlement == null)
                throw new ArgumentException($"{settlementCode} is an invalid {nameof(settlementCode)}");

            var buildingNames = await buildingNameGenerator.ListBuildingTypes();
            var results = CreateResultsModel(settlement);

            foreach (var settlementBuilding in settlement.Buildings)
            {
                var buildingType = buildingNames.FirstOrDefault(x => x.Id == settlementBuilding.BuildingTypeId);
                results.Buildings.Add(buildingType, new List<BuildingNameResultModel>());
                var buildingList = results.Buildings[buildingType];
                var numberToMake = _randomNumber.GetRandomInteger(settlementBuilding.MinBuildings, settlementBuilding.MaxBuildings);

                for (int i = 0; i < numberToMake; i++)
                {
                    var building = await buildingNameGenerator.GenerateBuildingName(buildingType.Code);
                    if (buildingList.Any(x => x.BuildingName == building.BuildingName))
                        numberToMake--;
                    else
                        buildingList.Add(building);
                }
            }

            return results;
        }

        private SettlementResultsModel CreateResultsModel(SettlementTypeDetails settlement)
        {
            return new SettlementResultsModel
            {
                Code = settlement.Code,
                Id = settlement.Id,
                Name = settlement.Name,
                Population = _randomNumber.GetRandomInteger(settlement.MinSize, settlement.MaxSize),
            };
        }
    }
}
