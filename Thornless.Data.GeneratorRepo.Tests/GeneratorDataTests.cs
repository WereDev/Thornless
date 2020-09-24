using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo.Tests
{
    [TestFixture]
    public class GeneratorDataTests
    {
        [Test]
        public void CheckAncestriesForValidData()
        {
            var database = new GeneratorContext(new DbContextOptions<GeneratorContext>());
            var data = database.CharacterAncestries
                                .Include(x => x.Options)
                                    .ThenInclude(x => x.SegmentGroups)
                                .Include(x => x.NameParts);
            
            foreach (var ancestry in data)
            {
                VerifyCharacterAncestryNameParts(ancestry.NameParts);
                VerifyCharacterAncestryOptions(ancestry.Options);
            }
        }

        private void VerifyCharacterAncestryNameParts(List<CharacterAncestryNamePartDto> nameParts)
        {
            foreach (var namePart in nameParts)
            {
                Assert.IsTrue(IsValidJson(namePart.NamePartsJson), $"Invalid NamePartsJson in CharacterAncestryNamePart {namePart.Id}.");
                if (!string.IsNullOrWhiteSpace(namePart.NameMeaningsJson))
                    Assert.IsTrue(IsValidJson(namePart.NameMeaningsJson), $"Invalid NameMeaningsJson in CharacterAncestryNamePart {namePart.Id}.");
            }
        }

        private void VerifyCharacterAncestryOptions(List<CharacterAncestryOptionDto> options)
        {
            Assert.AreEqual(options.Count(), options.Select(x => x.Code).Distinct().Count(), $"Duplicate CharacterAncestryOption.Code for CharacterAncestry {options[0].CharacterAncestryId}.");
            foreach (var option in options)
            {
                Assert.IsTrue(IsValidJson(option.NamePartSeperatorJson), $"Invalid NamePartSeperatorJson in CharacterAncestryOption {option.Id}");
                Assert.GreaterOrEqual(option.SeperatorChancePercentage, 0, $"Invalid SeperatorChancePercentage in CharacterAncestryOption {option.Id}");
                Assert.LessOrEqual(option.SeperatorChancePercentage, 100, $"Invalid SeperatorChancePercentage in CharacterAncestryOption {option.Id}");
                VerifyCharacterAncestrySegmentGroups(option.SegmentGroups);
            }
        }

        private void VerifyCharacterAncestrySegmentGroups(List<CharacterAncestrySegmentGroupDto> groups)
        {
            foreach (var group in groups)
            {
                Assert.IsTrue(IsValidJson(group.NameSegmentCodesJson), $"Invalid NameSegmentCodesJson in CharacterAncestrySegment {group.Id}.");
            }
        }

        private bool IsValidJson(string value)
        {
            try
            {
                ParseJsonArray(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string[] ParseJsonArray(string s)
        {
            return JsonSerializer.Deserialize<string[]>(s, null);
        }
    }
}
