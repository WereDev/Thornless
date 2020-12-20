using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.Settlements;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Domain.Tests.Settlements
{
    public class SettlementGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public async Task ListSettlementTypes_GivenData_ReturnsArray()
        {
            var mockRng = RandomizationHelper.CreateMockRandomNumberGenerator();

            var mockRepo = new Mock<ISettlementRepository>();
            var testSettlements = _fixture.CreateMany<SettlementTypeModel>().ToArray();
            mockRepo.Setup(x => x.ListSettlementTypes()).ReturnsAsync(testSettlements);

            var generator = new SettlementGenerator(mockRepo.Object, mockRng.Object);
            var result = await generator.ListSettlementTypes();

            mockRepo.Verify(x => x.ListSettlementTypes(), Times.Once);
            mockRepo.VerifyNoOtherCalls();
            Assert.NotNull(result);
            Assert.Greater(result.Count(), 0);
            Assert.AreEqual(testSettlements, result);
        }

        [Test]
        public async Task ListSettlementTypes_EmptyList_ReturnsEmptyArray()
        {
            var mockRng = RandomizationHelper.CreateMockRandomNumberGenerator();
            var mockRepo = new Mock<ISettlementRepository>();

            var generator = new SettlementGenerator(mockRepo.Object, mockRng.Object);
            var result = await generator.ListSettlementTypes();

            mockRepo.Verify(x => x.ListSettlementTypes(), Times.Once);
            mockRepo.VerifyNoOtherCalls();
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task GenerateSettlement_ValidCode_GeneratesSettlement()
        {
            var mockRng = RandomizationHelper.CreateMockRandomNumberGenerator();

            var mockRepo = new Mock<ISettlementRepository>();
            var settlement = _fixture.Create<SettlementTypeDetails>();
            mockRepo.Setup(x => x.GetSettlement(settlement.Code)).ReturnsAsync(settlement);

            var buildingTypes = _fixture.CreateMany<BuildingTypeModel>(settlement.Buildings.Count()).ToArray();

            for (int i = 0; i < settlement.Buildings.Count(); i++)
            {
                settlement.Buildings[i].BuildingTypeId = i + 10;
                buildingTypes[i].Id = i + 10;
            }

            var mockBuildingGenerator = new Mock<IBuildingNameGenerator>();
            mockBuildingGenerator.Setup(x => x.ListBuildingTypes()).ReturnsAsync(buildingTypes);
            var buildingResults = buildingTypes.Select(x => new BuildingNameResultModel
            {
                BuildingName = _fixture.Create<string>(),
                BuildingTypeCode = x.Code,
                BuildingTypeName = x.Name,
            }).ToArray();
            
            mockBuildingGenerator.Setup(x => x.GenerateBuildingName(It.IsAny<string>()))
                .Returns<string>((code) => Task.Run(() => buildingResults.FirstOrDefault(x => x.BuildingTypeCode == code)));
            
            var generator = new SettlementGenerator(mockRepo.Object, mockRng.Object);
            var result = await generator.GenerateSettlement(settlement.Code, mockBuildingGenerator.Object);

            // Assert general result
            Assert.NotNull(result);
            Assert.AreEqual(settlement.Code, result.Code);
            Assert.AreEqual(settlement.Id, result.Id);
            Assert.AreEqual(settlement.Name, result.Name);
            mockRng.Verify(x => x.GetRandomInteger(settlement.MinSize, settlement.MaxSize), Times.AtLeastOnce);
            Assert.GreaterOrEqual(result.Population, Math.Min(settlement.MinSize, settlement.MaxSize));
            Assert.LessOrEqual(result.Population, Math.Max(settlement.MinSize, settlement.MaxSize));
            Assert.AreEqual(settlement.Buildings.Count(), result.Buildings.Count());

            // Assert buildings created
            foreach (var settlmentBuilding in settlement.Buildings)
            {
                var resultBuilding = result.Buildings.First(x => x.Key.Id == settlmentBuilding.BuildingTypeId);
                Assert.AreEqual(settlmentBuilding.MinBuildings, resultBuilding.Value.Count());

                var expectedBuildingType = buildingTypes.First(x => x.Id == settlmentBuilding.BuildingTypeId);
                Assert.AreEqual(expectedBuildingType.Code, resultBuilding.Key.Code);

                mockRng.Verify(x => x.GetRandomInteger(settlmentBuilding.MinBuildings, settlmentBuilding.MaxBuildings), Times.AtLeastOnce);
                Assert.GreaterOrEqual(resultBuilding.Value.Count, Math.Min(settlmentBuilding.MinBuildings, settlmentBuilding.MaxBuildings));
                Assert.LessOrEqual(resultBuilding.Value.Count, Math.Max(settlmentBuilding.MinBuildings, settlmentBuilding.MaxBuildings));
            }
        }
    }
}
