using System.Collections.Generic;
using Thornless.Domain.Randomization;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingNameGroups
    {
        public Dictionary<string, NamePartModel[]> NameParts { get; set; }

        public class NamePartModel : BaseWeightedItemModel
        {
            public string NamePart { get; set; }
        }
    }
}
