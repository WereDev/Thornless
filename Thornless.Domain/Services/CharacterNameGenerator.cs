using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thornless.Domain.Interfaces;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Domain.Services
{
    public class CharacterNameGenerator : ICharacterNameGenerator
    {
        private readonly IGeneratorRepo _generatorRepo;
        private readonly IRandomItemSelector _randomItemSelector;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public CharacterNameGenerator(IGeneratorRepo generatorRepo, IRandomItemSelector randomItemSelector, IRandomNumberGenerator randomNumberGenerator)
        {
            _generatorRepo = generatorRepo ?? throw new ArgumentNullException(nameof(generatorRepo));
            _randomItemSelector = randomItemSelector ?? throw new ArgumentNullException(nameof(randomItemSelector));
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }

        public Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = _generatorRepo.ListAncestries();
            return ancestries;
        }

        public Task<AncestryDetailsModel> ListAncestryOptions(string ancestryCode)
        {
            var ancestry = _generatorRepo.GetAncestry(ancestryCode);
            return ancestry;            
        }

        public async Task<CharacterNameResultModel[]> GenerateNames(string ancestryCode, string ancestryOptionCode, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));
            var ancestry = await ListAncestryOptions(ancestryCode);
            var option = ancestry?.Options.FirstOrDefault(x => x.Code == ancestryOptionCode);
            if (option == null)
                return null;

            var results = new CharacterNameResultModel[count];

            for (var i = 0; i < count; i++)
            {
                results[i] = CreateCharacterName(ancestry, option);
            }

            return results;
        }

        private CharacterNameResultModel CreateCharacterName(AncestryDetailsModel ancestry, AncestryDetailsModel.AncestryOptionsModel selectedOption)
        {
            var segment = _randomItemSelector.GetRandomWeightedItem(selectedOption.SegmentGroups);
            var nameParts = GetNameDefinitions(segment, ancestry.NameParts);
            var name = CreateNameFromParts(nameParts, selectedOption.SeperatorChancePercentage, selectedOption.NamePartSeperators);

            return new CharacterNameResultModel
            {
                AncestryCode = ancestry.Code,
                AncestryName = ancestry.Name,
                Definitions = nameParts.Where(x => x.Meanings.Any()).ToArray(),
                Name = name,
                OptionCode = selectedOption.Code,
                OptionName = selectedOption.Name,
            };
        }

        private CharacterNameResultModel.CharacterNameDefinition[] GetNameDefinitions(
            AncestryDetailsModel.AncestrySegmentGroupModel segment, 
            AncestryDetailsModel.AncestryNamePartModel[] nameParts)
        {
            var selectedParts = new List<CharacterNameResultModel.CharacterNameDefinition>();

            foreach (var segmentCode in segment.NameSegmentCodes)
            {
                var segments = nameParts.Where(x => x.NameSegmentCode == segmentCode).ToArray();
                var randomSegment = _randomItemSelector.GetRandomWeightedItem(segments);
                var namePart = new CharacterNameResultModel.CharacterNameDefinition
                {
                    Meanings = randomSegment.NameMeanings,
                    NamePart = _randomItemSelector.GetRandomItem(randomSegment.NameParts),
                };
                selectedParts.Add(namePart);
            }

            return selectedParts.ToArray();
        }

        private string CreateNameFromParts(CharacterNameResultModel.CharacterNameDefinition[] nameParts, int seperatorChance, string[] seperators)
        {
            var name = new StringBuilder();

            var adjustedChance = seperatorChance % 101;

            for (var i = 0; i < nameParts.Length; i++)
            {
                var namePart = nameParts[i];

                name.Append(namePart.NamePart);

                if (i < nameParts.Length - 1)
                {
                    var randomNumber = _randomNumberGenerator.GetRandomInteger(100) + 1;
                    if (randomNumber > adjustedChance)
                        continue;

                    var seperator = _randomItemSelector.GetRandomItem(seperators);
                    if (namePart.NamePart.EndsWith(seperator, true, null))
                        continue;

                    var nextPart = nameParts[i + 1];
                    if (!nextPart.NamePart.StartsWith(seperator, true, null))
                        name.Append(seperator);
                }
            }

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.ToString().ToLower());
        }
    }
}
