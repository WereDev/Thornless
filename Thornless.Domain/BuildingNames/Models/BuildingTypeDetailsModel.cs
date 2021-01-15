using Thornless.Domain.Randomization;

namespace Thornless.Domain.BuildingNames.Models
{
    public class BuildingTypeDetailsModel : BuildingTypeModel
    {
        public BuildingNameFormatModel[] NameFormats { get; set; } = new BuildingNameFormatModel[0];

        public class BuildingNameFormatModel : BaseWeightedItemModel
        {
            public string NameFormat { get; set; } = string.Empty;
        }
    }
}
