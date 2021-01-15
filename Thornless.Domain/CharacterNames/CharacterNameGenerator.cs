using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thornless.Domain.CharacterNames.Models;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.CharacterNames
{
    public class CharacterNameGenerator : ICharacterNameGenerator
    {
        private readonly ICharacterNameRepository _repo;
        private readonly IRandomItemSelector _randomItemSelector;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public CharacterNameGenerator(ICharacterNameRepository repo, IRandomItemSelector randomItemSelector, IRandomNumberGenerator randomNumberGenerator)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _randomItemSelector = randomItemSelector ?? throw new ArgumentNullException(nameof(randomItemSelector));
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }

        public Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = _repo.ListAncestries();
            return ancestries;
        }

        public async Task<AncestryDetailsModel> ListAncestryOptions(string ancestryCode)
        {
            var ancestry = await _repo.GetAncestry(ancestryCode);
            return ancestry ?? new AncestryDetailsModel();            
        }

        public async Task<CharacterNameResultModel[]> GenerateNames(string ancestryCode, string ancestryOptionCode, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));
            var ancestry = await ListAncestryOptions(ancestryCode);
            if (ancestry == null)
                return new CharacterNameResultModel[0];

            var option = ancestry.Options.FirstOrDefault(x => x.Code == ancestryOptionCode);
            if (option == null)
                return new CharacterNameResultModel[0];

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
                Definitions = nameParts.Where(x => x.Meanings?.Any() == true).ToArray(),
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
                    NamePartCode = randomSegment.NameSegmentCode,
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
                    var nextPart = nameParts[i + 1];

                    if (AncestryLiterals.Literals.Contains(namePart.NamePartCode)
                        || AncestryLiterals.Literals.Contains(nextPart.NamePartCode))
                        continue;

                    var randomNumber = _randomNumberGenerator.GetRandomInteger(100);
                    if (randomNumber > adjustedChance)
                        continue;

                    var seperator = _randomItemSelector.GetRandomItem(seperators);
                    if (namePart.NamePart.EndsWith(seperator, true, null)
                        || nextPart.NamePart.StartsWith(seperator, true, null))
                        continue;

                    name.Append(seperator);
                }
            }

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.ToString().ToLower());
        }
    }
}
