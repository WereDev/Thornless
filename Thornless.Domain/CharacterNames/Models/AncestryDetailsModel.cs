using Thornless.Domain.Randomization;

namespace Thornless.Domain.CharacterNames.Models
{
    public class AncestryDetailsModel : AncestryModel
    {
        public AncestryOptionsModel[] Options { get; set; }

        public AncestryNamePartModel[] NameParts { get; set; }

        public class AncestryOptionsModel
        {
            public string Code { get; set; }

            public string Name { get; set; }

            public string[] NamePartSeperators { get; set; }

            public int SeperatorChancePercentage { get; set; }

            public int SortOrder { get; set; }

            public AncestrySegmentGroupModel[] SegmentGroups { get; set; }
        }

        public class AncestryNamePartModel : BaseWeightedItemModel
        {
            public string NameSegmentCode { get; set; }

            public string[] NameParts { get; set; }

            public string[] NameMeanings { get; set; }
        }

        public class AncestrySegmentGroupModel : BaseWeightedItemModel
        {
            public string[] NameSegmentCodes { get; set; }
        }
    }
}
