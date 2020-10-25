using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo.Tests
{
    [TestFixture]
    public class BuildingMappingTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public async Task ListBuldingTypes_MapsCorrectly()
        {
            var expectedBuildingTypes = CreateBuildingTypes();
            var repo = CreateInMemoryRepo(expectedBuildingTypes, null);
            var actualBuildngTypes = await repo.ListBuildingTypes();

            Assert.AreEqual(expectedBuildingTypes.Length, actualBuildngTypes.Length);

            for (var i = 0; i < expectedBuildingTypes.Length; i++)
            {
                var expectedBuilding = expectedBuildingTypes[0];
                var actualBuilding = actualBuildngTypes.FirstOrDefault(x => x.Code == expectedBuilding.Code);
                Assert.NotNull(actualBuilding);
                Assert.AreEqual(expectedBuilding.Copyright, actualBuilding.Copyright);
                Assert.AreEqual(DateTimeOffset.Parse(expectedBuilding.LastUpdatedDate), actualBuilding.LastUpdatedDate);
                Assert.AreEqual(expectedBuilding.Name, actualBuilding.Name);
                Assert.AreEqual(expectedBuilding.SortOrder, actualBuilding.SortOrder);
            }
        }

        [Test]
        public async Task GetBuildingType_MapsCorrectly()
        {
            var expectedBuildings = CreateBuildingTypes();
            var repo = CreateInMemoryRepo(expectedBuildings, null);

            var expectedBuilding = expectedBuildings[1];
            var actualBuilding = await repo.GetBuildingType(expectedBuilding.Code);

            Assert.NotNull(actualBuilding);
            Assert.AreEqual(expectedBuilding.Code, actualBuilding.Code);
            Assert.AreEqual(expectedBuilding.Copyright, actualBuilding.Copyright);
            Assert.AreEqual(DateTimeOffset.Parse(expectedBuilding.LastUpdatedDate), actualBuilding.LastUpdatedDate);
            Assert.AreEqual(expectedBuilding.Name, actualBuilding.Name);
            Assert.AreEqual(expectedBuilding.SortOrder, actualBuilding.SortOrder);

            Assert.AreEqual(expectedBuilding.NameFormats.Count(), actualBuilding.NameFormats.Count());
            for (var i = 0; i < expectedBuilding.NameFormats.Count(); i++)
            {
                var expectedNameFormat = expectedBuilding.NameFormats[i];
                var actualNameFormat = actualBuilding.NameFormats[i];

                Assert.AreEqual(expectedNameFormat.NameFormat, actualNameFormat.NameFormat);
                Assert.AreEqual(expectedNameFormat.RandomizationWeight, actualNameFormat.RandomizationWeight);
                Assert.AreEqual(expectedNameFormat.NameFormat.Where(x => x == '{').Count(), actualNameFormat.NameFormatParts.Length);
                foreach (var part in actualNameFormat.NameFormatParts)
                    Assert.True(expectedNameFormat.NameFormat.Contains("{" + part + "}"));
            }
        }

        [Test]
        public async Task GetBuildingNameGroups_MapsCorrectly()
        {
            var group1Code = _fixture.Create<string>();
            var group2Code = _fixture.Create<string>();
            var group3Code = _fixture.Create<string>();

            var expectedNameParts = CreateBuildingNameParts(group1Code, group2Code, group3Code);
            var repo = CreateInMemoryRepo(null, expectedNameParts);

            var actualNameParts = await repo.GetBuildingNameGroups();

            Assert.AreEqual(1, actualNameParts.NameParts[group1Code].Count());
            Assert.AreEqual(2, actualNameParts.NameParts[group2Code].Count());
            Assert.AreEqual(3, actualNameParts.NameParts[group3Code].Count());

            foreach (var expectedPart in expectedNameParts)
            {
                Assert.True(actualNameParts.NameParts[expectedPart.GroupCode].Any(x => x.NamePart == expectedPart.NamePart && x.RandomizationWeight == expectedPart.RandomizationWeight));
            }
        }

        private BuildingTypeDto[] CreateBuildingTypes()
        {
            var buildingTypes = _fixture.Build<BuildingTypeDto>()
                                        .With(x => x.LastUpdatedDate, _fixture.Create<DateTime>().ToString())
                                        .With(x => x.NameFormats, CreateBuildingNameFormats())
                                        .CreateMany(3)
                                        .ToArray();
            return buildingTypes;
        }

        private List<BuildingNameFormatDto> CreateBuildingNameFormats()
        {
            return new List<BuildingNameFormatDto>()
            {
                _fixture.Build<BuildingNameFormatDto>()
                        .With(x => x.NameFormat, $"One {{{_fixture.Create<string>() }}}")
                        .Create(),
                _fixture.Build<BuildingNameFormatDto>()
                        .With(x => x.NameFormat, $"Two {{{_fixture.Create<string>() }}} And {{{_fixture.Create<string>() }}}")
                        .Create(),
                _fixture.Build<BuildingNameFormatDto>()
                        .With(x => x.NameFormat, $"Three {{{_fixture.Create<string>() }}} And {{{_fixture.Create<string>() }}} Or {{{_fixture.Create<string>() }}}")
                        .Create(),
            };
        }

        private BuildingNamePartDto[] CreateBuildingNameParts(string group1Code, string group2Code, string group3Code)
        {
            List<BuildingNamePartDto> nameParts = new List<BuildingNamePartDto>();

            nameParts.AddRange(_fixture.Build<BuildingNamePartDto>()
                                        .With(x => x.GroupCode, group1Code)
                                        .CreateMany(1)
                                        .ToArray());

            nameParts.AddRange(_fixture.Build<BuildingNamePartDto>()
                                        .With(x => x.GroupCode, group2Code)
                                        .CreateMany(2)
                                        .ToArray());

            nameParts.AddRange(_fixture.Build<BuildingNamePartDto>()
                                        .With(x => x.GroupCode, group3Code)
                                        .CreateMany(3)
                                        .ToArray());

            return nameParts.ToArray();
        }

        private GeneratorRepo CreateInMemoryRepo(BuildingTypeDto[] buildingTypes, BuildingNamePartDto[] nameParts)
        {
            var options = new DbContextOptionsBuilder<GeneratorContext>()
                    .UseSqlite("DataSource=:memory:")
                    .Options;

            var context = new GeneratorContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            if (buildingTypes != null)
                context.AddRange(buildingTypes);

            if (nameParts != null)
                context.AddRange(nameParts);

            context.SaveChanges();

            return new GeneratorRepo(context);
        }
    }
}
