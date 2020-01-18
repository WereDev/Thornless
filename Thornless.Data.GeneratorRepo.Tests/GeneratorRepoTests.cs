using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo.Tests
{
    [TestFixture]
    public class GeneratorRepoTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public async Task ListAncestries_MapsCorrectly()
        {
            var expectedAncestries = CreateAncestries();
            var repo = CreateInMemoryRepo(expectedAncestries);
            var actualAncestries = await repo.ListAncestries();

            Assert.AreEqual(expectedAncestries.Length, actualAncestries.Length);

            for (var i = 0; i < expectedAncestries.Length; i++)
            {
                var expectedAncestry = expectedAncestries[0];
                var actualAncestry = actualAncestries.FirstOrDefault(x => x.Code == expectedAncestry.Code);
                Assert.NotNull(actualAncestry);
                Assert.AreEqual(expectedAncestry.Copyright, actualAncestry.Copyright);
                Assert.AreEqual(expectedAncestry.FlavorHtml, actualAncestry.FlavorHtml);
                Assert.AreEqual(expectedAncestry.LastUpdatedDate, actualAncestry.LastUpdatedDate);
                Assert.AreEqual(expectedAncestry.Name, actualAncestry.Name);
                Assert.AreEqual(expectedAncestry.SortOrder, actualAncestry.SortOrder);
            }
        }

        [Test]
        public async Task GetAncestry_MapsCorrectly()
        {
            var expectedAncestries = CreateAncestries();
            var repo = CreateInMemoryRepo(expectedAncestries);

            var expectedAncestry = expectedAncestries[1];
            var actualAncestry = await repo.GetAncestry(expectedAncestry.Code);

            Assert.NotNull(actualAncestry);
            Assert.AreEqual(expectedAncestry.Copyright, actualAncestry.Copyright);
            Assert.AreEqual(expectedAncestry.FlavorHtml, actualAncestry.FlavorHtml);
            Assert.AreEqual(expectedAncestry.LastUpdatedDate, actualAncestry.LastUpdatedDate);
            Assert.AreEqual(expectedAncestry.Name, actualAncestry.Name);
            Assert.AreEqual(expectedAncestry.SortOrder, actualAncestry.SortOrder);

            Assert.AreEqual(expectedAncestry.NameParts.Count(), actualAncestry.NameParts.Count());
            for (var i = 0; i < expectedAncestry.NameParts.Count(); i++)
            {
                var expectedNamePart = expectedAncestry.NameParts[i];
                var actualNamePart = actualAncestry.NameParts[i];

                Assert.AreEqual(ParseJsonArray(expectedNamePart.NameMeaningsJson), actualNamePart.NameMeanings);
                Assert.AreEqual(ParseJsonArray(expectedNamePart.NamePartsJson), actualNamePart.NameParts);
                Assert.AreEqual(expectedNamePart.NameSegmentCode, actualNamePart.NameSegmentCode);
                Assert.AreEqual(expectedNamePart.RandomizationWeight, actualNamePart.RandomizationWeight);
            }

            Assert.AreEqual(expectedAncestry.Options.Count(), actualAncestry.Options.Count());
            for (var i = 0; i < expectedAncestry.Options.Count(); i++)
            {
                var expectedOption = expectedAncestry.Options[i];
                var actualOption = actualAncestry.Options[i];

                Assert.AreEqual(expectedOption.Code, actualOption.Code);
                Assert.AreEqual(expectedOption.Name, actualOption.Name);
                Assert.AreEqual(ParseJsonArray(expectedOption.NamePartSeperatorJson), actualOption.NamePartSeperators);
                
                Assert.AreEqual(expectedOption.SegmentGroups.Count(), actualOption.SegmentGroups.Count());
                for (var j = 0; j < expectedOption.SegmentGroups.Count(); j++)
                {
                    var expectedSegmentGroup = expectedOption.SegmentGroups[j];
                    var actualSegmentGroup = actualOption.SegmentGroups[j];

                    Assert.AreEqual(ParseJsonArray(expectedSegmentGroup.NameSegmentCodesJson), actualSegmentGroup.NameSegmentCodes);
                    Assert.AreEqual(expectedSegmentGroup.RandomizationWeight, actualSegmentGroup.RandomizationWeight);
                }

                Assert.AreEqual(expectedOption.SeperatorChancePercentage, actualOption.SeperatorChancePercentage);
                Assert.AreEqual(expectedOption.SortOrder, actualOption.SortOrder);
            }
        }

        private CharacterAncestryDto[] CreateAncestries()
        {
            var ancestries = _fixture.Build<CharacterAncestryDto>()
                                    .With(x => x.NameParts,
                                            _fixture.Build<CharacterAncestryNamePartDto>()
                                                    .With(x => x.NameMeaningsJson, CreateJsonArrayString())
                                                    .With(x => x.NamePartsJson, CreateJsonArrayString())
                                                    .CreateMany(3)
                                                    .ToList())
                                    .With(x => x.Options,
                                            _fixture.Build<CharacterAncestryOptionDto>()
                                                    .With(x => x.NamePartSeperatorJson, CreateJsonArrayString())
                                                    .With(x => x.SegmentGroups, _fixture.Build<CharacterAncestrySegmentGroupDto>()
                                                                                        .With(x => x.NameSegmentCodesJson, CreateJsonArrayString())
                                                                                        .CreateMany(3)
                                                                                        .ToList())
                                                    .CreateMany(3)
                                                    .ToList())
                                    .CreateMany(3)
                                    .ToArray();
            return ancestries;
        }

        private GeneratorRepo CreateInMemoryRepo(CharacterAncestryDto[] ancestries)
        {
            var options = new DbContextOptionsBuilder<GeneratorContext>()
                    .UseSqlite("DataSource=:memory:")
                    .Options;

            var context = new GeneratorContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.AddRange(ancestries);
            context.SaveChanges();

            return new GeneratorRepo(context);
        }

        private string CreateJsonArrayString()
        {
            var strings = _fixture.CreateMany<string>();
            return JsonSerializer.Serialize(strings);
        }

        private string[] ParseJsonArray(string s)
        {
            return JsonSerializer.Deserialize<string[]>(s, null);
        }
    }
}
