using System.Collections.Generic;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingNameGroups
    {
        public Dictionary<string, NamePartModel[]> NameParts { get; set; } = new Dictionary<string, NamePartModel[]>();

        public class NamePartModel : BaseWeightedItemModel
        {
            public string NamePart { get; set; } = string.Empty;
        }
    }
}
