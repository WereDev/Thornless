using Thornless.Domain.Randomization;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingTypeDetailsModel : BuildingTypeModel
    {
        public BuildingNameFormatModel[] NameFormats { get; set; }

        public class BuildingNameFormatModel : BaseWeightedItemModel
        {
            public string NameFormat { get; set; }

            public string[] NameFormatParts { get; set; }
        }
    }
}
