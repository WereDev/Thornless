using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo.Tests
{
    [TestFixture]
    public class SettlementDataTests
    {
        [Test]
        public async Task CheckSettlementData()
        {
            var database = new GeneratorContext(new DbContextOptions<GeneratorContext>());
            var data = await database.SettlementTypes
                                    .Include(x => x.Buildings)
                                    .ToListAsync();

            Assert.AreEqual(data.Count, data.Select(x => x.Code).Distinct().Count(), $"Settlement codes must be unique.");
            foreach (var settlement in data)
            {
                VerifySettlementData(settlement);
            }
        }

        private void VerifySettlementData(SettlementTypeDto settlement)
        {
            Assert.IsNotNull(settlement.Code, $"Settlement Code can not be null: {settlement.Id}");
            Assert.IsNotNull(settlement.Name, $"Settlement Name can not be null: {settlement.Id}");
            Assert.True(settlement.SortOrder >= 0, $"Settlement SortOrder must be greater than zero: {settlement.Id}");
            Assert.True(settlement.MaxSize > settlement.MinSize, $"Settlement MaxSize must be larger than MinSize: {settlement.Id}");
            Assert.IsNotNull(settlement.Buildings, $"Settlement does not have buildings: {settlement.Id}");
            Assert.IsNotEmpty(settlement.Buildings, $"Settlement does not have buildings: {settlement.Id}");

            Assert.AreEqual(settlement.Buildings.Count,
                        settlement.Buildings.Select(x => x.BuildingTypeId).Distinct().Count(),
                        $"Building type must be unique: {settlement.Id}");

            foreach (var building in settlement.Buildings)
            {
                VerifyBuildingData(building);
            }
        }

        private void VerifyBuildingData(SettlementBuildingDistributionDto building)
        {
            Assert.NotNull(building.BuildingTypeId, $"Building type must be set: {building.Id}");
            Assert.True(building.MaxBuildings >= building.MinBuildings,
                    $"Building MaxBuildings must be larger than MinBuildings: {building.Id}");
        }
    }
}
