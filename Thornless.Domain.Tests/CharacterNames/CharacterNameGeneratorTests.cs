using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Thornless.Domain.CharacterNames;
using Thornless.Domain.CharacterNames.Models;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.Tests.CharacterNames
{
    [TestFixture]
    public class CharacterNameGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public async Task ListAncestries_MapsCorrectly()
        {
            var ancestryModels = CreateAncestryModels();
            var generator = CreateCharacterNameGenerator(ancestryModels);

            var results = await generator.ListAncestries();

            Assert.AreEqual(ancestryModels, results);
        }

        [Test]
        public async Task ListAncestries_WhenNull_ReturnsNull()
        {
            var generator = CreateCharacterNameGenerator(new AncestryModel[0]);
            var results = await generator.ListAncestries();
            Assert.True(results.Length == 0);
        }

        [Test]
        public async Task ListAncestryOptions_WhenHasMatch_ReturnsMatch()
        {
            var ancestryDetails = CreateAncestryDetailsModel();
            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.ListAncestryOptions(ancestryDetails.Code);

            Assert.AreEqual(ancestryDetails, result);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ListAncestryOptions_WhenNotValidCount_Throws(int count)
        {
            var generator = CreateCharacterNameGenerator(new AncestryModel[0]);

            Assert.ThrowsAsync<ArgumentException>(() => generator.GenerateNames(string.Empty, string.Empty, count));
        }

        [Test]
        public async Task GenerateNames_WhenInValidCombo_ReturnsEmptyNames()
        {
            var ancestryDetails = CreateAncestryDetailsModel();
            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(_fixture.Create<string>(), _fixture.Create<string>(), 1);

            Assert.AreEqual(0, result.Length);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GenerateNames_WhenValidCombo_ReturnsNames(int count)
        {
            var ancestryDetails = CreateAncestryDetailsModel();
            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(ancestryDetails.Code, ancestryDetails.Options.First().Code, count);

            Assert.AreEqual(count, result.Length);

            var firstOption = ancestryDetails.Options.First();
            var firstName = result.First();

            Assert.AreEqual(ancestryDetails.Code, firstName.AncestryCode);
            Assert.AreEqual(ancestryDetails.Name, firstName.AncestryName);
            Assert.AreEqual(firstOption.Code, firstName.OptionCode);
            Assert.AreEqual(firstOption.Name, firstName.OptionName);
            Assert.False(string.IsNullOrWhiteSpace(firstName.Name));

            var firstDefinition = firstName.Definitions.First();
            var namePart = ancestryDetails.NameParts.FirstOrDefault(x => x.NameParts.Contains(firstDefinition.NamePart));

            Assert.IsNotNull(namePart);
            Assert.AreEqual(namePart.NameMeanings, firstDefinition.Meanings);
        }

        [Test]
        public async Task GenerateNames_WhenValid_ConfirmHasNameSeperator()
        {
            var seperatorText = "seperator_Text"; // odd casing is to match ToTitleCase result.
            var ancestryDetails = CreateAncestryDetailsModel();
            foreach (var option in ancestryDetails.Options)
            {
                option.NamePartSeperators = new string[] { seperatorText };
                option.SeperatorChancePercentage = 100;
            }

            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(ancestryDetails.Code, ancestryDetails.Options.First().Code, 1);

            Assert.True(result.First().Name.Contains(seperatorText));
        }

        [Test]
        public async Task GenerateNames_WhenValid_ConfirmHasTitleCase()
        {
            var seperatorText = "seperator_text";
            var ancestryDetails = CreateAncestryDetailsModel();
            foreach (var option in ancestryDetails.Options)
            {
                option.NamePartSeperators = new string[] { seperatorText };
                option.SeperatorChancePercentage = 100;
            }

            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(ancestryDetails.Code, ancestryDetails.Options.First().Code, 1);

            var name = result.First().Name;

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            Assert.AreEqual(textInfo.ToTitleCase(name.ToLower()), name);
        }

        [TestCase("a")]
        [TestCase("ab")]
        public async Task GenerateNames_WhenHasSeperator_SeperatorDoesntMatchSegments(string seperator)
        {
            var firstChar = seperator.First();
            var lastChar = seperator.Last();

            var ancestryDetails = CreateAncestryDetailsModel(1);

            foreach (var option in ancestryDetails.Options)
            {
                option.NamePartSeperators = new string[] { seperator };
                option.SeperatorChancePercentage = 100;
            }

            foreach (var namePart in ancestryDetails.NameParts)
            {
                namePart.NameParts = new string[] { $"{firstChar}namePart{lastChar}" };
            }

            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(ancestryDetails.Code, ancestryDetails.Options.First().Code, 1);

            var name = result.First().Name;
            Assert.False(name.Contains($"{firstChar}{firstChar}{firstChar}"));
            Assert.False(name.Contains($"{lastChar}{lastChar}{lastChar}"));
        }

        [TestCase(AncestryLiterals.Apostrophe)]
        [TestCase(AncestryLiterals.Empty)]
        [TestCase(AncestryLiterals.Space)]
        public async Task GenerateNames_WithLiteral_SkipsSeperators(string literal)
        {
            var seperatorText = "seperator_text";

            var ancestryDetails = CreateAncestryDetailsModel(1);

            foreach (var option in ancestryDetails.Options)
            {
                option.NamePartSeperators = new string[] { seperatorText };
                option.SeperatorChancePercentage = 100;
                option.SegmentGroups = new AncestryDetailsModel.AncestrySegmentGroupModel[]
                {
                    new AncestryDetailsModel.AncestrySegmentGroupModel()
                    {
                        NameSegmentCodes = new string[]
                        {
                            option.SegmentGroups.First().NameSegmentCodes.First(),
                            literal,
                            option.SegmentGroups.First().NameSegmentCodes.First(),
                        },
                    },
                };
            }

            var nameParts = ancestryDetails.NameParts.ToList();
            nameParts.Add(new AncestryDetailsModel.AncestryNamePartModel
            {
                NameParts = new string[] { literal },
                NameSegmentCode = literal,
            });
            ancestryDetails.NameParts = nameParts.ToArray();

            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(ancestryDetails.Code, ancestryDetails.Options.First().Code, 1);

            var name = result.First().Name;
            Assert.True(name.Contains(literal, StringComparison.CurrentCultureIgnoreCase));
            Assert.False(name.Contains(seperatorText, StringComparison.CurrentCultureIgnoreCase));
        }

        private CharacterNameGenerator CreateCharacterNameGenerator(AncestryModel[] ancestryModels)
        {
            var mockRepository = new Mock<ICharacterNameRepository>();
            mockRepository.Setup(x => x.ListAncestries()).ReturnsAsync(ancestryModels);
            var mockItemSelector = new Mock<IRandomItemSelector>();
            var mockNumberGenerator = new Mock<IRandomNumberGenerator>();

            var generator = new CharacterNameGenerator(mockRepository.Object, mockItemSelector.Object, mockNumberGenerator.Object);

            return generator;
        }

        private CharacterNameGenerator CreateCharacterNameGenerator(AncestryDetailsModel detailModel)
        {
            var mockRepository = new Mock<ICharacterNameRepository>();
            mockRepository.Setup(x => x.GetAncestry(detailModel.Code)).ReturnsAsync(detailModel);

            var rng = new RandomNumberGenerator();
            var ris = new RandomItemSelector(rng);

            var mockNumberGenerator = new Mock<IRandomNumberGenerator>();

            var generator = new CharacterNameGenerator(mockRepository.Object, ris, rng);

            return generator;
        }

        private AncestryModel[] CreateAncestryModels()
        {
            return new AncestryModel[]
            {
                _fixture.Build<AncestryModel>().With(x => x.Code, "ancestryCode1").With(x => x.Name, "ancestryName1").Create(),
            };
        }

        private AncestryDetailsModel CreateAncestryDetailsModel(int numberOfDetails = 3)
        {
            var detailsModel = _fixture.Build<AncestryDetailsModel>()
                                        .With(x => x.Options, _fixture.Build<AncestryDetailsModel.AncestryOptionsModel>()
                                                                                                 .CreateMany(numberOfDetails)
                                                                                                 .ToArray())
                                        .Create();

            var nameParts = new List<AncestryDetailsModel.AncestryNamePartModel>();
            for (var i = 0; i < detailsModel.Options.Length; i++)
            {
                var detailOption = detailsModel.Options[0];
                detailOption.Code = $"detailOption{i}";

                for (var j = 0; j < detailOption.SegmentGroups.Length; j++)
                {
                    var segmentGroup = detailOption.SegmentGroups[j];
                    for (var k = 0; k < segmentGroup.NameSegmentCodes.Length; k++)
                    {
                        var code = segmentGroup.NameSegmentCodes[k];
                        nameParts.AddRange(_fixture.Build<AncestryDetailsModel.AncestryNamePartModel>()
                                                  .With(x => x.NameSegmentCode, code)
                                                  .CreateMany());
                    }
                }
            }

            detailsModel.NameParts = nameParts.ToArray();

            return detailsModel;
        }
    }
}
