using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.Randomization;
using Thornless.Domain.Templates;

namespace Thornless.Domain.BuildingNames
{
    public class BuildingNameGenerator : IBuildingNameGenerator
    {
        private readonly IBuildingNameRepository _repo;
        private readonly IRandomItemSelector _randomItemSelector;
        private readonly IMemoryCache _memoryCache;

        public BuildingNameGenerator(IBuildingNameRepository repo, IRandomItemSelector randomItemSelector, IMemoryCache cache)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _randomItemSelector = randomItemSelector ?? throw new ArgumentNullException(nameof(randomItemSelector));
            _memoryCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<BuildingNameResultModel> GenerateBuildingName(string buildingTypeCode)
        {
            var buildingType = await _repo.GetBuildingType(buildingTypeCode);
            if (buildingType == null)
                return null;

            var nameFormat = _randomItemSelector.GetRandomWeightedItem(buildingType.NameFormats);
            var nameGroups = await GetBuildingNameGroups();
            var buildingName = GenerateBuildingNameFromFormat(nameFormat.NameFormat, nameGroups);

            return new BuildingNameResultModel
            {
                BuildingName = buildingName,
                BuildingTypeCode = buildingTypeCode,
                BuildingTypeName = buildingType.Name,
            };
        }

        public async Task<BuildingTypeModel[]> ListBuildingTypes()
        {
            return await _repo.ListBuildingTypes();
        }

        private string GenerateBuildingNameFromFormat(string nameFormat, BuildingNameGroups nameGroups)
        {
            var template = new TemplateString(nameFormat);

            var results = new HashSet<string>();

            foreach (var field in template.TemplateFields)
            {
                var nameGroup = nameGroups.NameParts[field.FieldName];

                var nameResult = _randomItemSelector.GetRandomWeightedItem(nameGroup);
                while (results.Contains(nameResult.NamePart))
                    nameResult = _randomItemSelector.GetRandomWeightedItem(nameGroup);
                results.Add(nameResult.NamePart);

                var regex = new Regex(Regex.Escape(field.FieldTemplate));
                nameFormat = regex.Replace(nameFormat, nameResult.NamePart, 1);
            }

            // There are template results that then reference another template.
            if (nameFormat.Contains("{"))
            {
                nameFormat = GenerateBuildingNameFromFormat(nameFormat, nameGroups);
            }

            return nameFormat;
        }

        private async Task<BuildingNameGroups> GetBuildingNameGroups()
        {
            return await _memoryCache.GetOrCreateAsync(nameof(BuildingNameGroups), nameGroup =>
            {
                return _repo.GetBuildingNameGroups();
            });
        }
    }
}
