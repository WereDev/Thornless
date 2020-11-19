using Thornless.Domain.Randomization;

namespace Thornless.Domain.CharacterNames.Models
{
    public class AncestryDetailsModel : AncestryModel
    {
        public AncestryOptionsModel[] Options { get; set; } = new AncestryOptionsModel[0];

        public AncestryNamePartModel[] NameParts { get; set; } = new AncestryNamePartModel[0];

        public class AncestryOptionsModel
        {
            public string Code { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;

            public string[] NamePartSeperators { get; set; } = new string[0];

            public int SeperatorChancePercentage { get; set; }

            public int SortOrder { get; set; }

            public AncestrySegmentGroupModel[] SegmentGroups { get; set; } = new AncestrySegmentGroupModel[0];
        }

        public class AncestryNamePartModel : BaseWeightedItemModel
        {
            public string NameSegmentCode { get; set; } = string.Empty;

            public string[] NameParts { get; set; } = new string[0];

            public string[] NameMeanings { get; set; } = new string[0];
        }

        public class AncestrySegmentGroupModel : BaseWeightedItemModel
        {
            public string[] NameSegmentCodes { get; set; } = new string[0];
        }
    }
}
