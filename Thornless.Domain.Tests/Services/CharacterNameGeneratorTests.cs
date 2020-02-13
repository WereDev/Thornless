using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Thornless.Domain.Interfaces;
using Thornless.Domain.Models.CharacterNames;
using Thornless.Domain.Services;

namespace Thornless.Domain.Tests.Services
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
            var generator = CreateCharacterNameGenerator((AncestryModel[])null);
            var results = await generator.ListAncestries();
            Assert.Null(results);
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
            var generator = CreateCharacterNameGenerator((AncestryModel[])null);

            Assert.ThrowsAsync<ArgumentException>(() => generator.GenerateNames(string.Empty, string.Empty, count));
        }

        [Test]
        public async Task GenerateNames_WhenInValidCombo_ReturnsNull()
        {
            var ancestryDetails = CreateAncestryDetailsModel();
            var generator = CreateCharacterNameGenerator(ancestryDetails);

            var result = await generator.GenerateNames(_fixture.Create<string>(), _fixture.Create<string>(), 1);

            Assert.IsNull(result);
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
            var seperatorText = "seperator_text";
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

        private CharacterNameGenerator CreateCharacterNameGenerator(AncestryModel[] ancestryModels)
        {
            var mockRepository = new Mock<IGeneratorRepo>();
            mockRepository.Setup(x => x.ListAncestries()).ReturnsAsync(ancestryModels);
            var mockItemSelector = new Mock<IRandomItemSelector>();
            var mockNumberGenerator = new Mock<IRandomNumberGenerator>();

            var generator = new CharacterNameGenerator(mockRepository.Object, mockItemSelector.Object, mockNumberGenerator.Object);

            return generator;
        }

        private CharacterNameGenerator CreateCharacterNameGenerator(AncestryDetailsModel detailModel)
        {
            var mockRepository = new Mock<IGeneratorRepo>();
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

        private AncestryDetailsModel CreateAncestryDetailsModel()
        {
            var detailsModel = _fixture.Build<AncestryDetailsModel>()
                                        .Create();

            var nameParts = new List<AncestryDetailsModel.AncestryNamePartModel>();
            foreach (var detailOption in detailsModel.Options)
            {
                foreach (var segmentGroup in detailOption.SegmentGroups)
                {
                    foreach (var code in segmentGroup.NameSegmentCodes)
                    {
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
