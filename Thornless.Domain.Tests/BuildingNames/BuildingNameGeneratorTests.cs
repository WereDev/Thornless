using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests.BuildingNames
{
    public class BuildingNameGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public async Task ListBuildingTypes_MapsCorrectly()
        {
            var buildingTypes = CreateBuildingType();
            var generator = CreateBuildingNameGenerator(buildingTypes, null, null);

            var results = await generator.ListBuildingTypes();

            Assert.AreEqual(buildingTypes, results);
        }

        [Test]
        public async Task ListBuildingTypes_WhenNull_ReturnsEmpty()
        {
            var generator = CreateBuildingNameGenerator(null, null, null);

            var results = await generator.ListBuildingTypes();

            Assert.AreEqual(0, results.Length);
        }

        [Test]
        public async Task GenerateBuildingName_WhenInvalidType_ReturnsNull()
        {
            var generator = CreateBuildingNameGenerator(null, null, null);

            var results = await generator.GenerateBuildingName(_fixture.Create<string>());

            Assert.IsNull(results);
        }

        [Test]
        public async Task GenerateBuildingName_WhenValid_CreatesResult()
        {
            var details = CreateFullBuildingTypeDetailsModel();
            var generator = CreateBuildingNameGenerator(null, details, GetFullBuildingNameParts());
            var index = new Random().Next(0, 2);

            var results = await generator.GenerateBuildingName(details[index].Code);

            Assert.AreEqual(details[index].Code, results.BuildingTypeCode);
            Assert.AreEqual(details[index].Name, results.BuildingTypeName);

            if (results.BuildingName.StartsWith("The"))
            {
                Assert.True(results.BuildingName.StartsWith("The personA's")
                            || results.BuildingName.StartsWith("The personB's"), $"Person not found: {results.BuildingName}");

                Assert.True(results.BuildingName.EndsWith("'s nomsA")
                            || results.BuildingName.EndsWith("'s nomsB")
                            || results.BuildingName.EndsWith("'s nomsC"), $"Food item not found: {results.BuildingName}");
            }
            else if (results.BuildingName.EndsWith("Place"))
            {
                Assert.True(results.BuildingName.StartsWith("nomsA &")
                            || results.BuildingName.StartsWith("nomsB &")
                            || results.BuildingName.StartsWith("nomsC &"), $"Food item not found: {results.BuildingName}");

                Assert.True(results.BuildingName.EndsWith("& thingA Place")
                            || results.BuildingName.EndsWith("& thingB Place")
                            || results.BuildingName.EndsWith("& thingC Place"), $"Thing item not found: {results.BuildingName}");
            }
            else
            {
                Assert.True(results.BuildingName.StartsWith("thingA OR")
                            || results.BuildingName.StartsWith("thingB OR")
                            || results.BuildingName.StartsWith("thingC OR"), $"Thing item not found: {results.BuildingName}");

                Assert.True(results.BuildingName.EndsWith("OR nomsA")
                            || results.BuildingName.EndsWith("OR nomsB")
                            || results.BuildingName.EndsWith("OR nomsC"), $"Food item not found: {results.BuildingName}");
            }
        }

        [Test]
        public async Task GenerateBuildingName_WhenValid_HasNoDupes()
        {
            var details = CreateSingleBuildingTypeDetailsModel();
            var generator = CreateBuildingNameGenerator(null, details, GetFullBuildingNameParts());

            var detail = details[0];
            var results = await generator.GenerateBuildingName(detail.Code);

            Assert.AreEqual(detail.Code, results.BuildingTypeCode);
            Assert.AreEqual(detail.Name, results.BuildingTypeName);

            Assert.True(results.BuildingName == "personA & personB"
                        || results.BuildingName == "personB & personA");
        }

        [Test]
        public async Task GenerateBuildingName_WhenHasSecondaryTemplateItem_FillsAdditionalFormat()
        {
            var details = CreateFullBuildingTypeDetailsModel(true);
            var generator = CreateBuildingNameGenerator(null, details, GetFullBuildingNameParts(true));

            var results = await generator.GenerateBuildingName(details[0].Code);

            Assert.False(results.BuildingName.Contains("{"));
            Assert.True(results.BuildingName.StartsWith("thing"));
        }

        private BuildingTypeModel[] CreateBuildingType()
        {
            return new BuildingTypeModel[]
            {
                _fixture.Create<BuildingTypeModel>(),
            };
        }

        private BuildingNameGenerator CreateBuildingNameGenerator(
            BuildingTypeModel[] buildingModels,
            BuildingTypeDetailsModel[] detailsModels,
            BuildingNameGroups nameGroups)
        {
            var mockRepository = new Mock<IBuildingNameRepository>();

            if (buildingModels != null)
                mockRepository.Setup(x => x.ListBuildingTypes()).ReturnsAsync(buildingModels);

            if (detailsModels != null)
            {
                foreach (var detail in detailsModels)
                    mockRepository.Setup(x => x.GetBuildingType(detail.Code)).ReturnsAsync(detail);
            }

            if (nameGroups != null)
                mockRepository.Setup(x => x.GetBuildingNameGroups()).ReturnsAsync(nameGroups);

            // var mockCache = new Mock<IMemoryCache>();
            // mockCache.Setup(x => x.GetOrCreateAsync(nameof(BuildingNameGroups)))
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var generator = new BuildingNameGenerator(mockRepository.Object, new RandomItemSelector(new RandomNumberGenerator()), memoryCache);

            return generator;
        }

        private BuildingTypeDetailsModel[] CreateFullBuildingTypeDetailsModel(bool useEmbedded = false)
        {
            var nameFormats = new BuildingTypeDetailsModel.BuildingNameFormatModel[]
                                {
                                    new BuildingTypeDetailsModel.BuildingNameFormatModel { NameFormat = "The {people}'s {food}", RandomizationWeight = 1 },
                                    new BuildingTypeDetailsModel.BuildingNameFormatModel { NameFormat = "{food} & {item} Place", RandomizationWeight = 1 },
                                    new BuildingTypeDetailsModel.BuildingNameFormatModel { NameFormat = "{item} OR {food}", RandomizationWeight = 1 },
                                };

            if (useEmbedded)
            {
                nameFormats = new BuildingTypeDetailsModel.BuildingNameFormatModel[]
                {
                    new BuildingTypeDetailsModel.BuildingNameFormatModel { NameFormat = "{embed}", RandomizationWeight = 1 },
                };
            }

            var detailsModel = _fixture.Build<BuildingTypeDetailsModel>()
                                        .With(x => x.NameFormats, nameFormats)
                                        .CreateMany(useEmbedded ? 1 : 3)
                                        .ToArray();

            return detailsModel;
        }

        private BuildingTypeDetailsModel[] CreateSingleBuildingTypeDetailsModel()
        {
            var nameFormats = new BuildingTypeDetailsModel.BuildingNameFormatModel[]
                                {
                                    new BuildingTypeDetailsModel.BuildingNameFormatModel { NameFormat = "{people} & {people}", RandomizationWeight = 1 },
                                };

            var detailsModel = _fixture.Build<BuildingTypeDetailsModel>()
                                        .With(x => x.NameFormats, nameFormats)
                                        .CreateMany(1)
                                        .ToArray();

            return detailsModel;
        }

        private BuildingNameGroups GetFullBuildingNameParts(bool includeEmbedded = false)
        {
            var nameParts = new Dictionary<string, BuildingNameGroups.NamePartModel[]>
            {
                {
                    "people",
                    new BuildingNameGroups.NamePartModel[]
                    {
                        new BuildingNameGroups.NamePartModel { NamePart = "personA", RandomizationWeight = 1 },
                        new BuildingNameGroups.NamePartModel { NamePart = "personB", RandomizationWeight = 1 },
                    }
                },
                {
                    "food",
                    new BuildingNameGroups.NamePartModel[]
                    {
                        new BuildingNameGroups.NamePartModel { NamePart = "nomsA", RandomizationWeight = 1 },
                        new BuildingNameGroups.NamePartModel { NamePart = "nomsB", RandomizationWeight = 1 },
                        new BuildingNameGroups.NamePartModel { NamePart = "nomsC", RandomizationWeight = 1 },
                    }
                },
                {
                    "item",
                    new BuildingNameGroups.NamePartModel[]
                    {
                        new BuildingNameGroups.NamePartModel { NamePart = "thingA", RandomizationWeight = 1 },
                        new BuildingNameGroups.NamePartModel { NamePart = "thingB", RandomizationWeight = 1 },
                        new BuildingNameGroups.NamePartModel { NamePart = "thingC", RandomizationWeight = 1 },
                    }
                },
            };

            if (includeEmbedded)
            {
                nameParts.Add(
                    "embed",
                    new BuildingNameGroups.NamePartModel[]
                    {
                        new BuildingNameGroups.NamePartModel { NamePart = "{item}", RandomizationWeight = 1 },
                    });
            }

            return new BuildingNameGroups
            {
                NameParts = nameParts,
            };
        }
    }
}
