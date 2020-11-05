using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo.Tests
{
    public class BuildingDataTests
    {
        [Test]
        public async Task CheckBuildingsForValidData()
        {
            var database = new GeneratorContext(new DbContextOptions<GeneratorContext>());

            var nameParts = await database.BuildingNameParts
                                                .ToListAsync();

            foreach (var namePart in nameParts)
                VerifyNamePart(namePart);

            var buildingTypes = await database.BuildingTypes
                                        .Include(x => x.NameFormats)
                                        .ToListAsync();

            var namePartGroups = nameParts.Select(x => x.GroupCode)
                                            .Distinct()
                                            .ToHashSet();

            Assert.AreEqual(buildingTypes.Select(x => x.SortOrder).Distinct().Count(), buildingTypes.Count());

            foreach (var building in buildingTypes)
                VerifyBuildingType(building, namePartGroups);
        }

        private void VerifyNamePart(BuildingNamePartDto namePart)
        {
            Assert.NotNull(namePart.GroupCode);
            Assert.AreEqual(namePart.GroupCode, namePart.GroupCode.ToLower(), "BuildingNamePart Code should be lower case");
            Assert.NotNull(namePart.NamePart);
            Assert.Greater(namePart.RandomizationWeight, 0, "BuildingNamePart RandomizationWeight should be greater than zero.");
        }

        private void VerifyBuildingType(BuildingTypeDto buildingType, HashSet<string> namePartGroups)
        {
            Assert.NotNull(buildingType.Code, "BuildingType Code should not be null");
            Assert.AreEqual(buildingType.Code, buildingType.Code.ToLower(), "BuildingType Code should be lower case");
            Assert.NotNull(buildingType.Name, "BuildingType Name should not be null");
            Assert.NotNull(buildingType.Copyright, "BuildingType Copyright should not be null");
            Assert.DoesNotThrow(() => DateTimeOffset.Parse(buildingType.LastUpdatedDate));

            foreach (var nameFormat in buildingType.NameFormats)
                VerifyNameFormat(nameFormat, namePartGroups);
        }

        private void VerifyNameFormat(BuildingNameFormatDto nameFormat, HashSet<string> namePartGroups)
        {
            Assert.Greater(nameFormat.RandomizationWeight, 0, "BuildingNameFormat RandomizationWeight should be greater than zero.");
            Assert.NotNull(nameFormat.NameFormat, "BuildingNamePart NameFormat should not be null");
            var formatGroups = GetNamePartGroups(nameFormat.NameFormat);
            foreach (var formatGroup in formatGroups)
            {
                Assert.False(string.IsNullOrWhiteSpace(formatGroup), "BuildingNameFormat template specifier should not be empty");
                Assert.True(namePartGroups.Contains(formatGroup), $"BuildingNameFormat template not found in BuildingNameParts: {formatGroup}");
            }
        }

        private string[] GetNamePartGroups(string format)
        {
            List<string> items = new List<string>();

            List<char> chars = new List<char>();
            bool addChars = false;
            foreach (var c in format)
            {
                switch (c)
                {
                    case '{':
                        chars.Clear();
                        addChars = true;
                        break;
                    case '}':
                        addChars = false;
                        var s = new string(chars.ToArray());
                        items.Add(s);
                        break;
                    default:
                        if (addChars)
                            chars.Add(c);
                        break;
                }
            }

            return items.ToArray();
        }
    }
}
